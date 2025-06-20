using CapaDatos.CargarOrdenes_Datos;
using CapaDatos.Inicio_Datos;
using CapaEntidades;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CapaLogica.CargarOrdenes
{
    public class ServicioValidaciones_CargarOrdenes
    {
        private readonly L_Articulo _L_Articulo;
        private readonly ServicioGuardarOrdenes_Cargar_Ordenes _servicioOrdenGuardado;

        public ServicioValidaciones_CargarOrdenes()
        {
            _L_Articulo = new L_Articulo();
            _servicioOrdenGuardado = new ServicioGuardarOrdenes_Cargar_Ordenes(this);

        }

        public async Task<List<ServicioColoracion_CargarOrdenes>> AplicarServicioColoracion_btnProcesar(List<string> codigosFactura, bool esDegradado, SqlCommand command = null)
        {
            if (codigosFactura == null || codigosFactura.Count == 0)
                return new List<ServicioColoracion_CargarOrdenes>();

            string cristalColor = codigosFactura.Find(c => c.StartsWith("C"));
            bool contieneColoracion = codigosFactura.Contains("S000004");

            if (string.IsNullOrWhiteSpace(cristalColor) || !contieneColoracion)
            {
                return new List<ServicioColoracion_CargarOrdenes>(); // no aplica
            }

            try
            {
                var resultado = await Task.Run(() =>
                {
                    return _L_Articulo.EjecutarServicioColoracion_btnProcesar(cristalColor, !esDegradado, command); // List<ServicioColoracion_CargarOrdenes>
                });

                return resultado;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al ejecutar la lógica de servicio de coloración", ex);
            }
        }

        //-----



        public async Task<DataSet> ObtenerServiciosARDataset(string codCristal, bool codServicio, SqlCommand command = null)
        {
            return await Task.Run(() =>
            {
                return _L_Articulo.ObtenerServiciosARDataset(codCristal, codServicio, command);
            });
        }


        public bool VerificoIgualAntirefCrist(DataGridView grid, DataSet dsServAR)
        {
            try
            {
                int cantCristales = 0;
                bool cambiosRealizados = false;

                // 0. Verificar si hay algún código que sea coloración o AR
                bool hayARoColoracion = false;

                foreach (DataGridViewRow row in grid.Rows)
                {
                    if (row.IsNewRow || row.Cells["CodArticulo"].Value == null)
                        continue;

                    string codigo = row.Cells["CodArticulo"].Value.ToString();

                    if (codigo == "S000004")
                    {
                        hayARoColoracion = true;
                        break;
                    }

                    foreach (DataRow filaAR in dsServAR.Tables[1].Rows)
                    {
                        if (codigo == filaAR["CodServicio"].ToString())
                        {
                            hayARoColoracion = true;
                            break;
                        }
                    }

                    if (hayARoColoracion)
                        break;
                }

                if (!hayARoColoracion)
                    return false; // No hay nada que validar

                // 1. Buscar cantidad de cristales (CodArticulo empieza con "C")
                foreach (DataGridViewRow row in grid.Rows)
                {
                    if (!row.IsNewRow && row.Cells["CodArticulo"].Value != null)
                    {
                        string codigo = row.Cells["CodArticulo"].Value.ToString();
                        if (codigo.StartsWith("C"))
                        {
                            cantCristales = Convert.ToInt32(row.Cells["ART_EXIST"].Value);
                            break;
                        }
                    }
                }

                // 2. Recorrer el grid para igualar cantidades
                foreach (DataGridViewRow row in grid.Rows)
                {
                    if (row.IsNewRow || row.Cells["CodArticulo"].Value == null)
                        continue;

                    string codigo = row.Cells["CodArticulo"].Value.ToString();

                    //Si es coloración
                    if (codigo == "S000004")
                    {
                        object valorF1 = row.Cells["TienePromo"].Value;
                        decimal valorF2 = Convert.ToDecimal(row.Cells["ART_PVP"].Value);
                        object valorF3 = row.Cells["PromoEvaluada"].Value;

                        int cantidad = Convert.ToInt32(row.Cells["ART_EXIST"].Value);

                        if (cantidad != cantCristales)
                        {
                            row.Cells["TienePromo"].Value = valorF2;
                            row.Cells["ART_PVP"].Value = valorF2.ToString("N2");
                            row.Cells["ART_EXIST"].Value = cantCristales;
                            row.Cells["Total"].Value = (valorF2 * cantCristales).ToString("N2");
                            row.Cells["PromoEvaluada"].Value = valorF3;

                            cambiosRealizados = true;
                        }
                    }

                    //Si es un AR (validar con tabla 1 del dataset)
                    foreach (DataRow filaAR in dsServAR.Tables[1].Rows)
                    {
                        string codAR = filaAR["CodServicio"].ToString();
                        if (codigo == codAR)
                        {
                            object valorF1 = row.Cells["TienePromo"].Value;
                            decimal valorF2 = Convert.ToDecimal(row.Cells["ART_PVP"].Value);
                            object valorF3 = row.Cells["PromoEvaluada"].Value;

                            int cantidad = Convert.ToInt32(row.Cells["ART_EXIST"].Value);

                            if (cantidad != cantCristales)
                            {
                                row.Cells["TienePromo"].Value = valorF2;
                                row.Cells["ART_PVP"].Value = valorF2.ToString("N2");
                                row.Cells["ART_EXIST"].Value = cantCristales;
                                row.Cells["Total"].Value = (valorF2 * cantCristales).ToString("N2");
                                row.Cells["PromoEvaluada"].Value = valorF3;

                                cambiosRealizados = true;
                                //break;
                            }
                            break;
                        }
                    }
                }

                return cambiosRealizados;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en VerificoIgualAntirefCrist: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> ValidarOrdenServicioAsync(DataGridView dgvTotales, DataGridView dgvDetalle, SqlCommand command,
        Func<string, string, DialogResult> mostrarPregunta, Action<string> mostrarError, ValidacionEstucheDTO datosEstuche)
        {
            try
            {
                // 1. Validar monto total
                foreach (DataGridViewRow row in dgvTotales.Rows)
                {
                    if (row.Cells["Concepto"].Value?.ToString().Trim() == "Total")
                    {
                        string valorStr = row.Cells["Valor"].Value?.ToString();
                        if (string.IsNullOrWhiteSpace(valorStr) || Convert.ToDecimal(valorStr) == 0)
                        {
                            mostrarError("El monto total no puede ser cero");
                            return false;
                        }
                        break;
                    }
                }

                //// 2. Validar observación
                //if (!ValidarObservacionYEstuche(datosEstuche, mostrarPregunta, mostrarError))
                //{
                //    return false;
                //}


                //// 3. Validar montos 
                //decimal montoTotalEncabezado = ObtenerMontoTotalDesdeGrid(dgvTotales);

                //bool montosValidos = ValidoMontos(dgvDetalle, montoTotalEncabezado, mostrarPregunta, mostrarError);

                //if (!montosValidos)
                //{
                //    return false;
                //}

                //// 2. Validar observación y estuche anidado: validar montovalido,
                decimal montoTotalEncabezado = ObtenerMontoTotalDesdeGrid(dgvTotales);

                if (!ValidarObservacionYEstucheConMontos(datosEstuche, dgvDetalle, montoTotalEncabezado, mostrarPregunta, mostrarError))
                {
                    return false;
                }


                // 4. (Opcional) Validar productos y existencia (si aplica)
                // ...

                // 5. Insertar orden
                //return await InsertarOrdenCabeceraYDetalleAsync(dgvDetalle, command);

                return true;
            }
            catch (Exception ex)
            {
                mostrarError("Error al guardar la orden de servicio: " + ex.Message);
                return false;
            }
        }

        private decimal ObtenerMontoTotalDesdeGrid(DataGridView grid)
        {
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.Cells["Concepto"].Value?.ToString().Trim() == "Total")
                {
                    string valorStr = row.Cells["Valor"].Value?.ToString();
                    if (decimal.TryParse(valorStr, out decimal valor))
                        return valor;
                }
            }

            return 0;
        }

        private bool ValidarObservacionYEstucheConMontos(ValidacionEstucheDTO datosEstuche, DataGridView dgvDetalle, decimal montoTotalEncabezado, Func<string, string, DialogResult> mostrarPregunta, Action<string> mostrarError)
        {
            if (!ValidarObservacionYEstuche(datosEstuche, mostrarPregunta, mostrarError))
            {
                return false;
            }

            if (!ValidoMontos(dgvDetalle, montoTotalEncabezado, mostrarPregunta, mostrarError))
            {
                return false;
            }

            return true;
        }

        //1
        public bool ValidarObservacionYEstuche(ValidacionEstucheDTO datos, Func<string, string, DialogResult> mostrarPregunta, Action<string> mostrarError)
        {
            switch (datos.TipoTrabajo)
            {
                case "001":
                    return true;

                case "002":
                    if (string.IsNullOrEmpty(datos.Observacion))
                    {
                        var resultado = mostrarPregunta(
                            "Esta orden no posee observación ¿Desea continuar?",
                            "Verifique por favor"
                        );

                        if (resultado != DialogResult.OK)
                            return false;
                    }

                    if (!datos.MonturaPropia)
                    {
                        bool tieneEstuche = datos.CodigosDesdeGrid.Any(c => c.StartsWith("E"));

                        if (!tieneEstuche)
                        {
                            var resultado = mostrarPregunta(
                                "Esta orden no posee estuche ¿Desea continuar?",
                                "Verifique por favor"
                            );

                            if (resultado != DialogResult.OK)
                                return false;
                        }
                    }

                    //if (datos.Garantia && string.IsNullOrEmpty(datos.Observacion))
                    //{
                    //    mostrarError("Esta Orden no posee OBSERVACION. Recuerde incluir el número de Aprobación de Asegurado");
                    //    return false;
                    //}

                    return true;

                case "003":
                    if (string.IsNullOrEmpty(datos.Observacion))
                    {
                        mostrarError("El campo de Observación no puede estar en blanco");
                        return false;
                    }
                    return true;

                default:
                    return true;
            }
        }

        //2
        public bool ValidoMontos(DataGridView grid, decimal montoTotalEncabezado, Func<string, string, DialogResult> mostrarPregunta, Action<string> mostrarError)
        {
            try
            {
                decimal total = 0;

                for (int fila = 0; fila < grid.RowCount; fila++)
                {
                    var row = grid.Rows[fila];

                    if (row.Cells["ART_EXIST"].Value == DBNull.Value || row.Cells["ART_PVP"].Value == DBNull.Value || row.Cells["Impuesto"].Value == DBNull.Value)
                    {
                        var resultado = mostrarPregunta($"Los valores de la fila {fila} están en blanco, no se puede guardar esta orden", "Por favor verifique sus datos");

                        if (resultado == DialogResult.No)
                            return false;
                    }

                    decimal cantidad = Convert.ToDecimal(row.Cells["ART_EXIST"].Value);
                    decimal precio = Convert.ToDecimal(row.Cells["ART_PVP"].Value);
                    decimal descuento = Convert.ToDecimal(row.Cells["PORCTDESCUENTO"].Value ?? 0);
                    decimal impuesto = Convert.ToDecimal(row.Cells["Impuesto"].Value);

                    decimal totalFila = cantidad * precio;
                    totalFila = totalFila - ((totalFila * descuento) / 100);
                    totalFila = Math.Round(totalFila, 2);
                    totalFila = totalFila + ((totalFila * impuesto) / 100);
                    totalFila = Math.Round(totalFila, 2);

                    total += totalFila;
                }

                if (total == montoTotalEncabezado)
                {
                    return true;
                }
                else if (Math.Abs(total - montoTotalEncabezado) < 1)
                {
                    return true;
                }
                else
                {
                    mostrarError("No se puede guardar esta orden, hay disparidad en los montos");
                    return false;
                }


            }
            catch (Exception ex)
            {
                mostrarError($"Error en validación de montos: {ex.Message}");
                return false;
            }
        }

        //3
        //public async Task<bool> VerificoProductosRepetidos(string codArticulo, int filaActual, DataGridView grid, Func<string, string, DialogResult> mostrarPregunta, Action<string> mostrarError)
        //{
        //    // Esta funcion evalua todos los artículos que esten el la pantalla en este momento y verifica en base
        //    // a ellos que el que se esta recien agregando no este repetido.
        //    // La idea de este método es que sea utilizado cuando se agreguen productos "a mano", es decir que no sea
        //    // productos agregados automáticamente por una promoción, para esos casos es el método VerificoProductoRepetidoPromo.

        //    try
        //    {
        //        // Verifica si el grid tiene filas.
        //        if (grid.RowCount > 0)
        //        {
        //            // Caso especial cuando se manejan existencias: glbManejaExisLC = "1" y GlbCodDetVta = "02"
        //            if (glbManejaExisLC == "1" && GlbCodDetVta == "02") //GlbCodDetVta:Cbx_Pnl2_Trbajo  combobox tomar valor oculto // glbManejaExisLC: agregar variable global en formulario mientras "1"
        //            {
        //                for (int x = 0; x < grid.RowCount; x++)
        //                {
        //                    var currentRow = grid.Rows[x];
        //                    // Compara el valor de la celda "Código" con el código en mayúsculas.
        //                    if (currentRow.Cells["CodArticulo"].Value != null &&
        //                        currentRow.Cells["CodArticulo"].Value.ToString().Equals(codArticulo.ToUpper()) &&
        //                        filaActual != x)
        //                    {
        //                        // Para poder comparar con la fila anterior, se verifica que x sea mayor a 0.
        //                        if (x > 0)
        //                        {
        //                            var prevRow = grid.Rows[x - 1];
        //                            // Compara el valor de "CodigoLab" de la fila actual con el de la fila anterior (convertido a mayúsculas).
        //                            if (currentRow.Cells["CodigoLab"].Value != null && //no esta la columna todavia, igual hacer que esta
        //                                prevRow.Cells["CodigoLab"].Value != null &&
        //                                currentRow.Cells["CodigoLab"].Value.ToString().Equals(prevRow.Cells["CodigoLab"].Value.ToString().ToUpper()) &&
        //                                filaActual != x)
        //                            {
        //                                // El artículo ya fue agregado.
        //                                // Si el campo "Generico" es verdadero, se realiza una validación especial.
        //                                bool esGenerico = false;
        //                                if (currentRow.Cells["Generico"].Value != null) //no esta la columna todavia, igual hacer que esta
        //                                    bool.TryParse(currentRow.Cells["Generico"].Value.ToString(), out esGenerico);

        //                                if (esGenerico)
        //                                {
        //                                    var resultado = mostrarPregunta($"El artículo {codArticulo} ({prevRow.Cells["CodigoLab"].Value?.ToString().ToUpper()}) ya fué agregado antes y no se puede incluir de nuevo.", 
        //                                        "Este es un código GENERICO, aplica para ambos ojos aun si el RX es diferente. Coloque la cantidad que desee y verifique que la columna OJO contenga 'Ambos'.");

        //                                    if (resultado == DialogResult.No)
        //                                        return false;

        //                                    // Llamadas a métodos de limpieza y actualización; estos métodos deben estar implementados en tu servicio o clase// agregar método después

        //                                    //BorrarFila(); // agregar método después
        //                                    //BarridoDeFilas(sqlCom);

        //                                    //dtFacturas.Clear();
        //                                    //grid.DataSource = dtFacturas;
        //                                    //RetrieveStructure(gexFacturas);
        //                                    //FormatoTabla(gexFacturas);

        //                                    if (!string.IsNullOrEmpty(glbServicio))
        //                                    {
        //                                        CargoServicioExpress(sqlCom);
        //                                    }

        //                                    // Se retorna false al haber encontrado el artículo repetido.
        //                                    return false;
        //                                }
        //                                else
        //                                {
        //                                    var resultado = mostrarPregunta($"El artículo {codArticulo} ({prevRow.Cells["CodigoLab"].Value?.ToString().ToUpper()}) ya fué agregado antes y no se puede incluir de nuevo.",
        //                                        "Modifique la cantidad del anterior si lo desea.");

        //                                    if (resultado == DialogResult.No)
        //                                        return false;
        //                                }

        //                                // Limpia los valores de la fila actual.
        //                                for (int w = 0; w < currentRow.Cells.Count; w++)
        //                                {
        //                                    currentRow.Cells[w].Value = "";
        //                                }
        //                                return false;
        //                            }
        //                            else
        //                            {
        //                                // Si no se cumple la condición anterior, se asume que no hay conflicto en esta iteración.
        //                                // Continúa la validación.
        //                            }
        //                        }
        //                    }
        //                    // Si no se da el if anterior, se continúa el loop.
        //                }
        //                // Si el loop finaliza sin encontrar conflicto, se retorna true.
        //                return true;
        //            }
        //            else
        //            {
        //                // Caso alterno sin validación especial
        //                for (int x = 0; x < grid.RowCount; x++)
        //                {
        //                    var currentRow = grid.Rows[x];
        //                    if (currentRow.Cells["CodArticulo"].Value != null &&
        //                        currentRow.Cells["CodArticulo"].Value.ToString().Equals(codArticulo.ToUpper()) &&
        //                        filaActual != x)
        //                    {
        //                        var resultado = mostrarPregunta($"El artículo {codArticulo} ya fué agregado antes y no se puede incluir de nuevo.",
        //                                        "Modifique la cantidad del anterior si lo desea.");

        //                        if (resultado == DialogResult.No)
        //                            return false;
        //                    }
        //                    else
        //                    {

        //                        // Verifica que solo se acepte una montura cuando sea un trabajo.
        //                        if (glbTipoTrabajo == "002") //esto sacar del combobox //realizar función que busque en TB_VentasDetalle el código de ventas
        //                        {
        //                            // Verifica si el código empieza con "M" (montura) en ambas condiciones.
        //                            if (currentRow.Cells["CodArticulo"].Text.StartsWith("M") && codArticulo.ToUpper().StartsWith("M") && x != filaActual)
        //                            {
        //                                var resultado = mostrarPregunta("Este artículo no se puede agregar a esta Orden de Trabajo porque ya tiene una montura agregada",
        //                                  "Artículo repetido");

        //                                if (resultado == DialogResult.No)
        //                                    return false;


        //                                // Borra el contenido de la fila.
        //                                for (int w = 0; w < currentRow.Cells.Count; w++)
        //                                {
        //                                    currentRow.Cells[w].Value = "";
        //                                }
        //                                return false;
        //                            }
        //                        }
        //                    }
        //                }
        //                return true;
        //            }
        //        }
        //        else
        //        {
        //            // Si no hay filas en el grid, consideramos que no hay repetición.
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        mostrarError($"Error en la función 'frmFacturas.VerificoProductosRepetidos'. " +
        //            $"Por favor comunicarse con el Dpto de Sistemas y reportar el siguiente error: {ex.Message}");

        //        return false;
        //    }

        //}

        public decimal ObtenerValorDesdeGrid_Totales(DataGridView dgv, string concepto)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                var celdaConcepto = row.Cells["Concepto"].Value?.ToString()?.Trim();
                if (celdaConcepto != null && celdaConcepto.Equals(concepto, StringComparison.OrdinalIgnoreCase))
                {
                    string valorStr = row.Cells["Valor"].Value?.ToString();
                    if (decimal.TryParse(valorStr, out decimal valor))
                    {
                        return valor;
                    }
                }
            }

            return 0m; // si no se encuentra o no es válido
        }

        public decimal ObtenerTotalImpuestoDesdeGrid(DataGridView dgv)
        {
            decimal totalImpuesto = 0m;
            decimal ivaTotal = 0;
            decimal subtotalConIva = 0;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells["Impuesto"].Value != null)
                {
                    decimal impuesto = Convert.ToDecimal(row.Cells["Impuesto"].Value);
                    subtotalConIva = Convert.ToDecimal(row.Cells["ART_PVP"].Value) * Convert.ToDecimal(row.Cells["ART_EXIST"].Value);
                    ivaTotal += subtotalConIva * (impuesto / 100);
                }
            }

            return totalImpuesto;
        }

        //public decimal ObtenerValorDesdeGrid_Articulos(DataGridView dgv, string concepto)
        //{
        //    foreach (DataGridViewRow row in dgv.Rows)
        //    {
        //        var celdaConcepto = row.Cells["Impuesto"].Value?.ToString()?.Trim();
        //        if (celdaConcepto != null && celdaConcepto.Equals(concepto, StringComparison.OrdinalIgnoreCase))
        //        {
        //            string valorStr = row.Cells["ART_PVP"].Value?.ToString();
        //            if (decimal.TryParse(valorStr, out decimal valor))
        //            {
        //                return valor;
        //            }
        //        }
        //    }

        //    return 0m; // si no se encuentra o no es válido
        //}

        public string ValidarMonturaQuorumYCristales(DataGridView dgvArticulos, string laboratorio, string sucursal, string servicio, string cedNacio, string cedId, string examen, Action<string> mostrarError , SqlCommand command)
        {
            if (!laboratorio.Equals("QUO", StringComparison.OrdinalIgnoreCase))
                return "OK";

            foreach (DataGridViewRow row in dgvArticulos.Rows)
            {
                string codArticulo = row.Cells["CodArticulo"]?.Value?.ToString();
                if (string.IsNullOrWhiteSpace(codArticulo))
                    continue;

                // Validar montura QUORUM (si empieza por M)
                if (codArticulo.StartsWith("M", StringComparison.OrdinalIgnoreCase))
                {
                    string monturaQuorum = _L_Articulo.ObtenerMonturaQuorumPorArticulo(codArticulo, sucursal, servicio);

                    if (!string.IsNullOrWhiteSpace(monturaQuorum))
                    {
                        mostrarError($"La montura {codArticulo.ToLower()} no requiere envió a QUORUM");
                        //return codArticulo.ToLower();
                        //MessageBox.Show($"La Montura {codArticulo} no requiere el envío al Laboratorio QUORUM.", "No Enviar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                // Validar cristal con baja existencia (si empieza por C)
                if (codArticulo.StartsWith("C", StringComparison.OrdinalIgnoreCase))
                {
                    string cristalConBaja = _L_Articulo.ObtenerBajaExistenciaCristales(cedNacio, cedId, sucursal, examen, codArticulo, command);

                    if (!string.IsNullOrWhiteSpace(cristalConBaja))
                    {
                        mostrarError($"El cristal {codArticulo.ToLower()} tiene baja existencia en Laboratorio");
                        //return codArticulo.ToLower();
                        //MessageBox.Show($"El Cristal {cristalConBaja} tiene baja existencia en Laboratorio.", "No Enviar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            return "OK";
        }

        public bool MonturaEstaEnQuorum(string codArticulo, string sucursal, string codServicio)
        {
            return _L_Articulo.ObtenerMonturaQuorumPorArticulo(codArticulo, sucursal, codServicio) != null;
        }

     









    }

    



}



