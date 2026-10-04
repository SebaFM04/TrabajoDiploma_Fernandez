using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SERVICIO
{
    // Servicio genérico para generar un comprobante en PDF (PDFsharp, decisión 43). No tiene lógica de negocio:
    // recibe el título, las líneas de encabezado, una tabla y las líneas del pie ya armadas.
    public static class GeneradorPdf
    {
        // Carpeta de las facturas (supuesto S4): Documentos\SistemaBar\Facturas
        public static string CarpetaFacturas
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SistemaBar", "Facturas"); }
        }

        public static string RutaFactura(int numeroComprobante)
        {
            return Path.Combine(CarpetaFacturas, $"Factura_{numeroComprobante:00000000}.pdf");
        }

        // filas[0] es el encabezado de la tabla. anchos: proporción de cada columna (suman 1).
        // Las primeras columnasTexto van a la izquierda; el resto (números) a la derecha.
        // Devuelve la ruta donde quedó guardado (puede ser otra si la original está en uso).
        public static string GenerarComprobante(string ruta, string titulo, IList<string> encabezado, IList<string[]> filas, double[] anchos, IList<string> pie, int columnasTexto = 1)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ruta));
            var opciones = new XPdfFontOptions(PdfFontEncoding.Unicode);
            var fuenteTitulo = new XFont("Arial", 16, XFontStyle.Bold, opciones);
            var fuente = new XFont("Arial", 10, XFontStyle.Regular, opciones);
            var fuenteNegrita = new XFont("Arial", 10, XFontStyle.Bold, opciones);

            using (var documento = new PdfDocument())
            {
                documento.Info.Title = titulo;
                PdfPage pagina = documento.AddPage();
                pagina.Size = PdfSharp.PageSize.A4;
                using (XGraphics gfx = XGraphics.FromPdfPage(pagina))
                {
                    double margen = 50, ancho = pagina.Width.Point - 2 * margen, y = margen;

                    gfx.DrawString(titulo, fuenteTitulo, XBrushes.Black, new XRect(margen, y, ancho, 24), XStringFormats.TopLeft);
                    y += 34;
                    foreach (string linea in encabezado)
                    {
                        gfx.DrawString(linea, fuente, XBrushes.Black, new XRect(margen, y, ancho, 14), XStringFormats.TopLeft);
                        y += 16;
                    }
                    y += 8;

                    for (int f = 0; f < filas.Count; f++)
                    {
                        double x = margen;
                        for (int c = 0; c < filas[f].Length; c++)
                        {
                            double anchoColumna = ancho * anchos[c];
                            var formato = c < columnasTexto ? XStringFormats.TopLeft : XStringFormats.TopRight;
                            gfx.DrawString(filas[f][c] ?? "", f == 0 ? fuenteNegrita : fuente, XBrushes.Black,
                                new XRect(x + 2, y, anchoColumna - 4, 14), formato);
                            x += anchoColumna;
                        }
                        y += 16;
                        if (f == 0) gfx.DrawLine(XPens.Black, margen, y - 2, margen + ancho, y - 2);
                    }
                    gfx.DrawLine(XPens.Black, margen, y + 2, margen + ancho, y + 2);
                    y += 10;

                    foreach (string linea in pie)
                    {
                        gfx.DrawString(linea, fuenteNegrita, XBrushes.Black, new XRect(margen, y, ancho, 14), XStringFormats.TopLeft);
                        y += 16;
                    }
                }
                try
                {
                    documento.Save(ruta);
                }
                catch (IOException)
                {
                    // El archivo está abierto en otro programa (por ejemplo, el visor de PDF): se guarda una copia aparte
                    ruta = Path.Combine(Path.GetDirectoryName(ruta),
                        Path.GetFileNameWithoutExtension(ruta) + "_" + DateTime.Now.ToString("HHmmss") + Path.GetExtension(ruta));
                    documento.Save(ruta);
                }
            }
            return ruta;
        }
    }
}
