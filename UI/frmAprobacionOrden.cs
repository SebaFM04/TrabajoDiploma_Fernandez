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
    // CU017 Aprobar Orden de Compra (DSS-CU017). Actor: Dueño.
    public partial class frmAprobacionOrden : Form, IObservadorIdioma
    {
        ORDEN_COMPRA_BLL GestorOrden = new ORDEN_COMPRA_BLL();
        bool cargando = false;

        public frmAprobacionOrden()
        {
            InitializeComponent();
            // Las columnas se crean acá: GUIManager traduce el formulario antes del Load
            Enlazar();
        }

        private void frmAprobacionOrden_Load(object sender, EventArgs e)
        {
            CargarOrdenes(true);
        }

        private void Enlazar()
        {
            var g = GestorIdioma.Instancia;
            dgvOrdenesfrmAprobacionOrden.Columns.Clear();
            foreach (string col in new[] { "colNumerofrmAprobacionOrden", "colProveedorfrmAprobacionOrden", "colFechafrmAprobacionOrden" })
                dgvOrdenesfrmAprobacionOrden.Columns.Add(col, g.Traducir(col));
            dgvOrdenesfrmAprobacionOrden.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvDetallefrmAprobacionOrden.Columns.Clear();
            foreach (string col in new[] { "colInsumofrmAprobacionOrden", "colCantidadfrmAprobacionOrden", "colUnidadComprafrmAprobacionOrden", "colCostoEstimadofrmAprobacionOrden" })
                dgvDetallefrmAprobacionOrden.Columns.Add(col, g.Traducir(col));
            dgvDetallefrmAprobacionOrden.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // CU017 paso 2 / FA1: órdenes pendientes de aprobación
        private void CargarOrdenes(bool avisarSiVacio)
        {
            var g = GestorIdioma.Instancia;
            cargando = true;
            dgvOrdenesfrmAprobacionOrden.Rows.Clear();
            dgvDetallefrmAprobacionOrden.Rows.Clear();
            txtObservacionesfrmAprobacionOrden.Text = string.Empty;
            var ordenes = new List<BE.ORDEN_COMPRA>();
            try
            {
                ordenes = GestorOrden.ListarPorEstado(ORDEN_COMPRA_BLL.EstadoPendienteAprobacion);
                foreach (var o in ordenes)
                {
                    int fila = dgvOrdenesfrmAprobacionOrden.Rows.Add(o.IdOrdenCompra, o.Proveedor.RazonSocial, o.FechaGeneracion.ToString("dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture));
                    dgvOrdenesfrmAprobacionOrden.Rows[fila].Tag = o;
                }
                dgvOrdenesfrmAprobacionOrden.ClearSelection();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
            finally
            {
                cargando = false;
            }
            btnAprobarfrmAprobacionOrden.Enabled = btnDevolverfrmAprobacionOrden.Enabled = false;
            if (avisarSiVacio && ordenes.Count == 0)
                MessageBox.Show(g.Traducir("msgOrdenSinPendientesAprobacion"), g.Traducir("frmAprobacionOrden"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private BE.ORDEN_COMPRA OrdenSeleccionada()
        {
            return dgvOrdenesfrmAprobacionOrden.SelectedRows.Count > 0 ? dgvOrdenesfrmAprobacionOrden.SelectedRows[0].Tag as BE.ORDEN_COMPRA : null;
        }

        // CU017 pasos 3 y 4: detalle (proveedor, insumos y cantidades en unidades de compra)
        private void dgvOrdenesfrmAprobacionOrden_SelectionChanged(object sender, EventArgs e)
        {
            if (cargando) return;
            dgvDetallefrmAprobacionOrden.Rows.Clear();
            var seleccionada = OrdenSeleccionada();
            btnAprobarfrmAprobacionOrden.Enabled = btnDevolverfrmAprobacionOrden.Enabled = seleccionada != null;
            if (seleccionada == null) return;
            try
            {
                var orden = GestorOrden.ObtenerDetalle(seleccionada.IdOrdenCompra);
                foreach (var d in orden.Detalles)
                    dgvDetallefrmAprobacionOrden.Rows.Add(d.Insumo.Nombre, d.CantidadPedida, d.Insumo.UnidadCompra,
                        FormatoMoneda.Pesos(d.CantidadPedida * d.Insumo.CostoUnidadCompra));
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        // CU017 pasos 5 y 6
        private void btnAprobarfrmAprobacionOrden_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            var seleccionada = OrdenSeleccionada();
            if (seleccionada == null) return;
            try
            {
                GestorOrden.Aprobar(seleccionada.IdOrdenCompra);
                MessageBox.Show(g.Traducir("msgOrdenAprobada"), g.Traducir("frmAprobacionOrden"), MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        // CU017 FA2 y FA3: devolución con observaciones obligatorias
        private void btnDevolverfrmAprobacionOrden_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            var seleccionada = OrdenSeleccionada();
            if (seleccionada == null) return;
            try
            {
                GestorOrden.Devolver(seleccionada.IdOrdenCompra, txtObservacionesfrmAprobacionOrden.Text);
                MessageBox.Show(g.Traducir("msgOrdenDevuelta"), g.Traducir("frmAprobacionOrden"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarOrdenes(false);
            }
            catch (ArgumentException argEx)
            {
                Avisar(argEx.Message);
                txtObservacionesfrmAprobacionOrden.Focus();
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
            MessageBox.Show(g.Traducir("msgOrdenError") + ex.GetBaseException().Message, g.Traducir("frmAprobacionOrden"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ActualizarIdioma()
        {
            var g = GestorIdioma.Instancia;
            this.Text = g.Traducir(this.Name);
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is DataGridView || ctrl is TextBox)
                    continue;
                ctrl.Text = g.Traducir(ctrl.Name);
            }
            foreach (DataGridViewColumn col in dgvOrdenesfrmAprobacionOrden.Columns)
                col.HeaderText = g.Traducir(col.Name);
            foreach (DataGridViewColumn col in dgvDetallefrmAprobacionOrden.Columns)
                col.HeaderText = g.Traducir(col.Name);
        }
    }
}
