using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vitro.Controllers
{
    public class ConfiguracionController : Controller
    {
        private readonly Models.ApplicationDbContext db = new Models.ApplicationDbContext();

        // GET: Configuracion
        public ActionResult Index()
        {
            var configuraciones = db.Configuraciones.FirstOrDefault();
            var model = new Models.ConfiguracionViewModel()
            {
                ConfiguracionId = configuraciones.ConfiguracionId,
                DiasVigenciaNuevosProductos = configuraciones.DiasVigenciaNuevosProductos,
                MailConfig = db.MailConfigs.FirstOrDefault() ?? new VitroSql.MailConfig() { }
            };
            return View(model);
        }

        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var configuracion = db.Configuraciones.Where(x => x.ConfiguracionId.Equals(id)).FirstOrDefault();
            var model = new Models.ConfiguracionViewModel()
            {
                ConfiguracionId = configuracion.ConfiguracionId,
                DiasVigenciaNuevosProductos = configuracion.DiasVigenciaNuevosProductos
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Models.ConfiguracionViewModel model)
        {
            var configuracion = db.Configuraciones.Where(x => x.ConfiguracionId.Equals(model.ConfiguracionId)).FirstOrDefault();
            configuracion.DiasVigenciaNuevosProductos = model.DiasVigenciaNuevosProductos;
            db.Entry<VitroSql.Configuracion>(configuracion).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Mail()
        {
            if (db.MailConfigs.Any())
            {
                var configuracion = db.MailConfigs.FirstOrDefault();
                var model = new Models.MailConfigViewModel()
                {
                    Host = configuracion.Host ?? string.Empty,
                    MailAccount = configuracion.MailAccount ?? string.Empty,
                    MailPassword = configuracion.MailPassword ?? string.Empty,
                    Puerto = configuracion.Puerto,
                    HabilitarSSL = configuracion.HabilitarSSL.Value
                };
                return View(model);
            }
            else
            {
                return View();
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Mail(Models.MailConfigViewModel model)
        {
            var configuracion = db.MailConfigs.FirstOrDefault();
            if (configuracion == null)
            {
                db.MailConfigs.Add(new VitroSql.MailConfig()
                {
                    MailConfigId = $"{Guid.NewGuid()}",
                    Host = model.Host,
                    HabilitarSSL = model.HabilitarSSL,
                    MailAccount = model.MailAccount,
                    MailPassword = model.MailPassword,
                    Puerto = model.Puerto
                });
            }
            else
            {
                configuracion.HabilitarSSL = model.HabilitarSSL;
                configuracion.Host = model.Host;
                configuracion.MailAccount = model.MailAccount;
                configuracion.MailPassword = model.MailPassword;
                configuracion.Puerto = model.Puerto;
                db.Entry(configuracion).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}