using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
	public class LIST_TB_USUARIO
	{
		[Key]
		[StringLength(5)]
		[Required]
		public string COD_USR { get; set; }

		[StringLength(25)]
		[Required]
		public string NOMBRE { get; set; }

		[StringLength(25)]
		[Required]
		public string USER_APELLIDO { get; set; }

		[StringLength(28)]
		[Required]
		public string USER_HASH { get; set; }

		[StringLength(25)]
		[Required]
		public string USER_CARGO { get; set; }

		[StringLength(1)]
		public string USER_ST { get; set; }

		[StringLength(5)]
		public string COD_EMPLEADO { get; set; }

		[StringLength(80)]
		public string Email { get; set; }

		[StringLength(3)]
		public string Id_Rol { get; set; }

		[StringLength(5)]
		public string Id_especial { get; set; }

		public DateTime USER_Fec_Crea { get; set; }

		public DateTime? USER_Fec_Modif { get; set; }

		[StringLength(5)]
		[Required]
		public string USER_Crea { get; set; }

		[StringLength(5)]
		public string USER_Modif { get; set; }

		[StringLength(3)]
		public string COD_SUCURSAL { get; set; }

		public double? EVasignado { get; set; }

		public bool? Certificado { get; set; }

		public bool Bloqueado { get; set; }

		public DateTime? Fecha_ActClave { get; set; }


	}

	public static class TB_USUARIO
	{
		[StringLength(5)]
		[Key]
		[Column(Order = 0)]
		[Required]
		public static string COD_USR { get; set; }

		[StringLength(25)]
		[Required]
		public static string USER_NOMBRE { get; set; }

		[StringLength(25)]
		[Required]
		public static string USER_APELLIDO { get; set; }

		[StringLength(28)]
		[Required]
		public static string USER_HASH { get; set; }

		[StringLength(25)]
		[Required]
		public static string USER_CARGO { get; set; }

		[StringLength(1)]
		public static string USER_ST { get; set; }

		[StringLength(5)]
		public static string COD_EMPLEADO { get; set; }

		[StringLength(80)]
		public static string Email { get; set; }

		[StringLength(3)]
		public static string Id_Rol { get; set; }

		[StringLength(5)]
		public static string Id_especial { get; set; }

		public static DateTime USER_Fec_Crea { get; set; }

		public static DateTime? USER_Fec_Modif { get; set; }

		[StringLength(5)]
		[Required]
		public static string USER_Crea { get; set; }

		[StringLength(5)]
		public static string USER_Modif { get; set; }

		[StringLength(3)]
		public static string COD_SUCURSAL { get; set; }

		public static double? EVasignado { get; set; }

		public static bool? Certificado { get; set; }

		public static bool Bloqueado { get; set; }

		public static DateTime? Fecha_ActClave { get; set; }

	}
}
