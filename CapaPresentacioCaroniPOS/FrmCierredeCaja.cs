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
using System.Reflection;
using System.Data.SqlClient;
using CapaDatos.Conexion;
using System.Text.RegularExpressions;

namespace CapaVisual_Login
{
    public partial class FrmCierredeCaja : Form
    {
        private L_CierreCaja _L_CierreCaja = new L_CierreCaja();
        FrmMensajes _FrmMensajes = new FrmMensajes();
        private D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        D_Inicio _D_Inicio = new D_Inicio();
        bool ValideClave = false;
        DateTime diaActivo;
        DataTable dtPtoVenta = new DataTable();
        string sucursal;
        DataTable dtPagoMovil = new DataTable();
        FrmClaveGerente _FrmClaveGerente = new FrmClaveGerente();
        D_Anulacion _D_Anulacion = new D_Anulacion();
        public string GerenteAutoriza = "";
        List<Usuario> usuarios = new List<Usuario>();
        List<Usuario> listaUsuarios = new List<Usuario>();
        List<Usuario> listaTemporalUsuarios = new List<Usuario>();
        DataTable dtCierreCaja = new DataTable();
        DataTable dtLogCierre = new DataTable();

        DataTable dtConsignacion = new DataTable();
        DataTable dtAsistenciaPendiente = new DataTable();
        DateTime FechaInicioCierre;

        //private FrmPrincipal _frmPrincipal;


        private Action _onCancelarSolicitado;

        public void CancelarSolicitado(Action Cancelar)
        {
            _onCancelarSolicitado = Cancelar;
        }

        public FrmCierredeCaja()
        {
            InitializeComponent();

        }

        private void mostrarError(string mensaje)
        {
            FrmMensajes.MostrarError(mensaje);
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (_D_Inicio.DiaActivo() >= DateTime.Now)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Imposible cerrar la caja, el día activo es mayor a la fecha de hoy");
                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                _FrmMensajes.ShowDialog();
                return;

            }
            if (!_L_CierreCaja.CierreFueradeHorario(sucursal, DateTime.Now, DateTime.Now) && txtCierreHora.Text == "")
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Debe registrar el cierre de la sucursal");
                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                _FrmMensajes.ShowDialog();
                txtCierreHora.Enabled = true;
                return;
            }

            int cantCaracteres = Convert.ToInt32(_D_DetalleOrden.TB_PARAMETRO("canCaractCierre"));

            DataTable dtPuntosCerrados = _L_CierreCaja.CierrePuntodeVenta(sucursal, "", "", diaActivo);

