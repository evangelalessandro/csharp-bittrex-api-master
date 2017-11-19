using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bittrex_App.Componenti
{
    public partial class gridViewUsercontrol : UserControl
    {
        public gridViewUsercontrol()
        {
            InitializeComponent();
        }
        /// <summary>
        /// riga corrente
        /// </summary>
        public object CurrentRow
        {
            get
            {
                return gvRules.CurrentRow.DataBoundItem;

            }
        }
        private bool _renamedRow;
        public void SetDatasource(object obj)
        {
            gvRules.DataSource = obj;
            gvRules.Refresh();
            if (!_renamedRow)
            {
                gvRules.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
                foreach (DataGridViewColumn item in gvRules.Columns)
                {
                    if (item.HeaderText.Length < 3)
                        continue;

                    foreach (var itemName in item.HeaderText.Where(a => a.ToString() == a.ToString().ToUpperInvariant()).Distinct())
                    {
                        item.HeaderText = item.HeaderText.Replace(itemName.ToString(), " " + itemName.ToString()).Trim();

                    }
                }
                _renamedRow = true;
            }
        }
        private void Aggiorna()
        {

            gvRules.Refresh();
        }
        private void gridView_Load(object sender, EventArgs e)
        {

        }
    }
}
