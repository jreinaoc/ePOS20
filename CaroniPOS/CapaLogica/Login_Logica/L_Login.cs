using CapaEntidades;
using CapaDatos.Login_Datos;
using CapaLogica.Login_Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;

namespace CapaLogica.Login_Logica
{
    public class L_Login
    {
        //Instanciamos nuestra clase D_Loguin para poder utilizar sus miembros
        private D_Login _usuario = new D_Login();

        //El uso de la clase StringBuilder nos ayudara a devolver los mensajes de las validaciones
        public readonly StringBuilder stringBuilder = new StringBuilder();


        //******** Variables *****************
        public DataTable dt;

        public bool TraerUsuario(string IdUsuario)
        {
            try
            {
                stringBuilder.Clear();

                _usuario.BuscarUsuario(IdUsuario);
                return stringBuilder.Length == 0;
            }

            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return false;
            }

        }

        // enviar un textbox como parámetro nota: se debe definir el  using System.Windows.Forms; en el proyecto
        public bool ComprobarCampos(System.Windows.Forms.TextBox TxtLogin, System.Windows.Forms.TextBox TxtClave)
        {
            stringBuilder.Clear();

            if (string.IsNullOrEmpty(TxtLogin.Text) && TxtClave.Text!="") stringBuilder.Append(Environment.NewLine + "El campo codigo de usuario no puede estar en blanco");
            if (string.IsNullOrEmpty(TxtClave.Text) && TxtLogin.Text!= "") stringBuilder.Append(Environment.NewLine + "El campo contraseña no puede estar en blanco");
            if (string.IsNullOrEmpty(TxtClave.Text) && string.IsNullOrEmpty(TxtLogin.Text)) stringBuilder.Append(Environment.NewLine + "Los campos no pueden estar en blanco");

            return stringBuilder.Length == 0;
        }

        public bool ValidarClave(string Clave)
        {
            try
            {
            stringBuilder.Clear();
            if (TB_USUARIO.USER_HASH != L_EncriptarDesEscriptar.Encriptar(Clave)) stringBuilder.Append(Environment.NewLine + "Datos invalidos");


            if (TB_USUARIO.USER_HASH == L_EncriptarDesEscriptar.Encriptar(Clave)) stringBuilder.Append(Environment.NewLine + "Bienvenido: " + TB_USUARIO.USER_NOMBRE);  

                return stringBuilder.Length == 0;
            }

            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return false;
            }
        }

        public string UsuarioSistema()
        {
            try
            {
                string HASH = _usuario.PaswordSistema();
            return HASH;
            }
        
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return null;
            }
        }
    }
}

