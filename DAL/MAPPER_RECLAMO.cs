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
    public class MAPPER_RECLAMO
    {
        ACCESO acceso;

        public MAPPER_RECLAMO()
        {
            acceso = new ACCESO();
        }

        // CU019: ResolverPorOrden se usa dentro de la transacción de la recepción (decisión 27)
        public MAPPER_RECLAMO(ACCESO accesoCompartido)
        {
            acceso = accesoCompartido;
        }

        // CU020 mensajes 16-19: reclamo "Pendiente" con sus líneas
        public BE.RECLAMO GuardarReclamo(BE.RECLAMO reclamo)
        {
            acceso.Abrir();
            try
            {
                DataTable tabla = acceso.Leer("GuardarReclamo", new List<SqlParameter>
                {
                    acceso.CrearParametro("@IdRecepcion", reclamo.IdRecepcion),
                    acceso.CrearParametro("@FechaReclamo", reclamo.FechaReclamo)
                });
                reclamo.IdReclamo = Convert.ToInt32(tabla.Rows[0]["IdReclamo"]);
                reclamo.Estado = "Pendiente";
                foreach (var d in reclamo.Detalles)
                {
                    d.IdReclamo = reclamo.IdReclamo;
                    acceso.Escribir("GuardarReclamoDetalle", new List<SqlParameter>
                    {
                        acceso.CrearParametro("@IdReclamo", reclamo.IdReclamo),
                        acceso.CrearParametro("@IdInsumo", d.IdInsumo),
                        acceso.CrearParametro("@CantidadPendiente", d.CantidadPendiente),
                        acceso.CrearParametro("@Motivo", d.Motivo),
                        acceso.CrearParametro("@Descripcion", d.Descripcion ?? string.Empty)
                    });
                }
                return reclamo;
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        // CU019 mensajes 52-55 (decisión 60): los reclamos pendientes de la orden pasan a "Resuelto"
        public void ResolverPorOrden(int idOrdenCompra)
        {
            acceso.Abrir();
            try
            {
                acceso.Escribir("ResolverReclamosPorOrden", new List<SqlParameter> { acceso.CrearParametro("@IdOrdenCompra", idOrdenCompra) });
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        // Reclamos de la orden con sus líneas (para mostrarlos en la recepción)
        public List<BE.RECLAMO> ListarPorOrden(int idOrdenCompra)
        {
            acceso.Abrir();
            DataTable tabla;
            try
            {
                tabla = acceso.Leer("ListarReclamoPorOrden", new List<SqlParameter> { acceso.CrearParametro("@IdOrdenCompra", idOrdenCompra) });
            }
            finally
            {
                acceso.Cerrar();
            }
            var reclamos = new List<BE.RECLAMO>();
            foreach (DataRow u in tabla.Rows)
            {
                int id = Convert.ToInt32(u["IdReclamo"]);
                var r = reclamos.FirstOrDefault(x => x.IdReclamo == id);
                if (r == null)
                {
                    r = new BE.RECLAMO { IdReclamo = id, IdRecepcion = Convert.ToInt32(u["IdRecepcion"]), FechaReclamo = Convert.ToDateTime(u["FechaReclamo"]), Estado = u["Estado"].ToString() };
                    reclamos.Add(r);
                }
                r.Detalles.Add(new BE.RECLAMO_DETALLE
                {
                    IdReclamo = id,
                    IdInsumo = Convert.ToInt32(u["IdInsumo"]),
                    CantidadPendiente = Convert.ToInt32(u["CantidadPendiente"]),
                    Motivo = u["Motivo"].ToString(),
                    Descripcion = u["Descripcion"] == DBNull.Value ? null : u["Descripcion"].ToString(),
                    Insumo = new BE.INSUMO { IdInsumo = Convert.ToInt32(u["IdInsumo"]), Nombre = u["NombreInsumo"].ToString() }
                });
            }
            return reclamos;
        }
    }
}
