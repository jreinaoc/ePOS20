using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace CapaEntidades
{
    public class ReporteGlobal_NotasDia
    {
        //TB_NOTASCREDITODEBITO
        public string Numero { get; set; }
        public string NumeroControl { get; set; }
        public int Cedula { get; set; }
        public string Factura { get; set; }
        public decimal Monto { get; set; }
        public decimal Aplicado { get; set; }
        public decimal Saldo { get; set; }


        public bool EsTotal { get; set; } = false;
        public string TextoTotal { get; set; }
        public int TotalOrden { get; set; }
        public decimal TotalMonto { get; set; }
        public decimal TotalAplicado { get; set; }
        public decimal TotalSaldo { get; set; }

    }
}
