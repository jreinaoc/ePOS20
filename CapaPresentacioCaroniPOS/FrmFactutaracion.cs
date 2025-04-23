using CapaDatos.DetalleOrden_Datos;
using CapaEntidades;
using CapaLogica.DetalleOrden_Logica;
using CapaLogica.Impresora_Fiscal;
using CapaLogica.Anulacion_Logica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaDatos.Inicio_Datos;
using CapaLogica.Login_Logica;
using CapaDatos.Anulacion;
using CapaLogica.DanaService_Logic;
using CapaDatos.DanaService_Datos;
using CapaLogica.ListaOrden_Logica;
using System.Data.SqlClient;
using CapaDatos.Conexion;
using System.Threading;

namespace CapaVisual_Login
{
    public partial class FrmFacturacion : Form
    {
        public FrmFacturacion()
        {
            InitializeComponent();
            ttMensaje.SetToolTip(this.btnExamen, "Examen");
            ttMensaje.SetToolTip(this.btnDetalleOrden, "Detalle de la orden");
            ttMensaje.SetToolTip(this.btnPrincipal, "Principal");


        }
        private int glbPuertoCOM = Convert.ToInt16(ConfigurationManager.AppSettings.Get("PuertoCOMimpresora"));

        L_Anulacion _LAnulacion = new L_Anulacion();
        public static FrmPrincipal _FrmPrincipal = new FrmPrincipal();
        public static FrmPrincipal GetInstancia()//para cargar formulario de busqueda de modelos
        {
            if (_FrmPrincipal == null)//sino se ha cargado el formulario buscar
            {
                _FrmPrincipal = new FrmPrincipal(); //que cree una nueva isndtancia del mismo
            }

            return _FrmPrincipal;

        }

        FrmMostarDeclaracion _FrmMostarDeclaracion = new FrmMostarDeclaracion();
        FrmRepOrden _FrmRepOrden = new FrmRepOrden();
        FrmRepOrdenTContact _FrmRepOrdenTContact = new FrmRepOrdenTContact();
        FrmMostrarReporte _FrmMostrarReporte = new FrmMostrarReporte();
        FrmRepContratGart _FrmRepContrat = new FrmRepContratGart();
        FrmAnulacion _FrmAnulacion = new FrmAnulacion();
        FrmClaveAutorizada _FrmClaveAutorizada = new FrmClaveAutorizada();
        FrmClaveGerente _FrmClaveGerente = new FrmClaveGerente();
        D_Anulacion _D_Anulacion = new D_Anulacion();
        D_Dana _D_Dana = new D_Dana();
        L_DanaService _L_DanaService = new L_DanaService();
        FrmMostrarRep _FrmMostrarRep = new FrmMostrarRep();

        D_Inicio _D_Inicio = new D_Inicio();
        private L_Facturacion _L_Facturacion = new L_Facturacion();
        private L_ListaOrdenes _L_ListaOrdenes = new L_ListaOrdenes();
        private Impresora_Fiscal _Impresora_Fiscal = new Impresora_Fiscal();
        private FrmMensajes _FrmMensajes = new FrmMensajes();
        private D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        public DataTable Dt_Abonos = new DataTable();
        public DataTable Dt_Billetes = new DataTable();
        public DataTable Dt_PagoMovil = new DataTable();
        string ClienteAnterior;
        string CedulaAnterior;
        string TlfAnterior;
        string CorreoAnterior;
        public bool FalBod = false;
        bool MovInventarioo = false;
        private string Medio_Pago = "";
        private string mensaje = "";
        private string NumeroComprobanteFiscal = "";
        private bool FacturaManual;
        private bool ImprimirFacturaFiscall;
        private string TotalFacturaFiscal = "";
        private string NotaNumFactura = "";
        private string NotaNumOrden = "";
        private string NotaNumNota = "";
        private string NotaNumNotaDevolucion = "";
        private string PosicionInical = "";
        private string PosicionInicalNotaDevolucion = "";
        private string MotivoNota = "";
        private string MotivoNotaDevolucion = "";
        //--------colocar la referencia 
        private VmaxComVe.VmaxComClass objVmax = new VmaxComVe.VmaxComClass();
        private string Num_Factura = "";
        public bool ModoClaro = true;
        private bool rollbackRealizado = false;
        //---------AGREGADO POR Angel Hernandez para controlar ciertas funcionalidades del btn de eliminar pagos
        public bool HabEliminarPagos = false;

        ToolTip toolTip1 = new ToolTip();
        ToolTip toolTip2 = new ToolTip();
        public int CantAbonosPrevios = 0;
        public int idAbonoPagoMovil = 0;
        private void FrmDetalleOrden_Load(object sender, EventArgs e)
        {
            LimpiaVariablesIdAbonoPagoMovil();
            tabControl.SelectTab(0);

            LimpiarGrid();
            _L_Facturacion.CargarTasa();
            GbxVentasDia.Size = new System.Drawing.Size(1048, 154);
            _L_Facturacion.ComboboxTipoMoneda(CbxMoneda);
            _L_Facturacion.LLenarComboboxPagos(CbxMetodosPago);
            _L_Facturacion.LLenarComboboxPagos(CbxMetodosPago2);
            FormatoTexbox();
            _L_Facturacion.CrearTabla(Dt_Abonos);
            _L_Facturacion.CrearTablaPagoMovil(Dt_PagoMovil);
            DgvAbonos.DataSource = Dt_Abonos;
            FormatoDataGrid();
            lbTasa.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", Math.Round(Convert.ToDouble(TB_TASA_Dolar.Tasa), 2)).Replace(".", ",");
            _L_Facturacion.ComboboxTipoTarjeta(CbxTarjeta);
            DtpFecha.Text = DateTime.UtcNow.ToShortDateString();
            CbxMetodosPago.SelectedIndex = 2;
            TxtNumFact.Text = _D_DetalleOrden.ParametroSerieManual();

            // Monto de los billetes
            CbxBillete.Items.Add("20");
            CbxBillete.Items.Add("50");
            CbxBillete.Items.Add("100");
            _L_Facturacion.CrearTablaBilletes(Dt_Billetes);

            
        }

        public void ColorearStatus()
        {
            System.Drawing.Color colfact = System.Drawing.ColorTranslator.FromHtml("#84fb6c");
            System.Drawing.Color colabon = System.Drawing.ColorTranslator.FromHtml("#f59e51");
            System.Drawing.Color colporp = System.Drawing.ColorTranslator.FromHtml("#e9e9e9");
            System.Drawing.Color colanul = System.Drawing.ColorTranslator.FromHtml("#ff353a");


            try
            {

                if (TxtStatus.Text == "Facturada")
                {
                    TxtStatus.BackColor = colfact;
                }

                if (TxtStatus.Text == "Anulada")
                {
                    TxtStatus.BackColor = colanul;
                }

                if (TxtStatus.Text == "Por pagar")
                {
                    TxtStatus.BackColor = colporp;
                }

                if (TxtStatus.Text == "Abonada")
                {
                    TxtStatus.BackColor = colabon;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }
        }


        private void btnIngresar_Click(object sender, EventArgs e)
        {

            LimpiaVariablesIdAbonoPagoMovil();
            //Validacion del dia activo 
            string DiaActual = (DateTime.Now.ToString("dd/MM/yyyy"));
            string DiaActivo = _D_Inicio.DiaActivo().ToShortDateString();


            if (DiaActivo != DiaActual)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Debe cerrar caja del día anterior para continuar");
                _FrmMensajes.ShowDialog();
                return;

            }

            //validar si es factura manual 

            if (_L_Facturacion.ValidaFactManual() == false)
            {
                if (_L_Facturacion.stringBuilder.ToString().Length > 2)
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje(_L_Facturacion.stringBuilder.ToString());
                    _FrmMensajes.ShowDialog();
                    return;
                }

                LblNroFactura.Visible = false;
                TxtNumFact.Visible = false;

                LblNroCorrelativo.Visible = false;
                TxtNroCorrelativo.Visible = false;
            }
            else
            {
                LblNroFactura.Visible = true;
                TxtNumFact.Visible = true;

                LblNroCorrelativo.Visible = true;
                TxtNroCorrelativo.Visible = true;


            }

            VisualizarPanel("MostrarPanelPrincipal");
            Double Valor = _D_DetalleOrden.MontoMinimoAbono();
            lbMinAbo.Text = Convert.ToString(Math.Round(Valor * Convert.ToDouble((String.Format(CultureInfo.InvariantCulture, "{0:0.00}", TB_CAORDSER.VtaTotal).Replace(".", ",")).Replace(".", ",")), 2));
            HabilitacionControl("Bloquear"); // inhabilita  los controles de atras 


            if (TB_CAORDSER.OrSer_Status != "004")
            {
                label25.Visible = false;
                label25.Enabled = false;
                lbMinAbo.Visible = false;
                lbMinAbo.Enabled = false;
            }
            BolivaresConveridos(TB_CAORDSER.OrSer_Saldo, txtMontoBs);
            //-----------ConvertirBolivares---------------------------
            //txtMontoBs.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", TB_CAORDSER.OrSer_Saldo).Replace(".", ",");

        }

        public void VisualizarPanel(string Case)
        {

            switch (Case)
            {
                case "MostrarPanelPrincipal":
                    this.PnlPrimario.Enabled = true;
                    this.PnlPrimario.Visible = true;
                    this.PnlPrimario.Location = new Point(250, 1);
                    this.PnlPrimario.BringToFront();
                    this.PnlSecundario.Visible = false;
                    this.PnlSecundario.Enabled = false;
                    _L_Facturacion.ComboboxTipoMoneda(CbxMoneda);
                    break;

                case "MostrarPanelSecundario":
                    this.PnlSecundario.Enabled = true;
                    this.PnlSecundario.Visible = true;
                    this.PnlSecundario.Location = new Point(250, 1);
                    this.PnlSecundario.BringToFront();
                    this.PnlPrimario.Visible = false;
                    this.PnlPrimario.Enabled = false;

                    break;

                case "MostrarFormulario":
                    this.PnlPrimario.Visible = false;
                    this.PnlPrimario.Enabled = false;
                    this.PnlSecundario.Visible = false;
                    this.PnlSecundario.Enabled = false;
                    Dt_Abonos.Rows.Clear();
                    break;

                case "MostrarNotasCredito":
                    this.PnlNotaCredito.Enabled = true;
                    this.PnlNotaCredito.Visible = true;
                    this.PnlNotaCredito.Location = new Point(250, 1);
                    this.PnlNotaCredito.BringToFront();
                    this.PnlPrimario.Visible = false;
                    this.PnlPrimario.Enabled = false;

                    break;

                case "MostrarNotasDevolucion":
                    this.PnlNotaDevolucion.Enabled = true;
                    this.PnlNotaDevolucion.Visible = true;
                    this.PnlNotaDevolucion.Location = new Point(250, 1);
                    this.PnlNotaDevolucion.BringToFront();
                    this.PnlPrimario.Visible = false;
                    this.PnlPrimario.Enabled = false;

                    break;

                default:
                    break;
            }


        }


