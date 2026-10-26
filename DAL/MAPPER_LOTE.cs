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
    // Lotes de insumos con vencimiento (decisiones 65 a 71)
    public class MAPPER_LOTE
    {
        ACCESO acceso;

        public MAPPER_LOTE()
        {
            acceso = new ACCESO();
        }

        // CU010, CU019, CU023 y CU036: se usa dentro de la transacción de la operación (decisión 27)
        public MAPPER_LOTE(ACCESO accesoCompartido)
        {
            acceso = accesoCompartido;
        }

        // CU019 y CU023: lote nuevo. Devuelve el Id generado.
        public int InsertarLote(BE.LOTE_INSUMO lote)
        {
            acceso.Abrir();
            try
            {
                DataTable tabla = acceso.Leer("InsertarLote", new List<SqlParameter>
                {
                    acceso.CrearParametro("@IdInsumo", lote.IdInsumo),
                    new SqlParameter("@IdRecepcion", SqlDbType.Int) { Value = (object)lote.IdRecepcion ?? DBNull.Value },
                    acceso.CrearParametro("@FechaIngreso", lote.FechaIngreso),
                    new SqlParameter("@FechaVencimiento", SqlDbType.Date) { Value = (object)lote.FechaVencimiento?.Date ?? DBNull.Value },
                    acceso.CrearParametro("@Cantidad", lote.CantidadInicial),
                    acceso.CrearParametro("@CostoUnidadCompra", lote.CostoUnidadCompra)
                });
                lote.IdLote = Convert.ToInt32(tabla.Rows[0]["IdLote"]);
                return lote.IdLote;
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        public BE.LOTE_INSUMO BuscarLote(int idLote)
        {
            var lotes = Listar("BuscarLote", new List<SqlParameter> { acceso.CrearParametro("@IdLote", idLote) });
            return lotes.FirstOrDefault();
        }

        // Todos los lotes del insumo, en orden FEFO
        public List<BE.LOTE_INSUMO> ListarLotesInsumo(int idInsumo)
        {
            return Listar("ListarLotesInsumo", new List<SqlParameter> { acceso.CrearParametro("@IdInsumo", idInsumo) });
        }

        // CU010 (decisiones 67 y 68): lotes Disponible sin fecha o no vencidos, en orden FEFO (los sin fecha al final)
        public List<BE.LOTE_INSUMO> ListarLotesVendibles(int idInsumo)
        {
            return Listar("ListarLotesVendibles", new List<SqlParameter> { acceso.CrearParametro("@IdInsumo", idInsumo) });
        }

        // Lotes de insumos activos que vencen entre hoy y hoy + dias
        public List<BE.LOTE_INSUMO> ListarLotesPorVencer(int dias)
        {
            return Listar("ListarLotesPorVencer", new List<SqlParameter> { acceso.CrearParametro("@Dias", dias) });
        }

        // CU036: lotes vencidos con cantidad (sin dar de baja)
        public List<BE.LOTE_INSUMO> ListarLotesVencidos()
        {
            return Listar("ListarLotesVencidos");
        }

        // CU010: consume una cantidad del lote; si llega a 0 queda Agotado
        public void DescontarLote(int idLote, decimal cantidad)
        {
            acceso.Abrir();
            try
            {
                acceso.Escribir("DescontarLote", new List<SqlParameter>
                {
                    acceso.CrearParametro("@IdLote", idLote),
                    acceso.CrearParametro("@Cantidad", cantidad)
                });
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        // CU036: el lote pasa a Vencido con su cantidad como baja. Devuelve 0 si no estaba vencido o no tenía cantidad.
        public int DarDeBajaLote(int idLote, DateTime fechaBaja)
        {
            acceso.Abrir();
            try
            {
                return acceso.Escribir("DarDeBajaLote", new List<SqlParameter>
                {
                    acceso.CrearParametro("@IdLote", idLote),
                    acceso.CrearParametro("@FechaBaja", fechaBaja)
                });
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        private List<BE.LOTE_INSUMO> Listar(string nombreSP, List<SqlParameter> parametros = null)
        {
            acceso.Abrir();
            DataTable tabla;
            try
            {
                tabla = acceso.Leer(nombreSP, parametros);
            }
            finally
            {
                acceso.Cerrar();
            }
            return tabla.Rows.Cast<DataRow>().Select(MapearLote).ToList();
        }

        private BE.LOTE_INSUMO MapearLote(DataRow u)
        {
            return new BE.LOTE_INSUMO
            {
                IdLote = Convert.ToInt32(u["IdLote"]),
                IdInsumo = Convert.ToInt32(u["IdInsumo"]),
                IdRecepcion = u["IdRecepcion"] == DBNull.Value ? (int?)null : Convert.ToInt32(u["IdRecepcion"]),
                FechaIngreso = Convert.ToDateTime(u["FechaIngreso"]),
                FechaVencimiento = u["FechaVencimiento"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(u["FechaVencimiento"]),
                CantidadInicial = Convert.ToDecimal(u["CantidadInicial"]),
                CantidadDisponible = Convert.ToDecimal(u["CantidadDisponible"]),
                CostoUnidadCompra = Convert.ToDecimal(u["CostoUnidadCompra"]),
                Estado = u["Estado"].ToString(),
                FechaBaja = u["FechaBaja"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(u["FechaBaja"]),
                CantidadBaja = u["CantidadBaja"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(u["CantidadBaja"]),
                // Lo justo del insumo para mostrar el lote y pasar el costo a la unidad de medida
                Insumo = new BE.INSUMO
                {
                    IdInsumo = Convert.ToInt32(u["IdInsumo"]),
                    Nombre = u["NombreInsumo"].ToString(),
                    UnidadMedida = u["UnidadMedida"].ToString(),
                    EquivalenciaMagnitud = Convert.ToDecimal(u["EquivalenciaMagnitud"])
                }
            };
        }
    }
}
