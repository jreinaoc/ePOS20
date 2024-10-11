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
        public string MetaUds;
        public string MetaIng;
        public string Metas;

        public bool LlenadoComboBox(System.Windows.Forms.ComboBox PeriodoG)
        {
            // LLena el combobox 
            // No solo muestra un texto sino que tambien almacena valores dependiendo de la opcion selccionada
            // El index es el valor y el value es lo que se muestra 
            var Valores = new List<Valor>();
            Valores.Add(new Valor() { Index = "1", Value = "Hoy" });
            Valores.Add(new Valor() { Index = "2", Value = "Ayer" });
            Valores.Add(new Valor() { Index = "7", Value = "Últimos 7 días" });
            Valores.Add(new Valor() { Index = "15", Value = "Últimos 15 días" });
            Valores.Add(new Valor() { Index = "30", Value = "Últimos 30 días" });
            //Valores.Add(new Valor() { Index = "150", Value = "Últimos 150 días" });
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


        public void EnviarConfiguracion(string CodEmpleado, string PeriodoG, string miinfo, string PeriodoM, string Unidades, string IngresosBs, string TipoGrafico, string tasa, string venta, string PeriodoHasta)
        {

            // Se envian los datos al stored dependiendo de cada opcion seleccionada en configuracion 
            // Existe 8 casos diferentes para enviar la seleccion precisa al stored

            _D_Configuracion.GetConfiguracion(CodEmpleado, "02", miinfo, " ");
            _D_Configuracion.GetConfiguracion(CodEmpleado, "05", PeriodoM, " ");
            _D_Configuracion.GetConfiguracion(CodEmpleado, "03", tasa, " ");
            _D_Configuracion.GetConfiguracion(CodEmpleado, "04", venta, " ");
            _D_Configuracion.GetConfiguracion(CodEmpleado, "06", Unidades, " ");
            _D_Configuracion.GetConfiguracion(CodEmpleado, "07", IngresosBs, " ");
            _D_Configuracion.GetConfiguracion(CodEmpleado, "08", TipoGrafico, " ");
            _D_Configuracion.GetConfiguracion(CodEmpleado, "01", PeriodoG, PeriodoHasta);


        }

        public void CargarDatosMetas(string CodEmpleado, string Periodo, string MetaUds, string MetaIng)
        {
            // Traigo el historial de metas

            _D_Configuracion.PostHistMetas(CodEmpleado, Periodo, MetaUds, MetaIng);

        }

        public DataTable CargarColaboradores()
        {
            // Traigo a los colaboradores para llenar al combobox 
            DataTable dt = new DataTable();
            dt = _D_Configuracion.TraerColaboradores();
            return dt;

        }

        public DataTable CargarPeriodos()
        {
            // Traigo el periodo para llenar el combobox 
            DataTable dt = new DataTable();
            dt = _D_Configuracion.TraerPeriodos();
            return dt;
        }

        public void CargarConfiguracion(string CodEmpleado)
        {
            // Traigo configuracion seleccionada 
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

        public void CargarConfigMetas(string CodEmpleado, string Periodo)
        {
            // Traigo configuracion de metas seleccionada 
            _D_Configuracion.TraerCofigMetas(CodEmpleado, Periodo);
            MetaUds = _D_Configuracion.Unds;
            MetaIng = _D_Configuracion.Ing;
            Metas = _D_Configuracion.CamposMetas;

        }



        public string CargarDescSucursal()
        {
            string Descripcion = _D_Configuracion.TraerSucursal();
            return (Descripcion);
        }

        public string CargarSuc()
        {
            string SUC = _D_Configuracion.Sucursal();
            return (SUC);
        }


    }
}
