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
        // Nombre del XML si el análisis a la vista se abrió de un archivo (modo consulta, decisión 85)
        string archivoAbierto = null;
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
            // CU033 se ofrece solo a quien tiene el permiso de promociones
            var usuario = SessionManager.Instancia.UsuarioActual;
            btnCrearPromocionfrmAnalisisRentabilidad.Visible = usuario != null && usuario.TienePermiso("Gestion Promociones");
            CargarCriterios();
            MostrarAnalisis();
        }

        // CU031 → CU033 («extend»): promoción con los sugeridos marcados en la grilla (si no hay ninguno, todos)
        private void btnCrearPromocionfrmAnalisisRentabilidad_Click(object sender, EventArgs e)
        {
            if (analisis == null) return;
            var elegidos = dgvSugeridosfrmAnalisisRentabilidad.SelectedRows.Cast<DataGridViewRow>()
                .Select(f => f.Tag as BE.ANALISIS_ITEM).Where(i => i != null).OrderBy(i => i.Orden).ToList();
            if (elegidos.Count == 0)
                elegidos = analisis.ObtenerSugeridos();
            try
            {
                var promocion = new PROMOCION_BLL().ArmarPromocionDesdeAnalisis(analisis, elegidos);
                GUIManager.Instancia.AbrirForm(new frmPromocion(promocion));
            }
            catch (Exception ex)
            {
                var g = GestorIdioma.Instancia;
                MessageBox.Show(g.Traducir("msgAnalisisError") + ex.GetBaseException().Message, g.Traducir("frmAnalisisRentabilidad"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                archivoAbierto = null;
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
            btnCrearPromocionfrmAnalisisRentabilidad.Enabled = false;
            // CU032 FA1 y XML: sin análisis a la vista no hay nada para exportar ni guardar
            btnExportarPdffrmAnalisisRentabilidad.Enabled = btnGuardarfrmAnalisisRentabilidad.Enabled = analisis != null;
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
                + (analisis.DiasPorVencer > 0 ? " " + string.Format(g.Traducir("msgAnalisisResumenDias"), analisis.DiasPorVencer) : "")
                + (archivoAbierto != null ? " " + string.Format(g.Traducir("msgAnalisisAbiertoDesde"), archivoAbierto) : "");
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
                int fila = dgvSugeridosfrmAnalisisRentabilidad.Rows.Add(i.Orden, i.NombreProducto, i.NombreTamanio, GestorAnalisis.TextoMotivo(i), i.MargenUnitarioActual);
                dgvSugeridosfrmAnalisisRentabilidad.Rows[fila].Tag = i;
            }
            dgvSugeridosfrmAnalisisRentabilidad.ClearSelection();
            // CU033 «extend» CU031: se puede crear una promoción con los sugeridos (con permiso de promociones)
            btnCrearPromocionfrmAnalisisRentabilidad.Enabled = analisis.ObtenerSugeridos().Count > 0;

            lblAvisosfrmAnalisisRentabilidad.Text = string.Join(Environment.NewLine,
                analisis.Avisos.Select(a => string.Format(g.Traducir(a), analisis.MesesHistoria, analisis.DiasPorVencer)));
            ArmarGraficos();
        }

        // ── CU032 y XML (decisiones 84 a 87) ───────────────────────────

        // CU032: PDF del análisis a la vista (generado o abierto), con los gráficos de sus pestañas como imagen
        private void btnExportarPdffrmAnalisisRentabilidad_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            Cursor = Cursors.WaitCursor;
            try
            {
                string ruta = GestorAnalisis.ExportarPdf(analisis, ImagenesGraficos());
                Cursor = Cursors.Default;
                MessageBox.Show(string.Format(g.Traducir("msgAnalisisPdfExportado"), ruta), g.Traducir("frmAnalisisRentabilidad"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start(ruta);
            }
            catch (ArgumentException argEx)
            {
                // FA1 no hay análisis / FA2 error al guardar
                Cursor = Cursors.Default;
                MessageBox.Show(g.Traducir(argEx.Message), g.Traducir("frmAnalisisRentabilidad"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(g.Traducir("msgAnalisisError") + ex.GetBaseException().Message, g.Traducir("frmAnalisisRentabilidad"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Decisión 84: cada gráfico de las pestañas se dibuja aparte, más ancho, y se pasa como PNG en memoria
        private List<KeyValuePair<string, byte[]>> ImagenesGraficos()
        {
            var imagenes = new List<KeyValuePair<string, byte[]>>();
            if (analisis == null) return imagenes;
            foreach (TabPage pagina in tabGraficosfrmAnalisisRentabilidad.TabPages)
            {
                string grafico = pagina.Name.Substring("tab".Length);
                using (var chart = CrearGrafico(grafico, analisis.Series.Where(s => s.Grafico == grafico && s.Puntos.Count > 0).ToList()))
                using (var flujo = new System.IO.MemoryStream())
                {
                    chart.Dock = DockStyle.None;
                    chart.Size = new Size(1000, 460);
                    chart.SaveImage(flujo, ChartImageFormat.Png);
                    imagenes.Add(new KeyValuePair<string, byte[]>(pagina.Text, flujo.ToArray()));
                }
            }
            return imagenes;
        }

        // Decisión 85: "Guardar análisis" en Documentos\SistemaBar\Analisis\Analisis_aaaaMMdd_HHmm.xml
        private void btnGuardarfrmAnalisisRentabilidad_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            if (analisis == null) return;
            using (var dialogo = new SaveFileDialog { Filter = "XML (*.xml)|*.xml", FileName = GestorAnalisis.NombreArchivo(analisis) })
            {
                System.IO.Directory.CreateDirectory(GestorAnalisis.CarpetaAnalisis);
                dialogo.InitialDirectory = GestorAnalisis.CarpetaAnalisis;
                if (dialogo.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    GestorAnalisis.GuardarAnalisis(analisis, dialogo.FileName);
                    MessageBox.Show(string.Format(g.Traducir("msgAnalisisGuardado"), dialogo.FileName), g.Traducir("frmAnalisisRentabilidad"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (ArgumentException argEx)
                {
                    MessageBox.Show(g.Traducir(argEx.Message), g.Traducir("frmAnalisisRentabilidad"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        // Decisión 85: "Abrir análisis" lo muestra en modo consulta, sin recalcular
        private void btnAbrirfrmAnalisisRentabilidad_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            using (var dialogo = new OpenFileDialog { Filter = "XML (*.xml)|*.xml" })
            {
                System.IO.Directory.CreateDirectory(GestorAnalisis.CarpetaAnalisis);
                dialogo.InitialDirectory = GestorAnalisis.CarpetaAnalisis;
                if (dialogo.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    analisis = GestorAnalisis.AbrirAnalisis(dialogo.FileName);
                    archivoAbierto = System.IO.Path.GetFileName(dialogo.FileName);
                    MostrarAnalisis();
                }
                catch (ArgumentException argEx)
                {
                    // El archivo no es un análisis válido: se avisa y queda lo que se estaba mostrando
                    MessageBox.Show(g.Traducir(argEx.Message), g.Traducir("frmAnalisisRentabilidad"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
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
