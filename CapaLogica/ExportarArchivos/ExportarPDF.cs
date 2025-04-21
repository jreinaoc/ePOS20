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

            // Título
            Paragraph title = new Paragraph("Reporte de Facturas y Notas de Credito/Debito", new Font(Font.FontFamily.TIMES_ROMAN, 16, Font.BOLD));
            title.Alignment = Element.ALIGN_CENTER;
            document.Add(title);

            // Más espacio entre título y fechas
            document.Add(new Paragraph(" "));

            // Fechas filtradas
            Paragraph filteredDates = new Paragraph($"Desde: {formatoPdf.FechaInicio.ToString("dd/MM/yyyy")} Hasta: {formatoPdf.FechaFin.ToString("dd/MM/yyyy")}", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL));
            filteredDates.Alignment = Element.ALIGN_LEFT;
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

            foreach (T item in data)
            {
                dynamic factura = item;

                document.Add(new Paragraph("Factura: " + factura.NumeroFactura, headerFont));

                if (!string.IsNullOrEmpty(factura.NotaCredito))
                    document.Add(new Paragraph("Nota de Crédito: " + factura.NotaCredito, dataFont));

                if (!string.IsNullOrEmpty(factura.NombreCliente))
                    document.Add(new Paragraph("Cliente: " + factura.NombreCliente, dataFont));

                document.Add(new Paragraph("Cédula: " + factura.CedulaCliente, dataFont));
                document.Add(new Paragraph("Sub Total: " + factura.FactSub.ToString("N2"), dataFont));
                document.Add(new Paragraph("Impuesto: " + factura.FactImpuesto.ToString("N2"), dataFont));
                document.Add(new Paragraph("IGTF: " + factura.FactIGTF.ToString("N2"), dataFont));
                document.Add(new Paragraph("Total: " + factura.FactTotal.ToString("N2"), dataFont));
                document.Add(new Paragraph("Fecha: " + factura.Fecha, dataFont));

                document.Add(new Paragraph(" "));

            }


            //foreach (T item in data)
            //{
            //    // Aquí, debes mapear las propiedades del objeto `item` a sus respectivos valores
            //    string nroOrden = GetValue("NroOrden", item) ?? string.Empty;
            //    string rev = GetValue("Rev", item) ?? string.Empty;
            //    string fecha = GetValue("Fecha", item) ?? string.Empty;
            //    string codCausa = GetValue("CodCausa", item) ?? string.Empty;
            //    string observacion = GetValue("Observacion", item) ?? string.Empty;

            //    // Agregar las celdas con sus valores
            //    document.Add(new Paragraph("Orden: " + nroOrden, headerFont) { Alignment = Element.ALIGN_LEFT });
            //    document.Add(new Paragraph("Revisión: " + rev, dataFont) { Alignment = Element.ALIGN_LEFT });
            //    document.Add(new Paragraph("Fecha: " + fecha, dataFont) { Alignment = Element.ALIGN_LEFT });
            //    document.Add(new Paragraph("Código de Causa: " + codCausa, dataFont) { Alignment = Element.ALIGN_LEFT });

            //    // Para la observación, asegura que el texto haga un salto de línea automáticamente
            //    PdfPCell observationCell = new PdfPCell(new Phrase("Observación: " + observacion, dataFont))
            //    {
            //        Border = PdfPCell.NO_BORDER,
            //        HorizontalAlignment = Element.ALIGN_LEFT,
            //        NoWrap = false, 
            //        PaddingTop = 5f,
            //        PaddingBottom = 5f
            //    };

            //    PdfPTable observationTable = new PdfPTable(1);
            //    observationTable.WidthPercentage = 100;
            //    observationTable.AddCell(observationCell);
            //    document.Add(observationTable);

            //    document.Add(new Paragraph(" ", dataFont)); // Espacio entre registros
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
