namespace Bittrex_App
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gvBalance = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pnlTab01Parte02 = new System.Windows.Forms.Panel();
            this.gvStato = new System.Windows.Forms.DataGridView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gvErrori = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnAggiornaStatoMercato = new System.Windows.Forms.Button();
            this.chkAcquisti = new System.Windows.Forms.CheckBox();
            this.chkVendite = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.gvBalance)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.pnlTab01Parte02.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvStato)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvErrori)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvBalance
            // 
            this.gvBalance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvBalance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvBalance.Location = new System.Drawing.Point(0, 0);
            this.gvBalance.Name = "gvBalance";
            this.gvBalance.Size = new System.Drawing.Size(779, 100);
            this.gvBalance.TabIndex = 3;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(793, 442);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pnlTab01Parte02);
            this.tabPage1.Controls.Add(this.splitter1);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 33);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(785, 405);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // pnlTab01Parte02
            // 
            this.pnlTab01Parte02.Controls.Add(this.gvStato);
            this.pnlTab01Parte02.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTab01Parte02.Location = new System.Drawing.Point(3, 106);
            this.pnlTab01Parte02.Name = "pnlTab01Parte02";
            this.pnlTab01Parte02.Size = new System.Drawing.Size(779, 296);
            this.pnlTab01Parte02.TabIndex = 6;
            // 
            // gvStato
            // 
            this.gvStato.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvStato.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvStato.Location = new System.Drawing.Point(0, 0);
            this.gvStato.Name = "gvStato";
            this.gvStato.Size = new System.Drawing.Size(779, 296);
            this.gvStato.TabIndex = 4;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(3, 103);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(779, 3);
            this.splitter1.TabIndex = 5;
            this.splitter1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gvBalance);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(779, 100);
            this.panel1.TabIndex = 4;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gvErrori);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 33);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(785, 405);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Errori";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // gvErrori
            // 
            this.gvErrori.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvErrori.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvErrori.Location = new System.Drawing.Point(3, 27);
            this.gvErrori.Name = "gvErrori";
            this.gvErrori.Size = new System.Drawing.Size(779, 375);
            this.gvErrori.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Errori";
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 33);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(785, 405);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Automatico";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnAggiornaStatoMercato
            // 
            this.btnAggiornaStatoMercato.Location = new System.Drawing.Point(7, 444);
            this.btnAggiornaStatoMercato.Name = "btnAggiornaStatoMercato";
            this.btnAggiornaStatoMercato.Size = new System.Drawing.Size(176, 118);
            this.btnAggiornaStatoMercato.TabIndex = 6;
            this.btnAggiornaStatoMercato.Text = "Aggiorna stato mercato";
            this.btnAggiornaStatoMercato.UseVisualStyleBackColor = true;
            this.btnAggiornaStatoMercato.Click += new System.EventHandler(this.button3_Click);
            // 
            // chkAcquisti
            // 
            this.chkAcquisti.AutoSize = true;
            this.chkAcquisti.Location = new System.Drawing.Point(60, 27);
            this.chkAcquisti.Name = "chkAcquisti";
            this.chkAcquisti.Size = new System.Drawing.Size(63, 17);
            this.chkAcquisti.TabIndex = 7;
            this.chkAcquisti.Text = "Acquisti";
            this.chkAcquisti.UseVisualStyleBackColor = true;
            this.chkAcquisti.CheckedChanged += new System.EventHandler(this.chkAcquisti_CheckedChanged);
            // 
            // chkVendite
            // 
            this.chkVendite.AutoSize = true;
            this.chkVendite.Location = new System.Drawing.Point(60, 50);
            this.chkVendite.Name = "chkVendite";
            this.chkVendite.Size = new System.Drawing.Size(62, 17);
            this.chkVendite.TabIndex = 8;
            this.chkVendite.Text = "Vendite";
            this.chkVendite.UseVisualStyleBackColor = true;
            this.chkVendite.CheckedChanged += new System.EventHandler(this.chkVendite_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkVendite);
            this.groupBox1.Controls.Add(this.chkAcquisti);
            this.groupBox1.Location = new System.Drawing.Point(230, 448);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(253, 96);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Attiva Bot o spegni";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 641);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnAggiornaStatoMercato);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvBalance)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.pnlTab01Parte02.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvStato)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvErrori)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView gvBalance;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel pnlTab01Parte02;
        private System.Windows.Forms.DataGridView gvStato;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView gvErrori;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnAggiornaStatoMercato;
        private System.Windows.Forms.CheckBox chkAcquisti;
        private System.Windows.Forms.CheckBox chkVendite;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

