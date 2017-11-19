using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRexSql.Entity
{
    public class RulesBuySell
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; } 
        public string Exchange { get; set; }
        public decimal VendiSeScendeDiXPercento { get; set; }
        public decimal VendiSeAumentaDiXPercento { get; set; }
        public decimal AcquistaValoreMassimoUnitario { get; set; }
        public int AcquistaSeAumentatoVolumeDi { get; set; }
        public decimal AcquistaImportoMaxBtc { get; set; }

        public decimal AcquistaAlValoreDiLastPiuPercentuale { get; set; }
    }
}
