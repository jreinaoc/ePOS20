using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CapaEntidades
{
    public class TB_EMPAFI
    {
       // public string Rif { get; set; }
       // public string Nombre { get; set; }
  
        public string Codigo_Emp { get; set; }
        public string Nombre { get; set; }
        //public string Direccion { get; set; }
        //public string Telefono1 { get; set; }
        //public string Telefono2 { get; set; }
        //public DateTime? FechaAfiliacion { get; set; }
        public string PorcentajeDes1 { get; set; }
        public string PorcentajeDes2 { get; set; }
        public string PorcentajeDes3 { get; set; }
        //public DateTime? EMP_Fec_Crea { get; set; }
        //public DateTime? EMP_Fec_Mod { get; set; }
        //public string USER_Crea { get; set; }
        //public string USER_Modif { get; set; }
        //public bool? AceptaFinanciamiento { get; set; }
        //public DateTime? FechaInicio { get; set; }
        //public DateTime? FechaFin { get; set; }
        //public string Sucursal { get; set; }
        //public bool? Activo { get; set; }
    }
}
