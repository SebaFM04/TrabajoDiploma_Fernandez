namespace UI
{
    partial class frmEntrega
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
            this.lblValefrmEntrega = new System.Windows.Forms.Label();
            this.txtValefrmEntrega = new System.Windows.Forms.TextBox();
            this.btnBuscarfrmEntrega = new System.Windows.Forms.Button();
            this.lblBebidasfrmEntrega = new System.Windows.Forms.Label();
            this.dgvBebidasfrmEntrega = new System.Windows.Forms.DataGridView();
            this.lblRecetasfrmEntrega = new System.Windows.Forms.Label();
            this.dgvRecetasfrmEntrega = new System.Windows.Forms.DataGridView();
            this.btnConfirmarEntregafrmEntrega = new System.Windows.Forms.Button();
            this.btnLimpiarfrmEntrega = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBebidasfrmEntrega)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecetasfrmEntrega)).BeginInit();
            this.SuspendLayout();
            //
            // lblValefrmEntrega
            //
            this.lblValefrmEntrega.AutoSize = true;
            this.lblValefrmEntrega.Location = new System.Drawing.Point(20, 20);
            this.lblValefrmEntrega.Name = "lblValefrmEntrega";
            this.lblValefrmEntrega.TabIndex = 0;
            this.lblValefrmEntrega.Text = "N° de vale";
            //
            // txtValefrmEntrega
            //
            this.txtValefrmEntrega.Location = new System.Drawing.Point(120, 17);
            this.txtValefrmEntrega.MaxLength = 9;
            this.txtValefrmEntrega.Name = "txtValefrmEntrega";
            this.txtValefrmEntrega.Size = new System.Drawing.Size(120, 23);
            this.txtValefrmEntrega.TabIndex = 1;
            this.txtValefrmEntrega.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtValefrmEntrega_KeyDown);
            //
            // btnBuscarfrmEntrega
            //
            this.btnBuscarfrmEntrega.BackColor = System.Drawing.Color.SandyBrown;
            this.btnBuscarfrmEntrega.Location = new System.Drawing.Point(255, 12);
            this.btnBuscarfrmEntrega.Name = "btnBuscarfrmEntrega";
            this.btnBuscarfrmEntrega.Size = new System.Drawing.Size(160, 34);
            this.btnBuscarfrmEntrega.TabIndex = 2;
            this.btnBuscarfrmEntrega.Text = "Validar vale";
            this.btnBuscarfrmEntrega.UseVisualStyleBackColor = false;
            this.btnBuscarfrmEntrega.Click += new System.EventHandler(this.btnBuscarfrmEntrega_Click);
            //
            // lblBebidasfrmEntrega
            //
            this.lblBebidasfrmEntrega.AutoSize = true;
            this.lblBebidasfrmEntrega.Location = new System.Drawing.Point(20, 60);
            this.lblBebidasfrmEntrega.Name = "lblBebidasfrmEntrega";
            this.lblBebidasfrmEntrega.TabIndex = 3;
            this.lblBebidasfrmEntrega.Text = "Bebidas de la venta";
            //
            // dgvBebidasfrmEntrega
            //
            this.dgvBebidasfrmEntrega.AllowUserToAddRows = false;
            this.dgvBebidasfrmEntrega.AllowUserToDeleteRows = false;
            this.dgvBebidasfrmEntrega.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBebidasfrmEntrega.Location = new System.Drawing.Point(20, 85);
            this.dgvBebidasfrmEntrega.Name = "dgvBebidasfrmEntrega";
            this.dgvBebidasfrmEntrega.ReadOnly = true;
            this.dgvBebidasfrmEntrega.RowHeadersVisible = false;
            this.dgvBebidasfrmEntrega.Size = new System.Drawing.Size(360, 250);
            this.dgvBebidasfrmEntrega.TabIndex = 4;
            //
            // lblRecetasfrmEntrega
            //
            this.lblRecetasfrmEntrega.AutoSize = true;
            this.lblRecetasfrmEntrega.Location = new System.Drawing.Point(400, 60);
            this.lblRecetasfrmEntrega.Name = "lblRecetasfrmEntrega";
            this.lblRecetasfrmEntrega.TabIndex = 5;
            this.lblRecetasfrmEntrega.Text = "Recetas escaladas al tamaño vendido";
            //
            // dgvRecetasfrmEntrega
            //
            this.dgvRecetasfrmEntrega.AllowUserToAddRows = false;
            this.dgvRecetasfrmEntrega.AllowUserToDeleteRows = false;
            this.dgvRecetasfrmEntrega.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecetasfrmEntrega.Location = new System.Drawing.Point(400, 85);
            this.dgvRecetasfrmEntrega.Name = "dgvRecetasfrmEntrega";
            this.dgvRecetasfrmEntrega.ReadOnly = true;
            this.dgvRecetasfrmEntrega.RowHeadersVisible = false;
            this.dgvRecetasfrmEntrega.Size = new System.Drawing.Size(440, 250);
            this.dgvRecetasfrmEntrega.TabIndex = 6;
            //
            // btnConfirmarEntregafrmEntrega
            //
            this.btnConfirmarEntregafrmEntrega.BackColor = System.Drawing.Color.SandyBrown;
            this.btnConfirmarEntregafrmEntrega.Location = new System.Drawing.Point(620, 350);
            this.btnConfirmarEntregafrmEntrega.Name = "btnConfirmarEntregafrmEntrega";
            this.btnConfirmarEntregafrmEntrega.Size = new System.Drawing.Size(220, 40);
            this.btnConfirmarEntregafrmEntrega.TabIndex = 7;
            this.btnConfirmarEntregafrmEntrega.Text = "Confirmar entrega";
            this.btnConfirmarEntregafrmEntrega.UseVisualStyleBackColor = false;
            this.btnConfirmarEntregafrmEntrega.Click += new System.EventHandler(this.btnConfirmarEntregafrmEntrega_Click);
            //
            // btnLimpiarfrmEntrega
            //
            this.btnLimpiarfrmEntrega.BackColor = System.Drawing.Color.SandyBrown;
            this.btnLimpiarfrmEntrega.Location = new System.Drawing.Point(20, 350);
            this.btnLimpiarfrmEntrega.Name = "btnLimpiarfrmEntrega";
            this.btnLimpiarfrmEntrega.Size = new System.Drawing.Size(160, 40);
            this.btnLimpiarfrmEntrega.TabIndex = 8;
            this.btnLimpiarfrmEntrega.Text = "Otro vale";
            this.btnLimpiarfrmEntrega.UseVisualStyleBackColor = false;
            this.btnLimpiarfrmEntrega.Click += new System.EventHandler(this.btnLimpiarfrmEntrega_Click);
            //
            // frmEntrega
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.NavajoWhite;
            this.ClientSize = new System.Drawing.Size(860, 410);
            this.Controls.Add(this.btnLimpiarfrmEntrega);
            this.Controls.Add(this.btnConfirmarEntregafrmEntrega);
            this.Controls.Add(this.dgvRecetasfrmEntrega);
            this.Controls.Add(this.lblRecetasfrmEntrega);
            this.Controls.Add(this.dgvBebidasfrmEntrega);
            this.Controls.Add(this.lblBebidasfrmEntrega);
            this.Controls.Add(this.btnBuscarfrmEntrega);
            this.Controls.Add(this.txtValefrmEntrega);
            this.Controls.Add(this.lblValefrmEntrega);
            this.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmEntrega";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmEntrega";
            this.Load += new System.EventHandler(this.frmEntrega_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBebidasfrmEntrega)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecetasfrmEntrega)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblValefrmEntrega;
        private System.Windows.Forms.TextBox txtValefrmEntrega;
        private System.Windows.Forms.Button btnBuscarfrmEntrega;
        private System.Windows.Forms.Label lblBebidasfrmEntrega;
        private System.Windows.Forms.DataGridView dgvBebidasfrmEntrega;
        private System.Windows.Forms.Label lblRecetasfrmEntrega;
        private System.Windows.Forms.DataGridView dgvRecetasfrmEntrega;
        private System.Windows.Forms.Button btnConfirmarEntregafrmEntrega;
        private System.Windows.Forms.Button btnLimpiarfrmEntrega;
    }
}
