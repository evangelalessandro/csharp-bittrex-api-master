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
using BitRexSql.Repo.RepoAndUnitOfWork.Domain.Concrete;
using System.Threading;

namespace Bittrex_App
{
    public partial class Form1 : Form
    {
        Manager _manager = new Manager();



        public Form1()
        {
            InitializeComponent();

            Task ts = new Task(ScaricaAggiornamentiStatoListino);
            ts.Start();

            AggiornaLabel();
        }

        private void RefreshError()
        {
            if (this.InvokeRequired)
            {
                var inv = new MethodInvoker(RefreshError);
                inv.Invoke();
                return;
            }

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var dati = unitOfWork.EventLogRepository.Find(a => 1 == 1).Select(a =>
                          new { a.Evento, a.TipoEvento, a.TimeStamp, a.InnerException, a.StackTrace })
                    .OrderByDescending(A => A.TimeStamp).Take(1000).ToList();

                gvErrori.DataSource = dati;
                gvErrori.Refresh();

            }
        }


        private void RefreshConti()
        {
            if (this.InvokeRequired)
            {
                var inv = new MethodInvoker(RefreshConti);
                inv.Invoke();
                return;
            }
            gvStato.DataSource = _manager.Db.StatoConto;
            gvStato.Refresh();
        }

        private void AggiornaBilancio()
        {
            if (this.InvokeRequired)
            {
                var inv = new MethodInvoker(AggiornaBilancio);
                inv.Invoke();
                return;
            }
            _manager.AggiornaTutto();
            gvBalance.DataSource = _manager.Db.Bilancio.Select(a => new { a.Currency, a.Available, a.Pending, a.Requested, a.Balance }).ToList();
            gvBalance.Refresh();

        }

        private bool _attivaAggiornamento;
        private void button3_Click(object sender, EventArgs e)
        {
            Task ts = new Task(ScaricaAggiornamentiStatoListino);
            ts.Start();
        }
        private async Task AggiornaTutto(CancellationToken token = default(CancellationToken))
        {
            while (!token.IsCancellationRequested)
            {
                this.RefreshError();
                AggiornaBilancio();
                RefreshConti();
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(30), token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }
        private async void ScaricaAggiornamentiStatoListino()
        {
            _attivaAggiornamento = !_attivaAggiornamento;
            AggiornaLabel();
            if (_attivaAggiornamento)
            {
                _manager.UpdateMarketHystory();

            }
            else
            {
                _manager.UpdateMarketHystory(new System.Threading.CancellationToken(true));
            }

        }
        // This delegate enables asynchronous calls for setting  
        // the text property on a TextBox control.  
        delegate void StringArgReturningVoidDelegate(string text);
        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the  
            // calling thread to the thread ID of the creating thread.  
            // If these threads are different, it returns true.  
            if (this.btnAggiornaStatoMercato.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.btnAggiornaStatoMercato.Text = text;
            }
        }
        private void AggiornaLabel()
        {

            if (_attivaAggiornamento)
            {

                SetText("Avviato aggiornamento automatico");
            }
            else
            {
                SetText("Fermato aggiornamento automatico");
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AggiornaTutto();
        }

        private void chkAcquisti_CheckedChanged(object sender, EventArgs e)
        {
            _manager.Bot.AcquistiController.Attiva((!chkAcquisti.Checked));

        }

        private void chkVendite_CheckedChanged(object sender, EventArgs e)
        {

            _manager.Bot.VenditaController.Attiva((!chkVendite.Checked));
        }
    }
}
