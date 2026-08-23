using BE;
using DAL;
using SERVICIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class DIGITOVERIFICADOR_BLL
    {
        MAPPER_DIGITOVERIFICADOR mapperDV = new MAPPER_DIGITOVERIFICADOR();
        MAPPER_PRODUCTO mapperProducto = new MAPPER_PRODUCTO();
        MAPPER_BACKUP mapperBackup = new MAPPER_BACKUP();
        MAPPER_RESTORE mapperRestore = new MAPPER_RESTORE();

        // ── DVH: suma ponderada por posición de atributo y carácter
        // Orden fijo de atributos:
        //   pos 1: NombreProducto
        //   pos 2: PrecioProducto
        //   pos 3: TipoProducto
        //   pos 4: Descripcion
        //   pos 5: Cantidad
        //   pos 6: CodigoProducto
        public string CalcularDVH(PRODUCTO producto)
        {
            var atributos = new List<string>
            {
                producto.IdProducto.ToString(),
                producto.NombreProducto ?? "",
                producto.PrecioProducto.ToString("F2", CultureInfo.InvariantCulture),
                producto.TipoProducto   ?? "",
                producto.Descripcion    ?? "",
                producto.Cantidad.ToString(),
                producto.CodigoProducto.ToString()
            };

            long suma = 0;
            int posAtributo = 1;

            foreach (string valor in atributos)
            {
                int posChar = 1;
                foreach (char c in valor)
                {
                    suma += (long)c * posChar * posAtributo;
                    posChar++;
                }
                posAtributo++;
            }

            return ENCRIPTADOR.Hash(suma.ToString());
        }

        private string CalcularDVV(List<PRODUCTO> productos)
        {
            long suma = 0;
            int posFila = 1;
            foreach (var p in productos)
            {
                int posChar = 1;
                foreach (char c in (p.DVH ?? ""))
                {
                    suma += (long)c * posChar * posFila;
                    posChar++;
                }
                posFila++;
            }
            return ENCRIPTADOR.Hash(suma.ToString());
        }

        public void RecalcularDV()
        {
            var productos = mapperProducto.ListarProductos();

            foreach (var p in productos)
            {
                string nuevoDVH = CalcularDVH(p);
                if (p.DVH != nuevoDVH)
                {
                    p.DVH = nuevoDVH;
                    mapperProducto.ActualizarDVH(p.IdProducto, nuevoDVH);
                }
            }

            // Recargar con DVH actualizados
            productos = mapperProducto.ListarProductos();
            string dvv = CalcularDVV(productos);
            mapperDV.GuardarDVV(new DIGITOVERIFICADOR
            {
                NombreTabla = "PRODUCTO",
                DVV = dvv
            });
        }

        public List<string> VerificarIntegridad()
        {
            var errores = new List<string>();
            var productos = mapperProducto.ListarProductos();

            foreach (var p in productos)
            {
                string dvhCalculado = CalcularDVH(p);
                if (p.DVH != dvhCalculado)
                {
                    errores.Add($"Error horizontal en producto ID {p.IdProducto} " + $"('{p.NombreProducto}'): la fila fue alterada.");
                }          
            }

            var dvvAlmacenado = mapperDV.ObtenerDVV("PRODUCTO");
            string dvvCalculado = CalcularDVV(productos);

            if (dvvAlmacenado == null || dvvAlmacenado.DVV != dvvCalculado)
            {
                errores.Add("Error vertical en PRODUCTO: se agregaron, " + "quitaron o reordenaron filas fuera del sistema.");
            }          
            return errores;
        }

        public void HacerBackup(string ruta)
        {
            mapperBackup.GenerarBackup(ruta);
        }

        public void RestaurarDesdeBackup(string ruta)
        {
            mapperRestore.RestaurarBackup(ruta);
            RecalcularDV();
        }

        public DateTime? ObtenerFechaUltimoBackup()
        {
            return null;
        }
    }
}
