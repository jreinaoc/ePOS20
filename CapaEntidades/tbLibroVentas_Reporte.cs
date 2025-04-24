using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class tbLibroVentas_Reporte
    {
        public DateTime Fecha { get; set; }
        public string Rif_Cedula { get; set; }
        public string Nombre_RazonSocial { get; set; }
        public string NumeroFactura { get; set; }
        public string NumeroControl { get; set; }
        public string ImpresoraFiscal { get; set; }
        public string NotaDebito { get; set; }
        public string NotaCredito { get; set; }
        public string TipoTransaccion { get; set; }
        public string FacturaAfectada { get; set; }
        public decimal TotalVentas_Iva { get; set; }
        public decimal VentasExentas { get; set; }
        public string VentasExoneradas { get; set; }
        public string VentaNoSujetas { get; set; }
        public decimal TotalNoGravadas { get; set; }
        public decimal BaseImponible { get; set; }
        public decimal Alicuota { get; set; }
        public decimal ImpuestoIVA { get; set; }
        public string BaseImponibleReducida { get; set; }
        public string AlicuotaReducida { get; set; }
        public string ImpuestoIVAReducido { get; set; }
        public string BaseImponibleAdicional { get; set; }
        public string AlicuotaAdicional { get; set; }
        public string ImpuestoIVAAdicional { get; set; }
        public string FechaRetencion { get; set; }
        public string FacturaAfectadaRetencion { get; set; }
        public decimal IVARetenido { get; set; }
        public string ComprobanteRetencion { get; set; }


   
        ///////////

        public bool EsTotal { get; set; } = false;
        public string TextoTotal { get; set; }
        public decimal TotalSumaVentas_Iva { get; set; }
        public decimal TotalSumaVentasExentas { get; set; }
        public decimal TotalSumaNoGravadas { get; set; }
        public decimal TotalSumaBaseImponible { get; set; }
        public decimal TotalSumaImpuestoIVA { get; set; }


    }

}
