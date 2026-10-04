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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnModificacionfrmProducto
            // 
            this.btnModificacionfrmProducto.BackColor = System.Drawing.Color.SandyBrown;
            this.btnModificacionfrmProducto.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnModificacionfrmProducto.Location = new System.Drawing.Point(424, 331);
            this.btnModificacionfrmProducto.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnModificacionfrmProducto.Name = "btnModificacionfrmProducto";
            this.btnModificacionfrmProducto.Size = new System.Drawing.Size(147, 47);
            this.btnModificacionfrmProducto.TabIndex = 9;
            this.btnModificacionfrmProducto.Text = "Modificar";
            this.btnModificacionfrmProducto.UseVisualStyleBackColor = false;
            this.btnModificacionfrmProducto.Click += new System.EventHandler(this.btnModificacionfrmProducto_Click);
            // 
            // btnBajafrmProducto
            // 
            this.btnBajafrmProducto.BackColor = System.Drawing.Color.SandyBrown;
            this.btnBajafrmProducto.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBajafrmProducto.Location = new System.Drawing.Point(220, 331);
            this.btnBajafrmProducto.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnBajafrmProducto.Name = "btnBajafrmProducto";
            this.btnBajafrmProducto.Size = new System.Drawing.Size(147, 47);
            this.btnBajafrmProducto.TabIndex = 8;
            this.btnBajafrmProducto.Text = "Dar de baja";
            this.btnBajafrmProducto.UseVisualStyleBackColor = false;
            this.btnBajafrmProducto.Click += new System.EventHandler(this.btnBajafrmProducto_Click);
            // 
            // btnAltafrmProducto
            // 
            this.btnAltafrmProducto.BackColor = System.Drawing.Color.SandyBrown;
            this.btnAltafrmProducto.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAltafrmProducto.Location = new System.Drawing.Point(29, 331);
            this.btnAltafrmProducto.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAltafrmProducto.Name = "btnAltafrmProducto";
            this.btnAltafrmProducto.Size = new System.Drawing.Size(147, 47);
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
            this.dataGridView1.Location = new System.Drawing.Point(607, 34);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(779, 383);
            this.dataGridView1.TabIndex = 10;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // cmbTipofrmProducto
            //
            this.cmbTipofrmProducto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipofrmProducto.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTipofrmProducto.FormattingEnabled = true;
            this.cmbTipofrmProducto.Location = new System.Drawing.Point(192, 78);
            this.cmbTipofrmProducto.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbTipofrmProducto.Name = "cmbTipofrmProducto";
            this.cmbTipofrmProducto.Size = new System.Drawing.Size(360, 30);
            this.cmbTipofrmProducto.TabIndex = 2;
            //
            // lblTipofrmProducto
            //
            this.lblTipofrmProducto.AutoSize = true;
            this.lblTipofrmProducto.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipofrmProducto.Location = new System.Drawing.Point(39, 85);
            this.lblTipofrmProducto.Name = "lblTipofrmProducto";
            this.lblTipofrmProducto.Size = new System.Drawing.Size(48, 23);
            this.lblTipofrmProducto.TabIndex = 59;
            this.lblTipofrmProducto.Text = "Tipo";
            //
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(192, 34);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(360, 29);
            this.textBox1.TabIndex = 1;
            // 
            // lblNombrefrmProducto
            // 
            this.lblNombrefrmProducto.AutoSize = true;
            this.lblNombrefrmProducto.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombrefrmProducto.Location = new System.Drawing.Point(39, 39);
            this.lblNombrefrmProducto.Name = "lblNombrefrmProducto";
            this.lblNombrefrmProducto.Size = new System.Drawing.Size(82, 23);
            this.lblNombrefrmProducto.TabIndex = 55;
            this.lblNombrefrmProducto.Text = "Nombre";
            //
            // frmProducto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.NavajoWhite;
            this.ClientSize = new System.Drawing.Size(1397, 507);
            this.Controls.Add(this.btnModificacionfrmProducto);
            this.Controls.Add(this.btnBajafrmProducto);
            this.Controls.Add(this.btnAltafrmProducto);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.cmbTipofrmProducto);
            this.Controls.Add(this.lblTipofrmProducto);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lblNombrefrmProducto);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "frmProducto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmProducto";
            this.Load += new System.EventHandler(this.frmProducto_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
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
    }
}