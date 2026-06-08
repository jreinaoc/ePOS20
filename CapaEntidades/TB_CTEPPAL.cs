using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class TB_CTEPPAL
    {
       
        public string CTE_Nacio { get; set; }
        public string CTE_CedIden { get; set; }
        public string CTE_id { get; set; }
        public string CTE_PNombre { get; set; }
        public string CTE_SNombre { get; set; }
        public string CTE_PApellido { get; set; }
        public string CTE_SApellido { get; set; }
        public DateTime CTE_FNac { get; set; }
        public bool CTE_VIP { get; set; }
        public DateTime? CTE_AfilVip { get; set; }
        public DateTime CTE_FecAfil { get; set; }
        public string COD_STCTE { get; set; }
        public string CTE_Sex { get; set; }
        public string CTE_CodOcup { get; set; }
        public string CTE_EdoCiv { get; set; }
        public string COD_Sucursal { get; set; }
        public DateTime CTE_FecCreacion { get; set; }
        public DateTime? CTE_FecMod { get; set; }
        public string USER_CREA { get; set; }
        public string USER_MOD { get; set; }
        public string Direccion_fact { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public bool CTE_RETISLR { get; set; }
        public bool CTE_RETIVA { get; set; }
 
        public string COD_Edo { get; set; }
        public string COD_Ciud { get; set; }
        public string EDO_Nombre { get; set; }
        public string CIUD_Nombre { get; set; }

        public string NumExamen { get; set; }


        public string TLF_Cod002 { get; set; }
        public string TLF_Numero002 { get; set; }
        public string TLF_Ext002 { get; set; }
        public string TLF_Cod003 { get; set; }
        public string TLF_Numero003 { get; set; }
        public string TLF_Ext03 { get; set; }
        public string Mail_Loogin { get; set; }

        public bool ImpuestoMunicipal { get; set; }
    }
}
