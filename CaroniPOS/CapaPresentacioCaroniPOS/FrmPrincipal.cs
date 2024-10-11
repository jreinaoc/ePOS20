using System;
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
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }

        //**** Instancias******
        FrmListaOrdenes _FrmListaOrdenes = new FrmListaOrdenes();
        FrmInicio _FrmInicio = new FrmInicio();
        FrmMensajes _FrmMensajes = new FrmMensajes();
        FrmClaveAutorizada _FrmClaveAutorizada = new FrmClaveAutorizada();
        FrmConfiguracion _FrmConfiguracion = new FrmConfiguracion();

        public void addformulario(Form F)
        {
            F.TopLevel = false;
            this.PnlListadoOrdenes.Controls.Add(F);
            F.Show();
            F.BringToFront();
            return;

        }

        private void BtnInicio_Click_1(object sender, EventArgs e)
        {

            if (_FrmConfiguracion.Signal == "yes")
            {
                ActualizacionInicio();
            }

            else
            {
                PnlListadoOrdenes.Controls.Clear();
                addformulario(_FrmInicio);

            }
        }

        private void BtnListadoOrdenes_Click(object sender, EventArgs e)
        {
            PnlListadoOrdenes.Controls.Clear();
            addformulario(_FrmListaOrdenes);
            Focus();

            _FrmListaOrdenes.listaOrdenesPrincipal();
        }

        private void FrmPrincipal_Load_1(object sender, EventArgs e)
        {
            BtnInicio.PerformClick();
            string dia = (DateTime.UtcNow.ToShortDateString());
            LblFechday.Text = dia;
            Btnosc.Visible = true;
            BtnClaro.Visible = false; 

        }

        public void Angel()
        {
            MessageBox.Show("Pruebaaaaaaaaaaaaa", "Error inesperado");

        }

        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            //this.Close();
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void Btnosc_Click(object sender, EventArgs e)
        {
            //En esta funcion se configuran los colores para el fondo y para grid de listado de ordenes 
            //col 1 es blanco, col 2 es azul agua oscuro, col 3 es azul agua claro 
            System.Drawing.Color col2 = System.Drawing.ColorTranslator.FromHtml("#257b78");
            GbxMenuPrincipal.BackColor = col2;
            System.Drawing.Color col4 = System.Drawing.ColorTranslator.FromHtml("#2f6b64");
            System.Drawing.Color col3 = System.Drawing.ColorTranslator.FromHtml(" #07a79b");

            _FrmListaOrdenes.FormatoDataGrid2(col2, col3, col4);
            BtnInicio.ForeColor = Color.White;
            BtnListadoOrdenes.ForeColor = Color.White;
            btnClienteEspera.ForeColor = Color.White;
            btnconfiguracion.ForeColor = Color.White;
            LblFechday.ForeColor = Color.White;
            BackColor = col2;
            FrmListaOrdenes frmListaOrdenes = new FrmListaOrdenes();
            frmListaOrdenes.BackColor = col2;
            //_FrmListaOrdenes.EstructuraGrid();
            foreach (var form in Application.OpenForms.Cast<Form>())
            {
                form.BackColor = col2;
            }

            Btnosc.Visible = false;
            BtnClaro.Visible = true;
        }

        private void BtnClaro_Click(object sender, EventArgs e)
        {
            //En esta funcion se configuran los colores para el fondo y para grid de listado de ordenes 
            //col 1 es blanco, col 3 es azul agua claro 
            System.Drawing.Color col1 = System.Drawing.ColorTranslator.FromHtml("#ffffff");
            System.Drawing.Color col3 = System.Drawing.ColorTranslator.FromHtml(" #07a79b");
            GbxMenuPrincipal.BackColor = col1;
            BackColor = col1;
            BtnInicio.ForeColor = Color.Black;
            BtnListadoOrdenes.ForeColor = Color.Black;
            btnClienteEspera.ForeColor = Color.Black;
            btnconfiguracion.ForeColor = Color.Black;
            LblFechday.ForeColor = Color.Black;
            _FrmListaOrdenes.FormatoDataGrid1(col1, col3);
            //_FrmListaOrdenes.EstructuraGrid();
            foreach (var form in Application.OpenForms.Cast<Form>())
            {
                form.BackColor = col1;

            }

            Btnosc.Visible = true;
            BtnClaro.Visible = false;
        }

        private void btnconfiguracion_Click(object sender, EventArgs e)
        {
            PnlListadoOrdenes.Controls.Clear();
            addformulario(_FrmConfiguracion);

            Focus();
   
        }


        public void ActualizacionInicio()
        {
            _FrmInicio.Close();
            FrmInicio pp = new FrmInicio();
            addformulario(pp);

            Focus();


        }

        private void PnlListadoOrdenes_Paint(object sender, PaintEventArgs e)
        {

        }

        private void GbxMenuPrincipal_Enter(object sender, EventArgs e)
        {

        }

        private void btnClienteEspera_Click(object sender, EventArgs e)
        {

        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    _FrmClaveAutorizada.Show();
        //}
    }
}
