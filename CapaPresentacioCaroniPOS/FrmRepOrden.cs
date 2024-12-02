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
    public partial class FrmRepOrden : Form
    {
        bool Convencional = false;
        bool Contacto = false;

        public FrmRepOrden()
        {
            InitializeComponent();
        }

        private void FrmRepOrden_Load(object sender, EventArgs e)
        {
            //// TODO: esta línea de código carga datos en la tabla 'dsAbono.SP_CPOS_ReporAbono' Puede moverla o quitarla según sea necesario.
            //this.sP_CPOS_REP_ORDENTableAdapter.Connection.Close();
            //string conexion = ConfigurationManager.ConnectionStrings["Epos"].ConnectionString;
            //this.sP_CPOS_REP_ORDENTableAdapter.Connection.ConnectionString = conexion;
            //this.sP_CPOS_REP_ORDENTableAdapter.Fill(this.dsRepOrden.SP_CPOS_REP_ORDEN, TxtOrden.Text);
            //this.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DsRepOrden", sPCPOSREPORDENBindingSource));
            //this.reportViewer1.RefreshReport();


            //Imprimir el reporte de contacto si MostrarContacto es true
            if (Contacto && !Convencional)
            {
                LocalReport rdlcContacto = new LocalReport();
                rdlcContacto.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RepOrdenTContacto.rdlc";

                // Llenar el dataset del reporte de contacto
                DataTable dtContacto = GetSubreportData(TxtOrden.Text);
                rdlcContacto.DataSources.Add(new ReportDataSource("DsRepOrden", dtContacto));

                // Mostrar el reporte de orden en el ReportViewer
                MostrarReporteEnReportViewer(rdlcContacto);
            }

            // Imprimir el reporte de orden si MostrarSubRlc es true
            if (Convencional && !Contacto)
            {
                LocalReport rdlcOrden = new LocalReport();
                rdlcOrden.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RepOrden.rdlc";

                // Llenar el dataset del reporte de orden
                DataTable dtOrden = GetSubreportData(TxtOrden.Text);
                rdlcOrden.DataSources.Add(new ReportDataSource("DsRepOrden", dtOrden));

                // Mostrar el reporte de orden en el ReportViewer
                MostrarReporteEnReportViewer(rdlcOrden);

            }


        }

        private void MostrarReporteEnReportViewer(LocalReport report)
        {
            // Crear una instancia de ReportViewer
            ReportViewer reportViewer = new ReportViewer();

            // Configurar el ReportViewer con el LocalReport
            reportViewer.ProcessingMode = ProcessingMode.Local;
           
            if (Convencional && !Contacto)
            {
                reportViewer.LocalReport.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RepOrden.rdlc";
            }

            if (Contacto && !Convencional)
            {
                reportViewer.LocalReport.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RepOrdenTContacto.rdlc";
            }


            reportViewer.LocalReport.DataSources.Clear();

            foreach (var dataSource in report.DataSources)
            {
                reportViewer.LocalReport.DataSources.Add(dataSource);
            }

            // Refrescar el ReportViewer para mostrar el reporte
            reportViewer.RefreshReport();

            // Mostrar el ReportViewer en un formulario o control
            Form reportForm = new Form();
            reportForm.Controls.Add(reportViewer);
            reportViewer.Dock = DockStyle.Fill;
            reportForm.ShowDialog();
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


        public void setParametros(string nroDoc)
        {
            TxtOrden.Text = nroDoc;
            reportViewer1.RefreshReport();
        }

        //public void ConfigRep()
        //{
        //    ReportDataSource fuente = new ReportDataSource();

        //    //string ds = Path.GetFullPath("Reportes\\dsRepOrden");
        //    //fuente.Name = ds; // Nombre identico al que le di al dataset del report en tiempo de diseño
            
        //    fuente.Name = "CapaVisual_Login.Reportes.dsRepOrden";
        //    fuente.Value = reportViewer1.LocalReport.DataSources;

        //    //string reporte = Path.GetFullPath("Reportes\\RepOrden.rdlc");

        //    reportViewer1.LocalReport.DataSources.Clear();
        //    reportViewer1.LocalReport.DataSources.Add(fuente);
        //    //reportViewer1.LocalReport.ReportPath = reporte;
        //    reportViewer1.LocalReport.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RepOrden.rdlc";
        //    reportViewer1.ProcessingMode = ProcessingMode.Local;

        //}


        public void ConfigRep(bool MostrarConvencional = false, bool MostrarContacto = false)
        {
            ReportDataSource fuente = new ReportDataSource();
            fuente.Name = "CapaVisual_Login.Reportes.RepOrden.rdlc"; // Nombre identico al que le di al dataset del report en tiempo de diseño
            fuente.Value = reportViewer1.LocalReport.DataSources;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(fuente);
            reportViewer1.LocalReport.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RepOrden.rdlc";
            reportViewer1.ProcessingMode = ProcessingMode.Local;

            Convencional = MostrarConvencional;
            Contacto = MostrarContacto;
        }


        public void imprimir()
        {
            //LocalReport rdlc = new LocalReport();   //importante
            ////rdlc.ReportPath = reportViewer1.LocalReport.ReportPath;
            //rdlc.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RepOrden.rdlc";
            //this.sP_CPOS_REP_ORDENTableAdapter.Connection.Close();

            //string conexion = ConfigurationManager.ConnectionStrings["Epos"].ConnectionString;
            //this.sP_CPOS_REP_ORDENTableAdapter.Connection.ConnectionString = conexion;


            //this.sP_CPOS_REP_ORDENTableAdapter.Fill(this.dsRepOrden.SP_CPOS_REP_ORDEN, TxtOrden.Text);
            //rdlc.DataSources.Add(new ReportDataSource("DsRepOrden", sPCPOSREPORDENBindingSource));


            //Impresor imp = new Impresor();
            //imp.Imprime(rdlc);


            //Imprimir el reporte de contacto si MostrarContacto es true
            if (Contacto && !Convencional)
            {
                LocalReport rdlcContacto = new LocalReport();
                rdlcContacto.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RepOrdenTContacto.rdlc";

                // Llenar el dataset del reporte de contacto
                DataTable dtContacto = GetSubreportData(TxtOrden.Text);
                rdlcContacto.DataSources.Add(new ReportDataSource("DsRepOrden", dtContacto));

                Impresor imp = new Impresor();
                imp.Imprime(rdlcContacto);
            }

            // Imprimir el reporte de orden si MostrarSubRlc es true
            if (Convencional && !Contacto)
            {
                LocalReport rdlcOrden = new LocalReport();
                rdlcOrden.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RepOrden.rdlc";

                // Llenar el dataset del reporte de orden
                DataTable dtOrden = GetSubreportData(TxtOrden.Text);
                rdlcOrden.DataSources.Add(new ReportDataSource("DsRepOrden", dtOrden));

                Impresor imp = new Impresor();
                imp.Imprime(rdlcOrden);

            }
        }

        public LocalReport EnviarRreporte()
        {
            LocalReport rdlc2 = new LocalReport();   //importante
            //rdlc.ReportPath = reportViewer1.LocalReport.ReportPath;
            rdlc2.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RepOrden.rdlc";
            this.sP_CPOS_REP_ORDENTableAdapter.Connection.Close();

            string conexion = ConfigurationManager.ConnectionStrings["Epos"].ConnectionString;
            this.sP_CPOS_REP_ORDENTableAdapter.Connection.ConnectionString = conexion;


            this.sP_CPOS_REP_ORDENTableAdapter.Fill(this.dsRepOrden.SP_CPOS_REP_ORDEN, TxtOrden.Text);
            rdlc2.DataSources.Add(new ReportDataSource("DsRepOrden", sPCPOSREPORDENBindingSource));

            return rdlc2;
        }

    }
}
