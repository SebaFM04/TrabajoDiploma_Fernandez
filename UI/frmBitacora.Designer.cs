namespace UI
{
    partial class frmBitacora
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
            this.lblUsuarioBitacora = new System.Windows.Forms.Label();
            this.lblDesdeBitacora = new System.Windows.Forms.Label();
            this.lblHastaBitacora = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.dtpFechaDesde = new System.Windows.Forms.DateTimePicker();
            this.dtpFechaHasta = new System.Windows.Forms.DateTimePicker();
            this.btnBuscarBitacora = new System.Windows.Forms.Button();
            this.btnLimpiarBitacora = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblUsuarioBitacora
            // 
            this.lblUsuarioBitacora.AutoSize = true;
            this.lblUsuarioBitacora.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsuarioBitacora.Location = new System.Drawing.Point(14, 16);
            this.lblUsuarioBitacora.Name = "lblUsuarioBitacora";
            this.lblUsuarioBitacora.Size = new System.Drawing.Size(60, 18);
            this.lblUsuarioBitacora.TabIndex = 0;
            this.lblUsuarioBitacora.Text = "Usuario";
            // 
            // lblDesdeBitacora
            // 
            this.lblDesdeBitacora.AutoSize = true;
            this.lblDesdeBitacora.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDesdeBitacora.Location = new System.Drawing.Point(14, 50);
            this.lblDesdeBitacora.Name = "lblDesdeBitacora";
            this.lblDesdeBitacora.Size = new System.Drawing.Size(51, 18);
            this.lblDesdeBitacora.TabIndex = 1;
            this.lblDesdeBitacora.Text = "Desde";
            // 
            // lblHastaBitacora
            // 
            this.lblHastaBitacora.AutoSize = true;
            this.lblHastaBitacora.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHastaBitacora.Location = new System.Drawing.Point(14, 79);
            this.lblHastaBitacora.Name = "lblHastaBitacora";
            this.lblHastaBitacora.Size = new System.Drawing.Size(47, 18);
            this.lblHastaBitacora.TabIndex = 2;
            this.lblHastaBitacora.Text = "Hasta";
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(74, 10);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(155, 25);
            this.comboBox1.TabIndex = 3;
            // 
            // dtpFechaDesde
            // 
            this.dtpFechaDesde.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFechaDesde.Location = new System.Drawing.Point(74, 45);
            this.dtpFechaDesde.Name = "dtpFechaDesde";
            this.dtpFechaDesde.Size = new System.Drawing.Size(258, 24);
            this.dtpFechaDesde.TabIndex = 4;
            // 
            // dtpFechaHasta
            // 
            this.dtpFechaHasta.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFechaHasta.Location = new System.Drawing.Point(74, 74);
            this.dtpFechaHasta.Name = "dtpFechaHasta";
            this.dtpFechaHasta.Size = new System.Drawing.Size(258, 24);
            this.dtpFechaHasta.TabIndex = 5;
            // 
            // btnBuscarBitacora
            // 
            this.btnBuscarBitacora.BackColor = System.Drawing.Color.SandyBrown;
            this.btnBuscarBitacora.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuscarBitacora.Location = new System.Drawing.Point(368, 10);
            this.btnBuscarBitacora.Name = "btnBuscarBitacora";
            this.btnBuscarBitacora.Size = new System.Drawing.Size(101, 36);
            this.btnBuscarBitacora.TabIndex = 6;
            this.btnBuscarBitacora.Text = "Buscar";
            this.btnBuscarBitacora.UseVisualStyleBackColor = false;
            this.btnBuscarBitacora.Click += new System.EventHandler(this.btnBuscarBitacora_Click);
            // 
            // btnLimpiarBitacora
            // 
            this.btnLimpiarBitacora.BackColor = System.Drawing.Color.SandyBrown;
            this.btnLimpiarBitacora.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimpiarBitacora.Location = new System.Drawing.Point(368, 62);
            this.btnLimpiarBitacora.Name = "btnLimpiarBitacora";
            this.btnLimpiarBitacora.Size = new System.Drawing.Size(101, 37);
            this.btnLimpiarBitacora.TabIndex = 7;
            this.btnLimpiarBitacora.Text = "Limpiar";
            this.btnLimpiarBitacora.UseVisualStyleBackColor = false;
            this.btnLimpiarBitacora.Click += new System.EventHandler(this.btnLimpiarBitacora_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(17, 105);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(698, 297);
            this.dataGridView1.TabIndex = 8;
            // 
            // frmBitacora
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.NavajoWhite;
            this.ClientSize = new System.Drawing.Size(727, 450);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnLimpiarBitacora);
            this.Controls.Add(this.btnBuscarBitacora);
            this.Controls.Add(this.dtpFechaHasta);
            this.Controls.Add(this.dtpFechaDesde);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.lblHastaBitacora);
            this.Controls.Add(this.lblDesdeBitacora);
            this.Controls.Add(this.lblUsuarioBitacora);
            this.Name = "frmBitacora";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmBitácora";
            this.Load += new System.EventHandler(this.frmBitacora_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUsuarioBitacora;
        private System.Windows.Forms.Label lblDesdeBitacora;
        private System.Windows.Forms.Label lblHastaBitacora;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.DateTimePicker dtpFechaDesde;
        private System.Windows.Forms.DateTimePicker dtpFechaHasta;
        private System.Windows.Forms.Button btnBuscarBitacora;
        private System.Windows.Forms.Button btnLimpiarBitacora;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}