using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CONTROLCAMBIO_BLL
    {
        MAPPER_CONTROLCAMBIO mapperCambios = new MAPPER_CONTROLCAMBIO();
        MAPPER_PRODUCTO mapperProducto = new MAPPER_PRODUCTO();
        DIGITOVERIFICADOR_BLL dvBLL = new DIGITOVERIFICADOR_BLL();

        public void RegistrarCambio(int idUsuario, int idProducto, string campo, string valorAnterior, string valorActual, string tipoOperacion)
        {
            var cambio = new CONTROLCAMBIO
            {
                IdUsuario = idUsuario,
                IdProducto = idProducto,
                CampoModificado = campo,
                ValorAnterior = valorAnterior,
                ValorActual = valorActual,
                FechaModificacion = DateTime.Now,
                TipoOperacion = tipoOperacion
            };
            mapperCambios.RegistrarCambio(cambio);
        }

        public List<CONTROLCAMBIO> ListarTodos()
        {
            return mapperCambios.ListarTodos();
        }

        public List<CONTROLCAMBIO> ListarPorProducto(int idProducto)
        {
            return mapperCambios.ListarPorProducto(idProducto);
        }

        // ── Revertir un campo al valor anterior ──────────────────
        public void RevertirCambio(CONTROLCAMBIO cambio, int idUsuarioActual)
        {
            var producto = mapperProducto.ObtenerPorId(cambio.IdProducto);
            if (producto == null)
                throw new Exception($"No se encontró el producto ID {cambio.IdProducto}.");

            string valorActualAntesDerevertir = cambio.ValorActual;

            switch (cambio.CampoModificado)
            {
                case "NombreProducto":
                    producto.NombreProducto = cambio.ValorAnterior;
                    break;
                case "PrecioProducto":
                    producto.PrecioProducto = decimal.Parse(cambio.ValorAnterior);
                    break;
                case "TipoProducto":
                    producto.TipoProducto = cambio.ValorAnterior;
                    break;
                case "Descripcion":
                    producto.Descripcion = cambio.ValorAnterior;
                    break;
                case "Cantidad":
                    producto.Cantidad = int.Parse(cambio.ValorAnterior);
                    break;
                case "CodigoProducto":
                    producto.CodigoProducto = int.Parse(cambio.ValorAnterior);
                    break;
                default:
                    throw new Exception($"Campo '{cambio.CampoModificado}' no reconocido.");
            }

            // Recalcular DVH antes de guardar
            producto.DVH = dvBLL.CalcularDVH(producto);
            mapperProducto.EditarProducto(producto);
            dvBLL.RecalcularDV();

            // Registrar el cambio de reversión
            RegistrarCambio(idUsuarioActual, cambio.IdProducto, cambio.CampoModificado, valorActualAntesDerevertir, cambio.ValorAnterior, "Reversión");
        }
        //Nuevo Entrega 3
        public void RevertirTodo(int idProducto, int idUsuarioActual)
        {
            var cambios = mapperCambios.ListarPorProducto(idProducto)
                .Where(c => c.TipoOperacion == "Modificación")
                .OrderByDescending(c => c.IdCambio)
                .ToList();

            if (cambios.Count == 0)
                throw new Exception("No hay cambios de modificación para revertir.");

            var producto = mapperProducto.ObtenerPorId(idProducto);
            if (producto == null)
                throw new Exception($"No se encontró el producto ID {idProducto}.");

            var camposARevertir = cambios
                .GroupBy(c => c.CampoModificado)
                .Select(g => g.First())
                .ToList();

            bool huboCambiosReales = false;

            foreach (var cambio in camposARevertir)
            {
                switch (cambio.CampoModificado)
                {
                    case "NombreProducto":
                        if (producto.NombreProducto != cambio.ValorAnterior)
                        { producto.NombreProducto = cambio.ValorAnterior; huboCambiosReales = true; }
                        break;
                    case "PrecioProducto":
                        var precioAnterior = decimal.Parse(cambio.ValorAnterior);
                        if (producto.PrecioProducto != precioAnterior)
                        { producto.PrecioProducto = precioAnterior; huboCambiosReales = true; }
                        break;
                    case "TipoProducto":
                        if (producto.TipoProducto != cambio.ValorAnterior)
                        { producto.TipoProducto = cambio.ValorAnterior; huboCambiosReales = true; }
                        break;
                    case "Descripcion":
                        if (producto.Descripcion != cambio.ValorAnterior)
                        { producto.Descripcion = cambio.ValorAnterior; huboCambiosReales = true; }
                        break;
                    case "Cantidad":
                        var cantidadAnterior = int.Parse(cambio.ValorAnterior);
                        if (producto.Cantidad != cantidadAnterior)
                        { producto.Cantidad = cantidadAnterior; huboCambiosReales = true; }
                        break;
                    case "CodigoProducto":
                        var codigoAnterior = int.Parse(cambio.ValorAnterior);
                        if (producto.CodigoProducto != codigoAnterior)
                        { producto.CodigoProducto = codigoAnterior; huboCambiosReales = true; }
                        break;
                }
            }

            if (!huboCambiosReales)
                throw new Exception("No hay cambios nuevos que revertir: el producto ya está en su estado anterior.");

            producto.DVH = dvBLL.CalcularDVH(producto);
            mapperProducto.EditarProducto(producto);
            dvBLL.RecalcularDV();

            RegistrarCambio(idUsuarioActual, idProducto,
                "Reversión Total", "Múltiples campos",
                "Estado anterior restaurado", "Reversión Total");
        }
    }
}
