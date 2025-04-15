using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades;
using CapaDatos.Anulacion;
using CapaDatos.CargarOrdenes_Datos;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using CapaDatos.Conexion;

namespace CapaLogica.CargarOrdenes
{
    public class L_Articulo
    {
        D_Anulacion _D_Anulacion = new D_Anulacion();
        D_Articulos _D_Articulos = new D_Articulos();
        //El uso de la clase StringBuilder nos ayudara a devolver los mensajes 
        public readonly StringBuilder stringBuilder = new StringBuilder();


        public void CargarArticulos (System.Windows.Forms.DataGridView DgvArticulo, List<TB_ARTICULO> listaArticulos)
        {
            Conexion cn = new Conexion();
            SqlConnection connection = cn.LeerCadena();
            SqlCommand command = connection.CreateCommand();
            SqlTransaction transaction;
            // Iniciar la transacción
            transaction = connection.BeginTransaction();
            command.Connection = connection;
            command.Transaction = transaction;
            command.Parameters.Clear();
            command.CommandTimeout = 120;

            try
            {
                // Obtener los artículos desde la base de datos
                var articulosObtenidos = _D_Articulos.ObtenerArticulos(command);

                // Limpiar la lista pasada como parámetro y llenarla con los nuevos datos
                listaArticulos.Clear(); // Limpiar la lista para evitar duplicados
                listaArticulos.AddRange(articulosObtenidos); // Agregar los datos obtenidos

                // Asignar la lista como fuente de datos del DataGridView
                if (listaArticulos != null && listaArticulos.Count > 0 && _D_Articulos.stringBuilder.Length== 0)
                {
                    //DgvArticulo.DataSource = listaArticulos;
                    
                    // Confirmar la transacción
                    transaction.Commit();
                }
                else
                {
                    //DgvArticulo.DataSource = null; // Si no hay datos, limpiar el DataGridView
                    _D_Articulos.stringBuilder.AppendLine("No se pudieron cargar los artículos correctamente.");
                    transaction.Rollback();
                }

        
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                transaction.Rollback();
            }

        }

        public void FiltrarArticulos(string filtro, System.Windows.Forms.RadioButton Rd_Pnl3_Descripcion, System.Windows.Forms.RadioButton Rd_Pnl3_Codigo, System.Windows.Forms.DataGridView Dgv_Pnl3_Articulo, List<TB_ARTICULO> listaArticulos, List<TB_ARTICULO> listaTemporal)
        {

            // Verificar si el filtro está vacío
            if (string.IsNullOrWhiteSpace(filtro))
            {
                // Restablecer la información original en el DataGridView
                listaTemporal = new List<TB_ARTICULO>(listaArticulos); // Restaurar desde la lista original
                Dgv_Pnl3_Articulo.DataSource = listaTemporal;
                return;
            }

            // Convertir el filtro a minúsculas para una búsqueda insensible a mayúsculas
            filtro = filtro.ToLower();

            // Crear una lista para almacenar los resultados filtrados
            var datosFiltrados = new List<TB_ARTICULO>();

            // Recorrer la lista original (listaArticulos) para aplicar el filtro
            foreach (var articulo in listaArticulos)
            {
                // Filtrar según la opción seleccionada
                if (Rd_Pnl3_Descripcion.Checked && articulo.DESART != null && articulo.DESART.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(articulo);
                }
                else if (Rd_Pnl3_Codigo.Checked && articulo.CodArticulo != null && articulo.CodArticulo.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(articulo);
                }
            }

            // Actualizar la lista temporal con los datos filtrados
            listaTemporal = datosFiltrados;

            // Actualizar la fuente de datos del DataGridView con los resultados filtrados
            Dgv_Pnl3_Articulo.DataSource = datosFiltrados;
        }

        public bool CargarArticulo_ValidarTexbox(System.Windows.Forms.TextBox Codigo, System.Windows.Forms.TextBox Descripcion, System.Windows.Forms.TextBox Precio, System.Windows.Forms.TextBox Cantidad)
        {
            // Validar si el campo Código está vacío
            if (string.IsNullOrWhiteSpace(Codigo.Text))
            {
                Codigo.Focus(); // Establecer el foco en el campo Código
                return true;
            }

            // Validar si el campo Descripción está vacío
            if (string.IsNullOrWhiteSpace(Descripcion.Text))
            {
                Descripcion.Focus(); // Establecer el foco en el campo Descripción
                return true;
            }

            // Validar si el campo Precio está vacío
            if (string.IsNullOrWhiteSpace(Precio.Text))
            {
                Precio.Focus(); // Establecer el foco en el campo Precio
                return true;
            }

            // Validar si el campo Cantidad está vacío
            if (string.IsNullOrWhiteSpace(Cantidad.Text))
            {
                Cantidad.Focus(); // Establecer el foco en el campo Cantidad
                return true;
            }

            // Si todos los campos tienen valores, devolver false
            return false;

        }

