using System;
using System.Reflection;
using CapaEntidades;
using System.Configuration;
using CapaLogica.Configuracion_Logica;
using CapaLogica.Inicio_Logica;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaDatos.DetalleOrden_Datos;


namespace CapaVisual_Login
{
    public partial class FrmConfiguracion : Form
    {


        public FrmConfiguracion()
        {
            InitializeComponent();
        }
        FrmInicio frminicio = new FrmInicio();
        FrmMensajes frmMensajes = new FrmMensajes();
        L_Configuracion _L_Configuracion = new L_Configuracion();
        L_Inicio _L_Inicio = new L_Inicio();

        public string cod;
        public string Signal;
        private D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();

        // public string BaseDatos = Convert.ToString(ConfigurationManager.AppSettings.Get("Initial Catalog"));
        //public string Serv = Convert.ToString(ConfigurationManager.AppSettings.Get("Servidor"));


        private void LblPeriodoDashboard_Click(object sender, EventArgs e)
        {

        }

        private void Rbmiinfo_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void GbxConfDashBoard_Enter(object sender, EventArgs e)
        {

        }

        private void FrmConfiguracion_Load(object sender, EventArgs e)
        {
            string bd = ConfigurationManager.AppSettings["Initial Catalog"];


            string conexion = ConfigurationManager.ConnectionStrings["Epos"].ConnectionString; // Obtengo la cadena de conexion completa 
            string ISource = "Source="; // parametro para realizar recorte de string 
            string FSource = ";"; // parametro para realizar recorte de string
            string ICatalog = "Initial Catalog="; // parametro para realizar recorte de string
            string FCatalog = ";"; // parametro para realizar recorte de string
            string BD = stringBetween(conexion, ICatalog, FCatalog); // Funcion para obtener exactamente la base de datos
            string Servidor = stringBetween(conexion, ISource, FSource);// Funcion para obtener exactamente el servisor 
            string diasOsAbonadas = _D_DetalleOrden.TB_PARAMETRO("LimTiempoOsAbo");
            txtDiasAbo.Text = diasOsAbonadas;
            //string pruebita = Assembly.GetExecutingAssembly().GetName().Version.ToString(); // Esta es una prueba que trae la version del ensamblado

            // Para desaparecer los bordes del gruopbox
            label2.Visible = false;
            label5.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
            label12.Visible = false;
            label13.Visible = false;
            label14.Visible = false;
            label15.Visible = false;
            label16.Visible = false;

            // Se muestra la base de datos y el servidor que se esta utilizando 
            // Con la funcion aplicacion trae la version del producto, se tiene que cambiar constantemente 
            //LblVersion.Text = "Versión: "+ Application.ProductVersion+ "\n Sucursal: " + _L_Configuracion.CargarSuc() + " " + _L_Configuracion.CargarDescSucursal() + "\nInstancia:" + BD + "(" + Servidor + ")";
            LblVersion.Text = "Versión: 1.0.0.1" + "\n Sucursal: " + _L_Configuracion.CargarSuc() + " " + _L_Configuracion.CargarDescSucursal() + "\nInstancia:" + BD + "(" + Servidor + ")";
            string sucursal = TB_USUARIO.COD_SUCURSAL;// numero de sucursal para mostrar los colaboradores 
            _L_Configuracion.LlenadoComboBox(CbxPeriodoDash);
            CbxPeriodoDash.SelectedIndex = 0;
            Lbldesde.Visible = false;
            LblHasta.Visible = false;
            DtpDesde.Visible = false;
            DtpHasta.Visible = false;
            CbxColaborador.DataSource = _L_Configuracion.CargarColaboradores(); // Para cargar combobox 
            CbxColaborador.DisplayMember = "Vendedores"; // Para desplegar el combobox 
            CbxColaborador.ValueMember = "CodigoEmpleado"; // El valor que tendra el combobox al seleccionar una opcion 
            CbxPeriodoMetas.DataSource = _L_Configuracion.CargarPeriodos();
            CbxPeriodoMetas.DisplayMember = "Periodo";
            CbxPeriodoMetas.ValueMember = "Periodo";
            if (TB_USUARIO.Id_Rol == "001" || TB_USUARIO.Id_Rol == "003")
            {
                RbColaborador.Visible = true;
                CbxColaborador.Visible = true;
                Rbmiinfo.Visible = true; // Esta en modo beta esperando aprobacion 
            }
            else
            {
                CbxColaborador.Visible = false;
                RbColaborador.Visible = false;
            }

            string CodEmpleado = TB_USUARIO.COD_EMPLEADO;
            CheckConfig(); // Para mostrar en pantalla los datos de configuracion si fueron guardados 
            MostrarCamposMetas();// Para mostrar en pantalla los datos de configuracion de METAS si fueron guardados 



        }



