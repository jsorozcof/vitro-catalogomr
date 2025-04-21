using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vitro.Models
{
    public class DetailsProductoViewModel
    {
        //public VitroSql.Producto Producto { get; set; }
        public VitroSql.TbProduct Product { get; set; }
        public IEnumerable<VitroSql.ProductoImagen> ProductoImagen { get; set; }
        public IEnumerable<VitroSql.ImagenCargue> ImagenCargue { get; set; }
        public IEnumerable<ProductImageDto> ProductImages { get; set; }
        //public IEnumerable<VitroSql.ProductImages> ProductImages { get; set; }

        //public IEnumerable<VitroSql.Producto> Homologos { get; set; }
        public IEnumerable<VitroSql.TbProduct> Homologos { get; set; }
    }

}