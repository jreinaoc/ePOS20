using CapaEntidades;
using CapaDatos.DetalleOrden_Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using CapaDatos.Inicio_Datos;
using CapaDatos.Anulacion;
using System.Data.SqlClient;

namespace CapaLogica.Anulacion_Logica
{
    public class L_Anulacion
    {
        public string orden;
        public bool OK;
        public bool GuardoMovimientoArticulo;
        public bool MontRecib;
        public bool ElimineOrden = false;
        public string NroNota;
        D_Anulacion _D_Anulacion = new D_Anulacion();
        D_Inicio _D_Inicio = new D_Inicio();
        D_DetalleOrden _DetalleOrden = new D_DetalleOrden();

        //El uso de la clase StringBuilder nos ayudara a devolver los mensajes de las validaciones
        public readonly StringBuilder stringBuilder = new StringBuilder();

        public DataTable CargarResponsables(string Sucursal)
        {

            DataTable dt = new DataTable();
            dt = _D_Anulacion.TraerResponsables(Sucursal);
            return dt;
        }

        public DataTable CargarMotivos(string Responsable)
        {
            DataTable dt = new DataTable();
            dt = _D_Anulacion.TraerMotivos(Responsable);
            return dt;

        }

        public string Anulacion(string Orden, string CodMotivo, string CodRespon, string Observaciones, string usuario, string nota, SqlCommand command = null)
        {
            string resp=  _D_Anulacion.AnularOrden(Orden, CodMotivo, CodRespon, Observaciones, usuario, nota, command);
            return resp;
        }

        public DataTable CargarDetalleOrd(string NumeroDoc, string CodMov, string TipoDoc, SqlCommand command = null)
        {
            DataTable dt = new DataTable();
            try
            { 
            dt = _D_Anulacion.TraerOrdenDet(TB_CAORDSER.NumOrdserv,command);

            if (CodMov == "N") // realiza el movimiento solo para nota de credito 
            {
                foreach (DataRow row in dt.Rows)
                {
                    //MessageBox.Show("Vamos por la ejecucion de la logica de anular, justamente cargando el detalle");
                    //string fecha = TB_CAORDSER.Fecha.ToString("yyyyMMdd");
                    string fecha  =  (DateTime.UtcNow.ToShortDateString());


                    string articulo = row["CodArticulo"].ToString();
                    string precio = "0";
                    
                    string cantidad = row["Ordserv_Cant"].ToString();

                    string costo = "0";
                    ;


                    if (row["CodArticulo"].ToString().StartsWith("M") || row["CodArticulo"].ToString().StartsWith("L"))
                    {
                        //MessageBox.Show("Vamos por la condicional de si es M o L para ver si es monturaquorum");
                        if (TB_CAORDSER.MonturaEnQuorum == false)
                        {

                                    // MessageBox.Show("Montura quorum false, por lo tanto se ejecura el movimiento");
                                    _DetalleOrden.MovimientoInventario(articulo, TipoDoc, TB_CAORDSER.NumOrdserv, cantidad, costo, precio, CodMov, TB_USUARIO.COD_USR, _D_Inicio.Sucursal(), fecha, TB_CAORDSER.NumOrdserv, command);
                                    if (_DetalleOrden.MovimientoInv == "SATISFACTORIO")
                                    {
                                        //MessageBox.Show("Movimieno satisfactorio ml ");
                                        OK = true;
                                        GuardoMovimientoArticulo = true;
                                    }
                                    else
                                    {
                                        OK = false;
                                        GuardoMovimientoArticulo = false;
                                        return null;
                                    }                          


                        }
                        else
                        {
                            GuardoMovimientoArticulo = true;
                            MontRecib = true;
                        }

                    }
                    else
                    {
                        // MessageBox.Show("No es M L ES otro y por eso entra aqui ");
                        _DetalleOrden.MovimientoInventario(articulo, TipoDoc, TB_CAORDSER.NumOrdserv, cantidad, costo, precio, CodMov, TB_USUARIO.COD_USR, _D_Inicio.Sucursal(), fecha, TB_CAORDSER.NumOrdserv, command);

                        if (_DetalleOrden.MovimientoInv == "SATISFACTORIO")
                        {
                            OK = true;
                            GuardoMovimientoArticulo = true;
                            //MessageBox.Show("Movimiento Sarisfactorio c");
                        }
                        else
                        {
                            OK = false;
                            GuardoMovimientoArticulo = false;
                            return null;
                        }


                    }

                }
            }
            else // Realiza el movimiento para cualquier tipo de accion 
            {
                foreach (DataRow row in dt.Rows)
                {
                    //MessageBox.Show("Vamos por la ejecucion de la logica de anular, justamente cargando el detalle");
                    //string fecha = TB_CAORDSER.Fecha.ToString("yyyyMMdd");
                    string fecha = (DateTime.UtcNow.ToShortDateString());


                    string articulo = row["CodArticulo"].ToString();
                    string precio = row["Ordserv_Precio"].ToString();
                    precio = precio.Replace(",", ".");
                    string cantidad = row["Ordserv_Cant"].ToString();

                    string costo = _D_Anulacion.TraerArt(articulo);
                    costo = costo.Replace(",", ".");


                    if (row["CodArticulo"].ToString().StartsWith("M") || row["CodArticulo"].ToString().StartsWith("L"))
                    {
                        //MessageBox.Show("Vamos por la condicional de si es M o L para ver si es monturaquorum");
                        if (TB_CAORDSER.MonturaEnQuorum == false)
                        {
                            // MessageBox.Show("Montura quorum false, por lo tanto se ejecura el movimiento");
                            _DetalleOrden.MovimientoInventario(articulo, TipoDoc, TB_CAORDSER.NumOrdserv, cantidad, costo, precio, CodMov, TB_USUARIO.COD_USR, _D_Inicio.Sucursal(), fecha, TB_CAORDSER.NumOrdserv, command);
                            if (_DetalleOrden.MovimientoInv == "SATISFACTORIO")
                            {
                                //MessageBox.Show("Movimieno satisfactorio ml ");
                                OK = true;
                                GuardoMovimientoArticulo = true;
                            }
                            else
                            {
                                OK = false;
                                GuardoMovimientoArticulo = false;
                            }


                        }
                        else
                        {
                            GuardoMovimientoArticulo = true;
                            MontRecib = true;
                        }

                    }
                    else
                    {
                        // MessageBox.Show("No es M L ES otro y por eso entra aqui ");
                        _DetalleOrden.MovimientoInventario(articulo, TipoDoc, TB_CAORDSER.NumOrdserv, cantidad, costo, precio, CodMov, TB_USUARIO.COD_USR, _D_Inicio.Sucursal(), fecha, TB_CAORDSER.NumOrdserv, command);

                        if (_DetalleOrden.MovimientoInv == "SATISFACTORIO")
                        {
                            OK = true;
                            GuardoMovimientoArticulo = true;
                            //MessageBox.Show("Movimiento Sarisfactorio c");
                        }
                        else
                        {
                            OK = false;
                            GuardoMovimientoArticulo = false;

                        }


                    }

                }





            }

            return dt;
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                //return stringBuilder.Length == 0;
                ElimineOrden = false;
                return null;

            }

        }


