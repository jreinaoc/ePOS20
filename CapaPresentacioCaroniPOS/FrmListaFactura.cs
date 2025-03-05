using CapaLogica.ListaFactura_Logica;
using CapaLogica.ListaOrden_Logica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using classUtilities;

namespace CapaVisual_Login
{
    public partial class FrmListaFactura : Form
    {
        public FrmListaFactura()
        {
            InitializeComponent();
        }

        DataSet Dts;
        private L_ListaOrdenes _ListaOrdenes = new L_ListaOrdenes();
        private L_ListaFacturas _L_ListaFacturas = new L_ListaFacturas();
        int PaginaInico = 1, Indice = 0, NUmeroFilas = 12, PaginaFinal;

        private void Btnlupa_Click(object sender, EventArgs e)
        {
               LimpiarGrid();

            if (CbxEstatus.SelectedIndex <= 0)
            {
                Dts = _L_ListaFacturas.TraerFacturasRango(DtpDesde, DtpHasta);

            }
            else
            {
                Dts = _L_ListaFacturas.TraerNotasRango(DtpDesde, DtpHasta);
            }

            if (Dts != null)
            {
                DgvListaFacturas1.DataSource = Dts.Tables[0];
                Paginado(Dts);
                Paginado_Habilitar(true);
            }
            else
            {
                Paginado_Habilitar(false);
            }


            if (DgvListaFacturas1.Rows.Count > 0)
            {
                DgvListaFacturas1.Visible = true;
                EstructuraGrid();
            }
            else
            {
                DgvListaFacturas1.Visible = false;
            }


        }

        private void LimpiarGrid()
        {
            try
            {
                //limpiar el grid 

                DgvListaFacturas1.DataSource = "";
                DgvListaFacturas1.DataMember = "";
            }

            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }

        }

        private void cbPagina_Ini_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int Pagina = Convert.ToInt32(cbPagina_Ini.SelectedIndex + 1);
            Indice = Pagina - 1;
            PaginaInico = ((Pagina - 1) * NUmeroFilas) + 1;
            PaginaFinal = Pagina * NUmeroFilas;


            LimpiarGrid();

            Dts = _L_ListaFacturas.TraerFacturasRango(DtpDesde, DtpHasta, PaginaInico, PaginaFinal);
            if (Dts != null)
            {
                DgvListaFacturas1.DataSource = Dts.Tables[0];
                Paginado(Dts);
                Paginado_Habilitar(true);
            }
            else
            {
                Paginado_Habilitar(false);
            }

