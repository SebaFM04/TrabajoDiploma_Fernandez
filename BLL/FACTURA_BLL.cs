using BE;
using DAL;
using SERVICIO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace BLL
{
    // CU015 Emitir Factura (DSS-CU015). Comprobante simulado a consumidor final, sin conexión con ARCA (decisión 11).
    public class FACTURA_BLL
    {
        // Violación de UNIQUE (UQ_FACTURA_IdVenta): la venta ya tiene factura
        private const int ErrorClaveDuplicada = 2627;

        // CU015 mensajes 3-6 / FA1. Los datos son opcionales (decisión 46): solo se valida el formato de lo cargado.
        public bool ValidarDatosCliente(string nombre, string telefono, string correo)
        {
            if (!string.IsNullOrWhiteSpace(nombre) && nombre.Trim().Length > 100)
                throw new ArgumentException("msgFacturaNombreInvalido");
            if (!string.IsNullOrWhiteSpace(telefono) && !Regex.IsMatch(telefono.Trim(), @"^[0-9+()\-\s]{6,30}$"))
                throw new ArgumentException("msgFacturaTelefonoInvalido");
            if (!string.IsNullOrWhiteSpace(correo) && (correo.Trim().Length > 100 || !Regex.IsMatch(correo.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$")))
                throw new ArgumentException("msgFacturaCorreoInvalido");
            return true;
        }

        // CU015 mensajes 7-20. Número y alta en una transacción propia, después de la venta (decisión 42).
        // FA2: si falla, la venta queda registrada y la factura se puede reintentar.
        public BE.FACTURA EmitirFactura(BE.VENTA venta, string nombre, string telefono, string correo)
        {
            if (venta == null || venta.IdVenta <= 0)
                throw new ArgumentException("msgFacturaVentaInexistente");
            ValidarDatosCliente(nombre, telefono, correo);

            var factura = new BE.FACTURA
            {
                IdVenta = venta.IdVenta,
                FechaHoraEmision = DateTime.Now,
                NombreCliente = string.IsNullOrWhiteSpace(nombre) ? null : nombre.Trim(),
                TelefonoCliente = string.IsNullOrWhiteSpace(telefono) ? null : telefono.Trim(),
                CorreoCliente = string.IsNullOrWhiteSpace(correo) ? null : correo.Trim(),
                Total = venta.Monto
            };

            var acceso = new ACCESO();
            var mapper = new MAPPER_FACTURA(acceso);
            acceso.IniciarTransaccion();
            try
            {
                factura.NumeroComprobante = mapper.ObtenerProximoNumero();
                mapper.GuardarFactura(factura);
                acceso.ConfirmarTransaccion();
            }
            catch (SqlException ex) when (ex.Number == ErrorClaveDuplicada && ex.Message.Contains("UQ_FACTURA_IdVenta"))
            {
                acceso.DeshacerTransaccion();
                // Condición del CU: la venta no debe tener una factura emitida previamente
                throw new ArgumentException("msgFacturaYaEmitida", ex);
            }
            catch
            {
                acceso.DeshacerTransaccion();
                throw;
            }

            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Emisión de factura",
                $"Factura {factura.NumeroComprobante:00000000} de la venta {venta.IdVenta}: {factura.Total:0.00}");
            return factura;
        }

        // CU015 (decisión 43, supuesto S4): PDF de la factura en Documentos\SistemaBar\Facturas. Devuelve la ruta.
        public string GenerarPdf(BE.FACTURA factura, BE.VENTA venta)
        {
            var ar = CultureInfo.GetCultureInfo("es-AR");
            var encabezado = new List<string>
            {
                "Comprobante simulado - sin validez fiscal",
                $"Número: {factura.NumeroComprobante:00000000}",
                $"Fecha: {factura.FechaHoraEmision.ToString("dd/MM/yyyy HH:mm", ar)}",
                $"Cliente: {factura.NombreCliente ?? "Consumidor final"}"
            };
            if (factura.TelefonoCliente != null) encabezado.Add($"Teléfono: {factura.TelefonoCliente}");
            if (factura.CorreoCliente != null) encabezado.Add($"Correo: {factura.CorreoCliente}");
            encabezado.Add($"Venta: {venta.IdVenta} - Vale: {venta.Vale?.IdVale}");

            var filas = new List<string[]> { new[] { "Producto", "Tamaño", "Cantidad", "Precio", "Subtotal" } };
            foreach (var d in venta.Detalles)
            {
                filas.Add(new[]
                {
                    d.ProductoTamanio?.Producto?.Nombre ?? d.IdProducto.ToString(),
                    d.ProductoTamanio?.Tamanio?.Nombre ?? d.IdTamanio.ToString(),
                    d.Cantidad.ToString(ar),
                    (d.ProductoTamanio?.Precio ?? 0).ToString("N2", ar),
                    d.MontoLinea.ToString("N2", ar)
                });
            }
            var pie = new List<string>
            {
                $"TOTAL: $ {factura.Total.ToString("N2", ar)}",
                $"Medio de pago: {venta.MedioPago}"
            };

            string ruta = GeneradorPdf.RutaFactura(factura.NumeroComprobante);
            GeneradorPdf.GenerarComprobante(ruta, "FACTURA C - Consumidor final", encabezado, filas,
                new[] { 0.36, 0.2, 0.12, 0.16, 0.16 }, pie, columnasTexto: 2);
            return ruta;
        }
    }
}
