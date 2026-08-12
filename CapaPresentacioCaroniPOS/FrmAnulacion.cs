using System;
using CapaEntidades;
using System.Configuration;
using CapaLogica.Anulacion_Logica;
using CapaLogica.DetalleOrden_Logica;
using CapaLogica.Inicio_Logica;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaDatos.Inicio_Datos;
using CapaDatos.DetalleOrden_Datos;
using CapaLogica.ListaOrden_Logica;
using CapaLogica.Impresora_Fiscal;
using CapaDatos.Anulacion;
using CapaLogica.Colores_Logica;
using System.Management;
using CapaDatos.Conexion;
using System.Data.SqlClient;
using System.Threading;
using CapaLogica.GiftCard_Logica;
using CapaLogica.Cashea_Logica;

namespace CapaVisual_Login
{
    public partial class FrmAnulacion : Form
    {
        L_Anulacion _L_Anulacion = new L_Anulacion();
        D_Anulacion _D_Anulacion = new D_Anulacion();
        FrmMensajes _FrmMensajes = new FrmMensajes();
        FrmInicio _FrmInicio = new FrmInicio();
        D_Inicio _D_Inicio = new D_Inicio();
        D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        L_ListaOrdenes _ListaOrdenes = new L_ListaOrdenes();
        FrmClaveGerente _FrmClaveGerente = new FrmClaveGerente();
        Impresora_Fiscal _Impresora_Fiscal = new Impresora_Fiscal();
        L_Facturacion _L_Facturacion = new L_Facturacion();
        FrmRepNotaDev _FrmRepNotaDev = new FrmRepNotaDev();
        private readonly L_GiftCard _lGiftCard = new L_GiftCard();
        private L_Cashea _L_Cashea = new L_Cashea();

        private int glbPuertoCOM = Convert.ToInt16(ConfigurationManager.AppSettings.Get("PuertoCOMimpresora"));
        public string NumeroOrden;
        public string CodResp;
        public string CodMoti;
        public string GerenteAutoriza = "";
        public bool AnulacionPagos = false;
        public Double MontoAnulacion = 0.00;
        public string TipoPagoAnular = "";
        public int IdAbono = 0;
        public bool PnlNCManual;
        public string ManualNroNotaCredito = "";
        public string ResultadoNCManual = "";
        public string NumeroNCFiscal = "";
        public string ManualNroNotaCreditonNunControl = "";
        public string codBanco = "";
        public string Abo_CVCNROCHEQUE = "";

        public FrmAnulacion()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Limpiarcbx();
        }

        private void FrmAnulacion_Load(object sender, EventArgs e)
        {
            CbxSelecResp.DataSource = _L_Anulacion.CargarResponsables(TB_USUARIO.COD_SUCURSAL);
            CbxSelecResp.DisplayMember = "Descripcion";
            CbxSelecResp.ValueMember = "Codigo";
            CodResp = CbxSelecResp.SelectedValue.ToString();
            CbxSelectMotivo.DataSource = _L_Anulacion.CargarMotivos(CodResp);
            CbxSelectMotivo.DisplayMember = "Descripcion";
            CbxSelectMotivo.ValueMember = "Codigo";
            //this.Location = new Point(500, 220);
            // 525 location centro del grid
            if (L_Colores.Oscuro == true)
            {

                FormatoOsc();
            }
            else
            {
                FormatoClar();
            }

            CargarConfiguracionGiftCard();



        }

        private void CbxSelecResp_SelectionChangeCommitted(object sender, EventArgs e)
        {
            CodResp = CbxSelecResp.SelectedValue.ToString();
            CbxSelectMotivo.DataSource = _L_Anulacion.CargarMotivos(CodResp);
            CbxSelectMotivo.DisplayMember = "Descripcion";
            CbxSelectMotivo.ValueMember = "Codigo";
        }


        public void CargarOrdenAnular(string NumOrden)
        {
            NumeroOrden = NumOrden;

        }

