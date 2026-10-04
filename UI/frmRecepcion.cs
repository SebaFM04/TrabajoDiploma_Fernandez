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
    // CU019 Registrar Recepción de Mercadería (DSS-CU019) y CU020 Registrar Reclamo al Proveedor (DSS-CU020). Actor: Encargado de Stock.
    public partial class frmRecepcion : Form, IObservadorIdioma
    {
        ORDEN_COMPRA_BLL GestorOrden = new ORDEN_COMPRA_BLL();
        RECEPCION_BLL GestorRecepcion = new RECEPCION_BLL();
        RECLAMO_BLL GestorReclamo = new RECLAMO_BLL();
        bool cargando = false;
        // CU020: recepción parcial que espera su reclamo
        BE.RECEPCION recepcionParcial = null;

        public frmRecepcion()
        {
            InitializeComponent();
            // Las columnas se crean acá: GUIManager traduce el formulario antes del Load
            Enlazar();
        }

        private void frmRecepcion_Load(object sender, EventArgs e)
        {
            CargarOrdenes(true);
        }

        private void Enlazar()
        {
            var g = GestorIdioma.Instancia;
            dgvOrdenesfrmRecepcion.Columns.Clear();
            foreach (string col in new[] { "colNumerofrmRecepcion", "colProveedorfrmRecepcion", "colEstadofrmRecepcion" })
                dgvOrdenesfrmRecepcion.Columns.Add(col, g.Traducir(col));
            AjusteGrilla.Configurar(dgvOrdenesfrmRecepcion);

            dgvInsumosfrmRecepcion.Columns.Clear();
            foreach (string col in new[] { "colInsumofrmRecepcion", "colUnidadComprafrmRecepcion", "colPedidafrmRecepcion", "colPendientefrmRecepcion", "colRecibidafrmRecepcion", "colCostofrmRecepcion" })
                dgvInsumosfrmRecepcion.Columns.Add(col, g.Traducir(col));
            foreach (DataGridViewColumn col in dgvInsumosfrmRecepcion.Columns)
                col.ReadOnly = col.Name != "colRecibidafrmRecepcion" && col.Name != "colCostofrmRecepcion";
            AjusteGrilla.Configurar(dgvInsumosfrmRecepcion);

            // CU020: motivo con valores de la base y texto traducido
            dgvReclamofrmRecepcion.Columns.Clear();
            dgvReclamofrmRecepcion.Columns.Add("colInsumoReclamofrmRecepcion", g.Traducir("colInsumofrmRecepcion"));
            dgvReclamofrmRecepcion.Columns.Add("colCantidadPendientefrmRecepcion", g.Traducir("colPendientefrmRecepcion"));
            var motivo = new DataGridViewComboBoxColumn { Name = "colMotivofrmRecepcion", HeaderText = g.Traducir("colMotivofrmRecepcion"), DisplayMember = "Value", ValueMember = "Key" };
            motivo.DataSource = Motivos();
            dgvReclamofrmRecepcion.Columns.Add(motivo);
            dgvReclamofrmRecepcion.Columns.Add("colDescripcionfrmRecepcion", g.Traducir("colDescripcionfrmRecepcion"));
            dgvReclamofrmRecepcion.Columns["colInsumoReclamofrmRecepcion"].ReadOnly = true;
            dgvReclamofrmRecepcion.Columns["colCantidadPendientefrmRecepcion"].ReadOnly = true;
            ((DataGridViewTextBoxColumn)dgvReclamofrmRecepcion.Columns["colDescripcionfrmRecepcion"]).MaxInputLength = 200;
            AjusteGrilla.Configurar(dgvReclamofrmRecepcion);
        }

        private List<KeyValuePair<string, string>> Motivos()
        {
            var g = GestorIdioma.Instancia;
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(RECLAMO_BLL.MotivoFaltante, g.Traducir("msgReclamoMotivoFaltante")),
                new KeyValuePair<string, string>(RECLAMO_BLL.MotivoDaniado, g.Traducir("msgReclamoMotivoDaniado"))
            };
        }

        // CU019 paso 2 / FA1: órdenes "Aprobada" o "Recibida parcialmente"
        private void CargarOrdenes(bool avisarSiVacio)
        {
            var g = GestorIdioma.Instancia;
            cargando = true;
            dgvOrdenesfrmRecepcion.Rows.Clear();
            dgvInsumosfrmRecepcion.Rows.Clear();
            txtRemitofrmRecepcion.Text = txtFacturafrmRecepcion.Text = string.Empty;
            var ordenes = new List<BE.ORDEN_COMPRA>();
            try
            {
                ordenes = GestorOrden.ListarPendientesRecepcion();
                foreach (var o in ordenes)
                {
                    int fila = dgvOrdenesfrmRecepcion.Rows.Add(o.IdOrdenCompra, o.Proveedor.RazonSocial, o.Estado);
                    dgvOrdenesfrmRecepcion.Rows[fila].Tag = o;
                }
                dgvOrdenesfrmRecepcion.ClearSelection();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
            finally
            {
                cargando = false;
            }
            btnRegistrarRecepcionfrmRecepcion.Enabled = false;
            if (avisarSiVacio && ordenes.Count == 0)
                MessageBox.Show(g.Traducir("msgRecepcionSinOrdenes"), g.Traducir("frmRecepcion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private BE.ORDEN_COMPRA OrdenSeleccionada()
        {
            return dgvOrdenesfrmRecepcion.SelectedRows.Count > 0 ? dgvOrdenesfrmRecepcion.SelectedRows[0].Tag as BE.ORDEN_COMPRA : null;
        }

        // CU019 pasos 3 y 4: insumos con la cantidad pedida y la pendiente. Se propone recibir todo lo pendiente al costo actual.
        private void dgvOrdenesfrmRecepcion_SelectionChanged(object sender, EventArgs e)
        {
            if (cargando || recepcionParcial != null) return;
            dgvInsumosfrmRecepcion.Rows.Clear();
            var seleccionada = OrdenSeleccionada();
            btnRegistrarRecepcionfrmRecepcion.Enabled = seleccionada != null;
            if (seleccionada == null) return;
            try
            {
                var orden = GestorOrden.ObtenerDetalle(seleccionada.IdOrdenCompra);
                foreach (var d in orden.Detalles)
                {
                    int fila = dgvInsumosfrmRecepcion.Rows.Add(d.Insumo.Nombre, d.Insumo.UnidadCompra, d.CantidadPedida, d.CantidadPendiente,
                        d.CantidadPendiente.ToString(), FormatoMoneda.Numero(d.Insumo.CostoUnidadCompra));
                    dgvInsumosfrmRecepcion.Rows[fila].Tag = d;
                    if (d.CantidadPendiente == 0)
                        dgvInsumosfrmRecepcion.Rows[fila].ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        // Recepción con lo cargado en la grilla. Devuelve null si un número no es válido (FA2).
        private BE.RECEPCION LeerRecepcion(BE.ORDEN_COMPRA orden)
        {
            dgvInsumosfrmRecepcion.EndEdit();
            var recepcion = new BE.RECEPCION
            {
                IdOrdenCompra = orden.IdOrdenCompra,
                NumeroRemito = txtRemitofrmRecepcion.Text,
                NumeroFacturaProveedor = txtFacturafrmRecepcion.Text
            };
            foreach (DataGridViewRow fila in dgvInsumosfrmRecepcion.Rows)
            {
                var linea = fila.Tag as BE.ORDEN_COMPRA_DETALLE;
                string textoCantidad = fila.Cells["colRecibidafrmRecepcion"].Value?.ToString();
                int cantidad = 0;
                if (!string.IsNullOrWhiteSpace(textoCantidad) && !int.TryParse(textoCantidad, out cantidad))
                    return null;
                if (cantidad == 0) continue;
                if (!FormatoMoneda.TryLeer(fila.Cells["colCostofrmRecepcion"].Value?.ToString(), out decimal costo))
                    return null;
                recepcion.Detalles.Add(new BE.RECEPCION_DETALLE { IdInsumo = linea.IdInsumo, CantidadRecibida = cantidad, CostoUnidadCompra = costo, Insumo = linea.Insumo });
            }
            return recepcion;
        }

        // CU019 pasos 5 y 6 (FA2 datos inválidos, FA3 recepción incompleta → CU020)
        private void btnRegistrarRecepcionfrmRecepcion_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            var orden = OrdenSeleccionada();
            if (orden == null) return;
            var recepcion = LeerRecepcion(orden);
            if (recepcion == null)
            {
                Avisar("msgRecepcionCantidadInvalida");
                return;
            }
            try
            {
                GestorRecepcion.ValidarRecepcion(recepcion);
                string estado = GestorRecepcion.RegistrarRecepcion(recepcion);
                if (estado == ORDEN_COMPRA_BLL.EstadoCerrada)
                {
                    MessageBox.Show(g.Traducir("msgRecepcionCerrada"), g.Traducir("frmRecepcion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarOrdenes(false);
                    return;
                }
                // FA3: quedan pendientes → CU020 Registrar Reclamo al Proveedor
                MessageBox.Show(g.Traducir("msgRecepcionParcial"), g.Traducir("frmRecepcion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                recepcionParcial = recepcion;
                MostrarReclamo(orden.IdOrdenCompra);
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

        // CU020 paso 1: insumos con diferencias y su cantidad pendiente
        private void MostrarReclamo(int idOrdenCompra)
        {
            dgvReclamofrmRecepcion.Rows.Clear();
            foreach (var d in GestorReclamo.ObtenerDiferencias(idOrdenCompra))
            {
                int fila = dgvReclamofrmRecepcion.Rows.Add(d.Insumo.Nombre, d.CantidadPendiente, null, string.Empty);
                dgvReclamofrmRecepcion.Rows[fila].Tag = d;
            }
            HabilitarReclamo(true);
        }

        // Mientras el reclamo está pendiente, la recepción queda bloqueada
        private void HabilitarReclamo(bool reclamo)
        {
            lblReclamofrmRecepcion.Visible = dgvReclamofrmRecepcion.Visible = btnRegistrarReclamofrmRecepcion.Visible = reclamo;
            dgvOrdenesfrmRecepcion.Enabled = dgvInsumosfrmRecepcion.Enabled = btnRegistrarRecepcionfrmRecepcion.Enabled = !reclamo;
            txtRemitofrmRecepcion.Enabled = txtFacturafrmRecepcion.Enabled = !reclamo;
        }

        // CU020 pasos 2 y 3 (FA1 motivo faltante)
        private void btnRegistrarReclamofrmRecepcion_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            if (recepcionParcial == null) return;
            dgvReclamofrmRecepcion.EndEdit();
            var reclamo = new BE.RECLAMO { IdRecepcion = recepcionParcial.IdRecepcion };
            foreach (DataGridViewRow fila in dgvReclamofrmRecepcion.Rows)
            {
                var d = fila.Tag as BE.RECLAMO_DETALLE;
                reclamo.Detalles.Add(new BE.RECLAMO_DETALLE
                {
                    IdInsumo = d.IdInsumo,
                    CantidadPendiente = d.CantidadPendiente,
                    Motivo = fila.Cells["colMotivofrmRecepcion"].Value?.ToString(),
                    Descripcion = fila.Cells["colDescripcionfrmRecepcion"].Value?.ToString(),
                    Insumo = d.Insumo
                });
            }
            try
            {
                GestorReclamo.ValidarReclamo(reclamo);
                GestorReclamo.RegistrarReclamo(reclamo);
                MessageBox.Show(g.Traducir("msgReclamoRegistrado"), g.Traducir("frmRecepcion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                recepcionParcial = null;
                HabilitarReclamo(false);
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

        // CU019 FA3 ejecuta CU020: se avisa si se cierra con el reclamo sin registrar
        private void frmRecepcion_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (recepcionParcial == null) return;
            var g = GestorIdioma.Instancia;
            if (MessageBox.Show(g.Traducir("msgReclamoPendienteCerrar"), g.Traducir("frmRecepcion"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                e.Cancel = true;
        }

        private void Avisar(string clave)
        {
            var g = GestorIdioma.Instancia;
            MessageBox.Show(g.Traducir(clave), g.Traducir("msgOrdenAviso"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void MostrarError(Exception ex)
        {
            var g = GestorIdioma.Instancia;
            MessageBox.Show(g.Traducir("msgRecepcionError") + ex.GetBaseException().Message, g.Traducir("frmRecepcion"), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            foreach (DataGridViewColumn col in dgvOrdenesfrmRecepcion.Columns)
                col.HeaderText = g.Traducir(col.Name);
            foreach (DataGridViewColumn col in dgvInsumosfrmRecepcion.Columns)
                col.HeaderText = g.Traducir(col.Name);
            dgvReclamofrmRecepcion.Columns["colInsumoReclamofrmRecepcion"].HeaderText = g.Traducir("colInsumofrmRecepcion");
            dgvReclamofrmRecepcion.Columns["colCantidadPendientefrmRecepcion"].HeaderText = g.Traducir("colPendientefrmRecepcion");
            dgvReclamofrmRecepcion.Columns["colMotivofrmRecepcion"].HeaderText = g.Traducir("colMotivofrmRecepcion");
            dgvReclamofrmRecepcion.Columns["colDescripcionfrmRecepcion"].HeaderText = g.Traducir("colDescripcionfrmRecepcion");
            ((DataGridViewComboBoxColumn)dgvReclamofrmRecepcion.Columns["colMotivofrmRecepcion"]).DataSource = Motivos();
        }
    }
}
