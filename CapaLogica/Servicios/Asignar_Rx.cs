using CapaDatos.CargarOrdenes_Datos;
using CapaDatos.Conexion;
using CapaDatos.Inicio_Datos;
using CapaEntidades;
using CapaLogica.ListaFactura_Logica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica.Servicios
{
    public class Asignar_Rx
    {

        private readonly L_ListaFacturas _logicaFacturas = new L_ListaFacturas();
        private D_Articulos _D_Articulo = new D_Articulos();
        private List<TB_TRABAJO> _TRABAJO = new List<TB_TRABAJO>();
        private D_Inicio _D_Inicio = new D_Inicio();
        public readonly StringBuilder stringBuilder = new StringBuilder();


        public void AsignarRx(string GlbCodDetVta, string OSaModificar, string sucursal, string nacio, string cediden, string Numero_Orden)
        {
            stringBuilder.Clear();
            Conexion cn = new Conexion();
            SqlConnection connection = cn.LeerCadena();
            SqlCommand command = connection.CreateCommand();
            SqlTransaction transaction;
            transaction = connection.BeginTransaction();
            command.Connection = connection;
            command.Transaction = transaction;
            command.Parameters.Clear();
            command.CommandTimeout = 300000;

            try
            { 

            if (GlbCodDetVta == "08" && !string.IsNullOrEmpty(OSaModificar))
            {
                var Lista_Trabajo = _D_Articulo.ObtenerTrabajo(sucursal, nacio, cediden, Numero_Orden, command);  // Trae el detalle del articulo 
                _TRABAJO.Clear(); // Limpiar la lista para evitar duplicados
                _TRABAJO.AddRange(Lista_Trabajo); // Agregar los datos obtenidos

                // Obtener artículos de la OS
                DataSet dsOS = _D_Articulo.ArticulosOS(OSaModificar, sucursal, command);

                var (fecha, hora) = FechaOfrecida(
                    dsOS.Tables[0].Rows[0]["CRISTALD"].ToString(),
                    dsOS.Tables[0].Rows[0]["MONTURA"].ToString(),
                    dsOS.Tables[0].Rows[0]["COLOR"].ToString(),
                    dsOS.Tables[0].Rows[0]["AR"].ToString(), sucursal);

                string color = dsOS.Tables[0].Rows[0]["COLOR"].ToString();
                string cristalD = dsOS.Tables[0].Rows[0]["CRISTALD"].ToString();
                string cristalI = dsOS.Tables[0].Rows[0]["CRISTALI"].ToString();
                string Examen = "1";

                //if (!VerificoParametrosCristales(_TRABAJO, nacio, cediden, Examen, cristalD, cristalI, color == "0" ? "NO" : "SI"))
                //    return;

                if (!VerificoRangoDiametroCristales(_TRABAJO, nacio, cediden, Examen, cristalD, cristalI, dsOS.Tables[0].Rows[0]["MONTURA"].ToString(), sucursal))
                    return;

                var trabajo = _TRABAJO.FirstOrDefault();
                // Modificar trabajo
                DataSet dsModificoTrabajo = _D_Articulo.MODIFICATB_TRABAJO(
    trabajo.T_NumOrdserv,                                 // @NUMOS
    trabajo.T_EXAMEN?.ToString(),                         // @EXAM
    trabajo.T_HORIZONTAL?.ToString(),                     // @HORIZ
    trabajo.T_VERTICAL?.ToString(),                       // @VERT
    trabajo.T_MAXIMA?.ToString(),                         // @MAX
    trabajo.T_PUENTE?.ToString(),                         // @PTE
    trabajo.T_DISTANCIAVERTICE?.ToString(),               // @DISVERT
    trabajo.T_ANGULOPANTOSCOPICO?.ToString(),             // @ANPANT
    trabajo.T_ANGULOFACIAL?.ToString(),                   // @ANFAC
    trabajo.T_ALTD?.ToString(),                           // @ALTD
    trabajo.T_ALTI?.ToString(),                           // @ALTI
    trabajo.T_OJO,                                        // @OJO
    trabajo.T_TIPOVISIOND,                                // @TVISD
    trabajo.T_TIPOVISIONI,                                // @TVISI
    trabajo.T_LABORATORIO,                                // @LAB
    trabajo.T_SERVICIO,                                   // @SERV
    trabajo.T_HORAOFRECIDO,                               // @HOFRE
    trabajo.T_FECHAOFRECIDO,                              // @FOFRE
    trabajo.Cod_DetVta,                                   // @CODDETV
    trabajo.TipoExamen,                                   // @TEXAM
    trabajo.USER_CREA,                                    // @USER
    trabajo.T_SUCURSAL                                    // @SUC
);
                if (dsModificoTrabajo.Tables[0].Rows[0][0].ToString() == "SATISFACTORIO")
                {
                    // Modificar OS
                    DataSet dsModificoOS = _D_Articulo.ModificaTbCaOrdSerRx(trabajo.T_NumOrdserv, trabajo.T_EXAMEN?.ToString(), TB_CAORDSER.Cod_Laboratorio, TB_CAORDSER.Cod_Servicio, fecha, hora,
                        (GlbCodDetVta == "08" ? "01" : "02"),TB_USUARIO.COD_USR, sucursal);

                    if (dsModificoOS.Tables[0].Rows[0][0].ToString() == "SATISFACTORIO")
                    {
                        command.Transaction.Commit();
                        // reporte de orden 

                    }
                }
                else
                {
                    command.Transaction.Rollback();
                    stringBuilder.Append("Se produjo un error mientras se modificaba la Orden");
                }
            }
            }
            catch (Exception ex)
            {
                command.Transaction.Rollback();
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
            }
        }
        public bool VerificoRangoDiametroCristales(List<TB_TRABAJO> Trabajo, string nacionalidad, string cedula, string Examen, string CristalD, string CristalI, string Montura, string sucursal, SqlCommand command = null)
        {
            bool AceptaCristalD = false;
            bool AceptaCristalI = false;

            string diamD = "";
            string diamI = "";

            // Obtén el primer trabajo (o el que corresponda según tu lógica)
            var _Trabajo = Trabajo.FirstOrDefault();

            if (_Trabajo == null)
                return false;

            // Obtener diámetros efectivos
            DataSet dsDiametroEfectivo = _D_Articulo.MostrarDiametroEfectivoCrtGrid(
                nacionalidad,
                cedula,
                Examen,
                CristalD,
                CristalI,
                "A",
                _Trabajo.T_TIPOVISIOND,
                _Trabajo.T_TIPOVISIONI,
                Montura,
    _Trabajo.T_HORIZONTAL?.ToString().Replace(".", ""),
    _Trabajo.T_MAXIMA?.ToString().Replace(".", ""),
    _Trabajo.T_PUENTE?.ToString().Replace(".", ""),
             sucursal, command);

            if (Convert.ToInt32(dsDiametroEfectivo.Tables[1].Rows[0][0]) > 0)
            {
                diamD = dsDiametroEfectivo.Tables[1].Rows[0]["DIAMETROEFECTIVODERECHO"].ToString();
                diamI = dsDiametroEfectivo.Tables[1].Rows[0]["DIAMETROEFECTIVOIZQUIERDO"].ToString();
            }
            else
            {
                diamD = "0";
                diamI = "0";
            }

            if (CristalD.StartsWith("C") || CristalI.StartsWith("C"))
            {
                DataSet dsValidaciones = _D_Articulo.MostrarValidaRangoCrtGrid(
                    nacionalidad,
                    cedula,
                    Examen,
                    "A",
                    CristalD,
                    CristalI,
                    _Trabajo.T_ALTD,
                    _Trabajo.T_ALTI,
                    _Trabajo.T_TIPOVISIOND,
                    _Trabajo.T_TIPOVISIONI,
                    diamD,
                    diamI, command
                );

                string ojo;
                if (!string.IsNullOrEmpty(CristalD) && !string.IsNullOrEmpty(CristalI))
                {
                    ojo = "A";
                }
                else if (!string.IsNullOrEmpty(CristalD) && string.IsNullOrEmpty(CristalI))
                {
                    ojo = "D";
                }
                else
                {
                    ojo = "I";
                }

                if (ojo == "A")
                {
                    AceptaCristalD = dsValidaciones.Tables[0].Rows.Count > 0;
                    AceptaCristalI = dsValidaciones.Tables[1].Rows.Count > 0;
                }
                else if (ojo == "D")
                {
                    AceptaCristalD = dsValidaciones.Tables[0].Rows.Count > 0;
                    AceptaCristalI = true;
                }
                else if (ojo == "I")
                {
                    AceptaCristalI = dsValidaciones.Tables[0].Rows.Count > 0;
                    AceptaCristalD = true;
                }

                if (!AceptaCristalD || !AceptaCristalI)
                {
                    return false; // No se adapta a los rangos
                }
            }

            return true; // Todo correcto
        }

    public bool VerificoParametrosCristales(List<TB_TRABAJO> Trabajo, string nacionalidad, string cedula, string Examen, string CristalD, string CristalI, string Color,
        System.Windows.Forms.ListView LbResultado2, System.Windows.Forms.ListView LbResultados, SqlCommand command = null)
        {
        bool AceptaCristalD = false;
        bool AceptaCristalI = false;
        string diamD = "";
        string diamI = "";
        DataSet dsParamCRT;
        DataSet dsParamCRT2;

        // Obtén el primer trabajo (o el que corresponda según tu lógica)
        var _Trabajo = Trabajo.FirstOrDefault();

        string ojo;
        if (!string.IsNullOrEmpty(CristalD) && !string.IsNullOrEmpty(CristalI))
        {
            ojo = "A";
        }
        else if (!string.IsNullOrEmpty(CristalD) && string.IsNullOrEmpty(CristalI))
        {
            ojo = "D";
        }
        else
        {
            ojo = "I";
        }

        LbResultados.Items.Add("OJO DERECHO");
        LbResultado2.Items.Add("OJO IZQUIERDO");


                if (ojo == "A")
                {
                    dsParamCRT = _D_Articulo.MostrarRangosCrtGrid(nacionalidad, cedula, Examen, CristalD, "D", _Trabajo.T_TIPOVISIOND, Convert.ToDecimal(_Trabajo.T_ALTD), 0, Convert.ToDecimal(_Trabajo.T_DISTANCIAVERTICE), Convert.ToDecimal(_Trabajo.T_ANGULOFACIAL), Convert.ToDecimal(_Trabajo.T_ANGULOPANTOSCOPICO), "", _Trabajo.T_SERVICIO, _Trabajo.T_LABORATORIO, Convert.ToString(_Trabajo.T_DISTANCIAVERTICE), Convert.ToString(_Trabajo.T_ANGULOFACIAL), Convert.ToString(_Trabajo.T_ANGULOPANTOSCOPICO), Convert.ToString(_Trabajo.T_DISTANCIADELECTURA), command);
                    dsParamCRT2 = _D_Articulo.MostrarRangosCrtGrid(nacionalidad, cedula, Examen, CristalI, "I", _Trabajo.T_TIPOVISIONI, Convert.ToDecimal(_Trabajo.T_ALTI), 0, Convert.ToDecimal(_Trabajo.T_DISTANCIAVERTICE), Convert.ToDecimal(_Trabajo.T_ANGULOFACIAL), Convert.ToDecimal(_Trabajo.T_ANGULOPANTOSCOPICO), "", _Trabajo.T_SERVICIO, _Trabajo.T_LABORATORIO, Convert.ToString(_Trabajo.T_DISTANCIAVERTICE), Convert.ToString(_Trabajo.T_ANGULOFACIAL), Convert.ToString(_Trabajo.T_ANGULOPANTOSCOPICO), Convert.ToString(_Trabajo.T_DISTANCIADELECTURA), command);

                    // Validación de parámetros

                    if (Enumerable.Range(0, 13).All(x => dsParamCRT.Tables[2].Rows[0][x].ToString() == "1"))
                    {
                        AceptaCristalD = true;
                    }
                    else
                    {
                        AceptaCristalD = false;
                        for (int x = 0; x <= 16; x++)
                        {
                            if (dsParamCRT.Tables[2].Rows[0][x].ToString() != "1")
                            {
                                LbResultados.Items.Add(dsParamCRT.Tables[2].Rows[0][x].ToString());
                            }
                        }
                    }

                    if (Enumerable.Range(0, 17).All(x => dsParamCRT2.Tables[2].Rows[0][x].ToString() == "1"))
                    {
                        AceptaCristalI = true;
                    }
                    else
                    {
                        AceptaCristalI = false;
                        for (int x = 0; x <= 16; x++)
                        {
                            if (dsParamCRT2.Tables[2].Rows[0][x].ToString() != "1")
                            {
                                LbResultado2.Items.Add(dsParamCRT2.Tables[2].Rows[0][x].ToString());
                            }
                        }
                    }
                }
                else if (ojo == "D")
                {
                    dsParamCRT = _D_Articulo.MostrarRangosCrtGrid(nacionalidad, cedula, Examen, CristalD, "D", _Trabajo.T_TIPOVISIOND, Convert.ToDecimal(_Trabajo.T_ALTD), 0, Convert.ToDecimal(_Trabajo.T_DISTANCIAVERTICE), Convert.ToDecimal(_Trabajo.T_ANGULOFACIAL), Convert.ToDecimal(_Trabajo.T_ANGULOPANTOSCOPICO), "", _Trabajo.T_SERVICIO, _Trabajo.T_LABORATORIO, Convert.ToString(_Trabajo.T_DISTANCIAVERTICE), Convert.ToString(_Trabajo.T_ANGULOFACIAL), Convert.ToString(_Trabajo.T_ANGULOPANTOSCOPICO), Convert.ToString(_Trabajo.T_DISTANCIADELECTURA), command);

                    AceptaCristalD = Enumerable.Range(0, 17).All(x => dsParamCRT.Tables[2].Rows[0][x].ToString() == "1");
                    AceptaCristalI = AceptaCristalD;

                    if (!AceptaCristalD)
                    {
                        for (int x = 0; x <= 16; x++)
                        {
                            if (dsParamCRT.Tables[2].Rows[0][x].ToString() != "1")
                            {
                                LbResultados.Items.Add(dsParamCRT.Tables[2].Rows[0][x].ToString());
                            }
                        }
                    }
                }
                else if (ojo == "I")
                {
                    dsParamCRT2 = _D_Articulo.MostrarRangosCrtGrid(nacionalidad, cedula, Examen, CristalI, "I", _Trabajo.T_TIPOVISIONI, Convert.ToDecimal(_Trabajo.T_ALTI), 0, Convert.ToDecimal(_Trabajo.T_DISTANCIAVERTICE), Convert.ToDecimal(_Trabajo.T_ANGULOFACIAL), Convert.ToDecimal(_Trabajo.T_ANGULOPANTOSCOPICO), "", _Trabajo.T_SERVICIO, _Trabajo.T_LABORATORIO, Convert.ToString(_Trabajo.T_DISTANCIAVERTICE), Convert.ToString(_Trabajo.T_ANGULOFACIAL), Convert.ToString(_Trabajo.T_ANGULOPANTOSCOPICO), Convert.ToString(_Trabajo.T_DISTANCIADELECTURA), command);

                    AceptaCristalI = Enumerable.Range(0, 17).All(x => dsParamCRT2.Tables[2].Rows[0][x].ToString() == "1");
                    AceptaCristalD = AceptaCristalI;

                    if (!AceptaCristalI)
                    {
                        for (int x = 0; x <= 16; x++)
                        {
                            if (dsParamCRT2.Tables[2].Rows[0][x].ToString() != "1")
                            {
                                LbResultado2.Items.Add(dsParamCRT2.Tables[2].Rows[0][x].ToString());
                            }
                        }
                    }
                }

        bool VerificoParametrosCristales = false;
        if (AceptaCristalD == true && AceptaCristalI == true)
        {
            VerificoParametrosCristales = true;
        }
        else if (AceptaCristalD == false || AceptaCristalI == false)
        {
            VerificoParametrosCristales = false;

            DataSet dsConsultaCristal = _D_Articulo.MostrarParametrosCrtGrid(CristalD, CristalI, command);
            stringBuilder.Append("El Cristal no se adapta a estos parámetros");
        }

            return VerificoParametrosCristales;
    }

        public (DateTime FechaOfre, string HoraOfre) FechaOfrecida(string Cristal, string Montura, string Color, string AR, string Sucursal, SqlCommand command = null)
        {
            DateTime fechaOfre = TB_CAORDSER.Fec_ofrecido;
            string horaOfre = TB_CAORDSER.Hor_ofrecido;

            string Quorum_fo = "0";
                string Remoto_fo = "0";

                if (TB_CAORDSER.Cod_Laboratorio == "QUO")
                {
                    Quorum_fo = "1";
                    Remoto_fo = "0";
                }

                if (TB_CAORDSER.Cod_Venta == "002" && TB_CAORDSER.Cod_Servicio != "004")
                {
                    Dictionary<string, string> Variables_calculo = new Dictionary<string, string>
            {
                    { "@CRISTAL", Cristal},
                    { "@ANTIREFL", AR},
                    { "@MONTURA",  Montura },
                    { "@MONTURAPROPIA", "0"},
                    { "@LC", "" },
                    { "@Quorum", Quorum_fo },
                    { "@Remoto", Remoto_fo},
                    { "@Color", Color },
                    { "@Laboratorio", "" },
                    { "@Sucursal", Sucursal }
                    };

                    DataSet dsFechaOfr = _D_Articulo.ActulizaFechaOfre(Variables_calculo, null);

                    if (dsFechaOfr.Tables[1].Rows.Count > 0)
                    {
                        fechaOfre = Convert.ToDateTime(dsFechaOfr.Tables[1].Rows[0]["Dias"]);
                        horaOfre = dsFechaOfr.Tables[1].Rows[0]["Hora"].ToString();
                    }
                }
                else
                {
                    if (TB_CAORDSER.Cod_Servicio != "004")
                    {
                        fechaOfre = DateTime.Now;
                        horaOfre = DateTime.Now.ToString("HH:mm:ss");
                    }
                }
            

            return (fechaOfre, horaOfre);
        }
    }
}
