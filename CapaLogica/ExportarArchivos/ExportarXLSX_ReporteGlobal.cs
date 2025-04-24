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
            ws.Cell("A1").Value = $"Compañía: {VariablesGlobales.Compania}";
            ws.Cell("A2").Value = $"Rif: {VariablesGlobales.Rif}";
            ws.Cell("A3").Value = $"Sucursal: {VariablesGlobales.CodSucursal} - {VariablesGlobales.Sucursal}";

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
            ws.Cell("A1").Value = $"Compañía: {VariablesGlobales.Compania}";
            ws.Cell("A2").Value = $"Rif: {VariablesGlobales.Rif}";
            ws.Cell("A3").Value = $"Sucursal: {VariablesGlobales.CodSucursal} - {VariablesGlobales.Sucursal}";

            ws.Cell("G1").Value = $"Fecha: {DateTime.Now:dd/MM/yyyy}";
            ws.Cell("G2").Value = $"Hora: {DateTime.Now:hh:mm tt}";
            //ws.Cell("F3").Value = "Pag: 1 de 1";

            // Opcional: Negritas y tamaño de letra
            ws.Range("A1:A3").Style.Font.Bold = true;
            ws.Range("F1:F3").Style.Font.Bold = true;

            // ======= TÍTULO DEL REPORTE =======
            ws.Cell("D4").Value = "Cierre de Caja";
            ws.Cell("D4").Style.Font.Italic = true;
            ws.Cell("D4").Style.Font.FontSize = 12;

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

            ws.Columns("A", "H").Width = 20;

            var headerRange = ws.Range("A8:H8");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#027a65");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // ======= LLENAR DATOS =======
            int fila = 9;
            foreach (var factura in facturas)
            {
                if (factura.EsTotal)
                {
                    ws.Cell(fila, 1).Value = factura.Fact_Num;
                    ws.Cell(fila, 2).Value = factura.TotalOrden;
                    ws.Cell(fila, 4).Value = factura.TotalSubtotal;
                    ws.Cell(fila, 6).Value = factura.TotalImpuesto;
                    ws.Cell(fila, 7).Value = factura.TotalIGTF;
                    ws.Cell(fila, 8).Value = factura.Totaltotal;


                    ws.Range($"A{fila}:H{fila}").Style.Font.Bold = true;
                    ws.Range($"A{fila}:H{fila}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }
                else
                {
                    ws.Cell(fila, 1).Value = factura.Fact_Num;
                    ws.Cell(fila, 2).Value = factura.NumOrdServ;
                    ws.Cell(fila, 3).Value = factura.CTE_CedIdenPAG;
                    ws.Cell(fila, 4).Value = factura.Fact_SubTotal;
                    ws.Cell(fila, 5).Value = factura.Fact_Descuento;
                    ws.Cell(fila, 6).Value = factura.Fact_Impuesto;
                    ws.Cell(fila, 7).Value = factura.Fact_IGTF;
                    ws.Cell(fila, 8).Value = factura.Fact_Total;
                }

                fila++;
            }

            ws.Columns().AdjustToContents();
        }

        public void GenerarHojaVueltosDia(IXLWorksheet ws, List<ReporteGlobal_VueltosDia> vueltosDia, DateTime fecha)
        {

            // ======= ENCABEZADO (ARRIBA) =======
            ws.Cell("A1").Value = $"Compañía: {VariablesGlobales.Compania}";
            ws.Cell("A2").Value = $"Rif: {VariablesGlobales.Rif}";
            ws.Cell("A3").Value = $"Sucursal: {VariablesGlobales.CodSucursal} - {VariablesGlobales.Sucursal}";


            ws.Cell("F1").Value = $"Fecha: {DateTime.Now:dd/MM/yyyy}";
            ws.Cell("F2").Value = $"Hora: {DateTime.Now:hh:mm tt}";
            //ws.Cell("F3").Value = "Pag: 1 de 1";

            // Opcional: Negritas y tamaño de letra
            ws.Range("A1:A3").Style.Font.Bold = true;
            ws.Range("F1:F3").Style.Font.Bold = true;

            // ======= TÍTULO DEL REPORTE =======
            ws.Cell("D4").Value = "Cierre de Caja";
            ws.Cell("D4").Style.Font.Italic = true;
            ws.Cell("D4").Style.Font.FontSize = 12;

            ws.Cell("D5").Value = "Vueltos del día";
            ws.Cell("D5").Style.Font.Bold = true;
            ws.Cell("D5").Style.Font.FontSize = 14;

            ws.Cell("D6").Value = $"fecha: {fecha:dd/MM/yyyy}";
            ws.Cell("D6").Style.Font.Italic = true;


            // Encabezados de la tabla

            ws.Cell("B8").Value = "NumOrden";
            ws.Cell("C8").Value = "Referencia";
            ws.Cell("D8").Value = "BancoEmisor";
            ws.Cell("E8").Value = "BancoReceptor";
            ws.Cell("F8").Value = "VueltoBS";
            ws.Cell("G8").Value = "VueltoDivisa";

            ws.Columns("B", "G").Width = 20;

            var headerRange = ws.Range("B8:G8");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#027a65");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // ======= LLENAR DATOS =======  
            int fila = 9;
            foreach (var vuelto in vueltosDia)
            {
                if (vuelto.EsTotal)
                {
                    ws.Cell(fila, 2).Value = vuelto.TextoTotal ?? "Totales:";
                    ws.Cell(fila, 3).Value = vuelto.TotalOrden;
                    ws.Cell(fila, 6).Value = vuelto.TotalVueltoBS;
                    ws.Cell(fila, 7).Value = vuelto.TotalVueltoDivisa;

                    ws.Range($"B{fila}:F{fila}").Style.Font.Bold = true;
                    ws.Range($"B{fila}:F{fila}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }
                else
                {
                    ws.Cell(fila, 2).Value = vuelto.NumOrden;
                    ws.Cell(fila, 3).Value = vuelto.Referencia;
                    ws.Cell(fila, 4).Value = vuelto.BancoEmisor;
                    ws.Cell(fila, 5).Value = vuelto.BancoReceptor;
                    ws.Cell(fila, 6).Value = vuelto.VueltoBS;
                    ws.Cell(fila, 7).Value = vuelto.VueltoDivisa;
                }

                fila++;
            }

            ws.Columns().AdjustToContents();
        }

        public void GenerarHojaPagosDia(IXLWorksheet ws, List<ReporteGlobal_PagosDia> pagosDia, DateTime fecha)
        {

            // ======= ENCABEZADO (ARRIBA) =======
            ws.Cell("A1").Value = $"Compañía: {VariablesGlobales.Compania}";
            ws.Cell("A2").Value = $"Rif: {VariablesGlobales.Rif}";
            ws.Cell("A3").Value = $"Sucursal: {VariablesGlobales.CodSucursal} - {VariablesGlobales.Sucursal}";

            ws.Cell("F1").Value = $"Fecha: {DateTime.Now:dd/MM/yyyy}";
            ws.Cell("F2").Value = $"Hora: {DateTime.Now:hh:mm tt}";
            //ws.Cell("F3").Value = "Pag: 1 de 1";

            // Opcional: Negritas y tamaño de letra
            ws.Range("A1:A3").Style.Font.Bold = true;
            ws.Range("F1:F3").Style.Font.Bold = true;

            // ======= TÍTULO DEL REPORTE =======
            ws.Cell("D4").Value = "Cierre de Caja";
            ws.Cell("D4").Style.Font.Italic = true;
            ws.Cell("D4").Style.Font.FontSize = 12;

            ws.Cell("D5").Value = "Pagos del día";
            ws.Cell("D5").Style.Font.Bold = true;
            ws.Cell("D5").Style.Font.FontSize = 14;

            ws.Cell("D6").Value = $"fecha: {fecha:dd/MM/yyyy}";
            ws.Cell("D6").Style.Font.Italic = true;


            // Encabezados de la tabla

            ws.Cell("B8").Value = "OrdenServicio";
            ws.Cell("C8").Value = "TipoVenta";
            ws.Cell("D8").Value = "Fecha";
            ws.Cell("E8").Value = "Cedula";
            ws.Cell("F8").Value = "TipoPago";
            ws.Cell("G8").Value = "Pago";

            ws.Columns("B", "G").Width = 20;

            var headerRange = ws.Range("B8:G8");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#027a65");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // ======= LLENAR DATOS =======
            int fila = 9;
            foreach (var pago in pagosDia)
            {
                if (pago.EsTotal)
                {
                    ws.Cell(fila, 2).Value = pago.TextoTotal ?? "Totales:";
                    ws.Cell(fila, 3).Value = pago.TotalOrden;
                    ws.Cell(fila, 7).Value = pago.TotalPago;

                    ws.Range($"B{fila}:F{fila}").Style.Font.Bold = true;
                    ws.Range($"B{fila}:F{fila}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }
                else
                {
                    ws.Cell(fila, 2).Value = pago.OrdenServicio;
                    ws.Cell(fila, 3).Value = pago.TipoVenta;
                    ws.Cell(fila, 4).Value = pago.Fecha;
                    ws.Cell(fila, 5).Value = pago.Cedula;
                    ws.Cell(fila, 6).Value = pago.TipoPago;
                    ws.Cell(fila, 7).Value = pago.Pago;
                }

                fila++;
            }

            ws.Columns().AdjustToContents();
        }

        public void GenerarHojaNotasDia(IXLWorksheet ws, List<ReporteGlobal_NotasDia> notasDia, DateTime fecha)
        {

            // ======= ENCABEZADO (ARRIBA) =======
            ws.Cell("A1").Value = $"Compañía: {VariablesGlobales.Compania}";
            ws.Cell("A2").Value = $"Rif: {VariablesGlobales.Rif}";
            ws.Cell("A3").Value = $"Sucursal: {VariablesGlobales.CodSucursal} - {VariablesGlobales.Sucursal}";

            // ======= ENCABEZADO (DERECHA) =======
            ws.Cell("F1").Value = $"Fecha: {DateTime.Now:dd/MM/yyyy}";
            ws.Cell("F2").Value = $"Hora: {DateTime.Now:hh:mm tt}";

            // Opcional: Negritas y tamaño de letra
            ws.Range("A1:A3").Style.Font.Bold = true;
            ws.Range("F1:F3").Style.Font.Bold = true;

            // ======= TÍTULO DEL REPORTE =======
            ws.Cell("D4").Value = "Cierre de Caja";
            ws.Cell("D4").Style.Font.Italic = true;
            ws.Cell("D4").Style.Font.FontSize = 12;

            ws.Cell("D5").Value = "Notas del día";
            ws.Cell("D5").Style.Font.Bold = true;
            ws.Cell("D5").Style.Font.FontSize = 14;

            ws.Cell("D6").Value = $"fecha: {fecha:dd/MM/yyyy}";
            ws.Cell("D6").Style.Font.Italic = true;

            // ======= ENCABEZADOS DE LA TABLA =======
            ws.Cell("B8").Value = "Número";
            ws.Cell("C8").Value = "Número Control";
            ws.Cell("D8").Value = "Cédula";
            ws.Cell("E8").Value = "Factura";
            ws.Cell("F8").Value = "Monto";
            ws.Cell("G8").Value = "Aplicado";
            ws.Cell("H8").Value = "Saldo";

            ws.Columns("B", "H").Width = 20;

            var headerRange = ws.Range("B8:H8");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#027a65");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // ======= LLENAR DATOS =======
            int fila = 9;
            foreach (var nota in notasDia)
            {
                if (nota.EsTotal)
                {
                    ws.Cell(fila, 1).Value = nota.TextoTotal ?? "Total Notas:";
                    ws.Cell(fila, 2).Value = nota.TotalOrden;
                    ws.Cell(fila, 6).Value = nota.TotalMonto;
                    ws.Cell(fila, 7).Value = nota.TotalAplicado;
                    ws.Cell(fila, 8).Value = nota.TotalSaldo;

                    ws.Range($"A{fila}:G{fila}").Style.Font.Bold = true;
                    ws.Range($"A{fila}:G{fila}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }
                else
                {
                    ws.Cell(fila, 2).Value = nota.Numero;
                    ws.Cell(fila, 3).Value = nota.NumeroControl;
                    ws.Cell(fila, 4).Value = nota.Cedula;
                    ws.Cell(fila, 5).Value = nota.Factura;
                    ws.Cell(fila, 6).Value = nota.Monto;
                    ws.Cell(fila, 7).Value = nota.Aplicado;
                    ws.Cell(fila, 8).Value = nota.Saldo;
                }

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

