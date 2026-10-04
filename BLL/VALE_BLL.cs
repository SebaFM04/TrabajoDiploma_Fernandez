using BE;
using DAL;
using SERVICIO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class VALE_BLL
    {
        // CU010 mensajes 40-45: vale de la venta, dentro de la transacción (VENTA 1 → 1 VALE)
        public BE.VALE GenerarVale(BE.VENTA venta, ACCESO acceso)
        {
            return new MAPPER_VALE(acceso).GuardarVale(new BE.VALE
            {
                IdVenta = venta.IdVenta,
                FechaHoraEmision = venta.Fecha,
                MontoCertificado = venta.Monto,
                Utilizado = false
            });
        }
    }
}
