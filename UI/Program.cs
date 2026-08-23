using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            // Verificar integridad ANTES del login
            try
            {
                var errores = new BLL.PRODUCTO_BLL().VerificarIntegridad();
                if (errores.Count > 0)
                {
                    MessageBox.Show(
                        "Se detectaron errores de integridad:\n\n" + string.Join("\n", errores),
                        "Error de Integridad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                new BLL.IDIOMA_BLL().InicializarIdiomaUsuario(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo conectar a la base de datos:\n" + ex.GetBaseException().Message,
                    "Error crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Application.Run(new frmLogin());



        }
    }
}
