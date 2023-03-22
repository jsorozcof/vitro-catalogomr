using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VitroSql
{
    [Table("Producto")]
    public class Producto
    {
        [Key]
        public string ProductoId { get; set; }
        public string SAP { get; set; }
        public string NAGS { get; set; }
        public string ModeloId { get; set; }
        public string Descripcion { get; set; }
        public string MercadoId { get; set; }
        public string ColorId { get; set; }
        public string TipoVidrioId { get; set; }
        public string TipoParteId { get; set; }
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
        public double Perforacion { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string ProcedenciaId { get; set; }
        public bool Activo { get; set; }
        public bool Antena { get; set; }
        public bool SubEnsamble { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public TipoVidrio TipoVidrio { get; set; }
        public Modelo Modelo { get; set; }
        public Mercado Mercado { get; set; }
        public TipoParte TipoParte { get; set; }
        public Color Color { get; set; }
        public Procedencia Procedencia { get; set; }
    }
}
