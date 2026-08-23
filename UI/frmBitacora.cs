using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using BE;
using SERVICIO.MultiIdioma_Observer;

namespace UI
{
    public partial class frmBitacora : Form, IObservadorIdioma
    {
        BITACORA_BLL Bitacorabll = new BITACORA_BLL();
        USUARIO_BLL Usariobll = new USUARIO_BLL();
        public frmBitacora()
        {
            InitializeComponent();
        }
        private void frmBitacora_Load(object sender, EventArgs e)
        {
            CargarUsuario();
            Enlazar();
        }
        private void Enlazar()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("Id", "ID");
            dataGridView1.Columns.Add("Usuario", "Usuario");
            dataGridView1.Columns.Add("Actividad", "Actividad");
            dataGridView1.Columns.Add("FechaHora", "Fecha y Hora");
            dataGridView1.Columns.Add("InformacionAsociada", "Información Asociada");
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void CargarBitacora(List<BITACORA> bitacoraList)
        {
            dataGridView1.Rows.Clear();

            // 1. Obtener diccionario de usuarios para el mapeo de nombres
            var users = Usariobll.ListarUsuarios();
            Dictionary<int, string> dictUsuarios = users.ToDictionary(u => u.IdUsuario, u => u.CorreoElectronico);

            // 2. Cargar los datos fila por fila
            foreach (var registro in bitacoraList)
            {
                // Buscamos el nombre del usuario en el diccionario usando el ID que viene en la bitácora
                string nombreUsuario = dictUsuarios.ContainsKey(registro.IdUsuario)
                                       ? dictUsuarios[registro.IdUsuario]
                                       : "Desconocido";

                dataGridView1.Rows.Add(registro.IdBitacora,nombreUsuario,registro.Actividad,registro.FechaHora,registro.InformacionAsociada);
            }
        }

        private void CargarUsuario()
        {
            try
            {
                var users = Usariobll.ListarUsuarios();
                var userList = users.Select(u => new { u.IdUsuario, u.CorreoElectronico }).ToList();
                comboBox1.DataSource = userList;
                comboBox1.DisplayMember = "CorreoElectronico";
                comboBox1.ValueMember = "IdUsuario";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la lista de usuarios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuscarBitacora_Click(object sender, EventArgs e)
        {
            try
            {
                int idUsuario = 0;
                if (comboBox1.SelectedValue != null && Convert.ToInt32(comboBox1.SelectedValue) != 0)
                {
                    idUsuario = Convert.ToInt32(comboBox1.SelectedValue);
                }
                //MEJORA A FUTURO
                //Aca podria agregar un TextBox para que el usuario ingrese la actividad que desea buscar, y luego obtener el valor de ese TextBox en lugar de usar una cadena vacía.
                //Para ello tendria que agregar un TextBox en el diseñador y luego obtener su valor en este metodo.
                string actividad = "";
                if (string.IsNullOrEmpty(actividad))
                {
                    actividad = null;
                }

                DateTime? fechaDesde = dtpFechaDesde.Checked ? (DateTime?)dtpFechaDesde.Value.Date : null;
                DateTime? fechaHasta = dtpFechaHasta.Checked ? (DateTime?)dtpFechaHasta.Value.Date.AddDays(1).AddSeconds(-1) : null;

                var registros = Bitacorabll.BuscarRegistros(idUsuario, actividad, fechaDesde, fechaHasta);
                CargarBitacora(registros);
            }
            catch (Exception ex )
            {
                MessageBox.Show($"Error al buscar registros en la bitácora: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiarBitacora_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
            dtpFechaDesde.Value = DateTime.Now;
        }

        public void ActualizarIdioma()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is TextBox || ctrl is DataGridView || ctrl is ComboBox || ctrl is DateTimePicker)
                    continue;
                ctrl.Text = GestorIdioma.Instancia.Traducir(ctrl.Name);
            }
        }
    }
}
