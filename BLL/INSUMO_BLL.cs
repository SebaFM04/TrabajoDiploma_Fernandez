using BE;
using DAL;
using SERVICIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

namespace BLL
{
    public class INSUMO_BLL
    {
        MAPPER_INSUMO GestorInsumo = new MAPPER_INSUMO();

        // Error que lanza un SP con RAISERROR (nombre duplicado)
        private const int ErrorDeNegocioSql = 50000;

        // Unidades en que se lleva el stock (decisión 4)
        public string[] ListarUnidadesMedida()
        {
            return new[] { "ml", "g", "un" };
        }

        // Reglas de docs/04_ABM_INSUMOS.md. Las excepciones llevan la clave de idioma del mensaje.
        private void ValidarInsumo(BE.INSUMO insumo)
        {
            if (string.IsNullOrWhiteSpace(insumo.Nombre))
                throw new ArgumentException("msgInsumoNombreObligatorio");
            insumo.Nombre = insumo.Nombre.Trim();
            if (insumo.Nombre.Length > 100)
                throw new ArgumentException("msgInsumoNombreLargo");

            if (Array.IndexOf(ListarUnidadesMedida(), insumo.UnidadMedida) < 0)
                throw new ArgumentException("msgInsumoUnidadMedidaObligatoria");

            if (string.IsNullOrWhiteSpace(insumo.UnidadCompra))
                throw new ArgumentException("msgInsumoUnidadCompraObligatoria");
            insumo.UnidadCompra = insumo.UnidadCompra.Trim();
            if (insumo.UnidadCompra.Length > 50)
                throw new ArgumentException("msgInsumoUnidadCompraLarga");

            if (insumo.EquivalenciaMagnitud <= 0)
                throw new ArgumentException("msgInsumoEquivalenciaInvalida");

            if (insumo.CostoUnidadCompra < 0 || insumo.UmbralReposicion < 0 || insumo.VolumenPesoDisponible < 0)
                throw new ArgumentException("msgInsumoValorNegativo");
            // Decisión 58: bebidas, comidas o ambos
            if (!insumo.UsoBebidas && !insumo.UsoComidas)
                throw new ArgumentException("msgInsumoSinUso");
        }

        // CU023 paso 3 (decisión 52): el stock inicial se ingresa en unidades de compra
        // y se lleva en la unidad de medida. Ej.: 4 botellas × 2250 ml = 9000 ml.
        public decimal CalcularStockInicial(decimal cantidadUnidadesCompra, decimal equivalencia)
        {
            if (cantidadUnidadesCompra < 0)
                throw new ArgumentException("msgInsumoValorNegativo");
            return Math.Round(cantidadUnidadesCompra * equivalencia, 3);
        }

        // CU023 Insertar Insumo
        public void InsertarInsumo(BE.INSUMO insumo)
        {
            ValidarInsumo(insumo);
            // El aviso de stock bajo lo maneja el sistema: el insumo nace activo y sin aviso
            insumo.AvisoStockBajo = false;
            insumo.Activo = true;

            try
            {
                insumo.IdInsumo = GestorInsumo.AltaInsumo(insumo);
            }
            catch (SqlException ex) when (ex.Number == ErrorDeNegocioSql)
            {
                // FA2: el SP detectó un insumo con el mismo nombre (activo o dado de baja)
                throw new ArgumentException("msgInsumoDuplicado", ex);
            }

            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Alta de insumo", $"Se agrego el insumo: {insumo.Nombre}");
        }

        // CU024 Modificar Insumo
        public void ModificarInsumo(BE.INSUMO insumo)
        {
            var insumoActual = GestorInsumo.BuscarInsumo(insumo.IdInsumo);
            if (insumoActual == null || !insumoActual.Activo)
                throw new ArgumentException("msgInsumoInexistente");

            // La unidad de medida y el stock no se modifican (decisión 37): se conservan los actuales
            insumo.UnidadMedida = insumoActual.UnidadMedida;
            insumo.VolumenPesoDisponible = insumoActual.VolumenPesoDisponible;
            insumo.AvisoStockBajo = insumoActual.AvisoStockBajo;
            insumo.Activo = insumoActual.Activo;
            ValidarInsumo(insumo);

            try
            {
                GestorInsumo.ModificarInsumo(insumo);
            }
            catch (SqlException ex) when (ex.Number == ErrorDeNegocioSql)
            {
                // FA2: otro insumo (activo o dado de baja) ya tiene ese nombre
                throw new ArgumentException("msgInsumoDuplicado", ex);
            }

            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Modificación de insumo", $"Se modificó el insumo: {insumo.Nombre}");
        }

