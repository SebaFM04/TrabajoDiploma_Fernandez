using BE;
using BLL;
using SERVICIO;
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
    // ABM de insumos: CU023 Insertar Insumo, CU024 Modificar Insumo, CU025 Dar de Baja Insumo.
    // Modo alta: todos los campos editables. Modo edición (insumo seleccionado): unidad de medida y stock de solo lectura.
    // El stock inicial se ingresa en unidades de compra y se muestra calculado en la unidad de medida (decisión 52).
    public partial class frmInsumo : Form, IObservadorIdioma
    {
        INSUMO_BLL GestorInsumo = new INSUMO_BLL();
        // Evita que la carga de la grilla dispare el modo edición
        bool cargando = false;
        // Stock real del insumo en edición (null en modo alta)
        decimal? stockEdicion = null;
        // Filtro por uso (decisión 58), en el mismo orden que el combo
        readonly string[] usos = { INSUMO_BLL.UsoTodos, INSUMO_BLL.UsoBebidas, INSUMO_BLL.UsoComidas };
        readonly string[] clavesUso = { "msgInsumoUsoTodos", "msgInsumoUsoBebidas", "msgInsumoUsoComidas" };

        public frmInsumo()
        {
            InitializeComponent();
            // Las columnas se crean acá: GUIManager traduce el formulario antes del Load
            Enlazar();
        }

        private void frmInsumo_Load(object sender, EventArgs e)
        {
            cmbUnidadMedidafrmInsumo.Items.Clear();
            cmbUnidadMedidafrmInsumo.Items.AddRange(GestorInsumo.ListarUnidadesMedida());
            CargarFiltroUso();
            CargarInsumos();
            ModoAlta();
        }

        private void Enlazar()
        {
            var g = GestorIdioma.Instancia;
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("colIdfrmInsumo", "Id");
            dataGridView1.Columns["colIdfrmInsumo"].Visible = false;
            foreach (string columna in new[] { "colNombrefrmInsumo", "colUnidadMedidafrmInsumo", "colUnidadComprafrmInsumo",
                                               "colEquivalenciafrmInsumo", "colStockfrmInsumo", "colUmbralfrmInsumo", "colCostofrmInsumo", "colUsofrmInsumo" })
            {
                dataGridView1.Columns.Add(columna, g.Traducir(columna));
            }
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // CU023 paso 1-2: formulario vacío para registrar un insumo nuevo
        private void ModoAlta()
        {
            cargando = true;
            dataGridView1.ClearSelection();
            cargando = false;

            txtNombrefrmInsumo.Text = string.Empty;
            cmbUnidadMedidafrmInsumo.SelectedIndex = -1;
            txtUnidadComprafrmInsumo.Text = string.Empty;
            nudEquivalenciafrmInsumo.Value = 0;
            nudUmbralfrmInsumo.Value = 0;
            nudCostofrmInsumo.Value = 0;
            nudStockInicialfrmInsumo.Value = 0;
            chkUsoBebidasfrmInsumo.Checked = false;
            chkUsoComidasfrmInsumo.Checked = false;

            cmbUnidadMedidafrmInsumo.Enabled = true;
            nudStockInicialfrmInsumo.Enabled = true;
            stockEdicion = null;
            MostrarStockCalculado();
            btnAltafrmInsumo.Enabled = true;
            btnModificacionfrmInsumo.Enabled = false;
            btnBajafrmInsumo.Enabled = false;
        }

        // CU024 paso 2: datos del insumo seleccionado; unidad de medida y stock solo de lectura (decisión 37)
        private void ModoEdicion(BE.INSUMO insumo)
        {
            txtNombrefrmInsumo.Text = insumo.Nombre;
            cmbUnidadMedidafrmInsumo.SelectedItem = insumo.UnidadMedida;
            txtUnidadComprafrmInsumo.Text = insumo.UnidadCompra;
            nudEquivalenciafrmInsumo.Value = insumo.EquivalenciaMagnitud;
            nudUmbralfrmInsumo.Value = insumo.UmbralReposicion;
            nudCostofrmInsumo.Value = insumo.CostoUnidadCompra;
            chkUsoBebidasfrmInsumo.Checked = insumo.UsoBebidas;
            chkUsoComidasfrmInsumo.Checked = insumo.UsoComidas;
            // El stock actual se muestra tal cual; el campo de unidades queda como referencia
            stockEdicion = insumo.VolumenPesoDisponible;
            nudStockInicialfrmInsumo.Value = insumo.EquivalenciaMagnitud > 0
                ? Math.Min(nudStockInicialfrmInsumo.Maximum, Math.Round(insumo.VolumenPesoDisponible / insumo.EquivalenciaMagnitud, 3))
                : 0;

            cmbUnidadMedidafrmInsumo.Enabled = false;
            nudStockInicialfrmInsumo.Enabled = false;
            MostrarStockCalculado();
            btnAltafrmInsumo.Enabled = false;
            btnModificacionfrmInsumo.Enabled = true;
            btnBajafrmInsumo.Enabled = true;
        }

        private void CargarInsumos()
        {
            var g = GestorIdioma.Instancia;
            cargando = true;
            dataGridView1.Rows.Clear();
            try
            {
                foreach (var i in GestorInsumo.ListarInsumosPorUso(usos[Math.Max(0, cmbFiltroUsofrmInsumo.SelectedIndex)]))
                {
                    int fila = dataGridView1.Rows.Add(i.IdInsumo, i.Nombre, i.UnidadMedida, i.UnidadCompra,
                        i.EquivalenciaMagnitud.ToString("0.###"), i.VolumenPesoDisponible.ToString("0.###"),
                        i.UmbralReposicion.ToString("0.###"), FormatoMoneda.Pesos(i.CostoUnidadCompra), TextoUso(i));
                    dataGridView1.Rows[fila].Tag = i;
                }
                dataGridView1.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(g.Traducir("msgInsumoErrorOperacion") + ex.GetBaseException().Message, g.Traducir("frmInsumo"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cargando = false;
            }
        }

        // Decisión 58: Bebidas, Comidas o Ambos
        private string TextoUso(BE.INSUMO i)
        {
            var g = GestorIdioma.Instancia;
            if (i.UsoBebidas && i.UsoComidas) return g.Traducir("msgInsumoUsoAmbos");
            return g.Traducir(i.UsoBebidas ? "msgInsumoUsoBebidas" : "msgInsumoUsoComidas");
        }

        private void CargarFiltroUso()
        {
            var g = GestorIdioma.Instancia;
            int seleccionado = Math.Max(0, cmbFiltroUsofrmInsumo.SelectedIndex);
            cmbFiltroUsofrmInsumo.SelectedIndexChanged -= cmbFiltroUsofrmInsumo_SelectedIndexChanged;
            cmbFiltroUsofrmInsumo.Items.Clear();
            foreach (string clave in clavesUso)
                cmbFiltroUsofrmInsumo.Items.Add(g.Traducir(clave));
            cmbFiltroUsofrmInsumo.SelectedIndex = seleccionado;
            cmbFiltroUsofrmInsumo.SelectedIndexChanged += cmbFiltroUsofrmInsumo_SelectedIndexChanged;
        }

        private void cmbFiltroUsofrmInsumo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarInsumos();
            ModoAlta();
        }

        private BE.INSUMO InsumoSeleccionado()
        {
            if (dataGridView1.SelectedRows.Count == 0) return null;
            return dataGridView1.SelectedRows[0].Tag as BE.INSUMO;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (cargando) return;
            var insumo = InsumoSeleccionado();
            if (insumo != null) ModoEdicion(insumo);
        }

        private void btnNuevofrmInsumo_Click(object sender, EventArgs e)
        {
            ModoAlta();
            txtNombrefrmInsumo.Focus();
        }

        // Stock en la unidad de medida: en alta lo calcula la BLL (unidades × equivalencia); en edición es el actual
        private void MostrarStockCalculado()
        {
            var g = GestorIdioma.Instancia;
            decimal stock;
            if (stockEdicion.HasValue)
                stock = stockEdicion.Value;
            else
            {
                try { stock = GestorInsumo.CalcularStockInicial(nudStockInicialfrmInsumo.Value, nudEquivalenciafrmInsumo.Value); }
                catch (ArgumentException) { stock = 0; }
            }
            string unidad = cmbUnidadMedidafrmInsumo.SelectedItem?.ToString() ?? string.Empty;
            lblStockCalculadofrmInsumo.Text = string.Format(g.Traducir("msgInsumoStockCalculado"), stock.ToString("#,0.###"), unidad).Trim();
        }

        private void StockInicial_Changed(object sender, EventArgs e)
        {
            MostrarStockCalculado();
        }

        private void cmbUnidadMedidafrmInsumo_SelectedIndexChanged(object sender, EventArgs e)
        {
            MostrarStockCalculado();
        }

        // Datos editables del formulario. La unidad de medida y el stock solo cuentan en el alta.
        private BE.INSUMO LeerCampos()
        {
            return new BE.INSUMO
            {
                Nombre = txtNombrefrmInsumo.Text,
                UnidadMedida = cmbUnidadMedidafrmInsumo.SelectedItem?.ToString(),
                UnidadCompra = txtUnidadComprafrmInsumo.Text,
                EquivalenciaMagnitud = nudEquivalenciafrmInsumo.Value,
                UmbralReposicion = nudUmbralfrmInsumo.Value,
                CostoUnidadCompra = nudCostofrmInsumo.Value,
                UsoBebidas = chkUsoBebidasfrmInsumo.Checked,
                UsoComidas = chkUsoComidasfrmInsumo.Checked,
                // CU023 (decisión 52): unidades de compra × equivalencia
                VolumenPesoDisponible = GestorInsumo.CalcularStockInicial(nudStockInicialfrmInsumo.Value, nudEquivalenciafrmInsumo.Value)
            };
        }

        // CU023 pasos 3 y 4. Las validaciones son de INSUMO_BLL (FA1) y del SP (FA2).
        private void btnAltafrmInsumo_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            try
            {
                GestorInsumo.InsertarInsumo(LeerCampos());
                MessageBox.Show(g.Traducir("msgInsumoAlta"), g.Traducir("frmInsumo"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarInsumos();
                ModoAlta();
                txtNombrefrmInsumo.Focus();
            }
            catch (ArgumentException argEx)
            {
                // FA1 / FA2: se informa el dato a corregir y se conservan los datos cargados
                MessageBox.Show(g.Traducir(argEx.Message), g.Traducir("msgInsumoAviso"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(g.Traducir("msgInsumoError") + ex.GetBaseException().Message, g.Traducir("frmInsumo"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // CU024 pasos 3 y 4
        private void btnModificacionfrmInsumo_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            var seleccionado = InsumoSeleccionado();
            if (seleccionado == null)
            {
                MessageBox.Show(g.Traducir("msgInsumoSeleccionar"), g.Traducir("msgInsumoAviso"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var insumo = LeerCampos();
            insumo.IdInsumo = seleccionado.IdInsumo;
            try
            {
                GestorInsumo.ModificarInsumo(insumo);
                MessageBox.Show(g.Traducir("msgInsumoModificado"), g.Traducir("frmInsumo"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarInsumos();
                ModoAlta();
            }
            catch (ArgumentException argEx)
            {
                // FA1 / FA2: vuelve al paso 3 con los datos cargados
                MessageBox.Show(g.Traducir(argEx.Message), g.Traducir("msgInsumoAviso"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(g.Traducir("msgInsumoErrorOperacion") + ex.GetBaseException().Message, g.Traducir("frmInsumo"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // CU025 Dar de Baja Insumo
        private void btnBajafrmInsumo_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            var seleccionado = InsumoSeleccionado();
            if (seleccionado == null)
            {
                MessageBox.Show(g.Traducir("msgInsumoSeleccionar"), g.Traducir("msgInsumoAviso"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Paso 2 / FA1: el insumo no puede estar en recetas de productos activos
                var productos = GestorInsumo.VerificarBajaInsumo(seleccionado.IdInsumo);
                if (productos.Count > 0)
                {
                    MessageBox.Show(string.Format(g.Traducir("msgInsumoEnReceta"), string.Join(", ", productos.Select(p => p.Nombre))),
                        g.Traducir("msgInsumoAviso"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // FA2: el insumo figura en órdenes de compra abiertas (N02)
                var ordenes = GestorInsumo.VerificarOrdenesAbiertas(seleccionado.IdInsumo);
                if (ordenes.Count > 0)
                {
                    MessageBox.Show(string.Format(g.Traducir("msgInsumoEnOrdenAbierta"), string.Join(", ", ordenes.Select(o => $"N° {o.IdOrdenCompra} ({o.Estado})"))),
                        g.Traducir("msgInsumoAviso"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Paso 2: datos del insumo y pedido de confirmación. FA3: con "No" se vuelve al listado.
                var confirmar = MessageBox.Show(
                    string.Format(g.Traducir("msgInsumoConfirmarBaja"), seleccionado.Nombre, seleccionado.VolumenPesoDisponible.ToString("0.###"), seleccionado.UnidadMedida),
                    g.Traducir("msgInsumoConfirmarTitulo"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmar != DialogResult.Yes) return;

                // Pasos 3 y 4
                GestorInsumo.DarDeBajaInsumo(seleccionado.IdInsumo);
                MessageBox.Show(g.Traducir("msgInsumoBaja"), g.Traducir("frmInsumo"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarInsumos();
                ModoAlta();
            }
            catch (ArgumentException argEx)
            {
                MessageBox.Show(g.Traducir(argEx.Message).Replace("{0}", string.Empty), g.Traducir("msgInsumoAviso"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(g.Traducir("msgInsumoErrorOperacion") + ex.GetBaseException().Message, g.Traducir("frmInsumo"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.HeaderText = g.Traducir(col.Name);
            }
            CargarFiltroUso();
            foreach (DataGridViewRow fila in dataGridView1.Rows)
                if (fila.Tag is BE.INSUMO insumo)
                    fila.Cells["colUsofrmInsumo"].Value = TextoUso(insumo);
            MostrarStockCalculado();
        }
    }
}
