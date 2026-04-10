using CapaVisual_Login.Reportes;
using Microsoft.Reporting.WinForms;
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
    public partial class FrmMostrarRep : Form
    {
        private string _nombreReporte;
        private string _nombreDataSource;
        private bool _imprimir;
        private DataTable _datosReporte;
        Dictionary<string, string> _Parametros;
        Dictionary<string, DataTable> _dataSources;
        Dictionary<string, DataTable> dataSources = new Dictionary<string, DataTable>();

        public FrmMostrarRep(string nombreReporte = null , string nombreDataSource = null , bool imprimir = false, DataTable datosReporte = null, Dictionary<string, string> parametros = null, Dictionary<string, DataTable> dataSources = null)
        {
            InitializeComponent();
            _nombreReporte = nombreReporte;
            _nombreDataSource = nombreDataSource;
            _datosReporte = datosReporte;
            _imprimir = imprimir;
            _Parametros = parametros;
            _dataSources = dataSources;
        }

        private string conexion = ConfigurationManager.ConnectionStrings["Epos"].ConnectionString;

        private void FrmMostrarRep_Load(object sender, EventArgs e)
        {
            LocalReport rdlc = null;

            if (!string.IsNullOrEmpty(_nombreReporte) && !string.IsNullOrEmpty(_nombreDataSource) && _datosReporte != null)
            {
                rdlc = MostrarReporteGenerico(_nombreReporte, _nombreDataSource, _datosReporte, _Parametros);
                if (_imprimir)
                {
                    Impresor imp = new Impresor();
                    //imp.Imprime(rdlc);    
                    imp.Imprime_NumeroCopias(rdlc, 1);
                    this.Close();
                }
            }
            else if (!string.IsNullOrEmpty(_nombreReporte) && _dataSources != null)
            {
                rdlc = MostrarReporteGenerico2(_nombreReporte, _dataSources, _Parametros);
                if (_imprimir)
                {
                    Impresor imp = new Impresor();
                    imp.Imprime(rdlc);
                    this.Close();
                }
            }
        }

        private LocalReport MostrarReporteGenerico(string nombreReporte, string nombreDataSource, DataTable datos, Dictionary<string, string> parametros = null)
        {
            // Asigna los datos al BindingSource
            bindingSource2.DataSource = datos;

            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.ReportEmbeddedResource = nombreReporte;

            // Usa el BindingSource como fuente de datos para el reporte
            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource(nombreDataSource, bindingSource2));

            //if (datos == null || datos.Rows.Count == 0)
            //{
            //    MessageBox.Show("No hay datos para mostrar en el reporte.");
            //}

            // Si hay parámetros, los asignas aquí
            if (parametros != null)
            {
                var listaParametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
                foreach (var kvp in parametros)
                {
                    listaParametros.Add(new Microsoft.Reporting.WinForms.ReportParameter(kvp.Key, kvp.Value));
                }
                reportViewer1.LocalReport.SetParameters(listaParametros);
            }

            reportViewer1.RefreshReport();

            return reportViewer1.LocalReport;
        }

        private LocalReport MostrarReporteGenerico2(
            string nombreReporte,
            Dictionary<string, DataTable> dataSources,
            Dictionary<string, string> parametros = null)
        {
            try
            {
                reportViewer1.LocalReport.ReportEmbeddedResource = nombreReporte;
                reportViewer1.LocalReport.DataSources.Clear();

                bool hasData = false;

                foreach (var ds in dataSources)
                {
                    if (ds.Value != null && ds.Value.Rows.Count > 0)
                    {
                        reportViewer1.LocalReport.DataSources.Add(
                            new ReportDataSource(ds.Key, ds.Value));
                        hasData = true;
                    }
                }

                //if (!hasData)
                //{
                //    MessageBox.Show("No hay datos para mostrar en el reporte.");
                //    return null;
                //}

                if (parametros != null && parametros.Count > 0)
                {
                    var listaParametros = parametros.Select(kvp =>
                        new Microsoft.Reporting.WinForms.ReportParameter(kvp.Key, kvp.Value))
                        .ToList();

                    reportViewer1.LocalReport.SetParameters(listaParametros);
                }

                reportViewer1.RefreshReport();

                return reportViewer1.LocalReport;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar el reporte: {ex.Message}");
                return null;
            }
        }


        public void ConfigRep()
        {
            ReportDataSource fuente = new ReportDataSource();
            fuente.Name = "CapaVisual_Login.Reportes.DsRepCambio"; // Nombre identico al que le di al dataset del report en tiempo de diseño
            fuente.Value = reportViewer1.LocalReport.DataSources;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(fuente);
            reportViewer1.LocalReport.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RepCambio.rdlc";
            reportViewer1.ProcessingMode = ProcessingMode.Local;
        }

        public void imprimir(string Orden, string Factura, string correlativo, string Nombre_Sucursal, string Cliente, string telefono, string Banco, string Monto)
        {

            LocalReport rdlc = new LocalReport();   //importante
            rdlc.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RepCambio.rdlc";
            // cierras cualquier conexion que pueda estar abierta 
            this.SP_CPOS_RepCambioTableAdapter.Connection.Close();
            this.SP_CPOS_RepCambioTableAdapter.Connection.ConnectionString = conexion;
            this.SP_CPOS_RepCambioTableAdapter.Fill(this.dsCambio.SP_CPOS_RepCambio, "", "", "");

            //parametro para enviar al reporte 
            ReportParameter reportParameter = new ReportParameter("NunFactura", Factura);
            ReportParameter reportParameter2 = new ReportParameter("NunCorrelativo", correlativo);
            ReportParameter reportParameter3 = new ReportParameter("NunOs", Orden);
            ReportParameter reportParameter4 = new ReportParameter("NunSucursal", Nombre_Sucursal);
            ReportParameter reportParameter5 = new ReportParameter("NunCliente", Cliente);
            ReportParameter reportParameter6 = new ReportParameter("NunTelefono", telefono);
            ReportParameter reportParameter7 = new ReportParameter("NunBanco", Banco);
            ReportParameter reportParameter8 = new ReportParameter("NunMonto", Monto);

            rdlc.SetParameters(new ReportParameter[] { reportParameter, reportParameter2, reportParameter3, reportParameter4, reportParameter5, reportParameter6, reportParameter7, reportParameter8 });
            rdlc.DataSources.Add(new ReportDataSource("DsRepCambio", bindingSource1));
            Impresor imp = new Impresor();
            imp.Imprime(rdlc);

        }

        public void ImprimirPagosTarjeta( DateTime fecha,  string codSucursal,  string FechaDesde, string Compania,  string RifCompania, string Sucursal,  string NombreSucursal, bool Imprimir)
        {
            //    LocalReport rdlc = new LocalReport();
            //    rdlc.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RepPagsoPorTarjeta.rdlc";

            //    // 🔴 TABLA 1
            //    this.cPOS_PagosTarjetaTableAdapter.Connection.Close();
            //    this.cPOS_PagosTarjetaTableAdapter.Connection.ConnectionString = conexion;
            //    this.cPOS_PagosTarjetaTableAdapter.Fill(this.dsRepPagosPorTarjeta.CPOS_PagosTarjeta, fecha, codSucursal);
            //    // 🔴 TABLA 2
            //    this.cPOS_PagosTarjeta_CreditoTableAdapter.Connection.Close();
            //    this.cPOS_PagosTarjeta_CreditoTableAdapter.Connection.ConnectionString = conexion;
            //    this.cPOS_PagosTarjeta_CreditoTableAdapter.Fill(this.dsRepPagosPorTarjeta.CPOS_PagosTarjeta_Credito, fecha, codSucursal);

            //    // 🔥 PARÁMETROS
            //    ReportParameter[] parametros = new ReportParameter[]
            //    { new ReportParameter("FechaDesde", FechaDesde),
            //new ReportParameter("Compania", Compania),
            //new ReportParameter("RifCompania", RifCompania),
            //new ReportParameter("Sucursal", Sucursal),
            //new ReportParameter("NombreSucursal", NombreSucursal)
            //    };

            //    rdlc.SetParameters(parametros);

            //    // 🔥 DATA SOURCES (CLAVE)
            //    rdlc.DataSources.Clear();


            //    rdlc.DataSources.Add(new ReportDataSource(
            //        "DataSet1", // 👈 nombre del RDLC
            //        bindingSource4
            //    ));

            //    rdlc.DataSources.Add(new ReportDataSource(
            //        "DataSet2", // 👈 nombre del RDLC
            //        bindingSource3
            //    ));

            //    if (Imprimir)
            //    {
            //        reportViewer1.LocalReport.DataSources.Clear();
            //        reportViewer1.LocalReport.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RepPagsoPorTarjeta.rdlc";

            //        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("CPOS_PagosTarjeta", this.DsRepPagosPorTarjeta.CPOS_PagosTarjeta));
            //        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("CPOS_PagosTarjeta_Credito", this.DsRepPagosPorTarjeta.CPOS_PagosTarjeta_Credito));

            //        reportViewer1.LocalReport.SetParameters(parametros);
            //        reportViewer1.RefreshReport();
            //    }
            //    else
            //    {

            //    }




            //    // 🔥 IMPRIMIR
            //    Impresor imp = new Impresor();
            //    imp.Imprime(rdlc);

            // Configurar los BindingSources
            bindingSource4.DataSource = this.dsRepPagosPorTarjeta.CPOS_PagosTarjeta;
            bindingSource3.DataSource = this.dsRepPagosPorTarjeta.CPOS_PagosTarjeta_Credito;

            // Configurar conexiones y llenar datos
            this.cPOS_PagosTarjetaTableAdapter.Connection.ConnectionString = conexion;
            this.cPOS_PagosTarjeta_CreditoTableAdapter.Connection.ConnectionString = conexion;

            this.cPOS_PagosTarjetaTableAdapter.Fill(this.dsRepPagosPorTarjeta.CPOS_PagosTarjeta, fecha, codSucursal);
            this.cPOS_PagosTarjeta_CreditoTableAdapter.Fill(this.dsRepPagosPorTarjeta.CPOS_PagosTarjeta_Credito, fecha, codSucursal);

            // Parámetros
            ReportParameter[] parametros = new ReportParameter[]
            {
            new ReportParameter("FechaDesde", FechaDesde),
            new ReportParameter("Compania", Compania),
            new ReportParameter("RifCompania", RifCompania),
            new ReportParameter("Sucursal", Sucursal),
            new ReportParameter("NombreSucursal", NombreSucursal)
            };

            LocalReport rdlc = new LocalReport();
            rdlc.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RepPagsoPorTarjeta.rdlc";
            rdlc.SetParameters(parametros);
            rdlc.DataSources.Clear();

            rdlc.DataSources.Add(new ReportDataSource("CPOS_PagosTarjeta", bindingSource4));
            rdlc.DataSources.Add(new ReportDataSource("CPOS_PagosTarjeta_Credito", bindingSource3));

            if (Imprimir)
            {
                Impresor imp = new Impresor();
                imp.Imprime(rdlc);
            }
            else
            {
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.ReportEmbeddedResource = "CapaVisual_Login.Reportes.RepPagsoPorTarjeta.rdlc";
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("CPOS_PagosTarjeta", bindingSource4));
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("CPOS_PagosTarjeta_Credito", bindingSource3));
                reportViewer1.LocalReport.SetParameters(parametros);
                reportViewer1.RefreshReport();
            }
        }


        public void Mostrar(string Orden,string Factura, string correlativo, string Nombre_Sucursal, string Cliente, string telefono, string Banco, string Monto)
        {
            // cierras cualquier conexion que pueda estar abierta 
            this.SP_CPOS_RepCambioTableAdapter.Connection.Close();
            //Abres nuevamente la conexion
            this.SP_CPOS_RepCambioTableAdapter.Connection.ConnectionString = conexion;
            this.SP_CPOS_RepCambioTableAdapter.Fill(this.dsCambio.SP_CPOS_RepCambio, "", "", "");
            

            //parametro para enviar al reporte 
            ReportParameter reportParameter = new ReportParameter("NunFactura", Factura);
            ReportParameter reportParameter2 = new ReportParameter("NunCorrelativo", correlativo);
            ReportParameter reportParameter3 = new ReportParameter("NunOs", Orden);
            ReportParameter reportParameter4 = new ReportParameter("NunSucursal", Nombre_Sucursal);
            ReportParameter reportParameter5 = new ReportParameter("NunCliente", Cliente);
            ReportParameter reportParameter6 = new ReportParameter("NunTelefono", telefono);
            ReportParameter reportParameter7 = new ReportParameter("NunBanco", Banco);
            ReportParameter reportParameter8 = new ReportParameter("NunMonto", Monto);

            this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { reportParameter,reportParameter2,reportParameter3,reportParameter4,reportParameter5,reportParameter6,reportParameter7,reportParameter8 });

            //Mostrar el reporte en el reportViwer1
            this.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DsRepCambio", bindingSource1));
            this.reportViewer1.RefreshReport();

        }

    }

 }

