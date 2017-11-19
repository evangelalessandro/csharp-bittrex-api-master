using BitRexSql.Entity;
using BitRexSql.Repo.RepoAndUnitOfWork.Domain.Concrete;
using Bittrex_App.Componenti;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Bittrex_App
{
    internal partial class frmEditRules : Form
    {
        Manager _manager = null;
        List<RulesBuySell> _data = null;
        gridViewUsercontrol _udcGrid;
        public frmEditRules(Manager manager)
        {
            InitializeComponent();
            _manager = manager;
        }

        private void frmEditRules_Load(object sender, EventArgs e)
        {
            _udcGrid = new gridViewUsercontrol();
            this.Controls.Add(_udcGrid);
            _udcGrid.Dock = DockStyle.Top;
            _udcGrid.Height = btnAggiorna.Top - 150;
            _udcGrid.Top = 100;
            Aggiorna();   
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            using (var dat = new UnitOfWork())
            {
                dat.RulesRepository.Add(new RulesBuySell());
                dat.Commit();
                Aggiorna();
            }
        }

        private void btnSalva_Click(object sender, EventArgs e)
        {
            using (var dat = new UnitOfWork())
            {

                foreach (var item in _data)
                {
                    var itemToUpdate = dat.RulesRepository.Find(a => a.ID == item.ID).FirstOrDefault();
                    if (itemToUpdate != null)
                    {
                        var props = item.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                        foreach (var itemPr in props.Where(a => a.Name != "ID"))
                        {
                            itemPr.SetValue(itemToUpdate, itemPr.GetValue(item));
                        }
                        dat.Commit();
                    }
                }
            }
        }

        private void btnElimina_Click(object sender, EventArgs e)
        {
            using (var dat = new UnitOfWork())
            {
                var itemToDelete = dat.RulesRepository.Find(a => a.ID == ((RulesBuySell)_udcGrid.CurrentRow).ID).FirstOrDefault();
                if (itemToDelete != null)
                {
                    dat.RulesRepository.Delete(itemToDelete);
                    dat.Commit();
                    Aggiorna();
                }
            }
        }

        private void btnAggiorna_Click(object sender, EventArgs e)
        {
            Aggiorna();
        }
        private void Aggiorna()
        {
            using (var dat = new UnitOfWork())
            {
                _data = dat.RulesRepository.Find(a => 1 == 1).ToList();
                _udcGrid.SetDatasource(_data);
            } 

        }
    }
}
