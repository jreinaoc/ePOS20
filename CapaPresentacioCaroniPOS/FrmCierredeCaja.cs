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
    public partial class FrmCierredeCaja : Form
    {
        public FrmCierredeCaja()
        {
            InitializeComponent();
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            tcCierreCaja.SelectedIndex = 1;
            lblPaso.Text = "Paso 2";
        }

        private void btnAtrasPaso1_Click(object sender, EventArgs e)
        {
            tcCierreCaja.SelectedIndex = 0;
            lblPaso.Text = "Paso 1";

        }
    }
}
