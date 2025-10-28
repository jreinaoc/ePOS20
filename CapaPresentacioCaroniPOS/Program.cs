using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using CapaDatos.DetalleOrden_Datos;
using Script;



namespace CapaVisual_Login
{
    public class Program
    {
        public static D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        public static V_Actualizar V_Actualizar = new V_Actualizar();

        static Mutex mutex = null;

        [STAThread]
        static void Main()
        {
            const string appName = "APP_EPOS_2_0";
            bool createdNew;

            mutex = new Mutex(true, appName, out createdNew);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false); // ✅ Llamar antes de crear formularios

            FrmMensajes _FrmMensajes = new FrmMensajes(); // ✅ Crear después de SetCompatibleTextRenderingDefault
            string ValidaInstancia = _D_DetalleOrden.TB_PARAMETRO("ValidaInstancia");

            if (ValidaInstancia == "1")
            {
                if (!createdNew)
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("La aplicación ya está en ejecución");
                    _FrmMensajes.ShowDialog();
                    return;
                }
            }

            if (!V_Actualizar.ObtieneVersionActualizada())
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje("Script desactualizado");
                _FrmMensajes.ShowDialog();
                return;
            }

            Application.Run(new FrmLoguin());
        }
    }
}
