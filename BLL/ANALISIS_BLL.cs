using BE;
using DAL;
using SERVICIO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    // N03 CU031 Generar Análisis de Rentabilidad (decisiones 74 a 77). La lógica va acá, nunca en el formulario.
    public class ANALISIS_BLL
    {
        MAPPER_ANALISIS GestorAnalisis = new MAPPER_ANALISIS();

        // Gráficos (decisión 83) y tipos de serie
        public const string GraficoEvolucion = "Evolucion";
        public const string GraficoParticipacion = "Participacion";
        public const string GraficoRiesgo = "Riesgo";
        public const string GraficoEstacionalidad = "Estacionalidad";
        public const string TipoLinea = "Linea";
        public const string TipoTorta = "Torta";
        public const string TipoBarras = "Barras";

        // Decisión 76: días que cuentan como "por vencer"
        public const int DiasPorVencerDefecto = 15;
        public int[] ListarDiasPorVencer()
        {
            return new[] { 7, 15, 30 };
        }

        public string[] ListarCriterios()
        {
            return new PROMOCION_BLL().ListarCriterios();
        }

        // CU031: genera el análisis del período [desde, hasta] (días completos) con el criterio elegido.
        // FA1 período inválido (desde mayor a hasta, o hasta en el futuro) y FA2 sin ventas: no se genera nada.
        // Sin lotes por vencer o con poca historia se genera igual, con un aviso.
        public BE.ANALISIS_RENTABILIDAD GenerarAnalisis(string criterio, DateTime desde, DateTime hasta, int diasPorVencer)
        {
            if (desde.Date > hasta.Date || hasta.Date > DateTime.Today)
                throw new ArgumentException("msgAnalisisPeriodoInvalido");
            if (Array.IndexOf(ListarCriterios(), criterio) < 0)
                throw new ArgumentException("msgAnalisisCriterioInvalido");
            bool vencimiento = criterio == PROMOCION_BLL.CriterioVencimiento;
            if (vencimiento && Array.IndexOf(ListarDiasPorVencer(), diasPorVencer) < 0)
                throw new ArgumentException("msgAnalisisDiasInvalidos");

            DateTime hastaExclusivo = hasta.Date.AddDays(1);
            var vendidos = GestorAnalisis.ListarVentasPorProducto(desde.Date, hastaExclusivo);
            if (vendidos.Sum(v => v.Unidades) == 0)
                throw new ArgumentException("msgAnalisisSinVentas");

            var usuario = SessionManager.Instancia.UsuarioActual;
            var analisis = new BE.ANALISIS_RENTABILIDAD
            {
                Criterio = criterio,
                FechaDesde = desde.Date,
                FechaHasta = hasta.Date,
                DiasPorVencer = vencimiento ? diasPorVencer : 0,
                FechaGeneracion = DateTime.Now,
                Usuario = usuario == null ? string.Empty : $"{usuario.NombreUsuario} {usuario.ApellidoUsuario} ({usuario.CorreoElectronico})"
            };
            // Receta por unidad de cada producto y tamaño a la venta (clave "IdProducto-IdTamanio"), para costos y criterios
            var recetas = new Dictionary<string, List<BE.RECETA>>();
            analisis.Items = ArmarItems(vendidos, GestorAnalisis.ListarCostoActualInsumos(), recetas);
            CalcularBase(analisis);
            Clasificar(analisis.Items);
            ArmarGraficosBase(analisis, hastaExclusivo);
            AplicarCriterio(analisis, hastaExclusivo, recetas);

            if (usuario != null)
                new BITACORA_BLL().RegistrarEvento(usuario.IdUsuario, "Análisis de rentabilidad generado",
                    $"Criterio {criterio}, del {analisis.FechaDesde:dd/MM/yyyy} al {analisis.FechaHasta:dd/MM/yyyy}"
                    + (vencimiento ? $", {diasPorVencer} días por vencer" : "") + $": {analisis.ObtenerSugeridos().Count} productos sugeridos");
            return analisis;
        }

        static string Clave(int idProducto, int idTamanio)
        {
            return idProducto + "-" + idTamanio;
        }

        // Propuesta 2 de la Fase 4: todos los productos y tamaños a la venta (aunque no hayan vendido) más los vendidos
        // en el período que ya no están a la venta. Costo actual = receta × costo de los lotes que se pueden vender.
        List<BE.ANALISIS_ITEM> ArmarItems(List<BE.ANALISIS_ITEM> vendidos, Dictionary<int, decimal> costosInsumo, Dictionary<string, List<BE.RECETA>> recetas)
        {
            var porClave = vendidos.ToDictionary(v => Clave(v.IdProducto, v.IdTamanio));
            var items = new List<BE.ANALISIS_ITEM>();
            var gestorReceta = new RECETA_BLL();
            foreach (var pt in new VENTA_BLL().ListarProductosParaVenta())
            {
                string clave = Clave(pt.IdProducto, pt.IdTamanio);
                if (!porClave.TryGetValue(clave, out var item))
                    item = new BE.ANALISIS_ITEM { IdProducto = pt.IdProducto, IdTamanio = pt.IdTamanio };
                item.NombreProducto = pt.Producto.Nombre;
                item.TipoProducto = pt.Producto.Tipo;
                item.NombreTamanio = pt.Tamanio.Nombre;
                item.PrecioActual = pt.Precio;
                var receta = gestorReceta.ObtenerRecetaEscalada(pt.IdProducto, pt.Tamanio.CantidadMagnitud, 1);
                recetas[clave] = receta;
                item.CostoUnitarioActual = Math.Round(receta.Sum(r => r.Proporcion * (costosInsumo.TryGetValue(r.IdInsumo, out decimal c) ? c : 0)), 2);
                items.Add(item);
                porClave.Remove(clave);
            }
            items.AddRange(porClave.Values);
            return items.OrderBy(i => i.NombreProducto).ThenBy(i => i.NombreTamanio).ToList();
        }

        // Base del ítem 16 del TODO: margen total, unitario, %, participación y margen unitario actual
        void CalcularBase(BE.ANALISIS_RENTABILIDAD analisis)
        {
            foreach (var i in analisis.Items)
            {
                i.MargenTotal = i.Ingresos - i.Costo;
                i.MargenUnitario = i.Unidades > 0 ? Math.Round(i.MargenTotal / i.Unidades, 2) : 0;
                i.MargenPorcentaje = i.Ingresos > 0 ? Math.Round(i.MargenTotal / i.Ingresos * 100, 1) : 0;
                i.MargenUnitarioActual = i.PrecioActual > 0 ? i.PrecioActual - i.CostoUnitarioActual : 0;
            }
            analisis.TotalUnidades = analisis.Items.Sum(i => i.Unidades);
            analisis.TotalIngresos = analisis.Items.Sum(i => i.Ingresos);
            analisis.TotalCosto = analisis.Items.Sum(i => i.Costo);
            analisis.TotalMargen = analisis.TotalIngresos - analisis.TotalCosto;
            foreach (var i in analisis.Items)
                i.Participacion = analisis.TotalIngresos > 0 ? Math.Round(i.Ingresos / analisis.TotalIngresos * 100, 1) : 0;
        }

        // Margen unitario con el que se compara un ítem: el del período si vendió, el actual si no vendió
        static decimal MargenReferencia(BE.ANALISIS_ITEM i)
        {
            return i.Unidades > 0 ? i.MargenUnitario : i.MargenUnitarioActual;
        }

        // Clasificación por margen y ventas contra el promedio (alto margen y venta baja = conveniente para promocionar)
        void Clasificar(List<BE.ANALISIS_ITEM> items)
        {
            decimal margenPromedio = items.Average(MargenReferencia);
            double unidadesPromedio = items.Average(i => i.Unidades);
            foreach (var i in items)
            {
                bool altoMargen = MargenReferencia(i) >= margenPromedio;
                bool ventaAlta = i.Unidades >= unidadesPromedio;
                i.Clasificacion = altoMargen
                    ? (ventaAlta ? "msgAnalisisAltoMargenVentaAlta" : "msgAnalisisAltoMargenVentaBaja")
                    : (ventaAlta ? "msgAnalisisBajoMargenVentaAlta" : "msgAnalisisBajoMargenVentaBaja");
            }
        }

        // Gráficos de todos los criterios: evolución diaria de los ingresos (línea) y participación por tipo (torta)
        void ArmarGraficosBase(BE.ANALISIS_RENTABILIDAD analisis, DateTime hastaExclusivo)
        {
            var diarias = GestorAnalisis.ListarVentasDiarias(analisis.FechaDesde, hastaExclusivo).ToDictionary(p => p.Etiqueta, p => p.Valor);
            var evolucion = new BE.ANALISIS_SERIE { Grafico = GraficoEvolucion, Nombre = "msgAnalisisSerieIngresos", Tipo = TipoLinea };
            for (DateTime dia = analisis.FechaDesde; dia < hastaExclusivo; dia = dia.AddDays(1))
            {
                string etiqueta = dia.ToString("yyyy-MM-dd");
                evolucion.Puntos.Add(new BE.ANALISIS_PUNTO { Etiqueta = etiqueta, Valor = diarias.TryGetValue(etiqueta, out decimal v) ? v : 0 });
            }
            analisis.Series.Add(evolucion);

            var participacion = new BE.ANALISIS_SERIE { Grafico = GraficoParticipacion, Nombre = "msgAnalisisSerieParticipacion", Tipo = TipoTorta };
            foreach (var tipo in analisis.Items.Where(i => i.Ingresos > 0).GroupBy(i => i.TipoProducto).OrderBy(g => g.Key))
                participacion.Puntos.Add(new BE.ANALISIS_PUNTO { Etiqueta = tipo.Key, Valor = tipo.Sum(i => i.Ingresos) });
            analisis.Series.Add(participacion);
        }

        // Ítem 17 del TODO: cada criterio marca los productos sugeridos con su motivo
        void AplicarCriterio(BE.ANALISIS_RENTABILIDAD analisis, DateTime hastaExclusivo, Dictionary<string, List<BE.RECETA>> recetas)
        {
        }
    }
}
