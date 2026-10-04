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
    // ABM de insumos. Por ahora CU023 Insertar Insumo; CU024 y CU025 se agregan después.
    public partial class frmInsumo : Form, IObservadorIdioma
    {
        INSUMO_BLL GestorInsumo = new INSUMO_BLL();

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
            LimpiarCampos();
            CargarInsumos();
        }

        private void Enlazar()
        {
            var g = GestorIdioma.Instancia;
            dataGridView1.Columns.Clear();
            foreach (string columna in new[] { "colNombrefrmInsumo", "colUnidadMedidafrmInsumo", "colUnidadComprafrmInsumo",
                                               "colEquivalenciafrmInsumo", "colStockfrmInsumo", "colUmbralfrmInsumo", "colCostofrmInsumo" })
            {
                dataGridView1.Columns.Add(columna, g.Traducir(columna));
            }
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LimpiarCampos()
        {
            txtNombrefrmInsumo.Text = string.Empty;
            cmbUnidadMedidafrmInsumo.SelectedIndex = -1;
            txtUnidadComprafrmInsumo.Text = string.Empty;
            nudEquivalenciafrmInsumo.Value = 0;
            nudUmbralfrmInsumo.Value = 0;
            nudCostofrmInsumo.Value = 0;
            nudStockInicialfrmInsumo.Value = 0;
        }

        private void CargarInsumos()
        {
            var g = GestorIdioma.Instancia;
            dataGridView1.Rows.Clear();
            try
            {
                foreach (var i in GestorInsumo.ListarInsumosActivos())
                {
                    dataGridView1.Rows.Add(i.Nombre, i.UnidadMedida, i.UnidadCompra,
                        i.EquivalenciaMagnitud.ToString("0.###"), i.VolumenPesoDisponible.ToString("0.###"),
                        i.UmbralReposicion.ToString("0.###"), i.CostoUnidadCompra.ToString("0.00"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(g.Traducir("msgInsumoError") + ex.GetBaseException().Message, g.Traducir("frmInsumo"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // CU023 pasos 3 y 4. Las validaciones son de INSUMO_BLL (FA1) y del SP (FA2).
        private void btnAltafrmInsumo_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            BE.INSUMO insumo = new BE.INSUMO
            {
                Nombre = txtNombrefrmInsumo.Text,
                UnidadMedida = cmbUnidadMedidafrmInsumo.SelectedItem?.ToString(),
                UnidadCompra = txtUnidadComprafrmInsumo.Text,
                EquivalenciaMagnitud = nudEquivalenciafrmInsumo.Value,
                UmbralReposicion = nudUmbralfrmInsumo.Value,
                CostoUnidadCompra = nudCostofrmInsumo.Value,
                VolumenPesoDisponible = nudStockInicialfrmInsumo.Value
            };

            try
            {
                GestorInsumo.InsertarInsumo(insumo);
                MessageBox.Show(g.Traducir("msgInsumoAlta"), g.Traducir("frmInsumo"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarInsumos();
                LimpiarCampos();
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
