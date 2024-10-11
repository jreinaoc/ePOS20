using CapaEntidades;
using CapaLogica;
using CapaLogica.Login_Logica;
using CapaVisual_Login;
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
    public partial class FrmLoguin : Form
    {
        //**** Instancias******
        private readonly L_Login _Login = new L_Login();
        FrmPrincipal _FrmPrincipal = new FrmPrincipal();
        FrmMensajes _FrmMensajes = new FrmMensajes();

        string user = "";
        string pass = "";
        string mensaje; 

        public FrmLoguin()
        {
            InitializeComponent();

        }

        private void FrmLoguin_Load(object sender, EventArgs e)
        {
            textBox1.Text = "";
            txtNombreUsuario.Text = "Nombre de usuario";
            txtNombreUsuario.ForeColor = Color.Gray;
            txtClaveUsuario.PasswordChar = '\0';
            txtClaveUsuario.Text = "Contraseña";
            txtClaveUsuario.ForeColor = Color.Gray;
            LblIncorrect.Visible = false;

        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            ValidarUsuario(txtNombreUsuario.Text, txtClaveUsuario.Text);

        }

        private void ValidarUsuario(string IdUsuario, string Contraseña)
        {
            mensaje = "Credenciales incorrectas, vuelva a intentarlo.";
            try
            {
               if (_Login.ComprobarCampos(txtNombreUsuario, txtClaveUsuario))
               {
                _Login.TraerUsuario(IdUsuario);

               if (TB_USUARIO.COD_EMPLEADO != null && TB_USUARIO.COD_EMPLEADO != "")
               {

                  if (TB_USUARIO.COD_EMPLEADO == "99999")
                  {
                   var HASH = _Login.UsuarioSistema();
                    TB_USUARIO.USER_HASH = HASH;
                    _FrmMensajes.co = 2;
                  }

                  bool Respuesta = _Login.ValidarClave(IdUsuario + Contraseña);
                  //mensaje = _Login.stringBuilder.ToString();
                  _FrmMensajes.avisomensaje(mensaje);
                        

                   if (_Login.stringBuilder.Length != 0)

                   {
                     if (_Login.stringBuilder.Length==17) {
                                LblIncorrect.Text = mensaje;
                                LblIncorrect.Visible = true;
                                vald.SetHighlightColor(txtNombreUsuario, DevComponents.DotNetBar.Validator.eHighlightColor.Red);
                                //DialogResult result = _FrmMensajes.ShowDialog();
                      }


                     else if (_Login.stringBuilder.Length == 29)
                      {
                      
                      this.Visible = false;
                      _FrmPrincipal.Show();
                      }
                            
                    }

               }
               else { 
                 LblIncorrect.Text = mensaje;
                        LblIncorrect.Visible = true;
                        vald.SetHighlightColor(txtNombreUsuario, DevComponents.DotNetBar.Validator.eHighlightColor.Red);
                        //_FrmMensajes.co = 2;
                        //_FrmMensajes.avisomensaje(mensaje);
                        //_FrmMensajes.ShowDialog();
                    }
               }
               else { 
                mensaje = _Login.stringBuilder.ToString();
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje(mensaje);
                _FrmMensajes.ShowDialog();
                }
            }
            catch (Exception ex)
            {
             MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }

            finally
            {
                txtClaveUsuario.Text = "";
                txtNombreUsuario.Text = "";
                txtNombreUsuario.Focus();

            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
           Close();
        }

        private void txtClaveUsuario_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    btnIngresar.PerformClick();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }
        }

        private void txtNombreUsuario_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                //para que solo acepte numeros
                if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
                {
                    e.Handled = true;
                }

                //validar que no sea la tecla de borrar 
                if (e.KeyChar != (char)8 )
                {
                    if (txtNombreUsuario.Text.Length == 4 )
                    {
                    txtClaveUsuario.Focus();
                    }
                }

                if (e.KeyChar == 13)
                {
                    if (txtNombreUsuario.Text!="" && txtClaveUsuario.Text != "")
                    {
                        btnIngresar.PerformClick();
                    }
                 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message), "Error inesperado");
            }
        }

        private void txtNombreUsuario_Enter(object sender, EventArgs e)
        {
            txtNombreUsuario.Text = "";
            txtNombreUsuario.ForeColor = Color.Black;
            
        }

        private void txtNombreUsuario_Leave(object sender, EventArgs e)
        {
            user = txtNombreUsuario.Text;
            if (user.Equals("Nombre de usuario"))
            {
                txtNombreUsuario.Text = "Nombre de usuario";
                txtNombreUsuario.ForeColor = Color.Gray;

            }
            else
            {
                if (user.Equals(""))
                {
                    txtNombreUsuario.Text = "Nombre de usuario";
                    txtNombreUsuario.ForeColor = Color.Gray;
                }
                else
                {
                    txtNombreUsuario.Text = user;
                    txtNombreUsuario.ForeColor = Color.Black;
                    
                }
            }
        }

        private void txtClaveUsuario_Enter(object sender, EventArgs e)
        {
            txtClaveUsuario.Text = "";
            txtClaveUsuario.ForeColor = Color.Black;
            txtClaveUsuario.PasswordChar = '*'; 

        }

        private void txtClaveUsuario_Leave(object sender, EventArgs e)
        {
            pass = txtClaveUsuario.Text;
            if (pass.Equals("Contraseña"))
            {
                txtClaveUsuario.Text = "Contraseña";
                txtClaveUsuario.ForeColor = Color.Gray;

            }
            else
            {
                if (pass.Equals(""))
                {
                    txtClaveUsuario.PasswordChar = '\0';
                    txtClaveUsuario.Text = "Contraseña";
                    txtClaveUsuario.ForeColor = Color.Gray;
                }
                else
                {
                    txtClaveUsuario.PasswordChar = '*';
                    txtClaveUsuario.Text = pass;
                    txtClaveUsuario.ForeColor = Color.Black;

                }
            }
        }
    }
}
