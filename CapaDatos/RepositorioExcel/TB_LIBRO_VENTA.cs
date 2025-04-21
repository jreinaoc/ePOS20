using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades;
using System.Windows.Forms;


namespace CapaDatos.RepositorioExcel
{
    public class TB_LIBRO_VENTA
    {
        Conexion.Conexion cn = new Conexion.Conexion();

        public async Task<List<repositorioLibro_Venta>> ObtenerRegistros(FiltroFechas_LibroVenta filtroFechas)
        {
            var lista = new List<repositorioLibro_Venta>();

            using (SqlConnection connection = cn.LeerCadena())
            {
                using (SqlCommand cmd = new SqlCommand(@"
               SELECT Fecha, Fact_Num, Fact_NumCtrol, Fact_SubTotal, Fact_Impuesto, Fact_Descuento, Fact_Total,
               CTE_CedIden, CTE_PNombre, CTE_PApellido, CTE_Nacio, Cod_Sucursal, NRONOTA, PorcImpuesto,
               Anulado, Nota, ComprobRetencionIVA, IvaRetenido, Fact_SerialImpresora, NC_SerialImpresora,
               Fact_MontoExento, Fact_MontoGravable, Fact_AlicuotaIva, NC_MontoExento, NC_MontoGravable,
               NC_AlicuotaIva, NC_MontoIva, NC_MontoTotal, Fact_ImpuestoIGTF, PorcImpuestoIGTF, NroOrden,
               Fact_MontoPagBsIGTF, Fact_MontoPagDivIGTF, Fact_TasaAbono, Fact_Moneda, ComprobRetencionISLR,
               ISLRRetenido, FechaRegistroComprobIVA, FechaRegistroComprobISLR
                FROM BD005_FB.dbo.TB_LIBRO_VENTA
                WHERE Fecha BETWEEN @fechaInicio AND @fechaFin", connection))

                {
                    cmd.Parameters.AddWithValue("@fechaInicio", DateTime.Parse(filtroFechas.FechaDesde));
                    cmd.Parameters.AddWithValue("@fechaFin", DateTime.Parse(filtroFechas.FechaHasta));
                    cmd.CommandType = CommandType.Text;

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var item = new repositorioLibro_Venta
                            {
                                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                NumeroFactura = reader["Fact_Num"]?.ToString(),
                                NumeroControl = reader["Fact_NumCtrol"]?.ToString(),
                                TotalVentasIncluyendoIVA = reader.GetDecimal(reader.GetOrdinal("Fact_Total")),
                                VentasExentas = reader.GetDecimal(reader.GetOrdinal("Fact_MontoExento")),
                                VentasInternasGravadas = reader.GetDecimal(reader.GetOrdinal("Fact_MontoGravable")),
                                PorcentajeAlicuotaGeneral = reader.GetDecimal(reader.GetOrdinal("Fact_AlicuotaIva")),
                                IVARetenido = reader.GetDecimal(reader.GetOrdinal("IvaRetenido")),
                                NumeroComprobante = reader["ComprobRetencionIVA"]?.ToString(),
                                OperacionNro = reader["NRONOTA"]?.ToString(),
                                RIF = reader["CTE_CedIden"]?.ToString(),
                                NombreORazonSocial = $"{reader["CTE_PNombre"]?.ToString()} {reader["CTE_PApellido"]?.ToString()}",
                                ImpresoraFiscal = reader["Fact_SerialImpresora"]?.ToString(),
                                NumeroNotaDebito = reader["NC_SerialImpresora"]?.ToString(),
                                NumeroNotaCredito = reader["NC_SerialImpresora"]?.ToString(),
                                TipoTransaccion = reader["Anulado"]?.ToString(),
                                NumeroFacturaAfectada = reader["Nota"]?.ToString(),
                                VentasInternasNoGravadas = reader.GetDecimal(reader.GetOrdinal("Fact_MontoGravable")),
                                VentasExoneradas = reader.GetDecimal(reader.GetOrdinal("Fact_MontoExento")),
                                VentasNoSujetas = reader.GetDecimal(reader.GetOrdinal("Fact_MontoExento")),
                                TotalNoGravadas = reader.GetDecimal(reader.GetOrdinal("Fact_MontoExento")),
                                AlicuotaGeneral = reader.GetDecimal(reader.GetOrdinal("Fact_AlicuotaIva")),
                                BaseImponibleAlicuotaGeneral = reader.GetDecimal(reader.GetOrdinal("Fact_MontoGravable")),
                                ImpuestoIVA_AlicuotaGeneral = reader.GetDecimal(reader.GetOrdinal("Fact_Impuesto")),
                                AlicuotaReducida = reader.GetDecimal(reader.GetOrdinal("Fact_AlicuotaIva")),
                                BaseImponibleAlicuotaReducida = reader.GetDecimal(reader.GetOrdinal("Fact_MontoGravable")),
                                ImpuestoIVA_AlicuotaReducida = reader.GetDecimal(reader.GetOrdinal("Fact_Impuesto")),
                                AlicuotaGeneralAdicional = reader.GetDecimal(reader.GetOrdinal("Fact_AlicuotaIva")),
                                BaseImponibleAlicuotaGeneralAdicional = reader.GetDecimal(reader.GetOrdinal("Fact_MontoGravable")),
                                ImpuestoIVA_AlicuotaGeneralAdicional = reader.GetDecimal(reader.GetOrdinal("Fact_Impuesto")),
                                RetencionesIVA = reader.GetDecimal(reader.GetOrdinal("IvaRetenido")),
                                FacturasAfectadas = reader["Nota"]?.ToString(),
                                ISLRRetenido = reader.GetDecimal(reader.GetOrdinal("ISLRRetenido")),
                                FechaRegistroComprobIVA = reader.GetDateTime(reader.GetOrdinal("FechaRegistroComprobIVA")),
                                FechaRegistroComprobISLR = reader.GetDateTime(reader.GetOrdinal("FechaRegistroComprobISLR"))

                            };

                            lista.Add(item);
                        }
                    }

                }
           
            }

            return lista;
        }








    }
}
