using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bittrex;

namespace Bittrex_App
{
    public partial class Form1 : Form
    {

        ExchangeContext _context = new ExchangeContext();
        Exchange _exchange= new Exchange();
        State.MemoDati _memo = new State.MemoDati();
        public Form1()
        {
            InitializeComponent();
            _context.ApiKey= "22f68f6b120248adb21e9de867a87841";
            _context.Secret= "79c2d7f991674c9b8c7962acdde30f44";
            _context.QuoteCurrency = ",";
            _context.Simulate = true;
            _exchange.Initialise(_context);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _memo.Bilancio= _exchange.GetBalances();

            gvBalance.DataSource = _memo.Bilancio.ToList().Where(a => a.Available != 0 || a.Pending != 0 || a.Balance!=0).Select(a => new { a.Currency, a.Available, a.Pending, a.Requested, a.Balance }).ToList();

            gvBalance.Refresh();


            //prendere i valori e le quantità delle azioni che hai per vedere il valore medio e il valore attuale della perdita\guadagno

        }
    }
}
