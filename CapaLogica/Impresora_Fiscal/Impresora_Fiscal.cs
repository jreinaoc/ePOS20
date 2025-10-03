using CapaDatos.DetalleOrden_Datos;
using CapaDatos.Inicio_Datos;
using System.Globalization;
using System;
using System.Configuration;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CapaEntidades;
//using CapaLogica.Anulacion_Logica;
using CapaDatos.Anulacion;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;

namespace CapaLogica.Impresora_Fiscal
{
    public class Impresora_Fiscal
    {
        //El uso de la clase StringBuilder nos ayudara a devolver los mensajes de las validaciones
        public readonly StringBuilder stringBuilder = new StringBuilder();
        private int glbPuertoCOM = Convert.ToInt16(ConfigurationManager.AppSettings.Get("PuertoCOMimpresora"));
        private D_Inicio _D_Inicio = new D_Inicio();
        private D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        public string NumeroNCFiscal = "";
        //private FrmMensajes _FrmMensajes = new FrmMensajes();
        D_Anulacion _D_Anulacion = new D_Anulacion();

        static readonly Regex Hex4 = new Regex(@"^[0-9A-Fa-f]{4}$", RegexOptions.Compiled);

        // Clases de apoyo
        public class EstadoImpresora
        {
            public bool Online { get; set; }
            public bool TapaAbierta { get; set; }
            public bool TemperaturaAlta { get; set; }
            public bool ErrorNoRecuperable { get; set; }
            public bool ErrorCortadora { get; set; }
            public bool BufferOverflow { get; set; }
            public bool TienePapel { get; set; }
            public bool PocoPapel { get; set; }
            public bool SinPapel { get; set; }
            public bool ErrorGeneral { get; set; }

        }

        public void IniciarImpresora(int Opcion)
        {
            string puerto = "COM1";
            uint resp = 0;


            try
            {
                puerto = ("COM" + Convert.ToString(AbrirPuerto()));

                VmaxComVe.VmaxComClass objVmax = new VmaxComVe.VmaxComClass();
                resp = objVmax.AbrirPuerto(puerto);

                if (resp != 0)
                {
                    objVmax.CerrarPuerto();
                }

                else
                {

                    switch (Opcion)
                    {
                        case 1:
                            Factura(puerto);
                            break;

                        case 2:
                            Nota_De_Credito(puerto);
                            break;

                        case 3:
                            Reporte("X", puerto);
                            break;

                        case 4:
                            Reporte("Z", puerto);
                            break;

                        case 5:
                            ReportesElectronicos(puerto, 1);
                            break;

                        case 6:
                            ReportesElectronicos(puerto, 2);
                            break;

                        default:
                            Environment.Exit(0);
                            break;

                    }
                }


            }

            catch (Exception ex)
            {
                stringBuilder.Append("Por favor comunicarse con el Dpto de Sistemas y reportar el siguiente error: " + Environment.NewLine + string.Format("Error: {0}", ex.Message));
            }
        }


        public int AbrirPuerto()
        {
            int port;
            port = glbPuertoCOM;
            return port;
        }

        private void Reporte(string tipo, string puerto)
        {
            try
            {
                VmaxComVe.VmaxComClass objVmax = new VmaxComVe.VmaxComClass();

                uint ret = 0;

                ret = objVmax.AbrirPuerto(puerto);

                if (tipo == "X")
                    ret = objVmax.ReporteX();
                else
                    ret = objVmax.ReporteZ();

                ret = objVmax.CerrarPuerto();
            }

            catch (Exception ex)
            {
                stringBuilder.Append("Por favor comunicarse con el Dpto de Sistemas y reportar el siguiente error: " + Environment.NewLine + string.Format("Error: {0}", ex.Message));
            }
        }


        private void ReportesElectronicos(string puerto, int respuesta)
        {

            try
            {
                VmaxComVe.VmaxComClass objVmax = new VmaxComVe.VmaxComClass();

                //1. Reporte Informativo
                //2. Reporte de memoria fiscal

                switch (respuesta)
                {

                    case 1:
                        datosfiscales(puerto);
                        Console.Clear();
                        break;

                    case 2:
                        memoriafiscal(puerto);
                        Console.Clear();
                        break;
                }
            }

            catch (Exception ex)
            {
                stringBuilder.Append("Por favor comunicarse con el Dpto de Sistemas y reportar el siguiente error: " + Environment.NewLine + string.Format("Error: {0}", ex.Message));
            }
        }

        private void datosfiscales(string puerto)
        {
            try
            {
                VmaxComVe.VmaxComClass objVmax = new VmaxComVe.VmaxComClass();
                uint ret = 0;
                ret = objVmax.AbrirPuerto(puerto);
                ret = objVmax.ObtenerReporteInformativo();
                ret = objVmax.AbrirDNF();
                ret = objVmax.TextoNoFiscal("Organismo de hacienda: " + objVmax.RetornoMI.sDescriptorOrganismoHacienda.ToString());
                ret = objVmax.TextoNoFiscal("Rif: " + objVmax.RetornoMI.sRif);
                ret = objVmax.TextoNoFiscal("Tasa 1: " + objVmax.RetornoMI.sTasa_1);
                ret = objVmax.TextoNoFiscal("Tasa 2: " + objVmax.RetornoMI.sTasa_2);
                ret = objVmax.TextoNoFiscal("Tasa 3: " + objVmax.RetornoMI.sTasa_3);
                ret = objVmax.TextoNoFiscal("Numero de decimales: " + objVmax.RetornoMI.sNumDecimales);
                ret = objVmax.TextoNoFiscal("Descripcion de la moneda: " + objVmax.RetornoMI.sDescMoneda);
                ret = objVmax.TextoNoFiscal("Metodo de impuesto: " + objVmax.RetornoMI.sMetodoImpuesto);
                ret = objVmax.TextoNoFiscal("Serial: " + objVmax.RetornoMI.sSerial);
                ret = objVmax.TextoNoFiscal("Fecha: " + objVmax.RetornoMI.sFecha);
                ret = objVmax.TextoNoFiscal("Hora: " + objVmax.RetornoMI.sHora);
                ret = objVmax.CerrarDNF();
                ret = objVmax.CerrarPuerto();
            }

            catch (Exception ex)
            {
                stringBuilder.Append("Por favor comunicarse con el Dpto de Sistemas y reportar el siguiente error: " + Environment.NewLine + string.Format("Error: {0}", ex.Message));
            }
        }


        private void memoriafiscal(string puerto)
        {
            try
            {
                VmaxComVe.VmaxComClass objVmax = new VmaxComVe.VmaxComClass();
                uint ret = 0;

                ret = objVmax.AbrirPuerto(puerto);

                ret = objVmax.ObtenerReporteMf("");
                ret = objVmax.AbrirDNF();
                ret = objVmax.TextoNoFiscal("Numero del ultimo Z: " + objVmax.RetornoMF.uiUltNumZ);
                ret = objVmax.TextoNoFiscal("Fecha y hora del ultimo Z: " + objVmax.RetornoMF.sFechaHoraUltNumZ);
                ret = objVmax.TextoNoFiscal("Venta exenta: " + objVmax.RetornoMF.uiTotVenta_E);
                ret = objVmax.TextoNoFiscal("Venta impuesto G: " + objVmax.RetornoMF.uiTotVenta_G);
                ret = objVmax.TextoNoFiscal("Venta impuesto R: " + objVmax.RetornoMF.uiTotVenta_R);
                ret = objVmax.TextoNoFiscal("Venta impuesto A: " + objVmax.RetornoMF.uiTotVenta_A);
                ret = objVmax.TextoNoFiscal("Devolucion exenta " + objVmax.RetornoMF.uiTotDev_E);
                ret = objVmax.TextoNoFiscal("Devolucion impuesto G: " + objVmax.RetornoMF.uiTotDev_G);
                ret = objVmax.TextoNoFiscal("Devolucion impuesto R: " + objVmax.RetornoMF.uiTotDev_R);
                ret = objVmax.TextoNoFiscal("Devolucion impuesto A: " + objVmax.RetornoMF.uiTotDev_A);
                ret = objVmax.TextoNoFiscal("Descuento exenta: " + objVmax.RetornoMF.uiTotDes_E);
                ret = objVmax.TextoNoFiscal("Descuento impuesto G: " + objVmax.RetornoMF.uiTotDes_G);
                ret = objVmax.TextoNoFiscal("Descuento impuesto R: " + objVmax.RetornoMF.uiTotDes_R);
                ret = objVmax.TextoNoFiscal("Descuento impuesto A: " + objVmax.RetornoMF.uiTotDes_A);
                ret = objVmax.TextoNoFiscal("Alicuota G: " + objVmax.RetornoMF.uiAlicuota_G);
                ret = objVmax.TextoNoFiscal("Alicuota R: " + objVmax.RetornoMF.uiAlicuota_R);
                ret = objVmax.TextoNoFiscal("Alicuota A: " + objVmax.RetornoMF.uiAlicuota_A);
                ret = objVmax.TextoNoFiscal("Facturas emitidas: " + objVmax.RetornoMF.uiTotalFacturasEmitidas);
                ret = objVmax.TextoNoFiscal("Fecha y hora de la ultima factura: " + objVmax.RetornoMF.sFechaHoraUltFactura);
                ret = objVmax.TextoNoFiscal("Facturas diarias: " + objVmax.RetornoMF.uiTotalFacturasDiarias);
                ret = objVmax.TextoNoFiscal("Notas de credito diarias: " + objVmax.RetornoMF.uiTotalNCDiarias);
                ret = objVmax.TextoNoFiscal("Numero de decimales: " + objVmax.RetornoMF.uiNumDecimales);
                ret = objVmax.TextoNoFiscal("Abreviatura de moneda: " + objVmax.RetornoMF.sAbreviacionMoneda);
                ret = objVmax.TextoNoFiscal("Serial de la impresora fiscal: " + objVmax.RetornoMF.sSerial);

                ret = objVmax.CerrarDNF();

            }

            catch (Exception ex)
            {
                stringBuilder.Append("Por favor comunicarse con el Dpto de Sistemas y reportar el siguiente error: " + Environment.NewLine + string.Format("Error: {0}", ex.Message));
            }
        }


        private void Factura(string puerto)
        {
            try
            {

                VmaxComVe.VmaxComClass objVmax = new VmaxComVe.VmaxComClass();

                uint ret;
                uint ret1;

                uint numfactura = 0;
                uint numultimafactcancelada = 0;
                string estado;

                ret = objVmax.AbrirPuerto(puerto);
                estado = objVmax.RetornoStatusImpresora.sStatus;

                //ret = objVmax.ObtenerEstado();
                // ret = objVmax.ObtenerEstadoImpresora();

                ret = objVmax.AbrirCF("MILECETH CARABALLO", "V010151960", "1", "294", "12345", "", "", 40);
                numfactura = objVmax.RetornoAbrirFactura.uiNumeroFactura;
                numultimafactcancelada = objVmax.RetornoAbrirFactura.uiUltNumeroCancelado;


                ret = objVmax.TextoNoFiscal("farma D 1000UT X 30 TAB 9075 E: 9.00 7591821102");
                ret = objVmax.Item("E: 116.00", "1000", "999999999999", "0", "1", 40);
                ret = objVmax.TextoNoFiscal("Paticas de Cochino f");
                ret = objVmax.Item("Servicio Exlusivo", "1000", "999999999999", "0", "1", 40);
                ret = objVmax.TextoNoFiscal("Huesos de Chocozuela");
                ret = objVmax.Item(" x K 12950000X1690", "1000", "999999999999", "0", "1", 40);
                ret = objVmax.TextoNoFiscal("Harina Maiz SANTA LU");
                ret = objVmax.Item("CIA  31080000X3000", "1000", "999999999999", "0", "1", 40);
                ret = objVmax.TextoNoFiscal("Arroz MARY Tradicion");
                ret = objVmax.Item("al 1 301920.00X1.000", "1000", "999999999999", "0", "1", 40);
                ret = objVmax.TextoNoFiscal("Bolsa Camiseta x Uni");
                ret = objVmax.Subtotal();
                ret = objVmax.PagoCF(objVmax.RetornoSubtotal.llSubtotal.ToString(), "Tarjeta de Debito             ", 1);
                ret1 = objVmax.Cerrar();

                ret = objVmax.CerrarPuerto();

                //VMAXOCX.VMAX objVmax = new VMAXOCX.VMAX();    
                //bool reyt = objVmax.LeeDatosFiscales();

                //bool resp = objVmax.AbrirCF("COMPRADOR", "J-309860895", "0", "", "", "", "");
                //resp = objVmax.Item("Venta", "1000", "443", "E", 1);
                //resp = objVmax.Item("Venta", "1000", "795", "G", 1);
                //resp = objVmax.SubTotal();
                //resp = objVmax.PagoCF("Efectivo", objVmax.TotalFactura, 1);
                //resp = objVmax.CerrarCF();

            }
            catch (Exception ex)
            {
                stringBuilder.Append("Por favor comunicarse con el Dpto de Sistemas y reportar el siguiente error: " + Environment.NewLine + string.Format("Error: {0}", ex.Message));
            }
        }

