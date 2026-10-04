using BE;
using DAL;
using SERVICIO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace BLL
{
    // ABM de proveedores: CU022 Insertar, CU026 Modificar y CU027 Dar de Baja Proveedor (docs/05_ABM_PROVEEDORES.md)
    public class PROVEEDOR_BLL
    {
        MAPPER_PROVEEDOR GestorProveedor = new MAPPER_PROVEEDOR();
        MAPPER_ORDEN_COMPRA GestorOrden = new MAPPER_ORDEN_COMPRA();
        // Error que lanza un SP con RAISERROR (CUIT repetido)
        private const int ErrorDeNegocioSql = 50000;

        public List<BE.PROVEEDOR> ListarProveedores()
        {
            return GestorProveedor.ListarProveedores();
        }

        public List<BE.INSUMO> ListarInsumosDeProveedor(int idProveedor)
        {
            return GestorProveedor.ListarInsumosDeProveedor(idProveedor);
        }

        // FA1 de CU022 y CU026. Las excepciones llevan la clave de idioma del mensaje.
        private void ValidarProveedor(BE.PROVEEDOR p)
        {
            p.RazonSocial = (p.RazonSocial ?? string.Empty).Trim();
            p.Telefono = (p.Telefono ?? string.Empty).Trim();
            p.Correo = (p.Correo ?? string.Empty).Trim();
            p.CUIT = NormalizarCuit(p.CUIT);
            if (p.RazonSocial.Length == 0 || p.Telefono.Length == 0 || p.Correo.Length == 0 || string.IsNullOrEmpty(p.CUIT))
                throw new ArgumentException("msgProveedorDatosObligatorios");
            if (p.RazonSocial.Length > 100 || p.Telefono.Length > 30 || p.Correo.Length > 100)
                throw new ArgumentException("msgProveedorDatoLargo");
            if (!Regex.IsMatch(p.CUIT, @"^\d{2}-\d{8}-\d$"))
                throw new ArgumentException("msgProveedorCuitInvalido");
            if (!Regex.IsMatch(p.Correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("msgProveedorCorreoInvalido");
            if (p.Insumos == null || p.Insumos.Count == 0)
                throw new ArgumentException("msgProveedorSinInsumos");
        }

        // Acepta "30712345678" o "30-71234567-8" y lo deja como XX-XXXXXXXX-X
        private string NormalizarCuit(string cuit)
        {
            string digitos = new string((cuit ?? string.Empty).Where(char.IsDigit).ToArray());
            if (digitos.Length == 11 && Regex.IsMatch((cuit ?? string.Empty).Trim(), @"^[\d-]+$"))
                return $"{digitos.Substring(0, 2)}-{digitos.Substring(2, 8)}-{digitos.Substring(10)}";
            return (cuit ?? string.Empty).Trim();
        }

        // CU022 Insertar Proveedor: proveedor e insumos en una transacción
        public void InsertarProveedor(BE.PROVEEDOR proveedor)
        {
            ValidarProveedor(proveedor);
            var acceso = new ACCESO();
            acceso.IniciarTransaccion();
            try
            {
                new MAPPER_PROVEEDOR(acceso).GuardarProveedor(proveedor);
                acceso.ConfirmarTransaccion();
            }
            catch (SqlException ex) when (ex.Number == ErrorDeNegocioSql)
            {
                acceso.DeshacerTransaccion();
                // FA2: ya existe un proveedor con ese CUIT (activo o dado de baja)
                throw new ArgumentException("msgProveedorDuplicado", ex);
            }
            catch
            {
                acceso.DeshacerTransaccion();
                throw;
            }
            proveedor.Activo = true;
            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Alta de proveedor", $"Se agregó el proveedor: {proveedor.RazonSocial} ({proveedor.CUIT})");
        }

        // CU026 FA3: órdenes abiertas de este proveedor que incluyen alguno de los insumos que se quitan
        public List<BE.ORDEN_COMPRA> VerificarInsumosQuitados(BE.PROVEEDOR proveedor)
        {
            var actuales = GestorProveedor.ListarInsumosDeProveedor(proveedor.IdProveedor).Select(i => i.IdInsumo);
            var quitados = actuales.Except(proveedor.Insumos.Select(i => i.IdInsumo)).ToList();
            return GestorOrden.ListarAbiertasPorProveedor(proveedor.IdProveedor)
                .Where(o => o.Detalles.Any(d => quitados.Contains(d.IdInsumo))).ToList();
        }

        // CU026 Modificar Proveedor
        public void ModificarProveedor(BE.PROVEEDOR proveedor)
        {
            if (!GestorProveedor.ListarProveedores().Any(p => p.IdProveedor == proveedor.IdProveedor))
                throw new ArgumentException("msgProveedorInexistente");
            ValidarProveedor(proveedor);
            if (VerificarInsumosQuitados(proveedor).Count > 0)
                throw new ArgumentException("msgProveedorInsumoEnOrden");

            var acceso = new ACCESO();
            acceso.IniciarTransaccion();
            try
            {
                new MAPPER_PROVEEDOR(acceso).ModificarProveedor(proveedor);
                acceso.ConfirmarTransaccion();
            }
            catch (SqlException ex) when (ex.Number == ErrorDeNegocioSql)
            {
                acceso.DeshacerTransaccion();
                // FA2: otro proveedor ya tiene ese CUIT
                throw new ArgumentException("msgProveedorDuplicado", ex);
            }
            catch
            {
                acceso.DeshacerTransaccion();
                throw;
            }
            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Modificación de proveedor", $"Se modificó el proveedor: {proveedor.RazonSocial}");
        }

        // CU027 paso 2 / FA1: órdenes no cerradas del proveedor. Si la lista no está vacía, no se puede dar de baja.
        public List<BE.ORDEN_COMPRA> VerificarBajaProveedor(int idProveedor)
        {
            return GestorOrden.ListarAbiertasPorProveedor(idProveedor);
        }

        // CU027 Dar de Baja Proveedor: baja lógica, conserva el historial
        public void DarDeBajaProveedor(int idProveedor)
        {
            var proveedor = GestorProveedor.ListarProveedores().FirstOrDefault(p => p.IdProveedor == idProveedor);
            if (proveedor == null)
                throw new ArgumentException("msgProveedorInexistente");
            if (VerificarBajaProveedor(idProveedor).Count > 0)
                throw new ArgumentException("msgProveedorConOrdenes");
            GestorProveedor.BajaProveedor(idProveedor);
            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Baja de proveedor", $"Se dio de baja el proveedor: {proveedor.RazonSocial}");
        }
    }
}
