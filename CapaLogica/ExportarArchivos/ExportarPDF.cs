using CapaEntidades;
using CapaLogica.ExportarArchivos;
using DataAccess.ExportarArchivos;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

public class ExportarPDF : ExportarArchivoPdf_Facturas
{
    //public override async Task<byte[]> ExportAsync<T>(IEnumerable<T> data, DatosEncabezado datosEncabezado)
    public override async Task<byte[]> ExportAsync<T>(IEnumerable<T> data, FormatoPdfDTO formatoPdf)
    {
        if (data == null || !data.Any())
        {
            throw new ArgumentException("No data available to export.");
        }

        using (MemoryStream stream = new MemoryStream())
        {
            Document document = new Document(PageSize.A4);
            PdfWriter.GetInstance(document, stream);
            document.Open();

            // Agregar el encabezado
            PdfPTable headerTable = new PdfPTable(2);
            headerTable.WidthPercentage = 100;
            float[] columnWidths = { 0.7f, 0.3f }; // Ajustar el ancho de las columnas
            headerTable.SetWidths(columnWidths);

            // Logo
            Image logo = Image.GetInstance(formatoPdf.LogoBytes);
            logo.ScaleToFit(110, 110);
            PdfPCell logoCell = new PdfPCell(logo);
            logoCell.Border = PdfPCell.NO_BORDER;
            logoCell.HorizontalAlignment = Element.ALIGN_LEFT;
            headerTable.AddCell(logoCell);

            // Fecha y Hora en la esquina superior derecha
            PdfPCell dateTimeCell = new PdfPCell();
            dateTimeCell.Border = PdfPCell.NO_BORDER;
            dateTimeCell.HorizontalAlignment = Element.ALIGN_RIGHT;

            // Aumentar el margen derecho para alinear más hacia la derecha
            dateTimeCell.PaddingRight = 0;

            // Reducir el tamaño de la fuente para la fecha y hora
            Font dateFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.BLACK);

            dateTimeCell.AddElement(new Phrase("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy"), dateFont));
            dateTimeCell.AddElement(new Phrase("Hora: " + DateTime.Now.ToString("HH:mm:ss"), dateFont));
            headerTable.AddCell(dateTimeCell);

            // Añadir la tabla del encabezado al documento
            document.Add(headerTable);

            // Agregar espacio entre logo y título
            document.Add(new Paragraph(" "));

