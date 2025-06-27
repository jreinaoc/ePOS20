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

namespace CapaLogica.TasaDia_Logica
{
    public class TasaSecuencia
    {
        private D_TasaSecuencia _D_TasaSecuencia = new D_TasaSecuencia();
        private D_Inicio _D_Inicio = new D_Inicio();
        public readonly StringBuilder stringBuilder = new StringBuilder();


        //private void BtnActivarS_Click(System.Windows.Forms.TextBox TxtCadenaEncriptada, System.Windows.Forms.Label LblTasaDesenc, System.Windows.Forms.Label LblFechaDesenc, System.Windows.Forms.Label LblHoraDesenc)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(TxtCadenaEncriptada.Text))
        //        {
        //            // Validar espacios en blanco
        //            bool ValidoEspacio = false;

        //            for (int i = 0; i < TxtCadenaEncriptada.Text.Length; i++)
        //            {
        //                if (TxtCadenaEncriptada.Text.Substring(i, 1) == " ")
        //                {
        //                    ValidoEspacio = true;
        //                }
        //            }

        //            if (!ValidoEspacio)
        //            {
        //                bool ValidoDescrip = false;
        //                bool ValidoLimiteCambio = false;
        //                ValidoFormatoTasa(TxtCadenaEncriptada.Text, ref ValidoDescrip, ref ValidoLimiteCambio, LblTasaDesenc, LblFechaDesenc, LblHoraDesenc);

        //                if (!ValidoDescrip)
        //                {
        //                    string ValDol, ValEur;

        //                    Autoriz_GteReg_Activar();

        //                    Fecha_formato(Command);

        //                    Usua.ObtenerUsuarioCodigo(glbUsuarioActual, Command);

        //                    if (DateTimeTasa.Value >= glbFechaActiva)
        //                    {
        //                        if (Convert.ToDateTime(DateTimeTasa.Value) < Convert.ToDateTime(ValorParametro("FechaUSecuencia", Command)))
        //                        {
        //                            MessageBox.Show("No se puede realizar la Activación.", "Activación", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //                        }
        //                        else
        //                        {
        //                            if (AGteRegD == "NO")
        //                            {
        //                                if (MessageBox.Show("¿Está seguro que desea Activar la Secuencia Diaria?", "CONFIRME", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
        //                                {
        //                                    return;
        //                                }
        //                                else
        //                                {
        //                                    if (!string.IsNullOrEmpty(TxtCadenaEncriptada.Text))
        //                                    {
        //                                        // Desencriptar
        //                                        DataSet dsEjecutaDesencriptar = ManBD.EjecutaStoreProcedure("SP_EncripDecrip", TxtCadenaEncriptada.Text + "','" + "I", Command);
        //                                        string Resultado = "";
        //                                        int Cadena1 = 0;

        //                                        DataSet dsDigitoVerificador = new DataSet();
        //                                        string DigitoVerificador = "";
        //                                        string ResDigitoVerificador = "";

        //                                        if (dsEjecutaDesencriptar.Tables[0].Rows.Count > 0)
        //                                        {
        //                                            Resultado = dsEjecutaDesencriptar.Tables[0].Rows[0][0].ToString();
        //                                            Cadena1 = Resultado.Length;
        //                                            LblTasaDesenc.Text = Resultado.Substring(0, Cadena1 - 11);
        //                                            LblFechaDesenc.Text = Resultado.Substring(Cadena1 - 11, 6);
        //                                            LblHoraDesenc.Text = Resultado.Substring(Cadena1 - 5, 4);
        //                                            DigitoVerificador = Resultado.Substring(Cadena1 - 1, 1);
        //                                            dsDigitoVerificador = ManBD.EjecutaStoreProcedure("pComparaDigitoVerificador", TxtCadenaEncriptada.Text + "','" + DigitoVerificador, Command);
        //                                            ResDigitoVerificador = dsDigitoVerificador.Tables[0].Rows[0][0].ToString();
        //                                            if (ResDigitoVerificador == "SECUENCIA NO VALIDA")
        //                                            {
        //                                                MessageBox.Show("La secuencia no es valida.", "Secuencia Activación Diaria", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        //                                                return;
        //                                            }
        //                                        }

