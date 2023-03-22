using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VitroSql
{
    [Table("TipoParte")]
    public class TipoParte
    {
        public string TipoParteId { get; set; }
        public string Nombre { get; set; }
        public string ClasificacionId { get; set; }
        public bool Activo { get; set; }

        public Clasificacion Clasificacion { get; set; }
    }
}
