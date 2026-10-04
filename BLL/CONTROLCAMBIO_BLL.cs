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
                case "Nombre":
                    producto.Nombre = cambio.ValorAnterior;
                    break;
                case "Tipo":
                    producto.Tipo = cambio.ValorAnterior;
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
                    case "Nombre":
                        if (producto.Nombre != cambio.ValorAnterior)
                        { producto.Nombre = cambio.ValorAnterior; huboCambiosReales = true; }
                        break;
                    case "Tipo":
                        if (producto.Tipo != cambio.ValorAnterior)
                        { producto.Tipo = cambio.ValorAnterior; huboCambiosReales = true; }
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
