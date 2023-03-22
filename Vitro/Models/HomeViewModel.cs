using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vitro.Models
{
    public class HomeViewModel
    {
        public IEnumerable<VitroSql.Producto> ProductosNuevos { get; set; }
        public IEnumerable<VitroSql.ProductoPromocion> ProductosPromocion { get; set; }
    }
}