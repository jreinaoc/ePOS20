using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades;
using CapaLogica.ListaFactura_Logica;

namespace CapaLogica.Servicios
{
    public class CierreCaja_Servicio
    {
        private readonly L_ListaFacturas _logicaFacturas = new L_ListaFacturas();

        public async Task<List<ReporteGlobal_CierreCaja>> ObtenerCierreCajaCalculado(DateTime fecha)
        {
            var datos = await _logicaFacturas.ObtenerCierreCajaPorFecha(fecha);

            if (datos == null || !datos.Any())
                return new List<ReporteGlobal_CierreCaja>();

            // Aquí puedes agregar tus cálculos o diferencias:
            foreach (var item in datos)
            {
                item.DIFERENCIAEFECTIVO = item.MANUALEFECTIVO - item.SISTEMAEFECTIVO;
                item.DIFERENCIADEBITO = item.MANUALDEBITO - item.SISTEMADEBITO;
                item.DIFERENCIACREDITO = item.MANUALCREDITO - item.SISTEMACREDITO;
                item.DIFERENCIAGASTOS = item.MANUALGASTOS - item.SISTEMAGASTOS;
                item.DIFERENCIAIVA = item.ManualIVARetenido - item.SistemaIVARetenido;
                item.DIFERENCIAISLR = item.ManualISLRRetenido - item.SistemaISLRRetenido;
                item.DIFERENCIATRANSFERENCIA = item.MANUALTRANSFERENCIA - item.SISTEMATRANSFERENCIA;
            }

            return datos;
        }

        public async Task<List<ReporteGlobal_Facturas>> ObtenerFacturasConTotales(DateTime fecha)
        {
            var facturas = await _logicaFacturas.ObtenerFacturasPorFecha(fecha);

            if (facturas == null || !facturas.Any())
                return new List<ReporteGlobal_Facturas>();

            var totales = new ReporteGlobal_Facturas
            {
                EsTotal = true,
                Fact_Num = "Totales:",
                TotalOrden = facturas.Count,
                TotalSubtotal = facturas.Sum(f => f.Fact_SubTotal),
                TotalImpuesto = facturas.Sum(f => f.Fact_Impuesto),
                TotalIGTF = facturas.Sum(f => f.Fact_IGTF),
                Totaltotal = facturas.Sum(f => f.Fact_Total),
                Fact_MontoExento = facturas.Sum(f => f.Fact_MontoExento),
                Fact_MontoGravable = facturas.Sum(f => f.Fact_MontoExento)
            };

            facturas.Add(totales);
            return facturas;
        }

        public async Task<List<ReporteGlobal_PagosDia>> ObtenerPagosDiaConTotales(DateTime fecha)
        {
            var pagos = await _logicaFacturas.ObtenerPagosDiaPorFecha(fecha);

            if (pagos == null || !pagos.Any())
                return new List<ReporteGlobal_PagosDia>();

            var totales = new ReporteGlobal_PagosDia
            {
                EsTotal = true,
                TextoTotal = "Totales:",
                TotalOrden = pagos.Count,
                TotalPago = pagos.Sum(p => p.Pago)
            };

            pagos.Add(totales);

            return pagos;
        }

        public async Task<List<ReporteGlobal_VueltosDia>> ObtenerVueltosDiaConTotales(DateTime fecha)
        {
            var vueltos = await _logicaFacturas.ObtenerVueltosDiaPorFecha(fecha);

            if (vueltos == null || !vueltos.Any())
                return new List<ReporteGlobal_VueltosDia>();

            var totales = new ReporteGlobal_VueltosDia
            {
                EsTotal = true,
                TextoTotal = "Totales:",
                TotalOrden = vueltos.Count,
                TotalVueltoBS = vueltos.Sum(v => v.VueltoBS),
                TotalVueltoDivisa = vueltos.Sum(v => v.VueltoDivisa)
            };

            vueltos.Add(totales);

            return vueltos;
        }

        public async Task<List<ReporteGlobal_NotasDia>> ObtenerNotasDiaConTotales(DateTime fecha)
        {
            var notas = await _logicaFacturas.ObtenerNotasDiaPorFecha(fecha);

            if (notas == null || !notas.Any())
                return new List<ReporteGlobal_NotasDia>();

            var totales = new ReporteGlobal_NotasDia
            {
                EsTotal = true,
                TextoTotal = "Total Notas:",
                TotalOrden = notas.Count,
                TotalMonto = notas.Sum(n => n.Monto),
                TotalAplicado = notas.Sum(n => n.Aplicado),
                TotalSaldo = notas.Sum(n => n.Saldo),
                MontoExento = notas.Sum(n => n.MontoExento),
                MontoGravable = notas.Sum(n => n.MontoGravable),
                MontoIva = notas.Sum(n => n.MontoIva)
                
            };

            notas.Add(totales);
            return notas;
        }


    }


}

