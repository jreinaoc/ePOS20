using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica.ExportarArchivos
{
    public class DatosEncabezado
    {
        public string OperacionNro { get; set; }
        public DateTime Fecha { get; set; }
        public System.Drawing.Image Logo { get; set; }
        public string RIF { get; set; }
        public string NombreORazonSocial { get; set; }
        public string NumeroFactura { get; set; }
        public string NumeroControl { get; set; }
        public string ImpresoraFiscal { get; set; }
        public string NumeroNotaDebito { get; set; }
        public string NumeroNotaCredito { get; set; }
        public string TipoTransaccion { get; set; }
        public string NumeroFacturaAfectada { get; set; }
        public decimal TotalVentasIncluyendoIVA { get; set; }
        public decimal VentasExentas { get; set; }
        public decimal VentasInternasNoGravadas { get; set; }
        public decimal VentasExoneradas { get; set; }
        public decimal VentasNoSujetas { get; set; }
        public decimal TotalNoGravadas { get; set; }
        public decimal VentasInternasGravadas { get; set; }
        public decimal AlicuotaGeneral { get; set; }
        public decimal BaseImponibleAlicuotaGeneral { get; set; }
        public decimal PorcentajeAlicuotaGeneral { get; set; }
        public decimal ImpuestoIVA_AlicuotaGeneral { get; set; }
        public decimal AlicuotaReducida { get; set; }
        public decimal BaseImponibleAlicuotaReducida { get; set; }
        public decimal PorcentajeAlicuotaReducida { get; set; }
        public decimal ImpuestoIVA_AlicuotaReducida { get; set; }
        public decimal AlicuotaGeneralAdicional { get; set; }
        public decimal BaseImponibleAlicuotaGeneralAdicional { get; set; }
        public decimal PorcentajeAlicuotaGeneralAdicional { get; set; }
        public decimal ImpuestoIVA_AlicuotaGeneralAdicional { get; set; }
        public decimal RetencionesIVA { get; set; }
        public string FacturasAfectadas { get; set; }
        public string NumeroComprobante { get; set; }
        public decimal IVARetenido { get; set; }

    }
}
