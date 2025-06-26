using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.CierreCaja_Datos
{
    public class D_CierreCaja
    {
        Conexion.Conexion cn = new Conexion.Conexion();

        public DataTable ChequeaFacturasdelDia(string fecha, string usuario, SqlCommand command = null)
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

                cmd.CommandText = "SP_CHEQUEAFACTURASDELDIA";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Fecha", fecha);
                cmd.Parameters.AddWithValue("@user", usuario);


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

        public DataTable ORDSERVCRITERIOSVARIOS(string bandera, string condicion, SqlCommand command = null)
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

                cmd.CommandText = "SP_ORDSERVCRITERIOSVARIOS";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BANDERA", bandera);
                cmd.Parameters.AddWithValue("@CONDICION", condicion);


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

        public DataTable CierreFueradeHorario(string codsuc, DateTime fechaIni, DateTime fechaFin, SqlCommand command = null)
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

                cmd.CommandText = "pGetCierreSucursal";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodSuc", codsuc);
                cmd.Parameters.AddWithValue("@FechaIni", fechaIni);
                cmd.Parameters.AddWithValue("@FechaFin", fechaFin);



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

        public DataTable CierrePuntodeVenta(string codsuc, string codBanco, string nroLote, DateTime fecha, SqlCommand command = null)
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

                cmd.CommandText = "pGetCierrePtoVta";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodSuc", codsuc);
                cmd.Parameters.AddWithValue("@CodBanco", codBanco);
                cmd.Parameters.AddWithValue("@NroLote", nroLote);
                cmd.Parameters.AddWithValue("@Fecha", fecha);

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

        public DataTable ObtienePuntosdeVenta(string codPunto, SqlCommand command = null)
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

                cmd.CommandText = "pGetPuntosVta";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodPunto", codPunto);
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

        public DataTable AgregaPuntosdeVenta( string codBanco,    DateTime fecha,    string nroLote,    decimal manualTarjCredito,    decimal manualTarjCreditoAmex,
        decimal manualTarjDebito,    decimal manualTarjOtros,    SqlCommand command = null)
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

                cmd.CommandText = "pAddCierrePtoVta";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodBanco", codBanco);
                cmd.Parameters.AddWithValue("@Fecha", fecha);
                cmd.Parameters.AddWithValue("@NroLote", nroLote);
                cmd.Parameters.AddWithValue("@ManualTarjCredito", manualTarjCredito);
                cmd.Parameters.AddWithValue("@ManualTarjCreditoAmex", manualTarjCreditoAmex);
                cmd.Parameters.AddWithValue("@ManualTarjDebito", manualTarjDebito);
                cmd.Parameters.AddWithValue("@ManualTarjOtros", manualTarjOtros);
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

        public DataTable ObtineneCambioCierre(DateTime fecha, string codSuc, SqlCommand command = null)
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

                cmd.CommandText = "pGetCambioCierre";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Fecha", fecha);
                cmd.Parameters.AddWithValue("@CodSuc", codSuc);



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

        public DataTable AgregaReferenciaCambioCierre(string codSuc, string orden, string referencia, string bancoEmisor, SqlCommand command = null)
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

                cmd.CommandText = "pGetCambioCierre";
                cmd.CommandType = CommandType.StoredProcedure;
             
                cmd.Parameters.AddWithValue("@CodSuc", codSuc);
                cmd.Parameters.AddWithValue("@NroOs", orden);
                cmd.Parameters.AddWithValue("@Referencia", referencia);
                cmd.Parameters.AddWithValue("@BancoEmisor", bancoEmisor);


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

        public DataTable ObtieneBancosPagoMovil(string codSuc, SqlCommand command = null)
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

                cmd.CommandText = "pGetCargoBancoPagomovil";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@suc", codSuc);


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

        public DataTable AgregaReferenciaPagoMovil(string codSuc, string nroOs, string referencia, string bancoEmisor, SqlCommand command = null)
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

                cmd.CommandText = "pAddReferenciaCambio";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CodSuc", codSuc);
                cmd.Parameters.AddWithValue("@NroOs", nroOs);
                cmd.Parameters.AddWithValue("@Referencia", referencia);
                cmd.Parameters.AddWithValue("@BancoEmisor", bancoEmisor);


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

        public DataTable VerificaAsistenciaPendiente(string fecha, string tipoAsis, SqlCommand command = null)
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

                cmd.CommandText = "pVerificoAsistenciaPend";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FECHA", fecha);
                cmd.Parameters.AddWithValue("@ASIS", "PEND");


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

        public DataTable ActualizaAsistencia(string fecha, string hora, string codEmp, string usuario, SqlCommand command = null)
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

                cmd.CommandText = "pUpdHoraSalidaAsistencia";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FECHA", fecha);
                cmd.Parameters.AddWithValue("@HORA", hora);
                cmd.Parameters.AddWithValue("@CODEMP", codEmp);
                cmd.Parameters.AddWithValue("@USER", usuario);


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
