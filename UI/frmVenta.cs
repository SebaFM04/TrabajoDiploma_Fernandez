using BE;
using BLL;
using SERVICIO;
using SERVICIO.MultiIdioma_Observer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    // CU010 Registrar Venta e «include» CU015 Emitir Factura (DSS-CU010 y DSS-CU015). Actor: Cajero.
    public partial class frmVenta : Form, IObservadorIdioma
    {
        VENTA_BLL GestorVenta = new VENTA_BLL();
        FACTURA_BLL GestorFactura = new FACTURA_BLL();
        // Pedido en armado; después de cobrar es la venta registrada
        BE.VENTA venta = new BE.VENTA();
        // true cuando el pedido actual pasó la verificación de stock (paso 2) y se calculó el total (paso 3)
        bool verificado = false;
        BE.FACTURA factura = null;

        public frmVenta()
        {
            InitializeComponent();
            // Las columnas se crean acá: GUIManager traduce el formulario antes del Load
            Enlazar();
        }

        private void frmVenta_Load(object sender, EventArgs e)
        {
            cmbMedioPagofrmVenta.Items.Clear();
            cmbMedioPagofrmVenta.Items.AddRange(GestorVenta.ListarMediosPago());
            CargarProductos();
            NuevaVenta();
        }

        private void Enlazar()
        {
            var g = GestorIdioma.Instancia;
            dgvProductosfrmVenta.Columns.Clear();
            dgvProductosfrmVenta.Columns.Add("colProductofrmVenta", g.Traducir("colProductofrmVenta"));
            dgvProductosfrmVenta.Columns.Add("colTamaniofrmVenta", g.Traducir("colTamaniofrmVenta"));
            dgvProductosfrmVenta.Columns.Add("colPreciofrmVenta", g.Traducir("colPreciofrmVenta"));
            AjusteGrilla.Configurar(dgvProductosfrmVenta);

            dgvPedidofrmVenta.Columns.Clear();
            dgvPedidofrmVenta.Columns.Add("colProductoPedidofrmVenta", g.Traducir("colProductofrmVenta"));
            dgvPedidofrmVenta.Columns.Add("colTamanioPedidofrmVenta", g.Traducir("colTamaniofrmVenta"));
            dgvPedidofrmVenta.Columns.Add("colCantidadfrmVenta", g.Traducir("colCantidadfrmVenta"));
            dgvPedidofrmVenta.Columns.Add("colSubtotalfrmVenta", g.Traducir("colSubtotalfrmVenta"));
            AjusteGrilla.Configurar(dgvPedidofrmVenta);
        }

        // CU010 paso 1: productos activos con receta (S1) con sus tamaños y precios
        private void CargarProductos()
        {
            var g = GestorIdioma.Instancia;
            dgvProductosfrmVenta.Rows.Clear();
            try
            {
                var lista = GestorVenta.ListarProductosParaVenta();
                foreach (var pt in lista)
                {
                    int fila = dgvProductosfrmVenta.Rows.Add(pt.Producto.Nombre, pt.Tamanio.Nombre, FormatoMoneda.Pesos(pt.Precio));
                    dgvProductosfrmVenta.Rows[fila].Tag = pt;
                }
                dgvProductosfrmVenta.ClearSelection();
                if (lista.Count == 0)
                    MessageBox.Show(g.Traducir("msgVentaSinProductos"), g.Traducir("frmVenta"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        // Estado inicial: pedido vacío, sin factura
        private void NuevaVenta()
        {
            venta = new BE.VENTA();
            factura = null;
            dgvPedidofrmVenta.Rows.Clear();
            cmbMedioPagofrmVenta.SelectedIndex = -1;
            txtNombreClientefrmVenta.Text = txtTelefonoClientefrmVenta.Text = txtCorreoClientefrmVenta.Text = string.Empty;
            txtResultadofrmVenta.Text = string.Empty;
            nudCantidadfrmVenta.Value = 1;
            PedidoModificado();
            HabilitarEtapa(cobrada: false);
        }

        // Pedido editable hasta cobrar; después solo la factura y "Nueva venta"
        private void HabilitarEtapa(bool cobrada)
        {
            dgvProductosfrmVenta.Enabled = nudCantidadfrmVenta.Enabled = btnAgregarfrmVenta.Enabled = !cobrada;
            btnQuitarfrmVenta.Enabled = btnVerificarfrmVenta.Enabled = cmbMedioPagofrmVenta.Enabled = btnCancelarfrmVenta.Enabled = !cobrada;
            btnCobrarfrmVenta.Enabled = !cobrada && verificado;
            txtNombreClientefrmVenta.Enabled = txtTelefonoClientefrmVenta.Enabled = txtCorreoClientefrmVenta.Enabled = cobrada && factura == null;
            btnEmitirFacturafrmVenta.Enabled = cobrada && factura == null;
            btnNuevaVentafrmVenta.Enabled = cobrada;
        }

        // Cualquier cambio en el pedido obliga a verificar de nuevo (vuelve al paso 1)
        private void PedidoModificado()
        {
            verificado = false;
            lblTotalfrmVenta.Text = "-";
            btnCobrarfrmVenta.Enabled = false;
        }

        private void MostrarPedido()
        {
            dgvPedidofrmVenta.Rows.Clear();
            foreach (var d in venta.Detalles)
            {
                int fila = dgvPedidofrmVenta.Rows.Add(d.ProductoTamanio.Producto.Nombre, d.ProductoTamanio.Tamanio.Nombre, d.Cantidad,
                    FormatoMoneda.Pesos(d.ProductoTamanio.Precio * d.Cantidad));
                dgvPedidofrmVenta.Rows[fila].Tag = d;
            }
            dgvPedidofrmVenta.ClearSelection();
        }

        // CU010 paso 1: el Cajero selecciona productos y tamaños
        private void btnAgregarfrmVenta_Click(object sender, EventArgs e)
        {
            if (dgvProductosfrmVenta.SelectedRows.Count == 0)
            {
                Avisar("msgVentaSeleccionarProducto");
                return;
            }
            var pt = dgvProductosfrmVenta.SelectedRows[0].Tag as BE.PRODUCTO_TAMANIO;
            int cantidad = (int)nudCantidadfrmVenta.Value;
            // Mismo producto y tamaño: se suma la cantidad a la línea existente
            var existente = venta.Detalles.FirstOrDefault(d => d.IdProducto == pt.IdProducto && d.IdTamanio == pt.IdTamanio);
            if (existente != null)
                existente.Cantidad += cantidad;
            else
                venta.Detalles.Add(new BE.VENTA_DETALLE { IdProducto = pt.IdProducto, IdTamanio = pt.IdTamanio, Cantidad = cantidad, ProductoTamanio = pt });
            MostrarPedido();
            PedidoModificado();
            nudCantidadfrmVenta.Value = 1;
        }

        private void dgvProductosfrmVenta_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) btnAgregarfrmVenta_Click(sender, e);
        }

        private void btnQuitarfrmVenta_Click(object sender, EventArgs e)
        {
            if (dgvPedidofrmVenta.SelectedRows.Count == 0) return;
            venta.Detalles.Remove(dgvPedidofrmVenta.SelectedRows[0].Tag as BE.VENTA_DETALLE);
            MostrarPedido();
            PedidoModificado();
        }

        // CU010 pasos 2 y 3: verifica los insumos (FA1) y muestra el total y los medios de pago
        private void btnVerificarfrmVenta_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            try
            {
                var faltantes = GestorVenta.VerificarDisponibilidadInsumos(venta);
                if (faltantes.Count > 0)
                {
                    // FA1 2.2: cuál insumo falta y qué productos afecta. El Cajero modifica el pedido o cancela.
                    txtResultadofrmVenta.Text = DescribirFaltantes(faltantes);
                    MessageBox.Show(txtResultadofrmVenta.Text, g.Traducir("msgVentaFaltantesTitulo"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    PedidoModificado();
                    return;
                }
                decimal total = GestorVenta.CalcularMontoTotal(venta);
                lblTotalfrmVenta.Text = FormatoMoneda.Pesos(total);
                verificado = true;
                btnCobrarfrmVenta.Enabled = true;
                txtResultadofrmVenta.Text = string.Format(g.Traducir("msgVentaStockOk"), lblTotalfrmVenta.Text);
                cmbMedioPagofrmVenta.Focus();
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

        private string DescribirFaltantes(List<BE.RECETA> faltantes)
        {
            var g = GestorIdioma.Instancia;
            var texto = new StringBuilder();
            foreach (var grupo in faltantes.GroupBy(f => f.IdInsumo))
            {
                var f = grupo.First();
                var productos = grupo.Select(x => venta.Detalles.First(d => d.IdProducto == x.IdProducto).ProductoTamanio.Producto.Nombre).Distinct();
                texto.AppendLine(string.Format(g.Traducir("msgVentaInsumoInsuficiente"),
                    f.Insumo?.Nombre ?? f.IdInsumo.ToString(),
                    f.Proporcion.ToString("#,0.###", CultureInfo.CurrentCulture),
                    f.Insumo?.UnidadMedida,
                    (f.Insumo?.VolumenPesoDisponible ?? 0).ToString("#,0.###", CultureInfo.CurrentCulture),
                    string.Join(", ", productos)));
            }
            return texto.ToString();
        }

        // CU010 pasos 4 y 5: medio de pago y cobro. Registra todo en una transacción (VENTA_BLL.RegistrarVenta).
        private void btnCobrarfrmVenta_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            if (!verificado)
            {
                Avisar("msgVentaVerificarPrimero");
                return;
            }
            if (cmbMedioPagofrmVenta.SelectedItem == null)
            {
                Avisar("msgVentaMedioPagoObligatorio");
                return;
            }
            venta.MedioPago = cmbMedioPagofrmVenta.SelectedItem.ToString();
            try
            {
                GestorVenta.RegistrarVenta(venta);
                var texto = new StringBuilder();
                texto.AppendLine(string.Format(g.Traducir("msgVentaRegistrada"), venta.IdVenta, venta.Vale.IdVale, FormatoMoneda.Pesos(venta.Monto)));
                if (venta.Comanda != null)
                    texto.AppendLine(string.Format(g.Traducir("msgVentaComanda"), venta.Comanda.IdComanda));
                texto.AppendLine(g.Traducir("msgVentaPedirDatosFactura"));
                txtResultadofrmVenta.Text = texto.ToString();
                HabilitarEtapa(cobrada: true);
                // CU015 paso 1: solicita los datos del cliente
                txtNombreClientefrmVenta.Focus();
            }
            catch (ArgumentException argEx)
            {
                // Ajuste del paso 5: el stock cambió; no se registró nada y se sigue en FA1 2.2
                Avisar(argEx.Message);
                PedidoModificado();
                if (argEx.Message == "msgVentaStockInsuficiente")
                    btnVerificarfrmVenta_Click(sender, e);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
                PedidoModificado();
            }
        }

        // FA1 2.4: el cliente no acepta la alternativa; no se cobra ni se genera nada
        private void btnCancelarfrmVenta_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            if (venta.Detalles.Count > 0 &&
                MessageBox.Show(g.Traducir("msgVentaCancelarConfirm"), g.Traducir("frmVenta"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            NuevaVenta();
        }

        // CU015 Emitir Factura (pasos 2 a 6). FA2: si falla, la venta queda registrada y se puede reintentar.
        private void btnEmitirFacturafrmVenta_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            try
            {
                // FA1: formato de los datos cargados
                GestorFactura.ValidarDatosCliente(txtNombreClientefrmVenta.Text, txtTelefonoClientefrmVenta.Text, txtCorreoClientefrmVenta.Text);
            }
            catch (ArgumentException argEx)
            {
                Avisar(argEx.Message);
                return;
            }

            try
            {
                factura = GestorFactura.EmitirFactura(venta, txtNombreClientefrmVenta.Text, txtTelefonoClientefrmVenta.Text, txtCorreoClientefrmVenta.Text);
            }
            catch (ArgumentException argEx)
            {
                Avisar(argEx.Message);
                return;
            }
            catch (Exception ex)
            {
                // FA2: no se pudo registrar la factura
                MessageBox.Show(string.Format(g.Traducir("msgFacturaNoEmitida"), ex.GetBaseException().Message), g.Traducir("frmVenta"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // CU010 paso 6: se muestran el vale, la factura y la comanda
            var texto = new StringBuilder(txtResultadofrmVenta.Text.Replace(g.Traducir("msgVentaPedirDatosFactura"), string.Empty).TrimEnd());
            texto.AppendLine();
            texto.AppendLine(string.Format(g.Traducir("msgFacturaEmitida"), GestorFactura.NumeroCompleto(factura), FormatoMoneda.Pesos(factura.Total)));
            try
            {
                string ruta = GestorFactura.GenerarPdf(factura, venta);
                texto.AppendLine(string.Format(g.Traducir("msgFacturaPdfGuardado"), ruta));
                Process.Start(ruta);
            }
            catch (Exception ex)
            {
                texto.AppendLine(string.Format(g.Traducir("msgFacturaPdfError"), ex.GetBaseException().Message));
            }
            txtResultadofrmVenta.Text = texto.ToString();
            HabilitarEtapa(cobrada: true);
        }

        private void btnNuevaVentafrmVenta_Click(object sender, EventArgs e)
        {
            var g = GestorIdioma.Instancia;
            if (factura == null &&
                MessageBox.Show(g.Traducir("msgVentaSinFacturaConfirm"), g.Traducir("frmVenta"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;
            NuevaVenta();
            // Los stocks cambiaron; se recargan por si algún producto dejó de estar disponible
            CargarProductos();
        }

        private void Avisar(string clave)
        {
            var g = GestorIdioma.Instancia;
            MessageBox.Show(g.Traducir(clave), g.Traducir("msgVentaAviso"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void MostrarError(Exception ex)
        {
            var g = GestorIdioma.Instancia;
            MessageBox.Show(g.Traducir("msgVentaError") + ex.GetBaseException().Message, g.Traducir("frmVenta"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ActualizarIdioma()
        {
            var g = GestorIdioma.Instancia;
            this.Text = g.Traducir(this.Name);
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is TextBox || ctrl is DataGridView || ctrl is ComboBox || ctrl is NumericUpDown || ctrl == lblTotalfrmVenta)
                    continue;
                ctrl.Text = g.Traducir(ctrl.Name);
            }
            foreach (DataGridViewColumn col in dgvProductosfrmVenta.Columns)
                col.HeaderText = g.Traducir(col.Name);
            dgvPedidofrmVenta.Columns["colProductoPedidofrmVenta"].HeaderText = g.Traducir("colProductofrmVenta");
            dgvPedidofrmVenta.Columns["colTamanioPedidofrmVenta"].HeaderText = g.Traducir("colTamaniofrmVenta");
            dgvPedidofrmVenta.Columns["colCantidadfrmVenta"].HeaderText = g.Traducir("colCantidadfrmVenta");
            dgvPedidofrmVenta.Columns["colSubtotalfrmVenta"].HeaderText = g.Traducir("colSubtotalfrmVenta");
        }
    }
}
