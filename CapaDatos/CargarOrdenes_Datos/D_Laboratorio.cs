using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using CapaEntidades;
using CapaDatos.Conexion;

namespace CapaDatos
{
    public class D_Laboratorio
    {
        private readonly Conexion.Conexion _conexion = new Conexion.Conexion();
        public readonly StringBuilder stringBuilder = new StringBuilder();

        public List<TB_LABORATORIOS> ObtenerTodosLosLaboratorios()
        {
            List<TB_LABORATORIOS> laboratorios = new List<TB_LABORATORIOS>();
            stringBuilder.Clear();
            try
            {
                using (SqlConnection connection = _conexion.LeerCadena())
                {
                 //mcll   connection.Open();
                    string query = "SELECT CODIGO_LAB, DESCRIPCION, DIRECCION, TELEFONO, FAX, CONTACTO, ST_LABORATORIO, FEC_CREA, FEC_MOD, USER_CREA, USER_MOD FROM dbo.TB_LABORATORIOS";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                laboratorios.Add(MapDataReaderToLaboratorio(reader));
                            }
                        }
                    }
                }
                return laboratorios;
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
            }
        }

        public TB_LABORATORIOS ObtenerLaboratorioPorCodigo(string codigoLaboratorio)
        {
            TB_LABORATORIOS laboratorio = null;
            stringBuilder.Clear();
            try
            {
                using (SqlConnection connection = _conexion.LeerCadena())
                {
                    connection.Open();
                    string query = "SELECT CODIGO_LAB, DESCRIPCION, DIRECCION, TELEFONO, FAX, CONTACTO, ST_LABORATORIO, FEC_CREA, FEC_MOD, USER_CREA, USER_MOD FROM dbo.TB_LABORATORIOS WHERE CODIGO_LAB = @CodigoLab";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CodigoLab", codigoLaboratorio);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                laboratorio = MapDataReaderToLaboratorio(reader);
                            }
                        }
                    }
                }
                return laboratorio;
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
            }
        }

        public void AgregarLaboratorio(TB_LABORATORIOS nuevoLaboratorio)
        {
            stringBuilder.Clear();
            try
            {
                using (SqlConnection connection = _conexion.LeerCadena())
                {
                    connection.Open();
                    string query = @"INSERT INTO dbo.TB_LABORATORIOS (CODIGO_LAB, DESCRIPCION, DIRECCION, TELEFONO, FAX, CONTACTO, ST_LABORATORIO, FEC_CREA, USER_CREA)
                                   VALUES (@CodigoLab, @Descripcion, @Direccion, @Telefono, @Fax, @Contacto, @StLaboratorio, @FecCrea, @UserCrea)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CodigoLab", nuevoLaboratorio.CODIGO_LAB);
                        command.Parameters.AddWithValue("@Descripcion", nuevoLaboratorio.DESCRIPCION ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Direccion", nuevoLaboratorio.DIRECCION ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Telefono", nuevoLaboratorio.TELEFONO ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Fax", nuevoLaboratorio.FAX ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Contacto", nuevoLaboratorio.CONTACTO ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@StLaboratorio", nuevoLaboratorio.ST_LABORATORIO ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@FecCrea", nuevoLaboratorio.FEC_CREA.HasValue ? (object)nuevoLaboratorio.FEC_CREA.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@UserCrea", nuevoLaboratorio.USER_CREA ?? (object)DBNull.Value);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error al agregar laboratorio: {0}", ex.Message));
            }
        }

        public void ActualizarLaboratorio(TB_LABORATORIOS laboratorioActualizado)
        {
            stringBuilder.Clear();
            try
            {
                using (SqlConnection connection = _conexion.LeerCadena())
                {
                    connection.Open();
                    string query = @"UPDATE dbo.TB_LABORATORIOS
                                   SET DESCRIPCION = @Descripcion,
                                       DIRECCION = @Direccion,
                                       TELEFONO = @Telefono,
                                       FAX = @Fax,
                                       CONTACTO = @Contacto,
                                       ST_LABORATORIO = @StLaboratorio,
                                       FEC_MOD = @FecMod,
                                       USER_MOD = @UserMod
                                   WHERE CODIGO_LAB = @CodigoLab";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CodigoLab", laboratorioActualizado.CODIGO_LAB);
                        command.Parameters.AddWithValue("@Descripcion", laboratorioActualizado.DESCRIPCION ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Direccion", laboratorioActualizado.DIRECCION ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Telefono", laboratorioActualizado.TELEFONO ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Fax", laboratorioActualizado.FAX ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Contacto", laboratorioActualizado.CONTACTO ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@StLaboratorio", laboratorioActualizado.ST_LABORATORIO ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@FecMod", laboratorioActualizado.FEC_MOD.HasValue ? (object)laboratorioActualizado.FEC_MOD.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@UserMod", laboratorioActualizado.USER_MOD ?? (object)DBNull.Value);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error al actualizar laboratorio: {0}", ex.Message));
            }
        }

        public void EliminarLaboratorio(string codigoLaboratorio)
        {
            stringBuilder.Clear();
            try
            {
                using (SqlConnection connection = _conexion.LeerCadena())
                {
                    connection.Open();
                    string query = "DELETE FROM dbo.TB_LABORATORIOS WHERE CODIGO_LAB = @CodigoLab";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CodigoLab", codigoLaboratorio);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error al eliminar laboratorio: {0}", ex.Message));
            }
        }

        private TB_LABORATORIOS MapDataReaderToLaboratorio(SqlDataReader reader)
        {
            return new TB_LABORATORIOS
            {
                CODIGO_LAB = reader["CODIGO_LAB"] == DBNull.Value ? null : reader["CODIGO_LAB"].ToString(),
                DESCRIPCION = reader["DESCRIPCION"] == DBNull.Value ? null : reader["DESCRIPCION"]?.ToString(),
                DIRECCION = reader["DIRECCION"] == DBNull.Value ? null : reader["DIRECCION"]?.ToString(),
                TELEFONO = reader["TELEFONO"] == DBNull.Value ? null : reader["TELEFONO"]?.ToString(),
                FAX = reader["FAX"] == DBNull.Value ? null : reader["FAX"]?.ToString(),
                CONTACTO = reader["CONTACTO"] == DBNull.Value ? null : reader["CONTACTO"]?.ToString(),
                ST_LABORATORIO = reader["ST_LABORATORIO"] == DBNull.Value ? null : reader["ST_LABORATORIO"]?.ToString(),
                FEC_CREA = reader["FEC_CREA"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["FEC_CREA"],
                FEC_MOD = reader["FEC_MOD"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["FEC_MOD"],
                USER_CREA = reader["USER_CREA"] == DBNull.Value ? null : reader["USER_CREA"]?.ToString(),
                USER_MOD = reader["USER_MOD"] == DBNull.Value ? null : reader["USER_MOD"]?.ToString()
            };
        }
    }
}