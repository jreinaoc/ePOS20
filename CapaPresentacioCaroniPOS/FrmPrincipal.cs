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
using EnvioPagoMovil;
using CapaDatos.DetalleOrden_Datos;
using CapaEntidades;
using CapaDatos.Anulacion;
using CapaLogica.CierreCaja_Logica;
using CapaDatos.ListaOrdenes_Datos;

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
        FrmPagoMovil _FrmPagoMovil = new FrmPagoMovil();
        FrmListaFactura _FrmListaFactura = new FrmListaFactura();
        D_Anulacion _D_Anulacion = new D_Anulacion();
        FrmCargarOrden _FrmCargarOrden = new FrmCargarOrden();
        FrmCierredeCaja _FrmCierreDeCaja = new FrmCierredeCaja();
        FrmTasaDia _FrmTasaDia = new FrmTasaDia();
        FrmPrueba _frmPrueba = new FrmPrueba();
        FrmReimpresionDocumentos _FrmReimpresionDocumentos = new FrmReimpresionDocumentos();

        private D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        public bool osc;
        public string mostrarclientes;
        private string mensaje = "";

        //L_CierreCaja _L_CierreCaja = new L_CierreCaja();
        private L_CierreCaja _L_CierreCaja = new L_CierreCaja();
        private D_ListaOrdenes _D_ListaOrdenes = new D_ListaOrdenes();
        public void addformulario(Form F)
        {
            F.TopLevel = false;
            this.PnlListadoOrdenes.Controls.Add(F);
            F.Show();
            F.BringToFront();
            return;

        }

        public void addformularioCargaOrdenes(Form F)
        {
            F.TopLevel = false;

            // Establecer la ubicación y el tamaño del formulario
            F.Location = new Point(50, 50); // Coordenadas
            F.Size = new Size(1472, 1168); // Tamaño

            // Agregar el formulario al panel y mostrarlo
            this.PnlListadoOrdenes.Controls.Add(F);
            F.Show();
            F.BringToFront();
        }

        private void BtnInicio_Click_1(object sender, EventArgs e)
        {


            pnlUtilitarios.Visible = false;

            System.Drawing.Color col2 = System.Drawing.ColorTranslator.FromHtml("#257b78");
            System.Drawing.Color col4 = System.Drawing.ColorTranslator.FromHtml("#2f6b64");
            System.Drawing.Color col3 = System.Drawing.ColorTranslator.FromHtml(" #07a79b");

            if (this.BackColor == col2)
            {
                BtnInicio.BackColor = Color.FromArgb(4, 185, 166);
                btnconfiguracion.BackColor = col2;
                BtnListadoOrdenes.BackColor = col2;
                btnClienteEspera.BackColor = col2;
                btnPagoMovil.BackColor = col2;
                btnListaFactura.BackColor = col2;
                btnCargarOrdenes.BackColor = col2;
            }
            else
            {
                BtnInicio.BackColor = Color.FromArgb(4, 185, 166);
                btnconfiguracion.BackColor = Color.White;
                BtnListadoOrdenes.BackColor = Color.White;
                btnClienteEspera.BackColor = Color.White;
                btnPagoMovil.BackColor = Color.White;
                btnListaFactura.BackColor = Color.White;
                btnCargarOrdenes.BackColor = Color.White;
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
                _FrmInicio.Actualizar_Tasas();
                PnlListadoOrdenes.Controls.Clear();
                addformulario(_FrmInicio);
                _FrmInicio.Actualizar_UltimaVenta_VentasDia();
            }



        }

        private void BtnListadoOrdenes_Click(object sender, EventArgs e)
        {
            pnlUtilitarios.Visible = false;

            System.Drawing.Color col2 = System.Drawing.ColorTranslator.FromHtml("#257b78");



            if (this.BackColor == col2)
            {
                BtnListadoOrdenes.BackColor = Color.FromArgb(4, 185, 166);
                BtnInicio.BackColor = col2;
                btnconfiguracion.BackColor = col2;
                btnClienteEspera.BackColor = col2;
                btnPagoMovil.BackColor = col2;
                btnListaFactura.BackColor = col2;
                btnCargarOrdenes.BackColor = col2;
            }
            else
            {
                BtnListadoOrdenes.BackColor = Color.FromArgb(4, 185, 166);
                BtnInicio.BackColor = Color.White;
                btnconfiguracion.BackColor = Color.White;
                btnClienteEspera.BackColor = Color.White;
                btnPagoMovil.BackColor = Color.White;
                btnListaFactura.BackColor = Color.White;
                btnCargarOrdenes.BackColor = Color.White;
            }




            string bloqFacturacion = _D_DetalleOrden.TB_PARAMETRO("BloqFacturacion");

            if (bloqFacturacion == "0")
            {
                PnlListadoOrdenes.Controls.Clear();
                addformulario(_FrmListaOrdenes);
                Focus();
                //_FrmListaOrdenes.cerrar();
                //_FrmListaOrdenes.ListadoOrdenosRebot();

                string Sucursal = _D_DetalleOrden.TB_PARAMETRO("SucursalId");
                _D_Anulacion.CaragarAuditor(Sucursal, "010", TB_USUARIO.COD_EMPLEADO, "Entrada a ventas pendientes");
            }
            else
            {
                mensaje = "Se eliminaron registros de la base de datos, comuníquese con el Departamento de sistemas";
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(mensaje);
                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                _FrmMensajes.ShowDialog();
            }
        }

        private void FrmPrincipal_Load_1(object sender, EventArgs e)
        {
            BtnInicio.PerformClick();
            string dia = (DateTime.UtcNow.ToShortDateString());
            string DiaActivo = _D_Inicio.DiaActivo().ToShortDateString();
            LblFechday.Text = DiaActivo;
            Btnosc.Visible = true;
            BtnClaro.Visible = false;

            string PMAutomatico = _D_DetalleOrden.TB_PARAMETRO("PMAutomatico");
            if (Envio.validarPermisos() == true && PMAutomatico  == "1")
            {
                
                btnPagoMovil.Visible = true;
                pictBoxPagoMovil.Visible = true;
            }
            else
            {
                btnPagoMovil.Visible = false;
                pictBoxPagoMovil.Visible = false;
            }

        }


        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            //this.Close();
            string Sucursal = _D_DetalleOrden.TB_PARAMETRO("SucursalId");
            _D_Anulacion.CaragarAuditor(Sucursal, "002", TB_USUARIO.COD_EMPLEADO, "Cerrar Sesión (Salida del Sistema)");

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
            System.Drawing.Color col1 = System.Drawing.ColorTranslator.FromHtml("#ffffff");
            System.Drawing.Color col2 = System.Drawing.ColorTranslator.FromHtml("#257b78");
            GbxMenuPrincipal.BackColor = col2;
            System.Drawing.Color col4 = System.Drawing.ColorTranslator.FromHtml("#2f6b64");
            System.Drawing.Color col3 = System.Drawing.ColorTranslator.FromHtml(" #07a79b");
            //col 5 es Noble Black, col6 es Nordic Noir
            System.Drawing.Color col5 = System.Drawing.ColorTranslator.FromHtml("#1c2422");
            System.Drawing.Color col6 = System.Drawing.ColorTranslator.FromHtml("#003536");


            _FrmInicio.ConfigOscuro(col2, col3, col4);
            _FrmListaOrdenes.FormatoDataGrid2(col2, col3, col4);
            _FrmConfiguracion.FormatoConfig2(col2, col3, col4);
            _FrmFacturacion.FormatoFacturacion2(col2, col3, col4);
            _FrmListaFactura.FormatoDataGrid_Oscuro_ListaFact(col2, col3, col4);
            //_FrmCargarOrden.FormatoDataGrid_Oscuro_Dgv_Tap3_Articulo(col2, col3, col4);
            _FrmCierreDeCaja.FormatoOsc(col1, col3, col5, col6);
            _FrmCargarOrden.FormatoOscuro(col2, col3, col4);

            BtnListadoOrdenes.BackColor = col2;
            BtnInicio.BackColor = col2;
            btnconfiguracion.BackColor = col2;
            btnClienteEspera.BackColor = col2;
            btnListaFactura.BackColor = col2;
            btnPagoMovil.BackColor = col2;
            btnCargarOrdenes.BackColor = col2;

            BtnInicio.ForeColor = Color.White;
            BtnListadoOrdenes.ForeColor = Color.White;
            btnClienteEspera.ForeColor = Color.White;
            btnconfiguracion.ForeColor = Color.White;
            LblFechday.ForeColor = Color.White;
            btnListaFactura.ForeColor = Color.White;
            btnPagoMovil.ForeColor = Color.White;
            btnCargarOrdenes.ForeColor = Color.White;
            BackColor = col2;
            button2.ForeColor = Color.White;
            btnUtilitarios.ForeColor = Color.White;
            

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
            PicBoxCargaOrdenClaro.Visible = false;
            PicBoxListClaro.Visible = false;
            PicBoxConfigClaro.Visible = false;
            pictBoxPagoMovil.Visible = false;
            pictBoxListaFactura.Visible = false;

            PicBoxInicOsc.Visible = true;
            PicBoxListOsc.Visible = true;
            PicBoxClientOsc.Visible = true;
            PicBoxCargaOrdenOsc.Visible = true;
            PicBoxConfigOsc.Visible = true;
           // pictBoxPagoMovilOsc.Visible = true;
            pictureBox2.Visible = true;

            string PMAutomatico = _D_DetalleOrden.TB_PARAMETRO("PMAutomatico");
            if (Envio.validarPermisos() == true && PMAutomatico == "1")
            {
                btnPagoMovil.Visible = true;
                pictBoxPagoMovilOsc.Visible = true;
            }
            else
            {
                btnPagoMovil.Visible = false;
                pictBoxPagoMovilOsc.Visible = false;
            }
        }

        private void 
            BtnClaro_Click(object sender, EventArgs e)
        {
            //En esta funcion se configuran los colores para el fondo y para grid de listado de ordenes 
            //col 1 es blanco, col 3 es azul agua claro 

            L_Colores.Claro = true;
            L_Colores.Oscuro = false;
            System.Drawing.Color col1 = System.Drawing.ColorTranslator.FromHtml("#ffffff");
            System.Drawing.Color col3 = System.Drawing.ColorTranslator.FromHtml(" #07a79b");
            System.Drawing.Color col5 = System.Drawing.ColorTranslator.FromHtml("#1c2422");
            GbxMenuPrincipal.BackColor = col1;
            BackColor = col1;

            BtnListadoOrdenes.BackColor = Color.White;
            BtnInicio.BackColor = Color.White;
            btnconfiguracion.BackColor = Color.White;
            btnClienteEspera.BackColor = Color.White;
            btnListaFactura.BackColor = Color.White;
            btnPagoMovil.BackColor = Color.White;
            btnCargarOrdenes.BackColor = Color.White;

            button2.ForeColor = Color.Black;
            BtnInicio.ForeColor = Color.Black;
            BtnListadoOrdenes.ForeColor = Color.Black;
            btnClienteEspera.ForeColor = Color.Black;
            btnconfiguracion.ForeColor = Color.Black;
            LblFechday.ForeColor = Color.Black;
            btnListaFactura.ForeColor = Color.Black;
            btnPagoMovil.ForeColor = Color.Black;
            btnCargarOrdenes.ForeColor = Color.Black;
            btnUtilitarios.ForeColor = Color.Black;
     

            _FrmInicio.ConfigClara(col1, col3);
            _FrmListaOrdenes.FormatoDataGrid1(col1, col3);
            _FrmConfiguracion.FormatoConfig1(col1, col3);
            _FrmFacturacion.FormatoFacturacion1(col1, col3);
            _FrmListaFactura.FormatoDataGrid_Claro_ListaFact(col1, col3);
            _FrmCargarOrden.FormatoDataGrid_Claro_Dgv_Tap3_Articulo(col1, col3);
            _FrmCargarOrden.FormatoOscuro(col1, col3, col1);
            _FrmCierreDeCaja.FormatoClaro(col1, col3, col5);


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
            PicBoxCargaOrdenClaro.Visible = true;
            PicBoxListClaro.Visible = true;
            PicBoxConfigClaro.Visible = true;
            //pictBoxPagoMovil.Visible = true;
            pictBoxListaFactura.Visible = true;

            PicBoxInicOsc.Visible = false;
            PicBoxListOsc.Visible = false;
            PicBoxClientOsc.Visible = false;
            PicBoxCargaOrdenOsc.Visible = false;
            PicBoxConfigOsc.Visible = false;
            //pictBoxPagoMovil.Visible = false;
            pictureBox2.Visible = false;

            string PMAutomatico = _D_DetalleOrden.TB_PARAMETRO("PMAutomatico");
            if (Envio.validarPermisos() == true && PMAutomatico == "1")
            {
                btnPagoMovil.Visible = true;
                pictBoxPagoMovil.Visible = true;
            }
            else
            {
                btnPagoMovil.Visible = false;
                pictBoxPagoMovil.Visible = false;
            }

        }

        private void btnconfiguracion_Click(object sender, EventArgs e)
        {
            pnlUtilitarios.Visible = false;

            System.Drawing.Color col2 = System.Drawing.ColorTranslator.FromHtml("#257b78");
            if (this.BackColor == col2)
            {
                btnconfiguracion.BackColor = Color.FromArgb(4, 185, 166);
                BtnInicio.BackColor = col2;
                BtnListadoOrdenes.BackColor = col2;
                btnClienteEspera.BackColor = col2;
                btnPagoMovil.BackColor = col2;
                btnListaFactura.BackColor = col2;
                btnCargarOrdenes.BackColor = col2;
            }
            else
            {
                btnconfiguracion.BackColor = Color.FromArgb(4, 185, 166);
                BtnInicio.BackColor = Color.White;
                BtnListadoOrdenes.BackColor = Color.White;
                btnClienteEspera.BackColor = Color.White;
                btnPagoMovil.BackColor = Color.White;
                btnListaFactura.BackColor = Color.White;
                btnCargarOrdenes.BackColor = Color.White;
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
            pnlUtilitarios.Visible = false; 
            
            System.Drawing.Color col2 = System.Drawing.ColorTranslator.FromHtml("#257b78");
            System.Drawing.Color col4 = System.Drawing.ColorTranslator.FromHtml("#2f6b64");
            System.Drawing.Color col3 = System.Drawing.ColorTranslator.FromHtml(" #07a79b");

            if (this.BackColor == col2)
            {
                btnClienteEspera.BackColor = Color.FromArgb(4, 185, 166);
                btnconfiguracion.BackColor = col2;
                BtnInicio.BackColor = col2;
                BtnListadoOrdenes.BackColor = col2;
                btnPagoMovil.BackColor = col2;
                btnListaFactura.BackColor = col2;
                btnCargarOrdenes.BackColor = col2;
            }
            else
            {
                btnClienteEspera.BackColor = Color.FromArgb(4, 185, 166);
                btnconfiguracion.BackColor = Color.White;
                BtnInicio.BackColor = Color.White;
                BtnListadoOrdenes.BackColor = Color.White;
                btnPagoMovil.BackColor = Color.White;
                btnListaFactura.BackColor = Color.White;
                btnCargarOrdenes.BackColor = Color.White;
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

        private void btnPagoMovil_Click(object sender, EventArgs e)
        {
            pnlUtilitarios.Visible = false; 
            System.Drawing.Color col2 = System.Drawing.ColorTranslator.FromHtml("#257b78");

            if (this.BackColor == col2)
            {
                btnPagoMovil.BackColor = Color.FromArgb(4, 185, 166);
                BtnListadoOrdenes.BackColor = col2;
                BtnInicio.BackColor = col2;
                btnconfiguracion.BackColor = col2;
                btnClienteEspera.BackColor = col2;
                btnListaFactura.BackColor = col2;
                btnCargarOrdenes.BackColor = col2;

            }
            else
            {
                btnPagoMovil.BackColor = Color.FromArgb(4, 185, 166);
                BtnInicio.BackColor = Color.White;
                btnconfiguracion.BackColor = Color.White;
                btnClienteEspera.BackColor = Color.White;
                BtnListadoOrdenes.BackColor = Color.White;
                btnListaFactura.BackColor = Color.White;
                btnCargarOrdenes.BackColor = Color.White;
            }

            PnlListadoOrdenes.Controls.Clear();
            addformulario(_FrmPagoMovil);
            _FrmPagoMovil.btnlupa_Click_1(this, EventArgs.Empty);
            Focus();
        }

        private void btnListaFactura_Click(object sender, EventArgs e)
        {
            pnlUtilitarios.Visible = false; 
            System.Drawing.Color col2 = System.Drawing.ColorTranslator.FromHtml("#257b78");

            if (this.BackColor == col2)
            {
                btnListaFactura.BackColor = Color.FromArgb(4, 185, 166);
                BtnListadoOrdenes.BackColor = col2;
                BtnInicio.BackColor = col2;
                btnconfiguracion.BackColor = col2;
                btnClienteEspera.BackColor = col2;
                btnCargarOrdenes.BackColor = col2;


            }
            else
            {
                btnListaFactura.BackColor = Color.FromArgb(4, 185, 166);
                BtnListadoOrdenes.BackColor = Color.White;
                btnconfiguracion.BackColor = Color.White;
                btnClienteEspera.BackColor = Color.White;
                BtnListadoOrdenes.BackColor = Color.White;
                btnPagoMovil.BackColor = Color.White;
                btnCargarOrdenes.BackColor = Color.White;
            }

            PnlListadoOrdenes.Controls.Clear();
            addformulario(_FrmListaFactura);
            Focus();

        }

        private void btnCargarOrdenes_Click(object sender, EventArgs e)
        {
            pnlUtilitarios.Visible = false;
            System.Drawing.Color col2 = System.Drawing.ColorTranslator.FromHtml("#257b78");



            if (this.BackColor == col2)
            {
                btnCargarOrdenes.BackColor = Color.FromArgb(4, 185, 166);
                BtnListadoOrdenes.BackColor = col2;
                BtnInicio.BackColor = col2;
                btnconfiguracion.BackColor = col2;
                btnClienteEspera.BackColor = col2;
                btnPagoMovil.BackColor = col2;
                btnListaFactura.BackColor = col2;

            }
            else
            {
                btnCargarOrdenes.BackColor = Color.FromArgb(4, 185, 166);
                BtnListadoOrdenes.BackColor = Color.White;
                BtnInicio.BackColor = Color.White;
                btnconfiguracion.BackColor = Color.White;
                btnClienteEspera.BackColor = Color.White;
                btnPagoMovil.BackColor = Color.White;
                btnListaFactura.BackColor = Color.White;
            }




            //FrmCargarOrden formularioSecundario = new FrmCargarOrden();
            ////formularioSecundario = new FrmCargarOrden();
            //formularioSecundario.TopLevel = false;
            //formularioSecundario.Dock = DockStyle.Fill;
            ////pnlContenedor.Controls.Add(formularioSecundario);  // Tu panel
            ////formularioSecundario.Show();

            //FrmCargarOrden formularioSecundario = new FrmCargarOrden();

            PnlListadoOrdenes.Controls.Clear();
            addformularioCargaOrdenes(_FrmCargarOrden);
            Focus();
            //_FrmListaOrdenes.cerrar();
            //_FrmListaOrdenes.ListadoOrdenosRebot();

            //FrmCargarOrden nuevoFormulario = new FrmCargarOrden();

            // Cargar usando la función que limpia y configura el panel
            //CerrarYRecargarFormulario(nuevoFormulario);



        }

        private void GbxMenuPrincipal_Enter(object sender, EventArgs e)
        {

        }

        private void btnUtilitarios_Click(object sender, EventArgs e)
        {
            if (pnlUtilitarios.Visible == true)
            {
                pnlUtilitarios.Visible = false;
            }
            else
            {
                pnlUtilitarios.Visible = true;
            }
        }

        private void btnCierredeCaja_Click(object sender, EventArgs e)
        {
            pnlUtilitarios.Visible = false;

            string DiaActual = (DateTime.Now.ToString("dd/MM/yyyy"));
            string DiaActivo = _D_Inicio.DiaActivo().ToShortDateString();



            //if (TB_USUARIO.COD_EMPLEADO != "99999")
            //{
            if (_D_Inicio.DiaActivo() >= DateTime.Now)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Imposible cerrar la caja, el día activo es mayor a la fecha de hoy");
                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                _FrmMensajes.ShowDialog();
                return;

            }
            //}

            DateTime currentDate = _D_Inicio.DiaActivo();
            string formattedDate = currentDate.ToString("yyyyMMdd");

            if (_L_CierreCaja.ChequeaFacturasdelDia(formattedDate, TB_USUARIO.COD_USR))
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Existen INCONSISTENCIAS en los abonos de las facturas del día. Comuníquese con el Dpto de sistemas");
                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                _FrmMensajes.ShowDialog();
                return;
            }

            //Le enviamos el index asociados al valor selecionado en el combobox 
            DataSet Ordenesrango = _D_ListaOrdenes.CargarOrdPorRango(DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("dd/MM/yyyy"), "004", "", 1, 12);

            if (Ordenesrango.Tables[0].Rows.Count > 0)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("No Puede Cerrar Caja. Hay Ventas Pendientes");
                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                _FrmMensajes.ShowDialog();
                return;
            }

            string sucursal = _D_DetalleOrden.TB_PARAMETRO("sucursalId");

            if (_D_DetalleOrden.TB_PARAMETRO("VerificaEnvioCi") == "1")
            {
                string condicion = "(TB_CAORDSER.Cod_DetVta <> '08') AND (TB_CAORDSER.Cod_Sucursal='" + sucursal + "') AND (TB_CAORDSER.Cod_Venta <> '001') AND (TB_CAORDSER.OrSer_Status <> '004') AND (TB_CAORDSER.OrSer_Status <> '003') AND (TB_CAORDSER.OrSer_Status <> '006') AND (TB_CAORDSER.Fec_Envio IS NULL) AND (TB_CAORDSER.Fec_Recibido IS NULL) AND (TB_CAORDSER.Fec_Entrega IS NULL) AND (TB_SUCURSALLABORATORIOSERVICIO.Envio_Digital = '1') AND (TB_CAORDSER.Anulado='0') ORDER BY TB_CAORDSER.NumOrdserv";

                string bandera = "ENVOS";

                _L_CierreCaja.ORDSERVCRITERIOSVARIOS("","");

                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Realice todos los envios digitales antes de realizar el cierre definitivo");
                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                _FrmMensajes.ShowDialog();
                return;
            }

            PnlListadoOrdenes.Controls.Clear();
            addformulario(_FrmCierreDeCaja);
            Focus();

        }

        private void btn_FrmCierreDeCaja_Click(object sender, EventArgs e)
        {
            PnlListadoOrdenes.Controls.Clear();
            addformulario(_FrmCierreDeCaja);
            Focus();
        }

        private void btnTasaSec_Click(object sender, EventArgs e)
        {
            pnlUtilitarios.Visible = false; 
            PnlListadoOrdenes.Controls.Clear();
            addformulario(_FrmTasaDia);
            Focus();
        }

        private void button1_Click_6(object sender, EventArgs e)
        {
            PnlListadoOrdenes.Controls.Clear();
            addformulario(_frmPrueba);
            Focus();
        }


        public void ActualizarTextoLabel(string nuevoTexto)
        {
            LblFechday.Text = nuevoTexto;
        }

        public void CerrarYRecargarFormulario(Form nuevoFormulario)
        {
            // Limpiar eventos y controles anteriores
            foreach (Control ctrl in PnlListadoOrdenes.Controls)
            {
                if (ctrl is Form frm)
                {
                    frm.Hide();
                    frm.Close();       // Cierra de forma estándar
                    frm.Dispose();     // Libera recursos
                }
                ctrl.Dispose();        // Por si hay otros controles residuales
            }

            PnlListadoOrdenes.Controls.Clear();
            GC.Collect();              // Fuerza limpieza de memoria (opcional)

            // Cargar nuevo formulario
            nuevoFormulario.TopLevel = false;
            nuevoFormulario.Dock = DockStyle.Fill;
            PnlListadoOrdenes.Controls.Add(nuevoFormulario);
            nuevoFormulario.Show();
        }

        private void btnReimpresion_Click(object sender, EventArgs e)
        {
            pnlUtilitarios.Visible = false;
            PnlListadoOrdenes.Controls.Clear();
            addformulario(_FrmReimpresionDocumentos);
            Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pnlUtilitarios.Visible = false;

            FrmPrueba frmReportes = new FrmPrueba();

            frmReportes.ReportesCierreCaja();
        }
    }
}