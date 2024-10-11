using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaLogica.Inicio_Logica;
using CapaLogica.Colores_Logica;
using CapaDatos.Inicio_Datos;

namespace CapaVisual_Login
{
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }

        //**** Instancias******
        FrmListaOrdenes _FrmListaOrdenes = new FrmListaOrdenes();
        FrmInicio _FrmInicio = new FrmInicio();
        FrmMensajes _FrmMensajes = new FrmMensajes();
        FrmClaveAutorizada _FrmClaveAutorizada = new FrmClaveAutorizada();
        FrmConfiguracion _FrmConfiguracion = new FrmConfiguracion();
        FrmRepContratGart _FrmRepContrat = new FrmRepContratGart();
        FrmFacturacion _FrmFacturacion = new FrmFacturacion();
        FrmRepProSinPag _FrmRepProSinPaq = new FrmRepProSinPag();
        FrmRepOrden _FrmRepOrden = new FrmRepOrden();
        FrmRepOrdenTContact _FrmRepOrdenTContact = new FrmRepOrdenTContact();
        FrmRepNotaDev _FrmRepNotaDev = new FrmRepNotaDev();
        FrmMostrarReporte frmMostrar = new FrmMostrarReporte();
        D_Inicio _D_Inicio = new D_Inicio();
        L_Colores _L_Colores = new L_Colores();
        
        public bool osc;
        public string mostrarclientes;

        public void addformulario(Form F)
        {
            F.TopLevel = false;
            this.PnlListadoOrdenes.Controls.Add(F);
            F.Show();
            F.BringToFront();
            return;

        }

        private void BtnInicio_Click_1(object sender, EventArgs e)
        {




            System.Drawing.Color col2 = System.Drawing.ColorTranslator.FromHtml("#257b78");
            System.Drawing.Color col4 = System.Drawing.ColorTranslator.FromHtml("#2f6b64");
            System.Drawing.Color col3 = System.Drawing.ColorTranslator.FromHtml(" #07a79b");

            if (this.BackColor == col2)
            {
                BtnInicio.BackColor = Color.FromArgb(4, 185, 166);
                btnconfiguracion.BackColor = col2;
                BtnListadoOrdenes.BackColor = col2;
                btnClienteEspera.BackColor = col2;


            }
            else
            {
                BtnInicio.BackColor = Color.FromArgb(4, 185, 166);
                btnconfiguracion.BackColor = Color.White;
                BtnListadoOrdenes.BackColor = Color.White;
                btnClienteEspera.BackColor = Color.White;
            }




            if (_FrmConfiguracion.Signal == "yes")
            {
                if (this.BackColor == col2)
                {
                    ActualizacionInicio();
                    _FrmInicio.ConfigOscuro(col2, col3, col4);
                }

                else
                {

                    ActualizacionInicio();
                }
            }

            else
            {
                PnlListadoOrdenes.Controls.Clear();
                addformulario(_FrmInicio);
                _FrmInicio.Actualizar_UltimaVenta_VentasDia();
            }


        }

        private void BtnListadoOrdenes_Click(object sender, EventArgs e)
        {

            System.Drawing.Color col2 = System.Drawing.ColorTranslator.FromHtml("#257b78");



            if (this.BackColor == col2)
            {
                BtnListadoOrdenes.BackColor = Color.FromArgb(4, 185, 166);
                BtnInicio.BackColor = col2;
                btnconfiguracion.BackColor = col2;
                btnClienteEspera.BackColor = col2;


            }
            else
            {
                BtnListadoOrdenes.BackColor = Color.FromArgb(4, 185, 166);
                BtnInicio.BackColor = Color.White;
                btnconfiguracion.BackColor = Color.White;
                btnClienteEspera.BackColor = Color.White;
            }








            PnlListadoOrdenes.Controls.Clear();
            addformulario(_FrmListaOrdenes);
            Focus();
            //_FrmListaOrdenes.cerrar();
            //_FrmListaOrdenes.ListadoOrdenosRebot();
        }

        private void FrmPrincipal_Load_1(object sender, EventArgs e)
        {
            BtnInicio.PerformClick();
            string dia = (DateTime.UtcNow.ToShortDateString());
            string DiaActivo = _D_Inicio.DiaActivo().ToShortDateString();
            LblFechday.Text = DiaActivo;
            Btnosc.Visible = true;
            BtnClaro.Visible = false;

        }


        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            //this.Close();
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void Btnosc_Click(object sender, EventArgs e)
        {
          

            L_Colores.Oscuro = true;
            L_Colores.Claro = false;
            osc = true;

            //En esta funcion se configuran los colores para el fondo y para grid de listado de ordenes 
            //col 1 es blanco, col 2 es azul agua oscuro, col 3 es azul agua claro 
            System.Drawing.Color col2 = System.Drawing.ColorTranslator.FromHtml("#257b78");
            GbxMenuPrincipal.BackColor = col2;
            System.Drawing.Color col4 = System.Drawing.ColorTranslator.FromHtml("#2f6b64");
            System.Drawing.Color col3 = System.Drawing.ColorTranslator.FromHtml(" #07a79b");

            _FrmInicio.ConfigOscuro(col2, col3, col4);
            _FrmListaOrdenes.FormatoDataGrid2(col2, col3, col4);
            _FrmConfiguracion.FormatoConfig2(col2, col3, col4);
            _FrmFacturacion.FormatoFacturacion2(col2, col3, col4);
            


            BtnListadoOrdenes.BackColor = col2;
            BtnInicio.BackColor = col2;
            btnconfiguracion.BackColor = col2;
            btnClienteEspera.BackColor = col2;


            BtnInicio.ForeColor = Color.White;
            BtnListadoOrdenes.ForeColor = Color.White;
            btnClienteEspera.ForeColor = Color.White;
            btnconfiguracion.ForeColor = Color.White;
            LblFechday.ForeColor = Color.White;
            BackColor = col2;

            //FrmListaOrdenes frmListaOrdenes = new FrmListaOrdenes();
            //frmListaOrdenes.BackColor = col2;

            //_FrmListaOrdenes.EstructuraGrid();
            foreach (var form in Application.OpenForms.Cast<Form>())
            {
                form.BackColor = col2;
            }

            Btnosc.Visible = false;
            BtnClaro.Visible = true;

            // Cambiar iconos a color oscuro
            PicBoxInicClaro.Visible = false;
            PicBoxClientClaro.Visible = false;
            PicBoxListClaro.Visible = false;
            PicBoxConfigClaro.Visible = false;

            PicBoxInicOsc.Visible = true;
            PicBoxListOsc.Visible = true;
            PicBoxClientOsc.Visible = true;
            PicBoxConfigOsc.Visible = true;




        }

        private void BtnClaro_Click(object sender, EventArgs e)
        {
            //En esta funcion se configuran los colores para el fondo y para grid de listado de ordenes 
            //col 1 es blanco, col 3 es azul agua claro 

            L_Colores.Claro = true;
            L_Colores.Oscuro = false;
            System.Drawing.Color col1 = System.Drawing.ColorTranslator.FromHtml("#ffffff");
            System.Drawing.Color col3 = System.Drawing.ColorTranslator.FromHtml(" #07a79b");
            GbxMenuPrincipal.BackColor = col1;
            BackColor = col1;

            BtnListadoOrdenes.BackColor = Color.White;
            BtnInicio.BackColor = Color.White;
            btnconfiguracion.BackColor = Color.White;
            btnClienteEspera.BackColor = Color.White;



            BtnInicio.ForeColor = Color.Black;
            BtnListadoOrdenes.ForeColor = Color.Black;
            btnClienteEspera.ForeColor = Color.Black;
            btnconfiguracion.ForeColor = Color.Black;
            LblFechday.ForeColor = Color.Black;
            _FrmInicio.ConfigClara(col1, col3);
            _FrmListaOrdenes.FormatoDataGrid1(col1, col3);
            _FrmConfiguracion.FormatoConfig1(col1, col3);
            _FrmFacturacion.FormatoFacturacion1(col1, col3);
            
            //_FrmListaOrdenes.EstructuraGrid();
            foreach (var form in Application.OpenForms.Cast<Form>())
            {
                form.BackColor = col1;

            }

            Btnosc.Visible = true;
            BtnClaro.Visible = false;



            // Cambiar iconos a color claro
            PicBoxInicClaro.Visible = true;
            PicBoxClientClaro.Visible = true;
            PicBoxListClaro.Visible = true;
            PicBoxConfigClaro.Visible = true;

            PicBoxInicOsc.Visible = false;
            PicBoxListOsc.Visible = false;
            PicBoxClientOsc.Visible = false;
            PicBoxConfigOsc.Visible = false;

        }

        private void btnconfiguracion_Click(object sender, EventArgs e)
        {

            System.Drawing.Color col2 = System.Drawing.ColorTranslator.FromHtml("#257b78");
            if (this.BackColor == col2)
            {
                btnconfiguracion.BackColor = Color.FromArgb(4, 185, 166);
                BtnInicio.BackColor = col2;
                BtnListadoOrdenes.BackColor = col2;
                btnClienteEspera.BackColor = col2;


            }
            else
            {
                btnconfiguracion.BackColor = Color.FromArgb(4, 185, 166);
                BtnInicio.BackColor = Color.White;
                BtnListadoOrdenes.BackColor = Color.White;
                btnClienteEspera.BackColor = Color.White;
            }







            PnlListadoOrdenes.Controls.Clear();
            addformulario(_FrmConfiguracion);
            Focus();

        }


        public void ActualizacionInicio()
        {


            System.Drawing.Color col2 = System.Drawing.ColorTranslator.FromHtml("#257b78");
            System.Drawing.Color col4 = System.Drawing.ColorTranslator.FromHtml("#2f6b64");
            System.Drawing.Color col3 = System.Drawing.ColorTranslator.FromHtml(" #07a79b");
            _FrmInicio.Close();
            FrmInicio pp = new FrmInicio();




            if (this.BackColor == col2)
            {
                pp.ConfigOscuro(col2, col3, col4);
            }
            addformulario(pp);

            if (mostrarclientes == "yes")
            {
                pp.MostrarPnlClientesESpera();

            }
            mostrarclientes = "no";
            Focus();




        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {

            _FrmRepContrat.setParametros("01300000030");
            _FrmRepContrat.ConfigRep();
            _FrmRepContrat.ShowDialog();
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            System.Drawing.Color col2 = System.Drawing.ColorTranslator.FromHtml("#257b78");
            System.Drawing.Color col4 = System.Drawing.ColorTranslator.FromHtml("#2f6b64");
            System.Drawing.Color col3 = System.Drawing.ColorTranslator.FromHtml(" #07a79b");


            if (_FrmConfiguracion.Signal == "yes")
            {
                if (this.BackColor == col2)
                {
                    ActualizacionInicio();
                    _FrmInicio.ConfigOscuro(col2, col3, col4);
                }

                else
                {

                    ActualizacionInicio();
                }
            }

            else
            {
                PnlListadoOrdenes.Controls.Clear();
                addformulario(_FrmInicio);

            }

        }

        private void btnClienteEspera_Click(object sender, EventArgs e)
        {
            System.Drawing.Color col2 = System.Drawing.ColorTranslator.FromHtml("#257b78");
            System.Drawing.Color col4 = System.Drawing.ColorTranslator.FromHtml("#2f6b64");
            System.Drawing.Color col3 = System.Drawing.ColorTranslator.FromHtml(" #07a79b");

            if (this.BackColor == col2)
            {
                btnClienteEspera.BackColor = Color.FromArgb(4, 185, 166);
                btnconfiguracion.BackColor = col2;
                BtnInicio.BackColor = col2;
                BtnListadoOrdenes.BackColor = col2;


            }
            else
            {
                btnClienteEspera.BackColor = Color.FromArgb(4, 185, 166);
                btnconfiguracion.BackColor = Color.White;
                BtnInicio.BackColor = Color.White;
                BtnListadoOrdenes.BackColor = Color.White;
            }

            // _FrmClienteEspera.ShowDialog();





            if (_FrmConfiguracion.Signal == "yes")
            {
                mostrarclientes = "yes";
                ActualizacionInicio();

            }
            else
            {
                addformulario(_FrmInicio);
                _FrmInicio.MostrarPnlClientesESpera();


            }



        }

        private void label46_Click(object sender, EventArgs e)
        {


        }

        private void BtnMinimizar_Click(object sender, EventArgs e)
        {
            // Para minimizar Caronipos
            this.WindowState = FormWindowState.Minimized;
        }

        private void button1_Click_3(object sender, EventArgs e)
        {
          
        }

        private void button1_Click_4(object sender, EventArgs e)
        {
           
        }

        private void button1_Click_5(object sender, EventArgs e)
        {
           
        }
    }
}