using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades;
using ClosedXML.Excel;
using DataAccess.ExportarArchivos;

namespace CapaLogica.ExportarArchivos
{
    public class ExportarXLSX_ReporteGlobal : ExportarArchivoXLSX_ReporteGlobal
    {
        public override async Task<byte[]> ExportAsync(List<ReporteGlobal_HojasExcels> hojasExcels, DateTime fechaSeleccionada)
        {

            using (XLWorkbook workbook = new XLWorkbook())
            {

                foreach (var hoja in hojasExcels)
                {
                    if (hoja.Datos == null || !hoja.Datos.Any())
                        continue;

                    var worksheet = workbook.Worksheets.Add(hoja.NombreHoja);

                    if (hoja.NombreHoja == "Cierre de Caja")
                    {
                        var data = hoja.Datos.Cast<ReporteGlobal_CierreCaja>().FirstOrDefault();
                        GenerarHojaCierreCaja(worksheet, data, fechaSeleccionada);
                    }
                    else if (hoja.NombreHoja == "Facturas")
                    {
                        var data = hoja.Datos.Cast<ReporteGlobal_Facturas>().ToList();
                        GenerarHojaFacturas(worksheet, data, fechaSeleccionada);
                    }
                    else
                    {
                        GenerarHojaGenerica(worksheet, hoja.Datos);
                    }
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return await Task.FromResult(stream.ToArray());
                }


            };


        }

