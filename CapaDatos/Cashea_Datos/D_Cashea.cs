using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Cashea_Datos
{
    public class D_Cashea
    {
        Conexion.Conexion cn = new Conexion.Conexion();
        public DataSet RegistrarCuotasCashea(string codSucursal,string nroOren, string nroContrato, string nroCuota, decimal montoCuota,string userCrea, SqlCommand command = null)
        {
            try
            {
                Conexion.Conexion cn = new Conexion.Conexion();
                if (command == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command = connection.CreateCommand();
                }
                SqlCommand cmd = command;
                cmd.Parameters.Clear();

                cmd.CommandText = "SP_CPOS_AddCuotasCashea";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodSucursal", codSucursal);
                cmd.Parameters.AddWithValue("@NroOren", nroOren);
                cmd.Parameters.AddWithValue("@NroContrato", nroContrato);
                cmd.Parameters.AddWithValue("@NroCuota", nroCuota);
                cmd.Parameters.AddWithValue("@MontoCuota", montoCuota);
                cmd.Parameters.AddWithValue("@User_Crea", userCrea);

                DataSet dt = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cmd.Parameters.Clear();
                return dt;

            }
            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                EscribirLog(ex.Message.ToString());
                return null;
            }
        }

        public DataSet RegistrarOrdenCashea(string codSucursal, string nroOrden, string nroFactura, string nroOrdenCashea, bool status, string userCrea, SqlCommand command = null)
        {
            try
            {
                Conexion.Conexion cn = new Conexion.Conexion();
                if (command == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command = connection.CreateCommand();
                }
                SqlCommand cmd = command;
                cmd.Parameters.Clear();

                cmd.CommandText = "SP_CPOS_AddOrdenCashea";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodSucursal", codSucursal);
                cmd.Parameters.AddWithValue("@NroOrden", nroOrden);
                cmd.Parameters.AddWithValue("@NroFactura", nroFactura);
                cmd.Parameters.AddWithValue("@NroOrdenCashea", nroOrdenCashea);
                cmd.Parameters.AddWithValue("@Estatus ", status);
                cmd.Parameters.AddWithValue("@UserCrea", userCrea);

                DataSet dt = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cmd.Parameters.Clear();
                return dt;

            }
            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                EscribirLog(ex.Message.ToString());
                return null;
            }
        }

        public DataTable ObtenerConfigCashea(SqlCommand command = null)
        {
            try
            {
                Conexion.Conexion cn = new Conexion.Conexion();
                if (command == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command = connection.CreateCommand();
                }
                SqlCommand cmd = command;
                cmd.Parameters.Clear();

                cmd.CommandText = "SP_CPOS_GetConfigCashea";
                cmd.CommandType = CommandType.StoredProcedure;



                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cmd.Parameters.Clear();
                return dt;

            }
            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                EscribirLog(ex.Message.ToString());
                return null;
            }
        }


        //public DataSet ObtenerConfigCashea(SqlCommand command = null)
        //{
        //    try
        //    {
        //        Conexion.Conexion cn = new Conexion.Conexion();
        //        if (command == null)
        //        {
        //            SqlConnection connection = cn.LeerCadena();
        //            command = connection.CreateCommand();
        //        }
        //        SqlCommand cmd = command;
        //        cmd.Parameters.Clear();

        //        cmd.CommandText = "SP_CPOS_GetConfigCashea";
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        DataSet dt = new DataSet();
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        cmd.Parameters.Clear();
        //        return dt;

        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = string.Format("Error: {0}", ex.Message);
        //        EscribirLog(ex.Message.ToString());
        //        return null;
        //    }
        //}

        public DataTable ObtieneOrdenesSinFacturaCashea(string codSucursal, SqlCommand command = null)
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

                cmd.CommandText = "SP_CPOS_GetOrdenCashea";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodSucursal", codSucursal);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cmd.Parameters.Clear();
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
