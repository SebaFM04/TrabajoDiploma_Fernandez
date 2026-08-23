using DAL;
using SERVICIO;
using SERVICIO.MultiIdioma_Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class IDIOMA_BLL
    {
        private readonly MAPPER_IDIOMA _mapper = new MAPPER_IDIOMA();

        public List<IDIOMA> ListarIdiomas()
        {
            return _mapper.ListarIdiomas();
        }

        public void CambiarIdioma(int idIdioma)
        {
            Dictionary<string, string> traducciones = _mapper.ObtenerTraducciones(idIdioma);
            GestorIdioma.Instancia.CambiarIdioma(idIdioma, traducciones);

            // Persistir preferencia del usuario logueado (si hay sesión)
            if (SessionManager.Instancia.IsLogged())
            {
                int idUsuario = SessionManager.Instancia.UsuarioActual.IdUsuario;
                _mapper.ActualizarIdiomaUsuario(idUsuario, idIdioma);
            }
        }

        public void InicializarIdiomaUsuario(int? idIdioma)
        {
            int id = (idIdioma.HasValue && idIdioma.Value > 0) ? idIdioma.Value : 1;
            Dictionary<string, string> traducciones = _mapper.ObtenerTraducciones(id);
            GestorIdioma.Instancia.CambiarIdioma(id, traducciones);
        }

        public List<TRADUCCION_DETALLE> ObtenerTraduccionesConDetalle(int idIdioma)
        {
            return _mapper.ObtenerTraduccionesConDetalle(idIdioma);
        }

        public void AgregarIdioma(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new Exception("El nombre del idioma no puede estar vacío.");
            }

            if (_mapper.ListarIdiomas().Any(i => i.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception($"El idioma '{nombre}' ya existe.");
            }
            _mapper.AgregarIdioma(nombre);
        }

        public void EditarNombreIdioma(int idIdioma, string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new Exception("El nombre del idioma no puede estar vacío.");
            }
            _mapper.ModificarNombreIdioma(idIdioma, nombre);
        }

        public bool ToggleDisponibilidad(int idIdioma)
        {
            return _mapper.ToggleDisponibilidad(idIdioma);
        }

        public void ModificarTraducciones(int idIdioma, List<TRADUCCION_DETALLE> traducciones)
        {
            foreach (var t in traducciones)
            {
                _mapper.ModificarTraduccion(t.IdControl, idIdioma, t.TextoTraducido);
            }

            // Si el idioma está activo, recargar el GestorIdioma
            if (GestorIdioma.Instancia.IdIdiomaActual == idIdioma)
            {
                CambiarIdioma(idIdioma);
            }
        }
    }
}
