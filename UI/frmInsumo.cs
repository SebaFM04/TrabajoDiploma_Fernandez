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
    // ABM de insumos: CU023 Insertar Insumo, CU024 Modificar Insumo, CU025 Dar de Baja Insumo.
    // Modo alta: todos los campos editables. Modo edición (insumo seleccionado): unidad de medida y stock de solo lectura.
    public partial class frmInsumo : Form, IObservadorIdioma
    {
        INSUMO_BLL GestorInsumo = new INSUMO_BLL();
        // Evita que la carga de la grilla dispare el modo edición
        bool cargando = false;

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
                                               "colEquivalenciafrmInsumo", "colStockfrmInsumo", "colUmbralfrmInsumo", "colCostofrmInsumo" })
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

            cmbUnidadMedidafrmInsumo.Enabled = true;
            nudStockInicialfrmInsumo.Enabled = true;
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
            nudStockInicialfrmInsumo.Value = insumo.VolumenPesoDisponible;

            cmbUnidadMedidafrmInsumo.Enabled = false;
            nudStockInicialfrmInsumo.Enabled = false;
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
                foreach (var i in GestorInsumo.ListarInsumosActivos())
                {
                    int fila = dataGridView1.Rows.Add(i.IdInsumo, i.Nombre, i.UnidadMedida, i.UnidadCompra,
                        i.EquivalenciaMagnitud.ToString("0.###"), i.VolumenPesoDisponible.ToString("0.###"),
                        i.UmbralReposicion.ToString("0.###"), i.CostoUnidadCompra.ToString("0.00"));
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
                VolumenPesoDisponible = nudStockInicialfrmInsumo.Value
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
        }
    }
}
