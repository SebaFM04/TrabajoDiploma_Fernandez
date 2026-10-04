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
    public class MAPPER_RECEPCION
    {
        ACCESO acceso;

        public MAPPER_RECEPCION()
        {
            acceso = new ACCESO();
        }

        // CU019: se usa dentro de la transacción de la recepción (decisión 27)
        public MAPPER_RECEPCION(ACCESO accesoCompartido)
        {
            acceso = accesoCompartido;
        }

        // CU019 mensajes 25-28: recepción con sus líneas
        public BE.RECEPCION GuardarRecepcion(BE.RECEPCION recepcion)
        {
            acceso.Abrir();
            try
            {
                DataTable tabla = acceso.Leer("GuardarRecepcion", new List<SqlParameter>
                {
                    acceso.CrearParametro("@IdOrdenCompra", recepcion.IdOrdenCompra),
                    acceso.CrearParametro("@FechaRecepcion", recepcion.FechaRecepcion),
                    acceso.CrearParametro("@NumeroRemito", recepcion.NumeroRemito),
                    acceso.CrearParametro("@NumeroFacturaProveedor", recepcion.NumeroFacturaProveedor)
                });
                recepcion.IdRecepcion = Convert.ToInt32(tabla.Rows[0]["IdRecepcion"]);
                foreach (var d in recepcion.Detalles)
                {
                    d.IdRecepcion = recepcion.IdRecepcion;
                    acceso.Escribir("GuardarRecepcionDetalle", new List<SqlParameter>
                    {
                        acceso.CrearParametro("@IdRecepcion", recepcion.IdRecepcion),
                        acceso.CrearParametro("@IdInsumo", d.IdInsumo),
                        acceso.CrearParametro("@CantidadRecibida", d.CantidadRecibida),
                        acceso.CrearParametro("@CostoUnidadCompra", d.CostoUnidadCompra)
                    });
                }
                return recepcion;
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        // CU021: recepciones de la orden con sus líneas (facturas asociadas y total)
        public List<BE.RECEPCION> ListarPorOrden(int idOrdenCompra)
        {
            acceso.Abrir();
            DataTable tabla;
            try
            {
                tabla = acceso.Leer("ListarRecepcionPorOrden", new List<SqlParameter> { acceso.CrearParametro("@IdOrdenCompra", idOrdenCompra) });
            }
            finally
            {
                acceso.Cerrar();
            }
            var recepciones = new List<BE.RECEPCION>();
            foreach (DataRow u in tabla.Rows)
            {
                int id = Convert.ToInt32(u["IdRecepcion"]);
                var r = recepciones.FirstOrDefault(x => x.IdRecepcion == id);
                if (r == null)
                {
                    r = new BE.RECEPCION
                    {
                        IdRecepcion = id,
                        IdOrdenCompra = idOrdenCompra,
                        FechaRecepcion = Convert.ToDateTime(u["FechaRecepcion"]),
                        NumeroRemito = u["NumeroRemito"].ToString(),
                        NumeroFacturaProveedor = u["NumeroFacturaProveedor"].ToString()
                    };
                    recepciones.Add(r);
                }
                r.Detalles.Add(new BE.RECEPCION_DETALLE
                {
                    IdRecepcion = id,
                    IdInsumo = Convert.ToInt32(u["IdInsumo"]),
                    CantidadRecibida = Convert.ToInt32(u["CantidadRecibida"]),
                    CostoUnidadCompra = Convert.ToDecimal(u["CostoUnidadCompra"]),
                    Insumo = new BE.INSUMO { IdInsumo = Convert.ToInt32(u["IdInsumo"]), Nombre = u["NombreInsumo"].ToString() }
                });
            }
            return recepciones;
        }
    }
}
