using CapaDatos.DetalleOrden_Datos;
using CapaEntidades;
using CapaLogica.DetalleOrden_Logica;
using CapaLogica.Impresora_Fiscal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVisual_Login
{
    public partial class FrmFacturacion : Form
    {
        public FrmFacturacion()
        {
            InitializeComponent();
        }
        private int glbPuertoCOM = Convert.ToInt16(ConfigurationManager.AppSettings.Get("PuertoCOMimpresora"));
        private L_Facturacion _L_Facturacion = new L_Facturacion();
        private Impresora_Fiscal _Impresora_Fiscal = new Impresora_Fiscal();
        private FrmMensajes _FrmMensajes = new FrmMensajes();
        private D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        public DataTable Dt_Abonos = new DataTable();
        private string mensaje = "";
        private string NumeroComprobanteFiscal = "";
        private bool FacturaManual;
        private bool ImprimirFacturaFiscall;
        private string TotalFacturaFiscal = "";
        //--------colocar la referencia 
        private VmaxComVe.VmaxComClass objVmax = new VmaxComVe.VmaxComClass();

        private void FrmDetalleOrden_Load(object sender, EventArgs e)
        {
            _L_Facturacion.CargarTasa();
            _L_Facturacion.ComboboxTipoMoneda(CbxMoneda);
            _L_Facturacion.LLenarComboboxPagos(CbxMetodosPago);
            _L_Facturacion.LLenarComboboxPagos(CbxMetodosPago2);
            FormatoTexbox();
            _L_Facturacion.CrearTabla(Dt_Abonos);
            DgvAbonos.DataSource = Dt_Abonos;
            FormatoDataGrid();
            lbTasa.Text = Convert.ToString(TB_TASA_Dolar.Tasa);
            _L_Facturacion.ComboboxTipoTarjeta(CbxTarjeta);

        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            VisualizarPanel("MostrarPanelPrincipal");

            lbMinAbo.Text = Convert.ToString(Math.Round(((0.80 * Convert.ToDouble(TxtMontoRef.Text))), 2));
            lbMinAbo.Text = Convert.ToString(Convert.ToDouble(lbMinAbo.Text) * TB_TASA_Dolar.Tasa);
            txtMontoBs.Text = Convert.ToString(TxtSaldoOrd.Text);

        }

        private void VisualizarPanel(string Case)
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

                    break;

                case "MostrarNotasCredito":
                    this.PnlNotaCredito.Enabled = true;
                    this.PnlNotaCredito.Visible = true;
                    this.PnlNotaCredito.Location = new Point(250, 1);
                    this.PnlNotaCredito.BringToFront();
                    this.PnlPrimario.Visible = false;
                    this.PnlPrimario.Enabled = false;

                    break;

                default:
                    break;
            }


        }


        private void txtRef_KeyPress(object sender, KeyPressEventArgs e)
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


            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);
                double MontoFaltanteIgtfbs = Convert.ToDouble(txtMontoBs.Text) * 0.03;

                if (Math.Round((double)((Convert.ToDouble(txtMontoBs.Text) + MontoFaltanteIgtfbs) / TB_TASA_Dolar.Tasa), 2) >= Convert.ToDouble(txtRef.Text))
                {
                    txtIGTF.Text = Convert.ToString(_L_Facturacion.CalculoIgtf(txtNumeroOrden, txtRef, DgvAbonos));
                    _L_Facturacion.ConvertirDolaresBolivares(txtRef, txtMonto2Bs);
                    txtTranferencia.Focus();
                }
                else
                {
                    MessageBox.Show("El Monto debe ser igual o menor al saldo de la orden", "Verifique");
                    txtRef.Text = "";
                }

            }

        }

        private void FormatoTexbox()
        {
            try
            {
                if (string.IsNullOrEmpty(txtRef.Text) && txtRef.Text == "") txtRef.Text = "0.00";
                if (string.IsNullOrEmpty(txtMonto2Bs.Text) && txtRef.Text == "") txtMonto2Bs.Text = "0.00";
                if (string.IsNullOrEmpty(txtIGTF.Text) && txtRef.Text == "") txtIGTF.Text = "0.00";
                if (string.IsNullOrEmpty(TxtVuelto.Text) && txtRef.Text == "") TxtVuelto.Text = "0.00";


            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }

        }


        private void txtRef_Click(object sender, EventArgs e)
        {
            txtMonto2Bs.Text = "0.00";
            txtIGTF.Text = "0.00";
            TxtVuelto.Text = "0.00";
            txtTranferencia.Text = "";

        }

        private void txtMonto2Bs_KeyPress(object sender, KeyPressEventArgs e)
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
                txtTranferencia.Focus();
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
            //para que solo acepte numeros y una sola coma
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }

        }

        private void txtTranferencia_KeyPress(object sender, KeyPressEventArgs e)
        {
            //para que solo acepte numeros y una sola coma
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            if (e.KeyChar == 13)
            {
                DtpFecha.Focus();
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
            //para que solo acepte numeros y una sola coma
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }


        private void btnCancelar2_Click(object sender, EventArgs e)
        {
            VisualizarPanel("MostrarPanelPrincipal");
            LimpiarTxbox();

        }

        private void LimpiarTxbox()
        {
            this.PnlSecundario.Visible = false;
            this.PnlNotaCredito.Visible = false;

            txtRef.Text = "0.00";
            txtMonto2Bs.Text = "0.00";
            txtIGTF.Text = "0.00";
            TxtVuelto.Text = "0.00";
            txtTranferencia.Text = "";
            CbxMetodosPago.SelectedIndex = 0;
            txtCVC.Text = "";
            txtCheque.Text = "";
            txtVence.Text = "";

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


        }

        private void btnProcesar2_Click(object sender, EventArgs e)
        {
            try
            {
                if (CbxMetodosPago2.Text == "Transferencia Divisa")
                {

                    //'Si los campos poseen valores proceso los datos
                    if (txtRef.Text != "0.00" && txtMonto2Bs.Text != "0.00" && txtIGTF.Text != "0.00" && txtTranferencia.Text != "" && CbxBanco.Text != "")
                    {
                        _L_Facturacion.GuardarAbonoGrid(Dt_Abonos, CbxMetodosPago2.SelectedValue.ToString(), CbxMoneda.Text, CbxBanco.SelectedValue.ToString(), txtMonto2Bs.Text, txtTranferencia.Text, DtpFecha.Text, TxtVuelto.Text, txtRef.Text, txtIGTF.Text);
                        txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);
                        lbMinAbo.Text = Convert.ToString(Convert.ToDouble(lbMinAbo.Text) + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos)));
                        VisualizarPanel("MostrarPanelPrincipal");
                        LimpiarTxbox();
                        CbxMetodosPago.SelectedIndex = 0;
                    }

                    else
                    {
                        MessageBox.Show("Debe llenar todos los campos", "Verifique");
                    }

                }


                if (CbxMetodosPago2.Text == "Transferencia Bolivares")
                {
                    //'Si los campos poseen valores proceso los datos
                    if (txtMonto2Bs.Text != "0.00" && txtTranferencia.Text != "" && CbxBanco.Text != "")
                    {
                        _L_Facturacion.GuardarAbonoGrid(Dt_Abonos, CbxMetodosPago2.SelectedValue.ToString(), CbxMoneda.Text, CbxBanco.SelectedValue.ToString(), txtMonto2Bs.Text, txtTranferencia.Text, DtpFecha.Text, TxtVuelto.Text);
                        txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);
                        lbMinAbo.Text = Convert.ToString(Convert.ToDouble(lbMinAbo.Text) + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos)));
                        VisualizarPanel("MostrarPanelPrincipal");
                        LimpiarTxbox();
                        CbxMetodosPago.SelectedIndex = 0;
                    }

                    else
                    {
                        MessageBox.Show("Debe llenar todos los campos", "Verifique");
                    }
                }


                if (CbxMetodosPago2.Text == "Debito")
                {
                    //'Si los campos poseen valores proceso los datos
                    if (txtMonto2Bs.Text != "" && txtMonto2Bs.Text != "0.00" && txtTranferencia.Text != "" && CbxBanco.Text != "" )
                    {
                        _L_Facturacion.GuardarAbonoGrid(Dt_Abonos, CbxMetodosPago2.SelectedValue.ToString(), CbxMoneda.Text, CbxBanco.SelectedValue.ToString(), txtMonto2Bs.Text, txtTranferencia.Text, DtpFecha.Text, TxtVuelto.Text, "","" , txtCVC.Text, txtVence.Text);
                        txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);
                        lbMinAbo.Text = Convert.ToString(Convert.ToDouble(lbMinAbo.Text) + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos)));
                        VisualizarPanel("MostrarPanelPrincipal");
                        LimpiarTxbox();
                        CbxMetodosPago.SelectedIndex = 0;
                    }

                    else
                    {
                        MessageBox.Show("Debe llenar todos los campos", "Verifique");
                    }
                }


                if (CbxMetodosPago2.Text == "Tarjeta de Credito")
                {
                    //'Si los campos poseen valores proceso los datos
                    if (txtMonto2Bs.Text != "0.00" && txtTranferencia.Text != "" && CbxBanco.Text != "" && txtVence.Text != "" && txtCVC.Text != "")
                    {
                        _L_Facturacion.GuardarAbonoGrid(Dt_Abonos, CbxMetodosPago2.SelectedValue.ToString(), CbxMoneda.Text, CbxBanco.SelectedValue.ToString(), txtMonto2Bs.Text, txtTranferencia.Text, DtpFecha.Text, TxtVuelto.Text, "", "", txtCVC.Text, txtVence.Text);
                        txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);
                        lbMinAbo.Text = Convert.ToString(Convert.ToDouble(lbMinAbo.Text) + Convert.ToDouble(_L_Facturacion.TotalIgtf(DgvAbonos)));
                        VisualizarPanel("MostrarPanelPrincipal");
                        LimpiarTxbox();
                        CbxMetodosPago.SelectedIndex = 0;
                    }

                    else
                    {
                        MessageBox.Show("Debe llenar todos los campos", "Verifique");
                    }
                }

             

            }

            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }
        }

        private void btnProcesar1_Click(object sender, EventArgs e)
        {
            _L_Facturacion.LLenadoParametros(DgvAbonos, CbxMetodosPago2);

        }

        public void CargarDatosOrden(string NumeroOrden, String NombreCliente)
        {
            try
            {
                _L_Facturacion.DatosOrden(NumeroOrden);
                txtNumeroOrden.Text = TB_CAORDSER.NumOrdserv;
                txtNombreVendedor.Text = TB_USUARIO.USER_NOMBRE + " " + TB_USUARIO.USER_APELLIDO;
                txtCedula.Text = TB_CAORDSER.CTE_Nacio + "-" + TB_CAORDSER.CTE_CedIden;
                TxtFecha.Text = Convert.ToString(TB_CAORDSER.Fecha).Substring(0, 11);
                TxtStatus.Text = _L_Facturacion.StatusOrde(TB_CAORDSER.OrSer_Status);
                TxtMontoBs2.Text = Convert.ToString(TB_CAORDSER.VtaTotal);
                TxtSaldoOrd.Text = Convert.ToString(TB_CAORDSER.OrSer_Saldo);
                TxtMontoRef.Text = Convert.ToString(TB_CAORDSER.Orser_Total_Mon);
                txtNombreCliente.Text = NombreCliente;
                if (TB_CAORDSER.OrSer_Saldo != 0)
                {
                    TxtAbono.Text = Convert.ToString((TB_CAORDSER.VtaTotal - TB_CAORDSER.OrSer_Saldo));
                }

                else
                {
                    TxtAbono.Text = Convert.ToString(TB_CAORDSER.OrSer_Saldo);
                }

                TxtSaldoRef.Text = Convert.ToString(TB_CAORDSER.OrSer_Saldo_Mon);

                if (TB_CAORDSER.OrSer_Saldo_Mon != 0)
                {
                    TxtAbonoRef.Text = Convert.ToString((TB_CAORDSER.Orser_Total_Mon - TB_CAORDSER.OrSer_Saldo_Mon));
                }

                else
                {
                    TxtAbonoRef.Text = Convert.ToString(TB_CAORDSER.OrSer_Saldo_Mon);
                }

                TxtIgtfOrd.Text = Convert.ToString(TB_CAORDSER.VtaImpuestoIGTF);


                if (TxtStatus.Text == "Facturada" || TxtStatus.Text == "Anulada")
                {
                    btnIngresar.Enabled = false;
                }

                else
                {
                    btnIngresar.Enabled = true;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }

        }

        private void txtMontoBs_Validating(object sender, CancelEventArgs e)
        {

        }

        private void btnCancelar1_Click(object sender, EventArgs e)
        {
            VisualizarPanel("MostrarFormulario");
            Dt_Abonos.Rows.Clear();

        }

        private void txtRef_Validating(object sender, CancelEventArgs e)
        {

            FormatoTexbox();
            txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);

            double MontoFaltanteIgtfbs = Convert.ToDouble(txtMontoBs.Text) * 0.03;

            if (Math.Round((double)((Convert.ToDouble(txtMontoBs.Text)+ MontoFaltanteIgtfbs) / TB_TASA_Dolar.Tasa), 2) >= Convert.ToDouble(txtRef.Text) )
            {
                txtIGTF.Text = Convert.ToString(_L_Facturacion.CalculoIgtf(txtNumeroOrden, txtRef, DgvAbonos));
                _L_Facturacion.ConvertirDolaresBolivares(txtRef, txtMonto2Bs);
                txtTranferencia.Focus();
            }
            else
            {

                MessageBox.Show("El Monto debe ser igual o menor al saldo de la orden", "Verifique");
                txtRef.Text = "";
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
            if (e.KeyCode == Keys.Enter)
            {
                if (CbxMetodosPago.Text == "Efectivo")
                {
                    if (txtMontoBs.Text != "" && txtMontoBs.Text != "0.00" && txtMontoBs.Text != "0")
                    {
                        _L_Facturacion.GuardarAbonoGrid(Dt_Abonos, CbxMetodosPago.Text, "Bolivares", "", txtMontoBs.Text, "", DtpFecha.Text);
                        txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);
                        LimpiarTxbox();
                    }
                }

                if (CbxMetodosPago.Text == "Iva Retenido")
                {
                    if (txtMontoBs.Text != "" && txtMontoBs.Text != "0.00" && txtMontoBs.Text != "0")
                    {
                        _L_Facturacion.GuardarAbonoGrid(Dt_Abonos, CbxMetodosPago.Text, "Bolivares", "", txtMontoBs.Text, "", DtpFecha.Text);
                        txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);
                        LimpiarTxbox();
                    }
                }

                if (CbxMetodosPago.Text == "ISLR Retenido")
                {
                    if (txtMontoBs.Text != "" && txtMontoBs.Text != "0.00" && txtMontoBs.Text != "0")
                    {
                        _L_Facturacion.GuardarAbonoGrid(Dt_Abonos, CbxMetodosPago.Text, "Bolivares", "", txtMontoBs.Text, "", DtpFecha.Text);
                        txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);
                        LimpiarTxbox();
                    }
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
                    CbxMoneda.SelectedIndex = 0;
                    txtMonto2Bs.Enabled = false;
                    txtRef.Enabled = true; ;
                    txtMontoBs.Enabled = false;
                    txtMontoBs.Text = _L_Facturacion.CalcularNuevoTotalOrden(DgvAbonos);
                    double MontoFaltanteIgtfbs = Convert.ToDouble(txtMontoBs.Text) * 0.03;
                    txtRef.Text = Convert.ToString(Math.Round(((Convert.ToDouble(txtMontoBs.Text)+ MontoFaltanteIgtfbs) / Convert.ToDouble(TB_TASA_Dolar.Tasa)), 2));
                    //txtRef.Text = Convert.ToString(Convert.ToDouble(txtRef.Text == "" ? "0.00" : Convert.ToString(txtRef.Text)) - Convert.ToDouble(TxtSaldoRef.Text));
                    VisualizarPanel("MostrarPanelSecundario");
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
                    label15.Visible = false;
                    label27.Visible = false;
                }

                if (CbxMetodosPago.Text == "Transferencia Bolivares")
                {
                    _L_Facturacion.LLenarComboboxBancos(CbxBanco, false);
                    CbxMetodosPago2.SelectedIndex = CbxMetodosPago.SelectedIndex;
                    CbxMoneda.SelectedIndex = 2;
                    txtMonto2Bs.Enabled = true;
                    txtRef.Enabled = false; 
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
                    label15.Visible = false;
                    label27.Visible = false;
                }

                if (CbxMetodosPago.Text == "Cheque")
                {
                    _L_Facturacion.LLenarComboboxBancos(CbxBanco, false);
                    CbxMetodosPago2.SelectedIndex = CbxMetodosPago.SelectedIndex;
                    CbxMoneda.SelectedIndex = 2;
                    txtMonto2Bs.Enabled = true;
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
                    label15.Visible = false;

                }

                if (CbxMetodosPago.Text == "Debito")
                {
                    _L_Facturacion.LLenarComboboxBancos(CbxBanco, false);
                    CbxMetodosPago2.SelectedIndex = CbxMetodosPago.SelectedIndex;
                    CbxMoneda.SelectedIndex = 2;
                    txtMonto2Bs.Enabled = true;
                    txtRef.Enabled = false; ;
                    txtMontoBs.Enabled = false;
                    VisualizarPanel("MostrarPanelSecundario");
                    label28.Visible = true;
                    label22.Visible = false;
                    txtRef.Visible = false;
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
                    label15.Visible = false;
                }

                if (CbxMetodosPago.Text == "Tarjeta de Credito")
                {
                    _L_Facturacion.LLenarComboboxBancos(CbxBanco, false);
                    CbxMetodosPago2.SelectedIndex = CbxMetodosPago.SelectedIndex;
                    CbxMoneda.SelectedIndex = 2;
                    txtMonto2Bs.Enabled = true;
                    txtRef.Enabled = false; ;
                    txtMontoBs.Enabled = false;
                    VisualizarPanel("MostrarPanelSecundario");
                    label28.Visible = true;
                    label22.Visible = false;
                    txtRef.Visible = false;
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
                    label15.Visible = false;
                }


                if (CbxMetodosPago.Text == "Nota Credito")
                {
                    VisualizarPanel("MostrarNotasCredito");
                }


                if (CbxMetodosPago.Text == "Efectivo")
                {
                    txtMontoBs.Enabled = true;

                }

                if (CbxMetodosPago.Text == "Iva Retenido")
                {
                    txtMontoBs.Enabled = true;

                }

                if (CbxMetodosPago.Text == "ISLR Retenido")
                {
                    txtMontoBs.Enabled = true;

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
                DgvAbonos.Columns["TipoPago"].HeaderText = "TipoPago";
                DgvAbonos.Columns["Moneda"].HeaderText = "Moneda";
                DgvAbonos.Columns["Ref"].HeaderText = "Ref";
                DgvAbonos.Columns["Banco"].HeaderText = "Banco";
                DgvAbonos.Columns["Igtf"].HeaderText = "Igtf";
                DgvAbonos.Columns["Bs"].HeaderText = "Bolivares";
                DgvAbonos.Columns["N°Tranferencia"].HeaderText = "N°Tranferencia";
                DgvAbonos.Columns["Fecha"].HeaderText = "Fecha";
                DgvAbonos.Columns["Vuelto"].HeaderText = "Vuelto";

                DgvAbonos.Columns["CVC"].HeaderText = "CVC";
                DgvAbonos.Columns["Vence"].HeaderText = "Vence";
                DgvAbonos.Columns["TipoTrajeta"].HeaderText = "TipoTrajeta";


                //Ancho de columna
                DgvAbonos.Columns["TipoPago"].Width = 100;
                DgvAbonos.Columns["Moneda"].Width = 65;
                DgvAbonos.Columns["Ref"].Width = 50;
                DgvAbonos.Columns["Banco"].Width = 50;
                DgvAbonos.Columns["Igtf"].Width = 50;
                DgvAbonos.Columns["Bs"].Width = 125;
                DgvAbonos.Columns["N°Tranferencia"].Width = 50;
                DgvAbonos.Columns["Fecha"].Width = 50;
                DgvAbonos.Columns["Vuelto"].Width = 50;
                //Bloquear Columna 


                DgvAbonos.Columns["TipoPago"].ReadOnly = true;
                DgvAbonos.Columns["Moneda"].ReadOnly = true;
                DgvAbonos.Columns["Ref"].ReadOnly = true;
                DgvAbonos.Columns["Banco"].ReadOnly = true;
                DgvAbonos.Columns["Igtf"].ReadOnly = true;
                DgvAbonos.Columns["Bs"].ReadOnly = true;
                DgvAbonos.Columns["N°Tranferencia"].ReadOnly = true;
                DgvAbonos.Columns["Fecha"].ReadOnly = true;
                DgvAbonos.Columns["Vuelto"].ReadOnly = true;
                DgvAbonos.Columns["CVC"].ReadOnly = true;
                DgvAbonos.Columns["Vence"].ReadOnly = true;
                DgvAbonos.Columns["TipoTrajeta"].ReadOnly = true;

                DgvAbonos.Columns["TipoPago"].Visible = true;
                DgvAbonos.Columns["Moneda"].Visible = true;
                DgvAbonos.Columns["Ref"].Visible = false;
                DgvAbonos.Columns["Banco"].Visible = false;
                DgvAbonos.Columns["Igtf"].Visible = false;
                DgvAbonos.Columns["Bs"].Visible = true;
                DgvAbonos.Columns["N°Tranferencia"].Visible = false;
                DgvAbonos.Columns["Fecha"].Visible = false;
                DgvAbonos.Columns["Vuelto"].Visible = false;
                DgvAbonos.Columns["CVC"].Visible = false;
                DgvAbonos.Columns["Vence"].Visible = false;
                DgvAbonos.Columns["TipoTrajeta"].Visible = false;


            }


            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }
        }

        private void btnProcesar1_Validating(object sender, CancelEventArgs e)
        {
            //if (Convert.ToDouble(_L_Facturacion.) >= )
            //{

            //}
        }

        private void btnCancelar3_Click(object sender, EventArgs e)
        {
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
                        if (_L_Facturacion.BuscarNotas(txtNroNota.Text, DgvNotas)) ;
                        else
                        {
                            mensaje = _L_Facturacion.stringBuilder.ToString();
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

                else
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("Seleciones el tipo de documento");
                    _FrmMensajes.ShowDialog();
                }
            }
        }

        private void txtNroNota_KeyPress(object sender, KeyPressEventArgs e)
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
        }

        private void button3_Click(object sender, EventArgs e)
        {

            try
            {
                ImprimirFacturaFiscal(txtNumeroOrden.Text, txtCedula.Text, txtNombreCliente.Text);

            }

            catch (Exception ex)
            {
               MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }

        }

        public bool ImprimirFacturaFiscal(string NumeroOrden, string CedulaCliente, string NacionalidadRif)
        {
            try
            {
                DataSet DtIGTF;
                NumeroComprobanteFiscal = "";
                DataSet dsArti = _D_DetalleOrden.DETALLEFACTURAFISCAL(txtNumeroOrden.Text);

                DataTable dt = dsArti.Tables[0];
                int x = 0;
                int totaldetalle = dsArti.Tables[0].Rows.Count;
                double TotalItems= 0;
                bool DctoFactura=false;
                string TipoTasaIVA= "1";
                string statusOS = "";
                string DctoExento="";
                string DctoGravable="";

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
                DtIGTF = _D_DetalleOrden.PagosConIGTF(TB_CAORDSER.Cod_Sucursal, txtNumeroOrden.Text);



                DataSet dsFacturaManual = _D_DetalleOrden.FacturaManual(TB_CAORDSER.Cod_Sucursal, txtNumeroOrden.Text);

                if ((dsFacturaManual.Tables[0].Rows.Count == 0) & (dsFacturaManual.Tables[1].Rows.Count == 0) & ((Convert.ToInt16(DtIGTF.Tables[0].Rows[0]["Abo_Monto"]) > 0 & Convert.ToInt16(DtIGTF.Tables[0].Rows[0]["FacturaManualIGTF"]) == 0) | Convert.ToInt16(DtIGTF.Tables[0].Rows[0]["Abo_Monto"]) == 0))
                {
                    FacturaManual = false;
                    ImprimirFacturaFiscall = false;

                    uint resp = 0;

                    resp = objVmax.AbrirPuerto(Convert.ToString(glbPuertoCOM));

                    if (resp != 0)
                    {
                        objVmax.CerrarPuerto();
                        ImprimirFacturaFiscall = false;
                        NumeroComprobanteFiscal = Convert.ToString(objVmax.RetornoMF.uiUltNumZ);
                    }

                    else
                    {

                        objVmax.AbrirPuerto(Convert.ToString(glbPuertoCOM));
                        bool respuesta = false;
                        // ''''' ********* DATOS DEL CLIENTE ************
                        resp = objVmax.AbrirCF(txtNombreCliente.Text, txtCedula.Text, "1", "294", "12345", "", "", 40);


                        /// **********'IMPRIMO LOS ITEMS *********************
                      
                        string desart;

                        foreach (DataRow drItem in dt.Rows)
                        {
                            // EL DESCUENTO DE LOS ARTICULOS SE ENVIARA AL FINAL, ANTES DE CERRAR EL CF

                            if (Convert.ToInt16(drItem["Ordserv_Dto"]) > 0)
                            {
                                DctoFactura = true;
                            }

                            desart = drItem["CodArticulo"] + " " + drItem["DESART"];

                            if (Convert.ToString(drItem["ART_EXENTO"]) == "1")
                            {
                                TipoTasaIVA = "0";
                            }


                            resp = objVmax.Item(desart, Convert.ToString(Convert.ToDouble(drItem["Ordserv_Cant"]) * 1000), drItem["OrdServ_Precio"].ToString(), TipoTasaIVA, "1", 40);
                           
                            TotalItems = TotalItems + ((Convert.ToDouble(drItem["OrdServ_Precio"]) * Convert.ToInt32(drItem["Ordserv_Cant"]))) + Convert.ToDouble(drItem["OrdServ_Impuesto"]);

                        }


                        Double totalpagos =0;
                        bool sumo01 = false;


                        // ************SUB TOTAL DEL CF ********************
                        // Si la impresora devuelve true imprimo  subtotal
                        if (resp == 0)
                        {

                            // OBTENGO LA SUMATORIA DE LOS ABONOS PARA SABER SI COINCIDEN CON EL TOTAL DE LOS ARTICULOS  PARA CANCELAR EL CF ANTES DE ENVIAR EL SUBTOTAL. 
                            DataTable dtPago = _D_DetalleOrden.TEMP_ABONO(txtNumeroOrden.Text);

                            foreach (DataRow drPago in dtPago.Rows)
                            {
                                totalpagos = totalpagos + (Convert.ToDouble(drPago["Abo_Monto"].ToString().Replace(",", "")));
                            }

 
                            // ----CONSULTO PAGOS EN DIVISA
                            if (Convert.ToInt16(DtIGTF.Tables[0].Rows[0]["Abo_Monto"]) > 0 & Convert.ToBoolean(DtIGTF.Tables[0].Rows[0]["ActivaIGTF"]) == true)
                            {
                                TotalItems = TotalItems + Convert.ToDouble(DtIGTF.Tables[0].Rows[0]["Abo_IGTF"]);
                            }
                                

                            // ----CONSULTO LOS DESCUENTOS DE LA FACTURA 
                            DataSet dsDcto = _D_DetalleOrden.DESCUENTOSFACTURAFISCAL(txtNumeroOrden.Text);
                            DataTable dtcto = dsDcto.Tables[0];
                            Double DescuentoExento = Math.Round(Convert.ToDouble(dtcto.Rows[0]["DescuentoExento"].ToString()), 2);
                            Double DescuentoGravable = Math.Round(Convert.ToDouble(dtcto.Rows[0]["DescuentoGravable"].ToString()), 2);
                            Double DescuentoGravableA = Math.Round(Convert.ToDouble(dtcto.Rows[0]["DescuentoGravableA"].ToString()), 2);
                            Double DescuentoGravableR = Math.Round(Convert.ToDouble(dtcto.Rows[0]["DescuentoGravableR"].ToString()), 2);

                            if(totalpagos != (TotalItems- DescuentoExento- DescuentoGravable- DescuentoGravableA- DescuentoGravableR))
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
                                    // borro los pobles abonos que hayan quedado en la tabla
                                    _D_DetalleOrden.Delete_Sencillo("TEMP_ABONO",txtNumeroOrden.Text);


                                }
                            }

                            else
                            {
                                // MsgBox("desc")
                                // ****ENVIO LOS DESCUENTOS DE ESTA FACTURA******
                                if (DctoFactura == true)
                                {
                                    foreach (DataRow drItemdcto in dtcto.Rows)
                                    {
                                        // resp = VMAX1.DescuentoCF("Descuento", drItemdcto("DescuentoExento"), drItemdcto("DescuentoGravable"), "", "")
                                        if (TipoTasaIVA == "1")

                                           resp = objVmax.DescuentoCF("Descuento", drItemdcto["DescuentoExento"].ToString(), drItemdcto["DescuentoGravable"].ToString(), "", "", "");

                                        if (TipoTasaIVA == "3")

                                            resp = objVmax.DescuentoCF("Descuento", drItemdcto["DescuentoExento"].ToString(), "","",drItemdcto["DescuentoGravableA"].ToString(), "");


                                        if (TipoTasaIVA == "2")

                                            resp= objVmax.DescuentoCF("Descuento", drItemdcto["DescuentoExento"].ToString(), "", drItemdcto["DescuentoGravableR"].ToString(), "","");

                                        DctoExento = drItemdcto["DescuentoExento"].ToString();
                                        DctoGravable = drItemdcto["DescuentoGravable"].ToString() + drItemdcto["DescuentoGravableA"].ToString() + drItemdcto["DescuentoGravableR"].ToString();
                                    }
                                }


                                // ******ENVIO EL SUBTOTAL DE LA FACTURA**************
                                if (resp == 0) 
                                {

                                    // Verifico si la orden tiene Igtf
                                    if (Convert.ToBoolean(DtIGTF.Tables[0].Rows[0]["ActivaIGTF"].ToString()) == true & Convert.ToInt16(DtIGTF.Tables[0].Rows[0]["Abo_Monto"].ToString()) > 0)
                                    {

                                        if (DtIGTF.Tables[0].Rows[0]["IGTFCAORDSER"].ToString() == DtIGTF.Tables[0].Rows[0]["IGTFCALC"].ToString())
                                             resp = objVmax.SubtotalT_sinRetorno(DtIGTF.Tables[0].Rows[0]["Abo_Monto"].ToString());
                                        else
                                            respuesta = false;
                                    }

                                    else
                                    {
                                        // If DtIGTF.Tables(0).Rows(0)("Abo_Monto") = 0 Then
                                        resp = objVmax.Subtotal();

                                        //Modificarrrr
                                        TotalFacturaFiscal = objVmax.RetornoSubtotal.ToString();
                                        NumeroComprobanteFiscal = objVmax.RetornoAbrirFactura.uiNumeroFactura.ToString();

                                    }
                                      
                                }
                                else
                                {
                                    objVmax.Cancelar();
                                    ImprimirFacturaFiscall = false;
                                    respuesta = false;

                                }
                            }


                            if (sumo01 == true)
                            {
                                // ****ENVIO LOS DESCUENTOS DE ESTA FACTURA******
                                if (DctoFactura == true)
                                {
                               
                                    foreach (DataRow drItemdcto in dtcto.Rows)
                                    {

                                        // resp = VMAX1.DescuentoCF("Descuento", drItemdcto("DescuentoExento"), drItemdcto("DescuentoGravable"), "", "")
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
                                if (resp != 0)
                                {
                                    if (Convert.ToInt16(DtIGTF.Tables[0].Rows[0]["ActivaIGTF"].ToString()) > 0 & Convert.ToInt16(DtIGTF.Tables[0].Rows[0]["Abo_Monto"].ToString()) > 0)
                                    {
                                        if (DtIGTF.Tables[0].Rows[0]["IGTFCAORDSER"].ToString() == DtIGTF.Tables[0].Rows[0]["IGTFCALC"].ToString())
                                            resp = objVmax.SubtotalT_sinRetorno(DtIGTF.Tables[0].Rows[0]["Abo_Monto"].ToString());
                                         else
                                            respuesta = false;

                                    }
                                    else

                                    resp = objVmax.Subtotal();
                                    //respuesta = Vmax.LeoDatosFiscales()
                                    TotalFacturaFiscal = objVmax.RetornoSubtotal.ToString();

                                }
                                else
                                {
                                    objVmax.Cancelar();
                                    ImprimirFacturaFiscall = false;
                                    respuesta = false;

                                }
                            }
                        }

                        else
                            
                        ImprimirFacturaFiscall = false;
                  
                        // *************ENVIO LOS PAGOS***********************
                        if (resp == 0)
                        {
                            DataTable dtPago = _D_DetalleOrden.Pasgos(TB_CAORDSER.Cod_Sucursal, txtNumeroOrden.Text);
                            TotalFacturaFiscal = objVmax.RetornoSubtotal.ToString();
                            decimal PagosEnviados=0;

                            foreach (DataRow drPago in dtPago.Rows)
                            {
                                if (Convert.ToDecimal(drPago["Abo_Monto"].ToString()) > 0)
                                {
                                    int NumeroMaximoCaracteres = Convert.ToInt32(drPago["Abo_Tipo"].ToString().Length);

                                    if (drPago["Abo_Tipo"].ToString().Substring(11, NumeroMaximoCaracteres - 11) == "DIVISAS")
                                    {
                                        if (DtIGTF.Tables[0].Rows[0]["Abo_Monto"].ToString() != drPago["Abo_Monto"].ToString().Replace(",", ""))
                                        {
                                            respuesta = false;
                                            break;
                                        }
                                    }

                                    //resp = objVmax.PagoCF(drPago["Abo_Tipo"].ToString(), drPago["Abo_Monto"].ToString().Replace(",", ""), 1);
                                   
                                    resp = objVmax.PagoCF(drPago["Abo_Monto"].ToString(),drPago["Abo_Tipo"].ToString(), 1);
                                    PagosEnviados = PagosEnviados + Convert.ToDecimal(drPago["Abo_Monto"].ToString());
                                }
                            }

                            decimal Calculo= Convert.ToDecimal(TotalFacturaFiscal) - PagosEnviados;

                            if (Convert.ToString(Convert.ToDecimal(TotalFacturaFiscal) - PagosEnviados) == "001" )
                            {
                                resp = objVmax.PagoCF( "1", "EFECTIVO", 1);
                                _D_DetalleOrden.SP_SUMOFACTURA(TB_CAORDSER.Cod_Sucursal, NumeroComprobanteFiscal, objVmax.RetornoMF.sSerial.ToString(), txtNumeroOrden.Text, TotalFacturaFiscal, DctoExento, DctoGravable);
                            }
                            else if (Convert.ToString(Convert.ToDecimal(TotalFacturaFiscal) - PagosEnviados) == "002")
                            {
                                resp = objVmax.PagoCF("2", "EFECTIVO", 1);
                                _D_DetalleOrden.SP_SUMOFACTURA(TB_CAORDSER.Cod_Sucursal, NumeroComprobanteFiscal, objVmax.RetornoMF.sSerial.ToString(), txtNumeroOrden.Text, TotalFacturaFiscal, DctoExento, DctoGravable);
                            }
                            else if (Convert.ToString(Convert.ToDecimal(TotalFacturaFiscal) - PagosEnviados) == "003")
                            {
                                resp = objVmax.PagoCF("3", "EFECTIVO", 1);
                                _D_DetalleOrden.SP_SUMOFACTURA(TB_CAORDSER.Cod_Sucursal, NumeroComprobanteFiscal, objVmax.RetornoMF.sSerial.ToString(), txtNumeroOrden.Text, TotalFacturaFiscal, DctoExento, DctoGravable);
                            }
                            else if (Convert.ToString(Convert.ToDecimal(TotalFacturaFiscal) - PagosEnviados) == "004")
                            {
                                resp = objVmax.PagoCF("4", "EFECTIVO", 1);
                                _D_DetalleOrden.SP_SUMOFACTURA(TB_CAORDSER.Cod_Sucursal, NumeroComprobanteFiscal, objVmax.RetornoMF.sSerial.ToString(), txtNumeroOrden.Text, TotalFacturaFiscal, DctoExento, DctoGravable);
                            }
                            else if (Convert.ToString(Convert.ToDecimal(TotalFacturaFiscal) - PagosEnviados) == "005")
                            {
                                resp = objVmax.PagoCF("5", "EFECTIVO", 1);
                                _D_DetalleOrden.SP_SUMOFACTURA(TB_CAORDSER.Cod_Sucursal, NumeroComprobanteFiscal, objVmax.RetornoMF.sSerial.ToString(), txtNumeroOrden.Text, TotalFacturaFiscal, DctoExento, DctoGravable);

                            }

                        }
                        else
                            ImprimirFacturaFiscall = false;


                        if (resp == 0)
                        {
                            // Si la impresora devuelve true imprimo los comentarios y cierro el CF

                            resp = objVmax.TextoNoFiscal("");
                            resp = objVmax.TextoNoFiscal("Numero Orden: " + txtNumeroOrden.Text);
                            resp = objVmax.TextoNoFiscal("");

  
                            if ((_D_DetalleOrden.TB_PARAMETROSPGE("FactFiscalconRxPGE")) == "1")
                            {
                                //oOrdenServicio.ObtenerOrdenServicio(NumeroOrdenImprimir, glbSucursalActual, sqlCom);

                                if (TB_CAORDSER.Asegurada== true)
                                {

                                //    CapaNegocio.Formulas oFormulas = new CapaNegocio.Formulas();
                                //    double ESFD;
                                //    double ESFI;
                                //    double CILD;
                                //    double CILI;
                                //    int EJED;
                                //    int EJEI;
                                //    double ADDD;
                                //    double ADDI;
                                //    // 2da Refraccion
                                //    double ESFD2;
                                //    double ESFI2;
                                //    double CILD2;
                                //    double CILI2;
                                //    int EJED2;
                                //    int EJEI2;

                                //    // oExamenConv.ObtenerRx(NacionalidadRif, CedulaCliente, oOrdenServicio.NumeroExamen, glbSucursalActual, sqlCom)
                                //    oExamenConv.ObtenerRx(oOrdenServicio.ClienteNacionalidad, oOrdenServicio.ClienteCedulaIdentidad, oOrdenServicio.NumeroExamen, glbSucursalActual, sqlCom);

                                //    ESFD = oExamenConv.ESFD;
                                //    ESFI = oExamenConv.ESFI;
                                //    CILD = oExamenConv.CILD;
                                //    CILI = oExamenConv.CILI;
                                //    EJED = oExamenConv.EJED;
                                //    EJEI = oExamenConv.EJEI;
                                //    ADDD = oExamenConv.ADDD;
                                //    ADDI = oExamenConv.ADDI;

                                //    // 2da Refraccion
                                //    ESFD2 = oExamenConv.ESFD2;
                                //    ESFI2 = oExamenConv.ESFI2;
                                //    CILD2 = oExamenConv.CILD2;
                                //    CILI2 = oExamenConv.CILI2;
                                //    EJED2 = oExamenConv.EJED2;
                                //    EJEI2 = oExamenConv.EJEI2;

                                //    if (oExamenConv.CILD > 0)
                                //    {
                                //        oFormulas.Transposicion(ESFD, CILD, EJED);
                                //        ESFD = oFormulas.ESF;
                                //        CILD = oFormulas.CIL;
                                //        EJED = oFormulas.EJE;
                                //    }
                                //    if (oExamenConv.CILI > 0)
                                //    {
                                //        oFormulas.Transposicion(ESFI, CILI, EJEI);
                                //        ESFI = oFormulas.ESF;
                                //        CILI = oFormulas.CIL;
                                //        EJEI = oFormulas.EJE;
                                //    }

                                //    // 2da Refraccion
                                //    if (oExamenConv.CILD2 > 0)
                                //    {
                                //        oFormulas.Transposicion(ESFD2, CILD2, EJED2);
                                //        ESFD2 = oFormulas.ESF;
                                //        CILD2 = oFormulas.CIL;
                                //        EJED2 = oFormulas.EJE;
                                //    }
                                //    if (oExamenConv.CILI2 > 0)
                                //    {
                                //        oFormulas.Transposicion(ESFI2, CILI2, EJEI2);
                                //        ESFI2 = oFormulas.ESF;
                                //        CILI2 = oFormulas.CIL;
                                //        EJEI2 = oFormulas.EJE;
                                //    }
                                //    resp = Vmax.TextoDNF("*Contrato de Garantia Extendida para");
                                //    resp = Vmax.TextoDNF("      Cristales Formulados:");
                                //    resp = Vmax.TextoDNF(" ");

                                //    resp = Vmax.TextoDNF("OD: ESF " + Strings.Format((float)ESFD, "#,##0.00") + " EJE " + EJED + " CIL " + Strings.Format((float)CILD, "#,##0.00") + " ADD " + Strings.Format((float)ADDD, "#,##0.00"));
                                //    // 2da Refraccion
                                //    if (ESFD2 != 0)
                                //    {
                                //        resp = Vmax.TextoDNF("2da. Refracción");
                                //        resp = Vmax.TextoDNF("OD: ESF " + Strings.Format((float)ESFD2, "#,##0.00") + " EJE " + EJED2 + " CIL " + Strings.Format((float)CILD2, "#,##0.00"));
                                //    }

                                //    resp = Vmax.TextoDNF("OI: ESF " + Strings.Format((float)ESFI, "#,##0.00") + " EJE " + EJEI + " CIL " + Strings.Format((float)CILI, "#,##0.00") + " ADD " + Strings.Format((float)ADDI, "#,##0.00"));
                                //    // 2da Refraccion
                                //    if (ESFI2 != 0)
                                //    {
                                //        resp = Vmax.TextoDNF("2da. Refracción");
                                //        resp = Vmax.TextoDNF("OI: ESF " + Strings.Format((float)ESFI2, "#,##0.00") + " EJE " + EJEI2 + " CIL " + Strings.Format((float)CILI2, "#,##0.00"));
                                //    }

                                //    resp = Vmax.TextoDNF(" ");
                                //    resp = Vmax.TextoDNF("Condiciones legales en el Contrato");
                                //    resp = Vmax.TextoDNF("Suscrito.");
                                //    resp = Vmax.TextoDNF(" ");
                                //}
                            }


                            DataTable DtTexto = _D_DetalleOrden.TB_INUTILIZADO();

                            foreach (DataRow DrTexto in DtTexto.Rows)
                            {
                                resp = objVmax.TextoNoFiscal(DrTexto["texto"].ToString());
                            }

                                resp = objVmax.Cerrar();
                                resp = objVmax.CerrarPuerto();

                                if (resp == 0)
                            {
                                ImprimirFacturaFiscall = true;
                                statusOS = "002";
                                NumeroComprobanteFiscal = objVmax.RetornoAbrirFactura.uiNumeroFactura.ToString();

                                    switch (NumeroComprobanteFiscal.Length)
                                {
                                    case 7:
                                        {
                                            NumeroComprobanteFiscal = NumeroComprobanteFiscal;
                                            break;
                                        }

                                    case 6:
                                        {
                                            NumeroComprobanteFiscal = "0" + NumeroComprobanteFiscal;
                                            break;
                                        }

                                    case 5:
                                        {
                                            NumeroComprobanteFiscal = "00" + NumeroComprobanteFiscal;
                                            break;
                                        }

                                    case 4:
                                        {
                                            NumeroComprobanteFiscal = "000" + NumeroComprobanteFiscal;
                                            break;
                                        }

                                    case 3:
                                        {
                                            NumeroComprobanteFiscal = "0000" + NumeroComprobanteFiscal;
                                            break;
                                        }

                                    case 2:
                                        {
                                            NumeroComprobanteFiscal = "00000" + NumeroComprobanteFiscal;
                                            break;
                                        }

                                    case 1:
                                        {
                                            NumeroComprobanteFiscal = "000000" + NumeroComprobanteFiscal;
                                            break;
                                        }
                                }

                                //CapaNegocio.Usuario oUsu = new CapaNegocio.Usuario();
                                //oUsu.ObtenerUsuarioCodigo(glbUsuarioActual, Interaction.Command);
                                //oAuditor.AgregarRegistroAuditor(glbSucursalActual, "071", oUsu.CarnetEmpleado, "OS: " + NroOrdenServicio + ", Factura: " + NumeroComprobanteFiscal + ", Serial: " + Vmax.RetornoSerial, Interaction.Command);
                            
                            }
                            else
                            {
                                    objVmax.Cancelar();
                                    objVmax.Cerrar();
                                    resp = objVmax.CerrarPuerto();
                                    ImprimirFacturaFiscall = false;
                            }
                        }

                }

                else
                {
         
                }

            return respuesta;
            }
                   
            }

            return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
                objVmax.Cancelar();
                objVmax.Cerrar();
                objVmax.CerrarPuerto();
                return false;
            }
           
        }


    }
}