        public async void BtnGuardar_Click(object sender, EventArgs e)
        {
            //DialogResult = DialogResult.OK;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (CbxSelecResp.Text != "" & CbxSelectMotivo.Text != "" & TxtObservaciones.Text != "")
                {
                    //Anular Orden Abonada
                    if (AnulacionPagos == false & MontoAnulacion == 0.00)
                    {
                        if (TB_CAORDSER.OrSer_Status == "005")
                        {
                            //bool Respuesta = EstaEnLineaLaImpresora();
                            //if (Respuesta == false)
                            //{
                            //    string mensaje = "No se encontró la impresora";
                            //    _FrmMensajes.co = 2;
                            //    _FrmMensajes.avisomensaje(mensaje);
                            //    _FrmMensajes.ShowDialog();
                            //    Limpiarcbx();
                            //    return;
                            //}

                            Anular_Orden_Abonada();
                        }
                    }

                    //Anular Orden Por Pagar (No tiene Abonos)
                    if (AnulacionPagos == false & MontoAnulacion == 0.00)
                    {
                        if (TB_CAORDSER.OrSer_Status == "004" || TB_CAORDSER.OrSer_Status == "007")
                        {
                            Anular_Orden_Por_Pagar();
                            //this.DialogResult = DialogResult.OK;
                        }
                    }


                    //Generar Nota de Credito, Orden Facturada
                    if (AnulacionPagos == false & MontoAnulacion == 0.00)
                    {
                          if (TB_CAORDSER.OrSer_Status == "002")
                            {

                                if (TB_CAORDSER.OTCORRESPONDIENTE != "" && TB_CAORDSER.OTCORRESPONDIENTE != null)
                                {
                                    DataTable OssHijo = _D_DetalleOrden.ObtenerOrdenHijo(TB_CAORDSER.OTCORRESPONDIENTE);
                                    DataTable oFactHijo = _D_DetalleOrden.ObtenerFacturaHijo(TB_CAORDSER.OTCORRESPONDIENTE);

                                    if (TB_CAORDSER.OTCORRESPONDIENTE != ""  & OssHijo.Rows[0]["OrSer_Status"].ToString() != "003")
                                    {
                                        if (oFactHijo.Rows[0]["Fact_Status"].ToString() == "A")
                                        {
                                            _FrmMensajes.co = 2;
                                            _FrmMensajes.avisomensaje("Esta factura es parte de una PROMOCIÓN y tiene una orden asociada. Debe anular primero la orden hijo(fact: " + oFactHijo.Rows[0]["Fact_Num"].ToString() + ") y luego proceda con esta anulación, Proceso no permitido");
                                            _FrmMensajes.ShowDialog();
                                        Limpiarcbx();
                                        return;
                                        }
                                        else
                                        {
                                            _FrmMensajes.co = 2;
                                            _FrmMensajes.avisomensaje("Esta factura es parte de una PROMOCIÓN y tiene una orden asociada la cual se encuentra abonada. Debe anular primero la orden hijo (OS: " + TB_CAORDSER.OTCORRESPONDIENTE.ToString() + ") y luego proceda con esta anulación, Proceso no permitido");
                                            _FrmMensajes.ShowDialog();
                                        Limpiarcbx();
                                        return;
                                        }
                                    }
                                }

                                DataSet dsNCFACT = _D_DetalleOrden.VerificarExistenciaNotaCredito(TB_FACTURAS.Fact_Num, TB_FACTURAS.Fact_SerialImpresora);

                                if (dsNCFACT.Tables[0].Rows.Count > 0)
                                {
                                    _FrmMensajes.co = 2;
                                    _FrmMensajes.avisomensaje("Ya se encuentra registrada una Nota de Crédito de la factura: " + TB_FACTURAS.Fact_Num.ToString() + "-" + TB_FACTURAS.Fact_SerialImpresora.ToString() + ". Comuníquese con el Dpto de Sistemas, Factura anulada");
                                    _FrmMensajes.ShowDialog();
                                Limpiarcbx();
                                return;
                                }

                                if (_L_Facturacion.ValidaFactManual() == false)
                                {
                                   if (_L_Facturacion.stringBuilder.ToString().Length > 2)
                                   {
                                    _FrmMensajes.co = 2;
                                    _FrmMensajes.avisomensaje(_L_Facturacion.stringBuilder.ToString());
                                    _FrmMensajes.ShowDialog();
                                    Limpiarcbx();
                                    return;
                                   }

                                    Nota_Credito_Automatica(_D_Inicio.DiaActivo().ToString("yyyy/MM/dd"));
                                }
                                else
                                {
                                    if (ManualNroNotaCredito != "" & ManualNroNotaCreditonNunControl != "")
                                    {
                                        AnulacionNCManual(ManualNroNotaCredito);
                                    }

                                    else
                                    {
                                        string mensaje = "Error al generar nota de Credito Manual";
                                        _FrmMensajes.co = 2;
                                        _FrmMensajes.avisomensaje(mensaje);
                                        _FrmMensajes.ShowDialog();
                                    Limpiarcbx();
                                    return;
                                    }
                                }

                            }
                        

                        return;
                    }
                    //Fin de Generar Nota de Credito.


                    //AnularPagos de una Orden Abonada
                    if (AnulacionPagos == true & MontoAnulacion > 0.00)
                    {
                        bool Proceso = false;

                        //Es un pago gift card o una os tipo giftCard, anulo la giftcard antes de anular el pago
                        if (codBanco == "115" || TB_CAORDSER.Cod_DetVta == "10")
                        {
                            var resultado = await _lGiftCard.AnularGiftCardPorCodigo(Abo_CVCNROCHEQUE);

                            if (resultado.IsSuccess)
                            {
                                _FrmMensajes.co = 1;
                                _FrmMensajes.avisomensaje("La Gift Card ha sido anulada exitosamente");
                                _FrmMensajes.ShowDialog();

                                _lGiftCard.AgregarGiftCard(TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision, 0, "", "", "", 0, Abo_CVCNROCHEQUE, false, TB_USUARIO.COD_USR, TB_USUARIO.COD_USR);
                                
                                //return;

                                //abonada
                                if (TB_CAORDSER.OrSer_Status == "005")
                                {

                                    bool resp = _L_Anulacion.EliminarPagosActualizarSaldo(IdAbono, TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision, TB_USUARIO.COD_USR, MontoAnulacion, TxtObservaciones.Text, CbxSelectMotivo.SelectedValue.ToString(), CbxSelecResp.SelectedValue.ToString());
                                    string mensaje = _L_Anulacion.stringBuilder.ToString();

                                    if (resp == true)
                                    {
                                        if (_L_Anulacion.ElimineOrden == true)
                                        {

                                            if (_D_DetalleOrden.TB_PARAMETRO("LCManejaExist") == "1" & TB_CAORDSER.Cod_Venta.ToString() == "02")
                                            {
                                                //// Verificar una transferencia de inventario NUEVO 23-07-2024
                                                //if()
                                                //{

                                                //}

                                                DataSet dsEjecutaMovimiento = _D_DetalleOrden.MovimientosAnulacionOSLC(TB_FACTURAS.NumOrdServ, "005", "N", _D_Inicio.DiaActivo().ToString("yyyyMMdd"), _Impresora_Fiscal.NumeroNCFiscal, TB_USUARIO.COD_USR);
                                            }

                                            else
                                            {
                                                //_L_Anulacion.CargarDetalleOrd(TB_CAORDSER.NumOrdserv, "ND", "003");
                                                _L_Anulacion.CargarDetalleOrd(TB_CAORDSER.NumOrdserv, "AN", "003");
                                                if (_L_Anulacion.GuardoMovimientoArticulo == false)
                                                {
                                                    _FrmMensajes.co = 2;
                                                    _FrmMensajes.avisomensaje("Ocurrio un error creando el movimiento de la orden");
                                                    _FrmMensajes.ShowDialog();

                                                }
                                                if (_L_Anulacion.MontRecib == true)
                                                {
                                                    _FrmMensajes.co = 1;
                                                    _FrmMensajes.avisomensaje("Si recibió la montura, recuerde enviarla al laboratorio");
                                                    _FrmMensajes.ShowDialog();
                                                }
                                            }

                                        }
                                    }

                                    else
                                    {
                                        _FrmMensajes.co = 2;
                                        _FrmMensajes.avisomensaje(mensaje);
                                        _FrmMensajes.ShowDialog();
                                    }


                                    _D_Anulacion.CaragarAuditor(TB_USUARIO.COD_SUCURSAL, "029", TB_USUARIO.COD_EMPLEADO, "OS: " + TB_CAORDSER.NumOrdserv + ", Monto: " + Convert.ToString(MontoAnulacion) + " Autoriza: " + GerenteAutoriza);
                                    _FrmMensajes.co = 1;
                                    _FrmMensajes.avisomensaje("Los pagos han sido eliminados");
                                    _FrmMensajes.ShowDialog();

                                    Limpiarcbx();

                                    this.DialogResult = DialogResult.OK;
                                    this.Close();
                                }


                                //facturada
                                if (TB_CAORDSER.OrSer_Status == "002")
                                {
                                    bool resp = _L_Anulacion.EliminarPagosActualizarSaldo(IdAbono, TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision, TB_USUARIO.COD_USR, MontoAnulacion, TxtObservaciones.Text, CbxSelectMotivo.SelectedValue.ToString(), CbxSelecResp.SelectedValue.ToString());
                                    string mensaje = _L_Anulacion.stringBuilder.ToString();

                                    if (resp == true)
                                    {
                                        if (_L_Anulacion.ElimineOrden == true)
                                        {
                                            if (_D_DetalleOrden.TB_PARAMETRO("LCManejaExist") == "1" & TB_CAORDSER.Cod_Venta.ToString() == "02")
                                            {
                                                DataSet dsEjecutaMovimiento = _D_DetalleOrden.MovimientosAnulacionOSLC(TB_FACTURAS.NumOrdServ, "005", "N", _D_Inicio.DiaActivo().ToString("yyyyMMdd"), _Impresora_Fiscal.NumeroNCFiscal, TB_USUARIO.COD_USR);
                                            }

                                            else
                                            {
                                                _L_Anulacion.CargarDetalleOrd(TB_CAORDSER.NumOrdserv, "ND", "003");
                                                if (_L_Anulacion.GuardoMovimientoArticulo == false)
                                                {
                                                    _FrmMensajes.co = 2;
                                                    _FrmMensajes.avisomensaje("Ocurrio un error creando el movimiento de la orden");
                                                    _FrmMensajes.ShowDialog();

                                                }
                                                if (_L_Anulacion.MontRecib == true)
                                                {
                                                    _FrmMensajes.co = 1;
                                                    _FrmMensajes.avisomensaje("Si recibió la montura, recuerde enviarla al laboratorio");
                                                    _FrmMensajes.ShowDialog();
                                                }
                                            }

                                        }


                                        resp = _L_Anulacion.EliminarFacturaLogico(TB_FACTURAS.Cod_Sucursal, TB_FACTURAS.Fact_Num, TB_FACTURAS.Fact_SerialImpresora, TB_USUARIO.COD_USR);
                                        mensaje = _L_Anulacion.stringBuilder.ToString();

                                        if (resp == false)
                                        {
                                            _FrmMensajes.co = 2;
                                            _FrmMensajes.avisomensaje(mensaje);
                                            _FrmMensajes.ShowDialog();
                                        }

                                        _D_Anulacion.CaragarAuditor(TB_USUARIO.COD_SUCURSAL, "029", TB_USUARIO.COD_EMPLEADO, TB_CAORDSER.NumOrdserv + ", Monto: " + Convert.ToString(MontoAnulacion) + " Autoriza: " + GerenteAutoriza);
                                        _FrmMensajes.co = 1;
                                        _FrmMensajes.avisomensaje("La factura y sus pagos han sido eliminados");
                                        _FrmMensajes.ShowDialog();

                                        Limpiarcbx();
                                    }

                                    else
                                    {
                                        _FrmMensajes.co = 2;
                                        _FrmMensajes.avisomensaje(mensaje);
                                        _FrmMensajes.ShowDialog();
                                    }

                                }
                            }
                            else
                            {
                                _FrmMensajes.co = 1;
                                _FrmMensajes.avisomensaje("La Gift Card no pudo ser anulada");
                                _FrmMensajes.ShowDialog();
                            }
                        }

                        else
                        {
                            //abonada
                            if (TB_CAORDSER.OrSer_Status == "005")
                            {

                                bool resp = _L_Anulacion.EliminarPagosActualizarSaldo(IdAbono, TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision, TB_USUARIO.COD_USR, MontoAnulacion, TxtObservaciones.Text, CbxSelectMotivo.SelectedValue.ToString(), CbxSelecResp.SelectedValue.ToString());
                                string mensaje = _L_Anulacion.stringBuilder.ToString();

                                if (resp == true)
                                {
                                    if (_L_Anulacion.ElimineOrden == true)
                                    {

                                        if (_D_DetalleOrden.TB_PARAMETRO("LCManejaExist") == "1" & TB_CAORDSER.Cod_Venta.ToString() == "02")
                                        {
                                            //// Verificar una transferencia de inventario NUEVO 23-07-2024
                                            //if()
                                            //{

                                            //}

                                            DataSet dsEjecutaMovimiento = _D_DetalleOrden.MovimientosAnulacionOSLC(TB_FACTURAS.NumOrdServ, "005", "N", _D_Inicio.DiaActivo().ToString("yyyyMMdd"), _Impresora_Fiscal.NumeroNCFiscal, TB_USUARIO.COD_USR);
                                        }

                                        else
                                        {
                                            //_L_Anulacion.CargarDetalleOrd(TB_CAORDSER.NumOrdserv, "ND", "003");
                                            _L_Anulacion.CargarDetalleOrd(TB_CAORDSER.NumOrdserv, "AN", "003");
                                            if (_L_Anulacion.GuardoMovimientoArticulo == false)
                                            {
                                                _FrmMensajes.co = 2;
                                                _FrmMensajes.avisomensaje("Ocurrio un error creando el movimiento de la orden");
                                                _FrmMensajes.ShowDialog();

                                            }
                                            if (_L_Anulacion.MontRecib == true)
                                            {
                                                _FrmMensajes.co = 1;
                                                _FrmMensajes.avisomensaje("Si recibió la montura, recuerde enviarla al laboratorio");
                                                _FrmMensajes.ShowDialog();
                                            }
                                        }

                                    }
                                }

                                else
                                {
                                    _FrmMensajes.co = 2;
                                    _FrmMensajes.avisomensaje(mensaje);
                                    _FrmMensajes.ShowDialog();
                                }


                                _D_Anulacion.CaragarAuditor(TB_USUARIO.COD_SUCURSAL, "029", TB_USUARIO.COD_EMPLEADO, "OS: " + TB_CAORDSER.NumOrdserv + ", Monto: " + Convert.ToString(MontoAnulacion) + " Autoriza: " + GerenteAutoriza);
                                _FrmMensajes.co = 1;
                                _FrmMensajes.avisomensaje("Los pagos han sido eliminados");
                                _FrmMensajes.ShowDialog();

                                Limpiarcbx();
                            }


                            //facturada
                            if (TB_CAORDSER.OrSer_Status == "002")
                            {
                                bool resp = _L_Anulacion.EliminarPagosActualizarSaldo(IdAbono, TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision, TB_USUARIO.COD_USR, MontoAnulacion, TxtObservaciones.Text, CbxSelectMotivo.SelectedValue.ToString(), CbxSelecResp.SelectedValue.ToString());
                                string mensaje = _L_Anulacion.stringBuilder.ToString();

                                if (resp == true)
                                {
                                    if (_L_Anulacion.ElimineOrden == true)
                                    {
                                        if (_D_DetalleOrden.TB_PARAMETRO("LCManejaExist") == "1" & TB_CAORDSER.Cod_Venta.ToString() == "02")
                                        {
                                            DataSet dsEjecutaMovimiento = _D_DetalleOrden.MovimientosAnulacionOSLC(TB_FACTURAS.NumOrdServ, "005", "N", _D_Inicio.DiaActivo().ToString("yyyyMMdd"), _Impresora_Fiscal.NumeroNCFiscal, TB_USUARIO.COD_USR);
                                        }

                                        else
                                        {
                                            _L_Anulacion.CargarDetalleOrd(TB_CAORDSER.NumOrdserv, "ND", "003");
                                            if (_L_Anulacion.GuardoMovimientoArticulo == false)
                                            {
                                                _FrmMensajes.co = 2;
                                                _FrmMensajes.avisomensaje("Ocurrio un error creando el movimiento de la orden");
                                                _FrmMensajes.ShowDialog();

                                            }
                                            if (_L_Anulacion.MontRecib == true)
                                            {
                                                _FrmMensajes.co = 1;
                                                _FrmMensajes.avisomensaje("Si recibió la montura, recuerde enviarla al laboratorio");
                                                _FrmMensajes.ShowDialog();
                                            }
                                        }

                                    }


                                    resp = _L_Anulacion.EliminarFacturaLogico(TB_FACTURAS.Cod_Sucursal, TB_FACTURAS.Fact_Num, TB_FACTURAS.Fact_SerialImpresora, TB_USUARIO.COD_USR);
                                    mensaje = _L_Anulacion.stringBuilder.ToString();

                                    if (resp == false)
                                    {
                                        _FrmMensajes.co = 2;
                                        _FrmMensajes.avisomensaje(mensaje);
                                        _FrmMensajes.ShowDialog();
                                    }

                                    _D_Anulacion.CaragarAuditor(TB_USUARIO.COD_SUCURSAL, "029", TB_USUARIO.COD_EMPLEADO, TB_CAORDSER.NumOrdserv + ", Monto: " + Convert.ToString(MontoAnulacion) + " Autoriza: " + GerenteAutoriza);
                                    _FrmMensajes.co = 1;
                                    _FrmMensajes.avisomensaje("La factura y sus pagos han sido eliminados");
                                    _FrmMensajes.ShowDialog();

                                    Limpiarcbx();
                                }

                                else
                                {
                                    _FrmMensajes.co = 2;
                                    _FrmMensajes.avisomensaje(mensaje);
                                    _FrmMensajes.ShowDialog();
                                }

                            }
                        }
                        

                       

                    }

                }

                else
                {
                    this.Close();
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("Debe completar todos los campos para continuar");
                    _FrmMensajes.ShowDialog();
                    if (_FrmMensajes.DialogResult == DialogResult.OK)
                    {
                        this.Visible = false;// De esta manera hago que cuando se le de ok al mensaje no se cierre la anulacion 
                        FrmAnulacion anulacion = new FrmAnulacion(); // Para que el mensaje no me cierre este formulario tengo que intanciar anulacion y volverlo a cargar  
                        anulacion.ShowDialog();

                    }
                }

                this.Cursor = Cursors.Default;
               
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
                
            }
            finally
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private async Task<decimal?> ConsultarGiftCard(string codigoGiftCard)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(codigoGiftCard))
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("Por favor, ingrese un código de Gift Card válido.");
                    _FrmMensajes.ShowDialog();
                    return null; // ❌ Cambiado de false a null
                }

                var resultado = await _lGiftCard.ConsultarGiftCardPorCodigo(codigoGiftCard);


                if (resultado.IsSuccess && resultado.Data != null)
                {
                    string codigoTarjeta = resultado.Data.Code;
                    decimal saldoRestante = resultado.Data.Remaining;
                    string estado = resultado.Data.IsActive;
                    int idInternoWoo = resultado.Data.Id;

                    if (estado.ToLower() == "on")
                    {
                        if (saldoRestante <= 0)
                        {
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje($"La Gift Card está activa, pero no tiene saldo disponible");
                            _FrmMensajes.ShowDialog();
                            return null; // ❌ Cambiado de false a null
                        }

                        // ==========================================================
                        // 🌟 ¡LA CLAVE!: RETENEMOS LOS DATOS EN LAS VARIABLES GLOBALES
                        // ==========================================================
                        //_idgiftCardWebValidado = idInternoWoo;
                        //_codigoGiftCardValidado = codigoTarjeta;
                        // ==========================================================

                        //string mensajeExito = $"¡Gift Card válida!\n\nCódigo: {codigoTarjeta}\nSaldo Disponible: {saldoRestante:N2} USD";
                        //_FrmMensajes.co = 1;
                        //_FrmMensajes.avisomensaje(mensajeExito);
                        //_FrmMensajes.ShowDialog();

                        return saldoRestante; // 🌟 ¡LA CLAVE!: Retornamos el valor decimal directamente
                    }
                    else
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("La Gift Card consultada se encuentra inactiva.");
                        _FrmMensajes.ShowDialog();
                        return null;
                    }
                }
                else
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje($"Código de Gift Card inválido");
                    _FrmMensajes.ShowDialog();
                    return null;
                }
            }
            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje($"Ocurrió un error inesperado al consultar: {ex.Message}");
                _FrmMensajes.ShowDialog();
                return null;
            }
        }

        public void AnulacionNCManual(string NroNotaCredito) // para realizar anulacion con la nota de credito manual
        {
            string SucursalActual = _D_Inicio.Sucursal();
            string rept= "";
            string fecha_dia_activo = _D_Inicio.DiaActivo().ToString("yyyy/MM/dd");

            try
            {

                Conexion cn = new Conexion();
                SqlConnection connection = cn.LeerCadena();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction();
                command.Connection = connection;
                command.Transaction = transaction;
                command.Parameters.Clear();

                //Generar Nota de Credito MANUAL

                // Cambio el status en Caorser
                rept = _D_DetalleOrden.ActualizarCaorser(CbxSelectMotivo.SelectedValue.ToString(), CbxSelecResp.SelectedValue.ToString(), TB_FACTURAS.NumOrdServ, "0", TB_USUARIO.COD_USR, command);
                        if (rept == "SATISFACTORIO")
                            // Ejecuto el movimiento 
                            _L_Anulacion.CargarDetalleOrd(NumeroNCFiscal, "N", "005", command);

                        if (_L_Anulacion.GuardoMovimientoArticulo == false)
                        {
                            string mensaje = "Ocurrio un error creando el movimiento de la orden";
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje(mensaje);
                            _FrmMensajes.ShowDialog();
                            rept = "error";
                            transaction.Rollback();
                            Limpiarcbx();
                            return;
                        }

                        else
                        {
                            rept = "SATISFACTORIO";
                        }
                        if (_L_Anulacion.MontRecib == true)
                        {
                            string mensaje = "Si recibió la montura, recuerde enviarla al laboratorio";
                            _FrmMensajes.co = 1;
                            _FrmMensajes.avisomensaje(mensaje);
                            _FrmMensajes.ShowDialog();

                        }

                        if (rept == "SATISFACTORIO")
                        {
                            rept = _D_DetalleOrden.RegistrarNCFISCAL(SucursalActual, NroNotaCredito, "005", TB_FACTURAS.Fact_Num, TB_FACTURAS.Fact_SerialImpresora,fecha_dia_activo, TB_FACTURAS.Fact_SerialImpresora, "", TB_FACTURAS.CTE_NacioPAG, TB_FACTURAS.CTE_CedIdenPAG, TxtObservaciones.Text.ToUpper(), Convert.ToDouble(TB_FACTURAS.Fact_MontoGravable), Convert.ToDouble(TB_FACTURAS.Fact_Total), TB_USUARIO.COD_USR, "", Convert.ToDouble(TB_FACTURAS.Fact_IGTF), Convert.ToDouble(TB_FACTURAS.Fact_AlicuotaIGTF), Convert.ToDouble(TB_FACTURAS.Fact_MontoExento), CbxSelectMotivo.SelectedValue.ToString(), true, ManualNroNotaCreditonNunControl, command);

                            if (rept == "SATISFACTORIO")
                            {
                                transaction.Commit();
                                ResultadoNCManual = "SATISFACTORIO";
                                _D_Anulacion.CaragarAuditor(TB_USUARIO.COD_SUCURSAL, "016", TB_USUARIO.COD_EMPLEADO, "Factura: " + TB_FACTURAS.Fact_Num + ", Cliente: " + TB_FACTURAS.CTE_NacioPAG + "-" + TB_FACTURAS.CTE_CedIdenPAG + ", Nº Nota: " + NroNotaCredito + ", Monto: " + TB_FACTURAS.Fact_Total + ", Autoriza: " + GerenteAutoriza);

                                if (_D_DetalleOrden.TB_PARAMETRO("LCManejaExist") == "1" & TB_CAORDSER.Cod_Venta.ToString() == "02")
                                {
                                    DataSet dsEjecutaMovimiento = _D_DetalleOrden.MovimientosAnulacionOSLC(TB_FACTURAS.NumOrdServ, "005", "N", _D_Inicio.DiaActivo().ToString("yyyyMMdd"), NroNotaCredito, TB_USUARIO.COD_USR);

                                }

                                Limpiarcbx();
                                return;
                            }

                            else
                            {
                                ResultadoNCManual = "error";
                                transaction.Rollback();
                                Limpiarcbx();
                                _FrmMensajes.co = 2;
                                _FrmMensajes.avisomensaje("No se pudo procesar nota de crédito manual, comunicarse con el Dpto de sistemas");
                                _FrmMensajes.ShowDialog();
                                return;
                            }
                               
                        }

                        else 
                        {
                            transaction.Rollback();
                            Limpiarcbx();
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje("No se pudo procesar nota de crédito manual, comunicarse con el Dpto de sistemas");
                            _FrmMensajes.ShowDialog();
                            return;
                        }

            }
            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }


        }


        //public void Limpiarcbx()
        //{
        //    this.Hide();
        //    CbxSelecResp.DataSource = _L_Anulacion.CargarResponsables(TB_USUARIO.COD_SUCURSAL);
        //    CbxSelecResp.DisplayMember = "Descripcion";
        //    CbxSelecResp.ValueMember = "Codigo";
        //    CodResp = CbxSelecResp.SelectedValue.ToString();
        //    CbxSelectMotivo.DataSource = _L_Anulacion.CargarMotivos(CodResp);
        //    CbxSelectMotivo.DisplayMember = "Descripcion";
        //    CbxSelectMotivo.ValueMember = "Codigo";
        //    TxtObservaciones.Text = "";


        //}

        public void Limpiarcbx()
        {
            // 1. Cargamos el ComboBox de Responsables
            CbxSelecResp.DataSource = _L_Anulacion.CargarResponsables(TB_USUARIO.COD_SUCURSAL);
            CbxSelecResp.DisplayMember = "Descripcion";
            CbxSelecResp.ValueMember = "Codigo";

            // 2. Validación SEGURA contra Null antes de hacer ToString()
            if (CbxSelecResp.SelectedValue != null)
            {
                CodResp = CbxSelecResp.SelectedValue.ToString();
            }
            else
            {
                CodResp = string.Empty; // o null, según cómo manejes la variable
            }

            // 3. Cargamos el ComboBox de Motivos
            CbxSelectMotivo.DataSource = _L_Anulacion.CargarMotivos(CodResp);
            CbxSelectMotivo.DisplayMember = "Descripcion";
            CbxSelectMotivo.ValueMember = "Codigo";

            TxtObservaciones.Text = "";
        }


        public bool ImprimirNCFiscal_Local(string SucursalActual, string NumeroFactura, string SerialImpresora, double Monto, string DiaActivo, string CodMotivoAnulacion, SqlCommand command, SqlTransaction transaction)
        {

            VmaxComVe.VmaxComClass objVmax = new VmaxComVe.VmaxComClass();
            uint resp = 0;
            string status = "";
            DataTable dtFacturas;
            DataTable dtCliente;
            bool StatusNoataCredito = true;
            string CedulaCliente = "";
            string NacionalidadCliente = "";
            string NunOrden = "";
            DateTime FechaOperacion = DateTime.Today;
            string PrimerNombre = "";
            string PrimerApellido = "";
            string SerialImpresoraNC = "";
            string FechaImpresora = "";
            string mensaje = "No hay conexión con la impresora fiscal";
            string UltimoNumeroNotaCancelado;
            string NumeroComprobanteFiscal;

            try
            {

                //status = objVmax.RetornoStatusImpresora.sStatus;

                //if (SucursalActual != "")
                //{
                    dtFacturas = _D_DetalleOrden.BucarFactura(NumeroFactura, SerialImpresora, SucursalActual, command);

                    if (dtFacturas.Rows.Count > 0)
                    {
                        foreach (DataRow drItem in dtFacturas.Rows)
                        {
                            CedulaCliente = drItem["CTE_CedIdenPAG"].ToString();
                            NacionalidadCliente = drItem["CTE_NacioPAG"].ToString();
                            FechaOperacion = Convert.ToDateTime(TB_FACTURAS.Fact_FecCrea);
                            NunOrden = drItem["NumOrdServ"].ToString();
                        break;
                        }
                    }

                    dtCliente = _D_DetalleOrden.BucarTB_CTEPPAL(TB_FACTURAS.CTE_CedIdenPAG, TB_FACTURAS.CTE_NacioPAG, command);

                    if (dtCliente.Rows.Count > 0)
                    {
                        foreach (DataRow drItem in dtCliente.Rows)
                        {
                            PrimerNombre = drItem["CTE_PNombre"].ToString();
                            PrimerApellido = drItem["CTE_PApellido"].ToString();
                        break;
                        }

                    }

                    resp = objVmax.AbrirPuerto(Convert.ToString(1));

                //do
                //{
                //    if (_Impresora_Fiscal.VerficarConexionImpresoraFiscalSinCerrar()) break;
                //    mensaje = _Impresora_Fiscal.stringBuilder.ToString();
                //    _FrmMensajes.co = 2;
                //    _FrmMensajes.avisomensaje(mensaje);
                //    _FrmMensajes.ShowDialog();
                //    _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "090", TB_USUARIO.COD_EMPLEADO, "No hay conexión con la impresora fiscal");
                //} while (true);

                //objVmax.ObtenerContadores();
                //UltimoNumeroNotaCancelado = objVmax.RetornoContadores.uiUltNCAnulada.ToString().PadLeft(7, '0');
                //NumeroComprobanteFiscal = objVmax.RetornoContadores.uiUltNCAbierta.ToString().PadLeft(7, '0');

                //if (UltimoNumeroNotaCancelado == NumeroComprobanteFiscal)
                //{
                //    return false;
                //}

                //if (resp != 0)
                //{
                //        objVmax.Cancelar();
                //        objVmax.Cerrar();
                //        objVmax.CerrarPuerto();
                //        StatusNoataCredito = false;
                //        _FrmMensajes.co = 2;
                //        _FrmMensajes.avisomensaje("No hay conexión con la impresora fiscal");
                //        _FrmMensajes.ShowDialog();
                //        Impresora_Fiscal.AgregarAccionPendiente("096");
                //       return StatusNoataCredito;

                //}
                //else
                //{
                        //resp = objVmax.AbrirCF("Jesus Antonio Pabon Mavare", "J309860895", "2", "000000262", "TIU2202214", "26032022", "1055", 40);
                    resp = objVmax.AbrirCF(PrimerNombre + " " + PrimerApellido, Convert.ToString(TB_FACTURAS.CTE_NacioPAG) + "" + Convert.ToString(TB_FACTURAS.CTE_CedIdenPAG), "2", Convert.ToString(TB_FACTURAS.Fact_Num), Convert.ToString(TB_FACTURAS.Fact_SerialImpresora), Convert.ToDateTime(TB_FACTURAS.Fact_FecCrea).ToString("dd/MM/yyyy"), FechaOperacion.ToString("HH:mm"), 40);

                //NumeroNCFiscal = (Convert.ToInt32(objVmax.RetornoMF.uiTotalNCDiarias) + 1).ToString();
                    objVmax.ObtenerContadores();
                    NumeroNCFiscal = objVmax.RetornoContadores.uiUltNCAbierta.ToString();
                    NumeroNCFiscal = NumeroNCFiscal.PadLeft(7, '0');
                    do
                    {
                        if (_Impresora_Fiscal.VerficarConexionImpresoraFiscalSinCerrar()) break;
                        mensaje = _Impresora_Fiscal.stringBuilder.ToString();
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje(mensaje);
                        _FrmMensajes.ShowDialog();
                        _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "090", TB_USUARIO.COD_EMPLEADO, "No hay conexión con la impresora fiscal");
                    } while (true);

                    objVmax.ObtenerContadores();
                    UltimoNumeroNotaCancelado = objVmax.RetornoContadores.uiUltNCAnulada.ToString().PadLeft(7, '0');
                    //NumeroComprobanteFiscal = objVmax.RetornoAbrirFactura.uiNumeroFactura.ToString();

                    if (UltimoNumeroNotaCancelado == NumeroNCFiscal)
                    {
                        return false;
                    }
                    //if (resp != 0)
                    //    {
                        //NumeroNCFiscal = objVmax.RetornoAbrirFactura.uiNumeroFactura.ToString(); 
                        //StatusNoataCredito = false;
                        //    objVmax.Cancelar();
                        //    objVmax.Cerrar();
                        //    objVmax.CerrarPuerto();
                        //    _FrmMensajes.co = 2;
                        //    _FrmMensajes.avisomensaje("Error inesperado, al generar la NC");
                        //    _FrmMensajes.ShowDialog();
                        //    Impresora_Fiscal.AgregarAccionPendiente("096");
                        //return StatusNoataCredito;
                     //   }

                     

                    //if (resp == 0)
                    //{

                    DataSet dsArti = _D_DetalleOrden.DetalleNotaCreditoFiscal(NumeroFactura, SerialImpresora,command);
                    string desart;
                double MontoDonacion = 0;

                foreach (DataRow drItem in dsArti.Tables[0].Rows)
                    {
                    if (drItem["CodArticulo"].ToString().StartsWith("H"))
                    {
                        MontoDonacion = Convert.ToDouble(drItem["Ordserv_Bruto"]) - 0.01;
                    }
                    if (Convert.ToUInt32(drItem["Ordserv_Dto"].ToString()) > 0)
                        {
                        }

                        desart = drItem["CodArticulo"] + " " + drItem["DESART"];
                        //Valido que el texto no sea mayor a 40 caracteres para que no salga duplicado el articulo en la factura 25-05-2023
                        desart = Validar_Cadena(desart);

                        string Impuesto = "";
                        if (drItem["ART_EXENTO"].ToString() == "1")
                        {
                           Impuesto = "0";
                        }
                        else
                        {
                           Impuesto = "1";
                        }

                        resp = objVmax.ItemDev(desart, (Convert.ToDouble(drItem["Ordserv_Cant"]) * 1000).ToString(), drItem["OrdServ_Precio"].ToString(), Impuesto, "1", 40);

                        do
                        {
                            if (_Impresora_Fiscal.VerficarConexionImpresoraFiscalSinCerrar()) break;
                            mensaje = _Impresora_Fiscal.stringBuilder.ToString();
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje(mensaje);
                            _FrmMensajes.ShowDialog();
                            _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "090", TB_USUARIO.COD_EMPLEADO, "No hay conexión con la impresora fiscal");
                        } while (true);

                        objVmax.ObtenerContadores();
                        UltimoNumeroNotaCancelado = objVmax.RetornoContadores.uiUltNCAnulada.ToString().PadLeft(7, '0');
                        //NumeroComprobanteFiscal = objVmax.RetornoAbrirFactura.uiNumeroFactura.ToString();

                        if (UltimoNumeroNotaCancelado == NumeroNCFiscal)
                        {
                            return false;
                        }
                    }

                    DataSet dsDcto = _D_DetalleOrden.DescuentosNotaCreditoFiscal(NunOrden, command);

                    if (resp == 0)
                    {
                        foreach (DataRow drItemdcto in dsDcto.Tables[0].Rows)
                        {
                            if(Convert.ToDouble(drItemdcto["DescuentoExento"])>0)
                            {

                                resp = objVmax.DescuentoCF("Descuento", drItemdcto["DescuentoExento"].ToString(), drItemdcto["DescuentoGravable"].ToString(), "", "");
                                do
                                {
                                    if (_Impresora_Fiscal.VerficarConexionImpresoraFiscalSinCerrar()) break;
                                    mensaje = _Impresora_Fiscal.stringBuilder.ToString();
                                    _FrmMensajes.co = 2;
                                    _FrmMensajes.avisomensaje(mensaje);
                                    _FrmMensajes.ShowDialog();
                                    _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "090", TB_USUARIO.COD_EMPLEADO, "No hay conexión con la impresora fiscal");
                                } while (true);

                                objVmax.ObtenerContadores();
                                UltimoNumeroNotaCancelado = objVmax.RetornoContadores.uiUltNCAnulada.ToString().PadLeft(7, '0');
                                //NumeroComprobanteFiscal = objVmax.RetornoAbrirFactura.uiNumeroFactura.ToString();

                                if (UltimoNumeroNotaCancelado == NumeroNCFiscal)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                            //if (resp == 0)
                            //{
                    DataSet DtIGTF = _D_DetalleOrden.PagosConIGTF_NotaCredito(TB_FACTURAS.Cod_Sucursal, TB_FACTURAS.NumOrdServ, TB_FACTURAS.Revision, command);
                    Double subtotal = 0.00;
                    Double subtotalIgtf = 0.00;

                    // ******ENVIO EL SUBTOTAL DE LA FACTURA**************
                        foreach (DataRow drIgtf in DtIGTF.Tables[0].Rows)
                        {
                            subtotal = subtotal + Convert.ToDouble(drIgtf["Abo_Monto"].ToString()) / 100;
                            subtotalIgtf = subtotalIgtf + Convert.ToDouble(drIgtf["IGTFCALC1"].ToString());
                        }
                        // Verifico si la orden tiene Igtf 1
                        if (Convert.ToBoolean(DtIGTF.Tables[0].Rows[0]["ActivaIGTF"].ToString()) == true & DtIGTF.Tables[0].Rows[0]["Abo_Monto"].ToString() != "0")
                            resp = objVmax.SubtotalT_sinRetorno(Convert.ToString(subtotal * 100).Replace(".", ","));
                        else
                        resp = objVmax.Subtotal();
                        
                        do
                        {
                            if (_Impresora_Fiscal.VerficarConexionImpresoraFiscalSinCerrar()) break;
                            mensaje = _Impresora_Fiscal.stringBuilder.ToString();
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje(mensaje);
                            _FrmMensajes.ShowDialog();
                            _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "090", TB_USUARIO.COD_EMPLEADO, "No hay conexión con la impresora fiscal");
                        } while (true);

                        objVmax.ObtenerContadores();
                        UltimoNumeroNotaCancelado = objVmax.RetornoContadores.uiUltNCAnulada.ToString().PadLeft(7, '0');
                        //NumeroComprobanteFiscal = objVmax.RetornoAbrirFactura.uiNumeroFactura.ToString();

                        if (UltimoNumeroNotaCancelado == NumeroNCFiscal)
                        {
                            return false;
                        }
                        // *****Texto no fiscal *****
                        resp = objVmax.TextoNoFiscal("Monto Disponible:  " + (Monto).ToString());
                                        objVmax.ObtenerReporteInformativo();
                                        SerialImpresoraNC = objVmax.RetornoMI.sSerial;
                                        FechaImpresora = objVmax.RetornoMI.sFecha;
                       resp = objVmax.Cerrar();
                        do
                        {
                            if (_Impresora_Fiscal.VerficarConexionImpresoraFiscalSinCerrar()) break;
                            mensaje = _Impresora_Fiscal.stringBuilder.ToString();
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje(mensaje);
                            _FrmMensajes.ShowDialog();
                            _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "090", TB_USUARIO.COD_EMPLEADO, "No hay conexión con la impresora fiscal");
                        } while (true);

                        objVmax.ObtenerContadores();
                        UltimoNumeroNotaCancelado = objVmax.RetornoContadores.uiUltNCAnulada.ToString().PadLeft(7, '0');
                        //NumeroComprobanteFiscal = objVmax.RetornoAbrirFactura.uiNumeroFactura.ToString();

                        if (UltimoNumeroNotaCancelado == NumeroNCFiscal)
                        {
                            return false;
                        }
                        resp = objVmax.CerrarPuerto();


                        //}
                    //}
                    //if (resp == 0)
                    //{
                           

                        //switch (NumeroNCFiscal.Length)
                        //    {
                        //        case 7:
                        //            {
                        //                NumeroNCFiscal = NumeroNCFiscal;
                        //                break;
                        //            }

                        //        case 6:
                        //            {
                        //                NumeroNCFiscal = "0" + NumeroNCFiscal;
                        //                break;
                        //            }

                        //        case 5:
                        //            {
                        //                NumeroNCFiscal = "00" + NumeroNCFiscal;
                        //                break;
                        //            }

                        //        case 4:
                        //            {
                        //                NumeroNCFiscal = "000" + NumeroNCFiscal;
                        //                break;
                        //            }

                        //        case 3:
                        //            {
                        //                NumeroNCFiscal = "0000" + NumeroNCFiscal;
                        //                break;
                        //            }

                        //        case 2:
                        //            {
                        //                NumeroNCFiscal = "00000" + NumeroNCFiscal;
                        //                break;
                        //            }

                        //        case 1:
                        //            {
                        //                NumeroNCFiscal = "000000" + NumeroNCFiscal;
                        //                break;
                        //            }
                        //}
                        //int tiempoImpTermica = Convert.ToInt32(_D_DetalleOrden.TB_PARAMETRO("TiempoImpTerm"));
                        //// Esperar un tiempo para que la impresora emita el ticket
                        //Thread.Sleep(tiempoImpTermica);  // Esperar 15 segundos (ajusta el tiempo según sea necesario)
                        //objVmax.AbrirPuerto(Convert.ToString(glbPuertoCOM));
                        //objVmax.ObtenerContadores();
                        //string UltimoNumeroNotaEmitido2 = objVmax.RetornoContadores.uiUltNCAbierta.ToString().PadLeft(7, '0');
                        //string UltimoNumeroCancelado11 = objVmax.RetornoContadores.uiUltFacturaAnulada.ToString().PadLeft(7, '0');
                        //string UltimoNumeroCancelado12 = objVmax.RetornoContadores.uiUltNCAnulada.ToString().PadLeft(7, '0');
                        //objVmax.CerrarPuerto();
                        //objVmax.Cerrar();
                        // Verificar si se emitió el ticket
                        if (UltimoNumeroNotaCancelado != NumeroNCFiscal)// & UltimoNumeroCancelado12 != NumeroNCFiscal)
                        {
                            string Transaccionn = _D_DetalleOrden.RegistrarNCFISCAL(SucursalActual, NumeroNCFiscal, "005", TB_FACTURAS.Fact_Num, SerialImpresora, DiaActivo, SerialImpresoraNC, "", TB_FACTURAS.CTE_NacioPAG, TB_FACTURAS.CTE_CedIdenPAG, TxtObservaciones.Text.ToUpper(), Convert.ToDouble(TB_FACTURAS.Fact_MontoGravable), Convert.ToDouble(TB_FACTURAS.Fact_Total), TB_USUARIO.COD_USR, "", Convert.ToDouble(TB_FACTURAS.Fact_IGTF), Convert.ToDouble(TB_FACTURAS.Fact_AlicuotaIGTF), Convert.ToDouble(TB_FACTURAS.Fact_MontoExento), CodMotivoAnulacion, false, "" , command);

                            if (Transaccionn == "SATISFACTORIO")
                            {
                                StatusNoataCredito = true;
                            }

                            else
                            {
                                Impresora_Fiscal.AgregarAccionPendiente("101");
                                StatusNoataCredito = false;
                                _FrmMensajes.co = 2;
                                _FrmMensajes.avisomensaje("Por favor comunicarse con el Dpto de sistemas y reportar el siguiente error: " + string.Format("Error: {0}", Transaccionn) + ", Error inesperado");
                                _FrmMensajes.ShowDialog();
                                //stringBuilder.Append("Por favor comunicarse con el Dpto de sistemas y reportar el siguiente error: " + Environment.NewLine + string.Format("Error: {0}", Transaccion));
                            }

                            return StatusNoataCredito;
                        }
                //    else
                //    {
                ////Impresora_Fiscal.AgregarAccionPendiente("094");
                //_D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "094", TB_USUARIO.COD_EMPLEADO, "Nota de credito fiscal reversada N° " + NumeroNCFiscal + " ,Factura:" + TB_FACTURAS.Fact_Num);

                ////StatusNoataCredito = false;
                //        _FrmMensajes.co = 2;
                //        _FrmMensajes.avisomensaje("No se pudo verificar la emisión del ticket.");
                //        _FrmMensajes.ShowDialog();
                //        return StatusNoataCredito;

                //    }
                //}
                //else
                //{
                //    Impresora_Fiscal.AgregarAccionPendiente("099");
                //}

                //}
                //objVmax.Cancelar();
                //objVmax.Cerrar();
                //objVmax.CerrarPuerto();
                //_FrmMensajes.co = 2;
                //_FrmMensajes.avisomensaje("Error inesperado al generar la NC");
                //_FrmMensajes.ShowDialog();
                //return false;
                return true;

            }
            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Por favor comunicarse con el Dpto de sistemas y reportar el siguiente error: " + string.Format("Error: {0}", ex) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
                objVmax.Cancelar();
                objVmax.Cerrar();
                objVmax.CerrarPuerto(); 
                return false;
                

            }
        }

        public string Validar_Cadena(string _cadena)
        {
            int MaxLength = 39;
            string Cadena = "";

            if (_cadena.Length > MaxLength)
            {
                Cadena = _cadena.Substring(0, MaxLength);
            }

            else
            {
                Cadena = _cadena;
            }

            return Cadena;
        }

        public void FormatoOsc()
        {

            System.Drawing.Color col2 = System.Drawing.ColorTranslator.FromHtml("#257b78");// color anterior
           // System.Drawing.Color col2 = System.Drawing.ColorTranslator.FromHtml(" #07a79b"); AZUL MAS CLARO
            this.BackColor = col2;
            LblResponsable.ForeColor = Color.White;
            LblMotivo.ForeColor = Color.White;
            //LblObservacion.ForeColor = Color.White;
            System.Drawing.Color col3 = System.Drawing.ColorTranslator.FromHtml(" #07a79b");
            TxtObservaciones.BackColor = col3;
            TxtObservaciones.ForeColor = Color.White;



        }
        public void FormatoClar()
        {
            System.Drawing.Color col1 = System.Drawing.ColorTranslator.FromHtml("#ffffff");
            this.BackColor = col1;
            LblResponsable.ForeColor = Color.DarkGray;
            LblMotivo.ForeColor = Color.DarkGray;
            //LblObservacion.ForeColor = Color.DarkGray;
            TxtObservaciones.BackColor = Color.White;
            TxtObservaciones.ForeColor = Color.Black;
        }


        public bool EstaEnLineaLaImpresora()
        {
            string str = "";
            bool online = false;
            string Impresora = (ConfigurationManager.AppSettings.Get("ImpresoraTienda")); // Impresora definidad desde el config 

            ManagementScope scope = new ManagementScope("root\\CIMV2");
            scope.Connect();

            //Consulta para obtener las impresoras, en la API Win32
            SelectQuery query = new SelectQuery("select * from Win32_Printer");

            ManagementClass m = new ManagementClass("Win32_Printer");

            ManagementObjectSearcher obj = new ManagementObjectSearcher(scope, query);

            //Obtenemos cada instancia del objeto ManagementObjectSearcher
            using (ManagementObjectCollection printers = m.GetInstances())
                foreach (ManagementObject printer in printers)
                {
                    if (printer != null)
                    {
                        //Obtenemos la primera impresora en el bucle
                        str = printer["Name"].ToString().ToLower();
                        string impresora2 = Impresora.ToLower();
                        impresora2 = (impresora2.Substring(impresora2.LastIndexOf("\\"), impresora2.Length- impresora2.LastIndexOf("\\"))).Replace("\\", "");
                        if (str.Equals(Impresora.ToLower()))
                        {
                            //Una vez encontrada verificamos el estado de ésta
                            if (printer["WorkOffline"].ToString().ToLower().Equals("true") || printer["PrinterStatus"].Equals(7))
                                //Fuera de línea
                                online = false;
                            else
                                //En línea
                                online = true;
                        }
                        // por si no consigue el nombre hacemos un like 
                        else if (str.Contains(impresora2))
                        {
                            //Una vez encontrada verificamos el estado de ésta
                            if (printer["WorkOffline"].ToString().ToLower().Equals("true") || printer["PrinterStatus"].Equals(7))
                                //Fuera de línea
                                online = false;
                            else
                                //En línea
                                online = true;
                        }

                    }
                    else
                        throw new Exception("No fueron encontradas impresoras instaladas en el equipo");
                }

            return online;
        }

        public string MovInventario(SqlCommand command)
        {
            // Para hacer el movimiento de inventario 
            string mensaje="";
            _L_Anulacion.CargarDetalleOrd(TB_CAORDSER.NumOrdserv, "ND", "003", command); // Se coloca antes de la impresion de manera que si no encuentra la impresora el cacth no nos salte nada importante
            if (_L_Anulacion.GuardoMovimientoArticulo == false)
            {
                mensaje = "Ocurrio un error creando el movimiento de la orden";
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(mensaje);
                _FrmMensajes.ShowDialog();
            }

            else
            {
                mensaje = "SATISFACTORIO";
            }

            if (_L_Anulacion.MontRecib == true)
            {
                _FrmMensajes.co = 1;
                _FrmMensajes.avisomensaje("Si recibió la montura, recuerde enviarla al laboratorio");
                _FrmMensajes.ShowDialog();

            }

            return mensaje;
        }

        private void ProcesarAccionesPendientes()
        {
            if (Impresora_Fiscal.TieneAccionesPendientes())
            {
                var acciones = Impresora_Fiscal.ObtenerAccionesPendientes();

                foreach (var accion in acciones)
                {
                    _D_Anulacion.CaragarAuditor(
                        _D_Inicio.Sucursal(),
                        accion.Key,  // Código
                        TB_USUARIO.COD_EMPLEADO,
                        $"Proceso Nota de credito, Numero de Factura: {TB_FACTURAS.Fact_Num}, Error: {accion.Value}"
                    );
                }

                Impresora_Fiscal.LimpiarAccionesPendientes();
            }
        }

        public async void Nota_Credito_Automatica(string DiaActivo) // para realizar anulacion con la nota de credito manual
        {
            // Si hay commit, limpiamos acciones pendientes
            Impresora_Fiscal.LimpiarAccionesPendientes();

            bool ReversoAutomatico = false;
            string Resultado_Parametro = _D_DetalleOrden.TB_PARAMETRO("ReversoAuto");
            ReversoAutomatico = Convert.ToBoolean(Convert.ToInt32(Resultado_Parametro));

            Conexion cn = new Conexion();
            SqlConnection connection = cn.LeerCadena();
            SqlCommand command = connection.CreateCommand();
            SqlTransaction transaction;
            transaction = connection.BeginTransaction();
            command.Connection = connection;
            command.Transaction = transaction;
            command.Parameters.Clear();
            command.CommandTimeout = 120;
            string rept = "";

            try 
            {

                string mensaje;
                _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "095", TB_USUARIO.COD_EMPLEADO, "Número de factura " + TB_FACTURAS.Fact_Num + " Valor del Parametro Reverso Automático." + ReversoAutomatico.ToString(), command);
                _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "093", TB_USUARIO.COD_EMPLEADO, "Número de factura " + TB_FACTURAS.Fact_Num + " En proceso de nota de crédito.");

                //-----------------------Despues de Validar Continuo el proceso
                // Cambio el status en Caorser
                rept = _D_DetalleOrden.ActualizarCaorser(CbxSelectMotivo.SelectedValue.ToString(), CbxSelecResp.SelectedValue.ToString(), TB_FACTURAS.NumOrdServ, "0", TB_USUARIO.COD_USR, command);
                if (rept == "SATISFACTORIO")
                    // Ejecuto el movimiento 
                    _L_Anulacion.CargarDetalleOrd(NumeroNCFiscal, "N", "005", command);

                if (_L_Anulacion.GuardoMovimientoArticulo == false)
                {
                    string statusActual;
                    statusActual = _D_DetalleOrden.ObtieneStatusOrden(TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision, command);
                    if (statusActual == "003")
                    {
                         mensaje = "La nota ya fue generada";
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje(mensaje);
                        _FrmMensajes.ShowDialog();
                        command.Transaction.Rollback();

                        return;
                    }
                     mensaje = "Ocurrio un error creando el movimiento de la orden";
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje(mensaje);
                    _FrmMensajes.ShowDialog();
                    rept = "error";
                    transaction.Rollback();
                    Limpiarcbx();
                    return;
                }

                else
                {
                    rept = "SATISFACTORIO";
                }
                if (rept == "SATISFACTORIO" & TB_CAORDSER.MonturaEnQuorum == true)
                {
                     mensaje = "Si recibió la montura, recuerde enviarla al laboratorio";
                    _FrmMensajes.co = 1;
                    _FrmMensajes.avisomensaje(mensaje);
                    _FrmMensajes.ShowDialog();

                }
                if (rept == "SATISFACTORIO")
                {
                    bool Impresion = false;

                    DataTable Pagos = _L_Facturacion.MostarPagosGrid(TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision);

                    // Verifica directamente si al menos una fila cumple la condición
                    // Busca la primera fila que coincida con el banco 115 (retorna null si no la encuentra)
                    DataRow filaBanco = Pagos.AsEnumerable()
                                             .FirstOrDefault(row => row["cod_banco"].ToString() == "115");

                    
                    if (filaBanco != null)
                    {
                        // Obtenemos el valor de la columna Abo_CVCNROCHEQUE
                        string codGiftCard = filaBanco["Abo_CVCNROCHEQUE"].ToString();

                        var resultado = await _lGiftCard.AnularGiftCardPorCodigo(codGiftCard);

                        if (resultado.IsSuccess)
                        {
                            _lGiftCard.AgregarGiftCard(TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision, 0, "", "", "", 0, codGiftCard, false, TB_USUARIO.COD_USR, TB_USUARIO.COD_USR);

                            Impresion = ImprimirNCFiscal_Local(TB_FACTURAS.Cod_Sucursal, TB_FACTURAS.Fact_Num, TB_FACTURAS.Fact_SerialImpresora, Convert.ToDouble(TB_FACTURAS.Fact_Total), DiaActivo, CbxSelectMotivo.SelectedValue.ToString(), command, transaction);

                        }
                        else
                        {
                            mensaje = "No se pudo anular la Gift Card";
                            _FrmMensajes.co = 1;
                            _FrmMensajes.avisomensaje(mensaje);
                            _FrmMensajes.ShowDialog();
                            return;
                        }
                    }
                    else
                    {
                         Impresion = ImprimirNCFiscal_Local(TB_FACTURAS.Cod_Sucursal, TB_FACTURAS.Fact_Num, TB_FACTURAS.Fact_SerialImpresora, Convert.ToDouble(TB_FACTURAS.Fact_Total), DiaActivo, CbxSelectMotivo.SelectedValue.ToString(), command, transaction);

                    }
                    if (Impresion == false)
                    {

                        if (ReversoAutomatico)
                        {

                            command.Transaction.Rollback();

                            // ✅ FUNCIÓN EXISTENTE Para Registras las nuevas Acciones
                            ProcesarAccionesPendientes();

                            _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "094", TB_USUARIO.COD_EMPLEADO, "Nota de credito fiscal reversada N° " + NumeroNCFiscal + " ,Factura:" + TB_FACTURAS.Fact_Num);

                        }
                        else
                        {
                            // Si hay commit, limpiamos acciones pendientes
                            Impresora_Fiscal.LimpiarAccionesPendientes();
                            command.Transaction.Commit();
                        }

                        Limpiarcbx();
                        return;

                    }

                    else
                    {
                        transaction.Commit();
                        _D_Anulacion.CaragarAuditor(TB_USUARIO.COD_SUCURSAL, "016", TB_USUARIO.COD_EMPLEADO, "Factura:" + TB_FACTURAS.Fact_Num + ", Cliente: " + TB_FACTURAS.CTE_NacioPAG + "-" + TB_FACTURAS.CTE_CedIdenPAG + ", Nº Nota: " + NumeroNCFiscal + ", Monto: " + TB_FACTURAS.Fact_Total + ", Autoriza: " + GerenteAutoriza);
                        //Movimiento Inventario
                        if (_D_DetalleOrden.TB_PARAMETRO("LCManejaExist") == "1" & TB_CAORDSER.Cod_Venta.ToString() == "02")
                        {
                            DataSet dsEjecutaMovimiento = _D_DetalleOrden.MovimientosAnulacionOSLC(TB_FACTURAS.NumOrdServ, "005", "N", _D_Inicio.DiaActivo().ToString("yyyyMMdd"), NumeroNCFiscal, TB_USUARIO.COD_USR);

                        }

                        Limpiarcbx();
                    }

                }

                else
                {
                    if (ReversoAutomatico)
                    {
                        transaction.Rollback();
                        _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "095", TB_USUARIO.COD_EMPLEADO, "Número de factura " + TB_FACTURAS.Fact_Num + " Valor del Parametro Reverso Automático." + ReversoAutomatico.ToString(), command);
                        _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "094", TB_USUARIO.COD_EMPLEADO, "Nota de credito fiscal reversada N° " + NumeroNCFiscal + " ,Factura:" + TB_FACTURAS.Fact_Num);
                    }
                    else
                        command.Transaction.Commit();

                    Limpiarcbx();
                    return;
                }


            }
            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();

                try
                {
                    // Attempt to roll back the transaction.
                    transaction.Rollback();
                }
                catch (Exception exRollback)
                {
                    // Throws an InvalidOperationException if the connection
                    // is closed or the transaction has already been rolled
                    // back on the server.
                    Console.WriteLine(exRollback.Message);
                }

            }


        }

        public void Anular_Orden_Por_Pagar() // para realizar anulacion con la nota de credito manual
        {
            try
            {
                Conexion cn = new Conexion();
                SqlConnection connection = cn.LeerCadena();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction();
                command.Connection = connection;
                command.Transaction = transaction;
                command.Parameters.Clear();
                string rept = "";

                CodMoti = CbxSelectMotivo.SelectedValue.ToString();
                string observaciones = TxtObservaciones.Text;

                if (TB_CAORDSER.OTCORRESPONDIENTE != "" && TB_CAORDSER.OTCORRESPONDIENTE != null)
                {
                    DataTable OssHijo = _D_DetalleOrden.ObtenerOrdenHijo(TB_CAORDSER.OTCORRESPONDIENTE);
                    //DataTable oFactHijo = _D_DetalleOrden.ObtenerFacturaHijo(TB_CAORDSER.OTCORRESPONDIENTE);

                    if (TB_CAORDSER.OTCORRESPONDIENTE != "" & OssHijo.Rows[0]["OrSer_Status"].ToString() != "003")
                    {
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje("Esta orden es parte de una PROMOCIÓN y tiene una orden asociada. Debe anular primero la orden hijo (OS: " + TB_CAORDSER.OTCORRESPONDIENTE.ToString() + ") y luego proceda con esta anulación, Proceso no permitido");
                            _FrmMensajes.ShowDialog();
                            Limpiarcbx();
                            return;
                    }
                }

                rept = _L_Anulacion.Anulacion(TB_CAORDSER.NumOrdserv, CodMoti, CodResp, observaciones, TB_USUARIO.COD_USR, "00", command);
                if (rept == "SATISFACTORIO")
                    _L_Anulacion.EnviarAuditor("012", VariablesGlobales.UsuarioAutorizado_FrmClaveGerente, command);

                if (rept == "SATISFACTORIO")
                {
                    if (rept == "SATISFACTORIO")
                    {
                        transaction.Commit();
                    }

                    else
                    {
                        transaction.Rollback();
                        Limpiarcbx();
                        return;
                    }
                }

                string mensaje = "Se anulo la orden satisfactoriamente";
                _FrmMensajes.co = 1;
                _FrmMensajes.avisomensaje(mensaje);
                _FrmMensajes.ShowDialog();

                Limpiarcbx();
                return;

            }

            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }

        }

         public async void Anular_Orden_Abonada() // para realizar anulacion con la nota de credito manual
         {
            try
            {
                // Saber el total de pagos que tiene la orden 
                decimal Total_Pagos= _L_Anulacion.Saldo_Total_Orden(TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision);

                Conexion cn = new Conexion();
                SqlConnection connection = cn.LeerCadena();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction();
                command.Connection = connection;
                command.Transaction = transaction;
                command.Parameters.Clear();
                string rept = "";

                string DiaActivo = _D_Inicio.DiaActivo().ToString("yyyyMMdd");

                //Verificar si los pagos son del dia activo 
                bool TienePagosDia = _L_Anulacion.ValidarPagosDia(TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv, DiaActivo, command);

                if (_L_Anulacion.stringBuilder.ToString().Length > 2)
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje(_L_Anulacion.stringBuilder.ToString());
                    _FrmMensajes.ShowDialog();
                    Limpiarcbx();
                    transaction.Rollback();
                    return;
                }

                if (TienePagosDia == true)
                {
                    _FrmMensajes.co = 1;
                    _FrmMensajes.avisomensaje("No es posible ejecutar este proceso si hay pagos en el día");
                    _FrmMensajes.ShowDialog();
                    Limpiarcbx();
                    transaction.Rollback();
                    return;
                }

               CodMoti = CbxSelectMotivo.SelectedValue.ToString();
                string observaciones = TxtObservaciones.Text;

                DataTable Pagos = _L_Facturacion.MostarPagosGrid(TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision);

                // Verifica directamente si al menos una fila cumple la condición
                // Busca la primera fila que coincida con el banco 115 (retorna null si no la encuentra)
                DataRow filaBanco = Pagos.AsEnumerable()
                                         .FirstOrDefault(row => row["cod_banco"].ToString() == "115");


                if (filaBanco != null)
                {
                    // Obtenemos el valor de la columna Abo_CVCNROCHEQUE
                    string codGiftCard = filaBanco["Abo_CVCNROCHEQUE"].ToString();

                    var resultado = await _lGiftCard.AnularGiftCardPorCodigo(codGiftCard);

                    if (resultado.IsSuccess)
                    {
                        _lGiftCard.AgregarGiftCard(TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision, 0, "", "", "",0, codGiftCard, false, TB_USUARIO.COD_USR, TB_USUARIO.COD_USR);
                    }
                    else
                    {
                        string mensaje = "No se pudo anular la Gift Card";
                        _FrmMensajes.co = 1;
                        _FrmMensajes.avisomensaje(mensaje);
                        _FrmMensajes.ShowDialog();
                        return;
                    }
                }

                rept = _L_Anulacion.Anulacion(TB_CAORDSER.NumOrdserv, CodMoti, CodResp, observaciones, TB_USUARIO.COD_USR, "01", command);
                if (rept == "SATISFACTORIO")
                    rept = MovInventario(command);
                if (rept == "SATISFACTORIO" && Total_Pagos > 0) // si el total de pagos es mayor a 0 generamos nota de Devolucion 
                    rept = _L_Anulacion.EnviarDatoaNotaDev(observaciones, CbxSelectMotivo.SelectedValue.ToString(), command); // Genera la nota de devolucion 
                if (rept == "SATISFACTORIO")
                {
                    rept = _L_Anulacion.EnviarGarantia();
                    _L_Anulacion.EnviarAuditor_Nota("015", VariablesGlobales.UsuarioAutorizado_FrmClaveAutorizada, _L_Anulacion.NroNota, _L_Anulacion.MontoNota, command);
                }

                //Nuevo Orden Antiguas con Lentes de contacto 02-06-2023
                //Revisar despues para que utiliza epos los datos de este select 
                if (_D_DetalleOrden.TB_PARAMETRO("LCManejaExist") == "1" & TB_CAORDSER.Cod_Venta.ToString() == "02" & TB_CAORDSER.OrSer_Status == "005" & rept == "SATISFACTORIO")
                {
                    DataSet dst = _D_Anulacion.OSAntiguaLC(TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision, command);

                }


                // Ejecuta el movimiento de anulacion de lentes de contacto

                if (_D_DetalleOrden.TB_PARAMETRO("LCManejaExist") == "1" & TB_CAORDSER.Cod_Venta.ToString() == "02" & rept == "SATISFACTORIO")
                {
                    rept = _L_Anulacion.EnviarMovAnulacion(command);
                }


                    if (rept == "SATISFACTORIO")
                    {

                    transaction.Commit();

                    if (Total_Pagos > 0)
                    {
                        _FrmMensajes.co = 1;
                        _FrmMensajes.avisomensaje("El número de nota de devolución es " + _L_Anulacion.NroNota);
                        _FrmMensajes.ShowDialog();

                        string concat = TB_CAORDSER.Cod_Sucursal + TB_CAORDSER.NumOrdserv + TB_CAORDSER.Revision;

                        // Para mostrar o imprimir el reporte de nota de devolucion 
                        _FrmRepNotaDev.setParametros(concat);
                        _FrmRepNotaDev.ConfigRep();

                        if (_D_DetalleOrden.ParametroImpresion() == "1")
                        {
                            _FrmRepNotaDev.imprimir(); // imprime 

                        }
                        else
                        {
                            _FrmRepNotaDev.ShowDialog(); // Muestra
                        }
                    }
                    }

                    else
                    {
                        transaction.Rollback();
                        Limpiarcbx();
                        return;
                    }

                Limpiarcbx();
                return;

            }
                        

            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }
         }

        public void CargarConfiguracionGiftCard()
        {
            DataTable dt = _L_Cashea.ObtenerConfigCashea("GIFTCARD");

            foreach (DataRow row in dt.Rows)
            {
                string nombre = row["Parametro"].ToString();
                string valorCifrado = row["Valor"].ToString();

                switch (nombre)
                {
                    case "GiftCard_ConsumerKey":
                        ConfigServiciosExternos.GiftCard_ConsumerKey = ConfigServiciosExternos.Decodificar(valorCifrado);
                        break;
                    case "GiftCard_ConsumerSecret":
                        ConfigServiciosExternos.GiftCard_ConsumerSecret = ConfigServiciosExternos.Decodificar(valorCifrado);
                        break;
                    case "GiftCard_BaseUrl":
                        ConfigServiciosExternos.GiftCard_BaseUrl = ConfigServiciosExternos.Decodificar(valorCifrado);
                        break;
                }
            }
        }
    }
}
