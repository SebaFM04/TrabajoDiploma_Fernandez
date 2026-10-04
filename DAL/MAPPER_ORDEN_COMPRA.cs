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
