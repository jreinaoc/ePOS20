using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Reporting.WinForms;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.Drawing;
using System.Configuration;


namespace CapaVisual_Login.Reportes
{
    public class Impresor
    {
        FrmMensajes _FrmMensajes = new FrmMensajes();
        private int m_currentPageIndex;
        private IList<Stream> m_streams;
        private string Impresora = (ConfigurationManager.AppSettings.Get("ImpresoraTienda")); // Impresora definida desde el config
        private int totalPages; // Variable para almacenar el número de páginas

        // Creamos el stream con el que vamos a trabajar y en el que meteremos el report
        private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }

        // Exporta el report indicado a un archivo EMF (Enhanced Metafile).
        private void Export(LocalReport report)
        {
            try
            {
                string deviceInfo =
                  @"<DeviceInfo>
                    <OutputFormat>EMF</OutputFormat>
                    <PageWidth>8.5in</PageWidth>
                    <PageHeight>11in</PageHeight>
                    <MarginTop>0.25in</MarginTop>
                    <MarginLeft>0.25in</MarginLeft>
                    <MarginRight>0.25in</MarginRight>
                    <MarginBottom>0.25in</MarginBottom>
                    <PrintDpiX>300</PrintDpiX>
                    <PrintDpiY>300</PrintDpiY>
                    <DpiX>300</DpiX>
                    <DpiY>300</DpiY>
                </DeviceInfo>";
                Warning[] warnings;
                m_streams = new List<Stream>();
                report.Render("Image", deviceInfo, CreateStream, out warnings);

                foreach (Stream stream in m_streams)
                {
                    stream.Position = 0;
                }

                // Almacenar el número de páginas
                totalPages = m_streams.Count;
            }
            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                // Manejar el error según sea necesario
            }
        }

        // Handler para los eventos PrintPageEvents
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new Metafile(m_streams[m_currentPageIndex]);

            // Ajustar el área rectangular con los márgenes de la impresora.
            Rectangle adjustedRect = new Rectangle(
                ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                ev.PageBounds.Width,
                ev.PageBounds.Height);

            // Dibuja un fondo blanco para el report
            ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

            // Dibuja el contenido del report
            ev.Graphics.DrawImage(pageImage, adjustedRect);

            // Lo prepara para la siguiente página y comprueba que no ha llegado al final
            m_currentPageIndex++;
            ev.HasMorePages = false; // Solo imprimimos una página
        }

        private void Print()
        {
            PrintDocument printDoc;

            // String printerName = ImpresoraPredeterminada(); // Se usa para imprimir con la impresora predeterminada
            String printerName = Impresora; // En este caso lee la impresora configurada desde el app config

            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: No hay datos que imprimir.");

            printDoc = new PrintDocument();
            printDoc.PrinterSettings.PrinterName = printerName;
            if (!printDoc.PrinterSettings.IsValid)
            {
                string mensajer = "No se encontró la impresora";
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(mensajer);
                _FrmMensajes.ShowDialog();
            }
            else
            {
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                m_currentPageIndex = 0;
                printDoc.Print();
            }
        }

        private string ImpresoraPredeterminada() // Función que se encarga de imprimir con la impresora predeterminada
        {
            for (Int32 i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                PrinterSettings a = new PrinterSettings();
                a.PrinterName = PrinterSettings.InstalledPrinters[i].ToString();
                if (a.IsDefaultPrinter)
                {
                    return PrinterSettings.InstalledPrinters[i].ToString();
                }
            }
            return "";
        }

        // Exporta el report a un archivo .emf y lo imprime
        public void Imprime(LocalReport rdlc)
        {
            Export(rdlc);
            Print();
        }

        public int GetTotalPages()
        {
            return totalPages;
        }

        public void Dispose()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();
                m_streams = null;
            }
        }
    }
}
