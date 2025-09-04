using CapaDatos.Inicio_Datos;
using System;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Anulacion
{

    public class D_Anulacion
    {
        Conexion.Conexion cn = new Conexion.Conexion();
        D_Inicio _D_Inicio = new D_Inicio();
        public string MovimientoInv;
        public string NroNota;
        public string MontoNota;
        public DataTable TraerResponsables(string CodSucursal)
        {
            SqlCommand cmd = new SqlCommand("SP_CPOS_RESPONSAB_ANUL", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodSucursal", CodSucursal);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return (dt);

        }
        public DataTable TraerMotivos(string CodResponsble)
        {
            SqlCommand cmd = new SqlCommand("SP_CPOS_MOTIVO_ANULACION", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodResponsable", CodResponsble);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return (dt);
        }

        public string AnularOrden(string NumOrden, string CodMotivo, string CodRespAnu, string Observ, string CodUser, string Nota, SqlCommand command = null)
        {
            try
            {
                if (command == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command = connection.CreateCommand();
                }
            SqlCommand cmd = command;
            cmd.CommandText = ("SP_CPOS_Anulacion_Orden");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NumOrden", NumOrden);
            cmd.Parameters.AddWithValue("@CodigoMotivo", CodMotivo);
            cmd.Parameters.AddWithValue("@CodigoRespAnu", CodRespAnu);
            cmd.Parameters.AddWithValue("@ObsAnul", Observ);
            cmd.Parameters.AddWithValue("@CodUsuario", CodUser);
            cmd.Parameters.AddWithValue("@NotaDev", Nota);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            cmd.Parameters.Clear();
            return "SATISFACTORIO";
            }
            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return Error;
            }

}

public DataTable TraerOrdenDet(string NumOrden , SqlCommand command = null)  // Trae el detalle de la orden 
        {
            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }
            SqlCommand cmd = command;
            cmd.CommandText ="SP_CPOS_REC_DETALLE";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NumOrden", NumOrden);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            cmd.Parameters.Clear();
            return (dt);

        }

        public string TraerArt(string CodArticulo, SqlCommand command = null)  // Trae el detalle del articulo 
        {
             if (command == null)
             {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
             }
            SqlCommand cmd = command;
            cmd.CommandText ="SP_CPOS_GET_ARTICULO";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodArticulo", CodArticulo);
            cmd.Parameters.AddWithValue("@TipoTrabajo", "");

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            cmd.Parameters.Clear();
            string CostoArt = dt.Rows[0]["COSTOULTI"].ToString();
            return (CostoArt);

        }

        public decimal Saldo_Orden(string CodSucursal, string NumeroOrden, string Revision, SqlCommand command = null)
        {
                if (command == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command = connection.CreateCommand();
                }
                SqlCommand cmd = command;
                cmd.CommandText = " select ISNULL(CONVERT (DECIMAL (28, 2), SUM(Abo_Monto)),0) as resultado from TB_ABONO WHERE Cod_Sucursal = @CodSucursal and NumOrdserv = @NumOrden and Revision = @Revision and Anulado = 0 ";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@NumOrden", NumeroOrden);
                cmd.Parameters.AddWithValue("@CodSucursal", CodSucursal);
                cmd.Parameters.AddWithValue("@Revision", Revision);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                decimal resultado = Convert.ToDecimal( dt.Rows[0]["resultado"]) ;
                cmd.Parameters.Clear();
                return resultado;

        }



        public string CargarNotaDevolucion(string CodSucursal, string CodDoc, string NumeroOrden, string Revision, string NroControl, string CteNacionalidad, string CteCedula, string Motivo, string MontoNota, string MontoAplicado, bool Reintegro, bool Anulado, string UserCrea, string UserMod, SqlCommand command = null)
        {
            try
            {

                if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }
            SqlCommand cmd = command;
            cmd.CommandText ="SP_CPOS_GeneraNotaDev";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodSucursal", CodSucursal);
            cmd.Parameters.AddWithValue("@CodDoc", CodDoc);
            cmd.Parameters.AddWithValue("@NumOrden", NumeroOrden);
            cmd.Parameters.AddWithValue("@Revision", Revision);
            cmd.Parameters.AddWithValue("@NroControl", NroControl);
            cmd.Parameters.AddWithValue("@CteNacio", CteNacionalidad);
            cmd.Parameters.AddWithValue("@CteCedIden", CteCedula);
            cmd.Parameters.AddWithValue("@Motivo", Motivo);
            cmd.Parameters.AddWithValue("@MontoNota", MontoNota);
            cmd.Parameters.AddWithValue("@MontoAplicado", MontoAplicado);
            cmd.Parameters.AddWithValue("@Reintegro", Reintegro);
            cmd.Parameters.AddWithValue("@Anulado", Anulado);
            cmd.Parameters.AddWithValue("@UserCrea", UserCrea);
            cmd.Parameters.AddWithValue("@User_Mod", UserMod);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            cmd.Parameters.Clear();
                return "SATISFACTORIO";
            }
            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return Error;
            }

        }

        public string CargarGarantia(string NumeroOrden, string CodSuc, string Osrepo, SqlCommand command = null)
        {
            try
            {
                if (command == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command = connection.CreateCommand();
                }
                SqlCommand cmd = command;
                cmd.CommandText ="pUpdOSGarantia";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OS", NumeroOrden);
            cmd.Parameters.AddWithValue("@SUC", CodSuc);
            cmd.Parameters.AddWithValue("@OSREPO", Osrepo);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            cmd.Parameters.Clear();
            return "SATISFACTORIO";

            }
            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return Error;
            }

        }
        public void CaragarAuditor(string CodSucursal, string CodAccion, string CodEmpleado, string Detalles, SqlCommand command = null)
        {
            try
            {

                if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }
            SqlCommand cmd = command;
            cmd.CommandText ="SP_CPOS_CARGAR_AUDITOR";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodSucursal", CodSucursal);
            cmd.Parameters.AddWithValue("@CodAccion", CodAccion);
            cmd.Parameters.AddWithValue("@CarnetUsusario", CodEmpleado);
            cmd.Parameters.AddWithValue("@Detalles", Detalles);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
             
            }

        }

        public string ObtenerNroNota (string NroOrden, string CodSuc, string Revision, SqlCommand command = null)
        {
            try
            {
                if (command == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command = connection.CreateCommand();
                }
            SqlCommand cmd = command;
            cmd.CommandText ="SP_CPOS_GET_NRO_NOTADEV";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NumeroOrden", NroOrden);
            cmd.Parameters.AddWithValue("@CodSuc", CodSuc);
            cmd.Parameters.AddWithValue("@Revision", Revision);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            NroNota = dt.Rows[0]["NRONOTA"].ToString();
            MontoNota = dt.Rows[0]["MontoNota"].ToString();

            cmd.Parameters.Clear();
            return "SATISFACTORIO";
        }
            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return Error;
            }

        }

