namespace UI
{
    partial class frmSimuladorVentas
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
            this.lblAyudafrmSimuladorVentas = new System.Windows.Forms.Label();
            this.lblMesesfrmSimuladorVentas = new System.Windows.Forms.Label();
            this.nudMesesfrmSimuladorVentas = new System.Windows.Forms.NumericUpDown();
            this.lblVentasNochefrmSimuladorVentas = new System.Windows.Forms.Label();
            this.nudVentasNochefrmSimuladorVentas = new System.Windows.Forms.NumericUpDown();
            this.lblSemillafrmSimuladorVentas = new System.Windows.Forms.Label();
            this.nudSemillafrmSimuladorVentas = new System.Windows.Forms.NumericUpDown();
            this.chkPromocionEjemplofrmSimuladorVentas = new System.Windows.Forms.CheckBox();
            this.btnGenerarfrmSimuladorVentas = new System.Windows.Forms.Button();
            this.btnBorrarfrmSimuladorVentas = new System.Windows.Forms.Button();
            this.lblEstadofrmSimuladorVentas = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudMesesfrmSimuladorVentas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVentasNochefrmSimuladorVentas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSemillafrmSimuladorVentas)).BeginInit();
            this.SuspendLayout();
            //
            // lblAyudafrmSimuladorVentas
            //
            this.lblAyudafrmSimuladorVentas.Location = new System.Drawing.Point(20, 15);
            this.lblAyudafrmSimuladorVentas.Name = "lblAyudafrmSimuladorVentas";
            this.lblAyudafrmSimuladorVentas.Size = new System.Drawing.Size(520, 55);
            this.lblAyudafrmSimuladorVentas.TabIndex = 0;
            this.lblAyudafrmSimuladorVentas.Text = "Genera ventas de los meses anteriores para probar el análisis de rentabilidad.";
            //
            // lblMesesfrmSimuladorVentas
            //
            this.lblMesesfrmSimuladorVentas.AutoSize = true;
            this.lblMesesfrmSimuladorVentas.Location = new System.Drawing.Point(20, 85);
            this.lblMesesfrmSimuladorVentas.Name = "lblMesesfrmSimuladorVentas";
            this.lblMesesfrmSimuladorVentas.TabIndex = 1;
            this.lblMesesfrmSimuladorVentas.Text = "Meses hacia atrás";
            //
            // nudMesesfrmSimuladorVentas
            //
            this.nudMesesfrmSimuladorVentas.Location = new System.Drawing.Point(260, 83);
            this.nudMesesfrmSimuladorVentas.Maximum = new decimal(new int[] { 24, 0, 0, 0 });
            this.nudMesesfrmSimuladorVentas.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudMesesfrmSimuladorVentas.Name = "nudMesesfrmSimuladorVentas";
            this.nudMesesfrmSimuladorVentas.Size = new System.Drawing.Size(120, 23);
            this.nudMesesfrmSimuladorVentas.TabIndex = 2;
            this.nudMesesfrmSimuladorVentas.Value = new decimal(new int[] { 12, 0, 0, 0 });
            //
            // lblVentasNochefrmSimuladorVentas
            //
            this.lblVentasNochefrmSimuladorVentas.AutoSize = true;
            this.lblVentasNochefrmSimuladorVentas.Location = new System.Drawing.Point(20, 120);
            this.lblVentasNochefrmSimuladorVentas.Name = "lblVentasNochefrmSimuladorVentas";
            this.lblVentasNochefrmSimuladorVentas.TabIndex = 3;
            this.lblVentasNochefrmSimuladorVentas.Text = "Ventas promedio por noche";
            //
            // nudVentasNochefrmSimuladorVentas
            //
            this.nudVentasNochefrmSimuladorVentas.Location = new System.Drawing.Point(260, 118);
            this.nudVentasNochefrmSimuladorVentas.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            this.nudVentasNochefrmSimuladorVentas.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudVentasNochefrmSimuladorVentas.Name = "nudVentasNochefrmSimuladorVentas";
            this.nudVentasNochefrmSimuladorVentas.Size = new System.Drawing.Size(120, 23);
            this.nudVentasNochefrmSimuladorVentas.TabIndex = 4;
            this.nudVentasNochefrmSimuladorVentas.Value = new decimal(new int[] { 30, 0, 0, 0 });
            //
            // lblSemillafrmSimuladorVentas
            //
            this.lblSemillafrmSimuladorVentas.AutoSize = true;
            this.lblSemillafrmSimuladorVentas.Location = new System.Drawing.Point(20, 155);
            this.lblSemillafrmSimuladorVentas.Name = "lblSemillafrmSimuladorVentas";
            this.lblSemillafrmSimuladorVentas.TabIndex = 5;
            this.lblSemillafrmSimuladorVentas.Text = "Semilla (misma semilla = mismas ventas)";
            //
            // nudSemillafrmSimuladorVentas
            //
            this.nudSemillafrmSimuladorVentas.Location = new System.Drawing.Point(260, 153);
            this.nudSemillafrmSimuladorVentas.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            this.nudSemillafrmSimuladorVentas.Name = "nudSemillafrmSimuladorVentas";
            this.nudSemillafrmSimuladorVentas.Size = new System.Drawing.Size(120, 23);
            this.nudSemillafrmSimuladorVentas.TabIndex = 6;
            this.nudSemillafrmSimuladorVentas.Value = new decimal(new int[] { 2026, 0, 0, 0 });
            //
            // chkPromocionEjemplofrmSimuladorVentas
            //
            this.chkPromocionEjemplofrmSimuladorVentas.AutoSize = true;
            this.chkPromocionEjemplofrmSimuladorVentas.Checked = true;
            this.chkPromocionEjemplofrmSimuladorVentas.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPromocionEjemplofrmSimuladorVentas.Location = new System.Drawing.Point(20, 188);
            this.chkPromocionEjemplofrmSimuladorVentas.Name = "chkPromocionEjemplofrmSimuladorVentas";
            this.chkPromocionEjemplofrmSimuladorVentas.TabIndex = 10;
            this.chkPromocionEjemplofrmSimuladorVentas.Text = "Incluir una promoción de ejemplo";
            this.chkPromocionEjemplofrmSimuladorVentas.UseVisualStyleBackColor = true;
            //
            // btnGenerarfrmSimuladorVentas
            //
            this.btnGenerarfrmSimuladorVentas.BackColor = System.Drawing.Color.SandyBrown;
            this.btnGenerarfrmSimuladorVentas.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerarfrmSimuladorVentas.Location = new System.Drawing.Point(20, 225);
            this.btnGenerarfrmSimuladorVentas.Name = "btnGenerarfrmSimuladorVentas";
            this.btnGenerarfrmSimuladorVentas.Size = new System.Drawing.Size(250, 38);
            this.btnGenerarfrmSimuladorVentas.TabIndex = 7;
            this.btnGenerarfrmSimuladorVentas.Text = "Generar simulación";
            this.btnGenerarfrmSimuladorVentas.UseVisualStyleBackColor = false;
            this.btnGenerarfrmSimuladorVentas.Click += new System.EventHandler(this.btnGenerarfrmSimuladorVentas_Click);
            //
            // btnBorrarfrmSimuladorVentas
            //
            this.btnBorrarfrmSimuladorVentas.BackColor = System.Drawing.Color.SandyBrown;
            this.btnBorrarfrmSimuladorVentas.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBorrarfrmSimuladorVentas.Location = new System.Drawing.Point(290, 225);
            this.btnBorrarfrmSimuladorVentas.Name = "btnBorrarfrmSimuladorVentas";
            this.btnBorrarfrmSimuladorVentas.Size = new System.Drawing.Size(250, 38);
            this.btnBorrarfrmSimuladorVentas.TabIndex = 8;
            this.btnBorrarfrmSimuladorVentas.Text = "Borrar simulación";
            this.btnBorrarfrmSimuladorVentas.UseVisualStyleBackColor = false;
            this.btnBorrarfrmSimuladorVentas.Click += new System.EventHandler(this.btnBorrarfrmSimuladorVentas_Click);
            //
            // lblEstadofrmSimuladorVentas
            //
            this.lblEstadofrmSimuladorVentas.Location = new System.Drawing.Point(20, 280);
            this.lblEstadofrmSimuladorVentas.Name = "lblEstadofrmSimuladorVentas";
            this.lblEstadofrmSimuladorVentas.Size = new System.Drawing.Size(520, 40);
            this.lblEstadofrmSimuladorVentas.TabIndex = 9;
            this.lblEstadofrmSimuladorVentas.Text = "-";
            //
            // frmSimuladorVentas
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.NavajoWhite;
            this.ClientSize = new System.Drawing.Size(560, 335);
            this.Controls.Add(this.chkPromocionEjemplofrmSimuladorVentas);
            this.Controls.Add(this.lblEstadofrmSimuladorVentas);
            this.Controls.Add(this.btnBorrarfrmSimuladorVentas);
            this.Controls.Add(this.btnGenerarfrmSimuladorVentas);
            this.Controls.Add(this.nudSemillafrmSimuladorVentas);
            this.Controls.Add(this.lblSemillafrmSimuladorVentas);
            this.Controls.Add(this.nudVentasNochefrmSimuladorVentas);
            this.Controls.Add(this.lblVentasNochefrmSimuladorVentas);
            this.Controls.Add(this.nudMesesfrmSimuladorVentas);
            this.Controls.Add(this.lblMesesfrmSimuladorVentas);
            this.Controls.Add(this.lblAyudafrmSimuladorVentas);
            this.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmSimuladorVentas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmSimuladorVentas";
            this.Load += new System.EventHandler(this.frmSimuladorVentas_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudMesesfrmSimuladorVentas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVentasNochefrmSimuladorVentas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSemillafrmSimuladorVentas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAyudafrmSimuladorVentas;
        private System.Windows.Forms.Label lblMesesfrmSimuladorVentas;
        private System.Windows.Forms.NumericUpDown nudMesesfrmSimuladorVentas;
        private System.Windows.Forms.Label lblVentasNochefrmSimuladorVentas;
        private System.Windows.Forms.NumericUpDown nudVentasNochefrmSimuladorVentas;
        private System.Windows.Forms.Label lblSemillafrmSimuladorVentas;
        private System.Windows.Forms.NumericUpDown nudSemillafrmSimuladorVentas;
        private System.Windows.Forms.CheckBox chkPromocionEjemplofrmSimuladorVentas;
        private System.Windows.Forms.Button btnGenerarfrmSimuladorVentas;
        private System.Windows.Forms.Button btnBorrarfrmSimuladorVentas;
        private System.Windows.Forms.Label lblEstadofrmSimuladorVentas;
    }
}
