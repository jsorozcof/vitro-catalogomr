using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VitroSql
{
    [Table("Color")]
    public class Color
    {
        [Key]
        public string ColorId { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public bool Activo { get; set; }
    }
}