        private void txtRef_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8)
            {
                e.Handled = false;
                return;
            }


            bool IsDec = false;
            int nroDec = 0;


            if (txtRef.SelectionLength <= 0)
            {

                for (int i = 0; i < txtRef.Text.Length; i++)
                {
                    if (txtRef.Text[i] == ',')
                        IsDec = true;

                    if (IsDec && nroDec++ >= 2)
                    {
                        e.Handled = true;
                        return;
                    }


                }

            }

            if (e.KeyChar >= 44 && e.KeyChar <= 57)
                e.Handled = false;
            else if (e.KeyChar == 46)
                e.Handled = (IsDec) ? true : false;
            else
                e.Handled = true;


            //para que solo acepte numeros y una sola coma
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }


            if (txtRef.Text.Contains("") && e.KeyChar == 44)
            {
                //separamos por punto
                string[] parts = txtRef.Text.Split(',');
                //si el primer elemento está vacío, significa que no se escribió nada antes de, entonces, añadimos el cero al textbox.
                if (parts[0].Length <= 0)
                {
                    txtRef.Text = "0" + txtRef.Text;
                    //UPDATE: colocamos el cursor al final del texto
                    txtRef.SelectionStart = txtRef.Text.Length;
                }
            }

            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);
                //-----------ConvertirBolivares---------------------------
                //txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);

                string Resultado_Parametro = _D_DetalleOrden.TB_PARAMETRO("ActivaIGTF");
                bool Cobro_IGTF = Convert.ToBoolean(Convert.ToInt32(Resultado_Parametro));
                double MontoFaltanteIgtfbs = 0.00;
                if (Cobro_IGTF == true)
                {
                    MontoFaltanteIgtfbs = Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) * 0.03;
                }
                else
                {
                    MontoFaltanteIgtfbs = 0.00;
                }

                double IgtfAbonado = Convert.ToDouble(TxtIgtfOrd.Text.Replace(".", "")) + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos).Replace(".", ""));
                if (CbxMoneda.SelectedIndex.ToString() == "0")
                {

                    if (Math.Round((double)((Convert.ToDouble(txtMontoBs.Text.Replace(".","")) - IgtfAbonado) / TB_TASA_Dolar.Tasa), 2) >= (txtRef.Text == "" ? (Double)0.00 : Convert.ToDouble(txtRef.Text)))
                    {
                        _L_Facturacion.ConvertirDolaresBolivares(txtRef, txtMonto2Bs, CbxMoneda.Text);
                        txtIGTF.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", _L_Facturacion.CalculoIgtf(txtNumeroOrden, txtMonto2Bs.Text.Replace(".", ""), DgvAbonos)).Replace(".", ",");
                        txtTranferencia.Focus();

                        if (Convert.ToDouble(TxtRecibidoREF.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtRecibidoREF.Text)) >= Convert.ToDouble(txtRef.Text == "" ? (Double)0.00 : Convert.ToDouble(txtRef.Text)))
                        {
                            TxtVuelto.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", Math.Round((Convert.ToDouble(TxtRecibidoREF.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtRecibidoREF.Text))) - Convert.ToDouble(txtRef.Text == "" ? (Double)0.00 : Convert.ToDouble(txtRef.Text)), 2));
                            TxtMontoPagoMovil.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", Math.Round(Convert.ToDouble(TB_TASA_Dolar.Tasa) * ((Convert.ToDouble(TxtRecibidoREF.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtRecibidoREF.Text))) - Convert.ToDouble(txtRef.Text == "" ? (Double)0.00 : Convert.ToDouble(txtRef.Text))), 2));
                        }

                    }
                    else
                    {

                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("El monto a pagar deber ser menor o igual a su saldo sin IGTF");
                        _FrmMensajes.ShowDialog();
                        txtRef.Text = "";
                        txtRef.Focus();

                    }
                }

                if (CbxMoneda.SelectedIndex.ToString() == "1")
                {
                    if (TB_TASA_Euro.Tasa != 0 && TB_TASA_Euro.Tasa != null)
                    {
                        if (Math.Round((double)((Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) - IgtfAbonado) / TB_TASA_Euro.Tasa), 2) >= (txtRef.Text == "" ? (Double)0.00 : Convert.ToDouble(txtRef.Text)))
                        {
                            _L_Facturacion.ConvertirDolaresBolivares(txtRef, txtMonto2Bs, CbxMoneda.Text);
                            txtIGTF.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", _L_Facturacion.CalculoIgtf(txtNumeroOrden, txtMonto2Bs.Text.Replace(".", ""), DgvAbonos)).Replace(".", ",");
                            txtTranferencia.Focus();


                            if (Convert.ToDouble(TxtRecibidoREF.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtRecibidoREF.Text)) >= Convert.ToDouble(txtRef.Text == "" ? (Double)0.00 : Convert.ToDouble(txtRef.Text)))
                            {
                                TxtVuelto.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", Math.Round((Convert.ToDouble(TxtRecibidoREF.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtRecibidoREF.Text))) - Convert.ToDouble(txtRef.Text == "" ? (Double)0.00 : Convert.ToDouble(txtRef.Text)), 2));
                                TxtMontoPagoMovil.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", Math.Round(Convert.ToDouble(TB_TASA_Euro.Tasa) * ((Convert.ToDouble(TxtRecibidoREF.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtRecibidoREF.Text))) - Convert.ToDouble(txtRef.Text == "" ? (Double)0.00 : Convert.ToDouble(txtRef.Text))), 2));
                            }

                        }

                        else
                        {

                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje("El monto a pagar deber ser menor o igual a su saldo sin IGTF");
                            _FrmMensajes.ShowDialog();
                            txtRef.Text = "";
                            txtRef.Focus();

                        }

                    }

                    else
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("Debe Actualizar la Tasa de las Monedas y Activación de Secuencia Diaria, verifique");
                        _FrmMensajes.ShowDialog();
                    }

                }

            }

        }

        private void FormatoTexbox()
        {
            try
            {
                if (string.IsNullOrEmpty(txtRef.Text) && txtRef.Text == "") txtRef.Text = "0.00";
                if (string.IsNullOrEmpty(txtMonto2Bs.Text) && txtRef.Text == "") txtMonto2Bs.Text = "0,00";
                if (string.IsNullOrEmpty(txtIGTF.Text) && txtRef.Text == "") txtIGTF.Text = "0.00";
                if (string.IsNullOrEmpty(TxtVuelto.Text) && txtRef.Text == "") TxtVuelto.Text = "0.00";


            }
            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();

            }

        }


        private void txtRef_Click(object sender, EventArgs e)
        {
            txtMonto2Bs.Text = "0,00";
            txtIGTF.Text = "0.00";
            TxtVuelto.Text = "0.00";
            txtTranferencia.Text = "";
            txtRef.Text = "";

        }

        private void txtMonto2Bs_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8)
            {
                e.Handled = false;
                return;
            }


            bool IsDec = false;
            int nroDec = 0;

            if (txtMonto2Bs.SelectionLength <= 0)
            {

                for (int i = 0; i < txtMonto2Bs.Text.Length; i++)
                {
                    if (txtMonto2Bs.Text[i] == ',')
                        IsDec = true;

                    if (IsDec && nroDec++ >= 2)
                    {
                        e.Handled = true;
                        return;
                    }


                }
            }

            if (e.KeyChar >= 44 && e.KeyChar <= 57)
                e.Handled = false;
            else if (e.KeyChar == 46)
                e.Handled = (IsDec) ? true : false;
            else
                e.Handled = true;


            //para que solo acepte numeros y una sola coma
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }


            if (txtMonto2Bs.Text.Contains("") && e.KeyChar == 44)
            {
                //separamos por punto
                string[] parts = txtMonto2Bs.Text.Split(',');
                //si el primer elemento está vacío, significa que no se escribió nada antes de, entonces, añadimos el cero al textbox.
                if (parts[0].Length <= 0)
                {
                    txtMonto2Bs.Text = "0" + txtMonto2Bs.Text;
                    //UPDATE: colocamos el cursor al final del texto
                    txtMonto2Bs.SelectionStart = txtMonto2Bs.Text.Length;
                }
            }

        }

        private void txtMonto2Bs_Click(object sender, EventArgs e)
        {
            txtMonto2Bs.Text = "";
        }

        private void TxtVuelto_Click(object sender, EventArgs e)
        {
            TxtVuelto.Text = "";
        }

        private void TxtVuelto_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8)
            {
                e.Handled = false;
                return;
            }


            bool IsDec = false;
            int nroDec = 0;

            for (int i = 0; i < TxtVuelto.Text.Length; i++)
            {
                if (TxtVuelto.Text[i] == ',')

                    IsDec = true;

                if (IsDec && nroDec++ >= 2)
                {
                    e.Handled = true;
                    return;
                }
            }


            if (e.KeyChar >= 44 && e.KeyChar <= 57)
                e.Handled = false;
            else if (e.KeyChar == 46)
                e.Handled = (IsDec) ? true : false;
            else
                e.Handled = true;


            //para que solo acepte numeros y una sola coma
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }


            if (TxtVuelto.Text.Contains("") && e.KeyChar == 44)
            {
                //separamos por punto
                string[] parts = TxtVuelto.Text.Split(',');
                //si el primer elemento está vacío, significa que no se escribió nada antes de, entonces, añadimos el cero al textbox.
                if (parts[0].Length <= 0)
                {
                    TxtVuelto.Text = "0" + TxtVuelto.Text;
                    //UPDATE: colocamos el cursor al final del texto
                    TxtVuelto.SelectionStart = TxtVuelto.Text.Length;
                }
            }


        }

        private void txtTranferencia_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (CbxMetodosPago2.Text == "Transferencia Divisa")
            {


            }

            else
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }

                if (e.KeyChar == 13)
                {
                    DtpFecha.Focus();
                }
            }
        }

        private void txtIGTF_KeyPress(object sender, KeyPressEventArgs e)
        {
            //para que solo acepte numeros y una sola coma
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }

            if (e.KeyChar == 13)
            {
                txtMonto2Bs.Focus();
            }
        }

        private void txtIGTF_Click(object sender, EventArgs e)
        {
            //txtIGTF.Text = "";
        }

        private void txtMontoBs_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8)
            {
                e.Handled = false;
                return;
            }


            bool IsDec = false;
            int nroDec = 0;


            if (txtMontoBs.SelectionLength <= 0)
            {

                for (int i = 0; i < txtMontoBs.Text.Length; i++)
                {
                    if (txtMontoBs.Text[i] == ',')
                        IsDec = true;

                    if (IsDec && nroDec++ >= 2)
                    {
                        e.Handled = true;
                        return;
                    }


                }
            }

            if (e.KeyChar >= 44 && e.KeyChar <= 57)
                e.Handled = false;
            ///46 = .
            ///46 = ,
            else if (e.KeyChar == 46)
                e.Handled = (IsDec) ? true : false;
            else
                e.Handled = true;


            //para que solo acepte numeros y una sola coma
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }


            if (txtMontoBs.Text.Contains("") && e.KeyChar == 44)
            {
                //separamos por punto
                string[] parts = txtMontoBs.Text.Split(',');
                //si el primer elemento está vacío, significa que no se escribió nada antes de, entonces, añadimos el cero al textbox.
                if (parts[0].Length <= 0)
                {
                    txtMontoBs.Text = "0" + txtMontoBs.Text;
                    //UPDATE: colocamos el cursor al final del texto
                    txtMontoBs.SelectionStart = txtMontoBs.Text.Length;
                }
            }
        }


        private void btnCancelar2_Click(object sender, EventArgs e)
        {
            LimpiaVariablesIdAbonoPagoMovil();
            VisualizarPanel("MostrarPanelPrincipal");
            LimpiarTxbox();
        }

        public void LimpiarTxbox()
        {
            this.PnlSecundario.Visible = false;
            this.PnlNotaCredito.Visible = false;
            this.PnlNotaDevolucion.Visible = false;
            txtRef.Text = "0.00";
            txtMonto2Bs.Text = "0,00";
            txtIGTF.Text = "0.00";
            TxtVuelto.Text = "0.00";
            txtTranferencia.Text = "";
            CbxMetodosPago.SelectedIndex = 2;
            txtCVC.Text = "";
            txtCheque.Text = "";
            txtVence.Text = "";
            txtNroNota.Text = "";
            txtNroNotaDevolucion.Text = "";
            txtBsNotaCredito.Text = "0,00";
            txtBsNotaDevolucion.Text = "0,00";

            txtRef.Visible = true;
            label4.Visible = true;

            txtCheque.Enabled = false;
            txtCheque.Visible = false;
            label24.Visible = false;

            label26.Visible = false;
            txtCVC.Enabled = false;
            txtCVC.Visible = false;
            CbxTarjeta.Visible = false;
            CbxTarjeta.Enabled = false;
            label27.Visible = false;
            txtVence.Enabled = false;
            txtVence.Visible = false;
            label49.Visible = false;
            CbxPunto_Venta.Visible = false;
            CbxBanco.Enabled = true;

            //Pago Movil 
            lbCedulaPago.Visible = false;
            CbxNacionalidadPagoMovil.Visible = false;
            TxtCedulaPagoMovil.Visible = false;
            lbCedularPago.Visible = false;
            CbxCelularPagoMovil.Visible = false;
            TxtCedularPagoMovil.Visible = false;
            lbMontoPago.Visible = false;
            TxtMontoPagoMovil.Visible = false;
            lbBancoPago.Visible = false;
            CbxBancoPagoMovil.Visible = false;


            CbxBanco.Visible = true;
            CbxBanco.Location = new Point(8, 124);
            label45.Visible = true;
            label45.Location = new Point(4, 102);
            txtMonto2Bs.Location = new Point(279, 124);
            Bs.Location = new Point(278, 102);
            label42.Location = new Point(183, 102);
            txtIGTF.Location = new Point(187, 124);
            label28.Visible = true;
            label28.Location = new Point(5, 157);
            txtTranferencia.Visible = true;
            txtTranferencia.Location = new Point(8, 178);
            label21.Visible = true;
            label21.Location = new Point(199, 156);
            DtpFecha.Visible = true;
            DtpFecha.Location = new Point(203, 178);
            label6.Location = new Point(324, 155);
            TxtVuelto.Location = new Point(328, 178);
            label22.Visible = true;
            label71.Visible = false;
            TxtRecibidoREF.Visible = false;
            CbxNacionalidadPagoMovil.Enabled = false;
            CbxCelularPagoMovil.Enabled = false;

            TxtMontoPagoMovil.Text = "0.00";
            TxtCedulaPagoMovil.Text = "";
            TxtCedularPagoMovil.Text = "";
            TxtRecibidoREF.Text = "";

            DtpFecha.Text = DateTime.UtcNow.ToShortDateString();

            // ----------------------14/08/2023--------------------------------------
            label41.Location = new Point(4, 43);
            CbxMetodosPago2.Location = new Point(8, 66);
            Bs.Text = "Monto en Bs";
            txtTranferencia.Size = new Size(175, 21);


        }

        private void btnProcesar2_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiaVariablesIdAbonoPagoMovil();
                Double Bolivares = 0.00;
                Double TotalAbono = 0.00;
                Bolivares = (txtMonto2Bs.Text == "" ? (Double)0.00 : Convert.ToDouble(txtMonto2Bs.Text.Replace(".", "")));
                TotalAbono = Convert.ToDouble(_L_Facturacion.TotalizarAbono(DgvAbonos));

                if (CbxMetodosPago2.Text == "Transferencia Divisa" && CbxMetodosPago.Text == "Transferencia Divisa")
                {

                    //'Si los campos poseen valores proceso los datos
                    if (txtRef.Text != "0.00" && txtMonto2Bs.Text != "0,00" && txtIGTF.Text != "0.00" && txtTranferencia.Text != "" && CbxBanco.Text != "" && Convert.ToDouble(txtMonto2Bs.Text.Replace(".", "")) >= 1)
                    {

                        if (Bolivares > (Math.Round((TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos)) + Convert.ToDouble(txtIGTF.Text.Replace(".", ","))) - TotalAbono, 2)))
                        {
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje("El monto debe ser igual o menor al saldo de la orden");
                            _FrmMensajes.ShowDialog();
                            return;

                        }

                        _L_Facturacion.GuardarAbonoGrid(CantAbonosPrevios + Dt_Abonos.Rows.Count + 1, Dt_Abonos, CbxMetodosPago2.Text, CbxMoneda.Text, CbxBanco.Text, txtMonto2Bs.Text, txtTranferencia.Text, DtpFecha.Value.ToString(), CbxMetodosPago2.SelectedValue.ToString(), CbxBanco.SelectedValue.ToString(), "", TxtVuelto.Text, txtRef.Text, txtIGTF.Text);
                        
                        BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);
                        //-----------ConvertirBolivares---------------------------
                        //txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);

                        lbMinAbo.Text = Convert.ToString(Convert.ToDouble(lbMinAbo.Text) + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos)));
                        VisualizarPanel("MostrarPanelPrincipal");
                        LimpiarTxbox();
                        CbxMetodosPago.SelectedIndex = 2;


                    }

                    else
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("Debe llenar todos los campos para continuar");
                        _FrmMensajes.ShowDialog();
                        return;

                    }


                }


                if (CbxMetodosPago2.Text == "Transferencia")
                {
                    //'Si los campos poseen valores proceso los datos
                    if (txtMonto2Bs.Text != "0.00" && txtTranferencia.Text != "" && CbxBanco.Text != "" && Convert.ToDouble(txtMonto2Bs.Text.Replace(".", "")) > 0)
                    {
                        if (Bolivares > (Math.Round((TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos))) - TotalAbono, 2)))
                        {
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje("El monto debe ser igual o menor al saldo de la orden");
                            _FrmMensajes.ShowDialog();
                            return;
                        }

                        _L_Facturacion.GuardarAbonoGrid(2,Dt_Abonos, CbxMetodosPago2.Text, CbxMoneda.Text, CbxBanco.Text, txtMonto2Bs.Text, txtTranferencia.Text, DtpFecha.Value.ToString(), CbxMetodosPago2.SelectedValue.ToString(), CbxBanco.SelectedValue.ToString(), CbxBancoRecp.SelectedValue.ToString(), TxtVuelto.Text);
                        
                        BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);
                        //-----------ConvertirBolivares---------------------------
                        //txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);

                        lbMinAbo.Text = Convert.ToString(Convert.ToDouble(lbMinAbo.Text) + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos)));
                        VisualizarPanel("MostrarPanelPrincipal");
                        LimpiarTxbox();
                        CbxMetodosPago.SelectedIndex = 2;
                    }

                    else
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("Debe llenar todos los campos para continuar");
                        _FrmMensajes.ShowDialog();
                        return;

                    }

                }


                if (CbxMetodosPago2.Text == "Debito")
                {
                    //'Si los campos poseen valores proceso los datos
                    if (txtMonto2Bs.Text != "" && txtMonto2Bs.Text != "0,00" && txtTranferencia.Text != "" && CbxBanco.Text != "" && Convert.ToDouble(txtMonto2Bs.Text.Replace(".", "")) > 0)
                    {
                        if (Bolivares > (Math.Round((TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos))) - TotalAbono, 2)))
                        {
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje("El monto debe ser igual o menor al saldo de la orden");
                            _FrmMensajes.ShowDialog();
                            return;
                        }

                        if (txtTranferencia.Text.Length >= 4)
                        {
                            _L_Facturacion.GuardarAbonoGrid(2,Dt_Abonos, CbxMetodosPago2.Text, CbxMoneda.Text, CbxBanco.Text, txtMonto2Bs.Text, txtTranferencia.Text, DtpFecha.Value.ToString(), CbxMetodosPago2.SelectedValue.ToString(), CbxBanco.SelectedValue.ToString(), "", TxtVuelto.Text, "", "", txtCVC.Text, txtVence.Text, "", "", CbxPunto_Venta.SelectedValue.ToString());
                            BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);
                            //-----------ConvertirBolivares---------------------------
                            //txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);

                            lbMinAbo.Text = Convert.ToString(Convert.ToDouble(lbMinAbo.Text) + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos)));
                            VisualizarPanel("MostrarPanelPrincipal");
                            LimpiarTxbox();
                            CbxMetodosPago.SelectedIndex = 2;

                        }

                        else
                        {
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje("El N° Tarjeta debe ser mayor a 4 digitos, Verifique");
                            _FrmMensajes.ShowDialog();
                            return;
                        }

                    }

                    else
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("Debe llenar todos los campos para continuar");
                        _FrmMensajes.ShowDialog();
                        return;

                    }

                }


                if (CbxMetodosPago2.Text == "Cashea")
                {
                    //'Si los campos poseen valores proceso los datos
                    if (txtMonto2Bs.Text != "" && txtMonto2Bs.Text != "0,00" && txtTranferencia.Text != "" && CbxBanco.Text != "" && (txtTranferencia.Text.Replace(" ", "")).Length > 3 && Convert.ToDouble(txtMonto2Bs.Text.Replace(".", "")) > 0)
                    {
                        if (Bolivares > (Math.Round((TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos))) - TotalAbono, 2)))
                        {
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje("El monto debe ser igual o menor al saldo de la orden");
                            _FrmMensajes.ShowDialog();
                            txtMonto2Bs.Focus();
                            return;

                        }

                        if (txtTranferencia.Text.Length >= 4)
                        {
                            //CbxMetodosPago2.SelectedIndex = 10;
                            _L_Facturacion.GuardarAbonoGrid(2,Dt_Abonos, CbxMetodosPago2.Text, CbxMoneda.Text, "CASHEA", txtMonto2Bs.Text, txtTranferencia.Text, DtpFecha.Value.ToString(), "021", "110", "110", TxtVuelto.Text, "", "", "", "", "", "", "000");
                            
                            BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);
                            //-----------ConvertirBolivares---------------------------
                            //txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);

                            lbMinAbo.Text = Convert.ToString(Convert.ToDouble(lbMinAbo.Text) + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos)));
                            VisualizarPanel("MostrarPanelPrincipal");
                            LimpiarTxbox();
                            CbxMetodosPago.SelectedIndex = 2;

                        }

                        else
                        {
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje("El N° de Referencia debe ser mayor a 4 digitos, Verifique");
                            _FrmMensajes.ShowDialog();
                            return;
                        }

                    }

                    else
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("Debe llenar todos los campos para continuar");
                        _FrmMensajes.ShowDialog();
                        return;

                    }


                    if (Bolivares > (Math.Round((TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos))) - TotalAbono, 2)))
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("El monto debe ser igual o menor al saldo de la orden");
                        _FrmMensajes.ShowDialog();
                        return;
                    }
                }


                if (CbxMetodosPago2.Text == "Pago Móvil")
                {
                    //'Si los campos poseen valores proceso los datos
                    if (txtMonto2Bs.Text != "0,00" && txtTranferencia.Text != "" && CbxBanco.Text != "" && Convert.ToDouble(txtMonto2Bs.Text.Replace(".", "")) > 0)
                    {
                        if (Bolivares > (Math.Round((TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos))) - TotalAbono, 2)))
                        {
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje("El monto debe ser igual o menor al saldo de la orden");
                            _FrmMensajes.ShowDialog();
                            return;
                        }

                        //CbxMetodosPago2.SelectedIndex = 10;
                        _L_Facturacion.GuardarAbonoGrid(2,Dt_Abonos, CbxMetodosPago2.Text, CbxMoneda.Text, CbxBanco.Text, txtMonto2Bs.Text, txtTranferencia.Text, DtpFecha.Value.ToString(), CbxMetodosPago2.SelectedValue.ToString(), CbxBanco.SelectedValue.ToString(), CbxBancoRecp.SelectedValue.ToString(), TxtVuelto.Text);
                        
                        BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);
                        //-----------ConvertirBolivares---------------------------
                        //txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);

                        lbMinAbo.Text = Convert.ToString(Convert.ToDouble(lbMinAbo.Text) + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos)));
                        VisualizarPanel("MostrarPanelPrincipal");
                        LimpiarTxbox();
                        CbxMetodosPago.SelectedIndex = 2;
                    }

                    else
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("Debe llenar todos los campos para continuar");
                        _FrmMensajes.ShowDialog();
                        return;

                    }
                }


                // Nuevo 09- 08- 2023 Efectivo Divisas  Guardar el abono 
                if (CbxMetodosPago2.Text == "Efectivo Divisa" && CbxMetodosPago.Text == "Efectivo Divisa")
                {

                    //'Si los campos poseen valores proceso los datos
                    if (txtRef.Text != "0.00" && txtMonto2Bs.Text != "0,00" && txtIGTF.Text != "0.00" && CbxBanco.Text != "" && Convert.ToDouble(txtMonto2Bs.Text.Replace(".", "")) > 0)
                    {


                        if (Bolivares > (Math.Round((TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos)) + Convert.ToDouble(txtIGTF.Text.Replace(".", ","))) - TotalAbono, 2)))
                        {
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje("El monto debe ser igual o menor al saldo de la orden");
                            _FrmMensajes.ShowDialog();
                            return;
                        }

                        if (TxtRecibidoREF.Text != "" && TxtRecibidoREF.Text != "0.00")
                        {
                            _FrmMensajes.co = 3;
                            _FrmMensajes.avisomensaje("¿Está seguro que estos son los datos para realizar el pago móvil de la orden " + txtNumeroOrden.Text + "?");
                            _FrmMensajes.ShowDialog();

                            //Preguta
                            if (_FrmMensajes.DialogResult == DialogResult.OK)
                            {
                                DataTable Pagos = _L_Facturacion.MostarPagosGrid(TB_CAORDSER.Cod_Sucursal, txtNumeroOrden.Text, TB_CAORDSER.Revision);

                                if (Pagos.Rows.Count > 0)
                                {
                                    CantAbonosPrevios = Pagos.Rows.Count;

                                }
                                idAbonoPagoMovil = CantAbonosPrevios + Dt_Abonos.Rows.Count + 1;

                                double recibidoREF = 0; ;
                                if (TxtRecibidoREF.Text != "" && TxtRecibidoREF.Text != "0.00")
                                {
                                    double reftotal = Convert.ToDouble(TxtRecibidoREF.Text.Replace(".", ""));
                                    double tasa = Convert.ToDouble(txtTasaFact.Text.Replace(".", ""));
                                    recibidoREF = reftotal * tasa;
                                }
                                // Guardo el Abono y retotno a la pantalla principal 
                                _L_Facturacion.GuardarAbonoGrid(idAbonoPagoMovil, Dt_Abonos, CbxMetodosPago2.Text, CbxMoneda.Text, CbxBanco.Text, txtMonto2Bs.Text, "0000", DtpFecha.Value.ToString(), "021", CbxBanco.SelectedValue.ToString(), "", TxtVuelto.Text, txtRef.Text, txtIGTF.Text,"","","","","", Convert.ToString(recibidoREF));

                                //'Valido que recibido ref no este vacio para guardar el pago si no continuo mi proceso normal 
                                if (TxtRecibidoREF.Text != "" && TxtRecibidoREF.Text != "0.00")
                                {

                                    if (Convert.ToDouble(TxtRecibidoREF.Text) <= Convert.ToDouble(txtRef.Text))
                                    {
                                        _FrmMensajes.co = 2;
                                        _FrmMensajes.avisomensaje("El Monto recibido debe ser mayor a: " + txtRef.Text + ".");
                                        _FrmMensajes.ShowDialog();
                                        TxtRecibidoREF.Text = "";
                                        TxtRecibidoREF.Focus();
                                        return;

                                    }

                                    if (TxtCedulaPagoMovil.Text != "" && TxtCedulaPagoMovil.TextLength > 1 && (TxtCedulaPagoMovil.Text.Replace(" ", "")).Length > 1 && TxtMontoPagoMovil.Text != "0.00"
                                    && TxtCedularPagoMovil.Text != "" && TxtCedularPagoMovil.TextLength > 1 && (TxtCedularPagoMovil.Text.Replace(" ", "")).Length > 1 && TxtMontoPagoMovil.Text != "" && CbxCelularPagoMovil.Text != "")
                                    {

                                        //if (CbxMoneda.SelectedIndex.ToString() == "1")
                                        //{
                                        //    if (Convert.ToDouble(TxtMontoPagoMovil.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtMontoPagoMovil.Text.Replace(".", ","))) >= Math.Round(19 * Convert.ToDouble(TB_TASA_Euro.Tasa), 2) |
                                        //        Convert.ToDouble(TxtMontoPagoMovil.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtMontoPagoMovil.Text.Replace(".", ","))) >= Math.Round(Convert.ToDouble(_D_DetalleOrden.LimitePagoMovil(TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv) == "" ? (Double)0.00 : Convert.ToDouble(_D_DetalleOrden.LimitePagoMovil(TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv))) * Convert.ToDouble(TB_TASA_Euro.Tasa), 2)
                                        //        | Convert.ToDouble(TxtMontoPagoMovil.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtMontoPagoMovil.Text.Replace(".", ","))) >= Math.Round(_L_Facturacion.RecorrerPagoMovil_dt(Dt_PagoMovil) * Convert.ToDouble(TB_TASA_Dolar.Tasa), 2))
                                        //    {
                                        //        _FrmMensajes.co = 2;
                                        //        _FrmMensajes.avisomensaje("El limite diario de pago móvil es de 19$ .");
                                        //        _FrmMensajes.ShowDialog();
                                        //        TxtRecibidoREF.Focus();
                                        //        return;

                                        //    }
                                        //}
                                        //else
                                        //{
                                        //    if (Convert.ToDouble(TxtMontoPagoMovil.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtMontoPagoMovil.Text.Replace(".", ","))) >= Math.Round(19 * Convert.ToDouble(TB_TASA_Dolar.Tasa), 2) |
                                        //        Convert.ToDouble(TxtMontoPagoMovil.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtMontoPagoMovil.Text.Replace(".", ","))) >= Math.Round(Convert.ToDouble(_D_DetalleOrden.LimitePagoMovil(TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv) == "" ? (Double)0.00 : Convert.ToDouble(_D_DetalleOrden.LimitePagoMovil(TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv))) * Convert.ToDouble(TB_TASA_Dolar.Tasa), 2)
                                        //        | Convert.ToDouble(TxtMontoPagoMovil.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtMontoPagoMovil.Text.Replace(".", ","))) >= Math.Round(_L_Facturacion.RecorrerPagoMovil_dt(Dt_PagoMovil) * Convert.ToDouble(TB_TASA_Dolar.Tasa), 2))
                                        //    {
                                        //        _FrmMensajes.co = 2;
                                        //        _FrmMensajes.avisomensaje("El limite diario de pago móvil es de 19$ .");
                                        //        _FrmMensajes.ShowDialog();
                                        //        TxtRecibidoREF.Focus();
                                        //        return;

                                        //    }
                                        //}

                                        if ((CbxNacionalidadPagoMovil.Text + "-" + TxtCedulaPagoMovil.Text).Replace(" ", "") != (txtCedula.Text).Replace(" ", "") | (TxtTelefono.Text).Replace(" ", "") != (CbxCelularPagoMovil.Text + "-" + TxtCedularPagoMovil.Text).Replace(" ", ""))
                                        {
                                            _FrmClaveAutorizada.ShowDialog();

                                            if (_FrmClaveAutorizada.DialogResult == DialogResult.OK)
                                            {
                                                if (_FrmClaveAutorizada.ClaveCorrecta == true)
                                                {
                                                    string Autorizaa = VariablesGlobales.UsuarioAutorizado_FrmClaveAutorizada;
                                                    string DescripAuditorAbono = "OS: " + TB_CAORDSER.NumOrdserv + ", Autorizado por: " + Autorizaa;
                                                    _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "086", TB_USUARIO.COD_EMPLEADO, DescripAuditorAbono);
                                                }
                                                else
                                                {
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                return;
                                            }
                                        }

                                        //Guardo el Pago Movil 
                                        _L_Facturacion.GuardarPagoMovilTabla(idAbonoPagoMovil, Dt_PagoMovil, CbxNacionalidadPagoMovil.Text, TxtCedulaPagoMovil.Text, CbxCelularPagoMovil.Text, TxtCedularPagoMovil.Text, TxtMontoPagoMovil.Text, CbxBancoPagoMovil.SelectedValue.ToString(), TxtRecibidoREF.Text, TxtVuelto.Text, CbxMoneda.Text);

                                    }

                                    else
                                    {
                                        _FrmMensajes.co = 2;
                                        _FrmMensajes.avisomensaje("Debe llenar todos los campos para continuar");
                                        _FrmMensajes.ShowDialog();
                                        return;

                                    }
                                }


                                BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);
                                //-----------ConvertirBolivares---------------------------
                                //txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);

                                lbMinAbo.Text = Convert.ToString(Convert.ToDouble(lbMinAbo.Text) + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos)));
                                VisualizarPanel("MostrarPanelPrincipal");
                                LimpiarTxbox();
                                CbxMetodosPago.SelectedIndex = 2;
                            }
                            else
                            {
                                return;
                            }
                        } 
                        else
                        {
                            //Guarda abono
                            DataTable Pagos = _L_Facturacion.MostarPagosGrid(TB_CAORDSER.Cod_Sucursal, txtNumeroOrden.Text, TB_CAORDSER.Revision);

                            if (Pagos.Rows.Count > 0)
                            {
                                CantAbonosPrevios = Pagos.Rows.Count;

                            }
                            idAbonoPagoMovil = CantAbonosPrevios + Dt_Abonos.Rows.Count + 1;
                            // Guardo el Abono y retotno a la pantalla principal 
                            _L_Facturacion.GuardarAbonoGrid(idAbonoPagoMovil, Dt_Abonos, CbxMetodosPago2.Text, CbxMoneda.Text, CbxBanco.Text, txtMonto2Bs.Text, "0000", DtpFecha.Value.ToString(), "021", CbxBanco.SelectedValue.ToString(), "", TxtVuelto.Text, txtRef.Text, txtIGTF.Text);

                            BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);
                            //-----------ConvertirBolivares---------------------------
                            //txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);

                            lbMinAbo.Text = Convert.ToString(Convert.ToDouble(lbMinAbo.Text) + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos)));
                            VisualizarPanel("MostrarPanelPrincipal");
                            LimpiarTxbox();
                            CbxMetodosPago.SelectedIndex = 2;
                        }

                    }
                    else
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("Debe llenar todos los campos para continuar");
                        _FrmMensajes.ShowDialog();
                        return;

                    }

                }


                if (CbxMetodosPago2.Text == "Tarjeta de Credito")
                {
                    //'Si los campos poseen valores proceso los datos
                    if (txtMonto2Bs.Text != "0,00" && txtTranferencia.Text != "" && CbxBanco.Text != "" && txtCVC.Text.Length == 3 && txtCVC.Text != "" && txtVence.Text != "" && Bolivares <= (Math.Round((TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos))) - TotalAbono, 2)))
                    {

                        //Obtengo el año actual 
                        string yearactual = DateTime.Now.Year.ToString();
                        //Obtengo los dos ultimos digitos del año para validar 
                        yearactual = yearactual.Substring(2, 2);

                        // Obtengo los dos primeros digitos del campo vence para validar el mes 
                        string mesvence = txtVence.Text.Substring(0, 2);
                        // Obtengo los doa ultimos digitos del campo vence para validar el año 
                        string anvence = txtVence.Text.Substring(2, 2);

                        if (Convert.ToInt32(anvence) < Convert.ToInt32(yearactual))
                        { // valido que el año no sea menor al año actual 
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje("El año esta errado, no puede ser menor al año actual");
                            _FrmMensajes.ShowDialog();
                            return;
                        }

                        if (Convert.ToInt32(mesvence) > 12)
                        {
                            // valido que el numero del mes no sea mayor a 12

                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje("El valor del mes esta errado, no puede ser mayor a 12");
                            _FrmMensajes.ShowDialog();
                            return;

                        }

                        if (txtTranferencia.Text.Length >= 12) // Valida que el Numero de TC Sea minimo de 12 digitos
                        {
                            // Si es de 12 digitos se valida el comienzo de la tarjeta dependiendo del tipo de tarjeta
                            if (_L_Facturacion.ValidoNumeroTarjeta_Credito(txtTranferencia.Text, CbxTarjeta.Text) == true)
                            {
                                _L_Facturacion.GuardarAbonoGrid(2,Dt_Abonos, CbxTarjeta.Text, CbxMoneda.Text, CbxBanco.Text, txtMonto2Bs.Text, txtTranferencia.Text, DtpFecha.Value.ToString(), CbxMetodosPago2.SelectedValue.ToString(), CbxBanco.SelectedValue.ToString(), "", TxtVuelto.Text, "", "", txtCVC.Text, txtVence.Text, "", "", CbxPunto_Venta.SelectedValue.ToString());
                                
                                BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);
                                //-----------ConvertirBolivares---------------------------
                                //txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);

                                lbMinAbo.Text = Convert.ToString(Convert.ToDouble(lbMinAbo.Text) + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos)));
                                VisualizarPanel("MostrarPanelPrincipal");
                                LimpiarTxbox();
                                CbxMetodosPago.SelectedIndex = 2;
                            }


                            // Si es de 12 digitos pero los primeros numeros son invalidos segun el tipo de tarjeta> Se pide clave de gerente
                            else
                            {
                                _FrmMensajes.co = 3;
                                _FrmMensajes.avisomensaje("Número de tarjeta no válido, debe introducir clave de Gerente para continuar");
                                _FrmMensajes.ShowDialog();


                                if (_FrmMensajes.DialogResult == DialogResult.OK) // si presiona que si le muestro la clave de gerente 
                                {
                                    _FrmClaveGerente.ShowDialog();
                                    if (_FrmClaveGerente.ClaveCorrecta == true)
                                    {
                                        //if (_L_Facturacion.ValidoNumeroTarjeta_Credito(txtTranferencia.Text, CbxTarjeta.Text) == true)
                                        //{
                                        _L_Facturacion.GuardarAbonoGrid(2,Dt_Abonos, CbxTarjeta.Text, CbxMoneda.Text, CbxBanco.Text, txtMonto2Bs.Text, txtTranferencia.Text, DtpFecha.Value.ToString(), CbxMetodosPago2.SelectedValue.ToString(), CbxBanco.SelectedValue.ToString(), "", TxtVuelto.Text, "", "", txtCVC.Text, txtVence.Text, "", "", CbxPunto_Venta.SelectedValue.ToString());
                                        
                                        BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);
                                        //-----------ConvertirBolivares---------------------------
                                        //txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);

                                        lbMinAbo.Text = Convert.ToString(Convert.ToDouble(lbMinAbo.Text) + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos)));
                                        VisualizarPanel("MostrarPanelPrincipal");
                                        LimpiarTxbox();
                                        CbxMetodosPago.SelectedIndex = 2;
                                        //}

                                        // else
                                        //{
                                        //  _FrmMensajes.co = 2;
                                        //_FrmMensajes.avisomensaje("Número de tarjeta no válida");
                                        //_FrmMensajes.ShowDialog();
                                        //}
                                    }

                                    else
                                    {
                                        return; // si la  clave es invalida termina el proceso 

                                    }

                                }
                                else
                                {

                                    return; // Si presiona no salgo de la ventana de pagos 
                                }

                            }
                        }
                        else //Si el numero de TC es menor de 12 digitos 
                        {
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje("Número de tarjeta no válido");
                            _FrmMensajes.ShowDialog();
                        }
                    }
                    else // Si alguno de los campos validados se encuentran vacios.
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("Debe llenar todos los campos para continuar");
                        _FrmMensajes.ShowDialog();
                        return;

                    }


                    if (Bolivares > (Math.Round((TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos))) - TotalAbono, 2)))
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("El monto debe ser igual o menor al saldo de la orden");
                        _FrmMensajes.ShowDialog();
                        return;
                    }
                }



            }

            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }
        }

        private void btnProcesar1_Click(object sender, EventArgs e) 

        {
            this.Enabled = false;
            LimpiaVariablesIdAbonoPagoMovil();
            Cursor.Current = new Cursor(Properties.Resources.relojArena__1_.Handle);
            //Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            string rept = "";

            bool completo;
            string concat = TB_CAORDSER.Cod_Sucursal + TB_CAORDSER.NumOrdserv + TB_CAORDSER.Revision;

            try
            {

                double TotalAbono;
                double MinAbono;
                double TotalOrden;
                double MenorMinimoAbono;
                TotalAbono = Convert.ToDouble(_L_Facturacion.TotalizarAbono(DgvAbonos));
                //MinAbono = Convert.ToDouble(lbMinAbo.Text) + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos));
                MinAbono = Math.Round(0.80 * Convert.ToDouble((String.Format(CultureInfo.InvariantCulture, "{0:0.00}", TB_CAORDSER.VtaTotal).Replace(".", ",")).Replace(".", ",")), 2);
                MenorMinimoAbono = Math.Round(0.50 * Convert.ToDouble((String.Format(CultureInfo.InvariantCulture, "{0:0.00}", TB_CAORDSER.VtaTotal).Replace(".", ",")).Replace(".", ",")), 2);
                TotalOrden = Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos));
                bool Negaivo = _L_Facturacion.VerificarAbonoNegativo(DgvAbonos);
                completo = _L_Facturacion.ValidacionNumFact(TxtNumFact, TxtNroCorrelativo); // SE valida que sea factura manual y que el campo numero de factura este debidamente lleno

                // Validar que el total de los abonos no sea Negativo
                if (Negaivo == false)
                {
                    //Validar que existan datos en el entidad Tb Tasa
                    if (TB_TASA_Dolar.Tasa == 0.00 | TB_TASA_Dolar.Tasa == null | TB_TASA_Euro.Tasa == null | TB_TASA_Euro.Tasa == 0.00)
                    {
                        mensaje = "Debe actualizar la tasa de las divisas y secuencia diaria";
                        rept = "Error";
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje(mensaje);
                        _FrmMensajes.ShowDialog();
                        btnCancelar1.PerformClick();
                        return;
                    }

                    //Validar que el cliente pagador no sea un Niño
                    if (txtCedula.Text.Substring(0, (txtCedula.Text.Length) - (txtCedula.Text.Length - 1)) == "N" | txtCedula.Text.Substring(0, (txtCedula.Text.Length) - (txtCedula.Text.Length - 1)) == "n")
                    {
                        mensaje = "No está permitido emitir una factura a nombre de un menor de edad, modifique el cliente pagador";
                        rept = "Error";
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje(mensaje);
                        _FrmMensajes.ShowDialog();
                        btnCancelar1.PerformClick();
                        return;
                    }

                    //Validar que Exista existencia de inventario para empezar el proceso Solo para ordener PorPagar 
                    if(TB_CAORDSER.OrSer_Status == "004" && TB_CAORDSER.Cod_DetVta!= "02")
                    {
                        string Rep= Verificar_Existencia(TB_CAORDSER.NumOrdserv, TB_CAORDSER.Cod_DetVta, TB_CAORDSER.OrSer_Status);
                        if(Rep !="SATISFACTORIO")
                        {
                            return;
                        }
                    }


                    // Valido si el el saldo es 0 para relizar la validacion de la impresora 
                    if (TotalAbono == Math.Round(TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos)), 2))
                    {
                        if (_L_Facturacion.ValidaFactManual() == false)
                        {
                            if (_Impresora_Fiscal.VerficarConexionImpresoraFiscal() == false)
                            {
                                mensaje = _Impresora_Fiscal.stringBuilder.ToString();
                                rept = "Error";
                                _FrmMensajes.co = 2;
                                _FrmMensajes.avisomensaje(mensaje);
                                _FrmMensajes.ShowDialog();
                                btnCancelar1.PerformClick();
                                return;
                            }
                        }

                    }


                    // limpiamos TEMP_ABONO
                    _D_DetalleOrden.Limpiar_TEMP_ABONO(TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision);

                    
                    // Agregamos los abonos que se encuentran en Tb_ABONO a la tabla Temporal
                    _D_DetalleOrden.InsertarDatos_TEMP_ABONO(TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision);

                    if (TB_CAORDSER.Cod_Venta == "001") // Venta directa
                    {
                        if (TotalAbono == Math.Round((TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos))), 2))
                        {
                            //Se valida que sea factura manual  y que el campo este vacio y que vaya a facturar para que pueda dar error 
                            if (completo == false && TotalAbono == Math.Round((TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos))), 2))
                            {
                                mensaje = "El número de factura debe tener 7 dígitos y el número de control 10 dígitos";
                                rept = "Error";
                                _FrmMensajes.co = 2;
                                _FrmMensajes.avisomensaje(mensaje);
                                _FrmMensajes.ShowDialog();
                                return;

                            }


                            //Registro los pagos en una tabla temporal 
                            rept = _L_Facturacion.LLenadoParametros(DgvAbonos);

                            // funcion para ejecutar todo el proceso de Facturacion en una sola transaccion 
                            if (rept == "SATISFACTORIO")
                            {
                                // validar el numero de factura, solo para impprimir factura manual 
                                completo = _L_Facturacion.ValidacionNumFact(TxtNumFact, TxtNroCorrelativo);

                                // insertar abonos, actualizar caorser, ejecutar movimiento, insertar los billetes TB_BILLETE , emitir factura, Imprimir reporte, Enviar Dana, Actualizar fecha ofrecida
                                string Estado = ProcesarPagos(TotalAbono, Math.Round((TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos))), 2));
                                return;
                            }


                        }
                        else
                        {
                            //mensaje = "El abono debe ser igual al total de la Orden" + lbMinAbo.Text + ",verifique";
                            mensaje = "No se permite abonar en venta directa";
                            rept = "Error";
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje(mensaje);
                            _FrmMensajes.ShowDialog();
                        }

                    }

                    if (TB_CAORDSER.Cod_Venta == "002" | TB_CAORDSER.Cod_Venta == "003" | TB_CAORDSER.Cod_Venta == "004") // trabajo convencional, lente contacto y reparacion
                    {
                        if (TB_CAORDSER.OrSer_Status == "004")
                        {
                            //Cashea  nuevo 07-06-2023
                            if (ValidarPagosIva(DgvAbonos) == true)
                            {
                                //Valido que se cancele el total de la orden
                                if (TotalAbono != Math.Round((TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos))), 2))
                                {
                                    mensaje = "No se permite abonar cuando se utiliza: " + Medio_Pago + " ,como medio de pago";
                                    rept = "Error";
                                    _FrmMensajes.co = 2;
                                    _FrmMensajes.avisomensaje(mensaje);
                                    _FrmMensajes.ShowDialog();
                                    return;
                                }
                            }

                            if (ValidarPagosCASHEA(DgvAbonos) == true)
                            {
                                //Valido que se cancele el total de la orden
                                if (TotalAbono != Math.Round((TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos))), 2))
                                {
                                    mensaje = "No se permite abonar con Cashea";
                                    rept = "Error";
                                    _FrmMensajes.co = 2;
                                    _FrmMensajes.avisomensaje(mensaje);
                                    _FrmMensajes.ShowDialog();
                                    return;
                                }
                            }

                            if (TotalAbono >= Convert.ToDouble(MinAbono) && TotalAbono <= Math.Round(TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos)), 2))
                            {
                                //Se valida que sea factura manual  y que el campo este vacio y que vaya a facturar para que pueda dar error 
                                if (completo == false && TotalAbono == Math.Round((TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos))), 2))
                                {
                                    mensaje = "El número de factura debe tener 7 dígitos y el número de control 10 dígitos";
                                    rept = "Error";
                                    _FrmMensajes.co = 2;
                                    _FrmMensajes.avisomensaje(mensaje);
                                    _FrmMensajes.ShowDialog();
                                    return;

                                }

                                //Registro los pagos en una tabla temporal 
                                rept = _L_Facturacion.LLenadoParametros(DgvAbonos);

                                // funcion para ejecutar todo el proceso de Facturacion en una sola transaccion 
                                if (rept == "SATISFACTORIO")
                                {
                                    // validar el numero de factura, solo para impprimir factura manual 
                                    completo = _L_Facturacion.ValidacionNumFact(TxtNumFact, TxtNroCorrelativo);

                                    // insertar abonos, actualizar caorser, ejecutar movimiento, insertar los billetes TB_BILLETE , emitir factura, Imprimir reporte, Enviar Dana, Actualizar fecha ofrecida
                                    string Estado = ProcesarPagos(TotalAbono, Math.Round((TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos))), 2));
                                    return;
                                }

                            }
                            else
                            {
                                //mensaje = "El abono debe ser igual o mayor al monto minimo " + lbMinAbo.Text + ", No esta autorizado a hacer este proceso, ¿Desea introducir una clave Autorizada para continuar ? , Seleccione una opcion";
                                mensaje = "El abono debe ser igual o mayor al 80% del total de la orden, de lo contrario debe solicitar clave autorizada. ¿Desea continuar?";
                                rept = "";
                                _FrmMensajes.co = 3;
                                _FrmMensajes.avisomensaje(mensaje);
                                _FrmMensajes.ShowDialog();

                                if (_FrmMensajes.DialogResult == DialogResult.OK)
                                {
                                    string Autoriza = "";
                                    string Estado = "";

                                   //if (TotalAbono < Convert.ToDouble(MenorMinimoAbono))
                                    if (TotalAbono >= Convert.ToDouble(MenorMinimoAbono))
                                    {
                                        _FrmClaveGerente.ShowDialog();
                                        if (_FrmClaveGerente.ClaveCorrecta == true)

                                        {
                                            Autoriza = VariablesGlobales.UsuarioAutorizado_FrmClaveGerente;
                                            //Autoriza = _FrmClaveAutorizada.RetornoNombreUsuario();

                                            //Registro los pagos en una tabla temporal 
                                            rept = _L_Facturacion.LLenadoParametros(DgvAbonos);


                                        }

                                    }

                                    //else if (TotalAbono >= Convert.ToDouble(MenorMinimoAbono))
                                    else if (TotalAbono < Convert.ToDouble(MenorMinimoAbono))
                                    {
                                        _FrmClaveAutorizada.ShowDialog();

                                        if (_FrmClaveAutorizada.ClaveCorrecta == true)
                                        {
                                            //Autoriza = _FrmClaveGerente.RetornoNombreUsuario();
                                            Autoriza = VariablesGlobales.UsuarioAutorizado_FrmClaveAutorizada;

                                            //Registro los pagos en una tabla temporal 
                                            rept = _L_Facturacion.LLenadoParametros(DgvAbonos);


                                        }

                                    }

                                    // funcion para ejecutar todo el proceso de Facturacion en una sola transaccion 
                                        if (rept == "SATISFACTORIO" & (_FrmClaveAutorizada.ClaveCorrecta == true | _FrmClaveGerente.ClaveCorrecta == true))
                                    {
                                        // validar el numero de factura, solo para impprimir factura manual 
                                        completo = _L_Facturacion.ValidacionNumFact(TxtNumFact, TxtNroCorrelativo);

                                        // insertar abonos, actualizar caorser, ejecutar movimiento, insertar los billetes TB_BILLETE , emitir factura, Imprimir reporte, Enviar Dana, Actualizar fecha ofrecida
                                        Estado = ProcesarPagos(TotalAbono, Math.Round((TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos))), 2));

                                        if (Estado == "SATISFACTORIO")
                                        {
                                            string DescripAuditorAbono = "OS: " + TB_CAORDSER.NumOrdserv + ", Monto abono: " + Convert.ToString(TotalAbono) + ", Autoriza: " + Autoriza;
                                            _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "060", TB_USUARIO.COD_EMPLEADO, DescripAuditorAbono);
                                            return;
                                        }

                                    }
                                }

                            }

                            return;
                        }

                        if (TB_CAORDSER.OrSer_Status == "005")
                        {
                            if (ValidarPagosIva(DgvAbonos) == true)
                            {
                                //Valido que se cancele el total de la orden
                                if (TotalAbono != Math.Round((TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos))), 2))
                                {
                                    mensaje = "No se permite abonar cuando se utiliza: " + Medio_Pago + " ,como medio de pago";
                                    rept = "Error";
                                    _FrmMensajes.co = 2;
                                    _FrmMensajes.avisomensaje(mensaje);
                                    _FrmMensajes.ShowDialog();
                                    return;
                                }
                            }

                            if (ValidarPagosCASHEA(DgvAbonos) == true)
                            {
                                //Valido que se cancele el total de la orden
                                if (TotalAbono != Math.Round((TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos))), 2))
                                {
                                    mensaje = "No se permite abonar con Cashea";
                                    rept = "Error";
                                    _FrmMensajes.co = 2;
                                    _FrmMensajes.avisomensaje(mensaje);
                                    _FrmMensajes.ShowDialog();
                                    return;
                                }
                            }
                            if (TotalAbono <= Math.Round((TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos))), 2))
                            {
                                //Se valida que sea factura manual  y que el campo este vacio y que vaya a facturar para que pueda dar error
                                if (completo == false && TotalAbono == Math.Round((TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos))), 2))
                                {
                                    mensaje = "El número de factura debe tener 7 dígitos y el número de control 10 dígitos";
                                    rept = "Error";
                                    _FrmMensajes.co = 2;
                                    _FrmMensajes.avisomensaje(mensaje);
                                    _FrmMensajes.ShowDialog();
                                    return;

                                }

                                //Registro los pagos en una tabla temporal 
                                rept = _L_Facturacion.LLenadoParametros(DgvAbonos);

                                // funcion para ejecutar todo el proceso en una sola transaccion 
                                if (rept == "SATISFACTORIO")
                                {
                                    // validar el numero de factura, solo para impprimir factura manual 
                                    completo = _L_Facturacion.ValidacionNumFact(TxtNumFact, TxtNroCorrelativo);

                                    // insertar abonos, actualizar caorser, ejecutar movimiento, insertar los billetes TB_BILLETE , emitir factura
                                    rept = ProcesarPagos(TotalAbono, Math.Round((TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos))), 2));
                                    return;
                                }

                            }

                            else
                            {
                                mensaje = "El abono debe ser menor al saldo total de la orden " + TB_CAORDSER.OrSer_Saldo.ToString() + ", verifique";
                                rept = "Error";
                                _FrmMensajes.co = 2;
                                _FrmMensajes.avisomensaje(mensaje);
                                _FrmMensajes.ShowDialog();
                            }

                        }

                    }

                }


            }

            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }

            finally
            {
                try
                {
                    

                    LimpiarGrid();

                DataTable Pagos = _L_Facturacion.MostarPagosGrid(TB_CAORDSER.Cod_Sucursal, txtNumeroOrden.Text, TB_CAORDSER.Revision);

                if (Pagos.Rows.Count > 0)
                {
                    CantAbonosPrevios = Pagos.Rows.Count;
                    DgvListadoOrdenes.DataSource = Pagos;
                    CrearObjetos();
                }

                CargarDatosOrden(txtNumeroOrden.Text, txtNombreCliente.Text, TB_CAORDSER.Revision);

                    this.Enabled = true;
                if (rept == "SATISFACTORIO" || rept == "" || rept == "Error")
                {
                    btnCancelar1.PerformClick();
                    HabilitacionControl("Habilitar");
                }
                else
                {
                    HabilitacionControl("Bloquear");
                }

                if (TxtStatus.Text == "Facturada" || TxtStatus.Text == "Anulada")
                {
                    btnIngresar.Enabled = false;
                    BtnClientePagador.Visible = false;
                }

                else
                {
                    btnIngresar.Enabled = true;
                    BtnClientePagador.Visible = true;
                }

                Cursor = System.Windows.Forms.Cursors.Default;

                }

                catch (Exception ex)
                {
                    // Manejo de excepciones dentro del bloque finally
                    Console.WriteLine("Ocurrió un error en el boton btnProcesar1, bloque finally: " + ex.Message);
                    // Puedes registrar el error o mostrar un mensaje al usuario
                }
            }

           
        }

        public void CargarDatosOrden(string NumeroOrden, String NombreCliente, string Revison)
        {
            try
            {
                // Actualizar la entidad TB_tasa 
                DateTime FechaActiva = _D_Inicio.DiaActivo();
                _D_Inicio.TasaDiaEuroEntidad(FechaActiva.ToString("yyyyMMdd"));
                _D_Inicio.TasaDiaDolarEntidad(FechaActiva.ToString("yyyyMMdd"));

                btnPrincipal.PerformClick();
                btnExamen.Enabled = false;
                btnDetalleOrden.Enabled = false;
                _L_Facturacion.DatosOrden(NumeroOrden, Revison);

                ////agregado 19-05-2023 Para que se Actualize el Igtf de la Orden que viene de Epos
                if (TB_CAORDSER.OrSer_Status == "005")
                {
                    DataTable Abonos = _D_DetalleOrden.RevisarAbonosIGTF_Epos(TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision);
                    bool Actualizar = false;
                    if (Abonos != null)
                    {
                        foreach (DataRow drIgtf in Abonos.Rows)
                        {
                            if (drIgtf["Abo_IGTF"].ToString() == null | drIgtf["Abo_IGTF"].ToString() == "0,000000" | drIgtf["Abo_IGTF"].ToString() == "" | drIgtf["Abo_IGTF"].ToString() == "0.000000")
                            {
                                Actualizar = true;
                            }
                        }


                    }

                    if (Actualizar == true)
                    {
                        _D_DetalleOrden.ActualizarIgtf(TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision, TB_USUARIO.COD_USR);
                        //Actualizo la entidad TB_CAORDSER
                        _L_Facturacion.DatosOrden(NumeroOrden, Revison);
                    }
                }
                //Fin de lo agregado


                txtNumeroOrden.Text = TB_CAORDSER.NumOrdserv;
                //txtNombreVendedor.Text = TB_USUARIO.USER_NOMBRE + " " + TB_USUARIO.USER_APELLIDO; Muestra El usuario logeado y tiene que aparecer quien creo la orden
                txtNombreVendedor.Text = _D_DetalleOrden.BuscarUsuarioOrden(TB_CAORDSER.USER_Crea);
                txtCedula.Text = TB_CAORDSER.CTE_Nacio + "-" + TB_CAORDSER.CTE_CedIden;
                TxtFecha.Text = Convert.ToString(TB_CAORDSER.Fecha).Substring(0, 11);
                TxtStatus.Text = _L_Facturacion.StatusOrde(TB_CAORDSER.OrSer_Status);
                TxtMontoBs2.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", TB_CAORDSER.VtaTotal).Replace(".", ",");
                TxtSaldoOrd.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", TB_CAORDSER.OrSer_Saldo).Replace(".", ",");
                TxtMontoRef.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", TB_CAORDSER.Orser_Total_Mon).Replace(".", ",");


                DataTable dtCliente = _D_DetalleOrden.BucarTB_CTEPPAL(TB_CAORDSER.CTE_CedIden, TB_CAORDSER.CTE_Nacio);

                if (dtCliente.Rows.Count > 0)
                {
                    foreach (DataRow drItem in dtCliente.Rows)
                    {
                        string PrimerNombre = drItem["CTE_PNombre"].ToString();
                        string PrimerApellido = drItem["CTE_PApellido"].ToString();
                        txtNombreCliente.Text = PrimerNombre + " " + PrimerApellido;
                        break;
                    }

                }
                else
                {
                    txtNombreCliente.Text = NombreCliente;
                }
                    

                _L_Facturacion.BuscarTlfCorreo(NumeroOrden);
                TxtTelefono.Text = _L_Facturacion.TlfCliente;
                TxtCorreo.TextAlign = HorizontalAlignment.Center;

                TxtCorreo.Text = _L_Facturacion.CorreoCliente;

                if (TB_CAORDSER.OrSer_Saldo != 0)
                {
                    DataTable dtPago = _D_DetalleOrden.TEMP_ABONO(TB_CAORDSER.Cod_Sucursal, txtNumeroOrden.Text, TB_CAORDSER.Revision);
                    Double totalpagos = 0;
                    foreach (DataRow drPago in dtPago.Rows)
                    {
                        totalpagos = totalpagos + (Convert.ToDouble(drPago["Abo_Monto"].ToString()));
                    }

                    TxtAbono.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", totalpagos).Replace(".", ",");
                }

                else
                {
                    TxtAbono.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", TB_CAORDSER.OrSer_Saldo).Replace(".", ",");
                }

                TxtSaldoRef.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", TB_CAORDSER.OrSer_Saldo_Mon).Replace(".", ",");

                if (TB_CAORDSER.OrSer_Saldo_Mon != 0)
                {
                    TxtAbonoRef.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", (TB_CAORDSER.Orser_Total_Mon - TB_CAORDSER.OrSer_Saldo_Mon)).Replace(".", ",");
                }

                else
                {
                    TxtAbonoRef.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", (TB_CAORDSER.OrSer_Saldo_Mon)).Replace(".", ",");
                }

                TxtIgtfOrd.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", (Math.Round(Convert.ToDecimal(TB_CAORDSER.VtaImpuestoIGTF), 2))).Replace(".", ",");
              

                if (TxtStatus.Text == "Facturada" || TxtStatus.Text == "Anulada")
                {
                    btnIngresar.Enabled = false;
                    BtnClientePagador.Visible = false;
                }

                else
                {
                    btnIngresar.Enabled = true;
                    BtnClientePagador.Visible = true;
                }

                ColorearStatus();

                //Cargar el Examen y el Detalle de la orden ( LLenar el TabPage )

                LLenar_Datos_Convencional();
                LLenar_Datos_Orden();

                // si la orden es Contacto no mostrar las medidas de la montura 
                if (TxtTrabajo.Text.Replace(" ", "") == "CONTACTO")
                {
                    label67.Visible = false;
                    DgvMontura.Visible = false;
                    label69.Location = new Point(-4, 212);
                    DgvObservaMon.Location = new Point(-4, 243);
                }
                else
                {
                    label67.Visible = true;
                    DgvMontura.Visible = true;
                    label67.Location = new Point(-4, 212);
                    DgvMontura.Location = new Point(-4, 243);
                    label69.Location = new Point(-4, 294);
                    DgvObservaMon.Location = new Point(-4, 325);

                }

                //El numero de Factura a las ordenes Facturadas o Anuladas 
                if (TB_CAORDSER.OrSer_Status == "002" | TB_CAORDSER.OrSer_Status == "003")
                {
                    _D_DetalleOrden.ObtenerFactura(TB_CAORDSER.NumOrdserv);
                    label3.Location = new Point(20, 12);
                    txtNumeroOrden.Location = new Point(24, 37);
                    label72.Visible = true;
                    txtNumeroFactura.Visible = true;
                    txtNumeroFactura.Text = TB_FACTURAS.Fact_Num;
                    label72.Location = new Point(126, 12);
                    txtNumeroFactura.Location = new Point(130, 37);


                }
                else
                {
                    label3.Location = new Point(45, 12);
                    txtNumeroOrden.Location = new Point(48, 37);
                    label72.Visible = false;
                    txtNumeroFactura.Visible = false;
                }


                if (TxtStatus.Text == "Facturada" || TxtStatus.Text == "Anulada")
                {
                    toolTip1.ShowAlways = false;
                    toolTip1.Active = false;

                      if ( (TB_TASA_Dolar.Tasa is null) | (TB_TASA_Euro.Tasa is null) )
                      {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("Debe registrar la tasa del día");
                        _FrmMensajes.ShowDialog();
                        return;
                      }
                
                }
                else
                {
                    if ((TB_TASA_Dolar.Tasa is null) | (TB_TASA_Euro.Tasa is null))
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("Debe registrar la tasa del día");
                        _FrmMensajes.ShowDialog();
                        btnIngresar.Enabled = false;
                        return;
                    }


                    double IgtfAbonado = Convert.ToDouble(TxtIgtfOrd.Text.Replace(".", "")) + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos).Replace(".", ""));

                    string saldo_bolivares = Convert.ToString(Math.Round((double)((Convert.ToDouble(TxtSaldoOrd.Text.Replace(".", "")) )), 2));
                    string saldo_ref = Convert.ToString(Math.Round((double)((Convert.ToDouble(TxtSaldoOrd.Text.Replace(".", ""))) / TB_TASA_Dolar.Tasa), 2));
                    saldo_ref = string.Format("{0:#,0.00}", Convert.ToDecimal(saldo_ref));
                    saldo_bolivares = string.Format("{0:#,0.00}", Convert.ToDecimal(saldo_bolivares));
                    // Crear un nuevo componente ToolTip
                    toolTip1.ShowAlways = true;
                    toolTip1.Active = true;
                    //toolTip1.SetToolTip(this.label12, "Saldo BS sin IGTF: " + saldo_bolivares);
                    //toolTip1.SetToolTip(this.label16, "Saldo $ sin IGTF: " + saldo_ref);
                    toolTip1.SetToolTip(this.label12, "Saldo BS con IGTF: " + saldo_bolivares);
                    toolTip1.SetToolTip(this.label16, "Saldo $ con IGTF: " + saldo_ref);
                }

                // toolTip2 para Ordenes Facturadas con clientes distinto 
                if (TxtStatus.Text == "Facturada")
                {
                    _D_DetalleOrden.ObtenerFactura(TB_CAORDSER.NumOrdserv);

                    if (txtCedula.Text != TB_FACTURAS.CTE_NacioPAG + "-" + TB_FACTURAS.CTE_CedIdenPAG)
                    {
                        toolTip2.ShowAlways = true;
                        toolTip2.Active = true;
                        toolTip2.SetToolTip(this.label14, "Cédula del Cliente Pagador: : " + TB_FACTURAS.CTE_NacioPAG + " " + TB_FACTURAS.CTE_CedIdenPAG);
                        DataTable dtCliente2 = _D_DetalleOrden.BucarTB_CTEPPAL(TB_FACTURAS.CTE_CedIdenPAG, TB_FACTURAS.CTE_NacioPAG);

                        if (dtCliente.Rows.Count > 0)
                        {
                            foreach (DataRow drItem in dtCliente2.Rows)
                            {
                                string PrimerNombre = drItem["CTE_PNombre"].ToString();
                                string PrimerApellido = drItem["CTE_PApellido"].ToString();
                                toolTip2.SetToolTip(this.label7, "Nombre del Cliente Pagador: : " + PrimerNombre + " " + PrimerApellido);
                                break;
                            }

                        }
                    }

                    else
                    {
                        toolTip2.ShowAlways = false;
                        toolTip2.Active = false;
                        
                    }

                }
                else
                {
                    toolTip2.ShowAlways = false;
                    toolTip2.Active = false;
                }

            }

            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }


            // nuevo 18-08-2023 Poner las unidades de mil 
            TxtMontoBs2.Text = string.Format("{0:#,0.00}", Convert.ToDecimal(TxtMontoBs2.Text));
            TxtSaldoOrd.Text = string.Format("{0:#,0.00}", Convert.ToDecimal(TxtSaldoOrd.Text));
            TxtAbono.Text = string.Format("{0:#,0.00}", Convert.ToDecimal(TxtAbono.Text));
            TxtMontoRef.Text = string.Format("{0:#,0.00}", Convert.ToDecimal(TxtMontoRef.Text));
            TxtSaldoRef.Text = string.Format("{0:#,0.00}", Convert.ToDecimal(TxtSaldoRef.Text));
            TxtAbonoRef.Text = string.Format("{0:#,0.00}", Convert.ToDecimal(TxtAbonoRef.Text));
            TxtIgtfOrd.Text = string.Format("{0:#,0.00}", Convert.ToDecimal(TxtIgtfOrd.Text));

            if (TxtStatus.Text == "Facturada" || TxtStatus.Text == "Anulada")
            {
                double IgtfAbonado_2 = Convert.ToDouble(TxtIgtfOrd.Text.Replace(".", "")) + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos).Replace(".", ""));
                string saldo_bolivares_2 = Convert.ToString(Math.Round((double)(Convert.ToDouble(TxtSaldoOrd.Text.Replace(".", ""))), 2));
                saldo_bolivares_2 = string.Format("{0:#,0.00}", Convert.ToDecimal(saldo_bolivares_2));
                string saldo_ref_2 = Convert.ToString(Math.Round((double)(Convert.ToDouble(TxtSaldoOrd.Text.Replace(".", "")) / TB_TASA_Dolar.Tasa), 2));
                saldo_ref_2 = string.Format("{0:#,0.00}", Convert.ToDecimal(saldo_ref_2));
                TxtSaldoRef_2.Text = saldo_ref_2;
                TxtSaldoOrd_2.Text = saldo_bolivares_2;
            }
            else
            { 

            // 02/12/2024 Mostrar el saldo de la orden sin el Igtf en un nuevo Texbox y colocar el Texbox orig
            // inal que si tiene el salgo con Igtf Oculto 
            double IgtfAbonado_2 = Convert.ToDouble(TxtIgtfOrd.Text.Replace(".", "")) + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos).Replace(".", ""));
            string saldo_bolivares_2 = Convert.ToString(Math.Round((double)((Convert.ToDouble(TxtSaldoOrd.Text.Replace(".", "")) - IgtfAbonado_2)), 2));
            saldo_bolivares_2 = string.Format("{0:#,0.00}", Convert.ToDecimal(saldo_bolivares_2));
            
                string saldo_ref_2 = Convert.ToString(Math.Round((double)((Convert.ToDouble(TxtSaldoOrd.Text.Replace(".", "")) - IgtfAbonado_2) / TB_TASA_Dolar.Tasa), 2));
            saldo_ref_2 = string.Format("{0:#,0.00}", Convert.ToDecimal(saldo_ref_2));
            TxtSaldoRef_2.Text = saldo_ref_2;
            TxtSaldoOrd_2.Text = saldo_bolivares_2;
            }


        // nuevo 21-08-2023 llenar los texbox de vuelto en bs y dolares
        DataTable dt = _D_DetalleOrden.Buscar_Cambios_Realizados(TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv);
            foreach (DataRow row in dt.Rows)
            {
                textBox8.Text = string.Format("{0:#,0.00}", row["TotalPagomovilBolivares"].ToString() == "" ? (Decimal?)0.00 : Convert.ToDecimal(row["TotalPagomovilBolivares"].ToString()));
                TxtCambioRef.Text = string.Format("{0:#,0.00}", row["TotalPagomovilDolares"].ToString() == "" ? (Decimal?)0.00 : Convert.ToDecimal(row["TotalPagomovilDolares"].ToString()));
                return;
            }

        }

        private void txtMontoBs_Validating(object sender, CancelEventArgs e)
        {

            if (txtMontoBs.Text == "")
            {
                txtMontoBs.Text = "0,00";
            }
            else
            {
                BolivaresConveridos(Convert.ToDouble(txtMontoBs.Text), txtMontoBs);
                //-----------ConvertirBolivares---------------------------
                //txtMontoBs.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", txtMontoBs.Text).Replace(".", ",");
            }

        }

        private void btnCancelar1_Click(object sender, EventArgs e)
        {
            LimpiaVariablesIdAbonoPagoMovil();
            VisualizarPanel("MostrarFormulario");
            LimpiarNotasCredito();
            TxtNumFact.Text = _D_DetalleOrden.ParametroSerieManual();
            TxtNroCorrelativo.Text = "";
            HabilitacionControl("Habilitar");
            Dt_PagoMovil.Clear();


        }

        private void txtRef_Validating(object sender, CancelEventArgs e)
        {
            if (txtRef.Text == "")
            {
                txtRef.Text = "0,00";
            }
            else
            {
                txtRef.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", txtRef.Text).Replace(".", ",");
            }

            BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);
            //-----------ConvertirBolivares---------------------------
            //txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);

            string Resultado_Parametro = _D_DetalleOrden.TB_PARAMETRO("ActivaIGTF");
            bool Cobro_IGTF = Convert.ToBoolean(Convert.ToInt32(Resultado_Parametro));
            double MontoFaltanteIgtfbs = 0.00;
            if (Cobro_IGTF == true)
            {
                 MontoFaltanteIgtfbs = Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) * 0.03;
            }
            else
            {
                MontoFaltanteIgtfbs = 0.00;
            }

            double IgtfAbonado = Convert.ToDouble(TxtIgtfOrd.Text.Replace(".", ""))+ Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos).Replace(".", ""));

            if (CbxMoneda.SelectedIndex.ToString() == "0")
            {

                if (Math.Round((double)((Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) - IgtfAbonado) / TB_TASA_Dolar.Tasa), 2) >= (txtRef.Text == "" ? (Double)0.00 : Convert.ToDouble(txtRef.Text)))
                {
                    _L_Facturacion.ConvertirDolaresBolivares(txtRef, txtMonto2Bs, CbxMoneda.Text);
                    txtIGTF.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", _L_Facturacion.CalculoIgtf(txtNumeroOrden, txtMonto2Bs.Text.Replace(".", ""), DgvAbonos)).Replace(".", ",");
                    txtTranferencia.Focus();

                    if (Convert.ToDouble(TxtRecibidoREF.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtRecibidoREF.Text)) >= Convert.ToDouble(txtRef.Text == "" ? (Double)0.00 : Convert.ToDouble(txtRef.Text)))
                    {
                        TxtVuelto.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", Math.Round((Convert.ToDouble(TxtRecibidoREF.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtRecibidoREF.Text))) - Convert.ToDouble(txtRef.Text == "" ? (Double)0.00 : Convert.ToDouble(txtRef.Text)), 2));
                        TxtMontoPagoMovil.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", Math.Round(Convert.ToDouble(TB_TASA_Dolar.Tasa) * ((Convert.ToDouble(TxtRecibidoREF.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtRecibidoREF.Text))) - Convert.ToDouble(txtRef.Text == "" ? (Double)0.00 : Convert.ToDouble(txtRef.Text))), 2));
                    }

                }
                else
                {

                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("El monto a pagar deber ser menor o igual a su saldo sin IGTF");
                    _FrmMensajes.ShowDialog();
                    e.Cancel = true;
                    txtRef.Text = "";
                    txtRef.Focus();

                }
            }

            if (CbxMoneda.SelectedIndex.ToString() == "1")
            {
                if (TB_TASA_Euro.Tasa != 0 && TB_TASA_Euro.Tasa != null)
                {
                    if (Math.Round((double)((Convert.ToDouble(txtMontoBs.Text.Replace(".","")) - IgtfAbonado) / TB_TASA_Euro.Tasa), 2) >= (txtRef.Text == "" ? (Double)0.00 : Convert.ToDouble(txtRef.Text)))
                    {
                        _L_Facturacion.ConvertirDolaresBolivares(txtRef, txtMonto2Bs, CbxMoneda.Text);
                        txtIGTF.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", _L_Facturacion.CalculoIgtf(txtNumeroOrden, txtMonto2Bs.Text.Replace(".", ""), DgvAbonos)).Replace(".", ",");
                        txtTranferencia.Focus();


                        if (Convert.ToDouble(TxtRecibidoREF.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtRecibidoREF.Text)) >= Convert.ToDouble(txtRef.Text == "" ? (Double)0.00 : Convert.ToDouble(txtRef.Text)))
                        {
                            TxtVuelto.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", Math.Round((Convert.ToDouble(TxtRecibidoREF.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtRecibidoREF.Text))) - Convert.ToDouble(txtRef.Text == "" ? (Double)0.00 : Convert.ToDouble(txtRef.Text)), 2));
                            TxtMontoPagoMovil.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", Math.Round(Convert.ToDouble(TB_TASA_Euro.Tasa) * ((Convert.ToDouble(TxtRecibidoREF.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtRecibidoREF.Text))) - Convert.ToDouble(txtRef.Text == "" ? (Double)0.00 : Convert.ToDouble(txtRef.Text))), 2));
                        }

                    }

                    else
                    {

                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("El monto a pagar deber ser menor o igual a su saldo sin IGTF");
                        _FrmMensajes.ShowDialog();
                        txtRef.Text = "";
                        txtRef.Focus();

                    }

                }

                else
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("Debe Actualizar la Tasa de las Monedas y Activación de Secuencia Diaria, verifique");
                    _FrmMensajes.ShowDialog();
                }

            }



        }

        private void txtRef_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                LimpiarTxbox();
            }
        }

        private void txtMontoBs_KeyDown(object sender, KeyEventArgs e)
        {
            Double Bolivares = 0.00;
            Double TotalAbono = 0.00;
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    //lo agrego para validar que los numeros ingresados en el textbox de monto tengan los separadores adecuados cuando se presiona enter A.H 
                    if (string.IsNullOrEmpty(txtMontoBs.Text) && txtMontoBs.Text == "") txtMontoBs.Text = "0,00";
                    else
                    {
                        txtMontoBs.Text = string.Format("{0:#,0.00}", Convert.ToDecimal(txtMontoBs.Text));
                    }




                    Bolivares = (txtMontoBs.Text == "" ? (Double)0.00 : Convert.ToDouble(txtMontoBs.Text.Replace(".", "")));
                    TotalAbono = Convert.ToDouble(_L_Facturacion.TotalizarAbono(DgvAbonos));

                    if (CbxMetodosPago.Text == "Efectivo")
                    {
                        if (txtMontoBs.Text != "" && txtMontoBs.Text != "0.00" && txtMontoBs.Text != "0" && Bolivares <= (Math.Round((TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos))) - TotalAbono, 2)))
                        {
                            if (Bolivares > 0)
                            {
                                _L_Facturacion.GuardarAbonoGrid(2,Dt_Abonos, CbxMetodosPago.Text, "Bolivares", "", txtMontoBs.Text, "", DtpFecha.Value.ToString(), CbxMetodosPago.SelectedValue.ToString());
                                
                                BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);
                                //-----------ConvertirBolivares---------------------------
                                //txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);

                                LimpiarTxbox();
                            }
                        }
                        else
                        {
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje("El monto debe ser igual o menor al saldo de la orden");
                            _FrmMensajes.ShowDialog();

                        }
                    }

                    
                    if (CbxMetodosPago.Text == "Iva Retenido")
                    {
                        if (txtMontoBs.Text != "" && txtMontoBs.Text != "0.00" && txtMontoBs.Text != "0" && Bolivares <= (Math.Round((TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos))) - TotalAbono, 2)))
                        {
                            if (Bolivares > 0)
                            {
                                _L_Facturacion.GuardarAbonoGrid(2,Dt_Abonos, CbxMetodosPago.Text, "Bolivares", "", txtMontoBs.Text, "", DtpFecha.Value.ToString(), CbxMetodosPago.SelectedValue.ToString(), "000");
                                BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);
                                //-----------ConvertirBolivares---------------------------
                                //txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);

                                LimpiarTxbox();
                            }
                        }

                        else
                        {
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje("El monto debe ser igual o menor al saldo de la orden");
                            _FrmMensajes.ShowDialog();
                        }

                    }

                    if (CbxMetodosPago.Text == "ISLR Retenido")
                    {
                        if (txtMontoBs.Text != "" && txtMontoBs.Text != "0.00" && txtMontoBs.Text != "0" && Bolivares <= (Math.Round((TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos))) - TotalAbono, 2)))
                        {
                            if (Bolivares > 0)
                            {
                                _L_Facturacion.GuardarAbonoGrid(2,Dt_Abonos, CbxMetodosPago.Text, "Bolivares", "", txtMontoBs.Text, "", DtpFecha.Value.ToString(), CbxMetodosPago.SelectedValue.ToString(), "000");
                                BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);
                                //-----------ConvertirBolivares---------------------------
                                //txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);

                                LimpiarTxbox();
                            }
                        }
                        else
                        {
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje("El monto debe ser igual o menor al saldo de la orden");
                            _FrmMensajes.ShowDialog();
                        }

                    }
                }

            }

            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }

            finally
            {
                if (Bolivares < 0)
                {
                    MessageBox.Show("El Monto No puede ser Negativo", "Verifique", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMontoBs.Text = "";
                    txtMontoBs.Focus();
                }

            }

        }


        private void CbxMetodosPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxMetodosPago.SelectedIndex != -1)
            {

                if (CbxMetodosPago.Text == "Transferencia Divisa")
                {
                    _L_Facturacion.LLenarComboboxBancos(CbxBanco, true);
                    CbxMetodosPago2.SelectedIndex = CbxMetodosPago.SelectedIndex;
                    CbxBanco.SelectedIndex = 1;
                    txtMonto2Bs.Enabled = false;
                    txtRef.Enabled = true; ;
                    txtMontoBs.Enabled = false;

                    //agregado nuevo 17-05-2023
                    DataTable dt = _D_DetalleOrden.BucarTotalAbonosRealizados(TB_CAORDSER.NumOrdserv);
                    Double Igtf_TotalAboTranferenciaDolar = Math.Round(Convert.ToDouble(dt.Rows[0]["TotalPagoIgtf"].ToString()), 2);
                    Igtf_TotalAboTranferenciaDolar = Igtf_TotalAboTranferenciaDolar + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos));

                    BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);
                    //-----------ConvertirBolivares---------------------------
                    //txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);

                    string Resultado_Parametro = _D_DetalleOrden.TB_PARAMETRO("ActivaIGTF");
                    bool Cobro_IGTF = Convert.ToBoolean(Convert.ToInt32(Resultado_Parametro));

                    if(Cobro_IGTF == true)
                    {
                        double MontoFaltanteIgtfbs = (Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) - Igtf_TotalAboTranferenciaDolar) * 0.03;

                        txtRef.Text = Convert.ToString(Math.Round(((Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) + MontoFaltanteIgtfbs) / Convert.ToDouble(TB_TASA_Dolar.Tasa)), 2));
                    }
                    else
                    {
                        txtRef.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) / Convert.ToDouble(TB_TASA_Dolar.Tasa)), 2));
                    }



                    //modificado 17-05-2023
                    txtIGTF.Text = Convert.ToString(_L_Facturacion.CalculoIgtf(txtNumeroOrden, Convert.ToString(Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) - Igtf_TotalAboTranferenciaDolar), DgvAbonos));
                    BolivaresConveridos(Convert.ToDouble(Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) + Convert.ToDouble(txtIGTF.Text)), txtMonto2Bs);
                    //txtMonto2Bs.Text = Convert.ToString(Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) + Convert.ToDouble(txtIGTF.Text));
                    VisualizarPanel("MostrarPanelSecundario");

                    label26.Visible = false;
                    txtCVC.Enabled = false;
                    txtCVC.Visible = false;
                    CbxTarjeta.Visible = false;
                    CbxTarjeta.Enabled = false;
                    txtVence.Enabled = false;
                    txtVence.Visible = false;
                    label24.Visible = false;
                    txtCheque.Enabled = false;
                    txtCheque.Visible = false;
                    label15.Visible = true;
                    label27.Visible = false;
                    label22.Visible = true;
                    label28.Visible = false;
                    CbxMoneda.Enabled = true;
                    _L_Facturacion.ComboboxTipoMonedaTranferenciaDivisa(CbxMoneda,txtTasaFact);
                    CbxMoneda.SelectedIndex = 0;
                    label5.Visible = true;
                    CbxMoneda.Visible = true;
                    label42.Visible = true;
                    txtIGTF.Visible = true;
                    LbePagoMovil.Visible = false;
                    label4.Visible = true;
                    txtRef.Visible = true;
                    label21.Visible = true;
                    DtpFecha.Visible = true;
                    TxtVuelto.Visible = true;
                    CbxMetodosPago2.Visible = true;
                    label41.Visible = true;
                    LblMontoBillete.Visible = false;
                    CbxBillete.Visible = false;
                    LblCodBillete.Visible = false;
                    TxtCodBillete.Visible = false;
                    DgvBilletes.Visible = false;
                    LblBancoRecep.Visible = false;
                    CbxBancoRecp.Visible = false;
                    label45.Text = "Banco";
                    label28.Text = "N° Cuenta/Tarjeta";
                    Bs.Text = "Monto en Bs.";
                    CbxPunto_Venta.Visible = false;
                    TxtVuelto.Visible = false;
                    TxtVuelto.Enabled = false;
                    label6.Visible = false;
                    FalBod = false;
                    Validar_FalBod(CbxBanco, txtTranferencia);
                   


                    //Reubicacion de los objetos 
                    label41.Location = new System.Drawing.Point(33, 53);
                    CbxMetodosPago2.Location = new System.Drawing.Point(33, 75);
                    label5.Location = new System.Drawing.Point(226, 53);
                    CbxMoneda.Location = new System.Drawing.Point(226, 75);
                    label4.Location = new System.Drawing.Point(418, 53);
                    txtRef.Location = new System.Drawing.Point(418, 75);
                    label45.Location = new System.Drawing.Point(33, 118);
                    CbxBanco.Location = new System.Drawing.Point(33, 140);
                    label42.Location = new System.Drawing.Point(225, 118);
                    txtIGTF.Location = new System.Drawing.Point(225, 140);
                    Bs.Location = new System.Drawing.Point(417, 118);
                    txtMonto2Bs.Location = new System.Drawing.Point(417, 140);
                    label22.Location = new System.Drawing.Point(33, 183);
                    txtTranferencia.Location = new System.Drawing.Point(33, 205);
                    label21.Location = new System.Drawing.Point(225, 183);
                    DtpFecha.Location = new System.Drawing.Point(225, 205);
                    btnCancelar2.Location = new Point(370, 255);
                    btnProcesar2.Location = new Point(491, 255);


                    // Tamaño de la fuente 
                    Bs.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label22.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label41.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label5.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label4.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label45.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label42.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label21.Font = new Font("Century Gothic", 12, FontStyle.Bold);


                    // Tamaño de los Objetos 
                    PnlSecundario.Size = new System.Drawing.Size(618, 300);
                    label2.Size = new System.Drawing.Size(617, 35);
                    label41.Size = new System.Drawing.Size(140, 19);
                    CbxMetodosPago2.Size = new System.Drawing.Size(168, 21);
                    label5.Size = new System.Drawing.Size(121, 19);
                    CbxMoneda.Size = new System.Drawing.Size(168, 21);
                    label4.Size = new System.Drawing.Size(42, 19);
                    txtRef.Size = new System.Drawing.Size(168, 21);
                    label45.Size = new System.Drawing.Size(168, 21);
                    CbxBanco.Size = new System.Drawing.Size(168, 21);
                    label42.Size = new System.Drawing.Size(168, 21);
                    txtIGTF.Size = new System.Drawing.Size(168, 21);
                    Bs.Size = new System.Drawing.Size(168, 21);
                    txtMonto2Bs.Size = new System.Drawing.Size(168, 21);
                    label22.Size = new System.Drawing.Size(168, 21);
                    txtTranferencia.Size = new System.Drawing.Size(168, 21);
                    label21.Size = new System.Drawing.Size(168, 21);
                    DtpFecha.Size = new System.Drawing.Size(168, 21);

                }


                if (CbxMetodosPago.Text == "Efectivo Divisa")
                {
                    _L_Facturacion.LLenarComboboxBancos2(CbxBanco, true);
                    CbxMetodosPago2.SelectedIndex = 3;
                    CbxBanco.SelectedIndex = 1;
                    txtMonto2Bs.Enabled = false;
                    txtRef.Enabled = true; ;
                    txtMontoBs.Enabled = false;
                    DataTable dt = _D_DetalleOrden.BucarTotalAbonosRealizados(TB_CAORDSER.NumOrdserv);
                    Double Igtf_TotalAboTranferenciaDolar = Math.Round(Convert.ToDouble(dt.Rows[0]["TotalPagoIgtf"].ToString()), 2);
                    Igtf_TotalAboTranferenciaDolar = Igtf_TotalAboTranferenciaDolar + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos));

                    BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);
                    //-----------ConvertirBolivares---------------------------
                    //txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);

                    string Resultado_Parametro = _D_DetalleOrden.TB_PARAMETRO("ActivaIGTF");
                    bool Cobro_IGTF = Convert.ToBoolean(Convert.ToInt32(Resultado_Parametro));

                    if (Cobro_IGTF == true)
                    {
                        double MontoFaltanteIgtfbs = (Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) - Igtf_TotalAboTranferenciaDolar) * 0.03;

                        txtRef.Text = Convert.ToString(Math.Round(((Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) + MontoFaltanteIgtfbs) / Convert.ToDouble(TB_TASA_Dolar.Tasa)), 2));
                    }
                    else
                    {
                        txtRef.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) / Convert.ToDouble(TB_TASA_Dolar.Tasa)), 2));
                    }

                    txtIGTF.Text = Convert.ToString(_L_Facturacion.CalculoIgtf(txtNumeroOrden, Convert.ToString(Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) - Igtf_TotalAboTranferenciaDolar), DgvAbonos));

                    BolivaresConveridos(Convert.ToDouble(Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) + Convert.ToDouble(txtIGTF.Text)), txtMonto2Bs);
                    //txtMonto2Bs.Text = Convert.ToString(Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) + Convert.ToDouble(txtIGTF.Text));
                    VisualizarPanel("MostrarPanelSecundario");
                    _L_Facturacion.ComboboxTipoMonedaTranferenciaDivisa(CbxMoneda, txtTasaFact );

                    label26.Visible = false;
                    txtCVC.Enabled = false;
                    txtCVC.Visible = false;
                    CbxTarjeta.Visible = false;
                    CbxTarjeta.Enabled = false;
                    txtVence.Enabled = false;
                    txtVence.Visible = false;
                    label24.Visible = false;
                    txtCheque.Enabled = false;
                    txtCheque.Visible = false;
                    label15.Visible = true;
                    label27.Visible = false;
                    label5.Visible = true;
                    label28.Visible = false;
                    CbxMoneda.Enabled = true;
                    CbxMoneda.SelectedIndex = 0;
                    CbxMoneda.Visible = true;
                    label42.Visible = true;
                    txtIGTF.Visible = true;
                    label4.Visible = true;
                    txtRef.Visible = true;
                    label21.Visible = true;
                    label6.Visible = true;
                    TxtVuelto.Visible = true;
                    CbxMetodosPago2.Visible = true;
                    label41.Visible = true;
                    LblBancoRecep.Visible = false;
                    CbxBancoRecp.Visible = false;
                    CbxBanco.Enabled = false;
                    CbxBanco.Visible = false;
                    label45.Visible = false;
                    TxtRecibidoREF.Visible = true;
                    label71.Visible = true;
                    lbCedulaPago.Visible = true;
                    CbxNacionalidadPagoMovil.Visible = true;
                    TxtCedulaPagoMovil.Visible = true;
                    lbCedularPago.Visible = true;
                    CbxCelularPagoMovil.Visible = true;
                    TxtCedularPagoMovil.Visible = true;
                    lbMontoPago.Visible = true;
                    TxtMontoPagoMovil.Visible = true;
                    lbBancoPago.Visible = true;
                    CbxBancoPagoMovil.Visible = true;
                    label22.Visible = false;
                    txtTranferencia.Visible = false;
                    label21.Visible = false;
                    DtpFecha.Visible = false;
                    LbePagoMovil.Visible = true;
                    LblMontoBillete.Visible = true;
                    CbxBillete.Visible = true;
                    LblCodBillete.Visible = true;
                    TxtCodBillete.Visible = true;
                    DgvBilletes.Visible = true;
                    FalBod = true;
                    label45.Text = "Banco";
                    label28.Text = "N° Cuenta/Tarjeta";
                    Bs.Text = "Monto en Bs.";
                    label71.Text = "Recibido REF.";

                    Validar_FalBod(CbxBanco, txtTranferencia);
                    CbxBanco.SelectedIndex = 0;
                    LLenar_Datos_PagoMovil();
                    LimpiarDgvBilletes();
                    LimpiarCamposBill();
                    Dt_Billetes.Rows.Clear();

                    txtTotalRef.Visible = true;
                    label73.Visible = true;

                    txtTasaFact.Visible = true;
                    label74.Visible = true;
                    Bs.Visible = true;
                    Bs.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label71.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    LblMontoBillete.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    LblCodBillete.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label5.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label42.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label41.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label4.Font = new Font("Century Gothic", 12, FontStyle.Bold);

                    //Tasa
                    label74.Location = new System.Drawing.Point(418, 53);
                    txtTasaFact.Location = new System.Drawing.Point(418, 75);
                    label74.Visible = true;
                    txtTasaFact.Visible = true;

                    //REF
                    label4.Location = new System.Drawing.Point(33, 118);
                    txtRef.Location = new System.Drawing.Point(33, 140);

                    //IGTF
                    label42.Location = new System.Drawing.Point(226, 118);
                    txtIGTF.Location = new System.Drawing.Point(226, 140);

                    //TOTAL REF
                    label73.Location = new System.Drawing.Point(418, 118);
                    txtTotalRef.Location = new System.Drawing.Point(418, 140);

                    //MONTO BS
                    Bs.Location = new System.Drawing.Point(226, 180);
                    txtMonto2Bs.Location = new System.Drawing.Point(226, 202);

                    //SERIAL BILLETE
                    LblCodBillete.Location = new System.Drawing.Point(33, 240);
                    TxtCodBillete.Location = new System.Drawing.Point(33, 262);

                    //DENOM BILLETE
                    LblMontoBillete.Location = new System.Drawing.Point(226, 240);
                    CbxBillete.Location = new System.Drawing.Point(226, 262);

                    lbCedulaPago.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    lbMontoPago.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    lbBancoPago.Font = new Font("Century Gothic", 12, FontStyle.Bold);

                    //RECIBIDO REF
                    label71.Location = new System.Drawing.Point(33, 180);
                    TxtRecibidoREF.Location = new System.Drawing.Point(33, 202);

                    //VUELTO
                    label6.Location = new System.Drawing.Point(418, 180);
                    TxtVuelto.Location = new System.Drawing.Point(418, 202);

                    //Reubicacion de los objetos 




                    DgvBilletes.Location = new System.Drawing.Point(33, 300);
                    label5.Location = new System.Drawing.Point(226, 53);
                   
                    label41.Location = new System.Drawing.Point(33, 53);
                    
       
                    CbxMoneda.Location = new System.Drawing.Point(226, 75);
                   
                    CbxMetodosPago2.Location = new System.Drawing.Point(33, 75);
                  
                    LbePagoMovil.Location = new System.Drawing.Point(0, 378);
                    btnProcesar2.Location = new System.Drawing.Point(512, 568);
                    btnCancelar2.Location = new System.Drawing.Point(391, 568);
                    lbCedulaPago.Location = new System.Drawing.Point(33, 432);
                    CbxNacionalidadPagoMovil.Location = new System.Drawing.Point(33, 453);
                    TxtCedulaPagoMovil.Location = new System.Drawing.Point(81, 453);
                    lbCedularPago.Location = new System.Drawing.Point(223, 432);
                    CbxCelularPagoMovil.Location = new System.Drawing.Point(223, 453);
                    TxtCedularPagoMovil.Location = new System.Drawing.Point(280, 453);
                    lbMontoPago.Location = new System.Drawing.Point(33, 503);
                    lbBancoPago.Location = new System.Drawing.Point(419, 432);
                    CbxBancoPagoMovil.Location = new System.Drawing.Point(419, 453);
                    TxtMontoPagoMovil.Location = new System.Drawing.Point(33, 525);


                    // Tamaño de los Objetos 
                    PnlSecundario.Size = new System.Drawing.Size(653, 617);
                    TxtMontoPagoMovil.Size = new System.Drawing.Size(168, 21);
                    CbxBancoPagoMovil.Size = new System.Drawing.Size(168, 21);
                    lbBancoPago.Size = new System.Drawing.Size(59, 19);
                    lbMontoPago.Size = new System.Drawing.Size(57, 19);
                    TxtCedularPagoMovil.Size = new System.Drawing.Size(122, 21);
                    CbxCelularPagoMovil.Size = new System.Drawing.Size(54, 21);
                    lbCedularPago.Size = new System.Drawing.Size(157, 19);
                    TxtCedulaPagoMovil.Size = new System.Drawing.Size(122, 21);
                    CbxNacionalidadPagoMovil.Size = new System.Drawing.Size(46, 21);
                    lbCedulaPago.Size = new System.Drawing.Size(176, 19);
                    TxtRecibidoREF.Size = new System.Drawing.Size(168, 21);
                    label71.Size = new System.Drawing.Size(107, 19);
                    TxtCodBillete.Size = new System.Drawing.Size(168, 21);
                    LblMontoBillete.Size = new System.Drawing.Size(207, 19);
                    CbxBillete.Size = new System.Drawing.Size(168, 24);
                    LblCodBillete.Size = new System.Drawing.Size(133, 19);
                    DgvBilletes.Size = new System.Drawing.Size(553, 75);
                    label5.Size = new System.Drawing.Size(121, 19);
                    label42.Size = new System.Drawing.Size(41, 19);
                    label41.Size = new System.Drawing.Size(140, 19);
                    label6.Size = new System.Drawing.Size(58, 19);
                    Bs.Size = new System.Drawing.Size(100, 19);
                    txtMonto2Bs.Size = new System.Drawing.Size(168, 21);
                    TxtVuelto.Size = new System.Drawing.Size(168, 21);
                    CbxMoneda.Size = new System.Drawing.Size(168, 21);
                    btnCancelar2.Size = new System.Drawing.Size(105, 30);
                    btnProcesar2.Size = new System.Drawing.Size(104, 30);
                    txtIGTF.Size = new System.Drawing.Size(168, 21);
                    CbxMetodosPago2.Size = new System.Drawing.Size(168, 21);
                    label4.Size = new System.Drawing.Size(42, 19);
                    txtRef.Size = new System.Drawing.Size(168, 21);
                    label2.Size = new System.Drawing.Size(653, 35);
                    LbePagoMovil.Size = new System.Drawing.Size(653, 35);

                    double reftotal = Convert.ToDouble(txtRef.Text.Replace(".", ""));
                    double igtftotal = Convert.ToDouble(txtIGTF.Text.Replace(".", ""));
                    double tasa = Convert.ToDouble(txtTasaFact.Text.Replace(".", ""));

                    //txtTotalRef.Text = Convert.ToString(reftotal + (igtftotal / tasa));
                    txtTotalRef.Text = string.Format("{0:#,0.00}", Convert.ToDecimal(reftotal + (igtftotal / tasa)));



                }

                if (CbxMetodosPago.Text == "Transferencia")
                {
                    _L_Facturacion.LLenarComboboxBancos(CbxBanco, false);
                    _L_Facturacion.LLenarCbxBancoRecp(CbxBancoRecp, false);
                    CbxMetodosPago2.SelectedIndex = CbxMetodosPago.SelectedIndex;
                    CbxMoneda.SelectedIndex = 2;
                    txtMonto2Bs.Enabled = true;

                    BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);
                    //-----------ConvertirBolivares---------------------------
                    //txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);

                    BolivaresConveridos(Convert.ToDouble(txtMontoBs.Text.Replace(".", "")), txtMonto2Bs);
                    //txtMonto2Bs.Text = Convert.ToString(Convert.ToDouble(txtMontoBs.Text.Replace(".", "")));

                    txtRef.Enabled = false;
                    txtRef.Visible = false;
                    txtMontoBs.Enabled = false;
                    VisualizarPanel("MostrarPanelSecundario");
                    label28.Visible = false;
                    label22.Visible = true;
                    label26.Visible = false;
                    txtCVC.Enabled = false;
                    txtCVC.Visible = false;
                    CbxTarjeta.Visible = false;
                    CbxTarjeta.Enabled = false;
                    txtVence.Enabled = false;
                    txtVence.Visible = false;
                    label24.Visible = false;
                    txtCheque.Enabled = false;
                    txtCheque.Visible = false;
                    label15.Visible = true;
                    label27.Visible = false;
                    label21.Visible = true;
                    DtpFecha.Visible = true;
                    CbxMoneda.Enabled = false;
                    CbxMoneda.Visible = false;
                    label5.Visible = false;
                    label42.Visible = false;
                    txtIGTF.Visible = false;
                    LbePagoMovil.Visible = false;
                    label4.Visible = false;
                    CbxMetodosPago2.Visible = true;
                    label41.Visible = true;
                    LblMontoBillete.Visible = false;
                    CbxBillete.Visible = false;
                    LblCodBillete.Visible = false;
                    TxtCodBillete.Visible = false;
                    DgvBilletes.Visible = false;
                    label6.Visible = false;
                    TxtVuelto.Visible = false;
                    LblBancoRecep.Visible = true;
                    CbxBancoRecp.Visible = true;
                    CbxBancoRecp.Enabled = true;
                    Bs.Visible = true;

                    label45.Text = "Banco Emisor";
                    label28.Text = "N° Cuenta/Tarjeta";
                    Bs.Text = "Monto";


                    // Tamaño de la fuente 
                    Bs.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label22.Font = new Font("Century Gothic", 12, FontStyle.Bold);

                    //Reubicacion de los objetos 
                    btnCancelar2.Location = new System.Drawing.Point(165, 249);
                    btnProcesar2.Location = new System.Drawing.Point(285, 249);
                    label41.Location = new System.Drawing.Point(32, 54);
                    Bs.Location = new System.Drawing.Point(32, 184);
                    txtMonto2Bs.Location = new System.Drawing.Point(33, 206);
                    CbxMetodosPago2.Location = new System.Drawing.Point(33, 76);
                    txtTranferencia.Location = new System.Drawing.Point(33, 144);
                    label22.Location = new System.Drawing.Point(32, 121);
                    CbxBanco.Location = new System.Drawing.Point(218, 76);
                    label45.Location = new System.Drawing.Point(218, 54);
                    label21.Location = new System.Drawing.Point(218, 184);
                    DtpFecha.Location = new System.Drawing.Point(218, 206);
                    LblBancoRecep.Location = new System.Drawing.Point(218, 121);
                    CbxBancoRecp.Location = new System.Drawing.Point(218, 144);

                    // Tamaño de los Objetos 
                    PnlSecundario.Size = new System.Drawing.Size(424, 299);
                    label2.Size = new System.Drawing.Size(423, 35);
                    btnCancelar2.Size = new System.Drawing.Size(105, 30);
                    btnProcesar2.Size = new System.Drawing.Size(104, 30);
                    label41.Size = new System.Drawing.Size(140, 19);
                    Bs.Size = new System.Drawing.Size(57, 19);
                    txtMonto2Bs.Size = new System.Drawing.Size(168, 25);
                    CbxMetodosPago2.Location = new System.Drawing.Point(33, 76);
                    txtTranferencia.Size = new System.Drawing.Size(168, 25);
                    label22.Size = new System.Drawing.Size(134, 19);
                    CbxBanco.Size = new System.Drawing.Size(168, 25);
                    label45.Size = new System.Drawing.Size(59, 19);
                    label21.Size = new System.Drawing.Size(58, 19);
                    DtpFecha.Size = new System.Drawing.Size(168, 25);
                    LblBancoRecep.Size = new System.Drawing.Size(132, 19);
                    CbxBancoRecp.Size = new System.Drawing.Size(168, 25);

                }

                if (CbxMetodosPago.Text == "Pago Móvil")
                {
                    _L_Facturacion.LLenarComboboxBancos(CbxBanco, false);
                    _L_Facturacion.LLenarCbxBancoRecp_PagoMovil(CbxBancoRecp, false);
                    CbxMetodosPago2.SelectedIndex = 8;
                    CbxMoneda.SelectedIndex = 2;
                    txtMonto2Bs.Enabled = true;

                    BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);
                    //-----------ConvertirBolivares---------------------------
                    //txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);
                    BolivaresConveridos(Convert.ToDouble(txtMontoBs.Text.Replace(".", "")), txtMonto2Bs);
                    //txtMonto2Bs.Text = Convert.ToString(Convert.ToDouble(txtMontoBs.Text.Replace(".","")));


                    txtRef.Enabled = false;
                    txtRef.Visible = false;
                    txtMontoBs.Enabled = false;
                    VisualizarPanel("MostrarPanelSecundario");
                    label28.Visible = false;
                    label22.Visible = true;
                    label26.Visible = false;
                    txtCVC.Enabled = false;
                    txtCVC.Visible = false;
                    CbxTarjeta.Visible = false;
                    CbxTarjeta.Enabled = false;
                    txtVence.Enabled = false;
                    txtVence.Visible = false;
                    label24.Visible = false;
                    txtCheque.Enabled = false;
                    txtCheque.Visible = false;
                    label15.Visible = true;
                    label27.Visible = false;
                    CbxMoneda.Enabled = false;
                    CbxMoneda.Visible = false;
                    label5.Visible = false;
                    label42.Visible = false;
                    txtIGTF.Visible = false;
                    label4.Visible = false;

                    label21.Visible = true;
                    DtpFecha.Visible = true;
                    CbxMetodosPago2.Visible = true;
                    label41.Visible = true;
                    LblMontoBillete.Visible = false;
                    CbxBillete.Visible = false;
                    LblCodBillete.Visible = false;
                    TxtCodBillete.Visible = false;
                    DgvBilletes.Visible = false;
                    label6.Visible = false;
                    TxtVuelto.Visible = false;
                    LblBancoRecep.Visible = true;
                    CbxBancoRecp.Visible = true;
                    CbxBancoRecp.Enabled = true;
                    label49.Visible = false;
                    CbxPunto_Venta.Visible = false;
                    CbxPunto_Venta.Enabled = false;
                    LbePagoMovil.Visible = false;
                   
                    label45.Text = "Banco Emisor";
                    label28.Text = "Referencia";
                    Bs.Text = "Monto";

                    Bs.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label22.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label41.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label22.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label45.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label21.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    LblBancoRecep.Font = new Font("Century Gothic", 12, FontStyle.Bold);

                    //Reubicacion de los objetos 
                    btnCancelar2.Location = new System.Drawing.Point(165, 249);
                    btnProcesar2.Location = new System.Drawing.Point(285, 249);
                    label41.Location = new System.Drawing.Point(32, 54);
                    Bs.Location = new System.Drawing.Point(32, 184);
                    txtMonto2Bs.Location = new System.Drawing.Point(33, 206);
                    CbxMetodosPago2.Location = new System.Drawing.Point(33, 76);
                    txtTranferencia.Location = new System.Drawing.Point(33, 144);
                    label22.Location = new System.Drawing.Point(32, 121);
                    CbxBanco.Location = new System.Drawing.Point(218, 76);
                    label45.Location = new System.Drawing.Point(218, 54);
                    label21.Location = new System.Drawing.Point(218, 184);
                    DtpFecha.Location = new System.Drawing.Point(218, 206);
                    LblBancoRecep.Location = new System.Drawing.Point(218, 121);
                    CbxBancoRecp.Location = new System.Drawing.Point(218, 144);


                    // Tamaño de los Objetos 
                    PnlSecundario.Size = new System.Drawing.Size(424, 299);
                    label2.Size = new System.Drawing.Size(423, 35);
                    btnCancelar2.Size = new System.Drawing.Size(105, 30);
                    btnProcesar2.Size = new System.Drawing.Size(104, 30);
                    label41.Size = new System.Drawing.Size(140, 19);
                    Bs.Size = new System.Drawing.Size(57, 19);
                    txtMonto2Bs.Size = new System.Drawing.Size(168, 25);
                    CbxMetodosPago2.Location = new System.Drawing.Point(33, 76);
                    txtTranferencia.Size = new System.Drawing.Size(168, 25);
                    label22.Size = new System.Drawing.Size(134, 19);
                    CbxBanco.Size = new System.Drawing.Size(168, 25);
                    label45.Size = new System.Drawing.Size(59, 19);
                    label21.Size = new System.Drawing.Size(58, 19);
                    DtpFecha.Size = new System.Drawing.Size(168, 25);
                    LblBancoRecep.Size = new System.Drawing.Size(132, 19);
                    CbxBancoRecp.Size = new System.Drawing.Size(168, 25);


                    //// SELECIONAR EL BANCO Plaza
                    for (int i = 0; i < CbxBancoRecp.Items.Count; i++)
                    {
                        if (CbxBancoRecp.Text != "BANCO PLAZA")
                            CbxBancoRecp.SelectedIndex = i;
                        else
                            break;


                    }

                }


                if (CbxMetodosPago.Text == "Cheque")
                {
                    _L_Facturacion.LLenarComboboxBancos(CbxBanco, false);
                    CbxMetodosPago2.SelectedIndex = CbxMetodosPago.SelectedIndex;
                    CbxMoneda.SelectedIndex = 2;
                    txtMonto2Bs.Enabled = true;
                    BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);
                    //-----------ConvertirBolivares---------------------------
                    //txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);
                    BolivaresConveridos(Convert.ToDouble(txtMontoBs.Text.Replace(".", "")), txtMonto2Bs);
                    //txtMonto2Bs.Text = Convert.ToString(Convert.ToDouble(txtMontoBs.Text.Replace(".","")));

                    txtRef.Enabled = false;
                    txtMontoBs.Enabled = false;
                    VisualizarPanel("MostrarPanelSecundario");
                    txtCheque.Enabled = true;
                    txtCheque.Visible = true;
                    label24.Visible = true;
                    txtRef.Visible = false;
                    label4.Visible = false;
                    label28.Visible = true;
                    label22.Visible = false;

                    label26.Visible = false;
                    txtCVC.Visible = false;
                    txtCVC.Enabled = false;
                    CbxTarjeta.Visible = false;
                    CbxTarjeta.Enabled = false;
                    label15.Visible = true;

                    label5.Visible = false;
                    CbxMoneda.Enabled = false;

                    label5.Visible = false;
                    CbxMoneda.Visible = false;

                    label42.Visible = false;
                    txtIGTF.Visible = false;

                    label4.Visible = false;
                    txtRef.Visible = false;
                   


                    // agregadp para el billete de falbod  13/06/2023
                    LblMontoBillete.Visible = false;
                    CbxBillete.Visible = false;
                    LblCodBillete.Visible = false;
                    TxtCodBillete.Visible = false;
                    DgvBilletes.Visible = false;

                    // Agregadp para el banco receptor de transferencia 22/06/2023
                    LblBancoRecep.Visible = false;
                    CbxBancoRecp.Visible = false;
                    label45.Text = "Banco";
                    label28.Text = "N° Cuenta/Tarjeta";

                    label41.Location = new Point(4, 43);
                    CbxMetodosPago2.Location = new Point(8, 66);
                    Bs.Text = "Monto en Bs";
                    txtTranferencia.Size = new Size(175, 21);
                }

                if (CbxMetodosPago.Text == "Debito")
                {
                    _L_Facturacion.LLenarComboboxBancos(CbxBanco, false);
                    CbxMetodosPago2.SelectedIndex = CbxMetodosPago.SelectedIndex;
                    CbxMoneda.SelectedIndex = 2;
                    txtMonto2Bs.Enabled = true;
                    BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);
                    //-----------ConvertirBolivares---------------------------
                    //txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);
                    BolivaresConveridos(Convert.ToDouble(txtMontoBs.Text.Replace(".", "")), txtMonto2Bs);
                    //txtMonto2Bs.Text = Convert.ToString(Convert.ToDouble(txtMontoBs.Text.Replace(".","")));
                    txtRef.Visible = false;
                    txtRef.Enabled = false; ;
                    txtMontoBs.Enabled = false;
                    VisualizarPanel("MostrarPanelSecundario");
                    label28.Visible = true;
                    label22.Visible = false;
                    label4.Visible = false;
                    label27.Visible = false;
                    label26.Visible = false;
                    txtCVC.Enabled = false;
                    txtCVC.Visible = false;
                    CbxTarjeta.Visible = false;
                    CbxTarjeta.Enabled = false;
                    txtVence.Enabled = false;
                    txtVence.Visible = false;
                    label24.Visible = false;
                    txtCheque.Enabled = false;
                    txtCheque.Visible = false;
                    label15.Visible = true;
                    label5.Visible = false;
                    CbxMoneda.Enabled = false;
                    CbxMoneda.Visible = false;
                    label42.Visible = false;
                    txtIGTF.Visible = false;
                    label21.Visible = false;
                    DtpFecha.Visible = false;
                    label6.Visible = false;
                    TxtVuelto.Visible = false;
                    CbxMetodosPago2.Visible = true;
                    label41.Visible = true;
                    _L_Facturacion.LLenarComboboxTipoPunto(CbxPunto_Venta, "TD");
                    label49.Visible = true;
                    CbxPunto_Venta.Visible = true;
                    LblMontoBillete.Visible = false;
                    CbxBillete.Visible = false;
                    LblCodBillete.Visible = false;
                    TxtCodBillete.Visible = false;
                    DgvBilletes.Visible = false;
                    LblBancoRecep.Visible = false;
                    CbxBancoRecp.Visible = false;
                    LbePagoMovil.Visible = false;
                    Bs.Visible = false;

                    label45.Text = "Banco";
                    label28.Text = "N° Tarjeta";
                    Bs.Text = "Monto";

                    Bs.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label22.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label41.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label45.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label28.Font = new Font("Century Gothic", 12, FontStyle.Bold);

                    //Reubicacion de los objetos 
                    btnCancelar2.Location = new Point(165, 249);
                    btnProcesar2.Location = new Point(285, 249);
                    label41.Location = new Point(32, 52);
                    CbxMetodosPago2.Location = new Point(32, 76);
                    label45.Location = new Point(219, 52);
                    CbxBanco.Location = new Point(218, 76);
                    label28.Location = new Point(32, 121);
                    txtTranferencia.Location = new Point(32, 144);
                    Bs.Location = new Point(219, 121);
                    txtMonto2Bs.Location = new Point(219, 144);
                    label49.Location = new Point(32, 184);
                    CbxPunto_Venta.Location = new Point(32, 206);

                    // Tamaño de los Objetos 
                    PnlSecundario.Size = new Size(424, 299);
                    label2.Size = new Size(423, 35);
                    CbxMetodosPago2.Size = new Size(168, 25);
                    CbxBanco.Size = new Size(168, 25);
                    txtTranferencia.Size = new Size(168, 25);
                    CbxPunto_Venta.Size = new Size(168, 25);
                    txtMonto2Bs.Size = new Size(168, 25);
                }


                if (CbxMetodosPago.Text == "Cashea")
                {
                    bool Cashea = _L_Facturacion.Verificar_Pago_CACHEA(DgvAbonos);
                    if (Cashea == false)
                    {

                        _L_Facturacion.LLenarComboboxBancos2(CbxBanco, false);
                        CbxMetodosPago2.SelectedIndex = 0;
                        CbxMoneda.SelectedIndex = 2;
                        BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);
                        //-----------ConvertirBolivares---------------------------
                        //txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);
                        BolivaresConveridos(Convert.ToDouble(txtMontoBs.Text.Replace(".", "")), txtMonto2Bs);
                        //txtMonto2Bs.Text = Convert.ToString(Convert.ToDouble(txtMontoBs.Text.Replace(".","")));

                        txtRef.Enabled = false;
                        txtRef.Visible = false;
                        txtMontoBs.Enabled = false;
                        VisualizarPanel("MostrarPanelSecundario");
                        label28.Visible = true;
                        label22.Visible = false;
                        label4.Visible = false;
                        label27.Visible = false;
                        label26.Visible = false;
                        txtCVC.Enabled = false;
                        txtCVC.Visible = false;
                        CbxTarjeta.Visible = false;
                        CbxTarjeta.Enabled = false;
                        txtVence.Enabled = false;
                        txtVence.Visible = false;
                        label24.Visible = false;
                        txtCheque.Enabled = false;
                        txtCheque.Visible = false;
                        label15.Visible = true;
                        CbxMoneda.Enabled = false;
                        label5.Visible = false;
                        CbxMoneda.Visible = false;
                        label42.Visible = false;
                        txtIGTF.Visible = false;
                        label21.Visible = false;
                        DtpFecha.Visible = false;
                        DtpFecha.Text = DateTime.UtcNow.ToShortDateString();
                        label6.Visible = false;
                        TxtVuelto.Visible = false;
                        CbxMetodosPago2.Visible = true;
                        label41.Visible = true;
                        _L_Facturacion.LLenarComboboxTipoPunto(CbxPunto_Venta, "TD");
                        label49.Visible = false;
                        CbxPunto_Venta.Visible = false;
                        LblMontoBillete.Visible = false;
                        CbxBillete.Visible = false;
                        LblCodBillete.Visible = false;
                        TxtCodBillete.Visible = false;
                        DgvBilletes.Visible = false;
                        LblBancoRecep.Visible = false;
                        CbxBancoRecp.Visible = false;
                        CbxBanco.Visible = false;
                        txtMonto2Bs.Enabled = true;
                        label45.Visible = false;
                        CbxBanco.Enabled = false;
                        LbePagoMovil.Visible = false;

                        label28.Text = "N° Tarjeta";
                        CbxBanco.SelectedIndex = 30;
                        Bs.Text = "Monto";
                        label28.Text = "Referencia";

                        Bs.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                        label28.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                        label41.Font = new Font("Century Gothic", 12, FontStyle.Bold);

                        //Reubicacion de los objetos 
                        btnCancelar2.Location = new Point(341, 138);
                        btnProcesar2.Location = new Point(461, 138);
                        label41.Location = new Point(33, 60);
                        CbxMetodosPago2.Location = new Point(33, 82);
                        Bs.Location = new Point(216, 60);
                        txtMonto2Bs.Location = new Point(216, 84);
                        label28.Location = new Point(400, 60);
                        txtTranferencia.Location = new Point(400, 83);


                        // Tamaño de los Objetos 
                        PnlSecundario.Size = new Size(602, 189);
                        label2.Size = new Size(601, 35);
                        txtMonto2Bs.Size = new Size(168, 25);
                        txtTranferencia.Size = new Size(168, 25);

                    }

                    else
                    {
                        if (_L_Facturacion.stringBuilder.ToString().Length > 2)
                        {
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje(_L_Facturacion.stringBuilder.ToString());
                            _FrmMensajes.ShowDialog();
                        }

                        VisualizarPanel("MostrarPanelPrincipal");
                        LimpiarTxbox();
                    }

                }

                if (CbxMetodosPago.Text == "Tarjeta de Credito")
                {
                    _L_Facturacion.LLenarComboboxBancos(CbxBanco, false);
                    CbxMetodosPago2.SelectedIndex = CbxMetodosPago.SelectedIndex;
                    CbxMoneda.SelectedIndex = 2;
                    txtMonto2Bs.Enabled = true;
                    BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);
                    //-----------ConvertirBolivares---------------------------
                    //txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);
                    BolivaresConveridos(Convert.ToDouble(txtMontoBs.Text.Replace(".", "")), txtMonto2Bs);
                    //txtMonto2Bs.Text = Convert.ToString(Convert.ToDouble(txtMontoBs.Text.Replace(".", "")));
                    txtRef.Visible = false;
                    txtRef.Enabled = false; ;
                    txtMontoBs.Enabled = false;
                    VisualizarPanel("MostrarPanelSecundario");
                    label28.Visible = true;
                    label22.Visible = false;
                    label4.Visible = false;
                    label27.Visible = true;
                    label26.Visible = true;
                    txtCVC.Enabled = true;
                    txtCVC.Visible = true;
                    CbxTarjeta.Visible = true;
                    CbxTarjeta.Enabled = true;
                    txtVence.Enabled = true;
                    txtVence.Visible = true;
                    label24.Visible = false;
                    txtCheque.Enabled = false;
                    txtCheque.Visible = false;
                    label15.Visible = true;
                    label5.Visible = false;
                    CbxMoneda.Enabled = false;
                    CbxMoneda.Visible = false;
                    label42.Visible = false;
                    txtIGTF.Visible = false;
                    label21.Visible = false;
                    DtpFecha.Visible = false;
                    label6.Visible = false;
                    TxtVuelto.Visible = false;
                    CbxMetodosPago2.Visible = true;
                    label41.Visible = true;
                    _L_Facturacion.LLenarComboboxTipoPunto(CbxPunto_Venta, "TC");
                    label49.Visible = true;
                    CbxPunto_Venta.Visible = true;
                    LblMontoBillete.Visible = false;
                    CbxBillete.Visible = false;
                    LblCodBillete.Visible = false;
                    TxtCodBillete.Visible = false;
                    DgvBilletes.Visible = false;
                    LblBancoRecep.Visible = false;
                    CbxBancoRecp.Visible = false;
                    LbePagoMovil.Visible = false;

                    label45.Text = "Banco";
                    label28.Text = "N° Tarjeta";
                    Bs.Text = "Monto";


                    // Tamaño de la fuente 
                    Bs.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label41.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label45.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label21.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label28.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label26.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label27.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    label49.Font = new Font("Century Gothic", 12, FontStyle.Bold);

                    //Reubicacion de los objetos

                    //label41.Location = new System.Drawing.Point(33, 53);
                    //CbxMetodosPago2.Location = new System.Drawing.Point(33, 75);
                    //label45.Location = new System.Drawing.Point(33, 118);
                    //CbxBanco.Location = new System.Drawing.Point(33, 140);
                    //label28.Location = new System.Drawing.Point(33, 183);
                    //txtTranferencia.Location = new System.Drawing.Point(33, 205);
                    //label26.Location = new System.Drawing.Point(226, 53);
                    //txtCVC.Location = new System.Drawing.Point(226, 75);
                    //label27.Location = new System.Drawing.Point(225, 118);
                    //txtVence.Location = new System.Drawing.Point(225, 140);
                    //label49.Location = new System.Drawing.Point(225, 183);
                    //CbxPunto_Venta.Location = new System.Drawing.Point(225, 205);
                    //CbxTarjeta.Location = new System.Drawing.Point(417, 75);
                    //Bs.Location = new System.Drawing.Point(417, 118);
                    //txtMonto2Bs.Location = new System.Drawing.Point(417, 140);
                    //btnCancelar2.Location = new Point(370, 255);
                    //btnProcesar2.Location = new Point(491, 255);

                    label41.Location = new System.Drawing.Point(33, 53);
                    CbxMetodosPago2.Location = new System.Drawing.Point(33, 75);
                    
                    label45.Location = new System.Drawing.Point(33, 183 );
                    CbxBanco.Location = new System.Drawing.Point(33, 205);

                    label28.Location = new System.Drawing.Point(226, 53);
                    txtTranferencia.Location = new System.Drawing.Point(226, 75);

                    label26.Location = new System.Drawing.Point(225, 118);
                    txtCVC.Location = new System.Drawing.Point(225, 140);

                    label27.Location = new System.Drawing.Point(33, 118);
                    txtVence.Location = new System.Drawing.Point(33, 140);

                    label49.Location = new System.Drawing.Point(225, 183);
                    CbxPunto_Venta.Location = new System.Drawing.Point(225, 205);
                    CbxTarjeta.Location = new System.Drawing.Point(417, 75);
                    Bs.Location = new System.Drawing.Point(417, 118);
                    txtMonto2Bs.Location = new System.Drawing.Point(417, 140);
                    btnCancelar2.Location = new Point(370, 255);
                    btnProcesar2.Location = new Point(491, 255);

                    // Tamaño de los Objetos 
                    PnlSecundario.Size = new System.Drawing.Size(618, 300);
                    label2.Size = new System.Drawing.Size(617, 35);
                    label41.Size = new System.Drawing.Size(168, 21);
                    CbxMetodosPago2.Size = new System.Drawing.Size(168, 21);
                    label45.Size = new System.Drawing.Size(168, 21);
                    CbxBanco.Size = new System.Drawing.Size(168, 21);
                    label28.Size = new System.Drawing.Size(168, 21);
                    txtTranferencia.Size = new System.Drawing.Size(168, 21);
                    label26.Size = new System.Drawing.Size(168, 21);
                    txtCVC.Size = new System.Drawing.Size(168, 21);
                    label27.Size = new System.Drawing.Size(168, 21);
                    txtVence.Size = new System.Drawing.Size(168, 21);
                    label49.Size = new System.Drawing.Size(168, 21);
                    CbxPunto_Venta.Size = new System.Drawing.Size(168, 21);
                    CbxTarjeta.Size = new System.Drawing.Size(168, 21);
                    Bs.Size = new System.Drawing.Size(168, 21);
                    txtMonto2Bs.Size = new System.Drawing.Size(168, 21);

                }


                if (CbxMetodosPago.Text == "Nota Credito")
                {
                    label5.Visible = false;
                    DataTable dt = _D_DetalleOrden.ExistenNotas(txtCedula.Text.Substring(2, txtCedula.Text.Length - 2));

                    if (dt.Rows.Count > 0)
                    {

                        bool rep = _L_Facturacion.BuscarNotasGrid(DgvAbonos);
                        if (rep == true)
                        {
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje(_L_Facturacion.stringBuilder.ToString());
                            _FrmMensajes.ShowDialog();
                            CbxMetodosPago.SelectedIndex = 2;
                        }
                        else
                        {
                            _L_Facturacion.CargarGridNotasTotales(DgvNotas, dt);
                            CrearObjetosNotasCredito();
                            VisualizarPanel("MostrarNotasCredito");
                            txtNroNota.Enabled = true;
                            //txtNroNota.Visible = true;
                            btnProcesar3.Enabled = false;
                            //txtBsNotaCredito.Enabled = false;
                            txtBsNotaCredito.Visible = true;
                            //txtNroNotaDevolucion.Visible = true;
                            txtBsNotaCredito.Enabled = false;
                        }

                    }
                    label5.Visible = false;
                    CbxMoneda.Visible = false;

                    label42.Visible = false;
                    txtIGTF.Visible = false;

                    label4.Visible = false;
                    txtRef.Visible = false;

                    // agregadp para el billete de falbod  13/06/2023
                    LblMontoBillete.Visible = false;
                    CbxBillete.Visible = false;
                    LblCodBillete.Visible = false;
                    TxtCodBillete.Visible = false;
                    DgvBilletes.Visible = false;


                    // Agregadp para el banco receptor de transferencia 22/06/2023
                    LblBancoRecep.Visible = false;
                    CbxBancoRecp.Visible = false;
                    label45.Text = "Banco";

                }


                if (CbxMetodosPago.Text == "Efectivo")
                {
                    txtMontoBs.Enabled = true;
                    label5.Visible = false;
                    CbxMoneda.Visible = false;

                    label42.Visible = false;
                    txtIGTF.Visible = false;

                    label4.Visible = false;
                    txtRef.Visible = false;

                    // agregadp para el billete de falbod  13/06/2023
                    LblMontoBillete.Visible = false;
                    CbxBillete.Visible = false;
                    LblCodBillete.Visible = false;
                    TxtCodBillete.Visible = false;
                    DgvBilletes.Visible = false;


                    // Agregadp para el banco receptor de transferencia 22/06/2023
                    LblBancoRecep.Visible = false;
                    CbxBancoRecp.Visible = false;
                }

                if (CbxMetodosPago.Text == "Iva Retenido")
                {
                    txtMontoBs.Enabled = false;
                    label5.Visible = false;
                    CbxMoneda.Visible = false;
                    label42.Visible = false;
                    txtIGTF.Visible = false;

                    label4.Visible = false;
                    txtRef.Visible = false;

                    // agregadp para el billete de falbod  13/06/2023
                    LblMontoBillete.Visible = false;
                    CbxBillete.Visible = false;
                    LblCodBillete.Visible = false;
                    TxtCodBillete.Visible = false;
                    DgvBilletes.Visible = false;

                    // Agregadp para el banco receptor de transferencia 22/06/2023
                    LblBancoRecep.Visible = false;
                    CbxBancoRecp.Visible = false;
                    label45.Text = "Banco";
                    bool Retencion_Iva = _L_Facturacion.Verificar_AgenteRetencion(DgvAbonos, txtCedula.Text.Substring(0, (txtCedula.Text.Length)-(txtCedula.Text.Length - 1)), txtCedula.Text.Substring(2,  txtCedula.Text.Length - 2), true, false);
                    if (Retencion_Iva == true)
                    {
                        Double Bolivares = 0.00;
                        Double TotalAbono = 0.00;

                        Bolivares = Math.Round(TB_CAORDSER.VtaImpuesto * 0.75, 2);
                        TotalAbono = Convert.ToDouble(_L_Facturacion.TotalizarAbono(DgvAbonos));

                        if (Bolivares <= (Math.Round((TB_CAORDSER.OrSer_Saldo + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos))) - TotalAbono, 2)))
                        {
                            if (Bolivares > 0)
                            {
                                _L_Facturacion.GuardarAbonoGrid(2,Dt_Abonos, CbxMetodosPago.Text, "Bolivares", "", Bolivares.ToString(), "", DtpFecha.Value.ToString(), CbxMetodosPago.SelectedValue.ToString(), "000");
                                BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);
                                //-----------ConvertirBolivares---------------------------
                                //txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);

                                VisualizarPanel("MostrarPanelPrincipal");
                                LimpiarTxbox();
                            }
                            else
                            {
                                _FrmMensajes.co = 2;
                                _FrmMensajes.avisomensaje("Esta orden no posee IVA");
                                _FrmMensajes.ShowDialog();
                            }
                        }
                        else
                        {
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje("Este abono es mayor al saldo de la orden");
                            _FrmMensajes.ShowDialog();
                        }
                    }

                    else
                    {
                        if (_L_Facturacion.stringBuilder.ToString().Length > 2)
                        {
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje(_L_Facturacion.stringBuilder.ToString());
                            _FrmMensajes.ShowDialog();
                        }

                        VisualizarPanel("MostrarPanelPrincipal");
                        LimpiarTxbox();
                    }
                }

                if (CbxMetodosPago.Text == "ISLR Retenido")
                {
                    bool Retencion_ISLR = _L_Facturacion.Verificar_AgenteRetencion(DgvAbonos, txtCedula.Text.Substring(0, (txtCedula.Text.Length) - (txtCedula.Text.Length - 1)), txtCedula.Text.Substring(2, txtCedula.Text.Length - 2), false, true);
                    if (Retencion_ISLR == true)
                    {
                        txtMontoBs.Enabled = true;
                        label5.Visible = false;
                        CbxMoneda.Visible = false;

                        label42.Visible = false;
                        txtIGTF.Visible = false;

                        label4.Visible = false;
                        txtRef.Visible = false;

                        // agregadp para el billete de falbod  13/06/2023
                        LblMontoBillete.Visible = false;
                        CbxBillete.Visible = false;
                        LblCodBillete.Visible = false;
                        TxtCodBillete.Visible = false;
                        DgvBilletes.Visible = false;

                        // Agregadp para el banco receptor de transferencia 22/06/2023
                        LblBancoRecep.Visible = false;
                        CbxBancoRecp.Visible = false;
                        label45.Text = "Banco";
                    }
                }

                //Comentar
                if (CbxMetodosPago.Text == "Nota Devolucion")
                {
                    label5.Visible = false;
                    DataTable dt = _D_DetalleOrden.ExistenNotasDevolucion(txtCedula.Text.Substring(2, txtCedula.Text.Length - 2));

                    if (dt.Rows.Count > 0)
                    {

                        bool rep = _L_Facturacion.BuscarNotasDevolucionGrid(DgvAbonos, TB_CAORDSER.Cod_Sucursal, txtNumeroOrden.Text, TB_CAORDSER.Revision);
                        if (rep == true)
                        {
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje(_L_Facturacion.stringBuilder.ToString());
                            _FrmMensajes.ShowDialog();
                            CbxMetodosPago.SelectedIndex = 2;
                        }
                        else
                        {
                            _L_Facturacion.CargarGridNotasTotales(DgvNotasDevolicion, dt);
                            CrearObjetosNotasDevolucion();
                            VisualizarPanel("MostrarNotasDevolucion");
                            //txtNroNotaDevolucion.Enabled = true;
                            //txtNroNotaDevolucion.Visible = true;
                            btnProcesar4.Enabled = false;
                            txtBsNotaDevolucion.Enabled = false;
                        }

                    }
                    label5.Visible = false;
                    CbxMoneda.Visible = false;

                    label42.Visible = false;
                    txtIGTF.Visible = false;

                    label4.Visible = false;
                    txtRef.Visible = false;


                    // agregadp para el billete de falbod  13/06/2023
                    LblMontoBillete.Visible = false;
                    CbxBillete.Visible = false;
                    LblCodBillete.Visible = false;
                    TxtCodBillete.Visible = false;
                    DgvBilletes.Visible = false;

                    // Agregadp para el banco receptor de transferencia 22/06/2023
                    LblBancoRecep.Visible = false;
                    CbxBancoRecp.Visible = false;
                    label45.Text = "Banco";
                }
            }

        }

        private void FormatoDataGrid()
        {

            try
            {

                //Centrar todas las colucnas 
                DgvAbonos.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //asignar Nombres a cada colucna 
                DgvAbonos.Columns["TipoPago"].HeaderText = "Tipo pago";
                DgvAbonos.Columns["Moneda"].HeaderText = "Moneda";
                DgvAbonos.Columns["Ref"].HeaderText = "Ref";
                DgvAbonos.Columns["Banco"].HeaderText = "Banco";
                DgvAbonos.Columns["Igtf"].HeaderText = "Igtf";
                DgvAbonos.Columns["Bs"].HeaderText = "Bolivares";
                DgvAbonos.Columns["N°Tranferencia"].HeaderText = "N°Tranferencia";
                DgvAbonos.Columns["Fecha"].HeaderText = "Fecha";
                DgvAbonos.Columns["CodPago"].HeaderText = "CPAGO";
                DgvAbonos.Columns["CodBanco"].HeaderText = "CBANCO";
                DgvAbonos.Columns["Vuelto"].HeaderText = "Vuelto";

                DgvAbonos.Columns["CVC"].HeaderText = "CVC";
                DgvAbonos.Columns["Vence"].HeaderText = "Vence";
                DgvAbonos.Columns["TipoTrajeta"].HeaderText = "TipoTrajeta";
                DgvAbonos.Columns["Abo_CVCNROCHEQUE"].HeaderText = "N° nota";
                DgvAbonos.Columns["Tipo_Punto"].HeaderText = "Tipo_Punto";

                //Ancho de columna
                DgvAbonos.Columns["TipoPago"].Width = 145;
                DgvAbonos.Columns["Moneda"].Width = 85;
                DgvAbonos.Columns["Ref"].Width = 50;
                DgvAbonos.Columns["Banco"].Width = 50;
                DgvAbonos.Columns["Igtf"].Width = 50;
                DgvAbonos.Columns["Bs"].Width = 125;
                DgvAbonos.Columns["N°Tranferencia"].Width = 50;
                DgvAbonos.Columns["Fecha"].Width = 50;
                DgvAbonos.Columns["Vuelto"].Width = 50;
                DgvAbonos.Columns["Tipo_Punto"].Width = 50;
                //Bloquear Columna 

                
                DgvAbonos.Columns["TipoPago"].ReadOnly = true;
                DgvAbonos.Columns["Moneda"].ReadOnly = true;
                DgvAbonos.Columns["Ref"].ReadOnly = true;
                DgvAbonos.Columns["Banco"].ReadOnly = true;
                DgvAbonos.Columns["Igtf"].ReadOnly = true;
                DgvAbonos.Columns["Bs"].ReadOnly = true;
                DgvAbonos.Columns["N°Tranferencia"].ReadOnly = true;
                DgvAbonos.Columns["Fecha"].ReadOnly = true;
                DgvAbonos.Columns["CodPago"].ReadOnly = true;
                DgvAbonos.Columns["CodBanco"].ReadOnly = true;
                DgvAbonos.Columns["Vuelto"].ReadOnly = true;
                DgvAbonos.Columns["CVC"].ReadOnly = true;
                DgvAbonos.Columns["Vence"].ReadOnly = true;
                DgvAbonos.Columns["TipoTrajeta"].ReadOnly = true;
                DgvAbonos.Columns["Abo_CVCNROCHEQUE"].ReadOnly = true;
                DgvAbonos.Columns["Tipo_Punto"].ReadOnly = true;

                DgvAbonos.Columns["IdAbono"].Visible = false;
                DgvAbonos.Columns["TipoPago"].Visible = true;
                DgvAbonos.Columns["Moneda"].Visible = true;
                DgvAbonos.Columns["Ref"].Visible = false;
                DgvAbonos.Columns["Banco"].Visible = false;
                DgvAbonos.Columns["Igtf"].Visible = false;
                DgvAbonos.Columns["Bs"].Visible = true;
                DgvAbonos.Columns["N°Tranferencia"].Visible = false;
                DgvAbonos.Columns["Fecha"].Visible = false;
                DgvAbonos.Columns["CodPago"].Visible = false;
                DgvAbonos.Columns["CodBanco"].Visible = false;
                DgvAbonos.Columns["Vuelto"].Visible = false;
                DgvAbonos.Columns["CVC"].Visible = false;
                DgvAbonos.Columns["Vence"].Visible = false;
                DgvAbonos.Columns["TipoTrajeta"].Visible = false;
                DgvAbonos.Columns["Abo_CVCNROCHEQUE"].Visible = false;
                DgvAbonos.Columns["Tipo_Punto"].Visible = false;

            }


            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }
        }



        private void FormatoDataGridPagosRealizados()
        {

            try
            {

                //Centrar todas las colucnas 
                //DgvListadoOrdenes.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //asignar Nombres a cada colucna 
                DgvListadoOrdenes.Columns["Fecha"].HeaderText = "   Fecha";
                DgvListadoOrdenes.Columns["Abo_Tipo"].HeaderText = "      Método de pago";
                DgvListadoOrdenes.Columns["Abo_Monto"].HeaderText = "     Monto";
                DgvListadoOrdenes.Columns["Tipo_Pago"].HeaderText = "Tipo_Pago";
                DgvListadoOrdenes.Columns["Fec_Crea"].HeaderText = "Fec_Crea";
                DgvListadoOrdenes.Columns["ID_Abono"].HeaderText = "ID_Abono";

                //Ancho de columna
                DgvListadoOrdenes.Columns["Fecha"].Width = 315;
                DgvListadoOrdenes.Columns["Abo_Tipo"].Width = 350;
                DgvListadoOrdenes.Columns["Abo_Monto"].Width = 315;
                DgvListadoOrdenes.Columns["Eliminar"].Width = 125;


                //Bloquear Columna 
                DgvListadoOrdenes.Columns["Fecha"].ReadOnly = true;
                DgvListadoOrdenes.Columns["Abo_Tipo"].ReadOnly = true;
                DgvListadoOrdenes.Columns["Abo_Monto"].ReadOnly = true;

                //Posicion  
                DgvListadoOrdenes.Columns["Fecha"].DisplayIndex = 0;
                DgvListadoOrdenes.Columns["Abo_Tipo"].DisplayIndex = 1;
                DgvListadoOrdenes.Columns["Abo_Monto"].DisplayIndex = 2;
                DgvListadoOrdenes.Columns["Eliminar"].DisplayIndex = 3;
                DgvListadoOrdenes.Columns["Tipo_Pago"].DisplayIndex = 4;
                DgvListadoOrdenes.Columns["Fec_Crea"].DisplayIndex = 5;
                DgvListadoOrdenes.Columns["ID_Abono"].DisplayIndex = 6;

                //Alineación

                DgvListadoOrdenes.Columns["Fecha"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvListadoOrdenes.Columns["Abo_Tipo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvListadoOrdenes.Columns["Abo_Monto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvListadoOrdenes.Columns["Tipo_Pago"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvListadoOrdenes.Columns["Fec_Crea"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


                DgvListadoOrdenes.Columns["Tipo_Pago"].Visible = false;
                DgvListadoOrdenes.Columns["Fec_Crea"].Visible = false;
                DgvListadoOrdenes.Columns["ID_Abono"].Visible = false;
                DgvListadoOrdenes.Columns["Eliminar"].Visible = true;

                if (TB_CAORDSER.OrSer_Status != "005")
                {
                    DgvListadoOrdenes.Columns["Eliminar"].Visible = false;
                    DgvListadoOrdenes.Columns["Fecha"].Width = 360;
                    DgvListadoOrdenes.Columns["Abo_Tipo"].Width = 350;
                    DgvListadoOrdenes.Columns["Abo_Monto"].Width = 360;
                }


                // nuevo 21-08-2023 
                DgvListadoOrdenes.Columns["Abo_Monto"].DefaultCellStyle.Format = "##,##0.00";

            }


            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }
        }

        public void LimpiarGrid()
        {
            try
            {
                //limpiar el grid 

                DgvListadoOrdenes.DataSource = "";
                DgvListadoOrdenes.DataMember = "";

                var dataGridViewColumn2 = DgvListadoOrdenes.Columns["Eliminar"];

                if (dataGridViewColumn2 != null)

                {

                    DgvListadoOrdenes.Columns.RemoveAt(DgvListadoOrdenes.Columns.Count - 1);
                }

            }

            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }

        }

        public void CrearObjetos()
        {
            try
            {

                DataGridViewButtonColumn BtnEliminar = new DataGridViewButtonColumn();
                BtnEliminar.Name = "Eliminar";
                BtnEliminar.Width = 120;
                BtnEliminar.HeaderText = "Eliminar";
                DgvListadoOrdenes.Columns.Add(BtnEliminar);


                FormatoDataGridPagosRealizados();
            }

            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }
        }


        private void FormatoDataGridNotasCredito()
        {

            try
            {

                //Centrar todas las colucnas 
                DgvNotas.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //asignar Nombres a cada colucna 
                DgvNotas.Columns["NRONOTA"].HeaderText = "Número Nota";
                DgvNotas.Columns["Fact_Num"].HeaderText = "Número Factura ";
                DgvNotas.Columns["Motivo"].HeaderText = "Motivo";
                DgvNotas.Columns["SaldoNota"].HeaderText = "Saldo";

                //Ancho de columna

                DgvNotas.Columns["NRONOTA"].Width = 125;
                DgvNotas.Columns["Fact_Num"].Width = 150;
                DgvNotas.Columns["Motivo"].Width = 120;
                DgvNotas.Columns["SaldoNota"].Width = 90;
                DgvNotas.Columns["RdButom"].Width = 55;
                

                //Bloquear Columna 
                DgvNotas.Columns["NRONOTA"].ReadOnly = true;
                DgvNotas.Columns["Fact_Num"].ReadOnly = true;
                DgvNotas.Columns["Motivo"].ReadOnly = true;
                DgvNotas.Columns["SaldoNota"].ReadOnly = true;

                //Posicion  
                DgvNotas.Columns["NRONOTA"].DisplayIndex = 0;
                DgvNotas.Columns["Fact_Num"].DisplayIndex = 1;
                DgvNotas.Columns["Motivo"].DisplayIndex = 2;
                DgvNotas.Columns["RdButom"].DisplayIndex = 4;
                DgvNotas.Columns["SaldoNota"].DisplayIndex = 3;

                DgvNotas.Columns["NRONOTA"].Visible = true;
                DgvNotas.Columns["Fact_Num"].Visible = true;
                DgvNotas.Columns["Motivo"].Visible = false;
                DgvNotas.Columns["SaldoNota"].Visible = true;

                // 
                DgvNotas.Columns["SaldoNota"].DefaultCellStyle.Format = "##,##0.00";

            }


            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }
        }

        public void CrearObjetosNotasCredito()
        {
            try
            {

                DataGridViewCheckBoxColumn RdButom = new DataGridViewCheckBoxColumn();
                RdButom.Name = "RdButom";
                RdButom.Width = 50;
                RdButom.HeaderText = "";
                DgvNotas.Columns.Add(RdButom);

                FormatoDataGridNotasCredito();


            }

            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }
        }

        private void btnProcesar1_Validating(object sender, CancelEventArgs e)
        {
            //if (Convert.ToDouble(_L_Facturacion.) >= )
            //{

            //}
        }

        public void LimpiarNotasCredito()
        {

            DgvNotas.DataSource = "";
            DgvNotas.DataMember = "";

            var dataGridViewColumn = DgvNotas.Columns["RdButom"];

            if (dataGridViewColumn != null && dataGridViewColumn.Visible)

            {

                DgvNotas.Columns.RemoveAt(DgvNotas.Columns.Count - 1);
            }

            txtNroNota.Text = "";
            PosicionInical = "";
            txtNroNota.Enabled = true;
            rd_Nota.Enabled = true;
            rd_Nota.Checked = false;
            rd_Fact.Enabled = true;
            rd_Fact.Checked = false;
            txtBsNotaCredito.Enabled = false;
            btnProcesar3.Enabled = false;

        }


        private void btnCancelar3_Click(object sender, EventArgs e)
        {
            LimpiaVariablesIdAbonoPagoMovil();
            LimpiarNotasCredito();
            VisualizarPanel("MostrarPanelPrincipal");
            LimpiarTxbox();

        }

        private void txtNroNota_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (rd_Nota.Checked || rd_Fact.Checked)
                {
                    if (txtNroNota.Text != "")
                    {
                        _L_Facturacion.BuscarNotas(txtNroNota.Text, rd_Nota.Checked, txtCedula.Text.Substring(2, txtCedula.Text.Length - 2), DgvNotas);
                        mensaje = _L_Facturacion.stringBuilder.ToString();
                        if (DgvNotas.Rows.Count > 0)
                        {
                            CrearObjetosNotasCredito();

                        }

                        else
                        {
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje(mensaje);
                            _FrmMensajes.ShowDialog();
                        }

                    }

                    else
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("Debe ingresar un numero de 7 digitos");
                        _FrmMensajes.ShowDialog();
                    }
                }

                //else
                //{
                //    _FrmMensajes.co = 2;
                //    _FrmMensajes.avisomensaje("Selecione el tipo de documento");
                //    _FrmMensajes.ShowDialog();
                //}
            }
        }

        private void txtNroNota_KeyPress(object sender, KeyPressEventArgs e)
        {
            //para que solo acepte numeros
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }

        public string ImprimirFacturaFiscal(string NumeroOrden, string CedulaCliente, string NacionalidadRif, SqlCommand command)
        {
            string SerialImpresora = "";
            try
            {
                DataSet DtIGTF;
                NumeroComprobanteFiscal = "";
                DataSet dsArti = _D_DetalleOrden.DETALLEFACTURAFISCAL(txtNumeroOrden.Text, command);
                DataTable dt = dsArti.Tables[0];
                int totaldetalle = dsArti.Tables[0].Rows.Count;
                double TotalItems = 0;
                bool DctoFactura = false;
                string TipoTasaIVA = "1";
                string DctoExento = "";
                string DctoGravable = "";
                string FechaImpresora = "";
                string Transaccion = "";
                Double Fact_MontoExento = 0;
                Double Fact_MontoGravable = 0;
                Double iGTF = 0.00;

                foreach (DataRow drItem in dt.Rows)
                {
                    if (Convert.ToInt16(drItem["OrdServ_PorcImp"]) > 0)
                    {
                        if (Convert.ToString(drItem["OrdServ_PorcImp"]) == "PorcIvaTipoA")
                        {
                            //IVA "A";
                            TipoTasaIVA = "3";
                            break;
                        }
                        else if (Convert.ToString(drItem["OrdServ_PorcImp"]) == "PorcIvaTipoR")
                        {
                            //IVA "R";
                            TipoTasaIVA = "2";
                            break;
                        }
                        else
                        {
                            //IVA  "G";
                            TipoTasaIVA = "1";
                            break;
                        }
                    }
                    else
                        //IVA  "G";
                        TipoTasaIVA = "1";
                }

                // ----CONSULTO PAGOS EN DIVISA
                DtIGTF = _D_DetalleOrden.PagosConIGTF(TB_CAORDSER.Cod_Sucursal, txtNumeroOrden.Text, TB_CAORDSER.Revision, command);
                string Fecha2 = DateTime.Today.ToString("yyyyMMdd");

                // Se valida si es factura manual 

                if (_L_Facturacion.ValidaFactManual(command) == false)
                {
                    FacturaManual = false;
                    ImprimirFacturaFiscall = true;

                    uint resp = 0;

                    resp = objVmax.AbrirPuerto(Convert.ToString(glbPuertoCOM));

                    if (resp != 0)
                    {
                        objVmax.CerrarPuerto();
                        ImprimirFacturaFiscall = false;

                        mensaje = "No hay conexión con la impresora fiscal";
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje(mensaje);
                        _FrmMensajes.ShowDialog();
                        objVmax.Cancelar();
                        objVmax.Cerrar();
                        objVmax.CerrarPuerto();
                        return "Error";
                    }

                    else
                    {

                        objVmax.AbrirPuerto(Convert.ToString(glbPuertoCOM));
                        bool respuesta = false;
                        // ''''' ********* DATOS DEL CLIENTE ************
                        //resp = objVmax.AbrirCF(txtNombreCliente.Text, (txtCedula.Text.Replace("-","")) , "1", "294", "12345", "", "", 40);
                        resp = objVmax.AbrirCF(txtNombreCliente.Text, txtCedula.Text, "1", "294", "12345", "", "", 40);

                        if (resp != 0)
                        {
                            mensaje = "No hay conexión con la impresora fiscal";
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje(mensaje);
                            _FrmMensajes.ShowDialog();
                            ImprimirFacturaFiscall = false;
                            objVmax.Cancelar();
                            objVmax.Cerrar();
                            objVmax.CerrarPuerto();
                            return "Error";
                        }
                        else
                        {
                            objVmax.ObtenerReporteInformativo();
                            SerialImpresora = objVmax.RetornoMI.sSerial;
                            NumeroComprobanteFiscal = objVmax.RetornoAbrirFactura.uiNumeroFactura.ToString();

                        }


                        /// **********'IMPRIMO LOS ITEMS *********************

                        string desart;

                        foreach (DataRow drItem in dt.Rows)
                        {

                            // EL DESCUENTO DE LOS ARTICULOS SE ENVIARA AL FINAL, ANTES DE CERRAR EL CF

                            if (Convert.ToInt64(drItem["Ordserv_Dto"]) > 0)
                            {
                                DctoFactura = true;
                            }

                            desart = drItem["CodArticulo"] + " " + drItem["DESART"];

                            ////Valido que el texto no sea mayor a 40 caracteres para que no salga duplicado el articulo en la factura 25-05-2023
                            //desart = Validar_Cadena(desart);

                            if (Convert.ToString(drItem["ART_EXENTO"]) == "1")
                            {
                                TipoTasaIVA = "0";
                                Fact_MontoExento = Fact_MontoExento + (Convert.ToDouble(drItem["Ordserv_Bruto"]) - (Convert.ToDouble(drItem["Ordserv_Dto"]) / 100));
                                
                            }
                            else
                            {
                                TipoTasaIVA = "1";
                                Fact_MontoGravable = Fact_MontoGravable + (Convert.ToDouble(drItem["Ordserv_Bruto"]) - (Convert.ToDouble(drItem["Ordserv_Dto"]) / 100));
                            }


                            resp = objVmax.Item(desart, Convert.ToString(Convert.ToDouble(drItem["Ordserv_Cant"]) * 1000), drItem["OrdServ_Precio"].ToString(), TipoTasaIVA, "1", 40);

                            TotalItems = TotalItems + ((Convert.ToDouble(drItem["OrdServ_Precio"]) * Convert.ToInt32(drItem["Ordserv_Cant"]))) + Convert.ToDouble(drItem["OrdServ_Impuesto"]);

                        }


                        Double totalpagos = 0;
                        bool sumo01 = false;


                        // ************SUB TOTAL DEL CF ********************
                        // Si la impresora devuelve true imprimo  subtotal
                        if (resp == 0)
                        {

                            // OBTENGO LA SUMATORIA DE LOS ABONOS PARA SABER SI COINCIDEN CON EL TOTAL DE LOS ARTICULOS  PARA CANCELAR EL CF ANTES DE ENVIAR EL SUBTOTAL. 
                            DataTable dtPago = _D_DetalleOrden.TEMP_ABONO(TB_CAORDSER.Cod_Sucursal, txtNumeroOrden.Text, TB_CAORDSER.Revision, command);

                            foreach (DataRow drPago in dtPago.Rows)
                            {
                                totalpagos = totalpagos + (Convert.ToDouble(drPago["Abo_Monto"].ToString().Replace(",", "")));
                            }

                            // ----CONSULTO PAGOS EN DIVISA
                            foreach (DataRow drIgtf in DtIGTF.Tables[0].Rows)
                            {
                                //if (DtIGTF.Tables[0].Rows[0]["Abo_Monto"].ToString() != "0" & Convert.ToBoolean(DtIGTF.Tables[0].Rows[0]["ActivaIGTF"]) == true)
                                if (DtIGTF.Tables[0].Rows[0]["Abo_Monto"].ToString() != "0")

                                {
                                    TotalItems = TotalItems + Convert.ToDouble(drIgtf["Abo_IGTF"]);
                                }

                            }

                            // ----CONSULTO LOS DESCUENTOS DE LA FACTURA 
                            
                            DataSet dsDcto = _D_DetalleOrden.DESCUENTOSFACTURAFISCAL(txtNumeroOrden.Text, command);
                            DataTable dtcto = dsDcto.Tables[0];
                            Double DescuentoExento = Math.Round(Convert.ToDouble(dtcto.Rows[0]["DescuentoExento"].ToString()), 2);
                            Double DescuentoGravable = Math.Round(Convert.ToDouble(dtcto.Rows[0]["DescuentoGravable"].ToString()), 2);
                            Double DescuentoGravableA = Math.Round(Convert.ToDouble(dtcto.Rows[0]["DescuentoGravableA"].ToString()), 2);
                            Double DescuentoGravableR = Math.Round(Convert.ToDouble(dtcto.Rows[0]["DescuentoGravableR"].ToString()), 2);

                            if (totalpagos != (TotalItems - DescuentoExento - DescuentoGravable - DescuentoGravableA - DescuentoGravableR))
                            {
                                if (((TotalItems - DescuentoExento - DescuentoGravable - DescuentoGravableA - DescuentoGravableR) - totalpagos == 001 |
                                    totalpagos - (TotalItems - DescuentoExento - DescuentoGravable - DescuentoGravableA - DescuentoGravableR) == 001) |
                                    ((TotalItems - DescuentoExento - DescuentoGravable - DescuentoGravableA - DescuentoGravableR) - totalpagos == 002 |
                                    totalpagos - (TotalItems - DescuentoExento - DescuentoGravable - DescuentoGravableA - DescuentoGravableR) == 002) |
                                    ((TotalItems - DescuentoExento - DescuentoGravable - DescuentoGravableA - DescuentoGravableR) - totalpagos == 003 |
                                    totalpagos - (TotalItems - DescuentoExento - DescuentoGravable - DescuentoGravableA - DescuentoGravableR) == 003) |
                                    ((TotalItems - DescuentoExento - DescuentoGravable - DescuentoGravableA - DescuentoGravableR) - totalpagos == 004 |
                                    totalpagos - (TotalItems - DescuentoExento - DescuentoGravable - DescuentoGravableA - DescuentoGravableR) == 004) |
                                    ((TotalItems - DescuentoExento - DescuentoGravable - DescuentoGravableA - DescuentoGravableR) - totalpagos == 005 |
                                    totalpagos - (TotalItems - DescuentoExento - DescuentoGravable - DescuentoGravableA - DescuentoGravableR) == 005))

                                {
                                    sumo01 = true;
                                }

                                else
                                {

                                    // SI NO COINCIDEN, ANULO EL TICKET
                                    objVmax.Cancelar();
                                    objVmax.Cerrar();
                                    objVmax.CerrarPuerto();
                                    ImprimirFacturaFiscall = false;
                                    respuesta = false;
                                    return "Error";


                                }
                            }

                            else
                            {

                                // ****ENVIO LOS DESCUENTOS DE ESTA FACTURA******
                                if (DctoFactura == true)
                                {
                                    foreach (DataRow drItem in dt.Rows)
                                    {
                                        if (Convert.ToInt16(drItem["OrdServ_PorcImp"]) > 0)
                                        {
                                            if (Convert.ToString(drItem["OrdServ_PorcImp"]) == "PorcIvaTipoA")
                                            {
                                                //IVA "A";
                                                TipoTasaIVA = "3";
                                                break;
                                            }
                                            else if (Convert.ToString(drItem["OrdServ_PorcImp"]) == "PorcIvaTipoR")
                                            {
                                                //IVA "R";
                                                TipoTasaIVA = "2";
                                                break;
                                            }
                                            else
                                            {
                                                //IVA  "G";
                                                TipoTasaIVA = "1";
                                                break;
                                            }
                                        }
                                        else
                                            //IVA  "G";
                                            TipoTasaIVA = "1";
                                    }

                                    foreach (DataRow drItemdcto in dtcto.Rows)
                                    {
                                        if (TipoTasaIVA == "1")

                                            resp = objVmax.DescuentoCF("Descuento", drItemdcto["DescuentoExento"].ToString(), drItemdcto["DescuentoGravable"].ToString(), "", "", "");

                                        if (TipoTasaIVA == "3")

                                            resp = objVmax.DescuentoCF("Descuento", drItemdcto["DescuentoExento"].ToString(), "", "", drItemdcto["DescuentoGravableA"].ToString(), "");


                                        if (TipoTasaIVA == "2")

                                            resp = objVmax.DescuentoCF("Descuento", drItemdcto["DescuentoExento"].ToString(), "", drItemdcto["DescuentoGravableR"].ToString(), "", "");

                                        DctoExento = drItemdcto["DescuentoExento"].ToString();
                                        DctoGravable = drItemdcto["DescuentoGravable"].ToString() + drItemdcto["DescuentoGravableA"].ToString() + drItemdcto["DescuentoGravableR"].ToString();
                                    }
                                }

                                Double subtotal = 0.00;
                                Double subtotalIgtf = 0.00;

                                // ******ENVIO EL SUBTOTAL DE LA FACTURA**************
                                if (resp == 0)
                                {

                                    foreach (DataRow drIgtf in DtIGTF.Tables[0].Rows)
                                    {

                                        subtotal = subtotal + Convert.ToDouble(drIgtf["Abo_Monto"].ToString()) / 100;
                                        subtotalIgtf = subtotalIgtf + Convert.ToDouble(drIgtf["IGTFCALC1"].ToString());
                                    }

                                    // Verifico si la orden tiene Igtf
                                    if (Convert.ToBoolean(DtIGTF.Tables[0].Rows[0]["ActivaIGTF"].ToString()) == true & DtIGTF.Tables[0].Rows[0]["Abo_Monto"].ToString() != "0")
                                    {

                                        if (DtIGTF.Tables[0].Rows[0]["IGTFCAORDSER"].ToString().Replace(",", "") == (subtotalIgtf * 100).ToString().Replace(",", ""))
                                        {
                                            resp = objVmax.SubtotalT_sinRetorno(Convert.ToString(subtotal * 100).Replace(".", ","));
                                        }
                                        else
                                            respuesta = false;
                                    }

                                    else
                                    {
                                        resp = objVmax.Subtotal();
                                        //Modificarrrr
                                        TotalFacturaFiscal = objVmax.RetornoSubtotal.llSubtotal.ToString();
                                        NumeroComprobanteFiscal = objVmax.RetornoAbrirFactura.uiNumeroFactura.ToString();

                                    }

                                }
                                else
                                {
                                    objVmax.Cancelar();
                                    objVmax.Cerrar();
                                    objVmax.CerrarPuerto();
                                    ImprimirFacturaFiscall = false;
                                    respuesta = false;
                                    return "Error";

                                }
                            }

                            if (sumo01 == true)
                            {

                                // ****ENVIO LOS DESCUENTOS DE ESTA FACTURA******
                                if (DctoFactura == true)
                                {
                                    foreach (DataRow drItem in dt.Rows)
                                    {
                                        if (Convert.ToInt16(drItem["OrdServ_PorcImp"]) > 0)
                                        {
                                            if (Convert.ToString(drItem["OrdServ_PorcImp"]) == "PorcIvaTipoA")
                                            {
                                                //IVA "A";
                                                TipoTasaIVA = "3";
                                                break;
                                            }
                                            else if (Convert.ToString(drItem["OrdServ_PorcImp"]) == "PorcIvaTipoR")
                                            {
                                                //IVA "R";
                                                TipoTasaIVA = "2";
                                                break;
                                            }
                                            else
                                            {
                                                //IVA  "G";
                                                TipoTasaIVA = "1";
                                                break;
                                            }
                                        }
                                        else
                                            //IVA  "G";
                                            TipoTasaIVA = "1";
                                    }

                                    foreach (DataRow drItemdcto in dtcto.Rows)
                                    {
                                        if (TipoTasaIVA == "1")

                                            resp = objVmax.DescuentoCF("Descuento", drItemdcto["DescuentoExento"].ToString(), drItemdcto["DescuentoGravable"].ToString(), "", "", "");

                                        if (TipoTasaIVA == "3")

                                            resp = objVmax.DescuentoCF("Descuento", drItemdcto["DescuentoExento"].ToString(), "", "", drItemdcto["DescuentoGravableA"].ToString(), "");


                                        if (TipoTasaIVA == "2")

                                            resp = objVmax.DescuentoCF("Descuento", drItemdcto["DescuentoExento"].ToString(), "", drItemdcto["DescuentoGravableR"].ToString(), "", "");

                                        DctoExento = drItemdcto["DescuentoExento"].ToString();
                                        DctoGravable = drItemdcto["DescuentoGravable"].ToString() + drItemdcto["DescuentoGravableA"].ToString() + drItemdcto["DescuentoGravableR"].ToString();

                                    }
                                }




                                // ******ENVIO EL SUBTOTAL DE LA FACTURA**************
                                if (resp == 0)
                                {
   
                                    Double subtotal = 0.00;
                                    Double subtotalIgtf = 0.00;

                                    foreach (DataRow drIgtf in DtIGTF.Tables[0].Rows)
                                    {

                                        subtotal = subtotal + Convert.ToDouble(drIgtf["Abo_Monto"].ToString()) / 100;
                                        subtotalIgtf = subtotalIgtf + Convert.ToDouble(drIgtf["IGTFCALC1"].ToString());
                                    }

                                    // Verifico si la orden tiene Igtf
                                    if (Convert.ToBoolean(DtIGTF.Tables[0].Rows[0]["ActivaIGTF"].ToString()) == true & DtIGTF.Tables[0].Rows[0]["Abo_Monto"].ToString() != "0")
                                    {

                                        if (DtIGTF.Tables[0].Rows[0]["IGTFCAORDSER"].ToString().Replace(",", "") == (subtotalIgtf * 100).ToString().Replace(",", ""))
                                        {
                                            resp = objVmax.SubtotalT_sinRetorno(Convert.ToString(subtotal * 100).Replace(".", ","));
                                        }
                                        else
                                            respuesta = false;
                                    }

                                    else
                                    {
                                        resp = objVmax.Subtotal();
                                        //Modificarrrr
                                        TotalFacturaFiscal = objVmax.RetornoSubtotal.llSubtotal.ToString();
                                        NumeroComprobanteFiscal = objVmax.RetornoAbrirFactura.uiNumeroFactura.ToString();

                                    }

                                }
                                else
                                {
                                    objVmax.Cancelar();
                                    objVmax.Cerrar();
                                    objVmax.CerrarPuerto();
                                    ImprimirFacturaFiscall = false;
                                    respuesta = false;
                                    return "Error";

                                }

                            }
                        }

                        else
                        {
                            objVmax.Cancelar();
                            objVmax.Cerrar();
                            objVmax.CerrarPuerto();
                            ImprimirFacturaFiscall = false;
                            respuesta = false;
                            return "Error";

                        }

                        // *************ENVIO LOS PAGOS***********************
                        if (resp == 0)
                        {
                            DataTable dtPago = _D_DetalleOrden.Pasgos(TB_CAORDSER.Cod_Sucursal, txtNumeroOrden.Text, TB_CAORDSER.Revision, command);
                            TotalFacturaFiscal = objVmax.RetornoSubtotal.llSubtotal.ToString(); 
                            decimal PagosEnviados = 0;
                            foreach (DataRow drPago in dtPago.Rows)
                            {
                                if (Convert.ToDecimal(drPago["Abo_Monto"].ToString()) > 0)
                                {
                                    int NumeroMaximoCaracteres = Convert.ToInt32(drPago["Abo_Tipo"].ToString().Length); 
                                    if(Convert.ToDecimal(drPago["Abo_Monto_RecibidoREF"].ToString()) == 0)
                                    {
                                        resp = objVmax.PagoCF(drPago["Abo_Monto"].ToString(), drPago["Abo_Tipo"].ToString(), 1);
                                    }
                                    else
                                    {
                                        resp = objVmax.PagoCF(drPago["Abo_Monto_RecibidoREF"].ToString(), drPago["Abo_Tipo"].ToString(), 1);
                                    }
                                    PagosEnviados = PagosEnviados + Convert.ToDecimal(drPago["Abo_Monto"].ToString());
                                }
                            }

                            TotalFacturaFiscal = objVmax.RetornoSubtotal.llSubtotal.ToString();                      
                            decimal Calculo = Convert.ToDecimal(TotalFacturaFiscal) - PagosEnviados;

                            if (Convert.ToString(Convert.ToDecimal(TotalFacturaFiscal) - PagosEnviados) == "1")
                            {
                             
                                resp = objVmax.PagoCF("1", "EFECTIVO", 1);
                                _D_DetalleOrden.SP_SUMOFACTURA(TB_CAORDSER.Cod_Sucursal, NumeroComprobanteFiscal, SerialImpresora, txtNumeroOrden.Text, TotalFacturaFiscal, DctoExento, DctoGravable, command);
                            }
                            else if (Convert.ToString(Convert.ToDecimal(TotalFacturaFiscal) - PagosEnviados) == "2")
                            {
                               
                                resp = objVmax.PagoCF("2", "EFECTIVO", 1);
                                _D_DetalleOrden.SP_SUMOFACTURA(TB_CAORDSER.Cod_Sucursal, NumeroComprobanteFiscal, SerialImpresora, txtNumeroOrden.Text, TotalFacturaFiscal, DctoExento, DctoGravable, command);
                            }
                            else if (Convert.ToString(Convert.ToDecimal(TotalFacturaFiscal) - PagosEnviados) == "3")
                            {
                              
                                resp = objVmax.PagoCF("3", "EFECTIVO", 1);
                                _D_DetalleOrden.SP_SUMOFACTURA(TB_CAORDSER.Cod_Sucursal, NumeroComprobanteFiscal, SerialImpresora, txtNumeroOrden.Text, TotalFacturaFiscal, DctoExento, DctoGravable, command);
                            }
                            else if (Convert.ToString(Convert.ToDecimal(TotalFacturaFiscal) - PagosEnviados) == "4")
                            {
                                
                                resp = objVmax.PagoCF("4", "EFECTIVO", 1);
                                _D_DetalleOrden.SP_SUMOFACTURA(TB_CAORDSER.Cod_Sucursal, NumeroComprobanteFiscal, SerialImpresora , txtNumeroOrden.Text, TotalFacturaFiscal, DctoExento, DctoGravable, command);
                            }
                            else if (Convert.ToString(Convert.ToDecimal(TotalFacturaFiscal) - PagosEnviados) == "5")
                            {
                                
                                resp = objVmax.PagoCF("5", "EFECTIVO", 1);
                                _D_DetalleOrden.SP_SUMOFACTURA(TB_CAORDSER.Cod_Sucursal, NumeroComprobanteFiscal, SerialImpresora, txtNumeroOrden.Text, TotalFacturaFiscal, DctoExento, DctoGravable, command);

                            }

                        }
                        else
                        {
                            objVmax.Cancelar();
                            objVmax.Cerrar();
                            objVmax.CerrarPuerto();
                            ImprimirFacturaFiscall = false;
                            respuesta = false;
                            return "Error";

                        }


                        if (resp == 0)
                        {
                            

                         


                            if ((_D_DetalleOrden.TB_PARAMETROSPGE("FactFiscalconRxPGE")) == "1")
                            {

                                if (TB_CAORDSER.Asegurada == true)
                                {
                                    double ESFD = 0.00;
                                    double ESFI = 0.00;
                                    double CILD = 0.00;
                                    double CILI = 0.00;
                                    int EJED = 0;
                                    int EJEI = 0;
                                    double ADDD = 0.00;
                                    double ADDI = 0.00;
                                    // 2da Refraccion
                                    double ESFD2 = 0.00;
                                    double ESFI2 = 0.00;
                                    double CILD2 = 0.00;
                                    double CILI2 = 0.00;
                                    int EJED2 = 0;
                                    int EJEI2 = 0;

                                    // Obtengo el Examen asociado a esta orden
                                    DataSet DtsExamen = _D_DetalleOrden.OptenerExamen(TB_CAORDSER.CTE_Nacio, TB_CAORDSER.CTE_CedIden, Convert.ToInt32(TB_CAORDSER.NumExamen), TB_CAORDSER.Cod_Sucursal, command);

                                    if (DtsExamen.Tables[0].Rows.Count > 0)
                                    {

                                        foreach (DataRow row in DtsExamen.Tables[0].Rows)
                                        {
                                            ESFD = row["ESFD"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["ESFD"].ToString());
                                            ESFI = row["ESFI"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["ESFI"].ToString());
                                            CILD = row["CILD"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["CILD"].ToString());
                                            CILI = row["CILI"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["CILI"].ToString());
                                            EJED = row["EJED"] == DBNull.Value ? (int)0 : Convert.ToInt32(row["EJED"].ToString());
                                            EJEI = row["EJEI"] == DBNull.Value ? (int)0 : Convert.ToInt32(row["EJEI"].ToString());
                                            ADDD = row["ADDD"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["ADDD"].ToString());
                                            ADDI = row["ADDI"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["ADDI"].ToString());

                                            // 2da Refraccion
                                            ESFD2 = row["ESFD2"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["ESFD2"].ToString());
                                            ESFI2 = row["ESFI2"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["ESFI2"].ToString());
                                            CILD2 = row["CILD2"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["CILD2"].ToString());
                                            CILI2 = row["CILI2"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["CILI2"].ToString());
                                            EJED2 = row["EJED2"] == DBNull.Value ? (int)0 : Convert.ToInt32(row["EJED2"].ToString());
                                            EJEI2 = row["EJEI2"] == DBNull.Value ? (int)0 : Convert.ToInt32(row["EJEI2"].ToString());
                                        }

                                        if (CILD > 0.00)
                                        {
                                            var Formula = _L_Facturacion.Transposicion(ESFD, CILD, EJED);
                                            ESFD = Formula.TransESF;
                                            CILD = Formula.TransCIL;
                                            EJED = Formula.TransEJE;
                                        }
                                        if (CILI > 0)
                                        {
                                            var Formula = _L_Facturacion.Transposicion(ESFI, CILI, EJEI);
                                            ESFI = Formula.TransESF;
                                            CILI = Formula.TransCIL;
                                            EJEI = Formula.TransEJE;
                                        }

                                        // 2da Refraccion
                                        if (CILD2 > 0)
                                        {
                                            var Formula = _L_Facturacion.Transposicion(ESFD2, CILD2, EJED2);
                                            ESFD2 = Formula.TransESF;
                                            CILD2 = Formula.TransCIL;
                                            EJED2 = Formula.TransEJE;
                                        }
                                        if (CILI2 > 0)
                                        {
                                            var Formula = _L_Facturacion.Transposicion(ESFI2, CILI2, EJEI2);
                                            ESFI2 = Formula.TransESF;
                                            CILI2 = Formula.TransCIL;
                                            EJEI2 = Formula.TransEJE;
                                        }

                                        resp = objVmax.TextoNoFiscal("*Contrato de Garantia Extendida para");
                                        resp = objVmax.TextoNoFiscal("      Cristales Formulados:");
                                        resp = objVmax.TextoNoFiscal(" ");

                                        resp = objVmax.TextoNoFiscal("OD: ESF " + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", ESFD).Replace(".", ",") + " EJE " + EJED.ToString().Replace(".", ",") + " CIL " + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", CILD).Replace(".", ",") + " ADD " + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", ADDD).Replace(".", ","));

                                        // 2da Refraccion
                                        if (ESFD2 != 0)
                                        {
                                            resp = objVmax.TextoNoFiscal("2da. Refracción");
                                            resp = objVmax.TextoNoFiscal("OD: ESF " + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", ESFD2).Replace(".", ",") + " EJE " + EJED2.ToString().Replace(".", ",") + " CIL " + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", CILD2).Replace(".", ","));
                                        }

                                        resp = objVmax.TextoNoFiscal("OI: ESF " + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", ESFI).Replace(".", ",") + " EJE " + EJEI.ToString().Replace(".", ",") + " CIL " + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", CILI).Replace(".", ",") + " ADD " + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", ADDI).Replace(".", ","));
                                        // 2da Refraccion
                                        if (ESFI2 != 0)
                                        {
                                            resp = objVmax.TextoNoFiscal("2da. Refracción");
                                            resp = objVmax.TextoNoFiscal("OI: ESF " + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", ESFI2).Replace(".", ",") + " EJE " + EJEI2.ToString().Replace(".", ",") + " CIL " + String.Format(CultureInfo.InvariantCulture, "{0:0.00}", CILI2).Replace(".", ","));
                                        }

                                        resp = objVmax.TextoNoFiscal(" ");
                                        resp = objVmax.TextoNoFiscal("Condiciones legales en el Contrato");
                                        resp = objVmax.TextoNoFiscal("Suscrito.");
                                        resp = objVmax.TextoNoFiscal(" ");
                                    }

                                }


                                


                                objVmax.ObtenerReporteInformativo();
                                //objVmax.AbrirDNF();
                                SerialImpresora = objVmax.RetornoMI.sSerial;
                                FechaImpresora = objVmax.RetornoMI.sFecha;
                                // string Fecha2 = DateTime.Today.ToString("yyyyMMdd");

                                // Si la impresora devuelve true imprimo los comentarios y cierro el CF

                                resp = objVmax.TextoNoFiscal("");
                                resp = objVmax.TextoNoFiscal("Numero Orden: " + txtNumeroOrden.Text);
                                resp = objVmax.TextoNoFiscal("");

                                //// Texto de GRACIAS POR SU COMPRA
                                DataTable DtTexto = _D_DetalleOrden.TB_INUTILIZADO();

                                foreach (DataRow row in DtTexto.Rows)
                                {
                                    resp = objVmax.TextoNoFiscal(row["texto"].ToString());
                                }



                                resp = objVmax.Cerrar();
                                resp = objVmax.CerrarPuerto();


                               
                                if (resp == 0)
                                {
                                   

                                    ImprimirFacturaFiscall = true;
                                    NumeroComprobanteFiscal = objVmax.RetornoAbrirFactura.uiNumeroFactura.ToString();
                                    string Ultimo_Tiket_Anulado = objVmax.RetornoAbrirFactura.uiUltNumeroCancelado.ToString();


                                    switch (NumeroComprobanteFiscal.Length)
                                    {
                                        case 7:
                                            {
                                                NumeroComprobanteFiscal = NumeroComprobanteFiscal;
                                                Ultimo_Tiket_Anulado = Ultimo_Tiket_Anulado;
                                                break;
                                            }

                                        case 6:
                                            {
                                                NumeroComprobanteFiscal = "0" + NumeroComprobanteFiscal;
                                                Ultimo_Tiket_Anulado = "0" + Ultimo_Tiket_Anulado;
                                                break;
                                            }

                                        case 5:
                                            {
                                                NumeroComprobanteFiscal = "00" + NumeroComprobanteFiscal;
                                                Ultimo_Tiket_Anulado = "00" + Ultimo_Tiket_Anulado;
                                                break;
                                            }

                                        case 4:
                                            {
                                                NumeroComprobanteFiscal = "000" + NumeroComprobanteFiscal;
                                                Ultimo_Tiket_Anulado = "000" + Ultimo_Tiket_Anulado;
                                                break;
                                            }

                                        case 3:
                                            {
                                                NumeroComprobanteFiscal = "0000" + NumeroComprobanteFiscal;
                                                Ultimo_Tiket_Anulado = "0000" + Ultimo_Tiket_Anulado;
                                                break;
                                            }

                                        case 2:
                                            {
                                                NumeroComprobanteFiscal = "00000" + NumeroComprobanteFiscal;
                                                Ultimo_Tiket_Anulado = "00000" + Ultimo_Tiket_Anulado;
                                                break;
                                            }

                                        case 1:
                                            {
                                                NumeroComprobanteFiscal = "000000" + NumeroComprobanteFiscal;
                                                Ultimo_Tiket_Anulado = "000000" + Ultimo_Tiket_Anulado;
                                                break;
                                            }
                                    }

                                    iGTF = 0.00;
                                    foreach (DataRow drIgtf in DtIGTF.Tables[0].Rows)
                                    {

                                        iGTF = iGTF + Convert.ToDouble(drIgtf["IGTFCALC1"].ToString());
                                    }

                                    string cedula = txtCedula.Text;
                                    string Nacionalidad = cedula[0].ToString();
                                    int tiempoImpTermica = Convert.ToInt32(_D_DetalleOrden.TB_PARAMETRO("TiempoImpTerm"));

                                    // Esperar un tiempo para que la impresora emita el ticket
                                    Thread.Sleep(tiempoImpTermica); // Esperar 5 segundos (ajusta el tiempo según sea necesario)
                                    objVmax.AbrirPuerto(Convert.ToString(glbPuertoCOM));
                                    objVmax.ObtenerContadores();
                                    string UltimoNumeroFacturaCancelado2 = objVmax.RetornoContadores.uiUltFacturaAnulada.ToString().PadLeft(7, '0');
                                    string UltimoNumeroFacturaEmitido2 = objVmax.RetornoContadores.uiUltFacturaAbierta.ToString().PadLeft(7, '0');
                                    objVmax.CerrarPuerto();
                                    objVmax.Cerrar();
                                    //con datos de tb_abono
                                    if (resp == 0 && UltimoNumeroFacturaCancelado2 != NumeroComprobanteFiscal)
                                    {
                                        

                                        Transaccion = _D_DetalleOrden.GetFactura(TB_CAORDSER.Cod_Sucursal, NumeroComprobanteFiscal, Fecha2, Nacionalidad,
                                     txtCedula.Text.Substring(2, txtCedula.Text.Length - 2), TB_CAORDSER.COD_EMPLEADO, TB_CAORDSER.Cod_Venta, txtNumeroOrden.Text, Convert.ToString(TB_CAORDSER.Fec_ofrecido.ToString("yyyyMMdd")), TB_CAORDSER.Hor_ofrecido, Convert.ToDouble(String.Format(CultureInfo.InvariantCulture, "{0:0.00}", Convert.ToDouble(DtIGTF.Tables[0].Rows[0]["BaseImponible"].ToString()) / 100).Replace(".", ",")),
                                      Convert.ToDouble(String.Format(CultureInfo.InvariantCulture, "{0:0.00}", Convert.ToDouble(DtIGTF.Tables[0].Rows[0]["Alicuota"].ToString()) / 100).Replace(".", ",")), TB_CAORDSER.VtaDescuento, (totalpagos / 100), TB_USUARIO.COD_USR, 0, 0, SerialImpresora,
                                     Fact_MontoExento, Fact_MontoGravable, 0, iGTF, "A", command);

                                       _D_DetalleOrden.PostFactManual(NumeroComprobanteFiscal, txtNumeroOrden.Text, SerialImpresora, "VENEZUELA", command);
                                        Num_Factura = NumeroComprobanteFiscal;

                                        _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "071", TB_USUARIO.COD_EMPLEADO, "OS: " + txtNumeroOrden.Text + ", Factura: " + NumeroComprobanteFiscal + ", Serial: " + SerialImpresora);


                                    }
                                    else
                                    {
                                        ImprimirFacturaFiscall = false;
                                    }

                                }
                                else
                                {
                                    objVmax.Cancelar();
                                    objVmax.Cerrar();
                                    resp = objVmax.CerrarPuerto();
                                    ImprimirFacturaFiscall = false;
                                    return "Error";
                                }
                            }


                        }
                        else
                        {

                            // SI NO COINCIDEN, ANULO EL TICKET
                            objVmax.Cancelar();
                            objVmax.Cerrar();
                            objVmax.CerrarPuerto();
                            ImprimirFacturaFiscall = false;
                            respuesta = false;
                            return "Error";

                        }
                       
                        return Transaccion;
                    }


                }
                else
                {
                    string NroControl = Convert.ToString(TxtNroCorrelativo.Text);
                    NroControl = NroControl.Replace("_", "");

                    // Se valida que el numero de control tenga 11 caracteres y el numero de factura 11
                    if (NroControl.Trim().Length < 11 | TxtNumFact.Text.Trim().Length != 7)
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("El número de factura debe tener 7 dígitos y el número de control 10 dígitos");
                        _FrmMensajes.ShowDialog();
                        return "FALLIDO";

                    }

                    Double totalpagosManual = 0;

                        // OBTENGO LA SUMATORIA DE LOS ABONOS PARA enviarlos en la factura manual  
                        DataTable dtPago = _D_DetalleOrden.TEMP_ABONO(TB_CAORDSER.Cod_Sucursal, txtNumeroOrden.Text, TB_CAORDSER.Revision, command);

                        foreach (DataRow drPago in dtPago.Rows)
                        {
                        totalpagosManual = totalpagosManual + (Convert.ToDouble(drPago["Abo_Monto"].ToString().Replace(",", "")));
                        }


                    // OBTENGO el IGTF enviarlos en la factura manual  
                    iGTF = 0.00;
                    foreach (DataRow drIgtf in DtIGTF.Tables[0].Rows)
                    {

                        iGTF = iGTF + Convert.ToDouble(drIgtf["IGTFCALC1"].ToString());
                    }

                    // OBTENGO el Fact_MontoExento y  Fact_MontoGravable  en la factura manual 
                    Fact_MontoExento = 0;
                    Fact_MontoGravable = 0;
                    foreach (DataRow drItem in dt.Rows)
                    {
                        if (Convert.ToString(drItem["ART_EXENTO"]) == "1")
                        {
                            Fact_MontoExento = Fact_MontoExento + (Convert.ToDouble(drItem["Ordserv_Bruto"]) - (Convert.ToDouble(drItem["Ordserv_Dto"]) / 100));

                        }
                        else
                        {
                            Fact_MontoGravable = Fact_MontoGravable + (Convert.ToDouble(drItem["Ordserv_Bruto"]) - (Convert.ToDouble(drItem["Ordserv_Dto"]) / 100));
                        }

                    }


                    // FACTURA MANUAL 
                    // Si es factura manual se llena tbfactura de la siguiente manera 
                    string cedula = txtCedula.Text;
                    string Nacionalidad = cedula[0].ToString();

                    Transaccion = _D_DetalleOrden.GetFactura(TB_CAORDSER.Cod_Sucursal, TxtNumFact.Text, Fecha2, Nacionalidad,
                                     txtCedula.Text.Substring(2, txtCedula.Text.Length - 2), TB_CAORDSER.COD_EMPLEADO, TB_CAORDSER.Cod_Venta, txtNumeroOrden.Text, Convert.ToString(TB_CAORDSER.Fec_ofrecido.ToString("yyyyMMdd")), TB_CAORDSER.Hor_ofrecido, Convert.ToDouble(String.Format(CultureInfo.InvariantCulture, "{0:0.00}", Convert.ToDouble(DtIGTF.Tables[0].Rows[0]["BaseImponible"].ToString()) / 100).Replace(".", ",")),
                                      Convert.ToDouble(String.Format(CultureInfo.InvariantCulture, "{0:0.00}", Convert.ToDouble(DtIGTF.Tables[0].Rows[0]["Alicuota"].ToString()) / 100).Replace(".", ",")), TB_CAORDSER.VtaDescuento, (totalpagosManual/100), TB_USUARIO.COD_USR, 0, 0, "FACTMANUAL",
                                     Fact_MontoExento, Fact_MontoExento, 0, iGTF, "A", command, true, _L_ListaOrdenes.Completar_Numero_Control(TxtNroCorrelativo.Text));

                    _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "071", TB_USUARIO.COD_EMPLEADO, "OS: " + txtNumeroOrden.Text + ", Factura: " + TxtNumFact.Text + ", Serial: " + "FACTMANUAL");


                    _D_DetalleOrden.PostFactManual(TxtNumFact.Text, txtNumeroOrden.Text, "FACTMANUAL", "VENEZUELA", command);
                    Num_Factura = TxtNumFact.Text;
                    //if (Transaccion == "SATISFACTORIO")
                    //{
                    //    ImprimirFacturaFiscall = true;
                    //}

                    //else
                    //{
                    //    ImprimirFacturaFiscall = false;
                    //}

                }

                return Transaccion;
            }
            catch (SqlException ex)
            {
                _FrmMensajes.co = 2;
                if (ex.Number == 2627) // Código de error común para violación de clave primaria
                {
                    _FrmMensajes.avisomensaje("Este número de factura ya existe");
                }   
                else
                {
                    _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                }
                _FrmMensajes.ShowDialog();
                objVmax.Cancelar();
                objVmax.Cerrar();
                objVmax.CerrarPuerto();
                ImprimirFacturaFiscall = false;
                return "";
            }
            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                // Manejar otras excepciones
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
                objVmax.Cancelar();
                objVmax.Cerrar();
                objVmax.CerrarPuerto();
                ImprimirFacturaFiscall = false;
                return "";
            }


            finally
            {
              try
              {
                if (ImprimirFacturaFiscall == false)
                {
                  if (_L_Facturacion.ValidaFactManual(command) == false && NumeroComprobanteFiscal != "0" &&  SerialImpresora != "")
                  {
                            // Reversamos la Transacion para guardar la factura en la base de datos 
                            //command.Transaction.Rollback();
                            rollbackRealizado = true;
                            string Resp = _D_DetalleOrden.GetFactura(TB_CAORDSER.Cod_Sucursal, NumeroComprobanteFiscal.PadLeft(7, '0'), DateTime.Today.ToString("yyyyMMdd"), txtCedula.Text[0].ToString(),
                                      txtCedula.Text.Substring(2, txtCedula.Text.Length - 2), TB_CAORDSER.COD_EMPLEADO, TB_CAORDSER.Cod_Venta, txtNumeroOrden.Text, Convert.ToString(TB_CAORDSER.Fec_ofrecido.ToString("yyyyMMdd")), TB_CAORDSER.Hor_ofrecido, Convert.ToDouble("0,00"),
                                       Convert.ToDouble("0,00"), Convert.ToDouble("0,00"), Convert.ToDouble("0,00"), TB_USUARIO.COD_USR, 0, 0, SerialImpresora,
                                        Convert.ToDouble("0,00"), Convert.ToDouble("0,00"), Convert.ToDouble("0,00"), Convert.ToDouble("0,00"), "I", null);

                            _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "071", TB_USUARIO.COD_EMPLEADO, "OS: " + txtNumeroOrden.Text + ", Factura: " + NumeroComprobanteFiscal + ", Serial: " + SerialImpresora);
                        }
                }
              }
                catch (Exception ex)
                {
                    _FrmMensajes.co = 2;
                    // Manejar otras excepcionesDEBE LLENAR TODOS
                    _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error nivel 2");
                    _FrmMensajes.ShowDialog();
                    throw;
                }
            }

        }

        private void DgvListadoOrdenes_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            //Para colocar un icono en el boton del grid
            if (e.ColumnIndex >= 0 && this.DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Eliminar" && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                e.CellStyle.BackColor = Color.Red;
                DataGridViewButtonCell celBoton = this.DgvListadoOrdenes.Rows[e.RowIndex].Cells["Eliminar"] as DataGridViewButtonCell;
                Icon IconAtomico;

                //Validar la fecha activa para poner un iciono o otro 
                if (Convert.ToDateTime(DgvListadoOrdenes.Rows[e.RowIndex].Cells["Fecha"].Value).ToString("yyyyMMdd") == _D_Inicio.DiaActivo().ToString("yyyyMMdd"))
                {
                
                    if(ModoClaro == false)
                    {
                        IconAtomico = new Icon(Environment.CurrentDirectory + @"\\cuadraditoOscuro2.ico");
                        HabEliminarPagos = true; //Se manda señal de boton ACTIVADO para realizar validaciones posteriores 
                    }
                    else
                    {
                        IconAtomico = new Icon(Environment.CurrentDirectory + @"\\cuadraditoOscuro.ico");
                        HabEliminarPagos = true; //Se manda señal de boton ACTIVADO para realizar validaciones posteriores 
                    }


                }
                else
                {
                    if (ModoClaro == false)
                    {
                        IconAtomico = new Icon(Environment.CurrentDirectory + @"\\cuadraditoClaro2.ico");
                        HabEliminarPagos = false; //Se manda señal de boton DESACTIVADO para realizar validaciones posteriores 

                    }

                    else 
                    {
                        IconAtomico = new Icon(Environment.CurrentDirectory + @"\\cuadraditoClaro.ico");
                        HabEliminarPagos = false;//Se manda señal de boton DESACTIVADO para realizar validaciones posteriores 

                    }
                }

                e.Graphics.DrawIcon(IconAtomico, e.CellBounds.Left + 1, e.CellBounds.Top + 0);
                this.DgvListadoOrdenes.Rows[e.RowIndex].Height = IconAtomico.Height + 0;
                this.DgvListadoOrdenes.Columns[e.ColumnIndex].Width = IconAtomico.Width + 2;
                e.Handled = true;

            }

            DgvListadoOrdenes.Size = new Size(1059, 150);
        }

        private void DgvNotas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //////Esta funcion  sirve para marcar y desmarcar la lista del DataGrid de Notas 
            string PosicionActual = DgvNotas.CurrentRow.Index.ToString();
            if (DgvNotas.Columns[e.ColumnIndex].Name == "RdButom")
            {

                for (int Indice = 0; Indice <= DgvNotas.Rows.GetLastRow(DataGridViewElementStates.Displayed); Indice++)
                {
                    if (PosicionInical == "")
                    {
                        DgvNotas.Rows[Indice].Cells["RdButom"].Value = false;
                    }

                }

                if (PosicionInical == "")
                {
                    txtBsNotaCredito.Text = string.Format("{0:#,0.00}", Convert.ToDecimal(DgvNotas.CurrentRow.Cells["SaldoNota"].Value.ToString()));
                    NotaNumFactura = DgvNotas.CurrentRow.Cells["Fact_Num"].Value.ToString();
                    NotaNumNota = DgvNotas.CurrentRow.Cells["NRONOTA"].Value.ToString();
                    MotivoNota = DgvNotas.CurrentRow.Cells["Motivo"].Value.ToString();
                    DgvNotas.CurrentRow.Cells["RdButom"].Value = true;
                    txtBsNotaCredito.Enabled = true;
                    btnProcesar3.Enabled = true;
                    txtNroNota.Enabled = false;
                    rd_Nota.Enabled = false;
                    rd_Fact.Enabled = false;

                }

                if (PosicionInical != "" && PosicionActual != PosicionInical)

                {
                    txtBsNotaCredito.Text = string.Format("{0:#,0.00}", Convert.ToDecimal(DgvNotas.CurrentRow.Cells["SaldoNota"].Value.ToString()));
                    NotaNumFactura = DgvNotas.CurrentRow.Cells["Fact_Num"].Value.ToString();
                    NotaNumNota = DgvNotas.CurrentRow.Cells["NRONOTA"].Value.ToString();
                    MotivoNota = DgvNotas.CurrentRow.Cells["Motivo"].Value.ToString();
                    DgvNotas.CurrentRow.Cells["RdButom"].Value = true;
                    txtBsNotaCredito.Enabled = true;
                    btnProcesar3.Enabled = true;
                    txtNroNota.Enabled = false;
                    rd_Nota.Enabled = false;
                    rd_Fact.Enabled = false;

                }

                if (PosicionInical != "" && PosicionActual == PosicionInical)
                {
                    txtBsNotaCredito.Text = "";
                    NotaNumFactura = "";
                    NotaNumNota = "";
                    MotivoNota = "";
                    DgvNotas.CurrentRow.Cells["RdButom"].Value = false;
                    txtBsNotaCredito.Enabled = false;
                    btnProcesar3.Enabled = false;
                    txtNroNota.Enabled = true;
                    rd_Nota.Enabled = true;
                    rd_Fact.Enabled = true;
                    PosicionInical = "Repetido";
                    PosicionActual = "-4";
                }

            }


            for (int Indice = 0; Indice <= DgvNotas.Rows.GetLastRow(DataGridViewElementStates.Displayed); Indice++)
            {

                if (Indice != Convert.ToInt64(PosicionActual))
                {
                    DgvNotas.Rows[Indice].Cells["RdButom"].Value = false;
                }

            }

            if (PosicionInical == "Repetido")
            {
                PosicionInical = "";
            }
            else
            {
                PosicionInical = DgvNotas.CurrentRow.Index.ToString();
            }

        }



        private void btnProcesar3_Click(object sender, EventArgs e)
        {
            LimpiaVariablesIdAbonoPagoMovil();
            double TotalNota = 0.00;
            double TotalOrden;
            TotalOrden = Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos));



            for (int Indice = 0; Indice <= DgvNotas.Rows.GetLastRow(DataGridViewElementStates.Displayed); Indice++)
            {
                if (DgvNotas.Rows[Indice].Cells["NRONOTA"].Value.ToString() == NotaNumNota && DgvNotas.Rows[Indice].Cells["Fact_Num"].Value.ToString() == NotaNumFactura)
                {

                    TotalNota = Convert.ToDouble(DgvNotas.Rows[Indice].Cells["SaldoNota"].Value.ToString());

                }


            }

            if (TotalOrden >= (txtBsNotaCredito.Text == "" ? (Double)0.00 : Convert.ToDouble(txtBsNotaCredito.Text.Replace(".",""))))
            {

                if (TotalNota >= (txtBsNotaCredito.Text == "" ? (Double)0.00 : Convert.ToDouble(txtBsNotaCredito.Text.Replace(".", ""))) && (txtBsNotaCredito.Text == "" ? (Double)0.00 : Convert.ToDouble(txtBsNotaCredito.Text.Replace(".", ""))) > 0)
                {

                    _L_Facturacion.GuardarAbonoGrid(2,Dt_Abonos, CbxMetodosPago.Text, "Bolivares", "", txtBsNotaCredito.Text, "", DtpFecha.Value.ToString(), CbxMetodosPago.SelectedValue.ToString(), "000", "", "", "", "", "", "", "", NotaNumNota);
                    BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);
                    //-----------ConvertirBolivares---------------------------
                    //txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);

                    LimpiarTxbox();
                    VisualizarPanel("MostrarPanelPrincipal");
                    LimpiarNotasCredito();
                }

                else
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("El monto de la nota debe ser menor o igual al de la orden");
                    _FrmMensajes.ShowDialog();

                }



            }

            else
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("El monto de la nota debe ser menor o igual al de la orden");
                _FrmMensajes.ShowDialog();

            }


        }

        private void txtBsNotaCredito_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == 20)
            //{
            //    e.Handled = false;
            //    return;
            //}



            bool IsDec = false;
            int nroDec = 0;

            //if (txtBsNotaCredito.SelectionLength <= 0)
            //{



            //    for (int i = 0; i < txtBsNotaCredito.Text.Length; i++)
            //    {
            //        if (txtBsNotaCredito.Text[i] == ',')
            //            IsDec = true;

            //        if (IsDec && nroDec++ >= 2)
            //        {
            //            e.Handled = true;
            //            return;
            //        }


            //    }
            //}
            //if (e.KeyChar >= 44 && e.KeyChar <= 57)
            //    e.Handled = false;
            //else if (e.KeyChar == 46)
            //    e.Handled = (IsDec) ? true : false;
            //else
            //    e.Handled = true;


            //para que solo acepte numeros y una sola coma
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }


            if (txtBsNotaCredito.Text.Contains("") && e.KeyChar == 44)
            {
                //separamos por punto
                string[] parts = txtBsNotaCredito.Text.Split(',');
                //si el primer elemento está vacío, significa que no se escribió nada antes de, entonces, añadimos el cero al textbox.
                if (parts[0].Length <= 0)
                {
                    txtBsNotaCredito.Text = "0" + txtBsNotaCredito.Text;
                    //UPDATE: colocamos el cursor al final del texto
                    txtBsNotaCredito.SelectionStart = txtBsNotaCredito.Text.Length;
                }
            }

        }

        private void txtBsNotaCredito_Click(object sender, EventArgs e)
        {
            //txtBsNotaCredito.Text = "";
        }


        private void txtCVC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtVence_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtVuelto_Validating(object sender, CancelEventArgs e)
        {
            if (TxtVuelto.Text == "")
            {
                TxtVuelto.Text = "0,00";
            }
            else
            {
                TxtVuelto.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", TxtVuelto.Text).Replace(".", ",");
            }
        }

        private void TxtAbono_TextChanged(object sender, EventArgs e)
        {

        }

        private void DgvListadoOrdenes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            //DgvListadoOrdenes.Columns["Fec_Crea"]

            if (DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Eliminar")
            {
                _D_DetalleOrden.ObtenerFactura(TB_CAORDSER.NumOrdserv);

                if (TB_CAORDSER.OrSer_Status == "005")
                {
                    if (Convert.ToDateTime(DgvListadoOrdenes.CurrentRow.Cells["Fecha"].Value).ToString("yyyyMMdd") == _D_Inicio.DiaActivo().ToString("yyyyMMdd"))
                    {
                        if (TB_CAORDSER.OTCORRESPONDIENTE != "" && TB_CAORDSER.OTCORRESPONDIENTE != null)
                        {
                            DataTable OssHijo = _D_DetalleOrden.ObtenerOrdenHijo(TB_CAORDSER.OTCORRESPONDIENTE);
                            DataTable oFactHijo = _D_DetalleOrden.ObtenerFacturaHijo(TB_CAORDSER.OTCORRESPONDIENTE);

                            if (TB_CAORDSER.Aplica == true && TB_CAORDSER.OTCORRESPONDIENTE != "" && OssHijo.Rows[0]["Cod_Servicio"].ToString() != "003")
                            {
                                if (oFactHijo.Rows[0]["Fact_Status"].ToString() == "A")
                                {
                                    _FrmMensajes.co = 2;
                                    _FrmMensajes.avisomensaje("Esta factura es parte de una PROMOCIÓN y tiene una orden asociada. Debe anular primero la orden hijo(fact: " + oFactHijo.Rows[0]["Fact_Num"].ToString() + ") y luego proceda con esta anulación, Proceso no permitido");
                                    _FrmMensajes.ShowDialog();
                                    return;
                                }
                                else
                                {
                                    _FrmMensajes.co = 2;
                                    _FrmMensajes.avisomensaje("Esta factura es parte de una PROMOCIÓN y tiene una orden asociada la cual se encuentra abonada. Debe anular primero la orden hijo (OS: " + TB_CAORDSER.OTCORRESPONDIENTE.ToString() + ") y luego proceda con esta anulación, Proceso no permitido");
                                    _FrmMensajes.ShowDialog();
                                    return;
                                }
                            }
                        }

                        _FrmClaveAutorizada.ShowDialog();

                        if (_FrmClaveAutorizada.DialogResult == DialogResult.OK)
                        {
                            if (_FrmClaveAutorizada.ClaveCorrecta == true)
                            {
                                string mensaje = "¿ Está seguro de anular este pago ? ";
                                _FrmMensajes.co = 3;
                                _FrmMensajes.avisomensaje(mensaje);
                                _FrmMensajes.ShowDialog();

                                if (_FrmMensajes.DialogResult == DialogResult.OK)
                                {

                                    _FrmAnulacion.AnulacionPagos = true;
                                    _FrmAnulacion.IdAbono = Convert.ToInt32(DgvListadoOrdenes.CurrentRow.Cells["ID_Abono"].Value.ToString());
                                    _FrmAnulacion.MontoAnulacion = Convert.ToDouble(DgvListadoOrdenes.CurrentRow.Cells["Abo_Monto"].Value.ToString());
                                    _FrmAnulacion.TipoPagoAnular = DgvListadoOrdenes.CurrentRow.Cells["Tipo_Pago"].Value.ToString();
                                    _FrmAnulacion.GerenteAutoriza = VariablesGlobales.UsuarioAutorizado_FrmClaveAutorizada;
                                    _FrmAnulacion.ShowDialog();

                                }
                                else
                                {
                                    string mensajer = "La clave ingresada es invalida";
                                    _FrmMensajes.co = 2;
                                    _FrmMensajes.avisomensaje(mensajer);
                                    _FrmMensajes.ShowDialog();
                                }

                                LimpiarGrid();

                                DataTable Pagos = _L_Facturacion.MostarPagosGrid(TB_CAORDSER.Cod_Sucursal, txtNumeroOrden.Text, TB_CAORDSER.Revision);

                                if (Pagos.Rows.Count > 0)
                                {
                                    CantAbonosPrevios = Pagos.Rows.Count;
                                    DgvListadoOrdenes.DataSource = Pagos;
                                    CrearObjetos();
                                }

                                CargarDatosOrden(txtNumeroOrden.Text, txtNombreCliente.Text, TB_CAORDSER.Revision);

                            }


                        }


                    }

                    else
                    { //CUANDO NO SE PUEDE ELIMINAR PAGOS PORQUE NOP FUEREO REALIZADOS EL MISMO DIA DEL DIA ACTIVO

                        if (HabEliminarPagos==true) //Si el boton de eliminarpago  llegase a estar activado se muestra el siguiente mensaje 
                        {
                            _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("La fecha del abono debe ser igual al día activo, para eliminar el abono. Proceso no permitido");
                        _FrmMensajes.ShowDialog();
                        return;
                        }

                        else // si el boton de eliminarpago se encuentra desactivado no se muestra nada 
                        {

                            //No se mnuestra nada 
                        }
                }

                }
            }
        }

        public void FormatoFacturacion1(System.Drawing.Color col1, System.Drawing.Color col3)
        {
            ModoClaro = true;

            this.BackColor = col1;

            DgvAbonos.BackgroundColor = col1;
            DgvAbonos.DefaultCellStyle.BackColor = col1;
            DgvAbonos.ColumnHeadersDefaultCellStyle.BackColor = col3;
            DgvAbonos.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(30, 30, 30, 30);
            DgvAbonos.DefaultCellStyle.ForeColor = Color.Black;

            DgvNotas.BackgroundColor = col1;
            DgvNotas.DefaultCellStyle.BackColor = col1;
            DgvNotas.ColumnHeadersDefaultCellStyle.BackColor = col3;
            DgvNotas.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(30, 30, 30, 30);
            DgvNotas.DefaultCellStyle.ForeColor = Color.Black;

            DgvListadoOrdenes.BackgroundColor = col1;
            DgvListadoOrdenes.DefaultCellStyle.BackColor = col1;
            DgvListadoOrdenes.ColumnHeadersDefaultCellStyle.BackColor = col3;
            DgvListadoOrdenes.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(30, 30, 30, 30);
            DgvListadoOrdenes.DefaultCellStyle.ForeColor = Color.Black;

            DgvBilletes.BackgroundColor = col1;
            DgvBilletes.DefaultCellStyle.BackColor = col1;
            DgvBilletes.ColumnHeadersDefaultCellStyle.BackColor = col3;
            DgvBilletes.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(30, 30, 30, 30);
            DgvBilletes.DefaultCellStyle.ForeColor = Color.Black;

            DgvNotasDevolicion.BackgroundColor = col1;
            DgvNotasDevolicion.DefaultCellStyle.BackColor = col1;
            DgvNotasDevolicion.BackgroundColor = col3;
            DgvNotasDevolicion.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(30, 30, 30, 30);
            DgvNotasDevolicion.DefaultCellStyle.ForeColor = Color.Black;

            label41.ForeColor = Color.FromArgb(30, 30, 30);
            label5.ForeColor = Color.FromArgb(30, 30, 30);
            label26.ForeColor = Color.FromArgb(30, 30, 30);
            label24.ForeColor = Color.FromArgb(30, 30, 30);
            label4.ForeColor = Color.FromArgb(30, 30, 30);
            label45.ForeColor = Color.FromArgb(30, 30, 30);
            label42.ForeColor = Color.FromArgb(30, 30, 30);
            Bs.ForeColor = Color.FromArgb(30, 30, 30);
            label27.ForeColor = Color.FromArgb(30, 30, 30);
            label28.ForeColor = Color.FromArgb(30, 30, 30);
            label22.ForeColor = Color.FromArgb(30, 30, 30);
            label21.ForeColor = Color.FromArgb(30, 30, 30);
            label6.ForeColor = Color.FromArgb(30, 30, 30);
            label43.ForeColor = Color.FromArgb(30, 30, 30);
            label44.ForeColor = Color.FromArgb(30, 30, 30);

            tabPage1.BackColor = Color.White;
            label110.BackColor = Color.White;
            label108.BackColor = Color.White;

            label11.ForeColor = Color.Black;
            label12.ForeColor = Color.Black;
            label13.ForeColor = Color.Black;
            label15.ForeColor = Color.Black;
            label16.ForeColor = Color.Black;
            label17.ForeColor = Color.Black;
            label18.ForeColor = Color.Black;
            label19.ForeColor = Color.Black;
            label20.ForeColor = Color.Black;

            label26.ForeColor = Color.Black;
            label24.ForeColor = Color.Black;
            label15.ForeColor = Color.Black;
            label27.ForeColor = Color.Black;
            label5.ForeColor = Color.Black;
            label28.ForeColor = Color.Black;
            label42.ForeColor = Color.Black;
            label4.ForeColor = Color.Black;
            label21.ForeColor = Color.Black;
            label6.ForeColor = Color.Black;
            label41.ForeColor = Color.Black;
            label45.ForeColor = Color.Black;
            label71.ForeColor = Color.Black;
            lbCedularPago.ForeColor = Color.Black;
            lbMontoPago.ForeColor = Color.Black;
            lbBancoPago.ForeColor = Color.Black;
            label22.ForeColor = Color.Black;
            label21.ForeColor = Color.Black;
            LbePagoMovil.ForeColor = Color.Black;
            LblMontoBillete.ForeColor = Color.Black;
            LblCodBillete.ForeColor = Color.Black;
            lbCedulaPago.ForeColor = Color.Black;
            label49.ForeColor = Color.Black;
            label51.ForeColor = Color.Black;
            label33.ForeColor = Color.Black;
            LblBancoRecep.ForeColor = Color.Black;

            LblTituloDatos.BackColor = Color.FromArgb(7, 167, 155);
            LblTituloDatos.ForeColor = Color.FromArgb(30, 30, 30);
            LblTituloGrid.BackColor = Color.FromArgb(7, 167, 155);
            LblTituloGrid.ForeColor = Color.FromArgb(30, 30, 30);


        }

        public void FormatoFacturacion2(System.Drawing.Color col2, System.Drawing.Color col3, System.Drawing.Color col4)
        {
            ModoClaro = false;

            this.BackColor = col2;
            DgvAbonos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DgvAbonos.ColumnHeadersDefaultCellStyle.BackColor = col4;
            DgvAbonos.BackgroundColor = col3;
            DgvAbonos.DefaultCellStyle.BackColor = col3;
            DgvAbonos.DefaultCellStyle.ForeColor = Color.White;

            DgvNotas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DgvNotas.ColumnHeadersDefaultCellStyle.BackColor = col4;
            DgvNotas.BackgroundColor = col3;
            DgvNotas.DefaultCellStyle.BackColor = col3;
            DgvNotas.DefaultCellStyle.ForeColor = Color.White;

            DgvListadoOrdenes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DgvListadoOrdenes.ColumnHeadersDefaultCellStyle.BackColor = col4;
            DgvListadoOrdenes.BackgroundColor = col3;
            DgvListadoOrdenes.DefaultCellStyle.BackColor = col3;
            DgvListadoOrdenes.DefaultCellStyle.ForeColor = Color.White;

            DgvBilletes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DgvBilletes.ColumnHeadersDefaultCellStyle.BackColor = col4;
            DgvBilletes.BackgroundColor = col3;
            DgvBilletes.DefaultCellStyle.BackColor = col3;
            DgvBilletes.DefaultCellStyle.ForeColor = Color.White;

            DgvNotasDevolicion.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DgvNotasDevolicion.ColumnHeadersDefaultCellStyle.BackColor = col4;
            DgvNotasDevolicion.BackgroundColor = col3;
            DgvNotasDevolicion.DefaultCellStyle.BackColor = col3;
            DgvNotasDevolicion.DefaultCellStyle.ForeColor = Color.White;

            tabPage1.BackColor = col3;
            label110.BackColor = col2;
            label108.BackColor = col2;

            label41.ForeColor = Color.White;
            label5.ForeColor = Color.White;
            label26.ForeColor = Color.White;
            label24.ForeColor = Color.White;
            label4.ForeColor = Color.White;
            label45.ForeColor = Color.White;
            label42.ForeColor = Color.White;
            Bs.ForeColor = Color.White;
            label27.ForeColor = Color.White;
            label28.ForeColor = Color.White;
            label22.ForeColor = Color.White;
            label21.ForeColor = Color.White;
            label6.ForeColor = Color.White;
            label43.ForeColor = Color.White;
            label44.ForeColor = Color.White;


            label11.ForeColor = Color.White;
            label12.ForeColor = Color.White;
            label13.ForeColor = Color.White;
            label15.ForeColor = Color.White;
            label16.ForeColor = Color.White;
            label17.ForeColor = Color.White;
            label18.ForeColor = Color.White;
            label19.ForeColor = Color.White;
            label20.ForeColor = Color.White;

            label26.ForeColor = Color.White;
            label24.ForeColor = Color.White;
            label15.ForeColor = Color.White;
            label27.ForeColor = Color.White;
            label5.ForeColor = Color.White;
            label28.ForeColor = Color.White;
            label42.ForeColor = Color.White;
            label4.ForeColor = Color.White;
            label21.ForeColor = Color.White;
            label6.ForeColor = Color.White;
            label41.ForeColor = Color.White;
            label45.ForeColor = Color.White;
            label71.ForeColor = Color.White;
            lbCedularPago.ForeColor = Color.White;
            lbMontoPago.ForeColor = Color.White;
            lbBancoPago.ForeColor = Color.White;
            label22.ForeColor = Color.White;
            label21.ForeColor = Color.White;
            LbePagoMovil.ForeColor = Color.White;
            LblMontoBillete.ForeColor = Color.White;
            LblCodBillete.ForeColor = Color.White;
            lbCedulaPago.ForeColor = Color.White;

            label49.ForeColor = Color.White;
            label51.ForeColor = Color.White;
            label33.ForeColor = Color.White;
            LblBancoRecep.ForeColor = Color.White;


            //GbxVentasDia.BackColor = Color.FromArgb(0, 152, 153);
            LblTituloDatos.BackColor = Color.FromArgb(0, 53, 54);
            LblTituloDatos.ForeColor = Color.White;

            LblTituloGrid.BackColor = Color.FromArgb(0, 53, 54);
            LblTituloGrid.ForeColor = Color.White;

        }

        private void CbxMoneda_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxMoneda.SelectedIndex != -1)
            {

                if (CbxMoneda.Text == "Dólares")
                {
                    //agregado nuevo 17-05-2023
                    DataTable dt = _D_DetalleOrden.BucarTotalAbonosRealizados(TB_CAORDSER.NumOrdserv);
                    Double Igtf_TotalAboTranferenciaDolar = Math.Round(Convert.ToDouble(dt.Rows[0]["TotalPagoIgtf"].ToString()), 2);
                    Igtf_TotalAboTranferenciaDolar = Igtf_TotalAboTranferenciaDolar + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos));

                    BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);

                    string Resultado_Parametro = _D_DetalleOrden.TB_PARAMETRO("ActivaIGTF");
                    bool Cobro_IGTF = Convert.ToBoolean(Convert.ToInt32(Resultado_Parametro));

                    
                    if (Cobro_IGTF == true)
                    {
                        double MontoFaltanteIgtfbs = (Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) - Igtf_TotalAboTranferenciaDolar) * 0.03;

                        txtRef.Text = Convert.ToString(Math.Round(((Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) + MontoFaltanteIgtfbs) / Convert.ToDouble(TB_TASA_Dolar.Tasa)), 2));
                    }
                    else
                    {
                        txtRef.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) / Convert.ToDouble(TB_TASA_Dolar.Tasa)), 2));
                    }


                    //modificado 17-05-2023
                    txtIGTF.Text = Convert.ToString(_L_Facturacion.CalculoIgtf(txtNumeroOrden, Convert.ToString(Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) - Igtf_TotalAboTranferenciaDolar), DgvAbonos));
                    BolivaresConveridos(Convert.ToDouble(Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) + Convert.ToDouble(txtIGTF.Text)), txtMonto2Bs);
                    //txtMonto2Bs.Text = Convert.ToString(Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) + Convert.ToDouble(txtIGTF.Text));

                    txtTasaFact.Text = string.Format("{0:#,0.00}", Convert.ToDecimal(TB_TASA_Dolar.Tasa.ToString()));

                }

                if (CbxMoneda.Text == "Euros")
                {
                    if (CbxMoneda.Text == "Euros" && TB_TASA_Euro.Tasa != 0 && TB_TASA_Euro.Tasa != null)
                    {

                        //agregado nuevo 17-05-2023
                        DataTable dt = _D_DetalleOrden.BucarTotalAbonosRealizados(TB_CAORDSER.NumOrdserv);
                        Double Igtf_TotalAboTranferenciaDolar = Math.Round(Convert.ToDouble(dt.Rows[0]["TotalPagoIgtf"].ToString()), 2);
                        Igtf_TotalAboTranferenciaDolar = Igtf_TotalAboTranferenciaDolar + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos));

                        BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);
                        //-----------ConvertirBolivares---------------------------
                        //txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);

                        string Resultado_Parametro = _D_DetalleOrden.TB_PARAMETRO("ActivaIGTF");
                        bool Cobro_IGTF = Convert.ToBoolean(Convert.ToInt32(Resultado_Parametro));

                        if (Cobro_IGTF == true)
                        {
                            double MontoFaltanteIgtfbs = (Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) - Igtf_TotalAboTranferenciaDolar) * 0.03;

                            txtRef.Text = Convert.ToString(Math.Round(((Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) + MontoFaltanteIgtfbs) / Convert.ToDouble(TB_TASA_Euro.Tasa)), 2));
                        }
                        else
                        {
                            txtRef.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) / Convert.ToDouble(TB_TASA_Euro.Tasa)), 2));
                        }


                        //modificado 17-05-2023
                        txtIGTF.Text = Convert.ToString(_L_Facturacion.CalculoIgtf(txtNumeroOrden, Convert.ToString(Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) - Igtf_TotalAboTranferenciaDolar), DgvAbonos));
                        BolivaresConveridos(Convert.ToDouble(Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) + Convert.ToDouble(txtIGTF.Text)), txtMonto2Bs);
                        //txtMonto2Bs.Text = Convert.ToString(Convert.ToDouble(txtMontoBs.Text.Replace(".", "")) + Convert.ToDouble(txtIGTF.Text));

                        txtTasaFact.Text = string.Format("{0:#,0.00}", Convert.ToDecimal(TB_TASA_Euro.Tasa.ToString()));

                    }
                    else
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("Debe Actualizar la Tasa de las Monedas y Activación de Secuencia Diaria, verifique");
                        _FrmMensajes.ShowDialog();
                    }
                    double reftotal = Convert.ToDouble(txtRef.Text.Replace(".", ""));
                    double igtftotal = Convert.ToDouble(txtIGTF.Text.Replace(".", ""));
                    double tasa = Convert.ToDouble(txtTasaFact.Text.Replace(".", ""));

                    txtTotalRef.Text = string.Format("{0:#,0.00}",  Convert.ToDecimal(reftotal + (igtftotal / tasa)));
                   



                }

            }
        }

        private void BtnClientePagador_Click(object sender, EventArgs e)
        {
            txtCedula.Enabled = true;
            ClienteAnterior = txtNombreCliente.Text;
            CedulaAnterior = txtCedula.Text;
            TlfAnterior = TxtTelefono.Text;
            CorreoAnterior = TxtCorreo.Text;


            txtCedula.Text = "";
            txtCedula.Focus();

        }

        private void DanaServis()
        {
            _D_DetalleOrden.ObtenerFactura(TB_CAORDSER.NumOrdserv);
            DataSet Factura = _D_Dana.FactPagadaNC(TB_FACTURAS.Fact_Num, TB_FACTURAS.Fact_SerialImpresora);

            if (Factura != null)
            {
                if (Factura.Tables[2].Rows[0][0].ToString() == "1")
                {
                    DataSet dsSMS = _D_Dana.ServicioDanaActivo("ServicioDANAActivo%");
                    if (dsSMS.Tables[0].Rows.Count > 0)
                    {
                        if (dsSMS.Tables[0].Rows[0]["Valor"].ToString() == "1")
                        {

                            if (_L_DanaService.DanaService(txtCedula.Text.Substring(2, txtCedula.Text.Length - 2), TB_CAORDSER.CTE_Nacio, "OrdenFacturada") == "OK")
                            {
                                _D_Dana.IngresarMensajeDana(_D_Inicio.Sucursal(), TB_CAORDSER.CTE_Nacio, txtCedula.Text.Substring(2, txtCedula.Text.Length - 2), "OrdenFacturada", DateTime.Now.ToString("yyyy/MM/dd"), DateTime.Now.ToString("HH:mm:00"), TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision, true);

                            }
                            else
                            {
                                _D_Dana.IngresarMensajeDana(_D_Inicio.Sucursal(), TB_CAORDSER.CTE_Nacio, txtCedula.Text.Substring(2, txtCedula.Text.Length - 2), "OrdenFacturada", DateTime.Now.ToString("yyyy/MM/dd"), DateTime.Now.ToString("HH:mm:00"), TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision, false);
                            }
                        }
                    }

                }
            }
        }

        private void txtCedula_KeyPress(object sender, KeyPressEventArgs e)
        {



            // Validacion para que solo permite caracteres alfanumericos(NO SE BORRA PORQUE PUEDE SER UTIL)
            //if ((!(char.IsNumber(e.KeyChar)) && (!(char.IsLetter(e.KeyChar))) && (e.KeyChar != (char)Keys.Back)))
            //{
            //    e.Handled = true;
            //}

            // Validacion para que solo permita numeros 
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
            }

            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                string cedula = txtCedula.Text;

                _L_Facturacion.BuscarCliente(cedula);

                if (_L_Facturacion.MostrarClientePag == true)
                {
                    txtNombreCliente.Text = _L_Facturacion.NombreCliente;
                    txtNombreCliente.Enabled = false;
                    txtCedula.Text = _L_Facturacion.CedCliente;
                    txtCedula.Enabled = false;
                    TxtTelefono.Text = _L_Facturacion.TlfCliente;
                    TxtTelefono.Enabled = false;
                    TxtCorreo.Text = _L_Facturacion.CorreoCliente;
                    TxtCorreo.Enabled = false;

                }

                else
                {

                    txtNombreCliente.Text = ClienteAnterior;
                    txtNombreCliente.Enabled = false;
                    txtCedula.Text = CedulaAnterior;
                    txtCedula.Enabled = false;
                    TxtTelefono.Text = TlfAnterior;
                    TxtTelefono.Enabled = false;
                    TxtCorreo.Text = CorreoAnterior;
                    TxtCorreo.Enabled = false;

                    _FrmMensajes.co = 2;
                    string mensaje = "No se encontró el cliente";
                    _FrmMensajes.avisomensaje(mensaje);
                    _FrmMensajes.ShowDialog();


                }


            }
        }
        private string RemoveLetters(string input)
        {
            // Usar LINQ para filtrar solo los caracteres que son dígitos
            return new string(input.Where(char.IsDigit).ToArray());
        }

        private void txtCedula_Leave(object sender, EventArgs e)
        {
            if (txtCedula.Text == "")
            {
                txtNombreCliente.Text = ClienteAnterior;
                txtNombreCliente.Enabled = false;
                txtCedula.Text = CedulaAnterior;
                txtCedula.Enabled = false;


            }
            else
            { 
            string cedula = RemoveLetters(txtCedula.Text);
            _L_Facturacion.BuscarCliente(cedula);
            if (_L_Facturacion.MostrarClientePag == true)
            {
                txtNombreCliente.Text = _L_Facturacion.NombreCliente;
                txtNombreCliente.Enabled = false;
                txtCedula.Text = _L_Facturacion.CedCliente;
                txtCedula.Enabled = false;
                TxtTelefono.Text = _L_Facturacion.TlfCliente;
                TxtTelefono.Enabled = false;
                TxtCorreo.Text = _L_Facturacion.CorreoCliente;
                TxtCorreo.Enabled = false;

            }

            else
            {
                txtNombreCliente.Text = ClienteAnterior;
                txtNombreCliente.Enabled = false;
                txtCedula.Text = CedulaAnterior;
                txtCedula.Enabled = false;
                TxtTelefono.Text = TlfAnterior;
                TxtTelefono.Enabled = false;
                TxtCorreo.Text = CorreoAnterior;
                TxtCorreo.Enabled = false;

                _FrmMensajes.co = 2;
                string mensaje = "No se encontró el cliente";
                _FrmMensajes.avisomensaje(mensaje);
                _FrmMensajes.ShowDialog();

            }

        }
    }

        public void FuncionSaldo0()
        {

            //Cursor = System.Windows.Forms.Cursors.WaitCursor;

            //registro Garantia
            _L_Facturacion.AgregarGarantia();

            // actualizar Fecha Ofrecida
            _D_DetalleOrden.ACTUALIZA_FECHAOFREC(TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision, DateTime.UtcNow);

            //mensaje de Dana
            DanaServis();

            //Cursor = System.Windows.Forms.Cursors.Default;


        }

        public void RegistroAbono_Auditor(string MontoAbonado)
        {
               _L_Facturacion.RegistarAbonoAuditor("013",TB_CAORDSER.NumOrdserv, MontoAbonado);
     
        }

        public string ProcesarPagos(double TotalAbono, double TotalSaldoOrdenConIgtf)
        {
            string concat = TB_CAORDSER.Cod_Sucursal + TB_CAORDSER.NumOrdserv + TB_CAORDSER.Revision;
            string rept = "";
            Conexion cn = new Conexion();
            SqlConnection connection = cn.LeerCadena();
            SqlCommand command = connection.CreateCommand();
            SqlTransaction transaction;
            transaction = connection.BeginTransaction();
            command.Connection = connection;
            command.Transaction = transaction;
            command.Parameters.Clear();
            command.CommandTimeout = 120;
            string Correlativo = "";
            Num_Factura = "";
            rollbackRealizado = false;
            try
            {

                MovInventarioo = false;    
                rept = _L_Facturacion.RegistarAbonos(TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision, TB_USUARIO.COD_USR, Convert.ToDouble(TB_TASA_Dolar.Tasa), command);
                if (rept == "SATISFACTORIO")   
                rept = _L_Facturacion.RegistarNotaCredito(DgvAbonos, txtCedula.Text.Substring(2, txtCedula.Text.Length - 2), NotaNumFactura, command);
                if (rept == "SATISFACTORIO")
                rept = _L_Facturacion.ActualizarSaldoNotaDevolucion(DgvAbonos, txtCedula.Text.Substring(2, txtCedula.Text.Length - 2), NotaNumOrden, command);

                // Ejecuto el movimiento de inventario 
                if (TB_CAORDSER.OrSer_Status == "004" && rept == "SATISFACTORIO") // PorPagar
                {
                    rept = MovInventario(command);
                    MovInventarioo = true;

                    // Cuando la orden es lentes de contacto rebajo la reserva; ejecuto la modificacion en TB_LENTESCONTACTO y RelacionMovimientosLC
                    if (TB_CAORDSER.Cod_DetVta == "02" & _D_DetalleOrden.TB_PARAMETRO("LCManejaExist") == "1")
                    {
                        string DiaActivo = _D_Inicio.DiaActivo().ToShortDateString();
                        DataSet DST_RelacionMovimientosLC = _D_DetalleOrden.RelacionMovimientosLC(TB_CAORDSER.NumOrdserv, "O", TB_CAORDSER.Cod_Sucursal, TB_USUARIO.COD_USR, DiaActivo, TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision, command);
                        DataSet DST_Stock_LC = _D_DetalleOrden.Stock_LC(TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision, "004", command);

                    }
                }


                // Registro los billetes 
                if (FalBod == true && rept == "SATISFACTORIO")
                {
                    foreach (DataRow fila in Dt_Billetes.Rows)
                    {
                        rept = _L_Facturacion.EnviarBilltesBD(TB_CAORDSER.NumOrdserv, fila["Tipo"].ToString(), fila["Monto_Bill"].ToString(), fila["Cod_Billete"].ToString(), (DateTime.Now).ToString("yyyy/MM/dd HH:mm"), TB_USUARIO.COD_EMPLEADO, "Guardar", command);
                    }
                }

                // Registro El Pago Movil
                if (rept == "SATISFACTORIO")
                {
                    Correlativo = _D_DetalleOrden.ID_PagoMovil(TB_CAORDSER.NumOrdserv, TB_CAORDSER.Cod_Sucursal, command);
                    rept = _L_Facturacion.RegistarPagoMovil(Dt_PagoMovil, TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision, command);

                }

                string PMAutomatico = _D_DetalleOrden.TB_PARAMETRO("PMAutomatico");

                // Verifico si esta lista Para Facturar 
                
                if (TotalAbono == TotalSaldoOrdenConIgtf)
                {
                    // Imprimo La Factura
                    if (rept == "SATISFACTORIO" )
                        if (_Impresora_Fiscal.VerficarConexionImpresoraFiscal() == false)
                        {
                            transaction.Rollback();
                            mensaje = _Impresora_Fiscal.stringBuilder.ToString();
                            rept = "Error";
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje(mensaje);
                            _FrmMensajes.ShowDialog();
                            btnCancelar1.PerformClick();
                         
                            return "";
                        }
                    rept = ImprimirFacturaFiscal(txtNumeroOrden.Text, txtCedula.Text, txtNombreCliente.Text, command);

                    // Imprimo el Pago Movil 
                    if (rept == "SATISFACTORIO" && PMAutomatico == "0")
                     rept = ImprimirCambio(Correlativo, Dt_PagoMovil, TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision, Num_Factura, command);

                }
                else
                {
                    // Imprimo el Pago Movil 
                    if (rept == "SATISFACTORIO" && PMAutomatico == "0")
                        rept = ImprimirCambio(Correlativo, Dt_PagoMovil, TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision, Num_Factura, command);
                }

                //Attempt to commit the transaction.
                if (rept == "SATISFACTORIO")
                    transaction.Commit();
                else
                if (transaction != null && !rollbackRealizado)
                {
                    transaction.Rollback();
                }

                //Cursor = System.Windows.Forms.Cursors.Default;
                return rept;

            }

            catch (Exception ex)
            {
                rept = string.Format("Error: {0}", ex.Message);
                return rept;

                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    // Este bloque catch manejará cualquier error que pueda haber ocurrido
                    // en el servidor que haría que la reversión fallara, en una conexión cerrada.

                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }

            }

            finally
            {
                try
                {
                 btnCancelar1.PerformClick();
                LimpiarNotasCredito();
                _D_DetalleOrden.Limpiar_TEMP_ABONO(TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision);
                Dt_PagoMovil.Clear();

                string OrSer_Statu = TB_CAORDSER.OrSer_Status;
                string Cod_Venta = TB_CAORDSER.Cod_Venta;

                _D_DetalleOrden.Datos_de_la_Orden(txtNumeroOrden.Text, TB_CAORDSER.Revision);

                if (rept == "SATISFACTORIO")
                {
                    if (TB_CAORDSER.OrSer_Saldo == 0)
                    {
                        FuncionSaldo0();// Funcion de dana, garantia, actualizar Fecha Ofrecida 

                        if (TB_CAORDSER.Cod_Venta == "002" && TB_CAORDSER.Asegurada == true && _D_DetalleOrden.TB_PARAMETROSPGE("ImprimeContrato") == "1")
                        {
                            // Para inmprimir o mostrar contrato de garantia solo cuando es trabajo convencio
                            RepContrato(concat);
                        }
                    }
                    if (TB_CAORDSER.OrSer_Saldo != 0)
                    {
                        mensaje = "Se ha realizado correctamente el abono";
                        _FrmMensajes.co = 1;
                        _FrmMensajes.avisomensaje(mensaje);
                        _FrmMensajes.ShowDialog();
                        LimpiarNotasCredito();

                        // Validamos si este es necesario imprimir el Reporte de Orden o de Contacto Para imprimirlo junto con el Reporte de Abono como un Sub Reporte 
                        if (OrSer_Statu == "004" && Cod_Venta != "001") // Estatus PorPagar , se ejecuto la funcion y es diferente de Venta directa
                        {
                            // Trabajo convencional reservado = 08 
                            if (TB_CAORDSER.Cod_DetVta != "08") // Solo se muestra o se imprime el reporte de la orden si no es trabajo convencional reservado 
                            {
                                // Si es trabajo de contacto se muestra este reporte 
                                if (TB_CAORDSER.Cod_DetVta == "02") // Si se procesa uan orden de contacto se muestra reporte de contacto  
                                {
                                    _FrmMostrarReporte.setParametros(concat);
                                    _FrmMostrarReporte.ConfigRep(true, true);

                                    if (_D_DetalleOrden.ParametroImpresion() == "1") // Si el parametro de impresion es 1 entonces imprime el reporte 
                                    {
                                        _FrmMostrarReporte.imprimir();

                                    }
                                    else
                                    {
                                        _FrmMostrarReporte.ShowDialog();

                                    }

                                }
                                else // si es otro tipo de trabajo 
                                {
                                    _FrmMostrarReporte.setParametros(concat);
                                    _FrmMostrarReporte.ConfigRep(true, false);

                                    if (_D_DetalleOrden.ParametroImpresion() == "1") // Si el parametro de impresion es 1 entonces imprime el reporte 
                                    {
                                        _FrmMostrarReporte.imprimir();

                                    }
                                    else
                                    {
                                        _FrmMostrarReporte.ShowDialog();

                                    }


                                }
                            }

                            else
                            {
                                _FrmMostrarReporte.setParametros(concat);
                                _FrmMostrarReporte.ConfigRep(false, false);

                                if (_D_DetalleOrden.ParametroImpresion() == "1") // Si el parametro de impresion es 1 entonces imprime el reporte 
                                {
                                    _FrmMostrarReporte.imprimir();

                                }
                                else
                                {
                                    _FrmMostrarReporte.ShowDialog();

                                }
                            }
                        }
                        else
                        {
                            _FrmMostrarReporte.setParametros(concat);
                            _FrmMostrarReporte.ConfigRep(false, false);

                            if (_D_DetalleOrden.ParametroImpresion() == "1") // Si el parametro de impresion es 1 entonces imprime el reporte 
                            {
                                _FrmMostrarReporte.imprimir();

                            }
                            else
                            {
                                _FrmMostrarReporte.ShowDialog();

                            }
                        }

                        //7//// Reporte de Declaracion se imprime si se abono la orden y el status antes era por pagar 
                        if (OrSer_Statu == "004" && (Cod_Venta == "003" | Cod_Venta == "002") && (_D_DetalleOrden.TB_PARAMETRO("ImprimeDocResp") =="1")) // Estatus PorPagar , se ejecuto la funcion es trabajo convencional o reparacion y el parametro de imprimirDeclaracion sea 1 
                        {
                            if (TB_CAORDSER.Cod_DetVta != "08")
                            {
                                ImprimirDeclaracion_CristalPropio_MonturaPropia();
                            }
                            else
                            {
                                _FrmMensajes.co = 1;
                                _FrmMensajes.avisomensaje("El reporte de la Orden de Servicio se debe imprimir cuando se asigne el RX correspondiente");
                                _FrmMensajes.ShowDialog();

                            }

                        }

                    }
                  else
                  {

                    //7//// Reporte de Declaracion se imprime si se facturo la orden y el status antes era por pagar 
                    if (OrSer_Statu == "004" && (Cod_Venta == "003" | Cod_Venta == "002") && (_D_DetalleOrden.TB_PARAMETRO("ImprimeDocResp") == "1")) // Estatus PorPagar , se ejecuto la funcion es trabajo convencional o reparacion y el parametro de imprimirDeclaracion sea 1 
                    {
                            if (TB_CAORDSER.Cod_DetVta != "08")
                            {
                                ImprimirDeclaracion_CristalPropio_MonturaPropia();
                            }
                            else
                            {
                                _FrmMensajes.co = 1;
                                _FrmMensajes.avisomensaje("El reporte de la Orden de Servicio se debe imprimir cuando se asigne el RX correspondiente");
                                _FrmMensajes.ShowDialog();

                            }

                    }

                        //7//// Reporte de Orden y Rep Contacto
                        if (OrSer_Statu == "004" && Cod_Venta != "001") // Estatus PorPagar , se ejecuto la funcion y es diferente de Venta directa
                    {
                        RepOrden(concat);// reporte de orden se emite cuando la orden tiene status por pagar 
                    }

                  }

                }

                }
                catch (Exception ex)
                {
                    // Manejo de excepciones dentro del bloque finally
                    Console.WriteLine("Ocurrió un error en la funcion ProcesarPagos, bloque finally: " + ex.Message);
                    // Puedes registrar el error o mostrar un mensaje al usuario
                }
            }


        }


        private void CbxBanco_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxBanco.SelectedIndex != -1)
            {

                Validar_FalBod(CbxBanco, txtTranferencia);
                //Valido que selecion estos metodos de pago para entrar en esta funcion
                if (CbxMetodosPago2.Text == "Tarjeta de Credito" | CbxMetodosPago2.Text == "Debito" | CbxMetodosPago2.Text == "Cashea")
                {
                    PagarCASHEA(CbxBanco, txtMonto2Bs);
                }


            }
        }

        private void Validar_FalBod(System.Windows.Forms.ComboBox Banco, System.Windows.Forms.TextBox Tranfere)
        {
            if (Banco.Text == " FAL BOD" || Banco.SelectedValue == "007")
            {
                Tranfere.Text = "0000";
                Tranfere.Enabled = false;

            }
            else
            {
                Tranfere.Text = "";
                Tranfere.Enabled = true;

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

        public void RepOrden(string concat)
        {

            //// Imprimir o mosotrar reporte de orden
            //// Trabajo convencional reservado = 08 
            //if (TB_CAORDSER.Cod_DetVta != "08") // Solo se muestra o se imprime el reporte de la orden si no es trabajo convencional reservado 
            //{
            //    // Si es trabajo de contacto se muestra este reporte 
            //    if (TB_CAORDSER.Cod_DetVta == "02") // Si se procesa uan orden de contacto se muestra reporte de contacto  
            //    {
            //        _FrmRepOrdenTContact.setParametros(concat);
            //        _FrmRepOrdenTContact.ConfigRep();

            //        if (_D_DetalleOrden.ParametroImpresion() == "1") // Si el parametro de impresion es 1 entonces imprime el reporte 
            //        {
            //            _FrmRepOrdenTContact.imprimir();

            //        }
            //        else
            //        {
            //            _FrmRepOrdenTContact.ShowDialog();

            //        }

            //    }
            //    else // si es otro tipo de trabajo 
            //    {
            //        _FrmRepOrden.setParametros(concat);
            //        _FrmRepOrden.ConfigRep();

            //        if (_D_DetalleOrden.ParametroImpresion() == "1") // Si el parametro de impresion es 1 entonces imprime el reporte 
            //        {
            //            _FrmRepOrden.imprimir();

            //        }
            //        else
            //        {
            //            _FrmRepOrden.ShowDialog();

            //        }

            //    }

            //}
            // Si el codigo detalle venta es igual a 08 entonces no hace nada 

            if (TB_CAORDSER.Cod_DetVta != "08") // Solo se muestra o se imprime el reporte de la orden si no es trabajo convencional reservado 
            {
                if (TB_CAORDSER.Cod_DetVta == "02") // Si se procesa uan orden de contacto se muestra reporte de contacto  
                {
                    _FrmRepOrden.setParametros(concat);
                    _FrmRepOrden.ConfigRep(false,true);

                    if (_D_DetalleOrden.ParametroImpresion() == "1") // Si el parametro de impresion es 1 entonces imprime el reporte 
                    {
                        _FrmRepOrden.imprimir();

                    }
                    else
                    {
                        _FrmRepOrden.ShowDialog();

                    }


                }
                else
                {
                    _FrmRepOrden.setParametros(concat);
                    _FrmRepOrden.ConfigRep(true, false);

                    if (_D_DetalleOrden.ParametroImpresion() == "1") // Si el parametro de impresion es 1 entonces imprime el reporte 
                    {
                        _FrmRepOrden.imprimir();

                    }
                    else
                    {
                        _FrmRepOrden.ShowDialog();

                    }

                }

            }


        }

        public void RepContrato(string concat)
        {

            // Para la impresion del reporte del contrato 
            _FrmRepContrat.setParametros(concat);
            _FrmRepContrat.ConfigRep();
            if (_D_DetalleOrden.ParametroImpresion() == "1")
            {
                _FrmRepContrat.imprimir();

            }
            else
            {
                _FrmRepContrat.ShowDialog();

            }



        }

        public string MovInventario(SqlCommand command)
        {
            // Para hacer el movimiento de inventario 
            _LAnulacion.CargarDetalleOrd(TB_CAORDSER.NumOrdserv, "O", "006", command); // Se coloca antes de la impresion de manera que si no encuentra la impresora el cacth no nos salte nada importante
            if (_LAnulacion.GuardoMovimientoArticulo == false)
            {
                mensaje = "Se produjo error al hacer el movimiento.";
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(mensaje);
                _FrmMensajes.ShowDialog();
            }
            else
            {
                mensaje = "SATISFACTORIO";
            }

            return mensaje;
        }

        public string Verificar_Existencia (string Numero_orden ,string Cod_DetVta, string OrSer_Statu, SqlCommand command= null)
        {
           string Resp= _LAnulacion.Verificar_Existencia_Inv(Numero_orden, Cod_DetVta, OrSer_Statu, command);
            if (Resp!= "SATISFACTORIO")
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(Resp);
                _FrmMensajes.ShowDialog();
            }

            return Resp;
        }

        public void HabilitacionControl(string Able)
        {
            switch (Able)
            {
                case "Habilitar":
                    this.DgvListadoOrdenes.Enabled = true;
                    this.BtnClientePagador.Enabled = true;
                    this.btnIngresar.Enabled = true;
                    LLenar_Datos_Convencional();
                    LLenar_Datos_Orden();
                    this.btnPrincipal.Enabled = true;
                    break;

                case "Bloquear":
                    this.DgvListadoOrdenes.Enabled = false;
                    this.BtnClientePagador.Enabled = false;
                    this.btnIngresar.Enabled = false;
                    this.TxtCambioRef.Enabled = false;
                    this.textBox8.Enabled = false;
                    this.btnExamen.Enabled = false;
                    this.btnDetalleOrden.Enabled = false;

                    break;

            }


        }

        //Comentar
        private void txtNroNotaDevolucion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (rd_NotaDevolucion.Checked || rd_NunOrden.Checked)
                {
                    if (txtNroNotaDevolucion.Text != "")
                    {
                        _L_Facturacion.BuscarNotasDevolucion(txtNroNotaDevolucion.Text, rd_NotaDevolucion.Checked, txtCedula.Text.Substring(2, txtCedula.Text.Length - 2), DgvNotasDevolicion);
                        mensaje = _L_Facturacion.stringBuilder.ToString();
                        if (DgvNotasDevolicion.Rows.Count > 0)
                        {
                            CrearObjetosNotasDevolucion();

                        }

                        else
                        {
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje(mensaje);
                            _FrmMensajes.ShowDialog();
                        }

                    }

                    else
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("Debe ingresar un numero de 7 digitos");
                        _FrmMensajes.ShowDialog();
                    }
                }

                //else
                //{
                //    _FrmMensajes.co = 2;
                //    _FrmMensajes.avisomensaje("Seleciones el tipo de documento");
                //    _FrmMensajes.ShowDialog();
                //}
            }
        }

        //Comentar
        public void CrearObjetosNotasDevolucion()
        {
            try
            {

                DataGridViewCheckBoxColumn RdButom = new DataGridViewCheckBoxColumn();
                RdButom.Name = "RdButom";
                RdButom.Width = 50;
                RdButom.HeaderText = "";
                DgvNotasDevolicion.Columns.Add(RdButom);

                FormatoDataGridNotasDevolucion();

            }

            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }
        }

        //Comentar
        private void FormatoDataGridNotasDevolucion()
        {

            try
            {

                //Centrar todas las colucnas 
                DgvNotasDevolicion.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //asignar Nombres a cada colucna 
                DgvNotasDevolicion.Columns["NRONOTA"].HeaderText = "Número Nota";
                DgvNotasDevolicion.Columns["NumOrdserv"].HeaderText = "Número de Orden";
                DgvNotasDevolicion.Columns["Motivo"].HeaderText = "Motivo";
                DgvNotasDevolicion.Columns["SaldoNota"].HeaderText = "Saldo";

                //Ancho de columna

                DgvNotasDevolicion.Columns["NRONOTA"].Width = 125;
                DgvNotasDevolicion.Columns["NumOrdserv"].Width = 160;
                DgvNotasDevolicion.Columns["Motivo"].Width = 120;
                DgvNotasDevolicion.Columns["SaldoNota"].Width = 80;
                DgvNotasDevolicion.Columns["RdButom"].Width = 57;

                //Bloquear Columna 
                DgvNotasDevolicion.Columns["NRONOTA"].ReadOnly = true;
                DgvNotasDevolicion.Columns["NumOrdserv"].ReadOnly = true;
                DgvNotasDevolicion.Columns["Motivo"].ReadOnly = true;
                DgvNotasDevolicion.Columns["SaldoNota"].ReadOnly = true;

                //Posicion  
                DgvNotasDevolicion.Columns["NRONOTA"].DisplayIndex = 0;
                DgvNotasDevolicion.Columns["NumOrdserv"].DisplayIndex = 1;
                DgvNotasDevolicion.Columns["Motivo"].DisplayIndex = 2;
                DgvNotasDevolicion.Columns["RdButom"].DisplayIndex = 4;
                DgvNotasDevolicion.Columns["SaldoNota"].DisplayIndex = 3;

                DgvNotasDevolicion.Columns["SaldoNota"].Visible = true;
                DgvNotasDevolicion.Columns["Motivo"].Visible = false;

                DgvNotasDevolicion.Columns["SaldoNota"].DefaultCellStyle.Format = "##,##0.00";
            }


            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }
        }

        //Comentar
        private void btnCancelar4_Click(object sender, EventArgs e)
        {
            LimpiaVariablesIdAbonoPagoMovil();
            LimpiarNotasDevolucion();
            VisualizarPanel("MostrarPanelPrincipal");
            LimpiarTxbox();
        }

        //Comentar
        public void LimpiarNotasDevolucion()
        {

            DgvNotasDevolicion.DataSource = "";
            DgvNotasDevolicion.DataMember = "";

            var dataGridViewColumn = DgvNotasDevolicion.Columns["RdButom"];

            if (dataGridViewColumn != null && dataGridViewColumn.Visible)

            {

                DgvNotasDevolicion.Columns.RemoveAt(DgvNotasDevolicion.Columns.Count - 1);
            }

            txtNroNotaDevolucion.Text = "";
            PosicionInicalNotaDevolucion = "";
            txtNroNotaDevolucion.Enabled = true;
            rd_NotaDevolucion.Enabled = true;
            rd_NotaDevolucion.Checked = false;
            rd_NunOrden.Enabled = true;
            rd_NunOrden.Checked = false;
            txtBsNotaDevolucion.Enabled = false;
            btnProcesar4.Enabled = false;

        }

        private void DgvNotasDevolicion_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //////Esta funcion  sirve para marcar y desmarcar la lista del DataGrid de NotasDevolucion 
            string PosicionActual = DgvNotasDevolicion.CurrentRow.Index.ToString();
            if (DgvNotasDevolicion.Columns[e.ColumnIndex].Name == "RdButom")
            {

                for (int Indice = 0; Indice <= DgvNotasDevolicion.Rows.GetLastRow(DataGridViewElementStates.Displayed); Indice++)
                {
                    if (PosicionInicalNotaDevolucion == "")
                    {
                        DgvNotasDevolicion.Rows[Indice].Cells["RdButom"].Value = false;
                    }

                }

                if (PosicionInicalNotaDevolucion == "")
                {
                    txtBsNotaDevolucion.Text = string.Format("{0:#,0.00}", Convert.ToDecimal(DgvNotasDevolicion.CurrentRow.Cells["SaldoNota"].Value.ToString()));
                    NotaNumOrden = DgvNotasDevolicion.CurrentRow.Cells["NumOrdserv"].Value.ToString();
                    NotaNumNotaDevolucion = DgvNotasDevolicion.CurrentRow.Cells["NRONOTA"].Value.ToString();
                    MotivoNotaDevolucion = DgvNotasDevolicion.CurrentRow.Cells["Motivo"].Value.ToString();
                    DgvNotasDevolicion.CurrentRow.Cells["RdButom"].Value = true;
                    txtBsNotaDevolucion.Enabled = true;
                    btnProcesar4.Enabled = true;
                    txtNroNotaDevolucion.Enabled = false;
                    rd_NotaDevolucion.Enabled = false;
                    rd_NunOrden.Enabled = false;

                }

                if (PosicionInicalNotaDevolucion != "" && PosicionActual != PosicionInicalNotaDevolucion)

                {
                    txtBsNotaDevolucion.Text = string.Format("{0:#,0.00}", Convert.ToDecimal(DgvNotasDevolicion.CurrentRow.Cells["SaldoNota"].Value.ToString()));
                    NotaNumOrden = DgvNotasDevolicion.CurrentRow.Cells["NumOrdserv"].Value.ToString();
                    NotaNumNotaDevolucion = DgvNotasDevolicion.CurrentRow.Cells["NRONOTA"].Value.ToString();
                    MotivoNotaDevolucion = DgvNotasDevolicion.CurrentRow.Cells["Motivo"].Value.ToString();
                    DgvNotasDevolicion.CurrentRow.Cells["RdButom"].Value = true;
                    txtBsNotaDevolucion.Enabled = true;
                    btnProcesar4.Enabled = true;
                    txtNroNotaDevolucion.Enabled = false;
                    rd_NotaDevolucion.Enabled = false;
                    rd_NunOrden.Enabled = false;

                }

                if (PosicionInicalNotaDevolucion != "" && PosicionActual == PosicionInicalNotaDevolucion)
                {
                    txtBsNotaDevolucion.Text = "";
                    NotaNumOrden = "";
                    NotaNumNotaDevolucion = "";
                    MotivoNotaDevolucion = "";
                    DgvNotasDevolicion.CurrentRow.Cells["RdButom"].Value = false;
                    txtBsNotaDevolucion.Enabled = false;
                    btnProcesar4.Enabled = false;
                    txtNroNotaDevolucion.Enabled = true;
                    rd_NotaDevolucion.Enabled = true;
                    rd_NunOrden.Enabled = true;
                    PosicionInicalNotaDevolucion = "Repetido";
                    PosicionActual = "-4";
                }

            }
        }

        private void btnProcesar4_Click(object sender, EventArgs e)
        {
            LimpiaVariablesIdAbonoPagoMovil();
            double TotalNota = 0.00;
            double TotalOrden;
            TotalOrden = Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos));



            for (int Indice = 0; Indice <= DgvNotasDevolicion.Rows.GetLastRow(DataGridViewElementStates.Displayed); Indice++)
            {
                if (DgvNotasDevolicion.Rows[Indice].Cells["NRONOTA"].Value.ToString() == NotaNumNotaDevolucion && DgvNotasDevolicion.Rows[Indice].Cells["NumOrdserv"].Value.ToString() == NotaNumOrden)
                {

                    TotalNota = Convert.ToDouble(DgvNotasDevolicion.Rows[Indice].Cells["SaldoNota"].Value.ToString());

                }


            }

            if (TotalOrden >= (txtBsNotaDevolucion.Text == "" ? (Double)0.00 : Convert.ToDouble(txtBsNotaDevolucion.Text.Replace(".", ""))))
            {

                if (TotalNota >= (txtBsNotaDevolucion.Text == "" ? (Double)0.00 : Convert.ToDouble(txtBsNotaDevolucion.Text.Replace(".",""))) && (txtBsNotaDevolucion.Text.Replace(".", "") == "" ? (Double)0.00 : Convert.ToDouble(txtBsNotaDevolucion.Text.Replace(".", ""))) > 0)
                {

                    _L_Facturacion.GuardarAbonoGrid(2,Dt_Abonos, CbxMetodosPago.Text, "Bolivares", "", txtBsNotaDevolucion.Text, "", DtpFecha.Value.ToString(), CbxMetodosPago.SelectedValue.ToString(), "000", "", "", "", "", "", "", "", NotaNumNotaDevolucion);
                    BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);
                    //-----------ConvertirBolivares---------------------------
                    //txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);

                    LimpiarTxbox();
                    VisualizarPanel("MostrarPanelPrincipal");
                    LimpiarNotasDevolucion();
                }

                else
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("El monto debe ser igual o menor que el saldo total de la nota");
                    _FrmMensajes.ShowDialog();

                }



            }

            else
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("El monto debe ser igual o menor al saldo de la orden");
                _FrmMensajes.ShowDialog();

            }


        }

        private void PagarCASHEA(System.Windows.Forms.ComboBox Banco, System.Windows.Forms.TextBox Bolivares)
        {

            if (Banco.Text == "CASHEA" || Banco.SelectedValue == "110")
            {
                if (TB_CAORDSER.OrSer_Status == "004") //por pagar
                {
                    Double MenorMinimoAbono = Math.Round(0.50 * Convert.ToDouble((String.Format(CultureInfo.InvariantCulture, "{0:0.00}", TB_CAORDSER.VtaTotal).Replace(".", ",")).Replace(".", ",")), 2);
                    Double TotalOrden = Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos));
                    // Si el saldo que pendiente es mayor al 50% de la orden 
                    if (TotalOrden >= MenorMinimoAbono)
                    {
                        BolivaresConveridos(MenorMinimoAbono, Bolivares);
                        //Bolivares.Text = Convert.ToString(MenorMinimoAbono);
                        //Bolivares.Enabled = false;

                    }
                    // Si el saldo que pendiente es menor al 50% de la orden 
                    else
                    {
                        BolivaresConveridos(TotalOrden, Bolivares);
                        //Bolivares.Text = Convert.ToString(TotalOrden);
                        //Bolivares.Enabled = false;
                    }

                }
                else
                {
                    VisualizarPanel("MostrarPanelPrincipal");
                    LimpiarTxbox();

                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("No se permite utilizar Cashea en ordenes abonadas");
                    _FrmMensajes.ShowDialog();


                }
            }
            else
            {
                Bolivares.Enabled = true;
                BolivaresConveridos(Convert.ToDouble(_L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos)), txtMontoBs);
                //-----------ConvertirBolivares---------------------------
                //txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);
                Bolivares.Text = Convert.ToString(Convert.ToDouble(txtMontoBs.Text.Replace(".","")));
                Bolivares.Text = string.Format("{0:#,0.00}", Convert.ToDecimal(Bolivares.Text));
            }

        }


        private bool ValidarPagosCASHEA(System.Windows.Forms.DataGridView Dt_Abono)
        {
            bool CASHEA = false;
            foreach (DataGridViewRow row in Dt_Abono.Rows)
            {
                string Codigo = row.Cells["CodBanco"].Value.ToString();
                string NombreBanco = row.Cells["Banco"].Value.ToString();
                if (Codigo == "110" | NombreBanco == "CASHEA")
                {
                    CASHEA = true;
                    Medio_Pago = "CASHEA";
                }

            }
            return CASHEA;
        }



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        public void FormatoDgvBilletes()
        {
            try
            {

                //asignar Nombres a cada colucna 


                DgvBilletes.Columns["Cod_Billete"].HeaderText = "Código de billete";
                DgvBilletes.Columns["Monto_Bill"].HeaderText = "Monto de billete";
                DgvBilletes.Columns["Tipo"].HeaderText = "Tipo";


                //Ancho de columna

                DgvBilletes.Columns["Cod_Billete"].Width = 150;
                DgvBilletes.Columns["Monto_Bill"].Width = 250;


                //Bloquear Columna 
                DgvBilletes.Columns["Cod_Billete"].ReadOnly = true;
                DgvBilletes.Columns["Monto_Bill"].ReadOnly = true;


                //Posicion  
                DgvBilletes.Columns["Cod_Billete"].DisplayIndex = 0;
                DgvBilletes.Columns["Monto_Bill"].DisplayIndex = 1;
                DgvBilletes.Columns["Tipo"].DisplayIndex = 2;



                //Alineación

                DgvBilletes.Columns["Cod_Billete"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvBilletes.Columns["Monto_Bill"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //oculta columna de tipo
                DgvBilletes.Columns["Tipo"].Visible = false;




            }


            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }




        }







        public void EstructuraDgvBilletes()
        {//Le da estructura al grid y se agrega la columna del boton 

            try
            {
                DgvBilletes.Columns["Cod_Billete"].SortMode = DataGridViewColumnSortMode.NotSortable;
                DgvBilletes.Columns["Monto_Bill"].SortMode = DataGridViewColumnSortMode.NotSortable;
                DgvBilletes.Columns["Tipo"].SortMode = DataGridViewColumnSortMode.NotSortable;


                DataGridViewButtonColumn Btnclm = new DataGridViewButtonColumn();


                Btnclm.HeaderText = "Borrar";
                Btnclm.Name = "Eliminar";
                Btnclm.UseColumnTextForButtonValue = true;
                DgvBilletes.Columns.Add(Btnclm);

            }


            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }
        }

        private void DgvBilletes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // cuando se pulsa el boton eliminar del grid se elmina la fila
            if (DgvBilletes.Columns[e.ColumnIndex].Name == "Eliminar")
            {


                DgvBilletes.Rows.Remove(DgvBilletes.CurrentRow);


            }
        }

        private void DgvBilletes_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            //Para colocar un icono en el boton del grid
            if (e.ColumnIndex >= 0 && this.DgvBilletes.Columns[e.ColumnIndex].Name == "Eliminar" && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                e.CellStyle.BackColor = Color.Red;
                DataGridViewButtonCell celBoton = this.DgvBilletes.Rows[e.RowIndex].Cells["Eliminar"] as DataGridViewButtonCell;
                Icon iconAtomico;

                if (ModoClaro == false)
                {
                    iconAtomico = new Icon(Environment.CurrentDirectory + @"\\Eliminar_Billete2.ico");
                }
                else
                {
                    iconAtomico = new Icon(Environment.CurrentDirectory + @"\\Eliminar_Billete.ico");
                }

                e.Graphics.DrawIcon(iconAtomico, e.CellBounds.Left + 0, e.CellBounds.Top + 3);

                //Le da tamano al icono
                this.DgvBilletes.Rows[e.RowIndex].Height = iconAtomico.Height + 0;
                this.DgvBilletes.Columns[e.ColumnIndex].Width = iconAtomico.Width + 2;

                e.Handled = true;

            }
        }

        private void CbxBillete_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string Moneda = "";

            if (CbxMoneda.Text == "Dólares")
            {

                Moneda = "DÓLAR";
            }
            if (CbxMoneda.Text == "Euros")
            {
                Moneda = "EURO";

            }
            // Cada vez que cambie la seleccion del combobobox se ejecuta esta funcion para insertar en el grid
            // Se consulta si existe el billete
            if (TxtCodBillete.Text.Replace(" ","")== "" | TxtCodBillete.Text.Replace(" ", "")== null)
            {
                return;
            }
            if (_D_DetalleOrden.ConsultarBilletes(TB_CAORDSER.NumOrdserv, TxtCodBillete.Text, Moneda) == "HABILITADO")
            {
                // Si no existe entonces se encuentra habilitado para insertar

                CargarDgvBilletes(CbxBillete.Text, TxtCodBillete.Text, Moneda);
                TxtCodBillete.Text = "";
                CbxBillete.SelectedIndex = -1;
            }

            else
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("El billete se encuentra duplicado");
                _FrmMensajes.ShowDialog();

            }




        }



        public void CargarDgvBilletes(string MontoBillete, string CodBillete, string TipoMoneda)
        {
            LimpiarDgvBilletes();
            DgvBilletes.DataSource = Dt_Billetes;
            FormatoDgvBilletes();
            EstructuraDgvBilletes();

            _L_Facturacion.GuardarBilletesGrid(Dt_Billetes, CodBillete, MontoBillete, TipoMoneda);

        }


        public void LimpiarDgvBilletes()
        {
            DgvBilletes.DataSource = "";
            DgvBilletes.DataMember = "";


            var dataGridViewColumn2 = DgvBilletes.Columns["Eliminar"];
            if (dataGridViewColumn2 != null && dataGridViewColumn2.Visible)

            {

                DgvBilletes.Columns.RemoveAt(DgvBilletes.Columns.Count - 1);
            }

        }

        public void LimpiarCamposBill()
        {
            TxtCodBillete.Text = " ";
            CbxBillete.SelectedIndex = -1;

        }

        private void btnPrincipal_CheckedChanged(object sender, EventArgs e)
        {
            tabControl.SelectTab(0);

            //KEDWIN
            FrmListaOrdenes.kedInstancia.BtnCancelar.Visible = true;
        }
        private void btnExamen_CheckedChanged(object sender, EventArgs e)
        {
            tabControl.SelectTab(1);

            //KEDWIN
            FrmListaOrdenes.kedInstancia.BtnCancelar.Visible = false;


            DgvFormula.ClearSelection();
            DgvObservaMon.ClearSelection();
            DgvMontura.ClearSelection();
            DgvArticulo.ClearSelection();
            DgvObservaconExamen.ClearSelection();
            DgvColoracion.ClearSelection();
            DgvMedidasMontura.ClearSelection();
            DgvMimesys.ClearSelection();
            DgvTotales.ClearSelection();
        }

        private void btnDetalleOrden_CheckedChanged(object sender, EventArgs e)
        {
            tabControl.SelectTab(2);

            //KEDWIN
            FrmListaOrdenes.kedInstancia.BtnCancelar.Visible = false;


            DgvFormula.ClearSelection();
            DgvObservaMon.ClearSelection();
            DgvMontura.ClearSelection();
            DgvArticulo.ClearSelection();
            DgvObservaconExamen.ClearSelection();
            DgvColoracion.ClearSelection();
            DgvMedidasMontura.ClearSelection();
            DgvMimesys.ClearSelection();
            DgvTotales.ClearSelection();

            // Poner la fila de subTotal verde 
            System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml("#07a79b");
            foreach (DataGridViewRow row in DgvTotales.Rows)
            {
                if (row.Cells["ReciboPago"].Value.ToString() == "SubTotal")
                {

                    row.DefaultCellStyle.BackColor = color;
                    row.DefaultCellStyle.ForeColor = System.Drawing.Color.White;

                }

                row.Height = 25;
            }

        }

        private void FormatoExamen()
        {
            try
            {

                //Centrar todas las colucnas 
                DgvFormula.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                DgvFormula.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

                if (TxtTrabajo.Text == "CONVENCIONAL")
                {
                    //asignar Nombres a cada colucna 
                    DgvFormula.Columns["Nombre"].HeaderText = "";
                    DgvFormula.Columns["Esfera"].HeaderText = "Esfera";
                    DgvFormula.Columns["Cilindro"].HeaderText = "Cilindro";
                    DgvFormula.Columns["Eje"].HeaderText = "Eje";
                    DgvFormula.Columns["ADD"].HeaderText = "ADD";
                    DgvFormula.Columns["Lejos"].HeaderText = "Lejos";
                    DgvFormula.Columns["Cerca"].HeaderText = "Cerca";
                    DgvFormula.Columns["Altura"].HeaderText = "Altura";
                    DgvFormula.Columns["Vision"].HeaderText = "Visión";

                    //Ancho de columna
                    DgvFormula.Columns["Nombre"].Width = 155;
                    DgvFormula.Columns["Esfera"].Width = 110;
                    DgvFormula.Columns["Cilindro"].Width = 110;
                    DgvFormula.Columns["Eje"].Width = 110;
                    DgvFormula.Columns["ADD"].Width = 110;
                    DgvFormula.Columns["Lejos"].Width = 110;
                    DgvFormula.Columns["Cerca"].Width = 110;
                    DgvFormula.Columns["Altura"].Width = 110;
                    DgvFormula.Columns["Vision"].Width = 127;


                    DgvFormula.Columns["Nombre"].ReadOnly = true;
                    DgvFormula.Columns["Esfera"].ReadOnly = true;
                    DgvFormula.Columns["Cilindro"].ReadOnly = true;
                    DgvFormula.Columns["Eje"].ReadOnly = true;
                    DgvFormula.Columns["ADD"].ReadOnly = true;
                    DgvFormula.Columns["Lejos"].ReadOnly = true;
                    DgvFormula.Columns["Cerca"].ReadOnly = true;
                    DgvFormula.Columns["Altura"].ReadOnly = true;
                    DgvFormula.Columns["Vision"].ReadOnly = true;

                    DgvFormula.Columns["Nombre"].SortMode = DataGridViewColumnSortMode.NotSortable;
                    DgvFormula.Columns["Esfera"].SortMode = DataGridViewColumnSortMode.NotSortable;
                    DgvFormula.Columns["Cilindro"].SortMode = DataGridViewColumnSortMode.NotSortable;
                    DgvFormula.Columns["Eje"].SortMode = DataGridViewColumnSortMode.NotSortable;
                    DgvFormula.Columns["ADD"].SortMode = DataGridViewColumnSortMode.NotSortable;
                    DgvFormula.Columns["Lejos"].SortMode = DataGridViewColumnSortMode.NotSortable;
                    DgvFormula.Columns["Cerca"].SortMode = DataGridViewColumnSortMode.NotSortable;
                    DgvFormula.Columns["Altura"].SortMode = DataGridViewColumnSortMode.NotSortable;
                    DgvFormula.Columns["Vision"].SortMode = DataGridViewColumnSortMode.NotSortable;

                    //quitar seleccion por defecto de datagrid
                    DgvFormula.ClearSelection();

                }

                if (TxtTrabajo.Text == "CONTACTO")
                {

                    //asignar Nombres a cada colucna 
                    DgvFormula.Columns["Nombre"].HeaderText = "";
                    DgvFormula.Columns["CurvaBase"].HeaderText = "Curva Base";
                    DgvFormula.Columns["Diametro"].HeaderText = "Diametro";
                    DgvFormula.Columns["Esfera"].HeaderText = "Esfera";
                    DgvFormula.Columns["Cilindro"].HeaderText = "Cilindro";
                    DgvFormula.Columns["Eje"].HeaderText = "Eje";
                    DgvFormula.Columns["ADD"].HeaderText = "ADD";
                    DgvFormula.Columns["Color"].HeaderText = "Color";

                    //Ancho de columna
                    DgvFormula.Columns["Nombre"].Width = 170;
                    DgvFormula.Columns["CurvaBase"].Width = 130;
                    DgvFormula.Columns["Diametro"].Width = 130;
                    DgvFormula.Columns["Esfera"].Width = 130;
                    DgvFormula.Columns["Cilindro"].Width = 130;
                    DgvFormula.Columns["Eje"].Width = 120;
                    DgvFormula.Columns["ADD"].Width = 120;
                    DgvFormula.Columns["Color"].Width = 120;

                    DgvFormula.Columns["Nombre"].ReadOnly = true;
                    DgvFormula.Columns["CurvaBase"].ReadOnly = true;
                    DgvFormula.Columns["Diametro"].ReadOnly = true;
                    DgvFormula.Columns["Esfera"].ReadOnly = true;
                    DgvFormula.Columns["Cilindro"].ReadOnly = true;
                    DgvFormula.Columns["Eje"].ReadOnly = true;
                    DgvFormula.Columns["ADD"].ReadOnly = true;
                    DgvFormula.Columns["Color"].ReadOnly = true;

                    DgvFormula.Columns["Nombre"].SortMode = DataGridViewColumnSortMode.NotSortable;
                    DgvFormula.Columns["CurvaBase"].SortMode = DataGridViewColumnSortMode.NotSortable;
                    DgvFormula.Columns["Diametro"].SortMode = DataGridViewColumnSortMode.NotSortable;
                    DgvFormula.Columns["Esfera"].SortMode = DataGridViewColumnSortMode.NotSortable;
                    DgvFormula.Columns["Cilindro"].SortMode = DataGridViewColumnSortMode.NotSortable;
                    DgvFormula.Columns["Eje"].SortMode = DataGridViewColumnSortMode.NotSortable;
                    DgvFormula.Columns["ADD"].SortMode = DataGridViewColumnSortMode.NotSortable;
                    DgvFormula.Columns["Color"].SortMode = DataGridViewColumnSortMode.NotSortable;
                    //quitar seleccion por defecto de datagrid
                    DgvFormula.ClearSelection();
                }

            }


            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }

        }

        private void FormatoMimessy()
        {
            try
            {

                //Centrar todas las colucnas 
                DgvMimesys.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //asignar Nombres a cada colucna 
                DgvMimesys.Columns["Codigo_Mimesys"].HeaderText = "Código de Mimesys";

                DgvMimesys.Columns["Codigo_Mimesys"].Width = 500;

                DgvMimesys.Columns["Codigo_Mimesys"].ReadOnly = true;

                //Para ocultar los encabezados de columna
                DgvMimesys.ColumnHeadersVisible = false;

                DgvMimesys.Columns["Codigo_Mimesys"].SortMode = DataGridViewColumnSortMode.NotSortable;
                //quitar seleccion por defecto de datagrid
                DgvMimesys.ClearSelection();

            }

            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }

        }

        private void FormatoMontura()
        {
            try
            {
                //Centrar todas las colucnas 
                DgvMedidasMontura.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //asignar Nombres a cada colucna 
                DgvMedidasMontura.Columns["Horizontal"].HeaderText = "Horizontal";
                DgvMedidasMontura.Columns["Vertical"].HeaderText = "Vertical";
                DgvMedidasMontura.Columns["Maxima"].HeaderText = "Máxima";
                DgvMedidasMontura.Columns["DEL"].HeaderText = "Puente";
                DgvMedidasMontura.Columns["DV"].HeaderText = "DV";
                DgvMedidasMontura.Columns["AP"].HeaderText = "AP";
                DgvMedidasMontura.Columns["AF"].HeaderText = "AF";
                DgvMedidasMontura.Columns["DDL"].HeaderText = "DDL";

                //Ancho de columna
                DgvMedidasMontura.Columns["Horizontal"].Width = 130;
                DgvMedidasMontura.Columns["Vertical"].Width = 130;
                DgvMedidasMontura.Columns["Maxima"].Width = 130;
                DgvMedidasMontura.Columns["DEL"].Width = 130;
                DgvMedidasMontura.Columns["DV"].Width = 130;
                DgvMedidasMontura.Columns["AP"].Width = 130;
                DgvMedidasMontura.Columns["AF"].Width = 130;
                DgvMedidasMontura.Columns["DDL"].Width = 140;

                DgvMedidasMontura.Columns["Horizontal"].ReadOnly = true;
                DgvMedidasMontura.Columns["Vertical"].ReadOnly = true;
                DgvMedidasMontura.Columns["Maxima"].ReadOnly = true;
                DgvMedidasMontura.Columns["DEL"].ReadOnly = true;
                DgvMedidasMontura.Columns["DV"].ReadOnly = true;
                DgvMedidasMontura.Columns["AP"].ReadOnly = true;
                DgvMedidasMontura.Columns["AF"].ReadOnly = true;
                DgvMedidasMontura.Columns["DDL"].ReadOnly = true;

                DgvMedidasMontura.Columns["Horizontal"].SortMode = DataGridViewColumnSortMode.NotSortable;
                DgvMedidasMontura.Columns["Vertical"].SortMode = DataGridViewColumnSortMode.NotSortable;
                DgvMedidasMontura.Columns["Maxima"].SortMode = DataGridViewColumnSortMode.NotSortable;
                DgvMedidasMontura.Columns["DEL"].SortMode = DataGridViewColumnSortMode.NotSortable;
                DgvMedidasMontura.Columns["DV"].SortMode = DataGridViewColumnSortMode.NotSortable;
                DgvMedidasMontura.Columns["AP"].SortMode = DataGridViewColumnSortMode.NotSortable;
                DgvMedidasMontura.Columns["AF"].SortMode = DataGridViewColumnSortMode.NotSortable;
                DgvMedidasMontura.Columns["DDL"].SortMode = DataGridViewColumnSortMode.NotSortable;
                //quitar seleccion por defecto de datagrid
                DgvMedidasMontura.ClearSelection();

            }


            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }

        }

        private void FormatoColora()
        {
            try
            {
                //Centrar todas las colucnas 
                DgvColoracion.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //asignar Nombres a cada colucna 
                DgvColoracion.Columns["Codigo"].HeaderText = "Código";
                DgvColoracion.Columns["Descripcion"].HeaderText = "Descripción";
                DgvColoracion.Columns["Porcentaje_material"].HeaderText = "Porcentaje Material";

                //Ancho de columna
                DgvColoracion.Columns["Codigo"].Width = 180;
                DgvColoracion.Columns["Descripcion"].Width = 180;
                DgvColoracion.Columns["Porcentaje_material"].Width = 180;

                DgvColoracion.Columns["Codigo"].ReadOnly = true;
                DgvColoracion.Columns["Descripcion"].ReadOnly = true;
                DgvColoracion.Columns["Porcentaje_material"].ReadOnly = true; ;

                DgvColoracion.Columns["Codigo"].SortMode = DataGridViewColumnSortMode.NotSortable;
                DgvColoracion.Columns["Descripcion"].SortMode = DataGridViewColumnSortMode.NotSortable;
                DgvColoracion.Columns["Porcentaje_material"].SortMode = DataGridViewColumnSortMode.NotSortable;
                //quitar seleccion por defecto de datagrid
                DgvColoracion.ClearSelection();

            }


            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }

        }

        private void FormatoObservacionExamen()
        {
            try
            {

                //Centrar todas las colucnas 
                DgvObservaconExamen.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //asignar Nombres a cada colucna 
                DgvObservaconExamen.Columns["Observacion"].HeaderText = "Observación";

                DgvObservaconExamen.Columns["Observacion"].Width = 1050;

                DgvObservaconExamen.Columns["Observacion"].ReadOnly = true;

                //Para ocultar los encabezados de columna
                DgvObservaconExamen.ColumnHeadersVisible = false;
                //quitar seleccion por defecto de datagrid
                DgvObservaconExamen.ClearSelection();

            }

            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }

        }

        private void LLenar_Datos_Convencional()
        {
            try
            {
                bool resultado = _L_Facturacion.Cargar_Examen_Coloracion_Montura(DgvFormula, DgvMedidasMontura, DgvColoracion, DgvMimesys, DgvObservaconExamen, TxtTrabajo, TxtLabo, TxtServ, TxtFecOfre, TxtOptome, TxtNunExa, TxtFechaExa);
                if (resultado == true)
                {
                    if (TxtTrabajo.Text == "CONVENCIONAL")
                    {
                        FormatoExamen();
                        FormatoMontura();
                        FormatoColora();
                        FormatoObservacionExamen();
                        FormatoMimessy();

                        label64.Visible = true;
                        DgvMedidasMontura.Visible = true;
                        label65.Visible = true;
                        DgvColoracion.Visible = true;
                        label66.Visible = true;
                        DgvMimesys.Visible = true;

                        label70.Location = new Point(-4, 375);
                        label70.Size = (Size)new Point(1054, 21);
                        DgvObservaconExamen.Location = new Point(-8, 403);
                        DgvObservaconExamen.Size = (Size)new Point(1061, 19);
                        btnExamen.Enabled = true;

                    }

                    if (TxtTrabajo.Text == "CONTACTO")
                    {
                        FormatoExamen();
                        FormatoObservacionExamen();
                        label64.Visible = false;
                        DgvMedidasMontura.Visible = false;
                        label65.Visible = false;
                        DgvColoracion.Visible = false;
                        label66.Visible = false;
                        DgvMimesys.Visible = false;
                        label70.Location = new Point(-4, 235);
                        label70.Size = (Size)new Point(1061, 28);

                        DgvObservaconExamen.Location = new Point(3, 266);
                        DgvObservaconExamen.Size = (Size)new Point(1061, 19);
                        btnExamen.Enabled = true;

                    }
                }
            }
            catch (Exception ex)
            {
                string error = (string.Format("Error: {0}", ex.Message) + ", Error inesperado");
            }
        }

        private void FormatoArticulo()
        {
            try
            {

                //Centrar todas las colucnas 
                DgvArticulo.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvArticulo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

                //asignar Nombres a cada colucna 
                DgvArticulo.Columns["Codigo_Articulo"].HeaderText = "Código";
                DgvArticulo.Columns["Cod_Laboratorio"].HeaderText = "Cód. Laboratorio ";
                DgvArticulo.Columns["Descripcion"].HeaderText = "Descripción";
                DgvArticulo.Columns["Cantidad"].HeaderText = "Cantidad";
                DgvArticulo.Columns["Precio"].HeaderText = "Precio";
                DgvArticulo.Columns["Descuento"].HeaderText = "% Descuento";
                DgvArticulo.Columns["Total"].HeaderText = "Total";
                DgvArticulo.Columns["Impuesto"].HeaderText = "% Impuesto";
                DgvArticulo.Columns["Ojo"].HeaderText = "Ojo";

                if (TxtTrabajo.Text.Replace(" ", "") == "CONTACTO")
                {
                    //Ancho de columna
                    DgvArticulo.Columns["Codigo_Articulo"].Width = 80;
                    DgvArticulo.Columns["Cod_Laboratorio"].Width = 120;
                    DgvArticulo.Columns["Descripcion"].Width = 320;
                    DgvArticulo.Columns["Cantidad"].Width = 80;
                    DgvArticulo.Columns["Precio"].Width = 80;
                    DgvArticulo.Columns["Descuento"].Width = 80;
                    DgvArticulo.Columns["Total"].Width = 150;
                    DgvArticulo.Columns["Impuesto"].Width = 80;
                    DgvArticulo.Columns["Ojo"].Width = 60;
                    DgvArticulo.Columns["Cod_Laboratorio"].Visible = true;
                }
                else
                {
                    //Ancho de columna
                    DgvArticulo.Columns["Codigo_Articulo"].Width = 80;
                    DgvArticulo.Columns["Descripcion"].Width = 320;
                    DgvArticulo.Columns["Cantidad"].Width = 100;
                    DgvArticulo.Columns["Precio"].Width = 100;
                    DgvArticulo.Columns["Descuento"].Width = 100;
                    DgvArticulo.Columns["Total"].Width = 170;
                    DgvArticulo.Columns["Impuesto"].Width = 100;
                    DgvArticulo.Columns["Ojo"].Width = 90;
                    DgvArticulo.Columns["Cod_Laboratorio"].Visible = false;
                }

                DgvArticulo.Columns["Codigo_Articulo"].ReadOnly = true;
                DgvArticulo.Columns["Descripcion"].ReadOnly = true;
                DgvArticulo.Columns["Cantidad"].ReadOnly = true;
                DgvArticulo.Columns["Precio"].ReadOnly = true;
                DgvArticulo.Columns["Descuento"].ReadOnly = true;
                DgvArticulo.Columns["Total"].ReadOnly = true;
                DgvArticulo.Columns["Impuesto"].ReadOnly = true;
                DgvArticulo.Columns["Ojo"].ReadOnly = true;
                DgvArticulo.Columns["Cod_Laboratorio"].ReadOnly = true;

                DgvArticulo.Columns["Codigo_Articulo"].SortMode = DataGridViewColumnSortMode.NotSortable;
                DgvArticulo.Columns["Cod_Laboratorio"].SortMode = DataGridViewColumnSortMode.NotSortable;
                DgvArticulo.Columns["Descripcion"].SortMode = DataGridViewColumnSortMode.NotSortable;
                DgvArticulo.Columns["Cantidad"].SortMode = DataGridViewColumnSortMode.NotSortable;
                DgvArticulo.Columns["Precio"].SortMode = DataGridViewColumnSortMode.NotSortable;
                DgvArticulo.Columns["Descuento"].SortMode = DataGridViewColumnSortMode.NotSortable;
                DgvArticulo.Columns["Total"].SortMode = DataGridViewColumnSortMode.NotSortable;
                DgvArticulo.Columns["Impuesto"].SortMode = DataGridViewColumnSortMode.NotSortable;
                DgvArticulo.Columns["Ojo"].SortMode = DataGridViewColumnSortMode.NotSortable;

                //quitar seleccion por defecto de datagrid
                DgvArticulo.ClearSelection();


            }


            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }

        }

        private void FormatoMedidasMonturas()
        {
            try
            {

                //Centrar todas las colucnas 
                DgvMontura.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //asignar Nombres a cada colucna 
                DgvMontura.Columns["Horizontal"].HeaderText = "Horizontal";
                DgvMontura.Columns["Vertical"].HeaderText = "Vertical";
                DgvMontura.Columns["Maxima"].HeaderText = "Máxima";
                DgvMontura.Columns["Puente"].HeaderText = "Puente";


                //Ancho de columna
                DgvMontura.Columns["Horizontal"].Width = 175;
                DgvMontura.Columns["Vertical"].Width = 175;
                DgvMontura.Columns["Maxima"].Width = 175;
                DgvMontura.Columns["Puente"].Width = 175;


                DgvMontura.Columns["Horizontal"].ReadOnly = true;
                DgvMontura.Columns["Vertical"].ReadOnly = true;
                DgvMontura.Columns["Maxima"].ReadOnly = true;
                DgvMontura.Columns["Puente"].ReadOnly = true;

                DgvMontura.Columns["Horizontal"].SortMode = DataGridViewColumnSortMode.NotSortable;
                DgvMontura.Columns["Vertical"].SortMode = DataGridViewColumnSortMode.NotSortable;
                DgvMontura.Columns["Maxima"].SortMode = DataGridViewColumnSortMode.NotSortable;
                DgvMontura.Columns["Puente"].SortMode = DataGridViewColumnSortMode.NotSortable;

                //quitar seleccion por defecto de datagrid
                DgvMontura.ClearSelection();

            }


            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }

        }

        private void FormatoCalculo()
        {
            try
            {
                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml("#07a79b");

                // Poner la fila de subTotal verde 
                foreach (DataGridViewRow row in DgvTotales.Rows)
                {
                    if (row.Cells["ReciboPago"].Value.ToString() == "SubTotal")
                    {

                        row.DefaultCellStyle.BackColor = color;
                        row.DefaultCellStyle.ForeColor = System.Drawing.Color.White;

                    }
                    row.Height = 25;
                }

                DgvTotales.ClearSelection();
                DgvTotales.AutoResizeRows(DataGridViewAutoSizeRowsMode.DisplayedCells);

                //Centrar todas las colucnas 
                DgvTotales.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //asignar Nombres a cada colucna 
                DgvTotales.Columns["ReciboPago"].HeaderText = "";
                DgvTotales.Columns["Monto"].HeaderText = "Monto";


                DgvTotales.Columns["ReciboPago"].Width = 185;
                DgvTotales.Columns["Monto"].Width = 185;

                //DgvTotales.Rows[0].Height = 30;
                //DgvTotales.Rows[1].Height = 30;
                //DgvTotales.Rows[2].Height = 30;
                //DgvTotales.Rows[3].Height = 30;
                //DgvTotales.Rows[4].Height = 30;


                DgvTotales.Columns["ReciboPago"].ReadOnly = true;
                DgvTotales.Columns["Monto"].ReadOnly = true;

                //Para ocultar los encabezados de columna
                DgvTotales.ColumnHeadersVisible = false;

            }


            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }

        }

        private void FormatoObserva()
        {
            try
            {

                //Centrar todas las colucnas 
                DgvObservaMon.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //asignar Nombres a cada colucna 
                DgvObservaMon.Columns["Observacion"].HeaderText = "Observación";

                DgvObservaMon.Columns["Observacion"].Width = 700;

                DgvObservaMon.Columns["Observacion"].ReadOnly = true;

                //Para ocultar los encabezados de columna
                DgvObservaMon.ColumnHeadersVisible = false;

                //Para ocultar los encabezados de columna
                DgvObservaMon.ColumnHeadersVisible = false;
                //quitar seleccion por defecto de datagrid
                DgvObservaMon.ClearSelection();

            }

            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }

        }

        private void LLenar_Datos_Orden()
        {
            try
            {
                bool resultado = _L_Facturacion.Cargar_Detalle_Orden_Parte1(DgvArticulo, DgvMontura, DgvTotales, DgvObservaMon, txtMontura_quorun, txtPromocion, txtMontura_propia, txtCristales_propio, TxtEmpresa, lbPromocion);
                if (resultado == true)
                {
                    FormatoArticulo();
                    FormatoMedidasMonturas();
                    FormatoCalculo();
                    FormatoObserva();


                    btnDetalleOrden.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                string error = (string.Format("Error: {0}", ex.Message) + ", Error inesperado");
            }
        }

        private bool ValidarPagosIva(System.Windows.Forms.DataGridView Dt_Abono)
        {
            bool IVA = false;
            foreach (DataGridViewRow row in Dt_Abono.Rows)
            {
                string Codigo = row.Cells["CodPago"].Value.ToString();
                if (Codigo == "013")
                {
                    IVA = true;
                    Medio_Pago = "Iva Retenido";
                }

            }
            return IVA;
        }

        private void LLenar_Datos_PagoMovil()
        {
            try
            {
                //Nacionalidad
                _L_Facturacion.ComboboxNacionalidad(CbxNacionalidadPagoMovil);

                //Prefijos de Celular
                _L_Facturacion.ComboboxPrefijosCelular(CbxCelularPagoMovil);


                // llenar Bancos
                _L_Facturacion.LLenarComboboxBancos(CbxBancoPagoMovil, false);


                TxtCedulaPagoMovil.Text = txtCedula.Text.Substring(2, txtCedula.Text.Length - 2);
                TxtCedularPagoMovil.Text = _L_Facturacion.TlfCliente.Substring(7, _L_Facturacion.TlfCliente.Length - 7);
                TxtMontoPagoMovil.Text = TxtVuelto.Text;
                CbxNacionalidadPagoMovil.Enabled = true;
                CbxCelularPagoMovil.Enabled = true;
                String d = _L_Facturacion.TlfCliente.Substring(0, _L_Facturacion.TlfCliente.Length - 11);

                //Recorrer el combobox  Prefijos
                for (int i = 0; i <= CbxCelularPagoMovil.Items.Count; i++)
                {

                    if (i == 6)
                    {
                        CbxCelularPagoMovil.SelectedIndex = 0;
                        TxtCedularPagoMovil.Text = "";
                        break;
                    }


                    if (d != CbxCelularPagoMovil.Text)
                    {
                        CbxCelularPagoMovil.SelectedIndex = i;
                    }
                    else
                    {
                        break;
                    }

                }

                //Recorrer el combobox  Nacionalidad 
                for (int i = 0; i <= CbxNacionalidadPagoMovil.Items.Count; i++)
                {
                    if(txtCedula.Text.Length >= 7)
                        if (txtCedula.Text.Substring(0, txtCedula.Text.Length - 8) != CbxNacionalidadPagoMovil.Text)
                            CbxNacionalidadPagoMovil.SelectedIndex = i;
                        else
                            break;
                    else if (txtCedula.Text.Length == 10)
                        if (txtCedula.Text.Substring(0, txtCedula.Text.Length - 9) != CbxNacionalidadPagoMovil.Text)
                        CbxNacionalidadPagoMovil.SelectedIndex = i;
                    else
                            break;
                    else
                   if (txtCedula.Text.Substring(0, txtCedula.Text.Length - 10) != CbxNacionalidadPagoMovil.Text)
                            CbxNacionalidadPagoMovil.SelectedIndex = i;
                        else
                            break;

                }

               

   
            }
            catch (Exception ex)
            {
                string error = (string.Format("Error: {0}", ex.Message) + ", Error inesperado");
            }
        }

        private void TxtVuelto_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void TxtMontoPagoMovil_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                TxtMontoPagoMovil.Text = "";
            }
        }

        private void TxtRecibidoREF_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8)
            {
                e.Handled = false;
                return;
            }


            bool IsDec = false;
            int nroDec = 0;

            for (int i = 0; i < TxtRecibidoREF.Text.Length; i++)
            {
                if (TxtRecibidoREF.Text[i] == ',')

                    IsDec = true;

                if (IsDec && nroDec++ >= 2)
                {
                    e.Handled = true;
                    return;
                }
            }


            if (e.KeyChar >= 44 && e.KeyChar <= 57)
                e.Handled = false;
            else if (e.KeyChar == 46)
                e.Handled = (IsDec) ? true : false;
            else
                e.Handled = true;


            //para que solo acepte numeros y una sola coma
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }


            if (TxtRecibidoREF.Text.Contains("") && e.KeyChar == 44)
            {
                //separamos por punto
                string[] parts = TxtRecibidoREF.Text.Split(',');
                //si el primer elemento está vacío, significa que no se escribió nada antes de, entonces, añadimos el cero al textbox.
                if (parts[0].Length <= 0)
                {
                    TxtRecibidoREF.Text = "0" + TxtRecibidoREF.Text;
                    //UPDATE: colocamos el cursor al final del texto
                    TxtRecibidoREF.SelectionStart = TxtRecibidoREF.Text.Length;
                }
            }

            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                if (TxtRecibidoREF.Text != "")
                {
                    if (Convert.ToDouble(TxtRecibidoREF.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtRecibidoREF.Text)) >= Convert.ToDouble(txtRef.Text == "" ? (Double)0.00 : Convert.ToDouble(txtRef.Text)))
                    {
                        if (CbxMoneda.SelectedIndex.ToString() == "1")
                        {
                            TxtVuelto.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", Math.Round((Convert.ToDouble(TxtRecibidoREF.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtRecibidoREF.Text))) - Convert.ToDouble(txtRef.Text == "" ? (Double)0.00 : Convert.ToDouble(txtRef.Text)), 2));
                            TxtMontoPagoMovil.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", Math.Round(Convert.ToDouble(TB_TASA_Euro.Tasa) * ((Convert.ToDouble(TxtRecibidoREF.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtRecibidoREF.Text))) - Convert.ToDouble(txtRef.Text == "" ? (Double)0.00 : Convert.ToDouble(txtRef.Text))), 2));
                        }
                        else
                        {
                            TxtVuelto.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", Math.Round((Convert.ToDouble(TxtRecibidoREF.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtRecibidoREF.Text))) - Convert.ToDouble(txtRef.Text == "" ? (Double)0.00 : Convert.ToDouble(txtRef.Text)), 2));
                            TxtMontoPagoMovil.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", Math.Round(Convert.ToDouble(TB_TASA_Dolar.Tasa) * ((Convert.ToDouble(TxtRecibidoREF.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtRecibidoREF.Text))) - Convert.ToDouble(txtRef.Text == "" ? (Double)0.00 : Convert.ToDouble(txtRef.Text))), 2));
                        }
                    }

                    else
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("El monto recibido REF debe ser mayor al total en divisa.");
                        _FrmMensajes.ShowDialog();
                        TxtRecibidoREF.Text = "";
                        TxtRecibidoREF.Focus();
                    }
                }
            }
        }

        private void TxtRecibidoREF_Click(object sender, EventArgs e)
        {
            TxtRecibidoREF.Text = "";
        }

        private void TxtRecibidoREF_Validating(object sender, CancelEventArgs e)
        {
            if (TxtRecibidoREF.Text != "")
            {

                if (Convert.ToDouble(TxtRecibidoREF.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtRecibidoREF.Text)) >= Convert.ToDouble(txtRef.Text == "" ? (Double)0.00 : Convert.ToDouble(txtRef.Text)))
                {

                    if (CbxMoneda.SelectedIndex.ToString() == "1")
                    {
                        TxtVuelto.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", Math.Round((Convert.ToDouble(TxtRecibidoREF.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtRecibidoREF.Text))) - Convert.ToDouble(txtRef.Text == "" ? (Double)0.00 : Convert.ToDouble(txtRef.Text)), 2));
                        TxtMontoPagoMovil.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", Math.Round(Convert.ToDouble(TB_TASA_Euro.Tasa) * ((Convert.ToDouble(TxtRecibidoREF.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtRecibidoREF.Text))) - Convert.ToDouble(txtRef.Text == "" ? (Double)0.00 : Convert.ToDouble(txtRef.Text))), 2));
                    }
                    else
                    {
                        TxtVuelto.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", Math.Round((Convert.ToDouble(TxtRecibidoREF.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtRecibidoREF.Text))) - Convert.ToDouble(txtRef.Text == "" ? (Double)0.00 : Convert.ToDouble(txtRef.Text)), 2));
                        TxtMontoPagoMovil.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", Math.Round(Convert.ToDouble(TB_TASA_Dolar.Tasa) * ((Convert.ToDouble(TxtRecibidoREF.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtRecibidoREF.Text))) - Convert.ToDouble(txtRef.Text == "" ? (Double)0.00 : Convert.ToDouble(txtRef.Text))), 2));
                    }

                }
                else
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("El monto recibido REF debe ser mayor al total en divisa.");
                    _FrmMensajes.ShowDialog();
                    TxtRecibidoREF.Text = "";
                    TxtRecibidoREF.Focus();
                }

            }
        }

        private void TxtMontoPagoMovil_Validating(object sender, CancelEventArgs e)
        {
            if (TxtMontoPagoMovil.Text == "")
            {
                TxtMontoPagoMovil.Text = "0,00";
            }
            else
            {
                TxtMontoPagoMovil.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", TxtMontoPagoMovil.Text).Replace(".", ",");
            }


            if (Convert.ToDouble(TxtMontoPagoMovil.Text == "" ? (Double)0.00 : Convert.ToDouble(TxtMontoPagoMovil.Text)) > Math.Round(20 * Convert.ToDouble(TB_TASA_Dolar.Tasa), 2))
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(" El limite de pagos móvil es de 20$, Verifique");
                _FrmMensajes.ShowDialog();
                txtRef.Text = "";
                txtRef.Focus();
            }


        }

        private void BolivaresConveridos(Double Bolivares, System.Windows.Forms.TextBox BolivaresDeseados)
        {
            try
            {
            BolivaresDeseados.Text= String.Format(CultureInfo.InvariantCulture, "{0:0.00}",Bolivares).Replace(".", ",");
            BolivaresDeseados.Text = String.Format("{0:#,0.00}", Bolivares);

            }
            catch (Exception ex)
            {
                string error = (string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.avisomensaje(error);
                _FrmMensajes.ShowDialog();

            }

        }

        private void txtMonto2Bs_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMonto2Bs.Text) && txtMonto2Bs.Text == "") txtMonto2Bs.Text = "0,00";
            else
            {
                txtMonto2Bs.Text = string.Format("{0:#,0.00}", Convert.ToDecimal(txtMonto2Bs.Text));
            }
        }

        private void txtMontoBs_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMontoBs.Text) && txtMontoBs.Text == "") txtMontoBs.Text = "0,00";
            else
            {
                txtMontoBs.Text = string.Format("{0:#,0.00}", Convert.ToDecimal(txtMontoBs.Text));
            }
        }

        public string ImprimirCambio(string Correlativo, DataTable PagoMovilRealizados, string CodigoSucursal, string NumeroOrden, string Revision, string NumeroFactura, SqlCommand command)
        {
            uint resp = 0;
            try
            {
                //'''''' * ********DATOS DEL VUELTO PAGO MOVIL ************
                string Respuesta = "SATISFACTORIO";
                Double MontoRecibidoRef1 = 0;
                Double MontoVueltoRef1 = 0;
                Double MontoVueltoBs1 = 0;
                string CteNacionalidad1 = "";
                string Cedula1 = "";
                string CodBancoReceptor1 = "";
                string Telefono1 = "";
                string Nombre_Sucursal = _D_DetalleOrden.Nombre_Surculsal_PagoMovil(CodigoSucursal, command);
                if (PagoMovilRealizados != null)
                {
                    if (PagoMovilRealizados.Rows.Count > 0)
                    {
                        foreach (DataRow Row in PagoMovilRealizados.Rows)
                        {


                            MontoRecibidoRef1 = Convert.ToDouble(Row["MontoRecibidoRef"].ToString());
                            MontoVueltoRef1 = Convert.ToDouble(Row["MontoVueltoRef"].ToString().Replace(".", ","));
                            MontoVueltoBs1 = Convert.ToDouble(Row["MontoVueltoBs"].ToString().Replace(".", ","));
                            CteNacionalidad1 = Row["Nacionalidad"].ToString();
                            Cedula1 = Row["Cedula"].ToString();
                            CodBancoReceptor1 = Row["Banco"].ToString();
                            Telefono1 = Row["PrefijoCelular"].ToString() + "-" + Row["Celular"].ToString();
                            string NombreBanco_PagoMovil = _D_DetalleOrden.NombreBanco_PagoMovil(CodBancoReceptor1, command);

                            

                            //Factura Automatica
                            if (_L_Facturacion.ValidaFactManual(command) == false)
                            {
                                objVmax.AbrirPuerto(Convert.ToString(glbPuertoCOM));
                                resp = objVmax.AbrirDNF();
                                resp = objVmax.TextoNoFiscal("----------------------------------------");
                                resp = objVmax.TextoNoFiscal(" ");
                                resp = objVmax.TextoNoFiscal("Os: " + NumeroOrden);
                                resp = objVmax.TextoNoFiscal("Factura:  " + NumeroFactura);
                                resp = objVmax.TextoNoFiscal("Correlativo: " + Correlativo);
                                resp = objVmax.TextoNoFiscal("Nombre de la sucursal: " + Nombre_Sucursal);
                                resp = objVmax.TextoNoFiscal("Cliente: " + CteNacionalidad1 + "-" + Cedula1);
                                resp = objVmax.TextoNoFiscal("Telefono: " + Telefono1);
                                resp = objVmax.TextoNoFiscal("Banco: " + NombreBanco_PagoMovil);
                                resp = objVmax.TextoNoFiscal("Monto:    " + MontoVueltoBs1);
                                resp = objVmax.TextoNoFiscal(" ");
                                resp = objVmax.TextoNoFiscal("----------------------------------------");
                                resp = objVmax.CerrarDNF();
                                resp = objVmax.CerrarPuerto();
                            }

                            //Factura Manual
                            else
                            {
                                _FrmMostrarRep.ConfigRep();

                                if (_D_DetalleOrden.ParametroImpresion() == "1") // Si el parametro de impresion es 1 entonces imprime el reporte 
                                {
                                    _FrmMostrarRep.imprimir(NumeroOrden, NumeroFactura, Correlativo, Nombre_Sucursal, CteNacionalidad1 + "-" + Cedula1, Telefono1, NombreBanco_PagoMovil, Convert.ToString(MontoVueltoBs1));

                                }
                                else
                                {
                                    _FrmMostrarRep.Mostrar(NumeroOrden, NumeroFactura, Correlativo, Nombre_Sucursal, CteNacionalidad1 + "-" + Cedula1, Telefono1, NombreBanco_PagoMovil, Convert.ToString(MontoVueltoBs1));
                                    _FrmMostrarRep.ShowDialog();

                                }

                            }

                            MontoRecibidoRef1 = 0;
                            MontoVueltoRef1 = 0;
                            MontoVueltoBs1 = 0;
                            CteNacionalidad1 = "";
                            Cedula1 = "";
                            CodBancoReceptor1 = "";
                            Telefono1 = "";
                            Correlativo = Convert.ToString(Convert.ToInt64(Correlativo) + 1);

                            if (resp != 0)
                            {
                                objVmax.Cancelar();
                                objVmax.Cerrar();
                                resp = objVmax.CerrarPuerto();
                                ImprimirFacturaFiscall = false;
                                return "Error";
                            }






                        }
                    }
                }
                return Respuesta;
            }

            catch (Exception ex)
            {

                string Respuesta = string.Format("Error: {0}", ex.Message);
                objVmax.Cancelar();
                objVmax.Cerrar();
                resp = objVmax.CerrarPuerto();
                return Respuesta;
            }
        }

        private void txtBsNotaCredito_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBsNotaCredito.Text) && txtBsNotaCredito.Text == "") txtBsNotaCredito.Text = "0,00";
            else
            {
                txtBsNotaCredito.Text = string.Format("{0:#,0.00}", Convert.ToDecimal(txtBsNotaCredito.Text));
            }
        }

        private void txtBsNotaDevolucion_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBsNotaDevolucion.Text) && txtBsNotaDevolucion.Text == "") txtBsNotaDevolucion.Text = "0,00";
            else
            {
                txtBsNotaDevolucion.Text = string.Format("{0:#,0.00}", Convert.ToDecimal(txtBsNotaDevolucion.Text));
            }
        }

        private void TxtCedularPagoMovil_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Validacion para que solo permita numeros 
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void TxtCedulaPagoMovil_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Validacion para que solo permita numeros 
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            


        }

        private void TxtNumFact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            // Permitir números del 0 al 9 y la tecla Backspace
            if (e.KeyChar == '\b' )
            {
                if(TxtNumFact.TextLength <= 1)
                {
                    // Seleccionar texto desde la posición 1
                    TxtNumFact.Select(1, TxtNumFact.Text.Length - 1);
                    TxtNumFact.SelectionStart = 1;
                    TxtNumFact.SelectionLength = 0;
                }
                else
                {
                    return;
                }

            }
        }

        private void TxtNumFact_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TxtNroCorrelativo.Focus();
            }

        }
        private void TxtNumFact_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtNumFact.Text) && TxtNumFact.Text == "")
                TxtNumFact.Text = "M";
        }

        private void TxtNumFact_Validating(object sender, CancelEventArgs e)
        {
            if (TxtNumFact.Text == "")
            {
                TxtNumFact.Text = "M";
            }
        }

        private void TxtNumFact_TextChanged(object sender, EventArgs e)
        {
            // Verificar si el texto inicia con "M"
            if (!TxtNumFact.Text.StartsWith("M") && (TxtNumFact.Text == "" || string.IsNullOrEmpty(TxtNumFact.Text)))
            {
                // Agregar "M" al principio
                TxtNumFact.Text = "M" + TxtNumFact.Text;
                TxtNumFact.SelectionStart = 1;
                TxtNumFact.SelectionLength = 0;
            }
        }

        public void ImprimirDeclaracion_CristalPropio_MonturaPropia()
        {
            try
            {
                // Busco si esta orden tiene CristalPropio o MonturaPropia
                if (TB_CAORDSER.MonturaPropia == true | TB_CAORDSER.cristalpropio == true)
                {

                    int numCopias = Convert.ToUInt16(_D_DetalleOrden.TB_PARAMETRO("NumCopContResp"));
                    string Sucursal = _D_DetalleOrden.TB_PARAMETRO("SucursalId");
                    string DiaActivo = _D_Inicio.DiaActivo().ToString("dd-MMM-yyyy").Replace(".","");
                    string NombreCompleto = "";

                    DataTable dtCliente = _D_DetalleOrden.BucarTB_CTEPPAL(TB_CAORDSER.CTE_CedIden, TB_CAORDSER.CTE_Nacio);

                    if (dtCliente.Rows.Count > 0)
                    {
                        foreach (DataRow drItem in dtCliente.Rows)
                        {
                            string PrimerNombre = drItem["CTE_PNombre"].ToString();
                            string PrimerApellido = drItem["CTE_PApellido"].ToString();
                            NombreCompleto = PrimerNombre + " " + PrimerApellido;
                            break;
                        }

                    }
                    _D_DetalleOrden.Nombre_Surculsal_PagoMovil(Sucursal,null);
                    string Sucursal_Descripcion =_D_DetalleOrden.Nombre_Surculsal_PagoMovil(Sucursal, null) ;
                    string compania = _D_DetalleOrden.TB_PARAMETRO("Compania");

                    _FrmMostarDeclaracion.Parametros(Sucursal_Descripcion, DiaActivo, TB_CAORDSER.NumOrdserv, NombreCompleto, TB_CAORDSER.CTE_Nacio + "-" + TB_CAORDSER.CTE_CedIden, TB_CAORDSER.OrSer_Observ, (TB_CAORDSER.Cod_Venta == "003" ? "X" : ""), (TB_CAORDSER.cristalpropio == true ? "X" : ""), (TB_CAORDSER.MonturaPropia == true ? "X" : ""), compania);
                    _FrmMostarDeclaracion.ConfigRep();
                    if (_D_DetalleOrden.ParametroImpresion() == "1") // Si el parametro de impresion es 1 entonces imprime el reporte 
                    {
                        _FrmMostarDeclaracion.imprimir_NumeroCopia(numCopias);

                    }
                    else
                    {
                        _FrmMostarDeclaracion.ShowDialog();

                    }

                }
            }
            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }
        }
        public void LimpiaVariablesIdAbonoPagoMovil()
        {
            idAbonoPagoMovil = 0;
            CantAbonosPrevios = 0;
            textBox8.Text = "0,00";
            TxtCambioRef.Text = "0,00";
            return;
        }

        private void label110_Click(object sender, EventArgs e)
        {

        }

        private void txtRef_TextChanged(object sender, EventArgs e)
        {

        }

        private void label74_Click(object sender, EventArgs e)
        {

        }

        private void Bs_Click(object sender, EventArgs e)
        {

        }

        //Para Probar los reportes

        //private void button1_Click_1(object sender, EventArgs e)
        //{
        //    string concat = TB_CAORDSER.Cod_Sucursal + TB_CAORDSER.NumOrdserv + TB_CAORDSER.Revision;
        //    _FrmMostrarReporte.setParametros(concat);
        //    _FrmMostrarReporte.ConfigRep(true, true);

        //    if (_D_DetalleOrden.ParametroImpresion() == "1") // Si el parametro de impresion es 1 entonces imprime el reporte 
        //    {
        //        _FrmMostrarReporte.imprimir();

        //    }
        //    else
        //    {
        //        _FrmMostrarReporte.ShowDialog();

        //    }
        //}

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    string concat = TB_CAORDSER.Cod_Sucursal + TB_CAORDSER.NumOrdserv + TB_CAORDSER.Revision;

        //    _FrmMostrarReporte.setParametros(concat);
        //    _FrmMostrarReporte.ConfigRep(true, false);

        //    if (_D_DetalleOrden.ParametroImpresion() == "1") // Si el parametro de impresion es 1 entonces imprime el reporte 
        //    {
        //        _FrmMostrarReporte.imprimir();

        //    }
        //    else
        //    {
        //        _FrmMostrarReporte.ShowDialog();

        //    }
        //}
    }
}