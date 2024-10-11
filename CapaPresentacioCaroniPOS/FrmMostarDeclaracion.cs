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
        private string sucursal;
        private DateTime fecha;
        private string numeroOrden;
        private string nombreCliente;
        private string cedula;

        public FrmMostarDeclaracion(string sucursal, DateTime fecha, string numeroOrden= "", string nombreCliente = "", string cedula = "")
        {
            InitializeComponent();
            this.sucursal = sucursal;
            this.fecha = fecha;
            this.numeroOrden = numeroOrden;
            this.nombreCliente = nombreCliente;
            this.cedula = cedula;
        }

        private void FrmMostarDeclaracion_Load(object sender, EventArgs e)
        {
            // Formatear la fecha en el formato "dd-MMM-yyyy"
            string fechaFormateada = this.fecha.ToString("dd-MMM-yyyy", new System.Globalization.CultureInfo("es-ES"));

            // Establecer los parámetros del informe
            ReportParameter[] reportParameters = new ReportParameter[]
            {
            new ReportParameter("Sucursal", this.sucursal),
            new ReportParameter("Fecha", fechaFormateada),
            new ReportParameter("NumeroOrden", this.numeroOrden),
            new ReportParameter("NombreCliente", this.nombreCliente),
            new ReportParameter("Cedula", this.cedula)
            };

            this.reportViewer1.LocalReport.SetParameters(reportParameters);
            this.reportViewer1.RefreshReport();

            // Ajustar el tamaño del reportViewer1 para que ocupe todo el formulario
            this.reportViewer1.Size = new Size(this.ClientSize.Width, this.ClientSize.Height);
            this.reportViewer1.Location = new Point(0, 0);

            // Manejar el evento Resize del formulario para ajustar el tamaño del reportViewer1 dinámicamente
            this.Resize += new EventHandler(FrmMostarDeclaracion_Resize);
            this.reportViewer2.RefreshReport();
        }

        public void imprimir()
        {
            LocalReport rdlc = new LocalReport();   //importante
            Impresor imp = new Impresor();
            imp.Imprime(rdlc);
        }

        private void FrmMostarDeclaracion_Resize(object sender, EventArgs e)
        {
            //this.reportViewer1.Dock = DockStyle.Fill;
            this.reportViewer1.Size = new Size(this.ClientSize.Width, this.ClientSize.Height);
        }
    }
}
