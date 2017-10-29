using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bittrex
{
    public class OpenOrder
    {
        [Key]
        [Column(Order = 1)]
        public string Uuid{get;set;}
        [Key]
        [Column(Order = 2)]
        public string OrderUuid{get;set;}
		public string Exchange{get;set;}
		public OpenOrderType OrderType{get;set;}
		public decimal Quantity{get;set;}
		public decimal QuantityRemaining{get;set;}
		public decimal Limit{get;set;}
		public decimal CommissionPaid{get;set;}
		public decimal Price{get;set;}
        //public decimal? PricePerUnit{get;set;}
		public DateTime Opened{get;set;}
		//public string Closed" : null,
		public bool CancelInitiated{get;set;}
		public bool ImmediateOrCancel{get;set;}
		public bool IsConditional{get;set;}
		public string Condition{get;set;}
        public string ConditionTarget { get; set; }
        public bool EseguitoDalBot { get; set; }
    }
}
