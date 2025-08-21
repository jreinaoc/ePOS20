using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class AgregarOrdenServicio_CargarOrdenes
    {
        public string Cod_Sucursal { get; set; }
        public string NumOrdserv { get; set; }
        public string Revision { get; set; }
        public string Cod_Venta { get; set; }
        public string CTE_Nacio { get; set; }
        public string CTE_CedIden { get; set; }
        public string NumExamen { get; set; }
        public string COD_EMPLEADO { get; set; }
        public string Cod_Laboratorio { get; set; }
             
        public string Cod_Servicio { get; set; }
        public string Vision { get; set; }
        public DateTime Fec_ofrecido { get; set; }
        public string Hor_ofrecido { get; set; }
        public DateTime? Fec_Entrega { get; set; }
        public DateTime? Fec_Envio { get; set; }
        public decimal VtaSubTotal { get; set; }
        public decimal VtaImpuesto { get; set; }
        public decimal VtaDescuento { get; set; }
        public decimal VtaTotal { get; set; }
        public string OrSer_Saldo { get; set; }
        public bool OrSer_Finan { get; set; }
        public string OrSer_Status { get; set; }
        public string OrSer_Observ { get; set; }
        public string User_Crea { get; set; }
        public DateTime Fecha { get; set; }
        public bool MonturaPropia { get; set; }
        public string Cod_DetVta { get; set; }
        public bool Aplica { get; set; }
        public string OTCORRESPONDIENTE { get; set; }
        public bool VentaAfil { get; set; }
        public bool CristalPropio { get; set; }
        public string TipoMonturaPropia { get; set; }
        public string CodMotivoReposicion { get; set; }
        public string Cedula_CteAfil { get; set; }
        public string Codigo_EmpAfil { get; set; }
        public bool? Asegurada { get; set; }
        public bool? Exonerada { get; set; }
        public bool MonturaEnQuorum { get; set; }
        public string Cod_Coloracion { get; set; }
        public string  Codmotivodes { get; set; }
        public decimal OrSer_Saldo_Mon { get; set; }

    }
}
