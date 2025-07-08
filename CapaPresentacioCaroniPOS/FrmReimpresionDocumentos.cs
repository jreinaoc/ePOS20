using CapaDatos.Inicio_Datos;
using CapaDatos.TasaDia_Datos;
using CapaLogica.Impresora_Fiscal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
                resp = objVmax.CerrarPuerto();
                SerialImpresora = "TIX2490051";
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

                DateTime fechaHastaFinal = FechaHasta ?? DateTime.Today;
                DataTable dt4= null;
                //CrearTabla(dt4);
                DataSet ds =  _D_TasaSecuencia.Reimprimir_Documentos(TipoDocumen,TipoUsuario, FechaDesde, fechaHastaFinal, SerialImpresora);
            dt4 = ds.Tables[0];
            gexMensajesDANA.DataSource = dt4;
                FormatoTabla();
                DataRow dr2;

            // Verificar si el DataTable tiene filas antes de recorrer
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr2 = dt4.NewRow();
                    dr2["E"] = false;
                    dr2["TipoDocumen"] = dr["TipoDocumen"];
                    dr2["NumeroDocumento"] = dr["NumeroDocumento"];
                    dr2["Fecha"] = Convert.ToDateTime(dr["Fecha"]).ToString("dd/MM/yyyy");
                    dt4.Rows.Add(dr2);
                }
            }

            gexMensajesDANA.DataSource = dt4;
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

                        Pruebaaa = objVmax.AbrirPuerto(Convert.ToString(glbPuertoCOM));
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

        private void lbl_Utiliarios_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            ImprimirDocumentoUsuarioSistema();
        }
    }
}
