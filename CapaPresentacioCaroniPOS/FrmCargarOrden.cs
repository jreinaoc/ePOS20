using CapaEntidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        private L_Articulo _L_Articulo = new L_Articulo();
        private FrmMensajes _FrmMensajes = new FrmMensajes();
        private D_Inicio _D_Inicio = new D_Inicio();
        public bool HabEliminar = false;

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
            HabilitacionControl("Bloquear_Lista_Articulo");
            LimpiarControles("Motro_Busqueda_Articulos");
            LimpiarControles("Carga_Articulos");
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

                case "Carga_Articulos":
                    this.Txt_Tap3_Articulo_Codigo.Text = "";
                    Txt_Tap3_Articulo_Descripcion.Text = "";
                    Txt_Tap3_Articulo_Cantidad.Text = "";
                    Txt_Tap3_Articulo_Precio.Text = "";
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

                    break;

                case "MostrarCabezeraSecundaria":
                    this.Pnl_2.Enabled = true;
                    this.Pnl_2.Visible = true;
                    this.Pnl_1.Visible = false;
                    this.Pnl_1.Enabled = false;
                    this.Pnl_3_Lista_Articulo.Enabled = false;
                    this.Pnl_3_Lista_Articulo.Visible = false;
                    this.Pnl_2.Location = new Point(0, 0); // Establecer posición en (0, 0)

                    break;

                case "Lista_Articulo":
                    this.Pnl_3_Lista_Articulo.Enabled = true;
                    this.Pnl_3_Lista_Articulo.Visible = true;
                    this.Pnl_3_Lista_Articulo.Location = new Point(250, 1);
                    this.Pnl_3_Lista_Articulo.BringToFront();
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
                    this.Pnl_1_Tap3.Enabled = false;
                    this.Dgv_Tap3_Articulo.Enabled = false;
                    this.Pnl_2_Tap3.Enabled = false;
                    this.Pnl_3_Tap3.Enabled = false;
                    this.Dgv_Tap3_Medidas_Montura.Enabled = false;
                    this.Btn_Tap3_Cancelar.Enabled = false;
                    this.Btn_Tap3_Procesar.Enabled = false;

                    this.Dgv_Pnl3_Articulo.Enabled = true;
                    this.Txt_Pnl3_Articulo.Enabled = true;
                    this.Rd_Pnl3_Descripcion.Enabled = true;
                    this.Rd_Pnl3_Codigo.Enabled = true;
                    this.Rd_Pnl3_Codigo.Checked= true;
                    this.Rd_Pnl3_Descripcion.Checked = false;
                    this.btnCancelar3.Enabled = true;

                    break;

                case "Bloquear_Lista_Articulo":
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
                Dgv_Pnl3_Articulo.Columns["DESART"].Width = 320;
                Dgv_Pnl3_Articulo.Columns["ART_PVP"].Width = 100;
                Dgv_Pnl3_Articulo.Columns["ART_EXIST"].Width = 40;
                Dgv_Pnl3_Articulo.Columns["MARCA"].Width = 60;

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

                    // Establecer el foco en el TextBox de cantidad
                    Txt_Tap3_Articulo_Cantidad.Focus();
                }
            }
        }

        private void CargarArticulos_Girdvew()
        {
            // Validar que los campos no estén vacíos
            if (_L_Articulo.CargarArticulo_ValidarTexbox(Txt_Tap3_Articulo_Codigo, Txt_Tap3_Articulo_Descripcion, Txt_Tap3_Articulo_Precio, Txt_Tap3_Articulo_Cantidad))
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Por favor, complete todos los campos antes de agregar el artículo.");
                _FrmMensajes.ShowDialog();
                return;
            }

            //Formatear los caracteres a 7 Digitos cuando es un cristal 
            _L_Articulo.FormatearCampo7Digitos(Txt_Tap3_Articulo_Codigo);

            // Verifica si el articulo ya fue Agregado
            if (_L_Articulo.CargarArticulo_EvitarDuplicado(Dgv_Tap3_Articulo, Txt_Tap3_Articulo_Codigo))
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("El artículo ya fue agregado al listado");
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
                CargarArticulos_Girdvew();
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
                    MessageBox.Show("El artículo no existe en la lista.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validar que los campos de precio y cantidad sean válidos
                if (!decimal.TryParse(Txt_Tap3_Articulo_Precio.Text, out decimal precio))
                {
                    MessageBox.Show("El precio ingresado no es válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(Txt_Tap3_Articulo_Cantidad.Text, out int cantidad))
                {
                    MessageBox.Show("La cantidad ingresada no es válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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




                // Agregar el artículo al DataGridView
                Dgv_Tap3_Articulo.Rows.Add(
                    articulo.CodArticulo,
                    articulo.DESART,
                    cantidad, // Cantidad desde el TextBox
                    precio,   // Precio desde el TextBox
                    articulo.PORCTDESCUENTO,
                    total,    // Total calculado
                    impuesto,
                    _Trabajo.T_OJO
                );

                // Limpiar los TextBox después de agregar el artículo
                Txt_Tap3_Articulo_Codigo.Clear();
                Txt_Tap3_Articulo_Precio.Clear();
                Txt_Tap3_Articulo_Cantidad.Clear();

                // Establecer el foco en el campo de código
                Txt_Tap3_Articulo_Codigo.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar el artículo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Formato_Dgv_Carga_Articulo()
        {
            try
            {

                //Centrar todas las colucnas 
                Dgv_Tap3_Articulo.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Dgv_Tap3_Articulo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

                // Quitar la flecha del selector de fila
                Dgv_Tap3_Articulo.RowHeadersVisible = false;

                // Deshabilitar el redimensionamiento de filas
                Dgv_Tap3_Articulo.AllowUserToResizeRows = false;

                //asignar Nombres a cada colucna 
                Dgv_Tap3_Articulo.Columns["CodArticulo"].HeaderText = "Código";
                Dgv_Tap3_Articulo.Columns["DESART"].HeaderText = "Descripción";
                Dgv_Tap3_Articulo.Columns["ART_EXIST"].HeaderText = "Cantidad";
                Dgv_Tap3_Articulo.Columns["ART_PVP"].HeaderText = "Precio";
                Dgv_Tap3_Articulo.Columns["PORCTDESCUENTO"].HeaderText = "%Descuento";
                Dgv_Tap3_Articulo.Columns["Total"].HeaderText = "Total";
                Dgv_Tap3_Articulo.Columns["Impuesto"].HeaderText = "%Impuesto";
                Dgv_Tap3_Articulo.Columns["Ojo"].HeaderText = "Ojo";
                //Dgv_Tap3_Articulo.Columns["Eliminar"].HeaderText = "";


                //Ancho de columna
                Dgv_Tap3_Articulo.Columns["CodArticulo"].Width = 80;
                Dgv_Tap3_Articulo.Columns["DESART"].Width = 320;
                Dgv_Tap3_Articulo.Columns["ART_EXIST"].Width = 60;
                Dgv_Tap3_Articulo.Columns["ART_PVP"].Width = 100;
                Dgv_Tap3_Articulo.Columns["PORCTDESCUENTO"].Width = 100;
                Dgv_Tap3_Articulo.Columns["Total"].Width = 100;
                Dgv_Tap3_Articulo.Columns["Impuesto"].Width = 100;
                Dgv_Tap3_Articulo.Columns["Ojo"].Width = 60;
                Dgv_Tap3_Articulo.Columns["Eliminar"].Width = 125;

                // No modificable
                Dgv_Tap3_Articulo.Columns["CodArticulo"].ReadOnly = true;
                Dgv_Tap3_Articulo.Columns["DESART"].ReadOnly = true;
                Dgv_Tap3_Articulo.Columns["ART_EXIST"].ReadOnly = true;
                Dgv_Tap3_Articulo.Columns["ART_PVP"].ReadOnly = true;
                Dgv_Tap3_Articulo.Columns["PORCTDESCUENTO"].ReadOnly = true;
                Dgv_Tap3_Articulo.Columns["Total"].ReadOnly = true;
                Dgv_Tap3_Articulo.Columns["Impuesto"].ReadOnly = true;
                Dgv_Tap3_Articulo.Columns["Ojo"].ReadOnly = true;
                Dgv_Tap3_Articulo.Columns["Eliminar"].ReadOnly = true;


                Dgv_Tap3_Articulo.Columns["CodArticulo"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["DESART"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["ART_EXIST"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["ART_PVP"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["PORCTDESCUENTO"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["Total"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["Impuesto"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["Ojo"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["Eliminar"].SortMode = DataGridViewColumnSortMode.NotSortable; ;

                /// Se utiliza un bucle foreach para recorrer todas las columnas del DataGridView. 
                /// Si el nombre de la columna no coincide con las columnas que deseas mostrar, 
                /// se oculta configurando su propiedad Visible como false:

                foreach (DataGridViewColumn column in Dgv_Tap3_Articulo.Columns)
                {
                    if (column.Name != "CodArticulo" &&
                        column.Name != "DESART" &&
                        column.Name != "ART_EXIST" &&
                        column.Name != "ART_PVP" &&
                        column.Name != "PORCTDESCUENTO" &&
                        column.Name != "Total" &&
                        column.Name != "Impuesto" &&
                        column.Name != "Ojo" &&
                        column.Name != "Eliminar")
                    {
                        column.Visible = false;
                    }
                }



                //quitar seleccion por defecto de datagrid
                Dgv_Tap3_Articulo.ClearSelection();

                //AutoGenerar Columnas:
                Dgv_Tap3_Articulo.AutoGenerateColumns = false;


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

            if (Dgv_Tap3_Articulo.Columns[e.ColumnIndex].Name == "Eliminar")
            {
                 LimpiarGrid();

            }

        }

        public void LimpiarGrid()
        {
            try
            {
                //limpiar el grid 
                Dgv_Tap3_Articulo.DataSource = "";
                Dgv_Tap3_Articulo.DataMember = "";

                var dataGridViewColumn2 = Dgv_Tap3_Articulo.Columns["Eliminar"];

                if (dataGridViewColumn2 != null)
                {
                    Dgv_Tap3_Articulo.Columns.RemoveAt(Dgv_Tap3_Articulo.Columns.Count - 1);
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
                        IconAtomico = new Icon(Environment.CurrentDirectory + @"\\cuadraditoOscuro2.ico");
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
            }
        }

        private void Txt_Tap3_Articulo_Codigo_KeyDown(object sender, KeyEventArgs e)
        {
            // Verificar si se presionó la tecla F2
            if (e.KeyCode == Keys.F2)
            {
                VisualizarPanel("Lista_Articulo");
                HabilitacionControl("Habilitar_Lista_Articulo");
                _L_Articulo.CargarArticulos(Dgv_Pnl3_Articulo, listaArticulos);
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

                _L_Articulo.stringBuilder.Clear();

                // Evitar que el evento se propague
                e.Handled = true;
            }

            else if (e.KeyCode == Keys.Enter)
            {
                // Acción para Enter
                _L_Articulo.CargarArticulos(Dgv_Pnl3_Articulo, listaArticulos);

                //Formatear los caracteres a 7 Digitos cuando es un cristal 
                _L_Articulo.FormatearCampo7Digitos(Txt_Tap3_Articulo_Codigo);

                // Buscar el articulo 
                _L_Articulo.FiltrarArticulos_Tap3(Txt_Tap3_Articulo_Codigo.Text ,listaArticulos, listaTemporal, Txt_Tap3_Articulo_Codigo, Txt_Tap3_Articulo_Descripcion, Txt_Tap3_Articulo_Cantidad , Txt_Tap3_Articulo_Precio);

                // Evitar que el evento se propague
                e.Handled = true;
            }
        }
    }
}
