using CapaEntidades;
using CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using CapaDatos.ListaOrdenes_Datos;
using CapaDatos.DetalleOrden_Datos;
using CapaDatos.Inicio_Datos;
using CapaDatos.Anulacion;

namespace CapaLogica.ListaOrden_Logica
{
    public class L_ListaOrdenes
    {
        //El uso de la clase StringBuilder nos ayudara a devolver los mensajes de las validaciones
        public readonly StringBuilder stringBuilder = new StringBuilder();

        //Instanciamos nuestra clase D_Loguin para poder utilizar sus miembros
        private D_ListaOrdenes _D_ListaOrdenes = new D_ListaOrdenes();

        private D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        private D_Inicio _D_Inicio = new D_Inicio();
        private D_Anulacion _D_Anulacion = new D_Anulacion();

        public bool requiereClave = false;
        public bool LCBajoPedido = false;

        public class Valor
        {
            public string Value { get; set; }
            public string Index { get; set; }
        }



        public bool LLenarCombobox(System.Windows.Forms.ComboBox Dias, System.Windows.Forms.ComboBox Status, System.Windows.Forms.DateTimePicker cale)
        {
            stringBuilder.Clear();

            //rellenar el ComboBox Dias 

            var Valores = new List<Valor>();

            Valores.Add(new Valor() { Index = "0", Value = "Hoy" });
            Valores.Add(new Valor() { Index = "1", Value = "Ayer" });
            Valores.Add(new Valor() { Index = "7", Value = "Últimos 7 días" });
            Valores.Add(new Valor() { Index = "15", Value = "Últimos 15 días" });
            Valores.Add(new Valor() { Index = "30", Value = "Últimos 30 días" });
            Valores.Add(new Valor() { Index = "45", Value = "Seleccione período manualmente" });

            Dias.DataSource = Valores;
            Dias.DisplayMember = "Value";
            Dias.ValueMember = "Index";



            //rellenar el ComboBox Status 

            var Statu = new List<Valor>();

            Statu.Add(new Valor() { Index = "", Value = "Todos" });
            Statu.Add(new Valor() { Index = "005", Value = ">  Abonada" });
            Statu.Add(new Valor() { Index = "003", Value = ">  Anulada" });
            Statu.Add(new Valor() { Index = "002", Value = ">  Facturada" });
            Statu.Add(new Valor() { Index = "004", Value = ">  Por Pagar" });

            Status.DataSource = Statu;
            Status.DisplayMember = "Value";
            Status.ValueMember = "Index";

            return stringBuilder.Length == 0;
        }

        public List<Valor> LLenarComboboxOpciones()
        {
            stringBuilder.Clear();

            //rellenar el ComboBox Opciones 

            var Valores = new List<Valor>();


            Valores.Add(new Valor() { Index = "Procesar sin pago", Value = "Procesar sin pago" });
            Valores.Add(new Valor() { Index = "Anular orden", Value = "Anular orden" });
            Valores.Add(new Valor() { Index = "Reimprimir orden", Value = "Reimprimir orden" });

            return Valores;
        }

        public DataSet TraerOrdenes(System.Windows.Forms.ComboBox Dias, System.Windows.Forms.ComboBox Status, string NunOrden, string NumCedula, int Inicio = 1, int Final = 12)
        {
            try
            {
                stringBuilder.Clear();

                //Le enviamos el index asociados al valor selecionado en el combobox 
                DataSet Ordenes = _D_ListaOrdenes.CargarOrdenes(Dias.SelectedValue.ToString(), Status.SelectedValue.ToString(), NunOrden, NumCedula,Inicio,Final);

                if (Ordenes.Tables[0].Rows.Count > 0)
                {
                    return Ordenes;
                }
                stringBuilder.Append(Environment.NewLine + "No hay ordenes");
                return null;

            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
            }

        }

        public DataSet TraerOrdporRango(System.Windows.Forms.DateTimePicker Fechadesde, System.Windows.Forms.DateTimePicker Fechahasta, System.Windows.Forms.ComboBox Status, string NumCedula, int Inicio = 1, int Final = 12)
        {
            try
            {
                stringBuilder.Clear();


                DateTime PRUE = Fechadesde.Value;
                DateTime PRUEB = Fechahasta.Value;
                string PeriodoDesde = PRUE.ToString("yyyyMMdd");
                string PeriodoHasta = PRUEB.ToString("yyyyMMdd");

                //Le enviamos el index asociados al valor selecionado en el combobox 
                DataSet Ordenesrango = _D_ListaOrdenes.CargarOrdPorRango(PeriodoDesde, PeriodoHasta, Status.SelectedValue.ToString(),NumCedula, Inicio, Final);

                if (Ordenesrango.Tables[0].Rows.Count > 0)
                {
                    return Ordenesrango;
                }
                stringBuilder.Append(Environment.NewLine + "No hay ordenes");
                return null;

            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
            }
        }

