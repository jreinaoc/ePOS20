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
    public partial class FrmPrueba : Form
    {
        public FrmPrueba()
        {
            InitializeComponent();
        }

        private string conexion = ConfigurationManager.ConnectionStrings["Epos"].ConnectionString;

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            { 
            if (checkedListBox1.Text== "PagosTranferencia")
            {
                var parametros = new Dictionary<string, string>
                {
                      { "FechaDesde", dateTimePicker1.Text },
                      { "Compania", "Prueba" },
                      { "RifCompania", "preuba22" },
                      { "Sucursal", "prueba33" }
                };

                    // Supón que tienes estos datos:
                    string nombreReporte = "CapaVisual_Login.Reportes.RptPagosTranferencia.rdlc";
                    string nombreDataSource = "CapaVisual_Login.Reportes.DsRepPagosTranferencia";     
                    bool imprimir = false; // o true si quieres imprimir automáticamente
                DataTable datosReporte = ObtenerDatosParaReporte(conexion,dateTimePicker1.Text, "095"); // Tu método para obtener los datos

                // Instanciar y mostrar el formulario
                FrmMostrarRep frm = new FrmMostrarRep(nombreReporte, nombreDataSource, imprimir, datosReporte, parametros);
                frm.ShowDialog();

            }
            else if (checkedListBox1.Text == "Vuelto")
            {

            }
            }
            catch (Exception ex)
            {

                string Respuesta = string.Format("Error: {0}", ex.Message);
            }
        }

        public DataTable ObtenerDatosParaReporte(string conexion, string param1, string param2)
        {
            // Crea una instancia del DataSet y TableAdapter
            Reportes.DsRepPagosTranferencia ds = new Reportes.DsRepPagosTranferencia();
            Reportes.DsRepPagosTranferenciaTableAdapters.Cpos_PagoTransferenciaTableAdapter adapter = new Reportes.DsRepPagosTranferenciaTableAdapters.Cpos_PagoTransferenciaTableAdapter();

            // Cierra cualquier conexión abierta
            adapter.Connection.Close();

            // Asigna la cadena de conexión
            adapter.Connection.ConnectionString = conexion;

            // Llena el DataTable usando el TableAdapter y los parámetros necesarios
            adapter.Fill(ds.Cpos_PagoTransferencia, param1, param2);

            // Retorna el DataTable con los datos
            return ds.Cpos_PagoTransferencia;
        }
    }
}
