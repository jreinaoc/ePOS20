using CapaEntidades;
using CapaDatos.CveAutorizada_Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace CapaLogica.ClaveAutorizada_Logica
{
    public class L_ClaveAutorizada
    {
        //public bool llenarCbxGerentes()

        //   //El uso de la clase StringBuilder nos ayudara a devolver los mensajes de las validaciones
        //   public readonly StringBuilder stringBuilder = new StringBuilder();

        //   //Instanciamos nuestra clase D_Loguin para poder utilizar sus miembros
        //   private D_ClaveAutorizada ClaveAutorizada= new D_ClaveAutorizada();

        D_ClaveAutorizada _D_ClaveAutorizada = new D_ClaveAutorizada();
        public LIST_TB_USUARIO _USUARIO = new LIST_TB_USUARIO();
        public int clavegenerada;
        public string digitos;

        public DataTable CargarGerentes()
        {

            List<LIST_TB_USUARIO> USUARIO = new List<LIST_TB_USUARIO>();
            USUARIO = _D_ClaveAutorizada.ClaveAutorizada("150");


            DataTable DT_USUARIO = new DataTable();
            CrearTabla(DT_USUARIO);

            Type itemType = typeof(LIST_TB_USUARIO);
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
            foreach (LIST_TB_USUARIO item in USUARIO)
            {
                object[] rowData = new object[DT_USUARIO.Columns.Count];
                int rowDataIndex = 0;
                // Iterate through Item Properties

                foreach (PropertyInfo property in publicProperties)
                {
                    //así obtenemos el nombre del atributo
                    string Atributo = property.Name.ToString();

                    if (Atributo == "Nombre")
                    {
                        /* así obtenemos el valor del atributo*/
                        rowData[rowDataIndex] = property.GetValue(item, null);
                        rowDataIndex++;
                    }


                }
            }

            return DT_USUARIO;
        }
        public void CrearTabla(DataTable DT_USUARIO)
        {
            //************crear Tabla*****************
            DataColumn column1 = new DataColumn("Nombre");
            DT_USUARIO.Columns.Add(column1);
            //*************************************************
        }

        public void Main()
        {
            //int a;
            //int b;
            //int c;
            //int d;
            //int e;
            //string primeroa;
            //string segundoa;
            //string terceroa;
            //string cuartoa;
            //string quintoa;
            //int alea;
            //string aleag;
            //string claveg;

            //DateTime localdate = DateTime.Now;
            //var dia = (DateTime.Now.Day);
            //var mes = (DateTime.Now.Month);
            //var hora = (DateTime.Now.Hour);
            //int clave;
            //Random r = new Random();


            //a = (r.Next(0, 10));
            //primeroa = $"{a}";
            //b = (r.Next(0, 10));
            //segundoa = $"{b}";
            //c = (r.Next(0, 10));
            //terceroa = $"{c}";
            //d = (r.Next(0, 10));
            //cuartoa = $"{d}";
            //e = (r.Next(0, 10));
            //quintoa = $"{e}";
            ////claveg = $"{clave}";
            //aleag = primeroa + segundoa + terceroa + cuartoa + quintoa;
            // alea = Convert.ToInt32(aleag);
            //MessageBox.Show(aleag);



        }
        public void generador()
        {
            int a;
            int aleatorio;
            string conv;
            DateTime localdate = DateTime.Now;
            var dia = (DateTime.Now.Day);
            var mes = (DateTime.Now.Month);
            var hora = (DateTime.Now.Hour);
            digitos = "";


            Random r = new Random();
            for (int i = 0; i < 5; i++)
            {
                a = (r.Next(1, 10));
                conv = $"{a}";
                digitos = digitos + a;
            }
            //MessageBox.Show("Su numero aleatorio es: " + digitos);
            aleatorio = Convert.ToInt32(digitos);

            clavegenerada = mes + hora + dia + aleatorio;
            

        }
    }
}
