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

        [JsonProperty("message")]
        public string Message { get; set; }

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

    

    public class WooOrderRequest
    {
        [JsonProperty("status")]
        public string Status { get; set; } = "pending";

        [JsonProperty("customer_id")]
        public int CustomerId { get; set; } = 0;

        [JsonProperty("billing")]
        public WooBilling Billing { get; set; } = new WooBilling();

        [JsonProperty("line_items")]
        public List<WooLineItem> LineItems { get; set; } = new List<WooLineItem>();

        [JsonProperty("gift_cards")]
        public List<WooGiftCardPayment> GiftCards { get; set; } = new List<WooGiftCardPayment>();
    }

    public class WooBilling
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; } = "Venta ePOS";

        [JsonProperty("last_name")]
        public string LastName { get; set; } = "Punto de Venta";
    }

    public class WooLineItem
    {
        [JsonProperty("product_id")]
        public int ProductId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; } = 1;

        [JsonProperty("total")]
        public string Total { get; set; }
    }

    public class WooGiftCardPayment
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }
    }

    // Clase básica para capturar la respuesta y obtener el ID de la orden generada
    public class WooOrderResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}