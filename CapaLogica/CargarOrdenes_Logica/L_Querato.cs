using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades; // Asegúrate de que esta referencia sea correcta
using CapaDatos.CargarOrdenes_Datos; // Asegúrate de que esta referencia sea correcta
using System.Windows.Forms;
using CapaDatos.Conexion;
using System.Data.SqlClient;
using System.Data;

namespace CapaLogica.CargarOrdenes_Logica
{
    public class L_Querato
    {
        private readonly D_Querato _D_Querato = new D_Querato();  // Usa el nombre de la clase de la capa de datos
        public readonly StringBuilder stringBuilder = new StringBuilder();

      
        public void FiltrarQuerato(string filtro, DataGridView DgvQuerato, List<TB_QUERATO> listaQuerato, List<TB_QUERATO> listaTemporal)
        {
            if (string.IsNullOrWhiteSpace(filtro))
            {
                listaTemporal.Clear();
                listaTemporal.AddRange(listaQuerato);
                DgvQuerato.DataSource = listaTemporal;
                return;
            }

            filtro = filtro.ToLower();
            var datosFiltrados = new List<TB_QUERATO>();

            foreach (var querato in listaQuerato)
            {
                // Ajusta las propiedades según tu clase TB_QUERATO
                if (querato.CTE_Nacio != null && querato.CTE_Nacio.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(querato);
                }
                else if (querato.CTE_CedIden != null && querato.CTE_CedIden.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(querato);
                }
                else if (querato.COD_Sucursal != null && querato.COD_Sucursal.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(querato);
                }
                else if (querato.QUE_OBSERV != null && querato.QUE_OBSERV.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(querato);
                }
                // Agrega más condiciones de filtrado para otros campos de TB_QUERATO
            }

            listaTemporal.Clear();
            listaTemporal.AddRange(datosFiltrados);
            DgvQuerato.DataSource = datosFiltrados;
        }

        public TB_QUERATO ObtenerQueratoPorClave(string cteNacio, string cteCedIden,  int numExamen) // Ejemplo de método para obtener por clave
        {
            return _D_Querato.ObtenerQuerato(cteNacio, cteCedIden,  numExamen); // Llama al método de la capa de datos
        }

        public int AgregarQuerato(TB_QUERATO nuevoQuerato)
        {
            return _D_Querato.AgregarQuerato(nuevoQuerato);
        }

    
    
        // Agrega aquí otros métodos para operaciones de lógica de negocio
        // relacionadas con Querato, como validaciones.
    }
}
