using BE;
using DAL;
using SERVICIO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    // N02 CU021 Registrar Pago a Proveedor (DSS-CU021). El pago se ejecuta fuera del sistema (decisión 19).
    public class PAGO_PROVEEDOR_BLL
    {
        ORDEN_COMPRA_BLL GestorOrden = new ORDEN_COMPRA_BLL();

        // Medios de pago a proveedores
        public string[] ListarMediosPago()
        {
            return new[] { "Transferencia", "Efectivo", "Cheque", "Débito" };
        }

        // CU021 paso 4 / FA2 (mensajes 22-25): campos completos, monto mayor a cero e igual al total de la orden
        public bool ValidarPago(BE.PAGO_PROVEEDOR pago)
        {
            var orden = GestorOrden.ObtenerDetalle(pago.IdOrdenCompra);
            // Condición del CU: orden "Cerrada" y sin pago (al pagarse pasa a "Pagada")
            if (orden.Estado != ORDEN_COMPRA_BLL.EstadoCerrada)
                throw new ArgumentException("msgOrdenEstadoInvalido");
            pago.NumeroComprobante = (pago.NumeroComprobante ?? string.Empty).Trim();
            if (Array.IndexOf(ListarMediosPago(), pago.MedioPago) < 0 || pago.NumeroComprobante.Length == 0)
                throw new ArgumentException("msgPagoDatosObligatorios");
            if (pago.NumeroComprobante.Length > 30)
                throw new ArgumentException("msgPagoComprobanteLargo");
            if (pago.FechaPago.Date > DateTime.Today || pago.FechaPago.Date < orden.FechaGeneracion.Date)
                throw new ArgumentException("msgPagoFechaInvalida");
            if (pago.Monto <= 0)
                throw new ArgumentException("msgPagoMontoInvalido");
            if (pago.Monto != GestorOrden.CalcularTotal(pago.IdOrdenCompra))
                throw new ArgumentException("msgPagoMontoDistinto");
            return true;
        }

        // CU021 paso 4 (mensajes 26-38): pago registrado y orden "Pagada", en una transacción
        public void RegistrarPago(BE.PAGO_PROVEEDOR pago)
        {
            ValidarPago(pago);
            var acceso = new ACCESO();
            acceso.IniciarTransaccion();
            try
            {
                new MAPPER_PAGO_PROVEEDOR(acceso).GuardarPago(pago);
                GestorOrden.CambiarEstado(pago.IdOrdenCompra, ORDEN_COMPRA_BLL.EstadoPagada, acceso);
                acceso.ConfirmarTransaccion();
            }
            catch
            {
                acceso.DeshacerTransaccion();
                pago.IdPago = 0;
                throw;
            }
            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Pago a proveedor",
                $"Orden {pago.IdOrdenCompra}: {FormatoMoneda.Pesos(pago.Monto)} ({pago.MedioPago}, comprobante {pago.NumeroComprobante})");
        }
    }
}
