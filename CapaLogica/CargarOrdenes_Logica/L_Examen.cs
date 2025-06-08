using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades;
using CapaDatos.CargarOrdenes_Datos;
using System.Windows.Forms;
using CapaDatos.Conexion;
using System.Data.SqlClient;
using System.Data; // Se agregó esta línea para poder usar ConnectionState

namespace CapaLogica.CargarOrdenes_Logica
{
    public class L_Examen
    {
        private readonly D_Examen _D_Examen = new D_Examen();
        // El uso de la clase StringBuilder nos ayudará a devolver los mensajes
        public readonly StringBuilder stringBuilder = new StringBuilder();

        public void CargarExamenes(DataGridView DgvExamen, List<TB_EXAMENCTE> listaExamenes)
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
                // Obtener los exámenes desde la base de datos
                var examenesObtenidos = _D_Examen.ObtenerTodosLosExamenes();

                // Limpiar la lista pasada como parámetro y llenarla con los nuevos datos
                listaExamenes.Clear(); // Limpiar la lista para evitar duplicados
                if (examenesObtenidos != null)
                    listaExamenes.AddRange(examenesObtenidos); // Agregar los datos obtenidos

                // Asignar la lista como fuente de datos del DataGridView
                if (listaExamenes != null && listaExamenes.Count > 0 && _D_Examen.stringBuilder.Length == 0)
                {
                    DgvExamen.DataSource = listaExamenes;

                    // Confirmar la transacción
                    transaction.Commit();
                }
                else
                {
                    DgvExamen.DataSource = null; // Si no hay datos, limpiar el DataGridView
                    if (_D_Examen.stringBuilder.Length == 0)
                        _D_Examen.stringBuilder.AppendLine("No se pudieron cargar los exámenes correctamente.");
                    transaction.Rollback();
                }
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                transaction.Rollback();
            }
            finally
            {
                if (connection.State == ConnectionState.Open) // Aquí es donde se usa ConnectionState
                    connection.Close();
            }
        }

        public void FiltrarExamenes(string filtro, DataGridView DgvExamen, List<TB_EXAMENCTE> listaExamenes, List<TB_EXAMENCTE> listaTemporal)
        {
            // Verificar si el filtro está vacío
            if (string.IsNullOrWhiteSpace(filtro))
            {
                // Restablecer la información original en el DataGridView
                listaTemporal.Clear();
                listaTemporal.AddRange(listaExamenes); // Restaurar desde la lista original
                DgvExamen.DataSource = listaTemporal;
                return;
            }

            // Convertir el filtro a minúsculas para una búsqueda insensible a mayúsculas
            filtro = filtro.ToLower();

            // Crear una lista para almacenar los resultados filtrados
            var datosFiltrados = new List<TB_EXAMENCTE>();

            // Recorrer la lista original (listaExamenes) para aplicar el filtro
            foreach (var examen in listaExamenes)
            {
                // Filtrar por los campos que consideres relevantes para la búsqueda
                if (examen.CTE_Nacio != null && examen.CTE_Nacio.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(examen);
                }
                else if (examen.CTE_CedIden != null && examen.CTE_CedIden.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(examen);
                }
                else if (examen.NUM_Examen.ToString().Contains(filtro))
                {
                    datosFiltrados.Add(examen);
                }
                // Puedes agregar más condiciones de filtrado por otros campos
            }

            // Actualizar la lista temporal con los datos filtrados
            listaTemporal.Clear();
            listaTemporal.AddRange(datosFiltrados);

            // Actualizar la fuente de datos del DataGridView con los resultados filtrados
            DgvExamen.DataSource = datosFiltrados;
        }

        public TB_EXAMENCTE ObtenerExamenPorNumero(int numeroExamen)
        {
            return _D_Examen.ObtenerExamenPorNumero(numeroExamen);
        }

        public string AgregarExamen(TB_EXAMENCTE nuevoExamen)
        {

            stringBuilder.Clear();
            try
            {
                _D_Examen.AgregarExamen(nuevoExamen);
                return "Guardado";
            }
            catch (Exception ex)
            {
                return $"Error al guardar el cliente: {ex.Message}";
            }
            
        }

        //public string GuardarExamen(TB_EXAMEN examen)
        //{
        //    //stringBuilder.Clear();
        //    //if (ValidarExamen(examen))
        //    //{
        //    //    try
        //    //    {
        //            _D_Examen.AgregarExamen(examen);
        //            return "Guardado";
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        stringBuilder.AppendLine($"Error al guardar el examen: {ex.Message}");
        //    //        return $"Error al guardar el AgregarExamen: {ex.Message}";
        //    //    }
        //    //}
        //    //return $"Error al guardar el AgregarExamen";
        //}


        //private bool ValidarExamen(TB_EXAMEN examen)
        //{
        //    stringBuilder.Clear();
        //    if (string.IsNullOrEmpty(examen.CTE_CedIden))
        //    {
        //        stringBuilder.AppendLine("La cédula del paciente es obligatoria.");
        //    }
        //    if (examen.NUM_Examen <= 0)
        //    {
        //        stringBuilder.AppendLine("El número de examen debe ser mayor que cero.");
        //    }

        //    return stringBuilder.Length == 0;
        //}
    }
}
