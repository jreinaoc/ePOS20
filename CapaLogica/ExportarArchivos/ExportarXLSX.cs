using CapaLogica.ExportarArchivos;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ExportarArchivos
{
    public class ExportarXLSX : ExportarArchivo
    {
        // Add your code here
        public override async Task<byte[]> ExportAsync<T>(IEnumerable<T> data, DatosEncabezado datosEncabezado)
        {
            if (data == null || !data.Any())
            {
                throw new ArgumentException("No data available to export.");
            }

            using (XLWorkbook workbook = new XLWorkbook())
            {

                IXLWorksheet worksheet = workbook.Worksheets.Add("Sheet1");
                List<PropertyInfoWrapper> properties = GetProperties(typeof(T));

                // Add header row
                for (int i = 0; i < properties.Count; i++)
                {
                    IXLCell cell = worksheet.Cell(1, i + 1);

                    cell.Value =
                    properties[i].Name;
                    cell.Style.Font.FontColor = XLColor.White;
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#027a65");
                    cell.Style.Font.Bold = true;
                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin; // Add border around the cell
                    cell.Style.Border.OutsideBorderColor = XLColor.Black; // Set border color to black

                }

                // Add data rows
                int row = 2;
                foreach (T item in data)
                {
                    for (int col = 0; col < properties.Count; col++)
                    {
                        worksheet.Cell(row, col + 1).Value = GetValue(properties[col], item);
                    }
                    row++;
                }

                _ = worksheet.Columns().AdjustToContents();


                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return await Task.FromResult(stream.ToArray());
                };
            };
        }

        public override async Task<FormatoDeArchivo> ExportWithFormatAsync<T>(IEnumerable<T> data, DatosEncabezado datosEncabezado)
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
