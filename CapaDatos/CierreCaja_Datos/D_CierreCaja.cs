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


        

        public DataTable ConsultaOsDia(DateTime fecha, string suc, SqlCommand command = null)
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

                cmd.CommandText = "SP_CONSULTAOSDIA";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fecha", fecha);
                cmd.Parameters.AddWithValue("@suc", suc);



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

        public DataTable ModificaVendedor(string order, string codEmpleadoNew, string usuario, string Suc, SqlCommand command = null)
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

                cmd.CommandText = "SP_MODIFICACODVENDEDOR";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fecha", order);
                cmd.Parameters.AddWithValue("@CodEmpleadoNuevo", codEmpleadoNew);
                cmd.Parameters.AddWithValue("@Usuario", usuario);
                cmd.Parameters.AddWithValue("@suc", Suc);



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

        public DataTable CierreDeCaja(DateTime fecha, string codSucursal, decimal M_TotalIngresos, decimal M_Efectivo, decimal M_Cheques,
            decimal M_Cupones, decimal M_TicketsSalud, decimal M_TicketsSaludEfec, decimal M_TarjetaC, decimal M_TarjetaD, decimal M_NotaCredito,
            decimal M_Credito, decimal M_Reintegro, decimal M_Gastos, decimal M_Financiamiento, decimal M_NotaDevolucion, decimal M_OrdenPago,
            decimal M_IVARetenido, decimal M_ISRLRetenido, decimal M_Transferencia, decimal M_Vuelto, string M_Observacion, string M_Usuario,
            bool cierreParcial, bool trabajaDomingos, string userEntrega, string userRecibe, SqlCommand command = null)
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

                cmd.CommandText = "SP_CierreDeCaja";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Fecha", fecha);
                cmd.Parameters.AddWithValue("@CodSucursal", codSucursal);
                cmd.Parameters.AddWithValue("@MTotalIngresos", M_TotalIngresos);
                cmd.Parameters.AddWithValue("@MEfectivo", M_Efectivo);
                cmd.Parameters.AddWithValue("@MCheques", M_Cheques);
                cmd.Parameters.AddWithValue("@MCupones", M_Cupones);
                cmd.Parameters.AddWithValue("@MTicketsSalud", M_TicketsSalud);
                cmd.Parameters.AddWithValue("@MTicketsSaludEfec", M_TicketsSaludEfec);
                cmd.Parameters.AddWithValue("@MTarjetaC", M_TarjetaC);
                cmd.Parameters.AddWithValue("@MTarjetaD", M_TarjetaD);
                cmd.Parameters.AddWithValue("@MNotaCredito", M_NotaCredito);
                cmd.Parameters.AddWithValue("@MCredito", M_Credito);
                cmd.Parameters.AddWithValue("@MReintegro", M_Reintegro);
                cmd.Parameters.AddWithValue("@MGastos", M_Gastos);
                cmd.Parameters.AddWithValue("@MFinanciamiento", M_Financiamiento);
                cmd.Parameters.AddWithValue("@MNotaDevolucion", M_NotaDevolucion);
                cmd.Parameters.AddWithValue("@MOrdenPago", M_OrdenPago);
                cmd.Parameters.AddWithValue("@MIVARetenido", M_IVARetenido);
                cmd.Parameters.AddWithValue("@MISLRRetenido", M_ISRLRetenido);
                cmd.Parameters.AddWithValue("@MTransferencia", M_Transferencia);
                cmd.Parameters.AddWithValue("@MVuelto", M_Vuelto);
                cmd.Parameters.AddWithValue("@MObservacion", M_Observacion);
                cmd.Parameters.AddWithValue("@MUsuario", M_Usuario);
                cmd.Parameters.AddWithValue("@CierreParcial", cierreParcial);
                cmd.Parameters.AddWithValue("@TrabajaDomingos", trabajaDomingos);
                cmd.Parameters.AddWithValue("@UserEntrega", userEntrega);
                cmd.Parameters.AddWithValue("@UserRecibe", userRecibe);



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

        public DataTable ActualizarParamCierreCaja(string suc, SqlCommand command = null)
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

                cmd.CommandText = "pUpdateParamCierreCaja";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SUC", suc);



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

        public DataTable DesbloqueSistema(string bloqueo, string sucursal, SqlCommand command = null)
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

                cmd.CommandText = "SP_DES_BLOQUEASISTEMA";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Bloqueo", bloqueo);
                cmd.Parameters.AddWithValue("@sucursal", sucursal);



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

        public DataTable ActualizarFacturas(string sucursal, SqlCommand command = null)
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

                cmd.CommandText = "SP_ACTUALIZAFACTURAS01";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sucursal", sucursal);


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

        public DataTable LibroVenta(DateTime fechaIni, DateTime fechaFin, SqlCommand command = null)
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

                cmd.CommandText = "SP_LIBRO_VENTA";
                cmd.CommandType = CommandType.StoredProcedure;
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

        public DataTable InventarioFaltante(DateTime diaFact, SqlCommand command = null)
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

                cmd.CommandText = "pAddMovInventarioFaltante";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DIAFACT", diaFact);

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
