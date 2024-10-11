using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica.Colores_Logica
{
    public class L_Colores
    {
        public static bool Oscuro { get; set; }
        public static bool  Claro  { get; set; }

        public void ColorOsc()
        {

            Oscuro = true;
            Claro = false;
        }
        public void ColorClar()
        {

            Oscuro = false;
            Claro = true;
        }

        public bool SelectorColor()
        {

            if (Oscuro == true)
            {
                return true; 

            }
            else
            {

                return false;
            }

        }

    }
}
