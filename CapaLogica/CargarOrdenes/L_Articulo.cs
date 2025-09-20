using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades;
using CapaDatos.Anulacion;
using CapaDatos.CargarOrdenes_Datos;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using CapaDatos.Conexion;
using CapaDatos.DetalleOrden_Datos;
using CapaDatos.Inicio_Datos;
using System.Text.RegularExpressions;
using CapaDatos.Login_Datos; // Necesario para usar Regex
using System.Globalization;

namespace CapaLogica.CargarOrdenes
{
    public class L_Articulo
    {

        private D_Anulacion _D_Anulacion;
        private D_Articulos _D_Articulos;
        private D_DetalleOrden _D_DetalleOrden;
        private D_Inicio _D_Inicio;

        public L_Articulo()
        {
            _D_Anulacion = new D_Anulacion();
            _D_Articulos = new D_Articulos();
            _D_DetalleOrden = new D_DetalleOrden();
            _D_Inicio = new D_Inicio();
        }

        //D_Anulacion _D_Anulacion = new D_Anulacion();
        //D_Articulos _D_Articulos = new D_Articulos();
        //D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        //D_Inicio _D_Inicio = new D_Inicio();
        public Boolean Diopprima = false;
        private System.Reflection.Assembly oEnsamblado;
        //El uso de la clase StringBuilder nos ayudara a devolver los mensajes 
        public readonly StringBuilder stringBuilder = new StringBuilder();
        private List<string> ArtPromocion = new List<string>();

        public void BucarTipoVenta(System.Windows.Forms.ComboBox comboBox)
        {
            DataTable dt = _D_Articulos.BucarTipoVenta();
            if (dt.Rows.Count > 0)
            {
                // Asignar el DataTable como fuente de datos del ComboBox
                comboBox.DataSource = dt;

                comboBox.DisplayMember = "Descripcion";
                comboBox.ValueMember = "CodModo";
            }
            else
            {
                // Si no hay datos, limpiar el ComboBox
                comboBox.DataSource = null;
                comboBox.Items.Clear();
            }

        }

        public void CargarArticulos(System.Windows.Forms.DataGridView DgvArticulo, List<TB_ARTICULO> listaArticulos, string TipoTrabajo, string CodArticulo = "")
        {
            stringBuilder.Clear();
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
                // Obtener los artículos desde la base de datos
                var articulosObtenidos = _D_Articulos.ObtenerArticulos(TipoTrabajo, CodArticulo, command);

                // Limpiar la lista pasada como parámetro y llenarla con los nuevos datos
                listaArticulos.Clear(); // Limpiar la lista para evitar duplicados
                listaArticulos.AddRange(articulosObtenidos); // Agregar los datos obtenidos

                // Asignar la lista como fuente de datos del DataGridView
                if (listaArticulos != null && listaArticulos.Count > 0 && _D_Articulos.stringBuilder.Length == 0)
                {
                    //DgvArticulo.DataSource = listaArticulos;

                    // Confirmar la transacción
                    transaction.Commit();
                }
                else
                {
                    //DgvArticulo.DataSource = null; // Si no hay datos, limpiar el DataGridView
                    stringBuilder.AppendLine("No se pudieron cargar los artículos correctamente");
                    transaction.Rollback();
                }


            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                transaction.Rollback();
            }

        }

        public string ValidarExtenciaCristal(DataGridView Dgv_Tap3_Articulo, string Cod_Vta)
        {
            try
            { 
              if(Dgv_Tap3_Articulo.Rows.Count> 0 && (Cod_Vta== "01"|| Cod_Vta == "09" || Cod_Vta == "08"))
              {
                foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
                {
                        if (row.Cells["CodArticulo"].Value != null && (row.Cells["CodArticulo"].Value.ToString().StartsWith("C")))
                        {
                            string CodArticulo = row.Cells["CodArticulo"].Value.ToString();
                            return CodArticulo;
                        }
                }
              }
                return "";
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción
                throw new Exception("Error al verificar y corregir los totales: " + ex.Message, ex);
            }
        }

        public void FiltrarArticulos(string filtro, System.Windows.Forms.RadioButton Rd_Pnl3_Descripcion, System.Windows.Forms.RadioButton Rd_Pnl3_Codigo, System.Windows.Forms.DataGridView Dgv_Pnl3_Articulo, List<TB_ARTICULO> listaArticulos, List<TB_ARTICULO> listaTemporal)
        {

            // Verificar si el filtro está vacío
            if (string.IsNullOrWhiteSpace(filtro))
            {
                // Restablecer la información original en el DataGridView
                listaTemporal = new List<TB_ARTICULO>(listaArticulos); // Restaurar desde la lista original
                Dgv_Pnl3_Articulo.DataSource = listaTemporal;
                return;
            }

            // Convertir el filtro a minúsculas para una búsqueda insensible a mayúsculas
            filtro = filtro.ToLower();

            // Crear una lista para almacenar los resultados filtrados
            var datosFiltrados = new List<TB_ARTICULO>();

            // Recorrer la lista original (listaArticulos) para aplicar el filtro
            foreach (var articulo in listaArticulos)
            {
                // Filtrar según la opción seleccionada
                if (Rd_Pnl3_Descripcion.Checked && articulo.DESART != null && articulo.DESART.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(articulo);
                }
                else if (Rd_Pnl3_Codigo.Checked && articulo.CodArticulo != null && articulo.CodArticulo.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(articulo);
                }
            }

            // Actualizar la lista temporal con los datos filtrados
            listaTemporal = datosFiltrados;

            // Actualizar la fuente de datos del DataGridView con los resultados filtrados
            Dgv_Pnl3_Articulo.DataSource = datosFiltrados;
        }

        public void FiltrarEmpresasAfiliadas(string filtro, System.Windows.Forms.RadioButton Rd_Pnl3_Descripcion, System.Windows.Forms.RadioButton Rd_Pnl3_Codigo, System.Windows.Forms.DataGridView Dgv_Pnl3_ClienteAfiliado, List<TB_EMPAFI> listaClienteAfiliado, List<TB_EMPAFI> listaTemporal)
        {

            // Verificar si el filtro está vacío
            if (string.IsNullOrWhiteSpace(filtro))
            {
                // Restablecer la información original en el DataGridView
                listaTemporal = new List<TB_EMPAFI>(listaClienteAfiliado); // Restaurar desde la lista original
                Dgv_Pnl3_ClienteAfiliado.DataSource = listaTemporal;
                return;
            }

            // Convertir el filtro a minúsculas para una búsqueda insensible a mayúsculas
            filtro = filtro.ToLower();

            // Crear una lista para almacenar los resultados filtrados
            var datosFiltrados = new List<TB_EMPAFI>();

            // Recorrer la lista original (listaArticulos) para aplicar el filtro
            foreach (var clienteAfiliado in listaClienteAfiliado)
            {
                // Filtrar según la opción seleccionada
                if (Rd_Pnl3_Descripcion.Checked && clienteAfiliado.Nombre != null && clienteAfiliado.Nombre.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(clienteAfiliado);
                }
                else if (Rd_Pnl3_Codigo.Checked && clienteAfiliado.Codigo_Emp != null && clienteAfiliado.Codigo_Emp.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(clienteAfiliado);
                }
            }

            // Actualizar la lista temporal con los datos filtrados
            listaTemporal = datosFiltrados;

            // Actualizar la fuente de datos del DataGridView con los resultados filtrados
            Dgv_Pnl3_ClienteAfiliado.DataSource = datosFiltrados;
        }
        public void FiltrarArticulos_Tap3(string filtro, List<TB_ARTICULO> listaArticulos, List<TB_ARTICULO> listaTemporal, System.Windows.Forms.TextBox Codigo, System.Windows.Forms.TextBox Descripcion, System.Windows.Forms.TextBox Precio, System.Windows.Forms.TextBox Cantidad, string Ojo)
        {
            // Verificar si el filtro está vacío
            if (string.IsNullOrWhiteSpace(filtro))
            {
                // Restablecer la información original en la lista temporal
                listaTemporal.Clear();
                listaTemporal.AddRange(listaArticulos); // Restaurar desde la lista original
                return;
            }

            // Convertir el filtro a minúsculas para una búsqueda insensible a mayúsculas
            filtro = filtro.ToLower();

            // Crear una lista para almacenar los resultados filtrados
            var datosFiltrados = new List<TB_ARTICULO>();

            // Recorrer la lista original (listaArticulos) para aplicar el filtro
            foreach (var articulo in listaArticulos)
            {
                // Filtrar EXACTAMENTE la opción seleccionada
                if (articulo.CodArticulo != null && articulo.CodArticulo.Equals(filtro, StringComparison.OrdinalIgnoreCase))
                {
                    datosFiltrados.Add(articulo);

                    // Actualizar los TextBox con los datos del artículo filtrado
                    Codigo.Text = articulo.CodArticulo;
                    Descripcion.Text = articulo.DESART;
                    Precio.Text = articulo.ART_PVP.ToString("F2"); // Formato de 2 decimales
                    Cantidad.Text = string.Empty; // Limpiar el campo de cantidad
                   
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

                    if (Codigo.Text.StartsWith("W") || Codigo.Text.StartsWith("C") || EsAR)
                    {
                        if (Ojo == "Ambos")
                        {
                            Cantidad.Text = "2";
                        }
                        else
                        {
                            Cantidad.Text = "1";
                        }
                    }
                    else
                    {
                        Cantidad.Text = "1";
                    }

                    // Establecer el foco en el TextBox de cantidad
                    Cantidad.Focus();

                    // Salir del bucle después de encontrar el artículo
                    break;
                }
            }



            // Actualizar la lista temporal con los datos filtrados
            listaTemporal.Clear();
            listaTemporal.AddRange(datosFiltrados);
        }

        public void AgregarFila(DataGridView Dgv_Tap3_Articulo, string codArticulo, string codLab, string generico, string colorLC, string descripcion, int cantidad, decimal precio, decimal descuento, decimal total, decimal impuesto, string ojo, decimal CostoProme, string artPadre = "", string agregado = "NO" ,string AgreDer ="NO", string AgreIzq = "NO", string tienePromo = "No", string codPromo = "", string promoEvaluada = "No")
        {
          try {
                // Verificar y agregar columnas si no existen
                if (Dgv_Tap3_Articulo.Columns.Count == 0)
                {
                Dgv_Tap3_Articulo.Columns.Add("CodArticulo", "Código del Artículo");
                    
                //LenteContacto
                Dgv_Tap3_Articulo.Columns.Add("codLab", "codLab");
                Dgv_Tap3_Articulo.Columns.Add("generico", "generico");
                Dgv_Tap3_Articulo.Columns.Add("ColorLC", "ColorLC");

                Dgv_Tap3_Articulo.Columns.Add("DESART", "Descripción");
                Dgv_Tap3_Articulo.Columns.Add("ART_EXIST", "Cantidad");
                Dgv_Tap3_Articulo.Columns.Add("ART_PVP", "Precio");
                Dgv_Tap3_Articulo.Columns.Add("PORCTDESCUENTO", "PORCTDESCUENTO");
                Dgv_Tap3_Articulo.Columns.Add("Total", "Total");
                Dgv_Tap3_Articulo.Columns.Add("Impuesto", "Impuesto");
                Dgv_Tap3_Articulo.Columns.Add("Ojo", "Ojo");
                Dgv_Tap3_Articulo.Columns.Add("CostoProme", "CostoProme");
                   
                    // columnas opcionales
                Dgv_Tap3_Articulo.Columns.Add("ArtPadre", "Artículo Padre");
                Dgv_Tap3_Articulo.Columns.Add("Agregado", "Agregado");
                Dgv_Tap3_Articulo.Columns.Add("AgreDer", "AgreDer");
                Dgv_Tap3_Articulo.Columns.Add("AgreIzq", "AgreIzq");
                Dgv_Tap3_Articulo.Columns.Add("PrecioViejo", "PrecioViejo");

                // columnas de promociones
                 Dgv_Tap3_Articulo.Columns.Add("TienePromo", "Tiene Promo");
                 Dgv_Tap3_Articulo.Columns.Add("CodPromo", "Código de Promoción");
                Dgv_Tap3_Articulo.Columns.Add("PromoEvaluada", "Promoción Evaluada");

                    CrearObjetos(Dgv_Tap3_Articulo);

                }
                // FormatoDataGrivew
                Formato_Dgv_Carga_Articulo(Dgv_Tap3_Articulo, colorLC);

                // Agregar la fila con los valores proporcionados
                Dgv_Tap3_Articulo.Rows.Add(codArticulo, codLab, generico, colorLC, descripcion, cantidad, precio, descuento, total, impuesto, ojo, CostoProme, artPadre, agregado, AgreDer, AgreIzq, precio, tienePromo, codPromo, promoEvaluada);

            }
            catch (Exception ex)
            {
                //// Lanzar una excepción personalizada para que sea manejada en la capa visual
                //throw new Exception("Error al agregar una fila al DataGridView. Detalles: " + ex.Message, ex);
            }

        }

