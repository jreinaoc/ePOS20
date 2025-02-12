using System;
using System.Collections.Generic;
using CapaDatos.DetalleOrden_Datos;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaLogica.Inicio_Logica;
using System.Data;
using CapaEntidades;
using System.Windows.Forms;
using CapaDatos.Inicio_Datos;
using System.Globalization;
using CapaLogica.Login_Logica;
using System.Data.SqlClient;
using System.Drawing;

namespace CapaLogica.DetalleOrden_Logica
{
    public class L_Facturacion
    {
        //El uso de la clase StringBuilder nos ayudara a devolver los mensajes de las validaciones
        public readonly StringBuilder stringBuilder = new StringBuilder();

        //Instanciamos nuestra clase D_Loguin para poder utilizar sus miembros

        private D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        private D_Inicio _D_Inicio = new D_Inicio();

        public double IgtfNOT;
        public double BsNOT;
        public bool MostrarClientePag = false;
        public string NombreCliente;
        public string CedCliente;
        public string TlfCliente;
        public string CorreoCliente;


        public class Valor
        {
            public string Value { get; set; }
            public string Index { get; set; }
        }

        public struct ValuesTransposicion
        {
            public Double TransESF;
            public Double TransCIL;
            public int TransEJE;
        }


        public void CargarTasa()
        {
            DateTime FechaActiva = _D_Inicio.DiaActivo();
            _D_Inicio.TasaDiaDolarEntidad(FechaActiva.ToString("yyyyMMdd"));
        }

        public void ComboboxTipoMoneda(System.Windows.Forms.ComboBox Moneda)
        {

            var Valores = new List<Valor>();

            Valores.Add(new Valor() { Index = "Dólares", Value = "Dólares" });
            Valores.Add(new Valor() { Index = "Euros", Value = "Euros" });
            Valores.Add(new Valor() { Index = "Bolívares", Value = "Bolívares" });

            Moneda.DataSource = Valores;
            Moneda.DisplayMember = "Value";
            Moneda.ValueMember = "Index";
        }


        public void ComboboxTipoMonedaTranferenciaDivisa(System.Windows.Forms.ComboBox Moneda)
        {

            var Valores = new List<Valor>();

            Valores.Add(new Valor() { Index = "Dólares", Value = "Dólares" });
            Valores.Add(new Valor() { Index = "Euros", Value = "Euros" });

            Moneda.DataSource = Valores;
            Moneda.DisplayMember = "Value";
            Moneda.ValueMember = "Index";
        }

        public void ComboboxTipoTarjeta(System.Windows.Forms.ComboBox Moneda)
        {

            var Valores = new List<Valor>();

            Valores.Add(new Valor() { Index = "Visa", Value = "Visa" });
            Valores.Add(new Valor() { Index = "Master", Value = "Master" });
            Valores.Add(new Valor() { Index = "American", Value = "American" });
            Valores.Add(new Valor() { Index = "Dinners", Value = "Dinners" });
            Valores.Add(new Valor() { Index = "Sisa", Value = "Sisa" });

            Moneda.DataSource = Valores;
            Moneda.DisplayMember = "Value";
            Moneda.ValueMember = "Index";

        }


        public void ComboboxTipoPago1(System.Windows.Forms.ComboBox Pagos)
        {

            var Valores = new List<Valor>();
            Valores.Add(new Valor() { Index = "Cashea", Value = "021" });
            Valores.Add(new Valor() { Index = "Debito", Value = "003" });
            Valores.Add(new Valor() { Index = "Efectivo", Value = "001" });
            Valores.Add(new Valor() { Index = "Efectivo Divisa", Value = "022" });
            Valores.Add(new Valor() { Index = "ISLR Retenido", Value = "014" });
            Valores.Add(new Valor() { Index = "Iva Retenido", Value = "013" });
            Valores.Add(new Valor() { Index = "Nota Credito", Value = "006" });
            Valores.Add(new Valor() { Index = "Nota Devolucion", Value = "010" });
            Valores.Add(new Valor() { Index = "Pago Móvil", Value = "024" });
            Valores.Add(new Valor() { Index = "Tarjeta de Credito", Value = "007" });
            Valores.Add(new Valor() { Index = "Transferencia", Value = "021" });
            Valores.Add(new Valor() { Index = "Transferencia Divisa", Value = "022" });

            //Valores.Add(new Valor() { Index = "Cheque", Value = "002" });
            //Valores.Add(new Valor() { Index = "Credito", Value = "004" });
            //Valores.Add(new Valor() { Index = "Financiamiento", Value = "005" });
            //Valores.Add(new Valor() { Index = "Inicial Financiamiento", Value = "008" });
            //Valores.Add(new Valor() { Index = "RetiroEfectivo", Value = "009" });
            //Valores.Add(new Valor() { Index = "Orden de Pago", Value = "011" });
            //Valores.Add(new Valor() { Index = "Reintegro Dinero", Value = "012" });
            //Valores.Add(new Valor() { Index = "Cupon", Value = "015" });
            //Valores.Add(new Valor() { Index = "Tarjeta Propia", Value = "016" });
            //Valores.Add(new Valor() { Index = "Exoneracion ITBIS", Value = "017" });
            //Valores.Add(new Valor() { Index = "Comprobantes Cancelados", Value = "018" });
            //Valores.Add(new Valor() { Index = "Ticket Salud Tarjeta", Value = "019" });
            //Valores.Add(new Valor() { Index = "Ticket Salud Efectivo", Value = "020" });


            Pagos.DataSource = Valores;
            Pagos.DisplayMember = "Index";
            Pagos.ValueMember = "Value";

        }

        public void ComboboxNacionalidad(System.Windows.Forms.ComboBox Nacionalidad)
        {

            var Valores = new List<Valor>();
            Valores.Add(new Valor() { Index = "G", Value = "G" });
            Valores.Add(new Valor() { Index = "E", Value = "E" });
            Valores.Add(new Valor() { Index = "J", Value = "J" });
            //Valores.Add(new Valor() { Index = "N", Value = "N" });
            Valores.Add(new Valor() { Index = "V", Value = "V" });
    

            Nacionalidad.DataSource = Valores;
            Nacionalidad.DisplayMember = "Index";
            Nacionalidad.ValueMember = "Value";

        }


        public void ComboboxPrefijosCelular(System.Windows.Forms.ComboBox Prefijos)
        {

            var Valores = new List<Valor>();
            Valores.Add(new Valor() { Index = "", Value = "" });
            Valores.Add(new Valor() { Index = "0414", Value = "0414" });
            Valores.Add(new Valor() { Index = "0424", Value = "0424" });
            Valores.Add(new Valor() { Index = "0412", Value = "0412" });
            Valores.Add(new Valor() { Index = "0416", Value = "0416" });
            Valores.Add(new Valor() { Index = "0426", Value = "0426" });



            Prefijos.DataSource = Valores;
            Prefijos.DisplayMember = "Index";
            Prefijos.ValueMember = "Value";

        }

        public void LLenarComboboxPagos(System.Windows.Forms.ComboBox Pago)
        {
            ComboboxTipoPago1(Pago);
            //Pago.DataSource = _D_DetalleOrden.Pagos();
            //Pago.DisplayMember = "Indexx";
            //Pago.ValueMember = "Value";

        }

        public void LLenarComboboxBancos2(System.Windows.Forms.ComboBox Pago, bool Extranjera)
        {
            Pago.DataSource = _D_DetalleOrden.Bancos2(Extranjera);
            Pago.DisplayMember = "Indexx";
            Pago.ValueMember = "Value";
        }


        public void LLenarComboboxBancos(System.Windows.Forms.ComboBox Pago, bool Extranjera)
        {
            Pago.DataSource = _D_DetalleOrden.Bancos(Extranjera);
            Pago.DisplayMember = "Indexx";
            Pago.ValueMember = "Value";
        }

        public void LLenarCbxBancoRecp(System.Windows.Forms.ComboBox Pago, bool Extranjera)
        {
            Pago.DataSource = _D_DetalleOrden.BancoRecp(Extranjera);
            Pago.DisplayMember = "Indexx";
            Pago.ValueMember = "Value";
        }

        public void LLenarCbxBancoRecp_PagoMovil(System.Windows.Forms.ComboBox Pago, bool Extranjera)
        {
            Pago.DataSource = _D_DetalleOrden.BancoRecp_Pagomovil(Extranjera);
            Pago.DisplayMember = "Indexx";
            Pago.ValueMember = "Value";
        }

