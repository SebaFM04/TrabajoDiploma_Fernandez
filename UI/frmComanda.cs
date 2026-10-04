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
    // CU011 Confirmar Entrega de Piqueos (DSS-CU011). Actor: Cocina.
    public partial class frmComanda : Form, IObservadorIdioma
    {
        COMANDA_BLL GestorComanda = new COMANDA_BLL();
        // Evita que la carga de la grilla dispare la selección
        bool cargando = false;

        public frmComanda()
        {
            InitializeComponent();
            // Las columnas se crean acá: GUIManager traduce el formulario antes del Load
            Enlazar();
        }

        private void frmComanda_Load(object sender, EventArgs e)
        {
            CargarComandas(true);
        }

        private void Enlazar()
        {
            var g = GestorIdioma.Instancia;
            dgvComandasfrmComanda.Columns.Clear();
            foreach (string col in new[] { "colNumerofrmComanda", "colVentafrmComanda", "colHorafrmComanda" })
                dgvComandasfrmComanda.Columns.Add(col, g.Traducir(col));
            AjusteGrilla.Configurar(dgvComandasfrmComanda);

            dgvDetallefrmComanda.Columns.Clear();
            foreach (string col in new[] { "colPiqueofrmComanda", "colTamaniofrmComanda", "colCantidadfrmComanda" })
                dgvDetallefrmComanda.Columns.Add(col, g.Traducir(col));
            AjusteGrilla.Configurar(dgvDetallefrmComanda);
        }

        // CU011 paso 1: comandas pendientes. avisarSiVacio: informa si no hay ninguna.
        private void CargarComandas(bool avisarSiVacio)
        {
            var g = GestorIdioma.Instancia;
            cargando = true;
            dgvComandasfrmComanda.Rows.Clear();
            dgvDetallefrmComanda.Rows.Clear();
            List<BE.COMANDA> comandas = new List<BE.COMANDA>();
            try
            {
                comandas = GestorComanda.ListarPendientes();
                foreach (var c in comandas)
                {
                    int fila = dgvComandasfrmComanda.Rows.Add(c.IdComanda, c.IdVenta, c.FechaHoraEmision.ToString("dd/MM HH:mm", CultureInfo.CurrentCulture));
                    dgvComandasfrmComanda.Rows[fila].Tag = c;
                }
                dgvComandasfrmComanda.ClearSelection();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
            finally
            {
                cargando = false;
            }
            btnConfirmarEntregafrmComanda.Enabled = false;
            if (avisarSiVacio && comandas.Count == 0)
                MessageBox.Show(g.Traducir("msgComandaSinPendientes"), g.Traducir("frmComanda"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private BE.COMANDA ComandaSeleccionada()
        {
            if (dgvComandasfrmComanda.SelectedRows.Count == 0) return null;
            return dgvComandasfrmComanda.SelectedRows[0].Tag as BE.COMANDA;
        }

        // CU011 pasos 1 y 2: Cocina selecciona la comanda y el sistema muestra los piqueos y cantidades
        private void dgvComandasfrmComanda_SelectionChanged(object sender, EventArgs e)
        {
            if (cargando) return;
            var seleccionada = ComandaSeleccionada();
            dgvDetallefrmComanda.Rows.Clear();
            btnConfirmarEntregafrmComanda.Enabled = seleccionada != null;
            if (seleccionada == null) return;
            try
            {
                var comanda = GestorComanda.ObtenerDetalle(seleccionada.IdComanda);
                foreach (var d in comanda.Detalles)
                    dgvDetallefrmComanda.Rows.Add(d.ProductoTamanio.Producto.Nombre, d.ProductoTamanio.Tamanio.Nombre, d.Cantidad);
            }
            catch (ArgumentException argEx)
            {
                Avisar(argEx.Message);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        // CU011 pasos 3 y 4. FA1: la comanda ya fue confirmada.
        private void btnConfirmarEntregafrmComanda_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            var seleccionada = ComandaSeleccionada();
            if (seleccionada == null)
            {
                Avisar("msgComandaSeleccionar");
                return;
            }
            if (MessageBox.Show(string.Format(g.Traducir("msgComandaConfirmar"), seleccionada.IdComanda), g.Traducir("frmComanda"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {
                GestorComanda.ConfirmarEntrega(seleccionada.IdComanda);
                MessageBox.Show(string.Format(g.Traducir("msgComandaEntregada"), seleccionada.IdComanda), g.Traducir("frmComanda"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (ArgumentException argEx)
            {
                Avisar(argEx.Message);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
            CargarComandas(false);
        }

        private void btnActualizarfrmComanda_Click(object sender, EventArgs e)
        {
            CargarComandas(true);
        }

        private void Avisar(string clave)
        {
            var g = GestorIdioma.Instancia;
            MessageBox.Show(g.Traducir(clave), g.Traducir("msgComandaAviso"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void MostrarError(Exception ex)
        {
            var g = GestorIdioma.Instancia;
            MessageBox.Show(g.Traducir("msgComandaError") + ex.GetBaseException().Message, g.Traducir("frmComanda"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ActualizarIdioma()
        {
            var g = GestorIdioma.Instancia;
            this.Text = g.Traducir(this.Name);
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is DataGridView)
                    continue;
                ctrl.Text = g.Traducir(ctrl.Name);
            }
            foreach (DataGridViewColumn col in dgvComandasfrmComanda.Columns)
                col.HeaderText = g.Traducir(col.Name);
            foreach (DataGridViewColumn col in dgvDetallefrmComanda.Columns)
                col.HeaderText = g.Traducir(col.Name);
        }
    }
}
