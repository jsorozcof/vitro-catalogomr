using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Vitro.Models
{
    public class MassiveProductImagesViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Extenxion { get; set; }

        [Required]
        public byte[] Contenido { get; set; }

        
        [Required]
        [Display(Name = "ImagenId")]
        public string ImagenId { get; set; }

        [Required]
        [Display(Name = "ProductoId")]
        public string ProductoId { get; set; }


        public IEnumerable<VitroSql.Producto> Productos { get; set; }
        public IEnumerable<VitroSql.ProductoImagen> ProductoImagenes { get; set; }
    }
}