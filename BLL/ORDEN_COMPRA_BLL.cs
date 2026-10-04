using BE;
using DAL;
using SERVICIO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    // N02: órdenes de compra (CU016 a CU019 y CU021)
    public class ORDEN_COMPRA_BLL
    {
        // Estados de ORDEN_COMPRA (decisión 49)
        public const string EstadoPendienteAprobacion = "Pendiente de aprobación";
        public const string EstadoAprobada = "Aprobada";
        public const string EstadoObservada = "Observada";
        public const string EstadoRecibidaParcialmente = "Recibida parcialmente";
        public const string EstadoCerrada = "Cerrada";
        public const string EstadoPagada = "Pagada";

        MAPPER_ORDEN_COMPRA GestorOrden = new MAPPER_ORDEN_COMPRA();
        MAPPER_PROVEEDOR GestorProveedor = new MAPPER_PROVEEDOR();
        INSUMO_BLL GestorInsumo = new INSUMO_BLL();

        public List<BE.ORDEN_COMPRA> ListarPorEstado(string estado)
        {
            return GestorOrden.ListarPorEstado(estado);
        }

        // Orden con sus líneas. ArgumentException si no existe.
        public BE.ORDEN_COMPRA ObtenerDetalle(int idOrdenCompra)
        {
            var orden = GestorOrden.BuscarOrden(idOrdenCompra);
            if (orden == null)
                throw new ArgumentException("msgOrdenInexistente");
            return orden;
        }

        // Condición de CU017/CU018/CU019/CU021: la orden tiene que estar en uno de los estados indicados
        private BE.ORDEN_COMPRA ObtenerEnEstado(int idOrdenCompra, params string[] estados)
        {
            var orden = ObtenerDetalle(idOrdenCompra);
            if (!estados.Contains(orden.Estado))
                throw new ArgumentException("msgOrdenEstadoInvalido");
            return orden;
        }

        // CU017 pasos 5 y 6 (mensajes 29-36): la orden pasa a "Aprobada" con la fecha de aprobación
        public void Aprobar(int idOrdenCompra)
        {
            var orden = ObtenerEnEstado(idOrdenCompra, EstadoPendienteAprobacion);
            GestorOrden.ActualizarEstado(idOrdenCompra, EstadoAprobada, DateTime.Now);
            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Orden de compra aprobada",
                $"Orden {idOrdenCompra} al proveedor {orden.Proveedor?.RazonSocial}");
        }

        // CU017 FA2 / FA3 (mensajes 19-28): la orden vuelve al Encargado como "Observada"; las observaciones son obligatorias
        public void Devolver(int idOrdenCompra, string observaciones)
        {
            if (string.IsNullOrWhiteSpace(observaciones))
                throw new ArgumentException("msgOrdenSinObservaciones");
            observaciones = observaciones.Trim();
            if (observaciones.Length > 500)
                throw new ArgumentException("msgOrdenObservacionesLargas");
            ObtenerEnEstado(idOrdenCompra, EstadoPendienteAprobacion);
            GestorOrden.ActualizarEstado(idOrdenCompra, EstadoObservada, null, observaciones);
            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Orden de compra observada",
                $"Orden {idOrdenCompra}: {observaciones}");
        }

        // CU016 paso 6 / FA3 y CU018 FA2. Las excepciones llevan la clave de idioma del mensaje.
        public bool ValidarOrden(BE.ORDEN_COMPRA orden)
        {
            var proveedor = GestorProveedor.ListarProveedores().FirstOrDefault(p => p.IdProveedor == orden.IdProveedor);
            if (proveedor == null)
                throw new ArgumentException("msgOrdenSinProveedor");
            if (orden.Detalles == null || orden.Detalles.Count == 0)
                throw new ArgumentException("msgOrdenSinInsumos");
            if (orden.Detalles.Any(d => d.CantidadPedida <= 0))
                throw new ArgumentException("msgOrdenCantidadInvalida");
            if (orden.Detalles.GroupBy(d => d.IdInsumo).Any(g => g.Count() > 1))
                throw new ArgumentException("msgOrdenInsumoRepetido");
            // Decisión 62: solo insumos activos que el proveedor ofrece
            var ofrecidos = GestorProveedor.ListarInsumosDeProveedor(orden.IdProveedor).Where(i => i.Activo).Select(i => i.IdInsumo).ToList();
            if (orden.Detalles.Any(d => !ofrecidos.Contains(d.IdInsumo)))
                throw new ArgumentException("msgOrdenInsumoNoOfrecido");
            return true;
        }

        // CU016 pasos 5 y 6 (mensajes 40-51): orden en "Pendiente de aprobación" y avisos asociados, en una transacción
        public BE.ORDEN_COMPRA GenerarOrden(BE.ORDEN_COMPRA orden)
        {
            ValidarOrden(orden);
            orden.FechaGeneracion = DateTime.Now;
            orden.Estado = EstadoPendienteAprobacion;

            var acceso = new ACCESO();
            acceso.IniciarTransaccion();
            try
            {
                new MAPPER_ORDEN_COMPRA(acceso).GuardarOrden(orden);
                GestorInsumo.AsociarAvisosAOrden(orden.Detalles.Select(d => d.IdInsumo).ToList(), orden.IdOrdenCompra, acceso);
                acceso.ConfirmarTransaccion();
            }
            catch
            {
                acceso.DeshacerTransaccion();
                orden.IdOrdenCompra = 0;
                throw;
            }
            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Orden de compra generada",
                $"Orden {orden.IdOrdenCompra} al proveedor {orden.IdProveedor} con {orden.Detalles.Count} insumos");
            return orden;
        }
    }
}
