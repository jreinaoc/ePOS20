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

namespace CapaLogica.CargarOrdenes
{
    public class L_Articulo
    {
        D_Anulacion _D_Anulacion = new D_Anulacion();
        D_Articulos _D_Articulos = new D_Articulos();
        D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        D_Inicio _D_Inicio = new D_Inicio();
        public Boolean Diopprima = false;
        private System.Reflection.Assembly oEnsamblado;
        //El uso de la clase StringBuilder nos ayudara a devolver los mensajes 
        public readonly StringBuilder stringBuilder = new StringBuilder();
        private List<string> ArtPromocion = new List<string>();

        public void BucarTipoVenta(System.Windows.Forms.ComboBox comboBox)
        {
            DataTable dt =_D_Articulos.BucarTipoVenta();
            if(dt.Rows.Count > 0)
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

        public void CargarArticulos(System.Windows.Forms.DataGridView DgvArticulo, List<TB_ARTICULO> listaArticulos, string TipoTrabajo)
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
                var articulosObtenidos = _D_Articulos.ObtenerArticulos(TipoTrabajo,"",command);

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
                    _D_Articulos.stringBuilder.AppendLine("No se pudieron cargar los artículos correctamente.");
                    transaction.Rollback();
                }


            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                transaction.Rollback();
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

        public void FiltrarArticulos_Tap3(string filtro, List<TB_ARTICULO> listaArticulos, List<TB_ARTICULO> listaTemporal, System.Windows.Forms.TextBox Codigo, System.Windows.Forms.TextBox Descripcion, System.Windows.Forms.TextBox Precio, System.Windows.Forms.TextBox Cantidad)
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

