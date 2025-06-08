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
    public class L_Ficconv
    {
        private readonly D_Ficconv _D_Ficconv = new D_Ficconv();  // Usa el nombre de la clase de la capa de datos proporcionada
        public readonly StringBuilder stringBuilder = new StringBuilder();


        public void FiltrarFicconv(string filtro, DataGridView DgvFicconv, List<TB_FICCONVCTE> listaFicconv, List<TB_FICCONVCTE> listaTemporal)
        {
            if (string.IsNullOrWhiteSpace(filtro))
            {
                listaTemporal.Clear();
                listaTemporal.AddRange(listaFicconv);
                DgvFicconv.DataSource = listaTemporal;
                return;
            }

            filtro = filtro.ToLower();
            var datosFiltrados = new List<TB_FICCONVCTE>();

            foreach (var ficconv in listaFicconv)
            {
                // Ajusta las propiedades según tu clase TB_ficconv
                if (ficconv.CTE_Nacio != null && ficconv.CTE_Nacio.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(ficconv);
                }
                else if (ficconv.CTE_CedIden != null && ficconv.CTE_CedIden.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(ficconv);
                }
                else if (ficconv.COD_Sucursal != null && ficconv.COD_Sucursal.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(ficconv);
                }

                // Agrega más condiciones de filtrado para otros campos de TB_ficconv
            }

            listaTemporal.Clear();
            listaTemporal.AddRange(datosFiltrados);
            DgvFicconv.DataSource = datosFiltrados;
        }

        public TB_FICCONVCTE ObtenerFicconvPorClave(string cteNacio, string cteCedIden,  int numExamen) // Ejemplo de método para obtener por clave
        {
            return _D_Ficconv.ObtenerFicConv(cteNacio, cteCedIden,  numExamen); // Llama al método de la capa de datos
        }

        public string AgregarFicconv(TB_FICCONVCTE nuevoFicconv)
        {
             _D_Ficconv.AgregarFicConv(nuevoFicconv);
            return "Guardado";
        }

  
        // Agrega aquí otros métodos para operaciones de lógica de negocio
        // relacionadas con Ficconv, como validaciones.
    }
}
