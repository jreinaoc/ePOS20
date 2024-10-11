using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using log4net;
using log4net.Config;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Conexion
{
    public class Conexion
    {

        public SqlConnection LeerCadena()
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Epos"].ConnectionString);
            if (cn.State == ConnectionState.Open)
            {
                cn.Close();


            }

            else
            {
                cn.Open();
            }

            return cn;
        }


        public static SqlConnection LeerCadenaStatica()
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Epos"].ConnectionString);
            if (cn.State == ConnectionState.Open)
            {
                cn.Close();


            }

            else
            {
                cn.Open();
            }

            return cn;
        }


    }
}
