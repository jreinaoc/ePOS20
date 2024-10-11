using CapaEntidades;
using CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using CapaDatos.ListaOrdenes_Datos;

namespace CapaLogica.ListaOrden_Logica
{
    public class L_ListaOrdenes
    {
        //El uso de la clase StringBuilder nos ayudara a devolver los mensajes de las validaciones
        public readonly StringBuilder stringBuilder = new StringBuilder();

        //Instanciamos nuestra clase D_Loguin para poder utilizar sus miembros
        private D_ListaOrdenes _ListaOrdenes = new D_ListaOrdenes();


        public class Valor
        {
            public string Value { get; set; }
            public string Index { get; set; }
        }



        public bool LLenarCombobox(System.Windows.Forms.ComboBox Dias, System.Windows.Forms.ComboBox Status,System.Windows.Forms.DateTimePicker cale)
        {
            stringBuilder.Clear();

            //rellenar el ComboBox Dias 

            var Valores = new List<Valor>();

            Valores.Add(new Valor() { Index = "0", Value = "Hoy" });
            Valores.Add(new Valor() { Index = "1", Value = "Ayer" });
            Valores.Add(new Valor() { Index = "7", Value = "Ultimos 7 días" });
            Valores.Add(new Valor() { Index = "15", Value = "Ultimos 15 días" });
            Valores.Add(new Valor() { Index = "30", Value = "Ultimos 30 días" });
            Valores.Add(new Valor() { Index = "45" , Value = "Seleccione período manualmente" }); 

            Dias.DataSource = Valores;
            Dias.DisplayMember = "Value";
            Dias.ValueMember = "Index";



            //rellenar el ComboBox Status 

            var Statu = new List<Valor>();

            Statu.Add(new Valor() { Index = "", Value = "Todos" });
            Statu.Add(new Valor() { Index = "002", Value = "Enviado" });
            Statu.Add(new Valor() { Index = "003", Value = "Recibido" });
            Statu.Add(new Valor() { Index = "005", Value = "Entregado" });

            Status.DataSource = Statu;
            Status.DisplayMember = "Value";
            Status.ValueMember = "Index";

            return stringBuilder.Length == 0;
         }

        public List<Valor> LLenarComboboxOpciones()
        {
            stringBuilder.Clear();

            //rellenar el ComboBox Opciones 

            var Valores = new List<Valor>();


            Valores.Add(new Valor() { Index = "Procesar sin pago", Value = "Procesar sin pago" });
            Valores.Add(new Valor() { Index = "Anular orden", Value = "Anular orden" });
            Valores.Add(new Valor() { Index = "Reimprimir orden", Value = "Reimprimir orden" });

            return Valores;
        }

        public DataTable TraerOrdenes(System.Windows.Forms.ComboBox Dias, System.Windows.Forms.ComboBox Status, System.Windows.Forms.TextBox NunOrden)
        {
            try
            {
            stringBuilder.Clear();

                //Le enviamos el index asociados al valor selecionado en el combobox 
            DataTable Ordenes = _ListaOrdenes.CargarOrdenes(Dias.SelectedValue.ToString(), Status.SelectedValue.ToString(), NunOrden.Text.ToString());

            if (Ordenes.Rows.Count>0)
            {
                return Ordenes;
            }
            stringBuilder.Append(Environment.NewLine + "No hay ordenes");
            return null;

            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
            }

       }

        public DataTable TraerOrdporRango(System.Windows.Forms.DateTimePicker Fechadesde, System.Windows.Forms.DateTimePicker Fechahasta, System.Windows.Forms.ComboBox Status)
        {
            try
            {
                stringBuilder.Clear();

                //Le enviamos el index asociados al valor selecionado en el combobox 
                DataTable Ordenesrango = _ListaOrdenes.CargarOrdPorRango(Fechadesde.Text.ToString(),Fechahasta.Text.ToString(), Status.SelectedValue.ToString());

                if (Ordenesrango.Rows.Count > 0)
                {
                    return Ordenesrango;
                }
                stringBuilder.Append(Environment.NewLine + "No hay ordenes");
                return null;

            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
            }
        }
    }
}