            if (!_L_CierreCaja.CierreFueradeHorario(sucursal, DateTime.Now, DateTime.Now))
            {
                if (txtCierreHora.Text.Length < cantCaracteres)
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("La observación ingresada debe tener un mínimo de " + cantCaracteres + " caracteres");
                    _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                    _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                    _FrmMensajes.ShowDialog();
                    return;
                }
            }

            bool valido = true;
            bool todosLotesEnBlanco = false;

            foreach (DataGridViewRow row in Dvg_CierrePuntoVenta.Rows)
            {
                if (row.IsNewRow) continue;

                // Validar Nro. Lote
                string nroLote = row.Cells["Nro. Lote"].Value?.ToString().Trim();
                if (string.IsNullOrEmpty(nroLote) || nroLote == "0")
                {
                    valido = false;
                    //row.Cells["Nro. Lote"].Style.BackColor = Color.LightCoral;
                    continue; // ya no hace falta validar totales si falla el lote
                }
                //else
                //{
                //    row.Cells["Nro. Lote"].Style.BackColor = Color.White;
                //}

                // Validar que al menos un total sea distinto de 0
                decimal totalCredito = Convert.ToDecimal(row.Cells["Total T. Crédito"].Value ?? 0);
                decimal totalAmex = Convert.ToDecimal(row.Cells["Total T. Amex"].Value ?? 0);
                decimal totalDebito = Convert.ToDecimal(row.Cells["Total T. Débito"].Value ?? 0);
                decimal totalOtros = Convert.ToDecimal(row.Cells["Total T. Otros"].Value ?? 0);

                if (totalCredito == 0 && totalAmex == 0 && totalDebito == 0 && totalOtros == 0)
                {
                    valido = false;
                    // Marcar todas las celdas de totales en rojo
                    //row.Cells["Total T. Crédito"].Style.BackColor = Color.LightCoral;
                    //row.Cells["Total T. Amex"].Style.BackColor = Color.LightCoral;
                    //row.Cells["Total T. Débito"].Style.BackColor = Color.LightCoral;
                    //row.Cells["Total T. Otros"].Style.BackColor = Color.LightCoral;
                }
                //else
                //{
                //    // Restaurar color si son válidos
                //    row.Cells["Total T. Crédito"].Style.BackColor = Color.White;
                //    row.Cells["Total T. Amex"].Style.BackColor = Color.White;
                //    row.Cells["Total T. Débito"].Style.BackColor = Color.White;
                //    row.Cells["Total T. Otros"].Style.BackColor = Color.White;
                //}
            }

            if (!valido)
            {
                todosLotesEnBlanco = true;
                //    MessageBox.Show("Existen filas con Nro. Lote vacío/0 o sin ningún total cargado.");
            }
            //else
            //{
            //    //MessageBox.Show("Validación correcta. Todos los datos son válidos.");
            //}



            //Si no se han cerrado 
            //if (Dvg_CierrePuntoVenta.Rows.Count > 0 && dtPuntosCerrados.Rows.Count == 0 )
            //{


            //foreach (DataGridViewRow fila in Dvg_CierrePuntoVenta.Rows)
            //{
            //    // Ignorar fila nueva si está habilitada la opción de agregar
            //    if (!fila.IsNewRow)
            //    {
            //        var valorLote = fila.Cells["Nro. Lote"].Value?.ToString().Trim();

            //        if (valorLote != "")
            //        {
            //            todosLotesEnBlanco = false;
            //            break;
            //        }
            //    }
            //}

            if (todosLotesEnBlanco)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Debe llenar todos los puntos de venta");
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
                        if (valorLote != "")
                        {
                            decimal.TryParse(fila.Cells[4].Value?.ToString().Trim().Replace(".", ""), out decimal totalCredito);
                            decimal.TryParse(fila.Cells[5].Value?.ToString().Trim().Replace(".", ""), out decimal totalAmex);
                            decimal.TryParse(fila.Cells[6].Value?.ToString().Trim().Replace(".", ""), out decimal totalDebito);
                            decimal.TryParse(fila.Cells[7].Value?.ToString().Trim().Replace(".", ""), out decimal totalOtros);


                            if (!_L_CierreCaja.AgregaPuntosdeVenta(fila.Cells[0].Value?.ToString().Trim(), fila.Cells[1].Value?.ToString().Trim(), diaActivo, fila.Cells[3].Value?.ToString().Trim(), totalCredito, totalAmex, totalDebito, totalOtros))

                            {
                                _FrmMensajes.co = 2;
                                _FrmMensajes.avisomensaje("Error registrando puntos de venta");
                                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                                _FrmMensajes.ShowDialog();
                                return;
                            }
                        }
                    }
                }


            }

            //_FrmMensajes.co = 2;
            //_FrmMensajes.avisomensaje("Debe cerrar los puntos de venta antes de cerrar la caja");
            //_FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
            //_FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
            //_FrmMensajes.ShowDialog();
            //return;
            //}

            //ASISTENCIA PENDIENTE
            dtAsistenciaPendiente = _L_CierreCaja.VerificaAsistenciaPendiente(diaActivo.ToString("yyyyMMdd"), "PEND");

            //dtAsistenciaPendiente.Columns["HORASALIDAT1"].AllowDBNull = true;

            // Asignar al DataGridView
            Dvg_MarcajeAsistenciaPendiente.DataSource = dtAsistenciaPendiente;

            FormatoTabla("Asistencia");


            tcCierreCaja.SelectedIndex = 1;
            //tcCierreCaja.SelectedIndex = 0;
            //tcCierreCaja.SelectedIndex = 1;
            lblPaso.Text = "Asistencia";
            lbPaso.Text = "Paso 2";
        }

        private void btnAtrasPaso1_Click(object sender, EventArgs e)
        {
            tcCierreCaja.SelectedIndex = 0;
            lblPaso.Text = "Confirmación";
            lbPaso.Text = "Paso 1";

        }

        public void RetornarInicio()
        {
            tcCierreCaja.SelectedIndex = 0;
            lblPaso.Text = "Confirmación";
            lbPaso.Text = "Paso 1";
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


            //Pagina 4
            tabPage4.BackColor = col1;
            lbl_CierreDeCaja.BackColor = col3;
            lbl_CierreDeCaja.ForeColor = col5;
            lbl_Observaciones.BackColor = col3;
            lbl_Observaciones.ForeColor = col5;
            dgvCierredecaja.BackColor = col1;
            dgvCierredecaja.BackgroundColor = col1;

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

            //Quita Bordes
            QuitarBorde1.BackColor = col1;
            QuitarBorde2.BackColor = col1;
            QuitarBorde3.BackColor = col1;
            QuitarBorde4.BackColor = col1;
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


            lbl_CambiarVendedor.BackColor = col6;
            lbl_CambiarVendedor.ForeColor = col1;
            Pnl1_CambiarVendedor.BackColor = ColorTranslator.FromHtml("#257b78");
            lbl_ListadoDeVendedores.BackColor = col6;
            lbl_ListadoDeVendedores.ForeColor = col1;
            Pnl2_ListadoDeVendedores.BackColor = ColorTranslator.FromHtml("#257b78");

            //Pagina 4
            tabPage4.BackColor = col3;
            lbl_CierreDeCaja.BackColor = col6;
            lbl_CierreDeCaja.ForeColor = col1;
            lbl_Observaciones.BackColor = col6;
            lbl_Observaciones.ForeColor = col1;
            dgvCierredecaja.BackColor = col3;
            dgvCierredecaja.BackgroundColor = col3;

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

            //Quita Bordes
            QuitarBorde1.BackColor = col3;
            QuitarBorde2.BackColor = col3;
            QuitarBorde3.BackColor = col3;
            QuitarBorde4.BackColor = col3;
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
                Dvg_MarcajeAsistenciaPendiente.CurrentCell = Dvg_MarcajeAsistenciaPendiente.Rows[0].Cells[0];
                Dvg_MarcajeAsistenciaPendiente.Rows[0].Cells[0].Selected = true;
                btn_Cancelar_pg2.Focus();
                // Ignorar fila nueva si está habilitada la opción de agregar
                if (!filaAsis.IsNewRow)
                {
                    var HoraEntrada1 = filaAsis.Cells["HORAENTRADAT1"].Value?.ToString().Trim();
                    var HoraSalida1 = filaAsis.Cells["HORASALIDAT1"].Value?.ToString().Trim();
                    var HoraEntrada2 = filaAsis.Cells["HORAENTRADAT2"].Value?.ToString().Trim();
                    var HoraSalida2 = filaAsis.Cells["HORASALIDAT2"].Value?.ToString().Trim();
                    var codigoEmp = filaAsis.Cells["COD_EMPLEADO"].Value?.ToString().Trim();

                    //if (string.IsNullOrEmpty(HoraEntrada1) || string.IsNullOrEmpty(HoraSalida1) || string.IsNullOrEmpty(HoraEntrada2) || string.IsNullOrEmpty(HoraSalida2))
                    //Si tengo entrada1 y no tengo salida1
                    if ((!string.IsNullOrEmpty(HoraEntrada1) && string.IsNullOrEmpty(HoraSalida1)) || (!string.IsNullOrEmpty(HoraEntrada2) && string.IsNullOrEmpty(HoraSalida2)))
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
                        //foreach (DataGridViewRow fila in Dvg_MarcajeAsistenciaPendiente.Rows)
                        //{
                        //    // Ignorar fila nueva si está habilitada la opción de agregar
                        //    if (!fila.IsNewRow)
                        //    {
                        string codigoEmpleado = filaAsis.Cells["COD_EMPLEADO"].Value?.ToString() ?? string.Empty;
                        // DateTime horaSeleccionada = miTimePicker.Value;
                        // si  tengo salida1
                        if (!string.IsNullOrEmpty(HoraSalida1))
                        {
                            // 1. Reemplazar cualquier tipo de espacio entre "p." y "m." (incluyendo NO-BREAK SPACE)
                            HoraSalida1 = Regex.Replace(HoraSalida1, @"p\.\s*m\.", "pm", RegexOptions.IgnoreCase);

                            // 2. Reemplazar cualquier tipo de espacio entre "a." y "m." (por si hay AM)
                            HoraSalida1 = Regex.Replace(HoraSalida1, @"a\.\s*m\.", "am", RegexOptions.IgnoreCase);

                            // 3. Eliminar espacios adicionales antes del AM/PM
                            HoraSalida1 = HoraSalida1.Trim();



                            _L_CierreCaja.ActualizaAsistencia(diaActivo.ToString("dd/MM/yyyy"), HoraSalida1, codigoEmpleado, codigoEmpleado,"S");
                            _D_Anulacion.CaragarAuditor(TB_USUARIO.COD_SUCURSAL, "072", TB_USUARIO.COD_EMPLEADO, "Se asigno la hora de salida " + HoraSalida1 + ", al empleado " + codigoEmp + ", Autoriza: " + GerenteAutoriza);

                        }

                        // si  tengo salida1
                        if (!string.IsNullOrEmpty(HoraSalida2))
                        {
                            // 1. Reemplazar cualquier tipo de espacio entre "p." y "m." (incluyendo NO-BREAK SPACE)
                            HoraSalida2 = Regex.Replace(HoraSalida2, @"p\.\s*m\.", "pm", RegexOptions.IgnoreCase);

                            // 2. Reemplazar cualquier tipo de espacio entre "a." y "m." (por si hay AM)
                            HoraSalida2 = Regex.Replace(HoraSalida2, @"a\.\s*m\.", "am", RegexOptions.IgnoreCase);

                            // 3. Eliminar espacios adicionales antes del AM/PM
                            HoraSalida2 = HoraSalida2.Trim();


                            _L_CierreCaja.ActualizaAsistencia(diaActivo.ToString("dd/MM/yyyy"), HoraSalida2, codigoEmp, TB_USUARIO.COD_EMPLEADO, "S2");
                            _D_Anulacion.CaragarAuditor(TB_USUARIO.COD_SUCURSAL, "072", TB_USUARIO.COD_EMPLEADO, "Se asigno la hora de salida2 " + HoraSalida2 + ", al empleado " + codigoEmp + ", Autoriza: " + GerenteAutoriza);

                        }

                        if (!string.IsNullOrEmpty(HoraEntrada1))
                        {
                            // 1. Reemplazar cualquier tipo de espacio entre "p." y "m." (incluyendo NO-BREAK SPACE)
                            HoraEntrada1 = Regex.Replace(HoraEntrada1, @"p\.\s*m\.", "pm", RegexOptions.IgnoreCase);

                            // 2. Reemplazar cualquier tipo de espacio entre "a." y "m." (por si hay AM)
                            HoraEntrada1 = Regex.Replace(HoraEntrada1, @"a\.\s*m\.", "am", RegexOptions.IgnoreCase);

                            // 3. Eliminar espacios adicionales antes del AM/PM
                            HoraEntrada1 = HoraEntrada1.Trim();



                            _L_CierreCaja.ActualizaAsistencia(diaActivo.ToString("dd/MM/yyyy"), HoraEntrada1, codigoEmpleado, codigoEmpleado, "E");
                            _D_Anulacion.CaragarAuditor(TB_USUARIO.COD_SUCURSAL, "072", TB_USUARIO.COD_EMPLEADO, "Se asigno la hora de entrada " + HoraEntrada1 + ", al empleado " + codigoEmp + ", Autoriza: " + GerenteAutoriza);

                        }


                        if (!string.IsNullOrEmpty(HoraEntrada2))
                        {
                            // 1. Reemplazar cualquier tipo de espacio entre "p." y "m." (incluyendo NO-BREAK SPACE)
                            HoraEntrada2 = Regex.Replace(HoraEntrada2, @"p\.\s*m\.", "pm", RegexOptions.IgnoreCase);

                            // 2. Reemplazar cualquier tipo de espacio entre "a." y "m." (por si hay AM)
                            HoraEntrada2 = Regex.Replace(HoraEntrada2, @"a\.\s*m\.", "am", RegexOptions.IgnoreCase);

                            // 3. Eliminar espacios adicionales antes del AM/PM
                            HoraEntrada2 = HoraEntrada2.Trim();



                            _L_CierreCaja.ActualizaAsistencia(diaActivo.ToString("dd/MM/yyyy"), HoraEntrada2, codigoEmpleado, codigoEmpleado, "E2");
                            _D_Anulacion.CaragarAuditor(TB_USUARIO.COD_SUCURSAL, "072", TB_USUARIO.COD_EMPLEADO, "Se asigno la hora de entrada2 " + HoraEntrada2 + ", al empleado " + codigoEmp + ", Autoriza: " + GerenteAutoriza);

                        }


                        //    }
                        //}
                    }
                }
            }


            tcCierreCaja.SelectedIndex = 2;
            lblPaso.Text = "Ordenes";
            lbPaso.Text = "Paso 3";
        }

        private void FrmCierredeCaja_Load(object sender, EventArgs e)
        {
            try
            {
                // Configurar propiedades del DataGridView
                Dvg_CierrePuntoVenta.EditMode = DataGridViewEditMode.EditOnEnter;
                Dvg_CierrePuntoVenta.SelectionMode = DataGridViewSelectionMode.CellSelect;
                Dvg_CierrePuntoVenta.StandardTab = false;


                this.txtFiltroVendedor.TextChanged += new System.EventHandler(this.txtFiltroVendedor_TextChanged);
                this.Dvg_MarcajeAsistenciaPendiente.CellClick += Dgv_MarcajeAsistenciaPendiente_CellClick;
                this.Dvg_ConsignacionDeOS.CellClick += Dvg_ConsignacionDeOS_CellClick;
                this.Dgv_Usuarios.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Dgv_Usuarios_CellContentClick);
                this.Dvg_OSconPagoMovil.EditingControlShowing += Dvg_OSconPagoMovil_EditingControlShowing;
                this.Dvg_CierrePuntoVenta.EditingControlShowing += Dvg_CierrePuntoVenta_EditingControlShowing;
                this.dgvCierredecaja.EditingControlShowing += dgvCierredecaja_EditingControlShowing;
                this.Dvg_MarcajeAsistenciaPendiente.CellMouseClick += Dvg_MarcajeAsistenciaPendiente_CellMouseClick;


                tcCierreCaja.ItemSize = new Size(0, 1);
                tcCierreCaja.SizeMode = TabSizeMode.Fixed;

                QuitarBorde1.BringToFront();
                QuitarBorde2.BringToFront();
                QuitarBorde3.BringToFront();
                QuitarBorde4.BringToFront();


                var labelVertical = new VerticalLabel
                {
                    Text = "Caja",
                    Font = new Font("Century Gothic", 13),


                    ForeColor = Color.Black,
                    Size = new Size(30, 148),

                    Location = new Point(110, 126),

                    Invertir = true // ponlo en true si quieres que el texto vaya de abajo hacia arriba
                };
                labelVertical.BackColor = Color.FromArgb(0, 186, 173);
                labelVertical.ForeColor = Color.White;
                tabPage4.Controls.Add(labelVertical);

                var labelVerticalPagos = new VerticalLabel
                {
                    Text = "Pagos",
                    Font = new Font("Century Gothic", 13),
                    ForeColor = Color.Black,
                    Size = new Size(30, 210),
                    Location = new Point(110, 278),
                    Invertir = true // ponlo en true si quieres que el texto vaya de abajo hacia arriba
                };
                labelVerticalPagos.BackColor = Color.FromArgb(0, 186, 173);
                labelVerticalPagos.ForeColor = Color.White;
                tabPage4.Controls.Add(labelVerticalPagos);




            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Ocurrió un error al obtener la información del cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        //protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        //{
        //    var col = Dvg_MarcajeAsistenciaPendiente.CurrentCell?.OwningColumn;

        //    if (col is DataGridViewTimePickerColumn)
        //    {
        //        var dgv = Dvg_MarcajeAsistenciaPendiente;

        //        if (dgv.IsCurrentCellInEditMode)
        //            dgv.EndEdit();
        //        else
        //            dgv.BeginEdit(false);

        //        // Solo consumir ENTER, no otras teclas
        //        if (keyData == Keys.Enter)
        //            return true;
        //    }

        //    return base.ProcessCmdKey(ref msg, keyData);
        //}

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // 1) Detecta ENTER
            if (keyData == Keys.Enter)
            {
                // 2) ¿Estamos en la columna TimePicker?
                var col = Dvg_MarcajeAsistenciaPendiente.CurrentCell?.OwningColumn;
                if (col is DataGridViewTimePickerColumn && ValideClave == true)
                {
                    var dgv = Dvg_MarcajeAsistenciaPendiente;

                    // 3) Si ya editamos, finaliza; si no, inicia
                    if (dgv.IsCurrentCellInEditMode)
                        dgv.EndEdit();
                    else
                        dgv.BeginEdit(false);

                    // 4) Consume ENTER para que no siga al DateTimePicker
                    return true;
                }
            }

            // Deja el resto de teclas con su comportamiento normal
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void CrearTabla(string tipo)
        {
            switch (tipo)
            {
                case "PuntodeVenta":
                    if (dtPtoVenta.Columns.Count == 0)
                    {
                        dtPtoVenta.Columns.Add("CodPunto", typeof(string));
                        dtPtoVenta.Columns.Add("Tipo", typeof(string));
                        dtPtoVenta.Columns.Add("Banco", typeof(string));
                        dtPtoVenta.Columns.Add("Nro. Lote", typeof(string)); // Vacía
                        dtPtoVenta.Columns.Add("Total T. Crédito", typeof(string)); // Vacía
                        dtPtoVenta.Columns.Add("Total T. Amex", typeof(string)); // Vacía
                        dtPtoVenta.Columns.Add("Total T. Débito", typeof(string)); // Vacía
                        dtPtoVenta.Columns.Add("Total T. Otros", typeof(string)); // Vacía
                    }
                    break;

                case "CierredeCaja":
                    if (dtCierreCaja.Columns.Count == 0)
                    {
                        dtCierreCaja.Columns.Add("TipoTotal", typeof(string));
                        dtCierreCaja.Columns.Add("Total", typeof(string));
                    }
                    break;

                case "LogCierre":
                    if (dtLogCierre.Columns.Count == 0)
                    {
                        dtLogCierre.Columns.Add("Descripcion", typeof(string));
                        dtLogCierre.Columns.Add("Resultado", typeof(string));
                    }

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

        public void CargarInicio()
        {
            FechaInicioCierre = DateTime.Now;
            btnCancelar.Enabled = true;
            string sucursal = _D_DetalleOrden.TB_PARAMETRO("sucursalId");
            if (!_L_CierreCaja.CierreFueradeHorario(sucursal, DateTime.Now, DateTime.Now) && txtCierreHora.Text == "")
            {
                txtCierreHora.Enabled = true;
            }
            else
            {
                txtCierreHora.Enabled = false;
            }
        }

        public void CargarDatos()
        {
            FechaInicioCierre = DateTime.Now;
            lblPaso.Text = "Confirmación";
            lbPaso.Text = "Paso 1";
            btnFinalizar.Text = "Finalizar";
            btnCancelar.Enabled = true;

            sucursal = _D_DetalleOrden.TB_PARAMETRO("sucursalId");
            diaActivo = _D_Inicio.DiaActivo();

            dtLogCierre.Clear();
            dgvLogCierre.Refresh();

            dtCierreCaja.Clear();
            dgvCierredecaja.Refresh();

            dtPagoMovil.Clear();
            Dvg_OSconPagoMovil.Refresh();

            dtAsistenciaPendiente.Clear();
            Dvg_MarcajeAsistenciaPendiente.Refresh();

            dtConsignacion.Clear();
            Dvg_ConsignacionDeOS.Refresh();

            dtPtoVenta.Clear();
            Dvg_CierrePuntoVenta.Refresh();

            txtBox_observaciones_pg4.Text = "";

            //PUNTOS DE VENTA
            //Consulto si existen cerrados
            if (!_L_CierreCaja.CierreFueradeHorario(sucursal, DateTime.Now, DateTime.Now) && txtCierreHora.Text == "")
            {
                txtCierreHora.Enabled = true;
            }
            else
            {
                txtCierreHora.Enabled = false;
            }

            txtCierreHora.Text = "";

            DataTable dtPuntosCerrados = new DataTable();
            dtPuntosCerrados = _L_CierreCaja.CierrePuntodeVenta(sucursal, "", "", diaActivo);
            Dvg_CierrePuntoVenta.DataSource = null; // Desenlaza cualquier fuente
            Dvg_CierrePuntoVenta.Rows.Clear();      // Borra filas
            Dvg_CierrePuntoVenta.Columns.Clear();   // Borra columnas


            //DataTable dtPuntosCerrados = _L_CierreCaja.CierrePuntodeVenta(sucursal, "", "", diaActivo);

            //Si no se han cerrado lleno datos en 0
            if (dtPuntosCerrados.Rows.Count == 0)
            {
                DataTable dt = _L_CierreCaja.ObtienePuntosdeVenta("", diaActivo);

                CrearTabla("PuntodeVenta");

                foreach (DataRow fila in dt.Rows)
                {
                    dtPtoVenta.Rows.Add(fila["CodPunto"], fila["Tipo"], fila["Banco"], "", "0,00", "0,00", "0,00", "0,00");
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
                    //dtPtoVenta.Rows.Add(fila["Tipo"], fila[""CodPunto], fila[1], fila[2], fila[3], fila[4], fila[5], fila[6]);
                    dtPtoVenta.Rows.Add(fila["CodPunto"], fila["Tipo"], fila["Banco"], fila["NroLote"], fila["ManualTarjCredito"], fila["ManualTarjAmex"], fila["ManualTarjDebito"], fila["ManualTarjOtros"]);
                }


                // Asignar al DataGridView
                Dvg_CierrePuntoVenta.DataSource = dtPtoVenta;

                FormatoTabla("PuntodeVenta");
            }


            //OS CON PAGOMOVIL
            dtPagoMovil = new DataTable();
            dtPagoMovil = _L_CierreCaja.ObtineneCambioCierre(diaActivo, sucursal);
            Dvg_OSconPagoMovil.DataSource = null; // Desenlaza cualquier fuente
            Dvg_OSconPagoMovil.Rows.Clear();      // Borra filas
            Dvg_OSconPagoMovil.Columns.Clear();   // Borra columnas

            // Asignar al DataGridView
            Dvg_OSconPagoMovil.DataSource = dtPagoMovil;

            FormatoTabla("PagoMovil");

            DataTable dtBancos = _L_CierreCaja.ObtieneBancosPagoMovil(sucursal);

            //ASISTENCIA PENDIENTE
            //DataTable dtAsistenciaPendiente = _L_CierreCaja.VerificaAsistenciaPendiente(diaActivo.ToString("yyyyMMdd"), "PEND");

            //// Asignar al DataGridView
            //Dvg_MarcajeAsistenciaPendiente.DataSource = dtAsistenciaPendiente;

            //FormatoTabla("Asistencia");

            Dvg_MarcajeAsistenciaPendiente.EditMode = DataGridViewEditMode.EditProgrammatically;

            //CONSIGNACION
            dtConsignacion = _L_CierreCaja.ConsultaOsDia(diaActivo, sucursal);

            // Asignar al DataGridView
            Dvg_ConsignacionDeOS.DataSource = dtConsignacion;

            FormatoTabla("Consignacion");

            //CIERRE DE CAJA
            CrearTabla("CierredeCaja");

            dtCierreCaja = _L_CierreCaja.ObtienePagosCierreCaja(sucursal);


            // Asignar al DataGridView
            dgvCierredecaja.DataSource = dtCierreCaja;

            FormatoTabla("CierredeCaja");
        }
        public void FormatoTabla(string tipo)
        {
            switch (tipo)
            {
                case "PuntodeVenta":



                    // Asignar ancho personalizado a cada columna
                    Dvg_CierrePuntoVenta.Columns["CodPunto"].Width = 0;
                    Dvg_CierrePuntoVenta.Columns["CodPunto"].Visible = false;
                    Dvg_CierrePuntoVenta.Columns["Tipo"].Width = 50;
                    Dvg_CierrePuntoVenta.Columns["Banco"].Width = 100;
                    Dvg_CierrePuntoVenta.Columns["Nro. Lote"].Width = 100;
                    Dvg_CierrePuntoVenta.Columns["Total T. Crédito"].Width = 120;
                    Dvg_CierrePuntoVenta.Columns["Total T. Amex"].Width = 120;
                    Dvg_CierrePuntoVenta.Columns["Total T. Débito"].Width = 120;
                    Dvg_CierrePuntoVenta.Columns["Total T. Otros"].Width = 120;


                    Dvg_CierrePuntoVenta.Columns["Banco"].ReadOnly = true;

                    //Dvg_CierrePuntoVenta.DefaultCellStyle.Font = new Font("Century Gothic", 20);
                    Dvg_CierrePuntoVenta.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 9);
                    Dvg_CierrePuntoVenta.DefaultCellStyle.Font = new Font("Century Gothic", 9);
                    break;

                case "Consignacion":

                    // Asignar ancho personalizado a cada columna
                    Dvg_ConsignacionDeOS.Columns["Orden"].Width = 60;
                    Dvg_ConsignacionDeOS.Columns["CodLab"].Visible = false;
                    Dvg_ConsignacionDeOS.Columns["Lab"].Width = 100;
                    Dvg_ConsignacionDeOS.Columns["CodServicio"].Visible = false;
                    Dvg_ConsignacionDeOS.Columns["Servicio"].Width = 160;
                    Dvg_ConsignacionDeOS.Columns["CodVendedor"].Width = 120;
                    Dvg_ConsignacionDeOS.Columns["Vendedor"].Width = 290;

                    Dvg_ConsignacionDeOS.Columns["Orden"].ReadOnly = true;
                    Dvg_ConsignacionDeOS.Columns["Lab"].ReadOnly = true;
                    Dvg_ConsignacionDeOS.Columns["Servicio"].ReadOnly = true;


                    Dvg_ConsignacionDeOS.Columns["Orden"].HeaderText = "Orden";
                    Dvg_ConsignacionDeOS.Columns["Lab"].HeaderText = "Laboratorio";
                    Dvg_ConsignacionDeOS.Columns["Servicio"].HeaderText = "Servicio";
                    Dvg_ConsignacionDeOS.Columns["CodVendedor"].HeaderText = "Código";
                    Dvg_ConsignacionDeOS.Columns["Vendedor"].HeaderText = "Vendedor";

                    Dvg_ConsignacionDeOS.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 9);
                    Dvg_ConsignacionDeOS.DefaultCellStyle.Font = new Font("Century Gothic", 9);

                    break;

                case "CierredeCaja":

                    // Asignar ancho personalizado a cada columna
                    dgvCierredecaja.Columns["TipoTotal"].Width = 250;
                    dgvCierredecaja.Columns["Total"].Width = 100;

                    dgvCierredecaja.Columns["TipoTotal"].HeaderText = "Tipo Total";
                    dgvCierredecaja.Columns["Total"].HeaderText = "Total";

                    dgvCierredecaja.Columns["TipoTotal"].ReadOnly = true;

                    dgvCierredecaja.DefaultCellStyle.Font = new Font("Century Gothic", 9);
                    // Change the font for the COLUMN HEADERS
                    dgvCierredecaja.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 9);
                    dgvCierredecaja.RowTemplate.Height = 30; // Puedes ajustar el número a tu gusto
                    dgvCierredecaja.ColumnHeadersVisible = false;
                    dgvCierredecaja.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                    dgvCierredecaja.EnableHeadersVisualStyles = false; // Opcional si también quieres personalizar encabezados
                    dgvCierredecaja.GridColor = Color.FromArgb(200, 200, 200);


                    break;

                case "LogCierre":

                    // Asignar ancho personalizado a cada columna
                    dgvLogCierre.Columns["Descripcion"].Width = 245;
                    dgvLogCierre.Columns["Resultado"].Width = 177;
                    dgvLogCierre.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 9);
                    dgvLogCierre.DefaultCellStyle.Font = new Font("Century Gothic", 9);

                    dgvLogCierre.Columns["Descripcion"].HeaderText = "Descripción";

                    dgvLogCierre.Columns["Descripcion"].ReadOnly = true;
                    dgvLogCierre.Columns["Resultado"].ReadOnly = true;

                    dgvLogCierre.ClearSelection();
                    dgvLogCierre.CurrentCell = null;

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

                    Dvg_OSconPagoMovil.Columns["Orden"].Width = 110;
                    Dvg_OSconPagoMovil.Columns["Referencia"].Width = 150;
                    Dvg_OSconPagoMovil.Columns["BancoReceptor"].Width = 150;
                    Dvg_OSconPagoMovil.Columns["MontoVueltoRef"].Width = 115;
                    Dvg_OSconPagoMovil.Columns["MontoVueltoBs"].Width = 115;

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

                    Dvg_OSconPagoMovil.Columns["CodBancoEmisor"].Visible = false;
                    Dvg_OSconPagoMovil.Columns["CodigoBancoEmisor"].Visible = false;
                    Dvg_OSconPagoMovil.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 9);
                    Dvg_OSconPagoMovil.DefaultCellStyle.Font = new Font("Century Gothic", 9);


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


                    string[] columnasTimePicker = { "HORAENTRADAT1", "HORASALIDAT1", "HORAENTRADAT2", "HORASALIDAT2" };

                    foreach (string columna in columnasTimePicker)
                    {
                        if (Dvg_MarcajeAsistenciaPendiente.Columns.Contains(columna))
                        {
                            int colIndex = Dvg_MarcajeAsistenciaPendiente.Columns[columna].Index;

                            // Eliminar columna original
                            Dvg_MarcajeAsistenciaPendiente.Columns.RemoveAt(colIndex);

                            // Agregar columna TimePicker
                            var timePickerCol = new DataGridViewTimePickerColumn
                            {
                                Name = columna,
                                HeaderText = GetHeaderText(columna),
                                Width = 100,
                                DataPropertyName = columna,
                                DefaultCellStyle = new DataGridViewCellStyle { Format = "t" }
                            };

                            Dvg_MarcajeAsistenciaPendiente.Columns.Insert(colIndex, timePickerCol);
                        }
                    }


                    foreach (DataGridViewRow row in Dvg_MarcajeAsistenciaPendiente.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            // Verificar cada columna que quieres proteger
                            VerificarYProtegerCelda(row, "HORAENTRADAT1");
                            VerificarYProtegerCelda(row, "HORASALIDAT1");
                            VerificarYProtegerCelda(row, "HORAENTRADAT2");
                            VerificarYProtegerCelda(row, "HORASALIDAT2");
                        }
                    }

                    Dvg_MarcajeAsistenciaPendiente.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 9);
                    Dvg_MarcajeAsistenciaPendiente.DefaultCellStyle.Font = new Font("Century Gothic", 9);


                    break;

                case "Usuarios":

                    // Asignar ancho personalizado a cada columna
                    Dgv_Usuarios.Columns["COD_USR"].Visible = false;
                    Dgv_Usuarios.Columns["Nombre"].Width = 320;
                    Dgv_Usuarios.Columns["COD_EMPLEADO"].HeaderText = "Código";
                    Dgv_Usuarios.Columns["COD_EMPLEADO"].Width = 75;
                    Dgv_Usuarios.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 9);
                    Dgv_Usuarios.DefaultCellStyle.Font = new Font("Century Gothic", 9);



                    break;

                default:
                    break;
            }
        }

        private string GetHeaderText(string columnName)
        {
            switch (columnName)
            {
                case "HORAENTRADAT1":
                    return "Entrada 1";
                case "HORASALIDAT1":
                    return "Salida 1";
                case "HORAENTRADAT2":
                    return "Entrada 2";
                case "HORASALIDAT2":
                    return "Salida 2";
                default:
                    return columnName;
            }
        }

        private void VerificarYProtegerCelda(DataGridViewRow row, string columnName)
        {

            string[] columnasControlar = { "HORAENTRADAT1", "HORASALIDAT1", "HORAENTRADAT2", "HORASALIDAT2" };

            if (Dvg_MarcajeAsistenciaPendiente.Columns.Contains(columnName) &&
        columnasControlar.Contains(columnName))
            {
                DataGridViewCell cell = row.Cells[columnName];

                // Si la celda tiene datos, hacerla de solo lectura y no seleccionable
                if (cell.Value != null && !string.IsNullOrWhiteSpace(cell.Value.ToString()))
                {
                    cell.ReadOnly = true;
                    //cell.Style.BackColor = Color.LightGray;
                    //cell.Style.SelectionBackColor = Color.LightGray; // Mismo color cuando está seleccionada
                    //cell.Style.SelectionForeColor = Color.DarkGray;
                }
                else
                {
                    cell.ReadOnly = false;
                    cell.Style.BackColor = Color.White;
                    //cell.Style.SelectionBackColor = SystemColors.Highlight; // Color normal de selección
                    //cell.Style.SelectionForeColor = SystemColors.HighlightText;
                }
            }
        }


        //        // Si la celda tiene datos, hacerla de solo lectura y no seleccionable
        //        if (cell.Value != null && !string.IsNullOrWhiteSpace(cell.Value.ToString()))
        //        {
        //            cell.ReadOnly = true;
        //            //cell.Style.BackColor = Color.LightGray;
        //            //cell.Style.SelectionBackColor = Color.LightGray; // Mismo color cuando está seleccionada
        //            //cell.Style.SelectionForeColor = Color.DarkGray;
        //        }
        //        else
        //        {
        //            cell.ReadOnly = false;
        //            cell.Style.BackColor = Color.White;
        //            //cell.Style.SelectionBackColor = SystemColors.Highlight; // Color normal de selección
        //            //cell.Style.SelectionForeColor = SystemColors.HighlightText;
        //        }
        //    }
        //}

        private void btn_Cancelar_pg2_Click(object sender, EventArgs e)
        {
            tcCierreCaja.SelectedIndex = 0;
            lblPaso.Text = "Confirmación";
            lbPaso.Text = "Paso 1";
            ValideClave = false;
        }

        public void RegresarInicio()
        {
            tcCierreCaja.SelectedIndex = 0;
            lblPaso.Text = "Confirmación";
            lbPaso.Text = "Paso 1";
        }


        private void btn_Cancelar_pg3_Click(object sender, EventArgs e)
        {
            tcCierreCaja.SelectedIndex = 1;
            lblPaso.Text = "Asistencia";
            lbPaso.Text = "Paso 2";
        }
        // 1) En tu formulario declara esto a nivel de clase:
        private HashSet<(int row, int col)> _celdasAutorizadas = new HashSet<(int, int)>();

        // 2) Suscribe ambos eventos en tu Form_Load o constructor:

        // 3) En CellBeginEdit pides la clave y, si es correcta,
        //    guardas la posición de la celda en el HashSet:


        // 4) En CellEndEdit revisas si la celda estuvo autorizada.
        //    Si NO, la vacías; si SÍ, la aceptas y quitas la autorización
        //    para que, si editan de nuevo, vuelvan a pedir clave:
        //private void Dgv_MarcajeAsistenciaPendiente_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (Dvg_MarcajeAsistenciaPendiente.Columns[e.ColumnIndex] is DataGridViewTimePickerColumn)
        //    {
        //        var key = (e.RowIndex, e.ColumnIndex);
        //        if (!_celdasAutorizadas.Contains(key))
        //        {
        //            // limpiamos ese tiempo “por defecto” que se coló
        //            Dvg_MarcajeAsistenciaPendiente.Rows[e.RowIndex]
        //                .Cells[e.ColumnIndex].Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            // ya fue autorizado y guardado: quitamos la marca
        //            _celdasAutorizadas.Remove(key);
        //        }
        //    }
        //}

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


        //private void Dvg_MarcajeAsistenciaPendiente_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        //    {
        //        var columna = Dvg_MarcajeAsistenciaPendiente.Columns[e.ColumnIndex];

        //        if (columna is DataGridViewTimePickerColumn)
        //        {
        //            _FrmClaveGerente.ShowDialog();

        //            if (_FrmClaveGerente.ClaveCorrecta && _FrmClaveGerente.DialogResult == DialogResult.OK)
        //            {
        //                Dvg_MarcajeAsistenciaPendiente.BeginEdit(true);
        //                GerenteAutoriza = _FrmClaveGerente.RetornoNombreUsuario();
        //            }
        //            else
        //            {
        //                // ⚠️ Cancelar cualquier edición activa
        //                if (Dvg_MarcajeAsistenciaPendiente.IsCurrentCellInEditMode)
        //                    Dvg_MarcajeAsistenciaPendiente.CancelEdit();

        //                // ⚠️ Forzar la celda a un valor nulo
        //                Dvg_MarcajeAsistenciaPendiente.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = DBNull.Value;

        //                // Opcional: refrescar visualmente el grid
        //                Dvg_MarcajeAsistenciaPendiente.RefreshEdit();
        //            }
        //        }
        //    }
        //}

        // 3) En CellBeginEdit pides la clave y, si es correcta,
        //    guardas la posición de la celda en el HashSet:
        //private void Dgv_MarcajeAsistenciaPendiente_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        //{
        //    var col = Dvg_MarcajeAsistenciaPendiente.Columns[e.ColumnIndex];
        //    if (col is DataGridViewTimePickerColumn)
        //    {
        //        _FrmClaveGerente.ShowDialog();
        //        if (_FrmClaveGerente.DialogResult == DialogResult.OK && _FrmClaveGerente.ClaveCorrecta)
        //        {
        //            // autorizamos esta celda:
        //            _celdasAutorizadas.Add((e.RowIndex, e.ColumnIndex));
        //            GerenteAutoriza = _FrmClaveGerente.RetornoNombreUsuario();
        //            return;
        //        }

        //        // si no autoriza, cancelamos edición de una vez 
        //        e.Cancel = true;
        //        Dvg_MarcajeAsistenciaPendiente.Rows[e.RowIndex]
        //            .Cells[e.ColumnIndex].Value = DBNull.Value;
        //    }
        //}
        private void Dvg_ConsignacionDeOS_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var columna = Dvg_ConsignacionDeOS.Columns[e.ColumnIndex];

                if (columna.Name == "CodVendedor")
                {
                    //string codigo = Dvg_ConsignacionDeOS.Rows[e.RowIndex].Cells["CodVendedor"].Value?.ToString();
                    DataTable dtUsuarios = _L_CierreCaja.ObtieneUsuarios(sucursal);
                    usuarios = ConvertToList<Usuario>(dtUsuarios);

                    Dgv_Usuarios.DataSource = dtUsuarios;

                    FormatoTabla("Usuarios");
                    //Formato_Dgv_Pnl3_ColoresLC();

                    if (Pnl2_ListadoDeVendedores.Visible == true)
                    {
                        Pnl2_ListadoDeVendedores.Visible = false;
                    }
                    else
                    {
                        Pnl2_ListadoDeVendedores.Visible = true;
                    }
                }
            }
        }

        public static List<T> ConvertToList<T>(DataTable table) where T : new()
        {
            List<T> list = new List<T>();

            foreach (DataRow row in table.Rows)
            {
                T obj = new T();
                foreach (DataColumn col in table.Columns)
                {
                    PropertyInfo prop = typeof(T).GetProperty(col.ColumnName);
                    if (prop != null && row[col] != DBNull.Value)
                        prop.SetValue(obj, Convert.ChangeType(row[col], prop.PropertyType));
                }
                list.Add(obj);
            }

            return list;
        }

        private void txtFiltroVendedor_TextChanged(object sender, EventArgs e)
        {
            // Filtrar los datos según el texto ingresado en el TextBox
            _L_CierreCaja.FiltrarUsuario(txtFiltroVendedor.Text.ToLower(), rbNombre, rbCodigo, Dgv_Usuarios, usuarios, listaTemporalUsuarios);

        }

        private void Dgv_Usuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Obtener datos del usuario seleccionado
                string codUsr = Dgv_Usuarios.Rows[e.RowIndex].Cells["COD_EMPLEADO"].Value?.ToString();

                string nombreUsr = Dgv_Usuarios.Rows[e.RowIndex].Cells["Nombre"].Value?.ToString();

                // Validar que haya una fila seleccionada en Dvg_ConsignacionDeOS
                if (Dvg_ConsignacionDeOS.CurrentRow != null)
                {
                    DataGridViewRow filaActiva = Dvg_ConsignacionDeOS.CurrentRow;

                    filaActiva.Cells["CodVendedor"].Value = codUsr;

                    filaActiva.Cells["Vendedor"].Value = nombreUsr;
                    Pnl2_ListadoDeVendedores.Visible = false;
                }
            }
        }

        private void btn_Siguiente_pg3_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow fila in Dvg_ConsignacionDeOS.Rows)
            {
                // Ignorar fila nueva si está habilitada la opción de agregar
                if (!fila.IsNewRow)
                {
                    var Orden = fila.Cells["Orden"].Value?.ToString().Trim();
                    var CodVendedor = fila.Cells["CodVendedor"].Value?.ToString().Trim();

                    _L_CierreCaja.ModificaVendedor(Orden, CodVendedor, TB_USUARIO.COD_EMPLEADO, sucursal);
                }
            }
            tcCierreCaja.SelectedIndex = 3;
            dgvCierredecaja.ClearSelection();

            lblPaso.Text = "Cierre de Caja";
            lbPaso.Text = "Paso 4";
        }


        private double GetValorFila(int filaIndex)
        {
            var valor = dgvCierredecaja.Rows[filaIndex].Cells["Total"].Value;
            if (valor != null && double.TryParse(valor.ToString(), out double resultado))
            {
                return resultado;
            }
            return 0;
        }

        private void dgvCierredecaja_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvCierredecaja.Columns[e.ColumnIndex].Name == "Total")
                {
                    var cell = dgvCierredecaja.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    if (decimal.TryParse(cell.Value?.ToString(), out decimal valor))
                    {
                        // Formato con punto como miles y coma como decimales
                        CultureInfo cultura = new CultureInfo("es-VE"); // o "es-ES"
                        cell.Value = valor.ToString("#,##0.00", cultura);
                    }
                }

                double existenteEnCaja = GetValorFila(0);
                double efectivo = GetValorFila(5);
                double debito = GetValorFila(6);
                double tarjetaCredito = GetValorFila(7);
                double ivaRetenido = GetValorFila(8);
                double islrRetenido = GetValorFila(9);

                double transferencia = GetValorFila(10);
                //double transferenciaDivisa = GetValorFila(11);

                double diferencia = existenteEnCaja - (efectivo + debito + tarjetaCredito + ivaRetenido + islrRetenido + transferencia);

                // Mostrar el resultado en la fila "Diferencia" (fila 6)
                dgvCierredecaja.Rows[11].Cells["Total"].Value = diferencia.ToString("N2");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al calcular diferencia: " + ex.Message);
            }
        }

        private void Dgv_MarcajeAsistenciaPendiente_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (!(Dvg_MarcajeAsistenciaPendiente.Columns[e.ColumnIndex] is DataGridViewTimePickerColumn))
                return;

            // VERIFICAR SI LA CELDA YA TIENE DATOS - AGREGAR ESTA VALIDACIÓN
            DataGridViewCell celda = Dvg_MarcajeAsistenciaPendiente.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (celda.Value != null && !string.IsNullOrWhiteSpace(celda.Value.ToString()))
            {
                // Si la celda ya tiene datos, no hacer nada
                return;
            }

            _FrmClaveGerente.ShowDialog();
            if (_FrmClaveGerente.DialogResult == DialogResult.OK
                && _FrmClaveGerente.ClaveCorrecta)
            {
                ValideClave = true;
                GerenteAutoriza = _FrmClaveGerente.RetornoNombreUsuario();
                // Aquí sí iniciamos la edición y aparece el picker
                Dvg_MarcajeAsistenciaPendiente.CurrentCell =
                    Dvg_MarcajeAsistenciaPendiente.Rows[e.RowIndex].Cells[e.ColumnIndex];
                Dvg_MarcajeAsistenciaPendiente.BeginEdit(true);
            }
            else
            {
                ValideClave = false;
                // Al no llamar a BeginEdit, nunca instancias el DateTimePicker.
                // Y de paso dejas el valor en null/blank si quieres:
                Dvg_MarcajeAsistenciaPendiente.Rows[e.RowIndex]
                    .Cells[e.ColumnIndex].Value = DBNull.Value;
            }
        }

        private void Dvg_MarcajeAsistenciaPendiente_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string[] columnasControlar = { "HORAENTRADAT1", "HORASALIDAT1", "HORAENTRADAT2", "HORASALIDAT2" };

                string nombreColumna = Dvg_MarcajeAsistenciaPendiente.Columns[e.ColumnIndex].Name;

                // Verificar si es una columna que controlamos
                if (columnasControlar.Contains(nombreColumna))
                {
                    DataGridViewCell celda = Dvg_MarcajeAsistenciaPendiente.Rows[e.RowIndex].Cells[e.ColumnIndex];

                    // Si la celda tiene datos (está en gris), evitar la selección
                    if (celda.Value != null && !string.IsNullOrWhiteSpace(celda.Value.ToString()))
                    {
                        // Limpiar la selección actual
                        Dvg_MarcajeAsistenciaPendiente.ClearSelection();

                        // Opcional: Seleccionar la primera celda editable o mantener sin selección
                        // Esto evita que quede seleccionada visualmente
                        return;
                    }
                }
            }
        }
        public class VerticalLabel : Control
        {
            public bool Invertir { get; set; } = false; // si quieres que el texto vaya de abajo hacia arriba

            public VerticalLabel()
            {
                this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                this.BackColor = Color.Transparent;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                using (StringFormat format = new StringFormat())
                {
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;

                    e.Graphics.TranslateTransform(this.Width / 2, this.Height / 2);

                    if (Invertir)
                        e.Graphics.RotateTransform(-90); // de abajo hacia arriba
                    else
                        e.Graphics.RotateTransform(90);  // de arriba hacia abajo

                    e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), 0, 0, format);
                    e.Graphics.ResetTransform();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (btnFinalizar.Text == "Finalizar")
            {
                bool cierreEncero = false;
                Conexion cn = new Conexion();
                SqlConnection connection = cn.LeerCadena();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction();
                command.Connection = connection;
                command.Transaction = transaction;
                command.Parameters.Clear();
                command.CommandTimeout = 300000;

                try
                {
                    //Invenvio
                    string rutaInvenvio;
                    string nombreInvenvio;
                    rutaInvenvio = _D_DetalleOrden.TB_PARAMETRO("RutaInvenvio");
                    nombreInvenvio = _D_DetalleOrden.TB_PARAMETRO("NombreArchInv");



                    //Existencia en Caja
                    if (!_L_CierreCaja.ValidaExistenciaCaja(dgvCierredecaja))
                    {
                        _FrmMensajes.co = 3;
                        _FrmMensajes.avisomensaje("¿Está seguro de querer cerrar la caja en cero?");
                        _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                        _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                        _FrmMensajes.ShowDialog();

                        if (_FrmMensajes.DialogResult == DialogResult.OK)
                        {
                            _FrmClaveGerente.ShowDialog();
                            if (_FrmClaveGerente.ClaveCorrecta == true)
                            {
                                cierreEncero = true;
                                if (_FrmClaveGerente.DialogResult == DialogResult.OK)
                                {
                                    //Dvg_MarcajeAsistenciaPendiente.BeginEdit(true); // inicia edición con un clic
                                    GerenteAutoriza = _FrmClaveGerente.RetornoNombreUsuario();
                                }
                                else
                                {
                                    return;
                                }
                            }
                            else
                            {
                                return;
                            }

                        }
                        else
                        {

                            return;
                        }
                    }

                    dtLogCierre.Clear();
                    CrearTabla("LogCierre");
                    dgvLogCierre.DataSource = dtLogCierre;
                    FormatoTabla("LogCierre");

                    dtLogCierre.Rows.Add("Inicio", FechaInicioCierre);
                    dtLogCierre.Rows.Add("Confirmación", "✔ Completado");
                    dtLogCierre.Rows.Add("Asistencia", "✔ Completado");
                    dtLogCierre.Rows.Add("Ordenes", "✔ Completado");

                    dgvLogCierre.Refresh();



                    //SP Cierre de Caja
                    if (_L_CierreCaja.CierreDeCaja(dgvCierredecaja, diaActivo, sucursal, txtBox_observaciones_pg4.Text, TB_USUARIO.COD_USR, mostrarError, command))
                    {
                        dtLogCierre.Rows.Add("Cierre de Caja", "✔ Completado");
                        dgvLogCierre.DataSource = dtLogCierre;
                        dgvLogCierre.Refresh();
                    }
                    else
                    {
                        dtLogCierre.Rows.Add("Cierre de Caja", "❌ Fallido");
                        dgvLogCierre.DataSource = dtLogCierre;
                        dgvLogCierre.Refresh();
                        if (command.Transaction != null && command.Transaction.Connection != null)
                        {
                            command.Transaction.Rollback();
                        }
                        return;
                    }

                    if (_L_CierreCaja.HayDatosInvenvioTXT(sucursal, diaActivo.ToString("yyyyMMdd"), rutaInvenvio, nombreInvenvio, command))
                    {
                        if (_L_CierreCaja.GeneraInvenvioTXT(sucursal, diaActivo.ToString("yyyyMMdd"), rutaInvenvio, nombreInvenvio, command))
                        {
                            dtLogCierre.Rows.Add("Archivo Invenvio.txt", "✔ Completado");
                            dgvLogCierre.DataSource = dtLogCierre;
                            dgvLogCierre.Refresh();
                        }
                        else
                        {
                            dtLogCierre.Rows.Add("Archivo Invenvio.txt", "❌ Fallido");
                            dgvLogCierre.DataSource = dtLogCierre;
                            dgvLogCierre.Refresh();
                        }
                    }

                    if (!_L_CierreCaja.ActualizarParamCierreCaja(sucursal))
                    {
                        dtLogCierre.Rows.Add("Error Actualizando parametros", "❌ Fallido");
                        dgvLogCierre.DataSource = dtLogCierre;
                        dgvLogCierre.Refresh();
                        command.Transaction.Rollback();
                        return;
                    }

                    if (!_L_CierreCaja.DesbloqueSistema("1", sucursal))
                    {
                        dtLogCierre.Rows.Add("Error Actualizando parametros", "❌ Fallido");
                        dgvLogCierre.DataSource = dtLogCierre;
                        dgvLogCierre.Refresh();
                        command.Transaction.Rollback();
                        return;
                    }

                    if (!_L_CierreCaja.ActualizarFacturas(sucursal, command))
                    {
                        dtLogCierre.Rows.Add("Error Actualizando Facturas", "❌ Fallido");
                        dgvLogCierre.DataSource = dtLogCierre;
                        dgvLogCierre.Refresh();
                        command.Transaction.Rollback();
                        return;
                    }

                    dtLogCierre.Rows.Add("Generando Libro de Ventas", "...");
                    dgvLogCierre.DataSource = dtLogCierre;
                    dgvLogCierre.Refresh();

                    if (_L_CierreCaja.LibroVenta(diaActivo, diaActivo, command))
                    {
                        foreach (DataRow row in dtLogCierre.Rows)
                        {
                            if (row["Descripcion"].ToString() == "Generando Libro de Ventas")
                            {
                                row["Resultado"] = "✔ Completado";
                                dgvLogCierre.Refresh();
                                break;
                            }
                        }

                    }
                    else
                    {
                        foreach (DataRow row in dtLogCierre.Rows)
                        {
                            if (row["Descripcion"].ToString() == "Generando Libro de Ventas")
                            {
                                row["Resultado"] = "❌ Fallido";
                                command.Transaction.Rollback();
                                dgvLogCierre.Refresh();
                                break;
                            }
                        }
                        return;
                    }

                    dtLogCierre.Rows.Add("Consolidando movimientos", "...");
                    dgvLogCierre.DataSource = dtLogCierre;
                    dgvLogCierre.Refresh();

                    if (_L_CierreCaja.InventarioFaltante(diaActivo, command))
                    {
                        foreach (DataRow row in dtLogCierre.Rows)
                        {
                            if (row["Descripcion"].ToString() == "Consolidando movimientos")
                            {
                                row["Resultado"] = "✔ Completado";
                                dgvLogCierre.Refresh();
                                break;
                            }
                        }

                    }
                    else
                    {
                        foreach (DataRow row in dtLogCierre.Rows)
                        {
                            if (row["Descripcion"].ToString() == "Consolidando movimientos")
                            {
                                row["Resultado"] = "❌ Fallido";
                                dgvLogCierre.Refresh();
                                command.Transaction.Rollback();
                                break;
                            }
                        }
                        return;
                    }

                    _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "027", TB_USUARIO.COD_EMPLEADO, "Fin de cierre definitivo");

                    //ACC
                    dtLogCierre.Rows.Add("Generando ACC", "...");
                    dgvLogCierre.DataSource = dtLogCierre;
                    dgvLogCierre.Refresh();

                    if (_L_CierreCaja.CreaAcc(diaActivo, sucursal, command))
                    {
                        foreach (DataRow row in dtLogCierre.Rows)
                        {
                            if (row["Descripcion"].ToString() == "Generando ACC")
                            {
                                row["Resultado"] = "✔ Completado";
                                dgvLogCierre.Refresh();
                                break;
                            }
                        }

                    }
                    else
                    {
                        foreach (DataRow row in dtLogCierre.Rows)
                        {
                            if (row["Descripcion"].ToString() == "Generando ACC")
                            {
                                row["Resultado"] = "❌ Fallido";
                                dgvLogCierre.Refresh();
                                command.Transaction.Rollback();
                                break;
                            }
                        }
                        return;
                    }

                    //XML ACC
                    dtLogCierre.Rows.Add("Creando zip xml", "...");
                    dgvLogCierre.DataSource = dtLogCierre;
                    dgvLogCierre.Refresh();

                    if (_L_CierreCaja.CreaXMLACC(sucursal, command))
                    {
                        foreach (DataRow row in dtLogCierre.Rows)
                        {
                            if (row["Descripcion"].ToString() == "Creando zip xml")
                            {
                                row["Resultado"] = "✔ Completado";
                                dgvLogCierre.Refresh();
                                break;
                            }
                        }

                    }
                    else
                    {
                        foreach (DataRow row in dtLogCierre.Rows)
                        {
                            if (row["Descripcion"].ToString() == "Creando zip xml")
                            {
                                row["Resultado"] = "❌ Fallido";
                                dgvLogCierre.Refresh();
                                command.Transaction.Rollback();
                                break;
                            }
                        }
                        return;
                    }

                    _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "070", TB_USUARIO.COD_EMPLEADO, "Se generaron los ACC correctamente");

                    int DiasAAgregar = 0;
                    DayOfWeek dia = diaActivo.DayOfWeek;
                    string TrabajaDomingos = _D_DetalleOrden.TB_PARAMETRO("TrabajaDomingo");
                    if (dia == DayOfWeek.Saturday)
                    {
                        if (TrabajaDomingos == "1")
                            DiasAAgregar = 1;
                        else
                            DiasAAgregar = 2;
                    }
                    else

                    {
                        DiasAAgregar = 1;
                    }

                    FrmPrincipal frmPrincipal = this.ParentForm as FrmPrincipal;

                    if (frmPrincipal != null)
                    {
                        frmPrincipal.ActualizarTextoLabel(diaActivo.AddDays(DiasAAgregar).ToString("dd/MM/yyyy"));
                    }

                    //dtLogCierre.Rows.Add("Imprimiendo reportes", "...");
                    //dgvLogCierre.DataSource = dtLogCierre;
                    //dgvLogCierre.Refresh();

                    //FrmPrueba frmReportes = new FrmPrueba();

                    //frmReportes.ReportesCierreCaja();

                    //foreach (DataRow row in dtLogCierre.Rows)
                    //{
                    //    if (row["Descripcion"].ToString() == "Imprimiendo reportes")
                    //    {
                    //        row["Resultado"] = "✔ Completado";
                    //        dgvLogCierre.Refresh();
                    //        break;
                    //    }
                    //}
                    command.Transaction.Commit();

                    dtLogCierre.Rows.Add("Fin", DateTime.Now);
                    dgvLogCierre.Refresh();
                    //tcCierreCaja.SelectedIndex = 0;
                    btnFinalizar.Text = "Confirmar";
                    btnCancelar.Enabled = false;

                    //CargarDatos();

                    //dtLogCierre.Rows.Add("Cierre de Caja", "✔ Completado");
                    //dgvLogCierre.DataSource = dtLogCierre;
                    //dgvLogCierre.Refresh();

                    FrmPrueba frmReportes = new FrmPrueba();

                    frmReportes.ReportesCierreCaja(cierreEncero);
                }
                catch (Exception ex)
                {
                    command.Transaction.Rollback();
                }
            }
            else
            {
                tcCierreCaja.SelectedIndex = 0;
                CargarDatos();
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            tcCierreCaja.SelectedIndex = 2;
            lblPaso.Text = "Ordenes";
            lbPaso.Text = "Paso 3";
        }

        private void btn_MarcarSalida_pg2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _onCancelarSolicitado?.Invoke();
            //tcCierreCaja.SelectedIndex = 0;
            //this.Hide();
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        // 1) Al entrar en modo edición…
        private void Dvg_OSconPagoMovil_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // Comprueba si la celda que se edita es de la columna "Referencia"
            if (Dvg_OSconPagoMovil.CurrentCell.ColumnIndex ==
                Dvg_OSconPagoMovil.Columns["Referencia"].Index)
            {
                // Es un TextBox por defecto en DataGridViewTextBoxColumn
                var tb = e.Control as TextBox;
                if (tb != null)
                {
                    // Quita cualquier handler previo para no enganchar varios
                    tb.KeyPress -= ReferenciaColumn_KeyPress;

                    // Limita la longitud a 10
                    tb.MaxLength = 10;

                    // Engancha el KeyPress para filtrar sólo dígitos
                    tb.KeyPress += ReferenciaColumn_KeyPress;
                }
            }
            else
            {
                // Si sale de esa columna, opcionalmente remueve el handler
                var tb = e.Control as TextBox;
                if (tb != null)
                    tb.KeyPress -= ReferenciaColumn_KeyPress;
            }
        }

        // 2) Valida cada pulsación de tecla
        private void ReferenciaColumn_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir Backspace, Delete, flechas, etc.
            if (char.IsControl(e.KeyChar))
                return;

            // Permitir sólo dígitos
            if (!char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void Dvg_CierrePuntoVenta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Simplemente finaliza la edición, el evento Leave se disparará automáticamente
                Dvg_CierrePuntoVenta.EndEdit();
                e.Handled = true; // Evita el comportamiento por defecto

                int currentRow = dataGridView1.CurrentCell.RowIndex;
                int currentCol = dataGridView1.CurrentCell.ColumnIndex;

                // Si no es la última columna, pasa a la siguiente columna
                if (currentCol < dataGridView1.Columns.Count - 1)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[currentRow].Cells[currentCol + 1];
                }
                // Si es la última columna, pasa a la primera columna de la siguiente fila
                else if (currentRow < dataGridView1.Rows.Count - 1)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[currentRow + 1].Cells[0];
                }
            }
        }

        private void FormatTextBoxValue(TextBox tb)
        {
            if (string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.Text = "0,00";
            }
            else
            {
                string cleanText = tb.Text.Replace(".", "").Replace(",", ".");
                if (decimal.TryParse(cleanText, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal value))
                {
                    tb.Text = value.ToString("N2");
                }
                else
                {
                    tb.Text = "0,00";
                }
            }

            // Actualizar el valor en el DataGridView
            if (Dvg_CierrePuntoVenta.CurrentCell != null)
            {
                Dvg_CierrePuntoVenta.CurrentCell.Value = tb.Text;
            }
        }

        private bool IsValidDecimal(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return true;

            string cleanText = text.Replace(".", "").Replace(",", ".");
            return decimal.TryParse(cleanText, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
        }

        private void TotalCredito_Validating(object sender, CancelEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                // Formatear el valor cuando pierde el foco
                FormatTextBoxValue(tb);

                // Aquí puedes agregar validación adicional
                if (!IsValidDecimal(tb.Text))
                {
                    e.Cancel = true; // Impide que pierda el foco si es inválido
                }
            }
        }


        private void Dvg_CierrePuntoVenta_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (Dvg_CierrePuntoVenta.CurrentCell == null)
                return;

            int columnIndex = Dvg_CierrePuntoVenta.CurrentCell.ColumnIndex;
            var tb = e.Control as TextBox;

            if (tb == null)
                return;

            // Comprueba si la celda que se edita es de la columna "Referencia"
            if (columnIndex == Dvg_CierrePuntoVenta.Columns["Nro. Lote"].Index)
            {

                // Quita cualquier handler previo para no enganchar varios
                tb.KeyPress -= NroLoteColumn_KeyPress;

                // Limita la longitud a 10
                tb.MaxLength = 10;

                // Engancha el KeyPress para filtrar sólo dígitos
                tb.KeyPress += NroLoteColumn_KeyPress;

                // Para otras columnas, remover los eventos que no deben tener
                tb.KeyPress -= TotalCredito_KeyPress;
                tb.KeyDown -= TotalCredito_KeyDown;
                tb.Leave -= TotalCredito_Leave;
                tb.Validating -= TotalCredito_Validating;
            }

            else if (Dvg_CierrePuntoVenta.CurrentCell != null && (columnIndex == Dvg_CierrePuntoVenta.Columns["Total T. Crédito"].Index)
                || Dvg_CierrePuntoVenta.CurrentCell.ColumnIndex == Dvg_CierrePuntoVenta.Columns["Total T. Amex"].Index
                || Dvg_CierrePuntoVenta.CurrentCell.ColumnIndex == Dvg_CierrePuntoVenta.Columns["Total T. Débito"].Index
                || Dvg_CierrePuntoVenta.CurrentCell.ColumnIndex == Dvg_CierrePuntoVenta.Columns["Total T. Otros"].Index)
            {
                tb.KeyPress -= TotalCredito_KeyPress;
                tb.KeyPress += TotalCredito_KeyPress;
                tb.KeyDown -= TotalCredito_KeyDown;
                tb.KeyDown += TotalCredito_KeyDown;
                tb.Validating -= TotalCredito_Validating;
                tb.Validating += TotalCredito_Validating;
                tb.Leave -= TotalCredito_Leave;
                tb.Leave += TotalCredito_Leave;
                tb.MaxLength = 10; // Opcional: límite de caracteres

            }
            else
            {
                // Remover eventos de otras columnas para evitar conflictos
                tb.KeyPress -= TotalCredito_KeyPress;
                tb.KeyDown -= TotalCredito_KeyDown;
                tb.Leave -= TotalCredito_Leave;
                tb.Validating -= TotalCredito_Validating;
            }


        }
        private void TotalCredito_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;

            // Permitir teclas de control (Backspace, Delete, etc.)
            if (char.IsControl(e.KeyChar))
                return;

            // Permitir dígitos
            if (char.IsDigit(e.KeyChar))
                return;

            // Permitir solo una coma
            if (e.KeyChar == ',' || (int)e.KeyChar == 44)
            {
                e.Handled = tb.Text.Contains(","); // Si ya hay coma, rechazar
                return;
            }

            // Permitir punto decimal (opcional, dependiendo de tu región)
            if (e.KeyChar == '.')
            {
                // Convertir punto a coma si prefieres comas decimales
                e.KeyChar = ',';
                e.Handled = tb.Text.Contains(","); // Si ya hay coma, rechazar
                return;
            }

            // Bloquear cualquier otro carácter
            e.Handled = true;


        }

        private void TotalCredito_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true; // Esto es importante para evitar el sonido del sistema

                TextBox tb = sender as TextBox;
                var currentCell = Dvg_CierrePuntoVenta.CurrentCell;

                // Validar que estamos en una columna de totales
                if (currentCell != null &&
                    (currentCell.ColumnIndex == Dvg_CierrePuntoVenta.Columns["Total T. Crédito"].Index ||
                     currentCell.ColumnIndex == Dvg_CierrePuntoVenta.Columns["Total T. Amex"].Index ||
                     currentCell.ColumnIndex == Dvg_CierrePuntoVenta.Columns["Total T. Débito"].Index ||
                     currentCell.ColumnIndex == Dvg_CierrePuntoVenta.Columns["Total T. Otros"].Index))
                {
                    // Formatear el número
                    if (tb != null)
                    {
                        string cleanText = tb.Text.Replace(".", "").Replace(",", ".");
                        if (decimal.TryParse(cleanText, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal value))
                        {
                            tb.Text = value.ToString("N2", CultureInfo.CurrentCulture);
                        }
                        else
                        {
                            tb.Text = "0,00";
                        }
                    }

                    // Finalizar la edición y mover a la siguiente celda
                    Dvg_CierrePuntoVenta.EndEdit();
                    MoveToNextCell();
                }
            }
        }

        // Método para mover a la siguiente celda
        private void MoveToNextCell()
        {
            if (Dvg_CierrePuntoVenta.CurrentCell != null)
            {
                int nextColumn = Dvg_CierrePuntoVenta.CurrentCell.ColumnIndex + 1;
                int currentRow = Dvg_CierrePuntoVenta.CurrentCell.RowIndex;

                if (nextColumn >= Dvg_CierrePuntoVenta.ColumnCount)
                {
                    nextColumn = 0;
                    currentRow++;

                    // Si es la última fila, agregar nueva fila
                    if (currentRow >= Dvg_CierrePuntoVenta.RowCount)
                    {
                        Dvg_CierrePuntoVenta.Rows.Add();
                    }
                }

                Dvg_CierrePuntoVenta.CurrentCell = Dvg_CierrePuntoVenta[nextColumn, currentRow];

                // Iniciar edición en la nueva celda
                if (!Dvg_CierrePuntoVenta.CurrentCell.ReadOnly)
                {
                    Dvg_CierrePuntoVenta.BeginEdit(true);
                }
            }
        }

        private void TotalCredito_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && !string.IsNullOrEmpty(tb.Text))
            {
                string cleanText = tb.Text.Replace(".", "");
                if (decimal.TryParse(cleanText, out decimal value))
                {
                    tb.Text = string.Format("{0:#,0.00}", value);
                }
            }
        }

        private void NroLoteColumn_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir Backspace, Delete, flechas, etc.
            if (char.IsControl(e.KeyChar))
                return;

            // Permitir sólo dígitos
            if (!char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void dgvCierredecaja_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvCierredecaja.CurrentCell != null && (dgvCierredecaja.CurrentCell.ColumnIndex == dgvCierredecaja.Columns["Total"].Index))
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress -= TotalCierre_KeyPress;
                    tb.KeyPress += TotalCierre_KeyPress;
                    tb.MaxLength = 10; // Opcional: límite de caracteres
                }
            }

            else
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress -= TotalCierre_KeyPress;
                }
            }


        }
        private void TotalCierre_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;

            // Permitir teclas de control (Backspace, Delete, etc.)
            if (char.IsControl(e.KeyChar))
                return;

            // Permitir dígitos
            if (char.IsDigit(e.KeyChar))
                return;

            // Permitir una sola coma como separador decimal
            if (e.KeyChar == ',' && !tb.Text.Contains(","))
                return;

            // Permitir un solo punto como separador de miles
            if (e.KeyChar == '.' && !tb.Text.Contains("."))
                return;

            // Bloquear cualquier otro carácter
            e.Handled = true;
        }


        private void lbl_Paso3_Click(object sender, EventArgs e)
        {

        }

        private void dgvCierredecaja_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lbl_Observaciones_Click(object sender, EventArgs e)
        {

        }

        private void txtBox_observaciones_pg4_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_Cancelar_pnl2_Click(object sender, EventArgs e)
        {
            Pnl2_ListadoDeVendedores.Visible = false;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void lbl_Paso2_Click(object sender, EventArgs e)
        {

        }

        public bool BuscoAsistencia(string Codigo)
        {
            try
            {
                //var Suc = new Configuration.AppSettingsReader();
                //var Asis = new CapaNegocio.Asistencia();
                //var Usu = new CapaNegocio.Usuario();
                //var Asist = new EPOS.frAsistencia();
                bool OkAsis = false;
                //var Sucur = new CapaNegocio.ConfiguraSucursal();

                //string IActivarAsisDia = ValorParametro("ActivarAsisDia", sqlCom);
                //if (IActivarAsisDia == "0")
                //{
                //    DataSet dsInsertDetalle = ManBD.EjecutaStoreProcedure("SP_INSERTATB_ASISTENCIA", glbSucursalActual, sqlCom);
                //}

                ////Usu.ObtenerUsuarioCodigo(Codigo, sqlCom);
                DateTime currentDate = DateTime.Now;
                string formattedDate = currentDate.ToString("yyyyMMdd");

                if (TB_USUARIO.Id_Rol != "000" && TB_USUARIO.Id_Rol != "013" && TB_USUARIO.Id_Rol != "017")
                {

                    //Asis.ObtenerAsistenciasCodEmpleado(Codigo, DateTime.Today.ToString("dd/MM/yyyy"), sucursal, command);
                    DataTable dtAsis = _L_CierreCaja.ObtieneAsistenciaPendienteds(formattedDate, TB_USUARIO.COD_USR);

                    if (dtAsis.Rows.Count == 0 || string.IsNullOrWhiteSpace(dtAsis.Rows[0]["CodEmpleado"].ToString()))
                    {
                        _FrmMensajes.co = 2;
                        _FrmMensajes.avisomensaje("Debe marcar asistencia para el día activo");
                        _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                        _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                        _FrmMensajes.ShowDialog();
                        return false;


                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(dtAsis.Rows[0]["HoraDeSalidaPrimerTurno"]?.ToString()))
                        {
                            OkAsis = true;
                        }
                        else if (!string.IsNullOrWhiteSpace(dtAsis.Rows[0]["HoraDeEntradaSegundoTurno"]?.ToString()) &&
                                 string.IsNullOrWhiteSpace(dtAsis.Rows[0]["HoraDeSalidaSegundoTurno"]?.ToString()))
                        {
                            OkAsis = true;
                        }
                        else if (!string.IsNullOrWhiteSpace(dtAsis.Rows[0]["HoraDeEntradaTercerTurno"]?.ToString()) &&
                                 string.IsNullOrWhiteSpace(dtAsis.Rows[0]["HoraDeSalidaTercerTurno"]?.ToString()))
                        {
                            OkAsis = true;
                        }
                        else
                        {
                            //if (IActivarAsisDia == "1")
                            //{
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje("Debe marcar asistencia para la entrada de turno");
                            _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                            _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                            _FrmMensajes.ShowDialog();
                            return false;
                            //}
                        }
                    }

                    if (OkAsis)
                    {
                        return VerificarTiempoMaxTrabajo(Codigo, sucursal);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
                //MensajeError.MuestroMensaje("Error en la función", "Variables.BuscoAsistencia", "Por favor comunicarse con el Dpto. de Sistemas y reportar el siguiente error: ", ex.Message, CapaNegocio.MensajesGenerales.TiposIconos.IconoError, glbUsuarioActual);
                //MensajeError.ShowDialog();
                //return false;
            }
        }

        public bool VerificarTiempoMaxTrabajo(string CodigoUsuario, string sucursal)
        {
            try
            {
                //var Asis = new CapaNegocio.Asistencia();
                //var Usu = new CapaNegocio.Usuario();
                string UltimaHoraMarcada;

                DateTime currentDate = _D_Inicio.DiaActivo();
                string formattedDate = currentDate.ToString("yyyyMMdd");

                //Usu.ObtenerUsuarioCodigo(sucursal, sqlCom);
                if (TB_USUARIO.Id_Rol != "013")
                {
                    //Asis.ObtenerAsistenciasCodEmpleado(CodigoUsuario, DateTime.Today.ToString("dd/MM/yyyy"), sucursal, sqlCom);
                    DataTable dtAsis = _L_CierreCaja.ObtieneAsistenciaPendienteds(formattedDate, TB_USUARIO.COD_USR);

                    if (string.IsNullOrWhiteSpace(dtAsis.Rows[0]["HoraDeSalidaPrimerTurno"]?.ToString()))
                    {
                        DateTime entradaPrimerTurno = Convert.ToDateTime(dtAsis.Rows[0]["HoraDeEntradaPrimerTurno"]?.ToString());
                        int tiempoMax = Convert.ToInt32(_D_DetalleOrden.TB_PARAMETRO("TiempMaxTraba"));
                        double horasTrabajadas = DateTime.Now.Subtract(entradaPrimerTurno).TotalHours;

                        if (horasTrabajadas < tiempoMax)
                        {
                            double avisoHoras = DateTime.Now.Subtract(entradaPrimerTurno.AddMinutes(-30)).TotalHours;
                            if (avisoHoras >= tiempoMax)
                            {
                                //var Usus = new CapaNegocio.Usuario();
                                //Usus.ObtenerUsuarioCodigo(CodigoUsuario, sqlCom);
                                int minutosRestantes = 30 - DateTime.Now.Subtract(entradaPrimerTurno.AddMinutes(-30)).Minutes;

                                _FrmMensajes.co = 2;
                                _FrmMensajes.avisomensaje(TB_USUARIO.USER_NOMBRE + " iniciará su período de descanso obligatorio en los próximos " + minutosRestantes + " minutos");
                                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                                _FrmMensajes.ShowDialog();
                                return true;

                                //MessageBox.Show(TB_USUARIO.USER_NOMBRE + "en los próximos {minutosRestantes} min\ncomienza su tiempo de descanso obligatorio",
                                //    "Tome sus medidas preventivas...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            }

                            return true;
                        }
                        else
                        {
                            _FrmMensajes.co = 2;
                            _FrmMensajes.avisomensaje("Han transcurrido más de " + tiempoMax + " horas desde su última marca de asistencia, registre su salida");
                            _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                            _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                            _FrmMensajes.ShowDialog();
                            return false;
                            //MessageBox.Show($"Hay más de {tiempoMax} horas desde su última marca en ASISTENCIA y por lo tanto debe marcar una Salida\n(Presione las teclas Ctrl + F2 para marcar su asistencia)",
                            //    "Más de 4 horas", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            //return false;
                        }
                    }
                    else
                    {
                        // Lógica de verificación para segundo y tercer turno desactivada según comentario original
                        return true;
                    }
                }
                else
                {
                    // RolEmpleado "013" se considera exento
                    return true;
                }
            }
            catch (Exception ex)
            {
                //MensajeError.MuestroMensaje("Error en la función", "Variables.VerificarTiempoMaxTrabajo",
                //    "Por favor comunicarse con el Dpto. de Sistemas y reportar el siguiente error: ",
                //    ex.Message, CapaNegocio.MensajesGenerales.TiposIconos.IconoError, CodigoUsuario);
                //MensajeError.ShowDialog();
                return false;
            }
        }

        //private void tcCierreCaja_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (tcCierreCaja.SelectedIndex == 0)
        //    {
        //        FechaInicioCierre = DateTime.Now;
        //        lblPaso.Text = "Confirmación";
        //        lbPaso.Text = "Paso 1";
        //        btnFinalizar.Text = "Finalizar";
        //        btnCancelar.Enabled = true;

        //        sucursal = _D_DetalleOrden.TB_PARAMETRO("sucursalId");
        //        diaActivo = _D_Inicio.DiaActivo();

        //        dtLogCierre.Clear();
        //        dgvLogCierre.Refresh();

        //        dtCierreCaja.Clear();
        //        dgvCierredecaja.Refresh();

        //        dtPagoMovil.Clear();
        //        Dvg_OSconPagoMovil.Refresh();

        //        dtAsistenciaPendiente.Clear();
        //        Dvg_MarcajeAsistenciaPendiente.Refresh();

        //        dtConsignacion.Clear();
        //        Dvg_ConsignacionDeOS.Refresh();

        //        dtPtoVenta.Clear();
        //        Dvg_CierrePuntoVenta.Refresh();

        //        txtBox_observaciones_pg4.Text = "";

        //        //PUNTOS DE VENTA
        //        DataTable dtPuntosCerrados = _L_CierreCaja.CierrePuntodeVenta(sucursal, "", "", diaActivo);

        //        //Si no se han cerrado lleno datos en 0f
        //        if (dtPuntosCerrados.Rows.Count == 0)
        //        {
        //            DataTable dt = _L_CierreCaja.ObtienePuntosdeVenta("", diaActivo);

        //            CrearTabla("PuntodeVenta");

        //            foreach (DataRow fila in dt.Rows)
        //            {
        //                dtPtoVenta.Rows.Add(fila["CodPunto"], fila["Descripcion"], "", "0,00", "0,00", "0,00", "0,00");
        //            }


        //            // Asignar al DataGridView
        //            Dvg_CierrePuntoVenta.DataSource = dtPtoVenta;

        //            FormatoTabla("PuntodeVenta");
        //        }
        //        else
        //        {
        //            CrearTabla("PuntodeVenta");

        //            foreach (DataRow fila in dtPuntosCerrados.Rows)
        //            {
        //                dtPtoVenta.Rows.Add(fila[0], fila[1], fila[2], fila[3], fila[4], fila[5], fila[6]);
        //            }


        //            // Asignar al DataGridView
        //            Dvg_CierrePuntoVenta.DataSource = dtPtoVenta;

        //            FormatoTabla("PuntodeVenta");
        //        }


        //        //OS CON PAGOMOVIL
        //        dtPagoMovil = _L_CierreCaja.ObtineneCambioCierre(diaActivo, sucursal);

        //        // Asignar al DataGridView
        //        Dvg_OSconPagoMovil.DataSource = dtPagoMovil;

        //        FormatoTabla("PagoMovil");

        //        DataTable dtBancos = _L_CierreCaja.ObtieneBancosPagoMovil(sucursal);

        //        //ASISTENCIA PENDIENTE
        //        //DataTable dtAsistenciaPendiente = _L_CierreCaja.VerificaAsistenciaPendiente(diaActivo.ToString("yyyyMMdd"), "PEND");

        //        //// Asignar al DataGridView
        //        //Dvg_MarcajeAsistenciaPendiente.DataSource = dtAsistenciaPendiente;

        //        //FormatoTabla("Asistencia");

        //        Dvg_MarcajeAsistenciaPendiente.EditMode = DataGridViewEditMode.EditProgrammatically;

        //        //CONSIGNACION
        //        dtConsignacion = _L_CierreCaja.ConsultaOsDia(diaActivo, sucursal);

        //        // Asignar al DataGridView
        //        Dvg_ConsignacionDeOS.DataSource = dtConsignacion;

        //        FormatoTabla("Consignacion");

        //        //CIERRE DE CAJA
        //        CrearTabla("CierredeCaja");

        //        dtCierreCaja = _L_CierreCaja.ObtienePagosCierreCaja(sucursal);


        //        // Asignar al DataGridView
        //        dgvCierredecaja.DataSource = dtCierreCaja;

        //        FormatoTabla("CierredeCaja");
        //    }
        //}
    }
}
