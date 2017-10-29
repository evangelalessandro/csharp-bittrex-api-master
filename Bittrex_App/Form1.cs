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
using Bittrex_App.Classes;
using Bittrex.Data;

namespace Bittrex_App
{
    public partial class Form1 : Form
    {
        Manager _manager = new Manager();

        public Form1()
        {
            InitializeComponent();
        }

        private void RefreshError()
        {
            gvErrori.DataSource = _manager.Db.Errori;
            gvErrori.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            AggiornaBilancio();
            RefreshConti();

        }

        private void RefreshConti()
        {
            gvStato.DataSource = _manager.Db.StatoConto;
            gvStato.Refresh();
        }

        private void AggiornaBilancio()
        {
            _manager.AggiornaTutto();
            gvBalance.DataSource = _manager.Db.Bilancio.Select(a => new { a.Currency, a.Available, a.Pending, a.Requested, a.Balance }).ToList();
            gvBalance.Refresh();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            _manager.AggiornaSommarioMarket();

        }
    }
}
