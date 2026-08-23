using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MAPPER_BITACORA
    {
        ACCESO acceso = new ACCESO();
        public void GuardarRegistro(BITACORA registro)
        {
            string NombreSp = "InsertarBitacora";
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@IdUsuario", registro.IdUsuario));
            parametros.Add(acceso.CrearParametro("@Actividad", registro.Actividad));
            parametros.Add(acceso.CrearParametro("@InformacionAsociada", registro.InformacionAsociada));
            int filas = acceso.Escribir(NombreSp, parametros);
            acceso.Cerrar();
        }

        public List<BITACORA> ObtenerRegistros(int? idUsuario, string actividad, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            var registros = new List<BITACORA>();
            string NombreSp = "ObtenerBitacora";
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            if (idUsuario.HasValue)
            {
                parametros.Add(acceso.CrearParametro("@IdUsuario", idUsuario.Value));
            }
            else
            {
                parametros.Add(new SqlParameter("@IdUsuario", DBNull.Value));
            }
            if (string.IsNullOrEmpty(actividad))
            {
                parametros.Add(new SqlParameter("@Actividad", DBNull.Value));
            }
            else
            {
                parametros.Add(new SqlParameter("@Actividad", actividad));
            }

            if (fechaDesde.HasValue)
            {
                parametros.Add(acceso.CrearParametro("@FechaDesde", fechaDesde.Value));
            }
            else
            {
                parametros.Add(new SqlParameter("@FechaDesde", DBNull.Value));
            }

            if (fechaHasta.HasValue)
            {
                parametros.Add(acceso.CrearParametro("@FechaHasta", fechaHasta.Value));
            }
            else
            {
                parametros.Add(new SqlParameter("@FechaHasta", DBNull.Value));
            }

            using (SqlCommand cmd = acceso.CrearComando(NombreSp, parametros))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        registros.Add(new BITACORA()
                        {
                            IdBitacora = Convert.ToInt32(reader["IdBitacora"]),
                            FechaHora = Convert.ToDateTime(reader["FechaHora"]),
                            IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                            Actividad = reader["Actividad"].ToString(),
                            InformacionAsociada = reader["InformacionAsociada"].ToString()
                        });
                    }
                }
            }
            acceso.Cerrar();
            return registros;
        }
    }
}
