using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class List_TB_CAORDSER
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(3)]
        [Required]
        public string Cod_Sucursal { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(7)]
        [Required]
        public string NumOrdserv { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(1)]
        [Required]
        public string Revision { get; set; }

        [Required]
        [Column(TypeName = "smalldatetime")]
        public DateTime Fecha { get; set; }

        [Required]
        [StringLength(3)]
        public string Cod_Venta { get; set; }

        [Required]
        [StringLength(1)]
        public string CTE_Nacio { get; set; }

        [Required]
        [StringLength(10)]
        public string CTE_CedIden { get; set; }

        [StringLength(5)]
        public int? NumExamen { get; set; }

        [Required]
        [StringLength(5)]
        public string COD_EMPLEADO { get; set; }

        [StringLength(11)]
        public string Cod_Laboratorio { get; set; }

        [StringLength(3)]
        public string Cod_Servicio { get; set; }

        [StringLength(10)]
        public string Vision { get; set; }

        [Column(TypeName = "smalldatetime")]
        [Required]
        public DateTime Fec_ofrecido { get; set; }

        [StringLength(13)]
        public string Hor_ofrecido { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Fec_Entrega { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Fec_Envio { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Fec_Recibido { get; set; }

        [Required]
        public double VtaSubTotal { get; set; }

        [Required]
        public double VtaImpuesto { get; set; }

        [Required]
        public double VtaDescuento { get; set; }

        [Required]
        public double VtaTotal { get; set; }

        [Required]
        public double OrSer_Saldo { get; set; }

        [Required]
        public bool OrSer_Finan { get; set; }

        [StringLength(3)]
        [Required]
        public string OrSer_Status { get; set; }

        [StringLength(100)]
        public string OrSer_Observ { get; set; }

        [StringLength(2)]
        [Required]
        public string CodCausa { get; set; }

        public bool? MonturaPropia { get; set; }

        [Column(TypeName = "smalldatetime")]
        [Required]
        public DateTime OrSer_fecCrea { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? OrSer_FecMod { get; set; }

        [StringLength(5)]
        [Required]
        public string USER_Crea { get; set; }

        [StringLength(5)]
        public string USER_Mod { get; set; }

        [StringLength(2)]
        public string Cod_DetVta { get; set; }

        public bool? Aplica { get; set; }

        [StringLength(7)]
        public string OTCORRESPONDIENTE { get; set; }

        public bool? Anulado { get; set; }

        public bool? Ventaafil { get; set; }

        [Required]
        public bool cristalpropio { get; set; }

        public bool? Nota { get; set; }

        public int? Cuantas { get; set; }

        [StringLength(3)]
        public string CodMotivoAnul { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FechaAnulacion { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FechaCaja { get; set; }

        [StringLength(5)]
        public string Cod_ResponsableRev { get; set; }

        [StringLength(5)]
        public string Cod_ResponsableAnu { get; set; }

        public bool? Asegurada { get; set; }

        public bool? Exonerada { get; set; }

        [StringLength(2)]
        public string TipoMonturaPropia { get; set; }

        [StringLength(3)]
        public string CodMotivoReposicion { get; set; }

        [StringLength(10)]
        public string Cedula_CteAfil { get; set; }

        [StringLength(10)]
        public string Codigo_EmpAfil { get; set; }

        public bool? MonturaEnQuorum { get; set; }

        [StringLength(6)]
        public string Cod_Coloracion { get; set; }

        [StringLength(7)]
        public string OS_Externa { get; set; }

        public double? OrSer_Saldo_Mon { get; set; }

        [StringLength(2)]
        public string OrSer_Tipo_Mon { get; set; }

        public double? Orser_Total_Mon { get; set; }

        public double? VtaImpuestoIGTF { get; set; }


    }


    public class TB_CAORDSER
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(3)]
        [Required]
        public static string Cod_Sucursal { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(7)]
        [Required]
        public static string NumOrdserv { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(1)]
        [Required]
        public static string Revision { get; set; }

        [Required]
        [Column(TypeName = "smalldatetime")]
        public static DateTime Fecha { get; set; }

        [Required]
        [StringLength(3)]
        public static string Cod_Venta { get; set; }

        [Required]
        [StringLength(1)]
        public static string CTE_Nacio { get; set; }

        [Required]
        [StringLength(10)]
        public static string CTE_CedIden { get; set; }

        [StringLength(5)]
        public static int? NumExamen { get; set; }

        [Required]
        [StringLength(5)]
        public static string COD_EMPLEADO { get; set; }

        [StringLength(11)]
        public static string Cod_Laboratorio { get; set; }

        [StringLength(3)]
        public static string Cod_Servicio { get; set; }

        [StringLength(10)]
        public static string Vision { get; set; }

        [Column(TypeName = "smalldatetime")]
        [Required]
        public static DateTime Fec_ofrecido { get; set; }

        [StringLength(13)]
        public static string Hor_ofrecido { get; set; }

        [Column(TypeName = "smalldatetime")]
        public static DateTime? Fec_Entrega { get; set; }

        [Column(TypeName = "smalldatetime")]
        public static DateTime? Fec_Envio { get; set; }

        [Column(TypeName = "smalldatetime")]
        public static DateTime? Fec_Recibido { get; set; }

        [Required]
        public static double VtaSubTotal { get; set; }

        [Required]
        public static double VtaImpuesto { get; set; }

        [Required]
        public static double VtaDescuento { get; set; }

        [Required]
        public static double VtaTotal { get; set; }

        [Required]
        public static double OrSer_Saldo { get; set; }

        [Required]
        public static bool OrSer_Finan { get; set; }

        [StringLength(3)]
        [Required]
        public static string OrSer_Status { get; set; }

        [StringLength(100)]
        public static string OrSer_Observ { get; set; }

        [StringLength(2)]
        [Required]
        public static string CodCausa { get; set; }

        public static bool? MonturaPropia { get; set; }

        [Column(TypeName = "smalldatetime")]
        [Required]
        public static DateTime OrSer_fecCrea { get; set; }

        [Column(TypeName = "smalldatetime")]
        public static DateTime? OrSer_FecMod { get; set; }

        [StringLength(5)]
        [Required]
        public static string USER_Crea { get; set; }

        [StringLength(5)]
        public static string USER_Mod { get; set; }

        [StringLength(2)]
        public static string Cod_DetVta { get; set; }

        public static bool? Aplica { get; set; }

        [StringLength(7)]
        public static string OTCORRESPONDIENTE { get; set; }

        public static bool? Anulado { get; set; }

        public static bool? Ventaafil { get; set; }

        [Required]
        public static bool cristalpropio { get; set; }

        public static bool? Nota { get; set; }

        public static int? Cuantas { get; set; }

        [StringLength(3)]
        public static string CodMotivoAnul { get; set; }

        [Column(TypeName = "smalldatetime")]
        public static DateTime? FechaAnulacion { get; set; }

        [Column(TypeName = "smalldatetime")]
        public static DateTime? FechaCaja { get; set; }

        [StringLength(5)]
        public static string Cod_ResponsableRev { get; set; }

        [StringLength(5)]
        public static string Cod_ResponsableAnu { get; set; }

        public static bool? Asegurada { get; set; }

        public static bool? Exonerada { get; set; }

        [StringLength(2)]
        public static string TipoMonturaPropia { get; set; }

        [StringLength(3)]
        public static string CodMotivoReposicion { get; set; }

        [StringLength(10)]
        public static string Cedula_CteAfil { get; set; }

        [StringLength(10)]
        public static string Codigo_EmpAfil { get; set; }

        public static bool? MonturaEnQuorum { get; set; }

        [StringLength(6)]
        public static string Cod_Coloracion { get; set; }

        [StringLength(7)]
        public static string OS_Externa { get; set; }

        public static double? OrSer_Saldo_Mon { get; set; }

        [StringLength(2)]
        public static string OrSer_Tipo_Mon { get; set; }

        public static double? Orser_Total_Mon { get; set; }

        public static double? VtaImpuestoIGTF { get; set; }


    }

}
