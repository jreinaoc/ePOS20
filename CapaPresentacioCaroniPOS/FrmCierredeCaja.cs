using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaLogica.CierreCaja_Logica;
using CapaDatos.DetalleOrden_Datos;
using CapaDatos.Inicio_Datos;
using CapaEntidades;
using CapaDatos.Anulacion;
using System.Globalization;


namespace CapaVisual_Login
{
    public partial class FrmCierredeCaja : Form
    {
        private L_CierreCaja _L_CierreCaja = new L_CierreCaja();
        FrmMensajes _FrmMensajes = new FrmMensajes();
        private D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        D_Inicio _D_Inicio = new D_Inicio();

        DateTime diaActivo;
        DataTable dtPtoVenta = new DataTable();
        string sucursal;
        DataTable dtPagoMovil = new DataTable();
        FrmClaveGerente _FrmClaveGerente = new FrmClaveGerente();
        D_Anulacion _D_Anulacion = new D_Anulacion();
        public string GerenteAutoriza = "";

        public FrmCierredeCaja()
        {
            InitializeComponent();
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {

            

            if (_L_CierreCaja.CierreFueradeHorario(sucursal,DateTime.Now, DateTime.Now))
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Debe registrar el cierre de la sucursal");
                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                _FrmMensajes.ShowDialog();
                return;
            }

            DataTable dtPuntosCerrados = _L_CierreCaja.CierrePuntodeVenta(sucursal, "", "", diaActivo);

            //Si no se han cerrado 
            if (dtPuntosCerrados.Rows.Count == 0)
            {
                bool hayLotesEnBlanco = false;

                foreach (DataGridViewRow fila in Dvg_CierrePuntoVenta.Rows)
                {
                    // Ignorar fila nueva si está habilitada la opción de agregar
                    if (!fila.IsNewRow)
                    {
                        var valorLote = fila.Cells["Nro. Lote"].Value?.ToString().Trim();

                        if (string.IsNullOrEmpty(valorLote))
                        {
                            hayLotesEnBlanco = true;
                            break;
                        }
                    }
                }

                if (hayLotesEnBlanco)
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("Debe escribir el Nro. de lote");
                    _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                    _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                    _FrmMensajes.ShowDialog();
                    return;
                }
                else
                {
                    foreach (DataGridViewRow fila in Dvg_CierrePuntoVenta.Rows)
                    {
                        // Ignorar fila nueva si está habilitada la opción de agregar
                        if (!fila.IsNewRow)
                        {
                            var valorLote = fila.Cells["Nro. Lote"].Value?.ToString().Trim();
                            decimal.TryParse(fila.Cells[3].Value?.ToString().Trim(), out decimal totalCredito);
                            decimal.TryParse(fila.Cells[4].Value?.ToString().Trim(), out decimal totalAmex);
                            decimal.TryParse(fila.Cells[5].Value?.ToString().Trim(), out decimal totalDebito);
                            decimal.TryParse(fila.Cells[6].Value?.ToString().Trim(), out decimal totalOtros);

                           
                            _L_CierreCaja.AgregaPuntosdeVenta(fila.Cells[0].Value?.ToString().Trim(), diaActivo, fila.Cells[2].Value?.ToString().Trim(), totalCredito, totalAmex, totalDebito, totalOtros);
                        }
                    }
                    
                    
                }

                //_FrmMensajes.co = 2;
                //_FrmMensajes.avisomensaje("Debe cerrar los puntos de venta antes de cerrar la caja");
                //_FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                //_FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                //_FrmMensajes.ShowDialog();
                //return;
            }
            tcCierreCaja.SelectedIndex = 1;
            lblPaso.Text = "Paso 2";
        }

        private void btnAtrasPaso1_Click(object sender, EventArgs e)
        {
            tcCierreCaja.SelectedIndex = 0;
            lblPaso.Text = "Paso 1";

        }

