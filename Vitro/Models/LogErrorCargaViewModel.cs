using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vitro.Models
{
    public class LogErrorCargaViewModel
    {
        public int ID { get; set; }
        public DateTime FechaProceso { get; set; }
        public string Usuario { get; set; }
        public string Sap { get; set; }
        public int Fila { get; set; }
        public string Columna { get; set; }
        public string ValorIncorrecto { get; set; }
        public string DescripcionError { get; set; }

        public DataTable LogErrores { get; set; }
    }
}