        private void Nota_De_Credito(string puerto)
        {
            try
            {

                VmaxComVe.VmaxComClass objVmax = new VmaxComVe.VmaxComClass();

                uint ret = 0;

                ret = objVmax.AbrirPuerto(puerto);
                ret = objVmax.AbrirCF("MILECETH CARABALLO", "V010151960", "2", "000000141", "KHA1700115", "27102020", "1432", 40);

                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "0", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "1", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "2", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "3", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "4", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "0", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "1", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "2", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "3", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "4", "1", 40); ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "0", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "1", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "2", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "3", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "4", "1", 40); ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "0", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "1", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "2", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "3", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "4", "1", 40); ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "0", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "1", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "2", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "3", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "4", "1", 40); ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "0", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "1", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "2", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "3", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "4", "1", 40); ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "0", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "1", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "2", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "3", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "4", "1", 40); ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "0", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "1", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "2", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "3", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "4", "1", 40); ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "0", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "1", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "2", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "3", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "4", "1", 40); ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "0", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "1", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "2", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "3", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "4", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "0", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "1", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "2", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "3", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "4", "1", 40); ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "0", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "1", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "2", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "3", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "4", "1", 40); ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "0", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "1", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "2", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "3", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "4", "1", 40); ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "0", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "1", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "2", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "3", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "4", "1", 40); ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "0", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "1", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "2", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "3", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "4", "1", 40); ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "0", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "1", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "2", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "3", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "4", "1", 40); ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "0", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "1", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "2", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "3", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "4", "1", 40); ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "0", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "1", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "2", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "3", "1", 40);
                ret = objVmax.ItemDev("Item de Prueba", "1000", "999999999999", "4", "1", 40);
                ret = objVmax.Subtotal();
                ret = objVmax.Cerrar();
                ret = objVmax.CerrarPuerto();

            }
            catch (Exception ex)
            {
                stringBuilder.Append("Por favor comunicarse con el Dpto de Sistemas y reportar el siguiente error: " + Environment.NewLine + string.Format("Error: {0}", ex.Message));
            }


        }


        public bool ImprimirNCFiscal(string SucursalActual, string NumeroFactura, string SerialImpresora, double Monto, string Motivo, string CodigoMotivoAnulacion)
        {
            VmaxComVe.VmaxComClass objVmax = new VmaxComVe.VmaxComClass();
            stringBuilder.Clear();
            uint resp = 0;
            string status = "";
            DataTable dtFacturas;
            DataTable dtCliente;
            bool StatusNoataCredito = true;
            string CedulaCliente = "";
            string NacionalidadCliente = "";
            string NunOrden = "";
            DateTime FechaOperacion = DateTime.Today;
            string PrimerNombre = "";
            string PrimerApellido = "";
            string SerialImpresoraNC = "";
            string FechaImpresora = "";
            string Prueba = _D_Inicio.Sucursal();

            try
            {

                //status = objVmax.RetornoStatusImpresora.sStatus;

                if (SucursalActual != "")
                {
                    dtFacturas = _D_DetalleOrden.BucarFactura(NumeroFactura, SerialImpresora, SucursalActual);

                    if (dtFacturas.Rows.Count > 0)
                    {
                        foreach (DataRow drItem in dtFacturas.Rows)
                        {
                            CedulaCliente = drItem["CTE_CedIdenPAG"].ToString();
                            NacionalidadCliente = drItem["CTE_NacioPAG"].ToString();
                            FechaOperacion = Convert.ToDateTime(TB_FACTURAS.Fact_FecCrea);
                            NunOrden = drItem["NumOrdServ"].ToString();
                            break;
                        }

                    }

                    dtCliente = _D_DetalleOrden.BucarTB_CTEPPAL(TB_FACTURAS.CTE_CedIdenPAG, TB_FACTURAS.CTE_NacioPAG);

                    if (dtCliente.Rows.Count > 0)
                    {
                        foreach (DataRow drItem in dtCliente.Rows)
                        {
                            PrimerNombre = drItem["CTE_PNombre"].ToString();
                            PrimerApellido = drItem["CTE_PApellido"].ToString();
                            break;
                        }

                    }

                    resp = objVmax.AbrirPuerto(Convert.ToString(glbPuertoCOM));

                    if (resp != 0)
                    {
                        objVmax.Cancelar();
                        objVmax.Cerrar();
                        objVmax.CerrarPuerto();
                        stringBuilder.Append(Environment.NewLine + "No hay conexión con la impresora fiscal");
                        _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "090", TB_USUARIO.COD_EMPLEADO, "No hay conexión con la impresora fiscal");
                        StatusNoataCredito = false;
                        return StatusNoataCredito;

                    }

                    else
                    {
                        //resp = objVmax.AbrirCF("Jesus Antonio Pabon Mavare", "J309860895", "2", "000000262", "TIU2202214", "26032022", "1055", 40);
                        resp = objVmax.AbrirCF(PrimerNombre + " " + PrimerApellido, Convert.ToString(TB_FACTURAS.CTE_NacioPAG) + "" + Convert.ToString(TB_FACTURAS.CTE_CedIdenPAG), "2", Convert.ToString(TB_FACTURAS.Fact_Num), Convert.ToString(TB_FACTURAS.Fact_SerialImpresora), Convert.ToDateTime(TB_FACTURAS.Fact_FecCrea).ToString("dd/MM/yyyy"), FechaOperacion.ToString("HH:mm"), 40);


                        if (resp != 0)
                        {
                            stringBuilder.Append(Environment.NewLine + "No hay conexión con la impresora fiscal");
                            _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "090", TB_USUARIO.COD_EMPLEADO, "No hay conexión con la impresora fiscal");
                            StatusNoataCredito = false;
                            objVmax.Cancelar();
                            objVmax.Cerrar();
                            objVmax.CerrarPuerto();
                            return StatusNoataCredito;
                        }

                        NumeroNCFiscal = (Convert.ToInt32(objVmax.RetornoMF.uiTotalNCDiarias) + 1).ToString();

                        if (resp == 0)
                        {

                            DataSet dsArti = _D_DetalleOrden.DetalleNotaCreditoFiscal(NumeroFactura, SerialImpresora);
                            string desart;

                            foreach (DataRow drItem in dsArti.Tables[0].Rows)
                            {
                                if (Convert.ToUInt32(drItem["Ordserv_Dto"].ToString()) > 0)
                                {
                                }

                                desart = drItem["CodArticulo"] + " " + drItem["DESART"];
                                //Valido que el texto no sea mayor a 40 caracteres para que no salga duplicado el articulo en la factura 25-05-2023
                                desart = Validar_Cadena(desart);

                                string Impuesto = "";
                                if (drItem["ART_EXENTO"].ToString() == "1")
                                {
                                    Impuesto = "0";
                                }
                                else
                                {
                                    Impuesto = "1";
                                }

                                resp = objVmax.ItemDev(desart, (Convert.ToDouble(drItem["Ordserv_Cant"]) * 1000).ToString(), drItem["OrdServ_Precio"].ToString(), Impuesto, "1", 40);

                            }

                            DataSet dsDcto = _D_DetalleOrden.DescuentosNotaCreditoFiscal(NunOrden);

                            if (resp == 0)
                            {
                                foreach (DataRow drItemdcto in dsDcto.Tables[0].Rows)
                                {

                                    resp = objVmax.DescuentoCF("Descuento", drItemdcto["DescuentoExento"].ToString(), drItemdcto["DescuentoGravable"].ToString(), "", "");

                                }

                            }
                            if (resp == 0)
                            {
                                resp = objVmax.Subtotal();
                                resp = objVmax.TextoNoFiscal("Monto Disponible:  " + Monto.ToString());
                                objVmax.ObtenerReporteInformativo();
                                objVmax.AbrirDNF();
                                SerialImpresoraNC = objVmax.RetornoMI.sSerial;
                                FechaImpresora = objVmax.RetornoMI.sFecha;
                                resp = objVmax.Cerrar();
                                resp = objVmax.CerrarPuerto();

                            }


                        }

                        if (resp == 0)
                        {
                            NumeroNCFiscal = objVmax.RetornoAbrirFactura.uiNumeroFactura.ToString();

                            switch (NumeroNCFiscal.Length)
                            {
                                case 7:
                                    {
                                        NumeroNCFiscal = NumeroNCFiscal;
                                        break;
                                    }

                                case 6:
                                    {
                                        NumeroNCFiscal = "0" + NumeroNCFiscal;
                                        break;
                                    }

                                case 5:
                                    {
                                        NumeroNCFiscal = "00" + NumeroNCFiscal;
                                        break;
                                    }

                                case 4:
                                    {
                                        NumeroNCFiscal = "000" + NumeroNCFiscal;
                                        break;
                                    }

                                case 3:
                                    {
                                        NumeroNCFiscal = "0000" + NumeroNCFiscal;
                                        break;
                                    }

                                case 2:
                                    {
                                        NumeroNCFiscal = "00000" + NumeroNCFiscal;
                                        break;
                                    }

                                case 1:
                                    {
                                        NumeroNCFiscal = "000000" + NumeroNCFiscal;
                                        break;
                                    }
                            }
                            string Transaccion = _D_DetalleOrden.RegistrarNCFISCAL(SucursalActual, NumeroNCFiscal, "005", TB_FACTURAS.Fact_Num, SerialImpresora, _D_Inicio.DiaActivo().ToString("yyyy/MM/dd"), SerialImpresoraNC, "", TB_FACTURAS.CTE_NacioPAG, TB_FACTURAS.CTE_CedIdenPAG, Motivo.ToUpper(), Convert.ToDouble(TB_FACTURAS.Fact_MontoGravable), Convert.ToDouble(TB_FACTURAS.Fact_Total), TB_USUARIO.COD_USR, "", Convert.ToDouble(TB_FACTURAS.Fact_IGTF), Convert.ToDouble(TB_FACTURAS.Fact_AlicuotaIGTF), Convert.ToDouble(TB_FACTURAS.Fact_MontoExento), CodigoMotivoAnulacion);

                            if (Transaccion == "SATISFACTORIO")
                            {
                                StatusNoataCredito = true;
                            }

                            else
                            {
                                StatusNoataCredito = false;
                                stringBuilder.Append("Por favor comunicarse con el Dpto de sistemas y reportar el siguiente error: " + Environment.NewLine + string.Format("Error: {0}", Transaccion));
                            }

                            return StatusNoataCredito;
                        }


                    }

                }


                stringBuilder.Append(Environment.NewLine + "El numero de sucursal no tiene un valor valido, Verifique");
                return false;

            }

            catch (Exception ex)
            {
                stringBuilder.Append("Por favor comunicarse con el Dpto de sistemas y reportar el siguiente error: " + Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return false;
            }
        }





        public bool VerficarConexionImpresoraFiscal()
        {
            //VmaxComVe.VmaxComClass objVmax = new VmaxComVe.VmaxComClass();
            stringBuilder.Clear();
            uint resp = 0;
            bool Conexion = false;
            string status;

           
            try
            {
                VmaxComVe.VmaxComClass objVmax = new VmaxComVe.VmaxComClass();
                uint ret = 0;

                ret = objVmax.AbrirPuerto(Convert.ToString(glbPuertoCOM));
                ret = objVmax.ObtenerEstadoImpresora();
               
                if (ret != 16 && ret != 0)
                {
                    //resp = objVmax.AbrirCF("", "", "1", "1", "12345", "", "", 40);
                    //objVmax.Cancelar();
                    //objVmax.Cerrar();
                    objVmax.CerrarPuerto();
                    stringBuilder.Append(Environment.NewLine + "No hay conexión con la impresora fiscal ");
                    Conexion = false;
                    _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "090", TB_USUARIO.COD_EMPLEADO, "No hay conexión con la impresora fiscal");


                }

                else
                {
                    Conexion = true;
                    objVmax.Cancelar();
                    objVmax.Cerrar();
                    objVmax.CerrarPuerto();
                }

                return Conexion;
            }

            catch (Exception ex)
            {
                stringBuilder.Append("Por favor comunicarse con el Dpto de sistemas y reportar el siguiente error: " + Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return false;
            }
        }

        public EstadoImpresora ObtenerEstadoCompleto()
        {
            VmaxComVe.VmaxComClass _printer = new VmaxComVe.VmaxComClass();
            var estado = new EstadoImpresora();

            try
            {
                uint estadoMecanico = _printer.ObtenerEstadoImpresora();

                // Interpretar estados - VERIFICAR ESTAS MÁSCARAS CON EL MANUAL DE LA IMPRESORA
                estado.Online = (estadoMecanico & 0x01) == 0;        // Bit 0
                estado.TapaAbierta = (estadoMecanico & 0x02) != 0;   // Bit 1 - CORREGIDO
                estado.PocoPapel = (estadoMecanico & 0x04) != 0;     // Bit 2
                estado.SinPapel = (estadoMecanico & 0x08) != 0;      // Bit 3
                estado.TemperaturaAlta = (estadoMecanico & 0x10) != 0; // Bit 4
                estado.ErrorGeneral = (estadoMecanico & 0x20) != 0;  // Bit 5
                estado.ErrorCortadora = (estadoMecanico & 0x40) != 0; // Bit 6
                estado.BufferOverflow = (estadoMecanico & 0x80) != 0; // Bit 7

                // Estado del papel basado en múltiples bits
                estado.TienePapel = !estado.SinPapel;

                // Para debugging - agrega esto temporalmente
                Console.WriteLine($"Estado mecánico (hex): 0x{estadoMecanico:X2}");
                Console.WriteLine($"Estado mecánico (bin): {Convert.ToString(estadoMecanico, 2).PadLeft(8, '0')}");
                Console.WriteLine($"Tapa abierta: {estado.TapaAbierta}");

                return estado;
            }
            finally
            {
                _printer.CerrarPuerto();
            }
        }

        private static bool InterpretarEstadoPapel(uint estado)
        {
            // Bits 6-10: 0 = CON PAPEL, 1 = SIN PAPEL
            bool sinPapelBit6 = (estado & (1 << 6)) != 0;
            bool sinPapelBit7 = (estado & (1 << 7)) != 0;
            bool sinPapelBit8 = (estado & (1 << 8)) != 0;
            bool sinPapelBit9 = (estado & (1 << 9)) != 0;
            bool sinPapelBit10 = (estado & (1 << 10)) != 0;

            // Si alguno de los bits 6-10 está en 1, NO hay papel
            return !(sinPapelBit6 || sinPapelBit7 || sinPapelBit8 || sinPapelBit9 || sinPapelBit10);
        }

        // Opción 2: recibe la máscara cruda de 16 bits
        public static (bool SinPapel, bool TapaAbierta) PapelOTapa(ushort mask)
        {
            bool tapaAbierta = (mask & (1 << 1)) != 0;

            bool sinPapel =
                   ((mask & (1 << 6)) != 0)   // Fin de papel
                || ((mask & (1 << 7)) != 0)   // Ausencia de papel
                || ((mask & (1 << 8)) != 0)   // TOF sin papel
                || ((mask & (1 << 9)) != 0)   // COF sin papel
                || ((mask & (1 << 10)) != 0);  // BOF sin papel

            bool finDePapel = (mask & (1 << 6)) != 0;  // Se consumió el papel
            bool ausenciaDePapel = (mask & (1 << 7)) != 0;  // No hay papel cargado
            bool tofSinPapel = (mask & (1 << 8)) != 0;  // Top Of Form sin papel
            bool cofSinPapel = (mask & (1 << 9)) != 0;  // Center Of Form sin papel
            bool bofSinPapel = (mask & (1 << 10)) != 0;  // Bottom Of Form sin papel

            return (sinPapel, tapaAbierta);
        }

        // Devuelve los 4 dígitos HEX del estado del mecanismo de impresión.
        public static string LeerEstadoHex(VmaxComVe.VmaxComClass vx)
        {
            uint rc = vx.ObtenerEstadoImpresora();
            if (rc != 0) throw new InvalidOperationException($"ObtenerEstadoImpresora() retornó {rc}");

            // Ruta 1: estructura de retorno típica "pRetornoEstadoImpresora.sEstatusMecanismo"
            var t = vx.GetType();
            var ret = t.GetProperty("pRetornoEstadoImpresora")?.GetValue(vx);
            if (ret != null)
            {
                var hex = ret.GetType().GetProperty("sEstatusMecanismo")?.GetValue(ret) as string;
                if (!string.IsNullOrWhiteSpace(hex)) return hex.Trim();
            }

            // Ruta 2: propiedad plana según versión de la DLL.
            var hexPlano =
                  t.GetProperty("sEstatusMecanismoImpresion")?.GetValue(vx) as string
               ?? t.GetProperty("sEstatusImpresoraHex")?.GetValue(vx) as string;
            if (!string.IsNullOrWhiteSpace(hexPlano)) return hexPlano.Trim();

            throw new MissingMemberException("No se encontró la propiedad con el estado HEX (4). Verifica el Interop generado.");
        }

        public static (bool SinPapel, bool TapaAbierta) PapelOTapa2(string estadoHex4)
        {
            if (string.IsNullOrWhiteSpace(estadoHex4))
                throw new ArgumentException("estadoHex4 vacío.", nameof(estadoHex4));

            ushort mask = Convert.ToUInt16(estadoHex4.Trim(), 16);
            return PapelOTapa(mask);
        }

        public static string LeerEstadoHexRobusto(VmaxComVe.VmaxComClass vx)
        {
            // Debe llamarse después de vx.ObtenerEstadoImpresora()
            // 1) Buscar en propiedades de nivel 1 del COM
            var hex = BuscarHexEnObjeto(vx, priorizarPorNombre: true);
            if (hex != null) return hex;

            // 2) Buscar en objetos de retorno tipo "pRetorno*"
            foreach (var p in vx.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!p.Name.StartsWith("pRetorno", StringComparison.OrdinalIgnoreCase)) continue;
                var ret = p.GetValue(vx);
                var h = BuscarHexEnObjeto(ret, priorizarPorNombre: true);
                if (h != null) return h;
            }

            // 3) Buscar en campos (algunos interop exponen fields, no properties)
            foreach (var f in vx.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                var h = ConvertirAHex4(f.GetValue(vx), f.Name);
                if (h != null) return h;
            }

            throw new MissingMemberException("No se encontró el H(4) del estado del mecanismo en el interop.");
        }

        static string BuscarHexEnObjeto(object obj, bool priorizarPorNombre)
        {
            if (obj == null) return null;

            string candidato = null;
            foreach (var p in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var val = p.GetValue(obj);
                var h = ConvertirAHex4(val, p.Name);
                if (h == null) continue;

                // Nombres prioritarios expandidos
                if (priorizarPorNombre && EsNombrePrioritario(p.Name))
                    return h;

                // Reemplazo de: candidato ??= h;
                if (candidato == null)
                {
                    candidato = h;
                }
            }
            return candidato;
        }

        static bool EsNombrePrioritario(string nombre)
        {
            var nombresPrioritarios = new[] {
        "Estatus", "Status", "Estado", "State",
        "Mecan", "Mechanism", "Mecanismo",
        "Hex", "Hexadecimal", "H", "Codigo", "Code"
    };

            return nombresPrioritarios.Any(n =>
                nombre.IndexOf(n, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        //static string ConvertirAHex4(object val, string name)
        //{
        //    switch (val)
        //    {
        //        case null: return null;
        //        case string s when Hex4.IsMatch(s.Trim()): return s.Trim().ToUpperInvariant();
        //        case ushort u: return u.ToString("X4");
        //        case short si: return unchecked((ushort)si).ToString("X4");
        //        case int i: return unchecked((ushort)i).ToString("X4");
        //        case byte[] ba when ba.Length >= 2:
        //            return $"{ba[0]:X2}{ba[1]:X2}";
        //        default: return null;
        //    }
        //}

        public static void ExplorarObjetoManual(VmaxComVe.VmaxComClass vx)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("=== EXPLORACIÓN MANUAL DEL OBJETO VmaxComClass ===");

            // Explorar propiedades
            sb.AppendLine("\n--- PROPIEDADES ---");
            foreach (var prop in vx.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                try
                {
                    var valor = prop.GetValue(vx);
                    sb.AppendLine($"Prop: {prop.Name} = {valor} (Tipo: {valor?.GetType().Name ?? "null"})");
                }
                catch (Exception ex)
                {
                    sb.AppendLine($"Prop: {prop.Name} -> ERROR: {ex.Message}");
                }
            }

            // Explorar campos
            sb.AppendLine("\n--- CAMPOS ---");
            foreach (var field in vx.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                try
                {
                    var valor = field.GetValue(vx);
                    sb.AppendLine($"Field: {field.Name} = {valor} (Tipo: {valor?.GetType().Name ?? "null"})");
                }
                catch (Exception ex)
                {
                    sb.AppendLine($"Field: {field.Name} -> ERROR: {ex.Message}");
                }
            }

            // Explorar métodos
            sb.AppendLine("\n--- MÉTODOS DISPONIBLES ---");
            foreach (var method in vx.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                if (method.DeclaringType == vx.GetType()) // Solo métodos de esta clase
                {
                    sb.AppendLine($"Method: {method.Name}");
                }
            }

            //string archivo = @"C:\Prueba\exploracion.txt";
            //using (StreamWriter sw = new StreamWriter(archivo))
            //{
            //    sw.Write(sb.ToString());
            //}
        }

        public static void ExploracionRapidaEstructuras(VmaxComVe.VmaxComClass vx)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("EXPLORACIÓN RÁPIDA DE ESTRUCTURAS - " + DateTime.Now);

            // Explorar específicamente RetornoObtenerEstado
            try
            {
                var retornoEstado = vx.RetornoObtenerEstado;
                sb.AppendLine("\n=== RetornoObtenerEstado ===");
                foreach (var prop in retornoEstado.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    var valor = prop.GetValue(retornoEstado);
                    sb.AppendLine($"{prop.Name} = {valor} (Tipo: {valor?.GetType().Name ?? "null"})");

                    // Verificar si es hexadecimal
                    var hex = ConvertirAHex4(valor, prop.Name);
                    if (hex != null)
                    {
                        sb.AppendLine($"  → HEX: {hex}");
                    }
                }

                foreach (var field in retornoEstado.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
                {
                    var valor = field.GetValue(retornoEstado);
                    sb.AppendLine($"{field.Name} = {valor} (Tipo: {valor?.GetType().Name ?? "null"})");

                    var hex = ConvertirAHex4(valor, field.Name);
                    if (hex != null)
                    {
                        sb.AppendLine($"  → HEX: {hex}");
                    }
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine($"ERROR en RetornoObtenerEstado: {ex.Message}");
            }

            // Explorar específicamente RetornoStatusImpresora
            try
            {
                var retornoStatus = vx.RetornoStatusImpresora;
                sb.AppendLine("\n=== RetornoStatusImpresora ===");
                foreach (var prop in retornoStatus.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    var valor = prop.GetValue(retornoStatus);
                    sb.AppendLine($"{prop.Name} = {valor} (Tipo: {valor?.GetType().Name ?? "null"})");

                    var hex = ConvertirAHex4(valor, prop.Name);
                    if (hex != null)
                    {
                        sb.AppendLine($"  → HEX: {hex}");
                    }
                }

                foreach (var field in retornoStatus.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
                {
                    var valor = field.GetValue(retornoStatus);
                    sb.AppendLine($"{field.Name} = {valor} (Tipo: {valor?.GetType().Name ?? "null"})");

                    var hex = ConvertirAHex4(valor, field.Name);
                    if (hex != null)
                    {
                        sb.AppendLine($"  → HEX: {hex}");
                    }
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine($"ERROR en RetornoStatusImpresora: {ex.Message}");
            }

            // Guardar y abrir
            //string archivo = @"C:\Prueba\exploracion.txt";
            //using (StreamWriter sw = new StreamWriter(archivo))
            //{
            //    sw.Write(sb.ToString());
            //}
        }

        public static void ExploracionCompletaEstructuras(VmaxComVe.VmaxComClass vx)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("EXPLORACIÓN COMPLETA DE ESTRUCTURAS - " + DateTime.Now);

            // Explorar RetornoObtenerEstado
            try
            {
                sb.AppendLine("\n" + new string('=', 50));
                sb.AppendLine("RETORNO OBTENER ESTADO - COMPLETO");
                sb.AppendLine(new string('=', 50));

                var estado = vx.RetornoObtenerEstado;
                ExplorarEstructuraCompleta(estado, "RetornoObtenerEstado", sb);
            }
            catch (Exception ex)
            {
                sb.AppendLine($"ERROR en RetornoObtenerEstado: {ex.Message}");
                sb.AppendLine($"Stack Trace: {ex.StackTrace}");
            }

            // Explorar RetornoStatusImpresora
            try
            {
                sb.AppendLine("\n" + new string('=', 50));
                sb.AppendLine("RETORNO STATUS IMPRESORA - COMPLETO");
                sb.AppendLine(new string('=', 50));

                var status = vx.RetornoStatusImpresora;
                ExplorarEstructuraCompleta(status, "RetornoStatusImpresora", sb);
            }
            catch (Exception ex)
            {
                sb.AppendLine($"ERROR en RetornoStatusImpresora: {ex.Message}");
            }

            // Guardar en archivo
            GuardarYMostrarResultado(sb, "vmax_estructuras_completas.txt");
        }

        static void ExplorarEstructuraCompleta(object structObj, string nombre, StringBuilder sb)
        {
            if (structObj == null)
            {
                sb.AppendLine($"{nombre} es null");
                return;
            }

            sb.AppendLine($"Tipo: {structObj.GetType().FullName}");
            sb.AppendLine(new string('-', 40));

            int contador = 0;

            // Propiedades PRIMERO
            var propiedades = structObj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            sb.AppendLine($"PROPIEDADES ({propiedades.Length}):");

            foreach (var prop in propiedades)
            {
                try
                {
                    var valor = prop.GetValue(structObj);
                    sb.AppendLine($"  [{contador++:00}] {prop.Name} = {ObtenerValorFormateado(valor)}");

                    // Verificar si podría ser hexadecimal
                    var hex = ConvertirAHex4(valor, prop.Name);
                    if (hex != null)
                    {
                        sb.AppendLine($"        → HEX: {hex} ← POSIBLE CANDIDATO");
                    }
                }
                catch (Exception ex)
                {
                    sb.AppendLine($"  [{contador++:00}] {prop.Name} = ERROR: {ex.Message}");
                }
            }

            // Campos LUEGO
            contador = 0;
            var campos = structObj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            sb.AppendLine($"\nCAMPOS ({campos.Length}):");

            foreach (var field in campos)
            {
                try
                {
                    var valor = field.GetValue(structObj);
                    sb.AppendLine($"  [{contador++:00}] {field.Name} = {ObtenerValorFormateado(valor)}");

                    var hex = ConvertirAHex4(valor, field.Name);
                    if (hex != null)
                    {
                        sb.AppendLine($"        → HEX: {hex} ← POSIBLE CANDIDATO");
                    }
                }
                catch (Exception ex)
                {
                    sb.AppendLine($"  [{contador++:00}] {field.Name} = ERROR: {ex.Message}");
                }
            }
        }

        static string ObtenerValorFormateado(object valor)
        {
            if (valor == null) return "null";

            if (valor is byte[] array)
            {
                return $"byte[{array.Length}] = {BitConverter.ToString(array).Replace("-", " ")}";
            }

            return $"{valor} (Tipo: {valor.GetType().Name})";
        }

        static void GuardarYMostrarResultado(StringBuilder sb, string nombreArchivo)
        {
            string carpeta = @"C:\Prueba";
            Directory.CreateDirectory(carpeta); // Crear si no existe
            string archivo = Path.Combine(carpeta, "vmax_estructuras.txt");
            File.WriteAllText(archivo, sb.ToString());
        }

        public static string LeerEstadoHexRobusto2(VmaxComVe.VmaxComClass vx)
        {
            // Versión directa - accede al campo que sabemos que existe
            try
            {
                string estadoHex = vx.RetornoStatusImpresora.sStatus?.Trim();

                if (!string.IsNullOrEmpty(estadoHex))
                {
                    // Verificar formato hexadecimal de 4 caracteres
                    if (System.Text.RegularExpressions.Regex.IsMatch(estadoHex, @"^[0-9A-Fa-f]{4}$"))
                    {
                        Console.WriteLine($"✅ Estado hexadecimal encontrado: {estadoHex}");
                        return estadoHex.ToUpperInvariant();
                    }
                    else
                    {
                        // Intentar convertir si no está en formato directo
                        var hex = ConvertirAHex4(estadoHex, "sStatus");
                        if (hex != null)
                        {
                            Console.WriteLine($"✅ Estado hexadecimal convertido: {hex}");
                            return hex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error accediendo a RetornoStatusImpresora.sStatus: {ex.Message}");
                // Continuar con la búsqueda robusta
            }

            // Si llegamos aquí, usar la búsqueda robusta original como respaldo
            Console.WriteLine("🔍 Usando búsqueda robusta como respaldo...");

            // Tu código original de búsqueda robusta aquí
            try
            {
                var hex = BuscarHexEnObjeto(vx, priorizarPorNombre: true);
                if (hex != null)
                {
                    Console.WriteLine($"✅ Estado encontrado en búsqueda robusta: {hex}");
                    return hex;
                }

                foreach (var p in vx.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (!p.Name.StartsWith("pRetorno", StringComparison.OrdinalIgnoreCase)) continue;
                    var ret = p.GetValue(vx);
                    var h = BuscarHexEnObjeto(ret, priorizarPorNombre: true);
                    if (h != null)
                    {
                        Console.WriteLine($"✅ Estado encontrado en {p.Name}: {h}");
                        return h;
                    }
                }

                foreach (var f in vx.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
                {
                    var h = ConvertirAHex4(f.GetValue(vx), f.Name);
                    if (h != null)
                    {
                        Console.WriteLine($"✅ Estado encontrado en campo {f.Name}: {h}");
                        return h;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error en búsqueda robusta: {ex.Message}");
            }

            throw new MissingMemberException("No se encontró el H(4) del estado del mecanismo en el interop.");
        }

        private static string ConvertirAHex4(object val, string name)
        {
            try
            {
                switch (val)
                {
                    case null: return null;
                    case string s when System.Text.RegularExpressions.Regex.IsMatch(s.Trim(), @"^[0-9A-Fa-f]{4}$"):
                        return s.Trim().ToUpperInvariant();
                    case ushort u: return u.ToString("X4");
                    case short si: return ((ushort)si).ToString("X4");
                    case int i when i >= 0 && i <= 0xFFFF: return ((ushort)i).ToString("X4");
                    case uint ui when ui <= 0xFFFF: return ((ushort)ui).ToString("X4");
                    case byte[] ba when ba.Length >= 2:
                        return $"{ba[0]:X2}{ba[1]:X2}";
                    case byte b: return b.ToString("X2") + "00";
                    default:
                        if (ushort.TryParse(val.ToString(), out ushort result))
                            return result.ToString("X4");
                        return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public static class EstadoImpresoraVmax
        {
            // Máscaras de bits según el manual Vmax
            public const ushort ONLINE = 0x0001;           // Bit 0
            public const ushort TAPA_ABIERTA = 0x0002;     // Bit 1
            public const ushort TEMPERATURA_ALTA = 0x0004; // Bit 2
            public const ushort ERROR_GRAVE = 0x0008;      // Bit 3
            public const ushort ERROR_CORTADORA = 0x0010;  // Bit 4
            public const ushort BUFFER_OVERFLOW = 0x0020;  // Bit 5
            public const ushort SIN_PAPEL_FIN = 0x0040;    // Bit 6
            public const ushort SIN_PAPEL_AUSENCIA = 0x0080; // Bit 7
            public const ushort SIN_PAPEL_TOF = 0x0100;    // Bit 8
            public const ushort SIN_PAPEL_COF = 0x0200;    // Bit 9
            public const ushort SIN_PAPEL_BOF = 0x0400;    // Bit 10

            /// <summary>
            /// Verifica si la impresora tiene papel según el estado hexadecimal
            /// </summary>
            public static bool TienePapel(string estadoHex)
            {
                try
                {
                    ushort estado = Convert.ToUInt16(estadoHex, 16);

                    // Según el manual, si CUALQUIERA de estos bits está en 1, significa SIN PAPEL
                    bool sinPapel = (estado & (SIN_PAPEL_FIN | SIN_PAPEL_AUSENCIA | SIN_PAPEL_TOF | SIN_PAPEL_COF | SIN_PAPEL_BOF)) != 0;

                    return !sinPapel; // Retorna true si TIENE papel
                }
                catch
                {
                    return false; // En caso de error, asumimos que no hay papel por seguridad
                }
            }

            /// <summary>
            /// Verifica si la tapa está abierta según el estado hexadecimal
            /// </summary>
            public static bool TapaAbierta(string estadoHex)
            {
                try
                {
                    ushort estado = Convert.ToUInt16(estadoHex, 16);
                    return (estado & TAPA_ABIERTA) != 0;
                }
                catch
                {
                    return true; // En caso de error, asumimos tapa abierta por seguridad
                }
            }

            /// <summary>
            /// Verifica si la impresora está online/lista
            /// </summary>
            public static bool EstaOnline(string estadoHex)
            {
                try
                {
                    ushort estado = Convert.ToUInt16(estadoHex, 16);
                    return (estado & ONLINE) != 0;
                }
                catch
                {
                    return false;
                }
            }

            /// <summary>
            /// Verifica si hay errores graves
            /// </summary>
            public static bool TieneErroresGraves(string estadoHex)
            {
                try
                {
                    ushort estado = Convert.ToUInt16(estadoHex, 16);
                    return (estado & (ERROR_GRAVE | ERROR_CORTADORA | BUFFER_OVERFLOW | TEMPERATURA_ALTA)) != 0;
                }
                catch
                {
                    return true; // En caso de error, asumimos que hay problemas
                }
            }

            /// <summary>
            /// Análisis completo del estado de la impresora
            /// </summary>
            public static void AnalizarEstadoCompleto(string estadoHex)
            {
                Console.WriteLine($"🔍 ANÁLISIS ESTADO VMAX: {estadoHex}");

                try
                {
                    ushort estado = Convert.ToUInt16(estadoHex, 16);
                    string binario = Convert.ToString(estado, 2).PadLeft(16, '0');

                    Console.WriteLine($"Binario: {binario}");
                    Console.WriteLine($"Decimal: {estado}");

                    Console.WriteLine("\n📊 ESTADO ACTUAL:");

                    // Estado principal
                    if (EstaOnline(estadoHex))
                        Console.WriteLine("✅ EN LÍNEA - Impresora conectada");
                    else
                        Console.WriteLine("❌ FUERA DE LÍNEA - Impresora desconectada");

                    // Tapa
                    if (TapaAbierta(estadoHex))
                        Console.WriteLine("❌ TAPA ABIERTA - Cierre la tapa de la impresora");
                    else
                        Console.WriteLine("✅ TAPA CERRADA - Correctamente cerrada");

                    // Papel
                    if (TienePapel(estadoHex))
                        Console.WriteLine("✅ CON PAPEL - Hay papel disponible");
                    else
                        Console.WriteLine("❌ SIN PAPEL - Inserte papel en la impresora");

                    // Errores
                    if (TieneErroresGraves(estadoHex))
                    {
                        Console.WriteLine("🚨 ERRORES DETECTADOS:");
                        if ((estado & TEMPERATURA_ALTA) != 0) Console.WriteLine("   • Temperatura del cabezal ALTA");
                        if ((estado & ERROR_GRAVE) != 0) Console.WriteLine("   • Error no recuperable");
                        if ((estado & ERROR_CORTADORA) != 0) Console.WriteLine("   • Error en cortadora de papel");
                        if ((estado & BUFFER_OVERFLOW) != 0) Console.WriteLine("   • Buffer overflow");
                    }
                    else
                    {
                        Console.WriteLine("✅ SIN ERRORES GRAVES");
                    }

                    Console.WriteLine("\n💡 RECOMENDACIONES:");
                    if (!TienePapel(estadoHex))
                        Console.WriteLine("   • Inserte papel en la impresora");
                    if (TapaAbierta(estadoHex))
                        Console.WriteLine("   • Cierre la tapa de la impresora");
                    if (TieneErroresGraves(estadoHex))
                        Console.WriteLine("   • Reinicie la impresora o contacte soporte");
                    if (EstaOnline(estadoHex) && !TapaAbierta(estadoHex) && TienePapel(estadoHex) && !TieneErroresGraves(estadoHex))
                        Console.WriteLine("   • Impresora lista para usar");

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error analizando estado: {ex.Message}");
                }
            }
        }

        public bool VerficarConexionImpresoraFiscalSinCerrar()
        {
            //VmaxComVe.VmaxComClass objVmax = new VmaxComVe.VmaxComClass();
            stringBuilder.Clear();
            uint resp = 0;
            bool Conexion = false;
            string status;


            try
            {
                VmaxComVe.VmaxComClass objVmax = new VmaxComVe.VmaxComClass();
                uint ret = 0;
                uint Prueba = 0;
                ret = objVmax.AbrirPuerto(Convert.ToString(glbPuertoCOM));
                ret = objVmax.ObtenerEstadoImpresora();

                ////ExplorarObjetoManual(objVmax);
                ////ExploracionCompletaEstructuras(objVmax);

                //string estadoHex = ret.ToString("X4");

                ////// Análisis completo
                ////EstadoImpresoraVmax.AnalizarEstadoCompleto(estadoHex);

                ////// Verificaciones rápidas para lógica de programa
                ////Console.WriteLine("\n✅ VERIFICACIONES RÁPIDAS:");

                //bool tienePapel = EstadoImpresoraVmax.TienePapel(estadoHex);
                //bool tapaAbierta = EstadoImpresoraVmax.TapaAbierta(estadoHex);
                //bool estaOnline = EstadoImpresoraVmax.EstaOnline(estadoHex);
                //bool tieneErrores = EstadoImpresoraVmax.TieneErroresGraves(estadoHex);

                //Console.WriteLine($"Papel: {(tienePapel ? "✅ DISPONIBLE" : "❌ FALTANTE")}");
                //Console.WriteLine($"Tapa: {(tapaAbierta ? "❌ ABIERTA" : "✅ CERRADA")}");
                //Console.WriteLine($"Conexión: {(estaOnline ? "✅ EN LÍNEA" : "❌ FUERA DE LÍNEA")}");
                //Console.WriteLine($"Errores: {(tieneErrores ? "❌ PRESENTES" : "✅ NINGUNO")}");


                //ushort mask = Convert.ToUInt16(hex4, 16);

                //// Bits de “sin papel” y “tapa” según tabla del manual
                //bool tapaAbierta = (mask & (1 << 1)) != 0;
                //bool sinPapel = ((mask & (1 << 6)) != 0)
                //             || ((mask & (1 << 7)) != 0)
                //             || ((mask & (1 << 8)) != 0)
                //             || ((mask & (1 << 9)) != 0)
                //             || ((mask & (1 << 10)) != 0);


                //var estados = ObtenerEstadoCompleto();
                //string estadoHex4 = LeerEstadoHex(objVmax);
                //var (sinPapel, tapaAbierta) = PapelOTapa((ushort)ret);
                if (ret != 16 && ret != 0)
                {
                    //resp = objVmax.AbrirCF("", "", "1", "1", "12345", "", "", 40);
                    //objVmax.Cancelar();
                    //objVmax.Cerrar();
                    objVmax.CerrarPuerto();
                    stringBuilder.Append(Environment.NewLine + "No hay conexión con la impresora fiscal");
                    Conexion = false;
                    _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "090", TB_USUARIO.COD_EMPLEADO, "No hay conexión con la impresora fiscal");


                }

                else
                {
                    Conexion = true;
                    //objVmax.Cancelar();
                    //objVmax.Cerrar();
                    //objVmax.CerrarPuerto();
                }

                return Conexion;
            }

            catch (Exception ex)
            {
                stringBuilder.Append("Por favor comunicarse con el Dpto de sistemas y reportar el siguiente error: " + Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return false;
            }

        }

        public string Validar_Cadena(string _cadena)
        {
            int MaxLength = 39;
            string Cadena = "";

            if (_cadena.Length > MaxLength)
            {
                Cadena = _cadena.Substring(0, MaxLength);
            }

            else
            {
                Cadena = _cadena;
            }

            return Cadena;
        }

        //Funcion Imrprimir Factura en Epos 

        //public bool ImprimirFacturaFiscal(string strNumFactura, string CedulaCliente, string NacionalidadRif, ref SqlClient.SqlCommand sqlCom)
        //{
        //    try
        //    {
        //        DataSet DtIGTF;
        //        NumeroComprobanteFiscal = "";
        //        DataSet dsArti = ManBD.ExecutaSqlDataSet("exec SP_DETALLEFACTURAFISCAL '" + NumeroOrdenImprimir + "'", "Prueba", sqlCom);

        //        DataTable dt = dsArti.Tables(0);
        //        int x = 0;

        //        string totaldetalle = dsArti.Tables(0).Rows.Count;

        //        DataRow drItem;
        //        double TotalItems;
        //        bool DctoFactura;
        //        string TipoTasaIVA;

        //        foreach (var drItem in dt.Rows)
        //        {
        //            if (drItem("OrdServ_PorcImp") > 0)
        //            {
        //                if (drItem("OrdServ_PorcImp") == PorcIvaTipoA)
        //                {
        //                    TipoTasaIVA = "A";
        //                    break;
        //                }
        //                else if (drItem("OrdServ_PorcImp") == PorcIvaTipoR)
        //                {
        //                    TipoTasaIVA = "R";
        //                    break;
        //                }
        //                else
        //                {
        //                    TipoTasaIVA = "G";
        //                    break;
        //                }
        //            }
        //            else
        //                TipoTasaIVA = "G";
        //        }

        //        // Muestro la dirección que esta guardada en la tabla Cliente para verificar si esta o no correcta
        //        Cliente.ObtenerDatos(CedulaCliente, NacionalidadRif, sqlCom);

        //        // ----CONSULTO PAGOS EN DIVISA
        //        DtIGTF = ManBD.EjecutaStoreProcedure("pGetPagosConIGTF", glbSucursalActual + "', '" + NroOrdenServicio + "', '" + 0 + "", sqlCom);
        //        // If DtIGTF.Tables(0).Rows(0)("Abo_Monto") > 0 Then


        //        // End If

        //        DataSet dsFacturaManual = ManBD.EjecutaStoreProcedure("pGetFacturaManual", NroOrdenServicio + "','" + glbSucursalActual + "','" + CodBancoFactManual, Interaction.Command);

        //        if ((dsFacturaManual.Tables(0).Rows.Count == 0) & (dsFacturaManual.Tables(1).Rows.Count == 0) & ((DtIGTF.Tables(0).Rows(0)("Abo_Monto") > 0 & DtIGTF.Tables(0).Rows(0)("FacturaManualIGTF") == 0) | DtIGTF.Tables(0).Rows(0)("Abo_Monto") == 0))
        //        {
        //            FacturaManual = false;

        //            ImpresorasFiscales.Vmax Vmax = new ImpresorasFiscales.Vmax();
        //            ImprimirFacturaFiscal = false;

        //            // Dim Status As New FrmConfigurarImpFiscal
        //            // If Status.ValidaFechaImpresora = False Then
        //            // Exit Function
        //            // End If

        //            if (Vmax.EstadoFiscal == 0)
        //            {
        //                Barra.Visible = true;
        //                Barra.InhabilitarCursor();
        //                Barra.AvanceBarraProgreso();

        //                // ***************************
        //                // IMPRESORA FISCAL

        //                Barra.AvanceBarraProgreso();
        //                frmPrepararImpresora PrepararImpres = new frmPrepararImpresora();
        //                PrepararImpres.ShowDialog();
        //                bool resp;
        //                // abro el CF. 

        //                // AxVMAX3.AbrirPuerto()
        //                Vmax.AbrirPuerto(glbPuertoCOM);

        //                // ''''' ********* DATOS DEL CLIENTE ************
        //                resp = Vmax.AbrirCF(Cliente.NombreCompleto, Cliente.Nacionalidad + "-" + Cliente.Cedula, "F", "", "", "", "");
        //                // resp = Vmax.AbrirCF("", Cliente.Nacionalidad & "-" & Cliente.Cedula, "F", "", "", "", "")

        //                /// **********'IMPRIMO LOS ITEMS *********************
        //                // MsgBox("paso abrir comprobante fiscal")
        //                // x = 0
        //                string desart;

        //                foreach (var drItem in dt.Rows)
        //                {
        //                    // EL DESCUENTO DE LOS ARTICULOS SE ENVIARA AL FINAL, ANTES DE CERRAR EL CF

        //                    if (drItem.Item("Ordserv_Dto") > 0)
        //                        DctoFactura = true;

        //                    desart = drItem.Item("CodArticulo") + " " + drItem.Item("DESART");

        //                    resp = Vmax.Item(drItem.Item("CodArticulo") + " " + drItem.Item("DESART"), drItem.Item("Ordserv_Cant") * 1000, drItem("OrdServ_Precio"), IIf(drItem.Item("ART_EXENTO") == 1, "E", TipoTasaIVA), 1);
        //                    TotalItems = TotalItems + ((drItem("OrdServ_Precio") * drItem("Ordserv_Cant"))) + drItem("OrdServ_Impuesto");
        //                }
        //                // Thread.Sleep(15000)

        //                decimal totalpagos;
        //                bool sumo01 = false;
        //                string DctoExento;
        //                string DctoGravable;


        //                // ************SUB TOTAL DEL CF ********************
        //                // Si la impresora devuelve true imprimo  subtotal
        //                if (resp == true)
        //                {
        //                    DataSet dsBorroAbonos;

        //                    // OBTENGO LA SUMATORIA DE LOS ABONOS PARA SABER SI COINCIDEN CON EL TOTAL DE LOS ARTICULOS  PARA CANCELAR EL CF ANTES DE ENVIAR EL SUBTOTAL. 
        //                    // Dim dtPago As DataTable = ManBD.ExecutaSqlDataSet("pGetPagos '" & glbSucursalActual & "', '" & NroOrdenServicio & "', '0'", "Prueba", sqlCom).Tables(0)
        //                    // ManBD.ExecutaSqlDataSet("select 'DIVISAS'  Abo_Tipo, SUM(Abo_Monto) Abo_Monto from TEMP_ABONO A INNER JOIN TB_BANCOS B on A.Cod_Banco = B.CODBAN  WHERE NumOrdserv = " & NumeroOrdenImprimir & " and B.MONEDAEXTRANJERA = 1 GROUP BY convert(varchar,Fecha,103) ,Abo_Tipo UNION select convert(varchar,Fecha,103) +' '+ Abo_Tipo Abo_TIpo , Abo_Monto from TEMP_ABONO A INNER JOIN TB_BANCOS B on A.Cod_Banco = B.CODBAN  WHERE NumOrdserv = " & NumeroOrdenImprimir & " and B.MONEDAEXTRANJERA = 0 ", "Prueba", sqlCom).Tables(0)                         'DtIGTF.Tables(0)
        //                    DataTable dtPago = ManBD.ExecutaSqlDataSet("select * from TEMP_ABONO where NumOrdserv = " + NumeroOrdenImprimir, "Prueba", sqlCom).Tables(0);
        //                    DataRow drPago;

        //                    foreach (var drPago in dtPago.Rows)
        //                        totalpagos = totalpagos + drPago("Abo_Monto").ToString().Replace(",", "");

        //                    // ----CONSULTO PAGOS EN DIVISA
        //                    if (DtIGTF.Tables(0).Rows(0)("Abo_Monto") > 0 & DtIGTF.Tables(0).Rows(0)("ActivaIGTF") == true)
        //                        // MsgBox("TotalItems antes de IGTF: " & TotalItems)
        //                        // MsgBox("Monto IGTF: " & DtIGTF.Tables(0).Rows(0)("Abo_IGTF"))
        //                        TotalItems = TotalItems + DtIGTF.Tables(0).Rows(0)("Abo_IGTF");

        //                    // ----CONSULTO LOS DESCUENTOS DE LA FACTURA 
        //                    DataSet dsDcto = ManBD.ExecutaSqlDataSet("exec SP_DESCUENTOSFACTURAFISCAL '" + NumeroOrdenImprimir + "'", "Prueba", sqlCom);
        //                    DataTable dtcto = dsDcto.Tables(0);

        //                    if (totalpagos != (TotalItems - dtcto.Rows(0).Item("DescuentoExento") - dtcto.Rows(0).Item("DescuentoGravable") - dtcto.Rows(0).Item("DescuentoGravableA") - dtcto.Rows(0).Item("DescuentoGravableR")))
        //                    {
        //                        if (((TotalItems - dtcto.Rows(0).Item("DescuentoExento") - dtcto.Rows(0).Item("DescuentoGravable") - dtcto.Rows(0).Item("DescuentoGravableA") - dtcto.Rows(0).Item("DescuentoGravableR")) - totalpagos == "001" | totalpagos - (TotalItems - dtcto.Rows(0).Item("DescuentoExento") - dtcto.Rows(0).Item("DescuentoGravable") - dtcto.Rows(0).Item("DescuentoGravableA") - dtcto.Rows(0).Item("DescuentoGravableR")) == "001") | ((TotalItems - dtcto.Rows(0).Item("DescuentoExento") - dtcto.Rows(0).Item("DescuentoGravable") - dtcto.Rows(0).Item("DescuentoGravableA") - dtcto.Rows(0).Item("DescuentoGravableR")) - totalpagos == "002" | totalpagos - (TotalItems - dtcto.Rows(0).Item("DescuentoExento") - dtcto.Rows(0).Item("DescuentoGravable") - dtcto.Rows(0).Item("DescuentoGravableA") - dtcto.Rows(0).Item("DescuentoGravableR")) == "002") | ((TotalItems - dtcto.Rows(0).Item("DescuentoExento") - dtcto.Rows(0).Item("DescuentoGravable") - dtcto.Rows(0).Item("DescuentoGravableA") - dtcto.Rows(0).Item("DescuentoGravableR")) - totalpagos == "003" | totalpagos - (TotalItems - dtcto.Rows(0).Item("DescuentoExento") - dtcto.Rows(0).Item("DescuentoGravable") - dtcto.Rows(0).Item("DescuentoGravableA") - dtcto.Rows(0).Item("DescuentoGravableR")) == "003") | ((TotalItems - dtcto.Rows(0).Item("DescuentoExento") - dtcto.Rows(0).Item("DescuentoGravable") - dtcto.Rows(0).Item("DescuentoGravableA") - dtcto.Rows(0).Item("DescuentoGravableR")) - totalpagos == "004" | totalpagos - (TotalItems - dtcto.Rows(0).Item("DescuentoExento") - dtcto.Rows(0).Item("DescuentoGravable") - dtcto.Rows(0).Item("DescuentoGravableA") - dtcto.Rows(0).Item("DescuentoGravableR")) == "004") | ((TotalItems - dtcto.Rows(0).Item("DescuentoExento") - dtcto.Rows(0).Item("DescuentoGravable") - dtcto.Rows(0).Item("DescuentoGravableA") - dtcto.Rows(0).Item("DescuentoGravableR")) - totalpagos == "005" | totalpagos - (TotalItems - dtcto.Rows(0).Item("DescuentoExento") - dtcto.Rows(0).Item("DescuentoGravable") - dtcto.Rows(0).Item("DescuentoGravableA") - dtcto.Rows(0).Item("DescuentoGravableR")) == "005"))
        //                            sumo01 = true;
        //                        else
        //                        {
        //                            // MsgBox("mucha diferencia anula la impresión")
        //                            // SI NO COINCIDEN, ANULO EL TICKET
        //                            Vmax.CancelaCF();
        //                            ImprimirFacturaFiscal = false;
        //                            resp = false;
        //                            // borro los pobles abonos que hayan quedado en la tabla
        //                            ManBD.EjecutaStoreProcedure("Delete_Sencillo", "TEMP_ABONO" + "', '" + "NumOrdServ = " + NroOrdenServicio, Interaction.Command);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        // MsgBox("desc")
        //                        // ****ENVIO LOS DESCUENTOS DE ESTA FACTURA******
        //                        if (DctoFactura == true)
        //                        {
        //                            DataRow drItemdcto;
        //                            foreach (var drItemdcto in dtcto.Rows)
        //                            {
        //                                // resp = VMAX1.DescuentoCF("Descuento", drItemdcto("DescuentoExento"), drItemdcto("DescuentoGravable"), "", "")
        //                                if (TipoTasaIVA == "1")
        //                                    resp = Vmax.DescuentoCF("Descuento", drItemdcto("DescuentoExento"), drItemdcto("DescuentoGravable"), "", "", "");

        //                                if (TipoTasaIVA == "3")
        //                                    resp = Vmax.DescuentoCF("Descuento", drItemdcto("DescuentoExento"), "", "", drItemdcto("DescuentoGravableA"), "");


        //                                if (TipoTasaIVA == "2")
        //                                    resp = Vmax.DescuentoCF("Descuento", drItemdcto("DescuentoExento"), "", drItemdcto("DescuentoGravableR"), "", "");

        //                                DctoExento = drItemdcto("DescuentoExento");
        //                                DctoGravable = drItemdcto("DescuentoGravable") + drItemdcto("DescuentoGravableA") + drItemdcto("DescuentoGravableR");
        //                            }
        //                        }


        //                        // ******ENVIO EL SUBTOTAL DE LA FACTURA**************
        //                        if (resp == true)
        //                        {
        //                            if (DtIGTF.Tables(0).Rows(0)("ActivaIGTF") & DtIGTF.Tables(0).Rows(0)("Abo_Monto") > 0)
        //                            {
        //                                // MsgBox("Monto función IGTF : " & DtIGTF.Tables(0).Rows(0)("Abo_Monto"))
        //                                // resp = Vmax.SubTotalT(DtIGTF.Tables(0).Rows(0)("Abo_Monto"))
        //                                // MsgBox("IGTF CAORDSER: " & DtIGTF.Tables(0).Rows(0)("IGTFCAORDSER"))
        //                                // MsgBox("IGTF CALC: " & DtIGTF.Tables(0).Rows(0)("IGTFCALC"))
        //                                if (DtIGTF.Tables(0).Rows(0)("IGTFCAORDSER") == DtIGTF.Tables(0).Rows(0)("IGTFCALC"))
        //                                    resp = Vmax.SubTotalT(DtIGTF.Tables(0).Rows(0)("Abo_Monto"));
        //                                else
        //                                    resp = false;
        //                            }
        //                            else
        //                                // If DtIGTF.Tables(0).Rows(0)("Abo_Monto") = 0 Then
        //                                resp = Vmax.SubTotal;


        //                            // resp = Vmax.PagoCF("Divisas", 4380, 1)
        //                            // resp = Vmax.LeoDatosFiscales()
        //                            TotalFacturaFiscal = (Vmax.TotalFactura);
        //                        }
        //                        else
        //                        {
        //                            Vmax.CancelaCF();
        //                            ImprimirFacturaFiscal = false;
        //                            resp = false;
        //                        }
        //                    }
        //                    if (sumo01 == true)
        //                    {
        //                        // ****ENVIO LOS DESCUENTOS DE ESTA FACTURA******
        //                        if (DctoFactura == true)
        //                        {
        //                            DataRow drItemdcto;
        //                            foreach (var drItemdcto in dtcto.Rows)
        //                            {
        //                                // resp = Vmax.DescuentoCF("Descuento", drItemdcto("DescuentoExento"), drItemdcto("DescuentoGravable"), "", "")
        //                                if (TipoTasaIVA == "G")
        //                                    resp = Vmax.DescuentoCF("Descuento", drItemdcto("DescuentoExento"), drItemdcto("DescuentoGravable"), "", "", "");

        //                                if (TipoTasaIVA == "A")
        //                                    resp = Vmax.DescuentoCF("Descuento", drItemdcto("DescuentoExento"), "", "", drItemdcto("DescuentoGravableA"), "");

        //                                if (TipoTasaIVA == "R")
        //                                    resp = Vmax.DescuentoCF("Descuento", drItemdcto("DescuentoExento"), "", drItemdcto("DescuentoGravableR"), "", "");

        //                                DctoExento = drItemdcto("DescuentoExento");
        //                                DctoGravable = drItemdcto("DescuentoGravable") + drItemdcto("DescuentoGravableA") + drItemdcto("DescuentoGravableR");
        //                            }
        //                        }

        //                        // ******ENVIO EL SUBTOTAL DE LA FACTURA**************
        //                        if (resp == true)
        //                        {
        //                            if (DtIGTF.Tables(0).Rows(0)("ActivaIGTF") & DtIGTF.Tables(0).Rows(0)("Abo_Monto") > 0)
        //                            {
        //                                // MsgBox("Monto IGTF: " & DtIGTF.Tables(0).Rows(0)("Abo_Monto"))
        //                                // MsgBox("Monto función IGTF : " & DtIGTF.Tables(0).Rows(0)("Abo_Monto"))
        //                                // MsgBox("IGTF CAORDSER: " & DtIGTF.Tables(0).Rows(0)("IGTFCAORDSER"))
        //                                // MsgBox("IGTF CALC: " & DtIGTF.Tables(0).Rows(0)("IGTFCALC"))
        //                                if (DtIGTF.Tables(0).Rows(0)("IGTFCAORDSER") == DtIGTF.Tables(0).Rows(0)("IGTFCALC"))
        //                                    resp = Vmax.SubTotalT(DtIGTF.Tables(0).Rows(0)("Abo_Monto"));
        //                                else
        //                                    resp = false;
        //                            }
        //                            else
        //                                // If DtIGTF.Tables(0).Rows(0)("Abo_Monto") = 0 Then
        //                                resp = Vmax.SubTotal;
        //                            resp = Vmax.LeoDatosFiscales();
        //                            TotalFacturaFiscal = (Vmax.TotalFactura);
        //                        }
        //                        else
        //                        {
        //                            // VMAX1.CancelaCF()
        //                            Vmax.CancelaCF();
        //                            ImprimirFacturaFiscal = false;
        //                            resp = false;
        //                        }
        //                    }
        //                }
        //                else
        //                    ImprimirFacturaFiscal = false;

        //                // *************ENVIO LOS PAGOS***********************
        //                if (resp == true)
        //                {
        //                    DataTable dtPago = ManBD.ExecutaSqlDataSet("pGetPagos '" + glbSucursalActual + "', '" + NroOrdenServicio + "', '0'", "Prueba", sqlCom).Tables(0);

        //                    // Dim dtPago As DataTable = ManBD.ExecutaSqlDataSet("select 'DIVISAS'  Abo_Tipo, SUM(Abo_Monto) Abo_Monto from TEMP_ABONO A INNER JOIN TB_BANCOS B on A.Cod_Banco = B.CODBAN  WHERE NumOrdserv = " & NumeroOrdenImprimir & " and B.MONEDAEXTRANJERA = 1 GROUP BY convert(varchar,Fecha,103) ,Abo_Tipo UNION select convert(varchar,Fecha,103) +' '+ Abo_Tipo Abo_TIpo , Abo_Monto from TEMP_ABONO A INNER JOIN TB_BANCOS B on A.Cod_Banco = B.CODBAN  WHERE NumOrdserv = " & NumeroOrdenImprimir & " and B.MONEDAEXTRANJERA = 0 ", "Prueba", sqlCom).Tables(0)                         'DtIGTF.Tables(0)

        //                    // Dim dtPago As DataTable = ManBD.ExecutaSqlDataSet("select * from TEMP_ABONO where NumOrdserv = " & NumeroOrdenImprimir, "Prueba", sqlCom).Tables(0)
        //                    // Dim dtPago As DataTable = ManBD.ExecutaSqlDataSet("select Case WHEN B.MONEDAEXTRANJERA = 1 then 'DIVISAS' else  Convert(varchar,A.Fecha,103) +' '+A.Abo_Tipo     end Tipo_Pago, Abo_Monto  from TEMP_ABONO A INNER JOIN TB_BANCOS B on A.Cod_Banco = B.CODBAN  WHERE NumOrdserv = " & NumeroOrdenImprimir, "Prueba", sqlCom).Tables(0)

        //                    DataRow drPago;

        //                    // TotalFacturaFiscal = (VMAX1.TotalFactura)
        //                    TotalFacturaFiscal = (Vmax.TotalFactura);
        //                    decimal PagosEnviados;

        //                    foreach (var drPago in dtPago.Rows)
        //                    {
        //                        if (drPago("Abo_Monto") > 0)
        //                        {
        //                            if (drPago.Item("Abo_Tipo") == "DIVISAS")
        //                            {
        //                                if (DtIGTF.Tables(0).Rows(0)("Abo_Monto") != drPago("Abo_Monto").ToString().Replace(",", ""))
        //                                {
        //                                    resp = false;
        //                                    break;
        //                                }
        //                            }
        //                            // resp = Vmax.PagoCF(drPago.Item("Fecha") + " " + drPago.Item("Abo_Tipo"), drPago("Abo_Monto").ToString().Replace(",", ""), 1)
        //                            resp = Vmax.PagoCF(drPago.Item("Abo_Tipo"), drPago("Abo_Monto").ToString().Replace(",", ""), 1);
        //                            // resp = Vmax.PagoCF(drPago.Item("Tipo_Pago"), drPago("Abo_Monto").ToString().Replace(",", ""), 1)
        //                            PagosEnviados = PagosEnviados + drPago("Abo_Monto");
        //                        }
        //                    }
        //                    // f TotalFacturaFiscal - PagosEnviados.ToString().Replace(",", "") = "001" Or sumo01 = True Then
        //                    if (TotalFacturaFiscal - PagosEnviados.ToString().Replace(",", "") == "001" == true)
        //                    {
        //                        resp = Vmax.PagoCF("EFECTIVO", 1, 1);
        //                        DataSet dsSumoFact = ManBD.EjecutaStoreProcedure("SP_SUMOFACTURA", glbSucursalActual + "','" + Vmax.UltimoCFAbierto + "','" + Vmax.RetornoSerial + "','" + NumeroOrdenImprimir + "','" + TotalFacturaFiscal + "','" + DctoExento + "','" + DctoGravable, Interaction.Command);                         // 
        //                    }
        //                    else if (TotalFacturaFiscal - PagosEnviados.ToString().Replace(",", "") == "002")
        //                    {
        //                        resp = Vmax.PagoCF("EFECTIVO", 2, 1);
        //                        DataSet dsSumoFact = ManBD.EjecutaStoreProcedure("SP_SUMOFACTURA", glbSucursalActual + "','" + Vmax.UltimoCFAbierto + "','" + Vmax.RetornoSerial + "','" + NumeroOrdenImprimir + "','" + TotalFacturaFiscal + "','" + DctoExento + "','" + DctoGravable, Interaction.Command);                         // 
        //                    }
        //                    else if (TotalFacturaFiscal - PagosEnviados.ToString().Replace(",", "") == "003")
        //                    {
        //                        resp = Vmax.PagoCF("EFECTIVO", 3, 1);
        //                        DataSet dsSumoFact = ManBD.EjecutaStoreProcedure("SP_SUMOFACTURA", glbSucursalActual + "','" + Vmax.UltimoCFAbierto + "','" + Vmax.RetornoSerial + "','" + NumeroOrdenImprimir + "','" + TotalFacturaFiscal + "','" + DctoExento + "','" + DctoGravable, Interaction.Command);                         // 
        //                    }
        //                    else if (TotalFacturaFiscal - PagosEnviados.ToString().Replace(",", "") == "004")
        //                    {
        //                        resp = Vmax.PagoCF("EFECTIVO", 4, 1);
        //                        DataSet dsSumoFact = ManBD.EjecutaStoreProcedure("SP_SUMOFACTURA", glbSucursalActual + "','" + Vmax.UltimoCFAbierto + "','" + Vmax.RetornoSerial + "','" + NumeroOrdenImprimir + "','" + TotalFacturaFiscal + "','" + DctoExento + "','" + DctoGravable, Interaction.Command);                         // 
        //                    }
        //                    else if (TotalFacturaFiscal - PagosEnviados.ToString().Replace(",", "") == "005")
        //                    {
        //                        resp = Vmax.PagoCF("EFECTIVO", 5, 1);
        //                        DataSet dsSumoFact = ManBD.EjecutaStoreProcedure("SP_SUMOFACTURA", glbSucursalActual + "','" + Vmax.UltimoCFAbierto + "','" + Vmax.RetornoSerial + "','" + NumeroOrdenImprimir + "','" + TotalFacturaFiscal + "','" + DctoExento + "','" + DctoGravable, Interaction.Command);                         // 
        //                    }
        //                }
        //                else
        //                    ImprimirFacturaFiscal = false;

        //                if (resp == true)
        //                {
        //                    // Si la impresora devuelve true imprimo los comentarios y cierro el CF
        //                    // If DtIGTF.Tables(0).Rows(0)("Abo_Monto") > 0 Then
        //                    // resp = Vmax.TextoDNF("Divisas: " & DtIGTF.Tables(0).Rows(0)("Abo_Monto"))
        //                    // End If

        //                    resp = Vmax.TextoDNF(" ");
        //                    resp = Vmax.TextoDNF("Numero Orden: " + NumeroOrdenImprimir);
        //                    resp = Vmax.TextoDNF(" ");

        //                    if (ValorParametroPGE("FactFiscalconRxPGE", Interaction.Command) == 1)
        //                    {
        //                        oOrdenServicio.ObtenerOrdenServicio(NumeroOrdenImprimir, glbSucursalActual, sqlCom);

        //                        if (oOrdenServicio.EsAsegurada)
        //                        {

        //                            // oExamenConv.ObtenerRx(NacionalidadRif, CedulaCliente, oOrdenServicio.NumeroExamen, glbSucursalActual, sqlCom)

        //                            // resp = VMAX1.TextoDNF("     ESF   EJE   CIL   ADD   DNP")
        //                            // resp = VMAX1.TextoDNF("O.D: " & oExamen.ESFD & "   " & oExamen.EJED & "   " & oExamen.CILD & "   " & oExamen.ADDD)
        //                            // resp = VMAX1.TextoDNF("E.I: " & oExamen.ESFI & "   " & oExamen.EJEI & "   " & oExamen.CILI & "   " & oExamen.ADDI)
        //                            // resp = VMAX1.TextoDNF(" ")

        //                            CapaNegocio.Formulas oFormulas = new CapaNegocio.Formulas();
        //                            double ESFD;
        //                            double ESFI;
        //                            double CILD;
        //                            double CILI;
        //                            int EJED;
        //                            int EJEI;
        //                            double ADDD;
        //                            double ADDI;
        //                            // 2da Refraccion
        //                            double ESFD2;
        //                            double ESFI2;
        //                            double CILD2;
        //                            double CILI2;
        //                            int EJED2;
        //                            int EJEI2;

        //                            // oExamenConv.ObtenerRx(NacionalidadRif, CedulaCliente, oOrdenServicio.NumeroExamen, glbSucursalActual, sqlCom)
        //                            oExamenConv.ObtenerRx(oOrdenServicio.ClienteNacionalidad, oOrdenServicio.ClienteCedulaIdentidad, oOrdenServicio.NumeroExamen, glbSucursalActual, sqlCom);

        //                            ESFD = oExamenConv.ESFD;
        //                            ESFI = oExamenConv.ESFI;
        //                            CILD = oExamenConv.CILD;
        //                            CILI = oExamenConv.CILI;
        //                            EJED = oExamenConv.EJED;
        //                            EJEI = oExamenConv.EJEI;
        //                            ADDD = oExamenConv.ADDD;
        //                            ADDI = oExamenConv.ADDI;

        //                            // 2da Refraccion
        //                            ESFD2 = oExamenConv.ESFD2;
        //                            ESFI2 = oExamenConv.ESFI2;
        //                            CILD2 = oExamenConv.CILD2;
        //                            CILI2 = oExamenConv.CILI2;
        //                            EJED2 = oExamenConv.EJED2;
        //                            EJEI2 = oExamenConv.EJEI2;

        //                            if (oExamenConv.CILD > 0)
        //                            {
        //                                oFormulas.Transposicion(ESFD, CILD, EJED);
        //                                ESFD = oFormulas.ESF;
        //                                CILD = oFormulas.CIL;
        //                                EJED = oFormulas.EJE;
        //                            }
        //                            if (oExamenConv.CILI > 0)
        //                            {
        //                                oFormulas.Transposicion(ESFI, CILI, EJEI);
        //                                ESFI = oFormulas.ESF;
        //                                CILI = oFormulas.CIL;
        //                                EJEI = oFormulas.EJE;
        //                            }

        //                            // 2da Refraccion
        //                            if (oExamenConv.CILD2 > 0)
        //                            {
        //                                oFormulas.Transposicion(ESFD2, CILD2, EJED2);
        //                                ESFD2 = oFormulas.ESF;
        //                                CILD2 = oFormulas.CIL;
        //                                EJED2 = oFormulas.EJE;
        //                            }
        //                            if (oExamenConv.CILI2 > 0)
        //                            {
        //                                oFormulas.Transposicion(ESFI2, CILI2, EJEI2);
        //                                ESFI2 = oFormulas.ESF;
        //                                CILI2 = oFormulas.CIL;
        //                                EJEI2 = oFormulas.EJE;
        //                            }
        //                            resp = Vmax.TextoDNF("*Contrato de Garantia Extendida para");
        //                            resp = Vmax.TextoDNF("      Cristales Formulados:");
        //                            resp = Vmax.TextoDNF(" ");

        //                            resp = Vmax.TextoDNF("OD: ESF " + Strings.Format((float)ESFD, "#,##0.00") + " EJE " + EJED + " CIL " + Strings.Format((float)CILD, "#,##0.00") + " ADD " + Strings.Format((float)ADDD, "#,##0.00"));
        //                            // 2da Refraccion
        //                            if (ESFD2 != 0)
        //                            {
        //                                resp = Vmax.TextoDNF("2da. Refracción");
        //                                resp = Vmax.TextoDNF("OD: ESF " + Strings.Format((float)ESFD2, "#,##0.00") + " EJE " + EJED2 + " CIL " + Strings.Format((float)CILD2, "#,##0.00"));
        //                            }

        //                            resp = Vmax.TextoDNF("OI: ESF " + Strings.Format((float)ESFI, "#,##0.00") + " EJE " + EJEI + " CIL " + Strings.Format((float)CILI, "#,##0.00") + " ADD " + Strings.Format((float)ADDI, "#,##0.00"));
        //                            // 2da Refraccion
        //                            if (ESFI2 != 0)
        //                            {
        //                                resp = Vmax.TextoDNF("2da. Refracción");
        //                                resp = Vmax.TextoDNF("OI: ESF " + Strings.Format((float)ESFI2, "#,##0.00") + " EJE " + EJEI2 + " CIL " + Strings.Format((float)CILI2, "#,##0.00"));
        //                            }

        //                            resp = Vmax.TextoDNF(" ");
        //                            resp = Vmax.TextoDNF("Condiciones legales en el Contrato");
        //                            resp = Vmax.TextoDNF("Suscrito.");
        //                            resp = Vmax.TextoDNF(" ");
        //                        }
        //                    }

        //                    DataTable DtTexto = ManBD.ExecutaSqlDataSet("select * from TB_INUTILIZADO", "Prueba", sqlCom).Tables(0);
        //                    DataRow DrTexto;

        //                    foreach (var DrTexto in DtTexto.Rows)
        //                        resp = Vmax.TextoDNF(DrTexto.Item("texto"));

        //                    resp = Vmax.CerrarCF;
        //                    if (resp == true)
        //                    {
        //                        ImprimirFacturaFiscal = true;
        //                        statusOS = "002";
        //                        NumeroComprobanteFiscal = Vmax.UltimoCFAbierto;

        //                        switch (NumeroComprobanteFiscal.Length)
        //                        {
        //                            case 7:
        //                                {
        //                                    NumeroComprobanteFiscal = NumeroComprobanteFiscal;
        //                                    break;
        //                                }

        //                            case 6:
        //                                {
        //                                    NumeroComprobanteFiscal = "0" + NumeroComprobanteFiscal;
        //                                    break;
        //                                }

        //                            case 5:
        //                                {
        //                                    NumeroComprobanteFiscal = "00" + NumeroComprobanteFiscal;
        //                                    break;
        //                                }

        //                            case 4:
        //                                {
        //                                    NumeroComprobanteFiscal = "000" + NumeroComprobanteFiscal;
        //                                    break;
        //                                }

        //                            case 3:
        //                                {
        //                                    NumeroComprobanteFiscal = "0000" + NumeroComprobanteFiscal;
        //                                    break;
        //                                }

        //                            case 2:
        //                                {
        //                                    NumeroComprobanteFiscal = "00000" + NumeroComprobanteFiscal;
        //                                    break;
        //                                }

        //                            case 1:
        //                                {
        //                                    NumeroComprobanteFiscal = "000000" + NumeroComprobanteFiscal;
        //                                    break;
        //                                }
        //                        }

        //                        CapaNegocio.Usuario oUsu = new CapaNegocio.Usuario();
        //                        oUsu.ObtenerUsuarioCodigo(glbUsuarioActual, Interaction.Command);
        //                        oAuditor.AgregarRegistroAuditor(glbSucursalActual, "071", oUsu.CarnetEmpleado, "OS: " + NroOrdenServicio + ", Factura: " + NumeroComprobanteFiscal + ", Serial: " + Vmax.RetornoSerial, Interaction.Command);
        //                    }
        //                    else
        //                    {
        //                        Vmax.CancelaCF();
        //                        ImprimirFacturaFiscal = false;
        //                    }
        //                }
        //                else
        //                    ImprimirFacturaFiscal = false;
        //                NumeroComprobanteFiscal = Vmax.UltimoCFAbierto;


        //                // borro los pobles abonos que hayan quedado en la tabla
        //                // comentado para recuperar la informacion de esta tabla si la factura no se graba ya se imprimio 18/12/09
        //                // ManBD.EjecutaStoreProcedure("Delete_Sencillo", "TEMP_ABONO" & "', '" & "NumOrdServ = " & NroOrdenServicio, Command)

        //                // Command.Connection.Close()

        //                Barra.AvanceBarraProgreso();
        //                Vmax.CerrarPuerto();
        //            }
        //        }
        //        else
        //        {
        //            FacturaManual = true;

        //            frmFacturaManual oFactManual = new frmFacturaManual();

        //            string SerieManual; // EH: 18/07/2018
        //            SerieManual = ValorParametro("SerieManual", sqlCom);


        //            oFactManual.txtNroOs.Text = NroOrdenServicio;
        //            oFactManual.txtClientePagador.Text = Cliente.NombreCompleto;
        //            oFactManual.txtNacioCtePagador.Text = Cliente.Nacionalidad;
        //            oFactManual.txtCedCtePagador.Text = Cliente.Cedula;
        //            oFactManual.txtFechaE.Text = DateTime.Today.Now;
        //            // oFactManual.txtTelefono.Text = IIf(txtTelCel.Text <> "", txtTelCel.Text, txtTelHab.Text)
        //            oFactManual.txtNroFact.Text = SerieManual + NumeroComprobanteFiscal; // EH: 18/07/2018
        //            oFactManual.txtNroFact.Focus();

        //            // OrdSerSubTotal = gexOrdSer.GetRow(gexOrdSer.Row).Cells("SubTotal").Value ' - gexOrdSer.GetRow(gexOrdSer.Row).Cells("Descuento").Value
        //            OrdSerTotal = gexOrdSer.GetRow(gexOrdSer.Row).Cells("Total").Value;

        //            oFactManual.txtMontoDesc.Text = Format(Val(System.Convert.ToDecimal(OrdSerDescuento)), "Standard");
        //            oFactManual.txtTotalSinIVA.Text = Format(Val(System.Convert.ToDecimal(OrdSerSubTotal)), "Standard");
        //            oFactManual.txtTotalaPagar.Text = Format(Val(System.Convert.ToDecimal(OrdSerTotal)), "Standard");
        //            oFactManual.txtTotalIVA.Text = Format(Val(System.Convert.ToDecimal(OrdSerImpuesto)), "Standard");
        //            oFactManual.txtTotalIGTF.Text = Format(Val(System.Convert.ToDecimal(MontoIGTF)), "Standard");
        //            oFactManual.txtPorcIGTF.Text = System.Convert.ToInt32(PorcIGTF);
        //            // oFactManual.CodVendedor = CodVendedor
        //            // oFactManual.FechaOfrecido = FechaOfrecido
        //            // oFactManual.HoraOfrecido = HoraOfrecido
        //            // oFactManual.TipoVtaFactura = TipoVtaFactura


        //            IVA.ObtenerImpDes(Interaction.IIf(TipoTasaIVA == "G", "I", TipoTasaIVA), Interaction.Command);
        //            oFactManual.txtPorcIVA.Text = System.Convert.ToInt32(IVA.PorcentajeID);
        //            oFactManual.OrigenLlamado = "VentaPendiente";
        //            oFactManual.ShowDialog();

        //            if (oFactManual.RetornoOK())
        //            {
        //                Barra.Visible = true;
        //                Barra.InhabilitarCursor();
        //                Barra.AvanceBarraProgreso();

        //                ImprimirFacturaFiscal = true;

        //                // OrdSerDescuento = oFactManual.txtMontoDesc.Text
        //                // OrdSerSubTotal = oFactManual.txtTotalSinIVA.Text
        //                // OrdSerTotal = oFactManual.txtTotalaPagar.Text
        //                // OrdSerImpuesto = oFactManual.txtTotalIVA.Text
        //                NumeroComprobanteFiscal = oFactManual.txtNroFact.Text;

        //                switch (NumeroComprobanteFiscal.Length)
        //                {
        //                    case 7:
        //                        {
        //                            NumeroComprobanteFiscal = NumeroComprobanteFiscal;
        //                            break;
        //                        }

        //                    case 6:
        //                        {
        //                            NumeroComprobanteFiscal = "0" + NumeroComprobanteFiscal;
        //                            break;
        //                        }

        //                    case 5:
        //                        {
        //                            NumeroComprobanteFiscal = "00" + NumeroComprobanteFiscal;
        //                            break;
        //                        }

        //                    case 4:
        //                        {
        //                            NumeroComprobanteFiscal = "000" + NumeroComprobanteFiscal;
        //                            break;
        //                        }

        //                    case 3:
        //                        {
        //                            NumeroComprobanteFiscal = "0000" + NumeroComprobanteFiscal;
        //                            break;
        //                        }

        //                    case 2:
        //                        {
        //                            NumeroComprobanteFiscal = "00000" + NumeroComprobanteFiscal;
        //                            break;
        //                        }

        //                    case 1:
        //                        {
        //                            NumeroComprobanteFiscal = "000000" + NumeroComprobanteFiscal;
        //                            break;
        //                        }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MensajeError.MuestroMensaje("Error en la función", "frmOrdenesServicio.ImprimirFacturaFiscal", "Por favor comunicarse con el Dpto de Sistemas y reportar el siguiente error: ", Information.Err.Description, CapaNegocio.MensajesGenerales.TiposIconos.IconoError, glbUsuarioActual);
        //        MensajeError.ShowDialog();
        //    }
        //    finally
        //    {
        //        Barra.Visible = false;
        //        Barra.HabilitarCursor();
        //    }
        //}


    }
}




