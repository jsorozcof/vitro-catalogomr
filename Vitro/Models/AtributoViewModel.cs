using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vitro.Models
{
    public class AtributoViewModel
    {
        public string Atributo { get; set; }
        public string Referencia { get; set; }
        public string Definicion { get; set; }

        public IEnumerable<VitroSql.Pais> Paises { get; set; }
        public IEnumerable<VitroSql.Marca> Marcas { get; set; }
        public IEnumerable<VitroSql.Modelo> Modelos { get; set; }
        public IEnumerable<VitroSql.Clasificacion> Clasificaciones { get; set; }
        public IEnumerable<VitroSql.TipoParte> TipoPartes { get; set; }
        public IEnumerable<VitroSql.TipoVidrio> TipoVidrios { get; set; }
        public IEnumerable<VitroSql.Color> Colores { get; set; }
        public IEnumerable<VitroSql.Mercado> Mercados { get; set; }
        public IEnumerable<VitroSql.Procedencia> Procedencias { get; set; }
    }
}
