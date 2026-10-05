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
    // Consulta de compras (decisión 63): órdenes de compra del período con sus líneas, recepciones (facturas), reclamos y pago.
    public partial class frmConsultaCompras : Form, IObservadorIdioma
    {
        ORDEN_COMPRA_BLL GestorOrden = new ORDEN_COMPRA_BLL();
        PROVEEDOR_BLL GestorProveedor = new PROVEEDOR_BLL();
        RECLAMO_BLL GestorReclamo = new RECLAMO_BLL();
        List<BE.ORDEN_COMPRA> ordenes = new List<BE.ORDEN_COMPRA>();
        // Evita que la carga de la grilla dispare la selección
        bool cargando = false;
        int idOrdenMostrada = -1;

        public frmConsultaCompras()
        {
            InitializeComponent();
            // Las columnas se crean acá: GUIManager traduce el formulario antes del Load
            Enlazar();
        }

        private void frmConsultaCompras_Load(object sender, EventArgs e)
        {
            dtpDesdefrmConsultaCompras.Value = DateTime.Today.AddMonths(-1);
            dtpHastafrmConsultaCompras.Value = DateTime.Today;
            CargarFiltros();
            Buscar();
        }

        private void Enlazar()
        {
            var g = GestorIdioma.Instancia;
            AgregarColumnas(dgvOrdenesfrmConsultaCompras, "colNumerofrmConsultaCompras", "colProveedorfrmConsultaCompras", "colFechafrmConsultaCompras",
                "colEstadofrmConsultaCompras", "colAprobacionfrmConsultaCompras", "colRecibidofrmConsultaCompras", "colPagofrmConsultaCompras");
            AgregarColumnas(dgvLineasfrmConsultaCompras, "colInsumofrmConsultaCompras", "colUnidadComprafrmConsultaCompras", "colPedidafrmConsultaCompras",
                "colCantidadRecibidafrmConsultaCompras", "colPendientefrmConsultaCompras");
            AgregarColumnas(dgvRecepcionesfrmConsultaCompras, "colFechaRecepcionfrmConsultaCompras", "colRemitofrmConsultaCompras",
                "colFacturafrmConsultaCompras", "colImportefrmConsultaCompras");
            AgregarColumnas(dgvReclamosfrmConsultaCompras, "colFechaReclamofrmConsultaCompras", "colEstadoReclamofrmConsultaCompras",
                "colInsumoReclamofrmConsultaCompras", "colCantidadReclamofrmConsultaCompras", "colMotivofrmConsultaCompras", "colDescripcionfrmConsultaCompras");
        }

        private void AgregarColumnas(DataGridView dgv, params string[] columnas)
        {
            var g = GestorIdioma.Instancia;
            dgv.Columns.Clear();
            foreach (string col in columnas)
                dgv.Columns.Add(col, g.Traducir(col));
            AjusteGrilla.Configurar(dgv);
        }

        // Filtros: "Todos" + estados de la orden; "Todos" + proveedores activos
        private void CargarFiltros()
        {
            var g = GestorIdioma.Instancia;
            object estado = cmbEstadofrmConsultaCompras.SelectedValue;
            object proveedor = cmbProveedorfrmConsultaCompras.SelectedValue;
            var estados = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>(string.Empty, g.Traducir("msgConsultaComprasTodos")) };
            estados.AddRange(GestorOrden.ListarEstados().Select(e => new KeyValuePair<string, string>(e, e)));
            cmbEstadofrmConsultaCompras.DisplayMember = "Value";
            cmbEstadofrmConsultaCompras.ValueMember = "Key";
            cmbEstadofrmConsultaCompras.DataSource = estados;
            if (estado != null) cmbEstadofrmConsultaCompras.SelectedValue = estado;
            try
            {
                var proveedores = new List<KeyValuePair<int, string>> { new KeyValuePair<int, string>(0, g.Traducir("msgConsultaComprasTodos")) };
                proveedores.AddRange(GestorProveedor.ListarProveedores().Select(p => new KeyValuePair<int, string>(p.IdProveedor, p.RazonSocial)));
                cmbProveedorfrmConsultaCompras.DisplayMember = "Value";
                cmbProveedorfrmConsultaCompras.ValueMember = "Key";
                cmbProveedorfrmConsultaCompras.DataSource = proveedores;
                if (proveedor != null) cmbProveedorfrmConsultaCompras.SelectedValue = proveedor;
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void Buscar()
        {
            var g = GestorIdioma.Instancia;
            cargando = true;
            dgvOrdenesfrmConsultaCompras.Rows.Clear();
            ordenes = new List<BE.ORDEN_COMPRA>();
            try
            {
                string estado = cmbEstadofrmConsultaCompras.SelectedValue as string;
                int idProveedor = cmbProveedorfrmConsultaCompras.SelectedValue is int id ? id : 0;
                ordenes = GestorOrden.ListarOrdenes(dtpDesdefrmConsultaCompras.Value, dtpHastafrmConsultaCompras.Value,
                    string.IsNullOrEmpty(estado) ? null : estado, idProveedor > 0 ? idProveedor : (int?)null);
                foreach (var o in ordenes)
                {
                    int fila = dgvOrdenesfrmConsultaCompras.Rows.Add(
                        o.IdOrdenCompra,
                        o.Proveedor.RazonSocial,
                        o.FechaGeneracion.ToString("dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture),
                        o.Estado,
                        o.FechaAprobacion?.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture) ?? "-",
                        FormatoMoneda.Pesos(o.TotalRecibido),
                        o.Pago != null ? $"{o.Pago.FechaPago.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture)} - {FormatoMoneda.Pesos(o.Pago.Monto)}" : g.Traducir("msgConsultaComprasSinPago"));
                    dgvOrdenesfrmConsultaCompras.Rows[fila].Tag = o;
                }
            }
            catch (ArgumentException argEx)
            {
                MessageBox.Show(g.Traducir(argEx.Message), g.Traducir("frmConsultaCompras"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            idOrdenMostrada = -1;
            // Queda seleccionada la orden más reciente con su detalle
            if (dgvOrdenesfrmConsultaCompras.Rows.Count > 0)
            {
                dgvOrdenesfrmConsultaCompras.CurrentCell = dgvOrdenesfrmConsultaCompras.Rows[0].Cells[0];
                dgvOrdenesfrmConsultaCompras.Rows[0].Selected = true;
            }
            MostrarDetalle(OrdenSeleccionada());
        }

        private void MostrarTotales()
        {
            lblTotalesfrmConsultaCompras.Text = string.Format(GestorIdioma.Instancia.Traducir("lblTotalesfrmConsultaCompras"),
                ordenes.Count, FormatoMoneda.Pesos(ordenes.Sum(o => o.TotalRecibido)), FormatoMoneda.Pesos(ordenes.Where(o => o.Pago != null).Sum(o => o.Pago.Monto)));
        }

        // La fila seleccionada; si no hay selección visual, la fila actual
        private BE.ORDEN_COMPRA OrdenSeleccionada()
        {
            var fila = dgvOrdenesfrmConsultaCompras.SelectedRows.Count > 0 ? dgvOrdenesfrmConsultaCompras.SelectedRows[0] : dgvOrdenesfrmConsultaCompras.CurrentRow;
            return fila?.Tag as BE.ORDEN_COMPRA;
        }

        private BE.ORDEN_COMPRA OrdenDeFila(int indice)
        {
            return indice >= 0 && indice < dgvOrdenesfrmConsultaCompras.Rows.Count ? dgvOrdenesfrmConsultaCompras.Rows[indice].Tag as BE.ORDEN_COMPRA : null;
        }

        private void btnBuscarfrmConsultaCompras_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        // El detalle sigue a la orden elegida (la fila se toma del evento, como en la consulta de ventas)
        private void dgvOrdenesfrmConsultaCompras_SelectionChanged(object sender, EventArgs e)
        {
            if (!cargando && dgvOrdenesfrmConsultaCompras.SelectedRows.Count > 0)
                MostrarDetalle(dgvOrdenesfrmConsultaCompras.SelectedRows[0].Tag as BE.ORDEN_COMPRA);
        }

        private void dgvOrdenesfrmConsultaCompras_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!cargando) MostrarDetalle(OrdenDeFila(e.RowIndex));
        }

        private void dgvOrdenesfrmConsultaCompras_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!cargando && e.RowIndex >= 0) MostrarDetalle(OrdenDeFila(e.RowIndex));
        }

        // Líneas, recepciones con sus facturas, reclamos y resumen (aprobación, observaciones y pago)
        private void MostrarDetalle(BE.ORDEN_COMPRA cabecera)
        {
            var g = GestorIdioma.Instancia;
            if (cabecera != null && cabecera.IdOrdenCompra == idOrdenMostrada) return;
            dgvLineasfrmConsultaCompras.Rows.Clear();
            dgvRecepcionesfrmConsultaCompras.Rows.Clear();
            dgvReclamosfrmConsultaCompras.Rows.Clear();
            txtResumenfrmConsultaCompras.Text = string.Empty;
            idOrdenMostrada = cabecera?.IdOrdenCompra ?? -1;
            lblDetallefrmConsultaCompras.Text = g.Traducir("lblDetallefrmConsultaCompras") + (cabecera != null ? $" N° {cabecera.IdOrdenCompra}" : string.Empty);
            if (cabecera == null) return;
            try
            {
                var orden = GestorOrden.ObtenerDetalle(cabecera.IdOrdenCompra);
                foreach (var d in orden.Detalles)
                    dgvLineasfrmConsultaCompras.Rows.Add(d.Insumo.Nombre, d.Insumo.UnidadCompra, d.CantidadPedida, d.CantidadRecibida, d.CantidadPendiente);
                foreach (var r in GestorOrden.ListarRecepciones(orden.IdOrdenCompra))
                    dgvRecepcionesfrmConsultaCompras.Rows.Add(r.FechaRecepcion.ToString("dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture), r.NumeroRemito,
                        r.NumeroFacturaProveedor, FormatoMoneda.Pesos(r.Detalles.Sum(d => d.CantidadRecibida * d.CostoUnidadCompra)));
                foreach (var rc in GestorReclamo.ListarPorOrden(orden.IdOrdenCompra))
                    foreach (var d in rc.Detalles)
                        dgvReclamosfrmConsultaCompras.Rows.Add(rc.FechaReclamo.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture), rc.Estado, d.Insumo.Nombre,
                            d.CantidadPendiente, g.Traducir(d.Motivo == RECLAMO_BLL.MotivoFaltante ? "msgReclamoMotivoFaltante" : "msgReclamoMotivoDaniado"), d.Descripcion ?? string.Empty);
                txtResumenfrmConsultaCompras.Text = Resumen(cabecera, orden);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private string Resumen(BE.ORDEN_COMPRA cabecera, BE.ORDEN_COMPRA orden)
        {
            var g = GestorIdioma.Instancia;
            var lineas = new List<string>
            {
                string.Format(g.Traducir("msgConsultaComprasGenerada"), orden.FechaGeneracion.ToString("dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture), orden.Estado),
                orden.FechaAprobacion.HasValue
                    ? string.Format(g.Traducir("msgConsultaComprasAprobada"), orden.FechaAprobacion.Value.ToString("dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture))
                    : g.Traducir("msgConsultaComprasSinAprobar")
            };
            if (!string.IsNullOrWhiteSpace(orden.Observaciones))
                lineas.Add(string.Format(g.Traducir("msgConsultaComprasObservaciones"), orden.Observaciones));
            lineas.Add(cabecera.Pago != null
                ? string.Format(g.Traducir("msgConsultaComprasPagada"), cabecera.Pago.FechaPago.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture),
                    cabecera.Pago.MedioPago, cabecera.Pago.NumeroComprobante, FormatoMoneda.Pesos(cabecera.Pago.Monto))
                : g.Traducir("msgConsultaComprasSinPago"));
            return string.Join(Environment.NewLine, lineas);
        }

        private void MostrarError(Exception ex)
        {
            var g = GestorIdioma.Instancia;
            MessageBox.Show(g.Traducir("msgConsultaError") + ex.GetBaseException().Message, g.Traducir("frmConsultaCompras"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ActualizarIdioma()
        {
            var g = GestorIdioma.Instancia;
            this.Text = g.Traducir(this.Name);
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is DataGridView || ctrl is DateTimePicker || ctrl is ComboBox || ctrl is TextBox
                    || ctrl == lblTotalesfrmConsultaCompras || ctrl == lblDetallefrmConsultaCompras)
                    continue;
                ctrl.Text = g.Traducir(ctrl.Name);
            }
            foreach (var dgv in new[] { dgvOrdenesfrmConsultaCompras, dgvLineasfrmConsultaCompras, dgvRecepcionesfrmConsultaCompras, dgvReclamosfrmConsultaCompras })
                foreach (DataGridViewColumn col in dgv.Columns)
                    col.HeaderText = g.Traducir(col.Name);
            MostrarTotales();
            // "Todos", "Sin pagar", motivos y resumen se vuelven a armar en el idioma nuevo
            if (IsHandleCreated)
            {
                CargarFiltros();
                Buscar();
            }
            else
            {
                lblDetallefrmConsultaCompras.Text = g.Traducir("lblDetallefrmConsultaCompras");
            }
        }
    }
}
