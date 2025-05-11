using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class ValidacionEstucheDTO
    {

            public string TipoTrabajo { get; set; }
            public string Observacion { get; set; }
            public bool Garantia { get; set; }
            public bool MonturaPropia { get; set; }
            public List<string> CodigosDesdeGrid { get; set; }
        
    }
}
