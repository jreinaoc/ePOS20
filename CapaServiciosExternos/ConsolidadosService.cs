using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CapaServiciosExternos.Modelos;
using CapaEntidades;

namespace CapaServiciosExternos
{
    /// <summary>
    /// Servicio de consulta al consolidado de clientes (API WebApplication5 -> ConsolidadosACC).
    /// Sigue el mismo patrón que CasheaService / GiftCardService.
    /// </summary>
    public class ConsolidadosService
    {
        private const string GetClienteEndpoint = "/api/ACC_Temp/GetACC_TB_CTEPPAL/";
        private const string ActualizarClienteEndpoint = "/api/ACC_Temp/ActualizarACC_TB_CTEPPAL/";
        private const string CrearClienteEndpoint = "/api/ACC_Temp/ACC_TB_CTEPPAL";

        /// <summary>
        /// [GET] Busca un cliente en el consolidado por cédula + nacionalidad.
        /// Retorna el registro con MAX(FechaRegistroWeb).
        /// 200 -> IsSuccess=true con Data; 404 -> IsSuccess=false; error de red/timeout -> StatusCode=500.
        /// </summary>
        public async Task<ConsolidadosResult<AccCtePpalResponse>> GetClienteAsync(string cedula, string nacio)
        {
            // 1. Si no hay URL configurada, devolvemos un error controlado 400
            if (string.IsNullOrEmpty(ConfigServiciosExternos.Consolidados_BaseUrl))
            {
                return new ConsolidadosResult<AccCtePpalResponse>
                {
                    Url = "",
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Consolidados_BaseUrl no configurada"
                };
            }

            // 2. Validar que ninguno de los parámetros de la ruta venga vacío
            if (string.IsNullOrEmpty(cedula) || string.IsNullOrEmpty(nacio))
            {
                return new ConsolidadosResult<AccCtePpalResponse>
                {
                    Url = "",
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Parámetros incompletos (cedula/nacio)"
                };
            }

            string fullUrl = $"{ConfigServiciosExternos.Consolidados_BaseUrl.TrimEnd('/')}{GetClienteEndpoint}{cedula}/{nacio}";

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(15);

                try
                {
                    var response = await client.GetAsync(fullUrl);
                    string contenido = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return new ConsolidadosResult<AccCtePpalResponse>
                        {
                            Url = fullUrl,
                            IsSuccess = true,
                            StatusCode = (int)response.StatusCode,
                            Message = "Cliente encontrado en consolidado",
                            Data = JsonConvert.DeserializeObject<AccCtePpalResponse>(contenido)
                        };
                    }

                    // 404 u otro código -> no encontrado / error de la API
                    return new ConsolidadosResult<AccCtePpalResponse>
                    {
                        Url = fullUrl,
                        IsSuccess = false,
                        StatusCode = (int)response.StatusCode,
                        Message = contenido
                    };
                }
                catch (Exception ex)
                {
                    // Servidor caído, DNS fail, timeout...
                    return new ConsolidadosResult<AccCtePpalResponse>
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
        /// [PUT] Actualiza un cliente existente en el consolidado.
        /// Busca por cédula + nacionalidad (registro con MAX(FechaRegistroWeb)).
        /// El body se envía COMPLETO (todos los atributos editables) porque la API sobrescribe
        /// cada campo con el valor del body; los campos no enviados quedarían en null.
        /// 200 -> IsSuccess=true; 404 -> IsSuccess=false; error de red/timeout -> StatusCode=500.
        /// </summary>
        public async Task<ConsolidadosResult<AccCtePpalResponse>> ActualizarClienteAsync(string cedula, string nacio, AccCtePpalResponse cliente)
        {
            // 1. Si no hay URL configurada, devolvemos un error controlado 400
            if (string.IsNullOrEmpty(ConfigServiciosExternos.Consolidados_BaseUrl))
            {
                return new ConsolidadosResult<AccCtePpalResponse>
                {
                    Url = "",
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Consolidados_BaseUrl no configurada"
                };
            }

            // 2. Validar parámetros de la ruta y el body
            if (string.IsNullOrEmpty(cedula) || string.IsNullOrEmpty(nacio) || cliente == null)
            {
                return new ConsolidadosResult<AccCtePpalResponse>
                {
                    Url = "",
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Parámetros incompletos para actualizar (cedula/nacio/cliente)"
                };
            }

            string fullUrl = $"{ConfigServiciosExternos.Consolidados_BaseUrl.TrimEnd('/')}{ActualizarClienteEndpoint}{cedula}/{nacio}";

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(15);

                try
                {
                    // Serializa el DTO completo respetando los [JsonProperty] en camelCase
                    string json = JsonConvert.SerializeObject(cliente);
                    var contenido = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PutAsync(fullUrl, contenido);
                    string bodyRespuesta = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return new ConsolidadosResult<AccCtePpalResponse>
                        {
                            Url = fullUrl,
                            IsSuccess = true,
                            StatusCode = (int)response.StatusCode,
                            Message = bodyRespuesta,
                            Data = cliente
                        };
                    }

                    return new ConsolidadosResult<AccCtePpalResponse>
                    {
                        Url = fullUrl,
                        IsSuccess = false,
                        StatusCode = (int)response.StatusCode,
                        Message = bodyRespuesta
                    };
                }
                catch (Exception ex)
                {
                    return new ConsolidadosResult<AccCtePpalResponse>
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
        /// [POST] Crea un cliente nuevo en el consolidado (ACC_TB_CTEPPAL).
        /// La API devuelve:
        ///   200 -> insertado correctamente
        ///   409 -> ya existe (violación de clave primaria)
        ///   500 -> error del servidor
        /// </summary>
        public async Task<ConsolidadosResult<AccCtePpalResponse>> CrearClienteAsync(AccCtePpalResponse cliente)
        {
            // 1. Si no hay URL configurada, devolvemos un error controlado 400
            if (string.IsNullOrEmpty(ConfigServiciosExternos.Consolidados_BaseUrl))
            {
                return new ConsolidadosResult<AccCtePpalResponse>
                {
                    Url = "",
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Consolidados_BaseUrl no configurada"
                };
            }

            // 2. Validar que el body no venga vacío
            if (cliente == null)
            {
                return new ConsolidadosResult<AccCtePpalResponse>
                {
                    Url = "",
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Parámetros incompletos para crear cliente (cliente)"
                };
            }

            string fullUrl = $"{ConfigServiciosExternos.Consolidados_BaseUrl.TrimEnd('/')}{CrearClienteEndpoint}";

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(15);

                try
                {
                    // Serializa el DTO completo respetando los [JsonProperty] en camelCase
                    string json = JsonConvert.SerializeObject(cliente);
                    var contenido = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(fullUrl, contenido);
                    string bodyRespuesta = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return new ConsolidadosResult<AccCtePpalResponse>
                        {
                            Url = fullUrl,
                            IsSuccess = true,
                            StatusCode = (int)response.StatusCode,
                            Message = bodyRespuesta,
                            Data = cliente
                        };
                    }

                    return new ConsolidadosResult<AccCtePpalResponse>
                    {
                        Url = fullUrl,
                        IsSuccess = false,
                        StatusCode = (int)response.StatusCode,
                        Message = bodyRespuesta
                    };
                }
                catch (Exception ex)
                {
                    return new ConsolidadosResult<AccCtePpalResponse>
                    {
                        Url = fullUrl,
                        IsSuccess = false,
                        StatusCode = 500,
                        Message = "Error de conexión o Timeout: " + ex.Message
                    };
                }
            }
        }
    }
}