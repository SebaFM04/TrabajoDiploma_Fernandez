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
        // Precio y costo de cada mes = actual deflactado por esta tasa mensual (TODO_N03, ítem 11)
        const decimal InflacionMensual = 0.03m;

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

        // Ventas por día de la semana (domingo a sábado), con promedio 1: viernes y sábados venden más
        static readonly double[] FactorDia = Normalizar(new[] { 0.8, 0.6, 0.6, 0.7, 1.1, 1.6, 1.8 });
        // Horario de 19 a 03, con el pico entre las 23 y la 01
        static readonly int[] Horas = { 19, 20, 21, 22, 23, 0, 1, 2 };
        static readonly double[] PesoHora = { 0.4, 0.6, 0.8, 1.0, 1.4, 1.5, 1.2, 0.6 };
        // Medios de pago, en el orden de VENTA_BLL.ListarMediosPago (Efectivo, Débito, Crédito, Transferencia, QR)
        static readonly double[] PesoMedioPago = { 25, 25, 15, 15, 20 };
        // Piqueos en el 35 % de las ventas (TODO: 30 a 40 %)
        const double ProbabilidadPiqueo = 0.35;

        static double[] Normalizar(double[] pesos)
        {
            double promedio = pesos.Average();
            return pesos.Select(p => p / promedio).ToArray();
        }

        // Estado de la simulación: cantidad de ventas simuladas y su rango de fechas
        public int ObtenerEstado(out DateTime? desde, out DateTime? hasta)
        {
            return new MAPPER_VENTA().ContarVentasSimuladas(out desde, out hasta);
        }

        // Genera la simulación y devuelve la cantidad de ventas. Las noches van desde 'meses' atrás hasta anteayer,
        // así la última venta (hasta las 03) queda en ayer y el tablero de hoy no cambia.
        // Todo en una transacción: un llamado al SP por mes (inserción en bloque).
        public int GenerarSimulacion(int meses, int ventasPorNoche, int semilla)
        {
            if (meses < MesesMinimo || meses > MesesMaximo)
                throw new ArgumentException("msgSimuladorMesesInvalidos");
            if (ventasPorNoche < VentasMinimo || ventasPorNoche > VentasMaximo)
                throw new ArgumentException("msgSimuladorVentasInvalidas");
            if (ObtenerEstado(out _, out _) > 0)
                throw new ArgumentException("msgSimuladorYaExiste");
            var productos = new VENTA_BLL().ListarProductosParaVenta();
            if (productos.Count == 0)
                throw new ArgumentException("msgSimuladorSinProductos");

            var costos = CalcularCostosActuales(productos);
            var bebidas = productos.Where(p => p.Producto.Tipo != "Piqueo").ToList();
            var piqueos = productos.Where(p => p.Producto.Tipo == "Piqueo").ToList();
            var mediosPago = new VENTA_BLL().ListarMediosPago();
            var rnd = new Random(semilla);

            DateTime primeraNoche = DateTime.Today.AddMonths(-meses);
            DateTime ultimaNoche = DateTime.Today.AddDays(-2);
            var ventas = new List<BE.VENTA>();
            for (DateTime noche = primeraNoche; noche <= ultimaNoche; noche = noche.AddDays(1))
            {
                double esperado = ventasPorNoche * FactorDia[(int)noche.DayOfWeek] * FactorMes(noche.Month) * (0.8 + 0.4 * rnd.NextDouble());
                int cantidad = (int)Math.Round(esperado);
                for (int i = 0; i < cantidad; i++)
                    ventas.Add(ArmarVenta(noche, rnd, bebidas, piqueos, costos, mediosPago));
            }

            int guardadas = 0;
            var acceso = new ACCESO();
            acceso.IniciarTransaccion();
            try
            {
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

            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Simulación de ventas generada",
                $"{guardadas} ventas simuladas del {primeraNoche:dd/MM/yyyy} al {ultimaNoche.AddDays(1):dd/MM/yyyy} ({meses} meses, {ventasPorNoche} por noche, semilla {semilla})");
            return guardadas;
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
                            Dictionary<string, decimal> costos, string[] mediosPago)
        {
            int indiceHora = Elegir(rnd, PesoHora);
            int hora = Horas[indiceHora];
            // Las horas de 0 a 2 son de la madrugada siguiente
            DateTime fecha = noche.Date.AddDays(hora < 12 ? 1 : 0).AddHours(hora).AddMinutes(rnd.Next(60)).AddSeconds(rnd.Next(60));
            // Meses entre la venta y el mes actual, para deflactar precio y costo
            int mesesAtras = (DateTime.Today.Year - fecha.Year) * 12 + DateTime.Today.Month - fecha.Month;
            decimal deflactor = 1 / (decimal)Math.Pow(1 + (double)InflacionMensual, mesesAtras);

            var venta = new BE.VENTA { Fecha = fecha, MedioPago = mediosPago[Elegir(rnd, PesoMedioPago)], Simulada = true };
            if (bebidas.Count > 0)
            {
                double r = rnd.NextDouble();
                int lineas = r < 0.55 ? 1 : r < 0.90 ? 2 : 3;
                for (int i = 0; i < lineas; i++)
                    AgregarLinea(venta, ElegirProducto(rnd, bebidas, fecha.Month), rnd, costos, deflactor);
            }
            if (piqueos.Count > 0 && (bebidas.Count == 0 || rnd.NextDouble() < ProbabilidadPiqueo))
                AgregarLinea(venta, ElegirProducto(rnd, piqueos, fecha.Month), rnd, costos, deflactor);
            venta.Monto = venta.Detalles.Sum(d => d.MontoLinea);
            return venta;
        }

        // Producto según popularidad y temporada; el tamaño más chico sale el 60 % de las veces
        static BE.PRODUCTO_TAMANIO ElegirProducto(Random rnd, List<BE.PRODUCTO_TAMANIO> opciones, int mes)
        {
            var porProducto = opciones.GroupBy(o => o.IdProducto).ToList();
            var pesos = porProducto.Select(g => (double)(Popularidad(g.First().Producto) * FactorTemporada(g.First().Producto, mes))).ToArray();
            var tamanios = porProducto[Elegir(rnd, pesos)].OrderBy(t => t.Tamanio.CantidadMagnitud).ToList();
            if (tamanios.Count == 1) return tamanios[0];
            return rnd.NextDouble() < 0.6 ? tamanios[0] : tamanios[1 + rnd.Next(tamanios.Count - 1)];
        }

        // Línea con precio y costo deflactados (precio redondeado a $ 10). Si el producto y tamaño ya está, suma la cantidad.
        static void AgregarLinea(BE.VENTA venta, BE.PRODUCTO_TAMANIO pt, Random rnd, Dictionary<string, decimal> costos, decimal deflactor)
        {
            double r = rnd.NextDouble();
            int cantidad = r < 0.85 ? 1 : r < 0.97 ? 2 : 3;
            decimal precio = Math.Round(pt.Precio * deflactor / 10, 0) * 10;
            decimal costo = costos[Clave(pt)] * deflactor;
            var linea = venta.Detalles.FirstOrDefault(d => d.IdProducto == pt.IdProducto && d.IdTamanio == pt.IdTamanio);
            if (linea == null)
            {
                linea = new BE.VENTA_DETALLE { IdProducto = pt.IdProducto, IdTamanio = pt.IdTamanio, ProductoTamanio = pt };
                venta.Detalles.Add(linea);
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
