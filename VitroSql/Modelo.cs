using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VitroSql
{
    [Table("Modelo")]
    public class Modelo
    {
        [Key]
        public string ModeloId { get; set; }
        public string Nombre { get; set; }
        public string MarcaId { get; set; }
        public bool Activo { get; set; }

        public Marca Marca { get; set; }
    }
}
