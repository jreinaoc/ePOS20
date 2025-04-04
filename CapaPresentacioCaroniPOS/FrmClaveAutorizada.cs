using CapaEntidades;
using CapaDatos.CveAutorizada_Datos;
using CapaDatos.Conexion;
using CapaLogica.ClaveAutorizada_Logica;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaLogica.Colores_Logica;
using System.Data.SqlClient;
using System.Windows.Forms;
using System;

namespace CapaVisual_Login
{
    public partial class FrmClaveAutorizada : Form
    {
        LIST_TB_USUARIO List = new LIST_TB_USUARIO();
        L_ClaveAutorizada _L_ClaveAutorizada = new L_ClaveAutorizada();
        D_ClaveAutorizada _D_ClaveAutorizada = new D_ClaveAutorizada();
        Conexion cn = new Conexion();
        FrmMensajes _FrmMensajes = new FrmMensajes();
        FrmAnulacion _FrmAnulacion = new FrmAnulacion();
        int idespecial;
        string texto;
        int validacion;
        string probar;
        public string orden;
        public bool ClaveCorrecta;
        public bool CampoCorrect;
        L_Colores _L_Colores = new L_Colores();


        public FrmClaveAutorizada()
        {
            InitializeComponent();
            CbxSelecGerent.Text = "Seleccionar";



        }

        public void validarclave()
        {
            if (TxtClave.Text == probar)
            {
                string mensaje = "¿ Esta seguro de anular esta orden ? ";
                _FrmMensajes.co = 3;
                _FrmMensajes.avisomensaje(mensaje);
                _FrmMensajes.ShowDialog();
                if (orden == "24551")
                {

                }
                if (_FrmMensajes.DialogResult == DialogResult.OK)
                {
                    _FrmAnulacion.CargarOrdenAnular(orden);
                    //this.Hide();
                    _FrmAnulacion.ShowDialog();


                }
                else
                {

                }
            }
            else
            {
                string mensaje = "La clave ingresada es invalida";
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(mensaje);
                _FrmMensajes.ShowDialog();
            }
        }

        public void IngresoClaveEsp(string gerente)
        {
            string a;
            string b;
            string mensaje;
            SqlCommand cmd = new SqlCommand("SELECT Id_especial FROM TB_USUARIO WHERE @gerente = USER_NOMBRE + ' '+ USER_APELLIDO", cn.LeerCadena());
            cmd.Parameters.AddWithValue("gerente", gerente);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 1)
            {
                //this.Hide();

                a = dt.Rows[0][0].ToString();
                //b = a.Substring(0, 3);
                idespecial = Convert.ToInt32(a);
                //if (b == TxtClave.Text)
                {
                   // idespecial = Convert.ToInt32(b);
                    //MessageBox.Show("se pudo");
                }
                //else
                //{
                //    mensaje = "Su clave de gerente es invalida" ;
                //    _frmMensajes.co = 2;
                //    _frmMensajes.avisomensaje(mensaje);
                //    _frmMensajes.ShowDialog();

                //}

            }

        }

        private void BtnGuardar_Click(object sender, System.EventArgs e)
        {

            DialogResult = DialogResult.OK;
            string gerente;
            gerente = CbxSelecGerent.Text;
            IngresoClaveEsp(gerente);
            validacion = _L_ClaveAutorizada.clavegenerada + idespecial;
            probar = $"{validacion}";

            if (TxtClave.TextLength < 5)
            {
                CampoCorrect = false;
                string mensajer = "El campo de clave debe tener al menos 5 carácteres";
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(mensajer);
                _FrmMensajes.ShowDialog();
            }

            else
            {
                CampoCorrect = true;
                if (TxtClave.Text == probar)
                {
                    ClaveCorrecta = true;
                    // Asignar el valor del usuario autorizado a la propiedad estática
                    VariablesGlobales.UsuarioAutorizado_FrmClaveAutorizada = CbxSelecGerent.Text;


                }
                else
                {
                    ClaveCorrecta = false;
                    // Asignar el valor del usuario autorizado a la propiedad estática
                    VariablesGlobales.UsuarioAutorizado_FrmClaveAutorizada = "";
                    string mensajer = "La clave ingresada es invalida";
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje(mensajer);
                    _FrmMensajes.ShowDialog();

                }
            }
            Limpiar();

        }