            // Título dinámico según la selección de datos a exportar.
            bool contieneNotaCredito = data.Any(item => !string.IsNullOrEmpty(((dynamic)item).NotaCredito));
            string titulo = contieneNotaCredito ? "Reporte de Notas de Crédito" : "Reporte de Facturas";
            // Título
            Paragraph title = new Paragraph(titulo, new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.BOLD));
            title.Alignment = Element.ALIGN_CENTER;
            document.Add(title);

            // Más espacio entre título y fechas
            document.Add(new Paragraph(" "));

            // Fechas filtradas
            Paragraph filteredDates = new Paragraph($"Desde: {formatoPdf.FechaInicio.ToString("dd/MM/yyyy")} Hasta: {formatoPdf.FechaFin.ToString("dd/MM/yyyy")}", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
            filteredDates.Alignment = Element.ALIGN_CENTER;
            document.Add(filteredDates);

            // Línea separadora
            document.Add(new Paragraph(new Chunk(new LineSeparator(0.1f, 100.0f, BaseColor.BLACK, Element.ALIGN_LEFT, 1))));

            //// Código y Descripción de Sucursal
            //Paragraph sucursalInfo = new Paragraph($"Sucursal: {datosEncabezado.CodSucursal} - {datosEncabezado.DescSucursal}", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
            //sucursalInfo.Alignment = Element.ALIGN_LEFT;
            //document.Add(sucursalInfo);

            // Más espacio antes de los datos
            document.Add(new Paragraph(" "));

            // Formatear las filas de datos
            Font headerFont = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD);
            Font dataFont = new Font(Font.FontFamily.HELVETICA, 10);

            int columnas = contieneNotaCredito ? 9 : 8;
            PdfPTable table = new PdfPTable(columnas);
            table.WidthPercentage = 100;

            float[] widths = contieneNotaCredito
                ? new float[] { 10f, 10f, 15f, 20f, 15f, 10f, 10f, 10f, 10f } // Con Nota Crédito
                : new float[] { 10f, 10f, 20f, 15f, 10f, 10f, 10f, 10f };      // Sin Nota Crédito

            table.SetWidths(widths);

            // Títulos dinámicos según corresponda
            string[] titulos = contieneNotaCredito
                ? new string[] { "Factura", "Fecha", "Nota Crédito", "Cliente", "Cédula", "SubTotal", "Impuesto", "IGTF", "Total" }
                : new string[] { "Factura", "Fecha", "Cliente", "Cédula", "SubTotal", "Impuesto", "IGTF", "Total" };

            foreach (string titulopdf in titulos)
            {
                PdfPCell cell = new PdfPCell(new Phrase(titulopdf, headerFont));
                cell.Border = Rectangle.NO_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
            }

            // Agregar las filas de datos
            foreach (T item in data)
            {
                dynamic factura = item;
                bool esNotaCredito = !string.IsNullOrEmpty(factura.NotaCredito);

                // Crear las celdas de cada campo
                table.AddCell(new PdfPCell(new Phrase(factura.NumeroFactura.ToString(), dataFont)) { Border = Rectangle.NO_BORDER });
                table.AddCell(new PdfPCell(new Phrase(factura.Fecha.ToString(), dataFont)) { Border = Rectangle.NO_BORDER });

                if (contieneNotaCredito)
                {
                    table.AddCell(new PdfPCell(new Phrase(esNotaCredito ? factura.NotaCredito.ToString() : "-", dataFont)) { Border = Rectangle.NO_BORDER });
                }

                table.AddCell(new PdfPCell(new Phrase(factura.NombreCliente.ToString(), dataFont)) { Border = Rectangle.NO_BORDER }); table.AddCell(new PdfPCell(new Phrase(factura.CedulaCliente.ToString(), dataFont)) { Border = Rectangle.NO_BORDER });
                table.AddCell(new PdfPCell(new Phrase(factura.FactSub.ToString("N2"), dataFont)) { Border = Rectangle.NO_BORDER });
                table.AddCell(new PdfPCell(new Phrase(factura.FactImpuesto.ToString("N2"), dataFont)) { Border = Rectangle.NO_BORDER });
                table.AddCell(new PdfPCell(new Phrase(factura.FactIGTF.ToString("N2"), dataFont)) { Border = Rectangle.NO_BORDER });
                table.AddCell(new PdfPCell(new Phrase(factura.FactTotal.ToString("N2"), dataFont)) { Border = Rectangle.NO_BORDER });
            }

            // Añadir la tabla al documento
            document.Add(table);

            //foreach (T item in data)
            //{
            //    dynamic factura = item;

            //    document.Add(new Paragraph("Factura: " + factura.NumeroFactura, headerFont));

            //    if (!string.IsNullOrEmpty(factura.NotaCredito))
            //        document.Add(new Paragraph("Nota de Crédito: " + factura.NotaCredito, dataFont));

            //    if (!string.IsNullOrEmpty(factura.NombreCliente))
            //        document.Add(new Paragraph("Cliente: " + factura.NombreCliente, dataFont));

            //    document.Add(new Paragraph("Cédula: " + factura.CedulaCliente, dataFont));
            //    document.Add(new Paragraph("Sub Total: " + factura.FactSub.ToString("N2"), dataFont));
            //    document.Add(new Paragraph("Impuesto: " + factura.FactImpuesto.ToString("N2"), dataFont));
            //    document.Add(new Paragraph("IGTF: " + factura.FactIGTF.ToString("N2"), dataFont));
            //    document.Add(new Paragraph("Total: " + factura.FactTotal.ToString("N2"), dataFont));
            //    document.Add(new Paragraph("Fecha: " + factura.Fecha, dataFont));

            //    document.Add(new Paragraph(" "));

            //}


            document.Close();

            return await Task.FromResult(stream.ToArray());
        }
    }
    public iTextSharp.text.Image ConvertToITextSharpImage(System.Drawing.Image image)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            // Guarda la imagen en el MemoryStream en formato PNG
            image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            memoryStream.Position = 0;

            // Convierte el MemoryStream a una imagen de iTextSharp
            return iTextSharp.text.Image.GetInstance(memoryStream);
        }
    }
    //public override async Task<FormatoDeArchivo> ExportWithFormatAsync<T>(IEnumerable<T> data, DatosEncabezado datosEncabezado)
    public override async Task<FormatoDeArchivo> ExportWithFormatAsync<T>(IEnumerable<T> data, FormatoPdfDTO formatoPdf)

    {
        byte[] bytes = await ExportAsync(data, formatoPdf); 
        return new FormatoDeArchivo
        {
            Content = bytes,
            Type = "application/pdf",
            Extension = "pdf"
        };
    }

    private string GetValue(string propertyName, object item)
    {
        var property = item.GetType().GetProperty(propertyName);
        return property != null ? property.GetValue(item)?.ToString() : string.Empty;
    }
}
