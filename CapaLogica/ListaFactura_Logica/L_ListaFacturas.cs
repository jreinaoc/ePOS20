using CapaDatos.Anulacion;
using CapaDatos.DetalleOrden_Datos;
using CapaDatos.Inicio_Datos;
using CapaDatos.ListaFacturas_Datos;
using CapaDatos.ListaOrdenes_Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica.ListaFactura_Logica
{
    public class L_ListaFacturas
    {
        //El uso de la clase StringBuilder nos ayudara a devolver los mensajes de las validaciones
        public readonly StringBuilder stringBuilder = new StringBuilder();

        //Instanciamos nuestra clase D_Loguin para poder utilizar sus miembros
        private D_ListaOrdenes _D_ListaOrdenes = new D_ListaOrdenes();

        private D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        private D_Inicio _D_Inicio = new D_Inicio();
        private D_Anulacion _D_Anulacion = new D_Anulacion();
        private D_ListaFactura _D_ListaFactura = new D_ListaFactura();



        public DataSet CargarFacturas(int Inicio = 1, int Final = 12)
        {
            try
            {
                stringBuilder.Clear();

                //Le enviamos el index asociados al valor selecionado en el combobox 

                DataSet Ordenes  = _D_ListaFactura.CargarFacturas("", "", Inicio, Final);

                if (Ordenes.Tables[0].Rows.Count > 0)
                {
                    return Ordenes;
                }
                stringBuilder.Append(Environment.NewLine + "No hay ordenes");
                return null;

            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
            }

        }

        public DataSet TraerFacturasRango(System.Windows.Forms.DateTimePicker Fechadesde, System.Windows.Forms.DateTimePicker Fechahasta, int Inicio = 1, int Final = 12)
        {
            try
            {
                stringBuilder.Clear();


                DateTime PRUE = Fechadesde.Value;
                DateTime PRUEB = Fechahasta.Value;
                string PeriodoDesde = PRUE.ToString("yyyyMMdd");
                string PeriodoHasta = PRUEB.ToString("yyyyMMdd");

                //Le enviamos el index asociados al valor selecionado en el combobox 
                DataSet Ordenesrango = _D_ListaFactura.CargarFacturas(PeriodoDesde, PeriodoHasta, Inicio, Final);

                if (Ordenesrango.Tables[0].Rows.Count > 0)
                {
                    return Ordenesrango;
                }
                stringBuilder.Append(Environment.NewLine + "No hay ordenes");
                return null;

            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
            }
        }

        public DataSet TraerNotasRango(System.Windows.Forms.DateTimePicker Fechadesde, System.Windows.Forms.DateTimePicker Fechahasta, int Inicio = 1, int Final = 12)
        {
            try
            {
                stringBuilder.Clear();


                DateTime PRUE = Fechadesde.Value;
                DateTime PRUEB = Fechahasta.Value;
                string PeriodoDesde = PRUE.ToString("yyyyMMdd");
                string PeriodoHasta = PRUEB.ToString("yyyyMMdd");

                //Le enviamos el index asociados al valor selecionado en el combobox 
                DataSet Ordenesrango = _D_ListaFactura.CargarNotas(PeriodoDesde, PeriodoHasta, Inicio, Final);

                if (Ordenesrango.Tables[0].Rows.Count > 0)
                {
                    return Ordenesrango;
                }
                stringBuilder.Append(Environment.NewLine + "No hay ordenes");
                return null;

            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
            }
        }

    }
}
