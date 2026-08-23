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
    public partial class frmABMIdioma : Form, IObservadorIdioma
    {
        private IDIOMA_BLL _idiomaBLL = new IDIOMA_BLL();
        private int _idIdiomaSeleccionado = -1;

        public frmABMIdioma()
        {
            InitializeComponent();
            GestorIdioma.Instancia.Suscribir(this);
        }

        public void ActualizarIdioma()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is DataGridView || ctrl is ComboBox || ctrl is TextBox) continue;
                ctrl.Text = GestorIdioma.Instancia.Traducir(ctrl.Name);
            }
        }

        private void frmABMIdioma_Load(object sender, EventArgs e)
        {
            ConfigurarGrillas();
            CargarIdiomas();
        }

        private void ConfigurarGrillas()
        {
            // dgvIdiomas
            dgvIdiomas.Columns.Clear();
            dgvIdiomas.Columns.Add("IdIdioma", "Id");
            dgvIdiomas.Columns["IdIdioma"].Visible = false;
            dgvIdiomas.Columns.Add("Nombre", "Idioma");
            dgvIdiomas.Columns.Add("IsDisponible", "Disponible");
            dgvIdiomas.Columns["Nombre"].ReadOnly = true;
            dgvIdiomas.Columns["IsDisponible"].ReadOnly = true;
            dgvIdiomas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvIdiomas.MultiSelect = false;

            // dgvTraduccion
            dgvTraduccion.Columns.Clear();
            dgvTraduccion.Columns.Add("IdControl", "Id");
            dgvTraduccion.Columns["IdControl"].Visible = false;
            dgvTraduccion.Columns.Add("NombreFormulario", "Formulario");
            dgvTraduccion.Columns.Add("NombreControl", "Control");
            dgvTraduccion.Columns.Add("TextoTraducido", "Traducción");
            dgvTraduccion.Columns["NombreFormulario"].ReadOnly = true;
            dgvTraduccion.Columns["NombreControl"].ReadOnly = true;
            dgvTraduccion.Columns["TextoTraducido"].ReadOnly = false;
            dgvTraduccion.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void CargarIdiomas()
        {
            dgvIdiomas.Rows.Clear();
            var idiomas = new IDIOMA_BLL().ListarIdiomas();
            foreach (var i in idiomas)
                dgvIdiomas.Rows.Add(i.IdIdioma, i.Nombre, i.IsDisponible);

            // Forzar selección del primer item al cargar
            if (dgvIdiomas.Rows.Count > 0)
            {
                dgvIdiomas.Rows[0].Selected = true;
                _idIdiomaSeleccionado = Convert.ToInt32(dgvIdiomas.Rows[0].Cells["IdIdioma"].Value);
                CargarTraducciones(_idIdiomaSeleccionado);
                ActualizarBotonDeshabilitar(); 
            }
            CargarComboIdiomasABM();
        }

        private void CargarTraducciones(int idIdioma)
        {
            dgvTraduccion.Rows.Clear();
            var traducciones = _idiomaBLL.ObtenerTraduccionesConDetalle(idIdioma);
            foreach (var t in traducciones)
            {
                dgvTraduccion.Rows.Add(t.IdControl, t.NombreFormulario, t.NombreControl, t.TextoTraducido);
            }
        }

        private void ActualizarBotonDeshabilitar()
        {
            if (_idIdiomaSeleccionado == -1)
            {
                btnDeshabilitar.Text = GestorIdioma.Instancia.Traducir("btnDeshabilitar");
                return;
            }
            var idioma = _idiomaBLL.ListarIdiomas().Find(i => i.IdIdioma == _idIdiomaSeleccionado);
            if (idioma != null)
            {
                btnDeshabilitar.Text = idioma.IsDisponible ? GestorIdioma.Instancia.Traducir("btnDeshabilitar") : GestorIdioma.Instancia.Traducir("btnHabilitar");
            }
        }

        private void dgvIdiomas_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvIdiomas.CurrentRow == null) return;

            _idIdiomaSeleccionado = Convert.ToInt32(dgvIdiomas.CurrentRow.Cells["IdIdioma"].Value);
            CargarTraducciones(_idIdiomaSeleccionado);
            ActualizarBotonDeshabilitar(); 
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombreIdioma.Text.Trim();
            if (string.IsNullOrWhiteSpace(nombre)) return;
            try
            {
                _idiomaBLL.AgregarIdioma(nombre);
                txtNombreIdioma.Clear();
                CargarIdiomas();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            CargarComboIdiomasABM();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (_idIdiomaSeleccionado == -1)
            {
                MessageBox.Show("Seleccioná un idioma.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string nombre = txtNombreIdioma.Text.Trim();
            if (string.IsNullOrWhiteSpace(nombre)) return;
            try
            {
                _idiomaBLL.EditarNombreIdioma(_idIdiomaSeleccionado, nombre);
                txtNombreIdioma.Clear();
                CargarIdiomas();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            CargarComboIdiomasABM();
        }

        private void btnDeshabilitar_Click(object sender, EventArgs e)
        {
            if (_idIdiomaSeleccionado == -1)
            {
                MessageBox.Show("Seleccioná un idioma.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var idiomas = _idiomaBLL.ListarIdiomas();
            var idiomaActual = idiomas.Find(i => i.IdIdioma == _idIdiomaSeleccionado);

            // Solo validar si se intenta DESHABILITAR (no habilitar)
            if (idiomaActual != null && idiomaActual.IsDisponible)
            {
                int disponibles = idiomas.Count(i => i.IsDisponible);
                if (disponibles <= 1)
                {
                    MessageBox.Show("No se puede deshabilitar el único idioma disponible.","Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            try
            {
                _idiomaBLL.ToggleDisponibilidad(_idIdiomaSeleccionado);
                CargarIdiomas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            CargarComboIdiomasABM();
        }

        private void btnModificacionTraduccion_Click(object sender, EventArgs e)
        {
            if (_idIdiomaSeleccionado == -1)
            {
                MessageBox.Show("Seleccioná un idioma.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                var traducciones = new List<TRADUCCION_DETALLE>();
                foreach (DataGridViewRow row in dgvTraduccion.Rows)
                {
                    if (row.IsNewRow) continue;
                    traducciones.Add(new TRADUCCION_DETALLE
                    {
                        IdControl = Convert.ToInt32(row.Cells["IdControl"].Value),
                        TextoTraducido = row.Cells["TextoTraducido"].Value?.ToString() ?? ""
                    });
                }
                _idiomaBLL.ModificarTraducciones(_idIdiomaSeleccionado, traducciones);
                MessageBox.Show("Traducciones guardadas.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            
        }
        private void CargarComboIdiomasABM()
        {
            comboIdiomasABM.SelectedIndexChanged -= comboIdiomasABM_SelectedIndexChanged;

            var idiomas = new IDIOMA_BLL().ListarIdiomas()
                                          .Where(i => i.IsDisponible)
                                          .ToList();
            idiomas.Insert(0, new IDIOMA { IdIdioma = -1, Nombre = "-- Idioma / Language --" });

            comboIdiomasABM.DataSource = null;
            comboIdiomasABM.DataSource = idiomas;
            comboIdiomasABM.DisplayMember = "Nombre";
            comboIdiomasABM.ValueMember = "IdIdioma";

            int idActual = GestorIdioma.Instancia.IdIdiomaActual;
            comboIdiomasABM.SelectedValue = idiomas.Any(i => i.IdIdioma == idActual)
                                             ? (object)idActual
                                             : -1;
            comboIdiomasABM.SelectedIndexChanged += comboIdiomasABM_SelectedIndexChanged;
        }
        private void comboIdiomasABM_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboIdiomasABM.SelectedValue == null) return;
            int id = Convert.ToInt32(comboIdiomasABM.SelectedValue);
            if (id == -1) return;

            new IDIOMA_BLL().CambiarIdioma(id);
        }
    }
}