        //                                        // Fin Desencriptar

        //                                        if (!string.IsNullOrEmpty(LblTasaDesenc.Text))
        //                                        {
        //                                            MessageBox.Show("Se realizará el proceso de actualización.", "Conexión", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

        //                                            for (int x = 1; x < 30; x++)
        //                                                ProgressBar1.Value = x;

        //                                            DataSet DSUpdPrecio = ManBD.EjecutaStoreProcedure(
        //                                                "SP_ACTUALIZAARTICULO_DOLAR",
        //                                                glbSucursalActual + "','" + glbUsuarioActual + "','" + TxtCadenaEncriptada.Text + "','" + LblTasaDesenc.Text + "','" + glbFechaActiva.ToString("yyyy/MM/dd") + "','" + DateTimeAct.Value.ToString("dd/MM/yyyy hh:mm:ss tt") + "','" + DateTimeTasa.Value.ToString("dd/MM/yyyy hh:mm:ss tt"),
        //                                                Command);

        //                                            for (int x = 30; x < 70; x++)
        //                                                ProgressBar1.Value = x;

        //                                            if (DSUpdPrecio.Tables[0].Rows[0][0].ToString() == "SATISFACTORIO")
        //                                            {
        //                                                Control.lblFecUltActSec.Text = ValorParametro("FechaUActivaT", Command);

        //                                                var oFRMControl = new frmControlDatos();
        //                                                oFRMControl.BuscoFechaUltimaSecuencia(Command);

        //                                                var ManDB = new CapaNegocio.ManejoBD();

        //                                                oAuditor.AgregarRegistroAuditor(glbSucursalActual, "083", Usua.CarnetEmpleado, "La Secuencia de Activación Diaria se actualizó correctamente. ", Command);

        //                                                for (int x = 71; x < 100; x++)
        //                                                    ProgressBar1.Value = x;

        //                                                MessageBox.Show("El proceso culmino correctamente.", "Secuencia Activación Diaria", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

        //                                                ProgressBar1.Value = 0;

        //                                                // Envio de Correo
        //                                                string IActivarEmailASD = ValorParametro("ActivarEmailASD", Command);

        //                                                if (IActivarEmailASD == "1")
        //                                                {
        //                                                    string Asunto, Body;

        //                                                    Command = ManBD.SQLCommand();
        //                                                    Transac = ManBD.SQLTransaccion(Command.Connection);
        //                                                    Command.Transaction = Transac;
        //                                                    Command.Parameters.Clear();

        //                                                    var oDatosConfSucursal = new CapaNegocio.ConfiguraSucursal();
        //                                                    var oDatosSucursal = new CapaNegocio.Sucursal();
        //                                                    string lblNombreSucursal;

        //                                                    string strSucursalDeTrabajo = ValorParametro("Sucursal", Command);

        //                                                    oDatosSucursal.ObtenerSucursal(strSucursalDeTrabajo, Command);

        //                                                    lblNombreSucursal = oDatosSucursal.Descripcion();

        //                                                    string valor1 = ValorParametro("FechaUSecuencia", Command);

        //                                                    Asunto = "Sucursal " + glbSucursalActual + " " + lblNombreSucursal + " - Secuencia Activación Diaria";
        //                                                    Body = " Secuencia Activación Diaria -  Tasa: " + LblTasaDesenc.Text + " Fecha: " + valor1;

        //                                                    Enviar_Email(Asunto, Body, Command);
        //                                                }

