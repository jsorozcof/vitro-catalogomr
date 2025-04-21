using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitroSql
{
    [Table("ProductImages")]
    public class ProductImages
    {
        public string ProductId { get; set; } = string.Empty;
        public string Sap { get; set; }
        [Key]
        public Guid ImagenId { get; set; } 
        public int? Posicion { get; set; } = null;
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public byte[] Contenido { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

    }
}
