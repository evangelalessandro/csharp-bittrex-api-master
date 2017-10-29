using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRexSql.Entity
{
    public class EventLog
    {
        [Key]
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public string Evento { get; set; }
        public string TipoEvento { get; set; }
        public string Errore { get; set; }
        public string StackTrace { get; set; }
        public string InnerException { get; set; }
    }
}
