using CapaEntidades;
using CapaLogica.DetalleOrden_Logica;
using CapaLogica.Impresora_Fiscal;
using CapaVisual_Externa;
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

namespace CapaVisual_Facturacion
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
        public DataTable Dt_Abonos = new DataTable();
        private string mensaje = "";

        private void FrmDetalleOrden_Load(object sender, EventArgs e)
        {
            _L_Facturacion.ComboboxTipoMoneda(CbxMoneda);
            _L_Facturacion.LLenarComboboxPagos(CbxMetodosPago);
            _L_Facturacion.LLenarComboboxPagos(CbxMetodosPago2);
            FormatoTexbox();
            _L_Facturacion.CrearTabla(Dt_Abonos);
            DgvAbonos.DataSource = Dt_Abonos;
            lbTasa.Text = Convert.ToString(TB_TASA_Dolar.Tasa);
            _L_Facturacion.ComboboxTipoTarjeta(CbxTarjeta);
            FormatoDataGrid();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            VisualizarPanel("MostrarPanelPrincipal");

            lbMinAbo.Text = Convert.ToString(Math.Round(((0.80 * Convert.ToDouble(TxtMontoRef.Text))), 2));
            lbMinAbo.Text = Convert.ToString(Convert.ToDouble(lbMinAbo.Text) * TB_TASA_Dolar.Tasa);
            txtMontoBs.Text= Convert.ToString(TxtSaldoOrd.Text);

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
                if (Math.Round((double)(Convert.ToDouble(txtMontoBs.Text) / TB_TASA_Dolar.Tasa), 2) >= (Convert.ToDouble(txtRef.Text) - (Convert.ToDouble(TxtSaldoRef.Text))))
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
            txtRef.Text = "";

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
            txtIGTF.Text = "";
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
            txtCVC.Text = "";
            txtVence.Text = "";
            txtCheque.Text = "";

            CbxMetodosPago.SelectedIndex = 0;

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
                if (CbxMetodosPago2.Text== "Transferencia Divisa")
                {

                //'Si los campos poseen valores proceso los datos
                if (txtRef.Text != "0.00" && txtMonto2Bs.Text != "0.00" && txtIGTF.Text != "0.00" && txtTranferencia.Text != "" && CbxBanco.Text != "")
                {
                    _L_Facturacion.GuardarAbonoGrid(Dt_Abonos, CbxMetodosPago2.Text, CbxMoneda.Text, CbxBanco.Text, txtMonto2Bs.Text, txtTranferencia.Text, DtpFecha.Text, TxtVuelto.Text, txtRef.Text, txtIGTF.Text);
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

                        _L_Facturacion.GuardarAbonoGrid(Dt_Abonos, CbxMetodosPago2.Text, CbxMoneda.Text, CbxBanco.Text, txtMonto2Bs.Text, txtTranferencia.Text, DtpFecha.Text, TxtVuelto.Text);
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
                    if (txtMonto2Bs.Text != "0.00" && txtTranferencia.Text != "" && CbxBanco.Text != "")
                    {
                        _L_Facturacion.GuardarAbonoGrid(Dt_Abonos, CbxMetodosPago2.Text, CbxMoneda.Text, CbxBanco.Text, txtMonto2Bs.Text, txtTranferencia.Text, DtpFecha.Text, TxtVuelto.Text, txtRef.Text, txtIGTF.Text, txtCVC.Text, txtVence.Text, CbxTarjeta.Text);
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
                        if (txtMonto2Bs.Text != "0.00" && txtTranferencia.Text != "" && CbxBanco.Text != "" && txtCVC.Text != "" && txtVence.Text != "")
                        {
                            _L_Facturacion.GuardarAbonoGrid(Dt_Abonos, CbxMetodosPago2.Text, CbxMoneda.Text, CbxBanco.Text, txtMonto2Bs.Text, txtTranferencia.Text, DtpFecha.Text, TxtVuelto.Text, txtRef.Text, txtIGTF.Text, txtCVC.Text, txtVence.Text, CbxTarjeta.Text);
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


        public void CargarDatosOrden(string NumeroOrden)
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
            if (Math.Round((double)(Convert.ToDouble(txtMontoBs.Text)/ TB_TASA_Dolar.Tasa), 2) >= (Convert.ToDouble(txtRef.Text) - (Convert.ToDouble(TxtSaldoRef.Text))))
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
                if (CbxMetodosPago.Text == "Efectivo" || CbxMetodosPago.Text == "Iva Retenido" || CbxMetodosPago.Text == "ISLR Retenido")
                {
                    if (txtMontoBs.Text!= "" && txtMontoBs.Text != "0.00" && txtMontoBs.Text != "0")
                    {
                        CbxMoneda.SelectedIndex = 2;
                        _L_Facturacion.GuardarAbonoGrid(Dt_Abonos, CbxMetodosPago.Text, CbxMoneda.Text, "", txtMontoBs.Text, "", DtpFecha.Text, "");
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
                        txtRef.Text = Convert.ToString(Convert.ToDouble(txtRef.Text == "" ? "0.00" : Convert.ToString(txtRef.Text)) - Convert.ToDouble(TxtSaldoRef.Text));
                        VisualizarPanel("MostrarPanelSecundario");
                        label22.Visible = true;

                       txtCVC.Visible = false;
                       label26.Visible = false;
                       CbxTarjeta.Visible = false;
                       label24.Visible = false;
                       txtVence.Visible = false;
                       label27.Visible = false;
                       txtCheque.Visible = false;
                       label28.Visible= false;

                }

                    if (CbxMetodosPago.Text == "Transferencia Bolivares")
                    {
                        _L_Facturacion.LLenarComboboxBancos(CbxBanco, false);
                        CbxMetodosPago2.SelectedIndex = CbxMetodosPago.SelectedIndex;
                        CbxMoneda.SelectedIndex = 2;
                        txtMonto2Bs.Enabled = true;
                        txtRef.Enabled = false; ;
                        txtMontoBs.Enabled = false;
                        VisualizarPanel("MostrarPanelSecundario");
                        label28.Visible = false;
                        label22.Visible = true;
                   
                        txtCVC.Visible = false;
                        label26.Visible = false;
                        CbxTarjeta.Visible = false;
                        label24.Visible = false;
                        txtVence.Visible = false;
                        label27.Visible = false;
                        txtRef.Enabled = false;
                        txtCheque.Visible = false;
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
                    label22.Visible = true;

                    txtCVC.Visible = false;
                    label26.Visible = false;
                    CbxTarjeta.Visible = false;
                    label24.Visible = false;
                    txtVence.Visible = false;
                    label27.Visible = false;
                    txtRef.Enabled = false;
                    txtCheque.Visible = false;
                   
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
                DgvAbonos.Columns["Bs"].HeaderText = "Bolivare";

                DgvAbonos.Columns["Vuelto"].HeaderText = "Vuelto";
                DgvAbonos.Columns["CVC"].HeaderText = "CVC";
                DgvAbonos.Columns["Vence"].HeaderText = "Fecha";
                DgvAbonos.Columns["TipoTrajeta"].HeaderText = "TipoTrajeta";


                //Ancho de columna
                DgvAbonos.Columns["TipoPago"].Width = 160;
                DgvAbonos.Columns["Moneda"].Width = 75;
                DgvAbonos.Columns["Ref"].Width = 50;
                DgvAbonos.Columns["Banco"].Width = 50;
                DgvAbonos.Columns["Igtf"].Width = 50;
                DgvAbonos.Columns["Bs"].Width = 100;
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


                DgvAbonos.Columns["TipoPago"].Visible = true;
                DgvAbonos.Columns["Moneda"].Visible = true;
                DgvAbonos.Columns["Ref"].Visible = false;
                DgvAbonos.Columns["Banco"].Visible = false;
                DgvAbonos.Columns["Igtf"].Visible = false;
                DgvAbonos.Columns["Bs"].Visible = true;
                DgvAbonos.Columns["N°Tranferencia"].Visible = false;
                DgvAbonos.Columns["Fecha"].Visible = false;
                DgvAbonos.Columns["Vuelto"].Visible = false;
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
                    if (txtNroNota.Text != "" )
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
            _Impresora_Fiscal.AbrirPuerto(glbPuertoCOM);
        }

        private void btnProcesar3_Click(object sender, EventArgs e)
        {

        }

        private void btnProcesar1_Click(object sender, EventArgs e)
        {
            string respuesta= _L_Facturacion.CargarAbonos(DgvAbonos);
        }
    }
}