        // CU025 paso 2 / FA1: productos activos que usan el insumo en su receta.
        // Si la lista no está vacía, el insumo no se puede dar de baja.
        public List<BE.PRODUCTO> VerificarBajaInsumo(int idInsumo)
        {
            return GestorInsumo.ListarProductosActivosPorInsumo(idInsumo);
        }

        // CU025 FA2: órdenes de compra abiertas (ni Cerrada ni Pagada, decisión 49) que incluyen el insumo
        public List<BE.ORDEN_COMPRA> VerificarOrdenesAbiertas(int idInsumo)
        {
            return new MAPPER_ORDEN_COMPRA().ListarAbiertasPorInsumo(idInsumo);
        }

        // CU025 Dar de Baja Insumo: baja lógica. Un stock mayor a cero no la impide.
        public void DarDeBajaInsumo(int idInsumo)
        {
            var insumo = GestorInsumo.BuscarInsumo(idInsumo);
            if (insumo == null || !insumo.Activo)
                throw new ArgumentException("msgInsumoInexistente");

            if (VerificarBajaInsumo(idInsumo).Count > 0)
                throw new ArgumentException("msgInsumoEnReceta");
            if (VerificarOrdenesAbiertas(idInsumo).Count > 0)
                throw new ArgumentException("msgInsumoEnOrdenAbierta");

            GestorInsumo.BajaInsumo(idInsumo);

            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Baja de insumo", $"Se dio de baja el insumo: {insumo.Nombre}");
        }

        public List<BE.INSUMO> ListarInsumosActivos()
        {
            return GestorInsumo.ListarInsumosActivos();
        }

        // Valores del filtro por uso (decisión 58)
        public const string UsoTodos = "Todos";
        public const string UsoBebidas = "Bebidas";
        public const string UsoComidas = "Comidas";

        // Decisión 58: insumos activos según su uso. Los de ambos usos aparecen en los dos filtros.
        // Decisión 64: ABM de insumos, activos y dados de baja del uso elegido
        public List<BE.INSUMO> ListarInsumos(string uso)
        {
            var insumos = GestorInsumo.ListarInsumos();
            if (uso == UsoBebidas) return insumos.Where(i => i.UsoBebidas).ToList();
            if (uso == UsoComidas) return insumos.Where(i => i.UsoComidas).ToList();
            return insumos;
        }

        // Decisión 64: reactivación de un insumo dado de baja (vuelve a estar disponible para recetas y órdenes)
        public void ReactivarInsumo(int idInsumo)
        {
            var insumo = GestorInsumo.BuscarInsumo(idInsumo);
            if (insumo == null)
                throw new ArgumentException("msgInsumoInexistente");
            if (insumo.Activo) return;
            GestorInsumo.ReactivarInsumo(idInsumo);
            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Reactivación de insumo", $"Se reactivó el insumo: {insumo.Nombre}");
        }

        public List<BE.INSUMO> ListarInsumosPorUso(string uso)
        {
            var insumos = GestorInsumo.ListarInsumosActivos();
            if (uso == UsoBebidas) return insumos.Where(i => i.UsoBebidas).ToList();
            if (uso == UsoComidas) return insumos.Where(i => i.UsoComidas).ToList();
            return insumos;
        }

        // CU016 paso 2 (mensajes 2-7): insumos con aviso de stock bajo pendiente. Lista vacía = FA1.
        public List<BE.INSUMO> ListarConAvisoPendiente()
        {
            return GestorInsumo.ListarConAvisoPendiente();
        }

        // CU016 mensajes 45-50: los avisos pendientes de los insumos pedidos pasan a "En compra" con la orden
        public void AsociarAvisosAOrden(List<int> idsInsumo, int idOrdenCompra, ACCESO acceso)
        {
            var mapper = new MAPPER_INSUMO(acceso);
            foreach (int id in idsInsumo.Distinct())
                mapper.ActualizarAvisos(id, idOrdenCompra);
        }

        // CU018: al ajustar la orden, los avisos de insumos quitados vuelven a "Pendiente" y los de insumos nuevos se asocian
        public void ReasociarAvisos(BE.ORDEN_COMPRA orden, ACCESO acceso)
        {
            new MAPPER_INSUMO(acceso).LiberarAvisos(orden.IdOrdenCompra);
            AsociarAvisosAOrden(orden.Detalles.Select(d => d.IdInsumo).ToList(), orden.IdOrdenCompra, acceso);
        }

