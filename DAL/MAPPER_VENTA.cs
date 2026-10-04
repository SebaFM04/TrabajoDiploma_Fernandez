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
    public class MAPPER_VENTA
    {
        ACCESO acceso;

        public MAPPER_VENTA()
        {
            acceso = new ACCESO();
        }

        // CU010: se usa dentro de la transacción de la venta (decisión 27)
        public MAPPER_VENTA(ACCESO accesoCompartido)
        {
            acceso = accesoCompartido;
        }

        // CU010 mensajes 36-39: guarda la venta y sus detalles (decisión 53). Devuelve la venta con sus Id.
        public BE.VENTA GuardarVenta(BE.VENTA venta)
        {
            acceso.Abrir();
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@Fecha", venta.Fecha),
                    acceso.CrearParametro("@Monto", venta.Monto),
                    acceso.CrearParametro("@MedioPago", venta.MedioPago)
                };
                DataTable tabla = acceso.Leer("GuardarVenta", parametros);
                venta.IdVenta = Convert.ToInt32(tabla.Rows[0]["IdVenta"]);

                foreach (var detalle in venta.Detalles)
                {
                    detalle.IdVenta = venta.IdVenta;
                    List<SqlParameter> pDetalle = new List<SqlParameter>
                    {
                        acceso.CrearParametro("@IdVenta", venta.IdVenta),
                        acceso.CrearParametro("@IdProducto", detalle.IdProducto),
                        acceso.CrearParametro("@IdTamanio", detalle.IdTamanio),
                        acceso.CrearParametro("@Cantidad", detalle.Cantidad),
                        acceso.CrearParametro("@MontoLinea", detalle.MontoLinea)
                    };
                    DataTable tDetalle = acceso.Leer("GuardarVentaDetalle", pDetalle);
                    detalle.IdVentaDetalle = Convert.ToInt32(tDetalle.Rows[0]["IdVentaDetalle"]);
                }
                return venta;
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        // Consulta de ventas (decisión 56): ventas del período [desde, hasta) con vale, factura y comanda
        public List<BE.VENTA> ListarVentas(DateTime desde, DateTime hasta)
        {
            acceso.Abrir();
            DataTable tabla;
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@Desde", desde),
                    acceso.CrearParametro("@Hasta", hasta)
                };
                tabla = acceso.Leer("ListarVentasPorFecha", parametros);
            }
            finally
            {
                acceso.Cerrar();
            }

            var ventas = new List<BE.VENTA>();
            foreach (DataRow u in tabla.Rows)
            {
                var venta = new BE.VENTA
                {
                    IdVenta = Convert.ToInt32(u["IdVenta"]),
                    Fecha = Convert.ToDateTime(u["Fecha"]),
                    Monto = Convert.ToDecimal(u["Monto"]),
                    MedioPago = u["MedioPago"].ToString()
                };
                if (u["IdVale"] != DBNull.Value)
                    venta.Vale = new BE.VALE { IdVale = Convert.ToInt32(u["IdVale"]), IdVenta = venta.IdVenta, MontoCertificado = venta.Monto, Utilizado = Convert.ToBoolean(u["Utilizado"]) };
                if (u["IdFactura"] != DBNull.Value)
                    venta.Factura = new BE.FACTURA
                    {
                        IdFactura = Convert.ToInt32(u["IdFactura"]),
                        IdVenta = venta.IdVenta,
                        NumeroComprobante = Convert.ToInt32(u["NumeroComprobante"]),
                        FechaHoraEmision = Convert.ToDateTime(u["FechaFactura"]),
                        NombreCliente = u["NombreCliente"] == DBNull.Value ? null : u["NombreCliente"].ToString(),
                        TelefonoCliente = u["TelefonoCliente"] == DBNull.Value ? null : u["TelefonoCliente"].ToString(),
                        CorreoCliente = u["CorreoCliente"] == DBNull.Value ? null : u["CorreoCliente"].ToString(),
                        Total = venta.Monto
                    };
                if (u["IdComanda"] != DBNull.Value)
                    venta.Comanda = new BE.COMANDA { IdComanda = Convert.ToInt32(u["IdComanda"]), IdVenta = venta.IdVenta, Estado = u["EstadoComanda"].ToString() };
                ventas.Add(venta);
            }
            return ventas;
        }

        // Consulta de ventas: líneas de una venta con producto y tamaño
        public List<BE.VENTA_DETALLE> BuscarDetalle(int idVenta)
        {
            acceso.Abrir();
            DataTable tabla;
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@IdVenta", idVenta)
                };
                tabla = acceso.Leer("ListarDetalleVenta", parametros);
            }
            finally
            {
                acceso.Cerrar();
            }

            var detalles = new List<BE.VENTA_DETALLE>();
            foreach (DataRow u in tabla.Rows)
            {
                int cantidad = Convert.ToInt32(u["Cantidad"]);
                decimal montoLinea = Convert.ToDecimal(u["MontoLinea"]);
                detalles.Add(new BE.VENTA_DETALLE
                {
                    IdVentaDetalle = Convert.ToInt32(u["IdVentaDetalle"]),
                    IdVenta = idVenta,
                    IdProducto = Convert.ToInt32(u["IdProducto"]),
                    IdTamanio = Convert.ToInt32(u["IdTamaño"]),
                    Cantidad = cantidad,
                    MontoLinea = montoLinea,
                    ProductoTamanio = new BE.PRODUCTO_TAMANIO
                    {
                        IdProducto = Convert.ToInt32(u["IdProducto"]),
                        IdTamanio = Convert.ToInt32(u["IdTamaño"]),
                        // Precio al momento de la venta (puede haber cambiado después)
                        Precio = cantidad > 0 ? montoLinea / cantidad : 0,
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
            return detalles;
        }
    }
}
