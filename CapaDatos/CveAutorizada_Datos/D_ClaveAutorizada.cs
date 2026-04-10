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

            var usuarios = new List<LIST_TB_USUARIO>(); // Lista local en lugar de usar una variable de clase

            try
            {
                using (SqlCommand cmd = new SqlCommand("pGet_Usuarios_ClaveAutorizada", cn.LeerCadena()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodSucursa", CodSucursal);

                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                usuarios.Add(new LIST_TB_USUARIO
                                {
                                    COD_USR = Convert.ToString(dataReader["COD_USR"]),
                                    NOMBRE = Convert.ToString(dataReader["NOMBRE"]),
                                    USER_CARGO = Convert.ToString(dataReader["USER_CARGO"]),
                                    COD_EMPLEADO = Convert.ToString(dataReader["COD_EMPLEADO"]),
                                    Id_Rol = Convert.ToString(dataReader["Id_Rol"]),
                                    USER_ST = Convert.ToString(dataReader["USER_ST"]),
                                    Id_especial = Convert.ToString(dataReader["Id_especial"]),
                                    Email = Convert.ToString(dataReader["Email"]),
                                    COD_SUCURSAL = Convert.ToString(dataReader["COD_SUCURSAL"])
                                });
                            }
                            return usuarios;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return null;
            }

        }

        public List<LIST_TB_USUARIO> ClaveAutorizadaII(string CodEmpleado)
        {
            var usuarios = new List<LIST_TB_USUARIO>(); // Lista local en lugar de usar una variable de clase

            try
            {
                using (SqlCommand cmd = new SqlCommand("pGet_Usuarios_ClaveAutorizada_II", cn.LeerCadena()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Parametro", CodEmpleado);

                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                usuarios.Add(new LIST_TB_USUARIO
                                {
                                    COD_USR = Convert.ToString(dataReader["COD_USR"]),
                                    NOMBRE = Convert.ToString(dataReader["NOMBRE"]),
                                    USER_CARGO = Convert.ToString(dataReader["USER_CARGO"]),
                                    COD_EMPLEADO = Convert.ToString(dataReader["COD_EMPLEADO"]),
                                    Id_Rol = Convert.ToString(dataReader["Id_Rol"]),
                                    USER_ST = Convert.ToString(dataReader["USER_ST"]),
                                    Id_especial = Convert.ToString(dataReader["Id_especial"]),
                                    Email = Convert.ToString(dataReader["Email"]),
                                    COD_SUCURSAL = Convert.ToString(dataReader["COD_SUCURSAL"])
                                });
                            }
                            return usuarios;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
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

        public string BuscarEmpleadosClave(string CodigoTipo, SqlCommand command = null)
        {
            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }

            SqlCommand cmd = new SqlCommand("SELECT CodigoEmpleado as Valor FROM  TIPOSCLAVEAUTORIZADA WHERE CodigoTipo = @CodigoTipo", cn.LeerCadena());
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("CodigoTipo", CodigoTipo);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt.Rows[0]["Valor"].ToString();

        }

        public string EmpleadosPorZona(string CodSucursal, SqlCommand command = null)
        {
            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }
            SqlCommand cmd = command;
            cmd.Parameters.Clear();

            cmd.CommandText = "pGet_EmpleadosPorZona";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodSucursa", CodSucursal.ToUpper());
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            cmd.Parameters.Clear();
            return dt.Rows[0]["CodigosEmpleado"].ToString();

        }

        public string BuscarTipoVentaClave(string CodigoTipo, SqlCommand command = null)
        {
            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }

            SqlCommand cmd = new SqlCommand("SELECT TipoCalculo as Valor FROM  TIPOSCLAVEAUTORIZADA WHERE CodigoTipo = @CodigoTipo", cn.LeerCadena());
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("CodigoTipo", CodigoTipo);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt.Rows[0]["Valor"].ToString();

        }

        public string BuscarNuevasClave(string TipoVenta, int IdEspecial, int Secuencia, SqlCommand command = null)
        {
            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }
            SqlCommand cmd = command;
            cmd.Parameters.Clear();

            cmd.CommandText = "SP_SECUENCIAEPOS_CALCULAR";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OPERACION", TipoVenta.ToUpper());
            cmd.Parameters.AddWithValue("@IdEspecialGerente", IdEspecial);
            cmd.Parameters.AddWithValue("@Clave", Secuencia);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            cmd.Parameters.Clear();
            return dt.Rows[0]["RESULTADO"].ToString();

        }

            public DataTable ObtengoGerentesClaveAutorizadaII(string CodEmpleado, SqlCommand command = null)
            {

            try
            {
                if (command == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command = connection.CreateCommand();
                }
                SqlCommand cmd = command;
                cmd.Parameters.Clear();

                cmd.CommandText = "pGet_Usuarios_ClaveAutorizada_II";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Parametro", CodEmpleado);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cmd.Parameters.Clear();
                return dt;

            }
            
            catch(Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return null;
            }

        }
        public DataTable ObtengoGerentesClaveAutorizadaIII(string CodEmpleado, SqlCommand command = null)
        {

            try
            {
                if (command == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command = connection.CreateCommand();
                }
                SqlCommand cmd = command;
                cmd.Parameters.Clear();

                cmd.CommandText = "pGet_Usuarios_ClaveAutorizada_III";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Parametro", CodEmpleado);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cmd.Parameters.Clear();
                return dt;

            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return null;
            }

        }
    } 
}
