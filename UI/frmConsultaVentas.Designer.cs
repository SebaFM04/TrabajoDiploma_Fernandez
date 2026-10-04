namespace UI
{
    partial class frmConsultaVentas
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
            this.lblDesdefrmConsultaVentas = new System.Windows.Forms.Label();
            this.dtpDesdefrmConsultaVentas = new System.Windows.Forms.DateTimePicker();
            this.lblHastafrmConsultaVentas = new System.Windows.Forms.Label();
            this.dtpHastafrmConsultaVentas = new System.Windows.Forms.DateTimePicker();
            this.btnBuscarfrmConsultaVentas = new System.Windows.Forms.Button();
            this.dgvVentasfrmConsultaVentas = new System.Windows.Forms.DataGridView();
            this.lblTotalesfrmConsultaVentas = new System.Windows.Forms.Label();
            this.lblDetallefrmConsultaVentas = new System.Windows.Forms.Label();
            this.dgvDetallefrmConsultaVentas = new System.Windows.Forms.DataGridView();
            this.btnVerFacturafrmConsultaVentas = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentasfrmConsultaVentas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetallefrmConsultaVentas)).BeginInit();
            this.SuspendLayout();
            //
            // lblDesdefrmConsultaVentas
            //
            this.lblDesdefrmConsultaVentas.AutoSize = true;
            this.lblDesdefrmConsultaVentas.Location = new System.Drawing.Point(20, 20);
            this.lblDesdefrmConsultaVentas.Name = "lblDesdefrmConsultaVentas";
            this.lblDesdefrmConsultaVentas.TabIndex = 0;
            this.lblDesdefrmConsultaVentas.Text = "Desde";
            //
            // dtpDesdefrmConsultaVentas
            //
            this.dtpDesdefrmConsultaVentas.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDesdefrmConsultaVentas.Location = new System.Drawing.Point(80, 16);
            this.dtpDesdefrmConsultaVentas.Name = "dtpDesdefrmConsultaVentas";
            this.dtpDesdefrmConsultaVentas.Size = new System.Drawing.Size(130, 23);
            this.dtpDesdefrmConsultaVentas.TabIndex = 1;
            //
            // lblHastafrmConsultaVentas
            //
            this.lblHastafrmConsultaVentas.AutoSize = true;
            this.lblHastafrmConsultaVentas.Location = new System.Drawing.Point(230, 20);
            this.lblHastafrmConsultaVentas.Name = "lblHastafrmConsultaVentas";
            this.lblHastafrmConsultaVentas.TabIndex = 2;
            this.lblHastafrmConsultaVentas.Text = "Hasta";
            //
            // dtpHastafrmConsultaVentas
            //
            this.dtpHastafrmConsultaVentas.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHastafrmConsultaVentas.Location = new System.Drawing.Point(290, 16);
            this.dtpHastafrmConsultaVentas.Name = "dtpHastafrmConsultaVentas";
            this.dtpHastafrmConsultaVentas.Size = new System.Drawing.Size(130, 23);
            this.dtpHastafrmConsultaVentas.TabIndex = 3;
            //
            // btnBuscarfrmConsultaVentas
            //
            this.btnBuscarfrmConsultaVentas.BackColor = System.Drawing.Color.SandyBrown;
            this.btnBuscarfrmConsultaVentas.Location = new System.Drawing.Point(440, 11);
            this.btnBuscarfrmConsultaVentas.Name = "btnBuscarfrmConsultaVentas";
            this.btnBuscarfrmConsultaVentas.Size = new System.Drawing.Size(120, 32);
            this.btnBuscarfrmConsultaVentas.TabIndex = 4;
            this.btnBuscarfrmConsultaVentas.Text = "Buscar";
            this.btnBuscarfrmConsultaVentas.UseVisualStyleBackColor = false;
            this.btnBuscarfrmConsultaVentas.Click += new System.EventHandler(this.btnBuscarfrmConsultaVentas_Click);
            //
            // dgvVentasfrmConsultaVentas
            //
            this.dgvVentasfrmConsultaVentas.AllowUserToAddRows = false;
            this.dgvVentasfrmConsultaVentas.AllowUserToDeleteRows = false;
            this.dgvVentasfrmConsultaVentas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVentasfrmConsultaVentas.Location = new System.Drawing.Point(20, 55);
            this.dgvVentasfrmConsultaVentas.MultiSelect = false;
            this.dgvVentasfrmConsultaVentas.Name = "dgvVentasfrmConsultaVentas";
            this.dgvVentasfrmConsultaVentas.ReadOnly = true;
            this.dgvVentasfrmConsultaVentas.RowHeadersVisible = false;
            this.dgvVentasfrmConsultaVentas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVentasfrmConsultaVentas.Size = new System.Drawing.Size(960, 250);
            this.dgvVentasfrmConsultaVentas.TabIndex = 5;
            this.dgvVentasfrmConsultaVentas.SelectionChanged += new System.EventHandler(this.dgvVentasfrmConsultaVentas_SelectionChanged);
            this.dgvVentasfrmConsultaVentas.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVentasfrmConsultaVentas_CellClick);
            this.dgvVentasfrmConsultaVentas.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVentasfrmConsultaVentas_RowEnter);
            //
            // lblTotalesfrmConsultaVentas
            //
            this.lblTotalesfrmConsultaVentas.AutoSize = true;
            this.lblTotalesfrmConsultaVentas.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalesfrmConsultaVentas.Location = new System.Drawing.Point(20, 312);
            this.lblTotalesfrmConsultaVentas.Name = "lblTotalesfrmConsultaVentas";
            this.lblTotalesfrmConsultaVentas.TabIndex = 6;
            this.lblTotalesfrmConsultaVentas.Text = "-";
            //
            // lblDetallefrmConsultaVentas
            //
            this.lblDetallefrmConsultaVentas.AutoSize = true;
            this.lblDetallefrmConsultaVentas.Location = new System.Drawing.Point(20, 340);
            this.lblDetallefrmConsultaVentas.Name = "lblDetallefrmConsultaVentas";
            this.lblDetallefrmConsultaVentas.TabIndex = 7;
            this.lblDetallefrmConsultaVentas.Text = "Detalle de la venta";
            //
            // dgvDetallefrmConsultaVentas
            //
            this.dgvDetallefrmConsultaVentas.AllowUserToAddRows = false;
            this.dgvDetallefrmConsultaVentas.AllowUserToDeleteRows = false;
            this.dgvDetallefrmConsultaVentas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetallefrmConsultaVentas.Location = new System.Drawing.Point(20, 362);
            this.dgvDetallefrmConsultaVentas.Name = "dgvDetallefrmConsultaVentas";
            this.dgvDetallefrmConsultaVentas.ReadOnly = true;
            this.dgvDetallefrmConsultaVentas.RowHeadersVisible = false;
            this.dgvDetallefrmConsultaVentas.Size = new System.Drawing.Size(700, 160);
            this.dgvDetallefrmConsultaVentas.TabIndex = 8;
            //
            // btnVerFacturafrmConsultaVentas
            //
            this.btnVerFacturafrmConsultaVentas.BackColor = System.Drawing.Color.SandyBrown;
            this.btnVerFacturafrmConsultaVentas.Location = new System.Drawing.Point(740, 362);
            this.btnVerFacturafrmConsultaVentas.Name = "btnVerFacturafrmConsultaVentas";
            this.btnVerFacturafrmConsultaVentas.Size = new System.Drawing.Size(240, 40);
            this.btnVerFacturafrmConsultaVentas.TabIndex = 9;
            this.btnVerFacturafrmConsultaVentas.Text = "Ver factura (PDF)";
            this.btnVerFacturafrmConsultaVentas.UseVisualStyleBackColor = false;
            this.btnVerFacturafrmConsultaVentas.Click += new System.EventHandler(this.btnVerFacturafrmConsultaVentas_Click);
            //
            // frmConsultaVentas
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.NavajoWhite;
            this.ClientSize = new System.Drawing.Size(1000, 540);
            this.Controls.Add(this.btnVerFacturafrmConsultaVentas);
            this.Controls.Add(this.dgvDetallefrmConsultaVentas);
            this.Controls.Add(this.lblDetallefrmConsultaVentas);
            this.Controls.Add(this.lblTotalesfrmConsultaVentas);
            this.Controls.Add(this.dgvVentasfrmConsultaVentas);
            this.Controls.Add(this.btnBuscarfrmConsultaVentas);
            this.Controls.Add(this.dtpHastafrmConsultaVentas);
            this.Controls.Add(this.lblHastafrmConsultaVentas);
            this.Controls.Add(this.dtpDesdefrmConsultaVentas);
            this.Controls.Add(this.lblDesdefrmConsultaVentas);
            this.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmConsultaVentas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmConsultaVentas";
            this.Load += new System.EventHandler(this.frmConsultaVentas_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentasfrmConsultaVentas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetallefrmConsultaVentas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDesdefrmConsultaVentas;
        private System.Windows.Forms.DateTimePicker dtpDesdefrmConsultaVentas;
        private System.Windows.Forms.Label lblHastafrmConsultaVentas;
        private System.Windows.Forms.DateTimePicker dtpHastafrmConsultaVentas;
        private System.Windows.Forms.Button btnBuscarfrmConsultaVentas;
        private System.Windows.Forms.DataGridView dgvVentasfrmConsultaVentas;
        private System.Windows.Forms.Label lblTotalesfrmConsultaVentas;
        private System.Windows.Forms.Label lblDetallefrmConsultaVentas;
        private System.Windows.Forms.DataGridView dgvDetallefrmConsultaVentas;
        private System.Windows.Forms.Button btnVerFacturafrmConsultaVentas;
    }
}
