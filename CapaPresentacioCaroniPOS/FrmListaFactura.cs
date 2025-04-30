using CapaLogica.ListaFactura_Logica;
using CapaLogica.ListaOrden_Logica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using classUtilities;
using CapaEntidades;
using CapaLogica.ExportarArchivos;
using System.IO;
using CapaLogica.Servicios;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace CapaVisual_Login
{
    public partial class FrmListaFactura : Form
    {
        public FrmListaFactura()
        {
            InitializeComponent();
        }

        DataSet Dts;
        private L_ListaOrdenes _ListaOrdenes = new L_ListaOrdenes();
        private L_ListaFacturas _L_ListaFacturas = new L_ListaFacturas();

        int PaginaInico = 1, Indice = 0, NUmeroFilas = 12, PaginaFinal;

        private FrmMensajes _FrmMensajes = new FrmMensajes();

        private async void Btnlupa_Click(object sender, EventArgs e)
        {
            //   LimpiarGrid();

            // if (CbxEstatus.SelectedIndex <= 0)
            //{
            //    Dts = _L_ListaFacturas.TraerFacturasRango(DtpDesde, DtpHasta);

            //}
            //else
            //{
            //    Dts = _L_ListaFacturas.TraerNotasRango(DtpDesde, DtpHasta);
            //}

            //if (Dts != null)
            //{
            //    DgvListaFacturas1.DataSource = Dts.Tables[0];
            //    Paginado(Dts);
            //    Paginado_Habilitar(true);
            //}
            //else
            //{
            //    Paginado_Habilitar(false);
            //}


            //if (DgvListaFacturas1.Rows.Count > 0)
            //{
            //    DgvListaFacturas1.Visible = true;
            //    EstructuraGrid();
            //}
            //else
            //{
            //    DgvListaFacturas1.Visible = false;
            //}

            try
            {
                LimpiarGrid();

                bool esNotaCredito = CbxEstatus.SelectedIndex > 0;

                if (esNotaCredito)
                {
                    Dts = await _L_ListaFacturas.TraerNotasRango(DtpDesde, DtpHasta);
                }
                else
                {
                    Dts = await _L_ListaFacturas.TraerFacturasRango(DtpDesde, DtpHasta);
                }

                if (Dts != null && Dts.Tables[0].Rows.Count > 0)
                {
                    DgvListaFacturas1.DataSource = Dts.Tables[0];
                    Paginado(Dts);
                    Paginado_Habilitar(true);
                    DgvListaFacturas1.Visible = true;
                    EstructuraGrid();
                }
                else
                {
                    DgvListaFacturas1.Visible = false;
                    Paginado_Habilitar(false);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error");
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }


        }

        private void LimpiarGrid()
        {
            try
            {
                //limpiar el grid 

                DgvListaFacturas1.DataSource = "";
                DgvListaFacturas1.DataMember = "";
            }

            catch (Exception ex)
            {
                //MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }

        }

        private async void cbPagina_Ini_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int Pagina = Convert.ToInt32(cbPagina_Ini.SelectedIndex + 1);
            Indice = Pagina - 1;
            PaginaInico = ((Pagina - 1) * NUmeroFilas) + 1;
            PaginaFinal = Pagina * NUmeroFilas;


            LimpiarGrid();

            Dts = await _L_ListaFacturas.TraerFacturasRango(DtpDesde, DtpHasta, PaginaInico, PaginaFinal);
            if (Dts != null)
            {
                DgvListaFacturas1.DataSource = Dts.Tables[0];
                Paginado(Dts);
                Paginado_Habilitar(true);
            }
            else
            {
                Paginado_Habilitar(false);
            }

            if (DgvListaFacturas1.Rows.Count > 0)
            {
                DgvListaFacturas1.Visible = true;
                EstructuraGrid();
            }
            else
            {
                DgvListaFacturas1.Visible = false;

            }

        }

        private void FrmListaFactura_Load(object sender, EventArgs e)
        {
            this.DgvListaFacturas1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            this.DgvListaFacturas1.RowTemplate.Height = 30;


            //Dts = _L_ListaFacturas.TraerFacturasRango(DtpDesde, DtpHasta);
            //if (Dts != null)
            //{
            //    DgvListaFacturas1.DataSource = Dts.Tables[0];
            //    Paginado(Dts);
            //    Paginado_Habilitar(true);
            //}
            //else
            //{
            //    Paginado_Habilitar(false);
            //}


            //if (DgvListaFacturas1.Rows.Count > 0)
            //{
            //    DgvListaFacturas1.Visible = true;
            //    EstructuraGrid();
            //}
            //else
            //{
            //    DgvListaFacturas1.Visible = false;
            //}

            //DgvListaFacturas1.Invalidate();

            // No cargar los datos aquí, solo configurar el DataGrid.
            DgvListaFacturas1.Visible = false;
            Paginado_Habilitar(false);

        }

        private void DgvListaFacturas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Paginado(DataSet Dts)
        {
            PaginaFinal = NUmeroFilas;
            int Pagina = Convert.ToInt32(cbPagina_Ini.SelectedIndex + 1);

            if (Pagina == 0)
                Pagina = 1;

            //Numero Filas
            Int32 Cantidad = Convert.ToInt32(Dts.Tables[1].Rows[0][0]) / NUmeroFilas;

            if (Convert.ToInt32(Dts.Tables[1].Rows[0][0]) % NUmeroFilas > 0)
                Cantidad++;
            txtPagina_Fin.Text = Cantidad.ToString();
            cbPagina_Ini.Items.Clear();

            for (int x = 1; x <= Cantidad; x++)
            {
                cbPagina_Ini.Items.Add(x.ToString());
            }

            if (Pagina > Cantidad)
            {
                cbPagina_Ini.SelectedIndex = 0;
            }
            else
            {
                Indice = Pagina - 1;
                cbPagina_Ini.SelectedIndex = Indice;
            }



        }

        private void Paginado_Habilitar(Boolean Habilitar)
        {
            label11.Visible = Habilitar;
            cbPagina_Ini.Visible = Habilitar;
            label12.Visible = Habilitar;
            txtPagina_Fin.Visible = Habilitar;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            uExportar.exportToExcel(Dts, true);
        }

        private void DtpHasta_ValueChanged(object sender, EventArgs e)
        {

        }

        public void EstructuraGrid()
        {
            //Con esta funcion se le da la estructura a las columnas del grid 
            try
            {

                //Centrar todas las columnas 
                DgvListaFacturas1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


                Padding newPadding = new Padding(15, 15, 15, 15);
                DgvListaFacturas1.ColumnHeadersDefaultCellStyle.Padding = newPadding;


                //asignar Nombres a cada columna 
                DgvListaFacturas1.Columns["NumeroFactura"].HeaderText = "N° de Factura";

                if (DgvListaFacturas1.Columns.Contains("NotaCredito"))
                {
                    DgvListaFacturas1.Columns["NotaCredito"].HeaderText = "Nota de Crédito";
                }

                DgvListaFacturas1.Columns["CedulaCliente"].HeaderText = "N° de Cédula";
                DgvListaFacturas1.Columns["FactSub"].HeaderText = "Sub Total";
                DgvListaFacturas1.Columns["FactImpuesto"].HeaderText = "Impuesto";
                DgvListaFacturas1.Columns["FactIGTF"].HeaderText = "IGTF";
                DgvListaFacturas1.Columns["FactTotal"].HeaderText = "Total";
                DgvListaFacturas1.Columns["Fecha"].HeaderText = "Fecha";

                //Ancho de columna
                DgvListaFacturas1.Columns["NumeroFactura"].Width = 110;
                DgvListaFacturas1.Columns["CedulaCliente"].Width = 110;
                DgvListaFacturas1.Columns["FactSub"].Width = 110;
                DgvListaFacturas1.Columns["FactImpuesto"].Width = 110;
                DgvListaFacturas1.Columns["FactIGTF"].Width = 80;
                DgvListaFacturas1.Columns["FactTotal"].Width = 110;
                DgvListaFacturas1.Columns["Fecha"].Width = 140;

                //Bloquear Columna 
                DgvListaFacturas1.Columns["NumeroFactura"].ReadOnly = true;
                DgvListaFacturas1.Columns["CedulaCliente"].ReadOnly = true;
                DgvListaFacturas1.Columns["FactSub"].ReadOnly = true;
                DgvListaFacturas1.Columns["FactImpuesto"].ReadOnly = true;
                DgvListaFacturas1.Columns["FactIGTF"].ReadOnly = true;
                DgvListaFacturas1.Columns["FactTotal"].ReadOnly = true;
                DgvListaFacturas1.Columns["Fecha"].ReadOnly = true;

                //ordenar las colunmnas del grid 
                DgvListaFacturas1.Columns["NumeroFactura"].DisplayIndex = 0;
                // Verificar si existe la columna NotaCredito antes de moverla
                if (DgvListaFacturas1.Columns.Contains("NotaCredito"))
                {
                    DgvListaFacturas1.Columns["NotaCredito"].DisplayIndex = 1;
                    DgvListaFacturas1.Columns["CedulaCliente"].DisplayIndex = 2;
                    DgvListaFacturas1.Columns["FactSub"].DisplayIndex = 3;
                    DgvListaFacturas1.Columns["FactImpuesto"].DisplayIndex = 4;
                    DgvListaFacturas1.Columns["FactIGTF"].DisplayIndex = 5;
                    DgvListaFacturas1.Columns["FactTotal"].DisplayIndex = 6;
                    DgvListaFacturas1.Columns["Fecha"].DisplayIndex = 7;
                }
                else
                {
                    DgvListaFacturas1.Columns["CedulaCliente"].DisplayIndex = 1;
                    DgvListaFacturas1.Columns["FactSub"].DisplayIndex = 2;
                    DgvListaFacturas1.Columns["FactImpuesto"].DisplayIndex = 3;
                    DgvListaFacturas1.Columns["FactIGTF"].DisplayIndex = 4;
                    DgvListaFacturas1.Columns["FactTotal"].DisplayIndex = 5;
                    DgvListaFacturas1.Columns["Fecha"].DisplayIndex = 6;
                }

                DgvListaFacturas1.Columns["Numero"].Visible = false;
                DgvListaFacturas1.Columns["NombreCliente"].Visible = false;

                DgvListaFacturas1.Columns["NumeroFactura"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvListaFacturas1.Columns["CedulaCliente"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvListaFacturas1.Columns["FactSub"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvListaFacturas1.Columns["FactImpuesto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvListaFacturas1.Columns["FactIGTF"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvListaFacturas1.Columns["FactTotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvListaFacturas1.Columns["Fecha"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // nuevo 21-08-2023 
                DgvListaFacturas1.Columns["FactSub"].DefaultCellStyle.Format = "##,##0.00";
                DgvListaFacturas1.Columns["FactImpuesto"].DefaultCellStyle.Format = "##,##0.00";
                DgvListaFacturas1.Columns["FactIGTF"].DefaultCellStyle.Format = "##,##0.00";
                DgvListaFacturas1.Columns["FactTotal"].DefaultCellStyle.Format = "##,##0.00";
                DgvListaFacturas1.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";

                //// Deshabilitar el ajuste automático de la altura de las filas
                //DgvListaFacturas.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                //// Establecer la altura de las filas
                //this.DgvListaFacturas1.RowTemplate.Height = 50;

            }


            catch (Exception ex)
            {
                //MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }
        }

        public void FormatoDataGrid_Oscuro_ListaFact(System.Drawing.Color col2, System.Drawing.Color col3, System.Drawing.Color col4)
        {

            this.BackColor = col2;
            LblListadoFactura.ForeColor = Color.White;
            //Lbldesde.ForeColor = Color.White;
            //LblHasta.ForeColor = Color.White;

            //Con esta funcion coloreamos el grid del color oscuro 
            DgvListaFacturas1.BackgroundColor = col3;
            DgvListaFacturas1.DefaultCellStyle.BackColor = col3;
            DgvListaFacturas1.ColumnHeadersDefaultCellStyle.BackColor = col4;
            DgvListaFacturas1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DgvListaFacturas1.DefaultCellStyle.ForeColor = Color.White;
          
        }

        private async void btnPDF_Click(object sender, EventArgs e)
        {
            try
            {
                //Loader_PDF.Visible = true;

                ExportarPDF ExportarPDF = new ExportarPDF();

                //var facturas = ObtenerFacturasDesdeConsultaCompleta();
                var facturas = await ObtenerFacturasDesdeConsultaCompleta();

                var encabezado = new DatosEncabezado();

                if (!facturas.Any())
                {
                    //MessageBox.Show("No hay datos para exportar.");
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("No hay datos para exportar");
                    _FrmMensajes.ShowDialog();
                    return;
                }
                var logoBytes = (byte[])(new System.Drawing.ImageConverter())
                .ConvertTo(Properties.Resources.LogoCaroni, typeof(byte[]));

                var encabezadoPDF = new FormatoPdfDTO
                {
                    FechaInicio = DtpDesde.Value,
                    FechaFin = DtpHasta.Value,
                    LogoBytes = logoBytes
                };

                var archivo = await ExportarPDF.ExportWithFormatAsync(facturas, encabezadoPDF);

                //// Guardar el archivo
                //using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                //{
                //    saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                //    saveFileDialog.FileName = "ReporteFacturas_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf";

                //    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                //    {
                //        File.WriteAllBytes(saveFileDialog.FileName, archivo.Content);
                //        MessageBox.Show("PDF exportado exitosamente.");
                //    }
                //}

                // Crear archivo temporal
                string tempFilePath = Path.Combine(Path.GetTempPath(),
                    "ReporteFacturas_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf");

                File.WriteAllBytes(tempFilePath, archivo.Content);

                // Abrir el PDF directamente en el lector predeterminado del sistema
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = tempFilePath,
                    UseShellExecute = true // Necesario para que use el lector predeterminado
                });


            }
            catch (Exception ex)
            {
                //MessageBox.Show("Ocurrió un error al exportar el PDF: " + ex.Message);
                var detalle = $"{ex.GetType().FullName}: {ex.Message}\n{ex.StackTrace}";
                _FrmMensajes.co = 2;
                //_FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.avisomensaje(detalle);
                _FrmMensajes.ShowDialog();
            }
            finally
            {
                //Loader_PDF.Visible = false;
            }

        }

        private async Task<List<FacturaDTO>> ObtenerFacturasDesdeConsultaCompleta()
        {
            var lista = new List<FacturaDTO>();
            DataSet ds;

            bool esNotaCredito = CbxEstatus.SelectedIndex > 0;

            ds = esNotaCredito
                ? await _L_ListaFacturas.TraerNotasSinPaginado(DtpDesde, DtpHasta)
                : await _L_ListaFacturas.TraerFacturasSinPaginado(DtpDesde, DtpHasta);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return lista;
            }

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                lista.Add(new FacturaDTO
                {
                    NumeroFactura = row["NumeroFactura"]?.ToString(),
                    NotaCredito = esNotaCredito ? row["NotaCredito"]?.ToString() : null,
                    CedulaCliente = row["CedulaCliente"]?.ToString(),
                    NombreCliente = row["NombreCliente"]?.ToString(),
                    FactSub = Convert.ToDecimal(row["FactSub"] ?? 0),
                    FactImpuesto = Convert.ToDecimal(row["FactImpuesto"] ?? 0),
                    FactIGTF = Convert.ToDecimal(row["FactIGTF"] ?? 0),
                    FactTotal = Convert.ToDecimal(row["FactTotal"] ?? 0),
                    Fecha = Convert.ToDateTime(row["Fecha"]).ToString("dd/MM/yyyy")
                });
            }

            return lista;
        }

        public void FormatoDataGrid_Claro_ListaFact(System.Drawing.Color col1, System.Drawing.Color col3)
        {

            this.BackColor = col1;
            LblListadoFactura.ForeColor = Color.Black;
            //Lbldesde.ForeColor = Color.Black;
            //LblHasta.ForeColor = Color.Black;
           
            //Con esta funcion coloreamos el grid del fondo blanco 
            DgvListaFacturas1.BackgroundColor = col1;
            DgvListaFacturas1.DefaultCellStyle.BackColor = col1;
            DgvListaFacturas1.ColumnHeadersDefaultCellStyle.BackColor = col3;
            DgvListaFacturas1.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(30, 30, 30, 30);
            DgvListaFacturas1.DefaultCellStyle.ForeColor = Color.Black;

        }

        private void BtnReporteGlobal_Click(object sender, EventArgs e)
        {
            try
            {
                MostrarPanelReporteGlobal();
                panel_ReporteGlobal.BringToFront();
            }
            catch(Exception ex)
            {
                //MessageBox.Show("Ocurrió un error al exportar el Reporte Global: " + ex.Message);
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }

        }

        private async void btnAceptar_ReporteGlobal_Click(object sender, EventArgs e)
        {
            //Loader_PDF.Visible = true;

            try
            {
                var fechaSeleccionada = dtp_ReporteGlobal.Value;

                var archivo = await Task.Run(async () =>
                {
                    //Obtener todos los datos para el Excel
                    var logica = new L_ListaFacturas();

                    //Servicio para cálculos del cierre de caja
                    var servicio = new CierreCaja_Servicio();
                    var cierreCaja = await servicio.ObtenerCierreCajaCalculado(fechaSeleccionada);
                    var facturas = await servicio.ObtenerFacturasConTotales(fechaSeleccionada);
                    var pagosDia = await servicio.ObtenerPagosDiaConTotales(fechaSeleccionada);
                    var vueltosDia = await servicio.ObtenerVueltosDiaConTotales(fechaSeleccionada);
                    var notasDia = await servicio.ObtenerNotasDiaConTotales(fechaSeleccionada);

                    //if (cierreCaja == null || !cierreCaja.Any())
                    //{
                    //    MessageBox.Show("No hay datos de cierre de caja para la fecha seleccionada.");
                    //    return;
                    //}

                    //Hojas para exportar en el excel
                    var hojas = new List<ReporteGlobal_HojasExcels>
                    {
                        new ReporteGlobal_HojasExcels
                        {
                            NombreHoja = "Cierre de Caja",
                            Datos = cierreCaja.Cast<object>().ToList()
                        },
                        new ReporteGlobal_HojasExcels
                        {
                            NombreHoja = "Facturas",
                            Datos = facturas.Cast<object>().ToList()
                        },
                        new ReporteGlobal_HojasExcels
                        {
                            NombreHoja = "Pagos del Dia",
                            Datos = pagosDia.Cast<object>().ToList()
                        },
                        new ReporteGlobal_HojasExcels
                        {
                            NombreHoja = "Vueltos del Dia",
                            Datos = vueltosDia.Cast<object>().ToList()
                        },
                        new ReporteGlobal_HojasExcels
                        {
                            NombreHoja = "Notas del Dia",
                            Datos = notasDia.Cast<object>().ToList()
                        },
                    };

                    var exportador = new ExportarXLSX_ReporteGlobal();
                    //var archivo = await exportador.ExportWithFormatAsync(hojas, fechaSeleccionada);
                    return await exportador.ExportWithFormatAsync(hojas, fechaSeleccionada);

                });

                //using (SaveFileDialog dialog = new SaveFileDialog())
                //{
                //    dialog.Filter = "Excel Files|*.xlsx";
                //    dialog.FileName = $"ReporteGlobal_{fechaSeleccionada:yyyyMMdd_HHmmss}.xlsx";

                //    if (dialog.ShowDialog() == DialogResult.OK)
                //    {
                //        File.WriteAllBytes(dialog.FileName, archivo.Content);
                //        MessageBox.Show("Excel generado exitosamente.");
                //        OcultarPanelReporteGlobal();
                //    }
                //}

                string tempFilePath = Path.Combine(Path.GetTempPath(),
                    $"ReporteGlobal_{fechaSeleccionada:yyyyMMdd_HHmmss}.xlsx");

                File.WriteAllBytes(tempFilePath, archivo.Content);

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = tempFilePath,
                    UseShellExecute = true 
                });

                OcultarPanelReporteGlobal();

            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error al generar el reporte: " + ex.Message);
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }
            finally
            {
                //Loader_PDF.Visible = false;
            }

        }

        private void btnCancelar_ReporteGlobal_Click(object sender, EventArgs e)
        {
            OcultarPanelReporteGlobal();
        }

        private void MostrarPanelReporteGlobal()
        {
            dtp_ReporteGlobal.Value = DateTime.Now;
            panel_ReporteGlobal.Visible = true;
        }

        private void OcultarPanelReporteGlobal()
        {
            panel_ReporteGlobal.Visible = false;
        }

        private async void Btn_LibroVentas_Click(object sender, EventArgs e)
        {
            if (DtpDesde.Value == DateTime.MinValue || DtpHasta.Value == DateTime.MinValue)
            {
                //MessageBox.Show("Por favor, seleccione un rango de fechas válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _FrmMensajes.co = 1;
                _FrmMensajes.avisomensaje("Por favor, seleccione un rango de fechas válido.");
                _FrmMensajes.ShowDialog();

                return;
            }

            //Loader_PDF.Visible = true;

            try
            {
                var encabezadoLibroVentas = new FormatoLibroVentasDTO
                {
                    FechaInicio = DtpDesde.Value,
                    FechaFin = DtpHasta.Value
                };

                var logica = new L_ListaFacturas();
                var dataSet = await logica.ReporteLibroVentas(DtpDesde, DtpHasta);

                var servicio = new LibroVentas_Servicio();
                var listaDatos = await servicio.ObtenerLibroVentasTotal(DtpDesde, DtpHasta); 

                if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                {
                    //MessageBox.Show("No hay datos para el rango de fechas seleccionado.");
                    _FrmMensajes.co = 1;
                    _FrmMensajes.avisomensaje("No hay datos para el rango de fechas seleccionado");
                    _FrmMensajes.ShowDialog();

                    return;
                }

                var exportador = new ExportarXLSX_LibroVentas();
                var archivo = await exportador.ExportWithFormatAsync(listaDatos, encabezadoLibroVentas);

                //using (SaveFileDialog dialog = new SaveFileDialog())
                //{
                //    dialog.Filter = "Excel Files|*.xlsx";
                //    dialog.FileName = $"ReporteLibroVentas_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                //    if (dialog.ShowDialog() == DialogResult.OK)
                //    {
                //        File.WriteAllBytes(dialog.FileName, archivo.Content);
                //        //MessageBox.Show("Excel generado exitosamente.");
                //    }
                //}

                string tempFilePath = Path.Combine(Path.GetTempPath(),
                    $"ReporteLibroVentas_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");

                File.WriteAllBytes(tempFilePath, archivo.Content);

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = tempFilePath,
                    UseShellExecute = true
                });


            }
            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(string.Format("Error: {0}", ex.Message) + ", Error inesperado");
                _FrmMensajes.ShowDialog();
            }
            finally
            {
                //Loader_PDF.Visible = false;
            }
        }

        private void CbxEstatus_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //if (CbxEstatus.SelectedIndex  == 0)
            //{
            //    Dts = _L_ListaFacturas.TraerFacturasRango(DtpDesde, DtpHasta);
               
            //}
            //else
            //{
            //    Dts = _L_ListaFacturas.TraerNotasRango(DtpDesde, DtpHasta);
            //}

            //if (Dts != null)
            //{
            //    DgvListaFacturas1.DataSource = Dts.Tables[0];
            //    Paginado(Dts);
            //    Paginado_Habilitar(true);
            //}
            //else
            //{
            //    Paginado_Habilitar(false);
            //}


            //if (DgvListaFacturas1.Rows.Count > 0)
            //{
            //    DgvListaFacturas1.Visible = true;
            //    EstructuraGrid();
            //}
            //else
            //{
            //    DgvListaFacturas1.Visible = false;
            //}
        }

        private void CbxEstatus_Enter(object sender, EventArgs e)
        {
            //orden = txtNumeroOrden.Text;
            //if (orden.Equals("N° de orden"))
            //{
            //    txtNumeroOrden.Text = "";
            //    txtNumeroOrden.ForeColor = Color.Gray;
            //}
        }


        ////Movimiento del panel Reporte Global
        //[DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        //private extern static void ReleaseCapture();
        //[DllImport("user32.DLL", EntryPoint = "SendMessage")]
        //private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void Lbl_ReporteGlobal_MouseDown(object sender, MouseEventArgs e)
        {
            //ReleaseCapture();
            //SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {}

        private void DgvListaFacturas1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {}

        private void panel_ReporteGlobal_Paint(object sender, PaintEventArgs e)
        {}



    }
}
