using CapaDatos.Anulacion;
using CapaDatos.Inicio_Datos;
using CapaDatos.TasaDia_Datos;
using CapaEntidades;
using CapaLogica.Impresora_Fiscal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVisual_Login
{
    public partial class FrmReimpresionDocumentos : Form
    {
        public FrmReimpresionDocumentos()
        {
            InitializeComponent();
        }
        private D_TasaSecuencia _D_TasaSecuencia = new D_TasaSecuencia();
        private D_Inicio _D_Inicio = new D_Inicio();
        public readonly StringBuilder stringBuilder = new StringBuilder();
        private VmaxComVe.VmaxComClass objVmax = new VmaxComVe.VmaxComClass();
        private int glbPuertoCOM = Convert.ToInt16(ConfigurationManager.AppSettings.Get("PuertoCOMimpresora"));
        private Impresora_Fiscal _Impresora_Fiscal = new Impresora_Fiscal();
        private D_Anulacion _D_Anulacion = new D_Anulacion();
        private void FrmReimpresionDocumentos_Load(object sender, EventArgs e)
        {
            // Verificar si el ComboBox está vacío
            if (CbTipoDocumento.Items.Count == 0)
            {
                // Agregar los elementos al ComboBox
                CbTipoDocumento.Items.Add("Factura");
                CbTipoDocumento.Items.Add("Nota de credito");
                CbTipoDocumento.Items.Add("Reporte Z");

                // Opcional: Seleccionar el primer elemento por defecto
                CbTipoDocumento.SelectedIndex = 0;
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

        private void button6_Click(object sender, EventArgs e)
        {       try
                {

                if (_Impresora_Fiscal.VerficarConexionImpresoraFiscalSinCerrar() == false)
                {
                    string mensaje = _Impresora_Fiscal.stringBuilder.ToString();
                    mostrarError(mensaje);
                    return;
                }

                uint resp = 0;
                resp = objVmax.AbrirPuerto(Convert.ToString(glbPuertoCOM));
                resp = objVmax.ObtenerReporteInformativo();
                string SerialImpresora = objVmax.RetornoMI.sSerial;
                //SerialImpresora = "TIU2206171";
                resp = objVmax.CerrarPuerto();
                string TipoDocumento = "";

                    // Asume que el ComboBox tiene los textos: "Factura", "Nota de credito", "Reporte Z"
                    switch (CbTipoDocumento.Text)
                    {
                        case "Factura":
                            TipoDocumento = "0";
                            break;
                        case "Nota de credito":
                            TipoDocumento = "1";
                            break;
                        case "Reporte Z":
                            TipoDocumento = "2";
                            break;
                        default:
                            MessageBox.Show("Seleccione un tipo de documento.");
                            return;
                    }

                    BuscarDocumento(
                        TipoDocumento,
                        true,
                        Convert.ToDateTime(DtReinDesde.Text),
                        SerialImpresora,Convert.ToDateTime(DtReinHasta.Text));
                }
                catch (Exception ex)
                {
                  mostrarError($"Error al BuscarDocuemtos: {ex.Message}"); 
               }
            
        }

        public bool BuscarDocumento(string TipoDocumen, bool TipoUsuario, DateTime FechaDesde, string SerialImpresora, DateTime? FechaHasta = null)
        {
            // Desvincular el DataGridView de su fuente de datos
            gexMensajesDANA.DataSource = null;
            gexMensajesDANA.DataMember = null;

            // Eliminar todas las filas
            gexMensajesDANA.Rows.Clear();

            // Eliminar todas las columnas
            gexMensajesDANA.Columns.Clear();

            // Verificar y eliminar la columna "Eliminar" si existe
            var dataGridViewColumn2 = gexMensajesDANA.Columns["E"];
            if (dataGridViewColumn2 != null)
            {
                gexMensajesDANA.Columns.Remove(dataGridViewColumn2);
            }



            DateTime fechaHastaFinal = FechaHasta ?? DateTime.Today;

            DataSet ds = _D_TasaSecuencia.Reimprimir_Documentos(TipoDocumen, TipoUsuario, FechaDesde, fechaHastaFinal, SerialImpresora);

            // Verificar si hay datos
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                gexMensajesDANA.DataSource = new DataTable(); // Asignar tabla vacía para limpiar grid
                return false;
            }

            // Clonar estructura original
            DataTable dtResultado = ds.Tables[0].Clone();

            // Configurar el grid antes de asignar datos
            gexMensajesDANA.DataSource = dtResultado;

            // Agregar columna de selección directamente al grid
            if (!gexMensajesDANA.Columns.Contains("E"))
            {
                DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn();
                checkColumn.Name = "E";
                checkColumn.HeaderText = "";
                checkColumn.Width = 80;
                checkColumn.FalseValue = false;
                checkColumn.TrueValue = true;
                gexMensajesDANA.Columns.Insert(0, checkColumn); // Insertar como primera columna
            }

            // Llenar los datos
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                DataRow newRow = dtResultado.NewRow();

                // Copiar los datos originales
                foreach (DataColumn col in dtResultado.Columns)
                {
                    if (col.ColumnName == "Fecha")
                    {
                        newRow[col] = Convert.ToDateTime(dr["Fecha"]).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        newRow[col] = dr[col.ColumnName];
                    }
                }

                dtResultado.Rows.Add(newRow);
            }

            // Asignar los datos con la estructura completa
            gexMensajesDANA.DataSource = dtResultado;

            // Aplicar formato
            FormatoTabla();

            return true;

        }

        private void FormatoTabla()
        {
            var cols = gexMensajesDANA.Columns;

            if (cols.Contains("E")) cols["E"].Width = 20;
            if (cols.Contains("TipoDocumen"))
            {
                cols["TipoDocumen"].Visible = false;
                cols["TipoDocumen"].Width = 80;
                cols["TipoDocumen"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                cols["TipoDocumen"].ReadOnly = true;
            }
            if (cols.Contains("NumeroDocumento"))
            {
                cols["NumeroDocumento"].Visible = true;
                cols["NumeroDocumento"].Width = 120;
                cols["NumeroDocumento"].HeaderText = "Nro. Documento";
                cols["NumeroDocumento"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (cols.Contains("Fecha"))
            {
                cols["Fecha"].Visible = true;
                cols["Fecha"].Width = 120;
                cols["Fecha"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                cols["Fecha"].ReadOnly = true;
            }

            if (cols.Contains("SerialImpresora"))
            {
                cols["SerialImpresora"].HeaderText = "Serial Impresora";
                cols["SerialImpresora"].Visible = true;
                cols["SerialImpresora"].Width = 155;
                cols["SerialImpresora"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                cols["SerialImpresora"].ReadOnly = true;
            }

            gexMensajesDANA.BackgroundColor = Color.Honeydew;
            gexMensajesDANA.Font = new Font("Arial", 9, FontStyle.Regular);
            gexMensajesDANA.EnableHeadersVisualStyles = false;
            gexMensajesDANA.DefaultCellStyle.SelectionBackColor = Color.Yellow;
        }

        // Extensión para verificar si existe la columna
        //public static class DataGridViewExtensions
        //{
        //    public static bool Contains(this DataGridViewColumnCollection columns, string columnName)
        //    {
        //        return columns.Contains(columnName);
        //    }
        //}

        public void CrearTabla(DataTable dt4)
        {
            dt4.Columns.Add("E", typeof(bool));
            dt4.Columns.Add("TipoDocumen", typeof(string));
            dt4.Columns.Add("NumeroDocumento", typeof(string));
            dt4.Columns.Add("Fecha", typeof(string));
        }

        public bool ImprimirDocumento(string TipoDocumen, bool TipoUsuario, string SerialImpresora, DateTime FechaDesde, DateTime? FechaHasta  = null)
        {
            try
            {
                DateTime fechaHastaFinal = FechaHasta ?? DateTime.Today;
                DataTable dt4 = null;
                CrearTabla(dt4);
                DataSet ds = _D_TasaSecuencia.Reimprimir_Documentos(TipoDocumen, TipoUsuario, FechaDesde, fechaHastaFinal, SerialImpresora);
                gexMensajesDANA.DataSource = dt4;
                FormatoTabla();
                dt4 = ds.Tables[0]; // Asigna la primera tabla del DataSet a dt4

                uint Pruebaaa= 5;
                // Verificar si el DataTable tiene filas antes de recorrer
              if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
              {
                foreach (DataRow fila in dt4.Rows)
                {
                    string TipoDoc = fila["TipoDocumen"].ToString();
                    string NumeroDoc = fila["NumeroDocumento"].ToString();
                    Pruebaaa = objVmax.ReimprimirDocumento(TipoDoc, NumeroDoc);
                }
              }
                return Pruebaaa != 0 ? false : true ;
            }
            catch (Exception ex)
            {
                mostrarError($"Error al Imprimir Documento: {ex.Message}");
                return false;
            }
        }

        public bool ImprimirDocumentoUsuarioSistema()
        {
            try
            {
                int x;
                uint Pruebaaa= 5;
                string TipoDoc;
                string NumeroDoc;

                foreach (DataGridViewRow row in gexMensajesDANA.Rows)
                {
                    // Omitir la fila nueva si AllowUserToAddRows = true
                    if (row.IsNewRow) continue;

                    var celdaE = row.Cells["E"].Value;
                    if (celdaE != null && celdaE is bool && (bool)celdaE)
                    {
                         TipoDoc = row.Cells["TipoDocumen"].Value?.ToString();
                        NumeroDoc = row.Cells["NumeroDocumento"].Value?.ToString();
                        //NumeroDoc = "234";
                        //TipoDoc = "2" ;
                         Pruebaaa = objVmax.AbrirPuerto(Convert.ToString(glbPuertoCOM));
                        //Pruebaaa = objVmax.ObtenerReporteInformativo();
                        //ushort ultimoZ = objVmax.ObtenerReporteMf("");
                        //Pruebaaa = objVmax.RetornoMF2.uiUltNumZ;
                        //Pruebaaa = objVmax.ReporteMfNumZ("01072025","21072025");
                        Pruebaaa = objVmax.ReimprimirDocumento(TipoDoc, NumeroDoc);
                        objVmax.CerrarPuerto();
                        //objVmax.Cancelar();
                        //objVmax.Cerrar();
                        //objVmax.CerrarPuerto();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                mostrarError($"Error al Imprimir Documento Sistema: {ex.Message}");
                return false;
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            ImprimirDocumentoUsuarioSistema();
        }


        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                button6.Enabled = false;
                btn_pg5_reporteX.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                ProcesarReporteZ();
            }
            catch (Exception ex)
            {
                mostrarError($"Error en la función Reporte Z: {ex.Message}");
            }

            finally
            {
                button6.Enabled = true;
                btn_pg5_reporteX.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }

        private void ProcesarReporteZ()
        {
            uint resp;
            resp = objVmax.AbrirPuerto(Convert.ToString(glbPuertoCOM));

            string fecha = DateTime.Now.ToString("MM/dd/yyyy");

            resp = objVmax.ReporteZ();

            // Esperar 18 segundos para el cierre fiscal
            Thread.Sleep(18000);
 

            // 1. Obtener el reporte fiscal actual
            ushort resultado = objVmax.ObtenerReporteMf("");

            // 2. Obtener datos del reporte actual
            string numZ = objVmax.RetornoMF.uiUltNumZ.ToString();

            if (resp != 0 ? false : true && !string.IsNullOrEmpty(numZ))
            {
            }
            else
            {
                objVmax.CerrarPuerto();
                mostrarError("Hubo problemas leyendo los datos del último reporte Z desde la impresora fiscal.");
                return;
            }

            string serialImpresora = objVmax.RetornoMF.sSerial;
            string cantidadFacturas = objVmax.RetornoMF.uiTotalFacturasDiarias.ToString();
            string cantidadNC = objVmax.RetornoMF.uiTotalNCDiarias.ToString();
            string ventasExentas = objVmax.RetornoMF.uiTotVenta_E.ToString();
            string ventasGravadas = objVmax.RetornoMF.uiTotVenta_G.ToString();
            string alicuota = objVmax.RetornoMF.uiAlicuota_G.ToString();
            string notaExento = objVmax.RetornoMF.uiTotDev_E.ToString();
            string notaGravable = objVmax.RetornoMF.uiTotDev_G.ToString();
            string fechaHoraReporte = objVmax.RetornoMF.sFechaHoraUltNumZ;

            // Datos adicionales para impuestos A y R
            string ventasGravA = objVmax.RetornoMF.uiTotVenta_A.ToString();
            string notaGravA = objVmax.RetornoMF.uiTotDev_A.ToString();
            string alicuotaA = objVmax.RetornoMF.uiAlicuota_A.ToString();
            string ventasGravR = objVmax.RetornoMF.uiTotVenta_R.ToString();
            string notaGravR = objVmax.RetornoMF.uiTotDev_R.ToString();
            string alicuotaR = objVmax.RetornoMF.uiAlicuota_R.ToString();

            // 3. Obtener el reporte anterior para la fecha
            ushort numZAnterior = (ushort)(objVmax.RetornoMF.uiUltNumZ - 1);
            objVmax.ObtenerReporteMf(numZAnterior.ToString());
            string fechaHoraReporteAnterior = objVmax.RetornoMF.sFechaHoraUltNumZ;
            // Ultimo numero de Factura
            resp = objVmax.ObtenerContadores();
            string ultimaFactura = objVmax.RetornoContadores.uiUltFacturaAbierta.ToString();
            objVmax.CerrarPuerto();

            // 4. Obtener información de usuario y sucursal (debes tener estas variables)
            string codSucursal = _D_Inicio.Sucursal();
            string usercrea = TB_USUARIO.COD_USR;
            string fechaMod = "";
            string usermod = "";

            switch (ultimaFactura.Length)
            {
                case 7:
                    {
                        ultimaFactura = ultimaFactura;
                        break;
                    }

                case 6:
                    {
                        ultimaFactura = "0" + ultimaFactura;
                        break;
                    }

                case 5:
                    {
                        ultimaFactura = "00" + ultimaFactura;
                        break;
                    }

                case 4:
                    {
                        ultimaFactura = "000" + ultimaFactura;
                        break;
                    }

                case 3:
                    {
                        ultimaFactura = "0000" + ultimaFactura;
                        break;
                    }

                case 2:
                    {
                        ultimaFactura = "00000" + ultimaFactura;
                        break;
                    }

                case 1:
                    {
                        ultimaFactura = "000000" + ultimaFactura;
                        break;
                    }
            }

            switch (numZ.Length)
            {
                case 4:
                    {
                        numZ = numZ;
                        break;
                    }

                case 3:
                    {
                        numZ = "0" + numZ;
                        break;
                    }

                case 2:
                    {
                        numZ = "00" + numZ;
                        break;
                    }

                case 1:
                    {
                        numZ = "000" + numZ;
                        break;
                    }
            }


            // 5. Llamar al método GuardarReporteZ con todos los parámetros
            DataSet dsResultado = _D_TasaSecuencia.GuardarReporteZ(
                FechaHoraReporteAnterior: fechaHoraReporteAnterior,
                FechaHoraReporte: fechaHoraReporte,
                NumReporteZ: numZ,
                SerialImpresora: serialImpresora,
                UltimaFactura: ultimaFactura,
                CantidadFacturas: cantidadFacturas,
                CantidadNC: cantidadNC,
                VentasExentas: ventasExentas,
                VentasGravadas: ventasGravadas,
                Alicuota: alicuota,
                NotaExento: notaExento,
                NotaGravable: notaGravable,
                codSucursal: codSucursal,
                usercrea: usercrea,
                FechaMod: fechaMod,
                usermod: usermod,
                VentasGravA: ventasGravA,
                NotaGravA: notaGravA,
                AlicuotaA: alicuotaA,
                VentasGravR: ventasGravR,
                NotaGravR: notaGravR,
                AlicuotaR: alicuotaR);

                  objVmax.CerrarPuerto();

                _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "054", TB_USUARIO.COD_EMPLEADO, "Reporte Z: " + numZ);
      
        }

        //private bool CapturaReportesZFaltantes(ref bool bErroresAlGuardar = false)
        //{
        //    try
        //    {
        //        uint Resp;
        //        int x, IntentosDeLeer = 0;
        //        string UltimoRepZ;
        //        Resp=objVmax.AbrirPuerto(Convert.ToString(glbPuertoCOM));
        //        Resp = objVmax.ObtenerReporteInformativo();
        //        string SerialImpresora = objVmax.RetornoMI.sSerial;
        //        UltimoRepZ = objVmax.RetornoMF.uiUltNumZ.ToString();
        //        Resp = objVmax.CerrarPuerto();

        //        int nUltimoRep = Convert.ToInt32(UltimoRepZ);
        //        DataTable dtRepzFalt = _D_TasaSecuencia.ExecuteGetReportesZProcedure(_D_Inicio.Sucursal(), SerialImpresora, nUltimoRep);

        //        if (dtRepzFalt != null)
        //        {
        //            barra.Minimum = 0;
        //            barra.Maximum = dtRepzFalt.Rows.Count;
        //            barra.Visible = true;

        //            while (Resp != 0 ? false : true == false)
        //            {
        //                Resp = objVmax.LeeZ("");
        //                IntentosDeLeer++;

        //                if (IntentosDeLeer == 100)
        //                {
        //                    barra.Visible = false;
        //                    return false;
        //                }
        //            }

        //            foreach (DataRow Fila in dtRepzFalt.Rows)
        //            {
        //                string NumRepZFalt = Convert.ToInt32(Fila["Numero"].ToString()).ToString();

        //                Resp = objVmax.LeeZ(NumRepZFalt);
        //                if (Resp && objVmax.rNumZ != null)
        //                {
        //                    string NumZ = objVmax.rNumZ;
        //                    string BaseEx = objVmax.rBaseE;
        //                    string BaseGr = objVmax.rBaseG;
        //                    string BaseGrA = objVmax.rBaseA;
        //                    string BaseGrR = objVmax.rBaseR;
        //                    string Alicuota = objVmax.rTasaG;
        //                    string AlicuotaA = objVmax.rTasaA;
        //                    string AlicuotaR = objVmax.rTasaR;
        //                    string UltimaFact = objVmax.rUltimaFacturaZ;
        //                    string SerialZ = objVmax.rSerialZ;
        //                    string totalFact = objVmax.rTotalFacturas;
        //                    string FechaHoraRep = objVmax.rFechaHoraZ;
        //                    string totalNC = objVmax.rTotalNotasCredito;
        //                    string NotaExento = objVmax.rDevE;
        //                    string NotaGravable = objVmax.rDevG;
        //                    string NotaGravA = objVmax.rDevA;
        //                    string NotaGravR = objVmax.rDevR;

        //                    string FechaRepAnterior;
        //                    objVmax.LeeZ(Convert.ToInt32(NumZ) - 1);
        //                    FechaRepAnterior = objVmax.rFechaHoraZ;

        //                    if (!_D_TasaSecuencia.GuadarReportesZ(FechaRepAnterior, FechaHoraRep, NumZ, SerialZ, UltimaFact,
        //                        totalFact, totalNC, BaseEx, BaseGr, Alicuota, NotaExento, NotaGravable,
        //                         _D_Inicio.Sucursal(), TB_USUARIO.COD_EMPLEADO, "1", "", BaseGrA, NotaGravA, AlicuotaA,
        //                        BaseGrR, NotaGravR, AlicuotaR))
        //                    {
        //                        bErroresAlGuardar = true;
        //                    }
        //                    barra.Value = barra.Value + 1;
        //                }
        //            }
        //        }

        //        objVmax.CerrarPuerto();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        mostrarError($"Error en la función Reporte Z: {ex.Message}");
        //        return false;
        //    }
        //    finally
        //    {
        //        barra.Visible = false;
        //    }
        //}

        private void btn_pg5_reporteX_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                button6.Enabled = false;
                btn_pg5_reporteX.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                uint Resp;
                Resp = objVmax.AbrirPuerto(Convert.ToString(glbPuertoCOM));
                Resp = objVmax.ReporteX();
                Resp = objVmax.CerrarPuerto();
            }
            catch (Exception ex)
            {
                mostrarError($"Error en la función Reporte X: {ex.Message}");
            }

            finally
            {
                button6.Enabled = true;
                btn_pg5_reporteX.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
 }

