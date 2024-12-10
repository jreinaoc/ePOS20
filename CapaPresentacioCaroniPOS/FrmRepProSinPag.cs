using CapaVisual_Login.Reportes;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVisual_Login
{
    public partial class FrmRepProSinPag : Form
    {
        FrmRepOrdenTContact _FrmRepOrdenTContact = new FrmRepOrdenTContact();
        FrmRepOrden _FrmRepOrden = new FrmRepOrden();
        bool MostrarSubReporte = false;
        bool Contacto = false;
        string Numero_Orden = "";

        public FrmRepProSinPag()
        {
            InitializeComponent();
        }

        private void FrmMostrarReporte_Load(object sender, EventArgs e)
        {

            // Configurar la conexión
            string conexion = ConfigurationManager.ConnectionStrings["Epos"].ConnectionString;
            this.sP_CPOS_ReporProSinPagoTableAdapter.Connection.ConnectionString = conexion;

            // Llenar el dataset principal
            this.sP_CPOS_ReporProSinPagoTableAdapter.Fill(this.dsRepProSinPago.SP_CPOS_ReporProSinPago, TxtOrden.Text);

            // Configurar el ReportViewer
            this.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DtsProSinPag", sPCPOSReporProSinPagoBindingSource));

            this.reportViewer1.LocalReport.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RptProSinPag.rdlc";
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

        private void FrmMostrarReporte_Resize(object sender, EventArgs e)
        {
            //this.reportViewer1.Dock = DockStyle.Fill;
            this.reportViewer1.Size = new Size(this.ClientSize.Width, this.ClientSize.Height);
        }

        public void setParametros(string nroDoc)
        {
            TxtOrden.Text = nroDoc;
            Numero_Orden = nroDoc;
            reportViewer1.RefreshReport();
        }

        public void ConfigRep(bool MostrarSubRlc = false, bool MostrarContacto = false)
        {
            ReportDataSource fuente = new ReportDataSource();
            fuente.Name = "CapaVisual_Login.Reportes.RptProSinPag"; // Nombre identico al que le di al dataset del report en tiempo de diseño
            fuente.Value = reportViewer1.LocalReport.DataSources;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(fuente);
            reportViewer1.LocalReport.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RptProSinPag.rdlc";
            reportViewer1.ProcessingMode = ProcessingMode.Local;

            MostrarSubReporte = MostrarSubRlc;
            Contacto = MostrarContacto;
        }

        private void OnSubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {


            // Llenar el dataset del subreporte
            DataTable subreportData = GetSubreportData(Numero_Orden);

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
            rdlc.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RptProSinPag.rdlc";

            // Configurar la conexión y llenar el dataset principal
            string conexion = ConfigurationManager.ConnectionStrings["Epos"].ConnectionString;

            this.sP_CPOS_ReporProSinPagoTableAdapter.Connection.ConnectionString = conexion;

            // Llenar el dataset principal
            this.sP_CPOS_ReporProSinPagoTableAdapter.Fill(this.dsRepProSinPago.SP_CPOS_ReporProSinPago, TxtOrden.Text);

            // Agregar el datasource principal al reporte
            rdlc.DataSources.Add(new ReportDataSource("DtsProSinPag", sPCPOSReporProSinPagoBindingSource));

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
