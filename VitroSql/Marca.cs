using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VitroSql
{
    [Table("Marca")]
    public class Marca
    {
        [Key]
        public string MarcaId { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }

        public string PaisId { get; set; }
        public Pais Pais { get; set; }
    }
}
