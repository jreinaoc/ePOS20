using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaLogica.ListaOrden_Logica;
using CapaDatos.DetalleOrden_Datos;
using CapaDatos.Inicio_Datos;
using CapaLogica.DetalleOrden_Logica;
using EnvioPagoMovil;
using Newtonsoft.Json;
using System.Data.SqlClient;
using CapaDatos.Conexion;
using System.Net.NetworkInformation;
using System.Diagnostics;


namespace CapaVisual_Login
{
    public partial class FrmPagoMovil : Form
    {
        public FrmPagoMovil()
        {
            InitializeComponent();
        }

        FrmFacturacion _FrmFacturacion = new FrmFacturacion();
        FrmClaveAutorizada _FrmClaveAutorizada = new FrmClaveAutorizada();
        FrmClaveGerente _FrmClaveGerente = new FrmClaveGerente();
        private D_Inicio _D_Inicio = new D_Inicio();
        private D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        private L_ListaOrdenes _ListaOrdenes = new L_ListaOrdenes();
        private L_Facturacion _L_Facturacion = new L_Facturacion();
        private FrmMensajes _FrmMensajes = new FrmMensajes();
        //string macAddress;
        private void FrmPagoMovil_Load(object sender, EventArgs e)
        {


            //DtpDesde.CustomFormat = "yyyy/MM/dd";
            //DtpDesde.Visible = false;
            //DtpHasta.Visible = false;
            ////DtpHasta.CustomFormat = "yyyy/MM/dd";
            //Lbldesde.Visible = false;
            //LblHasta.Visible = false;
            ////EstructuraGrid();




            DataSet Dts = _ListaOrdenes.TraerOrdenesConPagoMovil(DtpDesde, DtpHasta);
            if (Dts != null)
            {
                DgvListadoOrdenes.DataSource = Dts.Tables[0];
                //Paginado(Dts);
                //Paginado_Habilitar(true);
            }
            else
            {
                //Paginado_Habilitar(false);
            }


            if (DgvListadoOrdenes.Rows.Count > 0)
            {
                DgvListadoOrdenes.Visible = true;
                CrearObjetos();
                ColorearStatus();
                DesencriptarDatos();

            }
            else
            {
                DgvListadoOrdenes.Visible = false;
                //LblOpciones.Visible = false;
            }




        }

        private void CrearObjetos()
        {
            try
            {

                //DataGridViewComboBoxColumn columnCbn = new DataGridViewComboBoxColumn();
                //columnCbn.HeaderText = "Opciones";
                //columnCbn.Name = "CbnOpciones";
                //columnCbn.DataSource = _ListaOrdenes.LLenarComboboxOpciones();
                //columnCbn.DisplayMember = "Value";
                //columnCbn.ValueMember = "Index";
                //columnCbn.SortMode = DataGridViewColumnSortMode.Automatic;

                //DgvListadoOrdenes.Columns.Add(columnCbn);

                //LblOpciones.Visible = true;

                DataGridViewButtonColumn columnBtn = new DataGridViewButtonColumn();
                columnBtn.Name = "Btn";
                columnBtn.Width = 50;
                columnBtn.HeaderText = "";
                DgvListadoOrdenes.Columns.Add(columnBtn);


                DataGridViewButtonColumn columnBtn2 = new DataGridViewButtonColumn();
                columnBtn2.Name = "Btn2";
                columnBtn2.Width = 50;
                columnBtn2.HeaderText = "";
                DgvListadoOrdenes.Columns.Add(columnBtn2);




                EstructuraGrid();
            }

            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }
        }


