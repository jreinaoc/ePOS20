using CapaDatos.Anulacion;
using CapaDatos.DetalleOrden_Datos;
using CapaDatos.Inicio_Datos;
using CapaDatos.TasaDia_Datos;
using CapaEntidades;
using CapaLogica.TasaDia_Logica;
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
    public partial class FrmTasaDia : Form
    {
        public FrmTasaDia()
        {
            InitializeComponent();
        }
        private FrmClaveAutorizada _FrmClaveAutorizada = new FrmClaveAutorizada();
        private D_TasaSecuencia _D_TasaSecuencia = new D_TasaSecuencia();
        private D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        private D_Inicio _D_Inicio = new D_Inicio();
        private D_Anulacion _D_Anulacion = new D_Anulacion();
        private L_TasaSecuencia _L_TasaSecuencia = new L_TasaSecuencia();
        private void mostrarError(string mensaje)
        {
            FrmMensajes.MostrarError(mensaje);
        }

        private DialogResult mostrarPregunta(string mensaje, string titulo)
        {
            return FrmMensajes.MostrarPregunta(mensaje, titulo);
        }

        public (string, string) Autoriz_GteReg(TextBox txtDolar,TextBox txtEuro)
        {
            DataSet dsGteReg = _D_TasaSecuencia.TasaDia(_D_DetalleOrden.TB_PARAMETRO("SucursalId"), _D_Inicio.DiaActivo().ToString("yyyy/MM/dd"));
            string AGteRegD = "SI"; 
            string AGteRegE = "SI";

            if (dsGteReg.Tables.Count > 1 && dsGteReg.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow drGteReg in dsGteReg.Tables[1].Rows)
                {
                    //-------- Dólares ------------
                    if (!string.IsNullOrEmpty(txtDolar.Text) && txtDolar.Text != "0,000000" && txtDolar.Text != "0.000000")
                    {
                        if (drGteReg["AutGteRegD"].ToString() == "SI")
                        {
                            mostrarError("Superó el límite de registro de tasas de Dólar diario");
                            AGteRegD = "SI";

                            // Pido clave de gerente regional
                            _FrmClaveAutorizada.ShowDialog();

                            if (_FrmClaveAutorizada.DialogResult == DialogResult.OK && _FrmClaveAutorizada.ClaveCorrecta == true)
                            {
                                _D_TasaSecuencia.Update_TB_Parametro("0", "SwNCAutom");
                                string GerenteAprueba = VariablesGlobales.UsuarioAutorizado_FrmClaveAutorizada;
                                AGteRegD = "NO";
                                _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "082", TB_USUARIO.COD_EMPLEADO, "--");
                            }
                            else
                            {
                                AGteRegD = "SI";
                            }
                        }
                        else
                        {
                            AGteRegD = "NO";
                        }
                    }
                    //-------- Euros ------------
                    if (!string.IsNullOrEmpty(txtEuro.Text) && txtEuro.Text != "0,000000" && txtEuro.Text != "0.000000")
                    {
                        if (drGteReg["AutGteRegE"].ToString() == "SI")
                        {
                            mostrarError("Superó el límite de registro de tasas de Euro diario");
                            AGteRegE = "SI";

                            _FrmClaveAutorizada.ShowDialog();

                            if (_FrmClaveAutorizada.DialogResult == DialogResult.OK && _FrmClaveAutorizada.ClaveCorrecta == true)
                            {
                                _D_TasaSecuencia.Update_TB_Parametro("0", "SwNCAutom");
                                string GerenteAprueba = VariablesGlobales.UsuarioAutorizado_FrmClaveAutorizada;
                                AGteRegE = "NO";
                                _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "082", TB_USUARIO.COD_EMPLEADO, "--");
                            }
                            else
                            {
                                AGteRegE = "SI";
                            }
                        }
                        else
                        {
                            AGteRegE = "NO";
                        }
                    }
                }
            }

            return (AGteRegD,AGteRegE);
        }

        public string Autoriz_GteReg_Activar()
        {
        string AGteRegD = "NO";
        DataSet dsGteReg = _D_TasaSecuencia.ActivacionDia(_D_DetalleOrden.TB_PARAMETRO("SucursalId"), _D_Inicio.DiaActivo().ToString("yyyy/MM/dd"));
            if (dsGteReg.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drGteReg in dsGteReg.Tables[0].Rows)
                {
                    // Secuencia de Activación Diaria
                    if (drGteReg["AutGteRegD"].ToString() == "SI")
                    {
                        mostrarError("Superó el límite de registro de Secuencia de Activación Diaria");
                        AGteRegD = "SI";
                        // Pido clave de gerente regional

                        _FrmClaveAutorizada.ShowDialog();

                        if (_FrmClaveAutorizada.DialogResult == DialogResult.OK && _FrmClaveAutorizada.ClaveCorrecta == true)
                        {
                            string GerenteAprueba= VariablesGlobales.UsuarioAutorizado_FrmClaveAutorizada;
                            AGteRegD = "NO";
                            _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "082", TB_USUARIO.COD_EMPLEADO, "--");
                        }
                        else
                        {
                            AGteRegD = "SI";
                        }
                    }
                    else
                    {
                        AGteRegD = "NO";
                    }
                }
            }
            return AGteRegD;
        }

        public void FormatoClaro(System.Drawing.Color col1, System.Drawing.Color col3, System.Drawing.Color col5)
        {
            //col1 es blanco, col3 es Silken Jade, col5 es Noble Black
            //Pagina 6
            Lbl_Pnl1_SubTitulo1.BackColor = col3;
            Lbl_Pnl1_SubTitulo1.ForeColor = col5;
            Lbl_Pnl2_SubTitulo1.BackColor = col3;
            Lbl_Pnl2_SubTitulo1.ForeColor = col5;
            Lbl_Pnl3_SubTitulo1.BackColor = col3;
            Lbl_Pnl3_SubTitulo1.ForeColor = col5;
            Pnl1.BackColor = col1;
            Pnl2.BackColor = col1;
            Pnl3.BackColor = col1;
        }

        public void FormatoOsc(System.Drawing.Color col1, System.Drawing.Color col3, System.Drawing.Color col5, System.Drawing.Color col6)
        {
            //col1 es blanco, col3 es Silken Jade, col 5 es Noble Black, col6 es Nordic Noir
            //Pagina 6
            Lbl_Pnl1_SubTitulo1.BackColor = col6;
            Lbl_Pnl1_SubTitulo1.ForeColor = col1;
            Lbl_Pnl2_SubTitulo1.BackColor = col6;
            Lbl_Pnl2_SubTitulo1.ForeColor = col1;
            Lbl_Pnl3_SubTitulo1.BackColor = col6;
            Lbl_Pnl3_SubTitulo1.ForeColor = col1;
            Pnl1.BackColor = ColorTranslator.FromHtml("#257b78");
            Pnl2.BackColor = ColorTranslator.FromHtml("#257b78");
            Pnl3.BackColor = ColorTranslator.FromHtml("#257b78");
        }

        private void Txt_Pnl2_Euro1_LostFocus(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(Txt_Pnl2_Euro1.Text))
            {
                Txt_Pnl2_Euro1.Text = "0,000000";
            }

            decimal euroValue = 0;

            decimal.TryParse(Txt_Pnl2_Euro1.Text, out euroValue);

            Txt_Pnl2_Euro1.Text = euroValue.ToString("N6");
        }

        private void Txt_Pnl2_Dolar1_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Txt_Pnl2_Dolar1.Text))
            {
                Txt_Pnl2_Dolar1.Text = "0,000000";
            }

            decimal dolarValue = 0;

            decimal.TryParse(Txt_Pnl2_Dolar1.Text, out dolarValue);

            Txt_Pnl2_Dolar1.Text = dolarValue.ToString("N6");

        }

        private void Txt_Pnl2_Dolar1_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == 8)
            {
                e.Handled = false;
                return;
            }


            bool IsDec = false;
            int nroDec = 0;


            if (Txt_Pnl2_Dolar1.SelectionLength <= 0)
            {

                for (int i = 0; i < Txt_Pnl2_Dolar1.Text.Length; i++)
                {
                    if (Txt_Pnl2_Dolar1.Text[i] == ',')
                        IsDec = true;

                    if (IsDec && nroDec++ >= 2)
                    {
                        e.Handled = true;
                        return;
                    }


                }
            }

            if (e.KeyChar >= 44 && e.KeyChar <= 57)
                e.Handled = false;
            ///46 = .
            ///46 = ,
            else if (e.KeyChar == 46)
                e.Handled = (IsDec) ? true : false;
            else
                e.Handled = true;


            //para que solo acepte numeros y una sola coma
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }


            if (Txt_Pnl2_Dolar1.Text.Contains("") && e.KeyChar == 44)
            {
                //separamos por punto
                string[] parts = Txt_Pnl2_Dolar1.Text.Split(',');
                //si el primer elemento está vacío, significa que no se escribió nada antes de, entonces, añadimos el cero al textbox.
                if (parts[0].Length <= 0)
                {
                    Txt_Pnl2_Dolar1.Text = "0" + Txt_Pnl2_Dolar1.Text;
                    //UPDATE: colocamos el cursor al final del texto
                    Txt_Pnl2_Dolar1.SelectionStart = Txt_Pnl2_Dolar1.Text.Length;
                }
            }
        }

        private void Txt_Pnl2_Euro1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8)
            {
                e.Handled = false;
                return;
            }


            bool IsDec = false;
            int nroDec = 0;


            if (Txt_Pnl2_Euro1.SelectionLength <= 0)
            {

                for (int i = 0; i < Txt_Pnl2_Euro1.Text.Length; i++)
                {
                    if (Txt_Pnl2_Euro1.Text[i] == ',')
                        IsDec = true;

                    if (IsDec && nroDec++ >= 2)
                    {
                        e.Handled = true;
                        return;
                    }


                }
            }

            if (e.KeyChar >= 44 && e.KeyChar <= 57)
                e.Handled = false;
            ///46 = .
            ///46 = ,
            else if (e.KeyChar == 46)
                e.Handled = (IsDec) ? true : false;
            else
                e.Handled = true;


            //para que solo acepte numeros y una sola coma
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }


            if (Txt_Pnl2_Euro1.Text.Contains("") && e.KeyChar == 44)
            {
                //separamos por punto
                string[] parts = Txt_Pnl2_Euro1.Text.Split(',');
                //si el primer elemento está vacío, significa que no se escribió nada antes de, entonces, añadimos el cero al textbox.
                if (parts[0].Length <= 0)
                {
                    Txt_Pnl2_Euro1.Text = "0" + Txt_Pnl2_Euro1.Text;
                    //UPDATE: colocamos el cursor al final del texto
                    Txt_Pnl2_Euro1.SelectionStart = Txt_Pnl2_Euro1.Text.Length;
                }
            }
        }

        private void Btn_Pnl3_Activar_Click(object sender, EventArgs e)
        {
            var Resultado = Autoriz_GteReg_Activar();
            _L_TasaSecuencia.RegistarSecuencia(Txt_Pnl3_Secuencia, LblTasaDesenc, LblFechaDesenc, LblHoraDesenc, PrBarPnl3, Resultado, mostrarPregunta, mostrarError, Btn_Pnl3_Activar);
        }

        private void Btn_Pnl2_Regi_Click(object sender, EventArgs e)
        {
            var resultado= Autoriz_GteReg(Txt_Pnl2_Dolar1, Txt_Pnl2_Euro1);
            _L_TasaSecuencia.RegistarTasa(Txt_Pnl2_Dolar1, Txt_Pnl2_Euro1, Txt_Pnl1_Dolar1, Txt_Pnl1_Euro1, Txt_Pnl1_DolarFecha, Txt_Pnl1_EuroFecha, lbRegD, lbRegE, ref resultado.Item1, ref resultado.Item2, mostrarPregunta, mostrarError);
        }

        private void FrmTasaDia_Load(object sender, EventArgs e)
        {
            _L_TasaSecuencia.Ultima_Tasa_Dia(Txt_Pnl1_Dolar1, Txt_Pnl1_Euro1, Txt_Pnl1_DolarFecha, Txt_Pnl1_EuroFecha, lbRegD, lbRegE);
           string IActivarSecADia = _D_DetalleOrden.TB_PARAMETRO("ActivarSecADia");
           //' Desactivar opcion de Activacion de secuencia diaria
           if (IActivarSecADia == "1")
            {
                Txt_Pnl3_Secuencia.Enabled = true;
                Btn_Pnl3_Activar.Enabled = true;
            }
           else if (IActivarSecADia == "0")
            {
                Txt_Pnl3_Secuencia.Enabled = false;
                Btn_Pnl3_Activar.Enabled = false;
            }


        }

        private void Txt_Pnl2_Dolar1_Click(object sender, EventArgs e)
        {
            if (Txt_Pnl2_Dolar1.Text == "0,000000")
            {
                Txt_Pnl2_Dolar1.Text = "";
            }
        }

        private void Txt_Pnl2_Euro1_Click(object sender, EventArgs e)
        {
            if (Txt_Pnl2_Euro1.Text == "0,000000")
            {
                Txt_Pnl2_Euro1.Text = "";
            }
        }

        private void Txt_Pnl3_Secuencia_Enter(object sender, EventArgs e)
        {
            // Cuando el usuario hace clic o intenta escribir
            if (Txt_Pnl3_Secuencia.Text == "Insertar secuencia")
            {
                Txt_Pnl3_Secuencia.Text = ""; // Borrar el texto sugerido
                Txt_Pnl3_Secuencia.ForeColor = Color.Black; // Cambiar el color del texto a negro
            }
        }

        private void Txt_Pnl3_Secuencia_Leave(object sender, EventArgs e)
        {
            // Cuando el usuario deja el TextBox
            if (string.IsNullOrWhiteSpace(Txt_Pnl3_Secuencia.Text))
            {
                Txt_Pnl3_Secuencia.Text = "Insertar secuencia"; // Restaurar el texto sugerido
                Txt_Pnl3_Secuencia.ForeColor = Color.DarkGray; // Cambiar el color del texto a gris
            }
        }
    }
}
