using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class ReporteGlobal_PagosDia
    {
        //TB_ABONOS, TB_CAORDSER, TB_TIPOPAGO y TB_TIPOVENTA
        public string OrdenServicio { get; set; }
        public string TipoVenta { get; set; }
        public DateTime Fecha { get; set; }
        public string Cedula { get; set; }
        public string TipoPago { get; set; }
        public decimal  Pago { get; set; }


        public bool EsTotal { get; set; } = false;
        public string TextoTotal { get; set; }
        public decimal TotalOrden { get; set; }
        public decimal TotalPago { get; set; }

    }
}
