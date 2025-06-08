using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades; // Asegúrate de que esta referencia sea correcta
using CapaDatos.Conexion; // Asegúrate de que esta referencia sea correcta
using System.Data;

namespace CapaDatos.CargarOrdenes_Datos
{
    public class D_Ficconv
    {
        private Conexion.Conexion cn = new Conexion.Conexion();
        private StringBuilder stringBuilder = new StringBuilder();

        // Método para obtener un registro de TB_ficconv por clave primaria
        public TB_FICCONVCTE ObtenerFicConv(string cteNacio, string cteCedIden,  int numExamen)
        {
            TB_FICCONVCTE ficConv = null;
            stringBuilder.Clear();

            try
            {
                using (SqlConnection connection = cn.LeerCadena())
                {
                    //connection.Open();
                    string query = @"SELECT
                                        CTE_Nacio,
                                        CTE_CedIden,
                                        COD_Sucursal,
                                        NUM_Examen,
                                        ISNULL(DPDL, 0) AS DPDL,
                                        ISNULL(DPDC, 0) AS DPDC,
                                        ISNULL(DPIL, 0) AS DPIL,
                                        ISNULL(DPIC, 0) AS DPIC,
                                        ISNULL(ALTD, 0) AS ALTD,
                                        ISNULL(ALTI, 0) AS ALTI,
                                        ISNULL(PRISMAD, 0) AS PRISMAD,
                                        ISNULL(PRISMAI, 0) AS PRISMAI,
                                        ISNULL(PBASED, 0) AS PBASED,
                                        ISNULL(PBASEI, 0) AS PBASEI,
                                          OFTI,
                                          OFTD,
                                        ISNULL(AVD, 0) AS AVD,
                                        ISNULL(AVI, 0) AS AVI,
                                         RETI,
                                          RETD,
                                        ISNULL(PRISMAD2, 0) AS PRISMAD2,
                                        ISNULL(PRISMAI2, 0) AS PRISMAI2,
                                        ISNULL(PBASED2, 0) AS PBASED2,
                                        ISNULL(PBASEI2, 0) AS PBASEI2,
                                        ISNULL(PROGVISIONLEJOSDISTD, 0) AS PROGVISIONLEJOSDISTD,
                                        ISNULL(PROGVISIONLEJOSDISTI, 0) AS PROGVISIONLEJOSDISTI,
                                        ISNULL(PROGVISIONCERCADISTD, 0) AS PROGVISIONCERCADISTD,
                                        ISNULL(PROGVISIONCERCADISTI, 0) AS PROGVISIONCERCADISTI,
                                        ISNULL(PROGVISIONMEDIADISTD, 0) AS PROGVISIONMEDIADISTD,
                                        ISNULL(PROGVISIONMEDIADISTI, 0) AS PROGVISIONMEDIADISTI
                                    FROM [dbo].[TB_ficconv]
                                    WHERE CTE_Nacio = @CteNacio
                                      AND CTE_CedIden = @CteCedIden
                                      AND NUM_Examen = @NumExamen;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CteNacio", cteNacio);
                        command.Parameters.AddWithValue("@CteCedIden", cteCedIden);
                       // command.Parameters.AddWithValue("@CodSucursal", codSucursal);AND COD_Sucursal = @CodSucursal
                        command.Parameters.AddWithValue("@NumExamen", numExamen);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ficConv = MapDataReaderToFicConv(reader);
                            }
                        }
                    }
                }
                return ficConv;
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error al obtener TB_ficconv: {0}", ex.Message));
                // Considerar lanzar la excepción nuevamente o usar un mecanismo de registro más robusto.
                return null; // o throw;
            }
        }

        // Método para agregar un nuevo registro a TB_ficconv
        public void AgregarFicConv(TB_FICCONVCTE nuevoFicConv)
        {
            stringBuilder.Clear();
            string filasAfectadas = "";
            try
            {
                using (SqlConnection connection = cn.LeerCadena())
                {
                    //connection.Open();
                    using (SqlCommand command = new SqlCommand("SP_CPOSC_AgregarActualizarFicConv", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Agregar los parámetros al comando
                        command.Parameters.AddWithValue("@CTE_Nacio", nuevoFicConv.CTE_Nacio);
                        command.Parameters.AddWithValue("@CTE_CedIden", nuevoFicConv.CTE_CedIden);
                        command.Parameters.AddWithValue("@COD_Sucursal", nuevoFicConv.COD_Sucursal);
                        command.Parameters.AddWithValue("@NUM_Examen", nuevoFicConv.NUM_Examen);
                        command.Parameters.AddWithValue("@DPDL", nuevoFicConv.DPDL.HasValue ? (object)nuevoFicConv.DPDL.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@DPDC", nuevoFicConv.DPDC.HasValue ? (object)nuevoFicConv.DPDC.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@DPIL", nuevoFicConv.DPIL.HasValue ? (object)nuevoFicConv.DPIL.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@DPIC", nuevoFicConv.DPIC.HasValue ? (object)nuevoFicConv.DPIC.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@ALTD", nuevoFicConv.ALTD.HasValue ? (object)nuevoFicConv.ALTD.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@ALTI", nuevoFicConv.ALTI.HasValue ? (object)nuevoFicConv.ALTI.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@PRISMAD", nuevoFicConv.PRISMAD.HasValue ? (object)nuevoFicConv.PRISMAD.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@PRISMAI", nuevoFicConv.PRISMAI.HasValue ? (object)nuevoFicConv.PRISMAI.Value : DBNull.Value);

                        //command.Parameters.AddWithValue("@Observaciones", nuevoFicConv.OBSERVACIONES ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@OFTI", nuevoFicConv.OFTI ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@OFTD", nuevoFicConv.OFTD ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@AVD", nuevoFicConv.AVD.HasValue ? (object)nuevoFicConv.AVD.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@AVI", nuevoFicConv.AVI.HasValue ? (object)nuevoFicConv.AVI.Value : DBNull.Value);

                        command.Parameters.AddWithValue("@RETI", nuevoFicConv.RETI ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@RETD", nuevoFicConv.RETD ?? (object)DBNull.Value);

                        command.Parameters.AddWithValue("@PBASED", nuevoFicConv.PBASED.HasValue ? (object)nuevoFicConv.PBASED.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@PBASEI", nuevoFicConv.PBASEI.HasValue ? (object)nuevoFicConv.PBASED.Value : DBNull.Value);
                      
                        command.Parameters.AddWithValue("@PROGVISIONLEJOSDISTD", nuevoFicConv.PROGVISIONLEJOSDISTD.HasValue ? (object)nuevoFicConv.PROGVISIONLEJOSDISTD.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@PROGVISIONLEJOSDISTI", nuevoFicConv.PROGVISIONLEJOSDISTI.HasValue ? (object)nuevoFicConv.PROGVISIONLEJOSDISTI.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@PROGVISIONCERCADISTD", nuevoFicConv.PROGVISIONCERCADISTD.HasValue ? (object)nuevoFicConv.PROGVISIONCERCADISTD.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@PROGVISIONCERCADISTI", nuevoFicConv.PROGVISIONCERCADISTI.HasValue ? (object)nuevoFicConv.PROGVISIONCERCADISTI.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@PROGVISIONMEDIADISTD", nuevoFicConv.PROGVISIONMEDIADISTD.HasValue ? (object)nuevoFicConv.PROGVISIONMEDIADISTD.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@PROGVISIONMEDIADISTI", nuevoFicConv.PROGVISIONMEDIADISTI.HasValue ? (object)nuevoFicConv.PROGVISIONMEDIADISTI.Value : DBNull.Value);

                        // Ejecutar el Stored Procedure y obtener el número de filas afectadas
                        //filasAfectadas = command.ExecuteNonQuery();
                        command.ExecuteNonQuery();
                    }
                }
                //return "Guardado";
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error al agregar TB_ficconv: {0}", ex.Message));
                //return 0;
            }
        }


        // Método para mapear los datos del SqlDataReader a un objeto TB_ficconv
        private TB_FICCONVCTE MapDataReaderToFicConv(SqlDataReader reader)
        {
            return new TB_FICCONVCTE
            {
                CTE_Nacio = reader["CTE_Nacio"]?.ToString(),
                CTE_CedIden = reader["CTE_CedIden"]?.ToString(),
                COD_Sucursal = reader["COD_Sucursal"]?.ToString(),
                NUM_Examen = Convert.ToInt32(reader["NUM_Examen"]),
                DPDL = reader["DPDL"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["DPDL"]) : null,
                DPDC = reader["DPDC"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["DPDC"]) : null,
                DPIL = reader["DPIL"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["DPIL"]) : null,
                DPIC = reader["DPIC"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["DPIC"]) : null,
                ALTD = reader["ALTD"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["ALTD"]) : null,
                ALTI = reader["ALTI"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["ALTI"]) : null,
                PRISMAD = reader["PRISMAD"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["PRISMAD"]) : null,
                PRISMAI = reader["PRISMAI"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["PRISMAI"]) : null,
                PBASED = reader["PBASED"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["PBASED"]) : null,
                PBASEI = reader["PBASEI"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["PBASEI"]) : null,

                OFTI = reader["OFTI"]?.ToString(),
                OFTD = reader["OFTD"]?.ToString(),
                // ... otras propiedades de tu objeto ...
                AVD = reader["AVD"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["AVD"]) : null,
                AVI = reader["AVI"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["AVI"]) : null,
                // ... otras propiedades de tu objeto ...
                RETI = reader["RETI"]?.ToString(),
                RETD = reader["RETD"]?.ToString(),
                //PRISMAD2 = reader["PRISMAD2"] != DBNull.Value ? (float?)Convert.ToDecimal(reader["PRISMAD2"]) : null,
                //PRISMAI2 = reader["PRISMAI2"] != DBNull.Value ? (float?)Convert.ToDecimal(reader["PRISMAI2"]) : null,
                //PBASED2 = reader["PBASED2"]?.ToString(),
                PBASEI2 = reader["PBASEI2"]?.ToString(),
                PROGVISIONLEJOSDISTD = reader["PROGVISIONLEJOSDISTD"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["PROGVISIONLEJOSDISTD"]) : null,
                PROGVISIONLEJOSDISTI = reader["PROGVISIONLEJOSDISTI"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["PROGVISIONLEJOSDISTI"]) : null,
                PROGVISIONCERCADISTD = reader["PROGVISIONCERCADISTD"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["PROGVISIONCERCADISTD"]) : null,
                PROGVISIONCERCADISTI = reader["PROGVISIONCERCADISTI"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["PROGVISIONCERCADISTI"]) : null,
                PROGVISIONMEDIADISTD = reader["PROGVISIONMEDIADISTD"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["PROGVISIONMEDIADISTD"]) : null,
                PROGVISIONMEDIADISTI = reader["PROGVISIONMEDIADISTI"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["PROGVISIONMEDIADISTI"]) : null,
            };
        }
    }
}
