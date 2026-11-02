namespace UI
{
    partial class frmLote
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
            this.rdbVencidosfrmLote = new System.Windows.Forms.RadioButton();
            this.rdbPorVencerfrmLote = new System.Windows.Forms.RadioButton();
            this.dgvLotesfrmLote = new System.Windows.Forms.DataGridView();
            this.lblTotalfrmLote = new System.Windows.Forms.Label();
            this.btnDarDeBajafrmLote = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLotesfrmLote)).BeginInit();
            this.SuspendLayout();
            //
            // rdbVencidosfrmLote
            //
            this.rdbVencidosfrmLote.AutoSize = true;
            this.rdbVencidosfrmLote.Checked = true;
            this.rdbVencidosfrmLote.Location = new System.Drawing.Point(20, 15);
            this.rdbVencidosfrmLote.Name = "rdbVencidosfrmLote";
            this.rdbVencidosfrmLote.TabIndex = 0;
            this.rdbVencidosfrmLote.TabStop = true;
            this.rdbVencidosfrmLote.Text = "Vencidos sin dar de baja";
            this.rdbVencidosfrmLote.UseVisualStyleBackColor = true;
            this.rdbVencidosfrmLote.CheckedChanged += new System.EventHandler(this.Filtro_CheckedChanged);
            //
            // rdbPorVencerfrmLote
            //
            this.rdbPorVencerfrmLote.AutoSize = true;
            this.rdbPorVencerfrmLote.Location = new System.Drawing.Point(300, 15);
            this.rdbPorVencerfrmLote.Name = "rdbPorVencerfrmLote";
            this.rdbPorVencerfrmLote.TabIndex = 1;
            this.rdbPorVencerfrmLote.Text = "Por vencer (15 días)";
            this.rdbPorVencerfrmLote.UseVisualStyleBackColor = true;
            this.rdbPorVencerfrmLote.CheckedChanged += new System.EventHandler(this.Filtro_CheckedChanged);
            //
            // dgvLotesfrmLote
            //
            this.dgvLotesfrmLote.AllowUserToAddRows = false;
            this.dgvLotesfrmLote.AllowUserToDeleteRows = false;
            this.dgvLotesfrmLote.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLotesfrmLote.Location = new System.Drawing.Point(20, 45);
            this.dgvLotesfrmLote.MultiSelect = false;
            this.dgvLotesfrmLote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
            this.dgvLotesfrmLote.Name = "dgvLotesfrmLote";
            this.dgvLotesfrmLote.ReadOnly = true;
            this.dgvLotesfrmLote.RowHeadersVisible = false;
            this.dgvLotesfrmLote.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLotesfrmLote.Size = new System.Drawing.Size(760, 280);
            this.dgvLotesfrmLote.TabIndex = 2;
            //
            // lblTotalfrmLote
            //
            this.lblTotalfrmLote.AutoSize = true;
            this.lblTotalfrmLote.Location = new System.Drawing.Point(20, 340);
            this.lblTotalfrmLote.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.lblTotalfrmLote.Name = "lblTotalfrmLote";
            this.lblTotalfrmLote.TabIndex = 3;
            this.lblTotalfrmLote.Text = "-";
            //
            // btnDarDeBajafrmLote
            //
            this.btnDarDeBajafrmLote.BackColor = System.Drawing.Color.SandyBrown;
            this.btnDarDeBajafrmLote.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDarDeBajafrmLote.Location = new System.Drawing.Point(580, 333);
            this.btnDarDeBajafrmLote.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right));
            this.btnDarDeBajafrmLote.AutoSize = true;
            this.btnDarDeBajafrmLote.MinimumSize = new System.Drawing.Size(200, 38);
            this.btnDarDeBajafrmLote.Name = "btnDarDeBajafrmLote";
            this.btnDarDeBajafrmLote.Size = new System.Drawing.Size(200, 38);
            this.btnDarDeBajafrmLote.TabIndex = 4;
            this.btnDarDeBajafrmLote.Text = "Dar de baja";
            this.btnDarDeBajafrmLote.UseVisualStyleBackColor = false;
            this.btnDarDeBajafrmLote.Click += new System.EventHandler(this.btnDarDeBajafrmLote_Click);
            //
            // frmLote
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.NavajoWhite;
            this.ClientSize = new System.Drawing.Size(800, 385);
            this.Controls.Add(this.btnDarDeBajafrmLote);
            this.Controls.Add(this.lblTotalfrmLote);
            this.Controls.Add(this.dgvLotesfrmLote);
            this.Controls.Add(this.rdbPorVencerfrmLote);
            this.Controls.Add(this.rdbVencidosfrmLote);
            this.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(640, 360);
            this.Name = "frmLote";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmLote";
            this.Load += new System.EventHandler(this.frmLote_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLotesfrmLote)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdbVencidosfrmLote;
        private System.Windows.Forms.RadioButton rdbPorVencerfrmLote;
        private System.Windows.Forms.DataGridView dgvLotesfrmLote;
        private System.Windows.Forms.Label lblTotalfrmLote;
        private System.Windows.Forms.Button btnDarDeBajafrmLote;
    }
}
