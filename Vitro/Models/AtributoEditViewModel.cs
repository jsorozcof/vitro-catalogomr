using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vitro.Models
{
    public class AtributoEditViewModel
    {
        public string EntityId { get; set; }
        public string Atributo { get; set; }
        public string ModeloId { get; set; }
        public string PaisId { get; set; }
        public string MarcaId { get; set; }
        public string Definicion { get; set; }
        public string Codigo { get; set; }
        public string ClasificacionId { get; set; }
        public bool Activo { get; set; }

        public IEnumerable<VitroSql.Pais> Paises { get; set; }
        public IEnumerable<VitroSql.Marca> Marcas { get; set; }
        public IEnumerable<VitroSql.Modelo> Modelos { get; set; }
        public IEnumerable<VitroSql.Clasificacion> Clasificaciones { get; set; }
    }
}