using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;
using System.Net;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using VitroSql;
using static iTextSharp.text.pdf.AcroFields;
using VitroCore.Services;
using System.Web.Configuration;
using Microsoft.Ajax.Utilities;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Web.Http.Results;

namespace Vitro.Controllers
{
    [Authorize(Roles = "Administrador,Mercadotecnia,Ingenieria")]
    public class ProductoController : Controller
    {
        private readonly Models.ApplicationDbContext db = new Models.ApplicationDbContext();
        //private readonly ProcessProductRepository _productoService;

        public ActionResult Index()
        {
            var user = db.Users.Include(x => x.Pais).Where(x => x.UserName.Equals(User.Identity.Name)).FirstOrDefault();
            var model = new Models.ProductoViewModel()
            {
                Marcas = db.Marcas.Where(x => x.PaisId.Equals(user.PaisId ?? string.Empty) && x.Activo).OrderBy(x => x.Nombre).ToArray()
            };
            model.TotalProductos = db.Productos.Where(x => x.Modelo.Marca.Pais.PaisId.Equals(user.PaisId) && x.Activo).Count();
            model.TotalproductIMG = (from PD in db.Productos
                                     join PI in db.ProductoImagenes on PD.ProductoId equals PI.ProductoId
                                     join IMG in db.Imagenes on PI.ImagenId equals IMG.ImagenId
                                     where IMG.Nombre.Contains("default")
                                     select new { PD.ProductoId }).Count();

            
            return View(model);
        }

        public ActionResult Retrieve(Models.ProductoViewModel model)
        {
            switch (model.Mode)
            {
                case "ProductoViewModel":
                    if (model.Parametro.Equals("SAP"))
                    {
                        model.TbProduct = db.TbProduct.Include(x => x.Color).Include(x => x.Modelo.Marca).Include(x => x.Mercado).Include(x => x.Modelo).Include(x => x.Modelo.Marca.Pais).Include(x => x.TipoParte).Include(x => x.TipoParte.Clasificacion).Include(x => x.Procedencia).Include(x => x.TipoVidrio).Where(x => x.SAP.Contains(model.Busqueda) && x.Activo).OrderBy(x => x.Modelo.Nombre).ThenBy(x => x.StartYear).ThenBy(x => x.TipoParte.Clasificacion.Nombre).ThenBy(x => x.TipoParte.Nombre).ToArray();
                        model.ProductImages = db.ProductImages.Where(x => x.Sap == model.Busqueda).ToArray();
                        //model.ProductoImagenes = db.ProductoImagenes.Include(x => x.Imagen).ToArray();
                    }
                    else
                    {
                        model.TbProduct = db.TbProduct.Include(x => x.Color).Include(x => x.Modelo.Marca).Include(x => x.Mercado).Include(x => x.Modelo).Include(x => x.Modelo.Marca.Pais).Include(x => x.TipoParte).Include(x => x.TipoParte.Clasificacion).Include(x => x.Procedencia).Include(x => x.TipoVidrio).Where(x => x.NAGS.Contains(model.Busqueda) && x.Activo).OrderBy(x => x.Modelo.Nombre).ThenBy(x => x.StartYear).ThenBy(x => x.TipoParte.Clasificacion.Nombre).ThenBy(x => x.TipoParte.Nombre).ToArray();
                        model.ProductImages = db.ProductImages.Where(x => x.Sap == model.Busqueda).ToArray();
                    }
                    break;
                case "ProductoViewModel2":
                    int year = int.Parse(model.Year ?? "0");
                    if (year > 0)
                    {
                        model.TbProduct = db.TbProduct.Include(x => x.Color).Include(x => x.Modelo.Marca).Include(x => x.Mercado).Include(x => x.Modelo).Include(x => x.Modelo.Marca.Pais).Include(x => x.TipoParte).Include(x => x.TipoParte.Clasificacion).Include(x => x.Procedencia).Include(x => x.TipoVidrio).Where(x => x.Modelo.Marca.MarcaId.Equals(model.Marca) && x.Modelo.ModeloId.Equals(model.Modelo) && year >= x.StartYear && year <= x.EndYear && x.Activo).OrderBy(x => x.Modelo.Nombre).ThenBy(x => x.StartYear).ThenBy(x => x.TipoParte.Clasificacion.Nombre).ThenBy(x => x.TipoParte.Nombre).ToArray();
                        //model.Productos = db.Productos.Include(x => x.Modelo.Marca.Pais).Include(x => x.Modelo.Marca).Include(x => x.TipoParte).Include(x => x.Modelo).Include(x => x.TipoParte.Clasificacion).Where(x => x.Modelo.Marca.MarcaId.Equals(model.Marca) && x.Modelo.ModeloId.Equals(model.Modelo) && year >= x.StartYear && year <= x.EndYear && x.Activo).OrderBy(x => x.Modelo.Nombre).ThenBy(x => x.StartYear).ThenBy(x => x.TipoParte.Clasificacion.Nombre).ThenBy(x => x.TipoParte.Nombre).ToArray();
                        model.ProductImages = db.ProductImages.Where(x => x.Sap == model.Busqueda).ToArray();
                    }
                    else
                    {
                        model.TbProduct = db.TbProduct.Include(x => x.Color).Include(x => x.Modelo.Marca).Include(x => x.Mercado).Include(x => x.Modelo).Include(x => x.Modelo.Marca.Pais).Include(x => x.TipoParte).Include(x => x.TipoParte.Clasificacion).Include(x => x.Procedencia).Include(x => x.TipoVidrio).Where(x => x.Modelo.Marca.MarcaId.Equals(model.Marca) && x.Modelo.ModeloId.Equals(model.Modelo) && x.Activo).OrderBy(x => x.Modelo.Nombre).ThenBy(x => x.StartYear).ThenBy(x => x.TipoParte.Clasificacion.Nombre).ThenBy(x => x.TipoParte.Nombre).ToArray();
                        //model.Productos = db.Productos.Include(x => x.Modelo.Marca.Pais).Include(x => x.Modelo.Marca).Include(x => x.TipoParte).Include(x => x.Modelo).Include(x => x.TipoParte.Clasificacion).Where(x => x.Modelo.Marca.MarcaId.Equals(model.Marca) && x.Modelo.ModeloId.Equals(model.Modelo) && x.Activo).OrderBy(x => x.Modelo.Nombre).ThenBy(x => x.StartYear).ThenBy(x => x.TipoParte.Clasificacion.Nombre).ThenBy(x => x.TipoParte.Nombre).ToArray();
                        model.ProductImages = db.ProductImages.Where(x => x.Sap == model.Busqueda).ToArray();
                    }
                    break;
            }
            var user = db.Users.Include(x => x.Pais).Where(x => x.UserName.Equals(User.Identity.Name)).FirstOrDefault();
            model.Marcas = db.Marcas.Where(x => x.PaisId.Equals(user.PaisId ?? string.Empty) && x.Activo).OrderBy(x => x.Nombre).ToArray();
            model.TotalProductos = db.TbProduct.Where(x => x.Modelo.Marca.Pais.PaisId.Equals(user.PaisId) && x.Activo).Count();
            model.TotalproductIMG = (from PD in db.TbProduct
                                     join PI in db.ProductImages on PD.ProductId equals PI.ProductId
                                     //join IMG in db.pro on PI.ImagenId equals IMG.ImagenId
                                     //where PI.Nombre.Contains("default")
                                     where PI.Nombre.Contains("img1")
                                     select new { PD.ProductId }).Count();
            return View("Index", model);
        }