        private void FrmClaveAutorizada_Load(object sender, System.EventArgs e)
        {
            Limpiar();
            CbxSelecGerent.DataSource = _D_ClaveAutorizada.ClaveAutorizada(TB_USUARIO.COD_SUCURSAL);// Antes tenia el valor steado ahora es por codigo de sucursal 
            CbxSelecGerent.DisplayMember = "NOMBRE";
            CbxSelecGerent.ValueMember = "COD_USR";

            if(L_Colores.Oscuro == true)
            {

                FormatoOsc();
            }
            else
            {
                FormatoClar();
            }


        }

        private void BtnCancelar_Click(object sender, System.EventArgs e)
        {
            this.Hide();
            Limpiar();
        }

        private void CbxSelecGerent_Enter(object sender, System.EventArgs e)
        {
            CbxSelecGerent.Text = "Seleccionar";
            CbxSelecGerent.ForeColor = Color.Gray;
        }

        private void CbxSelecGerent_Leave(object sender, System.EventArgs e)
        {
            texto = CbxSelecGerent.Text;
            if (texto.Equals("Seleccionar"))
            {
                CbxSelecGerent.Text = "Seleccionar";
                CbxSelecGerent.ForeColor = Color.Gray;



            }
            else
            {
                if (texto.Equals(""))
                {
                    CbxSelecGerent.Text = "Seleccionar";
                    CbxSelecGerent.ForeColor = Color.Gray;
                }
                else
                {
                    CbxSelecGerent.Text = texto;
                    CbxSelecGerent.ForeColor = Color.Black;

                }
            }
        }

        private void BtnGenerar_Click(object sender, System.EventArgs e)
        {

            string mensaje;
            string gerente;
            gerente = CbxSelecGerent.Text;
            //IngresoClaveEsp(gerente);
            _L_ClaveAutorizada.generador();
            LblClaveAleatoria.Text = _L_ClaveAutorizada.digitos;
            //mensaje ="Su numero aleatorio es: " + _L_ClaveAutorizada.digitos;
            //_frmMensajes.co = 1;
            //_frmMensajes.avisomensaje(mensaje);
            //_frmMensajes.ShowDialog();
            //validacion = _L_ClaveAutorizada.clavegenerada + idespecial;
            //probar = $"{validacion}";
            //this.Visible = true;
        }

        private void CbxSelecGerent_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void ObtenerOrden(string Numero)
        {
            orden = Numero;
        }


        public void Limpiar()
        {

            if(TxtClave.Text.Replace(" ","") == "" & LblClaveAleatoria.Text == "")
            {
                ClaveCorrecta = false;
            }
            TxtClave.Text = "";
            //CbxSelecGerent.Text = "";
            LblClaveAleatoria.Text = "";
            CbxSelecGerent.SelectedIndex = - 1;

        }

        public string RetornoNombreUsuario()
        {
            string usuario = CbxSelecGerent.Text;
            return usuario;

        }

        public string RetornoClave()
        {
            string clave = TxtClave.Text;
            return clave;

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
            LblGeneraCodigo.ForeColor = Color.White;
            LblClave.ForeColor = Color.White;


        }
        public void FormatoClar()
        {
            System.Drawing.Color col1 = System.Drawing.ColorTranslator.FromHtml("#ffffff");
            this.BackColor = col1;
            LblSelecGerente.ForeColor = Color.DarkGray;
            LblGeneraCodigo.ForeColor = Color.DarkGray;
            LblClave.ForeColor = Color.Black;

        }
   

       


    }
}
