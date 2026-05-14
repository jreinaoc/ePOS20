using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class ReporteGlobal_CierreCaja
    {
        //tb_caja
        public int COD_Sucursal { get; set; }
        public decimal MANUALEFECTIVO { get; set; }
        public decimal SISTEMAEFECTIVO { get; set; }
        public decimal MANUALDEBITO { get; set; }
        public decimal SISTEMADEBITO { get; set; }
        public decimal MANUALCREDITO { get; set; }
        public decimal SISTEMACREDITO { get; set; }
        public decimal MANUALGASTOS { get; set; }
        public decimal SISTEMAGASTOS { get; set; }
        public decimal ManualIVARetenido { get; set; }
        public decimal SistemaIVARetenido { get; set; }
        public decimal ManualISLRRetenido { get; set; }
        public decimal SistemaISLRRetenido { get; set; }

        public decimal ManualImpMun { get; set; }
        public decimal SistemaImpMun { get; set; }

        public decimal MANUALTRANSFERENCIA { get; set; }
        public decimal SISTEMATRANSFERENCIA { get; set; }
        public decimal MANUALVUELTO { get; set; }
        public decimal SISTEMAVUELTO { get; set; }
        public decimal MANUALTOTALINGRESOS { get; set; }
        public decimal SISTEMATOTALINGRESOS { get; set; }
        public decimal MANUALREINTEGROS { get; set; }
        public decimal SISTEMAREINTEGRO { get; set; }
        public decimal SISTEMANOTACREDITO { get; set; }
        public decimal MANUALNOTACREDITO { get; set; }
        public decimal TOTAL_NETO { get; set; }
        public decimal IMPUESTO { get; set; }
        public decimal TOTAL_BRUTO { get; set; }

        public decimal DIFERENCIAEFECTIVO { get; set; }
        public decimal DIFERENCIADEBITO { get; set; }
        public decimal DIFERENCIACREDITO { get; set; }
        public decimal DIFERENCIAGASTOS { get; set; }
        public decimal DIFERENCIAIVA { get; set; }
        public decimal DIFERENCIAISLR { get; set; }
        public decimal DIFERENCIAImpMun { get; set; }
        public decimal DIFERENCIATRANSFERENCIA { get; set; }

    }
}
