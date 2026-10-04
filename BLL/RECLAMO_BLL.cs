using BE;
using DAL;
using SERVICIO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    // N02 CU020 Registrar Reclamo al Proveedor (DSS-CU020), «extend» desde CU019 FA3
    public class RECLAMO_BLL
    {
        // Motivos de RECLAMO_DETALLE (CHECK CK_RCD_Motivo)
        public const string MotivoFaltante = "Faltante";
        public const string MotivoDaniado = "Dañado";

        public string[] ListarMotivos()
        {
            return new[] { MotivoFaltante, MotivoDaniado };
        }

        // CU020 paso 1 (mensajes 1-9): insumos de la orden que quedaron con cantidad pendiente
        public List<BE.RECLAMO_DETALLE> ObtenerDiferencias(int idOrdenCompra)
        {
            var orden = new ORDEN_COMPRA_BLL().ObtenerDetalle(idOrdenCompra);
            return orden.Detalles.Where(d => d.CantidadPendiente > 0).Select(d => new BE.RECLAMO_DETALLE
            {
                IdInsumo = d.IdInsumo,
                CantidadPendiente = d.CantidadPendiente,
                Insumo = d.Insumo
            }).ToList();
        }

        // CU020 paso 3 / FA1 (mensajes 11-14): cada diferencia necesita su motivo
        public bool ValidarReclamo(BE.RECLAMO reclamo)
        {
            if (reclamo.Detalles == null || reclamo.Detalles.Count == 0)
                throw new ArgumentException("msgReclamoSinDiferencias");
            if (reclamo.Detalles.Any(d => Array.IndexOf(ListarMotivos(), d.Motivo) < 0))
                throw new ArgumentException("msgReclamoSinMotivo");
            if (reclamo.Detalles.Any(d => d.CantidadPendiente <= 0))
                throw new ArgumentException("msgReclamoCantidadInvalida");
            foreach (var d in reclamo.Detalles)
            {
                d.Descripcion = string.IsNullOrWhiteSpace(d.Descripcion) ? null : d.Descripcion.Trim();
                if (d.Descripcion != null && d.Descripcion.Length > 200)
                    throw new ArgumentException("msgReclamoDescripcionLarga");
            }
            return true;
        }

        // CU020 paso 3 (mensajes 15-21): reclamo "Pendiente" asociado a la recepción
        public void RegistrarReclamo(BE.RECLAMO reclamo)
        {
            ValidarReclamo(reclamo);
            if (reclamo.IdRecepcion <= 0)
                throw new ArgumentException("msgReclamoSinRecepcion");
            reclamo.FechaReclamo = DateTime.Now;
            var acceso = new ACCESO();
            acceso.IniciarTransaccion();
            try
            {
                new MAPPER_RECLAMO(acceso).GuardarReclamo(reclamo);
                acceso.ConfirmarTransaccion();
            }
            catch
            {
                acceso.DeshacerTransaccion();
                throw;
            }
            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Reclamo al proveedor",
                $"Reclamo {reclamo.IdReclamo} de la recepción {reclamo.IdRecepcion}: " + string.Join(", ", reclamo.Detalles.Select(d => $"{d.IdInsumo} x{d.CantidadPendiente} ({d.Motivo})")));
        }

        // CU019 mensajes 51-56 (decisión 60): al cerrarse la orden, sus reclamos pendientes se resuelven
        public void ResolverReclamos(int idOrdenCompra, ACCESO acceso)
        {
            new MAPPER_RECLAMO(acceso).ResolverPorOrden(idOrdenCompra);
        }

        // Reclamos de la orden (se muestran al recibir)
        public List<BE.RECLAMO> ListarPorOrden(int idOrdenCompra)
        {
            return new MAPPER_RECLAMO().ListarPorOrden(idOrdenCompra);
        }
    }
}
