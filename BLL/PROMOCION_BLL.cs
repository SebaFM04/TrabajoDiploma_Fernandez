using BE;
using DAL;
using SERVICIO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    // N03: promociones (decisiones 74 a 82). CU033, CU034 y CU035 se suman en las fases 5 y 6.
    public class PROMOCION_BLL
    {
        MAPPER_PROMOCION GestorPromocion = new MAPPER_PROMOCION();

        // Estados de documento del diagrama de actividad (decisión 78, CK_PROMOCION_Estado)
        public const string EstadoEnProceso = "En proceso";
        public const string EstadoVigente = "Vigente";
        public const string EstadoFinalizada = "Finalizada";
        public const string EstadoRenovada = "Renovada";

        // Criterios del análisis de rentabilidad (decisión 75, CK_PROMOCION_Criterio)
        public const string CriterioVencimiento = "Vencimiento";
        public const string CriterioEstacionalidad = "Estacionalidad";
        public const string CriterioVentas = "Ventas";
        public const string CriterioRotacion = "Rotación";

        // Resultado de CU034 (decisión 81, CK_PROMOCION_Resultado)
        public const string ResultadoEfectiva = "Efectiva";
        public const string ResultadoNoEfectiva = "No efectiva";

        public string[] ListarEstados()
        {
            return new[] { EstadoEnProceso, EstadoVigente, EstadoFinalizada, EstadoRenovada };
        }

        public string[] ListarCriterios()
        {
            return new[] { CriterioVencimiento, CriterioEstacionalidad, CriterioVentas, CriterioRotacion };
        }

        // Decisión 78: En proceso → Vigente al empezar la vigencia; En proceso o Vigente → Finalizada al terminar.
        // Se llama al abrir el menú (tablero) y los formularios de promociones y de venta. Devuelve cuántas cambiaron;
        // solo entonces se registra en la bitácora.
        public int ActualizarEstados()
        {
            int cambios = GestorPromocion.ActualizarEstados(DateTime.Today);
            if (cambios > 0 && SessionManager.Instancia.UsuarioActual != null)
                new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Estados de promociones actualizados",
                    $"{cambios} promociones cambiaron de estado por fecha");
            return cambios;
        }

        // Promociones de un estado (vacío o null = todas), sin el detalle
        public List<BE.PROMOCION> ListarPromociones(string estado)
        {
            return GestorPromocion.ListarPromociones(estado);
        }

        // Promoción con sus productos y tamaños
        public BE.PROMOCION ObtenerPromocion(int idPromocion)
        {
            return GestorPromocion.BuscarPromocion(idPromocion);
        }
    }
}
