namespace UI
{
    partial class frmControlCambio
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
            this.lblfrmCCTitu = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnRevertirCambio = new System.Windows.Forms.Button();
            this.btnRevertirTodo = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblfrmCCTitu
            // 
            this.lblfrmCCTitu.AutoSize = true;
            this.lblfrmCCTitu.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblfrmCCTitu.Location = new System.Drawing.Point(9, 22);
            this.lblfrmCCTitu.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblfrmCCTitu.Name = "lblfrmCCTitu";
            this.lblfrmCCTitu.Size = new System.Drawing.Size(161, 19);
            this.lblfrmCCTitu.TabIndex = 7;
            this.lblfrmCCTitu.Text = "Historial de Cambios";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 59);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(868, 301);
            this.dataGridView1.TabIndex = 8;
            // 
            // btnRevertirCambio
            // 
            this.btnRevertirCambio.BackColor = System.Drawing.Color.SandyBrown;
            this.btnRevertirCambio.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRevertirCambio.Location = new System.Drawing.Point(608, 14);
            this.btnRevertirCambio.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnRevertirCambio.Name = "btnRevertirCambio";
            this.btnRevertirCambio.Size = new System.Drawing.Size(134, 36);
            this.btnRevertirCambio.TabIndex = 9;
            this.btnRevertirCambio.Text = "Revertir Cambio";
            this.btnRevertirCambio.UseVisualStyleBackColor = false;
            this.btnRevertirCambio.Click += new System.EventHandler(this.btnFrmBRRestaurar_Click);
            // 
            // btnRevertirTodo
            // 
            this.btnRevertirTodo.BackColor = System.Drawing.Color.SandyBrown;
            this.btnRevertirTodo.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRevertirTodo.Location = new System.Drawing.Point(746, 14);
            this.btnRevertirTodo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnRevertirTodo.Name = "btnRevertirTodo";
            this.btnRevertirTodo.Size = new System.Drawing.Size(134, 36);
            this.btnRevertirTodo.TabIndex = 10;
            this.btnRevertirTodo.Text = "Revertir Todo";
            this.btnRevertirTodo.UseVisualStyleBackColor = false;
            this.btnRevertirTodo.Click += new System.EventHandler(this.btnRevertirTodo_Click);
            // 
            // frmControlCambio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.NavajoWhite;
            this.ClientSize = new System.Drawing.Size(898, 396);
            this.Controls.Add(this.btnRevertirTodo);
            this.Controls.Add(this.btnRevertirCambio);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lblfrmCCTitu);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "frmControlCambio";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmControlCambios";
            this.Load += new System.EventHandler(this.frmControlCambios_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblfrmCCTitu;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnRevertirCambio;
        private System.Windows.Forms.Button btnRevertirTodo;
    }
}