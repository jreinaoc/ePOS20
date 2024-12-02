using CapaVisual_Login.Reportes;
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


namespace CapaVisual_Login
{
    public partial class FrmMostarDeclaracion : Form
    {
        private string sucursal = "";
        private string fecha = DateTime.Now.ToString("dd/MM/yyyy");
        private string numeroOrden = "";
        private string nombreCliente = "";
        private string cedula = "";
        private string Observacion = "";
        private string Reparacion = "";
        private string CristalPropio = "";
        private string MonturaPropia = "";

        public FrmMostarDeclaracion()
        {
            InitializeComponent();
        }

        // Constructor con parámetros
        public void Parametros(string sucursal, string fecha, string numeroOrden, string nombreCliente, string cedula, string Observacion, string Reparacion, string CristalPropio, string MonturaPropia)
        {
            this.sucursal = sucursal;
            this.fecha = fecha;
            this.numeroOrden = numeroOrden;
            this.nombreCliente = nombreCliente;
            this.cedula = cedula;
            this.Observacion = Observacion;
            this.Reparacion = Reparacion;
            this.CristalPropio = CristalPropio;
            this.MonturaPropia = MonturaPropia;
            reportViewer1.RefreshReport();
        }

        private void FrmMostarDeclaracion_Load(object sender, EventArgs e)
        {
            // Establecer los parámetros del informe
            ReportParameter[] reportParameters = new ReportParameter[]
            {
                new ReportParameter("Sucursal", this.sucursal),
                new ReportParameter("Fecha", this.fecha),
                new ReportParameter("NumeroOrden", this.numeroOrden),
                new ReportParameter("NombreCliente", this.nombreCliente),
                new ReportParameter("Cedula", this.cedula),
                new ReportParameter("Observacion", this.Observacion),
                new ReportParameter("CristalPropio", this.CristalPropio),
                new ReportParameter("MonturaPropia", this.MonturaPropia)
            };

            this.reportViewer1.LocalReport.SetParameters(reportParameters);
            this.reportViewer1.RefreshReport();

            // Ajustar el tamaño del reportViewer1 para que ocupe todo el formulario
            this.reportViewer1.Size = new Size(this.ClientSize.Width, this.ClientSize.Height);
            this.reportViewer1.Location = new Point(0, 0);

            // Manejar el evento Resize del formulario para ajustar el tamaño del reportViewer1 dinámicamente
            this.Resize += new EventHandler(FrmMostarDeclaracion_Resize);
            this.reportViewer1.RefreshReport();
        }
        public void ConfigRep()
        {
            ReportDataSource fuente = new ReportDataSource();

            //string ds = Path.GetFullPath("Reportes\\dsRepOrden");
            //fuente.Name = ds; // Nombre identico al que le di al dataset del report en tiempo de diseño

            fuente.Name = "CapaVisual_Login.Reportes.RepDeclaracionResponsabilidad";
            fuente.Value = reportViewer1.LocalReport.DataSources;

            //string reporte = Path.GetFullPath("Reportes\\RepOrden.rdlc");

            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(fuente);
            //reportViewer1.LocalReport.ReportPath = reporte;
            reportViewer1.LocalReport.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RepDeclaracionResponsabilidad.rdlc";
            reportViewer1.ProcessingMode = ProcessingMode.Local;

        }


        public void imprimir_NumeroCopia(int numeroDeCopias)
        {
            LocalReport rdlc = new LocalReport();
            rdlc.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RepDeclaracionResponsabilidad.rdlc";

            // Establecer los parámetros del informe
            ReportParameter[] reportParameters = new ReportParameter[]
            {
                new ReportParameter("Sucursal", this.sucursal),
                new ReportParameter("Fecha", this.fecha),
                new ReportParameter("NumeroOrden", this.numeroOrden),
                new ReportParameter("NombreCliente", this.nombreCliente),
                new ReportParameter("Cedula", this.cedula),
                new ReportParameter("Observacion", this.Observacion),
                new ReportParameter("CristalPropio", this.CristalPropio),
                new ReportParameter("MonturaPropia", this.MonturaPropia)
            };

            // Establecer los parámetros en el reporte
            rdlc.SetParameters(reportParameters);

            //// Renderizar el informe para asegurarse de que se muestra correctamente
            //byte[] bytes = rdlc.Render("PDF");

            // Ajustar el tamaño del reportViewer1 para que ocupe todo el formulario
            this.reportViewer1.Size = new Size(this.ClientSize.Width, this.ClientSize.Height);
            this.reportViewer1.Location = new Point(0, 0);

            // Manejar el evento Resize del formulario para ajustar el tamaño del reportViewer1 dinámicamente
            this.Resize += new EventHandler(FrmMostarDeclaracion_Resize);
            this.reportViewer1.RefreshReport();

            Impresor imp = new Impresor();
            imp.Imprime_NumeroCopias(rdlc, numeroDeCopias);
        }

        private void FrmMostarDeclaracion_Resize(object sender, EventArgs e)
        {
            //this.reportViewer1.Dock = DockStyle.Fill;
            this.reportViewer1.Size = new Size(this.ClientSize.Width, this.ClientSize.Height);
        }

        private DataTable GetData()
        {
            // Implementa la lógica para obtener los datos
            DataTable dataTable = new DataTable();
            // Rellenar dataTable con datos
            return dataTable;
        }
    }
}