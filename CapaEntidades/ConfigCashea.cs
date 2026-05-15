using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public static class ConfigCashea
    {
        // Propiedades automáticas para almacenar los valores
        public static string ApiKey { get; set; }
        public static string BaseUrl { get; set; }
        public static string UuidCaja { get; set; }

        /// <summary>
        /// Decodifica una cadena en Base64 proveniente de SQL Server.
        /// </summary>
        public static string Decodificar(string valorCodificado)
        {
            try
            {
                if (string.IsNullOrEmpty(valorCodificado)) return "";
                byte[] data = Convert.FromBase64String(valorCodificado);
                return Encoding.UTF8.GetString(data);
            }
            catch
            {
                // Si por alguna razón el dato no es Base64, devolvemos el original
                return valorCodificado;
            }
        }
    }
}
