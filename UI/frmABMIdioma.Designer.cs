namespace UI
{
    partial class frmABMIdioma
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
            this.dgvIdiomas = new System.Windows.Forms.DataGridView();
            this.dgvTraduccion = new System.Windows.Forms.DataGridView();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnDeshabilitar = new System.Windows.Forms.Button();
            this.btnModificacionTraduccion = new System.Windows.Forms.Button();
            this.txtNombreIdioma = new System.Windows.Forms.TextBox();
            this.lblNombreIdioma = new System.Windows.Forms.Label();
            this.comboIdiomasABM = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIdiomas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTraduccion)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvIdiomas
            // 
            this.dgvIdiomas.AllowUserToAddRows = false;
            this.dgvIdiomas.AllowUserToDeleteRows = false;
            this.dgvIdiomas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvIdiomas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIdiomas.Location = new System.Drawing.Point(13, 13);
            this.dgvIdiomas.MultiSelect = false;
            this.dgvIdiomas.Name = "dgvIdiomas";
            this.dgvIdiomas.ReadOnly = true;
            this.dgvIdiomas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvIdiomas.Size = new System.Drawing.Size(240, 150);
            this.dgvIdiomas.TabIndex = 0;
            this.dgvIdiomas.SelectionChanged += new System.EventHandler(this.dgvIdiomas_SelectionChanged);
            // 
            // dgvTraduccion
            // 
            this.dgvTraduccion.AllowUserToAddRows = false;
            this.dgvTraduccion.AllowUserToDeleteRows = false;
            this.dgvTraduccion.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTraduccion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTraduccion.Location = new System.Drawing.Point(259, 13);
            this.dgvTraduccion.MultiSelect = false;
            this.dgvTraduccion.Name = "dgvTraduccion";
            this.dgvTraduccion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTraduccion.Size = new System.Drawing.Size(621, 356);
            this.dgvTraduccion.TabIndex = 1;
            // 
            // btnAgregar
            // 
            this.btnAgregar.BackColor = System.Drawing.Color.SandyBrown;
            this.btnAgregar.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgregar.Location = new System.Drawing.Point(11, 228);
            this.btnAgregar.Margin = new System.Windows.Forms.Padding(2);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(243, 32);
            this.btnAgregar.TabIndex = 9;
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.UseVisualStyleBackColor = false;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // btnEditar
            // 
            this.btnEditar.BackColor = System.Drawing.Color.SandyBrown;
            this.btnEditar.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditar.Location = new System.Drawing.Point(11, 264);
            this.btnEditar.Margin = new System.Windows.Forms.Padding(2);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(243, 32);
            this.btnEditar.TabIndex = 10;
            this.btnEditar.Text = "Editar";
            this.btnEditar.UseVisualStyleBackColor = false;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // btnDeshabilitar
            // 
            this.btnDeshabilitar.BackColor = System.Drawing.Color.SandyBrown;
            this.btnDeshabilitar.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeshabilitar.Location = new System.Drawing.Point(11, 301);
            this.btnDeshabilitar.Margin = new System.Windows.Forms.Padding(2);
            this.btnDeshabilitar.Name = "btnDeshabilitar";
            this.btnDeshabilitar.Size = new System.Drawing.Size(243, 32);
            this.btnDeshabilitar.TabIndex = 11;
            this.btnDeshabilitar.Text = "Deshabilitar";
            this.btnDeshabilitar.UseVisualStyleBackColor = false;
            this.btnDeshabilitar.Click += new System.EventHandler(this.btnDeshabilitar_Click);
            // 
            // btnModificacionTraduccion
            // 
            this.btnModificacionTraduccion.BackColor = System.Drawing.Color.SandyBrown;
            this.btnModificacionTraduccion.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnModificacionTraduccion.Location = new System.Drawing.Point(437, 374);
            this.btnModificacionTraduccion.Margin = new System.Windows.Forms.Padding(2);
            this.btnModificacionTraduccion.Name = "btnModificacionTraduccion";
            this.btnModificacionTraduccion.Size = new System.Drawing.Size(243, 32);
            this.btnModificacionTraduccion.TabIndex = 12;
            this.btnModificacionTraduccion.Text = "Modificar Traducciones";
            this.btnModificacionTraduccion.UseVisualStyleBackColor = false;
            this.btnModificacionTraduccion.Click += new System.EventHandler(this.btnModificacionTraduccion_Click);
            // 
            // txtNombreIdioma
            // 
            this.txtNombreIdioma.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F);
            this.txtNombreIdioma.Location = new System.Drawing.Point(12, 200);
            this.txtNombreIdioma.Name = "txtNombreIdioma";
            this.txtNombreIdioma.Size = new System.Drawing.Size(241, 23);
            this.txtNombreIdioma.TabIndex = 13;
            // 
            // lblNombreIdioma
            // 
            this.lblNombreIdioma.AutoSize = true;
            this.lblNombreIdioma.Location = new System.Drawing.Point(11, 181);
            this.lblNombreIdioma.Name = "lblNombreIdioma";
            this.lblNombreIdioma.Size = new System.Drawing.Size(78, 13);
            this.lblNombreIdioma.TabIndex = 14;
            this.lblNombreIdioma.Text = "Nombre Idioma";
            // 
            // comboIdiomasABM
            // 
            this.comboIdiomasABM.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F);
            this.comboIdiomasABM.FormattingEnabled = true;
            this.comboIdiomasABM.Location = new System.Drawing.Point(11, 344);
            this.comboIdiomasABM.Name = "comboIdiomasABM";
            this.comboIdiomasABM.Size = new System.Drawing.Size(242, 25);
            this.comboIdiomasABM.TabIndex = 15;
            this.comboIdiomasABM.SelectedIndexChanged += new System.EventHandler(this.comboIdiomasABM_SelectedIndexChanged);
            // 
            // frmABMIdioma
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.NavajoWhite;
            this.ClientSize = new System.Drawing.Size(892, 450);
            this.Controls.Add(this.comboIdiomasABM);
            this.Controls.Add(this.lblNombreIdioma);
            this.Controls.Add(this.txtNombreIdioma);
            this.Controls.Add(this.btnModificacionTraduccion);
            this.Controls.Add(this.btnDeshabilitar);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.dgvTraduccion);
            this.Controls.Add(this.dgvIdiomas);
            this.Name = "frmABMIdioma";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmABMIdioma";
            this.Load += new System.EventHandler(this.frmABMIdioma_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvIdiomas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTraduccion)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvIdiomas;
        private System.Windows.Forms.DataGridView dgvTraduccion;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnDeshabilitar;
        private System.Windows.Forms.Button btnModificacionTraduccion;
        private System.Windows.Forms.TextBox txtNombreIdioma;
        private System.Windows.Forms.Label lblNombreIdioma;
        private System.Windows.Forms.ComboBox comboIdiomasABM;
    }
}