        private void RbTienda_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {


            string CodEmpleado = TB_USUARIO.COD_EMPLEADO;
            string PeriodoG;
            string miinfo = CodEmpleado;
            string tienda = _L_Configuracion.CargarSuc();
            string colaborador = CbxColaborador.SelectedValue.ToString();
            string PeriodoM = CbxPeriodoMetas.SelectedValue.ToString();
            string TasaDol = "USD";
            string TasaEuro = "Euros";
            string VentasDol = "USD";
            string VentasBs = "Bs";
            string UnidadesMetas;
            string IngresosBs;
            if (TxtUnidadesMetas.Text == " ")
            {
                UnidadesMetas = "0";
            }
            else
            {
                UnidadesMetas = TxtUnidadesMetas.Text;
            }
            if (TxtIngresosBs.Text == " ")
            {

                IngresosBs = "0";

            }
            else
            {

                IngresosBs = TxtIngresosBs.Text;
            }


            string PeriodoHasta;
            string TipoGrafico;

            _L_Configuracion.CargarDatosMetas(CodEmpleado, PeriodoM, UnidadesMetas, IngresosBs, txtDiasAbo.Text);
            // Validacion para mostrar grafico de metas o de ventas
            if (CbMostrarMetas.Checked)
            {
                TipoGrafico = "Metas";
            }
            else
            {
                TipoGrafico = "Ventas";
            }

            if (CbxPeriodoDash.SelectedIndex == 5)
            {
                DateTime PRUE = DtpDesde.Value;
                DateTime PRUEB = DtpHasta.Value;
                PeriodoG = PRUE.ToString("yyyyMMdd");
                PeriodoHasta = PRUEB.ToString("yyyyMMdd");

            }
            else
            {
                PeriodoG = CbxPeriodoDash.SelectedValue.ToString();
                PeriodoHasta = " ";
            }

            // Validaciones para guardar configuracion en la base de datos
            // primer lote 

            if (Rbmiinfo.Checked && RbDolTasa.Checked && RbDolVentas.Checked)
            {


                _L_Configuracion.EnviarConfiguracion(CodEmpleado, PeriodoG, miinfo, PeriodoM, UnidadesMetas, IngresosBs, TipoGrafico, TasaDol, VentasDol, PeriodoHasta);

            }
            if (RbTienda.Checked && RbDolTasa.Checked && RbDolVentas.Checked)
            {

                _L_Configuracion.EnviarConfiguracion(CodEmpleado, PeriodoG, tienda, PeriodoM, UnidadesMetas, IngresosBs, TipoGrafico, TasaDol, VentasDol, PeriodoHasta);

            }
            if (RbColaborador.Checked && RbDolTasa.Checked && RbDolVentas.Checked)
            {

                _L_Configuracion.EnviarConfiguracion(CodEmpleado, PeriodoG, colaborador, PeriodoM, UnidadesMetas, IngresosBs, TipoGrafico, TasaDol, VentasDol, PeriodoHasta);

            }

            // Segundo lote 

            if (Rbmiinfo.Checked && RbDolTasa.Checked && RbBsVentas.Checked)
            {

                _L_Configuracion.EnviarConfiguracion(CodEmpleado, PeriodoG, miinfo, PeriodoM, UnidadesMetas, IngresosBs, TipoGrafico, TasaDol, VentasBs, PeriodoHasta);

            }

            if (RbTienda.Checked && RbDolTasa.Checked && RbBsVentas.Checked)
            {
                _L_Configuracion.EnviarConfiguracion(CodEmpleado, PeriodoG, tienda, PeriodoM, UnidadesMetas, IngresosBs, TipoGrafico, TasaDol, VentasBs, PeriodoHasta);

            }

            if (RbColaborador.Checked && RbDolTasa.Checked && RbBsVentas.Checked)
            {

                _L_Configuracion.EnviarConfiguracion(CodEmpleado, PeriodoG, colaborador, PeriodoM, UnidadesMetas, IngresosBs, TipoGrafico, TasaDol, VentasBs, PeriodoHasta);

            }

            //Tercer lote 

            if (Rbmiinfo.Checked && RbEurosTasa.Checked && RbBsVentas.Checked)
            {

                _L_Configuracion.EnviarConfiguracion(CodEmpleado, PeriodoG, miinfo, PeriodoM, UnidadesMetas, IngresosBs, TipoGrafico, TasaEuro, VentasBs, PeriodoHasta);

            }

            if (RbTienda.Checked && RbEurosTasa.Checked && RbBsVentas.Checked)
            {

                _L_Configuracion.EnviarConfiguracion(CodEmpleado, PeriodoG, tienda, PeriodoM, UnidadesMetas, IngresosBs, TipoGrafico, TasaEuro, VentasBs, PeriodoHasta);

            }

            if (RbColaborador.Checked && RbEurosTasa.Checked && RbBsVentas.Checked)
            {

                _L_Configuracion.EnviarConfiguracion(CodEmpleado, PeriodoG, colaborador, PeriodoM, UnidadesMetas, IngresosBs, TipoGrafico, TasaEuro, VentasBs, PeriodoHasta);

            }

            // Cuarto lote

            if (Rbmiinfo.Checked && RbEurosTasa.Checked && RbDolVentas.Checked)
            {

                _L_Configuracion.EnviarConfiguracion(CodEmpleado, PeriodoG, miinfo, PeriodoM, UnidadesMetas, IngresosBs, TipoGrafico, TasaEuro, VentasDol, PeriodoHasta);

            }

            if (RbTienda.Checked && RbEurosTasa.Checked && RbDolVentas.Checked)
            {

                _L_Configuracion.EnviarConfiguracion(CodEmpleado, PeriodoG, tienda, PeriodoM, UnidadesMetas, IngresosBs, TipoGrafico, TasaEuro, VentasDol, PeriodoHasta);

            }


            if (RbColaborador.Checked && RbEurosTasa.Checked && RbDolVentas.Checked)
            {

                _L_Configuracion.EnviarConfiguracion(CodEmpleado, PeriodoG, colaborador, PeriodoM, UnidadesMetas, IngresosBs, TipoGrafico, TasaEuro, VentasDol, PeriodoHasta);

            }

            // Para mostrar mensaje de que se guardo correctamente
            frmMensajes.co = 1;
            Signal = "yes";
            string mensaje = "Su configuración se ha guardado correctamente";
            frmMensajes.avisomensaje(mensaje);
            frmMensajes.ShowDialog();







        }

