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

        // CU012 paso 1 (mensajes 2-9): el vale con las bebidas de su venta si es válido;
        // null si no existe o ya fue utilizado (FA1). Detalles vacío = venta solo de piqueos (FA2).
        public BE.VALE ValidarVale(int idVale)
        {
            var vale = new MAPPER_VALE().BuscarVale(idVale);
            if (vale == null || vale.Utilizado) return null;
            return vale;
        }

        // CU012 paso 5 (mensajes 18-23): marca el vale como utilizado. No descuenta stock (se descontó en CU010)
        // ni depende de la comanda (decisión 12).
        public void MarcarUtilizado(int idVale)
        {
            var vale = ValidarVale(idVale);
            if (vale == null)
                throw new ArgumentException("msgValeInvalido");
            // FA2 (decisión 13): sin bebidas no se marca
            if (vale.Detalles.Count == 0)
                throw new ArgumentException("msgValeSinBebidas");
            // FA1: otro usuario lo marcó entre la validación y la confirmación
            if (new MAPPER_VALE().ActualizarUtilizado(idVale) == 0)
                throw new ArgumentException("msgValeInvalido");

            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Entrega de bebidas",
                $"Vale {idVale} de la venta {vale.IdVenta} utilizado");
        }
    }
}
