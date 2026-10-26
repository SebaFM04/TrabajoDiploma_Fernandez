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
    }
}
