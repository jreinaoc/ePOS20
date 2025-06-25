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

namespace CapaVisual_Login
{
    public partial class FrmCierredeCaja : Form
    {
        private L_CierreCaja _L_CierreCaja = new L_CierreCaja();
        FrmMensajes _FrmMensajes = new FrmMensajes();
        private D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        D_Inicio _D_Inicio = new D_Inicio();

        DateTime diaActivo;

        public FrmCierredeCaja()
        {
            InitializeComponent();
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            string sucursal = _D_DetalleOrden.TB_PARAMETRO("sucursalId");

            if (_L_CierreCaja.CierreFueradeHorario(sucursal,DateTime.Now, DateTime.Now))
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Debe registrar el cierre de la sucursal");
                _FrmMensajes.StartPosition = FormStartPosition.Manual; // Permite posicionarlo manualmente
                _FrmMensajes.Location = new System.Drawing.Point(600, 300); // Coordenadas específ
                _FrmMensajes.ShowDialog();
                return;
            }


            if (!_L_CierreCaja.CierrePuntodeVenta(sucursal, "","", diaActivo))
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
            checkBox2.BackColor = col1;
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

        }

        private void FrmCierredeCaja_Load(object sender, EventArgs e)
        {
            try
            {

                diaActivo = _D_Inicio.DiaActivo();

                DataTable dt = _L_CierreCaja.ObtienePuntosdeVenta("");

                // Crear la estructura de la tabla una sola vez
                DataTable dtPtoVenta = new DataTable();
                dtPtoVenta.Columns.Add("CodPunto", typeof(string));
                dtPtoVenta.Columns.Add("Banco", typeof(string));
                dtPtoVenta.Columns.Add("Nro. Lote", typeof(string)); // Vacía
                dtPtoVenta.Columns.Add("Total T. Crédito", typeof(string)); // Vacía
                dtPtoVenta.Columns.Add("Total T. Amex", typeof(string)); // Vacía
                dtPtoVenta.Columns.Add("Total T. Débito", typeof(string)); // Vacía
                dtPtoVenta.Columns.Add("Total T. Otros", typeof(string)); // Vacía

                // Cargar los datos de filas
                foreach (DataRow fila in dt.Rows)
                {
                    dtPtoVenta.Rows.Add(fila["CodPunto"],fila["Descripcion"], "", "0,00", "0,00", "0,00", "0,00");
                }

                // Asignar al DataGridView
                Dvg_CierrePuntoVenta.DataSource = dtPtoVenta;

                // Asignar ancho personalizado a cada columna
                Dvg_CierrePuntoVenta.Columns["CodPunto"].Width = 0;
                Dvg_CierrePuntoVenta.Columns["CodPunto"].Visible = false;
                Dvg_CierrePuntoVenta.Columns["Banco"].Width = 100;
                Dvg_CierrePuntoVenta.Columns["Nro. Lote"].Width = 150;
                Dvg_CierrePuntoVenta.Columns["Total T. Crédito"].Width = 120;
                Dvg_CierrePuntoVenta.Columns["Total T. Amex"].Width = 120;
                Dvg_CierrePuntoVenta.Columns["Total T. Débito"].Width = 120;
                Dvg_CierrePuntoVenta.Columns["Total T. Otros"].Width = 100;


            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Ocurrió un error al obtener la información del cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
