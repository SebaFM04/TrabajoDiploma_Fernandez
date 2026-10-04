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
    // CU021 Registrar Pago a Proveedor (DSS-CU021). Actor: Dueño.
    public partial class frmPagoProveedor : Form, IObservadorIdioma
    {
        ORDEN_COMPRA_BLL GestorOrden = new ORDEN_COMPRA_BLL();
        PAGO_PROVEEDOR_BLL GestorPago = new PAGO_PROVEEDOR_BLL();
        bool cargando = false;

        public frmPagoProveedor()
        {
            InitializeComponent();
            // Las columnas se crean acá: GUIManager traduce el formulario antes del Load
            Enlazar();
        }

        private void frmPagoProveedor_Load(object sender, EventArgs e)
        {
            CargarOrdenes(true);
        }

        private void Enlazar()
        {
            var g = GestorIdioma.Instancia;
            dgvOrdenesfrmPagoProveedor.Columns.Clear();
            foreach (string col in new[] { "colNumerofrmPagoProveedor", "colProveedorfrmPagoProveedor", "colFechafrmPagoProveedor", "colTotalfrmPagoProveedor" })
                dgvOrdenesfrmPagoProveedor.Columns.Add(col, g.Traducir(col));
            AjusteGrilla.Configurar(dgvOrdenesfrmPagoProveedor);

            dgvFacturasfrmPagoProveedor.Columns.Clear();
            foreach (string col in new[] { "colFechaRecepcionfrmPagoProveedor", "colRemitofrmPagoProveedor", "colFacturafrmPagoProveedor", "colImportefrmPagoProveedor" })
                dgvFacturasfrmPagoProveedor.Columns.Add(col, g.Traducir(col));
            AjusteGrilla.Configurar(dgvFacturasfrmPagoProveedor);

            cmbMediofrmPagoProveedor.Items.Clear();
            cmbMediofrmPagoProveedor.Items.AddRange(GestorPago.ListarMediosPago());
            dtpFechafrmPagoProveedor.MaxDate = DateTime.Today;
        }

        // CU021 paso 2 / FA1 (mensajes 2-10): órdenes "Cerrada" con su total
        private void CargarOrdenes(bool avisarSiVacio)
        {
            var g = GestorIdioma.Instancia;
            cargando = true;
            dgvOrdenesfrmPagoProveedor.Rows.Clear();
            var ordenes = new List<BE.ORDEN_COMPRA>();
            try
            {
                ordenes = GestorOrden.ListarPorEstado(ORDEN_COMPRA_BLL.EstadoCerrada);
                foreach (var o in ordenes)
                {
                    int fila = dgvOrdenesfrmPagoProveedor.Rows.Add(o.IdOrdenCompra, o.Proveedor.RazonSocial,
                        o.FechaGeneracion.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture), FormatoMoneda.Pesos(GestorOrden.CalcularTotal(o.IdOrdenCompra)));
                    dgvOrdenesfrmPagoProveedor.Rows[fila].Tag = o;
                }
                dgvOrdenesfrmPagoProveedor.ClearSelection();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
            finally
            {
                cargando = false;
            }
            LimpiarPago();
            if (avisarSiVacio && ordenes.Count == 0)
                MessageBox.Show(g.Traducir("msgPagoSinOrdenes"), g.Traducir("frmPagoProveedor"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LimpiarPago()
        {
            dgvFacturasfrmPagoProveedor.Rows.Clear();
            lblTotalfrmPagoProveedor.Text = GestorIdioma.Instancia.Traducir("lblTotalfrmPagoProveedor");
            cmbMediofrmPagoProveedor.SelectedIndex = -1;
            dtpFechafrmPagoProveedor.Value = DateTime.Today;
            txtComprobantefrmPagoProveedor.Text = txtMontofrmPagoProveedor.Text = string.Empty;
            btnRegistrarfrmPagoProveedor.Enabled = false;
        }

        private BE.ORDEN_COMPRA OrdenSeleccionada()
        {
            return dgvOrdenesfrmPagoProveedor.SelectedRows.Count > 0 ? dgvOrdenesfrmPagoProveedor.SelectedRows[0].Tag as BE.ORDEN_COMPRA : null;
        }

        // CU021 paso 3 (mensajes 11-20): proveedor, facturas asociadas y total a pagar
        private void dgvOrdenesfrmPagoProveedor_SelectionChanged(object sender, EventArgs e)
        {
            if (cargando) return;
            LimpiarPago();
            var seleccionada = OrdenSeleccionada();
            if (seleccionada == null) return;
            try
            {
                var orden = GestorOrden.ObtenerDetalle(seleccionada.IdOrdenCompra);
                foreach (var r in GestorOrden.ListarRecepciones(orden.IdOrdenCompra))
                    dgvFacturasfrmPagoProveedor.Rows.Add(r.FechaRecepcion.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture), r.NumeroRemito, r.NumeroFacturaProveedor,
                        FormatoMoneda.Pesos(r.Detalles.Sum(d => d.CantidadRecibida * d.CostoUnidadCompra)));
                decimal total = GestorOrden.CalcularTotal(orden.IdOrdenCompra);
                lblTotalfrmPagoProveedor.Text = string.Format(GestorIdioma.Instancia.Traducir("msgPagoTotal"), orden.Proveedor.RazonSocial, FormatoMoneda.Pesos(total));
                txtMontofrmPagoProveedor.Text = FormatoMoneda.Numero(total);
                btnRegistrarfrmPagoProveedor.Enabled = true;
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        // CU021 paso 4 / FA2 (mensajes 21-38)
        private void btnRegistrarfrmPagoProveedor_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            var orden = OrdenSeleccionada();
            if (orden == null) return;
            if (!FormatoMoneda.TryLeer(txtMontofrmPagoProveedor.Text, out decimal monto))
            {
                Avisar("msgPagoMontoInvalido");
                return;
            }
            var pago = new BE.PAGO_PROVEEDOR
            {
                IdOrdenCompra = orden.IdOrdenCompra,
                MedioPago = cmbMediofrmPagoProveedor.SelectedItem as string,
                FechaPago = dtpFechafrmPagoProveedor.Value.Date,
                NumeroComprobante = txtComprobantefrmPagoProveedor.Text,
                Monto = monto
            };
            try
            {
                GestorPago.ValidarPago(pago);
                GestorPago.RegistrarPago(pago);
                MessageBox.Show(g.Traducir("msgPagoRegistrado"), g.Traducir("frmPagoProveedor"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarOrdenes(false);
            }
            catch (ArgumentException argEx)
            {
                Avisar(argEx.Message);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void Avisar(string clave)
        {
            var g = GestorIdioma.Instancia;
            MessageBox.Show(g.Traducir(clave), g.Traducir("msgOrdenAviso"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void MostrarError(Exception ex)
        {
            var g = GestorIdioma.Instancia;
            MessageBox.Show(g.Traducir("msgPagoError") + ex.GetBaseException().Message, g.Traducir("frmPagoProveedor"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ActualizarIdioma()
        {
            var g = GestorIdioma.Instancia;
            this.Text = g.Traducir(this.Name);
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is DataGridView || ctrl is TextBox || ctrl is ComboBox || ctrl is DateTimePicker || ctrl == lblTotalfrmPagoProveedor)
                    continue;
                ctrl.Text = g.Traducir(ctrl.Name);
            }
            foreach (DataGridViewColumn col in dgvOrdenesfrmPagoProveedor.Columns)
                col.HeaderText = g.Traducir(col.Name);
            foreach (DataGridViewColumn col in dgvFacturasfrmPagoProveedor.Columns)
                col.HeaderText = g.Traducir(col.Name);
            if (OrdenSeleccionada() == null)
                lblTotalfrmPagoProveedor.Text = g.Traducir("lblTotalfrmPagoProveedor");
        }
    }
}
