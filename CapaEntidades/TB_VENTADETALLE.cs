using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class TB_VENTADETALLE
    {
        public int Cod_DetVta { get; set; }
        public int COD_Venta { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public string TipoExamen { get; set; }

    }
}
