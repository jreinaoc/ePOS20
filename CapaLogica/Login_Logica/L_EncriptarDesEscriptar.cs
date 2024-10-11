using Microsoft.VisualBasic;
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

        public static string FunEncriptarNumCuenta(string Palabra)
        {
            try
            {

            int Largo;
            string N_Palabra= "";
            int I;
            Largo = Palabra.Trim().Length;
            for (I = 1; I <= Largo; I++)
            {
                if (I <= Largo - 4)
                    N_Palabra = N_Palabra + Char.ConvertFromUtf32((Ascc(Convert.ToChar(Palabra.Substring(I, 1))) + 5) * 2);
                else
                    N_Palabra = N_Palabra + Palabra.Substring(I, 0);
            }

            return N_Palabra;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            }


        public static string funEncriptarKM(string oDolar)
        {
            try
            {
             String Resultado = "";
            int Tamano;
            int Ni;
            Ni = 1;
            oDolar= oDolar.Replace(" ", "");
            Tamano = oDolar.Trim().Length;
            Tamano = Tamano - 4;
            while (Ni <= Tamano)
            {
                Resultado = Resultado + Strings.Chr((Strings.Asc(Strings.Mid(oDolar, Ni, 1)) + 5) * 2);
                Ni = Ni + 1;
                
            }

            Resultado = Resultado + oDolar.Substring(Tamano, 4);

            return Resultado;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        static int Ascc(char String)
        {
            int int32 = Convert.ToInt32(String);
            if (int32 < 128)
                return int32;
            try
            {
                Encoding fileIoEncoding = Encoding.Default;
                char[] chars = new char[1] { String };
                if (fileIoEncoding.IsSingleByte)
                {
                    byte[] bytes = new byte[1];
                    fileIoEncoding.GetBytes(chars, 0, 1, bytes, 0);
                    return (int)bytes[0];
                }
                byte[] bytes1 = new byte[2];
                if (fileIoEncoding.GetBytes(chars, 0, 1, bytes1, 0) == 1)
                    return (int)bytes1[0];
                if (BitConverter.IsLittleEndian)
                {
                    byte num = bytes1[0];
                    bytes1[0] = bytes1[1];
                    bytes1[1] = num;
                }
                return (int)BitConverter.ToInt16(bytes1, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
