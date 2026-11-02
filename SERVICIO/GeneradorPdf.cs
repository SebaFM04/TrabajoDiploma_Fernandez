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

        // Cada letra tiene su numeración (decisión 94): la B conserva el nombre de siempre y la A lleva la letra
        public static string RutaFactura(int numeroComprobante, string tipoFactura = "B")
        {
            string letra = tipoFactura == "B" ? string.Empty : tipoFactura + "_";
            return Path.Combine(CarpetaFacturas, $"Factura_{letra}{numeroComprobante:00000000}.pdf");
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

        // ── Informes de varias páginas (decisión 87) ──────────────────

        // Carpeta de los informes: Documentos\SistemaBar\Informes
        public static string CarpetaInformes
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SistemaBar", "Informes"); }
        }

        // Informe en A4 (apaisado por defecto) con un encabezado y secciones: cada sección puede tener título, líneas de
        // texto (se cortan en renglones), una tabla (Filas[0] es el encabezado, que se repite en cada página) y una
        // imagen PNG (se ajusta al ancho y pasa a la página siguiente si no entra). Numera las páginas.
        // Devuelve la ruta donde quedó guardado (puede ser otra si la original está en uso).
        public static string GenerarInforme(string ruta, string titulo, IList<string> encabezado, IList<SeccionInforme> secciones, bool apaisado = true)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ruta));
            var opciones = new XPdfFontOptions(PdfFontEncoding.Unicode);
            var fuenteTitulo = new XFont("Arial", 16, XFontStyle.Bold, opciones);
            var fuenteSeccion = new XFont("Arial", 12, XFontStyle.Bold, opciones);
            var fuente = new XFont("Arial", 9, XFontStyle.Regular, opciones);
            var fuenteNegrita = new XFont("Arial", 9, XFontStyle.Bold, opciones);
            const double margen = 40, renglon = 14;

            using (var documento = new PdfDocument())
            {
                documento.Info.Title = titulo;
                PdfPage pagina = null;
                XGraphics gfx = null;
                double ancho = 0, limite = 0, y = 0;

                Action nuevaPagina = () =>
                {
                    gfx?.Dispose();
                    pagina = documento.AddPage();
                    pagina.Size = PdfSharp.PageSize.A4;
                    if (apaisado) pagina.Orientation = PdfSharp.PageOrientation.Landscape;
                    gfx = XGraphics.FromPdfPage(pagina);
                    ancho = pagina.Width.Point - 2 * margen;
                    limite = pagina.Height.Point - margen - renglon;
                    y = margen;
                };
                // Si no entra 'alto' en lo que queda de la página, sigue en una nueva
                Action<double> reservar = alto => { if (y + alto > limite) nuevaPagina(); };

                nuevaPagina();
                gfx.DrawString(titulo, fuenteTitulo, XBrushes.Black, new XRect(margen, y, ancho, 24), XStringFormats.TopLeft);
                y += 30;
                foreach (string linea in encabezado)
                    foreach (string parte in Partir(gfx, linea, fuente, ancho))
                    {
                        reservar(renglon);
                        gfx.DrawString(parte, fuente, XBrushes.Black, new XRect(margen, y, ancho, renglon), XStringFormats.TopLeft);
                        y += renglon;
                    }
                y += 8;

                foreach (var seccion in secciones)
                {
                    // Tamaño de la imagen ajustada al ancho (y al alto de una página)
                    XImage imagen = seccion.Imagen == null ? null : XImage.FromStream(new MemoryStream(seccion.Imagen));
                    double anchoImagen = 0, altoImagen = 0;
                    if (imagen != null)
                    {
                        anchoImagen = Math.Min(ancho, imagen.PointWidth);
                        altoImagen = anchoImagen * imagen.PointHeight / imagen.PointWidth;
                        double altoMaximo = limite - margen - 30;
                        if (altoImagen > altoMaximo)
                        {
                            anchoImagen *= altoMaximo / altoImagen;
                            altoImagen = altoMaximo;
                        }
                    }
                    if (!string.IsNullOrEmpty(seccion.Titulo))
                    {
                        // El título no queda solo al final de una página: si la sección es solo una imagen, va con ella
                        bool soloImagen = imagen != null && seccion.Lineas.Count == 0 && (seccion.Filas == null || seccion.Filas.Count == 0);
                        reservar((soloImagen ? altoImagen : 3 * renglon) + 26);
                        y += 6;
                        gfx.DrawString(seccion.Titulo, fuenteSeccion, XBrushes.Black, new XRect(margen, y, ancho, 18), XStringFormats.TopLeft);
                        y += 20;
                    }
                    foreach (string linea in seccion.Lineas)
                        foreach (string parte in Partir(gfx, linea, fuente, ancho))
                        {
                            reservar(renglon);
                            gfx.DrawString(parte, fuente, XBrushes.Black, new XRect(margen, y, ancho, renglon), XStringFormats.TopLeft);
                            y += renglon;
                        }

                    if (seccion.Filas != null && seccion.Filas.Count > 0)
                    {
                        Action<string[], XFont> fila = (celdas, f) =>
                        {
                            double x = margen;
                            for (int c = 0; c < celdas.Length; c++)
                            {
                                double anchoColumna = ancho * seccion.Anchos[c];
                                var formato = c < seccion.ColumnasTexto ? XStringFormats.TopLeft : XStringFormats.TopRight;
                                gfx.DrawString(Ajustar(gfx, celdas[c] ?? "", f, anchoColumna - 4), f, XBrushes.Black, new XRect(x + 2, y, anchoColumna - 4, renglon), formato);
                                x += anchoColumna;
                            }
                            y += renglon + 2;
                        };
                        Action encabezadoTabla = () =>
                        {
                            fila(seccion.Filas[0], fuenteNegrita);
                            gfx.DrawLine(XPens.Black, margen, y - 2, margen + ancho, y - 2);
                        };
                        reservar(3 * renglon);
                        encabezadoTabla();
                        for (int f = 1; f < seccion.Filas.Count; f++)
                        {
                            if (y + renglon + 2 > limite)
                            {
                                // Tabla con paginado: el encabezado se repite en cada página
                                nuevaPagina();
                                encabezadoTabla();
                            }
                            fila(seccion.Filas[f], fuente);
                        }
                        gfx.DrawLine(XPens.Black, margen, y, margen + ancho, y);
                        y += 8;
                    }

                    if (imagen != null)
                    {
                        reservar(altoImagen);
                        gfx.DrawImage(imagen, margen, y, anchoImagen, altoImagen);
                        y += altoImagen + 10;
                        imagen.Dispose();
                    }
                }
                gfx.Dispose();

                // Número de página al pie de cada una
                for (int i = 0; i < documento.PageCount; i++)
                {
                    var p = documento.Pages[i];
                    using (var pie = XGraphics.FromPdfPage(p, XGraphicsPdfPageOptions.Append))
                        pie.DrawString($"{i + 1} / {documento.PageCount}", fuente, XBrushes.Gray,
                            new XRect(margen, p.Height.Point - margen + 4, p.Width.Point - 2 * margen, renglon), XStringFormats.TopRight);
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

        // Corta un texto en renglones que entran en 'ancho'
        static List<string> Partir(XGraphics gfx, string texto, XFont fuente, double ancho)
        {
            var renglones = new List<string>();
            foreach (string parrafo in (texto ?? "").Split('\n'))
            {
                string actual = "";
                foreach (string palabra in parrafo.Split(' '))
                {
                    string prueba = actual.Length == 0 ? palabra : actual + " " + palabra;
                    if (gfx.MeasureString(prueba, fuente).Width > ancho && actual.Length > 0)
                    {
                        renglones.Add(actual);
                        actual = palabra;
                    }
                    else
                        actual = prueba;
                }
                renglones.Add(actual);
            }
            return renglones;
        }

        // Recorta el texto de una celda para que no invada la columna siguiente
        static string Ajustar(XGraphics gfx, string texto, XFont fuente, double ancho)
        {
            if (gfx.MeasureString(texto, fuente).Width <= ancho) return texto;
            while (texto.Length > 1 && gfx.MeasureString(texto + "…", fuente).Width > ancho)
                texto = texto.Substring(0, texto.Length - 1);
            return texto + "…";
        }
    }

    // Sección de un informe de GeneradorPdf.GenerarInforme (todas las partes son opcionales)
    public class SeccionInforme
    {
        public string Titulo { get; set; }
        public List<string> Lineas { get; set; } = new List<string>();
        // Filas[0] = encabezado de la tabla; Anchos = proporción de cada columna (suman 1)
        public List<string[]> Filas { get; set; }
        public double[] Anchos { get; set; }
        // Las primeras ColumnasTexto van a la izquierda; el resto (números) a la derecha
        public int ColumnasTexto { get; set; } = 1;
        // Imagen PNG (por ejemplo, un gráfico)
        public byte[] Imagen { get; set; }
    }
}
