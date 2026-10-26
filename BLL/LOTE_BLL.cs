using BE;
using DAL;
using SERVICIO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    // Lotes de insumos con vencimiento (decisiones 65 a 71)
    public class LOTE_BLL
    {
        // Valores de LOTE_INSUMO.Estado (CK_LOTE_Estado)
        public const string EstadoDisponible = "Disponible";
        public const string EstadoAgotado = "Agotado";
        public const string EstadoVencido = "Vencido";

        // Decisión 66: obligatoria si el insumo es perecedero; si se carga, tiene que ser posterior a hoy.
        // Las excepciones llevan la clave de idioma del mensaje.
        public void ValidarFechaVencimiento(BE.INSUMO insumo, DateTime? fechaVencimiento)
        {
            if (!fechaVencimiento.HasValue)
            {
                if (insumo.Perecedero)
                    throw new ArgumentException("msgLoteVencimientoObligatorio");
                return;
            }
            if (fechaVencimiento.Value.Date <= DateTime.Today)
                throw new ArgumentException("msgLoteVencimientoInvalido");
        }

        // CU019 y CU023: lote nuevo dentro de la transacción de la operación.
        // cantidad en la unidad de medida; costo por unidad de compra.
        public BE.LOTE_INSUMO CrearLote(BE.INSUMO insumo, int? idRecepcion, decimal cantidad, decimal costoUnidadCompra, DateTime? fechaVencimiento, ACCESO acceso)
        {
            var lote = new BE.LOTE_INSUMO
            {
                IdInsumo = insumo.IdInsumo,
                IdRecepcion = idRecepcion,
                FechaIngreso = DateTime.Now,
                FechaVencimiento = fechaVencimiento?.Date,
                CantidadInicial = cantidad,
                CantidadDisponible = cantidad,
                CostoUnidadCompra = costoUnidadCompra,
                Estado = EstadoDisponible,
                Insumo = insumo
            };
            new MAPPER_LOTE(acceso).InsertarLote(lote);
            return lote;
        }

        public List<BE.LOTE_INSUMO> ListarLotesInsumo(int idInsumo)
        {
            return new MAPPER_LOTE().ListarLotesInsumo(idInsumo);
        }

        // Días que el tablero considera "por vencer"
        public const int DiasPorVencerTablero = 15;

        // Tablero y criterio Vencimiento (N03): lotes de insumos activos que vencen entre hoy y hoy + dias
        public List<BE.LOTE_INSUMO> ListarLotesPorVencer(int dias)
        {
            if (dias < 0)
                throw new ArgumentException("msgLoteDiasInvalidos");
            return new MAPPER_LOTE().ListarLotesPorVencer(dias);
        }

        // CU036 paso 1: lotes vencidos que todavía tienen cantidad (sin dar de baja)
        public List<BE.LOTE_INSUMO> ListarLotesVencidos()
        {
            return new MAPPER_LOTE().ListarLotesVencidos();
        }

        // CU036 Dar de Baja Lote Vencido (decisión 69): en una transacción, el lote pasa a Vencido con su cantidad
        // como merma y el stock físico del insumo baja en esa cantidad (decisión 70). El stock vendible no cambia:
        // un lote vencido ya no contaba. FA1: el lote no está vencido o ya no tiene cantidad.
        public BE.LOTE_INSUMO DarDeBajaLoteVencido(int idLote)
        {
            var lote = new MAPPER_LOTE().BuscarLote(idLote);
            if (lote == null || lote.Estado != EstadoDisponible || lote.CantidadDisponible <= 0 || !lote.Vencido)
                throw new ArgumentException("msgLoteNoVencido");

            var acceso = new ACCESO();
            acceso.IniciarTransaccion();
            try
            {
                // El SP vuelve a controlar que siga vencido y con cantidad (otro usuario pudo darlo de baja antes)
                if (new MAPPER_LOTE(acceso).DarDeBajaLote(idLote, DateTime.Now) == 0)
                    throw new ArgumentException("msgLoteNoVencido");
                var mapperInsumo = new MAPPER_INSUMO(acceso);
                var insumo = mapperInsumo.BuscarInsumo(lote.IdInsumo);
                mapperInsumo.ActualizarVolumenPeso(insumo.IdInsumo, insumo.VolumenPesoDisponible - lote.CantidadDisponible);
                acceso.ConfirmarTransaccion();
            }
            catch
            {
                acceso.DeshacerTransaccion();
                throw;
            }

            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Baja de lote vencido",
                $"Lote {lote.IdLote} de {lote.Insumo.Nombre}: {lote.CantidadDisponible:0.###} {lote.Insumo.UnidadMedida}, venció el {lote.FechaVencimiento:dd/MM/yyyy}");
            lote.CantidadBaja = lote.CantidadDisponible;
            lote.CantidadDisponible = 0;
            lote.Estado = EstadoVencido;
            return lote;
        }
    }
}
