using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class ReporteGlobal_Facturas
    {
        //TB_FACTURAS y TB_CAORDSER  
        public string Cod_Sucursal { get; set; }
        public string Fact_Num { get; set; }
        public string NumOrdServ { get; set; }
        public int CTE_CedIdenPAG { get; set; }
        public decimal Fact_SubTotal { get; set; }
        public decimal Fact_Descuento { get; set; }
        public decimal Fact_Impuesto { get; set; }
        public decimal Fact_IGTF { get; set; }
        public decimal Fact_Total { get; set; }
    }
}
