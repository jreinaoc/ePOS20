using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.GiftCard_Datos
{
    public class D_GiftCard
    {
        Conexion.Conexion cn = new Conexion.Conexion();

        public bool AgregarGiftCard(string codSucursal, string nroOrden, string revision, decimal montoDolares, string nombreBeneficiario, string correoBeneficiario, string mensaje, int idGiftCard , string codigoGiftCard, string userCrea, string userMod, SqlCommand command1 = null)
        {
            //stringBuilder.Clear();

            try
            {
                if (command1 == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command1 = connection.CreateCommand();
                }

                SqlCommand command = command1;
                command.Parameters.Clear();
                command.CommandText = "SP_CPOS_AddOrdenGiftfCard";
                command.CommandType = CommandType.StoredProcedure;

                // Agregar los parámetros individuales al comando
                command.Parameters.AddWithValue("@CodSucursal", codSucursal ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@NroOrden", nroOrden ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Revision", string.IsNullOrEmpty(revision) ? "0" : revision);
                command.Parameters.AddWithValue("@MontoDolares", montoDolares);
                command.Parameters.AddWithValue("@NombreBeneficiario", nombreBeneficiario ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CorreoBeneficiario", correoBeneficiario ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Mensaje", mensaje ?? (object)DBNull.Value); 
                command.Parameters.AddWithValue("@idGiftCard", idGiftCard);
                command.Parameters.AddWithValue("@CodigoGiftCard", codigoGiftCard ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@UserCrea", userCrea ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@UserMod", userMod ?? (object)DBNull.Value);

                // Ejecutar el Stored Procedure
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataSet dts = new DataSet();
                da.Fill(dts);

                command.Parameters.Clear();

                return true;
            }
            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                EscribirLog(ex.Message.ToString());
                return false;
            }
        }
        public DataTable ObtenerGiftCard(string codSucursal, string nroOrden, string revision, string CodigoGiftCard, SqlCommand command1 = null)
        {
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
                command.Parameters.AddWithValue("@CodigoGiftCard", CodigoGiftCard ?? (object)DBNull.Value);

                // 🌟 Cambiamos a DataTable
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(command);

                // El SqlDataAdapter llena directamente el DataTable
                da.Fill(dt);

                command.Parameters.Clear();

                return dt;
            }
            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                EscribirLog(ex.Message.ToString());
                return null;
            }
        }

        public static void EscribirLog(string mensaje)
        {
            string ruta = "log.txt";
            string entrada = $"[{DateTime.Now}] {mensaje}";
            System.IO.File.AppendAllText(ruta, entrada + Environment.NewLine);
        }
    }
}
