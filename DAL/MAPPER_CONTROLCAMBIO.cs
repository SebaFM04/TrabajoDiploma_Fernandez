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
    public class MAPPER_CONTROLCAMBIO
    {
        ACCESO acceso = new ACCESO();

        public void RegistrarCambio(CONTROLCAMBIO cambio)
        {
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@IdUsuario",         cambio.IdUsuario),
                acceso.CrearParametro("@IdProducto",        cambio.IdProducto),
                acceso.CrearParametro("@CampoModificado",   cambio.CampoModificado),
                acceso.CrearParametro("@ValorAnterior",     cambio.ValorAnterior ?? ""),
                acceso.CrearParametro("@ValorActual",       cambio.ValorActual   ?? ""),
                acceso.CrearParametro("@FechaModificacion", cambio.FechaModificacion),
                acceso.CrearParametro("@TipoOperacion",     cambio.TipoOperacion)
            };
            acceso.Escribir("AltaControlCambios", parametros);
            acceso.Cerrar();
        }

        public List<CONTROLCAMBIO> ListarTodos()
        {
            var lista = new List<CONTROLCAMBIO>();
            acceso.Abrir();
            DataTable tabla = acceso.Leer("ListarControlCambios");
            acceso.Cerrar();

            foreach (DataRow row in tabla.Rows)
            {
                lista.Add(MapearFila(row));
            }
            return lista;
        }

        public List<CONTROLCAMBIO> ListarPorProducto(int idProducto)
        {
            var lista = new List<CONTROLCAMBIO>();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@IdProducto", idProducto)
            };
            DataTable tabla = acceso.Leer("ListarCambiosPorProducto", parametros);
            acceso.Cerrar();

            foreach (DataRow row in tabla.Rows)
            {
                lista.Add(MapearFila(row));

            }
            return lista;
        }

        private CONTROLCAMBIO MapearFila(DataRow row)
        {
            return new CONTROLCAMBIO
            {
                IdCambio = Convert.ToInt32(row["IdCambio"]),
                IdUsuario = Convert.ToInt32(row["IdUsuario"]),
                IdProducto = Convert.ToInt32(row["IdProducto"]),
                CampoModificado = row["CampoModificado"].ToString(),
                ValorAnterior = row["ValorAnterior"].ToString(),
                ValorActual = row["ValorActual"].ToString(),
                FechaModificacion = Convert.ToDateTime(row["FechaModificacion"]),
                TipoOperacion = row["TipoOperacion"].ToString()
            };
        }
    }
}
