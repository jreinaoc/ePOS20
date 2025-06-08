using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using CapaEntidades;
using CapaDatos.Conexion;

namespace CapaDatos
{


        public class D_ServicioLab
        {
            private readonly Conexion.Conexion _conexion = new Conexion.Conexion();
            public readonly StringBuilder stringBuilder = new StringBuilder();

            public List<TB_SERVICIOSLAB> ObtenerTodosLosServicios()
            {
                List<TB_SERVICIOSLAB> servicios = new List<TB_SERVICIOSLAB>();
                stringBuilder.Clear();
                try
                {

                    using (SqlConnection connection = _conexion.LeerCadena())
                    {
                    // Verificar si la conexión no está abierta antes de intentar abrirla
                    if (connection.State != System.Data.ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    string query = "SELECT top 1 Cod_servicio, Descripcion_servicio  FROM dbo.TB_SERVICIOSLAB";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    servicios.Add(MapDataReaderToServicio(reader));
                                }
                            }
                        }
                    }
                    return servicios;
                }
                catch (Exception ex)
                {
                    stringBuilder.Append(Environment.NewLine + string.Format("Error al obtener servicios: {0}", ex.Message));
                    return null;
                 }
            }

            public TB_SERVICIOSLAB ObtenerServicioPorCodigo(string codigoServicio)
            {
                TB_SERVICIOSLAB servicio = null;
                stringBuilder.Clear();
                try
                {
                    using (SqlConnection connection = _conexion.LeerCadena())
                    {
                       // connection.Open();
                        string query = "SELECT Cod_servicio, Descripcion_servicio, CodArticulo, Status_Servicio, Fec_Crea, Fec_Mod, User_Crea, User_Mod, HorasEntrega, EnvioApi FROM dbo.TB_SERVICIOSLAB WHERE Cod_servicio = @CodServicio";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@CodServicio", codigoServicio);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    servicio = MapDataReaderToServicio(reader);
                                }
                            }
                        }
                    }
                    return servicio;
                }
                catch (Exception ex)
                {
                    stringBuilder.Append(Environment.NewLine + string.Format("Error al obtener servicio por código: {0}", ex.Message));
                    return null;
                }
            }

            public void AgregarServicio(TB_SERVICIOSLAB nuevoServicio)
            {
                stringBuilder.Clear();
                try
                {
                    using (SqlConnection connection = _conexion.LeerCadena())
                    {
                      //  connection.Open();
                        string query = @"INSERT INTO dbo.TB_SERVICIOSLAB (Cod_servicio, Descripcion_servicio, CodArticulo, Status_Servicio, Fec_Crea, User_Crea, HorasEntrega, EnvioApi)
                                   VALUES (@CodServicio, @DescripcionServicio, @CodArticulo, @StatusServicio, @FecCrea, @UserCrea, @HorasEntrega, @EnvioApi)";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@CodServicio", nuevoServicio.Cod_servicio);
                            command.Parameters.AddWithValue("@DescripcionServicio", nuevoServicio.Descripcion_servicio ?? (object)DBNull.Value);
                            //command.Parameters.AddWithValue("@CodArticulo", nuevoServicio.CodArticulo ?? (object)DBNull.Value);
                            //command.Parameters.AddWithValue("@StatusServicio", nuevoServicio.Status_Servicio ?? (object)DBNull.Value);
                            //command.Parameters.AddWithValue("@FecCrea", nuevoServicio.Fec_Crea.HasValue ? (object)nuevoServicio.Fec_Crea.Value : DBNull.Value);
                            //command.Parameters.AddWithValue("@UserCrea", nuevoServicio.User_Crea ?? (object)DBNull.Value);
                            //command.Parameters.AddWithValue("@HorasEntrega", nuevoServicio.HorasEntrega.HasValue ? (object)nuevoServicio.HorasEntrega.Value : DBNull.Value);
                            //command.Parameters.AddWithValue("@EnvioApi", nuevoServicio.EnvioApi.HasValue ? (object)nuevoServicio.EnvioApi.Value : DBNull.Value);

                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    stringBuilder.Append(Environment.NewLine + string.Format("Error al agregar servicio: {0}", ex.Message));
                }
            }

            public void ActualizarServicio(TB_SERVICIOSLAB servicioActualizado)
            {
                stringBuilder.Clear();
                try
                {
                    using (SqlConnection connection = _conexion.LeerCadena())
                    {
                        //connection.Open();
                        string query = @"UPDATE dbo.TB_SERVICIOSLAB
                                   SET Descripcion_servicio = @DescripcionServicio,
                                       CodArticulo = @CodArticulo,
                                       Status_Servicio = @StatusServicio,
                                       Fec_Mod = @FecMod,
                                       User_Mod = @UserMod,
                                       HorasEntrega = @HorasEntrega,
                                       EnvioApi = @EnvioApi
                                   WHERE Cod_servicio = @CodServicio";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@CodServicio", servicioActualizado.Cod_servicio);
                            command.Parameters.AddWithValue("@DescripcionServicio", servicioActualizado.Descripcion_servicio ?? (object)DBNull.Value);
                            //command.Parameters.AddWithValue("@CodArticulo", servicioActualizado.CodArticulo ?? (object)DBNull.Value);
                            //command.Parameters.AddWithValue("@StatusServicio", servicioActualizado.Status_Servicio ?? (object)DBNull.Value);
                            //command.Parameters.AddWithValue("@FecMod", servicioActualizado.Fec_Mod.HasValue ? (object)servicioActualizado.Fec_Mod.Value : DBNull.Value);
                            //command.Parameters.AddWithValue("@UserMod", servicioActualizado.User_Mod ?? (object)DBNull.Value);
                            //command.Parameters.AddWithValue("@HorasEntrega", servicioActualizado.HorasEntrega.HasValue ? (object)servicioActualizado.HorasEntrega.Value : DBNull.Value);
                            //command.Parameters.AddWithValue("@EnvioApi", servicioActualizado.EnvioApi.HasValue ? (object)servicioActualizado.EnvioApi.Value : DBNull.Value);

                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    stringBuilder.Append(Environment.NewLine + string.Format("Error al actualizar servicio: {0}", ex.Message));
                }
            }

            public void EliminarServicio(string codigoServicio)
            {
                stringBuilder.Clear();
                try
                {
                    using (SqlConnection connection = _conexion.LeerCadena())
                    {
                       // connection.Open();
                        string query = "DELETE FROM dbo.TB_SERVICIOSLAB WHERE Cod_servicio = @CodServicio";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@CodServicio", codigoServicio);
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    stringBuilder.Append(Environment.NewLine + string.Format("Error al eliminar servicio: {0}", ex.Message));
                }
            }

            private TB_SERVICIOSLAB MapDataReaderToServicio(SqlDataReader reader)
            {
                return new TB_SERVICIOSLAB
                {
                    Cod_servicio = reader["Cod_servicio"] == DBNull.Value ? null : reader["Cod_servicio"].ToString(),
                    Descripcion_servicio = reader["Descripcion_servicio"] == DBNull.Value ? null : reader["Descripcion_servicio"]?.ToString(),
                    //CodArticulo = reader["CodArticulo"] == DBNull.Value ? null : reader["CodArticulo"]?.ToString(),
                    //Status_Servicio = reader["Status_Servicio"] == DBNull.Value ? null : reader["Status_Servicio"]?.ToString(),
                    //Fec_Crea = reader["Fec_Crea"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["Fec_Crea"],
                    //Fec_Mod = reader["Fec_Mod"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["Fec_Mod"],
                    //User_Crea = reader["User_Crea"] == DBNull.Value ? null : reader["User_Crea"]?.ToString(),
                    //User_Mod = reader["User_Mod"] == DBNull.Value ? null : reader["User_Mod"]?.ToString(),
                    //HorasEntrega = reader["HorasEntrega"] == DBNull.Value ? (int?)null : (int)reader["HorasEntrega"],
                    //EnvioApi = reader["EnvioApi"] == DBNull.Value ? (bool?)null : (bool)reader["EnvioApi"]
                };
            }
        }



}
