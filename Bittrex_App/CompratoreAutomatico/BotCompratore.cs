using BitRexSql.Repo.RepoAndUnitOfWork.Domain.Concrete;
using Bittrex_App.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public BotCompratore(Manager manager)
        {
            Manager = manager;
        }

        internal void Avvia()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var lastDate = unitOfWork.MarketSummaryResponseRepository.Find(a => a.MarketName.StartsWith("BTC")).
                    Max(a => a.TimeStamp).Subtract(new TimeSpan(0,0,15,0,0));
                
                var subset = unitOfWork.MarketSummaryResponseRepository.Find(a => a.MarketName.StartsWith("BTC"))
                    .Where(a => a.TimeStamp >= lastDate)
                    .ToList();

                
                foreach (var itemGr in subset.GroupBy(a => a.MarketName))
                {

                    var elementiPerMoneta = itemGr.Where(a => a.MarketName == itemGr.Key).ToList();
                    if (elementiPerMoneta.Count() < 4)
                    { 
                        continue;
                    }
                    var ultimo = elementiPerMoneta.OrderByDescending(a => a.TimeStamp).ToList().First();
                    var precedente = elementiPerMoneta.OrderByDescending(a => a.TimeStamp).ToList().Last();

                    //se la quotazione è aumentata
                    if (ultimo.Last>precedente.Last
                        //&&
                        //ultimo.Volume+2> precedente.Volume
                        )
                    {

                        FaiOrdineAcqusto(ultimo.MarketName);
                    }
                    else
                    {

                    }
                } 
            }
        }
        
        private void FaiOrdineAcqusto(string moneta)
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
                var valoreAttuale = Manager.Exchange.GetTicker( moneta).Last;
                Manager.Log("Il valore attuale moneta " + moneta + " è di " + valoreAttuale + " BTC", LogType.ReadDataFromInternet);

                var qta = Math.Round( btcAvaliable/valoreAttuale,0,MidpointRounding.ToEven)-2 ;
                var prezzo = valoreAttuale * 0.99M;
                Manager.Log(string.Format("Emissione ordine acquisto di moneta {0} al varore {1} rispetto all'attuale di {2} per la qta di {3}",
                    moneta, valoreAttuale, qta, prezzo), LogType.Buy);

                var ordine = Manager.Exchange.PlaceBuyOrder(moneta, qta, prezzo);
                if (ordine.uuid != null)
                {
                    Manager.Log("Ordine creato con successo " + ordine.uuid.ToString(), LogType.Buy);
                }
            }
            catch (Exception ex)
            {
                Manager.Log(ex);
            }
        }
    }
}
