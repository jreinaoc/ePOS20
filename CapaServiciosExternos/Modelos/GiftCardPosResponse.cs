using Newtonsoft.Json;
using System.Collections.Generic;

namespace CapaServiciosExternos.Modelos
{
    // 1. DTO de Entrada: Lo que envías a WooCommerce para crear la tarjeta
    public class CreateGiftCardRequest
    {
        [JsonProperty("recipient")]
        public string Recipient { get; set; }

        [JsonProperty("sender")]
        public string Sender { get; set; }

        [JsonProperty("balance")]
        public decimal Balance { get; set; }

        [JsonProperty("meta_data")]
        public List<GiftCardMeta> MetaData { get; set; } = new List<GiftCardMeta>(); // Arreglado para C# 7.3
    }

    // Sub-objeto para los metadatos
    public class GiftCardMeta
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    // 2. DTO de Salida: Lo que WooCommerce te devuelve (Ya unificado)
    public class GiftCardPosResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("balance")]
        public decimal Balance { get; set; }

        // 🌟 NUEVOS CAMPOS PARA EL FLUJO DE CONSULTA:

        [JsonProperty("remaining")]
        public decimal Remaining { get; set; } // El saldo disponible que le queda en la tienda

        [JsonProperty("is_active")]
        public string IsActive { get; set; } // Viene como "on" u "off" para saber si está válida

        [JsonProperty("recipient")]
        public string Recipient { get; set; } // El correo del beneficiario por si quieres validarlo
    }
}