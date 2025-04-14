using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class TB_ARTICULO
    {
        public string CodSucursal { get; set; }
        public string CodArticulo { get; set; }
        public string TIPART { get; set; }
        public string CODGRUPO { get; set; }
        public string MARCA { get; set; }
        public string MODELO { get; set; }
        public string PROVEEDOR { get; set; }
        public string DESART { get; set; }
        public string CODPRO { get; set; }
        public int ART_EXIST { get; set; }
        public bool MANEJAEXISTENCIA { get; set; }
        public int ART_CANTRESERVA { get; set; }
        public decimal ART_PVP { get; set; }
        public float? PORCTDESCUENTO { get; set; }
        public decimal ART_STOCKMIN { get; set; }
        public short ART_STOCKMAX { get; set; }
        public string CODUBI { get; set; }
        public decimal? COSTOULTI { get; set; }
        public decimal? COSTOPROME { get; set; }
        public bool ART_ACTIVO { get; set; }
        public string PROMO { get; set; }
        public bool? NOELIMINA { get; set; }
        public bool ART_EXENTO { get; set; }
        public DateTime Fec_Crea { get; set; }
        public DateTime? Fec_Modif { get; set; }
        public string USER_CREA { get; set; }
        public string USER_MOD { get; set; }
        public bool ServicioVisual { get; set; }
        public int? MHorizontal { get; set; }
        public int? MVertical { get; set; }
        public int? MMaxima { get; set; }
        public int? MPuente { get; set; }
        public decimal? CristalAlturaMin { get; set; }
        public decimal? CristalAlturaMax { get; set; }
        public decimal? CristalEsfMin { get; set; }
        public decimal? CristalEsfMax { get; set; }
        public decimal? CristalCilMin { get; set; }
        public decimal? CristalCilMax { get; set; }
        public decimal? CristalRangoMin { get; set; }
        public decimal? CristalRangoMax { get; set; }
        public decimal? CristalAdicionMin { get; set; }
        public decimal? CristalAdicionMax { get; set; }
        public string CodRango { get; set; }
        public string CodGrupoRango { get; set; }
        public string DescripcionColor { get; set; }
        public bool tieneTraza { get; set; }
        public bool permiteTraza { get; set; }
        public string tamano { get; set; }
        public string codColor { get; set; }
        public decimal? PrecioEnDolares { get; set; }
    }
}
