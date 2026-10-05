using BE;
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
    // ABM de proveedores: CU022 Insertar, CU026 Modificar y CU027 Dar de Baja Proveedor. Actor: Encargado de Stock.
    public partial class frmProveedor : Form, IObservadorIdioma
    {
        PROVEEDOR_BLL GestorProveedor = new PROVEEDOR_BLL();
        INSUMO_BLL GestorInsumo = new INSUMO_BLL();
        // Evita que la carga de la grilla dispare el modo edición
        bool cargando = false;

        // CU016 FA2 («extend» a CU022): el proveedor que se acaba de dar de alta
        public BE.PROVEEDOR ProveedorAgregado { get; private set; }

        public frmProveedor()
        {
            InitializeComponent();
            // Las columnas se crean acá: GUIManager traduce el formulario antes del Load
            Enlazar();
        }

        private void frmProveedor_Load(object sender, EventArgs e)
        {
            CargarInsumos();
            CargarProveedores();
            ModoAlta();
        }

        private void Enlazar()
        {
            var g = GestorIdioma.Instancia;
            dgvProveedoresfrmProveedor.Columns.Clear();
            foreach (string col in new[] { "colRazonSocialfrmProveedor", "colCuitfrmProveedor", "colTelefonofrmProveedor", "colCorreofrmProveedor" })
                dgvProveedoresfrmProveedor.Columns.Add(col, g.Traducir(col));
            // Decisión 64: casilla Activo (desmarcar = CU027, marcar = reactivar)
            ColumnaActivo.Agregar(dgvProveedoresfrmProveedor, "colActivofrmProveedor", g.Traducir("colActivofrmProveedor"), CambiarActivo);
            AjusteGrilla.Configurar(dgvProveedoresfrmProveedor);
        }

        // CU022 paso 2: insumos activos para asociar
        private void CargarInsumos()
        {
            try
            {
                clbInsumosfrmProveedor.Items.Clear();
                foreach (var i in GestorInsumo.ListarInsumosActivos())
                    clbInsumosfrmProveedor.Items.Add(i);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void CargarProveedores()
        {
            cargando = true;
            dgvProveedoresfrmProveedor.Rows.Clear();
            try
            {
                // Decisión 64: activos y dados de baja (en gris), para poder reactivarlos
                foreach (var p in GestorProveedor.ListarProveedoresConBajas().OrderByDescending(x => x.Activo).ThenBy(x => x.RazonSocial))
                {
                    int fila = dgvProveedoresfrmProveedor.Rows.Add(p.RazonSocial, p.CUIT, p.Telefono, p.Correo, p.Activo);
                    dgvProveedoresfrmProveedor.Rows[fila].Tag = p;
                    ColumnaActivo.Pintar(dgvProveedoresfrmProveedor.Rows[fila], p.Activo);
                }
                dgvProveedoresfrmProveedor.ClearSelection();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
            finally
            {
                cargando = false;
            }
        }

        // CU022 pasos 1 y 2: formulario vacío
        private void ModoAlta()
        {
            cargando = true;
            dgvProveedoresfrmProveedor.ClearSelection();
            cargando = false;
            txtRazonSocialfrmProveedor.Text = txtCuitfrmProveedor.Text = txtTelefonofrmProveedor.Text = txtCorreofrmProveedor.Text = string.Empty;
            for (int i = 0; i < clbInsumosfrmProveedor.Items.Count; i++)
                clbInsumosfrmProveedor.SetItemChecked(i, false);
            btnAltafrmProveedor.Enabled = true;
            btnModificacionfrmProveedor.Enabled = btnBajafrmProveedor.Enabled = false;
        }

        private BE.PROVEEDOR ProveedorSeleccionado()
        {
            if (dgvProveedoresfrmProveedor.SelectedRows.Count == 0) return null;
            return dgvProveedoresfrmProveedor.SelectedRows[0].Tag as BE.PROVEEDOR;
        }

        // CU026 pasos 1 y 2: datos actuales e insumos asociados
        private void dgvProveedoresfrmProveedor_SelectionChanged(object sender, EventArgs e)
        {
            if (cargando) return;
            var p = ProveedorSeleccionado();
            if (p == null) return;
            try
            {
                txtRazonSocialfrmProveedor.Text = p.RazonSocial;
                txtCuitfrmProveedor.Text = p.CUIT;
                txtTelefonofrmProveedor.Text = p.Telefono;
                txtCorreofrmProveedor.Text = p.Correo;
                var asociados = GestorProveedor.ListarInsumosDeProveedor(p.IdProveedor).Select(i => i.IdInsumo).ToList();
                for (int i = 0; i < clbInsumosfrmProveedor.Items.Count; i++)
                    clbInsumosfrmProveedor.SetItemChecked(i, asociados.Contains(((BE.INSUMO)clbInsumosfrmProveedor.Items[i]).IdInsumo));
                btnAltafrmProveedor.Enabled = false;
                // Un proveedor dado de baja solo se reactiva (casilla Activo)
                btnModificacionfrmProveedor.Enabled = btnBajafrmProveedor.Enabled = p.Activo;
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private BE.PROVEEDOR LeerCampos()
        {
            return new BE.PROVEEDOR
            {
                RazonSocial = txtRazonSocialfrmProveedor.Text,
                CUIT = txtCuitfrmProveedor.Text,
                Telefono = txtTelefonofrmProveedor.Text,
                Correo = txtCorreofrmProveedor.Text,
                Insumos = clbInsumosfrmProveedor.CheckedItems.Cast<BE.INSUMO>().ToList()
            };
        }

        private void btnNuevofrmProveedor_Click(object sender, EventArgs e)
        {
            ModoAlta();
            txtRazonSocialfrmProveedor.Focus();
        }

        // CU022 pasos 3 y 4 (FA1 y FA2)
        private void btnAltafrmProveedor_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            var proveedor = LeerCampos();
            try
            {
                GestorProveedor.InsertarProveedor(proveedor);
                ProveedorAgregado = proveedor;
                MessageBox.Show(g.Traducir("msgProveedorAlta"), g.Traducir("frmProveedor"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarProveedores();
                ModoAlta();
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

        // CU026 pasos 3 y 4 (FA1, FA2 y FA3)
        private void btnModificacionfrmProveedor_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            var seleccionado = ProveedorSeleccionado();
            if (seleccionado == null)
            {
                Avisar("msgProveedorSeleccionar");
                return;
            }
            if (!seleccionado.Activo)
            {
                Avisar("msgProveedorReactivarPrimero");
                return;
            }
            var proveedor = LeerCampos();
            proveedor.IdProveedor = seleccionado.IdProveedor;
            try
            {
                // FA3: se quitó un insumo que figura en una orden abierta de este proveedor
                var ordenes = GestorProveedor.VerificarInsumosQuitados(proveedor);
                if (ordenes.Count > 0)
                {
                    MessageBox.Show(string.Format(g.Traducir("msgProveedorInsumoEnOrden"), DescribirOrdenes(ordenes)), g.Traducir("msgProveedorAviso"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                GestorProveedor.ModificarProveedor(proveedor);
                MessageBox.Show(g.Traducir("msgProveedorModificado"), g.Traducir("frmProveedor"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarProveedores();
                ModoAlta();
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

        // CU027 Dar de Baja Proveedor (FA1 órdenes abiertas, FA2 cancelación)
        private void btnBajafrmProveedor_Click(object sender, EventArgs e)
        {
            var seleccionado = ProveedorSeleccionado();
            if (seleccionado == null)
            {
                Avisar("msgProveedorSeleccionar");
                return;
            }
            DarDeBaja(seleccionado);
        }

        // Decisión 64: la casilla Activo de la grilla da de baja o reactiva
        private void CambiarActivo(DataGridViewRow fila, bool activar)
        {
            var proveedor = fila.Tag as BE.PROVEEDOR;
            if (proveedor == null) return;
            if (activar)
                Reactivar(proveedor);
            else
                DarDeBaja(proveedor);
        }

        private void Reactivar(BE.PROVEEDOR proveedor)
        {
            var g = GestorIdioma.Instancia;
            if (MessageBox.Show(string.Format(g.Traducir("msgProveedorConfirmarReactivar"), proveedor.RazonSocial, proveedor.CUIT), g.Traducir("msgProveedorConfirmarTitulo"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {
                GestorProveedor.ReactivarProveedor(proveedor.IdProveedor);
                MessageBox.Show(g.Traducir("msgProveedorReactivado"), g.Traducir("frmProveedor"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarProveedores();
                ModoAlta();
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

        private void DarDeBaja(BE.PROVEEDOR seleccionado)
        {
            var g = GestorIdioma.Instancia;
            try
            {
                var ordenes = GestorProveedor.VerificarBajaProveedor(seleccionado.IdProveedor);
                if (ordenes.Count > 0)
                {
                    MessageBox.Show(string.Format(g.Traducir("msgProveedorConOrdenes"), DescribirOrdenes(ordenes)), g.Traducir("msgProveedorAviso"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var confirmar = MessageBox.Show(string.Format(g.Traducir("msgProveedorConfirmarBaja"), seleccionado.RazonSocial, seleccionado.CUIT),
                    g.Traducir("msgProveedorConfirmarTitulo"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmar != DialogResult.Yes)
                {
                    // FA2: vuelve al listado
                    ModoAlta();
                    return;
                }
                GestorProveedor.DarDeBajaProveedor(seleccionado.IdProveedor);
                MessageBox.Show(g.Traducir("msgProveedorBaja"), g.Traducir("frmProveedor"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarProveedores();
                ModoAlta();
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

        private string DescribirOrdenes(List<BE.ORDEN_COMPRA> ordenes)
        {
            return string.Join(", ", ordenes.Select(o => $"N° {o.IdOrdenCompra} ({o.Estado})"));
        }

        private void Avisar(string clave)
        {
            var g = GestorIdioma.Instancia;
            MessageBox.Show(g.Traducir(clave).Replace("{0}", string.Empty).Replace("{1}", string.Empty), g.Traducir("msgProveedorAviso"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void MostrarError(Exception ex)
        {
            var g = GestorIdioma.Instancia;
            MessageBox.Show(g.Traducir("msgProveedorError") + ex.GetBaseException().Message, g.Traducir("frmProveedor"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ActualizarIdioma()
        {
            var g = GestorIdioma.Instancia;
            this.Text = g.Traducir(this.Name);
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is TextBox || ctrl is DataGridView || ctrl is CheckedListBox)
                    continue;
                ctrl.Text = g.Traducir(ctrl.Name);
            }
            foreach (DataGridViewColumn col in dgvProveedoresfrmProveedor.Columns)
                col.HeaderText = g.Traducir(col.Name);
        }
    }
}