        public string Verificar_Existencia_Inv (string Numero_orden, string Cod_DetVta, string OrSer_Statu, SqlCommand command = null) 
        {

            DataSet DsExistencia = _DetalleOrden.Verificar_Existencia_Inventario(Numero_orden, command);

            if (DsExistencia.Tables[0].Rows.Count > 0 && OrSer_Statu != "005" && Cod_DetVta != "02")
            {
                StringBuilder art = new StringBuilder();

                for (int i = 0; i < DsExistencia.Tables[0].Rows.Count; i++)
                {
                    art.Append(DsExistencia.Tables[0].Rows[i]["CodArticulo"].ToString() + ", ");
                }

                if (art.Length > 2)
                {
                    art.Remove(art.Length - 2, 2); // Eliminar la última coma y espacio
                }

                if (art.Length > 7)
                {
                    string mensaje1 = "Los articulos " + art.ToString() + " NO tienen existencia";
                    return mensaje1;


                }
                else
                {
                    string mensaje1 = "El articulo " + art.ToString() + " no tiene existencia" ;
                    return mensaje1;

                }

              
            }
            return "SATISFACTORIO";
        }

        public decimal Saldo_Total_Orden (string Cod_Sucursal, string NumOrdserv, string Revision)
        {
            decimal Saldo= _D_Anulacion.Saldo_Orden(Cod_Sucursal, NumOrdserv, Revision);
            return Saldo;

        }


        public string EnviarDatoaNotaDev(string observaciones, SqlCommand command = null)
        {
            if (TB_CAORDSER.OrSer_Status == "005")
            {
                string rept = "";
                string Monto = TB_CAORDSER.VtaTotal.ToString();
                Monto = Monto.Replace(",", ".");
                rept =  _D_Anulacion.CargarNotaDevolucion(TB_CAORDSER.Cod_Sucursal, "003", TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision, "null", TB_CAORDSER.CTE_Nacio, TB_CAORDSER.CTE_CedIden, observaciones, Monto, Monto, false, false, TB_USUARIO.COD_USR, TB_USUARIO.COD_USR, command);
                if (rept == "SATISFACTORIO")
                rept = _D_Anulacion.ObtenerNroNota(TB_CAORDSER.NumOrdserv, TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.Revision, command);
                NroNota = _D_Anulacion.NroNota;

                return rept; 
            }

            return "SATISFACTORIO";
        }

        public string EnviarGarantia(SqlCommand command = null)
        {
            if (TB_CAORDSER.Asegurada == true)
            {
                string rp= _D_Anulacion.CargarGarantia(TB_CAORDSER.NumOrdserv, "REPO", "", command);
                return rp;
            }

            return "SATISFACTORIO";
        }

