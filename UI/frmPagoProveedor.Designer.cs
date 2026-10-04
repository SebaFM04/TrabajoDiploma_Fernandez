namespace UI
{
    partial class frmPagoProveedor
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
            this.lblOrdenesfrmPagoProveedor = new System.Windows.Forms.Label();
            this.dgvOrdenesfrmPagoProveedor = new System.Windows.Forms.DataGridView();
            this.lblFacturasfrmPagoProveedor = new System.Windows.Forms.Label();
            this.dgvFacturasfrmPagoProveedor = new System.Windows.Forms.DataGridView();
            this.lblTotalfrmPagoProveedor = new System.Windows.Forms.Label();
            this.lblMediofrmPagoProveedor = new System.Windows.Forms.Label();
            this.cmbMediofrmPagoProveedor = new System.Windows.Forms.ComboBox();
            this.lblFechafrmPagoProveedor = new System.Windows.Forms.Label();
            this.dtpFechafrmPagoProveedor = new System.Windows.Forms.DateTimePicker();
            this.lblComprobantefrmPagoProveedor = new System.Windows.Forms.Label();
            this.txtComprobantefrmPagoProveedor = new System.Windows.Forms.TextBox();
            this.lblMontofrmPagoProveedor = new System.Windows.Forms.Label();
            this.txtMontofrmPagoProveedor = new System.Windows.Forms.TextBox();
            this.btnRegistrarfrmPagoProveedor = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrdenesfrmPagoProveedor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFacturasfrmPagoProveedor)).BeginInit();
            this.SuspendLayout();
            //
            // lblOrdenesfrmPagoProveedor
            //
            this.lblOrdenesfrmPagoProveedor.AutoSize = true;
            this.lblOrdenesfrmPagoProveedor.Location = new System.Drawing.Point(20, 15);
            this.lblOrdenesfrmPagoProveedor.Name = "lblOrdenesfrmPagoProveedor";
            this.lblOrdenesfrmPagoProveedor.TabIndex = 0;
            this.lblOrdenesfrmPagoProveedor.Text = "Órdenes pendientes de pago";
            //
            // dgvOrdenesfrmPagoProveedor
            //
            this.dgvOrdenesfrmPagoProveedor.AllowUserToAddRows = false;
            this.dgvOrdenesfrmPagoProveedor.AllowUserToDeleteRows = false;
            this.dgvOrdenesfrmPagoProveedor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrdenesfrmPagoProveedor.Location = new System.Drawing.Point(20, 40);
            this.dgvOrdenesfrmPagoProveedor.MultiSelect = false;
            this.dgvOrdenesfrmPagoProveedor.Name = "dgvOrdenesfrmPagoProveedor";
            this.dgvOrdenesfrmPagoProveedor.ReadOnly = true;
            this.dgvOrdenesfrmPagoProveedor.RowHeadersVisible = false;
            this.dgvOrdenesfrmPagoProveedor.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrdenesfrmPagoProveedor.Size = new System.Drawing.Size(460, 400);
            this.dgvOrdenesfrmPagoProveedor.TabIndex = 1;
            this.dgvOrdenesfrmPagoProveedor.SelectionChanged += new System.EventHandler(this.dgvOrdenesfrmPagoProveedor_SelectionChanged);
            //
            // lblFacturasfrmPagoProveedor
            //
            this.lblFacturasfrmPagoProveedor.AutoSize = true;
            this.lblFacturasfrmPagoProveedor.Location = new System.Drawing.Point(500, 15);
            this.lblFacturasfrmPagoProveedor.Name = "lblFacturasfrmPagoProveedor";
            this.lblFacturasfrmPagoProveedor.TabIndex = 2;
            this.lblFacturasfrmPagoProveedor.Text = "Facturas del proveedor (recepciones)";
            //
            // dgvFacturasfrmPagoProveedor
            //
            this.dgvFacturasfrmPagoProveedor.AllowUserToAddRows = false;
            this.dgvFacturasfrmPagoProveedor.AllowUserToDeleteRows = false;
            this.dgvFacturasfrmPagoProveedor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFacturasfrmPagoProveedor.Location = new System.Drawing.Point(500, 40);
            this.dgvFacturasfrmPagoProveedor.Name = "dgvFacturasfrmPagoProveedor";
            this.dgvFacturasfrmPagoProveedor.ReadOnly = true;
            this.dgvFacturasfrmPagoProveedor.RowHeadersVisible = false;
            this.dgvFacturasfrmPagoProveedor.Size = new System.Drawing.Size(460, 170);
            this.dgvFacturasfrmPagoProveedor.TabIndex = 3;
            //
            // lblTotalfrmPagoProveedor
            //
            this.lblTotalfrmPagoProveedor.AutoSize = true;
            this.lblTotalfrmPagoProveedor.Font = new System.Drawing.Font("MS Reference Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalfrmPagoProveedor.Location = new System.Drawing.Point(500, 222);
            this.lblTotalfrmPagoProveedor.Name = "lblTotalfrmPagoProveedor";
            this.lblTotalfrmPagoProveedor.TabIndex = 4;
            this.lblTotalfrmPagoProveedor.Text = "Total";
            //
            // lblMediofrmPagoProveedor
            //
            this.lblMediofrmPagoProveedor.AutoSize = true;
            this.lblMediofrmPagoProveedor.Location = new System.Drawing.Point(500, 265);
            this.lblMediofrmPagoProveedor.Name = "lblMediofrmPagoProveedor";
            this.lblMediofrmPagoProveedor.TabIndex = 5;
            this.lblMediofrmPagoProveedor.Text = "Medio de pago";
            //
            // cmbMediofrmPagoProveedor
            //
            this.cmbMediofrmPagoProveedor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMediofrmPagoProveedor.Location = new System.Drawing.Point(660, 262);
            this.cmbMediofrmPagoProveedor.Name = "cmbMediofrmPagoProveedor";
            this.cmbMediofrmPagoProveedor.Size = new System.Drawing.Size(300, 24);
            this.cmbMediofrmPagoProveedor.TabIndex = 6;
            //
            // lblFechafrmPagoProveedor
            //
            this.lblFechafrmPagoProveedor.AutoSize = true;
            this.lblFechafrmPagoProveedor.Location = new System.Drawing.Point(500, 300);
            this.lblFechafrmPagoProveedor.Name = "lblFechafrmPagoProveedor";
            this.lblFechafrmPagoProveedor.TabIndex = 7;
            this.lblFechafrmPagoProveedor.Text = "Fecha de pago";
            //
            // dtpFechafrmPagoProveedor
            //
            this.dtpFechafrmPagoProveedor.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechafrmPagoProveedor.Location = new System.Drawing.Point(660, 297);
            this.dtpFechafrmPagoProveedor.Name = "dtpFechafrmPagoProveedor";
            this.dtpFechafrmPagoProveedor.Size = new System.Drawing.Size(300, 23);
            this.dtpFechafrmPagoProveedor.TabIndex = 8;
            //
            // lblComprobantefrmPagoProveedor
            //
            this.lblComprobantefrmPagoProveedor.AutoSize = true;
            this.lblComprobantefrmPagoProveedor.Location = new System.Drawing.Point(500, 335);
            this.lblComprobantefrmPagoProveedor.Name = "lblComprobantefrmPagoProveedor";
            this.lblComprobantefrmPagoProveedor.TabIndex = 9;
            this.lblComprobantefrmPagoProveedor.Text = "N° de comprobante";
            //
            // txtComprobantefrmPagoProveedor
            //
            this.txtComprobantefrmPagoProveedor.Location = new System.Drawing.Point(660, 332);
            this.txtComprobantefrmPagoProveedor.MaxLength = 30;
            this.txtComprobantefrmPagoProveedor.Name = "txtComprobantefrmPagoProveedor";
            this.txtComprobantefrmPagoProveedor.Size = new System.Drawing.Size(300, 23);
            this.txtComprobantefrmPagoProveedor.TabIndex = 10;
            //
            // lblMontofrmPagoProveedor
            //
            this.lblMontofrmPagoProveedor.AutoSize = true;
            this.lblMontofrmPagoProveedor.Location = new System.Drawing.Point(500, 370);
            this.lblMontofrmPagoProveedor.Name = "lblMontofrmPagoProveedor";
            this.lblMontofrmPagoProveedor.TabIndex = 11;
            this.lblMontofrmPagoProveedor.Text = "Monto ($)";
            //
            // txtMontofrmPagoProveedor
            //
            this.txtMontofrmPagoProveedor.Location = new System.Drawing.Point(660, 367);
            this.txtMontofrmPagoProveedor.Name = "txtMontofrmPagoProveedor";
            this.txtMontofrmPagoProveedor.Size = new System.Drawing.Size(300, 23);
            this.txtMontofrmPagoProveedor.TabIndex = 12;
            //
            // btnRegistrarfrmPagoProveedor
            //
            this.btnRegistrarfrmPagoProveedor.BackColor = System.Drawing.Color.SandyBrown;
            this.btnRegistrarfrmPagoProveedor.Location = new System.Drawing.Point(720, 404);
            this.btnRegistrarfrmPagoProveedor.Name = "btnRegistrarfrmPagoProveedor";
            this.btnRegistrarfrmPagoProveedor.Size = new System.Drawing.Size(240, 36);
            this.btnRegistrarfrmPagoProveedor.TabIndex = 13;
            this.btnRegistrarfrmPagoProveedor.Text = "Registrar pago";
            this.btnRegistrarfrmPagoProveedor.UseVisualStyleBackColor = false;
            this.btnRegistrarfrmPagoProveedor.Click += new System.EventHandler(this.btnRegistrarfrmPagoProveedor_Click);
            //
            // frmPagoProveedor
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.NavajoWhite;
            this.ClientSize = new System.Drawing.Size(980, 460);
            this.Controls.Add(this.btnRegistrarfrmPagoProveedor);
            this.Controls.Add(this.txtMontofrmPagoProveedor);
            this.Controls.Add(this.lblMontofrmPagoProveedor);
            this.Controls.Add(this.txtComprobantefrmPagoProveedor);
            this.Controls.Add(this.lblComprobantefrmPagoProveedor);
            this.Controls.Add(this.dtpFechafrmPagoProveedor);
            this.Controls.Add(this.lblFechafrmPagoProveedor);
            this.Controls.Add(this.cmbMediofrmPagoProveedor);
            this.Controls.Add(this.lblMediofrmPagoProveedor);
            this.Controls.Add(this.lblTotalfrmPagoProveedor);
            this.Controls.Add(this.dgvFacturasfrmPagoProveedor);
            this.Controls.Add(this.lblFacturasfrmPagoProveedor);
            this.Controls.Add(this.dgvOrdenesfrmPagoProveedor);
            this.Controls.Add(this.lblOrdenesfrmPagoProveedor);
            this.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmPagoProveedor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmPagoProveedor";
            this.Load += new System.EventHandler(this.frmPagoProveedor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrdenesfrmPagoProveedor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFacturasfrmPagoProveedor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblOrdenesfrmPagoProveedor;
        private System.Windows.Forms.DataGridView dgvOrdenesfrmPagoProveedor;
        private System.Windows.Forms.Label lblFacturasfrmPagoProveedor;
        private System.Windows.Forms.DataGridView dgvFacturasfrmPagoProveedor;
        private System.Windows.Forms.Label lblTotalfrmPagoProveedor;
        private System.Windows.Forms.Label lblMediofrmPagoProveedor;
        private System.Windows.Forms.ComboBox cmbMediofrmPagoProveedor;
        private System.Windows.Forms.Label lblFechafrmPagoProveedor;
        private System.Windows.Forms.DateTimePicker dtpFechafrmPagoProveedor;
        private System.Windows.Forms.Label lblComprobantefrmPagoProveedor;
        private System.Windows.Forms.TextBox txtComprobantefrmPagoProveedor;
        private System.Windows.Forms.Label lblMontofrmPagoProveedor;
        private System.Windows.Forms.TextBox txtMontofrmPagoProveedor;
        private System.Windows.Forms.Button btnRegistrarfrmPagoProveedor;
    }
}
