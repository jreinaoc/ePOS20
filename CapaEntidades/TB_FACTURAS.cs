using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class TB_FACTURAS
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
        public static string Fact_Num { get; set; }

        [Column(TypeName = "smalldatetime")]
        public static DateTime? Fecha { get; set; }

        [StringLength(9)]
        public static string Fact_NumCtrol { get; set; }

        [Required]
        [StringLength(1)]
        public static string CTE_NacioPAG { get; set; }

        [Required]
        [StringLength(10)]
        public static string CTE_CedIdenPAG { get; set; }

        [Required]
        [StringLength(5)]
        public static string COD_Empleado { get; set; }

        [Required]
        [StringLength(3)]
        public static string COD_VTA { get; set; }

        [StringLength(7)]
        [Required]
        public static string NumOrdServ { get; set; }

        [StringLength(1)]
        [Required]
        public static string Revision { get; set; }

        [Column(TypeName = "smalldatetime")]
        [Required]
        public static DateTime Fact_FecOfecido { get; set; }

        [StringLength(13)]
        [Required]
        public static string Fact_HoraOfrecido { get; set; }

        [Required]
        public static double Fact_SubTotal { get; set; }

        public static double Fact_Impuesto { get; set; }

        [Required]
        public static double Fact_Descuento { get; set; }

        [Required]
        public static double Fact_Total { get; set; }

        [StringLength(10)]
        [Required]
        public static string Fact_Status { get; set; }

        [Column(TypeName = "smalldatetime")]
        [Required]
        public static DateTime Fact_FecCrea { get; set; }

        [Column(TypeName = "smalldatetime")]
        public static DateTime? Fact_FecMod { get; set; }

        [StringLength(5)]
        [Required]
        public static string USER_Crea { get; set; }

        [StringLength(5)]
        public static string USER_Mod { get; set; }

        public static bool? Anulado { get; set; }

        public static bool? Nota { get; set; }

        public static double? IvaRetenido { get; set; }

        [StringLength(25)]
        public static string ComprobRetencionIva { get; set; }

        public static double? ISLRRetenido { get; set; }

        [StringLength(25)]
        public static string ComprobRetencionISLR { get; set; }

        [Column(TypeName = "smalldatetime")]
        public static DateTime? FechaRegistroComprobISLR { get; set; }

        [Column(TypeName = "smalldatetime")]
        public static DateTime? FechaRegistroComprobIVA { get; set; }

        [StringLength(2)]
        public static string Cod_TipoNControl { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        [Required]
        public static string Fact_SerialImpresora { get; set; }

        public static double? Fact_MontoExento { get; set; }

        public static double? Fact_MontoGravable { get; set; }

        public static double? Fact_AlicuotaIva { get; set; }

        [StringLength(20)]
        public static string NCF { get; set; }

        [StringLength(2)]
        public static string CodDocVta { get; set; }

        public static double? MontoReintegroIva { get; set; }

        public static double? Fact_IGTF { get; set; }

        public static double? Fact_AlicuotaIGTF { get; set; }

    }
}
