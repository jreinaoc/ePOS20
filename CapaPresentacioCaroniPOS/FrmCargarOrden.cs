using CapaEntidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaLogica.CargarOrdenes;
using CapaDatos.Conexion;
using System.Data.SqlClient;
using CapaDatos.Inicio_Datos;
using CapaDatos.DetalleOrden_Datos;
using CapaDatos.CargarOrdenes_Datos;
using CapaLogica.CargarClientes_Logica;
using CapaLogica.CargarOrdenes_Logica;
using DataGridViewNumericUpDownElements;
using System.Text.RegularExpressions;
using CapaLogica.Servicios;
using CapaDatos.Anulacion;
using System.Drawing; // Necesario para Font, Color, Pen
using System.Windows.Forms; // Necesario para DataGridView y DataGridViewCellPaintingEventArgs

namespace CapaVisual_Login
{
    public partial class FrmCargarOrden : Form
    {
        // Campo para almacenar el delegado de cierre
        private Action _onCierreSolicitado;
        private D_Articulos _D_Articulos  = new D_Articulos();

        // Método público para asignar el delegado desde el padre
        public void SetOnCierreSolicitado(Action onCierre)
        {
            _onCierreSolicitado = onCierre;
        }

        public FrmCargarOrden()
        {
            InitializeComponent();
            //mcll 13 06 25
            this.MaximumSize = new Size(0, 0); // Sin límite máximo
            this.MinimumSize = new Size(0, 0); // Sin límite mínimo

            _GuardarOrdenServ = new ServicioGuardarOrdenes_Cargar_Ordenes(_servicioValidaciones);

            // *** PASO CRÍTICO 1: Configurar el modo de edición del DataGridView ***
            // Esto asegura que la validación se dispare cuando el usuario escribe y luego intenta salir de la celda.
            Dgv_Pnl2_Querato.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2; // O DataGridViewEditMode.EditOnEnter

            // *** PASO CRÍTICO 2: Suscribir el evento CellValidating ***
            // Asegúrate de que este evento esté suscrito. Si ya lo hiciste en el diseñador,
            // esta línea puede ser redundante, pero no hace daño.

            // También es buena práctica limpiar el error al finalizar la edición
            Dgv_Pnl2_Querato.CellEndEdit += Dgv_Pnl2_Querato_CellEndEdit;


            // Asigna el evento KeyDown al TextBox de la cédula
            Txt_Tap1_Cedula.KeyDown += Txt_Tap1_Cedula_KeyDown;


            this.CargarCbx_fijos();

            Btn_Tap3_Procesar.BringToFront();

            btn_pln2_oft.BringToFront();
            btn_pln2_reti.BringToFront();
            btn_pln2_quer.BringToFront();

            LlenarCbx_Tap1_Estado();
            CargarCbx_Tap1_Nacionalidad();



            // Habilita la captura de eventos de teclado a nivel del formulario
            this.KeyPreview = true;
        }


        // Declarar la lista para almacenar los resultados
        private FrmRepOrden _FrmRepOrden = new FrmRepOrden();
        List<TB_ARTICULO> listaArticulos = new List<TB_ARTICULO>();
        FrmMostrarReporte _FrmMostrarReporte = new FrmMostrarReporte();
        private FrmClaveAutorizada _FrmClaveAutorizada = new FrmClaveAutorizada();
        private FrmClaveGerente _FrmClaveGerente = new FrmClaveGerente();
        private L_Articulo _L_Articulo = new L_Articulo();
        private FrmMensajes _FrmMensajes = new FrmMensajes();
        private D_Inicio _D_Inicio = new D_Inicio();
        public bool HabEliminar = false;
        private int filaSeleccionada;
        private string Tipo_Descuento = "";
        private string glbServicio_NUV = "";
        private string glbServicio = "";
        private string glbNumVision = "";
        private ServicioValidaciones_CargarOrdenes _servicioValidaciones = new ServicioValidaciones_CargarOrdenes();
        private ServicioGuardarOrdenes_Cargar_Ordenes _GuardarOrdenServ;
        private Asignar_Rx _Asignar_Rx = new Asignar_Rx();
        public bool Formulario_ListaOrdenes = false;
        private D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        private D_Articulos _D_Articulo = new D_Articulos();
        private string Codigo_Coloracion = "";
        private string Codigo_Promocion = "";
        private bool Promocion_Aplicada = false;
        private bool Cristal_Propio = false;
        private bool Montura_Propia = false;
        private string TipoMonturaPropia = "";
        private string EmpresaAfiliada = "";
        private decimal PorcDctoEmpresaAfiliada = 0;
        private bool Garantia;
        private string CodColorLC = "";
        private string Codmotivodes = "";
        private bool ApruebaAORangoCRT = false;
        private bool tipoTrabajoSeleccionado;
        private bool laboratorioSeleccionado;
        private bool servicioSeleccionado;
        private string Codigo_Servicio_Agregar = "";
        private string Os_Garantia_Trabajo = "";
        private string Numero_Examen_Garantia_Trabajo = "";
        public bool Formato_Claro = true;
        // Variables para guardar los datos recibidos del delegado Lentes de Contacto 
        string codLab = "";
        string generico = "";
        private BindingSource bindingSource = new BindingSource();
        // Lista temporal para relizar el filtrado 
        private List<TB_ARTICULO> listaTemporal = new List<TB_ARTICULO>();
        private List<TB_TRABAJO> _TRABAJO = new List<TB_TRABAJO>();
        List<TB_EMPAFI> listaClienteAfiliados = new List<TB_EMPAFI>();
        List<TB_EMPAFI> listaTemporalClienteAfiliados = new List<TB_EMPAFI>();
        List<FechaHoraOfrecida> _FechaHoraOfrecida = new List<FechaHoraOfrecida>();
        private string mensaje = "";
        private bool validandoCambioTab = false; // Variable de control
        /*MEIFER*/
        string ValidarPanel;
        int TopeExamen;
        int numeroExamen;
        // Declarar la lista para almacenar los resultados
        //List<TB_ARTICULO> listaArticulos = new List<TB_ARTICULO>();
        // Lista temporal para relizar el filtrado 
        //private List<TB_ARTICULO> listaTemporal = new List<TB_ARTICULO>();
        //private L_Articulo _L_Articulo = new L_Articulo();
        //private FrmMensajes _FrmMensajes = new FrmMensajes();
        string resultado = "";
        string resultadoTelefono = "";
        DataTable dtCliente;
        //mcll 15-04-25
        private L_Cliente _L_Cliente = new L_Cliente();
        private L_Cliente l_Cliente = new L_Cliente();
        private L_Laboratorio _L_Laboratorio = new L_Laboratorio();
        private L_ServicioLab _servicioLogica = new L_ServicioLab();



        private L_Ficcont _L_Ficcont = new L_Ficcont(); // Declaración e inicialización
        private L_Ficconv _L_Ficconv = new L_Ficconv(); // Declaración e inicialización

        private L_Querato _L_Querato = new L_Querato(); // Declaración e inicialización

        private L_Trabajo _L_Trabajo = new L_Trabajo(); // Declaración e inicialización

        private CapaLogica.CargarOrdenes_Logica.L_Examen _L_Examen = new CapaLogica.CargarOrdenes_Logica.L_Examen(); // Especifica el namespace completo


        //        private L_Cliente _L_Cliente = new L_Cliente(); // Instancia de la capa lógica


        private List<TB_CTEPPAL> listaDeClientes = new List<TB_CTEPPAL>();
        private List<TB_CTEPPAL> listaTemporalClientes = new List<TB_CTEPPAL>();
        //   private ClienteLogica CargarClientes_Logica = new ClienteLogica(); // Instancia de tu capa de lógica
        //private CargarClientes_Logica clienteLogica = new CargarClientes_Logica(); // Instancia de tu capa de lógica

        private bool _teclaF2Presionada = false; // Variable para rastrear si se presionó F2

        /// <summary>
        // Crear una instancia de la entidad TB_CTEPPAL para almacenar los datos

        TB_EXAMENCTE nuevoExamen = new TB_EXAMENCTE();

        TB_FICCONT nuevoFiccont = new TB_FICCONT();
        TB_FICCONVCTE nuevoFicconv = new TB_FICCONVCTE();

        TB_TRABAJOCTE nuevoTrabajo = new TB_TRABAJOCTE(); // Ahora el compilador debería encontrar la clase
        TB_QUERATO nuevoQuerato = new TB_QUERATO();
        // Variables a nivel de clase para almacenar los límites de validación
        private decimal minMeridianoCorneal = 6M; // Usa decimal si tus valores son monetarios o precisos
        private decimal maxMeridianoCorneal = 10M;

        //private D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        public DataTable dtClienteconGarantia;
        private bool _isCellValueChanging = false;

        private bool mantengoexamenseleccionado;

        private bool mantenervacio;

        private string codigoSucursal;
        D_Anulacion _D_Anulacion = new D_Anulacion();
        private string ojoLenteContacto;
        private bool LcAmbosCant1 = false;

        private void DgvListadoOrdenes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DgvListadoOrdenes_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

        }

        private void Txt_Tap3_Articulo_Codigo_Enter(object sender, EventArgs e)
        {
            // Cuando el usuario hace clic o intenta escribir
            if (Txt_Tap3_Articulo_Codigo.Text == "Código")
            {
                Txt_Tap3_Articulo_Codigo.Text = ""; // Borrar el texto sugerido
                Txt_Tap3_Articulo_Codigo.ForeColor = Color.Black; // Cambiar el color del texto a negro

            }
        }

        private void Txt_Tap3_Articulo_Codigo_Leave(object sender, EventArgs e)
        {
            // Cuando el usuario deja el TextBox
            if (string.IsNullOrWhiteSpace(Txt_Tap3_Articulo_Codigo.Text))
            {
                //Txt_Tap3_Articulo_Codigo.Text = "Código"; // Restaurar el texto sugerido
                //Txt_Tap3_Articulo_Codigo.ForeColor = Color.DarkGray; // Cambiar el color del texto a gris

                //Txt_Tap3_Articulo_Descripcion.Text = "Descripción";
                //Txt_Tap3_Articulo_Descripcion.ForeColor = Color.DarkGray;
                ReiniciarBusquedaarticulo();
            }
        }

        public void FormatoDataGrid_Oscuro_Dgv_Tap3_Articulo(System.Drawing.Color col2, System.Drawing.Color col3, System.Drawing.Color col4)
        {

            this.BackColor = col2;
            //LblListadoFactura.ForeColor = Color.White;
            //Lbldesde.ForeColor = Color.White;
            //LblHasta.ForeColor = Color.White;

            //Con esta funcion coloreamos el grid del color oscuro 
            Dgv_Tap3_Articulo.BackgroundColor = col3;
            Dgv_Tap3_Articulo.DefaultCellStyle.BackColor = col3;
            Dgv_Tap3_Articulo.ColumnHeadersDefaultCellStyle.BackColor = col4;
            Dgv_Tap3_Articulo.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            Dgv_Tap3_Articulo.DefaultCellStyle.ForeColor = Color.White;

        }

        public void FormatoDataGrid_Claro_Dgv_Tap3_Articulo(System.Drawing.Color col1, System.Drawing.Color col3)
        {

            this.BackColor = col1;
            //LblListadoFactura.ForeColor = Color.Black;
            //Lbldesde.ForeColor = Color.Black;
            //LblHasta.ForeColor = Color.Black;

            //Con esta funcion coloreamos el grid del fondo blanco 
            Dgv_Tap3_Articulo.BackgroundColor = col1;
            Dgv_Tap3_Articulo.DefaultCellStyle.BackColor = col1;
            Dgv_Tap3_Articulo.ColumnHeadersDefaultCellStyle.BackColor = col3;
            Dgv_Tap3_Articulo.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(30, 30, 30, 30);
            Dgv_Tap3_Articulo.DefaultCellStyle.ForeColor = Color.Black;

        }

        private void Txt_Tap3_Articulo_Descripcion_Enter(object sender, EventArgs e)
        {
            // Cuando el usuario hace clic o intenta escribir
            if (Txt_Tap3_Articulo_Descripcion.Text == "Descripción")
            {
                Txt_Tap3_Articulo_Descripcion.Text = ""; // Borrar el texto sugerido
                Txt_Tap3_Articulo_Descripcion.ForeColor = Color.Black; // Cambiar el color del texto a negro
            }
        }

        private void Txt_Tap3_Articulo_Descripcion_Leave(object sender, EventArgs e)
        {
            // Cuando el usuario deja el TextBox
            if (string.IsNullOrWhiteSpace(Txt_Tap3_Articulo_Descripcion.Text))
            {
                Txt_Tap3_Articulo_Descripcion.Text = "Descripción"; // Restaurar el texto sugerido
                Txt_Tap3_Articulo_Descripcion.ForeColor = Color.DarkGray; // Cambiar el color del texto a gris
            }
        }

        private void Txt_Tap3_Articulo_Cantidad_Enter(object sender, EventArgs e)
        {
            // Cuando el usuario hace clic o intenta escribir
            if (Txt_Tap3_Articulo_Cantidad.Text == "Cantidad")
            {
                Txt_Tap3_Articulo_Cantidad.Text = ""; // Borrar el texto sugerido
                Txt_Tap3_Articulo_Cantidad.ForeColor = Color.Black; // Cambiar el color del texto a negro
            }
        }

        private void Txt_Tap3_Articulo_Cantidad_Leave(object sender, EventArgs e)
        {
            // Cuando el usuario deja el TextBox
            if (string.IsNullOrWhiteSpace(Txt_Tap3_Articulo_Cantidad.Text))
            {
                Txt_Tap3_Articulo_Cantidad.Text = "Cantidad"; // Restaurar el texto sugerido
                Txt_Tap3_Articulo_Cantidad.ForeColor = Color.DarkGray; // Cambiar el color del texto a gris

                
            }
        }

        private void Txt_Tap3_Articulo_Precio_Leave(object sender, EventArgs e)
        {
            // Cuando el usuario deja el TextBox
            if (string.IsNullOrWhiteSpace(Txt_Tap3_Articulo_Precio.Text))
            {
                Txt_Tap3_Articulo_Precio.Text = "Precio"; // Restaurar el texto sugerido
                Txt_Tap3_Articulo_Precio.ForeColor = Color.DarkGray; // Cambiar el color del texto a gris
            }
        }

        private void Txt_Tap3_Articulo_Cantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            //para que solo acepte numeros
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
            }

            //validar que no sea la tecla de borrar 
            if (e.KeyChar != (char)8)
            {
                if (Txt_Tap3_Articulo_Cantidad.Text.Length == 4)
                {
                    Txt_Tap3_Articulo_Cantidad.Focus();
                }
            }
        }

        private void btnCancelar3_Click(object sender, EventArgs e)
        {
            VisualizarPanel("MostrarCabezeraSecundaria");
            HabilitacionControl("CabezeraPrincipal");
            LimpiarControles("Motro_Busqueda_Articulos");
            LimpiarControles("Carga_Articulos");
            HabilitacionControl("Bloquear_Lista_Articulo");
            Txt_Tap3_Articulo_Cantidad.Focus();
        }

        private void LimpiarControles(string Case)
        {
            switch (Case)
            {
                case "Motro_Busqueda_Articulos":
                    this.Txt_Pnl3_Articulo.Text = "";
                    Txt_Tap3_Articulo_Descripcion.Text = "";
                    Txt_Tap3_Articulo_Cantidad.Text = "";
                    Txt_Tap3_Articulo_Precio.Text = "";
                    listaArticulos.Clear();
                    listaTemporal.Clear();
                    Dgv_Pnl3_Articulo.DataSource = null;
                    break;

                case "Abrir_Busqueda_Articulos":
                    this.Rd_Pnl3_Codigo.Checked = true;
                    this.Txt_Pnl3_Articulo.Text = "";
                    break;


                case "Carga_Articulos":
                     ReiniciarBusquedaarticulo();
                    break;

                case "CambioPrecio":
                    this.Txt_Pnl3_CambioPrecioActual.Text = "";
                    Txt_Pnl3_CambioPrecioNuevo.Text = "";
                    break;

                case "Descuento":
                    this.Txt_Pnl3_PorcDescuento.Text = "";
                    this.Txt_Pnl3_MontoDesc.Text = "";
                    this.Txt_Pnl3_MontoDesc.Text = "";
                    this.Txt_Pnl3_ObservacionDesc.Text = "";
                    //this.Btn_Tap3_Descuento.BackColor = Color.LightCoral;
                    //Cbx_Pnl3_MotivoDesc.SelectedIndex = 0;
                    break;

                case "Coloracion":
                    this.Rd_Pnl3_FullColor.Checked = false;
                    this.Rd_Pnl3_Degradado.Checked = false;
                    this.Dgv_Pnl3_Coloracion.DataSource = null;
                    break;

                case "MonturaPropia":
                    rbCompleta.Checked = false;
                    rbRanurada.Checked = false;
                    rbAlaire.Checked = false;
                    

                    break;
                case "CristalPropio":
                    this.Btn_Tap3_CristalPropio.BackColor = Color.GreenYellow;

                    break;

                case "ClienteAfiliado":
                    //this.Btn_Tap3_ClienteAfiliado.BackColor = Color.LightCoral;
                    Rd_Pnl3_CteAfiliadoCodigo.Checked = false;
                    Rd_Pnl3_CteAfiliadoDesc.Checked = false;
                    Txt_Pnl3_ClienteAfiliado.Text = "";
                    break;

                case "Promociones":
                    Dgv_Pnl3_Promociones.DataSource = null;
                    break;

                default:
                    break;
            }


        }

        public void VisualizarPanel(string Case)
        {

            switch (Case)
            {
                case "MostrarCabezeraPrincipal":
                    this.Pnl_2.Enabled = false;
                    this.Pnl_2.Visible = false;
                    this.Pnl_1.Visible = true;
                    this.Pnl_1.Enabled = true;
                    this.Pnl_3_Lista_Articulo.Enabled = false;
                    this.Pnl_3_Lista_Articulo.Visible = false;
                    this.Pnl_3_CambioPrecio.Enabled = false;
                    this.Pnl_3_CambioPrecio.Visible = false;
                    this.Pnl_3_Descuento.Enabled = false;
                    this.Pnl_3_Descuento.Visible = false;
                    this.Pnl_3_Coloración.Visible = false;
                    this.Pnl_3_Coloración.Enabled = false;
                    this.Pnl_3_Garantia.Visible = false;
                    this.Pnl_3_Garantia.Enabled = false;
                    
                    break;

                case "MostrarCabezeraSecundaria":
                    this.Pnl_2.Enabled = true;
                    this.Pnl_2.Visible = true;
                    this.Pnl_1.Visible = false;
                    this.Pnl_1.Enabled = false;
                    //this.Pnl_3_Lista_Articulo.Enabled = false;
                    this.Pnl_3_Lista_Articulo.Visible = false;
                    this.Pnl_3_CambioPrecio.Enabled = false;
                    this.Pnl_3_CambioPrecio.Visible = false;
                    this.Pnl_3_Descuento.Enabled = false;
                    this.Pnl_3_Descuento.Visible = false;
                    this.Pnl_3_Coloración.Visible = false;
                    this.Pnl_3_Coloración.Enabled = false;
                    this.Pnl_3_Promociones.Visible = false;
                    this.Pnl_3_Promociones.Enabled = false;
                    //this.pnl_MonturaPropia.Enabled = false;
                    this.pnl_MonturaPropia.Visible = false;
                    this.Pnl_3_Lista_ClienteAfiliado.Visible = false;
                    this.Pnl_3_Lista_ClienteAfiliado.Enabled = false;
                    this.Pnl_3_Garantia.Visible = false;
                    this.Pnl_3_Garantia.Enabled = false;
                    this.Pnl_2.Location = new Point(0, 0); // Establecer posición en (0, 0)

                    Cbx_Pnl2_Trbajo.Visible = true;
                    Lbl_Pnl2_Trabajo.Visible = true;
                    Lbl_Pnl2_Laboratorio.Visible = true;
                    Cbx_Pnl2_Laboratorio.Visible = true;
                    Lbl_Pnl2_Servicio.Visible = true;
                    Cbx_Pnl2_Servicio.Visible = true;
                    //Txt_Pnl2_Examen.Visible = true;
                    //Lbl_Pnl2_Num_Examen.Visible = true;
                    Cbx_Tap2_Ojo.Visible = false;
                    label24.Visible = false;
                    Txt_Pnl2_Fecha_Ofre.Visible = true;
                    Lbl_Pnl2_Fecha_Ofre.Visible = true;
                    Lbl_Pnl2_Carga_Art.Text = "Carga de orden";
                    Lbl_Pnl2_Carga_Art.Visible = true;
                    Txt_Pnl2_Examen.Text = Txt_Tap2_Examen.Text;

                    label24.Visible = true;
                    Cbx_Tap2_Ojo.Visible = true;
                    label26.Visible = true;
                    cbVisionDerecha.Visible = true;
                    label33.Visible = true;
                    cbVisionIzquierda.Visible = true;
                    
                    label34.Visible = true;
                    label35.Visible = true;
                    txtAltD.Visible = true;
                    txtAltI.Visible = true;

                    mantengoexamenseleccionado = true;
                    break;
                case "MostrarCabeceraExamen":

                    Txt_Pnl2_Cedula.Text = Cbx_Tap1_Nacionalidad.Text.Trim() + "-" + Txt_Tap1_Cedula.Text.Trim();
                    Txt_Pnl_2_Nombre.Text = Txt_Tap1_Nombre.Text;

                    this.Pnl_2.Enabled = true;
                    this.Pnl_2.Visible = true;
                    this.Pnl_1.Visible = false;
                    this.Pnl_1.Enabled = false;
                    this.Pnl_3_Lista_Articulo.Enabled = false;
                    this.Pnl_3_Lista_Articulo.Visible = false;
                    this.Pnl_3_CambioPrecio.Enabled = false;
                    this.Pnl_3_CambioPrecio.Visible = false;
                    this.Pnl_3_Descuento.Enabled = false;
                    this.Pnl_3_Descuento.Visible = false;
                    this.Pnl_3_Coloración.Visible = false;
                    this.Pnl_3_Coloración.Enabled = false;
                    this.Pnl_3_Promociones.Visible = false;
                    this.Pnl_3_Promociones.Enabled = false;
                    this.pnl_MonturaPropia.Enabled = false;
                    this.pnl_MonturaPropia.Visible = false;
                    this.Pnl_3_Lista_ClienteAfiliado.Visible = false;
                    this.Pnl_3_Lista_ClienteAfiliado.Enabled = false;
                    this.Pnl_3_Garantia.Visible = false;
                    this.Pnl_3_Garantia.Enabled = false;
                    this.Pnl_2.Location = new Point(5, 0); // Establecer posición en (0, 0)

                    Cbx_Pnl2_Trbajo.Visible = false;
                    Lbl_Pnl2_Trabajo.Visible = false;
                    Lbl_Pnl2_Laboratorio.Visible = false;
                    Cbx_Pnl2_Laboratorio.Visible = false;
                    Lbl_Pnl2_Servicio.Visible = false;
                    Cbx_Pnl2_Servicio.Visible = false;
                    Txt_Pnl2_Examen.Visible = false;
                    
                    Cbx_Tap2_Ojo.Visible = false;
                    label24.Visible = false;
                    Txt_Pnl2_Fecha_Ofre.Visible = false;
                    Lbl_Pnl2_Fecha_Ofre.Visible = false;
                    Lbl_Pnl2_Carga_Art.Text = "Datos del Cliente";
                    //Lbl_Pnl2_Carga_Art.Visible = false;

                    label24.Visible = false;
                    Cbx_Tap2_Ojo.Visible = false;
                    label26.Visible = false;
                    cbVisionDerecha.Visible = false;
                    label33.Visible = false;
                    cbVisionIzquierda.Visible = false;

                    label34.Visible = false;
                    label35.Visible = false;
                    txtAltD.Visible = false;
                    txtAltI.Visible = false;

                    break;
                case "Lista_Articulo":
                    this.Pnl_3_Lista_Articulo.Enabled = true;
                    this.Pnl_3_Lista_Articulo.Visible = true;
                    this.Pnl_3_Lista_Articulo.Location = new Point(250, 1);
                    this.Pnl_3_Lista_Articulo.BringToFront();
                    break;

                case "CambioPrecio":
                    this.Pnl_3_CambioPrecio.Enabled = true;
                    this.Pnl_3_CambioPrecio.Visible = true;
                    this.Pnl_3_CambioPrecio.Location = new Point(400, 1);
                    this.Pnl_3_CambioPrecio.BringToFront();
                    break;

                case "Descuento":
                    this.Pnl_3_Descuento.Enabled = true;
                    this.Pnl_3_Descuento.Visible = true;
                    this.Pnl_3_Descuento.Location = new Point(300, 1);
                    this.Pnl_3_Descuento.BringToFront();
                    Lbl_Pnl3_Descuento.Text = Tipo_Descuento;
                    break;

                case "Coloracion":
                    this.Pnl_3_Coloración.Enabled = true;
                    this.Pnl_3_Coloración.Visible = true;
                    this.Pnl_3_Coloración.Location = new Point(300, 1);
                    this.Pnl_3_Coloración.BringToFront();
                    break;

                case "MonturaPropia":
                    this.pnl_MonturaPropia.Location = new Point(300, 30);
                    this.pnl_MonturaPropia.BringToFront();
                    this.pnl_MonturaPropia.Visible = true;
                    this.pnl_MonturaPropia.Enabled = true;
                    break;

                case "ClienteAfiliado":
                    this.Pnl_3_Lista_ClienteAfiliado.Location = new Point(300, 30);
                    this.Pnl_3_Lista_ClienteAfiliado.BringToFront();
                    this.Pnl_3_Lista_ClienteAfiliado.Visible = true;
                    this.Pnl_3_Lista_ClienteAfiliado.Enabled = true;
                    break;

                case "Promociones":
                    this.Pnl_3_Promociones.Enabled = true;
                    this.Pnl_3_Promociones.Visible = true;
                    this.Pnl_3_Promociones.Location = new Point(300, 1);
                    this.Pnl_3_Promociones.BringToFront();
                    break;
                  
                case "Garantia":
                    this.Pnl_3_Garantia.Enabled = true;
                    this.Pnl_3_Garantia.Visible = true;
                    this.Pnl_3_Garantia.Location = new Point(20, 30);
                    this.Pnl_3_Garantia.BringToFront();
                    break;

                default:
                    break;
            }


        }
       
        public void HabilitacionControl(string Case)
        {
            switch (Case)
            {
                case "Habilitar_Lista_Articulo":
                    // Panel descuento 
                    this.Pnl_3_Descuento.Enabled = false;
                    //Panel Ingresar Articulo
                    this.Pnl_1_Tap3.Enabled = false;
                    //Grid Carga Articulo
                    this.Dgv_Tap3_Articulo.Enabled = false;
                    //Panel de Medidas Montuta y Observacion
                    this.Pnl_2_Tap3.Enabled = false;
                    //Panel de Totales
                    this.Pnl_3_Tap3.Enabled = false;
                    //Grid Montutas
                    //this.Dgv_Tap3_Medidas_Montura.Enabled = false;
                    //Boton Cancelar Formulario Principal Articulo 
                    this.Btn_Tap3_Cancelar.Enabled = false;
                    //Boton Procesar Formulario Principal Articulo 
                    this.Btn_Tap3_Procesar.Enabled = false;

                    this.Dgv_Pnl3_Articulo.Enabled = true;
                    this.Txt_Pnl3_Articulo.Enabled = true;
                    this.Rd_Pnl3_Descripcion.Enabled = true;
                    this.Rd_Pnl3_Codigo.Enabled = true;
                    this.Rd_Pnl3_Descripcion.Checked = false;
                    //Lo comente porque cuando se guarda un examen y se carga una orden por algun motivo cuando se selecciona el articulo dispara el evento examen click y hace que se devuelva al examen
                    //this.btnCancelar3.Enabled = true;

                    // Botones del TapControl
                    //this.btnPrincipal.Enabled = false;
                    //this.btnExamen.Enabled = false;
                    //this.btnCargarOrden.Enabled = false;

                    // Botones Aciones 
                    this.Btn_Tap3_Descuento.Enabled = false;
                    this.Btn_Tap3_CambioPrecio.Enabled = false;
                    this.Btn_Tap3_Promocion.Enabled = false;
                    this.Btn_Tap3_MonturaPropia.Enabled = false;
                    this.Btn_Tap3_CristalPropio.Enabled = false;
                    this.Btn_Tap3_ClienteAfiliado.Enabled = false;
                    this.Btn_Tap3_Garantia.Enabled = false;

                    // Panel de Arriba
                    this.Txt_Pnl2_Cedula.Enabled = false;
                    this.Txt_Pnl2_Examen.Enabled = false;
                    this.Cbx_Pnl2_Trbajo.Enabled = false;
                    this.Cbx_Pnl2_Laboratorio.Enabled = false;
                    this.Cbx_Tap2_Ojo.Enabled = false;
                    this.Txt_Pnl2_Examen.Enabled = false;
                    this.Cbx_Pnl2_Servicio.Enabled = false;

                    break;

                case "CabezeraPrincipal":
                    //// Panel descuento 
                    //this.Pnl_3_Descuento.Enabled = false;

                    //// Panel cliente afiliado 
                    //this.Pnl_3_Lista_ClienteAfiliado.Enabled = false;

                    //// Panel Montura propia 
                    //this.pnl_MonturaPropia.Enabled = false;

                    ////Panel de coloracion
                    //this.Pnl_3_Coloración.Enabled = false;

                    // Controles del Panel Coloración 
                    this.Pnl_3_RadioButonColoracion.Enabled = false;
                    this.Rd_Pnl3_FullColor.Enabled = false;
                    this.Rd_Pnl3_Degradado.Enabled = false;
                    this.Dgv_Pnl3_Coloracion.Enabled = false;
                    this.Btn_Tap3_Cancelar_Coloracion.Enabled = false;
                    this.Btn_Tap3_Aceptar_Coloracion.Enabled = false;

                    // Controles del Panel Promocion
                    this.Dgv_Pnl3_Promociones.Enabled = false;
                    this.Btn_Tap3_Cancelar_Promo.Enabled = false;
                    this.Btn_Tap3_Aceptar_Promo.Enabled = false;

                    //Panel Cambio Precio
                    this.Pnl_3_CambioPrecio.Enabled = false;
                    this.Pnl_1_Tap3.Enabled = true;
                    this.Dgv_Tap3_Articulo.Enabled = true;
                    this.Pnl_2_Tap3.Enabled = true;
                    this.Pnl_3_Tap3.Enabled = true;
                    //this.Dgv_Tap3_Medidas_Montura.Enabled = true;
                    this.Btn_Tap3_Cancelar.Enabled = true;
                    this.Btn_Tap3_Procesar.Enabled = true;

                    this.Dgv_Pnl3_Articulo.Enabled = false;
                    this.Txt_Pnl3_Articulo.Enabled = false;
                    this.Rd_Pnl3_Descripcion.Enabled = false;
                    this.Rd_Pnl3_Codigo.Enabled = false;
                    
                    //Lo comente porque cuando se guarda un examen y se carga una orden por algun motivo cuando se selecciona el articulo dispara el evento examen click y hace que se devuelva al examen
                    //this.btnCancelar3.Enabled = false;

                    //this.btnPrincipal.Enabled = true;

                    //this.btnExamen.Enabled = true;
                    this.btnCargarOrden.Enabled = true;

                    // Botones Aciones 
                    ValidarRegistrosYHabilitar_Botones();

                    // Panel de Arriba
                    //this.Txt_Pnl2_Cedula.Enabled = true;
                    this.Txt_Pnl2_Examen.Enabled = true;
                    //this.Cbx_Pnl2_Trbajo.Enabled = true;
                    //this.Cbx_Pnl2_Laboratorio.Enabled = true;
                    //this.Cbx_Pnl2_Servicio.Enabled = true;
                    break;

                case "Habilitar_CambioPrecio":
                    //Panel Cambio Precio
                    this.Pnl_3_CambioPrecio.Enabled = true;

                    // Panel descuento 
                    this.Pnl_3_Descuento.Enabled = false;

                    //Panel Ingresar Articulo
                    this.Pnl_1_Tap3.Enabled = false;
                    //Grid Carga Articulo
                    this.Dgv_Tap3_Articulo.Enabled = false;
                    //Panel de Medidas Montuta y Observacion
                    this.Pnl_2_Tap3.Enabled = false;
                    //Panel de Totales
                    this.Pnl_3_Tap3.Enabled = false;
                    //Grid Montutas
                    //this.Dgv_Tap3_Medidas_Montura.Enabled = false;
                    //Boton Cancelar Formulario Principal Articulo 
                    this.Btn_Tap3_Cancelar.Enabled = false;
                    //Boton Procesar Formulario Principal Articulo 
                    this.Btn_Tap3_Procesar.Enabled = false;

                    // Panel de Arriba
                    this.Txt_Pnl2_Cedula.Enabled = false;
                    this.Txt_Pnl2_Examen.Enabled = false;
                    this.Cbx_Pnl2_Trbajo.Enabled = false;
                    this.Cbx_Pnl2_Laboratorio.Enabled = false;
                    this.Cbx_Tap2_Ojo.Enabled = false;
                    this.Txt_Pnl2_Examen.Enabled = false;
                    this.Cbx_Pnl2_Servicio.Enabled = false;


                    // Controles del Panel Cambio Precio
                    this.Txt_Pnl3_CambioPrecioNuevo.Enabled = true;
                    this.Btn_Tap3_Cancelar_CambioPrecio.Enabled = true;
                    this.Btn_Tap3_Aceptar_CambioPrecio.Enabled = true;

                    // Botones del TapControl
                    //this.btnPrincipal.Enabled = false;
                    //this.btnExamen.Enabled = false;
                    //this.btnCargarOrden.Enabled = false;

                    // Botones Aciones 
                    this.Btn_Tap3_Descuento.Enabled = false;
                    this.Btn_Tap3_CambioPrecio.Enabled = false;
                    this.Btn_Tap3_Promocion.Enabled = false;
                    this.Btn_Tap3_MonturaPropia.Enabled = false;
                    this.Btn_Tap3_CristalPropio.Enabled = false;
                    this.Btn_Tap3_ClienteAfiliado.Enabled = false;
                    this.Btn_Tap3_Garantia.Enabled = false;

                    break;

                case "Habilitar_Descuento":

                    // Panel descuento 
                    this.Pnl_3_Descuento.Enabled = true;

                    //Panel Cambio Precio
                    this.Pnl_3_CambioPrecio.Enabled = false;

                    //Panel Ingresar Articulo
                    this.Pnl_1_Tap3.Enabled = false;
                    //Grid Carga Articulo
                    this.Dgv_Tap3_Articulo.Enabled = false;
                    //Panel de Medidas Montuta y Observacion
                    this.Pnl_2_Tap3.Enabled = false;
                    //Panel de Totales
                    this.Pnl_3_Tap3.Enabled = false;
                    //Grid Montutas
                    //this.Dgv_Tap3_Medidas_Montura.Enabled = false;
                    //Boton Cancelar Formulario Principal Articulo 
                    this.Btn_Tap3_Cancelar.Enabled = false;
                    //Boton Procesar Formulario Principal Articulo 
                    this.Btn_Tap3_Procesar.Enabled = false;

                    // Panel de Arriba
                    this.Txt_Pnl2_Cedula.Enabled = false;
                    this.Txt_Pnl2_Examen.Enabled = false;
                    this.Cbx_Pnl2_Trbajo.Enabled = false;
                    this.Cbx_Pnl2_Laboratorio.Enabled = false;
                    this.Cbx_Tap2_Ojo.Enabled = false;
                    this.Cbx_Pnl2_Servicio.Enabled = false;


                    // Controles del Panel Cambio Precio
                    this.Txt_Pnl3_CambioPrecioNuevo.Enabled = false;
                    this.Btn_Tap3_Cancelar_CambioPrecio.Enabled = false;
                    this.Btn_Tap3_Aceptar_CambioPrecio.Enabled = false;

                    // Controles del Panel descuento
                    this.Txt_Pnl3_PorcDescuento.Enabled = true;
                    this.Txt_Pnl3_MontoDesc.Enabled = true;
                    this.Txt_Pnl3_ObservacionDesc.Enabled = true;
                    this.Cbx_Pnl3_MotivoDesc.Enabled = true;
                    this.Btn_Tap3_Cancelar_Desc.Enabled = true;
                    this.Btn_Tap3_Aceptar_Desc.Enabled = true;

                    // Botones del TapControl
                    //this.btnPrincipal.Enabled = false;
                    //this.btnExamen.Enabled = false;
                    //this.btnCargarOrden.Enabled = false;

                    // Botones Aciones 
                    this.Btn_Tap3_Descuento.Enabled = false;
                    this.Btn_Tap3_CambioPrecio.Enabled = false;
                    this.Btn_Tap3_Promocion.Enabled = false;
                    this.Btn_Tap3_MonturaPropia.Enabled = false;
                    this.Btn_Tap3_CristalPropio.Enabled = false;
                    this.Btn_Tap3_ClienteAfiliado.Enabled = false;
                    this.Btn_Tap3_Garantia.Enabled = false;

                    break;

                case "Habilitar_Coloracion":

                    // Panel Coloración 
                    this.Pnl_3_Coloración.Enabled = true;

                    //Panel Ingresar Articulo
                    this.Pnl_1_Tap3.Enabled = false;
                    //Grid Carga Articulo
                    this.Dgv_Tap3_Articulo.Enabled = false;
                    //Panel de Medidas Montuta y Observacion
                    this.Pnl_2_Tap3.Enabled = false;
                    //Panel de Totales
                    this.Pnl_3_Tap3.Enabled = false;
                    //Grid Montutas
                    //this.Dgv_Tap3_Medidas_Montura.Enabled = false;
                    //Boton Cancelar Formulario Principal Articulo 
                    this.Btn_Tap3_Cancelar.Enabled = false;
                    //Boton Procesar Formulario Principal Articulo 
                    this.Btn_Tap3_Procesar.Enabled = false;

                    // Panel de Arriba
                    this.Txt_Pnl2_Cedula.Enabled = false;
                    this.Txt_Pnl2_Examen.Enabled = false;
                    this.Cbx_Pnl2_Trbajo.Enabled = false;
                    this.Cbx_Pnl2_Laboratorio.Enabled = false;
                    this.Cbx_Tap2_Ojo.Enabled = false;
                    this.Txt_Pnl2_Examen.Enabled = false;
                    this.Cbx_Pnl2_Servicio.Enabled = false;

                    // Controles del Panel Coloración 
                    this.Pnl_3_RadioButonColoracion.Enabled = true;
                    this.Rd_Pnl3_FullColor.Enabled = true;
                    this.Rd_Pnl3_FullColor.Checked = true;
                    this.Rd_Pnl3_Degradado.Enabled = true;
                    this.Dgv_Pnl3_Coloracion.Enabled = true;
                    this.Btn_Tap3_Cancelar_Coloracion.Enabled = true;
                    this.Btn_Tap3_Aceptar_Coloracion.Enabled = true;

                    // Botones del TapControl
                    //this.btnPrincipal.Enabled = false;
                    //this.btnExamen.Enabled = false;
                    //this.btnCargarOrden.Enabled = false;

                    // Botones Aciones 
                    this.Btn_Tap3_Descuento.Enabled = false;
                    this.Btn_Tap3_CambioPrecio.Enabled = false;
                    this.Btn_Tap3_Promocion.Enabled = false;
                    this.Btn_Tap3_MonturaPropia.Enabled = false;
                    this.Btn_Tap3_CristalPropio.Enabled = false;
                    this.Btn_Tap3_ClienteAfiliado.Enabled = false;
                    this.Btn_Tap3_Garantia.Enabled = false;


                    break;


                case "Habilitar_Promociones":

                    // Panel Coloración 
                    this.Pnl_3_Promociones.Enabled = true;

                    //Panel Ingresar Articulo
                    this.Pnl_1_Tap3.Enabled = false;
                    //Grid Carga Articulo
                    this.Dgv_Tap3_Articulo.Enabled = false;
                    //Panel de Medidas Montuta y Observacion
                    this.Pnl_2_Tap3.Enabled = false;
                    //Panel de Totales
                    this.Pnl_3_Tap3.Enabled = false;
                    //Grid Montutas
                    //this.Dgv_Tap3_Medidas_Montura.Enabled = false;
                    //Boton Cancelar Formulario Principal Articulo 
                    this.Btn_Tap3_Cancelar.Enabled = false;
                    //Boton Procesar Formulario Principal Articulo 
                    this.Btn_Tap3_Procesar.Enabled = false;

                    // Panel de Arriba
                    this.Txt_Pnl2_Cedula.Enabled = false;
                    this.Txt_Pnl2_Examen.Enabled = false;
                    this.Cbx_Pnl2_Trbajo.Enabled = false;
                    this.Cbx_Pnl2_Laboratorio.Enabled = false;
                    this.Cbx_Tap2_Ojo.Enabled = false;
                    this.Txt_Pnl2_Examen.Enabled = false;
                    this.Cbx_Pnl2_Servicio.Enabled = false;

                    // Controles del Panel Promocion
                    this.Dgv_Pnl3_Promociones.Enabled = true;
                    this.Btn_Tap3_Cancelar_Promo.Enabled = true;
                    this.Btn_Tap3_Aceptar_Promo.Enabled = true;

                    // Botones del TapControl
                    //this.btnPrincipal.Enabled = false;
                    //this.btnExamen.Enabled = false;
                    //this.btnCargarOrden.Enabled = false;

                    // Botones Aciones 
                    this.Btn_Tap3_Descuento.Enabled = false;
                    this.Btn_Tap3_CambioPrecio.Enabled = false;
                    this.Btn_Tap3_Promocion.Enabled = false;
                    this.Btn_Tap3_MonturaPropia.Enabled = false;
                    this.Btn_Tap3_CristalPropio.Enabled = false;
                    this.Btn_Tap3_ClienteAfiliado.Enabled = false;
                    this.Btn_Tap3_Garantia.Enabled = false;

                    break;

                case "Habilitar_MonturaPropia":

                    // Panel Coloración 
                    this.Pnl_3_Promociones.Enabled = true;

                    //Panel Ingresar Articulo
                    this.Pnl_1_Tap3.Enabled = false;
                    //Grid Carga Articulo
                    this.Dgv_Tap3_Articulo.Enabled = false;
                    //Panel de Medidas Montuta y Observacion
                    this.Pnl_2_Tap3.Enabled = false;
                    //Panel de Totales
                    this.Pnl_3_Tap3.Enabled = false;
                    //Grid Montutas
                    //this.Dgv_Tap3_Medidas_Montura.Enabled = false;
                    //Boton Cancelar Formulario Principal Articulo 
                    this.Btn_Tap3_Cancelar.Enabled = false;
                    //Boton Procesar Formulario Principal Articulo 
                    this.Btn_Tap3_Procesar.Enabled = false;

                    // Panel de Arriba
                    this.Txt_Pnl2_Cedula.Enabled = false;
                    this.Txt_Pnl2_Examen.Enabled = false;
                    this.Cbx_Pnl2_Trbajo.Enabled = false;
                    this.Cbx_Pnl2_Laboratorio.Enabled = false;
                    this.Cbx_Tap2_Ojo.Enabled = false;
                    this.Txt_Pnl2_Examen.Enabled = false;
                    this.Cbx_Pnl2_Servicio.Enabled = false;

                    // Controles del Panel monturaPropia
                    this.rbCompleta.Checked = false;
                    this.rbCompleta.Enabled = true;
                    this.rbRanurada.Checked = false;
                    this.rbRanurada.Enabled = true;
                    this.rbAlaire.Checked = false;
                    this.rbAlaire.Enabled = true;
                    this.btnCancelarMonturaPropia.Enabled = true;
                    this.btnAceptarMonturaPropia.Enabled = true;

                    // Botones del TapControl
                    //this.btnPrincipal.Enabled = false;
                    //this.btnExamen.Enabled = false;
                    //this.btnCargarOrden.Enabled = false;

                    // Botones Aciones 
                    this.Btn_Tap3_Descuento.Enabled = false;
                    this.Btn_Tap3_CambioPrecio.Enabled = false;
                    this.Btn_Tap3_Promocion.Enabled = false;
                    this.Btn_Tap3_MonturaPropia.Enabled = false;
                    this.Btn_Tap3_CristalPropio.Enabled = false;
                    this.Btn_Tap3_ClienteAfiliado.Enabled = false;
                    this.Btn_Tap3_Garantia.Enabled = false;

                    break;

                case "Habilitar_ClienteAfiliado":

                    // Panel Coloración 
                    this.Pnl_3_Promociones.Enabled = true;

                    //Panel Ingresar Articulo
                    this.Pnl_1_Tap3.Enabled = false;
                    //Grid Carga Articulo
                    this.Dgv_Tap3_Articulo.Enabled = false;
                    //Panel de Medidas Montuta y Observacion
                    this.Pnl_2_Tap3.Enabled = false;
                    //Panel de Totales
                    this.Pnl_3_Tap3.Enabled = false;
                    //Grid Montutas
                    //this.Dgv_Tap3_Medidas_Montura.Enabled = false;
                    //Boton Cancelar Formulario Principal Articulo 
                    this.Btn_Tap3_Cancelar.Enabled = false;
                    //Boton Procesar Formulario Principal Articulo 
                    this.Btn_Tap3_Procesar.Enabled = false;

                    // Panel de Arriba
                    this.Txt_Pnl2_Cedula.Enabled = false;
                    this.Txt_Pnl2_Examen.Enabled = false;
                    this.Cbx_Pnl2_Trbajo.Enabled = false;
                    this.Cbx_Pnl2_Laboratorio.Enabled = false;
                    this.Cbx_Tap2_Ojo.Enabled = false;
                    this.Txt_Pnl2_Examen.Enabled = false;
                    this.Cbx_Pnl2_Servicio.Enabled = false;

                    // Controles del Panel ClienteAfiliado
                    this.Txt_Pnl3_ClienteAfiliado.Enabled = true;
                    this.Pnl_3_RadioButonClienteAfiliado.Enabled = true;
                    this.Rd_Pnl3_CteAfiliadoCodigo.Enabled = true;
                    this.Rd_Pnl3_CteAfiliadoDesc.Enabled = true;
                    this.Dgv_Pnl3_ClienteAfiliado.Enabled = true;
                    this.btnCancelarAfiliado.Enabled = true;
                    this.Rd_Pnl3_CteAfiliadoCodigo.Checked = true;

                    // Botones del TapControl
                    //this.btnPrincipal.Enabled = false;
                    //this.btnExamen.Enabled = false;
                    //this.btnCargarOrden.Enabled = false;

                    // Botones Aciones 
                    this.Btn_Tap3_Descuento.Enabled = false;
                    this.Btn_Tap3_CambioPrecio.Enabled = false;
                    this.Btn_Tap3_Promocion.Enabled = false;
                    this.Btn_Tap3_MonturaPropia.Enabled = false;
                    this.Btn_Tap3_CristalPropio.Enabled = false;
                    this.Btn_Tap3_ClienteAfiliado.Enabled = false;
                    this.Btn_Tap3_Garantia.Enabled = false;

                    break;

                case "Habilitar_Garantia":
                    this.Pnl_3_Garantia.Enabled = true;

                    //Panel Ingresar Articulo
                    this.Pnl_1_Tap3.Enabled = false;
                    //Grid Carga Articulo
                    this.Dgv_Tap3_Articulo.Enabled = false;
                    this.Pnl_2_Tap3.Enabled = false;
                    //Panel de Totales
                    this.Pnl_3_Tap3.Enabled = false;
                    this.Btn_Tap3_Cancelar.Enabled = false;
                    this.Btn_Tap3_Procesar.Enabled = false;

                    // Panel de Arriba
                    this.Txt_Pnl2_Cedula.Enabled = false;
                    this.Txt_Pnl2_Examen.Enabled = false;
                    this.Cbx_Pnl2_Trbajo.Enabled = false;
                    this.Cbx_Pnl2_Laboratorio.Enabled = false;
                    //this.Cbx_Tap2_Ojo.Enabled = false;
                    this.Txt_Pnl2_Examen.Enabled = false;
                    this.Cbx_Pnl2_Servicio.Enabled = false;

                    // Controles del Garantia
                    this.Dgv_Pnl3_Garantia.Enabled = true;
                    this.Cbx_Pnl3_Garantia.Enabled = true;
                    this.Btn_Tap3_Aceptar_Garantia.Enabled = true;
                    this.Btn_Tap3_Cancelar_Garantia.Enabled = true;

                    // Botones del TapControl
                    //this.btnPrincipal.Enabled = false;
                    //this.btnExamen.Enabled = false;
                    //this.btnCargarOrden.Enabled = false;

                    // Botones Aciones 
                    this.Btn_Tap3_Descuento.Enabled = false;
                    this.Btn_Tap3_CambioPrecio.Enabled = false;
                    this.Btn_Tap3_Promocion.Enabled = false;
                    this.Btn_Tap3_MonturaPropia.Enabled = false;
                    this.Btn_Tap3_CristalPropio.Enabled = false;
                    this.Btn_Tap3_ClienteAfiliado.Enabled = false;
                    this.Btn_Tap3_Garantia.Enabled = false;

                    break;

                case "Bloquear_Lista_Articulo":
                    this.Pnl_1_Tap3.Enabled = true;
                    this.Dgv_Tap3_Articulo.Enabled = true;
                    this.Pnl_2_Tap3.Enabled = true;
                    this.Pnl_3_Tap3.Enabled = true;
                    //this.Dgv_Tap3_Medidas_Montura.Enabled = true;
                    this.Btn_Tap3_Cancelar.Enabled = true;
                    this.Btn_Tap3_Procesar.Enabled = true;

                    this.Dgv_Pnl3_Articulo.Enabled = false;
                    this.Txt_Pnl3_Articulo.Enabled = false;
                    this.Rd_Pnl3_Descripcion.Enabled = false;
                    this.Rd_Pnl3_Codigo.Enabled = false;
                    //Lo comente porque cuando se guarda un examen y se carga una orden por algun motivo cuando se selecciona el articulo dispara el evento examen click y hace que se devuelva al examen
                    //this.btnCancelar3.Enabled = false;

                    break;

            }


        }

        

        private void Txt_Pnl3_Articulo_TextChanged(object sender, EventArgs e)
        {
            // Filtrar los datos según el texto ingresado en el TextBox
            _L_Articulo.FiltrarArticulos(Txt_Pnl3_Articulo.Text.ToLower(), Rd_Pnl3_Descripcion, Rd_Pnl3_Codigo, Dgv_Pnl3_Articulo, listaArticulos, listaTemporal);

        }

        private void Txt_Pnl3_ClienteAfiliado_TextChanged(object sender, EventArgs e)
        {
            // Filtrar los datos según el texto ingresado en el TextBox
            _L_Articulo.FiltrarEmpresasAfiliadas(Txt_Pnl3_ClienteAfiliado.Text.ToLower(), Rd_Pnl3_CteAfiliadoDesc, Rd_Pnl3_CteAfiliadoCodigo, Dgv_Pnl3_ClienteAfiliado, listaClienteAfiliados, listaTemporalClienteAfiliados);

        }


       

        private void Formato_Dgv_Busqueda_Articulo()
        {
            try
            {

                //Centrar todas las colucnas 
                Dgv_Pnl3_Articulo.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Dgv_Pnl3_Articulo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;


                // Quitar la flecha del selector de fila
                Dgv_Pnl3_Articulo.RowHeadersVisible = false;

                // Deshabilitar el redimensionamiento de filas
                Dgv_Pnl3_Articulo.AllowUserToResizeRows = false;

                //asignar Nombres a cada colucna 
                Dgv_Pnl3_Articulo.Columns["CodArticulo"].HeaderText = "Código";
                Dgv_Pnl3_Articulo.Columns["DESART"].HeaderText = "Descripción";
                Dgv_Pnl3_Articulo.Columns["ART_PVP"].HeaderText = "Precio";
                Dgv_Pnl3_Articulo.Columns["ART_EXIST"].HeaderText = "Cantidad";
                Dgv_Pnl3_Articulo.Columns["MARCA"].HeaderText = "Marca";


                //Ancho de columna
                Dgv_Pnl3_Articulo.Columns["CodArticulo"].Width = 80;
                Dgv_Pnl3_Articulo.Columns["DESART"].Width = 220;
                Dgv_Pnl3_Articulo.Columns["ART_PVP"].Width = 100;
                Dgv_Pnl3_Articulo.Columns["ART_EXIST"].Width = 75;
                Dgv_Pnl3_Articulo.Columns["MARCA"].Width = 50;

                // No modificable
                Dgv_Pnl3_Articulo.Columns["CodArticulo"].ReadOnly = true;
                Dgv_Pnl3_Articulo.Columns["DESART"].ReadOnly = true;
                Dgv_Pnl3_Articulo.Columns["ART_PVP"].ReadOnly = true;
                Dgv_Pnl3_Articulo.Columns["ART_EXIST"].ReadOnly = true;
                Dgv_Pnl3_Articulo.Columns["MARCA"].ReadOnly = true;

                Dgv_Pnl3_Articulo.Columns["CodArticulo"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Pnl3_Articulo.Columns["DESART"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Pnl3_Articulo.Columns["ART_PVP"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Pnl3_Articulo.Columns["ART_EXIST"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Pnl3_Articulo.Columns["MARCA"].SortMode = DataGridViewColumnSortMode.NotSortable;

                /// Se utiliza un bucle foreach para recorrer todas las columnas del DataGridView. 
                /// Si el nombre de la columna no coincide con las columnas que deseas mostrar, 
                /// se oculta configurando su propiedad Visible como false:

                foreach (DataGridViewColumn column in Dgv_Pnl3_Articulo.Columns)
                {
                    if (column.Name != "CodArticulo" &&
                        column.Name != "DESART" &&
                        column.Name != "ART_PVP" &&
                        column.Name != "ART_EXIST" &&
                        column.Name != "MARCA")
                    {
                        column.Visible = false;
                    }
                }


                Dgv_Pnl3_Articulo.Columns["ART_PVP"].DefaultCellStyle.Format = "N2"; // Formato de 2 decimales y unidades de mil

                //quitar seleccion por defecto de datagrid
                Dgv_Pnl3_Articulo.ClearSelection();

                //AutoGenerar Columnas:
                Dgv_Pnl3_Articulo.AutoGenerateColumns = false;


            }


            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }

        }

        private void Dgv_Pnl3_Articulo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try 
            { 
            // Verificar que la fila seleccionada no sea una fila nueva

            if (e.RowIndex >= 0 && !Dgv_Pnl3_Articulo.Rows[e.RowIndex].IsNewRow)
            {
                CodColorLC = "";
                // Obtener el artículo seleccionado
                var articulo = Dgv_Pnl3_Articulo.Rows[e.RowIndex].DataBoundItem as TB_ARTICULO;

                Txt_Tap3_Articulo_Codigo.Text = articulo.CodArticulo;

                if (Txt_Tap3_Articulo_Codigo.Text.StartsWith("W"))
                {
                    
                    Txt_Tap3_Articulo_Codigo.Text = articulo.CodArticulo;
                    Txt_Tap3_Articulo_Descripcion.Text = articulo.DESART;
                    Txt_Tap3_Articulo_Precio.Text = articulo.ART_PVP.ToString("F2"); // Formato de 2 decimales
                    Txt_Tap3_Articulo_Cantidad.Enabled = true;
                    Txt_Tap3_Articulo_Cantidad.Text = string.Empty; // Limpiar el campo de cantidad

                    DataSet dsColorLC = _L_Articulo.CargarColoresLC(Dgv_Pnl3_ColoresLC, Txt_Tap3_Articulo_Codigo.Text);
                    Dgv_Pnl3_ColoresLC.DataSource = dsColorLC.Tables[0];
                    Formato_Dgv_Pnl3_ColoresLC();
                    Pnl_3_Lista_Articulo.Visible = false;
                   
                    //VisualizarPanel("MostrarCabezeraSecundaria");
                    //HabilitacionControl("CabezeraPrincipal");

                    Pnl_3_Lista_ColoresLC.Visible = true;
                    Pnl_3_Lista_ColoresLC.Location = new Point(250, 1);
                    Pnl_3_Lista_ColoresLC.BringToFront();


                    //VisualizarPanel("MostrarCabezeraSecundaria");
                    //HabilitacionControl("CabezeraPrincipal");
                    //if (e.RowIndex >= 0 && !Dgv_Pnl3_Articulo.Rows[e.RowIndex].IsNewRow)
                    //{
                    //    var seleccionColorLC = Dgv_Pnl3_ColoresLC.Rows[e.RowIndex].DataBoundItem;
                    //}
                    //if (articulo != null)
                    //{
                    //    // Cargar los valores en los TextBox
                    //    Txt_Tap3_Articulo_Codigo.Text = articulo.CodArticulo;
                    //    Txt_Tap3_Articulo_Descripcion.Text = articulo.DESART;
                    //    Txt_Tap3_Articulo_Precio.Text = articulo.ART_PVP.ToString("F2"); // Formato de 2 decimales
                    //    Txt_Tap3_Articulo_Cantidad.Enabled = true;
                    //    Txt_Tap3_Articulo_Cantidad.Text = string.Empty; // Limpiar el campo de cantidad

                    //    VisualizarPanel("MostrarCabezeraSecundaria");
                    //    HabilitacionControl("CabezeraPrincipal");

                    //    // Establecer el foco en el TextBox de cantidad
                    //    Txt_Tap3_Articulo_Cantidad.Focus();
                    //}

                    //if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "09")
                    //{
                    //    AplicoGarantia(Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), Txt_Pnl2_Cedula.Text.Substring(0, 1), "", Txt_Pnl2_Examen.Text, _D_Inicio.Sucursal());
                    //}
                }
                else
                {
                    if (articulo != null)
                    {
                        // Cargar los valores en los TextBox
                        Txt_Tap3_Articulo_Codigo.Text = articulo.CodArticulo;
                        Txt_Tap3_Articulo_Descripcion.Text = articulo.DESART;
                        Txt_Tap3_Articulo_Precio.Text = articulo.ART_PVP.ToString("F2"); // Formato de 2 decimales
                        Txt_Tap3_Articulo_Cantidad.Enabled = true;
                        Txt_Tap3_Articulo_Cantidad.Text = string.Empty; // Limpiar el campo de cantidad

                        //VisualizarPanel("MostrarCabezeraSecundaria");
                        this.Pnl_3_Lista_Articulo.Visible = false;
                        HabilitacionControl("CabezeraPrincipal");

                        // Establecer el f
                        // oco en el TextBox de cantidad
                        Txt_Tap3_Articulo_Cantidad.Focus();
                    }

                    //if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "09")
                    //{
                    //    AplicoGarantia(Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), Txt_Pnl2_Cedula.Text.Substring(0, 1), "", Txt_Pnl2_Examen.Text, _D_Inicio.Sucursal());
                    //}
                }
                bool EsAR = false;
                DataSet dsServAR = _D_Articulos.ServiciosAR_btnProcesar(articulo.CodArticulo, false, null);
                //Si es un AR (validar con tabla 1 del dataset)
                foreach (DataRow filaAR in dsServAR.Tables[1].Rows)
                {
                    string codAR = filaAR["CodServicio"].ToString();
                    if (articulo.CodArticulo == codAR)
                    {
                        EsAR = true;
                        break;
                    }
                }

                if (Txt_Tap3_Articulo_Codigo.Text.StartsWith("W") || Txt_Tap3_Articulo_Codigo.Text.StartsWith("C") || EsAR)
                {
                    if (Cbx_Tap2_Ojo.Text == "Ambos")
                    {
                        Txt_Tap3_Articulo_Cantidad.Text = "2";
                    }
                    else
                    {
                        Txt_Tap3_Articulo_Cantidad.Text = "1";
                    }
                }
                else
                {
                    Txt_Tap3_Articulo_Cantidad.Text = "1";
                }


            }
        }
              catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
            }
        }

        private void CargarArticulos_Girdvew()
       {
            /////// ******************Validaciones **************************************
            ///***********************               *************************************

            // Validar que los campos no estén vacíos
            if (_L_Articulo.CargarArticulo_ValidarTexbox(Txt_Tap3_Articulo_Codigo, Txt_Tap3_Articulo_Descripcion, Txt_Tap3_Articulo_Precio, Txt_Tap3_Articulo_Cantidad, Txt_Pnl2_Examen))
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Por favor, complete todos los campos antes de agregar el artículo");
                _FrmMensajes.ShowDialog();
                return;
            }

            // Validar que Tasa
            if (!_L_Articulo.ExisteTasa() && _L_Articulo.stringBuilder.Length > 0)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(_L_Articulo.stringBuilder.ToString());
                _FrmMensajes.ShowDialog();
                return;
            }

            // LLenar Tb_Trabajo
            _L_Articulo.LlenarTB_Trbajo(_TRABAJO, _D_Inicio.Sucursal(), Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2));

            if (_L_Articulo.stringBuilder.Length > 0)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(_L_Articulo.stringBuilder.ToString());
                _FrmMensajes.ShowDialog();
                return;
            }

            // Validar Precio
            if (Txt_Tap3_Articulo_Codigo.Text.StartsWith("C") || Txt_Tap3_Articulo_Codigo.Text.StartsWith("M") || Txt_Tap3_Articulo_Codigo.Text.StartsWith("L"))
            {
                if (_L_Articulo.CargarArticulo_ValidarPrecio(Txt_Tap3_Articulo_Precio))
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("El precio del articulo debe ser mayor que 0");
                    _FrmMensajes.ShowDialog();
                    return;
                }
            }

            //Formatear los caracteres a 7 Digitos cuando es un cristal 
            _L_Articulo.FormatearCampo7Digitos(Txt_Tap3_Articulo_Codigo);

            // Verifica si el articulo ya fue Agregado
            if (_L_Articulo.CargarArticulo_EvitarDuplicado(Dgv_Tap3_Articulo, Txt_Tap3_Articulo_Codigo.Text))
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("El artículo ya fue agregado al listado");
                _FrmMensajes.ShowDialog();
                return;
            }

            //Verifico los productos permitidos
            Boolean VerificoProductos = _L_Articulo.VerificoProductosPermitidos(Txt_Tap3_Articulo_Codigo.Text, Cbx_Pnl2_Trbajo.SelectedValue.ToString(), Dgv_Tap3_Articulo);
            if (VerificoProductos == false && _L_Articulo.stringBuilder.Length > 0)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(_L_Articulo.stringBuilder.ToString());
                _FrmMensajes.ShowDialog();
                return;
            }

            // Validar la existencia del producto
            if (!_L_Articulo.ValidoExistenciaArticulo(Txt_Tap3_Articulo_Codigo.Text, Convert.ToInt16(Txt_Tap3_Articulo_Cantidad.Text), listaArticulos, _D_DetalleOrden.TB_PARAMETRO("LCManejaExist"), Cbx_Pnl2_Trbajo.SelectedValue.ToString()) && _L_Articulo.stringBuilder.Length > 0) // Si hay un mensaje de error
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(_L_Articulo.stringBuilder.ToString());
                _FrmMensajes.ShowDialog();
                return; // Salir 
            }

            //Validar Cantidad Maxima Permitida Para venta
            string mensaje = _L_Articulo.ValidarCantidadMaximaPermitida(Txt_Tap3_Articulo_Codigo.Text, Convert.ToInt16(Txt_Tap3_Articulo_Cantidad.Text));

            if (!string.IsNullOrEmpty(mensaje)) // Si hay un mensaje de error
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(mensaje);
                _FrmMensajes.ShowDialog();
                return; // Salir 
            }

            


            if (_L_Articulo.VerificoCantidadCristales(Txt_Tap3_Articulo_Codigo.Text, Convert.ToInt16(Txt_Tap3_Articulo_Cantidad.Text), _TRABAJO))
            {
                // Definir Accion
                
            }

            if (Txt_Tap3_Articulo_Codigo.Text.StartsWith("W"))
            {
                if (Cbx_Tap2_Ojo.Text == "Ambos" & Txt_Tap3_Articulo_Cantidad.Text == "1")
                {
                    if (LcAmbosCant1 == false)
                    {
                        pnlOjo.Visible = true;
                        return;
                    }
                    else
                    {
                        // Verifica si ya hay un artículo C o W con ojo D en el grid
                        string ojoGrid = "";
                        bool existeD = false;
                        foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
                        {
                            if (row.IsNewRow) continue;
                            string cod = row.Cells["CodArticulo"].Value?.ToString() ?? "";
                            ojoGrid = row.Cells["Ojo"].Value?.ToString() ?? "";
                            if (cod.StartsWith("W"))
                            {
                                existeD = true;
                                break;
                            }
                        }
                        //no hay nada en el grid
                        if (existeD == true)
                        {
                            ojoLenteContacto = (ojoGrid == "I") ? "D" : "I";

                        }
                    }
                }
                else
                {
                    pnlOjo.Visible = false;
                }
            }

            

            if (Cristal_Propio== true && Txt_Tap3_Articulo_Codigo.Text.StartsWith("C"))
            {

                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("No puede agregar un cristal si tiene marcada la opción cristal propio");
                    _FrmMensajes.ShowDialog();
                    return;

            }


            if (Montura_Propia == true && (Txt_Tap3_Articulo_Codigo.Text.StartsWith("L") || Txt_Tap3_Articulo_Codigo.Text.StartsWith("M")))
            {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("No puede agregar una montura si tiene marcada la opción montura propia");
                    _FrmMensajes.ShowDialog();
                    return;
            }

            // Verifico Existencia LC
            if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "02" && Txt_Tap3_Articulo_Codigo.Text.StartsWith("W") && !_L_Articulo.BuscoCodigoLabLC(Txt_Tap3_Articulo_Codigo.Text, CodColorLC, Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), Convert.ToInt16(Txt_Pnl2_Examen.Text),  Cbx_Tap2_Ojo.Text, ojoLenteContacto, Txt_Tap3_Articulo_Cantidad.Text,
                  // delegado
                  (lab, gen) => { codLab = lab; generico = gen; })
                && _L_Articulo.stringBuilder.Length > 0)

            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(_L_Articulo.stringBuilder.ToString());
                _FrmMensajes.ShowDialog();
                return; // Salir 
            }
            else if (_L_Articulo.stringBuilder.Length > 0)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(_L_Articulo.stringBuilder.ToString());
                _FrmMensajes.ShowDialog();
            }

           
            AgregarArticuloAlGrid();

            // Limpiar los TextBox
            LimpiarControles("Motro_Busqueda_Articulos");

            // Establecer el foco en el TextBox de código
            Txt_Tap3_Articulo_Codigo.Focus();

        }

        //private void Txt_Tap3_Articulo_Cantidad_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        // Validar que el texto sea un número válido y mayor que 0
        //        if (int.TryParse(Txt_Tap3_Articulo_Cantidad.Text, out int cantidad) && cantidad > 0)
        //        {
        //            CargarArticulos_Girdvew();
        //        }
        //    }
        //}

        private void Txt_Tap3_Articulo_Cantidad_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Validar que el texto sea un número válido y mayor que 0
                if (int.TryParse(Txt_Tap3_Articulo_Cantidad.Text, out int cantidad) && cantidad > 0)
                {
                    if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "09")
                    {
                        if (!_L_Articulo.AplicoGarantia(Dgv_Tap3_Articulo, Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), Os_Garantia_Trabajo, Numero_Examen_Garantia_Trabajo))
                        {
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje("Esta orden no aplica para reposición de garantia");
                            _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                            _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                            _FrmMensajes.ShowDialog();

                            return;
                        }
                    }
                    CargarArticulos_Girdvew();
                }
            }
        }

        private void AgregarArticuloAlGrid()
        {
            try
            {
                //string artPadre = "";
                // Buscar el artículo en la listaArticulos por el código
                var articulo = listaArticulos.FirstOrDefault(a => a.CodArticulo == Txt_Tap3_Articulo_Codigo.Text);

                if (articulo == null)
                {
                    _FrmMensajes.co = 2;

                    _FrmMensajes.avisomensaje("El artículo no existe en la lista");
                    _FrmMensajes.ShowDialog();
                    return;
                }

                // Validar que los campos de precio y cantidad sean válidos
                if (!decimal.TryParse(Txt_Tap3_Articulo_Precio.Text, out decimal precio))
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("El precio ingresado no es válido");
                    _FrmMensajes.ShowDialog();
                    return;
                }

                if (!int.TryParse(Txt_Tap3_Articulo_Cantidad.Text, out int cantidad))
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("La cantidad ingresada no es válida");
                    _FrmMensajes.ShowDialog();
                    return;
                }

                // Buscar el artículo en la listaArticulos por el código
                var _Trabajo = _TRABAJO.FirstOrDefault(a => a.T_CEDIDEN == Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2) & a.T_NACIO == Txt_Pnl2_Cedula.Text.Substring(0, 1));



                // Calcular el total
                decimal total = precio * cantidad;

                // Determinar el impuesto
                int impuesto;
                if (articulo.ART_EXENTO)
                {
                    impuesto = 0; // Si el artículo está exento, el impuesto es 0
                }
                else
                {
                    impuesto = _L_Articulo.BuscarIva("I"); // Llamar a la función para calcular el IVA
                }

                string ojo = "";

                if (articulo.CodArticulo.StartsWith("C"))
                {
                    string seleccionOjo = Cbx_Tap2_Ojo.Text ?? "";
                    int cantidadInt = 0;
                    int.TryParse(Txt_Tap3_Articulo_Cantidad.Text, out cantidadInt);

                    if (seleccionOjo == "Ambos")
                    {

                        if (cantidadInt == 1)
                        {
                            // Verifica si ya hay un artículo C o W con ojo D en el grid
                            bool existeD = false;
                            foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
                            {
                                if (row.IsNewRow) continue;
                                string cod = row.Cells["CodArticulo"].Value?.ToString() ?? "";
                                string ojoGrid = row.Cells["Ojo"].Value?.ToString() ?? "";
                                if ((cod.StartsWith("C") || cod.StartsWith("W")) && ojoGrid == "D")
                                {
                                    existeD = true;
                                    break;
                                }
                            }
                            ojo = existeD ? "I" : "D";
                        }
                        else
                        {
                            ojo = "A";
                        }
                       
                    }
                    else if (seleccionOjo == "Izquierdo" || seleccionOjo == "Derecho")
                    {
                       
                       ojo = seleccionOjo.Substring(0,1);

                        //else if (cantidadInt >= 2)
                        //{
                        //    _FrmMensajes.avisomensaje("No puede seleccionar cantidad: " + cantidadInt  + " para un solo ojo");
                        //    _FrmMensajes.ShowDialog();
                        //    return;
                            
                        //}
                    }
                }

                if (articulo.CodArticulo.StartsWith("W"))
                {
                    string seleccionOjo = Cbx_Tap2_Ojo.Text ?? "";
                    int cantidadInt = 0;
                    int.TryParse(Txt_Tap3_Articulo_Cantidad.Text, out cantidadInt);

                    string ojoGrid = "";

                    if (seleccionOjo == "Ambos")
                    {

                        if (cantidadInt == 1)
                        {
                         
                            // Verifica si ya hay un artículo C o W con ojo D en el grid
                            bool existeD = false;
                            foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
                            {
                                if (row.IsNewRow) continue;
                                string cod = row.Cells["CodArticulo"].Value?.ToString() ?? "";
                                ojoGrid = row.Cells["Ojo"].Value?.ToString() ?? "";
                                if (cod.StartsWith("W"))
                                {
                                    existeD = true;
                                    break;
                                }
                            }
                            //no hay nada en el grid
                            if (existeD == false)
                            {
                                ojo = ojoLenteContacto;
                            }
                            else
                            {
                                ojo = (ojoGrid == "D") ? "I" : "D";

                            }

                        }
                        else
                        {
                            ojo = "A";
                        }

                    }
                    else if (seleccionOjo == "Izquierdo" || seleccionOjo == "Derecho")
                    {

                        ojo = seleccionOjo.Substring(0, 1);

                        //else if (cantidadInt >= 2)
                        //{
                        //    _FrmMensajes.avisomensaje("No puede seleccionar cantidad: " + cantidadInt  + " para un solo ojo");
                        //    _FrmMensajes.ShowDialog();
                        //    return;

                        //}
                    }
                }

                //_L_Articulo.AgregarFila(Dgv_Tap3_Articulo, articulo.CodArticulo, articulo.DESART, cantidad, (decimal) precio, (decimal)articulo.PORCTDESCUENTO, (decimal) total, impuesto, _Trabajo.T_OJO);
                _L_Articulo.AgregarFila(Dgv_Tap3_Articulo,
                    articulo.CodArticulo,
                    Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "02" && articulo.CodArticulo.StartsWith("W")? codLab : "",
                    Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "02" ? generico : "",
                    CodColorLC,articulo.DESART, cantidad, (decimal)precio, EmpresaAfiliada != "" && PorcDctoEmpresaAfiliada > 0 ? PorcDctoEmpresaAfiliada : (decimal)articulo.PORCTDESCUENTO, (decimal)total, impuesto, ojo, (decimal) articulo.COSTOPROME);

                //if (artPadre != "")
                //{
                //    _L_Articulo.CargarServicioGarantia(Dgv_Tap3_Articulo);
                //}
                // Limpiar los TextBox después de agregar el artículo
                ReiniciarBusquedaarticulo();

                // Establecer el foco en el campo de código
                Txt_Tap3_Articulo_Codigo.Focus();
   

            }
            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();

            }
        }


        private void Dgv_Tap3_Articulo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //DgvListadoOrdenes.Columns["Fec_Crea"]
            Dgv_Tap3_Articulo.Focus();
            if (e.ColumnIndex >= 0 && Dgv_Tap3_Articulo.Columns[e.ColumnIndex].Name == "Eliminar")
            {
                int FilaPorBorrar = e.RowIndex;
                // Verificar si se puede borrar el artículo
                bool puedeBorrar = _L_Articulo.VerificarYBorrarArticulo(Dgv_Tap3_Articulo, ref FilaPorBorrar);

                if (puedeBorrar)
                {
                    // Si se puede borrar, eliminar la fila
                    
                    Dgv_Tap3_Articulo.Rows.RemoveAt(FilaPorBorrar);

                    if (Cbx_Tap2_Tipo_Examen.Text == "CONTACTO")
                    {
                        if (Dgv_Tap3_Articulo.Rows.Count == 0)
                        {
                            LcAmbosCant1 = false;
                            ojoLenteContacto = "";
                        }
                    }

                }
                else
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("No se puede borrar este artículo");
                    _FrmMensajes.ShowDialog();
                }


            }

        }

        public void LimpiarGrid()
        {
            try
            {
                // Desvincular el DataGridView de su fuente de datos
                Dgv_Tap3_Articulo.DataSource = null;
                Dgv_Tap3_Articulo.DataMember = null;

                // Eliminar todas las filas
                Dgv_Tap3_Articulo.Rows.Clear();

                // Eliminar todas las columnas
                Dgv_Tap3_Articulo.Columns.Clear();

                // Verificar y eliminar la columna "Eliminar" si existe
                var dataGridViewColumn2 = Dgv_Tap3_Articulo.Columns["Eliminar"]; 
                if (dataGridViewColumn2 != null)
                {
                    Dgv_Tap3_Articulo.Columns.Remove(dataGridViewColumn2);
                }

                // Limpiar Variables Coloracion y Promociones, montura propia , cristal propio y empresa afiliada 
                Codigo_Coloracion = "";
                Codigo_Promocion = "";
                Promocion_Aplicada = false;
                Cristal_Propio = false;
                Montura_Propia = false;
                TipoMonturaPropia = "";
                EmpresaAfiliada = "";
                PorcDctoEmpresaAfiliada = 0;
                Txt_Tap3_Articulo_Codigo.Text = "Código"; // Restaurar el texto sugerido
                Garantia = false;
                ValidarRegistrosYHabilitar_Botones();
                CodColorLC = "";
                Codigo_Servicio_Agregar = "";
                txtObservacion.Text = "";
                Codmotivodes = "";
                ApruebaAORangoCRT = false;
                // Botones Aciones 

                Cbx_Pnl2_Trbajo.Enabled = true;
                Cbx_Pnl2_Laboratorio.Enabled = true;
                this.Cbx_Tap2_Ojo.Enabled = true;
                this.Txt_Pnl2_Examen.Enabled = true; 
                Cbx_Pnl2_Servicio.Enabled = true;
                laboratorioSeleccionado = false;
                Txt_Pnl2_Fecha_Ofre.Text = "";
                txtHorizontal.Text = "";
                txtVertical.Text = "";
                txtMaxima.Text = "";
                txtPuente.Text = "";
                codLab = "";
                generico = "";
                Os_Garantia_Trabajo = "";
                Numero_Examen_Garantia_Trabajo = "";
                txtHorizontal.Enabled = false;
                txtVertical.Enabled = false;
                txtMaxima.Enabled = false;
                txtPuente.Enabled = false;

                txtObservacion.Text = "";

                ReiniciarBusquedaarticulo();

                Lbl_Tap3_Articulo1.Text = "Ingresar Articulo";
                btnPrincipal.Enabled = true;
                btnExamen.Enabled = true;
                btnCargarOrden.Enabled = true;
                tipoTrabajoSeleccionado = false;

                Cbx_Tap2_Ojo.Text = "AMBOS";
                cbVisionDerecha.Text = "Cerca";
                cbVisionIzquierda.Text = "Cerca";

                label24.Visible = true;
                Cbx_Tap2_Ojo.Visible = true;
                label26.Visible = true;
                cbVisionDerecha.Visible = true;
                label33.Visible = true;
                cbVisionIzquierda.Visible = true;

                label34.Visible = true;
                label35.Visible = true;
                txtAltD.Visible = true;
                txtAltI.Visible = true;

                txtAltD.Text  = "0";
                txtAltI.Text = "0";

                txtDistVertice.Text = "0,00";
                txtAngFac.Text = "0,00";
                txtAngPant.Text = "0";
                txtDll.Text = "0,00";
                txt_Pnl2_conv_mimesys.Text = "";

                ojoLenteContacto = "";
                pnlOjo.Visible = false;
                LcAmbosCant1 = false;
                BotonesColor(true,"todos");
                if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "09")
                    Btn_Tap3_Garantia.Enabled = true;
            }

            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }

        }

        public void LimpiarGridMantenerMedidasEsp()
        {
            try
            {
                // Desvincular el DataGridView de su fuente de datos
                Dgv_Tap3_Articulo.DataSource = null;
                Dgv_Tap3_Articulo.DataMember = null;

                // Eliminar todas las filas
                Dgv_Tap3_Articulo.Rows.Clear();

                // Eliminar todas las columnas
                Dgv_Tap3_Articulo.Columns.Clear();

                // Verificar y eliminar la columna "Eliminar" si existe
                var dataGridViewColumn2 = Dgv_Tap3_Articulo.Columns["Eliminar"];
                if (dataGridViewColumn2 != null)
                {
                    Dgv_Tap3_Articulo.Columns.Remove(dataGridViewColumn2);
                }

                // Limpiar Variables Coloracion y Promociones, montura propia , cristal propio y empresa afiliada 
                Codigo_Coloracion = "";
                Codigo_Promocion = "";
                Promocion_Aplicada = false;
                Cristal_Propio = false;
                Montura_Propia = false;
                TipoMonturaPropia = "";
                EmpresaAfiliada = "";
                PorcDctoEmpresaAfiliada = 0;
                Txt_Tap3_Articulo_Codigo.Text = "Código"; // Restaurar el texto sugerido
                Garantia = false;
                // Cargar los valores
                ValidarRegistrosYHabilitar_Botones();
                CodColorLC = "";
                Codigo_Servicio_Agregar = "";
                txtObservacion.Text = "";
                Codmotivodes = "";
                ApruebaAORangoCRT = false;
                // Botones Aciones 

                //Cbx_Pnl2_Trbajo.Enabled = true;
                //Cbx_Pnl2_Laboratorio.Enabled = true;
                //this.Cbx_Tap2_Ojo.Enabled = true;
                //this.Txt_Pnl2_Examen.Enabled = true;
                //Cbx_Pnl2_Servicio.Enabled = true;
                //laboratorioSeleccionado = false;

                txtHorizontal.Text = "";
                txtVertical.Text = "";
                txtMaxima.Text = "";
                txtPuente.Text = "";
                codLab = "";
                generico = "";
                Os_Garantia_Trabajo = "";
                Numero_Examen_Garantia_Trabajo = "";
                txtHorizontal.Enabled = false;
                txtVertical.Enabled = false;
                txtMaxima.Enabled = false;
                txtPuente.Enabled = false;

                txtObservacion.Text = "";

                ReiniciarBusquedaarticulo();

                Lbl_Tap3_Articulo1.Text = "Ingresar Articulo";
                //btnPrincipal.Enabled = true;
                //btnExamen.Enabled = true;
                //btnCargarOrden.Enabled = true;
                //tipoTrabajoSeleccionado = false;

                //Cbx_Tap2_Ojo.Text = "AMBOS";
                //cbVisionDerecha.Text = "Cerca";
                //cbVisionIzquierda.Text = "Cerca";

                //label24.Visible = true;
                //Cbx_Tap2_Ojo.Visible = true;
                //label26.Visible = true;
                //cbVisionDerecha.Visible = true;
                //label33.Visible = true;
                //cbVisionIzquierda.Visible = true;

                //label34.Visible = true;
                //label35.Visible = true;
                //txtAltD.Visible = true;
                //txtAltI.Visible = true;

                //txtAltD.Text = "0";
                //txtAltI.Text = "0";

                //txtDistVertice.Text = "0,00";
                //txtAngFac.Text = "0,00";
                //txtAngPant.Text = "0";
                //txtDll.Text = "0,00";
                //txt_Pnl2_conv_mimesys.Text = "";

                Cbx_Pnl2_Laboratorio.Visible = false;
                Lbl_Pnl2_Laboratorio.Visible = false;
                Cbx_Pnl2_Servicio.Visible = false;
                Lbl_Pnl2_Servicio.Visible = false;
                Lbl_Pnl2_Fecha_Ofre.Visible = false;
                Txt_Pnl2_Fecha_Ofre.Visible = false;


                ojoLenteContacto = "";
                pnlOjo.Visible = false;
                LcAmbosCant1 = false;

                BotonesColor(true, "todos");
                if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "09")
                    Btn_Tap3_Garantia.Enabled = true;
            }

            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }

        }

        public void ReiniciarBusquedaarticulo()
        {
            Txt_Tap3_Articulo_Codigo.Text = "Código"; // Restaurar el texto sugerido
            Txt_Tap3_Articulo_Codigo.ForeColor = Color.DarkGray; // Cambiar el color del texto a gris
            Txt_Tap3_Articulo_Descripcion.Text = "Descripción"; // Restaurar el texto sugerido
            Txt_Tap3_Articulo_Descripcion.ForeColor = Color.DarkGray; // Cambiar el color del texto a gris
            Txt_Tap3_Articulo_Cantidad.Text = "Cantidad"; // Restaurar el texto sugerido
            Txt_Tap3_Articulo_Cantidad.ForeColor = Color.DarkGray; // Cambiar el color del texto a gris
            Txt_Tap3_Articulo_Precio.Text = "Precio"; // Restaurar el texto sugerido
            Txt_Tap3_Articulo_Precio.ForeColor = Color.DarkGray; // Cambiar el color del texto a gris
        }

        private void Dgv_Tap3_Articulo_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            //Para colocar un icono en el boton del grid
            if (e.ColumnIndex >= 0 && this.Dgv_Tap3_Articulo.Columns[e.ColumnIndex].Name == "Eliminar" && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                // Ya no establecemos un color de fondo explícito para que sea transparente
                e.CellStyle.BackColor = Color.White;
                DataGridViewButtonCell celBoton = this.Dgv_Tap3_Articulo.Rows[e.RowIndex].Cells["Eliminar"] as DataGridViewButtonCell;
                Icon IconAtomico;
               
                if (!Formato_Claro)
                IconAtomico = new Icon(Environment.CurrentDirectory + @"\\cuadraditoOscuro2.ico");
                else
                IconAtomico = new Icon(Environment.CurrentDirectory + @"\\cuadraditoOscuro.ico");

                

                HabEliminar = true; //Se manda señal de boton ACTIVADO para realizar validaciones posteriores
                // Calcula un nuevo tamaño para el icono si quieres hacerlo más pequeño
                int nuevoAncho = IconAtomico.Width-37 ; // Ejemplo: reducir a la mitad
                int nuevoAlto = IconAtomico.Height-8 ; // Ejemplo: reducir a la mitad
                using (Bitmap bmp = new Bitmap(IconAtomico.ToBitmap(), new Size(nuevoAncho, nuevoAlto)))
                using (Bitmap bmpFondoBlanco = new Bitmap(nuevoAncho, nuevoAlto))
                using (Graphics g = Graphics.FromImage(bmpFondoBlanco))
                {
                    // Dibujar el fondo blanco
                    g.Clear(Color.White);

                    // Dibujar el icono encima del fondo blanco
                    g.DrawImage(bmp, 0, 0);

                    using (Icon iconoConFondoBlanco = Icon.FromHandle(bmpFondoBlanco.GetHicon()))
                    {
                        
                        // Calcula la posición para centrar el icono más pequeño dentro de la celda
                        int x = e.CellBounds.Left + (e.CellBounds.Width - nuevoAncho)/2 ;
                        int y = e.CellBounds.Top + (e.CellBounds.Height - nuevoAlto) /2;

                        e.Graphics.DrawIcon(iconoConFondoBlanco, x, y);

                        // Ajusta la altura de la fila al nuevo tamaño del icono (opcional)
                        this.Dgv_Tap3_Articulo.Rows[e.RowIndex].Height = nuevoAlto + 1;
                        // Ajusta el ancho de la columna al nuevo tamaño del icono (opcional)
                        this.Dgv_Tap3_Articulo.Columns[e.ColumnIndex].Width = nuevoAncho + 2;
                    }
                }
                e.Handled = true;
            }

            // Esta línea probablemente no debería estar aquí dentro de CellPainting, 
            // ya que se ejecutará por cada celda pintada. Considera moverla a otro evento 
            // o realizar el ajuste de tamaño del DataGridView de otra manera si es necesario.
            // Dgv_Tap3_Articulo.Size = new Size(1059, 150);
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*MEIFER*/
            //limpearExamen();

            //if (!string.IsNullOrEmpty(Txt_Tap1_Cedula.Text))
            //{
            //    // Asegúrate de que el TabControl se llama como en tu formulario (ej: tabControl1)
            //    if (tabControl.SelectedIndex == 1) // Las pestañas están indexadas desde 0. Tab2 sería el índice 1.
            //    {
            //        grp_pln2_Cont1.Visible = false;
            //        grp_pln2_Conv2.Visible = true;
            //        //Cbx_Tap2_Tipo_Examen.SelectedIndex = 1; // Seleciona o segundo item (índice 1)
            //        grp_pln2_oft3.Visible = false;
            //        grp_pln2_ret4.Visible = false;
            //        grp_pln2_quera5.Visible = false;
            //        Lbl_Pnl2_Datos_Cliente.Visible = false;

            //        CargarExamenConv();
            //        CargarExamenCont();
            //        CargarDgvPnl2MedConv();
            //        CargarFicconvOFT();
            //        CargarDgv_Pnl2_Querato();


            //    }
            //    Pnl_2.Visible = true;
            //    Lbl_Pnl2_Datos_Cliente.Visible = true;
            //    this.Txt_Pnl_2_Cedula.Text = Txt_Tap1_Cedula.Text;
            //    this.Txt_Pnl_2_Nombre.Text = Txt_Tap1_Nombre.Text;
            //    Cbx_Pnl2_Trbajo.Enabled = true;
            //    Cbx_Pnl2_Laboratorio.Enabled = true;
            //    Cbx_Pnl2_Servicio.Enabled = true;
            //    Cbx_Tap2_Tipo_Optome.Enabled = true;
            //    Cbx_Tap2_Nombre_Optome.Enabled = true;
            //    //Cbx_Tap2_Tipo_Examen.Enabled = true;
            //    Cbx_Tap2_Ojo.Enabled = true;

            //    // Llama directamente al método del evento
            //    Cbx_Tap2_Tipo_Examen_SelectedIndexChanged(Cbx_Tap2_Tipo_Examen, EventArgs.Empty);

            //    // -------------------------------------------------------
            //    DateTime fechaSeleccionada = Dtp_Tap2_FecExam.Value.Date; // Obtener solo la parte de la fecha
            //    DateTime fechaHoy = DateTime.Now.Date; // Obtener la fecha actual sin la hora

            //    if (fechaSeleccionada < fechaHoy)
            //    {
            //        BloquearCamposE(); // Llamar al método para bloquear los campos
            //    }
            //    else if (fechaSeleccionada == fechaHoy)
            //    {
            //        DesbloquearCamposE(); // Llamar al método para desbloquear los campos
            //    }
            //}
            //else
            //{

            //    tabControl.SelectedIndex = 0;
            //    tabControl.SelectedTab = tabControl.TabPages[0];
            //    Txt_Tap1_Cedula.Focus();
            //    //MessageBox.Show("Debe Seleccionar un Cliente Valido")
            //    Pnl_2_Msj.Visible = true;
            //    txt_pl2_msj.Text = "Debe Seleccionar un Cliente Valido";
            //    //pb_pl2_mj.Visible = true;


            //}

            ///*MEIFER*/

            // Verificar si la pestaña seleccionada es la pestaña 3
            if (tabControl.SelectedIndex == 2) // El índice es 0-based, por lo que la pestaña 3 tiene índice 2
            {
                VisualizarPanel("MostrarCabezeraSecundaria");
                _L_Articulo.InicializarDataGridViewTotales(Dgv_Tap3_Totales);
                Formato_Dgv_Totales();
                //_L_Articulo.BucarTipoVenta(Cbx_Pnl2_Trbajo);
                _L_Articulo.LlenarComboOjos(Cbx_Tap2_Ojo);
                ValidarTipoVenta(Cbx_Pnl2_Trbajo.Text);
                //CargarComboLaboratorios();
                //_L_Articulo.ObtenerFechaHoraOfrecida(Cbx_Pnl2_Servicio.SelectedValue.ToString(), Cbx_Pnl2_Trbajo.SelectedValue.ToString());



            }
            else if (tabControl.SelectedIndex == 0)
            {
                VisualizarPanel("MostrarCabezeraPrincipal");
            }
            else if (tabControl.SelectedIndex == 1)
            {

                VisualizarPanel("MostrarCabezeraExamen");
            }
        }

        private void Txt_Tap3_Articulo_Codigo_KeyDown(object sender, KeyEventArgs e)
        {
            // Verificar si se presionó la tecla F2
            if (ValidarTipoTrabajoTipoExamen(Cbx_Pnl2_Trbajo.SelectedValue.ToString(), Cbx_Tap2_Tipo_Examen.Text) == true)
            {
                if (e.KeyCode == Keys.F2)
                {
                    VisualizarPanel("Lista_Articulo");
                    HabilitacionControl("Habilitar_Lista_Articulo");
                    LimpiarControles("Abrir_Busqueda_Articulos");

                    _L_Articulo.CargarArticulos(Dgv_Pnl3_Articulo, listaArticulos, Cbx_Pnl2_Trbajo.SelectedValue.ToString(), _L_Articulo.ValidarExtenciaCristal(Dgv_Tap3_Articulo, Cbx_Pnl2_Trbajo.SelectedValue.ToString()));
                    if (_L_Articulo.stringBuilder.Length > 0)
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje(_L_Articulo.stringBuilder.ToString());
                        _FrmMensajes.ShowDialog();
                    }
                    else
                    {

                        listaTemporal = new List<TB_ARTICULO>(listaArticulos);
                        Dgv_Pnl3_Articulo.DataSource = listaTemporal;
                        Formato_Dgv_Busqueda_Articulo();
                    }

                    // Establecer el foco en el TextBox de cantidad
                    Txt_Pnl3_Articulo.Focus();

                    _L_Articulo.stringBuilder.Clear();

                    // Evitar que el evento se propague
                    e.Handled = true;
                }

                else if (e.KeyCode == Keys.Enter)
                {

                    if (Txt_Tap3_Articulo_Codigo.Text.StartsWith("W"))
                    {
                        DataSet dsColorLC = _L_Articulo.CargarColoresLC(Dgv_Pnl3_ColoresLC, Txt_Tap3_Articulo_Codigo.Text);
                        Dgv_Pnl3_ColoresLC.DataSource = dsColorLC.Tables[0];
                        Formato_Dgv_Pnl3_ColoresLC();
                        Pnl_3_Lista_ColoresLC.Visible = true;
                        Pnl_3_Lista_ColoresLC.Location = new Point(250, 1);
                    }
                    // Acción para Enter
                    _L_Articulo.CargarArticulos(Dgv_Pnl3_Articulo, listaArticulos, Cbx_Pnl2_Trbajo.SelectedValue.ToString(), _L_Articulo.ValidarExtenciaCristal(Dgv_Tap3_Articulo, Cbx_Pnl2_Trbajo.SelectedValue.ToString()));

                    //Formatear los caracteres a 7 Digitos cuando es un cristal 
                    _L_Articulo.FormatearCampo7Digitos(Txt_Tap3_Articulo_Codigo);

                    // Buscar el articulo 
                    _L_Articulo.FiltrarArticulos_Tap3(Txt_Tap3_Articulo_Codigo.Text, listaArticulos, listaTemporal, Txt_Tap3_Articulo_Codigo, Txt_Tap3_Articulo_Descripcion, Txt_Tap3_Articulo_Precio, Txt_Tap3_Articulo_Cantidad, Cbx_Tap2_Ojo.Text);

                    //if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "09")
                    //{
                    //    AplicoGarantia(Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), Txt_Pnl2_Cedula.Text.Substring(0, 1), "", Txt_Pnl2_Examen.Text, _D_Inicio.Sucursal());
                    //}

                    // Evitar que el evento se propague
                    e.Handled = true;
                }
            }
           

            

        }

        private void Btn_Tap3_Cancelar_Click(object sender, EventArgs e)
        {
            Cbx_Pnl2_Trbajo.SelectedIndex = 0;
            Cbx_Pnl2_Laboratorio.SelectedIndex = 0; 
            Cbx_Pnl2_Servicio.SelectedIndex = 0;
            LimpiarGrid();
        }

        public void CancelarPorCambioExamen()
        {
            
            LimpiarGridMantenerMedidasEsp();
        }


        private void btnPrincipal_Click(object sender, EventArgs e)
        {
            mantengoexamenseleccionado = false;
            VisualizarPanel("MostrarCabezeraPrincipal");
            //Txt_Pnl1_Cedula.Visible = false;
            //Txt_Pnl1_Nombre.Visible = false;
            //Lbl_Pnl1_Cedula.Visible = false;
            //Lbl_Pnl1_Nombre.Visible = false;

            //tabControl.SelectTacb(0);
            /*MEIFER*/
            if (tabControl.TabPages.Count > 0)
            {
                //if (tabControl.SelectedIndex == 2)
                //{
                //    tabControl.SelectedIndex = 2;
                //}
                //else
                //{

                //_FrmMensajes.co = 2;
                //_FrmMensajes.avisomensaje($"Control con foco: {this.ActiveControl?.Name}");
                //_FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                //_FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                //_FrmMensajes.ShowDialog();
                ////Debug.WriteLine($"Control con foco: {this.ActiveControl?.Name}");

                btnPrincipal.Focus();
                tabControl.SelectedIndex = 0;
               
                //}

                //Pnl_2.Visible = false;
                Pnl_1.Visible = true;

                // Opcional: Llamar al evento directamente si la selección no lo dispara
                // tabControl_SelectedIndexChanged(tabControl, EventArgs.Empty);
            }
        }

        

        private void btnExamen_Click(object sender, EventArgs e)
        {

            if (tabControl.SelectedIndex == 0)
            {
                Btn_Tap1_Guardar.PerformClick();
            }
            else
            {
                VisualizarPanel("MostrarCabeceraExamen");
                btnExamen.Focus();
                tabControl.SelectedIndex = 1;
               
            }


        }

        private void btnDetalleOrden_CheckedChanged(object sender, EventArgs e)
        {
            if (validandoCambioTab) return;

            try
            {
                validandoCambioTab = true;

                if (tabControl.SelectedIndex == 0)
                {
                    Btn_Tap1_Guardar.PerformClick();

                    // Validación de campos obligatorios
                    if (Cbx_Tap1_TLF_Local.SelectedIndex == -1 && Cbx_Tap1_TLF_Celular.SelectedIndex == -1 || string.IsNullOrEmpty(Txt_Tap1_Email.Text.Trim()))
                    {
                      
                        // Mantenerse en el Tab 0
                        tabControl.SelectedIndex = 0;
                        btnPrincipal.Focus();
                        btnCargarOrden.Enabled = true;
                    }
                    else
                    {
                        // Cambiar al Tab 2 solo si pasa validación
                        tabControl.SelectedIndex = 2;
                        btnCargarOrden.Focus();
                    }
                }

            if (tabControl.SelectedIndex == 1)
            {
                CancelarPorCambioExamen();
                LLenar_TbTrabajo();
                _L_Trabajo.AgregarTrabajo(nuevoTrabajo);
                btnCargarOrden.Focus(); 
                tabControl.SelectedIndex = 2;
                
                //ValidarTipoTrabajoTipoExamen(Cbx_Pnl2_Trbajo.SelectedValue.ToString(), Cbx_Tap2_Tipo_Examen.Text);
                return;
            }

            if (tabControl.SelectedIndex == 2)
            {
                //btnExamen.Focus();
                //tabControl.SelectedIndex = 1;
               
                return;
            }


            }
            finally
            {
                validandoCambioTab = false;
            }
        }

        private void Dgv_Tap3_Articulo_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            // Verifico si es un cristal y si posee servicio agregado
            for (int numFilas = e.RowIndex; numFilas < e.RowIndex + e.RowCount; numFilas++)
            {
                if (Dgv_Tap3_Articulo.Rows[numFilas].Cells["CodArticulo"].Value != null &&
                    Dgv_Tap3_Articulo.Rows[numFilas].Cells["CodArticulo"].Value.ToString().StartsWith("C"))
                {
                    int FilaCRT = numFilas;
                    //Verifico Prisma 
                    _L_Articulo.CargarServicioOPrima(PorcDctoEmpresaAfiliada,Dgv_Tap3_Articulo, "Prisma", Convert.ToInt32(Txt_Pnl2_Examen.Text), Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), Dgv_Tap3_Articulo.Rows[numFilas].Cells["Ojo"].Value.ToString());
                    //_L_Articulo.CargarServicioOPrima(Dgv_Tap3_Articulo, "", Convert.ToInt32(Txt_Pnl2_Examen.Text), Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), Dgv_Tap3_Articulo.Rows[numFilas].Cells["Ojo"].Value.ToString());

                    if (Convert.ToInt32(Dgv_Tap3_Articulo.Rows[numFilas].Cells["ART_EXIST"].Value) == 2)
                    {
                        //Verifico Diotria
                        _L_Articulo.EvaluoServicioAgregado(PorcDctoEmpresaAfiliada,Dgv_Tap3_Articulo, numFilas, "D", Convert.ToInt32(Txt_Pnl2_Examen.Text), Cbx_Pnl2_Trbajo.SelectedValue.ToString(), Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2));
                        _L_Articulo.EvaluoServicioAgregado(PorcDctoEmpresaAfiliada,Dgv_Tap3_Articulo, numFilas, "I", Convert.ToInt32(Txt_Pnl2_Examen.Text), Cbx_Pnl2_Trbajo.SelectedValue.ToString(), Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2));

                    }
                    else
                    {
                        //Verifico Diotria
                        _L_Articulo.EvaluoServicioAgregado(PorcDctoEmpresaAfiliada,Dgv_Tap3_Articulo, numFilas, Dgv_Tap3_Articulo.Rows[numFilas].Cells["Ojo"].Value.ToString(), Convert.ToInt32(Txt_Pnl2_Examen.Text), Cbx_Pnl2_Trbajo.SelectedValue.ToString(), Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2));
                    }

                    //Nota se Movio al panel de montura propia en el boton aceptar
                    //if (Montura_Propia == true)
                    //{
                    //    _L_Articulo.CargarServicioMonturaPropia(Dgv_Tap3_Articulo, TipoMonturaPropia == "Completa");

                    //}

                    if (Garantia == true)
                    {
                        _L_Articulo.CargarServicioGarantia(Dgv_Tap3_Articulo);

                    }

                    if (!string.IsNullOrEmpty(Codigo_Servicio_Agregar))
                    {
                        _L_Articulo.CargarServicioExpress(Dgv_Tap3_Articulo, Codigo_Servicio_Agregar);

                    }

                }

                if (Dgv_Tap3_Articulo.Rows[numFilas].Cells["CodArticulo"].Value != null &&
                    Dgv_Tap3_Articulo.Rows[numFilas].Cells["CodArticulo"].Value.ToString().StartsWith("W"))
                {
                    if (!string.IsNullOrEmpty(Codigo_Servicio_Agregar))
                    {
                        _L_Articulo.CargarServicioExpress(Dgv_Tap3_Articulo, Codigo_Servicio_Agregar);

                    }
                }
                if (Dgv_Tap3_Articulo.Rows[numFilas].Cells["CodArticulo"].Value != null &&
                    (Dgv_Tap3_Articulo.Rows[numFilas].Cells["CodArticulo"].Value.ToString().StartsWith("M") ||
                     Dgv_Tap3_Articulo.Rows[numFilas].Cells["CodArticulo"].Value.ToString().StartsWith("L")))
                {
                    _L_Articulo.CargarMedidasMontura(txtHorizontal, txtVertical, txtMaxima, txtPuente, Dgv_Tap3_Articulo.Rows[numFilas].Cells["CodArticulo"].Value.ToString());
                }

            }

            //Validar si Existe colorocaion agregada SI ya agregaron la coloracion no abro el panel 
            if (string.IsNullOrEmpty(Codigo_Coloracion))
            {
                //Validar si agregaron coloracion y el grid tiene un crsital 
                if (_L_Articulo.ServicioColoracion(Dgv_Tap3_Articulo, Dgv_Pnl3_Coloracion, Rd_Pnl3_FullColor))
                {

                    //Abro el panel de coloracion 
                    VisualizarPanel("Coloracion");
                    HabilitacionControl("Habilitar_Coloracion");
                }
            }

            // Validar si se agrego un servicio sin codigo padre
            _L_Articulo.VerificarServicioCodigoPadre(Dgv_Tap3_Articulo);

            if (Garantia == true)
            {
                _L_Articulo.CargarServicioGarantia(Dgv_Tap3_Articulo);

            }

            // validar si el servicio tiene una cantidad menor al cristal 
            _L_Articulo.Verificar_Cantidad_Articulo_Ingresada_Servicios(Dgv_Tap3_Articulo);


            // Verifico y Apligo Garantia para trabajo convencional reservado 
            if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "09")
            {
                if (!_L_Articulo.AplicoGarantia(Dgv_Tap3_Articulo, Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), Os_Garantia_Trabajo, Numero_Examen_Garantia_Trabajo))
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje(_L_Articulo.stringBuilder.ToString());
                    _FrmMensajes.ShowDialog();

                    if (_L_Articulo.stringBuilder.ToString()== "Esta OS no aplica Reposición" && Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "09")
                    {
                        int FilaPorBorrar = e.RowIndex;
                        // Verificar si se puede borrar el artículo
                        bool puedeBorrar = _L_Articulo.VerificarYBorrarArticulo(Dgv_Tap3_Articulo, ref FilaPorBorrar);

                        if (puedeBorrar)
                        {
                            // Si se puede borrar, eliminar la fila

                            Dgv_Tap3_Articulo.Rows.RemoveAt(FilaPorBorrar);
                            return;

                        }
                    }
                }

            }


            /// Verfico y aplico Promociones 
            /// 
                if (!string.IsNullOrEmpty(Codigo_Promocion))
            {
                 Promocion_Aplicada = _L_Articulo.EjecutarPromociones(listaArticulos, Dgv_Tap3_Articulo, Cbx_Pnl2_Trbajo.Text, Cbx_Pnl2_Trbajo.SelectedValue.ToString(), Codigo_Promocion, Montura_Propia, Cristal_Propio);
                if (!Promocion_Aplicada && _L_Articulo.stringBuilder.Length > 0)
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje(_L_Articulo.stringBuilder.ToString());
                    _FrmMensajes.ShowDialog();
                }
            }

            // Totalizo el grivew Totales cuando se agrega una fila 
            _L_Articulo.ActualizarTotales(Dgv_Tap3_Articulo, Dgv_Tap3_Totales);


            // Habilito o desabilito Botones 
            Txt_Tap3_Articulo_Codigo.Text = "Código";
            ValidarRegistrosYHabilitar_Botones();

            // Actualizar_Fecha_Ofrecido 
            ActualizaFechaOfrecida(Cbx_Pnl2_Trbajo.SelectedValue.ToString(), Cbx_Pnl2_Servicio.SelectedValue?.ToString());

        }

        private void Dgv_Tap3_Articulo_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            // Verificar si eliminaron una Coloracion ; 
            // La palabra clave ref permite que la función modifique directamente la variable Codigo_Coloracion que se pasa desde la capa visual.
            _L_Articulo.RemoveColoracion(Dgv_Tap3_Articulo, ref Codigo_Coloracion);

            if (!_L_Articulo.TieneMontura(Dgv_Tap3_Articulo))
             {
                txtHorizontal.Text = "";
                txtVertical.Text = "";
                txtMaxima.Text = "";
                txtPuente.Text = "";
            }

            if (Montura_Propia)
            {
                txtHorizontal.Enabled  = true;
                txtVertical.Enabled = true;
                txtMaxima.Enabled = true;
                txtPuente.Enabled = true;
            }
            else
            {
                txtHorizontal.Enabled = false;
                txtVertical.Enabled = false;
                txtMaxima.Enabled = false;
                txtPuente.Enabled = false;
            }

            // Totalizo el grivew Totales cuando se quita una fila 
            _L_Articulo.ActualizarTotales(Dgv_Tap3_Articulo, Dgv_Tap3_Totales);

            //if (Dgv_Tap3_Articulo.Rows[FilaPorBorrar].Cells["CodArticulo"].Value.ToString().StartsWith("M"))
            //{
            //    txtHorizontal.Text = "";
            //    txtVertical.Text = "";
            //    txtMaxima.Text = "";
            //    txtPuente.Text = "";
            //}
            // Habilito o desabilito Botones 
            ValidarRegistrosYHabilitar_Botones();

        }

        private void Formato_Dgv_Totales()
        {
            try
            {
                //Centrar todas las colucnas 
                Dgv_Tap3_Totales.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Dgv_Tap3_Totales.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

                // Quitar la flecha del selector de fila
                Dgv_Tap3_Totales.RowHeadersVisible = false;

                // Deshabilitar el redimensionamiento de filas
                Dgv_Tap3_Totales.AllowUserToResizeRows = false;

                //asignar Nombres a cada colucna 
                Dgv_Tap3_Totales.Columns["Concepto"].HeaderText = "       ";
                Dgv_Tap3_Totales.Columns["Valor"].HeaderText = "       ";


                //Ancho de columna
                Dgv_Tap3_Totales.Columns["Concepto"].Width = 255;
                Dgv_Tap3_Totales.Columns["Valor"].Width = 255;

                // No modificable
                Dgv_Tap3_Totales.Columns["Concepto"].ReadOnly = true;
                Dgv_Tap3_Totales.Columns["Valor"].ReadOnly = true;



                Dgv_Tap3_Totales.Columns["Concepto"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Totales.Columns["Valor"].SortMode = DataGridViewColumnSortMode.NotSortable;


                //quitar seleccion por defecto de datagrid
                Dgv_Tap3_Totales.ClearSelection();

                //AutoGenerar Columnas:
                Dgv_Tap3_Totales.AutoGenerateColumns = false;

            }


            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }

        }

        private void Formato_Dgv_Coloracion(bool Full_Color)
        {
            try
            {
                //Centrar todas las colucnas 
                Dgv_Pnl3_Coloracion.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Dgv_Pnl3_Coloracion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

                // Quitar la flecha del selector de fila
                Dgv_Pnl3_Coloracion.RowHeadersVisible = false;

                // Deshabilitar el redimensionamiento de filas
                Dgv_Pnl3_Coloracion.AllowUserToResizeRows = false;

                //asignar Nombres a cada colucna 
                Dgv_Pnl3_Coloracion.Columns["Cod_Coloracion"].HeaderText = "       ";
                Dgv_Pnl3_Coloracion.Columns["Desc_Color"].HeaderText = "       ";
                Dgv_Pnl3_Coloracion.Columns["Porc_Material"].HeaderText = "       ";

                //Ancho de columna
                Dgv_Pnl3_Coloracion.Columns["Cod_Coloracion"].Width = 100;
                Dgv_Pnl3_Coloracion.Columns["Desc_Color"].Width = 130;
                Dgv_Pnl3_Coloracion.Columns["Porc_Material"].Width = 130;


                // No modificable
                Dgv_Pnl3_Coloracion.Columns["Cod_Coloracion"].ReadOnly = true;
                Dgv_Pnl3_Coloracion.Columns["Desc_Color"].ReadOnly = true;
                Dgv_Pnl3_Coloracion.Columns["Porc_Material"].ReadOnly = true;


                Dgv_Pnl3_Coloracion.Columns["Cod_Coloracion"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Pnl3_Coloracion.Columns["Desc_Color"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Pnl3_Coloracion.Columns["Porc_Material"].SortMode = DataGridViewColumnSortMode.NotSortable;

                Dgv_Pnl3_Coloracion.Columns["Cod_Coloracion"].Visible = true;

                if (Full_Color)
                {
                    Dgv_Pnl3_Coloracion.Columns["Desc_Color"].Visible = true;
                    Dgv_Pnl3_Coloracion.Columns["Porc_Material"].Visible = false;
                }
                else
                {
                    Dgv_Pnl3_Coloracion.Columns["Desc_Color"].Visible = false;
                    Dgv_Pnl3_Coloracion.Columns["Porc_Material"].Visible = true;

                }
                //quitar seleccion por defecto de datagrid
                Dgv_Pnl3_Coloracion.ClearSelection();

                Dgv_Pnl3_Coloracion.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Selección de filas completas

                // Evitar la Selección de Encabezados de Fila
                Dgv_Pnl3_Coloracion.RowHeadersVisible = false;

                //AutoGenerar Columnas:
                Dgv_Pnl3_Coloracion.AutoGenerateColumns = false;

            }


            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }

        }

        private void Txt_Pnl2_Examen_KeyPress(object sender, KeyPressEventArgs e)
        {
            //para que solo acepte numeros
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
            }

            //validar que no sea la tecla de borrar 
            if (e.KeyChar != (char)8)
            {
                if (Txt_Tap3_Articulo_Cantidad.Text.Length == 4)
                {
                    Txt_Tap3_Articulo_Cantidad.Focus();
                }
            }
        }

        private void Txt_Pnl3_CambioPrecioNuevo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8)
            {
                e.Handled = false;
                return;
            }


            bool IsDec = false;
            int nroDec = 0;


            if (Txt_Pnl3_CambioPrecioNuevo.SelectionLength <= 0)
            {

                for (int i = 0; i < Txt_Pnl3_CambioPrecioNuevo.Text.Length; i++)
                {
                    if (Txt_Pnl3_CambioPrecioNuevo.Text[i] == ',')
                        IsDec = true;

                    if (IsDec && nroDec++ >= 2)
                    {
                        e.Handled = true;
                        return;
                    }


                }
            }

            if (e.KeyChar >= 44 && e.KeyChar <= 57)
                e.Handled = false;
            ///46 = .
            ///46 = ,
            else if (e.KeyChar == 46)
                e.Handled = (IsDec) ? true : false;
            else
                e.Handled = true;


            //para que solo acepte numeros y una sola coma
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }


            if (Txt_Pnl3_CambioPrecioNuevo.Text.Contains("") && e.KeyChar == 44)
            {
                //separamos por punto
                string[] parts = Txt_Pnl3_CambioPrecioNuevo.Text.Split(',');
                //si el primer elemento está vacío, significa que no se escribió nada antes de, entonces, añadimos el cero al textbox.
                if (parts[0].Length <= 0)
                {
                    Txt_Pnl3_CambioPrecioNuevo.Text = "0" + Txt_Pnl3_CambioPrecioNuevo.Text;
                    //UPDATE: colocamos el cursor al final del texto
                    Txt_Pnl3_CambioPrecioNuevo.SelectionStart = Txt_Pnl3_CambioPrecioNuevo.Text.Length;
                }
            }
        }

        private void Dgv_Tap3_Articulo_KeyDown(object sender, KeyEventArgs e)
        {
            // Verificar si se presionó la tecla F3 Para Abrir el Panel de Cambio Precio
            if (e.KeyCode == Keys.F3 && Btn_Tap3_CambioPrecio.Enabled)
            {
                // Verificar si hay una fila seleccionada
                if (Dgv_Tap3_Articulo.CurrentRow != null)
                {
                    // Obtener el índice de la fila seleccionada
                    filaSeleccionada = Dgv_Tap3_Articulo.CurrentRow.Index;

                    //Verifico si Tiene articulos Asociados 
                    if (_L_Articulo.AccionCambiarPrecio_ArticuloPadre(Dgv_Tap3_Articulo, filaSeleccionada))
                    {
                        return;
                    }
                    else
                    {
                        _FrmClaveGerente.ShowDialog();
                        if (_FrmClaveGerente.ClaveCorrecta == true)
                        {

                            if (_L_Articulo.ConfigurarCambioPrecio(Txt_Pnl3_CambioPrecioActual, Dgv_Tap3_Articulo.Rows[filaSeleccionada].Cells["CodArticulo"].Value.ToString()))
                            {
                                VisualizarPanel("CambioPrecio");
                                HabilitacionControl("Habilitar_CambioPrecio");
                            }
                            else
                            {
                                _FrmMensajes.co = 2;
                                _FrmMensajes.avisomensaje("Error al Cargar el Panel de Cambio de Precio");
                                _FrmMensajes.ShowDialog();
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
            else if (e.KeyCode == Keys.F4 && Btn_Tap3_Descuento.Enabled)
            {
                // Verificar si hay una fila seleccionada
                if (Dgv_Tap3_Articulo.CurrentRow != null)
                {
                    // Obtener el índice de la fila seleccionada
                    filaSeleccionada = Dgv_Tap3_Articulo.CurrentRow.Index;
                    Tipo_Descuento = "Descuento por articulo";
                    VisualizarPanel("Descuento");
                    HabilitacionControl("Habilitar_Descuento");
                    _L_Articulo.Cargo_CodMotivo_Descuento(Cbx_Pnl3_MotivoDesc);
                }
            }
        }

        private void Btn_Tap3_Aceptar_CambioPrecio_Click(object sender, EventArgs e)
        {
            Decimal DesMaximo = 0;
            if (_L_Articulo.CambioPrecio(Txt_Pnl3_CambioPrecioNuevo, Txt_Pnl3_CambioPrecioActual, DesMaximo))
            {
                if (_L_Articulo.stringBuilder.Length > 0)
                {
                    _FrmMensajes.co = 3;
                    _FrmMensajes.avisomensaje(_L_Articulo.stringBuilder.ToString());
                    _FrmMensajes.ShowDialog();

                    if (_FrmMensajes.DialogResult == DialogResult.OK)
                    {
                        _FrmClaveAutorizada.ShowDialog();

                        if (_FrmClaveAutorizada.DialogResult == DialogResult.OK && _FrmClaveAutorizada.ClaveCorrecta == true)
                        {
                            _L_Articulo.ActualizarCelda(Dgv_Tap3_Articulo, filaSeleccionada, "ART_PVP", Txt_Pnl3_CambioPrecioNuevo.Text);
                            CerrarPanelCambioPrecio();
                            //BotonesColor(false, "Cambio Precio");

                        }

                    }
                }
                else
                {
                    _L_Articulo.ActualizarCelda(Dgv_Tap3_Articulo, filaSeleccionada, "ART_PVP", Txt_Pnl3_CambioPrecioNuevo.Text);
                    CerrarPanelCambioPrecio();
                    //BotonesColor(false, "Cambio Precio");
                }

                _L_Articulo.ActualizarTotales(Dgv_Tap3_Articulo, Dgv_Tap3_Totales);
            }
            else
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(_L_Articulo.stringBuilder.ToString());
                _FrmMensajes.ShowDialog();
            }

        }

        private void CerrarPanelCambioPrecio()
        {
            VisualizarPanel("MostrarCabezeraSecundaria");
            HabilitacionControl("CabezeraPrincipal");
            LimpiarControles("CambioPrecio");
        }

        private void CerrarPanelDescuento()
        {
            VisualizarPanel("MostrarCabezeraSecundaria");
            HabilitacionControl("CabezeraPrincipal");
            LimpiarControles("Descuento");
        }

        private void CerrarPanelColoracion()
        {
            VisualizarPanel("MostrarCabezeraSecundaria");
            HabilitacionControl("CabezeraPrincipal");
            LimpiarControles("Coloracion");
        }

        private void Rd_Pnl3_FullColor_CheckedChanged(object sender, EventArgs e)
        {
            // Verificar si el RadioButton está seleccionado
            if (Rd_Pnl3_FullColor.Checked)
            {
                // Llamar a la función con el valor true
                Formato_Dgv_Coloracion(true);
            }
        }

        private void Rd_Pnl3_Degradado_CheckedChanged(object sender, EventArgs e)
        {
            // Verificar si el RadioButton está seleccionado
            if (Rd_Pnl3_Degradado.Checked)
            {
                // Llamar a la función con el valor true
                Formato_Dgv_Coloracion(false);
            }
        }

        private void Btn_Tap3_Cancelar_CambioPrecio_Click(object sender, EventArgs e)
        {
            CerrarPanelCambioPrecio();
        }

        private void Btn_Tap3_Aceptar_Coloracion_Click(object sender, EventArgs e)
        {
            if (Dgv_Pnl3_Coloracion.SelectedRows.Count > 0)
            {
                // Acceder a la primera fila seleccionada
                DataGridViewRow filaSeleccionada = Dgv_Pnl3_Coloracion.SelectedRows[0];

                // Obtener el valor de la celda "Cod_Coloracion"
                Codigo_Coloracion = filaSeleccionada.Cells["Cod_Coloracion"].Value.ToString();
                CerrarPanelColoracion();
            }
            else
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Debe seleccionar una codigo para continuar");
                _FrmMensajes.ShowDialog();
            }

        }

        private void Btn_Tap3_Cancelar_Desc_Click(object sender, EventArgs e)
        {
            CerrarPanelDescuento();
        }
        private void Btn_Tap3_Cancelar_Coloracion_Click(object sender, EventArgs e)
        {
            CerrarPanelColoracion();
        }

        private void Btn_Tap3_Descuento_Click(object sender, EventArgs e)
        {

            if  (Dgv_Tap3_Articulo.CurrentCell != null && Dgv_Tap3_Articulo.SelectedCells.Count > 0)
            {
                if (Dgv_Tap3_Articulo.CurrentRow != null)
                {
                    // Obtener el índice de la fila seleccionada
                    filaSeleccionada = Dgv_Tap3_Articulo.CurrentRow.Index;
                    Tipo_Descuento = "Descuento por articulo";
                    VisualizarPanel("Descuento");
                    HabilitacionControl("Habilitar_Descuento");
                    _L_Articulo.Cargo_CodMotivo_Descuento(Cbx_Pnl3_MotivoDesc);
                }
            }
                    
            else
            {
                //ActivoInactivoBtn("Descuento");
                Tipo_Descuento = "Descuento Global";
                VisualizarPanel("Descuento");
                HabilitacionControl("Habilitar_Descuento");
                _L_Articulo.Cargo_CodMotivo_Descuento(Cbx_Pnl3_MotivoDesc);
            }
        }

        private void Btn_Tap3_Aceptar_Desc_Click(object sender, EventArgs e)
        {
            try
            {
                // Por descuento
                if (!string.IsNullOrEmpty(Txt_Pnl3_ObservacionDesc.Text) && !string.IsNullOrEmpty(Txt_Pnl3_PorcDescuento.Text) && !string.IsNullOrEmpty(Txt_Pnl3_MontoDesc.Text))
                {
                    if (!string.IsNullOrEmpty(Cbx_Pnl3_MotivoDesc.Text))
                    {
                        if (Tipo_Descuento == "Descuento Global")
                        {
                            // Verifico se el Porcentaje de descuento esta por encima dle permitido para generar una clave autorizada diferente 

                            if (_L_Articulo.VerificarTopeMaximoDesceunto(Convert.ToDecimal(Txt_Pnl3_PorcDescuento.Text)))
                            {   // Pido Clave Autorizada con unos parametros especificos
                                _FrmClaveAutorizada.Nuevo_Parametro = true;
                                _FrmClaveAutorizada.Parametro_Nuevo = _L_Articulo.BuscarCodigoGerenteDescuento(Cbx_Pnl3_MotivoDesc.SelectedValue.ToString());
                            }

                            _FrmClaveAutorizada.ShowDialog();

                            if (_FrmClaveAutorizada.DialogResult == DialogResult.OK && _FrmClaveAutorizada.ClaveCorrecta == true)
                            {
                                 Codmotivodes = Cbx_Pnl3_MotivoDesc.SelectedValue.ToString();
                                _L_Articulo.ActualizarTodasCelda(Dgv_Tap3_Articulo, "PORCTDESCUENTO", Txt_Pnl3_PorcDescuento.Text);

                            }


                        }
                        else if (Tipo_Descuento == "Descuento por articulo")
                        {
                            // Verifico se el Porcentaje de descuento esta por encima dle permitido para generar una clave autorizada diferente 

                            if (_L_Articulo.VerificarTopeMaximoDesceunto(Convert.ToDecimal(Txt_Pnl3_PorcDescuento.Text)))
                            {   // Pido Clave Autorizada con unos parametros especificos
                                _FrmClaveAutorizada.Nuevo_Parametro = true;
                                _FrmClaveAutorizada.Parametro_Nuevo = _L_Articulo.BuscarCodigoGerenteDescuento(Cbx_Pnl3_MotivoDesc.SelectedValue.ToString());
                            }

                            // Pido Clave Autorizada con unos parametros especificos
                            _FrmClaveAutorizada.Nuevo_Parametro = true;
                            _FrmClaveAutorizada.Parametro_Nuevo = _L_Articulo.BuscarCodigoGerenteDescuento(Cbx_Pnl3_MotivoDesc.SelectedValue.ToString());
                            _FrmClaveAutorizada.ShowDialog();

                            if (_FrmClaveAutorizada.DialogResult == DialogResult.OK && _FrmClaveAutorizada.ClaveCorrecta == true)
                            {
                                Codmotivodes = Cbx_Pnl3_MotivoDesc.SelectedValue.ToString();
                                _L_Articulo.ActualizarCelda(Dgv_Tap3_Articulo, Dgv_Tap3_Articulo.CurrentRow.Index, "PORCTDESCUENTO", Txt_Pnl3_PorcDescuento.Text);

                            }
                        }

                        // Totalizo el grivew Totales cuando se agrega o modifica una fila 
                        _L_Articulo.ActualizarTotales(Dgv_Tap3_Articulo, Dgv_Tap3_Totales);
                        // Cierro el panel, limpio controles y Retorno a la pantalla primcipal 
                        CerrarPanelDescuento();
                        //BotonesColor(false, "Descuento");

                    }
                    else
                    {

                        Cbx_Pnl3_MotivoDesc.Focus();
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("Debe seleccionar un Motivo de Descuento para continuar");
                        _FrmMensajes.ShowDialog();
                    }
                }
                else
                {
                    Txt_Pnl3_ObservacionDesc.Focus();
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("Se requiere una observación para continuar");
                    _FrmMensajes.ShowDialog();
                }



            }
            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }

            finally
            {
                _FrmClaveAutorizada.Nuevo_Parametro = false;
                _FrmClaveAutorizada.Parametro_Nuevo = "";
            }
        }

        private void Txt_Pnl3_MontoDesc_Validating(object sender, CancelEventArgs e)
        {
            if (Txt_Pnl3_MontoDesc.Text == "" && string.IsNullOrEmpty(Txt_Pnl3_MontoDesc.Text))
            {
                Txt_Pnl3_MontoDesc.Text = "0,00";
            }
            else
            {
                FormatoBs(Convert.ToDouble(Txt_Pnl3_MontoDesc.Text), Txt_Pnl3_MontoDesc);

            }
        }

        private void FormatoBs(Double Bolivares, System.Windows.Forms.TextBox BolivaresDeseados)
        {
            try
            {
                BolivaresDeseados.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", Bolivares).Replace(".", ",");
                BolivaresDeseados.Text = String.Format("{0:#,0.00}", Bolivares);

            }
            catch (Exception ex)
            {
                string error = (string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.avisomensaje(error);
                _FrmMensajes.ShowDialog();

            }

        }

        private void Txt_Pnl3_MontoDesc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8)
            {
                e.Handled = false;
                return;
            }


            bool IsDec = false;
            int nroDec = 0;


            if (Txt_Pnl3_MontoDesc.SelectionLength <= 0)
            {

                for (int i = 0; i < Txt_Pnl3_MontoDesc.Text.Length; i++)
                {
                    if (Txt_Pnl3_MontoDesc.Text[i] == ',')
                        IsDec = true;

                    if (IsDec && nroDec++ >= 2)
                    {
                        e.Handled = true;
                        return;
                    }


                }
            }

            if (e.KeyChar >= 44 && e.KeyChar <= 57)
                e.Handled = false;
            ///46 = .
            ///46 = ,
            else if (e.KeyChar == 46)
                e.Handled = (IsDec) ? true : false;
            else
                e.Handled = true;


            //para que solo acepte numeros y una sola coma
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }


            if (Txt_Pnl3_MontoDesc.Text.Contains("") && e.KeyChar == 44)
            {
                //separamos por punto
                string[] parts = Txt_Pnl3_MontoDesc.Text.Split(',');
                //si el primer elemento está vacío, significa que no se escribió nada antes de, entonces, añadimos el cero al textbox.
                if (parts[0].Length <= 0)
                {
                    Txt_Pnl3_MontoDesc.Text = "0" + Txt_Pnl3_MontoDesc.Text;
                    //UPDATE: colocamos el cursor al final del texto
                    Txt_Pnl3_MontoDesc.SelectionStart = Txt_Pnl3_MontoDesc.Text.Length;
                }
            }
        }

        private void Txt_Pnl3_MontoDesc_Leave(object sender, EventArgs e)
        {
            if (!_L_Articulo.CalculoDescuento(Dgv_Tap3_Totales.Rows[4].Cells["Valor"].Value.ToString(), Dgv_Tap3_Articulo, Tipo_Descuento, "0.00", Txt_Pnl3_PorcDescuento, Txt_Pnl3_MontoDesc, Txt_Pnl3_ObservacionDesc))
            {
                //Txt_Pnl3_PorcDescuento.Text = "";
                //Txt_Pnl3_MontoDesc.Text = "";
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(_L_Articulo.stringBuilder.ToString());
                _FrmMensajes.ShowDialog();
            }
        }

        private void Txt_Pnl3_PorcDescuento_Validating(object sender, CancelEventArgs e)
        {
            if (Txt_Pnl3_PorcDescuento.Text == "" && string.IsNullOrEmpty(Txt_Pnl3_PorcDescuento.Text))
            {
                Txt_Pnl3_PorcDescuento.Text = "0,00";
            }
            else
            {
                FormatoBs(Convert.ToDouble(Txt_Pnl3_PorcDescuento.Text), Txt_Pnl3_PorcDescuento);

            }
        }

        private void Txt_Pnl3_PorcDescuento_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8)
            {
                e.Handled = false;
                return;
            }


            bool IsDec = false;
            int nroDec = 0;


            if (Txt_Pnl3_PorcDescuento.SelectionLength <= 0)
            {

                for (int i = 0; i < Txt_Pnl3_PorcDescuento.Text.Length; i++)
                {
                    if (Txt_Pnl3_PorcDescuento.Text[i] == ',')
                        IsDec = true;

                    if (IsDec && nroDec++ >= 2)
                    {
                        e.Handled = true;
                        return;
                    }


                }
            }

            if (e.KeyChar >= 44 && e.KeyChar <= 57)
                e.Handled = false;
            ///46 = .
            ///46 = ,
            else if (e.KeyChar == 46)
                e.Handled = (IsDec) ? true : false;
            else
                e.Handled = true;


            //para que solo acepte numeros y una sola coma
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }


            if (Txt_Pnl3_PorcDescuento.Text.Contains("") && e.KeyChar == 44)
            {
                //separamos por punto
                string[] parts = Txt_Pnl3_PorcDescuento.Text.Split(',');
                //si el primer elemento está vacío, significa que no se escribió nada antes de, entonces, añadimos el cero al textbox.
                if (parts[0].Length <= 0)
                {
                    Txt_Pnl3_PorcDescuento.Text = "0" + Txt_Pnl3_PorcDescuento.Text;
                    //UPDATE: colocamos el cursor al final del texto
                    Txt_Pnl3_PorcDescuento.SelectionStart = Txt_Pnl3_PorcDescuento.Text.Length;
                }
            }
        }

        private void Txt_Pnl3_PorcDescuento_Leave(object sender, EventArgs e)
        {
            if (!_L_Articulo.CalculoDescuento(Dgv_Tap3_Totales.Rows[4].Cells["Valor"].Value.ToString(), Dgv_Tap3_Articulo, Tipo_Descuento, "0.00", Txt_Pnl3_PorcDescuento, Txt_Pnl3_MontoDesc, Txt_Pnl3_ObservacionDesc))
            {
                //Txt_Pnl3_PorcDescuento.Text = "";
                //Txt_Pnl3_MontoDesc.Text = "";
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(_L_Articulo.stringBuilder.ToString());
                _FrmMensajes.ShowDialog();
            }
        }

        private void ValidarRegistrosYHabilitar_Botones()
        {
            // Verificar si el DataGridView tiene filas que no sean nuevas
           
           //// Descuento  
            if (Dgv_Tap3_Articulo.Rows.Count > 0 && PorcDctoEmpresaAfiliada <= 0 && string.IsNullOrEmpty(Codigo_Promocion))
            {
                Btn_Tap3_Descuento.Enabled = true; // Habilitar el TextBox o botón descuento
                Btn_Tap3_CambioPrecio.Enabled = true; // Habilitar el TextBox o botón cambioPrecio
                BotonesColor(true, "cambioprecio");
                BotonesColor(true, "descuento");
            }
            else
            {
                Btn_Tap3_Descuento.Enabled = false; // Deshabilitar el TextBox o botón
                Btn_Tap3_CambioPrecio.Enabled = false;
                BotonesColor(false, "cambioprecio");
                BotonesColor(false, "descuento");
            }

            //// Promociones
            if (string.IsNullOrEmpty(Codigo_Promocion) && Dgv_Tap3_Articulo.Rows.Count <= 0 && string.IsNullOrEmpty(EmpresaAfiliada))
                Btn_Tap3_Promocion.Enabled = true;
            else
                Btn_Tap3_Promocion.Enabled = false;

            //// EmpresasAfiliadas
            if ((string.IsNullOrEmpty(EmpresaAfiliada) && Dgv_Tap3_Articulo.Rows.Count <= 0 && string.IsNullOrEmpty(Codigo_Promocion)) && Cbx_Pnl2_Trbajo.SelectedValue.ToString() != "09")
                Btn_Tap3_ClienteAfiliado.Enabled = true;
            else
                Btn_Tap3_ClienteAfiliado.Enabled = false;

            // Montura Propia 
            _L_Articulo.VerificarMonturaPropia(Dgv_Tap3_Articulo, Btn_Tap3_MonturaPropia, Montura_Propia);

            // Cristal Propio
            _L_Articulo.VerificarCristalPropio(Dgv_Tap3_Articulo, Btn_Tap3_CristalPropio, Cristal_Propio);

            // Boton de Garantia y // Cristal Propio
            if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "09")
            {
                Btn_Tap3_Garantia.Enabled = false;
                Btn_Tap3_CristalPropio.Enabled = false;
            }
            else
            {
                Btn_Tap3_Garantia.Enabled = Garantia ? false : true;
                Btn_Tap3_CristalPropio.Enabled = true;
            }

            // Mostar o no el tipo de laboratirio y srevicio 
            ValidarTipoVenta(Cbx_Pnl2_Trbajo.Text);

            if (_D_DetalleOrden.TB_PARAMETRO("EditarGridFact") == "1" && Txt_Tap3_Articulo_Codigo.Text.Trim() != "Código")
            {
                Txt_Tap3_Articulo_Precio.Enabled = true;
                Txt_Tap3_Articulo_Precio.ForeColor = Color.Black;
                //Txt_Tap3_Articulo_Precio.BackColor = ColorTranslator.FromHtml("#002222");
            }
            else
            {
                Txt_Tap3_Articulo_Precio.Enabled = false;
                Txt_Tap3_Articulo_Precio.ForeColor = Color.DarkGray;
                Txt_Tap3_Articulo_Precio.BackColor = ColorTranslator.FromHtml("#ffffff");
            }

        }

        private void Dgv_Tap3_Totales_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Verificar que no sea una celda de encabezado
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Dibujar el fondo de la celda
                e.PaintBackground(e.ClipBounds, true);

                // Dibujar el contenido de la celda
                e.PaintContent(e.ClipBounds);

                // Crear un color para el borde interno
                using (Pen pen = new Pen(Color.FromArgb(7, 167, 155), 0.5f)) // Color deseado personalizado; // Grosor de 0.5 píxeles
                {
                    // Dibujar el borde derecho
                    e.Graphics.DrawLine(pen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);

                    // Dibujar el borde inferior
                    e.Graphics.DrawLine(pen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);

                    // Dibujar el borde izquierdo
                    e.Graphics.DrawLine(pen, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Left, e.CellBounds.Bottom);

                    // Dibujar el borde superior
                    e.Graphics.DrawLine(pen, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Right, e.CellBounds.Top);
                }

                // Indicar que el evento ha sido manejado
                e.Handled = true;
            }

            // Verificar si es la primera columna (índice 0)
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                // Aplicar estilo en negrita
                e.CellStyle.Font = new Font(Dgv_Tap3_Totales.Font, FontStyle.Bold);
            }
        }

        private void CargarComboLaboratorios()
        {
            _L_Articulo.ComboLaboratorio(Cbx_Pnl2_Trbajo, Cbx_Pnl2_Laboratorio, _D_Inicio.Sucursal());
            //_L_Articulo.Co
            //mboLaboratorio()
            //Cbx_Pnl2_Laboratorio.DataSource = lista;
            //Cbx_Pnl2_Laboratorio.ValueMember = "CODIGO_LAB";
            //Cbx_Pnl2_Laboratorio.DisplayMember = "DESCRIPCION";
        }
        private void CargarComboServicioLaboratorios()
        {
            //Cbx_Pnl2_Laboratorio.SelectedIndex = 0;
           _L_Articulo.ObtenerServicioLaboratorioCbx(Cbx_Pnl2_Servicio, Cbx_Pnl2_Trbajo, Cbx_Pnl2_Laboratorio, _D_Inicio.Sucursal() ,Cbx_Pnl2_Laboratorio.SelectedValue.ToString());

           
        }
        public (string visionIzquierda, string visionDerecha) ObtenerTiposVision()
        {
            string tipoVision = Cbx_Tap2_Ojo.Text.ToString(); // ComboBox principal
            string visionIzq = cbVisionIzquierda.Text.ToString();  // ComboBox visión izquierda
            string visionDer = cbVisionDerecha.Text.ToString();   // ComboBox visión derecha

            // Aplicar reglas de negocio
            switch (tipoVision)
            {
                case "Izquierdo":
                    return (visionIzq, ""); // Solo visión izquierda, derecha vacía

                case "Derecho":
                    return ("", visionDer); // Solo visión derecha, izquierda vacía

                case "Ambos":
                    return (visionIzq, visionDer); // Ambas visiones con sus valores

                default:
                    return ("", ""); // Caso no especificado
            }
        }

        private async void Btn_Tap3_Procesar_Click(object sender, EventArgs e)
        {
            try
            {
                //if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "01")
                //{
                //    if ((Convert.ToInt32(txtAltD.Text) < 10 || Convert.ToInt32(txtAltD.Text) > 35) || Convert.ToInt32(txtAltI.Text) < 10 || Convert.ToInt32(txtAltI.Text) > 35)
                //    {
                //        _FrmMensajes.co = 2;
                //        _FrmMensajes.avisomensaje("Valor inválido, rango entre 10 y 35");
                //        _FrmMensajes.ShowDialog();
                //        return;
                //    }
                //}

                if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "09")
                {
                    bool TieneMontura= false;
                    // Verificar si tiene  Montura
                    foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
                    {
                        if (row.Cells["CodArticulo"].Value != null && (row.Cells["CodArticulo"].Value.ToString().StartsWith("M") || row.Cells["CodArticulo"].Value.ToString().StartsWith("L")))
                        {
                            TieneMontura = true;
                        }
                        else
                        {
                            TieneMontura = false;
                        }
                    }

                    if (TieneMontura == false && Montura_Propia== false)
                    {
                        Btn_Tap3_MonturaPropia.PerformClick();
                        return;
                    }
                }


                if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() != "04" && Cbx_Pnl2_Trbajo.SelectedValue.ToString() != "05")
                {
                    if (!ValidarVisionConv())
                    {
                        return;
                    }
                }

                //Btn_Tap3_Procesar.Enabled = false;
                //LLenar_TbTrabajo();

                int car;
                string strMarcaC;
                bool cambioPrec = false;
                string codCristal = "";
                string codMonturaSeleccionada = "";
                glbServicio_NUV = glbServicio;
                Boolean Posee_Montura = false;
                Boolean Posee_Cristal = false;

                // 1. Obtener códigos desde el grid
                var codigosFactura = ObtenerCodigosDesdeGrid();

                if (codigosFactura == null || codigosFactura.Count == 0)
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("No hay códigos válidos para procesar");
                    _FrmMensajes.ShowDialog();
                    return;
                }

                if ((Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "01" || Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "09" ) && (
        string.IsNullOrEmpty(txtHorizontal.Text) ||
        string.IsNullOrEmpty(txtVertical.Text) ||
        string.IsNullOrEmpty(txtMaxima.Text) ||
        string.IsNullOrEmpty(txtPuente.Text)
    ))
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("Ingrese las medidas de la montura");
                    _FrmMensajes.ShowDialog();
                    return;
                }

                if(Montura_Propia  & txtObservacion.Text == "")
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("Debe escribir una descripción de la montura para poder procesar los datos");
                    _FrmMensajes.ShowDialog();
                    return;
                }

                foreach (string elemento in codigosFactura)
                {
                    string codigo =elemento;
                    if (codigo.StartsWith("C"))
                    {
                        Posee_Cristal = true;
                        codCristal = codigo;
                    }
                    else if (codigo.StartsWith("M")|| codigo.StartsWith("L"))
                    {
                        Posee_Montura = true;
                        codMonturaSeleccionada = codigo;
                    }
                       
                }


                if ((Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "01"|| Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "08") && !Posee_Cristal && !Cristal_Propio)
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("No se encontró ningún código de cristal válido");
                    _FrmMensajes.ShowDialog();
                    return;
                }

                if ((Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "01" || Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "08" ) && !Posee_Montura && !Montura_Propia)
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("No se encontró ningún código de Montura válido");
                    _FrmMensajes.ShowDialog();
                    return;
                }

                //Consultar servicios AR
                var dsAR = await _servicioValidaciones.ObtenerServiciosARDataset(codCristal, false, null);

                if (_servicioValidaciones.VerificoIgualAntirefCrist(Dgv_Tap3_Articulo, dsAR))
                {
                    if (Garantia == true)
                    {
                        _L_Articulo.VerificarServicioCodigoPadre(Dgv_Tap3_Articulo);

                   
                        _L_Articulo.CargarServicioGarantia(Dgv_Tap3_Articulo);

                    }
                    //MessageBox.Show("Este examen debe poseer DNP valida para este tipo de vision.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _L_Articulo.ActualizarTotales(Dgv_Tap3_Articulo, Dgv_Tap3_Totales);
                    //Btn_Tap3_Procesar.Enabled = true;
                    return;
                }

                string codSucursal;
                codSucursal = _D_DetalleOrden.TB_PARAMETRO("SucursalId");
                DateTime Fecha_Ofreci;
                string Hora_Ofrecido;
                CultureInfo cultura = new CultureInfo("es-ES");
                cultura.DateTimeFormat.AMDesignator = "a.m.";
                cultura.DateTimeFormat.PMDesignator = "p.m.";
                if (_FechaHoraOfrecida != null && _FechaHoraOfrecida.Count > 0)
                {
                    FechaHoraOfrecida resultado = _FechaHoraOfrecida.First();
                    Fecha_Ofreci = resultado.FechaOfrecida.Date;
                    string horaNormalizada = resultado.HoraOfrecida.ToLower().Replace("a. m.", "a.m.").Replace("p. m.", "p.m.").Replace("a.m.", "a.m.").Replace("p.m.", "p.m.");  // Eliminar espacios adicionales

                    // Parsear con el formato correcto
                    //DateTime hora = DateTime.ParseExact(horaNormalizada, "HH:mm:sstt", CultureInfo.InvariantCulture);

                    Hora_Ofrecido = horaNormalizada;

                    // 1. Reemplazar cualquier tipo de espacio entre "p." y "m." (incluyendo NO-BREAK SPACE)
                    Hora_Ofrecido = Regex.Replace(Hora_Ofrecido, @"p\.\s*m\.", "p.m.", RegexOptions.IgnoreCase);

                    // 2. Reemplazar cualquier tipo de espacio entre "a." y "m." (por si hay AM)
                    Hora_Ofrecido = Regex.Replace(Hora_Ofrecido, @"a\.\s*m\.", "a.m.", RegexOptions.IgnoreCase);

                    // 3. Eliminar espacios adicionales antes del AM/PM
                    Hora_Ofrecido = Hora_Ofrecido.Trim();

                    //// Formatear de vuelta al formato deseado
                    //Hora_Ofrecido = hora.ToString("hh:mm:ss tt", cultura)
                    //    .Replace("a.m.", "a.m.")
                    //    .Replace("p.m.", "p.m.");
                }
                else
                {
                    Fecha_Ofreci = DateTime.Today;
                    Hora_Ofrecido = DateTime.Now.ToString("hh:mm:ss tt", cultura).Replace("a.m.", "a.m.").Replace("p.m.", "p.m.");
                    //Hora_Ofrecido = DateTime.Now.ToString("HH:mm:ss: tt");
                }

                //_D_Articulo.Agregar_TB_TRABAJO(codSucursal, "", "", Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), "002", Convert.ToInt32(Txt_Pnl2_Examen.Text)
                //, txtHorizontal.Text, txtVertical.Text, txtMaxima.Text, txtPuente.Text, "0", "0", "A", "Cerca", "Cerca", "QUO", "001", "T", TB_USUARIO.COD_USR, "02", "CONVENCIONAL", "0", "0", "0", "0");

                //VerificoParametrosCristales();
                //VerificoRangoDiametroCristales();

                // VerificarCantidad Cantidad de Ojo y Cristal 
                if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() != "04" && Cbx_Pnl2_Trbajo.SelectedValue.ToString() != "05" && Cristal_Propio == false && !_L_Articulo.VerificoCantidadCristales(Cbx_Tap2_Ojo.Text, Dgv_Tap3_Articulo, mostrarError))
                {
                    return; // Salir 
                }

                LLenar_TbTrabajo();
                AsignarParametrosFaltantes(nuevoTrabajo,"", "0", txtHorizontal.Text.Replace(',', '.'), txtVertical.Text.Replace(',', '.'), txtMaxima.Text.Replace(',', '.'), txtPuente.Text.Replace(',', '.'), Cbx_Pnl2_Laboratorio.SelectedValue.ToString(), Cbx_Pnl2_Servicio.SelectedValue?.ToString(), Hora_Ofrecido, "T", Fecha_Ofreci, DateTime.Today, Cbx_Pnl2_Trbajo.SelectedValue.ToString(), Cbx_Pnl2_Trbajo.Text);
                _L_Trabajo.AgregarTrabajo(nuevoTrabajo);

                // Validar Tipo de Ojo Y Tipo de Vision 
                LLenarEntidadExamenFitcon();
                List<TB_FICCONV> nuevoFiccont2 = _D_Articulo.ObtenerRx(Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), codSucursal, Convert.ToInt32( Txt_Tap2_Examen.Text));
                bool Respuesta2 = _Asignar_Rx.ValidarExamenOptico(mostrarError, mostrarPregunta, _TRABAJO, nuevoExamen, nuevoFiccont2, ObtenerTiposVision().visionDerecha, ObtenerTiposVision().visionIzquierda, Cbx_Tap2_Ojo, Cbx_Pnl2_Trbajo,
                Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), Txt_Tap2_Examen.Text, Txt_Pnl2_Cedula.Text.Substring(0, 1), codSucursal, txtAltD, txtAltI);
                if (!Respuesta2)
                { 
                return; // Salir 
                }

                bool Respuesta = _Asignar_Rx.Verificar_Cristales_Parametros_Diametros(nuevoTrabajo, Dgv_Tap3_Articulo, Cbx_Pnl2_Trbajo.SelectedValue.ToString(), codSucursal, Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), Txt_Tap2_Examen.Text, LbResultado2, LbResultados, dgvRangoCrt);
                if (!ApruebaAORangoCRT && !Respuesta && _Asignar_Rx.stringBuilder.Length > 0)
                {

                    if (_Asignar_Rx.stringBuilder.ToString() == "El cristal seleccionado no se adapta a los siguientes rangos")
                    {
                        LbResultados.Visible = false;
                        LbResultado2.Visible = false;
                        lblDiametroD.Text = _Asignar_Rx.diamDgl;
                        lblDiametroI.Text = _Asignar_Rx.diamIgl;
                        lblDiametroD.Visible = lblDiametroI.Visible = true;
                        LblTitulo.Text = _Asignar_Rx.stringBuilder.ToString();
                        lblClaveAut.Visible = true;
                        lblLeyenda.Visible = false;
                        btnAutorizarRangosCrt.Visible = true;
                        pnlRangoCrt.Show();
                        pnlRangoCrt.Location = new Point(200, 150);
                        FormatoTablaRango();

                        //Titilo Diametro
                        lblTituloDiam.Location = new Point(19, 160);

                        //Titulo Derecho Izquierdo
                        label19.Location = new Point(161, 160); //D
                        label23.Location = new Point(212, 160); //I

                        //Valores Derecho Izquierdo
                        lblDiametroD.Location = new Point(165, 180); //D
                        lblDiametroI.Location = new Point(212, 180); //I

                        // Mensaje de clave Autorizada 
                        lblClaveAut.Location = new Point(10, 235);

                        return;

                    }
                    else if (_Asignar_Rx.stringBuilder.ToString() == "El cristal no se adapta a estos parámetros")
                    {
                        lblDiametroD.Visible = false;
                        lblDiametroI.Visible = false;
                        LblTitulo.Text = _Asignar_Rx.stringBuilder.ToString();
                        lblClaveAut.Visible = false;
                        lblLeyenda.Visible = true;
                        btnAutorizarRangosCrt.Visible = false;
                        LbResultados.Visible = true;
                        LbResultado2.Visible = true;
                        pnlRangoCrt.Show();
                        pnlRangoCrt.Location = new Point(200, 150);
                        FormatoDataGridRangosCristales();
                        
                        //Titilo Diametro
                        lblTituloDiam.Location = new Point(19, 194);

                        //Titulo Derecho Izquierdo
                        label19.Location = new Point(161, 194); //D
                        label23.Location = new Point(212, 194); //I

                        //Valores Derecho Izquierdo
                        lblDiametroD.Location = new Point(165, 214); //D
                        lblDiametroI.Location = new Point(212, 214); //I

                        // Mensaje de clave Autorizada 
                        lblClaveAut.Location = new Point(10, 269);

                        return;
                    }
                    else
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje(_Asignar_Rx.stringBuilder.ToString());
                        _FrmMensajes.ShowDialog();
                        return; // Salir 
                    }
                }


                string codModo = Cbx_Pnl2_Trbajo.SelectedValue?.ToString();
                string tipoTrabajoVenta = _L_Articulo.ObtenerTipoVentaPorModo(codModo);

                //Obtener datos para GuardarOrdenServicioAsync
                var datos = new ValidacionEstucheDTO
                {
                    TipoTrabajo = tipoTrabajoVenta,
                    Observacion = txtObservacion.Text,
                    Garantia = Garantia,
                    MonturaPropia = Montura_Propia,
                    CodigosDesdeGrid = ObtenerCodigosDesdeGrid()
                };

                string letraInicial, numeroCedula;
                _L_Articulo.DividirValoresCedula(Txt_Pnl2_Cedula.Text, out letraInicial, out numeroCedula);


                if ((Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "01"))//Convencional
                {
                    string valida = _servicioValidaciones.ValidarMonturaQuorumYCristales(Dgv_Tap3_Articulo,Cbx_Pnl2_Laboratorio.SelectedValue.ToString(),
                                    TB_USUARIO.COD_SUCURSAL,Cbx_Pnl2_Servicio.SelectedValue.ToString(),letraInicial,numeroCedula,Txt_Pnl2_Examen.Text, mostrarError, null);
  
                }


                if (!await _servicioValidaciones.ValidarOrdenServicioAsync(Dgv_Tap3_Totales,Dgv_Tap3_Articulo,null, mostrarPregunta, mostrarError, datos))
                {
                    return;
                }


                // Validar Lc CodigoLabLC
                 if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "02" && !_L_Articulo.VerificoCodigoLabLC(Cbx_Tap2_Ojo.Text, Dgv_Tap3_Articulo, Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), Convert.ToInt16(Txt_Pnl2_Examen.Text)) && _L_Articulo.stringBuilder.Length > 0)
                 {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje(_L_Articulo.stringBuilder.ToString());
                        _FrmMensajes.ShowDialog();
                        return; // Salir 
                 }

                // validar cantidad Maxima 
                if (!_L_Articulo.VerificoCantidadProducto(Dgv_Tap3_Articulo) && _L_Articulo.stringBuilder.Length > 0)
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje(_L_Articulo.stringBuilder.ToString());
                    _FrmMensajes.ShowDialog();
                    return; // Salir 
                }
                // Validacion de Altura y medidas verticales 
                if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "01" && !ValidoAlturaMedidas())
                {
                    return; // Salir
                }

                //Guardar datos en CAORDSERV
                string codServicio = Cbx_Pnl2_Servicio.SelectedValue?.ToString();
                var glbCodDetVta = Cbx_Pnl2_Trbajo.SelectedValue.ToString();
                string sucursal = TB_USUARIO.COD_SUCURSAL;
                string cedulaAfiliado = Dgv_Pnl3_ClienteAfiliado.CurrentRow?.Cells["Codigo_Emp"]?.Value?.ToString();
                string codigoEmpresaAfiliada = Dgv_Pnl3_ClienteAfiliado.CurrentRow?.Cells["Codigo_Emp"]?.Value?.ToString();
                DateTime FechaActiva = _D_Inicio.DiaActivo();
                bool monturaEstaEnQuorum = false;

                if ((Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "01"))//Convencional
                {
                    _servicioValidaciones.MonturaEstaEnQuorum(codMonturaSeleccionada, sucursal, codServicio);
                }
                //bool empresaAfiliada = EmpresaAfiliada;
                //bool esEmpresaAfiliada = empresaAfiliada == "1" || empresaAfiliada.ToLower() == "true";
                var glbManejaExisLC = _D_DetalleOrden.TB_PARAMETRO("LCManejaExist");


                Conexion cn = new Conexion();
                SqlConnection connection = cn.LeerCadena();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                // Iniciar la transacción
                transaction = connection.BeginTransaction();
                command.Connection = connection;
                command.Transaction = transaction;
                command.Parameters.Clear();
                command.CommandTimeout = 120;

                try
                {

                    var numeroOrden = await _GuardarOrdenServ.GuardarOrdenServicioDesdeFormularioAsync(
                        Dgv_Tap3_Articulo,
                        Dgv_Tap3_Totales,
                        codSucursal,
                        letraInicial,
                        numeroCedula,
                        TB_USUARIO.COD_EMPLEADO,
                        Cbx_Pnl2_Laboratorio.Visible== false? "000": Cbx_Pnl2_Laboratorio.SelectedValue.ToString(),
                        Cbx_Pnl2_Servicio.Visible == false ? "000" : Cbx_Pnl2_Servicio.SelectedValue?.ToString(),
                        glbNumVision,
                        //codigoEmpresaAfiliada,
                        txtObservacion.Text,
                        tipoTrabajoVenta,
                        Cbx_Pnl2_Trbajo.SelectedValue.ToString(),
                        TB_USUARIO.COD_USR,
                        Montura_Propia,
                        _L_Articulo.ValidarAplica(Dgv_Tap3_Articulo, glbCodDetVta , EmpresaAfiliada == "" ? false : true, Promocion_Aplicada , Montura_Propia, Cristal_Propio),
                        Cristal_Propio,
                        monturaEstaEnQuorum,
                        Garantia,
                        Codigo_Coloracion,
                        cedulaAfiliado,
                        codigoEmpresaAfiliada,
                        Txt_Pnl2_Examen.Text,
                        Codmotivodes,
                        Fecha_Ofreci,
                        Hora_Ofrecido,
                        EmpresaAfiliada == "" ? false :true,
                        FechaActiva,
                        command
                    );

                    if(string.IsNullOrEmpty(numeroOrden))
                    {
                        //throw new Exception("No se generó el Número de Orden. El proceso no puede continuar");
                        transaction.Rollback();
                        return;
                    }

                    // --- Guardar Detalle ---
                    var guardoDetalle = await _GuardarOrdenServ.GuardarDetalleOrdenServicioAsync(
                        Dgv_Tap3_Articulo,
                        numeroOrden,                // número de orden recién creado
                        "0",                        
                        glbCodDetVta,               
                        codSucursal,
                        command,
                        Cbx_Pnl2_Trbajo,
                        Cbx_Pnl2_Laboratorio,
                        EsEmpresaAfiliada()               // empresa afiliada (depende si tienes lógica para ello)
                    );
                    if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "02")
                    {
                        guardoDetalle = await _GuardarOrdenServ.AgregaRelacionOsLC(codSucursal, numeroOrden,"0", Dgv_Tap3_Articulo,TB_USUARIO.COD_USR,"", command);
                    }

                    if (!guardoDetalle)
                    {
                        //throw new Exception("Error guardando el Detalle de la Orden. El proceso no puede continuar");
                        transaction.Rollback();
                        return;
                    }

                    // --- Actualizar Trabajo y Existencias ---
                    //                    var actualizadoTrabajo = await _GuardarOrdenServ.ActualizarTrabajoYExistencias(numeroOrden, txtHorizontal.Text.Replace(',', '.'), txtVertical.Text.Replace(',', '.')
                    //, txtMaxima.Text.Replace(',', '.'), txtPuente.Text.Replace(',', '.'), "0", "0", "0", "0", "0", "A", "Cerca", "Cerca", TB_USUARIO.COD_USR, codSucursal, command);
                    //LLenar_TbTrabajo();

                    AsignarParametrosFaltantes(nuevoTrabajo, numeroOrden, "0", txtHorizontal.Text.Replace(',', '.'), txtVertical.Text.Replace(',', '.'), txtMaxima.Text.Replace(',', '.'), txtPuente.Text.Replace(',', '.'), Cbx_Pnl2_Laboratorio.SelectedValue.ToString(), Cbx_Pnl2_Servicio.SelectedValue?.ToString(), Hora_Ofrecido, "T", Fecha_Ofreci, DateTime.Today, Cbx_Pnl2_Trbajo.SelectedValue.ToString(), Cbx_Pnl2_Trbajo.Text);
                    
                    //_L_Trabajo.AgregarTrabajo(nuevoTrabajo);

                    var actualizadoTrabajo = _GuardarOrdenServ.AgregarTrabajo2(nuevoTrabajo, command);
                    //// _L_Articulo.Inserta_TB_TRABAJO(numeroOrden, txtHorizontal.Text, txtVertical.Text, txtMaxima.Text,"0", "0", "0", "0", "0", "0", "A", "Cerca", "Cerca", TB_USUARIO.COD_USR, codSucursal);

                    //if (!actualizadoTrabajo)
                    //{
                    //    //throw new Exception("Error actualizando la tabla TB_TRABAJO. El proceso no puede continuar");
                    //    transaction.Rollback();
                    //    return;
                    //}

                    // Verifico y Apligo Garantia para trabajo convencional reservado 
                    //if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "09" && !actualizadoTrabajo)
                    if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "09"
                        )

                    {
                        DataSet ds = await _GuardarOrdenServ.LlamarActualizarGarantiaAsync(Os_Garantia_Trabajo, codSucursal, numeroOrden, command);
                    }

                    transaction.Commit();
                    // Finaliza normalmente

                    tipoTrabajoSeleccionado = false;

                    Btn_Tap3_Cancelar.PerformClick();

                    btnPrincipal.PerformClick();

                    Btn_Pnl3_Cancelar.PerformClick();

                    //FrmPrincipal frmPrincipal = (FrmPrincipal)this.MdiParent ?? this.ParentForm as FrmPrincipal;
                    //FrmListaOrdenes frmListaOrdenes = new FrmListaOrdenes();
                    //frmPrincipal.addformulario(frmListaOrdenes);



                    //this.Close();
                    //LimpiarGrid();
                    //Btn_Tap3_Procesar.Enabled = true;

                }
                catch (Exception ex)
                {
                    mostrarError("No se pudo guardar la orden de servicio: " + ex.Message);
                    transaction.Rollback();
                }

            }
            catch (Exception ex)
            {
                mostrarError("Error al procesar el Guardado de Orden: " + ex.Message);
                //Btn_Tap3_Procesar.Enabled = true;
            }

        }

        public void AsignarParametrosFaltantes(TB_TRABAJOCTE nuevoTrabajo, string tNumOrdserv, string tRevision, string tHorizontal, string tVertical, string tMaxima,
       string tPuente, string tLaboratorio,   string tServicio, string tHoraOfrecido, string tTipoRx, DateTime? tFechaOfrecido, DateTime? tFecCrea, string codDetVta, string Descripcion_Tipo_Venta)
        {
            nuevoTrabajo.TSucursal = codigoSucursal;
            nuevoTrabajo.TNumOrdserv = tNumOrdserv;
            nuevoTrabajo.CodDetVta = codDetVta;
            nuevoTrabajo.THORAOFRECIDO = tHoraOfrecido;
            nuevoTrabajo.TFECHAOFRECIDO = tFechaOfrecido;

            if (Descripcion_Tipo_Venta == "Venta Directa")
            {
                //nuevoTrabajo.CodDetVta = codDetVta;
                nuevoTrabajo.TipoExamen = "";
                nuevoTrabajo.TTIPOTRABAJO = "001";
                //nuevoTrabajo.TEXAMEN = "0";
                nuevoTrabajo.THORIZONTAL = "0";
                nuevoTrabajo.TVERTICAL = "0";
                nuevoTrabajo.TMAXIMA = "0";
                nuevoTrabajo.TPUENTE = "0";

                nuevoTrabajo.TOJO = "";
                nuevoTrabajo.TTIPOVISIOND = "";
                nuevoTrabajo.TTIPOVISIONI = "";
                nuevoTrabajo.TLABORATORIO = "";
                nuevoTrabajo.TSERVICIO = "";
            }
            else if (Descripcion_Tipo_Venta == "Reparacion")
            {
                nuevoTrabajo.THORIZONTAL = "0";
                nuevoTrabajo.TVERTICAL = "0";
                nuevoTrabajo.TMAXIMA = "0";
                nuevoTrabajo.TPUENTE = "0";

                nuevoTrabajo.TOJO = "";
                nuevoTrabajo.TTIPOVISIOND = "";
                nuevoTrabajo.TTIPOVISIONI = "";
                nuevoTrabajo.TLABORATORIO = tLaboratorio;
                nuevoTrabajo.TSERVICIO = tServicio;
                nuevoTrabajo.TipoExamen = "";



            }
            else
            {

                nuevoTrabajo.THORIZONTAL = tHorizontal;
                nuevoTrabajo.TVERTICAL = tVertical;
                nuevoTrabajo.TMAXIMA = tMaxima;
                nuevoTrabajo.TPUENTE = tPuente;
                nuevoTrabajo.TLABORATORIO = tLaboratorio;
                nuevoTrabajo.TSERVICIO = tServicio;
                nuevoTrabajo.THORAOFRECIDO = tHoraOfrecido;
                nuevoTrabajo.TFECHAOFRECIDO = tFechaOfrecido;
                nuevoTrabajo.CodDetVta = codDetVta;
                nuevoTrabajo.TTIPOTRABAJO = (Descripcion_Tipo_Venta.Trim() == "Reparacion" || Descripcion_Tipo_Venta.Trim() == "Reparacion Empleado") ? "003" : "002";

                if (string.IsNullOrWhiteSpace(nuevoTrabajo.TRevision))
                    nuevoTrabajo.TRevision = tRevision;



                if (!nuevoTrabajo.TFECCREA.HasValue)
                    nuevoTrabajo.TFECCREA = tFecCrea;
            }

            if (string.IsNullOrWhiteSpace(nuevoTrabajo.TTIPORX))
                nuevoTrabajo.TTIPORX = tTipoRx;

        }

        private void LLenar_TbTrabajo()
        {
            //TB_TRABAJOCTE nuevoTrabajo = new TB_TRABAJOCTE();

            nuevoTrabajo.TNumOrdserv = null;
            nuevoTrabajo.TCEDIDEN = Txt_Tap1_Cedula.Text.Trim();
            nuevoTrabajo.TNACIO = Cbx_Tap1_Nacionalidad.Text.Trim(); // Ajusta según cómo manejas la nacionalidad
            nuevoTrabajo.TALTD = Convert.ToDecimal(txtAltD.Text);
            nuevoTrabajo.TALTI = Convert.ToDecimal(txtAltI.Text);
            nuevoTrabajo.TTIPOVISIOND = cbVisionDerecha.Text;//Dgv_Pnl2_conv.Rows[0].Cells["VISION"]?.Value?.ToString() ?? " ";
            nuevoTrabajo.TTIPOVISIONI = cbVisionIzquierda.Text;
            nuevoTrabajo.TSucursal = codigoSucursal;
            nuevoTrabajo.TTIPOTRABAJO = "002";
            nuevoTrabajo.USERCREA = TB_USUARIO.COD_USR;
            nuevoTrabajo.TEXAMEN = Txt_Tap2_Examen.Text;
            nuevoTrabajo.TALTD = Convert.ToDecimal(txtAltD.Text);
            nuevoTrabajo.TALTI = Convert.ToDecimal(txtAltI.Text);


            //if (Dgv_Pnl2_medconv.Rows.Count > 0)
            //{

            //nuevoTrabajo.TDISTANCIAVERTICE = Dgv_Pnl2_medconv.Rows[0].Cells[0]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_medconv.Rows[0].Cells[0].Value) : 0;
            //nuevoTrabajo.TANGULOPANTOSCOPICO = Dgv_Pnl2_medconv.Rows[0].Cells[1]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_medconv.Rows[0].Cells[1].Value) : 0;
            //nuevoTrabajo.TANGULOFACIAL = Dgv_Pnl2_medconv.Rows[0].Cells[2]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_medconv.Rows[0].Cells[2].Value) : 0;
            //nuevoTrabajo.TDISTANCIADELECTURA = Dgv_Pnl2_medconv.Rows[0].Cells[3]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_medconv.Rows[0].Cells[3].Value) : 0;

            nuevoTrabajo.TDISTANCIAVERTICE = Convert.ToDecimal(txtDistVertice.Text);
            nuevoTrabajo.TANGULOPANTOSCOPICO = Convert.ToDecimal(txtAngPant.Text);
            nuevoTrabajo.TANGULOFACIAL = Convert.ToDecimal(txtAngFac.Text);
            nuevoTrabajo.TDISTANCIADELECTURA = Convert.ToDecimal(txtDll.Text);

            //}
            nuevoTrabajo.TOJO = Cbx_Tap2_Ojo.Text;
            if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "01" || Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "02" || Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "09")
            {
                nuevoTrabajo.TipoExamen = Cbx_Tap2_Tipo_Examen.Text;
            }
            else
            {
                nuevoTrabajo.TipoExamen = "";
            }
            Txt_Pnl2_Cedula.Text = Cbx_Tap1_Nacionalidad.Text.Trim() + "-" +Txt_Tap1_Cedula.Text.Trim();

            bool encontrado = false;
            for (int i = 0; i < Cbx_Tap2_Ojo.Items.Count; i++)
            {
                var item = Cbx_Tap2_Ojo.Items[i];
                // Si es un objeto anónimo o tiene la propiedad Text
                var textProp = item.GetType().GetProperty("Text");
                if (textProp != null)
                {
                    string texto = textProp.GetValue(item, null)?.ToString();
                    if (texto.Trim().ToLower() == Cbx_Tap2_Ojo.Text.Trim().ToLower())
                    {
                        Cbx_Tap2_Ojo.SelectedIndex = i;
                        encontrado = true;
                        break;
                    }
                }
            }
            if (!encontrado && Cbx_Tap2_Ojo.Items.Count > 0)
            {
                Cbx_Tap2_Ojo.SelectedIndex = 0;
            }

        }

        private bool EsEmpresaAfiliada()
        {
            return !string.IsNullOrEmpty(EmpresaAfiliada);
        }

        private DialogResult mostrarPregunta(string mensaje, string titulo)
        {
            return FrmMensajes.MostrarPregunta(mensaje, titulo); 
        }

        private void mostrarError(string mensaje)
        {
            FrmMensajes.MostrarError(mensaje); 
        }

        private List<string> ObtenerCodigosDesdeGrid()
        {
            var codigos = new List<string>();

            foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
            {
                if (row.Cells["CodArticulo"].Value != null)
                {
                    codigos.Add(row.Cells["CodArticulo"].Value.ToString());
                }
            }

            return codigos;
        }



        private void FormatoDataGridRangosCristales()
        {

            try
            {
                // Establecer estilo para encabezados de columna
                dgvRangoCrt.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Establecer estilo para todas las celdas
                DataGridViewCellStyle centerStyle = new DataGridViewCellStyle();
                centerStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                centerStyle.Font = new Font("Century Gothic", 6.25F);

                dgvRangoCrt.DefaultCellStyle = centerStyle;
                dgvRangoCrt.RowTemplate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //Ancho de columna
                dgvRangoCrt.Columns["Cristal"].Width = 75;
                dgvRangoCrt.Columns["EsfCil"].Width = 65;
                dgvRangoCrt.Columns["Lejos"].Width = 50;
                dgvRangoCrt.Columns["Cerca"].Width = 60;
                dgvRangoCrt.Columns["Bifocal"].Width = 55;
                dgvRangoCrt.Columns["Progresivo"].Width = 100;
                dgvRangoCrt.Columns["Balance"].Width = 70;
                dgvRangoCrt.Columns["DPLejos"].Width = 75;
                dgvRangoCrt.Columns["DPCerca"].Width = 75;
                dgvRangoCrt.Columns["AddMin"].Width = 70;
                dgvRangoCrt.Columns["AddMax"].Width = 70;
                dgvRangoCrt.Columns["AltMin"].Width = 70;
                dgvRangoCrt.Columns["AltMax"].Width = 70;
                dgvRangoCrt.Columns["PrismaMin"].Width = 100;
                dgvRangoCrt.Columns["PrismaMax"].Width = 100;
                dgvRangoCrt.Columns["DiamMax"].Width = 80;
                dgvRangoCrt.Columns["DVCMin"].Width = 80;
                dgvRangoCrt.Columns["DVCMax"].Width = 80;
                dgvRangoCrt.Columns["AFMin"].Width = 60;
                dgvRangoCrt.Columns["AFMax"].Width = 60;
                dgvRangoCrt.Columns["APMin"].Width = 60;
                dgvRangoCrt.Columns["APMax"].Width = 60;
                dgvRangoCrt.Columns["ColorSi"].Width = 70;
                dgvRangoCrt.Columns["ColorNo"].Width = 70;
                dgvRangoCrt.Columns["Express1Hr"].Width = 100;
                dgvRangoCrt.Columns["Express3Hr"].Width = 100;
                dgvRangoCrt.Columns["Express12Hr"].Width = 100;
                dgvRangoCrt.Columns["5Dias"].Width = 60;
                dgvRangoCrt.Columns["7DiasHab"].Width = 90;
                dgvRangoCrt.Columns["15Dias"].Width = 60;
                dgvRangoCrt.Columns["30Dias"].Width = 60;
                dgvRangoCrt.Columns["21Dias"].Width = 60;
                dgvRangoCrt.Columns["45Dias"].Width = 60;
                dgvRangoCrt.Columns["60Dias"].Width = 60;
                dgvRangoCrt.Columns["90Dias"].Width = 60;
                dgvRangoCrt.Columns["MontRemoto"].Width = 120;
                dgvRangoCrt.Columns["MontQuorum"].Width = 120;
                dgvRangoCrt.Columns["ColoracionF12"].Width = 140;
                dgvRangoCrt.Columns["DDLMin"].Width = 100;
                dgvRangoCrt.Columns["DDLMax"].Width = 100;
                dgvRangoCrt.Columns["Mimesys"].Width = 100;


                dgvRangoCrt.Columns["ProgVisionLejosDistMin"].Visible = false;
                dgvRangoCrt.Columns["ProgVisionLejosDistMax"].Visible = false;
                dgvRangoCrt.Columns["ProgVisionCercaDistMin"].Visible = false;
                dgvRangoCrt.Columns["ProgVisionCercaDistMax"].Visible = false;
                dgvRangoCrt.Columns["ProgVisionMediaDistMin"].Visible = false;
                dgvRangoCrt.Columns["ProgVisionMediaDistMax"].Visible = false;
                dgvRangoCrt.Columns["2daRefraccion"].Visible = false;

                

                // nuevo 21-08-2023 
                //dgvRangoCrt.Columns["Abo_Monto"].DefaultCellStyle.Format = "##,##0.00";

            }


            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }
        }

        private void FormatoTablaRango()
        {
            try
            {
                // Establecer estilo para encabezados de columna
                dgvRangoCrt.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Establecer estilo para todas las celdas
                DataGridViewCellStyle centerStyle = new DataGridViewCellStyle();
                centerStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                centerStyle.Font = new Font("Century Gothic", 6.25F);

                dgvRangoCrt.DefaultCellStyle = centerStyle;
                dgvRangoCrt.RowTemplate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                ////// Configurar mayúsculas para la columna codCristalOptica
                ////dgvRangoCrt.Columns["codCristalOptica"].DefaultCellStyle.Font = new Font(dgvRangoCrt.Font, FontStyle.Regular);

                // Ancho de las columnas
                dgvRangoCrt.Columns["codCristalOptica"].Width = 65;
                dgvRangoCrt.Columns["esferaMin"].Width = 50;
                dgvRangoCrt.Columns["esferaMax"].Width = 60;
                dgvRangoCrt.Columns["cilindroMin"].Width = 55;
                dgvRangoCrt.Columns["cilindroMax"].Width = 60;
                dgvRangoCrt.Columns["sumatoriaMin"].Width = 60;
                dgvRangoCrt.Columns["diametroMax"].Width = 60;
                dgvRangoCrt.Columns["adicionMin"].Width = 55;
                dgvRangoCrt.Columns["adicionMax"].Width = 60;
                dgvRangoCrt.Columns["alturaMin"].Width = 55;
                dgvRangoCrt.Columns["alturaMax"].Width = 60;

                // Alineación del texto
                dgvRangoCrt.Columns["codCristalOptica"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvRangoCrt.Columns["esferaMin"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvRangoCrt.Columns["esferaMax"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvRangoCrt.Columns["cilindroMin"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvRangoCrt.Columns["cilindroMax"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvRangoCrt.Columns["sumatoriaMin"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvRangoCrt.Columns["diametroMax"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvRangoCrt.Columns["adicionMin"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvRangoCrt.Columns["adicionMax"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvRangoCrt.Columns["alturaMin"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvRangoCrt.Columns["alturaMax"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Hacer columnas no editables
                dgvRangoCrt.Columns["codCristalOptica"].ReadOnly = true;
                dgvRangoCrt.Columns["esferaMin"].ReadOnly = true;
                dgvRangoCrt.Columns["esferaMax"].ReadOnly = true;
                dgvRangoCrt.Columns["cilindroMin"].ReadOnly = true;
                dgvRangoCrt.Columns["cilindroMax"].ReadOnly = true;
                dgvRangoCrt.Columns["sumatoriaMin"].ReadOnly = true;
                dgvRangoCrt.Columns["diametroMax"].ReadOnly = true;
                dgvRangoCrt.Columns["adicionMin"].ReadOnly = true;
                dgvRangoCrt.Columns["adicionMax"].ReadOnly = true;
                dgvRangoCrt.Columns["alturaMin"].ReadOnly = true;
                dgvRangoCrt.Columns["alturaMax"].ReadOnly = true;

                // Configurar los títulos de las columnas
                dgvRangoCrt.Columns["codCristalOptica"].HeaderText = "Cristal";
                dgvRangoCrt.Columns["esferaMin"].HeaderText = "Esf.Min";
                dgvRangoCrt.Columns["esferaMax"].HeaderText = "Esf.Max";
                dgvRangoCrt.Columns["cilindroMin"].HeaderText = "Cil.Min";
                dgvRangoCrt.Columns["cilindroMax"].HeaderText = "Cil.Max";
                dgvRangoCrt.Columns["sumatoriaMin"].HeaderText = "Sum.Min";
                dgvRangoCrt.Columns["diametroMax"].HeaderText = "Diam.Max";
                dgvRangoCrt.Columns["adicionMin"].HeaderText = "Add.Min";
                dgvRangoCrt.Columns["adicionMax"].HeaderText = "Add.Max";
                dgvRangoCrt.Columns["alturaMin"].HeaderText = "Alt.Min";
                dgvRangoCrt.Columns["alturaMax"].HeaderText = "Alt.Max";

                // Ocultar columnas
                dgvRangoCrt.Columns["linea"].Visible = false;
                dgvRangoCrt.Columns["activo"].Visible = false;
                dgvRangoCrt.Columns["impresora"].Visible = false;
                dgvRangoCrt.Columns["EVDCODE"].Visible = false;
                dgvRangoCrt.Columns["codCalculo"].Visible = false;

                // Configurar estilo visual
                dgvRangoCrt.BackgroundColor = Color.Honeydew;
                //dgvRangoCrt.DefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
                dgvRangoCrt.RowHeadersVisible = false; // Equivalente a GroupByBoxVisible = false
                dgvRangoCrt.DefaultCellStyle.SelectionBackColor = Color.Yellow;

                // Enfocar la columna 7 (ajustar índice según necesidad)
                if (dgvRangoCrt.Columns.Count > 7)
                {
                    dgvRangoCrt.CurrentCell = dgvRangoCrt.Rows[0].Cells[7];
                }
            }
            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;

            string codSucursal;
            codSucursal = _D_DetalleOrden.TB_PARAMETRO("SucursalId");
            //_D_Articulo.Agregar_TB_TRABAJO(codSucursal, "", "", Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), "002", Convert.ToInt32(Txt_Pnl2_Examen.Text)
            //  , txtHorizontal.Text, txtVertical.Text, txtMaxima.Text, txtPuente.Text, "0", "0", "A", "Cerca", "Cerca", "QUO", "001", "T", TB_USUARIO.COD_USR, "02", "CONVENCIONAL", "0", "0", "0", "0");

        }

        private void btnCancelarRangosCrt_Click(object sender, EventArgs e)
        {
            pnlRangoCrt.Visible = false;
        }

        private void Btn_Tap3_MonturaPropia_Click(object sender, EventArgs e)
        {

            VisualizarPanel("MonturaPropia");
            HabilitacionControl("Habilitar_MonturaPropia");

        }

        private void btnAceptarMonturaPropia_Click(object sender, EventArgs e)
        {
            if (rbCompleta.Checked == false & rbRanurada.Checked == false & rbAlaire.Checked == false)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Por favor, seleccione un tipo de montura");
                _FrmMensajes.ShowDialog();
                return;

            }
            else
            {
                if (rbCompleta.Checked)
                {
                    TipoMonturaPropia = "Completa";
                }
                else if (rbRanurada.Checked)
                {
                    TipoMonturaPropia = "Ranurada";
                }
                else if (rbAlaire.Checked)
                {
                    TipoMonturaPropia = "Alaire";
                }
                Montura_Propia = true;
                txtHorizontal.Enabled = true;
                txtVertical.Enabled = true;
                txtMaxima.Enabled = true;
                txtPuente.Enabled = true;
                CerrarPanelMonturaPropia();

                if (Montura_Propia == true && Cbx_Pnl2_Trbajo.SelectedValue.ToString() != "09")
                {
                    _L_Articulo.CargarServicioMonturaPropia(Dgv_Tap3_Articulo, TipoMonturaPropia == "Completa");

                }

                BotonesColor(false, "Montura Propia");
            }

        }

        private void btnCancelarMonturaPropia_Click(object sender, EventArgs e)
        {
            CerrarPanelMonturaPropia();
        }

        private void CerrarPanelMonturaPropia()
        {
            VisualizarPanel("MostrarCabezeraSecundaria");
            HabilitacionControl("CabezeraPrincipal");
            LimpiarControles("MonturaPropia");
        }

        private void Btn_Tap1_Guardar_Click(object sender, EventArgs e)
        {

            //validarvacio();

            mantenervacio = false;
            //TopeExamen = 0;
            if (validarvacio())
            {

                guardacliente();

                Btn_Tap2_Derecha_Click(this.Btn_Tap2_Derecha, EventArgs.Empty);
                btnExamen.Focus();
                tabControl.SelectedIndex = 1;
                Txt_Tap2_Examen.Text = TopeExamen.ToString(); // Opcional: Restablecer el valor al máximo
                                                              //Btn_Tap2_Derecha_Click(this.Btn_Tap2_Derecha, EventArgs.Empty);
                Cbx_Tap2_Tipo_Examen_SelectedIndexChanged(Cbx_Tap2_Tipo_Examen, EventArgs.Empty);
                VisualizarPanel("MostrarCabeceraExamen");
            }

            Dtp_Tap2_Examen_ValueChanged(Dtp_Tap2_FecExam, EventArgs.Empty);
            //Dgv_Pnl2_medconv.Enabled = true;
            //llenarCabeceraExamenyOrden();
        }

        private void Btn_Tap3_CristalPropio_Click(object sender, EventArgs e)
        {
            Cristal_Propio = true;
            HabilitacionControl("CabezeraPrincipal");
            // Modo oscuro
            BotonesColor(false, "Cristal Propio");

        }

        private void QuitarLimea2_Click(object sender, EventArgs e)
        {}

        private void FrmCargarOrden_Load(object sender, EventArgs e)
        {
            BloquearCamposE();
              //tabControl.SelectedIndex = 0;
              //_L_Articulo.CargarClientesAfiliados(Dgv_Pnl3_ClienteAfiliado, listaClienteAfiliados);

              //Dgv_Pnl3_ClienteAfiliado.DataSource = listaClienteAfiliados;
              //Formato_Dgv_Pnl3_ClienteAfiliado();

              ////Guarda en tb_trabajo temporal
              //_D_Articulo.Agregar_TB_TRABAJO(_D_DetalleOrden.TB_PARAMETRO("SucursalId"), "", "", Txt_Pnl_2_Cedula.Text.Substring(0, 1), Txt_Pnl_2_Cedula.Text.Substring(2, Txt_Pnl_2_Cedula.Text.Length - 2), "002", Convert.ToInt32(Txt_Pnl2_Examen.Text)
              //  , txtHorizontal.Text, txtVertical.Text, txtMaxima.Text, txtPuente.Text, "0", "0", "A", "Cerca", "Cerca", "QUO", "001", "T", TB_USUARIO.COD_USR, "02", "CONVENCIONAL", "0", "0", "0", "0");

              /*MEIFER*/
              // Optional: Set the background color for the content area of each tab page
              // (This is separate from the tab headers handled by DrawItem)
              codigoSucursal = _D_DetalleOrden.TB_PARAMETRO("SucursalId");

            foreach (TabPage page in tabControl.TabPages)
            {
                page.BackColor = Color.White;
            }

            //panel1.Location = new Point(10, 126);

            //BloquearCampos();
            LimpiarCampos2();


            ConfigurarDgv_Pnl2_conv();
            ConfigurarDgv_Pnl2_cont();
            //ConfigurarDgv_Pnl2_medconv();
            ConfigurarDgv_Pnl2_Quera();

            DataTable dtMotivosGarantia = _L_Cliente.ObtenerMotivosReposicion(); // Usa la instancia _L_Cliente

            cbMotivosGarantia.DataSource = dtMotivosGarantia;

            dgvOrdenesGarantia.DataSource = _L_Cliente.ObtenerClienteConGarantia(codigoSucursal, Txt_Tap1_Cedula.Text, Cbx_Tap1_Nacionalidad.Text); // Usa la instancia _L_Cliente

            _L_Articulo.BucarTipoVenta(Cbx_Pnl2_Trbajo);
            CargarComboLaboratorios();
            CargarComboServicioLaboratorios();

            cbVisionDerecha.Text = "Cerca";
            cbVisionIzquierda.Text = "Cerca";

        }




        private void Btn_Tap3_ClienteAfiliado_Click(object sender, EventArgs e)
        {
            //Pido Clave Autorizada
            _FrmClaveAutorizada.Nuevo_Parametro = true;
            _FrmClaveAutorizada.Parametro_Nuevo = _L_Articulo.BuscarCodigoGerenteDescuento("009");
            _FrmClaveAutorizada.ShowDialog();

            if (_FrmClaveAutorizada.DialogResult == DialogResult.OK && _FrmClaveAutorizada.ClaveCorrecta == true)
            {

                VisualizarPanel("ClienteAfiliado");
                HabilitacionControl("Habilitar_ClienteAfiliado");

            }
          
           
        }

        private void btnCancelarAfiliado_Click(object sender, EventArgs e)
        {
            //ActivoInactivoBtn("ClienteAfiliado");
            VisualizarPanel("MostrarCabezeraSecundaria");
            HabilitacionControl("CabezeraPrincipal");
            LimpiarControles("ClienteAfiliado");
        }


        private void Btn_Tap3_Promocion_Click(object sender, EventArgs e)
        {
            if (_L_Articulo.ObtenerPromoVigente(Dgv_Pnl3_Promociones))
            {
                VisualizarPanel("Promociones");
                HabilitacionControl("Habilitar_Promociones");
            }
            else
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(_L_Articulo.stringBuilder.ToString());
                _FrmMensajes.ShowDialog();
            }
        }

        private void CerrarPanelPromocion()
        {
            VisualizarPanel("MostrarCabezeraSecundaria");
            HabilitacionControl("CabezeraPrincipal");
            LimpiarControles("Promociones");
        }

        private void Btn_Tap3_Cancelar_Promo_Click(object sender, EventArgs e)
        {
            CerrarPanelPromocion();
        }

        private void Btn_Tap3_Aceptar_Promo_Click(object sender, EventArgs e)
        {
            if (Dgv_Pnl3_Promociones.SelectedRows.Count > 0)
            {
                // Acceder a la primera fila seleccionada
                DataGridViewRow filaSeleccionada = Dgv_Pnl3_Promociones.SelectedRows[0];

                // Obtener el valor de la celda "Cod_Coloracion"
                Codigo_Promocion = filaSeleccionada.Cells["COD_Prom"].Value.ToString();
                Lbl_Tap3_Articulo1.Text= "Ingresar Articulo "+ " Promo: "+ filaSeleccionada.Cells["Prom_DESCRIP"].Value.ToString();
                BotonesColor(false, "Promocion");
                CerrarPanelPromocion();
            }
            else
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Debe seleccionar una promocion para continuar");
                _FrmMensajes.ShowDialog();
            }

        }

       

        private void Dgv_Pnl3_ClienteAfiliado_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar que la fila seleccionada no sea una fila nueva

            if (e.RowIndex >= 0 && !Dgv_Pnl3_ClienteAfiliado.Rows[e.RowIndex].IsNewRow)
            {
                // Obtener el artículo seleccionado
                var clienteafiliado = Dgv_Pnl3_ClienteAfiliado.Rows[e.RowIndex].DataBoundItem as TB_EMPAFI;

                if (clienteafiliado != null)
                {
                    // Cargar los valores
                      EmpresaAfiliada = clienteafiliado.Nombre;
                    if (!string.IsNullOrEmpty(clienteafiliado.PorcentajeDes1) && decimal.TryParse(clienteafiliado.PorcentajeDes1, out decimal porcentaje))
                    {
                        PorcDctoEmpresaAfiliada = porcentaje;
                       
                    }
                    else
                    {
                        PorcDctoEmpresaAfiliada = 0; // Valor predeterminado si la conversión falla
                    }

                    VisualizarPanel("MostrarCabezeraSecundaria");
                    HabilitacionControl("CabezeraPrincipal");

                    // Establecer el foco en el TextBox de cantidad
                    //Txt_Tap3_Articulo_Cantidad.Focus();

                    // Modo oscuro
                    BotonesColor(false, "Cliente Afiliado");
                }
            }
        }

        private void Btn_Tap3_Garantia_Click(object sender, EventArgs e)
        {
            Garantia = true;
            HabilitacionControl("CabezeraPrincipal");
            // Modo oscuro
            BotonesColor(false, "Garantia");
            Btn_Tap3_Garantia.Enabled = false;
        }

        private bool AplicoGarantia(string CI, string nacio, string OS, string Suc, string exam)
        {
            try
            {
               
                DataSet dsGetLC = _D_Articulo.ObtenerInfoReposicion(CI, nacio, OS, Suc, exam);

                if (dsGetLC.Tables[2].Rows.Count > 0)
                {
                    string codigo = Txt_Tap3_Articulo_Codigo.Text.ToUpper();
                    decimal precio = Convert.ToDecimal(Txt_Tap3_Articulo_Precio.Text);

                    // Si el código empieza con "C"
                    if (codigo.StartsWith("C"))
                    {
                        precio -= Convert.ToDecimal(dsGetLC.Tables[2].Rows[0]["DESCUENTOCRT"]);
                    }
                    // Si el código empieza con "S"
                    else if (codigo.StartsWith("S"))
                    {
                        precio -= Convert.ToDecimal(dsGetLC.Tables[2].Rows[0]["DESCUENTOSERVAR"]);
                    }

                    Txt_Tap3_Articulo_Precio.Text = precio.ToString("F2");

                    // Ajustar precio si es necesario según el país
                    if ( precio <= 0)
                    {
                        precio = Convert.ToDecimal("0.01");
                        Txt_Tap3_Articulo_Precio.Text = precio.ToString("F2");
                    }
                   

                    return true;
                }
                else
                {
                    MessageBox.Show("Esta OS no aplica reposición", "Verifique e intente de nuevo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void btnCancelarColorLC_Click(object sender, EventArgs e)
        {
           if (CodColorLC == "")
           {
                Pnl_3_Lista_ColoresLC.Visible = false;
                // Cargar los valores en los TextBox
       
                Txt_Tap3_Articulo_Cantidad.Enabled = false;
                // Txt_Tap3_Articulo_Cantidad.Text = string.Empty; // Limpiar el campo de cantidad

                VisualizarPanel("MostrarCabezeraSecundaria");
                HabilitacionControl("CabezeraPrincipal");

                ReiniciarBusquedaarticulo();

                // Establecer el foco en el TextBox de cantidad
                //Txt_Tap3_Articulo_Cantidad.Focus();
                //Txt_Tap3_Articulo_Cantidad.Text = "";
                //Txt_Tap3_Articulo_Cantidad.Enabled = false;
                //Pnl_3_Lista_ColoresLC.Visible = false;
            }
        }

        //private void Dgv_Pnl3_ColoresLC_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
           
           
        //}

        private void Dgv_Pnl3_ColoresLC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Dgv_Pnl3_ColoresLC.CurrentCell = Dgv_Pnl3_ColoresLC.Rows[e.RowIndex].Cells[0];
            if (e.RowIndex >= 0 && !Dgv_Pnl3_ColoresLC.Rows[e.RowIndex].IsNewRow)
            {
                // Obtener el color seleccionado
                //var CodColorLC = "";

                //CodColorLC = Dgv_Pnl3_ColoresLC.Rows[e.RowIndex].DataBoundItem.ToString();
                CodColorLC = Dgv_Pnl3_ColoresLC.CurrentRow.Cells[0].Value.ToString(); // Primera columna

                if (CodColorLC == null)
                {


                    Pnl_3_Lista_ColoresLC.Visible = false;
                    // Cargar los valores en los TextBox
                    ReiniciarBusquedaarticulo();
                    Txt_Tap3_Articulo_Cantidad.Enabled = false;
                   // Txt_Tap3_Articulo_Cantidad.Text = string.Empty; // Limpiar el campo de cantidad

                    VisualizarPanel("MostrarCabezeraSecundaria");
                    HabilitacionControl("CabezeraPrincipal");

                    // Establecer el foco en el TextBox de cantidad
                    Txt_Tap3_Articulo_Cantidad.Focus();


                }
                else
                {
                    Pnl_3_Lista_ColoresLC.Visible = false;
                    VisualizarPanel("MostrarCabezeraSecundaria");
                    HabilitacionControl("CabezeraPrincipal");

                    //Establecer el foco en el TextBox de cantidad
                    Txt_Tap3_Articulo_Cantidad.Focus();
                }

                
            }

        }

        private void Formato_Dgv_Pnl3_ColoresLC()
        {
            try
            {

                //Centrar todas las colucnas 
                Dgv_Pnl3_ColoresLC.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Dgv_Pnl3_ColoresLC.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;


                // Quitar la flecha del selector de fila
                Dgv_Pnl3_ColoresLC.RowHeadersVisible = false;

                // Deshabilitar el redimensionamiento de filas
                Dgv_Pnl3_ColoresLC.AllowUserToResizeRows = false;

                //asignar Nombres a cada colucna 
                Dgv_Pnl3_ColoresLC.Columns["CodColor"].HeaderText = "Código";
                Dgv_Pnl3_ColoresLC.Columns["DESCRIPCIONCOLOR"].HeaderText = "Descripción";

                //Ancho de columna
                Dgv_Pnl3_ColoresLC.Columns["CodColor"].Width = 80;
                Dgv_Pnl3_ColoresLC.Columns["DESCRIPCIONCOLOR"].Width = 305;


                // No modificable
                Dgv_Pnl3_ColoresLC.Columns["CodColor"].ReadOnly = true;
                Dgv_Pnl3_ColoresLC.Columns["DESCRIPCIONCOLOR"].ReadOnly = true;


                //quitar seleccion por defecto de datagrid
                Dgv_Pnl3_ColoresLC.ClearSelection();

                //AutoGenerar Columnas:
                Dgv_Pnl3_ColoresLC.AutoGenerateColumns = false;


            }


            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }

        }

        private void Formato_Dgv_Pnl3_ClienteAfiliado()
        {
            try
            {

                //Centrar todas las colucnas 
                Dgv_Pnl3_ClienteAfiliado.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Dgv_Pnl3_ClienteAfiliado.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;


                // Quitar la flecha del selector de fila
                Dgv_Pnl3_ClienteAfiliado.RowHeadersVisible = false;

                // Deshabilitar el redimensionamiento de filas
                Dgv_Pnl3_ClienteAfiliado.AllowUserToResizeRows = false;

                //asignar Nombres a cada colucna 
                Dgv_Pnl3_ClienteAfiliado.Columns["Codigo_Emp"].HeaderText = "Código";
                Dgv_Pnl3_ClienteAfiliado.Columns["Nombre"].HeaderText = "Descripción";

                //Ancho de columna
                Dgv_Pnl3_ClienteAfiliado.Columns["Codigo_Emp"].Width = 80;
                Dgv_Pnl3_ClienteAfiliado.Columns["Nombre"].Width = 305;


                // No modificable
                Dgv_Pnl3_ClienteAfiliado.Columns["Codigo_Emp"].ReadOnly = true;
                Dgv_Pnl3_ClienteAfiliado.Columns["Nombre"].ReadOnly = true;

                Dgv_Pnl3_ClienteAfiliado.Columns["PorcentajeDes1"].Visible = false;
                     Dgv_Pnl3_ClienteAfiliado.Columns["PorcentajeDes2"].Visible = false;
                Dgv_Pnl3_ClienteAfiliado.Columns["PorcentajeDes3"].Visible = false;
                //quitar seleccion por defecto de datagrid
                Dgv_Pnl3_ClienteAfiliado.ClearSelection();

                //AutoGenerar Columnas:
                Dgv_Pnl3_ClienteAfiliado.AutoGenerateColumns = false;


            }


            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }

        }

        private void ValidarTipoVenta(string TipoVenta)
        {
            if (TipoVenta == "Venta Directa")
            {
                Cbx_Pnl2_Laboratorio.Visible = false;
                this.Cbx_Tap2_Ojo.Visible = false;
                this.Txt_Pnl2_Examen.Visible = false;
                
                label24.Visible = false;
                Lbl_Pnl2_Fecha_Ofre.Visible = false;
                Txt_Pnl2_Fecha_Ofre.Visible = false;
                Lbl_Pnl2_Laboratorio.Visible = false;
                Lbl_Pnl2_Servicio.Visible = false;
                Cbx_Pnl2_Servicio.Visible = false;
                Txt_Pnl2_Examen.Visible = false;
               
                _FechaHoraOfrecida = _L_Articulo.ObtenerFechaHoraOfrecida("", Cbx_Pnl2_Trbajo.SelectedValue.ToString());
                if (_FechaHoraOfrecida != null && _FechaHoraOfrecida.Count > 0)
                {
                    FechaHoraOfrecida resultado = _FechaHoraOfrecida.First();
                    Txt_Pnl2_Fecha_Ofre.Text = $"{resultado.FechaOfrecida:dd/MM/yyyy}";
                }


                label34.Visible = false;
                label35.Visible = false;
                txtAltD.Visible = false;
                txtAltI.Visible = false;

                label24.Visible = false;
                Cbx_Tap2_Ojo.Visible = false;
                label26.Visible = false;
                cbVisionDerecha.Visible = false;
                label33.Visible = false;
                cbVisionIzquierda.Visible = false;

                




            }
            else if (TipoVenta == "Reparacion")
            {
                this.Cbx_Tap2_Ojo.Visible = false;
                this.Txt_Pnl2_Examen.Visible = false;
                
                label24.Visible = false;
                Lbl_Pnl2_Fecha_Ofre.Visible = false;
                Txt_Pnl2_Fecha_Ofre.Visible = false;

                label24.Visible = false;
                Cbx_Tap2_Ojo.Visible = false;
                label34.Visible = false;
                label35.Visible = false;
                txtAltD.Visible = false;
                txtAltI.Visible = false;

                label24.Visible = false;
                Cbx_Tap2_Ojo.Visible = false;
                label26.Visible = false;
                cbVisionDerecha.Visible = false;
                label33.Visible = false;
                cbVisionIzquierda.Visible = false;
            }
            else if (TipoVenta == "TC- Reposicion de Garantia" && string.IsNullOrEmpty(Os_Garantia_Trabajo) && string.IsNullOrEmpty(Numero_Examen_Garantia_Trabajo))
            {
                _L_Articulo.BucarTipoMotivoGarantia(Cbx_Pnl3_Garantia);
                if (_L_Articulo.BucarGarantiaCliente(Dgv_Pnl3_Garantia, Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2)))
                {
                    VisualizarPanel("Garantia");
                    HabilitacionControl("Habilitar_Garantia");
                }
                else
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje(_L_Articulo.stringBuilder.ToString());
                    _FrmMensajes.ShowDialog();
                }

            }
            else
            {
            //    this.Cbx_Tap2_Ojo.Visible = true;
            //    this.Txt_Pnl2_Examen.Visible = true;
            //    Lbl_Pnl2_Num_Examen.Visible = true;
            //    label24.Visible = true;
                Lbl_Pnl2_Fecha_Ofre.Visible = true;
                Txt_Pnl2_Fecha_Ofre.Visible = true;
                Cbx_Pnl2_Laboratorio.Visible = true;
                Lbl_Pnl2_Laboratorio.Visible = true;
                Lbl_Pnl2_Servicio.Visible = true;
                Cbx_Pnl2_Servicio.Visible = true;
                //Txt_Pnl2_Examen.Visible = true;
                //Lbl_Pnl2_Num_Examen.Visible = true;
            }
        }

        private void Cbx_Pnl2_Trbajo_SelectedIndexChanged(object sender, EventArgs e)
        {
            

            if (tipoTrabajoSeleccionado)
            {
                //if (ValidarTipoTrabajoTipoExamen(Cbx_Pnl2_Trbajo.SelectedValue.ToString(), Cbx_Tap2_Tipo_Examen.Text) == true)
                //{
                    Cbx_Pnl2_Trbajo.Enabled = false;
                    CargarComboLaboratorios();
                    //CargarComboServicioLaboratorios();
                    ValidarTipoVenta(Cbx_Pnl2_Trbajo.Text);
                //}
            }

            else
            {
                Cbx_Pnl2_Trbajo.Enabled = true;
            }
           

        }// Cbx_Pnl2_Trbajo.Enabled = false;

        private void Cbx_Pnl2_Trbajo_Click(object sender, EventArgs e)
        {
            tipoTrabajoSeleccionado = true;
        }

        private void Cbx_Pnl2_Laboratorio_Click(object sender, EventArgs e)
        {
            laboratorioSeleccionado = true;
        }

        private void Cbx_Pnl2_Servicio_Click(object sender, EventArgs e)
        {
            servicioSeleccionado = true;
        }

        private void Btn_Tap3_CambioPrecio_Click(object sender, EventArgs e)
        {
            if (Dgv_Tap3_Articulo.CurrentCell != null)
            {
                if (Dgv_Tap3_Articulo.CurrentRow != null)
                {
                    // Obtener el índice de la fila seleccionada
                    filaSeleccionada = Dgv_Tap3_Articulo.CurrentRow.Index;

                    //Verifico si Tiene articulos Asociados 
                    if (_L_Articulo.AccionCambiarPrecio_ArticuloPadre(Dgv_Tap3_Articulo, filaSeleccionada))
                    {
                        return;
                    }
                    else
                    {
                        _FrmClaveGerente.ShowDialog();
                        if (_FrmClaveGerente.ClaveCorrecta == true)
                        {

                            if (_L_Articulo.ConfigurarCambioPrecio(Txt_Pnl3_CambioPrecioActual, Dgv_Tap3_Articulo.Rows[filaSeleccionada].Cells["CodArticulo"].Value.ToString()))
                            {
                                VisualizarPanel("CambioPrecio");
                                HabilitacionControl("Habilitar_CambioPrecio");
                            }
                            else
                            {
                                _FrmMensajes.co = 2;
                                _FrmMensajes.avisomensaje("Error al Cargar el Panel de Cambio de Precio");
                                _FrmMensajes.ShowDialog();
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
        }

        private void Txt_Tap3_Articulo_Codigo_Click(object sender, EventArgs e)
        {
            // Deseleccionar cualquier celda seleccionada en el DataGridView
            if (Dgv_Tap3_Articulo.CurrentCell != null)
            {
                Dgv_Tap3_Articulo.ClearSelection();
                Dgv_Tap3_Articulo.CurrentCell = null; // Opcional: Elimina la celda activa
            }
        }

        private void Txt_Tap3_Articulo_Cantidad_Click(object sender, EventArgs e)
        {
            // Deseleccionar cualquier celda seleccionada en el DataGridView
            if (Dgv_Tap3_Articulo.CurrentCell != null)
            {
                Dgv_Tap3_Articulo.ClearSelection();
                Dgv_Tap3_Articulo.CurrentCell = null; // Opcional: Elimina la celda activa
            }
        }

        private void txtHorizontal_Click(object sender, EventArgs e)
        {
            // Deseleccionar cualquier celda seleccionada en el DataGridView
            if (Dgv_Tap3_Articulo.CurrentCell != null)
            {
                Dgv_Tap3_Articulo.ClearSelection();
                Dgv_Tap3_Articulo.CurrentCell = null; // Opcional: Elimina la celda activa
            }
        }

        private void txtVertical_Click(object sender, EventArgs e)
        {
            // Deseleccionar cualquier celda seleccionada en el DataGridView
            if (Dgv_Tap3_Articulo.CurrentCell != null)
            {
                Dgv_Tap3_Articulo.ClearSelection();
                Dgv_Tap3_Articulo.CurrentCell = null; // Opcional: Elimina la celda activa
            }
        }

        private void txtMaxima_Click(object sender, EventArgs e)
        {
            // Deseleccionar cualquier celda seleccionada en el DataGridView
            if (Dgv_Tap3_Articulo.CurrentCell != null)
            {
                Dgv_Tap3_Articulo.ClearSelection();
                Dgv_Tap3_Articulo.CurrentCell = null; // Opcional: Elimina la celda activa
            }
        }

        private void txtPuente_Click(object sender, EventArgs e)
        {
            // Deseleccionar cualquier celda seleccionada en el DataGridView
            if (Dgv_Tap3_Articulo.CurrentCell != null)
            {
                Dgv_Tap3_Articulo.ClearSelection();
                Dgv_Tap3_Articulo.CurrentCell = null; // Opcional: Elimina la celda activa
            }
        }

        private void Txt_Tap3_Articulo_Precio_TextChanged(object sender, EventArgs e)
        {
            //TextBox textBox = sender as TextBox;

            //if (decimal.TryParse(textBox.Text, out decimal valor))
            //{
            //    // Formatear el texto como un número con dos decimales
            //    textBox.Text = valor.ToString("N2");
            //}
        }

        

        private void txtObservacion_Click(object sender, EventArgs e)
        {
            // Deseleccionar cualquier celda seleccionada en el DataGridView
            if (Dgv_Tap3_Articulo.CurrentCell != null)
            {
                Dgv_Tap3_Articulo.ClearSelection();
                Dgv_Tap3_Articulo.CurrentCell = null; // Opcional: Elimina la celda activa
            }
        }

        private void Cbx_Pnl2_Servicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Validar si el usuario seleccionó algo
            //if (tabControl.SelectedIndex == 2)
            //{
            //    if (Cbx_Pnl2_Servicio.Items.Count > 0 && !string.IsNullOrEmpty(Cbx_Pnl2_Servicio.Text))
            //    {
            //        if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "01")
            //        {
            //            if (_L_Articulo.Disponible_Servicio_3Horas(Cbx_Pnl2_Servicio.Text, Cbx_Pnl2_Laboratorio.Text))
            //            {
            //                List<TB_SERVICIOSLABDTO> TB_SERVICIOSLABD = new List<TB_SERVICIOSLABDTO>();
            //                TB_SERVICIOSLABD = _D_Articulo.ServiciosLaboratorio(Cbx_Pnl2_Servicio.SelectedValue.ToString());
            //                if (TB_SERVICIOSLABD != null && TB_SERVICIOSLABD.Count > 0)
            //                {
            //                    TB_SERVICIOSLABDTO _SERVICIOSLABDTO = TB_SERVICIOSLABD.First();
            //                    Codigo_Servicio_Agregar = _SERVICIOSLABDTO.CodArticulo;

            //                    try
            //                    {
            //                        _FechaHoraOfrecida = _L_Articulo.ObtenerFechaHoraOfrecida(Cbx_Pnl2_Servicio.SelectedValue.ToString(), Cbx_Pnl2_Trbajo.SelectedValue.ToString());
            //                        if (_FechaHoraOfrecida != null && _FechaHoraOfrecida.Count > 0)
            //                        {
            //                            FechaHoraOfrecida resultado = _FechaHoraOfrecida.First();
            //                            Txt_Pnl2_Fecha_Ofre.Text = $"{resultado.FechaOfrecida:dd/MM/yyyy}";
            //                        }
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        _FrmMensajes.co = 2;
            //                        _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
            //                        _FrmMensajes.ShowDialog();
            //                    }
            //                }
            //                else
            //                {
            //                    Codigo_Servicio_Agregar = "";
            //                }
            //            }
            //            else
            //            {
            //                _FrmMensajes.co = 2;
            //                _FrmMensajes.avisomensaje("El servicio no está disponible en este horario");
            //                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
            //                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
            //                _FrmMensajes.ShowDialog();
            //                //Codigo_Servicio_Agregar = "";
            //            }
            //        }
            //    }
            //}

            if (servicioSeleccionado)
                Cbx_Pnl2_Laboratorio.Enabled = false;
            else
                Cbx_Pnl2_Laboratorio.Enabled = true;

        }

        private void Cbx_Pnl2_Laboratorio_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarComboServicioLaboratorios();
            if (laboratorioSeleccionado)
            {
                Cbx_Pnl2_Laboratorio.Enabled = false;
            }
            else
            {
                Cbx_Pnl2_Laboratorio.Enabled = true;
            }
        }

        private void Cbx_Tap2_Ojo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CargarComboServicioLaboratorios();
            if (Cbx_Tap2_Ojo.Text == "Derecho")
            {
                cbVisionIzquierda.Enabled = false;
                cbVisionDerecha.Enabled = true;
            }
           

            if (Cbx_Tap2_Ojo.Text == "Izquierdo")
            {
                cbVisionIzquierda.Enabled = true;
                cbVisionDerecha.Enabled = false;
            }

            if (Cbx_Tap2_Ojo.Text == "Ambos")
            {
                cbVisionIzquierda.Enabled = true;
                cbVisionDerecha.Enabled = true;
            }
        }

        private void ActualizaFechaOfrecida(string Cod_Vta, string Cod_Serv)
        {
            try
            {
               if ((Cod_Vta== "01" || Cod_Vta == "08" || Cod_Vta == "02") && Cod_Serv == "017")
               {
                    _FechaHoraOfrecida = _L_Articulo.ActualizarFechaOfre(Dgv_Tap3_Articulo, Montura_Propia.ToString(), Cbx_Pnl2_Laboratorio.Text== "QUORUM"? "1" :"0" , CodColorLC=="" ? "0" : "1");
                    if (_FechaHoraOfrecida != null && _FechaHoraOfrecida.Count > 0)
                    {
                        FechaHoraOfrecida resultado = _FechaHoraOfrecida.First();
                        Txt_Pnl2_Fecha_Ofre.Text = $"{resultado.FechaOfrecida:dd/MM/yyyy}";
                    }
                }

            }

            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }

        }


        private void txtHorizontal_Validated(object sender, EventArgs e)
        {
            if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "01" && !string.IsNullOrEmpty(txtHorizontal.Text))
            {
               
                bool Band = false;

                if (!decimal.TryParse(txtHorizontal.Text, out decimal valor))
                {
                     mensaje = "Valor inválido, debe escribir números";
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje(mensaje);
                    _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                    _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                    _FrmMensajes.ShowDialog(); 
                    // MessageBox.Show("Valor inválido, debe escribir números", "Inválido", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtHorizontal.Text = "";
                    return;
                }

                if (valor < 10 || valor > 90)
                {
                    mensaje = "Valor inválido, rango entre 10 y 90";
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje(mensaje);
                    _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                    _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                    _FrmMensajes.ShowDialog(); 
                    //MessageBox.Show($"Valor inválido, rango entre 10 y 90", "Rango", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Band = true;
                }
                else
                {
                    txtHorizontal.Text = valor.ToString("N2");
                }

                if (Band)
                {
                    txtHorizontal.Text = "";
                    txtHorizontal.Focus();
                }
            }
        }

        private void txtVertical_Validated(object sender, EventArgs e)
        {
            if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "01" && !string.IsNullOrEmpty(txtVertical.Text))
            {

                bool Band = false;

                if (!decimal.TryParse(txtVertical.Text, out decimal valor))
                {
                    mensaje = "Valor inválido, debe escribir números";
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje(mensaje);
                    _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                    _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                    _FrmMensajes.ShowDialog(); 
                    //MessageBox.Show("Valor inválido, debe escribir números", "Inválido", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtVertical.Text = "";
                    return;
                }

                if (valor < 10 || valor > 90)
                {
                    mensaje = "Valor inválido, rango entre 10 y 90";
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje(mensaje);
                    _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                    _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                    _FrmMensajes.ShowDialog();
                    //MessageBox.Show($"Valor inválido, rango entre 10 y 90", "Rango", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Band = true;
                }
                else
                {
                    txtVertical.Text = valor.ToString("N2");
                }

                if (Band)
                {
                    txtVertical.Text = "";
                    txtVertical.Focus();
                }
            }
        }

        private void txtMaxima_Validated(object sender, EventArgs e)
        {
            if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "01" && !string.IsNullOrEmpty(txtMaxima.Text))
            {

                bool Band = false;

                if (!decimal.TryParse(txtMaxima.Text, out decimal valor))
                {
                    mensaje = "Valor inválido, debe escribir números";
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje(mensaje);
                    _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                    _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                    _FrmMensajes.ShowDialog();
                    //MessageBox.Show("Valor inválido, debe escribir números", "Inválido", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtMaxima.Text = "";
                    return;
                }

                if (valor < 10 || valor > 90)
                {
                    mensaje = "Valor inválido, rango entre 10 y 90";
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje(mensaje);
                    _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                    _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                    _FrmMensajes.ShowDialog();
                    //MessageBox.Show($"Valor inválido, rango entre 10 y 90", "Rango", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Band = true;
                }
                else
                {
                    txtMaxima.Text = valor.ToString("N2");
                }

                if (Band)
                {
                    txtMaxima.Text = "";
                    txtMaxima.Focus();
                }
            }
        }

        private void txtPuente_Validated(object sender, EventArgs e)
        {
            if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "01" && !string.IsNullOrEmpty(txtPuente.Text))
            {

                bool Band = false;

                if (!decimal.TryParse(txtPuente.Text, out decimal valor))
                {

                    mensaje = "Valor inválido, debe escribir números";
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje(mensaje);
                    _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                    _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                    _FrmMensajes.ShowDialog();
                    //_FrmMensajes.Location = new Point(1, 1);

                    //MessageBox.Show("Valor inválido, debe escribir números", "Inválido", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtPuente.Text = "";
                    return;
                }

                if (valor < 5 || valor > 30)
                {
                    mensaje = "Valor inválido, rango entre 5 y 30";
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje(mensaje);
                    // Establece la posición del formulario
                    _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                    _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                    _FrmMensajes.ShowDialog();
                    //MessageBox.Show($"Valor inválido, rango entre 50 y 30", "Rango", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Band = true;
                }
                else
                {
                    txtPuente.Text = valor.ToString("N2");
                }

                if (Band)
                {
                    txtPuente.Text = "";
                    txtPuente.Focus();
                }
            }
        }

        private void Txt_Pnl2_Cedula_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DataTable dtCliente = _L_Articulo.ObtenerCliente(Txt_Pnl2_Cedula.Text.Substring(0,1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length-2));
                if (dtCliente.Rows.Count > 0)
                {
                    Txt_Pnl_2_Nombre.Text = dtCliente.Rows[0][0].ToString();
                    //Guarda en tb_trabajo temporal
                    //_D_Articulo.Agregar_TB_TRABAJO(_D_DetalleOrden.TB_PARAMETRO("SucursalId"), "", "", Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), "002", Convert.ToInt32(Txt_Pnl2_Examen.Text)
                    //  , txtHorizontal.Text, txtVertical.Text, txtMaxima.Text, txtPuente.Text, "0", "0", "A", "Cerca", "Cerca", "QUO", "001", "T", TB_USUARIO.COD_USR, "02", "CONVENCIONAL", "0", "0", "0", "0");

                }
            }
        }

        private void Txt_Pnl3_CambioPrecioNuevo_Validating(object sender, CancelEventArgs e)
        {
            if (Txt_Pnl3_CambioPrecioNuevo.Text == "" && string.IsNullOrEmpty(Txt_Pnl3_CambioPrecioNuevo.Text))
            {
                Txt_Pnl3_CambioPrecioNuevo.Text = "0,00";
            }
            else
            {
                FormatoBs(Convert.ToDouble(Txt_Pnl3_CambioPrecioNuevo.Text), Txt_Pnl3_CambioPrecioNuevo);

            }
        }

        private void Btn_Tap3_Cancelar_Garantia_Click(object sender, EventArgs e)
        {

            // Desvincular el DataGridView de su fuente de datos
            Dgv_Pnl3_Garantia.DataSource = null;
            Dgv_Pnl3_Garantia.DataMember = null;

            // Verificar y eliminar la columna "Eliminar" si existe
            var dataGridViewColumn2 = Dgv_Pnl3_Garantia.Columns["E"];
            if (dataGridViewColumn2 != null)
            {
                Dgv_Pnl3_Garantia.Columns.Remove(dataGridViewColumn2);
            }

            if (Cbx_Tap2_Tipo_Examen.Text == "CONTACTO")
            {
                Cbx_Pnl2_Trbajo.SelectedIndex = 1;
            }
            else if (Cbx_Tap2_Tipo_Examen.Text == "CONVENCIONAL")
            {
                Cbx_Pnl2_Trbajo.SelectedIndex = 0;
            }

            VisualizarPanel("MostrarCabezeraSecundaria");
            HabilitacionControl("CabezeraPrincipal");
        }

        private void Btn_Tap3_Aceptar_Garantia_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Cbx_Pnl3_Garantia.Text))
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Debe seleccionar el motivo de la reposicion");
                _FrmMensajes.ShowDialog();
                return; // Salir 
            }

            _L_Articulo.GarantiaCristales_Selecion(Dgv_Pnl3_Garantia, ref Os_Garantia_Trabajo, ref Numero_Examen_Garantia_Trabajo);
            Txt_Pnl2_Examen.Text = Numero_Examen_Garantia_Trabajo;
            VisualizarPanel("MostrarCabezeraSecundaria");
            HabilitacionControl("CabezeraPrincipal");
            Cbx_Pnl2_Laboratorio.Enabled = true;
            Cbx_Pnl2_Servicio.Enabled = true;
        }

      

        private void Txt_Tap1_Cedula_Pagador_KeyDown(object sender, KeyEventArgs e)
        {
            // Verifica si la tecla presionada es F2
            if (e.KeyCode == Keys.F2)
            {

                txtClienteP.Text = "";
                // Evita que el evento KeyDown se siga propagando (opcional)
                e.SuppressKeyPress = true;
                buscarclientep();
                txtClienteP.Focus();

            }// Verifica si la tecla presionada es F2

        }


        private void Txt_Tap1_Cedula_Pagador_MouseLeave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Txt_Tap1_Cedula_Pagador.Text))
            {
                try
                {
                    string cedula = Txt_Tap1_Cedula_Pagador.Text.Trim();
                    string nacio = this.Cbx_Tap1_Nacionalidad_Pagador.Text.Trim();

                    DataTable dtCliente = _L_Cliente.ObtenerClientePorCedula(cedula, nacio); // Usa la instancia _L_Cliente

                    if (dtCliente != null && dtCliente.Rows.Count > 0 && !string.IsNullOrEmpty(dtCliente.Rows[0]["CTE_CedIden"].ToString()))
                    {
                        // Asigna los valores de la base de datos a las cajas de texto
                        Txt_Tap1_Cedula_Pagador.Text = dtCliente.Rows[0]["CTE_CedIden"].ToString(); // Ajusta el nombre de la columna
                        Txt_Tap1_Nombre_Pagador.Text = dtCliente.Rows[0]["CTE_PNombre"].ToString(); // Ajusta el nombre de la columna

                        if (dtCliente.Rows[0]["CTE_RETIVA"] != DBNull.Value && Convert.ToBoolean(dtCliente.Rows[0]["CTE_RETIVA"]))
                        {
                            Chex_Tap1_Iva_Pagador.SetItemChecked(1, true); // Marcar el segundo elemento
                        }
                        else
                        {
                            Chex_Tap1_Iva_Pagador.SetItemChecked(1, false); // Desmarcar el segundo elemento si es falso o nulo
                        }




                        if (dtCliente.Rows[0]["CTE_RETISLR"] != DBNull.Value && Convert.ToBoolean(dtCliente.Rows[0]["CTE_RETISLR"]))
                        {
                            Chex_Tap1_Iva_Pagador.SetItemChecked(0, true); // Marcar el segundo elemento
                        }
                        else
                        {
                            Chex_Tap1_Iva_Pagador.SetItemChecked(0, false); // Desmarcar el segundo elemento si es falso o nulo
                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocurrió un error al obtener la información del cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
            }

        }


        private void Txt_Tap1_Cedula_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                _teclaF2Presionada = true;
                txtClienteBuscar.Clear(); ;

                LimpiarCampos2();
                buscarcliente();
                txtClienteBuscar.Focus();
            }
            else
            {
                _teclaF2Presionada = false; // Asegurar que sea false para otras teclas
            }



            if (e.KeyCode == Keys.Enter)
            {

                if (string.IsNullOrEmpty(Txt_Tap1_Nombre.Text))
                {
                    LimpiarCampos2();
                    // Llama al evento MouseLeave de Txt_Tap1_Cedula
                    Txt_Tap1_Cedula_MouseLeave(sender, e); // Llama al evento como si fuera un MouseLeave
                                                           // Establece el foco en Txt_Tap1_Nombre
                    Txt_Tap1_Nombre.Focus();
                }
                // Verifica si Txt_Tap1_Nombre no está vacío y bloquea los campos si es necesario
                if (!string.IsNullOrEmpty(Txt_Tap1_Nombre.Text))
                {
                    Txt_Tap1_Cedula.Enabled = false;
                    Cbx_Tap1_Nacionalidad.Enabled = false;
                }



                // Indica que el evento Enter ha sido manejado, para que no se procese de la manera predeterminada.
                e.Handled = true;
                e.SuppressKeyPress = true; // Evita el "ding" del Enter.
            }


            // Verifica si la tecla presionada es F2
            if (e.KeyCode == Keys.F2)
            {
                e.SuppressKeyPress = true;
                LimpiarCamposTodos();
                buscarcliente();

            }
        }

        private void Txt_Tap1_Cedula_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo dígitos (0-9) y teclas de control (como Backspace)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                // Si la tecla presionada no es un dígito ni una tecla de control,
                // se marca el evento como manejado para evitar que el carácter se escriba
                e.Handled = true;
            }
        }

        private void Txt_Tap1_Cedula_KeyUp(object sender, KeyEventArgs e)
        {
            if (!_teclaF2Presionada && string.IsNullOrEmpty(Cbx_Tap1_Nacionalidad.Text.Trim()))
            {


                Txt_Tap1_Cedula.Clear();
                Pnl_2_Msj.Visible = true;
                txt_pl2_msj.Text = "Debe seleccionar la Nacionalidad antes de ingresar la Cédula";
                //pb_pl2_mj.Visible = true;
                button3.Focus();

                //Cbx_Tap1_Nacionalidad.Focus();
                e.Handled = true; // Indica que el evento KeyUp ha sido manejado, evitando acciones adicionales del control
            }
            _teclaF2Presionada = false; // Restablecer la variable después de usarla
        }

        private void Txt_Tap1_Cedula_Leave(object sender, EventArgs e)
        {
            limpearExamen();
        }

        public void Txt_Tap1_Cedula_MouseLeave(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(Txt_Tap1_Nombre.Text))
            {




                if (!string.IsNullOrEmpty(Txt_Tap1_Cedula.Text))
                {
                    try
                    {

                        bool esNumero = true;
                        string cedula = Txt_Tap1_Cedula.Text.Trim();
                        foreach (char c in cedula)
                        {
                            if (!char.IsDigit(c))
                            {
                                esNumero = false;
                                break;
                            }
                        }

                        if (!esNumero)
                        {
                            // El campo cédula contiene caracteres no numéricos.

                            Pnl_2_Msj.Visible = true;
                            txt_pl2_msj.Text = "El campo Cédula debe contener solo números";
                            //pb_pl2_mj.Visible = true;

                            //MessageBox.Show("El campo Cédula debe contener solo números.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            Txt_Tap1_Cedula.SelectAll(); // Selecciona todo el texto para facilitar la corrección.
                            Txt_Tap1_Cedula.Clear();
                            Txt_Tap1_Cedula.Focus();
                            return;
                        }


                        string nacio = Cbx_Tap1_Nacionalidad.Text.Trim();
                        // string cedula = Txt_Tap1_Cedula.Text.Trim();


                        dtCliente = _L_Cliente.ObtenerClientePorCedula(cedula, nacio); // Usa la instancia _L_Cliente

                        if (dtCliente != null && dtCliente.Rows.Count > 0 && !string.IsNullOrEmpty(dtCliente.Rows[0]["CTE_CedIden"].ToString()))
                        {
                            LimpiarCampos2();
                            llenarcampos();
                            btnExamen.Enabled = true;
                        }
                        else
                        {

                            LimpiarCampos2();
                            //llenarcampos();
                            // Opcionalmente, puedes limpiar las otras cajas de texto o deshabilitarlas.
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ocurrió un error al obtener la información del cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    }
                }
            }
        }

     

        private void Txt_Tap1_Email_Leave(object sender, EventArgs e)
        {

            string textoIngresado = Txt_Tap1_Email.Text;
            if (!string.IsNullOrEmpty(textoIngresado)) // Solo revisa si no está vacío
            {
                Txt_Tap1_Email.Text = Txt_Tap1_Email.Text.ToLower();
                if (EsEmailValido(textoIngresado))
                {
                    // El valor ingresado parece una dirección de correo electrónico válida
                    //MessageBox.Show("El formato del correo electrónico es válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    // Puedes realizar alguna acción aquí
                }
                else
                {
                    // El valor ingresado no parece una dirección de correo electrónico válida

                    Pnl_2_Msj.Visible = true;
                    txt_pl2_msj.Text = "El formato del correo electrónico no es válido";
                    //pb_pl2_mj.Visible = true;
                    button3.Focus();

                    Txt_Tap1_Email.Focus(); // Devolver el foco al TextBox
                }
            }
        }

        
        private void Txt_Tap1_TLF_Local_Leave(object sender, EventArgs e)
        {



            string textoIngresado = Txt_Tap1_TLF_Local.Text;
            if (!string.IsNullOrEmpty(textoIngresado)) // Solo revisa si no está vacío
            {
                if (EsNumeroDeSieteDigitos(textoIngresado))
                {
                    // El valor ingresado es un número de 7 dígitos válido
                    //MessageBox.Show("El número de teléfono es válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    // Puedes realizar alguna acción aquí
                }
                else
                {

                    Pnl_2_Msj.Visible = true;
                    txt_pl2_msj.Text = "El número de teléfono debe contener exactamente 7 dígitos";
                    //pb_pl2_mj.Visible = true;

                    // El valor ingresado no es un número de 7 dígitos válido
                    //MessageBox.Show("El número de teléfono debe contener exactamente 7 dígitos.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    //Txt_Tap1_TLF_Local.Focus(); // Devolver el foco al TextBox para que el usuario corrija
                }
            }
        }

       

        private void Txt_Tap1_TLF_Celular_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo dígitos (0-9) y teclas de control (como Backspace)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                // Si la tecla presionada no es un dígito ni una tecla de control,
                // se marca el evento como manejado para evitar que el carácter se escriba
                e.Handled = true;
            }
        }


        private void Txt_Tap1_TLF_Celular_Leave(object sender, EventArgs e)
        {
            string textoIngresado = Txt_Tap1_TLF_Celular.Text;
            // Verifica primero si el campo está vacío
            if (!string.IsNullOrEmpty(textoIngresado)) // Solo revisa si no está vacío
            {
                if (EsNumeroDeSieteDigitos(textoIngresado))
                {
                    // El valor ingresado es un número de 7 dígitos válido
                    //MessageBox.Show("El número de teléfono es válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    // Puedes realizar alguna acción aquí
                }
                else
                {
                    // El valor ingresado no es un número de 7 dígitos válido

                    Pnl_2_Msj.Visible = true;
                    txt_pl2_msj.Text = "El número de teléfono debe contener exactamente 7 dígitos";
                    //pb_pl2_mj.Visible = true;



                    //MessageBox.Show("El número de teléfono debe contener exactamente 7 dígitos.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    //  Txt_Tap1_TLF_Celular.Focus(); // Devolver el foco al TextBox para que el usuario corrija
                }
            }
        }

        private void Dt_Tab1_nacimiento_ValueChanged(object sender, EventArgs e)
        {
            // Obtener la fecha de nacimiento del DateTimePicker
            DateTime fechaNacimiento = Dtp_Tap1_Nacimiento.Value;

            // Calcular la edad en años y meses
            (int años, int meses) = CalcularEdadCompleta(fechaNacimiento);

            // Mostrar la edad en el TextBox con el formato adecuado
            if (años > 0)
            {
                Txt_Tap1_Edad.Text = $"{años} año{(años == 1 ? "" : "s")}";
            }
            else if (meses > 0)
            {
                Txt_Tap1_Edad.Text = $"{meses} mes{(meses == 1 ? "" : "es")}";
            }
            else
            {
                Txt_Tap1_Edad.Text = "0"; // O algún otro texto apropiado para recién nacidos
            }


        }

        private void Cbx_Tap1_Estado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cbx_Tap1_Estado.SelectedItem != null)
            {
                TB_MAESEDO estadoSeleccionado = (TB_MAESEDO)Cbx_Tap1_Estado.SelectedItem;
                string codigoEstadoSeleccionado = estadoSeleccionado.COD_Edo;
                LlenarCbx_Tap1_Ciudad(codigoEstadoSeleccionado);
            }
            else
            {
                // Si no hay estado seleccionado, limpiar el ComboBox de ciudades
                Cbx_Tap1_Ciudad.DataSource = null;

            }
        }

        private void Btn_Pnl3_Cancelar_Click(object sender, EventArgs e)
        {
            LimpiarCamposTodos();
            Cbx_Pnl2_Trbajo.SelectedIndex = 0;
            LimpiarGrid();
            btnCargarOrden.Enabled = false;
       
            //Btn_Tap3_Cancelar.PerformClick();
            BloquearCamposE();
        }

        private void Cbx_Tap1_Nacionalidad_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Cbx_Tap1_Nacionalidad_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                _teclaF2Presionada = true;
            }
            else
            {
                _teclaF2Presionada = false; // Asegurar que sea false para otras teclas
            }
        }


        /*MEIFER*/
        // Evento CellEndEdit: Se dispara después de que la edición de la celda ha terminado.
        // Es útil para limpiar mensajes de error una vez que la edición ha finalizado.

        

        private void Dgv_Pnl2_Querato_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Asegúrate de que el índice de la fila sea válido.
            if (e.RowIndex >= 0)
            {
                // Limpia el mensaje de error de la fila después de que la edición ha terminado.
                // Esto es importante para que el ícono de error desaparezca si el usuario corrigió el valor.
                Dgv_Pnl2_Querato.Rows[e.RowIndex].ErrorText = string.Empty;
            }
        }

        private void Dgv_Pnl2_Querato_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Obtiene la celda que cambió (this actually refers to the cell being entered)
            DataGridViewCell CellLeave = Dgv_Pnl2_Querato.Rows[e.RowIndex].Cells[e.ColumnIndex];

            // Verifica que el valor no sea nulo ni vacío
            if (CellLeave.Value != null && CellLeave.Value.ToString() != string.Empty)
            {
                // Llama al evento CellLeave, pasando los mismos sender y argumentos
                Dgv_Pnl2_Querato_CellLeave(sender, e);
            }
        }

        private void Dgv_Pnl2_Querato_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            // Asegúrate de que el índice de la fila y la columna sean válidos para evitar errores
            // al acceder a celdas que no son de datos (ej. encabezados).
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            // Obtiene el nombre de la columna actual.
            string columnName = Dgv_Pnl2_Querato.Columns[e.ColumnIndex].Name;

            // *** LÍNEA DE DEPURACIÓN: Muestra el nombre de la columna actual ***
            // Esto te ayudará a verificar si el nombre se está recuperando correctamente.
            // Si aparece vacío o un nombre inesperado, el problema está en la configuración de las columnas.
            //MessageBox.Show($"Columna actual: '{columnName}'");

            // Verifica si la columna actual es una de las columnas que necesitamos validar."QUERATOMI1"; QUERATOMI1
            // El operador '||' (OR) permite que la validación se aplique a cualquiera de las dos.
            if (columnName == "QUERATOMD1" || columnName == "QUERATOMD2")
            {
                // Obtiene el valor de la celda. Se usa el operador ?. para manejar valores nulos.
                string cellValue = Dgv_Pnl2_Querato.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();

                // Intenta convertir el valor de la celda a un tipo decimal.
                if (decimal.TryParse(cellValue, out decimal valor))
                {
                    // Realiza la validación del rango utilizando las variables minMeridianoCorneal y maxMeridianoCorneal.
                    // Estas variables deben ser actualizadas por tu lógica de negocio (ej. RadioButtons).
                    // if (valor < minMeridianoCorneal || valor > maxMeridianoCorneal)
                    if ((valor < minMeridianoCorneal || valor > maxMeridianoCorneal) && valor != 0)
                    {
                        Dgv_Pnl2_Querato.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = minMeridianoCorneal;
                        // Si el valor está fuera del rango, establece un mensaje de error en la fila.
                        // Nota: CellLeave no permite cancelar la salida de la celda, solo mostrar una advertencia.
                        //e.Cancel = true; // Cancela la validación
                        Pnl_2_Msj.Visible = true;
                        txt_pl2_msj.Text = $"Los valores permitidos están entre  {minMeridianoCorneal} y {maxMeridianoCorneal}.";
                        //pb_pl2_mj.Visible = true;


                        //Dgv_Pnl2_Querato.Rows[e.RowIndex].ErrorText = $"Advertencia: El valor está fuera del rango permitido ({minMeridianoCorneal}-{maxMeridianoCorneal}).";
                    }
                    else
                    {
                        // Si el valor es válido, asegúrate de limpiar cualquier mensaje de error anterior para esa fila.
                        Dgv_Pnl2_Querato.Rows[e.RowIndex].ErrorText = string.Empty;
                    }
                }
                else
                {
                    // Si el valor no puede ser convertido a un número decimal, establece un mensaje de error.
                    Dgv_Pnl2_Querato.Rows[e.RowIndex].ErrorText = "Advertencia: Por favor, introduce un número válido.";
                }
            }
        }

        private void Dgv_Pnl2_Querato_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            // Obtiene la celda que cambió (this actually refers to the cell being entered)
            DataGridViewCell CellLeave = Dgv_Pnl2_Querato.Rows[e.RowIndex].Cells[e.ColumnIndex];

            // Verifica que el valor no sea nulo ni vacío
            if (CellLeave.Value != null && CellLeave.Value.ToString() != string.Empty)
            {
                // Llama al evento CellLeave, pasando los mismos sender y argumentos
                Dgv_Pnl2_Querato_CellLeave(sender, e);
            }
        }

        

        private void Dgv_Pnl2_Querato_Leave(object sender, EventArgs e)
        {
            // Assuming 'btnValidar' is the name of your button
            ValidarQueratometria();
        }

        private void CargarCbx_fijos()
        {
            string[] elementosArray = { "0212", "0232", "0236", "0239", "0241", "0242", "0243", "0244", "0251", "0255", "0256", "0247", "0273", "0276", "0274", "0285", "0286", "0281", "0282", "0283", "0291", "0293", "0268", "0269", "0261", "0264", "0265", "0272", "0271", "0246", "0248", "0258", "0253", "0240", "0278", "0245", "0249", "0259", "0235", "0238", "0252", "0275", "0234", "0237", "0292", "0295", "0296", "0257", "0294", "0277", "0262", "0263", "0266", "0267", "0287", "0288", "0289", "0284" };
            Cbx_Tap1_TLF_Local.Items.AddRange(elementosArray);
            Cbx_Tap1_TLF_Local.DropDownWidth = DropDownWidth(Cbx_Tap1_TLF_Local);

            string[] elementosArray2 = { "0414","0422",
                "0424",
                "0416",
                "0426",
                "0412"};
            this.Cbx_Tap1_TLF_Celular.Items.AddRange(elementosArray2);
            Cbx_Tap1_TLF_Celular.DropDownWidth = DropDownWidth(Cbx_Tap1_TLF_Celular);


            //string[] elementosArray3 = { "INTERNO",
            //    "EXTERNO",
            //    "EXTERNO ESPECIAL"};
            //Cbx_Pnl2_Trbajo.Items.AddRange(elementosArray3);
            //Cbx_Pnl2_Trbajo.DropDownWidth = DropDownWidth(Cbx_Pnl2_Trbajo);

            string[] elementosArray3a = { "INTERNO",
                "EXTERNO"};
            Cbx_Tap2_Tipo_Optome.Items.AddRange(elementosArray3a);
            Cbx_Tap2_Tipo_Optome.DropDownWidth = DropDownWidth(Cbx_Tap2_Tipo_Optome);



            string[] elementosArray5 = { "AMBOS",
                "OJO DERECHO","OJO IZQUIERDO"};
            this.Cbx_Tap2_Ojo.Items.AddRange(elementosArray5);
            Cbx_Tap2_Ojo.DropDownWidth = DropDownWidth(Cbx_Tap2_Ojo);


            string[] elementosArray5a = { "Cerca", "Lejos", "Bifocal", "Progresivo", "Balance", "Intermedia" };
            this.cbVisionDerecha.Items.AddRange(elementosArray5a);
            this.cbVisionIzquierda.Items.AddRange(elementosArray5a);

            cbVisionDerecha.DropDownWidth = DropDownWidth(cbVisionDerecha);
            cbVisionIzquierda.DropDownWidth = DropDownWidth(cbVisionIzquierda);
            //this.Cbx_Tap2_Visiond.Items.AddRange(elementosArray5a);
            //this.Cbx_Tap2_Visioni.Items.AddRange(elementosArray5a);
            //Cbx_Tap2_Visioni.DropDownWidth = DropDownWidth(Cbx_Tap2_Visioni);



            string[] elementosArray6a =
                { "CONTACTO",
                "CONVENCIONAL"};
            this.Cbx_Tap2_Tipo_Examen.Items.AddRange(elementosArray6a);
            Cbx_Tap2_Tipo_Examen.DropDownWidth = DropDownWidth(Cbx_Tap2_Tipo_Examen);




        }

        private void LlenarCbx_Tap1_Estado()
        {
            List<TB_MAESEDO> estados = _L_Cliente.ObtenerEstados();
            if (estados != null)
            {
                Cbx_Tap1_Estado.DataSource = estados;
                Cbx_Tap1_Estado.DisplayMember = "EDO_Nombre"; // El nombre del estado a mostrar
                Cbx_Tap1_Estado.ValueMember = "COD_Edo";   // El código del estado como valor asociado
                Cbx_Tap1_Estado.DropDownWidth = DropDownWidth(Cbx_Tap1_Estado);
            }
            else
            {
                MessageBox.Show("Error al cargar los estados: " + _L_Cliente.stringBuilder.ToString());
            }
        }


        private void CargarCbx_Tap1_Nacionalidad()
        {


            //Cbx_Tap1_Nacionalidad.Width = 40; // Ajusta el valor según sea necesario
            string[] elementosArray = { "V", "E", "N" };
            Cbx_Tap1_Nacionalidad.Items.AddRange(elementosArray);
            Cbx_Tap1_Nacionalidad.DropDownWidth = DropDownWidth(Cbx_Tap1_Nacionalidad);

            //Cbx_Tap1_Nacionalidad_Pagador.Width = 20; // Ajusta el valor según sea necesario
            string[] elementosArrayP = { "V", "E", "J", "G" };
            this.Cbx_Tap1_Nacionalidad_Pagador.Items.AddRange(elementosArrayP);
            Cbx_Tap1_Nacionalidad_Pagador.DropDownWidth = DropDownWidth(Cbx_Tap1_Nacionalidad_Pagador);
        }


        int DropDownWidth(ComboBox myCombo)
        {
            int maxWidth = 0;
            int temp = 0;
            Label label1 = new Label();

            foreach (var obj in myCombo.Items)
            {
                label1.Text = obj.ToString();
                temp = label1.PreferredWidth;
                if (temp > maxWidth)
                {
                    maxWidth = temp;
                }
            }
            label1.Dispose();
            return maxWidth;
        }

        private bool ValidarQueratometria()
        {
            foreach (DataGridViewRow row in Dgv_Pnl2_Querato.Rows)
            {
                // Skip the new row if it's present and not committed
                if (row.IsNewRow)
                {
                    continue;
                }

                bool hasNonZero = false;
                bool hasZero = false;

                // Check each cell in the current row
                foreach (DataGridViewCell cell in row.Cells)
                {
                    // Only consider cells that are DataGridViewNumericUpDownColumn type
                    // and ensure the cell value is not null.
                    if (cell is DataGridViewNumericUpDownCell numericCell && numericCell.Value != null)
                    {
                        if (Convert.ToDecimal(numericCell.Value) != 0)
                        {
                            hasNonZero = true;
                        }
                        else
                        {
                            hasZero = true;

                        }
                    }
                }

                // Apply the validation rule: if there's a non-zero value, there shouldn't be any zero values.
                //if (hasNonZero && hasZero)
                //{
                //    Pnl_2_Msj.Visible = true;
                //    txt_pl2_msj.Text = "Revisar los valores de Queratomia no debe tener valores en cero";
                //    //pb_pl2_mj.Visible = true;

                //    return false; // Stop validation on the first error found
                //}

            }
            return true;
        }

        //private void ConfigurarDgv_Pnl2_medconv()
        //{
        //    this.Dgv_Pnl2_medconv.DefaultCellStyle.Font = new Font("Century Gothic", 13);

        //    // Change the font for the COLUMN HEADERS
        //    this.Dgv_Pnl2_medconv.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 10);

        //    // Crear un DataTable para almacenar los datos del DataGridView
        //    DataTable dt = new DataTable();

        //    // Agregar la fila fija para el ojo derecho
        //    DataRow filaDerecha = dt.NewRow();
        //    dt.Rows.Add(filaDerecha);

        //    // Asignar el DataTable como fuente de datos del DataGridView
        //    Dgv_Pnl2_medconv.DataSource = dt;
        //    Dgv_Pnl2_medconv.AutoGenerateColumns = false; // Desactivar la generación automática de columnas

        //    // Opcional: Configurar propiedades del DataGridView para mejor visualización
        //    Dgv_Pnl2_medconv.AllowUserToAddRows = false;
        //    Dgv_Pnl2_medconv.AllowUserToDeleteRows = false;
        //    Dgv_Pnl2_medconv.ReadOnly = false;
        //    Dgv_Pnl2_medconv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        //    //Dgv_Pnl2_medconv.ColumnHeadersVisible = true;
        //    Dgv_Pnl2_medconv.RowHeadersVisible = false;
        //    Dgv_Pnl2_medconv.AllowUserToResizeColumns = false; // Bloquear el cambio de tamaño de las columnas
        //    Dgv_Pnl2_medconv.AllowUserToResizeRows = false;    // Bloquear el cambio de tamaño de las filas

        //    //,[T_DISTANCIAVERTICE]
        //    //,[T_ANGULOPANTOSCOPICO]
        //    //,[T_ANGULOFACIAL]
        //    //,[T_DISTANCIADELECTURA] DV, AP, AF y DDL

        //    // Crear y agregar las columnas DataGridView
        //    // Crear el DataTable con la columna esperada
        //    DataTable dta = new DataTable();
        //    dta.Columns.Add("T_DISTANCIAVERTICE", typeof(decimal)); // Debe coincidir con DataPropertyName

        //    // Agregar una fila con un valor inicial
        //    DataRow fila = dta.NewRow();
        //    fila["T_DISTANCIAVERTICE"] = 0.00M;
        //    dta.Rows.Add(fila);

        //    // Asignar el DataTable como fuente de datos
        //    dgvMedEspeciales.DataSource = dta;
        //    dgvMedEspeciales.AutoGenerateColumns = false;

        //    // Configuración visual del DataGridView
        //    dgvMedEspeciales.Font = new Font("Century Gothic", 13);
        //    dgvMedEspeciales.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 10);
        //    dgvMedEspeciales.ReadOnly = false;
        //    dgvMedEspeciales.AllowUserToAddRows = false;
        //    dgvMedEspeciales.AllowUserToDeleteRows = false;
        //    dgvMedEspeciales.AllowUserToResizeColumns = false;
        //    dgvMedEspeciales.AllowUserToResizeRows = false;
        //    dgvMedEspeciales.RowHeadersVisible = false;
        //    dgvMedEspeciales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        //    dgvMedEspeciales.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;

        //    // Crear y agregar columna personalizada editable con 2 decimales
        //    DataGridViewNumericUpDownColumn Dist_VertColumn = new DataGridViewNumericUpDownColumn();
        //    Dist_VertColumn.Name = "T_DISTANCIAVERTICE";
        //    Dist_VertColumn.DataPropertyName = "T_DISTANCIAVERTICE";
        //    Dist_VertColumn.HeaderText = "DV";
        //    Dist_VertColumn.DecimalPlaces = 2;
        //    Dist_VertColumn.Minimum = 0;
        //    Dist_VertColumn.Maximum = 30;
        //    Dist_VertColumn.Increment = 1;
        //    Dist_VertColumn.Width = 100;
        //    Dist_VertColumn.ReadOnly = false;

        //    dgvMedEspeciales.Columns.Add(Dist_VertColumn);



        //    DataGridViewNumericUpDownColumn Ang_PantColumn = new DataGridViewNumericUpDownColumn();
        //    Ang_PantColumn.Name = "T_ANGULOPANTOSCOPICO";
        //    Ang_PantColumn.DataPropertyName = "T_ANGULOPANTOSCOPICO";
        //    Ang_PantColumn.HeaderText = "AP";
        //    //Ang_PantColumn.HeaderText = "DV";
        //    //Ang_PantColumn.DecimalPlaces = 2;
        //    Ang_PantColumn.Minimum = -5;
        //    Ang_PantColumn.Maximum = 30;
        //    Ang_PantColumn.Increment = 1;
        //    Dgv_Pnl2_medconv.Columns.Add(Ang_PantColumn);


        //    DataGridViewNumericUpDownColumn Ang_FacColumn = new DataGridViewNumericUpDownColumn();
        //    Ang_FacColumn.Name = "T_ANGULOFACIAL";
        //    Ang_FacColumn.DataPropertyName = "T_ANGULOFACIAL";
        //    Ang_FacColumn.HeaderText = "AF";
        //    //Ang_FacColumn.HeaderText = "DV";
        //    Ang_FacColumn.DecimalPlaces = 2;
        //    Ang_FacColumn.Minimum = -5;
        //    Ang_FacColumn.Maximum = 25;
        //    Dist_VertColumn.Increment = 1;
        //    Dgv_Pnl2_medconv.Columns.Add(Ang_FacColumn);

        //    DataGridViewNumericUpDownColumn DDLColumn = new DataGridViewNumericUpDownColumn();
        //    DDLColumn.Name = "T_DISTANCIADELECTURA";
        //    DDLColumn.DataPropertyName = "T_DISTANCIADELECTURA";
        //    DDLColumn.HeaderText = "DDL";
        //    //DDLColumn.HeaderText = "DV";
        //    DDLColumn.DecimalPlaces = 2;
        //    DDLColumn.Minimum = 0.25M;
        //    DDLColumn.Maximum = 0.50M;
        //    DDLColumn.Increment = 0.01M;
        //    Dgv_Pnl2_medconv.Columns.Add(DDLColumn);

        //}

        private void ConfigurarDgv_Pnl2_cont()
        {
            this.Dgv_Pnl2_cont.DefaultCellStyle.Font = new Font("Century Gothic", 13);
            // Change the font for the COLUMN HEADERS
            this.Dgv_Pnl2_cont.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 10);
            // Crear un DataTable para almacenar los datos del DataGridView
            DataTable dt = new DataTable();
            dt.Columns.Add("Ojo", typeof(string));
            // Agregar las dos filas fijas
            DataRow filaDerecha = dt.NewRow();
            filaDerecha["Ojo"] = "Derecho";
            dt.Rows.Add(filaDerecha);

            DataRow filaIzquierda = dt.NewRow();
            filaIzquierda["Ojo"] = "Izquierdo";
            dt.Rows.Add(filaIzquierda);

            // Asignar el DataTable como fuente de datos del DataGridView
            Dgv_Pnl2_cont.DataSource = dt;
            Dgv_Pnl2_cont.AutoGenerateColumns = false; // Desactivar la generación automática de columnas

            // Opcional: Configurar propiedades del DataGridView para mejor visualización
            Dgv_Pnl2_cont.AllowUserToAddRows = false;
            Dgv_Pnl2_cont.AllowUserToDeleteRows = false;
            Dgv_Pnl2_cont.ReadOnly = false;
            Dgv_Pnl2_cont.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Dgv_Pnl2_cont.ColumnHeadersVisible = true;
            Dgv_Pnl2_cont.RowHeadersVisible = false;
            Dgv_Pnl2_cont.AllowUserToResizeColumns = false; // Bloquear el cambio de tamaño de las columnas
            Dgv_Pnl2_cont.AllowUserToResizeRows = false;   // Bloquear el cambio de tamaño de las filas


            //// Crear las columnas DataGridView y agregarlas al DataGridView
            //DataGridViewTextBoxColumn ojoColumn = new DataGridViewTextBoxColumn();
            //ojoColumn.Name = "Ojo";
            //ojoColumn.DataPropertyName = "Ojo";
            //ojoColumn.HeaderText = "Ojo";
            //Dgv_Pnl2_grid1.Columns.Add(ojoColumn);

            DataGridViewTextBoxColumn aesferaColumn = new DataGridViewTextBoxColumn();
            aesferaColumn.Name = "aEsfera";
            aesferaColumn.DataPropertyName = "aEsfera";
            aesferaColumn.HeaderText = "";
            aesferaColumn.Width = 10; // Puedes ajustar este valor según la fuente y el tamaño de la celda
                                      // Opcionalmente, puedes hacer que la columna no sea resizable por el usuario si el ancho fijo es importante
            aesferaColumn.Resizable = DataGridViewTriState.False;
            aesferaColumn.ReadOnly = true;
            Dgv_Pnl2_cont.Columns.Add(aesferaColumn);

            ////-------------------
            DataGridViewNumericUpDownColumn esferaColumn = new DataGridViewNumericUpDownColumn();
            esferaColumn.Name = "Esfera";
            esferaColumn.DataPropertyName = "Esfera";
            esferaColumn.HeaderText = "Esfera";
            esferaColumn.DecimalPlaces = 2;
            esferaColumn.Minimum = -30;
            esferaColumn.Maximum = +20M;
            esferaColumn.Increment = 0.25M;
            esferaColumn.Width = 60;
            esferaColumn.Resizable = DataGridViewTriState.False;
            Dgv_Pnl2_cont.Columns.Add(esferaColumn);

            Dgv_Pnl2_cont.CellFormatting += (sender, e) =>
            {
                if (e.ColumnIndex == Dgv_Pnl2_cont.Columns["Esfera"].Index && e.Value != null)
                {

                    if (e.Value == null || string.IsNullOrEmpty(e.Value.ToString()))
                    {
                        e.Value = "0.00"; // Asignar cero formateado
                        e.CellStyle.ForeColor = SystemColors.WindowText; // Color de texto predeterminado
                    }




                    decimal esferaValue;
                    if (decimal.TryParse(e.Value.ToString(), out esferaValue))
                    {
                        // Formato para números positivos (añadir el signo +)
                        if (esferaValue > 0)
                        {
                            e.Value = "+" + esferaValue.ToString("N2");
                        }
                        else if (esferaValue < 0)
                        {
                            // Si el valor es negativo, mostrarlo sin signo (si así lo prefieres)
                            e.Value = esferaValue.ToString("N2").Replace("-", "");
                            //e.CellStyle.ForeColor = Color.Red;
                        }
                        else // esferalValue == 0
                        {
                            e.Value = esferaValue.ToString("N2");
                            e.CellStyle.ForeColor = SystemColors.WindowText;
                        }




                    }
                    else
                    {
                        // Manejar casos donde el valor no es un número válido
                        e.CellStyle.ForeColor = SystemColors.WindowText;
                    }
                }
            };
            ///



            DataGridViewTextBoxColumn acilindroColumn = new DataGridViewTextBoxColumn();
            acilindroColumn.Name = "aCilindro";
            acilindroColumn.DataPropertyName = "aCilindro";
            acilindroColumn.HeaderText = "";
            // Establecer el ancho de la columna a un valor aproximado para un dígito
            acilindroColumn.Width = 10; // Puedes ajustar este valor según la fuente y el tamaño de la celda
                                        // Opcionalmente, puedes hacer que la columna no sea resizable por el usuario si el ancho fijo es importante
            acilindroColumn.Resizable = DataGridViewTriState.False;
            // Para no pintar la línea de separación de la columna de la derecha,
            // necesitas manejar el evento CellPainting del DataGridView.
            acilindroColumn.ReadOnly = true;
            Dgv_Pnl2_cont.Columns.Add(acilindroColumn);
            // Adjunta el evento CellPainting si aún no lo has hecho
            Dgv_Pnl2_cont.CellPainting += Dgv_Pnl2_conv_CellPainting_NoVerticalBorderA1;


            //////
            DataGridViewNumericUpDownColumn cilindroColumn = new DataGridViewNumericUpDownColumn();
            cilindroColumn.Name = "Cilindro";
            cilindroColumn.DataPropertyName = "Cilindro";
            cilindroColumn.HeaderText = "Cilindro";
            cilindroColumn.DecimalPlaces = 2;
            //cilindroColumn.Minimum = -5.75M; 12/07/2025
            //cilindroColumn.Maximum = +5.75M;
            cilindroColumn.Minimum = -10M;
            cilindroColumn.Maximum = +8M;
            cilindroColumn.Increment = 0.25M;
            cilindroColumn.Width = 60;
            cilindroColumn.Resizable = DataGridViewTriState.False;
            Dgv_Pnl2_cont.Columns.Add(cilindroColumn);

            ///

            ///


            Dgv_Pnl2_cont.CellFormatting += (sender, e) =>
            {
                if (Dgv_Pnl2_conv.Columns.Contains("Cilindro"))
                {
                    //DataGridViewColumn cilindroColumn = Dgv_Pnl2_conv.Columns["Cilindro"];
                    if (e.ColumnIndex == cilindroColumn.Index && e.Value != null)
                    {
                        decimal cilindroValue;
                        if (decimal.TryParse(e.Value.ToString(), out cilindroValue))
                        {
                            if (cilindroValue > 0)
                            {
                                e.Value = "+" + cilindroValue.ToString("N2");
                            }
                            else if (cilindroValue < 0)
                            {
                                e.Value = cilindroValue.ToString("N2").Replace("-", "");
                                //e.CellStyle.ForeColor = Color.Red;
                            }
                            else
                            {
                                e.Value = cilindroValue.ToString("N2");
                                e.CellStyle.ForeColor = SystemColors.WindowText;
                            }
                        }
                        else
                        {
                            e.CellStyle.ForeColor = SystemColors.WindowText;
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("¡Advertencia! La columna 'Cilindro' no se encontró en CellFormatting.");
                }
            };
            ///


            DataGridViewNumericUpDownColumn ejeColumn = new DataGridViewNumericUpDownColumn();
            ejeColumn.Name = "Eje";
            ejeColumn.DataPropertyName = "Eje";
            ejeColumn.HeaderText = "Eje";
            ejeColumn.Minimum = 0M;
            ejeColumn.Maximum = 180M;
            //ejeColumn.Increment = 5M;
            // Formato personalizado para mostrar siempre 3 dígitos
            //ejeColumn.DefaultCellStyle.Format = "000";
            ejeColumn.Width = 60;
            ejeColumn.Resizable = DataGridViewTriState.False;
            //ejeColumn.DecimalPlaces = 2;
            ejeColumn.Increment = 1M;
            Dgv_Pnl2_cont.Columns.Add(ejeColumn);



            DataGridViewNumericUpDownColumn adicionColumn = new DataGridViewNumericUpDownColumn();
            adicionColumn.Name = "Adicion";
            adicionColumn.DataPropertyName = "Adicion";
            adicionColumn.HeaderText = "ADD";
            adicionColumn.DecimalPlaces = 2;
            adicionColumn.Minimum = 0M;
            adicionColumn.Maximum = +3.50M;
            adicionColumn.Width = 60;
            adicionColumn.Increment = 1M;
            adicionColumn.Resizable = DataGridViewTriState.False;
            Dgv_Pnl2_cont.Columns.Add(adicionColumn);


            DataGridViewNumericUpDownColumn C_baseColumn = new DataGridViewNumericUpDownColumn();
            C_baseColumn.Name = "C_base";
            C_baseColumn.DataPropertyName = "C_base";
            C_baseColumn.HeaderText = "Curva Base";
            C_baseColumn.DecimalPlaces = 2;
            C_baseColumn.Minimum = 6.6M;
            C_baseColumn.Maximum = 10M;
            C_baseColumn.Width = 60;
            C_baseColumn.Increment = 1M;
            C_baseColumn.Resizable = DataGridViewTriState.False;
            Dgv_Pnl2_cont.Columns.Add(C_baseColumn);

            DataGridViewNumericUpDownColumn DiametroColumn = new DataGridViewNumericUpDownColumn();
            DiametroColumn.Name = "Diametro";
            DiametroColumn.DataPropertyName = "Diametro";
            DiametroColumn.HeaderText = "Diámetro ";
            DiametroColumn.DecimalPlaces = 2;
            DiametroColumn.Minimum = 0M; //Pase de 0 a 8.5 
            DiametroColumn.Maximum = 14.5M;
            //DiametroColumn.Increment = 0.50M; 12/07/2025
            DiametroColumn.Increment = 1M;
            DiametroColumn.Width = 60;
            DiametroColumn.Resizable = DataGridViewTriState.False;
            Dgv_Pnl2_cont.Columns.Add(DiametroColumn);

            if (Dgv_Pnl2_cont.Columns.Contains("aCilindro"))
            {
                Dgv_Pnl2_cont.Columns["aCilindro"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                Dgv_Pnl2_cont.Columns["aCilindro"].Width = 40; // Establecer el ancho fijo (aproximadamente 0.5 cm)
                Dgv_Pnl2_cont.Columns["aCilindro"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            // Configurar la columna "aEsfera" para que no se ajuste automáticamente
            if (Dgv_Pnl2_cont.Columns.Contains("aEsfera"))
            {
                Dgv_Pnl2_cont.Columns["aEsfera"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                Dgv_Pnl2_cont.Columns["aEsfera"].Width = 40; // Establecer el ancho fijo (aproximadamente 0.5 cm)
                Dgv_Pnl2_cont.Columns["aEsfera"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }



        }


        private void ConfigurarDgv_Pnl2_conv()
        {
            this.Dgv_Pnl2_conv.DefaultCellStyle.Font = new Font("Century Gothic", 13);
            // Change the font for the COLUMN HEADERS
            this.Dgv_Pnl2_conv.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 10);

            // Crear un DataTable para almacenar los datos del DataGridView
            DataTable dt = new DataTable();
            dt.Columns.Add("Ojo", typeof(string));
            //dt.Columns.Add("Esfera", typeof(decimal));
            //dt.Columns.Add("Cilindro", typeof(decimal));
            //dt.Columns.Add("Eje", typeof(int));
            //dt.Columns.Add("Adicion", typeof(decimal));
            //dt.Columns.Add("DNP_Lejos", typeof(decimal));
            //dt.Columns.Add("DPN_Cerca", typeof(decimal));
            //dt.Columns.Add("Agudeza_Visual", typeof(string));
            //dt.Columns.Add("Prisma1", typeof(decimal));
            //dt.Columns.Add("Grado1", typeof(decimal));
            //dt.Columns.Add("Prisma2", typeof(decimal));
            //dt.Columns.Add("Grado2", typeof(decimal));

            // Agregar las dos filas fijas
            DataRow filaDerecha = dt.NewRow();
            filaDerecha["Ojo"] = "Derecho";
            dt.Rows.Add(filaDerecha);

            DataRow filaIzquierda = dt.NewRow();
            filaIzquierda["Ojo"] = "Izquierdo";
            dt.Rows.Add(filaIzquierda);

            // Asignar el DataTable como fuente de datos del DataGridView
            Dgv_Pnl2_conv.DataSource = dt;
            //Dgv_Pnl2_conv.AutoGenerateColumns = false; // Desactivar la generación automática de columnas

            // Opcional: Configurar propiedades del DataGridView para mejor visualización
            Dgv_Pnl2_conv.AllowUserToAddRows = false;
            Dgv_Pnl2_conv.AllowUserToDeleteRows = false;
            Dgv_Pnl2_conv.ReadOnly = false;
            Dgv_Pnl2_conv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Dgv_Pnl2_conv.ColumnHeadersVisible = true;
            Dgv_Pnl2_conv.RowHeadersVisible = false;
            Dgv_Pnl2_conv.AllowUserToResizeColumns = false; // Bloquear el cambio de tamaño de las columnas
            Dgv_Pnl2_conv.AllowUserToResizeRows = false;   // Bloquear el cambio de tamaño de las filas


            //// Crear las columnas DataGridView y agregarlas al DataGridView
            //DataGridViewTextBoxColumn ojoColumn = new DataGridViewTextBoxColumn();
            //ojoColumn.Name = "Ojo";
            //ojoColumn.DataPropertyName = "Ojo";
            //ojoColumn.HeaderText = "Ojo";
            //Dgv_Pnl2_grid1.Columns.Add(ojoColumn);



            DataGridViewTextBoxColumn aesferaColumn = new DataGridViewTextBoxColumn();
            aesferaColumn.Name = "aEsfera";
            aesferaColumn.DataPropertyName = "aEsfera";
            aesferaColumn.HeaderText = "";
            aesferaColumn.Width = 10; // Puedes ajustar este valor según la fuente y el tamaño de la celda
                                      // Opcionalmente, puedes hacer que la columna no sea resizable por el usuario si el ancho fijo es importante
            aesferaColumn.Resizable = DataGridViewTriState.False;
            aesferaColumn.ReadOnly = true;
            Dgv_Pnl2_conv.Columns.Add(aesferaColumn);

            ////-------------------
            DataGridViewNumericUpDownColumn esferaColumn = new DataGridViewNumericUpDownColumn();
            esferaColumn.Name = "Esfera";
            esferaColumn.DataPropertyName = "Esfera";
            esferaColumn.HeaderText = "Esfera";
            esferaColumn.DecimalPlaces = 2;
            esferaColumn.Minimum = -20;
            esferaColumn.Maximum = +20M;
            esferaColumn.Increment = 0.25M;

            Dgv_Pnl2_conv.Columns.Add(esferaColumn);
            ///
            Dgv_Pnl2_conv.CellFormatting += (sender, e) =>
            {
                if (e.ColumnIndex == Dgv_Pnl2_conv.Columns["Esfera"].Index && e.Value != null)
                {

                    if (e.Value == null || string.IsNullOrEmpty(e.Value.ToString()))
                    {
                        e.Value = "0.00"; // Asignar cero formateado
                        e.CellStyle.ForeColor = SystemColors.WindowText; // Color de texto predeterminado
                    }




                    decimal esferaValue;
                    if (decimal.TryParse(e.Value.ToString(), out esferaValue))
                    {
                        // Formato para números positivos (añadir el signo +)
                        if (esferaValue > 0)
                        {
                            e.Value = "+" + esferaValue.ToString("N2");
                        }
                        else if (esferaValue < 0)
                        {
                            // Si el valor es negativo, mostrarlo sin signo (si así lo prefieres)
                            e.Value = esferaValue.ToString("N2").Replace("-", "");
                            //e.CellStyle.ForeColor = Color.Red;
                        }
                        else // esferalValue == 0
                        {
                            e.Value = esferaValue.ToString("N2");
                            e.CellStyle.ForeColor = SystemColors.WindowText;
                        }




                    }
                    else
                    {
                        // Manejar casos donde el valor no es un número válido
                        e.CellStyle.ForeColor = SystemColors.WindowText;
                    }
                }
            };
            ///



            DataGridViewTextBoxColumn acilindroColumn = new DataGridViewTextBoxColumn();
            acilindroColumn.Name = "aCilindro";
            acilindroColumn.DataPropertyName = "aCilindro";
            acilindroColumn.HeaderText = "";
            // Establecer el ancho de la columna a un valor aproximado para un dígito
            acilindroColumn.Width = 10; // Puedes ajustar este valor según la fuente y el tamaño de la celda
                                        // Opcionalmente, puedes hacer que la columna no sea resizable por el usuario si el ancho fijo es importante
            acilindroColumn.Resizable = DataGridViewTriState.False;
            // Para no pintar la línea de separación de la columna de la derecha,
            // necesitas manejar el evento CellPainting del DataGridView.
            Dgv_Pnl2_conv.Columns.Add(acilindroColumn);
            // Adjunta el evento CellPainting si aún no lo has hecho
            Dgv_Pnl2_conv.CellPainting += Dgv_Pnl2_conv_CellPainting_NoVerticalBorderA1;
            acilindroColumn.ReadOnly = true;

            //////
            DataGridViewNumericUpDownColumn cilindroColumn = new DataGridViewNumericUpDownColumn();
            cilindroColumn.Name = "Cilindro";
            cilindroColumn.DataPropertyName = "Cilindro";
            cilindroColumn.HeaderText = "Cilindro";
            cilindroColumn.DecimalPlaces = 2;
            cilindroColumn.Minimum = -5.75M;
            cilindroColumn.Maximum = +5.75M;
            cilindroColumn.Increment = 0.25M; //pase de 1 a 0.25 14/07/2025

            Dgv_Pnl2_conv.Columns.Add(cilindroColumn);

            ////
            ///

            ///
            Dgv_Pnl2_conv.CellFormatting += (sender, e) =>
            {
                if (e.ColumnIndex == Dgv_Pnl2_conv.Columns["Cilindro"].Index && e.Value != null)
                {

                    if (e.Value == null || string.IsNullOrEmpty(e.Value.ToString()))
                    {
                        e.Value = "0.00"; // Asignar cero formateado
                        e.CellStyle.ForeColor = SystemColors.WindowText; // Color de texto predeterminado
                    }




                    decimal cilindroValue;
                    if (decimal.TryParse(e.Value.ToString(), out cilindroValue))
                    {
                        // Formato para números positivos (añadir el signo +)
                        if (cilindroValue > 0)
                        {
                            e.Value = "+" + cilindroValue.ToString("N2");
                        }
                        else if (cilindroValue < 0)
                        {
                            // Si el valor es negativo, mostrarlo sin signo (si así lo prefieres)
                            e.Value = cilindroValue.ToString("N2").Replace("-", "");
                            //e.CellStyle.ForeColor = Color.Red;
                        }
                        else // esferalValue == 0
                        {
                            e.Value = cilindroValue.ToString("N2");
                            e.CellStyle.ForeColor = SystemColors.WindowText;
                        }




                    }
                    else
                    {
                        // Manejar casos donde el valor no es un número válido
                        e.CellStyle.ForeColor = SystemColors.WindowText;
                    }
                }
            };
            ///

            ///

            DataGridViewNumericUpDownColumn ejeColumn = new DataGridViewNumericUpDownColumn();
            ejeColumn.Name = "Eje";
            ejeColumn.DataPropertyName = "Eje";
            ejeColumn.HeaderText = "Eje";
            ejeColumn.Minimum = 0M;
            ejeColumn.Maximum = 180M;
            //ejeColumn.Increment = 1M; 12072025
            ejeColumn.Increment = 5M;
            Dgv_Pnl2_conv.Columns.Add(ejeColumn);



            DataGridViewNumericUpDownColumn adicionColumn = new DataGridViewNumericUpDownColumn();
            adicionColumn.Name = "Adicion";
            adicionColumn.DataPropertyName = "Adicion";
            adicionColumn.HeaderText = "Adición";
            adicionColumn.DecimalPlaces = 2;
            adicionColumn.Minimum = 0M;
            adicionColumn.Maximum = +4.75M;
            adicionColumn.Increment = 0.25M;
            // Formato personalizado para mostrar el signo + en números positivos
            //adicionColumn.DefaultCellStyle.Format = "+0.00;-0.00;0.00";
            Dgv_Pnl2_conv.Columns.Add(adicionColumn);

            DataGridViewNumericUpDownColumn dnpLejosColumn = new DataGridViewNumericUpDownColumn();
            dnpLejosColumn.Name = "Lejos";
            dnpLejosColumn.DataPropertyName = "Lejos";
            dnpLejosColumn.HeaderText = "Lejos";
            dnpLejosColumn.DecimalPlaces = 2;
            dnpLejosColumn.Minimum = 15;
            dnpLejosColumn.Maximum = 45;
            // Formato personalizado para mostrar el signo + en números positivos
            //dnpLejosColumn.DefaultCellStyle.Format = "+0.00;-0.00;0.00";
            Dgv_Pnl2_conv.Columns.Add(dnpLejosColumn);

            DataGridViewNumericUpDownColumn dnpCercaColumn = new DataGridViewNumericUpDownColumn();
            dnpCercaColumn.Name = "Cerca";
            dnpCercaColumn.DataPropertyName = "Cerca";
            dnpCercaColumn.HeaderText = "Cerca";
            dnpCercaColumn.DecimalPlaces = 2;
            dnpCercaColumn.ReadOnly = true; // Hace que la columna no sea editable
            Dgv_Pnl2_conv.Columns.Add(dnpCercaColumn);

            //--------------------------------------------
            DataGridViewTextBoxColumn agudezaColumn = new DataGridViewTextBoxColumn();
            agudezaColumn.Name = "Agudeza";
            agudezaColumn.DataPropertyName = "Agudeza";
            agudezaColumn.HeaderText = " ";
            agudezaColumn.ReadOnly = true; // Hace que la columna no sea editable
            //agudezaColumn.Minimum = 0M;
            //agudezaColumn.Maximum = 400M;
            //agudezaColumn.Increment = 1M;
            Dgv_Pnl2_conv.Columns.Add(agudezaColumn);

            // Asigna el valor "20" a todas las filas existentes en la columna "Agudeza"
            foreach (DataGridViewRow row in Dgv_Pnl2_conv.Rows)
            {
                if (!row.IsNewRow) // Evita la fila para agregar nuevos registros
                {
                    row.Cells["Agudeza"].Value = "20/";
                }
            }

            // Maneja el evento RowsAdded para asignar el valor "20" a las nuevas filas que se agreguen
            Dgv_Pnl2_conv.RowsAdded += (sender, e) =>
            {
                for (int i = 0; i < e.RowCount; i++)
                {
                    int newRowIndex = e.RowIndex + i;
                    if (Dgv_Pnl2_conv.Rows[newRowIndex].Cells["Agudeza"] != null)
                    {
                        Dgv_Pnl2_conv.Rows[newRowIndex].Cells["Agudeza"].Value = "20/";
                    }
                }
            };
            //----------------------------------------------

            DataGridViewNumericUpDownColumn VisualColumn = new DataGridViewNumericUpDownColumn();
            VisualColumn.Name = "Visual";
            VisualColumn.DataPropertyName = "Visual";
            VisualColumn.HeaderText = "Agudeza";
            VisualColumn.DecimalPlaces = 0;
            VisualColumn.Minimum = 20;
            VisualColumn.Maximum = 400;
            VisualColumn.Increment = 1;
            Dgv_Pnl2_conv.Columns.Add(VisualColumn);
            //if (Dgv_Pnl2_conv.Columns.Contains("VisualColumn"))
            //{
            //    Dgv_Pnl2_conv.Columns["VisualColumn"].HeaderText = "Agudeza\nVisual";
            //    Dgv_Pnl2_conv.Columns["VisualColumn"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            //}

            DataGridViewNumericUpDownColumn prisma1Column = new DataGridViewNumericUpDownColumn();
            prisma1Column.Name = "Prisma1";
            prisma1Column.DataPropertyName = "Prisma1";
            prisma1Column.HeaderText = "Prisma";
            prisma1Column.DecimalPlaces = 2;
            prisma1Column.Minimum = 0;
            prisma1Column.Maximum = 12;
            prisma1Column.Increment = 0.25M; //12072025
            Dgv_Pnl2_conv.Columns.Add(prisma1Column);

            DataGridViewNumericUpDownColumn grado1Column = new DataGridViewNumericUpDownColumn();
            grado1Column.Name = "Grado1";
            grado1Column.DataPropertyName = "Grado1";
            grado1Column.HeaderText = "Grado";
            grado1Column.DecimalPlaces = 0;
            grado1Column.Minimum = 0;
            grado1Column.Maximum = 270;
            grado1Column.Increment = 90; // Establece el incremento en 90
            Dgv_Pnl2_conv.Columns.Add(grado1Column);

            //DataGridViewNumericUpDownColumn AlturaColumn = new DataGridViewNumericUpDownColumn();
            //AlturaColumn.Name = "Altura";
            //AlturaColumn.DataPropertyName = "Altura";
            //AlturaColumn.HeaderText = "Altura";
            //AlturaColumn.Minimum = 10;
            //AlturaColumn.Maximum = 35;
            //// Formato personalizado para mostrar siempre 3 dígitos
            //AlturaColumn.DefaultCellStyle.Format = "000";
            //Dgv_Pnl2_conv.Columns.Add(AlturaColumn);


            // Now, add your custom DataGridViewComboBoxColumn for "Vision"
            //DataGridViewComboBoxColumn visionComboColumn = new DataGridViewComboBoxColumn();
            //visionComboColumn.Name = "Vision"; // Give it a distinct name for the DataGridView column
            //visionComboColumn.DataPropertyName = "Vision"; // This must match the DataTable column name
            //visionComboColumn.HeaderText = "Visión";
            //visionComboColumn.Items.AddRange(new object[] { "Cerca", "Lejos", "Bifocal", "Progresivo", "Balance", "Intermedia" });
            //visionComboColumn.ValueType = typeof(string);
            //Dgv_Pnl2_conv.Columns.Add(visionComboColumn);

            //// 2. Crear una nueva DataGridViewComboBoxColumn
            //DataGridViewComboBoxColumn visionComboColumn = new DataGridViewComboBoxColumn();
            //visionComboColumn.Name = "Vision";
            //visionComboColumn.DataPropertyName = "Vision"; // Mantén el mismo DataPropertyName si es apropiado
            //visionComboColumn.HeaderText = "Vision";

            //// 3. Definir los valores que aparecerán en el ComboBox
            //visionComboColumn.Items.AddRange(new object[] { "Cerca", "Lejos", "Bifocal", "Progresivo", "Balance", "Intermedia" });
            //// 4. Opcionalmente, puedes establecer el tipo de dato del valor (si es relevante)
            // visionComboColumn.ValueType = typeof(string); // Ejemplo si los valores son strings

            //// 5. Agregar la nueva columna ComboBox al DataGridView
            //Dgv_Pnl2_conv.Columns.Add(visionComboColumn);







            tamañoExamenGridConv();
        }

        private void Dgv_Pnl2_conv_CellPainting_NoVerticalBorderA1(object sender, DataGridViewCellPaintingEventArgs e)
        {


            if (e.RowIndex >= 0)
            {
                int columnIndexA1 = -1;
                if (Dgv_Pnl2_conv.Columns.Contains("aEsfera"))
                {
                    columnIndexA1 = Dgv_Pnl2_conv.Columns["aEsfera"].Index;
                }

                if (columnIndexA1 != -1)
                {
                    using (Pen gridLinePen = new Pen(Dgv_Pnl2_conv.GridColor))
                    {
                        // Pintar los bordes horizontal (superior e inferior) para todas las celdas
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Right, e.CellBounds.Top);
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);

                        // No pintar el borde derecho de la columna "aEsfera"
                        if (e.ColumnIndex == columnIndexA1)
                        {
                            // No dibujar nada en el borde derecho
                        }
                        // No pintar el borde izquierdo de la columna siguiente a "aEsfera"
                        else if (e.ColumnIndex == columnIndexA1 + 1 && e.ColumnIndex < Dgv_Pnl2_conv.Columns.Count)
                        {
                            // No dibujar nada en el borde izquierdo
                        }
                        // Pintar los bordes verticales para todas las demás columnas
                        else
                        {
                            // Borde izquierdo
                            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Left, e.CellBounds.Bottom);
                            // Borde derecho
                            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);
                        }
                    }

                    e.Paint(e.CellBounds, e.PaintParts & ~DataGridViewPaintParts.Border);
                    e.Handled = true;
                }
            }
        }

        private void ConfigurarDgv_Pnl2_Quera()
        {
            this.Dgv_Pnl2_Querato.DefaultCellStyle.Font = new Font("Century Gothic", 13);
            this.Dgv_Pnl2_Querato.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 8);
            // Crear un DataTable para almacenar los datos del DataGridView
            DataTable dt = new DataTable();

            // Definir las columnas en el DataTable
            //dt.Columns.Add("Meridiano_Corneal", typeof(decimal));
            //dt.Columns.Add("Grados", typeof(decimal));
            //dt.Columns.Add("Meridiano_Corneald", typeof(decimal));
            //dt.Columns.Add("Gradosd", typeof(decimal));

            // Asignar el DataTable como fuente de datos del DataGridView
            Dgv_Pnl2_Querato.DataSource = dt;
            Dgv_Pnl2_Querato.AutoGenerateColumns = false; // Desactivar la generación automática de columnas

            // Opcional: Configurar propiedades del DataGridView para mejor visualización
            Dgv_Pnl2_Querato.AllowUserToAddRows = false;
            Dgv_Pnl2_Querato.AllowUserToDeleteRows = false;
            Dgv_Pnl2_Querato.ReadOnly = false;
            Dgv_Pnl2_Querato.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Dgv_Pnl2_Querato.ColumnHeadersVisible = true;
            Dgv_Pnl2_Querato.RowHeadersVisible = false;
            Dgv_Pnl2_Querato.AllowUserToResizeColumns = false;
            Dgv_Pnl2_Querato.AllowUserToResizeRows = false;



            // Crear y agregar las columnas DataGridView
            DataGridViewNumericUpDownColumn Meridiano_CornealColumn = new DataGridViewNumericUpDownColumn();
            Meridiano_CornealColumn.Name = "QUERATOMD1";
            Meridiano_CornealColumn.DataPropertyName = "QUERATOMD1";
            Meridiano_CornealColumn.HeaderText = "Meridiano Corneal"; //Nombre para el usuario
            Meridiano_CornealColumn.DecimalPlaces = 2;

            // If radioButton5 is true (checked)
            //Meridiano_CornealColumn.Minimum = 6;

            //Meridiano_CornealColumn.Maximum = 56.25M;


            Meridiano_CornealColumn.Increment = 0.25M;
            Dgv_Pnl2_Querato.Columns.Add(Meridiano_CornealColumn);

            DataGridViewNumericUpDownColumn GradosColumn = new DataGridViewNumericUpDownColumn();
            GradosColumn.Name = "QUERATOGD1";
            GradosColumn.DataPropertyName = "QUERATOGD1";
            GradosColumn.HeaderText = "Grados"; //Nombre para el usuario
            GradosColumn.DecimalPlaces = 0;
            GradosColumn.Minimum = 0;
            GradosColumn.Maximum = 180;


            GradosColumn.Increment = 1M;
            Dgv_Pnl2_Querato.Columns.Add(GradosColumn);





            DataGridViewNumericUpDownColumn Meridiano_CornealDColumn = new DataGridViewNumericUpDownColumn();
            Meridiano_CornealDColumn.Name = "QUERATOMD2";
            Meridiano_CornealDColumn.DataPropertyName = "QUERATOMD2";
            Meridiano_CornealDColumn.HeaderText = "Meridiano Corneal";  //Nombre para el usuario
            Meridiano_CornealDColumn.DecimalPlaces = 2;
            // If radioButton5 is true (checked)

            //Meridiano_CornealDColumn.Minimum = 6;

            //Meridiano_CornealDColumn.Maximum = 56.25M;


            Meridiano_CornealDColumn.Increment = 0.25M;
            Dgv_Pnl2_Querato.Columns.Add(Meridiano_CornealDColumn);




            DataGridViewNumericUpDownColumn GradosDColumn = new DataGridViewNumericUpDownColumn();
            GradosDColumn.Name = "QUERATOGD2";
            GradosDColumn.DataPropertyName = "QUERATOGD2";
            GradosDColumn.HeaderText = "Grados"; //Nombre para el usuario
            GradosDColumn.DecimalPlaces = 0;
            GradosDColumn.Minimum = 0;
            GradosDColumn.Maximum = 180;
            GradosDColumn.Increment = 1M;
            Dgv_Pnl2_Querato.Columns.Add(GradosDColumn);
            // Agregar una fila al DataTable para mostrar los valores
            DataRow fila = dt.NewRow();
            dt.Rows.Add(fila);
            DataRow fila1 = dt.NewRow();
            dt.Rows.Add(fila1);
        }

        private void LimpiarCampos2()
        {
            if (Cbx_Tap1_Nacionalidad.Text == "")
            {
                Cbx_Tap1_Nacionalidad.Text = "V";
            }

            //Btn_Tap1_GuardarET.Enabled = true;
            Cbx_Tap1_Estado.SelectedIndex = -1;
            //Cbx_Tap1_Ciudad.SelectedIndex = -1;
            Cbx_Tap1_TLF_Celular.SelectedIndex = -1;
            Cbx_Tap1_TLF_Local.SelectedIndex = -1;
            Txt_Tap1_Nombre.Text = "";
            Dtp_Tap1_Nacimiento.Value = DateTime.Now;

            Rd_Tap1_SexoF.Checked = false;
            Rd_Tap1_SexoM.Checked = false;

            //Txt_Tap1_Ocupacion.Text = "";
            //Cbx_Tap1_EstadoCivil.SelectedIndex = -1;
            //Txt_Tap1_Direccion.Text = "";

            Chex_Tap1_Iva.SetItemChecked(0, false);
            Chex_Tap1_Iva.SetItemChecked(1, false);

            Chex_Tap1_Iva_Pagador.SetItemChecked(0, false);
            Chex_Tap1_Iva_Pagador.SetItemChecked(1, false);
            Txt_Tap1_Edad.Text = ""; // Limpiar el campo de edad también



            Txt_Tap1_Cedula.Enabled = true;
            Txt_Tap1_Cedula_Pagador.Enabled = true;
            Txt_Tap1_Edad.Enabled = true;
            Txt_Tap1_Email.Enabled = true;
            Dtp_Tap1_Nacimiento.Enabled = true;
            Txt_Tap1_Nombre.Enabled = true;
            Txt_Tap1_Nombre_Pagador.Enabled = true;
            Txt_Tap1_TLF_Celular.Enabled = true;
            Txt_Tap1_TLF_Local.Enabled = true;

            //Txt_Tap1_Facebook.Enabled = true;
            //Txt_Tap1_Instagram.Enabled = true;
            //Txt_Tap1_Twitter.Enabled = true;


            // Habilitar los controles
            Cbx_Tap1_Ciudad.Enabled = true;
            Cbx_Tap1_Estado.Enabled = true;
            Cbx_Tap1_Nacionalidad.Enabled = true;
            Cbx_Tap1_Nacionalidad_Pagador.Enabled = true;
            Cbx_Tap1_TLF_Celular.Enabled = true;
            Cbx_Tap1_TLF_Local.Enabled = true;

            ///// &&&&&&&&&&&&&&

            //Txt_Tap1_Direccion_fact.Text = "";
            Txt_Tap1_Cedula_Pagador.Text = "";
            Txt_Tap1_Edad.Text = "";
            Txt_Tap1_Email.Text = "";
            Dtp_Tap1_Nacimiento.Text = "";
            Txt_Tap1_Nombre.Text = "";
            Txt_Tap1_Nombre_Pagador.Text = "";
            Txt_Tap1_TLF_Celular.Text = "";
            Txt_Tap1_TLF_Local.Text = "";

            //Txt_Tap1_Facebook.Text = "";
            //Txt_Tap1_Instagram.Text = "";
            //Txt_Tap1_Twitter.Text = "";


            Cbx_Tap1_Ciudad.Text = "";
            Cbx_Tap1_Estado.Text = "";
            Cbx_Tap1_Nacionalidad_Pagador.Text = "";
            Cbx_Tap1_TLF_Celular.Text = "";
            Cbx_Tap1_TLF_Local.Text = "";

            limpearExamen();

        }

        private void limpearExamen()
        {

            grp_pln2_Cont1.Visible = true;
            grp_pln2_Cont1.BringToFront();
            AsignarCeroDgv_Pnl2_cont();
            AsignarCeroDgv_Pnl2_conv();
            //AsignarCeroDgv_Pnl2_medconv();
            AsignarCeroDgv_Pnl2_Querato();

            this.Txt_Pnl2_Examen.Text = "0";
            Txt_Tap2_Examen.Text = "0";
            //TopeExamen = 0;
            CargarExamenConv();
            CargarExamenCont();
            //CargarDgvPnl2MedConv();
            CargarFicconvOFT();
            CargarFicconvOFT();
            CargarDgv_Pnl2_Querato();

            AsignarCeroSiVacioDgv_Pnl2_cont();
            AsignarCeroSiVacioDgv_Pnl2_conv();
            //AsignarCeroSiVacioDgv_Pnl2_medconv();
            AsignarCeroSiVacioDgv_Pnl2_Querato();


            txt_Pnl2_cont_observa.Clear();
            txt_Pnl2_observa.Clear();
            txt_Pnl2_conv_mimesys.Clear();
            txt_Pnl2_retd.Clear();
            txt_Pnl2_reti.Clear();
            txt_Pnl2_oftd.Clear();
            txt_Pnl2_ofti.Clear();

            //Cbx_Tap2_Tipo_Examen.SelectedItem = 0;
            grp_pln2_Conv2.Visible = true;
            Cbx_Tap2_Tipo_Optome.SelectedItem = -1;
            Cbx_Tap2_Ojo.SelectedItem = -1;

            Cbx_Tap2_Tipo_Optome.SelectedIndex = -1; // Deselecciona el elemento seleccionado
            Cbx_Tap2_Nombre_Optome.SelectedIndex = -1; // Deselecciona el elemento seleccionado
            TXT_Tap2_Nombre_Optome.Text = ""; // Establece el texto en vacío
            TXT_Tap2_Nombre_Optome.Enabled = false;

        }
        private void LimpiarCamposTodos()
        {
            
            
            LimpiarCampos2();
            Txt_Pnl2_Cedula.Text = ""; // Ajusta el nombre de la columna
            Txt_Pnl_2_Nombre.Text = ""; // Ajusta el nombre de la columna

            Txt_Tap1_Cedula.Text = "";
            //Cbx_Tap1_Nacionalidad.SelectedIndex = -1; // Deselecciona el elemento
            Cbx_Tap1_Nacionalidad.Focus();
            DgvClientes.DataSource = null;
            DgvClientes.Rows.Clear(); // Ahora sí puedes limpiar
            //
            dvgClientePagador.DataSource = null;
            dvgClientePagador.Rows.Clear();
            // Ocultamos todos los GroupBox al principio
            //groupBox1.Visible = false;
            //grp_pln2_Exam1.Visible = false;
            //groupBox3.Visible = false;
            //groupBox4.Visible = false;
            //groupBox5.Visible = false;

            Txt_Tap2_Examen.Text = "0";
            dtCliente = null;
            TopeExamen = 0;


        }

        private void tamañoExamenGridConv()

        {
            // Opcional: Configurar propiedades del DataGridView
            Dgv_Pnl2_conv.AllowUserToAddRows = false;
            Dgv_Pnl2_conv.AllowUserToDeleteRows = false;
            Dgv_Pnl2_conv.ReadOnly = false;
            Dgv_Pnl2_conv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Dgv_Pnl2_conv.ColumnHeadersVisible = true;
            Dgv_Pnl2_conv.RowHeadersVisible = false;
            Dgv_Pnl2_conv.AllowUserToResizeColumns = false;
            Dgv_Pnl2_conv.AllowUserToResizeRows = false;
            // === Altura fija de fila para que no se corte la "q" ===
            Dgv_Pnl2_conv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            Dgv_Pnl2_conv.RowTemplate.Height = TextRenderer.MeasureText("Izquierdo", Dgv_Pnl2_conv.DefaultCellStyle.Font).Height + 5;

            foreach (DataGridViewRow row in Dgv_Pnl2_conv.Rows)
            {
                row.Height = Dgv_Pnl2_conv.RowTemplate.Height;
            }

            Dgv_Pnl2_conv.Columns[0].ReadOnly = true; // Hace que la columna no sea editable

            // Configurar la columna "aEsfera" para que no se ajuste automáticamente
            if (Dgv_Pnl2_conv.Columns.Contains("aEsfera"))
            {
                Dgv_Pnl2_conv.Columns["aEsfera"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                Dgv_Pnl2_conv.Columns["aEsfera"].Width = 20; // Establecer el ancho fijo (aproximadamente 0.5 cm)
                //Dgv_Pnl2_conv.Columns["aEsfera"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (Dgv_Pnl2_conv.Columns.Contains("Agudeza"))
            {
                Dgv_Pnl2_conv.Columns["Agudeza"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                Dgv_Pnl2_conv.Columns["Agudeza"].Width = 40; // Establecer el ancho fijo (aproximadamente 0.5 cm)
            }

            if (Dgv_Pnl2_conv.Columns.Contains("Visual"))
            {
                Dgv_Pnl2_conv.Columns["Visual"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                Dgv_Pnl2_conv.Columns["Visual"].Width = 80; // Establecer el ancho fijo (aproximadamente 0.5 cm)
            }


            //if (Dgv_Pnl2_conv.Columns.Contains("Altura"))
            //{
            //    Dgv_Pnl2_conv.Columns["Altura"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //    Dgv_Pnl2_conv.Columns["Altura"].Width = 60; // Establecer el ancho fijo (aproximadamente 0.5 cm)
            //}


            //if (Dgv_Pnl2_conv.Columns.Contains("Vision"))
            //{
            //    Dgv_Pnl2_conv.Columns["Vision"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //    Dgv_Pnl2_conv.Columns["Vision"].Width = 125; // Establecer el ancho fijo (aproximadamente 0.5 cm)
            //}


            if (Dgv_Pnl2_conv.Columns.Contains("Esfera"))
            {
                Dgv_Pnl2_conv.Columns["Esfera"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                Dgv_Pnl2_conv.Columns["Esfera"].Width = 80; // Establecer el ancho fijo (aproximadamente 0.5 cm)
            }
            if (Dgv_Pnl2_conv.Columns.Contains("Eje"))
            {
                Dgv_Pnl2_conv.Columns["Eje"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                Dgv_Pnl2_conv.Columns["Eje"].Width = 80; // Establecer el ancho fijo (aproximadamente 0.5 cm)
            }

            if (Dgv_Pnl2_conv.Columns.Contains("Grado1"))
            {
                Dgv_Pnl2_conv.Columns["Grado1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                Dgv_Pnl2_conv.Columns["Grado1"].Width = 80; // Establecer el ancho fijo (aproximadamente 0.5 cm)
            }
            if (Dgv_Pnl2_conv.Columns.Contains("Cilindro"))
            {
                Dgv_Pnl2_conv.Columns["Cilindro"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                Dgv_Pnl2_conv.Columns["Cilindro"].Width = 80; // Establecer el ancho fijo (aproximadamente 0.5 cm)
            }
            if (Dgv_Pnl2_conv.Columns.Contains("Adicion"))
            {
                Dgv_Pnl2_conv.Columns["Adicion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                Dgv_Pnl2_conv.Columns["Adicion"].Width = 80; // Establecer el ancho fijo (aproximadamente 0.5 cm)
            }


            if (Dgv_Pnl2_conv.Columns.Contains("Lejos"))
            {
                Dgv_Pnl2_conv.Columns["Lejos"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                Dgv_Pnl2_conv.Columns["Lejos"].Width = 80; // Establecer el ancho fijo (aproximadamente 0.5 cm)
            }

            if (Dgv_Pnl2_conv.Columns.Contains("Cerca"))
            {
                Dgv_Pnl2_conv.Columns["Cerca"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                Dgv_Pnl2_conv.Columns["Cerca"].Width = 80; // Establecer el ancho fijo (aproximadamente 0.5 cm)
            }

            if (Dgv_Pnl2_conv.Columns.Contains("Prisma1"))
            {
                Dgv_Pnl2_conv.Columns["Prisma1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                Dgv_Pnl2_conv.Columns["Prisma1"].Width = 80; // Establecer el ancho fijo (aproximadamente 0.5 cm)
            }


            // Configurar la columna "aEsfera" para que no se ajuste automáticamente
            if (Dgv_Pnl2_conv.Columns.Contains("aCilindro"))
            {
                Dgv_Pnl2_conv.Columns["aCilindro"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                Dgv_Pnl2_conv.Columns["aCilindro"].Width = 20; // Establecer el ancho fijo (aproximadamente 0.5 cm)
            }

            // Configurar la columna "aEsfera" para que no se ajuste automáticamente
            //if (Dgv_Pnl2_conv.Columns.Contains("aEsfera"))
            //{
            //    Dgv_Pnl2_conv.Columns["aEsfera"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //    Dgv_Pnl2_conv.Columns["aEsfera"].Width = 20; // Establecer el ancho fijo (aproximadamente 0.5 cm)
            //}
            //--------------------------------------------------------------------------------------------
            // Suponiendo que Dgv_Pnl2_conv es tu control DataGridView


        }

        private void AsignarCeroDgv_Pnl2_cont()
        {
            foreach (DataGridViewRow row in Dgv_Pnl2_cont.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    // Verifica si el índice de la columna no es 0 y el nombre de la columna no es "aEsfera" ni "aCilindro"
                    if (cell.ColumnIndex != 0 && Dgv_Pnl2_cont.Columns[cell.ColumnIndex].Name != "aEsfera" && Dgv_Pnl2_cont.Columns[cell.ColumnIndex].Name != "aCilindro")
                    {
                        cell.Value = 0; // Asigna 0 a la celda
                    }
                    else if (cell.ColumnIndex != 0)
                    {
                        cell.Value = "";
                    }
                    // Si el índice de la columna es 0, no se modifica el valor de la celda
                }
            }
        }

        //--------------------------------------------------
        private void AsignarCeroDgv_Pnl2_Querato()
        {
            foreach (DataGridViewRow row in Dgv_Pnl2_Querato.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {

                    cell.Value = 0; // O "0" si la columna espera un string

                }
            }
        }

        private void AsignarCeroDgv_Pnl2_conv()
        {
            foreach (DataGridViewRow row in Dgv_Pnl2_conv.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (Dgv_Pnl2_conv.Columns[cell.ColumnIndex].Name == "aEsfera" || Dgv_Pnl2_conv.Columns[cell.ColumnIndex].Name == "aCilindro")
                    {
                        cell.Value = "";
                    }
                    else if (Dgv_Pnl2_conv.Columns[cell.ColumnIndex].Name == "Vision")
                    {
                        //cell.Value = -1;
                    }
                    else if (cell.ColumnIndex != 9 && cell.ColumnIndex != 0)
                    {
                        cell.Value = "0";
                    }
                }
            }
        }

        //private void AsignarCeroDgv_Pnl2_medconv()
        //{
        //    foreach (DataGridViewRow row in Dgv_Pnl2_Querato.Rows)
        //    {
        //        foreach (DataGridViewCell cell in row.Cells)
        //        {

        //            cell.Value = 0; // O "0" si la columna espera un string

        //        }
        //    }
        //}

        private void CargarFicconvOFT()
        {
            if (mantenervacio == false)
            {
                // Obtener los valores de los controles de la interfaz de usuario
                string cedula = Txt_Tap1_Cedula.Text;
                string nacionalidad = Cbx_Tap1_Nacionalidad.SelectedItem?.ToString();
                int idExamen;

                if (!int.TryParse(Txt_Tap2_Examen.Text, out idExamen))
                {
                    return;
                }

                // Verificar que los valores requeridos estén presentes
                if (string.IsNullOrEmpty(nacionalidad) || string.IsNullOrEmpty(cedula))
                {
                    return;
                }

                try //Es buena practica usar try catch
                {
                    // Obtener los datos del examen usando el método que creaste
                    D_Ficconv dFicconv = new D_Ficconv();

                    // Verificar si dFicconv es nulo.
                    if (dFicconv != null)
                    {
                        TB_FICCONVCTE Ficconv = dFicconv.ObtenerFicConv(nacionalidad, cedula, idExamen); // Aquí se corrigió el orden de los parámetros y se agregó idExamen

                        if (Ficconv != null)
                        {
                            txt_Pnl2_ofti.Text = Ficconv.OFTI != null ? Ficconv.OFTI : string.Empty;
                            txt_Pnl2_oftd.Text = Ficconv.OFTD != null ? Ficconv.OFTD : string.Empty;

                            txt_Pnl2_reti.Text = Ficconv.RETI != null ? Ficconv.RETI : string.Empty;
                            txt_Pnl2_retd.Text = Ficconv.RETD != null ? Ficconv.RETD : string.Empty;
                            //txt_Pnl2_oft_mimesys.Text = Ficconv.CodigoMimesys != null ? Ficconv.CodigoMimesys : string.Empty;
                        }
                        else
                        {
                            MostrarMensajeTemporal("No se encontró ningún examen con la nacionalidad, cédula e ID de examen proporcionados.", 9000);

                        }
                    }
                    else
                    {
                        MostrarMensajeTemporal("Error: No se pudo instanciar la clase D_Ficconv.", 9000);

                    }


                }
            
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }


        private void CargarExamenConv()
        {
            if (mantenervacio == false)
            {
                this.Dgv_Pnl2_conv.DefaultCellStyle.Font = new Font("Century Gothic", 13);

                // Obtener los valores de los controles de la interfaz de usuario
                string cedula = Txt_Tap1_Cedula.Text;
                string nacionalidad = Cbx_Tap1_Nacionalidad.SelectedItem?.ToString();
                int idExamen;

                if (!int.TryParse(Txt_Tap2_Examen.Text, out idExamen))
                {
                    return;
                }

                // Verificar que los valores requeridos estén presentes
                if (string.IsNullOrEmpty(nacionalidad) || string.IsNullOrEmpty(cedula))
                {
                    return;
                }

                // Crear un DataTable para almacenar los datos del DataGridView 
                DataTable dt = new DataTable();
                dt.Columns.Add("Ojo", typeof(string));
                dt.Columns.Add("aEsfera", typeof(string));
                dt.Columns.Add("Esfera", typeof(decimal));
                dt.Columns.Add("aCilindro", typeof(string));
                dt.Columns.Add("Cilindro", typeof(decimal));
                dt.Columns.Add("Eje", typeof(int));

                dt.Columns.Add("Adicion", typeof(decimal));
                dt.Columns.Add("Lejos", typeof(decimal));
                dt.Columns.Add("Cerca", typeof(decimal));
                dt.Columns.Add("Agudeza", typeof(string));
                dt.Columns.Add("Visual", typeof(string));
                dt.Columns.Add("Prisma1", typeof(decimal));
                dt.Columns.Add("Grado1", typeof(decimal));

                //dt.Columns.Add("Altura", typeof(decimal));
                //dt.Columns.Add("Vision", typeof(string));

                // Agregar las dos filas fijas
                DataRow filaDerecha = dt.NewRow();
                filaDerecha["Ojo"] = "Derecho";
                dt.Rows.Add(filaDerecha);

                DataRow filaIzquierda = dt.NewRow();
                filaIzquierda["Ojo"] = "Izquierdo";
                dt.Rows.Add(filaIzquierda);

                //AVD, AVI en TB_FICCONV
                // Obtener los datos del examen usando el método que creaste
                D_Ficconv dFicconv = new D_Ficconv();
                // ***CORRECCIÓN:***
                // Convierte idExamen a string antes de pasarlo al método.
                TB_FICCONVCTE con = dFicconv.ObtenerFicConv(nacionalidad, cedula, idExamen);
                if (con != null) // Verifica si se obtuvo un objeto TB_Ficconv válido
                {
                    dt.Rows[0]["Agudeza"] = "20/";
                    dt.Rows[1]["Agudeza"] = "20/";

                    dt.Rows[0]["Visual"] = con.AVD; // Accede a AVI a través del objeto 'con' (TB_Ficconv)
                    dt.Rows[1]["Visual"] = con.AVI; // Accede a AVD a través del objeto 'con' (TB_Ficconv)
                    //dt.Rows[0]["Altura"] = con.ALTD; // Accede a AVI a través del objeto 'con' (TB_Ficconv)
                    //dt.Rows[1]["Altura"] = con.ALTI; // Accede a AVD a través del objeto 'con' (TB_Ficconv)

                    dt.Rows[0]["Lejos"] = con.DPDL.HasValue ? con.DPDL.Value : 0M;
                    dt.Rows[1]["Lejos"] = con.DPIL.HasValue ? con.DPIL.Value : 0M;

                    dt.Rows[0]["Cerca"] = con.DPDC.HasValue ? con.DPDC.Value : 0M;
                    dt.Rows[1]["Cerca"] = con.DPIC.HasValue ? con.DPIC.Value : 0M;

                    switch (con.PBASED)
                    {
                        case "Arr":
                            dt.Rows[0]["Grado1"] = 90;
                            break;

                        case "Abj":
                            dt.Rows[0]["Grado1"] = 270;
                            break;

                        case "Nas":
                            dt.Rows[0]["Grado1"] = 360;
                            break;

                        case "Tem":
                            dt.Rows[0]["Grado1"] = 180;
                            break;

                        default:
                            dt.Rows[0]["Grado1"] = 0;
                            break;
                    }

                    switch (con.PBASEI)
                    {
                        case "Arr":
                            dt.Rows[1]["Grado1"] = 90;
                            break;

                        case "Abj":
                            dt.Rows[1]["Grado1"] = 270;
                            break;

                        case "Nas":
                            dt.Rows[1]["Grado1"] = 360;
                            break;

                        case "Tem":
                            dt.Rows[1]["Grado1"] = 180;
                            break;

                        default:
                            dt.Rows[1]["Grado1"] = 0;
                            break;
                    }

                    //dt.Rows[0]["Grado1"] = string.IsNullOrWhiteSpace(con.PBASED) ? "0" : con.PBASED;
                    //dt.Rows[1]["Grado1"] = string.IsNullOrWhiteSpace(con.PBASEI) ? "0" : con.PBASEI;

                    dt.Rows[0]["Prisma1"] = con.PRISMAD;
                    dt.Rows[1]["Prisma1"] = con.PRISMAI;

                    txt_Pnl2_retd.Text = con.RETD != null ? con.RETD : string.Empty;
                    txt_Pnl2_reti.Text = con.RETI != null ? con.RETI : string.Empty;
                }





                // Obtener los datos del examen usando el método que creaste
                D_Examen dExamen = new D_Examen();
                // ***CORRECCIÓN:***
                // Convierte idExamen a string antes de pasarlo al método.
                TB_EXAMENCTE examen = dExamen.ObtenerExamenPorNumeroYNacionalidadCedula(idExamen, nacionalidad, cedula);

                // Crear una instancia de la capa de lógica (L_Trabajo)
                L_Trabajo lTrabajo = new L_Trabajo();
                // Obtener los datos del trabajo usando el método de la capa lógica
                TB_TRABAJOCTE trabajo = lTrabajo.ObtenerTrabajoPorOrdenServicio(nacionalidad, cedula, idExamen);


                if (examen != null)
                {


                    if (examen.TIPO_Optm != null)
                    {
                        foreach (var item in Cbx_Tap2_Tipo_Optome.Items)
                        {
                            // Asumiendo que los items en el ComboBox son strings.
                            // Si son objetos, necesitarás acceder a la propiedad correcta para comparar.
                            if (item != null && item.ToString() == examen.TIPO_Optm.ToString())
                            {
                                Cbx_Tap2_Tipo_Optome.SelectedItem = item;
                                break; // Salir del bucle una vez que se encuentra la coincidencia
                            }
                        }
                        // Si no se encuentra ninguna coincidencia, el ComboBox no tendrá ningún elemento seleccionado.
                    }
                    else
                    {
                        Cbx_Tap2_Tipo_Optome.SelectedIndex = -1; // Deseleccionar cualquier elemento si examen.TIPO_Optm es null
                    }

                    ////-----------------------------

                    //Cbx_Tap2_Nombre_Optome
                    if (examen.TIPO_Optm != null)
                    {
                        if (examen.TIPO_Optm == "02")
                        {
                            Cbx_Tap2_Tipo_Optome.SelectedIndex = 1;
                        }
                        else
                        {
                            Cbx_Tap2_Tipo_Optome.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        Cbx_Tap2_Tipo_Optome.SelectedIndex = -1; // Deseleccionar cualquier elemento si examen.TIPO_Optm es null
                    }
                    ///


                    //Cbx_Tap2_Tipo_Examen.Text = examen.TIPOEXAMEN != null ? examen.TIPOEXAMEN : string.Empty;

                    Cbx_Tap2_Tipo_Examen.SelectedItem = 0;
                    //if (examen.TIPOEXAMEN != null && (Cbx_Tap2_Tipo_Examen.Text == null || Cbx_Tap2_Tipo_Examen.Text == ""))
                    //{
                        Cbx_Tap2_Tipo_Examen.Text = examen.TIPOEXAMEN.ToString().Trim(); // Deseleccionar cualquier elemento si examen.TIPO_Optm es null
                        //foreach (var item in Cbx_Tap2_Tipo_Examen.Items)
                        //{
                        //    // Asumiendo que los items en el ComboBox son strings.
                        //    // Si son objetos, necesitarás acceder a la propiedad correcta para comparar.
                        //    if (item != null && item.ToString() == examen.TIPOEXAMEN.ToString())
                        //    {
                        //        Cbx_Tap2_Tipo_Examen.SelectedItem = item;
                        //        break; // Salir del bucle una vez que se encuentra la coincidencia
                        //    }
                        //}
                        // Si no se encuentra ninguna coincidencia, el ComboBox no tendrá ningún elemento seleccionado.
                    //}

                    TXT_Tap2_Nombre_Optome.Text = examen.NOM_Optm.ToString(); // Deseleccionar cualquier elemento si examen.TIPO_Optm es null



                    //    TXT_Tap2_Nombre_Optome.Text = examen.NOMBRE_CLINICA_OPTM != null ? examen.NOMBRE_CLINICA_OPTM : string.Empty;

                    Dtp_Tap2_FecExam.Text = examen.FEC_Examen.ToString();







                    txt_Pnl2_conv_mimesys.Text = examen.CodigoMimesys != null ? examen.CodigoMimesys : string.Empty;
                    txt_Pnl2_cont_observa.Text = examen.OBSERVACIONES != null ? examen.OBSERVACIONES : string.Empty;
                    txt_Pnl2_observa.Text = examen.OBSERVACIONES != null ? examen.OBSERVACIONES : string.Empty;
                    ////txt_Pnl2_oft_mimesys.Text = examen.CodigoMimesys != null ? examen.CodigoMimesys : string.Empty;

                    // Actualizar las filas del DataTable con los datos del examena
                    if (dt.Rows.Count > 0) // Asegúrate de que haya al menos una fila en el DataTable
                    {
                        if (examen.ESFD > 0)
                        {
                            dt.Rows[0]["aEsfera"] = "+";
                        }
                        else if (examen.ESFD < 0)
                        {
                            dt.Rows[0]["aEsfera"] = "-";
                        }
                        else
                        {
                            if (examen.ESFD < 0)
                            {
                                dt.Rows[0]["aEsfera"] = "-";
                            }
                            else
                            {
                                dt.Rows[0]["aEsfera"] = string.Empty; // O null si prefieres}
                            }
                        }
                    }
                    dt.Rows[0]["Esfera"] = examen.ESFD;

                    if (dt.Rows.Count > 0) // Asegúrate de que haya al menos una fila en el DataTable
                    {
                        if (examen.CILD > 0)
                        {
                            dt.Rows[0]["aCilindro"] = "+";
                        }
                        else if (examen.CILD < 0)
                        {
                            dt.Rows[0]["aCilindro"] = "-";
                        }
                        else
                        {

                            if (examen.CILD < 0)
                            {
                                dt.Rows[0]["aCilindro"] = "-";
                            }
                            else
                            {
                                dt.Rows[0]["aCilindro"] = string.Empty; // O null si prefieres
                            }
                        }
                    }



                    //nuevoFicconv.DPDL = Dgv_Pnl2_conv.Rows[0].Cells["Lejos"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[0].Cells["Lejos"].Value) : 0;
                    //nuevoFicconv.DPIL = Dgv_Pnl2_conv.Rows[1].Cells["Lejos"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[1].Cells["Lejos"].Value) : 0;


                    //nuevoFicconv.DPDC = Dgv_Pnl2_conv.Rows[0].Cells["Cerca"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[0].Cells["Cerca"].Value) : 0;
                    //nuevoFicconv.DPIC = Dgv_Pnl2_conv.Rows[1].Cells["Cerca"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[1].Cells["Cerca"].Value) : 0;


                    dt.Rows[0]["Cilindro"] = examen.CILD;
                    dt.Rows[1]["Cilindro"] = examen.CILI;

                    dt.Rows[0]["Eje"] = examen.EJED;
                    dt.Rows[1]["Eje"] = examen.EJEI;

                    dt.Rows[0]["Adicion"] = examen.ADDD;
                    dt.Rows[1]["Adicion"] = examen.ADDI;

                    //if (trabajo != null)
                    //{
                    //    dt.Rows[0]["Vision"] = (trabajo.TTIPOVISIOND ?? string.Empty).Trim();
                    //    dt.Rows[1]["Vision"] = (trabajo.TTIPOVISIONI ?? string.Empty).Trim();
                    //}



                    // Actualizar las filas del DataTable con los datos del examena
                    if (dt.Rows.Count > 0) // Asegúrate de que haya al menos una fila en el DataTable
                    {
                        if (examen.ESFI > 0)
                        {
                            dt.Rows[1]["aEsfera"] = "+";
                        }
                        else if (examen.ESFI < 0)
                        {
                            dt.Rows[1]["aEsfera"] = "-";
                        }
                        else
                        {

                            if (examen.ESFI < 0)
                            {
                                dt.Rows[1]["aEsfera"] = "-";
                            }
                            else
                            {
                                dt.Rows[1]["aEsfera"] = string.Empty; // O null si prefieres
                            }
                        }
                    }




                    dt.Rows[1]["Esfera"] = examen.ESFI;
                    if (dt.Rows.Count > 0) // Asegúrate de que haya al menos una fila en el DataTable
                    {
                        if (examen.CILI > 0)
                        {
                            dt.Rows[1]["aCilindro"] = "+";
                        }
                        else if (examen.CILI < 0)
                        {
                            dt.Rows[1]["aCilindro"] = "-";
                        }
                        else
                        {
                            if (examen.CILI < 0)
                            {
                                dt.Rows[1]["aCilindro"] = "-";
                            }
                            else
                            {
                                dt.Rows[1]["aCilindro"] = string.Empty; // O null si prefieres
                            }
                        }
                    }


                }
                else
                {

                    MostrarMensajeTemporal("No se encontró ningún examen con la nacionalidad, cédula e ID de examen proporcionados.", 9000); // 5000 ms = 5 segundos

                }








                // Asignar el DataTable como fuente de datos del DataGridView
                Dgv_Pnl2_conv.DataSource = dt;
                Dgv_Pnl2_conv.AutoGenerateColumns = false;

                tamañoExamenGridConv();
                AsignarCeroSiVacioDgv_Pnl2_conv();
                if (Formulario_ListaOrdenes == true && (Dgv_Pnl2_conv != null && Dgv_Pnl2_conv.Rows.Count > 0 || Dgv_Pnl2_cont != null && Dgv_Pnl2_cont.Rows.Count > 0))
                {
                    btnCargarOrden.Enabled = false;
                }
                // Habilitar la pestaña de Carga ordenes 
                else if (Dgv_Pnl2_conv != null && Dgv_Pnl2_conv.Rows.Count > 0 || Dgv_Pnl2_cont != null && Dgv_Pnl2_cont.Rows.Count > 0)
                {
                    btnCargarOrden.Enabled = true;
                }
                else
                {
                    btnCargarOrden.Enabled = false;
                }
            }


        }

        private void CargarExamenCont()
        {
            if (mantenervacio == false)
            {
                this.Dgv_Pnl2_cont.DefaultCellStyle.Font = new Font("Century Gothic", 13);

                // Crear un DataTable para almacenar los datos del DataGridView
                DataTable dt = new DataTable();
                dt.Columns.Add("Ojo", typeof(string));
                dt.Columns.Add("aEsfera", typeof(string));
                dt.Columns.Add("Esfera", typeof(decimal));
                dt.Columns.Add("aCilindro", typeof(string));
                dt.Columns.Add("Cilindro", typeof(decimal));
                dt.Columns.Add("Eje", typeof(decimal));
                dt.Columns.Add("Adicion", typeof(decimal));
                dt.Columns.Add("C_base", typeof(decimal));
                dt.Columns.Add("Diametro", typeof(decimal));

                // Agregar las dos filas fijas iniciales
                DataRow filaDerecha = dt.NewRow();
                filaDerecha["Ojo"] = "Derecho";
                dt.Rows.Add(filaDerecha);

                DataRow filaIzquierda = dt.NewRow();
                filaIzquierda["Ojo"] = "Izquierdo";
                dt.Rows.Add(filaIzquierda);     // Agregar las dos filas fijas iniciales


                /// -----------------------------------------------------------

                // Obtener los valores de los controles de la interfaz de usuario
                string cedula = Txt_Tap1_Cedula.Text;
                string nacionalidad = Cbx_Tap1_Nacionalidad.SelectedItem?.ToString();
                int idExamen;
                string codSucursal = "Sucursal1"; // TODO: Obtener la sucursal desde la interfaz de usuario

                if (!int.TryParse(Txt_Tap2_Examen.Text, out idExamen))
                {
                    //MessageBox.Show("  ingrese un ID de examen válido (numérico).", "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    return;
                }

                // Verificar que los valores requeridos estén presentes
                if (string.IsNullOrEmpty(nacionalidad) || string.IsNullOrEmpty(cedula))
                {
                    //MessageBox.Show("  ingrese la nacionalidad y la cédula.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    return;
                }

                try
                {


                    //AVD, AVI en TB_FICCONV
                    //// Obtener los datos del examen usando el método que creaste
                    //D_Ficconv dFicconv = new D_Ficconv();
                    //// ***CORRECCIÓN:***
                    //// Convierte idExamen a string antes de pasarlo al método.
                    //TB_Ficconv con = dFicconv.ObtenerFicConv(nacionalidad, cedula, idExamen);


                    //// Obtener los datos del examen usando el método de la capa lógica
                    ////L_Ficcont logicaFiccont = new L_Ficcont();
                    //_L_Ficcont.ObtenerFiccontPorClave(nacionalidad, cedula, idExamen);



                    //AVD, AVI en TB_FICCONV
                    // Obtener los datos del examen usando el método que creaste
                    D_FicCont dFiccont = new D_FicCont();
                    // ***CORRECCIÓN:***
                    // Convierte idExamen a string antes de pasarlo al método.
                    TB_FICCONT con = dFiccont.ObtenerFicCont(nacionalidad, cedula, idExamen);


                    if (con != null)
                    {
                        // Actualizar las filas del DataTable con los datos del examen
                        //dt.Rows[0]["Esfera"] = nuevoFiccont.ESFD; valores negativos
                        dt.Rows[0]["Esfera"] = con.ESFD ?? (object)DBNull.Value;
                        dt.Rows[0]["Cilindro"] = con.CILD ?? (object)DBNull.Value;
                        dt.Rows[0]["Eje"] = con.EJED ?? (object)DBNull.Value;
                        dt.Rows[0]["Adicion"] = con.ADDD ?? (object)DBNull.Value;
                        dt.Rows[0]["C_base"] = con.CBD ?? (object)DBNull.Value;
                        dt.Rows[0]["Diametro"] = con.DIAMD ?? (object)DBNull.Value;
                        //----------------------------------------------------
                        // Actualizar las filas del DataTable con los datos del examena
                        if (dt.Rows.Count > 0) // Asegúrate de que haya al menos una fila en el DataTable
                        {
                            if (con.ESFD > 0)
                            {
                                dt.Rows[0]["aEsfera"] = "+";
                            }
                            else if (con.ESFD < 0) // This condition is redundant with the one below and might be a typo. Should it be con.ESFI < 0?
                            {
                                dt.Rows[0]["aEsfera"] = "-";
                            }
                            else if (con.ESFD < 0) // This is a duplicate condition.
                            {
                                dt.Rows[0]["aEsfera"] = "-";
                            }
                            else
                            {
                                dt.Rows[0]["aEsfera"] = string.Empty; // O null si prefieres
                            }
                        }
                        dt.Rows[1]["Esfera"] = con.ESFI ?? (object)DBNull.Value;

                        dt.Rows[1]["Cilindro"] = con.CILI ?? (object)DBNull.Value;
                        dt.Rows[1]["Eje"] = con.EJEI ?? (object)DBNull.Value;
                        dt.Rows[1]["Adicion"] = con.ADDI ?? (object)DBNull.Value;
                        dt.Rows[1]["C_base"] = con.CBI ?? (object)DBNull.Value;
                        dt.Rows[1]["Diametro"] = con.DIAMI ?? (object)DBNull.Value;

                        // Actualizar las filas del DataTable con los datos del examena
                        if (dt.Rows.Count > 0) // Asegúrate de que haya al menos una fila en el DataTable
                        {
                            if (con.ESFI > 0)
                            {
                                dt.Rows[1][1] = "+";
                            }
                            else if (con.ESFI < 0)
                            {
                                dt.Rows[1][1] = "-";
                            }
                            else if (con.ESFI < 0) // This is a duplicate condition.
                            {
                                dt.Rows[1][1] = "-";
                            }
                            else
                            {
                                dt.Rows[1][1] = string.Empty; // O null si prefieres
                            }
                        }









                        if (dt.Rows.Count > 0) // Asegúrate de que haya al menos una fila en el DataTable
                        {
                            if (con.CILD > 0)
                            {
                                dt.Rows[0]["aCilindro"] = "+";
                            }
                            else if (con.CILD < 0)
                            {
                                dt.Rows[0]["aCilindro"] = "-";
                            }
                            else if (con.CILD < 0) // This is a duplicate condition.
                            {
                                dt.Rows[0]["aCilindro"] = "-";
                            }
                            else
                            {
                                dt.Rows[0]["aCilindro"] = string.Empty; // O null si prefieres
                            }
                        }

                        if (dt.Rows.Count > 0) // Asegúrate de que haya al menos una fila en el DataTable
                        {
                            if (con.CILI > 0)
                            {
                                dt.Rows[1]["aCilindro"] = "+";
                            }
                            else if (con.CILI < 0) // This condition seems to refer to CILD, not CILI. Might be a typo.
                            {
                                dt.Rows[1]["aCilindro"] = "-";
                            }
                            else if (con.CILI < 0)
                            {
                                dt.Rows[1]["aCilindro"] = "-";
                            }
                            else
                            {
                                dt.Rows[1]["aCilindro"] = string.Empty; // O null si prefieres
                            }
                        }



                        // Asignar el DataTable como fuente de datos del DataGridView
                        Dgv_Pnl2_cont.DataSource = dt;
                        Dgv_Pnl2_cont.AutoGenerateColumns = false; // Desactivar la generación automática de columnas

                        // Opcional: Configurar propiedades del DataGridView para mejor visualización
                        Dgv_Pnl2_cont.AllowUserToAddRows = false;
                        Dgv_Pnl2_cont.AllowUserToDeleteRows = false;
                        Dgv_Pnl2_cont.ReadOnly = false;
                        Dgv_Pnl2_cont.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        Dgv_Pnl2_cont.ColumnHeadersVisible = true;
                        Dgv_Pnl2_cont.RowHeadersVisible = false;
                        Dgv_Pnl2_cont.AllowUserToResizeColumns = false; // Bloquear el cambio de tamaño de las columnas
                        Dgv_Pnl2_cont.AllowUserToResizeRows = false;    // Bloquear el cambio de tamaño de las filas

                        txt_Pnl2_cont_observa.Text = con?.OBSERVACIONES?.ToString() ?? string.Empty;

                    } // This is the missing closing brace.
                    else
                    {
                        // Actualizar las filas del DataTable con los datos del examen
                        dt.Rows[0]["Esfera"] = 0;
                        dt.Rows[0]["Cilindro"] = 0;
                        dt.Rows[0]["Eje"] = 0;
                        dt.Rows[0]["Adicion"] = 0;
                        dt.Rows[0]["C_base"] = 0;
                        dt.Rows[0]["Diametro"] = 0;

                        dt.Rows[1]["Esfera"] = 0;
                        dt.Rows[1]["Cilindro"] = 0;
                        dt.Rows[1]["Eje"] = 0;
                        dt.Rows[1]["Adicion"] = 0;
                        dt.Rows[1]["C_base"] = 0;
                        dt.Rows[1]["Diametro"] = 0;
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar el examen: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }

                // Asignar el DataTable como fuente de datos del DataGridView
                Dgv_Pnl2_cont.DataSource = dt;
                Dgv_Pnl2_cont.AutoGenerateColumns = false;


                // Habilitar la pestaña de Carga ordenes 
                if (Formulario_ListaOrdenes && (Dgv_Pnl2_conv != null && Dgv_Pnl2_conv.Rows.Count > 0 || Dgv_Pnl2_cont != null && Dgv_Pnl2_cont.Rows.Count > 0))
                {
                    btnCargarOrden.Enabled = false;
                }
                else if (Dgv_Pnl2_conv != null && Dgv_Pnl2_conv.Rows.Count > 0 || Dgv_Pnl2_cont != null && Dgv_Pnl2_cont.Rows.Count > 0)
                {
                    btnCargarOrden.Enabled = true;
                }
                else
                {
                    btnCargarOrden.Enabled = false;
                }


                tamañoExamenGridCont();
                AsignarCeroSiVacioDgv_Pnl2_cont();
            }
        }

        //private void CargarDgvPnl2MedConv()
        //{
        //    if (mantenervacio == false)
        //    {

        //        this.Dgv_Pnl2_medconv.DefaultCellStyle.Font = new Font("Century Gothic", 13);

        //        // Crear un DataTable para almacenar los datos del DataGridView
        //        DataTable dt = new DataTable();
        //        dt.Columns.Add("Ojo", typeof(string));
        //        dt.Columns.Add("T_DISTANCIAVERTICE", typeof(decimal));
        //        dt.Columns.Add("T_ANGULOPANTOSCOPICO", typeof(decimal));
        //        dt.Columns.Add("T_ANGULOFACIAL", typeof(decimal));
        //        dt.Columns.Add("T_DISTANCIADELECTURA", typeof(decimal));

        //        // Agregar la fila al DataTable
        //        DataRow fila = dt.NewRow();
        //        fila["Ojo"] = "Único"; // O "Ambos", dependiendo de la lógica de tu aplicación
        //        dt.Rows.Add(fila);

        //        ////-------------------------------------------------
        //        // Obtener los valores de los controles de la interfaz de usuario
        //        string cedula = Txt_Tap1_Cedula.Text;
        //        string nacionalidad = Cbx_Tap1_Nacionalidad.SelectedItem?.ToString();
        //        int idExamen;

        //        if (!int.TryParse(Txt_Tap2_Examen.Text, out idExamen))
        //        {
        //            //MessageBox.Show("  ingrese un ID de examen válido (numérico).", "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        //            return;
        //        }

        //        // Verificar que los valores requeridos estén presentes
        //        if (string.IsNullOrEmpty(nacionalidad) || string.IsNullOrEmpty(cedula))
        //        {
        //            //MessageBox.Show("  ingrese la nacionalidad y la cédula.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        //            return;
        //        }

        //        try
        //        {

        //            // Crear una instancia de la capa de lógica (L_Trabajo)
        //            L_Trabajo lTrabajo = new L_Trabajo();
        //            // Obtener los datos del trabajo usando el método de la capa lógica
        //            TB_TRABAJOCTE trabajo = lTrabajo.ObtenerTrabajoPorOrdenServicio(nacionalidad, cedula, idExamen);

        //            if (trabajo != null)
        //            {



        //                //Cbx_Tap2_Ojo T_OJO

        //                Cbx_Tap2_Ojo.Text = trabajo.TOJO != null ? trabajo.TOJO.ToString().Trim() : "";



        //                // Llenar la fila del DataTable con los datos obtenidos del trabajo, manejando nulos
        //                if (trabajo == null)
        //                {
        //                    dt.Rows[0]["T_DISTANCIAVERTICE"] = 0;
        //                    dt.Rows[0]["T_ANGULOPANTOSCOPICO"] = 0;
        //                    dt.Rows[0]["T_ANGULOFACIAL"] = 0;
        //                    dt.Rows[0]["T_DISTANCIADELECTURA"] = 0;
        //                }
        //                else
        //                {
        //                    dt.Rows[0]["T_DISTANCIAVERTICE"] = trabajo.TDISTANCIAVERTICE ?? 0;
        //                    dt.Rows[0]["T_ANGULOPANTOSCOPICO"] = trabajo.TANGULOPANTOSCOPICO ?? 0;
        //                    dt.Rows[0]["T_ANGULOFACIAL"] = trabajo.TANGULOFACIAL ?? 0;
        //                    dt.Rows[0]["T_DISTANCIADELECTURA"] = trabajo.TDISTANCIADELECTURA ?? 0;
        //                }




        //                // Asignar el DataTable como fuente de datos del DataGridView
        //                Dgv_Pnl2_medconv.DataSource = dt;
        //                Dgv_Pnl2_medconv.AutoGenerateColumns = false; // Desactivar la generación automática de columnas

        //                // Opcional: Configurar las propiedades del DataGridView para una mejor visualización
        //                Dgv_Pnl2_medconv.AllowUserToAddRows = false;
        //                Dgv_Pnl2_medconv.AllowUserToDeleteRows = false;
        //                Dgv_Pnl2_medconv.ReadOnly = false;
        //                Dgv_Pnl2_medconv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        //                Dgv_Pnl2_medconv.ColumnHeadersVisible = true;
        //                //Dgv_Pnl2_medconv.RowHeadersVisible = false;
        //                Dgv_Pnl2_medconv.AllowUserToResizeColumns = false;
        //                Dgv_Pnl2_medconv.AllowUserToResizeRows = false;


        //                // Dgv_Pnl2_medconv.Refresh(); // No es necesario aquí, se actualiza al asignar el DataSource
        //            }
        //            else
        //            {
        //                MostrarMensajeTemporal("No se encontraron datos de Trabajo para la cédula, nacionalidad e ID de examen proporcionados.", 9000);

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Error al cargar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

        //        }


        //        // Asignar el DataTable como fuente de datos del DataGridView
        //        Dgv_Pnl2_medconv.DataSource = dt;
        //        Dgv_Pnl2_medconv.AutoGenerateColumns = false;

        //        // Opcional: Configurar propiedades del DataGridView
        //        Dgv_Pnl2_medconv.AllowUserToAddRows = false;
        //        Dgv_Pnl2_medconv.AllowUserToDeleteRows = false;
        //        Dgv_Pnl2_medconv.ReadOnly = false;
        //        Dgv_Pnl2_medconv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        //        Dgv_Pnl2_medconv.ColumnHeadersVisible = true;
        //        //Dgv_Pnl2_medconv.RowHeadersVisible = false;
        //        Dgv_Pnl2_medconv.AllowUserToResizeColumns = false;
        //        Dgv_Pnl2_medconv.AllowUserToResizeRows = false;

        //        AsignarCeroSiVacioDgv_Pnl2_medconv();
        //    }
        //}


        //private void AsignarCeroSiVacioDgv_Pnl2_medconv()
        //{
        //    foreach (DataGridViewRow row in Dgv_Pnl2_medconv.Rows)
        //    {
        //        foreach (DataGridViewCell cell in row.Cells)
        //        {
        //            if (cell.Value == null || string.IsNullOrEmpty(cell.Value?.ToString()))
        //            {
        //                cell.Value = 0; // O "0" si la columna espera un string
        //            }
        //        }
        //    }
        //}

        private void CargarDgv_Pnl2_Querato()
        {
            if (mantenervacio == false)
            {

                this.Dgv_Pnl2_Querato.DefaultCellStyle.Font = new Font("Century Gothic", 13);

                // Crear un DataTable para almacenar los datos del DataGridView
                DataTable dt = new DataTable();
                dt.Columns.Add("Ojo", typeof(string));
                dt.Columns.Add("QUERATOMD1", typeof(decimal));
                dt.Columns.Add("QUERATOGD1", typeof(decimal));
                dt.Columns.Add("QUERATOMD2", typeof(decimal));
                dt.Columns.Add("QUERATOGD2", typeof(decimal));

                dt.Columns.Add("QUERATOMI1", typeof(decimal));
                dt.Columns.Add("QUERATOGI1", typeof(decimal));
                dt.Columns.Add("QUERATOMI2", typeof(decimal));
                dt.Columns.Add("QUERATOGI2", typeof(decimal));





                // Agregar la fila al DataTable
                DataRow filaDerecha = dt.NewRow();
                filaDerecha["Ojo"] = "Derecho";
                dt.Rows.Add(filaDerecha);

                DataRow filaIzquierda = dt.NewRow();
                filaIzquierda["Ojo"] = "Izquierdo";
                dt.Rows.Add(filaIzquierda);

                ///-------------------------------------------------------------------------
                // Obtener los valores de los controles de la interfaz de usuario
                string cedula = Txt_Tap1_Cedula.Text;
                string nacionalidad = Cbx_Tap1_Nacionalidad.SelectedItem?.ToString();
                int idExamen;

                if (!int.TryParse(Txt_Tap2_Examen.Text, out idExamen))
                {
                    //MessageBox.Show("  ingrese un ID de examen válido (numérico).", "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    return;
                }

                // Verificar que los valores requeridos estén presentes
                if (string.IsNullOrEmpty(nacionalidad) || string.IsNullOrEmpty(cedula))
                {
                    //MessageBox.Show("  ingrese la nacionalidad y la cédula", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    return;
                }

                try
                {

                    // Crear una instancia de la capa de datos (D_Querato)
                    D_Querato dQuerato = new D_Querato();

                    // Obtener los datos de TB_QUERATO usando el método de la capa de datos
                    TB_QUERATO querato = dQuerato.ObtenerQuerato(nacionalidad, cedula, idExamen);

                    if (querato != null)
                    {
                        //txt_Pnl2_quero_mimesys.Text = querato.CodigoMimesys != null ? querato.CodigoMimesys : string.Empty;
                        //txt_Pnl2_quero_observa.Text = querato.QUE_OBSERV != null ? querato.QUE_OBSERV : string.Empty;

                        //Llenar la fila del DataTable con los datos obtenidos de TB_QUERATO, manejando nulos

                        if (querato.QUERATOMD1 <= 10)
                        {
                            // Si QUERATOMD1 es menor o igual a 10
                            radioButton5.Checked = true;  // Selecciona radioButton5
                            radioButton6.Checked = false; // Deselecciona radioButton6
                        }
                        else
                        {
                            // Si QUERATOMD1 es mayor que 10
                            radioButton5.Checked = false; // Deselecciona radioButton5
                            radioButton6.Checked = true;  // Selecciona radioButton6
                        }


                        dt.Rows[0]["QUERATOMD1"] = querato.QUERATOMD1 ?? 0;
                        dt.Rows[1]["QUERATOMD1"] = querato.QUERATOMI1 ?? 0;

                        dt.Rows[0]["QUERATOGD1"] = querato.QUERATOGD1 ?? 0;
                        dt.Rows[1]["QUERATOGD1"] = querato.QUERATOGI1 ?? 0;

                        dt.Rows[0]["QUERATOMD2"] = querato.QUERATOMD2 ?? 0;
                        dt.Rows[1]["QUERATOMD2"] = querato.QUERATOMI2 ?? 0;

                        dt.Rows[0]["QUERATOGD2"] = querato.QUERATOGD2 ?? 0;
                        dt.Rows[1]["QUERATOGD2"] = querato.QUERATOGI2 ?? 0;


                        txt_Pnl2_obsQuero.Text = querato.QUE_OBSERV;

                        // Asignar el DataTable como fuente de datos del DataGridView
                        Dgv_Pnl2_Querato.DataSource = dt;
                        Dgv_Pnl2_Querato.AutoGenerateColumns = false; // Desactivar la generación automática de columnas

                        // Opcional: Configurar las propiedades del DataGridView para una mejor visualización
                        Dgv_Pnl2_Querato.AllowUserToAddRows = false;
                        Dgv_Pnl2_Querato.AllowUserToDeleteRows = false;
                        Dgv_Pnl2_Querato.ReadOnly = true; // El DataGridView debe ser de solo lectura para mostrar los datos, a menos que el usuario los vaya a editar.
                        Dgv_Pnl2_Querato.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        Dgv_Pnl2_Querato.ColumnHeadersVisible = true;
                        Dgv_Pnl2_Querato.RowHeadersVisible = false;
                        Dgv_Pnl2_Querato.AllowUserToResizeColumns = false;
                        Dgv_Pnl2_Querato.AllowUserToResizeRows = false;
                    }
                    else
                    {
                        MostrarMensajeTemporal("No se encontraron datos de Querato para la cédula, nacionalidad e ID de examen proporcionados.", 9000);

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                }


                txt_Pnl2_obsQuero.Text = nuevoQuerato.QUE_OBSERV != null ? nuevoQuerato.QUE_OBSERV.ToString() : "";
                // Asignar el DataTable como fuente de datos del DataGridView
                Dgv_Pnl2_Querato.DataSource = dt;
                Dgv_Pnl2_Querato.AutoGenerateColumns = false;

                // Opcional: Configurar propiedades del DataGridView
                Dgv_Pnl2_Querato.AllowUserToAddRows = false;
                Dgv_Pnl2_Querato.AllowUserToDeleteRows = false;
                Dgv_Pnl2_Querato.ReadOnly = false;
                Dgv_Pnl2_Querato.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                Dgv_Pnl2_Querato.ColumnHeadersVisible = true;
                Dgv_Pnl2_Querato.RowHeadersVisible = false;
                Dgv_Pnl2_Querato.AllowUserToResizeColumns = false;
                Dgv_Pnl2_Querato.AllowUserToResizeRows = false;

                AsignarCeroSiVacioDgv_Pnl2_Querato();
            }
        }

        private void AsignarCeroSiVacioDgv_Pnl2_cont()
        {
            if (mantenervacio == false)
            {
                foreach (DataGridViewRow row in Dgv_Pnl2_cont.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        // Verifica si la celda está vacía o solo contiene espacios en blanco, y no es de la columna A1 o A0
                        if ((cell.Value == null || string.IsNullOrWhiteSpace(cell.Value?.ToString())))
                        {
                            if (Dgv_Pnl2_cont.Columns[cell.ColumnIndex].Name != "aEsfera" && Dgv_Pnl2_cont.Columns[cell.ColumnIndex].Name != "aCilindro")
                            {
                                cell.Value = 0;
                            }
                            else
                            {
                                cell.Value = "";
                            }
                        }
                    }
                }
            }
        }

        private void AsignarCeroSiVacioDgv_Pnl2_conv()
        {
            if (mantenervacio == false)
            {
                foreach (DataGridViewRow row in Dgv_Pnl2_conv.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        // Verifica si la celda está vacía y no es la columna "Vision"
                        if (cell.Value == null || string.IsNullOrEmpty(cell.Value?.ToString()))
                        {
                            if (Dgv_Pnl2_conv.Columns[cell.ColumnIndex].Name != "aEsfera" && Dgv_Pnl2_conv.Columns[cell.ColumnIndex].Name != "aCilindro" && Dgv_Pnl2_conv.Columns[cell.ColumnIndex].Name != "Vision")
                            {
                                cell.Value = 0; // Asigna 0 a la celda
                            }
                            else if (Dgv_Pnl2_conv.Columns[cell.ColumnIndex].Name == "aEsfera" || Dgv_Pnl2_conv.Columns[cell.ColumnIndex].Name == "aCilindro")
                            {
                                cell.Value = ""; // Asigna "" a la celda si es A1 o A0
                            }
                        }
                        //Si la celda no está vacía, verifica si pertenece a la columna "Vision"
                        //else if (Dgv_Pnl2_conv.Columns[cell.ColumnIndex].Name == "Vision")
                        //{
                        //    // Set the cell's value to an empty string
                        //    Dgv_Pnl2_conv.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Value = string.Empty;
                        //}
                    }
                }
            }
        }

        private void AsignarCeroSiVacioDgv_Pnl2_Querato()
        {
            if (mantenervacio == false)
            {
                    foreach (DataGridViewRow row in Dgv_Pnl2_Querato.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value == null || string.IsNullOrEmpty(cell.Value?.ToString()))
                        {
                            cell.Value = 0; // O "0" si la columna espera un string
                        }
                    }
                }
            }
        }

        private void MostrarMensajeTemporal(string mensaje, int duracion)
        {
            // Asegúrate de que tienes un control para mostrar el mensaje, como una barra de estado (StatusStrip) o un Label.
            // Aquí, se asume que tienes un StatusStrip llamado 'statusStripPrincipal' y un StatusLabel llamado 'statusLabelMensaje'.
            //if (statusStripPrincipal != null && statusLabelMensaje != null)
            //{
            //    statusLabelMensaje.Text = mensaje; // Mostrar el mensaje
            //  //  statusLabelMensaje.Visible = true;

            //    // Crear un temporizador para ocultar el mensaje después de la duración especificada.
            //    Timer timer = new Timer();
            //    timer.Interval = duracion;
            //    timer.Tick += (sender, e) =>
            //    {
            //        statusLabelMensaje.Text = ""; // Limpiar el mensaje
            //        //statusLabelMensaje.Visible = false;
            //        timer.Stop();
            //        timer.Dispose(); // Liberar recursos del temporizador
            //    };
            //    timer.Start(); // Iniciar el temporizador
            //}
            //else
            //{
            //    // Si no tienes los controles necesarios, puedes usar un MessageBox como alternativa (no recomendado para mensajes temporales).
            //    MessageBox.Show(mensaje, "Importante", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            //}
        }

        private void tamañoExamenGridCont()

        {   // Opcional: Configurar propiedades del DataGridView
            Dgv_Pnl2_cont.AllowUserToAddRows = false;
            Dgv_Pnl2_cont.AllowUserToDeleteRows = false;
            Dgv_Pnl2_cont.ReadOnly = false;
            //Dgv_Pnl2_cont.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Dgv_Pnl2_cont.ColumnHeadersVisible = true;
            Dgv_Pnl2_cont.RowHeadersVisible = false;
            Dgv_Pnl2_cont.AllowUserToResizeColumns = false;
            Dgv_Pnl2_cont.AllowUserToResizeRows = false;

            Dgv_Pnl2_cont.Columns[0].ReadOnly = true; // Hace que la columna no sea editable
            Dgv_Pnl2_cont.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            Dgv_Pnl2_cont.RowTemplate.Height = TextRenderer.MeasureText("Izquierdo", Dgv_Pnl2_conv.DefaultCellStyle.Font).Height + 6;

            foreach (DataGridViewRow row in Dgv_Pnl2_conv.Rows)
            {
                row.Height = Dgv_Pnl2_conv.RowTemplate.Height;
            }


            //if (Dgv_Pnl2_cont.Columns.Contains("Agudeza"))
            //{
            //    Dgv_Pnl2_cont.Columns["Agudeza"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //    Dgv_Pnl2_cont.Columns["Agudeza"].Width = 35; // Establecer el ancho fijo (aproximadamente 0.5 cm)
            //}

            //if (Dgv_Pnl2_cont.Columns.Contains("Visual"))
            //{
            //    Dgv_Pnl2_cont.Columns["Visual"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //    Dgv_Pnl2_cont.Columns["Visual"].Width = 70; // Establecer el ancho fijo (aproximadamente 0.5 cm)
            //}
            // Configurar la columna "aEsfera" para que no se ajuste automáticamente
            if (Dgv_Pnl2_cont.Columns.Contains("aCilindro"))
            {
                Dgv_Pnl2_cont.Columns["aCilindro"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                Dgv_Pnl2_cont.Columns["aCilindro"].Width = 40; // Establecer el ancho fijo (aproximadamente 0.5 cm)
                Dgv_Pnl2_cont.Columns["aCilindro"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            // Configurar la columna "aEsfera" para que no se ajuste automáticamente
            if (Dgv_Pnl2_cont.Columns.Contains("aEsfera"))
            {
                Dgv_Pnl2_cont.Columns["aEsfera"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                Dgv_Pnl2_cont.Columns["aEsfera"].Width = 40; // Establecer el ancho fijo (aproximadamente 0.5 cm)
                Dgv_Pnl2_cont.Columns["aEsfera"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (Dgv_Pnl2_cont.Columns.Contains("Esfera"))
            {
                Dgv_Pnl2_cont.Columns["Esfera"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }

            if (Dgv_Pnl2_cont.Columns.Contains("Cilindro"))
            {

                Dgv_Pnl2_cont.Columns["Cilindro"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }
            //if (Dgv_Pnl2_cont.Columns.Contains("Adicion"))
            //{
            //    Dgv_Pnl2_cont.Columns["Adicion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //    Dgv_Pnl2_cont.Columns["Adicion"].Width = 60; // Establecer el ancho fijo (aproximadamente 0.5 cm)
            //}


            //if (Dgv_Pnl2_cont.Columns.Contains("Lejos"))
            //{
            //    Dgv_Pnl2_cont.Columns["Lejos"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //    Dgv_Pnl2_cont.Columns["Lejos"].Width = 60; // Establecer el ancho fijo (aproximadamente 0.5 cm)
            //}

            //if (Dgv_Pnl2_cont.Columns.Contains("Cerca"))
            //{
            //    Dgv_Pnl2_cont.Columns["Cerca"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //    Dgv_Pnl2_cont.Columns["Cerca"].Width = 60; // Establecer el ancho fijo (aproximadamente 0.5 cm)
            //}



        }
        private void txtClienteBuscar_Enter(object sender, EventArgs e)
        {

            txtClienteBuscar_Leave(sender, e);
        }

        private void txtClienteBuscar_MouseLeave(object sender, EventArgs e)
        {
            txtClienteBuscar_Leave(sender, e);
        }

        private void txtClienteBuscar_Leave(object sender, EventArgs e)
        {

            // Obtener el texto del textBox1
            string textoFiltro = txtClienteBuscar.Text.Trim(); // .Trim() para eliminar espacios en blanco al inicio y al final

            // Validar la longitud del texto
            if (textoFiltro.Length < 3 && textoFiltro.Length > 0) // Si tiene entre 1 y 2 caracteres
            {
                // Mostrar un mensaje al usuario
                //  MessageBox.Show("Por favor, ingrese al menos 3 caracteres para realizar la búsqueda.", "Filtro Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtClienteBuscar.Focus(); // Opcional: devolver el foco al TextBox para que el usuario corrija
            }
            else if (textoFiltro.Length == 0) // Si el campo está vacío, puedes decidir si cargar todo o no hacer nada
            {
                // Si el campo está vacío, puedes optar por no hacer nada o cargar todos los datos
                // Por ejemplo, si quieres que al borrar el texto se muestren todos los clientes:
                // CargarDatosDeClientes(textoFiltro, radioButton2, radioButton1);
                // O simplemente no hacer nada si no hay filtro
                // Console.WriteLine("Campo de filtro vacío, no se realiza búsqueda.");
            }
            else // Si la longitud es 3 o más caracteres
            {
                // Llamar al método para cargar los datos de los clientes
                CargarDatosDeClientes(textoFiltro, radioButton2, radioButton1);
            }
        }

        private void txtClienteBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            try 
            { 
            // Verifica si la tecla presionada es la tecla Enter
            if (e.KeyCode == Keys.Enter)
            {
                // Opcional: Prevenir que el sonido de "ding" del sistema se reproduzca
                // cuando se presiona Enter en un TextBox multilínea.
                // Para un TextBox de una sola línea, esto no suele ser necesario.
                e.SuppressKeyPress = true;

                // Mueve el foco al control DgvClientes
                // Asegúrate de que 'DgvClientes' es el nombre correcto de tu DataGridView.
                if (DgvClientes != null) // Es buena práctica verificar que el control no sea nulo
                {
                    DgvClientes.Focus();
                }
                else
                {
                    // Mensaje de depuración si DgvClientes no se encuentra (solo para desarrollo)
                    Console.WriteLine("Error: El control DgvClientes no se encontró o no está inicializado.");
                }
            }
            }
            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
            }
        }

        private void txtClienteBuscar_TextChanged(object sender, EventArgs e)
        {
            txtClienteBuscar_Leave(sender, e);
        }



        private void CargarDatosDeClientesP(string filtro, RadioButton buscarPorCedula, RadioButton buscarPorNombre)
        {
            try
            {

                txtClienteP.Focus();
                _L_Cliente.CargarClientes(dvgClientePagador, listaDeClientes, filtro, buscarPorCedula, buscarPorNombre);



                if (_L_Cliente.stringBuilder.Length > 0)
                {
                    MessageBox.Show(_L_Cliente.stringBuilder.ToString());
                }
                else
                {
                    dvgClientePagador.DataSource = listaDeClientes; // Asignar aquí en la UI
                    listaTemporalClientes = new List<TB_CTEPPAL>(listaDeClientes); // Inicializar la lista temporal


                    // Ocultar todas las columnas inicialmente
                    foreach (DataGridViewColumn columna in dvgClientePagador.Columns)
                    {
                        columna.Visible = false;
                    }

                    // Hacer visibles las columnas con índice 0 y 1 (si existen)
                    if (dvgClientePagador.Columns.Count > 2)
                    {
                        dvgClientePagador.Columns[2].Visible = true;
                        // Centra el texto del encabezado.
                        // Esto se hace accediendo al estilo de celda por defecto de la celda del encabezado.
                        dvgClientePagador.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }

                    if (dvgClientePagador.Columns.Count > 3)
                    {
                        dvgClientePagador.Columns[3].Visible = true;
                        // Centra el texto del encabezado.
                        // Esto se hace accediendo al estilo de celda por defecto de la celda del encabezado.
                        dvgClientePagador.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }



                    dvgClientePagador.AllowUserToAddRows = false;
                    dvgClientePagador.AllowUserToDeleteRows = false;
                    dvgClientePagador.ColumnHeadersVisible = true;
                    dvgClientePagador.RowHeadersVisible = false;
                    dvgClientePagador.AllowUserToResizeColumns = false;
                    dvgClientePagador.AllowUserToResizeRows = false;

                    // Establecer el DataGridView como de solo lectura
                    dvgClientePagador.ReadOnly = true;

                    // Establecer los encabezados de las columnas

                    if (dvgClientePagador.ColumnCount > 3)
                    {
                        dvgClientePagador.Columns[2].HeaderText = "Cédula";
                    }
                    if (dvgClientePagador.ColumnCount > 4)
                    {
                        dvgClientePagador.Columns[3].HeaderText = "Nombre";
                    }

                    //// Quitar la línea vertical entre la columna 0 y la 1
                    //if (DgvClientes.ColumnCount > 1)
                    //{
                    //    foreach (DataGridViewRow row in DgvClientes.Rows)
                    //    {
                    //        row.Cells[0].Style.Border.Right = DataGridViewCellBorderStyles.None;
                    //    }
                    //    // Corrección para acceder a las celdas del encabezado
                    //    if (DgvClientes.ColumnHeadersHeightSizeMode != DataGridViewColumnHeadersHeightSizeMode.DisableResizing && DgvClientes.ColumnHeaders != null && DgvClientes.ColumnHeaders.Cells.Count > 1)
                    //    {
                    //        DgvClientes.ColumnHeaders.Cells[0].Style.Border.Right = DataGridViewCellBorderStyles.None;
                    //    }
                    //}


                    dvgClientePagador.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dvgClientePagador.Columns[2].Width = 100;
                    dvgClientePagador.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dvgClientePagador.Columns[3].Width = 300;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + "\nStackTrace: " + ex.StackTrace);
            }

        }

        private void textBox1_Enter(object sender, EventArgs e)
        {

            textBox1_Leave(sender, e);
        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            textBox1_Leave(sender, e);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {

            // Obtener el texto del textBox1
            string textoFiltro = textBox1.Text.Trim(); // .Trim() para eliminar espacios en blanco al inicio y al final

            // Validar la longitud del texto
            if (textoFiltro.Length < 3 && textoFiltro.Length > 0) // Si tiene entre 1 y 2 caracteres
            {
                // Mostrar un mensaje al usuario
                MessageBox.Show("Por favor, ingrese al menos 3 caracteres para realizar la búsqueda.", "Filtro Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Focus(); // Opcional: devolver el foco al TextBox para que el usuario corrija
            }
            else if (textoFiltro.Length == 0) // Si el campo está vacío, puedes decidir si cargar todo o no hacer nada
            {
                // Si el campo está vacío, puedes optar por no hacer nada o cargar todos los datos
                // Por ejemplo, si quieres que al borrar el texto se muestren todos los clientes:
                // CargarDatosDeClientes(textoFiltro, radioButton2, radioButton1);
                // O simplemente no hacer nada si no hay filtro
                // Console.WriteLine("Campo de filtro vacío, no se realiza búsqueda.");
            }
            else // Si la longitud es 3 o más caracteres
            {
                // Llamar al método para cargar los datos de los clientes
                CargarDatosDeClientes(textoFiltro, radioButton2, radioButton1);
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try { 

            // Verifica si la tecla presionada es la tecla Enter
            if (e.KeyCode == Keys.Enter)
            {
                // Opcional: Prevenir que el sonido de "ding" del sistema se reproduzca
                // cuando se presiona Enter en un TextBox multilínea.
                // Para un TextBox de una sola línea, esto no suele ser necesario.
                e.SuppressKeyPress = true;

                // Mueve el foco al control DgvClientes
                // Asegúrate de que 'DgvClientes' es el nombre correcto de tu DataGridView.
                if (DgvClientes != null) // Es buena práctica verificar que el control no sea nulo
                {
                    DgvClientes.Focus();
                }
                else
                {
                    // Mensaje de depuración si DgvClientes no se encuentra (solo para desarrollo)
                    Console.WriteLine("Error: El control DgvClientes no se encontró o no está inicializado.");
                }
            }
            }
            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
            }
        }

        private void CargarDatosDeClientes(string filtro, RadioButton buscarPorCedula, RadioButton buscarPorNombre)
        {
            try
            {
                txtClienteBuscar.Focus();

                // 1. Cargar datos en una lista temporal
                var listaTemporal = new List<TB_CTEPPAL>();
                _L_Cliente.CargarClientes(DgvClientes, listaTemporal, filtro, buscarPorCedula, buscarPorNombre);

                if (_L_Cliente.stringBuilder.Length > 0)
                {
                    MessageBox.Show(_L_Cliente.stringBuilder.ToString());
                    return;
                }

                // 2. Actualizar la lista principal y el BindingSource
                listaDeClientes = listaTemporal;
                bindingSource.DataSource = new List<TB_CTEPPAL>(listaDeClientes); // Copia para seguridad
                DgvClientes.DataSource = bindingSource;

                // 6. Configuración de columnas optimizada
                ConfigurarColumnasDataGridView();

                // 7. Configuración final del DataGridView
                DgvClientes.AllowUserToAddRows = false;
                DgvClientes.AllowUserToDeleteRows = false;
                DgvClientes.ReadOnly = true;
                DgvClientes.RowHeadersVisible = false;
                DgvClientes.Refresh();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void ConfigurarColumnasDataGridView()
        {
            // Ocultar todas las columnas primero
            foreach (DataGridViewColumn columna in DgvClientes.Columns)
            {
                columna.Visible = false;
            }

            // Mostrar solo las columnas necesarias con configuración específica
            if (DgvClientes.Columns.Count > 2)
            {
                var columnaCedula = DgvClientes.Columns[2];
                columnaCedula.Visible = true;
                columnaCedula.HeaderText = "Cédula";
                columnaCedula.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                columnaCedula.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                columnaCedula.Width = 100;
            }

            if (DgvClientes.Columns.Count > 3)
            {
                var columnaNombre = DgvClientes.Columns[3];
                columnaNombre.Visible = true;
                columnaNombre.HeaderText = "Nombre";
                columnaNombre.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                columnaNombre.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                columnaNombre.Width = 300;
            }
        }

        private void guardacliente()
        {



            // Crear una instancia de la entidad TB_CTEPPAL para almacenar los datos

            TB_CTEPPAL nuevoCliente = new TB_CTEPPAL();

            // Recopilar los datos de los controles del formulario
            nuevoCliente.CTE_CedIden = Txt_Tap1_Cedula.Text.Trim();
            nuevoCliente.CTE_Nacio = Cbx_Tap1_Nacionalidad.Text.Trim(); // Ajusta según cómo manejas la nacionalidad
            nuevoCliente.CTE_PNombre = Txt_Tap1_Nombre.Text.Trim();
            //nuevoCliente.CTE_SNombre = string.IsNullOrEmpty(Txt_Tap1_SegundoNombre.Text) ? null : Txt_Tap1_SegundoNombre.Text.Trim(); // Asume que tienes un Txt_Tap1_SegundoNombre
            //nuevoCliente.CTE_PApellido = Txt_Tap1_Apellido.Text.Trim(); // Asume que tienes un Txt_Tap1_Apellido
            //nuevoCliente.CTE_SApellido = string.IsNullOrEmpty(Txt_Tap1_SegundoApellido.Text) ? null : Txt_Tap1_SegundoApellido.Text.Trim(); // Asume que tienes un Txt_Tap1_SegundoApellido
            nuevoCliente.CTE_FNac = Dtp_Tap1_Nacimiento.Value.Date;
            nuevoCliente.CTE_FecAfil = DateTime.Now.Date; // Asigna la fecha de afiliación actual
            nuevoCliente.COD_STCTE = "ACT"; // Asigna un estado por defecto (ajusta según tu lógica)

            // Obtener el sexo del CheckListBox ahora es radiobuton
            if (Rd_Tap1_SexoF.Checked && !Rd_Tap1_SexoM.Checked)
            {
                nuevoCliente.CTE_Sex = "F"; // Femenino
            }
            else if (!Rd_Tap1_SexoF.Checked && Rd_Tap1_SexoM.Checked)
            {
                nuevoCliente.CTE_Sex = "M"; // Masculino
            }
            else
            {
                nuevoCliente.CTE_Sex = null; // O maneja un estado no especificado (ninguno seleccionado o ambos, aunque esto último no debería ocurrir en un grupo de RadioButton bien configurado)
            }

            //nuevoCliente.CTE_CodOcup = string.IsNullOrEmpty(Txt_Tap1_Ocupacion.Text) ? null : Txt_Tap1_Ocupacion.Text.Trim(); // Asume que tienes un Txt_Tap1_Ocupacion
            //nuevoCliente.CTE_EdoCiv = Cbx_Tap1_EstadoCivil.Text.Trim(); // Asume que tienes un Cbx_Tap1_EstadoCivil
            nuevoCliente.COD_Sucursal = codigoSucursal; // Asigna una sucursal por defecto (ajusta según tu lógica)
            nuevoCliente.CTE_FecCreacion = DateTime.Now;
            nuevoCliente.USER_CREA = TB_USUARIO.COD_USR;  // Reemplaza con el usuario actual del sistema

            //nuevoCliente.Direccion_fact = string.IsNullOrEmpty(Txt_Tap1_Direccion_fact.Text) ? null : Txt_Tap1_Direccion_fact.Text.Trim(); // Asume que tienes un Txt_Tap1_Direccion
            //nuevoCliente.Facebook = string.IsNullOrEmpty(Txt_Tap1_Facebook.Text) ? null : Txt_Tap1_Facebook.Text.Trim(); // Asume que tienes un Txt_Tap1_Facebook
            //nuevoCliente.Twitter = string.IsNullOrEmpty(Txt_Tap1_Twitter.Text) ? null : Txt_Tap1_Twitter.Text.Trim(); // Asume que tienes un Txt_Tap1_Twitter
            //nuevoCliente.Instagram = string.IsNullOrEmpty(Txt_Tap1_Instagram.Text) ? null : Txt_Tap1_Instagram.Text.Trim(); // Asume que tienes un Txt_Tap1_Instagram

            // Obtener los valores de los CheckBoxes de retención
            nuevoCliente.CTE_RETISLR = Chex_Tap1_Iva.GetItemChecked(0); // Asume que ISR está en el índice 0
            nuevoCliente.CTE_RETIVA = Chex_Tap1_Iva.GetItemChecked(1); // Asume que IVA está en el índice 1

            nuevoCliente.COD_Edo = Cbx_Tap1_Estado.SelectedValue != null ? Cbx_Tap1_Estado.SelectedValue.ToString() : null;

            nuevoCliente.COD_Ciud = Cbx_Tap1_Ciudad.SelectedValue != null ? Cbx_Tap1_Ciudad.SelectedValue.ToString() : null;

            //   nuevoCliente.CTE_CedIdenP = Txt_Tap1_Cedula_Pagador.Text.Trim();
            // nuevoCliente.CTE_NacioP = Cbx_Tap1_Nacionalidad_Pagador.Text.Trim(); // Ajusta según cómo manejas la nacionalidad

            // Llamar al método de la capa lógica para guardar el cliente
            resultado = _L_Cliente.GuardarCliente(nuevoCliente);

            // Procesar el resultado de la operación de guardado
            if (resultado.Equals("Guardado"))
            {

                if (!validaVaciocorreemail())
                { return; }

                // --- Proceso para Insertar Teléfono ---
                TB_CTETLF nuevoTelefono = new TB_CTETLF();
                CapaLogica.CargarClientes_Logica.L_Cliente logicaClienteTelefono = new CapaLogica.CargarClientes_Logica.L_Cliente();

                if (!string.IsNullOrEmpty(this.Txt_Tap1_TLF_Celular.Text))
                {
                    // Asigna los valores desde tus controles del formulario
                    nuevoTelefono.CTE_Nacio = this.Cbx_Tap1_Nacionalidad.Text; // Valor del control para la nacionalidad (ej: textBoxNacionalidadTelefono.Text);
                    nuevoTelefono.CTE_CedIden = this.Txt_Tap1_Cedula.Text; // Valor de la cédula del cliente (debes tenerla disponible, ej: textBoxCedulaCliente.Text);
                    nuevoTelefono.TLF_Tipo = "002"; // Valor del control para el tipo de teléfono (ej: comboBoxTipoTelefono.SelectedItem.ToString());
                    nuevoTelefono.TLF_Cod = this.Cbx_Tap1_TLF_Celular.Text;// Valor del control para el código del teléfono (ej: textBoxCodigoTelefono.Text);
                    nuevoTelefono.TLF_Numero = this.Txt_Tap1_TLF_Celular.Text; // Valor del control para el número de teléfono (ej: textBoxNumeroTelefono.Text);
                    nuevoTelefono.TLF_Ext = "";// Valor del control para la extensión (ej: textBoxExtensionTelefono.Text);
                                               //nuevoTelefono.TLF_FecCrea = DateTime.Now; // Establecer la fecha de creación (tipo DateTime)nuevoTelefono.TLF_FecCrea = DateTime.Now; // Establecer la fecha de creación (tipo DateTime)
                    nuevoTelefono.USER_Crea = TB_USUARIO.COD_USR; // Usuario que está creando el registro (debes tenerlo disponible, ej: UsuarioLogueado.NombreUsuario);

                    // Crea una instancia de la capa lógica para teléfonos
                    resultadoTelefono = logicaClienteTelefono.InsertarTelefono(nuevoTelefono);

                    // Maneja el resultado de la inserción del teléfono
                    if (resultadoTelefono == "Teléfono guardado")
                    {
                        //MessageBox.Show("Teléfono guardado exitosamente.", "Importante", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        // Puedes realizar acciones adicionales después de guardar el teléfono
                    }
                    else
                    {
                        // MessageBox.Show($"Error al guardar el teléfono: {resultadoTelefono}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        // Puedes registrar el error o informar al usuario de otra manera
                    }
                }




                if (!string.IsNullOrEmpty(this.Txt_Tap1_TLF_Local.Text))
                {
                    // Asigna los valores desde tus controles del formulario
                    nuevoTelefono.CTE_Nacio = this.Cbx_Tap1_Nacionalidad.Text; // Valor del control para la nacionalidad (ej: textBoxNacionalidadTelefono.Text);
                    nuevoTelefono.CTE_CedIden = this.Txt_Tap1_Cedula.Text; // Valor de la cédula del cliente (debes tenerla disponible, ej: textBoxCedulaCliente.Text);
                    nuevoTelefono.TLF_Tipo = "003"; // Valor del control para el tipo de teléfono (ej: comboBoxTipoTelefono.SelectedItem.ToString());
                    nuevoTelefono.TLF_Cod = this.Cbx_Tap1_TLF_Local.Text;// Valor del control para el código del teléfono (ej: textBoxCodigoTelefono.Text);
                    nuevoTelefono.TLF_Numero = this.Txt_Tap1_TLF_Local.Text; // Valor del control para el número de teléfono (ej: textBoxNumeroTelefono.Text);
                                                                             //nuevoTelefono.TLF_Ext = Txt_Tap1_ext_Local.Text;// Valor del control para la extensión (ej: textBoxExtensionTelefono.Text);
                                                                             //nuevoTelefono.TLF_FecCrea = DateTime.Now; // Establecer la fecha de creación (tipo DateTime)nuevoTelefono.TLF_FecCrea = DateTime.Now; // Establecer la fecha de creación (tipo DateTime)
                    nuevoTelefono.USER_Crea = TB_USUARIO.COD_USR; // Usuario que está creando el registro (debes tenerlo disponible, ej: UsuarioLogueado.NombreUsuario);

                    // Crea una instancia de la capa lógica para teléfonos
                    //CapaLogica.CargarClientes_Logica.L_Cliente logicaClienteTelefono = new CapaLogica.CargarClientes_Logica.L_Cliente();
                    resultadoTelefono = logicaClienteTelefono.InsertarTelefono(nuevoTelefono);

                    // Maneja el resultado de la inserción del teléfono
                    if (resultadoTelefono == "Teléfono guardado")
                    {
                        //MessageBox.Show("Teléfono guardado exitosamente.", "Importante", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        // Puedes realizar acciones adicionales después de guardar el teléfono
                    }
                    else
                    {
                        //MessageBox.Show($"Error al guardar el teléfono: {resultadoTelefono}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        // Puedes registrar el error o informar al usuario de otra manera
                    }
                }

                if (!string.IsNullOrEmpty(this.Txt_Tap1_Email.Text))
                {
                    // --- Proceso para Insertar Email ---
                    TB_CTEMAIL nuevoEmail = new TB_CTEMAIL();

                    // Asigna los valores desde tus controles del formulario
                    nuevoEmail.CTE_Nacio = this.Cbx_Tap1_Nacionalidad.Text;  // Valor del control para la nacionalidad (ej: textBoxNacionalidadEmail.Text);
                    nuevoEmail.CTE_CedIden = this.Txt_Tap1_Cedula.Text; // Valor de la cédula del cliente (debes tenerla disponible, ej: textBoxCedulaCliente.Text);
                    nuevoEmail.Mail_Loogin = this.Txt_Tap1_Email.Text;// Valor del control para el login del email (ej: textBoxLoginEmail.Text);
                                                                      //nuevoEmail.Mail_Dominio = // Valor del control para el dominio del email (ej: textBoxDominioEmail.Text);
                                                                      //nuevoEmail.Mail_Ext = // Valor del control para la extensión del email (ej: textBoxExtensionEmail.Text);
                                                                      //nuevoEmail.Mail_Pref = chk_Tap1_email.Checked.ToString();
                    nuevoEmail.Mail_FecCrea = DateTime.Now; // .Fecha de creación actual
                    nuevoEmail.Mail_FecModif = DateTime.Now; // .Fecha de creación actual
                    nuevoEmail.USER_Crea = TB_USUARIO.COD_USR; // Usuario que está creando el registro (debes tenerlo disponible, ej: UsuarioLogueado.NombreUsuario);

                    // Crea una instancia de la capa lógica para emails (puedes usar la misma instancia si prefieres)
                    CapaLogica.CargarClientes_Logica.L_Cliente logicaClienteEmail = new CapaLogica.CargarClientes_Logica.L_Cliente();
                    string resultadoEmail = logicaClienteEmail.InsertarEmail(nuevoEmail);

                    // Maneja el resultado de la inserción del email
                    if (resultadoEmail == "Email guardado")
                    {
                        //MessageBox.Show("Email guardado exitosamente.", "Importante", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        // Puedes realizar acciones adicionales después de guardar el email
                    }
                    else
                    {

                        Pnl_2_Msj.Visible = true;
                        txt_pl2_msj.Text = " Error al guardar el email: {resultadoEmail}";
                        //pb_pl2_mj.Visible = true;

                        // Puedes registrar el error o informar al usuario de otra manera
                    }

                    // Opcional: Puedes recargar la información del cliente para mostrar el teléfono y email recién agregados
                    // CargarInfoCliente(textBoxCedulaCliente.Text);
                }



                //Pnl_2_Msj.Visible = true;
                //txt_pl2_msj.Text = "Cliente Guardado con Exito";
                ////pb_pl2_mj.Visible = false;
                
                //MessageBox.Show("Cliente Guardado Exitosamente", "Importante", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                btnExamen.Enabled = true;



                // Limpiar los campos del formulario si es necesario
                guardaclienteP();

                
                
                // LimpiarCampos();
                // Recargar la lista de clientes si es necesario
                // CargarClientesEnDataGridView();
            }
            else
            {
                MessageBox.Show($"Error al guardar el cliente: {resultado}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }

        }

        private void llenarcampos()
        {

            LimpiarCampos2();
            limpearExamen();

            

            Txt_Pnl2_Cedula.Text = dtCliente.Rows[0]["CTE_Nacio"].ToString() +"-"+dtCliente.Rows[0]["CTE_CedIden"].ToString(); // Ajusta el nombre de la columna
            Txt_Pnl_2_Nombre.Text = dtCliente.Rows[0]["CTE_PNombre"].ToString(); // Ajusta el nombre de la columna


            // Asigna los valores de la base de datos a las cajas de texto
            Cbx_Tap1_Nacionalidad.Text = dtCliente.Rows[0]["CTE_Nacio"].ToString();
            Txt_Tap1_Cedula.Text = dtCliente.Rows[0]["CTE_CedIden"].ToString(); // Ajusta el nombre de la columna
            Txt_Tap1_Nombre.Text = dtCliente.Rows[0]["CTE_PNombre"].ToString(); // Ajusta el nombre de la columna


            Dtp_Tap1_Nacimiento.Text = dtCliente.Rows[0]["CTE_FNac"].ToString();

            // ... Asigna los demás campos según tu estructura de base de datos

            if (dtCliente.Rows[0]["CTE_RETIVA"] != DBNull.Value && Convert.ToBoolean(dtCliente.Rows[0]["CTE_RETIVA"]))
            {
                Chex_Tap1_Iva.SetItemChecked(1, true); // Marcar el segundo elemento
            }
            else
            {
                Chex_Tap1_Iva.SetItemChecked(1, false); // Desmarcar el segundo elemento si es falso o nulo
            }

            if (dtCliente.Rows[0]["CTE_RETISLR"] != DBNull.Value && Convert.ToBoolean(dtCliente.Rows[0]["CTE_RETISLR"]))
            {
                Chex_Tap1_Iva.SetItemChecked(0, true); // Marcar el primer elemento (asumiendo que es ISR)
            }
            else
            {
                Chex_Tap1_Iva.SetItemChecked(0, false); // Desmarcar el primer elemento si es falso o nulo
            }

            if (dtCliente.Rows[0]["CTE_Sex"] != DBNull.Value)
            {
                string sexo = dtCliente.Rows[0]["CTE_Sex"].ToString().Trim().ToUpper();

                if (sexo == "F")
                {
                    Rd_Tap1_SexoF.Checked = true;
                    Rd_Tap1_SexoM.Checked = false;
                }
                else if (sexo == "M")
                {
                    Rd_Tap1_SexoM.Checked = true;
                    Rd_Tap1_SexoF.Checked = false;
                }
            }
            else
            {
                Rd_Tap1_SexoF.Checked = false;
                Rd_Tap1_SexoM.Checked = true;
            }




            //Txt_Tap1_Direccion_fact.Text = dtCliente.Rows[0]["Direccion_fact"].ToString();
            //Txt_Tap1_Cedula_Pagador.Text = dtCliente.Rows[0]["CTE_CedIdenP"].ToString();
            //Txt_Tap1_Nombre_Pagador.Text = dtCliente.Rows[0]["Nombre_Pagador"].ToString();
            //Txt_Tap1_Facebook.Text = dtCliente.Rows[0]["Facebook"].ToString();
            //Txt_Tap1_Twitter.Text = dtCliente.Rows[0]["Twitter"].ToString();
            //Txt_Tap1_Instagram.Text = dtCliente.Rows[0]["Instagram"].ToString();




            // Opcional: Establecer un valor por defecto.
            if (dtCliente != null && dtCliente.Rows.Count > 0)
            {
                Cbx_Tap1_Ciudad.SelectedValue = dtCliente.Rows[0]["COD_Ciud"]; // Establece el primer estado como seleccionado
            }
            else
            {
                Cbx_Tap1_Estado.SelectedIndex = -1; // No selecciona nada si el DataTable está vacío
            }

            Txt_Tap2_Examen.Text = dtCliente.Rows[0]["NumExamen"].ToString();

            //TopeExamen = dtCliente.Rows[0]["NumExamen"].ToString();


            if (dtCliente.Rows.Count > 0 && dtCliente.Rows[0]["NumExamen"] != DBNull.Value)
            {
                if (int.TryParse(dtCliente.Rows[0]["NumExamen"].ToString(), out int topeExamenInt))
                {
                    TopeExamen = topeExamenInt;
                    
                    btnExamen.Enabled = true;
                    // Ahora la variable TopeExamen (que debe ser de tipo int)
                    // contiene el valor entero extraído de la DataTable.
                }

            }


            CargarExamenConv();




            Cbx_Tap1_Estado.SelectedValue = dtCliente.Rows[0]["COD_Edo"].ToString();


            Cbx_Tap1_Ciudad.SelectedValue = dtCliente.Rows[0]["COD_Ciud"].ToString();
            //Cbx_Tap1_Ciudad.DisplayMember = dtCliente.Rows[0]["CIUD_Nombre"].ToString();
            //
           
            //         	   ,NULL[TLF_Cod002]
            //,NULL[TLF_Numero002]
            //,NULL[TLF_Ext002]
            //,NULL[TLF_Cod003]
            //,NULL[TLF_Numero003]
            //,NULL[TLF_Ext03]
            //,NULL[Mail_Loogin]


            //Cbx_Tap1_TLF_Celular.SelectedValue = dtCliente.Rows[0]["TLF_Cod002"].ToString();
            //  Cbx_Tap1_TLF_Celular.SelectedValue = dtCliente.Rows[0]["TLF_Cod002"].ToString().Trim();

            string valorABuscar = dtCliente.Rows[0]["TLF_Cod002"].ToString().Trim();
            bool encontrado = false;

            foreach (object item in Cbx_Tap1_TLF_Celular.Items)
            {
                // Como llenaste el ComboBox con strings directamente,
                // cada 'item' en la colección Items es un string.
                if (item != null && item.ToString() == valorABuscar)
                {
                    Cbx_Tap1_TLF_Celular.SelectedItem = item;
                    encontrado = true;
                    break; // Importante salir del bucle una vez que se encuentra la coincidencia
                }
            }
            Txt_Tap1_TLF_Celular.Text = dtCliente.Rows[0]["TLF_Numero002"].ToString();


            //  Cbx_Tap1_TLF_Local.SelectedValue = dtCliente.Rows[0]["TLF_Cod003"].ToString().Trim();
            string valorABuscar1 = dtCliente.Rows[0]["TLF_Cod003"].ToString().Trim();
            bool encontrado1 = false;

            foreach (object item1 in Cbx_Tap1_TLF_Local.Items)
            {
                // Como llenaste el ComboBox con strings directamente,
                // cada 'item' en la colección Items es un string.
                if (item1 != null && item1.ToString() == valorABuscar1)
                {
                    Cbx_Tap1_TLF_Local.SelectedItem = item1;
                    encontrado1 = true;
                    break; // Importante salir del bucle una vez que se encuentra la coincidencia
                }
            }
            Txt_Tap1_TLF_Local.Text = dtCliente.Rows[0]["TLF_Numero003"].ToString();


            Txt_Tap1_Email.Text = dtCliente.Rows[0]["Mail_Loogin"].ToString();


            //Cbx_Tap1_Estado.DisplayMember = dtCliente.Rows[0]["EDO_Nombre"].ToString();
            // Habilita las demás cajas de texto (esto ya estaba habilitado en otro botón)

            Txt_Tap1_Cedula.Enabled = true;
            Txt_Tap1_Cedula_Pagador.Enabled = true;
            Txt_Tap1_Edad.Enabled = true;
            Txt_Tap1_Email.Enabled = true;
            Dtp_Tap1_Nacimiento.Enabled = true;
            Txt_Tap1_Nombre.Enabled = true;
            Txt_Tap1_Nombre_Pagador.Enabled = true;
            Txt_Tap1_TLF_Celular.Enabled = true;
            Txt_Tap1_TLF_Local.Enabled = true;
            //   this.Txt_Tap1_Direccion_fact.Enabled = true;
            Txt_Tap1_Cedula_Pagador.Enabled = true;
            Txt_Tap1_Nombre_Pagador.Enabled = true;
            txt_Pnl2_cont_observa.Enabled = true;
            //Txt_Tap1_Facebook.Enabled = true;
            //Txt_Tap1_Twitter.Enabled = true;
            //Txt_Tap1_Instagram.Enabled = true;

            //dtClienteconGarantia = _L_Cliente.ObtenerClienteConGarantia(_D_DetalleOrden.TB_PARAMETRO("SucursalID"), Txt_Tap1_Cedula.Text, Cbx_Tap1_Nacionalidad.Text); // Usa la instancia _L_Cliente

            //if (dtClienteconGarantia.Rows.Count > 0)
            //{
            //}

        }

        private bool validaVaciocorreemail()
        {
            // Validación para Txt_Tap1_TLF_hab

            string textoIngresadoTLFCelular = Txt_Tap1_TLF_Celular.Text;
            if (Cbx_Tap1_TLF_Celular.SelectedIndex > -1)
            {

                if (string.IsNullOrEmpty(textoIngresadoTLFCelular))
                {
                    Pnl_2_Msj.Visible = false;
                    txt_pl2_msj.Text = "Registre un número de celular para continuar";
                    //pb_pl2_mj.Visible = false;
                    Cbx_Tap1_TLF_Local.Focus(); // Coloca el foco en el ComboBox para que el usuario corrija.

                }
                
                return true; // Detiene la ejecución del resto del código del botón.
            }



            // Validación para Txt_Tap1_TLF_Celular

            if (!string.IsNullOrEmpty(textoIngresadoTLFCelular))
            {
                if (!EsNumeroDeSieteDigitos(textoIngresadoTLFCelular))
                {
                    Pnl_2_Msj.Visible = true;
                    txt_pl2_msj.Text = "El número de teléfono debe contener exactamente 7 dígitos";
                    //pb_pl2_mj.Visible = true;

                    Txt_Tap1_TLF_Celular.Focus();
                    return false;
                }
                if (Cbx_Tap1_TLF_Celular.SelectedIndex == -1)
                {

                    Pnl_2_Msj.Visible = true;
                    txt_pl2_msj.Text = "Seleccione un número de Operador para continuar";
                    //pb_pl2_mj.Visible = true;

                    Cbx_Tap1_TLF_Celular.Focus(); // Coloca el foco en el ComboBox para que el usuario corrija.
                    return false; // Detiene la ejecución del resto del código del botón.
                }

            }

            //// Validación para Txt_Tap1_TLF_Local
            string textoIngresadoTLFLocal = Txt_Tap1_TLF_Local.Text;
            //if (Cbx_Tap1_TLF_Local.SelectedIndex > -1)
            //{

            //    if (string.IsNullOrEmpty(textoIngresadoTLFLocal))
            //    {
            //        Pnl_2_Msj.Visible = true;
            //        txt_pl2_msj.Text = "Registre un número de Local para continuar";
            //        //pb_pl2_mj.Visible = true;

            //    }
            //    Cbx_Tap1_TLF_Local.Focus(); // Coloca el foco en el ComboBox para que el usuario corrija.
            //    return false; // Detiene la ejecución del resto del código del botón.
            //}


            if (!string.IsNullOrEmpty(textoIngresadoTLFLocal))
            {
                if (!EsNumeroDeSieteDigitos(textoIngresadoTLFLocal))
                {
                    Pnl_2_Msj.Visible = true;
                    txt_pl2_msj.Text = "El número de teléfono local debe contener exactamente 7 dígitos";
                    //pb_pl2_mj.Visible = true;


                    Txt_Tap1_TLF_Local.Focus();
                    return false;
                }
                // Verifica si no se ha seleccionado nada en el ComboBox.
                if (Cbx_Tap1_TLF_Local.SelectedIndex == -1)
                {

                    Pnl_2_Msj.Visible = true;
                    txt_pl2_msj.Text = "Seleccione un número de teléfono local para continuar";
                    //pb_pl2_mj.Visible = true;


                    Cbx_Tap1_TLF_Local.Focus(); // Coloca el foco en el ComboBox para que el usuario corrija.
                    return false; // Detiene la ejecución del resto del código del botón.
                }

            }

            // Validación para Txt_Tap1_Email
            string textoIngresadoEmail = Txt_Tap1_Email.Text;
            if (!string.IsNullOrEmpty(textoIngresadoEmail))
            {
                if (!EsEmailValido(textoIngresadoEmail))
                {
                    Pnl_2_Msj.Visible = true;
                    txt_pl2_msj.Text = "El formato del correo electrónico no es válido";
                    //pb_pl2_mj.Visible = true;



                    Txt_Tap1_Email.Focus();
                    return false;
                }
            }

            return true; // Si todas las validaciones (para los campos que tienen valor) pasan
        }

        public bool EsNumeroDeSieteDigitos(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return false; // Un valor vacío o solo espacios no es válido
            }

            texto = texto.Trim(); // Eliminar espacios en blanco al inicio y al final

            // Expresión regular para validar que solo contenga dígitos y sean exactamente 7
            string patron = @"^\d{7}$";

            // Utilizar la clase Regex para realizar la coincidencia
            return Regex.IsMatch(texto, patron);
        }

        public bool EsEmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false; // Un valor vacío o solo espacios no es válido
            }

            email = email.Trim(); // Eliminar espacios en blanco al inicio y al final

            // Expresión regular para validar el formato del correo electrónico (RFC 5322)
            string patron = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";

            // Utilizar la clase Regex para realizar la coincidencia
            return Regex.IsMatch(email, patron);
        }

        private bool validarvacio()
        {






            // Validación de Nacionalidad
            if (Cbx_Tap1_Nacionalidad.SelectedIndex == -1) // Mejor usar SelectedIndex para ComboBox
            {

                Pnl_2_Msj.Visible = true;
                txt_pl2_msj.Text = "Seleccione la nacionalidad antes de ingresar la cédula";
                //pb_pl2_mj.Visible = true;
                Cbx_Tap1_Nacionalidad.Focus();
                return false;
            }

            // Validación de Cédula
            if (string.IsNullOrEmpty(Txt_Tap1_Cedula.Text.Trim()))
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Registre el número de cédula del cliente para continuar");
                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                _FrmMensajes.ShowDialog();


                //MessageBox.Show("El campo de Cédula no puede estar vacío.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Txt_Tap1_Cedula.Focus();
                return false;
            }

            // Validación de Nombre
            if (string.IsNullOrEmpty(Txt_Tap1_Nombre.Text.Trim()))
            {

                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Registre el nombre y apellido del cliente para continuar");
                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                _FrmMensajes.ShowDialog();

                Txt_Tap1_Nombre.Focus();
                return false;
            }


            // 1. Obtener la fecha seleccionada del DateTimePicker
            DateTime fechaNacimiento = Dtp_Tap1_Nacimiento.Value;

            // 2. Obtener la fecha actual (solo la parte de la fecha, sin la hora)
            DateTime fechaActual = DateTime.Today;

            // 3. Realizar la validación
            if (fechaNacimiento < fechaActual)
            {
            }
            else
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Seleccione la fecha de nacimiento del cliente");
                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                _FrmMensajes.ShowDialog();


                Dtp_Tap1_Nacimiento.Focus(); // Opcional: enfocar el control DateTimePicker para que el usuario lo corrija
                return false; // Sale del método porque no se ha seleccionado ninguno.
            }



            if (!Rd_Tap1_SexoF.Checked && !Rd_Tap1_SexoM.Checked)
            {


                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Seleccione el género del cliente");
                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                _FrmMensajes.ShowDialog();


                // Aquí podrías decidir qué RadioButton enfocar. Por ejemplo, enfocar el primero:
                Rd_Tap1_SexoF.Focus();
                return false; // Sale del método porque no se ha seleccionado ninguno.
            }
            // Si llega aquí, es porque se seleccionó al menos un CheckBox de sexo.
            // Puedes continuar con la lógica que sigue a la validación.
            // Validación de Dirección


            // Validación de Estado
            if (Cbx_Tap1_Estado.SelectedIndex == -1) // Mejor usar SelectedIndex para ComboBox
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Seleccione un estado para continuar");
                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                _FrmMensajes.ShowDialog();

                Cbx_Tap1_Estado.Focus();
                return false;
            }

            // Validación de Ciudad
            if (Cbx_Tap1_Ciudad.SelectedIndex == -1) // Mejor usar SelectedIndex para ComboBox
            {

                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Seleccione una ciudad para continuar");
                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                _FrmMensajes.ShowDialog(); 
               

                Cbx_Tap1_Ciudad.Focus();
                return false;
            }



            // Validación de Ciudad
            // Check if both Local and Celular ComboBoxes are empty
            if (Cbx_Tap1_TLF_Local.SelectedIndex == -1 && Cbx_Tap1_TLF_Celular.SelectedIndex == -1)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Registre un número telefónico para continuar");
                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                _FrmMensajes.ShowDialog();

                // Decide which ComboBox to focus on. You might prioritize Local, or the first one.
                Cbx_Tap1_TLF_Local.Focus();
                return false;
            }





            // Validación de Nombre
            if (string.IsNullOrEmpty(Txt_Tap1_Email.Text.Trim()))
            {


                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Registre el email del cliente para continuar");
                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                _FrmMensajes.ShowDialog();


                Txt_Tap1_TLF_Local.Focus();
                return false;
            }


            return true; // Si todas las validaciones pasan, devuelve true
        }

        public void Btn_Tap2_Derecha_Click(object sender, EventArgs e)
        {
            CancelarPorCambioExamen();

            if (string.IsNullOrEmpty(Txt_Tap2_Examen.Text))
            {
                Txt_Tap2_Examen.Text = "1";
            }


            if (int.TryParse(Txt_Tap2_Examen.Text, out int valorActual))
            {
                if (valorActual > 0 && valorActual < TopeExamen)
                {
                    Txt_Tap2_Examen.Text = (valorActual + 1).ToString();
                }
                else
                {

                    Txt_Tap2_Examen.Text = TopeExamen.ToString(); // Opcional: Restablecer el valor al máximo
                }
            }
            else
            {
                Txt_Tap2_Examen.Text = "1"; // Opcional: Restablecer a un valor predeterminado (por ejemplo, el mínimo si aplica)
            }
           
            CargarExamenConv();
            CargarExamenCont();
            //CargarDgvPnl2MedConv();
            CargarFicconvOFT();

            CargarDgv_Pnl2_Querato();

            if (Cbx_Tap2_Tipo_Examen.Text == "CONTACTO")
            {
                grp_pln2_Cont1.Visible = true;
                grp_pln2_Cont1.BringToFront();
            }
            else if (Cbx_Tap2_Tipo_Examen.Text == "CONVENCIONAL" || Cbx_Tap2_Tipo_Examen.Text == "")
            {
                grp_pln2_Conv2.Visible = true;
                grp_pln2_Conv2.BringToFront();
              

            }

            btn_pln2_oft.BringToFront();
            btn_pln2_reti.BringToFront();
            btn_pln2_quer.BringToFront();
        }

        private void guardaclienteP()
        {
            // Crear una instancia de la entidad TB_CTEPPAL para almacenar los datos

            TB_CTEPPAL nuevoCliente = new TB_CTEPPAL();

            // Recopilar los datos de los controles del formulario
            nuevoCliente.CTE_CedIden = Txt_Tap1_Cedula_Pagador.Text.Trim();
            nuevoCliente.CTE_Nacio = Cbx_Tap1_Nacionalidad_Pagador.Text.Trim(); // Ajusta según cómo manejas la nacionalidad
            nuevoCliente.CTE_PNombre = Txt_Tap1_Nombre_Pagador.Text.Trim();
            //nuevoCliente.CTE_SNombre = string.IsNullOrEmpty(Txt_Tap1_SegundoNombre.Text) ? null : Txt_Tap1_SegundoNombre.Text.Trim(); // Asume que tienes un Txt_Tap1_SegundoNombre
            //nuevoCliente.CTE_PApellido = Txt_Tap1_Apellido.Text.Trim(); // Asume que tienes un Txt_Tap1_Apellido
            //nuevoCliente.CTE_SApellido = string.IsNullOrEmpty(Txt_Tap1_SegundoApellido.Text) ? null : Txt_Tap1_SegundoApellido.Text.Trim(); // Asume que tienes un Txt_Tap1_SegundoApellido
            nuevoCliente.CTE_FNac = Dtp_Tap1_Nacimiento.Value.Date;
            nuevoCliente.CTE_FecAfil = DateTime.Now.Date; // Asigna la fecha de afiliación actual
            nuevoCliente.COD_STCTE = "ACT"; // Asigna un estado por defecto (ajusta según tu lógica)

            // Obtener el sexo del CheckListBox
            if (Rd_Tap1_SexoF.Checked && !Rd_Tap1_SexoM.Checked)
            {
                nuevoCliente.CTE_Sex = "F"; // Femenino
            }
            else if (!Rd_Tap1_SexoF.Checked && Rd_Tap1_SexoM.Checked)
            {
                nuevoCliente.CTE_Sex = "M"; // Masculino
            }
            else
            {
                nuevoCliente.CTE_Sex = null; // O maneja un estado no especificado (ninguno seleccionado o ambos, aunque esto último no debería ocurrir en un grupo de RadioButton bien configurado)
            }

            //nuevoCliente.CTE_CodOcup = string.IsNullOrEmpty(Txt_Tap1_Ocupacion.Text) ? null : Txt_Tap1_Ocupacion.Text.Trim(); // Asume que tienes un Txt_Tap1_Ocupacion
            //nuevoCliente.CTE_EdoCiv = Cbx_Tap1_EstadoCivil.Text.Trim(); // Asume que tienes un Cbx_Tap1_EstadoCivil
            nuevoCliente.COD_Sucursal = codigoSucursal; // Asigna una sucursal por defecto (ajusta según tu lógica)
            nuevoCliente.CTE_FecCreacion = DateTime.Now;
            nuevoCliente.USER_CREA = TB_USUARIO.COD_USR;  // Reemplaza con el usuario actual del sistema

            //nuevoCliente.Direccion_fact = string.IsNullOrEmpty(Txt_Tap1_Direccion_fact.Text) ? null : Txt_Tap1_Direccion_fact.Text.Trim(); // Asume que tienes un Txt_Tap1_Direccion
            //nuevoCliente.Facebook = string.IsNullOrEmpty(Txt_Tap1_Facebook.Text) ? null : Txt_Tap1_Facebook.Text.Trim(); // Asume que tienes un Txt_Tap1_Facebook
            //nuevoCliente.Twitter = string.IsNullOrEmpty(Txt_Tap1_Twitter.Text) ? null : Txt_Tap1_Twitter.Text.Trim(); // Asume que tienes un Txt_Tap1_Twitter
            //nuevoCliente.Instagram = string.IsNullOrEmpty(Txt_Tap1_Instagram.Text) ? null : Txt_Tap1_Instagram.Text.Trim(); // Asume que tienes un Txt_Tap1_Instagram

            // Obtener los valores de los CheckBoxes de retención
            nuevoCliente.CTE_RETISLR = Chex_Tap1_Iva_Pagador.GetItemChecked(0); // Asume que ISR está en el índice 0
            nuevoCliente.CTE_RETIVA = Chex_Tap1_Iva_Pagador.GetItemChecked(1); // Asume que IVA está en el índice 1

            nuevoCliente.COD_Edo = Cbx_Tap1_Estado.SelectedValue.ToString(); // 
            nuevoCliente.COD_Ciud = Cbx_Tap1_Ciudad.SelectedValue.ToString(); // 

            // Llamar al método de la capa lógica para guardar el cliente
            resultado = _L_Cliente.GuardarClienteP(nuevoCliente);


        }

        private void LlenarCbx_Tap1_Ciudad(string codigoEstado)
        {
            List<TB_MAESCIUD> ciudades = _L_Cliente.ObtenerCiudadesPorEstado(codigoEstado);
            if (ciudades != null)
            {
                Cbx_Tap1_Ciudad.DataSource = ciudades;
                Cbx_Tap1_Ciudad.DisplayMember = "CIUD_Nombre"; // El nombre de la ciudad a mostrar
                Cbx_Tap1_Ciudad.ValueMember = "COD_Ciud";   // El código de la ciudad como valor asociado
                                                            //  Cbx_Tap1_Ciudad.DropDownWidth = DropDownWidth(Cbx_Tap1_Ciudad);
            }
            else
            {
                MessageBox.Show("Error al cargar las ciudades: " + _L_Cliente.stringBuilder.ToString());
            }
        }

        private void buscarcliente()
        {
            // Evita que el evento KeyDown se siga propagando (opcional)


            //DesbloquearCamposE();



            // Verifica si panel2 existe
            if (panel2 != null)
            {
                // Muestra panel2
                panel2.Visible = true;


                panel2.Location = new System.Drawing.Point(202, 46);

                // Centra panel2 en la pantalla (CORREGIR ESTO)
                // panel2.StartPosition = FormStartPosition.CenterScreen; // ESTO ES INCORRECTO PARA UN PANEL
                //panel2.Location = new System.Drawing.Point(
                //   ( (Screen.PrimaryScreen.WorkingArea.Width - panel2.Width) / 2)-30,
                //    (Screen.PrimaryScreen.WorkingArea.Height - panel2.Height) / 2);

                // O, si panel2 es un control dentro del formulario y quieres centrarlo dentro del formulario:
                // panel2.Location = new System.Drawing.Point(
                //     (this.ClientSize.Width - panel2.Width) / 2,
                //     (this.ClientSize.Height - panel2.Height) / 2);

                // Trae panel2 al frente si está detrás de otros controles (opcional)
                panel2.BringToFront();

                // Enfoca panel2 (opcional, si quieres que el usuario interactúe inmediatamente con él)
                panel2.Focus();
            }
            else
            {
                MessageBox.Show("El Panel2 no ha sido inicializado.");
            }
        }

        private void FrmCargarOrden_KeyDown(object sender, KeyEventArgs e)
        {

            // Verifica si la tecla presionada es Escape y si panel2 está visible
            if (e.KeyCode == Keys.Escape && panel2 != null && panel2.Visible)
            {
                panel2.Visible = false;
                Txt_Tap1_Cedula.Focus(); // Opcional: devolver el foco al TextBox
            }

        }

        private void DgvClientes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            { 
            //private void DgvClientes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
            //{
            // Verificar que el doble clic no sea en el encabezado de la columna
            if (e.RowIndex >= 0 && DgvClientes.Rows[e.RowIndex].Cells.Count > 0)
            {
                // Obtener el valor de la columna 0 (la cédula) de la fila en la que se hizo doble clic
                string cedulaSeleccionada = DgvClientes.Rows[e.RowIndex].Cells[1].Value?.ToString().Trim();
                string nacio = DgvClientes.Rows[e.RowIndex].Cells[0].Value?.ToString().Trim();
                // Verificar si se obtuvo una cédula válida
                if (!string.IsNullOrEmpty(cedulaSeleccionada))
                {
                    try
                    {
                        dtCliente = _L_Cliente.ObtenerClientePorCedula(cedulaSeleccionada, nacio); // Usa la cédula seleccionada

                        if (dtCliente != null && dtCliente.Rows.Count > 0)
                        {

                            llenarcampos();
                            panel2.Visible = false;
                            Txt_Tap1_Cedula.Focus();

                        }
                        else
                        {
                            MessageBox.Show("No se encontró ningún cliente con esa cédula.", "Importante", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            // Opcionalmente, puedes limpiar las otras cajas de texto o deshabilitarlas.
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ocurrió un error al obtener la información del cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    }
                }
            }

            }
            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
            }
        }

        private (int años, int meses) CalcularEdadCompleta(DateTime fechaNacimiento)
        {
            DateTime fechaActual = DateTime.Now;
            int años = fechaActual.Year - fechaNacimiento.Year;
            int meses = 0;

            if (fechaActual.Month < fechaNacimiento.Month || (fechaActual.Month == fechaNacimiento.Month && fechaActual.Day < fechaNacimiento.Day))
            {
                años--;
                meses = (12 - fechaNacimiento.Month + fechaActual.Month);
                if (fechaActual.Day < fechaNacimiento.Day)
                {
                    meses--;
                }
            }
            else
            {
                meses = fechaActual.Month - fechaNacimiento.Month;
                if (fechaActual.Day < fechaNacimiento.Day)
                {
                    meses--;
                    if (meses < 0)
                    {
                        meses = 11;
                        años--;
                    }
                }
            }

            return (años, meses);
        }

        private int CalcularEdad(DateTime fechaNacimiento)
        {
            // Obtener la fecha actual (considerando la zona horaria actual)
            DateTime fechaActual = DateTime.Now;

            // Calcular la diferencia de años
            int edad = fechaActual.Year - fechaNacimiento.Year;

            // Ajustar la edad si el cumpleaños aún no ha ocurrido este año
            if (fechaNacimiento.Month > fechaActual.Month || (fechaNacimiento.Month == fechaActual.Month && fechaNacimiento.Day > fechaActual.Day))
            {
                edad--;
            }

            return edad;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtClienteP.Text = "";
            Pnl_5_Lista_ClienPagador.Visible = false;
            Pnl_2_Msj.Visible = false;
        }

        private void DesbloquearCamposE()
        {
            //Cbx_Tap2_Tipo_Examen.Enabled = true;
            Cbx_Tap2_Tipo_Optome.Enabled = true;
            Cbx_Tap2_Nombre_Optome.Enabled = true;

            Cbx_Tap2_Ojo.Enabled = true;
            grp_pln2_Cont1.Enabled = true;
            grp_pln2_Conv2.Enabled = true;
            grp_pln2_oft3.Enabled = true;
            grp_pln2_ret4.Enabled = true;
            grp_pln2_quera5.Enabled = true;
            grp_pln2_OBS.Enabled = true;

            Dgv_Pnl2_cont.Enabled = true;
            Dgv_Pnl2_conv.Enabled = true;
            //Dgv_Pnl2_medconv.Enabled = true;
            txt_Pnl2_oftd.Enabled = true;
            txt_Pnl2_ofti.Enabled = true;
            txt_Pnl2_reti.Enabled = true;
            txt_Pnl2_retd.Enabled = true;
            TXT_Tap2_Nombre_Optome.Enabled = true;
            txt_Pnl2_observa.Enabled = true;

        }


        private void BloquearCamposE()
        {
            //Cbx_Tap2_Tipo_Examen.Enabled = false;
            Cbx_Tap2_Tipo_Optome.Enabled = false;
            Cbx_Tap2_Nombre_Optome.Enabled = false;



            //Cbx_Tap2_Ojo.Enabled = false;
            grp_pln2_Cont1.Enabled = false;
            grp_pln2_Conv2.Enabled = false;
            grp_pln2_oft3.Enabled = false;
            grp_pln2_ret4.Enabled = false;
            grp_pln2_quera5.Enabled = false;
            grp_pln2_OBS.Enabled = false;

            Dgv_Pnl2_cont.Enabled = false;
            Dgv_Pnl2_conv.Enabled = false;
            //Dgv_Pnl2_medconv.Enabled = false;
            txt_Pnl2_oftd.Enabled = false;
            txt_Pnl2_ofti.Enabled = false;
            txt_Pnl2_reti.Enabled = false;
            txt_Pnl2_retd.Enabled = false;
            txt_Pnl2_observa.Enabled = false;
        }

        private void BloquearCampos()
        {

            //Btn_Tap1_GuardarET.Enabled = false;
            Cbx_Tap1_Estado.SelectedIndex = -1;
            Cbx_Tap1_Ciudad.SelectedIndex = -1;
            Txt_Tap1_Nombre.Text = "";
            Dtp_Tap1_Nacimiento.Value = DateTime.Now;
            Rd_Tap1_SexoF.Checked = false;
            Rd_Tap1_SexoM.Checked = false;
            //Txt_Tap1_Ocupacion.Text = "";
            //Cbx_Tap1_EstadoCivil.SelectedIndex = -1;
            //Txt_Tap1_Direccion.Text = "";
            //Txt_Tap1_Facebook.Text = "";
            //Txt_Tap1_Twitter.Text = "";
            //Txt_Tap1_Instagram.Text = "";
            Chex_Tap1_Iva.SetItemChecked(0, false);
            Chex_Tap1_Iva.SetItemChecked(1, false);
            Txt_Tap1_Edad.Text = ""; // Limpiar el campo de edad también


            //Txt_Tap1_Direccion_fact.Enabled = false;
            Txt_Tap1_Cedula.Enabled = false;
            Txt_Tap1_Cedula_Pagador.Enabled = false;
            Txt_Tap1_Edad.Enabled = false;
            Txt_Tap1_Email.Enabled = false;
            Dtp_Tap1_Nacimiento.Enabled = false;
            Txt_Tap1_Nombre.Enabled = false;
            Txt_Tap1_Nombre_Pagador.Enabled = false;
            Txt_Tap1_TLF_Celular.Enabled = false;
            Txt_Tap1_TLF_Local.Enabled = false;

            //Txt_Tap1_Facebook.Enabled = false;
            //Txt_Tap1_Instagram.Enabled = false;
            //Txt_Tap1_Twitter.Enabled = false;


            // Habilitar los controles
            Cbx_Tap1_Ciudad.Enabled = false;
            Cbx_Tap1_Estado.Enabled = false;
            Cbx_Tap1_Nacionalidad.Enabled = false;
            Cbx_Tap1_Nacionalidad_Pagador.Enabled = false;
            Cbx_Tap1_TLF_Celular.Enabled = false;
            Cbx_Tap1_TLF_Local.Enabled = false;

            ///// &&&&&&&&&&&&&&

            //Txt_Tap1_Direccion_fact.Text = "";
            Txt_Tap1_Cedula_Pagador.Text = "";
            Txt_Tap1_Edad.Text = "";
            Txt_Tap1_Email.Text = "";
            Dtp_Tap1_Nacimiento.Text = "";
            Txt_Tap1_Nombre.Text = "";
            Txt_Tap1_Nombre_Pagador.Text = "";
            Txt_Tap1_TLF_Celular.Text = "";
            Txt_Tap1_TLF_Local.Text = "";




            Cbx_Tap1_Ciudad.Text = "";
            Cbx_Tap1_Estado.Text = "";
            Cbx_Tap1_Nacionalidad_Pagador.Text = "";
            Cbx_Tap1_TLF_Celular.Text = "";
            Cbx_Tap1_TLF_Local.Text = "";

            // TextBox
            Txt_Tap1_Email.Text = string.Empty;

            Txt_Tap1_TLF_Celular.Text = string.Empty;

            Txt_Tap1_TLF_Local.Text = string.Empty;

            // ComboBox
            Cbx_Tap1_TLF_Celular.SelectedIndex = -1; // Esto deselecciona el elemento

            Cbx_Tap1_TLF_Local.SelectedIndex = -1;



        }

        private void Cbx_Tap2_Tipo_Examen_SelectedIndexChanged(object sender, EventArgs e)
        {
            //// Ocultamos todos los GroupBox al principio
            grp_pln2_Cont1.Visible = false;
            grp_pln2_Conv2.Visible = false;
            grp_pln2_oft3.Visible = false;
            grp_pln2_ret4.Visible = false;
            grp_pln2_quera5.Visible = false;


            // centrargroup();



            //// Mostramos el GroupBox correspondiente según la selección del ComboBox
            switch (Cbx_Tap2_Tipo_Examen.SelectedIndex)
            {
                case 0: // Item 1 (los índices empyiezan en 0)
                    grp_pln2_Cont1.Visible = true;
                    CargarExamenCont();
                    AsignarCeroSiVacioDgv_Pnl2_cont();
                    //pnl_tab1_observa.Visible = true;
                    //pnl_tab1_observa.BringToFront();
                    pnlObservCon.Visible = true;
                    //txt_Pnl2_cont_observa.Visible = true;
                    //lbl_pnl2_con_obser.Visible = true;
                    ValidarPanel = "Cont";
                    //Point currentPosition = lbl_pnl2_con_obser.Location;
                    //Point newPositionl = new Point(currentPosition.X, currentPosition.Y - 100);
                    //lbl_pnl2_con_obser.Location = newPositionl;

                    // El resto de tu código Form1_Load, incluyendo el cambio de tamaño y posición.
                    //Point currentPositionT = txt_Pnl2_conv_observa.Location;
                    //Point newPositiont = new Point(currentPositionT.X, currentPositionT.Y - 100);
                    //txt_Pnl2_conv_observa.Location = newPositiont;
                    //txt_Pnl2_conv_observa.Height += 100; // Aumenta la altura en 200

                    //lbl_pnl2_obser.Location = new Point(7, 138);
                    //txt_Pnl2_conv_observa.Location = new Point(21, 231);
                    //txt_Pnl2_conv_observa.Size = new Size(1008, 114);
                    break;
                case 1: // Item 2
                    grp_pln2_Conv2.Visible = true;
                    //lbl_pnl2_obser.Location = new Point(12, 412);
                    //txt_Pnl2_conv_observa.Location = new Point(21, 434);
                    //txt_Pnl2_conv_observa.Size = new Size(1008, 31);
                    CargarExamenConv();
                    AsignarCeroSiVacioDgv_Pnl2_cont();
                    //CargarDgvPnl2MedConv();
                    //AsignarCeroSiVacioDgv_Pnl2_medconv();
                    ValidarPanel = "Conv";
                    //pnl_tab1_observa.Visible = false;
                    pnlObservCon.Visible = false;
                    //txt_Pnl2_cont_observa.Visible = false;
                    //lbl_pnl2_con_obser.Visible = false;
                    //lbl_pnl2_con_obser.Visible = false;
                    break;


                default:
                    // Si no se selecciona ningún ítem válido, podrías dejar todos los GroupBox ocultos
                    //Cbx_Tap2_Tipo_Examen.SelectedIndex = 1;
                    //grp_pln2_Conv2.Visible = true;
                    break;
            }


            btn_pln2_oft.BringToFront();
            btn_pln2_reti.BringToFront();
            btn_pln2_quer.BringToFront();
        }

        private void buscarclientep()
        {


            //CargarDatosDeClientesP();


            // Verifica si panel2 existe
            if (Pnl_5_Lista_ClienPagador != null)
            {
                // Muestra panel2
                Pnl_5_Lista_ClienPagador.Visible = true;


                Pnl_5_Lista_ClienPagador.Location = new System.Drawing.Point(202, 46);

                // Centra panel2 en la pantalla (CORREGIR ESTO)
                // panel2.StartPosition = FormStartPosition.CenterScreen; // ESTO ES INCORRECTO PARA UN PANEL
                //panel2.Location = new System.Drawing.Point(
                //   ( (Screen.PrimaryScreen.WorkingArea.Width - panel2.Width) / 2)-30,
                //    (Screen.PrimaryScreen.WorkingArea.Height - panel2.Height) / 2);

                // O, si panel2 es un control dentro del formulario y quieres centrarlo dentro del formulario:
                // panel2.Location = new System.Drawing.Point(
                //     (this.ClientSize.Width - panel2.Width) / 2,
                //     (this.ClientSize.Height - panel2.Height) / 2);

                // Trae panel2 al frente si está detrás de otros controles (opcional)
                Pnl_5_Lista_ClienPagador.BringToFront();

                // Enfoca panel2 (opcional, si quieres que el usuario interactúe inmediatamente con él)
                Pnl_5_Lista_ClienPagador.Focus();
            }
            else
            {
                MessageBox.Show("El Pnl_5_Lista_ClienPagador no ha sido inicializado.");
            }
        }

        private void LlenarCbxTap2NombreOptome()
        {
            if (Cbx_Tap2_Tipo_Optome.SelectedItem != null)
            {
                string tipoOptometrista = Cbx_Tap2_Tipo_Optome.SelectedItem.ToString();

                // Obtener los usuarios filtrados (esto asumo que ya lo tienes en tu lógica)
                DataTable dtOptometristas = _L_Cliente.ObtenerUsuariosOPTOMETRI(); // Obtén los datos

                List<TBF_USUARIO_OPTOMETRI> listaOptometristas = new List<TBF_USUARIO_OPTOMETRI>();

                if (dtOptometristas != null && dtOptometristas.Rows.Count > 0)
                {
                    foreach (DataRow row in dtOptometristas.Rows)
                    {
                        // Asegúrate de que esta lógica de filtrado sea correcta y coincida con lo que necesitas.

                        TBF_USUARIO_OPTOMETRI optometrista = new TBF_USUARIO_OPTOMETRI
                        {
                            COD_USR = row["COD_USR"].ToString(),
                            USER_NOMBRE = row["USER_NOMBRE"].ToString(),
                            USER_APELLIDO = row["USER_APELLIDO"].ToString(),
                            // ... (mapea otras propiedades)
                        };
                        listaOptometristas.Add(optometrista);

                    }
                }


                // Asigna la lista filtrada al DataSource del ComboBox
                Cbx_Tap2_Nombre_Optome.DataSource = listaOptometristas;
                Cbx_Tap2_Nombre_Optome.DisplayMember = "USER_NOMBRE"; // Ajusta el nombre del campo a mostrar
                Cbx_Tap2_Nombre_Optome.ValueMember = "COD_USR";    // Ajusta el nombre del campo del valor

                Cbx_Tap1_Estado.DropDownWidth = DropDownWidth(Cbx_Tap1_Estado);

            }


        }

        private void Cbx_Tap2_Tipo_Optome_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (Cbx_Tap2_Tipo_Optome.Enabled == true && Cbx_Tap2_Tipo_Optome.SelectedItem != null && Cbx_Tap2_Tipo_Optome.SelectedItem.ToString() == "INTERNO")
            {
                LlenarCbxTap2NombreOptome();
                TXT_Tap2_Nombre_Optome.Visible = false;
                Cbx_Tap2_Nombre_Optome.Visible = true;
            }
            else
            {
                // Cbx_Tap2_Nombre_Optome.Items.Clear(); // Limpia el ComboBox si no es "Interno"
                Cbx_Tap2_Nombre_Optome.Visible = false;
                TXT_Tap2_Nombre_Optome.Visible = true;
                //TXT_Tap2_Nombre_Optome.Enabled = true;
            }

        }

        private void btn_pln2_oft_Click(object sender, EventArgs e)
        {
            pnlObservCon.Visible = false;
            //txt_Pnl2_cont_observa.Visible = false;
            //lbl_pnl2_con_obser.Visible = false;
            //RestaurarPosicionOriginal();
            //RestaurarPosicionYAlturaOriginal();

            //// Ocultamos todos los GroupBox al principio
            grp_pln2_Cont1.Visible = false;
            grp_pln2_Cont1.BringToFront();
            grp_pln2_Conv2.Visible = false;
            grp_pln2_oft3.Visible = true;
            grp_pln2_ret4.Visible = false;
            grp_pln2_quera5.Visible = false;
            centrargroup();
            CargarFicconvOFT();
            ValidarPanel = "oft";
            btn_pln2_oft.BringToFront();
            btn_pln2_reti.BringToFront();
            btn_pln2_quer.BringToFront();

            //// Mostramos el GroupBox correspondiente según la selección del ComboBox

        }

        private void centrargroup()
        {
            //grp_pln2_Cont1.Location = new System.Drawing.Point(10, 200); //Posicion del GroupBox dentro del TabPage
            //grp_pln2_Conv2.Location = new System.Drawing.Point(10, 200); //Posicion del GroupBox dentro del TabPage
            //grp_pln2_oft3.Location = new System.Drawing.Point(10, 200); //Posicion del GroupBox dentro del TabPage
            //grp_pln2_ret4.Location = new System.Drawing.Point(10, 200); //Posicion del GroupBox dentro del TabPage
            //grp_pln2_quera5.Location = new System.Drawing.Point(10, 200); //Posicion del GroupBox dentro del TabPage
        }

        private void btn_pln2_reti_Click(object sender, EventArgs e)
        {
            pnlObservCon.Visible = false;
            //txt_Pnl2_cont_observa.Visible = false;
            //lbl_pnl2_con_obser.Visible = false;
            //RestaurarPosicionOriginal();
            //RestaurarPosicionYAlturaOriginal();

            //// Ocultamos todos los GroupBox al principio
            grp_pln2_Cont1.Visible = false;
            grp_pln2_Conv2.Visible = false;
            grp_pln2_oft3.Visible = false;
            grp_pln2_ret4.Visible = true;
            grp_pln2_ret4.BringToFront();
            grp_pln2_quera5.Visible = false;

            CargarFicconvOFT();
            centrargroup();

            ValidarPanel = "reti";
            btn_pln2_oft.BringToFront();
            btn_pln2_reti.BringToFront();
            btn_pln2_quer.BringToFront();

        }

        private void btn_pln2_quer_Click(object sender, EventArgs e)
        {
            pnlObservCon.Visible = false;
            //txt_Pnl2_cont_observa.Visible = false;
            //lbl_pnl2_con_obser.Visible = false;
            //RestaurarPosicionOriginal();
            //RestaurarPosicionYAlturaOriginal();

            //// Ocultamos todos los GroupBox al principio
            grp_pln2_Cont1.Visible = false;
            grp_pln2_Conv2.Visible = false;
            grp_pln2_oft3.Visible = false;
            grp_pln2_ret4.Visible = false;
            grp_pln2_quera5.Visible = true;
            grp_pln2_quera5.BringToFront();

            CargarDgv_Pnl2_Querato();
            ValidarPanel = "Querato";
            centrargroup();
            btn_pln2_oft.BringToFront();
            btn_pln2_reti.BringToFront();
            btn_pln2_quer.BringToFront();
        }

        private void Btn_Tap2_Examen_Click(object sender, EventArgs e)
        {
            CancelarPorCambioExamen();
            Txt_Tap2_Examen.Text = "0";
            mantenervacio = true;
            // Asumiendo que Dtp_Tap2_FecExam es de tipo DateTime


            Dtp_Tap2_FecExam.Text = DateTime.Today.ToString();

            limpearExamen();
            CargarExamenConv();
            CargarExamenCont();
//            CargarDgvPnl2MedConv();
            CargarFicconvOFT();
            CargarFicconvOFT();
            CargarDgv_Pnl2_Querato();
            DesbloquearCamposE();
            btnCargarOrden.Enabled = false;
            if (Cbx_Tap2_Tipo_Examen.Text == "CONTACTO")
            {
                grp_pln2_Cont1.Visible = true;
                grp_pln2_Cont1.BringToFront();
            }
            else if (Cbx_Tap2_Tipo_Examen.Text == "CONVENCIONAL" || Cbx_Tap2_Tipo_Examen.Text == "")
            {
                grp_pln2_Conv2.Visible = true;
                grp_pln2_Conv2.BringToFront();
            }

            btn_pln2_oft.BringToFront();
            btn_pln2_reti.BringToFront();
            btn_pln2_quer.BringToFront();

        }

        private void Btn_Tap2_Izquierda_Click(object sender, EventArgs e)
        {
            CancelarPorCambioExamen();



            if (string.IsNullOrEmpty(Txt_Tap2_Examen.Text))
            {
                Txt_Tap2_Examen.Text = "1";
            }



            if (int.TryParse(Txt_Tap2_Examen.Text, out int valorActual))
            {
                if (valorActual > 1)
                {
                    Txt_Tap2_Examen.Text = (valorActual - 1).ToString();
                }
                else
                {

                    Txt_Tap2_Examen.Text = "1"; // Opcional: Restablecer el valor al mínimo
                }
            }
            else
            {
                // Manejar el caso en que el texto no es un número válido.


                Txt_Tap2_Examen.Text = "1"; // Opcional: Restablecer a un valor predeterminado
            }

            CargarExamenConv();

            //CargarDgvPnl2MedConv();
            CargarFicconvOFT();

            CargarDgv_Pnl2_Querato();
            CargarExamenCont();

            //Dgv_Pnl2_medconv.Enabled = true;

        }

        private void button9_Click_2(object sender, EventArgs e)
        {
            txt_Pnl2_reti.Text = txt_Pnl2_retd.Text;
        }

        private void Dtp_Tap2_Examen_ValueChanged(object sender, EventArgs e)
        {
            DateTime fechaSeleccionada = Dtp_Tap2_FecExam.Value.Date; // Obtener solo la parte de la fecha
            DateTime fechaHoy = DateTime.Now.Date; // Obtener la fecha actual sin la hora

            if (fechaSeleccionada < fechaHoy)
            {
                BloquearCamposE(); // Llamar al método para bloquear los campos
            }
            else if (fechaSeleccionada == fechaHoy)
            {
                DesbloquearCamposE(); // Llamar al método para desbloquear los campos
            }
            // No es necesario un 'else' para cuando la fecha es mayor, ya que los campos deberían estar desbloqueados por defecto o por otra lógica.
        }

        private void guardaExamenConv()
        {//mcll




            if (validarvacioExam())
            {


                if (Txt_Tap2_Examen.Text == "0")
                {
                    Txt_Tap2_Examen.Text = numeroExamen.ToString();
                }


                // Recopilar los datos de los controles del formulario
                nuevoFicconv.COD_Sucursal = codigoSucursal;
                nuevoFicconv.CTE_CedIden = Txt_Tap1_Cedula.Text.Trim();
                nuevoFicconv.CTE_Nacio = Cbx_Tap1_Nacionalidad.Text.Trim(); // Ajusta según cómo manejas la nacionalidad

                // Recopilar los datos de los controles del formulario
                nuevoTrabajo.TCEDIDEN = Txt_Tap1_Cedula.Text.Trim();
                nuevoTrabajo.TNACIO = Cbx_Tap1_Nacionalidad.Text.Trim(); // Ajusta según cómo manejas la nacionalidad

                nuevoExamen.CTE_CedIden = Txt_Tap1_Cedula.Text.Trim();
                nuevoExamen.CTE_Nacio = Cbx_Tap1_Nacionalidad.Text.Trim(); // Ajusta según cómo manejas la nacionalidad

                nuevoExamen.FEC_Examen = Dtp_Tap2_FecExam.Value;

                // Datos de Dgv_Pnl2_medoftal           
                if (Dgv_Pnl2_conv.Rows.Count > 0)
                {


                    nuevoExamen.ESFD = Dgv_Pnl2_conv.Rows[0].Cells["Esfera"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[0].Cells["Esfera"].Value) : 0;
                    nuevoExamen.ESFI = Dgv_Pnl2_conv.Rows[1].Cells["Esfera"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[1].Cells["Esfera"].Value) : 0;


                    nuevoExamen.CILD = Dgv_Pnl2_conv.Rows[0].Cells["Cilindro"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[0].Cells["Cilindro"].Value) : 0;
                    nuevoExamen.CILI = Dgv_Pnl2_conv.Rows[1].Cells["Cilindro"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[1].Cells["Cilindro"].Value) : 0;

                    nuevoExamen.EJED = Dgv_Pnl2_conv.Rows[0].Cells["Eje"]?.Value != null ? Convert.ToInt32(Dgv_Pnl2_conv.Rows[0].Cells["Eje"].Value) : 0;
                    nuevoExamen.EJEI = Dgv_Pnl2_conv.Rows[1].Cells["Eje"]?.Value != null ? Convert.ToInt32(Dgv_Pnl2_conv.Rows[1].Cells["Eje"].Value) : 0;

                    nuevoExamen.ADDD = Dgv_Pnl2_conv.Rows[0].Cells["Adicion"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[0].Cells["Adicion"].Value) : 0;
                    nuevoExamen.ADDI = Dgv_Pnl2_conv.Rows[1].Cells["Adicion"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[1].Cells["Adicion"].Value) : 0;

                    nuevoFicconv.PRISMAD = Dgv_Pnl2_conv.Rows[0].Cells["Prisma1"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[0].Cells["Prisma1"].Value) : 0;
                    nuevoFicconv.PRISMAI = Dgv_Pnl2_conv.Rows[1].Cells["Prisma1"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[1].Cells["Prisma1"].Value) : 0;

                    
                    //nuevoExamen.CILD2 = Dgv_Pnl2_conv.Rows[1].Cells["Lejos"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[1].Cells["Lejos"].Value) : 0;
                    //nuevoExamen.CILI2 = Dgv_Pnl2_conv.Rows[1].Cells["Cerca"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[1].Cells["Cerca"].Value) : 0;
                    //nuevoExamen.OBSERVACIONES = Dgv_Pnl2_conv.Rows[1].Cells["Visual"]?.Value?.ToString();
                    nuevoExamen.ESFD2 = Dgv_Pnl2_conv.Rows[0].Cells["Prisma1"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[0].Cells["Prisma1"].Value) : 0;
                    nuevoExamen.ESFI2 = Dgv_Pnl2_conv.Rows[1].Cells["Prisma1"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[1].Cells["Prisma1"].Value) : 0;


                    nuevoTrabajo.TALTD = Convert.ToDecimal(txtAltD.Text);
                    nuevoTrabajo.TALTI = Convert.ToDecimal(txtAltI.Text);

                    //ALTD ALTI    PRISMAD PRISMAI  DPDL	DPDC	DPIL	DPIC



                    nuevoFicconv.DPDL = Dgv_Pnl2_conv.Rows[0].Cells["Lejos"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[0].Cells["Lejos"].Value) : 0;
                    nuevoFicconv.DPIL = Dgv_Pnl2_conv.Rows[1].Cells["Lejos"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[1].Cells["Lejos"].Value) : 0;


                    nuevoFicconv.DPDC = Dgv_Pnl2_conv.Rows[0].Cells["Cerca"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[0].Cells["Cerca"].Value) : 0;
                    nuevoFicconv.DPIC = Dgv_Pnl2_conv.Rows[1].Cells["Cerca"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[1].Cells["Cerca"].Value) : 0;



                    nuevoFicconv.PRISMAD = Dgv_Pnl2_conv.Rows[0].Cells["Prisma1"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[0].Cells["Prisma1"].Value) : 0;
                    nuevoFicconv.PRISMAI = Dgv_Pnl2_conv.Rows[1].Cells["Prisma1"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[1].Cells["Prisma1"].Value) : 0;


                    nuevoFicconv.ALTD = Convert.ToDecimal(txtAltD.Text);
                    nuevoFicconv.ALTI = Convert.ToDecimal(txtAltI.Text);

                    switch (Dgv_Pnl2_conv.Rows[0].Cells["Grado1"]?.Value?.ToString())
                    {
                        case "90":
                            nuevoFicconv.PBASED = "90"; // Arr
                            break;

                        case "270":
                            nuevoFicconv.PBASED = "270"; // Abj
                            break;

                        case "360":
                            nuevoFicconv.PBASED = "360"; // Nas
                            break;

                        case "180":
                            nuevoFicconv.PBASED = "180"; // Tem
                            break;

                        default:
                            nuevoFicconv.PBASED = "";
                            break;
                    }

                    switch (Dgv_Pnl2_conv.Rows[1].Cells["Grado1"]?.Value?.ToString())
                    {
                        case "90":
                            nuevoFicconv.PBASEI = "90";
                            break;

                        case "270":
                            nuevoFicconv.PBASEI = "270";
                            break;

                        case "360":
                            nuevoFicconv.PBASEI = "360";
                            break;

                        case "180":
                            nuevoFicconv.PBASEI = "180";
                            break;

                        default:
                            nuevoFicconv.PBASEI = "";
                            break;
                    }

                    //nuevoFicconv.PBASED = Dgv_Pnl2_conv.Rows[0].Cells["Grado1"]?.Value?.ToString() == "0" ? "" : Dgv_Pnl2_conv.Rows[0].Cells["Grado1"]?.Value?.ToString();
                    //nuevoFicconv.PBASEI = Dgv_Pnl2_conv.Rows[1].Cells["Grado1"]?.Value?.ToString() == "0" ? "" : Dgv_Pnl2_conv.Rows[1].Cells["Grado1"]?.Value?.ToString();


                    nuevoFicconv.AVD = Dgv_Pnl2_conv.Rows[0].Cells["VISUAL"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[0].Cells["VISUAL"].Value) : 0;
                    nuevoFicconv.AVI = Dgv_Pnl2_conv.Rows[1].Cells["VISUAL"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[1].Cells["VISUAL"].Value) : 0;


                    nuevoTrabajo.TTIPOVISIOND = cbVisionDerecha.Text;
                    nuevoTrabajo.TTIPOVISIONI = cbVisionIzquierda.Text;





                }

                nuevoExamen.OBSERVACIONES = txt_Pnl2_observa.Text.Trim();
                nuevoExamen.CodigoMimesys = txt_Pnl2_conv_mimesys.Text.Trim();
                nuevoExamen.COD_Sucursal = codigoSucursal;
                nuevoExamen.USER_CREA  = TB_USUARIO.COD_USR;
                nuevoExamen.USER_MOD = TB_USUARIO.COD_USR;
                //METOD DE GUARDARR OFT
                //nuevoFicconv.OFTD = txt_Pnl2_oftd.Text.Trim();
                //nuevoFicconv.OFTI = txt_Pnl2_ofti.Text.Trim();

                nuevoTrabajo.TSucursal = codigoSucursal;
                nuevoTrabajo.TTIPOTRABAJO = "002";
                nuevoTrabajo.USERCREA = TB_USUARIO.COD_USR;

                //nuevoTrabajo.TEXAMEN = this.Txt_Pnl2_Examen.Text;
                //if (Dgv_Pnl2_medconv.Rows.Count > 0)
                //{

                    //nuevoTrabajo.TDISTANCIAVERTICE = Dgv_Pnl2_medconv.Rows[0].Cells[0]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_medconv.Rows[0].Cells[0].Value) : 0;
                    //nuevoTrabajo.TANGULOPANTOSCOPICO = Dgv_Pnl2_medconv.Rows[0].Cells[1]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_medconv.Rows[0].Cells[1].Value) : 0;
                    //nuevoTrabajo.TANGULOFACIAL = Dgv_Pnl2_medconv.Rows[0].Cells[2]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_medconv.Rows[0].Cells[2].Value) : 0;
                    //nuevoTrabajo.TDISTANCIADELECTURA = Dgv_Pnl2_medconv.Rows[0].Cells[3]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_medconv.Rows[0].Cells[3].Value) : 0;

                    nuevoTrabajo.TDISTANCIAVERTICE = Convert.ToDecimal(txtDistVertice.Text);
                    nuevoTrabajo.TANGULOPANTOSCOPICO = Convert.ToDecimal(txtAngPant.Text);
                    nuevoTrabajo.TANGULOFACIAL = Convert.ToDecimal(txtAngFac.Text);
                    nuevoTrabajo.TDISTANCIADELECTURA = Convert.ToDecimal(txtDll.Text);

                //}
                nuevoTrabajo.TOJO = Cbx_Tap2_Ojo.Text;

                nuevoExamen.TIPOEXAMEN = Cbx_Tap2_Tipo_Examen.Text;


                if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "01" || Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "02" || Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "09")
                {
                    nuevoTrabajo.TipoExamen = Cbx_Tap2_Tipo_Examen.Text;
                }
                else
                {
                    nuevoTrabajo.TipoExamen = "";
                }
                nuevoExamen.TIPO_Optm = Cbx_Tap2_Tipo_Optome.Text;



                if (string.IsNullOrEmpty(TXT_Tap2_Nombre_Optome.Text))
                {
                    nuevoExamen.NOM_Optm = Cbx_Tap2_Nombre_Optome.Text;
                }
                else
                {
                    nuevoExamen.NOM_Optm = TXT_Tap2_Nombre_Optome.Text;
                }



                nuevoFicconv.RETD = string.IsNullOrWhiteSpace(txt_Pnl2_retd.Text) ? null : txt_Pnl2_retd.Text.Trim();
                nuevoFicconv.RETI = string.IsNullOrWhiteSpace(txt_Pnl2_reti.Text) ? null : txt_Pnl2_reti.Text.Trim();

                //L_Examen

                // Llamar al método de la capa lógica para guardar el cliente
                resultado = _L_Examen.AgregarExamen(nuevoExamen);



                _L_Ficconv.AgregarFicconv(nuevoFicconv);
                //_L_Trabajo.AgregarTrabajo(nuevoTrabajo);

            }

        }

        private void guardaExamenCont()
        {//mcll




            if (validarvacioExam())
            {
                if (Txt_Tap2_Examen.Text == "0")
                {
                    Txt_Tap2_Examen.Text = numeroExamen.ToString();
                }


                // Recopilar los datos de los controles del formulario

                // Datos de Dgv_Pnl2_medoftal           
                if (Dgv_Pnl2_cont.Rows.Count > 0)
                {

                    nuevoFiccont.ESFD = Dgv_Pnl2_cont.Rows[0].Cells["Esfera"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_cont.Rows[0].Cells["Esfera"].Value) : 0;
                    nuevoFiccont.CILD = Dgv_Pnl2_cont.Rows[0].Cells["Cilindro"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_cont.Rows[0].Cells["Cilindro"].Value) : 0;
                    nuevoFiccont.EJED = Dgv_Pnl2_cont.Rows[0].Cells["Eje"]?.Value != null ? Convert.ToInt32(Dgv_Pnl2_cont.Rows[0].Cells["Eje"].Value) : 0;
                    nuevoFiccont.ADDD = Dgv_Pnl2_cont.Rows[0].Cells["Adicion"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_cont.Rows[0].Cells["Adicion"].Value) : 0;

                    nuevoFiccont.CBD = Dgv_Pnl2_cont.Rows[0].Cells["C_BASE"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_cont.Rows[0].Cells["C_BASE"].Value) : 0;
                    nuevoFiccont.DIAMD = Dgv_Pnl2_cont.Rows[0].Cells["Diametro"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_cont.Rows[0].Cells["Diametro"].Value) : 0;



                    nuevoFiccont.ESFI = Dgv_Pnl2_cont.Rows[1].Cells["Esfera"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_cont.Rows[1].Cells["Esfera"].Value) : 0;
                    nuevoFiccont.CILI = Dgv_Pnl2_cont.Rows[1].Cells["Cilindro"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_cont.Rows[1].Cells["Cilindro"].Value) : 0;
                    nuevoFiccont.EJEI = Dgv_Pnl2_cont.Rows[1].Cells["Eje"]?.Value != null ? Convert.ToInt32(Dgv_Pnl2_cont.Rows[1].Cells["Eje"].Value) : 0;
                    nuevoFiccont.ADDI = Dgv_Pnl2_cont.Rows[1].Cells["Adicion"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_cont.Rows[1].Cells["Adicion"].Value) : 0;

                    nuevoFiccont.CBI = Dgv_Pnl2_cont.Rows[1].Cells["C_BASE"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_cont.Rows[1].Cells["C_BASE"].Value) : 0;
                    nuevoFiccont.DIAMI = Dgv_Pnl2_cont.Rows[1].Cells["Diametro"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_cont.Rows[1].Cells["Diametro"].Value) : 0;


                }

                nuevoFiccont.OBSERVACIONES = txt_Pnl2_cont_observa.Text;


                _L_Ficcont.AgregarFiccont(nuevoFiccont);
                //_L_Trabajo.AgregarTrabajo(nuevoTrabajo);

            }

        }


        private void guardaExamenreti()
        {//mcll




            if (validarvacioExam())
            {

                if (Txt_Tap2_Examen.Text == "0")
                {
                    Txt_Tap2_Examen.Text = numeroExamen.ToString();
                }

                nuevoFicconv.RETD = string.IsNullOrWhiteSpace(txt_Pnl2_retd.Text) ? null : txt_Pnl2_retd.Text.Trim();
                nuevoFicconv.RETI = string.IsNullOrWhiteSpace(txt_Pnl2_reti.Text) ? null : txt_Pnl2_reti.Text.Trim();
            }




            _L_Ficconv.AgregarFicconv(nuevoFicconv);


        }


        private void guardaExamenQuera()
        {//mcll




            if (validarvacioExam())
            {

                if (Txt_Tap2_Examen.Text == "0")
                {
                    Txt_Tap2_Examen.Text = numeroExamen.ToString();
                }

                // Recopilar los datos de los controles del formulario
                //dt.Columns.Add("Meridiano_Corneal", typeof(decimal));
                //dt.Columns.Add("Grados", typeof(decimal));
                //dt.Columns.Add("Meridiano_Corneald", typeof(decimal));
                //dt.Columns.Add("Gradosd", typeof(decimal));

                // Datos de Dgv_Pnl2_medoftal           
                if (Dgv_Pnl2_Querato.Rows[0].Cells[0].Value.ToString() != "0" || Dgv_Pnl2_Querato.Rows[0].Cells[1].Value.ToString() != "0" || Dgv_Pnl2_Querato.Rows[0].Cells[2].Value.ToString() != "0" || Dgv_Pnl2_Querato.Rows[0].Cells[3].Value.ToString() != "0"
                 || Dgv_Pnl2_Querato.Rows[1].Cells[0].Value.ToString() != "0" || Dgv_Pnl2_Querato.Rows[1].Cells[1].Value.ToString() != "0" || Dgv_Pnl2_Querato.Rows[1].Cells[2].Value.ToString() != "0" || Dgv_Pnl2_Querato.Rows[1].Cells[3].Value.ToString() != "0")
                {
                    nuevoQuerato.COD_Sucursal = codigoSucursal;
                    nuevoQuerato.QUERATOMD1 = Dgv_Pnl2_Querato.Rows[0].Cells[0]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_Querato.Rows[0].Cells[0].Value) : 0;
                    nuevoQuerato.QUERATOGD1 = Dgv_Pnl2_Querato.Rows[0].Cells[1]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_Querato.Rows[0].Cells[1].Value) : 0;
                    nuevoQuerato.QUERATOMD2 = Dgv_Pnl2_Querato.Rows[0].Cells[2]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_Querato.Rows[0].Cells[2].Value) : 0;
                    nuevoQuerato.QUERATOGD2 = Dgv_Pnl2_Querato.Rows[0].Cells[3]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_Querato.Rows[0].Cells[3].Value) : 0;

                    nuevoQuerato.QUERATOMI1 = Dgv_Pnl2_Querato.Rows[1].Cells[0]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_Querato.Rows[1].Cells[0].Value) : 0;
                    nuevoQuerato.QUERATOGI1 = Dgv_Pnl2_Querato.Rows[1].Cells[1]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_Querato.Rows[1].Cells[1].Value) : 0;
                    nuevoQuerato.QUERATOMI2 = Dgv_Pnl2_Querato.Rows[1].Cells[2]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_Querato.Rows[1].Cells[2].Value) : 0;
                    nuevoQuerato.QUERATOGI2 = Dgv_Pnl2_Querato.Rows[1].Cells[3]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_Querato.Rows[1].Cells[3].Value) : 0;

                    nuevoQuerato.QUE_OBSERV = txt_Pnl2_obsQuero.Text;


                    _L_Querato.AgregarQuerato(nuevoQuerato);

                }


                
                //_L_Trabajo.AgregarTrabajo(nuevoTrabajo);

            }

        }


        private void guardaExamenoft()
        {//mcll




            if (validarvacioExam())
            {



                //nuevoFicconv.OFTD = txt_Pnl2_oftd.Text.Trim();
                // Versión con operador ternario
                nuevoFicconv.OFTD = string.IsNullOrWhiteSpace(txt_Pnl2_oftd.Text) ? null : txt_Pnl2_oftd.Text.Trim();
                nuevoFicconv.OFTI = string.IsNullOrWhiteSpace(txt_Pnl2_ofti.Text) ? null : txt_Pnl2_ofti.Text.Trim();


                _L_Ficconv.AgregarFicconv(nuevoFicconv);
               // _L_Trabajo.AgregarTrabajo(nuevoTrabajo);

            }

        }

        private bool validarvacioExamConv()
        {
            bool todosValidos = true;

            // Validar Cbx_Tap2_Tipo_Optome
            if (Cbx_Tap2_Tipo_Optome.SelectedItem == null || string.IsNullOrEmpty(Cbx_Tap2_Tipo_Optome.Text))
            {

                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Seleccione un optometrista");
                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                _FrmMensajes.ShowDialog();

                //Pnl_2_Msj.Visible = true;
                //txt_pl2_msj.Text = "";
                //pb_pl2_mj.Visible = true;
                todosValidos = false;
                Cbx_Tap2_Tipo_Optome.Focus();
                return todosValidos; // Salir anticipadamente si este no es válido
            }

            return todosValidos; // Salir anticipadamente si este no es válido
        }


        private bool validarvacioExam()
        {
            bool todosValidos = true;



            string textoExamen = Txt_Tap2_Examen.Text.Trim(); // Obtener el texto y eliminar espacios




            // Validar Cbx_Tap2_Tipo_Optome
            if (Cbx_Tap2_Tipo_Optome.SelectedItem == null || string.IsNullOrEmpty(Cbx_Tap2_Tipo_Optome.Text))
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Seleccione un optometrista");
                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                _FrmMensajes.ShowDialog();


                todosValidos = false;
                Cbx_Tap2_Tipo_Optome.Focus();
                return todosValidos; // Salir anticipadamente si este no es válido
            }
            // borrar 

            //Validar Cbx_Tap2_Tipo_Optome si se esta creando uno nuevo
            if (Txt_Tap2_Examen.Text == "0" & Cbx_Tap2_Tipo_Optome.SelectedItem?.ToString() != "EXTERNO")
            {
                // Validar Cbx_Tap2_Nombre_Optome
                if (Cbx_Tap2_Nombre_Optome.SelectedItem == null || string.IsNullOrEmpty(Cbx_Tap2_Nombre_Optome.Text))
                {

                    Pnl_2_Msj.Visible = true;
                    txt_pl2_msj.Text = "Seleccione un nombre de optometría";
                    //pb_pl2_mj.Visible = true;

                    todosValidos = false;
                    Cbx_Tap2_Nombre_Optome.Focus();
                    return todosValidos; // Salir anticipadamente si este no es válido
                }
            }
            else if (Txt_Tap2_Examen.Text == "0" & Cbx_Tap2_Tipo_Optome.SelectedItem?.ToString() == "EXTERNO" & string.IsNullOrEmpty(TXT_Tap2_Nombre_Optome.Text))
            {

                Pnl_2_Msj.Visible = true;
                txt_pl2_msj.Text = "Debe llenar un Nombre de optometría";
                //pb_pl2_mj.Visible = true;
                todosValidos = false;
                TXT_Tap2_Nombre_Optome.Focus();
                return todosValidos; // Salir anticipadamente si este no es válido
            }

            // Validar Cbx_Tap2_Tipo_Examen
            if (Cbx_Tap2_Tipo_Examen.SelectedItem == null || string.IsNullOrEmpty(Cbx_Tap2_Tipo_Examen.Text))
            {

                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Seleccione un tipo de examen");
                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                _FrmMensajes.ShowDialog();


                todosValidos = false;
                Cbx_Tap2_Tipo_Examen.Focus();
                return todosValidos; // Salir anticipadamente si este no es válido
            }

            // Validar Cbx_Tap2_Ojo
            //if (Cbx_Tap2_Ojo.SelectedItem == null || string.IsNullOrEmpty(Cbx_Tap2_Ojo.Text))
            //{

            //    Pnl_2_Msj.Visible = true;
            //    txt_pl2_msj.Text = " Seleccionar un valor para  ojo";
            //    //pb_pl2_mj.Visible = true;

            //    todosValidos = false;
            //    Cbx_Tap2_Ojo.Focus();
            //    return todosValidos; // Salir anticipadamente si este no es válido
            //}

            return todosValidos; // Salir anticipadamente si este no es válido
        }

        private void Btn_Tap2_oft_ambos_Click_1(object sender, EventArgs e)
        {
            txt_Pnl2_ofti.Text = txt_Pnl2_oftd.Text;
        }

        private void Btn_Tap2_GuardarExam_Click(object sender, EventArgs e)
        {

            CancelarPorCambioExamen();

            mantenervacio = false;

            // First, perform the validation
            //if (!ValidarVisionConv())
            //{
            //    Pnl_2_Msj.Visible = true;
            //    txt_pl2_msj.Text = "Debe colocar tipos de visión validos La combinación posible es el mismo tipo de visión o Balance ";
            //    //pb_pl2_mj.Visible = true;
            //    Pnl_2_Msj.Location = new Point(396, 175);
            //    Pnl_2_Msj.BringToFront();

            //    return;
            //}



            if (!ValidarQueratometria())
            {
                Pnl_2_Msj.Visible = true;
                txt_pl2_msj.Text = "Valores de Queratometria Incompletos";
                //pb_pl2_mj.Visible = true;
                return;
            }

            if (string.IsNullOrWhiteSpace(Txt_Tap2_Examen.Text))
            {
                // If it's empty, set its value to "0"
                Txt_Tap2_Examen.Text = "0";
            }

            if (int.Parse(Txt_Tap2_Examen.Text) == 0)
            {

                Txt_Tap2_Examen.Text = (TopeExamen + 1).ToString();
                TopeExamen = TopeExamen + 1;
                numeroExamen = TopeExamen;
            }
            else
            { numeroExamen = int.Parse(Txt_Tap2_Examen.Text); }


            nuevoExamen.NUM_Examen = numeroExamen;

            nuevoFicconv.NUM_Examen = numeroExamen;
            nuevoFiccont.NUM_Examen = numeroExamen;

            nuevoQuerato.NUM_Examen = numeroExamen;
            nuevoTrabajo.TEXAMEN = Txt_Tap2_Examen.Text;

            nuevoFiccont.COD_Sucursal = codigoSucursal;
            nuevoFiccont.CTE_CedIden = Txt_Tap1_Cedula.Text.Trim();
            nuevoFiccont.CTE_Nacio = Cbx_Tap1_Nacionalidad.Text.Trim(); // Ajusta según cómo manejas la nacionalidad


            nuevoFicconv.CTE_CedIden = Txt_Tap1_Cedula.Text.Trim();
            nuevoFicconv.CTE_Nacio = Cbx_Tap1_Nacionalidad.Text.Trim(); // Ajusta según cómo manejas la nacionalidad

            nuevoExamen.CTE_CedIden = Txt_Tap1_Cedula.Text.Trim();
            nuevoExamen.CTE_Nacio = Cbx_Tap1_Nacionalidad.Text.Trim(); // Ajusta según cómo manejas la nacionalidad

            nuevoQuerato.CTE_CedIden = Txt_Tap1_Cedula.Text.Trim();
            nuevoQuerato.CTE_Nacio = Cbx_Tap1_Nacionalidad.Text.Trim(); // Ajusta según cómo manejas la nacionalidad

            nuevoTrabajo.TCEDIDEN = Txt_Tap1_Cedula.Text.Trim();
            nuevoTrabajo.TNACIO = Cbx_Tap1_Nacionalidad.Text.Trim(); // Ajusta según cómo manejas la nacionalidad


            nuevoTrabajo.TFECCREA = Dtp_Tap2_FecExam.Value;
            nuevoExamen.FEC_Examen = Dtp_Tap2_FecExam.Value;
            //nuevoExamen.FEC_Examen = Dtp_Tap2_Examen.Value;
            //nuevoExamen.FEC_Examen = Dtp_Tap2_Examen.Value;
            //nuevoExamen.FEC_Examen = Dtp_Tap2_Examen.Value;

            // Continuar con el resto de la lógica de tu método


            if (validarvacioExam())
            {

                guardaExamenConv();
                // Procesar el resultado de la operación de guardado
                if (resultado.Equals("Guardado"))
                {

                    if (Txt_Tap2_Examen.Text == "0")
                    {
                        Txt_Tap2_Examen.Text = numeroExamen.ToString();
                    }

                }

                //if (!ValidarCont_AllOrNoneZero())
                //{

                //    //Pnl_2_Msj.Visible = true;
                //    //txt_pl2_msj.Text = " Error  valores de la tabla de lentes de contacto";
                //    ////pb_pl2_mj.Visible = true;
                //    //return; // Stop further processing if validation fails
                //}



                guardaExamenCont();

                ValidarQueratometria();

                guardaExamenQuera();
                guardaExamenreti();
                guardaExamenoft();



                if (resultado.Equals("Guardado"))
                {

                    //Pnl_2_Msj.Visible = true;
                    //txt_pl2_msj.Text = "Examen guardado con éxito ";
                    //pb_pl2_mj.Visible = false;
                    if (tabControl.SelectedIndex == 1 && !Formulario_ListaOrdenes)
                    {
                        btnCargarOrden.Enabled = true;
                        //btnCargarOrden.Focus();
                        tabControl.SelectedIndex = 2;
                        
                    }
                    else
                    {
                        btnExamen.Focus();
                        tabControl.SelectedIndex = 1;
                       
                    }
                    //btnCargarOrden.Enabled = true;
                    //tabControl.SelectedIndex = 2;
                    AgregarRx();
                }

            }
            //nuevoTrabajo.TEXAMEN = Txt_Tap2_Examen.Text;
            LLenar_TbTrabajo();
            _L_Trabajo.AgregarTrabajo(nuevoTrabajo);

        }

        private bool ValidarCont_AllOrNoneZero()
        {
            // Define the names of the numeric columns to be validated
            // 'Ojo' and 'aEsfera', 'aCilindro' are text/placeholder columns and should be excluded.
            string[] numericColumns = { "Esfera", "Cilindro", "Eje", "Adicion", "C_base", "Diametro" };

            foreach (DataGridViewRow row in Dgv_Pnl2_cont.Rows)
            {
                // Skip the new row if it's present and not committed
                if (row.IsNewRow)
                {
                    continue;
                }

                bool hasNonZeroValue = false;
                bool hasZeroValue = false;
                List<int> zeroValueColumnIndexes = new List<int>(); // To highlight specific zero cells if needed

                // Iterate only through the relevant numeric columns
                foreach (string colName in numericColumns)
                {
                    if (Dgv_Pnl2_cont.Columns.Contains(colName))
                    {
                        DataGridViewCell cell = row.Cells[colName];

                        // Ensure the cell has a value and it can be parsed as a decimal
                        if (cell.Value != null && decimal.TryParse(cell.Value.ToString(), out decimal cellValue))
                        {
                            if (cellValue != 0)
                            {
                                hasNonZeroValue = true;
                            }
                            else
                            {
                                hasZeroValue = true;
                                zeroValueColumnIndexes.Add(cell.ColumnIndex);
                            }
                        }
                        else
                        {
                            // Handle cases where a numeric cell might be empty or invalid (e.g., text)
                            // Depending on your exact requirements, you might treat empty as zero, or as an error.
                            // For this rule, we'll assume an empty/invalid numeric cell should be considered zero-like for the "all or none" rule.
                            hasZeroValue = true;
                            zeroValueColumnIndexes.Add(cell.ColumnIndex);
                        }
                    }
                }

                // Apply the validation rule: if there's a non-zero value, there shouldn't be any zero values
                if (hasNonZeroValue && hasZeroValue)
                {
                    // Construct a more specific message about which row has the issue
                    string ojoValue = row.Cells["Ojo"].Value?.ToString() ?? "Desconocido";
                    string errorMessage = $"En la fila '{ojoValue}', se detectó un valor diferente de cero, pero también hay valores en cero. Todos los campos numéricos deben tener un valor o todos deben ser cero.";
                    ShowValidationMessage(errorMessage, Dgv_Pnl2_cont, zeroValueColumnIndexes.Any() ? zeroValueColumnIndexes.First() : 0, row.Index);
                    return false; // Validation failed for this row
                }
            }

            // If no validation errors were found after checking all rows
            HideValidationMessage();
            return true; // All rows passed the validation
        }

        private void ShowValidationMessage(string message, DataGridView dgv, int colIndex, int rowIndex)
        {
            Pnl_2_Msj.Visible = true;
            txt_pl2_msj.Text = message;
            //pb_pl2_mj.Visible = true;

            // Optional: Scroll to the error cell and select it
            if (rowIndex >= 0 && rowIndex < dgv.Rows.Count && colIndex >= 0 && colIndex < dgv.Columns.Count)
            {
                dgv.CurrentCell = dgv.Rows[rowIndex].Cells[colIndex];
                dgv.BeginEdit(true); // Put cell in edit mode if it's editable
            }
        }

        // Helper method to hide the validation message
        private void HideValidationMessage()
        {
            Pnl_2_Msj.Visible = false;
            txt_pl2_msj.Text = string.Empty;
            //pb_pl2_mj.Visible = false;
        }

        private bool ValidarVisionConv()
        {
            if (Cbx_Tap2_Ojo.Text == "Ambos" && cbVisionDerecha.Text != cbVisionIzquierda.Text && cbVisionIzquierda.Text != "Balance" && cbVisionDerecha.Text != "Balance")
            {
                ////Pnl_2_Msj.Visible = true;
                ////txt_pl2_msj.Text = "Debe colocar tipos de visión validos La combinación posible es el mismo tipo de visión o Balance";
                //////pb_pl2_mj.Visible = true;
                //////Pnl_2_Msj.Location = new Point(396, 175);
                ////Pnl_2_Msj.BringToFront();

                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Debe colocar tipos de visión validos La combinación posible es el mismo tipo de visión o Balance");
                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                _FrmMensajes.ShowDialog();

                // Do NOT set cellFila1.Value here, as this would trigger CellValueChanged again.
                // Instead, return false to indicate validation failure.
                return false;
            }

            if (Cbx_Tap2_Ojo.Text == "Ambos" && cbVisionDerecha.Text == "Balance" && cbVisionDerecha.Text == "Balance")
            {
                //Pnl_2_Msj.Visible = true;
                //txt_pl2_msj.Text = "No se puede colocar Balance en ambos ojos";
                ////pb_pl2_mj.Visible = true;
                ////Pnl_2_Msj.Location = new Point(396, 175);
                //Pnl_2_Msj.BringToFront();

                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Debe colocar tipos de visión validos La combinación posible es el mismo tipo de visión o Balance");
                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                _FrmMensajes.ShowDialog();

                // Do NOT set cellFila1.Value here, as this would trigger CellValueChanged again.
                // Instead, return false to indicate validation failure.
                return false;
            }

            //// This assumes you want to check all columns named "Vision" for this rule
            //// If you only want to check a specific column (e.g., the one at index X), adjust the loop.
            //foreach (DataGridViewColumn column in Dgv_Pnl2_conv.Columns)
            //{
            //    if (column.Name == "Vision")
            //    {
            //        int columnIndex = column.Index;

            //        // Ensure there are at least two rows to compare (row 0 and row 1)
            //        if (Dgv_Pnl2_conv.Rows.Count >= 2)
            //        {
            //            DataGridViewCell cellFila0 = Dgv_Pnl2_conv.Rows[0].Cells[columnIndex];
            //            DataGridViewComboBoxCell cellFila1 = Dgv_Pnl2_conv.Rows[1].Cells[columnIndex] as DataGridViewComboBoxCell;

            //            // Make sure cellFila1 is indeed a DataGridViewComboBoxCell and not null
            //            if (cellFila1 == null)
            //            {
            //                // Handle cases where the cell might not be a ComboBoxCell, though it should be if configured correctly.
            //                // Or, you might want to log this as an unexpected scenario.
            //                continue;
            //            }

            //            string valorFila0 = cellFila0.Value?.ToString().Trim().ToUpper();
            //            string valorFila1 = cellFila1.Value?.ToString().Trim().ToUpper();

            //            if (valorFila0 != valorFila1 && valorFila1 != "BALANCE" && valorFila0 != "BALANCE")
            //            {
            //                Pnl_2_Msj.Visible = true;
            //                txt_pl2_msj.Text = "Debe colocar tipos de visión validos La combinación posible es el mismo tipo de visión o Balance";
            //                //pb_pl2_mj.Visible = true;
            //                //Pnl_2_Msj.Location = new Point(396, 175);
            //                Pnl_2_Msj.BringToFront();

            //                // Do NOT set cellFila1.Value here, as this would trigger CellValueChanged again.
            //                // Instead, return false to indicate validation failure.
            //                return false;
            //            }
            //        }
            //    }
            //}


            return true; // Validation passed
        }

        private void Dgv_Pnl2_conv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            if (_isCellValueChanging)
                return;

            try
            {
                _isCellValueChanging = true;

                if (e.ColumnIndex >= 0 && Dgv_Pnl2_conv.Columns[e.ColumnIndex].Name == "Vision" && e.RowIndex == 1)
                {
                    DataGridViewCell cellFila0 = Dgv_Pnl2_conv.Rows[0].Cells[e.ColumnIndex];
                    DataGridViewComboBoxCell cellFila1 = (DataGridViewComboBoxCell)Dgv_Pnl2_conv.Rows[1].Cells[e.ColumnIndex];

                    string valorFila0 = cellFila0.Value?.ToString().Trim().ToUpper();
                    string valorFila1 = cellFila1.Value?.ToString().Trim().ToUpper();

                    if (valorFila0 != valorFila1 && valorFila1 != "BALANCE" && valorFila0 != "BALANCE")
                    {
                        Pnl_2_Msj.Visible = true;
                        txt_pl2_msj.Text = "Debe colocar tipos de visión validos La combinación posible es el mismo tipo de visión o Balance";
                        //pb_pl2_mj.Visible = true;

                        cellFila1.Value = null; // O el valor que desees restablecer
                    }
                    else
                    {
                        if (Pnl_2_Msj.Visible)
                        {
                            Pnl_2_Msj.Visible = false;
                            txt_pl2_msj.Text = string.Empty;
                            //pb_pl2_mj.Visible = false;
                        }
                    }
                }
            }
            finally
            {
                _isCellValueChanging = false;
            }


            if (e.ColumnIndex > 0 && Dgv_Pnl2_conv.Columns[e.ColumnIndex].Name == "Esfera" && e.RowIndex >= 0)
            {
                DataGridViewCell cell = Dgv_Pnl2_conv.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Verifica si la celda está vacía (Value es null o una cadena vacía)
                if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    cell.Value = 0; // Establece el valor a cero
                }
                else
                {
                    // Si la celda no está vacía, valida el número ingresado
                    if (double.TryParse(cell.Value.ToString(), out double enteredValue))
                    {
                        // Calcula el residuo de la división por 0.25
                        double remainder = enteredValue % 0.25;

                        // Define una pequeña tolerancia para evitar problemas de coma flotante
                        double tolerance = 0.0000000001;

                        // Si no es divisible por 0.25 (o el residuo no es cercano a cero)
                        if (Math.Abs(remainder) > tolerance && Math.Abs(remainder - 0.25) > tolerance)
                        {
                            // Calcula el número más cercano que es múltiplo de 0.25
                            double roundedValue = Math.Round(enteredValue / 0.25) * 0.25;


                            //Pnl_2_Msj.Visible = true;
                            //txt_pl2_msj.Text = "El valor ingresado debe ser un múltiplo de 0.25 Se ha ajustado a " + roundedValue.ToString("F2");
                            //pb_pl2_mj.Visible = true;


                            // Actualiza el valor de la celda
                            cell.Value = roundedValue;


                        }
                    }
                    else
                    {
                        // Si el valor no es un número válido, puedes manejarlo aquí (por ejemplo, establecerlo a 0 o mostrar un error)
                        MessageBox.Show("Por favor, ingrese un número válido.", "Error de entrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cell.Value = 0; // O la acción que consideres adecuada
                    }
                }
            }

            if (e.ColumnIndex >= 0 && Dgv_Pnl2_conv.Columns[e.ColumnIndex].Name == "Cilindro" && e.RowIndex >= 0)
            {
                DataGridViewCell cell = Dgv_Pnl2_conv.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Verifica si la celda está vacía (Value es null o una cadena vacía)
                if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    cell.Value = 0; // Establece el valor a cero
                }
                else
                {
                    // Si la celda no está vacía, valida el número ingresado
                    if (double.TryParse(cell.Value.ToString(), out double enteredValue))
                    {
                        // Calcula el residuo de la división por 0.25
                        double remainder = enteredValue % 0.25;

                        // Define una pequeña tolerancia para evitar problemas de coma flotante
                        double tolerance = 0.0000000001;

                        // Si no es divisible por 0.25 (o el residuo no es cercano a cero)
                        if (Math.Abs(remainder) > tolerance && Math.Abs(remainder - 0.25) > tolerance)
                        {
                            // Calcula el número más cercano que es múltiplo de 0.25
                            double roundedValue = Math.Round(enteredValue / 0.25) * 0.25;



                            //Pnl_2_Msj.Visible = true;
                            //txt_pl2_msj.Text = "El valor ingresado debe ser un múltiplo de 0.25 Se ha ajustado a " + roundedValue.ToString("F2");
                            //pb_pl2_mj.Visible = true;


                            // Actualiza el valor de la celda
                            cell.Value = roundedValue;


                        }
                    }
                    else
                    {
                        // Si el valor no es un número válido, puedes manejarlo aquí (por ejemplo, establecerlo a 0 o mostrar un error)
                        MessageBox.Show("Por favor, ingrese un número válido.", "Error de entrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cell.Value = 0; // O la acción que consideres adecuada
                    }
                }
            }

            if (e.ColumnIndex >= 0 && Dgv_Pnl2_conv.Columns[e.ColumnIndex].Name == "Eje" && e.RowIndex >= 0)
            {
                DataGridViewCell cell = Dgv_Pnl2_conv.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Verifica si la celda está vacía
                if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    cell.Value = 0; // Establece el valor a cero
                }
                else
                {
                    int valoreje = Convert.ToInt32(cell.Value);
                    // Si la celda no está vacía, valida el número ingresado como entero
                    if (int.TryParse(valoreje.ToString(), out int enteredValue))
                    {
                        // Verifica si el valor es un múltiplo de 5
                        if (enteredValue % 5 != 0)
                        {
                            // Calcula el múltiplo de 5 más cercano
                            int roundedValue = (int)Math.Round((double)enteredValue / 5) * 5;

                            //// Muestra un mensaje al usuario
                            //Pnl_2_Msj.Visible = true;
                            //txt_pl2_msj.Text = $" El rango valido va desde 0,75 y 3,50  ";
                            ////pb_pl2_mj.Visible = true;

                            // Actualiza el valor de la celda
                            cell.Value = roundedValue;
                        }
                    }
                    else
                    {
                        // Si el valor no es un entero válido
                        //MessageBox.Show("Por favor, ingrese un número entero válido.", "Error de entrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cell.Value = 0; // O la acción que consideres adecuada
                    }
                }
            }

            if (e.ColumnIndex >= 0 && Dgv_Pnl2_conv.Columns[e.ColumnIndex].Name == "Adicion" && e.RowIndex >= 0)
            {
                DataGridViewCell cell = Dgv_Pnl2_conv.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Verifica si la celda está vacía (Value es null o una cadena vacía)
                if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    cell.Value = 0; // Establece el valor a cero
                }
                else
                {
                    // Si la celda no está vacía, valida el número ingresado
                    if (double.TryParse(cell.Value.ToString(), out double enteredValue))
                    {
                        // Calcula el residuo de la división por 0.25
                        double remainder = enteredValue % 0.25;

                        // Define una pequeña tolerancia para evitar problemas de coma flotante
                        double tolerance = 0.0000000001;

                        // Si no es divisible por 0.25 (o el residuo no es cercano a cero)
                        if (Math.Abs(remainder) > tolerance && Math.Abs(remainder - 0.25) > tolerance)
                        {
                            // Calcula el número más cercano que es múltiplo de 0.25
                            double roundedValue = Math.Round(enteredValue / 0.25) * 0.25;


                            //Pnl_2_Msj.Visible = true;
                            //txt_pl2_msj.Text = "El valor ingresado debe ser un múltiplo de 0.25 Se ha ajustado a " + roundedValue.ToString("F2");
                            ////pb_pl2_mj.Visible = true;


                            // Actualiza el valor de la celda
                            cell.Value = roundedValue;


                        }
                    }
                    else
                    {
                        // Si el valor no es un número válido, puedes manejarlo aquí (por ejemplo, establecerlo a 0 o mostrar un error)
                        MessageBox.Show("Por favor, ingrese un número válido.", "Error de entrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cell.Value = 0; // O la acción que consideres adecuada
                    }
                }
            }

            if (e.ColumnIndex >= 0 && Dgv_Pnl2_conv.Columns[e.ColumnIndex].Name == "Prisma1" && e.RowIndex >= 0)
            {
                DataGridViewCell cell = Dgv_Pnl2_conv.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Verifica si la celda está vacía (Value es null o una cadena vacía)
                if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    cell.Value = 0; // Establece el valor a cero
                }
                else
                {
                    // Si la celda no está vacía, valida el número ingresado
                    if (double.TryParse(cell.Value.ToString(), out double enteredValue))
                    {
                        // Calcula el residuo de la división por 0.25
                        double remainder = enteredValue % 0.25;

                        // Define una pequeña tolerancia para evitar problemas de coma flotante
                        double tolerance = 0.0000000001;

                        // Si no es divisible por 0.25 (o el residuo no es cercano a cero)
                        if (Math.Abs(remainder) > tolerance && Math.Abs(remainder - 0.25) > tolerance)
                        {
                            // Calcula el número más cercano que es múltiplo de 0.25
                            double roundedValue = Math.Round(enteredValue / 0.25) * 0.25;


                            //Pnl_2_Msj.Visible = true;
                            //txt_pl2_msj.Text = "El valor invalido rango de decimales 00,25,50,75 Se ha ajustado a " + roundedValue.ToString("F2");
                            //pb_pl2_mj.Visible = true;


                            // Actualiza el valor de la celda
                            cell.Value = roundedValue;


                        }
                    }
                    else
                    {
                        // Si el valor no es un número válido, puedes manejarlo aquí (por ejemplo, establecerlo a 0 o mostrar un error)
                        MessageBox.Show("Por favor, ingrese un número válido.", "Error de entrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cell.Value = 0; // O la acción que consideres adecuada
                    }
                }
            }

            //VISION - BALANCE

            // Verificar si el cambio ocurrió en una fila válida y en la columna "Lejos"
            if (e.RowIndex >= 0 && Dgv_Pnl2_conv.Columns[e.ColumnIndex].Name == "Lejos")
            {
                // Obtener el valor de la celda "Lejos" que cambió
                if (Dgv_Pnl2_conv.Rows[e.RowIndex].Cells["Lejos"].Value != null &&
                    decimal.TryParse(Dgv_Pnl2_conv.Rows[e.RowIndex].Cells["Lejos"].Value.ToString(), out decimal lejosValue))
                {

                    // Calcular el valor para la columna "Cerca"
                    decimal cercaValue = 0;
                    if (lejosValue > 0)
                    {
                        // Calcular el valor para la columna "Cerca"
                        cercaValue = lejosValue - 1;

                        // Verificar si la columna "Cerca" existe y actualizar su valor en la misma fila
                        if (Dgv_Pnl2_cont.Columns.Contains("Cerca"))
                        {
                            Dgv_Pnl2_cont.Rows[e.RowIndex].Cells["Cerca"].Value = cercaValue;
                        }
                    }

                    // Verificar si la columna "Cerca" existe y actualizar su valor en la misma fila
                    if (Dgv_Pnl2_conv.Columns.Contains("Cerca"))
                    {
                        Dgv_Pnl2_conv.Rows[e.RowIndex].Cells["Cerca"].Value = cercaValue;
                    }
                }
            }

            // Verifica que el índice de la fila sea válido
            if (e.RowIndex >= 0)
            {
                // Llama al evento CellEnter, pasando los mismos sender y argumentos
                DataGridViewCell changedCell = Dgv_Pnl2_conv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                // Obtiene la celda que cambió
                if (changedCell.Value != null && changedCell.Value.ToString() != string.Empty)
                // Verifica que el valor no sea nulo ni vacío
                {
                    Dgv_Pnl2_conv_CellEnter(sender, e);
                }


                // Verifica si el cambio ocurrió en la columna "Grado1"
                if (Dgv_Pnl2_conv.Columns[e.ColumnIndex].Name == "Grado1")
                {
                    DataGridViewCell cell = Dgv_Pnl2_conv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    if (cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                    {
                        if (int.TryParse(cell.Value.ToString(), out int valorIngresado))
                        {
                            if (valorIngresado != 0 && valorIngresado != 90 && valorIngresado != 180 && valorIngresado != 270)
                            {

                                //Pnl_2_Msj.Visible = true;
                                //txt_pl2_msj.Text = "Ingrese solo los valores permitidos: 0, 90, 180, 270";
                                ////pb_pl2_mj.Visible = true;


                                cell.Value = 0; // Ejemplo: Revertir a 0
                                // cell.Value = null; // Ejemplo: Limpiar la celda
                            }
                        }
                        else
                        {
                            MessageBox.Show("  ingrese un número válido.", "Importante", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            // Opcionalmente, puedes revertir el valor o limpiar la celda
                            cell.Value = 0;
                            //cell.Value = null;
                        }
                    }
                    // Si la celda está vacía, puedes decidir si eso es válido o no
                    // else
                    // {
                    //     // Manejar celdas vacías si es necesario
                    // }
                }// Verifica si el cambio ocurrió en la columna "Grado1"


                // Validación de Cerca <= Lejos
                if (Dgv_Pnl2_conv.Columns[e.ColumnIndex].Name == "Cerca" || Dgv_Pnl2_conv.Columns[e.ColumnIndex].Name == "Lejos")
                {
                    decimal lejosValue = 0;
                    decimal cercaValue = 0;

                    // Intenta obtener los valores de las celdas "Lejos" y "Cerca"
                    if (Dgv_Pnl2_conv.Rows[e.RowIndex].Cells["Lejos"].Value != null &&
                        decimal.TryParse(Dgv_Pnl2_conv.Rows[e.RowIndex].Cells["Lejos"].Value.ToString(), out lejosValue))
                    {
                        if (Dgv_Pnl2_conv.Rows[e.RowIndex].Cells["Cerca"].Value != null &&
                           decimal.TryParse(Dgv_Pnl2_conv.Rows[e.RowIndex].Cells["Cerca"].Value.ToString(), out cercaValue))
                        {
                            if (cercaValue > lejosValue)
                            {


                                Pnl_2_Msj.Visible = true;
                                txt_pl2_msj.Text = "El valor de 'Cerca' no puede ser mayor que el valor de 'Lejos'";
                                //pb_pl2_mj.Visible = true;

                                Dgv_Pnl2_conv.Rows[e.RowIndex].Cells["Cerca"].Value = lejosValue; // Restablece el valor de "Cerca" a "Lejos"
                            }
                        }
                    }
                }
            }
        }

        private void Dgv_Pnl2_conv_CellEnter(object sender, DataGridViewCellEventArgs e)
        {





            //if (e.ColumnIndex >= 0 && Dgv_Pnl2_conv.Columns[e.ColumnIndex].Name == "Altura" && e.RowIndex >= 0)
            //{
            //    DataGridViewCell cell = Dgv_Pnl2_conv.Rows[e.RowIndex].Cells[e.ColumnIndex];

            //    // Verifica si la celda está vacía (Value es null o una cadena vacía)
            //    if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
            //    {
            //        cell.Value = 0; // Establece el valor a cero
            //    }
            //}


            if (e.ColumnIndex >= 0 && Dgv_Pnl2_conv.Columns[e.ColumnIndex].Name == "Cilindro")
            {
                DataGridViewCell cell = Dgv_Pnl2_conv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    cell.Value = 0;
                }
            }


            if (e.ColumnIndex >= 0 && Dgv_Pnl2_conv.Columns[e.ColumnIndex].Name == "Eje")
            {
                DataGridViewCell cell = Dgv_Pnl2_conv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    cell.Value = 0;
                }
            }

            //dt.Columns.Add("Adicion", typeof(decimal));
            //dt.Columns.Add("DNP_Lejos", typeof(decimal));
            //dt.Columns.Add("DPN_Cerca", typeof(decimal));
            //dt.Columns.Add("Agudeza_Visual", typeof(string));
            //dt.Columns.Add("Prisma1", typeof(decimal));
            //dt.Columns.Add("Grado1", typeof(decimal));

            if (e.ColumnIndex >= 0 && Dgv_Pnl2_conv.Columns[e.ColumnIndex].Name == "Adicion")
            {
                DataGridViewCell cell = Dgv_Pnl2_conv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    cell.Value = 0;
                }
            }

            if (e.ColumnIndex >= 0 && Dgv_Pnl2_conv.Columns[e.ColumnIndex].Name == "Lejos")
            {
                DataGridViewCell cell = Dgv_Pnl2_conv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    cell.Value = 0;
                }
            }

            if (e.ColumnIndex >= 0 && Dgv_Pnl2_conv.Columns[e.ColumnIndex].Name == "Cerca")
            {
                DataGridViewCell cell = Dgv_Pnl2_conv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    cell.Value = 0;
                }
            }

            if (e.ColumnIndex >= 0 && Dgv_Pnl2_conv.Columns[e.ColumnIndex].Name == "Visual")
            {
                DataGridViewCell cell = Dgv_Pnl2_conv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    cell.Value = 0;
                }
            }


            if (e.ColumnIndex >= 0 && Dgv_Pnl2_conv.Columns[e.ColumnIndex].Name == "Prisma1")
            {
                DataGridViewCell cell = Dgv_Pnl2_conv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    cell.Value = 0;
                }
            }

            if (e.ColumnIndex >= 0 && Dgv_Pnl2_conv.Columns[e.ColumnIndex].Name == "Grado1")
            {
                DataGridViewCell cell = Dgv_Pnl2_conv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    cell.Value = 0;
                }
            }

            // Verifica si la celda que recibió el foco está en la columna "Cilindro"
            if (Dgv_Pnl2_conv.Columns[e.ColumnIndex].Name == "Cilindro" && e.RowIndex >= 0)
            {
                // Borra el valor de la celda correspondiente en la columna "aEsfera"
                if (Dgv_Pnl2_conv.Columns.Contains("aCilindro"))
                {
                    Dgv_Pnl2_conv.Rows[e.RowIndex].Cells["aCilindro"].Value = string.Empty;
                }
                else
                {
                    Console.WriteLine("Error: La columna 'A0' no se encuentra en el DataGridView.");
                }
            }


            // Verifica si la celda que recibió el foco está en la columna "Cilindro"
            if (Dgv_Pnl2_conv.Columns[e.ColumnIndex].Name == "Cilindro" && e.RowIndex >= 0)
            {
                int rowIndex = e.RowIndex;
                object cellValue = Dgv_Pnl2_conv.Rows[rowIndex].Cells["Cilindro"].Value;
                decimal cilindroValue;

                if (cellValue != null && decimal.TryParse(cellValue.ToString(), out cilindroValue))
                {
                    // **Lógica de actualización de la columna "aEsfera" (copiada y adaptada)**
                    if (Dgv_Pnl2_conv.Columns.Contains("aCilindro"))
                    {
                        DataGridViewCell a1Cell = Dgv_Pnl2_conv.Rows[rowIndex].Cells["aCilindro"];
                        if (cilindroValue > 0)
                        {
                            a1Cell.Value = "+";
                        }
                        else
                        {
                            a1Cell.Value = "-";
                        }

                        if (cilindroValue == 0)
                        {

                            a1Cell.Value = "";
                        }


                    }
                    else
                    {
                        Console.WriteLine("Error: La columna 'A0' no se encuentra en el DataGridView.");
                    }
                }
                else
                {
                    // Manejar el caso en que el valor de "Cilindro" no es un número válido o está vacío
                    if (Dgv_Pnl2_conv.Columns.Contains("aCilindro"))
                    {
                        Dgv_Pnl2_conv.Rows[rowIndex].Cells["aCilindro"].Value = string.Empty;
                    }
                }
            }
            // ------------------------------------------------------



            if (Dgv_Pnl2_conv.Columns[e.ColumnIndex].Name == "Esfera" && e.RowIndex >= 0)
            {
                int rowIndex = e.RowIndex;
                object cellValue = Dgv_Pnl2_conv.Rows[rowIndex].Cells["Esfera"].Value;
                decimal cilindroValue;

                if (cellValue != null && decimal.TryParse(cellValue.ToString(), out cilindroValue))
                {
                    // **Lógica de actualización de la columna "aEsfera" (copiada y adaptada)**
                    if (Dgv_Pnl2_conv.Columns.Contains("aEsfera"))
                    {
                        DataGridViewCell a1Cell = Dgv_Pnl2_conv.Rows[rowIndex].Cells["aEsfera"];
                        if (cilindroValue > 0)
                        {
                            a1Cell.Value = "+";
                        }
                        else
                        {
                            a1Cell.Value = "-";
                        }

                        if (cilindroValue == 0)
                        {

                            a1Cell.Value = "";
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: La columna 'A1' no se encuentra en el DataGridView.");
                    }
                }
                else
                {
                    // Manejar el caso en que el valor de "Cilindro" no es un número válido o está vacío
                    if (Dgv_Pnl2_conv.Columns.Contains("aEsfera"))
                    {
                        Dgv_Pnl2_conv.Rows[rowIndex].Cells["aEsfera"].Value = string.Empty;
                    }
                }
            }
        }

        private void Dgv_Pnl2_conv_CellLeave(object sender, DataGridViewCellEventArgs e)
        {



            // Llama al evento CellEnter, pasando los mismos sender y argumentos
            DataGridViewCell changedCell = Dgv_Pnl2_conv.Rows[e.RowIndex].Cells[e.ColumnIndex]; // Obtiene la celda que cambió
            if (changedCell.Value != null && changedCell.Value.ToString() != string.Empty) // Verifica que el valor no sea nulo ni vacío
            {
                Dgv_Pnl2_conv_CellEnter(sender, e);
            }
        }
        private void Dgv_Pnl2_cont_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (Dgv_Pnl2_cont.Columns[e.ColumnIndex].Name == "Diametro")
            {
                if (decimal.TryParse(e.FormattedValue.ToString(), out decimal val))
                {
                    if (val != 0M && (val < 8.5M || val > 14.5M))
                    {
                        //MessageBox.Show("Solo se permite el valor 0 como excepción o valores entre 8.50 y 14.50.");
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("Valor inválido, rango entre 8.50 y 14.50");
                        _FrmMensajes.ShowDialog();
                        e.Cancel = true;
                        return;
                        
                    }
                }
                else
                {
                    MessageBox.Show("Ingrese un número válido.");
                    e.Cancel = true;
                }
            }
            // Aplica solo a la columna "Adicion"
            if (Dgv_Pnl2_cont.Columns[e.ColumnIndex].Name != "Adicion")
                return;

            string txt = e.FormattedValue?.ToString() ?? "";
            if (!decimal.TryParse(txt, out decimal valadd))
            {
                //MessageBox.Show("Ingrese un número válido.",
                //                "Error de entrada",
                //                MessageBoxButtons.OK,
                //                MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }

            if (valadd < 0.75M || valadd > 3.50M)
            {
                //MessageBox.Show("El valor debe estar entre 0.00 y 3.50.",
                //                "Rango inválido",
                //                MessageBoxButtons.OK,
                //                MessageBoxIcon.Warning);
                e.Cancel = true;
            }

        }

        private void Dgv_Pnl2_cont_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && Dgv_Pnl2_cont.Columns[e.ColumnIndex].Name == "Diametro" && e.RowIndex >= 0)
            {
                DataGridViewCell cell = Dgv_Pnl2_cont.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Verifica si la celda está vacía (Value es null o una cadena vacía)
                if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    cell.Value = 0; // Establece el valor a cero
                }
            }
            if (e.ColumnIndex >= 0 && Dgv_Pnl2_cont.Columns[e.ColumnIndex].Name == "Esfera" && e.RowIndex >= 0)
            {
                DataGridViewCell cell = Dgv_Pnl2_cont.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Verifica si la celda está vacía (Value es null o una cadena vacía)
                if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    cell.Value = 0; // Establece el valor a cero
                }
            }

            if (e.ColumnIndex >= 0 && Dgv_Pnl2_cont.Columns[e.ColumnIndex].Name == "Cilindro")
            {
                DataGridViewCell cell = Dgv_Pnl2_cont.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    cell.Value = 0;
                }
            }


            if (e.ColumnIndex >= 0 && Dgv_Pnl2_cont.Columns[e.ColumnIndex].Name == "Eje")
            {
                DataGridViewCell cell = Dgv_Pnl2_cont.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    cell.Value = 0;
                }
            }



            if (e.ColumnIndex >= 0 && Dgv_Pnl2_cont.Columns[e.ColumnIndex].Name == "Adicion")
            {
                DataGridViewCell cell = Dgv_Pnl2_cont.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    cell.Value = 0;
                }
            }

            // Verifica si la celda que recibió el foco está en la columna "Cilindro"
            // Actualizar columnas A0 y A1 en función de Esfera y Cilindro
            if (Dgv_Pnl2_cont.Columns.Contains("Esfera") && Dgv_Pnl2_cont.Columns.Contains("aEsfera"))
            {
                decimal esferaValue = 0; // Inicializar con un valor por defecto
                object esferaCellValue = Dgv_Pnl2_cont.Rows[e.RowIndex].Cells["Esfera"].Value; // Usar Dgv_Pnl2_cont

                if (esferaCellValue != null && decimal.TryParse(esferaCellValue.ToString(), out esferaValue))
                {
                    DataGridViewCell a0Cell = Dgv_Pnl2_cont.Rows[e.RowIndex].Cells["aEsfera"];
                    if (esferaValue > 0)
                    {
                        a0Cell.Value = "+";
                    }
                    else if (esferaValue < 0) // Añadido el caso para valores negativos
                    {
                        a0Cell.Value = "-";
                    }
                    else
                    {
                        a0Cell.Value = "";
                    }
                }
                else
                {
                    Dgv_Pnl2_cont.Rows[e.RowIndex].Cells["aEsfera"].Value = ""; // Limpiar A0 si Esfera no es válido
                }
            }
            // ------------------------------------------------------


            // Verifica si la celda que recibió el foco está en la columna "Cilindro"
            if (Dgv_Pnl2_cont.Columns.Contains("Cilindro") && Dgv_Pnl2_cont.Columns.Contains("aCilindro"))
            {
                decimal cilindroValue = 0;  // Inicializar
                object cilindroCellValue = Dgv_Pnl2_cont.Rows[e.RowIndex].Cells["Cilindro"].Value; // Usar Dgv_Pnl2_cont

                if (cilindroCellValue != null && decimal.TryParse(cilindroCellValue.ToString(), out cilindroValue))
                {
                    DataGridViewCell a1Cell = Dgv_Pnl2_cont.Rows[e.RowIndex].Cells["aCilindro"];
                    if (cilindroValue > 0)
                    {
                        a1Cell.Value = "+";
                    }
                    else if (cilindroValue < 0)
                    {
                        a1Cell.Value = "-";
                    }
                    else
                    {
                        a1Cell.Value = "";
                    }
                }
                else
                {
                    Dgv_Pnl2_cont.Rows[e.RowIndex].Cells["aCilindro"].Value = "";
                }
            }
            // ------------------------------------------------------



        }

        private void Dgv_Pnl2_cont_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex >= 0 && Dgv_Pnl2_conv.Columns[e.ColumnIndex].Name == "Adicion" && e.RowIndex >= 0)
            {
                DataGridViewCell cell = Dgv_Pnl2_conv.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Verifica si la celda está vacía (Value es null o una cadena vacía)
                if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    cell.Value = 0; // Establece el valor a cero
                }
                else
                {
                    // Si la celda no está vacía, valida el número ingresado
                    if (double.TryParse(cell.Value.ToString(), out double enteredValue))
                    {
                        // Calcula el residuo de la división por 0.25
                        double remainder = enteredValue % 1;

                        // Define una pequeña tolerancia para evitar problemas de coma flotante
                        double tolerance = 0.0000000001;

                        // Si no es divisible por 0.25 (o el residuo no es cercano a cero)
                        if (Math.Abs(remainder) > tolerance && Math.Abs(remainder - 1) > tolerance)
                        {
                            // Calcula el número más cercano que es múltiplo de 0.25
                            double roundedValue = Math.Round(enteredValue / 1) * 1;


                            //Pnl_2_Msj.Visible = true;
                            //txt_pl2_msj.Text = "El valor ingresado debe ser un múltiplo de 0.25 Se ha ajustado a " + roundedValue.ToString("F2");
                            ////pb_pl2_mj.Visible = true;


                            // Actualiza el valor de la celda
                            cell.Value = roundedValue;


                        }
                    }
                    else
                    {
                        // Si el valor no es un número válido, puedes manejarlo aquí (por ejemplo, establecerlo a 0 o mostrar un error)
                        MessageBox.Show("Por favor, ingrese un número válido.", "Error de entrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cell.Value = 0; // O la acción que consideres adecuada
                    }
                }
            }

            if (e.ColumnIndex >= 0 && Dgv_Pnl2_cont.Columns[e.ColumnIndex].Name == "Diametro" && e.RowIndex >= 0)
            {
                DataGridViewCell cell = Dgv_Pnl2_cont.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Verifica si la celda está vacía (Value es null o una cadena vacía)
                if ( cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    cell.Value = 0; // Establece el valor a cero
                }
            }

            if (e.ColumnIndex >= 0 && Dgv_Pnl2_cont.Columns[e.ColumnIndex].Name == "Esfera" && e.RowIndex >= 0)
            {
                DataGridViewCell cell = Dgv_Pnl2_cont.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Verifica si la celda está vacía (Value es null o una cadena vacía)
                if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    cell.Value = 0; // Establece el valor a cero
                }
                else
                {
                    // Si la celda no está vacía, valida el número ingresado
                    if (double.TryParse(cell.Value.ToString(), out double enteredValue))
                    {
                        // Calcula el residuo de la división por 0.25
                        double remainder = enteredValue % 0.25;

                        // Define una pequeña tolerancia para evitar problemas de coma flotante
                        double tolerance = 0.0000000001;

                        // Si no es divisible por 0.25 (o el residuo no es cercano a cero)
                        if (Math.Abs(remainder) > tolerance && Math.Abs(remainder - 0.25) > tolerance)
                        {
                            // Calcula el número más cercano que es múltiplo de 0.25
                            double roundedValue = Math.Round(enteredValue / 0.25) * 0.25;



                            // Muestra un mensaje al usuario
                            //MessageBox.Show("El valor ingresado debe ser un múltiplo de 0.25. Se ha ajustado a " + roundedValue.ToString("F2") + ".", "Advertencia de entrada", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            //Pnl_2_Msj.Visible = true;
                            //txt_pl2_msj.Text = "El valor ingresado debe ser un múltiplo de 0.25 Se ha ajustado a " + roundedValue.ToString("F2");
                            //pb_pl2_mj.Visible = true;

                            // Actualiza el valor de la celda
                            cell.Value = roundedValue;



                        }
                    }
                    else
                    {
                        // Si el valor no es un número válido, puedes manejarlo aquí (por ejemplo, establecerlo a 0 o mostrar un error)
                        MessageBox.Show("Por favor, ingrese un número válido.", "Error de entrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cell.Value = 0; // O la acción que consideres adecuada
                    }
                }
            }


            if (e.ColumnIndex >= 0 && Dgv_Pnl2_cont.Columns[e.ColumnIndex].Name == "Cilindro" && e.RowIndex >= 0)
            {
                DataGridViewCell cell = Dgv_Pnl2_cont.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Verifica si la celda está vacía (Value es null o una cadena vacía)
                if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    cell.Value = 0; // Establece el valor a cero
                }
                else
                {
                    // Si la celda no está vacía, valida el número ingresado
                    if (double.TryParse(cell.Value.ToString(), out double enteredValue))
                    {
                        // Calcula el residuo de la división por 0.25
                        double remainder = enteredValue % 0.25;

                        // Define una pequeña tolerancia para evitar problemas de coma flotante
                        double tolerance = 0.0000000001;

                        // Si no es divisible por 0.25 (o el residuo no es cercano a cero)
                        if (Math.Abs(remainder) > tolerance && Math.Abs(remainder - 0.25) > tolerance)
                        {
                            // Calcula el número más cercano que es múltiplo de 0.25
                            double roundedValue = Math.Round(enteredValue / 0.25) * 0.25;



                            //Pnl_2_Msj.Visible = true;
                            //txt_pl2_msj.Text = "El valor ingresado debe ser un múltiplo de 0.25 Se ha ajustado a " + roundedValue.ToString("F2");
                            //pb_pl2_mj.Visible = true;

                            // Actualiza el valor de la celda
                            cell.Value = roundedValue;



                        }
                    }
                    else
                    {
                        // Si el valor no es un número válido, puedes manejarlo aquí (por ejemplo, establecerlo a 0 o mostrar un error)
                        MessageBox.Show("Por favor, ingrese un número válido.", "Error de entrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cell.Value = 0; // O la acción que consideres adecuada
                    }
                }
            }

            if (e.ColumnIndex >= 0 && Dgv_Pnl2_cont.Columns[e.ColumnIndex].Name == "Eje" && e.RowIndex >= 0)
            {
                DataGridViewCell cell = Dgv_Pnl2_cont.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Verifica si la celda está vacía
                if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    cell.Value = 0; // Establece el valor a cero
                }
                else
                {
                    int valoreje = Convert.ToInt32(cell.Value);
                    // Si la celda no está vacía, valida el número ingresado
                    if (int.TryParse(valoreje.ToString(), out int enteredValue))
                    {
                        // Verifica si el valor es un múltiplo de 5
                        if (enteredValue % 5 != 0)
                        {
                            // Calcula el múltiplo de 5 más cercano
                            int roundedValue = (int)Math.Round((double)enteredValue / 5) * 5;

                            // Muestra un mensaje al usuario
                            //Pnl_2_Msj.Visible = true;
                            //txt_pl2_msj.Text = $"El valor ingresado debe ser un múltiplo de 5. Se ha ajustado a {roundedValue}.";
                            //pb_pl2_mj.Visible = true;

                            // Actualiza el valor de la celda
                            cell.Value = roundedValue;
                        }
                    }
                    else
                    {
                        // Si el valor no es un entero válido
                        //MessageBox.Show("Por favor, ingrese un número entero válido.", "Error de entrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cell.Value = 0; // O la acción que consideres adecuada
                    }
                }
            }






            // Verifica que el índice de la fila sea válido
            if (e.RowIndex >= 0)
            {
                // Llama al evento CellEnter, pasando los mismos sender y argumentos
                DataGridViewCell changedCell = Dgv_Pnl2_cont.Rows[e.RowIndex].Cells[e.ColumnIndex];
                // Obtiene la celda que cambió
                if (changedCell.Value != null && changedCell.Value.ToString() != string.Empty)
                // Verifica que el valor no sea nulo ni vacío
                {
                    Dgv_Pnl2_cont_CellEnter(sender, e);
                }

            }




            // Verificar si el cambio ocurrió en una fila válida y en la columna "Lejos"
            if (e.RowIndex >= 0 && Dgv_Pnl2_cont.Columns[e.ColumnIndex].Name == "Lejos")
            {
                // Obtener el valor de la celda "Lejos" que cambió
                if (Dgv_Pnl2_cont.Rows[e.RowIndex].Cells["Lejos"].Value != null &&
                    decimal.TryParse(Dgv_Pnl2_cont.Rows[e.RowIndex].Cells["Lejos"].Value.ToString(), out decimal lejosValue))
                {
                    // Verificar si lejosValue es mayor que 0
                    if (lejosValue > 0)
                    {
                        // Calcular el valor para la columna "Cerca"
                        decimal cercaValue = lejosValue - 1;

                        // Verificar si la columna "Cerca" existe y actualizar su valor en la misma fila
                        if (Dgv_Pnl2_cont.Columns.Contains("Cerca"))
                        {
                            Dgv_Pnl2_cont.Rows[e.RowIndex].Cells["Cerca"].Value = cercaValue;
                        }
                    }
                    // Puedes agregar un 'else' aquí si quieres hacer algo cuando lejosValue no es mayor que 0
                    // Por ejemplo, podrías establecer la celda "Cerca" a 0 o mostrar un mensaje.
                    /*
                    else
                    {
                        if (Dgv_Pnl2_cont.Columns.Contains("Cerca"))
                        {
                            Dgv_Pnl2_cont.Rows[e.RowIndex].Cells["Cerca"].Value = 0;
                        }
                    }
                    */
                }
            }
        }

        private void dvgClientePagador_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dvgClientePagador.Rows[e.RowIndex].Cells.Count > 0) // Cambiado a dvgClientePagador
            {
                string cedulaSeleccionada = dvgClientePagador.Rows[e.RowIndex].Cells[1].Value?.ToString().Trim(); // Cambiado a dvgClientePagador
                string nacio = dvgClientePagador.Rows[e.RowIndex].Cells[0].Value?.ToString().Trim(); // Cambiado a dvgClientePagador

                if (!string.IsNullOrEmpty(cedulaSeleccionada))
                {
                    try
                    {
                        dtCliente = _L_Cliente.ObtenerClientePorCedula(cedulaSeleccionada, nacio);

                        if (dtCliente != null && dtCliente.Rows.Count > 0)
                        {
                            Pnl_5_Lista_ClienPagador.Visible = false;
                            Txt_Tap1_Cedula_Pagador.Focus();

                            Cbx_Tap1_Nacionalidad_Pagador.Text = dtCliente.Rows[0]["CTE_Nacio"].ToString();

                            Txt_Tap1_Cedula_Pagador.Text = dtCliente.Rows[0]["CTE_CedIden"].ToString(); // Ajusta el nombre de la columna
                            Txt_Tap1_Nombre_Pagador.Text = dtCliente.Rows[0]["CTE_PNombre"].ToString(); // Ajusta el nombre de la columna

                        }
                        else
                        {
                            MessageBox.Show("No se encontró ningún cliente con esa cédula.", "Importante", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ocurrió un error al obtener la información del cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    }
                }
            }
        }


        private void button6_Click(object sender, EventArgs e)
        {
            mantenervacio = false;
            Btn_Tap2_Derecha_Click(this.Btn_Tap2_Derecha, EventArgs.Empty);


            //    Txt_Tap2_Examen.Text = TopeExamen.ToString();

            ////limpearExamen();
            //CargarExamenConv();
            //CargarExamenCont();
            //CargarDgvPnl2MedConv();
            //CargarFicconvOFT();

            //CargarDgv_Pnl2_Querato();
        }

        private void button12_Click(object sender, EventArgs e)
        {


            Dgv_Pnl2_Querato.Rows[1].Cells[0].Value = Dgv_Pnl2_Querato.Rows[0].Cells[0].Value;
            Dgv_Pnl2_Querato.Rows[1].Cells[1].Value = Dgv_Pnl2_Querato.Rows[0].Cells[1].Value;
            Dgv_Pnl2_Querato.Rows[1].Cells[2].Value = Dgv_Pnl2_Querato.Rows[0].Cells[2].Value;
            Dgv_Pnl2_Querato.Rows[1].Cells[3].Value = Dgv_Pnl2_Querato.Rows[0].Cells[3].Value;
        }

        private void txt_Pnl2_conv_mimesys_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits (0-9)
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = false; // Allow the character
            }
            // Allow the backspace key
            else if (e.KeyChar == (char)Keys.Back)
            {
                e.Handled = false; // Allow the backspace
            }
            // Optionally, allow a decimal point (if you need to enter floating-point numbers)
            else if (e.KeyChar == '.' && !((TextBox)sender).Text.Contains('.'))
            {
                e.Handled = false; // Allow one decimal point
            }
            // Block all other characters
            else
            {
                e.Handled = true; // Prevent the character from being entered
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            SetMeridianoCornealLimits(); // Llama al nuevo método para establecer los límites


        }
        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            SetMeridianoCornealLimits(); // Llama al nuevo método para establecer los límites


        }

        private void SetMeridianoCornealLimits()
        {
            if (radioButton5.Checked)
            {
                minMeridianoCorneal = 6M;
                maxMeridianoCorneal = 10M;
            }
            else
            {
                minMeridianoCorneal = 33.75M;
                maxMeridianoCorneal = 56.25M;
            }
            // Opcional: Si quieres que la validación se aplique inmediatamente a las celdas existentes
            // (por ejemplo, si cambias el radio button y la celda ya tiene un valor no válido con los nuevos límites)
            // puedes forzar una revalidación, aunque esto puede ser complejo y no siempre necesario.
            //Dgv_Pnl2_Querato.Invalidate(); // Fuerza un repintado (no una revalidación de datos)
            // Para revalidar datos específicos, tendrías que iterar por las filas y columnas.
        }

        

        private void button4_Click(object sender, EventArgs e)
        {

            Pnl_2_Msj.Visible = false;
        }

       

        private bool ValidarTipoTrabajoTipoExamen(string tipoVenta, string tipoExamen)
        {
            if ((tipoVenta == "01" && tipoExamen == "CONTACTO") ||  (tipoVenta == "02" && tipoExamen == "CONVENCIONAL"))
            {
                Btn_Tap3_Cancelar.PerformClick();

                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("El examen seleccionado no aplica para este tipo de trabajo");
                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                _FrmMensajes.ShowDialog();


                //CargarComboServicioLaboratorios();

                //if (tipoExamen == "CONTACTO")
                //{
                //    Cbx_Pnl2_Trbajo.SelectedIndex = 1;
                //}
                //else if (tipoExamen == "CONVENCIONAL")
                //{
                //    Cbx_Pnl2_Trbajo.SelectedIndex = 0;
                //}
                return false;
            }
            if ((tipoVenta == "01" || tipoVenta == "02") && ((Cbx_Tap2_Ojo.Text == "") ||  (Cbx_Tap2_Ojo.Text == "Ambos" &&  cbVisionDerecha.Text == "" || cbVisionIzquierda.Text == "") || (Cbx_Tap2_Ojo.Text == "DERECHO" && cbVisionDerecha.Text == "") || (Cbx_Tap2_Ojo.Text == "IZQUIERDO" && cbVisionIzquierda.Text == "")))
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Seleccione el ojo y tipo de visión");
                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                _FrmMensajes.ShowDialog();

                return false;
            }

            if (tabControl.SelectedIndex == 2)
            {
                if (Cbx_Pnl2_Servicio.Items.Count > 0 && !string.IsNullOrEmpty(Cbx_Pnl2_Servicio.Text))
                {
                    if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "01" || Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "02")
                    {
                        if (_L_Articulo.Disponible_Servicio_3Horas(Cbx_Pnl2_Servicio.Text, Cbx_Pnl2_Laboratorio.Text))
                        {
                            List<TB_SERVICIOSLABDTO> TB_SERVICIOSLABD = new List<TB_SERVICIOSLABDTO>();
                            TB_SERVICIOSLABD = _D_Articulo.ServiciosLaboratorio(Cbx_Pnl2_Servicio.SelectedValue.ToString());
                            if (TB_SERVICIOSLABD != null && TB_SERVICIOSLABD.Count > 0)
                            {
                                TB_SERVICIOSLABDTO _SERVICIOSLABDTO = TB_SERVICIOSLABD.First();
                                Codigo_Servicio_Agregar = _SERVICIOSLABDTO.CodArticulo;

                                string Cod_Vta = Cbx_Pnl2_Trbajo.SelectedValue.ToString();
                                string Cod_Serv = Cbx_Pnl2_Servicio.SelectedValue?.ToString();
                                if ((Cod_Vta == "01" || Cod_Vta == "08" || Cod_Vta == "02") && Cod_Serv != "017")
                                {
                                try
                                {
                                    _FechaHoraOfrecida = _L_Articulo.ObtenerFechaHoraOfrecida(Cbx_Pnl2_Servicio.SelectedValue.ToString(), Cbx_Pnl2_Trbajo.SelectedValue.ToString());
                                    if (_FechaHoraOfrecida != null && _FechaHoraOfrecida.Count > 0)
                                    {
                                        FechaHoraOfrecida resultado = _FechaHoraOfrecida.First();
                                        Txt_Pnl2_Fecha_Ofre.Text = $"{resultado.FechaOfrecida:dd/MM/yyyy}";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _FrmMensajes.co = 2;
                                    _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                                    _FrmMensajes.ShowDialog();
                                }

                               }
                        }
                            else
                            {
                                Codigo_Servicio_Agregar = "";
                            }
                        }
                        else
                        {
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje("El servicio no está disponible en este horario");
                            _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                            _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                            _FrmMensajes.ShowDialog();
                            //Codigo_Servicio_Agregar = "";
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        private void AgregarRx()
        {
            if (Formulario_ListaOrdenes)
            {
                if (Cbx_Tap2_Tipo_Examen.Text== "CONTACTO")
                {
                    mostrarError("No se puede agregar un examen de contacto a una orden convencional reservada");
                    return;
                }

                bool Respuesta = _Asignar_Rx.AsignarRx(TB_CAORDSER.Cod_DetVta, TB_CAORDSER.NumOrdserv, TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.CTE_Nacio, TB_CAORDSER.CTE_CedIden, Txt_Tap2_Examen.Text, TB_CAORDSER.NumOrdserv, LbResultado2, LbResultados, dgvRangoCrt, TB_CAORDSER.Cod_Laboratorio, TB_CAORDSER.Cod_Servicio);
                if (!Respuesta && _Asignar_Rx.stringBuilder.Length > 0)
                {

                    if (_Asignar_Rx.stringBuilder.ToString() == "El cristal seleccionado no se adapta a los siguientes rangos")
                    {
                        LbResultados.Visible = false;
                        LbResultado2.Visible = false;
                        lblDiametroD.Text = _Asignar_Rx.diamDgl;
                        lblDiametroI.Text = _Asignar_Rx.diamIgl;
                        lblDiametroD.Visible = lblDiametroI.Visible = true;
                        LblTitulo.Text = _Asignar_Rx.stringBuilder.ToString();
                        lblClaveAut.Visible = true;
                        lblLeyenda.Visible = false;
                        btnAutorizarRangosCrt.Visible = true;
                        pnlRangoCrt.Show();
                        pnlRangoCrt.Location = new Point(200, 150);
                        FormatoTablaRango();

                        //Titilo Diametro
                        lblTituloDiam.Location = new Point(19, 160);

                        //Titulo Derecho Izquierdo
                        label19.Location = new Point(161, 160); //D
                        label23.Location = new Point(212, 160); //I

                        //Valores Derecho Izquierdo
                        lblDiametroD.Location = new Point(165, 180); //D
                        lblDiametroI.Location = new Point(212, 180); //I

                        // Mensaje de clave Autorizada 
                        lblClaveAut.Location = new Point(10, 235);

                        return;

                    }
                    else if (_Asignar_Rx.stringBuilder.ToString() == "El cristal no se adapta a estos parámetros")
                    {
                        lblDiametroD.Visible = false;
                        lblDiametroI.Visible = false;
                        LblTitulo.Text = _Asignar_Rx.stringBuilder.ToString();
                        lblClaveAut.Visible = false;
                        lblLeyenda.Visible = true;
                        btnAutorizarRangosCrt.Visible = false;
                        LbResultados.Visible = true;
                        LbResultado2.Visible = true;
                        pnlRangoCrt.Show();
                        pnlRangoCrt.Location = new Point(200, 150);
                        FormatoDataGridRangosCristales();

                        //Titilo Diametro
                        lblTituloDiam.Location = new Point(19, 194);

                        //Titulo Derecho Izquierdo
                        label19.Location = new Point(161, 194); //D
                        label23.Location = new Point(212, 194); //I

                        //Valores Derecho Izquierdo
                        lblDiametroD.Location = new Point(165, 214); //D
                        lblDiametroI.Location = new Point(212, 214); //I

                        // Mensaje de clave Autorizada 
                        lblClaveAut.Location = new Point(10, 269);

                        return;
                    }
                    else
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje(_Asignar_Rx.stringBuilder.ToString());
                        _FrmMensajes.ShowDialog();
                        return; // Salir 
                    }
                }
                else if (Respuesta)
                {

                    string concat = TB_CAORDSER.Cod_Sucursal + TB_CAORDSER.NumOrdserv + TB_CAORDSER.Revision;
                    if (TB_CAORDSER.Cod_DetVta == "02") // Si se procesa uan orden de contacto se muestra reporte de contacto  
                    {
                        _FrmRepOrden.setParametros(concat);
                        _FrmRepOrden.ConfigRep(false, true);

                        if (_D_DetalleOrden.ParametroImpresion() == "1") // Si el parametro de impresion es 1 entonces imprime el reporte 
                        {
                            _FrmRepOrden.imprimir();

                        }
                        else
                        {
                            _FrmRepOrden.ShowDialog();

                        }


                    }
                    else
                    {
                        _FrmRepOrden.setParametros(concat);
                        _FrmRepOrden.ConfigRep(true, false);

                        if (_D_DetalleOrden.ParametroImpresion() == "1") // Si el parametro de impresion es 1 entonces imprime el reporte 
                        {
                            _FrmRepOrden.imprimir();

                        }
                        else
                        {
                            _FrmRepOrden.ShowDialog();

                        }

                    }

                    // Cerrar el contenedor después de procesar exitosamente
                    _onCierreSolicitado?.Invoke(); // 👈 Ejecuta el cierre del padre
                }

            }
        }

        public void FormatoOscuro(System.Drawing.Color col2, System.Drawing.Color col3, System.Drawing.Color col4 )
        {
            // col2 = white; col3 = verde azulado claro
            if (col4.ToString() == "Color [A=255, R=255, G=255, B=255]")
                Formato_Claro = true;
            else
                Formato_Claro = false;
           
            //codigo Claro 
            if (Formato_Claro)
            {
                
                tabPage1.BackColor = col2;
                tabPage3.BackColor = col2;
                QuitarLimea2.BackColor = col2;
                QuitarLimea3.BackColor = col2;
                Dgv_Tap3_Articulo.BackColor = col2;
                Lbl_Tap1_DatosPersonal.BackColor = col3;
                Lbl_Tap1_DatosPersonal.ForeColor = ColorTranslator.FromHtml("#1c2422");
                label11.BackColor = col3;
                label11.ForeColor = ColorTranslator.FromHtml("#1c2422");
                Lbl_Pnl1_TituloDatos.BackColor = col3;
                Lbl_Pnl1_TituloDatos.ForeColor = ColorTranslator.FromHtml("#1c2422");
                Lbl_Tap1_Contacto.BackColor = col3;
                Lbl_Tap1_Contacto.ForeColor = ColorTranslator.FromHtml("#1c2422");
                Lbl_Pnl2_Carga_Art.BackColor = col3;
                Lbl_Pnl2_Carga_Art.ForeColor = ColorTranslator.FromHtml("#1c2422");
                Lbl_Tap3_Articulo1.BackColor = col3;
                Lbl_Tap3_Articulo1.ForeColor = ColorTranslator.FromHtml("#1c2422");
                Lbl_Tap3_Articulo2.BackColor = col3;
                Lbl_Tap3_Articulo2.ForeColor = ColorTranslator.FromHtml("#1c2422");
                Lbl_Tap3_Medidas_Montura.BackColor = col3;
                Lbl_Tap3_Medidas_Montura.ForeColor = ColorTranslator.FromHtml("#1c2422");
                Lbl_Tap3_.BackColor = col3;
                Lbl_Tap3_.ForeColor = ColorTranslator.FromHtml("#1c2422");
                Dgv_Pnl3_Articulo.DefaultCellStyle.BackColor = col2;
                Dgv_Pnl3_Articulo.DefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#1c2422");
                Dgv_Pnl3_Articulo.ColumnHeadersDefaultCellStyle.BackColor = col3;
                Dgv_Pnl3_Articulo.ColumnHeadersDefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#1c2422");
                Lbl_Pnl3_Carga_Articulo.BackColor = col3;
                Lbl_Pnl3_Carga_Articulo.ForeColor = ColorTranslator.FromHtml("#1c2422");
                Lbl_Pnl3_Promociones.BackColor = col3;
                Lbl_Pnl3_Promociones.ForeColor = Color.White;
                Dgv_Pnl3_Promociones.BackgroundColor = Color.White;
                Lbl_Pnl3_Descuento.BackColor = col3;
                Lbl_Pnl3_Descuento.ForeColor = Color.White;
                Lbl_Pnl3_PorcDescuento.ForeColor = ColorTranslator.FromHtml("#1c2422");
                Lbl_Pnl3_Monto.ForeColor = ColorTranslator.FromHtml("#1c2422");
                Lbl_Pnl3_MotivoDesc.ForeColor = ColorTranslator.FromHtml("#1c2422");
                Lbl_Pnl3_ObservacionDesc.ForeColor = ColorTranslator.FromHtml("#1c2422");
                Lbl_Pnl3_Cambio_Precio.BackColor = col3;
                Lbl_Pnl3_Cambio_Precio.ForeColor = Color.White;
                label16.BackColor = col3;
                label16.ForeColor = Color.White;
                grp_pln2_Cont1.BackColor = col2;
                tabPage2.BackColor = col2;
                btn_pln2_quer.BackColor = col2;
                btn_pln2_reti.BackColor = col2;
                btn_pln2_oft.BackColor = col2;
                grp_pln2_OBS.BackColor = col2;
                Pnl_2_Tap2.BackColor = col3;
                Lbl_Tap2_Tipo_Exa.BackColor = col3;
                //Lbl_Tap2_Datos.BackColor = col3;
                //Lbl_Tap2_Datos.ForeColor = Color.White;
                label31.BackColor = col3;
                label31.ForeColor = Color.White;
                lbl_pnl2_obser.BackColor = col3;
                lbl_pnl2_obser.ForeColor = Color.White;
                label32.BackColor = col3;
                label32.ForeColor = Color.White;
                //Dgv_Pnl2_medconv.ColumnHeadersDefaultCellStyle.BackColor = col3;
                Dgv_Pnl2_cont.ColumnHeadersDefaultCellStyle.BackColor = col3;
                grp_pln2_oft3.BackColor = col2;
                label36.BackColor = col3;
                grp_pln2_ret4.BackColor = col2;
                label38.BackColor = col3;
                grp_pln2_quera5.BackColor = col2;
                label42.BackColor = col3;
                Dgv_Pnl2_Querato.ColumnHeadersDefaultCellStyle.BackColor = col3;
                grp_pln2_Conv2.BackColor = col2;
                lbl_pnl2_con_obser.BackColor = col3;
                lbl_pnl2_con_obser.ForeColor = col2;
                label30.BackColor = col3;
                label30.ForeColor = ColorTranslator.FromHtml("#1c2422");
                txt_pl2_msj.BackColor = col2;
                txt_pl2_msj.ForeColor = ColorTranslator.FromHtml("#1c2422");
                Dgv_Pnl3_Garantia.ColumnHeadersDefaultCellStyle.BackColor = col3;
            }
            //codigo oscuro
            else
            {
                tabPage1.BackColor = col3;
                tabPage3.BackColor = col3;
                QuitarLimea2.BackColor = col3;
                QuitarLimea3.BackColor = col3;
                Dgv_Tap3_Articulo.BackColor = col3;
                Lbl_Tap1_DatosPersonal.BackColor = ColorTranslator.FromHtml("#003536");
                Lbl_Tap1_DatosPersonal.ForeColor = Color.White;
                label11.BackColor = ColorTranslator.FromHtml("#003536");
                label11.ForeColor = Color.White;
                Lbl_Pnl1_TituloDatos.BackColor = ColorTranslator.FromHtml("#003536");
                Lbl_Pnl1_TituloDatos.ForeColor = Color.White;
                Lbl_Tap1_Contacto.BackColor = ColorTranslator.FromHtml("#003536"); 
                Lbl_Tap1_Contacto.ForeColor = Color.White;
                Lbl_Pnl2_Carga_Art.BackColor = ColorTranslator.FromHtml("#003536");
                Lbl_Pnl2_Carga_Art.ForeColor = Color.White;
                Lbl_Tap3_Articulo1.BackColor = ColorTranslator.FromHtml("#003536");
                Lbl_Tap3_Articulo1.ForeColor = Color.White;
                Lbl_Tap3_Articulo2.BackColor = ColorTranslator.FromHtml("#003536");
                Lbl_Tap3_Articulo2.ForeColor = Color.White;
                Lbl_Tap3_Medidas_Montura.BackColor = ColorTranslator.FromHtml("#003536");
                Lbl_Tap3_Medidas_Montura.ForeColor = Color.White;
                Lbl_Tap3_.BackColor = ColorTranslator.FromHtml("#003536");
                Lbl_Tap3_.ForeColor = Color.White;
                Lbl_Pnl3_Carga_Articulo.BackColor = ColorTranslator.FromHtml("#003536");
                Lbl_Pnl3_Carga_Articulo.ForeColor = Color.White;
                Dgv_Pnl3_Articulo.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#07a79b");
                Dgv_Pnl3_Articulo.DefaultCellStyle.ForeColor = Color.White;
                Dgv_Pnl3_Articulo.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#2f6b64");
                Dgv_Pnl3_Articulo.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                Dgv_Tap3_Articulo.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#07a79b");
                Dgv_Tap3_Articulo.BackgroundColor = ColorTranslator.FromHtml("#07a79b");
                Dgv_Tap3_Articulo.DefaultCellStyle.ForeColor = Color.White;
                Dgv_Tap3_Articulo.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#2f6b64");
                Dgv_Tap3_Articulo.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                Lbl_Pnl3_Promociones.BackColor = ColorTranslator.FromHtml("#003536");
                Lbl_Pnl3_Promociones.ForeColor = Color.White;
                Dgv_Pnl3_Promociones.BackgroundColor = ColorTranslator.FromHtml("#07a79b");
                Dgv_Pnl3_Promociones.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#07a79b");
                Lbl_Pnl3_Descuento.BackColor = ColorTranslator.FromHtml("#003536");
                Lbl_Pnl3_Descuento.ForeColor = Color.White;
                Lbl_Pnl3_PorcDescuento.ForeColor = Color.White;
                Lbl_Pnl3_Monto.ForeColor = Color.White;
                Lbl_Pnl3_MotivoDesc.ForeColor = Color.White;
                Lbl_Pnl3_ObservacionDesc.ForeColor = Color.White;
                Lbl_Pnl3_Cambio_Precio.BackColor = ColorTranslator.FromHtml("#003536");
                Lbl_Pnl3_Cambio_Precio.ForeColor = Color.White;
                label16.BackColor = ColorTranslator.FromHtml("#003536");
                label16.ForeColor = Color.White;
                grp_pln2_Cont1.BackColor = col3;
                tabPage2.BackColor = col3;
                btn_pln2_quer.BackColor = col3;
                btn_pln2_reti.BackColor = col3;
                btn_pln2_oft.BackColor = col3;
                grp_pln2_OBS.BackColor = col3;
                Pnl_2_Tap2.BackColor = ColorTranslator.FromHtml("#003536");
                Lbl_Tap2_Tipo_Exa.BackColor = ColorTranslator.FromHtml("#003536");
                //Lbl_Tap2_Datos.BackColor = ColorTranslator.FromHtml("#003536");
                //Lbl_Tap2_Datos.ForeColor = Color.White;
                label31.BackColor = ColorTranslator.FromHtml("#003536");
                label31.ForeColor = Color.White;
                lbl_pnl2_obser.BackColor = ColorTranslator.FromHtml("#003536");
                lbl_pnl2_obser.ForeColor = Color.White;
                label32.BackColor = ColorTranslator.FromHtml("#003536");
                label32.ForeColor = Color.White;
                //Dgv_Pnl2_medconv.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#2f6b64");
                Dgv_Pnl2_cont.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#2f6b64");
                grp_pln2_oft3.BackColor = col3;
                label36.BackColor = ColorTranslator.FromHtml("#003536");
                grp_pln2_ret4.BackColor = col3;
                label38.BackColor = ColorTranslator.FromHtml("#003536");
                grp_pln2_quera5.BackColor = col3;
                label42.BackColor = ColorTranslator.FromHtml("#003536");
                Dgv_Pnl2_Querato.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#2f6b64");
                grp_pln2_Conv2.BackColor = col3;
                lbl_pnl2_con_obser.ForeColor = col2;
                lbl_pnl2_con_obser.BackColor = ColorTranslator.FromHtml("#07a79b");
                label30.BackColor = ColorTranslator.FromHtml("#003536");
                label30.ForeColor = Color.White;
                txt_pl2_msj.BackColor = col2;
                txt_pl2_msj.ForeColor = Color.White;
                Dgv_Pnl3_Garantia.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#2f6b64");
            }

            this.BackColor = col2;
            QuitarLimea2.BackColor = col2;
            QuitarLimea3.BackColor = col2;
            QuitarLimea1.BackColor = col2;
        }

        public void AsignarRx_JuegoPantalla()
        {
            btnExamen.Focus();
            tabControl.SelectedIndex = 1;
           
            label43.Text = "Datos de Clientes";

                grp_pln2_Cont1.BringToFront();
                btn_pln2_oft.BringToFront();
                btn_pln2_reti.BringToFront();
                btn_pln2_quer.BringToFront();
                if (mantengoexamenseleccionado == false) // Voy al ultimo
                {
                    Btn_Tap2_Derecha_Click(this.Btn_Tap2_Derecha, EventArgs.Empty);
                }

                // Opcional: Llamar al evento directamente si la selección no lo dispara
                // tabControl_SelectedIndexChanged(tabControl, EventArgs.Empty);

            Pnl_2.Visible = true;
            grp_pln2_Conv2.Visible = true;
        }

        private void Dgv_Pnl3_Garantia_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (Dgv_Pnl3_Garantia.IsCurrentCellDirty)
            {
                Dgv_Pnl3_Garantia.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void Dgv_Pnl3_Garantia_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica si la columna es la de CheckBox
            if (Dgv_Pnl3_Garantia.Columns[e.ColumnIndex].Name == "E")
            {
                // Si la celda fue marcada
                if (Convert.ToBoolean(Dgv_Pnl3_Garantia.Rows[e.RowIndex].Cells["E"].Value))
                {
                    // Desmarca todas las demás filas
                    for (int i = 0; i < Dgv_Pnl3_Garantia.Rows.Count; i++)
                    {
                        if (i != e.RowIndex)
                        {
                            Dgv_Pnl3_Garantia.Rows[i].Cells["E"].Value = false;
                        }
                    }
                }
            }
        }

        private void Txt_Tap1_Nombre_TextChanged(object sender, EventArgs e)
        {
            // Guarda la posición actual del cursor (selección)
            int cursorPosition = this.Txt_Tap1_Nombre.SelectionStart;

            // Convierte el texto a mayúsculas
            this.Txt_Tap1_Nombre.Text = this.Txt_Tap1_Nombre.Text.ToUpper();

            // Restaura la posición del cursor para una mejor experiencia de usuario
            // Esto evita que el cursor salte al final cada cada vez que se escribe una letra.
            this.Txt_Tap1_Nombre.SelectionStart = cursorPosition;
        }

        private void Txt_Tap1_Nombre_Pagador_TextChanged(object sender, EventArgs e)
        {
            // Guarda la posición actual del cursor (selección)
            int cursorPosition = this.Txt_Tap1_Nombre_Pagador.SelectionStart;

            // Convierte el texto a mayúsculas
            this.Txt_Tap1_Nombre_Pagador.Text = this.Txt_Tap1_Nombre_Pagador.Text.ToUpper();

            // Restaura la posición del cursor para una mejor experiencia de usuario
            // Esto evita que el cursor salte al final cada cada vez que se escribe una letra.
            this.Txt_Tap1_Nombre_Pagador.SelectionStart = cursorPosition;
        }

        private void Dgv_Pnl2_Querato_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Solo aplica la lógica a las celdas de datos (no a los encabezados de fila o columna)
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // 1. Pintar el fondo y el contenido de la celda, EXCLUYENDO el borde predeterminado.
                e.Paint(e.CellBounds,
                        DataGridViewPaintParts.All
                        & ~DataGridViewPaintParts.Border);

                // 2. Dibujar manualmente solo el borde DERECHO (vertical) de la celda.
                // Aquí es donde corregimos el uso de GridColor.
                // Se usa Dgv_Pnl2_Querato.GridColor o ((DataGridView)sender).GridColor
                using (Pen p = new Pen(this.Dgv_Pnl2_Querato.GridColor, 1)) // <-- CORRECCIÓN AQUÍ
                {
                    // Dibujar la línea vertical en el lado derecho de la celda
                    e.Graphics.DrawLine(p, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);
                }

                // 3. Indicar que el evento ha sido manejado, para que el pintado predeterminado no se ejecute.
                e.Handled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtClienteBuscar.Text = "";
            panel2.Visible = false;
        }

        private void txtClienteP_TextChanged(object sender, EventArgs e)
        {
            // Obtener el texto del textBox1
            string textoFiltro = txtClienteP.Text.Trim(); // .Trim() para eliminar espacios en blanco al inicio y al final

            // Validar la longitud del texto
            if (textoFiltro.Length < 3 && textoFiltro.Length > 0) // Si tiene entre 1 y 2 caracteres
            {
                // Mostrar un mensaje al usuario
                //  MessageBox.Show("Por favor, ingrese al menos 3 caracteres para realizar la búsqueda.", "Filtro Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtClienteP.Focus(); // Opcional: devolver el foco al TextBox para que el usuario corrija
            }
            else if (textoFiltro.Length == 0) // Si el campo está vacío, puedes decidir si cargar todo o no hacer nada
            {
                // Si el campo está vacío, puedes optar por no hacer nada o cargar todos los datos
                // Por ejemplo, si quieres que al borrar el texto se muestren todos los clientes:
                // CargarDatosDeClientes(textoFiltro, radioButton2, radioButton1);
                // O simplemente no hacer nada si no hay filtro
                // Console.WriteLine("Campo de filtro vacío, no se realiza búsqueda.");
            }
            else // Si la longitud es 3 o más caracteres
            {
                // Llamar al método para cargar los datos de los clientes
                CargarDatosDeClientesP(textoFiltro, radioButton8, radioButton7);
            }

            //_L_Cliente.FiltrarClientes(textBox2.Text, radioButton2, radioButton1, DgvClientes, listaDeClientes, listaTemporalClientes);

        }


        private void llenarCabeceraExamenyOrden()
        {
            //Pnl_2.Visible = true;
            Txt_Pnl2_Cedula.Text = Cbx_Tap1_Nacionalidad.Text.Trim() + "-" + Txt_Tap1_Cedula.Text.Trim();
            Txt_Pnl_2_Nombre.Text = Txt_Tap1_Nombre.Text;
           

        }

        private void button14_Click(object sender, EventArgs e)
        {
            FrmPrincipal frmPrincipal = (FrmPrincipal)this.MdiParent ?? this.ParentForm as FrmPrincipal;
            //FrmListaOrdenes frmListaOrdenes = (FrmListaOrdenes)this.MdiParent ?? this.ParentForm as FrmListaOrdenes;
            FrmListaOrdenes frmListaOrdenes = new FrmListaOrdenes();
            frmPrincipal.addformulario(frmListaOrdenes);

            this.Close();
        }


       

        private void txtAltD_Leave(object sender, EventArgs e)
        {
            if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "01")
            {
                if (decimal.TryParse(txtAltD.Text, out decimal valor))
            {
                if (valor < 10 || valor > 35)
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("Valor inválido, rango entre 10 y 35");
                    _FrmMensajes.ShowDialog();
                    txtAltD.Text = "0"; // Borra el contenido si no está en rango
                    return;
                    
                }
            }
            else
            {
                txtAltD.Text = "0"; // Borra si no es numérico
            }
            }
        }

        private void txtAltI_Leave(object sender, EventArgs e)
        {
            if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "01")
            {
                var cultura = System.Globalization.CultureInfo.CurrentCulture;
                string texto = txtAltI.Text;

                // Expresión regular para máximo 2 decimales
                // Ejemplo válido: 12,34 — Ejemplo inválido: 12,345
                var regex = new System.Text.RegularExpressions.Regex(@"^\d{1,3}(,\d{1,2})?$");

                if (!regex.IsMatch(texto))
                {
                    //_FrmMensajes.co = 2;
                    //_FrmMensajes.avisomensaje("Ingrese un número válido con hasta 2 decimales");
                    //_FrmMensajes.ShowDialog();
                    txtAltI.Text = "0";
                    return;
                }

                // Validar rango con coma como separador decimal
                if (decimal.TryParse(texto, System.Globalization.NumberStyles.Number, cultura, out decimal valor))
                {
                    if (valor < 10 || valor > 35)
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("Valor fuera del rango permitido 10 a 35");
                        _FrmMensajes.ShowDialog();
                        txtAltI.Text = "0";
                    }
                }
                else
                {
                    txtAltI.Text = "0";
                }
            }
        }

        private void txtAltI_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtAltD.Focus();
            }
        }


        private void txtAltI_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = sender as TextBox;

            // Permitir solo números, coma y teclas de control (como retroceso)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
                return;
            }

            // Solo permitir una coma
            if (e.KeyChar == ',' && txt.Text.Contains(","))
            {
                e.Handled = true;
                return;
            }

            // Verificar si ya hay coma y limitar los decimales a dos
            if (txt.Text.Contains(","))
            {
                int indexComa = txt.Text.IndexOf(",");
                string decimales = txt.Text.Substring(indexComa + 1);

                // Si hay 2 decimales y el cursor está después de la coma
                if (txt.SelectionStart > indexComa && decimales.Length >= 2)
                {
                    e.Handled = true;
                }
            }
        }
        private void txtAltD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtAltI.Focus();
            }
        }
        private void txtAltD_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = sender as TextBox;

            // Permitir solo números, coma y teclas de control (como retroceso)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
                return;
            }

            // Solo permitir una coma
            if (e.KeyChar == ',' && txt.Text.Contains(","))
            {
                e.Handled = true;
                return;
            }

            // Verificar si ya hay coma y limitar los decimales a dos
            if (txt.Text.Contains(","))
            {
                int indexComa = txt.Text.IndexOf(",");
                string decimales = txt.Text.Substring(indexComa + 1);

                // Si hay 2 decimales y el cursor está después de la coma
                if (txt.SelectionStart > indexComa && decimales.Length >= 2)
                {
                    e.Handled = true;
                }
            }
        }

        private void txtDistVertice_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = sender as TextBox;

            // Permitir solo números, coma y teclas de control (como retroceso)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
                return;
            }

            // Solo permitir una coma
            if (e.KeyChar == ',' && txt.Text.Contains(","))
            {
                e.Handled = true;
                return;
            }

            // Verificar si ya hay coma y limitar los decimales a dos
            if (txt.Text.Contains(","))
            {
                int indexComa = txt.Text.IndexOf(",");
                string decimales = txt.Text.Substring(indexComa + 1);

                // Si hay 2 decimales y el cursor está después de la coma
                if (txt.SelectionStart > indexComa && decimales.Length >= 2)
                {
                    e.Handled = true;
                }
            }

        }

        private void txtDistVertice_Leave(object sender, EventArgs e)
        {
             var cultura = System.Globalization.CultureInfo.CurrentCulture;
                string texto = txtDistVertice.Text;

                // Expresión regular para máximo 2 decimales
                // Ejemplo válido: 12,34 — Ejemplo inválido: 12,345
                var regex = new System.Text.RegularExpressions.Regex(@"^\d{1,3}(,\d{1,2})?$");

                if (!regex.IsMatch(texto))
                {
                //_FrmMensajes.co = 2;
                //_FrmMensajes.avisomensaje("Ingrese un número válido con hasta 2 decimales");
                //_FrmMensajes.ShowDialog();
                txtDistVertice.Text = "0,00";
                    return;
                }

                // Validar rango con coma como separador decimal
                if (decimal.TryParse(texto, System.Globalization.NumberStyles.Number, cultura, out decimal valor))
                {
                    if (valor < 0 || valor > 30)
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("Valor fuera del rango permitido 0 a 30");
                        _FrmMensajes.ShowDialog();
                    txtDistVertice.Text = "0,00";
                    }
                }
                else
                {
                txtDistVertice.Text = "0,00";
                }
            
        }


        private void txtAngFac_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = sender as TextBox;

            // Permitir solo números, coma y teclas de control (como retroceso)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
                return;
            }

            // Solo permitir una coma
            if (e.KeyChar == ',' && txt.Text.Contains(","))
            {
                e.Handled = true;
                return;
            }

            // Verificar si ya hay coma y limitar los decimales a dos
            if (txt.Text.Contains(","))
            {
                int indexComa = txt.Text.IndexOf(",");
                string decimales = txt.Text.Substring(indexComa + 1);

                // Si hay 2 decimales y el cursor está después de la coma
                if (txt.SelectionStart > indexComa && decimales.Length >= 2)
                {
                    e.Handled = true;
                }
            }
        }

        private void txtAngFac_Leave(object sender, EventArgs e)
        {
            var cultura = System.Globalization.CultureInfo.CurrentCulture;
            string texto = txtAngFac.Text;

            // Expresión regular para máximo 2 decimales
            // Ejemplo válido: 12,34 — Ejemplo inválido: 12,345
            var regex = new System.Text.RegularExpressions.Regex(@"^\d{1,3}(,\d{1,2})?$");

            if (!regex.IsMatch(texto))
            {
                //_FrmMensajes.co = 2;
                //_FrmMensajes.avisomensaje("Ingrese un número válido con hasta 2 decimales");
                //_FrmMensajes.ShowDialog();
                txtAngFac.Text = "0,00";
                return;
            }

            // Validar rango con coma como separador decimal
            if (decimal.TryParse(texto, System.Globalization.NumberStyles.Number, cultura, out decimal valor))
            {
                if (valor < -5 || valor > 25)
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("Valor fuera del rango permitido -5 a 25");
                    _FrmMensajes.ShowDialog();
                    txtAngFac.Text = "0,00";
                }
            }
            else
            {
                txtAngFac.Text = "0,00";
            }

        }

        private void txtDll_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = sender as TextBox;

            // Permitir solo números, coma y teclas de control (como retroceso)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
                return;
            }

            // Solo permitir una coma
            if (e.KeyChar == ',' && txt.Text.Contains(","))
            {
                e.Handled = true;
                return;
            }

            // Verificar si ya hay coma y limitar los decimales a dos
            if (txt.Text.Contains(","))
            {
                int indexComa = txt.Text.IndexOf(",");
                string decimales = txt.Text.Substring(indexComa + 1);

                // Si hay 2 decimales y el cursor está después de la coma
                if (txt.SelectionStart > indexComa && decimales.Length >= 2)
                {
                    e.Handled = true;
                }
            }
        }
        private void txtDll_Leave(object sender, EventArgs e)
        {
            var cultura = System.Globalization.CultureInfo.CurrentCulture;
            string texto = txtDll.Text;

            // Expresión regular para máximo 2 decimales
            // Ejemplo válido: 12,34 — Ejemplo inválido: 12,345
            var regex = new System.Text.RegularExpressions.Regex(@"^\d{1,3}(,\d{1,2})?$");

            if (!regex.IsMatch(texto))
            {
                //_FrmMensajes.co = 2;
                //_FrmMensajes.avisomensaje("Ingrese un número válido con hasta 2 decimales");
                //_FrmMensajes.ShowDialog();
                txtDll.Text = "0,00";
                return;
            }

            // Validar rango con coma como separador decimal
            if (double.TryParse(texto, System.Globalization.NumberStyles.Number, cultura, out double valor))
            {
                if (valor < 0.25 || valor > 0.50)
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("Valor fuera del rango permitido 0,25 a 0,50");
                    _FrmMensajes.ShowDialog();
                    txtDll.Text = "0,00";
                }
            }
            else
            {
                txtDll.Text = "0,00";
            }

        }

        private void txtAngPant_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo dígitos y teclas de control (retroceso, etc.)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Bloquear cualquier carácter no numérico (incluye coma)
            }
        }

        private void txtAngPant_Leave(object sender, EventArgs e)
        {
            var cultura = System.Globalization.CultureInfo.CurrentCulture;
            string texto = txtAngPant.Text;

            // Expresión regular para máximo 2 decimales
            // Ejemplo válido: 12,34 — Ejemplo inválido: 12,345
            var regex = new System.Text.RegularExpressions.Regex(@"^\d{1,3}(,\d{1,2})?$");

            if (!regex.IsMatch(texto))
            {
                //_FrmMensajes.co = 2;
                //_FrmMensajes.avisomensaje("Ingrese un número válido con hasta 2 decimales");
                //_FrmMensajes.ShowDialog();
                txtAngPant.Text = "0";
                return;
            }

            // Validar rango con coma como separador decimal
            if (decimal.TryParse(texto, System.Globalization.NumberStyles.Number, cultura, out decimal valor))
            {
                if (valor < -5 || valor > 30)
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("Valor fuera del rango permitido -5 a 30");
                    _FrmMensajes.ShowDialog();
                    txtAngPant.Text = "0";
                }
            }
            else
            {
                txtAngPant.Text = "0";
            }

        }


        private void Lbl_Tap3_Articulo1_Click(object sender, EventArgs e)
        {

        }

        private void pnlObservCon_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnExamen_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtAngPant_TextChanged(object sender, EventArgs e)
        {

        }

        private void Txt_Tap3_Articulo_Precio_Validated(object sender, EventArgs e)
        {

            if ((Txt_Tap3_Articulo_Precio.Text == "Precio" && Txt_Tap3_Articulo_Precio.Enabled == true)|| string.IsNullOrEmpty(Txt_Tap3_Articulo_Precio.Text))
            {

                Txt_Tap3_Articulo_Precio.Text = "0,00";
            }
            else
            {
                FormatoBs(Convert.ToDouble(Txt_Tap3_Articulo_Precio.Text), Txt_Tap3_Articulo_Precio);

            }
        }

        private void Txt_Tap3_Articulo_Precio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8)
            {
                e.Handled = false;
                return;
            }


            bool IsDec = false;
            int nroDec = 0;


            if (Txt_Tap3_Articulo_Precio.SelectionLength <= 0)
            {

                for (int i = 0; i < Txt_Tap3_Articulo_Precio.Text.Length; i++)
                {
                    if (Txt_Tap3_Articulo_Precio.Text[i] == ',')
                        IsDec = true;

                    if (IsDec && nroDec++ >= 2)
                    {
                        e.Handled = true;
                        return;
                    }


                }
            }

            if (e.KeyChar >= 44 && e.KeyChar <= 57)
                e.Handled = false;
            ///46 = .
            ///46 = ,
            else if (e.KeyChar == 46)
                e.Handled = (IsDec) ? true : false;
            else
                e.Handled = true;


            //para que solo acepte numeros y una sola coma
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }


            if (Txt_Tap3_Articulo_Precio.Text.Contains("") && e.KeyChar == 44)
            {
                //separamos por punto
                string[] parts = Txt_Tap3_Articulo_Precio.Text.Split(',');
                //si el primer elemento está vacío, significa que no se escribió nada antes de, entonces, añadimos el cero al textbox.
                if (parts[0].Length <= 0)
                {
                    Txt_Tap3_Articulo_Precio.Text = "0" + Txt_Tap3_Articulo_Precio.Text;
                    //UPDATE: colocamos el cursor al final del texto
                    Txt_Tap3_Articulo_Precio.SelectionStart = Txt_Tap3_Articulo_Precio.Text.Length;
                }
            }
        }

        private void btnAutorizarRangosCrt_Click(object sender, EventArgs e)
        {
            _FrmClaveAutorizada.Nuevo_Parametro = true;
            _FrmClaveAutorizada.Id_Rol = "015";
            _FrmClaveAutorizada.ShowDialog();

            if (_FrmClaveAutorizada.DialogResult == DialogResult.OK && _FrmClaveAutorizada.ClaveCorrecta == true)
            {
                ApruebaAORangoCRT = true;
                pnlRangoCrt.Visible = false;
            }
        }

        private bool ValidoAlturaMedidasRevision()
        {
            try
            {
                //var gerenteRegio = new frmClaveAutorizada();
                bool altura = true;
                bool resultado = false;

                // Validación de altura - medida vertical de la montura
                if (Cbx_Tap2_Ojo.Text == "Ambos")
                {
                    if (!string.IsNullOrWhiteSpace(txtVertical.Text) &&
                        !string.IsNullOrWhiteSpace(txtAltD.Text) &&
                        !string.IsNullOrWhiteSpace(txtAltI.Text))
                    {
                        int vertical = Convert.ToInt32(txtVertical.Text);
                        int altD = Convert.ToInt32(txtAltD.Text);
                        int altI = Convert.ToInt32(txtAltI.Text);

                        if (cbVisionDerecha.Text == "Progresivo" && vertical - altD < 8)
                            altura = false;
                        else if (cbVisionIzquierda.Text == "Progresivo" && vertical - altI < 8)
                            altura = false;
                        else
                        {
                            altura = true;
                            resultado = true;
                        }
                    }
                }
                else if (Cbx_Tap2_Ojo.Text == "Derecho")
                {
                    if (!string.IsNullOrWhiteSpace(txtVertical.Text) &&
                        !string.IsNullOrWhiteSpace(txtAltD.Text))
                    {
                        int vertical = Convert.ToInt32(txtVertical.Text);
                        int altD = Convert.ToInt32(txtAltD.Text);

                        if (cbVisionDerecha.Text == "Progresivo" && vertical - altD < 8)
                            altura = false;
                        else
                        {
                            altura = true;
                            resultado = true;
                        }
                    }
                }
                else if (Cbx_Tap2_Ojo.Text == "Izquierdo")
                {
                    if (!string.IsNullOrWhiteSpace(txtVertical.Text) &&
                        !string.IsNullOrWhiteSpace(txtAltI.Text))
                    {
                        int vertical = Convert.ToInt32(txtVertical.Text);
                        int altI = Convert.ToInt32(txtAltI.Text);

                        if (cbVisionIzquierda.Text == "Progresivo" && vertical - altI < 8)
                            altura = false;
                        else
                        {
                            altura = true;
                            resultado = true;
                        }
                    }
                }
                else
                {
                    altura = true;
                    resultado = true;
                }

                if (!altura)
                {
                    _FrmMensajes.co = 3;
                    _FrmMensajes.avisomensaje("La Medida Vertical de la montura menos la Altura debe ser mayor o igual a 8\n¿Desea generar la venta con clave AUTORIZADA?");
                    _FrmMensajes.ShowDialog();

                    if (_FrmMensajes.DialogResult == DialogResult.OK)
                    {
                        _FrmClaveAutorizada.ShowDialog();

                        if (_FrmClaveAutorizada.DialogResult == DialogResult.OK && _FrmClaveAutorizada.ClaveCorrecta == true)
                        {
                            resultado = true;
                            _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "058", TB_USUARIO.COD_EMPLEADO, $"OS:  Altura D: {txtAltD.Text} Altura I: {txtAltI.Text} MVertical: {txtVertical.Text}, Autoriza: {TB_USUARIO.COD_EMPLEADO}");
                        }
                        else
                        {
                            resultado = false;
                        }

                    }

                }
                return resultado;
            }
            catch (Exception ex)
            {
                //MensajeError.MuestroMensaje(
                //    "Error en la función",
                //    "frmFacturas.VerificoCantidadProducto",
                //    "Por favor comunicarse con el Dpto de Sistemas y reportar el siguiente error: ",
                //    ex.Message,
                //    CapaNegocio.MensajesGenerales.TiposIconos.IconoError,
                //    glbUsuarioActual);

                //MensajeError.ShowDialog();
                return false;
            }
        }

        private bool ValidoAlturaMedidas()
        {
            try
            {
                bool altura = true;
                bool resultado = true;

                // Validación de altura - medida vertical de la montura
                if (Cbx_Tap2_Ojo.Text == "Ambos")
                {
                    if (!string.IsNullOrWhiteSpace(txtVertical.Text) &&
                        !string.IsNullOrWhiteSpace(txtAltD.Text) &&
                        !string.IsNullOrWhiteSpace(txtAltI.Text))
                    {
                        double vertical = Convert.ToDouble(txtVertical.Text);
                        double altD = Convert.ToDouble(txtAltD.Text);
                        double altI = Convert.ToDouble(txtAltI.Text);

                        if (Convert.ToInt32(txtAltD.Text) > 0 && (vertical - altD < 8))
                            altura = false;
                        else if (Convert.ToInt32(txtAltI.Text) > 0 && (vertical - altI < 8))
                            altura = false;
                        else
                        {
                            altura = true;
                            resultado = true;
                        }
                    }
                }
                else if (Cbx_Tap2_Ojo.Text == "Derecho")
                {
                    if (!string.IsNullOrWhiteSpace(txtVertical.Text) &&
                        !string.IsNullOrWhiteSpace(txtAltD.Text))
                    {
                        int vertical = Convert.ToInt32(txtVertical.Text);
                        int altD = Convert.ToInt32(txtAltD.Text);

                        if (Convert.ToInt32(txtAltD.Text) > 0 && (vertical - altD < 8))
                            altura = false;
                        else
                        {
                            altura = true;
                            resultado = true;
                        }
                    }
                }
                else if (Cbx_Tap2_Ojo.Text == "Izquierdo")
                {
                    if (!string.IsNullOrWhiteSpace(txtVertical.Text) &&
                        !string.IsNullOrWhiteSpace(txtAltI.Text))
                    {
                        int vertical = Convert.ToInt32(txtVertical.Text);
                        int altI = Convert.ToInt32(txtAltI.Text);

                        if (Convert.ToInt32(txtAltI.Text) > 0 && (vertical - altI < 8))
                            altura = false;
                        else
                        {
                            altura = true;
                            resultado = true;
                        }
                    }
                }
                else
                {
                    altura = true;
                    resultado = true;
                }

                if (!altura)
                {
                    _FrmMensajes.co = 3;
                    _FrmMensajes.avisomensaje("La Medida Vertical de la montura menos la Altura debe ser mayor o igual a 8\n¿Desea generar la venta con clave AUTORIZADA?");
                    _FrmMensajes.ShowDialog();

                    if (_FrmMensajes.DialogResult == DialogResult.OK)
                    {
                        _FrmClaveAutorizada.ShowDialog();

                        if (_FrmClaveAutorizada.DialogResult == DialogResult.OK && _FrmClaveAutorizada.ClaveCorrecta)
                        {
                            resultado = true;
                            _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "058", TB_USUARIO.COD_EMPLEADO,
                                $"OS:  Altura D: {txtAltD.Text} Altura I: {txtAltI.Text} MVertical: {txtVertical.Text}, Autoriza: {TB_USUARIO.COD_EMPLEADO}");
                        }
                        else
                        {
                            resultado = false;
                        }
                    }
                    else
                    {
                        resultado = false;
                    }
                }

                // Validación de cristales que llevan altura
                if (resultado) // Solo validar cristales si pasó la primera validación
                {
                    DataSet dsConsultaCristal;

                    foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
                    {
                        if (row.Cells["CodArticulo"].Value != null && (row.Cells["CodArticulo"].Value.ToString().StartsWith("C")))
                        {
                            string ojo = row.Cells["Ojo"].Value.ToString();
                            string codigo = row.Cells["CodArticulo"].Value.ToString();

                            dsConsultaCristal = _D_Articulo.CristalAltura(codigo);
                            string requiereAltura = dsConsultaCristal.Tables[0].Rows[0]["Altura"].ToString();

                            if (requiereAltura == "Con Altura")
                            {
                                if (ojo == "A" && (string.IsNullOrEmpty(txtAltD.Text) || txtAltD.Text == "0" ||
                                                  string.IsNullOrEmpty(txtAltI.Text) || txtAltI.Text == "0"))
                                {
                                    _FrmMensajes.avisomensaje($"El Cristal {codigo} debe llevar Altura. Repita el proceso y coloque la altura correspondiente");
                                    return false;
                                }
                                else if (ojo == "D" && (string.IsNullOrEmpty(txtAltD.Text) || txtAltD.Text == "0"))
                                {
                                    _FrmMensajes.avisomensaje($"El Cristal {codigo} debe llevar Altura. Repita el proceso y coloque la altura correspondiente");
                                    return false;
                                }
                                else if (ojo == "I" && (string.IsNullOrEmpty(txtAltI.Text) || txtAltI.Text == "0"))
                                {
                                    _FrmMensajes.avisomensaje($"El Cristal {codigo} debe llevar Altura. Repita el proceso y coloque la altura correspondiente");
                                    return false;
                                }
                            }
                        }
                    }
                }

                return resultado;
            }
            catch (Exception ex)
            {
                _FrmMensajes.avisomensaje($"Error al validar altura y medidas: {ex.Message}");
                return false;
            }
        }


        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button14_Click_1(object sender, EventArgs e)
        {
            if (rbDerecho.Checked == true)
            {
                ojoLenteContacto = "D";
            }
            else
            {
                ojoLenteContacto = "I";
            }
            pnlOjo.Visible = false;
            LcAmbosCant1 = true;
            CargarArticulos_Girdvew();
        }

        private void LLenarEntidadExamenFitcon()
        {//mcll

            nuevoFicconv = new TB_FICCONVCTE();
            nuevoExamen = new TB_EXAMENCTE();


                nuevoExamen.NUM_Examen = Convert.ToInt16( Txt_Tap2_Examen.Text);
                // Recopilar los datos de los controles del formulario
                nuevoFicconv.COD_Sucursal = codigoSucursal;
                nuevoFicconv.CTE_CedIden = Txt_Tap1_Cedula.Text.Trim();
                nuevoFicconv.CTE_Nacio = Cbx_Tap1_Nacionalidad.Text.Trim(); // Ajusta según cómo manejas la nacionalidad

                // Recopilar los datos de los controles del formulario
                nuevoTrabajo.TCEDIDEN = Txt_Tap1_Cedula.Text.Trim();
                nuevoTrabajo.TNACIO = Cbx_Tap1_Nacionalidad.Text.Trim(); // Ajusta según cómo manejas la nacionalidad

                nuevoExamen.CTE_CedIden = Txt_Tap1_Cedula.Text.Trim();
                nuevoExamen.CTE_Nacio = Cbx_Tap1_Nacionalidad.Text.Trim(); // Ajusta según cómo manejas la nacionalidad

                nuevoExamen.FEC_Examen = Dtp_Tap2_FecExam.Value;

                // Datos de Dgv_Pnl2_medoftal           
                if (Dgv_Pnl2_conv.Rows.Count > 0)
                {


                    nuevoExamen.ESFD = Dgv_Pnl2_conv.Rows[0].Cells["Esfera"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[0].Cells["Esfera"].Value) : 0;
                    nuevoExamen.ESFI = Dgv_Pnl2_conv.Rows[1].Cells["Esfera"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[1].Cells["Esfera"].Value) : 0;


                    nuevoExamen.CILD = Dgv_Pnl2_conv.Rows[0].Cells["Cilindro"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[0].Cells["Cilindro"].Value) : 0;
                    nuevoExamen.CILI = Dgv_Pnl2_conv.Rows[1].Cells["Cilindro"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[1].Cells["Cilindro"].Value) : 0;

                    nuevoExamen.EJED = Dgv_Pnl2_conv.Rows[0].Cells["Eje"]?.Value != null ? Convert.ToInt32(Dgv_Pnl2_conv.Rows[0].Cells["Eje"].Value) : 0;
                    nuevoExamen.EJEI = Dgv_Pnl2_conv.Rows[1].Cells["Eje"]?.Value != null ? Convert.ToInt32(Dgv_Pnl2_conv.Rows[1].Cells["Eje"].Value) : 0;

                    nuevoExamen.ADDD = Dgv_Pnl2_conv.Rows[0].Cells["Adicion"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[0].Cells["Adicion"].Value) : 0;
                    nuevoExamen.ADDI = Dgv_Pnl2_conv.Rows[1].Cells["Adicion"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[1].Cells["Adicion"].Value) : 0;

                    nuevoFicconv.PRISMAD = Dgv_Pnl2_conv.Rows[0].Cells["Prisma1"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[0].Cells["Prisma1"].Value) : 0;
                    nuevoFicconv.PRISMAI = Dgv_Pnl2_conv.Rows[1].Cells["Prisma1"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[1].Cells["Prisma1"].Value) : 0;


                    //nuevoExamen.CILD2 = Dgv_Pnl2_conv.Rows[1].Cells["Lejos"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[1].Cells["Lejos"].Value) : 0;
                    //nuevoExamen.CILI2 = Dgv_Pnl2_conv.Rows[1].Cells["Cerca"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[1].Cells["Cerca"].Value) : 0;
                    //nuevoExamen.OBSERVACIONES = Dgv_Pnl2_conv.Rows[1].Cells["Visual"]?.Value?.ToString();
                    nuevoExamen.ESFD2 = Dgv_Pnl2_conv.Rows[0].Cells["Prisma1"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[0].Cells["Prisma1"].Value) : 0;
                    nuevoExamen.ESFI2 = Dgv_Pnl2_conv.Rows[1].Cells["Prisma1"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[1].Cells["Prisma1"].Value) : 0;


                    nuevoTrabajo.TALTD = Convert.ToDecimal(txtAltD.Text);
                    nuevoTrabajo.TALTI = Convert.ToDecimal(txtAltI.Text);

                    //ALTD ALTI    PRISMAD PRISMAI  DPDL	DPDC	DPIL	DPIC



                    nuevoFicconv.DPDL = Dgv_Pnl2_conv.Rows[0].Cells["Lejos"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[0].Cells["Lejos"].Value) : 0;
                    nuevoFicconv.DPIL = Dgv_Pnl2_conv.Rows[1].Cells["Lejos"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[1].Cells["Lejos"].Value) : 0;


                    nuevoFicconv.DPDC = Dgv_Pnl2_conv.Rows[0].Cells["Cerca"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[0].Cells["Cerca"].Value) : 0;
                    nuevoFicconv.DPIC = Dgv_Pnl2_conv.Rows[1].Cells["Cerca"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[1].Cells["Cerca"].Value) : 0;



                    nuevoFicconv.PRISMAD = Dgv_Pnl2_conv.Rows[0].Cells["Prisma1"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[0].Cells["Prisma1"].Value) : 0;
                    nuevoFicconv.PRISMAI = Dgv_Pnl2_conv.Rows[1].Cells["Prisma1"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[1].Cells["Prisma1"].Value) : 0;


                    nuevoFicconv.ALTD = Convert.ToDecimal(txtAltD.Text);
                    nuevoFicconv.ALTI = Convert.ToDecimal(txtAltI.Text);

                    //nuevoFicconv.PBASED = Dgv_Pnl2_conv.Rows[0].Cells["Grado1"]?.Value.ToString();
                    nuevoFicconv.PBASED = Dgv_Pnl2_conv.Rows[0].Cells["Grado1"]?.Value?.ToString() == "0" ? "" : Dgv_Pnl2_conv.Rows[0].Cells["Grado1"]?.Value?.ToString();
                    nuevoFicconv.PBASEI = Dgv_Pnl2_conv.Rows[1].Cells["Grado1"]?.Value?.ToString() == "0" ? "" : Dgv_Pnl2_conv.Rows[1].Cells["Grado1"]?.Value?.ToString();


                    nuevoFicconv.AVD = Dgv_Pnl2_conv.Rows[0].Cells["VISUAL"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[0].Cells["VISUAL"].Value) : 0;
                    nuevoFicconv.AVI = Dgv_Pnl2_conv.Rows[1].Cells["VISUAL"]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_conv.Rows[1].Cells["VISUAL"].Value) : 0;


                    nuevoTrabajo.TTIPOVISIOND = cbVisionDerecha.Text;
                    nuevoTrabajo.TTIPOVISIONI = cbVisionIzquierda.Text;





                }

                nuevoExamen.OBSERVACIONES = txt_Pnl2_observa.Text.Trim();
                nuevoExamen.CodigoMimesys = txt_Pnl2_conv_mimesys.Text.Trim();
                nuevoExamen.COD_Sucursal = codigoSucursal;
                nuevoExamen.USER_CREA = TB_USUARIO.COD_USR;
                nuevoExamen.USER_MOD = TB_USUARIO.COD_USR;
                //METOD DE GUARDARR OFT
                //nuevoFicconv.OFTD = txt_Pnl2_oftd.Text.Trim();
                //nuevoFicconv.OFTI = txt_Pnl2_ofti.Text.Trim();

                nuevoTrabajo.TSucursal = codigoSucursal;
                nuevoTrabajo.TTIPOTRABAJO = "002";
                nuevoTrabajo.USERCREA = TB_USUARIO.COD_USR;

                //nuevoTrabajo.TEXAMEN = this.Txt_Pnl2_Examen.Text;
                //if (Dgv_Pnl2_medconv.Rows.Count > 0)
                //{

                //nuevoTrabajo.TDISTANCIAVERTICE = Dgv_Pnl2_medconv.Rows[0].Cells[0]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_medconv.Rows[0].Cells[0].Value) : 0;
                //nuevoTrabajo.TANGULOPANTOSCOPICO = Dgv_Pnl2_medconv.Rows[0].Cells[1]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_medconv.Rows[0].Cells[1].Value) : 0;
                //nuevoTrabajo.TANGULOFACIAL = Dgv_Pnl2_medconv.Rows[0].Cells[2]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_medconv.Rows[0].Cells[2].Value) : 0;
                //nuevoTrabajo.TDISTANCIADELECTURA = Dgv_Pnl2_medconv.Rows[0].Cells[3]?.Value != null ? Convert.ToDecimal(Dgv_Pnl2_medconv.Rows[0].Cells[3].Value) : 0;

                nuevoTrabajo.TDISTANCIAVERTICE = Convert.ToDecimal(txtDistVertice.Text);
                nuevoTrabajo.TANGULOPANTOSCOPICO = Convert.ToDecimal(txtAngPant.Text);
                nuevoTrabajo.TANGULOFACIAL = Convert.ToDecimal(txtAngFac.Text);
                nuevoTrabajo.TDISTANCIADELECTURA = Convert.ToDecimal(txtDll.Text);

                //}
                nuevoTrabajo.TOJO = Cbx_Tap2_Ojo.Text;

                nuevoExamen.TIPOEXAMEN = Cbx_Tap2_Tipo_Examen.Text;


                if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "01" || Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "02" || Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "09")
                {
                    nuevoTrabajo.TipoExamen = Cbx_Tap2_Tipo_Examen.Text;
                }
                else
                {
                    nuevoTrabajo.TipoExamen = "";
                }
                nuevoExamen.TIPO_Optm = Cbx_Tap2_Tipo_Optome.Text;



                if (string.IsNullOrEmpty(TXT_Tap2_Nombre_Optome.Text))
                {
                    nuevoExamen.NOM_Optm = Cbx_Tap2_Nombre_Optome.Text;
                }
                else
                {
                    nuevoExamen.NOM_Optm = TXT_Tap2_Nombre_Optome.Text;
                }



                nuevoFicconv.RETD = string.IsNullOrWhiteSpace(txt_Pnl2_retd.Text) ? null : txt_Pnl2_retd.Text.Trim();
                nuevoFicconv.RETI = string.IsNullOrWhiteSpace(txt_Pnl2_reti.Text) ? null : txt_Pnl2_reti.Text.Trim();
             

        }

        private void txt_Pnl2_conv_mimesys_Leave(object sender, EventArgs e)
        {
            // 1. Manejo seguro de la conversión del parámetro
            int longitudRequerida = ObtenerLongitudMimesysSegura();

            string cedula = Txt_Tap1_Cedula.Text;
            string nacionalidad = Cbx_Tap1_Nacionalidad.SelectedItem?.ToString();
            int idExamen;

            if (!int.TryParse(Txt_Tap2_Examen.Text, out idExamen))
            {
                return;
            }

            // Obtener los datos del examen usando el método que creaste
            D_Examen dExamen = new D_Examen();
            // ***CORRECCIÓN:***
            // Convierte idExamen a string antes de pasarlo al método.
            TB_EXAMENCTE examen = dExamen.ObtenerExamenPorNumeroYNacionalidadCedula(idExamen, nacionalidad, cedula);
            string COdigoMimesisBaseDatos = "0";
            if (examen != null)
                COdigoMimesisBaseDatos = examen.CodigoMimesys != null ? examen.CodigoMimesys : string.Empty;

            // 2. Validación de longitud solo si es un valor positivo
            if (COdigoMimesisBaseDatos != txt_Pnl2_conv_mimesys.Text && !string.IsNullOrEmpty(txt_Pnl2_conv_mimesys.Text) && longitudRequerida > 0 && txt_Pnl2_conv_mimesys.Text.Length != longitudRequerida)
            {
                MostrarMensajeLongitudIncorrecta(longitudRequerida);
            }
        }

        private int ObtenerLongitudMimesysSegura()
        {
            try
            {
                string valorParametro = _D_DetalleOrden.TB_PARAMETRO("CantDigMimesys");

                // Usar TryParse para conversión segura
                if (int.TryParse(valorParametro, out int longitud) && longitud > 0)
                {
                    return longitud;
                }

                // Log opcional para valores inválidos
                _FrmMensajes.avisomensaje($"Valor inválido para CantDigMimesys: {valorParametro}");
                return 0;
            }
            catch (Exception ex)
            {
                // Log del error si es necesario
                _FrmMensajes.avisomensaje($"Error al obtener CantDigMimesys: {ex.Message}");
                return 0;
            }
        }

        private void MostrarMensajeLongitudIncorrecta(int longitudRequerida)
        {
            _FrmMensajes.co = 2; // Código de tipo de mensaje

            // Mensaje más completo y profesional
            string mensaje = $"El código Mimesys debe contener exactamente {longitudRequerida} caracteres" ;

            _FrmMensajes.avisomensaje(mensaje);
            _FrmMensajes.ShowDialog();

            // Enfocar y seleccionar todo el texto para fácil corrección
            txt_Pnl2_conv_mimesys.Focus();
            txt_Pnl2_conv_mimesys.SelectAll();
        }

        private void txtHorizontal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtVertical.Focus();
               
            }
        }

        private void txtVertical_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtMaxima.Focus();

            }
        }

        private void txtMaxima_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPuente.Focus();

            }
        }

        private void txtPuente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtHorizontal.Focus();

            }
        }

        private void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (validandoCambioTab) return;

            // Si intentan cambiar manualmente al Tab 2 sin validar
            if (e.TabPageIndex == 2 && !ValidarDatosTab1())
            {
                e.Cancel = true;
                tabControl.SelectedIndex = 0;
            }
        }
        private bool ValidarDatosTab1()
        {
            // Misma lógica de validación que antes
            return !(Cbx_Tap1_TLF_Local.SelectedIndex == -1 && Cbx_Tap1_TLF_Celular.SelectedIndex == -1 || string.IsNullOrEmpty(Txt_Tap1_Email.Text.Trim()));
        }

        private void Dgv_Pnl2_cont_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (Dgv_Pnl2_cont.Columns[e.ColumnIndex].Name == "Diametro")
            //return;
            {

                var cell = Dgv_Pnl2_cont.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (!decimal.TryParse(cell.Value?.ToString(), out decimal val) ||
                    (val != 0M && (val < 8.5M || val > 14.5M)))
                {
                    cell.Value = 0;
                }
            }

            if (Dgv_Pnl2_cont.Columns[e.ColumnIndex].Name == "Adicion")
            //return;
            {
                var celladd = Dgv_Pnl2_cont.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (!decimal.TryParse(celladd.Value?.ToString(), out decimal valadd) ||
                    (valadd < 0.75M || valadd > 3.50M))
                {
                    celladd.Value = 0;
                }
            }
        }

        private void txtDistVertice_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                txtAngPant.Focus();

            }
        }

        private void txtDll_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtDistVertice.Focus();

            }
        }

        private void txtAngPant_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtAngFac.Focus();

            }
        }

        private void txtAngFac_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtDll.Focus();

            }
        }

        public void BotonesColor(bool modoClaro, string boton)
        {
            if (modoClaro)
            {
                switch (boton.ToLower()) // Convertir a minúsculas para comparación insensible a mayúsculas
                {
                    case "garantia":
                        AplicarColorBoton(true, Btn_Tap3_Garantia, "Garantia");
                        break;

                    case "descuento":
                        AplicarColorBoton(true, Btn_Tap3_Descuento, "Descuento");
                        break;

                    case "promocion":
                        AplicarColorBoton(true, Btn_Tap3_Promocion, "Promocion");
                        break;

                    case "monturapropia":
                    case "montura propia":
                        AplicarColorBoton(true, Btn_Tap3_MonturaPropia, "Montura Propia");
                        break;

                    case "cristalpropio":
                    case "cristal propio":
                        AplicarColorBoton(true, Btn_Tap3_CristalPropio, "Cristal Propio");
                        break;

                    case "clienteafiliado":
                    case "cliente afiliado":
                        AplicarColorBoton(true, Btn_Tap3_ClienteAfiliado, "Cliente Afiliado");
                        break;

                    case "cambioprecio":
                    case "cambio precio":
                        AplicarColorBoton(true, Btn_Tap3_CambioPrecio, "Cambio Precio");
                        break;

                    case "todos":
                        // Aplicar a todos los botones
                        AplicarColorBoton(true, Btn_Tap3_Garantia, "Garantia");
                        AplicarColorBoton(false, Btn_Tap3_CambioPrecio, "Cambio Precio");
                        AplicarColorBoton(false, Btn_Tap3_Descuento, "Descuento");
                        AplicarColorBoton(true, Btn_Tap3_Promocion, "Promocion");
                        AplicarColorBoton(true, Btn_Tap3_MonturaPropia, "Montura Propia");
                        AplicarColorBoton(true, Btn_Tap3_CristalPropio, "Cristal Propio");
                        AplicarColorBoton(true, Btn_Tap3_ClienteAfiliado, "Cliente Afiliado");
                        break;

                    default:
                        Console.WriteLine($"Nombre de botón no reconocido: {boton}");
                        break;
                }
            }
            else // Modo oscuro
            {
                switch (boton.ToLower())
                {
                    case "garantia":
                        AplicarColorBoton(false, Btn_Tap3_Garantia, "Garantia");
                        break;

                    case "descuento":
                        AplicarColorBoton(false, Btn_Tap3_Descuento, "Descuento");
                        break;

                    case "promocion":
                        AplicarColorBoton(false, Btn_Tap3_Promocion, "Promocion");
                        break;

                    case "monturapropia":
                    case "montura propia":
                        AplicarColorBoton(false, Btn_Tap3_MonturaPropia, "Montura Propia");
                        break;

                    case "cristalpropio":
                    case "cristal propio":
                        AplicarColorBoton(false, Btn_Tap3_CristalPropio, "Cristal Propio");
                        break;

                    case "clienteafiliado":
                    case "cliente afiliado":
                        AplicarColorBoton(false, Btn_Tap3_ClienteAfiliado, "Cliente Afiliado");
                        break;

                    case "cambioprecio":
                    case "cambio precio":
                        AplicarColorBoton(false, Btn_Tap3_CambioPrecio, "Cambio Precio");
                        break;

                    case "todos":
                        // Aplicar a todos los botones
                        AplicarColorBoton(false, Btn_Tap3_Garantia, "Garantia");
                        AplicarColorBoton(false, Btn_Tap3_CambioPrecio, "Cambio Precio");
                        AplicarColorBoton(false, Btn_Tap3_Descuento, "Descuento");
                        AplicarColorBoton(false, Btn_Tap3_Promocion, "Promocion");
                        AplicarColorBoton(false, Btn_Tap3_MonturaPropia, "Montura Propia");
                        AplicarColorBoton(false, Btn_Tap3_CristalPropio, "Cristal Propio");
                        AplicarColorBoton(false, Btn_Tap3_ClienteAfiliado, "Cliente Afiliado");
                        break;

                    default:
                        Console.WriteLine($"Nombre de botón no reconocido: {boton}");
                        break;
                }
            }
        }

        public void AplicarColorBoton(bool modoClaro, System.Windows.Forms.Button boton, string nombreBoton)
        {
            if (boton == null || string.IsNullOrEmpty(nombreBoton))
            {
                return;
            }

            // Diccionario de colores para modo CLARO (hexadecimal)
            var coloresOscuro = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase) // Ignora mayúsculas
    {
        { "Descuento", Color.FromArgb(0xEE, 0x94, 0x88) },
        { "Promocion", Color.FromArgb(0xFF, 0xD9, 0x66) },
        { "Montura Propia", Color.FromArgb(0x78, 0xAD, 0xDD) },
        { "Cristal Propio", Color.FromArgb(0xBE, 0xE3, 0x96) },
        { "Cliente Afiliado",  Color.FromArgb(244, 177, 131) },
        { "Cambio Precio", Color.FromArgb(178, 185, 255) },
        { "Garantia", Color.FromArgb(0xF4, 0x83, 0xA7) }
    };

            // Diccionario de colores para modo OSCURO
            var coloresClaro = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase)
    {
        { "Descuento", Color.OrangeRed },
        { "Cambio Precio", Color.FromArgb(65, 42, 156) },
        { "Promocion", Color.FromArgb(239, 184, 16) },
        { "Montura Propia", Color.FromArgb(21, 118, 187) },
        { "Cristal Propio", Color.FromArgb(92, 203, 95) },
        { "Cliente Afiliado", Color.FromArgb(255, 128, 0) },
        { "Garantia", Color.DeepPink }
    };

            var colores = modoClaro ? coloresClaro : coloresOscuro;

            if (colores.TryGetValue(nombreBoton, out Color color))
            {
                boton.BackColor = color;
                // fore color es para cambiar el color de las letras
                boton.ForeColor = Color.FromArgb(40, 40, 40);
                boton.UseVisualStyleBackColor = false; // Importante para que se vea el color
                boton.Refresh();
            }
            else
            {
                // Manejo de error o color por defecto
                boton.BackColor = SystemColors.Control;
                //boton.ForeColor = SystemColors.ControlText;
            }
        }


    }

}

 

