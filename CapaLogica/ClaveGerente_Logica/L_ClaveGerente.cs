using CapaEntidades;
using CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using CapaDatos.CveGerente_Datos;

namespace CapaLogica.ClaveGerente_Logica
{
    public class L_ClaveGerente
    {
        D_ClaveGerente _D_ClaveGerente = new D_ClaveGerente();
        public string clave;
        public bool aprob;
        public DataTable TraerGerentes(string Sucursal, string IdRol)
        {
            DataTable dt = new DataTable();
            dt = _D_ClaveGerente.GetGerenteTienda(Sucursal, IdRol);
            return dt;

        }
        public string TraerClave(string Gerente)
        {
            clave = _D_ClaveGerente.GetClaveEsp(Gerente);
            return clave;

        }

        public bool ValidarClave(string CveIngresada)
        {
            if (CveIngresada == clave)
            {
                aprob = true;
            }
            else
            {
                aprob = false;
            }
            return aprob;
        }

    }
}
