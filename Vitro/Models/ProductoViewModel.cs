using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Vitro.Models
{
    public class ProductoViewModel
    {
        public int TotalProductos { get; set; }
        public int TotalproductIMG { get; set; }
        public string ProductoId { get; set; }
        public string Parametro { get; set; }
        public string Busqueda { get; set; }
        public string Year { get; set; }
        public string Mode { get; set; }
        public string Pais { get; set; }
        public string NombreImagen { get; set; }
        
        [Required]
        [Display(Name = "Código SAP")]
        public string SAP { get; set; }
        [Required]
        [Display(Name = "Código NAGS")]
        public string NAGS { get; set; }
        [Required]
        public string Marca { get; set; }
        [Required]
        public string Modelo { get; set; }
        [Display(Name = "Año Inicial")]
        [RegularExpression(pattern: "^[0-9]{4}$", ErrorMessage = "Fecha de inicio no contiene un formato correcto")]
        public int StartYear { get; set; }
        [Display(Name = "Año Final")]
        [RegularExpression(pattern: "^[0-9]{4}$", ErrorMessage = "Fecha final no contiene un formato correcto")]
        public int EndYear { get; set; }
        public string Descripcion { get; set; }
        [Display(Name = "Tipo de Parte")]
        [Required]
        public string TipoParte { get; set; }
        public double Perforacion { get; set; }
        public double Ancho { get; set; }
        public double Alto { get; set; }
        public bool Boton { get; set; }
        public bool Red { get; set; }
        public bool Serigrafia { get; set; }
        public bool SensorLluvia { get; set; }
        public bool Moldura { get; set; }
        public bool Holder { get; set; }
        public bool SensorCondensacion { get; set; }
        public bool Homologo { get; set; }
        public bool Antena { get; set; }
        public bool SubEnsamble { get; set; }
        [Required]
        public string Clasificacion { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public string TipoVidrio { get; set; }
        [Required]
        public string Procedencia { get; set; }
        [Required]
        public string Mercado { get; set; }
        public HttpPostedFileBase[] Files { get; set; }
        [Display(Name = "Activo")]
        public bool Activo { get; set; }

        public IEnumerable<VitroSql.Pais> PaisList { get; set; }
        public IEnumerable<VitroSql.Modelo> ModeloList { get; set; }
        public IEnumerable<VitroSql.Marca> MarcaList { get; set; }
        public IEnumerable<VitroSql.Mercado> MercadoList { get; set; }
        public IEnumerable<VitroSql.Clasificacion> Clasificaciones { get; set; }
        public IEnumerable<VitroSql.Color> ColorList { get; set; }
        public IEnumerable<VitroSql.TipoVidrio> TipoVidroList { get; set; }
        public IEnumerable<VitroSql.TipoParte> TipoParteList { get; set; }
        public IEnumerable<VitroSql.Procedencia> ProcedenciaList { get; set; }
        public IEnumerable<VitroSql.Producto> Productos { get; set; }
        public IEnumerable<VitroSql.Producto> ProdcutosCount { get; set; }
        public IEnumerable<VitroSql.Marca> Marcas { get; set; }
        public IEnumerable<VitroSql.ProductoImagen> ProductoImagenes { get; set; }
    }
}