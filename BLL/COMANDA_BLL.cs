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
    }
}
