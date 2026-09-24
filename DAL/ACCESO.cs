using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;


namespace DAL
{
    public class ACCESO
    {
        SqlConnection Conexion;
        public static string ObtenerCadena()
        {
            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "conexion.txt");
            string instancia = File.Exists(ruta) ? File.ReadAllText(ruta).Trim() : @".";
            return $"Data Source={instancia};Initial Catalog=TpIngSoftware_2026;Integrated Security=True;TrustServerCertificate=True;";
        }
        public void Abrir()
        {
            Conexion = new SqlConnection();
            if (Conexion.ConnectionString == "")
            {
                Conexion.ConnectionString = ObtenerCadena();
            }
            Conexion.Open();
        }
        public void Cerrar()
        {
            Conexion.Close();
            GC.Collect();
            Conexion = null;
        }
        public SqlCommand CrearComando(string nombreSP, List<SqlParameter> parametros = null)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandText = nombreSP;
            comando.CommandType = CommandType.StoredProcedure;
            comando.Connection = Conexion;

            if (parametros != null)
            {
                foreach (SqlParameter p in parametros)
                {
                    comando.Parameters.Add(p);
                }
            }
            return comando;
        }

        public SqlCommand CrearComando2(string sql, List<SqlParameter> parametros = null)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandText = sql;
            comando.CommandType = CommandType.Text;
            comando.Connection = Conexion;

            if (parametros != null)
            {
                foreach (SqlParameter p in parametros)
                {
                    comando.Parameters.Add(p);
                }
            }
            return comando;
        }

        public int Escribir(string nombreSP, List<SqlParameter> parametros = null)
        {
            SqlCommand comando = CrearComando(nombreSP, parametros);
            int FilasAfectadas = 0;
            try
            {
                FilasAfectadas = comando.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                throw;
            }
            return FilasAfectadas;
        }
        public DataTable Leer(string nombreSP, List<SqlParameter> parametros = null)
        {
            DataTable tabla = new DataTable();
            SqlDataAdapter adaptador = new SqlDataAdapter();
            adaptador.SelectCommand = CrearComando(nombreSP, parametros);
            adaptador.Fill(tabla);
            return tabla;
        }
        public int LeerEscalar(string sql, List<SqlParameter> parametros = null)
        {
            SqlCommand cmd = CrearComando(sql, parametros);
            int resultado = int.Parse(cmd.ExecuteScalar().ToString());
            return resultado;
        }

        public SqlParameter CrearParametro(string nombre, string valor)
        {
            SqlParameter p = new SqlParameter();
            p.ParameterName = nombre;
            p.Value = valor;
            p.DbType = DbType.String;
            return p;
        }
        public SqlParameter CrearParametro(string nombre, decimal valor)
        {
            SqlParameter p = new SqlParameter();
            p.ParameterName = nombre;
            p.Value = valor;
            p.DbType = DbType.Decimal;
            return p;
        }
        public SqlParameter CrearParametro(string nombre, DateTime valor)
        {
            SqlParameter p = new SqlParameter();
            p.ParameterName = nombre;
            p.Value = valor;
            p.DbType = DbType.DateTime;
            return p;
        }
        public SqlParameter CrearParametro(string nombre, int valor)
        {
            SqlParameter p = new SqlParameter();
            p.ParameterName = nombre;
            p.Value = valor;
            p.DbType = DbType.Int32;
            return p;
        }
    }
}
