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

namespace CapaLogica.DetalleOrden_Logica
{
    public class L_Facturacion
    {
        //El uso de la clase StringBuilder nos ayudara a devolver los mensajes de las validaciones
        public readonly StringBuilder stringBuilder = new StringBuilder();

        //Instanciamos nuestra clase D_Loguin para poder utilizar sus miembros

        private D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        private D_Inicio _D_Inicio = new D_Inicio();

        public string pru; 

        public class Valor
        {
            public string Value { get; set; }
            public string Index { get; set; }
        }

        public void CargarTasa()
        {
                DateTime FechaActiva = _D_Inicio.DiaActivo();
                _D_Inicio.TasaDiaDolar(FechaActiva.ToString("yyyyMMdd"));
        }

        public void ComboboxTipoMoneda(System.Windows.Forms.ComboBox Moneda)
        {

            var Valores = new List<Valor>();

            Valores.Add(new Valor() { Index = "Dólares", Value = "Dólares" });
            Valores.Add(new Valor() { Index = "Euros", Value = "Euros" });
            Valores.Add(new Valor() { Index = "Bolivares", Value = "Bolivares" });

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

        public void LLenarComboboxPagos(System.Windows.Forms.ComboBox Pago)
        {
            Pago.DataSource = _D_DetalleOrden.Pagos();
            Pago.DisplayMember = "Indexx";
            pru = "Index";
            Pago.ValueMember = "Value";

        }


        public void LLenarComboboxBancos(System.Windows.Forms.ComboBox Pago, bool Extranjera)
        {
            Pago.DataSource = _D_DetalleOrden.Bancos(Extranjera);
            Pago.DisplayMember = "Indexx";
            Pago.ValueMember = "Value";
        }

        public void DatosOrden(string Orden)
        {
            try
            {

                stringBuilder.Clear();
                _D_DetalleOrden.Datos_de_la_Orden(Orden);

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

        public Double CalculoIgtf(System.Windows.Forms.TextBox NunOrden, System.Windows.Forms.TextBox Dolares, System.Windows.Forms.DataGridView Dt_Abono)
        {
            Double IgtfBs = 0;
            Double TotalAboTranferenciaDolar = 0;
            Double TopeMaxIgtf = 0;

            if (NunOrden.Text == TB_CAORDSER.NumOrdserv)
            {
                DataTable dt = _D_DetalleOrden.BucarTotalAbonosRealizados(NunOrden.Text);
                TotalAboTranferenciaDolar = Math.Round(Convert.ToDouble(dt.Rows[0]["TotalPagosDivisa"].ToString()), 2);
                TotalAboTranferenciaDolar = TotalAboTranferenciaDolar + Convert.ToDouble(TotalIgtf(Dt_Abono));
                IgtfBs = Math.Round(Convert.ToDouble((Convert.ToDouble(Dolares.Text) * 0.03) * TB_TASA_Dolar.Tasa), 2);
               
                //////// ****** Esta en fase de prueba si se utiliza el precio total de la orden en dolares o el saldo de la orden en dolares *****
                //TopeMaxIgtf = Math.Round(Convert.ToDouble((TB_CAORDSER.Orser_Total_Mon * 0.03) * TB_TASA_Dolar.Tasa), 2);
                //TopeMaxIgtf = Math.Round(TopeMaxIgtf - TotalAboTranferenciaDolar, 2);

                TopeMaxIgtf = Math.Round(Convert.ToDouble(TB_CAORDSER.OrSer_Saldo * 0.03) , 2);
                TopeMaxIgtf = Math.Round(TopeMaxIgtf - TotalAboTranferenciaDolar, 2);

                if (TopeMaxIgtf < IgtfBs)
                {
                    IgtfBs = TopeMaxIgtf;
                }


            }

            return IgtfBs;
        }

        public void ConvertirDolaresBolivares(System.Windows.Forms.TextBox Dolar, System.Windows.Forms.TextBox Bolivares)
        {


            if (Convert.ToDouble(Dolar.Text) != 0.00 && Dolar.Text != "")
            {

                Bolivares.Text = Convert.ToString(Math.Round(((Convert.ToDouble(Dolar.Text) * Convert.ToDouble(TB_TASA_Dolar.Tasa))), 2));

            }

            else
                Bolivares.Text = "0.00";


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
                    Bolivares = Bolivares + Convert.ToDouble(row.Cells["Bs"].Value.ToString());
                }
            }

            total = Convert.ToString(Math.Round(Bolivares , 2));

            return total;
        }

        public string TotalIgtf(System.Windows.Forms.DataGridView Dt_Abono)
        {
            stringBuilder.Clear();
            string total = "";
            Double Igtf = 0;

            foreach (DataGridViewRow row in Dt_Abono.Rows)
            {

                Igtf = Igtf + Convert.ToDouble(row.Cells["Igtf"].Value.ToString() == "" ? "0.00": row.Cells["Igtf"].Value.ToString());
            }

            total = Convert.ToString(Math.Round(Igtf, 2));

            return total;
        }

        public string CalcularNuevoTotalOrden(System.Windows.Forms.DataGridView Dt_Abono)
        {
            stringBuilder.Clear();
            string total = "";

            Double bs = ((double)(TB_CAORDSER.OrSer_Saldo_Mon * TB_TASA_Dolar.Tasa));
            Double igtf = Convert.ToDouble(TotalIgtf(Dt_Abono));
            Double Abono = Convert.ToDouble(TotalizarAbono(Dt_Abono));
            total = Convert.ToString(Math.Round((bs + igtf)- Abono, 2));
            return total;
        }

        public void GuardarAbonoGrid(DataTable Dt_Abonos, string MedioPago, string Moneda, string Banco, string Bolivares, string NunTranferencia, string Fecha, string Vuelto = "", string Ref = "", string Igtf = "", string CVC = "", string Vence = "", string TipoTrajeta = "")
        {
            stringBuilder.Clear();
            Dt_Abonos.Rows.Add(MedioPago,Moneda,  Ref,  Banco,  Igtf,  Bolivares,  NunTranferencia,  Fecha, Vuelto, CVC, Vence, TipoTrajeta);



        }

        public void CrearTabla(DataTable Dt_Abonos)
        {

            //************crear Tabla*****************
            DataColumn column1 = new DataColumn("TipoPago");
            DataColumn column2 = new DataColumn("Moneda");
            DataColumn column3 = new DataColumn("Ref");
            DataColumn column4 = new DataColumn("Banco");
            DataColumn column5 = new DataColumn("Igtf");
            DataColumn column6 = new DataColumn("Bs");
            DataColumn column7 = new DataColumn("N°Tranferencia");
            DataColumn column8 = new DataColumn("Fecha");
            DataColumn column9 = new DataColumn("Vuelto");
            DataColumn column10 = new DataColumn("CVC");
            DataColumn column11 = new DataColumn("Vence");
            DataColumn column12 = new DataColumn("TipoTrajeta");

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
            //*************************************************
        }

        public bool BuscarNotas(string Nota, System.Windows.Forms.DataGridView DgvNotas)
        {
            try
            {

                stringBuilder.Clear();
                DataTable dt = _D_DetalleOrden.BuscarNota(Nota);
                if (dt.Rows.Count>0)
                {
                    DgvNotas.DataSource = dt;
                }

                else
                {
                    stringBuilder.Append(Environment.NewLine + "Este Nro no tiene notas de credito asociadas ");
                }

                return stringBuilder.Length == 0;

            }
            catch (Exception ex)
            {
                stringBuilder.Append(Environment.NewLine + string.Format("Error: {0}", ex.Message));
                return stringBuilder.Length == 0;
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
                resuesta= ex.Message;
                return resuesta;
            }

        }

        public void LLenadoParametros(System.Windows.Forms.DataGridView Dt_Abono, System.Windows.Forms.ComboBox CbxMetodo)
        {

            string ID_Abono;
            string Cod_Sucursal;
            string NumOrdserv;
            string Revision;
            string Tipo_Pago;
            string Cod_Banco;
            string Abo_CTATARJETA;
            string Abo_CVCNROCHEQUE;
            DateTime vas;
            string Abo_Fecha;
            int Abo_Monto;
            string Abo_Tipo;
            string Tipo_Pto;
            string CodPunto;
            string Anulado;
            string Fec_Crea;
            string Fec_Mod;
            string USER_Crea;
            string USER_Mod;
            string Fecha;
            string Fecha_Abono;
            string Cod_BancoRecep;
            int Tasa_Abono;
            int Abo_Monto_Divisa;
            int Abo_Monto_SinIGTF;
            int Abo_IGTF;

            foreach (DataGridViewRow Row in Dt_Abono.Rows)
            {
                ID_Abono = "1";
                Cod_Sucursal = TB_USUARIO.COD_SUCURSAL;
                NumOrdserv = TB_CAORDSER.NumOrdserv;
                Revision = "0";
                Tipo_Pago = Convert.ToString(Row.Cells["TipoPago"].Value);
                Cod_Banco = Convert.ToString(Row.Cells["Banco"].Value);
                Abo_CTATARJETA = Convert.ToString(Row.Cells["CVC"].Value);
                Abo_CVCNROCHEQUE = "";


                vas = Convert.ToDateTime(Row.Cells["Fecha"].Value);
                Abo_Fecha = vas.ToString("yyyyMMdd");
               
                Abo_Monto = Convert.ToInt32(Row.Cells["Bs"].Value);
                Abo_Tipo = CbxMetodo.Text;
                if (Abo_Tipo == "Debito") {
                    Tipo_Pto = "TD";
                    CodPunto = "003";

                        }
                if (Abo_Tipo == "Credito")
                {
                    Tipo_Pto = "TC";
                    CodPunto = "003"; 
                }
                else
                {
                    Tipo_Pto = "XX";
                    CodPunto = "000";
                }

                Anulado = "False";
                Fec_Crea = Abo_Fecha;
                Fec_Mod = Abo_Fecha;
                USER_Crea = TB_USUARIO.COD_USR;
                USER_Mod = "";
                Fecha = Abo_Fecha;
                Fecha_Abono = Abo_Fecha;
                Cod_BancoRecep = Cod_Banco = Convert.ToString(Row.Cells["Banco"].Value);
                Tasa_Abono = Convert.ToInt32(TB_TASA_Dolar.Tasa);
                if (Row.Cells["Ref"].Value == "")
                {
                    Abo_Monto_Divisa = 0;
                }
                else
                {
                    Abo_Monto_Divisa = Convert.ToInt32(Row.Cells["Ref"].Value);
                }
                Abo_Monto_SinIGTF = Convert.ToInt32(Row.Cells["Bs"].Value);
                if (Row.Cells["Igtf"].Value == "")
                {
                    Abo_IGTF = 0;
                }
                else
                {
                    Abo_IGTF = Convert.ToInt32(Row.Cells["Igtf"].Value);
                }

                _D_DetalleOrden.GetAbono(ID_Abono, Cod_Sucursal, NumOrdserv, Revision, Tipo_Pago, Cod_Banco, Abo_CTATARJETA, Abo_CVCNROCHEQUE, Abo_Fecha, Abo_Monto, Abo_Tipo, Tipo_Pto, CodPunto, Anulado, Fec_Crea, Fec_Mod, USER_Crea, USER_Mod, Fecha, Fecha_Abono, Cod_BancoRecep, Tasa_Abono, Abo_Monto_Divisa, Abo_Monto_SinIGTF, Abo_IGTF);


            }


        }




    }
}