        public void VerificarYCorregirTotales(DataGridView Dgv_Tap3_Articulo)
        {
            try
            {
                foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
                {
                    // Verificar que la fila no sea nueva
                    if (!row.IsNewRow)
                    {
                        // Obtener los valores de las columnas
                        int cantidad = Convert.ToInt32(row.Cells["ART_EXIST"].Value ?? 0);
                        decimal precio = Convert.ToDecimal(row.Cells["ART_PVP"].Value ?? 0);
                        decimal total = Convert.ToDecimal(row.Cells["Total"].Value ?? 0);

                        // Calcular el total esperado
                        decimal totalEsperado = cantidad * precio;

                        // Verificar si el total coincide
                        if (total != totalEsperado)
                        {
                            // Corregir el valor de la columna "Total"
                            row.Cells["Total"].Value = totalEsperado;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción
                throw new Exception("Error al verificar y corregir los totales: " + ex.Message, ex);
            }
        }

        public void ActualizarCelda(DataGridView Dgv_Tap3_Articulo, int numeroFila, string nombreColumna, string nuevoValor)
        {
            try
            {
                // Validar que el número de fila esté dentro del rango
                if (numeroFila < 0 || numeroFila >= Dgv_Tap3_Articulo.Rows.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(numeroFila), "El número de fila está fuera del rango válido");
                }

                // Validar que la columna exista
                if (!Dgv_Tap3_Articulo.Columns.Contains(nombreColumna))
                {
                    throw new ArgumentException($"La columna '{nombreColumna}' no existe en el DataGridView", nameof(nombreColumna));
                }

                // Actualizar el valor de la celda
                Dgv_Tap3_Articulo.Rows[numeroFila].Cells[nombreColumna].Value = nuevoValor;
                VerificarYCorregirTotales(Dgv_Tap3_Articulo);
            }
            catch (Exception ex)
            {
                // Lanzar una excepción personalizada para que sea manejada en la capa visual
                throw new Exception($"Error al actualizar la celda en la fila {numeroFila} y columna '{nombreColumna}'. Detalles: {ex.Message}", ex);
            }
        }

        public void ActualizarTodasCelda(DataGridView Dgv_Tap3_Articulo, string nombreColumna, string nuevoValor)
        {
            try
            {
                // Validar que la columna exista
                if (!Dgv_Tap3_Articulo.Columns.Contains(nombreColumna))
                {
                    throw new ArgumentException($"La columna '{nombreColumna}' no existe en el DataGridView", nameof(nombreColumna));
                }

                // Recorrer todas las filas del DataGridView
                foreach (DataGridViewRow fila in Dgv_Tap3_Articulo.Rows)
                {
                    // Verificar que la fila no sea nueva
                    if (!fila.IsNewRow)
                    {
                        // Actualizar el valor de la celda en la columna especificada
                        fila.Cells[nombreColumna].Value = nuevoValor;
                    }
                }

                // Llamar a una función adicional si es necesario (por ejemplo, recalcular totales)
                VerificarYCorregirTotales(Dgv_Tap3_Articulo);
            }
            catch (Exception ex)
            {
                // Lanzar una excepción personalizada para que sea manejada en la capa visual
                throw new Exception($"Error al actualizar todas las celdas de la columna '{nombreColumna}'. Detalles: {ex.Message}", ex);
            }
        }

        public void CrearObjetos(DataGridView Dgv_Tap3_Articulo)
        {

            DataGridViewButtonColumn BtnEliminar = new DataGridViewButtonColumn();
            BtnEliminar.Name = "Eliminar";
            BtnEliminar.Width = 120;
            BtnEliminar.HeaderText = "Eliminar";
            Dgv_Tap3_Articulo.Columns.Add(BtnEliminar);

        }

        private void Formato_Dgv_Carga_Articulo(DataGridView Dgv_Tap3_Articulo, string colorLC)
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
          

            // Conacto
            Dgv_Tap3_Articulo.Columns["ColorLC"].HeaderText = "Color";
            Dgv_Tap3_Articulo.Columns["codLab"].HeaderText = "Cod. Laboratorio";
            Dgv_Tap3_Articulo.Columns["generico"].HeaderText = "Generico";

                Dgv_Tap3_Articulo.Columns["DESART"].HeaderText = "Descripción";
                Dgv_Tap3_Articulo.Columns["ART_EXIST"].HeaderText = "Cantidad";
                Dgv_Tap3_Articulo.Columns["ART_PVP"].HeaderText = "Precio";
                Dgv_Tap3_Articulo.Columns["PORCTDESCUENTO"].HeaderText = "%Descuento";
                Dgv_Tap3_Articulo.Columns["Total"].HeaderText = "Total";
                Dgv_Tap3_Articulo.Columns["Impuesto"].HeaderText = "%Impuesto";
                Dgv_Tap3_Articulo.Columns["Ojo"].HeaderText = "Ojo";
                Dgv_Tap3_Articulo.Columns["CostoProme"].HeaderText = "CostoProme";
                Dgv_Tap3_Articulo.Columns["Eliminar"].HeaderText = "";

            //Ancho de columna
            bool disableColorLCandColorLC = false;
            bool EsLC = false; 
            foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
            {
                if (row.Cells["CodArticulo"].Value.ToString().StartsWith("W"))
                {
                    EsLC = true;
                    break;
                }
                else
                {
                    EsLC = false;
                }
            }

            if (EsLC || colorLC != "")
            {
                Dgv_Tap3_Articulo.Columns["CodArticulo"].Width = 70;
                Dgv_Tap3_Articulo.Columns["codLab"].Width = 70;
                Dgv_Tap3_Articulo.Columns["ColorLC"].Width = 35;
                Dgv_Tap3_Articulo.Columns["DESART"].Width = 258;
                Dgv_Tap3_Articulo.Columns["ART_EXIST"].Width = 60;
                Dgv_Tap3_Articulo.Columns["ART_PVP"].Width = 100;
                Dgv_Tap3_Articulo.Columns["PORCTDESCUENTO"].Width = 100;
                Dgv_Tap3_Articulo.Columns["Total"].Width = 120;
                Dgv_Tap3_Articulo.Columns["Impuesto"].Width = 100;
                Dgv_Tap3_Articulo.Columns["Ojo"].Width = 70;
                Dgv_Tap3_Articulo.Columns["CostoProme"].Width = 80;
                Dgv_Tap3_Articulo.Columns["Eliminar"].Width = 90;
                // columnas opcionales
                Dgv_Tap3_Articulo.Columns["ArtPadre"].Width = 80;
                Dgv_Tap3_Articulo.Columns["Agregado"].Width = 40;
                disableColorLCandColorLC = true;
            }
            else
            {
                Dgv_Tap3_Articulo.Columns["CodArticulo"].Width = 70;
                Dgv_Tap3_Articulo.Columns["DESART"].Width = 363;
                Dgv_Tap3_Articulo.Columns["ART_EXIST"].Width = 60;
                Dgv_Tap3_Articulo.Columns["ART_PVP"].Width = 100;
                Dgv_Tap3_Articulo.Columns["PORCTDESCUENTO"].Width = 100;
                Dgv_Tap3_Articulo.Columns["Total"].Width = 120;
                Dgv_Tap3_Articulo.Columns["Impuesto"].Width = 100;
                Dgv_Tap3_Articulo.Columns["Ojo"].Width = 70;
                Dgv_Tap3_Articulo.Columns["CostoProme"].Width = 80;
                Dgv_Tap3_Articulo.Columns["Eliminar"].Width = 90;
                // columnas opcionales
                Dgv_Tap3_Articulo.Columns["ArtPadre"].Width = 80;
                Dgv_Tap3_Articulo.Columns["Agregado"].Width = 40;
                

            }

                // No modificable
                Dgv_Tap3_Articulo.Columns["CodArticulo"].ReadOnly = true;
                Dgv_Tap3_Articulo.Columns["ColorLC"].ReadOnly = true;
                Dgv_Tap3_Articulo.Columns["codLab"].ReadOnly = true;
                Dgv_Tap3_Articulo.Columns["generico"].ReadOnly = true;
                Dgv_Tap3_Articulo.Columns["DESART"].ReadOnly = true;
                Dgv_Tap3_Articulo.Columns["ART_EXIST"].ReadOnly = true;
                Dgv_Tap3_Articulo.Columns["ART_PVP"].ReadOnly = true;
                Dgv_Tap3_Articulo.Columns["PORCTDESCUENTO"].ReadOnly = true;
                Dgv_Tap3_Articulo.Columns["Total"].ReadOnly = true;
                Dgv_Tap3_Articulo.Columns["Impuesto"].ReadOnly = true;
                Dgv_Tap3_Articulo.Columns["Ojo"].ReadOnly = true;
                Dgv_Tap3_Articulo.Columns["CostoProme"].ReadOnly = true;
                Dgv_Tap3_Articulo.Columns["Eliminar"].ReadOnly = true;
                // columnas opcionales
                Dgv_Tap3_Articulo.Columns["ArtPadre"].ReadOnly = true;
                Dgv_Tap3_Articulo.Columns["Agregado"].ReadOnly = true;

                Dgv_Tap3_Articulo.Columns["CodArticulo"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["ColorLC"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["codLab"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["generico"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["DESART"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["ART_EXIST"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["ART_PVP"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["PORCTDESCUENTO"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["Total"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["Impuesto"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["Ojo"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["CostoProme"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["Eliminar"].SortMode = DataGridViewColumnSortMode.NotSortable; 

            // columnas opcionales
            Dgv_Tap3_Articulo.Columns["ArtPadre"].SortMode = DataGridViewColumnSortMode.NotSortable;
            Dgv_Tap3_Articulo.Columns["Agregado"].SortMode = DataGridViewColumnSortMode.NotSortable;


            /// Se utiliza un bucle foreach para recorrer todas las columnas del DataGridView. 
            /// Si el nombre de la columna no coincide con las columnas que deseas mostrar, 
            /// se oculta configurando su propiedad Visible como false:

            foreach (DataGridViewColumn column in Dgv_Tap3_Articulo.Columns)
            {
                if (EsLC || colorLC != "")
                {
                    if (column.Name != "CodArticulo" && column.Name != "DESART" &&  column.Name != "ART_EXIST" &&  column.Name != "ART_PVP" &&  column.Name != "PORCTDESCUENTO" &&
                       column.Name != "Total" &&   column.Name != "Impuesto" &&   column.Name != "Ojo" &&     column.Name != "Eliminar" &&  column.Name != "ColorLC" &&
                       column.Name != "codLab")
                    {
                        column.Visible = false;
                    }
                    else if (column.Index == 1 && disableColorLCandColorLC)
                    {
                        column.Visible = true;
                    }
                    else if (column.Index == 3 && disableColorLCandColorLC)
                    {
                        column.Visible = true;
                        disableColorLCandColorLC = false;
                    }
                }
                else
                { 
                    if (column.Name != "CodArticulo" &&  column.Name != "DESART" && column.Name != "ART_EXIST" &&  column.Name != "ART_PVP" && column.Name != "PORCTDESCUENTO" &&
                        column.Name != "Total" &&  column.Name != "Impuesto" &&  column.Name != "Ojo" && column.Name != "Eliminar")
                    {
                        column.Visible = false;
                    }
                 }
            }

            Dgv_Tap3_Articulo.Columns["ART_PVP"].DefaultCellStyle.Format = "N2"; // Formato de 2 decimales y unidades de mil
            Dgv_Tap3_Articulo.Columns["Total"].DefaultCellStyle.Format = "N2"; // Formato de 2 decimales y unidades de mil
            Dgv_Tap3_Articulo.Columns["CostoProme"].DefaultCellStyle.Format = "N2"; // Formato de 2 decimales y unidades de mil

            //quitar seleccion por defecto de datagrid
            Dgv_Tap3_Articulo.ClearSelection();

            //AutoGenerar Columnas:
            Dgv_Tap3_Articulo.AutoGenerateColumns = false;



        }

        public bool CargarArticulo_ValidarTexbox(System.Windows.Forms.TextBox Codigo, System.Windows.Forms.TextBox Descripcion, System.Windows.Forms.TextBox Precio, System.Windows.Forms.TextBox Cantidad, System.Windows.Forms.TextBox NumeroExamen)
        {
            // Validar si el campo Código está vacío
            if (string.IsNullOrWhiteSpace(Codigo.Text))
            {
                Codigo.Focus(); // Establecer el foco en el campo Código
                return true;
            }

            // Validar si el campo Descripción está vacío
            if (string.IsNullOrWhiteSpace(Descripcion.Text))
            {
                Descripcion.Focus(); // Establecer el foco en el campo Descripción
                return true;
            }

            // Validar si el campo Precio está vacío
            if (string.IsNullOrWhiteSpace(Precio.Text))
            {
                Precio.Focus(); // Establecer el foco en el campo Precio
                return true;
            }

            // Validar si el campo Cantidad está vacío
            if (string.IsNullOrWhiteSpace(Cantidad.Text))
            {
                Cantidad.Focus(); // Establecer el foco en el campo Cantidad
                return true;
            }

            // Validar si el campo Cantidad está vacío
            if (string.IsNullOrWhiteSpace(NumeroExamen.Text))
            {
                NumeroExamen.Focus(); // Establecer el foco en el campo Cantidad
                return true;
            }


            // Si todos los campos tienen valores, devolver false
            return false;

        }

        public bool ExisteTasa()
        {
            stringBuilder.Clear();

            //Validar que existan datos en el entidad Tb Tasa
            if (TB_TASA_Dolar.Tasa == 0.00 | TB_TASA_Dolar.Tasa == null | TB_TASA_Euro.Tasa == null | TB_TASA_Euro.Tasa == 0.00)
            {
                stringBuilder.Append("Debe actualizar la tasa de las divisas y secuencia diaria");
                return false;
            }
            else
            {
                return true;
            }

        }

        public bool CargarArticulo_ValidarPrecio(System.Windows.Forms.TextBox Precio)
        {

            // Validar si el campo Precio es mayor que 0
            if (!decimal.TryParse(Precio.Text, out decimal precioDecimal) || precioDecimal <= 0)
            {
                Precio.Focus(); // Establecer el foco en el campo Precio
                return true;
            }

            // Si todos los campos tienen valores, devolver false
            return false;

        }

        public bool CargarArticulo_EvitarDuplicado(System.Windows.Forms.DataGridView Dgv_Tap3_Articulo, string Codigo)
        {
            int cantidadCristales = 0;
            foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
            {
                if (!row.Cells["CodArticulo"].Value.ToString().StartsWith("W"))
                {
                    if (row.Cells["CodArticulo"].Value?.ToString() == Codigo)
                    {
                        //MessageBox.Show("El artículo ya está agregado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return true;
                    }   
                }

                //Verificar si ya hay un cristal agregado
                if (row.Cells["CodArticulo"].Value.ToString().StartsWith("C") && Codigo.StartsWith("C"))
                {
                    cantidadCristales += Convert.ToInt32(row.Cells["ART_EXIST"].Value);
                }

                if (cantidadCristales >= 2)
                {
                    return true;
                }

            }

            return false;
        }

        public bool VerificarYBorrarArticulo(DataGridView gexFacturas, ref int filaActual)
        {
            try
            {
                // Verificar si el artículo no tiene un padre (no es agregado por promoción)
                if (gexFacturas.Rows[filaActual].Cells["ArtPadre"].Value == DBNull.Value ||
                    gexFacturas.Rows[filaActual].Cells["ArtPadre"].Value.ToString() == "")
                {
                    string codPadre = gexFacturas.Rows[filaActual].Cells["CodArticulo"].Value.ToString();
                    List<int> filasParaEliminar = new List<int>();

                    // Recopilar las filas que deben eliminarse
                    foreach (DataGridViewRow fila in 
                        gexFacturas.Rows)
                    {
                        if (fila.Cells["ArtPadre"].Value != DBNull.Value)
                        {
                            // Verificar si el artículo es hijo del que se está borrando
                            if (fila.Cells["ArtPadre"].Value.ToString() == codPadre)
                            {
                                filasParaEliminar.Add(fila.Index);
                            }
                        }
                        else
                        {
                            // Verificar si es un cristal
                            if (fila.Cells["CodArticulo"].Value.ToString().StartsWith("C"))
                            {
                                if (fila.Cells["Ojo"].Value != DBNull.Value)
                                {
                                    // Contar los cristales restantes
                                    int cantidadCristales = 0;
                                    foreach (DataGridViewRow row in gexFacturas.Rows)
                                    {
                                        if (row.Cells["CodArticulo"].Value.ToString().StartsWith("C"))
                                        {
                                            cantidadCristales++;
                                        }
                                    }

                                    if (cantidadCristales <= 1)
                                    {
                                        // Puedes agregar lógica adicional aquí si es necesario
                                    }
                                }
                            }
                        }
                    }

                    // Eliminar las filas recopiladas en orden descendente
                    foreach (int index in filasParaEliminar.OrderByDescending(i => i))
                    {
                        gexFacturas.Rows.RemoveAt(index);
                    }

                    // Recalcular el índice de la fila actual
                    filaActual = -1; // Inicializar como no encontrado
                    for (int i = 0; i < gexFacturas.Rows.Count; i++)
                    {
                        if (gexFacturas.Rows[i].Cells["CodArticulo"].Value != null &&
                            gexFacturas.Rows[i].Cells["CodArticulo"].Value.ToString() == codPadre)
                        {
                            filaActual = i;
                            break;
                        }
                    }

                    // Verificar si la fila actual aún existe antes de eliminarla
                    if (filaActual >= 0)
                    {
                        return true;
                        //gexFacturas.Rows.RemoveAt(filaActual);
                    }

                    return true; // Artículos borrados correctamente
                }
                else
                {
                    // Verificar si el artículo tiene un padre
                    var artPadre = gexFacturas.Rows[filaActual].Cells["ArtPadre"].Value;

                    if (artPadre == null)
                    {
                        return true; // Permitir borrar si no tiene padre
                    }
                    else if (artPadre.ToString() == "X")
                    {
                        return false; // No permitir borrar si es un servicio especial
                    }
                    else
                    {
                        return false; // No permitir borrar si es un artículo hijo
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar y borrar el artículo. Detalles: " + ex.Message, ex);
            }
        }


        public bool ValidoExistenciaArticulo(string codigoProducto, int cantidadIngresada, List<TB_ARTICULO> listaArticulos, string glbManejaExisLC, string glbCodDetVta)
        {
            stringBuilder.Clear();
            try
            {
                // Buscar el producto en la lista por su código
                var articulo = listaArticulos.FirstOrDefault(a => a.CodArticulo == codigoProducto);

                // Validar si el código del producto no comienza con "A", "C", "S" o "W"
                if (!(codigoProducto.StartsWith("A") || codigoProducto.StartsWith("C") || codigoProducto.StartsWith("S") || codigoProducto.StartsWith("W") || codigoProducto.StartsWith("E")))
                {
                    if (articulo == null)
                    {
                        stringBuilder.Append($"El artículo {codigoProducto} no existe en la lista.");
                        return false;
                    }

                    // Verificar si la cantidad ingresada excede la existencia
                    if (cantidadIngresada > articulo.ART_EXIST)
                    {
                        stringBuilder.Append($"La cantidad ingresada excede la existencia disponible");
                        return false;
                    }
                }

                // Verificar si el artículo maneja existencia
                if (articulo.MANEJAEXISTENCIA)
                    {
                        // Verificar si el artículo tiene existencia
                        if (articulo.ART_EXIST > 0)
                        {
                            // Verificar la cantidad máxima permitida para la venta
                            var dsCantidad = _D_Articulos.MaxVta_btnProcesar(codigoProducto.Substring(0, 1));

                            if (cantidadIngresada > Convert.ToInt32(dsCantidad.Rows[0]["Max_Vta"]))
                            {
                            // La cantidad a vender es mayor al máximo permitido
                            stringBuilder.Append($"La cantidad sobrepasa el límite de venta");
                            //stringBuilder.Append($"El artículo {codigoProducto} tiene una cantidad a vender mayor que el máximo permitido. Por favor, modifique la cantidad para continuar");                   
                            return false;
                            }
                        }
                        else
                        {
                            // Verificar si se permite manejar existencia en ciertas condiciones
                            if (glbManejaExisLC == "1" && glbCodDetVta == "02")
                            {
                                return true;
                            }
                            else
                            {
                                stringBuilder.Append($"Este artículo no tiene existencia");
                                return false;
                            }
                        }
                    }
                
                // Si todo es válido, retornar true
                return true;
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return false;
            }
        }

        public string ValidarCantidadMaximaPermitida(string codigoProducto, int cantidadIngresada)
        {
            DataTable DT_ValorMaximo = _D_Articulos.BucarArticuloMaximoPorVenta(codigoProducto.Substring(0, 1));

            foreach (DataRow row in DT_ValorMaximo.Rows)
            {
                int ValorMaximoPorArticulo = Convert.ToInt32(row["Max_Vta"].ToString());
                if (cantidadIngresada > ValorMaximoPorArticulo)
                {
                    //return "El articulo " + codigoProducto + " tiene una cantidad a vender mayor que el maximo permitido";
                    return "La cantidad sobrepasa el límite de venta";
                }
            }
            return "";
        }



        public void FormatearCampo7Digitos(System.Windows.Forms.TextBox CodigoArticulo)
        {
            // Validar que el TextBox no sea nulo y que tenga texto
            if (CodigoArticulo == null || string.IsNullOrWhiteSpace(CodigoArticulo.Text))
            {
                return; // Salir si el TextBox está vacío o es nulo
            }

            if (CodigoArticulo.Text.StartsWith("C", StringComparison.OrdinalIgnoreCase))
            {
                switch (CodigoArticulo.Text.Length)
                {
                    case 1:
                        CodigoArticulo.Text = CodigoArticulo.Text + "000000";
                        break;
                    case 2:
                        CodigoArticulo.Text = CodigoArticulo.Text.Substring(0, 1) + "00000" + CodigoArticulo.Text.Substring(1, 1);
                        break;
                    case 3:
                        CodigoArticulo.Text = CodigoArticulo.Text.Substring(0, 1) + "0000" + CodigoArticulo.Text.Substring(1, 2);
                        break;
                    case 4:
                        CodigoArticulo.Text = CodigoArticulo.Text.Substring(0, 1) + "000" + CodigoArticulo.Text.Substring(1, 3);
                        break;
                    case 5:
                        CodigoArticulo.Text = CodigoArticulo.Text.Substring(0, 1) + "00" + CodigoArticulo.Text.Substring(1, 4);
                        break;
                    case 6:
                        CodigoArticulo.Text = CodigoArticulo.Text.Substring(0, 1) + "0" + CodigoArticulo.Text.Substring(1, 5);
                        break;
                }
            }
        }

        public bool VerificoCantidadCristales(string codigoProducto, int cantidadIngresada, List<TB_TRABAJO> trabajos)
        {
            //// Validar que la lista no sea nula o vacía
            //if (trabajos == null || trabajos.Count == 0)
            //{
                
            //    throw new ArgumentException("La lista de trabajos no puede estar vacía");
            //}

            // Iterar sobre los trabajos para verificar las condiciones
            foreach (var trabajo in trabajos)
            {
                // Verificar si el trabajo es "CONVENCIONAL" y el código comienza con "C"
                if (trabajo.T_TIPOTRABAJO == "CONVENCIONAL" && codigoProducto.StartsWith("C", StringComparison.OrdinalIgnoreCase))
                {
                    int cristal = trabajo.T_OJO == "Ambos" ? 2 : 1;

                    // Validar la cantidad ingresada
                    if (cantidadIngresada > cristal)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                // Verificar si el trabajo es "CONTACTO" y el código comienza con "W"
                else if (trabajo.T_TIPOTRABAJO == "CONTACTO" && codigoProducto.StartsWith("W", StringComparison.OrdinalIgnoreCase))
                {
                    int cantc = trabajo.T_OJO == "Ambos" ? 2 : 1;

                    // Validar la cantidad ingresada
                    if (cantidadIngresada > cantc)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                }
            }

            // Si no se cumple ninguna condición, devolver false por defecto
            return false;
        }

        public void LlenarTB_Trbajo(List<TB_TRABAJO> _TRABAJO, string sucursal, string NacioNalidad, string Cedula)
        {
            try
            {

                // Obtener los artículos desde la base de datos
                var TRABAJOS = _D_Articulos.ObtenerTrabajo(sucursal, NacioNalidad, Cedula);

                // Limpiar la lista pasada como parámetro y llenarla con los nuevos datos
                _TRABAJO.Clear(); // Limpiar la lista para evitar duplicados
                _TRABAJO.AddRange(TRABAJOS); // Agregar los datos obtenidos
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
            }
        }

        public int BuscarIva(string Codigo_Iva)
        {
            DataTable DT_Iva = _D_Articulos.BucarIva(Codigo_Iva);

            foreach (DataRow row in DT_Iva.Rows)
            {
                int Iva = Convert.ToInt32(row["Porcentaje"].ToString());
                return Iva;
            }

            return 0;
        }


        // Método para inicializar las filas del DataGridView de totales
        public void InicializarDataGridViewTotales(DataGridView Dgv_Totales)
        {
            // Verificar si ya existen filas en el DataGridView
            if (Dgv_Totales.Rows.Count > 0)
            {
                // Si ya tiene filas, no hacer nada
                return;
            }


            // Crear una lista con los conceptos predefinidos
            List<Total_Orden> detalles = new List<Total_Orden>
            {
        new Total_Orden { Concepto = "SubTotal", Valor = 0 },
        new Total_Orden { Concepto = "Descuento", Valor = 0 },
        new Total_Orden { Concepto = "IVA", Valor = 0 },
        new Total_Orden { Concepto = "IGTF", Valor = 0 },
        new Total_Orden { Concepto = "Total", Valor = 0 },
        new Total_Orden { Concepto = "Ref", Valor = 0 }
            };

            // Enlazar la lista al DataGridView de totales
            Dgv_Totales.DataSource = detalles;

            // Personalizar los encabezados de las columnas
            if (Dgv_Totales.Columns.Count > 0)
            {
                Dgv_Totales.Columns["Concepto"].HeaderText = "";
                Dgv_Totales.Columns["Valor"].HeaderText = "";
                Dgv_Totales.Columns["Valor"].DefaultCellStyle.Format = "N2"; // Formato de 2 decimales
            }

            // Deshabilitar la edición de las celdas
            Dgv_Totales.Columns["Concepto"].ReadOnly = true;
            Dgv_Totales.Columns["Valor"].ReadOnly = true;
        }


        // Método para calcular y actualizar los valores en el DataGridView de totales
        public void ActualizarTotales(DataGridView Dgv_Tap3_Articulo, DataGridView Dgv_Totales)
        {
            // Variables para almacenar los cálculos
            decimal subtotal = 0;
            decimal descuentoTotal = 0;
            decimal ivaTotal = 0;
            decimal igtfTotal = 0;
            decimal totalGeneral = 0;
            decimal Tasa = Convert.ToDecimal(TB_TASA_Dolar.Tasa);
            decimal subtotalConIva = 0;

            // Iterar sobre las filas del DataGridView base para calcular los totales
            foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
            {
                // Verificar que la fila no sea nueva
                if (row.IsNewRow) continue;
                string Aritculo = row.Cells["CodArticulo"].Value.ToString();
                // Subtotal: Cantidad * Precio
                if (row.Cells["ART_EXIST"].Value != null && row.Cells["ART_PVP"].Value != null)
                {
                    decimal cantidad = Convert.ToDecimal(row.Cells["ART_EXIST"].Value);
                    decimal precio = Convert.ToDecimal(row.Cells["ART_PVP"].Value);
                    subtotal += cantidad * precio;
                }

                // Descuento: Subtotal * (%Descuento / 100)
                if (row.Cells["PORCTDESCUENTO"].Value != null)
                {
                    decimal descuento = Convert.ToDecimal(row.Cells["PORCTDESCUENTO"].Value);
                    descuentoTotal += (Convert.ToDecimal(row.Cells["ART_EXIST"].Value)* Convert.ToDecimal(row.Cells["ART_PVP"].Value)) * (descuento / 100);
                }
                
                // Impuesto: Subtotal * (%Impuesto / 100)
                if (row.Cells["Impuesto"].Value != null)
                {
                    decimal impuesto = Convert.ToDecimal(row.Cells["Impuesto"].Value);
                    subtotalConIva = Convert.ToDecimal(row.Cells["ART_PVP"].Value) * Convert.ToDecimal(row.Cells["ART_EXIST"].Value)- (Convert.ToDecimal(row.Cells["ART_PVP"].Value) * Convert.ToDecimal(row.Cells["ART_EXIST"].Value) * (Convert.ToDecimal(row.Cells["PORCTDESCUENTO"].Value) / 100));
                    ivaTotal += subtotalConIva * (impuesto / 100);
                }
            }

            // Calcular IGTF (por ejemplo, 2% del subtotal)
            igtfTotal = 0;

            // Calcular el total general
            totalGeneral = Math.Round(Math.Round(subtotal,2) - Math.Round(descuentoTotal,2) + Math.Round(ivaTotal,2) + Math.Round(igtfTotal,2),2);

            // Actualizar los valores en el DataGridView de totales
            foreach (DataGridViewRow row in Dgv_Totales.Rows)
            {
                if (row.Cells["Concepto"].Value != null)
                {
                    string concepto = row.Cells["Concepto"].Value.ToString();
                    switch (concepto)
                    {
                        case "SubTotal":
                            row.Cells["Valor"].Value = subtotal;
                            break;
                        case "Descuento":
                            row.Cells["Valor"].Value = descuentoTotal;
                            break;
                        case "IVA":
                            row.Cells["Valor"].Value = ivaTotal;
                            break;
                        case "IGTF":
                            row.Cells["Valor"].Value = igtfTotal;
                            break;
                        case "Total":
                            row.Cells["Valor"].Value = totalGeneral;
                            break;
                        case "Ref":
                            row.Cells["Valor"].Value = totalGeneral / Tasa; // Ejemplo de referencia
                            break;
                    }
                }
            }
        }


        public bool VerificoProductosPermitidos(string CodArticulo, string glbTipoTrabajo, DataGridView gridFacturas, Boolean UsuAsegurado = false)
        {
            stringBuilder.Clear();
            try
            {
                // Verificar si existe más de un producto en el grid
                if (gridFacturas.RowCount > 0)
                {
                    for (int xx = 0; xx < gridFacturas.RowCount; xx++)
                    {
                        if (!string.IsNullOrEmpty(CodArticulo))
                        {
                            // Facturación para aseguradora
                            switch (CodArticulo.Substring(0, 1))
                            {
                                case "M": // Monturas
                                          // No permito monturas si ya existen lentes de contacto agregados
                                    if (gridFacturas.Rows[xx].Cells["CodArticulo"].Value != null &&
                                        gridFacturas.Rows[xx].Cells["CodArticulo"].Value.ToString().StartsWith("W"))
                                    {
                                        stringBuilder.AppendLine("No se permite tener Monturas y Lentes de Contacto en la misma orden");
                                        return false;
                                    }
                                    break;

                                case "C": // Cristales
                                          // No permito cristales si ya existen lentes de contacto agregados
                                    if (gridFacturas.Rows[xx].Cells["CodArticulo"].Value != null &&
                                        gridFacturas.Rows[xx].Cells["CodArticulo"].Value.ToString().StartsWith("W"))
                                    {
                                        stringBuilder.AppendLine("No se permite tener Cristales y Lentes de Contacto en la misma orden");
                                        return false;
                                    }
                                    break;

                                case "L": // Lentes de sol
                                    if (UsuAsegurado)
                                    {
                                        stringBuilder.AppendLine("No se permite facturar Lentes de Sol para Asegurados");
                                        return false;
                                    }
                                    else
                                    {
                                        // No permito cristales si ya existen lentes de contacto agregados
                                        if (gridFacturas.Rows[xx].Cells["CodArticulo"].Value != null &&
                                            gridFacturas.Rows[xx].Cells["CodArticulo"].Value.ToString().StartsWith("M") &&
                                            glbTipoTrabajo == "002")
                                        {
                                            stringBuilder.AppendLine("No se permite facturar Lentes de Sol y Monturas en la misma orden");
                                            return false;
                                        }
                                    }
                                    break;

                                case "W": // Lentes de contacto
                                    if (UsuAsegurado)
                                    {
                                        if (Verifico_Valor_Parametro(CodArticulo, "ArtNoCambiazo1") ||
                                            Verifico_Valor_Parametro(CodArticulo, "ArtNoCambiazo2"))
                                        {
                                            stringBuilder.AppendLine("No se permite facturar Lentes de Contacto Desechables para Asegurados");
                                            return false;
                                        }
                                    }

                                    // No permito cristales si ya existen lentes de contacto agregados
                                    if (gridFacturas.Rows[xx].Cells["CodArticulo"].Value != null &&
                                        gridFacturas.Rows[xx].Cells["CodArticulo"].Value.ToString().StartsWith("C")) // Cristales
                                    {
                                        stringBuilder.AppendLine("No se permite tener Lentes de Contacto y Cristales en la misma orden");
                                        return false;
                                    }
                                    else if (gridFacturas.Rows[xx].Cells["CodArticulo"].Value != null &&
                                             gridFacturas.Rows[xx].Cells["CodArticulo"].Value.ToString().StartsWith("M")) // Monturas
                                    {
                                        stringBuilder.AppendLine("No se permite tener Lentes de Contacto y Monturas en la misma orden");
                                        return false;
                                    }
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                }

                // Si no se encontraron problemas, devolver true
                return true;
            }
            catch (Exception ex)
            {
                stringBuilder.AppendLine($"Error: {ex.Message}");
                return false;
            }
        }

        public bool Verifico_Valor_Parametro(string codAbuscar, string ParametroABuscar)
        {
            try
            {
                // Obtener el valor del parámetro desde la tabla TB_PARAMETRO
                string valorParametro = _D_DetalleOrden.TB_PARAMETRO(ParametroABuscar);

                // Verificar si el código buscado está contenido en el valor del parámetro
                if (!string.IsNullOrEmpty(valorParametro))
                {
                    // Asegurarse de que el código esté rodeado por comas
                    string codigoConComas = $",{codAbuscar},";
                    if (valorParametro.Contains(codigoConComas))
                    {
                        return true; // El código está presente
                    }
                }

                return false; // El código no está presente
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return true;
            }
        }

        public void CargarServicioOPrima(decimal PorcDcto , DataGridView gridFacturas, string tipoServicio, int NumeroExamen = 0, string Nacionalidad = null, string txtCedula = null, string txtOjo = null)
        {
            try
            {
                bool tieneServicio = false;
                bool found = false;
                decimal montoTotal = 0;
                decimal prima = 0;
                int cantidadCristales = 0;
                int cantidadPrisma = 0;
                string codigo = "";
                string PrismaD = "0";
                string PrismaI = "0";

                DataSet dsExamenConPrisma = _D_Articulos.ValidarExamenConPrisma(NumeroExamen, Nacionalidad, txtCedula);

                if (dsExamenConPrisma == null)
                {
                    return;
                }

                if (dsExamenConPrisma.Tables[0].Rows.Count == 0)
                {
                    return; // No hay datos de prisma
                }

                PrismaD = dsExamenConPrisma.Tables[0].Rows[0]["PrismaD"].ToString();
                PrismaI = dsExamenConPrisma.Tables[0].Rows[0]["PrismaI"].ToString();

                if (!string.IsNullOrEmpty(PrismaD) && PrismaD != "0") cantidadPrisma++;
                if (!string.IsNullOrEmpty(PrismaI) && PrismaI != "0") cantidadPrisma++;

                if (PrismaD == "0" && PrismaI == "0")
                {
                    return; // No hay datos de prisma
                }


                // Recorrer las filas del DataGridView
                for (int x = 0; x < gridFacturas.RowCount; x++)
                {
                    codigo = gridFacturas.Rows[x].Cells["CodArticulo"].Value?.ToString();

                    if (codigo == "A000004" || codigo == "S000006")
                    {
                        tieneServicio = true;
                    }
                    else if (!string.IsNullOrEmpty(codigo) && codigo.StartsWith("C"))
                    {
                        found = true;
                        cantidadCristales = Convert.ToInt32(gridFacturas.Rows[x].Cells["ART_EXIST"].Value);
                    }
                }

                if (found)
                {
                    if (tieneServicio)
                    {
                        prima = Convert.ToDecimal(_D_DetalleOrden.TB_PARAMETROSPGE("PorcPrimaPGE")) / 100;

                        // Calcular el monto total
                        for (int x = 0; x < gridFacturas.RowCount; x++)
                        {
                            string codigo2 = gridFacturas.Rows[x].Cells["CodArticulo"].Value?.ToString();

                            if (!string.IsNullOrEmpty(codigo2) && codigo2.StartsWith("C"))
                            {
                                montoTotal += Convert.ToDecimal(gridFacturas.Rows[x].Cells["ART_PVP"].Value);
                            }
                        }

                        // Calcular la prima
                        prima *= montoTotal;

                        // Actualizar la fila correspondiente
                        for (int x = 0; x < gridFacturas.RowCount; x++)
                        {
                            if (gridFacturas.Rows[x].Cells["CodArticulo"].Value?.ToString() == "A000004")
                            {
                                gridFacturas.Rows[x].Cells["ART_PVP"].Value = prima.ToString("N2");
                                gridFacturas.Rows[x].Cells["Total"].Value = prima.ToString("N2");
                                break;
                            }
                        }
                    }
                    else
                    {
                        // Agregar un nuevo servicio o prima
                        List<TB_ARTICULO> articulos = _D_Articulos.ObtenerArticulos("", tipoServicio == "Prisma" ? "S000006" : "A000004");

                        if (articulos != null && articulos.Count > 0)
                        {
                            decimal total;
                            TB_ARTICULO articulo = articulos.First();
                            decimal precio = articulo.ART_PVP;
                            if (articulo.CodArticulo == "A000004")
                            {
                                total = precio * cantidadCristales;
                            }
                            else
                            {
                                total = precio * cantidadPrisma;
                            }
                            decimal CostoPromedio = (decimal) articulo.COSTOPROME;
                            decimal impuesto = articulo.ART_EXENTO ? 0 : BuscarIva("I");

                           AgregarFila(gridFacturas, articulo.CodArticulo, "", "", "", articulo.DESART, tipoServicio == "Prisma" ? cantidadPrisma :  1, (decimal)precio, PorcDcto, (decimal)total, impuesto, "", CostoPromedio, codigo);
          
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BorrarArticulosAgregados(DataGridView gridFacturas, int numArt)
        {
            int inicio = gridFacturas.RowCount - numArt;
            for (int i = inicio; i < gridFacturas.RowCount; i++)
            {
                gridFacturas.Rows.RemoveAt(i);
            }
        }

        private string ObtenerCodigoPadre(DataGridView gridFacturas, int fila)
        {
            for (int j = 0; j < gridFacturas.RowCount; j++)
            {
                string codigo = gridFacturas.Rows[j].Cells["CodArticulo"].Value?.ToString();
                if (!string.IsNullOrEmpty(codigo) && codigo.StartsWith("C"))
                {
                    return codigo;
                }
            }
            return "";
        }

        private void EjecutarAccionServicioAgregado(decimal PorcDcto, DataGridView gridFacturas, int fila, int filaServicioAgregado, string ladoOjo, string TipoTrabajo, SqlCommand sqlCom = null)
        {
            try
            {
                // Determinar si se debe ejecutar la acción según el lado del ojo
                bool ban = false;
                // Cantidad
                int cantidad = 0;
                // Evaluar el valor de ladoOjo
                string AgreDer = "NO";
                string AgreIzq = "NO";

                switch (ladoOjo)
                {
                    case "D": // Derecho
                        if (gridFacturas.Rows[fila].Cells["AgreDer"].Value.ToString() == "SI")
                        {
                            ban = false;
                            break;
                        }
                        else
                        {
                            AgreDer = "SI";
                            cantidad = 1;
                            ban = true;
                            break;
                        }

                    case "I": // Izquierdo
                        if (gridFacturas.Rows[fila].Cells["AgreIzq"].Value.ToString() == "SI")
                        {
                            ban = false;
                            break;
                        }
                        else
                        {
                            AgreIzq = "SI";
                            cantidad = 1;
                            ban = true;
                            break;
                        }
                    default:
                        // Si no coincide con ningún caso, mantener los valores predeterminados
                        cantidad = 0;
                        ban = false;
                        break;
                }

                // Verificar si se debe proceder con la acción
                if (ban)
                {
                    DataSet dsServicioAgregado = _D_Articulos.BucarServicioAgregado();
                    string expresionNuevoPro = dsServicioAgregado.Tables[0].Rows[filaServicioAgregado][3].ToString();

                    // Crear parámetros y espacios de nombres para la evaluación
                    var mParameters = new List<string> { "String Precio" };
                    var mNameSpaces = new List<string> { "System" };

                    if (!string.IsNullOrEmpty(expresionNuevoPro) && PrecompilarAssembly(expresionNuevoPro, mParameters, mNameSpaces))
                    {
                        // Evaluar la expresión
                        var mParam = new object[] { gridFacturas.Rows[fila].Cells["CodArticulo"].Value?.ToString() };
                        object resultado = Evaluar(mParam);

                        // Convertir el resultado a string
                        string codArt = resultado?.ToString();

                        // Obtener el código del artículo padre
                        string codPadre = ObtenerCodigoPadre(gridFacturas, fila).ToUpper();

                        // Agregar artículos promocionales
                        int numArt = 1;
                        for (int numArtPro = 0; numArtPro < NumArtPromocion(codArt); numArtPro++)
                        {
                            string codigoPromocion = ArtPromocion[numArtPro];

                            if (!CargarArticulo_EvitarDuplicado(gridFacturas, codigoPromocion))
                            {

                                // Buscar datos del artículo agregado utilizando ObtenerArticulos
                                var articulos = _D_Articulos.ObtenerArticulos("", codigoPromocion);

                                if (articulos == null || articulos.Count > 0)
                                {
                                    TB_ARTICULO articulo = articulos.First();
                                    decimal precio = articulo.ART_PVP;
                                    decimal total = precio * cantidad;
                                    decimal CostoPromedio = (decimal)articulo.COSTOPROME;
                                    decimal impuesto = articulo.ART_EXENTO ? 0 : BuscarIva("I");
                                    // Agregar nueva fila al DataGridView
                                    AgregarFila(gridFacturas, articulo.CodArticulo,"", "", "", articulo.DESART, cantidad, (decimal)precio, PorcDcto, (decimal)total, impuesto, ladoOjo, CostoPromedio, codPadre,"SI", AgreDer, AgreIzq);
                                    //Actualizo la fila del cristal 
                                    switch (ladoOjo)
                                    {
                                        case "D": // Derecho
                                            ActualizarCelda(gridFacturas, fila, "AgreDer", AgreDer);
                                            break;

                                        case "I": // Izquierdo
                                            ActualizarCelda(gridFacturas, fila, "AgreIzq", AgreIzq);
                                            break;

                                    }
                                }
                                else
                                {
                                    //return;
                                }



                                numArt++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public bool PrecompilarAssembly(string funcion, List<string> parametrosList, List<string> nameSpaceList)
        {
            try
            {
                // Lista de variables que necesitan conversión a double
                List<string> variablesNumericas = new List<string> { "ESFERA_DER", "ESFERA_IZQ", "CILINDRO_DER", "CILINDRO_IZQ" };

                // Reemplazar las variables numéricas con Convert.ToDouble
                foreach (string variable in variablesNumericas)
                {
                    funcion = funcion.Replace(variable, $"Convert.ToDouble({variable})");
                }

                //// Convertir los operadores de la condición
                //string funcionConvertida = funcion
                //    .Replace("AND", "&&")
                //    .Replace("and", "&&")
                //    .Replace("OR", "||")
                //    .Replace("or", "||")
                //    .Replace("OJO=", "OJO==");

                // Construir el código fuente dinámico
                StringBuilder codigoFuente = new StringBuilder();

                // Agregar los "using" necesarios al código fuente
                foreach (string nameSpace in nameSpaceList)
                {
                    codigoFuente.AppendLine($"using {nameSpace};");
                }

                // Preparar los parámetros que usará el método Eval de la clase EvalClase
                string parametros = string.Join(", ", parametrosList);

                // Construir la clase dinámica
                codigoFuente.AppendLine("public class EvalClase");
                codigoFuente.AppendLine("{");
                codigoFuente.AppendLine("    public static object Eval(" + parametros + ")");
                codigoFuente.AppendLine("    {");
                codigoFuente.AppendLine($"        return {funcion};");
                codigoFuente.AppendLine("    }");
                codigoFuente.AppendLine("}");

                //codigoFuente.Clear();
                // Crear una instancia de CSharpCodeProvider para compilar el código
                var codeProvider = new Microsoft.CSharp.CSharpCodeProvider();

                // Configurar los parámetros del compilador
                var compilerParams = new System.CodeDom.Compiler.CompilerParameters
                {
                    GenerateInMemory = true, // Generar el ensamblado en memoria
                    GenerateExecutable = false // No generar un ejecutable
                };

                // Compilar el código fuente
                var compilerResults = codeProvider.CompileAssemblyFromSource(compilerParams, codigoFuente.ToString());

                // Verificar si hubo errores de compilación
                if (compilerResults.Errors.Count > 0)
                {
                    foreach (System.CodeDom.Compiler.CompilerError error in compilerResults.Errors)
                    {
                        MessageBox.Show($"Error de compilación: {error.ErrorText}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return false;
                }
                else
                {
                    // Obtener el ensamblado generado en memoria
                    oEnsamblado = compilerResults.CompiledAssembly; // Asignar el ensamblado generado
                    // Guardar el ensamblado en una variable global si es necesario
                    // o realizar otras operaciones con él.

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la función: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public object Evaluar(params object[] parametros)
        {
            try
            {
                if (oEnsamblado == null)
                {
                    return null; // Si no hay ensamblado, no se puede evaluar
                }
                else
                {
                    // Obtener el tipo de la clase EvalClase dentro del ensamblado
                    Type oClass = oEnsamblado.GetType("EvalClase");

                    if (oClass == null)
                    {
                        throw new Exception("No se encontró la clase 'EvalClase' en el ensamblado");
                    }

                    // Obtener el método Eval de la clase
                    var metodoEval = oClass.GetMethod("Eval");

                    if (metodoEval == null)
                    {
                        throw new Exception("No se encontró el método 'Eval' en la clase EvalClase");
                    }

                    // Invocar el método Eval con los parámetros proporcionados
                    return metodoEval.Invoke(null, parametros);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error en la función: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return null;
            }
        }

        private int NumArtPromocion(string valor)
        {
            try
            {
                // En este código verifico y cuento la cantidad de artículos agregados que posea una
                // promoción según el string que esté escrito en la base de datos y guardo cada artículo
                // encontrado en una lista para poder leerla luego.

                int posicion = 0; // Valor de cada coma

                // Borro los artículos anteriores si los hay
                if (ArtPromocion.Count > 0)
                {
                    ArtPromocion.Clear();
                }

                // Verificar si el valor contiene comas
                if (valor.Contains(","))
                {
                    for (int x = 0; x < valor.Length; x++)
                    {
                        if (valor[x] == ',')
                        {
                            // Obtengo cada código y los guardo sin espacios en blanco
                            string codigo = valor.Substring(posicion, x - posicion).Trim();
                            ArtPromocion.Add(codigo);
                            posicion = x + 1;
                        }
                    }

                    // Agregar el último código después de la última coma
                    string ultimoCodigo = valor.Substring(posicion).Trim();
                    ArtPromocion.Add(ultimoCodigo);

                    // Retorno la cantidad de artículos agregados que posea esta promoción
                    return ArtPromocion.Count;
                }
                else
                {
                    // Si no hay comas, agregar el valor completo como un único artículo
                    ArtPromocion.Add(valor.Trim());
                    return 1;
                }
            }
            catch (Exception ex)
            {

                return 0; // Retornar 0 en caso de error
            }
        }

        public float[] Transposicion(float esf, float cil, int eje)
        {
            // Calcular los valores transpuestos
            float nuevoEsf = esf + cil;
            float nuevoCil = -1 * cil;
            int nuevoEje = (eje <= 90) ? eje + 90 : eje - 90;

            // Devolver los valores como un array
            return new float[] { nuevoEsf, nuevoCil, nuevoEje };
        }

        public void EvaluoServicioAgregado(decimal PorcDcto, DataGridView gridFacturas, int fila, string ladoOjo, int NunExamen, string TipoTrabajo, string Nacionalidad, string txtCedula)
        {
            // Declaración de variables
            double ESFD;
            double ESFI;
            double CILD;
            double CILI;
            int EJED;
            int EJEI;
            double ADDD;
            double ADDI;

            try
            {
                //// Determinar el lado del ojo basado en los valores del DataGridView
                //var ojo = gridFacturas.Rows[fila].Cells["Ojo"].Value?.ToString();
                //if (ojo == "D")
                //{
                //    ladoOjo = "D";
                //}
                //else if (ojo == "I")
                //{
                //    ladoOjo = "I";
                //}
                //else if (ojo == "A")
                //{
                //    ladoOjo = "Ambos";
                //}

                // Evaluar si los artículos tienen promociones y si se pueden cumplir
                int x = 0;
                string expresion;

                // Crear parámetros y espacios de nombres para la evaluación
                var mParameters = new List<string>
                {
            "string CILINDRO_DER",
            "string CILINDRO_IZQ",
            "string ESFERA_DER",
            "string ESFERA_IZQ",
            "string CANTIDAD",
            "string OJO",
            "string OjoDer",
            "string OjoIzq"
                };

                var mNameSpaces = new List<string>
                {
            "System"
                };
                DataSet dsServicioAgregado = _D_Articulos.BucarServicioAgregado();
                // Evaluar todas las promociones
                if (dsServicioAgregado == null)
                {
                    return;
                }
                foreach (DataRow dr in dsServicioAgregado.Tables[0].Rows)
                {
                    if (gridFacturas.RowCount - 1 < fila)
                        break;

                    if (gridFacturas.Rows[fila].Cells["AgreDer"].Value?.ToString() != null && gridFacturas.Rows[fila].Cells["AgreIzq"].Value?.ToString() != null)
                    {
                        if (gridFacturas.Rows[fila].Cells["AgreDer"].Value?.ToString() == "NO" || gridFacturas.Rows[fila].Cells["AgreIzq"].Value?.ToString() == "NO")
                        {
                            expresion = dr[2].ToString();

                            if (PrecompilarAssembly(expresion, mParameters, mNameSpaces))
                            {
                                if (_D_Inicio == null)
                                    _D_Inicio = new D_Inicio();

                                // Obtener datos del examen utilizando la nueva función
                                List<TB_Examen> examenes = _D_Articulos.ObtenerExamen(Nacionalidad, txtCedula, _D_Inicio.Sucursal(), NunExamen);

                                // Validar si se obtuvieron resultados
                                if ((TipoTrabajo == "08") || (examenes != null && examenes.Count > 0))
                                {
                                    // Tomar el primer resultado (o manejar múltiples resultados si es necesario)
                                    TB_Examen exa = examenes.First();

                                    // Realizar transposición de fórmulas

                                    // Verificar y calcular los valores transpuestos para el ojo derecho
                                    if (exa.ESFD < 0)
                                    {
                                        float[] transposicionDerecho = Transposicion((float)exa.ESFD, (float)exa.CILD, (int)exa.EJED);
                                        ESFD = transposicionDerecho[0];
                                        CILD = transposicionDerecho[1];
                                        EJED = (int)transposicionDerecho[2];
                                    }
                                    else
                                    {
                                        ESFD = (double)exa.ESFD;
                                        CILD = (double)exa.CILD;
                                        EJED = (int)exa.EJED;
                                    }

                                    // Verificar y calcular los valores transpuestos para el ojo izquierdo
                                    if (exa.ESFI < 0)
                                    {
                                        float[] transposicionIzquierdo = Transposicion((float)exa.ESFI, (float)exa.CILI, (int)exa.EJEI);
                                        ESFI = transposicionIzquierdo[0];
                                        CILI = transposicionIzquierdo[1];
                                        EJEI = (int)transposicionIzquierdo[2];
                                    }
                                    else
                                    {
                                        ESFI = (double)exa.ESFI;
                                        CILI = (double)exa.CILI;
                                        EJEI = (int)exa.EJEI;
                                    }

                                    // Asignar valores adicionales (si es necesario)
                                    ADDD = exa.ADDD.HasValue ? (double)exa.ADDD : 0.0;
                                    ADDI = exa.ADDI.HasValue ? (double)exa.ADDI : 0.0;


                                    // Parámetros para la evaluación
                                    var mParam = new object[]
                                    {
                                     CILD.ToString(), CILI.ToString(), ESFD.ToString(), ESFI.ToString(),
                                     gridFacturas.Rows[fila].Cells["ART_EXIST"].Value.ToString(), ladoOjo,
                                     gridFacturas.Rows[fila].Cells["AgreDer"].Value?.ToString() != null ? gridFacturas.Rows[fila].Cells["AgreDer"].Value?.ToString()  : "NO", // Verificar si AgreDer no es nulo
                                     gridFacturas.Rows[fila].Cells["AgreIzq"].Value?.ToString() != null ? gridFacturas.Rows[fila].Cells["AgreIzq"].Value?.ToString()  : "NO"  // Verificar si AgreIzq no es nulo

                                    };

                                    // Evaluar la expresión
                                    object resultado = Evaluar(mParam);

                                    if (resultado != null && (bool)resultado == true)
                                    {
                                        EjecutarAccionServicioAgregado(PorcDcto, gridFacturas, fila, x, ladoOjo, TipoTrabajo);
                                    }
                                }
                                else
                                {
                                    // Manejar el caso en que no se obtuvieron resultados
                                    MessageBox.Show("No se encontraron datos del examen.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                        
                        }
                        x++;
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }
        public void VerificarMonturaPropia(DataGridView Dgv_Tap3_Articulo, System.Windows.Forms.Button Btn_Tap3_MonturaPropia, bool Montura_Propia)
        {

            bool Posee_Montura = false;

            // Verificar si tiene  cristal
            foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
            {
                if (row.Cells["CodArticulo"].Value != null && (row.Cells["CodArticulo"].Value.ToString().StartsWith("M") || row.Cells["CodArticulo"].Value.ToString().StartsWith("L")))
                {
                    Posee_Montura = true;
                    break; // Salir del bucle al encontrar el primer cristal
                }
                else
                {
                    Posee_Montura = false;
                }
            }

            if (Montura_Propia == true)
            {
                Btn_Tap3_MonturaPropia.Enabled = false;
            }
            else if (Montura_Propia == false && Posee_Montura == false)
            {
                Btn_Tap3_MonturaPropia.Enabled = true;
            }
            else if (Montura_Propia == false && Posee_Montura == true)
            {
                Btn_Tap3_MonturaPropia.Enabled = false;
            }

        }

        public void VerificarCristalPropio(DataGridView Dgv_Tap3_Articulo, System.Windows.Forms.Button Btn_Tap3_CristalPropio, bool Cristal_Propio)
        {
            bool Posee_cristal = false;

            // Verificar si tiene  cristal
            foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
            {
                if (row.Cells["CodArticulo"].Value != null && row.Cells["CodArticulo"].Value.ToString().StartsWith("C"))
                {
                    Posee_cristal = true;
                    break; // Salir del bucle al encontrar el primer cristal
                }
                else
                {
                    Posee_cristal = false;
                }
            }

            if (Cristal_Propio == true)
            {
                Btn_Tap3_CristalPropio.Enabled = false;
            }
            else if (Cristal_Propio == false && Posee_cristal == false)
            {
                Btn_Tap3_CristalPropio.Enabled = true;
            }
            else if (Cristal_Propio == false && Posee_cristal == true)
            {
                Btn_Tap3_CristalPropio.Enabled = false;
            }

        }

        public bool ServicioColoracion(DataGridView Dgv_Tap3_Articulo, DataGridView Dvg_Coloracion, System.Windows.Forms.RadioButton Rd_FullColor)
        {
            try
            {
                string cristalColor = string.Empty;
                bool colorDegra = Rd_FullColor.Checked ? false : true;

                // Obtener el código del cristal
                foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
                {
                    if (row.Cells["CodArticulo"].Value != null && row.Cells["CodArticulo"].Value.ToString().StartsWith("C"))
                    {
                        cristalColor = row.Cells["CodArticulo"].Value.ToString();
                        break; // Salir del bucle al encontrar el primer cristal
                    }
                }

                // Buscar el servicio de coloración
                foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
                {
                    if (row.Cells["CodArticulo"].Value != null && row.Cells["CodArticulo"].Value.ToString() == "S000004" && !string.IsNullOrEmpty(cristalColor))
                    {
                        DataSet dsColor = _D_Articulos.BucarColoracion(cristalColor, colorDegra);

                        if (dsColor.Tables[0].Rows.Count > 0)
                        {
                            DataTable dtColoracion = new DataTable("Coloracion");
                            dtColoracion.Columns.Add("Cod_Coloracion", typeof(string));
                            dtColoracion.Columns.Add("Desc_Color", typeof(string));
                            dtColoracion.Columns.Add("Porc_Material", typeof(string));

                            foreach (DataRow dr in dsColor.Tables[0].Rows)
                            {
                                //Regex.Replace:  Se utiliza para buscar y reemplazar patrones en cadenas.
                                //El patrón @"\s{3,}" significa:
                                //\s: Coincide con cualquier espacio en blanco(espacio, tabulación, salto de línea, etc.).
                                //{ 3,}: Coincide con tres o más espacios consecutivos.
                                DataRow drColoracion = dtColoracion.NewRow();
                                drColoracion["Cod_Coloracion"] = Regex.Replace(dr["Cod_Coloracion"].ToString(), @"\s{3,}", ""); // Reemplaza 3 o más espacios por nada
                                drColoracion["Desc_Color"] = Regex.Replace(dr["Desc_Color"].ToString(), @"\s{3,}", "");
                                drColoracion["Porc_Material"] = Regex.Replace(dr["Porc_Material"].ToString(), @"\s{3,}", "");
                                dtColoracion.Rows.Add(drColoracion);
                            }

                            Dvg_Coloracion.DataSource = dtColoracion;
                            return true;
                        }

                        break; // Salir del bucle al procesar el servicio
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void VerificarServicioCodigoPadre(DataGridView Dgv_Tap3_Articulo)
        {
            string CodCristal = "";

            // Obtener el código del cristal
            foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
            {
                if (row.Cells["CodArticulo"]?.Value != null && row.Cells["CodArticulo"].Value.ToString().StartsWith("C"))
                {
                    CodCristal = row.Cells["CodArticulo"].Value.ToString();
                    break; // Salir del bucle al encontrar el primer cristal
                }
            }

            // Servicios Dioptria
            DataSet dsServicioAgregado = _D_Articulos.BucarServicioAgregado();

            // Validar servicios asociados
            foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
            {
                string codArticulo = row.Cells["CodArticulo"]?.Value?.ToString();
                string artPadre = row.Cells["ArtPadre"]?.Value?.ToString();

                if (string.IsNullOrEmpty(codArticulo) || string.IsNullOrEmpty(CodCristal))
                    continue; // Saltar filas inválidas

                // Coloración
                if (codArticulo == "S000004" && string.IsNullOrEmpty(artPadre))
                {
                    ActualizarCelda(Dgv_Tap3_Articulo, row.Index, "ArtPadre", CodCristal);
                }
                // Otros servicios
                else if (codArticulo.StartsWith("S") && string.IsNullOrEmpty(artPadre))
                {
                    if (dsServicioAgregado != null)
                    {
                        if (dsServicioAgregado.Tables.Count > 0 && dsServicioAgregado.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsServicioAgregado.Tables[0].Rows)
                            {
                                string agregadoProducto = dr["Agregado_Producto"]?.ToString().Trim('"');
                                if (!string.IsNullOrEmpty(agregadoProducto) && agregadoProducto == codArticulo)
                                {
                                    ActualizarCelda(Dgv_Tap3_Articulo, row.Index, "ArtPadre", CodCristal);
                                    break; // Salir del bucle interno si se encuentra el servicio
                                }
                            }
                        }
                    }
                }
            }

            //Si se agrego algun AR manual le coloco el cristal como padre
            foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
            {
               if (row.Cells["CodArticulo"]?.Value != null && row.Cells["CodArticulo"].Value.ToString().StartsWith("C"))
               {
                    CodCristal = row.Cells["CodArticulo"].Value.ToString();
                    break; // Salir del bucle al encontrar el primer cristal
               }
            }
            


            foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
            {
                    DataSet dsServAR = _D_Articulos.ServiciosAR_btnProcesar(CodCristal, false, null);
                    //Si es un AR (validar con tabla 1 del dataset)
                    foreach (DataRow filaAR in dsServAR.Tables[1].Rows)
                    {
                        string codAR = filaAR["CodServicio"].ToString();
                        if (row.Cells["CodArticulo"].Value.ToString() == codAR)
                        {
                            //artPAdre = codigo;
                            ActualizarCelda(Dgv_Tap3_Articulo, row.Index, "ArtPadre", CodCristal);
                            break;
                        }
                    }
            }

            //artPadre = _L_Articulo.EsARManual(Dgv_Tap3_Articulo, articulo.CodArticulo, null);
        }

        public void Verificar_Cantidad_Articulo_Ingresada_Servicios(DataGridView Dgv_Tap3_Articulo)
        {
            string CodCristal = "";
            int Cantidad_Cristal = 0;
            // Obtener el código del cristal
            foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
            {
                if (row.Cells["CodArticulo"]?.Value != null && row.Cells["CodArticulo"].Value.ToString().StartsWith("C"))
                {
                    CodCristal = row.Cells["CodArticulo"].Value.ToString();
                    Cantidad_Cristal = row.Cells["ART_EXIST"].Value != null ? Convert.ToInt32(row.Cells["ART_EXIST"].Value) : 0;
                    break; // Salir del bucle al encontrar el primer cristal
                }
            }

            // Servicios Dioptria
            DataSet dsServicioAgregado = _D_Articulos.BucarServicioAgregado();
            //AR
            DataSet dsServAR = _D_Articulos.ServiciosAR_btnProcesar("", false);
            // Validar servicios asociados
            foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
            {
                string codArticulo = row.Cells["CodArticulo"]?.Value?.ToString();
                int Cantidad_Servicio = row.Cells["ART_EXIST"].Value != null ? Convert.ToInt32(row.Cells["ART_EXIST"].Value) : 0;
                if (string.IsNullOrEmpty(codArticulo) || string.IsNullOrEmpty(CodCristal))
                    continue; // Saltar filas inválidas

                // Otros servicios
                else if (codArticulo.StartsWith("S") && Cantidad_Cristal !=  Cantidad_Servicio)
                {
                    // Coloración
                    //if (codArticulo == "S000004" || codArticulo == "S000006")
                    if (codArticulo == "S000004" )
                    {
                        ActualizarCelda(Dgv_Tap3_Articulo, row.Index, "ART_EXIST", Cantidad_Cristal.ToString());
                    }


                        //if (dsServicioAgregado != null && dsServicioAgregado.Tables.Count > 0 && dsServicioAgregado.Tables[0].Rows.Count > 0)
                        //{
                        //    foreach (DataRow dr in dsServicioAgregado.Tables[0].Rows)
                        //    {
                        //        string agregadoProducto = dr["Agregado_Producto"]?.ToString().Trim('"');
                        //        //if (!string.IsNullOrEmpty(agregadoProducto) && agregadoProducto == codArticulo && Cantidad_Cristal != Cantidad_Servicio)
                        //        if (!string.IsNullOrEmpty(agregadoProducto) && agregadoProducto == codArticulo)
                        //        {
                        //            ActualizarCelda(Dgv_Tap3_Articulo, row.Index, "ART_EXIST", Cantidad_Cristal.ToString());
                        //            break; // Salir del bucle interno si se encuentra el servicio
                        //        }
                        //    }
                        //}
                    

                    //if (dsServAR != null && dsServAR.Tables.Count > 1 && dsServAR.Tables[1].Rows.Count > 0)
                    //{
                    //    foreach (DataRow dr in dsServAR.Tables[1].Rows)
                    //    {
                    //        string agregadoProducto = dr["CodServicio"]?.ToString().Trim('"');
                    //        //if (!string.IsNullOrEmpty(agregadoProducto) && agregadoProducto == codArticulo && Cantidad_Cristal != Cantidad_Servicio)
                    //        if (!string.IsNullOrEmpty(agregadoProducto) && agregadoProducto == codArticulo)
                    //        {
                    //            ActualizarCelda(Dgv_Tap3_Articulo, row.Index, "ART_EXIST", Cantidad_Cristal.ToString());
                    //            break; // Salir del bucle interno si se encuentra el servicio
                    //        }
                    //    }
                    //}
                }
            }
        }

        public void RemoveColoracion(DataGridView Dgv_Tap3_Articulo, ref string Codigo_Coloracion)
        {

            bool encontrado = false;

            // Recorrer todas las filas del DataGridView
            foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
            {
                // Verificar si la celda "CodArticulo" no es nula y tiene el valor "S000004"
                if (row.Cells["CodArticulo"].Value != null && row.Cells["CodArticulo"].Value.ToString() == "S000004")
                {
                    encontrado = true; // Se encontró el valor "S000004"
                    break; // Salir del bucle, ya que no necesitamos seguir buscando
                }
            }

            // Si no se encontró "S000004", actualizar la variable Codigo_Coloracion
            if (!encontrado)
            {
                Codigo_Coloracion = ""; // Actualizar el valor
            }
        }

        public bool AccionCambiarPrecio_ArticuloPadre(DataGridView gexFacturas, int filaActual)
        {

            // Verificar si el artículo no tiene un padre (no es agregado por promoción)
            if (gexFacturas.Rows[filaActual].Cells["ArtPadre"].Value != DBNull.Value &&
              !string.IsNullOrEmpty(gexFacturas.Rows[filaActual].Cells["ArtPadre"].Value.ToString()))
            {

                return true;

            }

            return false;
        }


        public bool ConfigurarCambioPrecio(System.Windows.Forms.TextBox TxtPrecioActual, string CodigoArticulo)
        {
            // Bloquear el cuadro de texto para que no se pueda editar
            // Colocar el foco
            // Ocultar el campo de motivos

            // Buscar datos del artículo agregado utilizando ObtenerArticulos
            var articulos = _D_Articulos.ObtenerArticulos("", CodigoArticulo);

            if (articulos == null || articulos.Count > 0)
            {
                TB_ARTICULO articulo = articulos.First();
                decimal precio = articulo.ART_PVP;
                TxtPrecioActual.Text = string.Format("{0:#,0.00}", articulo.ART_PVP.ToString() == "" ? (Decimal?)0.00 : articulo.ART_PVP);
                return true;
            }
            return false;
        }


        public bool CambioPrecio(System.Windows.Forms.TextBox txtValor2, System.Windows.Forms.TextBox txtValor1, decimal DescMax)
        {
            stringBuilder.Clear();
            try
            {
                // Verificar si el nuevo precio está dentro del límite de descuento permitido
                decimal descuentoPermitido = (decimal.Parse(txtValor1.Text) * DescMax) / 100;

                if (!string.IsNullOrEmpty(txtValor2.Text) && !string.IsNullOrEmpty(txtValor1.Text))
                {
                    // Obtener el valor del parámetro desde la tabla TB_PARAMETRO
                    string valorParametro = _D_DetalleOrden.TB_PARAMETRO("CambPrecArrBaj");

                    // Verificar si el cambio de precio es hacia abajo y está permitido
                    if (decimal.Parse(txtValor2.Text) < decimal.Parse(txtValor1.Text) && valorParametro != "SI")
                    {
                        stringBuilder.Append("El cambio de precio no puede ser menor al precio actual de este producto, verifique e intente de nuevo.\n\n(Si desea hacer algún descuento utilice el botón F4 DESCUENTO)");
                        return false;
                    }

                    else if ((decimal.Parse(txtValor1.Text) - decimal.Parse(txtValor2.Text)) > descuentoPermitido && valorParametro != "SI")
                    {
                        stringBuilder.Append("No está autorizado para dar este descuento, ¿desea introducir una clave autorizada para poder continuar?");
                        return true;

                    }
                    else
                    {

                        return true;
                    }


                }
                else
                {
                    stringBuilder.Append("Por favor, asegúrate de completar todos los campos necesarios antes de proceder con esta acción");
                    return false;
                }
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return false;
            }
        }

        public bool CalculoDescuento(string precioMontoTotal, System.Windows.Forms.DataGridView DgvArticulo, string TipoDescuento, string DescMax, System.Windows.Forms.TextBox Porce_Descuento, System.Windows.Forms.TextBox Monto_Descuento, System.Windows.Forms.TextBox txtMotivo)
        {
            stringBuilder.Clear();
            try
            {
                // Total de la compra 
                //string precioMontoTotal;
                DataSet dsDesc;

                if (TipoDescuento == "Descuento Global")
                {
                    if (!string.IsNullOrEmpty(Porce_Descuento.Text))
                    {
                        if (Convert.ToDecimal(Porce_Descuento.Text) > 100)
                        {

                            stringBuilder.Append("El porcentaje de descuento es mayor al 100%");
                            Porce_Descuento.Focus();
                            Porce_Descuento.SelectAll();
                            return false;
                        }
                        else
                        {
                            for (int x = 0; x < DgvArticulo.RowCount; x++)
                            {
                                dsDesc = _D_Articulos.PermisosDescuento(DgvArticulo.Rows[x].Cells["CodArticulo"].Value.ToString(), Porce_Descuento.Text, TB_USUARIO.Id_Rol);


                                if (dsDesc != null && dsDesc.Tables.Count > 0 && dsDesc.Tables[0].Rows.Count > 0)
                                {
                                    stringBuilder.Append($"La marca {dsDesc.Tables[0].Rows[0][0]} no permite este % de descuento");
                                    Porce_Descuento.Focus();
                                    Porce_Descuento.SelectAll();
                                    return false;
                                }
                            }

                            if (Convert.ToDecimal(Porce_Descuento.Text) > Convert.ToDecimal(DescMax))
                            {
                                Monto_Descuento.Text = ((Convert.ToDecimal(Porce_Descuento.Text) * Convert.ToDecimal(precioMontoTotal)) / 100).ToString("N2");
                                txtMotivo.Focus();
                                return true;
                            }
                            else
                            {
                                Monto_Descuento.Text = ((Convert.ToDecimal(Porce_Descuento.Text) * Convert.ToDecimal(precioMontoTotal)) / 100).ToString("N2");
                                txtMotivo.Focus();
                                return true;
                            }

                        }
                    }
                    else if (!string.IsNullOrEmpty(Monto_Descuento.Text))
                    {
                        if (Convert.ToDecimal(Monto_Descuento.Text) > Convert.ToDecimal(precioMontoTotal))
                        {
                            stringBuilder.Append("El monto del descuento no puede ser mayor que el total de la orden");
                            Monto_Descuento.Focus();
                            Monto_Descuento.SelectAll();
                            return false;
                        }
                        else
                        {
                            Porce_Descuento.Text = ((Convert.ToDecimal(Monto_Descuento.Text) * 100) / Convert.ToDecimal(precioMontoTotal)).ToString("N2");
                            return true;

                        }
                    }
                }
                else // Descuento por artículo
                {
                    if (!string.IsNullOrEmpty(Porce_Descuento.Text))
                    {
                        if (Convert.ToDecimal(Porce_Descuento.Text) > 100)
                        {
                            stringBuilder.Append("El porcentaje de descuento es mayor al 100%");
                            Porce_Descuento.Focus();
                            Porce_Descuento.SelectAll();
                            return false;
                        }
                        else
                        {
                            dsDesc = _D_Articulos.PermisosDescuento(DgvArticulo.CurrentRow.Cells["CodArticulo"].Value.ToString(), Porce_Descuento.Text, TB_USUARIO.Id_Rol);

                            if (dsDesc != null && dsDesc.Tables.Count > 0 && dsDesc.Tables[0].Rows.Count > 0)
                            {

                                stringBuilder.Append($"La marca {dsDesc.Tables[0].Rows[0][0]} no permite este % de descuento");
                                Porce_Descuento.Focus();
                                Porce_Descuento.SelectAll();
                                return false;
                            }

                            Monto_Descuento.Text = ((Convert.ToDouble(Porce_Descuento.Text) * Convert.ToDouble((DgvArticulo.CurrentRow.Cells["ART_PVP"].Value.ToString()))) / 100).ToString("N2");
                            txtMotivo.Focus();
                            return true;

                        }
                    }
                    else if (!string.IsNullOrEmpty(Monto_Descuento.Text))
                    {
                        if (Convert.ToDecimal(Monto_Descuento.Text) > Convert.ToDecimal(DgvArticulo.CurrentRow.Cells["ART_PVP"].Value))
                        {
                            stringBuilder.Append("El monto del descuento no puede ser mayor que el precio original");
                            Monto_Descuento.Focus();
                            Monto_Descuento.SelectAll();
                            return false;
                        }
                        else
                        {
                            Porce_Descuento.Text = ((Convert.ToDecimal(Monto_Descuento.Text) * 100) / Convert.ToDecimal(DgvArticulo.CurrentRow.Cells["ART_PVP"].Value)).ToString("N2");
                            Monto_Descuento.Text = Convert.ToDecimal(Monto_Descuento.Text).ToString("N2");
                            return true;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return false;
            }
        }

        public void Cargo_CodMotivo_Descuento(System.Windows.Forms.ComboBox cbCodMotivo)
        {
            DataTable dtMotivoDes = _D_Articulos.MOTIVOSDESCUENTO();

            if (dtMotivoDes.Rows.Count > 0)
            {
                // Asignar el DataTable como fuente de datos del ComboBox
                cbCodMotivo.DataSource = dtMotivoDes;

                cbCodMotivo.DisplayMember = "Descripcion";
                cbCodMotivo.ValueMember = "CodMotivo";
            }
            else
            {
                // Si no hay datos, limpiar el ComboBox
                cbCodMotivo.DataSource = null;
                cbCodMotivo.Items.Clear();
            }

        }

        public string BuscarCodigoGerenteDescuento(string Codigo)
        {

            DataTable dtMotivoDes = _D_Articulos.MOTIVOSDESCUENTO(Codigo);

            if (dtMotivoDes.Rows.Count > 0)
            {
                return dtMotivoDes.Rows[0]["CodMotivo"].ToString();
            }
            else
            {
                return "";
            }

        }

        public bool VerificarTopeMaximoDesceunto(decimal PorcentajeDescuento )
        {

            // Obtener el valor del parámetro desde la tabla TB_PARAMETRO
            string valorParametro = _D_DetalleOrden.TB_PARAMETRO("TopeDescuento");
            if (PorcentajeDescuento > Convert.ToDecimal(valorParametro ))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public void CargarClientesAfiliados(System.Windows.Forms.DataGridView DgvClienteAfiliados, List<TB_EMPAFI> listaClienteAfiliados)
        {
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
                // Obtener los artículos desde la base de datos
                var clientesObtenidos = _D_Articulos.ObtenerClientesAfiliados(command);

                // Limpiar la lista pasada como parámetro y llenarla con los nuevos datos
                listaClienteAfiliados.Clear(); // Limpiar la lista para evitar duplicados
                listaClienteAfiliados.AddRange(clientesObtenidos); // Agregar los datos obtenidos

                // Asignar la lista como fuente de datos del DataGridView
                if (listaClienteAfiliados != null && listaClienteAfiliados.Count > 0 && _D_Articulos.stringBuilder.Length == 0)
                {
                    //DgvArticulo.DataSource = listaArticulos;

                    // Confirmar la transacción
                    transaction.Commit();
                }
                else
                {
                    //DgvArticulo.DataSource = null; // Si no hay datos, limpiar el DataGridView
                    _D_Articulos.stringBuilder.AppendLine("No se pudieron cargar los artículos correctamente");
                    transaction.Rollback();
                }


            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                transaction.Rollback();
            }

        }


        //Botón PROCESAR:
        public async Task<List<ServicioColoracion_CargarOrdenes>> EjecutarServicioColoracion_btnProcesar(string cristal, bool tipoColor, SqlCommand command = null)
        {
            return await Task.Run(() =>
            {
                var listaColoracion = new List<ServicioColoracion_CargarOrdenes>();
                DataTable dt = _D_Articulos.ServicioColoracion_btnProcesar(cristal, tipoColor, command);

                if (dt == null || dt.Rows.Count == 0)
                    return listaColoracion;

                foreach (DataRow row in dt.Rows)
                {
                    var color = new ServicioColoracion_CargarOrdenes
                    {
                        Cod_Coloracion = row["Cod_Coloracion"].ToString(),
                        Desc_Material = row["Desc_Material"].ToString(),
                        Desc_Color = row["Desc_Color"].ToString(),
                        Porc_Material = row["Porc_Material"].ToString(),
                        Tipo_Color = row["Tipo_Color"].ToString()
                    };
                    listaColoracion.Add(color);
                }

                return listaColoracion;
            });
        }

        public async Task<List<ServiciosAR_CargarOrdenes>> ObtenerServiciosAR_btnProcesar(string codCristal, bool codServicio, SqlCommand command = null)
        {
            return await Task.Run(() =>
            {
                var listaServicioAR = new List<ServiciosAR_CargarOrdenes>();

                DataSet dt = _D_Articulos.ServiciosAR_btnProcesar(codCristal, codServicio, command);

                foreach (DataRow row in dt.Tables[0].Rows)
                {
                    var item = new ServiciosAR_CargarOrdenes
                    {
                        CodServicio = row["CodServicio"].ToString(),
                        Obligatorio = Convert.ToBoolean(row["obligatorio"])
                    };
                    listaServicioAR.Add(item);
                }

                return listaServicioAR;

            });

        }

        public DataSet ObtenerServiciosARDataset(string codCristal, bool codServicio, SqlCommand command = null)
        {
            return _D_Articulos.ServiciosAR_btnProcesar(codCristal, codServicio, command);
        }




        public bool ObtenerPromoVigente(System.Windows.Forms.DataGridView Dgv_Pnl3_Promociones)
        {
            stringBuilder.Clear();
            // Obtener las promociones activas 
            DataTable PromoVigente = _D_Articulos.ObtenerPromoVigente();

            // Validar si PromoVigente no es null
            if (PromoVigente != null && PromoVigente.Rows.Count > 0)
            {
                Dgv_Pnl3_Promociones.DataSource = PromoVigente;
                Formato_Dgv_Promociones(Dgv_Pnl3_Promociones);
                if (stringBuilder.Length > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                stringBuilder.Append("No hay promociones activas");
                return false;
            }

        }

        public void CargarServicioMonturaPropia(DataGridView gridFacturas, bool completa, string txtOjo = null)
        {
            // Agregar un nuevo servicio o prima
            List<TB_ARTICULO> articulos = _D_Articulos.ObtenerArticulos("", completa == true ? "S000106" : "S000105");
            string codigo = "";
            bool tieneServicioMonturaPropia = false;

            if (articulos != null && articulos.Count > 0)
            {
                for (int x = 0; x < gridFacturas.RowCount; x++)
                {
                    codigo = gridFacturas.Rows[x].Cells["CodArticulo"].Value?.ToString();

                    if (codigo == "S000106" || codigo == "S000105")
                    {
                        tieneServicioMonturaPropia = true;
                    }
                    //else if (!string.IsNullOrEmpty(codigo) && codigo.StartsWith("C"))
                    //{
                       
                    //}
                }
                TB_ARTICULO articulo = articulos.First();
                decimal precio = articulo.ART_PVP;
                decimal total = precio * 1;
                decimal CostoPromedio = (decimal)articulo.COSTOPROME;
                decimal impuesto = articulo.ART_EXENTO ? 0 : BuscarIva("I");

                if (!tieneServicioMonturaPropia)
                {
                    AgregarFila(gridFacturas, articulo.CodArticulo, "", "", "", articulo.DESART, 1, (decimal) precio, (decimal) articulo.PORCTDESCUENTO, (decimal) total, impuesto, "", CostoPromedio, codigo);
                }
            }

        }

        public void CargarServicioGarantia(DataGridView gridFacturas, string txtOjo = null)
        {
            // Agregar un nuevo servicio o prima
            bool tieneServicio = false;
            bool found = false;
            decimal montoTotal = 0;
            decimal prima = 0;
            int cantidad = 0;
            string codigo = "";
            string codigoCristal;
            decimal montoTotalServicios = 0;
            int filaSeleccionada = 0;

            prima = Convert.ToDecimal(_D_DetalleOrden.TB_PARAMETROSPGE("PorcPrimaPGE")) / 100;

            for (int x = 0; x < gridFacturas.RowCount; x++)
            {
                codigo = gridFacturas.Rows[x].Cells["CodArticulo"].Value?.ToString();

                if (codigo == "A000004")
                {
                    tieneServicio = true;
                    filaSeleccionada = x;
                }
                else if (!string.IsNullOrEmpty(codigo) && codigo.StartsWith("C"))
                {
                    found = true;
                    codigoCristal = gridFacturas.Rows[x].Cells["CodArticulo"].Value?.ToString();
                    cantidad = Convert.ToInt32(gridFacturas.Rows[x].Cells["ART_EXIST"].Value);
                  
                }
            }

            //if (!tieneServicio)
            //{

            //    for (int x = 0; x < gridFacturas.RowCount; x++)
            //    {
            //        string codArticulo =  gridFacturas.Rows[x].Cells["CodArticulo"].Value?.ToString();
            //        string artPadre = gridFacturas.Rows[x].Cells["ArtPadre"].Value?.ToString();

            //        if (artPadre != "" && gridFacturas.Rows[x].Cells["ArtPadre"].Value != DBNull.Value)
            //        {
            //            montoTotalServicios += Convert.ToDecimal(gridFacturas.Rows[x].Cells["ART_PVP"].Value)* Convert.ToInt32(gridFacturas.Rows[x].Cells["ART_EXIST"].Value);
            //        }
            //    }

            //    // Calcular el monto total
            //    for (int x = 0; x < gridFacturas.RowCount; x++)
            //    {
            //        string codigo2 = gridFacturas.Rows[x].Cells["CodArticulo"].Value?.ToString();

            //        if (!string.IsNullOrEmpty(codigo2) && codigo2.StartsWith("C"))
            //        {
            //            montoTotal += Convert.ToDecimal(gridFacturas.Rows[x].Cells["ART_PVP"].Value)* Convert.ToInt32(gridFacturas.Rows[x].Cells["ART_EXIST"].Value);
            //        }
            //    }
            //    List<TB_ARTICULO> articulos = _D_Articulos.ObtenerArticulos("", "A000004");

            //    if (articulos != null && articulos.Count > 0)
            //    {
            //        decimal totalprima;
            //        totalprima = montoTotal + montoTotalServicios;
            //        TB_ARTICULO articulo = articulos.First();
            //        decimal precio = totalprima*= prima;
            //        decimal total = precio * 1;
            //        decimal CostoPromedio = (decimal)articulo.COSTOPROME;
            //        decimal impuesto = articulo.ART_EXENTO ? 0 : BuscarIva("I");

            //        AgregarFila(gridFacturas, articulo.CodArticulo, "", articulo.DESART, 1, (decimal)precio, (decimal)articulo.PORCTDESCUENTO, (decimal)total, impuesto, "", CostoPromedio, codigo);
            //    }
            //}
            //else
            //{
                for (int x = 0; x < gridFacturas.RowCount; x++)
                {
                    string codArticulo = gridFacturas.Rows[x].Cells["CodArticulo"].Value?.ToString();
                    string artPadre = gridFacturas.Rows[x].Cells["ArtPadre"].Value?.ToString();

                    if (artPadre != "" && gridFacturas.Rows[x].Cells["ArtPadre"].Value != DBNull.Value && codArticulo != "A000004")
                    {
                        montoTotalServicios += Convert.ToDecimal(gridFacturas.Rows[x].Cells["ART_PVP"].Value) * Convert.ToInt32(gridFacturas.Rows[x].Cells["ART_EXIST"].Value);
                    }
                }

                // Calcular el monto total
                for (int x = 0; x < gridFacturas.RowCount; x++)
                {
                    string codigo2 = gridFacturas.Rows[x].Cells["CodArticulo"].Value?.ToString();

                    if (!string.IsNullOrEmpty(codigo2) && codigo2.StartsWith("C"))
                    {
                        montoTotal += Convert.ToDecimal(gridFacturas.Rows[x].Cells["ART_PVP"].Value) * Convert.ToInt32(gridFacturas.Rows[x].Cells["ART_EXIST"].Value);
                    }
                }
                List<TB_ARTICULO> articulos = _D_Articulos.ObtenerArticulos("", "A000004");

                if (articulos != null && articulos.Count > 0)
                {
                    decimal totalprima;
                    totalprima = montoTotal + montoTotalServicios;
                    TB_ARTICULO articulo = articulos.First();
                    decimal precio = totalprima *= prima;

                    decimal total = precio * 1;
                    decimal CostoPromedio = (decimal)articulo.COSTOPROME;
                    decimal impuesto = articulo.ART_EXENTO ? 0 : BuscarIva("I");


                    if (!tieneServicio)
                    {
                        AgregarFila(gridFacturas, articulo.CodArticulo, "", "", "", articulo.DESART, 1, (decimal)precio, (decimal)articulo.PORCTDESCUENTO, (decimal)total, impuesto, "", CostoPromedio, codigo);
                    }
                    else
                    {
                        ActualizarCelda(gridFacturas, filaSeleccionada, "ART_PVP", precio.ToString("#,##0.00", new CultureInfo("es-ES")));
                    }
                   
                }

            //}


        }

        private void Formato_Dgv_Promociones(DataGridView Dgv_Pnl3_Promociones)
        {
            stringBuilder.Clear();
            try
            {
                //Centrar todas las colucnas 
                Dgv_Pnl3_Promociones.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Dgv_Pnl3_Promociones.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

                // Quitar la flecha del selector de fila
                Dgv_Pnl3_Promociones.RowHeadersVisible = false;

                // Deshabilitar el redimensionamiento de filas
                Dgv_Pnl3_Promociones.AllowUserToResizeRows = false;

                //asignar Nombres a cada colucna 
                Dgv_Pnl3_Promociones.Columns["Prom_DESCRIP"].HeaderText = "       ";

                // No modificable
                Dgv_Pnl3_Promociones.Columns["Prom_DESCRIP"].ReadOnly = true;

                Dgv_Pnl3_Promociones.Columns["Prom_DESCRIP"].SortMode = DataGridViewColumnSortMode.NotSortable;

                Dgv_Pnl3_Promociones.Columns["Prom_DESCRIP"].Width = 250;

                foreach (DataGridViewColumn column in Dgv_Pnl3_Promociones.Columns)
                {
                    if (column.Name != "Prom_DESCRIP")
                    {
                        column.Visible = false;
                    }
                }

                //quitar seleccion por defecto de datagrid
                Dgv_Pnl3_Promociones.ClearSelection();

                Dgv_Pnl3_Promociones.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Selección de filas completas

                // Evitar la Selección de Encabezados de Fila
                Dgv_Pnl3_Promociones.RowHeadersVisible = false;

                //AutoGenerar Columnas:
                Dgv_Pnl3_Promociones.AutoGenerateColumns = false;

        }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
            }
        }

        public Dictionary<string, string> CrearParametrosPromociones(string parametro01 = null, string parametro02 = null, string parametro03 = null,
    string parametro04 = null, string parametro05 = null, string parametro06 = null, string parametro07 = null, string parametro08 = null, string parametro09 = null,
    string parametro10 = null, string parametro11 = null, string parametro12 = null, string parametro13 = null, string parametro14 = null, string parametro15 = null, 
    string parametro16 = null, string parametro17 = null, string parametro18 = null, string parametro19 = null, string parametro20 = null)
        {
             return new Dictionary<string, string>
             {
                    { "@PARAMETRO01", parametro01 },
                    { "@PARAMETRO02", parametro02 },
                    { "@PARAMETRO03", parametro03 },
                    { "@PARAMETRO04", parametro04 },
                    { "@PARAMETRO05", parametro05 },
                    { "@PARAMETRO06", parametro06 },
                    { "@PARAMETRO07", parametro07 },
                    { "@PARAMETRO08", parametro08 },
                    { "@PARAMETRO09", parametro09 },
                    { "@PARAMETRO10", parametro10 },
                    { "@PARAMETRO11", parametro11 },
                    { "@PARAMETRO12", parametro12 },
                    { "@PARAMETRO13", parametro13 },
                    { "@PARAMETRO14", parametro14 },
                    { "@PARAMETRO15", parametro15 },
                    { "@PARAMETRO16", parametro16 },
                    { "@PARAMETRO17", parametro17 },
                    { "@PARAMETRO18", parametro18 },
                    { "@PARAMETRO19", parametro19 },
                    { "@PARAMETRO20", parametro20 }
             };
        }

        public bool EjecutarPromociones(List<TB_ARTICULO> listaArticulos,DataGridView DgvArticulo, string TipoExamen, string Cod_DetVta, string CodPromo, bool MonturaPropia, bool CristalPropio)
        {
            stringBuilder.Clear();
            try 
            { 

            // Declaración de variables
            bool MLS = false;
            bool CRT = false;
            string AR = string.Empty;
            string Cristal = string.Empty;
            string Montura = string.Empty;
            string LC = string.Empty;
            bool PromoAplica = false;
            bool promoAplicaAR = false;
            bool PromoARObligatorio = false;

            DataTable respuesta = _D_Articulos.BucarTipoVenta(Cod_DetVta);

            string glbTipoTrabajo= respuesta.Rows[0]["CodVenta"].ToString();

            // Determinar el tipo de examen según glbTipoTrabajo
                if (glbTipoTrabajo == "002" && Cod_DetVta!= "02")
                {
                TipoExamen = "CONVENCIONAL";
                }
                else if (glbTipoTrabajo == "002" && Cod_DetVta == "02")
                {
                    TipoExamen = "CONTACTO";
                }
                else if (glbTipoTrabajo == "001")
                {
                TipoExamen = "DIRECTA";
                }
                else if (glbTipoTrabajo == "003" && Cod_DetVta == "05")
                {
                 TipoExamen = "REPARACION";
                }

            // Recorrer las filas del DataGridView
            foreach (DataGridViewRow row in DgvArticulo.Rows)
            {
                if (row.Cells["CodArticulo"].Value != null)
                {
                    string codigo = row.Cells["CodArticulo"].Value.ToString();

                    // Verificar si es Montura o Lente de Contacto
                    if (codigo.StartsWith("M") || codigo.StartsWith("L"))
                    {
                        MLS = true;
                        Montura = codigo;
                    }
                    // Verificar si es Cristal
                    else if (codigo.StartsWith("C"))
                    {
                        CRT = true;
                        Cristal = codigo;
                    }
                    else if (codigo.StartsWith("W"))
                    {
                         LC= codigo;
                    }
                        // Verificar si es Servicio
                    else if (codigo.StartsWith("S"))
                    {
                        string Serv = codigo;

                        // Obtener los servicios AR desde la base de datos
                        DataSet dsServAR = _D_Articulos.ServiciosAR_btnProcesar("", false);

                        if (dsServAR.Tables.Count > 1)
                        {
                            foreach (DataRow dr in dsServAR.Tables[1].Rows)
                            {
                                if (codigo == dr["CodServicio"].ToString())
                                    //    &&
                                    //row.Cells["PromoEvaluada"].Value != null &&
                                    //row.Cells["PromoEvaluada"].Value.ToString() == "No")
                                {
                                    AR = codigo;
                                    promoAplicaAR = true;
                                }
                            }
                        }
                    }
                }
            }

            // Crear los parámetros para la función AplicarPromociones
            Dictionary<string, string> parametros = CrearParametrosPromociones(
                Montura,
                Cristal,
                LC,
                MonturaPropia ? "true" : PromoARObligatorio.ToString(), 
                CristalPropio ? "true" : PromoARObligatorio.ToString(), 
                AR,
                promoAplicaAR ? "true" : PromoARObligatorio.ToString(),
                glbTipoTrabajo,
                TipoExamen,
                _D_Inicio.DiaActivo().ToString("yyyy/MM/dd")
            );

            // Llamar a la función AplicarPromociones
            DataSet resultado = _D_Articulos.AplicarPromociones(CodPromo,parametros);

            // Verificar si la promoción aplica
            if (resultado.Tables.Count > 0 && resultado.Tables[0].Rows.Count > 0)
            {
                if (resultado.Tables[0].Rows[0]["Resultado"].ToString() == "APLICA" || resultado.Tables[0].Rows[0]["Resultado"].ToString() == "CASADA")
                {
                    PromoAplica= AplicarPromocionesEnGrid(listaArticulos, DgvArticulo, resultado, glbTipoTrabajo, TipoExamen);
                }
                else
                    PromoAplica= false;
            }

            return PromoAplica;
            }
             catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return false;
            }
        }
        public TB_ARTICULO ObtenerArticuloPorCodigo(List<TB_ARTICULO> listaArticulos, string codArticulo, DataGridView gridFacturas)
        {
            // Recorrer la lista para buscar el artículo por su código
            foreach (var articulo in listaArticulos)
            {
                if (articulo.CodArticulo != null && articulo.CodArticulo.Equals(codArticulo, StringComparison.OrdinalIgnoreCase))
                {
                    // Retornar el artículo completo si se encuentra
                    return articulo;
                }
            }



            // si No consigue el articulo buscamos el articulo en la base de datos de forma individual 
            // Agregar un nuevo servicio o prima
            List<TB_ARTICULO> articulos = _D_Articulos.ObtenerArticulos("", codArticulo);

            if (articulos != null && articulos.Count > 0)
            {
                foreach (var articulo in articulos)
                {
                    if (articulo.CodArticulo != null && articulo.CodArticulo.Equals(codArticulo, StringComparison.OrdinalIgnoreCase))
                    {
                        // Si es A000004, calcula el precio y asígnalo
                        if (articulo.CodArticulo == "A000004")
                        {
                            // Calcula el monto total de los artículos C*
                            decimal montoTotal = 0;
                            for (int x = 0; x < gridFacturas.RowCount; x++)
                            {
                                string codigo2 = gridFacturas.Rows[x].Cells["CodArticulo"].Value?.ToString();
                                if (!string.IsNullOrEmpty(codigo2) && codigo2.StartsWith("C"))
                                {
                                    montoTotal += Convert.ToDecimal(gridFacturas.Rows[x].Cells["PrecioViejo"].Value);
                                }
                            }
                            decimal prima = Convert.ToDecimal(_D_DetalleOrden.TB_PARAMETROSPGE("PorcPrimaPGE")) / 100;
                            decimal precio = montoTotal * prima;

                            // Asigna el precio calculado al artículo
                            articulo.ART_PVP = precio;
                        }
                        // Retorna el artículo con el precio actualizado
                        return articulo;
                    }
                }
            }

            // Retornar null si no se encuentra el artículo
            return null;
        }
        public bool AplicarPromocionesEnGrid(List<TB_ARTICULO> listaArticulos, DataGridView DgvArticulo, DataSet dsLl1so, string glbTipoTrabajo, string TipoExamen)
        {
            bool PromoAplicada = false;

            // Obtener la lista de códigos de artículos que no deben recibir promoción (si existe la segunda tabla)
            HashSet<string> codigosSinPromocion = new HashSet<string>();
            if (dsLl1so.Tables.Count > 1 && dsLl1so.Tables[1].Columns.Contains("CodArticulo"))
            {
                foreach (DataRow row in dsLl1so.Tables[1].Rows)
                {
                    if (row["CodArticulo"] != null)
                    {
                        codigosSinPromocion.Add(row["CodArticulo"].ToString());
                    }
                }
            }

            // Recorrer las filas del DataGridView
            foreach (DataGridViewRow row in DgvArticulo.Rows)
            {
                if (row.Cells["CodArticulo"].Value != null)
                {
                    string codigo = row.Cells["CodArticulo"].Value.ToString();

                    // Verificar si el resultado de la promoción es "APLICA"
                    if (dsLl1so.Tables[0].Rows[0]["Resultado"].ToString() == "APLICA")
                    {
                        // Si el código comienza con "C" (Cristal)
                        if (codigo.StartsWith("C"))
                        {
                            row.Cells["TienePromo"].Value = "Si";
                            row.Cells["ART_PVP"].Value = (decimal)dsLl1so.Tables[0].Rows[0]["PRECIOCRT_DESC"];
                            row.Cells["Total"].Value = (decimal)dsLl1so.Tables[0].Rows[0]["PRECIOCRT_DESC"] * Convert.ToDecimal(row.Cells["ART_EXIST"].Value);
                            row.Cells["CodPromo"].Value = dsLl1so.Tables[0].Rows[0]["CODPROM"];
                            row.Cells["PromoEvaluada"].Value = "Si";
                            PromoAplicada = true;
                        }
                        // Si el código comienza con "M" (Montura) o "L" (Lente de contacto)
                        else if (codigo.StartsWith("M") || codigo.StartsWith("L"))
                        {
                            row.Cells["TienePromo"].Value = "Si";
                            row.Cells["ART_PVP"].Value = (decimal)(dsLl1so.Tables[0].Rows[0]["PRECIOMONT_DESC"]);
                            row.Cells["Total"].Value = (decimal)((decimal)dsLl1so.Tables[0].Rows[0]["PRECIOMONT_DESC"] * Convert.ToDecimal(row.Cells["ART_EXIST"].Value));
                            row.Cells["CodPromo"].Value = dsLl1so.Tables[0].Rows[0]["CODPROM"];
                            row.Cells["PromoEvaluada"].Value = "Si";
                            PromoAplicada = true;
                        }

                        else if (codigo.StartsWith("W"))
                        {
                            row.Cells["TienePromo"].Value = "Si";
                            row.Cells["ART_PVP"].Value = (decimal)(dsLl1so.Tables[0].Rows[0]["TOTLC"]);
                            row.Cells["Total"].Value = (decimal)((decimal)dsLl1so.Tables[0].Rows[0]["TOTLC"] * Convert.ToDecimal(row.Cells["ART_EXIST"].Value));
                            row.Cells["CodPromo"].Value = dsLl1so.Tables[0].Rows[0]["CODPROM"];
                            row.Cells["PromoEvaluada"].Value = "Si";
                            PromoAplicada = true;
                        }

                        // Verificar si el artículo está en la lista negra (códigos de articulos sin promoción)
                        else if (codigosSinPromocion.Contains(codigo))
                        {
                            //TB_ARTICULO articuloEncontrado = ObtenerArticuloPorCodigo(listaArticulos, codigo);
                            //decimal montoArtic = articuloEncontrado.ART_PVP;
                            row.Cells["TienePromo"].Value = "Si";
                            //row.Cells["ART_PVP"].Value = (decimal)(montoArtic);
                            //row.Cells["Total"].Value = (decimal)(montoArtic * Convert.ToDecimal(row.Cells["Can"].Value));
                            row.Cells["CodPromo"].Value = dsLl1so.Tables[0].Rows[0]["CODPROM"];
                            row.Cells["PromoEvaluada"].Value = "Si";
                            PromoAplicada = true;
                        }


                        // Verificar si Es un Servicio
                        else if (codigo.StartsWith("S"))
                        {
                            DataSet dsServAR = _D_Articulos.ServiciosAR_btnProcesar("", false);

                            if (dsServAR.Tables.Count > 1)
                            {     
                                foreach (DataRow dr in dsServAR.Tables[1].Rows)
                                // Verificar si el artículo es un AR 
                                {      // preguntamos si el codigo del articulo es el mismo que el que devuelve el dsServAR 
                                       // preguntamos si existe el campo PRECIOAR_DESC  antes de accede a su valor 
                                       // Preguntamos si esa colunma no esta vacia 
                                    if (codigo == dr["CodServicio"].ToString() && dsLl1so.Tables[0].Columns.Contains("PRECIOAR_DESC") && !string.IsNullOrEmpty(dsLl1so.Tables[0].Rows[0]["PRECIOAR_DESC"].ToString()))
                                    {
                                        row.Cells["TienePromo"].Value = "Si";
                                        row.Cells["ART_PVP"].Value = (decimal)(dsLl1so.Tables[0].Rows[0]["PRECIOAR_DESC"]);
                                        row.Cells["Total"].Value = (decimal)((decimal)dsLl1so.Tables[0].Rows[0]["PRECIOAR_DESC"] * Convert.ToDecimal(row.Cells["ART_EXIST"].Value));
                                        row.Cells["CodPromo"].Value = dsLl1so.Tables[0].Rows[0]["CODPROM"];
                                        row.Cells["PromoEvaluada"].Value = "Si";
                                    }
                                    // Verificar si el otro servicio diferente al AR 
                                    // preguntamos si existe el campo PORCDCTO  antes de accede a su valor 
                                    else if (dsLl1so.Tables[0].Columns.Contains("PORC_SERVICIO_DESC") && !string.IsNullOrEmpty(dsLl1so.Tables[0].Rows[0]["PORC_SERVICIO_DESC"].ToString()))
                                    {
                                        TB_ARTICULO articuloEncontrado = ObtenerArticuloPorCodigo(listaArticulos, codigo, DgvArticulo);
                                        decimal montoArtic = articuloEncontrado.ART_PVP; // Método para obtener el precio del artículo
                                        row.Cells["TienePromo"].Value = "Si";
                                        row.Cells["ART_PVP"].Value = (decimal)(montoArtic - (montoArtic * Convert.ToDecimal(dsLl1so.Tables[0].Rows[0]["PORC_SERVICIO_DESC"]) / 100));
                                        row.Cells["Total"].Value = (decimal)((montoArtic - (montoArtic * Convert.ToDecimal(dsLl1so.Tables[0].Rows[0]["PORC_SERVICIO_DESC"]) / 100)) * Convert.ToDecimal(row.Cells["ART_EXIST"].Value));
                                        row.Cells["CodPromo"].Value = dsLl1so.Tables[0].Rows[0]["CODPROM"];
                                        row.Cells["PromoEvaluada"].Value = "Si";
                                        PromoAplicada = true;
                                    }
                                    else
                                    {
                                        row.Cells["TienePromo"].Value = "Si";
                                        row.Cells["CodPromo"].Value = dsLl1so.Tables[0].Rows[0]["CODPROM"];
                                        row.Cells["PromoEvaluada"].Value = "Si";
                                        PromoAplicada = true;
                                    }
                                }
                            }
                           
                        } 

                        // Si el código es un articulo diferente de S,M,L,W
                        else 
                        {    // Pregunto si Exte esta colunma para dar descuento 
                            if (dsLl1so.Tables[0].Columns.Contains("PORCDCTO") && !string.IsNullOrEmpty(dsLl1so.Tables[0].Rows[0]["PORCDCTO"].ToString()))
                            {
                            TB_ARTICULO articuloEncontrado = ObtenerArticuloPorCodigo(listaArticulos, codigo, DgvArticulo);
                            decimal montoArtic = articuloEncontrado.ART_PVP; // Método para obtener el precio del artículo
                            row.Cells["TienePromo"].Value = "Si";
                            row.Cells["ART_PVP"].Value = (decimal)(montoArtic - (montoArtic * Convert.ToDecimal(dsLl1so.Tables[0].Rows[0]["PORCDCTO"]) / 100));
                            row.Cells["Total"].Value = (decimal)((montoArtic - (montoArtic * Convert.ToDecimal(dsLl1so.Tables[0].Rows[0]["PORCDCTO"]) / 100)) * Convert.ToDecimal(row.Cells["ART_EXIST"].Value));
                            row.Cells["CodPromo"].Value = dsLl1so.Tables[0].Rows[0]["CODPROM"];
                            row.Cells["PromoEvaluada"].Value = "Si";
                            PromoAplicada = true;
                            }
                            else
                            {
                                row.Cells["TienePromo"].Value = "Si";
                                row.Cells["CodPromo"].Value = dsLl1so.Tables[0].Rows[0]["CODPROM"];
                                row.Cells["PromoEvaluada"].Value = "Si";
                                PromoAplicada = true;
                            }
                        }

                    }
                    // Si el resultado de la promoción es "NO APLICA"
                    else if (dsLl1so.Tables[0].Rows[0]["Resultado"].ToString() == "NO APLICA")
                    {
                        PromoAplicada = false;
                    }
                    // Si el resultado de la promoción es "NO APLICA"
                    else if (dsLl1so.Tables[0].Rows[0]["Resultado"].ToString() == "CASADA")
                    {
                        row.Cells["TienePromo"].Value = "Si";
                        row.Cells["CodPromo"].Value = dsLl1so.Tables[0].Rows[0]["CODPROM"];
                        row.Cells["PromoEvaluada"].Value = "Si";
                        PromoAplicada = true;
                    }
                }
            }

            return PromoAplicada;
        }

        public DataSet CargarColoresLC(System.Windows.Forms.DataGridView DgvColoresLC, string CodArticulo)
        {
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
               
                return _D_Articulos.ObtenerColorLC(CodArticulo);
            }
            catch (Exception ex)
            {
                return null;
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                transaction.Rollback();
            }
        }

        public DataTable  ObtenerCliente( string nacio, string cedula)
        {
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

                return _D_Articulos.ObtenerCliente(nacio, cedula);
            }
            catch (Exception ex)
            {
                return null;
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                transaction.Rollback();
            }
        }

        //public async Task<string> AgregarOrdenServicio(string codSucursal, string revision, string codVenta, string cteNacio, string cteCedIden,
        //                                    string numExamen, string codEmpleado, string codLaboratorio, string codServicio,
        //                                    string vision, DateTime fecOfrecido, string horOfrecido, DateTime? fecEntrega, DateTime? fecEnvio,
        //                                    decimal vtaSubTotal, decimal vtaImpuesto, decimal vtaDescuento, decimal vtaTotal,
        //                                    bool orSerFinan, string orSerStatus, string orSerObserv, string userCrea, DateTime fecha,
        //                                    bool monturaPropia, string codDetVta, bool aplica, string otCorrespondiente, bool ventaAfil,
        //                                    bool cristalPropio, string tipoMonturaPropia, string codMotivoReposicion,
        //                                    string cedulaCteAfil, string codigoEmpAfil, bool? asegurada, bool? exonerada,
        //                                    bool monturaEnQuorum, string codColoracion, SqlCommand command)
        //{
        //    try
        //    {
        //        return await _D_Articulos.AgregarOrdenServicio(
        //            codSucursal, revision, codVenta, cteNacio, cteCedIden, numExamen,
        //            codEmpleado, codLaboratorio, codServicio, vision, fecOfrecido, horOfrecido,
        //            fecEntrega, fecEnvio, vtaSubTotal, vtaImpuesto, vtaDescuento, vtaTotal,
        //            orSerFinan, orSerStatus, orSerObserv, userCrea, fecha, monturaPropia,
        //            codDetVta, aplica, otCorrespondiente, ventaAfil, cristalPropio,
        //            tipoMonturaPropia, codMotivoReposicion, cedulaCteAfil, codigoEmpAfil,
        //            asegurada, exonerada, monturaEnQuorum, codColoracion, command
        //        );
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error al agregar la orden de servicio", ex);
        //    }
        //}

        public async Task<string> AgregarOrdenServicio(AgregarOrdenServicio_CargarOrdenes datos, SqlCommand command)
        {
            try
            {
                return await _D_Articulos.AgregarOrdenServicio(datos, command);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar la orden de servicio", ex);
            }
        }

        public void DividirValoresCedula(string cedula, out string letraInicial, out string numeroCedula)
        {
            // Validar que la cédula no sea nula o vacía  
            if (string.IsNullOrWhiteSpace(cedula))
            {
                letraInicial = string.Empty;
                numeroCedula = string.Empty;
                return;
            }

            // Dividir la cédula en la letra inicial y el número  
            var partes = cedula.Split('-');
            if (partes.Length == 2)
            {
                letraInicial = partes[0];
                numeroCedula = partes[1];
            }
            else
            {
                letraInicial = string.Empty;
                numeroCedula = string.Empty;
            }
        }

        //public DataSet  ObtenerLaboratoriosParaCombo()
        //{
        //    return _D_Articulos.DatosLaboratorio();
        //}

        public void ComboLaboratorio(System.Windows.Forms.ComboBox Cbx_Pnl2_Trbajo, System.Windows.Forms.ComboBox Cbx_Pnl2_Laboratorio, string sucursal)
        {
            try
            {
                //var oSucursal = new CapaNegocio.ConfiguraSucursal();
                //var oLaboratorio = new CapaNegocio.Laboratorio();
                DataSet dsLaboratorio = _D_Articulos.DatosLaboratorio(sucursal);
                //oLaboratorio.ObtenerSucursalLaboratorio(oSucursal.CodSucur);

                //Cbx_Pnl2_Laboratorio.Items.Clear();

                // Crear un DataTable
                DataTable dt = new DataTable();

                // Definir columnas
                dt.Columns.Add("CODIGO_LAB", typeof(string));
                dt.Columns.Add("DESCRIPCION", typeof(string));

                //dt.Rows.Add("", "");


                // Si es una reposición de garantía, solo mostrar laboratorio Quorum
                if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "09")
                {
                    if (_D_DetalleOrden.TB_PARAMETROSPGE("LabQuorum") == "1")
                    {
                        foreach (DataRow dr in dsLaboratorio.Tables[0].Rows)
                        {
                            if (dr["CODIGO_LAB"].ToString() == "QUO")
                            {
                                DataRow fila = dt.NewRow();
                                dt.Rows.Add(dr[0].ToString(), dr[1].ToString());
                                //Cbx_Pnl2_Laboratorio.Items.Add(dr[1].ToString());
                            }
                        }
                    }
                    else
                    {
                        foreach (DataRow dr in dsLaboratorio.Tables[0].Rows)
                        {
                            DataRow fila = dt.NewRow();
                            dt.Rows.Add(dr[0].ToString(), dr[1].ToString());
                        }
                    }
                }
                else if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "05")
                {
                    if (_D_DetalleOrden.TB_PARAMETRO("RepLabBoleita") == "1")
                    {
                        foreach (DataRow dr in dsLaboratorio.Tables[0].Rows)
                        {
                            if (dr[0].ToString() == "BOL")
                            {
                                DataRow fila = dt.NewRow();
                                dt.Rows.Add(dr[0].ToString(), dr[1].ToString());
                            }
                        }
                    }
                    else
                    {
                        foreach (DataRow dr in dsLaboratorio.Tables[0].Rows)
                        {
                            DataRow fila = dt.NewRow();
                            dt.Rows.Add(dr[0].ToString(), dr[1].ToString());
                        }
                    }
                }
                else if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "02")
                {
                    foreach (DataRow dr in dsLaboratorio.Tables[0].Rows)
                    {
                        if (dr["CODIGO_LAB"].ToString() == "QUO")
                        {
                            DataRow fila = dt.NewRow();
                            dt.Rows.Add(dr[0].ToString(), dr[1].ToString());
                        }
                    }
                }
                else
                {
                    foreach (DataRow dr in dsLaboratorio.Tables[0].Rows)
                    {
                        DataRow fila = dt.NewRow();
                        dt.Rows.Add(dr[0].ToString(), dr[1].ToString());
                    }
                }

                Cbx_Pnl2_Laboratorio.DataSource = dt;

                Cbx_Pnl2_Laboratorio.ValueMember = "CODIGO_LAB";
                Cbx_Pnl2_Laboratorio.DisplayMember = "DESCRIPCION";
            }
            catch (Exception ex)
            {
                //MensajeError.MuestroMensaje("Error en la función", "frmParametrosVentas.ComboLaboratorio",
                //    "Por favor comunicarse con el Dpto de Sistemas y reportar el siguiente error: ", ex.Message,
                //    CapaNegocio.MensajesGenerales.TiposIconos.IconoError, glbUsuarioActual);
                //MensajeError.ShowDialog();
            }
        }
        public void ObtenerServicioLaboratorioCbx(System.Windows.Forms.ComboBox Cbx_Pnl2_Servicio, System.Windows.Forms.ComboBox Cbx_Pnl2_Trbajo, System.Windows.Forms.ComboBox Cbx_Pnl2_Laboratorio, string sucursal, string descripcionLaboratorio)
        {
            //Cbx_Pnl2_Servicio.Items.Clear();
            //return _D_Articulos.ServiciosLaboratorio();
            DataSet dsLaboratorioNew = new DataSet();

            DataTable dt = new DataTable();

            // Definir columnas
            dt.Columns.Add("Cod_servicio", typeof(string));
            dt.Columns.Add("Descripcion_servicio", typeof(string));

            DataSet dsLaboratorio = _D_Articulos.ObtenerLaboratorioServicio(sucursal, descripcionLaboratorio);
            foreach (DataRow dr in dsLaboratorio.Tables[0].Rows)
            {
                if ((Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "01" || Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "08") && Cbx_Pnl2_Laboratorio.SelectedValue.ToString() == "QUO")
                {
                    if (dr[1].ToString() == "SERVICIO QUORUM" || dr[0].ToString() == "004" || dr[0].ToString() == "005" || dr[0].ToString() == "007" || dr[0].ToString() == "018")
                    {
                        dt.Rows.Add(dr[0].ToString(), dr[1].ToString());
                    }
                }
                else if (Cbx_Pnl2_Trbajo.SelectedValue.ToString() == "02" && Cbx_Pnl2_Laboratorio.SelectedValue.ToString() == "QUO")
                {
                    if (dr[1].ToString() == "SERVICIO QUORUM" || dr[0].ToString() == "004" || dr[0].ToString() == "005" || dr[0].ToString() == "007" || dr[0].ToString() == "018")
                    {
                        dt.Rows.Add(dr[0].ToString(), dr[1].ToString());
                    }
                }
                else
                {
                    dt.Rows.Add(dr[0].ToString(), dr[1].ToString());
                }
            }

            Cbx_Pnl2_Servicio.DataSource = dt;
            Cbx_Pnl2_Servicio.ValueMember = "Cod_servicio";
            Cbx_Pnl2_Servicio.DisplayMember = "Descripcion_servicio";
        }

        public List<string> ObtenerMonturasEnQuorum(DataGridView dgvArticulos, string sucursal, string codServicio)
        {
            List<string> monturasEnQuorum = new List<string>();

            foreach (DataGridViewRow row in dgvArticulos.Rows)
            {
                string codArticulo = row.Cells["CodArticulo"].Value?.ToString();

                if (!string.IsNullOrWhiteSpace(codArticulo))
                {
                    var dt = _D_Articulos.ValidacionMonturaQuorum(codArticulo, sucursal, codServicio);
                    if (dt != null && dt.Tables[1].Rows.Count > 0)
                    {
                        monturasEnQuorum.Add(codArticulo);
                    }
                }
            }

            return monturasEnQuorum;
        }

        public string ObtenerMonturaQuorumPorArticulo(string codArticulo, string sucursal, string codServicio)
        {
            var dt = _D_Articulos.ValidacionMonturaQuorum(codArticulo, sucursal, codServicio);
            if (dt.Tables[1].Rows.Count > 0)
            {
                return codArticulo;
            }
            return null;
        }

        public string ObtenerBajaExistenciaCristales(string cedNacio, string cedId, string sucursal, string examen, string codArticulo, SqlCommand command = null)
        {
            var dt = _D_Articulos.ObtenerBajaExistenciaCristales(cedNacio, cedId, sucursal, examen, codArticulo, command = null);
            if (dt != null && dt.Tables[1].Rows.Count > 0)
            {
                return codArticulo;
            }
            return null;

        }

        public string ObtenerTipoVentaPorModo(string codModo)
        {
            return _D_Articulos.ObtenerCodVentaDesdeCodModo(codModo);
        }

        public string CalcularOjoDesdeGrid(DataGridView dgvArticulos)
        {
            bool tieneOjoDerecho = false;
            bool tieneOjoIzquierdo = false;

            foreach (DataGridViewRow row in dgvArticulos.Rows)
            {
                if (row.IsNewRow) continue;

                string ojo = row.Cells["Ojo"].Value?.ToString()?.Trim();

                if (ojo == "D")
                    tieneOjoDerecho = true;
                else if (ojo == "I")
                    tieneOjoIzquierdo = true;
                else if (ojo == "A")
                    return "A"; // Ambos
            }

            // Determinar el resultado final
            if (tieneOjoDerecho && tieneOjoIzquierdo)
                return "A"; // Ambos 
            else if (tieneOjoDerecho)
                return "D"; // Solo derecho
            else if (tieneOjoIzquierdo)
                return "I"; // Solo izquierdo
            else
                return "";  // No se especificó ojo
        }

        public (bool, string) DetectarProductoRepetido(DataGridView dgvArticulos)
        {
            for (int i = 0; i < dgvArticulos.Rows.Count - 1; i++)
            {
                if (dgvArticulos.Rows[i].Cells["CodArticulo"].Value?.ToString() == dgvArticulos.Rows[i + 1].Cells["CodArticulo"].Value?.ToString())
                    return (true, dgvArticulos.Rows[i].Cells["CodArticulo"].Value.ToString());
            }
            return (false, null);
        }

        public decimal ObtenerPorcentajeDescuento(DataGridViewRow dgvArticulos, bool empresaAfiliada)
        {
            decimal descuento = 0;

            string codArticulo = dgvArticulos.Cells["CodArticulo"].Value?.ToString()?.Trim();


            //Se guarda el descuento que posee por ser cliente afiliado
            if (dgvArticulos.Cells["PORCTDESCUENTO"].Value == null || Convert.ToDecimal(dgvArticulos.Cells["PORCTDESCUENTO"].Value) == 0)
            {

                //Verifico que porcentaje de descuento es que le corresponde guardar a esta orden según el tipo de cliente
                if (empresaAfiliada && codArticulo != "A000004" && codArticulo != "A000003") //Cuando es cliente afiliado
                {
                    //Se guarda el descuento que posee por ser cliente afiliado
                    descuento = Convert.ToDecimal(dgvArticulos.Cells["PORCTDESCUENTO"].Value);
                }
                else
                {
                    //Si al producto se le hizo un descuento adicional, este nuevo descuento "Sustituye" al descuento 'que el cliente tiene por ser afiliado.
                    descuento = Convert.ToDecimal(dgvArticulos.Cells["PORCTDESCUENTO"].Value); // Usa el descuento especial
                }

            }

            return descuento;

        }

        public int CalcularCantidadTotalRepetida(DataGridView dgvArticulos, string codigoProductoRepetido)
        {
            int cantidadTotal = 0;

            foreach (DataGridViewRow row in dgvArticulos.Rows)
            {
                if (row.IsNewRow) continue;

                string codArticulo = row.Cells["CodArticulo"].Value?.ToString()?.Trim();

                if (!string.IsNullOrEmpty(codArticulo) && codArticulo.Equals(codigoProductoRepetido, StringComparison.OrdinalIgnoreCase))
                {
                    if (int.TryParse(row.Cells["ART_EXIST"].Value?.ToString(), out int cantidad))
                    {
                        cantidadTotal += cantidad;
                    }
                }
            }

            return cantidadTotal;
        }

        public Task<bool> GuardarDescripcionDetalleOrdenServicioAsync(string numeroOrdenServicio, string numeroRevision,
                                                                      string codVenta, string codigoArticulo, int cantidad,
                                                                      string ojo, decimal precio, decimal porcentajeImpuesto,
                                                                      decimal porcentajeDescuento, decimal precioAnterior,
                                                                      string codPromocion, decimal costoArticulo, 
                                                                      string sucursalActual, SqlCommand command)
        {
            
            
            return _D_Articulos.GuardarDescripcionDetalleOrdenServicio(numeroOrdenServicio, numeroRevision, codVenta, 
                codigoArticulo, cantidad, ojo, precio, porcentajeImpuesto, porcentajeDescuento,precioAnterior, codPromocion, 
                costoArticulo,sucursalActual, command);
        }

        public Task<bool> ModificarTrabajo(string NUMOS, string HORIZ, string VERT, string MAX, string PTE, string DISVERT,
            string ANPANT, string ANFAC, string ALTD, string ALTI, string OJO, string TVISD, string TVISI, string USER, string SUC, SqlCommand command)
        {
            return _D_Articulos.ModificarTrabajoAsync(NUMOS, HORIZ, VERT, MAX, PTE, DISVERT, ANPANT, ANFAC, ALTD, ALTI, OJO, TVISD, TVISI, USER, SUC, command);
        }

        //public Task<bool> RebajarInventario(string codArticulo, string codLaboratorio, int cantidad, SqlCommand command)
        //{
        //    return _D_Articulos.RebajarInventarioAsync(codArticulo, codLaboratorio, cantidad, command);
        //}
        public void CargarServicioExpress(DataGridView gridFacturas, string Codigo_Servicio_Agregar)
        {
            // Agregar un nuevo servicio o prima
            List<TB_ARTICULO> articulos = _D_Articulos.ObtenerArticulos("", Codigo_Servicio_Agregar);
            string codigo = "";
            string codigo_cristal = "";
            bool tieneServicioExpress = false;

            if (articulos != null && articulos.Count > 0)
            {
                for (int x = 0; x < gridFacturas.RowCount; x++)
                {
                    codigo = gridFacturas.Rows[x].Cells["CodArticulo"].Value?.ToString();

                    if (codigo == Codigo_Servicio_Agregar)
                    {
                        tieneServicioExpress = true;
                    }

                    if (codigo.StartsWith("C"))
                    {
                        codigo_cristal = codigo;
                    }

                }
                TB_ARTICULO articulo = articulos.First();
                decimal precio = articulo.ART_PVP;
                decimal total = precio * 1;
                decimal CostoPromedio = (decimal)articulo.COSTOPROME;
                decimal impuesto = articulo.ART_EXENTO ? 0 : BuscarIva("I");

                if (!tieneServicioExpress)
                {
                    AgregarFila(gridFacturas, articulo.CodArticulo, "", "", "", articulo.DESART, 1, (decimal)precio, (decimal)articulo.PORCTDESCUENTO, (decimal)total, impuesto, "", CostoPromedio, codigo_cristal);
                }
            }

        }

        public List<FechaHoraOfrecida> ObtenerFechaHoraOfrecida(string servicio, string GlbCodDetVta)
        {
            try
            {
                DateTime fechaMaxVenta;

                // Obtener la fecha actual (solo la parte de la fecha, sin la hora)
                DateTime fechaActual = DateTime.Now.Date;

                // Obtener el valor del parámetro "HoraMaxVtaF12"
                string horaMaxVtaF12 = _D_DetalleOrden.TB_PARAMETRO("HoraMaxVtaF12");

                // Validar que el parámetro no sea nulo o vacío
                if (string.IsNullOrWhiteSpace(horaMaxVtaF12))
                {
                    throw new Exception("El parámetro 'HoraMaxVtaF12' no tiene un valor válido.");
                }

                // Limpiar el formato de la hora eliminando "a.m." o "p.m."
                horaMaxVtaF12 = horaMaxVtaF12.Replace("a.m.", "").Replace("p.m.", "").Trim();

                // Concatenar la fecha actual con la hora obtenida
                string fechaConcatenada = $"{fechaActual:yyyy/MM/dd} {horaMaxVtaF12}";

                // Intentar convertir la cadena concatenada a un objeto DateTime
                if (!DateTime.TryParse(fechaConcatenada, out fechaMaxVenta))
                {
                    throw new Exception($"No se pudo convertir la fecha y hora concatenada: {fechaConcatenada}");
                }

                // Convertir la cadena concatenada a un objeto DateTime
                //fechaMaxVenta = DateTime.ParseExact(fechaConcatenada, "yyyy/MM/dd H:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                // Variables iniciales
                int horasServicio= 0;

                List<TB_SERVICIOSLABDTO> TB_SERVICIOSLABD = new List<TB_SERVICIOSLABDTO>();
                TB_SERVICIOSLABD = _D_Articulos.ServiciosLaboratorio(servicio);
                if (TB_SERVICIOSLABD != null && TB_SERVICIOSLABD.Count > 0)
                {
                    TB_SERVICIOSLABDTO _SERVICIOSLABDTO = TB_SERVICIOSLABD.First();
                    horasServicio = int.Parse(_SERVICIOSLABDTO.HorasEntrega);
                }

                DateTime fechaOfrecida = DateTime.Now;
                //fechaOfrecida = _D_Inicio.DiaActivo();
                string horaOfrecida = string.Empty;

                // Lógica para HorasServicio SERVICIO EXPRESS
                if ((servicio == "018" || servicio == "005" || servicio == "019") && (GlbCodDetVta == "01" || GlbCodDetVta == "02" || GlbCodDetVta == "08"))
                {
                    FechaHoraOfrecida resultado = null;
                    if (servicio == "018")
                     resultado = Calculo_Servicio_3Horas(_D_Inicio.DiaActivo());
                    if(servicio == "005")
                    resultado = Calculo_Servicio_3Horas(DateTime.Now.Date);
                    if (servicio== "019")
                    resultado = Calculo_Servicio_1Horas(DateTime.Now.Date);
                    return new List<FechaHoraOfrecida> { resultado };
                }

                // Lógica para HorasServicio SERVICIO ENTREGA 3 HORAS
                else if (servicio == "004" && (GlbCodDetVta == "01" || GlbCodDetVta == "02" || GlbCodDetVta == "08"))
                {
                    FechaHoraOfrecida resultado = Calculo_Servicio_12Horas(_D_Inicio.DiaActivo());
                    return new List<FechaHoraOfrecida> { resultado };
                }

                // Lógica para HorasServicio SERVICIO ENTREGA 3 HORAS
                else if (servicio == "017" && (GlbCodDetVta == "01" || GlbCodDetVta == "02" || GlbCodDetVta == "08"))
                {
                    // Retornar el resultado como una lista
                    return new List<FechaHoraOfrecida>
                    {
                    new FechaHoraOfrecida
                    {
                        FechaOfrecida = fechaOfrecida,
                        HoraOfrecida = DateTime.Now.ToString("HH:mm:ss: tt")
                    }
                    };
                }

                // Lógica para HorasServicio = 12 y Servicio no es '004' ni '018'
                else if (horasServicio == 12 && servicio != "004" && servicio != "018")
                {
                    int diasEntrega = int.Parse(_D_DetalleOrden.TB_PARAMETRO("DiasEntregaF12"));
                    string horaEntregaF12 = _D_DetalleOrden.TB_PARAMETRO("HoraEntregaF12");
                    string hora2EntregaF12 = _D_DetalleOrden.TB_PARAMETRO("Hora2EntregaF12");
                    string horaMaxVentaF12 = _D_DetalleOrden.TB_PARAMETRO("HoraMaxVtaF12");

                    if (DateTime.Now < fechaMaxVenta)
                    {
                        // Hora actual menor a hora máxima de venta
                        fechaOfrecida = DateTime.Now.AddDays(1);

                        // Si la fecha ofrecida cae en domingo, sumar días de entrega
                        if (fechaOfrecida.DayOfWeek == DayOfWeek.Sunday)
                        {
                            fechaOfrecida = fechaOfrecida.AddDays(diasEntrega);
                        }

                        horaOfrecida = horaEntregaF12;
                    }
                    else
                    {
                        // Hora actual mayor a hora máxima de venta
                        fechaOfrecida = DateTime.Now.AddDays(diasEntrega);

                        // Si la fecha ofrecida cae en lunes, sumar días de entrega
                        if (fechaOfrecida.DayOfWeek == DayOfWeek.Monday)
                        {
                            fechaOfrecida = fechaOfrecida.AddDays(diasEntrega);
                        }

                        horaOfrecida = hora2EntregaF12;
                    }
                }
                // Lógica para HorasServicio > 12 y Servicio no es '004' ni '018'
                else if (horasServicio > 12 && servicio != "004" && servicio != "018" && servicio != "019")
                {
                    int diasAddEntrega = int.Parse(_D_DetalleOrden.TB_PARAMETRO("DiasAddEntrega"));
                    string horaMaxVentaServ =  _D_DetalleOrden.TB_PARAMETRO("HoraMaxVtaServ");
                    string hora2MaxVentaServ =  _D_DetalleOrden.TB_PARAMETRO("Hora2MaxVtaServ");

                    // Si es día de semana (Lunes a Viernes)
                    if (DateTime.Now.DayOfWeek > DayOfWeek.Sunday && DateTime.Now.DayOfWeek < DayOfWeek.Saturday)
                    {
                        DateTime horaMaxVenta = DateTime.Parse($"{DateTime.Now:yyyy-MM-dd} {horaMaxVentaServ}");

                        if (DateTime.Now > horaMaxVenta)
                        {
                            fechaOfrecida = fechaOfrecida.AddDays(diasAddEntrega);
                        }
                    }
                    // Si es fin de semana (Domingo o Sábado)
                    else if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday || DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
                    {
                        DateTime hora2MaxVenta = DateTime.Parse($"{DateTime.Now:yyyy-MM-dd} {hora2MaxVentaServ}");

                        if (DateTime.Now > hora2MaxVenta)
                        {
                            fechaOfrecida = fechaOfrecida.AddDays(diasAddEntrega);
                        }
                    }
                }
                // Lógica adicional para otros casos
                if (servicio != "004" && servicio != "018" && servicio != "019")
                {
                    //fechaOfrecida = _D_Inicio.DiaActivo().AddDays(5); /*DateTime.Now.AddDays(5);*/
                    fechaOfrecida = DateTime.Now.AddDays(5);
                    horaOfrecida = "12:01:01 P.M.";
                }

                // Retornar el resultado como una lista
                return new List<FechaHoraOfrecida>
                {
                    new FechaHoraOfrecida
                    {
                        FechaOfrecida = fechaOfrecida,
                        HoraOfrecida = horaOfrecida
                    }
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al calcular la fecha y hora ofrecida: {ex.Message}", ex);
            }
        }

        public FechaHoraOfrecida Calculo_Servicio_3Horas(DateTime glbFechaActiva)
        {
            // Variables iniciales
            DateTime fechaOfrecida = glbFechaActiva;
            DateTime horaActual = DateTime.Now;
            DateTime horaMas3 = horaActual.AddHours(3);

            // Consulta para obtener la hora de apertura y cierre de la sucursal
            DataSet dsSucursal = _D_Articulos.TB_SUCURSALES(_D_Inicio.Sucursal());

            if (dsSucursal.Tables[0].Rows.Count > 0)
            {
                DateTime horaApertura = Convert.ToDateTime(dsSucursal.Tables[0].Rows[0]["HorEntLAV"]);
                DateTime horaCierre = Convert.ToDateTime(dsSucursal.Tables[0].Rows[0]["HorSalLAV"]);

                // Si la hora con 3 horas añadidas excede la hora de cierre de la sucursal
                if (horaMas3 > horaCierre)
                {
                    TimeSpan horasRestantesHoy = horaCierre.Subtract(horaActual);
                    double horasPendientes = horasRestantesHoy.TotalHours > 0 ? 3 - horasRestantesHoy.TotalHours : 3;

                    // Calcular cuántos días se deben sumar
                    int diasAdicionales = (int)Math.Ceiling(horasPendientes / horaCierre.Subtract(horaApertura).TotalHours);

                    // Establecer la nueva fecha para el día siguiente y sumar las horas restantes desde la apertura
                    horaMas3 = horaApertura.AddHours(horasPendientes);

                    // Calcular la fecha ofrecida basada en días laborales
                    DateTime currentDate = glbFechaActiva.AddDays(diasAdicionales);
                    string formattedDate = currentDate.ToString("yyyy/MM/dd");

                    DataSet ds = _D_Articulos.tMASTER_diasHorario(formattedDate);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        fechaOfrecida = Convert.ToDateTime(ds.Tables[0].Rows[0]["fecha"].ToString());
                    }
                }
            }

            // Retornar la fecha y hora calculada
            return new FechaHoraOfrecida
            {
                FechaOfrecida = fechaOfrecida,
                HoraOfrecida = horaMas3.ToString("HH:mm:ss tt")
            };
        }

        public FechaHoraOfrecida Calculo_Servicio_1Horas(DateTime glbFechaActiva)
        {
            // Variables iniciales
            DateTime fechaOfrecida = glbFechaActiva;
            DateTime horaActual = DateTime.Now;
            DateTime horaMas1 = horaActual.AddHours(1);

            // Consulta para obtener la hora de apertura y cierre de la sucursal
            DataSet dsSucursal = _D_Articulos.TB_SUCURSALES(_D_Inicio.Sucursal());

            if (dsSucursal.Tables[0].Rows.Count > 0)
            {
                DateTime horaApertura = Convert.ToDateTime(dsSucursal.Tables[0].Rows[0]["HorEntLAV"]);
                DateTime horaCierre = Convert.ToDateTime(dsSucursal.Tables[0].Rows[0]["HorSalLAV"]);

                // Si la hora con 3 horas añadidas excede la hora de cierre de la sucursal
                if (horaMas1 > horaCierre)
                {
                    TimeSpan horasRestantesHoy = horaCierre.Subtract(horaActual);
                    double horasPendientes = horasRestantesHoy.TotalHours > 0 ? 1 - horasRestantesHoy.TotalHours : 1;

                    // Calcular cuántos días se deben sumar
                    int diasAdicionales = (int)Math.Ceiling(horasPendientes / horaCierre.Subtract(horaApertura).TotalHours);

                    // Establecer la nueva fecha para el día siguiente y sumar las horas restantes desde la apertura
                    horaMas1 = horaApertura.AddHours(horasPendientes);

                    // Calcular la fecha ofrecida basada en días laborales
                    DateTime currentDate = glbFechaActiva.AddDays(diasAdicionales);
                    string formattedDate = currentDate.ToString("yyyy/MM/dd");

                    DataSet ds = _D_Articulos.tMASTER_diasHorario(formattedDate);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        fechaOfrecida = Convert.ToDateTime(ds.Tables[0].Rows[0]["fecha"].ToString());
                    }
                }
            }

            // Retornar la fecha y hora calculada
            return new FechaHoraOfrecida
            {
                FechaOfrecida = fechaOfrecida,
                HoraOfrecida = horaMas1.ToString("HH:mm:ss tt")
            };
        }

        public FechaHoraOfrecida Calculo_Servicio_12Horas(DateTime glbFechaActiva)
        {
            // Variables iniciales
            DateTime fechaOfrecida = glbFechaActiva;
            DateTime horaActual = DateTime.Now;
            DateTime horaMas12 = horaActual.AddHours(12);

            // Calcular la fecha ofrecida basada en días laborales
            DateTime currentDate = glbFechaActiva.AddDays(1);
            string formattedDate = currentDate.ToString("yyyy/MM/dd");

            DataSet ds = _D_Articulos.tMASTER_diasHorario(formattedDate);
            if (ds.Tables[0].Rows.Count > 0)
            {
                fechaOfrecida = Convert.ToDateTime(ds.Tables[0].Rows[0]["fecha"]);
            }

            // Retornar la fecha y hora calculada
            return new FechaHoraOfrecida
            {
                FechaOfrecida = fechaOfrecida,
                HoraOfrecida = horaActual.ToString("HH:mm:ss tt")
            };
        }

        public DataTable Inserta_TB_TRABAJO(string NUMOS, string HORIZ, string VERT, string MAX, string PTE, string DISVERT,
            string ANPANT, string ANFAC, string ALTD, string ALTI, string OJO, string TVISD, string TVISI, string USER, string SUC)
        {
            Conexion cn = new Conexion();
            SqlConnection connection = cn.LeerCadena();
            SqlCommand command = connection.CreateCommand();
            //SqlTransaction transaction;
            // Iniciar la transacción
            //transaction = connection.BeginTransaction();
            command.Connection = connection;
            //command.Transaction = transaction;
            command.Parameters.Clear();
            command.CommandTimeout = 120;

            try
            {
               return _D_Articulos.Inserta_TB_TRABAJO(NUMOS, HORIZ, VERT, MAX, PTE, DISVERT, ANPANT, ANFAC, ALTD, ALTI, OJO, TVISD, TVISI, USER, SUC);
                

            }
            catch (Exception ex)
            {
               
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                //transaction.Rollback();
                return null;
            }
        }

        public List<FechaHoraOfrecida> ActualizarFechaOfre(DataGridView Dgv_Tap3_Articulo, string _MonturaPropia, string _Quorum, string _Color)
        {
            string CRISTAL = "";
            string ANTIREFL = "0";
            string MONTURA = "";
            string MONTURAPROPIA = _MonturaPropia== "False"? "0":"1";
            string LC = "";
            string Quorum = _Quorum == "False" ? "0" : "1"; 
            string Remoto = "0";
            string Color = _Color;
            string Laboratorio = "";
            string Sucursal = _D_Inicio.Sucursal();

            DataSet dsServAR = _D_Articulos.ServiciosAR_btnProcesar("", false);

            // Iterar sobre las filas del DataGridView base para calcular los totales
            foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
            {
                // Verificar que la fila no sea nueva
                if (row.IsNewRow) continue;

                // Subtotal: Cantidad * Precio
                if (row.Cells["CodArticulo"].Value != null)
                {
                    string codigo = row.Cells["CodArticulo"].Value.ToString();
                    if (codigo.StartsWith("C"))
                        CRISTAL = codigo;
                    else if (codigo.StartsWith("M") || codigo.StartsWith("L"))
                        MONTURA = codigo;
                    else if (codigo.StartsWith("W"))
                    {
                        LC = codigo;
                        Laboratorio = row.Cells["codLab"].Value.ToString();
                    }
                    else if (codigo.StartsWith("S"))
                        if (dsServAR.Tables.Count > 1)
                        {
                            foreach (DataRow dr in dsServAR.Tables[1].Rows)
                            {
                                if (codigo == dr["CodServicio"].ToString())
                                    ANTIREFL = "1";
                            }
                        }
                }

            }

            Dictionary<string, string> Variables_calculo = new Dictionary<string, string>
            {
                    { "@CRISTAL", CRISTAL },
                    { "@ANTIREFL", ANTIREFL},
                    { "@MONTURA",  MONTURA },
                    { "@MONTURAPROPIA", MONTURAPROPIA },
                    { "@LC", LC },
                    { "@Quorum", Quorum },
                    { "@Remoto", Remoto},
                    { "@Color", Color },
                    { "@Laboratorio", Laboratorio },
                    { "@Sucursal", Sucursal }
            };

            DataSet Calculos_Fecha = _D_Articulos.ActulizaFechaOfre(Variables_calculo, null);

            if (Calculos_Fecha.Tables[1].Rows.Count > 0)
            {
                DateTime fechaOfrecida= DateTime.Now;
                DateTime Hora_ofrecida= DateTime.Now;

                foreach (DataRow dr in Calculos_Fecha.Tables[1].Rows)
                {
                    fechaOfrecida = Convert.ToDateTime (dr["Dias"].ToString());
                    Hora_ofrecida = Convert.ToDateTime(dr["Hora"].ToString());
                    break;

                }

                // Retornar el resultado como una lista
                return new List<FechaHoraOfrecida>
                {
                    new FechaHoraOfrecida
                    {
                    FechaOfrecida = fechaOfrecida,
                    HoraOfrecida = Hora_ofrecida.ToString("hh:mm:ss tt")
                    }
                };
            }

            return null;
        }

        public bool Disponible_Servicio_3Horas(string servicio, string laboratorio)
        {
            // ----------------- Calculo si está disponible el servicio ENTREGA 3 HORAS -------------
            DateTime horaActual = DateTime.Now; // Obtener la hora actual completa (incluye minutos)

            //DataSet dsMontaje = ManBD.ExecutaSqlDataSet($"pGetSucursalMontaje '{glbSucursalActual}'", "", Command);
            DataSet dsMontaje = _D_Articulos.ObtenerSucursalMontaje(_D_Inicio.Sucursal());

            // Si es la sucursal que tiene el montaje
            if (dsMontaje.Tables[0].Rows.Count > 0)
            {
                
                DateTime horaMaxima = DateTime.Today.AddHours(Convert.ToDouble(_D_DetalleOrden.TB_PARAMETRO("HoraMaxServ3Hrs")));
                //DateTime.Today.AddHours(ValorParametro("HoraMaxServ3Hrs", Command));

                // Validar si la fecha es laboral
                //DateTime currentDate = glbFechaActiva;
                DateTime currentDate = _D_Inicio.DiaActivo();
                string formattedDate = currentDate.ToString("yyyy/MM/dd");
                bool Fecha_Laborable;

                //DataSet ds = ManBD.EjecutaSPSelectNuevo("fecha as Fecha_Laborable", "tMASTER_diasHorario", $"fecha = '{formattedDate}' AND laboral = 'True'", Command);
                DataSet ds = _D_Articulos.tMASTER_diasHorario(formattedDate);
                Fecha_Laborable = ds.Tables[0].Rows.Count > 0;

                if (servicio == "ENTREGA 3 HORAS" && laboratorio  == "QUORUM" && (horaActual > horaMaxima || !Fecha_Laborable))
                {
                    //MessageBox.Show("El servicio no está disponible en este horario", "Servicio no disponible", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            else
            {
                if (servicio == "ENTREGA 3 HORAS")
                {
                    //DateTime horaMaxima = DateTime.Today.AddHours(Convert.ToDateTime(_D_DetalleOrden.TB_PARAMETRO("HoraMaxServ3Hrs")));
                    DateTime horaMaxima = DateTime.Today.AddHours(Convert.ToDouble(_D_DetalleOrden.TB_PARAMETRO("HoraMaxServ3Hrs")));

                    // Validar si la fecha es laboral
                    //DateTime currentDate = glbFechaActiva;
                    DateTime currentDate = _D_Inicio.DiaActivo();
                    string formattedDate = currentDate.ToString("yyyy/MM/dd");
                    bool Fecha_Laborable;

                    //DataSet ds = ManBD.EjecutaSPSelectNuevo("fecha as Fecha_Laborable", "tMASTER_diasHorario", $"fecha = '{formattedDate}' AND laboral = 'True'", Command);
                    DataSet ds = _D_Articulos.tMASTER_diasHorario(formattedDate);
                    Fecha_Laborable = ds.Tables[0].Rows.Count > 0;

                    // Valido si Laboratorio es diferente de Quorum y tiene montaje remoto
                    if (laboratorio != "QUORUM" && dsMontaje.Tables[1].Rows.Count > 0)
                    {
                        if (horaActual > horaMaxima)
                        {
                            //MessageBox.Show("El servicio no está disponible en este horario", "Servicio no disponible", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                    }
                    else
                    {
                        // Laboratorio igual a Quorum o no tiene montaje remoto
                        // Si la hora actual supera la hora máxima o no es día laboral
                        if (horaActual > horaMaxima || !Fecha_Laborable)
                        {
                            //MessageBox.Show("El servicio no está disponible en este horario", "Servicio no disponible", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public string EsARManual(DataGridView Dgv_Tap3_Articulo,string codServicio, SqlCommand command = null)
        {
            
            string artPAdre = "";
            // 2. Recorrer el grid para igualar cantidades
            foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
            {
                if (row.IsNewRow || row.Cells["CodArticulo"].Value == null)
                    continue;

                string codigo = row.Cells["CodArticulo"].Value.ToString();

                //Si es coloración
                if (codigo.StartsWith("C"))
                {
                    DataSet dsServAR = _D_Articulos.ServiciosAR_btnProcesar(codigo, false, command);
                    //Si es un AR (validar con tabla 1 del dataset)
                    foreach (DataRow filaAR in dsServAR.Tables[1].Rows)
                    {
                        string codAR = filaAR["CodServicio"].ToString();
                        if (codServicio == codAR)
                        {
                            artPAdre = codigo;
                            
                            break;
                        }
                    }
                }
            }
            return artPAdre;
        }

        public bool CargarMedidasMontura(TextBox horizontal, TextBox vertical, TextBox maxima, TextBox puente, string CodMontura)
        {
            List<TB_ARTICULO> articulos = _D_Articulos.ObtenerArticulos("", CodMontura);

            if (articulos != null && articulos.Count > 0)
            {
                TB_ARTICULO articulo = articulos.First();

                horizontal.Text = articulo.MHorizontal.ToString();
                vertical.Text = articulo.MVertical.ToString();
                maxima.Text = articulo.MMaxima.ToString();
                puente.Text = articulo.MPuente.ToString();

                if (string.IsNullOrEmpty(horizontal.Text))
                    horizontal.Enabled = true;                   
                else
                    horizontal.Enabled = false;

                if (string.IsNullOrEmpty(vertical.Text))
                    vertical.Enabled = true;
                else
                    vertical.Enabled = false;

                if (string.IsNullOrEmpty(maxima.Text))
                    maxima.Enabled = true;
                else
                    maxima.Enabled = false;

                if (string.IsNullOrEmpty(puente.Text))
                {
                    puente.Enabled = true;
                    puente.Focus();
                }
                else
                    puente.Enabled = false;
               
                return true;
            }
            // Si todos los campos tienen valores, devolver false
            return false;

        }

        public bool TieneMontura(DataGridView Dgv_Tap3_Articulo)
        {

            bool encontrado = false;

            // Recorrer todas las filas del DataGridView
            foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
            {
                if (row.Cells["CodArticulo"].Value.ToString().StartsWith("M") || row.Cells["CodArticulo"].Value.ToString().StartsWith("L"))
                {
                    return true; // Se encontró montura
                    
                }
                
            }
            return false;

        }

        public bool BuscoCodigoLabLC(string codArticulo, string codColor, string Nacionalidad, string Cedula, int NumExamen, string ojoVision,string ojoLenteContacto, string cant,
            Action<string, string> guardarDatos // <-- delegado
        )
        {
            stringBuilder.Clear();

            try
            {

                if (cant == "0")
                {
                    stringBuilder.AppendLine("Indique la cantidad de este artículo");
                    return false;
                }

                if (ojoVision == "Ambos")
                {
                    ojoVision = "A";
                }

                if (ojoVision == "Derecho")
                {
                    ojoVision = "D";
                }

                if (ojoVision == "Izquierdo")
                {
                    ojoVision = "I";
                }

                // Ejecuta el procedimiento almacenado
                DataSet dsGetLC = _D_Articulos.lenteContacto_Color_Existencia(string.IsNullOrEmpty(ojoLenteContacto) ? ojoVision : ojoLenteContacto, codColor, Nacionalidad, Cedula, NumExamen.ToString(), codArticulo, null);

                if (dsGetLC.Tables[0].Rows.Count == 1)
                {
                    var row = dsGetLC.Tables[0].Rows[0];
                    guardarDatos.Invoke(row["CodLabarticulo"].ToString(), row["Generico"].ToString());
                    ////gexFacturas.GetRow(gexFacturas.Row).Cells["CodigoLab"].Value = row["CodLabarticulo"];
                    ////gexFacturas.GetRow(gexFacturas.Row).Cells["Existencia"].Value = row["CANTIDAD"];
                    ////gexFacturas.GetRow(gexFacturas.Row).Cells["ManejaExistencia"].Value = row["Stock"];
                    ////gexFacturas.GetRow(gexFacturas.Row).Cells["Generico"].Value = row["Generico"];

                    bool result = true;
                    DataSet dsGetEX = _D_Articulos.lenteContacto_Tranferencia(codArticulo, Convert.ToInt32(cant), null);

                    if (Convert.ToInt32(row["CANTIDAD"]) - Convert.ToInt32(cant) < 0)
                    {
                        if (dsGetEX.Tables[0].Rows.Count == 1 && dsGetEX.Tables[0].Rows[0][0].ToString() != "HAY EXISTENCIA" && dsGetEX.Tables[1].Rows.Count == 1)
                        {
                            stringBuilder.AppendLine("Este artículo está pendiente POR RECIBIR en una Transferencia. Realice primero este proceso y luego facture este artículo");
                            result = false;
                        }
                    }

                    ////if (dsGetEX.Tables[0].Rows.Count == 1 && dsGetEX.Tables[0].Rows[0][0].ToString() != "HAY EXISTENCIA")
                    ////{
                    ////    if (dsGetEX.Tables.Count > 1 && dsGetEX.Tables[1].Rows.Count == 1)
                    ////    {
                    ////        result = false;
                    ////    }
                    ////}

                    stringBuilder.AppendLine("Este artículo se encuentra en estatus amarillo en el laboratorio");

                    return result;
                }
                else if (dsGetLC.Tables[0].Rows.Count > 1)
                {
                    stringBuilder.AppendLine("La combinación de atributos para Lentes de Contacto genera 2 códigos diferentes. Si el cliente tiene esferas diferentes, debe especificar cada ojo por separado");
                    return false;
                }
                else
                {
                    stringBuilder.AppendLine("No se encontró coincidencia con el código de laboratorio para lente de contacto");
                    return false;
                }
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return false;
            }
        }

        public bool VerificoCodigoLabLC(string TxtOjo, DataGridView Dgv_Tap3_Articulo, string Nacionalidad, string Cedula, int NumExamen)
        {
                bool ojoD = false, ojoI = false, ojoA = false;
                bool lcSinExist = false, lcConExist = false;
                stringBuilder.Clear();


            // Validar stock si aplica
            if (_D_DetalleOrden.TB_PARAMETROSPAIS("ValidaStockLC") == "1")
            {
                foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
                {
                    if (row.IsNewRow) continue;

                    string codArticulo = row.Cells["CodArticulo"].Value?.ToString() ?? "";
                    string codColor = row.Cells["ColorLC"].Value?.ToString() ?? "";
                    string cantidad = row.Cells["ART_EXIST"].Value?.ToString() ?? "0";
                    string ojo = row.Cells["Ojo"].Value?.ToString() ?? "";
                    string codigoLab = "";
                    // Solo valida artículos de lentes de contacto (por ejemplo, los que empiezan con "W")
                    if (!codArticulo.StartsWith("W")) continue;

                    // Marcar qué ojos están presentes
                    if (ojo == "A") ojoA = true;
                    else if (ojo == "I") ojoI = true;
                    else if (ojo == "D") ojoD = true;

                    // Validar existencia
                    int existencia = 0, cant = 0;
                    int.TryParse(row.Cells["ART_EXIST"].Value?.ToString(), out existencia);

                    // Ejecuta el procedimiento almacenado
                    DataSet dsGetLC = _D_Articulos.lenteContacto_Color_Existencia(ojo, codColor, Nacionalidad, Cedula, NumExamen.ToString(), codArticulo, null);
                    if (dsGetLC.Tables[0].Rows.Count == 1)
                    {
                        var row2 = dsGetLC.Tables[0].Rows[0];
                        int.TryParse(row2["CANTIDAD"].ToString(), out cant);
                        codigoLab = row2["CodLabarticulo"].ToString();
                    }

                    if (string.IsNullOrEmpty(codigoLab))
                    {
                        stringBuilder.AppendLine("El código de laboratorio para el Lente de Contacto se encuentra vacío. Presione el Botón Cancelar y cargue los artículos nuevamente");
                        return false;
                    }

                    if (cant <= 0)
                        lcSinExist = true;
                    else
                    {
                        lcConExist = true;
                        //PQC
                        if (TxtOjo == "Ambos")
                        {
                            existencia = 2;
                        }
                        else
                        {
                            existencia = 1;
                        }
                        if (existencia < cant) //existencia es la cant que se esta vendiento y cantidad la existencia en la tabla
                        {
                            stringBuilder.AppendLine(
                                "Este artículo no tiene existencia");
                            //$"La existencia del lente no cubre la cantidad que desea vender. Solo puede vender {existencia} del artículo {codArticulo} en esta orden");
                            return false;
                        }
                    }

                  
                }

                // Validar combinación de ojos
                if (TxtOjo == "Ambos" && (ojoA || (ojoD && ojoI)))
                {
                    // válido
                }
                else if (TxtOjo == "Izquierdo" && !ojoA && !ojoD && ojoI)
                {
                    // válido
                }
                else if (TxtOjo == "Derecho" && !ojoA && ojoD && !ojoI)
                {
                    // válido
                }
                else
                {
                    stringBuilder.AppendLine("La cantidad de Ojos seleccionada no corresponde con los artículos cargados");
                    return false;
                }

                // Validar stock mixto
                if (lcSinExist && lcConExist)
                {
                    stringBuilder.AppendLine(
                    "No puede vender artículos con existencia y contra pedido en la misma Orden. Facture los artículos en órdenes separadas");
                    return false;
                }
            }
                return true;

        }

        public void LlenarComboOjos(ComboBox combo)
        {
            combo.Items.Clear();
            combo.DisplayMember = "Text";
            combo.ValueMember = "Value";

            combo.Items.Add(new { Text = "Ambos", Value = "A" });
            combo.Items.Add(new { Text = "Izquierdo", Value = "I" });
            combo.Items.Add(new { Text = "Derecho", Value = "D" });

            if (combo.Items.Count > 0)
                combo.SelectedIndex = 0;
        }

        public bool VerificoCantidadProducto(DataGridView Dgv_Tap3_Articulo)
        {
            stringBuilder.Clear();
            try
            {
                // Contadores por tipo de artículo
                int C = 0, E = 0, L = 0, M = 0, Q = 0, S = 0, W = 0, V = 0, B = 0, Misc = 0;

                foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
                {
                    if (row.IsNewRow) continue;

                    string codigoProducto = row.Cells["CodArticulo"].Value?.ToString() ?? "";
                    int cantidad = 0;
                    int.TryParse(row.Cells["ART_EXIST"].Value?.ToString(), out cantidad);

                    if (string.IsNullOrEmpty(codigoProducto)) continue;

                    string tipo = codigoProducto.Substring(0, 1).ToUpper();

                    switch (tipo)
                    {
                        case "C":
                            C += cantidad;
                            break;
                        case "E":
                            E += cantidad;
                            break;
                        case "L":
                            L += cantidad;
                            break;
                        case "M":
                            M += cantidad;
                            break;
                        case "Q":
                            Q += cantidad;
                            break;
                        case "S":
                            S += cantidad;
                            break;
                        case "W":
                            W += cantidad;
                            break;
                        case "V":
                            V += cantidad;
                            break;
                        case "X":
                            Misc += cantidad;
                            break;
                    }
                }

                // Validar máximos por tipo de artículo
                var tipos = new Dictionary<string, int>
        {
            { "C", C },
            { "E", E },
            { "L", L },
            { "M", M },
            { "Q", Q },
            { "S", S },
            { "W", W },
            { "V", V },
            { "X", Misc }
        };

                foreach (var tipo in tipos)
                {
                    // Consulta el máximo permitido para este tipo
                    DataTable DT_ValorMaximo = _D_Articulos.BucarArticuloMaximoPorVenta(tipo.Key);
                    if (DT_ValorMaximo.Rows.Count > 0)
                    {
                        int maxVta = Convert.ToInt32(DT_ValorMaximo.Rows[0]["Max_Vta"]);
                        if (tipo.Value > maxVta)
                        {
                            stringBuilder.AppendLine($"La cantidad del articulo sobrepasa su límite para la venta");
                        }
                    }
                }

                // Si hay mensajes en el stringBuilder, hubo errores
                return stringBuilder.Length == 0;
            }
            catch (Exception ex)
            {
                stringBuilder.AppendLine("Error: " + ex.Message);
                return false;
            }
        }



        public void BucarTipoMotivoGarantia(System.Windows.Forms.ComboBox comboBox)
        {
            DataTable dt = _D_Articulos.BucarMotivoRepoGarantia();
            if (dt.Rows.Count > 0)
            {
                // Asignar el DataTable como fuente de datos del ComboBox
                comboBox.DataSource = dt;

                comboBox.DisplayMember = "Descripcion";
                comboBox.ValueMember = "CodMotivo";
                comboBox.SelectedIndex = -1;

            }
            else
            {
                // Si no hay datos, limpiar el ComboBox
                comboBox.DataSource = null;
                comboBox.Items.Clear();
            }

        }

        public bool BucarGarantiaCliente(System.Windows.Forms.DataGridView DgvGarantia, string Nacionalidad, string Cedula)
        {
            if (string.IsNullOrEmpty(Nacionalidad) || string.IsNullOrEmpty(Cedula))
            {
                stringBuilder.Append("Debe colocar un nunmero de cedula");
                return false;
            }

            DataTable dt= _D_Articulos.BucarOsRepoGarantia(_D_Inicio.Sucursal(), Nacionalidad, Cedula);
            if (dt.Rows.Count > 0)
            {
                CrearObjeto_GarantiaGrid(DgvGarantia);
                DgvGarantia.DataSource = dt;
                // Ahora puedes llenar esa columna combinando valores de otras columnas
                foreach (DataGridViewRow row in DgvGarantia.Rows)
                {
                    if (row.IsNewRow) continue; // Evita la fila para nueva entrada
                    var valor1 = row.Cells["Cte_Nacionalidad"].Value?.ToString() ?? "";
                    var valor2 = row.Cells["Cte_Cedula"].Value?.ToString() ?? "";
                    row.Cells["Cte_Cedula"].Value = valor1 + "-" + valor2;
                }

                Formato_Dgv_Garantia(DgvGarantia);
                return true;
            }
            else
                stringBuilder.Append("Este cliente no posee Ordenes de Servicio con Plan de Garantia Extendida");
            return false;
        }
        public void CrearObjeto_GarantiaGrid(DataGridView DgvGarantia)
        {

            DataGridViewCheckBoxColumn CheckBoxColumn = new DataGridViewCheckBoxColumn();
            CheckBoxColumn.Name = "E";
            CheckBoxColumn.Width = 40;
            CheckBoxColumn.HeaderText = "E";
            DgvGarantia.Columns.Add(CheckBoxColumn);

            //DataGridViewColumn Colunma = new DataGridViewColumn();
            //Colunma.Name = "Cedula";
            //Colunma.Width = 70;
            //Colunma.HeaderText = "Cedula";
            //DgvGarantia.Columns.Add(Colunma);

        }

        private void Formato_Dgv_Garantia(DataGridView Dgv_Tap3_Garantia)
        {

            //Centrar todas las colucnas 
            Dgv_Tap3_Garantia.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv_Tap3_Garantia.ScrollBars = ScrollBars.Both;

            // Quitar la flecha del selector de fila
            Dgv_Tap3_Garantia.RowHeadersVisible = false;

            // Deshabilitar el redimensionamiento de filas
            Dgv_Tap3_Garantia.AllowUserToResizeRows = false;

            Dgv_Tap3_Garantia.Columns["E"].DisplayIndex = 0;

            //Dgv_Tap3_Garantia.Columns["Cedula"].DisplayIndex = 1;
            //asignar Nombres a cada colucna 
            Dgv_Tap3_Garantia.Columns["E"].HeaderText = "E";
            Dgv_Tap3_Garantia.Columns["Cod_Sucursal"].HeaderText = "Sucursal";
            Dgv_Tap3_Garantia.Columns["Cte_Nacionalidad"].HeaderText = "Nacionalidad";
            Dgv_Tap3_Garantia.Columns["Cte_Cedula"].HeaderText = "Cédula";
            //Dgv_Tap3_Garantia.Columns["Cedula"].HeaderText = "Cedula";
            Dgv_Tap3_Garantia.Columns["NumOrdServ"].HeaderText = "N° Orden";
            Dgv_Tap3_Garantia.Columns["FechaOS"].HeaderText = "Fecha Orden";
            Dgv_Tap3_Garantia.Columns["FechaFactura"].HeaderText = "Fecha Factura";
            Dgv_Tap3_Garantia.Columns["FactNum"].HeaderText = "N° Factura";
            Dgv_Tap3_Garantia.Columns["StatusFactura"].HeaderText = "Status";
            Dgv_Tap3_Garantia.Columns["Edad"].HeaderText = "Edad";
            Dgv_Tap3_Garantia.Columns["TiempoRepos"].HeaderText = "Tiempo Reposición";
            Dgv_Tap3_Garantia.Columns["NumExamen"].HeaderText = "N° Examen";

            Dgv_Tap3_Garantia.Columns["CristalDerecho"].HeaderText = "Cristal Derecho";
            Dgv_Tap3_Garantia.Columns["CristalIzquierdo"].HeaderText = "Cristal Izquierdo";
            Dgv_Tap3_Garantia.Columns["Montura"].HeaderText = "Montura";

            Dgv_Tap3_Garantia.Columns["Edad"].DisplayIndex = 17;
            Dgv_Tap3_Garantia.Columns["TiempoRepos"].DisplayIndex = 18;
            Dgv_Tap3_Garantia.Columns["CristalDerecho"].DisplayIndex = 20;
            Dgv_Tap3_Garantia.Columns["CristalIzquierdo"].DisplayIndex = 21;
            Dgv_Tap3_Garantia.Columns["Montura"].DisplayIndex = 22;
            Dgv_Tap3_Garantia.Columns["FechaFactura"].DisplayIndex = 8;
            Dgv_Tap3_Garantia.Columns["NumExamen"].DisplayIndex = 19;
            Dgv_Tap3_Garantia.Columns["StatusFactura"].DisplayIndex = 25;

            //Ancho de columna
            Dgv_Tap3_Garantia.Columns["Cod_Sucursal"].Width = 70;
            Dgv_Tap3_Garantia.Columns["Cte_Nacionalidad"].Width = 70;
            Dgv_Tap3_Garantia.Columns["Cte_Cedula"].Width = 70;
            ////Dgv_Tap3_Garantia.Columns["Cedula"].Width = 70;
            Dgv_Tap3_Garantia.Columns["NumOrdServ"].Width = 70;
            Dgv_Tap3_Garantia.Columns["FechaOS"].Width = 70;
            Dgv_Tap3_Garantia.Columns["FactNum"].Width = 70;
            Dgv_Tap3_Garantia.Columns["StatusFactura"].Width = 40;
            Dgv_Tap3_Garantia.Columns["Edad"].Width = 57;
            Dgv_Tap3_Garantia.Columns["TiempoRepos"].Width = 120;
            Dgv_Tap3_Garantia.Columns["FechaFactura"].Width = 70;
            Dgv_Tap3_Garantia.Columns["CristalDerecho"].Width = 65;
            Dgv_Tap3_Garantia.Columns["CristalIzquierdo"].Width = 65;
            Dgv_Tap3_Garantia.Columns["Montura"].Width = 70;
            Dgv_Tap3_Garantia.Columns["NumExamen"].Width = 50;

            // No modificable
            Dgv_Tap3_Garantia.Columns["Cod_Sucursal"].ReadOnly = true;;
            Dgv_Tap3_Garantia.Columns["Cte_Nacionalidad"].ReadOnly = true;
            Dgv_Tap3_Garantia.Columns["Cte_Cedula"].ReadOnly = true;
            Dgv_Tap3_Garantia.Columns["NumOrdServ"].ReadOnly = true;
            Dgv_Tap3_Garantia.Columns["FechaOS"].ReadOnly = true;
            Dgv_Tap3_Garantia.Columns["FactNum"].ReadOnly = true;
            Dgv_Tap3_Garantia.Columns["StatusFactura"].ReadOnly = true;
            Dgv_Tap3_Garantia.Columns["Edad"].ReadOnly = true;
            Dgv_Tap3_Garantia.Columns["TiempoRepos"].ReadOnly = true;
            //Dgv_Tap3_Garantia.Columns["Cedula"].ReadOnly = true;
            Dgv_Tap3_Garantia.Columns["FechaFactura"].ReadOnly = true;
            Dgv_Tap3_Garantia.Columns["CristalDerecho"].ReadOnly = true;
            Dgv_Tap3_Garantia.Columns["CristalIzquierdo"].ReadOnly = true;
            Dgv_Tap3_Garantia.Columns["Montura"].ReadOnly = true;
            Dgv_Tap3_Garantia.Columns["NumExamen"].ReadOnly = true;

            Dgv_Tap3_Garantia.Columns["Cod_Sucursal"].SortMode = DataGridViewColumnSortMode.NotSortable;
            Dgv_Tap3_Garantia.Columns["Cte_Nacionalidad"].SortMode = DataGridViewColumnSortMode.NotSortable;
            Dgv_Tap3_Garantia.Columns["Cte_Cedula"].SortMode = DataGridViewColumnSortMode.NotSortable;
            Dgv_Tap3_Garantia.Columns["NumOrdServ"].SortMode = DataGridViewColumnSortMode.NotSortable;
            Dgv_Tap3_Garantia.Columns["FechaOS"].SortMode = DataGridViewColumnSortMode.NotSortable;
            Dgv_Tap3_Garantia.Columns["FactNum"].SortMode = DataGridViewColumnSortMode.NotSortable;
            Dgv_Tap3_Garantia.Columns["StatusFactura"].SortMode = DataGridViewColumnSortMode.NotSortable;
            Dgv_Tap3_Garantia.Columns["Edad"].SortMode = DataGridViewColumnSortMode.NotSortable;
            Dgv_Tap3_Garantia.Columns["TiempoRepos"].SortMode = DataGridViewColumnSortMode.NotSortable;
            Dgv_Tap3_Garantia.Columns["FechaFactura"].SortMode = DataGridViewColumnSortMode.NotSortable;
            Dgv_Tap3_Garantia.Columns["NumExamen"].SortMode = DataGridViewColumnSortMode.NotSortable;
            //Dgv_Tap3_Garantia.Columns["Cedula"].SortMode = DataGridViewColumnSortMode.NotSortable;

            foreach (DataGridViewColumn column in Dgv_Tap3_Garantia.Columns)
            {
                    if (!(column.Name.Equals("E", StringComparison.OrdinalIgnoreCase) ||
    column.Name.Equals("Cod_Sucursal", StringComparison.OrdinalIgnoreCase) ||
    column.Name.Equals("Cte_Cedula", StringComparison.OrdinalIgnoreCase) ||
    column.Name.Equals("NumOrdServ", StringComparison.OrdinalIgnoreCase) ||
    column.Name.Equals("FechaOS", StringComparison.OrdinalIgnoreCase) ||
    column.Name.Equals("FactNum", StringComparison.OrdinalIgnoreCase) ||
    column.Name.Equals("StatusFactura", StringComparison.OrdinalIgnoreCase) ||
    column.Name.Equals("Edad", StringComparison.OrdinalIgnoreCase) ||
    column.Name.Equals("TiempoRepos", StringComparison.OrdinalIgnoreCase) ||
    column.Name.Equals("CristalDerecho", StringComparison.OrdinalIgnoreCase) ||
    column.Name.Equals("CristalIzquierdo", StringComparison.OrdinalIgnoreCase) ||
    column.Name.Equals("FechaFactura", StringComparison.OrdinalIgnoreCase) ||
    column.Name.Equals("NumExamen", StringComparison.OrdinalIgnoreCase) ||
    column.Name.Equals("Montura", StringComparison.OrdinalIgnoreCase)))

                    {
                        column.Visible = false;
                    }

                    if (column.Index== 42)
                    column.Visible = true;


                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
             }

            //quitar seleccion por defecto de datagrid
            Dgv_Tap3_Garantia.ClearSelection();

            //AutoGenerar Columnas:
            Dgv_Tap3_Garantia.AutoGenerateColumns = false;
            // No puedad cambiar el tamaño de las columnas
            Dgv_Tap3_Garantia.AllowUserToResizeColumns = false;

        }

        public void GarantiaCristales_Selecion(DataGridView DgvGarantia, ref string Os_Garantia_Trabajo , ref string Numero_Examen_Garantia_Trabajo)
        {
            foreach (DataGridViewRow row in DgvGarantia.Rows)
            {
                if (row.Cells["E"].Selected)
                {
                    Os_Garantia_Trabajo = row.Cells["NumOrdServ"].Value?.ToString() ?? "";
                    Numero_Examen_Garantia_Trabajo = row.Cells["NumExamen"].Value?.ToString() ?? "";

                    //DataTable DT= _D_Articulos.ObtenerExamen(row.Cells["Cte_Cedula"].Value.ToString().Substring(0, 1), row.Cells["Cte_Cedula"].Value.ToString().Substring(2, row.Cells["Cte_Cedula"].Value.ToString().Length-2), _D_Inicio.Sucursal());
                    //foreach (DataRow row2 in DT.Rows)
                    //{
                    //    Numero_Examen_Garantia_Trabajo = row2["NUM_EXAMEN"].ToString();
                    //    break;
                    //}
                    break;
                }

            }

        }

        public bool AplicoGarantia(DataGridView DgvArticulo, string nacio, string CI, string OS, string exam)
        {
            stringBuilder.Clear();
            try
            {
                // Ejecuta el procedimiento almacenado
                DataSet dsGetLC = _D_Articulos.InfoReposicionGarantia(CI, nacio, OS, _D_Inicio.Sucursal(), exam);
                // Servicios Dioptria
                DataSet dsServicioAgregado = _D_Articulos.BucarServicioAgregado();
                //AR
                DataSet dsServAR = _D_Articulos.ServiciosAR_btnProcesar("", false);

                if (dsGetLC.Tables.Count > 2 && dsGetLC.Tables[2].Rows.Count > 0)
                {
                    foreach (DataGridViewRow row in DgvArticulo.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string codArticulo = row.Cells["CodArticulo"].Value?.ToString() ?? "";
                        int Cantidad_Servicio = row.Cells["ART_EXIST"].Value != null ? Convert.ToInt32(row.Cells["ART_EXIST"].Value) : 0;

                        int filaSeleccionada= row.Index;
                        if (codArticulo.StartsWith("C"))
                        {
                            decimal precio = Convert.ToDecimal(row.Cells["ART_PVP"].Value.ToString());
                            decimal descuento = Convert.ToDecimal(dsGetLC.Tables[2].Rows[0]["DESCUENTOCRT"]);
                            decimal NuevoPrecio = precio - descuento;
                            if (NuevoPrecio <= 0)
                            {
                                NuevoPrecio = 0.005M;
                            }
                            ActualizarCelda(DgvArticulo, filaSeleccionada, "ART_PVP", NuevoPrecio.ToString("N2"));
                        }

                        if (codArticulo.StartsWith("S"))
                        {
                            // Coloracion
                            if (codArticulo == "S000004")
                            {
                                decimal precio = Convert.ToDecimal(row.Cells["ART_PVP"].Value.ToString()); 
                                decimal descuento = Convert.ToDecimal(dsGetLC.Tables[2].Rows[0]["DESCUENTOSERVCOLOR"]);
                                decimal NuevoPrecio = precio - descuento;
                                if (NuevoPrecio <= 0)
                                {
                                    NuevoPrecio = 0.005M;
                                }
                                ActualizarCelda(DgvArticulo, filaSeleccionada, "ART_PVP", NuevoPrecio.ToString("N2"));
                            }

                            // Prisma 
                            if (codArticulo == "S000006")
                            {
                                decimal precio = Convert.ToDecimal(row.Cells["ART_PVP"].Value.ToString());
                                decimal descuento = Convert.ToDecimal(dsGetLC.Tables[2].Rows[0]["DESCUENTOSERVPRISMA"]);
                                decimal NuevoPrecio = precio - descuento;
                                if (NuevoPrecio <= 0)
                                {
                                    NuevoPrecio = 0.005M;
                                }
                                ActualizarCelda(DgvArticulo, filaSeleccionada, "ART_PVP", NuevoPrecio.ToString("N2"));
                            }

                            if (dsServicioAgregado != null && dsServicioAgregado.Tables.Count > 0 && dsServicioAgregado.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow dr in dsServicioAgregado.Tables[0].Rows)
                                {
                                    string agregadoProducto = dr["Agregado_Producto"]?.ToString().Trim('"');
                                    if (!string.IsNullOrEmpty(agregadoProducto) && agregadoProducto == codArticulo)
                                    {
                                        decimal precio = Convert.ToDecimal(row.Cells["ART_PVP"].Value.ToString());
                                        string Ojo = row.Cells["Ojo"].Value.ToString();
                                        decimal descuento = 0.00M;
                                        if (Ojo.StartsWith("D"))
                                        {
                                            descuento = Convert.ToDecimal(dsGetLC.Tables[2].Rows[0]["DESCUENTODIOPD"]);
                                        }
                                        else if (Ojo.StartsWith("I"))
                                        {
                                            descuento = Convert.ToDecimal(dsGetLC.Tables[2].Rows[0]["DESCUENTODIOPI"]);
                                        }
                                        else
                                        {
                                            descuento = Convert.ToDecimal(dsGetLC.Tables[2].Rows[0]["DESCUENTODIOPD"]);
                                        }

                                        decimal NuevoPrecio = precio - descuento;
                                        if (NuevoPrecio <= 0)
                                        {
                                            NuevoPrecio = 0.005M;
                                        }
                                        ActualizarCelda(DgvArticulo, filaSeleccionada, "ART_PVP", NuevoPrecio.ToString("N2"));
                                        //break; // Salir del bucle interno si se encuentra el servicio
                                    }
                                }
                            }

                            if (dsServAR != null && dsServAR.Tables.Count > 1 && dsServAR.Tables[1].Rows.Count > 0)
                            {
                                foreach (DataRow dr in dsServAR.Tables[1].Rows)
                                {
                                    string agregadoProducto = dr["CodServicio"]?.ToString().Trim('"');
                                    if (!string.IsNullOrEmpty(agregadoProducto) && agregadoProducto == codArticulo)
                                    {
                                        decimal precio1 = Convert.ToDecimal(row.Cells["ART_PVP"].Value.ToString());
                                        decimal descuento1 = Convert.ToDecimal(dsGetLC.Tables[2].Rows[0]["DESCUENTOSERVAR"]);
                                        decimal NuevoPrecio1 = precio1 - descuento1;
                                        if (NuevoPrecio1 <= 0)
                                        {
                                            NuevoPrecio1 = 0.005M;
                                        }
                                        ActualizarCelda(DgvArticulo, filaSeleccionada, "ART_PVP", NuevoPrecio1.ToString("N2"));
                                        //break; // Salir del bucle interno si se encuentra el servicio
                                    }
                                }
                            }
                           
                        }

                    }
                    return true;
                }
                else
                {
                    stringBuilder.Append("Esta orden no aplica para reposición de garantia");
                    return false;
                }
            }
            catch (Exception ex)
            {
                stringBuilder.AppendLine("Error: " + ex.Message);
                return false;
            }
        }


        public bool ValidarCantidadCristales(string OjoSelecionado, System.Windows.Forms.DataGridView Dgv_Tap3_Articulo, Action<string> mostrarError)
        {
            string ojo = "";
            string ojoGrid = "";
            int countCristales = 0;

            foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
            {
                if (row.IsNewRow) continue;
                string cod = row.Cells["CodArticulo"].Value?.ToString() ?? "";
                if ((cod.StartsWith("C") || cod.StartsWith("W")))
                {
                    countCristales = Convert.ToInt32( row.Cells["ART_EXIST"].Value?.ToString() ?? "0");
                }
            }


            if (OjoSelecionado == "Ambos")
            {
                if (countCristales < 2)
                {
                    mostrarError($"La cantidad de cristales no es correcta");
                    return false;
                }
                else if (countCristales > 2)
                {
                    mostrarError("Solo puede haber 2 cristales o lentes de contacto para Ambos ojos");
                    return false;
                }
                else if (countCristales == 2)
                {
                    return true;
                }

            }
            else if (OjoSelecionado == "Izquierdo" || OjoSelecionado == "Derecho")
            {

                if (countCristales > 1)
                {
                    mostrarError($"Solo puede haber 1 cristal o lente de contacto para 'Ojo {OjoSelecionado}'");
                    return false;
                }
                else if (countCristales <= 0)
                {
                    mostrarError($"Debe agregar  1 cristal o lente de contacto para 'Ojo {OjoSelecionado}'");
                    return false;
                }
                else if (countCristales == 1)
                {
                    return true;
                }
            }

            return false;
        }



        public bool VerificoCantidadCristales(string OjoSelecionado, System.Windows.Forms.DataGridView Dgv_Tap3_Articulo, Action<string> mostrarError)
        {
            try
            {
                int cristal = 0;
                int cantc = 0;
                bool LenteContacto = false;

                // Verificar cantidad de cristales
                foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
                {
                    string codigo = row.Cells["CodArticulo"].Value?.ToString() ?? "";
                    string cantidadTexto = row.Cells["ART_EXIST"].Value?.ToString() ?? "";

                    if (codigo.StartsWith("C"))
                    {
                        if (string.IsNullOrWhiteSpace(cantidadTexto))
                        {
                            mostrarError($"La cantidad de cristales no es correcta");
                            return false;
                           
                        }
                        else if (cantidadTexto == "2")
                        {
                            cristal = 2;
                            break;
                        }
                        else if (cantidadTexto == "1")
                        {
                            cristal += 1;
                        }
                    }
                }

                // Verificar lentes de contacto
                foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
                {
                    string codigo = row.Cells["CodArticulo"].Value?.ToString() ?? "";
                    if (codigo.StartsWith("W"))
                    {
                        object cantidad = row.Cells["ART_EXIST"].Value?.ToString() ?? "";
                        if (cantidad != null && int.TryParse(cantidad.ToString(), out int valor))
                        {
                            cantc += valor;
                        }
                        LenteContacto = true;
                    }
                }

                // Verificar trabajo y ojo seleccionado
                

                if (OjoSelecionado != "Ambos")
                {
                    cristal += 1;
                }

                if (cristal == 2)
                {
                    return true;
                }
                else if (LenteContacto)
                {
                    if (OjoSelecionado != "Ambos")
                    {
                        return cantc == 2 ? true : true;
                    }
                    else
                    {
                        return cantc == 2 ? true : true;
                    }
                }
                else
                {
                    mostrarError($"La cantidad de cristales no es correcta");
                    return false;
                }
            }
            catch (Exception ex)
            {
                //MensajeError.MuestroMensaje("Error en la función", "frmFacturas.VerificoCantidadCristales",
                //    "Por favor comunicarse con el Dpto de Sistemas y reportar el siguiente error: ",
                //    ex.Message, CapaNegocio.MensajesGenerales.TiposIconos.IconoError, glbUsuarioActual);
                //MensajeError.ShowDialog();
                return false;
            }
        }

        public bool ValidarAplica(DataGridView Dgv_Tap3_Articulo, string Cod_Vta, bool ClienteAfiliado, bool AplicaPromocion, bool MonturaPropia, bool CristalPropio)
        {
            try
            {
             
                bool Aplica= false;

            if (AplicaPromocion== false && ClienteAfiliado== false && MonturaPropia== false && CristalPropio== false)
            { 
                if ((Cod_Vta == "05" || Cod_Vta == "02"))
                {
                    Aplica = false;
                }
                else
                {
                    foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
                    {
                        if (row.Cells["CodArticulo"].Value != null && ((row.Cells["CodArticulo"].Value.ToString().StartsWith("M")) || (row.Cells["CodArticulo"].Value.ToString().StartsWith("L"))))
                        {
                            string CodArticulo = row.Cells["CodArticulo"].Value.ToString();
                            DataTable dt = _D_Articulos.ObtenerCODrango(CodArticulo);
                            if (dt.Rows!= null && dt.Rows.Count >  0 && dt.Rows[0]["CODrango"].ToString()!= "A" && dt.Rows[0]["CODrango"].ToString() != "B" && dt.Rows[0]["CODrango"].ToString() != "C")
                            {
                                    return true;
                            }
                            else
                            {
                             Aplica = false;
                            }
                        }
                    }
                }
                
            }
            else
            {
            Aplica = false;
            }  
                return Aplica;
            }
            catch (Exception ex)
            {
                //// Manejar cualquier excepción
                //throw new Exception("Error al verificar y corregir los totales: " + ex.Message, ex);
                return false;
            }
        }
    }
}

