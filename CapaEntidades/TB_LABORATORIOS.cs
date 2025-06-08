using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace CapaEntidades
    {
    //mcll
        public class TB_LABORATORIOS
        {
            public string CODIGO_LAB { get; set; }
            public string DESCRIPCION { get; set; }
            public string DIRECCION { get; set; }
            public string TELEFONO { get; set; }
            public string FAX { get; set; }
            public string CONTACTO { get; set; }
            public string ST_LABORATORIO { get; set; }
            public DateTime? FEC_CREA { get; set; } // Usamos Nullable<DateTime> o DateTime? porque puede ser nulo en la base de datos
            public DateTime? FEC_MOD { get; set; }  // Usamos Nullable<DateTime> o DateTime? porque puede ser nulo en la base de datos
            public string USER_CREA { get; set; }
            public string USER_MOD { get; set; }
        }
    }

