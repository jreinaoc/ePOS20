using CapaDatos.DetalleOrden_Datos;
using CapaDatos.Inicio_Datos;
using CapaEntidades;
using CapaLogica.Anulacion_Logica;
using CapaLogica.Impresora_Fiscal;
using CapaLogica.ListaOrden_Logica;
using CapaLogica.DetalleOrden_Logica;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaDatos.Anulacion;

namespace CapaVisual_Login
{
    public partial class FrmListaOrdenes : Form

    {

        //KEDWIN
        public static FrmListaOrdenes kedInstancia { get; private set; }

        string orden = "";
        string cedula = "";
        public FrmListaOrdenes()

        {
            InitializeComponent();

            //KEDWIN
            kedInstancia = this;
        }

        //Instanciamos nuestra clase D_Loguin para poder utilizar sus miembros
        private FrmCargarOrden _FrmCargarOrden = new FrmCargarOrden();
        private L_ListaOrdenes _ListaOrdenes = new L_ListaOrdenes();
        FrmInicio _FrmInicio = new FrmInicio();
        private FrmMensajes _FrmMensajes = new FrmMensajes();
        FrmFacturacion _FrmFacturacion = new FrmFacturacion();
        FrmClaveAutorizada _FrmClaveAutorizada = new FrmClaveAutorizada();
        FrmClaveGerente _FrmClaveGerente = new FrmClaveGerente();
        private D_Inicio _D_Inicio = new D_Inicio();
        private D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        private Impresora_Fiscal _Impresora_Fiscal = new Impresora_Fiscal();
        public DataTable dt = new DataTable("CAORDSER");
        L_Anulacion _LAnulacion = new L_Anulacion();
        FrmAnulacion _FrmAnulacion = new FrmAnulacion();
        L_Facturacion _L_Facturacion = new L_Facturacion();
        FrmMostrarReporte _FrmMostrarReporte = new FrmMostrarReporte();
        D_Anulacion _D_Anulacion = new D_Anulacion();
        FrmRepProSinPag _FrmRepProSinPag = new FrmRepProSinPag();
        //FrmRepOrden _FrmRepOrden = new FrmRepOrden();
        //FrmRepOrdenTContact _FrmRepOrdenTContact = new FrmRepOrdenTContact();
        public string NombreNCManual = "";
        int PaginaInico = 1, Indice = 0, NUmeroFilas = 12, PaginaFinal;
        public bool Formulario_CargaOrden = false;

        private void FrmListaOrdenes_Load(object sender, EventArgs e)
        {
            //EstructuraGrid();

            _ListaOrdenes.LLenarCombobox(CbxUltimosTesD, CbxEstatus, DtpDesde);
            PnlComprobanteRetencion.Visible = false;
            PnlComprobanteRetencion.Enabled = false;
            PnlLSecundario.Visible = false;
            PnlLSecundario.Enabled = false;
            BtnCancelar.Visible = false;
            //DtpDesde.CustomFormat = "yyyy/MM/dd";
            DtpDesde.Visible = false;
            DtpHasta.Visible = false;
            //DtpHasta.CustomFormat = "yyyy/MM/dd";
            Lbldesde.Visible = false;
            LblHasta.Visible = false;
            //EstructuraGrid();
            TxtNumeroNC.Text = _D_DetalleOrden.ParametroSerieManual();
            string NumOrden = RecNumOrden();
            string NumCedula = RecNumCedula();


            CbxUltimosTesD.SelectedIndex = 1;
            DataSet Dts = _ListaOrdenes.TraerOrdenes(CbxUltimosTesD, CbxEstatus, NumOrden, NumCedula);
            if (Dts != null)
            {
                DgvListadoOrdenes.DataSource = Dts.Tables[0];
                Paginado(Dts);
                Paginado_Habilitar(true);
            }
            else
            {
                Paginado_Habilitar(false);
            }


            if (DgvListadoOrdenes.Rows.Count > 0)
            {
                DgvListadoOrdenes.Visible = true;
                CrearObjetos();
                ColorearStatus();

            }
            else
            {
                DgvListadoOrdenes.Visible = false;
                LblOpciones.Visible = false;
            }


            txtNumeroOrden.Text = "N° de orden";
            txtNumeroOrden.ForeColor = Color.LightGray;

            // agregado el 01/06/2023
            TxtCedula.Text = "N° de cédula";
            TxtCedula.ForeColor = Color.LightGray;


            CbxEstatus.Text = "Estatus";
            CbxEstatus.ForeColor = Color.LightGray;


        }

