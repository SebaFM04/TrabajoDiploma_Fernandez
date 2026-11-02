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
    // N03 CU031: consultas de sumas para el análisis de rentabilidad. El período es [desde, hasta).
    public class MAPPER_ANALISIS
    {
        ACCESO acceso;

        public MAPPER_ANALISIS()
        {
            acceso = new ACCESO();
        }

        // Ventas por producto y tamaño: unidades, ingresos y costo (el resto de los datos los calcula ANALISIS_BLL)
        public List<BE.ANALISIS_ITEM> ListarVentasPorProducto(DateTime desde, DateTime hasta)
        {
            return Leer("ListarVentasPorProducto", desde, hasta).Rows.Cast<DataRow>().Select(u => new BE.ANALISIS_ITEM
            {
                IdProducto = Convert.ToInt32(u["IdProducto"]),
                NombreProducto = u["NombreProducto"].ToString(),
                TipoProducto = u["Tipo"].ToString(),
                IdTamanio = Convert.ToInt32(u["IdTamaño"]),
                NombreTamanio = u["NombreTamanio"].ToString(),
                Unidades = Convert.ToInt32(u["Unidades"]),
                Ingresos = Convert.ToDecimal(u["Ingresos"]),
                Costo = Convert.ToDecimal(u["Costo"])
            }).ToList();
        }

        // Ingresos por día (etiqueta aaaa-MM-dd), para el gráfico de evolución
        public List<BE.ANALISIS_PUNTO> ListarVentasDiarias(DateTime desde, DateTime hasta)
        {
            return Leer("ListarVentasDiarias", desde, hasta).Rows.Cast<DataRow>().Select(u => new BE.ANALISIS_PUNTO
            {
                Etiqueta = Convert.ToDateTime(u["Dia"]).ToString("yyyy-MM-dd"),
                Valor = Convert.ToDecimal(u["Ingresos"])
            }).ToList();
        }

        // Unidades por mes: una serie por producto y tamaño (Nombre = "IdProducto-IdTamanio"), con un punto por mes (aaaa-MM)
        public List<BE.ANALISIS_SERIE> ListarUnidadesPorMes(DateTime desde, DateTime hasta)
        {
            return Leer("ListarUnidadesPorMes", desde, hasta).Rows.Cast<DataRow>()
                .GroupBy(u => u["IdProducto"] + "-" + u["IdTamaño"])
                .Select(g => new BE.ANALISIS_SERIE
                {
                    Nombre = g.Key,
                    Puntos = g.Select(u => new BE.ANALISIS_PUNTO
                    {
                        Etiqueta = $"{Convert.ToInt32(u["Anio"]):0000}-{Convert.ToInt32(u["Mes"]):00}",
                        Valor = Convert.ToDecimal(u["Unidades"])
                    }).ToList()
                }).ToList();
        }

        // Consumo de cada insumo en el período (en su unidad de medida), por IdInsumo
        public Dictionary<int, decimal> ListarConsumoInsumos(DateTime desde, DateTime hasta)
        {
            return Leer("ListarConsumoInsumos", desde, hasta).Rows.Cast<DataRow>()
                .ToDictionary(u => Convert.ToInt32(u["IdInsumo"]), u => Convert.ToDecimal(u["Consumo"]));
        }

        // Costo actual por unidad de medida de cada insumo (lotes que se pueden vender), por IdInsumo
        public Dictionary<int, decimal> ListarCostoActualInsumos()
        {
            acceso.Abrir();
            DataTable tabla;
            try
            {
                tabla = acceso.Leer("ListarCostoActualInsumos");
            }
            finally
            {
                acceso.Cerrar();
            }
            return tabla.Rows.Cast<DataRow>().ToDictionary(u => Convert.ToInt32(u["IdInsumo"]), u => Convert.ToDecimal(u["CostoUnidadMedida"]));
        }

        private DataTable Leer(string nombreSP, DateTime desde, DateTime hasta)
        {
            acceso.Abrir();
            try
            {
                return acceso.Leer(nombreSP, new List<SqlParameter>
                {
                    acceso.CrearParametro("@Desde", desde),
                    acceso.CrearParametro("@Hasta", hasta)
                });
            }
            finally
            {
                acceso.Cerrar();
            }
        }
    }
}
