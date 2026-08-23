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
    public partial class frmControlCambio : Form, IObservadorIdioma
    {
        CONTROLCAMBIO_BLL gestorCambios = new CONTROLCAMBIO_BLL();
        public frmControlCambio()
        {
            InitializeComponent();
        }

        private void btnFrmBRRestaurar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un cambio para revertir.", "Aviso",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var cambio = dataGridView1.SelectedRows[0].DataBoundItem as CONTROLCAMBIO;
            if (cambio == null) return;

            if (string.IsNullOrWhiteSpace(cambio.ValorAnterior))
            {
                MessageBox.Show("Este registro no tiene valor anterior para revertir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cambio.TipoOperacion == "Alta" || cambio.TipoOperacion == "Baja")
            {
                MessageBox.Show("No se pueden revertir operaciones de Alta o Baja.","Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"¿Confirma revertir el campo '{cambio.CampoModificado}' " + $"del producto ID {cambio.IdProducto}?\n\n" + $"Valor actual:   {cambio.ValorActual}\n" + $"Valor anterior: {cambio.ValorAnterior}","Confirmar Reversión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                gestorCambios.RevertirCambio(cambio,SessionManager.Instancia.UsuarioActual.IdUsuario);

                MessageBox.Show("Cambio revertido correctamente.","Reversión", MessageBoxButtons.OK,MessageBoxIcon.Information);

                CargarCambios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al revertir: " + ex.GetBaseException().Message,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmControlCambios_Load(object sender, EventArgs e)
        {
            CargarCambios();
        }

        private void CargarCambios()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = gestorCambios.ListarTodos();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
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
        //Nuevo Entrega 3
        private void btnRevertirTodo_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un registro del producto a revertir.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var cambio = dataGridView1.SelectedRows[0].DataBoundItem as CONTROLCAMBIO;
            if (cambio == null) return;

            var confirm = MessageBox.Show(
                $"¿Confirma revertir TODOS los cambios del producto ID {cambio.IdProducto}?\n\n" +
                "El producto volverá a su estado anterior.",
                "Confirmar Reversión Total",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                gestorCambios.RevertirTodo(
                    cambio.IdProducto,
                    SessionManager.Instancia.UsuarioActual.IdUsuario);

                MessageBox.Show("Todos los cambios del producto fueron revertidos correctamente.",
                    "Reversión Total", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarCambios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al revertir: " + ex.GetBaseException().Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
