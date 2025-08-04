using System;
using CapaEntidades;
using System.Configuration;
using CapaLogica.ClaveGerente_Logica;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaLogica.Colores_Logica;
using System.Windows.Forms;
using CapaDatos.Inicio_Datos;
using CapaDatos.CveAutorizada_Datos;

namespace CapaVisual_Login
{
    public partial class FrmClaveGerente : Form
    {

        public FrmClaveGerente()
        {
            InitializeComponent();
        }

        L_ClaveGerente _L_ClaveGerente = new L_ClaveGerente();
        FrmMensajes _FrmMensajes = new FrmMensajes();
        D_Inicio _D_Inicio = new D_Inicio();
        D_ClaveAutorizada _D_ClaveAutorizada = new D_ClaveAutorizada();
        public bool Nuevo_Parametro { get; set; }
        public string Parametro_Nuevo { get; set; }

        public string Id_Rol { get; set; } 

        public string orden;
        public bool ClaveCorrecta;


        private void TxtClave_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FrmClaveGerente_Load(object sender, EventArgs e)
        {
            ClaveCorrecta = false;
            Limpiar();
            CargarDatosComboBox();
            AplicarFormatoVisual();

        }

        private void AplicarFormatoVisual()
        {
            if (L_Colores.Oscuro == true)
            {

                FormatoOsc();
            }
            else
            {
                FormatoClar();
            }

        }

        private void CargarDatosComboBox()
        {

            if (Nuevo_Parametro)
            {
                if (!string.IsNullOrEmpty(Id_Rol))
                {
                    CbxSelecGerentTiend.DataSource = _D_ClaveAutorizada.ObtengoGerentesClaveAutorizadaIII(Id_Rol);
                    if (CbxSelecGerentTiend.Items.Count > 0)
                    {
                        CbxSelecGerentTiend.SelectedIndex = 0;
                        CbxSelecGerentTiend.DisplayMember = "NOMBRE";
                        CbxSelecGerentTiend.ValueMember = "COD_USR";
                    }
                }
                else
                {
                    CbxSelecGerentTiend.DataSource = _L_ClaveGerente.TraerGerentes(_D_Inicio.Sucursal(), (LbIdRol.Text == "" ? (string)"003" : Convert.ToString(LbIdRol.Text)));
                    CbxSelecGerentTiend.DisplayMember = "Gerente";
                    CbxSelecGerentTiend.ValueMember = "CodigoEmpleado";
                }
            }
            else
            {
                CbxSelecGerentTiend.DataSource = _L_ClaveGerente.TraerGerentes(_D_Inicio.Sucursal(), (LbIdRol.Text == "" ? (string)"003" : Convert.ToString(LbIdRol.Text)));
                CbxSelecGerentTiend.DisplayMember = "Gerente";
                CbxSelecGerentTiend.ValueMember = "CodigoEmpleado";
            } 
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            ClaveCorrecta = false;
            Limpiar();
            this.Close();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            _L_ClaveGerente.TraerClave(CbxSelecGerentTiend.Text);
            if (_L_ClaveGerente.ValidarClave(TxtClave.Text) == true)
            {
                ClaveCorrecta = true;
                // Asignar el valor del usuario autorizado a la propiedad estática
                VariablesGlobales.UsuarioAutorizado_FrmClaveGerente = CbxSelecGerentTiend.Text;
            }
            else
            {
                ClaveCorrecta = false;
                // Asignar el valor del usuario autorizado a la propiedad estática
                VariablesGlobales.UsuarioAutorizado_FrmClaveGerente = "";
                string mensajer = "La clave ingresada es invalida";
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(mensajer);
                _FrmMensajes.ShowDialog();
            }
        }

        public void ObtenerOrden(string Numero)
        {
            orden = Numero;
        }

        public string RetornoNombreUsuario()
        {
            string usuario = CbxSelecGerentTiend.Text;
            return usuario;

        }

        public string RetornoClave()
        {
            string clave = TxtClave.Text;
            return clave;

        }

        public void Limpiar()
        {

            TxtClave.Text = "";
            CbxSelecGerentTiend.Text = "";
        }

        private void TxtClave_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void TxtClave_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        public void FormatoOsc()
        {
            //System.Drawing.Color col2 = System.Drawing.ColorTranslator.FromHtml("#257b78"); color anterior
           System.Drawing.Color col2 = System.Drawing.ColorTranslator.FromHtml(" #07a79b");
           this.BackColor = col2;
           LblSelecGerente.ForeColor = Color.White;
           LblClave.ForeColor = Color.White;
           LblClaveGerente.BackColor = ColorTranslator.FromHtml("#003536");


        }
        public void FormatoClar()
        {
            System.Drawing.Color col1 = System.Drawing.ColorTranslator.FromHtml("#ffffff");
            System.Drawing.Color col2 = System.Drawing.ColorTranslator.FromHtml(" #07a79b");
            this.BackColor = col1;
            LblSelecGerente.ForeColor = Color.DarkGray;
            LblClave.ForeColor = Color.Black;
            LblClaveGerente.ForeColor = Color.White;
            LblClaveGerente.BackColor = col2;
           
            
        }




    }

}
