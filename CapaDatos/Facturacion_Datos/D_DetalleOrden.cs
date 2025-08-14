using CapaDatos.Inicio_Datos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaDatos.DetalleOrden_Datos
{
    public class D_DetalleOrden 
    {
        Conexion.Conexion cn = new Conexion.Conexion();
        D_Inicio _D_Inicio = new D_Inicio();
        public string MovimientoInv;
        public bool ClientePagador = false;
        public string Nombre;
        public string CED;
        public string Correo;
        public string Tlf;
        public DataTable Pagos()
        {
            SqlDataAdapter da = new SqlDataAdapter("select COD_PAGO as Value, DescripPago as Indexx from TB_TIPOPAGO where  TipoPag_ST= 'A' order by Indexx ASC", cn.LeerCadena());
            da.SelectCommand.CommandType = CommandType.Text;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;

        }

        public DataTable Bancos(bool MonedaExtranjera)
        {
            SqlCommand cmd = new SqlCommand("select CODBAN as Value, NOMBREBANCO as Indexx from TB_BANCOS where  ST_BANCOS= 'A' and MONEDAEXTRANJERA= @Moneda and CODBAN<> '007' and CODBAN<> '110' ", cn.LeerCadena());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Moneda", MonedaExtranjera);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;

        }

        public DataTable Bancos2(bool MonedaExtranjera)
        {
            SqlCommand cmd = new SqlCommand("select CODBAN as Value, NOMBREBANCO as Indexx from TB_BANCOS where  ST_BANCOS= 'A' and MONEDAEXTRANJERA= @Moneda ", cn.LeerCadena());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Moneda", MonedaExtranjera);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;

        }


        public DataTable BancoRecp(bool MonedaExtranjera)
        {
            SqlCommand cmd = new SqlCommand("select CODBAN as Value, NOMBREBANCO as Indexx from TB_BANCOS where  RT_BANCOS= 'A' and MONEDAEXTRANJERA= @Moneda and CODBAN<> '007' and CODBAN<> '110'", cn.LeerCadena());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Moneda", MonedaExtranjera);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        public DataTable BancoRecp_Pagomovil(bool MonedaExtranjera)
        {
            SqlCommand cmd = new SqlCommand("select CODBAN as Value, NOMBREBANCO as Indexx from TB_BANCOS where MONEDAEXTRANJERA= @Moneda and PagoMovil=1 ", cn.LeerCadena());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Moneda", MonedaExtranjera);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        //public DataTable BancoRecp_Pagomovil(bool MonedaExtranjera)
        //{
        //    SqlCommand cmd = new SqlCommand("select CODBAN as Value, NOMBREBANCO as Indexx from TB_BANCOS where  CODBAN= '077' ", cn.LeerCadena());
        //    cmd.CommandType = CommandType.Text;
        //    cmd.Parameters.AddWithValue("@Moneda", MonedaExtranjera);
        //    DataTable dt = new DataTable();
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(dt);
        //    return dt;
        //}

        public DataTable BucarTotalAbonosRealizados(string NumeroOrden)
        {
            string CodSuc = _D_Inicio.Sucursal();


            SqlCommand cmd = new SqlCommand("SP_CPOS_BucarTotalAbonosRealizados", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodSuc", CodSuc);
            cmd.Parameters.AddWithValue("@NumeroOrden", NumeroOrden);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;

        }



        private void RecalcularOrden(string CodSuc, string NumeroOrden)
        {
            SqlCommand cmd = new SqlCommand("pRecalculaOSDivisa", cn.LeerCadena());
            //SqlCommand cmd = new SqlCommand("SP_CPOS_pRecalculaOSDivisa", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CodSuc", CodSuc);
            cmd.Parameters.AddWithValue("@NumordServ", NumeroOrden);
            cmd.Parameters.AddWithValue("@Revision", "0");

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
        }
        public void Datos_de_la_Orden(string NumeroOrden, string Revison)
        {
            try
            {
                string Sucursal = _D_Inicio.Sucursal();

                ////ejecuto el recalculo de la orden 
                //RecalcularOrden(Sucursal, NumeroOrden);

                //Busco los datos de la orden; datos que ya estan actualizados (Recalculados)
                SqlCommand cmd = new SqlCommand("SP_CPOS_Datos_de_la_Orden", cn.LeerCadena());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sucursal", Sucursal);
                cmd.Parameters.AddWithValue("@NumeroOrden", NumeroOrden);
                cmd.Parameters.AddWithValue("@Revison", Revison);
                SqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.HasRows)
                {

                    while (dataReader.Read())
                    {
                        TB_CAORDSER.Cod_Sucursal = Convert.ToString(dataReader["Cod_Sucursal"]);
                        TB_CAORDSER.NumOrdserv = Convert.ToString(dataReader["NumOrdserv"]);
                        TB_CAORDSER.Revision = Convert.ToString(dataReader["Revision"]);
                        TB_CAORDSER.Fecha = Convert.ToDateTime(dataReader["Fecha"].ToString());
                        TB_CAORDSER.Cod_Venta = Convert.ToString(dataReader["Cod_Venta"]);
                        TB_CAORDSER.CTE_Nacio = Convert.ToString(dataReader["CTE_Nacio"]);
                        TB_CAORDSER.CTE_CedIden = Convert.ToString(dataReader["CTE_CedIden"]);
                        TB_CAORDSER.NumExamen = dataReader["NumExamen"] == DBNull.Value ? (Int32?)0.00 : Convert.ToInt32(dataReader["NumExamen"]);
                        TB_CAORDSER.COD_EMPLEADO = Convert.ToString(dataReader["COD_EMPLEADO"]);
                        TB_CAORDSER.Cod_Laboratorio = Convert.ToString(dataReader["Cod_Laboratorio"]);
                        TB_CAORDSER.Cod_Servicio = Convert.ToString(dataReader["Cod_Servicio"]);
                        TB_CAORDSER.Vision = Convert.ToString(dataReader["Vision"]);
                        TB_CAORDSER.Fec_ofrecido = Convert.ToDateTime(dataReader["Fec_ofrecido"].ToString());
                        TB_CAORDSER.Hor_ofrecido = Convert.ToString(dataReader["Hor_ofrecido"]);
                        TB_CAORDSER.Fec_Entrega = dataReader["Fec_Entrega"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["Fec_Entrega"]);
                        TB_CAORDSER.Fec_Envio = dataReader["Fec_Envio"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["Fec_Envio"]);
                        TB_CAORDSER.Fec_Recibido = dataReader["Fec_Recibido"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["Fec_Recibido"]);
                        TB_CAORDSER.VtaSubTotal = Convert.ToDouble(dataReader["VtaSubTotal"]);
                        TB_CAORDSER.VtaImpuesto = Convert.ToDouble(dataReader["VtaImpuesto"]);
                        TB_CAORDSER.VtaDescuento = Convert.ToDouble(dataReader["VtaDescuento"]);
                        TB_CAORDSER.VtaTotal = Convert.ToDouble(dataReader["VtaTotal"]);
                        TB_CAORDSER.OrSer_Saldo = Convert.ToDouble(dataReader["OrSer_Saldo"]);
                        TB_CAORDSER.OrSer_Finan = Convert.ToBoolean(dataReader["OrSer_Finan"]);
                        TB_CAORDSER.OrSer_Status = Convert.ToString(dataReader["OrSer_Status"]);
                        TB_CAORDSER.OrSer_Observ = Convert.ToString(dataReader["OrSer_Observ"]);
                        TB_CAORDSER.CodCausa = Convert.ToString(dataReader["CodCausa"]);
                        TB_CAORDSER.MonturaPropia = dataReader["MonturaPropia"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dataReader["MonturaPropia"]);
                        TB_CAORDSER.OrSer_fecCrea = Convert.ToDateTime(dataReader["OrSer_fecCrea"].ToString());
                        TB_CAORDSER.OrSer_FecMod = dataReader["OrSer_FecMod"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["OrSer_FecMod"]);
                        TB_CAORDSER.USER_Crea = Convert.ToString(dataReader["USER_Crea"]);
                        TB_CAORDSER.USER_Mod = Convert.ToString(dataReader["USER_Mod"]);
                        TB_CAORDSER.Cod_DetVta = Convert.ToString(dataReader["Cod_DetVta"]);
                        TB_CAORDSER.Aplica = dataReader["Aplica"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dataReader["Aplica"]);
                        TB_CAORDSER.OTCORRESPONDIENTE = Convert.ToString(dataReader["OTCORRESPONDIENTE"]);
                        TB_CAORDSER.Anulado = dataReader["Anulado"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dataReader["Anulado"]);
                        TB_CAORDSER.Ventaafil = dataReader["Ventaafil"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dataReader["Ventaafil"]);
                        TB_CAORDSER.cristalpropio = Convert.ToBoolean(dataReader["cristalpropio"]);
                        TB_CAORDSER.Nota = dataReader["Nota"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dataReader["Nota"]);
                        TB_CAORDSER.Cuantas = dataReader["Cuantas"] == DBNull.Value ? (Int32?)0.00 : Convert.ToInt32(dataReader["Cuantas"]);
                        TB_CAORDSER.CodMotivoAnul = Convert.ToString(dataReader["CodMotivoAnul"]);
                        TB_CAORDSER.FechaAnulacion = dataReader["FechaAnulacion"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["FechaAnulacion"]);
                        TB_CAORDSER.FechaCaja = dataReader["FechaCaja"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["FechaCaja"]);
                        TB_CAORDSER.Cod_ResponsableRev = Convert.ToString(dataReader["Cod_ResponsableRev"]);
                        TB_CAORDSER.Cod_ResponsableAnu = Convert.ToString(dataReader["Cod_ResponsableAnu"]);
                        TB_CAORDSER.Asegurada = dataReader["Asegurada"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dataReader["Asegurada"]);
                        TB_CAORDSER.Exonerada = dataReader["Exonerada"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dataReader["Exonerada"]);
                        TB_CAORDSER.TipoMonturaPropia = Convert.ToString(dataReader["TipoMonturaPropia"]);
                        TB_CAORDSER.CodMotivoReposicion = Convert.ToString(dataReader["CodMotivoReposicion"]);
                        TB_CAORDSER.Cedula_CteAfil = Convert.ToString(dataReader["Cedula_CteAfil"]);
                        TB_CAORDSER.Codigo_EmpAfil = Convert.ToString(dataReader["Codigo_EmpAfil"]);
                        TB_CAORDSER.MonturaEnQuorum = dataReader["MonturaEnQuorum"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dataReader["MonturaEnQuorum"]);
                        TB_CAORDSER.Cod_Coloracion = Convert.ToString(dataReader["Cod_Coloracion"]);
                        TB_CAORDSER.OS_Externa = Convert.ToString(dataReader["OS_Externa"]);
                        TB_CAORDSER.OrSer_Saldo_Mon = dataReader["OrSer_Saldo_Mon"] == DBNull.Value ? (Double?)0.00 : Convert.ToDouble(dataReader["OrSer_Saldo_Mon"]);
                        TB_CAORDSER.OrSer_Tipo_Mon = Convert.ToString(dataReader["OrSer_Tipo_Mon"]);
                        TB_CAORDSER.Orser_Total_Mon = dataReader["Orser_Total_Mon"] == DBNull.Value ? (Double?)0.00 : Convert.ToDouble(dataReader["Orser_Total_Mon"]);
                        TB_CAORDSER.VtaImpuestoIGTF = dataReader["VtaImpuestoIGTF"] == DBNull.Value ? (Double?)0.00 : Convert.ToDouble(dataReader["VtaImpuestoIGTF"]);
                    }

                }
            }
            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);

            }
        }

        public DataTable BuscarNota(string numDocumento, bool nota, string cedulaCliente)
        {
            string CodSuc = _D_Inicio.Sucursal();


            SqlCommand cmd = new SqlCommand("SP_CPOS_BucarNota", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@numDocumento", numDocumento);
            cmd.Parameters.AddWithValue("@Nota", nota);
            cmd.Parameters.AddWithValue("@cedulaCliente", cedulaCliente);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;

        }

        public DataTable ExistenNotas(string cedulaCliente)
        {

            SqlCommand cmd = new SqlCommand("SELECT NRONOTA,Fact_Num,Motivo, SaldoNota from  TB_NOTASCREDITODEBITO where CTE_CedIden= @cedulaCliente and Anulado= 0 ", cn.LeerCadena());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@cedulaCliente", cedulaCliente);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;

        }


        public string RegistrarAbono(string NumeroOrden, string NombreCliente, string Cedula,
   string Dvc, string AgPant, string AgFac, string Mimesys, string OdEsfera,
   string OdCilindro, string OdEje, string OdAdicion, string OdLejos, string Odcerca,
   string OdLejos2, string Odcerca2, string OdMedia, string OiEsfera, string OiCilindro, string OiEje, string OiAdicion,
   string OiLejos, string Oicerca, string OiLejos2, string Oicerca2, string OiMedia, string Observacion,
   string Vison, string OjoDerecho, string OjoIzquierdo, string OiAltura, string OdAltura,
   string OdCb, string OiCb, string Laboratorio, string OdPrisma, string OdGrado, string OdPrisma2, string OdGrado2, string OiPrisma,
   string OiGrado, string OiPrisma2, string OiGrado2, string Horizontal, string Vertical, string Maxima, string Puente)
        {

            SqlCommand cmd = new SqlCommand("pAdd_GuardarOrden_CAB_CargaOs", cn.LeerCadena());
            double? prueba;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NumeroOrden", NumeroOrden);
            cmd.Parameters.AddWithValue("@NombreCliente", NombreCliente);
            cmd.Parameters.AddWithValue("@Cedula", Cedula);
            cmd.Parameters.AddWithValue("@Dvc", (Dvc == "" ? (Double?)0.00 : Convert.ToDouble(Dvc.ToString())));
            cmd.Parameters.AddWithValue("@AgPant", (AgPant == "" ? (Double?)0.00 : Convert.ToDouble(AgPant.ToString())));
            cmd.Parameters.AddWithValue("@AgFac", (AgFac == "" ? (Double?)0.00 : Convert.ToDouble(AgFac.ToString())));
            cmd.Parameters.AddWithValue("@Mimesys", Mimesys);
            cmd.Parameters.AddWithValue("@OdEsfera", (OdEsfera == "" ? (Double?)0.00 : Convert.ToDouble(OdEsfera.ToString())));
            cmd.Parameters.AddWithValue("@OdCilindro", (OdCilindro == "" ? (Double?)0.00 : Convert.ToDouble(OdCilindro.ToString())));
            cmd.Parameters.AddWithValue("@OdEje", (OdEje == "" ? (Int16?)0 : Convert.ToInt16(OdEje.ToString())));
            cmd.Parameters.AddWithValue("@OdAdicion", prueba = (OdAdicion == "" ? (Double?)0.00 : Convert.ToDouble(OdAdicion.ToString())));
            cmd.Parameters.AddWithValue("@OdLejos", prueba = (OdLejos == "" ? (Double?)0.00 : Convert.ToDouble(OdLejos.ToString())));
            cmd.Parameters.AddWithValue("@Odcerca", (Odcerca == "" ? (Double?)0.00 : Convert.ToDouble(Odcerca.ToString())));
            cmd.Parameters.AddWithValue("@OdLejos2", (OdLejos2 == "" ? (Double?)0.00 : Convert.ToDouble(OdLejos2.ToString())));
            cmd.Parameters.AddWithValue("@Odcerca2", (Odcerca2 == "" ? (Double?)0.00 : Convert.ToDouble(Odcerca2.ToString())));
            cmd.Parameters.AddWithValue("@OdMedia", (OdMedia == "" ? (Double?)0.00 : Convert.ToDouble(OdMedia.ToString())));
            cmd.Parameters.AddWithValue("@OiEsfera", (OiEsfera == "" ? (Double?)0.00 : Convert.ToDouble(OiEsfera.ToString())));
            cmd.Parameters.AddWithValue("@OiCilindro", (OiCilindro == "" ? (Double?)0.00 : Convert.ToDouble(OiCilindro.ToString())));
            cmd.Parameters.AddWithValue("@OiEje", (OiEje == "" ? (Int16?)0 : Convert.ToInt16(OiEje.ToString())));
            cmd.Parameters.AddWithValue("@OiAdicion", (OiAdicion == "" ? (Double?)0.00 : Convert.ToDouble(OiAdicion.ToString())));
            cmd.Parameters.AddWithValue("@OiLejos", (OiLejos == "" ? (Double?)0.00 : Convert.ToDouble(OiLejos.ToString())));
            cmd.Parameters.AddWithValue("@Oicerca", (Oicerca == "" ? (Double?)0.00 : Convert.ToDouble(Oicerca.ToString())));
            cmd.Parameters.AddWithValue("@OiLejos2", (OiLejos2 == "" ? (Double?)0.00 : Convert.ToDouble(OiLejos2.ToString())));
            cmd.Parameters.AddWithValue("@Oicerca2", (Oicerca2 == "" ? (Double?)0.00 : Convert.ToDouble(Oicerca2.ToString())));
            cmd.Parameters.AddWithValue("@OiMedia", (OiMedia == "" ? (Double?)0.00 : Convert.ToDouble(OiMedia.ToString())));
            cmd.Parameters.AddWithValue("@Observacion", Observacion);
            cmd.Parameters.AddWithValue("@Vison", Vison);
            cmd.Parameters.AddWithValue("@OjoDerecho", OjoDerecho);
            cmd.Parameters.AddWithValue("@OjoIzquierdo", OjoIzquierdo);
            cmd.Parameters.AddWithValue("@OiAltura ", (OiAltura == "" ? (Double?)0.00 : Convert.ToDouble(OiAltura.ToString())));
            cmd.Parameters.AddWithValue("@OdAltura", (OdAltura == "" ? (Double?)0.00 : Convert.ToDouble(OdAltura.ToString())));
            cmd.Parameters.AddWithValue("@OdCb", (OdCb == "" ? (Double?)0.00 : Convert.ToDouble(OdCb.ToString())));
            cmd.Parameters.AddWithValue("@OiCb", (OiCb == "" ? (Double?)0.00 : Convert.ToDouble(OiCb.ToString())));
            cmd.Parameters.AddWithValue("@Laboratorio", Laboratorio);
            cmd.Parameters.AddWithValue("@OdPrisma", (OdPrisma == "" ? (Double?)0.00 : Convert.ToDouble(OdPrisma.ToString())));
            cmd.Parameters.AddWithValue("@OdGrado", OdGrado);
            cmd.Parameters.AddWithValue("@OdPrisma2", (OdPrisma2 == "" ? (Double?)0.00 : Convert.ToDouble(OdPrisma2.ToString())));
            cmd.Parameters.AddWithValue("@OdGrado2", OdGrado2);
            cmd.Parameters.AddWithValue("@OiPrisma", (OiPrisma == "" ? (Double?)0.00 : Convert.ToDouble(OiPrisma.ToString())));
            cmd.Parameters.AddWithValue("@OiGrado", OiGrado);
            cmd.Parameters.AddWithValue("@OiPrisma2", (OiPrisma2 == "" ? (Double?)0.00 : Convert.ToDouble(OiPrisma2.ToString())));
            cmd.Parameters.AddWithValue("@OiGrado2", OiGrado2);
            cmd.Parameters.AddWithValue("@Horizontal", (Horizontal == "" ? (Double?)0.00 : Convert.ToDouble(Horizontal.ToString())));
            cmd.Parameters.AddWithValue("@Vertical", (Vertical == "" ? (Double?)0.00 : Convert.ToDouble(Vertical.ToString())));
            cmd.Parameters.AddWithValue("@Maxima", (Maxima == "" ? (Double?)0.00 : Convert.ToDouble(Maxima.ToString())));
            cmd.Parameters.AddWithValue("@Puente", (Puente == "" ? (Double?)0.00 : Convert.ToDouble(Puente.ToString())));

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            string respuesta = dt.Rows[0]["respuesta"].ToString();
            return (respuesta);

        }

        public DataSet DETALLEFACTURAFISCAL(string NumeroOrdenImprimir, SqlCommand command)
        {

            //try
            //{
                SqlCommand cmd = command;
                cmd.Parameters.Clear();
                cmd.CommandText = "SP_DETALLEFACTURAFISCAL";
                //SqlCommand cmd = new SqlCommand("SP_DETALLEFACTURAFISCAL", cn.LeerCadena());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NumOrden", NumeroOrdenImprimir);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet dts = new DataSet();
                da.Fill(dts);
                cmd.Parameters.Clear();
                return dts;
            //}

            // catch (Exception ex)
            //{
            //    string Error = string.Format("Error: {0}", ex.Message);
            //    MessageBox.Show(Error);
            //    DataSet dts = new DataSet();
            //    return dts;

            //}

        }

        public DataSet PagosConIGTF(string SucursalActual, string NroOrdenServicio, string Revision, SqlCommand command)
        {
            SqlCommand cmd = command;
            cmd.CommandText = "SP_CPOS_pGetPagosConIGTF";
            //SqlCommand cmd = new SqlCommand("SP_CPOS_pGetPagosConIGTF", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodSuc", SucursalActual);
            cmd.Parameters.AddWithValue("@NumordServ", NroOrdenServicio);
            cmd.Parameters.AddWithValue("@Revision", Revision);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet dts = new DataSet();
            da.Fill(dts);
            cmd.Parameters.Clear();
            return dts;
        }

        //Solo para hacer la validacion de factura manual. 
        public DataSet PagosConIGTFVal(string SucursalActual, string NroOrdenServicio, SqlCommand command = null)
        {
            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }
            string Sucursal = _D_Inicio.Sucursal();
            SqlCommand cmd = command;
            cmd.CommandText = ("pGetPagosConIGTF");
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CodSuc", SucursalActual);
            cmd.Parameters.AddWithValue("@NumordServ", NroOrdenServicio);
            cmd.Parameters.AddWithValue("@Revision", "0");
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet dts = new DataSet();
            da.Fill(dts);
            cmd.Parameters.Clear();
            return dts;

        }

        //Para hacer la nueva validacion de factura manual agregado el 25-01-2024. 
        public string Validar_Factura_Manual(string Parametro= "FactManual", SqlCommand command = null)
        {
            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }

            SqlCommand cmd = new SqlCommand("SELECT Valor from  TB_PARAMETRO where Parametro= @Parametro", cn.LeerCadena());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Parametro", Parametro);
            cmd.CommandTimeout = 120;
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt.Rows[0]["Valor"].ToString();
        }

        public DataSet FacturaManual(string SucursalActual, string NroOrdenServicio)
        {
            SqlCommand cmd = new SqlCommand("pGetFacturaManual", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NumordServ", NroOrdenServicio);
            cmd.Parameters.AddWithValue("@suc", SucursalActual);
            cmd.Parameters.AddWithValue("@CodBanco", "");
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet dts = new DataSet();
            da.Fill(dts);
            return dts;
        }

        public DataTable TEMP_ABONO(string Sucursal, string NumeroOrdenImprimir, string Revision, SqlCommand command= null)
        {

            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();

                SqlCommand cmd = command;
                cmd.CommandText = "select * from TB_ABONO where NumOrdserv = @NumeroOrdenImprimir and Anulado= 0 and Cod_Sucursal=@Sucursal and Revision= @Revision ";

                //SqlCommand cmd = new SqlCommand("select * from TEMP_ABONO where NumOrdserv = @NumeroOrdenImprimir and Anulado= 0", cn.LeerCadena());
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@NumeroOrdenImprimir", NumeroOrdenImprimir);
                cmd.Parameters.AddWithValue("@Sucursal", Sucursal);
                cmd.Parameters.AddWithValue("@Revision", Revision);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cmd.Parameters.Clear();
                return dt;
            }

            else
            { 
            SqlCommand cmd = command;
            cmd.CommandText = "select * from TEMP_ABONO where NumOrdserv = @NumeroOrdenImprimir and Anulado= 0";

            //SqlCommand cmd = new SqlCommand("select * from TEMP_ABONO where NumOrdserv = @NumeroOrdenImprimir and Anulado= 0", cn.LeerCadena());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@NumeroOrdenImprimir", NumeroOrdenImprimir);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            cmd.Parameters.Clear();
            return dt;
        }
    }

        public DataSet DESCUENTOSFACTURAFISCAL(string NumeroOrdenImprimir, SqlCommand command)
        {
            SqlCommand cmd = command;
            cmd.CommandText = "SP_DESCUENTOSFACTURAFISCAL";

            //SqlCommand cmd = new SqlCommand("SP_DESCUENTOSFACTURAFISCAL", cn.LeerCadena());

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NumOrden", NumeroOrdenImprimir);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet dts = new DataSet();
            da.Fill(dts);
            cmd.Parameters.Clear();
            return dts;
        }


        public void Delete_Sencillo(string Tabla, string Condicion)
        {
            SqlCommand cmd = new SqlCommand("Delete_sencillo", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Tabla", Tabla);
            cmd.Parameters.AddWithValue("@Condicion ", Condicion);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet dts = new DataSet();
            da.Fill(dts);
        }

        public DataTable Pasgos(string SucursalActual, string NroOrdenServicio, string Revision , SqlCommand command)
        {
            SqlCommand cmd = command;
            cmd.CommandText = "SP_CPOS_pGetPagos";
            
            //SqlCommand cmd = new SqlCommand("SP_CPOS_pGetPagos", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodSuc", SucursalActual);
            cmd.Parameters.AddWithValue("@NumordServ", NroOrdenServicio);
            cmd.Parameters.AddWithValue("@Revision", Revision);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmd.Parameters.Clear();
            return dt;

        }

        public string TB_PARAMETROSPGE(string Parametro)
        {
            SqlCommand cmd = new SqlCommand("SELECT Valor from  TB_PARAMETROSPGE where ParametroPGE= @Parametro", cn.LeerCadena());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Parametro", Parametro);
            cmd.CommandTimeout = 120;
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            cmd.Parameters.Clear();
            string Valor = dt.Rows[0]["Valor"].ToString();
            return Valor;

        }

        public string TB_PARAMETRO(string Parametro)
        {
            SqlCommand cmd = new SqlCommand("SELECT Valor from  TB_PARAMETRO where Parametro= @Parametro", cn.LeerCadena());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Parametro", Parametro);
            cmd.CommandTimeout = 120;
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            cmd.Parameters.Clear();
            string Valor = dt.Rows[0]["Valor"].ToString();
            return Valor;

        }

        public string TB_PARAMETROSPAIS(string Parametro)
        {
            SqlCommand cmd = new SqlCommand("SELECT Venezuela from  TB_PARAMETROSPAIS where Parametro= @Parametro", cn.LeerCadena());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Parametro", Parametro);
            cmd.CommandTimeout = 120;
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            cmd.Parameters.Clear();
            string Valor = dt.Rows[0]["Venezuela"].ToString();
            return Valor;

        }

        public DataTable TB_INUTILIZADO()
        {
            SqlCommand cmd = new SqlCommand("select * from TB_INUTILIZADO", cn.LeerCadena());
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;

        }

        public void SP_SUMOFACTURA(string CodSuc, string NumFactura, string Fact_SerialImpresora, string NumOs, string MontoFactImpreso, string MontoDescExento, string MontoDescGravable, SqlCommand command = null)
        {
            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }
            SqlCommand cmd = command;
            cmd.CommandText = ("SP_SUMOFACTURA");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodSuc", CodSuc);
            cmd.Parameters.AddWithValue("@NumFactura ", NumFactura);
            cmd.Parameters.AddWithValue("@Fact_SerialImpresora", Fact_SerialImpresora);
            cmd.Parameters.AddWithValue("@NumOS ", NumOs);
            cmd.Parameters.AddWithValue("@MontoFactImpreso", MontoFactImpreso);
            cmd.Parameters.AddWithValue("@MontoDescExento", MontoDescExento);
            cmd.Parameters.AddWithValue("@MontoDescGravable", MontoDescGravable);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmd.Parameters.Clear();

        }

        public string GetFactura(string Cod_Sucursal, string Fact_Num, string Fecha, string CTE_NacioPAG, string CTE_CedIdenPAG, string COD_Empleado, string COD_VTA, string NumOrdServ, string Fact_FecOfecido,
        string Fact_HoraOfrecido, double Fact_SubTotal, double Fact_Impuesto, double Fact_Descuento, double Fact_Total, string USER_Crea, double IvaRetenido, double ISLRRetenido, string Fact_SerialImpresora,
        double Fact_MontoExento, double Fact_MontoGravable, double MontoReintegroIva, double Fact_IGTF,string Fact_Status, SqlCommand command, bool Facturamanual= false, string Fact_NumCtrol= "")
        {
            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }
            SqlCommand cmd = command;
            cmd.Parameters.Clear();
            cmd.CommandText = "SP_CPOS_GET_TbFactura";
            //SqlCommand cmd = new SqlCommand("SP_CPOS_GET_TbFactura", cn.LeerCadena());

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Cod_Sucursal", Cod_Sucursal);
            cmd.Parameters.AddWithValue("@Fact_Num", Fact_Num);
            cmd.Parameters.AddWithValue("@Fecha", Fecha);
            cmd.Parameters.AddWithValue("@CTE_NacioPAG", CTE_NacioPAG);
            cmd.Parameters.AddWithValue("@CTE_CedIdenPAG", CTE_CedIdenPAG);
            cmd.Parameters.AddWithValue("@COD_Empleado", COD_Empleado);
            cmd.Parameters.AddWithValue("@COD_VTA", COD_VTA);
            cmd.Parameters.AddWithValue("@NumOrdServ", NumOrdServ);
            cmd.Parameters.AddWithValue("@Fact_FecOfecido", Fact_FecOfecido);
            cmd.Parameters.AddWithValue("@Fact_HoraOfrecido", Fact_HoraOfrecido);
            cmd.Parameters.AddWithValue("@Fact_SubTotal", Fact_SubTotal);
            cmd.Parameters.AddWithValue("@Fact_Impuesto", Fact_Impuesto);
            cmd.Parameters.AddWithValue("@Fact_Descuento", Fact_Descuento);
            cmd.Parameters.AddWithValue("@Fact_Total", Fact_Total);
            cmd.Parameters.AddWithValue("@USER_Crea", USER_Crea);
            cmd.Parameters.AddWithValue("@IvaRetenido", IvaRetenido);
            cmd.Parameters.AddWithValue("@ISLRRetenido", ISLRRetenido);
            cmd.Parameters.AddWithValue("@Fact_SerialImpresora", Fact_SerialImpresora);
            cmd.Parameters.AddWithValue("@Fact_MontoExento", Fact_MontoExento);
            cmd.Parameters.AddWithValue("@Fact_MontoGravable ", Fact_MontoGravable);
            cmd.Parameters.AddWithValue("@MontoReintegroIva", MontoReintegroIva);
            cmd.Parameters.AddWithValue("@Fact_IGTF", Fact_IGTF);
            cmd.Parameters.AddWithValue("@Fact_Status", Fact_Status);
            cmd.Parameters.AddWithValue("@FactManual", Facturamanual);
            cmd.Parameters.AddWithValue("@Fact_NumCtrol", Fact_NumCtrol);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            string resultado = dt.Rows[0]["resultado"].ToString();
            cmd.Parameters.Clear();
            return resultado;

        }


        public string GetAbono(string Cod_Sucursal, string NumOrdserv, string Revision, string Tipo_Pago, string Cod_Banco, string Abo_CTATARJETA, string Abo_CVCNROCHEQUE, string Abo_Fecha, double Abo_Monto, string Abo_Tipo,
    string Tipo_Pto, string CodPunto, string Anulado, string Fec_Crea, string Fec_Mod, string USER_Crea, string USER_Mod, string Fecha, string Fecha_Abono, string Cod_BancoRecep, Double Tasa_Abono, Double Abo_Monto_Divisa, string Abo_Monto_SinIGTF,
    string Abo_IGTF, string OrSer_Tipo_Mon, Double Tasa_Dolar, Double recibidoREF)
        {
            try
            {

                SqlCommand cmd = new SqlCommand( "SP_CPOS_GET_ABONO", cn.LeerCadena());
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
                cmd.Parameters.AddWithValue("@ANULADO", 0);
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
                cmd.Parameters.AddWithValue("@Tasa_Dolar", Tasa_Dolar);
                cmd.Parameters.AddWithValue("@RecibidoREF", recibidoREF);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cmd.Parameters.Clear();
                string Valor = dt.Rows[0]["resultado"].ToString();
                return Valor;

            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return null;
            }

        }


        public DataTable CargarPagosGrid(string sucursal, string orden, string Revision)
        {
            SqlCommand cmd = new SqlCommand("SELECT Fecha, CASE WHEN Tipo_Pago = '021'  and (Cod_Banco = '101'or Cod_Banco = '102' or Cod_Banco = '103' or Cod_Banco = '104' or  Cod_Banco = '105' or Cod_Banco = '106'or Cod_Banco = '107' or Cod_Banco = '108' or Cod_Banco = '109' or  Cod_Banco = '111') THEN 'TRANSFERENCIA DIVISA' WHEN Tipo_Pago = '007' THEN 'TARJETA DE CREDITO ' WHEN Cod_Banco = '110' AND Tipo_Pago = '021' THEN 'CASHEA' WHEN Cod_Banco = '007' AND Tipo_Pago = '021' THEN 'EFECTIVO DIVISA' WHEN Cod_Banco = '007' AND Tipo_Pago = '022' THEN 'EFECTIVO DIVISA' ELSE Abo_Tipo END as Abo_Tipo , Abo_Monto, Tipo_Pago, Fec_Crea, ID_Abono FROM TB_ABONO WHERE Cod_Sucursal = @sucursal and NumOrdserv = @orden and Revision = @revision and Anulado = 0", cn.LeerCadena());
            
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@sucursal", sucursal);
            cmd.Parameters.AddWithValue("@orden", orden);
            cmd.Parameters.AddWithValue("@revision", Revision);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;

        }


        public string RecalculaOSDivisa(string Cod_Sucursal, string NumOrdserv, string Revision)
        {
            SqlCommand cmd = new SqlCommand("SP_CPOS_pRecalculaOSDivisa", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodSuc", Cod_Sucursal);
            cmd.Parameters.AddWithValue("@NumordServ", NumOrdserv);
            cmd.Parameters.AddWithValue("@Revision", Revision);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            string Valor = dt.Rows[0]["Resultado"].ToString();
            return Valor;


        }

        public void RecalculaOPorpagar(string Cod_Sucursal, string NumOrdserv, string Revision)
        {
            SqlCommand cmd = new SqlCommand("SP_CPOS_pRecalculaOSPorPagar", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodSuc", Cod_Sucursal);
            cmd.Parameters.AddWithValue("@NumordServ", NumOrdserv);
            cmd.Parameters.AddWithValue("@Revision", Revision);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

        }

        public string RegistarNota(double Bolivares, string NumeroNota, string NumerFact, string cedulaCliente, SqlCommand command)
        {
            try
            {
                SqlCommand cmd = command;
                cmd.CommandText = ("UPDATE TB_NOTASCREDITODEBITO SET  MontoAplicado = MontoAplicado + @Bolivares where NRONOTA=@NumeroNota and Fact_Num= @NumerFact and CTE_CedIden= @cedulaCliente ");
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Bolivares", Bolivares);
                cmd.Parameters.AddWithValue("@NumeroNota", NumeroNota);
                cmd.Parameters.AddWithValue("@NumerFact", NumerFact);
                cmd.Parameters.AddWithValue("@cedulaCliente", cedulaCliente);
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

        public DataTable BucarFactura(string NumeroFactura, string SerialImpresora, string SucursalActual, SqlCommand command = null)
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

                cmd.CommandText ="SP_CPOS_BucarFactura";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NumeroFactura", NumeroFactura);
            cmd.Parameters.AddWithValue("@SerialImpresora", SerialImpresora);
            cmd.Parameters.AddWithValue("@SucursalActual", SucursalActual);

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
        public DataTable BucarTB_CTEPPAL(string CTE_CedIden, string CTE_Nacio, SqlCommand command = null)
        {
            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }
            SqlCommand cmd = command;
            cmd.CommandText =" SELECT* from TB_CTEPPAL where CTE_CedIden= @CTE_CedIden and CTE_Nacio= @CTE_Nacio";
            cmd.CommandTimeout = 120;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@CTE_CedIden", CTE_CedIden);
            cmd.Parameters.AddWithValue("@CTE_Nacio", CTE_Nacio);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            cmd.Parameters.Clear();
            return dt;


        }

        public Double MontoMinimoAbono(SqlCommand command = null)
        {
                if (command == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command = connection.CreateCommand();
                }

            string Parametro = "PorcAbonoOS";
            SqlCommand cmd = command;
            cmd.CommandText ="select top(1) Valor FROM TB_PARAMETRO where Parametro=@Parametro ";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Parametro", Parametro);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            string Valor1 = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", Convert.ToDouble(dt.Rows[0]["Valor"].ToString()) / 100).Replace(".", ",");
            Double ValorTotal = Convert.ToDouble(Valor1);
            cmd.Parameters.Clear();
            return ValorTotal;

}


public void MovimientoInventario(string CodArticulo, string TipoDoc, string Documento, string CantidadArt, string Costo, string Precio, string Movimiento, string Usuario, string sucursal, string Fecha, string os, SqlCommand command = null)
        {
            try
            {

                if (command== null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }
            SqlCommand cmd = command;
            cmd.CommandText = "SP_MOVIMIENTOINVENTARIO";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codArticulo", CodArticulo);
            cmd.Parameters.AddWithValue("@TipoDoc", TipoDoc);
            cmd.Parameters.AddWithValue("@documento", Documento);
            cmd.Parameters.AddWithValue("@cantidad", CantidadArt);
            cmd.Parameters.AddWithValue("@costo", Costo);
            cmd.Parameters.AddWithValue("@precio", Precio);
            cmd.Parameters.AddWithValue("@Movimiento", Movimiento);
            cmd.Parameters.AddWithValue("@Usuario", Usuario);
            cmd.Parameters.AddWithValue("@Sucursal", sucursal);
            cmd.Parameters.AddWithValue("@FechaActiva", Fecha);
            cmd.Parameters.AddWithValue("@OS", os);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            cmd.Parameters.Clear();


            MovimientoInv = dt.Rows[0]["EJECUTAMOVIMIENTO"].ToString();
            }
            catch (Exception ex)
            {
                MovimientoInv = string.Format("Error: {0}", ex.Message);
            }
        }

        public DataSet Verificar_Existencia_Inventario(string NroOrden, SqlCommand command = null)
        {
            try
            {

                if (command == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command = connection.CreateCommand();
                }
                SqlCommand cmd = command;
                cmd.CommandText = "pValidaExistenciaArticulo";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OS", NroOrden);
                cmd.CommandTimeout = 120;
                DataSet dts = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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
        

        public DataSet DetalleNotaCreditoFiscal(string NumeroFactura, string SerialImpresora, SqlCommand command = null)
        {
            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }
            SqlCommand cmd = command;
            cmd.CommandText ="SP_DETALLENOTACREDITOFISCAL";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NumFact", NumeroFactura);
            cmd.Parameters.AddWithValue("@SerialImpresora", SerialImpresora);

            DataSet dts = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dts);
            cmd.Parameters.Clear();
            return dts;


        }
        public DataSet DescuentosNotaCreditoFiscal(string NumeroOrdenServicio, SqlCommand command = null)
        {
            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }
            SqlCommand cmd = command;
            cmd.CommandText ="SP_DESCUENTOSFACTURAFISCAL";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NumOrden", NumeroOrdenServicio);
            DataSet dts = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dts);
            cmd.Parameters.Clear();
            return dts;


        }

        public DataSet IGTF_NotaCreditoFiscal(string NumeroOrdenServicio, SqlCommand command = null)
        {
            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }
            SqlCommand cmd = command;
            cmd.CommandText = "SP_IGTF_Factura";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Cod_Sucursal", NumeroOrdenServicio);
            cmd.Parameters.AddWithValue("@NumOrden", NumeroOrdenServicio);
            cmd.Parameters.AddWithValue("@NumOrden", NumeroOrdenServicio);
            DataSet dts = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dts);
            cmd.Parameters.Clear();
            return dts;


        }

        public void ObtenerFactura(string NumeroOrden)
        {
            try
            {
                string Sucursal = _D_Inicio.Sucursal();

                SqlCommand cmd = new SqlCommand("select * from TB_FACTURAS where Cod_Sucursal=@Sucursal and NumOrdServ=@NumeroOrden", cn.LeerCadena());
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Sucursal", Sucursal);
                cmd.Parameters.AddWithValue("@NumeroOrden", NumeroOrden);
                cmd.CommandTimeout = 120;
                SqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.HasRows)
                {

                    while (dataReader.Read())
                    {
                        TB_FACTURAS.Cod_Sucursal = Convert.ToString(dataReader["Cod_Sucursal"]);
                        TB_FACTURAS.Fact_Num = Convert.ToString(dataReader["Fact_Num"]);
                        TB_FACTURAS.Fecha = dataReader["Fecha"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["Fecha"]);
                        TB_FACTURAS.Fact_NumCtrol = Convert.ToString(dataReader["Fact_NumCtrol"]);
                        TB_FACTURAS.CTE_NacioPAG = Convert.ToString(dataReader["CTE_NacioPAG"]);
                        TB_FACTURAS.CTE_CedIdenPAG = Convert.ToString(dataReader["CTE_CedIdenPAG"]);
                        TB_FACTURAS.COD_Empleado = Convert.ToString(dataReader["COD_Empleado"]);
                        TB_FACTURAS.COD_VTA = Convert.ToString(dataReader["COD_VTA"]);
                        TB_FACTURAS.NumOrdServ = Convert.ToString(dataReader["NumOrdServ"]);
                        TB_FACTURAS.Revision = Convert.ToString(dataReader["Revision"]);
                        TB_FACTURAS.Fact_FecOfecido = Convert.ToDateTime(dataReader["Fact_FecOfecido"].ToString());
                        TB_FACTURAS.Fact_HoraOfrecido = Convert.ToString(dataReader["Fact_HoraOfrecido"]);
                        TB_FACTURAS.Fact_SubTotal = Convert.ToDouble(dataReader["Fact_SubTotal"]);
                        TB_FACTURAS.Fact_Impuesto = Convert.ToDouble(dataReader["Fact_Impuesto"]);
                        TB_FACTURAS.Fact_Descuento = Convert.ToDouble(dataReader["Fact_Descuento"]);
                        TB_FACTURAS.Fact_Total = Convert.ToDouble(dataReader["Fact_Total"]);
                        TB_FACTURAS.Fact_Status = Convert.ToString(dataReader["Fact_Status"]);
                        TB_FACTURAS.Fact_FecCrea = Convert.ToDateTime(dataReader["Fact_FecCrea"].ToString());
                        TB_FACTURAS.Fact_FecMod = dataReader["Fact_FecMod"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["Fact_FecMod"]);
                        TB_FACTURAS.USER_Crea = Convert.ToString(dataReader["USER_Crea"]);
                        TB_FACTURAS.USER_Mod = dataReader["USER_Mod"] == DBNull.Value ? (String)null : Convert.ToString(dataReader["USER_Mod"]);
                        TB_FACTURAS.Anulado = dataReader["Anulado"] == DBNull.Value ? (Boolean)false : Convert.ToBoolean(dataReader["Anulado"]);
                        TB_FACTURAS.Nota = dataReader["Nota"] == DBNull.Value ? (Boolean)false : Convert.ToBoolean(dataReader["Nota"]);
                        TB_FACTURAS.IvaRetenido = dataReader["IvaRetenido"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(dataReader["IvaRetenido"]);
                        TB_FACTURAS.ComprobRetencionIva = Convert.ToString(dataReader["ComprobRetencionIva"]);
                        TB_FACTURAS.ISLRRetenido = dataReader["ISLRRetenido"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(dataReader["ISLRRetenido"]);
                        TB_FACTURAS.ComprobRetencionISLR = Convert.ToString(dataReader["ComprobRetencionISLR"]);
                        TB_FACTURAS.FechaRegistroComprobISLR = dataReader["FechaRegistroComprobISLR"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["FechaRegistroComprobISLR"]);
                        TB_FACTURAS.FechaRegistroComprobIVA = dataReader["FechaRegistroComprobIVA"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["FechaRegistroComprobIVA"]);
                        TB_FACTURAS.Cod_TipoNControl = Convert.ToString(dataReader["Cod_TipoNControl"]);
                        TB_FACTURAS.Fact_SerialImpresora = Convert.ToString(dataReader["Fact_SerialImpresora"]);
                        TB_FACTURAS.Fact_MontoExento = dataReader["Fact_MontoExento"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(dataReader["Fact_MontoExento"]);
                        TB_FACTURAS.Fact_MontoGravable = dataReader["Fact_MontoGravable"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(dataReader["Fact_MontoGravable"]);
                        TB_FACTURAS.Fact_AlicuotaIva = dataReader["Fact_AlicuotaIva"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(dataReader["Fact_AlicuotaIva"]);
                        TB_FACTURAS.NCF = Convert.ToString(dataReader["NCF"]);
                        TB_FACTURAS.CodDocVta = Convert.ToString(dataReader["CodDocVta"]);
                        TB_FACTURAS.MontoReintegroIva = dataReader["MontoReintegroIva"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(String.Format(CultureInfo.InvariantCulture, "{0:0.00}", dataReader["MontoReintegroIva"].ToString()));
                        TB_FACTURAS.Fact_IGTF = dataReader["Fact_IGTF"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(dataReader["Fact_IGTF"]);
                        TB_FACTURAS.Fact_AlicuotaIGTF = dataReader["Fact_AlicuotaIGTF"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(dataReader["Fact_AlicuotaIGTF"]);
                    }

                }
            }
            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);

            }
        }

        public string RegistrarNCFISCAL(string Cod_Sucursal, string NumeroNCFiscal, string Tipo, string Factura, string Fact_SerialImpresora, string Fecha, string NC_SerialImpresora,
        string Control, string NacionalidadCliente, string CedulaCliente, string Motivo, double Monto, double MontoActual, string Usuario, string TipoNC, double MontoIGTF, double AlicuotaIGTF, double MontoExento , bool NCManual= false, string NC_NumCtrol = "", SqlCommand command = null)
        {
            try
            {
                if (command == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command = connection.CreateCommand();
                }
                SqlCommand cmd = command;
                cmd.CommandText ="SP_CPOS_GET_NotaCreditoFiscal";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Cod_Sucursal", Cod_Sucursal);
                cmd.Parameters.AddWithValue("@NumeroNCFiscal", NumeroNCFiscal);
                cmd.Parameters.AddWithValue("@Tipo", Tipo);
                cmd.Parameters.AddWithValue("@Factura", Factura);
                cmd.Parameters.AddWithValue("@Fact_SerialImpresora", Fact_SerialImpresora);
                cmd.Parameters.AddWithValue("@Fecha", Fecha);
                cmd.Parameters.AddWithValue("@NC_SerialImpresora", NC_SerialImpresora);
                cmd.Parameters.AddWithValue("@Control", Control);
                cmd.Parameters.AddWithValue("@NacionalidadCliente", NacionalidadCliente);
                cmd.Parameters.AddWithValue("@CedulaCliente", CedulaCliente);
                cmd.Parameters.AddWithValue("@Motivo", Motivo);
                cmd.Parameters.AddWithValue("@MontoGravable", Monto);
                cmd.Parameters.AddWithValue("@MontoActual", MontoActual);
                cmd.Parameters.AddWithValue("@Usuario", Usuario);
                cmd.Parameters.AddWithValue("@TipoNC", TipoNC);
                cmd.Parameters.AddWithValue("@MontoIGTF", MontoIGTF);
                cmd.Parameters.AddWithValue("@AlicuotaIGTF", AlicuotaIGTF);
                cmd.Parameters.AddWithValue("@MontoExento", MontoExento);
                cmd.Parameters.AddWithValue("@NCManual", NCManual);
                cmd.Parameters.AddWithValue("@NC_NumCtrol", NC_NumCtrol);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                string resultado = dt.Rows[0]["resultado"].ToString();
                cmd.Parameters.Clear();
                return resultado;
            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return Error;

            }
        }


        public string ParametroImpresion()
        {
            SqlDataAdapter da = new SqlDataAdapter("if EXISTS (select Valor from TB_PARAMETRO where Parametro= 'Imprimir') select Valor from TB_PARAMETRO where Parametro= 'Imprimir' else (select '1' AS Valor)", cn.LeerCadena());
            da.SelectCommand.CommandType = CommandType.Text;
            DataTable dt = new DataTable();
            da.Fill(dt);
            string imprimir = dt.Rows[0]["Valor"].ToString();

            return imprimir;


        }

        public string ParametroSerieManual()
        {
            SqlDataAdapter da = new SqlDataAdapter(" select Valor from TB_PARAMETRO where Parametro= 'SerieManual'", cn.LeerCadena());
            da.SelectCommand.CommandType = CommandType.Text;
            DataTable dt = new DataTable();
            da.Fill(dt);
            string SerieManual = dt.Rows[0]["Valor"].ToString();

            return SerieManual;

        }

        public DataSet GetFactManual(string NroOrden, string CodBanco, SqlCommand command= null)
        {
            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }
            string Sucursal = _D_Inicio.Sucursal();
            SqlCommand cmd = command;
            cmd.CommandText = ("pGetFacturaManual");
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@NumordServ", NroOrden);
            cmd.Parameters.AddWithValue("@suc", Sucursal);
            cmd.Parameters.AddWithValue("@CodBanco", CodBanco);
            DataSet dts = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dts);
            cmd.Parameters.Clear();
            return dts;

        }



        public DataTable ObtenerFacturaHijo(string NumeroOrden)
        {
            string Sucursal = _D_Inicio.Sucursal();

            SqlCommand cmd = new SqlCommand("select * from TB_FACTURAS where Cod_Sucursal=@Sucursal and NumOrdServ=@NumeroOrden and Revision= @Revision", cn.LeerCadena());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Sucursal", Sucursal);
            cmd.Parameters.AddWithValue("@NumeroOrden", NumeroOrden);
            cmd.Parameters.AddWithValue("@Revision", "0");
            cmd.CommandTimeout = 120;
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        public DataTable ObtenerOrdenHijo(string NumeroOrden)
        {
            string Sucursal = _D_Inicio.Sucursal();

            SqlCommand cmd = new SqlCommand("SP_CPOS_Datos_de_la_Orden", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Sucursal", Sucursal);
            cmd.Parameters.AddWithValue("@NumeroOrden", NumeroOrden);
            cmd.Parameters.AddWithValue("@Revison", "0");
            cmd.CommandTimeout = 120;
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        public DataSet VerificarExistenciaNotaCredito(string NumeroFactura, string SerialImpresoraFct)
        {
            SqlCommand cmd = new SqlCommand("pGetNCconFactura", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FAC", NumeroFactura);
            cmd.Parameters.AddWithValue("@SERIALFACT", SerialImpresoraFct);
            cmd.CommandTimeout = 120;
            DataSet dts = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dts);
            return dts;


        }

        public DataSet MovimientosAnulacionOSLC(string NumeroOrdenServicio, string TipoDocumento, string CodigoDc, string DIAACT, string NOTAC, string USER)
        {
            string Sucursal = _D_Inicio.Sucursal();

            SqlCommand cmd = new SqlCommand("pMovimientosAnulacionOSLC", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OS", NumeroOrdenServicio);
            cmd.Parameters.AddWithValue("@suc", Sucursal);
            cmd.Parameters.AddWithValue("@TIPDOC", TipoDocumento);
            cmd.Parameters.AddWithValue("@CODDOC", CodigoDc);
            cmd.Parameters.AddWithValue("@DIAACT", DIAACT);
            cmd.Parameters.AddWithValue("@NOTAC", NOTAC);
            cmd.Parameters.AddWithValue("@USER", USER);


            DataSet dts = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dts);
            return dts;


        }

        public string ActualizarCaorser(string CodMotivo, string Cod_ResponsableAnu, string NumOrden, string REVISION, string Usuario, SqlCommand command = null)
        {
            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }

            string Sucursal = _D_Inicio.Sucursal();
            SqlCommand cmd = command;
            cmd.CommandText ="SP_CPOS_GET_ActualizarCaorser";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Cod_Sucursal", Sucursal);
            cmd.Parameters.AddWithValue("@CodMotivo", CodMotivo);
            cmd.Parameters.AddWithValue("@Cod_ResponsableAnu", Cod_ResponsableAnu);
            cmd.Parameters.AddWithValue("@NumOrden", NumOrden);
            cmd.Parameters.AddWithValue("@REVISION", REVISION);
            cmd.Parameters.AddWithValue("@Usuario", Usuario);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            string resultado = dt.Rows[0]["resultado"].ToString();
            cmd.Parameters.Clear();
            return resultado;


        }

        public void PostFactManual(string NroFact, string NroOrden, string SerialImpre, string Pais, SqlCommand command= null)
        {
            if (command == null)
            {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
            }
            string Sucursal = _D_Inicio.Sucursal();
            SqlCommand cmd = command;
            cmd.CommandText = ("SP_CALCULAMONTOSFACTURA");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FACT", NroFact);
            cmd.Parameters.AddWithValue("@NumOrden", NroOrden);
            cmd.Parameters.AddWithValue("@SERIALIMP", SerialImpre);
            cmd.Parameters.AddWithValue("@PAIS", Pais);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            cmd.Parameters.Clear();

        }

        public DataSet OptenerExamen(string Nacionalidad, string Cedula, int NumeroExamen, string Sucursal, SqlCommand command)
        {     
            try
            {
                SqlCommand cmd = command;
                cmd.CommandText = ("SP_CPOS_OptenerExamen");

                //SqlCommand cmd = new SqlCommand("SP_CPOS_OptenerExamen", cn.LeerCadena());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nacionalidad", Nacionalidad);
                cmd.Parameters.AddWithValue("@Cedula", Cedula);
                cmd.Parameters.AddWithValue("@NumeroExamen", NumeroExamen);
                cmd.Parameters.AddWithValue("@sucursal", Sucursal);

                DataSet Examen = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(Examen);
                cmd.Parameters.Clear();
                return Examen;
            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return null;
            }


        }
        public DataTable ObtenerClientePagador( string Cedula)
        {
            SqlCommand cmd = new SqlCommand("SP_CPOS_GET_CLIENTE_PAGADOR", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CEDULA", Cedula);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                ClientePagador = false;
            }
            else
            {
                ClientePagador = true;
                Nombre = dt.Rows[0]["NOMBRE"].ToString();
                CED = dt.Rows[0]["CEDULA"].ToString();
                Tlf = dt.Rows[0]["Telefono"].ToString();
                Correo = dt.Rows[0]["Correo"].ToString();


            }

            return dt;

        }

        public DataTable ObtenerTlfCorreo(string NumOrden)
        {
            SqlCommand cmd = new SqlCommand("SP_CPOS_GET_TLF_CORREO", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NumOrden", NumOrden);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            
            Tlf = dt.Rows[0]["Telefono"].ToString();
            Correo = dt.Rows[0]["Correo"].ToString();

            return dt;


        }

        public DataSet ActivarGarantia(string NUMORDSERV, string SUC, string USER)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("pAddGarantia", cn.LeerCadena());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NUMORDSERV", NUMORDSERV);
                cmd.Parameters.AddWithValue("@SUC", SUC);
                cmd.Parameters.AddWithValue("@USER", USER);
                DataSet Garantia = new DataSet(); 
                 SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(Garantia);

                return Garantia;
            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return null;
            }


        }


        public DataTable RevisarAbonosIGTF_Epos(string Sucursal, string NumeroOrden, string REVISION)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("select * from TB_ABONO where Tipo_Pago = '021' and Cod_Sucursal = @COD_SUCURSAL and NumOrdserv= @NumOrden and Revision = @REVISION", cn.LeerCadena());
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@COD_SUCURSAL", Sucursal);
                cmd.Parameters.AddWithValue("@NumOrden", NumeroOrden);
                cmd.Parameters.AddWithValue("@REVISION", REVISION);

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


        public void ActualizarIgtf(string Sucursal, string NumeroOrden, string REVISION, string Usuario)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_CPOS_Actualizar_IGTF_Epos", cn.LeerCadena());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COD_SUCURSAL", Sucursal);
                cmd.Parameters.AddWithValue("@NumOrden", NumeroOrden);
                cmd.Parameters.AddWithValue("@REVISION ", REVISION);
                cmd.Parameters.AddWithValue("@User_Crea ", Usuario);
                DataSet Garantia = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(Garantia);
            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
            }


        }

        public DataSet ACTUALIZA_FECHAOFREC(string SucursalActual, string NroOrdenServicio, string Revision, DateTime FechaMaxVenta)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("SP_ACTUALIZA_FECHAOFREC_F12", cn.LeerCadena());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sucursal", SucursalActual);
                cmd.Parameters.AddWithValue("@Orden", NroOrdenServicio);
                cmd.Parameters.AddWithValue("@Revision", Revision);
                cmd.Parameters.AddWithValue("@FechaMaxVenta", FechaMaxVenta);
                cmd.CommandTimeout = 120;
                //DateTime.UtcNow.ToString("yyyy/MM/dd H:mm:00 ")

                DataSet FechaOfre = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(FechaOfre);
                cmd.Parameters.Clear();
                return FechaOfre;
            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return null;
            }


        }

        public string RespaldarDatos(string CodSuc, string NumordServ, string Revision, string Transaccion)
        {

            SqlCommand cmd = new SqlCommand("SP_CPOS_Respaldo", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodSuc", CodSuc);
            cmd.Parameters.AddWithValue("@NumordServ", NumordServ);
            cmd.Parameters.AddWithValue("@Revision", Revision);
            cmd.Parameters.AddWithValue("@Transaccion", Transaccion);


            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            string resultado = dt.Rows[0]["resultado"].ToString();
            return resultado;


        }

        public string RespaldarNota(string CodSuc, string NumeroNota, string NumerFact, string cedulaCliente, string Transaccion)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("SP_CPOS_Respaldo_NotaCredito", cn.LeerCadena());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodSuc", CodSuc);
                cmd.Parameters.AddWithValue("@NRONOTA", NumeroNota);
                cmd.Parameters.AddWithValue("@Fact_Num", NumerFact);
                cmd.Parameters.AddWithValue("@CTE_CedIden", cedulaCliente);
                cmd.Parameters.AddWithValue("@Transaccion", Transaccion);
                
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                string resultado = dt.Rows[0]["resultado"].ToString();
                return resultado;
            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return Error;
            }
        }

        public DataTable Punto(string Tipo_punto)
        {
            SqlCommand cmd = new SqlCommand("select CodPunto as Value, Descripcion as Indexx from PtoVenta_Puntos where  Status= 'A' and  Tipo=@Tipo  order by Indexx ASC", cn.LeerCadena());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Tipo", Tipo_punto);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;

        }
        //Comentar
        public DataTable BuscarNotaDevolucion(string numDocumento, bool nota, string cedulaCliente)
        {
            string CodSuc = _D_Inicio.Sucursal();


            SqlCommand cmd = new SqlCommand("SP_CPOS_BuscarNotaDevolucion", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@numDocumento", numDocumento);
            cmd.Parameters.AddWithValue("@Nota", nota);
            cmd.Parameters.AddWithValue("@cedulaCliente", cedulaCliente);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;

        }
        //Comentar
        public DataTable ExistenNotasDevolucion(string cedulaCliente)
        {

            SqlCommand cmd = new SqlCommand("SELECT NRONOTA,NumOrdserv,Motivo, SaldoNota from  TB_NOTASDEVOLUCION where CTE_CedIden= @cedulaCliente and Anulado= 0 ", cn.LeerCadena());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@cedulaCliente", cedulaCliente);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;

        }

    public string ActualizarSaldoNotaDevolucion(double Bolivares, string NumeroNota, string NumOrdserv, string cedulaCliente, string Usuario, SqlCommand command)
    {
        try
        {
                SqlCommand cmd = command;
                cmd.CommandText ="SP_CPOS_ActualizarSaldoNotaDevolucion";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NumeroNota", NumeroNota);
                cmd.Parameters.AddWithValue("@NumOrdserv", NumOrdserv);
                cmd.Parameters.AddWithValue("@Bolivares", Bolivares);
                cmd.Parameters.AddWithValue("@cedulaCliente", cedulaCliente);
                cmd.Parameters.AddWithValue("@Usuario", Usuario);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cmd.Parameters.Clear();
                string resultado = dt.Rows[0]["resultado"].ToString();
                cmd.Parameters.Clear();
                return resultado;

            }

        catch (Exception ex)
        {
            string Error = string.Format("Error: {0}", ex.Message);
                return Error;
            }
    }


        public string RespaldarNotaDevolucion(string CodSuc, string NumeroNota, string NumOrdservAsociadoNota, string cedulaCliente, string Transaccion)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("SP_CPOS_Respaldo_NotaDevolucion", cn.LeerCadena());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodSuc", CodSuc);
                cmd.Parameters.AddWithValue("@NRONOTA", NumeroNota);
                cmd.Parameters.AddWithValue("@NumOrdserv", NumOrdservAsociadoNota);
                cmd.Parameters.AddWithValue("@CTE_CedIden", cedulaCliente);
                cmd.Parameters.AddWithValue("@Transaccion", Transaccion);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                string resultado = dt.Rows[0]["resultado"].ToString();
                return resultado;
            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return Error;
            }
        }

        public string BuscarUsuarioOrden(string IdUsuario) // Para traer el ususario que creo la orden
        {

            SqlCommand cmd = new SqlCommand("SELECT USER_NOMBRE, USER_APELLIDO from TB_USUARIO where COD_USR= @usuario", cn.LeerCadena());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@usuario", IdUsuario);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            string Nombre = dt.Rows[0]["USER_NOMBRE"].ToString();
            string Apellido = dt.Rows[0]["USER_APELLIDO"].ToString();

            string UsuarioOrden = Nombre + " " + Apellido;
            return UsuarioOrden;

        }
        
        // Para consultar si existen los billetes
        public string ConsultarBilletes(string NumOrdenserv, string Serial , string Tipo)
        {
            string CodSuc = _D_Inicio.Sucursal();

            SqlCommand cmd = new SqlCommand("SP_CPOS_CONSULTA_BILLETE", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NumOrdserv", NumOrdenserv);
            cmd.Parameters.AddWithValue("@Cod_Sucursal", CodSuc);
            cmd.Parameters.AddWithValue("@Serial", Serial);
            cmd.Parameters.AddWithValue("@Tipo",Tipo);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            string resultado = dt.Rows[0]["RESULTADO"].ToString();
            return resultado;


        }


        // Para insertar los billetes en la tabla tb_billetes
        public string InsertarBilletes(string NumOrdenserv, string Tipo, string Monto , string Serial, string Fec_Crea, string USER_Crea, string Transaccion, SqlCommand command)
        {
            string CodSuc = _D_Inicio.Sucursal();
            SqlCommand cmd = command;
            cmd.CommandText= "SP_INSERT_BILLETE";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NumOrdserv", NumOrdenserv);
            cmd.Parameters.AddWithValue("@Cod_Sucursal", CodSuc);
            cmd.Parameters.AddWithValue("@Tipo", Tipo);
            cmd.Parameters.AddWithValue("@Denominacion", Monto);
            cmd.Parameters.AddWithValue("@Serial", Serial);
            cmd.Parameters.AddWithValue("@Fec_Crea", Fec_Crea);
            cmd.Parameters.AddWithValue("@USER_Crea", USER_Crea);
            cmd.Parameters.AddWithValue("@Transaccion", Transaccion);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            cmd.Parameters.Clear();
            string resultado = dt.Rows[0]["resultado"].ToString();
            cmd.Parameters.Clear();
            return resultado;


        }


        public string ReversarMovimientoInventario(string sucursal, string Usuario, string NumOrden)
        {
            SqlCommand cmd = new SqlCommand("SP_CPOS_ReversarMovInventario", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@sucursal", sucursal);
            cmd.Parameters.AddWithValue("@Usuario", Usuario);
            cmd.Parameters.AddWithValue("@NumOrden", NumOrden);
   
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            return dt.Rows[0]["EJECUTAMOVIMIENTO"].ToString();

        }

        public string InsertarAbono(string Cod_Sucursal, string NumOrdserv, string Revision, string USER_Crea, Double Tasa_Dolar, SqlCommand command)
        {
            try
            {
                SqlCommand cmd = command;
                cmd.CommandText = ("SP_CPOS_POST_ABONO");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COD_SUCURSAL", Cod_Sucursal);
                cmd.Parameters.AddWithValue("@NumOrden", NumOrdserv);
                cmd.Parameters.AddWithValue("@REVISION", Revision);   
                cmd.Parameters.AddWithValue("@User_Crea", USER_Crea);
                cmd.Parameters.AddWithValue("@Tasa_Dolar", Tasa_Dolar);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cmd.Parameters.Clear();
                string Valor = dt.Rows[0]["resultado"].ToString();
                cmd.Parameters.Clear();
                return Valor;

            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return Error;
            }

        }

        public void Limpiar_TEMP_ABONO(string Cod_Sucursal, string NumOrdserv, string Revision)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM TEMP_ABONO WHERE cod_sucursal = @Cod_Sucursal AND  NumOrdserv = @NumOrdserv AND Revision = @Revision", cn.LeerCadena());
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Cod_Sucursal", Cod_Sucursal);
                cmd.Parameters.AddWithValue("@NumOrdserv", NumOrdserv);
                cmd.Parameters.AddWithValue("@Revision", Revision);
                cmd.CommandTimeout = 120;
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);


            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
            }

        }

        public void InsertarDatos_TEMP_ABONO(string Cod_Sucursal, string NumOrdserv, string Revision)
        {
            try
            {
     
                SqlCommand cmd = new SqlCommand(" INSERT INTO TEMP_ABONO select c.*,null FROM TB_ABONO  C left join TEMP_ABONO d on d.Cod_Sucursal = c.Cod_Sucursal and d.NumOrdserv = c.NumOrdserv and d.Revision = c.Revision WHERE c.cod_sucursal = @Cod_Sucursal AND c.NumOrdserv = @NumOrdserv and c.Revision = @Revision", cn.LeerCadena());
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Cod_Sucursal", Cod_Sucursal);
                cmd.Parameters.AddWithValue("@NumOrdserv", NumOrdserv);
                cmd.Parameters.AddWithValue("@Revision", Revision);
                cmd.CommandTimeout = 120;
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);


            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
            }

        }

        public DataSet OptenerExamenCompleto(string COD_SUCURSAL, string NumOrden, string REVISION)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("SP_CPOS_EXAMEN_ORDEN", cn.LeerCadena());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COD_SUCURSAL", COD_SUCURSAL);
                cmd.Parameters.AddWithValue("@NumOrden", NumOrden);
                cmd.Parameters.AddWithValue("@REVISION", REVISION);

                DataSet Examen = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(Examen);
                cmd.Parameters.Clear();
                return Examen;
            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return null;
            }


        }

        public DataSet OptenerDetalleOrdenCompleto(string COD_SUCURSAL, string NumOrden, string REVISION)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("SP_CPOS_CONSULTA_ORDEN", cn.LeerCadena());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COD_SUCURSAL", COD_SUCURSAL);
                cmd.Parameters.AddWithValue("@NumOrden", NumOrden);
                cmd.Parameters.AddWithValue("@REVISION", REVISION);

                DataSet Examen = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(Examen);
                cmd.Parameters.Clear();
                return Examen;
            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return null;
            }


        }

        //Iva Retenido 
        public DataTable AgenteRetencion(string Nacionalidad, string Cedula)
        {

            try
            {
                SqlCommand cmd = new SqlCommand("SP_CPOS_AgenteRetencion", cn.LeerCadena());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nacio", Nacionalidad);
                cmd.Parameters.AddWithValue("@cedula", Cedula);
                //cmd.Parameters.AddWithValue("@REVISION", Revision);

                DataTable Agente = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(Agente);
                cmd.Parameters.Clear();
                return Agente;

            }

            catch (Exception ex)
            {

                string Error = string.Format("Error: {0}", ex.Message);
                return null;
            }


        }

        public string ComprobantesRegistardos_IVA_ISLR(string NumeroOrden)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("select Fact_Num as Fact_Num from TB_FACTURAS where NumOrdServ = @NumOrdServ ", cn.LeerCadena());
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@NumOrdServ", NumeroOrden);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                string Valor = dt.Rows[0]["Fact_Num"].ToString();
                return Valor;
            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return "";
            }
        }


        public string LimitePagoMovil (string CodigoSucursal, string NumOrden)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_CPOS_LimitePagoMovil", cn.LeerCadena());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codSuc", CodigoSucursal);
                cmd.Parameters.AddWithValue("@NumOrden", NumOrden);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                string Valor = dt.Rows[0]["TotalPagomovilDiario"].ToString();
                return Valor;
            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return "";
            }
        }

        public string ReguistarPagoMovil(int IdAbono,string CodigoSucursal, string NroOs, string Rev, Double Tasa, Double MontoRecibidoRef, Double MontoVueltoRef, Double MontoVueltoBs, string CteNacionalidad, string CteCedula, string CodBancoReceptor, string telefono, SqlCommand command)
        {
            try
            {
                SqlCommand cmd = command;
                cmd.CommandText = ("pAddCambio_CPOS");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idAbono", IdAbono);
                cmd.Parameters.AddWithValue("@codSuc", CodigoSucursal);
                cmd.Parameters.AddWithValue("@NroOs", NroOs);
                cmd.Parameters.AddWithValue("@Rev", Rev);
                cmd.Parameters.AddWithValue("@Tasa",Tasa);
                cmd.Parameters.AddWithValue("@MontoRecibidoRef",MontoRecibidoRef);
                cmd.Parameters.AddWithValue("@MontoVueltoRef", MontoVueltoRef);
                cmd.Parameters.AddWithValue("@MontoVueltoBs", MontoVueltoBs);
                cmd.Parameters.AddWithValue("@CteNacionalidad", CteNacionalidad);
                cmd.Parameters.AddWithValue("@CteCedula", _D_Inicio.Encriptar(CteCedula));
                cmd.Parameters.AddWithValue("@CodBancoReceptor", CodBancoReceptor);
                cmd.Parameters.AddWithValue("@telefono", _D_Inicio.Encriptar(telefono));

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cmd.Parameters.Clear();
                string Valor = dt.Rows[0]["resultado"].ToString();
                cmd.Parameters.Clear();
                return Valor;

            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                //MessageBox.Show(Error);
                return "";
            }
        }


        public DataSet ImprimirComprobantePagoMovil(string NumeroOrdenImprimir, string glbSucursalActual,  SqlCommand command)
        {

            try
            {
                SqlCommand cmd = command;
                cmd.CommandText = ("GetVuelto_CPOS");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@numeroOrden", NumeroOrdenImprimir);
                cmd.Parameters.AddWithValue("@codSuc", glbSucursalActual);
                DataSet PagoMovil = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(PagoMovil);
                cmd.Parameters.Clear();
                return PagoMovil;

            }

            catch (Exception ex)
            {

                string Error = string.Format("Error: {0}", ex.Message);
                return null;
            }


        }

        public DataTable Buscar_Cambios_Realizados(string Sucursal, string NumeroOrden)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("select sum(MontoVueltoRef) as TotalPagomovilDolares, sum(MontoVueltoBs) as TotalPagomovilBolivares from dbo.TB_CAMBIO where NroOrden = @NumOrden  and CodSuc = @codSuc", cn.LeerCadena());
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@codSuc", Sucursal);
                cmd.Parameters.AddWithValue("@NumOrden", NumeroOrden);
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


        public void Registar_ISLR_Facturacion(string ComprobRetencionISLR, string Fact_Numm, string Sucursal, string NunOrden)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE TB_FACTURAS SET ComprobRetencionISLR = @ComprobRetencionISLR, FechaRegistroComprobISLR = getdate()  WHERE Fact_Num=@Fact_Numm  and Cod_Sucursal=@Sucursal and NumOrdServ= @NumOrdServ  ", cn.LeerCadena());
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ComprobRetencionISLR", ComprobRetencionISLR);
                cmd.Parameters.AddWithValue("@Fact_Numm", Fact_Numm);
                cmd.Parameters.AddWithValue("@Sucursal", Sucursal);
                cmd.Parameters.AddWithValue("@NumOrdServ", NunOrden);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
            }
        }


        public void Registar_IVA_Facturacion(string ComprobRetencionIva, string Fact_Numm, string Sucursal, string NumOrdServ)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE TB_FACTURAS SET ComprobRetencionIva = @ComprobRetencionIva, FechaRegistroComprobIVA = getdate()  WHERE Fact_Num=@Fact_Numm  and Cod_Sucursal=@Sucursal and NumOrdServ= @NumOrdServ  ", cn.LeerCadena());
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ComprobRetencionIva", ComprobRetencionIva);
                cmd.Parameters.AddWithValue("@Fact_Numm", Fact_Numm);
                cmd.Parameters.AddWithValue("@Sucursal", Sucursal);
                cmd.Parameters.AddWithValue("@NumOrdServ", NumOrdServ);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
            }
        }


        public string Buscar_ISLR_Facturacion(string Fact_Numm, string Sucursal, string NumOrdServ)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("select ComprobRetencionISLR WHERE Fact_Num=@Fact_Numm  and Cod_Sucursal=@Sucursal and NumOrdServ= @NumOrdServ ", cn.LeerCadena());
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Fact_Numm", Fact_Numm);
                cmd.Parameters.AddWithValue("@Sucursal", Sucursal);
                cmd.Parameters.AddWithValue("@NumOrdServ", NumOrdServ);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                string Valor = dt.Rows[0]["ComprobRetencionISLR"].ToString();
                return Valor;

            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return Error;
            }
        }

        public string Buscar_Iva_Facturacion(string Fact_Numm, string Sucursal, string NumOrdServ)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("select ComprobRetencionIva WHERE Fact_Num=@Fact_Numm  and Cod_Sucursal=@Sucursal and NumOrdServ= @NumOrdServ ", cn.LeerCadena());
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Fact_Numm", Fact_Numm);
                cmd.Parameters.AddWithValue("@Sucursal", Sucursal);
                cmd.Parameters.AddWithValue("@NumOrdServ", NumOrdServ);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                string Valor = dt.Rows[0]["ComprobRetencionIva"].ToString();
                cmd.Parameters.Clear();
                return Valor;

            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return Error;
            }
        }

        public string ID_PagoMovil(string NumeroOrdenImprimir, string glbSucursalActual, SqlCommand command)
        {

            try
            {
                if (command == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command = connection.CreateCommand();
                }
                SqlCommand cmd = command;
                cmd.CommandText = ("Select isnull(max(Id+1),1) as ID from TB_CAMBIO");
                cmd.CommandType = CommandType.Text;
                DataTable PagoMovil = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(PagoMovil);
                cmd.Parameters.Clear();
                string Valor = PagoMovil.Rows[0]["ID"].ToString();
                return Valor;

            }

            catch (Exception ex)
            {

                string Error = string.Format("Error: {0}", ex.Message);
                //MessageBox.Show(Error);
                return null;
            }


        }

        public string NombreBanco_PagoMovil(string CodBanco, SqlCommand command)
        {

            try
            {
                if (command == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command = connection.CreateCommand();
                }
                SqlCommand cmd = command;
                cmd.CommandText = ("Select NOMBREBANCO as Banco from TB_BANCOS where CODBAN = @CodBanco");
                cmd.Parameters.AddWithValue("@CodBanco", CodBanco);
                cmd.CommandType = CommandType.Text;
                DataTable PagoMovil = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(PagoMovil);
                cmd.Parameters.Clear();
                string Valor = PagoMovil.Rows[0]["Banco"].ToString();
                return Valor;

            }

            catch (Exception ex)
            {

                string Error = string.Format("Error: {0}", ex.Message);
                return null;
            }


        }

        public string Nombre_Surculsal_PagoMovil(string CodSucursal, SqlCommand command)
        {

            try
            {
                if (command == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command = connection.CreateCommand();
                }
                SqlCommand cmd = command;
                cmd.CommandText = ("Select CodSucursal +'-' + Descripcion as Descripcion from TB_SUCURSALES where CodSucursal= @CodSucursal");
                cmd.Parameters.AddWithValue("@CodSucursal", CodSucursal);
                cmd.CommandTimeout = 120;
                cmd.CommandType = CommandType.Text;
                DataTable PagoMovil = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(PagoMovil);
                cmd.Parameters.Clear();
                string Valor = PagoMovil.Rows[0]["Descripcion"].ToString();
                return Valor;

            }

            catch (Exception ex)
            {

                string Error = string.Format("Error: {0}", ex.Message);
                return null;
            }


        }

        public DataSet Stock_LC(string NumeroOrden, string Revision, string Status, SqlCommand command)
        {

            try
            {
                if (command == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command = connection.CreateCommand();
                }
                SqlCommand cmd = command;
                cmd.CommandText = ("pUpdStockLC");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NUMORDSERV", NumeroOrden);
                cmd.Parameters.AddWithValue("@REVISION", Revision);
                cmd.Parameters.AddWithValue("@STATUSOS", Status);
                DataSet DST_Stock_LC = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DST_Stock_LC);
                cmd.Parameters.Clear();
                return DST_Stock_LC;

            }

            catch (Exception ex)
            {

                string Error = string.Format("Error: {0}", ex.Message);
                return null;
            }


        }


        public DataSet RelacionMovimientosLC(string NumeroOrden, string CodMov, string Sucursal, string Usuario, string FechaDiaAct, string NroOS, string Revision, SqlCommand command)
        {

            try
            {
                if (command == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command = connection.CreateCommand();
                }
                SqlCommand cmd = command;
                cmd.CommandText = ("pAddRelacionMovimientosLC");
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NroDocumento", NumeroOrden);
                cmd.Parameters.AddWithValue("@CodMov", CodMov);
                cmd.Parameters.AddWithValue("@Sucursal", Sucursal);
                cmd.Parameters.AddWithValue("@Usuario", Usuario);
                cmd.Parameters.AddWithValue("@FechaDiaAct", FechaDiaAct);
                cmd.Parameters.AddWithValue("@NroOS", NroOS);
                cmd.Parameters.AddWithValue("@Revision", Revision);

                DataSet DST_RelacionMovimientosLC = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DST_RelacionMovimientosLC);
                cmd.Parameters.Clear();
                return DST_RelacionMovimientosLC;

            }

            catch (Exception ex)
            {

                string Error = string.Format("Error: {0}", ex.Message);
                return null;
            }


        }

        public string ActualizarPagoMovil(string Cod_Sucursal, string NumOrdserv, string Revision, string idAbono , string Referencia, string DetalleTransaccion, string CodigoError)
        {
            SqlCommand cmd = new SqlCommand("SP_CPOS_ActualizarPagoMovil", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodSuc", Cod_Sucursal);
            cmd.Parameters.AddWithValue("@NroOrd", NumOrdserv);
            cmd.Parameters.AddWithValue("@Revision", Revision);
            cmd.Parameters.AddWithValue("@IdAbono", idAbono);
            cmd.Parameters.AddWithValue("@Referencia", _D_Inicio.Encriptar(Referencia));
            cmd.Parameters.AddWithValue("@DetalleTransaccion", DetalleTransaccion);
            cmd.Parameters.AddWithValue("@CodigoError", CodigoError);
            cmd.Parameters.AddWithValue("@Usuario", TB_USUARIO.COD_EMPLEADO);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            string Valor = dt.Rows[0][0].ToString();
            return Valor;


        }


        public string RegistarAuditorAbono(string CodSucursal, SqlCommand command)
        {

            try
            {
                if (command == null)
                {
                    SqlConnection connection = cn.LeerCadena();
                    command = connection.CreateCommand();
                }
                SqlCommand cmd = command;
                cmd.CommandText = ("Select CodSucursal +'-' + Descripcion as Descripcion from TB_SUCURSALES where CodSucursal= @CodSucursal");
                cmd.Parameters.AddWithValue("@CodSucursal", CodSucursal);
                cmd.CommandTimeout = 120;
                cmd.CommandType = CommandType.Text;
                DataTable PagoMovil = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(PagoMovil);
                cmd.Parameters.Clear();
                string Valor = PagoMovil.Rows[0]["Descripcion"].ToString();
                return Valor;

            }

            catch (Exception ex)
            {

                string Error = string.Format("Error: {0}", ex.Message);
                return null;
            }


        }

    }
}