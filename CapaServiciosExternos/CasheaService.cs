using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CapaServiciosExternos.Modelos;
using System.Configuration;
using System.Windows;
//using CapaDatos.Anulacion;
using CapaEntidades;


namespace CapaServiciosExternos
{
    public class CasheaService
    {

        // Configuración centralizada
        //private readonly string _apiKey = "6yaBo2pKwqKu4GtEkdpgDYSF8xgqQ06b";
        //private readonly string _baseUrl = "https://staging.external.cashea.app/v2";

        // Ahora leemos desde el archivo de configuración
        // Dinámico: Cambia según el ambiente (Staging/Prod)
        // Antes (Leyendo del archivo .config local)
        // private readonly string _baseUrl = ConfigurationManager.AppSettings["Cashea_BaseUrl"];
        // private readonly string _apiKey = ConfigurationManager.AppSettings["Cashea_ApiKey"];

        // Ahora (Leyendo de tu clase global cargada desde SQL)
        //private readonly string _baseUrl = ConfigCashea.BaseUrl;
        //private readonly string _apiKey = ConfigCashea.ApiKey;

        // Estático: El recurso dentro de la API no suele cambiar
        private const string HealthEndpoint = "/health";
        private const string PosEndpoint = "/pos/";
        private const string OrdersEndpoint = "/v2/orders/pos/";

        
        public async Task<CasheaResult<string>> CheckHealthAsync()
        {
            // 1. Si no hay URL, devolvemos un error controlado 400 (Bad Request local)
            //if (string.IsNullOrEmpty(_baseUrl))
            //{
            //    return new CasheaResult
            //    {
            //        IsSuccess = false,
            //        StatusCode = 400,
            //        Message = "Configuración de URL no encontrada en el App.config"
            //    };
            //}

            string fullUrl = $"{ConfigCashea.BaseUrl.TrimEnd('/')}{HealthEndpoint}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"ApiKey {ConfigCashea.ApiKey}");
                client.Timeout = TimeSpan.FromSeconds(10);

                try
                {
                    var response = await client.GetAsync(fullUrl);
                    string contenido = await response.Content.ReadAsStringAsync();

                    // Retornamos el objeto con toda la información necesaria para el auditor
                    return new CasheaResult<string>
                    {
                        Url = fullUrl,
                        IsSuccess = response.IsSuccessStatusCode,
                        StatusCode = (int)response.StatusCode,
                        Message = contenido
                    };
                }
                catch (Exception ex)
                {
                    // Si el servidor está caído o la URL es falsa (DNS fail), devolvemos 500
                    return new CasheaResult<string>
                    {
                        Url = fullUrl,
                        IsSuccess = false,
                        StatusCode = 500,
                        Message = "Error de conexión o Timeout: " + ex.Message
                    };
                }
            }
        }

