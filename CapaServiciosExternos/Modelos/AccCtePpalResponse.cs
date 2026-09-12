using System;
using Newtonsoft.Json;

namespace CapaServiciosExternos.Modelos
{
    /// <summary>
    /// DTO de respuesta del endpoint GET /api/ACC_Temp/GetACC_TB_CTEPPAL/{CODSUCURSAL}/{Cedula}/{CTE_Nacio}
    /// (API WebApplication5 -> ConsolidadosACC).
    /// </summary>
    public class AccCtePpalResponse
    {
        [JsonProperty("codsucursal")]
        public string CodSucursal { get; set; }

        [JsonProperty("cedula")]
        public string Cedula { get; set; }

        [JsonProperty("ctE_Nacio")]
        public string CtENacio { get; set; }

        [JsonProperty("ult_Compra")]
        public DateTime? UltCompra { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("apellido")]
        public string Apellido { get; set; }

        [JsonProperty("tipVen")]
        public string TipVen { get; set; }

        [JsonProperty("tlfHab")]
        public string TlfHab { get; set; }

        [JsonProperty("tlfOfic")]
        public string TlfOfic { get; set; }

        [JsonProperty("tlfCel")]
        public string TlfCel { get; set; }

        [JsonProperty("facebook")]
        public string Facebook { get; set; }

        [JsonProperty("twitter")]
        public string Twitter { get; set; }

        [JsonProperty("instagram")]
        public string Instagram { get; set; }

        [JsonProperty("mail")]
        public string Mail { get; set; }

        [JsonProperty("ip_envio")]
        public string IpEnvio { get; set; }

        [JsonProperty("fechaRegistroWeb")]
        public DateTime? FechaRegistroWeb { get; set; }
    }

    /// <summary>
    /// Wrapper genérico de resultado, mismo patrón que CasheaResult&lt;T&gt;.
    /// </summary>
    public class ConsolidadosResult<T>
    {
        public string Url { get; set; }
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}