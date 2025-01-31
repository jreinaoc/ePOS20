using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Inicio_Datos
{
    public class D_Inicio
    {
        Conexion.Conexion cn = new Conexion.Conexion();
        private string resultado;
        public string VentaCristDia;
        public string VentaMontDia;
        public string TotalBsCrist;
        public string TotalUSDCrist;
        public string TotalBsMont;
        public string TotalUSDMont;
        public int contC ;
        public int contM;
        public int contmet;
       
        public string Cod01;
        public string Cod01v2;
        public string Cod02;
        public string Cod03;
        public string Cod04;
        public string Cod05;
        public string Cod06;
        public string Cod07;
        public string Cod08;

        public string DEFAULT;

        public bool SinTasaDol =  false;
        public bool SinTasaEur = false;
        public bool SinTasa = false;


        //TB_CAORDSER CAORDSER = new TB_CAORDSER();
        List<List_TB_CAORDSER> CAORDSER = new List<List_TB_CAORDSER>();
       public  ArrayList CantidadC = new ArrayList();
       public ArrayList  NombreC = new ArrayList();
       public ArrayList CantidadM = new ArrayList();
       public ArrayList NombreM = new ArrayList();
       public ArrayList CantidadLeyC = new ArrayList();
       public ArrayList NombreLeyC = new ArrayList();
        public ArrayList CantidadLeyM = new ArrayList();
        public ArrayList TotalLeyM = new ArrayList();
        public ArrayList DatosMetUds = new ArrayList();
        public ArrayList LeyMetUds = new ArrayList();
        public ArrayList DatosMetBs = new ArrayList();
        public ArrayList LeyMetBs = new ArrayList();
        public ArrayList CantMetIng = new ArrayList();
        public ArrayList LeyMetIng = new ArrayList();
        public ArrayList CantMetUds = new ArrayList();
        public ArrayList LeyendaMetUds = new ArrayList();




        public string VentaDia(string Fecha)
        {
            SqlCommand cmd = new SqlCommand("SP_CPOS_pGet_VentaDiaBS", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Fecha", Fecha);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            resultado = dt.Rows[0]["VentaDia"].ToString();
            return (resultado);
        }
        
        public string VentaDiaDol(string Fecha)
        {
            SqlCommand cmd = new SqlCommand("pGet_VentaTDia", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Fecha", Fecha);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            resultado = dt.Rows[0]["VentaDia"].ToString();
            return (resultado);

        }




        public void GraficoVentas(string fecha, string CodEmpleado)
        {
            int cont;
            SqlCommand cmd = new SqlCommand("SP_CPOS_VENTACRIST_PORDIA", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FECHA", fecha);
            cmd.Parameters.AddWithValue("@EMPLEADO", CodEmpleado);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
       

            VentaCristDia = dt.Rows[0]["VENTACRSITDIA"].ToString();
            VentaMontDia = dt.Rows[0]["VENTAMONTDIA"].ToString();
            TotalBsCrist = dt.Rows[0]["TOTALBSCRIST"].ToString();
            TotalUSDCrist = dt.Rows[0]["TOTALUSDCRIST"].ToString();
            TotalBsMont = dt.Rows[0]["TOTALBSMONT"].ToString();
            TotalUSDMont = dt.Rows[0]["TOTALUSDMONT"].ToString();
        }

        //Stored para obtener la leyenda de la venta de cristales 
        public void leyendaCrist(string fecha, string CodEmpleado, string CodSucursal = "")
        {
            contC = 0;
            string CodSuc = Sucursal();
            SqlCommand cmd = new SqlCommand("SP_CPOS_VENTACR_NOMBRE_CANT", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FECHA", fecha);
            cmd.Parameters.AddWithValue("@EMPLEADO", CodEmpleado);
            cmd.Parameters.AddWithValue("@SUCURSAL", CodSuc);
            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                CantidadLeyC.Add(dr.GetValue(0));
                NombreLeyC.Add(dr.GetString(1));
                contC = contC + 1; 
                

            }

          
        }

        //Stored para obtrener la leyenda de la venta de cristales por rango de fecha 
        public void LeyendCristRango(string fechadesde, string fechahasta, string CodEmpleado, string CodSucursal = "")
        {
            //cont = 0;
            string CodSuc = Sucursal();
            SqlCommand cmd = new SqlCommand("SP_CPOS_VENTACRIST_NOMBRE_CANT_RANGO", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FECHADESDE", fechadesde);
            cmd.Parameters.AddWithValue("@FECHAHASTA", fechahasta);
            cmd.Parameters.AddWithValue("@EMPLEADO", CodEmpleado);
            cmd.Parameters.AddWithValue("@SUCURSAL", CodSuc);

            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                CantidadLeyC.Add(dr.GetValue(0));
                NombreLeyC.Add(dr.GetString(1));
                contC = contC + 1;


            }
        }



        // Stored para ontener la leyenda de la venta de monturas
        public void leyendaMont(string fecha, string CodEmpleado, string CodSucursal="")
        {
            contM = 0;
            string CodSuc = Sucursal();
            SqlCommand cmd = new SqlCommand("SP_CPOS_VENTAMONT_NOMBRE_CANT", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FECHA", fecha);
            cmd.Parameters.AddWithValue("@EMPLEADO", CodEmpleado);
            cmd.Parameters.AddWithValue("@SUCURSAL", CodSuc);
            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                CantidadLeyM.Add(dr.GetValue(0));
                TotalLeyM.Add(dr.GetString(1));
                contM = contM + 1;


            }





        }

        //Stored para obtener la leyenda de la monturas por rango de fecha 
        public void LeyendMontRango(string fechadesde, string fechahasta, string CodEmpleado, string CodSucursal="")
        {
            //cont = 0;
            string CodSuc = Sucursal();
            SqlCommand cmd = new SqlCommand("SP_CPOS_VENTAMONT_NOMBRE_CANT_RANGO", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FECHADESDE", fechadesde);
            cmd.Parameters.AddWithValue("@FECHAHASTA", fechahasta);
            cmd.Parameters.AddWithValue("@EMPLEADO", CodEmpleado);
            cmd.Parameters.AddWithValue("@SUCURSAL", CodSuc);

            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                CantidadLeyM.Add(dr.GetValue(0));
                TotalLeyM.Add(dr.GetString(1));
                contM = contM + 1;



            }
        }


        //Stored que armaa el grafico de las ventas de cristales 
        public void GraficoVentasDet(string fecha, string CodEmpleado, string CodSucursal= "")
        {
            //cont = 0;
            string CodSuc = Sucursal();

            SqlCommand cmd = new SqlCommand("SP_CPOS_VENTACM_DETALLADA", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FECHA", fecha);
            cmd.Parameters.AddWithValue("@EMPLEADO", CodEmpleado);
            cmd.Parameters.AddWithValue("@SUCURSAL", CodSuc);

            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                CantidadC.Add(dr.GetValue(0));
                NombreC.Add(dr.GetString(1));
                //cont = cont + 1;


            }

            }
            
        // Stored que arma el grafico de las ventas de cristales por rango
        public void GraficoVentasDetRango(string fechadesde, string fechahasta, string CodEmpleado, string CodSucursal = "")
        {
            //cont = 0;
            string CodSuc = Sucursal();
            SqlCommand cmd = new SqlCommand("SP_CPOS_VENTACRIST_DETALLADA_RANGO", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FECHADESDE", fechadesde);
            cmd.Parameters.AddWithValue("@FECHAHASTA", fechahasta);
            cmd.Parameters.AddWithValue("@EMPLEADO", CodEmpleado);
            cmd.Parameters.AddWithValue("@SUCURSAL", CodSuc);

            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                CantidadC.Add(dr.GetValue(0));
                NombreC.Add(dr.GetString(1));
                //cont = cont + 1;


            }

        }
        //Stored que arma el grafico de las metas por Unidades
            public void GraficoMetasUds(string Periodo, string CodEmpleado, string Meta)
        {
              string CodSuc = Sucursal();
            SqlCommand cmd = new SqlCommand("SP_CPOS_GRAF_METAS_UNID", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PERIODO", Periodo);
            cmd.Parameters.AddWithValue("@EMPLEADO", CodEmpleado);
            cmd.Parameters.AddWithValue("@SUCURSAL", CodSuc);
            cmd.Parameters.AddWithValue("@META", Meta);

            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int a= Convert.ToInt32(dr.GetValue(0));
                DatosMetUds.Add(a);
                LeyMetUds.Add(dr.GetString(1));

            }

        }

        //Stored que ontiene la leyenda del grafico de las metas por unidades
        public void LeyendaGraficoMetaUds(string Periodo, string CodEmpleado, string Meta)
        {
            string CodSuc = Sucursal();
            contmet = 0;
            SqlCommand cmd = new SqlCommand("SP_CPOS_GRAF_METAS_UNID_LEYE", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PERIODO", Periodo);
            cmd.Parameters.AddWithValue("@EMPLEADO", CodEmpleado);
            cmd.Parameters.AddWithValue("@SUCURSAL", CodSuc);
            cmd.Parameters.AddWithValue("@META", Meta);

            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int a = Convert.ToInt32(dr.GetValue(0));
                CantMetUds.Add(a);
                LeyendaMetUds.Add(dr.GetString(1));
                contmet = contmet + 1;

            }

        }

        // Stored que arma el grafico de las metas por ingresos
        public void GraficoMetasIng(string Periodo, string CodEmpleado, string Meta)
        {
            string CodSuc = Sucursal();
            SqlCommand cmd = new SqlCommand("SP_CPOS_GRAF_METAS_BS", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PERIODO", Periodo);
            cmd.Parameters.AddWithValue("@EMPLEADO", CodEmpleado);
            cmd.Parameters.AddWithValue("@SUCURSAL", CodSuc);
            cmd.Parameters.AddWithValue("@META", Meta);

            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int a = Convert.ToInt32(dr.GetValue(0));
                DatosMetBs.Add(a);
                LeyMetBs.Add(dr.GetString(1));
            }

        }
        //Storefd que obtiene la leyenda de los graficos de metas por ingresos
        public void LeyendaGraficoMetaIng(string Periodo, string CodEmpleado, string Meta)
        {
            string CodSuc = Sucursal();
            contmet = 0;
            SqlCommand cmd = new SqlCommand("SP_CPOS_GRAF_METAS_BS_LEYE", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PERIODO", Periodo);
            cmd.Parameters.AddWithValue("@EMPLEADO", CodEmpleado);
            cmd.Parameters.AddWithValue("@SUCURSAL", CodSuc);
            cmd.Parameters.AddWithValue("@META", Meta);


            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int a = Convert.ToInt32(dr.GetValue(0));
                CantMetIng.Add(a);
                LeyMetIng.Add(dr.GetString(1));
                contmet = contmet + 1;
            }

        }
        // Stored que arma el grafico de ventas de montura
        public void GraficoVentasMontDet(string fecha, string CodEmpleado, string CodSucursal="")
        {
            string CodSuc = Sucursal();
            SqlCommand cmd = new SqlCommand("SP_CPOS_VENTAM_DETALLADA", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FECHA", fecha);
            cmd.Parameters.AddWithValue("@EMPLEADO", CodEmpleado);
            cmd.Parameters.AddWithValue("@SUCURSAL", CodSuc);
            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                CantidadM.Add(dr.GetValue(0));
                NombreM.Add(dr.GetString(1));

            }
        }


        // Stored que arma el grafico de venta de monturas por rango de fecha
        public void GraficoVentasMontDetRango(string fechadesde, string fechahasta, string CodEmpleado, string CodSucursal = "")
        {
            string CodSuc = Sucursal();
            SqlCommand cmd = new SqlCommand("SP_CPOS_VENTAM_DETALLADA_RANGO", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FECHADESDE", fechadesde);
            cmd.Parameters.AddWithValue("@FECHAHASTA", fechahasta);
            cmd.Parameters.AddWithValue("@EMPLEADO", CodEmpleado);
            cmd.Parameters.AddWithValue("@SUCURSAL", CodSuc);
            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                CantidadM.Add(dr.GetValue(0));
                NombreM.Add(dr.GetString(1));

            }
        }


        public string TasaDivisa(string FechaDiaActivo, string CodMoneda)
        {
            string Cod_Sucursal = Sucursal();
            string Tasa;

            SqlCommand cmd = new SqlCommand("SP_CPOS_pGetTasaDia", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodSucursal", Cod_Sucursal);
            cmd.Parameters.AddWithValue("@FechaDiaActivo", FechaDiaActivo);
            cmd.Parameters.AddWithValue("@CodMoneda", CodMoneda);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            
           Tasa = dt.Rows[0]["Tasa"].ToString();
                
            
            return Tasa;

        }



        public void TasaDiaDolarEntidad(string fechaDiaActivo)
        {
            string Cod_Sucursal = Sucursal();

            //SqlCommand cmd = new SqlCommand(" SELECT * FROM TB_TASA WHERE CONVERT(VARCHAR(10), FecCreacion, 103) = CONVERT(VARCHAR(10), @fechaDiaActivo, 103) AND Cod_Moneda = '01'", cn.LeerCadena());
            SqlCommand cmd = new SqlCommand(" SELECT top(1)*FROM TB_TASA where Cod_Moneda = '01' and @fechaDiaActivo = FecCreacion order by FecCreacion DESC", cn.LeerCadena());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Cod_Sucursal", Cod_Sucursal);
            cmd.Parameters.AddWithValue("@fechaDiaActivo", fechaDiaActivo);
            SqlDataReader dataReader = cmd.ExecuteReader();

            if (dataReader.HasRows)
            {

                while (dataReader.Read())
                {
                    TB_TASA_Dolar.ID_Tasa = Convert.ToInt32(dataReader["ID_Tasa"]);
                    TB_TASA_Dolar.Cod_Sucursal = Convert.ToString(dataReader["Cod_Sucursal"]);
                    TB_TASA_Dolar.Tasa = dataReader["Tasa"] == DBNull.Value ? (Double?)0.00 : Convert.ToDouble(dataReader["Tasa"]);
                    TB_TASA_Dolar.Cod_Moneda = Convert.ToString(dataReader["Cod_Moneda"]);
                    TB_TASA_Dolar.FecCreacion = dataReader["FecCreacion"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["FecCreacion"]);
                    TB_TASA_Dolar.USER_Crea = Convert.ToString(dataReader["USER_Crea"]);
                }
            }
            else
            {
                SinTasaDol = true;
            }

        }


        public void TasaDiaEuroEntidad(string fechaDiaActivo)
        {
            //SqlCommand cmd = new SqlCommand(" SELECT * FROM TB_TASA WHERE CONVERT(VARCHAR(10), FecCreacion, 103) = CONVERT(VARCHAR(10), @fechaDiaActivo, 103) AND Cod_Moneda = '02'", cn.LeerCadena());
            SqlCommand cmd = new SqlCommand(" SELECT top(1)*FROM TB_TASA where Cod_Moneda = '02' and @fechaDiaActivo = FecCreacion order by FecCreacion DESC", cn.LeerCadena());
            cmd.CommandType = CommandType.Text;

            //cmd.Parameters.AddWithValue("@Cod_Sucursal", Cod_Sucursal);
            cmd.Parameters.AddWithValue("@fechaDiaActivo", fechaDiaActivo);
            SqlDataReader dataReader = cmd.ExecuteReader();

            if (dataReader.HasRows)
            {

                while (dataReader.Read())
                {
                    TB_TASA_Euro.ID_Tasa = Convert.ToInt32(dataReader["ID_Tasa"]);
                    TB_TASA_Euro.Cod_Sucursal = Convert.ToString(dataReader["Cod_Sucursal"]);
                    TB_TASA_Euro.Tasa = dataReader["Tasa"] == DBNull.Value ? (Double?)0.00 : Convert.ToDouble(dataReader["Tasa"]);
                    TB_TASA_Euro.Cod_Moneda = Convert.ToString(dataReader["Cod_Moneda"]);
                    TB_TASA_Euro.FecCreacion = dataReader["FecCreacion"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["FecCreacion"]);
                    TB_TASA_Euro.USER_Crea = Convert.ToString(dataReader["USER_Crea"]);
                }
            }
            else
            {
                SinTasaEur = true; 
            }

        }

        public string Sucursal()
        {
            SqlDataAdapter da = new SqlDataAdapter("select Valor from TB_PARAMETRO where Parametro= 'Sucursal'", cn.LeerCadena());
            da.SelectCommand.CommandType = CommandType.Text;
            DataTable dt = new DataTable();
            da.Fill(dt);
            string SucursalActual = dt.Rows[0]["Valor"].ToString();
            return SucursalActual;

        }


        public DateTime DiaActivo()
        {
            SqlDataAdapter da = new SqlDataAdapter("select top (1) FECHA from TB_CAJA ORDER BY FECHA DESC", cn.LeerCadena());
            da.SelectCommand.CommandType = CommandType.Text;
            DataTable dt = new DataTable();
            da.Fill(dt);
            DateTime FechaActiva = Convert.ToDateTime(dt.Rows[0]["FECHA"].ToString());
            return FechaActiva;

        }


        public List<List_TB_CAORDSER> BuscarOrdenes(string Dias)
        {
            try
            {


                SqlCommand cmd = new SqlCommand("SP_CPOS_BuscarOrdenes", cn.LeerCadena());

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Dias", Dias);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {

                    while (dataReader.Read())
                    {
                        CAORDSER.Add(new List_TB_CAORDSER
                        {
                            Cod_Sucursal = Convert.ToString(dataReader["Cod_Sucursal"]),
                            NumOrdserv = Convert.ToString(dataReader["NumOrdserv"]),
                            Revision = Convert.ToString(dataReader["Revision"]),
                            Fecha = Convert.ToDateTime(dataReader["Fecha"].ToString()),
                            Cod_Venta = Convert.ToString(dataReader["Cod_Sucursal"]),
                            CTE_Nacio = Convert.ToString(dataReader["Cod_Sucursal"]),
                            CTE_CedIden = Convert.ToString(dataReader["CTE_CedIden"]),
                            NumExamen = dataReader["NumExamen"] == DBNull.Value ? (Int32?)0.00 : Convert.ToInt32(dataReader["NumExamen"]),
                            COD_EMPLEADO = Convert.ToString(dataReader["COD_EMPLEADO"]),
                            Cod_Laboratorio = Convert.ToString(dataReader["Cod_Laboratorio"]),
                            Cod_Servicio = Convert.ToString(dataReader["Cod_Servicio"]),
                            Vision = Convert.ToString(dataReader["Vision"]),
                            Fec_ofrecido = Convert.ToDateTime(dataReader["Fec_ofrecido"].ToString()),
                            Hor_ofrecido = Convert.ToString(dataReader["Hor_ofrecido"]),
                            Fec_Entrega = dataReader["Fec_Entrega"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["Fec_Entrega"]),
                            Fec_Envio = dataReader["Fec_Envio"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["Fec_Envio"]),
                            Fec_Recibido = dataReader["Fec_Recibido"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["Fec_Recibido"]),
                            VtaSubTotal = Convert.ToDouble(dataReader["VtaSubTotal"]),
                            VtaImpuesto = Convert.ToDouble(dataReader["VtaImpuesto"]),
                            VtaDescuento = Convert.ToDouble(dataReader["VtaDescuento"]),
                            VtaTotal = Convert.ToDouble(dataReader["VtaTotal"]),
                            OrSer_Saldo = Convert.ToDouble(dataReader["OrSer_Saldo"]),
                            OrSer_Finan = Convert.ToBoolean(dataReader["OrSer_Finan"]),
                            OrSer_Status = Convert.ToString(dataReader["OrSer_Status"]),
                            OrSer_Observ = Convert.ToString(dataReader["OrSer_Observ"]),
                            CodCausa = Convert.ToString(dataReader["CodCausa"]),
                            MonturaPropia = dataReader["MonturaPropia"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dataReader["MonturaPropia"]),
                            OrSer_fecCrea = Convert.ToDateTime(dataReader["OrSer_fecCrea"].ToString()),
                            OrSer_FecMod = dataReader["OrSer_FecMod"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["OrSer_FecMod"]),
                            USER_Crea = Convert.ToString(dataReader["USER_Crea"]),
                            USER_Mod = Convert.ToString(dataReader["USER_Mod"]),
                            Cod_DetVta = Convert.ToString(dataReader["Cod_DetVta"]),
                            Aplica = dataReader["Aplica"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dataReader["Aplica"]),
                            OTCORRESPONDIENTE = Convert.ToString(dataReader["OTCORRESPONDIENTE"]),
                            Anulado = dataReader["Anulado"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dataReader["Anulado"]),
                            Ventaafil = dataReader["Ventaafil"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dataReader["Ventaafil"]),
                            cristalpropio = Convert.ToBoolean(dataReader["cristalpropio"]),
                            Nota = dataReader["Nota"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dataReader["Nota"]),
                            Cuantas = dataReader["Cuantas"] == DBNull.Value ? (Int32?)0.00 : Convert.ToInt32(dataReader["Cuantas"]),
                            CodMotivoAnul = Convert.ToString(dataReader["CodMotivoAnul"]),
                            FechaAnulacion = dataReader["FechaAnulacion"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["FechaAnulacion"]),
                            FechaCaja = dataReader["FechaCaja"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataReader["FechaCaja"]),
                            Cod_ResponsableRev = Convert.ToString(dataReader["Cod_ResponsableRev"]),
                            Cod_ResponsableAnu = Convert.ToString(dataReader["Cod_ResponsableAnu"]),
                            Asegurada = dataReader["Asegurada"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dataReader["Asegurada"]),
                            Exonerada = dataReader["Exonerada"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dataReader["Exonerada"]),
                            TipoMonturaPropia = Convert.ToString(dataReader["TipoMonturaPropia"]),
                            CodMotivoReposicion = Convert.ToString(dataReader["CodMotivoReposicion"]),
                            Cedula_CteAfil = Convert.ToString(dataReader["Cedula_CteAfil"]),
                            Codigo_EmpAfil = Convert.ToString(dataReader["Codigo_EmpAfil"]),
                            MonturaEnQuorum = dataReader["MonturaEnQuorum"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dataReader["MonturaEnQuorum"]),
                            Cod_Coloracion = Convert.ToString(dataReader["Cod_Coloracion"]),
                            OS_Externa = Convert.ToString(dataReader["OS_Externa"]),
                            OrSer_Saldo_Mon = dataReader["OrSer_Saldo_Mon"] == DBNull.Value ? (Double?)0.00 : Convert.ToDouble(dataReader["OrSer_Saldo_Mon"]),
                            OrSer_Tipo_Mon = Convert.ToString(dataReader["OrSer_Tipo_Mon"]),
                            Orser_Total_Mon = dataReader["Orser_Total_Mon"] == DBNull.Value ? (Double?)0.00 : Convert.ToDouble(dataReader["Orser_Total_Mon"]),
                            VtaImpuestoIGTF = dataReader["VtaImpuestoIGTF"] == DBNull.Value ? (Double?)0.00 : Convert.ToDouble(dataReader["VtaImpuestoIGTF"])
                        });

                    }
                    return CAORDSER;
                }
                return null;
            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return null;
            }
        }

        public DataTable GridListaEspera()
        {
            SqlCommand cmd = new SqlCommand("SP_CPOS_ClienteEspera", cn.LeerCadena());

            cmd.CommandType = CommandType.StoredProcedure;

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        public DataTable GridOrdenes(string Fecha)
        {
            SqlCommand cmd = new SqlCommand("SP_CPOS_CargarUltimasVentas", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Fecha", Fecha);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }
        public bool DeleteListaEspera(string contenido)
        {
            try
            {
            SqlCommand cmd = new SqlCommand("SP_CPOS_DeleteClienteESspera", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@contenido", contenido);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return true;
            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return false;
            }
        }

        public void TraerConfiguracion(string CodEmpleado)
        {
            SqlCommand cmd = new SqlCommand("SP_CPOS_CargarConfiguracion", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EMPLEADO", CodEmpleado);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                DEFAULT = "Default";

            }
            else
            {
                Cod01 = dt.Rows[0]["Valor1"].ToString();
                Cod01v2 = dt.Rows[0]["Valor2"].ToString();
                Cod02 = dt.Rows[1]["Valor1"].ToString();
                Cod03 = dt.Rows[2]["Valor1"].ToString();
                Cod04 = dt.Rows[3]["Valor1"].ToString();
                Cod05 = dt.Rows[4]["Valor1"].ToString();
                Cod06 = dt.Rows[5]["Valor1"].ToString();
                Cod07 = dt.Rows[6]["Valor1"].ToString();
                Cod08 = dt.Rows[7]["Valor1"].ToString();
            }


        }

        public DataTable EnviarCliente(string Cedula, string Nombre, string Apellido)
        {

            SqlCommand cmd = new SqlCommand("SP_CPOS_POST_LISTA_ESPERA", cn.LeerCadena());

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Cedula", Cedula);
            cmd.Parameters.AddWithValue("@Nombre", Nombre);
            cmd.Parameters.AddWithValue("@Apellido", Apellido);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return (dt);
        }

         public string Encriptar(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public string DesEncriptar(string _cadenaADesencriptar)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(_cadenaADesencriptar);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }


    }



}

