using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data;
using CapaDatos.Configuracion;

namespace Script
{
    public class V_Actualizar
    {
        public static bool ObtieneVersionActualizada()
        {
            try
            {
                D_Configuracion _D_Configuracion = new D_Configuracion();
                //var assembly = Assembly.LoadFrom(dllPath);
                //string versionDLL = assembly.GetName().Version.ToString();

                var assembly = Assembly.GetExecutingAssembly(); // O typeof(DllVersionChecker).Assembly
                string versionDLL = assembly.GetName().Version.ToString();
                //return version?.ToString(); // Ejemplo: "1.2.3.4"


                DataTable dtUltimaVersion = _D_Configuracion.ObtieneUltimaVersionScript();

                int ScriptFaltante = Convert.ToInt32(dtUltimaVersion.Rows[0]["ScriptFaltante"]);
                int UltVersionScript = Convert.ToInt32(dtUltimaVersion.Rows[0]["UltimoScript"]);
                string versionBD = dtUltimaVersion.Rows[0]["Version"].ToString();
                //int UltVersionApp = _D_Configuracion.ScriptVersionEsperada;

                if (versionDLL != versionBD || ScriptFaltante > 0)
                {
                    return false;
                }
                return true; // Ejemplo: "1.2.3.4"
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar la DLL: {ex.Message}");
                return false;
            }
        }
    }
}
