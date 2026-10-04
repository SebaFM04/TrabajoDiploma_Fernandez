using BE;
using BLL;
using SERVICIO;
using SERVICIO.MultiIdioma_Observer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    // Consulta de ventas realizadas (decisión 56): ventas del período con su vale, factura y comanda.
    public partial class frmConsultaVentas : Form, IObservadorIdioma
    {
        VENTA_BLL GestorVenta = new VENTA_BLL();
        FACTURA_BLL GestorFactura = new FACTURA_BLL();
        List<BE.VENTA> ventas = new List<BE.VENTA>();
        // Evita que la carga de la grilla dispare la selección
        bool cargando = false;

        public frmConsultaVentas()
        {
            InitializeComponent();
            // Las columnas se crean acá: GUIManager traduce el formulario antes del Load
            Enlazar();
        }

        private void frmConsultaVentas_Load(object sender, EventArgs e)
        {
            dtpDesdefrmConsultaVentas.Value = DateTime.Today;
            dtpHastafrmConsultaVentas.Value = DateTime.Today;
            Buscar();
        }

        private void Enlazar()
        {
            var g = GestorIdioma.Instancia;
            dgvVentasfrmConsultaVentas.Columns.Clear();
            foreach (string col in new[] { "colVentafrmConsultaVentas", "colFechafrmConsultaVentas", "colTotalfrmConsultaVentas", "colMedioPagofrmConsultaVentas",
                                           "colValefrmConsultaVentas", "colValeUsadofrmConsultaVentas", "colFacturafrmConsultaVentas", "colComandafrmConsultaVentas" })
                dgvVentasfrmConsultaVentas.Columns.Add(col, g.Traducir(col));
            dgvVentasfrmConsultaVentas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvDetallefrmConsultaVentas.Columns.Clear();
            foreach (string col in new[] { "colProductofrmConsultaVentas", "colTamaniofrmConsultaVentas", "colCantidadfrmConsultaVentas", "colSubtotalfrmConsultaVentas" })
                dgvDetallefrmConsultaVentas.Columns.Add(col, g.Traducir(col));
            dgvDetallefrmConsultaVentas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void Buscar()
        {
            var g = GestorIdioma.Instancia;
            cargando = true;
            dgvVentasfrmConsultaVentas.Rows.Clear();
            dgvDetallefrmConsultaVentas.Rows.Clear();
            try
            {
                ventas = GestorVenta.ListarVentas(dtpDesdefrmConsultaVentas.Value, dtpHastafrmConsultaVentas.Value);
                foreach (var v in ventas)
                {
                    int fila = dgvVentasfrmConsultaVentas.Rows.Add(
                        v.IdVenta,
                        v.Fecha.ToString("dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture),
                        FormatoMoneda.Pesos(v.Monto),
                        v.MedioPago,
                        v.Vale?.IdVale.ToString() ?? "-",
                        v.Vale == null ? "-" : g.Traducir(v.Vale.Utilizado ? "msgConsultaSi" : "msgConsultaNo"),
                        v.Factura != null ? GestorFactura.NumeroCompleto(v.Factura) : g.Traducir("msgConsultaSinFactura"),
                        v.Comanda != null ? $"{v.Comanda.IdComanda} ({v.Comanda.Estado})" : "-");
                    dgvVentasfrmConsultaVentas.Rows[fila].Tag = v;
                }
                dgvVentasfrmConsultaVentas.ClearSelection();
            }
            catch (ArgumentException argEx)
            {
                MessageBox.Show(g.Traducir(argEx.Message), g.Traducir("frmConsultaVentas"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
            finally
            {
                cargando = false;
            }
            MostrarTotales();
        }

        private void MostrarTotales()
        {
            lblTotalesfrmConsultaVentas.Text = string.Format(GestorIdioma.Instancia.Traducir("lblTotalesfrmConsultaVentas"),
                ventas.Count, FormatoMoneda.Pesos(ventas.Sum(v => v.Monto)));
        }

        private BE.VENTA VentaSeleccionada()
        {
            if (dgvVentasfrmConsultaVentas.SelectedRows.Count == 0) return null;
            return dgvVentasfrmConsultaVentas.SelectedRows[0].Tag as BE.VENTA;
        }

        private void btnBuscarfrmConsultaVentas_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        private void dgvVentasfrmConsultaVentas_SelectionChanged(object sender, EventArgs e)
        {
            if (cargando) return;
            dgvDetallefrmConsultaVentas.Rows.Clear();
            var venta = VentaSeleccionada();
            if (venta == null) return;
            try
            {
                venta.Detalles = GestorVenta.ObtenerDetalleVenta(venta.IdVenta);
                foreach (var d in venta.Detalles)
                    dgvDetallefrmConsultaVentas.Rows.Add(d.ProductoTamanio.Producto.Nombre, d.ProductoTamanio.Tamanio.Nombre, d.Cantidad, FormatoMoneda.Pesos(d.MontoLinea));
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void btnVerFacturafrmConsultaVentas_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            var venta = VentaSeleccionada();
            if (venta?.Factura == null)
            {
                MessageBox.Show(g.Traducir("msgConsultaSeleccionarVenta"), g.Traducir("frmConsultaVentas"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                Process.Start(GestorFactura.ObtenerPdf(venta));
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void MostrarError(Exception ex)
        {
            var g = GestorIdioma.Instancia;
            MessageBox.Show(g.Traducir("msgConsultaError") + ex.GetBaseException().Message, g.Traducir("frmConsultaVentas"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ActualizarIdioma()
        {
            var g = GestorIdioma.Instancia;
            this.Text = g.Traducir(this.Name);
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is DataGridView || ctrl is DateTimePicker || ctrl == lblTotalesfrmConsultaVentas)
                    continue;
                ctrl.Text = g.Traducir(ctrl.Name);
            }
            foreach (DataGridViewColumn col in dgvVentasfrmConsultaVentas.Columns)
                col.HeaderText = g.Traducir(col.Name);
            foreach (DataGridViewColumn col in dgvDetallefrmConsultaVentas.Columns)
                col.HeaderText = g.Traducir(col.Name);
            MostrarTotales();
        }
    }
}
