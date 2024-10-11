using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVisual_Externa
{
    public partial class FrmMensajes : Form
    {


        public Button BtnAceptar = new Button();

        public Button BtnSi = new Button();

        public Button BtnNo = new Button();



        public string avisomensaje(string mensaje)
        {
            LblMensaje.Text = mensaje;
            return mensaje;
        }

        public int co;


        public FrmMensajes()
        {
            InitializeComponent();
        }

        private void FrmMensajes_Load(object sender, EventArgs e)
        {
            if (co == 1) {
                Size = new System.Drawing.Size(350, 224);
                this.BtnAceptar.BackColor = Color.Aquamarine;
                this.BtnAceptar.Text = "Aceptar";
                this.BtnAceptar.Font = new Font("Century Gothic", 12, FontStyle.Bold,GraphicsUnit.Point);
                this.BtnAceptar.Location = new System.Drawing.Point(256, 190);
                this.BtnAceptar.Size = new System.Drawing.Size(82, 27);
                this.BtnAceptar.FlatStyle = FlatStyle.Flat;
                this.BtnAceptar.FlatAppearance.BorderSize = 0;
                LblMensaje.Location = new System.Drawing.Point(78, 86);
                LblMensaje.TextAlign = ContentAlignment.MiddleCenter;
                LogoError.Visible = false;
                this.Controls.Add(BtnAceptar);
                BtnAceptar.Click += BtnAceptar_Click;
                BtnAceptar.Tag = true;
                 
            }

            if (co == 2)
            {
                Size = new System.Drawing.Size(350, 224);
                this.BtnAceptar.BackColor = Color.Aquamarine;
                this.BtnAceptar.Text = "Aceptar";
                this.BtnAceptar.Font = new Font("Century Gothic", 12, FontStyle.Bold,GraphicsUnit.Point);
                this.BtnAceptar.Location = new System.Drawing.Point(256, 190);
                this.BtnAceptar.Size = new System.Drawing.Size(82, 27);
                this.BtnAceptar.FlatStyle = FlatStyle.Flat;
                this.BtnAceptar.FlatAppearance.BorderSize = 0;
                this.Controls.Add(BtnAceptar);
                BtnAceptar.Click += BtnAceptar_Click;
            }

            if (co == 3)
            {
                Size = new System.Drawing.Size(350, 224);
                this.BtnSi.BackColor = Color.Aquamarine;
                this.BtnSi.Text = "SI";
                this.BtnSi.Font = new Font("Century Gothic", 12, FontStyle.Bold,GraphicsUnit.Point);
                this.BtnSi.Location = new System.Drawing.Point(256, 190);
                this.BtnSi.Size = new System.Drawing.Size(82, 27);
                this.BtnSi.FlatStyle = FlatStyle.Flat;
                this.BtnSi.FlatAppearance.BorderSize = 0;
                this.BtnNo.BackColor = Color.IndianRed;
                this.BtnNo.Text = "NO";
                this.BtnNo.Font = new Font("Century Gothic", 12, FontStyle.Bold,GraphicsUnit.Point);
                this.BtnNo.Location = new System.Drawing.Point(140, 190);
                this.BtnNo.Size = new System.Drawing.Size(82, 27);
                this.BtnNo.FlatStyle = FlatStyle.Flat;
                this.BtnNo.FlatAppearance.BorderSize = 0;

                LblMensaje.Location = new System.Drawing.Point(78, 86);
                LblMensaje.TextAlign = ContentAlignment.MiddleCenter;
                LogoError.Visible = false;
                this.Controls.Add(BtnSi);
                this.Controls.Add(BtnNo);
                BtnSi.Click += BtnSi_Click;
                BtnNo.Click += BtnNo_Click;
            }

        }

        

        public void BtnAceptar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
           
        }

        private void BtnSi_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnNo_Click(object sender, EventArgs e)
        {

            this.Close();
        }

    }
}
