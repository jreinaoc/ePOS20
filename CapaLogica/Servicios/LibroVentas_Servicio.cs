using CapaEntidades;
using CapaLogica.ListaFactura_Logica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaLogica.Servicios
{
    public class LibroVentas_Servicio
    {
        private readonly L_ListaFacturas _logicaFacturas = new L_ListaFacturas();

        public async Task<List<tbLibroVentas_Reporte>> ObtenerLibroVentasTotal(DateTimePicker desde, DateTimePicker hasta)
        {
            var dataSet = await _logicaFacturas.ReporteLibroVentas(desde, hasta);

            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                return new List<tbLibroVentas_Reporte>();

            var listaDatos = new List<tbLibroVentas_Reporte>();

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                listaDatos.Add(new tbLibroVentas_Reporte
                {
                    Fecha = Convert.ToDateTime(row["Fecha"]),
                    Rif_Cedula = row["Rif_Cedula"]?.ToString(),
                    Nombre_RazonSocial = row["Nombre_RazonSocial"]?.ToString(),
                    NumeroFactura = row["NumeroFactura"]?.ToString(),
                    NumeroControl = row["NumeroControl"]?.ToString(),
                    ImpresoraFiscal = row["ImpresoraFiscal"]?.ToString(),
                    NotaDebito = row["NotaDebito"]?.ToString(),
                    NotaCredito = row["NotaCredito"]?.ToString(),
                    TipoTransaccion = row["TipoTransaccion"]?.ToString(),
                    FacturaAfectada = row["FacturaAfectada"]?.ToString(),
                    TotalVentas_Iva = Convert.ToDecimal(row["TotalVentas_Iva"] ?? 0),
                    VentasExentas = Convert.ToDecimal(row["VentasExentas"] ?? 0),
                    VentasExoneradas = row["VentasExoneradas"]?.ToString(),
                    VentaNoSujetas = row["VentaNoSujetas"]?.ToString(),
                    TotalNoGravadas = Convert.ToDecimal(row["TotalNoGravadas"] ?? 0),
                    BaseImponible = Convert.ToDecimal(row["BaseImponible"] ?? 0),
                    Alicuota = Convert.ToDecimal(row["Alicuota"] ?? 0),
                    ImpuestoIVA = Convert.ToDecimal(row["ImpuestoIVA"] ?? 0),
                    BaseImponibleReducida = row["BaseImponibleReducida"]?.ToString(),
                    AlicuotaReducida = row["AlicuotaReducida"]?.ToString(),
                    ImpuestoIVAReducido = row["ImpuestoIVAReducido"]?.ToString(),
                    BaseImponibleAdicional = row["BaseImponibleAdicional"]?.ToString(),
                    AlicuotaAdicional = row["AlicuotaAdicional"]?.ToString(),
                    ImpuestoIVAAdicional = row["ImpuestoIVAAdicional"]?.ToString(),
                    FechaRetencion = row["FechaRetencion"]?.ToString(),
                    FacturaAfectadaRetencion = row["FacturaAfectadaRetencion"]?.ToString(),
                    ComprobanteRetencion = row["ComprobanteRetencion"]?.ToString(),
                    IVARetenido = Convert.ToDecimal(row["IVARetenido"] ?? 0)
                });
            }

            var totales = new tbLibroVentas_Reporte
            {
                EsTotal = true,
                TextoTotal = "TOTAL GENERAL:",
                TotalSumaVentas_Iva = listaDatos.Sum(x => x.TotalVentas_Iva),
                TotalSumaVentasExentas = listaDatos.Sum(x => x.VentasExentas),
                TotalSumaNoGravadas = listaDatos.Sum(x => x.TotalNoGravadas),
                TotalSumaBaseImponible = listaDatos.Sum(x => x.BaseImponible),
                TotalSumaImpuestoIVA = listaDatos.Sum(x => x.ImpuestoIVA)
            };

            listaDatos.Add(totales);

            return listaDatos;
        }

    }
}
