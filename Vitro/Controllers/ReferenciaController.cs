using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;

namespace Vitro.Controllers
{
    [Authorize(Roles = "Administrador,Mercadotecnia,Ingenieria")]
    public class ReferenciaController : Controller
    {
        private readonly Models.ApplicationDbContext db = new Models.ApplicationDbContext();

        public ActionResult Index(string Page)
        {
            Models.AtributoViewModel model = new Models.AtributoViewModel();
            switch (Page)
            {
                case "Pais":
                    model.Paises = db.Paises.ToArray().OrderBy(x => x.Nombre);
                    break;
                case "Marca":
                    model.Paises = db.Paises.Where(x => x.Activo).ToArray().OrderBy(x => x.Nombre);
                    model.Marcas = db.Marcas.Include(x => x.Pais).ToArray().OrderBy(x => x.Pais.Nombre).ThenBy(x => x.Nombre);
                    break;
                case "Modelo":
                    model.Paises = db.Paises.Where(x => x.Activo).ToArray().OrderBy(x => x.Nombre);
                    model.Modelos = db.Modelos.Include(x => x.Marca).Include(x => x.Marca.Pais).ToArray().OrderBy(x => x.Marca.Pais.Nombre).ThenBy(x => x.Marca.Nombre).ThenBy(x => x.Nombre);
                    break;
                case "Clasificacion":
                    model.Clasificaciones = db.Clasificaciones.ToArray().OrderBy(x => x.Nombre);
                    break;
                case "TPartes":
                    model.Clasificaciones = db.Clasificaciones.Where(x => x.Activo).ToArray().OrderBy(x => x.Nombre);
                    model.TipoPartes = db.TipoPartes.Include(x => x.Clasificacion).ToArray().OrderBy(x => x.Clasificacion.Nombre).ThenBy(x => x.Nombre);
                    break;
                case "TVidrios":
                    model.TipoVidrios = db.TipoVidrios.Where(x => x.Activo).ToArray().OrderBy(x => x.Nombre);
                    break;
                case "Color":
                    model.Colores = db.Colores.ToArray().OrderBy(x => x.Nombre);
                    break;
                case "Mercado":
                    model.Mercados = db.Mercados.ToArray().OrderBy(x => x.Nombre);
                    break;
                case "Procedencia":
                    model.Procedencias = db.Procedencias.ToArray().OrderBy(x => x.Nombre);
                    break;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(Models.AtributoViewModel model)
        {
            switch (model.Atributo)
            {
                case "Pais":
                    var pais = db.Paises.Where(x => x.Nombre.ToLower().Contains(model.Definicion.ToLower())).FirstOrDefault();
                    if (pais != null)
                    {
                        pais.Nombre = model.Definicion.ToUpper();
                        db.Entry(pais).State = EntityState.Modified;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(model.Definicion))
                        {
                            db.Paises.Add(new VitroSql.Pais()
                            {
                                PaisId = $"{Guid.NewGuid()}",
                                Nombre = model.Definicion.ToUpper(),
                                Activo = true
                            });
                        }
                    }
                    db.SaveChanges();
                    break;
                case "Marca":
                    var ref_pais = db.Paises.Where(x => x.PaisId.Equals(model.Referencia)).FirstOrDefault();

                    if (ref_pais != null)
                    {
                        var marcas = db.Marcas.Where(x => x.Pais.PaisId.Equals(ref_pais.PaisId)).ToArray();
                        if (marcas.Any(x => x.Nombre.ToLower().Contains(model.Definicion.ToLower())))
                        {
                            var marca = db.Marcas.Where(x => x.Nombre.ToLower().Contains(model.Definicion.ToLower())).FirstOrDefault();
                            marca.Nombre = model.Definicion.ToUpper();
                            marca.PaisId = model.Referencia;
                            db.Entry(marca).State = EntityState.Modified;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(model.Definicion))
                            {
                                db.Marcas.Add(new VitroSql.Marca()
                                {
                                    MarcaId = $"{Guid.NewGuid()}",
                                    Nombre = model.Definicion.ToUpper(),
                                    PaisId = model.Referencia,
                                    Activo = true
                                });
                            }
                        }
                    }

                    db.SaveChanges();
                    break;
                case "Modelo":
                    var ref_marca = db.Marcas.Where(x => x.MarcaId.Equals(model.Referencia)).FirstOrDefault();
                    if (ref_marca != null)
                    {
                        var modelos = db.Modelos.Where(x => x.Marca.MarcaId.Equals(ref_marca.MarcaId)).ToArray();

                        if (modelos.Any(x => x.Nombre.ToLower().Contains(model.Definicion.ToLower())))
                        {
                            var modelo = db.Modelos.Where(x => x.Nombre.ToLower().Contains(model.Definicion.ToLower())).FirstOrDefault();
                            modelo.Nombre = model.Definicion.ToUpper();
                            db.Entry(modelo).State = EntityState.Modified;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(model.Definicion))
                            {
                                db.Modelos.Add(new VitroSql.Modelo()
                                {
                                    ModeloId = $"{Guid.NewGuid()}",
                                    Nombre = model.Definicion.ToUpper(),
                                    MarcaId = ref_marca.MarcaId,
                                    Activo = true
                                });
                            }

                        }
                        db.SaveChanges();
                    }
                    break;
                case "Clasificacion":
                    var clasificacion = db.Clasificaciones.Where(x => x.Nombre.ToLower().Contains(model.Definicion.ToLower())).FirstOrDefault();
                    if (clasificacion != null)
                    {
                        clasificacion.Nombre = model.Definicion.ToUpper();
                        db.Entry(clasificacion).State = EntityState.Modified;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(model.Definicion))
                        {
                            db.Clasificaciones.Add(new VitroSql.Clasificacion()
                            {
                                ClasificacionId = $"{Guid.NewGuid()}",
                                Nombre = model.Definicion.ToUpper(),
                                Activo = true
                            });
                        }
                    }
                    db.SaveChanges();
                    break;
                case "TPartes":
                    var ref_clasificacion = db.Clasificaciones.Where(x => x.ClasificacionId.Equals(model.Referencia)).FirstOrDefault();
                    if (ref_clasificacion != null)
                    {
                        var tpartes = db.TipoPartes.Where(x => x.Clasificacion.ClasificacionId.Equals(ref_clasificacion.ClasificacionId)).ToArray();
                        if (tpartes.Any(x => x.Nombre.ToLower().Contains(model.Definicion.ToLower())))
                        {
                            var tparte = db.TipoPartes.Where(x => x.Nombre.ToLower().Contains(model.Definicion.ToLower())).FirstOrDefault();
                            tparte.Nombre = model.Definicion.ToUpper();
                            db.Entry(tparte).State = EntityState.Modified;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(model.Definicion))
                            {
                                db.TipoPartes.Add(new VitroSql.TipoParte()
                                {
                                    TipoParteId = $"{Guid.NewGuid()}",
                                    Nombre = model.Definicion.ToUpper(),
                                    ClasificacionId = model.Referencia,
                                    Activo = true
                                });
                            }

                        }
                        db.SaveChanges();
                    }
                    break;
                case "TVidrios":
                    var tvidrios = db.TipoVidrios.Where(x => x.Nombre.Contains(model.Definicion)).FirstOrDefault();
                    if (tvidrios != null)
                    {
                        tvidrios.Nombre = model.Definicion.ToUpper();
                        db.Entry(tvidrios).State = EntityState.Modified;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(model.Definicion))
                        {
                            db.TipoVidrios.Add(new VitroSql.TipoVidrio()
                            {
                                TipoVidrioId = $"{Guid.NewGuid()}",
                                Nombre = model.Definicion.ToUpper(),
                                Activo = true
                            });
                        }
                    }
                    db.SaveChanges();
                    break;
                case "Color":
                    var color = db.Colores.Where(x => x.Codigo.ToLower().Equals(model.Referencia.ToLower())).FirstOrDefault();
                    if (color != null)
                    {
                        color.Nombre = model.Definicion.ToUpper();
                        db.Entry(color).State = EntityState.Modified;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(model.Definicion))
                        {
                            db.Colores.Add(new VitroSql.Color()
                            {
                                ColorId = $"{Guid.NewGuid()}",
                                Nombre = model.Definicion.ToUpper(),
                                Codigo = model.Referencia.ToUpper(),
                                Activo = true
                            });
                        }
                    }
                    db.SaveChanges();
                    break;
                case "Mercado":
                    var mercado = db.Mercados.Where(x => x.Nombre.ToLower().Contains(model.Definicion.ToLower())).FirstOrDefault();
                    if (mercado != null)
                    {
                        mercado.Nombre = model.Definicion.ToUpper();
                        db.Entry(mercado).State = EntityState.Modified;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(model.Definicion))
                        {
                            db.Mercados.Add(new VitroSql.Mercado()
                            {
                                MercadoId = $"{Guid.NewGuid()}",
                                Nombre = model.Definicion.ToUpper(),
                                Activo = true
                            });
                        }
                    }
                    db.SaveChanges();
                    break;
                case "Procedencia":
                    var procedencia = db.Procedencias.Where(x => x.Nombre.ToLower().Contains(model.Definicion.ToLower())).FirstOrDefault();
                    if (procedencia != null)
                    {
                        procedencia.Nombre = model.Definicion.ToUpper();
                        db.Entry(procedencia).State = EntityState.Modified;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(model.Definicion))
                        {
                            db.Procedencias.Add(new VitroSql.Procedencia()
                            {
                                ProcedenciaId = $"{Guid.NewGuid()}",
                                Nombre = model.Definicion.ToUpper(),
                                Activo = true
                            });
                        }
                    }
                    db.SaveChanges();
                    break;
                default:
                    break;
            }
            return RedirectToAction("Index", new { Page = model.Atributo });
        }

        public ActionResult Edit(string id, string Page)
        {
            Models.AtributoEditViewModel model = new Models.AtributoEditViewModel();

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(Page))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            switch (Page)
            {
                case "Pais":
                    var pais = db.Paises.Where(x => x.PaisId.Equals(id)).FirstOrDefault();
                    if (pais != null)
                    {
                        model.EntityId = pais.PaisId;
                        model.Definicion = pais.Nombre;
                        model.Activo = pais.Activo;
                    }
                    else
                    {
                        return HttpNotFound();
                    }

                    break;
                case "Marca":
                    var marca = db.Marcas.Include(x => x.Pais).Where(x => x.MarcaId.Equals(id)).FirstOrDefault();
                    if (marca != null)
                    {
                        model.EntityId = marca.MarcaId;
                        model.Definicion = marca.Nombre;
                        model.PaisId = marca.Pais.PaisId;
                        model.Activo = marca.Activo;
                        model.Paises = db.Paises.ToArray();
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                    break;
                case "Modelo":
                    var modelo = db.Modelos.Include(x => x.Marca).Include(x => x.Marca.Pais).Where(x => x.ModeloId.Equals(id)).FirstOrDefault();
                    if (model != null)
                    {
                        model.EntityId = modelo.ModeloId;
                        model.PaisId = modelo.Marca.Pais.PaisId;
                        model.MarcaId = modelo.Marca.MarcaId;
                        model.Definicion = modelo.Nombre;
                        model.Activo = modelo.Activo;

                        model.Paises = db.Paises.ToArray();
                        model.Marcas = db.Marcas.Where(x => x.Pais.PaisId.Equals(modelo.Marca.Pais.PaisId)).ToArray();
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                    break;
                case "Clasificacion":
                    var clasificacion = db.Clasificaciones.Where(x => x.ClasificacionId.Equals(id)).FirstOrDefault();
                    if (clasificacion != null)
                    {
                        model.EntityId = clasificacion.ClasificacionId;
                        model.Definicion = clasificacion.Nombre;
                        model.Activo = clasificacion.Activo;
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                    break;
                case "TPartes":
                    var tparte = db.TipoPartes.Where(x => x.TipoParteId.Equals(id)).FirstOrDefault();
                    if (tparte != null)
                    {
                        model.EntityId = tparte.TipoParteId;
                        model.Activo = tparte.Activo;
                        model.Definicion = tparte.Nombre;
                        model.ClasificacionId = tparte.ClasificacionId;

                        model.Clasificaciones = db.Clasificaciones.Where(x => x.Activo).ToArray();
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                    break;
                case "TVidrios":
                    var tvidrio = db.TipoVidrios.Where(x => x.TipoVidrioId.Equals(id)).FirstOrDefault();
                    if (tvidrio != null)
                    {
                        model.EntityId = tvidrio.TipoVidrioId;
                        model.Definicion = tvidrio.Nombre;
                        model.Activo = tvidrio.Activo;
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                    break;
                case "Color":
                    var color = db.Colores.Where(x => x.ColorId.Equals(id)).FirstOrDefault();
                    if (color != null)
                    {
                        model.EntityId = color.ColorId;
                        model.Definicion = color.Nombre;
                        model.Codigo = color.Codigo;
                        model.Activo = color.Activo;
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                    break;
                case "Mercado":
                    var mercado = db.Mercados.Where(x => x.MercadoId.Equals(id)).FirstOrDefault();
                    if (mercado != null)
                    {
                        model.EntityId = mercado.MercadoId;
                        model.Definicion = mercado.Nombre;
                        model.Activo = mercado.Activo;
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                    break;
                case "Procedencia":
                    var procedencia = db.Procedencias.Where(x => x.ProcedenciaId.Equals(id)).FirstOrDefault();
                    if (procedencia != null)
                    {
                        model.EntityId = procedencia.ProcedenciaId;
                        model.Definicion = procedencia.Nombre;
                        model.Activo = procedencia.Activo;
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                    break;
            }
            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Models.AtributoEditViewModel model)
        {
            switch (model.Atributo)
            {
                case "Pais":
                    var pais = db.Paises.Where(x => x.PaisId.Equals(model.EntityId)).FirstOrDefault();
                    pais.Nombre = model.Definicion.ToUpper();
                    pais.Activo = model.Activo;
                    db.Entry(pais).State = EntityState.Modified;
                    db.SaveChanges();
                    break;
                case "Marca":
                    var marca = db.Marcas.Where(x => x.MarcaId.Equals(model.EntityId)).FirstOrDefault();
                    marca.Nombre = model.Definicion.ToUpper();
                    marca.PaisId = model.PaisId;
                    marca.Activo = model.Activo;
                    db.Entry(marca).State = EntityState.Modified;
                    db.SaveChanges();
                    break;
                case "Modelo":
                    var modelo = db.Modelos.Include(x => x.Marca.Pais).Where(x => x.ModeloId.Equals(model.EntityId)).FirstOrDefault();
                    modelo.Nombre = model.Definicion.ToUpper();
                    modelo.Activo = model.Activo;
                    modelo.MarcaId = model.MarcaId;
                    modelo.Marca.PaisId = model.PaisId;
                    db.Entry(modelo).State = EntityState.Modified;
                    db.SaveChanges();
                    break;
                case "Clasificacion":
                    var clasificacion = db.Clasificaciones.Where(x => x.ClasificacionId.Equals(model.EntityId)).FirstOrDefault();
                    clasificacion.Nombre = model.Definicion.ToUpper();
                    clasificacion.Activo = model.Activo;
                    db.Entry(clasificacion).State = EntityState.Modified;
                    db.SaveChanges();
                    break;
                case "TPartes":
                    var tpartes = db.TipoPartes.Where(x => x.TipoParteId.Equals(model.EntityId)).FirstOrDefault();
                    tpartes.Nombre = model.Definicion.ToUpper();
                    tpartes.ClasificacionId = model.ClasificacionId;
                    tpartes.Activo = model.Activo;
                    db.Entry(tpartes).State = EntityState.Modified;
                    db.SaveChanges();
                    break;
                case "TVidrios":
                    var tvidrios = db.TipoVidrios.Where(x => x.TipoVidrioId.Equals(model.EntityId)).FirstOrDefault();
                    tvidrios.Nombre = model.Definicion.ToUpper();
                    tvidrios.Activo = model.Activo;
                    db.Entry(tvidrios).State = EntityState.Modified;
                    db.SaveChanges();
                    break;
                case "Color":
                    var color = db.Colores.Where(x => x.ColorId.Equals(model.EntityId)).FirstOrDefault();
                    color.Nombre = model.Definicion.ToUpper();
                    color.Codigo = model.Codigo.ToUpper();
                    color.Activo = model.Activo;
                    db.Entry(color).State = EntityState.Modified;
                    db.SaveChanges();
                    break;
                case "Mercado":
                    var mercado = db.Mercados.Where(x => x.MercadoId.Equals(model.EntityId)).FirstOrDefault();
                    mercado.Nombre = model.Definicion.ToUpper();
                    mercado.Activo = model.Activo;
                    db.Entry(mercado).State = EntityState.Modified;
                    db.SaveChanges();
                    break;
                case "Procedencia":
                    var procedencia = db.Procedencias.Where(x => x.ProcedenciaId.Equals(model.EntityId)).FirstOrDefault();
                    procedencia.Nombre = model.Definicion.ToUpper();
                    procedencia.Activo = model.Activo;
                    db.Entry(procedencia).State = EntityState.Modified;
                    db.SaveChanges();
                    break;
            }
            return RedirectToAction("Index", new { Page = model.Atributo });
        }
    }
}