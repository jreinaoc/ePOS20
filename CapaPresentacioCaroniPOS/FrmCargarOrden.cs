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
namespace CapaVisual_Login
{
    public partial class FrmCargarOrden : Form
    {
        public FrmCargarOrden()
        {
            InitializeComponent();
        }


        // Declarar la lista para almacenar los resultados
        List<TB_ARTICULO> listaArticulos = new List<TB_ARTICULO>();
        // Lista temporal para relizar el filtrado 
        private List<TB_ARTICULO> listaTemporal = new List<TB_ARTICULO>();
        private List<TB_TRABAJO> _TRABAJO = new List<TB_TRABAJO>();
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
        private ServicioValidaciones_CargarOrdenes _servicioValidaciones = new ServicioValidaciones_CargarOrdenes();


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
        private bool tipoTrabajoSeleccionado;

        List<TB_EMPAFI> listaClienteAfiliados = new List<TB_EMPAFI>();

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

        private void Txt_Tap3_Articulo_Precio_Enter(object sender, EventArgs e)
        {
            // Cuando el usuario hace clic o intenta escribir
            if (Txt_Tap3_Articulo_Precio.Text == "Precio")
            {
                Txt_Tap3_Articulo_Precio.Text = ""; // Borrar el texto sugerido
                Txt_Tap3_Articulo_Precio.ForeColor = Color.Black; // Cambiar el color del texto a negro
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
                    radioButton2.Checked = false;
                    radioButton1.Checked = false;
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

                    break;

                case "MostrarCabezeraSecundaria":
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
                    this.Pnl_2.Location = new Point(0, 0); // Establecer posición en (0, 0)

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
                    this.Pnl_3_CambioPrecio.Location = new Point(500, 1);
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
                    this.btnCancelar3.Enabled = true;

                    // Botones del TapControl
                    this.btnPrincipal.Enabled = false;
                    this.btnExamen.Enabled = false;
                    this.btnCargarOrden.Enabled = false;

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
                    this.btnCancelar3.Enabled = false;

                    this.btnPrincipal.Enabled = true;
                    this.btnExamen.Enabled = true;
                    this.btnCargarOrden.Enabled = true;

                    // Botones Aciones 
                    ValidarRegistrosYHabilitar_Botones();

                    // Panel de Arriba
                    this.Txt_Pnl2_Cedula.Enabled = true;
                    this.Txt_Pnl2_Examen.Enabled = true;
                    //this.Cbx_Pnl2_Trbajo.Enabled = true;
                    this.Cbx_Pnl2_Laboratorio.Enabled = true;
                    this.Cbx_Pnl2_Servicio.Enabled = true;
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
                    this.Cbx_Pnl2_Servicio.Enabled = false;


                    // Controles del Panel Cambio Precio
                    this.Txt_Pnl3_CambioPrecioNuevo.Enabled = true;
                    this.Btn_Tap3_Cancelar_CambioPrecio.Enabled = true;
                    this.Btn_Tap3_Aceptar_CambioPrecio.Enabled = true;

                    // Botones del TapControl
                    this.btnPrincipal.Enabled = false;
                    this.btnExamen.Enabled = false;
                    this.btnCargarOrden.Enabled = false;

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
                    this.btnPrincipal.Enabled = false;
                    this.btnExamen.Enabled = false;
                    this.btnCargarOrden.Enabled = false;

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
                    this.btnPrincipal.Enabled = false;
                    this.btnExamen.Enabled = false;
                    this.btnCargarOrden.Enabled = false;

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
                    this.Cbx_Pnl2_Servicio.Enabled = false;

                    // Controles del Panel Promocion
                    this.Dgv_Pnl3_Promociones.Enabled = true;
                    this.Btn_Tap3_Cancelar_Promo.Enabled = true;
                    this.Btn_Tap3_Aceptar_Promo.Enabled = true;

                    // Botones del TapControl
                    this.btnPrincipal.Enabled = false;
                    this.btnExamen.Enabled = false;
                    this.btnCargarOrden.Enabled = false;

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
                    this.btnPrincipal.Enabled = false;
                    this.btnExamen.Enabled = false;
                    this.btnCargarOrden.Enabled = false;

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
                    this.Cbx_Pnl2_Servicio.Enabled = false;

                    // Controles del Panel ClienteAfiliado
                    this.Txt_Pnl3_ClienteAfiliado.Enabled = true;
                    this.Pnl_3_RadioButonClienteAfiliado.Enabled = true;
                    this.radioButton2.Enabled = true;
                    this.radioButton1.Enabled = true;
                    this.Dgv_Pnl3_ClienteAfiliado.Enabled = true;
                    this.btnCancelarAfiliado.Enabled = true;
                    this.radioButton2.Checked = true;

                    // Botones del TapControl
                    this.btnPrincipal.Enabled = false;
                    this.btnExamen.Enabled = false;
                    this.btnCargarOrden.Enabled = false;

                    // Botones Aciones 
                    this.Btn_Tap3_Descuento.Enabled = false;
                    this.Btn_Tap3_CambioPrecio.Enabled = false;
                    this.Btn_Tap3_Promocion.Enabled = false;
                    this.Btn_Tap3_MonturaPropia.Enabled = false;
                    this.Btn_Tap3_CristalPropio.Enabled = false;
                    this.Btn_Tap3_ClienteAfiliado.Enabled = false;
                    this.Btn_Tap3_Garantia.Enabled = false;

                    break;

            }


        }

        private void Txt_Tap3_Articulo_Codigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {

            }
        }

        private void Txt_Pnl3_Articulo_TextChanged(object sender, EventArgs e)
        {
            // Filtrar los datos según el texto ingresado en el TextBox
            _L_Articulo.FiltrarArticulos(Txt_Pnl3_Articulo.Text.ToLower(), Rd_Pnl3_Descripcion, Rd_Pnl3_Codigo, Dgv_Pnl3_Articulo, listaArticulos, listaTemporal);

        }