        /// <summary>
        /// [GET] Obtener Cajas: Retorna la lista de puntos de venta (líneas) disponibles.
        /// </summary>
        public async Task<List<PointOfSale>> GetBoxesAsync()
        {
            // Validación preventiva por si el App.config falla
            if (string.IsNullOrEmpty(ConfigCashea.BaseUrl)) return new List<PointOfSale>();

            string fullUrl = $"{ConfigCashea.BaseUrl.TrimEnd('/')}{PosEndpoint}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"ApiKey {ConfigCashea.ApiKey}");
                client.Timeout = TimeSpan.FromSeconds(15);

                try
                {
                    var response = await client.GetAsync(fullUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        // Deserializamos usando tu modelo de respuesta
                        var result = JsonConvert.DeserializeObject<CasheaPosResponse>(json);

                        return result?.PointOfSales ?? new List<PointOfSale>();
                    }
                    return new List<PointOfSale>();
                }
                catch (Exception)
                {
                    // En producción, aquí podrías loguear el error
                    return new List<PointOfSale>();
                }
            }
        }

        public async Task<CasheaResult<CasheaOrderResponse>> CreateOrderAsync(CasheaOrderRequest request)
        {
            if (string.IsNullOrEmpty(ConfigCashea.BaseUrl))
                return new CasheaResult<CasheaOrderResponse> { IsSuccess = false, StatusCode = 400, Message = "URL no configurada" };

            string fullUrl = $"{ConfigCashea.BaseUrl.TrimEnd('/')}{OrdersEndpoint}{ConfigCashea.UuidCaja}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"ApiKey {ConfigCashea.ApiKey}");

                string jsonContent = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                try
                {
                    var response = await client.PostAsync(fullUrl, content);
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return new CasheaResult<CasheaOrderResponse>
                        {
                            Url = fullUrl,
                            IsSuccess = true,
                            StatusCode = (int)response.StatusCode,
                            Data = JsonConvert.DeserializeObject<CasheaOrderResponse>(jsonResponse),
                            Message = "Orden creada con éxito"
                        };
                    }
                    else
                    {
                        return new CasheaResult<CasheaOrderResponse>
                        {
                            Url = fullUrl,
                            IsSuccess = false,
                            StatusCode = (int)response.StatusCode,
                            Message = jsonResponse // Aquí Cashea explica por qué falló (ej: "Límite insuficiente")
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new CasheaResult<CasheaOrderResponse> { Url = fullUrl, IsSuccess = false, StatusCode = 500, Message = ex.Message };
                }
            }
        }

        public async Task<CasheaResult<bool>> SimularEscaneoQRAsync(string orderUuid)
        {
            if (string.IsNullOrEmpty(ConfigCashea.BaseUrl))
                return new CasheaResult<bool> { IsSuccess = false, StatusCode = 400, Message = "URL no configurada" };

            string fullUrl = $"{ConfigCashea.BaseUrl.TrimEnd('/')}/v2/orders/{orderUuid}/scan-qr";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"ApiKey {ConfigCashea.ApiKey}");

                try
                {
                    // Enviamos un contenido vacío (Empty Content)
                    var response = await client.PostAsync(fullUrl, new StringContent("", System.Text.Encoding.UTF8, "application/json"));
                    string contenido = await response.Content.ReadAsStringAsync();

                    return new CasheaResult<bool>
                    {
                        Url = fullUrl,
                        IsSuccess = response.IsSuccessStatusCode,
                        StatusCode = (int)response.StatusCode,
                        Message = response.IsSuccessStatusCode ? "Simulación de escaneo exitosa" : contenido,
                        Data = response.IsSuccessStatusCode
                    };
                }
                catch (Exception ex)
                {
                    return new CasheaResult<bool> { Url = fullUrl, IsSuccess = false, StatusCode = 500, Message = ex.Message };
                }
            }
        }

        public async Task<CasheaResult<CasheaPaymentPlanResponse>> GetPaymentPlanAsync(string orderUuid)
        {
            if (string.IsNullOrEmpty(ConfigCashea.BaseUrl))
                return new CasheaResult<CasheaPaymentPlanResponse> { IsSuccess = false, StatusCode = 400, Message = "URL no configurada" };

            string fullUrl = $"{ConfigCashea.BaseUrl.TrimEnd('/')}/v2/orders/{orderUuid}/payment-plan";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"ApiKey {ConfigCashea.ApiKey}");

                try
                {
                    var response = await client.GetAsync(fullUrl);
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var plan = JsonConvert.DeserializeObject<CasheaPaymentPlanResponse>(jsonResponse);
                        return new CasheaResult<CasheaPaymentPlanResponse>
                        {
                            Url = fullUrl,
                            IsSuccess = true,
                            StatusCode = (int)response.StatusCode,
                            Data = plan,
                            Message = "Plan de pagos obtenido"
                        };
                    }

                    return new CasheaResult<CasheaPaymentPlanResponse>
                    {
                        Url = fullUrl,
                        IsSuccess = false,
                        StatusCode = (int)response.StatusCode,
                        Message = jsonResponse // Cashea suele enviar el motivo del error aquí
                    };
                }
                catch (Exception ex)
                {
                    return new CasheaResult<CasheaPaymentPlanResponse> { Url = fullUrl, IsSuccess = false, StatusCode = 500, Message = ex.Message };
                }
            }
        }

        public async Task<CasheaResult<bool>> CancelarOrdenAsync(string orderUuid)
        {
            if (string.IsNullOrEmpty(ConfigCashea.BaseUrl))
                return new CasheaResult<bool> { IsSuccess = false, StatusCode = 400, Message = "URL no configurada" };

            string fullUrl = $"{ConfigCashea.BaseUrl.TrimEnd('/')}/v2/orders/{orderUuid}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"ApiKey {ConfigCashea.ApiKey}");

                try
                {
                    // Ejecutamos el DELETE
                    var response = await client.DeleteAsync(fullUrl);
                    string contenido = await response.Content.ReadAsStringAsync();

                    return new CasheaResult<bool>
                    {
                        Url = fullUrl,
                        IsSuccess = response.IsSuccessStatusCode,
                        StatusCode = (int)response.StatusCode,
                        Message = response.IsSuccessStatusCode ? "Orden cancelada exitosamente" : contenido,
                        Data = response.IsSuccessStatusCode
                    };
                }
                catch (Exception ex)
                {
                    return new CasheaResult<bool> { Url = fullUrl, IsSuccess = false, StatusCode = 500, Message = ex.Message };
                }
            }
        }

        public async Task<CasheaResult<bool>> ConfirmarPagoInicialAsync(string orderUuid, double monto)
        {
            if (string.IsNullOrEmpty(ConfigCashea.BaseUrl))
                return new CasheaResult<bool> { IsSuccess = false, StatusCode = 400, Message = "URL no configurada" };

            string fullUrl = $"{ConfigCashea.BaseUrl.TrimEnd('/')}/v2/orders/{orderUuid}/down-payment";
            var request = new { amount = monto };

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"ApiKey {ConfigCashea.ApiKey}");

                try
                {
                    string jsonBody = JsonConvert.SerializeObject(request);
                    var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(fullUrl, content);
                    string contenido = await response.Content.ReadAsStringAsync();

                    // CASO 1: Éxito total (200/201)
                    if (response.IsSuccessStatusCode)
                    {
                        return new CasheaResult<bool> { Url = fullUrl, IsSuccess = true, StatusCode = (int)response.StatusCode, Message = "Pago confirmado", Data = true };
                    }

                    // CASO 2: Error 400 - Posible pago ya realizado
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        var errorObj = JsonConvert.DeserializeObject<CasheaErrorResponse>(contenido);

                        // Si el mensaje es exactamente el que viste en Postman
                        if (errorObj?.Message == "Down payment already paid")
                        {
                            return new CasheaResult<bool>
                            {
                                Url = fullUrl,
                                IsSuccess = true, // <--- LO TRATAMOS COMO ÉXITO
                                StatusCode = 200,
                                Message = "El pago ya había sido confirmado previamente.",
                                Data = true
                            };
                        }
                    }

                    // CASO 3: Otros errores (401, 404, 500, etc.)
                    return new CasheaResult<bool> { Url = fullUrl, IsSuccess = false, StatusCode = (int)response.StatusCode, Message = contenido };
                }
                catch (Exception ex)
                {
                    return new CasheaResult<bool> { Url = fullUrl, IsSuccess = false, StatusCode = 500, Message = ex.Message };
                }
            }
        }

        public async Task<CasheaResult<CasheaOrderDetailsResponse>> GetOrderDetailsAsync(string orderUuid)
        {
            if (string.IsNullOrEmpty(ConfigCashea.BaseUrl))
                return new CasheaResult<CasheaOrderDetailsResponse> { IsSuccess = false, StatusCode = 400, Message = "URL no configurada" };

            string fullUrl = $"{ConfigCashea.BaseUrl.TrimEnd('/')}/v2/orders/{orderUuid}/details";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"ApiKey {ConfigCashea.ApiKey}");

                try
                {
                    var response = await client.GetAsync(fullUrl);
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return new CasheaResult<CasheaOrderDetailsResponse>
                        {
                            Url = fullUrl,
                            IsSuccess = true,
                            StatusCode = (int)response.StatusCode,
                            Data = JsonConvert.DeserializeObject<CasheaOrderDetailsResponse>(jsonResponse),
                            Message = "Detalles de la orden recuperados"
                        };
                    }

                    return new CasheaResult<CasheaOrderDetailsResponse>
                    {
                        Url = fullUrl,
                        IsSuccess = false,
                        StatusCode = (int)response.StatusCode,
                        Message = jsonResponse
                    };
                }
                catch (Exception ex)
                {
                    return new CasheaResult<CasheaOrderDetailsResponse> { Url = fullUrl, IsSuccess = false, StatusCode = 500, Message = ex.Message };
                }
            }
        }

        public async Task<CasheaResult<bool>> ActualizarFacturaAsync(string orderUuid, string numeroFactura)
        {
            if (string.IsNullOrEmpty(ConfigCashea.BaseUrl))
                return new CasheaResult<bool> { IsSuccess = false, StatusCode = 400, Message = "URL no configurada" };

            string fullUrl = $"{ConfigCashea.BaseUrl.TrimEnd('/')}/v2/orders/{orderUuid}";

            // Usamos un objeto anónimo para asegurar que el JSON lleve 'invoiceId'
            var requestData = new { invoiceId = numeroFactura };

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"ApiKey {ConfigCashea.ApiKey}");

                try
                {
                    string jsonBody = JsonConvert.SerializeObject(requestData);
                    var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

                    var request = new HttpRequestMessage(new HttpMethod("PATCH"), fullUrl) { Content = content };
                    var response = await client.SendAsync(request);
                    string respuestaApi = await response.Content.ReadAsStringAsync();

                    return new CasheaResult<bool>
                    {
                        Url = fullUrl,
                        IsSuccess = response.IsSuccessStatusCode,
                        StatusCode = (int)response.StatusCode,
                        Message = response.IsSuccessStatusCode ? "Factura vinculada exitosamente" : respuestaApi,
                        Data = response.IsSuccessStatusCode
                    };
                }
                catch (Exception ex)
                {
                    return new CasheaResult<bool> { Url = fullUrl, IsSuccess = false, StatusCode = 500, Message = ex.Message };
                }
            }
        }

        public async Task<CasheaResult<CasheaPaymentPlanResponse>> GetPaymentPlanByCodeAsync(string orderUuid, string userCode)
        {
            if (string.IsNullOrEmpty(ConfigCashea.BaseUrl))
                return new CasheaResult<CasheaPaymentPlanResponse> { IsSuccess = false, StatusCode = 400, Message = "URL no configurada" };

            string fullUrl = $"{ConfigCashea.BaseUrl.TrimEnd('/')}/v2/orders/{orderUuid}/payment-plan";
            var requestData = new { userCode = userCode }; // Asegúrate de que el JSON pida 'userCode' en minúscula

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"ApiKey {ConfigCashea.ApiKey}");

                try
                {
                    string jsonBody = JsonConvert.SerializeObject(requestData);
                    var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

                    // Nota: Este flujo usa POST según tu código, enviando el código en el body
                    var response = await client.PostAsync(fullUrl, content);
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return new CasheaResult<CasheaPaymentPlanResponse>
                        {
                            Url = fullUrl,
                            IsSuccess = true,
                            StatusCode = (int)response.StatusCode,
                            Data = JsonConvert.DeserializeObject<CasheaPaymentPlanResponse>(jsonResponse),
                            Message = "Código validado exitosamente"
                        };
                    }

                    return new CasheaResult<CasheaPaymentPlanResponse>
                    {
                        Url = fullUrl,
                        IsSuccess = false,
                        StatusCode = (int)response.StatusCode,
                        Message = jsonResponse // Aquí Cashea dirá si el código expiró o es inválido
                    };
                }
                catch (Exception ex)
                {
                    return new CasheaResult<CasheaPaymentPlanResponse> { Url = fullUrl, IsSuccess = false, StatusCode = 500, Message = ex.Message };
                }
            }
        }
    }
}
