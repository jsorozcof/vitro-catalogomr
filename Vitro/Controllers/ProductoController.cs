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
using ClosedXML.Excel;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text.RegularExpressions;

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
            model.TotalProductos = db.TbProduct.Where(x => x.Modelo.Marca.Pais.PaisId.Equals(user.PaisId) && x.Activo).Count();
            model.TotalproductIMG = (from PD in db.TbProduct
                                     join PI in db.ProductoImagenes on PD.ProductId equals PI.ProductoId
                                     join IMG in db.Imagenes on PI.ImagenId equals IMG.ImagenId
                                     where IMG.Nombre.Contains("default")
                                     select new { PD.ProductId }).Count();

            
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
            if (!db.TbProduct.Any(x => x.ProductId.Equals(id)))
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
            if (!db.TbProduct.Any(x => x.ProductId.Equals(id)))
            {
                return HttpNotFound();
            }

            var producto = db.TbProduct.Include(x => x.Modelo.Marca).Include(x => x.Modelo).Include(x => x.Modelo.Marca.Pais).Include(x => x.TipoParte).Include(x => x.TipoVidrio).Include(x => x.TipoParte.Clasificacion).Include(x => x.Mercado).Include(x => x.Color).Include(x => x.Procedencia).Where(x => x.ProductId.Equals(id)).FirstOrDefault();

            var viewmodel = new Models.ProductoViewModel()
            {
                ProductoId = producto.ProductId,
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
                ProductoImagenes = db.ProductoImagenes.Include(x => x.Imagen).Where(x => x.ProductId.Equals(producto.ProductId)).ToArray()
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

            var producto = db.TbProduct.Where(x => x.ProductId.Equals(model.ProductoId)).FirstOrDefault();
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
                db.ProductoImagenes.RemoveRange(db.ProductoImagenes.Where(x => x.ProductoId.Equals(producto.ProductId)).ToArray());
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
                        ProductoId = producto.ProductId
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
            if (!db.TbProduct.Any(x => x.ProductId.Equals(id)))
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

            if (db.TbProduct.Any(x => x.ProductId.Equals(id)))
            {
                var producto = db.TbProduct.Where(x => x.ProductId.Equals(id)).FirstOrDefault();
                db.ProductoImagenes.RemoveRange(db.ProductoImagenes.Where(x => x.ProductoId.Equals(producto.ProductId)));
                db.TbProduct.Remove(producto);
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

            var user = db.Users.Include(x => x.Pais).Where(x => x.UserName.Equals(User.Identity.Name)).FirstOrDefault();
            var _clasificacion = db.Clasificaciones.Where(x => x.ClasificacionId.Equals(model.Clasificacion)).FirstOrDefault();
            var producto = new VitroSql.TbProduct()
            {
                PaisId = model.Pais,
                ProductId = $"{Guid.NewGuid()}",
                SAP = model.SAP,
                NAGS = model.NAGS,
                MarcaId = model.Marca,
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
                Clasificacion = _clasificacion.Nombre,
                MercadoId = model.Mercado,
                ProcedenciaId = model.Procedencia,
                CreadoPor = user.FullName,
                Activo = true,
                FechaCreacion = DateTime.Now
            };
            db.TbProduct.Add(producto);

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

                    //db.ProductoImagenes.Add(new VitroSql.ProductoImagen()
                    //{
                    //    ProductoImagenId = $"{Guid.NewGuid()}",
                    //    ImagenId = imagen.ImagenId,
                    //    ProductoId = producto.ProductId
                    //});
                   
                    ServerUploadsFolder();
                    file.SaveAs(Path.Combine(Server.MapPath("~/Resources/Uploads"), Path.GetFileName(filename)));
                    string destinationPath = Path.Combine(Server.MapPath("~/Resources/Uploads/"), filename);
                    string extension = Path.GetExtension(filename);

                    byte[] imageArray = System.IO.File.ReadAllBytes(destinationPath);
                    byte[] Imgbytes = imageArray;
                    db.ProductImages.Add(new ProductImages
                    {
                        ProductId = producto.ProductId,
                        Sap = producto.SAP,
                        ImagenId = Guid.NewGuid(),
                        Nombre = filename,
                        Posicion = 1,
                        Contenido = Imgbytes,
                        Extension = extension?.TrimStart('.'),
                        FechaCreacion = DateTime.UtcNow
                    });

                }
            }
            db.SaveChanges();
            TempData["ProductoCreado"] = true;
            return RedirectToAction("Create");
            //return RedirectToAction("Index");
        }

        public ActionResult Upload(string State)
        {
            try
            {
                TempData.Remove("ProccessSuccessCount");
                if (!string.IsNullOrEmpty(State) && State.Equals("Fails"))
                {
                    var modelView = new Models.UploadViewModel();

                    // Validación de errores de proceso
                    //if (TempData["ProccessFailsCount"] != null)
                    if(TempData["ProccessFailsCount"] != null && Convert.ToInt32(TempData["ProccessFailsCount"]) > 0)
                       {
                        var errorsTable = TempData["ProccessDataError"] as DataTable;

                        if (errorsTable != null && errorsTable.Rows.Count > 0)
                        {
                            var modelList = new List<Models.LogErrorCargaViewModel>();
                            var columns = errorsTable.Columns;
                            foreach (DataRow row in errorsTable.Rows)
                            {
                                var model = new Models.LogErrorCargaViewModel
                                {
                                    FechaProceso = columns.Contains("FECHA_PROCESO") && row["FECHA_PROCESO"] != DBNull.Value
                                     ? Convert.ToDateTime(row["FECHA_PROCESO"])
                                     : DateTime.MinValue,

                                                        Usuario = columns.Contains("USUARIO") ? row["USUARIO"]?.ToString() ?? string.Empty : string.Empty,
                                                        Sap = columns.Contains("SAP") ? row["SAP"]?.ToString() ?? string.Empty : string.Empty,
                                                        Fila = columns.Contains("FILA") && row["FILA"] != DBNull.Value
                                     ? Convert.ToInt32(row["FILA"])
                                     : -1,

                                    Columna = columns.Contains("COLUMNA") ? row["COLUMNA"]?.ToString() ?? string.Empty : string.Empty,
                                    ValorIncorrecto = columns.Contains("VALOR_INCORRECTO") ? row["VALOR_INCORRECTO"]?.ToString() ?? string.Empty : string.Empty,
                                    DescripcionError = columns.Contains("DESCRIPCION_ERROR") ? row["DESCRIPCION_ERROR"]?.ToString() ?? string.Empty : string.Empty
                                };

                                modelList.Add(model);
                            }

                            modelView.Errores = modelList;
                            ViewBag.Errores = modelList;

                            return View(modelView);
                        }
                    }

                    // Validación de errores de imagen
                    if (TempData["ErrorImageUploads"] is List<VitroSql.TempProducto> imageErrors && imageErrors.Count > 0)
                    {
                        modelView.TempProductos = imageErrors;
                        return View(modelView);
                    }

                    // Si no hay errores, redirige
                    return RedirectToAction("Upload", new { State = "Upload" });
                }

                // Estado distinto a "Fails"
                return View();
            }
            catch (Exception ex)
            {
                // Trazabilidad del error
                return new HttpStatusCodeResult(500, $"Error interno: {ex.Message}");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Upload(Models.UploadViewModel model)
        {
            db.TemporalProductos.RemoveRange(db.TemporalProductos.ToArray());
            model.Recursos = "C:\\imagenes_catalogo";

            ServerUploadsFolder();
            string fileName = Path.GetFileName(model.File.FileName);

            if (fileName != "plantilla_cargue_inicial.xlsx"
                && fileName != "plantilla_cargue_actualizar.xlsx")
            {
                TempData["ErrorMensaje"] = "ERROR: Este Nombre de archivo no es conocido";
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
            if (model.Actualizar)
            {
                if (!model.File.FileName.Contains("plantilla_cargue_actualizar.xlsx"))
                {
                    TempData["ErrorMensaje"] = $"ERROR: La plantilla seleccionada {model.File.FileName} no es válida para actualizaciones. Debe usar plantilla_cargue_actualizar.xlsx";
                    return RedirectToAction("Upload", new { State = "Fails" });
                }
            }
            else
            {
                if (!model.File.FileName.Contains("plantilla_cargue_inicial.xlsx"))
                {
                    TempData["ErrorMensaje"] = $"ERROR: La plantilla seleccionada {model.File.FileName} no es válida para creación de productos. Debe usar plantilla_cargue_inicial.xlsx";
                    return RedirectToAction("Upload", new { State = "Fails" });
                }
            }
          

            DataTable table = new VitroCore.ExcelManager().ReadFile(model.File.InputStream);
            ProcessProductRepository _processProductRepository = new ProcessProductRepository();

            if (table.Rows.Count > 1000)
            {
                ModelState.AddModelError("File", "El archivo indicado supera los 1000 registros máximos para procesar");
            }

            int filaError;
            if (ContieneImagenesPng(table, 26, out filaError))
            {
                TempData["ErrorMensaje"] = $"Se detectó al menos una imagen con extensión .png en la fila {filaError + 1}. Solo se permiten archivos .jpg";
                return RedirectToAction("Upload", new { State = "Fails" });
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            List<VitroSql.TempProducto> reg_errors = new List<VitroSql.TempProducto>();
            //DataTable Errores = new DataTable();
            ProcessResult Errores = new ProcessResult();
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

            TempData["ProccessDataError"] = Errores.Errores;
            TempData["ProccessRowsCount"] = table.Rows.Count;
            TempData["ErrorImageUploads"] = reg_errors;
            TempData["ProccessSuccessCount"] = model.Actualizar
              ? Errores.RowsUpdated
              : Errores.RowsInserted;
            TempData["ProccessFailsCount"] = Errores.ErrorsCount; //db.TemporalProductos.Count(x => !x.Valido);

            if (Errores.ErrorsCount > 0)
            {
                var registros = new List<TbLogErroresCarga>();
                foreach (DataRow row in Errores.Errores.Rows)
                {
                    registros.Add(new TbLogErroresCarga
                    {
                        FECHA_PROCESO = DateTime.Now,
                        USUARIO = user.FullName,
                        SAP = row.Table.Columns.Contains("SAP") ? row["SAP"]?.ToString() : null,
                        FILA = row.Table.Columns.Contains("FILA") ? Convert.ToInt32(row["FILA"]) : 0,
                        COLUMNA = row.Table.Columns.Contains("COLUMNA") ? row["COLUMNA"]?.ToString() : null,
                        VALOR_INCORRECTO = row.Table.Columns.Contains("VALOR_INCORRECTO") ? row["VALOR_INCORRECTO"]?.ToString() : null,
                        DESCRIPCION_ERROR = row.Table.Columns.Contains("DESCRIPCION_ERROR") ? row["DESCRIPCION_ERROR"]?.ToString() : null
                    });
                }

                if (registros.Any())
                {
                    db.TbLogErroresCarga.AddRange(registros);
                    db.SaveChanges();
                }
                return RedirectToAction("Upload", new { State = "Fails" });
            }

            // No hay errores, continuar con el procesamiento de imágenes
            InsertorUpdateMaxImage(productImages, model.Actualizar, model.File.FileName, table, Errores.Errores, user.FullName);
            return RedirectToAction("Upload", new { State = "Upload" });
        }

        private int InsertorUpdateMaxImage(List<ProductImages> list, bool update, string fileName, DataTable table, DataTable errors, string user)
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

          var result = db.SaveChanges();
          TempData["ErrorImageUploadsCount"] = errorImageUploadsCount;
          return result;
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


        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult DownloadFileErrors()
        {
            var errores = db.TbLogErroresCarga
                     .OrderBy(e => e.FILA)
                     .ToList();

            if (errores == null || !errores.Any())
            {
                TempData["MensajeError"] = "No hay errores registrados para exportar.";
                return RedirectToAction("Upload");
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("LogErrores");

                // Encabezados
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Fecha Proceso";
                worksheet.Cell(1, 3).Value = "Usuario";
                worksheet.Cell(1, 4).Value = "SAP";
                worksheet.Cell(1, 5).Value = "Fila";
                worksheet.Cell(1, 6).Value = "Columna";
                worksheet.Cell(1, 7).Value = "Valor Incorrecto";
                worksheet.Cell(1, 8).Value = "Descripción Error";

                // Formato encabezados
                worksheet.Range("A1:H1").Style.Font.Bold = true;
                worksheet.Range("A1:H1").Style.Fill.BackgroundColor = XLColor.LightGray;

                int row = 2;
                foreach (var error in errores)
                {
                    worksheet.Cell(row, 1).Value = error.ID;
                    worksheet.Cell(row, 2).Value = error.FECHA_PROCESO.ToString("yyyy-MM-dd HH:mm:ss");
                    worksheet.Cell(row, 3).Value = error.USUARIO;
                    worksheet.Cell(row, 4).Value = error.SAP;
                    worksheet.Cell(row, 5).Value = error.FILA;
                    worksheet.Cell(row, 6).Value = error.COLUMNA;
                    worksheet.Cell(row, 7).Value = error.VALOR_INCORRECTO;
                    worksheet.Cell(row, 8).Value = error.DESCRIPCION_ERROR;
                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var fileName = $"LogErrores_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }


        /// <summary>
        /// Valida las extensiones de las columnas de imagenes de un DataTable.
        /// Solo permite las extensiones definidas en allowedExtensions (por defecto ".jpg").
        /// </summary>
        /// <param name="table">DataTable con los datos del Excel.</param>
        /// <param name="columnaInicial">Índice de la primera columna de imágenes (ej. 28).</param>
        /// <param name="filasConErrores">Lista de índices de fila que contienen extensiones no permitidas.</param>
        /// <returns>True si se encontraron filas con extensiones no permitidas; false en caso contrario.</returns>
        private bool ContieneImagenesPng(DataTable table, int columnaInicial, out int filaError)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (columnaInicial < 0 || columnaInicial >= table.Columns.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(columnaInicial));
            }

            filaError = -1;

            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow fila = table.Rows[i];

                for (int j = columnaInicial; j < table.Columns.Count; j++)
                {
                    object celdaObj = fila[j];
                    if (celdaObj == null)
                    {
                        continue;
                    }

                    string celda = celdaObj.ToString();
                    if (string.IsNullOrWhiteSpace(celda))
                    {
                        continue;
                    }

                    // Normalizar posibles espacios no-break y cortar cadenas largas
                    string[] tokens = celda
                        .Replace("\u00A0", " ")
                        .Trim()
                        .Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string token in tokens)
                    {
                        string limpio = token.Trim('"', '\'', '(', ')', '[', ']');

                        int indiceQuery = limpio.IndexOfAny(new char[] { '?', '#', '&' });
                        if (indiceQuery >= 0)
                        {
                            limpio = limpio.Substring(0, indiceQuery);
                        }

                        string extension = Path.GetExtension(limpio).ToLowerInvariant();

                        if (extension == ".png")
                        {
                            filaError = i; // guarda la fila donde ocurrió
                            return true;   // detenemos inmediatamente
                        }
                    }
                }
            }

            return false;
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