using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace CapaVisual_Login.reportes
{
    public partial class kedwin : Form
    {
        Form1 uno = new Form1();

        public kedwin()
        {
            InitializeComponent();
        }

        private void btn_buscar_Click(object sender, EventArgs e)
        {
            uno.setParametros(txtNroOrden.Text);
            //uno.reportViewer1.LocalReport.RefreshReport();

            uno.algo();

            uno.ShowDialog();
        }
    }
}
