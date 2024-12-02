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
                    <MarginTop>0.10in</MarginTop>
                    <MarginLeft>0.10in</MarginLeft>
                    <MarginRight>0.10in</MarginRight>
                    <MarginBottom>0.10in</MarginBottom>
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

            //// Configurar el tamaño del papel a media carta (Half Letter)
            //PaperSize paperSize = new PaperSize("HalfLetter", 550, 850); // 5.5 x 8.5 pulgadas

            //printDoc.PrinterSettings.DefaultPageSettings.PaperSize = paperSize;


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
            Dispose();
        }

        // Exporta el report a un archivo .emf y lo imprime el número de veces especificado
        public void Imprime_NumeroCopias(LocalReport rdlc, int numeroDeCopias)
        {
            Export(rdlc);
            for (int i = 0; i < numeroDeCopias; i++)
            {
                m_currentPageIndex = 0; // Reiniciar el índice de página para cada copia
                Print();
            }
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

//namespace CapaVisual_Login.Reportes
//{
//    public class Impresor
//    {
//        FrmMensajes _FrmMensajes = new FrmMensajes();
//        private int m_currentPageIndex;
//        private IList<Stream> m_streams;
//        private string Impresora = ConfigurationManager.AppSettings.Get("ImpresoraTienda"); // Impresora definida desde el config
//        private int totalPages; // Variable para almacenar el número de páginas

//        // Creamos el stream con el que vamos a trabajar y en el que meteremos el report
//        private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
//        {
//            Stream stream = new MemoryStream();
//            m_streams.Add(stream);
//            return stream;
//        }

//        // Método para cortar el stream a tamaño media carta
//        //private Stream CropStreamToHalfLetter(Stream inputStream)
//        //{
//        //    inputStream.Position = 0;
//        //    using (var originalImage = new Metafile(inputStream))
//        //    {
//        //        var halfLetterSize = new Size(850, 550);
//        //        var croppedStream = new MemoryStream();

//        //        using (var graphics = Graphics.FromImage(new Bitmap(halfLetterSize.Width, halfLetterSize.Height)))
//        //        {
//        //            IntPtr hdc = graphics.GetHdc();
//        //            using (var metafile = new Metafile(croppedStream, hdc, new Rectangle(0, 0, halfLetterSize.Width, halfLetterSize.Height), MetafileFrameUnit.Pixel))
//        //            {
//        //                graphics.ReleaseHdc(hdc);
//        //                using (var g = Graphics.FromImage(metafile))
//        //                {
//        //                    g.DrawImage(originalImage, new Rectangle(Point.Empty, halfLetterSize));
//        //                }
//        //            }
//        //        }

//        //        croppedStream.Position = 0;
//        //        return croppedStream;
//        //    }
//        //}

//        private Stream CombineStreamsVertically(IList<Stream> streams1, IList<Stream> streams2)
//        {

//            // Crear un MemoryStream para el resultado combinado
//            var combinedStream = new MemoryStream();
//            // Definir el tamaño de media carta
//            var halfLetterSize = new Size(850, 550);
//            // Definir el tamaño carta
//            var letterSize = new Size(850, 1100);
//            // Definir la mitdad de la hoja 
//            var halfWidth = halfLetterSize.Width / 2;

//            // Definir la mitdad de la hoja 
//            var halfHeight = halfLetterSize.Height / 2;

//            // Crear un Graphics para dibujar en una imagen del doble de la altura de media carta
//            using (var graphics = Graphics.FromImage(new Bitmap(letterSize.Width, letterSize.Height/2)))
//            {
//                // Obtener el hdc (handle device context) del Graphics
//                IntPtr hdc = graphics.GetHdc();
//                // Crear un Metafile con el tamaño combinado (doble de la altura de media carta)
//                using (var metafile = new Metafile(combinedStream, hdc, new Rectangle(0, 0, letterSize.Width, letterSize.Height/2), MetafileFrameUnit.Pixel))
//                {
//                    // Liberar el hdc del Graphics
//                    graphics.ReleaseHdc(hdc);

//                    // Dibujar el contenido de los streams en el Metafile
//                    using (var g = Graphics.FromImage(metafile))
//                    {
//                        int yOffset = 0;
//                        // Iterar sobre streams1 y dibujar cada imagen recortada a la mitad verticalmente
//                        foreach (var stream in streams1)
//                        {
//                            stream.Position = 0;
//                            using (var img = Image.FromStream(stream))
//                            {
//                                g.DrawImage(img, new Rectangle(0, yOffset, halfLetterSize.Width, halfHeight));
//                                yOffset += halfLetterSize.Height;
//                            }
//                        }
//                        // Iterar sobre streams2 y dibujar cada imagen recortada a la mitad verticalmente debajo de las imágenes de streams1
//                        foreach (var stream in streams2)
//                        {
//                            stream.Position = 0;
//                            using (var img = Image.FromStream(stream))
//                            {
//                                g.DrawImage(img, new Rectangle(0, yOffset, halfLetterSize.Width, halfHeight));
//                                yOffset += halfLetterSize.Height;
//                            }
//                        }
//                    }
//                }
//            }
//            // Reiniciar la posición del combinedStream a 0
//            combinedStream.Position = 0;
//            return combinedStream;
//        }


//        // Exporta los reportes indicados a un archivo EMF (Enhanced Metafile).
//        private void Export(LocalReport report1, LocalReport report2, string deviceInfo)
//        {
//            try
//            {
//                // Exportar el primer reporte
//                Warning[] warnings;
//                m_streams = new List<Stream>();
//                report1.Render("Image", deviceInfo, CreateStream, out warnings);
//                IList<Stream> streams1 = new List<Stream>(m_streams);

//                // Exportar el segundo reporte si no es null
//                IList<Stream> streams2 = new List<Stream>();
//                if (report2 != null)
//                {
//                    m_streams = new List<Stream>(); // Reiniciar los streams
//                    report2.Render("Image", deviceInfo, CreateStream, out warnings);
//                    streams2 = new List<Stream>(m_streams);
//                }

//                // Combinar los streams verticalmente
//                m_streams = new List<Stream> { CombineStreamsVertically(streams1, streams2) };


//                //// Combinar los streams
//                //m_streams = new List<Stream>();
//                //foreach (var stream in streams1)
//                //{
//                //    m_streams.Add(stream);
//                //}
//                //foreach (var stream in streams2)
//                //{
//                //    m_streams.Add(stream);
//                //}

//                //// Reiniciar la posición de los streams
//                //foreach (Stream stream in m_streams)
//                //{
//                //    stream.Position = 0;
//                //}

//                //// Guardar los streams en archivos temporales para ver el contenido (opcional)
//                 SaveStreamsToFiles(m_streams);

//                // Almacenar el número de páginas
//                totalPages = m_streams.Count;

//                // Imprimir los streams combinados
//                Print1();
//            }

//            catch (Exception ex)
//            {
//                string Error = string.Format("Error: {0}", ex.Message);
//                // Manejar el error según sea necesario
//            }
//        }

//        private void Export1(LocalReport report)
//        {
//            try
//            {
//                string deviceInfo =
//                  @"<DeviceInfo>
//                    <OutputFormat>EMF</OutputFormat>
//                    <PageWidth>8.5in</PageWidth>
//                    <PageHeight>11in</PageHeight>
//                    <MarginTop>0.25in</MarginTop>
//                    <MarginLeft>0.25in</MarginLeft>
//                    <MarginRight>0.25in</MarginRight>
//                    <MarginBottom>0.25in</MarginBottom>
//                    <PrintDpiX>300</PrintDpiX>
//                    <PrintDpiY>300</PrintDpiY>
//                    <DpiX>300</DpiX>
//                    <DpiY>300</DpiY>
//                </DeviceInfo>";
//                Warning[] warnings;
//                m_streams = new List<Stream>();
//                report.Render("Image", deviceInfo, CreateStream, out warnings);

//                foreach (Stream stream in m_streams)
//                {
//                    stream.Position = 0;
//                }

//                // Almacenar el número de páginas
//                totalPages = m_streams.Count;
//            }
//            catch (Exception ex)
//            {
//                string Error = string.Format("Error: {0}", ex.Message);
//                // Manejar el error según sea necesario
//            }
//        }



//        private void SaveStreamsToFiles(IList<Stream> streams)
//        {
//            for (int i = 0; i < streams.Count; i++)
//            {
//                streams[i].Position = 0;
//                string filePath = Path.Combine(Path.GetTempPath(), $"stream_{i}.emf");
//                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
//                {
//                    streams[i].CopyTo(fileStream);
//                }
//                Console.WriteLine($"Stream {i} saved to {filePath}");
//            }
//        }



//        // Handler para los eventos PrintPageEvents
//        private void PrintPage(object sender, PrintPageEventArgs ev)
//        {

//            Metafile pageImage = new Metafile(m_streams[m_currentPageIndex]);

//            // Ajustar el área rectangular con los márgenes de la impresora.
//            Rectangle adjustedRect = new Rectangle(
//                ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
//                ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
//                ev.PageBounds.Width,
//                ev.PageBounds.Height);

//            // Dibuja un fondo blanco para el report
//            ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

//            // Dibuja el contenido del report
//            ev.Graphics.DrawImage(pageImage, adjustedRect);

//            // Lo prepara para la siguiente página y comprueba que no ha llegado al final
//            m_currentPageIndex++;
//            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
//            //ev.HasMorePages = false; // Solo imprimimos una página
//        }

//        private void Print(IList<Stream> streams)
//        {
//            PrintDocument printDoc;

//            // String printerName = ImpresoraPredeterminada(); // Se usa para imprimir con la impresora predeterminada
//            String printerName = Impresora; // En este caso lee la impresora configurada desde el app config

//            if (streams == null || streams.Count == 0)
//                throw new Exception("Error: No hay datos que imprimir.");

//            m_streams = streams;

//            printDoc = new PrintDocument();
//            printDoc.PrinterSettings.PrinterName = printerName;

//            // Definir el tamaño del papel (media carta)
//            PaperSize paperSize = new PaperSize("HalfLetter", 550, 850); // 5.5 x 8.5 pulgadas

//            printDoc.DefaultPageSettings.PaperSize = paperSize;

//            if (!printDoc.PrinterSettings.IsValid)
//            {
//                string mensajer = "No se encontró la impresora";
//                _FrmMensajes.co = 2;
//                _FrmMensajes.avisomensaje(mensajer);
//                _FrmMensajes.ShowDialog();
//            }
//            else
//            {
//                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
//                m_currentPageIndex = 0;
//                printDoc.Print();
//            }
//        }

//        private string ImpresoraPredeterminada() // Función que se encarga de imprimir con la impresora predeterminada
//        {
//            for (Int32 i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
//            {
//                PrinterSettings a = new PrinterSettings();
//                a.PrinterName = PrinterSettings.InstalledPrinters[i].ToString();
//                if (a.IsDefaultPrinter)
//                {
//                    return PrinterSettings.InstalledPrinters[i].ToString();
//                }
//            }
//            return "";
//        }

//        // Exporta los reportes a archivos .emf y los imprime
//        public void Imprime2(LocalReport rdlc1, LocalReport rdlc2)
//        {
//            // DeviceInfo para carta
//            string deviceInfo =
//                   @"<DeviceInfo>
//                    <OutputFormat>EMF</OutputFormat>
//                    <PageWidth>8.5in</PageWidth>
//                    <PageHeight>11in</PageHeight>
//                    <MarginTop>0.25in</MarginTop>
//                    <MarginLeft>0.25in</MarginLeft>
//                    <MarginRight>0.25in</MarginRight>
//                    <MarginBottom>0.25in</MarginBottom>
//                    <PrintDpiX>300</PrintDpiX>
//                    <PrintDpiY>300</PrintDpiY>
//                    <DpiX>300</DpiX>
//                    <DpiY>300</DpiY>
//                </DeviceInfo>";

//            // Exportar los reportes y combinarlos en una sola lista de streams
//            Export(rdlc1, rdlc2, deviceInfo);
//        }

//        // Exporta el report a un archivo .emf y lo imprime
//        public void Imprime(LocalReport rdlc)
//        {
//            Export1(rdlc);
//            Print1();
//        }

//        private void Print1()
//        {
//            PrintDocument printDoc;

//            // String printerName = ImpresoraPredeterminada(); // Se usa para imprimir con la impresora predeterminada
//            String printerName = Impresora; // En este caso lee la impresora configurada desde el app config

//            if (m_streams == null || m_streams.Count == 0)
//                throw new Exception("Error: No hay datos que imprimir.");

//            printDoc = new PrintDocument();
//            printDoc.PrinterSettings.PrinterName = printerName;

//            //// Configurar el tamaño del papel a media carta (Half Letter)
//            //PaperSize paperSize = new PaperSize("HalfLetter", 550, 850); // 5.5 x 8.5 pulgadas

//            //printDoc.PrinterSettings.DefaultPageSettings.PaperSize = paperSize;


//            if (!printDoc.PrinterSettings.IsValid)
//            {
//                string mensajer = "No se encontró la impresora";
//                _FrmMensajes.co = 2;
//                _FrmMensajes.avisomensaje(mensajer);
//                _FrmMensajes.ShowDialog();
//            }
//            else
//            {
//                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
//                m_currentPageIndex = 0;
//                printDoc.Print();
//            }
//        }

//        // Exporta el report a un archivo .emf y lo imprime el número de veces especificado
//        public void Imprime_NumeroCopias(LocalReport rdlc, int numeroDeCopias)
//        {
//            string deviceInfo =
//              @"<DeviceInfo>
//                <OutputFormat>EMF</OutputFormat>
//                <PageWidth>8.5in</PageWidth>
//                <PageHeight>11in</PageHeight>
//                <MarginTop>0.25in</MarginTop>
//                <MarginLeft>0.25in</MarginLeft>
//                <MarginRight>0.25in</MarginRight>
//                <MarginBottom>0.25in</MarginBottom>
//                <PrintDpiX>300</PrintDpiX>
//                <PrintDpiY>300</PrintDpiY>
//                <DpiX>300</DpiX>
//                <DpiY>300</DpiY>
//              </DeviceInfo>";

//            Export(rdlc, null, deviceInfo);
//            for (int i = 0; i < numeroDeCopias; i++)
//            {
//                m_currentPageIndex = 0; // Reiniciar el índice de página para cada copia
//                //Print();
//            }
//        }

//        public int GetTotalPages()
//        {
//            return totalPages;
//        }

//        public void Dispose()
//        {
//            if (m_streams != null)
//            {
//                foreach (Stream stream in m_streams)
//                    stream.Close();
//                m_streams = null;
//            }
//        }
//    }
//}