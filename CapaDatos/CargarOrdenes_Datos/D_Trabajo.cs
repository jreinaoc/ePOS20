using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades; // Asegúrate de que Trabajo esté en este namespace
using CapaDatos.Conexion;
using System.Data;

namespace CapaDatos.CargarOrdenes_Datos // O el namespace que prefieras para tus capas de datos
{
    public class D_Trabajo
    {
        private Conexion.Conexion cn = new Conexion.Conexion();
        private StringBuilder stringBuilder = new StringBuilder();

        public TB_TRABAJOCTE ObtenerTrabajoPorOrdenServicio(string nacionalidad, string cedula, int T_EXAMEN)
        {
            TB_TRABAJOCTE trabajo = null;
            // Asegúrate de que stringBuilder esté inicializado, por ejemplo:
            // private StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Clear();
            try
            {
                using (SqlConnection connection = cn.LeerCadena())
                {
                    //connection.Open(); // <-- Descomentar esta línea
                    string query = "[dbo].[SP_CPOSC_GET_TB_TrabajoPorOrdenServicio]"; // No necesitas los corchetes dobles si ya están en el SP
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure; // <-- Añadir esta línea

                        command.Parameters.AddWithValue("@Nacionalidad", nacionalidad);
                        command.Parameters.AddWithValue("@Cedula", cedula);
                        command.Parameters.AddWithValue("@NumOrdServ", T_EXAMEN); // El nombre del parámetro en el SP debe ser @NumOrdServ

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                trabajo = MapDataReaderToTrabajo(reader);
                            }
                        }
                    }
                }
                return trabajo;
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error al obtener Trabajo por Orden de Servicio: {0}", ex.Message));
                // Considerar loguear el error de forma más robusta, por ejemplo, usando un logger.
                return null;
            }
        }

        public void AgregarTrabajo(TB_TRABAJOCTE nuevoTrabajo)
        {
            stringBuilder.Clear();
          
            try
            {
                using (SqlConnection connection = cn.LeerCadena())
                {
                    //connection.Open();
                    using (SqlCommand command = new SqlCommand("SP_CPOSC_AgregarActualizarTrabajo", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Agregar los parámetros al comando
                        command.Parameters.AddWithValue("@TSucursal", nuevoTrabajo.TSucursal ?? (object)DBNull.Value);
                        //command.Parameters.AddWithValue("@TNumOrdserv", nuevoTrabajo.TNumOrdserv ?? (object)DBNull.Value);
                        //command.Parameters.AddWithValue("@TRevision", nuevoTrabajo.TRevision ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TCEDIDEN", nuevoTrabajo.TCEDIDEN ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TNACIO", nuevoTrabajo.TNACIO ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TTIPOTRABAJO", nuevoTrabajo.TTIPOTRABAJO ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TEXAMEN", nuevoTrabajo.TEXAMEN ?? (object)DBNull.Value);
                        //command.Parameters.AddWithValue("@THORIZONTAL", nuevoTrabajo.THORIZONTAL ?? (object)DBNull.Value);
                        //command.Parameters.AddWithValue("@TVERTICAL", nuevoTrabajo.TVERTICAL ?? (object)DBNull.Value);
                        //command.Parameters.AddWithValue("@TMAXIMA", nuevoTrabajo.TMAXIMA ?? (object)DBNull.Value);
                        //command.Parameters.AddWithValue("@TPUENTE", nuevoTrabajo.TPUENTE ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TALTD", nuevoTrabajo.TALTD);
                        command.Parameters.AddWithValue("@TALTI", nuevoTrabajo.TALTI);
                        command.Parameters.AddWithValue("@TOJO", nuevoTrabajo.TOJO?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TTIPOVISIOND", nuevoTrabajo.TTIPOVISIOND ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TTIPOVISIONI", nuevoTrabajo.TTIPOVISIONI ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TLABORATORIO", "");
                        //command.Parameters.AddWithValue("@TSERVICIO", nuevoTrabajo.TSERVICIO ?? (object)DBNull.Value);
                        //command.Parameters.AddWithValue("@THORAOFRECIDO", nuevoTrabajo.THORAOFRECIDO ?? (object)DBNull.Value);
                        //command.Parameters.AddWithValue("@TTIPORX", nuevoTrabajo.TTIPORX ?? (object)DBNull.Value);
                        //command.Parameters.AddWithValue("@TFECHAOFRECIDO", nuevoTrabajo.TFECHAOFRECIDO.HasValue ? (object)nuevoTrabajo.TFECHAOFRECIDO.Value : DBNull.Value);
                        //command.Parameters.AddWithValue("@TFECCREA", nuevoTrabajo.TFECCREA.HasValue ? (object)nuevoTrabajo.TFECCREA.Value : DBNull.Value);
                        //command.Parameters.AddWithValue("@TFECMOD", nuevoTrabajo.TFECMOD.HasValue ? (object)nuevoTrabajo.TFECMOD.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@USERCREA", nuevoTrabajo.USERCREA ?? (object)DBNull.Value);
                        //command.Parameters.AddWithValue("@USERMOD", nuevoTrabajo.USERMOD ?? (object)DBNull.Value);
                        //command.Parameters.AddWithValue("@CodDetVta", nuevoTrabajo.CodDetVta.HasValue ? (object)nuevoTrabajo.CodDetVta.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@TipoExamen", nuevoTrabajo.TipoExamen ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TDISTANCIAVERTICE", nuevoTrabajo.TDISTANCIAVERTICE ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TANGULOPANTOSCOPICO", nuevoTrabajo.TANGULOPANTOSCOPICO ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TANGULOFACIAL", nuevoTrabajo.TANGULOFACIAL ?? (object)DBNull.Value);
                        //command.Parameters.AddWithValue("@Correlativo", nuevoTrabajo.Correlativo.HasValue ? (object)nuevoTrabajo.Correlativo.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@TDISTANCIADELECTURA", nuevoTrabajo.TDISTANCIADELECTURA ?? (object)DBNull.Value);

                        // Ejecutar el Stored Procedure y obtener el número de filas afectadas
                        command.ExecuteNonQuery();
                    }
                }
                
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error al agregar Trabajo: {0}", ex.Message));
             
            }
        }

        public bool AgregarTrabajo2(TB_TRABAJOCTE nuevoTrabajo, SqlCommand command1 = null)
        {
            stringBuilder.Clear();

            try
            {
                if (command1 == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command1 = connection.CreateCommand();
                }

                SqlCommand command = command1;
                command.Parameters.Clear();
                command.CommandText = "SP_CPOSC_AgregarActualizarTrabajo";
                command.CommandType = CommandType.StoredProcedure;

                        // Agregar los parámetros al comando
                        command.Parameters.AddWithValue("@TSucursal", nuevoTrabajo.TSucursal ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TNumOrdserv", nuevoTrabajo.TNumOrdserv ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TRevision", nuevoTrabajo.TRevision ?? "0");
                        command.Parameters.AddWithValue("@TCEDIDEN", nuevoTrabajo.TCEDIDEN ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TNACIO", nuevoTrabajo.TNACIO ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TTIPOTRABAJO", nuevoTrabajo.TTIPOTRABAJO ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TEXAMEN", nuevoTrabajo.TEXAMEN ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@THORIZONTAL", nuevoTrabajo.THORIZONTAL ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TVERTICAL", nuevoTrabajo.TVERTICAL ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TMAXIMA", nuevoTrabajo.TMAXIMA ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TPUENTE", nuevoTrabajo.TPUENTE ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TALTD", nuevoTrabajo.TALTD);
                        command.Parameters.AddWithValue("@TALTI", nuevoTrabajo.TALTI);
                        command.Parameters.AddWithValue("@TOJO", nuevoTrabajo.TOJO ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TTIPOVISIOND", nuevoTrabajo.TTIPOVISIOND ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TTIPOVISIONI", nuevoTrabajo.TTIPOVISIONI ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TLABORATORIO", nuevoTrabajo.TLABORATORIO ?? "");
                        command.Parameters.AddWithValue("@TSERVICIO", nuevoTrabajo.TSERVICIO ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@THORAOFRECIDO", nuevoTrabajo.THORAOFRECIDO ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TTIPORX", nuevoTrabajo.TTIPORX ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TFECHAOFRECIDO", nuevoTrabajo.TFECHAOFRECIDO.HasValue ? (object)nuevoTrabajo.TFECHAOFRECIDO.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@TFECCREA", nuevoTrabajo.TFECCREA.HasValue ? (object)nuevoTrabajo.TFECCREA.Value : DBNull.Value);
                        //command.Parameters.AddWithValue("@TFECMOD", nuevoTrabajo.TFECMOD.HasValue ? (object)nuevoTrabajo.TFECMOD.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@USERCREA", nuevoTrabajo.USERCREA ?? (object)DBNull.Value);
                        //command.Parameters.AddWithValue("@USERMOD", nuevoTrabajo.USERMOD ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@CodDetVta", nuevoTrabajo.CodDetVta ?? (object) DBNull.Value);
                        command.Parameters.AddWithValue("@TipoExamen", nuevoTrabajo.TipoExamen ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TDISTANCIAVERTICE", nuevoTrabajo.TDISTANCIAVERTICE ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TANGULOPANTOSCOPICO", nuevoTrabajo.TANGULOPANTOSCOPICO ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TANGULOFACIAL", nuevoTrabajo.TANGULOFACIAL ?? (object)DBNull.Value);
                        //command.Parameters.AddWithValue("@Correlativo", nuevoTrabajo.Correlativo.HasValue ? (object)nuevoTrabajo.Correlativo.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@TDISTANCIADELECTURA", nuevoTrabajo.TDISTANCIADELECTURA ?? (object)DBNull.Value);
                // Ejecutar el Stored Procedure y obtener el número de filas afectadas

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataSet dts = new DataSet();
                da.Fill(dts);
                command.Parameters.Clear();

                return true;
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error al agregar Trabajo: {0}", ex.Message));
                return false;
            }
        }

        public void ActualizarTrabajoRx(TB_TRABAJOCTE nuevoTrabajo)
        {
            stringBuilder.Clear();

            try
            {
                using (SqlConnection connection = cn.LeerCadena())
                {
                    //connection.Open();
                    using (SqlCommand command = new SqlCommand("SP_CPOSC_AgregarActualizarTrabajoRx", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Agregar los parámetros al comando
                        command.Parameters.AddWithValue("@TSucursal", nuevoTrabajo.TSucursal ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TNumOrdserv", nuevoTrabajo.TNumOrdserv ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TRevision", nuevoTrabajo.TRevision ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TCEDIDEN", nuevoTrabajo.TCEDIDEN ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TNACIO", nuevoTrabajo.TNACIO ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TEXAMEN", nuevoTrabajo.TEXAMEN ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@USERMOD", nuevoTrabajo.USERMOD ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TDISTANCIAVERTICE", nuevoTrabajo.TDISTANCIAVERTICE ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TANGULOPANTOSCOPICO", nuevoTrabajo.TANGULOPANTOSCOPICO ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TANGULOFACIAL", nuevoTrabajo.TANGULOFACIAL ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TDISTANCIADELECTURA", nuevoTrabajo.TDISTANCIADELECTURA ?? (object)DBNull.Value);

                        // Ejecutar el Stored Procedure y obtener el número de filas afectadas
                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error al agregar Trabajo: {0}", ex.Message));

            }
        }
        private TB_TRABAJOCTE MapDataReaderToTrabajo(SqlDataReader reader)
        {
            return new TB_TRABAJOCTE
            {

                TDISTANCIAVERTICE = reader["T_DISTANCIAVERTICE"] != DBNull.Value ? Convert.ToDecimal(reader["T_DISTANCIAVERTICE"]) : (decimal?)null,
                TANGULOPANTOSCOPICO = reader["T_ANGULOPANTOSCOPICO"] != DBNull.Value ? Convert.ToDecimal(reader["T_ANGULOPANTOSCOPICO"]) : (decimal?)null,
                TANGULOFACIAL = reader["T_ANGULOFACIAL"] != DBNull.Value ? Convert.ToDecimal(reader["T_ANGULOFACIAL"]) : (decimal?)null,
                TDISTANCIADELECTURA = reader["T_DISTANCIADELECTURA"] != DBNull.Value ? Convert.ToDecimal(reader["T_DISTANCIADELECTURA"]) : (decimal?)null,
                //TSUCURSAL = reader["T_SUCURSAL"] != DBNull.Value ? reader["T_SUCURSAL"].ToString() : null,
                TNumOrdserv = reader["T_NumOrdserv"] != DBNull.Value ? reader["T_NumOrdserv"].ToString() : null,
                TRevision = reader["T_Revision"] != DBNull.Value ? reader["T_Revision"].ToString() : null,
                TCEDIDEN = reader["T_CEDIDEN"] != DBNull.Value ? reader["T_CEDIDEN"].ToString() : null,
                //TNACIO = reader["T_NACIO"] != DBNull.Value ? Convert.ToDateTime(reader["T_NACIO"]) : (DateTime?)null,
                TTIPOTRABAJO = reader["T_TIPOTRABAJO"] != DBNull.Value ? reader["T_TIPOTRABAJO"].ToString() : null,
                TEXAMEN = reader["T_EXAMEN"] != DBNull.Value ? reader["T_EXAMEN"].ToString() : null,
                //THORIZONTAL = reader["T_HORIZONTAL"] != DBNull.Value ? Convert.ToDecimal(reader["T_HORIZONTAL"]) : (decimal?)null,
                //TVERTICAL = reader["T_VERTICAL"] != DBNull.Value ? Convert.ToDecimal(reader["T_VERTICAL"]) : (decimal?)null,
                //TMAXIMA = reader["T_MAXIMA"] != DBNull.Value ? Convert.ToDecimal(reader["T_MAXIMA"]) : (decimal?)null,
                //TPUENTE = reader["T_PUENTE"] != DBNull.Value ? Convert.ToDecimal(reader["T_PUENTE"]) : (decimal?)null,
                //TALTD = reader["T_ALTD"] != DBNull.Value ? Convert.ToDecimal(reader["T_ALTD"]) : (decimal?)null,
                //TALTI = reader["T_ALTI"] != DBNull.Value ? Convert.ToDecimal(reader["T_ALTI"]) : (decimal?)null,
                TOJO = reader["T_OJO"] != DBNull.Value ? reader["T_OJO"].ToString() : null,
                TTIPOVISIOND = reader["T_TIPOVISIOND"] != DBNull.Value ? reader["T_TIPOVISIOND"].ToString() : null,
                TTIPOVISIONI = reader["T_TIPOVISIONI"] != DBNull.Value ? reader["T_TIPOVISIONI"].ToString() : null,
                TLABORATORIO = reader["T_LABORATORIO"] != DBNull.Value ? reader["T_LABORATORIO"].ToString() : null,
                TSERVICIO = reader["T_SERVICIO"] != DBNull.Value ? reader["T_SERVICIO"].ToString() : null,
                //THORAOFRECIDO = reader["T_HORAOFRECIDO"] != DBNull.Value ? TimeSpan.Parse(reader["T_HORAOFRECIDO"].ToString()) : (TimeSpan?)null,
                TTIPORX = reader["T_TIPORX"] != DBNull.Value ? reader["T_TIPORX"].ToString() : null,
                TFECHAOFRECIDO = reader["T_FECHAOFRECIDO"] != DBNull.Value ? Convert.ToDateTime(reader["T_FECHAOFRECIDO"]) : (DateTime?)null,
                TFECCREA = reader["T_FECCREA"] != DBNull.Value ? Convert.ToDateTime(reader["T_FECCREA"]) : (DateTime?)null,
                TFECMOD = reader["T_FECMOD"] != DBNull.Value ? Convert.ToDateTime(reader["T_FECMOD"]) : (DateTime?)null,
                USERCREA = reader["USER_CREA"] != DBNull.Value ? reader["USER_CREA"].ToString() : null,
                USERMOD = reader["USER_MOD"] != DBNull.Value ? reader["USER_MOD"].ToString() : null,
                //Cod_DetVta = reader["Cod_DetVta"] != DBNull.Value ? reader["Cod_DetVta"].ToString() : null,
                TipoExamen = reader["TipoExamen"] != DBNull.Value ? reader["TipoExamen"].ToString() : null,
                //Correlativo = reader["Correlativo"] != DBNull.Value ? reader["Correlativo"].ToString() : null
            };
        }

        //public bool AgregarGiftCard(string codSucursal, string nroOrden, string revision, decimal montoDolares, string nombreBeneficiario, string correoBeneficiario, string mensaje, string codigoGiftCard, string userCrea, string userMod, SqlCommand command1 = null)
        //{
        //    stringBuilder.Clear();

        //    try
        //    {
        //        if (command1 == null)
        //        {
        //            SqlConnection connection = cn.LeerCadena();
        //            command1 = connection.CreateCommand();
        //        }

        //        SqlCommand command = command1;
        //        command.Parameters.Clear();
        //        command.CommandText = "SP_CPOS_AddOrdenGiftfCard";
        //        command.CommandType = CommandType.StoredProcedure;

        //        // Agregar los parámetros individuales al comando
        //        command.Parameters.AddWithValue("@CodSucursal", codSucursal ?? (object)DBNull.Value);
        //        command.Parameters.AddWithValue("@NroOrden", nroOrden ?? (object)DBNull.Value);
        //        command.Parameters.AddWithValue("@Revision", string.IsNullOrEmpty(revision) ? "0" : revision);
        //        command.Parameters.AddWithValue("@MontoDolares", montoDolares);
        //        command.Parameters.AddWithValue("@NombreBeneficiario", nombreBeneficiario ?? (object)DBNull.Value);
        //        command.Parameters.AddWithValue("@CorreoBeneficiario", correoBeneficiario ?? (object)DBNull.Value);
        //        command.Parameters.AddWithValue("@Mensaje", mensaje ?? (object)DBNull.Value);
        //        command.Parameters.AddWithValue("@CodigoGiftCard", codigoGiftCard ?? (object)DBNull.Value);
        //        command.Parameters.AddWithValue("@UserCrea", userCrea ?? (object)DBNull.Value);
        //        command.Parameters.AddWithValue("@UserMod", userMod ?? (object)DBNull.Value);

        //        // Ejecutar el Stored Procedure
        //        SqlDataAdapter da = new SqlDataAdapter(command);
        //        DataSet dts = new DataSet();
        //        da.Fill(dts);

        //        command.Parameters.Clear();

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        stringBuilder.Append(Environment.NewLine + string.Format("Error al agregar Orden GiftCard: {0}", ex.Message));
        //        return false;
        //    }
        //}
        public bool ObtenerGiftCard(string codSucursal, string nroOrden, string revision, SqlCommand command1 = null)
        {
            stringBuilder.Clear();

            try
            {
                if (command1 == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command1 = connection.CreateCommand();
                }

                SqlCommand command = command1;
                command.Parameters.Clear();
                command.CommandText = "SP_CPOS_GetOrdenGiftfCard";
                command.CommandType = CommandType.StoredProcedure;

                // Agregar los parámetros individuales al comando
                command.Parameters.AddWithValue("@CodSucursal", codSucursal ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@NroOrden", nroOrden ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Revision", string.IsNullOrEmpty(revision) ? "0" : revision);

                // Ejecutar el Stored Procedure
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataSet dts = new DataSet();
                da.Fill(dts);

                command.Parameters.Clear();

                return true;
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error al obtener Orden GiftCard: {0}", ex.Message));
                return false;
            }
        }
    }
}