        //                                                // Blanquear campo de Secuencia al finalizar proceso de activación 
        //                                                TxtCadenaEncriptada.Text = "";
        //                                            }
        //                                            else
        //                                            {
        //                                                LblTasaDesenc.Text = "";
        //                                                LblFechaDesenc.Text = "";
        //                                                LblHoraDesenc.Text = "";
        //                                                MessageBox.Show("Hubo problemas realizando el proceso.", "Secuencia Activación Diaria", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        //                                                ProgressBar1.Value = 0;
        //                                                oAuditor.AgregarRegistroAuditor(glbSucursalActual, "083", Usua.CarnetEmpleado, "La Secuencia de Activación Diaria no se actualizó correctamente. ", Command);
        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            ProgressBar1.Value = 0;
        //                                            MessageBox.Show("El valor introducido no es correcto. Debe solicitar uno nuevo.", "Secuencia Activación Diaria", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                                            LblTasaDesenc.Text = "";
        //                                            LblFechaDesenc.Text = "";
        //                                            LblHoraDesenc.Text = "";
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                            ProgressBar1.Value = 0;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        ProgressBar1.Value = 0;
        //                        LblTasaDesenc.Text = "";
        //                        LblFechaDesenc.Text = "";
        //                        LblHoraDesenc.Text = "";
        //                        MessageBox.Show("La fecha de la secuencia debe ser mayor al día activo. Debe solicitar uno nuevo.", "Secuencia Activación Diaria", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                    }
        //                }
        //                else
        //                {
        //                    if (ValidoLimiteCambio)
        //                    {
        //                        ProgressBar1.Value = 0;
        //                        LblTasaDesenc.Text = "";
        //                        LblFechaDesenc.Text = "";
        //                        LblHoraDesenc.Text = "";
        //                        MessageBox.Show("El valor introducido supera el % permitido para actualizar la secuencia. Debe solicitar uno nuevo.", "Secuencia Activación Diaria", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                    }
        //                    else
        //                    {
        //                        ProgressBar1.Value = 0;
        //                        LblTasaDesenc.Text = "";
        //                        LblFechaDesenc.Text = "";
        //                        LblHoraDesenc.Text = "";
        //                        MessageBox.Show("El valor introducido no es correcto. Debe solicitar uno nuevo.", "Secuencia Activación Diaria", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            ProgressBar1.Value = 0;
        //            LblTasaDesenc.Text = "";
        //            LblFechaDesenc.Text = "";
        //            LblHoraDesenc.Text = "";
        //            MessageBox.Show("El valor introducido no es correcto. Debe solicitar uno nuevo.", "Secuencia Activación Diaria", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // MensajeError.MuestroMensaje("Error en la función", "FrmDesencriptar", "Por favor comunicarse con el Dpto. de Sistemas y reportar el siguiente error: ", ex.Message, CapaNegocio.MensajesGenerales.TiposIconos.IconoError, glbUsuarioActual);
        //        MensajeError.ShowDialog();
        //    }
        //}

        //private bool ValidoFormatoTasa(string Valor, ref bool ValidoDescrip, ref bool ValidoLimiteCambio, System.Windows.Forms.Label LblTasaDesenc, System.Windows.Forms.Label LblFechaDesenc, System.Windows.Forms.Label LblHoraDesenc)
        //{
        //    try
        //    {
        //        // Desencriptar
        //        string Resultado = "";
        //        int Cadena1 = 0;
        //        string UResultado = "";
        //        int UCadena1 = 0;
        //        int i;
        //        int VaMinMaxTasa = 0;
        //        int PorcMinTasa = 0;
        //        int PorcMaxTasa = 0;
        //        int UTasaDesenc = 0;
        //        string UFechaDesenc = "";
        //        string UHoraDesenc = "";

        //        DataSet dsEjecutaDesencriptar = ManBD.EjecutaStoreProcedure("SP_EncripDecrip", TxtCadenaEncriptada.Text + "','" + "I", Command);

        //        if (dsEjecutaDesencriptar.Tables[0].Rows.Count > 0)
        //        {
        //            Resultado = dsEjecutaDesencriptar.Tables[0].Rows[0][0].ToString();
        //            Cadena1 = Resultado.Length;
        //            LblTasaDesenc.Text = Resultado.Substring(0, Cadena1 - 11);
        //            LblFechaDesenc.Text = Resultado.Substring(Cadena1 - 11, 6);
        //            LblHoraDesenc.Text = Resultado.Substring(Cadena1 - 5, 4);
        //        }

        //        ValidoDescrip = false;

        //        // FORMATO DE LA TASA

        //        // Verifico que no este vacio
        //        if (LblTasaDesenc.Text == "")
        //        {
        //            ValidoDescrip = true;
        //        }

