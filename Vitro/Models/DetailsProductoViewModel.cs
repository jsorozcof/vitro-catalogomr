using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vitro.Models
{
    public class DetailsProductoViewModel
    {
        public VitroSql.Producto Producto { get; set; }
        public IEnumerable<VitroSql.ProductoImagen> ProductoImagen { get; set; }
        public IEnumerable<VitroSql.ImagenCargue> ImagenCargue { get; set; }

        public IEnumerable<VitroSql.MassiveProductImages> MassiveProductImage { get; set; }

        public IEnumerable<VitroSql.Producto> Homologos { get; set; }
    }

}