        public void FormatoClaro(System.Drawing.Color col1, System.Drawing.Color col3, System.Drawing.Color col5)
        {
            //col1 es blanco, col3 es Silken Jade, col5 es Noble Black
            
            //Pagina 1
            tabPage1.BackColor = col1;
            Lbl_Tap1_DatosPersonal.BackColor = col3;
            Lbl_Tap1_DatosPersonal.ForeColor = col5;
            label2.BackColor = col3;
            label2.ForeColor = col5;
            label1.BackColor = col3;
            label1.ForeColor = col5;
            Dvg_CierrePuntoVenta.ColumnHeadersDefaultCellStyle.BackColor = col3;
            Dvg_CierrePuntoVenta.ColumnHeadersDefaultCellStyle.ForeColor = col1;

            //Pagina 2
            tabPage2.BackColor = col1;
            lbl_MarcajeAsistenciaPen.BackColor = col3;
            lbl_MarcajeAsistenciaPen.ForeColor = col5;
            Dvg_MarcajeAsistenciaPendiente.ColumnHeadersDefaultCellStyle.BackColor = col3;
            Dvg_MarcajeAsistenciaPendiente.ColumnHeadersDefaultCellStyle.ForeColor = col1;
            lbl_OrdenesServPagoMovil.BackColor = col3;
            lbl_OrdenesServPagoMovil.ForeColor = col5;
            Dvg_OSconPagoMovil.ColumnHeadersDefaultCellStyle.BackColor = col3;
            Dvg_OSconPagoMovil.ColumnHeadersDefaultCellStyle.ForeColor = col1;

            //Pagina 3
            tabPage3.BackColor = col1;
            lbl_ConsignacionOrdenesServ.BackColor = col3;
            lbl_ConsignacionOrdenesServ.ForeColor = col5;
            Dvg_ConsignacionDeOS.ColumnHeadersDefaultCellStyle.BackColor = col3;
            Dvg_ConsignacionDeOS.ColumnHeadersDefaultCellStyle.ForeColor = col1;
            lbl_CambiarVendedor.BackColor = col3;
            lbl_CambiarVendedor.ForeColor = col5;
            Pnl1_CambiarVendedor.BackColor = col1;
            lbl_ListadoDeVendedores.BackColor = col3;
            lbl_ListadoDeVendedores.ForeColor = col5;
            Pnl2_ListadoDeVendedores.BackColor = col1;
            dataGridView4.ColumnHeadersDefaultCellStyle.BackColor = col3;
            dataGridView4.ColumnHeadersDefaultCellStyle.ForeColor = col1;

            //Pagina 4
            tabPage4.BackColor = col1;
            lbl_CierreDeCaja.BackColor = col3;
            lbl_CierreDeCaja.ForeColor = col5;
            dataGridView3.ColumnHeadersDefaultCellStyle.BackColor = col3;
            dataGridView3.ColumnHeadersDefaultCellStyle.ForeColor = col1;
            dataGridView5.ColumnHeadersDefaultCellStyle.BackColor = col3;
            dataGridView5.ColumnHeadersDefaultCellStyle.ForeColor = col1;
            lbl_Observaciones.BackColor = col3;
            lbl_Observaciones.ForeColor = col5;

            //Pagina 5
            tabPage5.BackColor = col1;
            label10.BackColor = col3;
            label10.ForeColor = col5;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = col3;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = col1;
            button6.BackColor = col3;

            //Pagina 6
            tabPage6.BackColor = col1;
            lbl_TasaDia_Secuencia.BackColor = col3;
            lbl_TasaDia_Secuencia.ForeColor = col5;
            label7.BackColor = col3;
            label7.ForeColor = col5;
            label11.BackColor = col3;
            label11.ForeColor = col5;
            panel1.BackColor = col1;
            panel2.BackColor = col1;
            label12.BackColor = col3;
            label12.ForeColor = col5;
            label15.BackColor = col3;
            label15.ForeColor = col5;
            panel3.BackColor = col1;
        }

