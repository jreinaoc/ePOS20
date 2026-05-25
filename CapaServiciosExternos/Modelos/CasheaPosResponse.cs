using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CapaServiciosExternos.Modelos
{
    public class CasheaHealthResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }
    public class CasheaPosResponse
    {
        // Este atributo es vital porque el JSON de Cashea viene en camelCase (minúscula la primera)
        [JsonProperty("pointOfSales")]
        public List<PointOfSale> PointOfSales { get; set; }
    }

    public class PointOfSale
    {
        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class CasheaOrderRequest
    {
        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("invoiceId")]
        public string InvoiceId { get; set; }

        [JsonProperty("identificationNumber")]
        public string IdentificationNumber { get; set; }

        // Esta línea es la clave: si el string está vacío o nulo, NO se incluirá en el JSON
        [JsonProperty("employeeId", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string EmployeeId { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; } = "2.0.0";
    }

    public class CasheaOrderResponse
    {
        [JsonProperty("uuid")]
        public string Uuid { get; set; } // ID único de la orden en Cashea

        [JsonProperty("idNumber")]
        public int IdNumber { get; set; } // ID para reportes
    }

    public class CasheaPaymentPlanResponse
    {
        [JsonProperty("downPayment")]
        public double DownPayment { get; set; } // Monto de la inicial (Lo que pagará en tienda)

        [JsonProperty("financedAmount")]
        public double FinancedAmount { get; set; } // Lo que Cashea le financia

        [JsonProperty("downPaymentStatus")]
        public string DownPaymentStatus { get; set; } // "PENDING" o "PAID"
    }

    public class CasheaConfirmAmountRequest
    {
        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("fees")]
        public double Fees { get; set; } = 0; // Por defecto 0 si no aplica
    }

    public class CasheaOrderDetailsResponse
    {
        [JsonProperty("identifierNumber")]
        public string identifierNumber { get; set; }

        [JsonProperty("amount")]
        public double TotalAmount { get; set; }

        [JsonProperty("downPaymentAmount")]
        public double DownPaymentAmount { get; set; }

        [JsonProperty("financedAmount")]
        public double FinancedAmount { get; set; }

        [JsonProperty("status")]
        public OrderStatus Status { get; set; }

        [JsonProperty("paymentDetails")]
        public PaymentDetails Payment { get; set; }
    }

    public class OrderStatus
    {
        [JsonProperty("name")]
        public string Name { get; set; } // Ejemplo: "CANCELLED", "COMPLETED", "APPROVED"
    }

    public class PaymentDetails
    {
        [JsonProperty("user")]
        public CasheaUser User { get; set; }

        [JsonProperty("installments")]
        public List<Installment> Installments { get; set; }
    }

    public class CasheaUser
    {
        [JsonProperty("identificationNumber")]
        public string IdentificationNumber { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }
    }

    public class Installment
    {
        [JsonProperty("installmentNumber")]
        public int InstallmentNumber { get; set; } // 0 = Inicial, 1 = Cuota 1, etc.

        [JsonProperty("amount")]
        public double Amount { get; set; } // Monto de la cuota

        [JsonProperty("status")]
        public string Status { get; set; } // Estado (ej. CANCELLED, PAID, PENDING)

        [JsonProperty("scheduledPaymentDate")]
        public DateTime ScheduledPaymentDate { get; set; } // Fecha programada
    }

    public class CasheaUpdateInvoiceRequest
    {
        [JsonProperty("invoiceId")]
        public string InvoiceId { get; set; }
    }

    public class CasheaUserCodeRequest
    {
        [JsonProperty("userCode")]
        public string UserCode { get; set; }
    }

    public class CasheaResult<T>
    {
        public string Url { get; set; }
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; } // Aquí guardaremos la CasheaOrderResponse
    }
        public class CasheaErrorResponse
    {
        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
