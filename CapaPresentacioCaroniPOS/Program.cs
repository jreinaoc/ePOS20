using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using CapaDatos.DetalleOrden_Datos;


namespace CapaVisual_Login
{
    public class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        /// 
        public static D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();

        static Mutex mutex = null;

        [STAThread]
        static void Main()
        {
            const string appName = "APP_EPOS_2_0";
            bool createdNew;

            mutex = new Mutex(true, appName, out createdNew);
            
            string ValidaInstancia = _D_DetalleOrden.TB_PARAMETRO("ValidaInstancia");

            if (ValidaInstancia == "1")
            {
                if (!createdNew)
                {
                    MessageBox.Show("La aplicación ya está en ejecución.", "Instancia duplicada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }   
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmLoguin());
        }
    }
}