        private void ColorearStatus()
        {
            System.Drawing.Color colfact = System.Drawing.ColorTranslator.FromHtml("#027b74");
            System.Drawing.Color colabon = System.Drawing.ColorTranslator.FromHtml("#f59e51");
            System.Drawing.Color colporp = System.Drawing.ColorTranslator.FromHtml("#e9e9e9");
            System.Drawing.Color colanul = System.Drawing.ColorTranslator.FromHtml("#ff353a");
            System.Drawing.Color IVAS_ISLR = System.Drawing.ColorTranslator.FromHtml("#1881b0");
            System.Drawing.Color colfall = System.Drawing.ColorTranslator.FromHtml("#00008B");


            try
            {


                foreach (DataGridViewRow Fila in DgvListadoOrdenes.Rows)
                {

                    //Padding newPadding = new Padding(10,10,10,10);


                    string Status = Fila.Cells["Estado"].Value.ToString();
                    //string PAGOS_IVA = Fila.Cells["PAGOS_IVA"].Value.ToString();
                    //string PAGOS_ISLR = Fila.Cells["PAGOS_ISLR"].Value.ToString();
                    //string Comprobante_IVA = Fila.Cells["Comprobante_IVA"].Value.ToString();
                    //string Comprobante_ISLR = Fila.Cells["Comprobante_ISLR"].Value.ToString();
                    if (Status == "PENDIENTE")
                    {
                        Fila.Cells["Estado"].Style.BackColor = colabon;
                        //Fila.Cells["Estatus"].Style.Padding = newPadding;
                        DgvListadoOrdenes.Columns["Estado"].DefaultCellStyle.Format = "C";
                        Fila.Cells["Estado"].Style.ForeColor = Color.White;
                        //DgvListadoOrdenes.Rows[e.RowIndex].Cells["Btn"]
                        //DgvListadoOrdenes.Columns["Btn"].Enabled



                    }

                    if (Status == "BLOQUEADO")
                    {
                        Fila.Cells["Estado"].Style.BackColor = colfall;
                        //Fila.Cells["Estatus"].Style.Padding = newPadding;
                        DgvListadoOrdenes.Columns["Estado"].DefaultCellStyle.Format = "C";
                        Fila.Cells["Estado"].Style.ForeColor = Color.White;
                    }

                    if (Status == "REALIZADO")
                    {
                        Fila.Cells["Estado"].Style.BackColor = colfact;
                        //Fila.Cells["Estatus"].Style.Padding = newPadding;
                        DgvListadoOrdenes.Columns["Estado"].DefaultCellStyle.Format = "C";
                        Fila.Cells["Estado"].Style.ForeColor = Color.White;
                        //Fila.Cells["Estado"].Style.ForeColor = Color.FromArgb(89, 190, 186);

                    }

                    if (Status == "FALLIDO")
                    {
                        Fila.Cells["Estado"].Style.BackColor = colanul;
                        //Fila.Cells["Estatus"].Style.Padding = newPadding;
                        DgvListadoOrdenes.Columns["Estado"].DefaultCellStyle.Format = "C";
                        Fila.Cells["Estado"].Style.ForeColor = Color.White;
                        //Fila.Cells["Estado"].Style.ForeColor = Color.FromArgb(89, 190, 186);

                    }



                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }
        }

        public void EstructuraGrid()
        {
            //Con esta funcion se le da la estructura a las columnas del grid 
            try
            {

                //Centrar todas las columnas 
                DgvListadoOrdenes.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


                Padding newPadding = new Padding(15, 15, 15, 15);
                DgvListadoOrdenes.ColumnHeadersDefaultCellStyle.Padding = newPadding;


                //asignar Nombres a cada columna 
                DgvListadoOrdenes.Columns["Fecha"].HeaderText = "Fecha";
                DgvListadoOrdenes.Columns["NroOrden"].HeaderText = "Orden";
                DgvListadoOrdenes.Columns["Nombre"].HeaderText = "Nombre";
                DgvListadoOrdenes.Columns["Nacionalidad"].HeaderText = "Nacionalidad";
                DgvListadoOrdenes.Columns["Cedula"].HeaderText = "Cédula";
                DgvListadoOrdenes.Columns["Telefono"].HeaderText = "Teléfono";
                DgvListadoOrdenes.Columns["CodBancoReceptor"].HeaderText = "CodBancoReceptor";
                DgvListadoOrdenes.Columns["CodBancoReceptorEpos"].HeaderText = "CodBancoReceptorEpos";
                DgvListadoOrdenes.Columns["BancoReceptor"].HeaderText = "Banco";
                DgvListadoOrdenes.Columns["MontoVueltoBs"].HeaderText = "Monto";
                DgvListadoOrdenes.Columns["MontoVueltoRef"].HeaderText = "MontoRef";
                DgvListadoOrdenes.Columns["MontoRecibidoRef"].HeaderText = "MontoRecibidoRef";
                DgvListadoOrdenes.Columns["Referencia"].HeaderText = "Referencia";
                DgvListadoOrdenes.Columns["Estado"].HeaderText = "Estado";
                DgvListadoOrdenes.Columns["NroFactura"].HeaderText = "NroFactura";
                DgvListadoOrdenes.Columns["IdAbono"].HeaderText = "IdAbono";
                DgvListadoOrdenes.Columns["Correlativo"].HeaderText = "IdAbono";
                DgvListadoOrdenes.Columns["Btn"].HeaderText = "Btn";
                DgvListadoOrdenes.Columns["Btn2"].HeaderText = "Btn2";

                //Ancho de columna
                DgvListadoOrdenes.Columns["Fecha"].Width = 27;
                DgvListadoOrdenes.Columns["NroOrden"].Width = 24;
                DgvListadoOrdenes.Columns["Nombre"].Width = 58;
                DgvListadoOrdenes.Columns["Cedula"].Width = 30;
                DgvListadoOrdenes.Columns["Telefono"].Width = 33;
                DgvListadoOrdenes.Columns["BancoReceptor"].Width = 47;

                DgvListadoOrdenes.Columns["MontoVueltoBs"].Width = 25;
                DgvListadoOrdenes.Columns["Referencia"].Width = 34;
                DgvListadoOrdenes.Columns["Estado"].Width = 35;

                DgvListadoOrdenes.Columns["Btn"].Width = 80;
                DgvListadoOrdenes.Columns["Btn2"].Width = 80;

                //Bloquear Columna 
                DgvListadoOrdenes.Columns["Fecha"].ReadOnly = true;
                DgvListadoOrdenes.Columns["NroOrden"].ReadOnly = true;

                DgvListadoOrdenes.Columns["Nombre"].ReadOnly = true;
                DgvListadoOrdenes.Columns["Cedula"].ReadOnly = true;
                DgvListadoOrdenes.Columns["Telefono"].ReadOnly = true;
                DgvListadoOrdenes.Columns["BancoReceptor"].ReadOnly = true;
                DgvListadoOrdenes.Columns["MontoVueltoBs"].ReadOnly = false;
                DgvListadoOrdenes.Columns["Referencia"].ReadOnly = false;
                DgvListadoOrdenes.Columns["Estado"].ReadOnly = false;
                DgvListadoOrdenes.Columns["Btn"].ReadOnly = true;
                DgvListadoOrdenes.Columns["Btn2"].ReadOnly = true;

                DgvListadoOrdenes.Columns["Nacionalidad"].Visible = false;
                //DgvListadoOrdenes.Columns["NroOrden"].Visible = false;
                //DgvListadoOrdenes.Columns["Cedula"].Visible = false;
                // DgvListadoOrdenes.Columns["Telefono"].Visible = false;
                DgvListadoOrdenes.Columns["CodBancoReceptor"].Visible = false;
                //DgvListadoOrdenes.Columns["BancoReceptor"].Visible = false;
                DgvListadoOrdenes.Columns["MontoVueltoRef"].Visible = false;
                DgvListadoOrdenes.Columns["MontoRecibidoRef"].Visible = false;
                DgvListadoOrdenes.Columns["CodBancoReceptorEpos"].Visible = false;
                DgvListadoOrdenes.Columns["NroFactura"].Visible = false;
                DgvListadoOrdenes.Columns["IdAbono"].Visible = false;
                DgvListadoOrdenes.Columns["Correlativo"].Visible = false;


                //ordenar las colunmnas del grid 
                DgvListadoOrdenes.Columns["Fecha"].DisplayIndex = 0;

                DgvListadoOrdenes.Columns["NroOrden"].DisplayIndex = 1;
                DgvListadoOrdenes.Columns["Nombre"].DisplayIndex = 2;
                DgvListadoOrdenes.Columns["Cedula"].DisplayIndex = 3;
                DgvListadoOrdenes.Columns["Telefono"].DisplayIndex = 4;
                DgvListadoOrdenes.Columns["BancoReceptor"].DisplayIndex = 5;
                DgvListadoOrdenes.Columns["MontoVueltoBs"].DisplayIndex = 6;
                DgvListadoOrdenes.Columns["Referencia"].DisplayIndex = 7;

                DgvListadoOrdenes.Columns["Estado"].DisplayIndex = 8;
                DgvListadoOrdenes.Columns["Btn"].DisplayIndex = 9;
                DgvListadoOrdenes.Columns["Btn2"].DisplayIndex = 10;

                // nuevo 21-08-2023 
                DgvListadoOrdenes.Columns["MontoVueltoBs"].DefaultCellStyle.Format = "##,##0.00";

                // Configura el DataGridView para ajustar la altura de las filas automáticamente
                DgvListadoOrdenes.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                // Configura el modo de ajuste de tamaño de las celdas para que ajuste el contenido
                DgvListadoOrdenes.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            }


            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }
        }