            if (DgvListaFacturas1.Rows.Count > 0)
            {
                DgvListaFacturas1.Visible = true;
                EstructuraGrid();
            }
            else
            {
                DgvListaFacturas1.Visible = false;

            }

        }

        private void FrmListaFactura_Load(object sender, EventArgs e)
        {
            this.DgvListaFacturas1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            this.DgvListaFacturas1.RowTemplate.Height = 30

                ;
            Dts = _L_ListaFacturas.TraerFacturasRango(DtpDesde, DtpHasta);
            if (Dts != null)
            {
                DgvListaFacturas1.DataSource = Dts.Tables[0];
                Paginado(Dts);
                Paginado_Habilitar(true);
            }
            else
            {
                Paginado_Habilitar(false);
            }


            if (DgvListaFacturas1.Rows.Count > 0)
            {
                DgvListaFacturas1.Visible = true;
                EstructuraGrid();
            }
            else
            {
                DgvListaFacturas1.Visible = false;
            }

            DgvListaFacturas1.Invalidate();

        }

        private void DgvListaFacturas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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

        private void button1_Click(object sender, EventArgs e)
        {
            uExportar.exportToExcel(Dts, true);
        }

        private void DtpHasta_ValueChanged(object sender, EventArgs e)
        {

        }

        public void EstructuraGrid()
        {
            //Con esta funcion se le da la estructura a las columnas del grid 
            try
            {

                //Centrar todas las columnas 
                DgvListaFacturas1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


                Padding newPadding = new Padding(15, 15, 15, 15);
                DgvListaFacturas1.ColumnHeadersDefaultCellStyle.Padding = newPadding;


                //asignar Nombres a cada columna 
                DgvListaFacturas1.Columns["NumeroFactura"].HeaderText = "N° de Factura";
                DgvListaFacturas1.Columns["CedulaCliente"].HeaderText = "N° de Cédula";
                DgvListaFacturas1.Columns["FactSub"].HeaderText = "Sub Total";
                DgvListaFacturas1.Columns["FactImpuesto"].HeaderText = "Impuesto";
                DgvListaFacturas1.Columns["FactIGTF"].HeaderText = "IGTF";
                DgvListaFacturas1.Columns["FactTotal"].HeaderText = "Total";


                //Ancho de columna
                DgvListaFacturas1.Columns["NumeroFactura"].Width = 110;
                DgvListaFacturas1.Columns["CedulaCliente"].Width = 110;
                DgvListaFacturas1.Columns["FactSub"].Width = 110;
                DgvListaFacturas1.Columns["FactImpuesto"].Width = 110;
                DgvListaFacturas1.Columns["FactIGTF"].Width = 110;
                DgvListaFacturas1.Columns["FactTotal"].Width = 110;


                //Bloquear Columna 
                DgvListaFacturas1.Columns["NumeroFactura"].ReadOnly = true;
                DgvListaFacturas1.Columns["CedulaCliente"].ReadOnly = true;
                DgvListaFacturas1.Columns["FactSub"].ReadOnly = true;
                DgvListaFacturas1.Columns["FactImpuesto"].ReadOnly = true;
                DgvListaFacturas1.Columns["FactIGTF"].ReadOnly = true;
                DgvListaFacturas1.Columns["FactTotal"].ReadOnly = true;


                //ordenar las colunmnas del grid 
                DgvListaFacturas1.Columns["NumeroFactura"].DisplayIndex = 0;
                DgvListaFacturas1.Columns["CedulaCliente"].DisplayIndex = 1;
                DgvListaFacturas1.Columns["FactSub"].DisplayIndex = 2;
                DgvListaFacturas1.Columns["FactImpuesto"].DisplayIndex = 3;
                DgvListaFacturas1.Columns["FactIGTF"].DisplayIndex = 4;
                DgvListaFacturas1.Columns["FactTotal"].DisplayIndex = 5;

                DgvListaFacturas1.Columns["Numero"].Visible = false;


                DgvListaFacturas1.Columns["NumeroFactura"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvListaFacturas1.Columns["CedulaCliente"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvListaFacturas1.Columns["FactSub"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvListaFacturas1.Columns["FactImpuesto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvListaFacturas1.Columns["FactIGTF"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvListaFacturas1.Columns["FactTotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // nuevo 21-08-2023 
                DgvListaFacturas1.Columns["FactSub"].DefaultCellStyle.Format = "##,##0.00";
                DgvListaFacturas1.Columns["FactImpuesto"].DefaultCellStyle.Format = "##,##0.00";
                DgvListaFacturas1.Columns["FactIGTF"].DefaultCellStyle.Format = "##,##0.00";
                DgvListaFacturas1.Columns["FactTotal"].DefaultCellStyle.Format = "##,##0.00";
                
                //// Deshabilitar el ajuste automático de la altura de las filas
                //DgvListaFacturas.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                //// Establecer la altura de las filas
                //this.DgvListaFacturas1.RowTemplate.Height = 50;

            }


            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }
        }

        public void FormatoDataGrid_Oscuro_ListaFact(System.Drawing.Color col2, System.Drawing.Color col3, System.Drawing.Color col4)
        {

            this.BackColor = col2;
            LblListadoFactura.ForeColor = Color.White;
            //Lbldesde.ForeColor = Color.White;
            //LblHasta.ForeColor = Color.White;

            //Con esta funcion coloreamos el grid del color oscuro 
            DgvListaFacturas1.BackgroundColor = col3;
            DgvListaFacturas1.DefaultCellStyle.BackColor = col3;
            DgvListaFacturas1.ColumnHeadersDefaultCellStyle.BackColor = col4;
            DgvListaFacturas1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DgvListaFacturas1.DefaultCellStyle.ForeColor = Color.White;
          
        }

        public void FormatoDataGrid_Claro_ListaFact(System.Drawing.Color col1, System.Drawing.Color col3)
        {

            this.BackColor = col1;
            LblListadoFactura.ForeColor = Color.Black;
            //Lbldesde.ForeColor = Color.Black;
            //LblHasta.ForeColor = Color.Black;
           
            //Con esta funcion coloreamos el grid del fondo blanco 
            DgvListaFacturas1.BackgroundColor = col1;
            DgvListaFacturas1.DefaultCellStyle.BackColor = col1;
            DgvListaFacturas1.ColumnHeadersDefaultCellStyle.BackColor = col3;
            DgvListaFacturas1.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(30, 30, 30, 30);
            DgvListaFacturas1.DefaultCellStyle.ForeColor = Color.Black;

        }

        private void CbxEstatus_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (CbxEstatus.SelectedIndex  == 0)
            {
                Dts = _L_ListaFacturas.TraerFacturasRango(DtpDesde, DtpHasta);
               
            }
            else
            {
                Dts = _L_ListaFacturas.TraerNotasRango(DtpDesde, DtpHasta);
            }

            if (Dts != null)
            {
                DgvListaFacturas1.DataSource = Dts.Tables[0];
                Paginado(Dts);
                Paginado_Habilitar(true);
            }
            else
            {
                Paginado_Habilitar(false);
            }


            if (DgvListaFacturas1.Rows.Count > 0)
            {
                DgvListaFacturas1.Visible = true;
                EstructuraGrid();
            }
            else
            {
                DgvListaFacturas1.Visible = false;
            }
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
    }
}