        public void FormatoOsc(System.Drawing.Color col1, System.Drawing.Color col3, System.Drawing.Color col5, System.Drawing.Color col6)
        {
            //col1 es blanco, col3 es Silken Jade, col 5 es Noble Black, col6 es Nordic Noir

            //Pagina 1
            tabPage1.BackColor = col3;
            Lbl_Tap1_DatosPersonal.BackColor = col6;
            Lbl_Tap1_DatosPersonal.ForeColor = col1;
            label2.BackColor = col6;
            label2.ForeColor = col1;
            label1.BackColor = col6;
            label1.ForeColor = col1;
            Dvg_CierrePuntoVenta.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#2f6b64");
            Dvg_CierrePuntoVenta.ColumnHeadersDefaultCellStyle.ForeColor = col1;

            //Pagina 2
            tabPage2.BackColor = col3;
            lbl_MarcajeAsistenciaPen.BackColor = col6;
            lbl_MarcajeAsistenciaPen.ForeColor = col1;
            Dvg_MarcajeAsistenciaPendiente.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#2f6b64");
            Dvg_MarcajeAsistenciaPendiente.ColumnHeadersDefaultCellStyle.ForeColor = col1;
            //checkBox2.BackColor = col1;
            lbl_OrdenesServPagoMovil.BackColor = col6;
            lbl_OrdenesServPagoMovil.ForeColor = col1;
            Dvg_OSconPagoMovil.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#2f6b64");
            Dvg_OSconPagoMovil.ColumnHeadersDefaultCellStyle.ForeColor = col1;

            //Pagina 3
            tabPage3.BackColor = col3;
            lbl_ConsignacionOrdenesServ.BackColor = col6;
            lbl_ConsignacionOrdenesServ.ForeColor = col1;
            Dvg_ConsignacionDeOS.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#2f6b64");
            Dvg_ConsignacionDeOS.ColumnHeadersDefaultCellStyle.ForeColor = col1;
            checkBox1.BackColor = col1;
            lbl_CambiarVendedor.BackColor = col6;
            lbl_CambiarVendedor.ForeColor = col1;
            Pnl1_CambiarVendedor.BackColor = ColorTranslator.FromHtml("#257b78");
            lbl_ListadoDeVendedores.BackColor = col6;
            lbl_ListadoDeVendedores.ForeColor = col1;
            Pnl2_ListadoDeVendedores.BackColor = ColorTranslator.FromHtml("#257b78");
            dataGridView4.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#2f6b64");
            dataGridView4.ColumnHeadersDefaultCellStyle.ForeColor = col1;

            //Pagina 4
            tabPage4.BackColor = col3;
            lbl_CierreDeCaja.BackColor = col6;
            lbl_CierreDeCaja.ForeColor = col1;
            dataGridView3.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#2f6b64");
            dataGridView3.ColumnHeadersDefaultCellStyle.ForeColor = col1;
            dataGridView5.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#2f6b64");
            dataGridView5.ColumnHeadersDefaultCellStyle.ForeColor = col1;
            lbl_Observaciones.BackColor = col6;
            lbl_Observaciones.ForeColor = col1;

            //Pagina 5
            tabPage5.BackColor = col3;
            label10.BackColor = col6;
            label10.ForeColor = col1;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#2f6b64");
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = col1;
            button6.BackColor = ColorTranslator.FromHtml("#2f6b64");

            //Pagina 6
            tabPage6.BackColor = col3;
            lbl_TasaDia_Secuencia.BackColor = col6;
            lbl_TasaDia_Secuencia.ForeColor = col1;
            label7.BackColor = col6;
            label7.ForeColor = col1;
            label11.BackColor = col6;
            label11.ForeColor = col1;
            panel1.BackColor = ColorTranslator.FromHtml("#257b78");
            panel2.BackColor = ColorTranslator.FromHtml("#257b78");
            label12.BackColor = col6;
            label12.ForeColor = col1;
            label15.BackColor = col6;
            label15.ForeColor = col1;
            panel3.BackColor = ColorTranslator.FromHtml("#257b78");
        }

        private void btn_Siguiente_pg2_Click(object sender, EventArgs e)
        {
            //bool hayReferenciasEnBlanco = false;
           // bool hayAsistenciasEnBlanco = false;

            foreach (DataGridViewRow fila in Dvg_OSconPagoMovil.Rows)
            {
                // Ignorar fila nueva si está habilitada la opción de agregar
                if (!fila.IsNewRow)
                {
                    var valorRef = fila.Cells["Referencia"].Value?.ToString().Trim();

                    if (string.IsNullOrEmpty(valorRef) || valorRef == "0" || valorRef.Length < 10)
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("Debe escribir el Nro. de referencia");
                        _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                        _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                        _FrmMensajes.ShowDialog();
                        return;
                        //break;
                    }
                    else
                    {
                        foreach (DataGridViewRow filaPM in Dvg_OSconPagoMovil.Rows)
                        {
                            // Ignorar fila nueva si está habilitada la opción de agregar
                            if (!filaPM.IsNewRow)
                            {
                                var valor = filaPM.Cells["SeleccioneBancoEmisor"].Value?.ToString();
                                _L_CierreCaja.AgregaReferenciaPagoMovil(sucursal, filaPM.Cells[1].Value?.ToString().Trim(), filaPM.Cells[7].Value?.ToString().Trim(), filaPM.Cells[9].Value?.ToString().Trim());
                            }
                        }
                    }
                }
            }

            //ASISTENCIAS
            
