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

        public List<BE.INSUMO> ListarInsumosActivos()
        {
            return GestorInsumo.ListarInsumosActivos();
        }
    }
}
