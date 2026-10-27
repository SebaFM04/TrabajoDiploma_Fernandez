namespace UI
{
    partial class frmAnalisisRentabilidad
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
            this.lblDesdefrmAnalisisRentabilidad = new System.Windows.Forms.Label();
            this.dtpDesdefrmAnalisisRentabilidad = new System.Windows.Forms.DateTimePicker();
            this.lblHastafrmAnalisisRentabilidad = new System.Windows.Forms.Label();
            this.dtpHastafrmAnalisisRentabilidad = new System.Windows.Forms.DateTimePicker();
            this.lblCriteriofrmAnalisisRentabilidad = new System.Windows.Forms.Label();
            this.cmbCriteriofrmAnalisisRentabilidad = new System.Windows.Forms.ComboBox();
            this.lblDiasfrmAnalisisRentabilidad = new System.Windows.Forms.Label();
            this.cmbDiasfrmAnalisisRentabilidad = new System.Windows.Forms.ComboBox();
            this.btnGenerarfrmAnalisisRentabilidad = new System.Windows.Forms.Button();
            this.lblResumenfrmAnalisisRentabilidad = new System.Windows.Forms.Label();
            this.dgvResultadosfrmAnalisisRentabilidad = new System.Windows.Forms.DataGridView();
            this.lblSugeridosfrmAnalisisRentabilidad = new System.Windows.Forms.Label();
            this.dgvSugeridosfrmAnalisisRentabilidad = new System.Windows.Forms.DataGridView();
            this.lblAvisosfrmAnalisisRentabilidad = new System.Windows.Forms.Label();
            this.tabGraficosfrmAnalisisRentabilidad = new System.Windows.Forms.TabControl();
            this.btnCrearPromocionfrmAnalisisRentabilidad = new System.Windows.Forms.Button();
            this.btnAbrirfrmAnalisisRentabilidad = new System.Windows.Forms.Button();
            this.btnGuardarfrmAnalisisRentabilidad = new System.Windows.Forms.Button();
            this.btnExportarPdffrmAnalisisRentabilidad = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResultadosfrmAnalisisRentabilidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSugeridosfrmAnalisisRentabilidad)).BeginInit();
            this.SuspendLayout();
            //
            // lblDesdefrmAnalisisRentabilidad
            //
            this.lblDesdefrmAnalisisRentabilidad.AutoSize = true;
            this.lblDesdefrmAnalisisRentabilidad.Location = new System.Drawing.Point(15, 18);
            this.lblDesdefrmAnalisisRentabilidad.Name = "lblDesdefrmAnalisisRentabilidad";
            this.lblDesdefrmAnalisisRentabilidad.TabIndex = 0;
            this.lblDesdefrmAnalisisRentabilidad.Text = "Desde";
            //
            // dtpDesdefrmAnalisisRentabilidad
            //
            this.dtpDesdefrmAnalisisRentabilidad.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDesdefrmAnalisisRentabilidad.Location = new System.Drawing.Point(75, 15);
            this.dtpDesdefrmAnalisisRentabilidad.Name = "dtpDesdefrmAnalisisRentabilidad";
            this.dtpDesdefrmAnalisisRentabilidad.Size = new System.Drawing.Size(120, 23);
            this.dtpDesdefrmAnalisisRentabilidad.TabIndex = 1;
            //
            // lblHastafrmAnalisisRentabilidad
            //
            this.lblHastafrmAnalisisRentabilidad.AutoSize = true;
            this.lblHastafrmAnalisisRentabilidad.Location = new System.Drawing.Point(210, 18);
            this.lblHastafrmAnalisisRentabilidad.Name = "lblHastafrmAnalisisRentabilidad";
            this.lblHastafrmAnalisisRentabilidad.TabIndex = 2;
            this.lblHastafrmAnalisisRentabilidad.Text = "Hasta";
            //
            // dtpHastafrmAnalisisRentabilidad
            //
            this.dtpHastafrmAnalisisRentabilidad.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHastafrmAnalisisRentabilidad.Location = new System.Drawing.Point(265, 15);
            this.dtpHastafrmAnalisisRentabilidad.Name = "dtpHastafrmAnalisisRentabilidad";
            this.dtpHastafrmAnalisisRentabilidad.Size = new System.Drawing.Size(120, 23);
            this.dtpHastafrmAnalisisRentabilidad.TabIndex = 3;
            //
            // lblCriteriofrmAnalisisRentabilidad
            //
            this.lblCriteriofrmAnalisisRentabilidad.AutoSize = true;
            this.lblCriteriofrmAnalisisRentabilidad.Location = new System.Drawing.Point(400, 18);
            this.lblCriteriofrmAnalisisRentabilidad.Name = "lblCriteriofrmAnalisisRentabilidad";
            this.lblCriteriofrmAnalisisRentabilidad.TabIndex = 4;
            this.lblCriteriofrmAnalisisRentabilidad.Text = "Criterio";
            //
            // cmbCriteriofrmAnalisisRentabilidad
            //
            this.cmbCriteriofrmAnalisisRentabilidad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCriteriofrmAnalisisRentabilidad.Location = new System.Drawing.Point(470, 14);
            this.cmbCriteriofrmAnalisisRentabilidad.Name = "cmbCriteriofrmAnalisisRentabilidad";
            this.cmbCriteriofrmAnalisisRentabilidad.Size = new System.Drawing.Size(170, 24);
            this.cmbCriteriofrmAnalisisRentabilidad.TabIndex = 5;
            this.cmbCriteriofrmAnalisisRentabilidad.SelectedIndexChanged += new System.EventHandler(this.cmbCriteriofrmAnalisisRentabilidad_SelectedIndexChanged);
            //
            // lblDiasfrmAnalisisRentabilidad
            //
            this.lblDiasfrmAnalisisRentabilidad.AutoSize = true;
            this.lblDiasfrmAnalisisRentabilidad.Location = new System.Drawing.Point(655, 18);
            this.lblDiasfrmAnalisisRentabilidad.Name = "lblDiasfrmAnalisisRentabilidad";
            this.lblDiasfrmAnalisisRentabilidad.TabIndex = 6;
            this.lblDiasfrmAnalisisRentabilidad.Text = "Días por vencer";
            //
            // cmbDiasfrmAnalisisRentabilidad
            //
            this.cmbDiasfrmAnalisisRentabilidad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDiasfrmAnalisisRentabilidad.Location = new System.Drawing.Point(785, 14);
            this.cmbDiasfrmAnalisisRentabilidad.Name = "cmbDiasfrmAnalisisRentabilidad";
            this.cmbDiasfrmAnalisisRentabilidad.Size = new System.Drawing.Size(70, 24);
            this.cmbDiasfrmAnalisisRentabilidad.TabIndex = 7;
            //
            // btnGenerarfrmAnalisisRentabilidad
            //
            this.btnGenerarfrmAnalisisRentabilidad.BackColor = System.Drawing.Color.SandyBrown;
            this.btnGenerarfrmAnalisisRentabilidad.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerarfrmAnalisisRentabilidad.Location = new System.Drawing.Point(875, 9);
            this.btnGenerarfrmAnalisisRentabilidad.Name = "btnGenerarfrmAnalisisRentabilidad";
            this.btnGenerarfrmAnalisisRentabilidad.Size = new System.Drawing.Size(160, 34);
            this.btnGenerarfrmAnalisisRentabilidad.TabIndex = 8;
            this.btnGenerarfrmAnalisisRentabilidad.Text = "Generar";
            this.btnGenerarfrmAnalisisRentabilidad.UseVisualStyleBackColor = false;
            this.btnGenerarfrmAnalisisRentabilidad.Click += new System.EventHandler(this.btnGenerarfrmAnalisisRentabilidad_Click);
            //
            // lblResumenfrmAnalisisRentabilidad
            //
            this.lblResumenfrmAnalisisRentabilidad.Location = new System.Drawing.Point(15, 52);
            this.lblResumenfrmAnalisisRentabilidad.Name = "lblResumenfrmAnalisisRentabilidad";
            this.lblResumenfrmAnalisisRentabilidad.Size = new System.Drawing.Size(1150, 22);
            this.lblResumenfrmAnalisisRentabilidad.TabIndex = 9;
            this.lblResumenfrmAnalisisRentabilidad.Text = "-";
            //
            // dgvResultadosfrmAnalisisRentabilidad
            //
            this.dgvResultadosfrmAnalisisRentabilidad.AllowUserToAddRows = false;
            this.dgvResultadosfrmAnalisisRentabilidad.AllowUserToDeleteRows = false;
            this.dgvResultadosfrmAnalisisRentabilidad.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResultadosfrmAnalisisRentabilidad.Location = new System.Drawing.Point(15, 80);
            this.dgvResultadosfrmAnalisisRentabilidad.Name = "dgvResultadosfrmAnalisisRentabilidad";
            this.dgvResultadosfrmAnalisisRentabilidad.ReadOnly = true;
            this.dgvResultadosfrmAnalisisRentabilidad.RowHeadersVisible = false;
            this.dgvResultadosfrmAnalisisRentabilidad.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResultadosfrmAnalisisRentabilidad.Size = new System.Drawing.Size(700, 360);
            this.dgvResultadosfrmAnalisisRentabilidad.TabIndex = 10;
            //
            // lblSugeridosfrmAnalisisRentabilidad
            //
            this.lblSugeridosfrmAnalisisRentabilidad.AutoSize = true;
            this.lblSugeridosfrmAnalisisRentabilidad.Location = new System.Drawing.Point(15, 450);
            this.lblSugeridosfrmAnalisisRentabilidad.Name = "lblSugeridosfrmAnalisisRentabilidad";
            this.lblSugeridosfrmAnalisisRentabilidad.TabIndex = 11;
            this.lblSugeridosfrmAnalisisRentabilidad.Text = "Productos sugeridos para promocionar";
            //
            // dgvSugeridosfrmAnalisisRentabilidad
            //
            this.dgvSugeridosfrmAnalisisRentabilidad.AllowUserToAddRows = false;
            this.dgvSugeridosfrmAnalisisRentabilidad.AllowUserToDeleteRows = false;
            this.dgvSugeridosfrmAnalisisRentabilidad.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSugeridosfrmAnalisisRentabilidad.Location = new System.Drawing.Point(15, 472);
            this.dgvSugeridosfrmAnalisisRentabilidad.Name = "dgvSugeridosfrmAnalisisRentabilidad";
            this.dgvSugeridosfrmAnalisisRentabilidad.ReadOnly = true;
            this.dgvSugeridosfrmAnalisisRentabilidad.RowHeadersVisible = false;
            this.dgvSugeridosfrmAnalisisRentabilidad.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSugeridosfrmAnalisisRentabilidad.Size = new System.Drawing.Size(700, 190);
            this.dgvSugeridosfrmAnalisisRentabilidad.TabIndex = 12;
            //
            // lblAvisosfrmAnalisisRentabilidad
            //
            this.lblAvisosfrmAnalisisRentabilidad.ForeColor = System.Drawing.Color.Firebrick;
            this.lblAvisosfrmAnalisisRentabilidad.Location = new System.Drawing.Point(15, 670);
            this.lblAvisosfrmAnalisisRentabilidad.Name = "lblAvisosfrmAnalisisRentabilidad";
            this.lblAvisosfrmAnalisisRentabilidad.Size = new System.Drawing.Size(700, 60);
            this.lblAvisosfrmAnalisisRentabilidad.TabIndex = 13;
            //
            // tabGraficosfrmAnalisisRentabilidad
            //
            this.tabGraficosfrmAnalisisRentabilidad.Location = new System.Drawing.Point(730, 80);
            this.tabGraficosfrmAnalisisRentabilidad.Name = "tabGraficosfrmAnalisisRentabilidad";
            this.tabGraficosfrmAnalisisRentabilidad.SelectedIndex = 0;
            this.tabGraficosfrmAnalisisRentabilidad.Size = new System.Drawing.Size(435, 600);
            //
            // btnAbrirfrmAnalisisRentabilidad
            //
            this.btnAbrirfrmAnalisisRentabilidad.BackColor = System.Drawing.Color.SandyBrown;
            this.btnAbrirfrmAnalisisRentabilidad.Location = new System.Drawing.Point(730, 692);
            this.btnAbrirfrmAnalisisRentabilidad.Name = "btnAbrirfrmAnalisisRentabilidad";
            this.btnAbrirfrmAnalisisRentabilidad.Size = new System.Drawing.Size(140, 34);
            this.btnAbrirfrmAnalisisRentabilidad.TabIndex = 16;
            this.btnAbrirfrmAnalisisRentabilidad.Text = "Abrir análisis";
            this.btnAbrirfrmAnalisisRentabilidad.UseVisualStyleBackColor = false;
            this.btnAbrirfrmAnalisisRentabilidad.Click += new System.EventHandler(this.btnAbrirfrmAnalisisRentabilidad_Click);
            //
            // btnGuardarfrmAnalisisRentabilidad
            //
            this.btnGuardarfrmAnalisisRentabilidad.BackColor = System.Drawing.Color.SandyBrown;
            this.btnGuardarfrmAnalisisRentabilidad.Location = new System.Drawing.Point(877, 692);
            this.btnGuardarfrmAnalisisRentabilidad.Name = "btnGuardarfrmAnalisisRentabilidad";
            this.btnGuardarfrmAnalisisRentabilidad.Size = new System.Drawing.Size(140, 34);
            this.btnGuardarfrmAnalisisRentabilidad.TabIndex = 17;
            this.btnGuardarfrmAnalisisRentabilidad.Text = "Guardar análisis";
            this.btnGuardarfrmAnalisisRentabilidad.UseVisualStyleBackColor = false;
            this.btnGuardarfrmAnalisisRentabilidad.Click += new System.EventHandler(this.btnGuardarfrmAnalisisRentabilidad_Click);
            //
            // btnExportarPdffrmAnalisisRentabilidad
            //
            this.btnExportarPdffrmAnalisisRentabilidad.BackColor = System.Drawing.Color.SandyBrown;
            this.btnExportarPdffrmAnalisisRentabilidad.Location = new System.Drawing.Point(1025, 692);
            this.btnExportarPdffrmAnalisisRentabilidad.Name = "btnExportarPdffrmAnalisisRentabilidad";
            this.btnExportarPdffrmAnalisisRentabilidad.Size = new System.Drawing.Size(140, 34);
            this.btnExportarPdffrmAnalisisRentabilidad.TabIndex = 18;
            this.btnExportarPdffrmAnalisisRentabilidad.Text = "Exportar PDF";
            this.btnExportarPdffrmAnalisisRentabilidad.UseVisualStyleBackColor = false;
            this.btnExportarPdffrmAnalisisRentabilidad.Click += new System.EventHandler(this.btnExportarPdffrmAnalisisRentabilidad_Click);
            this.tabGraficosfrmAnalisisRentabilidad.TabIndex = 14;
            //
            // btnCrearPromocionfrmAnalisisRentabilidad
            //
            this.btnCrearPromocionfrmAnalisisRentabilidad.BackColor = System.Drawing.Color.SandyBrown;
            this.btnCrearPromocionfrmAnalisisRentabilidad.Location = new System.Drawing.Point(445, 443);
            this.btnCrearPromocionfrmAnalisisRentabilidad.Name = "btnCrearPromocionfrmAnalisisRentabilidad";
            this.btnCrearPromocionfrmAnalisisRentabilidad.Size = new System.Drawing.Size(270, 27);
            this.btnCrearPromocionfrmAnalisisRentabilidad.TabIndex = 15;
            this.btnCrearPromocionfrmAnalisisRentabilidad.Text = "Crear promoción con los seleccionados";
            this.btnCrearPromocionfrmAnalisisRentabilidad.UseVisualStyleBackColor = false;
            this.btnCrearPromocionfrmAnalisisRentabilidad.Click += new System.EventHandler(this.btnCrearPromocionfrmAnalisisRentabilidad_Click);
            //
            // frmAnalisisRentabilidad
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.NavajoWhite;
            this.ClientSize = new System.Drawing.Size(1180, 745);
            this.Controls.Add(this.btnExportarPdffrmAnalisisRentabilidad);
            this.Controls.Add(this.btnGuardarfrmAnalisisRentabilidad);
            this.Controls.Add(this.btnAbrirfrmAnalisisRentabilidad);
            this.Controls.Add(this.btnCrearPromocionfrmAnalisisRentabilidad);
            this.Controls.Add(this.tabGraficosfrmAnalisisRentabilidad);
            this.Controls.Add(this.lblAvisosfrmAnalisisRentabilidad);
            this.Controls.Add(this.dgvSugeridosfrmAnalisisRentabilidad);
            this.Controls.Add(this.lblSugeridosfrmAnalisisRentabilidad);
            this.Controls.Add(this.dgvResultadosfrmAnalisisRentabilidad);
            this.Controls.Add(this.lblResumenfrmAnalisisRentabilidad);
            this.Controls.Add(this.btnGenerarfrmAnalisisRentabilidad);
            this.Controls.Add(this.cmbDiasfrmAnalisisRentabilidad);
            this.Controls.Add(this.lblDiasfrmAnalisisRentabilidad);
            this.Controls.Add(this.cmbCriteriofrmAnalisisRentabilidad);
            this.Controls.Add(this.lblCriteriofrmAnalisisRentabilidad);
            this.Controls.Add(this.dtpHastafrmAnalisisRentabilidad);
            this.Controls.Add(this.lblHastafrmAnalisisRentabilidad);
            this.Controls.Add(this.dtpDesdefrmAnalisisRentabilidad);
            this.Controls.Add(this.lblDesdefrmAnalisisRentabilidad);
            this.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmAnalisisRentabilidad";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmAnalisisRentabilidad";
            this.Load += new System.EventHandler(this.frmAnalisisRentabilidad_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResultadosfrmAnalisisRentabilidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSugeridosfrmAnalisisRentabilidad)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDesdefrmAnalisisRentabilidad;
        private System.Windows.Forms.DateTimePicker dtpDesdefrmAnalisisRentabilidad;
        private System.Windows.Forms.Label lblHastafrmAnalisisRentabilidad;
        private System.Windows.Forms.DateTimePicker dtpHastafrmAnalisisRentabilidad;
        private System.Windows.Forms.Label lblCriteriofrmAnalisisRentabilidad;
        private System.Windows.Forms.ComboBox cmbCriteriofrmAnalisisRentabilidad;
        private System.Windows.Forms.Label lblDiasfrmAnalisisRentabilidad;
        private System.Windows.Forms.ComboBox cmbDiasfrmAnalisisRentabilidad;
        private System.Windows.Forms.Button btnGenerarfrmAnalisisRentabilidad;
        private System.Windows.Forms.Label lblResumenfrmAnalisisRentabilidad;
        private System.Windows.Forms.DataGridView dgvResultadosfrmAnalisisRentabilidad;
        private System.Windows.Forms.Label lblSugeridosfrmAnalisisRentabilidad;
        private System.Windows.Forms.DataGridView dgvSugeridosfrmAnalisisRentabilidad;
        private System.Windows.Forms.Label lblAvisosfrmAnalisisRentabilidad;
        private System.Windows.Forms.TabControl tabGraficosfrmAnalisisRentabilidad;
        private System.Windows.Forms.Button btnCrearPromocionfrmAnalisisRentabilidad;
        private System.Windows.Forms.Button btnAbrirfrmAnalisisRentabilidad;
        private System.Windows.Forms.Button btnGuardarfrmAnalisisRentabilidad;
        private System.Windows.Forms.Button btnExportarPdffrmAnalisisRentabilidad;
    }
}
