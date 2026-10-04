namespace UI
{
    partial class frmRecepcion
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
            this.lblOrdenesfrmRecepcion = new System.Windows.Forms.Label();
            this.dgvOrdenesfrmRecepcion = new System.Windows.Forms.DataGridView();
            this.lblInsumosfrmRecepcion = new System.Windows.Forms.Label();
            this.dgvInsumosfrmRecepcion = new System.Windows.Forms.DataGridView();
            this.lblRemitofrmRecepcion = new System.Windows.Forms.Label();
            this.txtRemitofrmRecepcion = new System.Windows.Forms.TextBox();
            this.lblFacturafrmRecepcion = new System.Windows.Forms.Label();
            this.txtFacturafrmRecepcion = new System.Windows.Forms.TextBox();
            this.btnRegistrarRecepcionfrmRecepcion = new System.Windows.Forms.Button();
            this.lblReclamofrmRecepcion = new System.Windows.Forms.Label();
            this.dgvReclamofrmRecepcion = new System.Windows.Forms.DataGridView();
            this.btnRegistrarReclamofrmRecepcion = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrdenesfrmRecepcion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInsumosfrmRecepcion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReclamofrmRecepcion)).BeginInit();
            this.SuspendLayout();
            //
            // lblOrdenesfrmRecepcion
            //
            this.lblOrdenesfrmRecepcion.AutoSize = true;
            this.lblOrdenesfrmRecepcion.Location = new System.Drawing.Point(20, 15);
            this.lblOrdenesfrmRecepcion.Name = "lblOrdenesfrmRecepcion";
            this.lblOrdenesfrmRecepcion.TabIndex = 0;
            this.lblOrdenesfrmRecepcion.Text = "Órdenes para recibir";
            //
            // dgvOrdenesfrmRecepcion
            //
            this.dgvOrdenesfrmRecepcion.AllowUserToAddRows = false;
            this.dgvOrdenesfrmRecepcion.AllowUserToDeleteRows = false;
            this.dgvOrdenesfrmRecepcion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrdenesfrmRecepcion.Location = new System.Drawing.Point(20, 40);
            this.dgvOrdenesfrmRecepcion.MultiSelect = false;
            this.dgvOrdenesfrmRecepcion.Name = "dgvOrdenesfrmRecepcion";
            this.dgvOrdenesfrmRecepcion.ReadOnly = true;
            this.dgvOrdenesfrmRecepcion.RowHeadersVisible = false;
            this.dgvOrdenesfrmRecepcion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrdenesfrmRecepcion.Size = new System.Drawing.Size(360, 300);
            this.dgvOrdenesfrmRecepcion.TabIndex = 1;
            this.dgvOrdenesfrmRecepcion.SelectionChanged += new System.EventHandler(this.dgvOrdenesfrmRecepcion_SelectionChanged);
            //
            // lblInsumosfrmRecepcion
            //
            this.lblInsumosfrmRecepcion.AutoSize = true;
            this.lblInsumosfrmRecepcion.Location = new System.Drawing.Point(400, 15);
            this.lblInsumosfrmRecepcion.Name = "lblInsumosfrmRecepcion";
            this.lblInsumosfrmRecepcion.TabIndex = 2;
            this.lblInsumosfrmRecepcion.Text = "Insumos de la orden (cantidades en unidades de compra)";
            //
            // dgvInsumosfrmRecepcion
            //
            this.dgvInsumosfrmRecepcion.AllowUserToAddRows = false;
            this.dgvInsumosfrmRecepcion.AllowUserToDeleteRows = false;
            this.dgvInsumosfrmRecepcion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInsumosfrmRecepcion.Location = new System.Drawing.Point(400, 40);
            this.dgvInsumosfrmRecepcion.Name = "dgvInsumosfrmRecepcion";
            this.dgvInsumosfrmRecepcion.RowHeadersVisible = false;
            this.dgvInsumosfrmRecepcion.Size = new System.Drawing.Size(640, 250);
            this.dgvInsumosfrmRecepcion.TabIndex = 3;
            //
            // lblRemitofrmRecepcion
            //
            this.lblRemitofrmRecepcion.AutoSize = true;
            this.lblRemitofrmRecepcion.Location = new System.Drawing.Point(400, 305);
            this.lblRemitofrmRecepcion.Name = "lblRemitofrmRecepcion";
            this.lblRemitofrmRecepcion.TabIndex = 4;
            this.lblRemitofrmRecepcion.Text = "N° de remito";
            //
            // txtRemitofrmRecepcion
            //
            this.txtRemitofrmRecepcion.Location = new System.Drawing.Point(510, 302);
            this.txtRemitofrmRecepcion.MaxLength = 30;
            this.txtRemitofrmRecepcion.Name = "txtRemitofrmRecepcion";
            this.txtRemitofrmRecepcion.Size = new System.Drawing.Size(160, 23);
            this.txtRemitofrmRecepcion.TabIndex = 5;
            //
            // lblFacturafrmRecepcion
            //
            this.lblFacturafrmRecepcion.AutoSize = true;
            this.lblFacturafrmRecepcion.Location = new System.Drawing.Point(690, 305);
            this.lblFacturafrmRecepcion.Name = "lblFacturafrmRecepcion";
            this.lblFacturafrmRecepcion.TabIndex = 6;
            this.lblFacturafrmRecepcion.Text = "N° de factura";
            //
            // txtFacturafrmRecepcion
            //
            this.txtFacturafrmRecepcion.Location = new System.Drawing.Point(800, 302);
            this.txtFacturafrmRecepcion.MaxLength = 30;
            this.txtFacturafrmRecepcion.Name = "txtFacturafrmRecepcion";
            this.txtFacturafrmRecepcion.Size = new System.Drawing.Size(240, 23);
            this.txtFacturafrmRecepcion.TabIndex = 7;
            //
            // btnRegistrarRecepcionfrmRecepcion
            //
            this.btnRegistrarRecepcionfrmRecepcion.BackColor = System.Drawing.Color.SandyBrown;
            this.btnRegistrarRecepcionfrmRecepcion.Location = new System.Drawing.Point(800, 337);
            this.btnRegistrarRecepcionfrmRecepcion.Name = "btnRegistrarRecepcionfrmRecepcion";
            this.btnRegistrarRecepcionfrmRecepcion.Size = new System.Drawing.Size(240, 36);
            this.btnRegistrarRecepcionfrmRecepcion.TabIndex = 8;
            this.btnRegistrarRecepcionfrmRecepcion.Text = "Registrar recepción";
            this.btnRegistrarRecepcionfrmRecepcion.UseVisualStyleBackColor = false;
            this.btnRegistrarRecepcionfrmRecepcion.Click += new System.EventHandler(this.btnRegistrarRecepcionfrmRecepcion_Click);
            //
            // lblReclamofrmRecepcion
            //
            this.lblReclamofrmRecepcion.AutoSize = true;
            this.lblReclamofrmRecepcion.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReclamofrmRecepcion.Location = new System.Drawing.Point(20, 385);
            this.lblReclamofrmRecepcion.Name = "lblReclamofrmRecepcion";
            this.lblReclamofrmRecepcion.TabIndex = 9;
            this.lblReclamofrmRecepcion.Text = "Reclamo al proveedor: indique el motivo de cada diferencia";
            this.lblReclamofrmRecepcion.Visible = false;
            //
            // dgvReclamofrmRecepcion
            //
            this.dgvReclamofrmRecepcion.AllowUserToAddRows = false;
            this.dgvReclamofrmRecepcion.AllowUserToDeleteRows = false;
            this.dgvReclamofrmRecepcion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReclamofrmRecepcion.Location = new System.Drawing.Point(20, 410);
            this.dgvReclamofrmRecepcion.Name = "dgvReclamofrmRecepcion";
            this.dgvReclamofrmRecepcion.RowHeadersVisible = false;
            this.dgvReclamofrmRecepcion.Size = new System.Drawing.Size(770, 140);
            this.dgvReclamofrmRecepcion.TabIndex = 10;
            this.dgvReclamofrmRecepcion.Visible = false;
            //
            // btnRegistrarReclamofrmRecepcion
            //
            this.btnRegistrarReclamofrmRecepcion.BackColor = System.Drawing.Color.SandyBrown;
            this.btnRegistrarReclamofrmRecepcion.Location = new System.Drawing.Point(800, 410);
            this.btnRegistrarReclamofrmRecepcion.Name = "btnRegistrarReclamofrmRecepcion";
            this.btnRegistrarReclamofrmRecepcion.Size = new System.Drawing.Size(240, 36);
            this.btnRegistrarReclamofrmRecepcion.TabIndex = 11;
            this.btnRegistrarReclamofrmRecepcion.Text = "Registrar reclamo";
            this.btnRegistrarReclamofrmRecepcion.UseVisualStyleBackColor = false;
            this.btnRegistrarReclamofrmRecepcion.Visible = false;
            this.btnRegistrarReclamofrmRecepcion.Click += new System.EventHandler(this.btnRegistrarReclamofrmRecepcion_Click);
            //
            // frmRecepcion
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.NavajoWhite;
            this.ClientSize = new System.Drawing.Size(1060, 570);
            this.Controls.Add(this.btnRegistrarReclamofrmRecepcion);
            this.Controls.Add(this.dgvReclamofrmRecepcion);
            this.Controls.Add(this.lblReclamofrmRecepcion);
            this.Controls.Add(this.btnRegistrarRecepcionfrmRecepcion);
            this.Controls.Add(this.txtFacturafrmRecepcion);
            this.Controls.Add(this.lblFacturafrmRecepcion);
            this.Controls.Add(this.txtRemitofrmRecepcion);
            this.Controls.Add(this.lblRemitofrmRecepcion);
            this.Controls.Add(this.dgvInsumosfrmRecepcion);
            this.Controls.Add(this.lblInsumosfrmRecepcion);
            this.Controls.Add(this.dgvOrdenesfrmRecepcion);
            this.Controls.Add(this.lblOrdenesfrmRecepcion);
            this.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmRecepcion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmRecepcion";
            this.Load += new System.EventHandler(this.frmRecepcion_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmRecepcion_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrdenesfrmRecepcion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInsumosfrmRecepcion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReclamofrmRecepcion)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblOrdenesfrmRecepcion;
        private System.Windows.Forms.DataGridView dgvOrdenesfrmRecepcion;
        private System.Windows.Forms.Label lblInsumosfrmRecepcion;
        private System.Windows.Forms.DataGridView dgvInsumosfrmRecepcion;
        private System.Windows.Forms.Label lblRemitofrmRecepcion;
        private System.Windows.Forms.TextBox txtRemitofrmRecepcion;
        private System.Windows.Forms.Label lblFacturafrmRecepcion;
        private System.Windows.Forms.TextBox txtFacturafrmRecepcion;
        private System.Windows.Forms.Button btnRegistrarRecepcionfrmRecepcion;
        private System.Windows.Forms.Label lblReclamofrmRecepcion;
        private System.Windows.Forms.DataGridView dgvReclamofrmRecepcion;
        private System.Windows.Forms.Button btnRegistrarReclamofrmRecepcion;
    }
}
