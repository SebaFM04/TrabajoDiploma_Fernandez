namespace UI
{
    partial class frmLogin
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblLeyendaIniciofrmLogin = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnlniciarSesionfrmLogin = new System.Windows.Forms.Button();
            this.lblContraseñafrmLogin = new System.Windows.Forms.Label();
            this.lblCorreoElectronicofrmLogin = new System.Windows.Forms.Label();
            this.btnMostrarContraseñafrmLogIn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblLeyendaIniciofrmLogin
            // 
            this.lblLeyendaIniciofrmLogin.AutoSize = true;
            this.lblLeyendaIniciofrmLogin.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLeyendaIniciofrmLogin.Location = new System.Drawing.Point(162, 38);
            this.lblLeyendaIniciofrmLogin.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLeyendaIniciofrmLogin.Name = "lblLeyendaIniciofrmLogin";
            this.lblLeyendaIniciofrmLogin.Size = new System.Drawing.Size(272, 20);
            this.lblLeyendaIniciofrmLogin.TabIndex = 28;
            this.lblLeyendaIniciofrmLogin.Text = "¡Bienvenido a nuestro sistema! ";
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.textBox2.Location = new System.Drawing.Point(166, 170);
            this.textBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox2.Name = "textBox2";
            this.textBox2.PasswordChar = '*';
            this.textBox2.Size = new System.Drawing.Size(244, 25);
            this.textBox2.TabIndex = 26;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.textBox1.Location = new System.Drawing.Point(166, 127);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(244, 25);
            this.textBox1.TabIndex = 25;
            // 
            // btnlniciarSesionfrmLogin
            // 
            this.btnlniciarSesionfrmLogin.BackColor = System.Drawing.Color.SandyBrown;
            this.btnlniciarSesionfrmLogin.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnlniciarSesionfrmLogin.Location = new System.Drawing.Point(196, 221);
            this.btnlniciarSesionfrmLogin.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnlniciarSesionfrmLogin.Name = "btnlniciarSesionfrmLogin";
            this.btnlniciarSesionfrmLogin.Size = new System.Drawing.Size(162, 38);
            this.btnlniciarSesionfrmLogin.TabIndex = 23;
            this.btnlniciarSesionfrmLogin.Text = "Iniciar Sesión";
            this.btnlniciarSesionfrmLogin.UseVisualStyleBackColor = false;
            this.btnlniciarSesionfrmLogin.Click += new System.EventHandler(this.btniniciarSesionfrmLogin_Click);
            // 
            // lblContraseñafrmLogin
            // 
            this.lblContraseñafrmLogin.AutoSize = true;
            this.lblContraseñafrmLogin.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContraseñafrmLogin.Location = new System.Drawing.Point(13, 175);
            this.lblContraseñafrmLogin.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblContraseñafrmLogin.Name = "lblContraseñafrmLogin";
            this.lblContraseñafrmLogin.Size = new System.Drawing.Size(95, 19);
            this.lblContraseñafrmLogin.TabIndex = 22;
            this.lblContraseñafrmLogin.Text = "Contraseña";
            // 
            // lblCorreoElectronicofrmLogin
            // 
            this.lblCorreoElectronicofrmLogin.AutoSize = true;
            this.lblCorreoElectronicofrmLogin.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCorreoElectronicofrmLogin.Location = new System.Drawing.Point(13, 132);
            this.lblCorreoElectronicofrmLogin.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCorreoElectronicofrmLogin.Name = "lblCorreoElectronicofrmLogin";
            this.lblCorreoElectronicofrmLogin.Size = new System.Drawing.Size(147, 19);
            this.lblCorreoElectronicofrmLogin.TabIndex = 21;
            this.lblCorreoElectronicofrmLogin.Text = "Correo Electrónico";
            // 
            // btnMostrarContraseñafrmLogIn
            // 
            this.btnMostrarContraseñafrmLogIn.BackColor = System.Drawing.Color.SandyBrown;
            this.btnMostrarContraseñafrmLogIn.Font = new System.Drawing.Font("MS Reference Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMostrarContraseñafrmLogIn.Location = new System.Drawing.Point(422, 170);
            this.btnMostrarContraseñafrmLogIn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnMostrarContraseñafrmLogIn.Name = "btnMostrarContraseñafrmLogIn";
            this.btnMostrarContraseñafrmLogIn.Size = new System.Drawing.Size(74, 25);
            this.btnMostrarContraseñafrmLogIn.TabIndex = 29;
            this.btnMostrarContraseñafrmLogIn.Text = "Mostrar";
            this.btnMostrarContraseñafrmLogIn.UseVisualStyleBackColor = false;
            this.btnMostrarContraseñafrmLogIn.Click += new System.EventHandler(this.btnMostrarContraseñafrmLogIn_Click);
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.NavajoWhite;
            this.ClientSize = new System.Drawing.Size(542, 353);
            this.Controls.Add(this.btnMostrarContraseñafrmLogIn);
            this.Controls.Add(this.lblLeyendaIniciofrmLogin);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnlniciarSesionfrmLogin);
            this.Controls.Add(this.lblContraseñafrmLogin);
            this.Controls.Add(this.lblCorreoElectronicofrmLogin);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLeyendaIniciofrmLogin;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnlniciarSesionfrmLogin;
        private System.Windows.Forms.Label lblContraseñafrmLogin;
        private System.Windows.Forms.Label lblCorreoElectronicofrmLogin;
        private System.Windows.Forms.Button btnMostrarContraseñafrmLogIn;
    }
}

