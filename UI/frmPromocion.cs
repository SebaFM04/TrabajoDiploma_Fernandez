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
    // N03 CU033 Crear Promoción (decisiones 74 a 79): listado de promociones con filtro por estado y alta con productos,
    // tamaños y precio promocional. Muestra el margen de cada línea y pide confirmación si es negativo (decisión 77).
    public partial class frmPromocion : Form, IObservadorIdioma
    {
        PROMOCION_BLL GestorPromocion = new PROMOCION_BLL();
        // Productos y tamaños a la venta con precio de lista y costo actual
        List<BE.PROMOCION_DETALLE> disponibles = new List<BE.PROMOCION_DETALLE>();
        // Líneas de la promoción en armado
        List<BE.PROMOCION_DETALLE> lineas = new List<BE.PROMOCION_DETALLE>();
        // Promoción armada desde el análisis (CU031 → CU033) y filtro inicial (tablero)
        readonly BE.PROMOCION armada;
        readonly string filtroInicial;
        bool cargando = false;
        // Criterios en el orden del combo y sus claves de idioma (las mismas del análisis)
        readonly string[] criterios = { PROMOCION_BLL.CriterioVentas, PROMOCION_BLL.CriterioRotacion, PROMOCION_BLL.CriterioVencimiento, PROMOCION_BLL.CriterioEstacionalidad };

        const string FormatoPesos = "$ #,##0.00";
        const string FormatoPorcentaje = "0.0' %'";

        public frmPromocion() : this(null, null) { }

        // Desde el tablero: listado filtrado por estado
        public frmPromocion(string filtroEstado) : this(filtroEstado, null) { }

        // Desde el análisis de rentabilidad: alta con el criterio y los productos elegidos
        public frmPromocion(BE.PROMOCION promocionArmada) : this(null, promocionArmada) { }

        private frmPromocion(string filtroEstado, BE.PROMOCION promocionArmada)
        {
            InitializeComponent();
            filtroInicial = filtroEstado;
            armada = promocionArmada;
            // Las columnas se crean acá: GUIManager traduce el formulario antes del Load
            Enlazar();
        }

        private void frmPromocion_Load(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            try
            {
                // Decisión 78: estados por fecha al abrir el formulario de promociones
                GestorPromocion.ActualizarEstados();
                disponibles = GestorPromocion.ListarProductosParaPromocion();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
            CargarCombos();
            if (filtroInicial != null)
                cmbFiltrofrmPromocion.SelectedIndex = 1 + Array.IndexOf(GestorPromocion.ListarEstados(), filtroInicial);
            CargarPromociones();
            NuevaPromocion(armada);
        }

        private void Enlazar()
        {
            var g = GestorIdioma.Instancia;
            dgvPromocionesfrmPromocion.Columns.Clear();
            foreach (string col in new[] { "colNombrefrmPromocion", "colCriteriofrmPromocion", "colDesdefrmPromocion", "colHastafrmPromocion", "colEstadofrmPromocion", "colResultadofrmPromocion" })
                dgvPromocionesfrmPromocion.Columns.Add(col, g.Traducir(col));
            AjusteGrilla.Configurar(dgvPromocionesfrmPromocion);

            ArmarColumnasLineas(dgvDetallefrmPromocion, false);
            ArmarColumnasLineas(dgvLineasfrmPromocion, true);
        }

        // Columnas de una grilla de productos de promoción; en la de alta solo se edita el precio promocional
        private void ArmarColumnasLineas(DataGridView dgv, bool editable)
        {
            var g = GestorIdioma.Instancia;
            dgv.Columns.Clear();
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colProductofrmPromocion", HeaderText = g.Traducir("colProductofrmPromocion"), ReadOnly = true });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTamaniofrmPromocion", HeaderText = g.Traducir("colTamaniofrmPromocion"), ReadOnly = true });
            foreach (string col in new[] { "colPrecioListafrmPromocion", "colCostofrmPromocion", "colPrecioPromocionalfrmPromocion", "colMargenUnitariofrmPromocion", "colMargenPorcentajefrmPromocion" })
                dgv.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = col,
                    HeaderText = g.Traducir(col),
                    ValueType = typeof(decimal),
                    ReadOnly = !(editable && col == "colPrecioPromocionalfrmPromocion"),
                    DefaultCellStyle = { Format = col == "colMargenPorcentajefrmPromocion" ? FormatoPorcentaje : FormatoPesos, Alignment = DataGridViewContentAlignment.MiddleRight }
                });
            if (editable)
                dgv.Columns["colPrecioPromocionalfrmPromocion"].DefaultCellStyle.BackColor = Color.LightYellow;
            AjusteGrilla.Configurar(dgv);
        }

        private void CargarCombos()
        {
            var g = GestorIdioma.Instancia;
            cargando = true;
            int filtro = Math.Max(0, cmbFiltrofrmPromocion.SelectedIndex);
            cmbFiltrofrmPromocion.Items.Clear();
            cmbFiltrofrmPromocion.Items.Add(g.Traducir("msgPromocionEstadoTodas"));
            foreach (string estado in GestorPromocion.ListarEstados())
                cmbFiltrofrmPromocion.Items.Add(TextoEstado(estado));
            cmbFiltrofrmPromocion.SelectedIndex = filtro;

            int criterio = Math.Max(0, cmbCriteriofrmPromocion.SelectedIndex);
            cmbCriteriofrmPromocion.Items.Clear();
            foreach (string c in criterios)
                cmbCriteriofrmPromocion.Items.Add(TextoCriterio(c));
            cmbCriteriofrmPromocion.SelectedIndex = criterio;

            int producto = cmbProductofrmPromocion.SelectedIndex;
            cmbProductofrmPromocion.Items.Clear();
            foreach (var d in disponibles)
                cmbProductofrmPromocion.Items.Add($"{d.ProductoTamanio.Producto.Nombre} - {d.ProductoTamanio.Tamanio.Nombre} ({FormatoMoneda.Pesos(d.PrecioOriginal)})");
            if (producto >= 0 && producto < cmbProductofrmPromocion.Items.Count)
                cmbProductofrmPromocion.SelectedIndex = producto;
            cargando = false;
        }

        private static string TextoEstado(string estado)
        {
            return GestorIdioma.Instancia.Traducir("msgPromocionEstado" + estado.Replace(" ", ""));
        }

        private static string TextoCriterio(string criterio)
        {
            return GestorIdioma.Instancia.Traducir("msgCriterio" + criterio.Replace("ó", "o"));
        }

        // ── Listado ───────────────────────────────────────────────────
        private void CargarPromociones()
        {
            cargando = true;
            dgvPromocionesfrmPromocion.Rows.Clear();
            dgvDetallefrmPromocion.Rows.Clear();
            try
            {
                int filtro = cmbFiltrofrmPromocion.SelectedIndex;
                string estado = filtro <= 0 ? null : GestorPromocion.ListarEstados()[filtro - 1];
                foreach (var p in GestorPromocion.ListarPromociones(estado))
                {
                    int fila = dgvPromocionesfrmPromocion.Rows.Add(p.Nombre, TextoCriterio(p.Criterio), p.FechaDesde.ToString("dd/MM/yyyy"),
                        p.FechaHasta.ToString("dd/MM/yyyy"), TextoEstado(p.Estado),
                        p.Resultado == null ? "-" : GestorIdioma.Instancia.Traducir("msgPromocionResultado" + p.Resultado.Replace(" ", "")));
                    dgvPromocionesfrmPromocion.Rows[fila].Tag = p;
                    if (p.Estado == PROMOCION_BLL.EstadoVigente)
                        dgvPromocionesfrmPromocion.Rows[fila].DefaultCellStyle.BackColor = Color.Honeydew;
                }
                dgvPromocionesfrmPromocion.ClearSelection();
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

        private void cmbFiltrofrmPromocion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cargando) CargarPromociones();
        }

        private void dgvPromocionesfrmPromocion_SelectionChanged(object sender, EventArgs e)
        {
            if (cargando) return;
            dgvDetallefrmPromocion.Rows.Clear();
            var seleccionada = dgvPromocionesfrmPromocion.SelectedRows.Count > 0 ? dgvPromocionesfrmPromocion.SelectedRows[0].Tag as BE.PROMOCION : null;
            if (seleccionada == null) return;
            try
            {
                foreach (var d in GestorPromocion.ObtenerPromocion(seleccionada.IdPromocion).Detalles)
                    AgregarFila(dgvDetallefrmPromocion, d);
                dgvDetallefrmPromocion.ClearSelection();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void AgregarFila(DataGridView dgv, BE.PROMOCION_DETALLE d)
        {
            int fila = dgv.Rows.Add(d.ProductoTamanio.Producto.Nombre, d.ProductoTamanio.Tamanio.Nombre, d.PrecioOriginal, d.CostoUnitario,
                d.PrecioPromocional, d.MargenUnitario, d.MargenPorcentaje);
            dgv.Rows[fila].Tag = d;
            PintarMargen(dgv.Rows[fila], d);
        }

        // Decisión 77: el margen negativo se muestra en rojo (se puede aceptar con confirmación)
        private static void PintarMargen(DataGridViewRow fila, BE.PROMOCION_DETALLE d)
        {
            var color = d.MargenUnitario < 0 ? Color.Firebrick : Color.Empty;
            fila.Cells["colMargenUnitariofrmPromocion"].Style.ForeColor = color;
            fila.Cells["colMargenPorcentajefrmPromocion"].Style.ForeColor = color;
        }

        // ── Alta (CU033) ──────────────────────────────────────────────

        // CU033 paso 1: formulario vacío, o con el criterio y los productos que vienen del análisis
        private void NuevaPromocion(BE.PROMOCION desdeAnalisis)
        {
            txtNombrefrmPromocion.Text = string.Empty;
            dtpDesdefrmPromocion.Value = DateTime.Today;
            dtpHastafrmPromocion.Value = DateTime.Today.AddDays(14);
            lineas = new List<BE.PROMOCION_DETALLE>();
            if (desdeAnalisis != null)
            {
                cmbCriteriofrmPromocion.SelectedIndex = Math.Max(0, Array.IndexOf(criterios, desdeAnalisis.Criterio));
                dtpDesdefrmPromocion.Value = desdeAnalisis.FechaDesde;
                dtpHastafrmPromocion.Value = desdeAnalisis.FechaHasta;
                lineas.AddRange(desdeAnalisis.Detalles.Select(Copiar));
            }
            MostrarLineas();
        }

        private static BE.PROMOCION_DETALLE Copiar(BE.PROMOCION_DETALLE d)
        {
            return new BE.PROMOCION_DETALLE
            {
                IdProducto = d.IdProducto,
                IdTamanio = d.IdTamanio,
                PrecioOriginal = d.PrecioOriginal,
                PrecioPromocional = d.PrecioPromocional,
                CostoUnitario = d.CostoUnitario,
                ProductoTamanio = d.ProductoTamanio
            };
        }

        private void MostrarLineas()
        {
            cargando = true;
            dgvLineasfrmPromocion.Rows.Clear();
            foreach (var d in lineas)
                AgregarFila(dgvLineasfrmPromocion, d);
            dgvLineasfrmPromocion.ClearSelection();
            cargando = false;
        }

        // CU033 paso 3: al elegir un producto se propone el precio promocional (10 % menos, propuesta 5 de la Fase 5)
        private void cmbProductofrmPromocion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cargando || cmbProductofrmPromocion.SelectedIndex < 0) return;
            nudPreciofrmPromocion.Value = Math.Min(nudPreciofrmPromocion.Maximum, disponibles[cmbProductofrmPromocion.SelectedIndex].PrecioPromocional);
        }

        private void btnAgregarfrmPromocion_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            if (cmbProductofrmPromocion.SelectedIndex < 0)
            {
                Avisar("msgPromocionElegirProducto");
                return;
            }
            var elegido = disponibles[cmbProductofrmPromocion.SelectedIndex];
            if (lineas.Any(l => l.IdProducto == elegido.IdProducto && l.IdTamanio == elegido.IdTamanio))
            {
                Avisar("msgPromocionProductoRepetido");
                return;
            }
            var linea = Copiar(elegido);
            linea.PrecioPromocional = nudPreciofrmPromocion.Value;
            lineas.Add(linea);
            MostrarLineas();
        }

        private void btnQuitarfrmPromocion_Click(object sender, EventArgs e)
        {
            var linea = dgvLineasfrmPromocion.SelectedRows.Count > 0 ? dgvLineasfrmPromocion.SelectedRows[0].Tag as BE.PROMOCION_DETALLE : null;
            if (linea == null)
            {
                Avisar("msgPromocionSeleccionarLinea");
                return;
            }
            lineas.Remove(linea);
            MostrarLineas();
        }

        private void btnNuevafrmPromocion_Click(object sender, EventArgs e)
        {
            NuevaPromocion(null);
            txtNombrefrmPromocion.Focus();
        }

        // Precio promocional editado en la grilla: se recalcula el margen de la línea
        private void dgvLineasfrmPromocion_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (cargando || e.RowIndex < 0 || dgvLineasfrmPromocion.Columns[e.ColumnIndex].Name != "colPrecioPromocionalfrmPromocion") return;
            var fila = dgvLineasfrmPromocion.Rows[e.RowIndex];
            if (!(fila.Tag is BE.PROMOCION_DETALLE d)) return;
            d.PrecioPromocional = fila.Cells[e.ColumnIndex].Value is decimal precio ? precio : 0;
            cargando = true;
            fila.Cells["colMargenUnitariofrmPromocion"].Value = d.MargenUnitario;
            fila.Cells["colMargenPorcentajefrmPromocion"].Value = d.MargenPorcentaje;
            PintarMargen(fila, d);
            cargando = false;
        }

        private void dgvLineasfrmPromocion_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Texto que no es un importe: se avisa y se vuelve al valor anterior
            Avisar("msgPromocionPrecioInvalido");
            e.Cancel = false;
            e.ThrowException = false;
            dgvLineasfrmPromocion.CancelEdit();
        }

        // CU033 pasos 4 a 6: validar (FA1 a FA4), confirmar el margen negativo (decisión 77) y guardar
        private void btnCrearfrmPromocion_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            dgvLineasfrmPromocion.EndEdit();
            var promocion = new BE.PROMOCION
            {
                Nombre = txtNombrefrmPromocion.Text,
                Criterio = criterios[Math.Max(0, cmbCriteriofrmPromocion.SelectedIndex)],
                FechaDesde = dtpDesdefrmPromocion.Value.Date,
                FechaHasta = dtpHastafrmPromocion.Value.Date,
                Detalles = lineas.Select(Copiar).ToList()
            };
            try
            {
                GestorPromocion.ValidarPromocion(promocion);
            }
            catch (ArgumentException argEx) when (argEx.Message == "msgPromocionSuperpuesta")
            {
                // FA4: se informa con qué promociones se superpone
                var otras = GestorPromocion.BuscarSuperposiciones(promocion)
                    .Select(p => $"{p.Nombre} ({p.FechaDesde:dd/MM/yyyy} - {p.FechaHasta:dd/MM/yyyy}, {TextoEstado(p.Estado)})");
                MessageBox.Show(string.Format(g.Traducir("msgPromocionSuperpuesta"), Environment.NewLine + string.Join(Environment.NewLine, otras)),
                    g.Traducir("frmPromocion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            catch (ArgumentException argEx)
            {
                Avisar(argEx.Message);
                return;
            }
            catch (Exception ex)
            {
                MostrarError(ex);
                return;
            }

            // Decisión 77: con margen negativo el Dueño decide si la acepta igual
            var negativos = GestorPromocion.ObtenerMargenesNegativos(promocion);
            bool acepta = false;
            if (negativos.Count > 0)
            {
                var detalle = negativos.Select(d => $"{d.ProductoTamanio.Producto.Nombre} {d.ProductoTamanio.Tamanio.Nombre}: {FormatoMoneda.Pesos(d.MargenUnitario)} ({d.MargenPorcentaje:0.0} %)");
                if (MessageBox.Show(string.Format(g.Traducir("msgPromocionConfirmarMargenNegativo"), Environment.NewLine + string.Join(Environment.NewLine, detalle) + Environment.NewLine + Environment.NewLine),
                        g.Traducir("frmPromocion"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
                acepta = true;
            }
            try
            {
                GestorPromocion.CrearPromocion(promocion, acepta);
                MessageBox.Show(string.Format(g.Traducir("msgPromocionCreada"), promocion.Nombre, TextoEstado(promocion.Estado)),
                    g.Traducir("frmPromocion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarPromociones();
                NuevaPromocion(null);
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

        private void Avisar(string clave)
        {
            var g = GestorIdioma.Instancia;
            MessageBox.Show(g.Traducir(clave).Replace("{0}", string.Empty), g.Traducir("frmPromocion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void MostrarError(Exception ex)
        {
            var g = GestorIdioma.Instancia;
            MessageBox.Show(g.Traducir("msgPromocionError") + ex.GetBaseException().Message, g.Traducir("frmPromocion"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ActualizarIdioma()
        {
            var g = GestorIdioma.Instancia;
            this.Text = g.Traducir(this.Name);
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is DataGridView || ctrl is ComboBox || ctrl is TextBox || ctrl is DateTimePicker || ctrl is NumericUpDown)
                    continue;
                ctrl.Text = g.Traducir(ctrl.Name);
            }
            foreach (var dgv in new[] { dgvPromocionesfrmPromocion, dgvDetallefrmPromocion, dgvLineasfrmPromocion })
                foreach (DataGridViewColumn col in dgv.Columns)
                    col.HeaderText = g.Traducir(col.Name);
            if (IsHandleCreated)
            {
                CargarCombos();
                CargarPromociones();
            }
        }
    }
}
