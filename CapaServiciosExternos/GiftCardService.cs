using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CapaServiciosExternos.Modelos; // Ajusta si el namespace de tus modelos es diferente
using CapaEntidades;
using System.Collections.Generic;

namespace CapaServiciosExternos
{
    public class GiftCardService
    {
        private readonly HttpClient _httpClient;

        // Credenciales del ambiente de Staging de WooCommerce
        //private readonly string _consumerKey = ConfigServiciosExternos.GiftCard_ConsumerKey; //"ck_e5b245e926df532ea642a065cc3d7ae5eee0b72e";
        //private readonly string _consumerSecret = ConfigServiciosExternos.GiftCard_ConsumerSecret;

        public GiftCardService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            // Indicamos al servidor que esperamos la respuesta en formato JSON
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<GiftCardPosResponse> CreateGiftCardAsync(CreateGiftCardRequest request)
        {
            try
            {
                // 2. 🌟 Inyectamos las credenciales dinámicas en las cabeceras justo antes de disparar
                string consumerKey = ConfigServiciosExternos.GiftCard_ConsumerKey;
                string consumerSecret = ConfigServiciosExternos.GiftCard_ConsumerSecret;

                var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{consumerKey}:{consumerSecret}"));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                // 3. 🌟 Construimos la URL completa dinámicamente usando el string de la BD (Como en Cashea)
                string urlBase = ConfigServiciosExternos.GiftCard_BaseUrl; // ej: https://elementor-dev.optiserver.co.uk/
                string fullUrl = $"{urlBase}wp-json/wc/v3/gift-cards";

                var jsonBody = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                // 4. Disparamos a la URL completa sin depender de BaseAddress
                var response = await _httpClient.PostAsync(fullUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Error de respuesta en WooCommerce: {response.StatusCode} - {errorContent}");
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<GiftCardPosResponse>(responseBody);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Consulta una Gift Card existente en WooCommerce usando la URL dinámica de la BD
        /// </summary>
        public async Task<GiftCardPosResponse> ObtenerGiftCardPorIdAsync(int giftCardId)
        {
            try
            {
                // 1. Inyectamos las credenciales dinámicas en las cabeceras en caliente
                string consumerKey = ConfigServiciosExternos.GiftCard_ConsumerKey;
                string consumerSecret = ConfigServiciosExternos.GiftCard_ConsumerSecret;

                var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{consumerKey}:{consumerSecret}"));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                // 2. Construimos la URL completa dinámica agregando el ID al final (GET wp-json/wc/v3/gift-cards/<id>)
                string urlBase = ConfigServiciosExternos.GiftCard_BaseUrl; // ej: https://elementor-dev.optiserver.co.uk/
                string fullUrl = $"{urlBase}wp-json/wc/v3/gift-cards/{giftCardId}";

                // 3. Ejecutamos la petición GET asíncrona
                var response = await _httpClient.GetAsync(fullUrl);

                // 4. Validamos si la API respondió con algún error
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Error al consultar la Gift Card ({response.StatusCode}): {errorContent}");
                }

                // 5. Deserializamos la respuesta usando tu misma clase unificada
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<GiftCardPosResponse>(responseBody);
            }
            catch (Exception ex)
            {
                // Registra el error si manejas logs o propágalo al formulario
                throw;
            }
        }

        /// <summary>
        /// Consulta una Gift Card existente en WooCommerce filtrando por su CÓDIGO alfanumérico
        /// </summary>
        /// <summary>
        /// Consulta una Gift Card existente en WooCommerce filtrando por coincidencia EXACTA de su código
        /// </summary>
        public async Task<GiftCardPosResponse> ObtenerGiftCardPorCodigoAsync(string giftCardCode)
        {
            try
            {
                // 1. Inyectamos credenciales en caliente
                string consumerKey = ConfigServiciosExternos.GiftCard_ConsumerKey;
                string consumerSecret = ConfigServiciosExternos.GiftCard_ConsumerSecret;

                var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{consumerKey}:{consumerSecret}"));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                // 2. Construimos la URL pasando el código como Query Parameter
                string urlBase = ConfigServiciosExternos.GiftCard_BaseUrl;
                string fullUrl = $"{urlBase}wp-json/wc/v3/gift-cards?code={Uri.EscapeDataString(giftCardCode)}";

                // 3. Ejecutamos el GET
                var response = await _httpClient.GetAsync(fullUrl);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Error al buscar por código ({response.StatusCode}): {errorContent}");
                }

                var responseBody = await response.Content.ReadAsStringAsync();

                // 4. Deserializamos la lista completa que arroja WooCommerce
                var listaGiftCards = JsonConvert.DeserializeObject<List<GiftCardPosResponse>>(responseBody);

                if (listaGiftCards != null && listaGiftCards.Count > 0)
                {
                    // 🌟 LA CLAVE DE LA SOLUCIÓN:
                    // Forzamos un filtrado estricto comparando el código limpio ignorando mayúsculas/minúsculas.
                    var tarjetaExacta = listaGiftCards.Find(gc =>
                        gc.Code.Trim().ToLower() == giftCardCode.Trim().ToLower()
                    );

                    // Si se encontró la coincidencia exacta, devolvemos esa.
                    if (tarjetaExacta != null)
                    {
                        return tarjetaExacta;
                    }
                }

                // Si la lista vino vacía o ninguna coincidió exactamente letra por letra, es null
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Procesa el cobro seguro de una Gift Card creando una orden en WooCommerce y cerrándola inmediatamente
        /// </summary>
        public async Task<bool> ProcesarDebitoGiftCardAsync(string giftCardCode, decimal montoADebitar, int idProductoPos)
        {
            try
            {
                // 1. Inyectamos las credenciales dinámicas en las cabeceras en caliente
                string consumerKey = ConfigServiciosExternos.GiftCard_ConsumerKey;
                string consumerSecret = ConfigServiciosExternos.GiftCard_ConsumerSecret;

                var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{consumerKey}:{consumerSecret}"));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                string urlBase = ConfigServiciosExternos.GiftCard_BaseUrl;
                string ordersUrl = $"{urlBase}wp-json/wc/v3/orders";

                // 2. Construimos la estructura exacta del JSON usando un objeto anónimo
                var ordenPayload = new
                {
                    status = "pending", // Requerido en "pending" para que el plugin procese el saldo (Editable)
                    customer_id = 0,
                    billing = new
                    {
                        first_name = "Venta ePOS",
                        last_name = "Punto de Venta"
                    },
                    line_items = new[]
                    {
                        new
                        {
                            product_id = idProductoPos, // El ID de producto configurado en tu sucursal
                            quantity = 1,
                            total = montoADebitar.ToString("F2", System.Globalization.CultureInfo.InvariantCulture) // Formato "0.00"
                        }
                    },
                    gift_cards = new[]
                    {
                        new
                        {
                            code = giftCardCode,
                            amount = montoADebitar.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)
                        }
                    }
                };

                var jsonBody = JsonConvert.SerializeObject(ordenPayload);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                // 3. Disparamos el POST inicial para crear la orden y restar el balance
                var response = await _httpClient.PostAsync(ordersUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Error al crear la orden de débito ({response.StatusCode}): {errorContent}");
                }

                // Deserializamos la respuesta dinámica para capturar el ID generado por la web
                var responseBody = await response.Content.ReadAsStringAsync();
                var ordenCreada = JsonConvert.DeserializeAnonymousType(responseBody, new { id = 0 });

                // 4. PASO DE CIERRE AUTOMÁTICO: Cambiamos el estado de la orden a "completed"
                if (ordenCreada != null && ordenCreada.id > 0)
                {
                    var cierrePayload = new { status = "completed" };
                    var jsonCierre = JsonConvert.SerializeObject(cierrePayload);
                    var contentCierre = new StringContent(jsonCierre, Encoding.UTF8, "application/json");

                    string putUrl = $"{ordersUrl}/{ordenCreada.id}";
                    var putResponse = await _httpClient.PutAsync(putUrl, contentCierre);

                    // Aunque falle el cambio a completada, el dinero ya se restó en el paso anterior. 
                    // Retornamos true porque el saldo ya se procesó de forma exitosa.
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                // Propaga el error o regístralo según las políticas de tu ePOS
                throw;
            }
        }
    }
}