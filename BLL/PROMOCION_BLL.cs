using BE;
using DAL;
using SERVICIO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    // N03: promociones (decisiones 74 a 82). CU033, CU034 y CU035 se suman en las fases 5 y 6.
    public class PROMOCION_BLL
    {
        MAPPER_PROMOCION GestorPromocion = new MAPPER_PROMOCION();

        // Estados de documento del diagrama de actividad (decisión 78, CK_PROMOCION_Estado)
        public const string EstadoEnProceso = "En proceso";
        public const string EstadoVigente = "Vigente";
        public const string EstadoFinalizada = "Finalizada";
        public const string EstadoRenovada = "Renovada";

        // Criterios del análisis de rentabilidad (decisión 75, CK_PROMOCION_Criterio)
        public const string CriterioVencimiento = "Vencimiento";
        public const string CriterioEstacionalidad = "Estacionalidad";
        public const string CriterioVentas = "Ventas";
        public const string CriterioRotacion = "Rotación";

        // Resultado de CU034 (decisión 81, CK_PROMOCION_Resultado)
        public const string ResultadoEfectiva = "Efectiva";
        public const string ResultadoNoEfectiva = "No efectiva";

        public string[] ListarEstados()
        {
            return new[] { EstadoEnProceso, EstadoVigente, EstadoFinalizada, EstadoRenovada };
        }

        public string[] ListarCriterios()
        {
            return new[] { CriterioVencimiento, CriterioEstacionalidad, CriterioVentas, CriterioRotacion };
        }

        // Decisión 78: En proceso → Vigente al empezar la vigencia; En proceso o Vigente → Finalizada al terminar.
        // Se llama al abrir el menú (tablero) y los formularios de promociones y de venta. Devuelve cuántas cambiaron;
        // solo entonces se registra en la bitácora.
        public int ActualizarEstados()
        {
            int cambios = GestorPromocion.ActualizarEstados(DateTime.Today);
            if (cambios > 0 && SessionManager.Instancia.UsuarioActual != null)
                new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Estados de promociones actualizados",
                    $"{cambios} promociones cambiaron de estado por fecha");
            return cambios;
        }

        // Promociones de un estado (vacío o null = todas), sin el detalle
        public List<BE.PROMOCION> ListarPromociones(string estado)
        {
            return GestorPromocion.ListarPromociones(estado);
        }

        // Tablero y CU034: promociones finalizadas que todavía no se evaluaron
        public List<BE.PROMOCION> ListarParaEvaluar()
        {
            return GestorPromocion.ListarPromociones(EstadoFinalizada).Where(p => p.Resultado == null).ToList();
        }

        // Promoción con sus productos y tamaños
        public BE.PROMOCION ObtenerPromocion(int idPromocion)
        {
            return GestorPromocion.BuscarPromocion(idPromocion);
        }

        // CU010 (decisión 80, propuesta 2 de la Fase 5): precios promocionales vigentes hoy. Vigente = En proceso o Vigente
        // y hoy dentro de la vigencia (no depende de que ActualizarEstados haya corrido). La decisión 79 asegura una
        // sola promoción por producto y tamaño.
        public List<BE.PROMOCION_DETALLE> ListarPreciosVigentes()
        {
            return GestorPromocion.ListarPreciosVigentes(DateTime.Today);
        }

        // ── CU033 Crear Promoción ─────────────────────────────────────

        // Descuento con el que se propone el precio promocional al armar una promoción desde el análisis
        // (propuesta 5 de la Fase 5: 10 % menos que el de lista, redondeado a $ 10; el Dueño lo cambia)
        const decimal DescuentoSugerido = 0.10m;

        // CU033 paso 2: productos y tamaños a la venta, con el precio de lista y el costo unitario actual
        // (receta × costo de los lotes que se pueden vender, el mismo del análisis)
        public List<BE.PROMOCION_DETALLE> ListarProductosParaPromocion()
        {
            var productos = new VENTA_BLL().ListarProductosParaVenta();
            var costos = new ANALISIS_BLL().CalcularCostosActuales(productos);
            return productos.Select(pt => new BE.PROMOCION_DETALLE
            {
                IdProducto = pt.IdProducto,
                IdTamanio = pt.IdTamanio,
                PrecioOriginal = pt.Precio,
                PrecioPromocional = PrecioSugerido(pt.Precio),
                CostoUnitario = costos[pt.IdProducto + "-" + pt.IdTamanio],
                ProductoTamanio = pt
            }).OrderBy(d => d.ProductoTamanio.Producto.Nombre).ThenBy(d => d.ProductoTamanio.Tamanio.CantidadMagnitud).ToList();
        }

        public decimal PrecioSugerido(decimal precioLista)
        {
            return Math.Max(10, Math.Round(precioLista * (1 - DescuentoSugerido) / 10, 0) * 10);
        }

        // CU031 → CU033 («extend»): promoción armada con el criterio del análisis y los productos elegidos
        public BE.PROMOCION ArmarPromocionDesdeAnalisis(BE.ANALISIS_RENTABILIDAD analisis, List<BE.ANALISIS_ITEM> items)
        {
            var disponibles = ListarProductosParaPromocion();
            var promocion = new BE.PROMOCION { Criterio = analisis.Criterio, FechaDesde = DateTime.Today, FechaHasta = DateTime.Today.AddDays(14) };
            foreach (var i in items)
            {
                var d = disponibles.FirstOrDefault(x => x.IdProducto == i.IdProducto && x.IdTamanio == i.IdTamanio);
                if (d != null) promocion.Detalles.Add(d);
            }
            return promocion;
        }

        // CU033 paso 4 y flujos alternativos (las excepciones llevan la clave de idioma del mensaje):
        // FA1 datos incompletos, FA2 precio promocional inválido, FA3 vigencia inválida, FA4 superposición (decisión 79).
        // El precio original y el costo se toman de la base, no de la pantalla.
        public void ValidarPromocion(BE.PROMOCION promocion)
        {
            promocion.Nombre = (promocion.Nombre ?? string.Empty).Trim();
            if (promocion.Nombre.Length == 0)
                throw new ArgumentException("msgPromocionNombreObligatorio");
            if (promocion.Nombre.Length > 100)
                throw new ArgumentException("msgPromocionNombreLargo");
            if (Array.IndexOf(ListarCriterios(), promocion.Criterio) < 0)
                throw new ArgumentException("msgPromocionCriterioObligatorio");
            if (promocion.Detalles == null || promocion.Detalles.Count == 0)
                throw new ArgumentException("msgPromocionSinProductos");
            if (promocion.Detalles.GroupBy(d => new { d.IdProducto, d.IdTamanio }).Any(g => g.Count() > 1))
                throw new ArgumentException("msgPromocionProductoRepetido");

            var disponibles = ListarProductosParaPromocion();
            foreach (var d in promocion.Detalles)
            {
                var actual = disponibles.FirstOrDefault(x => x.IdProducto == d.IdProducto && x.IdTamanio == d.IdTamanio);
                if (actual == null)
                    throw new ArgumentException("msgPromocionProductoNoDisponible");
                d.PrecioOriginal = actual.PrecioOriginal;
                d.CostoUnitario = actual.CostoUnitario;
                d.ProductoTamanio = actual.ProductoTamanio;
                // Propuesta 3 de la Fase 5: mayor a 0 y menor que el de lista (el margen sí puede ser negativo, decisión 77)
                if (d.PrecioPromocional <= 0 || d.PrecioPromocional >= d.PrecioOriginal)
                    throw new ArgumentException("msgPromocionPrecioInvalido");
            }

            // Propuesta 4 de la Fase 5: no se crean promociones que empiecen en el pasado
            if (promocion.FechaDesde.Date < DateTime.Today || promocion.FechaHasta.Date < promocion.FechaDesde.Date)
                throw new ArgumentException("msgPromocionVigenciaInvalida");

            if (BuscarSuperposiciones(promocion).Count > 0)
                throw new ArgumentException("msgPromocionSuperpuesta");
        }

        // FA4 (decisión 79): promociones En proceso o Vigentes con algún producto y tamaño de esta y vigencia superpuesta
        public List<BE.PROMOCION> BuscarSuperposiciones(BE.PROMOCION promocion)
        {
            return promocion.Detalles
                .SelectMany(d => GestorPromocion.BuscarSuperpuestas(d.IdProducto, d.IdTamanio, promocion.FechaDesde, promocion.FechaHasta))
                .Where(p => p.IdPromocion != promocion.IdPromocion)
                .GroupBy(p => p.IdPromocion).Select(g => g.First()).ToList();
        }

        // Decisión 77: líneas con margen negativo; la pantalla pide confirmación antes de guardar
        public List<BE.PROMOCION_DETALLE> ObtenerMargenesNegativos(BE.PROMOCION promocion)
        {
            return promocion.Detalles.Where(d => d.MargenUnitario < 0).ToList();
        }

        // CU033 pasos 5 y 6: guarda la promoción con sus productos en una transacción. Estado inicial: Vigente si empieza
        // hoy, En proceso si empieza después. Con margen negativo solo se guarda si el Dueño lo aceptó (decisión 77).
        public BE.PROMOCION CrearPromocion(BE.PROMOCION promocion, bool aceptaMargenNegativo)
        {
            ValidarPromocion(promocion);
            bool margenNegativo = ObtenerMargenesNegativos(promocion).Count > 0;
            if (margenNegativo && !aceptaMargenNegativo)
                throw new ArgumentException("msgPromocionMargenNegativoSinConfirmar");
            promocion.Estado = promocion.FechaDesde.Date == DateTime.Today ? EstadoVigente : EstadoEnProceso;
            promocion.FechaCreacion = DateTime.Now;
            promocion.Simulada = false;
            promocion.Resultado = null;

            var acceso = new ACCESO();
            acceso.IniciarTransaccion();
            try
            {
                new MAPPER_PROMOCION(acceso).GuardarPromocion(promocion);
                acceso.ConfirmarTransaccion();
            }
            catch
            {
                acceso.DeshacerTransaccion();
                promocion.IdPromocion = 0;
                throw;
            }

            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Promoción creada",
                $"Promoción {promocion.IdPromocion} ({promocion.Nombre}), criterio {promocion.Criterio}, del {promocion.FechaDesde:dd/MM/yyyy} al {promocion.FechaHasta:dd/MM/yyyy}, "
                + $"{promocion.Detalles.Count} productos, {promocion.Estado}" + (margenNegativo ? ", aceptada con margen negativo" : ""));
            return promocion;
        }
    }
}
