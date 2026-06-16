using System;
using System.Net.Http;
using System.Threading.Tasks;
using CapaServiciosExternos;
using CapaServiciosExternos.Modelos;

namespace CapaLogica.GiftCard_Logica
{
    public class L_GiftCard
    {
        // Instancia directa del servicio externo (siguiendo el patrón tradicional del POS)
        // Nota: Si usas HttpClient en C# 7.3, compartimos la instancia para evitar agotamiento de sockets
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly GiftCardService _giftCardService = new GiftCardService(_httpClient);

        /// <summary>
        /// Lógica de negocio para solicitar la creación de una Gift Card en WooCommerce
        /// </summary>
        public async Task<GiftCardResult<GiftCardPosResponse>> CrearNuevaGiftCard(CreateGiftCardRequest request)
        {
            try
            {
                // Forzar TLS 1.2 o superior por si el servidor externo rechaza conexiones SSL viejas en .NET Framework
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                // Omitir validación estricta de SSL temporalmente debido al bloqueo del Web Proxy / VPN
                System.Net.ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) => true;

                // Llamamos al servicio de la capa ServiciosExternos
                var resultadoApi = await _giftCardService.CreateGiftCardAsync(request);

                return new GiftCardResult<GiftCardPosResponse>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Data = resultadoApi,
                    Message = "Gift Card generada exitosamente."
                };
            }
            catch (HttpRequestException httpEx)
            {
                // Captura errores de respuesta HTTP (ej: 400 Bad Request, 401 Unauthorized de WooCommerce)
                return new GiftCardResult<GiftCardPosResponse>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = $"Error de API externa: {httpEx.Message}"
                };
            }
            catch (Exception ex)
            {
                // Captura fallas generales (ej: Pérdida de conexión, Timeout por la VPN)
                return new GiftCardResult<GiftCardPosResponse>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = $"Error interno en el ePOS al procesar Gift Card: {ex.Message}"
                };
            }
        }
    }

    /// <summary>
    /// Estructura genérica para devolver respuestas estandarizadas a la capa visual (Similar a CasheaResult)
    /// </summary>
    public class GiftCardResult<T>
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}