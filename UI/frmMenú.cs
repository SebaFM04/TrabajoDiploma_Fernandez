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
        // Roles (decisión 55): cada opción del menú depende de su permiso y cada menú padre se muestra
        // si tiene al menos una opción disponible. Un usuario sin permisos no ve ninguna opción.
        private void AplicarPermisos()
        {
            var usuario = SERVICIO.SessionManager.Instancia.UsuarioActual;
            Func<string, bool> tiene = permiso => usuario.PermisosAsignados != null && usuario.TienePermiso(permiso);

            formularioUsuariosToolStripMenuItem.Available = tiene("Gestion Usuarios");
            admRolesToolStripMenuItem.Available = tiene("Adm Roles y Permisos");

            formularioProductosToolStripMenuItem.Available = tiene("Gestion Productos");
            formularioInsumosToolStripMenuItem.Available = tiene("Gestion Insumos");
            formularioRecetasToolStripMenuItem.Available = tiene("Gestion Recetas");

            // Operación del bar (N01)
            registrarVentaToolStripMenuItem.Available = tiene("Gestion Ventas");
            entregaPiqueosToolStripMenuItem.Available = tiene("Gestion Comandas");
            entregaBebidasToolStripMenuItem.Available = tiene("Gestion Entregas");

            // Consultas (decisión 56)
            consultaVentasToolStripMenuItem.Available = tiene("Consulta Ventas");
            consultaValesToolStripMenuItem.Available = tiene("Consulta Vales");

            bitacoraToolStripMenuItem.Available = tiene("Auditoria");
            backUpToolStripMenuItem1.Available = tiene("BackUp");
            recalcularDVToolStripMenuItem.Available = tiene("Recalcular DV") || tiene("Auditoria");
            controlCambiosToolStripMenuItem.Available = tiene("Control Cambios");

            admIdiomasToolStripMenuItem.Available = tiene("Gestion Idiomas");
            recepcionToolStripMenuItem.Available = tiene("Registrar Recepcion");
            aprobacionOrdenToolStripMenuItem.Available = tiene("Aprobar Ordenes Compra");
            ordenCompraToolStripMenuItem.Available = tiene("Gestion Ordenes Compra");
            proveedoresToolStripMenuItem.Available = tiene("Gestion Proveedores");

            foreach (ToolStripMenuItem padre in mnstripMenu.Items.OfType<ToolStripMenuItem>())
                padre.Available = padre.DropDownItems.OfType<ToolStripItem>().Any(h => h.Available);
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
            ActualizarTablero();
        }

        // ── Tablero del menú (decisión 56) ───────────────────────────
        // Cada indicador se muestra según los permisos del usuario. Se actualiza al volver de cada formulario.
        private void ActualizarTablero()
        {
            var g = GestorIdioma.Instancia;
            var usuario = SessionManager.Instancia.UsuarioActual;
            if (usuario == null) return;

            bool verVentas = usuario.TienePermiso("Gestion Ventas") || usuario.TienePermiso("Consulta Ventas");
            bool verComandas = usuario.TienePermiso("Gestion Comandas");
            bool verVales = usuario.TienePermiso("Gestion Entregas") || usuario.TienePermiso("Consulta Vales");
            bool verStock = usuario.TienePermiso("Ver Stock Bajo") || usuario.TienePermiso("Gestion Insumos");
            lblVentasHoyfrmMenu.Visible = verVentas;
            lblComandasPendientesfrmMenu.Visible = verComandas;
            lblValesSinUsarfrmMenu.Visible = verVales;
            lnkStockBajofrmMenu.Visible = verStock;
            lblTableroTitulofrmMenu.Visible = btnActualizarTablerofrmMenu.Visible = verVentas || verComandas || verVales || verStock;
            lblTableroTitulofrmMenu.Text = g.Traducir("lblTableroTitulofrmMenu");
            btnActualizarTablerofrmMenu.Text = g.Traducir("btnActualizarTablerofrmMenu");

            try
            {
                if (verVentas)
                {
                    var ventasHoy = new VENTA_BLL().ListarVentas(DateTime.Today, DateTime.Today);
                    lblVentasHoyfrmMenu.Text = string.Format(g.Traducir("msgTableroVentasHoy"), ventasHoy.Count, FormatoMoneda.Pesos(ventasHoy.Sum(v => v.Monto)));
                }
                if (verComandas)
                    lblComandasPendientesfrmMenu.Text = string.Format(g.Traducir("msgTableroComandas"), new COMANDA_BLL().ListarPendientes().Count);
                if (verVales)
                    lblValesSinUsarfrmMenu.Text = string.Format(g.Traducir("msgTableroVales"),
                        new VALE_BLL().ListarVales(DateTime.Today, DateTime.Today, VALE_BLL.EstadoSinUsar).Count(v => v.TieneBebidas));
                if (verStock)
                {
                    int bajos = new INSUMO_BLL().ListarStockBajo().Count;
                    lnkStockBajofrmMenu.Text = bajos > 0 ? string.Format(g.Traducir("msgTableroStockBajo"), bajos) : g.Traducir("msgTableroStockOk");
                    lnkStockBajofrmMenu.LinkColor = lnkStockBajofrmMenu.ActiveLinkColor = bajos > 0 ? System.Drawing.Color.Firebrick : System.Drawing.Color.DarkGreen;
                    lnkStockBajofrmMenu.Enabled = bajos > 0;
                }
            }
            catch (Exception ex)
            {
                // El tablero es informativo: un error no impide usar el menú
                lblTableroTitulofrmMenu.Text = g.Traducir("lblTableroTitulofrmMenu") + " (" + ex.GetBaseException().Message + ")";
            }
        }

        private void frmMenú_Activated(object sender, EventArgs e)
        {
            ActualizarTablero();
        }

        private void btnActualizarTablerofrmMenu_Click(object sender, EventArgs e)
        {
            ActualizarTablero();
        }

        private void lnkStockBajofrmMenu_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            gestorUI.AbrirForm(new frmStockBajo());
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

        private void formularioInsumosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gestorUI.AbrirForm(new frmInsumo());
        }

        private void formularioRecetasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gestorUI.AbrirForm(new frmReceta());
        }

        // CU010 Registrar Venta (incluye CU015 Emitir Factura)
        private void registrarVentaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gestorUI.AbrirForm(new frmVenta());
        }

        // CU011 Confirmar Entrega de Piqueos
        private void entregaPiqueosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gestorUI.AbrirForm(new frmComanda());
        }

        // CU012 Confirmar Entrega de Bebidas
        private void entregaBebidasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gestorUI.AbrirForm(new frmEntrega());
        }

        // Consultas (decisión 56)
        private void consultaVentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gestorUI.AbrirForm(new frmConsultaVentas());
        }

        // CU022, CU026 y CU027: ABM de proveedores
        private void proveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gestorUI.AbrirForm(new frmProveedor());
        }

        // CU016 Generar Orden de Compra y CU018 Ajustar Orden Observada
        private void ordenCompraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gestorUI.AbrirForm(new frmOrdenCompra());
        }

        // CU017 Aprobar Orden de Compra
        private void aprobacionOrdenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gestorUI.AbrirForm(new frmAprobacionOrden());
        }

        // CU019 Registrar Recepción y CU020 Registrar Reclamo
        private void recepcionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gestorUI.AbrirForm(new frmRecepcion());
        }

        private void consultaValesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gestorUI.AbrirForm(new frmConsultaVales());
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
                if (ctrl is TextBox || ctrl is MenuStrip || ctrl is ComboBox || ctrl.Name is "lblEmailTag" || ctrl.Name is "lblNombreTag" || ctrl.Name is "lblRolesTag") continue;
                // Los indicadores del tablero se arman en ActualizarTablero
                if (ctrl == lblVentasHoyfrmMenu || ctrl == lblComandasPendientesfrmMenu || ctrl == lblValesSinUsarfrmMenu || ctrl == lnkStockBajofrmMenu) continue;
                ctrl.Text = g.Traducir(ctrl.Name);
            }
            foreach (ToolStripMenuItem item in mnstripMenu.Items)
                TraducirMenuItem(item);
            CargarComboIdiomas();
            if (sesion.IsLogged())
            {
                lblEmailTag.Text = $"{g.Traducir("lblEmailTag")}: {sesion.UsuarioActual.CorreoElectronico}";
                lblNombreTag.Text = $"{g.Traducir("lblNombreTag")}: {sesion.UsuarioActual.NombreUsuario} {sesion.UsuarioActual.ApellidoUsuario}";
                // Roles del usuario (familias de permisos asignadas)
                var roles = (sesion.UsuarioActual.PermisosAsignados ?? new List<BE.PERMISOCOMPONENT>())
                    .Where(p => p is BE.PERMISOCOMPOSITE).Select(p => p.NombrePermiso.Replace("Rol - ", string.Empty));
                lblRolesTag.Text = $"{g.Traducir("lblRolesTag")}: {string.Join(", ", roles)}";
                ActualizarTablero();
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
