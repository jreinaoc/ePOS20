using System;
using System.Configuration;
using System.Text;

namespace CapaLogica.Impresora_Fiscal
{
    public class Impresora_Fiscal
    {
        //El uso de la clase StringBuilder nos ayudara a devolver los mensajes de las validaciones
        public readonly StringBuilder stringBuilder = new StringBuilder();
        private int glbPuertoCOM = Convert.ToInt16(ConfigurationManager.AppSettings.Get("PuertoCOMimpresora"));



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
                            ReportesElectronicos(puerto,1);
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
    }
}
