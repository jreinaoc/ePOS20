using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CapaDatos.ListaOrdenes_Datos
{
    public class D_ListaOrdenes
    {
        Conexion.Conexion cn = new Conexion.Conexion();

        public DataSet CargarOrdenes(string Fecha, string Status, string Orden, string Cedula, int Inicio = 1, int Final= 12)
        {
            SqlCommand cmd = new SqlCommand("SP_CPOS_BuscarOrdenesListaOrdenes", cn.LeerCadena());

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Fecha", Fecha);
            cmd.Parameters.AddWithValue("@Status", Status);
            cmd.Parameters.AddWithValue("@Orden", Orden);
            cmd.Parameters.AddWithValue("@Cedula", Cedula);
            cmd.Parameters.AddWithValue("@Inicio", Inicio);
            cmd.Parameters.AddWithValue("@Final", Final);

            DataSet dts = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dts);
            return (dts);

        }
        public DataSet CargarOrdPorRango(string Fechadesde, string Fechahasta, string Status, string Cedula, int Inicio = 1, int Final = 12)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_CPOS_BuscarOrdenesporRango", cn.LeerCadena());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Fechadesde", Fechadesde);
                cmd.Parameters.AddWithValue("@Fechahasta", Fechahasta);
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.Parameters.AddWithValue("@Cedula", Cedula);
                cmd.Parameters.AddWithValue("@Inicio", Inicio);
                cmd.Parameters.AddWithValue("@Final", Final);

                DataSet dts = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dts);
                return (dts);
            }

            catch (Exception ex)
            {
                return null;
                //MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }

        }

        public DataTable ParametrosPais(string Parametro)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT Parametro, Venezuela, RepublicaDominicana FROM TB_PARAMETROSPAIS where Parametro = @Parametro", cn.LeerCadena());
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Parametro", Parametro);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                return null;
                //MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }
        }

        public DataSet ValidaExistenciaArticulo(string NroOrden)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("pValidaExistenciaArticulo", cn.LeerCadena());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OS", NroOrden);


                DataSet dts = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dts);
                return dts;
            }
            catch (Exception ex)
            {
                return null;
                //MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }
        }

        public DataTable ObtenerDatos_TB_CAJA(string COD_SUCURSAL, bool Confirmado)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM TB_CAJA where COD_Sucursal = @COD_SUCURSAL and CONFIRMADO= @CONFIRMADO", cn.LeerCadena());
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@COD_SUCURSAL", COD_SUCURSAL);
                cmd.Parameters.AddWithValue("@CONFIRMADO", Confirmado);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                return null;
                //MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }
        }

        public string Procesar_Sin_Pago(string Cod_Sucursal, string NumOrdserv, string Revision, string Tipo_Pago, string Cod_Banco, string Abo_CTATARJETA, string Abo_CVCNROCHEQUE, string Abo_Fecha, double Abo_Monto, string Abo_Tipo,
    string Tipo_Pto, string CodPunto, bool Anulado, string Fec_Crea, string Fec_Mod, string USER_Crea, string USER_Mod, string Fecha, string Fecha_Abono, string Cod_BancoRecep, Double Tasa_Abono, Double Abo_Monto_Divisa, string Abo_Monto_SinIGTF,
    string Abo_IGTF, string OrSer_Tipo_Mon)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_CPOS_Procesar_Sin_Pago", cn.LeerCadena());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COD_SUCURSAL", Cod_Sucursal);
                cmd.Parameters.AddWithValue("@NumOrden", NumOrdserv);
                cmd.Parameters.AddWithValue("@REVISION", Revision);
                cmd.Parameters.AddWithValue("@TipoPago", Tipo_Pago);
                cmd.Parameters.AddWithValue("@CodBanco", Cod_Banco);
                cmd.Parameters.AddWithValue("@Abo_CTATARJETA", Abo_CTATARJETA);
                cmd.Parameters.AddWithValue("@Abo_CVCNEOCHEQUE", Abo_CVCNROCHEQUE);
                cmd.Parameters.AddWithValue("@Abo_Fecha", Abo_Fecha);
                cmd.Parameters.AddWithValue("@Abo_Monto", Abo_Monto);
                cmd.Parameters.AddWithValue("@Abo_Tipo", Abo_Tipo);
                cmd.Parameters.AddWithValue("@Tipo_Pto", Tipo_Pto);
                cmd.Parameters.AddWithValue("@CodPunto", CodPunto);
                cmd.Parameters.AddWithValue("@ANULADO", Anulado);
                cmd.Parameters.AddWithValue("@Fec_Crea", Fec_Crea);
                cmd.Parameters.AddWithValue("@Fec_Mod", "");
                cmd.Parameters.AddWithValue("@User_Crea", USER_Crea);
                cmd.Parameters.AddWithValue("@User_Mod", "");
                cmd.Parameters.AddWithValue("@FECHA", Fecha);
                cmd.Parameters.AddWithValue("@Fecha_Abono", Fecha_Abono);
                cmd.Parameters.AddWithValue("Cod_BancoRecep", Cod_BancoRecep);
                cmd.Parameters.AddWithValue("@TasaAbono", Tasa_Abono);
                cmd.Parameters.AddWithValue("Abo_Monto_Divisa", Abo_Monto_Divisa);
                cmd.Parameters.AddWithValue("@Abo_Monto_SinIGTF", Abo_Monto_SinIGTF);
                cmd.Parameters.AddWithValue("@Abo_IGTF", Abo_IGTF);
                cmd.Parameters.AddWithValue("@OrSer_Tipo_Mon", OrSer_Tipo_Mon);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                string Valor = dt.Rows[0]["resultado"].ToString();
                return Valor;

            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return null;
            }

        }

        public DataSet ReservaLC(string NumeroOrdenServicio, string Revision)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("pUpdReservaLC", cn.LeerCadena());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NUMORDSERV", NumeroOrdenServicio);
                cmd.Parameters.AddWithValue("@REVISION", Revision);

                DataSet dts = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dts);
                return dts;
            }
            catch (Exception ex)
            {
                return null;
                //MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }
        }

        public DataSet RelacionMov(string NroDocumento, string CodMov, string Sucursal, string Usuario, string FechaDiaAct, string NroOS, string Revision)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("pAddRelacionMovimientosLC", cn.LeerCadena());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NroDocumento", NroDocumento);
                cmd.Parameters.AddWithValue("@CodMov", CodMov);
                cmd.Parameters.AddWithValue("@Sucursal", Sucursal);
                cmd.Parameters.AddWithValue("@Usuario", Usuario);
                cmd.Parameters.AddWithValue("@FechaDiaAct", FechaDiaAct);
                cmd.Parameters.AddWithValue("@NroOS", NroOS);
                cmd.Parameters.AddWithValue("@Revision", Revision);

                DataSet dts = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dts);
                return dts;
            }
            catch (Exception ex)
            {
                return null;
                //MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }
        }

        public bool GetPagosdelDia(string NumOrden, string DiaActivo)
        {
            SqlCommand cmd = new SqlCommand("SP_CPOS_GET_PAGOSDELDIA", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ORDEN", NumOrden);
            cmd.Parameters.AddWithValue("@FECHA", DiaActivo);
            SqlDataReader dataReader = cmd.ExecuteReader();
            if (dataReader.HasRows)
            {
                return true;

            }
            else
            {

                return false;
            }

        }

        public DataTable RevisarAbonos_Epos(string Sucursal, string NumeroOrden, String Tipo_Pago)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("select * from TB_ABONO where Tipo_Pago = @Tipo_Pago and Cod_Sucursal = @COD_SUCURSAL and NumOrdserv= @NumOrden ", cn.LeerCadena());
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@COD_SUCURSAL", Sucursal);
                cmd.Parameters.AddWithValue("@NumOrden", NumeroOrden);
                cmd.Parameters.AddWithValue("@Tipo_Pago", Tipo_Pago);


                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return null;
            }
        }

        public DataSet CargarOrdenesConPagoMovil(string Fechadesde, string Fechahasta)
        {
            SqlCommand cmd = new SqlCommand("SP_CPOS_BuscarOrdenesPagoMovil", cn.LeerCadena());

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Fechadesde", Fechadesde);
            cmd.Parameters.AddWithValue("@Fechahasta", Fechahasta);

            DataSet dts = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dts);
            return (dts);

        }

      

        public DataSet CargarOrdenesPromo(string CodUsuario, string diaActivo, string CodPromo= "", SqlCommand command = null)
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

                cmd.CommandText = "pGetOSPromo";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@USER", CodUsuario);
                cmd.Parameters.AddWithValue("@DIAACTIVO", diaActivo);
                cmd.Parameters.AddWithValue("@CodigoPromo", CodPromo);
                DataSet dts = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dts);
                return (dts);

            }

            catch (Exception ex)
            {
                return null;
                //MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }

        }

        public DataSet CargarInformacionPromoCasadas(string diaActivo, SqlCommand command = null)
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

                cmd.CommandText = "SP_CPOS_Obtener_Informacion_Promos_Casadas";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DIAACTIVO", diaActivo);
                DataSet dts = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dts);
                return (dts);
            }

            catch (Exception ex)
            {
                return null;
                //MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }

        }

        public DataSet EjecutaStoreProcedure(string nombreProcedimiento, string parametrosOrdenes, SqlCommand command = null)
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

                // Configurar el comando para el stored procedure
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = nombreProcedimiento;
                cmd.Parameters.Clear();

                // Agregar parámetros según el formato que esperas
                string[] ordenes = parametrosOrdenes.Split(new string[] { "', '" }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < ordenes.Length; i++)
                {
                    string paramName = $"@OS{i + 1}";
                    cmd.Parameters.Add(CrearParametro(command, paramName, DbType.String, ordenes[i]));
                }

                DataSet dts = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dts);
                return (dts);

            }
            catch (Exception ex)
            {
                return null;
                //MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }
        }

        private IDbDataParameter CrearParametro(IDbCommand command, string nombre, DbType tipo, object valor)
        {
            var parametro = command.CreateParameter();
            parametro.ParameterName = nombre;
            parametro.DbType = tipo;
            parametro.Value = valor ?? DBNull.Value;
            return parametro;
        }


    }
}
