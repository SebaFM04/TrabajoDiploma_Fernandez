using BE;
using DAL;
using SERVICIO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    // Simulador de ventas (decisiones 88 y 89): herramienta de soporte para la demo de N03, no es un CU de negocio.
    // Genera ventas de los meses anteriores con patrones realistas, marcadas como simuladas. Solo crea VENTA y
    // VENTA_DETALLE (sin vale, comanda ni factura) y no descuenta stock. Con la misma semilla genera lo mismo.
    public class SIMULADOR_BLL
    {
        public const int MesesMinimo = 1;
        public const int MesesMaximo = 24;
        public const int VentasMinimo = 1;
        public const int VentasMaximo = 200;
        // Ventas (mesas) promedio por noche abierta que propone la pantalla
        public const int VentasPorNocheDefecto = 40;
        // Costo de cada mes = actual deflactado por esta tasa mensual (ítem 11b: 2,5 %). Los precios de lista se
        // actualizan cada MesesEntreListas meses (escalones redondeados a $ 100): entre una lista y la siguiente el
        // margen se achica y se recupera con la actualización.
        const decimal InflacionMensual = 0.025m;
        const int MesesEntreListas = 2;

        // Perfil de venta de cada producto de la carta de demostración: popularidad relativa y factor en verano
        // (diciembre a febrero) e invierno (junio a agosto). Un producto que no figura se simula con valores neutros.
        class Perfil
        {
            public decimal Popularidad = 1, Verano = 1, Invierno = 1;
        }
        static readonly Dictionary<string, Perfil> Perfiles = new Dictionary<string, Perfil>(StringComparer.OrdinalIgnoreCase)
        {
            { "Fernet con Coca",     new Perfil { Popularidad = 1.4m, Verano = 0.85m, Invierno = 1.25m } },
            { "Mojito",              new Perfil { Popularidad = 0.9m, Verano = 1.3m,  Invierno = 0.7m } },
            { "Cuba Libre",          new Perfil { Popularidad = 0.6m } },
            { "Cerveza tirada",      new Perfil { Popularidad = 1.8m, Verano = 1.4m,  Invierno = 0.7m } },
            { "Gin tonic",           new Perfil { Popularidad = 1.0m, Verano = 1.3m,  Invierno = 0.8m } },
            { "Campari con naranja", new Perfil { Popularidad = 0.7m, Verano = 1.2m,  Invierno = 0.85m } },
            { "Vino por copa",       new Perfil { Popularidad = 0.7m, Verano = 0.7m,  Invierno = 1.4m } },
            { "Gaseosa",             new Perfil { Popularidad = 0.9m, Verano = 1.3m,  Invierno = 0.8m } },
            { "Agua mineral",        new Perfil { Popularidad = 0.6m, Verano = 1.3m,  Invierno = 0.8m } },
            { "Papas fritas",        new Perfil { Popularidad = 1.3m } },
            { "Picada",              new Perfil { Popularidad = 0.8m, Verano = 1.0m,  Invierno = 1.15m } },
            { "Nachos",              new Perfil { Popularidad = 0.9m } },
            { "Maní",                new Perfil { Popularidad = 0.35m } }
        };

        // Ítem 11b: el bar abre de miércoles a domingo (lunes y martes cerrado, y las noches del 24 y el 31 de diciembre).
        // Ventas por día de la semana (domingo a sábado), con promedio 1 entre las noches abiertas.
        static readonly double[] FactorDia = Normalizar(new[] { 0.8, 0, 0, 0.6, 0.9, 1.4, 1.6 });
        // Horario: domingo a jueves de 20 a 02; viernes y sábado de 21 a 04, con el pico entre las 00 y las 02
        static readonly int[] HorasSemana = { 20, 21, 22, 23, 0, 1 };
        static readonly double[] PesoHoraSemana = { 0.5, 0.9, 1.2, 1.3, 1.1, 0.6 };
        static readonly int[] HorasFinde = { 21, 22, 23, 0, 1, 2, 3 };
        static readonly double[] PesoHoraFinde = { 0.5, 0.9, 1.2, 1.5, 1.5, 1.0, 0.5 };
        // Medios de pago, en el orden de VENTA_BLL.ListarMediosPago (Efectivo, Débito, Crédito, Transferencia, QR)
        static readonly double[] PesoMedioPago = { 25, 25, 15, 15, 20 };
        // Cada venta es una mesa de 1 a 4 personas (30 / 40 / 20 / 10 %), con 1 bebida por persona (2 en el 35 % de los
        // casos). El piqueo sale en el 25 % de las mesas de 1 o 2 personas y en el 60 % de las de 3 o más.
        static readonly double[] PesoPersonas = { 30, 40, 20, 10 };
        const double ProbabilidadSegundaBebida = 0.35;
        const double PiqueoMesaChica = 0.25;
        const double PiqueoMesaGrande = 0.60;

        static double[] Normalizar(double[] pesos)
        {
            double promedio = pesos.Where(p => p > 0).Average();
            return pesos.Select(p => p / promedio).ToArray();
        }

        // Noches en que el bar está cerrado
        static bool Cerrado(DateTime noche)
        {
            return FactorDia[(int)noche.DayOfWeek] == 0 || (noche.Month == 12 && (noche.Day == 24 || noche.Day == 31));
        }

        // Estado de la simulación: cantidad de ventas simuladas y su rango de fechas
        public int ObtenerEstado(out DateTime? desde, out DateTime? hasta)
        {
            return new MAPPER_VENTA().ContarVentasSimuladas(out desde, out hasta);
        }

        // Promoción de ejemplo (decisión 80): 15 días que terminaron hace 60, 20 % de descuento y 1,8 veces más ventas
        // del producto. CU034 compara con el período anterior de igual duración: hacen falta al menos 4 meses simulados.
        public const int PromocionMesesMinimo = 4;
        const int DiasPromocion = 15;
        const int PromocionTerminaHaceDias = 60;
        const decimal DescuentoPromocion = 0.20m;
        const double AumentoPromocion = 1.8;

        // Promoción de ejemplo en armado: la promoción, su producto y las líneas de venta que la usan
        // (reciben el IdPromocion cuando se guarda la promoción, dentro de la transacción)
        class PromocionEjemplo
        {
            public BE.PROMOCION Promocion;
            public int IdProducto;
            public List<BE.VENTA_DETALLE> Lineas = new List<BE.VENTA_DETALLE>();

            public bool Vigente(DateTime fecha)
            {
                return fecha.Date >= Promocion.FechaDesde && fecha.Date <= Promocion.FechaHasta;
            }
        }

        // Genera la simulación y devuelve la cantidad de ventas. Las noches van desde 'meses' atrás hasta anteayer,
        // así la última venta (hasta las 03) queda en ayer y el tablero de hoy no cambia.
        // Con incluirPromocion crea además la promoción de ejemplo (Finalizada, sin evaluar), que se devuelve en 'promocion'.
        // Todo en una transacción: la promoción y un llamado al SP de ventas por mes (inserción en bloque).
        public int GenerarSimulacion(int meses, int ventasPorNoche, int semilla, bool incluirPromocion, out BE.PROMOCION promocion)
        {
            promocion = null;
            if (meses < MesesMinimo || meses > MesesMaximo)
                throw new ArgumentException("msgSimuladorMesesInvalidos");
            if (ventasPorNoche < VentasMinimo || ventasPorNoche > VentasMaximo)
                throw new ArgumentException("msgSimuladorVentasInvalidas");
            if (incluirPromocion && meses < PromocionMesesMinimo)
                throw new ArgumentException("msgSimuladorPromocionMeses");
            if (ObtenerEstado(out _, out _) > 0)
                throw new ArgumentException("msgSimuladorYaExiste");
            var productos = new VENTA_BLL().ListarProductosParaVenta();
            if (productos.Count == 0)
                throw new ArgumentException("msgSimuladorSinProductos");

            var costos = CalcularCostosActuales(productos);
            var bebidas = productos.Where(p => p.Producto.Tipo != "Piqueo").ToList();
            var piqueos = productos.Where(p => p.Producto.Tipo == "Piqueo").ToList();
            var mediosPago = new VENTA_BLL().ListarMediosPago();
            var ejemplo = incluirPromocion ? ArmarPromocionEjemplo(bebidas.Count > 0 ? bebidas : productos, costos) : null;
            var rnd = new Random(semilla);

            DateTime primeraNoche = DateTime.Today.AddMonths(-meses);
            DateTime ultimaNoche = DateTime.Today.AddDays(-2);
            var ventas = new List<BE.VENTA>();
            for (DateTime noche = primeraNoche; noche <= ultimaNoche; noche = noche.AddDays(1))
            {
                if (Cerrado(noche)) continue;
                // ±30 % por noche (clima, eventos) y la primera semana de enero más floja
                double esperado = ventasPorNoche * FactorDia[(int)noche.DayOfWeek] * FactorMes(noche.Month) * (0.7 + 0.6 * rnd.NextDouble())
                                  * (noche.Month == 1 && noche.Day <= 7 ? 0.7 : 1);
                int cantidad = (int)Math.Round(esperado);
                for (int i = 0; i < cantidad; i++)
                    ventas.Add(ArmarVenta(noche, rnd, bebidas, piqueos, costos, mediosPago, ejemplo));
            }

            int guardadas = 0;
            var acceso = new ACCESO();
            acceso.IniciarTransaccion();
            try
            {
                if (ejemplo != null)
                {
                    new MAPPER_PROMOCION(acceso).GuardarPromocion(ejemplo.Promocion);
                    foreach (var linea in ejemplo.Lineas)
                        linea.IdPromocion = ejemplo.Promocion.IdPromocion;
                }
                var mapper = new MAPPER_VENTA(acceso);
                foreach (var mes in ventas.GroupBy(v => new { v.Fecha.Year, v.Fecha.Month }))
                    guardadas += mapper.GuardarVentasSimuladas(mes.ToList());
                acceso.ConfirmarTransaccion();
            }
            catch
            {
                acceso.DeshacerTransaccion();
                throw;
            }

            promocion = ejemplo?.Promocion;
            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Simulación de ventas generada",
                $"{guardadas} ventas simuladas del {primeraNoche:dd/MM/yyyy} al {ultimaNoche.AddDays(1):dd/MM/yyyy} ({meses} meses, {ventasPorNoche} por noche, semilla {semilla})"
                + (promocion != null ? $"; promoción de ejemplo {promocion.IdPromocion} ({promocion.Nombre}, {promocion.FechaDesde:dd/MM/yyyy} al {promocion.FechaHasta:dd/MM/yyyy})" : ""));
            return guardadas;
        }

        // Promoción de ejemplo (propuesta 2 de la Fase 3): el producto de mayor margen unitario entre los que se venden
        // poco (popularidad hasta 1), con todos sus tamaños a 20 % menos del precio de ese mes, criterio Ventas,
        // Finalizada y sin evaluar (para CU034).
        PromocionEjemplo ArmarPromocionEjemplo(List<BE.PRODUCTO_TAMANIO> opciones, Dictionary<string, decimal> costos)
        {
            var candidatos = opciones.Where(o => Popularidad(o.Producto) <= 1).ToList();
            if (candidatos.Count == 0) candidatos = opciones;
            var elegido = candidatos.OrderByDescending(o => o.Precio - costos[Clave(o)]).First();

            DateTime hasta = DateTime.Today.AddDays(-PromocionTerminaHaceDias);
            DateTime desde = hasta.AddDays(-(DiasPromocion - 1));
            var promocion = new BE.PROMOCION
            {
                Nombre = "Promo de ejemplo: " + elegido.Producto.Nombre,
                Criterio = PROMOCION_BLL.CriterioVentas,
                FechaDesde = desde,
                FechaHasta = hasta,
                Estado = PROMOCION_BLL.EstadoFinalizada,
                FechaCreacion = desde.AddDays(-3).AddHours(18),
                Observaciones = "Generada por el simulador de ventas",
                Simulada = true
            };
            foreach (var pt in opciones.Where(o => o.IdProducto == elegido.IdProducto))
            {
                // Precio de la lista vigente al empezar la promoción, con un 20 % de descuento (redondeado a $ 100)
                decimal original = PrecioLista(pt.Precio, desde);
                promocion.Detalles.Add(new BE.PROMOCION_DETALLE
                {
                    IdProducto = pt.IdProducto,
                    IdTamanio = pt.IdTamanio,
                    PrecioOriginal = original,
                    PrecioPromocional = Math.Max(100, Math.Round(original * (1 - DescuentoPromocion) / 100, 0) * 100),
                    CostoUnitario = Math.Round(costos[Clave(pt)] * Deflactor(desde), 2),
                    ProductoTamanio = pt
                });
            }
            return new PromocionEjemplo { Promocion = promocion, IdProducto = elegido.IdProducto };
        }

        static int MesesAtras(DateTime fecha)
        {
            return (DateTime.Today.Year - fecha.Year) * 12 + DateTime.Today.Month - fecha.Month;
        }

        // Costo del mes de 'fecha' = actual deflactado por los meses que pasaron hasta hoy (sube todos los meses)
        static decimal Deflactor(DateTime fecha)
        {
            return 1 / (decimal)Math.Pow(1 + (double)InflacionMensual, MesesAtras(fecha));
        }

        // Ítem 11b: precio de la lista vigente en 'fecha'. La lista actual es la de este mes y se actualiza cada
        // MesesEntreListas meses: un mes que quedó entre dos actualizaciones usa la lista anterior. Redondeado a $ 100.
        static decimal PrecioLista(decimal precioActual, DateTime fecha)
        {
            int mesesLista = (MesesAtras(fecha) + MesesEntreListas - 1) / MesesEntreListas * MesesEntreListas;
            decimal precio = precioActual / (decimal)Math.Pow(1 + (double)InflacionMensual, mesesLista);
            return Math.Max(100, Math.Round(precio / 100, 0) * 100);
        }

        // Decisión 89: borra físicamente las ventas simuladas. FA: no hay simulación.
        public int BorrarSimulacion()
        {
            if (ObtenerEstado(out _, out _) == 0)
                throw new ArgumentException("msgSimuladorNadaParaBorrar");
            int borradas = new MAPPER_VENTA().BorrarVentasSimuladas();
            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Simulación de ventas borrada",
                $"{borradas} ventas simuladas borradas");
            return borradas;
        }

        // Ventas generales del mes: algo más en verano y en las vacaciones de invierno
        static double FactorMes(int mes)
        {
            switch (mes)
            {
                case 12: return 1.15;
                case 1: case 2: return 1.1;
                case 7: return 1.05;
                default: return 1.0;
            }
        }

        // Factor de temporada del producto en el mes. Marzo, mayo, septiembre y noviembre son de transición.
        static decimal FactorTemporada(BE.PRODUCTO producto, int mes)
        {
            if (!Perfiles.TryGetValue(producto.Nombre, out var p)) return 1;
            switch (mes)
            {
                case 12: case 1: case 2: return p.Verano;
                case 3: case 11: return (1 + p.Verano) / 2;
                case 6: case 7: case 8: return p.Invierno;
                case 5: case 9: return (1 + p.Invierno) / 2;
                default: return 1;
            }
        }

        static decimal Popularidad(BE.PRODUCTO producto)
        {
            return Perfiles.TryGetValue(producto.Nombre, out var p) ? p.Popularidad : 1;
        }

        // Costo actual de una unidad de cada producto y tamaño: receta escalada × costo por unidad de medida del insumo
        // (CostoUnidadCompra / EquivalenciaMagnitud, decisión 73). La clave es "IdProducto-IdTamanio".
        Dictionary<string, decimal> CalcularCostosActuales(List<BE.PRODUCTO_TAMANIO> productos)
        {
            var insumos = new INSUMO_BLL().ListarInsumos(INSUMO_BLL.UsoTodos).ToDictionary(i => i.IdInsumo);
            var gestorReceta = new RECETA_BLL();
            var costos = new Dictionary<string, decimal>();
            foreach (var pt in productos)
            {
                decimal costo = 0;
                foreach (var r in gestorReceta.ObtenerRecetaEscalada(pt.IdProducto, pt.Tamanio.CantidadMagnitud, 1))
                    if (insumos.TryGetValue(r.IdInsumo, out var insumo) && insumo.EquivalenciaMagnitud > 0)
                        costo += r.Proporcion * insumo.CostoUnidadCompra / insumo.EquivalenciaMagnitud;
                costos[Clave(pt)] = costo;
            }
            return costos;
        }

        static string Clave(BE.PRODUCTO_TAMANIO pt)
        {
            return pt.IdProducto + "-" + pt.IdTamanio;
        }

        BE.VENTA ArmarVenta(DateTime noche, Random rnd, List<BE.PRODUCTO_TAMANIO> bebidas, List<BE.PRODUCTO_TAMANIO> piqueos,
                            Dictionary<string, decimal> costos, string[] mediosPago, PromocionEjemplo ejemplo)
        {
            // Viernes y sábado cierran más tarde; las horas de 0 a 3 son de la madrugada siguiente
            bool finde = noche.DayOfWeek == DayOfWeek.Friday || noche.DayOfWeek == DayOfWeek.Saturday;
            int hora = finde ? HorasFinde[Elegir(rnd, PesoHoraFinde)] : HorasSemana[Elegir(rnd, PesoHoraSemana)];
            DateTime fecha = noche.Date.AddDays(hora < 12 ? 1 : 0).AddHours(hora).AddMinutes(rnd.Next(60)).AddSeconds(rnd.Next(60));
            decimal deflactor = Deflactor(fecha);
            // Durante la vigencia de la promoción de ejemplo su producto se vende más y al precio promocional
            var promo = ejemplo != null && ejemplo.Vigente(fecha) ? ejemplo : null;

            var venta = new BE.VENTA { Fecha = fecha, MedioPago = mediosPago[Elegir(rnd, PesoMedioPago)], Simulada = true };
            int personas = 1 + Elegir(rnd, PesoPersonas);
            if (bebidas.Count > 0)
            {
                for (int p = 0; p < personas; p++)
                {
                    int tragos = rnd.NextDouble() < ProbabilidadSegundaBebida ? 2 : 1;
                    for (int t = 0; t < tragos; t++)
                        AgregarLinea(venta, ElegirProducto(rnd, bebidas, fecha.Month, promo?.IdProducto), 1, costos, deflactor, promo);
                }
            }
            double probabilidadPiqueo = personas >= 3 ? PiqueoMesaGrande : PiqueoMesaChica;
            if (piqueos.Count > 0 && (bebidas.Count == 0 || rnd.NextDouble() < probabilidadPiqueo))
                AgregarLinea(venta, ElegirProducto(rnd, piqueos, fecha.Month, promo?.IdProducto), 1, costos, deflactor, promo);
            venta.Monto = venta.Detalles.Sum(d => d.MontoLinea);
            return venta;
        }

        // Producto según popularidad y temporada (y el de la promoción vigente, 1,8 veces más); el tamaño más chico
        // sale el 60 % de las veces
        static BE.PRODUCTO_TAMANIO ElegirProducto(Random rnd, List<BE.PRODUCTO_TAMANIO> opciones, int mes, int? idProductoPromocion)
        {
            var porProducto = opciones.GroupBy(o => o.IdProducto).ToList();
            var pesos = porProducto.Select(g => (double)(Popularidad(g.First().Producto) * FactorTemporada(g.First().Producto, mes))
                                                * (g.Key == idProductoPromocion ? AumentoPromocion : 1)).ToArray();
            var tamanios = porProducto[Elegir(rnd, pesos)].OrderBy(t => t.Tamanio.CantidadMagnitud).ToList();
            if (tamanios.Count == 1) return tamanios[0];
            return rnd.NextDouble() < 0.6 ? tamanios[0] : tamanios[1 + rnd.Next(tamanios.Count - 1)];
        }

        // Línea con el precio de la lista vigente y el costo del mes. Si el producto y tamaño ya está, suma la cantidad.
        // Con la promoción de ejemplo vigente, su producto se cobra al precio promocional y la línea la referencia.
        static void AgregarLinea(BE.VENTA venta, BE.PRODUCTO_TAMANIO pt, int cantidad, Dictionary<string, decimal> costos, decimal deflactor, PromocionEjemplo promo)
        {
            decimal precio = PrecioLista(pt.Precio, venta.Fecha);
            var enPromocion = promo?.Promocion.Detalles.FirstOrDefault(d => d.IdProducto == pt.IdProducto && d.IdTamanio == pt.IdTamanio);
            if (enPromocion != null)
                precio = enPromocion.PrecioPromocional;
            decimal costo = costos[Clave(pt)] * deflactor;
            var linea = venta.Detalles.FirstOrDefault(d => d.IdProducto == pt.IdProducto && d.IdTamanio == pt.IdTamanio);
            if (linea == null)
            {
                linea = new BE.VENTA_DETALLE { IdProducto = pt.IdProducto, IdTamanio = pt.IdTamanio, ProductoTamanio = pt };
                venta.Detalles.Add(linea);
                if (enPromocion != null)
                    promo.Lineas.Add(linea);
            }
            linea.Cantidad += cantidad;
            linea.MontoLinea = precio * linea.Cantidad;
            linea.CostoLinea = Math.Round(costo * linea.Cantidad, 2);
        }

        // Índice elegido al azar según los pesos
        static int Elegir(Random rnd, double[] pesos)
        {
            double valor = rnd.NextDouble() * pesos.Sum();
            for (int i = 0; i < pesos.Length; i++)
            {
                valor -= pesos[i];
                if (valor < 0) return i;
            }
            return pesos.Length - 1;
        }
    }
}
