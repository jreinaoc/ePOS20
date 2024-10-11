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
    public partial class FrmMensajes : Form
    {
       public Button Btnaceptar = new Button();

        public Button Btacept = new Button(); 

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
            if (co == 1)
            {
                Size = new System.Drawing.Size(350, 224);
                this.Btacept.BackColor = Color.Aquamarine;
                this.Btacept.Text = "Aceptar";
                this.Btacept.Font = new Font("Century Gothic", 12, FontStyle.Bold, GraphicsUnit.Point);
                this.Btacept.Location = new System.Drawing.Point(256, 190);
                this.Btacept.Size = new System.Drawing.Size(82, 27);
                this.Btacept.FlatStyle = FlatStyle.Flat;
                this.Btacept.FlatAppearance.BorderSize = 0;
                LblMensaje.Location = new System.Drawing.Point(78, 86);
                LblMensaje.TextAlign = ContentAlignment.MiddleCenter;
                LogoError.Visible = false;
                this.Controls.Add(Btacept);
                Btacept.Click += Btacept_Click;
                Btacept.Tag = true;

            }

            if (co == 2)
            {
                Size = new System.Drawing.Size(350, 224);
                this.Btacept.BackColor = Color.Aquamarine;
                this.Btacept.Text = "Aceptar";
                this.Btacept.Font = new Font("Century Gothic", 12, FontStyle.Bold, GraphicsUnit.Point);
                this.Btacept.Location = new System.Drawing.Point(256, 190);
                this.Btacept.Size = new System.Drawing.Size(82, 27);
                this.Btacept.FlatStyle = FlatStyle.Flat;
                this.Btacept.FlatAppearance.BorderSize = 0;
                this.Controls.Add(Btacept);
                Btacept.Click += Btacept_Click;
            }

            if (co == 3)
            {
                Size = new System.Drawing.Size(350, 224);
                this.BtnSi.BackColor = Color.Aquamarine;
                this.BtnSi.Text = "SI";
                this.BtnSi.Font = new Font("Century Gothic", 12, FontStyle.Bold, GraphicsUnit.Point);
                this.BtnSi.Location = new System.Drawing.Point(256, 190);
                this.BtnSi.Size = new System.Drawing.Size(82, 27);
                this.BtnSi.FlatStyle = FlatStyle.Flat;
                this.BtnSi.FlatAppearance.BorderSize = 0;
                this.BtnNo.BackColor = Color.IndianRed;
                this.BtnNo.Text = "NO";
                this.BtnNo.Font = new Font("Century Gothic", 12, FontStyle.Bold, GraphicsUnit.Point);
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



        public void Btacept_Click(object sender, EventArgs e)
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

        private void LblMensaje_Click(object sender, EventArgs e)
        {

        }
    }
}