        //        // Validar espacios en blanco
        //        for (i = 0; i < LblTasaDesenc.Text.Length; i++)
        //        {
        //            if (LblTasaDesenc.Text.Substring(i, 1) == " ")
        //            {
        //                ValidoDescrip = true;
        //            }
        //        }

        //        // Verifico el largo del campo, debe ser al menos 6 caracteres
        //        if (LblTasaDesenc.Text.Length < 6)
        //        {
        //            ValidoDescrip = true;
        //        }

        //        // Verifico que sean numeros
        //        if (!Datos.ValidoNumeros(LblTasaDesenc.Text))
        //        {
        //            ValidoDescrip = true;
        //        }

        //        // Verifico que sean solo números o coma
        //        for (i = 0; i < LblTasaDesenc.Text.Length; i++)
        //        {
        //            char c = LblTasaDesenc.Text[i];
        //            if (!char.IsDigit(c) && c != ',')
        //            {
        //                ValidoDescrip = true;
        //            }
        //        }

        //        // Verifico que el valor esté dentro del rango permitido

        //        // Busco ultima secuencia cargada
        //        string UltSecCarg = "";

        //        DataSet dsEjecutaUltima = ManBD.EjecutaStoreProcedure("pGet_UltSecuencia", glbSucursalActual, Command);

        //        if (dsEjecutaUltima.Tables[0].Rows.Count > 0)
        //        {
        //            UltSecCarg = dsEjecutaUltima.Tables[0].Rows[0][0].ToString();
        //        }

        //        DataSet dsEjecutaDesencriptar2 = ManBD.EjecutaStoreProcedure("SP_EncripDecrip", UltSecCarg + "','" + "I", Command);

        //        if (dsEjecutaDesencriptar2.Tables[0].Rows.Count > 0)
        //        {
        //            UResultado = dsEjecutaDesencriptar2.Tables[0].Rows[0][0].ToString();
        //            UCadena1 = UResultado.Length;
        //            UTasaDesenc = int.Parse(UResultado.Substring(0, UCadena1 - 11));
        //            UFechaDesenc = UResultado.Substring(UCadena1 - 11, 6);
        //            UHoraDesenc = UResultado.Substring(UCadena1 - 5, 4);
        //        }

        //        // rango permitido
        //        VaMinMaxTasa = Convert.ToInt32(ValorParametro("ValorMinMaxTasa", Command));

        //        PorcMinTasa = UTasaDesenc - ((UTasaDesenc * VaMinMaxTasa) / 100);
        //        PorcMaxTasa = UTasaDesenc + ((UTasaDesenc * VaMinMaxTasa) / 100);

        //        decimal tasaDesenc = 0;
        //        decimal.TryParse(LblTasaDesenc.Text.Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out tasaDesenc);

        //        if (tasaDesenc < (PorcMinTasa / 100m))
        //        {
        //            ValidoDescrip = true;
        //            ValidoLimiteCambio = true;
        //        }

        //        if (tasaDesenc > PorcMaxTasa)
        //        {
        //            ValidoDescrip = true;
        //            ValidoLimiteCambio = true;
        //        }

        //        // FORMATO DE LA FECHA

        //        // Verifico que no este vacio
        //        if (LblFechaDesenc.Text == "")
        //        {
        //            ValidoDescrip = true;
        //        }

        //        // Verifico el largo del campo, debe ser de 6 caracteres
        //        if (LblFechaDesenc.Text.Length != 6)
        //        {
        //            ValidoDescrip = true;
        //        }

        //        bool TFecha = false;

        //        // Verifico que sean numeros
        //        for (i = 0; i < LblFechaDesenc.Text.Length; i++)
        //        {
        //            char c = LblFechaDesenc.Text[i];
        //            if (!char.IsDigit(c))
        //            {
        //                ValidoDescrip = true;
        //                TFecha = true;
        //            }
        //        }

        //        if (!TFecha)
        //        {
        //            if (!Datos.ValidoNumeros(LblFechaDesenc.Text))
        //            {
        //                ValidoDescrip = true;
        //            }

