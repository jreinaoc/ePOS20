using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.ListaFacturas_Datos
{
    public class D_ListaFactura
    {
        Conexion.Conexion cn = new Conexion.Conexion();

        public DataSet CargarFacturas(string Fecha_inicio= "", string Fecha_fin= "", int Inicio = 1, int Final = 12)
        {
            SqlCommand cmd = new SqlCommand("SP_CPOS_BuscarFacturasListFac", cn.LeerCadena());

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Fecha_inicio", Fecha_inicio);
            cmd.Parameters.AddWithValue("@Fecha_fin", Fecha_fin);
            cmd.Parameters.AddWithValue("@Inicio", Inicio);
            cmd.Parameters.AddWithValue("@Final", Final);

            DataSet dts = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dts);
            return (dts);

        }



    }
}
