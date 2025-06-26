using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaDatos.CierreCaja_Datos;

namespace CapaLogica.CierreCaja_Logica
{
    public class L_CierreCaja
    {
        
        private D_CierreCaja _D_CierreCaja = new D_CierreCaja();
        public bool ChequeaFacturasdelDia(string fecha, string usuario)
        {
            DataTable dt = _D_CierreCaja.ChequeaFacturasdelDia(fecha,usuario);

            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ORDSERVCRITERIOSVARIOS(string bandera, string condicion)
        {
            DataTable dt = _D_CierreCaja.ORDSERVCRITERIOSVARIOS(bandera, condicion);

            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CierreFueradeHorario(string codsuc, DateTime fechaIni, DateTime fechaFin)
        {
            DataTable dt = _D_CierreCaja.CierreFueradeHorario(codsuc, fechaIni,fechaFin);

            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataTable CierrePuntodeVenta(string codsuc, string codBanco, string nroLote, DateTime fecha)
        {
            DataTable dt = _D_CierreCaja.CierrePuntodeVenta(codsuc, codBanco, nroLote, fecha);

            //if (dt.Rows.Count > 0)
            //{
            //    return dt;
            //}
            return dt;
            //else
            //{
            //    return false;
            //}
        }

        public DataTable ObtienePuntosdeVenta(string codPunto)
        {
            DataTable dt = _D_CierreCaja.ObtienePuntosdeVenta(codPunto);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return dt;
            }
        }

        public DataTable AgregaPuntosdeVenta( string codBanco, DateTime fecha, string nroLote, decimal manualTarjCredito, decimal manualTarjCreditoAmex,
    decimal manualTarjDebito, decimal manualTarjOtros)
        {
            DataTable dt = _D_CierreCaja.AgregaPuntosdeVenta(  codBanco, fecha, nroLote, manualTarjCredito, manualTarjCreditoAmex,
    manualTarjDebito, manualTarjOtros);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return dt;
            }
        }

        public DataTable ObtineneCambioCierre(DateTime fechaIni, string codsuc)
        {
            DataTable dt = _D_CierreCaja.ObtineneCambioCierre(fechaIni, codsuc);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return dt;
            }
        }
        public DataTable ObtieneBancosPagoMovil(string codsuc)
        {
            DataTable dt = _D_CierreCaja.ObtieneBancosPagoMovil(codsuc);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return dt;
            }
        }

        public DataTable AgregaReferenciaPagoMovil(string codSuc, string nroOs, string referencia, string bancoEmisor)
        {
            DataTable dt = _D_CierreCaja.AgregaReferenciaPagoMovil(codSuc, nroOs, referencia, bancoEmisor);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return dt;
            }
        }

        public DataTable VerificaAsistenciaPendiente(string fecha, string tipoAsis)
        {
            DataTable dt = _D_CierreCaja.VerificaAsistenciaPendiente(fecha, "PEND");

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return dt;
            }
        }

        public DataTable ActualizaAsistencia(string fecha, string hora, string codEmp, string usuario)
        {
            DataTable dt = _D_CierreCaja.ActualizaAsistencia(fecha, hora, codEmp, usuario);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return dt;
            }
        }
    }
}