        //            // Verifico que el dia tenga numero valido 
        //            int dia = int.Parse(LblFechaDesenc.Text.Substring(0, 2));
        //            if (dia < 1 || dia > 31)
        //            {
        //                ValidoDescrip = true;
        //            }

        //            // Verifico que el mes tenga numero valido 
        //            int mes = int.Parse(LblFechaDesenc.Text.Substring(2, 2));
        //            if (mes < 1 || mes > 12)
        //            {
        //                ValidoDescrip = true;
        //            }

        //            // Verifico que el año tenga numero valido
        //            int anio = int.Parse(LblFechaDesenc.Text.Substring(4, 2));
        //            if (anio < 20 || anio > 50)
        //            {
        //                ValidoDescrip = true;
        //            }
        //        }

        //        // FORMATO DE LA HORA

        //        // Verifico que no este vacio
        //        if (LblHoraDesenc.Text == "")
        //        {
        //            ValidoDescrip = true;
        //        }

        //        // Verifico el largo del campo, debe ser de 4 caracteres
        //        if (LblHoraDesenc.Text.Length != 4)
        //        {
        //            ValidoDescrip = true;
        //        }

        //        bool THora = false;

        //        // Verifico que sean numeros
        //        for (i = 0; i < LblHoraDesenc.Text.Length; i++)
        //        {
        //            char c = LblHoraDesenc.Text[i];
        //            if (!char.IsDigit(c))
        //            {
        //                ValidoDescrip = true;
        //                THora = true;
        //            }
        //        }

        //        if (!THora)
        //        {
        //            if (!Datos.ValidoNumeros(LblHoraDesenc.Text))
        //            {
        //                ValidoDescrip = true;
        //            }

        //            // Verifico que la hora tenga numero valido
        //            int hora = int.Parse(LblHoraDesenc.Text.Substring(0, 2));
        //            if (hora < 1 || hora > 24)
        //            {
        //                ValidoDescrip = true;
        //            }

        //            // Verifico que los minutos tengan numero valido
        //            int minutos = int.Parse(LblHoraDesenc.Text.Substring(2, 2));
        //            if (minutos < 0 || minutos > 59)
        //            {
        //                ValidoDescrip = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Manejo de error opcional
        //    }
        //    return !ValidoDescrip;
        //}

        //public bool Fecha_formato(ref SqlCommand sqlCom, System.Windows.Forms.Label LblTasaDesenc, System.Windows.Forms.Label LblFechaDesenc, System.Windows.Forms.Label LblHoraDesenc)
        //{
        //    try
        //    {
        //        // Desencriptar
        //        DataSet dsEjecutaDesencriptar = ManBD.EjecutaStoreProcedure("SP_EncripDecrip", TxtCadenaEncriptada.Text + "','" + "I", Command);
        //        string Resultado = "";
        //        int Cadena1 = 0;

        //        if (dsEjecutaDesencriptar.Tables[0].Rows.Count > 0)
        //        {
        //            Resultado = dsEjecutaDesencriptar.Tables[0].Rows[0][0].ToString();
        //            Cadena1 = Resultado.Length;
        //            LblTasaDesenc.Text = Resultado.Substring(0, Cadena1 - 11);
        //            LblFechaDesenc.Text = Resultado.Substring(Cadena1 - 11, 6);
        //            LblHoraDesenc.Text = Resultado.Substring(Cadena1 - 5, 4);
        //        }

        //        // Fin Desencriptar 
        //        string valor1 = LblFechaDesenc.Text.Substring(0, 2) + "/" +
        //                        LblFechaDesenc.Text.Substring(2, 2) + "/" +
        //                        LblFechaDesenc.Text.Substring(4, 2);
        //        string valor2 = LblHoraDesenc.Text.Substring(0, 2) + ":" +
        //                        LblHoraDesenc.Text.Substring(2, 2) + ":00";

        //        // Asigna el valor formateado (ajusta el tipo de DateTimeTasa.Value si es necesario)
        //        DateTimeTasa.Value = valor1 + " " + valor2;
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "", MessageBoxButtons.OK);
        //        return false;
        //    }
        //}
    }
}
