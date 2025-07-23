using CapaDatos.CargarOrdenes_Datos;
using CapaDatos.DetalleOrden_Datos;
using CapaDatos.Inicio_Datos;
using CapaDatos.Login_Datos;
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
        private D_Login _D_Login = new D_Login();
        private D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        private D_Inicio _D_Inicio = new D_Inicio();
        private D_Articulos _D_Articulos = new D_Articulos() ;

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ReportesCierreCaja(false);


            }
            catch (Exception ex)
            {

                string Respuesta = string.Format("Error: {0}", ex.Message);
            }
        }

        public void ReportesCierreCaja(bool cierreEnCero)
        {
            string Sucursal = _D_DetalleOrden.TB_PARAMETRO("SucursalId");
            DataSet Datos = _D_Login.SucursalCompania(Sucursal);
            string Descripcion = "";
            string RifCompania = "";
            DateTime DiaActivo = _D_Inicio.DiaActivo().AddDays(-6);
            string NombreSucursal = "";
            bool imprimir = false;

            if (Datos.Tables[0].Rows.Count > 0)
            {
                DataRow row = Datos.Tables[0].Rows[0];
                Descripcion = row["DescripcionCompania"].ToString();
                RifCompania = row["RifCompania"].ToString();
            }

            DataSet dsSucursal = _D_Articulos.TB_SUCURSALES(Sucursal);

            if (dsSucursal.Tables[0].Rows.Count > 0)
            {
                NombreSucursal = Sucursal + " - " + dsSucursal.Tables[0].Rows[0]["Descripcion"].ToString();
            }


            ////ReporteTranferencia(imprimir, RifCompania, Descripcion, NombreSucursal, Sucursal, DiaActivo.AddDays(-5));
            if (cierreEnCero)
            {
                ReporteCierreCaja(imprimir, RifCompania, Descripcion, NombreSucursal, Sucursal, DiaActivo);
            }
            else
            {
                ReporteCierreCaja(imprimir, RifCompania, Descripcion, NombreSucursal, Sucursal, DiaActivo);
                ReporteTranferencia(imprimir, RifCompania, Descripcion, NombreSucursal, Sucursal, DiaActivo);
                ReporteVuelto(imprimir, RifCompania, Descripcion, NombreSucursal, Sucursal, DiaActivo);
            }

        }
        public void ReporteTranferencia(bool imprimir, string RifCompania, string Descripcion, string NombreSucursal, string Sucursal, DateTime DiaActivo)
        {

            var parametros = new Dictionary<string, string>
                {
                      { "FechaDesde", DiaActivo.ToString("dd/MM/yyyy")},
                      { "Compania", Descripcion  },
                      { "RifCompania", RifCompania },
                      { "Sucursal",  Sucursal },
                      { "NombreSucursal", NombreSucursal },
                };

            // Supón que tienes estos datos:
            string nombreReporte = "CapaVisual_Login.Reportes.RptPagosTranferencia.rdlc";
            string nombreDataSource = "DsRepPagosTranferencia";
            DataTable datosReporte = ObtenerDatosParaReporte(conexion, DiaActivo, Sucursal); // Tu método para obtener los datos

            // Instanciar y mostrar el formulario
            FrmMostrarRep frm = new FrmMostrarRep(nombreReporte, nombreDataSource, imprimir, datosReporte, parametros);
            frm.ShowDialog();
        }

        public void ReporteCierreCaja(bool imprimir, string RifCompania, string Descripcion, string NombreSucursal, string Sucursal, DateTime DiaActivo)
        {

            var parametros = new Dictionary<string, string>
                    {
                      { "Compania", Descripcion },
                      { "Fecha",DiaActivo.ToString("dd/MM/yyyy")},
                      { "Sucursal", Sucursal },
                      { "RifCompania",RifCompania },
                      { "NombreSucursal", NombreSucursal },
                    };


            var dataSources = new Dictionary<string, DataTable>
                    {
    { "DS_TB_CAJA", ObtenerDatosParaCierreCaja1(conexion, DiaActivo , Sucursal)},         // Nombre debe coincidir con el del reporte (.rdlc)
    { "DS_TB_SUCURSALES", ObtenerDatosParaCierreCaja2(conexion, DiaActivo ,Sucursal)},
    { "DS_VW_CierreCaja", ObtenerDatosParaCierreCaja3(conexion, DiaActivo , Sucursal)},
    { "DS_TB_USUARIO", ObtenerDatosParaCierreCaja4(conexion, DiaActivo , Sucursal)}
                    };

            // Supón que tienes estos datos:
            string nombreReporte = "CapaVisual_Login.Reportes.RepCierreDeCaja.rdlc";
            string nombreDataSource = "DSCierreDeCaja";
            // o true si quieres imprimir automáticamente

            // Instanciar y mostrar el formulario
            FrmMostrarRep frm = new FrmMostrarRep(nombreReporte, nombreDataSource, imprimir, null, parametros, dataSources);
            frm.ShowDialog();

        }
        public void ReporteVuelto (bool imprimir, string RifCompania, string Descripcion, string NombreSucursal,string Sucursal, DateTime DiaActivo)
            {
            var parametros = new Dictionary<string, string>
                    {
                      { "uNombreCompania",Descripcion },
                      { "uNombreSucursal",NombreSucursal},
                      { "uFechaInicio", DiaActivo.ToString("dd/MM/yyyy")},
                      { "RifCompania",RifCompania},
                    };

            // Supón que tienes estos datos:
            string nombreReporte = "CapaVisual_Login.Reportes.RepVuelto.rdlc";
            string nombreDataSource = "DSRepVuelto";

            DataTable datosReporte = ObtenerDatosParaReporteVuelto(conexion, Sucursal, DiaActivo); // Tu método para obtener los datos

            // Instanciar y mostrar el formulario
            FrmMostrarRep frm = new FrmMostrarRep(nombreReporte, nombreDataSource, imprimir, datosReporte, parametros);
            frm.ShowDialog();

        }

        public DataTable ObtenerDatosParaCierreCaja1(string conexion, DateTime param1, string param2)
        {
            //// Crea una instancia del DataSet y TableAdapter
            Reportes.DSCierreDeCaja ds = new Reportes.DSCierreDeCaja();
            ds.EnforceConstraints = false;

            // Configurar y ejecutar cada adaptador
            var adapter1 = new Reportes.DSCierreDeCajaTableAdapters.TB_CAJATableAdapter();
            adapter1.Connection.ConnectionString = conexion;
            adapter1.Fill(ds.TB_CAJA, param1, param2); // Ajusta según necesites parámetros

            return ds.TB_CAJA;
        }

        public DataTable ObtenerDatosParaCierreCaja2(string conexion, DateTime param1, string param2)
        {
            //// Crea una instancia del DataSet y TableAdapter
            Reportes.DSCierreDeCaja ds = new Reportes.DSCierreDeCaja();
            ds.EnforceConstraints = false;

            var adapter2 = new Reportes.DSCierreDeCajaTableAdapters.TB_SUCURSALESTableAdapter();
            adapter2.Connection.ConnectionString = conexion;
            adapter2.Fill(ds.TB_SUCURSALES, param2);

            // Establecer relaciones si es necesario
            // ds.Relations.Add(...);

            return ds.TB_SUCURSALES;
        }

        public DataTable ObtenerDatosParaCierreCaja3(string conexion, DateTime param1, string param2)
        {
            //// Crea una instancia del DataSet y TableAdapter
            Reportes.DSCierreDeCaja ds = new Reportes.DSCierreDeCaja();
            ds.EnforceConstraints = false;

            var adapter3 = new Reportes.DSCierreDeCajaTableAdapters.VW_CierreCajaTableAdapter();
            adapter3.Connection.ConnectionString = conexion;
            adapter3.Fill(ds.VW_CierreCaja, param1); // Ajusta parámetros

            // Establecer relaciones si es necesario
            // ds.Relations.Add(...);

            return ds.VW_CierreCaja;
        }

        public DataTable ObtenerDatosParaCierreCaja4(string conexion, DateTime param1, string param2)
        {
            //// Crea una instancia del DataSet y TableAdapter
            Reportes.DSCierreDeCaja ds = new Reportes.DSCierreDeCaja();
            ds.EnforceConstraints = false;

            var adapter4 = new Reportes.DSCierreDeCajaTableAdapters.TB_USUARIOTableAdapter();
            adapter4.Connection.ConnectionString = conexion;
            adapter4.Fill(ds.TB_USUARIO, param2);

            return ds.TB_USUARIO;
        }

        public DataTable ObtenerDatosParaReporteVuelto(string conexion, string param1, DateTime param2)
        {
            // Crea una instancia del DataSet y TableAdapter
            Reportes.DSRepVuelto ds = new Reportes.DSRepVuelto();
            Reportes.DSRepVueltoTableAdapters.SP_TraerTempCambioTableAdapter adapter = new Reportes.DSRepVueltoTableAdapters.SP_TraerTempCambioTableAdapter();

            // Cierra cualquier conexión abierta
            adapter.Connection.Close();

            // Asigna la cadena de conexión
            adapter.Connection.ConnectionString = conexion;
            ds.EnforceConstraints = false;
            // Llena el DataTable usando el TableAdapter y los parámetros necesarios
            adapter.Fill(ds.SP_TraerTempCambio, param1, param2);
            //ds.EnforceConstraints = true;
            // Retorna el DataTable con los datos
            return ds.SP_TraerTempCambio;
        }

        public DataTable ObtenerDatosParaReporte(string conexion, DateTime param1, string param2)
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

        private void FrmPrueba_Load(object sender, EventArgs e)
        {

        }
    }
}
