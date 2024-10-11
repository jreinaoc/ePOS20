using System;
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
            
            string sucursal = "013";// numero de sucursal para mostrar los colaboradores 
            _L_Configuracion.LlenadoComboBox(CbxPeriodoDash);
            CbxPeriodoDash.SelectedIndex = 0;
            Lbldesde.Visible = false;
            LblHasta.Visible = false;
            DtpDesde.Visible = false;
            DtpHasta.Visible = false;
            CbxColaborador.DataSource = _L_Configuracion.CargarColaboradores(sucursal);
            CbxColaborador.DisplayMember = "Vendedores";
            CbxColaborador.ValueMember = "CodigoEmpleado";
            CbxPeriodoMetas.DataSource = _L_Configuracion.CargarPeriodos();
            CbxPeriodoMetas.DisplayMember = "Periodo";
            CbxPeriodoMetas.ValueMember = "Periodo";
            DtpDesde.CustomFormat = "yyyy/MM/dd";
            DtpHasta.CustomFormat = "yyyy/MM/dd";
            if (TB_USUARIO.Id_Rol=="001"){
                RbColaborador.Visible = true;
                CbxColaborador.Visible = true;
            }
            else
            {
                CbxColaborador.Visible = false;
                RbColaborador.Visible = false;
            }

             string CodEmpleado = TB_USUARIO.COD_EMPLEADO;
            CheckConfig();

            //_L_Configuracion.prueba(CbxPeriodoDash);


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
            string tienda = "013"; // Ponemos el codigo de sucursal, en algun momento habra que obtenerlo. 
            string colaborador = CbxColaborador.SelectedValue.ToString();
            string PeriodoM = CbxPeriodoMetas.SelectedValue.ToString();
            string TasaDol = "USD";
            string TasaEuro = "Euros";
            string VentasDol = "USD";
            string VentasBs = "Bs";
            string UnidadesMetas = TxtUnidadesMetas.Text;
            string IngresosBs = TxtIngresosBs.Text;
            string PeriodoHasta ;
            string TipoGrafico;

            // Validacion para mostrar grafico de metas o de ventas
            if (CbMostrarMetas.Checked)
            {
                TipoGrafico = "Metas";
            }
            else
            {
                TipoGrafico = "Ventas";
            }

            if (CbxPeriodoDash.SelectedIndex == 6)
            {
                PeriodoG = DtpDesde.Text.ToString();
                PeriodoHasta = DtpHasta.Text.ToString();

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

                _L_Configuracion.EnviarConfiguracion(CodEmpleado, PeriodoG, colaborador, PeriodoM, UnidadesMetas, IngresosBs, TipoGrafico, TasaDol , VentasBs, PeriodoHasta);

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

                _L_Configuracion.EnviarConfiguracion(CodEmpleado, PeriodoG, tienda, PeriodoM, UnidadesMetas, IngresosBs, TipoGrafico, TasaEuro ,VentasDol, PeriodoHasta);

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
            if (CbxPeriodoDash.SelectedIndex == 6)
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
          string CodEmpleado = TB_USUARIO.COD_EMPLEADO;
         _L_Configuracion.CargarConfiguracion(CodEmpleado);


            if (_L_Configuracion.ConfigDefault == "Default")
            {
                CbxPeriodoDash.SelectedIndex = 5;
                RbTienda.Checked = true;
                RbDolTasa.Checked = true;
                RbDolVentas.Checked = true;

            }

            else
            {
                if (_L_Configuracion.PeriodoGraf == "0")
                {
                    CbxPeriodoDash.SelectedIndex = 0;
                }

                if (_L_Configuracion.PeriodoGraf == "1")
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

                if (_L_Configuracion.PeriodoGraf == "150")
                {
                    CbxPeriodoDash.SelectedIndex = 5;
                }

                if (_L_Configuracion.PeriodoGraf == "45")
                {
                    CbxPeriodoDash.SelectedIndex = 6;
                }

                if (_L_Configuracion.InfoGraf == CodEmpleado)
                {
                    Rbmiinfo.Checked = true;
                }

                if (_L_Configuracion.InfoGraf.Length <= 3)
                {
                    RbTienda.Checked = true;
                }

                if (_L_Configuracion.InfoGraf.Length == 5  && _L_Configuracion.InfoGraf != CodEmpleado)
                {
                    RbColaborador.Checked = true;
                }

                if (_L_Configuracion.TipoGrafico== "Metas")
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


    }

}
