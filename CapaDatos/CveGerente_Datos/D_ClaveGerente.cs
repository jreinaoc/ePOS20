using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.CveGerente_Datos
{
    public class D_ClaveGerente
    {
        Conexion.Conexion cn = new Conexion.Conexion();
        public string IDespecial;
        public DataTable GetGerenteTienda(string CodSucursal, string Id_Rol )
        {
            SqlCommand cmd = new SqlCommand("SP_CPOS_GET_GERENTE_TIENDA",cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodSucursal", CodSucursal);
            cmd.Parameters.AddWithValue("@Id_Rol", Id_Rol);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return (dt);

        }

        public string GetClaveEsp(string Gerente)
        {
            string a;

            SqlCommand cmd = new SqlCommand("SELECT Id_especial FROM TB_USUARIO WHERE @gerente = USER_NOMBRE + ' '+ USER_APELLIDO", cn.LeerCadena());
            cmd.Parameters.AddWithValue("gerente", Gerente);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 1)
            {

                a = dt.Rows[0][0].ToString();
                IDespecial = a.Replace(" ","");

            }

            return IDespecial;
        }
    }
}
