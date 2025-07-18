using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CapaDatos.Conexion;
using CapaDatos.TasaDia_Datos;
using CapaDatos.Inicio_Datos;
using CapaEntidades;
using CapaLogica.ListaFactura_Logica;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaDatos.DetalleOrden_Datos;
using System.Drawing;
using System.Diagnostics;
using CapaDatos.Anulacion;
using System.Globalization;

namespace CapaLogica.TasaDia_Logica
{
    public class L_TasaSecuencia
    {
        private D_TasaSecuencia _D_TasaSecuencia = new D_TasaSecuencia();
        private D_Inicio _D_Inicio = new D_Inicio();
        public readonly StringBuilder stringBuilder = new StringBuilder();
        public DateTime DateTimeTasa;
        private D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        private D_Anulacion _D_Anulacion = new D_Anulacion();
       
        public void RegistarSecuencia(System.Windows.Forms.TextBox TxtCadenaEncriptada, System.Windows.Forms.Label LblTasaDesenc, System.Windows.Forms.Label LblFechaDesenc, System.Windows.Forms.Label LblHoraDesenc, System.Windows.Forms.ProgressBar ProgressBar1,
        string AGteRegD, Func<string, string, DialogResult> mostrarPregunta, Action<string> mostrarError, System.Windows.Forms.Button Btn_Pnl3_Activar)
        {
            try
            {
                if (!string.IsNullOrEmpty(TxtCadenaEncriptada.Text))
                {
                    // Validar espacios en blanco
                    bool ValidoEspacio = false;

                    for (int i = 0; i < TxtCadenaEncriptada.Text.Length; i++)
                    {
                        if (TxtCadenaEncriptada.Text.Substring(i, 1) == " ")
                        {
                            ValidoEspacio = true;
                        }
                    }

                    if (!ValidoEspacio)
                    {
                        bool ValidoDescrip = false;
                        bool ValidoLimiteCambio = false;
                        ValidoFormatoTasa2(TxtCadenaEncriptada, ref ValidoDescrip, ref ValidoLimiteCambio, LblTasaDesenc, LblFechaDesenc, LblHoraDesenc);

                        if (!ValidoDescrip)
                        {
                            string ValDol, ValEur;

                            //Autoriz_GteReg_Activar();

                            Fecha_formato(TxtCadenaEncriptada, LblTasaDesenc,  LblFechaDesenc, LblHoraDesenc);

                            if (DateTimeTasa >= (_D_Inicio.DiaActivo()))
                            {
                                string FechaSecuencia = _D_DetalleOrden.TB_PARAMETRO("FechaUSecuencia");
                                // Opción 1: Normalizar los designadores AM/PM primero
                                FechaSecuencia = FechaSecuencia.Replace("a. m.", "a.m.")
                                                               .Replace("p. m.", "p.m.");

                                CultureInfo cultura = new CultureInfo("es-ES");
                                cultura.DateTimeFormat.AMDesignator = "a.m.";
                                cultura.DateTimeFormat.PMDesignator = "p.m.";

                                if (DateTime.TryParseExact(FechaSecuencia, "dd/MM/yyyy hh:mm:ss tt",
                          cultura,
                                DateTimeStyles.None,
                         out DateTime fechaConvertida))
                                {
                                    // Ahora podemos hacer la comparación
                                    if (Convert.ToDateTime(DateTimeTasa) < fechaConvertida)
                                    {
                                        mostrarError("No se puede realizar la Activación.");
                                    }
                                    else
                                    {
                                    if (AGteRegD == "NO")
                                    {
                                        var resultado = mostrarPregunta("¿Está seguro que desea activar la secuencia diaria?", "CONFIRME");

                                        if (resultado != DialogResult.OK)
                                        {
                                            return;
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(TxtCadenaEncriptada.Text))
                                            {
                                                // Desencriptar
                                                DataSet dsEjecutaDesencriptar = _D_TasaSecuencia.EncripDescrip(TxtCadenaEncriptada.Text, "I");
                                                string Resultado = "";
                                                int Cadena1 = 0;

                                                DataSet dsDigitoVerificador = new DataSet();
                                                string DigitoVerificador = "";
                                                string ResDigitoVerificador = "";

                                                if (dsEjecutaDesencriptar.Tables[0].Rows.Count > 0)
                                                {
                                                    Resultado = dsEjecutaDesencriptar.Tables[0].Rows[0][0].ToString();
                                                    Cadena1 = Resultado.Length;
                                                    LblTasaDesenc.Text = Resultado.Substring(0, Cadena1 - 11);
                                                    LblFechaDesenc.Text = Resultado.Substring(Cadena1 - 11, 6);
                                                    LblHoraDesenc.Text = Resultado.Substring(Cadena1 - 5, 4);
                                                    DigitoVerificador = Resultado.Substring(Cadena1 - 1, 1);
                                                    dsDigitoVerificador = _D_TasaSecuencia.ComparaDigitoVerificador(TxtCadenaEncriptada.Text, DigitoVerificador);
                                                    ResDigitoVerificador = dsDigitoVerificador.Tables[0].Rows[0][0].ToString();
                                                    if (ResDigitoVerificador == "SECUENCIA NO VALIDA")
                                                    {
                                                        mostrarError("La secuencia no es valida");
                                                        return;
                                                    }
                                                }

                                                // Fin Desencriptar

                                                if (!string.IsNullOrEmpty(LblTasaDesenc.Text))
                                                {
                                                    mostrarError("Se realizará el proceso de actualización");

                                                    for (int x = 1; x < 30; x++)
                                                        ProgressBar1.Value = x;

                                                    DataSet DSUpdPrecio = _D_TasaSecuencia.ActualizarArtDolar(_D_DetalleOrden.TB_PARAMETRO("SucursalId"), TB_USUARIO.COD_USR, TxtCadenaEncriptada.Text, LblTasaDesenc.Text, _D_Inicio.DiaActivo(), DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"), DateTimeTasa.ToString("dd/MM/yyyy hh:mm:ss tt"));

                                                    for (int x = 30; x < 70; x++)
                                                        ProgressBar1.Value = x;

                                                    if (DSUpdPrecio.Tables[0].Rows[0][0].ToString() == "SATISFACTORIO")
                                                    {
                                                        //lblFecUltActSec.Text = _D_DetalleOrden.TB_PARAMETRO("FechaUActivaT");

                                                        _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "083", TB_USUARIO.COD_EMPLEADO, "La Secuencia de Activación Diaria se actualizó correctamente");

                                                        for (int x = 71; x < 100; x++)
                                                            ProgressBar1.Value = x;

                                                        mostrarError("El proceso culminó correctamente");

                                                        ProgressBar1.Value = 0;

                                                        // Envio de Correo
                                                        string IActivarEmailASD = _D_DetalleOrden.TB_PARAMETRO("ActivarEmailASD");

                                                        if (IActivarEmailASD == "1")
                                                        {


                                                            string strSucursalDeTrabajo = _D_DetalleOrden.TB_PARAMETRO("Sucursal");

                                                            string NombreSucursal = _D_DetalleOrden.Nombre_Surculsal_PagoMovil(strSucursalDeTrabajo, null);

                                                            string valor1 = _D_DetalleOrden.TB_PARAMETRO("FechaUSecuencia");

                                                            var Correo = FormatoCorreoSecuencia(strSucursalDeTrabajo, NombreSucursal, LblTasaDesenc.Text, valor1);

                                                            EnviarCorreo(Correo.Asunto, Correo.CuerpoMensaje);

                                                        }

                                                        // Blanquear campo de Secuencia al finalizar proceso de activación 
                                                        TxtCadenaEncriptada.Text = "";

                                                            string IActivarSecADia = _D_DetalleOrden.TB_PARAMETRO("ActivarSecADia");
                                                            //' Desactivar opcion de Activacion de secuencia diaria
                                                            if (IActivarSecADia == "1")
                                                            {
                                                                TxtCadenaEncriptada.Enabled = true;
                                                                Btn_Pnl3_Activar.Enabled = true;
                                                            }
                                                            else if (IActivarSecADia == "0")
                                                            {
                                                                TxtCadenaEncriptada.Enabled = false;
                                                                Btn_Pnl3_Activar.Enabled = false;
                                                            }

                                                        }
                                                        else
                                                    {
                                                        LblTasaDesenc.Text = "";
                                                        LblFechaDesenc.Text = "";
                                                        LblHoraDesenc.Text = "";
                                                        mostrarError("Hubo problemas realizando el proceso");
                                                        ProgressBar1.Value = 0;
                                                        _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "083", TB_USUARIO.COD_EMPLEADO, "La Secuencia de Activación Diaria no se actualizó correctamente");
                                                    }
                                                }
                                                else
                                                {
                                                    ProgressBar1.Value = 0;
                                                    mostrarError("El valor introducido no es correcto. Debe solicitar uno nuevo");
                                                    LblTasaDesenc.Text = "";
                                                    LblFechaDesenc.Text = "";
                                                    LblHoraDesenc.Text = "";
                                                }
                                            }
                                        }
                                    }
                                    ProgressBar1.Value = 0;
                                }
                            }
                            }
                            else
                            {
                                ProgressBar1.Value = 0;
                                LblTasaDesenc.Text = "";
                                LblFechaDesenc.Text = "";
                                LblHoraDesenc.Text = "";
                                mostrarError("La fecha de la secuencia debe ser mayor al día activo. Debe solicitar uno nuevo");
                            }
                        }
                        else
                        {
                            if (ValidoLimiteCambio)
                            {
                                ProgressBar1.Value = 0;
                                LblTasaDesenc.Text = "";
                                LblFechaDesenc.Text = "";
                                LblHoraDesenc.Text = "";
                                mostrarError("El valor introducido supera el % permitido para actualizar la secuencia. Debe solicitar uno nuevo");
                            }
                            else
                            {
                                ProgressBar1.Value = 0;
                                LblTasaDesenc.Text = "";
                                LblFechaDesenc.Text = "";
                                LblHoraDesenc.Text = "";
                                mostrarError("El valor introducido no es correcto. Debe solicitar uno nuevo");
                            }
                        }
                    }
                }
                else
                {
                    ProgressBar1.Value = 0;
                    LblTasaDesenc.Text = "";
                    LblFechaDesenc.Text = "";
                    LblHoraDesenc.Text = "";
                    mostrarError("El valor introducido no es correcto. Debe solicitar uno nuevo");
                }
            }
            catch (Exception ex)
            {
                // MensajeError.MuestroMensaje("Error en la función", "FrmDesencriptar", "Por favor comunicarse con el Dpto. de Sistemas y reportar el siguiente error: ", ex.Message, CapaNegocio.MensajesGenerales.TiposIconos.IconoError, glbUsuarioActual);
                mostrarError($"Error en validación de montos: {ex.Message}");
            }
        }

        private bool ValidoFormatoTasa(System.Windows.Forms.TextBox TxtCadenaEncriptada, ref bool ValidoDescrip, ref bool ValidoLimiteCambio, System.Windows.Forms.Label LblTasaDesenc, System.Windows.Forms.Label LblFechaDesenc, System.Windows.Forms.Label LblHoraDesenc)
        {
                // Desencriptar
                string Resultado = "";
                int Cadena1 = 0;
                string UResultado = "";
                int UCadena1 = 0;
                int i;
                decimal VaMinMaxTasa = 0;
                decimal PorcMinTasa = 0;
                decimal PorcMaxTasa = 0;
                string UTasaDesenc = "";
                string UFechaDesenc = "";
                string UHoraDesenc = "";

                DataSet dsEjecutaDesencriptar = _D_TasaSecuencia.EncripDescrip(TxtCadenaEncriptada.Text, "I");
                if (dsEjecutaDesencriptar.Tables[0].Rows.Count > 0)
                {
                    Resultado = dsEjecutaDesencriptar.Tables[0].Rows[0][0].ToString();
                    Cadena1 = Resultado.Length;
                    LblTasaDesenc.Text = Resultado.Substring(0, Cadena1 - 11);
                    LblFechaDesenc.Text = Resultado.Substring(Cadena1 - 11, 6);
                    LblHoraDesenc.Text = Resultado.Substring(Cadena1 - 5, 4);
                }

                ValidoDescrip = false;

                // FORMATO DE LA TASA

                // Verifico que no este vacio
                if (LblTasaDesenc.Text == "")
                {
                    ValidoDescrip = true;
                }

                // Validar espacios en blanco
                for (i = 0; i < LblTasaDesenc.Text.Length; i++)
                {
                    if (LblTasaDesenc.Text.Substring(i, 1) == " ")
                    {
                        ValidoDescrip = true;
                    }
                }

                // Verifico el largo del campo, debe ser al menos 6 caracteres
                if (LblTasaDesenc.Text.Length < 6)
                {
                    ValidoDescrip = true;
                }

                // Verifico que sean numeros
                if (!ValidoNumeros(LblTasaDesenc.Text))
                {
                    ValidoDescrip = true;
                }

                // Verifico que sean solo números o coma
                for (i = 0; i < LblTasaDesenc.Text.Length; i++)
                {
                    char c = LblTasaDesenc.Text[i];
                    if (!char.IsDigit(c) && c != ',')
                    {
                        ValidoDescrip = true;
                    }
                }

                // Verifico que el valor esté dentro del rango permitido

                // Busco ultima secuencia cargada
                string UltSecCarg = "";

                DataSet dsEjecutaUltima = _D_TasaSecuencia.ObtenerUltSecuencia(_D_DetalleOrden.TB_PARAMETRO("SucursalId"));

                if (dsEjecutaUltima.Tables[0].Rows.Count > 0)
                {
                    UltSecCarg = dsEjecutaUltima.Tables[0].Rows[0][0].ToString();
                }

                DataSet dsEjecutaDesencriptar2 = _D_TasaSecuencia.EncripDescrip(UltSecCarg, "I");

                if (dsEjecutaDesencriptar2.Tables[0].Rows.Count > 0)
                {
                    UResultado = dsEjecutaDesencriptar2.Tables[0].Rows[0][0].ToString();
                    UCadena1 = UResultado.Length;
                    UTasaDesenc = UResultado.Substring(0, UCadena1 - 11);
                    UFechaDesenc = UResultado.Substring(UCadena1 - 11, 6);
                    UHoraDesenc = UResultado.Substring(UCadena1 - 5, 4);
                }

                // rango permitido
                VaMinMaxTasa = Convert.ToInt32(_D_DetalleOrden.TB_PARAMETRO("ValorMinMaxTasa"));

            PorcMinTasa = decimal.Parse(UTasaDesenc) - ((decimal.Parse(UTasaDesenc) * VaMinMaxTasa) / 100m);
            PorcMaxTasa = decimal.Parse(UTasaDesenc) + ((decimal.Parse(UTasaDesenc) * VaMinMaxTasa) / 100m);

            decimal tasaDesenc = 0;
                decimal.TryParse(LblTasaDesenc.Text.Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out tasaDesenc);

                if (tasaDesenc < (PorcMinTasa / 100m))
                {
                    ValidoDescrip = true;
                    ValidoLimiteCambio = true;
                }

                if (tasaDesenc > PorcMaxTasa)
                {
                    ValidoDescrip = true;
                    ValidoLimiteCambio = true;
                }

                // FORMATO DE LA FECHA

                // Verifico que no este vacio
                if (LblFechaDesenc.Text == "")
                {
                    ValidoDescrip = true;
                }

                // Verifico el largo del campo, debe ser de 6 caracteres
                if (LblFechaDesenc.Text.Length != 6)
                {
                    ValidoDescrip = true;
                }

                bool TFecha = false;

                // Verifico que sean numeros
                for (i = 0; i < LblFechaDesenc.Text.Length; i++)
                {
                    char c = LblFechaDesenc.Text[i];
                    if (!char.IsDigit(c))
                    {
                        ValidoDescrip = true;
                        TFecha = true;
                    }
                }

                if (!TFecha)
                {
                    if (!ValidoNumeros(LblFechaDesenc.Text))
                    {
                        ValidoDescrip = true;
                    }

                    // Verifico que el dia tenga numero valido 
                    int dia = int.Parse(LblFechaDesenc.Text.Substring(0, 2));
                    if (dia < 1 || dia > 31)
                    {
                        ValidoDescrip = true;
                    }

                    // Verifico que el mes tenga numero valido 
                    int mes = int.Parse(LblFechaDesenc.Text.Substring(2, 2));
                    if (mes < 1 || mes > 12)
                    {
                        ValidoDescrip = true;
                    }

                    // Verifico que el año tenga numero valido
                    int anio = int.Parse(LblFechaDesenc.Text.Substring(4, 2));
                    if (anio < 20 || anio > 50)
                    {
                        ValidoDescrip = true;
                    }
                }

                // FORMATO DE LA HORA

                // Verifico que no este vacio
                if (LblHoraDesenc.Text == "")
                {
                    ValidoDescrip = true;
                }

                // Verifico el largo del campo, debe ser de 4 caracteres
                if (LblHoraDesenc.Text.Length != 4)
                {
                    ValidoDescrip = true;
                }

                bool THora = false;

                // Verifico que sean numeros
                for (i = 0; i < LblHoraDesenc.Text.Length; i++)
                {
                    char c = LblHoraDesenc.Text[i];
                    if (!char.IsDigit(c))
                    {
                        ValidoDescrip = true;
                        THora = true;
                    }
                }

                if (!THora)
                {
                    if (!ValidoNumeros(LblHoraDesenc.Text))
                    {
                        ValidoDescrip = true;
                    }

                    // Verifico que la hora tenga numero valido
                    int hora = int.Parse(LblHoraDesenc.Text.Substring(0, 2));
                    if (hora < 1 || hora > 24)
                    {
                        ValidoDescrip = true;
                    }

                    // Verifico que los minutos tengan numero valido
                    int minutos = int.Parse(LblHoraDesenc.Text.Substring(2, 2));
                    if (minutos < 0 || minutos > 59)
                    {
                        ValidoDescrip = true;
                    }
                }

            return !ValidoDescrip;
        }

        public bool ValidoNumeros(string valor)
        {
            string str = "0123456789";
            // Si el valor es un solo carácter y no es Backspace (ASCII 8)
            if (valor.Length == 1 && (int)valor[0] != 8)
            {
                if (!str.Contains(valor))
                    return true;
                else
                    return false;
            }
            return false;
        }

        public void EnviarCorreo(string asunto, string cuerpoMensaje)
        {
            // Llamado a variables de config.
            string urlApp = _D_DetalleOrden.TB_PARAMETRO("RutaEnvioEmail") + "AppEnvioEmail.exe";
            var Orig = _D_DetalleOrden.TB_PARAMETRO("UserEnvioEmail");
            var DestinatariosP = _D_DetalleOrden.TB_PARAMETRO("EnvioEmail3");
            var DestinatariosC = "";
            var User = _D_DetalleOrden.TB_PARAMETRO("UserEnvioEmail");
            var pass = _D_DetalleOrden.TB_PARAMETRO("PassEnvioEmail");
            string adj = "";
            var HostEnvioMail = _D_DetalleOrden.TB_PARAMETRO("HostEnvioMail");

            // Construir el comando para la app externa
            string strComand = ";" + Orig + ";" + DestinatariosP + ";" + DestinatariosC + ";" + asunto + ";" + User + ";" + pass + ";" + cuerpoMensaje + ";" + adj + ";" + "CARONI" + ";1";

            // Ejecutar app externa con parámetros
            Process.Start(urlApp, strComand);
            return;
        }

        public (string Asunto, string CuerpoMensaje) FormatoCorreoSecuencia(string codSucursal, string sucursalDescripcion, string tasa, string fechaSecuencia)
        {
            string asunto = "Sucursal " + codSucursal + " " + sucursalDescripcion + " - Secuencia Activación Diaria";
            string cuerpo = "Secuencia Activación Diaria -  Tasa: " + tasa + " Fecha: " + fechaSecuencia;
            return (asunto, cuerpo);
        }

        public void Ultima_Tasa_Dia(TextBox txtUDolar, TextBox txtUEuro, TextBox txtUFechaDol, TextBox txtUFechaEur, Label lbRegD, Label lbRegE)
        {
                DataSet dsConsTasa = _D_TasaSecuencia.TasaDia(_D_DetalleOrden.TB_PARAMETRO("SucursalId"), _D_Inicio.DiaActivo().ToString("yyyy/MM/dd"));

                if (dsConsTasa.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsConsTasa.Tables[0].Rows)
                    {
                        txtUDolar.Text = Convert.ToDecimal(dr["UTasa_Dol"]).ToString("N6");
                        txtUEuro.Text = Convert.ToDecimal(dr["UTasa_Eur"]).ToString("N6");
                        txtUFechaDol.Text = dr["UFecCr_Dol"].ToString();
                        txtUFechaEur.Text = dr["UFecCr_Eur"].ToString();
                    }
                }
                if (dsConsTasa.Tables.Count > 1 && dsConsTasa.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsConsTasa.Tables[1].Rows)
                    {
                        lbRegD.Text = dr["CantDolar"].ToString();
                        lbRegE.Text = dr["CantEuro"].ToString();
                    }
                }
        }

    public void RegistarTasa(TextBox txtDolar, TextBox txtEuro, TextBox txtUDolar, TextBox txtUEuro, TextBox txtUFechaDol, TextBox txtUFechaEur, Label lbRegD, Label lbRegE,
    ref string AGteRegD, ref string  AGteRegE,
    Func<string, string, DialogResult> mostrarPregunta, Action<string> mostrarError)

    {
         try
         {
                string ValDol;
            string ValEur;

            // Autoriz_GteReg(ref AGteRegD, command, glbSucursalActual, glbFechaActiva); // Debes adaptar este método para que reciba los parámetros

            txtDolar.Text = Convert.ToDecimal(txtDolar.Text).ToString("N6");
            txtEuro.Text = Convert.ToDecimal(txtEuro.Text).ToString("N6");

            // Dólar
            if (AGteRegD == "NO")
            {
                if (!string.IsNullOrEmpty(txtDolar.Text) && txtDolar.Text != "0,000000" && txtDolar.Text != "0.000000")
                {
                    ValDol = txtDolar.Text.Replace(".", "").Replace(",", ".");

                    var resultado = mostrarPregunta("¿Está seguro que desea cambiar la tasa del Dólar?", "CONFIRME");

                    if (resultado != DialogResult.OK)
                    {
                        return;
                    }
                    else
                    {

                       DataSet dsAgregaFactD = _D_TasaSecuencia.AgregarTasaDia(_D_DetalleOrden.TB_PARAMETRO("SucursalId"), ValDol, _D_Inicio.DiaActivo().ToString("yyyy/MM/dd"),"01",TB_USUARIO.COD_USR);

                        if (dsAgregaFactD != null)
                        {
                            if (dsAgregaFactD.Tables[0].Rows[0]["Resultado"].ToString() == "APLICA")
                            {
                                _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "081", TB_USUARIO.COD_EMPLEADO, "La Tasa del Dólar se actualizó correctamente. Tasa Registrada: "+ ValDol);
                                mostrarError("La Tasa del Dolar se actualizó correctamente");                            
                            }
                            else if (dsAgregaFactD.Tables[0].Rows[0]["Resultado"].ToString() == "NO APLICA")
                            {
                                int ValorD = Convert.ToInt32(dsAgregaFactD.Tables[0].Rows[0]["ValorMinMax"]);
                                _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "081", TB_USUARIO.COD_EMPLEADO, "La Tasa no debe ser menor o mayor a " + ValorD + "% de la tasa vigente. Tasa Registrada: " + ValDol);
                                mostrarError("La Tasa no debe ser menor o mayor a " + ValorD + "% de la tasa vigente");

                            }
                        }
                    }
                }
                else
                {
                    mostrarError("La tasa no puede ser guardada con valor 0,000000");
                }
            }

            // Euro
            if (AGteRegE == "NO")
            {
                if (!string.IsNullOrEmpty(txtEuro.Text) && txtEuro.Text != "0,000000" && txtEuro.Text != "0.000000")
                {
                    ValEur = txtEuro.Text.Replace(".", "").Replace(",", ".");
                    
                    var resultado = mostrarPregunta("¿Está seguro que desea cambiar la tasa del Euro?", "CONFIRME");
                    if (resultado != DialogResult.OK)
                    {
                        return;
                    }
                    else
                    {
                        DataSet dsAgregaFactE = _D_TasaSecuencia.AgregarTasaDia(_D_DetalleOrden.TB_PARAMETRO("SucursalId"), ValEur, _D_Inicio.DiaActivo().ToString("yyyy/MM/dd"), "02", TB_USUARIO.COD_USR);

                        if (dsAgregaFactE != null)
                        {
                            if (dsAgregaFactE.Tables[0].Rows[0]["Resultado"].ToString() == "APLICA")
                            {  
                                _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "081", TB_USUARIO.COD_EMPLEADO, "La Tasa del Euro se actualizó correctamente. Tasa Registrada: " + ValEur);
                                mostrarError("La Tasa del Euro se actualizó correctamente");

                            }
                            else if (dsAgregaFactE.Tables[0].Rows[0]["Resultado"].ToString() == "NO APLICA")
                            {
                                int ValorE = Convert.ToInt32(dsAgregaFactE.Tables[0].Rows[0]["ValorMinMax"]);  
                                _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "081", TB_USUARIO.COD_EMPLEADO, "La Tasa no debe ser menor o mayor a " + ValorE + "% de la tasa vigente. Tasa Registrada: " + ValEur);
                                mostrarError("La Tasa no debe ser menor o mayor a " + ValorE + "% de la tasa vigente");

                            }
                        }
                    }
                }
                else
                {
                    mostrarError("La tasa no puede ser guardada con valor 0,000000");          
                }
            }

            Ultima_Tasa_Dia(txtUDolar, txtUEuro, txtUFechaDol, txtUFechaEur, lbRegD, lbRegE);
            txtDolar.Text = "0,000000";
            txtEuro.Text = "0,000000";

         }
         catch (Exception ex)
         {
                mostrarError($"Error en validación de montos: {ex.Message}");
         }
    }

        public bool Fecha_formato(System.Windows.Forms.TextBox TxtCadenaEncriptada, System.Windows.Forms.Label LblTasaDesenc, System.Windows.Forms.Label LblFechaDesenc, System.Windows.Forms.Label LblHoraDesenc)
        {
                // Desencriptar
                DataSet dsEjecutaDesencriptar = _D_TasaSecuencia.EncripDescrip(TxtCadenaEncriptada.Text,"I");
                string Resultado = "";
                int Cadena1 = 0;

                if (dsEjecutaDesencriptar.Tables[0].Rows.Count > 0)
                {
                    Resultado = dsEjecutaDesencriptar.Tables[0].Rows[0][0].ToString();
                    Cadena1 = Resultado.Length;
                    LblTasaDesenc.Text = Resultado.Substring(0, Cadena1 - 11);
                    LblFechaDesenc.Text = Resultado.Substring(Cadena1 - 11, 6);
                    LblHoraDesenc.Text = Resultado.Substring(Cadena1 - 5, 4);
                }

                // Fin Desencriptar 
                string valor1 = LblFechaDesenc.Text.Substring(0, 2) + "/" +
                                LblFechaDesenc.Text.Substring(2, 2) + "/" +
                                LblFechaDesenc.Text.Substring(4, 2);
                string valor2 = LblHoraDesenc.Text.Substring(0, 2) + ":" +
                                LblHoraDesenc.Text.Substring(2, 2) + ":00";

                // Asigna el valor formateado (ajusta el tipo de DateTimeTasa.Value si es necesario)
                DateTimeTasa = Convert.ToDateTime( valor1 + " " + valor2);
                return true;
          
        }

        ///// <summary>
        ///// Valida el formato de una tasa desencriptada y sus componentes (tasa, fecha y hora)
        ///// </summary>
        ///// <param name="TxtCadenaEncriptada">TextBox con la cadena encriptada</param>
        ///// <param name="ValidoDescrip">Referencia a bandera que indica si la descripción es válida</param>
        ///// <param name="ValidoLimiteCambio">Referencia a bandera que indica si se excedió el límite de cambio</param>
        ///// <param name="LblTasaDesenc">Label donde se muestra la tasa desencriptada</param>
        ///// <param name="LblFechaDesenc">Label donde se muestra la fecha desencriptada</param>
        ///// <param name="LblHoraDesenc">Label donde se muestra la hora desencriptada</param>
        ///// <returns>True si el formato es válido, False si hay errores</returns>
        public bool ValidoFormatoTasa2(TextBox TxtCadenaEncriptada, ref bool ValidoDescrip, ref bool ValidoLimiteCambio,
                                     Label LblTasaDesenc, Label LblFechaDesenc, Label LblHoraDesenc)
        {
            try
            {
                // 1. Inicialización y desencriptación
                ValidoDescrip = false;
                ValidoLimiteCambio = false;

                if (string.IsNullOrWhiteSpace(TxtCadenaEncriptada.Text))
                {
                    ValidoDescrip = true;
                    return false;
                }

                // 2. Desencriptar y extraer componentes
                var resultadoDesencriptacion = DesencriptarYExtraerComponentes(TxtCadenaEncriptada.Text);
                if (resultadoDesencriptacion == null)
                {
                    ValidoDescrip = true;
                    return false;
                }

                LblTasaDesenc.Text = resultadoDesencriptacion.Tasa;
                LblFechaDesenc.Text = resultadoDesencriptacion.Fecha;
                LblHoraDesenc.Text = resultadoDesencriptacion.Hora;

                // 3. Validar formato de tasa
                if (!ValidarFormatoTasa(LblTasaDesenc.Text, ref ValidoDescrip))
                {
                    return false;
                }

                // 4. Validar rango de tasa
                var ultimaSecuencia = ObtenerUltimaSecuencia();
                if (ultimaSecuencia != null)
                {
                    ValidarRangoTasa(LblTasaDesenc.Text, ultimaSecuencia.Tasa, ref ValidoDescrip, ref ValidoLimiteCambio);
                }

                // 5. Validar formato de fecha
                if (!ValidarFormatoFecha(LblFechaDesenc.Text, ref ValidoDescrip))
                {
                    return false;
                }

                // 6. Validar formato de hora
                if (!ValidarFormatoHora(LblHoraDesenc.Text, ref ValidoDescrip))
                {
                    return false;
                }

                return !ValidoDescrip;
            }
            catch (Exception ex)
            {
                //// Loggear el error completo
                //mostrarError($"Error en ValidoFormatoTasa. Valor: {ex.Message}");
                //Logger.Error($"Error en ValidoFormatoTasa. Valor: {TxtCadenaEncriptada?.Text}", ex);
                ValidoDescrip = true;
                return false;
            }
        }

        #region Métodos auxiliares

        private class DesencriptacionResult
        {
            public string Tasa { get; set; }
            public string Fecha { get; set; }
            public string Hora { get; set; }
        }

        private DesencriptacionResult DesencriptarYExtraerComponentes(string textoEncriptado)
        {
            var dsResultado = _D_TasaSecuencia.EncripDescrip(textoEncriptado, "I");
            if (dsResultado?.Tables[0]?.Rows.Count == 0) return null;

            string resultado = dsResultado.Tables[0].Rows[0][0].ToString();
            int longitud = resultado.Length;

            return new DesencriptacionResult
            {
                Tasa = resultado.Substring(0, longitud - 11),
                Fecha = resultado.Substring(longitud - 11, 6),
                Hora = resultado.Substring(longitud - 5, 4)
            };
        }

        private bool ValidarFormatoTasa(string tasaText, ref bool validoDescrip)
        {
            if (string.IsNullOrWhiteSpace(tasaText))
            {
                validoDescrip = true;
                return false;
            }

            // Validar longitud mínima
            if (tasaText.Length < 6)
            {
                validoDescrip = true;
                return false;
            }

            // Validar caracteres (solo números y coma decimal)
            foreach (char c in tasaText)
            {
                if (!char.IsDigit(c) && c != ',')
                {
                    validoDescrip = true;
                    return false;
                }
            }

            return true;
        }

        private class UltimaSecuenciaResult
        {
            public decimal Tasa { get; set; }
        }

        private UltimaSecuenciaResult ObtenerUltimaSecuencia()
        {
            var sucursalId = _D_DetalleOrden.TB_PARAMETRO("SucursalId");
            var dsResultado = _D_TasaSecuencia.ObtenerUltSecuencia(sucursalId);
            if (dsResultado?.Tables[0]?.Rows.Count == 0) return null;

            string ultSecCarg = dsResultado.Tables[0].Rows[0][0].ToString();
            var desencriptado = DesencriptarYExtraerComponentes(ultSecCarg);

            if (desencriptado == null || !decimal.TryParse(desencriptado.Tasa.Replace(",","."), NumberStyles.Any,
                CultureInfo.InvariantCulture, out decimal tasa))
            {
                return null;
            }

            return new UltimaSecuenciaResult { Tasa = tasa };
        }

        private void ValidarRangoTasa(string tasaActualText, decimal ultimaTasa, ref bool validoDescrip, ref bool validoLimiteCambio)
        {
            if (!decimal.TryParse(tasaActualText.Replace(',', '.'), NumberStyles.Any,
                CultureInfo.InvariantCulture, out decimal tasaActual))
            {
                validoDescrip = true;
                return;
            }

            decimal valorMinMaxTasa = Convert.ToInt32(_D_DetalleOrden.TB_PARAMETRO("ValorMinMaxTasa"));
            decimal porcentajeMin = ultimaTasa - ((ultimaTasa * valorMinMaxTasa) / 100m);
            decimal porcentajeMax = ultimaTasa + ((ultimaTasa * valorMinMaxTasa) / 100m);

            if (tasaActual < (porcentajeMin / 100m))
            {
                validoDescrip = true;
                validoLimiteCambio = true;
            }

            if (tasaActual > porcentajeMax)
            {
                validoDescrip = true;
                validoLimiteCambio = true;
            }
        }

        private bool ValidarFormatoFecha(string fechaText, ref bool validoDescrip)
        {
            if (string.IsNullOrWhiteSpace(fechaText) || fechaText.Length != 6)
            {
                validoDescrip = true;
                return false;
            }

            // Validar que sean solo dígitos
            foreach (char c in fechaText)
            {
                if (!char.IsDigit(c))
                {
                    validoDescrip = true;
                    return false;
                }
            }

            // Validar día, mes y año
            int dia = int.Parse(fechaText.Substring(0, 2));
            int mes = int.Parse(fechaText.Substring(2, 2));
            int anio = int.Parse(fechaText.Substring(4, 2));

            if (dia < 1 || dia > 31 || mes < 1 || mes > 12 || anio < 20 || anio > 50)
            {
                validoDescrip = true;
                return false;
            }

            return true;
        }

        private bool ValidarFormatoHora(string horaText, ref bool validoDescrip)
        {
            if (string.IsNullOrWhiteSpace(horaText) || horaText.Length != 4)
            {
                validoDescrip = true;
                return false;
            }

            // Validar que sean solo dígitos
            foreach (char c in horaText)
            {
                if (!char.IsDigit(c))
                {
                    validoDescrip = true;
                    return false;
                }
            }

            // Validar hora y minutos
            int hora = int.Parse(horaText.Substring(0, 2));
            int minutos = int.Parse(horaText.Substring(2, 2));

            if (hora < 0 || hora > 23 || minutos < 0 || minutos > 59)
            {
                validoDescrip = true;
                return false;
            }

            return true;
        }

        #endregion
    }
}