            foreach (DataGridViewRow filaAsis in Dvg_MarcajeAsistenciaPendiente.Rows)
            {
                // Ignorar fila nueva si está habilitada la opción de agregar
                if (!filaAsis.IsNewRow)
                {
                    var HoraEntrada1 = filaAsis.Cells["HORAENTRADAT1"].Value?.ToString().Trim();
                    var HoraSalida1 = filaAsis.Cells["HORASALIDAT1"].Value?.ToString().Trim();
                    var HoraEntrada2 = filaAsis.Cells["HORAENTRADAT2"].Value?.ToString().Trim();
                    var HoraSalida2 = filaAsis.Cells["HORASALIDAT2"].Value?.ToString().Trim();
                    var codigoEmp = filaAsis.Cells["COD_EMPLEADO"].Value?.ToString().Trim();

                    if (string.IsNullOrEmpty(HoraEntrada1) || string.IsNullOrEmpty(HoraSalida1) || string.IsNullOrEmpty(HoraEntrada2) || string.IsNullOrEmpty(HoraSalida2))
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("Debe marcar asistencia");
                        _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                        _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                        _FrmMensajes.ShowDialog();
                        return;
                        //break;
                    }
                    else
                    {
                        foreach (DataGridViewRow fila in Dvg_MarcajeAsistenciaPendiente.Rows)
                        {
                            // Ignorar fila nueva si está habilitada la opción de agregar
                            if (!fila.IsNewRow)
                            {
                               // DateTime horaSeleccionada = miTimePicker.Value;

                            
                                //string resultado = HoraSalida1.Replace(".", "").Replace("a m", "AM");
                                HoraSalida1 = HoraSalida1.Replace(".", "").Replace("a m", "am").Replace("p m", "pm").Replace("\u00A0", ""); // Por si acaso, eliminar cualquier espacio no separable

                                _L_CierreCaja.ActualizaAsistencia(diaActivo.ToString("dd/MM/yyyy"), HoraSalida1, codigoEmp, TB_USUARIO.COD_EMPLEADO);
                                _D_Anulacion.CaragarAuditor(TB_USUARIO.COD_SUCURSAL, "072", TB_USUARIO.COD_EMPLEADO, "Se asigno la hora de salida " + HoraSalida1 + ", al empleado " + codigoEmp  + ", Autoriza: " + GerenteAutoriza);

                            }
                        }
                    }
                }
            }


            tcCierreCaja.SelectedIndex = 2;
            lblPaso.Text = "Paso 3";
        }

        private void FrmCierredeCaja_Load(object sender, EventArgs e)
        {
            try
            {
                sucursal = _D_DetalleOrden.TB_PARAMETRO("sucursalId");
                diaActivo = _D_Inicio.DiaActivo();

                //PUNTOS DE VENTA
                //Consulto si existen cerrados
                
                DataTable dtPuntosCerrados = _L_CierreCaja.CierrePuntodeVenta(sucursal, "", "", diaActivo);
                
                //Si no se han cerrado lleno datos en 0
                if (dtPuntosCerrados.Rows.Count == 0)
                {
                    DataTable dt = _L_CierreCaja.ObtienePuntosdeVenta("");

                    CrearTabla("PuntodeVenta");
                    
                    foreach (DataRow fila in dt.Rows)
                        {
                            dtPtoVenta.Rows.Add(fila["CodPunto"], fila["Descripcion"], "", "0,00", "0,00", "0,00", "0,00");
                        }
                   

                    // Asignar al DataGridView
                    Dvg_CierrePuntoVenta.DataSource = dtPtoVenta;

                    FormatoTabla("PuntodeVenta");
                }
                else
                {
                    CrearTabla("PuntodeVenta");

                    foreach (DataRow fila in dtPuntosCerrados.Rows)
                    {
                        dtPtoVenta.Rows.Add(fila[0], fila[1], fila[2], fila[3], fila[4], fila[5], fila[6]);
                    }


                    // Asignar al DataGridView
                    Dvg_CierrePuntoVenta.DataSource = dtPtoVenta;

                    FormatoTabla("PuntodeVenta");
                }


                //OS CON PAGOMOVIL
                DataTable dtPagoMovil = _L_CierreCaja.ObtineneCambioCierre(diaActivo, sucursal);

                // Asignar al DataGridView
                Dvg_OSconPagoMovil.DataSource = dtPagoMovil;

                FormatoTabla("PagoMovil");

                DataTable dtBancos = _L_CierreCaja.ObtieneBancosPagoMovil(sucursal);

                //ASISTENCIA PENDIENTE
                DataTable dtAsistenciaPendiente = _L_CierreCaja.VerificaAsistenciaPendiente(diaActivo.ToString("yyyyMMdd"), "PEND");

                // Asignar al DataGridView
                Dvg_MarcajeAsistenciaPendiente.DataSource = dtAsistenciaPendiente;

                FormatoTabla("Asistencia");

                //DataTable dtBancos = _L_CierreCaja.ObtieneBancosPagoMovil(sucursal);
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Ocurrió un error al obtener la información del cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        public void CrearTabla(string tipo)
        {
            switch (tipo)
            {
                case "PuntodeVenta":
                    dtPtoVenta.Columns.Add("CodPunto", typeof(string));
                    dtPtoVenta.Columns.Add("Banco", typeof(string));
                    dtPtoVenta.Columns.Add("Nro. Lote", typeof(string)); // Vacía
                    dtPtoVenta.Columns.Add("Total T. Crédito", typeof(string)); // Vacía
                    dtPtoVenta.Columns.Add("Total T. Amex", typeof(string)); // Vacía
                    dtPtoVenta.Columns.Add("Total T. Débito", typeof(string)); // Vacía
                    dtPtoVenta.Columns.Add("Total T. Otros", typeof(string)); // Vacía
                break;

                case "PagoMovil":
                    //dtPagoMovil.Columns.Add("Id", typeof(string));
                    //dtPagoMovil.Columns.Add("Orden", typeof(string));
                    //dtPagoMovil.Columns.Add("Referencia", typeof(string));
                    //dtPagoMovil.Columns.Add("BancoEmisor", typeof(string));
                    //dtPagoMovil.Columns.Add("BancoReceptor", typeof(string));
                    //dtPagoMovil.Columns.Add("MontoVueltoRef", typeof(decimal));
                    //dtPagoMovil.Columns.Add("MontoVueltoBs", typeof(decimal));
                    //dtPagoMovil.Columns.Add("nombreSucursal", typeof(string));
                    //dtPagoMovil.Columns.Add("Referencia", typeof(string));
                    //dtPagoMovil.Columns.Add("CodigoError", typeof(string));
                break;


                default:
                    break;
            }
        }

        public void FormatoTabla(string tipo)
        {
            switch (tipo)
            {
                case "PuntodeVenta":

                    // Asignar ancho personalizado a cada columna
                    Dvg_CierrePuntoVenta.Columns["CodPunto"].Width = 0;
                    Dvg_CierrePuntoVenta.Columns["CodPunto"].Visible = false;
                    Dvg_CierrePuntoVenta.Columns["Banco"].Width = 100;
                    Dvg_CierrePuntoVenta.Columns["Nro. Lote"].Width = 150;
                    Dvg_CierrePuntoVenta.Columns["Total T. Crédito"].Width = 120;
                    Dvg_CierrePuntoVenta.Columns["Total T. Amex"].Width = 120;
                    Dvg_CierrePuntoVenta.Columns["Total T. Débito"].Width = 120;
                    Dvg_CierrePuntoVenta.Columns["Total T. Otros"].Width = 100;

                   
                    Dvg_CierrePuntoVenta.Columns["Banco"].ReadOnly = true;
                    break;

                case "PagoMovil":

                    // Obtener bancos
                    DataTable dtBancos = _L_CierreCaja.ObtieneBancosPagoMovil(sucursal);

                    // Asegurar que el DataTable tenga la columna que se enlazará con el ComboBox
                    if (!dtPagoMovil.Columns.Contains("CodigoBancoEmisor"))
                        dtPagoMovil.Columns.Add("CodigoBancoEmisor", typeof(string));

                    // Convertir nombres de banco en dtPagoMovil a códigos (coincidiendo con dtBancos)
                    foreach (DataRow fila in dtPagoMovil.Rows)
                    {
                        var nombre = fila["BancoEmisor"]?.ToString().Trim();
                        var banco = dtBancos.AsEnumerable()
                            .FirstOrDefault(b => b["BancoEmisor"].ToString().Trim() == nombre);

                        if (banco != null)
                            fila["CodigoBancoEmisor"] = banco["Codigo"];
                        else
                            fila["CodigoBancoEmisor"] = DBNull.Value;
                    }

                    // Asignar el DataSource al DataGridView

                    //Dvg_OSconPagoMovil.DataSource = dtPagoMovil;

                    // Configurar columnas visibles y anchos
                    Dvg_OSconPagoMovil.Columns["Id"].Visible = false;
                    Dvg_OSconPagoMovil.Columns["nombreSucursal"].Visible = false;
                    Dvg_OSconPagoMovil.Columns["CodigoError"].Visible = false;
                    Dvg_OSconPagoMovil.Columns["BancoEmisor"].Visible = false; // Ocultar nombre original
                    Dvg_OSconPagoMovil.Columns["CodBancoEmisor"].Visible = false; // Ocultar código si deseas

                    Dvg_OSconPagoMovil.Columns["Orden"].Width = 100;
                    Dvg_OSconPagoMovil.Columns["Referencia"].Width = 120;
                    Dvg_OSconPagoMovil.Columns["BancoReceptor"].Width = 120;
                    Dvg_OSconPagoMovil.Columns["MontoVueltoRef"].Width = 100;
                    Dvg_OSconPagoMovil.Columns["MontoVueltoBs"].Width = 100;

                    Dvg_OSconPagoMovil.Columns["BancoReceptor"].HeaderText = "Banco Receptor";
                    Dvg_OSconPagoMovil.Columns["MontoVueltoRef"].HeaderText = "Monto $";
                    Dvg_OSconPagoMovil.Columns["MontoVueltoBs"].HeaderText = "Monto Bs.";

                    Dvg_OSconPagoMovil.Columns["Orden"].ReadOnly = true;
                    Dvg_OSconPagoMovil.Columns["BancoReceptor"].ReadOnly = true;
                    Dvg_OSconPagoMovil.Columns["MontoVueltoRef"].ReadOnly = true;
                    Dvg_OSconPagoMovil.Columns["MontoVueltoBs"].ReadOnly = true;

                    Dvg_OSconPagoMovil.Columns["BancoReceptor"].ReadOnly = true;
                    Dvg_OSconPagoMovil.Columns["MontoVueltoRef"].ReadOnly = true;
                    Dvg_OSconPagoMovil.Columns["MontoVueltoBs"].ReadOnly = true;


                    // Crear y configurar la columna ComboBox para BancoEmisor
                    DataGridViewComboBoxColumn cboBancoEmisor = new DataGridViewComboBoxColumn();
                    cboBancoEmisor.Name = "SeleccioneBancoEmisor";
                    cboBancoEmisor.HeaderText = "Banco Emisor";
                    cboBancoEmisor.DisplayMember = "BancoEmisor"; // lo que se muestra
                    cboBancoEmisor.ValueMember = "Codigo";        // lo que se guarda internamente
                    cboBancoEmisor.DataPropertyName = "CodBancoEmisor"; // enlaza con el DataTable de origen
                    cboBancoEmisor.DataSource = dtBancos;
                    cboBancoEmisor.Width = 120;

                    // Insertar el ComboBox en la posición donde estaba BancoEmisor
                    int index = Dvg_OSconPagoMovil.Columns["BancoEmisor"].Index;
                    Dvg_OSconPagoMovil.Columns.Insert(index, cboBancoEmisor);

                    // Manejar posibles errores silenciosamente
                    Dvg_OSconPagoMovil.DataError += (s, e) => { e.ThrowException = false; };
                   

                    break;


                case "Asistencia":

                    // Asignar ancho personalizado a cada columna
                    Dvg_MarcajeAsistenciaPendiente.Columns["NOMBREEMPLEADO"].Width = 200;
                    Dvg_MarcajeAsistenciaPendiente.Columns["COD_SUCURSAL"].Visible = false;
                    Dvg_MarcajeAsistenciaPendiente.Columns["COD_EMPLEADO"].Width = 60;
                    Dvg_MarcajeAsistenciaPendiente.Columns["FECHA"].Width = 100;
                    Dvg_MarcajeAsistenciaPendiente.Columns["HORAENTRADAT1"].Width = 100;
                    Dvg_MarcajeAsistenciaPendiente.Columns["HORASALIDAT1"].Width = 100;
                    Dvg_MarcajeAsistenciaPendiente.Columns["HORAENTRADAT2"].Width = 100;
                    Dvg_MarcajeAsistenciaPendiente.Columns["HORASALIDAT2"].Width = 100;
                    Dvg_MarcajeAsistenciaPendiente.Columns["ASIS_DIA"].Visible = false;
                    Dvg_MarcajeAsistenciaPendiente.Columns["NOMBREEMPLEADO"].HeaderText = "Nombre Empleado";
                    Dvg_MarcajeAsistenciaPendiente.Columns["COD_EMPLEADO"].HeaderText = "Código";
                    Dvg_MarcajeAsistenciaPendiente.Columns["FECHA"].HeaderText = "Fecha";
                    Dvg_MarcajeAsistenciaPendiente.Columns["HORAENTRADAT1"].HeaderText = "Entrada 1";
                    Dvg_MarcajeAsistenciaPendiente.Columns["HORASALIDAT1"].HeaderText = "Salida 1";
                    Dvg_MarcajeAsistenciaPendiente.Columns["HORAENTRADAT2"].HeaderText = "Entrada 2";
                    Dvg_MarcajeAsistenciaPendiente.Columns["HORASALIDAT2"].HeaderText = "Salida 2";

                    Dvg_MarcajeAsistenciaPendiente.Columns["NOMBREEMPLEADO"].ReadOnly = true;
                    Dvg_MarcajeAsistenciaPendiente.Columns["COD_EMPLEADO"].ReadOnly = true;
                    Dvg_MarcajeAsistenciaPendiente.Columns["FECHA"].ReadOnly = true;
                    Dvg_MarcajeAsistenciaPendiente.Columns["HORAENTRADAT1"].ReadOnly = true;
                    Dvg_MarcajeAsistenciaPendiente.Columns["HORASALIDAT1"].ReadOnly = true;
                    Dvg_MarcajeAsistenciaPendiente.Columns["HORAENTRADAT2"].ReadOnly = true;
                    Dvg_MarcajeAsistenciaPendiente.Columns["HORASALIDAT2"].ReadOnly = true;




                    foreach (DataGridViewRow fila in Dvg_MarcajeAsistenciaPendiente.Rows)
                    {
                        // Ignorar fila nueva si está habilitada la opción de agregar
                        if (!fila.IsNewRow)
                        {
                            int colIndex = 0;
                            var HoraEntrada1 = fila.Cells["HORAENTRADAT1"].Value?.ToString().Trim();
                            var HoraSalida1 = fila.Cells["HORASALIDAT1"].Value?.ToString().Trim();
                            var HoraEntrada2 = fila.Cells["HORAENTRADAT2"].Value?.ToString().Trim();
                            var HoraSalida2 = fila.Cells["HORASALIDAT2"].Value?.ToString().Trim();

                            if (string.IsNullOrEmpty(HoraEntrada1))
                            {
                                // Obtener el índice y eliminar la columna original
                                colIndex = Dvg_MarcajeAsistenciaPendiente.Columns["HORAENTRADAT1" +
                                    ""].Index;
                                Dvg_MarcajeAsistenciaPendiente.Columns.RemoveAt(colIndex);

                                // Agregar la nueva columna personalizada
                                var colHoraEntrada1 = new DataGridViewTimePickerColumn
                                {
                                    Name = "HORAENTRADAT1",
                                    HeaderText = "Entrada 1",
                                    Width = 100,
                                    DataPropertyName = "HORAENTRADAT1",
                                    DefaultCellStyle = new DataGridViewCellStyle { Format = "t" } // formato corto de hora
                                };

                                Dvg_MarcajeAsistenciaPendiente.Columns.Insert(colIndex, colHoraEntrada1);
                                Dvg_MarcajeAsistenciaPendiente.Columns["HORAENTRADAT1"].ReadOnly = false;
                            }

                            if (string.IsNullOrEmpty(HoraSalida1))
                            {
                                // Obtener el índice y eliminar la columna original
                                colIndex = Dvg_MarcajeAsistenciaPendiente.Columns["HORASALIDAT1" + ""].Index;
                                Dvg_MarcajeAsistenciaPendiente.Columns.RemoveAt(colIndex);

                                // Agregar la nueva columna personalizada
                                var colHoraSalida1 = new DataGridViewTimePickerColumn
                                {
                                    Name = "HORASALIDAT1",
                                    HeaderText = "Salida 1",
                                    Width = 100,
                                    DataPropertyName = "HORASALIDAT1",
                                    DefaultCellStyle = new DataGridViewCellStyle { Format = "t" } // formato corto de hora
                                };

                                Dvg_MarcajeAsistenciaPendiente.Columns.Insert(colIndex, colHoraSalida1);
                                Dvg_MarcajeAsistenciaPendiente.Columns["HORASALIDAT1"].ReadOnly = false;
                            }

                            if (string.IsNullOrEmpty(HoraEntrada2))
                            {
                                // Obtener el índice y eliminar la columna original
                                colIndex = Dvg_MarcajeAsistenciaPendiente.Columns["HORAENTRADAT2" +
                                    ""].Index;
                                Dvg_MarcajeAsistenciaPendiente.Columns.RemoveAt(colIndex);

                                // Agregar la nueva columna personalizada
                                var colHoraEntrada2 = new DataGridViewTimePickerColumn
                                {
                                    Name = "HORAENTRADAT2",
                                    HeaderText = "Entrada 2",
                                    Width = 100,
                                    DataPropertyName = "HORAENTRADAT2",
                                    DefaultCellStyle = new DataGridViewCellStyle { Format = "t" } // formato corto de hora
                                };

                                Dvg_MarcajeAsistenciaPendiente.Columns.Insert(colIndex, colHoraEntrada2);
                                Dvg_MarcajeAsistenciaPendiente.Columns["HORAENTRADAT2"].ReadOnly = false;
                            }

                            if (string.IsNullOrEmpty(HoraSalida2))
                            {
                                // Obtener el índice y eliminar la columna original
                                colIndex = Dvg_MarcajeAsistenciaPendiente.Columns["HORASALIDAT2" + ""].Index;
                                Dvg_MarcajeAsistenciaPendiente.Columns.RemoveAt(colIndex);

                                // Agregar la nueva columna personalizada
                                var colHoraSalida2 = new DataGridViewTimePickerColumn
                                {
                                    Name = "HORASALIDAT2",
                                    HeaderText = "Salida 2",
                                    Width = 100,
                                    DataPropertyName = "HORASALIDAT1",
                                    DefaultCellStyle = new DataGridViewCellStyle { Format = "t" } // formato corto de hora
                                };

                                Dvg_MarcajeAsistenciaPendiente.Columns.Insert(colIndex, colHoraSalida2);
                                Dvg_MarcajeAsistenciaPendiente.Columns["HORASALIDAT2"].ReadOnly = false;
                            }


                        }
                    }

                    

                    break;

                default:
                    break;
            }
        }

        private void btn_Cancelar_pg2_Click(object sender, EventArgs e)
        {
            tcCierreCaja.SelectedIndex = 0;
            lblPaso.Text = "Paso 1";
        }

        private void btn_Cancelar_pg3_Click(object sender, EventArgs e)
        {
            tcCierreCaja.SelectedIndex = 1;
            lblPaso.Text = "Paso 2";
        }

        public class DataGridViewTimePickerColumn : DataGridViewColumn
        {
            public DataGridViewTimePickerColumn()
                : base(new DataGridViewTimePickerCell())
            {
            }

            public override DataGridViewCell CellTemplate
            {
                get => base.CellTemplate;
                set
                {
                    if (value != null &&
                        !value.GetType().IsAssignableFrom(typeof(DataGridViewTimePickerCell)))
                    {
                        throw new InvalidCastException("Debe ser una celda de tipo DataGridViewTimePickerCell");
                    }
                    base.CellTemplate = value;
                }
            }
        }

        public class DataGridViewTimePickerCell : DataGridViewTextBoxCell
        {
            public override Type EditType => typeof(TimePickerEditingControl);
            public override Type ValueType => typeof(DateTime);
            public override object DefaultNewRowValue => DateTime.Now;
        }

        public class TimePickerEditingControl : DateTimePicker, IDataGridViewEditingControl
        {
            DataGridView dataGridView;
            private bool valueChanged = false;
            int rowIndex;

            public TimePickerEditingControl()
            {
                this.Format = DateTimePickerFormat.Custom;
                this.CustomFormat = "hh:mm tt"; // o "HH:mm tt" si prefieres 24 horas con AM/PM
                this.ShowUpDown = true;
            }

            public object EditingControlFormattedValue
            {
                get => this.Value.ToShortTimeString();
                set
                {
                    if (DateTime.TryParse((string)value, out DateTime result))
                        this.Value = result;
                }
            }

            

            public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context) => EditingControlFormattedValue;
            public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle) => this.Font = dataGridViewCellStyle.Font;
            public int EditingControlRowIndex { get => rowIndex; set => rowIndex = value; }
            public bool EditingControlValueChanged { get => valueChanged; set => valueChanged = value; }
            public bool RepositionEditingControlOnValueChange => false;
            public DataGridView EditingControlDataGridView { get => dataGridView; set => dataGridView = value; }
            public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey) => true;
            public Cursor EditingPanelCursor => base.Cursor;
            public void PrepareEditingControlForEdit(bool selectAll)
            {
                // Marca como modificado aunque no cambie explícitamente
                this.EditingControlDataGridView?.NotifyCurrentCellDirty(true);
            }

            protected override void OnValueChanged(EventArgs eventargs)
            {
                valueChanged = true;
                this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
                base.OnValueChanged(eventargs);
            }
        }

        private void Dvg_MarcajeAsistenciaPendiente_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var columna = Dvg_MarcajeAsistenciaPendiente.Columns[e.ColumnIndex];
                if (columna is DataGridViewTimePickerColumn)
                {
                    _FrmClaveGerente.ShowDialog();
                    if (_FrmClaveGerente.ClaveCorrecta == true)
                    {

                        if (_FrmClaveGerente.DialogResult == DialogResult.OK)
                        {
                            Dvg_MarcajeAsistenciaPendiente.BeginEdit(true); // inicia edición con un clic
                            GerenteAutoriza = _FrmClaveGerente.RetornoNombreUsuario();
                        }
                    }
                }
            }
        }
    }
}
