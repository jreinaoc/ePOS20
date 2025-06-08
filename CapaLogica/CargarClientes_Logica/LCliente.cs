using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CapaEntidades;
using CapaDatos.CargarClientes_Datos;
using CapaDatos.Conexion;

namespace CapaLogica.CargarClientes_Logica
{
    public class L_Cliente
    {
        D_Clientes _D_Cliente = new D_Clientes();
        public readonly StringBuilder stringBuilder = new StringBuilder();


        public string GuardarCliente(TB_CTEPPAL cliente)
        {
            stringBuilder.Clear();
            try
            {
                _D_Cliente.InsertarCliente(cliente);
                return "Guardado";
            }
            catch (Exception ex)
            {
                return $"Error al guardar el cliente: {ex.Message}";
            }
        }

        public void CargarClientes(DataGridView DgvCliente, List<TB_CTEPPAL> listaClientes, string filtro, RadioButton Rd_Pnl3_Cedula, RadioButton Rd_Pnl3_Nombre)
        {
            Conexion cn = new Conexion(); // Instancia de tu clase de conexión
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlTransaction transaction = null;

            try
            {
                connection = cn.LeerCadena();
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                command = connection.CreateCommand();
                transaction = connection.BeginTransaction(); // Iniciar la transacción
                command.Connection = connection;
                command.Transaction = transaction;
                command.Parameters.Clear();
                command.CommandTimeout = 120;

                // Determinar los booleanos para el filtro según los RadioButtons
                bool buscarPorCedula = Rd_Pnl3_Cedula.Checked;
                bool buscarPorNombre = Rd_Pnl3_Nombre.Checked;

                // *** Llamar al método ObtenerClientes con los nuevos parámetros ***
                var clientesObtenidos = _D_Cliente.ObtenerClientes(filtro, buscarPorCedula, buscarPorNombre, command);

                // Limpiar la lista pasada como parámetro y llenarla con los nuevos datos
                listaClientes.Clear();
                if (clientesObtenidos != null)
                {
                    listaClientes.AddRange(clientesObtenidos);
                }

                // Asignar la lista como fuente de datos del DataGridView
                // Es crucial que DgvCliente.DataSource esté enlazado a una BindingList<TB_CTEPPAL>
                // si quieres que los cambios en listaClientes se reflejen automáticamente.
                // Si listaClientes es solo List<TB_CTEPPAL>, necesitarás reasignar el DataSource.
                DgvCliente.DataSource = null; // Desvincular para forzar actualización
                DgvCliente.DataSource = listaClientes; // Vincular la lista (ahora filtrada)

                // Si hay errores en la capa de datos, se reflejarán en _D_Cliente.stringBuilder
                if (_D_Cliente.stringBuilder.Length == 0) // No hay errores de la capa de datos
                {
                    transaction.Commit(); // Confirmar la transacción si todo fue bien
                }
                else
                {
                    // Si la capa de datos reportó un error, revertir y mostrarlo
                    transaction.Rollback();
                    stringBuilder.AppendLine(_D_Cliente.stringBuilder.ToString()); // Copiar el error
                    MessageBox.Show(_D_Cliente.stringBuilder.ToString(), "Error al Cargar Clientes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Opcional: Si no se encontraron clientes, puedes mostrar un mensaje
                if (listaClientes.Count == 0 && _D_Cliente.stringBuilder.Length == 0)
                {
                    //MessageBox.Show("No se encontraron clientes con el filtro especificado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                stringBuilder.AppendLine(string.Format("Error al cargar clientes: {0}", ex.Message));
                if (transaction != null)
                {
                    transaction.Rollback(); // Revertir la transacción en caso de error en la capa lógica
                }
                MessageBox.Show(stringBuilder.ToString(), "Error al Cargar Clientes", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close(); // Asegúrate de cerrar la conexión
                }
            }
        }


        public string GuardarClienteP(TB_CTEPPAL cliente)
        {
            stringBuilder.Clear();
            try
            {
                _D_Cliente.InsertarClienteP(cliente);
                return "Guardado";
            }
            catch (Exception ex)
            {
                return $"Error al guardar el cliente: {ex.Message}";
            }
        }

        //public void CargarClientes(DataGridView DgvCliente, List<TB_CTEPPAL> listaClientes, string filtro, RadioButton Rd_Pnl3_Cedula, RadioButton Rd_Pnl3_Nombre,)
        //{

        //    Conexion cn = new Conexion();
        //    SqlConnection connection = cn.LeerCadena();
        //    SqlCommand command = connection.CreateCommand();
        //    SqlTransaction transaction;

        //    // Iniciar la transacción
        //    transaction = connection.BeginTransaction();
        //    command.Connection = connection;
        //    command.Transaction = transaction;
        //    command.Parameters.Clear();
        //    command.CommandTimeout = 120;

        //    try
        //    {
        //        // Obtener los clientes desde la base de datos
        //        var clientesObtenidos = _D_Cliente.ObtenerClientes(command);

        //        // Limpiar la lista pasada como parámetro y llenarla con los nuevos datos
        //        listaClientes.Clear(); // Limpiar la lista para evitar duplicados
        //        listaClientes.AddRange(clientesObtenidos); // Agregar los datos obtenidos

        //        // Asignar la lista como fuente de datos del DataGridView
        //        if (listaClientes != null && listaClientes.Count > 0 && _D_Cliente.stringBuilder.Length == 0)
        //        {
        //            // DgvCliente.DataSource = listaClientes; // Se comenta para permitir personalización en la UI
        //            // Confirmar la transacción
        //            transaction.Commit();
        //        }
        //        else
        //        {
        //            // DgvCliente.DataSource = null; // Si no hay datos, limpiar el DataGridView
        //            _D_Cliente.stringBuilder.AppendLine("No se pudieron cargar los clientes correctamente.");
        //            transaction.Rollback();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        stringBuilder.Append(Environment.NewLine + string.Format("Error al cargar clientes: {0}", ex.Message));
        //        transaction.Rollback();
        //    }
        //}
   


        public void FiltrarClientes(string filtro, RadioButton Rd_Pnl3_Cedula, RadioButton Rd_Pnl3_Nombre, DataGridView Dgv_Pnl3_Cliente, List<TB_CTEPPAL> listaClientes, List<TB_CTEPPAL> listaTemporal)
        {
            // Verificar si el filtro está vacío
            if (string.IsNullOrWhiteSpace(filtro))
            {
                // Restablecer la información original en el DataGridView
                listaTemporal = new List<TB_CTEPPAL>(listaClientes); // Restaurar desde la lista original
                Dgv_Pnl3_Cliente.DataSource = listaTemporal;
                return;
            }

            // Convertir el filtro a minúsculas para una búsqueda insensible a mayúsculas
            filtro = filtro.ToLower();

            // Crear una lista para almacenar los resultados filtrados
            var datosFiltrados = new List<TB_CTEPPAL>();

            // Recorrer la lista original (listaClientes) para aplicar el filtro
            foreach (var cliente in listaClientes)
            {
                // Filtrar según la opción seleccionada
                if (Rd_Pnl3_Cedula.Checked && cliente.CTE_CedIden != null && cliente.CTE_CedIden.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(cliente);
                }
                else if (Rd_Pnl3_Nombre.Checked && cliente.CTE_PNombre != null && cliente.CTE_PNombre.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(cliente);
                }
                else if (Rd_Pnl3_Nombre.Checked && cliente.CTE_SNombre != null && cliente.CTE_SNombre.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(cliente);
                }
                else if (Rd_Pnl3_Nombre.Checked && cliente.CTE_PApellido != null && cliente.CTE_PApellido.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(cliente);
                }
                else if (Rd_Pnl3_Nombre.Checked && cliente.CTE_SApellido != null && cliente.CTE_SApellido.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(cliente);
                }
            }

            // Actualizar la lista temporal con los datos filtrados
            listaTemporal = datosFiltrados;

            // Actualizar la fuente de datos del DataGridView con los resultados filtrados
            Dgv_Pnl3_Cliente.DataSource = datosFiltrados;
        }


        public DataTable ObtenerClientePorCedula(string cedula, string nacio)
        {
            stringBuilder.Clear();
            DataTable cliente = _D_Cliente.ObtenerClientePorCedula(cedula,nacio); // Corregido a _D_Cliente
            if (cliente == null)
            {
                stringBuilder.AppendLine(_D_Cliente.stringBuilder.ToString()); // Corregido a _D_Cliente
            }
            return cliente;
        }

        public List<TB_MAESEDO> ObtenerEstados()
        {
            stringBuilder.Clear();
            List<TB_MAESEDO> estados = _D_Cliente.ObtenerEstados(); // Corregido a _D_Cliente
            if (estados == null)
            {
                stringBuilder.AppendLine(_D_Cliente.stringBuilder.ToString()); // Corregido a _D_Cliente
            }
            return estados;
        }

        public List<TB_MAESCIUD> ObtenerCiudadesPorEstado(string codigoEstado)
        {
            stringBuilder.Clear();
            List<TB_MAESCIUD> ciudades = _D_Cliente.ObtenerCiudadesPorEstado(codigoEstado); // Corregido a _D_Cliente
            if (ciudades == null)
            {
                stringBuilder.AppendLine(_D_Cliente.stringBuilder.ToString()); // Corregido a _D_Cliente
            }
            return ciudades;
        }


      
        ///////////////////////////////////////////
        public List<TB_CTEMAIL> ObtenerEmailsPorCedula(string cedula, string nacio)
        {
            stringBuilder.Clear();
            List<TB_CTEMAIL> emails = _D_Cliente.ObtenerEmailsPorCedula(cedula,  nacio);
            if (emails == null)
            {
                stringBuilder.AppendLine(_D_Cliente.stringBuilder.ToString());
            }
            return emails;
        }

public List<TB_CTETLF> ObtenerTelefonosPorCedula(string cedula ,string nacio)
        {
    stringBuilder.Clear();
    List<TB_CTETLF> telefonos = _D_Cliente.ObtenerTelefonosPorCedula(cedula,  nacio);
    if (telefonos == null)
    {
        stringBuilder.AppendLine(_D_Cliente.stringBuilder.ToString());
    }
    return telefonos;
}

// Puedes agregar métodos para Insertar, Actualizar, Eliminar emails y teléfonos
// que llamen a los métodos correspondientes en la capa de datos (D_Clientes)
// si necesitas esa funcionalidad desde la interfaz de usuario.

// Ejemplo para insertar un email (necesitarías un método en D_Clientes para esto):
public string InsertarEmail(TB_CTEMAIL email)
{
    stringBuilder.Clear();
    try
    {
        _D_Cliente.InsertarEmail(email);
        return "Email guardado";
    }
    catch (Exception ex)
    {
        return $"Error al guardar el email: {ex.Message}";
    }
}

////Ejemplo para insertar un teléfono (necesitarías un método en D_Clientes para esto):
 public string InsertarTelefono(TB_CTETLF telefono)
{
    stringBuilder.Clear();
    try
    {
        _D_Cliente.InsertarTelefono(telefono);
        return "Teléfono guardado";
    }
    catch (Exception ex)
    {
        return $"Error al guardar el teléfono: {ex.Message}";
    }
}

        /////////////////////////////////////////

  


        public DataTable ObtenerUsuariosOPTOMETRI()
        {
            stringBuilder.Clear();
            DataTable dt = _D_Cliente.ObtenerUsuariosOPTOMETRI(); // Llama al método en la capa de datos

            if (dt == null)
            {
                stringBuilder.AppendLine(_D_Cliente.stringBuilder.ToString()); // Obtiene el error de la capa de datos
                return null;
            }
            return dt;
        }

        public DataTable ObtenerClienteConGarantia(string sucursal, string cedula, string nacio)
        {
            stringBuilder.Clear();
            DataTable cliente = _D_Cliente.ObtenerClienteConGarantia(sucursal,cedula, nacio); // Corregido a _D_Cliente
            if (cliente == null)
            {
                stringBuilder.AppendLine(_D_Cliente.stringBuilder.ToString()); // Corregido a _D_Cliente
            }
            return cliente;
        }

        public DataTable ObtenerMotivosReposicion()
        {
            stringBuilder.Clear();
            DataTable motivo = _D_Cliente.ObtenerMotivosReposicion(); // Corregido a _D_Cliente
            if (motivo == null)
            {
                stringBuilder.AppendLine(_D_Cliente.stringBuilder.ToString()); // Corregido a _D_Cliente
            }
            return motivo;
        }

    }
}