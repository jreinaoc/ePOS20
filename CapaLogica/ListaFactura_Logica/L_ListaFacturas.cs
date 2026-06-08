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



        public async Task<DataSet> CargarFacturas(int Inicio = 1, int Final = 12)
        {
            try
            {
                stringBuilder.Clear();

                //Le enviamos el index asociados al valor selecionado en el combobox 

                DataSet Ordenes  = await _D_ListaFactura.CargarFacturas("", "", Inicio, Final);

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

        public async Task<DataSet> TraerFacturasRango(System.Windows.Forms.DateTimePicker Fechadesde, System.Windows.Forms.DateTimePicker Fechahasta, int Inicio = 1, int Final = 12)
        {
            try
            {
                stringBuilder.Clear();

                DateTime PRUE = Fechadesde.Value;
                DateTime PRUEB = Fechahasta.Value;
                string PeriodoDesde = PRUE.ToString("yyyyMMdd");
                string PeriodoHasta = PRUEB.ToString("yyyyMMdd");

                DataSet Ordenesrango = await _D_ListaFactura.CargarFacturas(PeriodoDesde, PeriodoHasta, Inicio, Final);

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

        public async Task<DataSet> TraerNotasRango(System.Windows.Forms.DateTimePicker Fechadesde, System.Windows.Forms.DateTimePicker Fechahasta, int Inicio = 1, int Final = 12)
        {
            try
            {
                stringBuilder.Clear();

                DateTime PRUE = Fechadesde.Value;
                DateTime PRUEB = Fechahasta.Value;
                string PeriodoDesde = PRUE.ToString("yyyyMMdd");
                string PeriodoHasta = PRUEB.ToString("yyyyMMdd");

                // Aquí cambia por la versión ASYNC:
                DataSet Ordenesrango = await _D_ListaFactura.CargarNotas(PeriodoDesde, PeriodoHasta, Inicio, Final);

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

        public Task<DataSet> TraerFacturasSinPaginado(DateTimePicker desde, DateTimePicker hasta)
        {
            //string inicio = desde.Value.ToString("yyyyMMdd");
            //string fin = hasta.Value.ToString("yyyyMMdd");
            //return _D_ListaFactura.CargarFacturas(inicio, fin, 1, int.MaxValue);

            return Task.Run(() =>
            {
                string inicio = desde.Value.ToString("yyyyMMdd");
                string fin = hasta.Value.ToString("yyyyMMdd");
                return _D_ListaFactura.CargarFacturas(inicio, fin, 1, int.MaxValue);
            });
        }

        public Task<DataSet> TraerNotasSinPaginado(DateTimePicker desde, DateTimePicker hasta)
        {
            //string inicio = desde.Value.ToString("yyyyMMdd");
            //string fin = hasta.Value.ToString("yyyyMMdd");
            //return _D_ListaFactura.CargarNotas(inicio, fin, 1, int.MaxValue);

            return Task.Run(() =>
            {
                string inicio = desde.Value.ToString("yyyyMMdd");
                string fin = hasta.Value.ToString("yyyyMMdd");
                return _D_ListaFactura.CargarNotas(inicio, fin, 1, int.MaxValue);
            });

        }

        public async Task<List<ReporteGlobal_CierreCaja>> ObtenerCierreCajaPorFecha(DateTime fecha)
        {
            return await Task.Run(() =>
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
                        ManualImpMun = Convert.ToDecimal(row["ManualImpMun"]),
                        SistemaImpMun = Convert.ToDecimal(row["SistemaImpMun"]),
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
            });
        }

        public async Task<List<ReporteGlobal_Facturas>> ObtenerFacturasPorFecha(DateTime fecha)
        {
            return await Task.Run(() =>
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
                        CTE_CedIdenPAG = row["CTE_CedIdenPAG"].ToString(),
                        CTE_PNombre = row["CTE_PNombre"].ToString(),
                        Fact_SubTotal = Convert.ToDecimal(row["Fact_SubTotal"]),
                        Fact_Descuento = Convert.ToDecimal(row["Fact_Descuento"]),
                        Fact_Impuesto = Convert.ToDecimal(row["Fact_Impuesto"]),
                        Fact_IGTF = Convert.ToDecimal(row["Fact_IGTF"]),
                        Fact_MontoExento = Convert.ToDecimal(row["Fact_MontoExento"]),
                        Fact_MontoGravable = Convert.ToDecimal(row["Fact_MontoGravable"]),

                        Fact_AlicuotaIva = Convert.ToDecimal(row["Fact_AlicuotaIva"]),
                        //Fact_IGTF = Convert.ToDecimal(row["Fact_IGTF"]),
                        Fact_AlicuotaIGTF = Convert.ToDecimal(row["Fact_AlicuotaIGTF"]),
                        Fact_SerialImpresora = (string)row["Fact_SerialImpresora"],
                        Fact_Total = Convert.ToDecimal(row["Fact_Total"])
                       
                    });
                }

                return lista_Facturas;
            });
        }

        public async Task<List<ReporteGlobal_PagosDia>> ObtenerPagosDiaPorFecha(DateTime fecha)
        {
            return await Task.Run(() =>
            {
                var lista_Facturas = new List<ReporteGlobal_PagosDia>();

                DataSet ds = _D_ListaFactura.PagosDia_ReporteGlobal(fecha);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return lista_Facturas;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    lista_Facturas.Add(new ReporteGlobal_PagosDia
                    {
                        OrdenServicio = row["OrdenServicio"]?.ToString(),
                        TipoVenta = row["TipoVenta"].ToString(),
                        Fecha = Convert.ToDateTime(row["Fecha"]),
                        Cedula = row["Cedula"].ToString(),
                        NombreCliente = row["NombreCliente"].ToString(),
                        TipoPago = row["TipoPago"].ToString(),
                        Pago = Convert.ToDecimal(row["Pago"] ?? 0)
                    });
                }

                return lista_Facturas;
            });
        }

        public async Task<List<ReporteGlobal_VueltosDia>> ObtenerVueltosDiaPorFecha(DateTime fecha)
        {
            return await Task.Run(() =>
            {
                var lista_Facturas = new List<ReporteGlobal_VueltosDia>();

                DataSet ds = _D_ListaFactura.VueltosDia_ReporteGlobal(fecha);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return lista_Facturas;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    lista_Facturas.Add(new ReporteGlobal_VueltosDia
                    {
                        NumOrden = row["NumOrden"]?.ToString(),
                        //Referencia = Convert.ToInt32(row["Referencia"] ?? 0),
                        Referencia = row["Referencia"]?.ToString(),
                        BancoEmisor = row["BancoEmisor"]?.ToString(),
                        BancoReceptor = row["BancoReceptor"]?.ToString(),
                        VueltoBS = Convert.ToDecimal(row["VueltoBS"] ?? 0),
                        VueltoDivisa = Convert.ToDecimal(row["VueltoDivisa"] ?? 0)
                    });
                }
                return lista_Facturas;
            });
        }

        public async Task<List<ReporteGlobal_NotasDia>> ObtenerNotasDiaPorFecha(DateTime fecha)
        {
            return await Task.Run(() =>
            {
                var lista_Facturas = new List<ReporteGlobal_NotasDia>();

                DataSet ds = _D_ListaFactura.NotasDia_ReporteGlobal(fecha);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return lista_Facturas;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    lista_Facturas.Add(new ReporteGlobal_NotasDia
                    {
                        Numero = row["Numero"]?.ToString(),
                        NumeroControl = row["NumeroControl"]?.ToString(),
                        Cedula = row["Cedula"].ToString(),
                        NombreCliente = row["NombreCliente"].ToString(),
                        Factura = row["Factura"]?.ToString(),
                        Monto = Convert.ToDecimal(row["Monto"] ?? 0),
                        Aplicado = Convert.ToDecimal(row["Aplicado"] ?? 0),
                        Saldo = Convert.ToDecimal(row["Saldo"] ?? 0),

                        MontoExento = Convert.ToDecimal(row["MontoExento"] ?? 0),
                        MontoGravable = Convert.ToDecimal(row["MontoGravable"] ?? 0),
                        MontoIva = Convert.ToDecimal(row["MontoIva"] ?? 0),
                        AlicuotaIva = Convert.ToDecimal(row["AlicuotaIva"] ?? 0),
                        NC_SerialImpresora = (string)row["NC_SerialImpresora"]
                    });
                }
                return lista_Facturas;
            });
        }

        //Reporte Libro Ventas
        public Task<DataSet> ReporteLibroVentas(DateTimePicker desde, DateTimePicker hasta)
        {
            return Task.Run(() =>
            {
                string inicio = desde.Value.ToString("yyyyMMdd");
                string fin = hasta.Value.ToString("yyyyMMdd");
                return _D_ListaFactura.CargarLibroVentas(inicio, fin);
            });

        }


    }
}
