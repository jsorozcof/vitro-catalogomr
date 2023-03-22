using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VitroSql
{
    [Table("Sugerencia")]
    public class Sugerencia
    {
        public string SugerenciaId { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string TipoParte { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
