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



    }
}
