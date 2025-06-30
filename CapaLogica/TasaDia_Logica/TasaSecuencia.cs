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

namespace CapaLogica.TasaDia_Logica
{
    public class TasaSecuencia
    {
        private D_TasaSecuencia _D_TasaSecuencia = new D_TasaSecuencia();
        private D_Inicio _D_Inicio = new D_Inicio();
        public readonly StringBuilder stringBuilder = new StringBuilder();
        public DateTime DateTimeTasa;
        private D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        private D_Anulacion _D_Anulacion = new D_Anulacion();
        private void BtnActivarS_Click(System.Windows.Forms.TextBox TxtCadenaEncriptada, System.Windows.Forms.Label LblTasaDesenc, System.Windows.Forms.Label LblFechaDesenc, System.Windows.Forms.Label LblHoraDesenc, System.Windows.Forms.ProgressBar ProgressBar1,
        string AGteRegD, Func<string, string, DialogResult> mostrarPregunta, Action<string> mostrarError, ValidacionEstucheDTO datosEstuche)
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
                        ValidoFormatoTasa(TxtCadenaEncriptada, ref ValidoDescrip, ref ValidoLimiteCambio, LblTasaDesenc, LblFechaDesenc, LblHoraDesenc);

                        if (!ValidoDescrip)
                        {
                            string ValDol, ValEur;

                            //Autoriz_GteReg_Activar();

                            Fecha_formato(TxtCadenaEncriptada, LblTasaDesenc,  LblFechaDesenc, LblHoraDesenc);

                            if (DateTimeTasa >= (_D_Inicio.DiaActivo()))
                            {
                                if (Convert.ToDateTime(DateTimeTasa) < Convert.ToDateTime(_D_DetalleOrden.TB_PARAMETRO("FechaUSecuencia")))
                                {
                                    mostrarError("No se puede realizar la Activación.");
                                }
                                else
                                {
                                    if (AGteRegD == "NO")
                                    {
                                        var resultado = mostrarPregunta("¿Está seguro que desea Activar la Secuencia Diaria?", "CONFIRME");

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
                                                        mostrarError("La secuencia no es valida.");
                                                        return;
                                                    }
                                                }

                                                // Fin Desencriptar

                                                if (!string.IsNullOrEmpty(LblTasaDesenc.Text))
                                                {
                                                    mostrarError("Se realizará el proceso de actualización.");

                                                    for (int x = 1; x < 30; x++)
                                                        ProgressBar1.Value = x;

                                                    DataSet DSUpdPrecio = _D_TasaSecuencia.ActualizarArtDolar(_D_DetalleOrden.TB_PARAMETRO("SucursalId"),TB_USUARIO.COD_USR, TxtCadenaEncriptada.Text, LblTasaDesenc.Text, _D_Inicio.DiaActivo(), DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"),DateTimeTasa.ToString("dd/MM/yyyy hh:mm:ss tt"));

                                                    for (int x = 30; x < 70; x++)
                                                        ProgressBar1.Value = x;

                                                    if (DSUpdPrecio.Tables[0].Rows[0][0].ToString() == "SATISFACTORIO")
                                                    {
                                                        //lblFecUltActSec.Text = _D_DetalleOrden.TB_PARAMETRO("FechaUActivaT");

                                                        _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "083", TB_USUARIO.COD_EMPLEADO, "La Secuencia de Activación Diaria se actualizó correctamente. ");

                                                        for (int x = 71; x < 100; x++)
                                                            ProgressBar1.Value = x;

                                                        mostrarError("El proceso culmino correctamente.");

                                                        ProgressBar1.Value = 0;

                                                        // Envio de Correo
                                                        string IActivarEmailASD = _D_DetalleOrden.TB_PARAMETRO("ActivarEmailASD");

                                                        if (IActivarEmailASD == "1")
                                                        {
                                                            

                                                            string strSucursalDeTrabajo = _D_DetalleOrden.TB_PARAMETRO("Sucursal");

                                                            string NombreSucursal = _D_DetalleOrden.Nombre_Surculsal_PagoMovil(strSucursalDeTrabajo,null);

                                                            string valor1 = _D_DetalleOrden.TB_PARAMETRO("FechaUSecuencia");

                                                            var Correo = FormatoCorreoSecuencia(strSucursalDeTrabajo, NombreSucursal, LblTasaDesenc.Text, valor1);

                                                            EnviarCorreo(Correo.Asunto, Correo.CuerpoMensaje);

                                                        }

                                                        // Blanquear campo de Secuencia al finalizar proceso de activación 
                                                        TxtCadenaEncriptada.Text = "";
                                                    }
                                                    else
                                                    {
                                                        LblTasaDesenc.Text = "";
                                                        LblFechaDesenc.Text = "";
                                                        LblHoraDesenc.Text = "";
                                                        mostrarError("Hubo problemas realizando el proceso.");
                                                        ProgressBar1.Value = 0;
                                                        _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "083", TB_USUARIO.COD_EMPLEADO, "La Secuencia de Activación Diaria no se actualizó correctamente. ");
                                                    }
                                                }
                                                else
                                                {
                                                    ProgressBar1.Value = 0;
                                                    mostrarError("El valor introducido no es correcto. Debe solicitar uno nuevo.");
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
                            else
                            {
                                ProgressBar1.Value = 0;
                                LblTasaDesenc.Text = "";
                                LblFechaDesenc.Text = "";
                                LblHoraDesenc.Text = "";
                                mostrarError("La fecha de la secuencia debe ser mayor al día activo. Debe solicitar uno nuevo.");
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
                                mostrarError("El valor introducido supera el % permitido para actualizar la secuencia. Debe solicitar uno nuevo.");
                            }
                            else
                            {
                                ProgressBar1.Value = 0;
                                LblTasaDesenc.Text = "";
                                LblFechaDesenc.Text = "";
                                LblHoraDesenc.Text = "";
                                mostrarError("El valor introducido no es correcto. Debe solicitar uno nuevo.");
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
                    mostrarError("El valor introducido no es correcto. Debe solicitar uno nuevo.");
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
                int VaMinMaxTasa = 0;
                int PorcMinTasa = 0;
                int PorcMaxTasa = 0;
                int UTasaDesenc = 0;
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
                    UTasaDesenc = int.Parse(UResultado.Substring(0, UCadena1 - 11));
                    UFechaDesenc = UResultado.Substring(UCadena1 - 11, 6);
                    UHoraDesenc = UResultado.Substring(UCadena1 - 5, 4);
                }

                // rango permitido
                VaMinMaxTasa = Convert.ToInt32(_D_DetalleOrden.TB_PARAMETRO("ValorMinMaxTasa"));

                PorcMinTasa = UTasaDesenc - ((UTasaDesenc * VaMinMaxTasa) / 100);
                PorcMaxTasa = UTasaDesenc + ((UTasaDesenc * VaMinMaxTasa) / 100);

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
    }
}
