using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Login_Datos
{
    public class D_Login
    {
        Conexion.Conexion cn = new Conexion.Conexion();

        public void BuscarUsuario(string IdUsuario)
        {

            SqlCommand cmd = new SqlCommand("SELECT * from TB_USUARIO where COD_EMPLEADO= @usuario", cn.LeerCadena());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@usuario", IdUsuario);

            SqlDataReader dataReader = cmd.ExecuteReader();
            if (dataReader.Read())
            {

                TB_USUARIO.COD_USR = Convert.ToString(dataReader["COD_USR"]);
                TB_USUARIO.USER_NOMBRE = Convert.ToString(dataReader["USER_NOMBRE"]);
                TB_USUARIO.USER_APELLIDO = Convert.ToString(dataReader["USER_APELLIDO"]);
                TB_USUARIO.USER_HASH = Convert.ToString(dataReader["USER_HASH"]);
                TB_USUARIO.USER_CARGO = Convert.ToString(dataReader["USER_CARGO"]);
                TB_USUARIO.USER_ST = Convert.ToString(dataReader["USER_ST"]);
                TB_USUARIO.COD_EMPLEADO = Convert.ToString(dataReader["COD_EMPLEADO"]);
                TB_USUARIO.Email = Convert.ToString(dataReader["Email"]);
                TB_USUARIO.Id_Rol = Convert.ToString(dataReader["Id_Rol"]);
                TB_USUARIO.Id_especial = Convert.ToString(dataReader["Id_especial"]);
                TB_USUARIO.USER_Fec_Crea = Convert.ToDateTime(dataReader["USER_Fec_Crea"]);
                TB_USUARIO.USER_Fec_Modif = dataReader["USER_Fec_Modif"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["USER_Fec_Modif"]);
                TB_USUARIO.USER_Crea = Convert.ToString(dataReader["USER_Crea"]);
                TB_USUARIO.USER_Modif = Convert.ToString(dataReader["USER_Modif"]);
                TB_USUARIO.COD_SUCURSAL = Convert.ToString(dataReader["COD_SUCURSAL"]);
                TB_USUARIO.EVasignado = dataReader["EVasignado"] == DBNull.Value ? (Double?)0.00 : Convert.ToDouble(dataReader["EVasignado"]);
                TB_USUARIO.Certificado = dataReader["Certificado"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dataReader["Certificado"]);
                TB_USUARIO.Bloqueado = Convert.ToBoolean(dataReader["Bloqueado"]);
                TB_USUARIO.Fecha_ActClave = dataReader["Fecha_ActClave"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["Fecha_ActClave"].ToString());

            }

        }


        public string PaswordSistema()
        {
            SqlCommand cmd = new SqlCommand("SELECT HASH from TB_CLAVESISTEMAS where Mes= @MesActual", cn.LeerCadena());
            cmd.CommandType = CommandType.Text;

            CultureInfo cult = new CultureInfo("es-ES", false);
            string MesActual = DateTime.Today.ToString("MMMM");
            int numeromes = DateTime.ParseExact(MesActual, "MMMM", cult).Month;
            MesActual = Convert.ToString(numeromes);

            cmd.Parameters.AddWithValue("@MesActual", MesActual);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            string HASH= dt.Rows[0]["HASH"].ToString();
            return HASH;
            
        }
    }
}
