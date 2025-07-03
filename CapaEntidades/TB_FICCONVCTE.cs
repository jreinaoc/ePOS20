using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class TB_FICCONVCTE
    {
        public string CTE_Nacio { get; set; }
        public string CTE_CedIden { get; set; }
        public string COD_Sucursal { get; set; }
        public int NUM_Examen { get; set; }
        public decimal? DPDL { get; set; }
        public decimal? DPDC { get; set; }
        public decimal? DPIL { get; set; }
        public decimal? DPIC { get; set; }
        public decimal? ALTD { get; set; }
        public decimal? ALTI { get; set; }
        public decimal? PRISMAD { get; set; }
        public decimal? PRISMAI { get; set; }
        public string PBASED { get; set; } // ¡Aquí está el cambio!
        public string PBASEI { get; set; } // ¡Aquí está el cambio!
        public string OFTI { get; set; }
        public string OFTD { get; set; }

        public decimal? AVD { get; set; } // Asegúrate de que esté así (o con el tipo correcto)
        public decimal? AVI { get; set; } // Asegúrate de que esté así (o con el tipo correcto)

        //public decimal? AVD { get; set; }
        //public decimal? AVI { get; set; }
        public string RETI { get; set; }
        public string RETD { get; set; }
        public decimal? PRISMAD2 { get; set; }
        public decimal? PRISMAI2 { get; set; }
        public string PBASED2 { get; set; }
        public string PBASEI2 { get; set; }
        public decimal? PROGVISIONLEJOSDISTD { get; set; }
        public decimal? PROGVISIONLEJOSDISTI { get; set; }
        public decimal? PROGVISIONCERCADISTD { get; set; }
        public decimal? PROGVISIONCERCADISTI { get; set; }
        public decimal? PROGVISIONMEDIADISTD { get; set; }
        public decimal? PROGVISIONMEDIADISTI { get; set; }
    }
}
