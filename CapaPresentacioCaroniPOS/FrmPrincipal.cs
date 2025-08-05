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
using CapaDatos.TasaDia_Datos;

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
        bool menuUtilitariosExpandido = false;

        //L_CierreCaja _L_CierreCaja = new L_CierreCaja();
        private L_CierreCaja _L_CierreCaja = new L_CierreCaja();
        private D_ListaOrdenes _D_ListaOrdenes = new D_ListaOrdenes();
        private D_TasaSecuencia _D_TasaSecuencia = new D_TasaSecuencia();
        string Sucursal;

        private FrmClaveGerente _FrmClaveGerente = new FrmClaveGerente();

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
            _FrmInicio.Actualizar_Tasas(); 
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

            DateTime currentDate = _D_Inicio.DiaActivo();
            string formattedDate = currentDate.ToString("yyyyMMdd");

            //if (_L_CierreCaja.ObtieneAsistenciaPendiente(formattedDate, TB_USUARIO.COD_USR))
            if (!_FrmCierreDeCaja.BuscoAsistencia(TB_USUARIO.COD_USR))
            {
                //_FrmMensajes.co = 2;
                //_FrmMensajes.avisomensaje("Debe marcar asistencia para el día activo");
                //_FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                //_FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                //_FrmMensajes.ShowDialog();
                return;
            }
            if (!ValidarConfirmacionDivisas())
            {
                return;
            }
            if (!ValidarRecepTrnSol())
            {
                return;
            }
            string bloqFacturacion = _D_DetalleOrden.TB_PARAMETRO("FactEliminada");

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
            Sucursal = _D_DetalleOrden.TB_PARAMETRO("SucursalId");

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
            _FrmInicio.Actualizar_Tasas(); 
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

            DateTime currentDate = _D_Inicio.DiaActivo();
            string formattedDate = currentDate.ToString("yyyyMMdd");

            if (!_L_CierreCaja.ObtieneAsistenciaPendiente(formattedDate, TB_USUARIO.COD_USR))
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Debe marcar asistencia para el día activo");
                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                _FrmMensajes.ShowDialog();
                return;
            }

            PnlListadoOrdenes.Controls.Clear();
            addformulario(_FrmPagoMovil);
            _FrmPagoMovil.btnlupa_Click_1(this, EventArgs.Empty);
            Focus();
        }

        private void btnListaFactura_Click(object sender, EventArgs e)
        {
            _FrmInicio.Actualizar_Tasas(); 
            pnlUtilitarios.Visible = false; 
            System.Drawing.Color col2 = System.Drawing.ColorTranslator.FromHtml("#257b78");

            DateTime currentDate = _D_Inicio.DiaActivo();
            string formattedDate = currentDate.ToString("yyyyMMdd");

            if (!_L_CierreCaja.ObtieneAsistenciaPendiente(formattedDate, TB_USUARIO.COD_USR))
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Debe marcar asistencia para el día activo");
                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                _FrmMensajes.ShowDialog();
                return;
            }

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
            _FrmInicio.Actualizar_Tasas();
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

           
            DateTime currentDate = _D_Inicio.DiaActivo();
            string formattedDate = currentDate.ToString("yyyyMMdd");

            if (TB_USUARIO.COD_EMPLEADO != "99999")
            {
                if (!_L_CierreCaja.ObtieneAsistenciaPendiente(formattedDate, TB_USUARIO.COD_USR))
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("Debe marcar asistencia para el día activo");
                    _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                    _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                    _FrmMensajes.ShowDialog();
                    return;
                }

                if (!ValidarConfirmacionDivisas())
                {
                    return;
                }
                if (!ValidarRecepTrnSol())
                {
                    return;
                }
            }

            

            string StatusTasa = "";
            string StatusSec = "";

            DataSet dsConsTasa = _D_TasaSecuencia.TasaDia(_D_DetalleOrden.TB_PARAMETRO("SucursalId"), _D_Inicio.DiaActivo().ToString("yyyy/MM/dd"));

            for (int x = 0; x < dsConsTasa.Tables[0].Rows.Count; x++)
            {
                StatusTasa = (string)dsConsTasa.Tables[0].Rows[x]["Fecha_Activa_Ppal"];
                StatusSec = (string)dsConsTasa.Tables[0].Rows[x]["Fecha_Activa_PpalSec"];
            }
            if  (_D_DetalleOrden.TB_PARAMETRO("ActivarSecAdia") == "0")
            {
                StatusSec = "SI";
            }
            if (TB_USUARIO.COD_EMPLEADO != "99999")
            {

                if (StatusTasa != "SI" || StatusSec != "SI")
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("Debe actualizar la tasa de las monedas y activación de secuencia diaria");
                    _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                    _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                    _FrmMensajes.ShowDialog();
                    return;
                }
              if (!_L_CierreCaja.ObtieneAsistenciaPendiente(formattedDate, TB_USUARIO.COD_USR))
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("Debe marcar asistencia para el día activo");
                    _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                    _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                    _FrmMensajes.ShowDialog();
                    return;
                }
            }

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
            if (menuUtilitariosExpandido)
                ContraerMenuUtilitarios();
            else
                ExpandirMenuUtilitarios();
        }

        private void ExpandirMenuUtilitarios()
        {
            int desplazamiento = pnlUtilitarios.Height;

            pnlUtilitarios.Visible = true;

            button2.Top += desplazamiento;
            pictureBox4.Top += desplazamiento;

            btnconfiguracion.Top += desplazamiento;
            PicBoxConfigClaro.Top += desplazamiento;

            menuUtilitariosExpandido = true;
        }

        private void ContraerMenuUtilitarios()
        {
            int desplazamiento = pnlUtilitarios.Height;

            pnlUtilitarios.Visible = false;

            button2.Top -= desplazamiento;
            pictureBox4.Top -= desplazamiento;

            btnconfiguracion.Top -= desplazamiento;
            PicBoxConfigClaro.Top -= desplazamiento;

            menuUtilitariosExpandido = false;
        }

        private void btnCierredeCaja_Click(object sender, EventArgs e)
        {
            DateTime currentDate = _D_Inicio.DiaActivo();
            string formattedDate = currentDate.ToString("yyyyMMdd");

            if (TB_USUARIO.COD_EMPLEADO != "99999")
            {
                //Si no tiene asistencia marcada
            //    if (!_L_CierreCaja.ObtieneAsistenciaPendiente(formattedDate, TB_USUARIO.COD_USR))
            //{
            //    _FrmMensajes.co = 2;
            //    _FrmMensajes.avisomensaje("Debe marcar asistencia para el día activo");
            //    _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
            //    _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
            //    _FrmMensajes.ShowDialog();
            //    return;
            //}

            if (menuUtilitariosExpandido)
                ContraerMenuUtilitarios();

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
            DataSet Ordenesrango = _D_ListaOrdenes.CargarOrdPorRango(DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("yyyyMMdd"), "004", "", 1, 12);

            if (Ordenesrango.Tables[0].Rows.Count > 0)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("No puede cerrar la caja, hay ventas pendientes");
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
            }
            PnlListadoOrdenes.Controls.Clear();
            _FrmCierreDeCaja.CargarInicio();
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
            if (menuUtilitariosExpandido)
                ContraerMenuUtilitarios();
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
            if (menuUtilitariosExpandido)
                ContraerMenuUtilitarios();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pnlUtilitarios.Visible = false;

            FrmPrueba frmReportes = new FrmPrueba();

            frmReportes.ReportesCierreCaja(false);
        }

        // Variables de clase / ámbito
        //private bool ArchPend;
        //private DateTime glbFechaActiva;
        //private string glbSucursalActual;
        //private ICommand Command;   // Ajusta al tipo real de tu Command
        //private IManBD ManBD;       // Ajusta a tu tipo de acceso a datos

        public bool  ValidarConfirmacionDivisas()
        {
            // 1) Declaraciones iniciales
            double total = 0;
            string fechaMax = string.Empty;
            //DateTime currentDate = glbFechaActiva;

            // 2) Formatear fechas
            //string formattedDate = _D_DetalleOrden.TB_PARAMETRO("fechaultconfFac");
            // formattedDate2 = _D_DetalleOrden.TB_PARAMETRO("fechaultconfFac").AddDays(1).ToString("yyyy/MM/dd");

            DateTime currentDate = _D_Inicio.DiaActivo();
            //string formattedDate = currentDate.ToString("yyyyMMdd");
            string formattedDate = _D_DetalleOrden.TB_PARAMETRO("fechaultconfFac");
            string formattedDate2 = "";

            if (DateTime.TryParse(formattedDate, out DateTime fecha3))
            {
                formattedDate2 = fecha3.AddDays(1).ToString("yyyyMMdd");
            }

            string formattedDateFin = currentDate.AddDays(-1).ToString("yyyy/MM/dd");

            // 3) Llamar al SP para obtener el monto confirmado
            const string FACT = "FACT";
            //DataSet ds = ManBD.EjecutaStoreProcedure(
            //    "pGetRepRelacMonedaEx_MontoConfirma",
            //    $"{formattedDate2}','{formattedDateFin}','{glbSucursalActual}','{FACT}",
            //    Command
            //);
            // Sucursal = _D_DetalleOrden.TB_PARAMETRO("SucursalId");
            DataTable ds = _L_CierreCaja.RelacionMonedaEx(formattedDate2, formattedDateFin, Sucursal, FACT);

            // 4) Extraer el valor de Abo_Monto
            if ( ds.Rows.Count > 0)
            {
                var obj = ds.Rows[0]["Abo_Monto"];
                total = (obj == DBNull.Value) ? 0 : Convert.ToDouble(obj);
            }

            // 5) Si hay monto por confirmar, validar tiempo y monto
            if (total != 0)
            {
                fechaMax = _D_DetalleOrden.TB_PARAMETRO("fechaultconfFac");//  ValorParametro("fechaultconfFac", Command).ToString();
                int diaParametro = Convert.ToInt32(_D_DetalleOrden.TB_PARAMETRO("Cnf_Dv_Tiempo"));  //Convert.ToInt32(ValorParametro("Cnf_Dv_Tiempo", Command));
                DateTime fechaParametro = currentDate.AddDays(-diaParametro);
                DateTime fechaConfirmacionDivisas = Convert.ToDateTime(fechaMax);

                // Si la fecha límite ya pasó o el monto supera el umbral
                bool montoExcede = Convert.ToDouble(_D_DetalleOrden.TB_PARAMETRO("Cnf_Dv_Monto")) < total;
                bool fechaExpirada = fechaParametro.CompareTo(fechaConfirmacionDivisas) > 0;

                if (montoExcede || fechaExpirada)
                {
                    _FrmMensajes.co = 3;
                    _FrmMensajes.avisomensaje("Debe realizar la Confirmación de Divisas ¿Desea desbloquear el sistema?");
                    _FrmMensajes.ShowDialog();

                    if (_FrmMensajes.DialogResult == DialogResult.OK)
                    {
                        _FrmClaveGerente.Nuevo_Parametro = true;
                        _FrmClaveGerente.Parametro_Nuevo = _D_DetalleOrden.TB_PARAMETRO("UsClavDesbq");
                        _FrmClaveGerente.ShowDialog();
                        
                        if (_FrmClaveGerente.ClaveCorrecta == true)
                        {
                            return true;
                            //_D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "058", TB_USUARIO.COD_EMPLEADO, $"OS:  Altura D: {txtAltD.Text} Altura I: {txtAltI.Text} MVertical: {txtVertical.Text}, Autoriza: {TB_USUARIO.COD_EMPLEADO}");
                        }
                        else
                        {
                            return false;
                        }

                    }
                    else
                    {
                        return false;
                    }

                    //return true;
                    //if (opcion == DialogResult.Yes)
                    //{
                    //    using (var claveAuto = new FrmClaveDesbloqueo())
                    //    {
                    //        claveAuto.ShowDialog();
                    //        ArchPend = !claveAuto.Autorizadoo;
                    //    }
                    //}
                    //else
                    //{
                    //    ArchPend = true;
                    //}
                }
            }
            return true;
        }

        public bool ValidarRecepTrnSol()
        {
           DataTable ds = _L_CierreCaja.FechasTrnSol(Sucursal);

            DateTime Fecha_Bloqueo_Transferencias_Monturas = Convert.ToDateTime(ds.Rows[0]["Fecha_Tr_Montura"]);
            DateTime Fecha_Bloqueo_Transferencias_Lc = Convert.ToDateTime(ds.Rows[0]["Fecha_Tr_Lc"]);
            DateTime Fecha_Bloqueo_Solicitud_M = Convert.ToDateTime(ds.Rows[0]["Fecha_Soli_M"]);
            DateTime Fecha_Bloqueo_Solicitud_LC = Convert.ToDateTime(ds.Rows[0]["Fecha_Soli_Lc"]);

            // 2) Parámetros de bloqueo
            int parametroBloqueoTransferenciasMonturas = Convert.ToInt32(_D_DetalleOrden.TB_PARAMETRO("BloqueoTransMLS"));
            int parametroBloqueoTransferenciasLc = Convert.ToInt32(_D_DetalleOrden.TB_PARAMETRO("BloqueoTransLC"));
            int parametroBloqueoSolicitudM = Convert.ToInt32(_D_DetalleOrden.TB_PARAMETRO("BloqueoSolicMLS"));
            int parametroBloqueoSolicitudLc = Convert.ToInt32(_D_DetalleOrden.TB_PARAMETRO("BloqueoSolicLC"));

            DateTime glbFechaActiva = _D_Inicio.DiaActivo();

            int comparisonResult2 = glbFechaActiva.AddDays(-parametroBloqueoTransferenciasMonturas).CompareTo(Fecha_Bloqueo_Transferencias_Monturas);
            int comparisonResult3 = glbFechaActiva.AddDays(-parametroBloqueoTransferenciasLc).CompareTo(Fecha_Bloqueo_Transferencias_Lc);
            int comparisonResult4 = glbFechaActiva.AddDays(-parametroBloqueoSolicitudM).CompareTo(Fecha_Bloqueo_Solicitud_M);
            int comparisonResult5 = glbFechaActiva.AddDays(-parametroBloqueoSolicitudLc).CompareTo(Fecha_Bloqueo_Solicitud_LC);

            // 4) Validar bloqueo y mostrar diálogo
            if (comparisonResult2 > 0   || comparisonResult3 > 0     || comparisonResult4 > 0       || comparisonResult5 > 0)
            {
                _FrmMensajes.co = 3;
                _FrmMensajes.avisomensaje("Bloqueado por no recibir transferencias o solicitudes ¿Desea desbloquear el sistema?");
                _FrmMensajes.ShowDialog();

                _FrmClaveAutorizada.Nuevo_Parametro = true;
                _FrmClaveAutorizada.Parametro_Nuevo = _D_DetalleOrden.TB_PARAMETRO("Codigo_nomina");
                _FrmClaveAutorizada.ShowDialog();

                if (_FrmClaveAutorizada.DialogResult == DialogResult.OK && _FrmClaveAutorizada.ClaveCorrecta == true)
                {
                    _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "088", TB_USUARIO.COD_EMPLEADO, $"Autorizacion por Transferencias o Solicitudes Autorizado por:  {TB_USUARIO.COD_EMPLEADO}");

                    return true;
                }
                else
                {
                    return false;
                }

                
            }

            return true;
        }

    }
}