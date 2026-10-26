using BLL;
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
    // Simulador de ventas (decisiones 88 y 89): herramienta de soporte para la demo de N03, no es un CU de negocio
    public partial class frmSimuladorVentas : Form, IObservadorIdioma
    {
        SIMULADOR_BLL GestorSimulador = new SIMULADOR_BLL();

        public frmSimuladorVentas()
        {
            InitializeComponent();
        }

        private void frmSimuladorVentas_Load(object sender, EventArgs e)
        {
            MostrarEstado();
        }

        // Ventas simuladas que hay en la base y su rango de fechas
        private void MostrarEstado()
        {
            var g = GestorIdioma.Instancia;
            try
            {
                int ventas = GestorSimulador.ObtenerEstado(out DateTime? desde, out DateTime? hasta);
                lblEstadofrmSimuladorVentas.Text = ventas == 0
                    ? g.Traducir("msgSimuladorSinSimulacion")
                    : string.Format(g.Traducir("msgSimuladorEstado"), ventas, desde?.ToString("dd/MM/yyyy"), hasta?.ToString("dd/MM/yyyy"));
                btnGenerarfrmSimuladorVentas.Enabled = ventas == 0;
                btnBorrarfrmSimuladorVentas.Enabled = ventas > 0;
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void btnGenerarfrmSimuladorVentas_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            Cursor = Cursors.WaitCursor;
            try
            {
                int ventas = GestorSimulador.GenerarSimulacion((int)nudMesesfrmSimuladorVentas.Value, (int)nudVentasNochefrmSimuladorVentas.Value, (int)nudSemillafrmSimuladorVentas.Value);
                Cursor = Cursors.Default;
                MessageBox.Show(string.Format(g.Traducir("msgSimuladorGenerada"), ventas), g.Traducir("frmSimuladorVentas"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (ArgumentException argEx)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(g.Traducir(argEx.Message), g.Traducir("frmSimuladorVentas"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MostrarError(ex);
            }
            MostrarEstado();
        }

        // Decisión 89: borra físicamente las ventas simuladas, con confirmación
        private void btnBorrarfrmSimuladorVentas_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            if (MessageBox.Show(g.Traducir("msgSimuladorConfirmarBorrar"), g.Traducir("frmSimuladorVentas"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            Cursor = Cursors.WaitCursor;
            try
            {
                int ventas = GestorSimulador.BorrarSimulacion();
                Cursor = Cursors.Default;
                MessageBox.Show(string.Format(g.Traducir("msgSimuladorBorrada"), ventas), g.Traducir("frmSimuladorVentas"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (ArgumentException argEx)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(g.Traducir(argEx.Message), g.Traducir("frmSimuladorVentas"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MostrarError(ex);
            }
            MostrarEstado();
        }

        private void MostrarError(Exception ex)
        {
            var g = GestorIdioma.Instancia;
            MessageBox.Show(g.Traducir("msgSimuladorError") + ex.GetBaseException().Message, g.Traducir("frmSimuladorVentas"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ActualizarIdioma()
        {
            var g = GestorIdioma.Instancia;
            this.Text = g.Traducir(this.Name);
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is NumericUpDown || ctrl == lblEstadofrmSimuladorVentas)
                    continue;
                ctrl.Text = g.Traducir(ctrl.Name);
            }
            if (IsHandleCreated)
                MostrarEstado();
        }
    }
}
