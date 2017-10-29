using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bittrex_App.Classes
{
    public class ErrorItem
    {
        public ErrorItem(Exception ex)
            :
            this(ex.Message)
        {
        }
        public ErrorItem(string ex)
        {
            Errore = ex;
            Ora = DateTime.Now;
        }
        public string Errore{ get; set; }
        public DateTime Ora { get; set; }
        
    }
}
