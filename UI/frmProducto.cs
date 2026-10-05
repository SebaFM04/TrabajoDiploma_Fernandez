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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    // CU006 Insertar Productos, CU007 Dar de baja producto y CU008 Modificar producto (con tamaños y precios, decisiones 50 y 51).
    public partial class frmProducto : Form , IObservadorIdioma
    {
        PRODUCTO_BLL GestorProducto = new PRODUCTO_BLL();
        // Evita que la carga de la grilla dispare la selección
        bool cargando = false;

        public frmProducto()
        {
            InitializeComponent();
            // Las columnas se crean acá: GUIManager traduce el formulario antes del Load
            Enlazar();
        }

        private void frmProducto_Load(object sender, EventArgs e)
        {
            CargarProductos();
            ModoAlta();
        }

        private void Enlazar()
        {
            var g = GestorIdioma.Instancia;
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("colIdfrmProducto", g.Traducir("colIdfrmProducto"));
            dataGridView1.Columns.Add("colNombrefrmProducto", g.Traducir("colNombrefrmProducto"));
            dataGridView1.Columns.Add("colTipofrmProducto", g.Traducir("colTipofrmProducto"));
            dataGridView1.Columns["colIdfrmProducto"].Visible = false;
            // Decisión 64: casilla Activo (desmarcar = CU007, marcar = reactivar)
            ColumnaActivo.Agregar(dataGridView1, "colActivofrmProducto", g.Traducir("colActivofrmProducto"), CambiarActivo);
            AjusteGrilla.Configurar(dataGridView1);
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

            // CU006 paso 3: se marcan los tamaños en que se vende y se carga el precio de cada uno
            dgvTamaniosfrmProducto.Columns.Clear();
            dgvTamaniosfrmProducto.Columns.Add(new DataGridViewCheckBoxColumn { Name = "colVendefrmProducto", HeaderText = g.Traducir("colVendefrmProducto"), FillWeight = 25 });
            dgvTamaniosfrmProducto.Columns.Add("colTamaniofrmProducto", g.Traducir("colTamaniofrmProducto"));
            dgvTamaniosfrmProducto.Columns.Add("colPreciofrmProducto", g.Traducir("colPreciofrmProducto"));
            dgvTamaniosfrmProducto.Columns["colTamaniofrmProducto"].ReadOnly = true;
            AjusteGrilla.Configurar(dgvTamaniosfrmProducto);

            cmbTipofrmProducto.Items.Clear();
            cmbTipofrmProducto.Items.AddRange(GestorProducto.ListarTiposProducto());
            cmbTipofrmProducto.SelectedIndex = -1;
        }

        // CU006 pasos 1 y 2: formulario vacío
        private void ModoAlta()
        {
            cargando = true;
            dataGridView1.ClearSelection();
            cargando = false;
            textBox1.Text = string.Empty;
            cmbTipofrmProducto.SelectedIndex = -1;
            dgvTamaniosfrmProducto.Rows.Clear();
            btnAltafrmProducto.Enabled = true;
            btnModificacionfrmProducto.Enabled = false;
            btnBajafrmProducto.Enabled = false;
        }

        private void CargarProductos()
        {
            cargando = true;
            dataGridView1.Rows.Clear();
            try
            {
                // Decisión 64: activos y dados de baja (en gris), para poder reactivarlos
                foreach (var p in GestorProducto.ListarProductos().OrderByDescending(x => x.Activo).ThenBy(x => x.Nombre))
                {
                    int fila = dataGridView1.Rows.Add(p.IdProducto, p.Nombre, p.Tipo, p.Activo);
                    dataGridView1.Rows[fila].Tag = p;
                    ColumnaActivo.Pintar(dataGridView1.Rows[fila], p.Activo);
                }
                dataGridView1.ClearSelection();
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

        // CU006 paso 2 / FA3: tamaños que corresponden al tipo, conservando lo ya cargado
        private void CargarTamanios(string tipo, List<BE.PRODUCTO_TAMANIO> marcados)
        {
            dgvTamaniosfrmProducto.Rows.Clear();
            if (string.IsNullOrEmpty(tipo)) return;
            try
            {
                foreach (var t in GestorProducto.ListarTamaniosPorTipo(tipo))
                {
                    var marcado = marcados?.FirstOrDefault(m => m.IdTamanio == t.IdTamanio);
                    string texto = $"{t.Nombre} ({t.CantidadMagnitud.ToString("0.###", CultureInfo.CurrentCulture)} {t.UnidadMagnitud})";
                    int fila = dgvTamaniosfrmProducto.Rows.Add(marcado != null, texto,
                        marcado != null ? FormatoMoneda.Numero(marcado.Precio) : string.Empty);
                    dgvTamaniosfrmProducto.Rows[fila].Tag = t;
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void cmbTipofrmProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarTamanios(cmbTipofrmProducto.SelectedItem?.ToString(), LeerTamanios(out _));
        }

        // Tamaños marcados con su precio. invalido = true si algún precio no es un número (FA1).
        private List<BE.PRODUCTO_TAMANIO> LeerTamanios(out bool invalido)
        {
            invalido = false;
            dgvTamaniosfrmProducto.EndEdit();
            var lista = new List<BE.PRODUCTO_TAMANIO>();
            foreach (DataGridViewRow fila in dgvTamaniosfrmProducto.Rows)
            {
                if (!(fila.Cells["colVendefrmProducto"].Value is bool vende) || !vende) continue;
                var tamanio = fila.Tag as BE.TAMANIO;
                string texto = fila.Cells["colPreciofrmProducto"].Value?.ToString();
                if (!FormatoMoneda.TryLeer(texto, out decimal precio))
                {
                    invalido = true;
                    precio = 0;
                }
                lista.Add(new BE.PRODUCTO_TAMANIO { IdTamanio = tamanio.IdTamanio, Precio = precio, Tamanio = tamanio });
            }
            return lista;
        }

        // Arma el producto con los datos del formulario. Devuelve null si un precio no es un número (FA1).
        private BE.PRODUCTO LeerProducto()
        {
            var tamanios = LeerTamanios(out bool invalido);
            if (invalido)
            {
                Avisar("msgProductoPrecioInvalido");
                return null;
            }
            return new BE.PRODUCTO
            {
                Nombre = textBox1.Text,
                Tipo = cmbTipofrmProducto.SelectedItem?.ToString(),
                Tamanios = tamanios
            };
        }

        private BE.PRODUCTO ProductoSeleccionado()
        {
            if (dataGridView1.SelectedRows.Count == 0) return null;
            return dataGridView1.SelectedRows[0].Tag as BE.PRODUCTO;
        }

        private void btnNuevofrmProducto_Click(object sender, EventArgs e)
        {
            ModoAlta();
            textBox1.Focus();
        }

        // CU006 pasos 3 a 5. FA1 y FA3 los valida PRODUCTO_BLL; FA2 (nombre duplicado) el SP.
        private void btnAltafrmProducto_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            var producto = LeerProducto();
            if (producto == null) return;
            try
            {
                GestorProducto.InsertarProducto(producto);
                MessageBox.Show(g.Traducir("msgProductoRegistrado"), g.Traducir("frmProducto"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarProductos();
                ModoAlta();
                textBox1.Focus();
            }
            catch (ArgumentException argEx)
            {
                // FA1 / FA2: continúa en el paso 3 con lo cargado
                Avisar(argEx.Message);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        // CU007 Dar de baja producto (baja lógica)
        private void btnBajafrmProducto_Click(object sender, EventArgs e)
        {
            var seleccionado = ProductoSeleccionado();
            if (seleccionado == null)
            {
                Avisar("msgProductoSeleccionar");
                return;
            }
            DarDeBaja(seleccionado);
        }

        // Decisión 64: la casilla Activo de la grilla da de baja o reactiva
        private void CambiarActivo(DataGridViewRow fila, bool activar)
        {
            var producto = fila.Tag as BE.PRODUCTO;
            if (producto == null) return;
            if (activar)
                Reactivar(producto);
            else
                DarDeBaja(producto);
        }

        // Decisión 64: no se reactiva si la receta usa insumos dados de baja
        private void Reactivar(BE.PRODUCTO producto)
        {
            var g = GestorIdioma.Instancia;
            try
            {
                var inactivos = GestorProducto.VerificarReactivacionProducto(producto.IdProducto);
                if (inactivos.Count > 0)
                {
                    MessageBox.Show(string.Format(g.Traducir("msgProductoReactivarInsumosInactivos"), string.Join(", ", inactivos.Select(i => i.Nombre))),
                        g.Traducir("msgProductoAviso"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MessageBox.Show(string.Format(g.Traducir("msgProductoConfirmarReactivar"), producto.Nombre), g.Traducir("msgProductoConfirmarTitulo"),
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
                GestorProducto.ReactivarProducto(producto);
                MessageBox.Show(g.Traducir("msgProductoReactivado"), g.Traducir("frmProducto"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarProductos();
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

        private void DarDeBaja(BE.PRODUCTO seleccionado)
        {
            var g = GestorIdioma.Instancia;
            var confirm = MessageBox.Show(string.Format(g.Traducir("msgProductoConfirmarBaja"), seleccionado.Nombre),
                g.Traducir("msgProductoConfirmarTitulo"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                int filas = GestorProducto.EliminarProducto(new BE.PRODUCTO { IdProducto = seleccionado.IdProducto, Nombre = seleccionado.Nombre });
                if (filas > 0)
                {
                    MessageBox.Show(g.Traducir("msgProductoBaja"), g.Traducir("frmProducto"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarProductos();
                    ModoAlta();
                }
                else
                {
                    Avisar("msgProductoBajaFallida");
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        // CU008 Modificar producto: nombre, tipo, tamaños y precios
        private void btnModificacionfrmProducto_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            var seleccionado = ProductoSeleccionado();
            if (seleccionado == null)
            {
                Avisar("msgProductoSeleccionar");
                return;
            }
            if (!seleccionado.Activo)
            {
                Avisar("msgProductoReactivarPrimero");
                return;
            }
            var producto = LeerProducto();
            if (producto == null) return;
            producto.IdProducto = seleccionado.IdProducto;

            var confirm = MessageBox.Show(g.Traducir("msgProductoConfirmarModificacion"), g.Traducir("msgProductoConfirmarTitulo"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                GestorProducto.ModificarProducto(producto);
                MessageBox.Show(g.Traducir("msgProductoModificado"), g.Traducir("frmProducto"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarProductos();
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

        // CU008: al seleccionar un producto se cargan sus datos, tamaños y precios
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (cargando) return;
            var seleccionado = ProductoSeleccionado();
            if (seleccionado == null) return;
            try
            {
                var precios = GestorProducto.ObtenerTamaniosProducto(seleccionado.IdProducto);
                textBox1.Text = seleccionado.Nombre;
                // Se desengancha el evento para no recargar los tamaños con la grilla vacía
                cmbTipofrmProducto.SelectedIndexChanged -= cmbTipofrmProducto_SelectedIndexChanged;
                cmbTipofrmProducto.SelectedItem = seleccionado.Tipo;
                cmbTipofrmProducto.SelectedIndexChanged += cmbTipofrmProducto_SelectedIndexChanged;
                CargarTamanios(seleccionado.Tipo, precios);
                btnAltafrmProducto.Enabled = false;
                // Un producto dado de baja solo se reactiva (casilla Activo)
                btnModificacionfrmProducto.Enabled = seleccionado.Activo;
                btnBajafrmProducto.Enabled = seleccionado.Activo;
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void Avisar(string clave)
        {
            var g = GestorIdioma.Instancia;
            MessageBox.Show(g.Traducir(clave), g.Traducir("msgProductoAviso"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void MostrarError(Exception ex)
        {
            var g = GestorIdioma.Instancia;
            MessageBox.Show(g.Traducir("msgProductoError") + ex.GetBaseException().Message, g.Traducir("frmProducto"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ActualizarIdioma()
        {
            var g = GestorIdioma.Instancia;
            this.Text = g.Traducir(this.Name);
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is TextBox || ctrl is DataGridView || ctrl is ComboBox)
                    continue;
                ctrl.Text = g.Traducir(ctrl.Name);
            }
            foreach (DataGridViewColumn col in dataGridView1.Columns)
                col.HeaderText = g.Traducir(col.Name);
            foreach (DataGridViewColumn col in dgvTamaniosfrmProducto.Columns)
                col.HeaderText = g.Traducir(col.Name);
        }
    }
}
