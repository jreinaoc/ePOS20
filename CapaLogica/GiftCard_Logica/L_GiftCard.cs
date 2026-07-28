using System;
using System.Net.Http;
using System.Threading.Tasks;
using CapaServiciosExternos;
using CapaServiciosExternos.Modelos;
using CapaDatos.GiftCard_Datos;
using System.Data.SqlClient;
using System.Data;

namespace CapaLogica.GiftCard_Logica
{
    public class L_GiftCard
    {
        private D_GiftCard _D_GiftCard = new D_GiftCard();
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

        /// <summary>
        /// Lógica de negocio para consultar una Gift Card en WooCommerce por su ID
        /// </summary>
        public async Task<GiftCardResult<GiftCardPosResponse>> ConsultarGiftCardPorId(int giftCardId)
        {
            try
            {
                // 1. Configuraciones de seguridad TLS estándar de la red del POS
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                System.Net.ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) => true;

                // 2. Invocar el método asíncrono que creamos en la Capa de Servicios
                GiftCardPosResponse responseData = await _giftCardService.ObtenerGiftCardPorIdAsync(giftCardId);

                // 3. Validar si la API devolvió datos correctos
                if (responseData != null)
                {
                    return new GiftCardResult<GiftCardPosResponse>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Message = "Gift Card recuperada con éxito.",
                        Data = responseData
                    };
                }

