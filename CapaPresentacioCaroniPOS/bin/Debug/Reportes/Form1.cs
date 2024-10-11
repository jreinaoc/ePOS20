using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using CapaDatos.Conexion;
using System.IO;

namespace CapaVisual_Login.reportes
{
    public partial class Form1 : Form
    {

        CapaDatos.Conexion.Conexion cn = new CapaDatos.Conexion.Conexion();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'DataSetReporAbono.SP_CPOS_ReporAbono' Puede moverla o quitarla según sea necesario.
            this.SP_CPOS_ReporAbonoTableAdapter.Fill(this.DsAbono.SP_CPOS_ReporAbono, Orden_txt.Text);
            // TODO: esta línea de código carga datos en la tabla 'DataSetRptAbono.SP_CPOS_RptOrdenAbono' Puede moverla o quitarla según sea necesario.
            //this.SP_CPOS_RptOrdenAbonoTableAdapter.Fill(this.DataSetRptAbono.SP_CPOS_RptOrdenAbono, Orden_txt.Text);

            //this.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dsOrdenAbono", SP_CPOS_RptOrdenAbonoBindingSource));
            this.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DsRpAbono", SP_CPOS_ReporAbonoBindingSource));
            // TODO: esta línea de código carga datos en la tabla 'DataSet1.SP_CPOS_ReporAbono' Puede moverla o quitarla según sea necesario.
            //this.SP_CPOS_ReporAbonoTableAdapter.Fill(this.DataSet1.SP_CPOS_ReporAbono, Orden_txt.Text);

            //this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
        }

        public void setParametros(string nroDoc) 
        {
            Orden_txt.Text = nroDoc;
            reportViewer1.RefreshReport();
        }

        public void algo()
        {
            ReportDataSource fuente = new ReportDataSource();

            string ds = Path.GetFullPath("..\\..\\Reportes\\dsOrdenAbono");
            fuente.Name = ds; // @"C:\CaroniPOS\CaroniPOS\CapaPresentacioCaroniPOS\reportes\dsOrdenAbono"; // Nombre identico al que le di al dataset del report en tiempo de diseño
            fuente.Value = reportViewer1.LocalReport.DataSources;

            string reporte = Path.GetFullPath("..\\..\\Reportes\\RptOrdenAbono.rdlc");

            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(fuente);
            //reportViewer1.LocalReport.ReportEmbeddedResource = "RptOrdenAbono"; //'exactamente como se llaman el proyecto
            reportViewer1.LocalReport.ReportPath = reporte; //@"C:\CaroniPOS\CaroniPOS\CapaPresentacioCaroniPOS\reportes\RptOrdenAbono.rdlc";
            reportViewer1.ProcessingMode = ProcessingMode.Local;

        }


        //private void btn_buscar_Click(object sender, EventArgs e)
        //{
        //  ReportParameter p = new ReportParameter("Orden", Orden_txt.Text);
        //  reportViewer1.LocalReport.SetParameters(p);
        //  reportViewer1.RefreshReport();
        //}
    }
}
