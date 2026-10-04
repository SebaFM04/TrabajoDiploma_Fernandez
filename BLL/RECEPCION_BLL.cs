using BE;
using DAL;
using SERVICIO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    // N02 CU019 Registrar Recepción de Mercadería (DSS-CU019)
    public class RECEPCION_BLL
    {
        ORDEN_COMPRA_BLL GestorOrden = new ORDEN_COMPRA_BLL();
        INSUMO_BLL GestorInsumo = new INSUMO_BLL();
        RECLAMO_BLL GestorReclamo = new RECLAMO_BLL();

        // CU019 paso 6 / FA2 (mensajes 20-23). Las líneas con cantidad 0 no se registran.
        public bool ValidarRecepcion(BE.RECEPCION recepcion)
        {
            var orden = GestorOrden.ObtenerDetalle(recepcion.IdOrdenCompra);
            if (orden.Estado != ORDEN_COMPRA_BLL.EstadoAprobada && orden.Estado != ORDEN_COMPRA_BLL.EstadoRecibidaParcialmente)
                throw new ArgumentException("msgOrdenEstadoInvalido");
            recepcion.NumeroRemito = (recepcion.NumeroRemito ?? string.Empty).Trim();
            recepcion.NumeroFacturaProveedor = (recepcion.NumeroFacturaProveedor ?? string.Empty).Trim();
            if (recepcion.NumeroRemito.Length == 0 || recepcion.NumeroFacturaProveedor.Length == 0)
                throw new ArgumentException("msgRecepcionSinComprobantes");
            if (recepcion.NumeroRemito.Length > 30 || recepcion.NumeroFacturaProveedor.Length > 30)
                throw new ArgumentException("msgRecepcionComprobanteLargo");
            recepcion.Detalles = recepcion.Detalles.Where(d => d.CantidadRecibida != 0).ToList();
            if (recepcion.Detalles.Count == 0)
                throw new ArgumentException("msgRecepcionSinCantidades");
            foreach (var d in recepcion.Detalles)
            {
                var linea = orden.Detalles.FirstOrDefault(x => x.IdInsumo == d.IdInsumo);
                // FA2: cantidad mayor a lo pendiente (o negativa) y costo menor o igual a cero
                if (linea == null || d.CantidadRecibida < 0 || d.CantidadRecibida > linea.CantidadPendiente)
                    throw new ArgumentException("msgRecepcionCantidadInvalida");
                if (d.CostoUnidadCompra <= 0)
                    throw new ArgumentException("msgRecepcionCostoInvalido");
            }
            return true;
        }

        // CU019 pasos 5 y 6 (mensajes 24-58), en una sola transacción: recepción, stock y costo, avisos (decisión 59),
        // cantidades y estado de la orden y, si se cerró, resolución de reclamos (decisión 60).
        // Devuelve el estado resultante: "Cerrada" o "Recibida parcialmente" (FA3: se abre CU020).
        public string RegistrarRecepcion(BE.RECEPCION recepcion)
        {
            ValidarRecepcion(recepcion);
            var orden = GestorOrden.ObtenerDetalle(recepcion.IdOrdenCompra);
            recepcion.FechaRecepcion = DateTime.Now;
            string estado;

            var acceso = new ACCESO();
            acceso.IniciarTransaccion();
            try
            {
                new MAPPER_RECEPCION(acceso).GuardarRecepcion(recepcion);
                GestorInsumo.IngresarStock(recepcion.Detalles, acceso);
                estado = GestorOrden.ActualizarPorRecepcion(orden, recepcion.Detalles, acceso);
                if (estado == ORDEN_COMPRA_BLL.EstadoCerrada)
                    GestorReclamo.ResolverReclamos(orden.IdOrdenCompra, acceso);
                acceso.ConfirmarTransaccion();
            }
            catch
            {
                acceso.DeshacerTransaccion();
                recepcion.IdRecepcion = 0;
                throw;
            }
            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Recepción de mercadería",
                $"Orden {orden.IdOrdenCompra}, remito {recepcion.NumeroRemito}, factura {recepcion.NumeroFacturaProveedor}: {estado}");
            return estado;
        }
    }
}
