using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Vitro.Controllers
{
    [RoutePrefix("api/Container")]
    public class ContainerController : ApiController
    {
        private readonly Models.ApplicationDbContext db = new Models.ApplicationDbContext();

        [HttpGet]
        [Route("Paises")]
        public IEnumerable<VitroSql.Pais> Paises()
        {
            return db.Paises.Where(x => x.Activo).ToArray();
        }

        [HttpGet]
        [Route("Marcas/{id}")]
        public IEnumerable<VitroSql.Marca> Marcas(string id)
        {
            return db.Marcas.Where(x => x.Pais.PaisId.Equals(id) && x.Activo).ToArray().OrderBy(x => x.Nombre);
        }

        [HttpGet]
        [Route("Modelos/{id}")]
        public IEnumerable<VitroSql.Modelo> Modelos(string id)
        {
            return db.Modelos.Where(x => x.Marca.MarcaId.Equals(id) && x.Activo).ToArray().OrderBy(x => x.Nombre);
        }

        [HttpGet]
        [Route("TipoPartes/{id}")]
        public IEnumerable<VitroSql.TipoParte> TipoParte(string id)
        {
            return db.TipoPartes.Where(x => x.Clasificacion.ClasificacionId.Equals(id) && x.Activo).ToArray().OrderBy(x => x.Nombre);
        }

        [HttpGet]
        [Route("Producto/ByNAGS/{id}")]
        public bool ExistsProductoByCodigoNAGS(string id)
        {
            return db.Productos.Any(x => x.SAP.Equals(id));
        }
    }
}
