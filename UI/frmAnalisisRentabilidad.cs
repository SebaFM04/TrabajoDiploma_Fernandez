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
    // N03 CU031 Generar Análisis de Rentabilidad (decisiones 74 a 77 y 83). La lógica está en ANALISIS_BLL:
    // el formulario pide el período y el criterio, y muestra la tabla, los sugeridos, los avisos y los gráficos.
    public partial class frmAnalisisRentabilidad : Form, IObservadorIdioma
    {
        ANALISIS_BLL GestorAnalisis = new ANALISIS_BLL();
        // Análisis que se está mostrando (null = todavía no se generó ninguno)
        BE.ANALISIS_RENTABILIDAD analisis = null;
        // Criterios en el orden del combo y sus claves de idioma
        readonly string[] criterios = { PROMOCION_BLL.CriterioVentas, PROMOCION_BLL.CriterioRotacion, PROMOCION_BLL.CriterioVencimiento, PROMOCION_BLL.CriterioEstacionalidad };
        readonly string[] clavesCriterio = { "msgCriterioVentas", "msgCriterioRotacion", "msgCriterioVencimiento", "msgCriterioEstacionalidad" };

        // Formatos de las celdas numéricas: los valores se guardan como números para que la grilla ordene bien
        const string FormatoPesos = "$ #,##0.00";
        const string FormatoPorcentaje = "0.0' %'";
        const string FormatoNumero = "#,##0.##";

        public frmAnalisisRentabilidad()
        {
            InitializeComponent();
            // Las columnas se crean acá: GUIManager traduce el formulario antes del Load
            Enlazar();
        }

        private void frmAnalisisRentabilidad_Load(object sender, EventArgs e)
        {
            dtpHastafrmAnalisisRentabilidad.Value = DateTime.Today;
            dtpDesdefrmAnalisisRentabilidad.Value = DateTime.Today.AddDays(-30);
            cmbDiasfrmAnalisisRentabilidad.Items.Clear();
            foreach (int dias in GestorAnalisis.ListarDiasPorVencer())
                cmbDiasfrmAnalisisRentabilidad.Items.Add(dias);
            cmbDiasfrmAnalisisRentabilidad.SelectedItem = ANALISIS_BLL.DiasPorVencerDefecto;
            CargarCriterios();
            MostrarAnalisis();
        }

        private void Enlazar()
        {
            var g = GestorIdioma.Instancia;
            dgvResultadosfrmAnalisisRentabilidad.Columns.Clear();
            AgregarColumna("colProductofrmAnalisisRentabilidad", null);
            AgregarColumna("colTamaniofrmAnalisisRentabilidad", null);
            AgregarColumna("colTipofrmAnalisisRentabilidad", null);
            AgregarColumna("colUnidadesfrmAnalisisRentabilidad", "#,##0");
            AgregarColumna("colIngresosfrmAnalisisRentabilidad", FormatoPesos);
            AgregarColumna("colCostofrmAnalisisRentabilidad", FormatoPesos);
            AgregarColumna("colMargenTotalfrmAnalisisRentabilidad", FormatoPesos);
            AgregarColumna("colMargenUnitariofrmAnalisisRentabilidad", FormatoPesos);
            AgregarColumna("colMargenPorcentajefrmAnalisisRentabilidad", FormatoPorcentaje);
            AgregarColumna("colParticipacionfrmAnalisisRentabilidad", FormatoPorcentaje);
            AgregarColumna("colPrecioActualfrmAnalisisRentabilidad", FormatoPesos);
            AgregarColumna("colCostoActualfrmAnalisisRentabilidad", FormatoPesos);
            AgregarColumna("colMargenActualfrmAnalisisRentabilidad", FormatoPesos);
            AgregarColumna("colClasificacionfrmAnalisisRentabilidad", null);
            AgregarColumna("colIndicadorfrmAnalisisRentabilidad", FormatoNumero);
            AjusteGrilla.Configurar(dgvResultadosfrmAnalisisRentabilidad);

            dgvSugeridosfrmAnalisisRentabilidad.Columns.Clear();
            foreach (string col in new[] { "colOrdenfrmAnalisisRentabilidad", "colProductofrmAnalisisRentabilidad", "colTamaniofrmAnalisisRentabilidad", "colMotivofrmAnalisisRentabilidad" })
                dgvSugeridosfrmAnalisisRentabilidad.Columns.Add(col, g.Traducir(col));
            dgvSugeridosfrmAnalisisRentabilidad.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colMargenActualSugeridofrmAnalisisRentabilidad",
                HeaderText = g.Traducir("colMargenActualfrmAnalisisRentabilidad"),
                ValueType = typeof(decimal),
                DefaultCellStyle = { Format = FormatoPesos, Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            AjusteGrilla.Configurar(dgvSugeridosfrmAnalisisRentabilidad);
        }

        // Columna de la grilla de resultados; con formato = numérica (ordena por valor, alineada a la derecha)
        private void AgregarColumna(string nombre, string formato)
        {
            var col = new DataGridViewTextBoxColumn { Name = nombre, HeaderText = GestorIdioma.Instancia.Traducir(nombre), SortMode = DataGridViewColumnSortMode.Automatic };
            if (formato != null)
            {
                col.ValueType = formato == "#,##0" ? typeof(int) : typeof(decimal);
                col.DefaultCellStyle.Format = formato;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            dgvResultadosfrmAnalisisRentabilidad.Columns.Add(col);
        }

        private void CargarCriterios()
        {
            var g = GestorIdioma.Instancia;
            int seleccionado = Math.Max(0, cmbCriteriofrmAnalisisRentabilidad.SelectedIndex);
            cmbCriteriofrmAnalisisRentabilidad.SelectedIndexChanged -= cmbCriteriofrmAnalisisRentabilidad_SelectedIndexChanged;
            cmbCriteriofrmAnalisisRentabilidad.Items.Clear();
            foreach (string clave in clavesCriterio)
                cmbCriteriofrmAnalisisRentabilidad.Items.Add(g.Traducir(clave));
            cmbCriteriofrmAnalisisRentabilidad.SelectedIndex = seleccionado;
            cmbCriteriofrmAnalisisRentabilidad.SelectedIndexChanged += cmbCriteriofrmAnalisisRentabilidad_SelectedIndexChanged;
            MostrarDias();
        }

        // Decisión 76: los días por vencer solo cuentan con el criterio Vencimiento
        private void MostrarDias()
        {
            bool vencimiento = criterios[Math.Max(0, cmbCriteriofrmAnalisisRentabilidad.SelectedIndex)] == PROMOCION_BLL.CriterioVencimiento;
            lblDiasfrmAnalisisRentabilidad.Visible = cmbDiasfrmAnalisisRentabilidad.Visible = vencimiento;
        }

        private void cmbCriteriofrmAnalisisRentabilidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            MostrarDias();
        }

        // CU031 pasos 1 a 4: el Dueño elige período y criterio, el sistema genera el análisis y lo muestra
        private void btnGenerarfrmAnalisisRentabilidad_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            Cursor = Cursors.WaitCursor;
            try
            {
                analisis = GestorAnalisis.GenerarAnalisis(criterios[cmbCriteriofrmAnalisisRentabilidad.SelectedIndex],
                    dtpDesdefrmAnalisisRentabilidad.Value.Date, dtpHastafrmAnalisisRentabilidad.Value.Date,
                    cmbDiasfrmAnalisisRentabilidad.SelectedItem is int dias ? dias : ANALISIS_BLL.DiasPorVencerDefecto);
                MostrarAnalisis();
                Cursor = Cursors.Default;
            }
            catch (ArgumentException argEx)
            {
                // FA1 período inválido / FA2 sin ventas en el período
                Cursor = Cursors.Default;
                MessageBox.Show(g.Traducir(argEx.Message), g.Traducir("frmAnalisisRentabilidad"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(g.Traducir("msgAnalisisError") + ex.GetBaseException().Message, g.Traducir("frmAnalisisRentabilidad"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Muestra el análisis actual: resumen, tabla, sugeridos, avisos y gráficos. Sirve también para un análisis abierto
        // desde XML (Fase 7), porque usa solo lo que trae el BE.
        private void MostrarAnalisis()
        {
            var g = GestorIdioma.Instancia;
            dgvResultadosfrmAnalisisRentabilidad.Rows.Clear();
            dgvSugeridosfrmAnalisisRentabilidad.Rows.Clear();
            tabGraficosfrmAnalisisRentabilidad.TabPages.Clear();
            lblAvisosfrmAnalisisRentabilidad.Text = string.Empty;
            if (analisis == null)
            {
                lblResumenfrmAnalisisRentabilidad.Text = g.Traducir("msgAnalisisSinGenerar");
                return;
            }

            int indice = Array.IndexOf(criterios, analisis.Criterio);
            string criterio = indice >= 0 ? g.Traducir(clavesCriterio[indice]) : analisis.Criterio;
            lblResumenfrmAnalisisRentabilidad.Text = string.Format(g.Traducir("msgAnalisisResumen"), criterio,
                analisis.FechaDesde.ToString("dd/MM/yyyy"), analisis.FechaHasta.ToString("dd/MM/yyyy"), analisis.TotalUnidades,
                FormatoMoneda.Pesos(analisis.TotalIngresos), FormatoMoneda.Pesos(analisis.TotalCosto), FormatoMoneda.Pesos(analisis.TotalMargen),
                analisis.FechaGeneracion.ToString("dd/MM/yyyy HH:mm"), analisis.Usuario)
                + (analisis.DiasPorVencer > 0 ? " " + string.Format(g.Traducir("msgAnalisisResumenDias"), analisis.DiasPorVencer) : "");
            dgvResultadosfrmAnalisisRentabilidad.Columns["colIndicadorfrmAnalisisRentabilidad"].HeaderText = g.Traducir("colIndicador" + analisis.Criterio.Replace("ó", "o") + "frmAnalisisRentabilidad");

            foreach (var i in analisis.Items.OrderByDescending(x => x.MargenTotal))
            {
                int fila = dgvResultadosfrmAnalisisRentabilidad.Rows.Add(i.NombreProducto, i.NombreTamanio, i.TipoProducto, i.Unidades,
                    i.Ingresos, i.Costo, i.MargenTotal, i.MargenUnitario, i.MargenPorcentaje, i.Participacion,
                    i.PrecioActual, i.CostoUnitarioActual, i.MargenUnitarioActual, g.Traducir(i.Clasificacion), i.Indicador);
                dgvResultadosfrmAnalisisRentabilidad.Rows[fila].Tag = i;
                // Los sugeridos se resaltan; los márgenes negativos en rojo
                if (i.Sugerido)
                    dgvResultadosfrmAnalisisRentabilidad.Rows[fila].DefaultCellStyle.BackColor = Color.Honeydew;
                if (i.MargenTotal < 0)
                    dgvResultadosfrmAnalisisRentabilidad.Rows[fila].Cells["colMargenTotalfrmAnalisisRentabilidad"].Style.ForeColor = Color.Firebrick;
            }
            dgvResultadosfrmAnalisisRentabilidad.ClearSelection();

            foreach (var i in analisis.ObtenerSugeridos())
            {
                int fila = dgvSugeridosfrmAnalisisRentabilidad.Rows.Add(i.Orden, i.NombreProducto, i.NombreTamanio, TextoMotivo(i), i.MargenUnitarioActual);
                dgvSugeridosfrmAnalisisRentabilidad.Rows[fila].Tag = i;
            }
            dgvSugeridosfrmAnalisisRentabilidad.ClearSelection();

            lblAvisosfrmAnalisisRentabilidad.Text = string.Join(Environment.NewLine,
                analisis.Avisos.Select(a => string.Format(g.Traducir(a), analisis.MesesHistoria, analisis.DiasPorVencer)));
            ArmarGraficos();
        }

        // Motivo de la sugerencia: la clave de idioma lleva los formatos de sus valores ({0} insumo, {1} y {2} números)
        private string TextoMotivo(BE.ANALISIS_ITEM i)
        {
            if (string.IsNullOrEmpty(i.Motivo)) return string.Empty;
            return string.Format(CultureInfo.CurrentCulture, GestorIdioma.Instancia.Traducir(i.Motivo), i.MotivoInsumo, i.MotivoValor1, i.MotivoValor2);
        }

        // ── Gráficos (decisión 83): una pestaña por gráfico con series; se arman con lo que trae el análisis ──
        private void ArmarGraficos()
        {
            foreach (string grafico in new[] { ANALISIS_BLL.GraficoEvolucion, ANALISIS_BLL.GraficoParticipacion, ANALISIS_BLL.GraficoRiesgo, ANALISIS_BLL.GraficoEstacionalidad })
            {
                var series = analisis.Series.Where(s => s.Grafico == grafico && s.Puntos.Count > 0).ToList();
                if (series.Count == 0) continue;
                var pagina = new TabPage(GestorIdioma.Instancia.Traducir("tabGrafico" + grafico + "frmAnalisisRentabilidad")) { Name = "tab" + grafico };
                pagina.Controls.Add(CrearGrafico(grafico, series));
                tabGraficosfrmAnalisisRentabilidad.TabPages.Add(pagina);
            }
        }

        private Chart CrearGrafico(string grafico, List<BE.ANALISIS_SERIE> series)
        {
            var g = GestorIdioma.Instancia;
            var chart = new Chart { Dock = DockStyle.Fill, BackColor = Color.NavajoWhite, Name = "chart" + grafico };
            var area = new ChartArea("area") { BackColor = Color.White };
            area.AxisX.MajorGrid.LineColor = area.AxisY.MajorGrid.LineColor = Color.Gainsboro;
            area.AxisX.LabelStyle.Font = area.AxisY.LabelStyle.Font = new Font("Segoe UI", 8F);
            chart.ChartAreas.Add(area);
            chart.Legends.Add(new Legend("leyenda") { Docking = Docking.Bottom, BackColor = Color.NavajoWhite });
            foreach (var s in series)
            {
                var serie = new Series(s.Nombre.StartsWith("msg") ? g.Traducir(s.Nombre) : s.Nombre) { ChartArea = "area", Legend = "leyenda" };
                switch (s.Tipo)
                {
                    case ANALISIS_BLL.TipoTorta:
                        serie.ChartType = SeriesChartType.Pie;
                        serie.Label = "#PERCENT{P0}";
                        serie.LegendText = "#VALX";
                        break;
                    case ANALISIS_BLL.TipoBarras:
                        serie.ChartType = SeriesChartType.Column;
                        serie.IsValueShownAsLabel = true;
                        serie.LabelFormat = "$ #,##0";
                        area.AxisY.LabelStyle.Format = "$ #,##0";
                        area.AxisX.Interval = 1;
                        break;
                    default:
                        serie.ChartType = SeriesChartType.Line;
                        serie.BorderWidth = 2;
                        serie.MarkerStyle = MarkerStyle.Circle;
                        break;
                }
                foreach (var p in s.Puntos)
                    serie.Points.AddXY(EtiquetaEje(p.Etiqueta), (double)p.Valor);
                chart.Series.Add(serie);
            }
            if (grafico == ANALISIS_BLL.GraficoEvolucion)
                area.AxisY.LabelStyle.Format = "$ #,##0";
            return chart;
        }

        // Fechas aaaa-MM-dd → dd/MM y meses aaaa-MM → MM/aaaa; el resto (tipo, insumo) queda igual
        private static string EtiquetaEje(string etiqueta)
        {
            if (DateTime.TryParseExact(etiqueta, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dia))
                return dia.ToString("dd/MM");
            if (DateTime.TryParseExact(etiqueta, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime mes))
                return mes.ToString("MM/yyyy");
            return etiqueta;
        }

        public void ActualizarIdioma()
        {
            var g = GestorIdioma.Instancia;
            this.Text = g.Traducir(this.Name);
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is DataGridView || ctrl is DateTimePicker || ctrl is ComboBox || ctrl is TabControl
                    || ctrl == lblResumenfrmAnalisisRentabilidad || ctrl == lblAvisosfrmAnalisisRentabilidad)
                    continue;
                ctrl.Text = g.Traducir(ctrl.Name);
            }
            foreach (DataGridViewColumn col in dgvResultadosfrmAnalisisRentabilidad.Columns)
                col.HeaderText = g.Traducir(col.Name);
            foreach (DataGridViewColumn col in dgvSugeridosfrmAnalisisRentabilidad.Columns)
                col.HeaderText = g.Traducir(col.Name == "colMargenActualSugeridofrmAnalisisRentabilidad" ? "colMargenActualfrmAnalisisRentabilidad" : col.Name);
            if (IsHandleCreated)
            {
                CargarCriterios();
                MostrarAnalisis();
            }
        }
    }
}
