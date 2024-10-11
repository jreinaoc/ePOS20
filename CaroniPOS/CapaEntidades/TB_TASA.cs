using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public static class TB_TASA_Dolar
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public static Int64 ID_Tasa { get; set; }

        [StringLength(3)]
        [Required]
        public static string Cod_Sucursal { get; set; }

        public static double? Tasa { get; set; }

        [StringLength(2)]
        public static string Cod_Moneda { get; set; }

        public static DateTime? FecCreacion { get; set; }

        [StringLength(5)]
        public static string USER_Crea { get; set; }


    }

    public static class TB_TASA_Euro
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public static Int64 ID_Tasa { get; set; }

        [StringLength(3)]
        [Required]
        public static string Cod_Sucursal { get; set; }

        public static double? Tasa { get; set; }

        [StringLength(2)]
        public static string Cod_Moneda { get; set; }

        public static DateTime? FecCreacion { get; set; }

        [StringLength(5)]
        public static string USER_Crea { get; set; }

    }
}
