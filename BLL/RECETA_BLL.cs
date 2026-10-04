using BE;
using DAL;
using SERVICIO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class RECETA_BLL
    {
        MAPPER_RECETA GestorReceta = new MAPPER_RECETA();
        MAPPER_PRODUCTO GestorProducto = new MAPPER_PRODUCTO();
        MAPPER_INSUMO GestorInsumo = new MAPPER_INSUMO();

        // CU013 paso 1: productos activos sin receta (FA1 si la lista está vacía)
        public List<BE.PRODUCTO> ListarProductosSinReceta()
        {
            return GestorReceta.ListarProductosSinReceta();
        }

        // CU014 paso 1: productos activos con receta (se informa si la lista está vacía)
        public List<BE.PRODUCTO> ListarProductosConReceta()
        {
            return GestorReceta.ListarProductosConReceta();
        }

        // CU014 paso 2: receta actual del producto, incluidos los insumos dados de baja
        public List<BE.RECETA> ObtenerReceta(int idProducto)
        {
            return GestorReceta.BuscarRecetaPorProducto(idProducto);
        }

        // CU013 paso 4 / FA2. Las excepciones llevan la clave de idioma del mensaje.
        public bool ValidarReceta(List<BE.RECETA> receta)
        {
            if (receta == null || receta.Count == 0)
                throw new ArgumentException("msgRecetaSinInsumos");

            if (receta.Any(l => l.Proporcion <= 0))
                throw new ArgumentException("msgRecetaProporcionInvalida");

            if (receta.GroupBy(l => l.IdInsumo).Any(g => g.Count() > 1))
                throw new ArgumentException("msgRecetaInsumoRepetido");

            // Solo insumos activos (decisión 7)
            foreach (var linea in receta)
            {
                var insumo = GestorInsumo.BuscarInsumo(linea.IdInsumo);
                if (insumo == null || !insumo.Activo)
                    throw new ArgumentException("msgRecetaInsumoInactivo");
            }
            return true;
        }

        // CU013 paso 5. Condición del CU: el producto no debe tener receta registrada.
        public void InsertarReceta(int idProducto, List<BE.RECETA> receta)
        {
            var producto = GestorProducto.ObtenerPorId(idProducto);
            if (producto == null || !producto.Activo)
                throw new ArgumentException("msgRecetaProductoInvalido");
            if (GestorReceta.BuscarRecetaPorProducto(idProducto).Count > 0)
                throw new ArgumentException("msgRecetaYaExiste");

            ValidarReceta(receta);
            GestorReceta.GuardarReceta(idProducto, receta);

            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Alta de receta", $"Se registró la receta del producto: {producto.Nombre}");
        }

        // CU014 pasos 4 y 5. Condición del CU: el producto debe tener receta registrada.
        public void ModificarReceta(int idProducto, List<BE.RECETA> receta)
        {
            var producto = GestorProducto.ObtenerPorId(idProducto);
            if (producto == null || !producto.Activo)
                throw new ArgumentException("msgRecetaProductoInvalido");
            if (GestorReceta.BuscarRecetaPorProducto(idProducto).Count == 0)
                throw new ArgumentException("msgRecetaNoExiste");

            ValidarReceta(receta);
            GestorReceta.ActualizarReceta(idProducto, receta);

            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Modificación de receta", $"Se modificó la receta del producto: {producto.Nombre}");
        }
    }
}
