using CapaDatos.DetalleOrden_Datos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaDatos.CargarOrdenes_Datos;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using System.Data;

namespace CapaLogica.CargarOrdenes
{
    public class ServicioGuardarOrdenes_Cargar_Ordenes
    {
        private readonly L_Articulo _L_Articulo;
        private readonly D_DetalleOrden _D_DetalleOrden;
        private readonly ServicioValidaciones_CargarOrdenes _servicioValidaciones;
        private readonly D_Trabajo  _D_Trabajo;

        public ServicioGuardarOrdenes_Cargar_Ordenes(ServicioValidaciones_CargarOrdenes servicioValidaciones)
        {
            _L_Articulo = new L_Articulo();
            _D_DetalleOrden = new D_DetalleOrden();
            _servicioValidaciones = servicioValidaciones;
            _D_Trabajo = new D_Trabajo();
        }

        //public async Task<string> GuardarOrdenServicioAsync(AgregarOrdenServicio_CargarOrdenes datos, SqlCommand command)
        //{
        //    try
        //    {
        //        //string numeroOrden = await _L_Articulo.AgregarOrdenServicio(
        //        //    datos.Cod_Sucursal, datos.Revision, datos.Cod_Venta, datos.CTE_Nacio, datos.CTE_CedIden, datos.NumExamen,
        //        //    datos.COD_EMPLEADO, datos.Cod_Laboratorio, datos.Cod_Servicio, datos.Vision, datos.Fec_ofrecido, datos.Hor_ofrecido,
        //        //    datos.Fec_Entrega, datos.Fec_Envio, datos.VtaSubTotal, datos.VtaImpuesto, datos.VtaDescuento, datos.VtaTotal,
        //        //    datos.OrSer_Finan, datos.OrSer_Status, datos.OrSer_Observ, datos.User_Crea, datos.Fecha, datos.MonturaPropia,
        //        //    datos.Cod_DetVta, datos.Aplica, datos.OTCORRESPONDIENTE, datos.VentaAfil, datos.CristalPropio,
        //        //    datos.TipoMonturaPropia, datos.CodMotivoReposicion, datos.Cedula_CteAfil, datos.Codigo_EmpAfil,
        //        //    datos.Asegurada, datos.Exonerada, datos.MonturaEnQuorum, datos.Cod_Coloracion,
        //        //    command // pasa el mismo SqlCommand para usar la misma transacción

        //            string numeroOrden = await _L_Articulo.AgregarOrdenServicio(datos, command); // pasa el mismo SqlCommand para usar la misma transacción

        //        if (string.IsNullOrWhiteSpace(numeroOrden))
        //        {
        //            throw new Exception("No se generó número de orden. El procedimiento puede haber fallado.");
        //        }

        //        return numeroOrden;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error guardando orden de servicio: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return null;
        //    }
        //}

