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

namespace CapaVisual_Login
{
    public partial class FrmRepContratGart : Form
    {
        public FrmRepContratGart()
        {
            InitializeComponent();
        }

        private void FrmRepContratGart_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'CGarantiaDataSet.SP_CPOS_ReporGarantiaExtendida' Puede moverla o quitarla según sea necesario.
            this.SP_CPOS_ReporGarantiaExtendidaTableAdapter.Connection.Close();

            string conexion = ConfigurationManager.ConnectionStrings["Epos"].ConnectionString;
            this.SP_CPOS_ReporGarantiaExtendidaTableAdapter.Connection.ConnectionString = conexion;

            this.SP_CPOS_ReporGarantiaExtendidaTableAdapter.Fill(this.CGarantiaDataSet.SP_CPOS_ReporGarantiaExtendida,TxtOrden.Text);

            this.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSetContGarantia", SP_CPOS_ReporGarantiaExtendidaBindingSource));

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
            //string ds = Path.GetFullPath("Reportes\\CGatantiaDataSet");
            //fuente.Name = ds; // Nombre identico al que le di al dataset del report en tiempo de diseño

            fuente.Name = "CapaVisual_Login.Reportes.CGatantiaDataSet";
            fuente.Value = reportViewer1.LocalReport.DataSources;

            //string reporte = Path.GetFullPath("Reportes\\RepContratoGarantia.rdlc");

            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(fuente);
            //reportViewer1.LocalReport.ReportPath = reporte;
            reportViewer1.LocalReport.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RepContratoGarantia.rdlc";
            reportViewer1.ProcessingMode = ProcessingMode.Local;
        }

        public void imprimir()
        {
            LocalReport rdlc = new LocalReport();   //importante
            //rdlc.ReportPath = reportViewer1.LocalReport.ReportPath;
            rdlc.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RepContratoGarantia.rdlc";
            this.SP_CPOS_ReporGarantiaExtendidaTableAdapter.Connection.Close();

            string conexion = ConfigurationManager.ConnectionStrings["Epos"].ConnectionString;
            this.SP_CPOS_ReporGarantiaExtendidaTableAdapter.Connection.ConnectionString = conexion;
            this.SP_CPOS_ReporGarantiaExtendidaTableAdapter.Fill(this.CGarantiaDataSet.SP_CPOS_ReporGarantiaExtendida, TxtOrden.Text);
            rdlc.DataSources.Add(new ReportDataSource("DataSetContGarantia", SP_CPOS_ReporGarantiaExtendidaBindingSource));

            Impresor imp = new Impresor();
            imp.Imprime(rdlc);
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