        // CU019 mensajes 29-38: ingresa lo recibido (decisión 61): stock += cantidad × equivalencia y costo promedio ponderado.
        // Decisión 59: si el insumo queda en o sobre el umbral, sus avisos se resuelven y el flag vuelve a 0.
        public void IngresarStock(List<BE.RECEPCION_DETALLE> recibidos, ACCESO acceso)
        {
            var mapper = new MAPPER_INSUMO(acceso);
            foreach (var r in recibidos)
            {
                var insumo = mapper.BuscarInsumo(r.IdInsumo);
                if (insumo == null)
                    throw new ArgumentException("msgInsumoInexistente");
                decimal stockEnUnidades = insumo.EquivalenciaMagnitud > 0 ? insumo.VolumenPesoDisponible / insumo.EquivalenciaMagnitud : 0;
                decimal costo = stockEnUnidades + r.CantidadRecibida > 0
                    ? (stockEnUnidades * insumo.CostoUnidadCompra + r.CantidadRecibida * r.CostoUnidadCompra) / (stockEnUnidades + r.CantidadRecibida)
                    : r.CostoUnidadCompra;
                decimal nuevoStock = insumo.VolumenPesoDisponible + r.CantidadRecibida * insumo.EquivalenciaMagnitud;
                mapper.ActualizarVolumenPeso(insumo.IdInsumo, nuevoStock);
                mapper.ActualizarCosto(insumo.IdInsumo, Math.Round(costo, 2));
                if (nuevoStock >= insumo.UmbralReposicion)
                    mapper.ResolverAvisos(insumo.IdInsumo);
            }
        }

        // Tablero del menú (decisión 56): insumos activos bajo el umbral o con aviso de stock bajo pendiente
        public List<BE.INSUMO> ListarStockBajo()
        {
            return GestorInsumo.ListarInsumosActivos()
                .Where(i => i.VolumenPesoDisponible < i.UmbralReposicion || i.AvisoStockBajo)
                .ToList();
        }

        // CU010 mensajes 9-17: hay stock para el consumo requerido. insumoRequerido.Proporcion = cantidad total (decisión 53).
        // Con acceso != null se lee dentro de la transacción de la venta (segunda verificación del paso 5).
        public bool VerificarDisponibilidad(BE.RECETA insumoRequerido, ACCESO acceso = null)
        {
            var mapper = acceso == null ? GestorInsumo : new MAPPER_INSUMO(acceso);
            var insumo = mapper.BuscarInsumo(insumoRequerido.IdInsumo);
            if (insumo == null || !insumo.Activo) return false;
            insumoRequerido.Insumo = insumo;
            return insumo.VolumenPesoDisponible >= insumoRequerido.Proporcion;
        }

        // CU010 mensajes 30-35: descuenta el consumo de cada insumo. El stock nunca queda negativo.
        public void DescontarStock(List<BE.RECETA> consumos, ACCESO acceso)
        {
            var mapper = new MAPPER_INSUMO(acceso);
            foreach (var consumo in consumos)
            {
                var insumo = mapper.BuscarInsumo(consumo.IdInsumo);
                if (insumo == null || insumo.VolumenPesoDisponible < consumo.Proporcion)
                    throw new ArgumentException("msgVentaStockInsuficiente");
                mapper.ActualizarVolumenPeso(insumo.IdInsumo, insumo.VolumenPesoDisponible - consumo.Proporcion);
            }
        }

        // CU010 mensajes 18-23, en el paso 5 (decisión 47): después de descontar, los insumos que quedaron
        // bajo el umbral y no tienen aviso pendiente generan un aviso. Devuelve los insumos avisados.
        public List<BE.INSUMO> VerificarStockBajoUmbral(List<int> idsInsumo, ACCESO acceso)
        {
            var mapper = new MAPPER_INSUMO(acceso);
            var avisados = new List<BE.INSUMO>();
            foreach (int id in idsInsumo.Distinct())
            {
                var insumo = mapper.BuscarInsumo(id);
                if (insumo != null && insumo.VolumenPesoDisponible < insumo.UmbralReposicion && !insumo.AvisoStockBajo)
                {
                    GenerarAvisoStockBajo(insumo, mapper);
                    avisados.Add(insumo);
                }
            }
            return avisados;
        }

        // Aviso "Pendiente" para el Encargado (lo atiende CU016 en N02)
        private BE.AVISO_STOCK_BAJO GenerarAvisoStockBajo(BE.INSUMO insumo, MAPPER_INSUMO mapper)
        {
            return mapper.GuardarAviso(new BE.AVISO_STOCK_BAJO
            {
                IdInsumo = insumo.IdInsumo,
                FechaHora = DateTime.Now,
                VolumenPesoAlMomento = insumo.VolumenPesoDisponible
            });
        }
    }
}
