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
                item.CostoUnitarioActual = CostoUnitario(receta, costosInsumo);
                items.Add(item);
                porClave.Remove(clave);
            }
            items.AddRange(porClave.Values);
            return items.OrderBy(i => i.NombreProducto).ThenBy(i => i.NombreTamanio).ToList();
        }

        // Costo de una unidad: receta por unidad × costo por unidad de medida de cada insumo
        static decimal CostoUnitario(List<BE.RECETA> receta, Dictionary<int, decimal> costosInsumo)
        {
            return Math.Round(receta.Sum(r => r.Proporcion * (costosInsumo.TryGetValue(r.IdInsumo, out decimal c) ? c : 0)), 2);
        }

        // Propuesta 8 de la Fase 5: costo unitario actual de cada producto y tamaño (clave "IdProducto-IdTamanio"),
        // con el mismo cálculo del análisis. Lo usa CU033 para mostrar el margen de la promoción.
        public Dictionary<string, decimal> CalcularCostosActuales(List<BE.PRODUCTO_TAMANIO> productos)
        {
            var costosInsumo = GestorAnalisis.ListarCostoActualInsumos();
            var gestorReceta = new RECETA_BLL();
            return productos.ToDictionary(pt => Clave(pt.IdProducto, pt.IdTamanio),
                pt => CostoUnitario(gestorReceta.ObtenerRecetaEscalada(pt.IdProducto, pt.Tamanio.CantidadMagnitud, 1), costosInsumo));
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

        // Ítem 17 del TODO: cada criterio marca los productos sugeridos (solo los que están a la venta) con su motivo,
        // y los numera en el orden del criterio
        void AplicarCriterio(BE.ANALISIS_RENTABILIDAD analisis, DateTime hastaExclusivo, Dictionary<string, List<BE.RECETA>> recetas)
        {
            List<BE.ANALISIS_ITEM> sugeridos;
            switch (analisis.Criterio)
            {
                case PROMOCION_BLL.CriterioVentas: sugeridos = CriterioVentas(analisis); break;
                case PROMOCION_BLL.CriterioRotacion: sugeridos = CriterioRotacion(analisis, hastaExclusivo, recetas); break;
                case PROMOCION_BLL.CriterioVencimiento: sugeridos = CriterioVencimiento(analisis, hastaExclusivo, recetas); break;
                default: sugeridos = CriterioEstacionalidad(analisis, hastaExclusivo); break;
            }
            int orden = 0;
            foreach (var i in sugeridos)
            {
                i.Sugerido = true;
                i.Orden = ++orden;
            }
            if (sugeridos.Count == 0)
                analisis.Avisos.Add("msgAnalisisSinSugeridos");
        }

        // Productos y tamaños que se pueden promocionar: los que están a la venta
        static IEnumerable<BE.ANALISIS_ITEM> ALaVenta(BE.ANALISIS_RENTABILIDAD analisis)
        {
            return analisis.Items.Where(i => i.PrecioActual > 0);
        }

        static int DiasDelPeriodo(BE.ANALISIS_RENTABILIDAD analisis, DateTime hastaExclusivo)
        {
            return Math.Max(1, (hastaExclusivo - analisis.FechaDesde).Days);
        }

        // Ventas: margen unitario sobre el promedio y unidades por debajo del 70 % del promedio (alto margen, poca venta)
        List<BE.ANALISIS_ITEM> CriterioVentas(BE.ANALISIS_RENTABILIDAD analisis)
        {
            decimal margenPromedio = analisis.Items.Average(MargenReferencia);
            double unidadesPromedio = analisis.Items.Average(i => i.Unidades);
            var sugeridos = new List<BE.ANALISIS_ITEM>();
            foreach (var i in ALaVenta(analisis))
            {
                i.Indicador = MargenReferencia(i);
                if (MargenReferencia(i) > margenPromedio && i.Unidades < unidadesPromedio * 0.7)
                {
                    i.Motivo = "msgMotivoVentas";
                    i.MotivoValor1 = MargenReferencia(i);
                    i.MotivoValor2 = Math.Round(margenPromedio, 2);
                    sugeridos.Add(i);
                }
            }
            return sugeridos.OrderByDescending(i => i.Indicador).ToList();
        }

        // Rotación: por insumo, días de cobertura = stock vendible / consumo diario del período. Hay sobrestock si la
        // cobertura pasa de 30 días o del doble del promedio, o si el insumo no se consumió y tiene stock.
        // Sugiere los productos con margen actual positivo que usan esos insumos.
        List<BE.ANALISIS_ITEM> CriterioRotacion(BE.ANALISIS_RENTABILIDAD analisis, DateTime hastaExclusivo, Dictionary<string, List<BE.RECETA>> recetas)
        {
            int dias = DiasDelPeriodo(analisis, hastaExclusivo);
            var consumo = GestorAnalisis.ListarConsumoInsumos(analisis.FechaDesde, hastaExclusivo);
            // Cobertura en días por insumo; null = sin consumo en el período (cobertura sin límite)
            var cobertura = new Dictionary<int, decimal?>();
            var insumos = new INSUMO_BLL().ListarInsumosActivos().Where(i => i.StockVendible > 0).ToDictionary(i => i.IdInsumo);
            foreach (var insumo in insumos.Values)
            {
                decimal diario = consumo.TryGetValue(insumo.IdInsumo, out decimal c) ? c / dias : 0;
                cobertura[insumo.IdInsumo] = diario > 0 ? Math.Round(insumo.StockVendible / diario, 1) : (decimal?)null;
            }
            var finitas = cobertura.Values.Where(v => v.HasValue).Select(v => v.Value).ToList();
            decimal promedio = finitas.Count > 0 ? Math.Round(finitas.Average(), 1) : 0;
            Func<int, bool> sobrestock = id => cobertura.TryGetValue(id, out var cob) && (!cob.HasValue || cob.Value > 30 || (promedio > 0 && cob.Value > promedio * 2));

            var sugeridos = new List<BE.ANALISIS_ITEM>();
            foreach (var i in ALaVenta(analisis).Where(i => i.MargenUnitarioActual > 0))
            {
                if (!recetas.TryGetValue(Clave(i.IdProducto, i.IdTamanio), out var receta)) continue;
                // El insumo con más cobertura de la receta es el que explica la sugerencia
                var enSobrestock = receta.Where(r => sobrestock(r.IdInsumo))
                    .OrderByDescending(r => cobertura[r.IdInsumo] ?? decimal.MaxValue).FirstOrDefault();
                if (enSobrestock == null) continue;
                var cob = cobertura[enSobrestock.IdInsumo];
                var insumo = insumos[enSobrestock.IdInsumo];
                i.Indicador = cob ?? 9999;
                i.MotivoInsumo = insumo.Nombre;
                i.Motivo = cob.HasValue ? "msgMotivoRotacion" : "msgMotivoRotacionSinConsumo";
                i.MotivoValor1 = cob ?? 0;
                i.MotivoValor2 = promedio;
                sugeridos.Add(i);
            }
            return sugeridos.OrderByDescending(i => i.Indicador).ThenByDescending(i => i.MargenUnitarioActual).ToList();
        }

        // Vencimiento: lotes que vencen dentro de los días elegidos (decisión 76). El consumo esperado hasta cada vencimiento
        // (consumo diario del período × días restantes) se descuenta primero del lote que vence antes (propuesta 4 de la
        // Fase 4); lo que no se llega a consumir está en riesgo y vale cantidad × costo del lote. Sugiere los productos que
        // usan esos insumos, aunque el margen sea bajo (decisión 77), primero los que sacan más valor en riesgo por unidad.
        List<BE.ANALISIS_ITEM> CriterioVencimiento(BE.ANALISIS_RENTABILIDAD analisis, DateTime hastaExclusivo, Dictionary<string, List<BE.RECETA>> recetas)
        {
            int dias = DiasDelPeriodo(analisis, hastaExclusivo);
            var consumo = GestorAnalisis.ListarConsumoInsumos(analisis.FechaDesde, hastaExclusivo);
            var lotes = new LOTE_BLL().ListarLotesPorVencer(analisis.DiasPorVencer);
            if (lotes.Count == 0)
            {
                analisis.Avisos.Add("msgAnalisisSinLotesPorVencer");
                return new List<BE.ANALISIS_ITEM>();
            }

            // Por insumo: cantidad y valor en riesgo, y costo por unidad de medida (del lote en riesgo)
            var riesgo = new Dictionary<int, (string nombre, string unidad, decimal cantidad, decimal valor, decimal costo)>();
            foreach (var porInsumo in lotes.GroupBy(l => l.IdInsumo))
            {
                decimal diario = consumo.TryGetValue(porInsumo.Key, out decimal c) ? c / dias : 0;
                decimal anteriores = 0;
                foreach (var lote in porInsumo.OrderBy(l => l.FechaVencimiento).ThenBy(l => l.IdLote))
                {
                    int diasRestantes = Math.Max(0, (lote.FechaVencimiento.Value.Date - DateTime.Today).Days);
                    decimal esperado = diario * diasRestantes;
                    decimal consumido = Math.Min(lote.CantidadDisponible, Math.Max(0, esperado - anteriores));
                    decimal enRiesgo = lote.CantidadDisponible - consumido;
                    anteriores += lote.CantidadDisponible;
                    if (enRiesgo <= 0) continue;
                    riesgo.TryGetValue(porInsumo.Key, out var r);
                    riesgo[porInsumo.Key] = (lote.Insumo.Nombre, lote.Insumo.UnidadMedida, r.cantidad + enRiesgo,
                                             r.valor + Math.Round(enRiesgo * lote.CostoUnidadMedida, 2), lote.CostoUnidadMedida);
                }
            }
            if (riesgo.Count == 0)
            {
                analisis.Avisos.Add("msgAnalisisSinRiesgo");
                return new List<BE.ANALISIS_ITEM>();
            }

            var barras = new BE.ANALISIS_SERIE { Grafico = GraficoRiesgo, Nombre = "msgAnalisisSerieRiesgo", Tipo = TipoBarras };
            foreach (var r in riesgo.Values.OrderByDescending(r => r.valor))
                barras.Puntos.Add(new BE.ANALISIS_PUNTO { Etiqueta = r.nombre, Valor = r.valor });
            analisis.Series.Add(barras);

            var sugeridos = new List<BE.ANALISIS_ITEM>();
            foreach (var i in ALaVenta(analisis))
            {
                if (!recetas.TryGetValue(Clave(i.IdProducto, i.IdTamanio), out var receta)) continue;
                // Insumo en riesgo del que el producto saca más valor por unidad vendida
                var usa = receta.Where(r => riesgo.ContainsKey(r.IdInsumo))
                    .OrderByDescending(r => r.Proporcion * riesgo[r.IdInsumo].costo).FirstOrDefault();
                if (usa == null) continue;
                var r2 = riesgo[usa.IdInsumo];
                i.Indicador = r2.valor;
                i.Motivo = "msgMotivoVencimiento";
                i.MotivoInsumo = $"{r2.nombre} ({r2.unidad})";
                i.MotivoValor1 = Math.Round(r2.cantidad, 0);
                i.MotivoValor2 = Math.Round(usa.Proporcion, 1);
                sugeridos.Add(i);
            }
            // Orden: valor en riesgo que saca cada unidad vendida (consumo por unidad × costo del insumo)
            return sugeridos.OrderByDescending(i => recetas[Clave(i.IdProducto, i.IdTamanio)]
                                .Where(r => riesgo.ContainsKey(r.IdInsumo)).Max(r => r.Proporcion * riesgo[r.IdInsumo].costo))
                            .ToList();
        }

        // Estacionalidad: índice = unidades por mes del período / promedio mensual de los 12 meses que terminan en
        // FechaHasta (propuesta 5 de la Fase 4). Sugiere índice menor a 0,8 (temporada baja) con margen actual positivo.
        // Con menos de 12 meses de historia usa los que haya y avisa.
        List<BE.ANALISIS_ITEM> CriterioEstacionalidad(BE.ANALISIS_RENTABILIDAD analisis, DateTime hastaExclusivo)
        {
            DateTime desde12 = hastaExclusivo.AddMonths(-12);
            var porMes = GestorAnalisis.ListarUnidadesPorMes(desde12, hastaExclusivo).ToDictionary(s => s.Nombre);
            var meses = new List<string>();
            for (DateTime m = new DateTime(desde12.Year, desde12.Month, 1); m < hastaExclusivo; m = m.AddMonths(1))
                meses.Add(m.ToString("yyyy-MM"));
            // La ventana de 12 meses toca 13 meses calendario (el primero y el último incompletos): se cuentan hasta 12
            var mesesConVentas = Math.Min(12, porMes.Values.SelectMany(s => s.Puntos).Where(p => p.Valor > 0).Select(p => p.Etiqueta).Distinct().Count());
            analisis.MesesHistoria = mesesConVentas;
            if (mesesConVentas < 12)
                analisis.Avisos.Add("msgAnalisisHistorialInsuficiente");

            decimal mesesPeriodo = DiasDelPeriodo(analisis, hastaExclusivo) / 30.4m;
            var sugeridos = new List<BE.ANALISIS_ITEM>();
            foreach (var i in ALaVenta(analisis))
            {
                if (!porMes.TryGetValue(Clave(i.IdProducto, i.IdTamanio), out var serie) || mesesConVentas == 0) continue;
                decimal promedioMensual = serie.Puntos.Sum(p => p.Valor) / mesesConVentas;
                if (promedioMensual <= 0) continue;
                i.Indicador = Math.Round(i.Unidades / mesesPeriodo / promedioMensual, 2);
                if (i.Indicador < 0.8m && i.MargenUnitarioActual > 0)
                {
                    i.Motivo = "msgMotivoEstacionalidad";
                    i.MotivoValor1 = i.Indicador;
                    i.MotivoValor2 = Math.Round(promedioMensual, 0);
                    sugeridos.Add(i);
                }
            }
            sugeridos = sugeridos.OrderBy(i => i.Indicador).ToList();

            // Gráfico: unidades por mes del total y de los 3 sugeridos con índice más bajo
            var total = new BE.ANALISIS_SERIE { Grafico = GraficoEstacionalidad, Nombre = "msgAnalisisSerieUnidadesTotales", Tipo = TipoLinea };
            foreach (var mes in meses)
                total.Puntos.Add(new BE.ANALISIS_PUNTO { Etiqueta = mes, Valor = porMes.Values.SelectMany(s => s.Puntos).Where(p => p.Etiqueta == mes).Sum(p => p.Valor) });
            analisis.Series.Add(total);
            foreach (var i in sugeridos.Take(3))
            {
                var serie = porMes[Clave(i.IdProducto, i.IdTamanio)];
                var linea = new BE.ANALISIS_SERIE { Grafico = GraficoEstacionalidad, Nombre = $"{i.NombreProducto} {i.NombreTamanio}", Tipo = TipoLinea };
                foreach (var mes in meses)
                    linea.Puntos.Add(new BE.ANALISIS_PUNTO { Etiqueta = mes, Valor = serie.Puntos.Where(p => p.Etiqueta == mes).Sum(p => p.Valor) });
                analisis.Series.Add(linea);
            }
            return sugeridos;
        }
    }
}
