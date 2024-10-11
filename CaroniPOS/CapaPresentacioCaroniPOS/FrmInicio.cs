using CapaEntidades;
using CapaLogica.Inicio_Logica;
using CapaLogica.Configuracion_Logica;
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

namespace CapaVisual_Login
{
    public partial class FrmInicio : Form
    {
        //Instanciamos nuestra clase L_Inicio para poder utilizar sus miembros
        L_Inicio _L_Inicio = new L_Inicio();

        List_TB_CAORDSER _CAORDSER = new List_TB_CAORDSER();
        D_Inicio _D_Inicio = new D_Inicio();
        FrmMensajes _FrmMensajes = new FrmMensajes();
        L_Configuracion _L_Configuracion = new L_Configuracion();
        



        public FrmInicio()
        {

            InitializeComponent();
        }

        private void FrmInicio_Load(object sender, EventArgs e)
        {

           
            MostarOrdenes();
            Clientes();

            MostrarConfiguracion();
            ConfigInicio();
    

            


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
            //_D_Inicio.GraficoVentas("20220808", "07165");
            //_D_Inicio.GraficoVentasDet("148", "07165");
            //_D_Inicio.GraficoVentasMontDet("148", "07165");
            //_D_Inicio.leyendaCrist("148", "07165");
            //_D_Inicio.leyendaMont("148", "07165");
            ////totalcrist = Convert.ToInt32(_D_Inicio.VentaCristDia);
            ////totalmont = Convert.ToInt32(_D_Inicio.VentaMontDia);
            ////string[] series = { "Cristales", "Montura" };
            ////int[] puntos = { totalcrist, totalmont };
            //Grafico1.Series["Series1"].Points.DataBindXY(_D_Inicio.NombreC, _D_Inicio.CantidadC);

            //for (int i = 0; i < _D_Inicio.contC; i++)
            //{
            //    string leyenC = (string)_D_Inicio.NombreLeyC[i];
            //    Grafico1.Series["Series1"].Points[i].LegendText = leyenC;

            //}
            //Grafico2.Series["Series1"].Points.DataBindXY(_D_Inicio.NombreM, _D_Inicio.CantidadM);
            //for (int i = 0; i < _D_Inicio.contM; i++)
            //{
            //    string leyenM = (string)_D_Inicio.TotalLeyM[i];
            //    Grafico2.Series["Series1"].Points[i].LegendText = leyenM;

            //}



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

        public void CrearGrafico(string parametro,string codempleado, string CodSuc)
        {
            _D_Inicio.GraficoVentas("20220808", "07165");
            _D_Inicio.GraficoVentasDet(parametro, codempleado,CodSuc);
            _D_Inicio.GraficoVentasMontDet(parametro, codempleado, CodSuc);
            _D_Inicio.leyendaCrist(parametro, codempleado,CodSuc);
            _D_Inicio.leyendaMont(parametro, codempleado, CodSuc);

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

        }


        public void CrearGraficoPorRango(string parametro1,string parametro2, string codempleado, string CodSuc)
        {
          
            _D_Inicio.GraficoVentasDetRango(parametro1,parametro2, codempleado, CodSuc);
            _D_Inicio.GraficoVentasMontDetRango(parametro1, parametro2, codempleado, CodSuc);
            _D_Inicio.LeyendCristRango(parametro1, parametro2, codempleado, CodSuc);
            _D_Inicio.LeyendMontRango(parametro1, parametro2, codempleado, CodSuc);

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

        }

        public void CrearGraficoMetas(string Periodo, string CodEmpleado)
        {

            _D_Inicio.GraficoMetasUds(Periodo, CodEmpleado);
            _D_Inicio.GraficoMetasIng(Periodo, CodEmpleado);
            Grafico3.Series["Series1"].Points.DataBindXY(_D_Inicio.LeyMetUds,_D_Inicio.DatosMetUds);
            Grafico4.Series["Series1"].Points.DataBindXY(_D_Inicio.LeyMetBs, _D_Inicio.DatosMetBs);


        }

        public void limpiargrafico()
        {
            LblTasaDia.Text = " aja "; 


            //foreach (var series in Grafico1.Series)
            //{
            //    series.Points.Clear();
            //}

            //foreach (var series in Grafico2.Series)
            //{
            //    series.Points.Clear();
            //}
            //Grafico1.Visible = false;
            //Grafico2.Visible = false; 
            //_D_Inicio.GraficoVentasDet(" ", " ");
            //_D_Inicio.GraficoVentasMontDet(" ", " ");
            //_D_Inicio.leyendaCrist(" ", " ");
            //_D_Inicio.leyendaMont(" ", " ");

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


        public void MostrarConfiguracion()
        {
            string CodEmpleado = TB_USUARIO.COD_EMPLEADO;
            _L_Inicio.CargarConfiguracion(CodEmpleado);
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

        public void Grafico1_Click(object sender, EventArgs e)
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


        public void ConfigInicio()
        {
            if (_L_Inicio.MonedaTasa == "USD")
            {
                _L_Inicio.CargarTexboxDol();
                TxtTasaDia.Text = _L_Inicio.TasaDia;
                LblBs.Text = "BS/$";
            }
            if (_L_Inicio.MonedaTasa == "Euros")
            {
                _L_Inicio.CargarTexboxEur();
                TxtTasaDia.Text = _L_Inicio.TasaDia;
              
                LblBs.Text = "BS/€";

            }

            if (_L_Inicio.MonedaVentas == "Bs")
            {

                _L_Inicio.CargarVentasDiasBs();
                TxtVentasDia.Text = _L_Inicio.VentaDia;
                LblMonedaVenta.Text = "Bs";

            }

            if (_L_Inicio.MonedaVentas == "USD")
            {

                _L_Inicio.CargarVentasDiasDol();
                TxtVentasDia.Text = _L_Inicio.VentaDia;
                LblMonedaVenta.Text = "$";

            }


            if (_L_Inicio.ConfigDefault == "Default")
            {

                Grafico3.Visible = false;
                Grafico4.Visible = false;
                CrearGrafico("150", "", "013");
                _L_Inicio.CargarTexboxDol();
                TxtTasaDia.Text = _L_Inicio.TasaDia;
                LblBs.Text = "BS/$";

                _L_Inicio.CargarVentasDiasDol();
                TxtVentasDia.Text = _L_Inicio.VentaDia;
                LblMonedaVenta.Text = "$";



            }



            else
            {
                if (_L_Inicio.TipoGrafico == "Metas")
                {
                    Grafico1.Visible = false;
                    Grafico2.Visible = false;
                    Grafico3.Visible = true;
                    Grafico4.Visible = true;
                    string par = _L_Inicio.PeriodoMeta + "/01";

                    CrearGraficoMetas(par, TB_USUARIO.COD_EMPLEADO);

                }
                else
                {
                    Grafico1.Visible = true;
                    Grafico2.Visible = true;
                    Grafico3.Visible = false;
                    Grafico4.Visible = false;

                    if (_L_Inicio.PeriodoGraf.Length <= 3)// Validamos  si el periodo configurad es por dias
                    {
                        if (_L_Inicio.InfoGraf.Length == 5) // Validamos si la informacion que se muestra es por usario tambien funciona para mostrart colaborador
                        {
                            CrearGrafico(_L_Inicio.PeriodoGraf, _L_Inicio.InfoGraf, ""); // estamos colocando como codigoemplead 07165 porque el de logeo no tiene ventas 
                        }

                        if (_L_Inicio.InfoGraf.Length <= 3) // Validamos si la informacion que se muestra es por sucursal
                        {
                            CrearGrafico(_L_Inicio.PeriodoGraf, "", _L_Inicio.InfoGraf);
                        }
                    }
                    else
                    {
                        if (_L_Inicio.InfoGraf.Length == 5) // Validamos si la informacion que se muestra es por usario tambien funciona para mostrart colaborador
                        {
                            CrearGraficoPorRango(_L_Inicio.PeriodoGraf,_L_Inicio.Valor2graf, _L_Inicio.InfoGraf, ""); // estamos colocando como codigoemplead 07165 porque el de logeo no tiene ventas 
                        }

                        if (_L_Inicio.InfoGraf.Length <= 3) // Validamos si la informacion que se muestra es por sucursal
                        {
                            CrearGraficoPorRango(_L_Inicio.PeriodoGraf, _L_Inicio.Valor2graf,"", _L_Inicio.InfoGraf);
                        }

                    }
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void LblBs_Click(object sender, EventArgs e)
        {

        }

        private void Grafico3_Click(object sender, EventArgs e)
        {

        }
    }
}
