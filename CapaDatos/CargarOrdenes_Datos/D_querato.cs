using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades; // Asegúrate de que TB_QUERATO esté en este namespace
using CapaDatos.Conexion;
using System.Data;

namespace CapaDatos.CargarOrdenes_Datos
{
    public class D_Querato
    {
        private Conexion.Conexion cn = new Conexion.Conexion();
        private StringBuilder stringBuilder = new StringBuilder();

        public TB_QUERATO ObtenerQuerato(string nacionalidad, string cedula,  int numeroExamen)
        {
            TB_QUERATO querato = null;
            stringBuilder.Clear();
            try
            {
                using (SqlConnection connection = cn.LeerCadena())
                {
                   // connection.Open();
                    string query = @"SELECT CTE_Nacio, CTE_CedIden, COD_Sucursal, NUM_Examen, QUERATOMD1, QUERATOGD1, QUERATOMD2, QUERATOGD2, QUERATOMI1, QUERATOGI1, QUERATOMI2, QUERATOGI2, QUE_OBSERV
                                    FROM [dbo].[TB_QUERATO]
                                    WHERE CTE_Nacio = @Nacionalidad AND CTE_CedIden = @Cedula  AND NUM_Examen = @NumeroExamen";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nacionalidad", nacionalidad);
                        command.Parameters.AddWithValue("@Cedula", cedula);
                        //command.Parameters.AddWithValue("@CodSucursal", codSucursal);string codSucursal,AND COD_Sucursal = @CodSucursal
                        command.Parameters.AddWithValue("@NumeroExamen", numeroExamen);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                querato = MapDataReaderToQuerato(reader);
                            }
                        }
                    }
                }
                return querato;
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error al obtener TB_QUERATO: {0}", ex.Message));
                // Considerar loguear el error en un archivo o base de datos.
                return null;
            }
        }

        public int AgregarQuerato(TB_QUERATO nuevoQuerato)
        {
            stringBuilder.Clear();
            int filasAfectadas = 0;
            try
            {
                using (SqlConnection connection = cn.LeerCadena())
                {
                   /// connection.Open();
                    using (SqlCommand command = new SqlCommand("SP_CPOSC_AgregarActualizarQuerato", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Agregar los parámetros al comando
                        command.Parameters.AddWithValue("@CTE_Nacio", nuevoQuerato.CTE_Nacio ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@CTE_CedIden", nuevoQuerato.CTE_CedIden ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@COD_Sucursal", nuevoQuerato.COD_Sucursal ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@NUM_Examen", nuevoQuerato.NUM_Examen);
                        command.Parameters.AddWithValue("@QUERATOMD1", nuevoQuerato.QUERATOMD1.HasValue ? (object)nuevoQuerato.QUERATOMD1.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@QUERATOGD1", nuevoQuerato.QUERATOGD1.HasValue ? (object)nuevoQuerato.QUERATOGD1.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@QUERATOMD2", nuevoQuerato.QUERATOMD2.HasValue ? (object)nuevoQuerato.QUERATOMD2.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@QUERATOGD2", nuevoQuerato.QUERATOGD2.HasValue ? (object)nuevoQuerato.QUERATOGD2.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@QUERATOMI1", nuevoQuerato.QUERATOMI1.HasValue ? (object)nuevoQuerato.QUERATOMI1.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@QUERATOGI1", nuevoQuerato.QUERATOGI1.HasValue ? (object)nuevoQuerato.QUERATOGI1.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@QUERATOMI2", nuevoQuerato.QUERATOMI2.HasValue ? (object)nuevoQuerato.QUERATOMI2.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@QUERATOGI2", nuevoQuerato.QUERATOGI2.HasValue ? (object)nuevoQuerato.QUERATOGI2.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@QueObserv", nuevoQuerato.QUE_OBSERV ?? (object)DBNull.Value);

                        // Ejecutar el Stored Procedure y obtener el número de filas afectadas
                        filasAfectadas = command.ExecuteNonQuery();
                    }
                }
                return filasAfectadas;
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error al agregar TB_QUERATO: {0}", ex.Message));
                return 0;
            }
        }


        private TB_QUERATO MapDataReaderToQuerato(SqlDataReader reader)
        {
            return new TB_QUERATO
            {
                CTE_Nacio = reader["CTE_Nacio"]?.ToString(),
                CTE_CedIden = reader["CTE_CedIden"]?.ToString(),
                COD_Sucursal = reader["COD_Sucursal"]?.ToString(),
                NUM_Examen = Convert.ToInt32(reader["NUM_Examen"]),
                QUERATOMD1 = reader["QUERATOMD1"] != DBNull.Value ? Convert.ToDecimal(reader["QUERATOMD1"]) : (decimal?)null,
                QUERATOGD1 = reader["QUERATOGD1"] != DBNull.Value ? Convert.ToDecimal(reader["QUERATOGD1"]) : (decimal?)null,
                QUERATOMD2 = reader["QUERATOMD2"] != DBNull.Value ? Convert.ToDecimal(reader["QUERATOMD2"]) : (decimal?)null,
                QUERATOGD2 = reader["QUERATOGD2"] != DBNull.Value ? Convert.ToDecimal(reader["QUERATOGD2"]) : (decimal?)null,
                QUERATOMI1 = reader["QUERATOMI1"] != DBNull.Value ? Convert.ToDecimal(reader["QUERATOMI1"]) : (decimal?)null,
                QUERATOGI1 = reader["QUERATOGI1"] != DBNull.Value ? Convert.ToDecimal(reader["QUERATOGI1"]) : (decimal?)null,
                QUERATOMI2 = reader["QUERATOMI2"] != DBNull.Value ? Convert.ToDecimal(reader["QUERATOMI2"]) : (decimal?)null,
                QUERATOGI2 = reader["QUERATOGI2"] != DBNull.Value ? Convert.ToDecimal(reader["QUERATOGI2"]) : (decimal?)null,
                QUE_OBSERV = reader["QUE_OBSERV"]?.ToString()
            };
        }
    }
}

