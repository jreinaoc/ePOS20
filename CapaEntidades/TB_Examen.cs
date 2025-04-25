using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class TB_Examen
    {
        public char CTE_Nacio { get; set; }
        public string CTE_CedIden { get; set; }
        public string COD_Sucursal { get; set; }
        public int NUM_Examen { get; set; }
        public DateTime? FEC_Examen { get; set; }
        public float? ESFD { get; set; }
        public float? ESFI { get; set; }
        public float? CILD { get; set; }
        public float? CILI { get; set; }
        public float? EJED { get; set; }
        public float? EJEI { get; set; }
        public float? ADDD { get; set; }
        public float? ADDI { get; set; }
        public string OBSERVACIONES { get; set; }
        public string TIPO_Optm { get; set; }
        public string NOM_Optm { get; set; }
        public DateTime EXA_Feccreacion { get; set; }
        public DateTime? EXA_Fecmod { get; set; }
        public string USER_CREA { get; set; }
        public string USER_MOD { get; set; }
        public string TIPOEXAMEN { get; set; }
        public string NOMBRE_CLINICA_OPTM { get; set; }
        public string TLF_TIPO { get; set; }
        public string TLF_COD { get; set; }
        public string TLF_NUMERO { get; set; }
        public string TLF_EXT { get; set; }
        public float? ESFD2 { get; set; }
        public float? ESFI2 { get; set; }
        public float? CILD2 { get; set; }
        public float? CILI2 { get; set; }
        public float? EJED2 { get; set; }
        public float? EJEI2 { get; set; }
        public string CodigoMimesys { get; set; }
    }
}
