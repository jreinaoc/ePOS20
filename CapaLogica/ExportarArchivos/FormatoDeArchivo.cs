using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ExportarArchivos
{
    public class FormatoDeArchivo
    {
        public string Type { get; set; }

        public byte[] Content { get; set; }

        public string Extension { get; set; }
    }
}
