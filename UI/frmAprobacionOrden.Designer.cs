namespace UI
{
    partial class frmAprobacionOrden
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
            this.lblOrdenesfrmAprobacionOrden = new System.Windows.Forms.Label();
            this.dgvOrdenesfrmAprobacionOrden = new System.Windows.Forms.DataGridView();
            this.lblDetallefrmAprobacionOrden = new System.Windows.Forms.Label();
            this.dgvDetallefrmAprobacionOrden = new System.Windows.Forms.DataGridView();
            this.lblObservacionesfrmAprobacionOrden = new System.Windows.Forms.Label();
            this.txtObservacionesfrmAprobacionOrden = new System.Windows.Forms.TextBox();
            this.btnAprobarfrmAprobacionOrden = new System.Windows.Forms.Button();
            this.btnDevolverfrmAprobacionOrden = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrdenesfrmAprobacionOrden)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetallefrmAprobacionOrden)).BeginInit();
            this.SuspendLayout();
            //
            // lblOrdenesfrmAprobacionOrden
            //
            this.lblOrdenesfrmAprobacionOrden.AutoSize = true;
            this.lblOrdenesfrmAprobacionOrden.Location = new System.Drawing.Point(20, 15);
            this.lblOrdenesfrmAprobacionOrden.Name = "lblOrdenesfrmAprobacionOrden";
            this.lblOrdenesfrmAprobacionOrden.TabIndex = 0;
            this.lblOrdenesfrmAprobacionOrden.Text = "Órdenes pendientes de aprobación";
            //
            // dgvOrdenesfrmAprobacionOrden
            //
            this.dgvOrdenesfrmAprobacionOrden.AllowUserToAddRows = false;
            this.dgvOrdenesfrmAprobacionOrden.AllowUserToDeleteRows = false;
            this.dgvOrdenesfrmAprobacionOrden.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrdenesfrmAprobacionOrden.Location = new System.Drawing.Point(20, 40);
            this.dgvOrdenesfrmAprobacionOrden.MultiSelect = false;
            this.dgvOrdenesfrmAprobacionOrden.Name = "dgvOrdenesfrmAprobacionOrden";
            this.dgvOrdenesfrmAprobacionOrden.ReadOnly = true;
            this.dgvOrdenesfrmAprobacionOrden.RowHeadersVisible = false;
            this.dgvOrdenesfrmAprobacionOrden.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrdenesfrmAprobacionOrden.Size = new System.Drawing.Size(400, 420);
            this.dgvOrdenesfrmAprobacionOrden.TabIndex = 1;
            this.dgvOrdenesfrmAprobacionOrden.SelectionChanged += new System.EventHandler(this.dgvOrdenesfrmAprobacionOrden_SelectionChanged);
            //
            // lblDetallefrmAprobacionOrden
            //
            this.lblDetallefrmAprobacionOrden.AutoSize = true;
            this.lblDetallefrmAprobacionOrden.Location = new System.Drawing.Point(440, 15);
            this.lblDetallefrmAprobacionOrden.Name = "lblDetallefrmAprobacionOrden";
            this.lblDetallefrmAprobacionOrden.TabIndex = 2;
            this.lblDetallefrmAprobacionOrden.Text = "Detalle de la orden";
            //
            // dgvDetallefrmAprobacionOrden
            //
            this.dgvDetallefrmAprobacionOrden.AllowUserToAddRows = false;
            this.dgvDetallefrmAprobacionOrden.AllowUserToDeleteRows = false;
            this.dgvDetallefrmAprobacionOrden.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetallefrmAprobacionOrden.Location = new System.Drawing.Point(440, 40);
            this.dgvDetallefrmAprobacionOrden.Name = "dgvDetallefrmAprobacionOrden";
            this.dgvDetallefrmAprobacionOrden.ReadOnly = true;
            this.dgvDetallefrmAprobacionOrden.RowHeadersVisible = false;
            this.dgvDetallefrmAprobacionOrden.Size = new System.Drawing.Size(520, 260);
            this.dgvDetallefrmAprobacionOrden.TabIndex = 3;
            //
            // lblObservacionesfrmAprobacionOrden
            //
            this.lblObservacionesfrmAprobacionOrden.AutoSize = true;
            this.lblObservacionesfrmAprobacionOrden.Location = new System.Drawing.Point(440, 312);
            this.lblObservacionesfrmAprobacionOrden.Name = "lblObservacionesfrmAprobacionOrden";
            this.lblObservacionesfrmAprobacionOrden.TabIndex = 4;
            this.lblObservacionesfrmAprobacionOrden.Text = "Observaciones (para devolver la orden)";
            //
            // txtObservacionesfrmAprobacionOrden
            //
            this.txtObservacionesfrmAprobacionOrden.Location = new System.Drawing.Point(440, 335);
            this.txtObservacionesfrmAprobacionOrden.MaxLength = 500;
            this.txtObservacionesfrmAprobacionOrden.Multiline = true;
            this.txtObservacionesfrmAprobacionOrden.Name = "txtObservacionesfrmAprobacionOrden";
            this.txtObservacionesfrmAprobacionOrden.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtObservacionesfrmAprobacionOrden.Size = new System.Drawing.Size(520, 75);
            this.txtObservacionesfrmAprobacionOrden.TabIndex = 5;
            //
            // btnAprobarfrmAprobacionOrden
            //
            this.btnAprobarfrmAprobacionOrden.BackColor = System.Drawing.Color.SandyBrown;
            this.btnAprobarfrmAprobacionOrden.Location = new System.Drawing.Point(760, 422);
            this.btnAprobarfrmAprobacionOrden.Name = "btnAprobarfrmAprobacionOrden";
            this.btnAprobarfrmAprobacionOrden.Size = new System.Drawing.Size(200, 38);
            this.btnAprobarfrmAprobacionOrden.TabIndex = 7;
            this.btnAprobarfrmAprobacionOrden.Text = "Aprobar";
            this.btnAprobarfrmAprobacionOrden.UseVisualStyleBackColor = false;
            this.btnAprobarfrmAprobacionOrden.Click += new System.EventHandler(this.btnAprobarfrmAprobacionOrden_Click);
            //
            // btnDevolverfrmAprobacionOrden
            //
            this.btnDevolverfrmAprobacionOrden.BackColor = System.Drawing.Color.SandyBrown;
            this.btnDevolverfrmAprobacionOrden.Location = new System.Drawing.Point(440, 422);
            this.btnDevolverfrmAprobacionOrden.Name = "btnDevolverfrmAprobacionOrden";
            this.btnDevolverfrmAprobacionOrden.Size = new System.Drawing.Size(240, 38);
            this.btnDevolverfrmAprobacionOrden.TabIndex = 6;
            this.btnDevolverfrmAprobacionOrden.Text = "Devolver con observaciones";
            this.btnDevolverfrmAprobacionOrden.UseVisualStyleBackColor = false;
            this.btnDevolverfrmAprobacionOrden.Click += new System.EventHandler(this.btnDevolverfrmAprobacionOrden_Click);
            //
            // frmAprobacionOrden
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.NavajoWhite;
            this.ClientSize = new System.Drawing.Size(980, 480);
            this.Controls.Add(this.btnDevolverfrmAprobacionOrden);
            this.Controls.Add(this.btnAprobarfrmAprobacionOrden);
            this.Controls.Add(this.txtObservacionesfrmAprobacionOrden);
            this.Controls.Add(this.lblObservacionesfrmAprobacionOrden);
            this.Controls.Add(this.dgvDetallefrmAprobacionOrden);
            this.Controls.Add(this.lblDetallefrmAprobacionOrden);
            this.Controls.Add(this.dgvOrdenesfrmAprobacionOrden);
            this.Controls.Add(this.lblOrdenesfrmAprobacionOrden);
            this.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmAprobacionOrden";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmAprobacionOrden";
            this.Load += new System.EventHandler(this.frmAprobacionOrden_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrdenesfrmAprobacionOrden)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetallefrmAprobacionOrden)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblOrdenesfrmAprobacionOrden;
        private System.Windows.Forms.DataGridView dgvOrdenesfrmAprobacionOrden;
        private System.Windows.Forms.Label lblDetallefrmAprobacionOrden;
        private System.Windows.Forms.DataGridView dgvDetallefrmAprobacionOrden;
        private System.Windows.Forms.Label lblObservacionesfrmAprobacionOrden;
        private System.Windows.Forms.TextBox txtObservacionesfrmAprobacionOrden;
        private System.Windows.Forms.Button btnAprobarfrmAprobacionOrden;
        private System.Windows.Forms.Button btnDevolverfrmAprobacionOrden;
    }
}