        public void Cerrarformulario()
        {
            PnlLSecundario.Visible = false;
            PnlLSecundario.Enabled = false;
            PnlLSecundario.Controls.Clear();


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
                DgvListadoOrdenes.Columns["NumOrdserv"].HeaderText = "N° de orden";
                DgvListadoOrdenes.Columns["Nombre"].HeaderText = "Nombre";
                DgvListadoOrdenes.Columns["Estatus"].HeaderText = "Estatus";
                DgvListadoOrdenes.Columns["Monto"].HeaderText = "Monto";
                DgvListadoOrdenes.Columns["Ref"].HeaderText = "Ref";
                DgvListadoOrdenes.Columns["Cod_Sucursal"].HeaderText = "Sucursal";
                DgvListadoOrdenes.Columns["Numero"].HeaderText = "Numero";

                DgvListadoOrdenes.Columns["Nacionalidad"].HeaderText = "Nacionalidad";
                DgvListadoOrdenes.Columns["Cedula"].HeaderText = "Cedula";
                DgvListadoOrdenes.Columns["PAGOS_IVA"].HeaderText = "PAGOS_IVA";
                DgvListadoOrdenes.Columns["PAGOS_ISLR"].HeaderText = "PAGOS_ISLR";
                DgvListadoOrdenes.Columns["Comprobante_IVA"].HeaderText = "Comprobante_IVA";
                DgvListadoOrdenes.Columns["Comprobante_ISLR"].HeaderText = "Comprobante_ISLR";
                //Ancho de columna
                DgvListadoOrdenes.Columns["Fecha"].Width = 110;
                DgvListadoOrdenes.Columns["NumOrdserv"].Width = 110;
                DgvListadoOrdenes.Columns["Nombre"].Width = 150;
                DgvListadoOrdenes.Columns["Estatus"].Width = 150;
                DgvListadoOrdenes.Columns["Monto"].Width = 100;
                DgvListadoOrdenes.Columns["Ref"].Width = 100;
                //DgvListadoOrdenes.Columns["CbnOpciones"].Width = 150;
                DgvListadoOrdenes.Columns["Btn"].Width = 80;
                DgvListadoOrdenes.Columns["Btn2"].Width = 80;
                DgvListadoOrdenes.Columns["Btn3"].Width = 80;
                DgvListadoOrdenes.Columns["Btn5"].Width = 80;
                DgvListadoOrdenes.Columns["Btn6"].Width = 40;
                DgvListadoOrdenes.Columns["Cod_Sucursal"].Width = 80;

                //Bloquear Columna 
                DgvListadoOrdenes.Columns["Fecha"].ReadOnly = true;
                DgvListadoOrdenes.Columns["NumOrdserv"].ReadOnly = true;
                DgvListadoOrdenes.Columns["Nombre"].ReadOnly = true;
                DgvListadoOrdenes.Columns["Estatus"].ReadOnly = true;
                DgvListadoOrdenes.Columns["Monto"].ReadOnly = true;
                DgvListadoOrdenes.Columns["Ref"].ReadOnly = true;
                //DgvListadoOrdenes.Columns["CbnOpciones"].ReadOnly = false;
                DgvListadoOrdenes.Columns["Btn"].ReadOnly = false;
                DgvListadoOrdenes.Columns["Btn2"].ReadOnly = false;
                DgvListadoOrdenes.Columns["Btn3"].ReadOnly = false;
                DgvListadoOrdenes.Columns["Cod_Sucursal"].ReadOnly = false;
                DgvListadoOrdenes.Columns["Nacionalidad"].ReadOnly = false;
                DgvListadoOrdenes.Columns["Cedula"].ReadOnly = false;
                DgvListadoOrdenes.Columns["PAGOS_IVA"].ReadOnly = false;
                DgvListadoOrdenes.Columns["PAGOS_ISLR"].ReadOnly = false;
                DgvListadoOrdenes.Columns["Comprobante_IVA"].ReadOnly = false;
                DgvListadoOrdenes.Columns["Comprobante_ISLR"].ReadOnly = false;

                //ordenar las colunmnas del grid 
                DgvListadoOrdenes.Columns["Fecha"].DisplayIndex = 0;
                DgvListadoOrdenes.Columns["NumOrdserv"].DisplayIndex = 1;
                DgvListadoOrdenes.Columns["Nombre"].DisplayIndex = 2;
                DgvListadoOrdenes.Columns["Estatus"].DisplayIndex = 3;
                DgvListadoOrdenes.Columns["Monto"].DisplayIndex = 4;
                DgvListadoOrdenes.Columns["Ref"].DisplayIndex = 5;
                DgvListadoOrdenes.Columns["Cod_Sucursal"].DisplayIndex = 7;
                DgvListadoOrdenes.Columns["Btn3"].DisplayIndex = 10;
                DgvListadoOrdenes.Columns["Btn4"].DisplayIndex = 11;
                DgvListadoOrdenes.Columns["Btn2"].DisplayIndex = 12;
                DgvListadoOrdenes.Columns["Btn"].DisplayIndex = 13;
                DgvListadoOrdenes.Columns["Btn5"].DisplayIndex = 14;

                DgvListadoOrdenes.Columns["Nacionalidad"].DisplayIndex = 6;
                DgvListadoOrdenes.Columns["Cedula"].DisplayIndex = 7;
                DgvListadoOrdenes.Columns["PAGOS_IVA"].DisplayIndex = 8;
                DgvListadoOrdenes.Columns["PAGOS_ISLR"].DisplayIndex = 9;

                DgvListadoOrdenes.Columns["Cod_Sucursal"].Visible = false;

                DgvListadoOrdenes.Columns["Nacionalidad"].Visible = false;
                DgvListadoOrdenes.Columns["Cedula"].Visible = false;
                DgvListadoOrdenes.Columns["PAGOS_IVA"].Visible = false;
                DgvListadoOrdenes.Columns["PAGOS_ISLR"].Visible = false;
                DgvListadoOrdenes.Columns["Comprobante_IVA"].Visible = false;
                DgvListadoOrdenes.Columns["Comprobante_ISLR"].Visible = false;
                DgvListadoOrdenes.Columns["Comprobante_IVA_Numero"].Visible = false;
                DgvListadoOrdenes.Columns["Comprobante_ISLR_Numero"].Visible = false;
                DgvListadoOrdenes.Columns["Revision"].Visible = false;
                DgvListadoOrdenes.Columns["Numero"].Visible = false;

                // nuevo 21-08-2023 
                DgvListadoOrdenes.Columns["Monto"].DefaultCellStyle.Format = "##,##0.00";
                DgvListadoOrdenes.Columns["Ref"].DefaultCellStyle.Format = "##,##0.00";

            }


            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }
        }


        public void ColorForm(System.Drawing.Color col1, System.Drawing.Color col3)
        { //Con esta funcion coloreamos el grid del fondo blanco 
            this.BackColor = col1;
            DgvListadoOrdenes.DefaultCellStyle.BackColor = col1;
            DgvListadoOrdenes.ColumnHeadersDefaultCellStyle.BackColor = col3;
            DgvListadoOrdenes.ColumnHeadersDefaultCellStyle.ForeColor = Color.Gray;
            DgvListadoOrdenes.DefaultCellStyle.ForeColor = Color.Black;
            //EstructuraGrid();

        }

        public void FormatoDataGrid1(System.Drawing.Color col1, System.Drawing.Color col3)
        {

            this.BackColor = col1;
            LblListadoOrdenes.ForeColor = Color.Black;
            Lbldesde.ForeColor = Color.Black;
            LblHasta.ForeColor = Color.Black;
            LblOpciones.BackColor = col3;
            LblOpciones.ForeColor = Color.FromArgb(30, 30, 30, 30);


            //Con esta funcion coloreamos el grid del fondo blanco 
            DgvListadoOrdenes.BackgroundColor = col1;
            DgvListadoOrdenes.DefaultCellStyle.BackColor = col1;
            DgvListadoOrdenes.ColumnHeadersDefaultCellStyle.BackColor = col3;
            DgvListadoOrdenes.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(30, 30, 30, 30);
            DgvListadoOrdenes.DefaultCellStyle.ForeColor = Color.Black;
            _FrmFacturacion.FormatoFacturacion1(col1, col3);
            //EstructuraGrid();

        }

        public void FormatoDataGrid2(System.Drawing.Color col2, System.Drawing.Color col3, System.Drawing.Color col4)
        {

            this.BackColor = col2;
            LblListadoOrdenes.ForeColor = Color.White;
            Lbldesde.ForeColor = Color.White;
            LblHasta.ForeColor = Color.White;
            LblOpciones.BackColor = col4;
            LblOpciones.ForeColor = Color.White;

            //Con esta funcion coloreamos el grid del color oscuro 
            DgvListadoOrdenes.BackgroundColor = col3;
            DgvListadoOrdenes.DefaultCellStyle.BackColor = col3;
            DgvListadoOrdenes.ColumnHeadersDefaultCellStyle.BackColor = col4;
            DgvListadoOrdenes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DgvListadoOrdenes.DefaultCellStyle.ForeColor = Color.White;
            //_FrmFacturacion.BackColor = Color.Red;
            _FrmFacturacion.FormatoFacturacion2(col2, col3, col4);

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

                LblOpciones.Visible = true;

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


                DataGridViewButtonColumn columnBtn3 = new DataGridViewButtonColumn();
                columnBtn3.Name = "Btn3";
                columnBtn3.Width = 50;
                columnBtn3.HeaderText = "";
                DgvListadoOrdenes.Columns.Add(columnBtn3);


                DataGridViewButtonColumn columnBtn4 = new DataGridViewButtonColumn();
                columnBtn4.Name = "Btn4";
                columnBtn4.Width = 50;
                columnBtn4.HeaderText = "";
                DgvListadoOrdenes.Columns.Add(columnBtn4);

                DataGridViewButtonColumn columnBtn5 = new DataGridViewButtonColumn();
                columnBtn5.Name = "Btn5";
                columnBtn5.Width = 50;
                columnBtn5.HeaderText = "";
                DgvListadoOrdenes.Columns.Add(columnBtn5);

                DataGridViewButtonColumn columnBtn6 = new DataGridViewButtonColumn();
                columnBtn6.Name = "Btn6";
                columnBtn6.Width = 50;
                columnBtn6.HeaderText = "";
                DgvListadoOrdenes.Columns.Add(columnBtn6);

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
            System.Drawing.Color colrevers = System.Drawing.ColorTranslator.FromHtml("#f6973a");


            try
            {


                foreach (DataGridViewRow Fila in DgvListadoOrdenes.Rows)
                {
                    //Padding newPadding = new Padding(10,10,10,10);


                    string Status = Fila.Cells["Estatus"].Value.ToString();
                    string PAGOS_IVA = Fila.Cells["PAGOS_IVA"].Value.ToString();
                    string PAGOS_ISLR = Fila.Cells["PAGOS_ISLR"].Value.ToString();
                    string Comprobante_IVA = Fila.Cells["Comprobante_IVA"].Value.ToString();
                    string Comprobante_ISLR = Fila.Cells["Comprobante_ISLR"].Value.ToString();
                    Status = Status.Trim();
                    if (Status == "Facturada")
                    {
                        if (PAGOS_IVA == "1" | PAGOS_ISLR == "1")
                        {
                            if (PAGOS_IVA == "1" && Comprobante_IVA == "0")
                            {
                                Fila.Cells["Estatus"].Style.BackColor = IVAS_ISLR;
                                //Fila.Cells["Estatus"].Style.Padding = newPadding;
                                DgvListadoOrdenes.Columns["Estatus"].DefaultCellStyle.Format = "C";
                                Fila.Cells["Estatus"].Style.ForeColor = Color.White;
                            }

                            if (PAGOS_ISLR == "1" && Comprobante_ISLR == "0")
                            {
                                Fila.Cells["Estatus"].Style.BackColor = IVAS_ISLR;
                                //Fila.Cells["Estatus"].Style.Padding = newPadding;
                                DgvListadoOrdenes.Columns["Estatus"].DefaultCellStyle.Format = "C";
                                Fila.Cells["Estatus"].Style.ForeColor = Color.White;
                            }

                            if (PAGOS_IVA == "1" & Comprobante_IVA == "1" | PAGOS_ISLR == "1" & Comprobante_ISLR == "1")
                            {
                                Fila.Cells["Estatus"].Style.BackColor = colfact;
                                //Fila.Cells["Estatus"].Style.Padding = newPadding;
                                DgvListadoOrdenes.Columns["Estatus"].DefaultCellStyle.Format = "C";
                                Fila.Cells["Estatus"].Style.ForeColor = Color.White;
                            }
                        }
                        else
                            Fila.Cells["Estatus"].Style.BackColor = colfact;
                        //Fila.Cells["Estatus"].Style.Padding = newPadding;
                        DgvListadoOrdenes.Columns["Estatus"].DefaultCellStyle.Format = "C";
                        Fila.Cells["Estatus"].Style.ForeColor = Color.White;

                    }

                    if (Status == "Anulada")
                    {
                        Fila.Cells["Estatus"].Style.BackColor = colanul;
                        //Fila.Cells["Estatus"].Style.Padding = newPadding;
                        DgvListadoOrdenes.Columns["Estatus"].DefaultCellStyle.Format = "C";
                        Fila.Cells["Estatus"].Style.ForeColor = Color.White;
                    }

                    if (Status == "Reversada")
                    {
                        Fila.Cells["Estatus"].Style.BackColor = colrevers;
                        //Fila.Cells["Estatus"].Style.Padding = newPadding;
                        DgvListadoOrdenes.Columns["Estatus"].DefaultCellStyle.Format = "C";
                        Fila.Cells["Estatus"].Style.ForeColor = Color.White;
                    }

                    if (Status == "Por pagar")
                    {
                        Fila.Cells["Estatus"].Style.BackColor = colporp;
                        //Fila.Cells["Estatus"].Style.Padding = newPadding;
                        DgvListadoOrdenes.Columns["Estatus"].DefaultCellStyle.Format = "C";
                        Fila.Cells["Estatus"].Style.ForeColor = Color.FromArgb(89, 190, 186);

                    }

                    if (Status == "Abonada")
                    {
                        Fila.Cells["Estatus"].Style.BackColor = colabon;
                        //Fila.Cells["Estatus"].Style.Padding = newPadding;
                        DgvListadoOrdenes.Columns["Estatus"].DefaultCellStyle.Format = "C";
                        Fila.Cells["Estatus"].Style.ForeColor = Color.White;

                    }


                }

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

                DgvListadoOrdenes.DataSource = "";
                DgvListadoOrdenes.DataMember = "";

                //var dataGridViewColumn = DgvListadoOrdenes.Columns["CbnOpciones"];
                var dataGridViewColumn2 = DgvListadoOrdenes.Columns["Btn"];
                var dataGridViewColumn3 = DgvListadoOrdenes.Columns["Btn2"];
                var dataGridViewColumn4 = DgvListadoOrdenes.Columns["Btn3"];
                var dataGridViewColumn5 = DgvListadoOrdenes.Columns["Btn4"];
                var dataGridViewColumn6 = DgvListadoOrdenes.Columns["Btn5"];
                var dataGridViewColumn7 = DgvListadoOrdenes.Columns["Btn6"];

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

                if (dataGridViewColumn7 != null && dataGridViewColumn7.Visible)

                {

                    DgvListadoOrdenes.Columns.RemoveAt(DgvListadoOrdenes.Columns.Count - 1);
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }

        }

        private void CbxUltimosTesD_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string NumOrden = RecNumOrden();
            string NumCedula = RecNumCedula();

            if (CbxUltimosTesD.SelectionLength > 0)
            {

                if (CbxUltimosTesD.SelectedIndex == 5)
                {
                    LimpiarGrid();
                    Paginado_Habilitar(false);
                    cbPagina_Ini.Items.Clear();
                    DtpDesde.Visible = true;
                    DtpHasta.Visible = true;
                    Lbldesde.Visible = true;
                    LblHasta.Visible = true;
                    LblOpciones.Visible = false;
                    DgvListadoOrdenes.Visible = false;
                    TxtCedula.Enabled = false;
                    txtNumeroOrden.Enabled = false;
                    CbxEstatus.Enabled = false;

                }
                else
                {
                    if (CbxEstatus.Text != "" || CbxUltimosTesD.Text != "")
                    {
                        LimpiarGrid();
                        DtpDesde.Visible = false;
                        DtpHasta.Visible = false;
                        Lbldesde.Visible = false;
                        LblHasta.Visible = false;
                        TxtCedula.Enabled = true;
                        txtNumeroOrden.Enabled = true;
                        CbxEstatus.Enabled = true;

                        DataSet Dts = _ListaOrdenes.TraerOrdenes(CbxUltimosTesD, CbxEstatus, NumOrden, NumCedula);
                        if (Dts != null)
                        {
                            DgvListadoOrdenes.DataSource = Dts.Tables[0];
                            Paginado(Dts);
                            Paginado_Habilitar(true);
                        }
                        else
                        {
                            Paginado_Habilitar(false);
                        }

                        if (DgvListadoOrdenes.Rows.Count > 0)
                        {
                            DgvListadoOrdenes.Visible = true;
                            CrearObjetos();
                            ColorearStatus();
                        }
                        else
                        {
                            DgvListadoOrdenes.Visible = false;
                            LblOpciones.Visible = false;

                        }


                    }
                }
            }

        }

        private void CbxEstatus_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string NumOrden = RecNumOrden();
            string NumCedula = RecNumCedula();
            CbxEstatus.ForeColor = Color.Black;


            if (CbxEstatus.SelectionLength > 0)
            {
                DtpDesde.Visible = false;
                DtpHasta.Visible = false;
                Lbldesde.Visible = false;
                LblHasta.Visible = false;
                LimpiarGrid();
                if (CbxEstatus.SelectedIndex != -1)
                {
                    LimpiarGrid();
                    if (CbxEstatus.Text != "" || CbxUltimosTesD.Text != "")
                    {
                        LimpiarGrid();

                        DataSet Dts = _ListaOrdenes.TraerOrdenes(CbxUltimosTesD, CbxEstatus, NumOrden, NumCedula);
                        if (Dts != null)
                        {
                            DgvListadoOrdenes.DataSource = Dts.Tables[0];
                            Paginado(Dts);
                            Paginado_Habilitar(true);
                        }
                        else
                        {
                            Paginado_Habilitar(false);
                        }

                        if (DgvListadoOrdenes.Rows.Count > 0)
                        {
                            DgvListadoOrdenes.Visible = true;
                            CrearObjetos();
                            ColorearStatus();
                        }
                        else
                        {
                            DgvListadoOrdenes.Visible = false;
                            LblOpciones.Visible = false;

                        }


                    }

                }
            }

        }

        private void DgvListadoOrdenes_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            //Para colocar un icono en el boton del grid
            if (e.ColumnIndex >= 0 && this.DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Btn" && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                e.CellStyle.BackColor = Color.Red;
                DataGridViewButtonCell celBoton = this.DgvListadoOrdenes.Rows[e.RowIndex].Cells["Btn"] as DataGridViewButtonCell;
                Icon IconAtomico = new Icon(Environment.CurrentDirectory + @"\\LibroVAS.ico");
                e.Graphics.DrawIcon(IconAtomico, e.CellBounds.Left + 1, e.CellBounds.Top + 0);


                this.DgvListadoOrdenes.Rows[e.RowIndex].Height = IconAtomico.Height + 0;
                this.DgvListadoOrdenes.Columns[e.ColumnIndex].Width = IconAtomico.Width + 2;

                e.Handled = true;
            }

            if (e.ColumnIndex >= 0 && this.DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Btn2" && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                e.CellStyle.BackColor = Color.Red;
                DataGridViewButtonCell celBoton = this.DgvListadoOrdenes.Rows[e.RowIndex].Cells["Btn2"] as DataGridViewButtonCell;
                Icon IconAtomico = new Icon(Environment.CurrentDirectory + @"\\AnularVAS.ico");
                e.Graphics.DrawIcon(IconAtomico, e.CellBounds.Left + 1, e.CellBounds.Top + 0);


                this.DgvListadoOrdenes.Rows[e.RowIndex].Height = IconAtomico.Height + 0;
                this.DgvListadoOrdenes.Columns[e.ColumnIndex].Width = IconAtomico.Width + 2;

                e.Handled = true;
            }


            if (e.ColumnIndex >= 0 && this.DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Btn3" && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                e.CellStyle.BackColor = Color.Red;
                DataGridViewButtonCell celBoton = this.DgvListadoOrdenes.Rows[e.RowIndex].Cells["Btn3"] as DataGridViewButtonCell;
                Icon IconAtomico = new Icon(Environment.CurrentDirectory + @"\\procesarsinp.ico");
                e.Graphics.DrawIcon(IconAtomico, e.CellBounds.Left + 1, e.CellBounds.Top + 0);


                this.DgvListadoOrdenes.Rows[e.RowIndex].Height = IconAtomico.Height + 0;
                this.DgvListadoOrdenes.Columns[e.ColumnIndex].Width = IconAtomico.Width + 2;

                e.Handled = true;
            }

            if (e.ColumnIndex >= 0 && this.DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Btn4" && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                e.CellStyle.BackColor = Color.Red;
                DataGridViewButtonCell celBoton = this.DgvListadoOrdenes.Rows[e.RowIndex].Cells["Btn4"] as DataGridViewButtonCell;
                Icon IconAtomico = new Icon(Environment.CurrentDirectory + @"\\reimprimirord.ico");
                e.Graphics.DrawIcon(IconAtomico, e.CellBounds.Left + 1, e.CellBounds.Top + 0);


                this.DgvListadoOrdenes.Rows[e.RowIndex].Height = IconAtomico.Height + 0;
                this.DgvListadoOrdenes.Columns[e.ColumnIndex].Width = IconAtomico.Width + 2;

                e.Handled = true;
            }

            if (e.ColumnIndex >= 0 && this.DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Btn5" && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                e.CellStyle.BackColor = Color.Red;
                DataGridViewButtonCell celBoton = this.DgvListadoOrdenes.Rows[e.RowIndex].Cells["Btn5"] as DataGridViewButtonCell;
                Icon IconAtomico;

                //Validar la fecha activa para poner un iciono o otro 

                //if (DgvListadoOrdenes.Rows[e.RowIndex].Cells["PAGOS_IVA"].Value.ToString() == "1" & DgvListadoOrdenes.Rows[e.RowIndex].Cells["Comprobante_IVA"].Value.ToString() == "0"| DgvListadoOrdenes.Rows[e.RowIndex].Cells["PAGOS_ISLR"].Value.ToString() == "1" & DgvListadoOrdenes.Rows[e.RowIndex].Cells["Comprobante_ISLR"].Value.ToString() == "0")
                if (DgvListadoOrdenes.Rows[e.RowIndex].Cells["PAGOS_IVA"].Value.ToString() == "1" | DgvListadoOrdenes.Rows[e.RowIndex].Cells["PAGOS_ISLR"].Value.ToString() == "1")
                {
                    IconAtomico = new Icon(Environment.CurrentDirectory + @"\\Boton Registro Comprobante.ico");
                }
                else
                {
                    IconAtomico = new Icon(Environment.CurrentDirectory + @"\\comprobanteInabilitado.ico");

                }

                //IconAtomico = new Icon(Environment.CurrentDirectory + @"\\Boton Registro Comprobante.ico");
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


        private void btnlupa_Click_1(object sender, EventArgs e)
        {
            LimpiarGrid();
            string NumOrden = RecNumOrden();
            string NumCedula = RecNumCedula();


            if (CbxUltimosTesD.SelectedIndex == 5)
            {

                DataSet Dts = _ListaOrdenes.TraerOrdporRango(DtpDesde, DtpHasta, CbxEstatus, NumCedula);
                if (Dts != null)
                {
                    DgvListadoOrdenes.DataSource = Dts.Tables[0];
                    Paginado(Dts);
                    Paginado_Habilitar(true);
                }
                else
                {
                    Paginado_Habilitar(false);
                }

                if (DgvListadoOrdenes.Rows.Count > 0)
                {
                    DgvListadoOrdenes.Visible = true;
                    CrearObjetos();
                    ColorearStatus();
                }
                else
                {
                    DgvListadoOrdenes.Visible = false;
                    LblOpciones.Visible = false;

                }

            }
            else
            {


                DataSet Dts = _ListaOrdenes.TraerOrdenes(CbxUltimosTesD, CbxEstatus, NumOrden, NumCedula);
                if (Dts != null)
                {
                    DgvListadoOrdenes.DataSource = Dts.Tables[0];
                    Paginado(Dts);
                    Paginado_Habilitar(true);
                }
                else
                {
                    Paginado_Habilitar(false);
                }

                if (DgvListadoOrdenes.Rows.Count > 0)
                {
                    DgvListadoOrdenes.Visible = true;
                    CrearObjetos();
                    ColorearStatus();
                }

                else
                {
                    DgvListadoOrdenes.Visible = false;
                    LblOpciones.Visible = false;

                }




            }
        }
        public string Verificar_Existencia(string Numero_orden, string Cod_DetVta, string OrSer_Statu)
        {
            string Resp = _LAnulacion.Verificar_Existencia_Inv(Numero_orden, Cod_DetVta, OrSer_Statu);
            if (Resp != "SATISFACTORIO")
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(Resp);
                _FrmMensajes.ShowDialog();
            }

            return Resp;
        }

        public void DgvListadoOrdenes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Btn") //  PARA FACTURAR
            {
                PnlLSecundario.Visible = true;
                PnlLSecundario.Enabled = true;
                LblListadoOrdenes.Visible = false;
                CbxUltimosTesD.Visible = false;
                txtNumeroOrden.Visible = false;
                TxtCedula.Visible = false; // agregado el 01/06/2023
                CbxEstatus.Visible = false;
                Btnlupa.Visible = false;
                DgvListadoOrdenes.Visible = false;
                DtpDesde.Visible = false;
                DtpHasta.Visible = false;
                Lbldesde.Visible = false;
                LblHasta.Visible = false;
                LblOpciones.Visible = false;
                PnlLSecundario.Visible = true;
                PnlLSecundario.Enabled = true;
                PnlLSecundario.Location = new Point(50, 50);
                PnlLSecundario.Size = (Size)new Point(1472, 1168);
                BtnCancelar.Location = new Point(985, 469);
                BtnCancelar.Visible = true;
                PnlLSecundario.Controls.Clear();
                _FrmFacturacion.LimpiaVariablesIdAbonoPagoMovil();
                AbrirForm(_FrmFacturacion);
                Paginado_Habilitar(false);
                // _FrmFacturacion.LimpiarGrid();
                if (DgvListadoOrdenes.CurrentRow.Cells["Estatus"].Value.ToString() == "Abonada  ")
                {
                    // Recalcular la orden
                    string rep = _D_DetalleOrden.RecalculaOSDivisa(DgvListadoOrdenes.CurrentRow.Cells["Cod_Sucursal"].Value.ToString(), DgvListadoOrdenes.CurrentRow.Cells["NumOrdserv"].Value.ToString(), "0");
                }
                else if (DgvListadoOrdenes.CurrentRow.Cells["Estatus"].Value.ToString() == "Por pagar")
                {
                    //por el mometo se comenta , esto soluciona el problema de la inconcistencia en el total de la factura 
                    //_D_DetalleOrden.RecalculaOPorpagar(DgvListadoOrdenes.CurrentRow.Cells["Cod_Sucursal"].Value.ToString(), DgvListadoOrdenes.CurrentRow.Cells["NumOrdserv"].Value.ToString(), "0");
                }

                // Cargar el formulario de facturacion
                _FrmFacturacion.CargarDatosOrden(DgvListadoOrdenes.CurrentRow.Cells["NumOrdserv"].Value.ToString(), DgvListadoOrdenes.CurrentRow.Cells["Nombre"].Value.ToString(), DgvListadoOrdenes.CurrentRow.Cells["Revision"].Value.ToString());

                // Cargar los Pagos de la orden 
                _FrmFacturacion.DgvListadoOrdenes.DataSource = _D_DetalleOrden.CargarPagosGrid(DgvListadoOrdenes.CurrentRow.Cells["Cod_Sucursal"].Value.ToString(), DgvListadoOrdenes.CurrentRow.Cells["NumOrdserv"].Value.ToString(), DgvListadoOrdenes.CurrentRow.Cells["Revision"].Value.ToString());
                if (DgvListadoOrdenes.Rows.Count > 0)
                {
                    //_FrmFacturacion.LimpiarGrid();
                    _FrmFacturacion.CrearObjetos();
                }


            }

            if (DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Btn2") // PARA ANULAR
            {
                // Para anular una orden abonada 
                if (DgvListadoOrdenes.CurrentRow.Cells["Estatus"].Value.ToString().Trim() == "Abonada  ")
                {
                    // Con la siguiente funcion se cargan los datos en la entidad tbCaordser
                    _FrmFacturacion.CargarDatosOrden(DgvListadoOrdenes.CurrentRow.Cells["NumOrdserv"].Value.ToString(), DgvListadoOrdenes.CurrentRow.Cells["Nombre"].Value.ToString(), DgvListadoOrdenes.CurrentRow.Cells["Revision"].Value.ToString());
                    if (_ListaOrdenes.PagosdelDia(TB_CAORDSER.NumOrdserv) == false)
                    {
                        // Se pregunta si esta seguro de anular
                        string mensaje = "¿ Esta seguro de devolver esta orden ? ";
                        _FrmMensajes.co = 3;
                        _FrmMensajes.avisomensaje(mensaje);
                        _FrmMensajes.ShowDialog();


                        if (_FrmMensajes.DialogResult == DialogResult.OK)
                        {
                            // Se pide la clave de autorizada
                            _FrmClaveAutorizada.ShowDialog();

                            if (_FrmClaveAutorizada.ClaveCorrecta == true)
                            {

                                // Si todo esta OK se muestra el formulario de anulacion 
                                if (_FrmClaveAutorizada.DialogResult == DialogResult.OK)
                                {
                                    _FrmAnulacion.CargarOrdenAnular(TB_CAORDSER.NumOrdserv);
                                    _FrmAnulacion.ShowDialog();
                                    RecargaPostNC();
                                }
                                if (_FrmAnulacion.DialogResult == DialogResult.OK)
                                {

                                }


                            }

                            cerrar();
                        }

                        cerrar();
                    }

                    else
                    {
                        string mensaje = "No es posible ejecutar este proceso si hay pagos en el día.";
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje(mensaje);
                        _FrmMensajes.ShowDialog();


                    }

                    {
                        return;
                    }

                }

                ////NoataCredito
                if (DgvListadoOrdenes.CurrentRow.Cells["Estatus"].Value.ToString().Trim() == "Facturada")
                {
                    _FrmFacturacion.CargarDatosOrden(DgvListadoOrdenes.CurrentRow.Cells["NumOrdserv"].Value.ToString(), DgvListadoOrdenes.CurrentRow.Cells["Nombre"].Value.ToString(), DgvListadoOrdenes.CurrentRow.Cells["Revision"].Value.ToString());
                    _D_DetalleOrden.ObtenerFactura(TB_CAORDSER.NumOrdserv);

                    if (TB_FACTURAS.Fact_Num != "" & TB_FACTURAS.Fact_Num != null)
                    {
                        _FrmMensajes.co = 3;
                        _FrmMensajes.avisomensaje("¿ Desea generar una Nota de Crédito ? ");
                        _FrmMensajes.ShowDialog();

                        //Preguta
                        if (_FrmMensajes.DialogResult == DialogResult.OK)
                        {
                            //PidoClave

                            int xDife = (int)(TB_FACTURAS.Fact_FecCrea - DateTime.Today).TotalDays;
                            String DiasGteTienda = _D_DetalleOrden.TB_PARAMETRO("DiasNCGteTienda");
                            if (DiasGteTienda != "")
                            {
                                if (xDife > Convert.ToInt32(DiasGteTienda))
                                {
                                    //Pido clave de gerente regional

                                    String DiasNCCA = _D_DetalleOrden.TB_PARAMETRO("DiasNCComAud");
                                    if (xDife > Convert.ToInt32(DiasNCCA) && Convert.ToInt32(DiasNCCA) > 0)
                                    {
                                        ////Pasar Rol Para mostrar 
                                        /*FrmClaveAutorizada.LbIdRol.Text = _D_DetalleOrden.TB_PARAMETRO("RolActivaNC");*/

                                        _FrmClaveAutorizada.ShowDialog();

                                        if (_FrmClaveAutorizada.ClaveCorrecta == true)
                                        {
                                            string Nombre = VariablesGlobales.UsuarioAutorizado_FrmClaveAutorizada;

                                            if (TB_FACTURAS.Fact_Num != "" & TB_FACTURAS.Fact_Num != null && _L_Facturacion.ValidaFactManual() == false)
                                            {
                                                if (_L_Facturacion.stringBuilder.ToString().Length > 2)
                                                {
                                                    _FrmMensajes.co = 2;
                                                    _FrmMensajes.avisomensaje(_L_Facturacion.stringBuilder.ToString());
                                                    _FrmMensajes.ShowDialog();
                                                    return;
                                                }

                                                _FrmAnulacion.GerenteAutoriza = Nombre;
                                                _FrmAnulacion.CargarOrdenAnular(TB_CAORDSER.NumOrdserv);

                                                _FrmAnulacion.ShowDialog();
                                            }

                                            else if (_L_Facturacion.ValidaFactManual() == true)
                                            {
                                                PnlNotaCreditoManual.Visible = true;
                                                NombreNCManual = Nombre;
                                                return;
                                            }

                                        }
                                    }

                                    else
                                    {
                                        _FrmClaveAutorizada.ShowDialog();

                                        if (_FrmClaveAutorizada.ClaveCorrecta == true)
                                        {
                                            string Nombre = VariablesGlobales.UsuarioAutorizado_FrmClaveAutorizada;

                                            if (TB_FACTURAS.Fact_Num != "" & TB_FACTURAS.Fact_Num != null && _L_Facturacion.ValidaFactManual() == false)
                                            {
                                                if (_L_Facturacion.stringBuilder.ToString().Length > 2)
                                                {
                                                    _FrmMensajes.co = 2;
                                                    _FrmMensajes.avisomensaje(_L_Facturacion.stringBuilder.ToString());
                                                    _FrmMensajes.ShowDialog();
                                                    return;
                                                }

                                                _FrmAnulacion.GerenteAutoriza = Nombre;
                                                _FrmAnulacion.CargarOrdenAnular(TB_CAORDSER.NumOrdserv);

                                                _FrmAnulacion.ShowDialog();
                                            }

                                            else if (_L_Facturacion.ValidaFactManual() == true)
                                            {
                                                PnlNotaCreditoManual.Visible = true;
                                                NombreNCManual = Nombre;
                                                return;
                                            }

                                        }
                                    }
                                }

                                else
                                {
                                    _FrmClaveAutorizada.ShowDialog();

                                    if (_FrmClaveAutorizada.ClaveCorrecta == true)
                                    {
                                        string Nombre = VariablesGlobales.UsuarioAutorizado_FrmClaveAutorizada;

                                        if (TB_FACTURAS.Fact_Num != "" & TB_FACTURAS.Fact_Num != null && _L_Facturacion.ValidaFactManual() == false)
                                        {
                                            if (_L_Facturacion.stringBuilder.ToString().Length > 2)
                                            {
                                                _FrmMensajes.co = 2;
                                                _FrmMensajes.avisomensaje(_L_Facturacion.stringBuilder.ToString());
                                                _FrmMensajes.ShowDialog();
                                                return;
                                            }

                                            _FrmAnulacion.GerenteAutoriza = Nombre;
                                            _FrmAnulacion.CargarOrdenAnular(TB_CAORDSER.NumOrdserv);

                                            _FrmAnulacion.ShowDialog();
                                        }

                                        else if (_L_Facturacion.ValidaFactManual() == true)
                                        {
                                            PnlNotaCreditoManual.Visible = true;
                                            NombreNCManual = Nombre;
                                            return;
                                        }

                                    }
                                }


                            }
                            else
                            {
                                _FrmMensajes.co = 3;
                                _FrmMensajes.avisomensaje("El parametro DiasNCGteTienda tiene un valor vacío. Comuníquese con el Dpto de Sistemas. Nº Inválido ");
                                _FrmMensajes.ShowDialog();

                            }

                            cerrar();
                        }
                        else
                        {
                            cerrar();
                            return;
                        }

                        return;
                    }


                }
                //fin Nota Credito


                //Para anular orden por pagar 
                if (DgvListadoOrdenes.CurrentRow.Cells["Estatus"].Value.ToString().Trim() == "Por pagar")
                {
                    _FrmFacturacion.CargarDatosOrden(DgvListadoOrdenes.CurrentRow.Cells["NumOrdserv"].Value.ToString(), DgvListadoOrdenes.CurrentRow.Cells["Nombre"].Value.ToString(), DgvListadoOrdenes.CurrentRow.Cells["Revision"].Value.ToString());

                    // Se pregunta si esta seguro de anular 
                    string mensaje = "¿ Esta seguro de devolver esta orden ? ";
                    _FrmMensajes.co = 3;
                    _FrmMensajes.avisomensaje(mensaje);
                    _FrmMensajes.ShowDialog();


                    if (_FrmMensajes.DialogResult == DialogResult.OK)
                    {
                        // Se pide la clave de gerente 
                        _FrmClaveGerente.ShowDialog();


                        if (_FrmClaveGerente.ClaveCorrecta == true)
                        {

                            if (_FrmClaveGerente.DialogResult == DialogResult.OK)
                            {
                                // Si todo esta OK se muestra el formulario de anulacion 
                                _FrmAnulacion.CargarOrdenAnular(TB_CAORDSER.NumOrdserv);

                                _FrmAnulacion.ShowDialog();

                            }
                            if (_FrmAnulacion.DialogResult == DialogResult.OK)
                            {
                                RecargaPostNC();
                            }
                        }

                    }


                    {
                        return;
                    }


                }
            }


            //Procesar sin pagos 

            if (DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Btn3")
            {
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

                _D_DetalleOrden.Datos_de_la_Orden(DgvListadoOrdenes.CurrentRow.Cells["NumOrdserv"].Value.ToString(), DgvListadoOrdenes.CurrentRow.Cells["Revision"].Value.ToString());

                if (TB_CAORDSER.OrSer_Status != "004" | TB_CAORDSER.Cod_Venta == "001") //Por pagar y /venta directa 
                {
                    return;
                }

                //Validar que Exista existencia de inventario para empezar el proceso Solo para ordener PorPagar 
                if (TB_CAORDSER.OrSer_Status == "004" && TB_CAORDSER.Cod_DetVta != "02")
                {
                    string Rep = Verificar_Existencia(TB_CAORDSER.NumOrdserv, TB_CAORDSER.Cod_DetVta, TB_CAORDSER.OrSer_Status);
                    if (Rep != "SATISFACTORIO")
                    {
                        return;
                    }
                }

                _ListaOrdenes.ValidarRequiereClave();// Valida si se requiere clave para seguir un proceso u otro 
                if (_ListaOrdenes.requiereClave == true)
                {
                    _FrmMensajes.co = 3;
                    _FrmMensajes.avisomensaje(_ListaOrdenes.stringBuilder.ToString());
                    _FrmMensajes.ShowDialog();

                    if (_FrmMensajes.DialogResult == DialogResult.OK)
                    {
                        _FrmClaveAutorizada.ShowDialog();

                        if (_FrmClaveAutorizada.DialogResult == DialogResult.OK)
                        {
                            if (_FrmClaveAutorizada.ClaveCorrecta == true)
                            {
                                //valido status de la orden 
                                bool Status = _ListaOrdenes.ValidoEstatusOrden();

                                if (Status == false)
                                {
                                    _FrmMensajes.co = 2;
                                    _FrmMensajes.avisomensaje(_ListaOrdenes.stringBuilder.ToString());
                                    _FrmMensajes.ShowDialog();
                                    return;
                                }

                                //valido si es una oren casada 
                                _ListaOrdenes.OrdenesCasadas();

                                //realizo el proceso
                                bool Proceso = _ListaOrdenes.ProcesarSinPago();
                                //Realizo el movimiento de de Lentes de contactos si la orden lo requiere 02-06-2023
                                _ListaOrdenes.MovInventarioLentesContactos();
                                if (Proceso == false)
                                {
                                    _FrmMensajes.co = 2;
                                    _FrmMensajes.avisomensaje(_ListaOrdenes.stringBuilder.ToString());
                                    _FrmMensajes.ShowDialog();
                                }

                                //Guardo en el auditor 
                                _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "011", TB_USUARIO.COD_EMPLEADO, "OS: " + TB_CAORDSER.NumOrdserv + ", Autorizado por: " + VariablesGlobales.UsuarioAutorizado_FrmClaveAutorizada);

                                //ImprimirReporte

                                ////Reporte de Procesar sin Pagos con reporte de Orden 
                                // Si es trabajo de contacto se muestra este reporte 
                                string concat = DgvListadoOrdenes.CurrentRow.Cells["Cod_Sucursal"].Value.ToString() + DgvListadoOrdenes.CurrentRow.Cells["NumOrdserv"].Value.ToString() + "0";
                                if (TB_CAORDSER.Cod_DetVta == "02") // Si se procesa uan orden de contacto se muestra reporte de contacto  
                                {
                                    _FrmRepProSinPag.setParametros(concat);
                                    _FrmRepProSinPag.ConfigRep(true, true);

                                    if (_D_DetalleOrden.ParametroImpresion() == "1") // Si el parametro de impresion es 1 entonces imprime el reporte 
                                    {
                                        _FrmRepProSinPag.imprimir();

                                    }
                                    else
                                    {
                                        _FrmRepProSinPag.ShowDialog();

                                    }

                                }
                                else // si es otro tipo de trabajo 
                                {
                                    _FrmRepProSinPag.setParametros(concat);
                                    _FrmRepProSinPag.ConfigRep(true, false);

                                    if (_D_DetalleOrden.ParametroImpresion() == "1") // Si el parametro de impresion es 1 entonces imprime el reporte 
                                    {
                                        _FrmRepProSinPag.imprimir();

                                    }
                                    else
                                    {
                                        _FrmRepProSinPag.ShowDialog();

                                    }



                                }



                            }

                        }

                    }
                }

                else // Si no requiere clave, se hace el proceso de la siguiente manera 
                {
                    //valido status de la orden 
                    bool Status = _ListaOrdenes.ValidoEstatusOrden();

                    if (Status == false)
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje(_ListaOrdenes.stringBuilder.ToString());
                        _FrmMensajes.ShowDialog();
                        return;
                    }

                    //valido si es una oren casada 
                    _ListaOrdenes.OrdenesCasadas();

                    _FrmMensajes.co = 3;
                    _FrmMensajes.avisomensaje(_ListaOrdenes.stringBuilder.ToString());
                    _FrmMensajes.ShowDialog();
                    if (_FrmMensajes.DialogResult == DialogResult.OK)
                    {
                    }

                    else
                    {
                        return;
                    }

                    //realizo el proceso
                    bool Proceso = _ListaOrdenes.ProcesarSinPago();

                    if (Proceso == false)
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje(_ListaOrdenes.stringBuilder.ToString());
                        _FrmMensajes.ShowDialog();
                    }

                    //Guardo en el auditor 
                    _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "011", TB_USUARIO.COD_EMPLEADO, "OS: " + TB_CAORDSER.NumOrdserv + ", Autorizado por: " + VariablesGlobales.UsuarioAutorizado_FrmClaveAutorizada);

                    //ImprimirReporte

                    ////Reporte de Procesar sin Pagos con reporte de Orden 
                    // Si es trabajo de contacto se muestra este reporte 
                    string concat = DgvListadoOrdenes.CurrentRow.Cells["Cod_Sucursal"].Value.ToString() + DgvListadoOrdenes.CurrentRow.Cells["NumOrdserv"].Value.ToString() + "0";
                    if (TB_CAORDSER.Cod_DetVta == "02") // Si se procesa uan orden de contacto se muestra reporte de contacto  
                    {
                        _FrmRepProSinPag.setParametros(concat);
                        _FrmRepProSinPag.ConfigRep(true, true);

                        if (_D_DetalleOrden.ParametroImpresion() == "1") // Si el parametro de impresion es 1 entonces imprime el reporte 
                        {
                            _FrmRepProSinPag.imprimir();

                        }
                        else
                        {
                            _FrmRepProSinPag.ShowDialog();

                        }

                    }
                    else // si es otro tipo de trabajo 
                    {
                        _FrmRepProSinPag.setParametros(concat);
                        _FrmRepProSinPag.ConfigRep(true, false);

                        if (_D_DetalleOrden.ParametroImpresion() == "1") // Si el parametro de impresion es 1 entonces imprime el reporte 
                        {
                            _FrmRepProSinPag.imprimir();

                        }
                        else
                        {
                            _FrmRepProSinPag.ShowDialog();

                        }



                    }


                }

                cerrar();
            }


            // Reimprimir Abono
            if (DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Btn4") // Cuando se da click en el boton azul de reimprimir, obtiene el reporte del abono 
            { //Se cargan los datos de la orden 
                string concat = DgvListadoOrdenes.CurrentRow.Cells["Cod_Sucursal"].Value.ToString() + DgvListadoOrdenes.CurrentRow.Cells["NumOrdserv"].Value.ToString() + "0";
                // Se valida si la orden es abonada para que pueda imprimir o mostrar el reporte 
                if (DgvListadoOrdenes.CurrentRow.Cells["Estatus"].Value.ToString() == "Abonada  ")
                {
                    _FrmMostrarReporte.setParametros(concat);
                    _FrmMostrarReporte.ConfigRep();
                    if (_D_DetalleOrden.ParametroImpresion() == "1") // Si el parametro de impresion es 1 entonces imprime el reporte 
                    {
                        _FrmMostrarReporte.imprimir();

                    }
                    else
                    {
                        _FrmMostrarReporte.ShowDialog();

                    }

                }

                cerrar();

            }

            //Boton numero 5 
            //Registrar Comprobante de IVA y ISLR
            if (DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Btn5") // Cuando se da click se despliega el Comprobante IVA y ISLR
            {
                //SE VALIDA QUE LA ORDEN TENGA PAGOS CON IVA Y ISLR Y QUE NO TENGA CARGADO EL COMPROBANTE 
                if (DgvListadoOrdenes.CurrentRow.Cells["PAGOS_IVA"].Value.ToString() == "1" | DgvListadoOrdenes.CurrentRow.Cells["PAGOS_ISLR"].Value.ToString() == "1")
                {
                    //SE MUESTRA EL PANEL PARA CARGAR EL COMPROBANTE 
                    PnlComprobanteRetencion.Visible = true;
                    PnlComprobanteRetencion.Enabled = true;
                    PnlComprobanteRetencion.Location = new Point(350, 200);
                    PnlComprobanteRetencion.Show();
                    PnlComprobanteRetencion.BringToFront();
                    TxtRetencionIVA.Enabled = false;
                    TxtRetencionISRL.Enabled = false;

                    TxtRetencionFactura.Text = _D_DetalleOrden.ComprobantesRegistardos_IVA_ISLR(DgvListadoOrdenes.CurrentRow.Cells["NumOrdserv"].Value.ToString());
                    if (DgvListadoOrdenes.CurrentRow.Cells["PAGOS_IVA"].Value.ToString() == "1" & DgvListadoOrdenes.CurrentRow.Cells["Comprobante_IVA"].Value.ToString() == "0")
                    {
                        TxtRetencionIVA.Enabled = true;
                    }
                    if (DgvListadoOrdenes.CurrentRow.Cells["PAGOS_ISLR"].Value.ToString() == "1" & DgvListadoOrdenes.CurrentRow.Cells["Comprobante_ISLR"].Value.ToString() == "0")
                    {
                        TxtRetencionISRL.Enabled = true;
                    }

                    if (DgvListadoOrdenes.CurrentRow.Cells["PAGOS_IVA"].Value.ToString() == "1" & DgvListadoOrdenes.CurrentRow.Cells["Comprobante_IVA"].Value.ToString() == "1")
                    {
                        TxtRetencionIVA.Enabled = false;
                        TxtRetencionIVA.Text = DgvListadoOrdenes.CurrentRow.Cells["Comprobante_IVA_Numero"].Value.ToString();

                    }

                    if (DgvListadoOrdenes.CurrentRow.Cells["PAGOS_ISLR"].Value.ToString() == "1" & DgvListadoOrdenes.CurrentRow.Cells["Comprobante_ISLR"].Value.ToString() == "1")
                    {
                        TxtRetencionISRL.Enabled = false;
                        TxtRetencionISRL.Text = DgvListadoOrdenes.CurrentRow.Cells["Comprobante_ISLR_Numero"].Value.ToString();
                    }
                    //NumeroOrdenRetencion = DgvListadoOrdenes.CurrentRow.Cells["NumOrdserv"].Value.ToString();
                    //PAGOS_IVA = DgvListadoOrdenes.CurrentRow.Cells["PAGOS_IVA"].Value.ToString();
                    //PAGOS_ISLR = DgvListadoOrdenes.CurrentRow.Cells["PAGOS_ISLR"].Value.ToString();
                }

            }

            // Asignar Rx
            if (DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Btn6")
            {
                //Se cargan los datos de la orden 
                _D_DetalleOrden.Datos_de_la_Orden(DgvListadoOrdenes.CurrentRow.Cells["NumOrdserv"].Value.ToString(), DgvListadoOrdenes.CurrentRow.Cells["Revision"].Value.ToString());

                // Se pide la clave de gerente
                if (TB_CAORDSER.Cod_DetVta == "08")
                    _FrmClaveGerente.ShowDialog();
                else
                    return;

                if (_FrmClaveGerente.ClaveCorrecta == true)
                {

                    if ( _FrmClaveGerente.DialogResult == DialogResult.OK)
                    {
                        Formulario_CargaOrden = true;
                        PnlLSecundario.Visible = true;
                        PnlLSecundario.Enabled = true;
                        LblListadoOrdenes.Visible = false;
                        CbxUltimosTesD.Visible = false;
                        txtNumeroOrden.Visible = false;
                        TxtCedula.Visible = false; // agregado el 01/06/2023
                        CbxEstatus.Visible = false;
                        Btnlupa.Visible = false;
                        DgvListadoOrdenes.Visible = false;
                        DtpDesde.Visible = false;
                        DtpHasta.Visible = false;
                        Lbldesde.Visible = false;
                        LblHasta.Visible = false;
                        LblOpciones.Visible = false;
                        PnlLSecundario.Visible = true;
                        PnlLSecundario.Enabled = true;
                        PnlLSecundario.Location = new Point(50, 50);
                        PnlLSecundario.Size = (Size)new Point(1472, 1168);
                        BtnCancelar.Location = new Point(865, 660);
                        BtnCancelar.Visible = true;
                        PnlLSecundario.Controls.Clear();
                        _FrmFacturacion.LimpiaVariablesIdAbonoPagoMovil();
                        AbrirForm(_FrmCargarOrden);
                        _FrmCargarOrden.Txt_Tap1_Cedula.Text = TB_CAORDSER.CTE_CedIden;
                        _FrmCargarOrden.Cbx_Tap1_Nacionalidad.Text = TB_CAORDSER.CTE_Nacio;
                        _FrmCargarOrden.Btn_Tap2_Derecha_Click(this, EventArgs.Empty);
                        _FrmCargarOrden.Txt_Tap1_Cedula_MouseLeave(sender, e);
                        _FrmCargarOrden.VisualizarPanel("MostrarCabeceraExamen");
                        _FrmCargarOrden.Formulario_ListaOrdenes = true;
                        _FrmCargarOrden.tabControl.SelectedIndex = 1;
                        _FrmCargarOrden.btnPrincipal.Enabled = false;
                        _FrmCargarOrden.btnCargarOrden.Enabled = false;
                        _FrmCargarOrden.btnExamen.Enabled = false;
                        _FrmCargarOrden.AsignarRx_JuegoPantalla();
                        Paginado_Habilitar(false);

                    }

                }

            }

        }

        public void AbrirForm(Form F)
        {

            while (PnlLSecundario.Controls.Count > 0)
            {
                PnlLSecundario.Controls.RemoveAt(index: 0);
            }


            F.TopLevel = false;
            F.FormBorderStyle = FormBorderStyle.None;
            //F.Dock = DockStyle.Fill;

            //AddOwnedForm(_FrmFacturacion);
            this.PnlLSecundario.Controls.Add(F);

            F.Show();
            F.BringToFront();
            return;

        }



        private void txtNumeroOrden_Enter(object sender, EventArgs e)
        {
            txtNumeroOrden.Text = "";
            txtNumeroOrden.ForeColor = Color.Black;
        }

        private void txtNumeroOrden_Leave(object sender, EventArgs e)
        {
            //orden = txtNumeroOrden.Text;
            if (txtNumeroOrden.Text.Equals("N° de orden"))
            {
                txtNumeroOrden.Text = "N° de orden";
                txtNumeroOrden.ForeColor = Color.Gray;



            }
            else
            {
                if (txtNumeroOrden.Text.Equals(""))
                {
                    txtNumeroOrden.Text = "N° de orden";
                    txtNumeroOrden.ForeColor = Color.Gray;
                }
                else
                {
                    orden = txtNumeroOrden.Text;

                    txtNumeroOrden.ForeColor = Color.Black;

                }
            }
        }

        private void CbxUltimosTesD_Enter(object sender, EventArgs e)
        {
            //orden = txtNumeroOrden.Text;
            //if (orden.Equals("N° de orden"))
            //{
            //    txtNumeroOrden.Text = "";
            //    txtNumeroOrden.ForeColor = Color.Gray;

            //}
        }


        private void CbxEstatus_Enter(object sender, EventArgs e)
        {
            //orden = txtNumeroOrden.Text;
            //if (orden.Equals("N° de orden"))
            //{
            //    txtNumeroOrden.Text = "";
            //    txtNumeroOrden.ForeColor = Color.Gray;
            //}
        }

        public void BtnCancelar_Click(object sender, EventArgs e)
        {
            string NumOrden = RecNumOrden();
            string NumCedula = RecNumCedula();

            Lbldesde.Visible = false;
            LblHasta.Visible = false;
            LblListadoOrdenes.Visible = true;
            CbxUltimosTesD.Visible = true;
            txtNumeroOrden.Visible = true;
            TxtCedula.Visible = true; // agregado el 01/06/2023 
            CbxEstatus.Visible = true;
            Btnlupa.Visible = true;
            DgvListadoOrdenes.Visible = true;
            PnlLSecundario.Visible = false;
            PnlLSecundario.Enabled = false;
            PnlLSecundario.Controls.Clear();
            BtnCancelar.Visible = false;

            if (!Formulario_CargaOrden)
            {
                _FrmFacturacion.LimpiarGrid();
                _FrmFacturacion.LimpiarNotasCredito();
                _FrmFacturacion.VisualizarPanel("MostrarFormulario");
                _FrmFacturacion.LimpiarTxbox();
                _FrmFacturacion.tabControl.SelectTab(0);
            }

            LimpiarGrid();

            int Pagina = Convert.ToInt32(cbPagina_Ini.SelectedIndex + 1);
            Indice = Pagina - 1;
            PaginaInico = ((Pagina - 1) * NUmeroFilas) + 1;
            PaginaFinal = Pagina * NUmeroFilas;

            if (CbxUltimosTesD.SelectedIndex == 5)
            {


                DataSet Dts = _ListaOrdenes.TraerOrdporRango(DtpDesde, DtpHasta, CbxEstatus, NumCedula, PaginaInico, PaginaFinal);
                if (Dts != null)
                {
                    DgvListadoOrdenes.DataSource = Dts.Tables[0];
                    Paginado(Dts);
                    Paginado_Habilitar(true);
                }
                else
                {
                    Paginado_Habilitar(false);
                }

                if (DgvListadoOrdenes.Rows.Count > 0)
                {
                    DgvListadoOrdenes.Visible = true;
                    DtpDesde.Visible = true;
                    DtpHasta.Visible = true;
                    Lbldesde.Visible = true;
                    LblHasta.Visible = true;
                    CrearObjetos();
                    ColorearStatus();
                }
                else
                {
                    DgvListadoOrdenes.Visible = false;

                }

                // Seleccionar una fila del Datagridview dependiendo de la ultima selecion del usuario 
                foreach (DataGridViewRow Fila in DgvListadoOrdenes.Rows)
                {
                    if (Fila.Cells["NumOrdserv"].Value.ToString() == TB_CAORDSER.NumOrdserv)
                    {
                        DgvListadoOrdenes.CurrentCell = this.DgvListadoOrdenes[0, Fila.Index];
                        //DgvListadoOrdenes.Rows[registro].Selected = true;
                    }
                }

            }
            else
            {

                DataSet Dts = _ListaOrdenes.TraerOrdenes(CbxUltimosTesD, CbxEstatus, NumOrden, NumCedula, PaginaInico, PaginaFinal);
                if (Dts != null)
                {
                    DgvListadoOrdenes.DataSource = Dts.Tables[0];
                    Paginado(Dts);
                    Paginado_Habilitar(true);
                }
                else
                {
                    Paginado_Habilitar(false);
                }

                if (DgvListadoOrdenes.Rows.Count > 0)
                {
                    DgvListadoOrdenes.Visible = true;
                    DtpDesde.Visible = false;
                    DtpHasta.Visible = false;
                    Lbldesde.Visible = false;
                    LblHasta.Visible = false;
                    CrearObjetos();
                    ColorearStatus();
                }
                else
                {
                    DgvListadoOrdenes.Visible = false;
                    LblOpciones.Visible = false;

                }



                // Seleccionar una fila del Datagridview dependiendo de la ultima selecion del usuario 
                foreach (DataGridViewRow Fila in DgvListadoOrdenes.Rows)
                {
                    if (Fila.Cells["NumOrdserv"].Value.ToString() == TB_CAORDSER.NumOrdserv)
                    {
                        DgvListadoOrdenes.CurrentCell = this.DgvListadoOrdenes[0, Fila.Index];
                        //DgvListadoOrdenes.Rows[registro].Selected = true;
                    }
                }

            }

            Formulario_CargaOrden = false;
        }


        public void cerrar()
        {
            string NumOrden = RecNumOrden();
            string NumCedula = RecNumCedula();
            int Pagina = Convert.ToInt32(cbPagina_Ini.SelectedIndex + 1);
            Indice = Pagina - 1;
            PaginaInico = ((Pagina - 1) * NUmeroFilas) + 1;
            PaginaFinal = Pagina * NUmeroFilas;
            LimpiarGrid();
            if (CbxUltimosTesD.SelectedIndex == 5)
            {


                DataSet Dts = _ListaOrdenes.TraerOrdporRango(DtpDesde, DtpHasta, CbxEstatus, NumCedula, PaginaInico, PaginaFinal);
                if (Dts != null)
                {
                    DgvListadoOrdenes.DataSource = Dts.Tables[0];
                    Paginado(Dts);
                    Paginado_Habilitar(true);
                }
                else
                {
                    Paginado_Habilitar(false);
                }

                if (DgvListadoOrdenes.Rows.Count > 0)
                {
                    DgvListadoOrdenes.Visible = true;
                    DtpDesde.Visible = true;
                    DtpHasta.Visible = true;
                    Lbldesde.Visible = true;
                    LblHasta.Visible = true;
                    CrearObjetos();
                    ColorearStatus();
                }
                else
                {
                    DgvListadoOrdenes.Visible = false;

                }

                // Seleccionar una fila del Datagridview dependiendo de la ultima selecion del usuario 
                foreach (DataGridViewRow Fila in DgvListadoOrdenes.Rows)
                {
                    if (Fila.Cells["NumOrdserv"].Value.ToString() == TB_CAORDSER.NumOrdserv)
                    {
                        DgvListadoOrdenes.CurrentCell = this.DgvListadoOrdenes[0, Fila.Index];
                        //DgvListadoOrdenes.Rows[registro].Selected = true;
                    }
                }

            }
            else
            {

                DataSet Dts = _ListaOrdenes.TraerOrdenes(CbxUltimosTesD, CbxEstatus, NumOrden, NumCedula, PaginaInico, PaginaFinal);
                if (Dts != null)
                {
                    DgvListadoOrdenes.DataSource = Dts.Tables[0];
                    Paginado(Dts);
                    Paginado_Habilitar(true);
                }
                else
                {
                    Paginado_Habilitar(false);
                }

                if (DgvListadoOrdenes.Rows.Count > 0)
                {
                    DgvListadoOrdenes.Visible = true;
                    DtpDesde.Visible = false;
                    DtpHasta.Visible = false;
                    Lbldesde.Visible = false;
                    LblHasta.Visible = false;
                    CrearObjetos();
                    ColorearStatus();
                }
                else
                {
                    DgvListadoOrdenes.Visible = false;
                    LblOpciones.Visible = false;

                }



                // Seleccionar una fila del Datagridview dependiendo de la ultima selecion del usuario 
                foreach (DataGridViewRow Fila in DgvListadoOrdenes.Rows)
                {
                    if (Fila.Cells["NumOrdserv"].Value.ToString() == TB_CAORDSER.NumOrdserv)
                    {
                        DgvListadoOrdenes.CurrentCell = this.DgvListadoOrdenes[0, Fila.Index];
                        //DgvListadoOrdenes.Rows[registro].Selected = true;
                    }
                }

            }


        }

        private void BtnEnviar_Click(object sender, EventArgs e)
        {
            //LimpiarGrid();
            //DgvListadoOrdenes.DataSource = _ListaOrdenes.TraerOrdporRango(DtpDesde, DtpHasta, CbxEstatus);
            //if (DgvListadoOrdenes.Rows.Count > 0)
            //{

            //    CrearObjetos();
            //    ColorearStatus();
            //}
        }

        private void Lbldesde_Click(object sender, EventArgs e)
        {

        }

        private void txtNumeroOrden_KeyPress(object sender, KeyPressEventArgs e)
        {
            //para que solo acepte numeros
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e) // Boton guardar del panel de nota de credito manual 
        {

            string NroControl = Convert.ToString(TxtNumeroCorrelativo.Text);
            NroControl = NroControl.Replace("_", "");

            // Se valida que el numero de control tenga 11 caracteres y el numero de factura 11
            if (TxtNumeroNC.TextLength == 7 & NroControl.Length == 11)
            {


                _FrmAnulacion.GerenteAutoriza = NombreNCManual;
                _FrmAnulacion.CargarOrdenAnular(TB_CAORDSER.NumOrdserv);
                _FrmAnulacion.ManualNroNotaCredito = TxtNumeroNC.Text;
                _FrmAnulacion.ManualNroNotaCreditonNunControl = TxtNumeroCorrelativo.Text;

                _FrmAnulacion.ShowDialog();

                if (_FrmAnulacion.ResultadoNCManual == "SATISFACTORIO")
                {
                    TxtNumeroNC.Text = "";
                    TxtNumeroCorrelativo.Text = "";
                    PnlNotaCreditoManual.Visible = false;
                    Btnlupa.PerformClick();

                    // Seleccionar una fila del Datagridview dependiendo de la ultima selecion del usuario 
                    foreach (DataGridViewRow Fila in DgvListadoOrdenes.Rows)
                    {
                        if (Fila.Cells["NumOrdserv"].Value.ToString() == TB_CAORDSER.NumOrdserv)
                        {
                            DgvListadoOrdenes.CurrentCell = this.DgvListadoOrdenes[0, Fila.Index];
                            //DgvListadoOrdenes.Rows[registro].Selected = true;
                        }
                    }

                }
                else
                {
                    PnlNotaCreditoManual.Visible = true;

                }
            }

            else
            {
                string mensajer = "El número de factura debe tener 7 dígitos y el número de control 10 dígitos";
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(mensajer);
                _FrmMensajes.ShowDialog();

            }


        }

        public void LimpiarTxt()
        {
            TxtNumeroNC.Text = _D_DetalleOrden.ParametroSerieManual();
            TxtNumeroCorrelativo.Text = "";


        }

        public void RecargaPostNC()
        {

            string NumOrden = RecNumOrden();
            string NumCedula = RecNumCedula();
            LimpiarGrid();
            if (CbxUltimosTesD.SelectedIndex == 5)
            {


                DataSet Dts = _ListaOrdenes.TraerOrdporRango(DtpDesde, DtpHasta, CbxEstatus, NumCedula);
                if (Dts != null)
                {
                    DgvListadoOrdenes.DataSource = Dts.Tables[0];
                    Paginado(Dts);
                    Paginado_Habilitar(true);
                }
                else
                {
                    Paginado_Habilitar(false);
                }

                if (DgvListadoOrdenes.Rows.Count > 0)
                {
                    DgvListadoOrdenes.Visible = true;
                    DtpDesde.Visible = true;
                    DtpHasta.Visible = true;
                    Lbldesde.Visible = true;
                    LblHasta.Visible = true;
                    CrearObjetos();
                    ColorearStatus();
                }
                else
                {
                    DgvListadoOrdenes.Visible = false;

                }



            }
            else
            {

                DataSet Dts = _ListaOrdenes.TraerOrdenes(CbxUltimosTesD, CbxEstatus, NumOrden, NumCedula);
                if (Dts != null)
                {
                    DgvListadoOrdenes.DataSource = Dts.Tables[0];
                    Paginado(Dts);
                    Paginado_Habilitar(true);
                }
                else
                {
                    Paginado_Habilitar(false);
                }

                if (DgvListadoOrdenes.Rows.Count > 0)
                {
                    DgvListadoOrdenes.Visible = true;
                    DtpDesde.Visible = false;
                    DtpHasta.Visible = false;
                    Lbldesde.Visible = false;
                    LblHasta.Visible = false;
                    CrearObjetos();
                    ColorearStatus();
                }
                else
                {
                    DgvListadoOrdenes.Visible = false;
                    LblOpciones.Visible = false;

                }



            }



        }

        private void button1_Click(object sender, EventArgs e)
        {
            PnlNotaCreditoManual.Visible = false;
            LimpiarTxt();

        }

        public string RecNumOrden()
        {
            // Con esta funcion se determina si el campo de orden esta vacio o si no se ha escrito nada para enviar blanco 
            // de manera que el texto N orden como place holder no afecte en la busqueda 
            if (txtNumeroOrden.Text.Equals("N° de orden"))
            {

                orden = " ";
            }
            else
            {
                if (orden.Equals(""))
                {
                    orden = " ";
                }
                else
                {
                    orden = txtNumeroOrden.Text;
                    txtNumeroOrden.ForeColor = Color.Black;

                }
            }
            return orden;

        }


        public string RecNumCedula()
        {
            // Con esta funcion se determina si el campo de cedula esta vacio o si no se ha escrito nada para enviar blanco 
            // de manera que el texto N de cedula como place holder no afecte en la busqueda 
            if (TxtCedula.Text.Equals("N° de cédula"))
            {

                cedula = "";
            }
            else
            {
                if (TxtCedula.Text.Equals(""))
                {
                    cedula = "";
                }
                else
                {
                    cedula = TxtCedula.Text;
                    TxtCedula.ForeColor = Color.Black;

                }
            }
            return cedula;

        }




        private void TxtCedula_Enter(object sender, EventArgs e)
        {
            TxtCedula.Text = "";
            TxtCedula.ForeColor = Color.Black;
        }

        private void TxtCedula_Leave(object sender, EventArgs e)
        {
            if (TxtCedula.Text.Equals("N° de cédula"))
            {
                TxtCedula.Text = "N° de cédula";
                TxtCedula.ForeColor = Color.Gray;



            }
            else
            {
                if (TxtCedula.Text.Equals(""))
                {
                    TxtCedula.Text = "N° de cédula";
                    TxtCedula.ForeColor = Color.Gray;
                }
                else
                {
                    orden = TxtCedula.Text;

                    TxtCedula.ForeColor = Color.Black;

                }
            }
        }

        private void TxtCedula_KeyPress(object sender, KeyPressEventArgs e)
        {
            //para que solo acepte numeros
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void BtnGuardarRetencion_Click(object sender, EventArgs e)
        {

            try
            {
                string NumeroOrdenRetencion = DgvListadoOrdenes.CurrentRow.Cells["NumOrdserv"].Value.ToString();
                string PAGOS_IVA = DgvListadoOrdenes.CurrentRow.Cells["PAGOS_IVA"].Value.ToString();
                string PAGOS_ISLR = DgvListadoOrdenes.CurrentRow.Cells["PAGOS_ISLR"].Value.ToString();

                if (PAGOS_IVA == "1" & DgvListadoOrdenes.CurrentRow.Cells["Comprobante_IVA"].Value.ToString() == "0")
                {
                    if (TxtRetencionIVA.Text == "" | TxtRetencionIVA.TextLength < 9 | TxtRetencionIVA.Text.Replace(" ", "") == "" | (TxtRetencionIVA.Text.Replace(" ", "")).Length < 9)
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("Debe llenar todos los campos, Verifique");
                        _FrmMensajes.ShowDialog();
                        return;
                    }

                }


                if (PAGOS_ISLR == "1" & DgvListadoOrdenes.CurrentRow.Cells["Comprobante_ISLR"].Value.ToString() == "0")
                {
                    if (TxtRetencionISRL.Text == "" | TxtRetencionISRL.TextLength < 9 | TxtRetencionISRL.Text.Replace(" ", "") == "" | (TxtRetencionISRL.Text.Replace(" ", "")).Length < 9)
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("Debe llenar todos los campos, Verifique");
                        _FrmMensajes.ShowDialog();
                        return;
                    }

                }

                if (PAGOS_IVA == "1" & DgvListadoOrdenes.CurrentRow.Cells["Comprobante_IVA"].Value.ToString() == "1" & TxtRetencionIVA.Enabled == false & TxtRetencionISRL.Enabled == false
                    | PAGOS_ISLR == "1" & DgvListadoOrdenes.CurrentRow.Cells["Comprobante_ISLR"].Value.ToString() == "1" & TxtRetencionISRL.Enabled == false & TxtRetencionIVA.Enabled == false)
                {

                    PnlComprobanteRetencion.Enabled = false;
                    PnlComprobanteRetencion.Visible = false;
                    TxtRetencionIVA.Text = "";
                    TxtRetencionISRL.Text = "";
                    TxtRetencionFactura.Text = "";
                    return;
                }

                _FrmClaveGerente.ShowDialog();

                if (_FrmClaveGerente.DialogResult == DialogResult.OK)
                {
                    if (_FrmClaveGerente.ClaveCorrecta == true)
                    {
                        if (PAGOS_IVA == "1" & DgvListadoOrdenes.CurrentRow.Cells["Comprobante_IVA"].Value.ToString() == "0")
                        {
                            _D_Anulacion.CaragarAuditor(TB_USUARIO.COD_SUCURSAL, "086", TB_USUARIO.COD_EMPLEADO, "OS: " + NumeroOrdenRetencion + ", Factura: " + TxtRetencionFactura.Text + ", ComprobanteIVA: " + TxtRetencionIVA.Text + ", Autoriza: " + VariablesGlobales.UsuarioAutorizado_FrmClaveGerente);
                            _D_DetalleOrden.Registar_IVA_Facturacion(TxtRetencionIVA.Text, TxtRetencionFactura.Text, DgvListadoOrdenes.CurrentRow.Cells["Cod_Sucursal"].Value.ToString(), DgvListadoOrdenes.CurrentRow.Cells["NumOrdserv"].Value.ToString());

                        }

                        if (PAGOS_ISLR == "1" & DgvListadoOrdenes.CurrentRow.Cells["Comprobante_ISLR"].Value.ToString() == "0")
                        {
                            _D_Anulacion.CaragarAuditor(TB_USUARIO.COD_SUCURSAL, "087", TB_USUARIO.COD_EMPLEADO, "OS: " + NumeroOrdenRetencion + ", Factura: " + TxtRetencionFactura.Text + ", ComprobanteISRL: " + TxtRetencionISRL.Text + ", Autoriza: " + VariablesGlobales.UsuarioAutorizado_FrmClaveGerente);
                            _D_DetalleOrden.Registar_ISLR_Facturacion(TxtRetencionISRL.Text, TxtRetencionFactura.Text, DgvListadoOrdenes.CurrentRow.Cells["Cod_Sucursal"].Value.ToString(), DgvListadoOrdenes.CurrentRow.Cells["NumOrdserv"].Value.ToString());
                        }

                        PnlComprobanteRetencion.Enabled = false;
                        PnlComprobanteRetencion.Visible = false;
                        TxtRetencionIVA.Text = "";
                        TxtRetencionISRL.Text = "";
                        TxtRetencionFactura.Text = "";
                        Btnlupa.PerformClick();

                        // Seleccionar una fila del Datagridview dependiendo de la ultima selecion del usuario 
                        foreach (DataGridViewRow Fila in DgvListadoOrdenes.Rows)
                        {
                            if (Fila.Cells["NumOrdserv"].Value.ToString() == TB_CAORDSER.NumOrdserv)
                            {
                                DgvListadoOrdenes.CurrentCell = this.DgvListadoOrdenes[0, Fila.Index];
                                //DgvListadoOrdenes.Rows[registro].Selected = true;
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
        }

        private void BtnCancelarRetencion_Click(object sender, EventArgs e)
        {
            PnlComprobanteRetencion.Enabled = false;
            PnlComprobanteRetencion.Visible = false;
            TxtRetencionIVA.Text = "";
            TxtRetencionISRL.Text = "";
            TxtRetencionFactura.Text = "";
        }

        // Colocarle Tool Tips a los botones de listado de órdenes, para indicar el nombre de cada botón.
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
                        cell.ToolTipText = "Facturación";
                    }
                }


                // Anular orden
                if (this.DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Btn2")
                {
                    // comprobar si la celda tiene contenido válido
                    if (e.Value != System.DBNull.Value)
                    {
                        cell.ToolTipText = "Devolución";
                    }
                }


                // Procesar sin pagos
                if (this.DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Btn3")
                {
                    // comprobar si la celda tiene contenido válido
                    if (e.Value != System.DBNull.Value)
                    {
                        cell.ToolTipText = "Procesar sin pagos";
                    }
                }


                // Reimprimir
                if (this.DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Btn4")
                {
                    // comprobar si la celda tiene contenido válido
                    if (e.Value != System.DBNull.Value)
                    {
                        cell.ToolTipText = "Reimprimir Abono";
                    }
                }


                // de Comprobante de Retención de IVA y ISRL 
                if (this.DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Btn5")
                {
                    // comprobar si la celda tiene contenido válido
                    if (e.Value != System.DBNull.Value)
                    {
                        cell.ToolTipText = "Comprobante de Retención";
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

        private void cbPagina_Ini_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int Pagina = Convert.ToInt32(cbPagina_Ini.SelectedIndex + 1);
            Indice = Pagina - 1;
            PaginaInico = ((Pagina - 1) * NUmeroFilas) + 1;
            PaginaFinal = Pagina * NUmeroFilas;

            string NumOrden = RecNumOrden();
            string NumCedula = RecNumCedula();
            if (CbxUltimosTesD.SelectedIndex == 5)
            {


                DataSet Dts = _ListaOrdenes.TraerOrdporRango(DtpDesde, DtpHasta, CbxEstatus, NumCedula, PaginaInico, PaginaFinal);
                if (Dts != null)
                {
                    LimpiarGrid();
                    DgvListadoOrdenes.DataSource = Dts.Tables[0];
                    Paginado(Dts);
                    Paginado_Habilitar(true);
                }
                else
                {
                    Paginado_Habilitar(false);
                }

                if (DgvListadoOrdenes.Rows.Count > 0)
                {
                    DgvListadoOrdenes.Visible = true;
                    DtpDesde.Visible = true;
                    DtpHasta.Visible = true;
                    Lbldesde.Visible = true;
                    LblHasta.Visible = true;
                    CrearObjetos();
                    ColorearStatus();
                }
                else
                {
                    DgvListadoOrdenes.Visible = false;

                }



            }
            else
            {

                DataSet Dts = _ListaOrdenes.TraerOrdenes(CbxUltimosTesD, CbxEstatus, NumOrden, NumCedula, PaginaInico, PaginaFinal);
                if (Dts != null)
                {
                    LimpiarGrid();
                    DgvListadoOrdenes.DataSource = Dts.Tables[0];
                    Paginado(Dts);
                    Paginado_Habilitar(true);
                }
                else
                {
                    Paginado_Habilitar(false);
                }

                if (DgvListadoOrdenes.Rows.Count > 0)
                {
                    DgvListadoOrdenes.Visible = true;
                    DtpDesde.Visible = false;
                    DtpHasta.Visible = false;
                    Lbldesde.Visible = false;
                    LblHasta.Visible = false;
                    CrearObjetos();
                    ColorearStatus();
                }
                else
                {
                    DgvListadoOrdenes.Visible = false;
                    LblOpciones.Visible = false;

                }



            }


        }

        private void cbPagina_Ini_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void TxtRetencionIVA_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtRetencionISRL_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void Paginado(DataSet Dts)
        {
            PaginaFinal = NUmeroFilas;
            int Pagina = Convert.ToInt32(cbPagina_Ini.SelectedIndex + 1);

            if (Pagina == 0)
                Pagina = 1;

            //Numero Filas
            Int32 Cantidad = Convert.ToInt32(Dts.Tables[1].Rows[0][0]) / NUmeroFilas;

            if (Convert.ToInt32(Dts.Tables[1].Rows[0][0]) % NUmeroFilas > 0)
                Cantidad++;
            txtPagina_Fin.Text = Cantidad.ToString();
            cbPagina_Ini.Items.Clear();

            for (int x = 1; x <= Cantidad; x++)
            {
                cbPagina_Ini.Items.Add(x.ToString());
            }

            if (Pagina > Cantidad)
            {
                cbPagina_Ini.SelectedIndex = 0;
            }
            else
            {
                Indice = Pagina - 1;
                cbPagina_Ini.SelectedIndex = Indice;
            }



        }

        private void Paginado_Habilitar(Boolean Habilitar)
        {
            label11.Visible = Habilitar;
            cbPagina_Ini.Visible = Habilitar;
            label12.Visible = Habilitar;
            txtPagina_Fin.Visible = Habilitar;
        }

    }
}
