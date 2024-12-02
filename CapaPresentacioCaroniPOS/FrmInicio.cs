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

        }

        public void CrearGrafico(string parametro, string codempleado, string CodSuc)
        {
            _D_Inicio.GraficoVentas("20220808", "07165");
            _D_Inicio.GraficoVentasDet(parametro, codempleado, CodSuc);// Arma grafico de cristales
            _D_Inicio.GraficoVentasMontDet(parametro, codempleado, CodSuc);//Arma Grafico de monturas
            _D_Inicio.leyendaCrist(parametro, codempleado, CodSuc);// obtiene leyenda de cristales
            _D_Inicio.leyendaMont(parametro, codempleado, CodSuc);// Obtiene leyenda de monturas

            Grafico1.Series["Series1"].Points.DataBindXY(_D_Inicio.NombreC, _D_Inicio.CantidadC); // Crea el grafico segun los datos

            for (int i = 0; i < _D_Inicio.contC; i++) // Inserta la leyenda por cada registro
            {
                string leyenC = (string)_D_Inicio.NombreLeyC[i];
                Grafico1.Series["Series1"].Points[i].LegendText = leyenC;

            }
            Grafico2.Series["Series1"].Points.DataBindXY(_D_Inicio.NombreM, _D_Inicio.CantidadM);// Crea el grafico segun los datos

            for (int i = 0; i < _D_Inicio.contM; i++) // Inserta la leyenda por cada registro
            {
                string leyenM = (string)_D_Inicio.TotalLeyM[i];
                Grafico2.Series["Series1"].Points[i].LegendText = leyenM;

            }

        }


        public void CrearGraficoPorRango(string parametro1, string parametro2, string codempleado, string CodSuc)
        {

            _D_Inicio.GraficoVentasDetRango(parametro1, parametro2, codempleado, CodSuc); // Arma grafico de cristales por rango 
            _D_Inicio.GraficoVentasMontDetRango(parametro1, parametro2, codempleado, CodSuc); //Arma Grafico de monturas por rango
            _D_Inicio.LeyendCristRango(parametro1, parametro2, codempleado, CodSuc); // obtiene leyenda de cristales por rango 
            _D_Inicio.LeyendMontRango(parametro1, parametro2, codempleado, CodSuc);// Obtiene leyenda de monturas por rango 

            Grafico1.Series["Series1"].Points.DataBindXY(_D_Inicio.NombreC, _D_Inicio.CantidadC); // Crea el grafico segun los datos

            for (int i = 0; i < _D_Inicio.contC; i++) // Inserta la leyenda por cada registro
            {
                string leyenC = (string)_D_Inicio.NombreLeyC[i];
                Grafico1.Series["Series1"].Points[i].LegendText = leyenC;

            }
            Grafico2.Series["Series1"].Points.DataBindXY(_D_Inicio.NombreM, _D_Inicio.CantidadM); // Crea el grafico segun los datos 
            for (int i = 0; i < _D_Inicio.contM; i++) // Inserta la leyenda por cada registro
            {
                string leyenM = (string)_D_Inicio.TotalLeyM[i];
                Grafico2.Series["Series1"].Points[i].LegendText = leyenM;

            }

        }

        public void CrearGraficoMetas(string Periodo, string CodEmpleado, string Meta)
        {

            _D_Inicio.GraficoMetasUds(Periodo, CodEmpleado, Meta); // Arma el grafico de metas por Uds
            _D_Inicio.GraficoMetasIng(Periodo, CodEmpleado, Meta); // Arma el grafico de metas por Ingresos
            _D_Inicio.LeyendaGraficoMetaIng(Periodo, CodEmpleado, Meta); // Obtiene leyenda de las metas por ingresos
            _D_Inicio.LeyendaGraficoMetaUds(Periodo, CodEmpleado, Meta); // Obtiene leyenda de las metas por unidad

            Grafico3.Series["Series1"].Points.DataBindXY(_D_Inicio.LeyMetUds, _D_Inicio.DatosMetUds); // Crea el grafico segun los datos

            for (int i = 0; i < _D_Inicio.contmet; i++) // Inserta la leyenda por cada registro
            {
                string LeyenMetUds = (string)_D_Inicio.LeyendaMetUds[i];
                Grafico3.Series["Series1"].Points[i].LegendText = LeyenMetUds;

            }



            Grafico4.Series["Series1"].Points.DataBindXY(_D_Inicio.LeyMetBs, _D_Inicio.DatosMetBs); // Crea el grafico segun los datos 

            for (int i = 0; i < _D_Inicio.contmet; i++) // Inserta la leyenda por cada registro
            {
                string LeyenMetIng = (string)_D_Inicio.LeyMetIng[i];
                Grafico4.Series["Series1"].Points[i].LegendText = LeyenMetIng;

            }



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
                DgvUltimasVentas.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

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


        public void FormatoDataGridListaEspera()
        {

            try
            {
                DgvClientes.Columns["Eliminar"].HeaderText = "Borrar";

                DgvClientes.Columns["Cliente"].Width = 30;
                DgvClientes.Columns["Cédula"].Width = 30;
                DgvClientes.Columns["Eliminar"].Width = 80;

                DgvClientes.Columns["Cliente"].SortMode = DataGridViewColumnSortMode.NotSortable;
                DgvClientes.Columns["Cédula"].SortMode = DataGridViewColumnSortMode.NotSortable;

                DgvClientes.Columns["Cliente"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvClientes.Columns["Cédula"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvClientes.Columns["Eliminar"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            }


            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }
        }


        public void Actualizar_UltimaVenta_VentasDia()
        {
            MostarOrdenes();
            string CodEmpleado = TB_USUARIO.COD_EMPLEADO;
            _D_Inicio.TraerConfiguracion(CodEmpleado);
            string MonedaVentas = _D_Inicio.Cod04;
            if (MonedaVentas == "Bs")
            {

                _L_Inicio.CargarVentasDiasBs();
                LblVentas.Text = _L_Inicio.VentaDia;
                LblMonedaVenta.Text = "Bs";

            }

            else 
            {

                _L_Inicio.CargarVentasDiasDol();
                LblVentas.Text = _L_Inicio.VentaDia;
                LblMonedaVenta.Text = "$";

            }

            // Refrescar Grid Ultimas Ventas del Dia 
            MostarOrdenes();
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


        public void Clientes()
        {
            try
            {
                DgvClientes.DataSource = "";
                DgvClientes.DataMember = "";
                DgvClientes.DataSource = _L_Inicio.CargarClientes();

                DataGridViewButtonColumn Btnclm = new DataGridViewButtonColumn();
                Btnclm.Name = "Eliminar";
                Btnclm.HeaderText = "Borrar";
                Btnclm.Width = 60;
                DgvClientes.Columns.Add(Btnclm);

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
            string mensaje = "Estas seguro de eliminar este cliente";

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
                Icon icoAtomico = new Icon(Environment.CurrentDirectory + @"\\Eliminar_Clientes.ico");
                e.Graphics.DrawIcon(icoAtomico, e.CellBounds.Left + 1, e.CellBounds.Top + 0);


                this.DgvClientes.Rows[e.RowIndex].Height = icoAtomico.Height + 0;
                this.DgvClientes.Columns[e.ColumnIndex].Width = icoAtomico.Width + 0;

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
                LblTasa.Text = _L_Inicio.TasaDiaDol;
                LblBs.Text = "Bs/$";
                _L_Inicio.CargarTexboxEur();
                LblTasaSec.Text = _L_Inicio.TasaDiaEur + " Bs/€";

            }
            if (_L_Inicio.MonedaTasa == "Euros")
            {
                _L_Inicio.CargarTexboxEur();
                LblTasa.Text = _L_Inicio.TasaDiaEur;
                LblBs.Text = "Bs/€";
                _L_Inicio.CargarTexboxDol();
                LblTasaSec.Text = _L_Inicio.TasaDiaDol + " Bs/$";

            }

            if (_L_Inicio.MonedaVentas == "Bs")
            {

                _L_Inicio.CargarVentasDiasBs();
                LblVentas.Text = _L_Inicio.VentaDia;
                LblMonedaVenta.Text = "Bs";

            }

            if (_L_Inicio.MonedaVentas == "USD")
            {

                _L_Inicio.CargarVentasDiasDol();
                LblVentas.Text = _L_Inicio.VentaDia;
                LblMonedaVenta.Text = "$";

            }


            if (_L_Inicio.ConfigDefault == "Default")
            {

                Grafico3.Visible = false;
                Grafico4.Visible = false;
                CrearGrafico("30", "", "075");
                _L_Inicio.CargarTexboxDol();
                LblTasa.Text = _L_Inicio.TasaDiaDol;
                LblBs.Text = "Bs/$";
                _L_Inicio.CargarTexboxEur();
                LblTasaSec.Text = _L_Inicio.TasaDiaEur + " Bs/€";

                _L_Inicio.CargarVentasDiasDol();
                LblVentas.Text = _L_Inicio.VentaDia;
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

                    // CrearGraficoMetas(par, TB_USUARIO.COD_EMP);

                    if (_L_Inicio.InfoGraf.Length == 5) // Validamos si la informacion que se muestra es por usario tambien funciona para mostrart colaborador
                    {
                        CrearGraficoMetas(par, _L_Inicio.InfoGraf, "E"); // estamos colocando como codigoemplead 07165 porque el de logeo no tiene ventas 
                    }

                    if (_L_Inicio.InfoGraf.Length <= 3) // Validamos si la informacion que se muestra es por sucursal
                    {
                        CrearGraficoMetas(par, TB_USUARIO.COD_EMPLEADO, "S");
                    }

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
                            CrearGraficoPorRango(_L_Inicio.PeriodoGraf, _L_Inicio.Valor2graf, _L_Inicio.InfoGraf, ""); // estamos colocando como codigoemplead 07165 porque el de logeo no tiene ventas 
                        }

                        if (_L_Inicio.InfoGraf.Length <= 3) // Validamos si la informacion que se muestra es por sucursal
                        {
                            CrearGraficoPorRango(_L_Inicio.PeriodoGraf, _L_Inicio.Valor2graf, "", _L_Inicio.InfoGraf);
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

        public void ConfigClara(System.Drawing.Color col1, System.Drawing.Color col3)
        {
            this.BackColor = col1;

        }
        public void ConfigOscuro(System.Drawing.Color col2, System.Drawing.Color col3, System.Drawing.Color col4)
        {
            this.BackColor = col2;

        }


        public void actGribdCliente()
        {
            Clientes();
            DgvClientes.Refresh();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (TxtNombreCliente.Text == "" || TxtApellidoCliente.Text == "" || TxtCedulaCliente.Text == "")
            {

                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Los campos no pueden estar en blanco");
                _FrmMensajes.ShowDialog();

            }
            else
            {
                _L_Inicio.GuardarClientesEspera(TxtCedulaCliente.Text, TxtNombreCliente.Text, TxtApellidoCliente.Text);
                LimpiarDgvCliente();
                Clientes();
                DgvClientes.Refresh();
            }

            PnlClientesEspera.Visible = false;
            LimpiarTxt();
        }

        public void MostrarPnlClientesESpera()
        {
            LimpiarTxt();

            PnlClientesEspera.Visible = true;

            if (PnlClientesEspera.Visible == false)
            {
                PnlClientesEspera.Visible = true;

            }





        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            PnlClientesEspera.Visible = false;
            LimpiarTxt();
        }

        public void LimpiarTxt()
        {
            TxtNombreCliente.Text = "";
            TxtApellidoCliente.Text = "";
            TxtCedulaCliente.Text = "";

        }

        public void LimpiarDgvCliente()
        {

            DgvClientes.DataSource = "";
            DgvClientes.DataMember = "";

            var dataGridViewColumn2 = DgvClientes.Columns["Eliminar"];
            if (dataGridViewColumn2 != null && dataGridViewColumn2.Visible)

            {

                DgvClientes.Columns.RemoveAt(DgvClientes.Columns.Count - 1);
            }

        }

        private void TxtCedulaCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            //para que solo acepte numeros
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void TxtCedulaCliente_TextChanged(object sender, EventArgs e)
        {

        }



    }




}

