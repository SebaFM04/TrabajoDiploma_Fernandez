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
using System.Windows.Forms.DataVisualization.Charting;

namespace UI
{
    // N03 CU034 Evaluar Resultado de Promoción (decisión 81): compara la vigencia con el período anterior de igual
    // duración, sugiere el resultado y el Dueño lo confirma. La lógica está en PROMOCION_BLL.
    public partial class frmEvaluacionPromocion : Form, IObservadorIdioma
    {
        PROMOCION_BLL GestorPromocion = new PROMOCION_BLL();
        // Evaluación que se está mostrando (null = ninguna)
        BE.EVALUACION_PROMOCION evaluacion = null;
        bool cargando = false;

        public frmEvaluacionPromocion()
        {
            InitializeComponent();
            // Las columnas se crean acá: GUIManager traduce el formulario antes del Load
            Enlazar();
        }

        private void frmEvaluacionPromocion_Load(object sender, EventArgs e)
        {
            try
            {
                // Decisión 78: estados por fecha al abrir el formulario de promociones
                GestorPromocion.ActualizarEstados();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
            CargarPromociones();
            MostrarEvaluacion();
        }

        private void Enlazar()
        {
            var g = GestorIdioma.Instancia;
            dgvPromocionesfrmEvaluacionPromocion.Columns.Clear();
            foreach (string col in new[] { "colNombrefrmEvaluacionPromocion", "colDesdefrmEvaluacionPromocion", "colHastafrmEvaluacionPromocion", "colEstadofrmEvaluacionPromocion", "colResultadofrmEvaluacionPromocion" })
                dgvPromocionesfrmEvaluacionPromocion.Columns.Add(col, g.Traducir(col));
            AjusteGrilla.Configurar(dgvPromocionesfrmEvaluacionPromocion);

            dgvComparacionfrmEvaluacionPromocion.Columns.Clear();
            foreach (string col in new[] { "colProductofrmEvaluacionPromocion", "colTamaniofrmEvaluacionPromocion",
                                           "colUnidadesAntesfrmEvaluacionPromocion", "colUnidadesDurantefrmEvaluacionPromocion", "colVariacionUnidadesfrmEvaluacionPromocion",
                                           "colIngresosAntesfrmEvaluacionPromocion", "colIngresosDurantefrmEvaluacionPromocion", "colVariacionIngresosfrmEvaluacionPromocion",
                                           "colMargenAntesfrmEvaluacionPromocion", "colMargenDurantefrmEvaluacionPromocion", "colVariacionMargenfrmEvaluacionPromocion" })
            {
                int i = dgvComparacionfrmEvaluacionPromocion.Columns.Add(col, g.Traducir(col));
                if (i >= 2)
                    dgvComparacionfrmEvaluacionPromocion.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            AjusteGrilla.Configurar(dgvComparacionfrmEvaluacionPromocion);
        }

        private static string TextoEstado(string estado)
        {
            return GestorIdioma.Instancia.Traducir("msgPromocionEstado" + estado.Replace(" ", ""));
        }

        private static string TextoResultado(string resultado)
        {
            return resultado == null ? "-" : GestorIdioma.Instancia.Traducir("msgPromocionResultado" + resultado.Replace(" ", ""));
        }

        // CU034 paso 1: promociones finalizadas sin evaluar; con "Mostrar todas", también las demás
        private void CargarPromociones()
        {
            cargando = true;
            dgvPromocionesfrmEvaluacionPromocion.Rows.Clear();
            try
            {
                var promociones = chkTodasfrmEvaluacionPromocion.Checked ? GestorPromocion.ListarPromociones(null) : GestorPromocion.ListarParaEvaluar();
                foreach (var p in promociones)
                {
                    int fila = dgvPromocionesfrmEvaluacionPromocion.Rows.Add(p.Nombre, p.FechaDesde.ToString("dd/MM/yyyy"), p.FechaHasta.ToString("dd/MM/yyyy"),
                        TextoEstado(p.Estado), TextoResultado(p.Resultado));
                    dgvPromocionesfrmEvaluacionPromocion.Rows[fila].Tag = p;
                }
                dgvPromocionesfrmEvaluacionPromocion.ClearSelection();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
            finally
            {
                cargando = false;
            }
        }

        private void chkTodasfrmEvaluacionPromocion_CheckedChanged(object sender, EventArgs e)
        {
            evaluacion = null;
            CargarPromociones();
            MostrarEvaluacion();
        }

        // CU034 pasos 2 y 3: al elegir una promoción, el sistema muestra la comparación y sugiere el resultado
        private void dgvPromocionesfrmEvaluacionPromocion_SelectionChanged(object sender, EventArgs e)
        {
            if (cargando) return;
            var g = GestorIdioma.Instancia;
            var promocion = dgvPromocionesfrmEvaluacionPromocion.SelectedRows.Count > 0 ? dgvPromocionesfrmEvaluacionPromocion.SelectedRows[0].Tag as BE.PROMOCION : null;
            evaluacion = null;
            MostrarEvaluacion();
            if (promocion == null) return;
            try
            {
                evaluacion = GestorPromocion.EvaluarPromocion(promocion.IdPromocion);
                MostrarEvaluacion();
            }
            catch (ArgumentException argEx)
            {
                // FA1: la promoción todavía no terminó
                lblPromocionfrmEvaluacionPromocion.Text = promocion.Nombre + ": " + g.Traducir(argEx.Message);
                lblPromocionfrmEvaluacionPromocion.ForeColor = Color.Firebrick;
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void MostrarEvaluacion()
        {
            var g = GestorIdioma.Instancia;
            dgvComparacionfrmEvaluacionPromocion.Rows.Clear();
            pnlGraficofrmEvaluacionPromocion.Controls.Clear();
            lblPromocionfrmEvaluacionPromocion.ForeColor = SystemColors.ControlText;
            lblSugerenciafrmEvaluacionPromocion.Text = string.Empty;
            txtObservacionesfrmEvaluacionPromocion.Text = string.Empty;
            rdbEfectivafrmEvaluacionPromocion.Checked = rdbNoEfectivafrmEvaluacionPromocion.Checked = false;
            if (evaluacion == null)
            {
                lblPromocionfrmEvaluacionPromocion.Text = g.Traducir("msgEvaluacionElegir");
                HabilitarResultado(false);
                btnRenovarfrmEvaluacionPromocion.Enabled = false;
                return;
            }

            var p = evaluacion.Promocion;
            lblPromocionfrmEvaluacionPromocion.Text = string.Format(g.Traducir("msgEvaluacionPeriodos"), p.Nombre,
                evaluacion.AntesDesde.ToString("dd/MM/yyyy"), evaluacion.AntesHasta.ToString("dd/MM/yyyy"),
                p.FechaDesde.ToString("dd/MM/yyyy"), p.FechaHasta.ToString("dd/MM/yyyy"));
            foreach (var i in evaluacion.Items)
                AgregarFila(i, i.NombreProducto, i.NombreTamanio);
            int total = AgregarFila(evaluacion.Total, g.Traducir("msgEvaluacionTotal"), string.Empty);
            dgvComparacionfrmEvaluacionPromocion.Rows[total].DefaultCellStyle.Font = new Font(dgvComparacionfrmEvaluacionPromocion.Font, FontStyle.Bold);
            dgvComparacionfrmEvaluacionPromocion.ClearSelection();

            var textos = new List<string> { string.Format(g.Traducir("msgEvaluacionSugerido"), TextoResultado(evaluacion.ResultadoSugerido)) };
            textos.AddRange(evaluacion.Avisos.Select(a => g.Traducir(a)));
            lblSugerenciafrmEvaluacionPromocion.Text = string.Join("  ", textos);
            pnlGraficofrmEvaluacionPromocion.Controls.Add(CrearGrafico());

            // Ya evaluada: se ve el resultado guardado y no se puede cambiar
            bool evaluada = p.Resultado != null;
            string resultado = evaluada ? p.Resultado : evaluacion.ResultadoSugerido;
            rdbEfectivafrmEvaluacionPromocion.Checked = resultado == PROMOCION_BLL.ResultadoEfectiva;
            rdbNoEfectivafrmEvaluacionPromocion.Checked = resultado == PROMOCION_BLL.ResultadoNoEfectiva;
            txtObservacionesfrmEvaluacionPromocion.Text = evaluada ? p.Observaciones : string.Empty;
            HabilitarResultado(!evaluada);
            // CU035 «extend» CU034 (propuesta 4 de la Fase 6): se renueva una promoción finalizada y ya evaluada
            btnRenovarfrmEvaluacionPromocion.Enabled = evaluada && p.Estado == PROMOCION_BLL.EstadoFinalizada;
        }

        // CU035 Renovar Promoción: abre Promociones en modo renovación con los datos propuestos
        private void btnRenovarfrmEvaluacionPromocion_Click(object sender, EventArgs e)
        {
            if (evaluacion == null) return;
            var g = GestorIdioma.Instancia;
            int id = evaluacion.Promocion.IdPromocion;
            try
            {
                var renovacion = GestorPromocion.ArmarRenovacion(id);
                GUIManager.Instancia.AbrirForm(new frmPromocion(renovacion));
                CargarPromociones();
                evaluacion = GestorPromocion.EvaluarPromocion(id);
                MostrarEvaluacion();
            }
            catch (ArgumentException argEx)
            {
                MessageBox.Show(g.Traducir(argEx.Message), g.Traducir("frmEvaluacionPromocion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void HabilitarResultado(bool habilitar)
        {
            rdbEfectivafrmEvaluacionPromocion.Enabled = rdbNoEfectivafrmEvaluacionPromocion.Enabled = txtObservacionesfrmEvaluacionPromocion.Enabled
                = btnConfirmarfrmEvaluacionPromocion.Enabled = habilitar;
        }

        private int AgregarFila(BE.EVALUACION_ITEM i, string producto, string tamanio)
        {
            return dgvComparacionfrmEvaluacionPromocion.Rows.Add(producto, tamanio,
                i.UnidadesAntes.ToString("#,##0"), i.UnidadesDurante.ToString("#,##0"), TextoVariacion(i.VariacionUnidades),
                FormatoMoneda.Pesos(i.IngresosAntes), FormatoMoneda.Pesos(i.IngresosDurante), TextoVariacion(i.VariacionIngresos),
                FormatoMoneda.Pesos(i.MargenAntes), FormatoMoneda.Pesos(i.MargenDurante), TextoVariacion(i.VariacionMargen));
        }

        // Propuesta 8 de la Fase 6: sin base (antes = 0) se muestra "–"
        private static string TextoVariacion(decimal? variacion)
        {
            return variacion.HasValue ? variacion.Value.ToString("+0.0;-0.0;0.0") + " %" : "–";
        }

        // Propuesta 7 de la Fase 6: unidades por día de los dos períodos, con la vigencia marcada como franja
        private Chart CrearGrafico()
        {
            var g = GestorIdioma.Instancia;
            var chart = new Chart { Dock = DockStyle.Fill, BackColor = Color.NavajoWhite };
            var area = new ChartArea("area") { BackColor = Color.White };
            area.AxisX.MajorGrid.LineColor = area.AxisY.MajorGrid.LineColor = Color.Gainsboro;
            area.AxisX.LabelStyle.Font = area.AxisY.LabelStyle.Font = new Font("Segoe UI", 8F);
            chart.ChartAreas.Add(area);
            chart.Legends.Add(new Legend("leyenda") { Docking = Docking.Bottom, BackColor = Color.NavajoWhite });
            var serie = new Series(g.Traducir(evaluacion.Serie.Nombre)) { ChartType = SeriesChartType.Line, BorderWidth = 2, MarkerStyle = MarkerStyle.Circle, ChartArea = "area", Legend = "leyenda" };
            foreach (var punto in evaluacion.Serie.Puntos)
            {
                DateTime dia = DateTime.ParseExact(punto.Etiqueta, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                serie.Points.AddXY(dia.ToString("dd/MM"), (double)punto.Valor);
            }
            chart.Series.Add(serie);
            // Los puntos de la vigencia van después de los del período anterior (misma cantidad de días)
            int dias = (evaluacion.AntesHasta - evaluacion.AntesDesde).Days + 1;
            area.AxisX.StripLines.Add(new StripLine
            {
                IntervalOffset = dias + 0.5,
                StripWidth = dias,
                BackColor = Color.FromArgb(70, Color.Orange),
                Text = g.Traducir("msgEvaluacionVigencia"),
                TextAlignment = StringAlignment.Near,
                TextLineAlignment = StringAlignment.Near
            });
            return chart;
        }

        // CU034 pasos 4 y 5: el Dueño confirma el resultado
        private void btnConfirmarfrmEvaluacionPromocion_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            if (evaluacion == null) return;
            string resultado = rdbEfectivafrmEvaluacionPromocion.Checked ? PROMOCION_BLL.ResultadoEfectiva
                             : rdbNoEfectivafrmEvaluacionPromocion.Checked ? PROMOCION_BLL.ResultadoNoEfectiva : null;
            try
            {
                GestorPromocion.RegistrarEvaluacion(evaluacion.Promocion.IdPromocion, resultado, txtObservacionesfrmEvaluacionPromocion.Text);
                MessageBox.Show(string.Format(g.Traducir("msgEvaluacionRegistrada"), evaluacion.Promocion.Nombre, TextoResultado(resultado)),
                    g.Traducir("frmEvaluacionPromocion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                int id = evaluacion.Promocion.IdPromocion;
                CargarPromociones();
                evaluacion = GestorPromocion.EvaluarPromocion(id);
                MostrarEvaluacion();
            }
            catch (ArgumentException argEx)
            {
                MessageBox.Show(g.Traducir(argEx.Message), g.Traducir("frmEvaluacionPromocion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void MostrarError(Exception ex)
        {
            var g = GestorIdioma.Instancia;
            MessageBox.Show(g.Traducir("msgPromocionError") + ex.GetBaseException().Message, g.Traducir("frmEvaluacionPromocion"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ActualizarIdioma()
        {
            var g = GestorIdioma.Instancia;
            this.Text = g.Traducir(this.Name);
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is DataGridView || ctrl is TextBox || ctrl is Panel
                    || ctrl == lblPromocionfrmEvaluacionPromocion || ctrl == lblSugerenciafrmEvaluacionPromocion)
                    continue;
                ctrl.Text = g.Traducir(ctrl.Name);
            }
            foreach (var dgv in new[] { dgvPromocionesfrmEvaluacionPromocion, dgvComparacionfrmEvaluacionPromocion })
                foreach (DataGridViewColumn col in dgv.Columns)
                    col.HeaderText = g.Traducir(col.Name);
            if (IsHandleCreated)
            {
                CargarPromociones();
                MostrarEvaluacion();
            }
        }
    }
}
