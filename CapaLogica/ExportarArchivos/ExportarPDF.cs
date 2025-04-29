using CapaEntidades;
using CapaLogica.ExportarArchivos;
using DataAccess.ExportarArchivos;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Pdf.Canvas.Draw;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iText.Layout.Borders;
using System.Diagnostics;

public class ExportarPDF : ExportarArchivoPdf_Facturas
{
    public override async Task<byte[]> ExportAsync<T>(IEnumerable<T> data, FormatoPdfDTO formatoPdf)
    {
        if (data == null || !data.Any())
        {
            throw new ArgumentException("No data available to export.");
        }

        using (MemoryStream stream = new MemoryStream())
        {
            // Obtén los ensamblados de Kernel y Layout
            var asmKernel = typeof(iText.Kernel.Pdf.PdfDocument).Assembly;
            var asmLayout = typeof(iText.Layout.Document).Assembly;

            // Loggea nombre, versión y ruta
            Debug.WriteLine($"[iText Debug] Kernel : {asmKernel.GetName().Name} v{asmKernel.GetName().Version}");
            Debug.WriteLine($"[iText Debug] Path   : {asmKernel.Location}");
            Debug.WriteLine($"[iText Debug] Layout : {asmLayout.GetName().Name} v{asmLayout.GetName().Version}");
            Debug.WriteLine($"[iText Debug] Path   : {asmLayout.Location}");


            PdfWriter writer;
            try
            {
                // 2) Creamos el PdfWriter con el stream
                writer = new PdfWriter(stream);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(">>> ERROR al instanciar PdfWriter:");
                Debug.WriteLine(ex.GetType().FullName + ": " + ex.Message);
                Debug.WriteLine(ex.StackTrace);
                if (ex.InnerException != null)
                {
                    Debug.WriteLine("INNER: " + ex.InnerException.GetType().FullName + ": " + ex.InnerException.Message);
                    Debug.WriteLine(ex.InnerException.StackTrace);
                }
                throw;
            }

            //PdfWriter writer = new PdfWriter(stream);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf, iText.Kernel.Geom.PageSize.A4);

            PdfFont normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

            // 1. ENCABEZADO: Logo + Fecha/Hora
            Table headerTable = new Table(UnitValue.CreatePercentArray(new float[] { 0.7f, 0.3f })).UseAllAvailableWidth();

            //// Logo
            //if (formatoPdf.LogoBytes != null && formatoPdf.LogoBytes.Length > 0)
            //{
            //    Image logo = new Image(ImageDataFactory.Create(formatoPdf.LogoBytes));
            //    logo.ScaleToFit(100, 100);

            //    Cell logoCell = new Cell().Add(logo)
            //                              .SetBorder(Border.NO_BORDER)
            //                              .SetTextAlignment(TextAlignment.LEFT);
            //    headerTable.AddCell(logoCell);
            //}
            //else
            //{
            //    // Celda vacía si no hay logo
            //    headerTable.AddCell(new Cell().SetBorder(Border.NO_BORDER));
            //}

            // Fecha y Hora
            Paragraph dateParagraph = new Paragraph()
                .Add(new Text("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy") + "\n").SetFont(normalFont).SetFontSize(10))
                .Add(new Text("Hora: " + DateTime.Now.ToString("HH:mm:ss")).SetFont(normalFont).SetFontSize(10));

            Cell dateCell = new Cell().Add(dateParagraph)
                                      .SetBorder(Border.NO_BORDER)
                                      .SetTextAlignment(TextAlignment.RIGHT);
            headerTable.AddCell(dateCell);

            document.Add(headerTable);

            // 2. Espacio
            document.Add(new Paragraph("\n"));

            // 3. Título
            bool contieneNotaCredito = data.Any(item => !string.IsNullOrEmpty(((dynamic)item).NotaCredito));
            string titulo = contieneNotaCredito ? "Reporte de Notas de Crédito" : "Reporte de Facturas";

            document.Add(new Paragraph(titulo)
                .SetFont(boldFont)
                .SetFontSize(16)
                .SetTextAlignment(TextAlignment.CENTER));

            document.Add(new Paragraph("\n"));

            // 4. Fechas Filtradas
            document.Add(new Paragraph($"Desde: {formatoPdf.FechaInicio:dd/MM/yyyy}  Hasta: {formatoPdf.FechaFin:dd/MM/yyyy}")
                .SetFont(normalFont)
                .SetFontSize(12)
                .SetTextAlignment(TextAlignment.CENTER));

            // 5. Separador
            document.Add(new LineSeparator(new SolidLine()));

            document.Add(new Paragraph("\n"));

            // 6. Tabla de datos
            int columnas = contieneNotaCredito ? 9 : 8;
            Table table = new Table(UnitValue.CreatePercentArray(columnas)).UseAllAvailableWidth();

            string[] headers = contieneNotaCredito
                ? new[] { "Factura", "Fecha", "Nota Crédito", "Cliente", "Cédula", "SubTotal", "Impuesto", "IGTF", "Total" }
                : new[] { "Factura", "Fecha", "Cliente", "Cédula", "SubTotal", "Impuesto", "IGTF", "Total" };

            foreach (var header in headers)
            {
                table.AddHeaderCell(new Cell()
                    .Add(new Paragraph(header).SetFont(boldFont).SetFontSize(10))
                    .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                    .SetTextAlignment(TextAlignment.CENTER));
            }

            foreach (T item in data)
            {
                dynamic factura = item;
                bool esNotaCredito = !string.IsNullOrEmpty(factura.NotaCredito);

                table.AddCell(CreateCell(factura.NumeroFactura?.ToString(), normalFont));
                table.AddCell(CreateCell(factura.Fecha?.ToString(), normalFont));

                if (contieneNotaCredito)
                {
                    table.AddCell(CreateCell(esNotaCredito ? factura.NotaCredito?.ToString() : "-", normalFont));
                }

                table.AddCell(CreateCell(factura.NombreCliente?.ToString(), normalFont));
                table.AddCell(CreateCell(factura.CedulaCliente?.ToString(), normalFont));
                table.AddCell(CreateCell(factura.FactSub.ToString("N2"), normalFont));
                table.AddCell(CreateCell(factura.FactImpuesto.ToString("N2"), normalFont));
                table.AddCell(CreateCell(factura.FactIGTF.ToString("N2"), normalFont));
                table.AddCell(CreateCell(factura.FactTotal.ToString("N2"), normalFont));
            }

            document.Add(table);

            document.Close();
            pdf.Close();

            return await Task.FromResult(stream.ToArray());
        }
    }

    private Cell CreateCell(string value, PdfFont font)
    {
        return new Cell()
            .Add(new Paragraph(value ?? "").SetFont(font).SetFontSize(9))
            .SetTextAlignment(TextAlignment.CENTER)
            .SetBorder(Border.NO_BORDER);
    }

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
}