        public ActionResult Create()
        {
            var model = new Models.ProductoViewModel() { PaisList = db.Paises.Where(x => x.Activo).ToArray().OrderBy(x => x.Nombre), ProcedenciaList = db.Procedencias.Where(x => x.Activo).ToArray().OrderBy(x => x.Nombre), TipoVidroList = db.TipoVidrios.Where(x => x.Activo).ToArray().OrderBy(x => x.Nombre), ColorList = db.Colores.Where(x => x.Activo).ToArray().OrderBy(x => x.Nombre), MercadoList = db.Mercados.Where(x => x.Activo).ToArray().OrderBy(x => x.Nombre), Clasificaciones = db.Clasificaciones.Where(x => x.Activo).ToArray().OrderBy(x => x.Nombre) };
            return View(model);
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

            //var producto = db.Productos.Include(x => x.Modelo.Marca).Include(x => x.Modelo).Include(x => x.TipoParte).Include(x => x.TipoVidrio).Include(x => x.TipoParte.Clasificacion).Include(x => x.Mercado).Include(x => x.Color).Include(x => x.Procedencia).Where(x => x.ProductoId.Equals(id)).FirstOrDefault();
            var producto = db.TbProduct.Include(x => x.Modelo.Marca).Include(x => x.Modelo).Include(x => x.TipoParte).Include(x => x.TipoVidrio).Include(x => x.TipoParte.Clasificacion).Include(x => x.Mercado).Include(x => x.Color).Include(x => x.Procedencia).Where(x => x.ProductId.Equals(id)).FirstOrDefault();
            var viewmodel = new Models.DetailsProductoViewModel()
            {
                Product = producto,
                ProductoImagen = db.ProductoImagenes.Include(x => x.Imagen).Where(x => x.ProductoId.Equals(producto.ProductId)).ToArray()
            };
            return View(viewmodel);
        }

        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!db.Productos.Any(x => x.ProductoId.Equals(id)))
            {
                return HttpNotFound();
            }

            var producto = db.Productos.Include(x => x.Modelo.Marca).Include(x => x.Modelo).Include(x => x.Modelo.Marca.Pais).Include(x => x.TipoParte).Include(x => x.TipoVidrio).Include(x => x.TipoParte.Clasificacion).Include(x => x.Mercado).Include(x => x.Color).Include(x => x.Procedencia).Where(x => x.ProductoId.Equals(id)).FirstOrDefault();

            var viewmodel = new Models.ProductoViewModel()
            {
                ProductoId = producto.ProductoId,
                Pais = producto.Modelo.Marca.Pais.PaisId,
                Marca = producto.Modelo.Marca.MarcaId,
                SAP = producto.SAP,
                NAGS = producto.NAGS,
                StartYear = producto.StartYear,
                EndYear = producto.EndYear,
                Clasificacion = producto.TipoParte.Clasificacion.ClasificacionId,
                Procedencia = producto.Procedencia.ProcedenciaId,
                Alto = producto.Alto,
                Ancho = producto.Ancho,
                Color = producto.Color.ColorId,
                Descripcion = producto.Descripcion,
                Mercado = producto.Mercado.MercadoId,
                Modelo = producto.Modelo.ModeloId,
                Boton = producto.Boton,
                Red = producto.Red,
                Serigrafia = producto.Serigrafia,
                SensorLluvia = producto.SensorLluvia,
                Moldura = producto.Moldura,
                SensorCondensacion = producto.SensorCondensacion,
                Holder = producto.Holder,
                Antena = producto.Antena,
                SubEnsamble = producto.SubEnsamble,
                Homologo = producto.Homologo,
                Activo = producto.Activo,
                Perforacion = producto.Perforacion,
                TipoParte = producto.TipoParte.TipoParteId,
                TipoVidrio = producto.TipoVidrio.TipoVidrioId,

                ColorList = db.Colores.ToArray(),
                MarcaList = db.Marcas.ToArray(),
                MercadoList = db.Mercados.ToArray(),
                ModeloList = db.Modelos.ToArray(),
                TipoParteList = db.TipoPartes.ToArray(),
                TipoVidroList = db.TipoVidrios.ToArray(),
                PaisList = db.Paises.ToArray(),
                Clasificaciones = db.Clasificaciones.ToArray(),
                ProcedenciaList = db.Procedencias.ToArray(),
                ProductoImagenes = db.ProductoImagenes.Include(x => x.Imagen).Where(x => x.ProductoId.Equals(producto.ProductoId)).ToArray()
            };
            //viewmodel.NombreImagen = db.ProductoImagenes.Where(x => x.ProductoId.Equals(producto.ProductoId)).FirstOrDefault().Imagen.Nombre;
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Models.ProductoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var viewmodel = new Models.ProductoViewModel()
                {
                    ProductoId = model.ProductoId,
                    Pais = model.Pais,
                    Marca = model.Marca,
                    SAP = model.SAP,
                    NAGS = model.NAGS,
                    StartYear = model.StartYear,
                    EndYear = model.EndYear,
                    Clasificacion = model.Clasificacion,
                    Procedencia = model.Procedencia,
                    Alto = model.Alto,
                    Ancho = model.Ancho,
                    Color = model.Color,
                    Descripcion = model.Descripcion,
                    Mercado = model.Mercado,
                    Modelo = model.Modelo,
                    Boton = model.Boton,
                    Red = model.Red,
                    Serigrafia = model.Serigrafia,
                    SensorLluvia = model.SensorLluvia,
                    Moldura = model.Moldura,
                    Antena = model.Antena,
                    SubEnsamble = model.SubEnsamble,
                    SensorCondensacion = model.SensorCondensacion,
                    Holder = model.Holder,
                    Homologo = model.Homologo,
                    Perforacion = model.Perforacion,
                    TipoParte = model.TipoParte,
                    TipoVidrio = model.TipoVidrio,
                    Activo = model.Activo,

                    ColorList = db.Colores.ToArray(),
                    MarcaList = db.Marcas.ToArray(),
                    MercadoList = db.Mercados.ToArray(),
                    ModeloList = db.Modelos.ToArray(),
                    TipoParteList = db.TipoPartes.ToArray(),
                    TipoVidroList = db.TipoVidrios.ToArray(),
                    PaisList = db.Paises.ToArray(),
                    Clasificaciones = db.Clasificaciones.ToArray(),
                    ProcedenciaList = db.Procedencias.ToArray()
                };

