using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades;
using CapaLogica.DatosGeneralesSucursal_Logica;
using ClosedXML.Excel;
using DataAccess.ExportarArchivos;

namespace CapaLogica.ExportarArchivos
{
    public class ExportarXLSX_ReporteGlobal : ExportarArchivoXLSX_ReporteGlobal
    {

        public override async Task<byte[]> ExportAsync(List<ReporteGlobal_HojasExcels> hojasExcels, DateTime fechaSeleccionada)
        {
            var datosSucursal = new L_DatosGenerales();
            await datosSucursal.ObtenerDatosSucursalYCompania_Global();

            using (XLWorkbook workbook = new XLWorkbook())
            {

                foreach (var hoja in hojasExcels)
                {
                    if (hoja.Datos == null)
                    {
                        hoja.Datos = new List<object>(); 
                    }
 
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
                    else if(hoja.NombreHoja == "Pagos del Dia")
                    {
                        var data = hoja.Datos.Cast<ReporteGlobal_PagosDia>().ToList();
                        GenerarHojaPagosDia(worksheet, data, fechaSeleccionada);
                    }
                    else if (hoja.NombreHoja == "Vueltos del Dia")
                    {
                        var data = hoja.Datos.Cast<ReporteGlobal_VueltosDia>().ToList();
                        GenerarHojaVueltosDia(worksheet, data, fechaSeleccionada);
                    }
                    else if (hoja.NombreHoja == "Notas del Dia")
                    {
                        var data = hoja.Datos.Cast<ReporteGlobal_NotasDia>().ToList();
                        GenerarHojaNotasDia(worksheet, data, fechaSeleccionada);
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
            ws.Range("A1:C1").Merge();
            ws.Cell("A1").Value = $"Compañía: {VariablesGlobales.Compania}";
            ws.Range("A2:C2").Merge();
            ws.Cell("A2").Value = $"Rif: {VariablesGlobales.Rif}";
            ws.Range("A3:C3").Merge();
            ws.Cell("A3").Value = $"Sucursal: {VariablesGlobales.CodSucursal} - {VariablesGlobales.Sucursal}";

            ws.Range("E1:F1").Merge();
            ws.Cell("E1").Value = $"Fecha: {DateTime.Now:dd/MM/yyyy}";
            ws.Range("E2:F2").Merge();
            ws.Cell("E2").Value = $"Hora: {DateTime.Now:hh:mm tt}";
            //ws.Cell("F3").Value = "Pag: 1 de 1";

            // Opcional: Negritas y tamaño de letra
            ws.Range("A1:A3").Style.Font.Bold = true;
            ws.Range("F1:F3").Style.Font.Bold = true;

            // ======= TÍTULO DEL REPORTE =======
            ws.Range("D5:E5").Merge();
            ws.Cell("D5").Value = "Cierre de caja";
            ws.Range("D5:E5").Style.Font.Bold = true;
            ws.Range("D5:E5").Style.Font.FontSize = 14;

            ws.Range("D6:E6").Merge();
            ws.Cell("D6").Value = $"fecha: {fecha:dd/MM/yyyy}";
            ws.Range("D6:E6").Style.Font.Italic = true;

            // ======= TABLA DE DETALLE =======
            string[,] campos =
            {
                { "Efectivo", "MANUALEFECTIVO", "SISTEMAEFECTIVO" },
                { "Tarjeta Débito", "MANUALDEBITO", "SISTEMADEBITO" },
                { "Tarjeta Crédito", "MANUALCREDITO", "SISTEMACREDITO" },
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
            ws.Range("C8:F8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range("C8:F8").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

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
                ws.Cell($"D{row}").Style.NumberFormat.Format = "#,##0.00";  

                ws.Cell($"E{row}").Value = sistema;
                ws.Cell($"E{row}").Style.NumberFormat.Format = "#,##0.00";

                ws.Cell($"F{row}").Value = diferencia;
                ws.Cell($"F{row}").Style.NumberFormat.Format = "#,##0.00";  //
            }

            // ======= TOTALES AL FINAL =======
            int ultimaFila = 9 + campos.GetLength(0);

            ws.Range($"C{ultimaFila}:F{ultimaFila}").Style.Border.TopBorder = XLBorderStyleValues.Thick;
            ws.Range($"C{ultimaFila}:F{ultimaFila}").Style.Border.TopBorderColor = XLColor.Black;

            ws.Cell($"D{ultimaFila + 1}").Value = "Total Bruto:";
            ws.Cell($"E{ultimaFila + 1}").Value = data.TOTAL_BRUTO;
            ws.Cell($"E{ultimaFila + 1}").Style.NumberFormat.Format = "#,##0.00";

            ws.Cell($"D{ultimaFila + 2}").Value = "Impuesto:";
            ws.Cell($"E{ultimaFila + 2}").Value = data.IMPUESTO;
            ws.Cell($"E{ultimaFila + 2}").Style.NumberFormat.Format = "#,##0.00";

            ws.Cell($"D{ultimaFila + 3}").Value = "Total Neto:";
            ws.Cell($"E{ultimaFila + 3}").Value = data.TOTAL_NETO;
            ws.Cell($"E{ultimaFila + 3}").Style.NumberFormat.Format = "#,##0.00";

            // Estilos para totales
            ws.Range($"D{ultimaFila + 1}:E{ultimaFila + 3}").Style.Font.Bold = true;
            ws.Range($"D{ultimaFila + 1}:E{ultimaFila + 3}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range($"D{ultimaFila + 1}:E{ultimaFila + 3}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range($"D{ultimaFila + 1}:E{ultimaFila + 3}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            //    ws.Range($"D{ultimaFila + 1}:E{ultimaFila + 3}").Style.Fill.BackgroundColor = XLColor.FromHtml("#e0f7fa");
            //    ws.Range($"D{ultimaFila + 1}:E{ultimaFila + 3}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }

        public void GenerarHojaFacturas(IXLWorksheet ws, List<ReporteGlobal_Facturas> facturas, DateTime fecha)
        {
            // ======= ENCABEZADO (ARRIBA) =======
            ws.Range("A1:C1").Merge();
            ws.Cell("A1").Value = $"Compañía: {VariablesGlobales.Compania}";

            ws.Range("A2:C2").Merge();
            ws.Cell("A2").Value = $"Rif: {VariablesGlobales.Rif}";

            ws.Range("A3:C3").Merge();
            ws.Cell("A3").Value = $"Sucursal: {VariablesGlobales.CodSucursal} - {VariablesGlobales.Sucursal}";

            ws.Range("H1:I1").Merge();
            ws.Cell("H1").Value = $"Fecha: {DateTime.Now:dd/MM/yyyy}";

            ws.Range("H2:I2").Merge();
            ws.Cell("H2").Value = $"Hora: {DateTime.Now:hh:mm tt}";

            // Opcional: Negritas y tamaño de letra
            ws.Range("A1:A3").Style.Font.Bold = true;
            ws.Range("F1:F3").Style.Font.Bold = true;

            // ======= TÍTULO DEL REPORTE =======
            ws.Range("D4:E4").Merge();
            ws.Cell("D4").Value = "Cierre de Caja";
            ws.Range("D4:E4").Style.Font.Italic = true;
            ws.Range("D4:E4").Style.Font.FontSize = 12;
            ws.Range("D4:E4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Range("D5:E5").Merge();
            ws.Cell("D5").Value = "Facturas";
            ws.Range("D5:E5").Style.Font.Bold = true;
            ws.Range("D5:E5").Style.Font.FontSize = 14;
            ws.Range("D5:E5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Range("D6:E6").Merge();
            ws.Cell("D6").Value = $"fecha: {fecha:dd/MM/yyyy}";
            ws.Range("D6:E6").Style.Font.Italic = true;
            ws.Range("D6:E6").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;


            // ======= ENCABEZADOS DE LA TABLA =======
            ws.Cell("A8").Value = "Factura";
            ws.Cell("B8").Value = "Orden";
            ws.Cell("C8").Value = "Cédula";
            ws.Cell("D8").Value = "Nombre Cliente";
            ws.Cell("E8").Value = "SubTotal";
            ws.Cell("F8").Value = "Descuento";
            ws.Cell("G8").Value = "IVA";
            ws.Cell("H8").Value = "IGTF";
            ws.Cell("I8").Value = "Total";

            ws.Columns("A", "I").Width = 20;

            var headerRange = ws.Range("A8:I8");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#027a65");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // ======= LLENAR DATOS =======
            int fila = 9;
            int filaDatosFinal = 0;

            foreach (var factura in facturas)
            {
                if (!factura.EsTotal)
                {
                    ws.Cell(fila, 1).Value = factura.Fact_Num;
                    ws.Cell(fila, 2).Value = factura.NumOrdServ;
                    ws.Cell(fila, 3).Value = factura.CTE_CedIdenPAG;
                    ws.Cell(fila, 4).Value = factura.CTE_PNombre;
                    ws.Cell(fila, 5).Value = factura.Fact_SubTotal;
                    ws.Cell(fila, 6).Value = factura.Fact_Descuento;
                    ws.Cell(fila, 7).Value = factura.Fact_Impuesto;
                    ws.Cell(fila, 8).Value = factura.Fact_IGTF;
                    ws.Cell(fila, 9).Value = factura.Fact_Total;

                    ws.Range($"E{fila}:I{fila}").Style.NumberFormat.Format = "#,##0.00";

                    filaDatosFinal = fila; // marcamos la última fila de datos reales
                }

                fila++;
            }

            // ======= LÍNEA NEGRA SEPARADORA DINÁMICA =======
            int filaSeparadora = filaDatosFinal + 1;
            ws.Range($"A{filaSeparadora}:I{filaSeparadora}").Style.Border.TopBorder = XLBorderStyleValues.Thick;
            ws.Range($"A{filaSeparadora}:I{filaSeparadora}").Style.Border.TopBorderColor = XLColor.Black;

            // ======= TOTALES =======
            var filaTotales = filaSeparadora + 1;

            var totales = facturas.FirstOrDefault(x => x.EsTotal);
            if (totales != null)
            {
                ws.Cell(filaTotales, 1).Value = totales.Fact_Num;
                ws.Cell(filaTotales, 2).Value = totales.TotalOrden;
                ws.Cell(filaTotales, 5).Value = totales.TotalSubtotal;
                ws.Cell(filaTotales, 7).Value = totales.TotalImpuesto;
                ws.Cell(filaTotales, 8).Value = totales.TotalIGTF;
                ws.Cell(filaTotales, 9).Value = totales.Totaltotal;

                ws.Range($"A{filaTotales}:I{filaTotales}").Style.Font.Bold = true;
                ws.Range($"E{filaTotales}:I{filaTotales}").Style.NumberFormat.Format = "#,##0.00";
                ws.Range($"A{filaTotales}:I{filaTotales}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }

            ws.Columns().AdjustToContents();
        }

        public void GenerarHojaVueltosDia(IXLWorksheet ws, List<ReporteGlobal_VueltosDia> vueltosDia, DateTime fecha)
        {
            // ======= ENCABEZADO (ARRIBA) =======
            ws.Range("A1:C1").Merge();
            ws.Cell("A1").Value = $"Compañía: {VariablesGlobales.Compania}";

            ws.Range("A2:C2").Merge();
            ws.Cell("A2").Value = $"Rif: {VariablesGlobales.Rif}";

            ws.Range("A3:C3").Merge();
            ws.Cell("A3").Value = $"Sucursal: {VariablesGlobales.CodSucursal} - {VariablesGlobales.Sucursal}";

            ws.Range("H1:I1").Merge();
            ws.Cell("H1").Value = $"Fecha: {DateTime.Now:dd/MM/yyyy}";

            ws.Range("H2:I2").Merge();
            ws.Cell("H2").Value = $"Hora: {DateTime.Now:hh:mm tt}";

            // Opcional: Negritas y tamaño de letra
            ws.Range("A1:A3").Style.Font.Bold = true;
            ws.Range("F1:F3").Style.Font.Bold = true;

            // ======= TÍTULO DEL REPORTE =======
            // ======= TÍTULO DEL REPORTE =======
            ws.Range("D4:E4").Merge();
            ws.Cell("D4").Value = "Cierre de Caja";
            ws.Range("D4:E4").Style.Font.Italic = true;
            ws.Range("D4:E4").Style.Font.FontSize = 12;
            ws.Range("D4:E4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Range("D5:E5").Merge();
            ws.Cell("D5").Value = "Vueltos del Día";
            ws.Range("D5:E5").Style.Font.Bold = true;
            ws.Range("D5:E5").Style.Font.FontSize = 14;
            ws.Range("D5:E5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Range("D6:E6").Merge();
            ws.Cell("D6").Value = $"fecha: {fecha:dd/MM/yyyy}";
            ws.Range("D6:E6").Style.Font.Italic = true;
            ws.Range("D6:E6").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;


            // Encabezados de la tabla

            ws.Cell("B8").Value = "Orden";
            ws.Cell("C8").Value = "Referencia";
            ws.Cell("D8").Value = "Banco Emisor";
            ws.Cell("E8").Value = "Banco Receptor";
            ws.Cell("F8").Value = "Vuelto BS";
            ws.Cell("G8").Value = "Vuelto Divisa";

            ws.Columns("B", "G").Width = 20;

            var headerRange = ws.Range("B8:G8");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#027a65");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // ======= LLENAR DATOS =======  
            int fila = 9;
            int filaDatosFinal = 0;

            foreach (var vuelto in vueltosDia)
            {
                if (vuelto.EsTotal)
                {
                    ws.Cell(fila, 2).Value = vuelto.TextoTotal ?? "Totales:";
                    ws.Cell(fila, 3).Value = vuelto.TotalOrden;
                    ws.Cell(fila, 6).Value = vuelto.TotalVueltoBS;
                    ws.Cell(fila, 7).Value = vuelto.TotalVueltoDivisa;

                    ws.Range($"B{fila}:G{fila}").Style.Font.Bold = true;
                    ws.Range($"B{fila}:G{fila}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    ws.Range($"E{fila}:G{fila}").Style.NumberFormat.Format = "#,##0.00";

                    //filaDatosFinal = fila;
                }
                else
                {
                    ws.Cell(fila, 2).Value = vuelto.NumOrden;
                    ws.Cell(fila, 3).Value = vuelto.Referencia;
                    ws.Cell(fila, 4).Value = vuelto.BancoEmisor;
                    ws.Cell(fila, 5).Value = vuelto.BancoReceptor;
                    ws.Cell(fila, 6).Value = vuelto.VueltoBS;
                    ws.Cell(fila, 7).Value = vuelto.VueltoDivisa;

                    ws.Range($"B{fila}:G{fila}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range($"E{fila}:G{fila}").Style.NumberFormat.Format = "#,##0.00";

                    filaDatosFinal = fila;
                }

                fila++;
            }

            // LÍNEA NEGRA SEPARADORA DINÁMICA
            int filaSeparadora = filaDatosFinal + 1;
            ws.Range($"B{filaSeparadora}:G{filaSeparadora}").Style.Border.TopBorder = XLBorderStyleValues.Thick;
            ws.Range($"B{filaSeparadora}:G{filaSeparadora}").Style.Border.TopBorderColor = XLColor.Black;

            ws.Columns().AdjustToContents();
        }

        public void GenerarHojaPagosDia(IXLWorksheet ws, List<ReporteGlobal_PagosDia> pagosDia, DateTime fecha)
        {
            // ======= ENCABEZADO (ARRIBA) =======
            ws.Range("A1:C1").Merge();
            ws.Cell("A1").Value = $"Compañía: {VariablesGlobales.Compania}";

            ws.Range("A2:C2").Merge();
            ws.Cell("A2").Value = $"Rif: {VariablesGlobales.Rif}";

            ws.Range("A3:C3").Merge();
            ws.Cell("A3").Value = $"Sucursal: {VariablesGlobales.CodSucursal} - {VariablesGlobales.Sucursal}";

            ws.Range("H1:I1").Merge();
            ws.Cell("H1").Value = $"Fecha: {DateTime.Now:dd/MM/yyyy}";

            ws.Range("H2:I2").Merge();
            ws.Cell("H2").Value = $"Hora: {DateTime.Now:hh:mm tt}";

            // Opcional: Negritas y tamaño de letra
            ws.Range("A1:A3").Style.Font.Bold = true;
            ws.Range("F1:F3").Style.Font.Bold = true;

            // ======= TÍTULO DEL REPORTE =======
            ws.Range("D4:E4").Merge();
            ws.Cell("D4").Value = "Cierre de Caja";
            ws.Range("D4:E4").Style.Font.Italic = true;
            ws.Range("D4:E4").Style.Font.FontSize = 12;
            ws.Range("D4:E4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Range("D5:E5").Merge();
            ws.Cell("D5").Value = "Pagos del Día";
            ws.Range("D5:E5").Style.Font.Bold = true;
            ws.Range("D5:E5").Style.Font.FontSize = 14;
            ws.Range("D5:E5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Range("D6:E6").Merge();
            ws.Cell("D6").Value = $"fecha: {fecha:dd/MM/yyyy}";
            ws.Range("D6:E6").Style.Font.Italic = true;
            ws.Range("D6:E6").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;


            // Encabezados de la tabla

            ws.Cell("B8").Value = "Orden";
            ws.Cell("C8").Value = "Tipo Venta";
            ws.Cell("D8").Value = "Fecha";
            ws.Cell("E8").Value = "Cédula";
            ws.Cell("F8").Value = "Nombre Cliente";
            ws.Cell("G8").Value = "Tipo Pago";
            ws.Cell("H8").Value = "Pago";

            ws.Columns("B", "H").Width = 20;

            var headerRange = ws.Range("B8:H8");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#027a65");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // ======= LLENAR DATOS =======
            int fila = 9;
            int filaDatosFinal = 0;
            foreach (var pago in pagosDia)
            {
                if (pago.EsTotal)
                {
                    ws.Cell(fila, 2).Value = pago.TextoTotal ?? "Totales:";
                    ws.Cell(fila, 3).Value = pago.TotalOrden;
                    ws.Cell(fila, 8).Value = pago.TotalPago;

                    ws.Range($"B{fila}:H{fila}").Style.Font.Bold = true;
                    ws.Range($"B{fila}:H{fila}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range($"H{fila}").Style.NumberFormat.Format = "#,##0.00";
                }
                else
                {
                    ws.Cell(fila, 2).Value = pago.OrdenServicio;
                    ws.Cell(fila, 3).Value = pago.TipoVenta;
                    ws.Cell(fila, 4).Value = pago.Fecha;
                    ws.Cell(fila, 5).Value = pago.Cedula;
                    ws.Cell(fila, 6).Value = pago.NombreCliente;
                    ws.Cell(fila, 7).Value = pago.TipoPago;
                    ws.Cell(fila, 8).Value = pago.Pago;

                    ws.Range($"B{fila}:H{fila}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range($"H{fila}").Style.NumberFormat.Format = "#,##0.00";

                    filaDatosFinal = fila;
                }

                fila++;
            }

            // LÍNEA NEGRA SEPARADORA DINÁMICA
            int filaSeparadora = filaDatosFinal + 1;
            ws.Range($"B{filaSeparadora}:H{filaSeparadora}").Style.Border.TopBorder = XLBorderStyleValues.Thick;
            ws.Range($"B{filaSeparadora}:H{filaSeparadora}").Style.Border.TopBorderColor = XLColor.Black;

            ws.Columns().AdjustToContents();
        }

        public void GenerarHojaNotasDia(IXLWorksheet ws, List<ReporteGlobal_NotasDia> notasDia, DateTime fecha)
        {
            // ======= ENCABEZADO (ARRIBA) =======
            ws.Range("A1:C1").Merge();
            ws.Cell("A1").Value = $"Compañía: {VariablesGlobales.Compania}";
            ws.Range("A2:C2").Merge();
            ws.Cell("A2").Value = $"Rif: {VariablesGlobales.Rif}";
            ws.Range("A3:C3").Merge();
            ws.Cell("A3").Value = $"Sucursal: {VariablesGlobales.CodSucursal} - {VariablesGlobales.Sucursal}";

            ws.Range("H1:I1").Merge();
            ws.Cell("H1").Value = $"Fecha: {DateTime.Now:dd/MM/yyyy}";
            ws.Range("H2:I2").Merge();
            ws.Cell("H2").Value = $"Hora: {DateTime.Now:hh:mm tt}";

            // Opcional: Negritas y tamaño de letra
            ws.Range("A1:A3").Style.Font.Bold = true;
            ws.Range("F1:F3").Style.Font.Bold = true;

            // ======= TÍTULO DEL REPORTE =======
            ws.Range("D4:E4").Merge();
            ws.Cell("D4").Value = "Cierre de Caja";
            ws.Range("D4:E4").Style.Font.Italic = true;
            ws.Range("D4:E4").Style.Font.FontSize = 12;
            ws.Range("D4:E4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Range("D5:E5").Merge();
            ws.Cell("D5").Value = "Notas del Día";
            ws.Range("D5:E5").Style.Font.Bold = true;
            ws.Range("D5:E5").Style.Font.FontSize = 14;
            ws.Range("D5:E5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Range("D6:E6").Merge();
            ws.Cell("D6").Value = $"fecha: {fecha:dd/MM/yyyy}";
            ws.Range("D6:E6").Style.Font.Italic = true;
            ws.Range("D6:E6").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // ======= ENCABEZADOS DE LA TABLA =======
            ws.Cell("B8").Value = "Orden";
            ws.Cell("C8").Value = "Número Control";
            ws.Cell("D8").Value = "Cédula";
            ws.Cell("E8").Value = "Nombre Cliente";
            ws.Cell("F8").Value = "Factura";
            ws.Cell("G8").Value = "Monto";
            ws.Cell("H8").Value = "Aplicado";
            ws.Cell("I8").Value = "Saldo";

            ws.Columns("B", "I").Width = 20;

            var headerRange = ws.Range("B8:I8");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#027a65");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // ======= LLENAR DATOS =======
            int fila = 9;
            int filaDatosFinal = 0;
            foreach (var nota in notasDia)
            {
                if (nota.EsTotal)
                {
                    ws.Cell(fila, 2).Value = nota.TextoTotal ?? "Total Notas:";
                    ws.Cell(fila, 3).Value = nota.TotalOrden;
                    ws.Cell(fila, 7).Value = nota.TotalMonto;
                    ws.Cell(fila, 8).Value = nota.TotalAplicado;
                    ws.Cell(fila, 9).Value = nota.TotalSaldo;

                    ws.Range($"B{fila}:I{fila}").Style.Font.Bold = true;
                    ws.Range($"B{fila}:I{fila}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range($"F{fila}:I{fila}").Style.NumberFormat.Format = "#,##0.00";

                    //filaDatosFinal = fila;
                }
                else
                {
                    ws.Cell(fila, 2).Value = nota.Numero;
                    ws.Cell(fila, 3).Value = nota.NumeroControl;
                    ws.Cell(fila, 4).Value = nota.Cedula;
                    ws.Cell(fila, 5).Value = nota.NombreCliente;
                    ws.Cell(fila, 6).Value = nota.Factura;
                    ws.Cell(fila, 7).Value = nota.Monto;
                    ws.Cell(fila, 8).Value = nota.Aplicado;
                    ws.Cell(fila, 9).Value = nota.Saldo;

                    ws.Range($"B{fila}:I{fila}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range($"F{fila}:HI{fila}").Style.NumberFormat.Format = "#,##0.00";

                    filaDatosFinal = fila;
                }

                fila++;
            }

            // LÍNEA NEGRA SEPARADORA DINÁMICA
            int filaSeparadora = filaDatosFinal + 1;
            ws.Range($"B{filaSeparadora}:I{filaSeparadora}").Style.Border.TopBorder = XLBorderStyleValues.Thick;
            ws.Range($"B{filaSeparadora}:I{filaSeparadora}").Style.Border.TopBorderColor = XLColor.Black;

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

