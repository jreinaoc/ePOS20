using CapaDatos.DetalleOrden_Datos;
using CapaDatos.Inicio_Datos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica.Impresion
{
    class Imprimir
    {
        private D_Inicio _D_Inicio = new D_Inicio();
        private D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        private VmaxComVe.VmaxComClass objVmax = new VmaxComVe.VmaxComClass();
        private int glbPuertoCOM = Convert.ToInt16(ConfigurationManager.AppSettings.Get("PuertoCOMimpresora"));
        public string SerialImpresora;
        public string FechaImpresora;
        public string NumeroComprobanteFiscal;
        string CodSuc = "";

        //private bool ImprimirNCFiscal(string NumeroFactAfectada, double Monto)
        //{
        //    try
        //    {
        //        uint resp = 0;
        //        CodSuc = _D_Inicio.Sucursal();
        //        resp = objVmax.AbrirPuerto(Convert.ToString(glbPuertoCOM));

        //        if (resp != 0)
        //        {
        //            objVmax.CerrarPuerto();
        //            NumeroComprobanteFiscal = Convert.ToString(objVmax.RetornoMF.uiUltNumZ);
        //        }

        //        else
        //        {

        //            objVmax.AbrirPuerto(Convert.ToString(glbPuertoCOM));
        //            objVmax.ObtenerReporteInformativo();
        //            objVmax.AbrirDNF();
        //            SerialImpresora = objVmax.RetornoMI.sSerial;
        //            FechaImpresora = objVmax.RetornoMI.sFecha;
        //            NumeroComprobanteFiscal = objVmax.RetornoAbrirFactura.uiNumeroFactura.ToString();
        //        }

        //            string Resultado;
        //            Resultado = objVmax.RetornoStatusImpresora.ToString();

        //        if (resp == 0 & SerialImpresora != "")
        //        {
                    
        //            DataTable dt = _D_DetalleOrden.BucarFactura(NumeroFactAfectada, SerialImpresora, CodSuc);

        //            oCliente.ObtenerDatosCliente(oFacturas.ClientePagadorCedula, oFacturas.ClientePagadorNacionalidad, sqlCom);
        //            // ********************************************

        //            // ***************************
        //            // '***************************
        //            // IMPRESORA FISCAL
        //            Barra.AvanceBarraProgreso();
        //            frmPrepararImpresora PrepararImpres = new frmPrepararImpresora();
        //            PrepararImpres.ShowDialog();

        //            bool resp;
        //            // abro el CF. Datos del cliente

        //            // AxVMAX3.AbrirPuerto()

        //            // ''''' ********* DATOS DEL CLIENTE ************
        //            resp = Vmax.LeoDatosFiscales();
        //            resp = Vmax.AbrirCF(oCliente.PrimerNombre + " " + oCliente.PrimerApellido, oCliente.Nacionalidad + "" + oCliente.Cedula, "D", NumeroFactAfectada, Vmax.RetornoSerial, oFacturas.FechaOperacion, System.Convert.ToString(Format(System.Convert.ToDateTime(oFacturas.FacturaFechaCreacion), "H:mm:00 ")));

        //            NumeroNCFiscal = IIf(ValorParametro("NCImpFiscalSuma", sqlCom) == 0, Vmax.UltimaNCAbierta, Vmax.UltimaNCAbierta + 1);

        //            bool DctoFactura;
        //            /// **********'IMPRIMO LOS ITEMS *********************
        //            if (resp == true)
        //            {
        //                /// *****
        //                // imprimo los items
        //                DataSet dsArti = ManBD.ExecutaSqlDataSet("exec SP_DETALLENOTACREDITOFISCAL '" + NumeroFactAfectada + "','" + oFacturas.FacturaSerialImpresora + "'", "Prueba", Interaction.Command);
        //                DataTable dt = dsArti.Tables(0);

        //                DataRow drItem;


        //                foreach (var drItem in dsArti.Tables(0).Rows)
        //                {
        //                    if (drItem.Item("Ordserv_Dto") > 0)
        //                        DctoFactura = true;

        //                    resp = Vmax.ItemDev(drItem.Item("CodArticulo") + " " + drItem.Item("DESART"), drItem.Item("Ordserv_Cant") * 1000, drItem("OrdServ_Precio"), IIf(drItem.Item("ART_EXENTO") == 1, "E", "G"), 1);
        //                }
        //            }
        //            else
        //                ImprimirNCFiscal = false;


        //            if (resp == true)
        //            {

        //                // ****ENVIO LOS DESCUENTOS DE ESTA LA FACTURA AFECTADA ******
        //                if (DctoFactura == true)
        //                {
        //                    DataSet dsDcto = ManBD.ExecutaSqlDataSet("exec SP_DESCUENTOSFACTURAFISCAL '" + oFacturas.NumeroOrdenServicio + "'", "Prueba", sqlCom);
        //                    DataTable dtcto = dsDcto.Tables(0);

        //                    DataRow drItemdcto;
        //                    foreach (var drItemdcto in dtcto.Rows)
        //                        // resp = VMAX.DescuentoCF("Descuento", drItemdcto("DescuentoExento"), drItemdcto("DescuentoGravable"), "", "")
        //                        resp = Vmax.DescuentoCF("Descuento", drItemdcto("DescuentoExento"), drItemdcto("DescuentoGravable"), "", "", "");
        //                }

        //                // ************SUB TOTAL DEL CF ********************
        //                resp = Vmax.SubTotal;
        //                resp = resp == Vmax.TextoDNF("Monto Disponible: " + Monto);


        //                // *****
        //                // LA NOTA DE CREDITO NO LLEVA PAGOS

        //                // ******CIERRO EL CF**********************
        //                resp = Vmax.CerrarCF;

        //                ImprimirNCFiscal = true;
        //            }
        //            else
        //                ImprimirNCFiscal = false;

        //            // completo el numero de la nota de credito
        //            switch (NumeroNCFiscal.Length)
        //            {
        //                case 1:
        //                    {
        //                        NumeroNCFiscal = "000000" + NumeroNCFiscal;
        //                        break;
        //                    }

        //                case 2:
        //                    {
        //                        NumeroNCFiscal = "00000" + NumeroNCFiscal;
        //                        break;
        //                    }

        //                case 3:
        //                    {
        //                        NumeroNCFiscal = "0000" + NumeroNCFiscal;
        //                        break;
        //                    }

        //                case 4:
        //                    {
        //                        NumeroNCFiscal = "000" + NumeroNCFiscal;
        //                        break;
        //                    }

        //                case 5:
        //                    {
        //                        NumeroNCFiscal = "00" + NumeroNCFiscal;
        //                        break;
        //                    }

        //                case 6:
        //                    {
        //                        NumeroNCFiscal = "0" + NumeroNCFiscal;
        //                        break;
        //                    }

        //                case 7:
        //                    {
        //                        NumeroNCFiscal = NumeroNCFiscal;
        //                        break;
        //                    }
        //            }
        //        }
        //    }

        //    // ***************************
        //    catch (Exception ex)
        //    {
        //        MensajeError.MuestroMensaje("Error en la función", "frmNotasCredito.ImprimirNCFiscal", "Por favor comunicarse con el Dpto de Sistemas y reportar el siguiente error: ", Information.Err.Description, CapaNegocio.MensajesGenerales.TiposIconos.IconoError, glbUsuarioActual);
        //        MensajeError.ShowDialog();
        //    }
        //}
    }
}
