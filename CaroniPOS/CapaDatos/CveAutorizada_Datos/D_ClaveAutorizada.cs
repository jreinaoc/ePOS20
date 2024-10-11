using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CapaDatos.CveAutorizada_Datos
{
    public class D_ClaveAutorizada
    {
        Conexion.Conexion cn = new Conexion.Conexion();
        private string resultado;
        List<LIST_TB_USUARIO> USUARIO = new List<LIST_TB_USUARIO>();



        public List<LIST_TB_USUARIO> ClaveAutorizada(string CodSucursal)
        {

            try
            {


                SqlCommand cmd = new SqlCommand("pGet_Usuarios_ClaveAutorizada", cn.LeerCadena());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodSucursa", CodSucursal);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        USUARIO.Add(new LIST_TB_USUARIO
                        {
                            COD_USR = Convert.ToString(dataReader["COD_USR"]),
                            NOMBRE = Convert.ToString(dataReader["NOMBRE"]),
                            //USER_APELLIDO = Convert.ToString(dataReader["USER_APELLIDO"]),
                            //USER_HASH = Convert.ToString(dataReader["USER_HASH"]),
                            USER_CARGO = Convert.ToString(dataReader["USER_CARGO"]),
                            COD_EMPLEADO = Convert.ToString(dataReader["COD_EMPLEADO"]),
                            Id_Rol = Convert.ToString(dataReader["Id_Rol"]),
                            USER_ST = Convert.ToString(dataReader["USER_ST"]),
                            Id_especial = Convert.ToString(dataReader["Id_especial"]),
                            Email = Convert.ToString(dataReader["Email"]),
                            COD_SUCURSAL = Convert.ToString(dataReader["COD_SUCURSAL"]),
                            //USER_Fec_Crea = Convert.ToDateTime(dataReader["USER_Fec_Crea"]),
                            //USER_Fec_Modif = dataReader["USER_Fec_Modif"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["USER_Fec_Modif"]),
                            //USER_Crea = Convert.ToString(dataReader["USER_Crea"]),
                            //USER_Modif = Convert.ToString(dataReader["USER_Modif"]),
                            //EVasignado = dataReader["EVasignado"] == DBNull.Value ? (Double?)0.00 : Convert.ToDouble(dataReader["EVasignado"]),
                            //Certificado = dataReader["Certificado"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dataReader["Certificado"]),
                            //Bloqueado = Convert.ToBoolean(dataReader["Bloqueado"]),
                            //Fecha_ActClave = dataReader["Fecha_ActClave"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["Fecha_ActClave"].ToString())

                        });


                    }
                    return USUARIO;

                }
                return null;
            }
            catch
            (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return null;
            }

        }


        public void BuscarUsuario(string gerente)
        {
            SqlCommand cmd = new SqlCommand("SELECT USER_HASH FROM TB_USUARIO WHERE @gerente = USER_NOMBRE + ' '+ USER_APELLIDO",cn.LeerCadena());
            cmd.Parameters.AddWithValue("gerente", gerente);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            
        }
    } 
}
