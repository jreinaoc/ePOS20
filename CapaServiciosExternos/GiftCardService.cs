using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CapaServiciosExternos.Modelos; // Ajusta si el namespace de tus modelos es diferente
using CapaEntidades;

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
    }
}