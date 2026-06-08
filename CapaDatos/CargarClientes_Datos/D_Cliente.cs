using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using CapaEntidades; // Asegúrate de que esta referencia sea correcta

namespace CapaDatos.CargarClientes_Datos
{
    public class D_Clientes
    {
        Conexion.Conexion cn = new Conexion.Conexion();
        public readonly StringBuilder stringBuilder = new StringBuilder();

        public void InsertarCliente(TB_CTEPPAL cliente)
        {
            using (SqlConnection connection = cn.LeerCadena())
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }


                using (SqlCommand command = new SqlCommand("SP_CPOSC_InsertarTB_CTEPPAL", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CTE_CedIden", cliente.CTE_CedIden);
                    command.Parameters.AddWithValue("@CTE_Nacio", cliente.CTE_Nacio);
                    command.Parameters.AddWithValue("@CTE_PNombre", cliente.CTE_PNombre);
                    command.Parameters.AddWithValue("@CTE_SNombre", cliente.CTE_SNombre ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CTE_PApellido", cliente.CTE_PApellido);
                    command.Parameters.AddWithValue("@CTE_SApellido", cliente.CTE_SApellido ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CTE_FNac", cliente.CTE_FNac);
                    command.Parameters.AddWithValue("@CTE_VIP", cliente.CTE_VIP);
                    command.Parameters.AddWithValue("@CTE_AfilVip", cliente.CTE_AfilVip ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CTE_FecAfil", cliente.CTE_FecAfil);
                    command.Parameters.AddWithValue("@COD_STCTE", cliente.COD_STCTE);
                    command.Parameters.AddWithValue("@CTE_Sex", cliente.CTE_Sex ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CTE_CodOcup", cliente.CTE_CodOcup ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CTE_EdoCiv", cliente.CTE_EdoCiv ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@COD_Sucursal", cliente.COD_Sucursal);
                    command.Parameters.AddWithValue("@CTE_FecCreacion", cliente.CTE_FecCreacion);
                    command.Parameters.AddWithValue("@CTE_FecMod", cliente.CTE_FecMod ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@USER_CREA", cliente.USER_CREA);
                    command.Parameters.AddWithValue("@USER_MOD", cliente.USER_MOD ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Direccion_fact", cliente.Direccion_fact ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Facebook", cliente.Facebook ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Twitter", cliente.Twitter ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Instagram", cliente.Instagram ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CTE_RETISLR", cliente.CTE_RETISLR);
                    command.Parameters.AddWithValue("@CTE_RETIVA", cliente.CTE_RETIVA);
                    command.Parameters.AddWithValue("@ImpuestoMunicipal", cliente.ImpuestoMunicipal);
                    //command.Parameters.AddWithValue("@CTE_RETIMUNICIPAL", cliente.CTE_RETIMUNICIPAL);
                    command.Parameters.AddWithValue("@COD_Edo", cliente.COD_Edo);
                    command.Parameters.AddWithValue("@COD_Ciud", cliente.COD_Ciud);
                    command.ExecuteNonQuery();
                }
            }
        }


        public void InsertarClienteP(TB_CTEPPAL cliente)
        {
            using (SqlConnection connection = cn.LeerCadena())
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }


                using (SqlCommand command = new SqlCommand("SP_CPOSC_InsertarTB_CTEPPALP", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CTE_CedIden", cliente.CTE_CedIden);
                    command.Parameters.AddWithValue("@CTE_Nacio", cliente.CTE_Nacio);
                    command.Parameters.AddWithValue("@CTE_PNombre", cliente.CTE_PNombre);
                    command.Parameters.AddWithValue("@CTE_SNombre", cliente.CTE_SNombre ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CTE_PApellido", cliente.CTE_PApellido);
                    command.Parameters.AddWithValue("@CTE_SApellido", cliente.CTE_SApellido ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CTE_FNac", cliente.CTE_FNac);
                    command.Parameters.AddWithValue("@CTE_VIP", cliente.CTE_VIP);
                    command.Parameters.AddWithValue("@CTE_AfilVip", cliente.CTE_AfilVip ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CTE_FecAfil", cliente.CTE_FecAfil);
                    command.Parameters.AddWithValue("@COD_STCTE", cliente.COD_STCTE);
                    command.Parameters.AddWithValue("@CTE_Sex", cliente.CTE_Sex ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CTE_CodOcup", cliente.CTE_CodOcup ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CTE_EdoCiv", cliente.CTE_EdoCiv ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@COD_Sucursal", cliente.COD_Sucursal);
                    command.Parameters.AddWithValue("@CTE_FecCreacion", cliente.CTE_FecCreacion);
                    command.Parameters.AddWithValue("@CTE_FecMod", cliente.CTE_FecMod ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@USER_CREA", cliente.USER_CREA);
                    command.Parameters.AddWithValue("@USER_MOD", cliente.USER_MOD ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Direccion_fact", cliente.Direccion_fact ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Facebook", cliente.Facebook ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Twitter", cliente.Twitter ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Instagram", cliente.Instagram ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CTE_RETISLR", cliente.CTE_RETISLR);
                    command.Parameters.AddWithValue("@CTE_RETIVA", cliente.CTE_RETIVA);
                    command.Parameters.AddWithValue("@ImpuestoMunicipal", cliente.ImpuestoMunicipal);
                    //command.Parameters.AddWithValue("@CTE_RETIMUNICIPAL", cliente.CTE_RETIMUNICIPAL);
                    command.Parameters.AddWithValue("@COD_Edo", cliente.COD_Edo);
                    command.Parameters.AddWithValue("@COD_Ciud", cliente.COD_Ciud);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<TB_CTEPPAL> ObtenerClientes(string filtro, bool buscarPorCedula, bool buscarPorNombre, SqlCommand externalCommand = null)
        {
            ////public List<TB_CTEPPAL> ObtenerClientes(SqlCommand externalCommand = null)
            ////{
            List<TB_CTEPPAL> listaClientes = new List<TB_CTEPPAL>();
            stringBuilder.Clear();
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            try
            {
                if (externalCommand == null)
                {
                    connection = cn.LeerCadena();
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    command = connection.CreateCommand();
                }
                else
                {
                    command = externalCommand;
                    if (command.Connection.State != ConnectionState.Open)
                    {
                        command.Connection.Open();
                    }
                }

                command.CommandText = "SP_CPOSC_GET_SOLO_CTEPPAL"; // para obtener todos los clientes de debe pasar siempre con un filtro
                command.CommandType = CommandType.StoredProcedure;


                // Añadir los parámetros al comando



                command.Parameters.AddWithValue("@Filtro", filtro ?? (object)DBNull.Value); // Pasa null si el filtro es null
                command.Parameters.AddWithValue("@BuscarPorCedula", buscarPorCedula);
                command.Parameters.AddWithValue("@BuscarPorNombre", buscarPorNombre);

                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TB_CTEPPAL cliente = new TB_CTEPPAL
                    {

                        CTE_Nacio = reader["CTE_Nacio"].ToString(),
                        CTE_CedIden = reader["CTE_CedIden"].ToString(),
                        CTE_id = reader["CTE_id"].ToString(),
                        CTE_PNombre = reader["CTE_PNombre"].ToString(),
                        ////CTE_SNombre = reader["CTE_SNombre"] != DBNull.Value ? reader["CTE_SNombre"].ToString() : null,
                        ////CTE_PApellido = reader["CTE_PApellido"].ToString(),
                        ////CTE_SApellido = reader["CTE_SApellido"] != DBNull.Value ? reader["CTE_SApellido"].ToString() : null,
                        //CTE_FNac = reader["CTE_FNac"] != DBNull.Value ? Convert.ToDateTime(reader["CTE_FNac"]) : DateTime.MinValue,
                        //CTE_VIP = reader["CTE_VIP"] != DBNull.Value && Convert.ToBoolean(reader["CTE_VIP"]),
                        //CTE_AfilVip = reader["CTE_AfilVip"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["CTE_AfilVip"]) : null,
                        //CTE_FecAfil = reader["CTE_FecAfil"] != DBNull.Value ? Convert.ToDateTime(reader["CTE_FecAfil"]) : DateTime.MinValue,
                        //COD_STCTE = reader["COD_STCTE"].ToString(),
                        //CTE_Sex = reader["CTE_Sex"].ToString(),
                        ////CTE_CodOcup = reader["CTE_CodOcup"] != DBNull.Value ? reader["CTE_CodOcup"].ToString() : null,
                        ////CTE_EdoCiv = reader["CTE_EdoCiv"] != DBNull.Value ? reader["CTE_EdoCiv"].ToString() : null,
                        //COD_Sucursal = reader["COD_Sucursal"].ToString(),
                        //CTE_FecCreacion = reader["CTE_FecCreacion"] != DBNull.Value ? Convert.ToDateTime(reader["CTE_FecCreacion"]) : DateTime.MinValue,
                        //CTE_FecMod = reader["CTE_FecMod"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["CTE_FecMod"]) : null,
                        //USER_CREA = reader["USER_CREA"].ToString(),
                        //USER_MOD = reader["USER_MOD"] != DBNull.Value ? reader["USER_MOD"].ToString() : null,
                        //////Direccion_fact = reader["Direccion_fact"] != DBNull.Value ? reader["Direccion_fact"].ToString() : null,
                        //////Facebook = reader["Facebook"] != DBNull.Value ? reader["Facebook"].ToString() : null,
                        //////Twitter = reader["Twitter"] != DBNull.Value ? reader["Twitter"].ToString() : null,
                        //////Instagram = reader["Instagram"] != DBNull.Value ? reader["Instagram"].ToString() : null,
                        //CTE_RETISLR = reader["CTE_RETISLR"] != DBNull.Value && Convert.ToBoolean(reader["CTE_RETISLR"]),
                        //CTE_RETIVA = reader["CTE_RETIVA"] != DBNull.Value && Convert.ToBoolean(reader["CTE_RETIVA"]),
                        //COD_Edo = reader["COD_Edo"] != DBNull.Value ? reader["COD_Edo"].ToString() : null,
                        //COD_Ciud = reader["COD_Ciud"] != DBNull.Value ? reader["COD_Ciud"].ToString() : null,
                        //EDO_Nombre = reader["EDO_Nombre"] != DBNull.Value ? reader["EDO_Nombre"].ToString() : null,
                        //CIUD_Nombre = reader["CIUD_Nombre"] != DBNull.Value ? reader["CIUD_Nombre"].ToString() : null,
                        //NumExamen = reader["NumExamen"] != DBNull.Value ? reader["NumExamen"].ToString() : null,
                        //TLF_Cod002 = reader["TLF_Cod002"] != DBNull.Value ? reader["TLF_Cod002"].ToString() : null,
                        //TLF_Numero002 = reader["TLF_Numero002"] != DBNull.Value ? reader["TLF_Numero002"].ToString() : null,
                        //TLF_Cod003 = reader["TLF_Cod003"] != DBNull.Value ? reader["TLF_Cod003"].ToString() : null,
                        //TLF_Numero003 = reader["TLF_Numero003"] != DBNull.Value ? reader["TLF_Numero003"].ToString() : null,
                        //Mail_Loogin = reader["Mail_Loogin"] != DBNull.Value ? reader["Mail_Loogin"].ToString() : null

                    };
                    listaClientes.Add(cliente);
                }
                return listaClientes;
            }
            catch (Exception ex)
            {
                stringBuilder.AppendLine(string.Format("Error al obtener clientes: {0}", ex.Message));
                return null;
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
                if (externalCommand == null && connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        // Método para obtener un cliente por su cédula (ejemplo)
        public DataTable ObtenerClientePorCedula(string cedula , string nacio)
        {
            DataTable dt = new DataTable();
            stringBuilder.Clear();
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataAdapter adapter = null;

            try
            {
                connection = cn.LeerCadena();

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command = new SqlCommand("SP_CPOSC_GET_TB_CTEPPAL", connection); // SP
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CTE_CedIden", cedula); // parámetro
                command.Parameters.AddWithValue("@CTE_Nacio", nacio); // parámetro
                adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                stringBuilder.AppendLine(string.Format("Error al obtener cliente por cédula: {0}", ex.Message));
                return null;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public List<TB_MAESEDO> ObtenerEstados()
        {
            List<TB_MAESEDO> listaEstados = new List<TB_MAESEDO>();
            stringBuilder.Clear();
            SqlConnection conexion = null;
            SqlCommand comando = null;
            SqlDataReader reader = null;

            try
            {
                conexion = cn.LeerCadena();
                if (conexion.State != ConnectionState.Open)
                {
                    conexion.Open();
                }
                string consulta = "SELECT COD_Edo, EDO_Nombre FROM TB_MAESEDO WHERE EDO_ST = 'A'";
                comando = new SqlCommand(consulta, conexion);
                reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    listaEstados.Add(new TB_MAESEDO
                    {
                        COD_Edo = reader["COD_Edo"].ToString(),
                        EDO_Nombre = reader["EDO_Nombre"].ToString()
                    });
                }
                return listaEstados;
            }
            catch (SqlException ex)
            {
                stringBuilder.AppendLine(string.Format("Error al obtener los estados desde la base de datos: {0}", ex.Message));
                return null; // O lanza una excepción específica
            }
            finally
            {
                if (reader != null && !reader.IsClosed) reader.Close();
                if (conexion != null && conexion.State == ConnectionState.Open) conexion.Close();
            }
        }

        public List<TB_MAESCIUD> ObtenerCiudadesPorEstado(string codigoEstado)
        {
            List<TB_MAESCIUD> listaCiudades = new List<TB_MAESCIUD>();
            stringBuilder.Clear();
            SqlConnection conexion = null;
            SqlCommand comando = null;
            SqlDataReader reader = null;

            try
            {
                conexion = cn.LeerCadena();
                if (conexion.State != ConnectionState.Open)
                {
                    conexion.Open();
                }
                string consulta = "SELECT COD_Ciud, COD_EDO, CIUD_Nombre FROM TB_MAESCIUD WHERE COD_EDO = @COD_EDO AND CIUD_ST = 'A'";
                comando = new SqlCommand(consulta, conexion);
                comando.Parameters.AddWithValue("@COD_EDO", codigoEstado);
                reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    listaCiudades.Add(new TB_MAESCIUD
                    {
                        COD_Ciud = reader["COD_Ciud"].ToString(),
                        COD_EDO = reader["COD_EDO"].ToString(),
                        CIUD_Nombre = reader["CIUD_Nombre"].ToString()
                    });
                }
                return listaCiudades;
            }
            catch (SqlException ex)
            {
                stringBuilder.AppendLine(string.Format("Error al obtener las ciudades desde la base de datos: {0}", ex.Message));
                return null; // O lanza una excepción específica
            }
            finally
            {
                if (reader != null && !reader.IsClosed) reader.Close();
                if (conexion != null && conexion.State == ConnectionState.Open) conexion.Close();
            }
        }

        // Métodos para TB_CTEMAIL
        public List<TB_CTEMAIL> ObtenerEmailsPorCedula(string cedula, string nacio)
        {
            List<TB_CTEMAIL> lista = new List<TB_CTEMAIL>();
            stringBuilder.Clear();
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader reader = null;


          
            try
            {
                connection = cn.LeerCadena();
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                command = new SqlCommand("SELECT * FROM TB_CTEMAIL WHERE CTE_CedIden = @Cedula and CTE_Nacio= @nacio", connection);
                command.Parameters.AddWithValue("@Cedula", cedula);
                command.Parameters.AddWithValue("@nacio", nacio);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new TB_CTEMAIL
                    {
                        Ind_Mail = Convert.ToInt32(reader["Ind_Mail"]),
                        CTE_Nacio = reader["CTE_Nacio"].ToString(),
                        CTE_CedIden = reader["CTE_CedIden"].ToString(),
                        Mail_Loogin = reader["Mail_Loogin"].ToString(),
                        Mail_Dominio = reader["Mail_Dominio"].ToString(),
                        Mail_Ext = reader["Mail_Ext"].ToString(),
                        Mail_Pref = reader["Mail_Pref"].ToString(),
                        Mail_FecCrea = reader["Mail_FecCrea"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["Mail_FecCrea"]),
                        Mail_FecModif = reader["Mail_FecModif"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["Mail_FecModif"]),
                        USER_Crea = reader["USER_Crea"].ToString(),
                        USER_Modif = reader["USER_Modif"].ToString()
                    });
                }
                return lista;
            }
            catch (Exception ex)
            {
                stringBuilder.AppendLine($"Error al obtener emails por cédula: {ex.Message}");
                return null;
            }
            finally
            {
                if (reader != null && !reader.IsClosed) reader.Close();
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
        }

        // Métodos para TB_CTETLF
        public List<TB_CTETLF> ObtenerTelefonosPorCedula(string cedula,  string nacio)
        {
            List<TB_CTETLF> lista = new List<TB_CTETLF>();
            stringBuilder.Clear();
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            try
            {
                connection = cn.LeerCadena();
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                command = new SqlCommand("SELECT * FROM TB_CTETLF WHERE CTE_CedIden = @Cedula", connection);
                command.Parameters.AddWithValue("@Cedula", cedula);
                command.Parameters.AddWithValue("@nacio", nacio);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new TB_CTETLF
                    {
                        Ind_Tlf = Convert.ToInt32(reader["Ind_Tlf"]),
                        CTE_Nacio = reader["CTE_Nacio"].ToString(),
                        CTE_CedIden = reader["CTE_CedIden"].ToString(),
                        TLF_Tipo = reader["TLF_Tipo"].ToString(),
                        TLF_Cod = reader["TLF_Cod"].ToString(),
                        TLF_Numero = reader["TLF_Numero"].ToString(),
                        TLF_Ext = reader["TLF_Ext"].ToString(),
                        TLF_FecCrea = reader["TLF_FecCrea"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["TLF_FecCrea"]),
                        TLF_FecModif = reader["TLF_FecModif"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["TLF_FecModif"]),
                        USER_Crea = reader["USER_Crea"].ToString(),
                        USER_Modif = reader["USER_Modif"].ToString()
                    });
                }
                return lista;
            }
            catch (Exception ex)
            {
                stringBuilder.AppendLine($"Error al obtener teléfonos por cédula: {ex.Message}");
                return null;
            }
            finally
            {
                if (reader != null && !reader.IsClosed) reader.Close();
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }
        }

        // Puedes agregar métodos para Insertar, Actualizar, Eliminar emails y teléfonos según necesites
        public void InsertarEmail(TB_CTEMAIL email)
        {
            using (SqlConnection connection = cn.LeerCadena())
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SP_CPOSC_Insertar_TB_CTEMAIL", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CTE_Nacio", email.CTE_Nacio);
                    command.Parameters.AddWithValue("@CTE_CedIden", email.CTE_CedIden);
                    command.Parameters.AddWithValue("@Mail_Loogin", email.Mail_Loogin);
                    command.Parameters.AddWithValue("@Mail_Dominio", email.Mail_Dominio);
                    command.Parameters.AddWithValue("@Mail_Ext", email.Mail_Ext);
                    command.Parameters.AddWithValue("@Mail_Pref", email.Mail_Pref);
                    command.Parameters.AddWithValue("@Mail_FecCrea", email.Mail_FecCrea.HasValue ? (object)email.Mail_FecCrea.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@USER_Crea", email.USER_Crea);
                    command.ExecuteNonQuery();
                }
            }
        }


      
        public void InsertarTelefono(TB_CTETLF telefono)
        {
            using (SqlConnection connection = cn.LeerCadena())
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (SqlCommand command = new SqlCommand("SP_CPOSC_Insertar_TB_CTETLF", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; // Importante: indicar que es un SP
                    command.Parameters.AddWithValue("@CTE_Nacio", telefono.CTE_Nacio);
                    command.Parameters.AddWithValue("@CTE_CedIden", telefono.CTE_CedIden);
                    command.Parameters.AddWithValue("@TLF_Tipo", telefono.TLF_Tipo);
                    command.Parameters.AddWithValue("@TLF_Cod", telefono.TLF_Cod);
                    command.Parameters.AddWithValue("@TLF_Numero", telefono.TLF_Numero);
                    command.Parameters.AddWithValue("@TLF_Ext", telefono.TLF_Ext ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@TLF_FecCrea", telefono.TLF_FecCrea.HasValue ? (object)telefono.TLF_FecCrea.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@USER_Crea", telefono.USER_Crea);

                    command.ExecuteNonQuery();
                }
            }
        }

            
     
        public DataTable ObtenerUsuariosOPTOMETRI()
        {
            DataTable dt = new DataTable();
            stringBuilder.Clear();
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataAdapter adapter = null;

            try
            {
                connection = cn.LeerCadena();
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                // Usamos una consulta directa a la función, no un procedimiento almacenado
                command = new SqlCommand("SELECT * FROM dbo.TBF_USUARIOOPTOMETRI()", connection);
                adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                stringBuilder.AppendLine($"Error al obtener usuarios: {ex.Message}");
                return null;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public DataTable ObtenerClienteConGarantia(string sucursal, string cedula, string nacio)
        {
            DataTable dt = new DataTable();
            stringBuilder.Clear();
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataAdapter adapter = null;

            try
            {
                connection = cn.LeerCadena();

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command = new SqlCommand("pGetClienteGarantia", connection); // SP
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SUC", sucursal); // parámetro
                command.Parameters.AddWithValue("@NACIO", nacio); // parámetro
                command.Parameters.AddWithValue("@CI", cedula); // parámetro
                adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                stringBuilder.AppendLine(string.Format("Error al obtener cliente por cédula: {0}", ex.Message));
                return null;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

      

        public DataTable ObtenerMotivosReposicion()
        {
            DataTable dt = new DataTable();
            stringBuilder.Clear();
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataAdapter adapter = null;

            try
            {
                connection = cn.LeerCadena();

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command = new SqlCommand("pGetMotivosReposicion", connection); // SP
                command.CommandType = CommandType.StoredProcedure;
              
                adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                stringBuilder.AppendLine(string.Format("Error al obtener cliente por cédula: {0}", ex.Message));
                return null;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public DataTable ObtenerExamenes(string cedula, string nacio)
        {
            stringBuilder.Clear();
            SqlConnection conexion = null;
            SqlCommand comando = null;
            SqlDataReader reader = null;

            try
            {
                conexion = cn.LeerCadena();
                if (conexion.State != ConnectionState.Open)
                {
                    conexion.Open();
                }
                string consulta = "SELECT  CTE_Nacio, CTE_CedIden, NUM_Examen FROM  TB_Examen WHERE CTE_CedIden = @cedula AND CTE_Nacio = @nacio ORDER BY NUM_Examen ";
                comando = new SqlCommand(consulta, conexion);
                comando.Parameters.AddWithValue("@cedula", cedula);
                comando.Parameters.AddWithValue("@nacio", nacio);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(comando);
                da.Fill(dt);
                return dt;
            }
            catch (SqlException ex)
            {
                stringBuilder.AppendLine(string.Format("Error al obtener examenes de cliente: {0}", ex.Message));
                return null; // O lanza una excepción específica
            }
            finally
            {
                if (reader != null && !reader.IsClosed) reader.Close();
                if (conexion != null && conexion.State == ConnectionState.Open) conexion.Close();
            }
        }

    }
}