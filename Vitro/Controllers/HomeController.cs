using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Vitro.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly Models.ApplicationDbContext db = new Models.ApplicationDbContext();
        public ActionResult Index()
        {
            DateTime FechaFinal = DateTime.Now.AddDays(-db.Configuraciones.FirstOrDefault().DiasVigenciaNuevosProductos);
            var productos = db.TbProduct.Include(x => x.Modelo).Include(x => x.Modelo.Marca).Where(x => x.FechaCreacion > FechaFinal).OrderBy(x => x.Modelo.Marca.Nombre).ThenBy(x => x.Modelo.Nombre).ToList();
            var promociones = db.ProductoPromociones.Include(x => x.Product).Include(x => x.Product.Modelo.Marca).Include(x => x.Product.Modelo).Where(x => x.FechaFinal >= DateTime.Today).OrderByDescending(x => x.FechaInicio).ToList();

            var model = new Models.HomeViewModel()
            {
                ProductosNuevos = productos,
                ProductosPromocion = promociones
            };
            return View(model);
        }
    }
}