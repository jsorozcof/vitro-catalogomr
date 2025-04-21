using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VitroSql
{
    [Table("ProductoImagen")]
    public class ProductoImagen
    {
        [Key]
        public string ProductoImagenId { get; set; }
        public string ProductoId { get; set; }
        public string ImagenId { get; set; }

        public Imagen Imagen { get; set; }
        public Producto Producto { get; set; }
        public TbProduct Product { get; set; }
        public string ProductId { get; set; }
    }
}
