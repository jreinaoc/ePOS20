using CapaEntidades;
using CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using CapaDatos.Inicio_Datos;
using CapaDatos.Configuracion;

namespace CapaLogica.Configuracion_Logica
{
    public class L_Configuracion
    {

        
        public readonly StringBuilder stringBuilder = new StringBuilder();

        public string Dayselect;

        D_Inicio _D_Inicio = new D_Inicio();
        D_Configuracion _D_Configuracion = new D_Configuracion();
        public class Valor
        {
            public string Value { get; set; }
            public string Index { get; set; }


        }

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

        public bool LlenadoComboBox(System.Windows.Forms.ComboBox PeriodoG)
        {
            var Valores = new List<Valor>();
            Valores.Add(new Valor() { Index = "0", Value = "Hoy" });
            Valores.Add(new Valor() { Index = "1", Value = "Ayer" });
            Valores.Add(new Valor() { Index = "7", Value = "Ultimos 7 días" });
            Valores.Add(new Valor() { Index = "15", Value = "Ultimos 15 días" });
            Valores.Add(new Valor() { Index = "30", Value = "Ultimos 30 días" });
            Valores.Add(new Valor() { Index = "150", Value = "Ultimos 150 dias" });
            Valores.Add(new Valor() { Index = "45", Value = "Seleccione período manualmente" });

            PeriodoG.DataSource = Valores;
            PeriodoG.DisplayMember = "Value";
            PeriodoG.ValueMember = "Index";

            

            //var Lapso = new List<Valor>();
            //Lapso.Add(new Valor() { Index = "2023/01", Value = "2023/01" });
            //Lapso.Add(new Valor() { Index = "2023/02", Value = "2023/02" });

            //PeriodoM.DataSource = Lapso;
            //PeriodoM.DisplayMember = "Value";
            //PeriodoM.ValueMember = "Index";

            return stringBuilder.Length == 0;
        }


        //public void EnviarConfiguracion(System.Windows.Forms.ComboBox comb,System.Windows.Forms.RadioButton Rad)
        //{
        //    for  (int TC = 01;TC < 3; TC++)
        //    {
        //        _D_Configuracion.GetConfiguracion("00001", Convert.ToString(TC), comb.SelectedValue.ToString(), "");
        //    }

        //}





        public void EnviarConfiguracion(string CodEmpleado, string PeriodoG, string miinfo, string PeriodoM, string Unidades, string IngresosBs, string TipoGrafico, string tasa, string venta, string PeriodoHasta )
        {


           
            _D_Configuracion.GetConfiguracion(CodEmpleado, "02", miinfo, " ");
            _D_Configuracion.GetConfiguracion(CodEmpleado, "05", PeriodoM, " ");
            _D_Configuracion.GetConfiguracion(CodEmpleado, "03", tasa, " ");
            _D_Configuracion.GetConfiguracion(CodEmpleado, "04", venta, " ");
            _D_Configuracion.GetConfiguracion(CodEmpleado, "06", Unidades, " ");
            _D_Configuracion.GetConfiguracion(CodEmpleado, "07", IngresosBs, " ");
            _D_Configuracion.GetConfiguracion(CodEmpleado, "08", TipoGrafico, " ");
            _D_Configuracion.GetConfiguracion(CodEmpleado, "01", PeriodoG, PeriodoHasta);


        }

        public DataTable CargarColaboradores(string sucursal)
        {
            DataTable dt = new DataTable();
            dt = _D_Configuracion.TraerColaboradores(sucursal);
            return dt;
            
        }

        public DataTable CargarPeriodos()
        {
            DataTable dt = new DataTable();
            dt = _D_Configuracion.TraerPeriodos();
            return dt;
        }

        public void CargarConfiguracion(string CodEmpleado)
        {

            _D_Configuracion.TraerConfiguracion(CodEmpleado);
            PeriodoGraf = _D_Configuracion.Cod01;
            Valor2graf = _D_Configuracion.Cod01v2;
            InfoGraf = _D_Configuracion.Cod02;
            MonedaTasa = _D_Configuracion.Cod03;
            MonedaVentas = _D_Configuracion.Cod04;
            PeriodoMeta = _D_Configuracion.Cod05;
            UnidadesMeta = _D_Configuracion.Cod06;
            IngresosMeta = _D_Configuracion.Cod07;
            TipoGrafico = _D_Configuracion.Cod08;
            ConfigDefault = _D_Configuracion.DEFAULT;


        }

        //public void prueba(System.Windows.Forms.ComboBox Dias)
        //    {

        //    _D_Inicio.GraficoVentasDet(Dias.SelectedValue.ToString(),"07165");
        //    _D_Inicio.GraficoVentasMontDet(Dias.SelectedValue.ToString(), "07165");
        //    _D_Inicio.leyendaCrist(Dias.SelectedValue.ToString(), "07165");
        //    _D_Inicio.leyendaMont(Dias.SelectedValue.ToString(), "07165");
        //    }



    }
}
