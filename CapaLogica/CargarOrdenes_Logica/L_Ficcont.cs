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
    public class L_Ficcont
    {
        private readonly D_FicCont _D_Ficcont = new D_FicCont();  // Usa el nombre de la clase de la capa de datos proporcionada
        public readonly StringBuilder stringBuilder = new StringBuilder();

     
        public  void FiltrarFiccont(string filtro, DataGridView DgvFiccont, List<TB_FICCONT> listaFiccont, List<TB_FICCONT> listaTemporal)
        {
            if (string.IsNullOrWhiteSpace(filtro))
            {
                listaTemporal.Clear();
                listaTemporal.AddRange(listaFiccont);
                DgvFiccont.DataSource = listaTemporal;
                return;
            }

            filtro = filtro.ToLower();
            var datosFiltrados = new List<TB_FICCONT>();

            foreach (var ficcont in listaFiccont)
            {
                // Ajusta las propiedades según tu clase TB_FICCONT
                if (ficcont.CTE_Nacio != null && ficcont.CTE_Nacio.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(ficcont);
                }
                else if (ficcont.CTE_CedIden != null && ficcont.CTE_CedIden.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(ficcont);
                }
                else if (ficcont.COD_Sucursal != null && ficcont.COD_Sucursal.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(ficcont);
                }
                else if (ficcont.COLOR != null && ficcont.COLOR.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(ficcont);
                }

                // Agrega más condiciones de filtrado para otros campos de TB_FICCONT
            }

            listaTemporal.Clear();
            listaTemporal.AddRange(datosFiltrados);
            DgvFiccont.DataSource = datosFiltrados;
        }

        public TB_FICCONT ObtenerFiccontPorClave(string cteNacio, string cteCedIden,  int numExamen) // Ejemplo de método para obtener por clave
        {
            return _D_Ficcont.ObtenerFicCont(cteNacio, cteCedIden,  numExamen); // Llama al método de la capa de datos
        }

        public int AgregarFiccont(TB_FICCONT nuevoFiccont)
        {
            return _D_Ficcont.AgregarFicCont(nuevoFiccont);
        }

   

       
        // Agrega aquí otros métodos para operaciones de lógica de negocio
        // relacionadas con Ficcont, como validaciones.
    }
}
