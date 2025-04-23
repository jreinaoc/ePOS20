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

        public List<ReporteGlobal_CierreCaja> ObtenerCierreCajaCalculado(DateTime fecha)
        {
            var datos = _logicaFacturas.ObtenerCierreCajaPorFecha(fecha);

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
    }
}
