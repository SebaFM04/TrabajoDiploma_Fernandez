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
    public partial class frmRolesyPermisos : Form, IObservadorIdioma
    {
        PERMISO_BLL permisoBLL = new PERMISO_BLL();
        USUARIO_BLL usuarioBLL = new USUARIO_BLL();
        public frmRolesyPermisos()
        {
            InitializeComponent();
        }

        private void frmRolesyPermisos_Load(object sender, EventArgs e)
        {
            ChBxfrmRolyPer.Checked = true;
            ChBxfrmRolyPer.Enabled = false;
            ChBxfrmRolyPer.Text = "Rol (siempre activo)";

            try
            {
                CargarComboRoles();
                CargarArbol();
                CargarPermisosDisponibles();
                CargarComboUsuarios();
                CargarComboRolAsignar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el formulario: " + ex.GetBaseException().Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarComboUsuarios()
        {
            comboBox2.Items.Clear();
            var usuarios = usuarioBLL.ListarUsuarios();
            foreach (var u in usuarios)
                comboBox2.Items.Add(u);
            if (comboBox2.Items.Count > 0)
                comboBox2.SelectedIndex = 0;
        }

        private void CargarComboRolAsignar()
        {
            comboBox3.Items.Clear();
            var roles = permisoBLL.ObtenerTodosLosRoles();
            foreach (var r in roles)
                comboBox3.Items.Add(r);
            if (comboBox3.Items.Count > 0)
                comboBox3.SelectedIndex = 0;
        }

        private void CargarComboRoles()
        {
            comboBox1.Items.Clear();
            var roles = permisoBLL.ObtenerTodosLosRoles();
            foreach (var r in roles)
            {
                comboBox1.Items.Add(r);
            }

            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }
        private void CargarArbol()
        {
            tVfrmRolyPer.Nodes.Clear();
            if (comboBox1.SelectedItem == null) return;

            var rolSeleccionado = (PERMISOCOMPONENT)comboBox1.SelectedItem;
            var arbol = permisoBLL.ObtenerPermisoConJerarquiaPorId(rolSeleccionado.Id);

            if (arbol != null)
                tVfrmRolyPer.Nodes.Add(CrearNodo(arbol, new HashSet<string>()));

            tVfrmRolyPer.ExpandAll();
        }

        private void CargarPermisosDisponibles()
        {
            lstfrmRolyPer.Items.Clear();
            var todos = permisoBLL.ObtenerTodosLosPermisos();
            foreach (var p in todos)
            {
                lstfrmRolyPer.Items.Add(p);
            }
        }

        private void CargarPermisosDelUsuario()
        {
            tVPermisosUsuario.Nodes.Clear();

            if (comboBox2.SelectedItem == null) return;

            var usuario = (BE.USUARIO)comboBox2.SelectedItem;

            try
            {
                var permisos = permisoBLL.ListarPermisosJerarquicosPorUsuarioId(usuario.IdUsuario);

                if (permisos.Count == 0)
                {
                    tVPermisosUsuario.Nodes.Add(new TreeNode("(sin roles asignados)"));
                    return;
                }

                foreach (var permiso in permisos)
                    tVPermisosUsuario.Nodes.Add(CrearNodo(permiso));

                tVPermisosUsuario.ExpandAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar permisos del usuario: " + ex.GetBaseException().Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }  

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarArbol();
        }

        private void Refrescar()
        {
            CargarComboRoles();
            CargarArbol();
            CargarPermisosDisponibles();
            CargarComboRolAsignar();
            CargarPermisosDelUsuario();
        }

        private void LimpiarCampos()
        {
            textBox1.Text = string.Empty;
            ChBxfrmRolyPer.Checked = true;
        }

        public void ActualizarIdioma()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is TextBox || ctrl is DataGridView || ctrl is ComboBox || ctrl is TreeView || ctrl is ListBox) continue;
                ctrl.Text = GestorIdioma.Instancia.Traducir(ctrl.Name);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarPermisosDelUsuario();
        }

        /*Como estaba antes (Entrega 2)
        private void CargarArbol()
        {
            tVfrmRolyPer.Nodes.Clear();

            if (comboBox1.SelectedItem == null) return;

            var rolSeleccionado = (PERMISOCOMPONENT)comboBox1.SelectedItem;
            var arbol = permisoBLL.ObtenerPermisoConJerarquiaPorId(rolSeleccionado.Id);

            if (arbol != null)
            {
                tVfrmRolyPer.Nodes.Add(CrearNodo(arbol));
            }

            tVfrmRolyPer.ExpandAll();
        }
        */
        /*Como estaba antes (Entrega 2)
        private TreeNode CrearNodo(PERMISOCOMPONENT permiso)
        {
            var nodo = new TreeNode(permiso.NombrePermiso);
            nodo.Tag = permiso;
            foreach (var hijo in permiso.ListarPermisosHijos())
            {
                nodo.Nodes.Add(CrearNodo(hijo));
            }
            return nodo;
        }
        */
        private TreeNode CrearNodo(PERMISOCOMPONENT permiso, HashSet<string> atomicosYaMostrados = null)
        {
            if (atomicosYaMostrados == null)
                atomicosYaMostrados = new HashSet<string>();

            var nodo = new TreeNode(permiso.NombrePermiso);
            nodo.Tag = permiso;

            foreach (var hijo in permiso.ListarPermisosHijos())
            {
                if (!hijo.EsFamilia)
                {
                    // Si el permiso atómico ya fue mostrado en un nivel superior, omitirlo
                    if (atomicosYaMostrados.Contains(hijo.NombrePermiso))
                        continue;
                    atomicosYaMostrados.Add(hijo.NombrePermiso);
                }
                nodo.Nodes.Add(CrearNodo(hijo, atomicosYaMostrados));
            }
            return nodo;
        }

        private void tVfrmRolyPer_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is PERMISOCOMPONENT p)
            {
                textBox1.Text = p.NombrePermiso;
                ChBxfrmRolyPer.Checked = p is PERMISOCOMPOSITE;
            }
        }     
        private void btn1frmRolyPer_Click(object sender, EventArgs e)
        {
            string nombre = textBox1.Text.Trim();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("Ingrese un nombre para el permiso.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool esFamilia = ChBxfrmRolyPer.Checked;

            try
            {
                permisoBLL.CrearPermiso(nombre, esFamilia);
                MessageBox.Show("Permiso agregado correctamente.", "Alta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                Refrescar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.GetBaseException().Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
      
        private void btn2frmRolyPer_Click(object sender, EventArgs e)
        {
            if (tVfrmRolyPer.SelectedNode == null)
            {
                MessageBox.Show("Seleccione un permiso en el árbol.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nombre = textBox1.Text.Trim();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("Ingrese el nuevo nombre.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var permiso = (PERMISOCOMPONENT)tVfrmRolyPer.SelectedNode.Tag;
            bool esFamilia = permiso is PERMISOCOMPOSITE;

            try
            {
                permisoBLL.ModificarPermiso(permiso.Id, nombre, esFamilia);
                MessageBox.Show("Permiso modificado correctamente.", "Modificación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                Refrescar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.GetBaseException().Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn3frmRolyPer_Click(object sender, EventArgs e)
        {
            if (tVfrmRolyPer.SelectedNode == null)
            {
                MessageBox.Show("Seleccione un permiso en el árbol.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var permiso = (PERMISOCOMPONENT)tVfrmRolyPer.SelectedNode.Tag;
            var confirm = MessageBox.Show($"¿Confirma eliminar el permiso '{permiso.NombrePermiso}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                permisoBLL.EliminarPermiso(permiso.Id);
                MessageBox.Show("Permiso eliminado correctamente.", "Baja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                Refrescar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.GetBaseException().Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn4frmRolyPer_Click(object sender, EventArgs e)
        {
            /*Como estaba antes (Entrega 2)
            if (tVfrmRolyPer.SelectedNode == null || lstfrmRolyPer.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un permiso padre en el árbol y un hijo de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var padre = (PERMISOCOMPONENT)tVfrmRolyPer.SelectedNode.Tag;
            var hijo = (PERMISOCOMPONENT)lstfrmRolyPer.SelectedItem;

            try
            {
                permisoBLL.AgregarRelacion(padre.Id, hijo.Id);
                Refrescar();
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }*/

            if (tVfrmRolyPer.SelectedNode == null || lstfrmRolyPer.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un permiso padre en el árbol y un hijo de la lista.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var padre = (PERMISOCOMPONENT)tVfrmRolyPer.SelectedNode.Tag;
            var hijo = (PERMISOCOMPONENT)lstfrmRolyPer.SelectedItem;

            try
            {
                permisoBLL.AgregarRelacion(padre.Id, hijo.Id);

                // Mostrar advertencia si hay permisos duplicados
                if (!string.IsNullOrEmpty(permisoBLL.MensajeDuplicados))
                    MessageBox.Show(permisoBLL.MensajeDuplicados, "Advertencia",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Refrescar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn5frmRolyPer_Click(object sender, EventArgs e)
        {
            if (tVfrmRolyPer.SelectedNode == null ||
                tVfrmRolyPer.SelectedNode.Parent == null)
            {
                MessageBox.Show("Seleccione un nodo hijo en el árbol (no la raíz).", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var hijo = (PERMISOCOMPONENT)tVfrmRolyPer.SelectedNode.Tag;
            var padre = (PERMISOCOMPONENT)tVfrmRolyPer.SelectedNode.Parent.Tag;

            try
            {
                permisoBLL.QuitarRelacion(padre.Id, hijo.Id);
                Refrescar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.GetBaseException().Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn6frmRolyPer_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null || comboBox3.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un usuario y un rol.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var usuario = (BE.USUARIO)comboBox2.SelectedItem;
            var rol = (PERMISOCOMPONENT)comboBox3.SelectedItem;

            try
            {
                permisoBLL.AsignarPermisoAUsuario(usuario.IdUsuario, rol.Id);
                MessageBox.Show($"Rol '{rol.NombrePermiso}' asignado a '{usuario.CorreoElectronico}' correctamente.", "Asignación", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.GetBaseException().Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnfrmPermisosDesag_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Seleccioná un usuario.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (tVPermisosUsuario.SelectedNode == null)
            {
                MessageBox.Show("Seleccioná un rol en el árbol del usuario.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (tVPermisosUsuario.SelectedNode.Parent != null)
            {
                MessageBox.Show("Solo se pueden desasignar roles asignados directamente al usuario.\n" + "Seleccioná un nodo raíz del árbol.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var usuario = (BE.USUARIO)comboBox2.SelectedItem;
            var rol = (PERMISOCOMPONENT)tVPermisosUsuario.SelectedNode.Tag;

            var confirm = MessageBox.Show($"¿Confirma desasignar el rol '{rol.NombrePermiso}' del usuario '{usuario.CorreoElectronico}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                permisoBLL.DesasignarPermisoDeUsuario(usuario.IdUsuario, rol.Id);
                MessageBox.Show($"Rol '{rol.NombrePermiso}' desasignado correctamente.", "Desasignación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Refrescar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.GetBaseException().Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }   
    }
}
