using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data;
using Vitro.Models;

namespace Vitro.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private readonly Models.ApplicationDbContext db = new Models.ApplicationDbContext();

        // GET: Search
        public ActionResult Index()
        {
            var user = db.Users.Include(x => x.Pais).Where(x => x.UserName.Equals(User.Identity.Name)).FirstOrDefault();
            var model = new Models.SearchViewModel()
            {
                Marcas = db.Marcas.Where(x => x.PaisId.Equals(user.PaisId ?? string.Empty) && x.Activo).OrderBy(x => x.Nombre).ToArray()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Retrieve(Models.SearchViewModel model)
        {
            string[] parametros = { };
            if (!string.IsNullOrEmpty(model.Busqueda))
            {
                parametros = model.Busqueda.Split(',');
            }

            switch (model.Mode)
            {
                case "SearchViewModel":
                    if (model.Parametro.Equals("SAP"))
                    {
                        model.Productos = db.Productos.Include(x => x.Modelo.Marca.Pais).Include(x => x.Modelo.Marca).Include(x => x.TipoParte).Include(x => x.Modelo).Include(x => x.TipoParte.Clasificacion)
                            .Where(x => parametros.Contains(x.SAP) && x.Activo)
                            .OrderBy(x => x.Modelo.Nombre).ThenBy(x => x.StartYear).ThenBy(x => x.TipoParte.Clasificacion.Nombre).ThenBy(x => x.TipoParte.Nombre).ToArray();
                        model.ProductoImagenes = db.ProductoImagenes.Include(x => x.Imagen).ToArray();
                    }
                    else
                    {
                        model.Productos = db.Productos.Include(x => x.Modelo.Marca.Pais).Include(x => x.Modelo.Marca).Include(x => x.TipoParte).Include(x => x.Modelo).Include(x => x.TipoParte.Clasificacion)
                            .Where(x => parametros.Contains(x.NAGS) && x.Activo)
                            .OrderBy(x => x.Modelo.Nombre).ThenBy(x => x.StartYear).ThenBy(x => x.TipoParte.Clasificacion.Nombre).ThenBy(x => x.TipoParte.Nombre).ToArray();
                        model.ProductoImagenes = db.ProductoImagenes.Include(x => x.Imagen).ToArray();
                    }
                    break;
                case "SearchViewModel2":
                    int year = int.Parse(model.Year ?? "0");
                    if (year > 0)
                    {
                        model.Productos = db.Productos.Include(x => x.Modelo.Marca.Pais).Include(x => x.Modelo.Marca).Include(x => x.TipoParte).Include(x => x.Modelo).Include(x => x.TipoParte.Clasificacion)
                            .Where(x => x.Modelo.Marca.MarcaId.Equals(model.Marca) && x.Modelo.ModeloId.Equals(model.Modelo) && year >= x.StartYear && year <= x.EndYear && x.Activo)
                            .OrderBy(x => x.Modelo.Nombre).ThenBy(x => x.StartYear).ThenBy(x => x.TipoParte.Clasificacion.Nombre).ThenBy(x => x.TipoParte.Nombre).ToArray();
                        model.ProductoImagenes = db.ProductoImagenes.Include(x => x.Imagen).ToArray();
                    }
                    else
                    {
                        model.Productos = db.Productos.Include(x => x.Modelo.Marca.Pais).Include(x => x.Modelo.Marca).Include(x => x.TipoParte).Include(x => x.Modelo).Include(x => x.TipoParte.Clasificacion)
                            .Where(x => x.Modelo.Marca.MarcaId.Equals(model.Marca) && x.Modelo.ModeloId.Equals(model.Modelo) && x.Activo)
                            .OrderBy(x => x.Modelo.Nombre).ThenBy(x => x.StartYear).ThenBy(x => x.TipoParte.Clasificacion.Nombre).ThenBy(x => x.TipoParte.Nombre).ToArray();
                        model.ProductoImagenes = db.ProductoImagenes.Include(x => x.Imagen).ToArray();
                    }
                    break;
            }
            var user = db.Users.Include(x => x.Pais).Where(x => x.UserName.Equals(User.Identity.Name)).FirstOrDefault();
            model.Marcas = db.Marcas.Where(x => x.PaisId.Equals(user.PaisId ?? string.Empty) && x.Activo).OrderBy(x => x.Nombre).ToArray();
            if (User.IsInRole("Cliente"))
            {
                model.Productos = model.Productos.Where(x => x.Modelo.Marca.Pais.PaisId.Equals(user.PaisId ?? string.Empty));
            }

            if (model.Productos.Count() == 1)
            {
                var producto = model.Productos.FirstOrDefault();
                model.Homologos = db.Productos.Where(x => x.NAGS.Contains(producto.NAGS) && !x.ProductoId.Equals(producto.ProductoId)).ToList();
            }
            return View("Index", model);
        }

        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!db.Productos.Any(x => x.ProductoId.Equals(id)))
            {
                return HttpNotFound();
            }

            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!db.Productos.Any(x => x.ProductoId.Equals(id)))
            {
                return HttpNotFound();
            }

            var producto = db.Productos.Include(x => x.Modelo.Marca).Include(x => x.Modelo).Include(x => x.TipoParte).Include(x => x.TipoVidrio).Include(x => x.TipoParte.Clasificacion).Include(x => x.Mercado).Include(x => x.Color).Include(x => x.Procedencia).Where(x => x.ProductoId.Equals(id)).FirstOrDefault();
            var homologos = db.Productos.Include(x => x.Modelo).Where(x => x.NAGS.ToLower().Contains(producto.NAGS.ToLower())).ToList();
          

            var viewmodel = new Models.DetailsProductoViewModel()
            {
                Producto = producto,
                ProductoImagen = db.ProductoImagenes.Include(x => x.Imagen).Where(x => x.ProductoId.Equals(producto.ProductoId)).ToArray(),
                MassiveProductImage = db.MassiveProductImages.Where(x => x.ProductoId.Equals(producto.ProductoId)).OrderBy(or => or.Posicion).ToArray(),
                //ImagenCargue = db.ImagenesCargue.Where(x => !x.CargueRef.Equals(producto.ProductoId)).ToArray(),
                Homologos = homologos.Where(x => !x.ProductoId.Equals(producto.ProductoId)).ToList()
            };


            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Suggest(Models.SearchViewModel model)
        {
            var user = db.Users.Where(x => x.UserName.Equals(User.Identity.Name)).FirstOrDefault();
            string output = null;
            db.Sugerencias.Add(new VitroSql.Sugerencia()
            {
                SugerenciaId = user.Id,
                Marca = model.SugerenciaViewModel.Marca,
                Modelo = model.SugerenciaViewModel.Modelo,
                TipoParte = model.SugerenciaViewModel.TipoParte,
                FechaCreacion = DateTime.Now,
                Descripcion = model.SugerenciaViewModel.Descripcion
            }
                );
            try
            {
                db.SaveChanges();
            }
            catch (SmtpException ex)
            {
                output = "Error Guardando Registro: " + ex.Message;
            }

            var emailconf = db.MailConfigs.FirstOrDefault();

            MailMessage mail = new MailMessage();
            mail.To.Add(new MailAddress(user.Email));
            mail.Subject = "Requerimiento Producto - Catalogo Virtual";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = "<p>Buen d&iacute;a,</p><p>A continuaci&oacute;n encontrar&aacute; los datos de productos no encontrados:</p><p>Fecha: " + DateTime.Now.ToString("dd-MM-yyyy") + "</p><p>C&oacute;digo del usuario: " + user.UserName + "</p><p>Nombre del usuario:: " + user.FullName + "</p><p>Correo del usuario: " + user.Email + "</p><p>Tel&eacute;fono: " + user.PhoneNumber + "</p><p>Marca: " + model.SugerenciaViewModel.Marca + "</p><p>Modelo: " + model.SugerenciaViewModel.Modelo + "</p><p>Tipo Parte: " + model.SugerenciaViewModel.TipoParte + "</p><p>Descripci&oacute;n: " + model.SugerenciaViewModel.Descripcion + "</p>";
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.Normal;
            mail.From = new MailAddress(emailconf.MailAccount);

            SmtpClient client = new SmtpClient();

            client.Port = emailconf.Puerto;
            client.EnableSsl = emailconf.HabilitarSSL.Value;
            client.Host = emailconf.Host;
            client.Credentials = new NetworkCredential(emailconf.MailAccount, emailconf.MailPassword);

            try
            {
                client.Send(mail);
                output = "Su requerimiento fue enviado exitosamente, lo estaremos contactando.";
            }
            catch (SmtpException ex)
            {
                output = "Error al enviar la solicitud" + ex.Message;
            }
            TempData["Message"] = output;
            return RedirectToAction("Index");
        }
    }
}