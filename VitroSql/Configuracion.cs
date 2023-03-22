using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VitroSql
{
    [Table("Configuracion")]
    public class Configuracion
    {
        [Key]
        public string ConfiguracionId { get; set; }
        public int DiasVigenciaNuevosProductos { get; set; }
    }
}
