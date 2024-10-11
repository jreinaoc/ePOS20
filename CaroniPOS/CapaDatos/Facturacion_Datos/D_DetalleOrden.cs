using CapaDatos.Inicio_Datos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.DetalleOrden_Datos
{
    public class D_DetalleOrden
    {
        Conexion.Conexion cn = new Conexion.Conexion();
        D_Inicio _D_Inicio = new D_Inicio();

        public DataTable Pagos()
        {
            SqlDataAdapter da = new SqlDataAdapter("select COD_PAGO as Value, DescripPago as Indexx from TB_TIPOPAGO where  TipoPag_ST= 'A'", cn.LeerCadena());
            da.SelectCommand.CommandType = CommandType.Text;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;

        }

        public DataTable Bancos(bool MonedaExtranjera)
        {
            SqlCommand cmd = new SqlCommand("select CODBAN as Value, NOMBREBANCO as Indexx from TB_BANCOS where  ST_BANCOS= 'A' and MONEDAEXTRANJERA= @Moneda", cn.LeerCadena());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Moneda", MonedaExtranjera);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;

        }


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

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CodSuc", CodSuc);
            cmd.Parameters.AddWithValue("@NumordServ", NumeroOrden);
            cmd.Parameters.AddWithValue("@Revision", "0");

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
        }
        public void Datos_de_la_Orden(string NumeroOrden)
        {
            try
            {
                string Sucursal = _D_Inicio.Sucursal();

                //ejecuto el recalculo de la orden 
                RecalcularOrden(Sucursal, NumeroOrden);

                //Busco los datos de la orden; datos que ya estan actualizados (Recalculados)
                SqlCommand cmd = new SqlCommand("SP_CPOS_Datos_de_la_Orden", cn.LeerCadena());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sucursal", Sucursal);
                cmd.Parameters.AddWithValue("@NumeroOrden", NumeroOrden);
                SqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.HasRows)
                {

                    while (dataReader.Read())
                    {
                        TB_CAORDSER.Cod_Sucursal = Convert.ToString(dataReader["Cod_Sucursal"]);
                        TB_CAORDSER.NumOrdserv = Convert.ToString(dataReader["NumOrdserv"]);
                        TB_CAORDSER.Revision = Convert.ToString(dataReader["Revision"]);
                        TB_CAORDSER.Fecha = Convert.ToDateTime(dataReader["Fecha"].ToString());
                        TB_CAORDSER.Cod_Venta = Convert.ToString(dataReader["Cod_Sucursal"]);
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

        public DataTable BuscarNota(string Nota)
        {
            string CodSuc = _D_Inicio.Sucursal();


            SqlCommand cmd = new SqlCommand("SP_CPOS_BucarNota", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Nota", Nota);
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

        public DataSet DETALLEFACTURAFISCAL(string NumeroOrdenImprimir)
        {
            SqlCommand cmd = new SqlCommand("SP_DETALLEFACTURAFISCAL", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NumOrden", NumeroOrdenImprimir);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet dts = new DataSet();
            da.Fill(dts);
            return dts;
        }

        public DataSet PagosConIGTF(string SucursalActual, string NroOrdenServicio)
        {
            SqlCommand cmd = new SqlCommand("pGetPagosConIGTF", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodSuc", SucursalActual);
            cmd.Parameters.AddWithValue("@NumordServ", NroOrdenServicio);
            cmd.Parameters.AddWithValue("@Revision", "0");
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet dts = new DataSet();
            da.Fill(dts);
            return dts;
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

        public DataTable TEMP_ABONO(string NumeroOrdenImprimir)
        {
            SqlCommand cmd = new SqlCommand("select * from TB_ABONO where NumOrdserv = @NumeroOrdenImprimir", cn.LeerCadena());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@NumeroOrdenImprimir", NumeroOrdenImprimir);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;

        }

        public DataSet DESCUENTOSFACTURAFISCAL(string NumeroOrdenImprimir)
        {
            SqlCommand cmd = new SqlCommand("SP_DESCUENTOSFACTURAFISCAL", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NumOrden", NumeroOrdenImprimir);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet dts = new DataSet();
            da.Fill(dts);
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

        public DataTable Pasgos(string SucursalActual, string NroOrdenServicio)
        {
            SqlCommand cmd = new SqlCommand("pGetPagos", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodSuc", SucursalActual);
            cmd.Parameters.AddWithValue("@NumordServ", NroOrdenServicio);
            cmd.Parameters.AddWithValue("@Revision", "0");
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;

        }

        public string TB_PARAMETROSPGE(string Parametro)
        {
            SqlCommand cmd = new SqlCommand("SELECT Valor from  TB_PARAMETROSPGE where ParametroPGE= @Parametro", cn.LeerCadena());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Parametro", Parametro);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            string Valor = dt.Rows[0]["Valor"].ToString();
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

        public void SP_SUMOFACTURA(string CodSuc, string NumFactura, string Fact_SerialImpresora, string NumOs, string MontoFactImpreso, string MontoDescExento, string MontoDescGravable)
        {
            SqlCommand cmd = new SqlCommand("SP_SUMOFACTURA", cn.LeerCadena());
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


        }

        public DataTable GetAbono(string ID_Abono, string Cod_Sucursal, string NumOrdserv, string Revision, string Tipo_Pago, string Cod_Banco, string Abo_CTATARJETA, string Abo_CVCNROCHEQUE, string Abo_Fecha, int Abo_Monto, string Abo_Tipo,
        string Tipo_Pto, string CodPunto, string Anulado, string Fec_Crea, string Fec_Mod, string USER_Crea, string USER_Mod, string Fecha, string Fecha_Abono, string Cod_BancoRecep, int Tasa_Abono, int Abo_Monto_Divisa, int Abo_Monto_SinIGTF,
        int Abo_IGTF)
        {
            SqlCommand cmd = new SqlCommand("SP_CPOS_GET_ABONO", cn.LeerCadena());

            cmd.CommandType = CommandType.StoredProcedure;

         
            cmd.Parameters.AddWithValue("@ID", ID_Abono);
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
            cmd.Parameters.AddWithValue("@Fec_Mod", Fec_Mod);
            cmd.Parameters.AddWithValue("@User_Crea", USER_Crea);
            cmd.Parameters.AddWithValue("@User_Mod", USER_Mod);
            cmd.Parameters.AddWithValue("@FECHA", Fecha);
            cmd.Parameters.AddWithValue("@Fecha_Abono", Fecha_Abono);
            cmd.Parameters.AddWithValue("Cod_BancoRecep", @Cod_BancoRecep);
            cmd.Parameters.AddWithValue("@TasaAbono", Tasa_Abono);
            cmd.Parameters.AddWithValue("Abo_Monto_Divisa", Abo_Monto_Divisa);
            cmd.Parameters.AddWithValue("@Abo_Monto_SinIGTF", Abo_Monto_SinIGTF);
            cmd.Parameters.AddWithValue("@Abo_IGTF", Abo_IGTF);
          

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return (dt);





        }
    }
}
