using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades;

namespace CapaDatos.CargarOrdenes_Datos
{
    public class D_Articulos
    {
        Conexion.Conexion cn = new Conexion.Conexion();
        //El uso de la clase StringBuilder nos ayudara a devolver los mensajes 
        public readonly StringBuilder stringBuilder = new StringBuilder();

        public List <TB_ARTICULO> ObtenerArticulos(SqlCommand command = null)  // Trae el detalle del articulo 
        {
            // Declarar la lista para almacenar los resultados
            List<TB_ARTICULO> listaArticulos = new List<TB_ARTICULO>();
            stringBuilder.Clear();
            try
            {
                if (command == null)
                {
                SqlConnection connection = cn.LeerCadena();
                command = connection.CreateCommand();
                }

            SqlCommand cmd = command;
            cmd.CommandText = "SP_CPOS_GET_ARTICULO";
            cmd.CommandType = CommandType.StoredProcedure;

            // Ejecutar el comando y leer los resultados
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Mapear cada fila a un objeto TB_ARTICULO
                    TB_ARTICULO articulo = new TB_ARTICULO
                    {
                        CodSucursal = reader["CodSucursal"].ToString(),
                        CodArticulo = reader["CodArticulo"].ToString(),
                        TIPART = reader["TIPART"].ToString(),
                        CODGRUPO = reader["CODGRUPO"].ToString(),
                        MARCA = reader["MARCA"].ToString(),
                        MODELO = reader["MODELO"].ToString(),
                        PROVEEDOR = reader["PROVEEDOR"].ToString(),
                        DESART = reader["DESART"].ToString(),
                        CODPRO = reader["CODPRO"].ToString(),
                        ART_EXIST = reader["ART_EXIST"] != DBNull.Value ? Convert.ToInt32(reader["ART_EXIST"]) : 0,
                        MANEJAEXISTENCIA = reader["MANEJAEXISTENCIA"] != DBNull.Value && Convert.ToBoolean(reader["MANEJAEXISTENCIA"]),
                        ART_CANTRESERVA = reader["ART_CANTRESERVA"] != DBNull.Value ? Convert.ToInt32(reader["ART_CANTRESERVA"]) : 0,
                        ART_PVP = reader["ART_PVP"] != DBNull.Value ? Convert.ToDecimal(reader["ART_PVP"]) : 0,
                        PORCTDESCUENTO = reader["PORCTDESCUENTO"] != DBNull.Value ? (float?)Convert.ToSingle(reader["PORCTDESCUENTO"]) : null,
                        ART_STOCKMIN = reader["ART_STOCKMIN"] != DBNull.Value ? Convert.ToDecimal(reader["ART_STOCKMIN"]) : 0,
                        ART_STOCKMAX = reader["ART_STOCKMAX"] != DBNull.Value ? Convert.ToInt16(reader["ART_STOCKMAX"]) : (short)0,
                        CODUBI = reader["CODUBI"].ToString(),
                        COSTOULTI = reader["COSTOULTI"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["COSTOULTI"]) : null,
                        COSTOPROME = reader["COSTOPROME"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["COSTOPROME"]) : null,
                        ART_ACTIVO = reader["ART_ACTIVO"] != DBNull.Value && Convert.ToBoolean(reader["ART_ACTIVO"]),
                        PROMO = reader["PROMO"].ToString(),
                        NOELIMINA = reader["NOELIMINA"] != DBNull.Value && Convert.ToBoolean(reader["NOELIMINA"]),
                        ART_EXENTO = reader["ART_EXENTO"] != DBNull.Value && Convert.ToBoolean(reader["ART_EXENTO"]),
                        Fec_Crea = reader["Fec_Crea"] != DBNull.Value ? Convert.ToDateTime(reader["Fec_Crea"]) : DateTime.MinValue,
                        Fec_Modif = reader["Fec_Modif"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["Fec_Modif"]) : null,
                        USER_CREA = reader["USER_CREA"].ToString(),
                        USER_MOD = reader["USER_MOD"].ToString(),
                        ServicioVisual = reader["ServicioVisual"] != DBNull.Value && Convert.ToBoolean(reader["ServicioVisual"]),
                        MHorizontal = reader["MHorizontal"] != DBNull.Value ? (int?)Convert.ToInt32(reader["MHorizontal"]) : null,
                        MVertical = reader["MVertical"] != DBNull.Value ? (int?)Convert.ToInt32(reader["MVertical"]) : null,
                        MMaxima = reader["MMaxima"] != DBNull.Value ? (int?)Convert.ToInt32(reader["MMaxima"]) : null,
                        MPuente = reader["MPuente"] != DBNull.Value ? (int?)Convert.ToInt32(reader["MPuente"]) : null,
                        CristalAlturaMin = reader["CristalAlturaMin"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["CristalAlturaMin"]) : null,
                        CristalAlturaMax = reader["CristalAlturaMax"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["CristalAlturaMax"]) : null,
                        CristalEsfMin = reader["CristalEsfMin"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["CristalEsfMin"]) : null,
                        CristalEsfMax = reader["CristalEsfMax"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["CristalEsfMax"]) : null,
                        CristalCilMin = reader["CristalCilMin"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["CristalCilMin"]) : null,
                        CristalCilMax = reader["CristalCilMax"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["CristalCilMax"]) : null,
                        CristalRangoMin = reader["CristalRangoMin"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["CristalRangoMin"]) : null,
                        CristalRangoMax = reader["CristalRangoMax"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["CristalRangoMax"]) : null,
                        CristalAdicionMin = reader["CristalAdicionMin"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["CristalAdicionMin"]) : null,
                        CristalAdicionMax = reader["CristalAdicionMax"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["CristalAdicionMax"]) : null,
                        CodRango = reader["CodRango"].ToString(),
                        CodGrupoRango = reader["CodGrupoRango"].ToString(),
                        DescripcionColor = reader["DescripcionColor"].ToString(),
                        tieneTraza = reader["tieneTraza"] != DBNull.Value && Convert.ToBoolean(reader["tieneTraza"]),
                        permiteTraza = reader["permiteTraza"] != DBNull.Value && Convert.ToBoolean(reader["permiteTraza"]),
                        tamano = reader["tamano"].ToString(),
                        codColor = reader["codColor"].ToString(),
                        PrecioEnDolares = reader["PrecioEnDolares"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["PrecioEnDolares"]) : null
                    };


                        // Agregar el objeto a la lista
                        listaArticulos.Add(articulo);
                }
            }

                return listaArticulos;
          }
          catch (Exception ex)
          {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
           }
        }
    }
}
