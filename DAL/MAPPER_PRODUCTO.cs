using BE;
using SERVICIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MAPPER_PRODUCTO
    {
        ACCESO acceso;

        public MAPPER_PRODUCTO()
        {
            acceso = new ACCESO();
        }

        // Para usar el mapper dentro de una transacción ya iniciada (decisión 27)
        public MAPPER_PRODUCTO(ACCESO accesoCompartido)
        {
            acceso = accesoCompartido;
        }

        public int AltaProducto(BE.PRODUCTO Producto)
        {
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@Nombre", Producto.Nombre));
            parametros.Add(acceso.CrearParametro("@Tipo", Producto.Tipo));
            parametros.Add(acceso.CrearParametro("@DVH", Producto.DVH));

            DataTable tabla = acceso.Leer("AltaProducto", parametros);
            acceso.Cerrar();

            if (tabla.Rows.Count > 0)
            {
                return Convert.ToInt32(tabla.Rows[0]["IdProducto"]);
            }
            return 0;
        }

        // Baja lógica: el SP pone Activo = 0 y guarda el DVH recalculado
        public int BajaProducto(BE.PRODUCTO Producto)
        {
            string NombreSp = "BajaProducto";
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@IdProducto", Producto.IdProducto));
            parametros.Add(acceso.CrearParametro("@DVH", Producto.DVH));
            int filas = acceso.Escribir(NombreSp, parametros);
            acceso.Cerrar();
            return filas;
        }

        // Decisión 64: reactivación (Activo = 1) con el DVH recalculado
        public int ReactivarProducto(BE.PRODUCTO Producto)
        {
            acceso.Abrir();
            try
            {
                return acceso.Escribir("ReactivarProducto", new List<SqlParameter>
                {
                    acceso.CrearParametro("@IdProducto", Producto.IdProducto),
                    acceso.CrearParametro("@DVH", Producto.DVH)
                });
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        public int EditarProducto(BE.PRODUCTO Producto)
        {
            string NombreSp = "ModificarProducto";
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@IdProducto", Producto.IdProducto));
            parametros.Add(acceso.CrearParametro("@Nombre", Producto.Nombre));
            parametros.Add(acceso.CrearParametro("@Tipo", Producto.Tipo));
            parametros.Add(acceso.CrearParametro("@DVH", Producto.DVH));
            int filas = acceso.Escribir(NombreSp, parametros);
            acceso.Cerrar();
            return filas;
        }

        // Todos los productos, activos y dados de baja (lo usa el dígito verificador)
        public List<BE.PRODUCTO> ListarProductos()
        {
            return Listar("ListarProducto");
        }

        // Solo los productos activos, para las pantallas de operación
        public List<BE.PRODUCTO> ListarProductosActivos()
        {
            return Listar("ListarProductoActivo");
        }

        private List<BE.PRODUCTO> Listar(string NombreSp)
        {
            List<BE.PRODUCTO> listaProductos = new List<BE.PRODUCTO>();
            acceso.Abrir();

            DataTable tabla = new DataTable();
            tabla = acceso.Leer(NombreSp);
            acceso.Cerrar();
            foreach (DataRow u in tabla.Rows)
            {
                listaProductos.Add(MapearProducto(u));
            }
            return listaProductos;
        }

        private BE.PRODUCTO MapearProducto(DataRow u)
        {
            return new BE.PRODUCTO
            {
                IdProducto = Convert.ToInt32(u["IdProducto"]),
                Nombre = u["Nombre"].ToString(),
                Tipo = u["Tipo"].ToString(),
                Activo = Convert.ToBoolean(u["Activo"]),
                DVH = u["DVH"] == DBNull.Value ? null : u["DVH"].ToString()
            };
        }

        public void ActualizarDVH(int idProducto, string dvh)
        {
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@IdProducto", idProducto),
                acceso.CrearParametro("@DVH", dvh)
            };
            acceso.Escribir("ActualizarDVHProducto", parametros);
            acceso.Cerrar();
        }

        public BE.PRODUCTO ObtenerPorId(int idProducto)
        {
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@IdProducto", idProducto)
            };
            DataTable tabla = acceso.Leer("ObtenerProductoPorId", parametros);
            acceso.Cerrar();

            if (tabla.Rows.Count == 0) return null;

            return MapearProducto(tabla.Rows[0]);
        }

        // CU006 / CU008: guarda el precio de un tamaño del producto (alta o cambio; el SP hace upsert)
        public void GuardarPrecio(int idProducto, BE.PRODUCTO_TAMANIO precio)
        {
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@IdProducto", idProducto),
                acceso.CrearParametro("@IdTamanio", precio.IdTamanio),
                acceso.CrearParametro("@Precio", precio.Precio)
            };
            acceso.Escribir("GuardarProductoTamanio", parametros);
            acceso.Cerrar();
        }

        // CU008: quita un tamaño del producto (fila de relación, decisión 53). El SP lo impide si ya tiene ventas.
        public void QuitarPrecio(int idProducto, int idTamanio)
        {
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@IdProducto", idProducto),
                acceso.CrearParametro("@IdTamanio", idTamanio)
            };
            acceso.Escribir("QuitarProductoTamanio", parametros);
            acceso.Cerrar();
        }

        // CU008: tamaños y precios actuales del producto
        public List<BE.PRODUCTO_TAMANIO> ListarPreciosPorProducto(int idProducto)
        {
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@IdProducto", idProducto)
            };
            DataTable tabla = acceso.Leer("ListarProductoTamanioPorProducto", parametros);
            acceso.Cerrar();
            return tabla.Rows.Cast<DataRow>().Select(MapearPrecio).ToList();
        }

        // CU010 paso 1: productos activos con receta, con cada tamaño y su precio
        public List<BE.PRODUCTO_TAMANIO> ListarProductosConPrecios()
        {
            acceso.Abrir();
            DataTable tabla = acceso.Leer("ListarProductoTamanioActivo");
            acceso.Cerrar();
            return tabla.Rows.Cast<DataRow>().Select(u =>
            {
                var precio = MapearPrecio(u);
                precio.Producto = new BE.PRODUCTO
                {
                    IdProducto = precio.IdProducto,
                    Nombre = u["NombreProducto"].ToString(),
                    Tipo = u["Tipo"].ToString(),
                    Activo = true
                };
                return precio;
            }).ToList();
        }

        private BE.PRODUCTO_TAMANIO MapearPrecio(DataRow u)
        {
            return new BE.PRODUCTO_TAMANIO
            {
                IdProducto = Convert.ToInt32(u["IdProducto"]),
                IdTamanio = Convert.ToInt32(u["IdTamaño"]),
                Precio = Convert.ToDecimal(u["Precio"]),
                Tamanio = new BE.TAMANIO
                {
                    IdTamanio = Convert.ToInt32(u["IdTamaño"]),
                    Nombre = u["NombreTamanio"].ToString(),
                    CantidadMagnitud = Convert.ToDecimal(u["CantidadMagnitud"]),
                    UnidadMagnitud = u["UnidadMagnitud"].ToString(),
                    Activo = true
                }
            };
        }

    }
}
