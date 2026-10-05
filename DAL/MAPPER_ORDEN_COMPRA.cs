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
    public class MAPPER_ORDEN_COMPRA
    {
        ACCESO acceso;

        public MAPPER_ORDEN_COMPRA()
        {
            acceso = new ACCESO();
        }

        // Para usar el mapper dentro de una transacción ya iniciada (decisión 27)
        public MAPPER_ORDEN_COMPRA(ACCESO accesoCompartido)
        {
            acceso = accesoCompartido;
        }

        // CU016 mensajes 41-44: cabecera ("Pendiente de aprobación") y líneas. Devuelve la orden con su Id.
        public BE.ORDEN_COMPRA GuardarOrden(BE.ORDEN_COMPRA orden)
        {
            acceso.Abrir();
            try
            {
                DataTable tabla = acceso.Leer("GuardarOrdenCompra", new List<SqlParameter>
                {
                    acceso.CrearParametro("@IdProveedor", orden.IdProveedor),
                    acceso.CrearParametro("@FechaGeneracion", orden.FechaGeneracion)
                });
                orden.IdOrdenCompra = Convert.ToInt32(tabla.Rows[0]["IdOrdenCompra"]);
                GuardarLineas(orden);
                return orden;
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        // CU018: proveedor y líneas nuevas (las anteriores se reemplazan, filas de relación, decisión 53)
        public void ActualizarOrden(BE.ORDEN_COMPRA orden)
        {
            acceso.Abrir();
            try
            {
                acceso.Escribir("ActualizarOrdenCompra", new List<SqlParameter>
                {
                    acceso.CrearParametro("@IdOrdenCompra", orden.IdOrdenCompra),
                    acceso.CrearParametro("@IdProveedor", orden.IdProveedor)
                });
                acceso.Escribir("QuitarDetalleOrdenCompra", new List<SqlParameter> { acceso.CrearParametro("@IdOrdenCompra", orden.IdOrdenCompra) });
                GuardarLineas(orden);
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        private void GuardarLineas(BE.ORDEN_COMPRA orden)
        {
            foreach (var linea in orden.Detalles)
            {
                linea.IdOrdenCompra = orden.IdOrdenCompra;
                acceso.Escribir("GuardarOrdenCompraDetalle", new List<SqlParameter>
                {
                    acceso.CrearParametro("@IdOrdenCompra", orden.IdOrdenCompra),
                    acceso.CrearParametro("@IdInsumo", linea.IdInsumo),
                    acceso.CrearParametro("@CantidadPedida", linea.CantidadPedida)
                });
            }
        }

        // CU017 / CU018 / CU019 / CU021: cambio de estado. Fecha de aprobación y observaciones solo si vienen informadas.
        public void ActualizarEstado(int idOrdenCompra, string estado, DateTime? fechaAprobacion = null, string observaciones = null)
        {
            acceso.Abrir();
            try
            {
                var parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@IdOrdenCompra", idOrdenCompra),
                    acceso.CrearParametro("@Estado", estado)
                };
                if (fechaAprobacion.HasValue) parametros.Add(acceso.CrearParametro("@FechaAprobacion", fechaAprobacion.Value));
                if (observaciones != null) parametros.Add(acceso.CrearParametro("@Observaciones", observaciones));
                acceso.Escribir("ActualizarEstadoOrdenCompra", parametros);
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        // Cabeceras de las órdenes en un estado, con el proveedor
        public List<BE.ORDEN_COMPRA> ListarPorEstado(string estado)
        {
            DataTable tabla = LeerTabla("ListarOrdenCompraPorEstado", acceso.CrearParametro("@Estado", estado));
            return tabla.Rows.Cast<DataRow>().Select(MapearCabecera).ToList();
        }

        // Consulta de compras: órdenes generadas en [desde, hasta), con el total recibido y el pago. Estado y proveedor son opcionales.
        public List<BE.ORDEN_COMPRA> ListarOrdenes(DateTime desde, DateTime hasta, string estado, int? idProveedor)
        {
            var parametros = new List<SqlParameter> { acceso.CrearParametro("@Desde", desde), acceso.CrearParametro("@Hasta", hasta) };
            // Sin filtro, el parámetro no se envía y el SP usa NULL
            if (estado != null) parametros.Add(acceso.CrearParametro("@Estado", estado));
            if (idProveedor.HasValue) parametros.Add(acceso.CrearParametro("@IdProveedor", idProveedor.Value));
            DataTable tabla = LeerTabla("ListarOrdenCompraPorFecha", parametros.ToArray());
            return tabla.Rows.Cast<DataRow>().Select(u =>
            {
                var orden = MapearCabecera(u);
                orden.TotalRecibido = Convert.ToDecimal(u["TotalRecibido"]);
                if (u["IdPago"] != DBNull.Value)
                    orden.Pago = new BE.PAGO_PROVEEDOR
                    {
                        IdPago = Convert.ToInt32(u["IdPago"]),
                        IdOrdenCompra = orden.IdOrdenCompra,
                        FechaPago = Convert.ToDateTime(u["FechaPago"]),
                        MedioPago = u["MedioPago"].ToString(),
                        Monto = Convert.ToDecimal(u["Monto"]),
                        NumeroComprobante = u["NumeroComprobante"].ToString()
                    };
                return orden;
            }).ToList();
        }

        // Orden con sus líneas y los datos de cada insumo. null si no existe.
        public BE.ORDEN_COMPRA BuscarOrden(int idOrdenCompra)
        {
            DataTable tabla = LeerTabla("BuscarOrdenCompra", acceso.CrearParametro("@IdOrdenCompra", idOrdenCompra));
            if (tabla.Rows.Count == 0) return null;
            var orden = MapearCabecera(tabla.Rows[0]);
            foreach (DataRow u in tabla.Rows)
            {
                if (u["IdInsumo"] == DBNull.Value) continue;
                orden.Detalles.Add(new BE.ORDEN_COMPRA_DETALLE
                {
                    IdOrdenCompra = orden.IdOrdenCompra,
                    IdInsumo = Convert.ToInt32(u["IdInsumo"]),
                    CantidadPedida = Convert.ToInt32(u["CantidadPedida"]),
                    CantidadRecibida = Convert.ToInt32(u["CantidadRecibida"]),
                    Insumo = new BE.INSUMO
                    {
                        IdInsumo = Convert.ToInt32(u["IdInsumo"]),
                        Nombre = u["NombreInsumo"].ToString(),
                        UnidadMedida = u["UnidadMedida"].ToString(),
                        UnidadCompra = u["UnidadCompra"].ToString(),
                        EquivalenciaMagnitud = Convert.ToDecimal(u["EquivalenciaMagnitud"]),
                        CostoUnidadCompra = Convert.ToDecimal(u["CostoUnidadCompra"])
                    }
                });
            }
            return orden;
        }

        // CU019 mensajes 40-43: suma lo recibido a la línea
        public void ActualizarCantidadRecibida(int idOrdenCompra, int idInsumo, int cantidad)
        {
            acceso.Abrir();
            try
            {
                acceso.Escribir("ActualizarCantidadRecibidaOrden", new List<SqlParameter>
                {
                    acceso.CrearParametro("@IdOrdenCompra", idOrdenCompra),
                    acceso.CrearParametro("@IdInsumo", idInsumo),
                    acceso.CrearParametro("@Cantidad", cantidad)
                });
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        // Órdenes abiertas (decisión 49: ni Cerrada ni Pagada) del proveedor, con los Id de sus insumos (CU026 FA3, CU027 FA1)
        public List<BE.ORDEN_COMPRA> ListarAbiertasPorProveedor(int idProveedor)
        {
            DataTable tabla = LeerTabla("ListarOrdenAbiertaPorProveedor", acceso.CrearParametro("@IdProveedor", idProveedor));
            return tabla.Rows.Cast<DataRow>().Select(u =>
            {
                var orden = new BE.ORDEN_COMPRA { IdOrdenCompra = Convert.ToInt32(u["IdOrdenCompra"]), IdProveedor = idProveedor, Estado = u["Estado"].ToString() };
                if (u["Insumos"] != DBNull.Value)
                    foreach (string id in u["Insumos"].ToString().Split(','))
                        orden.Detalles.Add(new BE.ORDEN_COMPRA_DETALLE { IdOrdenCompra = orden.IdOrdenCompra, IdInsumo = int.Parse(id) });
                return orden;
            }).ToList();
        }

        // Órdenes abiertas que incluyen el insumo (CU025 FA2)
        public List<BE.ORDEN_COMPRA> ListarAbiertasPorInsumo(int idInsumo)
        {
            DataTable tabla = LeerTabla("ListarOrdenAbiertaPorInsumo", acceso.CrearParametro("@IdInsumo", idInsumo));
            return tabla.Rows.Cast<DataRow>().Select(u => new BE.ORDEN_COMPRA
            {
                IdOrdenCompra = Convert.ToInt32(u["IdOrdenCompra"]),
                Estado = u["Estado"].ToString()
            }).ToList();
        }

        private BE.ORDEN_COMPRA MapearCabecera(DataRow u)
        {
            return new BE.ORDEN_COMPRA
            {
                IdOrdenCompra = Convert.ToInt32(u["IdOrdenCompra"]),
                IdProveedor = Convert.ToInt32(u["IdProveedor"]),
                FechaGeneracion = Convert.ToDateTime(u["FechaGeneracion"]),
                FechaAprobacion = u["FechaAprobacion"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(u["FechaAprobacion"]),
                Estado = u["Estado"].ToString(),
                Observaciones = u["Observaciones"] == DBNull.Value ? null : u["Observaciones"].ToString(),
                Proveedor = new BE.PROVEEDOR { IdProveedor = Convert.ToInt32(u["IdProveedor"]), RazonSocial = u["RazonSocial"].ToString() }
            };
        }

        private DataTable LeerTabla(string sp, params SqlParameter[] parametros)
        {
            acceso.Abrir();
            try
            {
                return acceso.Leer(sp, parametros.ToList());
            }
            finally
            {
                acceso.Cerrar();
            }
        }
    }
}
