using BE;
using DAL;
using SERVICIO;
using System;
using System.Collections.Generic;
using System.Configuration;
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

        // Consulta de ventas: PDF de una factura ya emitida. Se regenera siempre desde la base,
        // así el archivo nunca queda desactualizado (por ejemplo, si se recreó la base y se repite el número).
        public string ObtenerPdf(BE.VENTA venta)
        {
            if (venta?.Factura == null)
                throw new ArgumentException("msgConsultaSeleccionarVenta");
            if (venta.Detalles == null || venta.Detalles.Count == 0)
                venta.Detalles = new VENTA_BLL().ObtenerDetalleVenta(venta.IdVenta);
            return GenerarPdf(venta.Factura, venta);
        }

        // Datos del emisor (App.config, decisión 54). Si falta una clave se usa el valor por defecto.
        private static string Emisor(string clave, string porDefecto)
        {
            string valor = ConfigurationManager.AppSettings["Emisor." + clave];
            return string.IsNullOrWhiteSpace(valor) ? porDefecto : valor.Trim();
        }

        // Número con punto de venta: 0001-00000001
        public string NumeroCompleto(BE.FACTURA factura)
        {
            int puntoVenta = int.TryParse(Emisor("PuntoVenta", "1"), out int pv) ? pv : 1;
            return $"{puntoVenta:0000}-{factura.NumeroComprobante:00000000}";
        }

        // IVA contenido en el total (Ley 27.743, régimen de transparencia fiscal al consumidor)
        public decimal CalcularIvaContenido(decimal total)
        {
            decimal alicuota = decimal.TryParse(Emisor("AlicuotaIva", "21"), NumberStyles.Number, CultureInfo.InvariantCulture, out decimal a) ? a : 21m;
            return Math.Round(total - total / (1 + alicuota / 100m), 2);
        }

        // CU015 (decisiones 43 y 54, supuesto S4): PDF de la Factura B a consumidor final en Documentos\SistemaBar\Facturas.
        // Devuelve la ruta.
        public string GenerarPdf(BE.FACTURA factura, BE.VENTA venta)
        {
            var ar = FormatoMoneda.Cultura;
            var encabezado = new List<string>
            {
                "Código 006 - ORIGINAL",
                $"{Emisor("RazonSocial", "Bar de Tragos SRL")} - {Emisor("Domicilio", "")}",
                $"CUIT: {Emisor("Cuit", "30-12345678-9")} - {Emisor("CondicionIva", "IVA Responsable Inscripto")}",
                $"Ingresos Brutos: {Emisor("IngresosBrutos", "-")} - Inicio de actividades: {Emisor("InicioActividades", "-")}",
                $"Comprobante N°: {NumeroCompleto(factura)} - Fecha: {factura.FechaHoraEmision.ToString("dd/MM/yyyy HH:mm", ar)}",
                "Cliente: CONSUMIDOR FINAL" + (factura.NombreCliente != null ? $" - {factura.NombreCliente}" : ""),
            };
            if (factura.TelefonoCliente != null || factura.CorreoCliente != null)
                encabezado.Add($"Contacto: {factura.TelefonoCliente ?? ""} {factura.CorreoCliente ?? ""}".Trim());
            encabezado.Add($"Condición de venta: {venta.MedioPago} - Venta N° {venta.IdVenta} - Vale N° {venta.Vale?.IdVale}");

            var filas = new List<string[]> { new[] { "Producto", "Tamaño", "Cantidad", "Precio unit.", "Subtotal" } };
            foreach (var d in venta.Detalles)
            {
                filas.Add(new[]
                {
                    d.ProductoTamanio?.Producto?.Nombre ?? d.IdProducto.ToString(),
                    d.ProductoTamanio?.Tamanio?.Nombre ?? d.IdTamanio.ToString(),
                    d.Cantidad.ToString(ar),
                    FormatoMoneda.Pesos(d.ProductoTamanio?.Precio ?? 0),
                    FormatoMoneda.Pesos(d.MontoLinea)
                });
            }
            var pie = new List<string>
            {
                $"TOTAL: {FormatoMoneda.Pesos(factura.Total)}",
                $"Régimen de Transparencia Fiscal al Consumidor (Ley 27.743) - IVA contenido: {FormatoMoneda.Pesos(CalcularIvaContenido(factura.Total))}",
                "Comprobante simulado sin validez fiscal (sin CAE, sin conexión con ARCA)."
            };

            // Si el PDF anterior está abierto en el visor, GeneradorPdf guarda una copia y devuelve esa ruta
            return GeneradorPdf.GenerarComprobante(GeneradorPdf.RutaFactura(factura.NumeroComprobante), "FACTURA B", encabezado, filas,
                new[] { 0.34, 0.2, 0.12, 0.17, 0.17 }, pie, columnasTexto: 2);
        }
    }
}
