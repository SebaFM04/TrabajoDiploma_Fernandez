namespace UI
{
    partial class frmStockBajo
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
            this.lblAyudafrmStockBajo = new System.Windows.Forms.Label();
            this.dgvInsumosfrmStockBajo = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInsumosfrmStockBajo)).BeginInit();
            this.SuspendLayout();
            //
            // lblAyudafrmStockBajo
            //
            this.lblAyudafrmStockBajo.AutoSize = true;
            this.lblAyudafrmStockBajo.Location = new System.Drawing.Point(20, 15);
            this.lblAyudafrmStockBajo.Name = "lblAyudafrmStockBajo";
            this.lblAyudafrmStockBajo.TabIndex = 0;
            this.lblAyudafrmStockBajo.Text = "Insumos por debajo del umbral de reposición o con aviso pendiente";
            //
            // dgvInsumosfrmStockBajo
            //
            this.dgvInsumosfrmStockBajo.AllowUserToAddRows = false;
            this.dgvInsumosfrmStockBajo.AllowUserToDeleteRows = false;
            this.dgvInsumosfrmStockBajo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInsumosfrmStockBajo.Location = new System.Drawing.Point(20, 40);
            this.dgvInsumosfrmStockBajo.Name = "dgvInsumosfrmStockBajo";
            this.dgvInsumosfrmStockBajo.ReadOnly = true;
            this.dgvInsumosfrmStockBajo.RowHeadersVisible = false;
            this.dgvInsumosfrmStockBajo.Size = new System.Drawing.Size(640, 260);
            this.dgvInsumosfrmStockBajo.TabIndex = 1;
            //
            // frmStockBajo
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.NavajoWhite;
            this.ClientSize = new System.Drawing.Size(680, 320);
            this.Controls.Add(this.dgvInsumosfrmStockBajo);
            this.Controls.Add(this.lblAyudafrmStockBajo);
            this.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmStockBajo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmStockBajo";
            this.Load += new System.EventHandler(this.frmStockBajo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInsumosfrmStockBajo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAyudafrmStockBajo;
        private System.Windows.Forms.DataGridView dgvInsumosfrmStockBajo;
    }
}
