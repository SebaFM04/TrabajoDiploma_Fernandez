namespace UI
{
    partial class frmComanda
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
            this.lblComandasfrmComanda = new System.Windows.Forms.Label();
            this.dgvComandasfrmComanda = new System.Windows.Forms.DataGridView();
            this.lblDetallefrmComanda = new System.Windows.Forms.Label();
            this.dgvDetallefrmComanda = new System.Windows.Forms.DataGridView();
            this.btnConfirmarEntregafrmComanda = new System.Windows.Forms.Button();
            this.btnActualizarfrmComanda = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComandasfrmComanda)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetallefrmComanda)).BeginInit();
            this.SuspendLayout();
            //
            // lblComandasfrmComanda
            //
            this.lblComandasfrmComanda.AutoSize = true;
            this.lblComandasfrmComanda.Location = new System.Drawing.Point(20, 15);
            this.lblComandasfrmComanda.Name = "lblComandasfrmComanda";
            this.lblComandasfrmComanda.TabIndex = 0;
            this.lblComandasfrmComanda.Text = "Comandas pendientes";
            //
            // dgvComandasfrmComanda
            //
            this.dgvComandasfrmComanda.AllowUserToAddRows = false;
            this.dgvComandasfrmComanda.AllowUserToDeleteRows = false;
            this.dgvComandasfrmComanda.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvComandasfrmComanda.Location = new System.Drawing.Point(20, 40);
            this.dgvComandasfrmComanda.MultiSelect = false;
            this.dgvComandasfrmComanda.Name = "dgvComandasfrmComanda";
            this.dgvComandasfrmComanda.ReadOnly = true;
            this.dgvComandasfrmComanda.RowHeadersVisible = false;
            this.dgvComandasfrmComanda.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvComandasfrmComanda.Size = new System.Drawing.Size(330, 315);
            this.dgvComandasfrmComanda.TabIndex = 1;
            this.dgvComandasfrmComanda.SelectionChanged += new System.EventHandler(this.dgvComandasfrmComanda_SelectionChanged);
            //
            // lblDetallefrmComanda
            //
            this.lblDetallefrmComanda.AutoSize = true;
            this.lblDetallefrmComanda.Location = new System.Drawing.Point(370, 15);
            this.lblDetallefrmComanda.Name = "lblDetallefrmComanda";
            this.lblDetallefrmComanda.TabIndex = 2;
            this.lblDetallefrmComanda.Text = "Detalle de la comanda";
            //
            // dgvDetallefrmComanda
            //
            this.dgvDetallefrmComanda.AllowUserToAddRows = false;
            this.dgvDetallefrmComanda.AllowUserToDeleteRows = false;
            this.dgvDetallefrmComanda.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetallefrmComanda.Location = new System.Drawing.Point(370, 40);
            this.dgvDetallefrmComanda.Name = "dgvDetallefrmComanda";
            this.dgvDetallefrmComanda.ReadOnly = true;
            this.dgvDetallefrmComanda.RowHeadersVisible = false;
            this.dgvDetallefrmComanda.Size = new System.Drawing.Size(370, 260);
            this.dgvDetallefrmComanda.TabIndex = 3;
            //
            // btnConfirmarEntregafrmComanda
            //
            this.btnConfirmarEntregafrmComanda.BackColor = System.Drawing.Color.SandyBrown;
            this.btnConfirmarEntregafrmComanda.Location = new System.Drawing.Point(370, 315);
            this.btnConfirmarEntregafrmComanda.Name = "btnConfirmarEntregafrmComanda";
            this.btnConfirmarEntregafrmComanda.Size = new System.Drawing.Size(220, 40);
            this.btnConfirmarEntregafrmComanda.TabIndex = 4;
            this.btnConfirmarEntregafrmComanda.Text = "Confirmar entrega";
            this.btnConfirmarEntregafrmComanda.UseVisualStyleBackColor = false;
            this.btnConfirmarEntregafrmComanda.Click += new System.EventHandler(this.btnConfirmarEntregafrmComanda_Click);
            //
            // btnActualizarfrmComanda
            //
            this.btnActualizarfrmComanda.BackColor = System.Drawing.Color.SandyBrown;
            this.btnActualizarfrmComanda.Location = new System.Drawing.Point(20, 365);
            this.btnActualizarfrmComanda.Name = "btnActualizarfrmComanda";
            this.btnActualizarfrmComanda.Size = new System.Drawing.Size(150, 36);
            this.btnActualizarfrmComanda.TabIndex = 5;
            this.btnActualizarfrmComanda.Text = "Actualizar";
            this.btnActualizarfrmComanda.UseVisualStyleBackColor = false;
            this.btnActualizarfrmComanda.Click += new System.EventHandler(this.btnActualizarfrmComanda_Click);
            //
            // frmComanda
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.NavajoWhite;
            this.ClientSize = new System.Drawing.Size(760, 415);
            this.Controls.Add(this.btnActualizarfrmComanda);
            this.Controls.Add(this.btnConfirmarEntregafrmComanda);
            this.Controls.Add(this.dgvDetallefrmComanda);
            this.Controls.Add(this.lblDetallefrmComanda);
            this.Controls.Add(this.dgvComandasfrmComanda);
            this.Controls.Add(this.lblComandasfrmComanda);
            this.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmComanda";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmComanda";
            this.Load += new System.EventHandler(this.frmComanda_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvComandasfrmComanda)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetallefrmComanda)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblComandasfrmComanda;
        private System.Windows.Forms.DataGridView dgvComandasfrmComanda;
        private System.Windows.Forms.Label lblDetallefrmComanda;
        private System.Windows.Forms.DataGridView dgvDetallefrmComanda;
        private System.Windows.Forms.Button btnConfirmarEntregafrmComanda;
        private System.Windows.Forms.Button btnActualizarfrmComanda;
    }
}
