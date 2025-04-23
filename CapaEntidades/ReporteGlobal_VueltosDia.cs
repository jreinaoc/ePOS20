using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class ReporteGlobal_VueltosDia
    {
        //TB_CAMBIO y TB_CAORDSER
        public int NumOrden { get; set; }
        public int Referencia { get; set; }
        public string BancoEmisor { get; set; }
        public string BancoReceptor { get; set; }
        public decimal VueltoBS { get; set; }
        public decimal VueltoDivisa { get; set; }

    }
}
