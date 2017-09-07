using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bittrex;

namespace Bittrex_App.State
{
    public class MemoDati
    {
        
        public List<AccountBalance> Bilancio { get; set; }
    }
    public class StatoConto: AccountBalance
    {
        public decimal ActualValue { get; set; }

    }
}
