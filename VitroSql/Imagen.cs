using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VitroSql
{
    [Table("Imagen")]
    public class Imagen
    {
        [Key]
        public string ImagenId { get; set; }
        public int Indice { get; set; }
        public string Nombre { get; set; }
        public string ImageType { get; set; }
        public long ImageSize { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