        public void ValidarRequiereClave()
        {
            try
            {
                stringBuilder.Clear();

                DataTable Parametro = _D_ListaOrdenes.ParametrosPais("ValidaContraPedidoLC");

                if (TB_CAORDSER.Cod_DetVta == "02" & Parametro.Rows[0]["Venezuela"].ToString() == "1")
                {
                    DataSet DsExistencia = _D_ListaOrdenes.ValidaExistenciaArticulo(TB_CAORDSER.NumOrdserv);
                    if (DsExistencia.Tables[0].Rows.Count > 0)
                    {
                        if (DsExistencia.Tables[0].Rows[0][0].ToString() == "IMPRIMIR")
                            LCBajoPedido = true;
                        else
                            LCBajoPedido = false;

                        if (LCBajoPedido & (TB_USUARIO.Id_Rol == "003" | TB_USUARIO.Id_Rol == "004" | TB_USUARIO.Id_Rol == "011"))
                            requiereClave = false;
                        else
                            requiereClave = true;
                    }
                    else
                        requiereClave = true;
                }
                else if ((TB_USUARIO.Id_Rol != "000" & TB_USUARIO.Id_Rol != "001") & TB_CAORDSER.Cod_Venta != "003")
                    requiereClave = true;
                else
                    requiereClave = false;


                if (requiereClave == true)
                {
                    // Pregunto si se desea escribir la clave cuando no es Gerente Regional, Gerente de Tienda o Subgerente

                    //stringBuilder.Append(Environment.NewLine + "Para Procesar Sin Pago debe introducir clave autorizada, ¿Desea continuar?");
                    stringBuilder.Append(Environment.NewLine + "¿Está seguro de procesar sin pago la orden N° " + TB_CAORDSER.NumOrdserv +"?");
                }

            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
            }
        }

        public bool ValidoEstatusOrden(string OrigenLlamado = "")
        {
            try
            {
                stringBuilder.Clear();
                _D_DetalleOrden.Datos_de_la_Orden(TB_CAORDSER.OrSer_Status,TB_CAORDSER.Revision);

                bool ValidoEstatusOrden;

                if (OrigenLlamado != "Desincorporar")
                {
                    // Verifico el Status de la orden antes de proseguir, es para los casos en donde haya
                    // más de una caja vendiendo y por casualidad se quiera procesar la misma orden

                    if (TB_CAORDSER.OrSer_Status == "004")
                        // El estatus no a cambiado
                        ValidoEstatusOrden = true;
                    else
                    {
                        // El Estatus cambio
                        stringBuilder.Append(Environment.NewLine + "La Orden Nro: " + TB_CAORDSER.NumOrdserv + " ya fué procesada y su Estatus cambio. Ya fué o facturada o eliminada desde otro Punto de Venta, Orden Procesada");
                        ValidoEstatusOrden = false;
                    }
                }
                else
                {
                    ValidoEstatusOrden = true;
                }

                return ValidoEstatusOrden;
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return false;

            }
        }

        public bool OrdenesCasadas()
        {
            try
            {
                stringBuilder.Clear();
                bool OrdenCasada = false;

                // Verifico si esta orden es parte de una promoción 2x1

                if (TB_CAORDSER.OTCORRESPONDIENTE != "" && TB_CAORDSER.OTCORRESPONDIENTE != null)
                {
                    OrdenCasada = true;
                    stringBuilder.Append(Environment.NewLine + "¿Esta Orden es parte de una promoción 2 x 1, si la procesa será PROCESADA SIN PAGO la otra orden tambien. Está seguro de querer continuar con las Ordenes Nros " + TB_CAORDSER.NumOrdserv + " y " + TB_CAORDSER.OTCORRESPONDIENTE + " ?");
                }

                else
                {
                    OrdenCasada = false;
                    stringBuilder.Append(Environment.NewLine + "¿Está seguro de procesar sin pago la orden N° " + TB_CAORDSER.NumOrdserv + " ?");
                }

                return OrdenCasada;
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return false;
            }
        }


