using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades;

namespace CapaDatos.CargarOrdenes_Datos
{
    public class D_Articulos
    {
        Conexion.Conexion cn = new Conexion.Conexion();
        //El uso de la clase StringBuilder nos ayudara a devolver los mensajes 
        public readonly StringBuilder stringBuilder = new StringBuilder();

        public List <TB_ARTICULO> ObtenerArticulos(SqlCommand command = null)  // Trae el detalle del articulo 
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
                        COSTOPROME = reader["COSTOPROME"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["COSTOPROME"]) : null,
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
    }
}
