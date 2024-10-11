using CapaDatos.Inicio_Datos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaLogica.Inicio_Logica
{
    public class L_Inicio
    {

        //El uso de la clase StringBuilder nos ayudara a devolver los mensajes de las validaciones
        public readonly StringBuilder stringBuilder = new StringBuilder();

        //Instanciamos nuestra clase D_Inicio para poder utilizar sus miembros
        D_Inicio _D_Inicio = new D_Inicio();
        public List_TB_CAORDSER _CAORDSER = new List_TB_CAORDSER();

        public string PeriodoGraf;
        public string Valor2graf;
        public string InfoGraf;
        public string MonedaTasa;
        public string MonedaVentas;
        public string PeriodoMeta;
        public string UnidadesMeta;
        public string IngresosMeta;
        public string TipoGrafico;
        public string ConfigDefault;
        public string TasaDia;
        public string VentaDia;


        public bool CargarTexboxDol()
        {
            stringBuilder.Clear();

            DateTime FechaActiva = _D_Inicio.DiaActivo();
            _D_Inicio.TasaDiaDolar(FechaActiva.ToString("yyyyMMdd"));
            TasaDia = Convert.ToString(TB_TASA_Dolar.Tasa);
          

            return stringBuilder.Length == 0;

        }

       

        public bool CargarTexboxEur()
        {
            stringBuilder.Clear();

            DateTime FechaActiva = _D_Inicio.DiaActivo();
            _D_Inicio.TasaDiaEuro(FechaActiva.ToString("yyyyMMdd"));
            TasaDia = Convert.ToString(TB_TASA_Euro.Tasa);
    

            return stringBuilder.Length == 0;

        }

        public bool CargarVentasDiasDol()
        {
            stringBuilder.Clear();



            VentaDia = _D_Inicio.VentaDiaDol("20220804");// Estamos usando fecha setada porque no hay ventas en el dia actual
            //VentaDia = _D_Inicio.VentaDia(DateTime.UtcNow.ToString("yyyyMMdd"));
            return stringBuilder.Length == 0;

        }

        public bool CargarVentasDiasBs()
        {
            stringBuilder.Clear();



            VentaDia = _D_Inicio.VentaDia("20220804"); 
            //VentaDia = _D_Inicio.VentaDia(DateTime.UtcNow.ToString("yyyyMMdd")); 

            return stringBuilder.Length == 0;

        }


        public DataTable CargarUltimasOrdenes()
        {
            try
            {
                stringBuilder.Clear();
                List<List_TB_CAORDSER> CAORDSER = new List<List_TB_CAORDSER>();
                CAORDSER = _D_Inicio.BuscarOrdenes("80");


                DataTable DT_CAORDSER = new DataTable();
                CrearTabla(DT_CAORDSER);

                Type itemType = typeof(List_TB_CAORDSER);
               PropertyInfo[] publicProperties =  // Only public non inherited properties
               itemType.GetProperties(BindingFlags.Instance | BindingFlags.Public);


                //// Create Table Columns si queremos crear la tabla con todos los campos de la lista 
                //foreach (PropertyInfo property in publicProperties)
                //{
                //    // DataSet does not support System.Nullable<>
                //    if (property.PropertyType.IsGenericType &&
                //        property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                //    {
                //        // Set the column datatype as the nullable value type
                //        DT_CAORDSER.Columns.Add(property.Name, property.PropertyType.GetGenericArguments()[0]);
                //    }
                //    else
                //    {
                //        DT_CAORDSER.Columns.Add(property.Name, property.PropertyType);
                //    }
                //}



                // Convert the Data********* Insertar los datos en la Tabla *************
                foreach (List_TB_CAORDSER item in CAORDSER)
                {
                    object[] rowData = new object[DT_CAORDSER.Columns.Count];
                    int rowDataIndex = 0;
                    // Iterate through Item Properties

                    foreach (PropertyInfo property in publicProperties)
                    {
                        //así obtenemos el nombre del atributo
                        string Atributo = property.Name.ToString();

                        if (Atributo == "NumOrdserv")
                        {
                            /* así obtenemos el valor del atributo*/
                            rowData[rowDataIndex] = property.GetValue(item, null);
                            rowDataIndex++;
                        }

                        if (Atributo == "Fecha")
                        {
                           
                                /*así obtenemos el valor del atributo*/
                            rowData[rowDataIndex] = Convert.ToString(property.GetValue(item, null)).Substring(10, 8);
                            rowDataIndex++;
                            
                       
                        }

                        if (Atributo == "VtaTotal")
                        {
                            /*   así obtenemos el valor del atributo*/
                            rowData[rowDataIndex] = property.GetValue(item, null);
                            rowDataIndex++;
                        }

                    }
                    DT_CAORDSER.Rows.Add(rowData);
                }

                return DT_CAORDSER;

        }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
            }
        }

        public void CrearTabla(DataTable DT_CAORDSER)
        {
            //************crear Tabla*****************
            DataColumn column1 = new DataColumn("N° de Orden");
            DataColumn column2 = new DataColumn("Fecha");
            DataColumn column3 = new DataColumn("Total");
            DT_CAORDSER.Columns.Add(column1);
            DT_CAORDSER.Columns.Add(column2);
            DT_CAORDSER.Columns.Add(column3);
            //*************************************************
        }

        public DataTable CargarOrdenes()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = _D_Inicio.GridOrdenes("80");
                return dt;
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
            }
        }


        public DataTable CargarClientes()
        {
            try
            {
             DataTable dt = new DataTable();
             dt = _D_Inicio.GridListaEspera();

             return dt;
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
            }
        }

        public bool BorrarClienteListaEspera(string Contenido)
        {
            stringBuilder.Clear();

            _D_Inicio.DeleteListaEspera(Contenido);

            return stringBuilder.Length == 0;

        }

        public void CargarConfiguracion(string CodEmpleado)
        {
           
            _D_Inicio.TraerConfiguracion(CodEmpleado);
            PeriodoGraf = _D_Inicio.Cod01;
            Valor2graf = _D_Inicio.Cod01v2;
            InfoGraf = _D_Inicio.Cod02;
            MonedaTasa=_D_Inicio.Cod03;
            MonedaVentas=_D_Inicio.Cod04;
            PeriodoMeta=_D_Inicio.Cod05;
            UnidadesMeta=_D_Inicio.Cod06;
            IngresosMeta=_D_Inicio.Cod07;
            TipoGrafico=_D_Inicio.Cod08;
            ConfigDefault = _D_Inicio.DEFAULT; 


        }

    



    }

   
}
