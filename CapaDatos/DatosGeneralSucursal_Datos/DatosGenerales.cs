using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.DatosGeneralSucursal_Datos
{

    public class DatosGenerales
    {
        Conexion.Conexion cn = new Conexion.Conexion();

        public DataSet CargarDatosSucursalYCompania()
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = cn.LeerCadena();
                cmd.CommandType = CommandType.StoredProcedure;

                StringBuilder query = new StringBuilder("Datos_Sucursal_VariableGlobal");

                cmd.CommandText = query.ToString();

                DataSet dts = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dts);
                return dts;
            }
        }




    }
}
