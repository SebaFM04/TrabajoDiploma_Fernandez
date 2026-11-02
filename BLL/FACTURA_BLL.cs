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
    // CU015 Emitir Factura (DSS-CU015). Comprobante simulado, sin conexión con ARCA (decisión 11): Factura B a consumidor
    // final o Factura A a un cliente Responsable Inscripto (decisión 94).
    public class FACTURA_BLL
    {
        // Violación de UNIQUE (UQ_FACTURA_IdVenta): la venta ya tiene factura
        private const int ErrorClaveDuplicada = 2627;

        // Decisión 94: tipos de factura
        public const string TipoA = "A";
        public const string TipoB = "B";

        // CU015 mensajes 3-6 / FA1. En la Factura B los datos son opcionales (decisión 46): solo se valida el formato de
        // lo cargado. En la Factura A la razón social y un CUIT válido son obligatorios (decisión 94).
        public bool ValidarDatosCliente(string tipoFactura, string nombre, string cuit, string telefono, string correo)
        {
            if (tipoFactura != TipoA && tipoFactura != TipoB)
                throw new ArgumentException("msgFacturaTipoInvalido");
            if (tipoFactura == TipoA)
            {
                if (string.IsNullOrWhiteSpace(nombre))
                    throw new ArgumentException("msgFacturaRazonSocialObligatoria");
                if (string.IsNullOrWhiteSpace(cuit))
                    throw new ArgumentException("msgFacturaCuitObligatorio");
                if (!ValidarCuit(cuit))
                    throw new ArgumentException("msgFacturaCuitInvalido");
            }
            if (!string.IsNullOrWhiteSpace(nombre) && nombre.Trim().Length > 100)
                throw new ArgumentException("msgFacturaNombreInvalido");
            if (!string.IsNullOrWhiteSpace(telefono) && !Regex.IsMatch(telefono.Trim(), @"^[0-9+()\-\s]{6,30}$"))
                throw new ArgumentException("msgFacturaTelefonoInvalido");
            if (!string.IsNullOrWhiteSpace(correo) && (correo.Trim().Length > 100 || !Regex.IsMatch(correo.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$")))
                throw new ArgumentException("msgFacturaCorreoInvalido");
            return true;
        }

        // Decisión 94: CUIT de 11 dígitos (con o sin guiones) con el dígito verificador correcto (módulo 11)
        public bool ValidarCuit(string cuit)
        {
            string digitos = Regex.Replace(cuit ?? string.Empty, @"[\s-]", string.Empty);
            if (!Regex.IsMatch(digitos, @"^\d{11}$"))
                return false;
            int[] pesos = { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
            int suma = 0;
            for (int i = 0; i < 10; i++)
                suma += (digitos[i] - '0') * pesos[i];
            int verificador = 11 - suma % 11;
            if (verificador == 11) verificador = 0;
            return verificador != 10 && verificador == digitos[10] - '0';
        }

        // CUIT con el formato de los comprobantes: 30-12345678-9
        private static string FormatearCuit(string cuit)
        {
            string d = Regex.Replace(cuit, @"[\s-]", string.Empty);
            return $"{d.Substring(0, 2)}-{d.Substring(2, 8)}-{d.Substring(10, 1)}";
        }

        // CU015 mensajes 7-20. Número y alta en una transacción propia, después de la venta (decisión 42).
        // FA2: si falla, la venta queda registrada y la factura se puede reintentar.
        public BE.FACTURA EmitirFactura(BE.VENTA venta, string tipoFactura, string nombre, string cuit, string telefono, string correo)
        {
            if (venta == null || venta.IdVenta <= 0)
                throw new ArgumentException("msgFacturaVentaInexistente");
            ValidarDatosCliente(tipoFactura, nombre, cuit, telefono, correo);

            var factura = new BE.FACTURA
            {
                IdVenta = venta.IdVenta,
                FechaHoraEmision = DateTime.Now,
                NombreCliente = string.IsNullOrWhiteSpace(nombre) ? null : nombre.Trim(),
                TelefonoCliente = string.IsNullOrWhiteSpace(telefono) ? null : telefono.Trim(),
                CorreoCliente = string.IsNullOrWhiteSpace(correo) ? null : correo.Trim(),
                Total = venta.Monto,
                TipoFactura = tipoFactura,
                // En la Factura B el CUIT no se pide (consumidor final)
                CuitCliente = tipoFactura == TipoA ? FormatearCuit(cuit) : null
            };

            var acceso = new ACCESO();
            var mapper = new MAPPER_FACTURA(acceso);
            acceso.IniciarTransaccion();
            try
            {
                factura.NumeroComprobante = mapper.ObtenerProximoNumero(factura.TipoFactura);
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
                $"Factura {factura.TipoFactura} {factura.NumeroComprobante:00000000} de la venta {venta.IdVenta}: {factura.Total:0.00}");
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

        // Letra y número con punto de venta: B 0001-00000001 (cada letra tiene su numeración, decisión 94)
        public string NumeroCompleto(BE.FACTURA factura)
        {
            int puntoVenta = int.TryParse(Emisor("PuntoVenta", "1"), out int pv) ? pv : 1;
            return $"{factura.TipoFactura} {puntoVenta:0000}-{factura.NumeroComprobante:00000000}";
        }

        private static decimal Alicuota()
        {
            return decimal.TryParse(Emisor("AlicuotaIva", "21"), NumberStyles.Number, CultureInfo.InvariantCulture, out decimal a) ? a : 21m;
        }

        // IVA contenido en el total (Ley 27.743, régimen de transparencia fiscal al consumidor)
        public decimal CalcularIvaContenido(decimal total)
        {
            return Math.Round(total - total / (1 + Alicuota() / 100m), 2);
        }

        // Factura A (decisión 94): importe sin IVA de un precio o subtotal con IVA incluido
        public decimal CalcularNeto(decimal importeConIva)
        {
            return Math.Round(importeConIva / (1 + Alicuota() / 100m), 2);
        }

        // CU015 (decisiones 43 y 54, supuesto S4): PDF de la factura en Documentos\SistemaBar\Facturas. Factura B a
        // consumidor final con el IVA contenido; Factura A (decisión 94) con los datos del cliente y el IVA discriminado.
        // Devuelve la ruta.
        public string GenerarPdf(BE.FACTURA factura, BE.VENTA venta)
        {
            var ar = FormatoMoneda.Cultura;
            bool esA = factura.TipoFactura == TipoA;
            var encabezado = new List<string>
            {
                esA ? "Código 001 - ORIGINAL" : "Código 006 - ORIGINAL",
                $"{Emisor("RazonSocial", "Bar de Tragos SRL")} - {Emisor("Domicilio", "")}",
                $"CUIT: {Emisor("Cuit", "30-12345678-9")} - {Emisor("CondicionIva", "IVA Responsable Inscripto")}",
                $"Ingresos Brutos: {Emisor("IngresosBrutos", "-")} - Inicio de actividades: {Emisor("InicioActividades", "-")}",
                $"Comprobante N°: {NumeroCompleto(factura)} - Fecha: {factura.FechaHoraEmision.ToString("dd/MM/yyyy HH:mm", ar)}",
                esA ? $"Cliente: {factura.NombreCliente} - CUIT: {factura.CuitCliente} - IVA Responsable Inscripto"
                    : "Cliente: CONSUMIDOR FINAL" + (factura.NombreCliente != null ? $" - {factura.NombreCliente}" : ""),
            };
            if (factura.TelefonoCliente != null || factura.CorreoCliente != null)
                encabezado.Add($"Contacto: {factura.TelefonoCliente ?? ""} {factura.CorreoCliente ?? ""}".Trim());
            encabezado.Add($"Condición de venta: {venta.MedioPago} - Venta N° {venta.IdVenta} - Vale N° {venta.Vale?.IdVale}");

            // En la Factura A los precios y subtotales van sin IVA; el IVA se suma abajo
            var filas = new List<string[]> { esA
                ? new[] { "Producto", "Tamaño", "Cantidad", "Precio unit. (sin IVA)", "Subtotal (sin IVA)" }
                : new[] { "Producto", "Tamaño", "Cantidad", "Precio unit.", "Subtotal" } };
            decimal neto = 0;
            foreach (var d in venta.Detalles)
            {
                decimal precio = d.ProductoTamanio?.Precio ?? 0;
                decimal subtotal = esA ? CalcularNeto(d.MontoLinea) : d.MontoLinea;
                neto += subtotal;
                filas.Add(new[]
                {
                    // Propuesta 7 de la Fase 5: la línea con promoción se marca (el precio unitario es el cobrado)
                    (d.ProductoTamanio?.Producto?.Nombre ?? d.IdProducto.ToString()) + (d.IdPromocion.HasValue ? " (promo)" : ""),
                    d.ProductoTamanio?.Tamanio?.Nombre ?? d.IdTamanio.ToString(),
                    d.Cantidad.ToString(ar),
                    FormatoMoneda.Pesos(esA ? CalcularNeto(precio) : precio),
                    FormatoMoneda.Pesos(subtotal)
                });
            }
            var pie = esA
                ? new List<string>
                {
                    $"Neto gravado: {FormatoMoneda.Pesos(neto)}",
                    $"IVA {Alicuota().ToString("0.##", ar)} %: {FormatoMoneda.Pesos(factura.Total - neto)}",
                    $"TOTAL: {FormatoMoneda.Pesos(factura.Total)}"
                }
                : new List<string>
                {
                    $"TOTAL: {FormatoMoneda.Pesos(factura.Total)}",
                    $"Régimen de Transparencia Fiscal al Consumidor (Ley 27.743) - IVA contenido: {FormatoMoneda.Pesos(CalcularIvaContenido(factura.Total))}"
                };
            pie.Add("Comprobante simulado sin validez fiscal (sin CAE, sin conexión con ARCA).");

            // Si el PDF anterior está abierto en el visor, GeneradorPdf guarda una copia y devuelve esa ruta
            return GeneradorPdf.GenerarComprobante(GeneradorPdf.RutaFactura(factura.NumeroComprobante, factura.TipoFactura),
                esA ? "FACTURA A" : "FACTURA B", encabezado, filas, new[] { 0.3, 0.18, 0.1, 0.21, 0.21 }, pie, columnasTexto: 2);
        }
    }
}
