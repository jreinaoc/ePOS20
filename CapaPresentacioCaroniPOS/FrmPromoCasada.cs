using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaDatos.ListaOrdenes_Datos;
using CapaDatos.Inicio_Datos;
using CapaEntidades;



namespace CapaVisual_Login
{
    public partial class FrmPromoCasada : Form
    {
        public FrmPromoCasada()
        {
            InitializeComponent();
        }

        private D_ListaOrdenes _D_ListaOrdenes = new D_ListaOrdenes();
        D_Inicio _D_Inicio = new D_Inicio();
        private Dictionary<string, PromocionConfig> _configPromociones;
        private const string COL_SELECCION = "E"; // Nombre de la columna checkbox
        private string promocionActual = "";
        // Clase para almacenar la configuración de promociones
        public class PromocionConfig
        {
            public string CodigoPromocion { get; set; }
            public string Nombre_Promo { get; set; }
            public string StoredProcedure { get; set; }
            public int MinOrdenes { get; set; }
            public int MaxOrdenes { get; set; }
        }

        private void CargarPromocionesEnComboBox()
        {
            try
            {
                // Verificar que el ComboBox existe y no está disposed
                if (Cbx_Promo == null || Cbx_Promo.IsDisposed)
                {
                    MessageBox.Show("Error: El ComboBox no está disponible");
                    return;
                }

                // Verificar si necesitamos invocar en el hilo UI
                if (Cbx_Promo.InvokeRequired)
                {
                    Cbx_Promo.Invoke(new MethodInvoker(CargarPromocionesEnComboBox));
                    return;
                }

                // Cargar configuración si no está cargada
                if (_configPromociones == null || _configPromociones.Count == 0)
                {
                    CargarConfiguracionPromociones();

                    // Verificar nuevamente después de cargar
                    if (_configPromociones == null || _configPromociones.Count == 0)
                    {
                        mostrarError("No hay promociones configuradas");
                        return;
                    }
                }

                // Configurar ComboBox con BeginUpdate/EndUpdate para mejor rendimiento
                Cbx_Promo.BeginUpdate();
                try
                {
                    Cbx_Promo.DisplayMember = "Text";
                    Cbx_Promo.ValueMember = "Value";
                    Cbx_Promo.Items.Clear();

                    // Llenar el ComboBox
                    foreach (var promocion in _configPromociones)
                    {
                        if (promocion.Value == null) continue;

                        PromocionConfig config = promocion.Value;

                        if (string.IsNullOrEmpty(config.Nombre_Promo) ||
                            string.IsNullOrEmpty(config.CodigoPromocion))
                        {
                            continue;
                        }

                        Cbx_Promo.Items.Add(new
                        {
                            Text = config.Nombre_Promo,
                            Value = config.CodigoPromocion
                        });
                    }

                    // Seleccionar el primer item si hay elementos
                    if (Cbx_Promo.Items.Count > 0)
                    {
                        Cbx_Promo.SelectedIndex = 0;
                        var selectedItem = Cbx_Promo.SelectedItem;
                        dynamic item = selectedItem;
                        promocionActual = item.Text;
                    }
                }
                finally
                {
                    Cbx_Promo.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                mostrarError($"Error al cargar promociones: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void FrmPromoCasada_Load(object sender, EventArgs e)
        {
            try
            {
                CargarConfiguracionPromociones();
                CargarPromocionesEnComboBox();
            }
            catch (Exception ex)
            {
                mostrarError("Error al cargar las órdenes: " + ex.Message);
            }
        }

        private void mostrarError(string mensaje)
        {
            FrmMensajes.MostrarError(mensaje);
        }

        private DialogResult mostrarPregunta(string mensaje, string titulo)
        {
            return FrmMensajes.MostrarPregunta(mensaje, titulo);
        }
        private void PrepararGridParaNuevaCarga()
        {
            Dgv_ListOsCasadas.SuspendLayout();

            // 1. Limpiar el DataSource correctamente
            Dgv_ListOsCasadas.DataSource = null;

            // 2. Limpiar filas pero mantener columnas existentes
            Dgv_ListOsCasadas.Rows.Clear();

            // 3. Eliminar solo la columna "E" si existe
            if (Dgv_ListOsCasadas.Columns.Contains("E"))
            {
                Dgv_ListOsCasadas.Columns.Remove("E");
            }

            Dgv_ListOsCasadas.ResumeLayout();
        }
        private void CargarOrdenesDisponibles(string CodigoPromoActiva)
        {
            DateTime currentDate = _D_Inicio.DiaActivo();
            string formattedDate = currentDate.ToString("yyyyMMdd");

            PrepararGridParaNuevaCarga();

            DataSet Ordenesrango = _D_ListaOrdenes.CargarOrdenesPromo(TB_USUARIO.COD_USR, formattedDate, CodigoPromoActiva);


            if (Ordenesrango.Tables.Count > 0 && Ordenesrango.Tables[0].Rows.Count > 0)
            {
                Dgv_ListOsCasadas.DataSource = Ordenesrango.Tables[0];

                // Verificar si la columna ya existe antes de crearla
                if (Dgv_ListOsCasadas.Columns["E"] == null)
                {
                    CrearObjeto_GarantiaGrid(Dgv_ListOsCasadas);
                }

                ConfigurarDataGridViewOrdenes();
            }
            else
            {
                mostrarError("No se encontraron órdenes disponibles para promociones");
            }
        }

        private void btAplicarPromo_Click(object sender, EventArgs e)
        {
            try
            {

                // Validar que hay órdenes seleccionadas en el DataGridView
                if (Dgv_ListOsCasadas.SelectedRows.Count == 0)
                {
                    mostrarError("Debe seleccionar al menos una orden para aplicar la promoción");

                    return;
                }

                // Recoger las órdenes seleccionadas del DataGridView
                List<string> ordenesSeleccionadas = new List<string>();

                //foreach (DataGridViewRow row in Dgv_ListOsCasadas.SelectedRows)
                //{
                //    if (row.Cells["Orden"].Value != null)
                //    {
                //        ordenesSeleccionadas.Add(row.Cells["Orden"].Value.ToString());
                //    }
                //}

                // Verificar que el DataGridView tiene datos
                if (Dgv_ListOsCasadas.Rows.Count > 0)
                {
                    // Recorrer todas las filas (excepto la fila nueva si está en modo edición)
                    foreach (DataGridViewRow row in Dgv_ListOsCasadas.Rows)
                    {
                        // Asegurarse que no es la fila para nuevos registros
                        if (!row.IsNewRow)
                        {
                            // Verificar que la columna "E" existe
                            if (row.Cells["E"] != null && row.Cells["E"].Value != null)
                            {
                                // Verificar si el CheckBox está marcado
                                bool estaMarcado = false;

                                // Manejar diferentes tipos de valores del CheckBox
                                if (row.Cells["E"].Value is bool)
                                {
                                    estaMarcado = (bool)row.Cells["E"].Value;
                                }
                                else
                                {
                                    // Intentar convertir si no es bool directamente
                                    bool.TryParse(row.Cells["E"].Value.ToString(), out estaMarcado);
                                }

                                // Si está marcado y tiene orden, agregar a la lista
                                if (estaMarcado && row.Cells["Orden"].Value != null)
                                {
                                    string numeroOrden = row.Cells["Orden"].Value.ToString();
                                    if (!string.IsNullOrEmpty(numeroOrden))
                                    {
                                        ordenesSeleccionadas.Add(numeroOrden);
                                    }
                                }
                            }
                        }
                    }
                }


                // Construir mensaje de confirmación
                string mensajeConfirmacion = ConstruirMensajeConfirmacion(promocionActual, ordenesSeleccionadas);

                // Mostrar diálogo de confirmación
                DialogResult opcion = mostrarPregunta(mensajeConfirmacion, "Verifique por favor");

                if (opcion == DialogResult.OK)
                {
                    // Construir parámetros para el stored procedure
                    string parametros = string.Join("', '", ordenesSeleccionadas);

                    // Obtener el nombre del stored procedure según la promoción
                    string storedProcedure = ObtenerStoredProcedure(promocionActual);

                    // Ejecutar el stored procedure genérico
                    DataSet dsConsulta = _D_ListaOrdenes.EjecutaStoreProcedure(storedProcedure, parametros);

                    // Verificar resultado y actualizar DataGridView
                    if (dsConsulta.Tables.Count > 0 && dsConsulta.Tables[0].Rows[0]["Promocion"].ToString() == "APLICA")
                    {
                                mostrarError("La promoción fue aplicada satisfactoriamente");

                    }
                    else
                    {
                              
                        mostrarError("La promoción no fue aplicada");
                    }

                    dynamic selectedItem = Cbx_Promo.SelectedItem;
                    string nombre = selectedItem.Text;
                    string codigo = selectedItem.Value;
                    CargarOrdenesDisponibles(codigo);
                }
            }
            catch (Exception ex)
            {
                mostrarError("Error al aplicar la promoción: " + ex.Message);
            }
        }

        private string ConstruirMensajeConfirmacion(string promocion, List<string> ordenes)
        {
            StringBuilder mensaje = new StringBuilder();
            mensaje.Append("¿Desea aplicar la promoción ");
            mensaje.Append(promocion);
            mensaje.Append(" a ");

            if (ordenes.Count == 1)
            {
                mensaje.Append("la orden ");
                mensaje.Append(ordenes[0]);
            }
            else
            {
                mensaje.Append("las siguientes órdenes: ");
                mensaje.Append(string.Join(", ", ordenes));
            }

            mensaje.Append("?");
            return mensaje.ToString();
        }
        public void CrearObjeto_GarantiaGrid(DataGridView DgvGarantia)
        {

            DataGridViewCheckBoxColumn CheckBoxColumn = new DataGridViewCheckBoxColumn();
            CheckBoxColumn.Name = "E";
            CheckBoxColumn.Width = 40;
            CheckBoxColumn.HeaderText = "E";
            DgvGarantia.Columns.Add(CheckBoxColumn);


        }

        private void ConfigurarDataGridViewOrdenes()
        {
            // Configuración para la visualización de órdenes disponibles
            Dgv_ListOsCasadas.AutoGenerateColumns = false;
            // No puedad cambiar el tamaño de las columnas
            Dgv_ListOsCasadas.AllowUserToResizeColumns = false;

            //Dgv_ListOsCasadas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //Dgv_ListOsCasadas.AllowUserToAddRows = false;
            //Dgv_ListOsCasadas.ReadOnly = true;
            Dgv_ListOsCasadas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Dgv_ListOsCasadas.MultiSelect = true;


            //Centrar todas las colucnas 
            Dgv_ListOsCasadas.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv_ListOsCasadas.ScrollBars = ScrollBars.Both;

            Dgv_ListOsCasadas.Columns["E"].DisplayIndex = 0;

            // Personalizar columnas específicas si es necesario
            if (Dgv_ListOsCasadas.Columns.Contains("Orden"))
            {
                Dgv_ListOsCasadas.Columns["Orden"].HeaderText = "N° Orden";
                Dgv_ListOsCasadas.Columns["Orden"].Width = 70;
                Dgv_ListOsCasadas.Columns["Orden"].ReadOnly = true;
                Dgv_ListOsCasadas.Columns["Orden"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (Dgv_ListOsCasadas.Columns.Contains("Cedula"))
            {
                Dgv_ListOsCasadas.Columns["Cedula"].HeaderText = "Cedula";
                Dgv_ListOsCasadas.Columns["Cedula"].Width = 80;
                Dgv_ListOsCasadas.Columns["Cedula"].ReadOnly = true;
                Dgv_ListOsCasadas.Columns["Cedula"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (Dgv_ListOsCasadas.Columns.Contains("Cliente"))
            {
                Dgv_ListOsCasadas.Columns["Cliente"].HeaderText = "Cliente";
                Dgv_ListOsCasadas.Columns["Cliente"].Width = 120;
                Dgv_ListOsCasadas.Columns["Cliente"].ReadOnly = true;
                Dgv_ListOsCasadas.Columns["Cliente"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (Dgv_ListOsCasadas.Columns.Contains("Vendedor"))
            {
                Dgv_ListOsCasadas.Columns["Vendedor"].HeaderText = "Vendedor";
                Dgv_ListOsCasadas.Columns["Vendedor"].Width = 70;
                Dgv_ListOsCasadas.Columns["Vendedor"].ReadOnly = true;
                Dgv_ListOsCasadas.Columns["Vendedor"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (Dgv_ListOsCasadas.Columns.Contains("MontoTotal"))
            {
                Dgv_ListOsCasadas.Columns["MontoTotal"].HeaderText = "Monto Total";
                Dgv_ListOsCasadas.Columns["MontoTotal"].Width = 80;
                Dgv_ListOsCasadas.Columns["MontoTotal"].ReadOnly = true;
                Dgv_ListOsCasadas.Columns["MontoTotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Dgv_ListOsCasadas.Columns["MontoTotal"].DefaultCellStyle.Format = "N2"; // Formato de 2 decimales y unidades de mil
            }

            if (Dgv_ListOsCasadas.Columns.Contains("Saldo"))
            {
                Dgv_ListOsCasadas.Columns["Saldo"].HeaderText = "Saldo";
                Dgv_ListOsCasadas.Columns["Saldo"].Width = 80;
                Dgv_ListOsCasadas.Columns["Saldo"].ReadOnly = true;
                // Dgv_ListOsCasadas.Columns["Saldo"].Visible = false;
                Dgv_ListOsCasadas.Columns["Saldo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Dgv_ListOsCasadas.Columns["Saldo"].DefaultCellStyle.Format = "N2"; // Formato de 2 decimales y unidades de mil
            }

            if (Dgv_ListOsCasadas.Columns.Contains("Vision"))
            {
                Dgv_ListOsCasadas.Columns["Vision"].HeaderText = "Vision";
                Dgv_ListOsCasadas.Columns["Vision"].Width = 80;
                Dgv_ListOsCasadas.Columns["Vision"].ReadOnly = true;
                Dgv_ListOsCasadas.Columns["Vision"].Visible = false;
            }

            if (Dgv_ListOsCasadas.Columns.Contains("TipoVenta"))
            {
                Dgv_ListOsCasadas.Columns["TipoVenta"].HeaderText = "TipoVenta";
                Dgv_ListOsCasadas.Columns["TipoVenta"].Width = 80;
                Dgv_ListOsCasadas.Columns["TipoVenta"].ReadOnly = true;
                Dgv_ListOsCasadas.Columns["TipoVenta"].Visible = false;
            }

            if (Dgv_ListOsCasadas.Columns.Contains("PROMOCION"))
            {
                Dgv_ListOsCasadas.Columns["PROMOCION"].HeaderText = "PROMOCION";
                Dgv_ListOsCasadas.Columns["PROMOCION"].Width = 80;
                Dgv_ListOsCasadas.Columns["PROMOCION"].ReadOnly = true;
                Dgv_ListOsCasadas.Columns["PROMOCION"].Visible = false;
            }
        }

        private void ConfigurarDataGridViewResultados()
        {
            // Configuración especial para mostrar resultados de promociones
            Dgv_ListOsCasadas.AutoGenerateColumns = true;
            Dgv_ListOsCasadas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Agregar estilo condicional si es necesario
            if (Dgv_ListOsCasadas.Columns.Contains("Estado"))
            {
                Dgv_ListOsCasadas.Columns["Estado"].DefaultCellStyle.ForeColor = Color.Blue;
                Dgv_ListOsCasadas.Columns["Estado"].DefaultCellStyle.Font = new Font(Dgv_ListOsCasadas.Font, FontStyle.Bold);
            }
        }

        private string ObtenerStoredProcedure(string promocion)
        {
            try
            {
                // Validación básica de parámetros
                if (string.IsNullOrWhiteSpace(promocion))
                {
                    mostrarError("El nombre de la promoción no puede estar vacío");
                    return null;
                }

                // Verificar que el diccionario está inicializado y tiene datos
                if (_configPromociones == null || _configPromociones.Count == 0)
                {
                    mostrarError("La configuración de promociones no está cargada");
                    return null;
                }

                // Recorrer el diccionario buscando coincidencia
                foreach (var item in _configPromociones)
                {
                    // Comparación insensible a mayúsculas/minúsculas y sin espacios
                    if (item.Value.Nombre_Promo != null &&
                        item.Value.Nombre_Promo.Trim().Equals(promocion.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        if (!string.IsNullOrWhiteSpace(item.Value.StoredProcedure))
                        {
                            return item.Value.StoredProcedure;
                        }
                        else
                        {
                            mostrarError($"La promoción '{promocion}' no tiene stored procedure configurado");
                            return null;
                        }
                    }
                }

                mostrarError($"No se encontró configuración para la promoción: {promocion}");
                return null;
            }
            catch (Exception ex)
            {
                mostrarError($"Error al obtener stored procedure: {ex.Message}");
                return null;
            }
        }

        private void CargarConfiguracionPromociones()
        {
            try
            {
                _configPromociones = new Dictionary<string, PromocionConfig>();

                // Obtener configuración de promociones desde la base de datos
                DataSet dsConfig = _D_ListaOrdenes.CargarInformacionPromoCasadas(_D_Inicio.DiaActivo().ToString("yyyyMMdd"));

                if (dsConfig != null && dsConfig.Tables.Count > 0)
                {
                    foreach (DataRow row in dsConfig.Tables[0].Rows)
                    {
                        string CodigoPromocion = row["cod_prom"].ToString();
                        string Nombrepromo = row["Prom_DESCRIP"].ToString();
                        string nombrePromocion = row["Prom_DESCRIP"].ToString();
                        string storedProcedure = row["Nombre_Store_Promo"].ToString();
                        int minOrdenes = Convert.ToInt32(row["Cantidad_Minima_Ordenes"]);
                        int maxOrdenes = Convert.ToInt32(row["Cantidad_Maxima_Ordenes"]);

                        _configPromociones[nombrePromocion] = new PromocionConfig
                        {
                            CodigoPromocion = CodigoPromocion,
                            Nombre_Promo = Nombrepromo,
                            StoredProcedure = storedProcedure,
                            MinOrdenes = minOrdenes,
                            MaxOrdenes = maxOrdenes
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                // Loggear error pero continuar con valores por defecto
                mostrarError("Error al cargar configuración de promociones: " + ex.Message);
            }
        }

        private void Dgv_ListOsCasadas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Solo procesar clicks en la columna de checkbox y que no sea el header
            if (e.ColumnIndex == Dgv_ListOsCasadas.Columns[COL_SELECCION].Index)
            {
                ProcesarSeleccionOrden(e.RowIndex);
            }
        }

        private void ProcesarSeleccionOrden(int rowIndex)
        {
            try
            {
                // Validar índice de fila
                if (rowIndex < 0 || rowIndex >= Dgv_ListOsCasadas.Rows.Count)
                    return;

                // Cargar configuración si no está cargada
                if (_configPromociones == null)
                {
                    CargarConfiguracionPromociones();
                }

                // Obtener configuración
                var config = ObtenerConfiguracionPromocion(promocionActual);
                if (config == null)
                {
                    mostrarError("Configuración de promoción no encontrada");
                    return;
                }

                // Forzar el fin de la edición para actualizar el valor
                if (Dgv_ListOsCasadas.IsCurrentCellInEditMode)
                {
                    Dgv_ListOsCasadas.EndEdit();
                }

                // Obtener estado ACTUAL del checkbox (después de EndEdit)
                bool estaSeleccionada = false;
                if (Dgv_ListOsCasadas.Rows[rowIndex].Cells[COL_SELECCION].Value != null)
                {
                    estaSeleccionada = Convert.ToBoolean(Dgv_ListOsCasadas.Rows[rowIndex].Cells[COL_SELECCION].Value);
                }

                // Contar órdenes actualmente seleccionadas
                int cantSeleccionadas = ContarOrdenesSeleccionadas();

                // Validar límite máximo ANTES de cambiar
                if (cantSeleccionadas > config.MaxOrdenes)
                {
                    mostrarError($"Máximo {config.MaxOrdenes} órdenes pueden ser seleccionadas");
                    Dgv_ListOsCasadas.Rows[rowIndex].Cells[COL_SELECCION].Value = false;
                    btAplicarPromo.Enabled = cantSeleccionadas >= config.MinOrdenes;
                    return;
                }

                // Alternar selección (el valor ya fue cambiado por el usuario, solo actualizamos lógica)
                cantSeleccionadas += estaSeleccionada ? -1 : 1;

                // Controlar estado del botón de aplicar
                btAplicarPromo.Enabled = cantSeleccionadas >= config.MinOrdenes;
            }
            catch (Exception ex)
            {
                mostrarError("Error al procesar selección: " + ex.Message);
            }
        }

        private int ContarOrdenesSeleccionadas()
        {
            int count = 0;
            foreach (DataGridViewRow row in Dgv_ListOsCasadas.Rows)
            {
                if (!row.IsNewRow && row.Cells[COL_SELECCION].Value != null &&
                    Convert.ToBoolean(row.Cells[COL_SELECCION].Value))
                {
                    count++;
                }
            }
            return count;
        }

        private PromocionConfig ObtenerConfiguracionPromocion(string nombrePromocion)
        {
            // Si existe configuración específica, devolverla
            if (_configPromociones != null && _configPromociones.TryGetValue(nombrePromocion, out var config))
            {
                return config;
            }

            // Valores por defecto si no hay configuración
            return new PromocionConfig
            {
                MinOrdenes = 1,
                MaxOrdenes = 2,
                StoredProcedure = ""
            };
        }

        private void Cbx_Promociones_SelectedValueChanged_1(object sender, EventArgs e)
        {
            if (Cbx_Promo.SelectedItem != null)
            {
                dynamic selectedItem = Cbx_Promo.SelectedItem;
                string nombre = selectedItem.Text;
                string codigo = selectedItem.Value;
                CargarOrdenesDisponibles(codigo);
            }
        }

        private void Dgv_ListOsCasadas_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (Dgv_ListOsCasadas.IsCurrentCellDirty &&
        Dgv_ListOsCasadas.CurrentCell.ColumnIndex == Dgv_ListOsCasadas.Columns[COL_SELECCION].Index)
            {
                Dgv_ListOsCasadas.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
    }

}
