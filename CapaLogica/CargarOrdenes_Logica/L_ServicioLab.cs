using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades;
using CapaDatos; // Asegúrate de tener esta using para la clase D_Laboratorio
using System.Windows.Forms;
using CapaDatos.Conexion;
using System.Data.SqlClient;

namespace CapaLogica.CargarOrdenes_Logica
{
    public class L_ServicioLab
    {
        private readonly D_ServicioLab _D_ServicioLab = new D_ServicioLab();
        public readonly StringBuilder stringBuilder = new StringBuilder();

        public void CargarServicios(DataGridView DgvServicio, List<TB_SERVICIOSLAB> listaServicios)
        {
            Conexion cn = new Conexion();
            SqlConnection connection = cn.LeerCadena();
            SqlCommand command = connection.CreateCommand();
            SqlTransaction transaction;

            transaction = connection.BeginTransaction();
            command.Connection = connection;
            command.Transaction = transaction;
            command.Parameters.Clear();
            command.CommandTimeout = 120;

            try
            {
                var serviciosObtenidos = _D_ServicioLab.ObtenerTodosLosServicios();

                listaServicios.Clear();
                listaServicios.AddRange(serviciosObtenidos);

                if (listaServicios != null && listaServicios.Count > 0 && _D_ServicioLab.stringBuilder.Length == 0)
                {
                    DgvServicio.DataSource = listaServicios;
                    transaction.Commit();
                }
                else
                {
                    DgvServicio.DataSource = null;
                    _D_ServicioLab.stringBuilder.AppendLine("No se pudieron cargar los servicios correctamente.");
                    transaction.Rollback();
                }
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                transaction.Rollback();
            }
        }

        public void FiltrarServicios(string filtro, DataGridView DgvServicio, List<TB_SERVICIOSLAB> listaServicios, List<TB_SERVICIOSLAB> listaTemporal)
        {
            if (string.IsNullOrWhiteSpace(filtro))
            {
                listaTemporal = new List<TB_SERVICIOSLAB>(listaServicios);
                DgvServicio.DataSource = listaTemporal;
                return;
            }

            filtro = filtro.ToLower();
            var datosFiltrados = new List<TB_SERVICIOSLAB>();

            foreach (var servicio in listaServicios)
            {
                if (servicio.Cod_servicio != null && servicio.Cod_servicio.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(servicio);
                }
                else if (servicio.Descripcion_servicio != null && servicio.Descripcion_servicio.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(servicio);
                }
                //else if (servicio.CodArticulo != null && servicio.CodArticulo.ToLower().Contains(filtro))
                //{
                //    datosFiltrados.Add(servicio);
                //}
                //else if (servicio.Status_Servicio != null && servicio.Status_Servicio.ToLower().Contains(filtro))
                //{
                //    datosFiltrados.Add(servicio);
                //}
                // Puedes agregar más condiciones de filtrado por otros campos de TB_SERVICIOSLAB
            }

            listaTemporal = datosFiltrados;
            DgvServicio.DataSource = datosFiltrados;
        }

        public TB_SERVICIOSLAB ObtenerServicioPorCodigo(string codigoServicio)
        {
            return _D_ServicioLab.ObtenerServicioPorCodigo(codigoServicio);
        }

        public void AgregarServicio(TB_SERVICIOSLAB nuevoServicio)
        {
            _D_ServicioLab.AgregarServicio(nuevoServicio);
            if (_D_ServicioLab.stringBuilder.Length > 0)
            {
                stringBuilder.Append(_D_ServicioLab.stringBuilder);
            }
        }

        public void ActualizarServicio(TB_SERVICIOSLAB servicioActualizado)
        {
            _D_ServicioLab.ActualizarServicio(servicioActualizado);
            if (_D_ServicioLab.stringBuilder.Length > 0)
            {
                stringBuilder.Append(_D_ServicioLab.stringBuilder);
            }
        }

        public void EliminarServicio(string codigoServicio)
        {
            _D_ServicioLab.EliminarServicio(codigoServicio);
            if (_D_ServicioLab.stringBuilder.Length > 0)
            {
                stringBuilder.Append(_D_ServicioLab.stringBuilder);
            }
        }

        public List<TB_SERVICIOSLAB> ObtenerServicios()
        {
            return _D_ServicioLab.ObtenerTodosLosServicios();
        }

        // Aquí puedes agregar métodos para realizar otras operaciones de lógica de negocio
        // relacionadas con los servicios, como validaciones antes de guardar, etc.
    }
}