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
        ACCESO acceso;

        public MAPPER_INSUMO()
        {
            acceso = new ACCESO();
        }

        // CU010: se usa dentro de la transacción de la venta (decisión 27)
        public MAPPER_INSUMO(ACCESO accesoCompartido)
        {
            acceso = accesoCompartido;
        }

        // CU010 mensajes 31-34: nuevo stock del insumo después de descontar el consumo
        public void ActualizarVolumenPeso(int idInsumo, decimal nuevoValor)
        {
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@IdInsumo", idInsumo),
                acceso.CrearParametro("@NuevoValor", nuevoValor)
            };
            try
            {
                acceso.Escribir("ActualizarVolumenPesoInsumo", parametros);
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        // CU010 (decisión 47): guarda el aviso de stock bajo; el SP también pone INSUMO.AvisoStockBajo = 1
        public BE.AVISO_STOCK_BAJO GuardarAviso(BE.AVISO_STOCK_BAJO aviso)
        {
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@IdInsumo", aviso.IdInsumo),
                acceso.CrearParametro("@FechaHora", aviso.FechaHora),
                acceso.CrearParametro("@VolumenPesoAlMomento", aviso.VolumenPesoAlMomento)
            };
            try
            {
                DataTable tabla = acceso.Leer("GuardarAvisoStockBajo", parametros);
                aviso.IdAviso = Convert.ToInt32(tabla.Rows[0]["IdAviso"]);
                aviso.Estado = "Pendiente";
                return aviso;
            }
            finally
            {
                acceso.Cerrar();
            }
        }

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
            parametros.Add(acceso.CrearParametro("@UsoBebidas", Insumo.UsoBebidas ? 1 : 0));
            parametros.Add(acceso.CrearParametro("@UsoComidas", Insumo.UsoComidas ? 1 : 0));

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

        // CU024: el SP rechaza el nombre duplicado y no modifica UnidadMedida ni el stock
        public int ModificarInsumo(BE.INSUMO Insumo)
        {
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@IdInsumo", Insumo.IdInsumo));
            parametros.Add(acceso.CrearParametro("@Nombre", Insumo.Nombre));
            parametros.Add(acceso.CrearParametro("@UnidadCompra", Insumo.UnidadCompra));
            parametros.Add(acceso.CrearParametro("@EquivalenciaMagnitud", Insumo.EquivalenciaMagnitud));
            parametros.Add(acceso.CrearParametro("@UmbralReposicion", Insumo.UmbralReposicion));
            parametros.Add(acceso.CrearParametro("@CostoUnidadCompra", Insumo.CostoUnidadCompra));
            parametros.Add(acceso.CrearParametro("@UsoBebidas", Insumo.UsoBebidas ? 1 : 0));
            parametros.Add(acceso.CrearParametro("@UsoComidas", Insumo.UsoComidas ? 1 : 0));
            try
            {
                return acceso.Escribir("ModificarInsumo", parametros);
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        public BE.INSUMO BuscarInsumo(int idInsumo)
        {
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@IdInsumo", idInsumo)
            };
            DataTable tabla = acceso.Leer("BuscarInsumo", parametros);
            acceso.Cerrar();

            if (tabla.Rows.Count == 0) return null;
            return MapearInsumo(tabla.Rows[0]);
        }

        // Decisión 64: todos los insumos, activos y dados de baja (SP ListarInsumo; el método ya figuraba en EA)
        public List<BE.INSUMO> ListarInsumos()
        {
            acceso.Abrir();
            DataTable tabla;
            try
            {
                tabla = acceso.Leer("ListarInsumo");
            }
            finally
            {
                acceso.Cerrar();
            }
            return tabla.Rows.Cast<DataRow>().Select(MapearInsumo).ToList();
        }

        // Decisión 64: reactivación (Activo = 1)
        public int ReactivarInsumo(int idInsumo)
        {
            acceso.Abrir();
            try
            {
                return acceso.Escribir("ReactivarInsumo", new List<SqlParameter> { acceso.CrearParametro("@IdInsumo", idInsumo) });
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        // CU025: baja lógica (Activo = 0)
        public int BajaInsumo(int idInsumo)
        {
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@IdInsumo", idInsumo)
            };
            try
            {
                return acceso.Escribir("BajaInsumo", parametros);
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        // CU025 FA1: productos activos cuya receta usa el insumo
        public List<BE.PRODUCTO> ListarProductosActivosPorInsumo(int idInsumo)
        {
            List<BE.PRODUCTO> productos = new List<BE.PRODUCTO>();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@IdInsumo", idInsumo)
            };
            DataTable tabla = acceso.Leer("ListarProductoActivoPorInsumo", parametros);
            acceso.Cerrar();

            foreach (DataRow u in tabla.Rows)
            {
                productos.Add(new BE.PRODUCTO
                {
                    IdProducto = Convert.ToInt32(u["IdProducto"]),
                    Nombre = u["Nombre"].ToString(),
                    Tipo = u["Tipo"].ToString(),
                    Activo = Convert.ToBoolean(u["Activo"]),
                    DVH = u["DVH"] == DBNull.Value ? null : u["DVH"].ToString()
                });
            }
            return productos;
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

        // CU016 mensajes 3-6: insumos con aviso de stock bajo pendiente
        public List<BE.INSUMO> ListarConAvisoPendiente()
        {
            acceso.Abrir();
            DataTable tabla;
            try
            {
                tabla = acceso.Leer("ListarInsumoConAvisoPendiente");
            }
            finally
            {
                acceso.Cerrar();
            }
            return tabla.Rows.Cast<DataRow>().Select(MapearInsumo).ToList();
        }

        // CU016 mensajes 46-49: los avisos pendientes del insumo pasan a "En compra" asociados a la orden
        public void ActualizarAvisos(int idInsumo, int idOrdenCompra)
        {
            acceso.Abrir();
            try
            {
                acceso.Escribir("AsociarAvisoOrdenCompra", new List<SqlParameter>
                {
                    acceso.CrearParametro("@IdInsumo", idInsumo),
                    acceso.CrearParametro("@IdOrdenCompra", idOrdenCompra)
                });
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        // CU018: los avisos de la orden cuyo insumo se quitó vuelven a "Pendiente"
        public void LiberarAvisos(int idOrdenCompra)
        {
            acceso.Abrir();
            try
            {
                acceso.Escribir("LiberarAvisosOrdenCompra", new List<SqlParameter> { acceso.CrearParametro("@IdOrdenCompra", idOrdenCompra) });
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        // CU019 (decisión 59): el insumo volvió a tener stock; sus avisos se resuelven y el flag vuelve a 0
        public void ResolverAvisos(int idInsumo)
        {
            acceso.Abrir();
            try
            {
                acceso.Escribir("ResolverAvisosInsumo", new List<SqlParameter> { acceso.CrearParametro("@IdInsumo", idInsumo) });
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        // CU019 mensajes 34-37: costo por unidad de compra (promedio ponderado, decisión 61)
        public void ActualizarCosto(int idInsumo, decimal costo)
        {
            acceso.Abrir();
            try
            {
                acceso.Escribir("ActualizarCostoInsumo", new List<SqlParameter>
                {
                    acceso.CrearParametro("@IdInsumo", idInsumo),
                    acceso.CrearParametro("@CostoUnidadCompra", costo)
                });
            }
            finally
            {
                acceso.Cerrar();
            }
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
                Activo = Convert.ToBoolean(u["Activo"]),
                UsoBebidas = Convert.ToBoolean(u["UsoBebidas"]),
                UsoComidas = Convert.ToBoolean(u["UsoComidas"])
            };
        }
    }
}
