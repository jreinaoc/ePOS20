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
                Txt_Tap3_Articulo_Codigo.Text = "Código"; // Restaurar el texto sugerido
                Txt_Tap3_Articulo_Codigo.ForeColor = Color.DarkGray; // Cambiar el color del texto a gris
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
                    this.Txt_Tap3_Articulo_Codigo.Text = "";
                    Txt_Tap3_Articulo_Descripcion.Text = "";
                    Txt_Tap3_Articulo_Cantidad.Text = "";
                    Txt_Tap3_Articulo_Precio.Text = "";
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
                    //Cbx_Pnl3_MotivoDesc.SelectedIndex = 0;
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
                    this.Dgv_Tap3_Medidas_Montura.Enabled = false;
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

                    // Panel de Arriba
                    this.Txt_Pnl2_Cedula.Enabled = false;
                    this.Txt_Pnl2_Examen.Enabled = false;
                    this.Cbx_Pnl2_Trbajo.Enabled = false;
                    this.Cbx_Pnl2_Laboratorio.Enabled = false;
                    this.Cbx_Pnl2_Servicio.Enabled = false;

                    break;

                case "CabezeraPrincipal":
                    // Panel descuento 
                    this.Pnl_3_Descuento.Enabled = false;

                    //Panel Cambio Precio
                    this.Pnl_3_CambioPrecio.Enabled = false;
                    this.Pnl_1_Tap3.Enabled = true;
                    this.Dgv_Tap3_Articulo.Enabled = true;
                    this.Pnl_2_Tap3.Enabled = true;
                    this.Pnl_3_Tap3.Enabled = true;
                    this.Dgv_Tap3_Medidas_Montura.Enabled = true;
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
                    this.Cbx_Pnl2_Trbajo.Enabled = true;
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
                    this.Dgv_Tap3_Medidas_Montura.Enabled = false;
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
                    this.Dgv_Tap3_Medidas_Montura.Enabled = false;
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
                // Obtener el artículo seleccionado
                var articulo = Dgv_Pnl3_Articulo.Rows[e.RowIndex].DataBoundItem as TB_ARTICULO;

                if (articulo != null)
                {
                    // Cargar los valores en los TextBox
                    Txt_Tap3_Articulo_Codigo.Text = articulo.CodArticulo;
                    Txt_Tap3_Articulo_Descripcion.Text = articulo.DESART;
                    Txt_Tap3_Articulo_Precio.Text = articulo.ART_PVP.ToString("F2"); // Formato de 2 decimales
                    Txt_Tap3_Articulo_Cantidad.Text = string.Empty; // Limpiar el campo de cantidad

                    VisualizarPanel("MostrarCabezeraSecundaria");
                    HabilitacionControl("CabezeraPrincipal");

                    // Establecer el foco en el TextBox de cantidad
                    Txt_Tap3_Articulo_Cantidad.Focus();
                }
            }
        }

        private void CargarArticulos_Girdvew()
        {
            // Validar que los campos no estén vacíos
            if (_L_Articulo.CargarArticulo_ValidarTexbox(Txt_Tap3_Articulo_Codigo, Txt_Tap3_Articulo_Descripcion, Txt_Tap3_Articulo_Precio, Txt_Tap3_Articulo_Cantidad, Txt_Pnl2_Examen))
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Por favor, complete todos los campos antes de agregar el artículo.");
                _FrmMensajes.ShowDialog();
                return;
            }

            // Validar Precio
            if (_L_Articulo.CargarArticulo_ValidarPrecio(Txt_Tap3_Articulo_Precio))
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("El precio del articulo debe ser mayor que 0.");
                _FrmMensajes.ShowDialog();
                return;
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
            if (VerificoProductos== false && _L_Articulo.stringBuilder.Length > 0)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(_L_Articulo.stringBuilder.ToString());
                _FrmMensajes.ShowDialog();
                return;
            }


            // Validar la existencia del producto
            string mensaje = _L_Articulo.ValidarExistenciaProducto(Txt_Tap3_Articulo_Codigo.Text, Convert.ToInt16(Txt_Tap3_Articulo_Cantidad.Text), listaArticulos);

            if (!string.IsNullOrEmpty(mensaje)) // Si hay un mensaje de error
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(mensaje);
                _FrmMensajes.ShowDialog();
                return; // Salir 
            }

            //Validar Cantidad Maxima Permitida Para venta
            mensaje = _L_Articulo.ValidarCantidadMaximaPermitida(Txt_Tap3_Articulo_Codigo.Text, Convert.ToInt16(Txt_Tap3_Articulo_Cantidad.Text));

            if (!string.IsNullOrEmpty(mensaje)) // Si hay un mensaje de error
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(mensaje);
                _FrmMensajes.ShowDialog();
                return; // Salir 
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


            if (_L_Articulo.VerificoCantidadCristales(Txt_Tap3_Articulo_Codigo.Text, Convert.ToInt16(Txt_Tap3_Articulo_Cantidad.Text), _TRABAJO))
            {
                // Definir Accion
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
                    _FrmMensajes.avisomensaje("El artículo no existe en la lista.");
                    _FrmMensajes.ShowDialog();
                    return;
                }

                // Validar que los campos de precio y cantidad sean válidos
                if (!decimal.TryParse(Txt_Tap3_Articulo_Precio.Text, out decimal precio))
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("El precio ingresado no es válido.");
                    _FrmMensajes.ShowDialog();
                    return;
                }

                if (!int.TryParse(Txt_Tap3_Articulo_Cantidad.Text, out int cantidad))
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("La cantidad ingresada no es válida.");
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


                _L_Articulo.AgregarFila(Dgv_Tap3_Articulo, articulo.CodArticulo, articulo.DESART, cantidad, (decimal) precio, (decimal)articulo.PORCTDESCUENTO, (decimal) total, impuesto, _Trabajo.T_OJO);


                // Limpiar los TextBox después de agregar el artículo
                Txt_Tap3_Articulo_Codigo.Clear();
                Txt_Tap3_Articulo_Precio.Clear();
                Txt_Tap3_Articulo_Cantidad.Clear();

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

            if (e.ColumnIndex >= 0 && Dgv_Tap3_Articulo.Columns[e.ColumnIndex].Name == "Eliminar")
            {
                // Verificar si se puede borrar el artículo
                bool puedeBorrar = _L_Articulo.VerificarYBorrarArticulo(Dgv_Tap3_Articulo, e.RowIndex);

                if (puedeBorrar)
                {
                    // Si se puede borrar, eliminar la fila
                    Dgv_Tap3_Articulo.Rows.RemoveAt(e.RowIndex);
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
            }

            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }

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
                // Acción para Enter
                _L_Articulo.CargarArticulos(Dgv_Pnl3_Articulo, listaArticulos, Cbx_Pnl2_Trbajo.SelectedValue.ToString());

                //Formatear los caracteres a 7 Digitos cuando es un cristal 
                _L_Articulo.FormatearCampo7Digitos(Txt_Tap3_Articulo_Codigo);

                // Buscar el articulo 
                _L_Articulo.FiltrarArticulos_Tap3(Txt_Tap3_Articulo_Codigo.Text, listaArticulos, listaTemporal, Txt_Tap3_Articulo_Codigo, Txt_Tap3_Articulo_Descripcion, Txt_Tap3_Articulo_Precio, Txt_Tap3_Articulo_Cantidad);

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

                    if (Convert.ToInt32(Dgv_Tap3_Articulo.Rows[numFilas].Cells["ART_EXIST"].Value) == 2)
                    {
                        //Verifico Diotria
                        _L_Articulo.EvaluoServicioAgregado(Dgv_Tap3_Articulo, numFilas, "D", Convert.ToInt32(Txt_Pnl2_Examen.Text), Cbx_Pnl2_Trbajo.SelectedValue.ToString(), Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2));
                        _L_Articulo.EvaluoServicioAgregado(Dgv_Tap3_Articulo,numFilas, "I", Convert.ToInt32(Txt_Pnl2_Examen.Text), Cbx_Pnl2_Trbajo.SelectedValue.ToString(), Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2));

                    }
                    else
                    {
                        //Verifico Diotria
                        _L_Articulo.EvaluoServicioAgregado(Dgv_Tap3_Articulo, numFilas, Dgv_Tap3_Articulo.Rows[numFilas].Cells["Ojo"].Value.ToString(), Convert.ToInt32(Txt_Pnl2_Examen.Text), Cbx_Pnl2_Trbajo.SelectedValue.ToString(), Txt_Pnl2_Cedula.Text.Substring(0, 1), Txt_Pnl2_Cedula.Text.Substring(2, Txt_Pnl2_Cedula.Text.Length - 2));
                    }

                }
            }

            // Totalizo el grivew Totales cuando se agrega una fila 
            _L_Articulo.ActualizarTotales(Dgv_Tap3_Articulo, Dgv_Tap3_Totales);

            // Habilito o desabilito Botones 
            ValidarRegistrosYHabilitar_Botones();

        }

        private void Dgv_Tap3_Articulo_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {

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
                Dgv_Tap3_Totales.Columns["Valor"].HeaderText =    "       ";


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
                    VisualizarPanel("Descuento");
                    HabilitacionControl("Habilitar_Descuento");
                    _L_Articulo.Cargo_CodMotivo_Descuento(Cbx_Pnl3_MotivoDesc);
                    Tipo_Descuento = "Descuento por articulo";
                }
            }
        }

        private void Btn_Tap3_Aceptar_CambioPrecio_Click(object sender, EventArgs e)
        {
            Decimal DesMaximo = 0;
            if(_L_Articulo.CambioPrecio(Txt_Pnl3_CambioPrecioNuevo,Txt_Pnl3_CambioPrecioActual, DesMaximo))
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

        private void CerrarPanelCambioPrecio ()
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

        private void Btn_Tap3_Cancelar_CambioPrecio_Click(object sender, EventArgs e)
        {
            CerrarPanelCambioPrecio();
        }

        private void Btn_Tap3_Cancelar_Desc_Click(object sender, EventArgs e)
        {
            CerrarPanelDescuento();
        }

        private void Btn_Tap3_Descuento_Click(object sender, EventArgs e)
        {
            VisualizarPanel("Descuento");
            HabilitacionControl("Habilitar_Descuento");
            _L_Articulo.Cargo_CodMotivo_Descuento(Cbx_Pnl3_MotivoDesc);
            Tipo_Descuento = "Descuento Global";
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

                             if (_L_Articulo.VerificarTopeMaximoDesceunto(Convert.ToInt32( Txt_Pnl3_PorcDescuento.Text)))
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
                        else if (Tipo_Descuento=="Cambio de Descuento")
                        {
                            // Verifico se el Porcentaje de descuento esta por encima dle permitido para generar una clave autorizada diferente 

                            if (_L_Articulo.VerificarTopeMaximoDesceunto(Convert.ToInt32(Txt_Pnl3_PorcDescuento.Text)))
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
            if(!_L_Articulo.CalculoDescuento(Dgv_Tap3_Totales.Rows[4].Cells["Valor"].Value.ToString(), Dgv_Tap3_Articulo, Tipo_Descuento, "0.00", Txt_Pnl3_PorcDescuento, Txt_Pnl3_MontoDesc, Txt_Pnl3_ObservacionDesc))
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
            if (Dgv_Tap3_Articulo.Rows.Count > 0)
            {
                Btn_Tap3_Descuento.Enabled = true; // Habilitar el TextBox o botón
            }
            else
            {
                Btn_Tap3_Descuento.Enabled = false; // Deshabilitar el TextBox o botón
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

                if(_servicioValidaciones.VerificoIgualAntirefCrist(Dgv_Tap3_Articulo, dsAR))
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














    }


 }

