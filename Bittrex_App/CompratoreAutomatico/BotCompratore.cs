using BitRexSql.Entity;
using BitRexSql.Repo.RepoAndUnitOfWork.Domain.Concrete;
using Bittrex_App.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bittrex;

namespace Bittrex_App.CompratoreAutomatico
{

    /// <summary>
    /// deve controllare ogni 15 secondi e vedere se il prezzo aumenta lo mantiene, se scende il prezzo vende...
    /// 
    /// </summary>
    internal class BotCompratore
    {
        internal Manager Manager { get; set; }

        internal BotCompratore(Manager manager)
        {
            Manager = manager;
            Acquisti.Instance.Initialize(Manager);
            Vendita.Instance.Initialize(Manager);
        }

        public Vendita VenditaController { get { return Vendita.Instance; } }
        public Acquisti AcquistiController { get { return Acquisti.Instance; } }


        internal class Acquisti
        {
            private Manager Manager { get; set; }

            private Task TaskAcquisto { get; set; }

            private static Acquisti instance;


            internal void Initialize(Manager manager)
            {
                Manager = manager;
            }
            public static Acquisti Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new Acquisti();
                    }
                    return instance;
                }
            }


            public Acquisti()
            {
            }

            public void Attiva(bool cancel)
            {
                TaskAcquisto = BotAcquisto(new System.Threading.CancellationToken(cancel));
            }

            private async Task BotAcquisto(CancellationToken token = default(CancellationToken))
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
                Manager.Log("Bot Acquisto in azione", LogType.Information);

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
                            Manager.Log("Trend non in salita, non si acquista!", LogType.Information);
                        }
                    }
                }
            }

            private bool FaiOrdineAcqusto(string moneta, RulesBuySell regola)
            {
                try
                {

                    Manager.Log("Tentativo di acquisto " + moneta, LogType.Affermation);

                    var btcAvaliable = Manager.Exchange.GetBalance("BTC").Available;
                    if (btcAvaliable < 0.0005m)
                    {
                        Manager.Log("Fondi insufficienti per comprare " + moneta, LogType.Warning);
                        return false;
                    }
                    Manager.Log(string.Format("Disponibili {0} BTC", btcAvaliable)
                        , LogType.Information);

                    var valoreAttuale = Manager.Exchange.GetTicker(moneta).Last;
                    Manager.Log("Il valore attuale moneta " + moneta + " è di " + valoreAttuale + " BTC", LogType.ReadDataFromInternet);

                    var importobtcAcq = btcAvaliable;
                    if (regola.AcquistaImportoMaxBtc != 0)
                    {
                        if (btcAvaliable > regola.AcquistaImportoMaxBtc)
                        {
                            importobtcAcq = regola.AcquistaImportoMaxBtc;
                            Manager.Log(string.Format("Verrà acquistata la moneta " + moneta + " per una spesa massima di {0} BTC", importobtcAcq)
                                , LogType.ReadDataFromInternet);

                        }
                    }


                    var qta = Math.Round(importobtcAcq / valoreAttuale,4) ;
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
                                //se è stata acquistato va alla vendita
                                Vendita.Instance.Attiva(false);
                                return true;
                            }
                            else
                            {
                                Manager.Exchange.CancelOrder(ordine.uuid);
                                return false;
                            }


                        }

                    }
                }
                catch (Exception ex)
                {
                    Manager.Log(ex);
                }

                return false;
            }
        }
        internal class Vendita
        {
            public void Attiva(bool cancel)
            {
                TaskVendita = BotControlloPerVendereOrdineAcqBot(
                    new System.Threading.CancellationToken(cancel));
            }
            private static Vendita instance;

            public static Vendita Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new Vendita();
                    }
                    return instance;
                }
            }

            private Task TaskVendita { get; set; }

            internal Manager Manager { get; set; }

            internal void Initialize(Manager manager)
            {
                Manager = manager;
            }
            private async Task BotControlloPerVendereOrdineAcqBot(CancellationToken token = default(CancellationToken))
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
                try
                {

                    using (UnitOfWork unitOfWork = new UnitOfWork())
                    {
                        var regole = unitOfWork.RulesRepository.Find(a => 1 == 1).ToList();
                        if (regole.Count == 0)
                        {
                            Manager.Log("Impostare regole nel db per acquisti e vendite", LogType.Affermation);
                            return;
                        }

                        var subset = unitOfWork.CompletedOrderRepository.Find(a => 1 == 1).ToList();

                        //se non contiene la regola per tutte allora filtro le monete vendibili
                        if (regole.Where(a => a.Exchange == "*").Count() == 0)
                        {
                            subset = subset.FindAll(a => regole.Select(b => b.Exchange).ToList().Contains(a.Exchange));
                        }
                        foreach (var itemGr in subset.GroupBy(a => a.Exchange))
                        {
                            var regola = regole.Where(a => itemGr.Key == a.Exchange).FirstOrDefault();

                            if (regola == null)
                                regola = regole.Where(a => a.Exchange == "*").FirstOrDefault();
                            foreach (var item in itemGr.ToList())
                            {

                                VerificaVendita(regola, item);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Manager.Log(ex);
                }
            }
            /// <summary>
            /// attiva la vendita del ordine di acquisto effettuato
            /// </summary>
            /// <param name="regola"></param>
            /// <param name="order"></param>
            /// <param name="token"></param>
            /// <returns></returns>
            public async Task VerificaVenditaTask(RulesBuySell regola, CompletedOrder order, CancellationToken token = default(CancellationToken))
            {
                while (!token.IsCancellationRequested)
                {

                    if (this.VerificaVendita(regola, order))
                    {
                        //se ha venduto termina il task
                        break;
                    }
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

            private bool VerificaVendita(RulesBuySell regola, CompletedOrder order)
            {
                try
                {
                    var valoreAttuale = Manager.Exchange.GetTicker(order.Exchange).Ask;

                    Manager.Log("Il valore attuale moneta " + order.Exchange + " è di " + valoreAttuale + " BTC", LogType.ReadDataFromInternet);

                    if (regola.VendiSeAumentaDiXPercento > 0)
                    {
                        if (valoreAttuale > order.PricePerUnit * (regola.VendiSeAumentaDiXPercento + 100) / 100)
                        {
                            Manager.Log("Raggiunto aumento percentuale di " + regola.VendiSeAumentaDiXPercento
                                + " per moneta " + order.Exchange, LogType.Sell);

                            var orderSell = Manager.Exchange.PlaceSellOrder(order.Exchange, order.Quantity, valoreAttuale);

                            var OrderSelList = Manager.Exchange.GetOrderHistory(order.Exchange);
                            var orderFound = OrderSelList.Where(a => a.OrderUuid == orderSell.uuid).FirstOrDefault();
                            if (orderFound != null)
                            {
                                Manager.Log("Vendita completata dell'ordine " + orderFound.OrderUuid + " per moneta " + order.Exchange, LogType.Sell);
                                return true;
                            }
                            else
                            {
                                Manager.Exchange.CancelOrder(orderSell.uuid);
                                return false;
                            }
                        }
                    }
                    if (regola.VendiSeScendeDiXPercento > 0)
                    {
                        if (valoreAttuale > order.PricePerUnit * (100 - regola.VendiSeScendeDiXPercento) / 100)
                        {
                            Manager.Log("Raggiunto aumento percentuale di " + regola.VendiSeScendeDiXPercento
                                + " per moneta " + order.Exchange, LogType.Sell);

                            var orderSell = Manager.Exchange.PlaceSellOrder(order.Exchange, order.Quantity, valoreAttuale);

                            var OrderSelList = Manager.Exchange.GetOrderHistory(order.Exchange);
                            var orderFound = OrderSelList.Where(a => a.OrderUuid == orderSell.uuid).FirstOrDefault();
                            if (orderFound != null)
                            {
                                Manager.Log("Vendita completata dell'ordine " + orderFound.OrderUuid + " per moneta " + order.Exchange, LogType.Sell);
                                return true;
                            }
                            else
                            {
                                Manager.Exchange.CancelOrder(orderSell.uuid);
                                return false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Manager.Log(ex);
                }
                return false;
            }

        }
    }
}
