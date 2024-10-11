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
using System.Data.SqlClient;
using System.Windows.Forms;
using System;

namespace CapaVisual_Externa
{
    public partial class FrmClaveAutorizada : Form
    {
        LIST_TB_USUARIO List = new LIST_TB_USUARIO();
        L_ClaveAutorizada _L_ClaveAutorizada = new L_ClaveAutorizada();
        D_ClaveAutorizada _D_ClaveAutorizada = new D_ClaveAutorizada();
        Conexion cn = new Conexion();
        FrmMensajes _frmMensajes = new FrmMensajes();
        int idespecial;
        string texto;
        int validacion;
        string probar;


        public FrmClaveAutorizada()
        {
            InitializeComponent();
            CbxSelecGerent.Text = "Seleccionar";
           
            

        }

        public void validarclave()
        {
            if (TxtClave.Text == probar)
            {
                MessageBox.Show("vamos que si");
            }
            else
            {
                MessageBox.Show("casi casi");
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
                this.Hide();

                a = dt.Rows[0][0].ToString();
                b = a.Substring(0, 3);
                idespecial = Convert.ToInt32(b);
                //if (b == TxtClave.Text)
                {
                    idespecial = Convert.ToInt32(b);
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
            string gerente;
            gerente = CbxSelecGerent.Text;
            IngresoClaveEsp(gerente);
            validacion = _L_ClaveAutorizada.clavegenerada + idespecial;
            probar = $"{validacion}";
            validarclave();
            //this.Visible = true;
        }

        private void FrmClaveAutorizada_Load(object sender, System.EventArgs e)
        {
            
            CbxSelecGerent.DataSource = _D_ClaveAutorizada.ClaveAutorizada("150");
            CbxSelecGerent.DisplayMember = "NOMBRE";
            CbxSelecGerent.ValueMember = "COD_USR";
            
          
        } 

        private void BtnCancelar_Click(object sender, System.EventArgs e)
        {
            this.Hide();
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
            LblClaveAleatoria.Text = "Su numero aleatorio es: " + _L_ClaveAutorizada.digitos;
            //mensaje ="Su numero aleatorio es: " + _L_ClaveAutorizada.digitos;
            //_frmMensajes.co = 1;
            //_frmMensajes.avisomensaje(mensaje);
            //_frmMensajes.ShowDialog();
            //validacion = _L_ClaveAutorizada.clavegenerada + idespecial;
            //probar = $"{validacion}";
            //this.Visible = true;
        }
    }
    
}
