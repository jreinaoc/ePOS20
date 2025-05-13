using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaEntidades;

namespace CapaDatos.CargarOrdenes_Datos
{
    public class D_Articulos
    {
        Conexion.Conexion cn = new Conexion.Conexion();
        //El uso de la clase StringBuilder nos ayudara a devolver los mensajes 
        public readonly StringBuilder stringBuilder = new StringBuilder();

        public List <TB_ARTICULO> ObtenerArticulos(string TipoTrabajo, string CodArticulo = "", SqlCommand command = null)  // Trae el detalle del articulo 
        {
            // Declarar la lista para almacenar los resultados
            List<TB_ARTICULO> listaArticulos = new List<TB_ARTICULO>();
            stringBuilder.Clear();
            try
            {
                if (command == null)
                {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
                }

            SqlCommand cmd = command;
            cmd.CommandText = "SP_CPOS_GET_ARTICULO";
            cmd.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@CodArticulo", CodArticulo);
            command.Parameters.AddWithValue("@TipoTrabajo", TipoTrabajo);

                // Ejecutar el comando y leer los resultados
                using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Mapear cada fila a un objeto TB_ARTICULO
                    TB_ARTICULO articulo = new TB_ARTICULO
                    {
                        CodSucursal = reader["CodSucursal"].ToString(),
                        CodArticulo = reader["CodArticulo"].ToString(),
                        TIPART = reader["TIPART"].ToString(),
                        CODGRUPO = reader["CODGRUPO"].ToString(),
                        MARCA = reader["MARCA"].ToString(),
                        MODELO = reader["MODELO"].ToString(),
                        PROVEEDOR = reader["PROVEEDOR"].ToString(),
                        DESART = reader["DESART"].ToString(),
                        CODPRO = reader["CODPRO"].ToString(),
                        ART_EXIST = reader["ART_EXIST"] != DBNull.Value ? Convert.ToInt32(reader["ART_EXIST"]) : 0,
                        MANEJAEXISTENCIA = reader["MANEJAEXISTENCIA"] != DBNull.Value && Convert.ToBoolean(reader["MANEJAEXISTENCIA"]),
                        ART_CANTRESERVA = reader["ART_CANTRESERVA"] != DBNull.Value ? Convert.ToInt32(reader["ART_CANTRESERVA"]) : 0,
                        ART_PVP = reader["ART_PVP"] != DBNull.Value ? Convert.ToDecimal(reader["ART_PVP"]) : 0,
                        PORCTDESCUENTO = reader["PORCTDESCUENTO"] != DBNull.Value ? (float?)Convert.ToSingle(reader["PORCTDESCUENTO"]) : null,
                        ART_STOCKMIN = reader["ART_STOCKMIN"] != DBNull.Value ? Convert.ToDecimal(reader["ART_STOCKMIN"]) : 0,
                        ART_STOCKMAX = reader["ART_STOCKMAX"] != DBNull.Value ? Convert.ToInt16(reader["ART_STOCKMAX"]) : (short)0,
                        CODUBI = reader["CODUBI"].ToString(),
                        COSTOULTI = reader["COSTOULTI"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["COSTOULTI"]) : null,
                        COSTOPROME = reader["COSTOPROME"] != DBNull.Value ? Convert.ToDecimal(reader["COSTOPROME"]) : 0,
                        ART_ACTIVO = reader["ART_ACTIVO"] != DBNull.Value && Convert.ToBoolean(reader["ART_ACTIVO"]),
                        PROMO = reader["PROMO"].ToString(),
                        NOELIMINA = reader["NOELIMINA"] != DBNull.Value && Convert.ToBoolean(reader["NOELIMINA"]),
                        ART_EXENTO = reader["ART_EXENTO"] != DBNull.Value && Convert.ToBoolean(reader["ART_EXENTO"]),
                        Fec_Crea = reader["Fec_Crea"] != DBNull.Value ? Convert.ToDateTime(reader["Fec_Crea"]) : DateTime.MinValue,
                        Fec_Modif = reader["Fec_Modif"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["Fec_Modif"]) : null,
                        USER_CREA = reader["USER_CREA"].ToString(),
                        USER_MOD = reader["USER_MOD"].ToString(),
                        ServicioVisual = reader["ServicioVisual"] != DBNull.Value && Convert.ToBoolean(reader["ServicioVisual"]),
                        MHorizontal = reader["MHorizontal"] != DBNull.Value ? (int?)Convert.ToInt32(reader["MHorizontal"]) : null,
                        MVertical = reader["MVertical"] != DBNull.Value ? (int?)Convert.ToInt32(reader["MVertical"]) : null,
                        MMaxima = reader["MMaxima"] != DBNull.Value ? (int?)Convert.ToInt32(reader["MMaxima"]) : null,
                        MPuente = reader["MPuente"] != DBNull.Value ? (int?)Convert.ToInt32(reader["MPuente"]) : null,
                        CristalAlturaMin = reader["CristalAlturaMin"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["CristalAlturaMin"]) : null,
                        CristalAlturaMax = reader["CristalAlturaMax"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["CristalAlturaMax"]) : null,
                        CristalEsfMin = reader["CristalEsfMin"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["CristalEsfMin"]) : null,
                        CristalEsfMax = reader["CristalEsfMax"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["CristalEsfMax"]) : null,
                        CristalCilMin = reader["CristalCilMin"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["CristalCilMin"]) : null,
                        CristalCilMax = reader["CristalCilMax"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["CristalCilMax"]) : null,
                        CristalRangoMin = reader["CristalRangoMin"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["CristalRangoMin"]) : null,
                        CristalRangoMax = reader["CristalRangoMax"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["CristalRangoMax"]) : null,
                        CristalAdicionMin = reader["CristalAdicionMin"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["CristalAdicionMin"]) : null,
                        CristalAdicionMax = reader["CristalAdicionMax"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["CristalAdicionMax"]) : null,
                        CodRango = reader["CodRango"].ToString(),
                        CodGrupoRango = reader["CodGrupoRango"].ToString(),
                        DescripcionColor = reader["DescripcionColor"].ToString(),
                        tieneTraza = reader["tieneTraza"] != DBNull.Value && Convert.ToBoolean(reader["tieneTraza"]),
                        permiteTraza = reader["permiteTraza"] != DBNull.Value && Convert.ToBoolean(reader["permiteTraza"]),
                        tamano = reader["tamano"].ToString(),
                        codColor = reader["codColor"].ToString(),
                        PrecioEnDolares = reader["PrecioEnDolares"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["PrecioEnDolares"]) : null
                    };


                        // Agregar el objeto a la lista
                        listaArticulos.Add(articulo);
                }
            }