        public async Task<string> GuardarOrdenServicioDesdeFormularioAsync(
    DataGridView dgvArticulos,
    DataGridView dgvTotales,
    string codSucursal,
    string letraInicial,
    string numeroCedula,
    string codEmpleado,
    string codLaboratorio,
    string codServicio,
    string vision,
    string observacion,
    string codTrabajo,
    string codd_Venta,
    string usuarioActual,
    bool monturaPropia,
    bool Aplicaa,
    bool cristalPropio,
    bool monturaEnQuorum,
    bool asegurada,
    string codColoracion,
    string cedulaAfiliado,
    string codigoEmpresaAfiliada,
    string numExamen,
    string Codmotivodess,
    DateTime Fec_ofrecido,
    string Hor_ofrecido,
    bool ventaAfil,
    DateTime FechaActiva,
    string CodPromocion,
    SqlCommand command)
        {
            try
            {
                // Armo el objeto automáticamente
                var datosOrden = new AgregarOrdenServicio_CargarOrdenes
                {
                    Cod_Sucursal = codSucursal,
                    Revision = "0",
                    Cod_Venta = codTrabajo,
                    CTE_Nacio = letraInicial,
                    CTE_CedIden = numeroCedula,
                    NumExamen = codd_Venta != "04" ? numExamen : "0", //dgvArticulos.Rows[0].Cells["NumExamen"].Value?.ToString() ?? "0", // puedes adaptarlo
                    COD_EMPLEADO = codEmpleado,
                    Cod_Laboratorio = codLaboratorio,
                    Cod_Servicio = codServicio,
                    Vision = vision,
                    Fec_ofrecido = Fec_ofrecido,
                    Hor_ofrecido = Hor_ofrecido,
                    Fec_Entrega = null,
                    Fec_Envio = null,
                    VtaSubTotal = _servicioValidaciones.ObtenerValorDesdeGrid_Totales(dgvTotales, "SubTotal"),
                    VtaImpuesto = _servicioValidaciones.ObtenerValorDesdeGrid_Totales(dgvTotales, "IVA"),
                    VtaDescuento = _servicioValidaciones.ObtenerValorDesdeGrid_Totales(dgvTotales, "Descuento"),
                    VtaTotal = _servicioValidaciones.ObtenerValorDesdeGrid_Totales(dgvTotales, "Total"),
                    OrSer_Finan = false,
                    OrSer_Status = _D_DetalleOrden.CambiarSatusOrdenCasada(CodPromocion, command),
                    OrSer_Observ = observacion,
                    User_Crea = usuarioActual,
                    Fecha = FechaActiva,
                    MonturaPropia = monturaPropia,
                    Cod_DetVta = codd_Venta,
                    Aplica = Aplicaa,
                    OTCORRESPONDIENTE = "",
                    VentaAfil = ventaAfil,
                    CristalPropio = cristalPropio,
                    TipoMonturaPropia = null,
                    CodMotivoReposicion = null,
                    Cedula_CteAfil = cedulaAfiliado,
                    Codigo_EmpAfil = codigoEmpresaAfiliada,
                    Asegurada = asegurada,
                    Exonerada = false,
                    MonturaEnQuorum = monturaEnQuorum,
                    Cod_Coloracion = !string.IsNullOrEmpty(codColoracion) ? codColoracion : null,
                    Codmotivodes = !string.IsNullOrEmpty(Codmotivodess) ? Codmotivodess : null, // Asignar solo si no está vacío
                    OrSer_Saldo_Mon= _servicioValidaciones.ObtenerValorDesdeGrid_Totales(dgvTotales, "Ref")
                };

                //string numeroOrden = await GuardarOrdenServicioAsync(datosOrden, command);
                string numeroOrden = await _L_Articulo.AgregarOrdenServicio(datosOrden, command);
                return numeroOrden;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error general al guardar la orden de servicio: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public async Task<bool> GuardarDetalleOrdenServicioAsync(DataGridView dgvArticulos, string numeroOrden, string numeroRevision,
                                                                string codigoVenta, string sucursal, SqlCommand command, ComboBox Cbx_Pnl2_Trbajo,
                                                                ComboBox Cbx_Pnl2_Laboratorio, bool EmpresaAfiliada)
        {
            try
            {
                bool DetalleOrdenServicio = false;
                bool resultado = true;
                //string ojo = _L_Articulo.CalcularOjoDesdeGrid(dgvArticulos);
                bool productosRepetidos = false;
                string codigoProductoRepetido = null;
                int cantidadTotalRepetida = 0;
                var CodLab = Cbx_Pnl2_Laboratorio.SelectedValue.ToString();
                var glbCodDetVta = Cbx_Pnl2_Trbajo.SelectedValue.ToString();
                var glbManejaExisLC = _D_DetalleOrden.TB_PARAMETRO("LCManejaExist");
                bool empresaAfiliada = EmpresaAfiliada;
                bool prodRepetidoGuardado = false;

                // Detectar productos repetidos (si maneja existencias LC y venta de tipo 02)
                if (glbManejaExisLC == "1" && glbCodDetVta == "02")
                {
                    (productosRepetidos, codigoProductoRepetido) = _L_Articulo.DetectarProductoRepetido(dgvArticulos);
                    if (productosRepetidos)
                        cantidadTotalRepetida = _L_Articulo.CalcularCantidadTotalRepetida(dgvArticulos, codigoProductoRepetido);
                }

                // Guardar cada fila del detalle
                for (int fila = 0; fila < dgvArticulos.Rows.Count; fila++)
                {
                    var row = dgvArticulos.Rows[fila];
                    if (row.IsNewRow) continue;

                    string codArticulo = row.Cells["CodArticulo"].Value?.ToString();
                    int cantidad = Convert.ToInt32(row.Cells["ART_EXIST"].Value);
                    decimal precio = Convert.ToDecimal(row.Cells["ART_PVP"].Value);
                    decimal impuesto = row.Cells["Impuesto"].Value!= DBNull.Value ? Convert.ToDecimal(row.Cells["Impuesto"].Value) : 0;
                    decimal precioViejo = row.Cells["PrecioViejo"].Value != DBNull.Value ? Convert.ToDecimal(row.Cells["PrecioViejo"].Value) : 0;
                    string codPromo = row.Cells["CodPromo"].Value?.ToString();
                    decimal costoPromedio = row.Cells["costoProme"].Value != DBNull.Value ? Convert.ToDecimal(row.Cells["costoProme"].Value) : 0;
                    string codigoLab = CodLab;
                    decimal porcentajeDescuento = row.Cells["PORCTDESCUENTO"].Value != DBNull.Value ? Convert.ToDecimal(row.Cells["PORCTDESCUENTO"].Value) : 0;
                    string ojo = row.Cells["ojo"].Value.ToString();
                    int cantidadFinal = cantidad;
                    if (productosRepetidos && codArticulo == codigoProductoRepetido)
                        cantidadFinal = cantidadTotalRepetida;

                   

                    if (glbManejaExisLC == "1" && glbCodDetVta == "02")
                    {
                        if (codigoProductoRepetido == codArticulo)
                        {
                            if (prodRepetidoGuardado == false)
                            {
                                DetalleOrdenServicio = await _L_Articulo.GuardarDescripcionDetalleOrdenServicioAsync(numeroOrden, numeroRevision, codigoVenta,
                                codArticulo,  cantidadFinal,  "A", precio, impuesto, porcentajeDescuento,precioViejo,codPromo, costoPromedio, sucursal, command);
                                if (DetalleOrdenServicio)
                                {
                                    prodRepetidoGuardado = true;
                                }
                            }
                        }
                        else
                        {
                            DetalleOrdenServicio = await _L_Articulo.GuardarDescripcionDetalleOrdenServicioAsync(numeroOrden, numeroRevision, codigoVenta,
                           codArticulo, cantidadFinal, ojo, precio, impuesto, porcentajeDescuento, precioViejo, codPromo, costoPromedio, sucursal, command);
                        }
                    }
                    else
                    {
                        DetalleOrdenServicio = await _L_Articulo.GuardarDescripcionDetalleOrdenServicioAsync(numeroOrden, numeroRevision, codigoVenta,
                       codArticulo, cantidadFinal, ojo, precio, impuesto, porcentajeDescuento, precioViejo, codPromo, costoPromedio, sucursal, command);
                    }

                    if (!DetalleOrdenServicio)
                    {
                        resultado = false;
                        MessageBox.Show($"Error guardando el detalle de la Orden de Servicio en la fila {fila + 1}. No se pudo continuar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                }

                return resultado;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error general al guardar detalle de orden de servicio: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

    

        //Actualizo la tabla tbTrabajo
        public async Task<bool> ActualizarTrabajoYExistencias(string NUMOS, string HORIZ, string VERT, string MAX, string PTE, string DISVERT,
            string ANPANT, string ANFAC, string ALTD, string ALTI, string OJO, string TVISD, string TVISI, string USER, string SUC, SqlCommand command)
        {
            try
            {

                // 1. Actualizar datos en TB_TRABAJO
                bool trabajoActualizado = await _L_Articulo.ModificarTrabajo(NUMOS, HORIZ, VERT, MAX, PTE, DISVERT, ANPANT, ANFAC, ALTD, ALTI, OJO, TVISD, TVISI, USER, SUC,  command);

                if (!trabajoActualizado)
                {
                    MessageBox.Show("No se pudo actualizar la tabla TB_TRABAJO.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error general al actualizar TB_TRABAJO o rebajar inventario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool AgregarTrabajo2(TB_TRABAJOCTE nuevoTrabajo, SqlCommand command)
        {
            try
            {
                bool Respuesta = _D_Trabajo.AgregarTrabajo2(nuevoTrabajo, command);

                return Respuesta;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error general al actualizar TB_TRABAJO o rebajar inventario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        // Puedes 

     


        public async Task<DataSet> LlamarActualizarGarantiaAsync(string OS, string Suc, string OsResposable, SqlCommand command)
        {
                D_Articulos articulos = new D_Articulos();
                return await articulos.ActualizarGarantia(OS, Suc, OsResposable, command);
        }

        public async Task<bool> AgregaRelacionOsLC(string sucursal, string nroOs, string revision, DataGridView dgvArticulos,  string usuario, string stock, SqlCommand command)
        {
            D_Articulos articulos = new D_Articulos();


            bool trabajoActualizado;

            for (int fila = 0; fila < dgvArticulos.Rows.Count; fila++)
            {
                var row = dgvArticulos.Rows[fila];
                if (row.IsNewRow) continue;

                string codArticulo = row.Cells["CodArticulo"].Value?.ToString();
                int cantidad = Convert.ToInt32(row.Cells["ART_EXIST"].Value);
                string codigoLab = row.Cells[1].Value?.ToString(); 
                string ojo = row.Cells["ojo"].Value.ToString();
                
                if (codArticulo.StartsWith("W"))
                {
                    trabajoActualizado = await articulos.AgregaRelacionOsLC(sucursal, nroOs, revision, codArticulo, codigoLab, cantidad, ojo, usuario, stock, command);
                   
                    if (!trabajoActualizado)
                    {
                        return false;
                        MessageBox.Show($"Error guardando el detalle la relacion de os LC {fila + 1}. No se pudo continuar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                }

               
            }


            return true; 
        }

        public bool AgregarGiftCard(string codSucursal, string nroOrden, string revision, decimal montoDolares, string nombreBeneficiario, string correoBeneficiario, string mensaje, string codigoGiftCard, string userCrea, string userMod, SqlCommand command = null)
        {
            try
            {
                bool Respuesta = _D_Trabajo.AgregarGiftCard(codSucursal, nroOrden,revision,montoDolares, nombreBeneficiario,correoBeneficiario,mensaje, codigoGiftCard,userCrea, userMod,command);

                return Respuesta;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error general al actualizar TB_TRABAJO o rebajar inventario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }



    }




}



