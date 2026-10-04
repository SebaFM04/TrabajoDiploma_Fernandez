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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class frmProducto : Form , IObservadorIdioma
    {
        BE.PRODUCTO producto = new BE.PRODUCTO();
        PRODUCTO_BLL GestorProducto = new PRODUCTO_BLL();

        public frmProducto()
        {
            InitializeComponent();
        }

        private void frmProducto_Load(object sender, EventArgs e)
        {
            Enlazar();
            CargarProductos();
        }
        private void Enlazar()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("Id", "Id");
            dataGridView1.Columns.Add("Nombre", "Nombre");
            dataGridView1.Columns.Add("Tipo", "Tipo");
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            cmbTipofrmProducto.Items.Clear();
            cmbTipofrmProducto.Items.AddRange(GestorProducto.ListarTiposProducto());
            cmbTipofrmProducto.SelectedIndex = -1;
        }

        private void LimpiarCampos()
        {
            textBox1.Text = string.Empty;
            cmbTipofrmProducto.SelectedIndex = -1;
        }

        private void CargarProductos()
        {
            dataGridView1.Rows.Clear();

            try
            {
                // Solo activos: los dados de baja no se operan
                var productos = GestorProducto.ListarProductosActivos();

                foreach (var p in productos)
                {
                    dataGridView1.Rows.Add(p.IdProducto, p.Nombre, p.Tipo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.GetBaseException().Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Valida solo que los campos estén completos; las reglas de negocio están en PRODUCTO_BLL
        private bool ValidarCampos(out string nombre, out string tipo)
        {
            nombre = textBox1.Text?.Trim();
            tipo = cmbTipofrmProducto.SelectedItem?.ToString();

            var errores = new List<string>();
            if (string.IsNullOrWhiteSpace(nombre)) errores.Add("El campo Nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(tipo)) errores.Add("El campo Tipo es obligatorio.");

            if (errores.Any())
            {
                MessageBox.Show(string.Join(Environment.NewLine, errores), "Errores de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void btnAltafrmProducto_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos(out string nombre, out string tipo)) return;

            producto = new BE.PRODUCTO();
            producto.Nombre = nombre;
            producto.Tipo = tipo;

            try
            {
                GestorProducto.InsertarProducto(producto);
                MessageBox.Show("Producto registrado exitosamente.", "Producto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Refrescar lista y limpiar campos
                CargarProductos();
                LimpiarCampos();
                textBox1.Focus();
            }
            catch (ArgumentException argEx)
            {
                MessageBox.Show(argEx.Message, "Aviso de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                MessageBox.Show(sqlEx.Message, "Aviso de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar el producto: " + ex.GetBaseException().Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBajafrmProducto_Click(object sender, EventArgs e)
        {
            //evita que se intente dar de baja sin seleccionar un PRODUCTO
            if (dataGridView1.SelectedRows == null || dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto para dar de baja.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = dataGridView1.SelectedRows[0];
            int id;
            if (!int.TryParse(row.Cells[0].Value?.ToString(), out id))
            {
                MessageBox.Show("ID de producto inválido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string nombreProducto = row.Cells[1].Value?.ToString() ?? "";
            var confirm = MessageBox.Show($"¿Confirma la baja del producto '{nombreProducto}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                BE.PRODUCTO p = new BE.PRODUCTO();
                p.IdProducto = id;
                p.Nombre = nombreProducto;
                int filas = GestorProducto.EliminarProducto(p);
                if (filas > 0)
                {
                    MessageBox.Show("Producto dado de baja correctamente.", "Baja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarProductos();
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show("No se pudo dar de baja el producto.", "Baja", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al dar de baja el producto: " + ex.GetBaseException().Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModificacionfrmProducto_Click(object sender, EventArgs e)
        {
            // Modificar producto seleccionado con datos de los TextBox
            if (dataGridView1.SelectedRows == null || dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto para modificar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = dataGridView1.SelectedRows[0];
            int id;
            if (!int.TryParse(row.Cells[0].Value?.ToString(), out id))
            {
                MessageBox.Show("ID de producto inválido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!ValidarCampos(out string nombre, out string tipo)) return;

            var confirm = MessageBox.Show("¿Confirma la modificación del producto?", "Confirmar Modificación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                BE.PRODUCTO producto = new BE.PRODUCTO();
                producto.IdProducto = id;
                producto.Nombre = nombre;
                producto.Tipo = tipo;

                int filas = GestorProducto.ModificarProducto(producto);
                if (filas > 0)
                {
                    MessageBox.Show("Producto modificado correctamente.", "Edición", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarProductos();
                }
                else
                {
                    MessageBox.Show("No se realizaron cambios o no se pudo modificar el producto.", "Edición", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar el producto: " + ex.GetBaseException().Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ActualizarIdioma()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is TextBox || ctrl is DataGridView || ctrl is ComboBox)
                    continue;
                ctrl.Text = GestorIdioma.Instancia.Traducir(ctrl.Name);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;

            var row = dataGridView1.SelectedRows[0];
            textBox1.Text = row.Cells["Nombre"].Value?.ToString() ?? "";
            cmbTipofrmProducto.SelectedItem = row.Cells["Tipo"].Value?.ToString();
        }
    }
}
