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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class frmUsuario : Form, IObservadorIdioma
    {
        BE.USUARIO usuario = new BE.USUARIO();
        USUARIO_BLL GestorUsuario = new USUARIO_BLL();

        public frmUsuario()
        {
            InitializeComponent();
        }
        private void frmUsuario_Load(object sender, EventArgs e)
        {
            Enlazar();
            CargarUsuarios();
        }
        private void Enlazar()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("Id", "Id");
            dataGridView1.Columns.Add("Nombre", "Nombre");
            dataGridView1.Columns.Add("Apellido", "Apellido");
            dataGridView1.Columns.Add("Dni", "Dni");
            dataGridView1.Columns.Add("CorreoElectronico", "Correo Electronico");
            // Decisión 64: casilla Activo (desmarcar = baja lógica, marcar = reactivar)
            ColumnaActivo.Agregar(dataGridView1, "colActivofrmUsuario", GestorIdioma.Instancia.Traducir("colActivofrmUsuario"), CambiarActivo);
            AjusteGrilla.Configurar(dataGridView1);
        }

        private void CargarUsuarios()
        {
            dataGridView1.Rows.Clear();

            // 1. Obtener diccionario de usuarios para el mapeo de nombres
            var users = GestorUsuario.ListarUsuarios();
            Dictionary<int, string> dictUsuarios = users.ToDictionary(u => u.IdUsuario, u => u.NombreUsuario);

            // 2. Cargar los datos fila por fila
            foreach (var registro in users)
            {
                // Buscamos el nombre del usuario en el diccionario usando el ID que viene en la bitácora
                string nombreUsuario = dictUsuarios.ContainsKey(registro.IdUsuario)
                                       ? dictUsuarios[registro.IdUsuario]
                                       : "Desconocido";
                int fila = dataGridView1.Rows.Add(
                    registro.IdUsuario,
                    nombreUsuario,
                    registro.ApellidoUsuario,
                    registro.Dni,
                    registro.CorreoElectronico,
                    registro.Activo
                );
                dataGridView1.Rows[fila].Tag = registro;
                ColumnaActivo.Pintar(dataGridView1.Rows[fila], registro.Activo);
            }
        }

        private void btnAltafrmUsuario_Click(object sender, EventArgs e)
        {
            #region VALIDACIONES DE CAMPOS
            // Recolectar y normalizar valores
            string nombre = textBox1.Text?.Trim();
            string apellido = textBox2.Text?.Trim();
            string dniText = textBox3.Text?.Trim();
            string correo = textBox4.Text?.Trim();
            string contraseña = textBox5.Text ?? string.Empty;

            var errores = new List<string>();

            // Validaciones de campos obligatorios (Que todo este completo)
            if (string.IsNullOrWhiteSpace(nombre)) errores.Add("El campo Nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(apellido)) errores.Add("El campo Apellido es obligatorio.");
            if (string.IsNullOrWhiteSpace(dniText)) errores.Add("El campo DNI es obligatorio.");
            if (string.IsNullOrWhiteSpace(correo)) errores.Add("El campo Correo Electrónico es obligatorio.");
            if (string.IsNullOrWhiteSpace(contraseña)) errores.Add("El campo Contraseña es obligatorio.");

            // Validación DNI numérico y rango
            int dniParsed = 0;
            if (!string.IsNullOrWhiteSpace(dniText))
            {
                if (!int.TryParse(dniText, out dniParsed))
                {
                    errores.Add("DNI debe tener un rango númerico válido.");
                }
                else
                {
                    // Validar que tenga entre 1 y 8 dígitos (ajustar según reglas locales)
                    if (dniParsed < 1 || dniParsed > 99999999)
                    {
                        errores.Add("DNI fuera de rango válido (debe tener hasta 8 dígitos y ser mayor a 0).");
                    }
                }
            }

            // Validación de correo electrónico (utiliza el mismo patrón que frmLogin)
            if (!string.IsNullOrWhiteSpace(correo))
            {
                if (!Regex.IsMatch(correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    errores.Add("Formato de Correo Electrónico inválido.");
                }
            }

            // Validación básica de contraseña
            if (!string.IsNullOrWhiteSpace(contraseña) && contraseña.Length < 4)
            {
                errores.Add("La contraseña debe tener al menos 4 caracteres.");
            }

            // Si hay errores, mostrarlos, limpiar campos y abortar
            if (errores.Any())
            {
                MessageBox.Show(string.Join(Environment.NewLine, errores), "Errores de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Limpiar todos los TextBox del formulario para que el usuario reingrese datos
                textBox1.Text = string.Empty; // Nombre
                textBox2.Text = string.Empty; // Apellido
                textBox3.Text = string.Empty; // DNI
                textBox4.Text = string.Empty; // Correo
                textBox5.Text = string.Empty; // Contraseña

                // Poner foco en el primer campo
                textBox1.Focus();

                return;
            }
            #endregion  

            usuario.NombreUsuario = nombre;
            usuario.ApellidoUsuario = apellido;
            usuario.Dni = dniParsed;
            usuario.CorreoElectronico = correo;
            usuario.ContraseñaUsuario = contraseña;

            USUARIO_BLL GestorUsuario = new USUARIO_BLL();

            try
            {
                GestorUsuario.RegistrarUsuario(usuario);
                MessageBox.Show("Usuario registrado exitosamente.", "Registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                // RAISERROR con severidad 16 tira una SqlException. 
                // sqlEx.Message contendrá el texto: "Ya existe un usuario con ese..."
                MessageBox.Show(sqlEx.Message, "Aviso de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnBajafrmUsuario_Click(object sender, EventArgs e)
        {
            // Borrar usuario seleccionado
            if (dataGridView1.SelectedRows == null || dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un usuario para borrar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = dataGridView1.SelectedRows[0];
            int id;
            if (!int.TryParse(row.Cells[0].Value?.ToString(), out id))
            {
                MessageBox.Show("ID de usuario inválido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DarDeBaja(id, row.Cells[4].Value?.ToString() ?? "");
        }

        // Decisión 64: la casilla Activo de la grilla da de baja o reactiva
        private void CambiarActivo(DataGridViewRow fila, bool activar)
        {
            var u = fila.Tag as BE.USUARIO;
            if (u == null) return;
            if (activar)
                Reactivar(u);
            else
                DarDeBaja(u.IdUsuario, u.CorreoElectronico);
        }

        // Baja lógica (decisión 64): el usuario deja de poder iniciar sesión y conserva su historial
        private void DarDeBaja(int id, string correo)
        {
            var g = GestorIdioma.Instancia;
            var confirm = MessageBox.Show(string.Format(g.Traducir("msgUsuarioConfirmarBaja"), correo), g.Traducir("msgUsuarioConfirmarTitulo"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                int filas = GestorUsuario.EliminarUsuario(new BE.USUARIO { IdUsuario = id, CorreoElectronico = correo });
                if (filas > 0)
                {
                    MessageBox.Show(g.Traducir("msgUsuarioBaja"), g.Traducir("msgUsuarioConfirmarTitulo"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarUsuarios();
                    // Limpiar campos
                    textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = string.Empty;
                }
                else
                {
                    MessageBox.Show(g.Traducir("msgUsuarioBajaFallida"), g.Traducir("msgUsuarioConfirmarTitulo"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (ArgumentException argEx)
            {
                MessageBox.Show(g.Traducir(argEx.Message), g.Traducir("msgUsuarioConfirmarTitulo"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(g.Traducir("msgUsuarioError") + ex.GetBaseException().Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Reactivar(BE.USUARIO u)
        {
            var g = GestorIdioma.Instancia;
            if (MessageBox.Show(string.Format(g.Traducir("msgUsuarioConfirmarReactivar"), u.CorreoElectronico), g.Traducir("msgUsuarioConfirmarTitulo"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {
                GestorUsuario.ReactivarUsuario(u);
                MessageBox.Show(g.Traducir("msgUsuarioReactivado"), g.Traducir("msgUsuarioConfirmarTitulo"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show(g.Traducir("msgUsuarioError") + ex.GetBaseException().Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModificacionfrmUsuario_Click(object sender, EventArgs e)
        {
            // Modificar usuario seleccionado con datos de los TextBox
            if (dataGridView1.SelectedRows == null || dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un usuario para modificar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = dataGridView1.SelectedRows[0];
            int id;
            if (!int.TryParse(row.Cells[0].Value?.ToString(), out id))
            {
                MessageBox.Show("ID de usuario inválido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Decisión 64: un usuario dado de baja solo se reactiva (casilla Activo)
            if (row.Tag is BE.USUARIO seleccionado && !seleccionado.Activo)
            {
                MessageBox.Show(GestorIdioma.Instancia.Traducir("msgUsuarioReactivarPrimero"), GestorIdioma.Instancia.Traducir("msgUsuarioConfirmarTitulo"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Recolectar y validar campos (puede reutilizar la validación del alta)
            string nombre = textBox1.Text?.Trim();
            string apellido = textBox2.Text?.Trim();
            string dniText = textBox3.Text?.Trim();
            string correo = textBox4.Text?.Trim();
            string contraseña = textBox5.Text ?? string.Empty;

            var errores = new List<string>();
            if (string.IsNullOrWhiteSpace(nombre)) errores.Add("El campo Nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(apellido)) errores.Add("El campo Apellido es obligatorio.");
            if (string.IsNullOrWhiteSpace(dniText)) errores.Add("El campo DNI es obligatorio.");
            if (string.IsNullOrWhiteSpace(correo)) errores.Add("El campo Correo Electrónico es obligatorio.");
            if (string.IsNullOrWhiteSpace(contraseña)) errores.Add("El campo Contraseña es obligatorio.");

            int dniParsed = 0;
            if (!int.TryParse(dniText, out dniParsed)) errores.Add("DNI debe tener un rango númerico válido.");

            if (!string.IsNullOrWhiteSpace(correo) && !Regex.IsMatch(correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) errores.Add("Formato de Correo Electrónico inválido.");
            if (!string.IsNullOrWhiteSpace(contraseña) && contraseña.Length < 4) errores.Add("La contraseña debe tener al menos 4 caracteres.");

            if (errores.Any())
            {
                MessageBox.Show(string.Join(Environment.NewLine, errores), "Errores de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("¿Confirma la modificación del usuario?", "Confirmar Modificación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                BE.USUARIO usuario = new BE.USUARIO();
                usuario.IdUsuario = id;
                usuario.NombreUsuario = nombre;
                usuario.ApellidoUsuario = apellido;
                usuario.Dni = dniParsed;
                usuario.CorreoElectronico = correo;
                usuario.ContraseñaUsuario = contraseña;

                int filas = GestorUsuario.ModificarUsuario(usuario);
                if (filas > 0)
                {
                    MessageBox.Show("Usuario modificado correctamente.", "Edición", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarUsuarios();
                }
                else
                {
                    MessageBox.Show("No se realizaron cambios o no se pudo modificar el usuario.", "Edición", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar el usuario: " + ex.GetBaseException().Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (dataGridView1.Columns.Contains("colActivofrmUsuario"))
                dataGridView1.Columns["colActivofrmUsuario"].HeaderText = GestorIdioma.Instancia.Traducir("colActivofrmUsuario");
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;

            var row = dataGridView1.SelectedRows[0];
            textBox1.Text = row.Cells["Nombre"].Value?.ToString() ?? "";
            textBox2.Text = row.Cells["Apellido"].Value?.ToString() ?? "";
            textBox3.Text = row.Cells["Dni"].Value?.ToString() ?? "";
            textBox4.Text = row.Cells["CorreoElectronico"].Value?.ToString() ?? "";
            textBox5.Text = ""; // Contraseña siempre vacía por seguridad
        }
    }
}
