using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class FacturaDTO
    {
            public string NumeroFactura { get; set; }
            public string NotaCredito { get; set; } // Solo para notas de crédito
            public string CedulaCliente { get; set; }
            public string NombreCliente { get; set; } 
            public decimal FactSub { get; set; }
            public decimal FactImpuesto { get; set; }
            public decimal FactIGTF { get; set; }
            public decimal FactTotal { get; set; }
            public string Fecha { get; set; }
        
    }
}
