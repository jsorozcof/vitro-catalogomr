using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VitroSql
{
    [Table("Clasificacion")]
    public class Clasificacion
    {
        [Key]
        public string ClasificacionId { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
    }
}
