﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRexSql.Entity
{
    public class EventLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; } 

        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public string Evento { get; set; }
        public string TipoEvento { get; set; }
        public string Errore { get; set; }
        public string StackTrace { get; set; }
        public string InnerException { get; set; }
        public string Class { get; set; }
        public int ThreadId { get; set; }

    }
}
