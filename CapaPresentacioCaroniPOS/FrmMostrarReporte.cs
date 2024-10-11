using CapaVisual_Login.Reportes;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CapaVisual_Login
{
    public partial class FrmMostrarReporte : Form
    {
        FrmRepOrdenTContact _FrmRepOrdenTContact = new FrmRepOrdenTContact();
        FrmRepOrden _FrmRepOrden = new FrmRepOrden();
        bool MostrarSubReporte = false;
        bool Contacto = false;

        public FrmMostrarReporte()
        {
            InitializeComponent();
        }

        private void FrmMostrarReporte_Load(object sender, EventArgs e)
        {
            // Configurar la conexión
            string conexion = ConfigurationManager.ConnectionStrings["Epos"].ConnectionString;
            this.sP_CPOS_ReporAbonoTableAdapter.Connection.ConnectionString = conexion;

            // Llenar el dataset principal
            this.sP_CPOS_ReporAbonoTableAdapter.Fill(this.dsAbono.SP_CPOS_ReporAbono, Orden_txt.Text);

            // Configurar el ReportViewer
            this.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DsRpAbono", SP_CPOS_ReporAbonoBindingSource));
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RptAbono.rdlc";
            this.reportViewer1.ProcessingMode = ProcessingMode.Local;

            // Crear objetos ReportParameter
            ReportParameter param1 = new ReportParameter("MostrarSubreporte", MostrarSubReporte.ToString()); // Cambia a "false" para ocultar
            ReportParameter param2 = new ReportParameter("Contacto", Contacto.ToString()); // Cambia a "false" para ocultar

            reportViewer1.LocalReport.SetParameters(new ReportParameter[] { param1, param2 });

            // Manejar el evento SubreportProcessing
            this.reportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(OnSubreportProcessing);

            // Refrescar el ReportViewer
            this.reportViewer1.RefreshReport();
        }

        public void setParametros(string nroDoc)
        {
            Orden_txt.Text = nroDoc;
            reportViewer1.RefreshReport();
        }

        public void ConfigRep(bool MostrarSubRlc = false, bool MostrarContacto = false)
        {
            ReportDataSource fuente = new ReportDataSource();
            fuente.Name = "CapaVisual_Login.Reportes.dsRpAbono"; // Nombre identico al que le di al dataset del report en tiempo de diseño
            fuente.Value = reportViewer1.LocalReport.DataSources;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(fuente);
            reportViewer1.LocalReport.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RptAbono.rdlc";
            reportViewer1.ProcessingMode = ProcessingMode.Local;

            MostrarSubReporte = MostrarSubRlc;
            Contacto = MostrarContacto;
        }

        private void OnSubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            // Obtener el ID de la orden
            string ordenId = Orden_txt.Text;

            // Llenar el dataset del subreporte
            DataTable subreportData = GetSubreportData(ordenId);

            if (MostrarSubReporte)
            {
                if (e.ReportPath == "RepOrden" && !Contacto)
                {
                    // Pasar los datos al subreporte 1
                    e.DataSources.Add(new ReportDataSource("DsRepOrden", subreportData));
                }
                else if (e.ReportPath == "RepOrdenTContacto" && Contacto)
                {
                    // Pasar los datos al subreporte 2
                    e.DataSources.Add(new ReportDataSource("DsRepOrden", subreportData));
                }
            }

            //// Pasar los parámetros al subreporte
            //e.Parameters["MostrarSubreporte"].Values.Add(MostrarSubReporte.ToString());
            //e.Parameters["Contacto"].Values.Add(Contacto.ToString());
        }

        private DataTable GetSubreportData(string ordenId)
        {
            // Implementa la lógica para obtener los datos del subreporte
            DataTable dt = new DataTable();
            // Lógica para llenar el DataTable
            string conexion = ConfigurationManager.ConnectionStrings["Epos"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("SP_CPOS_REP_ORDEN", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Orden", ordenId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public void imprimir(String concat = null)
        {
            

            // Crear una instancia de LocalReport
            LocalReport rdlc = new LocalReport();
            rdlc.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RptAbono.rdlc";

            // Configurar la conexión y llenar el dataset principal
            string conexion = ConfigurationManager.ConnectionStrings["Epos"].ConnectionString;
            this.sP_CPOS_ReporAbonoTableAdapter.Connection.ConnectionString = conexion;
            this.sP_CPOS_ReporAbonoTableAdapter.Fill(this.dsAbono.SP_CPOS_ReporAbono, Orden_txt.Text);

            // Agregar el datasource principal al reporte
            rdlc.DataSources.Add(new ReportDataSource("DsRpAbono", SP_CPOS_ReporAbonoBindingSource));

            // Crear objetos ReportParameter
            ReportParameter param1 = new ReportParameter("MostrarSubreporte", MostrarSubReporte.ToString());
            ReportParameter param2 = new ReportParameter("Contacto", Contacto.ToString());

            // Establecer los parámetros en el reporte
            rdlc.SetParameters(new ReportParameter[] { param1, param2 });

            // Manejar el evento SubreportProcessing
            rdlc.SubreportProcessing += new SubreportProcessingEventHandler(OnSubreportProcessing);

            // Crear una instancia de Impresor y pasar el reporte para imprimir
            Impresor imp = new Impresor();
            imp.Imprime(rdlc);

        }
    }
}