using CapaDatos.Anulacion;
using CapaDatos.DetalleOrden_Datos;
using CapaDatos.Inicio_Datos;
using CapaDatos.ListaFacturas_Datos;
using CapaDatos.ListaOrdenes_Datos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public DataSet TraerFacturasSinPaginado(DateTimePicker desde, DateTimePicker hasta)
        {
            string inicio = desde.Value.ToString("yyyyMMdd");
            string fin = hasta.Value.ToString("yyyyMMdd");
            return _D_ListaFactura.CargarFacturas(inicio, fin, 1, int.MaxValue); 
        }

        public DataSet TraerNotasSinPaginado(DateTimePicker desde, DateTimePicker hasta)
        {
            string inicio = desde.Value.ToString("yyyyMMdd");
            string fin = hasta.Value.ToString("yyyyMMdd");
            return _D_ListaFactura.CargarNotas(inicio, fin, 1, int.MaxValue); 
        }

        public List<ReporteGlobal_CierreCaja> ObtenerCierreCajaPorFecha(DateTime fecha)
        {
            var lista_CierreCaja = new List<ReporteGlobal_CierreCaja>();

            DataSet ds = _D_ListaFactura.CierreCaja_ReporteGlobal(fecha);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return lista_CierreCaja;

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                lista_CierreCaja.Add(new ReporteGlobal_CierreCaja
                {
                    MANUALEFECTIVO = Convert.ToDecimal(row["MANUALEFECTIVO"]),
                    SISTEMAEFECTIVO = Convert.ToDecimal(row["SISTEMAEFECTIVO"]),
                    MANUALDEBITO = Convert.ToDecimal(row["MANUALDEBITO"]),
                    SISTEMADEBITO = Convert.ToDecimal(row["SISTEMADEBITO"]),
                    MANUALCREDITO = Convert.ToDecimal(row["MANUALCREDITO"]),
                    SISTEMACREDITO = Convert.ToDecimal(row["SISTEMACREDITO"]),
                    MANUALGASTOS = Convert.ToDecimal(row["MANUALGASTOS"]),
                    SISTEMAGASTOS = Convert.ToDecimal(row["SISTEMAGASTOS"]),
                    ManualIVARetenido = Convert.ToDecimal(row["ManualIVARetenido"]),
                    SistemaIVARetenido = Convert.ToDecimal(row["SistemaIVARetenido"]),
                    ManualISLRRetenido = Convert.ToDecimal(row["ManualISLRRetenido"]),
                    SistemaISLRRetenido = Convert.ToDecimal(row["SistemaISLRRetenido"]),
                    MANUALTRANSFERENCIA = Convert.ToDecimal(row["MANUALTRANSFERENCIA"]),
                    SISTEMATRANSFERENCIA = Convert.ToDecimal(row["SISTEMATRANSFERENCIA"]),
                    MANUALVUELTO = Convert.ToDecimal(row["MANUALVUELTO"]),
                    SISTEMAVUELTO = Convert.ToDecimal(row["SISTEMAVUELTO"]),
                    MANUALTOTALINGRESOS = Convert.ToDecimal(row["MANUALTOTALINGRESOS"]),
                    SISTEMATOTALINGRESOS = Convert.ToDecimal(row["SISTEMATOTALINGRESOS"]),
                    MANUALREINTEGROS = Convert.ToDecimal(row["MANUALREINTEGROS"]),
                    SISTEMAREINTEGRO = Convert.ToDecimal(row["SISTEMAREINTEGRO"]),
                    SISTEMANOTACREDITO = Convert.ToDecimal(row["SISTEMANOTACREDITO"]),
                    MANUALNOTACREDITO = Convert.ToDecimal(row["MANUALNOTACREDITO"]),
                    TOTAL_NETO = Convert.ToDecimal(row["TOTAL_NETO"]),
                    IMPUESTO = Convert.ToDecimal(row["IMPUESTO"]),
                    TOTAL_BRUTO = Convert.ToDecimal(row["TOTAL_BRUTO"]),
                });
            }

            return lista_CierreCaja;
        }

        public List<ReporteGlobal_Facturas> ObtenerFacturasPorFecha(DateTime fecha)
        {
            var lista_Facturas = new List<ReporteGlobal_Facturas>();

            DataSet ds = _D_ListaFactura.Facturas_ReporteGlobal(fecha);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return lista_Facturas;

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                lista_Facturas.Add(new ReporteGlobal_Facturas
                {
                    Cod_Sucursal = row["Cod_Sucursal"].ToString(),
                    Fact_Num = row["Fact_Num"].ToString(),
                    NumOrdServ = row["NumOrdServ"].ToString(),
                    CTE_CedIdenPAG = Convert.ToInt32(row["CTE_CedIdenPAG"]),
                    Fact_SubTotal = Convert.ToDecimal(row["Fact_SubTotal"]),
                    Fact_Descuento = Convert.ToDecimal(row["Fact_Descuento"]),
                    Fact_Impuesto = Convert.ToDecimal(row["Fact_Impuesto"]),
                    Fact_IGTF = Convert.ToDecimal(row["Fact_IGTF"]),
                    Fact_Total = Convert.ToDecimal(row["Fact_Total"])
                });
            }

            return lista_Facturas;
        }


















    }
}
