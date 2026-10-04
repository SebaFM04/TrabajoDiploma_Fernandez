using BE;
using DAL;
using SERVICIO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class COMANDA_BLL
    {
        // Estados de COMANDA (decisión 45)
        public const string EstadoPendiente = "Pendiente";
        public const string EstadoEntregada = "Entregada";

        // CU010 mensajes 46-51: comanda "Pendiente" para Cocina, dentro de la transacción. Solo si hay piqueos.
        public BE.COMANDA GenerarComanda(BE.VENTA venta, ACCESO acceso)
        {
            var comanda = new MAPPER_COMANDA(acceso).GuardarComanda(new BE.COMANDA
            {
                IdVenta = venta.IdVenta,
                FechaHoraEmision = venta.Fecha,
                Estado = EstadoPendiente
            });
            comanda.Detalles = venta.Detalles.Where(d => d.ProductoTamanio?.Producto?.Tipo == "Piqueo").ToList();
            return comanda;
        }

        // CU011 paso 1: comandas pendientes para Cocina
        public List<BE.COMANDA> ListarPendientes()
        {
            return new MAPPER_COMANDA().ListarPorEstado(EstadoPendiente);
        }

        // CU011 paso 2 (mensajes 2-8): detalle de la comanda (piqueos y cantidades)
        public BE.COMANDA ObtenerDetalle(int idComanda)
        {
            var comanda = new MAPPER_COMANDA().BuscarComanda(idComanda);
            if (comanda == null)
                throw new ArgumentException("msgComandaInexistente");
            return comanda;
        }

        // CU011 pasos 3 y 4 (mensajes 10-21). FA1: si ya estaba entregada, se cancela.
        public void ConfirmarEntrega(int idComanda)
        {
            var mapper = new MAPPER_COMANDA();
            var comanda = mapper.BuscarComanda(idComanda);
            if (comanda == null)
                throw new ArgumentException("msgComandaInexistente");
            if (comanda.Estado == EstadoEntregada)
                throw new ArgumentException("msgComandaYaEntregada");
            mapper.ActualizarEstado(idComanda, EstadoEntregada);

            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Entrega de piqueos",
                $"Comanda {idComanda} de la venta {comanda.IdVenta} entregada");
        }
    }
}