        private void Pnl_3_Lista_Articulo_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ObtenerArticulos()
        {

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

                        VisualizarPanel("MostrarCabezeraSecundaria");
                        HabilitacionControl("CabezeraPrincipal");

                        // Establecer el foco en el TextBox de cantidad
                        Txt_Tap3_Articulo_Cantidad.Focus();
                    }

                    if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "09")
                    {
                        AplicoGarantia(Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), Txt_Pnl2_Cedula.Text.Substring(0, 1), "", Txt_Pnl2_Examen.Text, _D_Inicio.Sucursal());
                    }
                }


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
            if (Txt_Tap3_Articulo_Precio.Text.StartsWith("C") ||  Txt_Tap3_Articulo_Precio.Text.StartsWith("M") || Txt_Tap3_Articulo_Precio.Text.StartsWith("L"))
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

           
            AgregarArticuloAlGrid();

            // Limpiar los TextBox
            LimpiarControles("Motro_Busqueda_Articulos");

            // Establecer el foco en el TextBox de código
            Txt_Tap3_Articulo_Codigo.Focus();

        }

        private void Txt_Tap3_Articulo_Cantidad_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Validar que el texto sea un número válido y mayor que 0
                if (int.TryParse(Txt_Tap3_Articulo_Cantidad.Text, out int cantidad) && cantidad > 0)
                {
                    CargarArticulos_Girdvew();
                }
            }
        }

        private void AgregarArticuloAlGrid()
        {
            try
            {
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
                    //depende del tipo mvision por ahora seteamos A
                    ojo = "A";
                }
                else
                {
                    //depende del tipo mvision por ahora seteamos A
                    ojo = "";
                }

                //_L_Articulo.AgregarFila(Dgv_Tap3_Articulo, articulo.CodArticulo, articulo.DESART, cantidad, (decimal) precio, (decimal)articulo.PORCTDESCUENTO, (decimal) total, impuesto, _Trabajo.T_OJO);
                _L_Articulo.AgregarFila(Dgv_Tap3_Articulo, articulo.CodArticulo, CodColorLC,articulo.DESART, cantidad, (decimal)precio, EmpresaAfiliada != "" && PorcDctoEmpresaAfiliada > 0 ? PorcDctoEmpresaAfiliada : (decimal)articulo.PORCTDESCUENTO, (decimal)total, impuesto, ojo, (decimal) articulo.COSTOPROME);


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
                // Cargar los valores
                ValidarRegistrosYHabilitar_Botones();
                Garantia = false;
                CodColorLC = "";
                // Botones Aciones 
               
                Cbx_Pnl2_Trbajo.Enabled = true;

                ReiniciarBusquedaarticulo();

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
                e.CellStyle.BackColor = Color.Red;
                DataGridViewButtonCell celBoton = this.Dgv_Tap3_Articulo.Rows[e.RowIndex].Cells["Eliminar"] as DataGridViewButtonCell;
                Icon IconAtomico;


                //if (ModoClaro == false)
                //{
                IconAtomico = new Icon(Environment.CurrentDirectory + @"\\Eliminar_Ordenes.ico");
                HabEliminar = true; //Se manda señal de boton ACTIVADO para realizar validaciones posteriores 
                                    //}
                                    //else
                                    //{
                                    //    IconAtomico = new Icon(Environment.CurrentDirectory + @"\\cuadraditoOscuro.ico");
                                    //     HabEliminar = true; //Se manda señal de boton ACTIVADO para realizar validaciones posteriores 
                                    //}


                e.Graphics.DrawIcon(IconAtomico, e.CellBounds.Left + 1, e.CellBounds.Top + 0);
                this.Dgv_Tap3_Articulo.Rows[e.RowIndex].Height = IconAtomico.Height + 0;
                this.Dgv_Tap3_Articulo.Columns[e.ColumnIndex].Width = IconAtomico.Width + 2;
                e.Handled = true;

            }

            Dgv_Tap3_Articulo.Size = new Size(1059, 150);
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Verificar si la pestaña seleccionada es la pestaña 3
            if (tabControl.SelectedIndex == 2) // El índice es 0-based, por lo que la pestaña 3 tiene índice 2
            {
                VisualizarPanel("MostrarCabezeraSecundaria");
                _L_Articulo.InicializarDataGridViewTotales(Dgv_Tap3_Totales);
                Formato_Dgv_Totales();
                _L_Articulo.BucarTipoVenta(Cbx_Pnl2_Trbajo);
            }
            else if (tabControl.SelectedIndex == 0)
            {
                VisualizarPanel("MostrarCabezeraPrincipal");
            }
            else if (tabControl.SelectedIndex == 1)
            {
                VisualizarPanel("MostrarCabezeraPrincipal");
            }
        }

        private void Txt_Tap3_Articulo_Codigo_KeyDown(object sender, KeyEventArgs e)
        {
            // Verificar si se presionó la tecla F2
            if (e.KeyCode == Keys.F2)
            {
                VisualizarPanel("Lista_Articulo");
                HabilitacionControl("Habilitar_Lista_Articulo");
                LimpiarControles("Abrir_Busqueda_Articulos");
                _L_Articulo.CargarArticulos(Dgv_Pnl3_Articulo, listaArticulos, Cbx_Pnl2_Trbajo.SelectedValue.ToString());
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
                _L_Articulo.CargarArticulos(Dgv_Pnl3_Articulo, listaArticulos, Cbx_Pnl2_Trbajo.SelectedValue.ToString());

                //Formatear los caracteres a 7 Digitos cuando es un cristal 
                _L_Articulo.FormatearCampo7Digitos(Txt_Tap3_Articulo_Codigo);

                // Buscar el articulo 
                _L_Articulo.FiltrarArticulos_Tap3(Txt_Tap3_Articulo_Codigo.Text, listaArticulos, listaTemporal, Txt_Tap3_Articulo_Codigo, Txt_Tap3_Articulo_Descripcion, Txt_Tap3_Articulo_Precio, Txt_Tap3_Articulo_Cantidad);

                if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "09")
                {
                    AplicoGarantia(Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), Txt_Pnl2_Cedula.Text.Substring(0, 1), "", Txt_Pnl2_Examen.Text, _D_Inicio.Sucursal());
                }

                // Evitar que el evento se propague
                e.Handled = true;
            }

            

        }

        private void Btn_Tap3_Cancelar_Click(object sender, EventArgs e)
        {
           
                LimpiarGrid();
        }

        private void btnPrincipal_CheckedChanged(object sender, EventArgs e)
        {
            tabControl.SelectTab(0);
        }

        private void btnExamen_CheckedChanged(object sender, EventArgs e)
        {
            tabControl.SelectTab(1);
        }

        private void btnDetalleOrden_CheckedChanged(object sender, EventArgs e)
        {
            tabControl.SelectTab(2);
            //codSucursal = _D_DetalleOrden.TB_PARAMETRO("SucursalId");
            _D_Articulo.Agregar_TB_TRABAJO(_D_DetalleOrden.TB_PARAMETRO("SucursalId"), "", "", Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), "002", Convert.ToInt32(Txt_Pnl2_Examen.Text)
              , txtHorizontal.Text, txtVertical.Text, txtMaxima.Text, txtPuente.Text, "0", "0", "A", "Cerca", "Cerca", "QUO", "001", "T", TB_USUARIO.COD_USR, "02", "CONVENCIONAL", "0", "0", "0", "0");

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
                    _L_Articulo.CargarServicioOPrima(Dgv_Tap3_Articulo, "Prisma", Convert.ToInt32(Txt_Pnl2_Examen.Text), Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), Dgv_Tap3_Articulo.Rows[numFilas].Cells["Ojo"].Value.ToString());
                    //_L_Articulo.CargarServicioOPrima(Dgv_Tap3_Articulo, "", Convert.ToInt32(Txt_Pnl2_Examen.Text), Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), Dgv_Tap3_Articulo.Rows[numFilas].Cells["Ojo"].Value.ToString());

                    if (Convert.ToInt32(Dgv_Tap3_Articulo.Rows[numFilas].Cells["ART_EXIST"].Value) == 2)
                    {
                        //Verifico Diotria
                        _L_Articulo.EvaluoServicioAgregado(Dgv_Tap3_Articulo, numFilas, "D", Convert.ToInt32(Txt_Pnl2_Examen.Text), Cbx_Pnl2_Trbajo.SelectedValue.ToString(), Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2));
                        _L_Articulo.EvaluoServicioAgregado(Dgv_Tap3_Articulo, numFilas, "I", Convert.ToInt32(Txt_Pnl2_Examen.Text), Cbx_Pnl2_Trbajo.SelectedValue.ToString(), Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2));

                    }
                    else
                    {
                        //Verifico Diotria
                        _L_Articulo.EvaluoServicioAgregado(Dgv_Tap3_Articulo, numFilas, Dgv_Tap3_Articulo.Rows[numFilas].Cells["Ojo"].Value.ToString(), Convert.ToInt32(Txt_Pnl2_Examen.Text), Cbx_Pnl2_Trbajo.SelectedValue.ToString(), Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2));
                    }

                    if (Montura_Propia == true)
                    {
                        _L_Articulo.CargarServicioMonturaPropia(Dgv_Tap3_Articulo, TipoMonturaPropia == "Completa");

                    }

                    if (Garantia == true)
                    {
                        _L_Articulo.CargarServicioGarantia(Dgv_Tap3_Articulo);

                    }

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
            ValidarRegistrosYHabilitar_Botones();

        }

        private void Dgv_Tap3_Articulo_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            // Verificar si eliminaron una Coloracion ; 
            // La palabra clave ref permite que la función modifique directamente la variable Codigo_Coloracion que se pasa desde la capa visual.
            _L_Articulo.RemoveColoracion(Dgv_Tap3_Articulo, ref Codigo_Coloracion);

            // Totalizo el grivew Totales cuando se quita una fila 
            _L_Articulo.ActualizarTotales(Dgv_Tap3_Articulo, Dgv_Tap3_Totales);

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

        private void Dgv_Tap3_Articulo_KeyDown(object sender, KeyEventArgs e)
        {
            // Verificar si se presionó la tecla F3 Para Abrir el Panel de Cambio Precio
            if (e.KeyCode == Keys.F3)
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
            else if (e.KeyCode == Keys.F4)
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
                        }

                    }
                }
                else
                {
                    _L_Articulo.ActualizarCelda(Dgv_Tap3_Articulo, filaSeleccionada, "ART_PVP", Txt_Pnl3_CambioPrecioNuevo.Text);
                    CerrarPanelCambioPrecio();
                }

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

            if  (Dgv_Tap3_Articulo.CurrentCell != null)
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

                                _L_Articulo.ActualizarCelda(Dgv_Tap3_Articulo, Dgv_Tap3_Articulo.CurrentRow.Index, "PORCTDESCUENTO", Txt_Pnl3_PorcDescuento.Text);

                            }
                        }

                        // Totalizo el grivew Totales cuando se agrega o modifica una fila 
                        _L_Articulo.ActualizarTotales(Dgv_Tap3_Articulo, Dgv_Tap3_Totales);
                        // Cierro el panel, limpio controles y Retorno a la pantalla primcipal 
                        CerrarPanelDescuento();

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
                    _FrmMensajes.avisomensaje("Se necesita una observación para continuar");
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
            if (Dgv_Tap3_Articulo.Rows.Count > 0 && PorcDctoEmpresaAfiliada <= 0)
            {
                Btn_Tap3_Descuento.Enabled = true; // Habilitar el TextBox o botón descuento
                Btn_Tap3_CambioPrecio.Enabled = true; // Habilitar el TextBox o botón cambioPrecio

            }
            else
            {
                Btn_Tap3_Descuento.Enabled = false; // Deshabilitar el TextBox o botón
                Btn_Tap3_CambioPrecio.Enabled = false;
            }

            //// Promociones
            if (string.IsNullOrEmpty(Codigo_Promocion) && Dgv_Tap3_Articulo.Rows.Count <= 0)
                Btn_Tap3_Promocion.Enabled = true;
            else
                Btn_Tap3_Promocion.Enabled = false;

            //// EmpresasAfiliadas
            if (string.IsNullOrEmpty(EmpresaAfiliada) && Dgv_Tap3_Articulo.Rows.Count <= 0)
                Btn_Tap3_ClienteAfiliado.Enabled = true;
            else
                Btn_Tap3_ClienteAfiliado.Enabled = false;


            // Montura Propia 
            _L_Articulo.VerificarMonturaPropia(Dgv_Tap3_Articulo, Btn_Tap3_MonturaPropia, Montura_Propia);

            // Cristal Propio
            _L_Articulo.VerificarCristalPropio(Dgv_Tap3_Articulo, Btn_Tap3_CristalPropio, Cristal_Propio);

            //Garantia
           
                Btn_Tap3_Garantia.Enabled = true;
            
           

       
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

        private async void Btn_Tap3_Procesar_Click(object sender, EventArgs e)
        {
            try
            {
                Btn_Tap3_Procesar.Enabled = false;

                int car;
                string strMarcaC;
                bool cambioPrec = false;
                glbServicio_NUV = glbServicio;
                SqlCommand command = null;

                // 1. Obtener códigos desde el grid
                var codigosFactura = ObtenerCodigosDesdeGrid();

                if (codigosFactura == null || codigosFactura.Count == 0)
                {
                    MessageBox.Show("No hay códigos válidos para procesar");
                    Btn_Tap3_Procesar.Enabled = true;
                    return;
                }

                ////2.Detectar el cristal(el primero que comience con "C")
                //string codCristal = codigosFactura.Find(c => c.StartsWith("C"));
                //if (string.IsNullOrWhiteSpace(codCristal))
                //{
                //    MessageBox.Show("No se encontró código de cristal");
                //    Btn_Tap3_Procesar.Enabled = true;
                //    return;
                //}

                // 2. Detectar el primer código de cristal (sin validación por "C")
                string codCristal = codigosFactura.FirstOrDefault(c => !string.IsNullOrWhiteSpace(c));

                if (string.IsNullOrWhiteSpace(codCristal))
                {
                    MessageBox.Show("No se encontró ningún código de cristal válido");
                    Btn_Tap3_Procesar.Enabled = true;
                    return;
                }

                //Consultar servicios AR
                var dsAR = await _servicioValidaciones.ObtenerServiciosARDataset(codCristal, false, command);

                if (_servicioValidaciones.VerificoIgualAntirefCrist(Dgv_Tap3_Articulo, dsAR))
                {
                    MessageBox.Show("La Cantidad de Antireflejos y Coloración debe ser igual a la Cantidad de Cristales.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Btn_Tap3_Procesar.Enabled = true;
                    return;
                }

                // Finaliza normalmente
                Btn_Tap3_Procesar.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al procesar coloración: " + ex.Message);
                Btn_Tap3_Procesar.Enabled = true;
            }

            string codSucursal;
            codSucursal = _D_DetalleOrden.TB_PARAMETRO("SucursalId");
            _D_Articulo.Agregar_TB_TRABAJO(codSucursal, "", "", Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), "002", Convert.ToInt32(Txt_Pnl2_Examen.Text)
              , txtHorizontal.Text, txtVertical.Text, txtMaxima.Text, txtPuente.Text, "0", "0", "A", "Cerca", "Cerca", "QUO", "001", "T", TB_USUARIO.COD_USR, "02", "CONVENCIONAL", "0", "0", "0", "0");

            VerificoParametrosCristales();
            VerificoRangoDiametroCristales();

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


        //////////////////////////////////////////////////////
        ///

        public void VerificoParametrosCristales()
        {
            string CristalI = "";
            string CristalD = "";
            bool AceptaCristalD = false;
            bool AceptaCristalI = false;
            string diamD = "";
            string diamI = "";
            DataSet dsParamCRT;
            DataSet dsParamCRT2;
            string Color = "";

            LbResultados.Items.Clear();
            LbResultado2.Items.Clear();

            _L_Articulo.LlenarTB_Trbajo(_TRABAJO, _D_Inicio.Sucursal(), Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2));
            var _Trabajo = _TRABAJO.FirstOrDefault(a => a.T_CEDIDEN == Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2) & a.T_NACIO == Txt_Pnl2_Cedula.Text.Substring(0, 1));


            for (int xx = 0; xx < Dgv_Tap3_Articulo.RowCount; xx++)
            {

                if (Dgv_Tap3_Articulo.Rows[xx].Cells["CodArticulo"].Value.ToString() == "S000004")
                {
                    Color = "SI";
                }
                else if (Dgv_Tap3_Articulo.Rows[xx].Cells["CodArticulo"].Value.ToString().StartsWith("C") && Dgv_Tap3_Articulo.Rows[xx].Cells["ojo"].Value.ToString() == "A")
                {
                    CristalD = Dgv_Tap3_Articulo.Rows[xx].Cells["CodArticulo"].Value.ToString();
                    CristalI = Dgv_Tap3_Articulo.Rows[xx].Cells["CodArticulo"].Value.ToString();
                }
                else if (Dgv_Tap3_Articulo.Rows[xx].Cells["CodArticulo"].Value.ToString().StartsWith("C") && Dgv_Tap3_Articulo.Rows[xx].Cells["ojo"].Value.ToString() == "D")
                {
                    CristalD = Dgv_Tap3_Articulo.Rows[xx].Cells["CodArticulo"].Value.ToString();
                }
                else if (Dgv_Tap3_Articulo.Rows[xx].Cells["CodArticulo"].Value.ToString().StartsWith("C") && Dgv_Tap3_Articulo.Rows[xx].Cells["ojo"].Value.ToString() == "I")
                {
                    CristalI = Dgv_Tap3_Articulo.Rows[xx].Cells["CodArticulo"].Value.ToString();
                }
            }

            string lab = "QUO";

            LbResultados.Items.Add("OJO DERECHO");
            LbResultado2.Items.Add("OJO IZQUIERDO");

            for (int xx = 0; xx < Dgv_Tap3_Articulo.RowCount; xx++)
            {
                if (Dgv_Tap3_Articulo.Rows[xx].Cells["CodArticulo"].Value.ToString().StartsWith("C"))
                {
                    if (Dgv_Tap3_Articulo.Rows[xx].Cells["Ojo"].Value.ToString() == "A")
                    {
                        dsParamCRT = _D_Articulo.MostrarRangosCrtGrid(Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), Txt_Pnl2_Examen.Text, Dgv_Tap3_Articulo.Rows[xx].Cells["CodArticulo"].Value.ToString(), "D", _Trabajo.T_TIPOVISIOND, Convert.ToDecimal(_Trabajo.T_ALTD), 0, Convert.ToDecimal(_Trabajo.T_DISTANCIAVERTICE), Convert.ToDecimal(_Trabajo.T_ANGULOFACIAL), Convert.ToDecimal(_Trabajo.T_ANGULOPANTOSCOPICO), "", _Trabajo.T_SERVICIO, _Trabajo.T_LABORATORIO, Convert.ToString(_Trabajo.T_DISTANCIAVERTICE), Convert.ToString(_Trabajo.T_ANGULOFACIAL), Convert.ToString(_Trabajo.T_ANGULOPANTOSCOPICO), Convert.ToString(_Trabajo.T_DISTANCIADELECTURA));
                        dsParamCRT2 = _D_Articulo.MostrarRangosCrtGrid(Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), Txt_Pnl2_Examen.Text, Dgv_Tap3_Articulo.Rows[xx].Cells["CodArticulo"].Value.ToString(), "I", _Trabajo.T_TIPOVISIONI, Convert.ToDecimal(_Trabajo.T_ALTI), 0, Convert.ToDecimal(_Trabajo.T_DISTANCIAVERTICE), Convert.ToDecimal(_Trabajo.T_ANGULOFACIAL), Convert.ToDecimal(_Trabajo.T_ANGULOPANTOSCOPICO), "", _Trabajo.T_SERVICIO, _Trabajo.T_LABORATORIO, Convert.ToString(_Trabajo.T_DISTANCIAVERTICE), Convert.ToString(_Trabajo.T_ANGULOFACIAL), Convert.ToString(_Trabajo.T_ANGULOPANTOSCOPICO), Convert.ToString(_Trabajo.T_DISTANCIADELECTURA));

                        // Validación de parámetros

                        if (Enumerable.Range(0, 13).All(x => dsParamCRT.Tables[2].Rows[0][x].ToString() == "1"))
                        {
                            AceptaCristalD = true;
                        }
                        else
                        {
                            AceptaCristalD = false;
                            for (int x = 0; x <= 16; x++)
                            {
                                if (dsParamCRT.Tables[2].Rows[0][x].ToString() != "1")
                                {
                                    LbResultados.Items.Add(dsParamCRT.Tables[2].Rows[0][x].ToString());
                                }
                            }
                        }

                        if (Enumerable.Range(0, 17).All(x => dsParamCRT2.Tables[2].Rows[0][x].ToString() == "1"))
                        {
                            AceptaCristalI = true;
                        }
                        else
                        {
                            AceptaCristalI = false;
                            for (int x = 0; x <= 16; x++)
                            {
                                if (dsParamCRT2.Tables[2].Rows[0][x].ToString() != "1")
                                {
                                    LbResultado2.Items.Add(dsParamCRT2.Tables[2].Rows[0][x].ToString());
                                }
                            }
                        }
                    }
                    else if (Dgv_Tap3_Articulo.Rows[xx].Cells["Ojo"].Value.ToString() == "D")
                    {
                        dsParamCRT = _D_Articulo.MostrarRangosCrtGrid(Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), Txt_Pnl2_Examen.Text, Dgv_Tap3_Articulo.Rows[xx].Cells["CodArticulo"].Value.ToString(), "D", _Trabajo.T_TIPOVISIOND, Convert.ToDecimal(_Trabajo.T_ALTD), 0, Convert.ToDecimal(_Trabajo.T_DISTANCIAVERTICE), Convert.ToDecimal(_Trabajo.T_ANGULOFACIAL), Convert.ToDecimal(_Trabajo.T_ANGULOPANTOSCOPICO), "", _Trabajo.T_SERVICIO, _Trabajo.T_LABORATORIO, Convert.ToString(_Trabajo.T_DISTANCIAVERTICE), Convert.ToString(_Trabajo.T_ANGULOFACIAL), Convert.ToString(_Trabajo.T_ANGULOPANTOSCOPICO), Convert.ToString(_Trabajo.T_DISTANCIADELECTURA));

                        AceptaCristalD = Enumerable.Range(0, 17).All(x => dsParamCRT.Tables[2].Rows[0][x].ToString() == "1");
                        AceptaCristalI = AceptaCristalD;

                        if (!AceptaCristalD)
                        {
                            for (int x = 0; x <= 16; x++)
                            {
                                if (dsParamCRT.Tables[2].Rows[0][x].ToString() != "1")
                                {
                                    LbResultados.Items.Add(dsParamCRT.Tables[2].Rows[0][x].ToString());
                                }
                            }
                        }
                    }
                    else if (Dgv_Tap3_Articulo.Rows[xx].Cells["Ojo"].Value.ToString() == "I")
                    {
                        dsParamCRT2 = _D_Articulo.MostrarRangosCrtGrid(Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), Txt_Pnl2_Examen.Text, Dgv_Tap3_Articulo.Rows[xx].Cells["CodArticulo"].Value.ToString(), "I", _Trabajo.T_TIPOVISIONI, Convert.ToDecimal(_Trabajo.T_ALTI), 0, Convert.ToDecimal(_Trabajo.T_DISTANCIAVERTICE), Convert.ToDecimal(_Trabajo.T_ANGULOFACIAL), Convert.ToDecimal(_Trabajo.T_ANGULOPANTOSCOPICO), "", _Trabajo.T_SERVICIO, _Trabajo.T_LABORATORIO, Convert.ToString(_Trabajo.T_DISTANCIAVERTICE), Convert.ToString(_Trabajo.T_ANGULOFACIAL), Convert.ToString(_Trabajo.T_ANGULOPANTOSCOPICO), Convert.ToString(_Trabajo.T_DISTANCIADELECTURA));

                        AceptaCristalI = Enumerable.Range(0, 17).All(x => dsParamCRT2.Tables[2].Rows[0][x].ToString() == "1");
                        AceptaCristalD = AceptaCristalI;

                        if (!AceptaCristalI)
                        {
                            for (int x = 0; x <= 16; x++)
                            {
                                if (dsParamCRT2.Tables[2].Rows[0][x].ToString() != "1")
                                {
                                    LbResultado2.Items.Add(dsParamCRT2.Tables[2].Rows[0][x].ToString());
                                }
                            }
                        }
                    }
                }
            }

            bool VerificoParametrosCristales;
            if (AceptaCristalD == true && AceptaCristalI == true)
            {
                VerificoParametrosCristales = true;
            }
            else if (AceptaCristalD == false || AceptaCristalI == false)
            {
                VerificoParametrosCristales = false;

                DataSet dsConsultaCristal = _D_Articulo.MostrarParametrosCrtGrid(CristalD, CristalI);

                dgvRangoCrt.DataSource = dsConsultaCristal.Tables[0];
                FormatoDataGridRangosCristales();

                lblDiametroD.Visible = false;
                lblDiametroI.Visible = false;

                LblTitulo.Text = "El Cristal no se adapta a estos parámetros";
                lblClaveAut.Visible = false;
                lblLeyenda.Visible = true;
                // lblLeyenda.Text = "P: Puede usarse según el Examen. NC: No corresponde. X: Aplica";
                btnAutorizarRangosCrt.Visible = false;
                //BtCancelarRgo.Visible = false;
                //BtRegresar.Visible = true;
                LbResultados.Visible = true;
                LbResultado2.Visible = true;

                pnlRangoCrt.Show();
                pnlRangoCrt.Location = new Point(200,150);
            }


            //DataSet RangosCrt = _L_Facturacion.MostrarRangosCrtGrid(TB_CAORDSER.Cod_Sucursal, txtNumeroOrden.Text, TB_CAORDSER.Revision);

            //if (RangosCrt.Tables[0].Rows.Count > 0)
            //{
            //    pnlRangoCrt.Visible = true;
            //    CantAbonosPrevios = RangosCrt.Tables[0].Rows.Count;
            //    dgvRangoCrt.DataSource = RangosCrt;
            //    FormatoDataGridRangosCristales();
            //}
        }

        private void FormatoDataGridRangosCristales()
        {

            try
            {

                //Centrar todas las colucnas 
                //DgvListadoOrdenes.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                // Verificar y agregar columnas si no existen


                //asignar Nombres a cada colucna 
                //dgvRangoCrt.Columns.Add("Cristal", "Esfera/Cilindro");
                //dgvRangoCrt.Columns.Add("EsfCil", "Esfera/Cilindro");
                //dgvRangoCrt.Columns.Add("Lejos", "Lejos");
                //dgvRangoCrt.Columns.Add("Cerca", "Cerca");
                //dgvRangoCrt.Columns.Add("Bifocal", "Bifocal");
                //dgvRangoCrt.Columns.Add("Progresivo", "Progresivo");
                //dgvRangoCrt.Columns.Add("Balance", "Balance");
                //dgvRangoCrt.Columns.Add("DPLejos", "DP Lejos");
                //dgvRangoCrt.Columns.Add("DPCerca", "DP Cerca");
                //dgvRangoCrt.Columns.Add("AddMin", "Adición Min");
                //dgvRangoCrt.Columns.Add("AddMax", "Adición Max");
                //dgvRangoCrt.Columns.Add("AltMin", "Altura Min");
                //dgvRangoCrt.Columns.Add("AltMax", "Altura Max");
                //dgvRangoCrt.Columns.Add("PrismaMin", "Prisma Min");
                //dgvRangoCrt.Columns.Add("PrismaMax", "Prisma Max");
                //dgvRangoCrt.Columns.Add("DiamMax", "Diámetro Max");
                //dgvRangoCrt.Columns.Add("DVCMin", "DVC Min");
                //dgvRangoCrt.Columns.Add("DVCMax", "DVC Max");
                //dgvRangoCrt.Columns.Add("AFMin", "Ángulo Facial Min");
                //dgvRangoCrt.Columns.Add("AFMax", "Ángulo Facial Max");
                //dgvRangoCrt.Columns.Add("APMin", "Ángulo Pantoscópico Min");
                //dgvRangoCrt.Columns.Add("APMax", "Ángulo Pantoscópico Max");
                //dgvRangoCrt.Columns.Add("ColorSi", "Color Sí");
                //dgvRangoCrt.Columns.Add("ColorNo", "Color No");
                //dgvRangoCrt.Columns.Add("Express1Hr", "Express 1 Hora");
                //dgvRangoCrt.Columns.Add("Express3Hr", "Express 3 Horas");
                //dgvRangoCrt.Columns.Add("Express12Hr", "Express 12 Horas");
                //dgvRangoCrt.Columns.Add("5Dias", "5 Días");
                //dgvRangoCrt.Columns.Add("7DiasHab", "7 Días Hábiles");
                //dgvRangoCrt.Columns.Add("15Dias", "15 Días");
                //dgvRangoCrt.Columns.Add("30Dias", "30 Días");
                //dgvRangoCrt.Columns.Add("21Dias", "21 Días");
                //dgvRangoCrt.Columns.Add("45Dias", "45 Días");
                //dgvRangoCrt.Columns.Add("60Dias", "60 Días");
                //dgvRangoCrt.Columns.Add("90Dias", "90 Días");
                //dgvRangoCrt.Columns.Add("MontRemoto", "Montaje Remoto");
                //dgvRangoCrt.Columns.Add("MontQuorum", "Montaje Quorum");


                //dgvRangoCrt.Columns["linea"].HeaderText = "Linea";
                //dgvRangoCrt.Columns["activo"].HeaderText = "Activo";
                //dgvRangoCrt.Columns["codCristalOptica"].HeaderText = "Código Cristal Óptica";
                //dgvRangoCrt.Columns["esferaMin"].HeaderText = "Esfera Min";
                //dgvRangoCrt.Columns["esferaMax"].HeaderText = "Esfera Max";
                //dgvRangoCrt.Columns["cilindroMin"].HeaderText = "Cilindro Min";
                //dgvRangoCrt.Columns["cilindroMax"].HeaderText = "Cilindro Max";
                //dgvRangoCrt.Columns["sumatoriaMin"].HeaderText = "Sumatoria Min";
                //dgvRangoCrt.Columns["diametroMax"].HeaderText = "Diámetro Max";
                //dgvRangoCrt.Columns["adicionMin"].HeaderText = "Adición Min";
                //dgvRangoCrt.Columns["adicionMax"].HeaderText = "Adición Max";
                //dgvRangoCrt.Columns["alturaMin"].HeaderText = "Altura Min";
                //dgvRangoCrt.Columns["alturaMax"].HeaderText = "Altura Max";
                //dgvRangoCrt.Columns["impresora"].HeaderText = "Impresora";
                //dgvRangoCrt.Columns["EVDCODE"].HeaderText = "EVD Code";
                //dgvRangoCrt.Columns["codCalculo"].HeaderText = "Código Cálculo";

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

                //Bloquear Columna 
                //DgvListadoOrdenes.Columns["linea"].ReadOnly = true;
                //    DgvListadoOrdenes.Columns["Abo_Tipo"].ReadOnly = true;
                //    DgvListadoOrdenes.Columns["Abo_Monto"].ReadOnly = true;

                //Posicion  
                //DgvListadoOrdenes.Columns["Fecha"].DisplayIndex = 0;
                //DgvListadoOrdenes.Columns["Abo_Tipo"].DisplayIndex = 1;
                //DgvListadoOrdenes.Columns["Abo_Monto"].DisplayIndex = 2;
                //DgvListadoOrdenes.Columns["Eliminar"].DisplayIndex = 3;
                //DgvListadoOrdenes.Columns["Tipo_Pago"].DisplayIndex = 4;
                //DgvListadoOrdenes.Columns["Fec_Crea"].DisplayIndex = 5;
                //DgvListadoOrdenes.Columns["ID_Abono"].DisplayIndex = 6;

                //Alineación

                //dgvRangoCrt.Columns["linea"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //dgvRangoCrt.Columns["activo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //dgvRangoCrt.Columns["codCristalOptica"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //dgvRangoCrt.Columns["esferaMin"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //dgvRangoCrt.Columns["esferaMax"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //dgvRangoCrt.Columns["cilindroMin"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //dgvRangoCrt.Columns["cilindroMax"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //dgvRangoCrt.Columns["sumatoriaMin"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //dgvRangoCrt.Columns["diametroMax"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //dgvRangoCrt.Columns["adicionMin"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //dgvRangoCrt.Columns["adicionMax"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //dgvRangoCrt.Columns["alturaMin"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //dgvRangoCrt.Columns["alturaMax"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //dgvRangoCrt.Columns["impresora"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //dgvRangoCrt.Columns["EVDCODE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //dgvRangoCrt.Columns["codCalculo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


                //dgvRangoCrt.Columns["linea"].Visible = false;
                //dgvRangoCrt.Columns["activo"].Visible = false;

                //if (TB_CAORDSER.OrSer_Status != "005")
                //{
                //    DgvListadoOrdenes.Columns["Eliminar"].Visible = false;
                //    DgvListadoOrdenes.Columns["Fecha"].Width = 360;
                //    DgvListadoOrdenes.Columns["Abo_Tipo"].Width = 350;
                //    DgvListadoOrdenes.Columns["Abo_Monto"].Width = 360;
                //}


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

        private void button1_Click(object sender, EventArgs e)
        {
            string codSucursal;
            codSucursal = _D_DetalleOrden.TB_PARAMETRO("SucursalId");
            _D_Articulo.Agregar_TB_TRABAJO(codSucursal, "", "", Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), "002", Convert.ToInt32(Txt_Pnl2_Examen.Text)
              , txtHorizontal.Text, txtVertical.Text, txtMaxima.Text, txtPuente.Text, "0", "0", "A", "Cerca", "Cerca", "QUO", "001", "T", TB_USUARIO.COD_USR, "02", "CONVENCIONAL", "0", "0", "0", "0");

        }

        private void btnAutorizarRangosCrt_Click(object sender, EventArgs e)
        {

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
                CerrarPanelMonturaPropia();
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

        }

        private void Btn_Tap3_CristalPropio_Click(object sender, EventArgs e)
        {
            Cristal_Propio = true;
            HabilitacionControl("CabezeraPrincipal");

        }

        private void QuitarLimea2_Click(object sender, EventArgs e)
        {

        }

        private void FrmCargarOrden_Load(object sender, EventArgs e)
        {

            _L_Articulo.CargarClientesAfiliados(Dgv_Pnl3_ClienteAfiliado, listaClienteAfiliados);

            Dgv_Pnl3_ClienteAfiliado.DataSource = listaClienteAfiliados;
            Formato_Dgv_Pnl3_ClienteAfiliado();
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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

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
                CerrarPanelPromocion();
            }
            else
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Debe seleccionar una promocion para continuar");
                _FrmMensajes.ShowDialog();
            }

        }

        public void VerificoRangoDiametroCristales()
        {
            bool AceptaCristalD = false;
            bool AceptaCristalI = false;
            string CristalD = "";
            string CristalI = "";
            string Montura = "";
            string diamD = "";
            string diamI = "";

            for (int xx = 0; xx < Dgv_Tap3_Articulo.RowCount; xx++)
            {
                var row = Dgv_Tap3_Articulo.Rows[xx];
                if (row.Cells["CodArticulo"].Value.ToString().StartsWith("C"))
                {
                    switch (row.Cells["Ojo"].Value.ToString())
                    {
                        case "A":
                            CristalD = CristalI = row.Cells["CodArticulo"].Value.ToString();
                            break;
                        case "D":
                            CristalD = row.Cells["CodArticulo"].Value.ToString();
                            break;
                        case "I":
                            CristalI = row.Cells["CodArticulo"].Value.ToString();
                            break;
                    }
                }
                else if (row.Cells["CodArticulo"].Value.ToString().StartsWith("M"))
                {
                    Montura = row.Cells["CodArticulo"].Value.ToString();
                }
            }

            DataSet dsDiametroEfectivo;

            _L_Articulo.LlenarTB_Trbajo(_TRABAJO, _D_Inicio.Sucursal(), Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2));
            var _Trabajo = _TRABAJO.FirstOrDefault(a => a.T_CEDIDEN == Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2) & a.T_NACIO == Txt_Pnl2_Cedula.Text.Substring(0, 1));


            dsDiametroEfectivo = _D_Articulo.MostrarDiametroEfectivoCrtGrid(Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), Txt_Pnl2_Examen.Text,CristalD, CristalI, "A",_Trabajo.T_TIPOVISIOND, _Trabajo.T_TIPOVISIONI,Montura, txtHorizontal.Text.Replace(".", ""),  txtMaxima.Text.Replace(".", ""),  txtPuente.Text.Replace(".", ""), _D_Inicio.Sucursal());


            //DataSet dsDiametroEfectivo = ManBD.EjecutaStoreProcedure("pGetDiametroEfectivo",
            //    $"{txtNacRif.Text}','{Cedula}','{NumExamen}','{CristalD}','{CristalI}','{TxtOjo.Text.Substring(0, 1)}','{TxtTipoVisionD.Text.ToUpper()}','{TxtTipoVisionI.Text.ToUpper()}','{Montura}',{txtHorizontal.Text.Replace(".", "")}, {txtMaxima.Text.Replace(".", "")}, {txtPuente.Text.Replace(".", "")}, '{glbSucursalActual}'");

            if (Convert.ToInt32(dsDiametroEfectivo.Tables[1].Rows[0][0]) > 0)
            {
                diamD = dsDiametroEfectivo.Tables[1].Rows[0]["DIAMETROEFECTIVODERECHO"].ToString();
                diamI = dsDiametroEfectivo.Tables[1].Rows[0]["DIAMETROEFECTIVOIZQUIERDO"].ToString();
            }
            else
            {
                diamD = "0";
                diamI = "0";
            }

            if (CristalD.StartsWith("C") || CristalI.StartsWith("C"))
            {
                DataSet dsValidaciones;


                 dsValidaciones = _D_Articulo.MostrarValidaRangoCrtGrid(Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2), Txt_Pnl2_Examen.Text, "A", CristalD, CristalI, _Trabajo.T_ALTD, _Trabajo.T_ALTI, _Trabajo.T_TIPOVISIOND, _Trabajo.T_TIPOVISIONI, diamD, diamI);


                //= ManBD.EjecutaStoreProcedure("pValidarangoCristal",
                //                    $"{txtNacRif.Text}', '{Cedula}', '{NumExamen}', '{TxtOjo.Text.Substring(0, 1)}', '{CristalD}', '{CristalI}', {txtAltD.Text.Replace(",", ".")}, {txtAltI.Text.Replace(",", ".")}, '{TxtTipoVisionD.Text.ToUpper()}', '{TxtTipoVisionI.Text.ToUpper()}', '{diamD}', '{diamI}'");

                string ojo = "A"; //TxtOjo.Text.Value.ToString().Substring(0, 1).ToUpper();
                if (ojo == "A")
                {
                    AceptaCristalD = dsValidaciones.Tables[0].Rows.Count > 0;
                    AceptaCristalI = dsValidaciones.Tables[1].Rows.Count > 0;
                }
                else if (ojo == "D")
                {
                    AceptaCristalD = dsValidaciones.Tables[0].Rows.Count > 0;
                    AceptaCristalI = true;
                }
                else if (ojo == "I")
                {
                    AceptaCristalI = dsValidaciones.Tables[0].Rows.Count > 0;
                    AceptaCristalD = true;
                }

                if (!AceptaCristalD || !AceptaCristalI)
                {
                    //DataSet dsConsultaCristal = ManBD.EjecutaStoreProcedure("pGetRangoCristal", $"{CristalD}', '{CristalI}'");
                    DataSet dsConsultaCristal = _D_Articulo.MostrarParametrosCrtGrid(CristalD, CristalI);
                    //dgvRangoCrt.ClearStructure();
                    dgvRangoCrt.DataSource = dsConsultaCristal.Tables[0];
                    //dgvRangoCrt.RetrieveStructure();
                    //FormatoTablaRango();

                    lblDiametroD.Text = diamD;
                    lblDiametroI.Text = diamI;

                    lblDiametroD.Visible = lblDiametroI.Visible = true;
                    LblTitulo.Text = "El cristal seleccionado no se adapta a los siguientes rangos:";
                    lblClaveAut.Visible = true;
                    lblLeyenda.Visible = false;
                    btnAutorizarRangosCrt.Visible =  true;
                    pnlRangoCrt.Show();
                }
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
                }
            }
        }

        private void Btn_Tap3_Garantia_Click(object sender, EventArgs e)
        {
            Garantia = true;
            HabilitacionControl("CabezeraPrincipal");
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
                Dgv_Pnl3_ColoresLC.Columns["DESCRIPCIONCOLOR"].Width = 500;


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
                Dgv_Pnl3_ClienteAfiliado.Columns["Nombre"].Width = 500;


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

        private void Cbx_Pnl2_Trbajo_SelectedIndexChanged(object sender, EventArgs e)
        {
           if (tipoTrabajoSeleccionado)
                {
                Cbx_Pnl2_Trbajo.Enabled = false;
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

        private void textBox2_Click(object sender, EventArgs e)
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
            TextBox textBox = sender as TextBox;

            if (decimal.TryParse(textBox.Text, out decimal valor))
            {
                // Formatear el texto como un número con dos decimales
                textBox.Text = valor.ToString("N2");
            }
        }
    }


    
    
}
 

