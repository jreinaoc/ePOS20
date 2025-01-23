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
                        

                    }

                    if (Status == "BLOQUEADO")
                    {
                        Fila.Cells["Estado"].Style.BackColor = colanul;
                        //Fila.Cells["Estatus"].Style.Padding = newPadding;
                        DgvListadoOrdenes.Columns["Estado"].DefaultCellStyle.Format = "C";
                        Fila.Cells["Estado"].Style.ForeColor = Color.White;
                    }

                    if (Status == "REALIZADO")
                    {
                        Fila.Cells["Estado"].Style.BackColor = colfact;
                        //Fila.Cells["Estatus"].Style.Padding = newPadding;
                        DgvListadoOrdenes.Columns["Estado" +
                            ""].DefaultCellStyle.Format = "C";
                        Fila.Cells["Estado"].Style.ForeColor = Color.FromArgb(89, 190, 186);

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
                DgvListadoOrdenes.Columns["NroOrden"].HeaderText = "N° de orden";
                DgvListadoOrdenes.Columns["Nombre"].HeaderText = "Nombre";
                DgvListadoOrdenes.Columns["Cedula"].HeaderText = "Cédula";
                DgvListadoOrdenes.Columns["Telefono"].HeaderText = "Teléfono";
                DgvListadoOrdenes.Columns["BancoReceptor"].HeaderText = "Banco";
                DgvListadoOrdenes.Columns["MontoVueltoRef"].HeaderText = "Monto";
                DgvListadoOrdenes.Columns["Referencia"].HeaderText = "Referencia";
                DgvListadoOrdenes.Columns["Estado"].HeaderText = "Estado";

                //Ancho de columna
                DgvListadoOrdenes.Columns["Fecha"].Width = 90;
                DgvListadoOrdenes.Columns["NroOrden"].Width = 60;
                DgvListadoOrdenes.Columns["Nombre"].Width = 150;
                DgvListadoOrdenes.Columns["Cedula"].Width = 80;
                DgvListadoOrdenes.Columns["Telefono"].Width = 100;
                DgvListadoOrdenes.Columns["BancoReceptor"].Width = 100;
                DgvListadoOrdenes.Columns["MontoVueltoRef"].Width = 100;
                DgvListadoOrdenes.Columns["Referencia"].Width = 80;
                DgvListadoOrdenes.Columns["Estado"].Width = 90;
                DgvListadoOrdenes.Columns["Btn"].Width = 80;
                DgvListadoOrdenes.Columns["Btn2"].Width = 80;

                //Bloquear Columna 
                DgvListadoOrdenes.Columns["Fecha"].ReadOnly = true;
                DgvListadoOrdenes.Columns["NroOrden"].ReadOnly = true;
                DgvListadoOrdenes.Columns["Nombre"].ReadOnly = true;
                DgvListadoOrdenes.Columns["Cedula"].ReadOnly = true;
                DgvListadoOrdenes.Columns["Telefono"].ReadOnly = true;
                DgvListadoOrdenes.Columns["BancoReceptor"].ReadOnly = true;
                DgvListadoOrdenes.Columns["MontoVueltoRef"].ReadOnly = false;
                DgvListadoOrdenes.Columns["Referencia"].ReadOnly = false;
                DgvListadoOrdenes.Columns["Estado"].ReadOnly = false;
                DgvListadoOrdenes.Columns["Btn"].ReadOnly = false;
                DgvListadoOrdenes.Columns["Btn2"].ReadOnly = false;
                
                //ordenar las colunmnas del grid 
                DgvListadoOrdenes.Columns["Fecha"].DisplayIndex = 0;
                DgvListadoOrdenes.Columns["NroOrden"].DisplayIndex = 1;
                DgvListadoOrdenes.Columns["Nombre"].DisplayIndex = 2;
                DgvListadoOrdenes.Columns["Cedula"].DisplayIndex = 3;
                DgvListadoOrdenes.Columns["Telefono"].DisplayIndex = 4;
                DgvListadoOrdenes.Columns["BancoReceptor"].DisplayIndex = 5;
                DgvListadoOrdenes.Columns["MontoVueltoRef"].DisplayIndex = 6;
                DgvListadoOrdenes.Columns["Referencia"].DisplayIndex = 7;
                DgvListadoOrdenes.Columns["Estado"].DisplayIndex = 8;
                DgvListadoOrdenes.Columns["Btn"].DisplayIndex = 9;
                DgvListadoOrdenes.Columns["Btn2"].DisplayIndex = 10;

                // nuevo 21-08-2023 
                DgvListadoOrdenes.Columns["MontoVueltoRef"].DefaultCellStyle.Format = "##,##0.00";

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

        public void DgvListadoOrdenes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Btn") //  PARA FACTURAR
            {

                //CbxUltimosTesD.Visible = false;
               
               
                Btnlupa.Visible = false;
                DgvListadoOrdenes.Visible = false;
                //DtpDesde.Visible = false;
                //DtpHasta.Visible = false;
                //Lbldesde.Visible = false;
                //LblHasta.Visible = false;

                //AbrirForm(_FrmFacturacion);
                // Paginado_Habilitar(false);
                // _FrmFacturacion.LimpiarGrid();
                if (DgvListadoOrdenes.CurrentRow.Cells["Estatus"].Value.ToString() == "Abonada  ")
                {
                    // Recalcular la orden
                    string rep = _D_DetalleOrden.RecalculaOSDivisa(DgvListadoOrdenes.CurrentRow.Cells["Cod_Sucursal"].Value.ToString(), DgvListadoOrdenes.CurrentRow.Cells["NumOrdserv"].Value.ToString(), "0");
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
                if (DgvListadoOrdenes.CurrentRow.Cells["Estatus"].Value.ToString() == "Abonada  ")
                {
                    // Con la siguiente funcion se cargan los datos en la entidad tbCaordser
                    _FrmFacturacion.CargarDatosOrden(DgvListadoOrdenes.CurrentRow.Cells["NumOrdserv"].Value.ToString(), DgvListadoOrdenes.CurrentRow.Cells["Nombre"].Value.ToString(), DgvListadoOrdenes.CurrentRow.Cells["Revision"].Value.ToString());


                    {
                        return;
                    }

                }

                ////NoataCredito
                if (DgvListadoOrdenes.CurrentRow.Cells["Estatus"].Value.ToString() == "Facturada")
                {



                }
                //fin Nota Credito


                //Para anular orden por pagar 
                if (DgvListadoOrdenes.CurrentRow.Cells["Estatus"].Value.ToString() == "Por pagar")
                {
                    _FrmFacturacion.CargarDatosOrden(DgvListadoOrdenes.CurrentRow.Cells["NumOrdserv"].Value.ToString(), DgvListadoOrdenes.CurrentRow.Cells["Nombre"].Value.ToString(), DgvListadoOrdenes.CurrentRow.Cells["Revision"].Value.ToString());

                    // Se pregunta si esta seguro de anular 



                }
            }


            //Procesar sin pagos 

            if (DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Btn3")
            {
                //Validacion del dia activo 
                string DiaActual = (DateTime.Now.ToString("dd/MM/yyyy"));
                string DiaActivo = _D_Inicio.DiaActivo().ToShortDateString();


            }


            // Reimprimir Abono
            if (DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Btn4") // Cuando se da click en el boton azul de reimprimir, obtiene el reporte del abono 
            { //Se cargan los datos de la orden 
                string concat = DgvListadoOrdenes.CurrentRow.Cells["Cod_Sucursal"].Value.ToString() + DgvListadoOrdenes.CurrentRow.Cells["NumOrdserv"].Value.ToString() + "0";
                // Se valida si la orden es abonada para que pueda imprimir o mostrar el reporte 


            }

            //Boton numero 5 
            //Registrar Comprobante de IVA y ISLR
            if (DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Btn5") // Cuando se da click se despliega el Comprobante IVA y ISLR
            {
                //SE VALIDA QUE LA ORDEN TENGA PAGOS CON IVA Y ISLR Y QUE NO TENGA CARGADO EL COMPROBANTE 


            }

        }

        private void btnlupa_Click_1(object sender, EventArgs e)
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
                }
                else
                {
                    DgvListadoOrdenes.Visible = false;
                    //LblOpciones.Visible = false;

                }

           
        }
    }
}
