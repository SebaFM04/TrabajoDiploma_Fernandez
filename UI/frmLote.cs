using BE;
using BLL;
using SERVICIO;
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
    // N02 CU036 Dar de Baja Lote Vencido (decisión 69). También lista los lotes por vencer (tablero del Encargado).
    public partial class frmLote : Form, IObservadorIdioma
    {
        LOTE_BLL GestorLote = new LOTE_BLL();
        // true = se abre mostrando los lotes por vencer (enlace del tablero)
        readonly bool abrirPorVencer;

        public frmLote() : this(false) { }

        public frmLote(bool porVencer)
        {
            InitializeComponent();
            abrirPorVencer = porVencer;
            // Las columnas se crean acá: GUIManager traduce el formulario antes del Load
            Enlazar();
        }

        private void frmLote_Load(object sender, EventArgs e)
        {
            if (abrirPorVencer)
                rdbPorVencerfrmLote.Checked = true;
            CargarLotes();
        }

        private void Enlazar()
        {
            var g = GestorIdioma.Instancia;
            dgvLotesfrmLote.Columns.Clear();
            foreach (string col in new[] { "colLotefrmLote", "colInsumofrmLote", "colIngresofrmLote", "colVencimientofrmLote", "colCantidadfrmLote", "colUnidadfrmLote", "colValorfrmLote" })
                dgvLotesfrmLote.Columns.Add(col, g.Traducir(col));
            AjusteGrilla.Configurar(dgvLotesfrmLote);
        }

        // CU036 paso 1: lotes vencidos con cantidad (o por vencer, solo para consultar)
        private void CargarLotes()
        {
            var g = GestorIdioma.Instancia;
            dgvLotesfrmLote.Rows.Clear();
            try
            {
                var lotes = rdbVencidosfrmLote.Checked
                    ? GestorLote.ListarLotesVencidos()
                    : GestorLote.ListarLotesPorVencer(LOTE_BLL.DiasPorVencerTablero);
                foreach (var l in lotes)
                {
                    int fila = dgvLotesfrmLote.Rows.Add(l.IdLote, l.Insumo.Nombre, l.FechaIngreso.ToString("dd/MM/yyyy"),
                        l.FechaVencimiento?.ToString("dd/MM/yyyy"), l.CantidadDisponible.ToString("#,0.###", CultureInfo.CurrentCulture),
                        l.Insumo.UnidadMedida, FormatoMoneda.Pesos(l.CantidadDisponible * l.CostoUnidadMedida));
                    dgvLotesfrmLote.Rows[fila].Tag = l;
                    if (l.Vencido)
                        dgvLotesfrmLote.Rows[fila].DefaultCellStyle.ForeColor = Color.Firebrick;
                }
                dgvLotesfrmLote.ClearSelection();
                lblTotalfrmLote.Text = string.Format(g.Traducir(rdbVencidosfrmLote.Checked ? "msgLoteTotalVencidos" : "msgLoteTotalPorVencer"),
                    lotes.Count, FormatoMoneda.Pesos(lotes.Sum(l => l.CantidadDisponible * l.CostoUnidadMedida)));
                // Solo se dan de baja los vencidos
                btnDarDeBajafrmLote.Enabled = rdbVencidosfrmLote.Checked && lotes.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(g.Traducir("msgLoteError") + ex.GetBaseException().Message, g.Traducir("frmLote"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Filtro_CheckedChanged(object sender, EventArgs e)
        {
            // Cada cambio dispara el evento en los dos botones: se carga una sola vez
            if (((RadioButton)sender).Checked)
                CargarLotes();
        }

        // CU036 pasos 2 a 4: el Encargado selecciona el lote, confirma y el sistema lo da de baja
        private void btnDarDeBajafrmLote_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            var lote = dgvLotesfrmLote.SelectedRows.Count > 0 ? dgvLotesfrmLote.SelectedRows[0].Tag as BE.LOTE_INSUMO : null;
            if (lote == null)
            {
                MessageBox.Show(g.Traducir("msgLoteSeleccionar"), g.Traducir("frmLote"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var confirmar = MessageBox.Show(string.Format(g.Traducir("msgLoteConfirmarBaja"), lote.IdLote, lote.Insumo.Nombre,
                    lote.CantidadDisponible.ToString("#,0.###", CultureInfo.CurrentCulture), lote.Insumo.UnidadMedida, lote.FechaVencimiento?.ToString("dd/MM/yyyy")),
                g.Traducir("frmLote"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmar != DialogResult.Yes) return;
            try
            {
                GestorLote.DarDeBajaLoteVencido(lote.IdLote);
                MessageBox.Show(g.Traducir("msgLoteBaja"), g.Traducir("frmLote"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (ArgumentException argEx)
            {
                // FA1: el lote no está vencido o ya no tiene cantidad
                MessageBox.Show(g.Traducir(argEx.Message), g.Traducir("frmLote"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(g.Traducir("msgLoteError") + ex.GetBaseException().Message, g.Traducir("frmLote"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            CargarLotes();
        }

        public void ActualizarIdioma()
        {
            var g = GestorIdioma.Instancia;
            this.Text = g.Traducir(this.Name);
            rdbVencidosfrmLote.Text = g.Traducir(rdbVencidosfrmLote.Name);
            rdbPorVencerfrmLote.Text = g.Traducir(rdbPorVencerfrmLote.Name);
            btnDarDeBajafrmLote.Text = g.Traducir(btnDarDeBajafrmLote.Name);
            foreach (DataGridViewColumn col in dgvLotesfrmLote.Columns)
                col.HeaderText = g.Traducir(col.Name);
            if (IsHandleCreated)
                CargarLotes();
        }
    }
}
