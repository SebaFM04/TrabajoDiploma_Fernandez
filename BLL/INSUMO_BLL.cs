using BE;
using DAL;
using SERVICIO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BLL
{
    public class INSUMO_BLL
    {
        MAPPER_INSUMO GestorInsumo = new MAPPER_INSUMO();

        // Error que lanza un SP con RAISERROR (nombre duplicado)
        private const int ErrorDeNegocioSql = 50000;

        // Unidades en que se lleva el stock (decisión 4)
        public string[] ListarUnidadesMedida()
        {
            return new[] { "ml", "g", "un" };
        }

        // Reglas de docs/04_ABM_INSUMOS.md. Las excepciones llevan la clave de idioma del mensaje.
        private void ValidarInsumo(BE.INSUMO insumo)
        {
            if (string.IsNullOrWhiteSpace(insumo.Nombre))
                throw new ArgumentException("msgInsumoNombreObligatorio");
            insumo.Nombre = insumo.Nombre.Trim();
            if (insumo.Nombre.Length > 100)
                throw new ArgumentException("msgInsumoNombreLargo");

            if (Array.IndexOf(ListarUnidadesMedida(), insumo.UnidadMedida) < 0)
                throw new ArgumentException("msgInsumoUnidadMedidaObligatoria");

            if (string.IsNullOrWhiteSpace(insumo.UnidadCompra))
                throw new ArgumentException("msgInsumoUnidadCompraObligatoria");
            insumo.UnidadCompra = insumo.UnidadCompra.Trim();
            if (insumo.UnidadCompra.Length > 50)
                throw new ArgumentException("msgInsumoUnidadCompraLarga");

            if (insumo.EquivalenciaMagnitud <= 0)
                throw new ArgumentException("msgInsumoEquivalenciaInvalida");

            if (insumo.CostoUnidadCompra < 0 || insumo.UmbralReposicion < 0 || insumo.VolumenPesoDisponible < 0)
                throw new ArgumentException("msgInsumoValorNegativo");
        }

        // CU023 paso 3 (decisión 52): el stock inicial se ingresa en unidades de compra
        // y se lleva en la unidad de medida. Ej.: 4 botellas × 2250 ml = 9000 ml.
        public decimal CalcularStockInicial(decimal cantidadUnidadesCompra, decimal equivalencia)
        {
            if (cantidadUnidadesCompra < 0)
                throw new ArgumentException("msgInsumoValorNegativo");
            return Math.Round(cantidadUnidadesCompra * equivalencia, 3);
        }

        // CU023 Insertar Insumo
        public void InsertarInsumo(BE.INSUMO insumo)
        {
            ValidarInsumo(insumo);
            // El aviso de stock bajo lo maneja el sistema: el insumo nace activo y sin aviso
            insumo.AvisoStockBajo = false;
            insumo.Activo = true;

            try
            {
                insumo.IdInsumo = GestorInsumo.AltaInsumo(insumo);
            }
            catch (SqlException ex) when (ex.Number == ErrorDeNegocioSql)
            {
                // FA2: el SP detectó un insumo con el mismo nombre (activo o dado de baja)
                throw new ArgumentException("msgInsumoDuplicado", ex);
            }

            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Alta de insumo", $"Se agrego el insumo: {insumo.Nombre}");
        }

        // CU024 Modificar Insumo
        public void ModificarInsumo(BE.INSUMO insumo)
        {
            var insumoActual = GestorInsumo.BuscarInsumo(insumo.IdInsumo);
            if (insumoActual == null || !insumoActual.Activo)
                throw new ArgumentException("msgInsumoInexistente");

            // La unidad de medida y el stock no se modifican (decisión 37): se conservan los actuales
            insumo.UnidadMedida = insumoActual.UnidadMedida;
            insumo.VolumenPesoDisponible = insumoActual.VolumenPesoDisponible;
            insumo.AvisoStockBajo = insumoActual.AvisoStockBajo;
            insumo.Activo = insumoActual.Activo;
            ValidarInsumo(insumo);

            try
            {
                GestorInsumo.ModificarInsumo(insumo);
            }
            catch (SqlException ex) when (ex.Number == ErrorDeNegocioSql)
            {
                // FA2: otro insumo (activo o dado de baja) ya tiene ese nombre
                throw new ArgumentException("msgInsumoDuplicado", ex);
            }

            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Modificación de insumo", $"Se modificó el insumo: {insumo.Nombre}");
        }

        // CU025 paso 2 / FA1: productos activos que usan el insumo en su receta.
        // Si la lista no está vacía, el insumo no se puede dar de baja.
        public List<BE.PRODUCTO> VerificarBajaInsumo(int idInsumo)
        {
            // N02: acá se suma el control de órdenes de compra no cerradas (FA2), cuando exista ORDEN_COMPRA
            return GestorInsumo.ListarProductosActivosPorInsumo(idInsumo);
        }

        // CU025 Dar de Baja Insumo: baja lógica. Un stock mayor a cero no la impide.
        public void DarDeBajaInsumo(int idInsumo)
        {
            var insumo = GestorInsumo.BuscarInsumo(idInsumo);
            if (insumo == null || !insumo.Activo)
                throw new ArgumentException("msgInsumoInexistente");

            if (VerificarBajaInsumo(idInsumo).Count > 0)
                throw new ArgumentException("msgInsumoEnReceta");

            GestorInsumo.BajaInsumo(idInsumo);

            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Baja de insumo", $"Se dio de baja el insumo: {insumo.Nombre}");
        }

        public List<BE.INSUMO> ListarInsumosActivos()
        {
            return GestorInsumo.ListarInsumosActivos();
        }
    }
}