        private void LimpiarGrid()
        {
            try
            {
                //limpiar el grid 
                lblMensaje.Text = "";
                DgvListadoOrdenes.DataSource = "";
                DgvListadoOrdenes.DataMember = "";

                //var dataGridViewColumn = DgvListadoOrdenes.Columns["CbnOpciones"];
                var dataGridViewColumn2 = DgvListadoOrdenes.Columns["Btn"];
                var dataGridViewColumn3 = DgvListadoOrdenes.Columns["Btn2"];
                var dataGridViewColumn4 = DgvListadoOrdenes.Columns["Btn3"];
                var dataGridViewColumn5 = DgvListadoOrdenes.Columns["Btn4"];
                var dataGridViewColumn6 = DgvListadoOrdenes.Columns["Btn5"];

                //if (dataGridViewColumn != null && dataGridViewColumn.Visible)

                //{

                //    DgvListadoOrdenes.Columns.RemoveAt(DgvListadoOrdenes.Columns.Count - 1);
                //}

                if (dataGridViewColumn2 != null && dataGridViewColumn2.Visible)

                {

                    DgvListadoOrdenes.Columns.RemoveAt(DgvListadoOrdenes.Columns.Count - 1);
                }

                if (dataGridViewColumn3 != null && dataGridViewColumn3.Visible)

                {

                    DgvListadoOrdenes.Columns.RemoveAt(DgvListadoOrdenes.Columns.Count - 1);
                }

                if (dataGridViewColumn4 != null && dataGridViewColumn4.Visible)

                {

                    DgvListadoOrdenes.Columns.RemoveAt(DgvListadoOrdenes.Columns.Count - 1);
                }

                if (dataGridViewColumn5 != null && dataGridViewColumn5.Visible)

                {

                    DgvListadoOrdenes.Columns.RemoveAt(DgvListadoOrdenes.Columns.Count - 1);
                }


                if (dataGridViewColumn6 != null && dataGridViewColumn6.Visible)

                {

                    DgvListadoOrdenes.Columns.RemoveAt(DgvListadoOrdenes.Columns.Count - 1);
                }


            }

            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }

        }

        private void DgvListadoOrdenes_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex >= 0 && this.DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Btn" && e.RowIndex >= 0)
            {
                // Verificar si el estado es "PENDIENTE"
                //string estado = this.DgvListadoOrdenes.Rows[e.RowIndex].Cells["Estado"].Value.ToString();
                //if (estado == "REALIZADO")
                //{
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                e.CellStyle.BackColor = Color.Red;
                DataGridViewButtonCell celBoton = this.DgvListadoOrdenes.Rows[e.RowIndex].Cells["Btn"] as DataGridViewButtonCell;
                Icon IconAtomico = new Icon(Environment.CurrentDirectory + @"\\dolar.ico");
                e.Graphics.DrawIcon(IconAtomico, e.CellBounds.Left + 1, e.CellBounds.Top + 0);

                this.DgvListadoOrdenes.Rows[e.RowIndex].Height = IconAtomico.Height + 0;
                this.DgvListadoOrdenes.Columns[e.ColumnIndex].Width = IconAtomico.Width + 2;

                e.Handled = true;
                //}
            }

            if (e.ColumnIndex >= 0 && this.DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Btn2" && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                e.CellStyle.BackColor = Color.Red;
                DataGridViewButtonCell celBoton = this.DgvListadoOrdenes.Rows[e.RowIndex].Cells["Btn2"] as DataGridViewButtonCell;
                Icon IconAtomico = new Icon(Environment.CurrentDirectory + @"\\mail.ico");
                e.Graphics.DrawIcon(IconAtomico, e.CellBounds.Left + 1, e.CellBounds.Top + 0);

                this.DgvListadoOrdenes.Rows[e.RowIndex].Height = IconAtomico.Height + 0;
                this.DgvListadoOrdenes.Columns[e.ColumnIndex].Width = IconAtomico.Width + 2;

                e.Handled = true;
            }

            if (e.ColumnIndex >= 0)
            {
                ColorearStatus();
            }
        }


        public void DgvListadoOrdenes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        
        {
            try
            {
                lblMensaje.Text = "";
                Conexion cn = new Conexion();
                SqlConnection connection = cn.LeerCadena();
                SqlCommand command = connection.CreateCommand();

                if (DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Btn") //  PARA FACTURAR
                {
                    lblMensaje.Text = "";
                    //Btnlupa.Visible = false;
                    //DgvListadoOrdenes.Visible = false;
                    string MaxDiaPagoMovil = _D_DetalleOrden.TB_PARAMETRO("PM");
                    Convert.ToDouble(DgvListadoOrdenes.CurrentRow.Cells["MontoVueltoBs"].Value.ToString());
                    if ((DgvListadoOrdenes.CurrentRow.Cells["Estado"].Value.ToString() == "PENDIENTE" || DgvListadoOrdenes.CurrentRow.Cells["Estado"].Value.ToString() == "FALLIDO") & Convert.ToDouble(MaxDiaPagoMovil) > Convert.ToDouble(DgvListadoOrdenes.CurrentRow.Cells["MontoVueltoBs"].Value.ToString()))
                    {
                        _FrmMensajes.co = 3;
                        _FrmMensajes.avisomensaje("¿Está seguro de procesar el pago móvil de la orden " + DgvListadoOrdenes.CurrentRow.Cells["NroOrden"].Value.ToString() +"?");
                        _FrmMensajes.ShowDialog();

                        //Preguta
                        if (_FrmMensajes.DialogResult == DialogResult.OK)
                        {
                            _FrmMensajes.BtnSi.Enabled = false;
                            DataTable Dt_PagoMovil = new DataTable();
                            string codSucursal;
                            string sucursal;
                            string nac;
                            string cedula;
                            string telefono;
                            string monto;
                            string montoRef;
                            string montoRecibidoRef;
                            string bancoEmisor = "0114";
                            string bancoReceptor;
                            string bancoReceptorEpos;
                            string concepto;
                            string referencia;
                            string nroOrden;
                            string nroFactura;
                            string revision;
                            string idAbono;
                            string correlativo;
                            string cedula1;

                            codSucursal = _D_DetalleOrden.TB_PARAMETRO("SucursalId");
                            //sucursal = cbSucursales.Text; ;
                            nac = DgvListadoOrdenes.CurrentRow.Cells["Nacionalidad"].Value.ToString();
                            cedula = nac + DgvListadoOrdenes.CurrentRow.Cells["Cedula"].Value.ToString();
                            cedula1 = DgvListadoOrdenes.CurrentRow.Cells["Cedula"].Value.ToString();
                            telefono = DgvListadoOrdenes.CurrentRow.Cells["Telefono"].Value.ToString().Replace("-", "");
                            monto = DgvListadoOrdenes.CurrentRow.Cells["MontoVueltoBs"].Value.ToString().Replace(",", ".");
                            montoRef = DgvListadoOrdenes.CurrentRow.Cells["MontoVueltoRef"].Value.ToString();

                            montoRecibidoRef = DgvListadoOrdenes.CurrentRow.Cells["montoRecibidoRef"].Value.ToString();
                            bancoEmisor = "0114";
                            bancoReceptor = DgvListadoOrdenes.CurrentRow.Cells["CodBancoReceptor"].Value.ToString();
                            nroOrden = DgvListadoOrdenes.CurrentRow.Cells["NroOrden"].Value.ToString();
                            bancoReceptorEpos = DgvListadoOrdenes.CurrentRow.Cells["CodBancoReceptorEpos"].Value.ToString();
                            revision = "0";
                            idAbono = DgvListadoOrdenes.CurrentRow.Cells["IdAbono"].Value.ToString();
                            nroFactura = DgvListadoOrdenes.CurrentRow.Cells["NroFactura"].Value.ToString();
                            concepto = "";
                            correlativo = DgvListadoOrdenes.CurrentRow.Cells["Correlativo"].Value.ToString();


                            //macAddress = _D_Inicio.ObtenerMacAddress();
                            // string responseToken = Envio.getToken();

                            string responseToken = Envio.getToken();

                            if ((responseToken != "Permisos no válidos"))
                            {
                                ResponseToken oModelToken = JsonConvert.DeserializeObject<ResponseToken>(responseToken);
                                string token = oModelToken.access_token;

                                string responseSendPaymentB2P = Envio.sendPaymentB2P(token, cedula, telefono, monto, "", bancoEmisor, bancoReceptor, codSucursal, codSucursal, concepto);
                                // responseSendPaymentB2P = "";
                                ResponseSendPaymentB2P oModelsendPaymentB2P = JsonConvert.DeserializeObject<ResponseSendPaymentB2P>(responseSendPaymentB2P);


                                if (oModelsendPaymentB2P.success == "true")
                                {
                                    if (oModelsendPaymentB2P.codigoConfirmacion != "0" & oModelsendPaymentB2P.codigoConfirmacion != "-1" & oModelsendPaymentB2P.codigoError == "0")
                                    {
                                        string rep = _D_DetalleOrden.ActualizarPagoMovil(codSucursal, nroOrden, revision, idAbono, oModelsendPaymentB2P.codigoConfirmacion, "Transacción Exitosa", oModelsendPaymentB2P.codigoError);


                                        lblMensaje.ForeColor = Color.DarkGreen;
                                        lblMensaje.Text = "Transacción Exitosa: " + oModelsendPaymentB2P.codigoConfirmacion;
                                        _L_Facturacion.CrearTablaPagoMovil(Dt_PagoMovil);
                                        _L_Facturacion.GuardarPagoMovilTabla(1, Dt_PagoMovil, nac, cedula1, telefono.Substring(0, 4), telefono.Substring(4, 7), monto.Replace(".", ","), bancoReceptorEpos, montoRecibidoRef, montoRef, "");

                                        //string Correlativo = _D_DetalleOrden.ID_PagoMovil(nroOrden, codSucursal, command);

                                        string rept = _FrmFacturacion.ImprimirCambio(correlativo, Dt_PagoMovil, codSucursal, nroOrden, revision, nroFactura, command);

                                        btnlupa_Click_1(this, EventArgs.Empty);
                                    }
                                    else
                                    {
                                        string rep = _D_DetalleOrden.ActualizarPagoMovil(codSucursal, nroOrden, revision, idAbono, oModelsendPaymentB2P.codigoConfirmacion, oModelsendPaymentB2P.descripcionError, oModelsendPaymentB2P.codigoError);
                                        btnlupa_Click_1(this, EventArgs.Empty);
                                        lblMensaje.ForeColor = Color.Red;
                                        lblMensaje.Text = "Transacción Fallida: Intente de nuevo";
                                        //lblMensaje.Text = "Transacción Fallida: " + oModelsendPaymentB2P.descripcionError;
                                    }
                                    //contador = 0;
                                }
                                else
                                {
                                    string rep = _D_DetalleOrden.ActualizarPagoMovil(codSucursal, nroOrden, revision, idAbono, oModelsendPaymentB2P.codigoConfirmacion, oModelsendPaymentB2P.descripcionError, oModelsendPaymentB2P.codigoError);
                                    btnlupa_Click_1(this, EventArgs.Empty);
                                    //progressBar1.Value = 100;
                                    lblMensaje.ForeColor = Color.Red;
                                    lblMensaje.Text = "Transacción Fallida: Intente de nuevo";
                                    //lblMensaje.Text = "Transacción Fallida: " + oModelsendPaymentB2P.descripcionError;

                                    //contador = 0;
                                }
                            }
                            else
                            {
                                lblMensaje.ForeColor = Color.Red;
                                lblMensaje.Text = "No tiene permisos para realizar esta acción";
                                lblMensaje.Visible = true;
                            }
                        }
                        else
                        {
                        }

                        




                    }
                    else
                    {
                        lblMensaje.ForeColor = Color.Red;
                        lblMensaje.Text = "No se puede realizar un pago móvil en estado: " + DgvListadoOrdenes.CurrentRow.Cells["Estado"].Value.ToString();
                        lblMensaje.Visible = true;
                    }
                }
                if (DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Btn2") // PARA ANULAR
                {
                    // Para anular una orden abonada 
                    if (DgvListadoOrdenes.CurrentRow.Cells["Estado"].Value.ToString() == "BLOQUEADO")
                    {
                        // EnviarMail


                    }
                    else
                    {
                        lblMensaje.ForeColor = Color.Red;
                        lblMensaje.Text = "No se puede realizar";
                    }
                }



            }

            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }
        }

        private void DgvListadoOrdenes_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                DataGridViewCell cell = this.DgvListadoOrdenes.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Detalle de orden
                if (this.DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Btn")
                {
                    // comprobar si la celda tiene contenido válido
                    if (e.Value != System.DBNull.Value)
                    {
                        cell.ToolTipText = "Pago Móvil";
                    }
                }


                // Anular orden
                if (this.DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Btn2")
                {
                    // comprobar si la celda tiene contenido válido
                    if (e.Value != System.DBNull.Value)
                    {
                        cell.ToolTipText = "Enviar email";
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



        public void btnlupa_Click_1(object sender, EventArgs e)
        {
            LimpiarGrid();



            DataSet Dts = _ListaOrdenes.TraerOrdenesConPagoMovil(DtpDesde, DtpHasta);
            if (Dts != null)
            {
                DgvListadoOrdenes.DataSource = Dts.Tables[0];
                //Paginado(Dts);
                //Paginado_Habilitar(true);
            }
            else
            {
                //Paginado_Habilitar(false);
            }

            if (DgvListadoOrdenes.Rows.Count > 0)
            {
                DgvListadoOrdenes.Visible = true;
                CrearObjetos();
                ColorearStatus();
                DesencriptarDatos();
            }
            else
            {
                DgvListadoOrdenes.Visible = false;
                //LblOpciones.Visible = false;

            }


        }

        public void EnviarMail()
        {
            //string dolarFn = Global.dolarOriginal.Replace(" ", "");
            string parte1 = "<font face='arial'>Buen día, <br/><br/>Secuencia válida hasta el día : </font>";
            string parte2 = "<font face='arial black'>";
            string parte3 = "</font><br/><br/><br/>";
            string parte4 = "<font face='arial'><blockquote> Tasa del día: </blockquote><br/><br/><blockquote><blockquote> 1 US   </font>";
            string parte5 = "<font face='arial black'>   $ ";
            //String parte6 = " (" + dolarFn + ")";
            string parte7 = "</font></blockquote></blockquote><br/>";
            string parte8 = "<blockquote><blockquote><font face='arial'>1 Euro</font>";
            String parte9 = "<font face='arial black'> € ";
            String parteX = "</font><br/><br/></blockquote></blockquote>";
            String parteXI = "<br/><font face='Calibri Light'>Generado automáticamente Epos 2.0</font>";
            //String RemitenteCaroni = ConfigurationManager.AppSettings.Get("RemitenteCaroni");
            //String RemitenteSaga = ConfigurationManager.AppSettings.Get("RemitenteSaga");

            string urlApp = _D_DetalleOrden.TB_PARAMETRO("RutaAppEnvMail");
            var ruta = ";";
            var Orig = _D_DetalleOrden.TB_PARAMETRO("UserEnvioEmail");
            var DestinatariosP = _D_DetalleOrden.TB_PARAMETRO("UserEnvioEmail");
            var DestinatariosC = _D_DetalleOrden.TB_PARAMETRO("UserEnvioEmail");
            var Asunto = "Pago Movil";
            var User = _D_DetalleOrden.TB_PARAMETRO("UserEnvioEmail");
            var pass = _D_DetalleOrden.TB_PARAMETRO("PassEnvioEmail");
            string CuerpoMensaje = parte1 + parte2 + parte3 + parte4 + parte5 + parte7 + parte8 + parte9 + parteX + parteXI;
            string adj = "";
            var HostEnvioMail = _D_DetalleOrden.TB_PARAMETRO("HostEnvioMail");
            string strComand = ruta + Orig + ";" + DestinatariosP + ";" + DestinatariosC + ";" + Asunto + ";" + User + ";" + pass + ";" + CuerpoMensaje + ";" + adj + ";" + HostEnvioMail + ";1";

            //Ejecutar app externa con parametros
            Process.Start(urlApp, strComand);
        }

        private void DesencriptarDatos()
        {
            try
            {


                // Reemplazar valores en la columna "Estado" según condiciones
                for (int i = 0; i < DgvListadoOrdenes.Rows.Count; i++)
                {
                    // Obtener el valor actual de la columna "Estado"
                    //string estadoActual = DgvListadoOrdenes.Rows[i].Cells["Estado"].Value.ToString();

                    // Aplicar las condiciones para reemplazar el valor

                    //DgvListadoOrdenes.Rows[i].Cells["MontoVueltoBs"].Value = _D_Inicio.DesEncriptar(DgvListadoOrdenes.Rows[i].Cells["MontoVueltoBs"].Value.ToString());
                    DgvListadoOrdenes.Rows[i].Cells["Referencia"].Value = _D_Inicio.DesEncriptar(DgvListadoOrdenes.Rows[i].Cells["Referencia"].Value.ToString());
                    DgvListadoOrdenes.Rows[i].Cells["Cedula"].Value = _D_Inicio.DesEncriptar(DgvListadoOrdenes.Rows[i].Cells["Cedula"].Value.ToString());
                    DgvListadoOrdenes.Rows[i].Cells["Telefono"].Value = _D_Inicio.DesEncriptar(DgvListadoOrdenes.Rows[i].Cells["Telefono"].Value.ToString());
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }
        }
    }
}
