using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaServiciosExternos;
using CapaServiciosExternos.Modelos;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using CapaDatos.Cashea_Datos;

namespace CapaLogica.Cashea_Logica
{
    public class L_Cashea
    {
        // Cambia la línea de declaración a como estaba:
        private readonly CasheaService _casheaService = new CasheaService();
        private D_Cashea _D_Cashea = new D_Cashea();

        // Cambia la línea 21 por esta:
        public async Task<CasheaResult<string>> ValidarEstadoServicio()
        {
            try
            {
                // Llamamos al método
                return await _casheaService.CheckHealthAsync();
            }
            catch (Exception ex)
            {
                return new CasheaResult<string>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = ex.Message
                };
            }
        }

        public async Task<List<PointOfSale>> ListarCajasPos()
        {
            // Podrías agregar lógica de filtrado aquí si fuera necesario
            return await _casheaService.GetBoxesAsync();
        }

        // Cambia Task<CasheaOrderResponse> por Task<CasheaResult<CasheaOrderResponse>>
        public async Task<CasheaResult<CasheaOrderResponse>> CrearOrdenCashea(double monto, string factura, string cedula)
        {
            var request = new CasheaOrderRequest
            {
                Amount = monto,
                InvoiceId = factura,
                IdentificationNumber = cedula,
                Version = "2.0.0"
            };

            // Ahora la flecha roja de la línea 57 desaparecerá
            return await _casheaService.CreateOrderAsync(request);
        }

        public async Task<CasheaResult<bool>> SimularEscaneoCliente(string orderUuid)
        {
            // Ahora retorna el objeto completo con el StatusCode para el auditor
            return await _casheaService.SimularEscaneoQRAsync(orderUuid);
        }

        public async Task<CasheaResult<CasheaPaymentPlanResponse>> ObtenerPlanPago(string orderUuid)
        {
            return await _casheaService.GetPaymentPlanAsync(orderUuid);
        }


        public async Task<CasheaResult<bool>> CancelarOrden(string orderUuid)
        {
            return await _casheaService.CancelarOrdenAsync(orderUuid);

        }
   
        public async Task<CasheaResult<bool>> ConfirmarPago(string orderUuid, double monto)
        {
            return await _casheaService.ConfirmarPagoInicialAsync(orderUuid, monto);
        }

        public async Task<CasheaResult<CasheaOrderDetailsResponse>> ConsultarDetallesOrden(string orderUuid)
        {
            return await _casheaService.GetOrderDetailsAsync(orderUuid);
        }

        public bool RegistrarCuotasCashea(string codSucursal, string nroOren, string nroContrato, string nroCuota, decimal montoCuota, string userCrea, SqlCommand command = null)
        {
            try
            {
                DataSet dts = _D_Cashea.RegistrarCuotasCashea(codSucursal, nroOren, nroContrato, nroCuota, montoCuota, userCrea, command);

                //if (dts != null && dts.Tables[0] != null)
                //{
                    //if (dts.Tables[0].Rows[0][0].ToString() == "SATISFACTORIO")
                    //{
                        return true;
                    //}
                    //else
                    //{
                    //    if (dts.Tables[0] != null && dts.Tables[0].Rows[0]["REPORTE_DIA"].ToString() == "0")
                    //    {
                    //        //mostrarError("Recuerde generar el reporte Z del día");
                    //        return true;
                    //        //return false;
                    //    }
                    //    else
                    //        return false;
                    //}
                //}
                //else
                //{
                //    return false;
                //}

            }
            catch (Exception ex)
            {
                // Código para manejar el error
                EscribirLog(ex.Message.ToString());
                return false;
            }
        }

        public static void EscribirLog(string mensaje)
        {
            string ruta = "log.txt";
            string entrada = $"[{DateTime.Now}] {mensaje}";
            System.IO.File.AppendAllText(ruta, entrada + Environment.NewLine);
        }

        //public async Task<bool> ActualizarFacturaOrden(string orderUuid, string nroFactura)
        //{
        //    return await _casheaService.ActualizarFacturaAsync(orderUuid, nroFactura);
        //}
        public async Task<CasheaResult<bool>> ActualizarFacturaOrden(string orderUuid, string numeroFactura)
        {
            return await _casheaService.ActualizarFacturaAsync(orderUuid, numeroFactura);
        }
        public bool RegistrarOrdenCashea(string codSucursal, string nroOrden, string nroFactura, bool status, string userCrea, SqlCommand command = null)
        {
            try
            {
                DataSet dts = _D_Cashea.RegistrarOrdenCashea(codSucursal, nroOrden, nroFactura, status, userCrea, command);

                //if (dts != null && dts.Tables[0] != null)
                //{
                //if (dts.Tables[0].Rows[0][0].ToString() == "SATISFACTORIO")
                //{
                return true;
                //}
                //else
                //{
                //    if (dts.Tables[0] != null && dts.Tables[0].Rows[0]["REPORTE_DIA"].ToString() == "0")
                //    {
                //        //mostrarError("Recuerde generar el reporte Z del día");
                //        return true;
                //        //return false;
                //    }
                //    else
                //        return false;
                //}
                //}
                //else
                //{
                //    return false;
                //}

            }
            catch (Exception ex)
            {
                // Código para manejar el error
                EscribirLog(ex.Message.ToString());
                return false;
            }
        }

       
        public async Task<CasheaResult<CasheaPaymentPlanResponse>> ValidarCodigoSeguridad(string orderUuid, string codigoSeguridad)
        {
            return await _casheaService.GetPaymentPlanByCodeAsync(orderUuid, codigoSeguridad);
        }

        public DataTable ObtenerConfigCashea(SqlCommand command = null)
        {
            DataTable dt = _D_Cashea.ObtenerConfigCashea();

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return dt;
            }
        }

       
    }
}
