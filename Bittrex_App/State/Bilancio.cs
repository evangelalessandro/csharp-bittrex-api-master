using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bittrex;
using Bittrex_App.Classes;

namespace Bittrex_App.State
{
    public class MemoDati
    {
        public MemoDati()
        {

        }
 
        public List<StatoConto> StatoConto { get; set; } = new List<State.StatoConto>();

        public List<AccountBalance> Bilancio { get; set; } = new List<AccountBalance>();

        public GetOpenOrdersResponse  OpenOrder { get; set; } = new GetOpenOrdersResponse();
        public GetOrderHistoryResponse OrderHistory { get; internal set; }
        public List<GetMarketSummaryResponse> StorioMercato { get; set; }
    }
    public class StatoConto: AccountBalance
    {
        /// <summary>
        /// Valore attuale
        /// </summary>
        public decimal ActualValue { get; set; }

        /// <summary>
        /// il valore medio delle azioni rimanenti
        /// </summary>
        public decimal CostoDiVenditaMinimoPerNonPerdere{ get; set; }


    }
}
