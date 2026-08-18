using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using CapaDatos.DetalleOrden_Datos;
using Script;
using System.Data.SqlClient;


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
            // 1. Configurar estilos de Windows Forms SIEMPRE al inicio
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 2. Declarar el formulario FUERA del try para que sea accesible en el catch
            FrmMensajes _FrmMensajes = new FrmMensajes();

            try
            {
                const string appName = "APP_EPOS_2_0";
                bool createdNew;

                mutex = new Mutex(true, appName, out createdNew);

                // Intento de conexión a Base de Datos
                string ValidaInstancia = _D_DetalleOrden.TB_PARAMETRO("ValidaInstancia");

                if (ValidaInstancia == "1" && !createdNew)
                {
                    _FrmMensajes.co = 2;
                    _FrmMensajes.avisomensaje("La aplicación ya está en ejecución");
                    _FrmMensajes.ShowDialog();
                    return;
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
            catch (SqlException ex)
            {
                // Se captura la falla de conexión a BD y se envía el mensaje visual
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje($"No se pudo conectar a la base de datos:");
                _FrmMensajes.ShowDialog();
            }
            catch (Exception ex)
            {
                _FrmMensajes.co = 2;
                _FrmMensajes.avisomensaje($"Error de inicio:\n{ex.Message}");
                _FrmMensajes.ShowDialog();
            }
        }
    }
}