                return View("Edit", viewmodel);
            }

            var producto = db.Productos.Where(x => x.ProductoId.Equals(model.ProductoId)).FirstOrDefault();
            producto.SAP = model.SAP;
            producto.NAGS = model.NAGS;
            producto.ModeloId = model.Modelo;
            producto.Descripcion = model.Descripcion;
            producto.MercadoId = model.Mercado;
            producto.ColorId = model.Color;
            producto.TipoVidrioId = model.TipoVidrio;
            producto.TipoParteId = model.TipoParte;
            producto.Ancho = model.Ancho;
            producto.Alto = model.Alto;
            producto.Boton = model.Boton;
            producto.Red = model.Red;
            producto.Serigrafia = model.Serigrafia;
            producto.SensorLluvia = model.SensorLluvia;
            producto.Holder = model.Holder;
            producto.SensorCondensacion = model.SensorCondensacion;
            producto.Moldura = model.Moldura;
            producto.Homologo = model.Homologo;
            producto.Perforacion = model.Perforacion;
            producto.StartYear = model.StartYear;
            producto.EndYear = model.EndYear;
            producto.Activo = model.Activo;
            producto.ProcedenciaId = model.Procedencia;
            producto.FechaModificacion = DateTime.Now;
            producto.Antena = model.Antena;
            producto.SubEnsamble = model.SubEnsamble;

            if (model.Files[0] != null && model.Files.Length > 0)
            {
                db.ProductoImagenes.RemoveRange(db.ProductoImagenes.Where(x => x.ProductoId.Equals(producto.ProductoId)).ToArray());
                foreach (var file in model.Files)
                {
                    var imagen = new VitroSql.Imagen()
                    {
                        ImagenId = $"{Guid.NewGuid()}",
                        Nombre = file.FileName,
                        ImageSize = file.ContentLength,
                        ImageType = file.ContentType,
                        FechaCreacion = DateTime.Now
                    };
                    db.Imagenes.Add(imagen);

                    db.ProductoImagenes.Add(new VitroSql.ProductoImagen()
                    {
                        ProductoImagenId = $"{Guid.NewGuid()}",
                        ImagenId = imagen.ImagenId,
                        ProductoId = producto.ProductoId
                    });
                    ServerUploadsFolder();
                    file.SaveAs(Path.Combine(Server.MapPath("~/Resources/Uploads"), Path.GetFileName(file.FileName)));
                }
            }

            db.Entry(producto).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!db.Productos.Any(x => x.ProductoId.Equals(id)))
            {
                return HttpNotFound();
            }

            //var producto = db.Productos.Include(x => x.Modelo.Marca).Include(x => x.Modelo).Include(x => x.TipoParte).Include(x => x.TipoVidrio).Include(x => x.TipoParte.Clasificacion).Include(x => x.Mercado).Include(x => x.Color).Include(x => x.Procedencia).Where(x => x.ProductoId.Equals(id)).FirstOrDefault();
            var producto = db.TbProduct.Include(x => x.Modelo.Marca).Include(x => x.Modelo).Include(x => x.TipoParte).Include(x => x.TipoVidrio).Include(x => x.TipoParte.Clasificacion).Include(x => x.Mercado).Include(x => x.Color).Include(x => x.Procedencia).Where(x => x.ProductId.Equals(id)).FirstOrDefault();
            var viewmodel = new Models.DetailsProductoViewModel()
            {
                Product = producto,
                ProductoImagen = db.ProductoImagenes.Include(x => x.Imagen).Where(x => x.ProductId.Equals(producto.ProductId)).ToArray()
            };
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(FormCollection collection)
        {
            string id = collection["PRDID"];
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (db.Productos.Any(x => x.ProductoId.Equals(id)))
            {
                var producto = db.Productos.Where(x => x.ProductoId.Equals(id)).FirstOrDefault();
                db.ProductoImagenes.RemoveRange(db.ProductoImagenes.Where(x => x.ProductoId.Equals(producto.ProductoId)));
                db.Productos.Remove(producto);
                db.SaveChanges();
            }
            else
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Models.ProductoViewModel model)
        {
            if (!db.Marcas.Any(x => x.MarcaId.Equals(model.Marca)))
            {
                ModelState.AddModelError("Marca", "No se ha seleccionado una marca");
            }
            if (!db.Modelos.Any(x => x.ModeloId.Equals(model.Modelo)))
            {
                ModelState.AddModelError("Modelo", "No se ha seleccionado un modelo");
            }
            if (model.Files[0] == null)
            {
                ModelState.AddModelError("Files", "Debe especificar un archivo de imagen");
            }
            if (!ModelState.IsValid)
            {
                var viewmodel = new Models.ProductoViewModel()
                {
                    ProductoId = model.ProductoId,
                    Pais = model.Pais,
                    Marca = model.Marca,
                    SAP = model.SAP,
                    NAGS = model.NAGS,
                    StartYear = model.StartYear,
                    EndYear = model.EndYear,
                    Clasificacion = model.Clasificacion,
                    Procedencia = model.Procedencia,
                    Alto = model.Alto,
                    Ancho = model.Ancho,
                    Color = model.Color,
                    Descripcion = model.Descripcion,
                    Mercado = model.Mercado,
                    Modelo = model.Modelo,
                    Boton = model.Boton,
                    Red = model.Red,
                    Serigrafia = model.Serigrafia,
                    SensorLluvia = model.SensorLluvia,
                    Holder = model.Holder,
                    SensorCondensacion = model.SensorCondensacion,
                    Moldura = model.Moldura,
                    Homologo = model.Homologo,
                    Antena = model.Antena,
                    SubEnsamble = model.SubEnsamble,
                    Perforacion = model.Perforacion,
                    TipoParte = model.TipoParte,
                    TipoVidrio = model.TipoVidrio,
                    Activo = model.Activo,

                    ColorList = db.Colores.ToArray(),
                    MarcaList = db.Marcas.ToArray(),
                    MercadoList = db.Mercados.ToArray(),
                    ModeloList = db.Modelos.ToArray(),
                    TipoParteList = db.TipoPartes.ToArray(),
                    TipoVidroList = db.TipoVidrios.ToArray(),
                    PaisList = db.Paises.ToArray(),
                    Clasificaciones = db.Clasificaciones.ToArray(),
                    ProcedenciaList = db.Procedencias.ToArray()
                };
                return View("Create", viewmodel);
            }

            var producto = new VitroSql.Producto()
            {
                ProductoId = $"{Guid.NewGuid()}",
                SAP = model.SAP,
                NAGS = model.NAGS,
                ModeloId = model.Modelo,
                StartYear = model.StartYear,
                EndYear = model.EndYear,
                Descripcion = model.Descripcion,
                TipoParteId = model.TipoParte,
                TipoVidrioId = model.TipoVidrio,
                Perforacion = model.Perforacion,
                Ancho = model.Ancho,
                Alto = model.Alto,
                Boton = model.Boton,
                Red = model.Red,
                Serigrafia = model.Serigrafia,
                SensorLluvia = model.SensorLluvia,
                Holder = model.Holder,
                Moldura = model.Moldura,
                SensorCondensacion = model.SensorCondensacion,
                ColorId = model.Color,
                Antena = model.Antena,
                SubEnsamble = model.SubEnsamble,
                Homologo = model.Homologo,
                MercadoId = model.Mercado,
                ProcedenciaId = model.Procedencia,
                Activo = true,
                FechaCreacion = DateTime.Now
            };
            db.Productos.Add(producto);

            if (model.Files[0] != null && model.Files.Length > 0)
            {
                foreach (var file in model.Files)
                {
                    string filename = $"{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}{DateTime.Now.Second}{file.FileName}";
                    var imagen = new VitroSql.Imagen()
                    {
                        ImagenId = $"{Guid.NewGuid()}",
                        Nombre = filename,
                        ImageSize = file.ContentLength,
                        ImageType = file.ContentType,
                        FechaCreacion = DateTime.Now
                    };
                    db.Imagenes.Add(imagen);

                    db.ProductoImagenes.Add(new VitroSql.ProductoImagen()
                    {
                        ProductoImagenId = $"{Guid.NewGuid()}",
                        ImagenId = imagen.ImagenId,
                        ProductoId = producto.ProductoId
                    });

                    ServerUploadsFolder();
                    file.SaveAs(Path.Combine(Server.MapPath("~/Resources/Uploads"), Path.GetFileName(filename)));
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Upload(string State)
        {
            if (!string.IsNullOrEmpty(State) && State.Equals("Fails"))
            {
                if(TempData["ProccessFailsCount"] != null )
                {
                    
                     var errors = TempData["ProccessDataError"] as DataTable;
                     var modelView = new Models.UploadViewModel();
                    if (errors != null && errors.Rows.Count > 0)
                    {

                        var modelList = new List<Models.LogErrorCargaViewModel>();
                        for (int i = 0; i < errors.Rows.Count; i++)
                        {
                            var model = new Models.LogErrorCargaViewModel();

                            model.FechaProceso = Convert.ToDateTime(errors.Rows[i]["FECHA_PROCESO"]);
                            model.Usuario = errors.Rows[i]["USUARIO"].ToString();
                            model.Fila =  Convert.ToInt32(errors.Rows[i]["FILA"]);
                            model.Columna = errors.Rows[i]["COLUMNA"].ToString();
                            model.ValorIncorrecto = errors.Rows[i]["VALOR_INCORRECTO"].ToString();
                            model.DescripcionError = errors.Rows[i]["DESCRIPCION_ERROR"].ToString();

                            modelList.Add(model);
                            
                        }
                        modelView = new Models.UploadViewModel()
                        {
                            Errores = modelList
                        };
                        ViewBag.Errores = modelList;
                        //return RedirectToAction("Upload", new { State = "Fails" });
                        return View(modelView);
                    }
                }

                if (TempData["ErrorImageUploads"] != null)
                {
                    var errors = TempData["ErrorImageUploads"] as List<VitroSql.TempProducto>;
                    if (errors != null && errors.Count > 0)
                    {
                        var model = new Models.UploadViewModel()
                        {
                            TempProductos = errors
                        };
                        return View(model);
                    }
                }
                else
                {
                    return RedirectToAction("Upload", new { State = "Upload" });
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Upload(Models.UploadViewModel model)
        {
            db.TemporalProductos.RemoveRange(db.TemporalProductos.ToArray());
            model.Recursos = "C:\\imagenes_catalogo";

            ServerUploadsFolder();
            if (!model.File.FileName.Contains("plantilla_cargue_inicial.xlsx") && !model.File.FileName.Contains("plantilla_cargue_actualizar.xlsx"))
            {
               TempData["ErrorMensaje"] = null;
               TempData["ErrorMensaje"] = string.Format($"ERROR: Este archivo no es conocido");
                return RedirectToAction("Upload", new { State = "Fails" });
            }

            if (model.File == null)
            {
                ModelState.AddModelError("File", "Debe seleccionar un archivo para la carga de productos.");
            }
            var filefilter = new string[] { ".xls", ".xlsx" };
            if (!filefilter.Contains(Path.GetExtension(model.File.FileName)))
            {
                ModelState.AddModelError("File", "El archivo indicado no contiene un formato correcto.");
            }
            if (!Directory.Exists(model.Recursos))
            {
                ModelState.AddModelError("File", "Directorio de recursos no es válido o no existe");
            }

            DataTable table = new VitroCore.ExcelManager().ReadFile(model.File.InputStream);
            ProcessProductRepository _processProductRepository = new ProcessProductRepository();

            if (table.Rows.Count > 1000)
            {
                ModelState.AddModelError("File", "El archivo indicado supera los 1000 registros máximos para procesar");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            List<VitroSql.TempProducto> reg_errors = new List<VitroSql.TempProducto>();
            DataTable Errores = new DataTable();
           
            List<string> imagenes = new List<string>();
            var listProductoImagen = new List<ProductImages>();
            var productImages = new List<ProductImages>();
            var user = db.Users.Include(x => x.Pais).Where(x => x.UserName.Equals(User.Identity.Name)).FirstOrDefault();

            if (table.Rows.Count <= 1000)
            {
                foreach (DataRow rows in table.Rows)
                {
                    VitroSql.TempProducto temp = new VitroSql.TempProducto();
                    VitroSql.Producto producto = new VitroSql.Producto();

                    temp.ProductoId = $"{Guid.NewGuid()}";
                    //temp.Pais = user.Pais.Nombre ?? string.Empty;
                    temp.FechaCreacion = DateTime.Now;
                    int IndiceImagen = 1;
                    foreach (DataColumn column in table.Columns)
                    {
                        switch (column.ColumnName)
                        {
                            case "PAIS":
                                temp.Pais = rows[0].ToString();
                                break;
                            case "SAP":
                                temp.SAP = rows[1].ToString();
                                break;
                            case "NAGS":
                                temp.NAGS = rows[2].ToString();
                                break;
                            case "MARCA":
                                temp.MarcaId = rows[3].ToString();
                                break;
                            case "MODELO":
                                temp.ModeloId = rows[4].ToString();
                                break;
                            case "AÑO INICIAL":
                                temp.StartYear = int.Parse(rows[5].ToString());
                                break;
                            case "AÑO FINAL":
                                temp.EndYear = int.Parse(rows[6].ToString());
                                break;
                            case "DESCRIPCION":
                                temp.Descripcion = rows[7].ToString();
                                break;
                            case "TIPO PARTE":
                                temp.TipoParteId = rows[8].ToString();
                                break;
                            case "PERFORACION":
                                temp.Perforacion = double.Parse(rows[9].ToString());
                                break;
                            case "ANCHO":
                                temp.Ancho = double.Parse(rows[10].ToString());
                                break;
                            case "ALTO":
                                temp.Alto = double.Parse(rows[11].ToString());
                                break;
                            case "BOTON":
                                temp.Boton = rows[12].ToString().Equals("SI") ? true : false;
                                break;
                            case "RED":
                                temp.Red = rows[13].ToString().Equals("SI") ? true : false;
                                break;
                            case "SERIGRAFIA":
                                temp.Serigrafia = rows[14].ToString().Equals("SI") ? true : false;
                                break;
                            case "SENSOR LLUVIA":
                                temp.SensorLluvia = rows[15].ToString().Equals("SI") ? true : false;
                                break;
                            case "MOLDURA":
                                temp.Moldura = rows[16].ToString().Equals("SI") ? true : false;
                                break;
                            case "HOLDER":
                                temp.Holder = rows[17].ToString().Equals("SI") ? true : false;
                                break;
                            case "ANTENA":
                                temp.Antena = rows[18].ToString().Equals("SI") ? true : false;
                                break;
                            case "SUB ENSAMBLE":
                                temp.SubEnsamble = rows[19].ToString().Equals("SI") ? true : false;
                                break;
                            case "SENSOR CONDENSACION":
                                temp.SensorCondensacion = rows[20].ToString().Equals("SI") ? true : false;
                                break;
                            case "COLOR":
                                temp.ColorId = rows[21].ToString();
                                break;
                            case "TIPO VIDRIO":
                                temp.TipoVidrioId = rows[22].ToString();
                                break;
                            case "PROCEDENCIA":
                                temp.ProcedenciaId = rows[23].ToString();
                                break;
                            case "HOMOLOGO":
                                temp.Homologo = rows[24].ToString().Equals("SI") ? true : false;
                                break;
                            case "CLASIFICACION":
                                temp.ClasificacionId = rows[25].ToString();
                                break;
                            case "MERCADO":
                                temp.MercadoId = rows[26].ToString();
                                break;
                        }

                        //int indiceImagen = 1;
                        //if (column.ColumnName.ToLower().StartsWith("imagen"))
                        //{
                        //    var valor = rows[column].ToString().Trim();
                        //    if (!string.IsNullOrWhiteSpace(valor) && Directory.Exists(model.Recursos))
                        //    {
                        //        try
                        //        {
                        //            var fullPath = Path.Combine(model.Recursos, valor);
                        //            if (!System.IO.File.Exists(fullPath))
                        //                throw new FileNotFoundException($"No se encontró la imagen: {fullPath}");

                        //            var nombreArchivo = $"{DateTime.Now:yyyyMMdd}_{valor}";
                        //            var extension = Path.GetExtension(nombreArchivo);
                        //            var rutaUpload = Path.Combine(Server.MapPath("~/Resources/Uploads/"), nombreArchivo);

                        //            Image.FromFile(fullPath).Save(rutaUpload);
                        //            var contenido = System.IO.File.ReadAllBytes(rutaUpload);

                        //            listProductoImagen.Add(new ProductImages
                        //            {
                        //                ProductId = "", //temp.ProductoId,
                        //                Sap = temp.SAP,
                        //                Nombre = nombreArchivo,
                        //                Extension = extension,
                        //                Contenido = contenido,
                        //                Posicion = null,
                        //                FechaCreacion = temp.FechaCreacion
                        //            });
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            // Puedes usar imagen por defecto o registrar el error
                        //            System.Diagnostics.Debug.WriteLine($"ERROR IMAGEN: {ex.Message}");
                        //            // Imagen por defecto si deseas
                        //        }
                        //    }

                        //    #region "Coment codigo anterior"
                        //    //if (rows[column].ToString() != "")
                        //    //{
                        //    //    if (Directory.Exists(model.Recursos))
                        //    //    {
                        //    //        try
                        //    //        {
                        //    //            string root = Path.GetFullPath(model.Recursos);
                        //    //            var filename = string.Empty;
                        //    //            string extension = string.Empty;

                        //    //            var result = Path.Combine(root, rows[column].ToString().Trim());
                        //    //            Image image = Image.FromFile(result);
                        //    //            filename = $"{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}__{rows[column].ToString()}";
                        //    //            image.Save(Path.Combine(Server.MapPath("~/Resources/Uploads/"), filename));
                        //    //            temp.RefImagen = filename;
                        //    //            extension = Path.GetExtension(filename);
                        //    //            imagenes.Add(filename);


                        //    //            string DefaultImagePath = Server.MapPath("~/Resources/Uploads/") + filename;
                        //    //            byte[] imageArray = System.IO.File.ReadAllBytes(DefaultImagePath);
                        //    //            string base64ImageRepresentation = Convert.ToBase64String(imageArray);


                        //    //            byte[] Imgbytes = imageArray;
                        //    //            string ImagenId = $"{Guid.NewGuid()}";


                        //    //            if (model.Actualizar)
                        //    //            {
                        //    //                if (!model.File.FileName.Contains("plantilla_cargue_actualizar.xlsx"))
                        //    //                {
                        //    //                    TempData["ErrorMensaje"] = null;
                        //    //                    TempData["ErrorMensaje"] = string.Format($"ERROR: La plantilla selecionada para actualizar {model.File.FileName} no es la correcta ");
                        //    //                    return RedirectToAction("Upload", new { State = "Fails" });
                        //    //                }

                        //    //                var listMassiveSearch = db.ProductImages.Where(m => m.Sap == temp.SAP).ToList();
                        //    //                var listProduct = db.TbProduct.Where(m => m.SAP == temp.SAP).FirstOrDefault();
                        //    //                if (listProduct != null)
                        //    //                {
                        //    //                    if (listMassiveSearch.Count > 0)
                        //    //                    {
                        //    //                        foreach (var detail in listMassiveSearch)
                        //    //                        {
                        //    //                            db.ProductImages.Remove(detail);
                        //    //                            db.SaveChanges();
                        //    //                        }

                        //    //                    }
                        //    //                    //else
                        //    //                    //{
                        //    //                    //    TempData["ErrorMensaje"] = null;
                        //    //                    //    TempData["ErrorMensaje"] = string.Format($"ERROR: El producto {temp.SAP} no tiene imagenes asociadas para actualizar");
                        //    //                    //    return RedirectToAction("Upload", new { State = "Fails" });

                        //    //                    //}

                        //    //                }

                        //    //                else
                        //    //                {
                        //    //                    TempData["ErrorMensaje"] = null;
                        //    //                    TempData["ErrorMensaje"] = string.Format($"ERROR: El producto {temp.SAP} no ha sido creado");
                        //    //                    return RedirectToAction("Upload", new { State = "Fails" });

                        //    //                }

                        //    //                productImages.Add(new VitroSql.ProductImages()
                        //    //                {
                        //    //                    Nombre = filename,
                        //    //                    Sap = temp.SAP,
                        //    //                    Extension = extension,
                        //    //                    Contenido = Imgbytes,
                        //    //                    Posicion = IndiceImagen,
                        //    //                    ProductId = ""
                        //    //                });
                        //    //            }

                        //    //            else if (model.File.FileName.Contains("plantilla_cargue_actualizar.xlsx"))
                        //    //            {
                        //    //                TempData["ErrorMensaje"] = null;
                        //    //                TempData["ErrorMensaje"] = string.Format($"ERROR: La plantilla seleccionada para cargar {model.File.FileName} no es la correcta, Si esta intentado actualizar selecione la casilla Actualizar Registros ");
                        //    //                return RedirectToAction("Upload", new { State = "Fails" });

                        //    //            }
                        //    //            else
                        //    //            {
                        //    //                //List add
                        //    //                productImages.Add(new VitroSql.ProductImages()
                        //    //                {
                        //    //                    Nombre = filename,
                        //    //                    Sap = temp.SAP,
                        //    //                    Extension = extension,
                        //    //                    Contenido = Imgbytes,
                        //    //                    Posicion = IndiceImagen,
                        //    //                    ProductId = ""

                        //    //                });
                        //    //            }

                        //    //            IndiceImagen++;
                        //    //        }
                        //    //        catch (Exception error)
                        //    //        {
                        //    //            string root = Path.GetFullPath(model.Recursos);
                        //    //            var filename = string.IsNullOrEmpty(rows[column].ToString()) ? $"{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}__{rows[0].ToString()}" : $"{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}__{rows[column].ToString()}";
                        //    //            Image image = Image.FromFile(Path.Combine(root, "default.jpg"));
                        //    //            image.Save(Path.Combine(Server.MapPath("~/Resources/Uploads/"), filename));
                        //    //            temp.RefImagen = filename;
                        //    //            imagenes.Add(filename);
                        //    //            System.Diagnostics.Debug.WriteLine($"ERROR: {error.Message}");
                        //    //        }
                        //    //    }

                        //    //}
                        //    #endregion
                        //}
                    }
                    if (temp.RefImagen == null)
                    {
                        reg_errors.Add(temp);
                    }
                    db.TemporalProductos.Add(temp);
                }
                db.SaveChanges();
                Errores = _processProductRepository.ProcesarProductos(table, listProductoImagen,"COLOMBIA", model.Actualizar, user.FullName);
                
                //db.Database.ExecuteSqlCommand("exec sp_cargue @PAIS, @USERNAME, @ACTUALIZAPRODUCTOS", new SqlParameter("@PAIS", "COLOMBIA"), new SqlParameter("@USERNAME", User.Identity.Name), new SqlParameter("@ACTUALIZAPRODUCTOS", model.Actualizar));
                db.Database.CommandTimeout = 300;
            }

            TempData["ProccessRowsCount"] = db.TemporalProductos.Count(x => x.Valido);
            TempData["ErrorImageUploadsCount"] = Errores.Rows.Count;
            TempData["ErrorImageUploads"] = reg_errors;
            TempData["ProccessDataError"] = Errores;
            TempData["ProccessSuccessCount"] = db.TemporalProductos.Count(x => x.Valido);
            TempData["ProccessFailsCount"] = Errores.Rows.Count; //db.TemporalProductos.Count(x => !x.Valido);
            InsertorUpdateMaxImage(productImages, model.Actualizar, model.File.FileName, table, Errores, user.FullName);
            return Errores.Rows.Count > 0 ? RedirectToAction("Upload", new { State = "Fails" }) : RedirectToAction("Upload", new { State = "Upload" });
            //return reg_errors.Count > 0 ? RedirectToAction("Upload", new { State = "Upload" }) : RedirectToAction("Upload", new { State = "Upload" });
            //return Errores.Rows.Count > 0 ? RedirectToAction("Upload", new { State = "Fails" }) : RedirectToAction("Upload", new { State = "Upload" });

        }

        public  ActionResult InsertorUpdateMaxImage(List<ProductImages> list, bool update, string fileName, DataTable table, DataTable errors, string user)
        {
            int errorImageUploadsCount = 0;

            try
            {
                foreach (DataRow row in table.Rows)
                {
                    var sap = row["SAP"].ToString();
                    var product = db.TbProduct.FirstOrDefault(p => p.SAP == sap);
                    if (product == null) continue;

                    int posicion = 1;

                    foreach (DataColumn col in table.Columns)
                    {
                        if (col.ColumnName.ToLower().StartsWith("imagen"))
                        {
                            var valor = row[col].ToString().Trim();

                            if (!string.IsNullOrWhiteSpace(valor) && Directory.Exists("C:\\imagenes_catalogo"))
                            {
                                string originalName = row[col].ToString();
                                string extension = Path.GetExtension(originalName);
                                string root = Path.GetFullPath("C:\\imagenes_catalogo");
                                string sourceImagePath = Path.Combine(root, originalName);

                                string filename = $"{sap}_img{posicion}{extension}";
                                string destinationPath = Path.Combine(Server.MapPath("~/Resources/Uploads/"), filename);

                                try
                                {
                                    Image image = Image.FromFile(sourceImagePath);
                                    image.Save(destinationPath);

                                    byte[] imageArray = System.IO.File.ReadAllBytes(destinationPath);
                                    byte[] Imgbytes = imageArray;

                                    Debug.WriteLine($"Procesando imagen: SAP={sap}, Posición={posicion}, Nombre={filename}");

                                    // Verifica si ya existe una imagen en esa posición
                                    var existingImage = db.ProductImages
                                        .FirstOrDefault(img => img.ProductId == product.ProductId && img.Posicion == posicion);

                                    if (existingImage != null)
                                    {
                                        // Actualiza imagen existente
                                        existingImage.Nombre = filename;
                                        existingImage.Contenido = Imgbytes;
                                        existingImage.Extension = extension?.TrimStart('.');
                                        existingImage.FechaActualizacion = DateTime.UtcNow;

                                    }
                                    else
                                    {
                                        // Inserta nueva imagen
                                        db.ProductImages.Add(new ProductImages
                                        {
                                            ProductId = product.ProductId,
                                            Sap = sap,
                                            ImagenId = Guid.NewGuid(),
                                            Nombre = filename,
                                            Posicion = posicion,
                                            Contenido = Imgbytes,
                                            Extension = extension?.TrimStart('.'),
                                            FechaCreacion = DateTime.UtcNow
                                        });
                                    }

                                    posicion++;
                                }
                                catch (FileNotFoundException exc)
                                {
                                    errorImageUploadsCount++;
                                    var log = new TbLogErrores
                                    {
                                        Usuario = user,
                                        Modulo = "CargueMasivo",
                                        Operacion = update ? "Actualizando imagen" : "Cargar imagen",
                                        Mensaje = $"Imagen no encontrada: {sourceImagePath}",
                                        DetalleError = exc.InnerException?.Message,
                                        StackTrace = exc.StackTrace,
                                        Archivo = fileName,
                                        Linea = null, // Podrías parsear la línea del stack si deseas
                                        SAP = sap,
                                        ProductoId = product?.ProductId
                                    };

                                    db.TbLogErrores.Add(log);
                                    //db.SaveChanges();
                                    continue;
                                }
                                catch (Exception ex)
                                {
                                    errorImageUploadsCount++;
                                    Debug.WriteLine($"Error al cargar imagen '{sourceImagePath}': {ex.Message}");
                                    var log = new TbLogErrores
                                    {
                                        Usuario = user,
                                        Modulo = "CargueMasivo",
                                        Operacion = update ? "Actualizando imagen" : "Cargar imagen",
                                        Mensaje = $"Error al cargar imagen '{sourceImagePath}': {ex.Message}",
                                        DetalleError = ex.InnerException?.Message,
                                        StackTrace = ex.StackTrace,
                                        Archivo = fileName,
                                        Linea = null, 
                                        SAP = sap,
                                        ProductoId = product?.ProductId
                                    };

                                    db.TbLogErrores.Add(log);
                                    continue;
                                }
                            }
                        }
                    }
                }

                var resultado = db.SaveChanges();
                Debug.WriteLine($"Cambios guardados en base de datos: {resultado}");
            }
            catch (Exception error)
            {
                Debug.WriteLine($"ERROR general: {error.Message}");
                var log = new TbLogErrores
                {
                    Usuario = user,
                    Modulo = "CargueMasivo",
                    Operacion = "Cargar ",
                    Mensaje = $"ERROR general: {error.Message}",
                    DetalleError = error.InnerException?.Message,
                    StackTrace = error.StackTrace,
                    Archivo = fileName,
                    Linea = null,
                    SAP = "",
                    ProductoId = ""
                };

                db.TbLogErrores.Add(log);
            }

            db.SaveChanges();

            TempData["ProccessRowsCount"] = table.Rows.Count;
            TempData["ErrorImageUploadsCount"] = errorImageUploadsCount;
            //TempData["ErrorImageUploads"] = errors;
            //TempData["ProccessSuccessCount"] = db.TemporalProductos.Count(x => x.Procesado); //db.TemporalProductos.Count(x => x.Valido);
            TempData["ProccessDataError"] = errors;
            TempData["ProccessFailsCount"] = errors.Rows.Count;//db.TemporalProductos.Count(x => !x.Valido);
            return errors.Rows.Count > 0 ? RedirectToAction("Upload", new { State = "Fails" }) : RedirectToAction("Upload", new { State = "Upload" });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResultUpload()
        {
            string filename = $"{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}RESULTADOSCARGUE.xlsx";
            var last = db.HistoricoCargues.Max(x => x.IdCargue);
            var historico = db.HistoricoCargues.Where(x => x.IdCargue == last).ToArray();
            DataTable table = new DataTable();
            table.Columns.Add("SAP", typeof(string));
            table.Columns.Add("NAGS", typeof(string));
            table.Columns.Add("MODELOID", typeof(string));
            table.Columns.Add("AÑO INICIAL", typeof(string));
            table.Columns.Add("AÑO FINAL", typeof(string));
            table.Columns.Add("DESCRIPCION", typeof(string));
            table.Columns.Add("TIPO PARTE", typeof(string));
            table.Columns.Add("PERFORACION", typeof(string));
            table.Columns.Add("ANCHO", typeof(string));
            table.Columns.Add("ALTO", typeof(string));
            table.Columns.Add("BOTON", typeof(string));
            table.Columns.Add("RED", typeof(string));
            table.Columns.Add("SERIGRAFIA", typeof(string));
            table.Columns.Add("SENSOR LLUVIA", typeof(string));
            table.Columns.Add("MOLDURA", typeof(string));
            table.Columns.Add("HOLDER", typeof(string));
            table.Columns.Add("ANTENA", typeof(string));
            table.Columns.Add("SUB_ENSAMBLE", typeof(string));
            table.Columns.Add("SENSOR CONDENSACION", typeof(string));
            table.Columns.Add("COLORID", typeof(string));
            table.Columns.Add("TIPOVIDIOID", typeof(string));
            table.Columns.Add("PROCEDENCIAID", typeof(string));
            table.Columns.Add("HOMOLOGO", typeof(string));
            table.Columns.Add("MERCADOID", typeof(string));
            table.Columns.Add("VALIDO", typeof(string));
            table.Columns.Add("RESULTADO", typeof(string));

            foreach (var reg in historico)
            {
                DataRow row = table.NewRow();
                row[0] = reg.SAP;
                row[1] = reg.NAGS;
                row[2] = reg.ModeloId ?? string.Empty;
                row[3] = reg.StartYear;
                row[4] = reg.EndYear;
                row[5] = reg.Descripcion;
                row[6] = reg.TipoParteId;
                row[7] = reg.Perforacion;
                row[8] = reg.Ancho;
                row[9] = reg.Alto;
                row[10] = reg.Boton ? "SI" : "NO";
                row[11] = reg.Red ? "SI" : "NO";
                row[12] = reg.Serigrafia ? "SI" : "NO";
                row[13] = reg.SensorLluvia ? "SI" : "NO";
                row[14] = reg.Moldura ? "SI" : "NO";
                row[15] = reg.Holder ? "SI" : "NO";
                row[16] = reg.Antena ? "SI" : "NO";
                row[17] = reg.SubEnsamble ? "SI" : "NO";
                row[18] = reg.SensorCondensacion ? "SI" : "NO";
                row[19] = reg.ColorId;
                row[20] = reg.TipoVidrioId;
                row[21] = reg.ProcedenciaId;
                row[22] = reg.Homologo ? "SI" : "NO";
                row[23] = reg.MercadoId;
                row[24] = reg.Valido ? "SI" : "NO";
                row[25] = reg.Resultado;
                table.Rows.Add(row);
            }
            VitroCore.ExcelManager excel = new VitroCore.ExcelManager();
            excel.CreateFile(Path.Combine(Server.MapPath("~/Resources/Files"), filename), table);

            return File(Path.Combine(Server.MapPath("~/Resources/Files"), filename), "application/octet-stream", "resumen-cargue.xlsx");
        }

        private void ServerUploadsFolder()
        {
            if (!Directory.Exists(Server.MapPath("~/Resources/Uploads")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Resources/Uploads"));
            }
        }
    }
}