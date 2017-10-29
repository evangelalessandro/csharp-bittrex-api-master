using BitRexSql.Repo.RepoAndUnitOfWork.Domain.Concrete;
using Bittrex;
using Bittrex.Data;
using Bittrex_App.Classes;
using Bittrex_App.CompratoreAutomatico;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bittrex_App
{
    class Manager
    {
        ExchangeContext _context = new ExchangeContext();
        public  Exchange Exchange { get; internal set; } = new Exchange();
        public State.MemoDati Db { get; set; } = new State.MemoDati();
        public BotCompratore Bot { get; internal set; }

        public Manager()
        {
            Bot = new BotCompratore(this);
            if (!File.Exists(Environment.CurrentDirectory + @"\key.txt"))
            {

                File.Create(Environment.CurrentDirectory + @"\key.txt").Close();
                File.AppendAllText(Environment.CurrentDirectory + @"\key.txt", "ApiKey=" + Environment.NewLine + "SecretKey=");
                MessageBox.Show("Imposta key e secretKey nel file " + Environment.CurrentDirectory + @"\key.txt");
                return;
            }
            var chiavi = File.ReadLines(Environment.CurrentDirectory + @"\key.txt");

            //0828009968044390b96a3ad3b4d58f6d
            //SecretKey=asfasfsaaaa
            //ApiKey = asfasfd

            _context.ApiKey = chiavi.Where(a => a.StartsWith("ApiKey", StringComparison.InvariantCultureIgnoreCase)).Select(a => a.Substring(a.IndexOf("=") + 1).Trim()).First();
            _context.Secret = chiavi.Where(a => a.StartsWith("SecretKey", StringComparison.InvariantCultureIgnoreCase)).Select(a => a.Substring(a.IndexOf("=") + 1).Trim()).First(); ;
            _context.QuoteCurrency = ",";
            _context.Simulate = false;
            Exchange.Initialise(_context);

        }

        internal void AggiornaSommarioMarket()
        {
            try
            {
                Log("Inizio Aggiornamento Sommario ", LogType.Information);
                var dato = Exchange.GetMarketSummaries();
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    foreach (var item in dato.ToList().Where(a => a.MarketName.StartsWith("BTC")))
                    {
                        if (unitOfWork.MarketSummaryResponseRepository
                            .Find(a => a.MarketName == item.MarketName).Select(a=>a.TimeStamp).ToList().Where(a=>item.TimeStamp == a).Count() == 0)
                        {
                            try
                            {

                                unitOfWork.MarketSummaryResponseRepository.Add(item);
                                unitOfWork.Commit();

                            }
                            catch (Exception ex)
                            {
                                Log(ex);
                            }

                        }
                    }
                }
                Log("Fine Aggiornamento Sommario ", LogType.Information);
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }
        internal void Log(string events, LogType logType)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                unitOfWork.EventLogRepository.Add(new BitRexSql.Entity.EventLog()
                {
                    Errore = events,
                    TipoEvento = logType.ToString(),
                });
                unitOfWork.Commit();
            }
        }
        internal void Log(Exception ex)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                unitOfWork.EventLogRepository.Add(new BitRexSql.Entity.EventLog()
                {
                    Errore = ex.Message,
                    TipoEvento = "Errore",
                    InnerException = ex.InnerException.ToString(),
                    StackTrace = ex.StackTrace,
                    Evento = ""
                });
                unitOfWork.Commit();
            }
        }
        public async Task UpdateMarketHystory(CancellationToken token = default(CancellationToken))
        {
            while (!token.IsCancellationRequested)
            {
                this.AggiornaSommarioMarket();
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(30), token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }
        public void FixEfProviderServicesProblem()
        {
            //The Entity Framework provider type 'System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer'
            //for the 'System.Data.SqlClient' ADO.NET provider could not be loaded. 
            //Make sure the provider assembly is available to the running application. 
            //See http://go.microsoft.com/fwlink/?LinkId=260882 for more information.

            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
        public void AggiornaTutto()
        {
            AggiornaBilancio();
            AggiornaOrdini();

        }
        public void AggiornaBilancio()
        {
            Db.Bilancio = Exchange.GetBalances().Where(a => a.Available != 0 || a.Pending != 0 || a.Balance != 0).ToList();

            var conti = new List<State.StatoConto>();
            //prendere i valori e le quantità delle azioni 
            //che hai per vedere il valore medio e il valore attuale della perdita\guadagno
            //_memo.Bilancio//.ForEach(a=>a.Currency)

            //var keypair=new List<Tuple<string,>>

            foreach (var item in Db.Bilancio)
            {
                if (item.Currency != "BTC")
                {
                    try
                    {
                        var dati = Exchange.GetTicker("BTC-" + item.Currency);

                        conti.Add(new State.StatoConto()
                        {
                            Currency = item.Currency,
                            Balance = item.Balance,
                            ActualValue = dati.Last,
                            Available = item.Available,
                            Pending = item.Pending,
                            Requested = item.Requested,
                            CryptoAddress = item.CryptoAddress
                        });


                    }
                    catch (Exception ex)
                    {
                       Log(ex);
                    }
                }
            }
            Db.StatoConto = conti;
        }
        public void AggiornaOrdini()
        {
            Db.OpenOrder = Exchange.GetOpenOrders();
            Db.OrderHistory = Exchange.GetOrderHistory("");


            foreach (var item in Db.StatoConto)
            {
                var datiXCurrency = Db.OrderHistory.Where(a => a.Exchange == "BTC-" + item.Currency).ToList();
                item.CostoDiVenditaMinimoPerNonPerdere = 0;
                if (datiXCurrency.Count != 0)
                {
                    var datiAcq = datiXCurrency.
                            Where(a => a.OrderType == OpenOrderType.Limit_Buy).ToList();
                    var costoAcquisti = datiAcq.Select(a => a.Price * a.Quantity).Sum();
                    var qtaAcq = datiAcq.Select(a => a.Quantity).Sum();

                    var datiSell = datiXCurrency.
                            Where(a => a.OrderType == OpenOrderType.Limit_Sell).ToList();
                    var costoVendite = datiSell.Select(a => a.Price * a.Quantity).Sum();
                    var qtaSell = datiSell.Select(a => a.Quantity).Sum();

                    item.CostoDiVenditaMinimoPerNonPerdere = (costoAcquisti - costoVendite) / (qtaAcq - qtaSell);
                }
            }
        }
         

    }
}