        private bool Movimiento()
        {
            bool Movimiento = false;

            if (TB_CAORDSER.OrSer_Status == "004")
            {
                DataTable dt = _D_Anulacion.TraerOrdenDet(TB_CAORDSER.NumOrdserv);
                foreach (DataRow row in dt.Rows)
                {
                    if (row["CodArticulo"].ToString().StartsWith("M") || row["CodArticulo"].ToString().StartsWith("L"))
                    {
                        if (TB_CAORDSER.MonturaEnQuorum == false)
                        {

                            _D_DetalleOrden.MovimientoInventario(row["CodArticulo"].ToString(), "006", TB_CAORDSER.NumOrdserv, row["Ordserv_Cant"].ToString(), _D_Anulacion.TraerArt(row["CodArticulo"].ToString()).Replace(",", "."), row["Ordserv_Precio"].ToString().Replace(",", "."), "O", TB_USUARIO.COD_USR, _D_Inicio.Sucursal(), _D_Inicio.DiaActivo().ToString("yyyyMMdd"), TB_CAORDSER.NumOrdserv);
                            if (_D_DetalleOrden.MovimientoInv == "SATISFACTORIO")
                            {
                                Movimiento = true;
                            }
                            else
                            {
                                Movimiento = false;
                                stringBuilder.Append(Environment.NewLine + "Ocurrio un error creando el movimiento de la Orden " + TB_CAORDSER.NumOrdserv + " para el articulo " + row["CodArticulo"].ToString() + ", Error en el Movimiento");
                            }
                        }
                        else
                        {
                            //Movimiento = false;
                            stringBuilder.Append(Environment.NewLine + "Si recibió la montura, recuerde enviarla al laboratorio");
                        }

                    }

                    else
                    {
                
                        _D_DetalleOrden.MovimientoInventario(row["CodArticulo"].ToString(), "006", TB_CAORDSER.NumOrdserv, row["Ordserv_Cant"].ToString(), row["Costo"].ToString().Replace(",", "."), row["Ordserv_Precio"].ToString().Replace(",", "."), "O", TB_USUARIO.COD_USR, _D_Inicio.Sucursal(), _D_Inicio.DiaActivo().ToString("yyyyMMdd"), TB_CAORDSER.NumOrdserv);
                        if (_D_DetalleOrden.MovimientoInv == "SATISFACTORIO")
                        {
                            Movimiento = true;
                        }
                        else
                        {
                            Movimiento = false;
                            stringBuilder.Append(Environment.NewLine + "Ocurrio un error creando el movimiento de la Orden " + TB_CAORDSER.NumOrdserv + " para el articulo " + row["CodArticulo"].ToString() + ", Error en el Movimiento");
                        }

                    }

                }

            }
            //}

            return Movimiento;
        }


        public bool ProcesarSinPago()
        {
            try
            {
                bool ProcesarSinPago = false;

                // Modifico el Estatus para que siga el proceso y Cargo el Abono

                string rep = _D_ListaOrdenes.Procesar_Sin_Pago(_D_Inicio.Sucursal(), TB_CAORDSER.NumOrdserv, "0", "001", "000", "", "", "", 0.00, "EFECTIVO", "XX", "000", true, DateTime.UtcNow.ToString("yyyyMMdd"), "", TB_USUARIO.COD_USR, "", _D_Inicio.DiaActivo().ToString("yyyyMMdd"), "", "", Convert.ToDouble(TB_TASA_Dolar.Tasa), 0, "", "", "01");
                //string rep = _D_ListaOrdenes.Procesar_Sin_Pago(_D_Inicio.Sucursal(), TB_CAORDSER.NumOrdserv, "0", "001", "000", "", "", "", TB_CAORDSER.OrSer_Saldo, "EFECTIVO", "XX", "000", true, DateTime.UtcNow.ToString("yyyyMMdd"), "", TB_USUARIO.COD_USR, "", _D_Inicio.DiaActivo().ToString("yyyyMMdd"), "", "", Convert.ToDouble(TB_TASA_Dolar.Tasa), 0, "", "", "01");
                if (rep == "SATISFACTORIO")
                {
                    // Se crean los movimientos cuandos sea sin pago
                
                    bool Mov = Movimiento();

                    if (Mov == false)
                    {
                        ProcesarSinPago = false;
                    }

                    else
                    {
                        ProcesarSinPago = true;
                    }

                }
                else
                    ProcesarSinPago = false;


                return ProcesarSinPago;
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return false;
            }

        }


