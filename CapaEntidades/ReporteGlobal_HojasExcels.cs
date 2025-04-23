using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class ReporteGlobal_HojasExcels
    {
        public string NombreHoja { get; set; }
        public IEnumerable<object> Datos { get; set; }
    }
}
