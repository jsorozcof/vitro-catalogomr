using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VitroSql
{
    [Table("Procedencia")]
    public class Procedencia
    {
        [Key]
        public string ProcedenciaId { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
    }
}
