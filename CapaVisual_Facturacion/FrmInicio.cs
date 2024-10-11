using CapaEntidades;
using CapaVisual_Externa;
using CapaLogica.Inicio_Logica;
using CapaDatos.Inicio_Datos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace CapaVisual_Facturacion
{
    public partial class FrmInicio : Form
    {
        //Instanciamos nuestra clase L_Inicio para poder utilizar sus miembros
        L_Inicio _L_Inicio = new L_Inicio();
        
        List_TB_CAORDSER _CAORDSER = new List_TB_CAORDSER();
        D_Inicio _D_Inicio = new D_Inicio();
        FrmMensajes _FrmMensajes = new FrmMensajes();



        public FrmInicio()
        {
     
            InitializeComponent();
        }

        private void FrmInicio_Load(object sender, EventArgs e)
        {
            _L_Inicio.CargarTexbox(TxtTasaDia, TxtVentasDia);
            MostarOrdenes();
            Clientes();


            //cristgraf = 150;
            //montgraf = 200;
            //Graphics torta = CreateGraphics();
            //int total = cristgraf + montgraf;
            //int grado1 = cristgraf * 360 / total;
            //int grado2 = montgraf * 360 / total;
            //torta.FillPie(new SolidBrush(Color.Aqua), 10, 10, 400, 400, 0, grado1);
            //torta.FillPie(new SolidBrush(Color.White), 10, 10, 400, 400, grado1, grado2);

            //int totalcrist;
            //int totalmont;
            _D_Inicio.GraficoVentas("20220808", "07165");
            _D_Inicio.GraficoVentasDet("20220808", "07165");
            _D_Inicio.GraficoVentasMontDet("20220920", "07165");
            _D_Inicio.leyendaCrist("20220808", "07165");
            _D_Inicio.leyendaMont("20220920", "07165");
            //totalcrist = Convert.ToInt32(_D_Inicio.VentaCristDia);
            //totalmont = Convert.ToInt32(_D_Inicio.VentaMontDia);
            //string[] series = { "Cristales", "Montura" };
            //int[] puntos = { totalcrist, totalmont };
            Grafico1.Series["Series1"].Points.DataBindXY(_D_Inicio.NombreC, _D_Inicio.CantidadC);
            
            for (int i = 0; i < _D_Inicio.contC; i++)
            {
                string leyenC = (string)_D_Inicio.NombreLeyC[i];
                Grafico1.Series["Series1"].Points[i].LegendText = leyenC;
              
            }
            Grafico2.Series["Series1"].Points.DataBindXY(_D_Inicio.NombreM, _D_Inicio.CantidadM);
            for (int i = 0; i < _D_Inicio.contM; i++)
            {
                string leyenM = (string)_D_Inicio.TotalLeyM[i];
                Grafico2.Series["Series1"].Points[i].LegendText = leyenM;

            }



            //for (int i = 0; i < series.Length; i++)
            //{
            //    //Series serie = chart1.Series.Add(series[i]);

            //    //serie.Label = puntos[i].ToString();
            //    //serie.Points.Add(puntos[i]);
            //    Grafico1.Series["Series1"].Points.AddXY(series[i], puntos[i]);
            //}



            //TxtBsCrist.Text = "Total Cristales BS: " + _D_Inicio.TotalBsCrist;
            //TxtUSDCrist.Text = "Total Cristales $: " + _D_Inicio.TotalUSDCrist;
            //TxtBsMont.Text = "Total Montura BS: " + _D_Inicio.TotalBsMont;
            //textBox1.Text = "Total Montura $: " + _D_Inicio.TotalUSDMont;





            //string[] series2 = { "Monturas", "Cristales"};
            //int[] puntos2 = { 23, 70 };


            //for (int i = 0; i < series2.Length; i++)
            //{
            //    Series serie = Grafico2.Series.Add(series2[i]);

            //    serie.Label = puntos2[i].ToString();
            //    serie.Points.Add(puntos2[i]);

            // }


            //string[] series = { "Monturas", "Cristales" };
            //    int[ ] puntos  = { 55, 80 };


            //for (int i = 0; i < series.Length; i++)
            //{
            //    //Series serie = chart1.Series.Add(series[i]);

            //    //serie.Label = puntos[i].ToString();
            //    //serie.Points.Add(puntos[i]);
            //    Grafico1.Series["Series1"].Points.AddXY(series[i], puntos[i]);
            //}
        }


        private void FormatoDataGridOrdenes()
        {
            try
            {
                //Centrar todas las colucnas 
                DgvUltimasVentas.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvUltimasVentas.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                
                
               

                //asignar Nombres a cada colucna 
                DgvUltimasVentas.Columns["Orden"].HeaderText = "N° de Orden";
                DgvUltimasVentas.Columns["Hora"].HeaderText = "Hora";
                DgvUltimasVentas.Columns["Total"].HeaderText = "Total";

                //Ancho de columna
                DgvUltimasVentas.Columns["Orden"].Width = 110;
                DgvUltimasVentas.Columns["Hora"].Width = 130;
                DgvUltimasVentas.Columns["Total"].Width = 100;

                //Bloquear Columna 
                DgvUltimasVentas.Columns["Orden"].ReadOnly = true;
                DgvUltimasVentas.Columns["Hora"].ReadOnly = true;
                DgvUltimasVentas.Columns["Total"].ReadOnly = true;
                DgvUltimasVentas.Columns["Orden"].SortMode = DataGridViewColumnSortMode.NotSortable;
                DgvUltimasVentas.Columns["Hora"].SortMode = DataGridViewColumnSortMode.NotSortable;
                DgvUltimasVentas.Columns["Total"].SortMode = DataGridViewColumnSortMode.NotSortable;



                //ordenar las colunmnas del grid 
                DgvUltimasVentas.Columns["Orden"].DisplayIndex = 0;
                DgvUltimasVentas.Columns["Hora"].DisplayIndex = 1;
                DgvUltimasVentas.Columns["Total"].DisplayIndex = 2;

            }


            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }
        }


        private void FormatoDataGridListaEspera()
        {
        
            try
            {
                DgvClientes.Columns["Cliente"].SortMode = DataGridViewColumnSortMode.NotSortable;
                DgvClientes.Columns["Cédula"].SortMode = DataGridViewColumnSortMode.NotSortable;



                DataGridViewButtonColumn Btnclm = new DataGridViewButtonColumn();


                Btnclm.HeaderText = "Borrar";
                Btnclm.Name = "Eliminar";
                Btnclm.UseColumnTextForButtonValue = true;
                DgvClientes.Columns.Add(Btnclm);

            }


            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }
        }




        private void MostarOrdenes()
        {
            try
            {
                DgvUltimasVentas.DataSource = "";
                DgvUltimasVentas.DataMember = "";
                DgvUltimasVentas.DataSource = _L_Inicio.CargarOrdenes();
                FormatoDataGridOrdenes();

            }


            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }
        }


        private void Clientes()
        {
            try
            {
                DgvClientes.DataSource = "";
                DgvClientes.DataMember = "";
                DgvClientes.DataSource = _L_Inicio.CargarClientes();
                FormatoDataGridListaEspera();
            }


            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }
        }

        private void DgvClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            _FrmMensajes.co = 3;
            string mensaje = "Estas seguro de elminar este cliente";

            if (DgvClientes.Columns[e.ColumnIndex].Name == "Eliminar")
            {
                _FrmMensajes.avisomensaje(mensaje);
                _FrmMensajes.ShowDialog();
                if (_FrmMensajes.DialogResult == DialogResult.OK)
                {
                    string contenido = Convert.ToString(DgvClientes.CurrentRow.Cells[2].Value);
                    _L_Inicio.BorrarClienteListaEspera(contenido);
                    DgvClientes.Rows.Remove(DgvClientes.CurrentRow);

                }
            }
        }

        private void GbxVentasSemana_Enter(object sender, EventArgs e)
        {

        }

        private void GbxVentasDia_Enter(object sender, EventArgs e)
        {

        }

        private void Grafico1_Click(object sender, EventArgs e)
        {

        }

        private void TxtUSDCrist_TextChanged(object sender, EventArgs e)
        {

        }

        private void DgvClientes_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && this.DgvClientes.Columns[e.ColumnIndex].Name == "Eliminar" && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                DataGridViewButtonCell celBoton = this.DgvClientes.Rows[e.RowIndex].Cells["Eliminar"] as DataGridViewButtonCell;
                Icon icoAtomico = new Icon(Environment.CurrentDirectory + @"\\TRASHV.ico");
                e.Graphics.DrawIcon(icoAtomico, e.CellBounds.Left + 1, e.CellBounds.Top + 0);
                

                this.DgvClientes.Rows[e.RowIndex].Height = icoAtomico.Height + 0;
                this.DgvClientes.Columns[e.ColumnIndex].Width = icoAtomico.Width + 2;
                
                e.Handled = true;


            }
        }

        private void TxtBsMont_TextChanged(object sender, EventArgs e)
        {

        }

        private void DgvClientes_Paint(object sender, PaintEventArgs e)
        {
            DataGridViewButtonCell Btncelda = new DataGridViewButtonCell();
     

        }

        private void TxtBsCrist_TextChanged(object sender, EventArgs e)
        {

        }

        private void Grafico2_Click(object sender, EventArgs e)
        {

        }

        private void TxtVentasDia_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

