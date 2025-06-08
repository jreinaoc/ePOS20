using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades; // Asegúrate de que TB_EXAMEN esté en este namespace
using CapaDatos.Conexion;

namespace CapaDatos.CargarOrdenes_Datos
{
    public class D_Examen
    {
        Conexion.Conexion cn = new Conexion.Conexion();
        public readonly StringBuilder stringBuilder = new StringBuilder();

        public List<TB_EXAMENCTE> ObtenerTodosLosExamenes()
        {
            List<TB_EXAMENCTE> examenes = new List<TB_EXAMENCTE>();
            stringBuilder.Clear();
            try
            {
                using (SqlConnection connection = cn.LeerCadena())
                {
                    connection.Open();
                    string query = "" +
                        "" +
                  "SELECT CTE_Nacio, CTE_CedIden, COD_Sucursal, NUM_Examen, FEC_Examen, " +
                    "ISNULL(ESFD, 0) AS ESFD, ISNULL(ESFI, 0) AS ESFI, ISNULL(CILD, 0) AS CILD, ISNULL(CILI, 0) AS CILI, ISNULL(EJED, 0) AS EJED, ISNULL(EJEI, 0) AS EJEI, ISNULL(ADDD, 0) AS ADDD, ISNULL(ADDI, 0) AS ADDI, OBSERVACIONES, " +
                    "TIPO_Optm, NOM_Optm, EXA_Feccreacion, EXA_Fecmod, USER_CREA, USER_MOD, TIPOEXAMEN, NOMBRE_CLINICA_OPTM, " +
                    "TLF_TIPO, TLF_COD, TLF_NUMERO, TLF_EXT, ISNULL(ESFD2, 0) AS ESFD2, ISNULL(ESFI2, 0) AS ESFI2, ISNULL(CILD2, 0) AS CILD2, ISNULL(CILI2, 0) AS CILI2, ISNULL(EJED2, 0) AS EJED2, ISNULL(EJEI2, 0) AS EJEI2, CodigoMimesys " +
                    "FROM [dbo].[TB_EXAMEN]";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                examenes.Add(MapDataReaderToExamen(reader));
                            }
                        }
                    }
                }
                return examenes;
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
            }
        }



        public TB_EXAMENCTE ObtenerExamenPorNumeroYNacionalidadCedula(int numeroExamen, string nacionalidad, string cedula)
        {
            TB_EXAMENCTE examen = null;
            StringBuilder stringBuilder = new StringBuilder(); // Usar StringBuilder para construir el mensaje de error

            try
            {
                using (SqlConnection connection = cn.LeerCadena())
                {
                   // connection.Open(); // Asegúrate de abrir la conexión

                    using (SqlCommand command = new SqlCommand("SP_CPOSC_ObtenerExamenPorNumeroYNacionalidadCedula", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure; // Indica que es un Stored Procedure

                        // Añade los parámetros al comando
                        command.Parameters.AddWithValue("@NumeroExamen", numeroExamen);
                        command.Parameters.AddWithValue("@Nacionalidad", nacionalidad);
                        command.Parameters.AddWithValue("@Cedula", cedula);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                examen = MapDataReaderToExamen(reader);
                            }
                        }
                    }
                }
                return examen;
            }
            catch (Exception ex)
            {
                stringBuilder.AppendLine("Error al obtener el examen:"); // Usar AppendLine para mejor formato
                stringBuilder.AppendLine(ex.Message);
                // Considera loggear el error en un archivo o sistema de registro.
                Console.Error.WriteLine(stringBuilder.ToString()); // Imprimir en la consola de error
                return null;
            }
        }
        public TB_EXAMENCTE ObtenerExamenPorNacionalidadCedula(string nacionalidad, string cedula)
        {
            TB_EXAMENCTE examen = null;
            stringBuilder.Clear();
            try
            {
                using (SqlConnection connection = cn.LeerCadena())
                {
                    connection.Open();
                    string query = @"SELECT CTE_Nacio, CTE_CedIden, COD_Sucursal, NUM_Examen, FEC_Examen, ESFD, ESFI, CILD, CILI, EJED, EJEI, ADDD, ADDI, 
                                   OBSERVACIONES, TIPO_Optm, NOM_Optm, EXA_Feccreacion, EXA_Fecmod, USER_CREA, USER_MOD, TIPOEXAMEN, 
                                   NOMBRE_CLINICA_OPTM, TLF_TIPO, TLF_COD, TLF_NUMERO, TLF_EXT, ESFD2, ESFI2, CILD2, CILI2, EJED2, EJEI2, 
                                   CodigoMimesys 
                            FROM [dbo].[TB_EXAMEN] 
                            WHERE CTE_Nacio = @Nacionalidad AND CTE_CedIden = @Cedula";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nacionalidad", nacionalidad);
                        command.Parameters.AddWithValue("@Cedula", cedula);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                examen = MapDataReaderToExamen(reader);
                            }
                        }
                    }
                }
                return examen;
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
            }
        }


        public TB_EXAMENCTE ObtenerExamenPorNumero(int numeroExamen)
        {
            TB_EXAMENCTE examen = null;
            stringBuilder.Clear();
            try
            {
                using (SqlConnection connection = cn.LeerCadena())
                {
                    connection.Open();
                    string query = "SELECT CTE_Nacio, CTE_CedIden, COD_Sucursal, NUM_Examen, FEC_Examen, ESFD, ESFI, CILD, CILI, EJED, EJEI, ADDD, ADDI, OBSERVACIONES, TIPO_Optm, NOM_Optm, EXA_Feccreacion, EXA_Fecmod, USER_CREA, USER_MOD, TIPOEXAMEN, NOMBRE_CLINICA_OPTM, TLF_TIPO, TLF_COD, TLF_NUMERO, TLF_EXT, ESFD2, ESFI2, CILD2, CILI2, EJED2, EJEI2, CodigoMimesys FROM [dbo].[TB_EXAMEN] WHERE NUM_Examen = @NumeroExamen";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NumeroExamen", numeroExamen);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                examen = MapDataReaderToExamen(reader);
                            }
                        }
                    }
                }
                return examen;
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
            }
        }

        public void AgregarExamen(TB_EXAMENCTE nuevoExamen)
        {
            stringBuilder.Clear();
            try
            {
                using (SqlConnection connection = cn.LeerCadena())
                {
                    //connection.Open();
                    using (SqlCommand command = new SqlCommand("SP_CPOSC_AgregarActualizarExamen", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Agregar los parámetros al comando
                        command.Parameters.AddWithValue("@CTE_Nacio", nuevoExamen.CTE_Nacio ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@CTE_CedIden", nuevoExamen.CTE_CedIden ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@COD_Sucursal", nuevoExamen.COD_Sucursal ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@NUM_Examen", nuevoExamen.NUM_Examen);
                        command.Parameters.AddWithValue("@FEC_Examen", nuevoExamen.FEC_Examen);
                        command.Parameters.AddWithValue("@ESFD", nuevoExamen.ESFD);
                        command.Parameters.AddWithValue("@ESFI", nuevoExamen.ESFI);
                        command.Parameters.AddWithValue("@CILD", nuevoExamen.CILD);
                        command.Parameters.AddWithValue("@CILI", nuevoExamen.CILI);
                        command.Parameters.AddWithValue("@EJED", nuevoExamen.EJED);
                        command.Parameters.AddWithValue("@EJEI", nuevoExamen.EJEI);
                        command.Parameters.AddWithValue("@ADDD", nuevoExamen.ADDD);
                        command.Parameters.AddWithValue("@ADDI", nuevoExamen.ADDI);
                        command.Parameters.AddWithValue("@OBSERVACIONES", nuevoExamen.OBSERVACIONES ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TIPO_Optm", nuevoExamen.TIPO_Optm ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@NOM_Optm", nuevoExamen.NOM_Optm ?? (object)DBNull.Value);
                         command.Parameters.AddWithValue("@EXA_Feccreacion", nuevoExamen.FEC_Examen);
                         command.Parameters.AddWithValue("@EXA_Fecmod", nuevoExamen.FEC_Examen);
                        //command.Parameters.AddWithValue("@USER_CREA", nuevoExamen.USER_CREA ?? (object)DBNull.Value);
                        //command.Parameters.AddWithValue("@USER_MOD", nuevoExamen.USER_MOD ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@TIPOEXAMEN", nuevoExamen.TIPOEXAMEN ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@NOMBRE_CLINICA_OPTM", nuevoExamen.NOMBRE_CLINICA_OPTM ?? (object)DBNull.Value);
                        //command.Parameters.AddWithValue("@TLF_TIPO", nuevoExamen.TLF_TIPO ?? (object)DBNull.Value);
                        //command.Parameters.AddWithValue("@TLF_COD", nuevoExamen.TLF_COD ?? (object)DBNull.Value);
                        //command.Parameters.AddWithValue("@TLF_NUMERO", nuevoExamen.TLF_NUMERO ?? (object)DBNull.Value);
                        //command.Parameters.AddWithValue("@TLF_EXT", nuevoExamen.TLF_EXT ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ESFD2", nuevoExamen.ESFD2);
                        command.Parameters.AddWithValue("@ESFI2", nuevoExamen.ESFI2);
                        command.Parameters.AddWithValue("@CILD2", nuevoExamen.CILD2);
                        command.Parameters.AddWithValue("@CILI2", nuevoExamen.CILI2);
                        command.Parameters.AddWithValue("@EJED2", nuevoExamen.EJED2);
                        command.Parameters.AddWithValue("@EJEI2", nuevoExamen.EJEI2);
                        command.Parameters.AddWithValue("@CodigoMimesys", nuevoExamen.CodigoMimesys ?? (object)DBNull.Value);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
            }
        }

        private TB_EXAMENCTE MapDataReaderToExamen(SqlDataReader reader)
        {
            return new TB_EXAMENCTE
            {
                CTE_Nacio = reader["CTE_Nacio"]?.ToString(),
                CTE_CedIden = reader["CTE_CedIden"]?.ToString(),
                COD_Sucursal = reader["COD_Sucursal"]?.ToString(),
                NUM_Examen = Convert.ToInt32(reader["NUM_Examen"]),
                FEC_Examen = Convert.ToDateTime(reader["FEC_Examen"]),
                ESFD = Convert.ToDecimal(reader["ESFD"]),
                ESFI = Convert.ToDecimal(reader["ESFI"]),
                CILD = Convert.ToDecimal(reader["CILD"]),
                CILI = Convert.ToDecimal(reader["CILI"]),
                EJED = Convert.ToDecimal(reader["EJED"]),
                EJEI = Convert.ToDecimal(reader["EJEI"]),
                ADDD = Convert.ToDecimal(reader["ADDD"]),
                ADDI = Convert.ToDecimal(reader["ADDI"]),
                OBSERVACIONES = reader["OBSERVACIONES"]?.ToString(),
                TIPO_Optm = reader["TIPO_Optm"]?.ToString(),
                NOM_Optm = reader["NOM_Optm"]?.ToString(),
                EXA_Feccreacion = reader["EXA_Feccreacion"] == DBNull.Value ? (DateTime)DateTime.MinValue : Convert.ToDateTime(reader["EXA_Feccreacion"]),
                EXA_Fecmod = reader["EXA_Fecmod"] == DBNull.Value ? (DateTime)DateTime.MinValue : Convert.ToDateTime(reader["EXA_Fecmod"]),
                USER_CREA = reader["USER_CREA"]?.ToString(),
                USER_MOD = reader["USER_MOD"]?.ToString(),
                TIPOEXAMEN = reader["TIPOEXAMEN"]?.ToString(),
                NOMBRE_CLINICA_OPTM = reader["NOMBRE_CLINICA_OPTM"]?.ToString(),
                TLF_TIPO = reader["TLF_TIPO"]?.ToString(),
                TLF_COD = reader["TLF_COD"]?.ToString(),
                TLF_NUMERO = reader["TLF_NUMERO"]?.ToString(),
                TLF_EXT = reader["TLF_EXT"]?.ToString(),
                ESFD2 = Convert.ToDecimal(reader["ESFD2"]),
                ESFI2 = Convert.ToDecimal(reader["ESFI2"]),
                CILD2 = Convert.ToDecimal(reader["CILD2"]),
                CILI2 = Convert.ToDecimal(reader["CILI2"]),
                EJED2 = Convert.ToDecimal(reader["EJED2"]),
                EJEI2 = Convert.ToDecimal(reader["EJEI2"]),
                CodigoMimesys = reader["CodigoMimesys"]?.ToString()







            };
        }
    }
}