                return listaArticulos;
          }
          catch (Exception ex)
          {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
           }
        }

        public List<TB_TRABAJO> ObtenerTrabajo(string sucursal, string nacio, string cediden, SqlCommand command = null)  // Trae el detalle del articulo 
        {
            // Declarar la lista para almacenar los resultados
            List<TB_TRABAJO> T_Trabajo = new List<TB_TRABAJO>();
            stringBuilder.Clear();
            try
            {
                if (command == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command = connection.CreateCommand();
                }


                SqlCommand cmd = command;
                cmd.CommandText = "SP_CPOS_BucarTrabajo";
                cmd.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@T_SUCURSAL", sucursal);
                command.Parameters.AddWithValue("@T_NACIO", nacio);
                command.Parameters.AddWithValue("@T_CEDIDEN", cediden);

                // Ejecutar el comando y leer los resultados
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Mapear cada fila a un objeto TB_ARTICULO
                        TB_TRABAJO TRABAJO = new TB_TRABAJO
                        {
                            T_SUCURSAL = reader["T_SUCURSAL"].ToString(),
                            T_NumOrdserv = reader["T_NumOrdserv"]?.ToString(),
                            T_Revision = reader["T_Revision"].ToString(),
                            T_CEDIDEN = reader["T_CEDIDEN"].ToString(),
                            T_NACIO = reader["T_NACIO"].ToString(),
                            T_TIPOTRABAJO = reader["T_TIPOTRABAJO"]?.ToString(),
                            T_EXAMEN = reader["T_EXAMEN"] != DBNull.Value ? (int?)Convert.ToInt32(reader["T_EXAMEN"]) : null,
                            T_HORIZONTAL = reader["T_HORIZONTAL"] != DBNull.Value ? (float?)Convert.ToSingle(reader["T_HORIZONTAL"]) : null,
                            T_VERTICAL = reader["T_VERTICAL"] != DBNull.Value ? (float?)Convert.ToSingle(reader["T_VERTICAL"]) : null,
                            T_MAXIMA = reader["T_MAXIMA"] != DBNull.Value ? (float?)Convert.ToSingle(reader["T_MAXIMA"]) : null,
                            T_PUENTE = reader["T_PUENTE"] != DBNull.Value ? (float?)Convert.ToSingle(reader["T_PUENTE"]) : null,
                            T_ALTD = reader["T_ALTD"] != DBNull.Value ? (float?)Convert.ToSingle(reader["T_ALTD"]) : null,
                            T_ALTI = reader["T_ALTI"] != DBNull.Value ? (float?)Convert.ToSingle(reader["T_ALTI"]) : null,
                            T_OJO = reader["T_OJO"]?.ToString(),
                            T_TIPOVISIOND = reader["T_TIPOVISIOND"]?.ToString(),
                            T_TIPOVISIONI = reader["T_TIPOVISIONI"]?.ToString(),
                            T_LABORATORIO = reader["T_LABORATORIO"].ToString(),
                            T_SERVICIO = reader["T_SERVICIO"]?.ToString(),
                            T_HORAOFRECIDO = reader["T_HORAOFRECIDO"]?.ToString(),
                            T_TIPORX = reader["T_TIPORX"]?.ToString(),
                            T_FECHAOFRECIDO = reader["T_FECHAOFRECIDO"]?.ToString(),
                            T_FECCREA = Convert.ToDateTime(reader["T_FECCREA"]),
                            T_FECMOD = reader["T_FECMOD"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["T_FECMOD"]) : null,
                            USER_CREA = reader["USER_CREA"].ToString(),
                            USER_MOD = reader["USER_MOD"]?.ToString(),
                            Cod_DetVta = reader["Cod_DetVta"]?.ToString(),
                            TipoExamen = reader["TipoExamen"]?.ToString(),
                            T_DISTANCIAVERTICE = reader["T_DISTANCIAVERTICE"] != DBNull.Value ? (float?)Convert.ToSingle(reader["T_DISTANCIAVERTICE"]) : null,
                            T_ANGULOPANTOSCOPICO = reader["T_ANGULOPANTOSCOPICO"] != DBNull.Value ? (float?)Convert.ToSingle(reader["T_ANGULOPANTOSCOPICO"]) : null,
                            T_ANGULOFACIAL = reader["T_ANGULOFACIAL"] != DBNull.Value ? (float?)Convert.ToSingle(reader["T_ANGULOFACIAL"]) : null,
                            Correlativo = Convert.ToDecimal(reader["Correlativo"]),
                            T_DISTANCIADELECTURA = reader["T_DISTANCIADELECTURA"] != DBNull.Value ? (float?)Convert.ToSingle(reader["T_DISTANCIADELECTURA"]) : null
                        };


                        // Agregar el objeto a la lista
                        T_Trabajo.Add(TRABAJO);
                    }
                }

                return T_Trabajo;
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
            }
        }

        public DataTable BucarArticuloMaximoPorVenta(string Inicial_Articulo, SqlCommand command = null)
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

                cmd.CommandText = "SP_CPOS_ArticuloMaximo";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Código", Inicial_Articulo);


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

        public DataTable BucarIva(string Codigo_Iva, SqlCommand command = null)
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

                cmd.CommandText = "SP_CPOS_BuscarIva";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Codigo", Codigo_Iva);


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

        public DataSet ValidarExamenConPrisma(int numExamen, string cteNacio, string cteCedula, SqlCommand command = null)
        {

                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();

                try
                {
                    if (command == null)
                    {
                        SqlConnection connection = cn.LeerCadena();
                        command = connection.CreateCommand();
                    }
                    SqlCommand cmd = command;

                cmd.Parameters.Clear();
                cmd.CommandText = "pGetExamenconPrisma";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@numExamen", numExamen);
                cmd.Parameters.AddWithValue("@cteNacio", cteNacio);
                cmd.Parameters.AddWithValue("@cteCedula", cteCedula);

                // Llenar el DataSet con los resultados del procedimiento almacenado
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                // Cerrar la conexión
                cmd.Connection.Close();

                // Verificar si el DataSet tiene datos
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }
                else
                {
                    return null; // Retornar null si no hay datos
                }
            }
            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return null;
            }
        }

        public DataSet BucarServicioAgregado( SqlCommand command = null)
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

                cmd.CommandText = "SP_CPOS_ObtenerServiciosAgregados";
                cmd.CommandType = CommandType.StoredProcedure;

                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                cmd.Parameters.Clear();
                // Verificar si el DataSet tiene datos
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }
                else
                {
                    return null; // Retornar null si no hay datos
                }

            }
            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return null;
            }


        }

        public List<TB_FICCONV> ObtenerRx(string nacionalidad, string cedulaCliente, string sucursalActual, int numeroExamen, SqlCommand command = null)
        {
            stringBuilder.Clear();
            try
            {
                // Crear la lista para almacenar los resultados
                List<TB_FICCONV> listaFicConv = new List<TB_FICCONV>();

                // Configurar el comando SQL para ejecutar el procedimiento almacenado
                if (command == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command = connection.CreateCommand();
                }
                SqlCommand cmd = command;
                cmd.Parameters.Clear();

                cmd.CommandText = "SP_CPOS_ObtenerRx";
                cmd.CommandType = CommandType.StoredProcedure;

                // Agregar los parámetros al comando
                cmd.Parameters.AddWithValue("@Nacionalidad", nacionalidad);
                cmd.Parameters.AddWithValue("@CedulaCliente", cedulaCliente);
                cmd.Parameters.AddWithValue("@SucursalActual", sucursalActual);
                cmd.Parameters.AddWithValue("@NumeroExamen", numeroExamen);

                    // Ejecutar el comando y leer los resultados
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Crear un nuevo objeto TB_FICCONV y llenarlo con los datos del lector
                            TB_FICCONV ficConv = new TB_FICCONV
                            {
                                CTE_Nacio = reader["CTE_Nacio"].ToString()[0],
                                CTE_CedIden = reader["CTE_CedIden"].ToString(),
                                COD_Sucursal = reader["COD_Sucursal"].ToString(),
                                NUM_Examen = Convert.ToInt32(reader["NUM_Examen"]),
                                DPDL = reader["DPDL"] as float?,
                                DPDC = reader["DPDC"] as float?,
                                DPIL = reader["DPIL"] as float?,
                                DPIC = reader["DPIC"] as float?,
                                ALTD = reader["ALTD"] as float?,
                                ALTI = reader["ALTI"] as float?,
                                PRISMAD = reader["PRISMAD"] as float?,
                                PRISMAI = reader["PRISMAI"] as float?,
                                PBASED = reader["PBASED"] as string,
                                PBASEI = reader["PBASEI"] as string,
                                OFTI = reader["OFTI"] as string,
                                OFTD = reader["OFTD"] as string,
                                AVD = reader["AVD"] as float?,
                                AVI = reader["AVI"] as float?,
                                RETI = reader["RETI"] as string,
                                RETD = reader["RETD"] as string,
                                PRISMAD2 = reader["PRISMAD2"] as float?,
                                PRISMAI2 = reader["PRISMAI2"] as float?,
                                PBASED2 = reader["PBASED2"] as string,
                                PBASEI2 = reader["PBASEI2"] as string,
                                PROGVISIONLEJOSDISTD = reader["PROGVISIONLEJOSDISTD"] as float?,
                                PROGVISIONLEJOSDISTI = reader["PROGVISIONLEJOSDISTI"] as float?,
                                PROGVISIONCERCADISTD = reader["PROGVISIONCERCADISTD"] as float?,
                                PROGVISIONCERCADISTI = reader["PROGVISIONCERCADISTI"] as float?,
                                PROGVISIONMEDIADISTD = reader["PROGVISIONMEDIADISTD"] as float?,
                                PROGVISIONMEDIADISTI = reader["PROGVISIONMEDIADISTI"] as float?
                            };

                            // Agregar el objeto a la lista
                            listaFicConv.Add(ficConv);
                        }

                    }
                // Retornar la lista con los resultados
                return listaFicConv;
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
            }
        }

        public List<TB_Examen> ObtenerExamen(string nacionalidad, string cedulaCliente, string sucursalActual, int numeroExamen, SqlCommand command = null)
        {
            stringBuilder.Clear();
            try
            {
                // Crear una instancia de TB_Examen para almacenar los resultados
                // Crear la lista para almacenar los resultados
                List<TB_Examen> _TB_Examen= new List<TB_Examen>();
                    
                    // Configurar el comando SQL para ejecutar el procedimiento almacenado
                    if (command == null)
                    {
                        SqlConnection connection = cn.LeerCadena();
                        command = connection.CreateCommand();
                    }
                    SqlCommand cmd = command;
                    cmd.Parameters.Clear();

                    cmd.CommandText = "SP_CPOS_ObtenerExamen";
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar los parámetros al comando
                    cmd.Parameters.AddWithValue("@Nacionalidad", nacionalidad);
                    cmd.Parameters.AddWithValue("@CedulaCliente", cedulaCliente);
                    cmd.Parameters.AddWithValue("@SucursalActual", sucursalActual);
                    cmd.Parameters.AddWithValue("@NumeroExamen", numeroExamen);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                        // Crear un nuevo objeto TB_Examen y llenarlo con los datos del lector
                        TB_Examen  examen = new TB_Examen
                               {
                            CTE_Nacio = reader["CTE_Nacio"].ToString()[0],
                            CTE_CedIden = reader["CTE_CedIden"].ToString(),
                            COD_Sucursal = reader["COD_Sucursal"].ToString(),
                            NUM_Examen = reader["NUM_Examen"] != DBNull.Value ? Convert.ToInt32(reader["NUM_Examen"]) : 0,
                            FEC_Examen = reader["FEC_Examen"] != DBNull.Value ? (DateTime?)reader["FEC_Examen"] : null,
                            ESFD = reader["ESFD"] != DBNull.Value ? (float?)Convert.ToSingle(reader["ESFD"]) : null,
                            ESFI = reader["ESFI"] != DBNull.Value ? (float?)Convert.ToSingle(reader["ESFI"]) : null,
                            CILD = reader["CILD"] != DBNull.Value ? (float?)Convert.ToSingle(reader["CILD"]) : null,
                            CILI = reader["CILI"] != DBNull.Value ? (float?)Convert.ToSingle(reader["CILI"]) : null,
                            EJED = reader["EJED"] != DBNull.Value ? (float?)Convert.ToSingle(reader["EJED"]) : null,
                            EJEI = reader["EJEI"] != DBNull.Value ? (float?)Convert.ToSingle(reader["EJEI"]) : null,
                            ADDD = reader["ADDD"] != DBNull.Value ? (float?)Convert.ToSingle(reader["ADDD"]) : null,
                            ADDI = reader["ADDI"] != DBNull.Value ? (float?)Convert.ToSingle(reader["ADDI"]) : null,
                            OBSERVACIONES = reader["OBSERVACIONES"] as string,
                            TIPO_Optm = reader["TIPO_Optm"] as string,
                            NOM_Optm = reader["NOM_Optm"] as string,
                            EXA_Feccreacion = Convert.ToDateTime(reader["EXA_Feccreacion"]),
                            EXA_Fecmod = reader["EXA_Fecmod"] as DateTime?,
                            USER_CREA = reader["USER_CREA"].ToString(),
                            USER_MOD = reader["USER_MOD"] as string,
                            TIPOEXAMEN = reader["TIPOEXAMEN"] as string,
                            NOMBRE_CLINICA_OPTM = reader["NOMBRE_CLINICA_OPTM"] as string,
                            TLF_TIPO = reader["TLF_TIPO"] as string,
                            TLF_COD = reader["TLF_COD"] as string,
                            TLF_NUMERO = reader["TLF_NUMERO"] as string,
                            TLF_EXT = reader["TLF_EXT"] as string,
                            ESFD2 = reader["ESFD2"] != DBNull.Value ? (float?)Convert.ToSingle(reader["ESFD2"]) : null,
                            ESFI2 = reader["ESFI2"] != DBNull.Value ? (float?)Convert.ToSingle(reader["ESFI2"]) : null,
                            CILD2 = reader["CILD2"] != DBNull.Value ? (float?)Convert.ToSingle(reader["CILD2"]) : null,
                            CILI2 = reader["CILI2"] != DBNull.Value ? (float?)Convert.ToSingle(reader["CILI2"]) : null,
                            EJED2 = reader["EJED2"] != DBNull.Value ? (float?)Convert.ToSingle(reader["EJED2"]) : null,
                            EJEI2 = reader["EJEI2"] != DBNull.Value ? (float?)Convert.ToSingle(reader["EJEI2"]) : null,
                            CodigoMimesys = reader["CodigoMimesys"] as string
                        };

                        // Agregar el objeto a la lista
                        _TB_Examen.Add(examen);

                        }
                    }

                // Retornar la lista con los resultados
                return _TB_Examen; 
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
            }
        }
        public DataTable BucarTipoVenta(string Cod_DetVta= "", SqlCommand command = null)
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

                cmd.CommandText = "SP_CPOS_TipoVenta";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Cod_DetVta", Cod_DetVta);
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
        public DataSet BucarColoracion(string Cristal, bool TipoColor, SqlCommand command = null)
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

                cmd.CommandText = "pServicioColoracion";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CRISTAL", Cristal);
                cmd.Parameters.AddWithValue("@TIPOCOLOR", TipoColor);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                cmd.Parameters.Clear();
                // Verificar si el DataSet tiene datos
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }
                else
                {
                    return null; // Retornar null si no hay datos
                }

            }
            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return null;
            }


        }

        public DataSet PermisosDescuento(string CodigoArticulo, string PorceDesc, string IdRol, SqlCommand command = null)
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

                cmd.CommandText = "pGetPermisosDescuentos";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodArticulo", CodigoArticulo);
                cmd.Parameters.AddWithValue("@PorcDcto", PorceDesc);
                cmd.Parameters.AddWithValue("@RolId", IdRol);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                cmd.Parameters.Clear();
                // Verificar si el DataSet tiene datos
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }
                else
                {
                    return null; // Retornar null si no hay datos
                }

            }
            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return null;
            }


        }

        public DataTable MOTIVOSDESCUENTO(string cbCodMotivo= ""  , SqlCommand command = null)
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

                cmd.CommandText = "SP_CPOS_MOTIVOSDESCUENTO";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cbCodMotivo", cbCodMotivo);
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

        //Botón PROCESAR:
        public DataTable ServicioColoracion_btnProcesar(string cristal, bool tipoColor, SqlCommand command = null)
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

                cmd.CommandText = "CPOS_pServicioColoracion";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CRISTAL", cristal);
                cmd.Parameters.AddWithValue("@TIPOCOLOR", tipoColor);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cmd.Parameters.Clear();
                return dt;
            }
            catch (Exception ex)
            {
                string error = $"Error: {ex.Message}";
                return null;
            }

        }

        public DataSet ServiciosAR_btnProcesar(string Codcristal, bool CodServicio, SqlCommand command = null)
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

                cmd.CommandText = "CPOS_pGetServiciosAR";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodCrt", Codcristal);
                cmd.Parameters.AddWithValue("@CodServ", CodServicio);

                DataSet dt = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cmd.Parameters.Clear();
                return dt;
            }
            catch (Exception ex)
            {
                string error = $"Error: {ex.Message}";
                return null;
            }

        }

        public DataTable ValidoTraza_btnProcesar(string articulo, SqlCommand command = null)
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

                cmd.CommandText = "CPOS_pValidoTraza";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@articulo", articulo);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cmd.Parameters.Clear();
                return dt;
            }
            catch (Exception ex)
            {
                string error = $"Error: {ex.Message}";
                return null;
            }

        }

        public DataTable VerificarCantidadMonturaAlmacenQUORUM(string NumordServ, string CodArticulo, int CodSucursal, string CodServicio)
        {

            try
            {
                SqlConnection connection = cn.LeerCadena();
                SqlCommand command = connection.CreateCommand();
                command.Parameters.Clear();
                command.CommandText = "SP_CPOS_pGetMonturaAlmacenQUORUM";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@NumordServ", NumordServ);
                command.Parameters.AddWithValue("@CodArticulo", CodArticulo);
                command.Parameters.AddWithValue("@CodSucursal", CodSucursal);
                command.Parameters.AddWithValue("@CodServicio", CodServicio);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dt);
                command.Parameters.Clear();
                return dt;
            }
            catch (Exception ex)
            {
                string error = $"Error: {ex.Message}";
                return null;
            }
        }







        ////////////////////////////////////////


        public DataSet GetDiametroEfectivo_btnProcesar(string nac, string cedula, int numExamen, string cristal, string cristalI, string ojo,
                                                       string tipoVisionD, string tipoVisionI, string montura, int horizontal, int maxima, 
                                                       int puente, string sucursal, SqlCommand command = null)
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

                cmd.CommandText = "CPOS_pGetDiametroEfectivo";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NACCTE", nac);
                cmd.Parameters.AddWithValue("@CEDULACTE", cedula);
                cmd.Parameters.AddWithValue("@NUMEXAMEN", numExamen);
                cmd.Parameters.AddWithValue("@CRISTAL", cristal);
                cmd.Parameters.AddWithValue("@CRISTALI", cristalI);
                cmd.Parameters.AddWithValue("@OJO", ojo);
                cmd.Parameters.AddWithValue("@TIPOVISIOND", tipoVisionD);
                cmd.Parameters.AddWithValue("@TIPOVISIONI", tipoVisionI);
                cmd.Parameters.AddWithValue("@MONTURA", montura);
                cmd.Parameters.AddWithValue("@HORIZONTAL", horizontal);
                cmd.Parameters.AddWithValue("@MAXIMA", maxima);
                cmd.Parameters.AddWithValue("@PUENTE", puente);
                cmd.Parameters.AddWithValue("@SUC", sucursal);

                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                cmd.Parameters.Clear();
                return ds;
            }
            catch (Exception ex)
            {
                string error = $"Error: {ex.Message}";
                return null;
            }
        }

        public DataTable ValidarRangoCristal_btnProcesar(string nac, string ci, string examen, string ojo, string cristalD, string cristalI,
                                                       decimal alturaD, decimal alturaI, string visionD, string visionI, decimal diametroD, 
                                                       decimal diametroI, SqlCommand command = null)
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

                cmd.CommandText = "CPOS_pValidarangoCristal";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nacio", nac);
                cmd.Parameters.AddWithValue("@CI", ci);
                cmd.Parameters.AddWithValue("@Examen", examen);
                cmd.Parameters.AddWithValue("@ojo", ojo);
                cmd.Parameters.AddWithValue("@CristalD", cristalD);
                cmd.Parameters.AddWithValue("@CristalI", cristalI);
                cmd.Parameters.AddWithValue("@AlturaD", alturaD);
                cmd.Parameters.AddWithValue("@AlturaI", alturaI);
                cmd.Parameters.AddWithValue("@VisionD", visionD);
                cmd.Parameters.AddWithValue("@VisionI", visionI);
                cmd.Parameters.AddWithValue("@diametroD", diametroD);
                cmd.Parameters.AddWithValue("@diametroI", diametroI);

                DataTable ds = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                cmd.Parameters.Clear();
                return ds;
            }
            catch (Exception ex)
            {
                string error = $"Error: {ex.Message}";
                return null;
            }
        }

        public DataTable GetRangoCristal_btnProcesar(string cristalD, string cristalI, SqlCommand command = null)
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

                cmd.CommandText = "CPOS_pGetRangoCristal";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CristalD", cristalD);
                cmd.Parameters.AddWithValue("@CristalI", cristalI);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cmd.Parameters.Clear();
                return dt;
            }
            catch (Exception ex)
            {
                string error = $"Error: {ex.Message}";
                return null;
            }
        }

        public DataTable MaxVta_btnProcesar(string Inicial_Producto = "" , SqlCommand command = null)
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

                cmd.CommandText = "CPOS_Max_Vta";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Cod_Producto", Inicial_Producto);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cmd.Parameters.Clear();
                return dt;
            }
            catch (Exception ex)
            {
                string error = $"Error: {ex.Message}";
                return null;
            }
        }





        public DataSet MostrarRangosCrtGrid(string nacRif, string cedula, string numExamen, string codArticulo, string ojo, string tipoVision, decimal alt, decimal diam, decimal disVert, decimal angFac, decimal angPant, string Color, string glbServicio, string lab, string medDisV, string medAngF, string medAngP, string DDL, SqlCommand command = null)
        {

            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }
            SqlCommand cmd = command;
            cmd.Parameters.Clear();
            cmd.CommandText = "pValidoParametrosCRT";
            //SqlCommand cmd = new SqlCommand("SP_DETALLEFACTURAFISCAL", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NACIO", nacRif);
            cmd.Parameters.AddWithValue("@CI", cedula);
            cmd.Parameters.AddWithValue("@EXAM", numExamen);
            cmd.Parameters.AddWithValue("@CRTDERECHO", codArticulo);
            cmd.Parameters.AddWithValue("@OJO", ojo);
            cmd.Parameters.AddWithValue("@VISION", tipoVision);
            cmd.Parameters.AddWithValue("@ALTURA", alt);
            cmd.Parameters.AddWithValue("@DIAMETRO", diam);
            cmd.Parameters.AddWithValue("@DISTVERT", disVert);
            cmd.Parameters.AddWithValue("@ANGFAC", angFac);
            cmd.Parameters.AddWithValue("@ANGPANT", angPant);
            cmd.Parameters.AddWithValue("@COLOR", Color);
            cmd.Parameters.AddWithValue("@TIEMPOENTREGA", glbServicio);
            cmd.Parameters.AddWithValue("@MONTAJE", lab);
            cmd.Parameters.AddWithValue("@MEDDIST", medDisV);
            cmd.Parameters.AddWithValue("@MEDANGF", medAngF);
            cmd.Parameters.AddWithValue("@MEDANGP", medAngP);
            cmd.Parameters.AddWithValue("@MEDDDL", DDL);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet dts = new DataSet();
            da.Fill(dts);
            cmd.Parameters.Clear();
            return dts;


        }

        public DataSet MostrarParametrosCrtGrid(string cristalD, string cristalI, SqlCommand command = null)
        {

            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }
            SqlCommand cmd = command;
            cmd.Parameters.Clear();
            cmd.CommandText = "pGetParametroCristal";
            //SqlCommand cmd = new SqlCommand("SP_DETALLEFACTURAFISCAL", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@cristalD", cristalD);
            cmd.Parameters.AddWithValue("@cristalI", cristalI);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet dts = new DataSet();
            da.Fill(dts);
            cmd.Parameters.Clear();
            return dts;


        }

        public DataSet MostrarDiametroEfectivoCrtGrid(string nacCte, string cedulaCte, string numExamen, string cristalD, string cristalI, string ojo, string tipoVisionD, string tipoVisionI, string montura, string horizontal, string maxima, string puente, string suc, SqlCommand command = null)
        {
            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }
            SqlCommand cmd = command;
            cmd.Parameters.Clear();
            cmd.CommandText = "pGetDiametroEfectivo";
            cmd.CommandType = CommandType.StoredProcedure;

            // Asignando los nombres de parámetros del SP correctamente
            cmd.Parameters.AddWithValue("@NACCTE", nacCte);
            cmd.Parameters.AddWithValue("@CEDULACTE", cedulaCte);
            cmd.Parameters.AddWithValue("@NUMEXAMEN", numExamen);
            cmd.Parameters.AddWithValue("@CRISTAL", cristalD);
            cmd.Parameters.AddWithValue("@CRISTALI", cristalI);
            cmd.Parameters.AddWithValue("@OJO", ojo);
            cmd.Parameters.AddWithValue("@TIPOVISIOND", tipoVisionD);
            cmd.Parameters.AddWithValue("@TIPOVISIONI", tipoVisionI);
            cmd.Parameters.AddWithValue("@MONTURA", montura);
            cmd.Parameters.AddWithValue("@HORIZONTAL", horizontal);
            cmd.Parameters.AddWithValue("@MAXIMA", maxima);
            cmd.Parameters.AddWithValue("@PUENTE", puente);
            cmd.Parameters.AddWithValue("@SUC", suc);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet dts = new DataSet();
            da.Fill(dts);
            cmd.Parameters.Clear();

            return dts;
        }

        public DataSet MostrarValidaRangoCrtGrid(string nacCte, string cedulaCte, string numExamen, string ojo, string cristalD, string cristalI, float? alturaD, float? alturaI, string tipoVisionD, string tipoVisionI, string diametroD, string diametroI, SqlCommand command = null)
        {
            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }
            SqlCommand cmd = command;
            cmd.Parameters.Clear();
            cmd.CommandText = "pValidarangoCristal";
            cmd.CommandType = CommandType.StoredProcedure;

            // Asignando los nombres de parámetros del SP correctamente
            cmd.Parameters.AddWithValue("@Nacio", nacCte);
            cmd.Parameters.AddWithValue("@CI", cedulaCte);
            cmd.Parameters.AddWithValue("@Examen", numExamen);
            cmd.Parameters.AddWithValue("@ojo", ojo);
            cmd.Parameters.AddWithValue("@CristalD", cristalD);
            cmd.Parameters.AddWithValue("@CristalI", cristalI);
            cmd.Parameters.AddWithValue("@AlturaD", alturaD);
            cmd.Parameters.AddWithValue("@AlturaI", alturaI);
            cmd.Parameters.AddWithValue("@VisionD   ", tipoVisionD);
            cmd.Parameters.AddWithValue("@VisionI", tipoVisionI);
            cmd.Parameters.AddWithValue("@diametroD", diametroD);
            cmd.Parameters.AddWithValue("@diametroI", diametroI);


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet dts = new DataSet();
            da.Fill(dts);
            cmd.Parameters.Clear();

            return dts;
        }

        public DataSet MostrarRangoCrtGrid(string cristalD, string cristalI, SqlCommand command = null)
        {

            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }
            SqlCommand cmd = command;
            cmd.Parameters.Clear();
            cmd.CommandText = "pGetRangoCristal";
            //SqlCommand cmd = new SqlCommand("SP_DETALLEFACTURAFISCAL", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@cristalD", cristalD);
            cmd.Parameters.AddWithValue("@cristalI", cristalI);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet dts = new DataSet();
            da.Fill(dts);
            cmd.Parameters.Clear();
            return dts;


        }


        public DataSet Agregar_TB_TRABAJO(string SUC, string NUMOS, string REV, string NACIO, string CEDULA,
        string TIPO_TRABAJO, int NUM_EXAMAEN, string HORIZ, string VERT, string MAX, string PTE,
        string ALTD, string ALTI, string OJO, string TVISD, string TVISI, string LAB, string SERV,
        string TIPO_RX, string USER, string CODDETVTA, string TIPO_EXAMEN, string DISVERT, string ANPANT, string ANFAC,
        string DDL, SqlCommand command = null)
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

                cmd.Parameters.Clear();
                cmd.CommandText = "SP_CPOS_ADD_TB_TRABAJO";
                //SqlCommand cmd = new SqlCommand("SP_DETALLEFACTURAFISCAL", cn.LeerCadena());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SUC", SUC);
                cmd.Parameters.AddWithValue("@NUMOS", NUMOS);
                cmd.Parameters.AddWithValue("@REV", REV);
                cmd.Parameters.AddWithValue("@CEDULA", CEDULA);
                cmd.Parameters.AddWithValue("@NACIO", NACIO);
                cmd.Parameters.AddWithValue("@TIPO_TRABAJO", TIPO_TRABAJO);
                cmd.Parameters.AddWithValue("@NUM_EXAMAEN", NUM_EXAMAEN);
                cmd.Parameters.AddWithValue("@HORIZ", HORIZ);
                cmd.Parameters.AddWithValue("@VERT", VERT);
                cmd.Parameters.AddWithValue("@MAX", MAX);
                cmd.Parameters.AddWithValue("@PTE", PTE);
                cmd.Parameters.AddWithValue("@ALTD", ALTD);
                cmd.Parameters.AddWithValue("@ALTI", ALTI);
                cmd.Parameters.AddWithValue("@OJO", OJO);
                cmd.Parameters.AddWithValue("@TVISD", TVISD);
                cmd.Parameters.AddWithValue("@TVISI", TVISI);
                cmd.Parameters.AddWithValue("@LAB", LAB);
                cmd.Parameters.AddWithValue("@SERV", SERV);
                cmd.Parameters.AddWithValue("@TIPO_RX", TIPO_RX);
                cmd.Parameters.AddWithValue("@USER", USER);
                cmd.Parameters.AddWithValue("@CODDETVTA", CODDETVTA);
                cmd.Parameters.AddWithValue("@TIPO_EXAMEN", TIPO_EXAMEN);
                cmd.Parameters.AddWithValue("@DISVERT", DISVERT);
                cmd.Parameters.AddWithValue("@ANPANT", ANPANT);
                cmd.Parameters.AddWithValue("@ANFAC", ANFAC);
                cmd.Parameters.AddWithValue("@DDL", DDL);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                cmd.Parameters.Clear();
                // Verificar si el DataSet tiene datos
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }
                else
                {
                    return null; // Retornar null si no hay datos
                }

            }
            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return null;
            }

            //SqlCommand cmd = new SqlCommand();

            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //DataSet dts = new DataSet();
            //da.Fill(dts);
            //cmd.Parameters.Clear();
            //return dts;


        }

        public List<TB_EMPAFI> ObtenerClientesAfiliados(SqlCommand command = null)  // Trae el detalle del articulo 
        {
            // Declarar la lista para almacenar los resultados
            List<TB_EMPAFI> listaClienteAfiliado = new List<TB_EMPAFI>();
            stringBuilder.Clear();
            try
            {
                if (command == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command = connection.CreateCommand();
                }

                SqlCommand cmd = command;
                cmd.CommandText = "SP_CPOS_GET_CLIENTEAFILIADO";
                cmd.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@CodArticulo", CodArticulo);
                //command.Parameters.AddWithValue("@TipoTrabajo", TipoTrabajo);

                // Ejecutar el comando y leer los resultados
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Mapear cada fila a un objeto TB_ARTICULO
                        TB_EMPAFI clienteAfiliado = new TB_EMPAFI
                        {
                            //Rif = reader["Rif"].ToString(),
                            Codigo_Emp = reader["Codigo_Emp"].ToString(),
                            Nombre = reader["Nombre"].ToString(),

                            //    Nit = reader["Nit"].ToString(),
                            //    Direccion = reader["Direccion"].ToString(),
                            //    Telefono1 = reader["Telefono1"].ToString(),
                            //    Telefono2 = reader["Telefono2"].ToString(),
                            //    FechaAfiliacion = reader["FechaAfiliacion"] != DBNull.Value ? Convert.ToDateTime(reader["FechaAfiliacion"]) : (DateTime?)null,
                            PorcentajeDes1 = reader["PorcentajeDes1"].ToString(),
                            PorcentajeDes2 = reader["PorcentajeDes2"].ToString(),
                            PorcentajeDes3 = reader["PorcentajeDes3"].ToString(),
                            //    EMP_Fec_Crea = reader["EMP_Fec_Crea"] != DBNull.Value ? Convert.ToDateTime(reader["EMP_Fec_Crea"]) : (DateTime?)null,
                            //    EMP_Fec_Mod = reader["EMP_Fec_Mod"] != DBNull.Value ? Convert.ToDateTime(reader["EMP_Fec_Mod"]) : (DateTime?)null,
                            //    USER_Crea = reader["USER_Crea"].ToString(),
                            //    USER_Modif = reader["USER_Modif"].ToString(),
                            //    AceptaFinanciamiento = reader["AceptaFinanciamiento"] != DBNull.Value ? Convert.ToBoolean(reader["AceptaFinanciamiento"]) : (bool?)null,
                            //    FechaInicio = reader["FechaInicio"] != DBNull.Value ? Convert.ToDateTime(reader["FechaInicio"]) : (DateTime?)null,
                            //    FechaFin = reader["FechaFin"] != DBNull.Value ? Convert.ToDateTime(reader["FechaFin"]) : (DateTime?)null,
                            //    Sucursal = reader["Sucursal"].ToString(),
                            //    Activo = reader["Activo"] != DBNull.Value ? Convert.ToBoolean(reader["Activo"]) : (bool?)null
                        };


                        // Agregar el objeto a la lista
                        listaClienteAfiliado.Add(clienteAfiliado);
                    }
                }

                return listaClienteAfiliado;
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
            }
        }

        public DataTable ObtenerPromoVigente(SqlCommand command = null)
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

                cmd.CommandText = "SP_CPOS_ObtenerPromoVigente";
                cmd.CommandType = CommandType.StoredProcedure;
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

        ////////

        public async Task<string> AgregarOrdenServicio(string codSucursal, string revision, string codVenta, string cteNacio, string cteCedIden,
                                       string numExamen, string codEmpleado, string codLaboratorio, string codServicio, string vision,
                                       DateTime fecOfrecido, string horOfrecido, DateTime? fecEntrega, DateTime? fecEnvio,
                                       decimal vtaSubTotal, decimal vtaImpuesto, decimal vtaDescuento, decimal vtaTotal,
                                       bool orSerFinan, string orSerStatus, string orSerObserv, string userCrea, DateTime fecha,
                                       bool monturaPropia, string codDetVta, bool aplica, string otCorrespondiente, bool ventaAfil,
                                       bool cristalPropio, string tipoMonturaPropia, string codMotivoReposicion,
                                       string cedulaCteAfil, string codigoEmpAfil, bool? asegurada, bool? exonerada,
                                       bool monturaEnQuorum, string codColoracion, SqlCommand command = null)
        {
            return await Task.Run(() =>
            {
                if (command == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command = connection.CreateCommand();
                }

                SqlCommand cmd = command;
                cmd.Parameters.Clear();
                cmd.CommandText = "SP_CPOS_AgregarOrdenServicio";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Cod_Sucursal", codSucursal);
                cmd.Parameters.AddWithValue("@Revision", revision);
                cmd.Parameters.AddWithValue("@Cod_Venta", codVenta);
                cmd.Parameters.AddWithValue("@CTE_Nacio", cteNacio);
                cmd.Parameters.AddWithValue("@CTE_CedIden", cteCedIden);
                cmd.Parameters.AddWithValue("@NumExamen", numExamen);
                cmd.Parameters.AddWithValue("@COD_EMPLEADO", codEmpleado);
                cmd.Parameters.AddWithValue("@Cod_Laboratorio", codLaboratorio);
                cmd.Parameters.AddWithValue("@Cod_Servicio", codServicio);
                cmd.Parameters.AddWithValue("@Vision", vision);
                cmd.Parameters.AddWithValue("@Fec_Ofrecido", fecOfrecido);
                cmd.Parameters.AddWithValue("@Hor_Ofrecido", horOfrecido);
                cmd.Parameters.AddWithValue("@Fec_Entrega", (object)fecEntrega ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Fec_Envio", (object)fecEnvio ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@VtaSubTotal", vtaSubTotal);
                cmd.Parameters.AddWithValue("@VtaImpuesto", vtaImpuesto);
                cmd.Parameters.AddWithValue("@VtaDescuento", vtaDescuento);
                cmd.Parameters.AddWithValue("@VtaTotal", vtaTotal);
                cmd.Parameters.AddWithValue("@OrSer_Finan", orSerFinan);
                cmd.Parameters.AddWithValue("@OrSer_Status", orSerStatus);
                cmd.Parameters.AddWithValue("@OrSer_Observ", orSerObserv);
                cmd.Parameters.AddWithValue("@USER_Crea", userCrea);
                cmd.Parameters.AddWithValue("@Fecha", fecha);
                cmd.Parameters.AddWithValue("@MonturaPropia", monturaPropia);
                cmd.Parameters.AddWithValue("@Cod_DetVta", codDetVta);
                cmd.Parameters.AddWithValue("@Aplica", aplica);
                cmd.Parameters.AddWithValue("@OTCORRESPONDIENTE", otCorrespondiente);
                cmd.Parameters.AddWithValue("@VentaAfil", ventaAfil);
                cmd.Parameters.AddWithValue("@CristalPropio", cristalPropio);
                cmd.Parameters.AddWithValue("@TipoMonturaPropia", (object)tipoMonturaPropia ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CodMotivoReposicion", (object)codMotivoReposicion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Cedula_CteAfil", (object)cedulaCteAfil ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Codigo_EmpAfil", (object)codigoEmpAfil ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Asegurada", (object)asegurada ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Exonerada", (object)exonerada ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MonturaEnQuorum", (object)monturaEnQuorum ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Cod_Coloracion", (object)codColoracion ?? DBNull.Value);

                SqlParameter numOrdenParam = new SqlParameter("@NumOrdserv", SqlDbType.VarChar, 20)
                {
                    Direction = ParameterDirection.Output
                };

                cmd.Parameters.Add(numOrdenParam);

                cmd.ExecuteNonQuery();

                string numeroOrdenGenerado = numOrdenParam.Value.ToString();
                cmd.Parameters.Clear();

                return numeroOrdenGenerado;
            });
        }

        public DataSet ObtenerInfoReposicion(string cedula, string nacio, string os,string suc,string nroExamen, SqlCommand command = null)
        {

            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }
            SqlCommand cmd = command;
            cmd.Parameters.Clear();
            cmd.CommandText = "pGetInfoReposicion";
            //SqlCommand cmd = new SqlCommand("SP_DETALLEFACTURAFISCAL", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CI", cedula);
            cmd.Parameters.AddWithValue("@NACIO", nacio);
            cmd.Parameters.AddWithValue("@OS", os);
            cmd.Parameters.AddWithValue("@suc", suc);
            cmd.Parameters.AddWithValue("@NEWRX", nroExamen);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet dts = new DataSet();
            da.Fill(dts);
            cmd.Parameters.Clear();
            return dts;


        }

        public DataSet AplicarPromociones(string CODPROMO, Dictionary<string, string> parametros = null, SqlCommand command = null)
        {
            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }

            SqlCommand cmd = command;
            cmd.Parameters.Clear();
            cmd.CommandText = "pEvaluoPromociones";
            cmd.CommandType = CommandType.StoredProcedure;

            // Agregar el parámetro obligatorio @CODPROMO
            cmd.Parameters.AddWithValue("@CODPROMO", CODPROMO);

            // Agregar los parámetros opcionales si existen
            if (parametros != null)
            {
                foreach (var parametro in parametros)
                {
                    cmd.Parameters.AddWithValue(parametro.Key, parametro.Value ?? (object)DBNull.Value);
                }
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet dts = new DataSet();
            da.Fill(dts);
            cmd.Parameters.Clear();
            return dts;
        }



        public List<TB_LABORATORIOSDTO> DatosLaboratorio(SqlCommand command = null)
        {
            try
            {
                if (command == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command = connection.CreateCommand();
                }
                // Declarar la lista para almacenar los resultados
                List<TB_LABORATORIOSDTO> listaCodLaboratorio = new List<TB_LABORATORIOSDTO>();

                SqlCommand cmd = command;
                cmd.Parameters.Clear();
                cmd.CommandText = "SP_CPOS_ListaLaboratorios";
                cmd.CommandType = CommandType.StoredProcedure;

                // Ejecutar el comando y leer los resultados
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Mapear cada fila a un objeto TB_ARTICULO
                        TB_LABORATORIOSDTO codLaboratorio = new TB_LABORATORIOSDTO
                        {
                            CODIGO_LAB = reader["CodLab"].ToString(),
                            DESCRIPCION = reader["Laboratorio"].ToString()
                        };


                        // Agregar el objeto a la lista
                        listaCodLaboratorio.Add(codLaboratorio);
                    }
                }

                return listaCodLaboratorio;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener laboratorios: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }


        }

       

        public DataSet ObtenerColorLC(string CodArticulo, SqlCommand command = null)
        {
            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }

            SqlCommand cmd = command;
            cmd.Parameters.Clear();
            cmd.CommandText = "SP_CPOS_GET_COLORLC";
            cmd.CommandType = CommandType.StoredProcedure;

            // Agregar el parámetro obligatorio @CODPROMO
            cmd.Parameters.AddWithValue("@CodArticulo", CodArticulo);

           
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet dts = new DataSet();
            da.Fill(dts);
            cmd.Parameters.Clear();
            return dts;
        }



        public List<TB_SERVICIOSLABDTO> ServiciosLaboratorio(SqlCommand command = null)
        {
            try
            {
                if (command == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command = connection.CreateCommand();
                }
                // Declarar la lista para almacenar los resultados
                List<TB_SERVICIOSLABDTO> listaServLaboratorio = new List<TB_SERVICIOSLABDTO>();

                SqlCommand cmd = command;
                cmd.Parameters.Clear();
                cmd.CommandText = "SP_CPOS_CodServiciosLaboratorio";
                cmd.CommandType = CommandType.StoredProcedure;

                // Ejecutar el comando y leer los resultados
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Mapear cada fila a un objeto TB_ARTICULO
                        TB_SERVICIOSLABDTO codServLaboratorio = new TB_SERVICIOSLABDTO
                        {
                            Cod_servicio = reader["CodServ"].ToString(),
                            Descripcion_servicio = reader["ServicioLab"].ToString()
                        };


                        // Agregar el objeto a la lista
                        listaServLaboratorio.Add(codServLaboratorio);
                    }
                }

                return listaServLaboratorio;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener Servicios de laboratorios: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }


        }

        public DataTable ValidacionMonturaQuorum(string codArticulo, string codSucursal, string codServicio, SqlCommand command = null)
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
                cmd.CommandText = "SP_CPOS_pGetMonturaAlmacenQUORUM";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NumordServ", ""); // vacío
                cmd.Parameters.AddWithValue("@CodArticulo", codArticulo);
                cmd.Parameters.AddWithValue("@suc", codSucursal);
                cmd.Parameters.AddWithValue("@CodServicio", codServicio);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                cmd.Parameters.Clear();
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

        public DataTable ObtenerBajaExistenciaCristales(string cedNacio, string cedId, string sucursal, string examen, string codArticulo, SqlCommand command = null)
        {
            if (command == null)
            {
                SqlConnection conn = cn.LeerCadena();
                command = conn.CreateCommand();
            }

            SqlCommand cmd = command;
            cmd.Parameters.Clear();
            cmd.CommandText = "pGetBajaExistencia";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CTE_Nacio", cedNacio);
            cmd.Parameters.AddWithValue("@CTE_CedIden", cedId);
            cmd.Parameters.AddWithValue("@COD_Sucursal", sucursal);
            cmd.Parameters.AddWithValue("@NUM_Examen", examen);
            cmd.Parameters.AddWithValue("@CodArticulo", codArticulo);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        public string ObtenerCodVentaDesdeCodModo(string codModo, SqlCommand command = null)
        {
            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }

            SqlCommand cmd = command;
            cmd.Parameters.Clear();
            cmd.CommandText = "SP_CPOS_TipoVenta";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Cod_DetVta", codModo);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader["CodVenta"].ToString();
                }
            }

            return null; 
        }

        public async Task<bool> GuardarDescripcionDetalleOrdenServicio(string numeroOrdenServicio, string numeroRevision,
                                                                            string codVenta, string codigoArticulo, int cantidad, 
                                                                            string ojo, decimal precio, decimal porcentajeImpuesto,
                                                                            decimal porcentajeDescuento, decimal precioAnterior, 
                                                                            string codPromocion, decimal costoArticulo, string sucursalActual, 
                                                                            SqlCommand command, int numeroSecuencia)
        {
            try
            {
                command.Parameters.Clear();
                command.CommandText = "SP_CPOS_InsertarDetalleOrdenServicio";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@CodSucursal", sucursalActual);
                command.Parameters.AddWithValue("@NumOrdserv", numeroOrdenServicio);
                command.Parameters.AddWithValue("@Ordserv_sec", numeroSecuencia);
                command.Parameters.AddWithValue("@Revision", numeroRevision);
                command.Parameters.AddWithValue("@Cod_Venta", codVenta);
                command.Parameters.AddWithValue("@CodArticulo", codigoArticulo);
                command.Parameters.AddWithValue("@Ordserv_Cant", cantidad);
                command.Parameters.AddWithValue("@Ordser_Ojo", (object)ojo ?? DBNull.Value);
                command.Parameters.AddWithValue("@Ordserv_Precio", precio);
                command.Parameters.AddWithValue("@Ordserv_PorcImp", porcentajeImpuesto);
                command.Parameters.AddWithValue("@Ordserv_PorcDto", porcentajeDescuento);
                command.Parameters.AddWithValue("@Ordserv_PrecioAnterior", precioAnterior);
                command.Parameters.AddWithValue("@COD_Prom", (object)codPromocion ?? DBNull.Value);
                command.Parameters.AddWithValue("@Costo", costoArticulo);

                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el detalle de la orden: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public async Task<bool> ModificarTrabajoAsync(string cedula, string nacRif, string numeroOrden, DateTime fecha, string usuario, string sucursal, string correlativoOS, SqlCommand command)
        {
            try
            {
                command.Parameters.Clear();
                command.CommandText = "SP_CPOS_ModificarTrabajo";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@CedulaCliente", cedula);
                command.Parameters.AddWithValue("@NacioRifCliente", nacRif);
                command.Parameters.AddWithValue("@OrdenServicio", numeroOrden);
                command.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                command.Parameters.AddWithValue("@UsuarioModificacion", usuario);
                command.Parameters.AddWithValue("@SucursalActual", sucursal);
                command.Parameters.AddWithValue("@CorrelativoOS", correlativoOS);

                await command.ExecuteNonQueryAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }


        //public async Task<bool> RebajarInventarioAsync(string codArticulo, string codLaboratorio, int cantidad, SqlCommand command)
        //{
        //    try
        //    {
        //        command.Parameters.Clear();
        //        command.CommandText = "SP_CPOS_RebajarInventario"; // Igual, deberías tener o crear este SP
        //        command.CommandType = CommandType.StoredProcedure;

        //        command.Parameters.AddWithValue("@CodArticulo", codArticulo);
        //        command.Parameters.AddWithValue("@CodLaboratorio", (object)codLaboratorio ?? DBNull.Value);
        //        command.Parameters.AddWithValue("@Cantidad", cantidad);

        //        await command.ExecuteNonQueryAsync();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

    }
}