        public bool CargarArticulo_EvitarDuplicado(System.Windows.Forms.DataGridView Dgv_Tap3_Articulo, System.Windows.Forms.TextBox Codigo)
        {
            foreach (DataGridViewRow row in Dgv_Tap3_Articulo.Rows)
            {
                if (row.Cells["CodArticulo"].Value?.ToString() == Codigo.Text)
                {
                    MessageBox.Show("El artículo ya está agregado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return true;
                }
            }

            return false;
        }

        public string ValidarExistenciaProducto(string codigoProducto, int cantidadIngresada, List<TB_ARTICULO> listaArticulos)
        {
            // Buscar el producto en la lista por su código
            var articulo = listaArticulos.FirstOrDefault(a => a.CodArticulo == codigoProducto);

            // Verificar si el producto existe en la lista
            if (articulo == null)
            {
                return "El producto no existe en la lista.";
            }

            // Verificar si el producto maneja existencia
            if (!articulo.MANEJAEXISTENCIA)
            {
                return "El producto no tiene existencia.";
            }

            // Comparar la cantidad ingresada con la existencia disponible
            if (cantidadIngresada > articulo.ART_EXIST)
            {
                return $"La cantidad ingresada ({cantidadIngresada}) excede la existencia disponible ({articulo.ART_EXIST}).";
            }

            // Si todo es válido, devolver true
            return "";
        }

        public string ValidarCantidadMaximaPermitida(string codigoProducto, int cantidadIngresada)
        {
            DataTable DT_ValorMaximo = _D_Articulos.BucarArticuloMaximoPorVenta(codigoProducto.Substring(0, 1));

            foreach (DataRow row in DT_ValorMaximo.Rows)
            {
                int ValorMaximoPorArticulo = Convert.ToInt32(row["Max_Vta"].ToString());
                if(cantidadIngresada > ValorMaximoPorArticulo)
                {
                    return "El articulo " + codigoProducto + " tiene una cantidad a vender mayor que el maximo permitido.";
                }
            }
            return "";
        }

        public void FormatearCampo7Digitos(System.Windows.Forms.TextBox CodigoArticulo)
        {
            // Validar que el TextBox no sea nulo y que tenga texto
            if (CodigoArticulo == null || string.IsNullOrWhiteSpace(CodigoArticulo.Text))
            {
                return; // Salir si el TextBox está vacío o es nulo
            }

            if (CodigoArticulo.Text.StartsWith("C", StringComparison.OrdinalIgnoreCase))
            {
                switch (CodigoArticulo.Text.Length)
                {
                    case 1:
                        CodigoArticulo.Text = CodigoArticulo.Text + "000000";
                        break;
                    case 2:
                        CodigoArticulo.Text = CodigoArticulo.Text.Substring(0, 1) + "00000" + CodigoArticulo.Text.Substring(1, 1);
                        break;
                    case 3:
                        CodigoArticulo.Text = CodigoArticulo.Text.Substring(0, 1) + "0000" + CodigoArticulo.Text.Substring(1, 2);
                        break;
                    case 4:
                        CodigoArticulo.Text = CodigoArticulo.Text.Substring(0, 1) + "000" + CodigoArticulo.Text.Substring(1, 3);
                        break;
                    case 5:
                        CodigoArticulo.Text = CodigoArticulo.Text.Substring(0, 1) + "00" + CodigoArticulo.Text.Substring(1, 4);
                        break;
                    case 6:
                        CodigoArticulo.Text = CodigoArticulo.Text.Substring(0, 1) + "0" + CodigoArticulo.Text.Substring(1, 5);
                        break;
                }
            }
        }

        public bool VerificoCantidadCristales(string codigoProducto, int cantidadIngresada, List<TB_TRABAJO> trabajos)
        {
            // Validar que la lista no sea nula o vacía
            if (trabajos == null || trabajos.Count == 0)
            {
                throw new ArgumentException("La lista de trabajos no puede estar vacía.");
            }

            // Iterar sobre los trabajos para verificar las condiciones
            foreach (var trabajo in trabajos)
            {
                // Verificar si el trabajo es "CONVENCIONAL" y el código comienza con "C"
                if (trabajo.T_TIPOTRABAJO == "CONVENCIONAL" && codigoProducto.StartsWith("C", StringComparison.OrdinalIgnoreCase))
                {
                    int cristal = trabajo.T_OJO == "Ambos" ? 2 : 1;

                    // Validar la cantidad ingresada
                    if (cantidadIngresada > cristal)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                // Verificar si el trabajo es "CONTACTO" y el código comienza con "W"
                else if (trabajo.T_TIPOTRABAJO == "CONTACTO" && codigoProducto.StartsWith("W", StringComparison.OrdinalIgnoreCase))
                {
                    int cantc = trabajo.T_OJO == "Ambos" ? 2 : 1;

                    // Validar la cantidad ingresada
                    if (cantidadIngresada > cantc)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            // Si no se cumple ninguna condición, devolver false por defecto
            return false;
        }

        public void LlenarTB_Trbajo( List<TB_TRABAJO> _TRABAJO, string sucursal, string NacioNalidad, string Cedula)
        {
          try 
          { 

                // Obtener los artículos desde la base de datos
                var TRABAJOS = _D_Articulos.ObtenerTrabajo(sucursal, NacioNalidad, Cedula);

                 // Limpiar la lista pasada como parámetro y llenarla con los nuevos datos
                _TRABAJO.Clear(); // Limpiar la lista para evitar duplicados
                _TRABAJO.AddRange(TRABAJOS); // Agregar los datos obtenidos
          }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
            }
        }

        public int BuscarIva (string Codigo_Iva)
        {
            DataTable DT_Iva = _D_Articulos.BucarIva(Codigo_Iva);

            foreach (DataRow row in DT_Iva.Rows)
            {
                int Iva = Convert.ToInt32(row["Porcentaje"].ToString());
                return Iva; 
            }

            return 0;
        }


    }

}

