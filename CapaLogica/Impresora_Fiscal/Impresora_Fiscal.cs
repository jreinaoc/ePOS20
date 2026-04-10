using CapaDatos.DetalleOrden_Datos;
using CapaDatos.Inicio_Datos;
using System.Globalization;
using System;
using System.Configuration;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CapaEntidades;
//using CapaLogica.Anulacion_Logica;
using CapaDatos.Anulacion;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace CapaLogica.Impresora_Fiscal
{
    public class Impresora_Fiscal
    {
        //El uso de la clase StringBuilder nos ayudara a devolver los mensajes de las validaciones
        public readonly StringBuilder stringBuilder = new StringBuilder();
        private int glbPuertoCOM = Convert.ToInt16(ConfigurationManager.AppSettings.Get("PuertoCOMimpresora"));
        private D_Inicio _D_Inicio = new D_Inicio();
        private D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        public string NumeroNCFiscal = "";
        //private FrmMensajes _FrmMensajes = new FrmMensajes();
        D_Anulacion _D_Anulacion = new D_Anulacion();
        // Diccionario para acciones pendientes
        private static readonly Dictionary<string, string> _accionesPendientes = new Dictionary<string, string>();
        private static readonly object _lockObject = new object();

        // Clases de apoyo
        public class EstadoImpresora
        {
            public bool Online { get; set; }
            public bool TapaAbierta { get; set; }
            public bool TemperaturaAlta { get; set; }
            public bool ErrorNoRecuperable { get; set; }
            public bool ErrorCortadora { get; set; }
            public bool BufferOverflow { get; set; }
            public bool TienePapel { get; set; }
            public bool PocoPapel { get; set; }
            public bool SinPapel { get; set; }
            public bool ErrorGeneral { get; set; }

        }

     

        public bool ImprimirNCFiscal(string SucursalActual, string NumeroFactura, string SerialImpresora, double Monto, string Motivo, string CodigoMotivoAnulacion)
        {
            VmaxComVe.VmaxComClass objVmax = new VmaxComVe.VmaxComClass();
            stringBuilder.Clear();
            uint resp = 0;
            string status = "";
            DataTable dtFacturas;
            DataTable dtCliente;
            bool StatusNoataCredito = true;
            string CedulaCliente = "";
            string NacionalidadCliente = "";
            string NunOrden = "";
            DateTime FechaOperacion = DateTime.Today;
            string PrimerNombre = "";
            string PrimerApellido = "";
            string SerialImpresoraNC = "";
            string FechaImpresora = "";
            string Prueba = _D_Inicio.Sucursal();

            try
            {

                //status = objVmax.RetornoStatusImpresora.sStatus;

                if (SucursalActual != "")
                {
                    dtFacturas = _D_DetalleOrden.BucarFactura(NumeroFactura, SerialImpresora, SucursalActual);

                    if (dtFacturas.Rows.Count > 0)
                    {
                        foreach (DataRow drItem in dtFacturas.Rows)
                        {
                            CedulaCliente = drItem["CTE_CedIdenPAG"].ToString();
                            NacionalidadCliente = drItem["CTE_NacioPAG"].ToString();
                            FechaOperacion = Convert.ToDateTime(TB_FACTURAS.Fact_FecCrea);
                            NunOrden = drItem["NumOrdServ"].ToString();
                            break;
                        }

                    }

                    dtCliente = _D_DetalleOrden.BucarTB_CTEPPAL(TB_FACTURAS.CTE_CedIdenPAG, TB_FACTURAS.CTE_NacioPAG);

                    if (dtCliente.Rows.Count > 0)
                    {
                        foreach (DataRow drItem in dtCliente.Rows)
                        {
                            PrimerNombre = drItem["CTE_PNombre"].ToString();
                            PrimerApellido = drItem["CTE_PApellido"].ToString();
                            break;
                        }

                    }

                    resp = objVmax.AbrirPuerto(Convert.ToString(glbPuertoCOM));

                    if (resp != 0)
                    {
                        objVmax.Cancelar();
                        objVmax.Cerrar();
                        objVmax.CerrarPuerto();
                        stringBuilder.Append(Environment.NewLine + "No hay conexión con la impresora fiscal");
                        _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "090", TB_USUARIO.COD_EMPLEADO, "No hay conexión con la impresora fiscal");
                        StatusNoataCredito = false;
                        return StatusNoataCredito;

                    }

                    else
                    {
                        //resp = objVmax.AbrirCF("Jesus Antonio Pabon Mavare", "J309860895", "2", "000000262", "TIU2202214", "26032022", "1055", 40);
                        resp = objVmax.AbrirCF(PrimerNombre + " " + PrimerApellido, Convert.ToString(TB_FACTURAS.CTE_NacioPAG) + "" + Convert.ToString(TB_FACTURAS.CTE_CedIdenPAG), "2", Convert.ToString(TB_FACTURAS.Fact_Num), Convert.ToString(TB_FACTURAS.Fact_SerialImpresora), Convert.ToDateTime(TB_FACTURAS.Fact_FecCrea).ToString("dd/MM/yyyy"), FechaOperacion.ToString("HH:mm"), 40);


                        if (resp != 0)
                        {
                            stringBuilder.Append(Environment.NewLine + "No hay conexión con la impresora fiscal");
                            _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "090", TB_USUARIO.COD_EMPLEADO, "No hay conexión con la impresora fiscal");
                            StatusNoataCredito = false;
                            objVmax.Cancelar();
                            objVmax.Cerrar();
                            objVmax.CerrarPuerto();
                            return StatusNoataCredito;
                        }

                        NumeroNCFiscal = (Convert.ToInt32(objVmax.RetornoMF.uiTotalNCDiarias) + 1).ToString();

                        if (resp == 0)
                        {

                            DataSet dsArti = _D_DetalleOrden.DetalleNotaCreditoFiscal(NumeroFactura, SerialImpresora);
                            string desart;

                            foreach (DataRow drItem in dsArti.Tables[0].Rows)
                            {
                                if (Convert.ToUInt32(drItem["Ordserv_Dto"].ToString()) > 0)
                                {
                                }

                                desart = drItem["CodArticulo"] + " " + drItem["DESART"];
                                //Valido que el texto no sea mayor a 40 caracteres para que no salga duplicado el articulo en la factura 25-05-2023
                                desart = Validar_Cadena(desart);

                                string Impuesto = "";
                                if (drItem["ART_EXENTO"].ToString() == "1")
                                {
                                    Impuesto = "0";
                                }
                                else
                                {
                                    Impuesto = "1";
                                }

                                resp = objVmax.ItemDev(desart, (Convert.ToDouble(drItem["Ordserv_Cant"]) * 1000).ToString(), drItem["OrdServ_Precio"].ToString(), Impuesto, "1", 40);

                            }

                            DataSet dsDcto = _D_DetalleOrden.DescuentosNotaCreditoFiscal(NunOrden);

                            if (resp == 0)
                            {
                                foreach (DataRow drItemdcto in dsDcto.Tables[0].Rows)
                                {

                                    resp = objVmax.DescuentoCF("Descuento", drItemdcto["DescuentoExento"].ToString(), drItemdcto["DescuentoGravable"].ToString(), "", "");

                                }

                            }
                            if (resp == 0)
                            {
                                resp = objVmax.Subtotal();
                                resp = objVmax.TextoNoFiscal("Monto Disponible:  " + Monto.ToString());
                                objVmax.ObtenerReporteInformativo();
                                objVmax.AbrirDNF();
                                SerialImpresoraNC = objVmax.RetornoMI.sSerial;
                                FechaImpresora = objVmax.RetornoMI.sFecha;
                                resp = objVmax.Cerrar();
                                resp = objVmax.CerrarPuerto();

                            }


                        }

                        if (resp == 0)
                        {
                            NumeroNCFiscal = objVmax.RetornoAbrirFactura.uiNumeroFactura.ToString();

                            switch (NumeroNCFiscal.Length)
                            {
                                case 7:
                                    {
                                        NumeroNCFiscal = NumeroNCFiscal;
                                        break;
                                    }

                                case 6:
                                    {
                                        NumeroNCFiscal = "0" + NumeroNCFiscal;
                                        break;
                                    }

                                case 5:
                                    {
                                        NumeroNCFiscal = "00" + NumeroNCFiscal;
                                        break;
                                    }

                                case 4:
                                    {
                                        NumeroNCFiscal = "000" + NumeroNCFiscal;
                                        break;
                                    }

                                case 3:
                                    {
                                        NumeroNCFiscal = "0000" + NumeroNCFiscal;
                                        break;
                                    }

                                case 2:
                                    {
                                        NumeroNCFiscal = "00000" + NumeroNCFiscal;
                                        break;
                                    }

                                case 1:
                                    {
                                        NumeroNCFiscal = "000000" + NumeroNCFiscal;
                                        break;
                                    }
                            }
                            string Transaccion = _D_DetalleOrden.RegistrarNCFISCAL(SucursalActual, NumeroNCFiscal, "005", TB_FACTURAS.Fact_Num, SerialImpresora, _D_Inicio.DiaActivo().ToString("yyyy/MM/dd"), SerialImpresoraNC, "", TB_FACTURAS.CTE_NacioPAG, TB_FACTURAS.CTE_CedIdenPAG, Motivo.ToUpper(), Convert.ToDouble(TB_FACTURAS.Fact_MontoGravable), Convert.ToDouble(TB_FACTURAS.Fact_Total), TB_USUARIO.COD_USR, "", Convert.ToDouble(TB_FACTURAS.Fact_IGTF), Convert.ToDouble(TB_FACTURAS.Fact_AlicuotaIGTF), Convert.ToDouble(TB_FACTURAS.Fact_MontoExento), CodigoMotivoAnulacion);

                            if (Transaccion == "SATISFACTORIO")
                            {
                                StatusNoataCredito = true;
                            }

                            else
                            {
                                StatusNoataCredito = false;
                                stringBuilder.Append("Por favor comunicarse con el Dpto de sistemas y reportar el siguiente error: " + Environment.NewLine + string.Format("Error: {0}", Transaccion));
                            }

                            return StatusNoataCredito;
                        }


                    }

                }


                stringBuilder.Append(Environment.NewLine + "El numero de sucursal no tiene un valor valido, Verifique");
                return false;

            }

            catch (Exception ex)
            {
                stringBuilder.Append("Por favor comunicarse con el Dpto de sistemas y reportar el siguiente error: " + Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return false;
            }
        }



        public bool VerficarConexionImpresoraFiscal()
        {
            //VmaxComVe.VmaxComClass objVmax = new VmaxComVe.VmaxComClass();
            stringBuilder.Clear();
            uint resp = 0;
            bool Conexion = false;
            string status;

           
            try
            {
                VmaxComVe.VmaxComClass objVmax = new VmaxComVe.VmaxComClass();
                uint ret = 0;

                ret = objVmax.AbrirPuerto(Convert.ToString(glbPuertoCOM));
                ret = objVmax.ObtenerEstadoImpresora();
               
                if (ret != 16 && ret != 0)
                {
                    //resp = objVmax.AbrirCF("", "", "1", "1", "12345", "", "", 40);
                    //objVmax.Cancelar();
                    //objVmax.Cerrar();
                    objVmax.CerrarPuerto();
                    stringBuilder.Append(Environment.NewLine + "No hay conexión con la impresora fiscal ");
                    Conexion = false;
                    _D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "090", TB_USUARIO.COD_EMPLEADO, "No hay conexión con la impresora fiscal");


                }

                else
                {
                    Conexion = true;
                    objVmax.Cancelar();
                    objVmax.Cerrar();
                    objVmax.CerrarPuerto();
                }

                return Conexion;
            }

            catch (Exception ex)
            {
                stringBuilder.Append("Por favor comunicarse con el Dpto de sistemas y reportar el siguiente error: " + Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return false;
            }
        }

        public EstadoImpresora ObtenerEstadoCompleto()
        {
            VmaxComVe.VmaxComClass _printer = new VmaxComVe.VmaxComClass();
            var estado = new EstadoImpresora();

            try
            {
                uint estadoMecanico = _printer.ObtenerEstadoImpresora();

                // Interpretar estados - VERIFICAR ESTAS MÁSCARAS CON EL MANUAL DE LA IMPRESORA
                estado.Online = (estadoMecanico & 0x01) == 0;        // Bit 0
                estado.TapaAbierta = (estadoMecanico & 0x02) != 0;   // Bit 1 - CORREGIDO
                estado.PocoPapel = (estadoMecanico & 0x04) != 0;     // Bit 2
                estado.SinPapel = (estadoMecanico & 0x08) != 0;      // Bit 3
                estado.TemperaturaAlta = (estadoMecanico & 0x10) != 0; // Bit 4
                estado.ErrorGeneral = (estadoMecanico & 0x20) != 0;  // Bit 5
                estado.ErrorCortadora = (estadoMecanico & 0x40) != 0; // Bit 6
                estado.BufferOverflow = (estadoMecanico & 0x80) != 0; // Bit 7

                // Estado del papel basado en múltiples bits
                estado.TienePapel = !estado.SinPapel;

                // Para debugging - agrega esto temporalmente
                Console.WriteLine($"Estado mecánico (hex): 0x{estadoMecanico:X2}");
                Console.WriteLine($"Estado mecánico (bin): {Convert.ToString(estadoMecanico, 2).PadLeft(8, '0')}");
                Console.WriteLine($"Tapa abierta: {estado.TapaAbierta}");

                return estado;
            }
            finally
            {
                _printer.CerrarPuerto();
            }
        }
     

        private static string ConvertirAHex4(object val, string name)
        {
            try
            {
                switch (val)
                {
                    case null: return null;
                    case string s when System.Text.RegularExpressions.Regex.IsMatch(s.Trim(), @"^[0-9A-Fa-f]{4}$"):
                        return s.Trim().ToUpperInvariant();
                    case ushort u: return u.ToString("X4");
                    case short si: return ((ushort)si).ToString("X4");
                    case int i when i >= 0 && i <= 0xFFFF: return ((ushort)i).ToString("X4");
                    case uint ui when ui <= 0xFFFF: return ((ushort)ui).ToString("X4");
                    case byte[] ba when ba.Length >= 2:
                        return $"{ba[0]:X2}{ba[1]:X2}";
                    case byte b: return b.ToString("X2") + "00";
                    default:
                        if (ushort.TryParse(val.ToString(), out ushort result))
                            return result.ToString("X4");
                        return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public bool VerficarConexionImpresoraFiscalSinCerrar()
        {
            //VmaxComVe.VmaxComClass objVmax = new VmaxComVe.VmaxComClass();
            stringBuilder.Clear();
            uint resp = 0;
            bool Conexion = false;
            string status;


            try
            {

                VmaxComVe.VmaxComClass objVmax = new VmaxComVe.VmaxComClass();

                //objVmax.ObtenerReporteInformativo();
                //objVmax.AbrirPuerto(glbPuertoCOM.ToString());
                //objVmax.ObtenerContadores();
                //objVmax.CerrarPuerto();
                //string UltimoNumeroFacturaCancelado2 = objVmax.RetornoContadores.uiUltFacturaAnulada.ToString().PadLeft(7, '0');

                uint ret = 0;
                uint Prueba = 0;
                ret = objVmax.AbrirPuerto(Convert.ToString(glbPuertoCOM));
                ret = objVmax.ObtenerEstadoImpresora();
                var Resss= objVmax.RetornoStatusImpresora.sStatus;


                // lo que nos dice Vmas 
                // 🔹 Para modelos VMAX2: 0x0001 = tapa abierta o sin papel
                if (!string.IsNullOrEmpty(Resss) && Resss != "0000")
                {
                    ret = 3;
                    objVmax.Cancelar();
                    objVmax.Cerrar();
                    objVmax.CerrarPuerto();
                    AgregarAccionPendiente("097"); // "Ausencia de papel o Tapa Abierta"
                    Console.WriteLine("La impresora reporta tapa abierta o ausencia de papel Advertencia");
                }
                else
                {
                    Console.WriteLine("✅ Impresora lista");
                }

              
                if (ret != 16 && ret != 0)
                {
                    objVmax.CerrarPuerto();
                    stringBuilder.Append(Environment.NewLine + "Verifique la conexión de la impresora fiscal");
                    Conexion = false;
                    //AgregarAccionPendiente("096"); // "Fallo en apertura de puerto"
                    //_D_Anulacion.CaragarAuditor(_D_Inicio.Sucursal(), "090", TB_USUARIO.COD_EMPLEADO, "No hay conexión con la impresora fiscal");

                }

                else
                {
                    Conexion = true;
                }

                return Conexion;
            }

            catch (Exception ex)
            {
                stringBuilder.Append("Por favor comunicarse con el Dpto de sistemas y reportar el siguiente error: " + Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return false;
            }

        }

        public string Validar_Cadena(string _cadena)
        {
            int MaxLength = 39;
            string Cadena = "";

            if (_cadena.Length > MaxLength)
            {
                Cadena = _cadena.Substring(0, MaxLength);
            }

            else
            {
                Cadena = _cadena;
            }

            return Cadena;
        }

        // Diccionario estático con todos los códigos predefinidos
        public static readonly Dictionary<string, string> _accionesPredefinidas = new Dictionary<string, string>
        {
            //{ "096", "Fallo en la apertura del puerto" },
            { "097", "Ausencia de papel o Tapa Abierta" },
            { "098", "numero de factura o serial de impresora fiscal en blanco" },
            { "099", "Discrepancia en totales" },
            //{ "100", "Numero de factura invalido o ya anulado" },
            { "101", "Violacion de calve primaria en la Base de datos" }
        };

        // Método para agregar una acción pendiente por código
        public static void AgregarAccionPendiente(string codigo)
        {
            lock (_lockObject)
            {
                if (_accionesPredefinidas.TryGetValue(codigo, out string descripcion))
                {
                    if (!_accionesPendientes.ContainsKey(codigo))
                    {
                        _accionesPendientes.Add(codigo, descripcion);
                    }
                }
                else
                {
                    throw new ArgumentException($"Código '{codigo}' no está definido");
                }
            }
        }

        // Método para OBTENER todas las acciones pendientes (NO las inserta)
        public static Dictionary<string, string> ObtenerAccionesPendientes()
        {
            lock (_lockObject)
            {
                return new Dictionary<string, string>(_accionesPendientes);
            }
        }

        // Método para LIMPIAR las acciones pendientes después de usarlas
        public static void LimpiarAccionesPendientes()
        {
            lock (_lockObject)
            {
                _accionesPendientes.Clear();
            }
        }

        // Método para OBTENER la cantidad de acciones pendientes
        public static int ObtenerCantidadAccionesPendientes()
        {
            lock (_lockObject)
            {
                return _accionesPendientes.Count;
            }
        }

        // Método para verificar si hay acciones pendientes
        public static bool TieneAccionesPendientes()
        {
            lock (_lockObject)
            {
                return _accionesPendientes.Count > 0;
            }
        }
    }
}