        public void DatosOrden(string Orden, string Revison)
        {
            try
            {

                stringBuilder.Clear();
                _D_DetalleOrden.Datos_de_la_Orden(Orden, Revison);

            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));

            }
        }

        public string StatusOrde(string StatusOrden)
        {
            string Status;

            switch (StatusOrden)
            {
                case "002":
                    Status = "Facturada";
                    break;
                case "003":
                    Status = "Anulada";
                    break;
                case "004":
                    Status = "Por pagar";
                    break;
                case "005":
                    Status = "Abonada";
                    break;
                default:
                    Status = "";
                    break;
            }

            return Status;
        }

        public Double CalculoIgtf(System.Windows.Forms.TextBox NunOrden, string Boivares, System.Windows.Forms.DataGridView Dt_Abono)
        {
            Double IgtfBs = 0;
            Double TotalAboTranferenciaDolar = 0;
            Double TopeMaxIgtf = 0;
            string Resultado_Parametro = _D_DetalleOrden.TB_PARAMETRO("ActivaIGTF");
            bool Cobro_IGTF= Convert.ToBoolean(Convert.ToInt32(Resultado_Parametro));

            if (NunOrden.Text == TB_CAORDSER.NumOrdserv & Cobro_IGTF== true) 
            {

                DataTable dt = _D_DetalleOrden.BucarTotalAbonosRealizados(NunOrden.Text);
                TotalAboTranferenciaDolar = Math.Round(Convert.ToDouble(dt.Rows[0]["TotalPagoIgtf"].ToString()), 2);
                TotalAboTranferenciaDolar = TotalAboTranferenciaDolar + Convert.ToDouble(TotalIgtf(Dt_Abono));
                IgtfBs = Math.Round((Convert.ToDouble(Boivares) * 0.03), 2);


                ////// ****** Esta en fase de prueba si se utiliza el precio total de la orden en dolares o el saldo de la orden en dolares *****
                //JR TopeMaxIgtf = Math.Round(Convert.ToDouble((TB_CAORDSER.Orser_Total_Mon * 0.03) * TB_TASA_Dolar.Tasa), 2);
                TopeMaxIgtf = Math.Round(Convert.ToDouble((TB_CAORDSER.VtaTotal * 0.03)), 2);
                TopeMaxIgtf = Math.Round(TopeMaxIgtf - TotalAboTranferenciaDolar, 2);

                //TopeMaxIgtf = Math.Round(Convert.ToDouble(TB_CAORDSER.OrSer_Saldo * 0.03) , 2);
                //TopeMaxIgtf = Math.Round(TopeMaxIgtf - TotalAboTranferenciaDolar, 2);

                if (TopeMaxIgtf < IgtfBs)
                {
                    IgtfBs = TopeMaxIgtf;
                }


            }

            return IgtfBs;
        }

        public void ConvertirDolaresBolivares(System.Windows.Forms.TextBox Dolar, System.Windows.Forms.TextBox Bolivares, string TipoMoneda)
        {
            double conversion = 0.00;

            if (TipoMoneda == "Dólares")
            {

                if (Convert.ToDouble(Dolar.Text) != 0.00 && Dolar.Text != "")
                {
                    conversion = Math.Round(((Convert.ToDouble(Dolar.Text) * Convert.ToDouble(TB_TASA_Dolar.Tasa))), 2);
                    Bolivares.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", conversion).Replace(".", ",");
                    Bolivares.Text = String.Format("{0:#,0.00}", conversion);
                    //Bolivares.Text = Convert.ToString(conversion);

                }

                else
                    Bolivares.Text = "0.00";

            }

            if (TipoMoneda == "Euros")
            {

                if (Convert.ToDouble(Dolar.Text) != 0.00 && Dolar.Text != "")
                {
                    conversion = Math.Round(((Convert.ToDouble(Dolar.Text) * Convert.ToDouble(TB_TASA_Euro.Tasa))), 2);
                    Bolivares.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", conversion).Replace(".", ",");
                    Bolivares.Text = String.Format("{0:#,0.00}", conversion);
                    //Bolivares.Text = Convert.ToString(conversion);

                }

                else
                    Bolivares.Text = "0.00";
            }


        }

        public bool VerificarAbonoNegativo(System.Windows.Forms.DataGridView Dt_Abono)
        {
            stringBuilder.Clear();
            bool Negativo = false;
            Double Bolivares = 0.00;

            foreach (DataGridViewRow row in Dt_Abono.Rows)
            {
                if (row.Cells["Bs"].Value.ToString() != "")
                {
                    Bolivares = Convert.ToDouble(row.Cells["Bs"].Value.ToString());

                    if (Bolivares < 0)
                    {
                        Negativo = true;
                        return Negativo;
                    }
                }
            }

            return Negativo;
        }




        public string TotalizarAbono(System.Windows.Forms.DataGridView Dt_Abono)
        {
            stringBuilder.Clear();
            string total = "";
            Double Bolivares = 0;

            foreach (DataGridViewRow row in Dt_Abono.Rows)
            {
                string prueba = row.Cells["TipoPago"].Value.ToString();
                if (row.Cells["Bs"].Value.ToString() != "")
                {
                    Bolivares = Bolivares + Convert.ToDouble(row.Cells["Bs"].Value.ToString().Replace(".", ""));
                }
            }

            total = Convert.ToString(Math.Round(Bolivares, 2));

            return total;
        }

        public string TotalIgtf(System.Windows.Forms.DataGridView Dt_Abono)
        {
            stringBuilder.Clear();
            string total = "";
            Double Igtf = 0;

            foreach (DataGridViewRow row in Dt_Abono.Rows)
            {

                Igtf = Igtf + Convert.ToDouble(row.Cells["Igtf"].Value.ToString() == "" ? "0.00" : row.Cells["Igtf"].Value.ToString());
            }

            total = Convert.ToString(Math.Round(Igtf, 2));

            return total;
        }

        public string CalcularNuevoTotalOrden(System.Windows.Forms.DataGridView Dt_Abono)
        {
            stringBuilder.Clear();
            string totalNuevoOrden = "";

            //BsNOT = ((double)(TB_CAORDSER.OrSer_Saldo_Mon * TB_TASA_Dolar.Tasa));
            BsNOT = TB_CAORDSER.OrSer_Saldo;
            IgtfNOT = Convert.ToDouble(TotalIgtf(Dt_Abono));
            Double Abono = Convert.ToDouble(TotalizarAbono(Dt_Abono));

            totalNuevoOrden = Convert.ToString(Math.Round((BsNOT + IgtfNOT) - Abono, 2));
            totalNuevoOrden =  totalNuevoOrden.Replace(".", ",");
            totalNuevoOrden =  string.Format("{0:#,0.00}", Convert.ToDecimal(totalNuevoOrden));
            return totalNuevoOrden;
        }


        public void GuardarAbonoGrid(int idAbono,DataTable Dt_Abonos, string MedioPago, string Moneda, string Banco, string Bolivares, string NunTranferencia, string Fecha, string CodPago = "", string CodBanco = "",  string Cod_BancoRecep = "",string Vuelto = "", string Ref = "", string Igtf = "", string CVC = "", string Vence = "", string TipoTrajeta = "", string Abo_CVCNROCHEQUE = "", string Tipo_Punto = "000")
        {
            stringBuilder.Clear();
            Dt_Abonos.Rows.Add(idAbono , MedioPago, Moneda, Ref, Banco, Igtf, Bolivares, NunTranferencia, Fecha, CodPago, CodBanco, Vuelto, CVC, Vence, TipoTrajeta, Abo_CVCNROCHEQUE, Tipo_Punto,Cod_BancoRecep);

        }

        public void CrearTabla(DataTable Dt_Abonos)
        {

            //************crear Tabla*****************
            DataColumn column0 = new DataColumn("IdAbono");
            DataColumn column1 = new DataColumn("TipoPago");
            DataColumn column2 = new DataColumn("Moneda");
            DataColumn column3 = new DataColumn("Ref");
            DataColumn column4 = new DataColumn("Banco");
            DataColumn column5 = new DataColumn("Igtf");
            DataColumn column6 = new DataColumn("Bs");
            DataColumn column7 = new DataColumn("N°Tranferencia");
            DataColumn column8 = new DataColumn("Fecha");
            DataColumn column9 = new DataColumn("CodPago");
            DataColumn column10 = new DataColumn("CodBanco");
            DataColumn column11 = new DataColumn("Vuelto");
            DataColumn column12 = new DataColumn("CVC");
            DataColumn column13 = new DataColumn("Vence");
            DataColumn column14 = new DataColumn("TipoTrajeta");
            DataColumn column15 = new DataColumn("Abo_CVCNROCHEQUE");
            DataColumn column16 = new DataColumn("Tipo_Punto");
            DataColumn column17 = new DataColumn("Cod_BancoRecep");

            Dt_Abonos.Columns.Add(column0);
            Dt_Abonos.Columns.Add(column1);
            Dt_Abonos.Columns.Add(column2);
            Dt_Abonos.Columns.Add(column3);
            Dt_Abonos.Columns.Add(column4);
            Dt_Abonos.Columns.Add(column5);
            Dt_Abonos.Columns.Add(column6);
            Dt_Abonos.Columns.Add(column7);
            Dt_Abonos.Columns.Add(column8);
            Dt_Abonos.Columns.Add(column9);
            Dt_Abonos.Columns.Add(column10);
            Dt_Abonos.Columns.Add(column11);
            Dt_Abonos.Columns.Add(column12);
            Dt_Abonos.Columns.Add(column13);
            Dt_Abonos.Columns.Add(column14);
            Dt_Abonos.Columns.Add(column15);
            Dt_Abonos.Columns.Add(column16);
            Dt_Abonos.Columns.Add(column17);

            //*************************************************
        }

        public void CrearTablaPagoMovil(DataTable Dt_PagoMovil)
        {

            //************crear Tabla*****************
            DataColumn column0 = new DataColumn("IdAbonoPagoMovil");
            DataColumn column1 = new DataColumn("Nacionalidad");
            DataColumn column2 = new DataColumn("Cedula");
            DataColumn column3 = new DataColumn("PrefijoCelular");
            DataColumn column4 = new DataColumn("Celular");
            DataColumn column5 = new DataColumn("MontoVueltoBs");
            DataColumn column6 = new DataColumn("Banco");
            DataColumn column7 = new DataColumn("MontoRecibidoRef");
            DataColumn column8 = new DataColumn("MontoVueltoRef");
            DataColumn column9 = new DataColumn("Moneda");

            Dt_PagoMovil.Columns.Add(column0);
            Dt_PagoMovil.Columns.Add(column1);
            Dt_PagoMovil.Columns.Add(column2);
            Dt_PagoMovil.Columns.Add(column3);
            Dt_PagoMovil.Columns.Add(column4);
            Dt_PagoMovil.Columns.Add(column5);
            Dt_PagoMovil.Columns.Add(column6);
            Dt_PagoMovil.Columns.Add(column7);
            Dt_PagoMovil.Columns.Add(column8);
            Dt_PagoMovil.Columns.Add(column9);


            //*************************************************
        }

        public void GuardarPagoMovilTabla(int idAbonoPagoMovil,DataTable Dt_PagoMovil, string Nacionalidad, string Cedula, string PrefijoCelular, string Celular, string MontoVueltoBs, string Banco, string MontoRecibidoRef, string MontoVueltoRef, string Moneda)
        {
            stringBuilder.Clear();
            Dt_PagoMovil.Rows.Add(idAbonoPagoMovil,Nacionalidad, Cedula, PrefijoCelular, Celular, MontoVueltoBs, Banco, MontoRecibidoRef, MontoVueltoRef, Moneda);


        }


        public bool BuscarNotas(string numDocumento, bool nota, string cedulaCliente, System.Windows.Forms.DataGridView DgvNotas)
        {
            try
            {
                DgvNotas.DataSource = "";
                DgvNotas.DataMember = "";

                var dataGridViewColumn = DgvNotas.Columns["RdButom"];

                if (dataGridViewColumn != null && dataGridViewColumn.Visible)

                {

                    DgvNotas.Columns.RemoveAt(DgvNotas.Columns.Count - 1);
                }


                stringBuilder.Clear();
                DataTable dt = _D_DetalleOrden.BuscarNota(numDocumento, nota, cedulaCliente);
                if (dt.Rows.Count > 0)
                {
                    DgvNotas.DataSource = dt;
                }

                else
                {
                    stringBuilder.Append(Environment.NewLine + "Este Nro de documento no tiene notas de credito asociadas ");
                }

                return stringBuilder.Length == 0;

            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return stringBuilder.Length == 0;
            }
        }


        public bool CargarGridNotasTotales(System.Windows.Forms.DataGridView DgvNotas, DataTable dt)
        {
            try
            {
                DgvNotas.DataSource = "";
                DgvNotas.DataMember = "";

                var dataGridViewColumn = DgvNotas.Columns["RdButom"];

                if (dataGridViewColumn != null && dataGridViewColumn.Visible)

                {

                    DgvNotas.Columns.RemoveAt(DgvNotas.Columns.Count - 1);
                }


                stringBuilder.Clear();
                if (dt.Rows.Count > 0)
                {
                    DgvNotas.DataSource = dt;
                }

                return stringBuilder.Length == 0;

            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return stringBuilder.Length == 0;
            }
        }


        public bool BuscarNotasGrid(System.Windows.Forms.DataGridView Dt_Abono)
        {
            bool validar = false;

            try
            {
                stringBuilder.Clear();
                foreach (DataGridViewRow row in Dt_Abono.Rows)
                {

                    if (row.Cells["TipoPago"].Value.ToString() == "Nota Credito")
                    {
                        validar = true;
                        stringBuilder.Append(Environment.NewLine + "No se puede cancelar una orden con mas de una nota");
                    }

                    else
                    {
                        validar = false;
                    }
                }

                return validar;

            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                validar = true;
                return validar;
            }
        }

        public string CargarAbonos(System.Windows.Forms.DataGridView Dt_Abono)
        {
            string resuesta = "";
            try
            {
                stringBuilder.Clear();




                return resuesta;
            }

            catch (Exception ex)
            {
                resuesta = ex.Message;
                return resuesta;
            }

        }

        public DataTable MostarPagosGrid(string sucursal, string orden, string Revision)
        {
            DataTable Pagos;
            try
            {
                Pagos = _D_DetalleOrden.CargarPagosGrid(sucursal, orden, Revision);

                return Pagos;
            }

            catch (Exception ex)
            {
                return null;
            }

        }

        public string LLenadoParametros(System.Windows.Forms.DataGridView Dt_Abono)
        {

            string rep = "";
            string Cod_Sucursal;
            string NumOrdserv;
            string Revision;
            string Tipo_Pago;
            string Cod_Banco;
            string Abo_CTATARJETA;
            string Abo_CVCNROCHEQUE;
            DateTime vas;
            string Abo_Fecha;
            Double Abo_Monto;
            string Abo_Tipo = "";
            string Tipo_Pto = "XX";
            string CodPunto = "000";
            string Anulado;
            string Fec_Crea;
            string Fec_Mod;
            string USER_Crea;
            string USER_Mod;
            string Fecha;
            string Fecha_Abono = "";
            string Cod_BancoRecep = "";
            Double Tasa_Abono;
            Double Tasa_Dolar;
            Double Abo_Monto_Divisa = 0;
            string Abo_Monto_SinIGTF = "";
            string Abo_IGTF = "";
            string OrSer_Tipo_Mon = "01";

            foreach (DataGridViewRow Row in Dt_Abono.Rows)
            {

                Cod_Sucursal = _D_Inicio.Sucursal();
                NumOrdserv = TB_CAORDSER.NumOrdserv;
                Revision = TB_CAORDSER.Revision;
                Tipo_Pago = Convert.ToString(Row.Cells["CodPago"].Value);
                Abo_Tipo = Convert.ToString(Row.Cells["TipoPago"].Value);
                Abo_CTATARJETA = "";
                Tasa_Dolar = Convert.ToDouble(TB_TASA_Dolar.Tasa);
                Abo_CVCNROCHEQUE = "";
                Abo_Fecha = "";
                CodPunto = Convert.ToString(Row.Cells["Tipo_Punto"].Value);


                // Agregado 25-08-2023

                if (Row.Cells["TipoPago"].Value == "Cashea" | Row.Cells["TipoPago"].Value == "Pago Móvil")
                {
                    Abo_Tipo = "Transferencia";
                }

                if (Row.Cells["CodPago"].Value == "024")
                {
                    Tipo_Pago = "021";
                }


                if (Row.Cells["TipoPago"].Value == "Efectivo Divisa")
                {
                    Abo_Tipo = "Transferencia Divisa";
                }

                if (Row.Cells["CodPago"].Value == "023")
                {
                    Tipo_Pago = "022";
                }

                //////////////////////////////////////



                if (Tipo_Pago == "001")
                {
                    Cod_Banco = "000";
                }
                else
                {
                    Cod_Banco = Convert.ToString(Row.Cells["CodBanco"].Value);
                }


                if (Row.Cells["Bs"].Value.ToString() == "")
                {
                    Abo_Monto = 0;
                }
                else
                {
                    Abo_Monto = Convert.ToDouble(Row.Cells["Bs"].Value.ToString().Replace(".", ""));
                }


                if (Abo_Tipo == "Debito")
                {
                    Tipo_Pto = "TD";
                    // CodPunto = "003";
                    Abo_CTATARJETA = L_EncriptarDesEscriptar.funEncriptarKM(Convert.ToString(Row.Cells["N°Tranferencia"].Value));
                    Abo_CVCNROCHEQUE = Convert.ToString(Row.Cells["CVC"].Value);
                    Fecha_Abono = "";
                }

                if (Abo_Tipo == "Credito" | Abo_Tipo == "Tarjeta de Credito" | Abo_Tipo == "Visa" | Abo_Tipo == "Master" | Abo_Tipo == "American" | Abo_Tipo == "Dinners" | Abo_Tipo == "Sisa")
                {
                    Tipo_Pto = "TC";
                    // CodPunto = "003";
                    Abo_CTATARJETA = L_EncriptarDesEscriptar.funEncriptarKM(Convert.ToString(Row.Cells["N°Tranferencia"].Value));
                    Abo_CVCNROCHEQUE = Convert.ToString(Row.Cells["CVC"].Value);
                    Fecha_Abono = "";
                    Abo_Fecha = Convert.ToString(Row.Cells["Vence"].Value).Substring(0, Row.Cells["Vence"].Value.ToString().Length - 2) + "/" + Convert.ToString(Row.Cells["Vence"].Value).Substring(2, Row.Cells["Vence"].Value.ToString().Length - 2);
                }

                if (Abo_Tipo == "Transferencia Divisa" | Abo_Tipo == "Transferencia")
                {
                    Fecha_Abono = Convert.ToDateTime(Row.Cells["Fecha"].Value).ToString("yyyyMMdd");
                    Abo_CVCNROCHEQUE = Convert.ToString(Row.Cells["N°Tranferencia"].Value);  

                    if (Abo_Tipo == "Transferencia Divisa")
                    {
                        Cod_BancoRecep = Cod_Banco;
                        if (Row.Cells["Igtf"].Value != "")
                        {
                            Abo_IGTF = Convert.ToString(Convert.ToDouble(Row.Cells["Igtf"].Value)).Replace(",", ".");
                            Abo_Monto_SinIGTF = Convert.ToString(Convert.ToDouble(Row.Cells["Bs"].Value) - Convert.ToDouble(Abo_IGTF.Replace(".", ","))).Replace(",", ".");
                            Abo_Fecha = "01";
                        }
                        Tipo_Pago = "021";
                        Abo_Tipo = "TRANSFERENCIA";

                    }
                    else
                    {
                        Cod_BancoRecep = Convert.ToString(Row.Cells["Cod_BancoRecep"].Value);
                        Abo_IGTF = "0.000000";
                        Abo_Monto_SinIGTF = "0.000000";
                    }

                }

                if (Abo_Tipo == "Nota Devolucion" | Abo_Tipo == "Nota Credito")
                {
                    
                    Abo_CVCNROCHEQUE = Convert.ToString(Row.Cells["Abo_CVCNROCHEQUE"].Value);
                }

                Anulado = "False";
                Fec_Crea = Convert.ToDateTime(Row.Cells["Fecha"].Value).ToString("yyyyMMdd");
                Fec_Mod = "";
                USER_Crea = TB_USUARIO.COD_USR;
                USER_Mod = "";
                Fecha = Convert.ToDateTime(Row.Cells["Fecha"].Value).ToString("yyyyMMdd");
                if (Row.Cells["Ref"].Value != "")
                {
                    Abo_Monto_Divisa = Convert.ToDouble(Row.Cells["Ref"].Value);
                }
                else
                {
                    Abo_Monto_Divisa = 0;
                }


                if (Row.Cells["Moneda"].Value == "Euros")
                {
                    OrSer_Tipo_Mon = "02";
                    Tasa_Abono = Convert.ToDouble(TB_TASA_Euro.Tasa);
                    Abo_Fecha = "02";
                }
                else
                {
                    OrSer_Tipo_Mon = "01";
                    Tasa_Abono = Convert.ToDouble(TB_TASA_Dolar.Tasa);
                }

                rep = _D_DetalleOrden.GetAbono(Cod_Sucursal, NumOrdserv, Revision, Tipo_Pago, Cod_Banco, Abo_CTATARJETA, Abo_CVCNROCHEQUE, Abo_Fecha, Abo_Monto, (Abo_Tipo.ToUpper(new CultureInfo("tr-TR", false))), Tipo_Pto, CodPunto, Anulado, Fec_Crea, Fec_Mod, USER_Crea, USER_Mod, _D_Inicio.DiaActivo().ToString("yyyyMMdd"), Fecha_Abono, Cod_BancoRecep, Tasa_Abono, Abo_Monto_Divisa, Abo_Monto_SinIGTF, Abo_IGTF, OrSer_Tipo_Mon, Tasa_Dolar);

                Tipo_Pto = "XX";
                // CodPunto = "000";
                Fecha_Abono = "";
                Cod_BancoRecep = "";
                Abo_Monto_SinIGTF = "";
                Abo_IGTF = "";
            }

            return rep;
        }

        public string RegistarNotaCredito(System.Windows.Forms.DataGridView Dt_Abono, string cedulaCliente, string NumerFact, SqlCommand command)
        {
            stringBuilder.Clear();

            try
            {
                Double Bolivares;
                string NumeroNota;
                string resultado = "SATISFACTORIO";

                foreach (DataGridViewRow row in Dt_Abono.Rows)
                {

                    if (Convert.ToString(row.Cells["TipoPago"].Value) == "Nota Credito" && Convert.ToString(row.Cells["CodPago"].Value) == "006")
                    {
                        Bolivares = Convert.ToString(row.Cells["Bs"].Value) == "" ? (Double)0.00 : Convert.ToDouble(row.Cells["Bs"].Value);
                        NumeroNota = Convert.ToString(row.Cells["Abo_CVCNROCHEQUE"].Value) == "" ? (string)"" : Convert.ToString(row.Cells["Abo_CVCNROCHEQUE"].Value);
                        resultado = _D_DetalleOrden.RegistarNota(Bolivares, NumeroNota, NumerFact, cedulaCliente, command);
                    }


                }

                return resultado;

            }

            catch (Exception ex)
            {

                string Error = string.Format("Error: {0}", ex.Message);
                return Error;
            }


        }

        public bool ValidaFactManual(SqlCommand command= null)// Valida si es factura manual
        {
            stringBuilder.Clear();

            try
            {
                ////////****************   Asi se Validaba la Factura Manual Antes   *********************///////////////
                //    DataSet dsFactManual = new DataSet();
                //    DataSet dsIGTF = new DataSet();

                //    dsFactManual = _D_DetalleOrden.GetFactManual(TB_CAORDSER.NumOrdserv, "", command);
                //    dsIGTF = _D_DetalleOrden.PagosConIGTFVal(TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv, command);

                //    bool FactManual;

                //    if ((dsFactManual.Tables[0].Rows.Count == 0) & (dsFactManual.Tables[1].Rows.Count == 0) & ((Convert.ToInt32(dsIGTF.Tables[0].Rows[0]["Abo_Monto"]) > 0 & dsIGTF.Tables[0].Rows[0]["FacturaManualIGTF"].ToString() == "False") | Convert.ToInt32(dsIGTF.Tables[0].Rows[0]["Abo_Monto"]) == 0))
                //    {
                //        FactManual = false;
                //    }
                //    else
                //    {
                //        FactManual = true;
                //    }

                //    return FactManual;

                ////////****************   Nueva forma de validar la Factura Manual 25-01-2024  *********************///////////////
                ///
                string FactManual = _D_DetalleOrden.Validar_Factura_Manual("FactManual");

                if (FactManual =="1")
                {
                    return true;
                }
                else if(FactManual == "0")
                {
                    return false;
                }
                else
                {
                    stringBuilder.Append(Environment.NewLine + "Error en la funcion ValidaFactManual: Formato de Cadena no Valido" );
                    return false;
                }

            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + "Error en la funcion ValidaFactManual: " + ex.Message.ToString());
                return false;
            }
        }

        public bool ValidacionNumFact(System.Windows.Forms.TextBox NroFact, System.Windows.Forms.MaskedTextBox NroControl) // Valida que si es factura manual el campo numero de factura este debidamente lleno. 
        {
            bool completo;
            string control;

            // Se convierte a string el control para poder hacer el replace
            control = Convert.ToString(NroControl.Text);

            // Se hace el replace para poder validar el numero de caracteres del numero de control 
            control = control.Replace("_","");

            if (ValidaFactManual() == true & NroFact.TextLength == 7 & NroControl.Text!= "  -" & control.Length == 11 )
            {
                completo = true;

            }
            else
            {
                completo = false;

            }

            if (ValidaFactManual() == false)
            {

                completo = true;
            }
            return completo;


        }

        public ValuesTransposicion Transposicion(Double ESF, Double CIL, int EJE)
        {
            ValuesTransposicion _ValuesTransposicion = new ValuesTransposicion();

            _ValuesTransposicion.TransESF = ESF + CIL;
            _ValuesTransposicion.TransCIL = -1 * (CIL);

            if (EJE <= 90)
            {
                _ValuesTransposicion.TransEJE = EJE + 90;
            }
            if (EJE > 90)
            {
                _ValuesTransposicion.TransEJE = EJE - 90;
            }

            return _ValuesTransposicion;
        }

        public void BuscarCliente(string CEDULA)
        {
            _D_DetalleOrden.ObtenerClientePagador(CEDULA);
            if (_D_DetalleOrden.ClientePagador == true)
            {
                MostrarClientePag = true;
                NombreCliente = _D_DetalleOrden.Nombre;
                CedCliente = _D_DetalleOrden.CED;
                TlfCliente = _D_DetalleOrden.Tlf;
                CorreoCliente = _D_DetalleOrden.Correo;
                CorreoCliente = CorreoCliente.Replace(" ", ""); // Para quitar los espacios 


            }
            else
            {
                MostrarClientePag = false;


            }

        }

        public void BuscarTlfCorreo(string Orden)
        {
            _D_DetalleOrden.ObtenerTlfCorreo(Orden);

            TlfCliente = _D_DetalleOrden.Tlf;
            CorreoCliente = _D_DetalleOrden.Correo;
            CorreoCliente = CorreoCliente.Replace(" ", ""); // Para quitar los espacios 


        }

        public void AgregarGarantia()
        {
            try
            {
                if ((_D_DetalleOrden.TB_PARAMETROSPGE("PGEActivo")) == "1")
                {
                    if (TB_CAORDSER.Cod_DetVta == "09" | TB_CAORDSER.Asegurada == true)
                    {
                        _D_DetalleOrden.ActivarGarantia(TB_CAORDSER.NumOrdserv, TB_CAORDSER.Cod_Sucursal, TB_USUARIO.COD_USR);
                    }
                }

            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);

            }
        }

        public string RespaldarNota(System.Windows.Forms.DataGridView Dt_Abono, string cedulaCliente, string NumerFact, string Transaccion)
        {
            string Rsp = "SATISFACTORIO";

            try
            {
                string NumeroNota;

                foreach (DataGridViewRow row in Dt_Abono.Rows)
                {

                    if (Convert.ToString(row.Cells["TipoPago"].Value) == "Nota Credito" && Convert.ToString(row.Cells["CodPago"].Value) == "006")
                    {
                        NumeroNota = Convert.ToString(row.Cells["Abo_CVCNROCHEQUE"].Value) == "" ? (string)"" : Convert.ToString(row.Cells["Abo_CVCNROCHEQUE"].Value);
                        Rsp = _D_DetalleOrden.RespaldarNota(TB_CAORDSER.Cod_Sucursal, NumeroNota, NumerFact, cedulaCliente, Transaccion);
                    }


                }
                return Rsp;
            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return Error;
            }

        }

        public void LLenarComboboxTipoPunto(System.Windows.Forms.ComboBox Punto, string Tipo_punto)
        {
            Punto.DataSource = _D_DetalleOrden.Punto(Tipo_punto);
            Punto.DisplayMember = "Indexx";
            Punto.ValueMember = "Value";

        }

        public bool ValidoNumeroTarjeta_Credito(string NumeroTarjeta, string TipoTarjeta)
        {
            string str;
            bool ValidoNumeroTarjetaCredito= false;
            TipoTarjeta = TipoTarjeta.ToUpper();
            str = NumeroTarjeta.Substring(0, 2);
            switch (TipoTarjeta)
            {
                case "VISA":
                    {
                        if (str.Substring(0, 1) == "4")
                            ValidoNumeroTarjetaCredito = true;
                        else if (str.Substring(0, 1) == "8")
                            ValidoNumeroTarjetaCredito = true;
                        else
                            ValidoNumeroTarjetaCredito = false;
                        break;
                    }

                case "MASTER":
                    {
                        if (str.Substring(0, 1) == "5")
                            ValidoNumeroTarjetaCredito = true;
                        else
                            ValidoNumeroTarjetaCredito = false;
                        break;
                    }

                case "AMERICAN":
                    {
                        if (str == "37")
                            ValidoNumeroTarjetaCredito = true;
                        else
                            ValidoNumeroTarjetaCredito = false;
                        break;
                    }

                case "DINNERS":
                    {
                        if (str == "36")
                            ValidoNumeroTarjetaCredito = true;
                        else
                            ValidoNumeroTarjetaCredito = false;
                        break;
                    }

                case "SISA":
                    {
                        if (str == "12")
                        {
                            if (NumeroTarjeta.StartsWith("1230") == true)
                                ValidoNumeroTarjetaCredito = true;
                            else
                                ValidoNumeroTarjetaCredito = false;
                        }
                        else
                            ValidoNumeroTarjetaCredito = false;
                        break;
                    }
                   
            }
            return ValidoNumeroTarjetaCredito;
        }



        //Comentar
        public bool BuscarNotasDevolucion(string numDocumento, bool nota, string cedulaCliente, System.Windows.Forms.DataGridView DgvNotas)
        {
            try
            {
                DgvNotas.DataSource = "";
                DgvNotas.DataMember = "";

                var dataGridViewColumn = DgvNotas.Columns["RdButom"];

                if (dataGridViewColumn != null && dataGridViewColumn.Visible)

                {

                    DgvNotas.Columns.RemoveAt(DgvNotas.Columns.Count - 1);
                }


                stringBuilder.Clear();
                DataTable dt = _D_DetalleOrden.BuscarNota(numDocumento, nota, cedulaCliente);
                if (dt.Rows.Count > 0)
                {
                    DgvNotas.DataSource = dt;
                }

                else
                {
                    stringBuilder.Append(Environment.NewLine + "Este Nro de documento no tiene notas de devolución asociadas ");
                }

                return stringBuilder.Length == 0;

            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return stringBuilder.Length == 0;
            }
        }

        public bool BuscarNotasDevolucionGrid(System.Windows.Forms.DataGridView Dt_Abono,string sucursal, string orden, string Revision)
        {
            bool validar = false;

            try
            {
                stringBuilder.Clear();
                DataTable DtNostasOrden = _D_DetalleOrden.CargarPagosGrid(sucursal,orden, Revision);

                //valido si la orden fue abonada con una nota de devolucion 
                foreach (DataRow row in DtNostasOrden.Rows)
                {

                    if (row["Tipo_Pago"].ToString() == "010")
                    {
                        validar = true;
                        stringBuilder.Append(Environment.NewLine + "Está orden ya fue abonada con una nota de devolución");
                    }

                    else
                    {
                        validar = false;
                    }
                }
                //valido si la orden tiene una nota de devolucion en el grid de pagos
                if (validar== false)
                {
                    foreach (DataGridViewRow row in Dt_Abono.Rows)
                    {

                        if (row.Cells["TipoPago"].Value.ToString() == "Nota Devolucion")
                        {
                            validar = true;
                            stringBuilder.Append(Environment.NewLine + "Está orden ya fue abonada con una nota de devolución");
                        }

                        else
                        {
                            validar = false;
                        }
                    }
                }
              

                return validar;

            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                validar = true;
                return validar;
            }
        }

        public string ActualizarSaldoNotaDevolucion(System.Windows.Forms.DataGridView Dt_Abono, string cedulaCliente, string NumOrdserv, SqlCommand command)
        {
            try
            {
                Double Bolivares;
                string NumeroNota;
                string resultado = "SATISFACTORIO";

                foreach (DataGridViewRow row in Dt_Abono.Rows)
                {

                    if (Convert.ToString(row.Cells["TipoPago"].Value) == "Nota Devolucion" && Convert.ToString(row.Cells["CodPago"].Value) == "010")
                    {
                        Bolivares = Convert.ToString(row.Cells["Bs"].Value) == "" ? (Double)0.00 : Convert.ToDouble(row.Cells["Bs"].Value);
                        NumeroNota = Convert.ToString(row.Cells["Abo_CVCNROCHEQUE"].Value) == "" ? (string)"" : Convert.ToString(row.Cells["Abo_CVCNROCHEQUE"].Value);
                        resultado = _D_DetalleOrden.ActualizarSaldoNotaDevolucion(Bolivares, NumeroNota, NumOrdserv, cedulaCliente,TB_USUARIO.COD_USR, command);
                    }


                }

                return resultado;
            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return Error;
            }


        }

        public string RespaldarNotaDevolucion(System.Windows.Forms.DataGridView Dt_Abono, string cedulaCliente, string NumOrdservAsociadoNota, string Transaccion)
        {
            string Rsp = "SATISFACTORIO"; 

            try
            {
                string NumeroNota;

                foreach (DataGridViewRow row in Dt_Abono.Rows)
                {

                    if (Convert.ToString(row.Cells["TipoPago"].Value) == "Nota Devolucion" && Convert.ToString(row.Cells["CodPago"].Value) == "010")
                    {
                        NumeroNota = Convert.ToString(row.Cells["Abo_CVCNROCHEQUE"].Value) == "" ? (string)"" : Convert.ToString(row.Cells["Abo_CVCNROCHEQUE"].Value);
                        Rsp = _D_DetalleOrden.RespaldarNotaDevolucion(TB_CAORDSER.Cod_Sucursal, NumeroNota, NumOrdservAsociadoNota, cedulaCliente, Transaccion);
                    }


                }
                return Rsp;
            }

            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return Error;
            }

        }

        // Esta funcion Guarda los billetes en el datatable
        public void GuardarBilletesGrid(DataTable Dt_Billetes, string CodigoBillete, string MontoBillete, string TipoBillete)
        {
            stringBuilder.Clear();
            Dt_Billetes.Rows.Add(CodigoBillete, MontoBillete, TipoBillete);

        }


        // Esta funcion crea el Datatable y le inserta las columnas
        public void CrearTablaBilletes(DataTable Dt_Billetes)
        {

            DataColumn column1 = new DataColumn("Cod_Billete");
            DataColumn column2 = new DataColumn("Monto_Bill");
            DataColumn column3 = new DataColumn("Tipo");

            Dt_Billetes.Columns.Add(column1);
            Dt_Billetes.Columns.Add(column2);
            Dt_Billetes.Columns.Add(column3);


        }

        public string EnviarBilltesBD(string NumOrdenserv, string Tipo,string Monto ,string Serial, string Fec_Crea, string USER_Crea, string Transaccion, SqlCommand command)
        {

            string RESULTADO = _D_DetalleOrden.InsertarBilletes(NumOrdenserv, Tipo, Monto,Serial,Fec_Crea, USER_Crea, Transaccion, command);
            return RESULTADO;

        }

        public string RegistarAbonos(string Cod_Sucursal, string NumOrdserv, string Revision, string USER_Crea, Double Tasa_Dolar, SqlCommand command)
        {

            try
            {

                string resultado = _D_DetalleOrden.InsertarAbono(Cod_Sucursal, NumOrdserv, Revision, USER_Crea, Tasa_Dolar, command);
                return resultado;

            }

            catch (Exception ex)
            {

                string Error = string.Format("Error: {0}", ex.Message);
                return Error;
            }


        }

        public DataSet CrearTablaExamen()
        {
            DataSet Examen_Orden_Convencional = new DataSet();

            DataTable Datos_Examen = Examen_Orden_Convencional.Tables.Add();

            //************crear Tabla*****************
            ////---------Datos del Examen-----------------------

            DataColumn column1 = new DataColumn("Tipo_trabajo");
            DataColumn column2 = new DataColumn("Laboratorio");
            DataColumn column3 = new DataColumn("Servicio");
            DataColumn column4 = new DataColumn("Fecha_ofrecido");
            DataColumn column5 = new DataColumn("Optometrista");
            DataColumn column6 = new DataColumn("N_examen");
            DataColumn column7 = new DataColumn("Fecha_examen");

            Datos_Examen.Columns.Add(column1);
            Datos_Examen.Columns.Add(column2);
            Datos_Examen.Columns.Add(column3);
            Datos_Examen.Columns.Add(column4);
            Datos_Examen.Columns.Add(column5);
            Datos_Examen.Columns.Add(column6);
            Datos_Examen.Columns.Add(column7);

            ////---------Formulas del pasiente-----------------------

            DataTable Formulas = Examen_Orden_Convencional.Tables.Add();

            DataColumn columnn1 = new DataColumn("Nombre");
            DataColumn columnn2 = new DataColumn("Esfera");
            DataColumn columnn3 = new DataColumn("Cilindro");
            DataColumn columnn4 = new DataColumn("Eje");
            DataColumn columnn5 = new DataColumn("ADD");
            DataColumn columnn6 = new DataColumn("Lejos");
            DataColumn columnn7 = new DataColumn("Cerca");
            DataColumn columnn8 = new DataColumn("Altura");
            DataColumn columnn9 = new DataColumn("Vision");

            Formulas.Columns.Add(columnn1);
            Formulas.Columns.Add(columnn2);
            Formulas.Columns.Add(columnn3);
            Formulas.Columns.Add(columnn4);
            Formulas.Columns.Add(columnn5);
            Formulas.Columns.Add(columnn6);
            Formulas.Columns.Add(columnn7);
            Formulas.Columns.Add(columnn8);
            Formulas.Columns.Add(columnn9);

            ////---------Medidas de la montura -----------------------

            DataTable Monturas = Examen_Orden_Convencional.Tables.Add();

            DataColumn column_1 = new DataColumn("Horizontal");
            DataColumn column_2 = new DataColumn("Vertical");
            DataColumn column_3 = new DataColumn("Maxima");
            DataColumn column_4 = new DataColumn("DEL");
            DataColumn column_5 = new DataColumn("DV");
            DataColumn column_6 = new DataColumn("AP");
            DataColumn column_7 = new DataColumn("AF");
            DataColumn column_8 = new DataColumn("DDL");

            Monturas.Columns.Add(column_1);
            Monturas.Columns.Add(column_2);
            Monturas.Columns.Add(column_3);
            Monturas.Columns.Add(column_4);
            Monturas.Columns.Add(column_5);
            Monturas.Columns.Add(column_6);
            Monturas.Columns.Add(column_7);
            Monturas.Columns.Add(column_8);

            ////---------Coloracion -----------------------
            DataTable Color = Examen_Orden_Convencional.Tables.Add();

            DataColumn colu1 = new DataColumn("Codigo");
            DataColumn colu2 = new DataColumn("Descripcion");
            DataColumn colu3 = new DataColumn("Porcentaje_material");

            Color.Columns.Add(colu1);
            Color.Columns.Add(colu2);
            Color.Columns.Add(colu3);

            ////---------Coloracion -----------------------
            DataTable Mimesys = Examen_Orden_Convencional.Tables.Add();

            DataColumn column11 = new DataColumn("Codigo_Mimesys");

            Mimesys.Columns.Add(column11);

            ////---------Observacion -----------------------

            DataTable Observacion = Examen_Orden_Convencional.Tables.Add();
            DataColumn column22 = new DataColumn("Observacion");

            Observacion.Columns.Add(column22);


            DataTable Formulas_Contacto = Examen_Orden_Convencional.Tables.Add();

            DataColumn columnnz0 = new DataColumn("Nombre");
            DataColumn columnnz1 = new DataColumn("CurvaBase");
            DataColumn columnnz2 = new DataColumn("Diametro");
            DataColumn columnnz3 = new DataColumn("Esfera");
            DataColumn columnnz4 = new DataColumn("Cilindro");
            DataColumn columnnz5 = new DataColumn("Eje");
            DataColumn columnnz6 = new DataColumn("ADD");
            DataColumn columnnz7 = new DataColumn("Color");

            Formulas_Contacto.Columns.Add(columnnz0);
            Formulas_Contacto.Columns.Add(columnnz1);
            Formulas_Contacto.Columns.Add(columnnz2);
            Formulas_Contacto.Columns.Add(columnnz3);
            Formulas_Contacto.Columns.Add(columnnz4);
            Formulas_Contacto.Columns.Add(columnnz5);
            Formulas_Contacto.Columns.Add(columnnz6);
            Formulas_Contacto.Columns.Add(columnnz7);

            //*************************************************

            return Examen_Orden_Convencional;
        }

        public bool Cargar_Examen_Coloracion_Montura(System.Windows.Forms.DataGridView DgvFormula, System.Windows.Forms.DataGridView DgvMedidasMontura, System.Windows.Forms.DataGridView DgvColoracion,
            System.Windows.Forms.DataGridView DgvMimesys, System.Windows.Forms.DataGridView DgvObservaconExamen, System.Windows.Forms.TextBox TipoTrabajo, System.Windows.Forms.TextBox Laboratorio, System.Windows.Forms.TextBox Servicio,
            System.Windows.Forms.TextBox FecgaOfre, System.Windows.Forms.TextBox Optome, System.Windows.Forms.TextBox NumEaxamen, System.Windows.Forms.TextBox FechaExamen)
          {

            try
            {
                DgvFormula.DataSource = "";
                DgvFormula.DataMember = "";

                DgvMedidasMontura.DataSource = "";
                DgvMedidasMontura.DataMember = "";

                DgvColoracion.DataSource = "";
                DgvColoracion.DataMember = "";

                DgvMimesys.DataSource = "";
                DgvMimesys.DataMember = "";

                DgvObservaconExamen.DataSource = "";
                DgvObservaconExamen.DataMember = "";
            
                // Obtengo el Examen asociado a esta orde
                DataSet DtsExamen = _D_DetalleOrden.OptenerExamenCompleto(TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision);
               
                
                if (DtsExamen != null)
                {
                    if (DtsExamen.Tables[0].Rows.Count > 0)
                    {
                        // Llenar los datos de la orden 
                        foreach (DataRow row in DtsExamen.Tables[0].Rows)
                        {
                            TipoTrabajo.Text = row["TIPOEXAMEN"].ToString().Replace(" ","");
                            Laboratorio.Text = row["DESCRIPCION"].ToString();
                            Servicio.Text = row["Descripcion_servicio"].ToString();
                            FecgaOfre.Text = row["Fec_ofrecido"].ToString().Substring(0,10);
                            Optome.Text = row["NOM_Optm"].ToString().Replace("    ", "");
                            NumEaxamen.Text = row["NumExamen"].ToString();
                            FechaExamen.Text = row["FEC_Examen"].ToString().Substring(0, 10); 
                            break;
                        }

                        if (TipoTrabajo.Text == "CONVENCIONAL")
                        {
                        DataSet Examen_Orden_Convencional = CrearTablaExamen();
                        Formula_Montua_Coloracion(DtsExamen, Examen_Orden_Convencional);

                        DgvFormula.DataSource = Examen_Orden_Convencional.Tables[1];
                        DgvMedidasMontura.DataSource = Examen_Orden_Convencional.Tables[2];
                        DgvColoracion.DataSource = Examen_Orden_Convencional.Tables[3];
                        DgvMimesys.DataSource = Examen_Orden_Convencional.Tables[4];
                        DgvObservaconExamen.DataSource = Examen_Orden_Convencional.Tables[5];

                            return true;
                        }

                        if (TipoTrabajo.Text == "CONTACTO")
                        {
                            DataSet Examen_Orden_Contacto = CrearTablaExamen();
                            Formula_Examen_Contacto(DtsExamen, Examen_Orden_Contacto);
                            DgvFormula.DataSource = Examen_Orden_Contacto.Tables[6];
                            DgvObservaconExamen.DataSource = Examen_Orden_Contacto.Tables[5];
                            return true;
                        }
                    }
            }

                return false;

            }
            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return false;
            }
        }

        public void Formula_Montua_Coloracion(DataSet DtsExamen, DataSet Examen_Orden_Convencional)
        {
            double ESFD = 0.00;
            double ESFI = 0.00;
            double CILD = 0.00;
            double CILI = 0.00;
            int EJED = 0;
            int EJEI = 0;
            double ADDD = 0.00;
            double ADDI = 0.00;
           
            // 2da Refraccion
            double ESFD2 = 0.00;
            double ESFI2 = 0.00;
            double CILD2 = 0.00;
            double CILI2 = 0.00;
            int EJED2 = 0;
            int EJEI2 = 0;

            double LejosD = 0.00;
            double CercaD = 0.00;
            double AlturaD = 0.00;

            double LejosI = 0.00;
            double CercaI = 0.00;
            double AlturaI = 0.00;

            // VISION 
            String VisionD = "";
            String VisionI = "";

            //MEDIDAS DE LA Montura 
            int HORIZONTAL = 0;
            int VERTICAL = 0;
            int MAXIMA = 0;
            int DEL = 0;
            int DV = 0;
            int AP = 0;
            int AF = 0;
            double DDL = 0.00;

            //Coloracion
            String CodigoColoracion = "";
            String DescripColoracion = "";
            String PorcentajeColoracion = "";

            //MIMESYS
            String CodigoMIMESYS = "";

            //Observa
            String Observa = "";


            if (DtsExamen.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow row in DtsExamen.Tables[0].Rows)
                {
                    ESFD = row["ESFD"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["ESFD"].ToString());
                    ESFI = row["ESFI"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["ESFI"].ToString());
                    CILD = row["CILD"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["CILD"].ToString());
                    CILI = row["CILI"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["CILI"].ToString());
                    EJED = row["EJED"] == DBNull.Value ? (int)0 : Convert.ToInt32(row["EJED"].ToString());
                    EJEI = row["EJEI"] == DBNull.Value ? (int)0 : Convert.ToInt32(row["EJEI"].ToString());
                    ADDD = row["ADDD"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["ADDD"].ToString());
                    ADDI = row["ADDI"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["ADDI"].ToString());

                    // 2da Refraccion
                    ESFD2 = row["ESFD2"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["ESFD2"].ToString());
                    ESFI2 = row["ESFI2"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["ESFI2"].ToString());
                    CILD2 = row["CILD2"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["CILD2"].ToString());
                    CILI2 = row["CILI2"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["CILI2"].ToString());
                    EJED2 = row["EJED2"] == DBNull.Value ? (int)0 : Convert.ToInt32(row["EJED2"].ToString());
                    EJEI2 = row["EJEI2"] == DBNull.Value ? (int)0 : Convert.ToInt32(row["EJEI2"].ToString());

                    //Altura 
                    AlturaD = row["ALTD"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["ALTD"].ToString());  
                    AlturaI = row["ALTI"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["ALTI"].ToString());

                    //CERCA
                    CercaD = row["DPDC"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["DPDC"].ToString());
                    CercaI = row["DPIC"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["DPIC"].ToString());


                    //LEJOS
                    LejosD = row["DPDL"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["DPDL"].ToString());
                    LejosI = row["DPIL"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["DPIL"].ToString());

                    //visom
                    VisionD = row["T_TIPOVISIOND"].ToString();
                    VisionI = row["T_TIPOVISIONI"].ToString();

                    //Montura 
                    HORIZONTAL = row["T_HORIZONTAL"] == DBNull.Value ? (int)0 : Convert.ToInt32(row["T_HORIZONTAL"].ToString());
                    VERTICAL = row["T_VERTICAL"] == DBNull.Value ? (int)0 : Convert.ToInt32(row["T_VERTICAL"].ToString());
                    MAXIMA = row["T_MAXIMA"] == DBNull.Value ? (int)0 : Convert.ToInt32(row["T_MAXIMA"].ToString());
                    DEL = row["T_PUENTE"] == DBNull.Value ? (int)0 : Convert.ToInt32(row["T_PUENTE"].ToString());

                    DV = row["T_DISTANCIAVERTICE"] == DBNull.Value ? (int)0 : Convert.ToInt32(row["T_DISTANCIAVERTICE"].ToString());
                    AP = row["T_ANGULOPANTOSCOPICO"] == DBNull.Value ? (int)0 : Convert.ToInt32(row["T_ANGULOPANTOSCOPICO"].ToString());
                    AF = row["T_ANGULOFACIAL"] == DBNull.Value ? (int)0 : Convert.ToInt32(row["T_ANGULOFACIAL"].ToString());
                    DDL = row["T_DISTANCIADELECTURA"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["T_DISTANCIADELECTURA"].ToString());

                    //Coloracion 
                    CodigoColoracion = row["Cod_Coloracion"].ToString();
                    DescripColoracion = row["Desc_Color"].ToString();
                    PorcentajeColoracion = row["Porc_Material"].ToString();

                    //MIMESYS
                    CodigoMIMESYS = row["CodigoMimesys"].ToString();

                    //Observacion 
                    Observa = row["OBSERVACIONES"].ToString();


                }

                if (CILD > 0.00)
                {
                    var Formula = Transposicion(ESFD, CILD, EJED);
                    ESFD = Formula.TransESF;
                    CILD = Formula.TransCIL;
                    EJED = Formula.TransEJE;
                }
                if (CILI > 0)
                {
                    var Formula = Transposicion(ESFI, CILI, EJEI);
                    ESFI = Formula.TransESF;
                    CILI = Formula.TransCIL;
                    EJEI = Formula.TransEJE;
                }

                // 2da Refraccion
                if (CILD2 > 0)
                {
                    var Formula = Transposicion(ESFD2, CILD2, EJED2);
                    ESFD2 = Formula.TransESF;
                    CILD2 = Formula.TransCIL;
                    EJED2 = Formula.TransEJE;
                }
                if (CILI2 > 0)
                {
                    var Formula = Transposicion(ESFI2, CILI2, EJEI2);
                    ESFI2 = Formula.TransESF;
                    CILI2 = Formula.TransCIL;
                    EJEI2 = Formula.TransEJE;
                }


            }

            //-----------------------------Formula-------------------------------
            //Derecho 
            Examen_Orden_Convencional.Tables[1].Rows.Add("OD", String.Format(CultureInfo.InvariantCulture, "{0:0.00}", ESFD).Replace(".", ","), String.Format(CultureInfo.InvariantCulture, "{0:0.00}", CILD).Replace(".", ","), EJED.ToString().Replace(".", ","), String.Format(CultureInfo.InvariantCulture, "{0:0.00}", ADDD).Replace(".", ","), LejosD, CercaD, AlturaD, VisionD);
            Examen_Orden_Convencional.Tables[1].Rows.Add("2da Refracción", String.Format(CultureInfo.InvariantCulture, "{0:0.00}", ESFD2).Replace(".", ","), String.Format(CultureInfo.InvariantCulture, "{0:0.00}", CILD2).Replace(".", ","), EJED2.ToString().Replace(".", ","), "0,00","0","0","0","");
            //Izquierdo 
            Examen_Orden_Convencional.Tables[1].Rows.Add("OI", String.Format(CultureInfo.InvariantCulture, "{0:0.00}", ESFI).Replace(".", ","), String.Format(CultureInfo.InvariantCulture, "{0:0.00}", CILI).Replace(".", ","), EJEI.ToString().Replace(".", ","), String.Format(CultureInfo.InvariantCulture, "{0:0.00}", ADDI).Replace(".", ","), LejosI, CercaI, AlturaI, VisionI);
            Examen_Orden_Convencional.Tables[1].Rows.Add("2da Refracción", String.Format(CultureInfo.InvariantCulture, "{0:0.00}", ESFI2).Replace(".", ","), String.Format(CultureInfo.InvariantCulture, "{0:0.00}", CILI2).Replace(".", ","), EJEI2.ToString().Replace(".", ","), "0,00", "0", "0", "0", "");

            //-----------------------------Montura-------------------------------
            Examen_Orden_Convencional.Tables[2].Rows.Add(HORIZONTAL, VERTICAL, MAXIMA, DEL, DV, AP, AF, DDL);

            //-----------------------------Coloracion-------------------------------
            Examen_Orden_Convencional.Tables[3].Rows.Add(CodigoColoracion, DescripColoracion, PorcentajeColoracion);

            //-----------------------------MIMESYS-------------------------------
            Examen_Orden_Convencional.Tables[4].Rows.Add(CodigoMIMESYS);

            //----------------------------Observacion--------------------------- 
            Examen_Orden_Convencional.Tables[5].Rows.Add(Observa);


        }

        public void Formula_Examen_Contacto(DataSet DtsExamen, DataSet Examen_Orden_Convencional)
        {

            //--Esfera
            double ESFD = 0.00;//Derecho 
            double ESFI = 0.00;//Izquierdo

            //--Cilindro
            double CILD = 0.00;//Derecho 
            double CILI = 0.00;//Izquierdo

            //--Eje
            int EJED = 0;//Derecho 
            int EJEI = 0;//Izquierdo

            //ADD
            double ADDD = 0.00;//Derecho 
            double ADDI = 0.00;//Izquierdo

            //--CurbaBase
            double CBD = 0.00; //Derecho 
            double CBI = 0.00; //Izquierdo

            //--Diametro
            double DIAMD = 0.00;//Derecho 
            double DIAMI = 0.00;//Izquierdo

            //--Color
            string COLOR = "";

            //Observa
            string Observa = "";

            if (DtsExamen.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow row in DtsExamen.Tables[0].Rows)
                {
                    ESFD = row["ESFD"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["ESFD"].ToString());
                    ESFI = row["ESFI"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["ESFI"].ToString());
                    CILD = row["CILD"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["CILD"].ToString());
                    CILI = row["CILI"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["CILI"].ToString());
                    EJED = row["EJED"] == DBNull.Value ? (int)0 : Convert.ToInt32(row["EJED"].ToString());
                    EJEI = row["EJEI"] == DBNull.Value ? (int)0 : Convert.ToInt32(row["EJEI"].ToString());
                    ADDD = row["ADDD"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["ADDD"].ToString());
                    ADDI = row["ADDI"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["ADDI"].ToString());


                    //--CurbaBase
                    CBD = row["CBD"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["CBD"].ToString());
                    CBI = row["CBI"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["CBI"].ToString());

                    //--Diametro
                    DIAMD = row["DIAMD"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["DIAMD"].ToString());
                    DIAMI = row["DIAMI"] == DBNull.Value ? (Double)0.00 : Convert.ToDouble(row["DIAMI"].ToString());


                    COLOR = row["COLOR"].ToString();


                    //Observacion 
                    Observa = row["OBSERVACIONES"].ToString();
                }

                if (CILD > 0.00)
                {
                    var Formula = Transposicion(ESFD, CILD, EJED);
                    ESFD = Formula.TransESF;
                    CILD = Formula.TransCIL;
                    EJED = Formula.TransEJE;
                }
                if (CILI > 0)
                {
                    var Formula = Transposicion(ESFI, CILI, EJEI);
                    ESFI = Formula.TransESF;
                    CILI = Formula.TransCIL;
                    EJEI = Formula.TransEJE;
                }


            }

            //-----------------------------Formula-------------------------------
            //Derecho 
            Examen_Orden_Convencional.Tables[6].Rows.Add("OD", String.Format(CultureInfo.InvariantCulture, "{0:0.00}", CBD).Replace(".", ","), String.Format(CultureInfo.InvariantCulture, "{0:0.00}", DIAMD).Replace(".", ","), String.Format(CultureInfo.InvariantCulture, "{0:0.00}", ESFD).Replace(".", ","), String.Format(CultureInfo.InvariantCulture, "{0:0.00}", CILD).Replace(".", ","), EJED.ToString().Replace(".", ","), String.Format(CultureInfo.InvariantCulture, "{0:0.00}", ADDD).Replace(".", ","), COLOR);
            //Izquierdo 
            Examen_Orden_Convencional.Tables[6].Rows.Add("OI", String.Format(CultureInfo.InvariantCulture, "{0:0.00}", CBI).Replace(".", ","), String.Format(CultureInfo.InvariantCulture, "{0:0.00}", DIAMI).Replace(".", ","), String.Format(CultureInfo.InvariantCulture, "{0:0.00}", ESFI).Replace(".", ","), String.Format(CultureInfo.InvariantCulture, "{0:0.00}", CILI).Replace(".", ","), EJEI.ToString().Replace(".", ","), String.Format(CultureInfo.InvariantCulture, "{0:0.00}", ADDI).Replace(".", ","), "");
            //----------------------------Observacion--------------------------- 
            Examen_Orden_Convencional.Tables[5].Rows.Add(Observa);
        }

        public DataSet CrearTabla_Detalle_Orden()
        {
            DataSet Detalle_Orden = new DataSet();

            DataTable Articulos = Detalle_Orden.Tables.Add();

            //************crear Tabla*****************
            ////---------Articulos-----------------------

            DataColumn column1 = new DataColumn("Codigo_Articulo");
            DataColumn column9 = new DataColumn("Cod_Laboratorio");
            DataColumn column2 = new DataColumn("Descripcion");
            DataColumn column3 = new DataColumn("Cantidad");
            DataColumn column4 = new DataColumn("Precio");
            DataColumn column5 = new DataColumn("Descuento");
            DataColumn column6 = new DataColumn("Total");
            DataColumn column7 = new DataColumn("Impuesto");
            DataColumn column8 = new DataColumn("Ojo");

            Articulos.Columns.Add(column1);
            Articulos.Columns.Add(column9);
            Articulos.Columns.Add(column2);
            Articulos.Columns.Add(column3);
            Articulos.Columns.Add(column4);
            Articulos.Columns.Add(column5);
            Articulos.Columns.Add(column6);
            Articulos.Columns.Add(column7);
            Articulos.Columns.Add(column8);

            ////---------Medidas de la Montura-----------------------

            DataTable Montura = Detalle_Orden.Tables.Add();

            DataColumn columnn1 = new DataColumn("Horizontal");
            DataColumn columnn2 = new DataColumn("Vertical");
            DataColumn columnn3 = new DataColumn("Maxima");
            DataColumn columnn4 = new DataColumn("Puente");

            Montura.Columns.Add(columnn1);
            Montura.Columns.Add(columnn2);
            Montura.Columns.Add(columnn3);
            Montura.Columns.Add(columnn4);

            ////---------Totalizacion -----------------------

            DataTable TotalOrden = Detalle_Orden.Tables.Add();

            DataColumn column_1 = new DataColumn("ReciboPago");
            DataColumn column_2 = new DataColumn("Monto");
            
            TotalOrden.Columns.Add(column_1);
            TotalOrden.Columns.Add(column_2);

           

            ////---------Observacion -----------------------
            DataTable Observa = Detalle_Orden.Tables.Add();

            DataColumn colu1 = new DataColumn("Observacion");

            Observa.Columns.Add(colu1);

            //*************************************************

            return Detalle_Orden;
        }
        public bool Cargar_Detalle_Orden_Parte1(System.Windows.Forms.DataGridView DgvArticulo, System.Windows.Forms.DataGridView DgvMontura, System.Windows.Forms.DataGridView DgvTotales,
           System.Windows.Forms.DataGridView DgvObservaMon, System.Windows.Forms.TextBox txtMontura_quorun, System.Windows.Forms.TextBox txtPromocion, System.Windows.Forms.TextBox txtMontura_propia,
           System.Windows.Forms.TextBox txtCristales_propio, System.Windows.Forms.TextBox TxtEmpresa, System.Windows.Forms.Label lbPromocion)
        {

            try
            {
                DgvArticulo.DataSource = "";
                DgvArticulo.DataMember = "";

                DgvMontura.DataSource = "";
                DgvMontura.DataMember = "";

                DgvTotales.DataSource = "";
                DgvTotales.DataMember = "";

                DgvObservaMon.DataSource = "";
                DgvObservaMon.DataMember = "";


                // Obtengo el Examen asociado a esta orde
                DataSet DtsDetalle_Orden_consulta = _D_DetalleOrden.OptenerDetalleOrdenCompleto(TB_CAORDSER.Cod_Sucursal, TB_CAORDSER.NumOrdserv, TB_CAORDSER.Revision);


                if (DtsDetalle_Orden_consulta != null)
                {
                    if (DtsDetalle_Orden_consulta.Tables[0].Rows.Count > 0)
                    {
                        // Llenar los datos de la orden 
                        foreach (DataRow row in DtsDetalle_Orden_consulta.Tables[0].Rows)
                        {
                            bool Montura_quorun = row["MonturaEnQuorum"] == DBNull.Value ? (Boolean)false : Convert.ToBoolean(row["MonturaEnQuorum"]);

                            if (Montura_quorun == true)
                            {
                                txtMontura_quorun.BackColor = System.Drawing.ColorTranslator.FromHtml("#008080");
                                txtMontura_quorun.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                            }
                            else
                            {
                                txtMontura_quorun.BackColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
                                txtMontura_quorun.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                            }


                            bool Montura_propia = row["MonturaPropia"] == DBNull.Value ? (Boolean)false : Convert.ToBoolean(row["MonturaPropia"]);
                            if (Montura_propia == true)
                            {
                                txtMontura_propia.BackColor = System.Drawing.ColorTranslator.FromHtml("#2e75b6");
                                txtMontura_propia.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                            }
                            else
                                txtMontura_propia.BackColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
                            txtMontura_propia.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");


                            bool Cristales_propio = row["CristalPropio"] == DBNull.Value ? (Boolean)false : Convert.ToBoolean(row["CristalPropio"]);
                            if (Cristales_propio == true)
                            {
                                txtCristales_propio.BackColor = System.Drawing.ColorTranslator.FromHtml("#92d050");
                                txtCristales_propio.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                            }
                            else
                                txtCristales_propio.BackColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
                            txtCristales_propio.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");


                            bool Empresa = row["Ventaafil"] == DBNull.Value ? (Boolean)false : Convert.ToBoolean(row["Ventaafil"]);
                            if (Empresa == true)
                            { 
                            TxtEmpresa.BackColor = System.Drawing.ColorTranslator.FromHtml("#ed7d31");
                            TxtEmpresa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                            }
                            else
                                TxtEmpresa.BackColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
                                TxtEmpresa.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");


                            break;
                        }
                        
                        foreach (DataRow row in DtsDetalle_Orden_consulta.Tables[1].Rows)
                        {
                            if (row["COD_Prom"].ToString() != "" && row["COD_Prom"].ToString() != " " && row["COD_Prom"].ToString() != null && row["Prom_DESCRIP"].ToString() != "" && row["Prom_DESCRIP"].ToString() != " " && row["Prom_DESCRIP"].ToString() != null)
                            {
                                txtPromocion.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffc000");
                                txtPromocion.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                                txtPromocion.Text = row["Prom_DESCRIP"].ToString().ToUpper().Substring(0,1) + row["Prom_DESCRIP"].ToString().ToLower().Substring(1, row["Prom_DESCRIP"].ToString().Length-1);
                                lbPromocion.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffc000");
                                lbPromocion.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                            }
                            else
                            {
                                txtPromocion.BackColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
                                txtPromocion.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                                txtPromocion.Text = "Promoción";
                                lbPromocion.BackColor = System.Drawing.ColorTranslator.FromHtml("#bfbfbf");
                                lbPromocion.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                            }
                        }

                       DataSet DtsDetalle_Orden = CrearTabla_Detalle_Orden();
                       Cargar_Detalle_Orden_Parte2(DtsDetalle_Orden_consulta, DtsDetalle_Orden);

                        DgvArticulo.DataSource = DtsDetalle_Orden.Tables[0];
                        DgvMontura.DataSource = DtsDetalle_Orden.Tables[1];
                        DgvTotales.DataSource = DtsDetalle_Orden.Tables[2];
                        DgvObservaMon.DataSource = DtsDetalle_Orden.Tables[3];

                        return true;  
                    }
                }

                return false;

            }
            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
                return false;
            }
        }


        public void Cargar_Detalle_Orden_Parte2(DataSet DtsDetalle_Orden_consulta, DataSet DtsDetalle_Orden)
        {
            try
            {
                Decimal SubTotal=0;
                Decimal Descuento=0;
                Decimal IVA=0;
                Decimal IGTF=0;
                Decimal Total=0;
                Decimal Total_Ref = 0;

            //MEDIDAS DE LA Montura 
                int HORIZONTAL = 0;
            int VERTICAL = 0;
            int MAXIMA = 0;
            int DEL = 0;

            string Observa = "";


            if (DtsDetalle_Orden_consulta.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow row in DtsDetalle_Orden_consulta.Tables[0].Rows)
                {
                    //Montura 
                    HORIZONTAL = (row["T_HORIZONTAL"] == DBNull.Value ? (int)0 : Convert.ToInt32(row["T_HORIZONTAL"].ToString()));
                    VERTICAL = (row["T_VERTICAL"] == DBNull.Value ? (int)0 : Convert.ToInt32(row["T_VERTICAL"].ToString()));
                    MAXIMA = (row["T_MAXIMA"] == DBNull.Value ? (int)0 : Convert.ToInt32(row["T_MAXIMA"].ToString()));
                    DEL = (row["T_PUENTE"] == DBNull.Value ? (int)0 : Convert.ToInt32(row["T_PUENTE"].ToString()));

                    //Observa
                    Observa = row["OrSer_Observ"].ToString();

                    //Calculos
                    SubTotal = (row["VtaSubTotal"] == DBNull.Value ? (Decimal)0.00 : Convert.ToDecimal(row["VtaSubTotal"].ToString()));
                    Descuento = (row["VtaDescuento"] == DBNull.Value ? (Decimal)0.00 : Convert.ToDecimal(row["VtaDescuento"].ToString()));
                    IVA = (row["VtaImpuesto"] == DBNull.Value ? (Decimal)0.00 : Convert.ToDecimal(row["VtaImpuesto"].ToString()));
                    IGTF = (row["VtaImpuestoIGTF"] == DBNull.Value ? (Decimal)0.00 : Convert.ToDecimal(row["VtaImpuestoIGTF"].ToString()));
                    Total = (row["VtaTotal"] == DBNull.Value ? (Decimal)0.00 : Convert.ToDecimal(row["VtaTotal"].ToString()));
                    Total_Ref = (row["Orser_Total_Mon"] == DBNull.Value ? (Decimal)0.00 : Convert.ToDecimal(row["Orser_Total_Mon"].ToString()));

                    }

                foreach (DataRow row in DtsDetalle_Orden_consulta.Tables[1].Rows)
                {

                    DtsDetalle_Orden.Tables[0].Rows.Add(row["CodArticulo"].ToString(), row["Cod_Laboratorio"].ToString(), row["DESART"].ToString(), Convert.ToInt32(row["Ordserv_Cant"].ToString()), string.Format("{0:#,0.00}", row["Ordserv_Precio"] == DBNull.Value ? (Decimal)0.00 : Convert.ToDecimal(row["Ordserv_Precio"])), Convert.ToDouble(String.Format(CultureInfo.InvariantCulture, "{0:0.00}", row["Ordserv_PorcDto"].ToString())), string.Format("{0:#,0.00}", row["Ordserv_Neto"] == DBNull.Value ? (Decimal)0.00 : Convert.ToDecimal(row["Ordserv_Neto"])), Convert.ToInt32(row["Ordserv_PorcImp"]), row["Ordser_Ojo"].ToString());

                }

            }

            //-----------------------------Medidas Montura-------------------------------
            
            DtsDetalle_Orden.Tables[1].Rows.Add(HORIZONTAL, VERTICAL, MAXIMA, DEL);

            //-----------------------------Observacion-------------------------------

            DtsDetalle_Orden.Tables[3].Rows.Add(Observa);

                //-----------------------------Calculos-------------------------------

      
            DtsDetalle_Orden.Tables[2].Rows.Add("SubTotal", String.Format("{0:#,0.00}", SubTotal));
            DtsDetalle_Orden.Tables[2].Rows.Add("Descuento", String.Format("{0:#,0.00}", Descuento));
            DtsDetalle_Orden.Tables[2].Rows.Add("IVA", String.Format("{0:#,0.00}", IVA));
            DtsDetalle_Orden.Tables[2].Rows.Add("IGTF", String.Format("{0:#,0.00}", IGTF));
            DtsDetalle_Orden.Tables[2].Rows.Add("Total", String.Format("{0:#,0.00}",Total));
            DtsDetalle_Orden.Tables[2].Rows.Add("Ref", String.Format("{0:#,0.00}", Total_Ref));
            }
            catch (Exception ex)
            {
                string Error = string.Format("Error: {0}", ex.Message);
            }
        }


        //Iva Retenido 
        public bool Verificar_AgenteRetencion(System.Windows.Forms.DataGridView Dt_Abono, string Nacionalidad, string Cedula, bool Iva= false, bool ISLR= false)
        {

            try
            {
                stringBuilder.Clear();
                DataTable Agente = _D_DetalleOrden.AgenteRetencion(Nacionalidad, Cedula);
                bool AgenteIva = false;
                bool AgenteISLR = false;


                foreach (DataGridViewRow row in Dt_Abono.Rows)
                {
                    if(Iva == true)
                    if (row.Cells["CodPago"].Value.ToString() == "013" ) 
                    {
                            stringBuilder.Append("Ya existe un abono con este tipo de pago");
                            return false;
                    }

                    if (ISLR == true)
                        if (row.Cells["CodPago"].Value.ToString() == "014")
                        {
                            stringBuilder.Append("Ya existe un abono con este tipo de pago");
                            return false;
                        }
                }

                if (Agente!= null)
                {
                   if(Agente.Rows.Count>0 )
                   {
                        foreach(DataRow row in Agente.Rows)
                        {
                            if (row["Retiene_IVA"].ToString() != "" && row["Retiene_IVA"].ToString() != " " && row["Retiene_IVA"].ToString() != null && row["Retiene_IVA"].ToString() != "False")
                                AgenteIva = true;
                            else
                                AgenteIva = false;

                            if (row["Retiene_ISLR"].ToString() != "" && row["Retiene_ISLR"].ToString() != " " && row["Retiene_ISLR"].ToString() != null && row["Retiene_ISLR"].ToString() != "False")
                                AgenteISLR = true;
                            else
                                AgenteISLR = false;
                            break;
                        }

                        if(Iva== true)
                        {
                            return AgenteIva;
                        }

                        if (ISLR == true)
                        {
                            return AgenteISLR;
                        }
                    }
               }
                stringBuilder.Append("No se encontraron registros");
                return false;
            }

            catch (Exception ex)
            {

                string Error = string.Format("Error: {0}", ex.Message);
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return false;
            }


        }

        public string RegistarPagoMovil(DataTable PagoMovilRealizados, string CodigoSucursal , string NumeroOrden, string Revision, SqlCommand command)
        {
            try
            {

                int idAbonoPagoMovil;
            Double MontoRecibidoRef = 0;
            Double MontoVueltoRef = 0;
            Double MontoVueltoBs = 0;
            string CteNacionalidad = "";
            string Cedula = "";
            string CodBancoReceptor = "";
            string Telefono = "";
            string Respuesta = "SATISFACTORIO";
            Double Tasa_Abono = 0;
            foreach (DataRow Row in PagoMovilRealizados.Rows)
            {
                    idAbonoPagoMovil = Convert.ToInt16(Row["idAbonoPagoMovil"].ToString());
                    MontoRecibidoRef = Convert.ToDouble(Row["MontoRecibidoRef"].ToString());
                MontoVueltoRef = Convert.ToDouble(Row["MontoVueltoRef"].ToString().Replace(".",","));
                MontoVueltoBs = Convert.ToDouble(Row["MontoVueltoBs"].ToString().Replace(".", ","));
                CteNacionalidad = Row["Nacionalidad"].ToString();
                Cedula = Row["Cedula"].ToString();
                CodBancoReceptor = Row["Banco"].ToString();
                Telefono = Row["PrefijoCelular"].ToString() + Row["Celular"].ToString();


                if (Row["Moneda"].ToString() == "Euros")
                {
                     Tasa_Abono = Convert.ToDouble(TB_TASA_Euro.Tasa);
                }
                else
                {
                    Tasa_Abono = Convert.ToDouble(TB_TASA_Dolar.Tasa);
                }

                Respuesta = _D_DetalleOrden.ReguistarPagoMovil(idAbonoPagoMovil, CodigoSucursal, NumeroOrden, Revision, Tasa_Abono, MontoRecibidoRef, MontoVueltoRef, MontoVueltoBs, CteNacionalidad, Cedula, CodBancoReceptor, Telefono.Trim(), command);
                
                Tasa_Abono = 0;
                MontoRecibidoRef = 0;
                MontoVueltoRef = 0;
                MontoVueltoBs = 0;
                CteNacionalidad = "";
                Cedula = "";
                CodBancoReceptor = "";
                Telefono = "";
            }

            return Respuesta;

            }

            catch (Exception ex)
            {

                string Respuesta = string.Format("Error: {0}", ex.Message);
                return Respuesta;
            }
        }


        public double RecorrerPagoMovil_dt(DataTable PagoMovill)
        {
            stringBuilder.Clear();
            Double PagoMovil = 0;

            foreach (DataRow row in PagoMovill.Rows)
            {

                PagoMovil = PagoMovil + Convert.ToDouble(row["MontoVueltoRef"].ToString() == "" ? "0.00" : row["MontoVueltoRef"].ToString().Replace(".",","));
            }


            return 19-PagoMovil;
        }


        public bool Verificar_Pago_CACHEA(System.Windows.Forms.DataGridView Dt_Abono)
        {

            try
            {
                stringBuilder.Clear();

                foreach (DataGridViewRow row in Dt_Abono.Rows)
                {
                        if (row.Cells["CodPago"].Value.ToString() == "021" && (row.Cells["Banco"].Value.ToString() == "CASHEA"| row.Cells["CodBanco"].Value.ToString() == "110"))
                        {
                            stringBuilder.Append("Ya existe un abono con este tipo de pago");
                            return true;
                        }

                }

                return false;
            }

            catch (Exception ex)
            {

                string Error = string.Format("Error: {0}", ex.Message);
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return true;
            }
        }
 

    }
}