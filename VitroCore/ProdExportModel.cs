using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitroCore
{
    public class ProdExportModel
    {
        public string SAP { get; set; }
        public string NAGS { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Descripcion { get; set; }
        public string Mercado { get; set; }
        public string Color { get; set; }
        public string TipoVidrio { get; set; }
        public string TipoParte { get; set; }
        public string Ancho { get; set; }
        public string Alto { get; set; }
        public bool Boton { get; set; }
        public bool Red { get; set; }
        public bool Serigrafia { get; set; }
        public bool SensorLluvia { get; set; }
        public bool Moldura { get; set; }
        public bool Holder { get; set; }
        public bool SensorCondensacion { get; set; }
        public bool Homologo { get; set; }
        public string Perforacion { get; set; }
        public string Clasificacion { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public string Procedencia { get; set; }
        public byte[] Imagen { get; set; }
    }
}
