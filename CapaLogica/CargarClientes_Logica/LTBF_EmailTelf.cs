using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos.CargarClientes_Datos; // Asegúrate de que esta referencia sea correcta
using CapaEntidades; // Asegúrate de que esta referencia sea correcta

namespace CapaLogica.CargarClientes_Logica
{

      public class LTBF_EmailTelf
    {
        private readonly D_TBF_EmailTelf_Datos _datos = new D_TBF_EmailTelf_Datos();
        public readonly StringBuilder stringBuilder = new StringBuilder();

        public List<TBF_EmailTelf> ObtenerContactos(string nacio,string cedula) // Modificado para aceptar el parámetro cedula
        {
            try
            {
                List<TBF_EmailTelf> contactos = _datos.ObtenerContactos(nacio,cedula); // Pasa la cédula a la capa de datos
                if (contactos == null)
                {
                    stringBuilder.AppendLine("No se pudieron obtener los contactos desde la capa de datos.");
                    return new List<TBF_EmailTelf>(); // Retorna una lista vacía en lugar de null
                }
                return contactos;
            }
            catch (Exception ex)
            {
                stringBuilder.AppendLine($"Error en la capa de lógica al obtener contactos: {ex.Message}");
                return new List<TBF_EmailTelf>();
            }
        }
    }
}


