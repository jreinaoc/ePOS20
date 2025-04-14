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
        private L_Articulo _L_Articulo = new L_Articulo();
        private FrmMensajes _FrmMensajes = new FrmMensajes();
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
            LimpiarControles("Carga_Articulos");
        }

        private void LimpiarControles(string Case)
        {
            switch (Case)
            {
                case "Carga_Articulos":
                    this.Txt_Pnl3_Articulo.Text = "";
                    listaArticulos.Clear();
                    listaTemporal.Clear();
                    Dgv_Pnl3_Articulo.DataSource = null;
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
                VisualizarPanel("Lista_Articulo");
                HabilitacionControl("Habilitar_Lista_Articulo");
                _L_Articulo.CargarArticulos(Dgv_Pnl3_Articulo, listaArticulos);
                if (_L_Articulo.stringBuilder.Length> 0)
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje(_L_Articulo.stringBuilder.ToString());
                    _FrmMensajes.ShowDialog();
                }
                else
                {

                    listaTemporal = new List<TB_ARTICULO>(listaArticulos);
                    Dgv_Pnl3_Articulo.DataSource = listaTemporal;
                    Formato_Dgv_Articulo();
                }

                _L_Articulo.stringBuilder.Clear();
            }
        }

        private void Txt_Pnl3_Articulo_TextChanged(object sender, EventArgs e)
        {
            // Filtrar los datos según el texto ingresado en el TextBox
            _L_Articulo.FiltrarArticulos(Txt_Pnl3_Articulo.Text.ToLower(), Rd_Pnl3_Descripcion, Rd_Pnl3_Codigo, Dgv_Pnl3_Articulo, listaArticulos, listaTemporal);          

        }

        //private void FiltrarArticulos(string filtro)
        //{

        //    // Verificar si el filtro está vacío
        //    if (string.IsNullOrWhiteSpace(filtro))
        //    {
        //        // Restablecer la información original en el DataGridView
        //        listaTemporal = new List<TB_ARTICULO>(listaArticulos); // Restaurar desde la lista original
        //        Dgv_Pnl3_Articulo.DataSource = listaTemporal;
        //        return;
        //    }

        //    // Convertir el filtro a minúsculas para una búsqueda insensible a mayúsculas
        //    filtro = filtro.ToLower();

        //    // Crear una lista para almacenar los resultados filtrados
        //    var datosFiltrados = new List<TB_ARTICULO>();

        //    // Recorrer la lista original (listaArticulos) para aplicar el filtro
        //    foreach (var articulo in listaArticulos)
        //    {
        //        // Filtrar según la opción seleccionada
        //        if (Rd_Pnl3_Descripcion.Checked && articulo.DESART != null && articulo.DESART.ToLower().Contains(filtro))
        //        {
        //            datosFiltrados.Add(articulo);
        //        }
        //        else if (Rd_Pnl3_Codigo.Checked && articulo.CodArticulo != null && articulo.CodArticulo.ToLower().Contains(filtro))
        //        {
        //            datosFiltrados.Add(articulo);
        //        }
        //    }

        //    // Actualizar la lista temporal con los datos filtrados
        //    listaTemporal = datosFiltrados;

        //    // Actualizar la fuente de datos del DataGridView con los resultados filtrados
        //    Dgv_Pnl3_Articulo.DataSource = datosFiltrados;
        //}

        private void Pnl_3_Lista_Articulo_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ObtenerArticulos()
        {

        }

        private void Formato_Dgv_Articulo()
        {
            try
            {

                //Centrar todas las colucnas 
                Dgv_Pnl3_Articulo.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Dgv_Pnl3_Articulo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

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
    }
}
