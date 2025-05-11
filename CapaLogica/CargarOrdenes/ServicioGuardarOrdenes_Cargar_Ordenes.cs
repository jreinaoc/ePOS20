using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaLogica.CargarOrdenes
{
    public class ServicioGuardarOrdenes_Cargar_Ordenes
    {
        private readonly L_Articulo _L_Articulo;

        public ServicioGuardarOrdenes_Cargar_Ordenes()
        {
            _L_Articulo = new L_Articulo();
        }

        public string GuardarOrdenServicio(AgregarOrdenServicio_CargarOrdenes datos)
        {
            string numeroOrden = _L_Articulo.AgregarOrdenServicio(
                datos.CodSucursal, datos.Revision, datos.CodVenta, datos.CteNacio, datos.CteCedIden, datos.NumExamen,
                datos.CodEmpleado, datos.CodLaboratorio, datos.CodServicio, datos.Vision, datos.FecOfrecido, datos.HorOfrecido,
                datos.FecEntrega, datos.FecEnvio, datos.VtaSubTotal, datos.VtaImpuesto, datos.VtaDescuento, datos.VtaTotal,
                datos.OrSerFinan, datos.OrSerStatus, datos.OrSerObserv, datos.UserCrea, datos.Fecha, datos.MonturaPropia,
                datos.CodDetVta, datos.Aplica, datos.OtCorrespondiente, datos.VentaAfil, datos.CristalPropio,
                datos.TipoMonturaPropia, datos.CodMotivoReposicion, datos.CedulaCteAfil, datos.CodigoEmpAfil,
                datos.Asegurada, datos.Exonerada, datos.MonturaEnQuorum, datos.CodColoracion
            );

            if (string.IsNullOrWhiteSpace(numeroOrden))
            {
                throw new Exception("No se generó número de orden. El procedimiento puede haber fallado.");
            }

            return numeroOrden;
        }




    }




}



