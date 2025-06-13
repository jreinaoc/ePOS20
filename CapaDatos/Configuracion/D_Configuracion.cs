using System;
using CapaEntidades;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos.Inicio_Datos;


namespace CapaDatos.Configuracion
{
    public class D_Configuracion
    {
        Conexion.Conexion cn = new Conexion.Conexion();

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
        public string CamposMetas;
        public string Ing;
        public string Unds;


        public DataTable GetConfiguracion(string CodEmpleado, string CodTipo, string Valor1, string Valor2)
        {
            SqlCommand cmd = new SqlCommand("SP_CPOS_GET_CONFIGURACION", cn.LeerCadena());

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@EMPLEADO", CodEmpleado);
            cmd.Parameters.AddWithValue("@CodTipo", CodTipo);
            cmd.Parameters.AddWithValue("@Valor1", Valor1);
            cmd.Parameters.AddWithValue("@Valor2", Valor2);


            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return (dt);

        }


        public DataTable PostHistMetas(string CodEmpleado, string Periodo, string MetaUds, string MetaIng, string ParamLimOSAbo)
        {
            SqlCommand cmd = new SqlCommand("SP_CPOS_POST_HIST_METAS", cn.LeerCadena());

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CodEmpleado", CodEmpleado);
            cmd.Parameters.AddWithValue("@Periodo", Periodo);
            cmd.Parameters.AddWithValue("@MetaUds", MetaUds);
            cmd.Parameters.AddWithValue("@MetaIngresos", MetaIng);
            cmd.Parameters.AddWithValue("@ParamLimOSAbo", ParamLimOSAbo);


            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return (dt);

        }


        public DataTable TraerColaboradores()
        {
            string CodSuc = Sucursal();

            SqlCommand cmd = new SqlCommand("SP_CPOS_COLABORADOR", cn.LeerCadena());

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CodSucursal", CodSuc);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return (dt);
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

        public void TraerCofigMetas(string CodEmpleado, string Periodo)
        {
            SqlCommand cmd = new SqlCommand("SP_CPOS_CargarConfig_Metas", cn.LeerCadena());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EMPLEADO", CodEmpleado);
            cmd.Parameters.AddWithValue("@PeriodoMet", Periodo);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                CamposMetas = "Blanco";

            }
            else
            {
                Unds = dt.Rows[0]["Meta_Uds"].ToString();
                Ing = dt.Rows[0]["Meta_Ingresos"].ToString();
                CamposMetas = "Carga";

            }
        }

        public DataTable TraerPeriodos()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM TB_CPOS_PeriodoMetas", cn.LeerCadena());
            cmd.CommandType = CommandType.Text;
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return (dt);
        }

        public string TraerSucursal()
        {
            string CodSuc = Sucursal();
            SqlCommand cmd = new SqlCommand("SELECT Descripcion FROM TB_SUCURSALES WHERE @sucursal = CodSucursal", cn.LeerCadena());
            cmd.Parameters.AddWithValue("sucursal", CodSuc);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            string Dsucursal = dt.Rows[0][0].ToString();
            return (Dsucursal);


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

    }
}