public string EjecMovAnulacion(string NroOrden, string CodSuc, string TipoDoc, string CodDoc, string DiaAct, string NotaAc, string CodUser, SqlCommand command = null)
        {
            try
            {
                if (command == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command = connection.CreateCommand();
                }

             SqlCommand cmd = command;
             cmd.CommandText = "pMovimientosAnulacionOSLC";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OS", NroOrden);
            cmd.Parameters.AddWithValue("@suc", CodSuc);
            cmd.Parameters.AddWithValue("@TIPDOC", TipoDoc);
            cmd.Parameters.AddWithValue("@CODDOC", CodDoc);
            cmd.Parameters.AddWithValue("@DIAACT", DiaAct);
            cmd.Parameters.AddWithValue("@NOTAC", NotaAc);
            cmd.Parameters.AddWithValue("@USER", CodUser);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            cmd.Parameters.Clear();
            return "SATISFACTORIO";

            }
            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return Error;
            }


        }

        public string EliminarAbonoActualizarSaldo(int ID_Abono, string Cod_Sucursal, string NumOrden, string Revision, string Usuario, Double AboMonto)
        {
            try
            {

            SqlCommand cmd = new SqlCommand("SP_CPOS_EliminarAbonoActualizarSaldo", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID_Abono", ID_Abono);
            cmd.Parameters.AddWithValue("@Cod_Sucursal", Cod_Sucursal);
            cmd.Parameters.AddWithValue("@NumOrden", NumOrden);
            cmd.Parameters.AddWithValue("@Usuario", Usuario);
            cmd.Parameters.AddWithValue("@REVISION", Revision);
            cmd.Parameters.AddWithValue("@AboMonto", AboMonto);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            string resultado = dt.Rows[0]["resultado"].ToString();

            return resultado;

            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return null;
            }


        }

        public DataTable ObtenerAbonosActivos(string Cod_Sucursal, string NumOrden, string Revision)
        {
            SqlCommand cmd = new SqlCommand("select * from TB_ABONO WHERE Cod_Sucursal = @COD_SUCURSAL and NumOrdserv = @NumOrden and Revision = @REVISION and Anulado = 0 ", cn.LeerCadena());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Cod_Sucursal", Cod_Sucursal);
            cmd.Parameters.AddWithValue("@NumOrden", NumOrden);
            cmd.Parameters.AddWithValue("@Revision", Revision);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;

        }

        public string EliminarOrden(string Cod_Sucursal, string NumOrden, string Revision, string CodMotivo, string Cod_ResponsableAnu, string Usuario, string Observa)
        {
            try
            {
            SqlCommand cmd = new SqlCommand("SP_CPOS_EliminarOrden", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Cod_Sucursal", Cod_Sucursal);
            cmd.Parameters.AddWithValue("@NumOrden", NumOrden);
            cmd.Parameters.AddWithValue("@REVISION", Revision);
            cmd.Parameters.AddWithValue("@CodMotivo", CodMotivo);
            cmd.Parameters.AddWithValue("@Cod_ResponsableAnu", Cod_ResponsableAnu);
            cmd.Parameters.AddWithValue("@Usuario", Usuario);
            cmd.Parameters.AddWithValue("@Observa", Observa);

                DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            string resultado = dt.Rows[0]["resultado"].ToString();

            return resultado;
            }

            catch (Exception ex)
            {
             string Error = string.Format("Error: {0}", ex.Message);
             return null;
            }
        }


        public string EliminarFactura(string Cod_Sucursal, string NumeroFactura, string Usuario, string Fact_SerialImpresora)
        {
            try
            {
                    SqlCommand cmd = new SqlCommand("SP_CPOS_EliminarFactura", cn.LeerCadena());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Cod_Sucursal", Cod_Sucursal);
                    cmd.Parameters.AddWithValue("@NumeroFactura", NumeroFactura);
                    cmd.Parameters.AddWithValue("@Fact_SerialImpresora", Fact_SerialImpresora);
                    cmd.Parameters.AddWithValue("@Usuario", Usuario);

                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    string resultado = dt.Rows[0]["resultado"].ToString();

                    return resultado;
             }

                catch (Exception ex)
                {
                    string Error = string.Format("Error: {0}", ex.Message);
                    return null;
                }


        }

        public DataSet OSAntiguaLC(string NumeroOrden, string Revision, SqlCommand command = null)
        {
            try
            { 
            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }
            SqlCommand cmd = command;
            cmd.CommandText ="pGetOSAntiguaLC";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OS", NumeroOrden);
            cmd.Parameters.AddWithValue("@REV", Revision);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet dts = new DataSet();
            da.Fill(dts);
            cmd.Parameters.Clear();
            return dts;
            }
            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return null;
            }

        }

        public DataTable TraerAbonos(string Sucursal, string NumeroOrden, string Revision, SqlCommand command = null)
        {
            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }

                SqlCommand cmd = command;
                cmd.CommandText= " SELECT * from TB_ABONO where NumOrdserv = @NumeroOrden and Anulado= 0 and Cod_Sucursal=@Sucursal and Revision= @Revision ";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@NumeroOrden", NumeroOrden);
                cmd.Parameters.AddWithValue("@Sucursal", Sucursal);
                cmd.Parameters.AddWithValue("@Revision", Revision);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cmd.Parameters.Clear();
                return dt;

        }
    }
}