        public void MovInventarioLentesContactos()
        {
            try
            {
                stringBuilder.Clear();

                // MovInventario Lentes contactos

                //if (_D_DetalleOrden.TB_PARAMETRO("LCManejaExist") == "1" & TB_CAORDSER.Cod_DetVta == "02" & TB_CAORDSER.Cod_Venta == "004")

                if (_D_DetalleOrden.TB_PARAMETRO("LCManejaExist") == "1" & TB_CAORDSER.Cod_DetVta == "02" )
                {
                    // rebajo la reserva del articulo
                    _D_ListaOrdenes.ReservaLC(TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision);
                    _D_ListaOrdenes.RelacionMov(TB_CAORDSER.NumOrdserv, "O", _D_Inicio.Sucursal(), TB_USUARIO.COD_EMPLEADO, _D_Inicio.DiaActivo().ToString("yyyyMMdd"), TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision);
                }

            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
            }

        }

        public bool PagosdelDia(string NumOrden)
        {
            DateTime DiaAct = Convert.ToDateTime(_D_Inicio.DiaActivo());
            string FechaAct = DiaAct.ToString("yyyyMMdd");
            if (_D_ListaOrdenes.GetPagosdelDia(NumOrden, FechaAct) == true)
            {

                return true;
            }
            else
            {
                return false;
            }

        }

        public String Completar_Numero_Control(string NumeroControl)
        {
            switch (NumeroControl.Length)
            {
                //case 9:
                //    {
                //        NumeroControl = NumeroControl;
                //        break;
                //    }

                //case 8:
                //    {
                //        NumeroControl = "0" + NumeroControl;
                //        break;
                //    }

                case 7:
                    {
                        NumeroControl =  NumeroControl;
                        break;
                    }

                case 6:
                    {
                        NumeroControl = "0" + NumeroControl;
                        break;
                    }

                case 5:
                    {
                        NumeroControl = "00" + NumeroControl;
                        break;
                    }

                case 4:
                    {
                        NumeroControl = "000" + NumeroControl;
                        break;
                    }

                case 3:
                    {
                        NumeroControl = "0000" + NumeroControl;
                        break;
                    }

                case 2:
                    {
                        NumeroControl = "00000" + NumeroControl;
                        break;
                    }

                case 1:
                    {
                        NumeroControl = "000000" + NumeroControl;
                        break;
                    }
            }
            return NumeroControl;

        }

        public DataSet TraerOrdenesConPagoMovil(System.Windows.Forms.DateTimePicker Fechadesde, System.Windows.Forms.DateTimePicker Fechahasta)
        {
            try
            {
                stringBuilder.Clear();

                DateTime PRUE = Fechadesde.Value;
                DateTime PRUEB = Fechahasta.Value;
                string PeriodoDesde = PRUE.ToString("yyyyMMdd");
                string PeriodoHasta = PRUEB.ToString("yyyyMMdd");

                //Le enviamos el index asociados al valor selecionado en el combobox 
                DataSet Ordenes = _D_ListaOrdenes.CargarOrdenesConPagoMovil(PeriodoDesde, PeriodoHasta);

                if (Ordenes.Tables[0].Rows.Count > 0)
                {
                    return Ordenes;
                }
                stringBuilder.Append(Environment.NewLine + "No hay ordenes");
                return null;

            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
            }

        }



        //public bool Verificar_Comprobante_ISLR_IVA(string Cod_Sucursal, string NumOrdserv, bool Iva = false, bool ISLR = false)
        //{
        //    try 
        //    { 
        //    string CodPago = "";

        //    if (Iva == true)
        //    {
        //        CodPago = "013";
        //    }

        //    if (ISLR == true)
        //    {
        //        CodPago = "014";
        //    }

        //   DataTable Pagos_IVA_ISLR=  _D_ListaOrdenes.RevisarAbonos_Epos(Cod_Sucursal, NumOrdserv, CodPago);
        //    if(Pagos_IVA_ISLR!=null)
        //    if(Pagos_IVA_ISLR.Rows.Count> 0)
        //    {
        //            DataTable cOMPROBANTE_IVA_ISLR = _D_ListaOrdenes.RevisarComprobantesRegistardos_IVA_ISLR("086", NumOrdserv);
        //            if (Pagos_IVA_ISLR.Rows.Count > 0)
        //                return false;
        //            else
        //                return true;
        //    }

        //    return false;

        //    }

        //    catch (Exception ex)
        //    {
        //        stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
        //        return false;
        //    }


        //}

    }
}