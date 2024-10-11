using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.ListaOrdenes_Datos
{
    public class D_ListaOrdenes
    {
        Conexion.Conexion cn = new Conexion.Conexion();

        public DataTable CargarOrdenes(string Fecha, string Status, string Orden)
        {
            SqlCommand cmd = new SqlCommand("SP_CPOS_BuscarOrdenesListaOrdenes", cn.LeerCadena());

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Fecha", Fecha);
            cmd.Parameters.AddWithValue("@Status", Status);
            cmd.Parameters.AddWithValue("@Orden", Orden);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return (dt);

        }
        public DataTable CargarOrdPorRango(string Fechadesde, string Fechahasta, string Status)
        { 
    
           SqlCommand cmd = new SqlCommand("SP_CPOS_BuscarOrdenesporRango", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Fechadesde", Fechadesde);
            cmd.Parameters.AddWithValue("@Fechahasta", Fechahasta);
            cmd.Parameters.AddWithValue("@Status", Status);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return (dt);

        }


    }
}
