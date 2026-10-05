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
    public class MAPPER_PROVEEDOR
    {
        ACCESO acceso;

        public MAPPER_PROVEEDOR()
        {
            acceso = new ACCESO();
        }

        // Para usar el mapper dentro de una transacción ya iniciada (decisión 27)
        public MAPPER_PROVEEDOR(ACCESO accesoCompartido)
        {
            acceso = accesoCompartido;
        }

        // CU016 / CU026 / CU027: proveedores activos
        public List<BE.PROVEEDOR> ListarProveedores()
        {
            return Listar("ListarProveedorActivo");
        }

        // Decisión 64: todos los proveedores, activos y dados de baja
        public List<BE.PROVEEDOR> ListarTodos()
        {
            return Listar("ListarProveedor");
        }

        // Decisión 64: reactivación (Activo = 1)
        public void ReactivarProveedor(int idProveedor)
        {
            acceso.Abrir();
            try
            {
                acceso.Escribir("ReactivarProveedor", new List<SqlParameter> { acceso.CrearParametro("@IdProveedor", idProveedor) });
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        private List<BE.PROVEEDOR> Listar(string sp)
        {
            acceso.Abrir();
            DataTable tabla;
            try
            {
                tabla = acceso.Leer(sp);
            }
            finally
            {
                acceso.Cerrar();
            }
            return tabla.Rows.Cast<DataRow>().Select(u => new BE.PROVEEDOR
            {
                IdProveedor = Convert.ToInt32(u["IdProveedor"]),
                RazonSocial = u["RazonSocial"].ToString(),
                CUIT = u["CUIT"].ToString(),
                Telefono = u["Telefono"].ToString(),
                Correo = u["Correo"].ToString(),
                Activo = Convert.ToBoolean(u["Activo"])
            }).ToList();
        }

        // CU016 mensajes 28-33 / CU026: insumos que ofrece el proveedor
        public List<BE.INSUMO> ListarInsumosDeProveedor(int idProveedor)
        {
            acceso.Abrir();
            DataTable tabla;
            try
            {
                tabla = acceso.Leer("ListarInsumosDeProveedor", new List<SqlParameter> { acceso.CrearParametro("@IdProveedor", idProveedor) });
            }
            finally
            {
                acceso.Cerrar();
            }
            return tabla.Rows.Cast<DataRow>().Select(u => new BE.INSUMO
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
            }).ToList();
        }

        // CU022: alta del proveedor y sus insumos (en la transacción del llamador). El SP rechaza el CUIT repetido.
        public int GuardarProveedor(BE.PROVEEDOR proveedor)
        {
            acceso.Abrir();
            try
            {
                DataTable tabla = acceso.Leer("AltaProveedor", ParametrosDatos(proveedor, false));
                proveedor.IdProveedor = Convert.ToInt32(tabla.Rows[0]["IdProveedor"]);
                GuardarInsumos(proveedor);
                return proveedor.IdProveedor;
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        // CU026: datos y reemplazo de los insumos asociados (filas de relación, decisión 53)
        public void ModificarProveedor(BE.PROVEEDOR proveedor)
        {
            acceso.Abrir();
            try
            {
                acceso.Escribir("ModificarProveedor", ParametrosDatos(proveedor, true));
                acceso.Escribir("QuitarInsumosProveedor", new List<SqlParameter> { acceso.CrearParametro("@IdProveedor", proveedor.IdProveedor) });
                GuardarInsumos(proveedor);
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        // CU027: baja lógica
        public void BajaProveedor(int idProveedor)
        {
            acceso.Abrir();
            try
            {
                acceso.Escribir("BajaProveedor", new List<SqlParameter> { acceso.CrearParametro("@IdProveedor", idProveedor) });
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        private void GuardarInsumos(BE.PROVEEDOR proveedor)
        {
            foreach (var insumo in proveedor.Insumos)
            {
                acceso.Escribir("AsignarInsumoProveedor", new List<SqlParameter>
                {
                    acceso.CrearParametro("@IdProveedor", proveedor.IdProveedor),
                    acceso.CrearParametro("@IdInsumo", insumo.IdInsumo)
                });
            }
        }

        private List<SqlParameter> ParametrosDatos(BE.PROVEEDOR proveedor, bool conId)
        {
            var parametros = new List<SqlParameter>();
            if (conId) parametros.Add(acceso.CrearParametro("@IdProveedor", proveedor.IdProveedor));
            parametros.Add(acceso.CrearParametro("@RazonSocial", proveedor.RazonSocial));
            parametros.Add(acceso.CrearParametro("@CUIT", proveedor.CUIT));
            parametros.Add(acceso.CrearParametro("@Telefono", proveedor.Telefono));
            parametros.Add(acceso.CrearParametro("@Correo", proveedor.Correo));
            return parametros;
        }
    }
}
