using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace VitroSql
{
    [Table("ProductoPromocion")]
    public class ProductoPromocion
    {
        [Key]
        public string PromocionId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public int DiasVigencia { get; set; }
        public string ProductoId { get; set; }
        public double Precio { get; set; }
        public int Stock { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public Producto Producto { get; set; }
    }
}
