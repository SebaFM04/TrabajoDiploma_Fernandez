using System;
using System.Globalization;

namespace SERVICIO
{
    // Formato de importes en pesos argentinos (decisión 54): "$ 1.234,56". Solo presentación, sin lógica de negocio.
    public static class FormatoMoneda
    {
        public static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("es-AR");

        // Importe con símbolo: "$ 1.234,56"
        public static string Pesos(decimal importe)
        {
            return "$ " + importe.ToString("N2", Cultura);
        }

        // Importe editable, sin símbolo ni separador de miles: "1234,56"
        public static string Numero(decimal importe)
        {
            return importe.ToString("0.00", Cultura);
        }

        // Lee un importe escrito por el usuario ("1234,5", "1.234,50" o "$ 1.234,50")
        public static bool TryLeer(string texto, out decimal importe)
        {
            string limpio = (texto ?? string.Empty).Replace("$", string.Empty).Trim();
            return decimal.TryParse(limpio, NumberStyles.Number, Cultura, out importe);
        }
    }
}
