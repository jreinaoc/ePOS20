using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
   public class TB_CTETLF
    {

        public int Ind_Tlf { get; set; }
        public string CTE_Nacio { get; set; }
        public string CTE_CedIden { get; set; }
        public string TLF_Tipo { get; set; }
        public string TLF_Cod { get; set; }
        public string TLF_Numero { get; set; }
        public string TLF_Ext { get; set; }
        public DateTime? TLF_FecCrea { get; set; }
        public DateTime? TLF_FecModif { get; set; }
        public string USER_Crea { get; set; }
        public string USER_Modif { get; set; }
    }
}
