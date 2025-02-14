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

namespace CapaVisual_Login
{
    public partial class FrmListaFactura : Form
    {
        public FrmListaFactura()
        {
            InitializeComponent();
        }

        private L_ListaOrdenes _ListaOrdenes = new L_ListaOrdenes();
        private L_ListaFacturas _L_ListaFacturas = new L_ListaFacturas();
        int PaginaInico = 1, Indice = 0, NUmeroFilas = 12, PaginaFinal;

        private void Btnlupa_Click(object sender, EventArgs e)
        {
               LimpiarGrid();

                DataSet Dts = _L_ListaFacturas.TraerFacturasRango(DtpDesde, DtpHasta);
                if (Dts != null)
                {
                    DgvListaFacturas.DataSource = Dts.Tables[0];
                    Paginado(Dts);
                    Paginado_Habilitar(true);
                }
                else
                {
                    Paginado_Habilitar(false);
                }

                if (DgvListaFacturas.Rows.Count > 0)
                {
                    DgvListaFacturas.Visible = true;
                    EstructuraGrid();


                }
                else
                {
                    DgvListaFacturas.Visible = false;

                }

            
        }

        private void LimpiarGrid()
        {
            try
            {
                //limpiar el grid 

                DgvListaFacturas.DataSource = "";
                DgvListaFacturas.DataMember = "";
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

            DataSet Dts = _L_ListaFacturas.TraerFacturasRango(DtpDesde, DtpHasta, PaginaInico, PaginaFinal);
            if (Dts != null)
            {
                DgvListaFacturas.DataSource = Dts.Tables[0];
                Paginado(Dts);
                Paginado_Habilitar(true);
            }
            else
            {
                Paginado_Habilitar(false);
            }

            if (DgvListaFacturas.Rows.Count > 0)
            {
                DgvListaFacturas.Visible = true;
                EstructuraGrid();
            }
            else
            {
                DgvListaFacturas.Visible = false;

            }

        }

        private void FrmListaFactura_Load(object sender, EventArgs e)
        {
            DataSet Dts= _L_ListaFacturas.TraerFacturasRango(DtpDesde, DtpHasta);
            if (Dts != null)
            {
                DgvListaFacturas.DataSource = Dts.Tables[0];
                Paginado(Dts);
                Paginado_Habilitar(true);
            }
            else
            {
                Paginado_Habilitar(false);
            }


            if (DgvListaFacturas.Rows.Count > 0)
            {
                DgvListaFacturas.Visible = true;
                EstructuraGrid();
            }
            else
            {
                DgvListaFacturas.Visible = false;
            }

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


        public void EstructuraGrid()
        {
            //Con esta funcion se le da la estructura a las columnas del grid 
            try
            {

                //Centrar todas las columnas 
                DgvListaFacturas.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


                Padding newPadding = new Padding(15, 15, 15, 15);
                DgvListaFacturas.ColumnHeadersDefaultCellStyle.Padding = newPadding;


                //asignar Nombres a cada columna 
                DgvListaFacturas.Columns["NumeroFactura"].HeaderText = "N° de Factura";
                DgvListaFacturas.Columns["CedulaCliente"].HeaderText = "N° de Cedula";
                DgvListaFacturas.Columns["FactSub"].HeaderText = "Sub Total";
                DgvListaFacturas.Columns["FactImpuesto"].HeaderText = "Impuesto";
                DgvListaFacturas.Columns["FactIGTF"].HeaderText = "IGTF";
                DgvListaFacturas.Columns["FactTotal"].HeaderText = "Total";


                //Ancho de columna
                DgvListaFacturas.Columns["NumeroFactura"].Width = 110;
                DgvListaFacturas.Columns["CedulaCliente"].Width = 110;
                DgvListaFacturas.Columns["FactSub"].Width = 110;
                DgvListaFacturas.Columns["FactImpuesto"].Width = 110;
                DgvListaFacturas.Columns["FactIGTF"].Width = 110;
                DgvListaFacturas.Columns["FactTotal"].Width = 110;


                //Bloquear Columna 
                DgvListaFacturas.Columns["NumeroFactura"].ReadOnly = true;
                DgvListaFacturas.Columns["CedulaCliente"].ReadOnly = true;
                DgvListaFacturas.Columns["FactSub"].ReadOnly = true;
                DgvListaFacturas.Columns["FactImpuesto"].ReadOnly = true;
                DgvListaFacturas.Columns["FactIGTF"].ReadOnly = true;
                DgvListaFacturas.Columns["FactTotal"].ReadOnly = true;


                //ordenar las colunmnas del grid 
                DgvListaFacturas.Columns["NumeroFactura"].DisplayIndex = 0;
                DgvListaFacturas.Columns["CedulaCliente"].DisplayIndex = 1;
                DgvListaFacturas.Columns["FactSub"].DisplayIndex = 2;
                DgvListaFacturas.Columns["FactImpuesto"].DisplayIndex = 3;
                DgvListaFacturas.Columns["FactIGTF"].DisplayIndex = 4;
                DgvListaFacturas.Columns["FactTotal"].DisplayIndex = 5;

                DgvListaFacturas.Columns["Numero"].Visible = false;


                DgvListaFacturas.Columns["NumeroFactura"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvListaFacturas.Columns["CedulaCliente"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvListaFacturas.Columns["FactSub"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvListaFacturas.Columns["FactImpuesto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvListaFacturas.Columns["FactIGTF"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvListaFacturas.Columns["FactTotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // nuevo 21-08-2023 
                DgvListaFacturas.Columns["FactSub"].DefaultCellStyle.Format = "##,##0.00";
                DgvListaFacturas.Columns["FactImpuesto"].DefaultCellStyle.Format = "##,##0.00";
                DgvListaFacturas.Columns["FactIGTF"].DefaultCellStyle.Format = "##,##0.00";
                DgvListaFacturas.Columns["FactTotal"].DefaultCellStyle.Format = "##,##0.00";
                
                //// Deshabilitar el ajuste automático de la altura de las filas
                //DgvListaFacturas.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                //// Establecer la altura de las filas
                //this.DgvListaFacturas.RowTemplate.Height = 26;

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
            Lbldesde.ForeColor = Color.White;
            LblHasta.ForeColor = Color.White;

            //Con esta funcion coloreamos el grid del color oscuro 
            DgvListaFacturas.BackgroundColor = col3;
            DgvListaFacturas.DefaultCellStyle.BackColor = col3;
            DgvListaFacturas.ColumnHeadersDefaultCellStyle.BackColor = col4;
            DgvListaFacturas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DgvListaFacturas.DefaultCellStyle.ForeColor = Color.White;
          
        }

        public void FormatoDataGrid_Claro_ListaFact(System.Drawing.Color col1, System.Drawing.Color col3)
        {

            this.BackColor = col1;
            LblListadoFactura.ForeColor = Color.Black;
            Lbldesde.ForeColor = Color.Black;
            LblHasta.ForeColor = Color.Black;
           
            //Con esta funcion coloreamos el grid del fondo blanco 
            DgvListaFacturas.BackgroundColor = col1;
            DgvListaFacturas.DefaultCellStyle.BackColor = col1;
            DgvListaFacturas.ColumnHeadersDefaultCellStyle.BackColor = col3;
            DgvListaFacturas.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(30, 30, 30, 30);
            DgvListaFacturas.DefaultCellStyle.ForeColor = Color.Black;

        }

    }
}
