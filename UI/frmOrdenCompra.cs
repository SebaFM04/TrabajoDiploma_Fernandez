using BE;
using BLL;
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
    // CU016 Generar Orden de Compra (DSS-CU016) y CU018 Ajustar Orden Observada (DSS-CU018). Actor: Encargado de Stock.
    public partial class frmOrdenCompra : Form, IObservadorIdioma
    {
        INSUMO_BLL GestorInsumo = new INSUMO_BLL();
        PROVEEDOR_BLL GestorProveedor = new PROVEEDOR_BLL();
        ORDEN_COMPRA_BLL GestorOrden = new ORDEN_COMPRA_BLL();
        // Insumos con aviso pendiente (se marcan para pedir por defecto)
        List<int> idsConAviso = new List<int>();
        bool cargando = false;
        // CU018: orden observada que se está ajustando (null en modo "Nueva orden")
        BE.ORDEN_COMPRA ordenEnAjuste = null;

        private bool ModoAjuste
        {
            get { return rdbObservadasfrmOrdenCompra.Checked; }
        }

        public frmOrdenCompra()
        {
            InitializeComponent();
            // Las columnas se crean acá: GUIManager traduce el formulario antes del Load
            Enlazar();
        }

        private void frmOrdenCompra_Load(object sender, EventArgs e)
        {
            CargarAvisos(true);
            CargarProveedores(null);
        }

        private void Enlazar()
        {
            var g = GestorIdioma.Instancia;
            dgvAvisosfrmOrdenCompra.Columns.Clear();
            foreach (string col in new[] { "colInsumofrmOrdenCompra", "colStockfrmOrdenCompra", "colUmbralfrmOrdenCompra" })
                dgvAvisosfrmOrdenCompra.Columns.Add(col, g.Traducir(col));
            AjusteGrilla.Configurar(dgvAvisosfrmOrdenCompra);

            dgvPedidofrmOrdenCompra.Columns.Clear();
            dgvPedidofrmOrdenCompra.Columns.Add(new DataGridViewCheckBoxColumn { Name = "colPedirfrmOrdenCompra", HeaderText = g.Traducir("colPedirfrmOrdenCompra"), FillWeight = 40 });
            foreach (string col in new[] { "colInsumoPedidofrmOrdenCompra", "colUnidadComprafrmOrdenCompra", "colEquivalenciafrmOrdenCompra", "colCantidadfrmOrdenCompra", "colAvisofrmOrdenCompra" })
                dgvPedidofrmOrdenCompra.Columns.Add(col, g.Traducir(col == "colInsumoPedidofrmOrdenCompra" ? "colInsumofrmOrdenCompra" : col));
            foreach (DataGridViewColumn col in dgvPedidofrmOrdenCompra.Columns)
                col.ReadOnly = col.Name != "colPedirfrmOrdenCompra" && col.Name != "colCantidadfrmOrdenCompra";
            AjusteGrilla.Configurar(dgvPedidofrmOrdenCompra);

            dgvObservadasfrmOrdenCompra.Columns.Clear();
            foreach (string col in new[] { "colNumerofrmOrdenCompra", "colProveedorfrmOrdenCompra", "colFechafrmOrdenCompra" })
                dgvObservadasfrmOrdenCompra.Columns.Add(col, g.Traducir(col));
            AjusteGrilla.Configurar(dgvObservadasfrmOrdenCompra);
        }

        // "Nueva orden" (CU016) u "Órdenes observadas" (CU018)
        private void rdbModo_CheckedChanged(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked) return;
            var g = GestorIdioma.Instancia;
            bool ajuste = ModoAjuste;
            lblAvisosfrmOrdenCompra.Visible = dgvAvisosfrmOrdenCompra.Visible = !ajuste;
            dgvObservadasfrmOrdenCompra.Visible = lblObservacionesfrmOrdenCompra.Visible = txtObservacionesfrmOrdenCompra.Visible = ajuste;
            btnConfirmarfrmOrdenCompra.Text = g.Traducir(ajuste ? "msgOrdenAjustarBoton" : "btnConfirmarfrmOrdenCompra");
            ordenEnAjuste = null;
            txtObservacionesfrmOrdenCompra.Text = string.Empty;
            if (ajuste)
            {
                CargarProveedores(null);
                CargarObservadas(true);
            }
            else
            {
                CargarAvisos(false);
                CargarProveedores(null);
            }
        }

        // CU018 paso 2 / FA1: órdenes observadas
        private void CargarObservadas(bool avisarSiVacio)
        {
            var g = GestorIdioma.Instancia;
            cargando = true;
            dgvObservadasfrmOrdenCompra.Rows.Clear();
            var ordenes = new List<BE.ORDEN_COMPRA>();
            try
            {
                ordenes = GestorOrden.ListarPorEstado(ORDEN_COMPRA_BLL.EstadoObservada);
                foreach (var o in ordenes)
                {
                    int fila = dgvObservadasfrmOrdenCompra.Rows.Add(o.IdOrdenCompra, o.Proveedor.RazonSocial, o.FechaGeneracion.ToString("dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture));
                    dgvObservadasfrmOrdenCompra.Rows[fila].Tag = o;
                }
                dgvObservadasfrmOrdenCompra.ClearSelection();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
            finally
            {
                cargando = false;
            }
            if (avisarSiVacio && ordenes.Count == 0)
                MessageBox.Show(g.Traducir("msgOrdenSinObservadas"), g.Traducir("frmOrdenCompra"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // CU018 pasos 3 y 4: detalle de la orden con las observaciones del Dueño, listo para modificar
        private void dgvObservadasfrmOrdenCompra_SelectionChanged(object sender, EventArgs e)
        {
            if (cargando || dgvObservadasfrmOrdenCompra.SelectedRows.Count == 0) return;
            var seleccionada = dgvObservadasfrmOrdenCompra.SelectedRows[0].Tag as BE.ORDEN_COMPRA;
            try
            {
                ordenEnAjuste = GestorOrden.ObtenerDetalle(seleccionada.IdOrdenCompra);
                txtObservacionesfrmOrdenCompra.Text = ordenEnAjuste.Observaciones;
                CargarProveedores(ordenEnAjuste.IdProveedor);
                foreach (DataGridViewRow fila in dgvPedidofrmOrdenCompra.Rows)
                {
                    var linea = ordenEnAjuste.Detalles.FirstOrDefault(d => d.IdInsumo == ((BE.INSUMO)fila.Tag).IdInsumo);
                    fila.Cells["colPedirfrmOrdenCompra"].Value = linea != null;
                    fila.Cells["colCantidadfrmOrdenCompra"].Value = linea != null ? linea.CantidadPedida.ToString() : string.Empty;
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        // CU016 paso 2 / FA1: insumos con aviso pendiente; si no hay, todos los insumos
        private void CargarAvisos(bool avisar)
        {
            var g = GestorIdioma.Instancia;
            dgvAvisosfrmOrdenCompra.Rows.Clear();
            try
            {
                var conAviso = GestorInsumo.ListarConAvisoPendiente().Where(i => i.Activo).ToList();
                idsConAviso = conAviso.Select(i => i.IdInsumo).ToList();
                var lista = conAviso;
                if (conAviso.Count == 0)
                {
                    if (avisar)
                        MessageBox.Show(g.Traducir("msgOrdenSinAvisos"), g.Traducir("frmOrdenCompra"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lista = GestorInsumo.ListarInsumosActivos();
                    lblAvisosfrmOrdenCompra.Text = g.Traducir("msgOrdenTodosLosInsumos");
                }
                else
                {
                    lblAvisosfrmOrdenCompra.Text = g.Traducir("lblAvisosfrmOrdenCompra");
                }
                foreach (var i in lista)
                {
                    int fila = dgvAvisosfrmOrdenCompra.Rows.Add(i.Nombre,
                        i.VolumenPesoDisponible.ToString("#,0.###", CultureInfo.CurrentCulture) + " " + i.UnidadMedida,
                        i.UmbralReposicion.ToString("#,0.###", CultureInfo.CurrentCulture) + " " + i.UnidadMedida);
                    if (i.VolumenPesoDisponible < i.UmbralReposicion)
                        dgvAvisosfrmOrdenCompra.Rows[fila].DefaultCellStyle.ForeColor = Color.Firebrick;
                }
                dgvAvisosfrmOrdenCompra.ClearSelection();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        // CU016 paso 3: proveedores activos
        private void CargarProveedores(int? seleccionar)
        {
            cargando = true;
            try
            {
                cmbProveedorfrmOrdenCompra.DataSource = null;
                cmbProveedorfrmOrdenCompra.DataSource = GestorProveedor.ListarProveedores();
                cmbProveedorfrmOrdenCompra.DisplayMember = "RazonSocial";
                cmbProveedorfrmOrdenCompra.SelectedIndex = -1;
                if (seleccionar.HasValue)
                {
                    var lista = (List<BE.PROVEEDOR>)cmbProveedorfrmOrdenCompra.DataSource;
                    cmbProveedorfrmOrdenCompra.SelectedIndex = lista.FindIndex(p => p.IdProveedor == seleccionar.Value);
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
            finally
            {
                cargando = false;
            }
            CargarInsumosProveedor();
        }

        // CU016 pasos 3 y 4 (mensajes 27-34): insumos que ofrece el proveedor, con unidad de compra y equivalencia
        private void CargarInsumosProveedor()
        {
            dgvPedidofrmOrdenCompra.Rows.Clear();
            var proveedor = cmbProveedorfrmOrdenCompra.SelectedItem as BE.PROVEEDOR;
            if (proveedor == null) return;
            var g = GestorIdioma.Instancia;
            try
            {
                foreach (var i in GestorProveedor.ListarInsumosDeProveedor(proveedor.IdProveedor).Where(x => x.Activo))
                {
                    bool aviso = idsConAviso.Contains(i.IdInsumo);
                    int fila = dgvPedidofrmOrdenCompra.Rows.Add(aviso, i.Nombre, i.UnidadCompra,
                        "1 = " + i.EquivalenciaMagnitud.ToString("#,0.###", CultureInfo.CurrentCulture) + " " + i.UnidadMedida,
                        aviso ? "1" : string.Empty,
                        aviso ? g.Traducir("msgConsultaSi") : string.Empty);
                    dgvPedidofrmOrdenCompra.Rows[fila].Tag = i;
                    if (aviso) dgvPedidofrmOrdenCompra.Rows[fila].DefaultCellStyle.ForeColor = Color.Firebrick;
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void cmbProveedorfrmOrdenCompra_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cargando) CargarInsumosProveedor();
        }

        // CU016 FA2: «extend» a CU022 Insertar Proveedor
        private void btnNuevoProveedorfrmOrdenCompra_Click(object sender, EventArgs e)
        {
            var frm = new frmProveedor();
            GUIManager.Instancia.AbrirForm(frm);
            CargarProveedores(frm.ProveedorAgregado?.IdProveedor);
        }

        // Orden con las líneas marcadas. Devuelve null si alguna cantidad no es un número entero (FA3).
        private BE.ORDEN_COMPRA LeerOrden()
        {
            dgvPedidofrmOrdenCompra.EndEdit();
            var orden = new BE.ORDEN_COMPRA { IdProveedor = (cmbProveedorfrmOrdenCompra.SelectedItem as BE.PROVEEDOR)?.IdProveedor ?? 0 };
            foreach (DataGridViewRow fila in dgvPedidofrmOrdenCompra.Rows)
            {
                if (!(fila.Cells["colPedirfrmOrdenCompra"].Value is bool pedir) || !pedir) continue;
                if (!int.TryParse(fila.Cells["colCantidadfrmOrdenCompra"].Value?.ToString(), out int cantidad))
                    return null;
                var insumo = fila.Tag as BE.INSUMO;
                orden.Detalles.Add(new BE.ORDEN_COMPRA_DETALLE { IdInsumo = insumo.IdInsumo, CantidadPedida = cantidad, Insumo = insumo });
            }
            return orden;
        }

        // CU016 pasos 5 y 6 (FA3: orden inválida)
        private void btnConfirmarfrmOrdenCompra_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            var orden = LeerOrden();
            if (orden == null)
            {
                Avisar("msgOrdenCantidadInvalida");
                return;
            }
            try
            {
                if (ModoAjuste)
                {
                    // CU018 pasos 5 y 6 (FA2: orden inválida)
                    if (ordenEnAjuste == null)
                    {
                        Avisar("msgOrdenSeleccionarObservada");
                        return;
                    }
                    orden.IdOrdenCompra = ordenEnAjuste.IdOrdenCompra;
                    GestorOrden.ValidarOrden(orden);
                    GestorOrden.AjustarOrden(orden);
                    MessageBox.Show(g.Traducir("msgOrdenAjustada"), g.Traducir("frmOrdenCompra"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ordenEnAjuste = null;
                    txtObservacionesfrmOrdenCompra.Text = string.Empty;
                    CargarProveedores(null);
                    CargarObservadas(false);
                    return;
                }
                GestorOrden.ValidarOrden(orden);
                GestorOrden.GenerarOrden(orden);
                MessageBox.Show(string.Format(g.Traducir("msgOrdenGenerada"), orden.IdOrdenCompra), g.Traducir("frmOrdenCompra"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarAvisos(false);
                CargarProveedores(null);
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

        private void btnLimpiarfrmOrdenCompra_Click(object sender, EventArgs e)
        {
            CargarProveedores(null);
        }

        private void Avisar(string clave)
        {
            var g = GestorIdioma.Instancia;
            MessageBox.Show(g.Traducir(clave), g.Traducir("msgOrdenAviso"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void MostrarError(Exception ex)
        {
            var g = GestorIdioma.Instancia;
            MessageBox.Show(g.Traducir("msgOrdenError") + ex.GetBaseException().Message, g.Traducir("frmOrdenCompra"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ActualizarIdioma()
        {
            var g = GestorIdioma.Instancia;
            this.Text = g.Traducir(this.Name);
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is DataGridView || ctrl is ComboBox || ctrl is TextBox || ctrl == lblAvisosfrmOrdenCompra)
                    continue;
                ctrl.Text = g.Traducir(ctrl.Name);
            }
            foreach (DataGridViewColumn col in dgvAvisosfrmOrdenCompra.Columns)
                col.HeaderText = g.Traducir(col.Name);
            foreach (DataGridViewColumn col in dgvPedidofrmOrdenCompra.Columns)
                col.HeaderText = g.Traducir(col.Name == "colInsumoPedidofrmOrdenCompra" ? "colInsumofrmOrdenCompra" : col.Name);
            lblAvisosfrmOrdenCompra.Text = g.Traducir(idsConAviso.Count > 0 ? "lblAvisosfrmOrdenCompra" : "msgOrdenTodosLosInsumos");
            foreach (DataGridViewColumn col in dgvObservadasfrmOrdenCompra.Columns)
                col.HeaderText = g.Traducir(col.Name);
            if (ModoAjuste) btnConfirmarfrmOrdenCompra.Text = g.Traducir("msgOrdenAjustarBoton");
        }
    }
}
