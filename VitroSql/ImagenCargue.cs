using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VitroSql
{
    [Table("ImagenCargue")]
    public class ImagenCargue
    {
        [Key]
        public string ImagenId { get; set; }
        public int Indice { get; set; }
        public string Nombre { get; set; }
        public string CargueRef { get; set; }
        public long ImageSize { get; set; }
        public string ImageType { get; set; }
        public bool Procesado { get; set; }


    }
}
