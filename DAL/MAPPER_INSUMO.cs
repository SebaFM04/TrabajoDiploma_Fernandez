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
    public class MAPPER_INSUMO
    {
        ACCESO acceso = new ACCESO();

        // CU023: el SP rechaza el nombre duplicado (RAISERROR) y devuelve el Id generado
        public int AltaInsumo(BE.INSUMO Insumo)
        {
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@Nombre", Insumo.Nombre));
            parametros.Add(acceso.CrearParametro("@UnidadMedida", Insumo.UnidadMedida));
            parametros.Add(acceso.CrearParametro("@UnidadCompra", Insumo.UnidadCompra));
            parametros.Add(acceso.CrearParametro("@EquivalenciaMagnitud", Insumo.EquivalenciaMagnitud));
            parametros.Add(acceso.CrearParametro("@VolumenPesoDisponible", Insumo.VolumenPesoDisponible));
            parametros.Add(acceso.CrearParametro("@UmbralReposicion", Insumo.UmbralReposicion));
            parametros.Add(acceso.CrearParametro("@CostoUnidadCompra", Insumo.CostoUnidadCompra));

            DataTable tabla;
            try
            {
                tabla = acceso.Leer("AltaInsumo", parametros);
            }
            finally
            {
                acceso.Cerrar();
            }

            if (tabla.Rows.Count > 0)
            {
                return Convert.ToInt32(tabla.Rows[0]["IdInsumo"]);
            }
            return 0;
        }

        // Solo los insumos activos, para las pantallas de operación
        public List<BE.INSUMO> ListarInsumosActivos()
        {
            List<BE.INSUMO> listaInsumos = new List<BE.INSUMO>();
            acceso.Abrir();
            DataTable tabla = acceso.Leer("ListarInsumoActivo");
            acceso.Cerrar();
            foreach (DataRow u in tabla.Rows)
            {
                listaInsumos.Add(MapearInsumo(u));
            }
            return listaInsumos;
        }

        private BE.INSUMO MapearInsumo(DataRow u)
        {
            return new BE.INSUMO
            {
                IdInsumo = Convert.ToInt32(u["IdInsumo"]),
                Nombre = u["Nombre"].ToString(),
                UnidadMedida = u["UnidadMedida"].ToString(),
                UnidadCompra = u["UnidadCompra"].ToString(),
                EquivalenciaMagnitud = Convert.ToDecimal(u["EquivalenciaMagnitud"]),
                VolumenPesoDisponible = Convert.ToDecimal(u["VolumenPesoDisponible"]),
                UmbralReposicion = Convert.ToDecimal(u["UmbralReposicion"]),
                CostoUnidadCompra = Convert.ToDecimal(u["CostoUnidadCompra"]),
                AvisoStockBajo = Convert.ToBoolean(u["AvisoStockBajo"]),
                Activo = Convert.ToBoolean(u["Activo"])
            };
        }
    }
}
