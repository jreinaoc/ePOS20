using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades;
using CapaDatos.CargarOrdenes_Datos;
using System.Windows.Forms;

namespace CapaLogica.CargarOrdenes_Logica
{
    public class L_Trabajo
    {
        private readonly D_Trabajo _D_Trabajo = new D_Trabajo();
        public readonly StringBuilder stringBuilder = new StringBuilder();

        public TB_TRABAJOCTE ObtenerTrabajoPorOrdenServicio(string nacionalidad, string cedula, int examen)
        {
            // Aquí podrías agregar lógica de negocio o validaciones antes de llamar a la capa de datos.
            return _D_Trabajo.ObtenerTrabajoPorOrdenServicio(nacionalidad, cedula, examen);
        }

        public void AgregarTrabajo(TB_TRABAJOCTE nuevoTrabajo)
        {
           

             _D_Trabajo.AgregarTrabajo(nuevoTrabajo);
        }

        public void ActualizarTrabajoRx(TB_TRABAJOCTE nuevoTrabajo)
        {


            _D_Trabajo.ActualizarTrabajoRx(nuevoTrabajo);
        }
        // Puedes agregar más métodos de la capa de datos aquí y, si es necesario,
        // implementar lógica de negocio adicional. Por ejemplo, un método para
        // obtener todos los trabajos, o trabajos por un criterio diferente.
    }
}