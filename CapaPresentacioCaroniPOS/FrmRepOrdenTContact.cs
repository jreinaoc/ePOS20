using CapaVisual_Login.Reportes;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVisual_Login
{
    public partial class FrmRepOrdenTContact : Form
    {
        public FrmRepOrdenTContact()
        {
            InitializeComponent();
        }

        private void FrmRepOrdenTContact_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'dsAbono.SP_CPOS_ReporAbono' Puede moverla o quitarla según sea necesario.
            this.sP_CPOS_REP_ORDENTableAdapter.Connection.Close();

            string conexion = ConfigurationManager.ConnectionStrings["Epos"].ConnectionString;
            this.sP_CPOS_REP_ORDENTableAdapter.Connection.ConnectionString = conexion;

        

            this.sP_CPOS_REP_ORDENTableAdapter.Fill(this.dsRepOrden.SP_CPOS_REP_ORDEN, TxtOrden.Text);

      

            this.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DsRepOrden", sPCPOSREPORDENBindingSource));

    

            this.reportViewer1.RefreshReport();
        }

        public void setParametros(string nroDoc)
        {
            TxtOrden.Text = nroDoc;
            reportViewer1.RefreshReport();
        }

        public void ConfigRep()
        {
            ReportDataSource fuente = new ReportDataSource();
            //string ds = Path.GetFullPath("Reportes\\dsRepOrden");
            //fuente.Name = ds; // Nombre identico al que le di al dataset del report en tiempo de diseño
            fuente.Name = "CapaVisual_Login.Reportes.dsRepOrden";
            fuente.Value = reportViewer1.LocalReport.DataSources;

            //string reporte = Path.GetFullPath("Reportes\\RepOrdenTContacto.rdlc");

            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(fuente);
            //reportViewer1.LocalReport.ReportPath = reporte;
            reportViewer1.LocalReport.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RepOrdenTContacto.rdlc";
            reportViewer1.ProcessingMode = ProcessingMode.Local;

          
        }

        public void imprimir()
        {
            LocalReport rdlc = new LocalReport();   //importante
            //rdlc.ReportPath = reportViewer1.LocalReport.ReportPath;
            rdlc.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RepOrdenTContacto.rdlc";
            this.sP_CPOS_REP_ORDENTableAdapter.Connection.Close();

            string conexion = ConfigurationManager.ConnectionStrings["Epos"].ConnectionString;
            this.sP_CPOS_REP_ORDENTableAdapter.Connection.ConnectionString = conexion;
            this.sP_CPOS_REP_ORDENTableAdapter.Fill(this.dsRepOrden.SP_CPOS_REP_ORDEN, TxtOrden.Text);
            rdlc.DataSources.Add(new ReportDataSource("DsRepOrden", sPCPOSREPORDENBindingSource));

            Impresor imp = new Impresor();
            imp.Imprime(rdlc);
        }



    }
}