        private void LblHasta_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void CbxPeriodoDash_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // aplica cuando cambio de opcion el combobox del dashboard 
            // En caso de que la seleccion sea 5 (Periodo manualmente) Va a mostrar los calendarios de seleccion 
            if (CbxPeriodoDash.SelectedIndex == 5)
            {
                Lbldesde.Visible = true;
                LblHasta.Visible = true;
                DtpDesde.Visible = true;
                DtpHasta.Visible = true;
            }

            else
            {
                Lbldesde.Visible = false;
                LblHasta.Visible = false;
                DtpDesde.Visible = false;
                DtpHasta.Visible = false;


            }
        }

        private void RbEurosTasa_CheckedChanged(object sender, EventArgs e)
        {

        }

        public void CheckConfig()
        {
            //Esta funcion se encarga de determinar si existe ya una configuracion guardada por el usuario
            // En dado caso de que exista entonces muestra todas las opciones selleccionadas que fueron guardadas
            // Si no existe muestra una seleccion por defecto. 
            string CodEmpleado = TB_USUARIO.COD_EMPLEADO;
            _L_Configuracion.CargarConfiguracion(CodEmpleado);


            if (_L_Configuracion.ConfigDefault == "Default")
            {
                CbxPeriodoDash.SelectedIndex = 4;
                RbTienda.Checked = true;
                RbDolTasa.Checked = true;
                RbDolVentas.Checked = true;

            }

            else
            {
                if (_L_Configuracion.PeriodoGraf == "1")
                {
                    CbxPeriodoDash.SelectedIndex = 0;
                }

                if (_L_Configuracion.PeriodoGraf == "2")
                {
                    CbxPeriodoDash.SelectedIndex = 1;
                }

                if (_L_Configuracion.PeriodoGraf == "7")
                {
                    CbxPeriodoDash.SelectedIndex = 2;
                }

                if (_L_Configuracion.PeriodoGraf == "15")
                {
                    CbxPeriodoDash.SelectedIndex = 3;
                }

                if (_L_Configuracion.PeriodoGraf == "30")
                {
                    CbxPeriodoDash.SelectedIndex = 4;
                }

                //if (_L_Configuracion.PeriodoGraf == "150")
                //{
                //    CbxPeriodoDash.SelectedIndex = 5;
                //}

                if (_L_Configuracion.PeriodoGraf == "45")
                {
                    CbxPeriodoDash.SelectedIndex = 5;
                }

                if (_L_Configuracion.InfoGraf == CodEmpleado)
                {
                    Rbmiinfo.Checked = true;
                }

                if (_L_Configuracion.InfoGraf.Length <= 3)
                {
                    RbTienda.Checked = true;
                }

                CbxPeriodoMetas.Text = _L_Configuracion.PeriodoMeta;

                if (_L_Configuracion.InfoGraf.Length == 5 && _L_Configuracion.InfoGraf != CodEmpleado)
                {
                    RbColaborador.Checked = true;
                }

                if (_L_Configuracion.TipoGrafico == "Metas")
                {
                    CbMostrarMetas.Checked = true;
                }

                if (_L_Configuracion.MonedaTasa == "USD")
                {
                    RbDolTasa.Checked = true;
                }

                if (_L_Configuracion.MonedaTasa == "Euros")
                {
                    RbEurosTasa.Checked = true;
                }

                if (_L_Configuracion.MonedaVentas == "USD")
                {
                    RbDolVentas.Checked = true;
                }

                if (_L_Configuracion.MonedaVentas == "Bs")
                {
                    RbBsVentas.Checked = true;
                }

            }
        }

