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
    // Tablero (decisión 56): insumos con stock bajo, solo lectura. La reposición es N02.
    public partial class frmStockBajo : Form, IObservadorIdioma
    {
        INSUMO_BLL GestorInsumo = new INSUMO_BLL();

        public frmStockBajo()
        {
            InitializeComponent();
            // Las columnas se crean acá: GUIManager traduce el formulario antes del Load
            Enlazar();
        }

        private void frmStockBajo_Load(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            dgvInsumosfrmStockBajo.Rows.Clear();
            try
            {
                foreach (var i in GestorInsumo.ListarStockBajo())
                {
                    int fila = dgvInsumosfrmStockBajo.Rows.Add(i.Nombre,
                        i.VolumenPesoDisponible.ToString("#,0.###", CultureInfo.CurrentCulture),
                        i.UmbralReposicion.ToString("#,0.###", CultureInfo.CurrentCulture),
                        i.UnidadMedida,
                        g.Traducir(i.AvisoStockBajo ? "msgConsultaSi" : "msgConsultaNo"));
                    if (i.VolumenPesoDisponible < i.UmbralReposicion)
                        dgvInsumosfrmStockBajo.Rows[fila].DefaultCellStyle.ForeColor = Color.Firebrick;
                }
                dgvInsumosfrmStockBajo.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(g.Traducir("msgConsultaError") + ex.GetBaseException().Message, g.Traducir("frmStockBajo"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Enlazar()
        {
            var g = GestorIdioma.Instancia;
            dgvInsumosfrmStockBajo.Columns.Clear();
            foreach (string col in new[] { "colInsumofrmStockBajo", "colStockfrmStockBajo", "colUmbralfrmStockBajo", "colUnidadfrmStockBajo", "colAvisofrmStockBajo" })
                dgvInsumosfrmStockBajo.Columns.Add(col, g.Traducir(col));
            AjusteGrilla.Configurar(dgvInsumosfrmStockBajo);
        }

        public void ActualizarIdioma()
        {
            var g = GestorIdioma.Instancia;
            this.Text = g.Traducir(this.Name);
            lblAyudafrmStockBajo.Text = g.Traducir(lblAyudafrmStockBajo.Name);
            foreach (DataGridViewColumn col in dgvInsumosfrmStockBajo.Columns)
                col.HeaderText = g.Traducir(col.Name);
        }
    }
}
