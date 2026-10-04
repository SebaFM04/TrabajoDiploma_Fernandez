namespace UI
{
    partial class frmVenta
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
            this.lblProductosfrmVenta = new System.Windows.Forms.Label();
            this.dgvProductosfrmVenta = new System.Windows.Forms.DataGridView();
            this.lblCantidadfrmVenta = new System.Windows.Forms.Label();
            this.nudCantidadfrmVenta = new System.Windows.Forms.NumericUpDown();
            this.btnAgregarfrmVenta = new System.Windows.Forms.Button();
            this.lblPedidofrmVenta = new System.Windows.Forms.Label();
            this.dgvPedidofrmVenta = new System.Windows.Forms.DataGridView();
            this.btnQuitarfrmVenta = new System.Windows.Forms.Button();
            this.btnVerificarfrmVenta = new System.Windows.Forms.Button();
            this.lblTotalTitulofrmVenta = new System.Windows.Forms.Label();
            this.lblTotalfrmVenta = new System.Windows.Forms.Label();
            this.lblMedioPagofrmVenta = new System.Windows.Forms.Label();
            this.cmbMedioPagofrmVenta = new System.Windows.Forms.ComboBox();
            this.btnCobrarfrmVenta = new System.Windows.Forms.Button();
            this.btnCancelarfrmVenta = new System.Windows.Forms.Button();
            this.lblDatosClientefrmVenta = new System.Windows.Forms.Label();
            this.lblNombreClientefrmVenta = new System.Windows.Forms.Label();
            this.txtNombreClientefrmVenta = new System.Windows.Forms.TextBox();
            this.lblTelefonoClientefrmVenta = new System.Windows.Forms.Label();
            this.txtTelefonoClientefrmVenta = new System.Windows.Forms.TextBox();
            this.lblCorreoClientefrmVenta = new System.Windows.Forms.Label();
            this.txtCorreoClientefrmVenta = new System.Windows.Forms.TextBox();
            this.btnEmitirFacturafrmVenta = new System.Windows.Forms.Button();
            this.btnNuevaVentafrmVenta = new System.Windows.Forms.Button();
            this.txtResultadofrmVenta = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductosfrmVenta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCantidadfrmVenta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPedidofrmVenta)).BeginInit();
            this.SuspendLayout();
            //
            // lblProductosfrmVenta
            //
            this.lblProductosfrmVenta.AutoSize = true;
            this.lblProductosfrmVenta.Location = new System.Drawing.Point(20, 15);
            this.lblProductosfrmVenta.Name = "lblProductosfrmVenta";
            this.lblProductosfrmVenta.TabIndex = 0;
            this.lblProductosfrmVenta.Text = "Productos";
            //
            // dgvProductosfrmVenta
            //
            this.dgvProductosfrmVenta.AllowUserToAddRows = false;
            this.dgvProductosfrmVenta.AllowUserToDeleteRows = false;
            this.dgvProductosfrmVenta.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductosfrmVenta.Location = new System.Drawing.Point(20, 40);
            this.dgvProductosfrmVenta.MultiSelect = false;
            this.dgvProductosfrmVenta.Name = "dgvProductosfrmVenta";
            this.dgvProductosfrmVenta.ReadOnly = true;
            this.dgvProductosfrmVenta.RowHeadersVisible = false;
            this.dgvProductosfrmVenta.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProductosfrmVenta.Size = new System.Drawing.Size(480, 245);
            this.dgvProductosfrmVenta.TabIndex = 1;
            this.dgvProductosfrmVenta.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProductosfrmVenta_CellDoubleClick);
            //
            // lblCantidadfrmVenta
            //
            this.lblCantidadfrmVenta.AutoSize = true;
            this.lblCantidadfrmVenta.Location = new System.Drawing.Point(20, 300);
            this.lblCantidadfrmVenta.Name = "lblCantidadfrmVenta";
            this.lblCantidadfrmVenta.TabIndex = 2;
            this.lblCantidadfrmVenta.Text = "Cantidad";
            //
            // nudCantidadfrmVenta
            //
            this.nudCantidadfrmVenta.Location = new System.Drawing.Point(110, 297);
            this.nudCantidadfrmVenta.Maximum = new decimal(new int[] { 99, 0, 0, 0 });
            this.nudCantidadfrmVenta.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudCantidadfrmVenta.Name = "nudCantidadfrmVenta";
            this.nudCantidadfrmVenta.Size = new System.Drawing.Size(70, 23);
            this.nudCantidadfrmVenta.TabIndex = 3;
            this.nudCantidadfrmVenta.Value = new decimal(new int[] { 1, 0, 0, 0 });
            //
            // btnAgregarfrmVenta
            //
            this.btnAgregarfrmVenta.BackColor = System.Drawing.Color.SandyBrown;
            this.btnAgregarfrmVenta.Location = new System.Drawing.Point(200, 292);
            this.btnAgregarfrmVenta.Name = "btnAgregarfrmVenta";
            this.btnAgregarfrmVenta.Size = new System.Drawing.Size(180, 32);
            this.btnAgregarfrmVenta.TabIndex = 4;
            this.btnAgregarfrmVenta.Text = "Agregar al pedido";
            this.btnAgregarfrmVenta.UseVisualStyleBackColor = false;
            this.btnAgregarfrmVenta.Click += new System.EventHandler(this.btnAgregarfrmVenta_Click);
            //
            // lblPedidofrmVenta
            //
            this.lblPedidofrmVenta.AutoSize = true;
            this.lblPedidofrmVenta.Location = new System.Drawing.Point(520, 15);
            this.lblPedidofrmVenta.Name = "lblPedidofrmVenta";
            this.lblPedidofrmVenta.TabIndex = 5;
            this.lblPedidofrmVenta.Text = "Pedido";
            //
            // dgvPedidofrmVenta
            //
            this.dgvPedidofrmVenta.AllowUserToAddRows = false;
            this.dgvPedidofrmVenta.AllowUserToDeleteRows = false;
            this.dgvPedidofrmVenta.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPedidofrmVenta.Location = new System.Drawing.Point(520, 40);
            this.dgvPedidofrmVenta.MultiSelect = false;
            this.dgvPedidofrmVenta.Name = "dgvPedidofrmVenta";
            this.dgvPedidofrmVenta.ReadOnly = true;
            this.dgvPedidofrmVenta.RowHeadersVisible = false;
            this.dgvPedidofrmVenta.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPedidofrmVenta.Size = new System.Drawing.Size(560, 200);
            this.dgvPedidofrmVenta.TabIndex = 6;
            //
            // btnQuitarfrmVenta
            //
            this.btnQuitarfrmVenta.BackColor = System.Drawing.Color.SandyBrown;
            this.btnQuitarfrmVenta.Location = new System.Drawing.Point(520, 248);
            this.btnQuitarfrmVenta.Name = "btnQuitarfrmVenta";
            this.btnQuitarfrmVenta.Size = new System.Drawing.Size(120, 32);
            this.btnQuitarfrmVenta.TabIndex = 7;
            this.btnQuitarfrmVenta.Text = "Quitar";
            this.btnQuitarfrmVenta.UseVisualStyleBackColor = false;
            this.btnQuitarfrmVenta.Click += new System.EventHandler(this.btnQuitarfrmVenta_Click);
            //
            // btnVerificarfrmVenta
            //
            this.btnVerificarfrmVenta.BackColor = System.Drawing.Color.SandyBrown;
            this.btnVerificarfrmVenta.Location = new System.Drawing.Point(650, 248);
            this.btnVerificarfrmVenta.Name = "btnVerificarfrmVenta";
            this.btnVerificarfrmVenta.Size = new System.Drawing.Size(220, 32);
            this.btnVerificarfrmVenta.TabIndex = 8;
            this.btnVerificarfrmVenta.Text = "Verificar stock y total";
            this.btnVerificarfrmVenta.UseVisualStyleBackColor = false;
            this.btnVerificarfrmVenta.Click += new System.EventHandler(this.btnVerificarfrmVenta_Click);
            //
            // lblTotalTitulofrmVenta
            //
            this.lblTotalTitulofrmVenta.AutoSize = true;
            this.lblTotalTitulofrmVenta.Location = new System.Drawing.Point(885, 255);
            this.lblTotalTitulofrmVenta.Name = "lblTotalTitulofrmVenta";
            this.lblTotalTitulofrmVenta.TabIndex = 9;
            this.lblTotalTitulofrmVenta.Text = "Total";
            //
            // lblTotalfrmVenta
            //
            this.lblTotalfrmVenta.AutoSize = true;
            this.lblTotalfrmVenta.Font = new System.Drawing.Font("MS Reference Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalfrmVenta.Location = new System.Drawing.Point(950, 253);
            this.lblTotalfrmVenta.Name = "lblTotalfrmVenta";
            this.lblTotalfrmVenta.TabIndex = 10;
            this.lblTotalfrmVenta.Text = "-";
            //
            // lblMedioPagofrmVenta
            //
            this.lblMedioPagofrmVenta.AutoSize = true;
            this.lblMedioPagofrmVenta.Location = new System.Drawing.Point(520, 298);
            this.lblMedioPagofrmVenta.Name = "lblMedioPagofrmVenta";
            this.lblMedioPagofrmVenta.TabIndex = 11;
            this.lblMedioPagofrmVenta.Text = "Medio de pago";
            //
            // cmbMedioPagofrmVenta
            //
            this.cmbMedioPagofrmVenta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMedioPagofrmVenta.Location = new System.Drawing.Point(650, 294);
            this.cmbMedioPagofrmVenta.Name = "cmbMedioPagofrmVenta";
            this.cmbMedioPagofrmVenta.Size = new System.Drawing.Size(170, 24);
            this.cmbMedioPagofrmVenta.TabIndex = 12;
            //
            // btnCobrarfrmVenta
            //
            this.btnCobrarfrmVenta.BackColor = System.Drawing.Color.SandyBrown;
            this.btnCobrarfrmVenta.Location = new System.Drawing.Point(840, 290);
            this.btnCobrarfrmVenta.Name = "btnCobrarfrmVenta";
            this.btnCobrarfrmVenta.Size = new System.Drawing.Size(115, 34);
            this.btnCobrarfrmVenta.TabIndex = 13;
            this.btnCobrarfrmVenta.Text = "Cobrar";
            this.btnCobrarfrmVenta.UseVisualStyleBackColor = false;
            this.btnCobrarfrmVenta.Click += new System.EventHandler(this.btnCobrarfrmVenta_Click);
            //
            // btnCancelarfrmVenta
            //
            this.btnCancelarfrmVenta.BackColor = System.Drawing.Color.SandyBrown;
            this.btnCancelarfrmVenta.Location = new System.Drawing.Point(965, 290);
            this.btnCancelarfrmVenta.Name = "btnCancelarfrmVenta";
            this.btnCancelarfrmVenta.Size = new System.Drawing.Size(115, 34);
            this.btnCancelarfrmVenta.TabIndex = 14;
            this.btnCancelarfrmVenta.Text = "Cancelar";
            this.btnCancelarfrmVenta.UseVisualStyleBackColor = false;
            this.btnCancelarfrmVenta.Click += new System.EventHandler(this.btnCancelarfrmVenta_Click);
            //
            // lblDatosClientefrmVenta
            //
            this.lblDatosClientefrmVenta.AutoSize = true;
            this.lblDatosClientefrmVenta.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDatosClientefrmVenta.Location = new System.Drawing.Point(20, 345);
            this.lblDatosClientefrmVenta.Name = "lblDatosClientefrmVenta";
            this.lblDatosClientefrmVenta.TabIndex = 15;
            this.lblDatosClientefrmVenta.Text = "Factura: datos del cliente (opcionales)";
            //
            // lblNombreClientefrmVenta
            //
            this.lblNombreClientefrmVenta.AutoSize = true;
            this.lblNombreClientefrmVenta.Location = new System.Drawing.Point(20, 378);
            this.lblNombreClientefrmVenta.Name = "lblNombreClientefrmVenta";
            this.lblNombreClientefrmVenta.TabIndex = 16;
            this.lblNombreClientefrmVenta.Text = "Nombre";
            //
            // txtNombreClientefrmVenta
            //
            this.txtNombreClientefrmVenta.Location = new System.Drawing.Point(130, 375);
            this.txtNombreClientefrmVenta.MaxLength = 100;
            this.txtNombreClientefrmVenta.Name = "txtNombreClientefrmVenta";
            this.txtNombreClientefrmVenta.Size = new System.Drawing.Size(270, 23);
            this.txtNombreClientefrmVenta.TabIndex = 17;
            //
            // lblTelefonoClientefrmVenta
            //
            this.lblTelefonoClientefrmVenta.AutoSize = true;
            this.lblTelefonoClientefrmVenta.Location = new System.Drawing.Point(20, 411);
            this.lblTelefonoClientefrmVenta.Name = "lblTelefonoClientefrmVenta";
            this.lblTelefonoClientefrmVenta.TabIndex = 18;
            this.lblTelefonoClientefrmVenta.Text = "Teléfono";
            //
            // txtTelefonoClientefrmVenta
            //
            this.txtTelefonoClientefrmVenta.Location = new System.Drawing.Point(130, 408);
            this.txtTelefonoClientefrmVenta.MaxLength = 30;
            this.txtTelefonoClientefrmVenta.Name = "txtTelefonoClientefrmVenta";
            this.txtTelefonoClientefrmVenta.Size = new System.Drawing.Size(270, 23);
            this.txtTelefonoClientefrmVenta.TabIndex = 19;
            //
            // lblCorreoClientefrmVenta
            //
            this.lblCorreoClientefrmVenta.AutoSize = true;
            this.lblCorreoClientefrmVenta.Location = new System.Drawing.Point(20, 444);
            this.lblCorreoClientefrmVenta.Name = "lblCorreoClientefrmVenta";
            this.lblCorreoClientefrmVenta.TabIndex = 20;
            this.lblCorreoClientefrmVenta.Text = "Correo";
            //
            // txtCorreoClientefrmVenta
            //
            this.txtCorreoClientefrmVenta.Location = new System.Drawing.Point(130, 441);
            this.txtCorreoClientefrmVenta.MaxLength = 100;
            this.txtCorreoClientefrmVenta.Name = "txtCorreoClientefrmVenta";
            this.txtCorreoClientefrmVenta.Size = new System.Drawing.Size(270, 23);
            this.txtCorreoClientefrmVenta.TabIndex = 21;
            //
            // btnEmitirFacturafrmVenta
            //
            this.btnEmitirFacturafrmVenta.BackColor = System.Drawing.Color.SandyBrown;
            this.btnEmitirFacturafrmVenta.Location = new System.Drawing.Point(130, 478);
            this.btnEmitirFacturafrmVenta.Name = "btnEmitirFacturafrmVenta";
            this.btnEmitirFacturafrmVenta.Size = new System.Drawing.Size(270, 34);
            this.btnEmitirFacturafrmVenta.TabIndex = 22;
            this.btnEmitirFacturafrmVenta.Text = "Emitir factura";
            this.btnEmitirFacturafrmVenta.UseVisualStyleBackColor = false;
            this.btnEmitirFacturafrmVenta.Click += new System.EventHandler(this.btnEmitirFacturafrmVenta_Click);
            //
            // btnNuevaVentafrmVenta
            //
            this.btnNuevaVentafrmVenta.BackColor = System.Drawing.Color.SandyBrown;
            this.btnNuevaVentafrmVenta.Location = new System.Drawing.Point(130, 522);
            this.btnNuevaVentafrmVenta.Name = "btnNuevaVentafrmVenta";
            this.btnNuevaVentafrmVenta.Size = new System.Drawing.Size(270, 34);
            this.btnNuevaVentafrmVenta.TabIndex = 23;
            this.btnNuevaVentafrmVenta.Text = "Nueva venta";
            this.btnNuevaVentafrmVenta.UseVisualStyleBackColor = false;
            this.btnNuevaVentafrmVenta.Click += new System.EventHandler(this.btnNuevaVentafrmVenta_Click);
            //
            // txtResultadofrmVenta
            //
            this.txtResultadofrmVenta.BackColor = System.Drawing.Color.White;
            this.txtResultadofrmVenta.Location = new System.Drawing.Point(520, 345);
            this.txtResultadofrmVenta.Multiline = true;
            this.txtResultadofrmVenta.Name = "txtResultadofrmVenta";
            this.txtResultadofrmVenta.ReadOnly = true;
            this.txtResultadofrmVenta.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResultadofrmVenta.Size = new System.Drawing.Size(560, 211);
            this.txtResultadofrmVenta.TabIndex = 24;
            //
            // frmVenta
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.NavajoWhite;
            this.ClientSize = new System.Drawing.Size(1100, 575);
            this.Controls.Add(this.txtResultadofrmVenta);
            this.Controls.Add(this.btnNuevaVentafrmVenta);
            this.Controls.Add(this.btnEmitirFacturafrmVenta);
            this.Controls.Add(this.txtCorreoClientefrmVenta);
            this.Controls.Add(this.lblCorreoClientefrmVenta);
            this.Controls.Add(this.txtTelefonoClientefrmVenta);
            this.Controls.Add(this.lblTelefonoClientefrmVenta);
            this.Controls.Add(this.txtNombreClientefrmVenta);
            this.Controls.Add(this.lblNombreClientefrmVenta);
            this.Controls.Add(this.lblDatosClientefrmVenta);
            this.Controls.Add(this.btnCancelarfrmVenta);
            this.Controls.Add(this.btnCobrarfrmVenta);
            this.Controls.Add(this.cmbMedioPagofrmVenta);
            this.Controls.Add(this.lblMedioPagofrmVenta);
            this.Controls.Add(this.lblTotalfrmVenta);
            this.Controls.Add(this.lblTotalTitulofrmVenta);
            this.Controls.Add(this.btnVerificarfrmVenta);
            this.Controls.Add(this.btnQuitarfrmVenta);
            this.Controls.Add(this.dgvPedidofrmVenta);
            this.Controls.Add(this.lblPedidofrmVenta);
            this.Controls.Add(this.btnAgregarfrmVenta);
            this.Controls.Add(this.nudCantidadfrmVenta);
            this.Controls.Add(this.lblCantidadfrmVenta);
            this.Controls.Add(this.dgvProductosfrmVenta);
            this.Controls.Add(this.lblProductosfrmVenta);
            this.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmVenta";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmVenta";
            this.Load += new System.EventHandler(this.frmVenta_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductosfrmVenta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCantidadfrmVenta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPedidofrmVenta)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProductosfrmVenta;
        private System.Windows.Forms.DataGridView dgvProductosfrmVenta;
        private System.Windows.Forms.Label lblCantidadfrmVenta;
        private System.Windows.Forms.NumericUpDown nudCantidadfrmVenta;
        private System.Windows.Forms.Button btnAgregarfrmVenta;
        private System.Windows.Forms.Label lblPedidofrmVenta;
        private System.Windows.Forms.DataGridView dgvPedidofrmVenta;
        private System.Windows.Forms.Button btnQuitarfrmVenta;
        private System.Windows.Forms.Button btnVerificarfrmVenta;
        private System.Windows.Forms.Label lblTotalTitulofrmVenta;
        private System.Windows.Forms.Label lblTotalfrmVenta;
        private System.Windows.Forms.Label lblMedioPagofrmVenta;
        private System.Windows.Forms.ComboBox cmbMedioPagofrmVenta;
        private System.Windows.Forms.Button btnCobrarfrmVenta;
        private System.Windows.Forms.Button btnCancelarfrmVenta;
        private System.Windows.Forms.Label lblDatosClientefrmVenta;
        private System.Windows.Forms.Label lblNombreClientefrmVenta;
        private System.Windows.Forms.TextBox txtNombreClientefrmVenta;
        private System.Windows.Forms.Label lblTelefonoClientefrmVenta;
        private System.Windows.Forms.TextBox txtTelefonoClientefrmVenta;
        private System.Windows.Forms.Label lblCorreoClientefrmVenta;
        private System.Windows.Forms.TextBox txtCorreoClientefrmVenta;
        private System.Windows.Forms.Button btnEmitirFacturafrmVenta;
        private System.Windows.Forms.Button btnNuevaVentafrmVenta;
        private System.Windows.Forms.TextBox txtResultadofrmVenta;
    }
}
