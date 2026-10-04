namespace UI
{
    partial class frmProducto
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
            this.btnModificacionfrmProducto = new System.Windows.Forms.Button();
            this.btnBajafrmProducto = new System.Windows.Forms.Button();
            this.btnAltafrmProducto = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.cmbTipofrmProducto = new System.Windows.Forms.ComboBox();
            this.lblTipofrmProducto = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lblNombrefrmProducto = new System.Windows.Forms.Label();
            this.lblTamaniosfrmProducto = new System.Windows.Forms.Label();
            this.dgvTamaniosfrmProducto = new System.Windows.Forms.DataGridView();
            this.btnNuevofrmProducto = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTamaniosfrmProducto)).BeginInit();
            this.SuspendLayout();
            // 
            // btnModificacionfrmProducto
            // 
            this.btnModificacionfrmProducto.BackColor = System.Drawing.Color.SandyBrown;
            this.btnModificacionfrmProducto.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnModificacionfrmProducto.Location = new System.Drawing.Point(334, 269);
            this.btnModificacionfrmProducto.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnModificacionfrmProducto.Name = "btnModificacionfrmProducto";
            this.btnModificacionfrmProducto.Size = new System.Drawing.Size(110, 38);
            this.btnModificacionfrmProducto.TabIndex = 9;
            this.btnModificacionfrmProducto.Text = "Modificar";
            this.btnModificacionfrmProducto.UseVisualStyleBackColor = false;
            this.btnModificacionfrmProducto.Click += new System.EventHandler(this.btnModificacionfrmProducto_Click);
            // 
            // btnBajafrmProducto
            // 
            this.btnBajafrmProducto.BackColor = System.Drawing.Color.SandyBrown;
            this.btnBajafrmProducto.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBajafrmProducto.Location = new System.Drawing.Point(213, 269);
            this.btnBajafrmProducto.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnBajafrmProducto.Name = "btnBajafrmProducto";
            this.btnBajafrmProducto.Size = new System.Drawing.Size(118, 38);
            this.btnBajafrmProducto.TabIndex = 8;
            this.btnBajafrmProducto.Text = "Dar de baja";
            this.btnBajafrmProducto.UseVisualStyleBackColor = false;
            this.btnBajafrmProducto.Click += new System.EventHandler(this.btnBajafrmProducto_Click);
            // 
            // btnAltafrmProducto
            // 
            this.btnAltafrmProducto.BackColor = System.Drawing.Color.SandyBrown;
            this.btnAltafrmProducto.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAltafrmProducto.Location = new System.Drawing.Point(110, 269);
            this.btnAltafrmProducto.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnAltafrmProducto.Name = "btnAltafrmProducto";
            this.btnAltafrmProducto.Size = new System.Drawing.Size(100, 38);
            this.btnAltafrmProducto.TabIndex = 7;
            this.btnAltafrmProducto.Text = "Agregar";
            this.btnAltafrmProducto.UseVisualStyleBackColor = false;
            this.btnAltafrmProducto.Click += new System.EventHandler(this.btnAltafrmProducto_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(455, 28);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(584, 311);
            this.dataGridView1.TabIndex = 10;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // cmbTipofrmProducto
            // 
            this.cmbTipofrmProducto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipofrmProducto.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTipofrmProducto.FormattingEnabled = true;
            this.cmbTipofrmProducto.Location = new System.Drawing.Point(144, 63);
            this.cmbTipofrmProducto.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbTipofrmProducto.Name = "cmbTipofrmProducto";
            this.cmbTipofrmProducto.Size = new System.Drawing.Size(271, 26);
            this.cmbTipofrmProducto.TabIndex = 2;
            this.cmbTipofrmProducto.SelectedIndexChanged += new System.EventHandler(this.cmbTipofrmProducto_SelectedIndexChanged);
            // 
            // lblTipofrmProducto
            // 
            this.lblTipofrmProducto.AutoSize = true;
            this.lblTipofrmProducto.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipofrmProducto.Location = new System.Drawing.Point(29, 69);
            this.lblTipofrmProducto.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTipofrmProducto.Name = "lblTipofrmProducto";
            this.lblTipofrmProducto.Size = new System.Drawing.Size(40, 19);
            this.lblTipofrmProducto.TabIndex = 59;
            this.lblTipofrmProducto.Text = "Tipo";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(144, 28);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(271, 25);
            this.textBox1.TabIndex = 1;
            // 
            // lblNombrefrmProducto
            // 
            this.lblNombrefrmProducto.AutoSize = true;
            this.lblNombrefrmProducto.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombrefrmProducto.Location = new System.Drawing.Point(29, 32);
            this.lblNombrefrmProducto.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNombrefrmProducto.Name = "lblNombrefrmProducto";
            this.lblNombrefrmProducto.Size = new System.Drawing.Size(69, 19);
            this.lblNombrefrmProducto.TabIndex = 55;
            this.lblNombrefrmProducto.Text = "Nombre";
            //
            // lblTamaniosfrmProducto
            //
            this.lblTamaniosfrmProducto.AutoSize = true;
            this.lblTamaniosfrmProducto.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTamaniosfrmProducto.Location = new System.Drawing.Point(29, 100);
            this.lblTamaniosfrmProducto.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTamaniosfrmProducto.Name = "lblTamaniosfrmProducto";
            this.lblTamaniosfrmProducto.Size = new System.Drawing.Size(135, 19);
            this.lblTamaniosfrmProducto.TabIndex = 60;
            this.lblTamaniosfrmProducto.Text = "Tamaños y precios";
            //
            // dgvTamaniosfrmProducto
            //
            this.dgvTamaniosfrmProducto.AllowUserToAddRows = false;
            this.dgvTamaniosfrmProducto.AllowUserToDeleteRows = false;
            this.dgvTamaniosfrmProducto.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTamaniosfrmProducto.Location = new System.Drawing.Point(29, 122);
            this.dgvTamaniosfrmProducto.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgvTamaniosfrmProducto.Name = "dgvTamaniosfrmProducto";
            this.dgvTamaniosfrmProducto.RowHeadersVisible = false;
            this.dgvTamaniosfrmProducto.RowHeadersWidth = 51;
            this.dgvTamaniosfrmProducto.RowTemplate.Height = 24;
            this.dgvTamaniosfrmProducto.Size = new System.Drawing.Size(415, 135);
            this.dgvTamaniosfrmProducto.TabIndex = 3;
            //
            // btnNuevofrmProducto
            //
            this.btnNuevofrmProducto.BackColor = System.Drawing.Color.SandyBrown;
            this.btnNuevofrmProducto.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNuevofrmProducto.Location = new System.Drawing.Point(22, 269);
            this.btnNuevofrmProducto.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnNuevofrmProducto.Name = "btnNuevofrmProducto";
            this.btnNuevofrmProducto.Size = new System.Drawing.Size(84, 38);
            this.btnNuevofrmProducto.TabIndex = 6;
            this.btnNuevofrmProducto.Text = "Nuevo";
            this.btnNuevofrmProducto.UseVisualStyleBackColor = false;
            this.btnNuevofrmProducto.Click += new System.EventHandler(this.btnNuevofrmProducto_Click);
            // 
            // frmProducto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.NavajoWhite;
            this.ClientSize = new System.Drawing.Size(1048, 412);
            this.Controls.Add(this.btnNuevofrmProducto);
            this.Controls.Add(this.dgvTamaniosfrmProducto);
            this.Controls.Add(this.lblTamaniosfrmProducto);
            this.Controls.Add(this.btnModificacionfrmProducto);
            this.Controls.Add(this.btnBajafrmProducto);
            this.Controls.Add(this.btnAltafrmProducto);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.cmbTipofrmProducto);
            this.Controls.Add(this.lblTipofrmProducto);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lblNombrefrmProducto);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "frmProducto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmProducto";
            this.Load += new System.EventHandler(this.frmProducto_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTamaniosfrmProducto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnModificacionfrmProducto;
        private System.Windows.Forms.Button btnBajafrmProducto;
        private System.Windows.Forms.Button btnAltafrmProducto;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox cmbTipofrmProducto;
        private System.Windows.Forms.Label lblTipofrmProducto;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lblNombrefrmProducto;
        private System.Windows.Forms.Label lblTamaniosfrmProducto;
        private System.Windows.Forms.DataGridView dgvTamaniosfrmProducto;
        private System.Windows.Forms.Button btnNuevofrmProducto;
    }
}