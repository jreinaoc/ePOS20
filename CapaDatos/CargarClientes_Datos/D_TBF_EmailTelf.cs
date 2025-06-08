using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using CapaEntidades; // Asegúrate de que esta referencia sea correcta

namespace CapaDatos.CargarClientes_Datos
{
    public class D_TBF_EmailTelf_Datos
    {
        Conexion.Conexion cn = new Conexion.Conexion();
        public readonly StringBuilder stringBuilder = new StringBuilder();

        public List<TBF_EmailTelf> ObtenerContactos(string nacio,string cedula)
        {
            List<TBF_EmailTelf> listaContactos = new List<TBF_EmailTelf>();
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            try
            {
                connection = cn.LeerCadena();
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                // Cambiar a una consulta directa a la función de tabla
                command = new SqlCommand("SELECT * FROM dbo.TBF_EmailTelf(@CTE_Nacio,@CTE_CedIden)  ORDER BY 4 DESC,1  ", connection);
                command.CommandType = CommandType.Text; // Importante: Cambiar el tipo de comando a Text
                command.Parameters.AddWithValue("@CTE_Nacio", nacio);
                command.Parameters.AddWithValue("@CTE_CedIden", cedula);

                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TBF_EmailTelf contacto = new TBF_EmailTelf
                    {
                        Ind_Mail = reader["Ind_Mail"] != DBNull.Value ? reader["Ind_Mail"].ToString() : string.Empty,
                        CTE_Nacio = reader["CTE_Nacio"] != DBNull.Value ? reader["CTE_Nacio"].ToString() : string.Empty,
                        CTE_CedIden = reader["CTE_CedIden"] != DBNull.Value ? reader["CTE_CedIden"].ToString() : string.Empty,                       
                        TipoTelefono = reader["TLF_Tipo"] != DBNull.Value ? reader["TLF_Tipo"].ToString() : string.Empty,
                        TipoTLF_Descrip = reader["TipoTLF_Descrip"] != DBNull.Value ? reader["TipoTLF_Descrip"].ToString() : string.Empty,
                        CodigoTelefono = reader["TLF_Cod"] != DBNull.Value ? reader["TLF_Cod"].ToString() : string.Empty,
                        NumeroTelefono = reader["TLF_Numero"] != DBNull.Value ? reader["TLF_Numero"].ToString() : string.Empty,
                        ExtensionTelefono = reader["TLF_Ext"] != DBNull.Value ? reader["TLF_Ext"].ToString() : string.Empty,
                        Mail_Loogin = reader["Mail_Loogin"] != DBNull.Value ? reader["Mail_Loogin"].ToString() : string.Empty
                    };
                    listaContactos.Add(contacto);
                }
                return listaContactos;
            }
            catch (Exception ex)
            {
                stringBuilder.AppendLine(string.Format("Error al obtener contactos: {0}", ex.Message));
                return null;
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
    }
}