        public void AgregarFila(DataGridView Dgv_Tap3_Articulo, string codArticulo, string descripcion, int cantidad, decimal precio, decimal descuento, decimal total, decimal impuesto, string ojo, string artPadre = "", string agregado = "NO" ,string AgreDer ="NO", string AgreIzq = "NO")
        {
          try {
                // Verificar y agregar columnas si no existen
                if (Dgv_Tap3_Articulo.Columns.Count == 0)
                {
                Dgv_Tap3_Articulo.Columns.Add("CodArticulo", "Código del Artículo");
                Dgv_Tap3_Articulo.Columns.Add("DESART", "Descripción");
                Dgv_Tap3_Articulo.Columns.Add("ART_EXIST", "Cantidad");
                Dgv_Tap3_Articulo.Columns.Add("ART_PVP", "Precio");
                Dgv_Tap3_Articulo.Columns.Add("PORCTDESCUENTO", "Descuento (%)");
                Dgv_Tap3_Articulo.Columns.Add("Total", "Total");
                Dgv_Tap3_Articulo.Columns.Add("Impuesto", "Impuesto");
                Dgv_Tap3_Articulo.Columns.Add("Ojo", "Ojo");
                // columnas opcionales
                Dgv_Tap3_Articulo.Columns.Add("ArtPadre", "Artículo Padre");
                Dgv_Tap3_Articulo.Columns.Add("Agregado", "Agregado");
                Dgv_Tap3_Articulo.Columns.Add("AgreDer", "AgreDer");
                Dgv_Tap3_Articulo.Columns.Add("AgreIzq", "AgreIzq");
                Dgv_Tap3_Articulo.Columns.Add("PrecioViejo", "PrecioViejo");
                    CrearObjetos(Dgv_Tap3_Articulo);

            }
                // FormatoDataGrivew
                Formato_Dgv_Carga_Articulo(Dgv_Tap3_Articulo);

                // Agregar la fila con los valores proporcionados
                Dgv_Tap3_Articulo.Rows.Add(codArticulo, descripcion, cantidad, precio, descuento, total, impuesto, ojo, artPadre, agregado, AgreDer, AgreIzq, precio);

            }
            catch (Exception ex)
            {
                // Lanzar una excepción personalizada para que sea manejada en la capa visual
                throw new Exception("Error al agregar una fila al DataGridView. Detalles: " + ex.Message, ex);
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
                    throw new ArgumentOutOfRangeException(nameof(numeroFila), "El número de fila está fuera del rango válido.");
                }

                // Validar que la columna exista
                if (!Dgv_Tap3_Articulo.Columns.Contains(nombreColumna))
                {
                    throw new ArgumentException($"La columna '{nombreColumna}' no existe en el DataGridView.", nameof(nombreColumna));
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
                    throw new ArgumentException($"La columna '{nombreColumna}' no existe en el DataGridView.", nameof(nombreColumna));
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

        private void Formato_Dgv_Carga_Articulo(DataGridView Dgv_Tap3_Articulo)
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
                Dgv_Tap3_Articulo.Columns["Eliminar"].HeaderText = "";


                //Ancho de columna
                Dgv_Tap3_Articulo.Columns["CodArticulo"].Width = 80;
                Dgv_Tap3_Articulo.Columns["DESART"].Width = 320;
                Dgv_Tap3_Articulo.Columns["ART_EXIST"].Width = 80;
                Dgv_Tap3_Articulo.Columns["ART_PVP"].Width = 100;
                Dgv_Tap3_Articulo.Columns["PORCTDESCUENTO"].Width = 100;
                Dgv_Tap3_Articulo.Columns["Total"].Width = 120;
                Dgv_Tap3_Articulo.Columns["Impuesto"].Width = 100;
                Dgv_Tap3_Articulo.Columns["Ojo"].Width = 70;
                Dgv_Tap3_Articulo.Columns["Eliminar"].Width = 90;
                // columnas opcionales
                Dgv_Tap3_Articulo.Columns["ArtPadre"].Width = 80;
                Dgv_Tap3_Articulo.Columns["Agregado"].Width = 40;

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
                // columnas opcionales
                Dgv_Tap3_Articulo.Columns["ArtPadre"].ReadOnly = true;
                Dgv_Tap3_Articulo.Columns["Agregado"].ReadOnly = true;

                Dgv_Tap3_Articulo.Columns["CodArticulo"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["DESART"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["ART_EXIST"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["ART_PVP"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["PORCTDESCUENTO"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["Total"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["Impuesto"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["Ojo"].SortMode = DataGridViewColumnSortMode.NotSortable;
                Dgv_Tap3_Articulo.Columns["Eliminar"].SortMode = DataGridViewColumnSortMode.NotSortable; 

               // columnas opcionales
               Dgv_Tap3_Articulo.Columns["ArtPadre"].SortMode = DataGridViewColumnSortMode.NotSortable;
               Dgv_Tap3_Articulo.Columns["Agregado"].SortMode = DataGridViewColumnSortMode.NotSortable;


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

        public bool CargarArticulo_ValidarPrecio( System.Windows.Forms.TextBox Precio)
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
            foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
            {
                if (row.Cells["CodArticulo"].Value?.ToString() == Codigo)
                {
                    //MessageBox.Show("El artículo ya está agregado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return true;
                }

                // Verificar si ya hay un cristal agregado
                if (row.Cells["CodArticulo"].Value.ToString().StartsWith("C") && Codigo.StartsWith("C"))
                {  
                    return true;
                }

            }

            return false;
        }

        public bool VerificarYBorrarArticulo(DataGridView gexFacturas, int filaActual)
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
                    foreach (DataGridViewRow fila in gexFacturas.Rows)
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

                    // Eliminar las filas recopiladas
                    foreach (int index in filasParaEliminar.OrderByDescending(i => i))
                    {
                        gexFacturas.Rows.RemoveAt(index);
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

        public string ValidarExistenciaProducto(string codigoProducto, int cantidadIngresada, List<TB_ARTICULO> listaArticulos)
        {
            if (codigoProducto.StartsWith("C"))
            {
                // Si es un cristal no realizo la validacion
                return "";
            }

            // Buscar el producto en la lista por su código
                var articulo = listaArticulos.FirstOrDefault(a => a.CodArticulo == codigoProducto);

            // Verificar si el producto existe en la lista
            if (articulo == null)
            {
                return "El producto no existe en la lista.";
            }

            // Verificar si el producto maneja existencia
            if (!articulo.MANEJAEXISTENCIA)
            {
                return "El producto no tiene existencia.";
            }

            // Comparar la cantidad ingresada con la existencia disponible
            if (cantidadIngresada > articulo.ART_EXIST)
            {
                return $"La cantidad ingresada {cantidadIngresada} excede la existencia disponible {articulo.ART_EXIST} .";
            }

            // Si todo es válido, devolver true
            return "";
        }

        public string ValidarCantidadMaximaPermitida(string codigoProducto, int cantidadIngresada)
        {
            DataTable DT_ValorMaximo = _D_Articulos.BucarArticuloMaximoPorVenta(codigoProducto.Substring(0, 1));

            foreach (DataRow row in DT_ValorMaximo.Rows)
            {
                int ValorMaximoPorArticulo = Convert.ToInt32(row["Max_Vta"].ToString());
                if (cantidadIngresada > ValorMaximoPorArticulo)
                {
                    return "El articulo " + codigoProducto + " tiene una cantidad a vender mayor que el maximo permitido.";
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
            // Validar que la lista no sea nula o vacía
            if (trabajos == null || trabajos.Count == 0)
            {
                throw new ArgumentException("La lista de trabajos no puede estar vacía.");
            }

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

            // Iterar sobre las filas del DataGridView base para calcular los totales
            foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
            {
                // Verificar que la fila no sea nueva
                if (row.IsNewRow) continue;

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
                    descuentoTotal += subtotal * (descuento / 100);
                }

                // Impuesto: Subtotal * (%Impuesto / 100)
                if (row.Cells["Impuesto"].Value != null)
                {
                    decimal impuesto = Convert.ToDecimal(row.Cells["Impuesto"].Value);
                    ivaTotal += subtotal * (impuesto / 100);
                }
            }

            // Calcular IGTF (por ejemplo, 2% del subtotal)
            igtfTotal = 0;

            // Calcular el total general
            totalGeneral = subtotal - descuentoTotal + ivaTotal + igtfTotal;

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


        public bool VerificoProductosPermitidos(string CodArticulo, string glbTipoTrabajo , DataGridView gridFacturas, Boolean UsuAsegurado= false)
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
                                        stringBuilder.AppendLine("No se permite tener Monturas y Lentes de Contacto en la misma orden.");
                                        return false;
                                    }
                                    break;

                                case "C": // Cristales
                                          // No permito cristales si ya existen lentes de contacto agregados
                                    if (gridFacturas.Rows[xx].Cells["CodArticulo"].Value != null &&
                                        gridFacturas.Rows[xx].Cells["CodArticulo"].Value.ToString().StartsWith("W"))
                                    {
                                        stringBuilder.AppendLine("No se permite tener Cristales y Lentes de Contacto en la misma orden.");
                                        return false;
                                    }
                                    break;

                                case "L": // Lentes de sol
                                    if (UsuAsegurado)
                                    {
                                        stringBuilder.AppendLine("No se permite facturar Lentes de Sol para Asegurados.");
                                        return false;
                                    }
                                    else
                                    {
                                        // No permito cristales si ya existen lentes de contacto agregados
                                        if (gridFacturas.Rows[xx].Cells["CodArticulo"].Value != null &&
                                            gridFacturas.Rows[xx].Cells["CodArticulo"].Value.ToString().StartsWith("M") &&
                                            glbTipoTrabajo == "002")
                                        {
                                            stringBuilder.AppendLine("No se permite facturar Lentes de Sol y Monturas en la misma orden.");
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
                                            stringBuilder.AppendLine("No se permite facturar Lentes de Contacto Desechables para Asegurados.");
                                            return false;
                                        }
                                    }

                                    // No permito cristales si ya existen lentes de contacto agregados
                                    if (gridFacturas.Rows[xx].Cells["CodArticulo"].Value != null &&
                                        gridFacturas.Rows[xx].Cells["CodArticulo"].Value.ToString().StartsWith("C")) // Cristales
                                    {
                                        stringBuilder.AppendLine("No se permite tener Lentes de Contacto y Cristales en la misma orden.");
                                        return false;
                                    }
                                    else if (gridFacturas.Rows[xx].Cells["CodArticulo"].Value != null &&
                                             gridFacturas.Rows[xx].Cells["CodArticulo"].Value.ToString().StartsWith("M")) // Monturas
                                    {
                                        stringBuilder.AppendLine("No se permite tener Lentes de Contacto y Monturas en la misma orden.");
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

        public void CargarServicioOPrima(DataGridView gridFacturas,string tipoServicio, int NumeroExamen = 0, string Nacionalidad = null, string txtCedula = null, string txtOjo = null)
        {
            try
            {
                bool tieneServicio = false;
                bool found = false;
                decimal montoTotal = 0;
                decimal prima = 0;
                int cantidad = 0;
                string codigo = "";
                string PrismaD = "0";
                string PrismaI = "0";

             DataSet dsExamenConPrisma = _D_Articulos.ValidarExamenConPrisma(NumeroExamen, Nacionalidad, txtCedula);

            if (dsExamenConPrisma.Tables[0].Rows.Count == 0)
            {
                        return; // No hay datos de prisma
            }

            PrismaD = dsExamenConPrisma.Tables[0].Rows[0]["PrismaD"].ToString();
            PrismaI = dsExamenConPrisma.Tables[0].Rows[0]["PrismaI"].ToString();

                    if (!string.IsNullOrEmpty(PrismaD) && PrismaD != "0") cantidad++;
                    if (!string.IsNullOrEmpty(PrismaI) && PrismaI != "0") cantidad++;

                if (PrismaD == "0" || PrismaI == "0")
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
                        cantidad = Convert.ToInt32(gridFacturas.Rows[x].Cells["ART_EXIST"].Value);
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
                            TB_ARTICULO articulo = articulos.First();
                            decimal precio = articulo.ART_PVP;
                            decimal total = precio * cantidad;
                            decimal impuesto = articulo.ART_EXENTO ? 0 : BuscarIva("I");

                           AgregarFila(gridFacturas, articulo.CodArticulo, articulo.DESART, cantidad, (decimal)precio, (decimal)articulo.PORCTDESCUENTO, (decimal)total, impuesto, txtOjo, codigo);
          
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

        private void EjecutarAccionServicioAgregado(DataGridView gridFacturas, int fila, int filaServicioAgregado, string ladoOjo, string TipoTrabajo ,SqlCommand sqlCom = null)
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
                                    decimal impuesto = articulo.ART_EXENTO ? 0 : BuscarIva("I");
                                    // Agregar nueva fila al DataGridView
                                    AgregarFila(gridFacturas, articulo.CodArticulo, articulo.DESART, cantidad, (decimal)precio, (decimal)articulo.PORCTDESCUENTO, (decimal)total, impuesto, ladoOjo, codPadre,"SI", AgreDer, AgreIzq);
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
                        throw new Exception("No se encontró la clase 'EvalClase' en el ensamblado.");
                    }

                    // Obtener el método Eval de la clase
                    var metodoEval = oClass.GetMethod("Eval");

                    if (metodoEval == null)
                    {
                        throw new Exception("No se encontró el método 'Eval' en la clase 'EvalClase'.");
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

        public void EvaluoServicioAgregado(DataGridView gridFacturas, int fila, string ladoOjo, int NunExamen,string TipoTrabajo ,string Nacionalidad, string txtCedula)
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
                // Determinar el lado del ojo basado en los valores del DataGridView
                var ojo = gridFacturas.Rows[fila].Cells["Ojo"].Value?.ToString();
                if (ojo == "D")
                {
                    ladoOjo = "D";
                }
                else if (ojo == "I")
                {
                    ladoOjo = "I";
                }
                else if (ojo == "A")
                {
                    ladoOjo = "Ambos";
                }

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

                                // Obtener datos del examen utilizando la nueva función
                                List<TB_Examen> examenes = _D_Articulos.ObtenerExamen(Nacionalidad, txtCedula, _D_Inicio.Sucursal(), NunExamen);

                                // Validar si se obtuvieron resultados
                                if (examenes != null && examenes.Count > 0)
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
                                        EjecutarAccionServicioAgregado(gridFacturas, fila, x, ladoOjo, TipoTrabajo);
                                    }
                                }
                                else
                                {
                                    // Manejar el caso en que no se obtuvieron resultados
                                    MessageBox.Show("No se encontraron datos del examen.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private bool ServicioColoracion(DataGridView Dgv_Tap3_Articulo, DataGridView Dvg_Coloracion, System.Windows.Forms.RadioButton Rd_FullColor)
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
                        if (row.Cells["CodArticulo"].Value != null && row.Cells["CodArticulo"].Value.ToString() == "S000004")
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
                                        DataRow drColoracion = dtColoracion.NewRow();
                                        drColoracion["Cod_Coloracion"] = dr["Cod_Coloracion"];
                                        drColoracion["Desc_Color"] = dr["Desc_Color"];
                                        drColoracion["Porc_Material"] = dr["Porc_Material"];
                                        dtColoracion.Rows.Add(drColoracion);
                                    }

                                    Dvg_Coloracion.DataSource = dtColoracion;
                                }

                            break; // Salir del bucle al procesar el servicio
                        }
                    }

                return true;
            }
            catch (Exception ex)
            {
                return false;
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

        public bool CalculoDescuento(string precioMontoTotal, System.Windows.Forms.DataGridView DgvArticulo , string TipoDescuento, string DescMax, System.Windows.Forms.TextBox Porce_Descuento , System.Windows.Forms.TextBox Monto_Descuento, System.Windows.Forms.TextBox txtMotivo)
        {
            try
            {
                // Total de la compra 
                //string precioMontoTotal;
                DataSet dsDesc;

                if (TipoDescuento=="Descuento Global")
                {
                    if (!string.IsNullOrEmpty(Porce_Descuento.Text))
                    {
                        if (Convert.ToDecimal(Porce_Descuento.Text) > 100)
                        {

                            stringBuilder.Append("El monto del descuento no puede ser mayor a 100%");
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
                                    stringBuilder.Append($"La marca { dsDesc.Tables[0].Rows[0][0]} no permite este % de descuento");
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
                            stringBuilder.Append("El monto del descuento no puede ser mayor a 100%");
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

    }
}

