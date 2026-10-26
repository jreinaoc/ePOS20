using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    /// <summary>
    /// Convenio de TB_CONVENIOS junto con el cliente pagador asociado.
    ///
    /// CTE_NACIOPAGADOR (char(1)) y CTE_CEDULAPAGADOR (varchar(10)) son columnas
    /// nuevas: pueden venir vacias cuando el convenio todavia no tiene cliente
    /// pagador definido, en ese caso el convenio se considera sin pagador.
    /// </summary>
    public class TB_CONVENIO
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string CTE_NACIOPAGADOR { get; set; }
        public string CTE_CEDULAPAGADOR { get; set; }

        /// <summary>
        /// Codigo de promocion (TB_PROMOCIONES.COD_Prom) asociado al convenio.
        /// Puede ser nulo/vacio cuando el convenio no tiene promocion ligada.
        /// </summary>
        public string Cod_Prom { get; set; }
    }
}