        public void MostrarCamposMetas()
        {
            // Esta funcion Se encarga de obtener si existe la configuracion para las metas, siempre que haya sido guardada. 
            // Si no hay una configuracion guardada para las metas, los campos de metas se mostraran en blanco 
            _L_Configuracion.CargarConfigMetas(TB_USUARIO.COD_EMPLEADO, CbxPeriodoMetas.Text);
            if (_L_Configuracion.Metas == "Blanco")
            {
                TxtIngresosBs.Text = " ";
                TxtUnidadesMetas.Text = " ";

            }
            else
            {
                TxtIngresosBs.Text = _L_Configuracion.MetaIng;
                TxtUnidadesMetas.Text = _L_Configuracion.MetaUds;

            }




        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void CbMostrarMetas_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void TxtUnidadesMetas_KeyPress(object sender, KeyPressEventArgs e)
        {
            //para que solo acepte numeros
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void TxtIngresosBs_KeyPress(object sender, KeyPressEventArgs e)
        {
            //para que solo acepte numeros
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
            }
        }


        public void FormatoConfig2(System.Drawing.Color col2, System.Drawing.Color col3, System.Drawing.Color col4)
        {
            this.BackColor = col2;
            //Con esta funcion coloreamos el grid del color oscuro 

            foreach (Control C in this.Controls)
            {
                C.ForeColor = Color.White;

            }

            GbxConfDashBoard.BackColor = Color.FromArgb(0, 152, 153);
            LblConfigDashboard.BackColor = Color.FromArgb(0, 53, 54);
            LblConfigDashboard.ForeColor = Color.White;
            GbxConfigMetas.BackColor = Color.FromArgb(0, 152, 153);
            LblMetas.BackColor = Color.FromArgb(0, 53, 54);
            LblMetas.ForeColor = Color.White;
            GbxConfigTasa.BackColor = Color.FromArgb(0, 152, 153);
            LblTasaDia.BackColor = Color.FromArgb(0, 53, 54);
            LblTasaDia.ForeColor = Color.White;
            GbxConfigVentas.BackColor = Color.FromArgb(0, 152, 153);
            LblVentaDia.BackColor = Color.FromArgb(0, 53, 54);
            LblVentaDia.ForeColor = Color.White;
            label2.Visible = true;
            label5.Visible = true;
            label16.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            label9.Visible = true;
            label10.Visible = true;
            label11.Visible = true;
            label12.Visible = true;
            label13.Visible = true;
            label14.Visible = true;
            label15.Visible = true;

        }

        public void FormatoConfig1(System.Drawing.Color col1, System.Drawing.Color col3)
        {
            this.BackColor = col1;
            //Con esta funcion coloreamos el grid del color oscuro 

            foreach (Control C in this.Controls)
            {
                C.ForeColor = Color.Black;

            }

            BtnGuardar.ForeColor = Color.White;
            BtnCancelar.ForeColor = Color.White;
            GbxConfDashBoard.BackColor = Color.White;
            LblConfigDashboard.BackColor = Color.FromArgb(0, 153, 153);
            LblConfigDashboard.ForeColor = Color.FromArgb(30, 30, 30);
            GbxConfigMetas.BackColor = Color.White;
            LblMetas.BackColor = Color.FromArgb(0, 153, 153);
            LblMetas.ForeColor = Color.FromArgb(30, 30, 30);
            GbxConfigTasa.BackColor = Color.White;
            LblTasaDia.BackColor = Color.FromArgb(0, 153, 153);
            LblTasaDia.ForeColor = Color.FromArgb(30, 30, 30);
            GbxConfigVentas.BackColor = Color.White;
            LblVentaDia.BackColor = Color.FromArgb(0, 153, 153);
            LblVentaDia.ForeColor = Color.FromArgb(30, 30, 30);

            label2.Visible = false;
            label5.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
            label12.Visible = false;
            label13.Visible = false;
            label14.Visible = false;
            label15.Visible = false;
            label16.Visible = false;

        }

        public static string stringBetween(string Source, string Start, string End)
        {
            // en esta funcion se recorta el string de la cadena de conexion solo para obtener la base de datos y el servidor 
            string result = "";
            if (Source.Contains(Start) && Source.Contains(End))
            {
                int StartIndex = Source.IndexOf(Start, 0) + Start.Length;
                int EndIndex = Source.IndexOf(End, StartIndex);
                result = Source.Substring(StartIndex, EndIndex - StartIndex);
                return result;
            }

            return result;
        }


        private void BtnCancelar_Click_1(object sender, EventArgs e)
        {
            this.Hide();
        }

        //private void CbxPeriodoMetas_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    // Aplica cada vez que se cambie de seleccion en el combobox 
        //    MostrarCamposMetas();
        //}

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void CbxPeriodoMetas_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            {
                // Aplica cada vez que se cambie de seleccion en el combobox 
                MostrarCamposMetas();
            }
        }
    }
}

