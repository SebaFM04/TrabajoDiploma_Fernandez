namespace UI
{
    partial class frmPromocion
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
            this.lblFiltrofrmPromocion = new System.Windows.Forms.Label();
            this.cmbFiltrofrmPromocion = new System.Windows.Forms.ComboBox();
            this.dgvPromocionesfrmPromocion = new System.Windows.Forms.DataGridView();
            this.lblDetallefrmPromocion = new System.Windows.Forms.Label();
            this.dgvDetallefrmPromocion = new System.Windows.Forms.DataGridView();
            this.lblNuevafrmPromocion = new System.Windows.Forms.Label();
            this.lblNombrefrmPromocion = new System.Windows.Forms.Label();
            this.txtNombrefrmPromocion = new System.Windows.Forms.TextBox();
            this.lblCriteriofrmPromocion = new System.Windows.Forms.Label();
            this.cmbCriteriofrmPromocion = new System.Windows.Forms.ComboBox();
            this.lblDesdefrmPromocion = new System.Windows.Forms.Label();
            this.dtpDesdefrmPromocion = new System.Windows.Forms.DateTimePicker();
            this.lblHastafrmPromocion = new System.Windows.Forms.Label();
            this.dtpHastafrmPromocion = new System.Windows.Forms.DateTimePicker();
            this.lblProductofrmPromocion = new System.Windows.Forms.Label();
            this.cmbProductofrmPromocion = new System.Windows.Forms.ComboBox();
            this.nudPreciofrmPromocion = new System.Windows.Forms.NumericUpDown();
            this.btnAgregarfrmPromocion = new System.Windows.Forms.Button();
            this.dgvLineasfrmPromocion = new System.Windows.Forms.DataGridView();
            this.btnQuitarfrmPromocion = new System.Windows.Forms.Button();
            this.btnNuevafrmPromocion = new System.Windows.Forms.Button();
            this.btnCrearfrmPromocion = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPromocionesfrmPromocion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetallefrmPromocion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPreciofrmPromocion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLineasfrmPromocion)).BeginInit();
            this.SuspendLayout();
            //
            // lblFiltrofrmPromocion
            //
            this.lblFiltrofrmPromocion.AutoSize = true;
            this.lblFiltrofrmPromocion.Location = new System.Drawing.Point(15, 16);
            this.lblFiltrofrmPromocion.Name = "lblFiltrofrmPromocion";
            this.lblFiltrofrmPromocion.TabIndex = 0;
            this.lblFiltrofrmPromocion.Text = "Estado";
            //
            // cmbFiltrofrmPromocion
            //
            this.cmbFiltrofrmPromocion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFiltrofrmPromocion.Location = new System.Drawing.Point(90, 12);
            this.cmbFiltrofrmPromocion.Name = "cmbFiltrofrmPromocion";
            this.cmbFiltrofrmPromocion.Size = new System.Drawing.Size(180, 24);
            this.cmbFiltrofrmPromocion.TabIndex = 1;
            this.cmbFiltrofrmPromocion.SelectedIndexChanged += new System.EventHandler(this.cmbFiltrofrmPromocion_SelectedIndexChanged);
            //
            // dgvPromocionesfrmPromocion
            //
            this.dgvPromocionesfrmPromocion.AllowUserToAddRows = false;
            this.dgvPromocionesfrmPromocion.AllowUserToDeleteRows = false;
            this.dgvPromocionesfrmPromocion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPromocionesfrmPromocion.Location = new System.Drawing.Point(15, 45);
            this.dgvPromocionesfrmPromocion.MultiSelect = false;
            this.dgvPromocionesfrmPromocion.Name = "dgvPromocionesfrmPromocion";
            this.dgvPromocionesfrmPromocion.ReadOnly = true;
            this.dgvPromocionesfrmPromocion.RowHeadersVisible = false;
            this.dgvPromocionesfrmPromocion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPromocionesfrmPromocion.Size = new System.Drawing.Size(540, 300);
            this.dgvPromocionesfrmPromocion.TabIndex = 2;
            this.dgvPromocionesfrmPromocion.SelectionChanged += new System.EventHandler(this.dgvPromocionesfrmPromocion_SelectionChanged);
            //
            // lblDetallefrmPromocion
            //
            this.lblDetallefrmPromocion.AutoSize = true;
            this.lblDetallefrmPromocion.Location = new System.Drawing.Point(15, 355);
            this.lblDetallefrmPromocion.Name = "lblDetallefrmPromocion";
            this.lblDetallefrmPromocion.TabIndex = 3;
            this.lblDetallefrmPromocion.Text = "Productos de la promoción";
            //
            // dgvDetallefrmPromocion
            //
            this.dgvDetallefrmPromocion.AllowUserToAddRows = false;
            this.dgvDetallefrmPromocion.AllowUserToDeleteRows = false;
            this.dgvDetallefrmPromocion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetallefrmPromocion.Location = new System.Drawing.Point(15, 378);
            this.dgvDetallefrmPromocion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left));
            this.dgvDetallefrmPromocion.Name = "dgvDetallefrmPromocion";
            this.dgvDetallefrmPromocion.ReadOnly = true;
            this.dgvDetallefrmPromocion.RowHeadersVisible = false;
            this.dgvDetallefrmPromocion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetallefrmPromocion.Size = new System.Drawing.Size(540, 290);
            this.dgvDetallefrmPromocion.TabIndex = 4;
            //
            // lblNuevafrmPromocion
            //
            this.lblNuevafrmPromocion.AutoSize = true;
            this.lblNuevafrmPromocion.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNuevafrmPromocion.Location = new System.Drawing.Point(575, 15);
            this.lblNuevafrmPromocion.Name = "lblNuevafrmPromocion";
            this.lblNuevafrmPromocion.TabIndex = 5;
            this.lblNuevafrmPromocion.Text = "Nueva promoción";
            //
            // lblNombrefrmPromocion
            //
            this.lblNombrefrmPromocion.AutoSize = true;
            this.lblNombrefrmPromocion.Location = new System.Drawing.Point(575, 48);
            this.lblNombrefrmPromocion.Name = "lblNombrefrmPromocion";
            this.lblNombrefrmPromocion.TabIndex = 6;
            this.lblNombrefrmPromocion.Text = "Nombre";
            //
            // txtNombrefrmPromocion
            //
            this.txtNombrefrmPromocion.Location = new System.Drawing.Point(690, 45);
            this.txtNombrefrmPromocion.MaxLength = 100;
            this.txtNombrefrmPromocion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
            this.txtNombrefrmPromocion.Name = "txtNombrefrmPromocion";
            this.txtNombrefrmPromocion.Size = new System.Drawing.Size(475, 23);
            this.txtNombrefrmPromocion.TabIndex = 7;
            //
            // lblCriteriofrmPromocion
            //
            this.lblCriteriofrmPromocion.AutoSize = true;
            this.lblCriteriofrmPromocion.Location = new System.Drawing.Point(575, 83);
            this.lblCriteriofrmPromocion.Name = "lblCriteriofrmPromocion";
            this.lblCriteriofrmPromocion.TabIndex = 8;
            this.lblCriteriofrmPromocion.Text = "Criterio";
            //
            // cmbCriteriofrmPromocion
            //
            this.cmbCriteriofrmPromocion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCriteriofrmPromocion.Location = new System.Drawing.Point(690, 80);
            this.cmbCriteriofrmPromocion.Name = "cmbCriteriofrmPromocion";
            this.cmbCriteriofrmPromocion.Size = new System.Drawing.Size(300, 24);
            this.cmbCriteriofrmPromocion.TabIndex = 9;
            //
            // lblDesdefrmPromocion
            //
            this.lblDesdefrmPromocion.AutoSize = true;
            this.lblDesdefrmPromocion.Location = new System.Drawing.Point(575, 118);
            this.lblDesdefrmPromocion.Name = "lblDesdefrmPromocion";
            this.lblDesdefrmPromocion.TabIndex = 10;
            this.lblDesdefrmPromocion.Text = "Desde";
            //
            // dtpDesdefrmPromocion
            //
            this.dtpDesdefrmPromocion.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDesdefrmPromocion.Location = new System.Drawing.Point(690, 115);
            this.dtpDesdefrmPromocion.Name = "dtpDesdefrmPromocion";
            this.dtpDesdefrmPromocion.Size = new System.Drawing.Size(120, 23);
            this.dtpDesdefrmPromocion.TabIndex = 11;
            //
            // lblHastafrmPromocion
            //
            this.lblHastafrmPromocion.AutoSize = true;
            this.lblHastafrmPromocion.Location = new System.Drawing.Point(830, 118);
            this.lblHastafrmPromocion.Name = "lblHastafrmPromocion";
            this.lblHastafrmPromocion.TabIndex = 12;
            this.lblHastafrmPromocion.Text = "Hasta";
            //
            // dtpHastafrmPromocion
            //
            this.dtpHastafrmPromocion.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHastafrmPromocion.Location = new System.Drawing.Point(890, 115);
            this.dtpHastafrmPromocion.Name = "dtpHastafrmPromocion";
            this.dtpHastafrmPromocion.Size = new System.Drawing.Size(120, 23);
            this.dtpHastafrmPromocion.TabIndex = 13;
            //
            // lblProductofrmPromocion
            //
            this.lblProductofrmPromocion.AutoSize = true;
            this.lblProductofrmPromocion.Location = new System.Drawing.Point(575, 153);
            this.lblProductofrmPromocion.Name = "lblProductofrmPromocion";
            this.lblProductofrmPromocion.TabIndex = 14;
            this.lblProductofrmPromocion.Text = "Producto";
            //
            // cmbProductofrmPromocion
            //
            this.cmbProductofrmPromocion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProductofrmPromocion.Location = new System.Drawing.Point(690, 150);
            this.cmbProductofrmPromocion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
            this.cmbProductofrmPromocion.Name = "cmbProductofrmPromocion";
            this.cmbProductofrmPromocion.Size = new System.Drawing.Size(280, 24);
            this.cmbProductofrmPromocion.TabIndex = 15;
            this.cmbProductofrmPromocion.SelectedIndexChanged += new System.EventHandler(this.cmbProductofrmPromocion_SelectedIndexChanged);
            //
            // nudPreciofrmPromocion
            //
            this.nudPreciofrmPromocion.DecimalPlaces = 2;
            this.nudPreciofrmPromocion.Location = new System.Drawing.Point(980, 151);
            this.nudPreciofrmPromocion.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            this.nudPreciofrmPromocion.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.nudPreciofrmPromocion.Name = "nudPreciofrmPromocion";
            this.nudPreciofrmPromocion.Size = new System.Drawing.Size(95, 23);
            this.nudPreciofrmPromocion.TabIndex = 16;
            this.nudPreciofrmPromocion.ThousandsSeparator = true;
            //
            // btnAgregarfrmPromocion
            //
            this.btnAgregarfrmPromocion.BackColor = System.Drawing.Color.SandyBrown;
            this.btnAgregarfrmPromocion.Location = new System.Drawing.Point(1080, 147);
            this.btnAgregarfrmPromocion.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnAgregarfrmPromocion.Name = "btnAgregarfrmPromocion";
            this.btnAgregarfrmPromocion.Size = new System.Drawing.Size(85, 30);
            this.btnAgregarfrmPromocion.TabIndex = 17;
            this.btnAgregarfrmPromocion.Text = "Agregar";
            this.btnAgregarfrmPromocion.UseVisualStyleBackColor = false;
            this.btnAgregarfrmPromocion.Click += new System.EventHandler(this.btnAgregarfrmPromocion_Click);
            //
            // dgvLineasfrmPromocion
            //
            this.dgvLineasfrmPromocion.AllowUserToAddRows = false;
            this.dgvLineasfrmPromocion.AllowUserToDeleteRows = false;
            this.dgvLineasfrmPromocion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLineasfrmPromocion.Location = new System.Drawing.Point(575, 190);
            this.dgvLineasfrmPromocion.MultiSelect = false;
            this.dgvLineasfrmPromocion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
            this.dgvLineasfrmPromocion.Name = "dgvLineasfrmPromocion";
            this.dgvLineasfrmPromocion.RowHeadersVisible = false;
            this.dgvLineasfrmPromocion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLineasfrmPromocion.Size = new System.Drawing.Size(590, 430);
            this.dgvLineasfrmPromocion.TabIndex = 18;
            this.dgvLineasfrmPromocion.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLineasfrmPromocion_CellValueChanged);
            this.dgvLineasfrmPromocion.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvLineasfrmPromocion_DataError);
            //
            // btnQuitarfrmPromocion
            //
            this.btnQuitarfrmPromocion.BackColor = System.Drawing.Color.SandyBrown;
            this.btnQuitarfrmPromocion.Location = new System.Drawing.Point(575, 630);
            this.btnQuitarfrmPromocion.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.btnQuitarfrmPromocion.AutoSize = true;
            this.btnQuitarfrmPromocion.MinimumSize = new System.Drawing.Size(130, 38);
            this.btnQuitarfrmPromocion.Name = "btnQuitarfrmPromocion";
            this.btnQuitarfrmPromocion.Size = new System.Drawing.Size(130, 38);
            this.btnQuitarfrmPromocion.TabIndex = 19;
            this.btnQuitarfrmPromocion.Text = "Quitar";
            this.btnQuitarfrmPromocion.UseVisualStyleBackColor = false;
            this.btnQuitarfrmPromocion.Click += new System.EventHandler(this.btnQuitarfrmPromocion_Click);
            //
            // btnNuevafrmPromocion
            //
            this.btnNuevafrmPromocion.BackColor = System.Drawing.Color.SandyBrown;
            this.btnNuevafrmPromocion.Location = new System.Drawing.Point(715, 630);
            this.btnNuevafrmPromocion.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.btnNuevafrmPromocion.AutoSize = true;
            this.btnNuevafrmPromocion.MinimumSize = new System.Drawing.Size(130, 38);
            this.btnNuevafrmPromocion.Name = "btnNuevafrmPromocion";
            this.btnNuevafrmPromocion.Size = new System.Drawing.Size(130, 38);
            this.btnNuevafrmPromocion.TabIndex = 20;
            this.btnNuevafrmPromocion.Text = "Limpiar";
            this.btnNuevafrmPromocion.UseVisualStyleBackColor = false;
            this.btnNuevafrmPromocion.Click += new System.EventHandler(this.btnNuevafrmPromocion_Click);
            //
            // btnCrearfrmPromocion
            //
            this.btnCrearfrmPromocion.BackColor = System.Drawing.Color.SandyBrown;
            this.btnCrearfrmPromocion.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCrearfrmPromocion.Location = new System.Drawing.Point(965, 630);
            this.btnCrearfrmPromocion.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right));
            this.btnCrearfrmPromocion.AutoSize = true;
            this.btnCrearfrmPromocion.MinimumSize = new System.Drawing.Size(200, 38);
            this.btnCrearfrmPromocion.Name = "btnCrearfrmPromocion";
            this.btnCrearfrmPromocion.Size = new System.Drawing.Size(200, 38);
            this.btnCrearfrmPromocion.TabIndex = 21;
            this.btnCrearfrmPromocion.Text = "Crear promoción";
            this.btnCrearfrmPromocion.UseVisualStyleBackColor = false;
            this.btnCrearfrmPromocion.Click += new System.EventHandler(this.btnCrearfrmPromocion_Click);
            //
            // frmPromocion
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.NavajoWhite;
            this.ClientSize = new System.Drawing.Size(1180, 685);
            this.Controls.Add(this.btnCrearfrmPromocion);
            this.Controls.Add(this.btnNuevafrmPromocion);
            this.Controls.Add(this.btnQuitarfrmPromocion);
            this.Controls.Add(this.dgvLineasfrmPromocion);
            this.Controls.Add(this.btnAgregarfrmPromocion);
            this.Controls.Add(this.nudPreciofrmPromocion);
            this.Controls.Add(this.cmbProductofrmPromocion);
            this.Controls.Add(this.lblProductofrmPromocion);
            this.Controls.Add(this.dtpHastafrmPromocion);
            this.Controls.Add(this.lblHastafrmPromocion);
            this.Controls.Add(this.dtpDesdefrmPromocion);
            this.Controls.Add(this.lblDesdefrmPromocion);
            this.Controls.Add(this.cmbCriteriofrmPromocion);
            this.Controls.Add(this.lblCriteriofrmPromocion);
            this.Controls.Add(this.txtNombrefrmPromocion);
            this.Controls.Add(this.lblNombrefrmPromocion);
            this.Controls.Add(this.lblNuevafrmPromocion);
            this.Controls.Add(this.dgvDetallefrmPromocion);
            this.Controls.Add(this.lblDetallefrmPromocion);
            this.Controls.Add(this.dgvPromocionesfrmPromocion);
            this.Controls.Add(this.cmbFiltrofrmPromocion);
            this.Controls.Add(this.lblFiltrofrmPromocion);
            this.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(1196, 620);
            this.Name = "frmPromocion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmPromocion";
            this.Load += new System.EventHandler(this.frmPromocion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPromocionesfrmPromocion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetallefrmPromocion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPreciofrmPromocion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLineasfrmPromocion)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFiltrofrmPromocion;
        private System.Windows.Forms.ComboBox cmbFiltrofrmPromocion;
        private System.Windows.Forms.DataGridView dgvPromocionesfrmPromocion;
        private System.Windows.Forms.Label lblDetallefrmPromocion;
        private System.Windows.Forms.DataGridView dgvDetallefrmPromocion;
        private System.Windows.Forms.Label lblNuevafrmPromocion;
        private System.Windows.Forms.Label lblNombrefrmPromocion;
        private System.Windows.Forms.TextBox txtNombrefrmPromocion;
        private System.Windows.Forms.Label lblCriteriofrmPromocion;
        private System.Windows.Forms.ComboBox cmbCriteriofrmPromocion;
        private System.Windows.Forms.Label lblDesdefrmPromocion;
        private System.Windows.Forms.DateTimePicker dtpDesdefrmPromocion;
        private System.Windows.Forms.Label lblHastafrmPromocion;
        private System.Windows.Forms.DateTimePicker dtpHastafrmPromocion;
        private System.Windows.Forms.Label lblProductofrmPromocion;
        private System.Windows.Forms.ComboBox cmbProductofrmPromocion;
        private System.Windows.Forms.NumericUpDown nudPreciofrmPromocion;
        private System.Windows.Forms.Button btnAgregarfrmPromocion;
        private System.Windows.Forms.DataGridView dgvLineasfrmPromocion;
        private System.Windows.Forms.Button btnQuitarfrmPromocion;
        private System.Windows.Forms.Button btnNuevafrmPromocion;
        private System.Windows.Forms.Button btnCrearfrmPromocion;
    }
}
