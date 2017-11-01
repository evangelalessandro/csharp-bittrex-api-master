using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRexSql.Entity
{
    public class OrdiniDelBot
    {
        [Key]
        public string Uuid { get; set; }
        public DateTime DateTimeStamp { get; set; } = DateTime.Now;
    }
}
