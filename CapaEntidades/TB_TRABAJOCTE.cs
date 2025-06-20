using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class TB_TRABAJOCTE
    {
        public string TSucursal { get; set; }
        public string TNumOrdserv { get; set; }
        public string TRevision { get; set; }
        public string TCEDIDEN { get; set; }
        public string TNACIO { get; set; }
        public string TTIPOTRABAJO { get; set; }
        public string TEXAMEN { get; set; }
        public string THORIZONTAL { get; set; }
        public string TVERTICAL { get; set; }
        public string TMAXIMA { get; set; }
        public string TPUENTE { get; set; }
        public decimal TALTD { get; set; }// en el grid del exabemn convnecional Dgv_Pnl2_conv
        public decimal TALTI { get; set; }// en el grid del exabemn convnecional Dgv_Pnl2_conv
        public string TOJO { get; set; }
        public string TTIPOVISIOND { get; set; }// en el grid del exabemn convnecional Dgv_Pnl2_conv
        public string TTIPOVISIONI { get; set; }// en el grid del exabemn convnecional Dgv_Pnl2_conv
        public string TLABORATORIO { get; set; }
        public string TSERVICIO { get; set; }
        public string THORAOFRECIDO { get; set; }
        public string TTIPORX { get; set; }
        public DateTime? TFECHAOFRECIDO { get; set; } // Usar DateTime? para permitir valores nulos
        public DateTime? TFECCREA { get; set; }       // Usar DateTime? para permitir valores nulos
        public DateTime? TFECMOD { get; set; }        // Usar DateTime? para permitir valores nulos
        public string USERCREA { get; set; }
        public string USERMOD { get; set; }
        public string CodDetVta { get; set; }          // Usar int? para permitir valores nulos
        public string TipoExamen { get; set; }
        public int? Correlativo { get; set; }        // Usar int? para permitir valores nulos
        public decimal? TDISTANCIAVERTICE { get; set; } // Cambiado a decimal?   Dgv_Pnl2_medconv
        public decimal? TANGULOPANTOSCOPICO { get; set; } // Cambiado a decimal? Dgv_Pnl2_medconv
        public decimal? TANGULOFACIAL { get; set; } // Cambiado a decimal?       Dgv_Pnl2_medconv
        public decimal? TDISTANCIADELECTURA { get; set; } // Cambiado a decimal? Dgv_Pnl2_medconv

        public string T_OJO { get; set; }
    }
}
