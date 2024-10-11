using CapaLogica.ListaOrden_Logica;
using System;
using CapaVisual_Facturacion;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CapaVisual_Facturacion
{
    public partial class FrmListaOrdenes : Form
        
    {
        string orden = "";
        public FrmListaOrdenes()
        
        {
            InitializeComponent();
        }

        //Instanciamos nuestra clase D_Loguin para poder utilizar sus miembros
        private L_ListaOrdenes _ListaOrdenes = new L_ListaOrdenes();
        FrmInicio _FrmInicio = new FrmInicio();
        private FrmFacturacion _FrmFacturacion = new FrmFacturacion();
        public DataTable dt = new DataTable("CAORDSER");

        private void FrmListaOrdenes_Load(object sender, EventArgs e)
        {
            //EstructuraGrid();

            _ListaOrdenes.LLenarCombobox(CbxUltimosTesD, CbxEstatus,DtpDesde);
            PnlLSecundario.Visible = false;
            PnlLSecundario.Enabled = false;
            BtnCancelar.Visible = false;
            DtpDesde.Visible = false;
            DtpHasta.Visible = false;
            Lbldesde.Visible = false;
            LblHasta.Visible = false;
            //EstructuraGrid();
           
          
         

            CbxUltimosTesD.SelectedIndex = 1;
            DgvListadoOrdenes.DataSource = _ListaOrdenes.TraerOrdenes(CbxUltimosTesD, CbxEstatus, txtNumeroOrden);
            if (DgvListadoOrdenes.Rows.Count > 0)
            {
             
                CrearObjetos();
                ColorearStatus();

            }

            txtNumeroOrden.Text = "N° de orden";
            txtNumeroOrden.ForeColor = Color.LightGray;
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



                //asignar Nombres a cada columna 
                DgvListadoOrdenes.Columns["NumOrdserv"].HeaderText = "N° de orden";
                DgvListadoOrdenes.Columns["Nombre"].HeaderText = "Nombre";
                DgvListadoOrdenes.Columns["Estatus"].HeaderText = "Estatus";
                DgvListadoOrdenes.Columns["Monto"].HeaderText = "Monto";
                DgvListadoOrdenes.Columns["Ref"].HeaderText = "Ref";


                //Ancho de columna
                DgvListadoOrdenes.Columns["NumOrdserv"].Width = 110;
                DgvListadoOrdenes.Columns["Nombre"].Width = 150;
                DgvListadoOrdenes.Columns["Estatus"].Width = 150;
                DgvListadoOrdenes.Columns["Monto"].Width = 100;
                DgvListadoOrdenes.Columns["Ref"].Width = 100;
                DgvListadoOrdenes.Columns["CbnOpciones"].Width = 150;
                DgvListadoOrdenes.Columns["Btn"].Width = 80;

                //Bloquear Columna 

                DgvListadoOrdenes.Columns["NumOrdserv"].ReadOnly = true;
                DgvListadoOrdenes.Columns["Nombre"].ReadOnly = true;
                DgvListadoOrdenes.Columns["Estatus"].ReadOnly = true;
                DgvListadoOrdenes.Columns["Monto"].ReadOnly = true;
                DgvListadoOrdenes.Columns["Ref"].ReadOnly = true;
                DgvListadoOrdenes.Columns["CbnOpciones"].ReadOnly = false;
                DgvListadoOrdenes.Columns["Btn"].ReadOnly = false;

                //ordenar las colunmnas del grid 
                DgvListadoOrdenes.Columns["NumOrdserv"].DisplayIndex = 0;
                DgvListadoOrdenes.Columns["Nombre"].DisplayIndex = 1;
                DgvListadoOrdenes.Columns["Estatus"].DisplayIndex = 2;
                DgvListadoOrdenes.Columns["Monto"].DisplayIndex = 3;
                DgvListadoOrdenes.Columns["Ref"].DisplayIndex = 4;
                DgvListadoOrdenes.Columns["CbnOpciones"].DisplayIndex = 5;
                DgvListadoOrdenes.Columns["Btn"].DisplayIndex = 6;

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
            //Con esta funcion coloreamos el grid del fondo blanco 
            DgvListadoOrdenes.BackgroundColor = col1;
            DgvListadoOrdenes.DefaultCellStyle.BackColor = col1;
            DgvListadoOrdenes.ColumnHeadersDefaultCellStyle.BackColor = col3;
            DgvListadoOrdenes.ColumnHeadersDefaultCellStyle.ForeColor = Color.Gray;
            DgvListadoOrdenes.DefaultCellStyle.ForeColor = Color.Black;
            //EstructuraGrid();

        }

        public void FormatoDataGrid2(System.Drawing.Color col2, System.Drawing.Color col3, System.Drawing.Color col4)
        {
            this.BackColor = col2;
            //Con esta funcion coloreamos el grid del color oscuro 
            DgvListadoOrdenes.BackgroundColor = col3;
            DgvListadoOrdenes.DefaultCellStyle.BackColor = col3;
            DgvListadoOrdenes.ColumnHeadersDefaultCellStyle.BackColor = col4;
            DgvListadoOrdenes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DgvListadoOrdenes.DefaultCellStyle.ForeColor = Color.White;

        }


        private void CrearObjetos()
        {
            try
            {

                DataGridViewComboBoxColumn columnCbn = new DataGridViewComboBoxColumn();
                columnCbn.HeaderText = "Opciones";
                columnCbn.Name = "CbnOpciones";
                columnCbn.DataSource = _ListaOrdenes.LLenarComboboxOpciones();
                columnCbn.DisplayMember = "Value";
                columnCbn.ValueMember = "Index";
                columnCbn.SortMode = DataGridViewColumnSortMode.Automatic;

                DgvListadoOrdenes.Columns.Add(columnCbn);

                DataGridViewButtonColumn columnBtn = new DataGridViewButtonColumn();
                columnBtn.Name = "Btn";
                columnBtn.Width = 50;
                columnBtn.HeaderText = "";
                DgvListadoOrdenes.Columns.Add(columnBtn);

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


            try
            {


                foreach (DataGridViewRow Fila in DgvListadoOrdenes.Rows)
                {
                    string Status = Fila.Cells["Estatus"].Value.ToString();
                    if (Status == "Facturada")
                    {
                        Fila.Cells["Estatus"].Style.BackColor = colfact;
                    }

                    if (Status == "Anulada  ")
                    {
                        Fila.Cells["Estatus"].Style.BackColor = colanul;
                    }

                    if (Status == "Por pagar")
                    {
                        Fila.Cells["Estatus"].Style.BackColor = colporp;

                    }

                    if (Status == "Abonada  ") 
                    {
                        Fila.Cells["Estatus"].Style.BackColor = colabon;
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

                var dataGridViewColumn = DgvListadoOrdenes.Columns["CbnOpciones"];
                var dataGridViewColumn2 = DgvListadoOrdenes.Columns["Btn"];

                if (dataGridViewColumn != null && dataGridViewColumn.Visible)

                {

                    DgvListadoOrdenes.Columns.RemoveAt(DgvListadoOrdenes.Columns.Count - 1);
                }

                if (dataGridViewColumn2 != null && dataGridViewColumn2.Visible)

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
 
            if (CbxUltimosTesD.SelectionLength > 0)
            {

                if (CbxUltimosTesD.SelectedIndex == 5)
                {
                    LimpiarGrid();
                    DtpDesde.Visible = true;
                    DtpHasta.Visible = true;
                    Lbldesde.Visible = true;
                    LblHasta.Visible = true;



                


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


                        DgvListadoOrdenes.DataSource = _ListaOrdenes.TraerOrdenes(CbxUltimosTesD, CbxEstatus, txtNumeroOrden);
                        if (DgvListadoOrdenes.Rows.Count > 0)
                        {
                            CrearObjetos();
                            ColorearStatus();
                        }
                    }
                }
            }

        }

        private void CbxEstatus_SelectionChangeCommitted(object sender, EventArgs e)
        {
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

                        DgvListadoOrdenes.DataSource = _ListaOrdenes.TraerOrdenes(CbxUltimosTesD, CbxEstatus, txtNumeroOrden);
                        if (DgvListadoOrdenes.Rows.Count > 0)
                        {
                            CrearObjetos();
                            ColorearStatus();
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
                Icon IconAtomico = new Icon(Environment.CurrentDirectory + @"\\book.ico");
                e.Graphics.DrawIcon(IconAtomico, e.CellBounds.Left + 3, e.CellBounds.Top + 3);
             

                this.DgvListadoOrdenes.Rows[e.RowIndex].Height = IconAtomico.Height + 8;
                this.DgvListadoOrdenes.Columns[e.ColumnIndex].Width = IconAtomico.Width + 8;

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

            if (CbxUltimosTesD.SelectedIndex == 5)
            {

                DgvListadoOrdenes.DataSource = _ListaOrdenes.TraerOrdporRango(DtpDesde, DtpHasta, CbxEstatus);
                if (DgvListadoOrdenes.Rows.Count > 0)
                {

                    CrearObjetos();
                    ColorearStatus();
                }
            }
            else
            {
                DgvListadoOrdenes.DataSource = _ListaOrdenes.TraerOrdenes(CbxUltimosTesD, CbxEstatus, txtNumeroOrden);
                if (DgvListadoOrdenes.Rows.Count > 0)
                {
                    CrearObjetos();
                    ColorearStatus();
                }
            }
        }

        public void DgvListadoOrdenes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DgvListadoOrdenes.Columns[e.ColumnIndex].Name == "Btn")
            {
                PnlLSecundario.Visible = true;
                PnlLSecundario.Enabled = true;
                LblListadoOrdenes.Visible = false;
                CbxUltimosTesD.Visible = false;
                txtNumeroOrden.Visible = false;
                CbxEstatus.Visible = false;
                Btnlupa.Visible = false;
                DgvListadoOrdenes.Visible = false;
                PnlLSecundario.Visible = true;
                PnlLSecundario.Enabled = true;
                PnlLSecundario.Location = new Point(50, 50);
                BtnCancelar.Location = new Point(645, 420);
                BtnCancelar.Visible = true;
                PnlLSecundario.Controls.Clear();
                AbrirForm(_FrmFacturacion);
                _FrmFacturacion.CargarDatosOrden(DgvListadoOrdenes.CurrentRow.Cells["NumOrdserv"].Value.ToString());


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
            orden = txtNumeroOrden.Text;
            if (orden.Equals("N° de orden"))
            {
                txtNumeroOrden.Text = "N° de orden";
                txtNumeroOrden.ForeColor = Color.Gray;
                


            }
            else
            {
                if (orden.Equals(""))
                {
                    txtNumeroOrden.Text = "N° de orden";
                    txtNumeroOrden.ForeColor = Color.Gray;
                }
                else
                {
                    txtNumeroOrden.Text = orden;
                    txtNumeroOrden.ForeColor = Color.Black;

                }
            }
        }

        private void CbxUltimosTesD_Enter(object sender, EventArgs e)
        {
            orden = txtNumeroOrden.Text;
            if (orden.Equals("N° de orden"))
            {
                txtNumeroOrden.Text = "";
                txtNumeroOrden.ForeColor = Color.Gray;

            }
        }


        private void CbxEstatus_Enter(object sender, EventArgs e)
        {
            orden = txtNumeroOrden.Text;
            if (orden.Equals("N° de orden"))
            {
                txtNumeroOrden.Text = "";
                txtNumeroOrden.ForeColor = Color.Gray;
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {


            LblListadoOrdenes.Visible = true;
            CbxUltimosTesD.Visible = true;
            txtNumeroOrden.Visible = true;
            CbxEstatus.Visible = true;
            Btnlupa.Visible = true;
            DgvListadoOrdenes.Visible = true;


            PnlLSecundario.Visible = false;
            PnlLSecundario.Enabled = false;
            PnlLSecundario.Controls.Clear();
            BtnCancelar.Visible = false;
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
    }

}
