using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Vitro.Models
{
    public class SearchViewModel
    {
        public string Parametro { get; set; }
        public string Busqueda { get; set; }
        public string Year { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Mode { get; set; }

        public SugerenciaViewModel SugerenciaViewModel { get; set; }

        public IEnumerable<VitroSql.Producto> Productos { get; set; }
        public IEnumerable<VitroSql.TbProduct> TbProduct { get; set; }
        public IEnumerable<VitroSql.Producto> Homologos { get; set; }
        public IEnumerable<VitroSql.TbProduct> TbHomologos { get; set; }
        public IEnumerable<VitroSql.Marca> Marcas { get; set; }
        public IEnumerable<VitroSql.ProductoImagen> ProductoImagenes { get; set; }
        public IEnumerable<VitroSql.ProductImages> ProductImages { get; set; }
    }
}