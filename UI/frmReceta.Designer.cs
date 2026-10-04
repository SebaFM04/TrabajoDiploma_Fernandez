namespace UI
{
    partial class frmReceta
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
            this.rdbSinRecetafrmReceta = new System.Windows.Forms.RadioButton();
            this.rdbConRecetafrmReceta = new System.Windows.Forms.RadioButton();
            this.lblProductosfrmReceta = new System.Windows.Forms.Label();
            this.dgvProductosfrmReceta = new System.Windows.Forms.DataGridView();
            this.lblRecetafrmReceta = new System.Windows.Forms.Label();
            this.lblInsumofrmReceta = new System.Windows.Forms.Label();
            this.cmbInsumofrmReceta = new System.Windows.Forms.ComboBox();
            this.btnNuevoInsumofrmReceta = new System.Windows.Forms.Button();
            this.lblProporcionfrmReceta = new System.Windows.Forms.Label();
            this.nudProporcionfrmReceta = new System.Windows.Forms.NumericUpDown();
            this.btnAgregarInsumofrmReceta = new System.Windows.Forms.Button();
            this.dgvRecetafrmReceta = new System.Windows.Forms.DataGridView();
            this.btnQuitarInsumofrmReceta = new System.Windows.Forms.Button();
            this.lblAyudaProporcionfrmReceta = new System.Windows.Forms.Label();
            this.btnConfirmarfrmReceta = new System.Windows.Forms.Button();
            this.btnCancelarfrmReceta = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductosfrmReceta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudProporcionfrmReceta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecetafrmReceta)).BeginInit();
            this.SuspendLayout();
            //
            // rdbSinRecetafrmReceta
            //
            this.rdbSinRecetafrmReceta.AutoSize = true;
            this.rdbSinRecetafrmReceta.Checked = true;
            this.rdbSinRecetafrmReceta.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbSinRecetafrmReceta.Location = new System.Drawing.Point(20, 12);
            this.rdbSinRecetafrmReceta.Name = "rdbSinRecetafrmReceta";
            this.rdbSinRecetafrmReceta.Size = new System.Drawing.Size(200, 27);
            this.rdbSinRecetafrmReceta.TabIndex = 0;
            this.rdbSinRecetafrmReceta.TabStop = true;
            this.rdbSinRecetafrmReceta.Text = "Productos sin receta";
            this.rdbSinRecetafrmReceta.UseVisualStyleBackColor = true;
            this.rdbSinRecetafrmReceta.CheckedChanged += new System.EventHandler(this.rdbModo_CheckedChanged);
            //
            // rdbConRecetafrmReceta
            //
            this.rdbConRecetafrmReceta.AutoSize = true;
            this.rdbConRecetafrmReceta.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbConRecetafrmReceta.Location = new System.Drawing.Point(260, 12);
            this.rdbConRecetafrmReceta.Name = "rdbConRecetafrmReceta";
            this.rdbConRecetafrmReceta.Size = new System.Drawing.Size(200, 27);
            this.rdbConRecetafrmReceta.TabIndex = 1;
            this.rdbConRecetafrmReceta.Text = "Productos con receta";
            this.rdbConRecetafrmReceta.UseVisualStyleBackColor = true;
            this.rdbConRecetafrmReceta.CheckedChanged += new System.EventHandler(this.rdbModo_CheckedChanged);
            //
            // lblProductosfrmReceta
            //
            this.lblProductosfrmReceta.AutoSize = true;
            this.lblProductosfrmReceta.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductosfrmReceta.Location = new System.Drawing.Point(20, 48);
            this.lblProductosfrmReceta.Name = "lblProductosfrmReceta";
            this.lblProductosfrmReceta.Size = new System.Drawing.Size(92, 23);
            this.lblProductosfrmReceta.TabIndex = 2;
            this.lblProductosfrmReceta.Text = "Productos";
            //
            // dgvProductosfrmReceta
            //
            this.dgvProductosfrmReceta.AllowUserToAddRows = false;
            this.dgvProductosfrmReceta.AllowUserToDeleteRows = false;
            this.dgvProductosfrmReceta.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductosfrmReceta.Location = new System.Drawing.Point(20, 75);
            this.dgvProductosfrmReceta.MultiSelect = false;
            this.dgvProductosfrmReceta.Name = "dgvProductosfrmReceta";
            this.dgvProductosfrmReceta.ReadOnly = true;
            this.dgvProductosfrmReceta.RowHeadersWidth = 51;
            this.dgvProductosfrmReceta.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProductosfrmReceta.Size = new System.Drawing.Size(400, 435);
            this.dgvProductosfrmReceta.TabIndex = 3;
            this.dgvProductosfrmReceta.SelectionChanged += new System.EventHandler(this.dgvProductosfrmReceta_SelectionChanged);
            //
            // lblRecetafrmReceta
            //
            this.lblRecetafrmReceta.AutoSize = true;
            this.lblRecetafrmReceta.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecetafrmReceta.Location = new System.Drawing.Point(440, 12);
            this.lblRecetafrmReceta.Name = "lblRecetafrmReceta";
            this.lblRecetafrmReceta.Size = new System.Drawing.Size(74, 25);
            this.lblRecetafrmReceta.TabIndex = 4;
            this.lblRecetafrmReceta.Text = "Receta";
            //
            // lblInsumofrmReceta
            //
            this.lblInsumofrmReceta.AutoSize = true;
            this.lblInsumofrmReceta.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInsumofrmReceta.Location = new System.Drawing.Point(440, 52);
            this.lblInsumofrmReceta.Name = "lblInsumofrmReceta";
            this.lblInsumofrmReceta.Size = new System.Drawing.Size(70, 23);
            this.lblInsumofrmReceta.TabIndex = 5;
            this.lblInsumofrmReceta.Text = "Insumo";
            //
            // cmbInsumofrmReceta
            //
            this.cmbInsumofrmReceta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInsumofrmReceta.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbInsumofrmReceta.FormattingEnabled = true;
            this.cmbInsumofrmReceta.Location = new System.Drawing.Point(560, 48);
            this.cmbInsumofrmReceta.Name = "cmbInsumofrmReceta";
            this.cmbInsumofrmReceta.Size = new System.Drawing.Size(300, 30);
            this.cmbInsumofrmReceta.TabIndex = 6;
            //
            // btnNuevoInsumofrmReceta
            //
            this.btnNuevoInsumofrmReceta.BackColor = System.Drawing.Color.SandyBrown;
            this.btnNuevoInsumofrmReceta.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNuevoInsumofrmReceta.Location = new System.Drawing.Point(875, 46);
            this.btnNuevoInsumofrmReceta.Name = "btnNuevoInsumofrmReceta";
            this.btnNuevoInsumofrmReceta.Size = new System.Drawing.Size(165, 34);
            this.btnNuevoInsumofrmReceta.TabIndex = 7;
            this.btnNuevoInsumofrmReceta.Text = "Nuevo insumo";
            this.btnNuevoInsumofrmReceta.UseVisualStyleBackColor = false;
            this.btnNuevoInsumofrmReceta.Click += new System.EventHandler(this.btnNuevoInsumofrmReceta_Click);
            //
            // lblProporcionfrmReceta
            //
            this.lblProporcionfrmReceta.AutoSize = true;
            this.lblProporcionfrmReceta.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProporcionfrmReceta.Location = new System.Drawing.Point(440, 94);
            this.lblProporcionfrmReceta.Name = "lblProporcionfrmReceta";
            this.lblProporcionfrmReceta.Size = new System.Drawing.Size(103, 23);
            this.lblProporcionfrmReceta.TabIndex = 8;
            this.lblProporcionfrmReceta.Text = "Proporción";
            //
            // nudProporcionfrmReceta
            //
            this.nudProporcionfrmReceta.DecimalPlaces = 3;
            this.nudProporcionfrmReceta.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudProporcionfrmReceta.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            this.nudProporcionfrmReceta.Location = new System.Drawing.Point(560, 91);
            this.nudProporcionfrmReceta.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.nudProporcionfrmReceta.Name = "nudProporcionfrmReceta";
            this.nudProporcionfrmReceta.Size = new System.Drawing.Size(150, 29);
            this.nudProporcionfrmReceta.TabIndex = 9;
            //
            // btnAgregarInsumofrmReceta
            //
            this.btnAgregarInsumofrmReceta.BackColor = System.Drawing.Color.SandyBrown;
            this.btnAgregarInsumofrmReceta.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgregarInsumofrmReceta.Location = new System.Drawing.Point(725, 88);
            this.btnAgregarInsumofrmReceta.Name = "btnAgregarInsumofrmReceta";
            this.btnAgregarInsumofrmReceta.Size = new System.Drawing.Size(165, 34);
            this.btnAgregarInsumofrmReceta.TabIndex = 10;
            this.btnAgregarInsumofrmReceta.Text = "Agregar insumo";
            this.btnAgregarInsumofrmReceta.UseVisualStyleBackColor = false;
            this.btnAgregarInsumofrmReceta.Click += new System.EventHandler(this.btnAgregarInsumofrmReceta_Click);
            //
            // dgvRecetafrmReceta
            //
            this.dgvRecetafrmReceta.AllowUserToAddRows = false;
            this.dgvRecetafrmReceta.AllowUserToDeleteRows = false;
            this.dgvRecetafrmReceta.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecetafrmReceta.Location = new System.Drawing.Point(440, 135);
            this.dgvRecetafrmReceta.MultiSelect = false;
            this.dgvRecetafrmReceta.Name = "dgvRecetafrmReceta";
            this.dgvRecetafrmReceta.RowHeadersWidth = 51;
            this.dgvRecetafrmReceta.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRecetafrmReceta.Size = new System.Drawing.Size(600, 235);
            this.dgvRecetafrmReceta.TabIndex = 11;
            //
            // btnQuitarInsumofrmReceta
            //
            this.btnQuitarInsumofrmReceta.BackColor = System.Drawing.Color.SandyBrown;
            this.btnQuitarInsumofrmReceta.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuitarInsumofrmReceta.Location = new System.Drawing.Point(440, 378);
            this.btnQuitarInsumofrmReceta.Name = "btnQuitarInsumofrmReceta";
            this.btnQuitarInsumofrmReceta.Size = new System.Drawing.Size(165, 34);
            this.btnQuitarInsumofrmReceta.TabIndex = 12;
            this.btnQuitarInsumofrmReceta.Text = "Quitar insumo";
            this.btnQuitarInsumofrmReceta.UseVisualStyleBackColor = false;
            this.btnQuitarInsumofrmReceta.Click += new System.EventHandler(this.btnQuitarInsumofrmReceta_Click);
            //
            // lblAyudaProporcionfrmReceta
            //
            this.lblAyudaProporcionfrmReceta.Font = new System.Drawing.Font("MS Reference Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAyudaProporcionfrmReceta.Location = new System.Drawing.Point(440, 420);
            this.lblAyudaProporcionfrmReceta.Name = "lblAyudaProporcionfrmReceta";
            this.lblAyudaProporcionfrmReceta.Size = new System.Drawing.Size(600, 42);
            this.lblAyudaProporcionfrmReceta.TabIndex = 13;
            this.lblAyudaProporcionfrmReceta.Text = "Proporción";
            //
            // btnConfirmarfrmReceta
            //
            this.btnConfirmarfrmReceta.BackColor = System.Drawing.Color.SandyBrown;
            this.btnConfirmarfrmReceta.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirmarfrmReceta.Location = new System.Drawing.Point(745, 468);
            this.btnConfirmarfrmReceta.Name = "btnConfirmarfrmReceta";
            this.btnConfirmarfrmReceta.Size = new System.Drawing.Size(140, 42);
            this.btnConfirmarfrmReceta.TabIndex = 14;
            this.btnConfirmarfrmReceta.Text = "Confirmar";
            this.btnConfirmarfrmReceta.UseVisualStyleBackColor = false;
            this.btnConfirmarfrmReceta.Click += new System.EventHandler(this.btnConfirmarfrmReceta_Click);
            //
            // btnCancelarfrmReceta
            //
            this.btnCancelarfrmReceta.BackColor = System.Drawing.Color.SandyBrown;
            this.btnCancelarfrmReceta.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelarfrmReceta.Location = new System.Drawing.Point(900, 468);
            this.btnCancelarfrmReceta.Name = "btnCancelarfrmReceta";
            this.btnCancelarfrmReceta.Size = new System.Drawing.Size(140, 42);
            this.btnCancelarfrmReceta.TabIndex = 15;
            this.btnCancelarfrmReceta.Text = "Cancelar";
            this.btnCancelarfrmReceta.UseVisualStyleBackColor = false;
            this.btnCancelarfrmReceta.Click += new System.EventHandler(this.btnCancelarfrmReceta_Click);
            //
            // frmReceta
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.NavajoWhite;
            this.ClientSize = new System.Drawing.Size(1062, 530);
            this.Controls.Add(this.btnCancelarfrmReceta);
            this.Controls.Add(this.btnConfirmarfrmReceta);
            this.Controls.Add(this.lblAyudaProporcionfrmReceta);
            this.Controls.Add(this.btnQuitarInsumofrmReceta);
            this.Controls.Add(this.dgvRecetafrmReceta);
            this.Controls.Add(this.btnAgregarInsumofrmReceta);
            this.Controls.Add(this.nudProporcionfrmReceta);
            this.Controls.Add(this.lblProporcionfrmReceta);
            this.Controls.Add(this.btnNuevoInsumofrmReceta);
            this.Controls.Add(this.cmbInsumofrmReceta);
            this.Controls.Add(this.lblInsumofrmReceta);
            this.Controls.Add(this.lblRecetafrmReceta);
            this.Controls.Add(this.dgvProductosfrmReceta);
            this.Controls.Add(this.lblProductosfrmReceta);
            this.Controls.Add(this.rdbConRecetafrmReceta);
            this.Controls.Add(this.rdbSinRecetafrmReceta);
            this.Name = "frmReceta";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmReceta";
            this.Load += new System.EventHandler(this.frmReceta_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductosfrmReceta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudProporcionfrmReceta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecetafrmReceta)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdbSinRecetafrmReceta;
        private System.Windows.Forms.RadioButton rdbConRecetafrmReceta;
        private System.Windows.Forms.Label lblProductosfrmReceta;
        private System.Windows.Forms.DataGridView dgvProductosfrmReceta;
        private System.Windows.Forms.Label lblRecetafrmReceta;
        private System.Windows.Forms.Label lblInsumofrmReceta;
        private System.Windows.Forms.ComboBox cmbInsumofrmReceta;
        private System.Windows.Forms.Button btnNuevoInsumofrmReceta;
        private System.Windows.Forms.Label lblProporcionfrmReceta;
        private System.Windows.Forms.NumericUpDown nudProporcionfrmReceta;
        private System.Windows.Forms.Button btnAgregarInsumofrmReceta;
        private System.Windows.Forms.DataGridView dgvRecetafrmReceta;
        private System.Windows.Forms.Button btnQuitarInsumofrmReceta;
        private System.Windows.Forms.Label lblAyudaProporcionfrmReceta;
        private System.Windows.Forms.Button btnConfirmarfrmReceta;
        private System.Windows.Forms.Button btnCancelarfrmReceta;
    }
}
