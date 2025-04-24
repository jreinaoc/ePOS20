using System;
using System.Collections.Generic;
using System.Globalization;
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


        public bool EsTotal { get; set; } = false;
        public decimal TotalOrden { get; set; }
        public decimal TotalSubtotal { get; set; }
        public decimal TotalImpuesto { get; set; }
        public decimal TotalIGTF { get; set; }
        public decimal Totaltotal { get; set; }
    }
}
