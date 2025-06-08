using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//mcll
namespace CapaEntidades
{
    public class TBF_EmailTelf
    {


        public string Ind_Mail { get; set; }  // Cambiado de Ind_Mail a Indice para generalizar
        public string CTE_Nacio { get; set; } // Cambiado de CTE_Nacio
        public string CTE_CedIden { get; set; }    // Cambiado de CTE_CedIden
     
        public string TipoTelefono { get; set; }
        public string TipoTLF_Descrip { get; set; }
    public string CodigoTelefono { get; set; }
        public string NumeroTelefono { get; set; }
        public string ExtensionTelefono { get; set; }
        public string Mail_Loogin { get; set; } // Cambiado a un nombre más claro
    }
}
