using CapaDatos.Inicio_Datos;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CapaDatos.ListaFacturas_Datos
{
    public class D_ListaFactura
    {
        Conexion.Conexion cn = new Conexion.Conexion();

        public async Task<DataSet> CargarFacturas(string Fecha_inicio = "", string Fecha_fin = "", int Inicio = 1, int Final = 12)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = cn.LeerCadena();
                cmd.CommandType = CommandType.StoredProcedure;

                StringBuilder query = new StringBuilder("SP_CPOS_BuscarFacturasListFac");

                cmd.Parameters.AddWithValue("@Fecha_inicio", Fecha_inicio);
                cmd.Parameters.AddWithValue("@Fecha_fin", Fecha_fin);
                cmd.Parameters.AddWithValue("@Inicio", Inicio);
                cmd.Parameters.AddWithValue("@Final", Final);
                cmd.CommandText = query.ToString();

                var dts = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                await Task.Run(() => da.Fill(dts));

                return dts;
            }

        }

        public async Task<DataSet> CargarNotas(string Fecha_inicio = "", string Fecha_fin = "", int Inicio = 1, int Final = 12)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = cn.LeerCadena();
                cmd.CommandType = CommandType.StoredProcedure;

                StringBuilder query = new StringBuilder("SP_CPOS_BuscarFacturasListNC");

                cmd.Parameters.AddWithValue("@Fecha_inicio", Fecha_inicio);
                cmd.Parameters.AddWithValue("@Fecha_fin", Fecha_fin);
                cmd.Parameters.AddWithValue("@Inicio", Inicio);
                cmd.Parameters.AddWithValue("@Final", Final);
                cmd.CommandText = query.ToString();

                var dts = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                await Task.Run(() => da.Fill(dts));

                return dts;
            }

        }

        public DataSet CierreCaja_ReporteGlobal(DateTime fecha)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = cn.LeerCadena();
                cmd.CommandType = CommandType.Text;

                StringBuilder query = new StringBuilder(@"
                   SELECT 
                        ISNULL(MANUALEFECTIVO, 0) AS MANUALEFECTIVO, 
                        ISNULL(SISTEMAEFECTIVO, 0) AS SISTEMAEFECTIVO, 
                        ISNULL(MANUALDEBITO, 0) AS MANUALDEBITO, 
                        ISNULL(SISTEMADEBITO, 0) AS SISTEMADEBITO, 
                        ISNULL(MANUALCREDITO, 0) AS MANUALCREDITO,
                        ISNULL(SISTEMACREDITO, 0) AS SISTEMACREDITO, 
                        ISNULL(MANUALGASTOS, 0) AS MANUALGASTOS, 
                        ISNULL(SISTEMAGASTOS, 0) AS SISTEMAGASTOS, 
                        ISNULL(ManualIVARetenido, 0) AS ManualIVARetenido, 
                        ISNULL(SistemaIVARetenido, 0) AS SistemaIVARetenido,
                        ISNULL(ManualISLRRetenido, 0) AS ManualISLRRetenido, 
                        ISNULL(SistemaISLRRetenido, 0) AS SistemaISLRRetenido, 
                        ISNULL(MANUALTRANSFERENCIA, 0) AS MANUALTRANSFERENCIA, 
                        ISNULL(SISTEMATRANSFERENCIA, 0) AS SISTEMATRANSFERENCIA, 
                        ISNULL(MANUALVUELTO, 0) AS MANUALVUELTO, 
                        ISNULL(SISTEMAVUELTO, 0) AS SISTEMAVUELTO, 
                        ISNULL(MANUALTOTALINGRESOS, 0) AS MANUALTOTALINGRESOS, 
                        ISNULL(SISTEMATOTALINGRESOS, 0) AS SISTEMATOTALINGRESOS, 
                        ISNULL(MANUALREINTEGROS, 0) AS MANUALREINTEGROS, 
                        ISNULL(SISTEMAREINTEGRO, 0) AS SISTEMAREINTEGRO,
                        ISNULL(SISTEMANOTACREDITO, 0) AS SISTEMANOTACREDITO, 
                        ISNULL(MANUALNOTACREDITO, 0) AS MANUALNOTACREDITO, 
                        ISNULL(TOTAL_NETO, 0) AS TOTAL_NETO, 
                        ISNULL(IMPUESTO, 0) AS IMPUESTO, 
                        ISNULL(TOTAL_BRUTO, 0) AS TOTAL_BRUTO
                    FROM TB_CAJA 
                    WHERE CAST(Fecha AS DATE) = @Fecha
                ");

                cmd.Parameters.AddWithValue("@Fecha", fecha.Date);
                cmd.CommandText = query.ToString();

                DataSet dts = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dts);
                return dts;
            }
        }

        public DataSet Facturas_ReporteGlobal(DateTime fecha)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = cn.LeerCadena();
                cmd.CommandType = CommandType.Text;

                StringBuilder query = new StringBuilder(@"
                  SELECT 
                       ISNULL(fc.Cod_Sucursal, '') AS Cod_Sucursal, 
                       ISNULL(fc.Fact_Num, '') AS Fact_Num, 
                       ISNULL(fc.NumOrdServ, '') AS NumOrdServ, 
                       ISNULL(fc.CTE_NacioPAG + '-' + fc.CTE_CedIdenPAG, 0) AS CTE_CedIdenPAG, 
					   ISNULL(cl.CTE_PNombre + ' ' + cl.CTE_PApellido, 0) AS CTE_PNombre,
                       ISNULL(fc.Fact_SubTotal, 0) AS Fact_SubTotal, 
                       ISNULL(fc.Fact_Descuento, 0) AS Fact_Descuento, 
                       ISNULL(fc.Fact_Impuesto, 0) AS Fact_Impuesto, 
                       ISNULL(fc.Fact_IGTF, 0) AS Fact_IGTF, 
                       ISNULL(fc.Fact_Total, 0) AS Fact_Total
                   FROM TB_FACTURAS fc
				   INNER JOIN TB_CTEPPAL cl on fc.CTE_CedIdenPAG = cl.CTE_CedIden
                   WHERE CAST(Fecha AS DATE) = @Fecha
               ");

                cmd.Parameters.AddWithValue("@Fecha", fecha.Date);
                cmd.CommandText = query.ToString();

                DataSet dts = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dts);
                return dts;
            }
        }

        public DataSet PagosDia_ReporteGlobal(DateTime fecha)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = cn.LeerCadena();
                cmd.CommandType = CommandType.StoredProcedure;

                StringBuilder query = new StringBuilder("SP_CPOS_ReporteGlobal_PagosDelDia");

                cmd.Parameters.AddWithValue("@Fecha", fecha.Date);
                cmd.CommandText = query.ToString();

                DataSet dts = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dts);
                return dts;
            }
        }

        public DataSet VueltosDia_ReporteGlobal(DateTime fecha)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = cn.LeerCadena();
                cmd.CommandType = CommandType.StoredProcedure;

                StringBuilder query = new StringBuilder("SP_CPOS_ReporteGlobal_VueltosDelDia");

                cmd.Parameters.AddWithValue("@Fecha", fecha.Date);
                cmd.CommandText = query.ToString();

                DataSet dts = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dts);
                return dts;
            }
        }

        public DataSet NotasDia_ReporteGlobal(DateTime fecha)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = cn.LeerCadena();
                cmd.CommandType = CommandType.Text;

                StringBuilder query = new StringBuilder(@"
                SELECT 
                    ISNULL(NT.NRONOTA, '') AS Numero, 
                    ISNULL(NT.NROCONTROL, '') AS NumeroControl, 
                    ISNULL(NT.CTE_Nacio + '-' + NT.CTE_CedIden, '') AS Cedula, 
                    ISNULL(CL.CTE_PNombre + ' ' + CL.CTE_PApellido,'') AS NombreCliente,
                    ISNULL(NT.Fact_Num, '') AS Factura, 
                    ISNULL(NT.MontoNota, 0) AS Monto, 
                    ISNULL(NT.MontoAplicado, 0) AS Aplicado, 
                    ISNULL(NT.SaldoNota, 0) AS Saldo

                FROM TB_NOTASCREDITODEBITO NT

                LEFT JOIN [dbo].[TB_CTEPPAL] CL
	            ON NT.CTE_CedIden = CL.CTE_CedIden

                WHERE CAST(Fecha AS DATE) = @Fecha
                ");

                cmd.Parameters.AddWithValue("@Fecha", fecha.Date);
                cmd.CommandText = query.ToString();

                DataSet dts = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dts);
                return dts;
            }
        }


        //Reporte Libro Ventas
        public DataSet CargarLibroVentas(string Fecha_inicio = "", string Fecha_fin = "")
        {
            SqlCommand cmd = new SqlCommand("SP_CPOS_LibroVentas_Datos", cn.LeerCadena());

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Fecha_inicio", Fecha_inicio);
            cmd.Parameters.AddWithValue("@Fecha_fin", Fecha_fin);

            DataSet dts = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dts);
            return (dts);

        }



    }



}



