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

namespace CapaLogica.CargarOrdenes
{
    public class ServicioGuardarOrdenes_Cargar_Ordenes
    {
        private readonly L_Articulo _L_Articulo;
        private readonly D_DetalleOrden _D_DetalleOrden;
        private readonly ServicioValidaciones_CargarOrdenes _servicioValidaciones;

        public ServicioGuardarOrdenes_Cargar_Ordenes(ServicioValidaciones_CargarOrdenes servicioValidaciones)
        {
            _L_Articulo = new L_Articulo();
            _D_DetalleOrden = new D_DetalleOrden();
            _servicioValidaciones = servicioValidaciones;
        }

        public async Task<string> GuardarOrdenServicioAsync(AgregarOrdenServicio_CargarOrdenes datos, SqlCommand command)
        {
            try
            {
                string numeroOrden = await _L_Articulo.AgregarOrdenServicio(
                    datos.Cod_Sucursal, datos.Revision, datos.Cod_Venta, datos.CTE_Nacio, datos.CTE_CedIden, datos.NumExamen,
                    datos.COD_EMPLEADO, datos.Cod_Laboratorio, datos.Cod_Servicio, datos.Vision, datos.Fec_ofrecido, datos.Hor_ofrecido,
                    datos.Fec_Entrega, datos.Fec_Envio, datos.VtaSubTotal, datos.VtaImpuesto, datos.VtaDescuento, datos.VtaTotal,
                    datos.OrSer_Finan, datos.OrSer_Status, datos.OrSer_Observ, datos.User_Crea, datos.Fecha, datos.MonturaPropia,
                    datos.Cod_DetVta, datos.Aplica, datos.OTCORRESPONDIENTE, datos.VentaAfil, datos.CristalPropio,
                    datos.TipoMonturaPropia, datos.CodMotivoReposicion, datos.Cedula_CteAfil, datos.Codigo_EmpAfil,
                    datos.Asegurada, datos.Exonerada, datos.MonturaEnQuorum, datos.Cod_Coloracion,
                    command // pasa el mismo SqlCommand para usar la misma transacción
                );

                if (string.IsNullOrWhiteSpace(numeroOrden))
                {
                    throw new Exception("No se generó número de orden. El procedimiento puede haber fallado.");
                }

                return numeroOrden;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error guardando orden de servicio: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

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
    string usuarioActual,
    bool monturaPropia,
    bool promocionAplicada,
    bool cristalPropio,
    bool monturaEnQuorum,
    bool asegurada,
    string codColoracion,
    string cedulaAfiliado,
    string codigoEmpresaAfiliada,
    string numExamen,
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
                    NumExamen = numExamen, //dgvArticulos.Rows[0].Cells["NumExamen"].Value?.ToString() ?? "0", // puedes adaptarlo
                    COD_EMPLEADO = codEmpleado,
                    Cod_Laboratorio = codLaboratorio,
                    Cod_Servicio = codServicio,
                    Vision = vision,
                    Fec_ofrecido = DateTime.Now,
                    Hor_ofrecido = DateTime.Now.ToString("HH:mm"),
                    Fec_Entrega = DateTime.Now,
                    Fec_Envio = DateTime.Now,
                    VtaSubTotal = _servicioValidaciones.ObtenerValorDesdeGrid_Totales(dgvTotales, "SubTotal"),
                    VtaImpuesto = _servicioValidaciones.ObtenerTotalImpuestoDesdeGrid(dgvArticulos),
                    VtaDescuento = _servicioValidaciones.ObtenerValorDesdeGrid_Totales(dgvTotales, "Descuento"),
                    VtaTotal = _servicioValidaciones.ObtenerValorDesdeGrid_Totales(dgvTotales, "Total"),
                    OrSer_Finan = false,
                    OrSer_Status = "004",
                    OrSer_Observ = observacion,
                    User_Crea = usuarioActual,
                    Fecha = DateTime.Now,
                    MonturaPropia = monturaPropia,
                    Cod_DetVta = codTrabajo,
                    Aplica = promocionAplicada,
                    OTCORRESPONDIENTE = "",
                    VentaAfil = false,
                    CristalPropio = cristalPropio,
                    TipoMonturaPropia = "",
                    CodMotivoReposicion = "",
                    Cedula_CteAfil = cedulaAfiliado,
                    Codigo_EmpAfil = codigoEmpresaAfiliada,
                    Asegurada = asegurada,
                    Exonerada = false,
                    MonturaEnQuorum = monturaEnQuorum,
                    Cod_Coloracion = codColoracion
                };

                string numeroOrden = await GuardarOrdenServicioAsync(datosOrden, command);

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
                bool resultado = true;
                string ojo = _L_Articulo.CalcularOjoDesdeGrid(dgvArticulos);
                bool productosRepetidos = false;
                string codigoProductoRepetido = null;
                int cantidadTotalRepetida = 0;
                var CodLab = Cbx_Pnl2_Laboratorio.SelectedValue.ToString();
                var glbCodDetVta = Cbx_Pnl2_Trbajo.SelectedValue.ToString();
                var glbManejaExisLC = _D_DetalleOrden.TB_PARAMETRO("LCManejaExist");
                bool empresaAfiliada = EmpresaAfiliada;

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
                    decimal impuesto = Convert.ToDecimal(row.Cells["Impuesto"].Value);
                    decimal precioViejo = row.Cells["PrecioViejo"].Value != DBNull.Value ? Convert.ToDecimal(row.Cells["PrecioViejo"].Value) : 0;
                    string codPromo = row.Cells["CodPromo"].Value?.ToString();
                    decimal costoPromedio = row.Cells["costoProme"].Value != DBNull.Value ? Convert.ToDecimal(row.Cells["costoProme"].Value) : 0;
                    string codigoLab = CodLab;
                    decimal porcentajeDescuento = _L_Articulo.ObtenerPorcentajeDescuento(row, empresaAfiliada);

                    int cantidadFinal = cantidad;
                    if (productosRepetidos && codArticulo == codigoProductoRepetido)
                        cantidadFinal = cantidadTotalRepetida;

                    bool DetalleOrdenServicio = await _L_Articulo.GuardarDescripcionDetalleOrdenServicioAsync(
                        numeroOrden,
                        numeroRevision,
                        codigoVenta,
                        codArticulo,
                        cantidadFinal,
                        ojo,
                        precio,
                        impuesto,
                        porcentajeDescuento,
                        precioViejo,
                        codPromo,
                        costoPromedio,
                        sucursal,
                        command
                    );

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
        public async Task<bool> ActualizarTrabajoYExistencias( DataGridView dgvArticulos, string cedula, string nacRif, 
                                                                    string numeroOrden, string usuarioActual, string sucursal, 
                                                                    string correlativoOS, SqlCommand command, string codDetVta, 
                                                                    string manejaExisLC)
        {
            try
            {
                // 1. Actualizar datos en TB_TRABAJO
                bool trabajoActualizado = await _L_Articulo.ModificarTrabajo(
                    cedula,
                    nacRif,
                    numeroOrden,
                    DateTime.Today,
                    usuarioActual,
                    sucursal,
                    correlativoOS,
                    command
                );

                if (!trabajoActualizado)
                {
                    MessageBox.Show("No se pudo actualizar la tabla TB_TRABAJO.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                //// 2. Actualizar existencias si corresponde
                //for (int fila = 0; fila < dgvArticulos.Rows.Count; fila++)
                //{
                //    var row = dgvArticulos.Rows[fila];
                //    if (row.IsNewRow) continue;

                //    string codArticulo = row.Cells["CodArticulo"].Value?.ToString();
                //    string codigoLab = row.Cells["CodigoLab"].Value?.ToString();
                //    int cantidad = Convert.ToInt32(row.Cells["ART_EXIST"].Value);

                //    //bool rebajoExistencia = false;

                //    //if (manejaExisLC == "1" && codDetVta == "02")
                //    //{
                //    //    rebajoExistencia = await _L_Articulo.RebajarInventario(codArticulo, codigoLab, cantidad, command);
                //    //}
                //    //else
                //    //{
                //    //    rebajoExistencia = await _L_Articulo.RebajarInventario(codArticulo, null, cantidad, command);
                //    //}

                //    //if (!rebajoExistencia)
                //    //{
                //    //    MessageBox.Show($"No se pudo rebajar inventario para el artículo {codArticulo}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    //    return false;
                //    //}
                //}

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error general al actualizar TB_TRABAJO o rebajar inventario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }





    }




}



