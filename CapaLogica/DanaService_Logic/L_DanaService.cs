using CapaDatos.DanaService_Datos;
using CapaDatos.DetalleOrden_Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica.DanaService_Logic
{
    public class L_DanaService
    {
        //El uso de la clase StringBuilder nos ayudara a devolver los mensajes de las validaciones
        public readonly StringBuilder stringBuilder = new StringBuilder();

        private D_Dana _D_Dana = new D_Dana();
        private D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();

        public string DanaService(string CI, string Nacionalidad, string Accion)
        {
            try
            {
                DataTable dtConfigDana;
                DataSet dsTlfValidoDANA;
                string Telefono;
                string ParamDana1;
                string ParamDana2;
                string ParamDana3;
                string ParamDana4;

                string IDCompañia;
                var Login = default(string);
                var Password = default(string);
                long IdCampañaFactura;
                // Dim IdCampañaNoche As String
                string IdCampañaDia;
                var IdCampañaRecepcionOS = default(string);
                var IdCampañaGracias = default(string);
                string DetCampañaDia;
                var DetCampañaRecepcionOS = default(string);
                var DetCampañaGracias = default(string);
                string HoraInicioDia = "";
                string HoraFinDia = "";

                var IdCampañaSatisfaccion = default(string);
                var DetCampañaSatisfaccion = default(string);

                var CamposP = default(string);
                var Detalle = default(string);
                string strComand;
                string ruta;
                string rutaV;
                var Campana = default(string);
                var DetCampana = default(string);
                int x; // = dtConfigDana.Rows.Count()

                DataSet dsSMS = _D_Dana.ServicioDanaActivo("ServicioDANAActivo%");
                if (dsSMS.Tables[0].Rows.Count > 0)
                {
                    if (dsSMS.Tables[0].Rows[0]["Valor"].ToString() == "1")
                    {

                        dsTlfValidoDANA = _D_Dana.ValidoDANA(Nacionalidad, CI);

                        if (dsTlfValidoDANA.Tables[0].Rows.Count > 0)
                        {
                            Telefono = dsTlfValidoDANA.Tables[0].Rows[0]["TLF_Cod"].ToString() + dsTlfValidoDANA.Tables[0].Rows[0]["TLF_Numero"].ToString();
                            DataTable DT = _D_Dana.TelefonoCliente(Nacionalidad, CI);
                            DataTable dtCliente = _D_DetalleOrden.BucarTB_CTEPPAL(CI, Nacionalidad);

                            //Datos.Add("NOMBRE", Cliente.PrimerNombre.Trim());
                            //Datos.Add("TELEFONO", Telefono);

                            ParamDana1 = dtCliente.Rows[0]["CTE_PNombre"].ToString();
                            ParamDana2 = Telefono;
                            ParamDana3 = "";
                            ParamDana4 = "";

                            DataSet Dtset = _D_Dana.ServicioDanaActivo("%");
                            dtConfigDana = Dtset.Tables[0];

                            var loopTo = dtConfigDana.Rows.Count - 1;
                            for (x = 0; x <= loopTo; x++)
                            {
                                switch (dtConfigDana.Rows[x]["parametrodana"].ToString())
                                {
                                    case "IDCompañia":
                                        {
                                            IDCompañia = dtConfigDana.Rows[x]["valor"].ToString();
                                            break;
                                        }
                                    case "Login":
                                        {
                                            Login = dtConfigDana.Rows[x]["valor"].ToString();
                                            break;
                                        }
                                    case "Password":
                                        {
                                            Password = dtConfigDana.Rows[x]["valor"].ToString();
                                            break;
                                        }
                                    case "IdCampañaDia":
                                        {
                                            IdCampañaDia = dtConfigDana.Rows[x]["valor"].ToString();
                                            DetCampañaDia = "IdCampañaDia";
                                            break;
                                        }
                                    // Case "IdCampañaNoche"
                                    // IdCampañaNoche = dtConfigDana.Rows(x).Item("valor")
                                    case "IdCampañaRecepcionOS":
                                        {
                                            IdCampañaRecepcionOS = dtConfigDana.Rows[x]["valor"].ToString();
                                            DetCampañaRecepcionOS = "IdCampañaRecepcionOS";
                                            break;
                                        }
                                    case "IdCampañaGracias":
                                        {
                                            IdCampañaGracias = dtConfigDana.Rows[x]["valor"].ToString();
                                            DetCampañaGracias = "IdCampañaGracias";
                                            break;
                                        }
                                    case "HoraInicioDia":
                                        {
                                            HoraInicioDia = dtConfigDana.Rows[x]["valor"].ToString();
                                            break;
                                        }
                                    case "HoraFinDia":
                                        {
                                            HoraFinDia = dtConfigDana.Rows[x]["valor"].ToString();
                                            break;
                                        }
                                    case "IdCampañaSatisfaccion":
                                        {
                                            IdCampañaSatisfaccion = dtConfigDana.Rows[x]["valor"].ToString();
                                            DetCampañaSatisfaccion = "IdCampañaSatisfaccion";
                                            break;
                                        }
                                }
                            }

                            switch (Accion ?? "")
                            {
                                case "OrdenFacturada":
                                    {
                                        Campana = IdCampañaGracias;
                                        DetCampana = DetCampañaGracias;
                                        break;
                                    }
                                case "OrdenRecibida": // Or "Reenviar"
                                    {
                                        Campana = IdCampañaRecepcionOS;
                                        DetCampana = DetCampañaRecepcionOS;
                                        break;
                                    }
                                // eh: 23/06/2021 
                                case "OrdenEntrega":
                                    {
                                        Campana = IdCampañaSatisfaccion;
                                        DetCampana = DetCampañaSatisfaccion;
                                        break;
                                    }
                            }

                            // 'RUTA

                            rutaV = _D_Dana.Parametro("RutaEnvioDana");
                            ruta = rutaV + "WebApiEnvioSMSDANA.exe ";

                            // ' CONSULTAR PARAMETROS CONFIGURADOS Y ARMAR DETALLE DEL MENSAJE
                            DataSet dsDana = _D_Dana.ParametroDana(DetCampana);
                            foreach (DataRow dr in dsDana.Tables[0].Rows)
                            {
                                //if (!(dr["ParametroDana"] is DBNull))  
                                if (!(dr[0] is DBNull))
                                {
                                    CamposP = dr["Campos"].ToString();
                                }
                            }

                            if (!string.IsNullOrEmpty(CamposP))
                            {
                                int letra;
                                var strCodParametro = default(string);
                                var cont = default(int);

                                Detalle = @"{\""";
                                var loopTo1 = CamposP.Length - 1;
                                for (letra = 0; letra <= loopTo1; letra++)
                                {
                                    if (CamposP.Substring(letra, 1) != ",")
                                    {
                                        strCodParametro = strCodParametro + CamposP.Substring(letra, 1);
                                    }
                                    else
                                    {
                                        cont = cont + 1;
                                        if (cont == 1)
                                        {
                                            Detalle = Detalle + strCodParametro + @"\"":\""" + ParamDana1;
                                            strCodParametro = "";
                                        }
                                        else if (cont == 2 & !string.IsNullOrEmpty(ParamDana2))
                                        {
                                            Detalle = Detalle + @"\"";\""" + strCodParametro + @"\"":\""" + ParamDana2;
                                            strCodParametro = "";
                                        }
                                        else if (cont == 3 & !string.IsNullOrEmpty(ParamDana3))
                                        {
                                            Detalle = Detalle + @"\"";\""" + strCodParametro + @"\"":\""" + ParamDana3;
                                            strCodParametro = "";
                                        }
                                        else if (cont == 4 & !string.IsNullOrEmpty(ParamDana4))
                                        {
                                            Detalle = Detalle + @"\"";\""" + strCodParametro + @"\"":\""" + ParamDana4;
                                            strCodParametro = "";
                                        }
                                    }
                                }
                                Detalle = Detalle + @"\""}";
                            }


                            string HoraSistema;
                            HoraSistema = DateTime.Now.ToString("HH:mm");
                            if (DateTime.Parse(HoraSistema) > DateTime.Parse(HoraInicioDia) | DateTime.Parse(HoraSistema) < DateTime.Parse(HoraFinDia))
                            {
                                strComand = ruta + "POST" + " " + Login + " " + Password + " " + Detalle + " " + Campana;
                                string Resp = EjecutarEXE(ruta, "POST" + " " + Login + " " + Password + " " + Detalle + " " + Campana);

                                //prueba Ejecutar servicio
                                // string Resp = EjecutarEXE1(ruta, "POST" + " " + "administrador@opticacaroni" + " " + "CaroniDana2020*" + " " + @"{\" + @"""ENCUESTASATIS_NOMBRE\""" + @":\""" + @"PRUEBA\""" + @",\""" + @"ENCUESTASATIS_CELULAR\""" + @":\""" + @"04129117765\""" + "}" + " " + "253638");

                                return Resp;
                            }
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error en la función, DanaService, Por favor comunicarse con el Dpto de Sistemas y reportar el siguiente error:", ex.Message));
                return null;
            }
            finally
            {

            }
        }

        //otra forma de ejecutar el archivo exe
        public string EjecutarEXE1(string strExe, string strArgumentos)
        {
            try
            {

                using (Process compiler = new Process())
                {
                    //// Armar el proceso a ejecutar
                    //System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(strExe);
                    //compiler.startInfo.WindowStyle = ProcessWindowStyle.Minimized;

                    compiler.StartInfo.FileName = strExe;

                    // argumento Nuevo
                    compiler.StartInfo.Arguments = strArgumentos;

                    //// Para poder manupular la salida indicamos que no se ejecute el shell
                    compiler.StartInfo.UseShellExecute = false;

                    // Por definicion (o mejor dicho codificacion) debemos ajustarnos a los que nos dice "San MSDN" 
                    // (...)UseShellExecute debe ser true si se desea establecer ErrorDialog en true(...)
                    compiler.StartInfo.ErrorDialog = false;

                    //// Sin ventana...
                    //startInfo.CreateNoWindow = true;

                    // Deseamos manipular la salida del proceso, para ello debemos establecer que se redirija la salida
                    compiler.StartInfo.RedirectStandardOutput = true;

                    compiler.Start();

                    Console.WriteLine(compiler.StandardOutput.ReadToEnd());

                    compiler.WaitForExit();
                    return "OK";
                }

            }

            // RespEXE1.Substring(0, 2)
            catch (Exception ex)
            {
                string RespEXE2 = (ex.Message);
                return RespEXE2;
            }
        }

        public string EjecutarEXE(string strExe, string strArgumentos)
        {

            // Armar el proceso a ejecutar
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(strExe, strArgumentos);

            // Para poder manupular la salida indicamos que no se ejecute el shell
            startInfo.UseShellExecute = false;

            // Por definicion (o mejor dicho codificacion) debemos ajustarnos a los que nos dice "San MSDN" 
            // (...)UseShellExecute debe ser true si se desea establecer ErrorDialog en true(...)
            startInfo.ErrorDialog = false;

            // Sin ventana...
            startInfo.CreateNoWindow = true;

            // Deseamos manipular la salida del proceso, para ello debemos establecer que se redirija la salida
            startInfo.RedirectStandardOutput = true;

            try
            {
                System.Diagnostics.Process p = System.Diagnostics.Process.Start(startInfo);

                // Leemos la salida (objeto StreamReader)
                System.IO.StreamReader sr = p.StandardOutput;
                string cadenaSalida = sr.ReadToEnd();
                sr.Close();

                // La visualizamos en el textbox. Un ejemplo basico ;)...
                string RespEXE1 = cadenaSalida.Substring(0, 2);
                return RespEXE1;
            }
            // RespEXE1.Substring(0, 2)
            catch (Exception ex)
            {
                string RespEXE2 = (ex.Message);
                return RespEXE2;
            }
        }

    }
}
