using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    // N03: promociones (decisiones 74 a 82)
    public class MAPPER_PROMOCION
    {
        ACCESO acceso;

        public MAPPER_PROMOCION()
        {
            acceso = new ACCESO();
        }

        // CU033, CU035 y simulador: se usa dentro de la transacción de la operación (decisión 27)
        public MAPPER_PROMOCION(ACCESO accesoCompartido)
        {
            acceso = accesoCompartido;
        }

        // Guarda la promoción con sus productos y tamaños. Devuelve el Id generado.
        public int GuardarPromocion(BE.PROMOCION promocion)
        {
            acceso.Abrir();
            try
            {
                DataTable tabla = acceso.Leer("GuardarPromocion", new List<SqlParameter>
                {
                    acceso.CrearParametro("@Nombre", promocion.Nombre),
                    acceso.CrearParametro("@Criterio", promocion.Criterio),
                    new SqlParameter("@FechaDesde", SqlDbType.Date) { Value = promocion.FechaDesde.Date },
                    new SqlParameter("@FechaHasta", SqlDbType.Date) { Value = promocion.FechaHasta.Date },
                    acceso.CrearParametro("@Estado", promocion.Estado),
                    acceso.CrearParametro("@FechaCreacion", promocion.FechaCreacion),
                    new SqlParameter("@IdPromocionOrigen", SqlDbType.Int) { Value = (object)promocion.IdPromocionOrigen ?? DBNull.Value },
                    new SqlParameter("@Observaciones", SqlDbType.NVarChar, 500) { Value = (object)promocion.Observaciones ?? DBNull.Value },
                    acceso.CrearParametro("@Simulada", promocion.Simulada ? 1 : 0)
                });
                promocion.IdPromocion = Convert.ToInt32(tabla.Rows[0]["IdPromocion"]);

                foreach (var d in promocion.Detalles)
                {
                    d.IdPromocion = promocion.IdPromocion;
                    acceso.Escribir("GuardarPromocionDetalle", new List<SqlParameter>
                    {
                        acceso.CrearParametro("@IdPromocion", d.IdPromocion),
                        acceso.CrearParametro("@IdProducto", d.IdProducto),
                        acceso.CrearParametro("@IdTamanio", d.IdTamanio),
                        acceso.CrearParametro("@PrecioOriginal", d.PrecioOriginal),
                        acceso.CrearParametro("@PrecioPromocional", d.PrecioPromocional),
                        acceso.CrearParametro("@CostoUnitario", d.CostoUnitario)
                    });
                }
                return promocion.IdPromocion;
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        // Promociones de un estado (vacío = todas), sin el detalle
        public List<BE.PROMOCION> ListarPromociones(string estado)
        {
            acceso.Abrir();
            DataTable tabla;
            try
            {
                tabla = acceso.Leer("ListarPromociones", new List<SqlParameter> { acceso.CrearParametro("@Estado", estado ?? string.Empty) });
            }
            finally
            {
                acceso.Cerrar();
            }
            return tabla.Rows.Cast<DataRow>().Select(MapearPromocion).ToList();
        }

        // Promoción con sus productos y tamaños (null si no existe)
        public BE.PROMOCION BuscarPromocion(int idPromocion)
        {
            acceso.Abrir();
            DataTable cabecera, detalle;
            try
            {
                cabecera = acceso.Leer("BuscarPromocion", new List<SqlParameter> { acceso.CrearParametro("@IdPromocion", idPromocion) });
                detalle = acceso.Leer("ListarDetallePromocion", new List<SqlParameter> { acceso.CrearParametro("@IdPromocion", idPromocion) });
            }
            finally
            {
                acceso.Cerrar();
            }
            if (cabecera.Rows.Count == 0) return null;
            var promocion = MapearPromocion(cabecera.Rows[0]);
            foreach (DataRow u in detalle.Rows)
            {
                promocion.Detalles.Add(new BE.PROMOCION_DETALLE
                {
                    IdPromocion = idPromocion,
                    IdProducto = Convert.ToInt32(u["IdProducto"]),
                    IdTamanio = Convert.ToInt32(u["IdTamaño"]),
                    PrecioOriginal = Convert.ToDecimal(u["PrecioOriginal"]),
                    PrecioPromocional = Convert.ToDecimal(u["PrecioPromocional"]),
                    CostoUnitario = Convert.ToDecimal(u["CostoUnitario"]),
                    ProductoTamanio = new BE.PRODUCTO_TAMANIO
                    {
                        IdProducto = Convert.ToInt32(u["IdProducto"]),
                        IdTamanio = Convert.ToInt32(u["IdTamaño"]),
                        Precio = Convert.ToDecimal(u["PrecioOriginal"]),
                        Producto = new BE.PRODUCTO { IdProducto = Convert.ToInt32(u["IdProducto"]), Nombre = u["NombreProducto"].ToString(), Tipo = u["Tipo"].ToString() },
                        Tamanio = new BE.TAMANIO
                        {
                            IdTamanio = Convert.ToInt32(u["IdTamaño"]),
                            Nombre = u["NombreTamanio"].ToString(),
                            CantidadMagnitud = Convert.ToDecimal(u["CantidadMagnitud"]),
                            UnidadMagnitud = u["UnidadMagnitud"].ToString()
                        }
                    }
                });
            }
            return promocion;
        }

        // Decisión 79: promociones En proceso o Vigentes del mismo producto y tamaño cuya vigencia se superpone con [desde, hasta]
        public List<BE.PROMOCION> BuscarSuperpuestas(int idProducto, int idTamanio, DateTime desde, DateTime hasta)
        {
            acceso.Abrir();
            DataTable tabla;
            try
            {
                tabla = acceso.Leer("BuscarPromocionesSuperpuestas", new List<SqlParameter>
                {
                    acceso.CrearParametro("@IdProducto", idProducto),
                    acceso.CrearParametro("@IdTamanio", idTamanio),
                    new SqlParameter("@Desde", SqlDbType.Date) { Value = desde.Date },
                    new SqlParameter("@Hasta", SqlDbType.Date) { Value = hasta.Date }
                });
            }
            finally
            {
                acceso.Cerrar();
            }
            return tabla.Rows.Cast<DataRow>().Select(MapearPromocion).ToList();
        }

        // CU010 (decisión 80): precio promocional de cada producto y tamaño con una promoción vigente hoy
        // (En proceso o Vigente y hoy dentro de la vigencia)
        public List<BE.PROMOCION_DETALLE> ListarPreciosVigentes(DateTime hoy)
        {
            acceso.Abrir();
            DataTable tabla;
            try
            {
                tabla = acceso.Leer("ListarPreciosPromocionVigentes", new List<SqlParameter>
                {
                    new SqlParameter("@Hoy", SqlDbType.Date) { Value = hoy.Date }
                });
            }
            finally
            {
                acceso.Cerrar();
            }
            return tabla.Rows.Cast<DataRow>().Select(u => new BE.PROMOCION_DETALLE
            {
                IdPromocion = Convert.ToInt32(u["IdPromocion"]),
                IdProducto = Convert.ToInt32(u["IdProducto"]),
                IdTamanio = Convert.ToInt32(u["IdTamaño"]),
                PrecioOriginal = Convert.ToDecimal(u["PrecioOriginal"]),
                PrecioPromocional = Convert.ToDecimal(u["PrecioPromocional"]),
                CostoUnitario = Convert.ToDecimal(u["CostoUnitario"])
            }).ToList();
        }

        // CU034: unidades por día (etiqueta aaaa-MM-dd) de los productos y tamaños de la promoción en [desde, hasta)
        public List<BE.ANALISIS_PUNTO> ListarUnidadesDiarias(int idPromocion, DateTime desde, DateTime hasta)
        {
            acceso.Abrir();
            DataTable tabla;
            try
            {
                tabla = acceso.Leer("ListarUnidadesDiariasPromocion", new List<SqlParameter>
                {
                    acceso.CrearParametro("@IdPromocion", idPromocion),
                    acceso.CrearParametro("@Desde", desde),
                    acceso.CrearParametro("@Hasta", hasta)
                });
            }
            finally
            {
                acceso.Cerrar();
            }
            return tabla.Rows.Cast<DataRow>().Select(u => new BE.ANALISIS_PUNTO
            {
                Etiqueta = Convert.ToDateTime(u["Dia"]).ToString("yyyy-MM-dd"),
                Valor = Convert.ToDecimal(u["Unidades"])
            }).ToList();
        }

        // CU034: guarda el resultado de la evaluación. Devuelve 0 si la promoción no está Finalizada o ya tenía resultado.
        public int GuardarEvaluacion(int idPromocion, string resultado, DateTime fechaEvaluacion, string observaciones)
        {
            acceso.Abrir();
            try
            {
                DataTable tabla = acceso.Leer("GuardarEvaluacionPromocion", new List<SqlParameter>
                {
                    acceso.CrearParametro("@IdPromocion", idPromocion),
                    acceso.CrearParametro("@Resultado", resultado),
                    acceso.CrearParametro("@FechaEvaluacion", fechaEvaluacion),
                    new SqlParameter("@Observaciones", SqlDbType.NVarChar, 500) { Value = (object)observaciones ?? DBNull.Value }
                });
                return Convert.ToInt32(tabla.Rows[0]["Filas"]);
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        // Decisión 78: cambios de estado por fecha. Devuelve cuántas promociones cambiaron.
        public int ActualizarEstados(DateTime hoy)
        {
            acceso.Abrir();
            try
            {
                DataTable tabla = acceso.Leer("ActualizarEstadosPromocion", new List<SqlParameter>
                {
                    new SqlParameter("@Hoy", SqlDbType.Date) { Value = hoy.Date }
                });
                return Convert.ToInt32(tabla.Rows[0]["Cambios"]);
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        private BE.PROMOCION MapearPromocion(DataRow u)
        {
            return new BE.PROMOCION
            {
                IdPromocion = Convert.ToInt32(u["IdPromocion"]),
                Nombre = u["Nombre"].ToString(),
                Criterio = u["Criterio"].ToString(),
                FechaDesde = Convert.ToDateTime(u["FechaDesde"]),
                FechaHasta = Convert.ToDateTime(u["FechaHasta"]),
                Estado = u["Estado"].ToString(),
                FechaCreacion = Convert.ToDateTime(u["FechaCreacion"]),
                IdPromocionOrigen = u["IdPromocionOrigen"] == DBNull.Value ? (int?)null : Convert.ToInt32(u["IdPromocionOrigen"]),
                Resultado = u["Resultado"] == DBNull.Value ? null : u["Resultado"].ToString(),
                FechaEvaluacion = u["FechaEvaluacion"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(u["FechaEvaluacion"]),
                Observaciones = u["Observaciones"] == DBNull.Value ? null : u["Observaciones"].ToString(),
                Simulada = Convert.ToBoolean(u["Simulada"])
            };
        }
    }
}
