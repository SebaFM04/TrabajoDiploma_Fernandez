using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SERVICIO
{
    // Servicio genérico de serialización en XML (decisión 86). No tiene lógica de negocio: guarda y lee cualquier
    // objeto con constructor sin parámetros y propiedades públicas de lectura y escritura.
    public static class Serializador
    {
        public static void Serializar<T>(T objeto, string ruta)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ruta));
            var serializador = new XmlSerializer(typeof(T));
            using (var escritor = new StreamWriter(ruta, false, Encoding.UTF8))
            {
                serializador.Serialize(escritor, objeto);
            }
        }

        // Lanza InvalidOperationException si el archivo no es un XML de ese tipo
        public static T Deserializar<T>(string ruta)
        {
            var serializador = new XmlSerializer(typeof(T));
            using (var lector = new StreamReader(ruta, Encoding.UTF8))
            {
                return (T)serializador.Deserialize(lector);
            }
        }
    }
}