        public void GenerarHojaCierreCaja(IXLWorksheet ws, ReporteGlobal_CierreCaja data, DateTime fecha)
        {
            // ======= ENCABEZADO (ARRIBA) =======
            //ws.Cell("A1").Value = "Compañía: ";
            //ws.Cell("A2").Value = "Rif: ";
            //ws.Cell("A3").Value = "Sucursal: ";

            ws.Cell("F1").Value = $"Fecha: {DateTime.Now:dd/MM/yyyy}";
            ws.Cell("F2").Value = $"Hora: {DateTime.Now:hh:mm tt}";
            //ws.Cell("F3").Value = "Pag: 1 de 1";

            // Opcional: Negritas y tamaño de letra
            ws.Range("A1:A3").Style.Font.Bold = true;
            ws.Range("F1:F3").Style.Font.Bold = true;

            // ======= TÍTULO DEL REPORTE =======
            ws.Cell("D5").Value = "Cierre de caja";
            ws.Cell("D5").Style.Font.Bold = true;
            ws.Cell("D5").Style.Font.FontSize = 14;

            ws.Cell("D6").Value = $"fecha: {fecha:dd/MM/yyyy}";
            ws.Cell("D6").Style.Font.Italic = true;

            // ======= TABLA DE DETALLE =======
            string[,] campos =
            {
                { "Efectivo", "MANUALEFECTIVO", "SISTEMAEFECTIVO" },
                { "Trj. Débito", "MANUALDEBITO", "SISTEMADEBITO" },
                { "Trj. Crédito", "MANUALCREDITO", "SISTEMACREDITO" },
                { "Gastos", "MANUALGASTOS", "SISTEMAGASTOS" },
                { "IVA Retenido", "ManualIVARetenido", "SistemaIVARetenido" },
                { "ISLR Retenido", "ManualISLRRetenido", "SistemaISLRRetenido" },
                { "Transferencia", "MANUALTRANSFERENCIA", "SISTEMATRANSFERENCIA" },
                { "Vuelto", "MANUALVUELTO", "SISTEMAVUELTO" },
                { "Total", "MANUALTOTALINGRESOS", "SISTEMATOTALINGRESOS" },
                { "Reintegros", "MANUALREINTEGROS", "SISTEMAREINTEGRO" },
                { "Notas Crédito", "MANUALNOTACREDITO", "SISTEMANOTACREDITO" }
            };

            // Encabezados de la tabla
            ws.Cell("C8").Value = "Concepto";
            ws.Cell("D8").Value = "Conteo Manual";
            ws.Cell("E8").Value = "Sistema";
            ws.Cell("F8").Value = "Diferencia";

            ws.Range("C8:F8").Style.Font.Bold = true;
            ws.Range("C8:F8").Style.Fill.BackgroundColor = XLColor.FromHtml("#027a65");
            ws.Range("C8:F8").Style.Font.FontColor = XLColor.White;

            // Rellenar los valores
            for (int i = 0; i < campos.GetLength(0); i++)
            {
                string concepto = campos[i, 0];
                string manualProp = campos[i, 1];
                string sistemaProp = campos[i, 2];

                decimal manual = Convert.ToDecimal(data.GetType().GetProperty(manualProp).GetValue(data));
                decimal sistema = Convert.ToDecimal(data.GetType().GetProperty(sistemaProp).GetValue(data));
                decimal diferencia = manual - sistema;

                int row = 9 + i;

                ws.Cell($"C{row}").Value = concepto;
                ws.Cell($"D{row}").Value = manual;
                ws.Cell($"E{row}").Value = sistema;
                ws.Cell($"F{row}").Value = diferencia;
            }

            // ======= TOTALES AL FINAL =======
            int ultimaFila = 9 + campos.GetLength(0);

            ws.Cell($"D{ultimaFila + 1}").Value = "Total Bruto:";
            ws.Cell($"E{ultimaFila + 1}").Value = data.TOTAL_BRUTO;

            ws.Cell($"D{ultimaFila + 2}").Value = "Impuesto:";
            ws.Cell($"E{ultimaFila + 2}").Value = data.IMPUESTO;

            ws.Cell($"D{ultimaFila + 3}").Value = "Total Neto:"; // Aquí agregas el Total Neto
            ws.Cell($"E{ultimaFila + 3}").Value = data.TOTAL_NETO;

            // Estilos para totales
            ws.Range($"D{ultimaFila + 1}:E{ultimaFila + 3}").Style.Font.Bold = true;
        //    ws.Range($"D{ultimaFila + 1}:E{ultimaFila + 3}").Style.Fill.BackgroundColor = XLColor.FromHtml("#e0f7fa");
        //    ws.Range($"D{ultimaFila + 1}:E{ultimaFila + 3}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }

        public void GenerarHojaFacturas(IXLWorksheet ws, List<ReporteGlobal_Facturas> facturas, DateTime fecha)
        {
            // ======= ENCABEZADO (ARRIBA) =======
            //ws.Cell("A1").Value = "Compañía: ";
            //ws.Cell("A2").Value = "Rif: ";
            //ws.Cell("A3").Value = $"Sucursal: {codSucursal}";

            ws.Cell("F1").Value = $"Fecha: {DateTime.Now:dd/MM/yyyy}";
            ws.Cell("F2").Value = $"Hora: {DateTime.Now:hh:mm tt}";
            //ws.Cell("F3").Value = "Pag: 1 de 1";

            // Opcional: Negritas y tamaño de letra
            ws.Range("A1:A3").Style.Font.Bold = true;
            ws.Range("F1:F3").Style.Font.Bold = true;

            // ======= TÍTULO DEL REPORTE =======
            ws.Cell("D5").Value = "Facturas";
            ws.Cell("D5").Style.Font.Bold = true;
            ws.Cell("D5").Style.Font.FontSize = 14;

            ws.Cell("D6").Value = $"fecha: {fecha:dd/MM/yyyy}";
            ws.Cell("D6").Style.Font.Italic = true;


            // Encabezados de la tabla
            ws.Cell("A8").Value = "Factura";
            ws.Cell("B8").Value = "Orden";
            ws.Cell("C8").Value = "Cedula";
            ws.Cell("D8").Value = "SubTotal";
            ws.Cell("E8").Value = "Descuento";
            ws.Cell("F8").Value = "IVA";
            ws.Cell("G8").Value = "IGTF";
            ws.Cell("H8").Value = "Total";
            ws.Columns("A", "H").Width = 15;

            var headerRange = ws.Range("A8:H8");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#027a65");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // ======= LLENAR DATOS =======
            int fila = 9;
            foreach (var factura in facturas)
            {
                ws.Cell(fila, 1).Value = factura.Fact_Num;
                ws.Cell(fila, 2).Value = factura.NumOrdServ;
                ws.Cell(fila, 3).Value = factura.CTE_CedIdenPAG;
                ws.Cell(fila, 4).Value = factura.Fact_SubTotal;
                ws.Cell(fila, 5).Value = factura.Fact_Descuento;
                ws.Cell(fila, 6).Value = factura.Fact_Impuesto;
                ws.Cell(fila, 7).Value = factura.Fact_IGTF;
                ws.Cell(fila, 8).Value = factura.Fact_Total;

                fila++;
            }

            ws.Columns().AdjustToContents();
        }



        private void GenerarHojaGenerica(IXLWorksheet worksheet, IEnumerable<object> datos)
        {
            var firstItem = datos.First();
            var properties = GetProperties(firstItem.GetType());

            for (int i = 0; i < properties.Count; i++)
            {
                worksheet.Cell(1, i + 1).Value = properties[i].Name;
            }

            int row = 2;
            foreach (var item in datos)
            {
                for (int col = 0; col < properties.Count; col++)
                {
                    worksheet.Cell(row, col + 1).Value = GetValue(properties[col], item);
                }
                row++;
            }

            worksheet.Columns().AdjustToContents();
        }

        public override async Task<FormatoDeArchivo> ExportWithFormatAsync(List<ReporteGlobal_HojasExcels> hojasExcels, DateTime fechaSeleccionada)
        {
            byte[] bytes = await ExportAsync(hojasExcels, fechaSeleccionada);
            return new FormatoDeArchivo
            {
                Content = bytes,
                Type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                Extension = "xlsx"
            };
        }













    }
}

