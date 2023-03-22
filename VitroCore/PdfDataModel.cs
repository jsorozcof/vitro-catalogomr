using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace VitroCore
{
    public class PdfDataModel
    {
        public DataTable TablaEncabezado { get; set; }
        public DataTable TablaDetalle { get; set; }
        public byte[] ImageFileName { get; set; }
    }
}
