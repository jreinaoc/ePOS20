using CapaEntidades;
using CapaLogica.DatosGeneralesSucursal_Logica;
using ClosedXML.Excel;
using DataAccess.ExportarArchivos;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica.ExportarArchivos
{
    public class ExportarXLSX_LibroVentas : ExportarArchivo_LibroVentas
    {

        public override async Task<byte[]> ExportAsync<T>(IEnumerable<T> data, FormatoLibroVentasDTO datosEncabezado)
        {
            var datosSucursal = new L_DatosGenerales();
            await datosSucursal.ObtenerDatosSucursalYCompania_Global();

            using (XLWorkbook workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("Libro de Ventas");

                // ======= ENCABEZADO SUPERIOR =======
                ws.Range("A1:C1").Merge().Value = $"Compañía: {VariablesGlobales.Compania}";
                ws.Range("A2:C2").Merge().Value = $"Rif: {VariablesGlobales.Rif}";
                ws.Range("A3:C3").Merge().Value = $"Sucursal: {VariablesGlobales.CodSucursal} - {VariablesGlobales.Sucursal}";

                ws.Cell("AC1").Value = $"Fecha: {DateTime.Now:dd/MM/yyyy}";
                ws.Cell("AC2").Value = $"Hora: {DateTime.Now:hh:mm tt}";

                ws.Range("O4:P4").Merge().Value = "LIBRO DE VENTAS";
                ws.Range("O4:P4").Style.Font.Bold = true;
                ws.Range("O4:P4").Style.Font.FontSize = 14;
                ws.Range("O4:P4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                ws.Range("N6:Q6").Merge().Value = $"fecha inicio: {datosEncabezado.FechaInicio:dd/MM/yyyy} - fecha fin: {datosEncabezado.FechaFin:dd/MM/yyyy}";
                ws.Range("N6:Q6").Merge().Style.Font.Italic = true;
                ws.Range("N6:Q6").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // ======= ENCABEZADOS AGRUPADOS (CELDAS COMBINADAS) =======

                // Agrupaciones principales
                ws.Range("N8:Q8").Merge().Value = "Ventas Internas No Gravadas"; //8
                ws.Range("R8:Z8").Merge().Value = "Ventas Internas Gravadas"; //8
                ws.Range("AA8:AC8").Merge().Value = "Retenciones de IVA"; //8

                // Subgrupos dentro de "Ventas Internas Gravadas"
                ws.Range("Q9:S9").Merge().Value = "Alicuota General"; //9
                ws.Range("T9:V9").Merge().Value = "Alicuota Reducida"; //9
                ws.Range("W9:Y9").Merge().Value = "Alicuota General + Adicional"; //9

                // Estilos de grupo principal
                var encabezadosGrupos = ws.Range("N8:AC8");
                encabezadosGrupos.Style.Font.Bold = true;
                encabezadosGrupos.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                encabezadosGrupos.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                encabezadosGrupos.Style.Fill.BackgroundColor = XLColor.FromHtml("#027a65");
                encabezadosGrupos.Style.Font.FontColor = XLColor.White;
                encabezadosGrupos.Style.Border.OutsideBorderColor = XLColor.Black;
                encabezadosGrupos.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                encabezadosGrupos.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                encabezadosGrupos.Style.Border.InsideBorderColor = XLColor.Black;


                //estilo de subgrupos
                var encabezadosSubGrupos = ws.Range("Q9:Y9");
                encabezadosSubGrupos.Style.Font.Bold = true;
                encabezadosSubGrupos.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                encabezadosSubGrupos.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                encabezadosSubGrupos.Style.Fill.BackgroundColor = XLColor.FromHtml("#04b596");
                encabezadosSubGrupos.Style.Font.FontColor = XLColor.White;
                encabezadosSubGrupos.Style.Border.OutsideBorderColor = XLColor.Black;
                encabezadosSubGrupos.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                encabezadosSubGrupos.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                encabezadosSubGrupos.Style.Border.InsideBorderColor = XLColor.Black;


                // Subencabezados 
                string[] titulos = new string[]
                {
                    "Operación Nro.", "Fecha", "RIF", "Nombre o Razon Social",
                    "Nº Factura", "Nº Control", "Impresora Fiscal", "Nº Nota Débito",
                    "Nº Nota Crédito", "Tipo Transacción", "Nº Factura Afectada",
                    "Total Ventas Incluyendo IVA", "Ventas Exentas", "Ventas Exoneradas",
                    "Ventas No Sujetas", "Total No Gravadas",
                    "Base Imponible", "% Alicuota", "Impuesto IVA",
                    "Base Imponible Reducida", "% Alicuota Reducida", "Impuesto IVA Reducido",
                    "Base Imponible Adicional", "% Alicuota Adicional", "Impuesto IVA Adicional",
                    "Fecha Retención", "Factura Afectada Retención", "Comprobante Retención", "IVA Retenido"
                };

                // Dibujar los encabezados
                for (int i = 0; i < titulos.Length; i++)
                {
                    var cell = ws.Cell(10, i + 1); // Fila 10 para subencabezados
                    cell.Value = titulos[i];
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#027a65");
                    cell.Style.Font.FontColor = XLColor.White;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    cell.Style.Border.OutsideBorderColor = XLColor.Black;
                }

                // ======= LLENAR DATOS =======
                int row = 11;
                int operacionNro = 1;
                foreach (T item in data)
                {
                    ws.Cell(row, 1).Value = operacionNro++;

                    for (int col = 1; col < titulos.Length; col++)
                    {
                        var valor = GetValueByTitle(titulos[col], item);

                        if (valor is DateTime fecha)
                            ws.Cell(row, col + 1).Value = fecha;
                        else if (valor is decimal dec)
                            ws.Cell(row, col + 1).Value = dec;
                        else if (valor is int entero)
                            ws.Cell(row, col + 1).Value = entero;
                        else
                            ws.Cell(row, col + 1).Value = valor?.ToString() ?? "";
                        
                    }
                    row++;
                }

                // ======= AGREGAR FILA DE TOTALES =======
                var totalRow = row; // La fila siguiente después de los datos

                // Texto "TOTAL GENERAL:"
                ws.Cell(totalRow, 4).Value = "TOTAL GENERAL:";
                ws.Cell(totalRow, 4).Style.Font.Bold = true;

                // Totales en sus respectivas columnas
                var listaDatos = data.Cast<tbLibroVentas_Reporte>().ToList();

                ws.Cell(totalRow, 12).Value = listaDatos.Sum(x => x.TotalVentas_Iva);
                ws.Cell(totalRow, 13).Value = listaDatos.Sum(x => x.VentasExentas);
                ws.Cell(totalRow, 16).Value = listaDatos.Sum(x => x.TotalNoGravadas);
                ws.Cell(totalRow, 17).Value = listaDatos.Sum(x => x.BaseImponible);
                ws.Cell(totalRow, 19).Value = listaDatos.Sum(x => x.ImpuestoIVA);

                // Ajustar estilos de la fila total
                var totalRange = ws.Range(totalRow, 4, totalRow, 19);
                totalRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                ws.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return await Task.FromResult(stream.ToArray());
                }
            }
        }


        private object GetValueByTitle(string title, object item)
        {
            var reporte = item as tbLibroVentas_Reporte;

            if (reporte == null)
                return "";

            if (title == "Fecha") return reporte.Fecha.ToString("dd/MM/yyyy");
            if (title == "RIF") return reporte.Rif_Cedula;
            if (title == "Nombre o Razon Social") return reporte.Nombre_RazonSocial;
            if (title == "Nº Factura") return reporte.NumeroFactura;
            if (title == "Nº Control") return reporte.NumeroControl;
            if (title == "Impresora Fiscal") return reporte.ImpresoraFiscal;
            if (title == "Nº Nota Débito") return reporte.NotaDebito;
            if (title == "Nº Nota Crédito") return reporte.NotaCredito;
            if (title == "Tipo Transacción") return reporte.TipoTransaccion;
            if (title == "Nº Factura Afectada") return reporte.FacturaAfectada;
            if (title == "Total Ventas Incluyendo IVA") return reporte.TotalVentas_Iva;
            if (title == "Ventas Exentas") return reporte.VentasExentas;
            if (title == "Ventas Exoneradas") return reporte.VentasExoneradas;
            if (title == "Ventas No Sujetas") return reporte.VentaNoSujetas;
            if (title == "Total No Gravadas") return reporte.TotalNoGravadas;

            // Ventas Internas Gravadas
            if (title == "Base Imponible") return reporte.BaseImponible;
            if (title == "% Alicuota") return reporte.Alicuota;
            if (title == "Impuesto IVA") return reporte.ImpuestoIVA;

            // Alicuota Reducida
            if (title == "Base Imponible Reducida") return reporte.BaseImponibleReducida;
            if (title == "% Alicuota Reducida") return reporte.AlicuotaReducida;
            if (title == "Impuesto IVA Reducido") return reporte.ImpuestoIVAReducido;

            // General + Adicional
            if (title == "Base Imponible Adicional") return reporte.BaseImponibleAdicional;
            if (title == "% Alicuota Adicional") return reporte.AlicuotaAdicional;
            if (title == "Impuesto IVA Adicional") return reporte.ImpuestoIVAAdicional;

            if (title == "Fecha Retención") return reporte.FechaRetencion;
            if (title == "Factura Afectada Retención") return reporte.FacturaAfectadaRetencion;
            if (title == "IVA Retenido") return reporte.IVARetenido;
            if (title == "Comprobante Retención") return reporte.ComprobanteRetencion;

            return "";
        }



        public override async Task<FormatoDeArchivo> ExportWithFormatAsync<T>(IEnumerable<T> data, FormatoLibroVentasDTO datosEncabezado)
        {
            byte[] bytes = await ExportAsync(data, datosEncabezado);
            return new FormatoDeArchivo
            {
                Content = bytes,
                Type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                Extension = "xlsx"
            };
        }
    }

}
