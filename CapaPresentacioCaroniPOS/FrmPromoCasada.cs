using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaDatos.ListaOrdenes_Datos;
using CapaDatos.Inicio_Datos;



namespace CapaVisual_Login
{
    public partial class FrmPromoCasada : Form
    {

        private D_ListaOrdenes _D_ListaOrdenes = new D_ListaOrdenes();
        D_Inicio _D_Inicio = new D_Inicio();
        public FrmPromoCasada()
        {
            InitializeComponent();
        }

        private void FrmPromoCasada_Load(object sender, EventArgs e)
        {
            DateTime currentDate = _D_Inicio.DiaActivo();
            string formattedDate = currentDate.ToString("yyyyMMdd");

            DataSet Ordenesrango = _D_ListaOrdenes.CargarOrdPorRango(formattedDate, formattedDate, "002", "", 1, 12);

            // DataSet Ordenesrango = _D_ListaOrdenes.CargarOrdPorRango(DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("dd/MM/yyyy"), "004", "", 1, 12);
            if (Ordenesrango.Tables[0].Rows.Count > 0)
            {
                Dgv_ListOsCasadas.DataSource  = Ordenesrango;
            }
        }
    }
}
