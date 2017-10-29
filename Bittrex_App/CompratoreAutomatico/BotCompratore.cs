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

    }
}
