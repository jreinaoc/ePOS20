using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.DanaService_Datos
{
    public class D_Dana
    {

        //El uso de la clase StringBuilder nos ayudara a devolver los mensajes de las validaciones
        public readonly StringBuilder stringBuilder = new StringBuilder();

        Conexion.Conexion cn = new Conexion.Conexion();


        public DataSet ServicioDanaActivo(string Parametro)
        {
            try
            {
                stringBuilder.Clear();

                SqlCommand cmd = new SqlCommand(" select * from TB_CONFIGDANA where ParametroDana LIKE @Parametro ", cn.LeerCadena());
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Parametro", Parametro);
                cmd.CommandTimeout = 120;
                DataSet Dana = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(Dana);
                cmd.Parameters.Clear();
                return Dana;
            }

            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
            }

        }


        public DataSet ValidoDANA(string Nacionalidad, string CI)
        {
            try
            {
                stringBuilder.Clear();

                SqlCommand cmd = new SqlCommand("pGetTlfValidoDANA", cn.LeerCadena());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CTE_Nacio", Nacionalidad);
                cmd.Parameters.AddWithValue("@CTE_CedIden", CI);

                DataSet Dana = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(Dana);

                return Dana;
            }

            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
            }

        }

        public DataTable TelefonoCliente(string Nacionalidad, string CI)
        {
            try
            {
                stringBuilder.Clear();

                SqlCommand cmd = new SqlCommand(" SELECT Ind_Tlf, CTE_Nacio, CTE_CedIden, TLF_Tipo, TLF_Cod, TLF_Numero, TLF_Ext, TLF_FecCrea, TLF_FecModif, USER_Crea, USER_Modif FROM TB_CTETLF where CTE_Nacio = @Nacionalidad and CTE_CedIden= @CTE_CedIden", cn.LeerCadena());
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@Nacionalidad", Nacionalidad);
                cmd.Parameters.AddWithValue("@CTE_CedIden", CI);

                DataTable DTTelefono = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DTTelefono);

                return DTTelefono;
            }

            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
            }

        }

        public string Parametro(string Parametro)
        {
            SqlCommand cmd = new SqlCommand("select Valor from TB_PARAMETRO where Parametro= @Parametro", cn.LeerCadena());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Parametro", Parametro);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            string Valor = dt.Rows[0]["Valor"].ToString();
            return Valor;

        }

        public DataSet ParametroDana(string Campana)
        {
            try
            {
                stringBuilder.Clear();

                SqlCommand cmd = new SqlCommand("pGetParametroDana", cn.LeerCadena());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Campana", @Campana);


                DataSet Dana = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(Dana);

                return Dana;
            }

            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
            }

        }

        public bool IngresarMensajeDana(string Sucursal, string Nacionalidad, string CI, string Campaña, string FechaEnvio, string HoraEnvio, string NumOrden, string Revision, bool StatusEnvio)
        {
            try
            {
                stringBuilder.Clear();

                SqlCommand cmd = new SqlCommand("pAddSMSPorEnviar", cn.LeerCadena());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Cod_Sucursal", Sucursal);
                cmd.Parameters.AddWithValue("@CTE_Nacio", Nacionalidad);
                cmd.Parameters.AddWithValue("@CTE_CedIden", CI);
                cmd.Parameters.AddWithValue("@Campana", Campaña);
                cmd.Parameters.AddWithValue("@FechaEnvio", FechaEnvio);
                cmd.Parameters.AddWithValue("@HoraEnvio", HoraEnvio);
                cmd.Parameters.AddWithValue("@numOrdServ", NumOrden);
                cmd.Parameters.AddWithValue("@Revision", Revision);
                cmd.Parameters.AddWithValue("@Status", StatusEnvio);
                cmd.CommandTimeout = 120;
                DataSet Dana = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(Dana);
                cmd.Parameters.Clear();
                return true;
            }

            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return false;
            }
        }


        public DataSet FactPagadaNC(string NumeroFactura, string SerialImpresora)
        {
            try
            {
                stringBuilder.Clear();

                SqlCommand cmd = new SqlCommand("pGetFactPagadaNC", cn.LeerCadena());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NumFactura", NumeroFactura);
                cmd.Parameters.AddWithValue("@SerialImpresora", SerialImpresora);
                cmd.CommandTimeout = 120;
                DataSet fact = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(fact);
                cmd.Parameters.Clear();
                return fact;
            }

            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
            }

        }
    }
}
