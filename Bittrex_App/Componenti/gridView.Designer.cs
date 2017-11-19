namespace Bittrex_App.Componenti
{
    partial class gridViewUsercontrol
    {
        /// <summary> 
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione componenti

        /// <summary> 
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.gvRules = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.gvRules)).BeginInit();
            this.SuspendLayout();
            // 
            // gvRules
            // 
            this.gvRules.AllowUserToOrderColumns = true;
            this.gvRules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvRules.Location = new System.Drawing.Point(0, 0);
            this.gvRules.Name = "gvRules";
            this.gvRules.Size = new System.Drawing.Size(1087, 518);
            this.gvRules.TabIndex = 6;
            // 
            // gridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gvRules);
            this.Name = "gridView";
            this.Size = new System.Drawing.Size(699, 464);
            this.Load += new System.EventHandler(this.gridView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvRules)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gvRules;
    }
}
