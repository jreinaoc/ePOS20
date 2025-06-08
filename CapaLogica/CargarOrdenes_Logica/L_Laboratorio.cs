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
    public class L_Laboratorio
    {
        private readonly D_Laboratorio _D_Laboratorio = new D_Laboratorio();
        public readonly StringBuilder stringBuilder = new StringBuilder();

        public void CargarLaboratorios(DataGridView DgvLaboratorio, List<TB_LABORATORIOS> listaLaboratorios)
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
                var laboratoriosObtenidos = _D_Laboratorio.ObtenerTodosLosLaboratorios();

                listaLaboratorios.Clear();
                listaLaboratorios.AddRange(laboratoriosObtenidos);

                if (listaLaboratorios != null && listaLaboratorios.Count > 0 && _D_Laboratorio.stringBuilder.Length == 0)
                {
                    DgvLaboratorio.DataSource = listaLaboratorios;
                    transaction.Commit();
                }
                else
                {
                    DgvLaboratorio.DataSource = null;
                    _D_Laboratorio.stringBuilder.AppendLine("No se pudieron cargar los laboratorios correctamente.");
                    transaction.Rollback();
                }
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                transaction.Rollback();
            }
        }

        public void FiltrarLaboratorios(string filtro, DataGridView DgvLaboratorio, List<TB_LABORATORIOS> listaLaboratorios, List<TB_LABORATORIOS> listaTemporal)
        {
            if (string.IsNullOrWhiteSpace(filtro))
            {
                listaTemporal = new List<TB_LABORATORIOS>(listaLaboratorios);
                DgvLaboratorio.DataSource = listaTemporal;
                return;
            }

            filtro = filtro.ToLower();
            var datosFiltrados = new List<TB_LABORATORIOS>();

            foreach (var laboratorio in listaLaboratorios)
            {
                if (laboratorio.CODIGO_LAB != null && laboratorio.CODIGO_LAB.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(laboratorio);
                }
                else if (laboratorio.DESCRIPCION != null && laboratorio.DESCRIPCION.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(laboratorio);
                }
                else if (laboratorio.DIRECCION != null && laboratorio.DIRECCION.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(laboratorio);
                }
                else if (laboratorio.TELEFONO != null && laboratorio.TELEFONO.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(laboratorio);
                }
                else if (laboratorio.CONTACTO != null && laboratorio.CONTACTO.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(laboratorio);
                }
                // Puedes agregar más condiciones de filtrado por otros campos de TB_LABORATORIOS
            }

            listaTemporal = datosFiltrados;
            DgvLaboratorio.DataSource = datosFiltrados;
        }

        public TB_LABORATORIOS ObtenerLaboratorioPorCodigo(string codigoLaboratorio)
        {
            return _D_Laboratorio.ObtenerLaboratorioPorCodigo(codigoLaboratorio);
        }

        public void AgregarLaboratorio(TB_LABORATORIOS nuevoLaboratorio)
        {
            _D_Laboratorio.AgregarLaboratorio(nuevoLaboratorio);
            if (_D_Laboratorio.stringBuilder.Length > 0)
            {
                stringBuilder.Append(_D_Laboratorio.stringBuilder);
            }
        }

        public void ActualizarLaboratorio(TB_LABORATORIOS laboratorioActualizado)
        {
            _D_Laboratorio.ActualizarLaboratorio(laboratorioActualizado);
            if (_D_Laboratorio.stringBuilder.Length > 0)
            {
                stringBuilder.Append(_D_Laboratorio.stringBuilder);
            }
        }

        public void EliminarLaboratorio(string codigoLaboratorio)
        {
            _D_Laboratorio.EliminarLaboratorio(codigoLaboratorio);
            if (_D_Laboratorio.stringBuilder.Length > 0)
            {
                stringBuilder.Append(_D_Laboratorio.stringBuilder);
            }
        }

        public List<TB_LABORATORIOS> ObtenerLaboratorios() // <-- Verifica este método
        {
            return _D_Laboratorio.ObtenerTodosLosLaboratorios();
        }

        // Aquí puedes agregar métodos para realizar otras operaciones de lógica de negocio
        // relacionadas con los laboratorios, como validaciones antes de guardar, etc.
    }
}