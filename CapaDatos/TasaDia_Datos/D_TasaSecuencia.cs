using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.TasaDia_Datos
{
    public class D_TasaSecuencia
    {
        Conexion.Conexion cn = new Conexion.Conexion();

        public DataTable AgregarTasaDia(string codSucursal, decimal tasa, DateTime fechaDiaActivo, string codMoneda, char userCrea , SqlCommand command = null)
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

                cmd.CommandText = "pAdd_TasaDia";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Cod_Sucursal", codSucursal);
                cmd.Parameters.AddWithValue("@Tasa", tasa);
                cmd.Parameters.AddWithValue("@fechaDiaActivo", fechaDiaActivo);
                cmd.Parameters.AddWithValue("@Cod_Moneda", codMoneda);
                cmd.Parameters.AddWithValue("@USER_Crea", userCrea);


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

        public DataTable EncripDescrip(string palabra, string accion, SqlCommand command = null)
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

                cmd.CommandText = "SP_EncripDecrip";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Palabra", palabra);
                cmd.Parameters.AddWithValue("@Accion", accion);

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

        public DataTable ComparaDigitoVerificador(string tasa, char digitoVerificador, SqlCommand command = null)
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

                cmd.CommandText = "pComparaDigitoVerificador";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Tasa", tasa);
                cmd.Parameters.AddWithValue("@DigitoVerificador", digitoVerificador);

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

        public DataTable ActualizarArtDolar(string codSucursal, string usuario, string secuencia, string tasa, DateTime fechaSec,
                                            string fechaSecA, string fechaPrincipal, SqlCommand command = null)
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

                cmd.CommandText = "SP_ACTUALIZAARTICULO_DOLAR";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodSucursal", codSucursal);
                cmd.Parameters.AddWithValue("@Usuario", usuario);
                cmd.Parameters.AddWithValue("@Secuencia", secuencia);
                cmd.Parameters.AddWithValue("@Tasa", tasa);
                cmd.Parameters.AddWithValue("@FechaSec", fechaSec);
                cmd.Parameters.AddWithValue("@FechaSecA", fechaSecA);
                cmd.Parameters.AddWithValue("@FechaPrincipal", fechaPrincipal);

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
