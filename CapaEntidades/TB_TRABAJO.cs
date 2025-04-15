using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class TB_TRABAJO
    {
        public string T_SUCURSAL { get; set; }
        public string T_NumOrdserv { get; set; }
        public string T_Revision { get; set; }
        public string T_CEDIDEN { get; set; }
        public string T_NACIO { get; set; }
        public string T_TIPOTRABAJO { get; set; }
        public int? T_EXAMEN { get; set; }
        public float? T_HORIZONTAL { get; set; }
        public float? T_VERTICAL { get; set; }
        public float? T_MAXIMA { get; set; }
        public float? T_PUENTE { get; set; }
        public float? T_ALTD { get; set; }
        public float? T_ALTI { get; set; }
        public string T_OJO { get; set; }
        public string T_TIPOVISIOND { get; set; }
        public string T_TIPOVISIONI { get; set; }
        public string T_LABORATORIO { get; set; }
        public string T_SERVICIO { get; set; }
        public string T_HORAOFRECIDO { get; set; }
        public string T_TIPORX { get; set; }
        public string T_FECHAOFRECIDO { get; set; }
        public DateTime T_FECCREA { get; set; }
        public DateTime? T_FECMOD { get; set; }
        public string USER_CREA { get; set; }
        public string USER_MOD { get; set; }
        public string Cod_DetVta { get; set; }
        public string TipoExamen { get; set; }
        public float? T_DISTANCIAVERTICE { get; set; }
        public float? T_ANGULOPANTOSCOPICO { get; set; }
        public float? T_ANGULOFACIAL { get; set; }
        public decimal Correlativo { get; set; }
        public float? T_DISTANCIADELECTURA { get; set; }
    }
}
