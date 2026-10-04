using BE;
using BLL;
using SERVICIO;
using SERVICIO.MultiIdioma_Observer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    // Consulta de vales emitidos (decisión 56): vales del período, con su venta y si ya se usaron (CU012).
    public partial class frmConsultaVales : Form, IObservadorIdioma
    {
        VALE_BLL GestorVale = new VALE_BLL();
        List<BE.VALE> vales = new List<BE.VALE>();
        // Valores del filtro, en el mismo orden que el combo
        readonly string[] estados = { VALE_BLL.EstadoTodos, VALE_BLL.EstadoSinUsar, VALE_BLL.EstadoUtilizados };
        readonly string[] clavesEstado = { "msgValeEstadoTodos", "msgValeEstadoSinUsar", "msgValeEstadoUtilizados" };

        public frmConsultaVales()
        {
            InitializeComponent();
            // Las columnas se crean acá: GUIManager traduce el formulario antes del Load
            Enlazar();
        }

        private void frmConsultaVales_Load(object sender, EventArgs e)
        {
            dtpDesdefrmConsultaVales.Value = DateTime.Today;
            dtpHastafrmConsultaVales.Value = DateTime.Today;
            Buscar();
        }

        private void Enlazar()
        {
            var g = GestorIdioma.Instancia;
            dgvValesfrmConsultaVales.Columns.Clear();
            foreach (string col in new[] { "colValefrmConsultaVales", "colFechafrmConsultaVales", "colVentafrmConsultaVales",
                                           "colMontofrmConsultaVales", "colBebidasfrmConsultaVales", "colUtilizadofrmConsultaVales" })
                dgvValesfrmConsultaVales.Columns.Add(col, g.Traducir(col));
            AjusteGrilla.Configurar(dgvValesfrmConsultaVales);
            CargarEstados();
        }

        private void CargarEstados()
        {
            var g = GestorIdioma.Instancia;
            int seleccionado = Math.Max(0, cmbEstadofrmConsultaVales.SelectedIndex);
            cmbEstadofrmConsultaVales.Items.Clear();
            foreach (string clave in clavesEstado)
                cmbEstadofrmConsultaVales.Items.Add(g.Traducir(clave));
            cmbEstadofrmConsultaVales.SelectedIndex = seleccionado;
        }

        private void Buscar()
        {
            var g = GestorIdioma.Instancia;
            dgvValesfrmConsultaVales.Rows.Clear();
            try
            {
                vales = GestorVale.ListarVales(dtpDesdefrmConsultaVales.Value, dtpHastafrmConsultaVales.Value, estados[Math.Max(0, cmbEstadofrmConsultaVales.SelectedIndex)]);
                foreach (var v in vales)
                {
                    int fila = dgvValesfrmConsultaVales.Rows.Add(v.IdVale,
                        v.FechaHoraEmision.ToString("dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture),
                        v.IdVenta,
                        FormatoMoneda.Pesos(v.MontoCertificado),
                        g.Traducir(v.TieneBebidas ? "msgConsultaSi" : "msgConsultaNo"),
                        g.Traducir(v.Utilizado ? "msgConsultaSi" : "msgConsultaNo"));
                    if (!v.Utilizado && v.TieneBebidas)
                        dgvValesfrmConsultaVales.Rows[fila].DefaultCellStyle.ForeColor = Color.DarkGreen;
                }
                dgvValesfrmConsultaVales.ClearSelection();
            }
            catch (ArgumentException argEx)
            {
                MessageBox.Show(g.Traducir(argEx.Message), g.Traducir("frmConsultaVales"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(g.Traducir("msgConsultaError") + ex.GetBaseException().Message, g.Traducir("frmConsultaVales"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            MostrarTotales();
        }

        private void MostrarTotales()
        {
            lblTotalesfrmConsultaVales.Text = string.Format(GestorIdioma.Instancia.Traducir("lblTotalesfrmConsultaVales"),
                vales.Count, vales.Count(v => !v.Utilizado), vales.Count(v => v.Utilizado));
        }

        private void btnBuscarfrmConsultaVales_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        public void ActualizarIdioma()
        {
            var g = GestorIdioma.Instancia;
            this.Text = g.Traducir(this.Name);
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is DataGridView || ctrl is DateTimePicker || ctrl is ComboBox || ctrl == lblTotalesfrmConsultaVales)
                    continue;
                ctrl.Text = g.Traducir(ctrl.Name);
            }
            foreach (DataGridViewColumn col in dgvValesfrmConsultaVales.Columns)
                col.HeaderText = g.Traducir(col.Name);
            CargarEstados();
            MostrarTotales();
        }
    }
}
