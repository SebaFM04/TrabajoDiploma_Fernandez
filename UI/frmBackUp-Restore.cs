using BLL;
using SERVICIO;
using SERVICIO.MultiIdioma_Observer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class frmBackUp_Restore : Form, IObservadorIdioma
    {
        BACKUP_BLL gestorBackup = new BACKUP_BLL();
        RESTORE_BLL gestorRestore = new RESTORE_BLL();
        PRODUCTO_BLL gestorProducto = new PRODUCTO_BLL();
        public frmBackUp_Restore()
        {
            InitializeComponent();
        }

        private void btnFrmBRCrearBackUp_Click(object sender, EventArgs e)
        {
            string ruta = textBox1.Text.Trim();
            try
            {
                gestorBackup.GenerarBackup(ruta);
                MessageBox.Show($"Backup generado exitosamente en:\n{ruta}","Backup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario,"Backup",$"Se realizó backup en: {ruta}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al realizar el backup: " + ex.GetBaseException().Message,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFrmBRRestaurar_Click(object sender, EventArgs e)
        {
            string ruta = textBox2.Text.Trim();

            var confirm = MessageBox.Show($"¿Confirma que desea restaurar la base de datos desde:\n{ruta}\n\n" +"Esta acción reemplazará todos los datos actuales.","Confirmar Restore", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                gestorRestore.RestaurarBackup(ruta);
                MessageBox.Show("Base de datos restaurada correctamente.", "Restore", MessageBoxButtons.OK, MessageBoxIcon.Information);
                new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario,"Restore",$"Se restauró la BD desde: {ruta}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al restaurar: " + ex.GetBaseException().Message,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFrmBRVerError_Click(object sender, EventArgs e)
        {
            try
            {
                var errores = gestorProducto.VerificarIntegridad();
                if (errores.Count == 0)
                {
                    MessageBox.Show("La integridad de los datos es correcta. " + "No se detectaron errores.","Verificación DV", MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Se detectaron errores de integridad:\n\n" + string.Join("\n", errores),"Errores de Integridad", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al verificar integridad: " + ex.GetBaseException().Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFrmBRSolucionarErrores_Click(object sender, EventArgs e)
        {
            var errores = gestorProducto.VerificarIntegridad();
            if (errores.Count == 0)
            {
                MessageBox.Show("No hay errores que solucionar.","Sin errores", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string mensaje = "Se detectaron errores de integridad:\n\n" + string.Join("\n", errores) + "\n\n¿Qué desea hacer?\n" + "SÍ  → Recalcular dígitos (acepta datos actuales)\n" +"NO → Cancelar";

            var resultado = MessageBox.Show(mensaje, "Solucionar Errores", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado != DialogResult.Yes) return;

            try
            {
                gestorProducto.RecalcularDV();
                MessageBox.Show("Dígitos verificadores recalculados correctamente.","Solucionado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Recalcular DV", "Se recalcularon los dígitos verificadores.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al recalcular: " + ex.GetBaseException().Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ActualizarIdioma()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is TextBox || ctrl is DataGridView || ctrl is ComboBox)
                    continue;
                ctrl.Text = GestorIdioma.Instancia.Traducir(ctrl.Name);
            }
        }
    }
}