                return new GiftCardResult<GiftCardPosResponse>
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "No se encontraron datos para la Gift Card especificada.",
                    Data = null
                };
            }
            catch (HttpRequestException httpEx)
            {
                // Captura fallos de credenciales Bad Request o 404 de la API web
                return new GiftCardResult<GiftCardPosResponse>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = $"Error de comunicación con WooCommerce: {httpEx.Message}",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                // Captura cualquier otro fallo general (red, timeout, etc.)
                return new GiftCardResult<GiftCardPosResponse>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = $"Error interno en el ePOS al consultar: {ex.Message}",
                    Data = null
                };
            }
        }

        /// <summary>
        /// Lógica de negocio para buscar una Gift Card usando su código alfanumérico
        /// </summary>
        public async Task<GiftCardResult<GiftCardPosResponse>> ConsultarGiftCardPorCodigo(string giftCardCode)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                // Invocamos el nuevo método de búsqueda por código
                GiftCardPosResponse responseData = await _giftCardService.ObtenerGiftCardPorCodigoAsync(giftCardCode);

                if (responseData != null)
                {
                    return new GiftCardResult<GiftCardPosResponse>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Message = "Gift Card localizada con éxito.",
                        Data = responseData // Aquí viaja el ID numérico que necesitaremos para el PUT posterior
                    };
                }

                return new GiftCardResult<GiftCardPosResponse>
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "El código de Gift Card ingresado no existe en el sistema.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new GiftCardResult<GiftCardPosResponse>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = $"Error interno en el ePOS: {ex.Message}",
                    Data = null
                };
            }
        }

        public bool AgregarGiftCard(string codSucursal, string nroOrden, string revision, decimal montoDolares, string nombreBeneficiario, string correoBeneficiario, string mensaje, int idGiftCard, string codigoGiftCard, bool activo, string userCrea, string userMod, SqlCommand command = null)
        {
            try
            {
                bool Respuesta = _D_GiftCard.AgregarGiftCard(codSucursal, nroOrden, revision, montoDolares, nombreBeneficiario, correoBeneficiario, mensaje, idGiftCard,codigoGiftCard, userCrea, userMod, activo , command);

                return Respuesta;
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Error general al actualizar TB_TRABAJO o rebajar inventario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        public DataTable ObtenerGiftCard(string codSucursal, string nroOrden, string revision, string codigoGiftCard, SqlCommand command = null)
        {
            DataTable dt = _D_GiftCard.ObtenerGiftCard(codSucursal, nroOrden, revision, codigoGiftCard);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return dt;
            }
        }

        /// <summary>
        /// Lógica de negocio para procesar el cobro/débito seguro de una Gift Card creando una orden en WooCommerce
        /// </summary>
        public async Task<GiftCardResult<bool>> DebitarSaldoGiftCard(string giftCardCode, decimal montoADebitar, int idProductoPos)
        {
            try
            {
                // 1. Forzar TLS 1.2 o superior por si el servidor externo rechaza conexiones SSL viejas
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                // 2. Omitir validación estricta de SSL temporalmente debido al bloqueo del Web Proxy / VPN
                System.Net.ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) => true;

                // 3. Invocar el método de débito transaccional en la Capa de Servicios
                bool exitoDebito = await _giftCardService.ProcesarDebitoGiftCardAsync(giftCardCode, montoADebitar, idProductoPos);

                if (exitoDebito)
                {
                    return new GiftCardResult<bool>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Message = "El monto de la Gift Card fue aplicado correctamente",
                        Data = true
                    };
                }

                return new GiftCardResult<bool>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "WooCommerce no pudo procesar la solicitud de cobro.",
                    Data = false
                };
            }
            catch (HttpRequestException httpEx)
            {
                // Captura errores específicos devueltos por la API web (como falta de saldo o pedido no editable)
                return new GiftCardResult<bool>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = $"Error de validación en la web: {httpEx.Message}",
                    Data = false
                };
            }
            catch (Exception ex)
            {
                // Captura fallas generales (Caídas de internet, Timeouts de la VPN, etc.)
                return new GiftCardResult<bool>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = $"Error interno en el ePOS al debitar la Gift Card: {ex.Message}",
                    Data = false
                };
            }
        }

        /// <summary>
        /// Lógica de negocio para eliminar una Gift Card usando su ID numérico
        /// </summary>
        public async Task<GiftCardResult<bool>> EliminarGiftCardPorId(int giftCardId)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                // Invocamos el método de eliminación por ID de tu GiftCardService
                bool isDeleted = await _giftCardService.EliminarGiftCardPorIdAsync(giftCardId);

                if (isDeleted)
                {
                    return new GiftCardResult<bool>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Message = "Gift Card eliminada con éxito",
                        Data = true
                    };
                }

                return new GiftCardResult<bool>
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "El ID de la Gift Card no existe o no pudo ser eliminada",
                    Data = false
                };
            }
            catch (Exception ex)
            {
                return new GiftCardResult<bool>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = $"Error interno en el ePOS al intentar eliminar: {ex.Message}",
                    Data = false
                };
            }
        }

        /// <summary>
        /// Lógica de negocio para anular/desactivar una Gift Card a partir de su código alfanumérico (Proceso en 2 pasos)
        /// </summary>
        public async Task<GiftCardResult<bool>> AnularGiftCardPorCodigo(string giftCardCode)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                // -------------------------------------------------------------
                // PASO 1: Consultar la Gift Card por su código alfanumérico
                // -------------------------------------------------------------
                GiftCardPosResponse cardData = await _giftCardService.ObtenerGiftCardPorCodigoAsync(giftCardCode);

                if (cardData == null)
                {
                    return new GiftCardResult<bool>
                    {
                        IsSuccess = false,
                        StatusCode = 404,
                        Message = "El código de Gift Card ingresado no existe en el sistema.",
                        Data = false
                    };
                }

                // Verificamos si la tarjeta ya se encontraba desactivada previamente
                if (cardData.IsActive  == "off")
                {
                    return new GiftCardResult<bool>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Message = "La Gift Card ya se encontraba anulada/inactiva previamente.",
                        Data = true
                    };
                }

                // Extraemos el ID numérico devuelto por la API (P. ej., 55)
                int giftCardId = cardData.Id;

                // -------------------------------------------------------------
                // PASO 2: Enviar el PUT usando el ID numérico para cambiar is_active a 'off'
                // -------------------------------------------------------------
                bool isDeactivated = await _giftCardService.AnularGiftCardPorIdAsync(giftCardId);

                if (isDeactivated)
                {
                    return new GiftCardResult<bool>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Message = "Gift Card anulada con éxito en WooCommerce.",
                        Data = true
                    };
                }

                return new GiftCardResult<bool>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = "No se pudo actualizar el estado de la Gift Card en el servidor web.",
                    Data = false
                };
            }
            catch (Exception ex)
            {
                return new GiftCardResult<bool>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = $"Error interno en el ePOS al intentar anular: {ex.Message}",
                    Data = false
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