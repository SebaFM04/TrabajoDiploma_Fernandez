using BE;
using BLL;
using SERVICIO.MultiIdioma_Observer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    // CU012 Confirmar Entrega de Bebidas (DSS-CU012 vigente, decisión 44). Actor: Bartender.
    public partial class frmEntrega : Form, IObservadorIdioma
    {
        VALE_BLL GestorVale = new VALE_BLL();
        RECETA_BLL GestorReceta = new RECETA_BLL();
        // Vale validado que se está por entregar
        BE.VALE vale = null;

        public frmEntrega()
        {
            InitializeComponent();
            // Las columnas se crean acá: GUIManager traduce el formulario antes del Load
            Enlazar();
        }

        private void frmEntrega_Load(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void Enlazar()
        {
            var g = GestorIdioma.Instancia;
            dgvBebidasfrmEntrega.Columns.Clear();
            foreach (string col in new[] { "colBebidafrmEntrega", "colTamaniofrmEntrega", "colCantidadfrmEntrega" })
                dgvBebidasfrmEntrega.Columns.Add(col, g.Traducir(col));
            AjusteGrilla.Configurar(dgvBebidasfrmEntrega);

            dgvRecetasfrmEntrega.Columns.Clear();
            dgvRecetasfrmEntrega.Columns.Add("colBebidaRecetafrmEntrega", g.Traducir("colBebidafrmEntrega"));
            foreach (string col in new[] { "colInsumofrmEntrega", "colConsumofrmEntrega", "colUnidadfrmEntrega" })
                dgvRecetasfrmEntrega.Columns.Add(col, g.Traducir(col));
            AjusteGrilla.Configurar(dgvRecetasfrmEntrega);
        }

        private void Limpiar()
        {
            vale = null;
            txtValefrmEntrega.Text = string.Empty;
            dgvBebidasfrmEntrega.Rows.Clear();
            dgvRecetasfrmEntrega.Rows.Clear();
            btnConfirmarEntregafrmEntrega.Enabled = false;
            txtValefrmEntrega.Enabled = btnBuscarfrmEntrega.Enabled = true;
            txtValefrmEntrega.Focus();
        }

        // CU012 pasos 1 a 3: valida el vale, muestra las bebidas y la receta escalada de cada trago
        private void btnBuscarfrmEntrega_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            if (!int.TryParse(txtValefrmEntrega.Text.Trim(), out int idVale) || idVale <= 0)
            {
                Avisar("msgValeIngresar");
                return;
            }
            dgvBebidasfrmEntrega.Rows.Clear();
            dgvRecetasfrmEntrega.Rows.Clear();
            btnConfirmarEntregafrmEntrega.Enabled = false;
            try
            {
                vale = GestorVale.ValidarVale(idVale);
                if (vale == null)
                {
                    // FA1: no existe o ya fue utilizado; se cancela la operación
                    MessageBox.Show(g.Traducir("msgValeInvalido"), g.Traducir("frmEntrega"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Limpiar();
                    return;
                }
                if (vale.Detalles.Count == 0)
                {
                    // FA2 (decisión 13): venta solo de piqueos; no se marca el vale
                    MessageBox.Show(g.Traducir("msgValeSinBebidas"), g.Traducir("frmEntrega"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Limpiar();
                    return;
                }
                foreach (var d in vale.Detalles)
                {
                    var pt = d.ProductoTamanio;
                    dgvBebidasfrmEntrega.Rows.Add(pt.Producto.Nombre, pt.Tamanio.Nombre, d.Cantidad);
                    // Paso 3 (mensajes 10-15): receta escalada al tamaño y la cantidad vendidos
                    foreach (var r in GestorReceta.ObtenerRecetaEscalada(d.IdProducto, pt.Tamanio.CantidadMagnitud, d.Cantidad))
                        dgvRecetasfrmEntrega.Rows.Add(pt.Producto.Nombre + " (" + pt.Tamanio.Nombre + " x" + d.Cantidad + ")",
                            r.Insumo.Nombre, r.Proporcion.ToString("#,0.###", CultureInfo.CurrentCulture), r.Insumo.UnidadMedida);
                }
                txtValefrmEntrega.Enabled = btnBuscarfrmEntrega.Enabled = false;
                btnConfirmarEntregafrmEntrega.Enabled = true;
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void txtValefrmEntrega_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) btnBuscarfrmEntrega_Click(sender, e);
        }

        // CU012 pasos 4 y 5: el Bartender entregó los tragos y confirma
        private void btnConfirmarEntregafrmEntrega_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            if (vale == null) return;
            try
            {
                GestorVale.MarcarUtilizado(vale.IdVale);
                MessageBox.Show(string.Format(g.Traducir("msgValeEntregado"), vale.IdVale), g.Traducir("frmEntrega"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (ArgumentException argEx)
            {
                Avisar(argEx.Message);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
                return;
            }
            Limpiar();
        }

        private void btnLimpiarfrmEntrega_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void Avisar(string clave)
        {
            var g = GestorIdioma.Instancia;
            MessageBox.Show(g.Traducir(clave), g.Traducir("msgValeAviso"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void MostrarError(Exception ex)
        {
            var g = GestorIdioma.Instancia;
            MessageBox.Show(g.Traducir("msgValeError") + ex.GetBaseException().Message, g.Traducir("frmEntrega"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ActualizarIdioma()
        {
            var g = GestorIdioma.Instancia;
            this.Text = g.Traducir(this.Name);
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is TextBox || ctrl is DataGridView)
                    continue;
                ctrl.Text = g.Traducir(ctrl.Name);
            }
            foreach (DataGridViewColumn col in dgvBebidasfrmEntrega.Columns)
                col.HeaderText = g.Traducir(col.Name);
            foreach (DataGridViewColumn col in dgvRecetasfrmEntrega.Columns)
                col.HeaderText = g.Traducir(col.Name == "colBebidaRecetafrmEntrega" ? "colBebidafrmEntrega" : col.Name);
        }
    }
}
