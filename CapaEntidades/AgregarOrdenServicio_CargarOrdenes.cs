using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class AgregarOrdenServicio_CargarOrdenes
    {
        public string CodSucursal { get; set; }
        public string Revision { get; set; }
        public string CodVenta { get; set; }
        public string CteNacio { get; set; }
        public string CteCedIden { get; set; }
        public string NumExamen { get; set; }
        public string CodEmpleado { get; set; }
        public string CodLaboratorio { get; set; }
        public string CodServicio { get; set; }
        public string Vision { get; set; }
        public DateTime FecOfrecido { get; set; }
        public string HorOfrecido { get; set; }
        public DateTime? FecEntrega { get; set; }
        public DateTime? FecEnvio { get; set; }
        public decimal VtaSubTotal { get; set; }
        public decimal VtaImpuesto { get; set; }
        public decimal VtaDescuento { get; set; }
        public decimal VtaTotal { get; set; }
        public bool OrSerFinan { get; set; }
        public string OrSerStatus { get; set; }
        public string OrSerObserv { get; set; }
        public string UserCrea { get; set; }
        public DateTime Fecha { get; set; }
        public bool MonturaPropia { get; set; }
        public string CodDetVta { get; set; }
        public bool Aplica { get; set; }
        public string OtCorrespondiente { get; set; }
        public bool VentaAfil { get; set; }
        public bool CristalPropio { get; set; }
        public string TipoMonturaPropia { get; set; }
        public string CodMotivoReposicion { get; set; }
        public string CedulaCteAfil { get; set; }
        public string CodigoEmpAfil { get; set; }
        public bool? Asegurada { get; set; }
        public bool? Exonerada { get; set; }
        public bool MonturaEnQuorum { get; set; }
        public string CodColoracion { get; set; }
    }
}
