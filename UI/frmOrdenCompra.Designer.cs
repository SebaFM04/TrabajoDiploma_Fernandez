namespace UI
{
    partial class frmOrdenCompra
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
            this.lblAvisosfrmOrdenCompra = new System.Windows.Forms.Label();
            this.rdbNuevafrmOrdenCompra = new System.Windows.Forms.RadioButton();
            this.rdbObservadasfrmOrdenCompra = new System.Windows.Forms.RadioButton();
            this.dgvObservadasfrmOrdenCompra = new System.Windows.Forms.DataGridView();
            this.lblObservacionesfrmOrdenCompra = new System.Windows.Forms.Label();
            this.txtObservacionesfrmOrdenCompra = new System.Windows.Forms.TextBox();
            this.dgvAvisosfrmOrdenCompra = new System.Windows.Forms.DataGridView();
            this.lblProveedorfrmOrdenCompra = new System.Windows.Forms.Label();
            this.cmbProveedorfrmOrdenCompra = new System.Windows.Forms.ComboBox();
            this.btnNuevoProveedorfrmOrdenCompra = new System.Windows.Forms.Button();
            this.lblInsumosProveedorfrmOrdenCompra = new System.Windows.Forms.Label();
            this.dgvPedidofrmOrdenCompra = new System.Windows.Forms.DataGridView();
            this.btnLimpiarfrmOrdenCompra = new System.Windows.Forms.Button();
            this.btnConfirmarfrmOrdenCompra = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvisosfrmOrdenCompra)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvObservadasfrmOrdenCompra)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPedidofrmOrdenCompra)).BeginInit();
            this.SuspendLayout();
            //
            // rdbNuevafrmOrdenCompra
            //
            this.rdbNuevafrmOrdenCompra.AutoSize = true;
            this.rdbNuevafrmOrdenCompra.Checked = true;
            this.rdbNuevafrmOrdenCompra.Location = new System.Drawing.Point(20, 12);
            this.rdbNuevafrmOrdenCompra.Name = "rdbNuevafrmOrdenCompra";
            this.rdbNuevafrmOrdenCompra.TabIndex = 20;
            this.rdbNuevafrmOrdenCompra.TabStop = true;
            this.rdbNuevafrmOrdenCompra.Text = "Nueva orden";
            this.rdbNuevafrmOrdenCompra.UseVisualStyleBackColor = true;
            this.rdbNuevafrmOrdenCompra.CheckedChanged += new System.EventHandler(this.rdbModo_CheckedChanged);
            //
            // rdbObservadasfrmOrdenCompra
            //
            this.rdbObservadasfrmOrdenCompra.AutoSize = true;
            this.rdbObservadasfrmOrdenCompra.Location = new System.Drawing.Point(200, 12);
            this.rdbObservadasfrmOrdenCompra.Name = "rdbObservadasfrmOrdenCompra";
            this.rdbObservadasfrmOrdenCompra.TabIndex = 21;
            this.rdbObservadasfrmOrdenCompra.Text = "Órdenes observadas";
            this.rdbObservadasfrmOrdenCompra.UseVisualStyleBackColor = true;
            this.rdbObservadasfrmOrdenCompra.CheckedChanged += new System.EventHandler(this.rdbModo_CheckedChanged);
            //
            // dgvObservadasfrmOrdenCompra
            //
            this.dgvObservadasfrmOrdenCompra.AllowUserToAddRows = false;
            this.dgvObservadasfrmOrdenCompra.AllowUserToDeleteRows = false;
            this.dgvObservadasfrmOrdenCompra.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvObservadasfrmOrdenCompra.Location = new System.Drawing.Point(20, 70);
            this.dgvObservadasfrmOrdenCompra.MultiSelect = false;
            this.dgvObservadasfrmOrdenCompra.Name = "dgvObservadasfrmOrdenCompra";
            this.dgvObservadasfrmOrdenCompra.ReadOnly = true;
            this.dgvObservadasfrmOrdenCompra.RowHeadersVisible = false;
            this.dgvObservadasfrmOrdenCompra.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvObservadasfrmOrdenCompra.Size = new System.Drawing.Size(420, 200);
            this.dgvObservadasfrmOrdenCompra.TabIndex = 22;
            this.dgvObservadasfrmOrdenCompra.Visible = false;
            this.dgvObservadasfrmOrdenCompra.SelectionChanged += new System.EventHandler(this.dgvObservadasfrmOrdenCompra_SelectionChanged);
            //
            // lblObservacionesfrmOrdenCompra
            //
            this.lblObservacionesfrmOrdenCompra.AutoSize = true;
            this.lblObservacionesfrmOrdenCompra.Location = new System.Drawing.Point(20, 280);
            this.lblObservacionesfrmOrdenCompra.Name = "lblObservacionesfrmOrdenCompra";
            this.lblObservacionesfrmOrdenCompra.TabIndex = 23;
            this.lblObservacionesfrmOrdenCompra.Text = "Observaciones del Dueño";
            this.lblObservacionesfrmOrdenCompra.Visible = false;
            //
            // txtObservacionesfrmOrdenCompra
            //
            this.txtObservacionesfrmOrdenCompra.BackColor = System.Drawing.Color.White;
            this.txtObservacionesfrmOrdenCompra.Location = new System.Drawing.Point(20, 303);
            this.txtObservacionesfrmOrdenCompra.Multiline = true;
            this.txtObservacionesfrmOrdenCompra.Name = "txtObservacionesfrmOrdenCompra";
            this.txtObservacionesfrmOrdenCompra.ReadOnly = true;
            this.txtObservacionesfrmOrdenCompra.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtObservacionesfrmOrdenCompra.Size = new System.Drawing.Size(420, 147);
            this.txtObservacionesfrmOrdenCompra.TabIndex = 24;
            this.txtObservacionesfrmOrdenCompra.Visible = false;
            //
            // lblAvisosfrmOrdenCompra
            //
            this.lblAvisosfrmOrdenCompra.AutoSize = true;
            this.lblAvisosfrmOrdenCompra.Location = new System.Drawing.Point(20, 45);
            this.lblAvisosfrmOrdenCompra.Name = "lblAvisosfrmOrdenCompra";
            this.lblAvisosfrmOrdenCompra.TabIndex = 0;
            this.lblAvisosfrmOrdenCompra.Text = "Insumos con aviso de stock bajo";
            //
            // dgvAvisosfrmOrdenCompra
            //
            this.dgvAvisosfrmOrdenCompra.AllowUserToAddRows = false;
            this.dgvAvisosfrmOrdenCompra.AllowUserToDeleteRows = false;
            this.dgvAvisosfrmOrdenCompra.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAvisosfrmOrdenCompra.Location = new System.Drawing.Point(20, 70);
            this.dgvAvisosfrmOrdenCompra.Name = "dgvAvisosfrmOrdenCompra";
            this.dgvAvisosfrmOrdenCompra.ReadOnly = true;
            this.dgvAvisosfrmOrdenCompra.RowHeadersVisible = false;
            this.dgvAvisosfrmOrdenCompra.Size = new System.Drawing.Size(420, 380);
            this.dgvAvisosfrmOrdenCompra.TabIndex = 1;
            //
            // lblProveedorfrmOrdenCompra
            //
            this.lblProveedorfrmOrdenCompra.AutoSize = true;
            this.lblProveedorfrmOrdenCompra.Location = new System.Drawing.Point(460, 45);
            this.lblProveedorfrmOrdenCompra.Name = "lblProveedorfrmOrdenCompra";
            this.lblProveedorfrmOrdenCompra.TabIndex = 2;
            this.lblProveedorfrmOrdenCompra.Text = "Proveedor";
            //
            // cmbProveedorfrmOrdenCompra
            //
            this.cmbProveedorfrmOrdenCompra.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProveedorfrmOrdenCompra.Location = new System.Drawing.Point(560, 42);
            this.cmbProveedorfrmOrdenCompra.Name = "cmbProveedorfrmOrdenCompra";
            this.cmbProveedorfrmOrdenCompra.Size = new System.Drawing.Size(270, 24);
            this.cmbProveedorfrmOrdenCompra.TabIndex = 3;
            this.cmbProveedorfrmOrdenCompra.SelectedIndexChanged += new System.EventHandler(this.cmbProveedorfrmOrdenCompra_SelectedIndexChanged);
            //
            // btnNuevoProveedorfrmOrdenCompra
            //
            this.btnNuevoProveedorfrmOrdenCompra.BackColor = System.Drawing.Color.SandyBrown;
            this.btnNuevoProveedorfrmOrdenCompra.Location = new System.Drawing.Point(840, 38);
            this.btnNuevoProveedorfrmOrdenCompra.Name = "btnNuevoProveedorfrmOrdenCompra";
            this.btnNuevoProveedorfrmOrdenCompra.Size = new System.Drawing.Size(140, 32);
            this.btnNuevoProveedorfrmOrdenCompra.TabIndex = 4;
            this.btnNuevoProveedorfrmOrdenCompra.Text = "Nuevo proveedor";
            this.btnNuevoProveedorfrmOrdenCompra.UseVisualStyleBackColor = false;
            this.btnNuevoProveedorfrmOrdenCompra.Click += new System.EventHandler(this.btnNuevoProveedorfrmOrdenCompra_Click);
            //
            // lblInsumosProveedorfrmOrdenCompra
            //
            this.lblInsumosProveedorfrmOrdenCompra.AutoSize = true;
            this.lblInsumosProveedorfrmOrdenCompra.Location = new System.Drawing.Point(460, 80);
            this.lblInsumosProveedorfrmOrdenCompra.Name = "lblInsumosProveedorfrmOrdenCompra";
            this.lblInsumosProveedorfrmOrdenCompra.TabIndex = 5;
            this.lblInsumosProveedorfrmOrdenCompra.Text = "Insumos del proveedor (cantidad en unidades de compra)";
            //
            // dgvPedidofrmOrdenCompra
            //
            this.dgvPedidofrmOrdenCompra.AllowUserToAddRows = false;
            this.dgvPedidofrmOrdenCompra.AllowUserToDeleteRows = false;
            this.dgvPedidofrmOrdenCompra.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPedidofrmOrdenCompra.Location = new System.Drawing.Point(460, 105);
            this.dgvPedidofrmOrdenCompra.Name = "dgvPedidofrmOrdenCompra";
            this.dgvPedidofrmOrdenCompra.RowHeadersVisible = false;
            this.dgvPedidofrmOrdenCompra.Size = new System.Drawing.Size(520, 345);
            this.dgvPedidofrmOrdenCompra.TabIndex = 6;
            //
            // btnLimpiarfrmOrdenCompra
            //
            this.btnLimpiarfrmOrdenCompra.BackColor = System.Drawing.Color.SandyBrown;
            this.btnLimpiarfrmOrdenCompra.Location = new System.Drawing.Point(460, 465);
            this.btnLimpiarfrmOrdenCompra.Name = "btnLimpiarfrmOrdenCompra";
            this.btnLimpiarfrmOrdenCompra.Size = new System.Drawing.Size(140, 38);
            this.btnLimpiarfrmOrdenCompra.TabIndex = 7;
            this.btnLimpiarfrmOrdenCompra.Text = "Limpiar";
            this.btnLimpiarfrmOrdenCompra.UseVisualStyleBackColor = false;
            this.btnLimpiarfrmOrdenCompra.Click += new System.EventHandler(this.btnLimpiarfrmOrdenCompra_Click);
            //
            // btnConfirmarfrmOrdenCompra
            //
            this.btnConfirmarfrmOrdenCompra.BackColor = System.Drawing.Color.SandyBrown;
            this.btnConfirmarfrmOrdenCompra.Location = new System.Drawing.Point(740, 465);
            this.btnConfirmarfrmOrdenCompra.Name = "btnConfirmarfrmOrdenCompra";
            this.btnConfirmarfrmOrdenCompra.Size = new System.Drawing.Size(240, 38);
            this.btnConfirmarfrmOrdenCompra.TabIndex = 8;
            this.btnConfirmarfrmOrdenCompra.Text = "Generar orden";
            this.btnConfirmarfrmOrdenCompra.UseVisualStyleBackColor = false;
            this.btnConfirmarfrmOrdenCompra.Click += new System.EventHandler(this.btnConfirmarfrmOrdenCompra_Click);
            //
            // frmOrdenCompra
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.NavajoWhite;
            this.ClientSize = new System.Drawing.Size(1000, 520);
            this.Controls.Add(this.btnConfirmarfrmOrdenCompra);
            this.Controls.Add(this.btnLimpiarfrmOrdenCompra);
            this.Controls.Add(this.dgvPedidofrmOrdenCompra);
            this.Controls.Add(this.lblInsumosProveedorfrmOrdenCompra);
            this.Controls.Add(this.btnNuevoProveedorfrmOrdenCompra);
            this.Controls.Add(this.cmbProveedorfrmOrdenCompra);
            this.Controls.Add(this.lblProveedorfrmOrdenCompra);
            this.Controls.Add(this.dgvAvisosfrmOrdenCompra);
            this.Controls.Add(this.lblAvisosfrmOrdenCompra);
            this.Controls.Add(this.txtObservacionesfrmOrdenCompra);
            this.Controls.Add(this.lblObservacionesfrmOrdenCompra);
            this.Controls.Add(this.dgvObservadasfrmOrdenCompra);
            this.Controls.Add(this.rdbObservadasfrmOrdenCompra);
            this.Controls.Add(this.rdbNuevafrmOrdenCompra);
            this.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmOrdenCompra";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmOrdenCompra";
            this.Load += new System.EventHandler(this.frmOrdenCompra_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvisosfrmOrdenCompra)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvObservadasfrmOrdenCompra)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPedidofrmOrdenCompra)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAvisosfrmOrdenCompra;
        private System.Windows.Forms.DataGridView dgvAvisosfrmOrdenCompra;
        private System.Windows.Forms.Label lblProveedorfrmOrdenCompra;
        private System.Windows.Forms.ComboBox cmbProveedorfrmOrdenCompra;
        private System.Windows.Forms.Button btnNuevoProveedorfrmOrdenCompra;
        private System.Windows.Forms.Label lblInsumosProveedorfrmOrdenCompra;
        private System.Windows.Forms.DataGridView dgvPedidofrmOrdenCompra;
        private System.Windows.Forms.Button btnLimpiarfrmOrdenCompra;
        private System.Windows.Forms.Button btnConfirmarfrmOrdenCompra;
        private System.Windows.Forms.RadioButton rdbNuevafrmOrdenCompra;
        private System.Windows.Forms.RadioButton rdbObservadasfrmOrdenCompra;
        private System.Windows.Forms.DataGridView dgvObservadasfrmOrdenCompra;
        private System.Windows.Forms.Label lblObservacionesfrmOrdenCompra;
        private System.Windows.Forms.TextBox txtObservacionesfrmOrdenCompra;
    }
}
