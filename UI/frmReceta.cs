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
    // Gestión de recetas (decisión 14: un solo formulario). CU013 Insertar Recetas y CU014 Modificar Recetas.
    public partial class frmReceta : Form, IObservadorIdioma
    {
        RECETA_BLL GestorReceta = new RECETA_BLL();
        INSUMO_BLL GestorInsumo = new INSUMO_BLL();
        BE.PRODUCTO productoSeleccionado = null;
        // Evita que la carga de la grilla de productos dispare la selección
        bool cargando = false;

        public frmReceta()
        {
            InitializeComponent();
            // Las columnas se crean acá: GUIManager traduce el formulario antes del Load
            Enlazar();
        }

        private void frmReceta_Load(object sender, EventArgs e)
        {
            CargarInsumos();
            CargarProductos();
        }

        private void Enlazar()
        {
            var g = GestorIdioma.Instancia;
            dgvProductosfrmReceta.Columns.Clear();
            dgvProductosfrmReceta.Columns.Add("colProductofrmReceta", g.Traducir("colProductofrmReceta"));
            dgvProductosfrmReceta.Columns.Add("colTipofrmReceta", g.Traducir("colTipofrmReceta"));
            AjusteGrilla.Configurar(dgvProductosfrmReceta);

            dgvRecetafrmReceta.Columns.Clear();
            dgvRecetafrmReceta.Columns.Add("colInsumofrmReceta", g.Traducir("colInsumofrmReceta"));
            dgvRecetafrmReceta.Columns.Add("colUnidadfrmReceta", g.Traducir("colUnidadfrmReceta"));
            dgvRecetafrmReceta.Columns.Add("colProporcionfrmReceta", g.Traducir("colProporcionfrmReceta"));
            dgvRecetafrmReceta.Columns["colInsumofrmReceta"].ReadOnly = true;
            dgvRecetafrmReceta.Columns["colUnidadfrmReceta"].ReadOnly = true;
            // La proporción se puede corregir en la grilla
            dgvRecetafrmReceta.Columns["colProporcionfrmReceta"].ReadOnly = false;
            AjusteGrilla.Configurar(dgvRecetafrmReceta);
        }

        // Insumos activos para armar la receta (decisión 7), filtrados por el uso que corresponde
        // al tipo del producto (decisión 58): bebida → insumos de bebidas, piqueo → insumos de comidas
        private void CargarInsumos()
        {
            try
            {
                string uso = productoSeleccionado?.Tipo == "Bebida" ? INSUMO_BLL.UsoBebidas
                           : productoSeleccionado?.Tipo == "Piqueo" ? INSUMO_BLL.UsoComidas
                           : INSUMO_BLL.UsoTodos;
                cmbInsumofrmReceta.DataSource = null;
                cmbInsumofrmReceta.DataSource = GestorInsumo.ListarInsumosPorUso(uso);
                cmbInsumofrmReceta.DisplayMember = "Nombre";
                cmbInsumofrmReceta.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        // Con "Productos con receta" el formulario modifica (CU014); si no, inserta (CU013)
        private bool ModoModificar
        {
            get { return rdbConRecetafrmReceta.Checked; }
        }

        // CU013 paso 1: productos sin receta (FA1 si no hay ninguno).
        // CU014 paso 1: productos con receta (se informa si no hay ninguno).
        private void CargarProductos()
        {
            var g = GestorIdioma.Instancia;
            cargando = true;
            dgvProductosfrmReceta.Rows.Clear();
            List<BE.PRODUCTO> productos = new List<BE.PRODUCTO>();
            try
            {
                productos = ModoModificar ? GestorReceta.ListarProductosConReceta() : GestorReceta.ListarProductosSinReceta();
                foreach (var p in productos)
                {
                    int fila = dgvProductosfrmReceta.Rows.Add(p.Nombre, p.Tipo);
                    dgvProductosfrmReceta.Rows[fila].Tag = p;
                }
                dgvProductosfrmReceta.ClearSelection();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
            finally
            {
                cargando = false;
            }
            LimpiarReceta();

            if (productos.Count == 0)
            {
                // CU013 FA1: "No hay productos sin receta". CU014: "No hay productos con receta"
                string clave = ModoModificar ? "msgRecetaSinProductosConReceta" : "msgRecetaSinProductos";
                MessageBox.Show(g.Traducir(clave), g.Traducir("frmReceta"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Sin producto seleccionado no se puede armar receta
        private void LimpiarReceta()
        {
            productoSeleccionado = null;
            dgvRecetafrmReceta.Rows.Clear();
            nudProporcionfrmReceta.Value = 0;
            cmbInsumofrmReceta.SelectedIndex = -1;
            HabilitarEdicion(false);
            MostrarTituloReceta();
        }

        private void HabilitarEdicion(bool habilitar)
        {
            cmbInsumofrmReceta.Enabled = habilitar;
            nudProporcionfrmReceta.Enabled = habilitar;
            btnAgregarInsumofrmReceta.Enabled = habilitar;
            btnQuitarInsumofrmReceta.Enabled = habilitar;
            btnNuevoInsumofrmReceta.Enabled = habilitar;
            dgvRecetafrmReceta.Enabled = habilitar;
            btnConfirmarfrmReceta.Enabled = habilitar;
            btnCancelarfrmReceta.Enabled = habilitar;
        }

        private void MostrarTituloReceta()
        {
            string titulo = GestorIdioma.Instancia.Traducir("lblRecetafrmReceta");
            lblRecetafrmReceta.Text = productoSeleccionado == null ? titulo : $"{titulo}: {productoSeleccionado.Nombre}";
        }

        private void rdbModo_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked) CargarProductos();
        }

        // CU013 paso 2: producto seleccionado, se muestran los insumos disponibles.
        // CU014 paso 2: además se muestra la receta actual.
        private void dgvProductosfrmReceta_SelectionChanged(object sender, EventArgs e)
        {
            if (cargando || dgvProductosfrmReceta.SelectedRows.Count == 0) return;
            productoSeleccionado = dgvProductosfrmReceta.SelectedRows[0].Tag as BE.PRODUCTO;
            CargarInsumos();
            dgvRecetafrmReceta.Rows.Clear();
            HabilitarEdicion(productoSeleccionado != null);
            MostrarTituloReceta();
            if (ModoModificar && productoSeleccionado != null) CargarRecetaActual();
        }

        private void CargarRecetaActual()
        {
            try
            {
                foreach (var linea in GestorReceta.ObtenerReceta(productoSeleccionado.IdProducto))
                    AgregarLinea(linea.Insumo, linea.Proporcion);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        // CU013 paso 3: agregar un insumo con su proporción (se valida al confirmar, FA2)
        private void btnAgregarInsumofrmReceta_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            var insumo = cmbInsumofrmReceta.SelectedItem as BE.INSUMO;
            if (insumo == null)
            {
                MessageBox.Show(g.Traducir("msgRecetaSeleccionarInsumo"), g.Traducir("msgRecetaAviso"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            AgregarLinea(insumo, nudProporcionfrmReceta.Value);
            cmbInsumofrmReceta.SelectedIndex = -1;
            nudProporcionfrmReceta.Value = 0;
        }

        private void AgregarLinea(BE.INSUMO insumo, decimal proporcion)
        {
            int fila = dgvRecetafrmReceta.Rows.Add(NombreLinea(insumo), insumo.UnidadMedida, proporcion.ToString("0.###", CultureInfo.CurrentCulture));
            dgvRecetafrmReceta.Rows[fila].Tag = insumo;
            // CU014: los insumos dados de baja se marcan para que el usuario los quite
            if (!insumo.Activo)
                dgvRecetafrmReceta.Rows[fila].DefaultCellStyle.ForeColor = Color.Firebrick;
        }

        private string NombreLinea(BE.INSUMO insumo)
        {
            return insumo.Activo ? insumo.Nombre : insumo.Nombre + GestorIdioma.Instancia.Traducir("msgRecetaInsumoDadoDeBaja");
        }

        private void btnQuitarInsumofrmReceta_Click(object sender, EventArgs e)
        {
            if (dgvRecetafrmReceta.SelectedRows.Count == 0) return;
            dgvRecetafrmReceta.Rows.Remove(dgvRecetafrmReceta.SelectedRows[0]);
        }

        // Punto de extensión CU023 - Insertar Insumo: el insumo que necesita la receta no existe
        private void btnNuevoInsumofrmReceta_Click(object sender, EventArgs e)
        {
            GUIManager.Instancia.AbrirForm(new frmInsumo());
            CargarInsumos();
        }

        // Arma la receta desde la grilla. Devuelve null si alguna proporción no es un número (FA2).
        private List<BE.RECETA> LeerReceta()
        {
            dgvRecetafrmReceta.EndEdit();
            var receta = new List<BE.RECETA>();
            foreach (DataGridViewRow fila in dgvRecetafrmReceta.Rows)
            {
                var insumo = fila.Tag as BE.INSUMO;
                string texto = fila.Cells["colProporcionfrmReceta"].Value?.ToString();
                if (!decimal.TryParse(texto, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal proporcion))
                    return null;
                receta.Add(new BE.RECETA
                {
                    IdProducto = productoSeleccionado.IdProducto,
                    IdInsumo = insumo.IdInsumo,
                    Proporcion = proporcion,
                    Insumo = insumo
                });
            }
            return receta;
        }

        // CU013 y CU014, pasos 4 y 5
        private void btnConfirmarfrmReceta_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            if (productoSeleccionado == null)
            {
                MessageBox.Show(g.Traducir("msgRecetaSeleccionarProducto"), g.Traducir("msgRecetaAviso"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var receta = LeerReceta();
            if (receta == null)
            {
                MessageBox.Show(g.Traducir("msgRecetaProporcionInvalida"), g.Traducir("msgRecetaAviso"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (ModoModificar)
                {
                    GestorReceta.ModificarReceta(productoSeleccionado.IdProducto, receta);
                    MessageBox.Show(g.Traducir("msgRecetaModificada"), g.Traducir("frmReceta"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    GestorReceta.InsertarReceta(productoSeleccionado.IdProducto, receta);
                    MessageBox.Show(g.Traducir("msgRecetaRegistrada"), g.Traducir("frmReceta"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                CargarProductos();
            }
            catch (ArgumentException argEx)
            {
                // CU013 FA2 / CU014 FA1: receta inválida, continúa en el paso 3 con lo cargado
                MessageBox.Show(g.Traducir(argEx.Message), g.Traducir("msgRecetaAviso"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        // CU013 FA3 / CU014 FA2: cancela y vuelve al listado sin guardar nada
        private void btnCancelarfrmReceta_Click(object sender, EventArgs e)
        {
            cargando = true;
            dgvProductosfrmReceta.ClearSelection();
            cargando = false;
            LimpiarReceta();
        }

        private void MostrarError(Exception ex)
        {
            var g = GestorIdioma.Instancia;
            MessageBox.Show(g.Traducir("msgRecetaError") + ex.GetBaseException().Message, g.Traducir("frmReceta"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ActualizarIdioma()
        {
            var g = GestorIdioma.Instancia;
            this.Text = g.Traducir(this.Name);
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is TextBox || ctrl is DataGridView || ctrl is ComboBox || ctrl is NumericUpDown)
                    continue;
                ctrl.Text = g.Traducir(ctrl.Name);
            }
            foreach (DataGridViewColumn col in dgvProductosfrmReceta.Columns)
                col.HeaderText = g.Traducir(col.Name);
            foreach (DataGridViewColumn col in dgvRecetafrmReceta.Columns)
                col.HeaderText = g.Traducir(col.Name);
            foreach (DataGridViewRow fila in dgvRecetafrmReceta.Rows)
                if (fila.Tag is BE.INSUMO insumo)
                    fila.Cells["colInsumofrmReceta"].Value = NombreLinea(insumo);
            MostrarTituloReceta();
        }
    }
}