        public void EnviarAuditor(string CodAccion, SqlCommand command = null)
        {


            string detalles = "OS: " + TB_CAORDSER.NumOrdserv + " ," + "Cliente: " + TB_CAORDSER.CTE_Nacio + "-" + TB_CAORDSER.CTE_CedIden;
            _D_Anulacion.CaragarAuditor(TB_USUARIO.COD_SUCURSAL, CodAccion, TB_USUARIO.COD_EMPLEADO, detalles, command);

        }

        public string EnviarMovAnulacion(SqlCommand command = null)
        {
            NroNota = _D_Anulacion.ObtenerNroNota(TB_CAORDSER.NumOrdserv, TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.Revision, command);
            DateTime FechaActiva = _D_Inicio.DiaActivo();
            string resp= _D_Anulacion.EjecMovAnulacion(TB_CAORDSER.NumOrdserv, TB_USUARIO.COD_SUCURSAL, "003", "N", FechaActiva.ToString(), NroNota, TB_USUARIO.COD_USR, command);
            return resp;

        }

        public bool EliminarPagosActualizarSaldo(int IdAbono, string Cod_Sucursal, string NumOrden, string Revision, string Usuario, Double AboMonto, string Observacion, string CodMotivo, string Cod_ResponsableAnu)
        {
            try
            {
                stringBuilder.Clear();
                ElimineOrden = false;
                string respuesta = _D_Anulacion.EliminarAbonoActualizarSaldo(IdAbono, Cod_Sucursal, NumOrden, Revision, Usuario, AboMonto);


                if (respuesta != "SATISFACTORIO")
                {
                    stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", respuesta) + ". Ocurrió un error eliminando el pago Nro: " + Convert.ToString(IdAbono) + " de la orden Nro: " + NumOrden + ", por favor comuniquese con el Dpto de Sistemas y reporte el error, " + "No se realizó el proceso");
                    //return stringBuilder.Length == 0;
                    ElimineOrden = false;
                    return false;
                }


                DataTable AbonosAct = _D_Anulacion.ObtenerAbonosActivos(Cod_Sucursal, NumOrden, Revision);

                if (AbonosAct.Rows.Count > 0)
                {
                    ElimineOrden = false;
                }

                else
                {     
                    respuesta = _D_Anulacion.EliminarOrden(Cod_Sucursal, NumOrden, Revision, CodMotivo, Cod_ResponsableAnu, Usuario, Observacion);
                    if (respuesta == "SATISFACTORIO")
                    {
                        ElimineOrden = true;
                    }

                }


                if (respuesta != "SATISFACTORIO")
                {
                    stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", respuesta) + ". Ocurrió un error eliminando la orden Nro: " + NumOrden + ", por favor comuniquese con el Dpto de Sistemas y reporte el error, " + "No se pudo procesar");
                    //return stringBuilder.Length == 0;
                    return false;
                }

                //return stringBuilder.Length == 0;

                //JR 05/02/2024 Lo comente porque se estaba yendo falso y no generaba mov
                //ElimineOrden = false;
                return true;

            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                //return stringBuilder.Length == 0;
                ElimineOrden = false;
                return false;

            }
        }


        public bool EliminarFacturaLogico(string Cod_Sucursal, string Fact_Num, string Fact_SerialImpresora, string COD_USR)
        {
            try
            {
                stringBuilder.Clear();

                string respuesta = _D_Anulacion.EliminarFactura(Cod_Sucursal, Fact_Num, COD_USR, Fact_SerialImpresora);


                if (respuesta != "SATISFACTORIO")
                {
                    stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", respuesta) + ". Ocurrió un error eliminando la factura Nro: " + Fact_Num + ", por favor comuniquese con el Dpto de Sistemas y reporte el error, " + "No se pudo procesar");
                    //return stringBuilder.Length == 0;
                    return false;
                }

                //return stringBuilder.Length == 0;
                return true;

            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                //return stringBuilder.Length == 0;
                return false;
            }
        }

        public bool ValidarPagosDia(string Cod_Sucursal, string NumeroOrden, string FechaActiva, SqlCommand command = null)
        {
            try
            {
                stringBuilder.Clear();

                DataTable respuesta = _D_Anulacion.TraerAbonos(Cod_Sucursal, NumeroOrden, "0", command);

                if (respuesta.Rows.Count > 0)
                {
                    foreach (DataRow drItem in respuesta.Rows)
                    {
                        DateTime FechaPago = (DateTime) drItem["Fecha"];
                        if (FechaPago.ToString("yyyyMMdd") == FechaActiva)
                        {
                            return true;            
                        }
                    }


                    return false;

                }
                else
                {
                    //return stringBuilder.Length == 0;
                    return false;
                }


            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                //return stringBuilder.Length == 0;
                return false;
            }
        }
    }
}
