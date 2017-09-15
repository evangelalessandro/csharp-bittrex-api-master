using Bittrex;
using Bittrex.Data;
using Bittrex_App.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bittrex_App
{
    class Manager
    {
        ExchangeContext _context = new ExchangeContext();
        Exchange _exchange = new Exchange();
        public State.MemoDati Db { get; set; } = new State.MemoDati();

        public Manager()
        {
            var chiavi = File.ReadLines(Environment.CurrentDirectory + @"\key.txt");
            //SecretKey=asfasfsaaaa
            //ApiKey = asfasfd

            _context.ApiKey = chiavi.Where(a => a.StartsWith("ApiKey", StringComparison.InvariantCultureIgnoreCase)).Select(a => a.Substring(a.IndexOf("=") + 1).Trim()).First();
            _context.Secret = chiavi.Where(a => a.StartsWith("SecretKey", StringComparison.InvariantCultureIgnoreCase)).Select(a => a.Substring(a.IndexOf("=") + 1).Trim()).First(); ;
            _context.QuoteCurrency = ",";
            _context.Simulate = false;
            _exchange.Initialise(_context);

        }
        public void AggiornaTutto()
        {
            AggiornaBilancio();
            AggiornaOrdini();

        }
        public void AggiornaBilancio()
        {
            Db.Bilancio = _exchange.GetBalances().Where(a => a.Available != 0 || a.Pending != 0 || a.Balance != 0).ToList();

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
                        var dati = _exchange.GetTicker("BTC-" + item.Currency);

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
                        AddLog(ex);
                    }
                }
            }
            Db.StatoConto = conti;
        }
        public void AggiornaOrdini()
        {
            Db.OpenOrder = _exchange.GetOpenOrders();
            Db.OrderHistory = _exchange.GetOrderHistory("");


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
        private void AddLog(Exception ex)
        {
            Db.Errori.Add(new ErrorItem(ex.Message));
        }

    }
}
