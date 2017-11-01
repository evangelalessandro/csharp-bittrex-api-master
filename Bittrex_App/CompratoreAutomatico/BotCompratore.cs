using BitRexSql.Entity;
using BitRexSql.Repo.RepoAndUnitOfWork.Domain.Concrete;
using Bittrex_App.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bittrex_App.CompratoreAutomatico
{

    /// <summary>
    /// deve controllare ogni 15 secondi e vedere se il prezzo aumenta lo mantiene, se scende il prezzo vende...
    /// 
    /// </summary>
    internal class BotCompratore
    {
        internal Manager Manager { get; set; }
        public Task TaskAcquisto { get; internal set; }
        public Task TaskVendita { get; internal set; }

        public BotCompratore(Manager manager)
        {
            Manager = manager;
        }
        public async Task BotAcquisto(CancellationToken token = default(CancellationToken))
        {
            while (!token.IsCancellationRequested)
            {
                this.Acquista();
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(10), token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }
        private void Acquista()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var regole = unitOfWork.RulesRepository.Find(a => 1 == 1).ToList();
                if (regole.Count == 0)
                {
                    Manager.Log("Impostare regole nel db per acquisti e vendite", LogType.Affermation);
                    return;
                }

                var lastDate = unitOfWork.MarketSummaryResponseRepository.Find(a => a.MarketName.StartsWith("BTC")).
                    Max(a => a.TimeStamp).Subtract(new TimeSpan(0, 0, 15, 0, 0));

                var subset = unitOfWork.MarketSummaryResponseRepository.Find(a => a.MarketName.StartsWith("BTC"))
                    .Where(a => a.TimeStamp >= lastDate)
                    .ToList();

                //se non contiene la regola per tutte allora filtro le monete acquistabili
                if (regole.Where(a => a.Exchange == "*").Count() == 0)
                {
                    subset = subset.FindAll(a => regole.Select(b => b.Exchange).ToList().Contains(a.MarketName));
                }
                foreach (var itemGr in subset.GroupBy(a => a.MarketName))
                {
                    //a.Exchange=="*" || 
                    var regola = regole.Where(a => itemGr.Key == a.Exchange).FirstOrDefault();

                    if (regola == null)
                        regola = regole.Where(a => a.Exchange == "*").FirstOrDefault();


                    var elementiPerMoneta = itemGr.Where(a => a.MarketName == itemGr.Key).ToList();
                    if (elementiPerMoneta.Count() < 4)
                    {
                        continue;
                    }
                    var ultimo = elementiPerMoneta.OrderByDescending(a => a.TimeStamp).ToList().First();
                    var precedente = elementiPerMoneta.OrderByDescending(a => a.TimeStamp).ToList().Last();

                    //se la quotazione è aumentata
                    if (ultimo.Last > precedente.Last
                        //&&
                        //ultimo.Volume+2> precedente.Volume
                        )
                    {
                        if ((regola.AcquistaSeAumentatoVolumeDi != 0 &&
                            ultimo.Volume + regola.AcquistaSeAumentatoVolumeDi > precedente.Volume) ||
                            regola.AcquistaSeAumentatoVolumeDi == 0)
                        {
                            if (regola.AcquistaSeAumentatoVolumeDi != 0)
                            {
                                Manager.Log(string.Format("Acquisto per aumento volume da {0} a {1} con aumento di almeno {2}",
                                    precedente.Volume,
                                    ultimo.Volume,
                                    regola.AcquistaSeAumentatoVolumeDi), LogType.Information);
                            }
                            else
                            {
                                Manager.Log(string.Format("Acquisto senza regola volume"), LogType.Information);
                            }
                            FaiOrdineAcqusto(ultimo.MarketName, regola);
                        }
                    }
                    else
                    {

                    }
                }
            }
        }

        private void FaiOrdineAcqusto(string moneta, RulesBuySell regola)
        {
            try
            {

                Manager.Log("Tentativo di acquisto " + moneta, LogType.Affermation);

                var btcAvaliable = Manager.Exchange.GetBalance("BTC").Available;
                if (btcAvaliable < 0.0005m)
                {
                    Manager.Log("Fondi insufficienti per comprare " + moneta, LogType.Warning);
                    return;
                }
                Manager.Log(string.Format( "Disponibili {0} BTC",btcAvaliable)
                    , LogType.Information);

                var valoreAttuale = Manager.Exchange.GetTicker(moneta).Last;
                Manager.Log("Il valore attuale moneta " + moneta + " è di " + valoreAttuale + " BTC", LogType.ReadDataFromInternet);

                var importobtcAcq = btcAvaliable;
                if (regola.AcquistaImportoMaxBtc!=0)
                {
                    if (btcAvaliable> regola.AcquistaImportoMaxBtc)
                    { 
                        importobtcAcq = regola.AcquistaImportoMaxBtc;
                        Manager.Log(string.Format("Verrà acquistata la moneta " + moneta + " per una spesa massima di {0} BTC", importobtcAcq)
                            , LogType.ReadDataFromInternet);

                    }
                }


                var qta = Math.Round(btcAvaliable / valoreAttuale, 0, MidpointRounding.ToEven) - 2;
                var prezzo = valoreAttuale;
                if (regola.AcquistaAlValoreDiLastPiuPercentuale == 0)
                {
                    prezzo = prezzo * (100 + regola.AcquistaAlValoreDiLastPiuPercentuale) / 100;
                }

                Manager.Log(string.Format("Emissione ordine acquisto di moneta {0} al varore {1} rispetto all'attuale di {2} per la qta di {3}",
                    moneta, valoreAttuale, qta, prezzo), LogType.Buy);

                var ordine = Manager.Exchange.PlaceBuyOrder(moneta, qta, prezzo);
                if (ordine.uuid != null)
                {

                    Manager.Log("Ordine creato con successo " + ordine.uuid.ToString(), LogType.Buy);

                    using (UnitOfWork unitOfWork = new UnitOfWork())
                    {
                        unitOfWork.OrdiniDelBotRepository.Add(new OrdiniDelBot() { Uuid = ordine.uuid });
                        unitOfWork.Commit();

                        Manager.AggiornaOrdini();
                        var ordineChiuso = Manager.Db.OrderHistory.Where(a => a.OrderUuid == ordine.uuid).ToList();
                        if (ordineChiuso != null)
                        {
                            if (TaskVendita.Status!=TaskStatus.Running)
                            {
                                var task = BotControlloPerVendereOrdineAcqBot(new System.Threading.CancellationToken());
                                TaskVendita = task;
                            }
                            
                        }
                        else
                        {
                            ControlloOrdiniIncompleti();
                        }


                    }

                }
            }
            catch (Exception ex)
            {
                Manager.Log(ex);
            }
        }

        private void ControlloOrdiniIncompleti()
        {

        }
        public async Task BotControlloPerVendereOrdineAcqBot(CancellationToken token = default(CancellationToken))
        {
            while (!token.IsCancellationRequested)
            {
                this.ControlloPerVendereOrdineAcqBotCiclo();
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(10), token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }

        }

        private void ControlloPerVendereOrdineAcqBotCiclo()
        {
            
        }
    }
}
