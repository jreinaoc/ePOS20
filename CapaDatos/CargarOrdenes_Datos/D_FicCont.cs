using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades;
using CapaDatos.Conexion;
using System.Data;

namespace CapaDatos.CargarOrdenes_Datos
{
    public class D_FicCont
    {
        private Conexion.Conexion cn = new Conexion.Conexion();
        private StringBuilder stringBuilder = new StringBuilder();

        public TB_FICCONT ObtenerFicCont(string nacionalidad, string cedula, int numeroExamen)
        {
            TB_FICCONT ficCont = null;
            stringBuilder.Clear();
            try
            {
                using (SqlConnection connection = cn.LeerCadena())
                {
                    //connection.Open();
                    string query = @"SELECT CTE_Nacio, CTE_CedIden, COD_Sucursal, NUM_Examen,  ISNULL(ESFD, 0) AS ESFD,
                                    ISNULL(ESFI, 0) AS ESFI,
                                    ISNULL(CILD, 0) AS CILD,
                                    ISNULL(CILI, 0) AS CILI,
                                    ISNULL(EJED, 0) AS EJED,
                                    ISNULL(EJEI, 0) AS EJEI,
                                    ISNULL(ADDD, 0) AS ADDD,
                                    ISNULL(ADDI, 0) AS ADDI,
                                    ISNULL(CBD, 0) AS CBD,
                                    ISNULL(CBI, 0) AS CBI,
                                    ISNULL(DIAMD, 0) AS DIAMD,
                                    ISNULL(DIAMI, 0) AS DIAMI,
                                    COLOR,
                                    ISNULL(AVD, 0) AS AVD,
                                    ISNULL(AVI, 0) AS AVI,
                                    OBSERVACIONES
                                    FROM [dbo].[TB_FICCONT]
                                    WHERE CTE_Nacio = @Nacionalidad AND CTE_CedIden = @Cedula AND NUM_Examen = @NumeroExamen";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nacionalidad", nacionalidad);
                        command.Parameters.AddWithValue("@Cedula", cedula);
                      
                        command.Parameters.AddWithValue("@NumeroExamen", numeroExamen);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ficCont = MapDataReaderToFicCont(reader);
                            }
                        }
                    }
                }
                return ficCont;
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error al obtener TB_FICCONT: {0}", ex.Message));
                return null;
            }
        }

        public int AgregarFicCont(TB_FICCONT nuevoFicCont)
        {
            stringBuilder.Clear();
            int filasAfectadas = 0;
            try
            {
                using (SqlConnection connection = cn.LeerCadena())
                {
                    //connection.Open();
                    using (SqlCommand command = new SqlCommand("SP_CPOSC_AgregarActualizarFicCont", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Agregar los parámetros al comando
                        command.Parameters.AddWithValue("@CTE_Nacio", nuevoFicCont.CTE_Nacio ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@CTE_CedIden", nuevoFicCont.CTE_CedIden ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@COD_Sucursal", nuevoFicCont.COD_Sucursal ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@NUM_Examen", nuevoFicCont.NUM_Examen);
                        command.Parameters.AddWithValue("@ESFD", nuevoFicCont.ESFD);
                        command.Parameters.AddWithValue("@ESFI", nuevoFicCont.ESFI);
                        command.Parameters.AddWithValue("@CILD", nuevoFicCont.CILD);
                        command.Parameters.AddWithValue("@CILI", nuevoFicCont.CILI);
                        command.Parameters.AddWithValue("@EJED", nuevoFicCont.EJED);
                        command.Parameters.AddWithValue("@EJEI", nuevoFicCont.EJEI);
                        command.Parameters.AddWithValue("@ADDD", nuevoFicCont.ADDD);
                        command.Parameters.AddWithValue("@ADDI", nuevoFicCont.ADDI);
                        command.Parameters.AddWithValue("@CBD", nuevoFicCont.CBD);
                        command.Parameters.AddWithValue("@CBI", nuevoFicCont.CBI);
                        command.Parameters.AddWithValue("@DIAMD", nuevoFicCont.DIAMD);
                        command.Parameters.AddWithValue("@DIAMI", nuevoFicCont.DIAMI);
                        //command.Parameters.AddWithValue("@CBD", nuevoFicCont.CBD.HasValue ? (object)nuevoFicCont.CBD.Value : DBNull.Value);
                        //command.Parameters.AddWithValue("@CBI", nuevoFicCont.CBI.HasValue ? (object)nuevoFicCont.CBI.Value : DBNull.Value);
                    
                        command.Parameters.AddWithValue("@AVD", nuevoFicCont.AVD);
                        command.Parameters.AddWithValue("@AVI", nuevoFicCont.AVI);
                        command.Parameters.AddWithValue("@Observaciones", nuevoFicCont.OBSERVACIONES ?? (object)DBNull.Value);

                        // Ejecutar el Stored Procedure y obtener el número de filas afectadas
                        filasAfectadas = command.ExecuteNonQuery();
                    }
                }
                return filasAfectadas;
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error al agregar TB_FICCONT: {0}", ex.Message));
                return 0;
            }
        }
        

        private TB_FICCONT MapDataReaderToFicCont(SqlDataReader reader)
        {
            return new TB_FICCONT
            {
                CTE_Nacio = reader["CTE_Nacio"]?.ToString(),
                CTE_CedIden = reader["CTE_CedIden"]?.ToString(),
                COD_Sucursal = reader["COD_Sucursal"]?.ToString(),
                NUM_Examen = Convert.ToInt32(reader["NUM_Examen"]),
                // Since ISNULL(ESFD, 0) is used in SQL, it will always be a decimal (0 if NULL in DB)
                ESFD = Convert.ToDecimal(reader["ESFD"]),
                ESFI = Convert.ToDecimal(reader["ESFI"]),
                CILD = Convert.ToDecimal(reader["CILD"]),
                CILI = Convert.ToDecimal(reader["CILI"]),
                EJED = Convert.ToDecimal(reader["EJED"]),
                EJEI = Convert.ToDecimal(reader["EJEI"]),
                ADDD = Convert.ToDecimal(reader["ADDD"]),
                ADDI = Convert.ToDecimal(reader["ADDI"]),
                CBD = Convert.ToDecimal(reader["CBD"]),
                CBI = Convert.ToDecimal(reader["CBI"]),
                DIAMD = Convert.ToDecimal(reader["DIAMD"]),
                DIAMI = Convert.ToDecimal(reader["DIAMI"]),
                AVD = Convert.ToDecimal(reader["AVD"]),
                AVI = Convert.ToDecimal(reader["AVI"]),
                COLOR = reader["COLOR"]?.ToString(),
                OBSERVACIONES = reader["OBSERVACIONES"]?.ToString()
            };
        }
    }
}
