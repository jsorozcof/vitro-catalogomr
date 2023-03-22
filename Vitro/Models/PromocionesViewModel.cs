using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Vitro.Models
{
    public class PromocionesViewModel
    {
        public string PromocionId { get; set; }
        public string ProductoId { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "dd/MM/yyyy")]
        public DateTime FechaInicio { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "dd/MM/yyyy")]
        public DateTime FechaFinal { get; set; }
        public string Descripcion { get; set; }
        public string SAP { get; set; }
        public string NAGS { get; set; }
        public double Precio { get; set; }
        public int Stock { get; set; }


        public string[] Productos { get; set; }

        public List<VitroSql.Producto> ProductosList { get; set; }
        public List<VitroSql.ProductoPromocion> ProductosPromocionesList { get; set; }
    }
}