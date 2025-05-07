using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaLogica.CargarOrdenes
{
    public class ServicioValidaciones_CargarOrdenes
    {
        private readonly L_Articulo _servicio;

        public ServicioValidaciones_CargarOrdenes()
        {
            _servicio = new L_Articulo();
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
                    return _servicio.EjecutarServicioColoracion_btnProcesar(cristalColor, !esDegradado, command); // List<ServicioColoracion_CargarOrdenes>
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
                return _servicio.ObtenerServiciosARDataset(codCristal, codServicio, command);
            });
        }


        public bool VerificoIgualAntirefCrist(DataGridView grid, DataSet dsServAR)
        {
            try
            {
                int cantCristales = 0;
                bool cambiosRealizados = false;

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
                                break;
                            }
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

        //private void ActualizarCantidadFactura(DataRow fila, int nuevaCantidad)
        //{
        //    decimal precioUnitario = Convert.ToDecimal(fila["ART_PVP"]);

        //    fila.BeginEdit();
        //    fila["TienePromo"] = precioUnitario;
        //    fila["Precio"] = precioUnitario.ToString("N2");
        //    fila["Can"] = nuevaCantidad;
        //    fila["Total"] = (precioUnitario * nuevaCantidad).ToString("N2");
        //    // PromoEvaluada se conserva
        //    fila.EndEdit();
        //}






    }


}



