using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
   public  class TB_CTEMAIL
    {
        public int Ind_Mail { get; set; }
        public string CTE_Nacio { get; set; }
        public string CTE_CedIden { get; set; }
        public string Mail_Loogin { get; set; }
        public string Mail_Dominio { get; set; }
        public string Mail_Ext { get; set; }
        public string Mail_Pref { get; set; }
        public DateTime? Mail_FecCrea { get; set; }
        public DateTime? Mail_FecModif { get; set; }
        public string USER_Crea { get; set; }
        public string USER_Modif { get; set; }
    }
}
