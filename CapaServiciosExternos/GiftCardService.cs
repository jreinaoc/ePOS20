using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CapaServiciosExternos.Modelos; // Ajusta si el namespace de tus modelos es diferente

namespace CapaServiciosExternos
{
    public class GiftCardService
    {
        private readonly HttpClient _httpClient;

        // Credenciales del ambiente de Staging de WooCommerce
        private readonly string _consumerKey = "ck_e5b245e926df532ea642a065cc3d7ae5eee0b72e";
        private readonly string _consumerSecret = "cs_33c677c3e03f6474493a60d3eda744171070a3ea";

        public GiftCardService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            // URL Base del servidor de Staging
            _httpClient.BaseAddress = new Uri("https://elementor-dev.optiserver.co.uk/");

            // Configuración del Basic Auth (Convierte "usuario:contraseña" a Base64)
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_consumerKey}:{_consumerSecret}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            // Indicamos al servidor que esperamos la respuesta en formato JSON
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<GiftCardPosResponse> CreateGiftCardAsync(CreateGiftCardRequest request)
        {
            try
            {
                // 1. Serializar el objeto de entrada a JSON utilizando Newtonsoft.Json
                var jsonBody = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                // 2. Ejecutar la petición POST al endpoint correspondiente
                var response = await _httpClient.PostAsync("wp-json/wc/v3/gift-cards", content);

                // 3. Validar si la petición falló (Códigos que no sean 2xx)
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    // Aquí puedes mapear tus logs o lanzar la excepción tal como lo haces en Cashea
                    throw new HttpRequestException($"Error de respuesta en WooCommerce: {response.StatusCode} - {errorContent}");
                }

                // 4. Si es exitoso, leer el contenido y deserializarlo a tu objeto de respuesta
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<GiftCardPosResponse>(responseBody);
            }
            catch (Exception ex)
            {
                // Registra el error en tu sistema de logs habitual (Ej: log.Error(ex);)
                throw;
            }
        }

        /// <summary>
        /// Consulta una Gift Card existente en WooCommerce usando el mismo modelo unificado
        /// </summary>
        //public async Task<GiftCardPosResponse> ObtenerGiftCardPorIdAsync(int giftCardId)
        //{
        //    string url = $"{_baseUrl}gift-cards/{giftCardId}";

        //    HttpResponseMessage response = await _httpClient.GetAsync(url);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        string jsonString = await response.Content.ReadAsStringAsync();
        //        // Reutilizamos tu clase perfectamente
        //        return JsonConvert.DeserializeObject<GiftCardPosResponse>(jsonString);
        //    }
        //    else
        //    {
        //        string errorContent = await response.Content.ReadAsStringAsync();
        //        throw new HttpRequestException($"Error al consultar la Gift Card ({response.StatusCode}): {errorContent}");
        //    }
        //}
    }
}