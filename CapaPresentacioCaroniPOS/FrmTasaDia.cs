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
    }
}
