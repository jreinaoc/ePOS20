using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica.Login_Logica
{
    public static class L_EncriptarDesEscriptar
    {
        /// Esta clase contiene funciones para encriptar/desencriptar
        /// El ser estática no es necesario instanciar un objeto para usar las funciones Encriptar y DesEncriptar

            /// Encripta una cadena
            public static string Encriptar(this string _cadenaAencriptar)
            {
            string result = string.Empty;
            UnicodeEncoding encoding = new UnicodeEncoding();
            SHA1CryptoServiceProvider sha1= new SHA1CryptoServiceProvider();
            byte[] bySource = encoding.GetBytes(_cadenaAencriptar);
            byte[] encryted = sha1.ComputeHash(bySource);
            result = Convert.ToBase64String(encryted);
            return result;
        }

            /// Esta función desencripta la cadena que le envíamos en el parámentro de entrada.
            public static string DesEncriptar(this string _cadenaAdesencriptar)
            {
            string result = string.Empty;
            byte[] decryted =
            Convert.FromBase64String(_cadenaAdesencriptar);
            //result = 
            System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
            result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
        }


    }
}
