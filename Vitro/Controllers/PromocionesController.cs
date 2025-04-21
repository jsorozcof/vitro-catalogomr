using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Vitro.Controllers
{
    [Authorize(Roles = "Administrador,Mercadotecnia")]
    public class PromocionesController : Controller
    {
        private readonly Models.ApplicationDbContext db = new Models.ApplicationDbContext();

        // GET: Promociones
        public ActionResult Index()
        {
            var model = new Models.PromocionesViewModel()
            {
                ProductosList = db.Productos.Where(x => x.Activo).ToList()
            };
            return View(model);
        }

        public ActionResult Details()
        {
            var promociones = db.ProductoPromociones.Include(x => x.Product).Where(x => x.Precio > 0).ToList();
            return View(promociones);
        }

        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var promocion = db.ProductoPromociones.Include(x => x.Product).Where(x => x.PromocionId.Equals(id)).FirstOrDefault();
            if (promocion == null)
            {
                return HttpNotFound();
            }
            var model = new Models.PromocionesViewModel()
            {
                ProductoId = promocion.ProductId,
                PromocionId = promocion.PromocionId,
                FechaFinal = promocion.FechaFinal,
                FechaInicio = promocion.FechaInicio,
                Descripcion = promocion.Product.Descripcion,
                SAP = promocion.Product.SAP,
                NAGS = promocion.Product.NAGS,
                Precio = promocion.Precio,
                Stock = promocion.Stock
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Models.PromocionesViewModel model)
        {
            if (!ModelState.IsValid)
            {

            }

            TimeSpan ndias = model.FechaFinal - model.FechaInicio;
            var promocion = db.ProductoPromociones.Where(x => x.PromocionId.Equals(model.PromocionId)).FirstOrDefault();
            promocion.FechaInicio = model.FechaInicio;
            promocion.FechaFinal = model.FechaFinal;
            promocion.FechaModificacion = DateTime.Now;
            promocion.DiasVigencia = ndias.Days;
            promocion.Stock = model.Stock;
            promocion.Precio = model.Precio;
            db.Entry<VitroSql.ProductoPromocion>(promocion).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult DownloadTemplate(Models.PromocionesViewModel model)
        {
            string filepath = $"{Server.MapPath("~/Resources/Files")}\\{Guid.NewGuid()}.xlsx";
            var productos = db.Productos.ToList();

            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(string));
            table.Columns.Add("SAP", typeof(string));
            table.Columns.Add("NAGS", typeof(string));
            table.Columns.Add("DESCRIPCION", typeof(string));
            table.Columns.Add("PRECIO", typeof(double));
            table.Columns.Add("STOCK", typeof(int));
            table.Columns.Add("FECHA INICIO", typeof(string));
            table.Columns.Add("FECHA FINAL", typeof(string));

            foreach (string idprod in model.Productos)
            {
                var producto = productos.Where(x => x.ProductoId.Equals(idprod)).FirstOrDefault();
                table.Rows.Add(new object[] { producto.ProductoId, producto.SAP, producto.NAGS, producto.Descripcion });
            }

            VitroCore.ExcelManager excel = new VitroCore.ExcelManager();
            excel.CreateFilePromociones(filepath, table);
            return File(filepath, "application/octet-stream", "PlantillaPromocion.xlsx");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            DataTable table = new VitroCore.ExcelManager().ReadFile(file.InputStream);
            List<VitroSql.ProductoPromocion> promociones = new List<VitroSql.ProductoPromocion>();

            foreach (DataRow row in table.Rows)
            {
                VitroSql.ProductoPromocion promocion = new VitroSql.ProductoPromocion() { PromocionId = $"{Guid.NewGuid()}", FechaCreacion = DateTime.Now };
                foreach (DataColumn column in table.Columns)
                {
                    switch (column.ColumnName)
                    {
                        case "ID":
                            promocion.ProductId = row[column].ToString();
                            break;
                        case "PRECIO":
                            promocion.Precio = Convert.ToDouble(row[column]);
                            break;
                        case "STOCK":
                            promocion.Stock = Convert.ToInt32(row[column]);
                            break;
                        case "FECHA INICIO":
                            promocion.FechaInicio = Convert.ToDateTime(row[column].ToString());
                            break;
                        case "FECHA FINAL":
                            promocion.FechaFinal = Convert.ToDateTime(row[column].ToString());
                            break;
                    }
                }
                TimeSpan ndias = promocion.FechaFinal - promocion.FechaInicio;
                promocion.DiasVigencia = ndias.Days;
                if (!db.ProductoPromociones.Any(x => x.ProductId.Equals(promocion.ProductId)))
                {
                    promociones.Add(promocion);
                }
            }

            db.ProductoPromociones.AddRange(promociones);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}