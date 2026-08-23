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
    public partial class frmMenú : Form, IObservadorIdioma
    {
        GUIManager gestorUI = GUIManager.Instancia;
        public frmMenú()
        {
            InitializeComponent();
            GestorIdioma.Instancia.Suscribir(this);
            SERVICIO.SessionManager sesion = SERVICIO.SessionManager.Instancia;
            var g = GestorIdioma.Instancia;           
        }
        private void AplicarPermisos()
        {
            var usuario = SERVICIO.SessionManager.Instancia.UsuarioActual;

            // Sin permisos asignados → muestra todo
            if (usuario.PermisosAsignados == null || usuario.PermisosAsignados.Count == 0)
                return;

            gestiónUsuariosToolStripMenuItem.Visible = usuario.TienePermiso("Gestion Usuarios");
            gestiónProductosToolStripMenuItem.Visible = usuario.TienePermiso("Gestion Productos");
            adminitraciónToolStripMenuItem.Visible = usuario.TienePermiso("Auditoria");
            idiomaToolStripMenuItem.Visible = usuario.TienePermiso("Gestion Idiomas");
            backUpToolStripMenuItem1.Visible = usuario.TienePermiso("BackUp");
            recalcularDVToolStripMenuItem.Visible = usuario.TienePermiso("Auditoria");
        }

        private void btnCerrarSesionfrmMenu_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            var result = MessageBox.Show(g.Traducir("msgCerrarSesionConfirm"),g.Traducir("msgCerrarSesionTitulo"),MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                new BLL.USUARIO_BLL().LogoutUsuario();
                this.Close();
            }
        }
        private void frmMenú_Load(object sender, EventArgs e)
        {
            AplicarPermisos();
            CargarComboIdiomas();
        }
        private void formularioUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gestorUI.AbrirForm(new frmUsuario());

            //this.Hide();

            //frmUsuario frmUsuario = new frmUsuario();
            //frmUsuario.MdiParent = MdiParent;
            //frmUsuario.ShowDialog();

            //this.Show();    
        }

        private void bitacoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gestorUI.AbrirForm(new frmBitacora());
        }

        private void formularioProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gestorUI.AbrirForm(new frmProducto());
        }

        private void admRolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gestorUI.AbrirForm(new frmRolesyPermisos());
        }
        private void backUpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            gestorUI.AbrirForm(new frmBackUp_Restore());
        }

        private void recalcularDVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            var confirm = MessageBox.Show(g.Traducir("msgRecalcularConfirm"), g.Traducir("msgRecalcularTitulo"),MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                new BLL.PRODUCTO_BLL().RecalcularDV();
                MessageBox.Show( g.Traducir("msgRecalcularOk"),g.Traducir("msgRecalcularTitulo"),MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al recalcular: " + ex.GetBaseException().Message,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void controlCambiosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gestorUI.AbrirForm(new frmControlCambio());            
        }

        public void ActualizarIdioma()
        {
            var g = GestorIdioma.Instancia;
            var sesion = SERVICIO.SessionManager.Instancia;

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is TextBox || ctrl is MenuStrip || ctrl is ComboBox || ctrl.Name is "lblEmailTag" || ctrl.Name is "lblNombreTag") continue;
                ctrl.Text = g.Traducir(ctrl.Name);
            }
            foreach (ToolStripMenuItem item in mnstripMenu.Items)
                TraducirMenuItem(item);
            CargarComboIdiomas();
            if (sesion.IsLogged())
            {
                lblEmailTag.Text = $"{g.Traducir("lblEmailTag")}: {sesion.UsuarioActual.CorreoElectronico}";
                lblNombreTag.Text = $"{g.Traducir("lblNombreTag")}: {sesion.UsuarioActual.NombreUsuario} {sesion.UsuarioActual.ApellidoUsuario}";
            }
        }

        private void TraducirMenuItem(ToolStripMenuItem item)
        {
            string clave = item.Name;
            string traduccion = GestorIdioma.Instancia.Traducir(clave);
            // Solo reemplaza si encontró una traducción real
            if (traduccion != clave)
                item.Text = traduccion;

            foreach (ToolStripMenuItem sub in item.DropDownItems.OfType<ToolStripMenuItem>())
                TraducirMenuItem(sub);
        }

        private void CargarComboIdiomas()
        {
            comboIdiomas.SelectedIndexChanged -= comboIdiomas_SelectedIndexChanged;

            var idiomas = new IDIOMA_BLL().ListarIdiomas()
                              .Where(i => i.IsDisponible)
                              .ToList();

            idiomas.Insert(0, new IDIOMA { IdIdioma = -1, Nombre = "-- Idioma / Language --" });

            comboIdiomas.DataSource = null;      
            comboIdiomas.DataSource = idiomas;
            comboIdiomas.DisplayMember = "Nombre";
            comboIdiomas.ValueMember = "IdIdioma";

            int idActual = GestorIdioma.Instancia.IdIdiomaActual;
            var existe = idiomas.Any(i => i.IdIdioma == idActual);
            comboIdiomas.SelectedValue = existe ? (object)idActual : -1;

            comboIdiomas.SelectedIndexChanged += comboIdiomas_SelectedIndexChanged;
        }

        private void comboIdiomas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboIdiomas.SelectedItem == null) return;
            var idioma = (IDIOMA)comboIdiomas.SelectedItem;
            if (idioma.IdIdioma == -1) return; // placeholder, no hacer nada
            new IDIOMA_BLL().CambiarIdioma(idioma.IdIdioma);
        }

        private void admIdiomasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gestorUI.AbrirForm(new frmABMIdioma());
            CargarComboIdiomas();
        }

        private void frmMenú_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SERVICIO.SessionManager.Instancia.IsLogged())
            {
                new BLL.USUARIO_BLL().LogoutUsuario();
            }
        }
    }
}
