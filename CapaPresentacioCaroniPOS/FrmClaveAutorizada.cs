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
using CapaDatos.DetalleOrden_Datos;
using System.Collections;

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
        private D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        int idespecial;
        string texto;
        int validacion;
        string probar;
        public string orden;
        public bool ClaveCorrecta;
        public bool CampoCorrect;
        public bool Nuevo_Parametro { get; set; }
        public string Parametro_Nuevo { get; set; }

        public string Id_Rol { get; set; }

        L_Colores _L_Colores = new L_Colores();

        private string CodigoTipo; // Variable privada NuevoRequerimiento

        public FrmClaveAutorizada(string codigoTipo)
        {
            InitializeComponent();
            CodigoTipo = codigoTipo; // Asignas el parámetro al constructor
            CbxSelecGerent.Text = "Seleccionar";
        }

        public void ValidarClave()
        {
            if (TxtClave.Text != probar)
            {
                MostrarMensajeError("La clave ingresada es invalida");
                return;
            }

            var mensaje = "¿Está seguro de devolver esta orden?";
            _FrmMensajes.co = 3;
            _FrmMensajes.avisomensaje(mensaje);
            _FrmMensajes.ShowDialog();

            if (_FrmMensajes.DialogResult == DialogResult.OK)
            {
                _FrmAnulacion.CargarOrdenAnular(orden);
                _FrmAnulacion.ShowDialog();
            }
        }

        public void IngresoClaveEsp(string gerente)
        {
            using (var cmd = new SqlCommand(
                "SELECT Id_especial FROM TB_USUARIO WHERE @gerente = USER_NOMBRE + ' ' + USER_APELLIDO",
                cn.LeerCadena()))
            {
                cmd.Parameters.AddWithValue("gerente", gerente);

                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count == 1)
                    {
                        idespecial = Convert.ToInt32(dt.Rows[0][0]);
                    }
                }
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            var gerente = CbxSelecGerent.Text;
            IngresoClaveEsp(gerente);
            string clave = "";
            if (string.IsNullOrWhiteSpace(CodigoTipo))
            {
                validacion = _L_ClaveAutorizada.clavegenerada + idespecial;
                clave = validacion.ToString();
            }
            else if (!string.IsNullOrWhiteSpace(CodigoTipo))
            {
                string TipoVenta = _D_ClaveAutorizada.BuscarTipoVentaClave(CodigoTipo);
                int Aleatoria = (int) Convert.ToInt32(LblClaveAleatoria.Text);
                // Crea una instancia:
                D_ClaveAutorizada dClaveAutorizada = new D_ClaveAutorizada();
                clave = dClaveAutorizada.BuscarNuevasClave(TipoVenta, idespecial, Aleatoria);

            }

            if (TxtClave.TextLength < 5)
            {
                CampoCorrect = false;
                MostrarMensajeError("El campo de clave debe tener al menos 5 carácteres");
                return;
            }
            CampoCorrect = true;
            if (TxtClave.Text == clave)
            {
                ClaveCorrecta = true;
                VariablesGlobales.UsuarioAutorizado_FrmClaveAutorizada = gerente;
                VariablesGlobales.CodigoUsuarioAutorizado_FrmClaveAutorizada = CbxSelecGerent.SelectedValue.ToString();
            }
            else
            {
                ClaveCorrecta = false;
                VariablesGlobales.UsuarioAutorizado_FrmClaveAutorizada = "";
                VariablesGlobales.CodigoUsuarioAutorizado_FrmClaveAutorizada = "";
                MostrarMensajeError("La clave ingresada es invalida");
            }   
            Limpiar();
            Nuevo_Parametro = false;
            Parametro_Nuevo = "";
            Id_Rol = "";
        }



        private void FrmClaveAutorizada_Load(object sender, EventArgs e)
        {
            Limpiar();
            CargarDatosComboBox(CodigoTipo);
            AplicarFormatoVisual();
        }

        private void CargarDatosComboBox(string codigoTipo)
        {
            object dataSource = null;
            //Logica vieja 
            //Si codigoTipo es Null o blanco 
            if (string.IsNullOrWhiteSpace(codigoTipo) || codigoTipo == "001")
            {
            if (Nuevo_Parametro)
            {
                if (!string.IsNullOrEmpty(Parametro_Nuevo))
                {
                    CbxSelecGerent.DataSource = _D_ClaveAutorizada.ObtengoGerentesClaveAutorizadaII(Parametro_Nuevo);
                }
                else if (!string.IsNullOrEmpty(Id_Rol))
                {
                    CbxSelecGerent.DataSource = _D_ClaveAutorizada.ObtengoGerentesClaveAutorizadaIII(Id_Rol);
                }
                else
                {
                    CbxSelecGerent.DataSource = _D_ClaveAutorizada.ClaveAutorizadaII(_D_DetalleOrden.TB_PARAMETRO("Codigo_nomina"));
                }
            }
            else
            {
                CbxSelecGerent.DataSource = _D_ClaveAutorizada.ClaveAutorizada(TB_USUARIO.COD_SUCURSAL);
            }
            }
            else if (!string.IsNullOrWhiteSpace(codigoTipo))
            {
                CbxSelecGerent.DataSource = _D_ClaveAutorizada.ObtengoGerentesClaveAutorizadaII(_D_ClaveAutorizada.BuscarEmpleadosClave(codigoTipo));
            }

            if (CbxSelecGerent.Items.Count > 0)
            {
                CbxSelecGerent.SelectedIndex = 0;
                CbxSelecGerent.DisplayMember = "NOMBRE";
                CbxSelecGerent.ValueMember = "COD_USR";
            }
        }

        private void ConfigurarDisplayYValueMembers(object dataSource)
        {
            var firstItem = (dataSource as IEnumerable)?.Cast<object>().FirstOrDefault();
            if (firstItem != null)
            {
                var type = firstItem.GetType();

                if (type.GetProperty("NOMBRE") != null)
                    CbxSelecGerent.DisplayMember = "NOMBRE";

                if (type.GetProperty("COD_USR") != null)
                    CbxSelecGerent.ValueMember = "COD_USR";
            }
        }

        private void AplicarFormatoVisual()
        {
            if (L_Colores.Oscuro)
            {
                FormatoOsc();
            }
            else
            {
                FormatoClar();
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            Hide();
            Limpiar();
            Nuevo_Parametro = false;
            Parametro_Nuevo = "";
            Id_Rol = "";
        }

        private void CbxSelecGerent_Enter(object sender, EventArgs e)
        {
            CbxSelecGerent.Text = "Seleccionar";
            CbxSelecGerent.ForeColor = Color.Gray;
        }

        private void CbxSelecGerent_Leave(object sender, EventArgs e)
        {
            var texto = CbxSelecGerent.Text;
            CbxSelecGerent.Text = string.IsNullOrEmpty(texto) || texto == "Seleccionar"
                ? "Seleccionar"
                : texto;

            CbxSelecGerent.ForeColor = texto == "Seleccionar" || string.IsNullOrEmpty(texto)
                ? Color.Gray
                : Color.Black;
        }

        private void BtnGenerar_Click(object sender, EventArgs e)
        {
            _L_ClaveAutorizada.generador();
            LblClaveAleatoria.Text = _L_ClaveAutorizada.digitos;
        }

        public void ObtenerOrden(string numero) => orden = numero;

        public void Limpiar()
        {
            if (string.IsNullOrWhiteSpace(TxtClave.Text) && string.IsNullOrEmpty(LblClaveAleatoria.Text))
            {
                ClaveCorrecta = false;
            }

            TxtClave.Text = "";
            LblClaveAleatoria.Text = "";
            CbxSelecGerent.SelectedIndex = -1;
            //CbxSelecGerent.BeginUpdate();
            CbxSelecGerent.DataSource = null;
            CbxSelecGerent.Items.Clear();
        }

        public string RetornoNombreUsuario() => CbxSelecGerent.Text;

        public string RetornoClave() => TxtClave.Text;

        private void TxtClave_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsNumber(e.KeyChar) && e.KeyChar != (char)Keys.Back;
        }

        public void FormatoOsc()
        {
            this.BackColor = ColorTranslator.FromHtml("#07a79b");
            LblSelecGerente.ForeColor = Color.White;
            LblGeneraCodigo.ForeColor = Color.White;
            LblClave.ForeColor = Color.White;
            LblClaveAutorizada.BackColor = ColorTranslator.FromHtml("#003536");
            LblClaveAutorizada.ForeColor = Color.White;
        }

        public void FormatoClar()
        {
            this.BackColor = ColorTranslator.FromHtml("#ffffff");
            LblSelecGerente.ForeColor = Color.DarkGray;
            LblGeneraCodigo.ForeColor = Color.DarkGray;
            LblClave.ForeColor = Color.Black;
            LblClaveAutorizada.BackColor = ColorTranslator.FromHtml("#07a79b");
            LblClaveAutorizada.ForeColor = Color.White;
        }

        private void MostrarMensajeError(string mensaje)
        {
            _FrmMensajes.co = 2;
            _FrmMensajes.avisomensaje(mensaje);
            _FrmMensajes.ShowDialog();
        }

        private void CbxSelecGerent_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
