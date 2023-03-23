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

namespace Vitro.Controllers
{
    [Authorize(Roles = "Administrador,Mercadotecnia,Ingenieria")]
    public class ProductoController : Controller
    {
        private readonly Models.ApplicationDbContext db = new Models.ApplicationDbContext();

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
                        model.Productos = db.Productos.Include(x => x.Color).Include(x => x.Modelo.Marca).Include(x => x.Mercado).Include(x => x.Modelo).Include(x => x.Modelo.Marca.Pais).Include(x => x.TipoParte).Include(x => x.TipoParte.Clasificacion).Include(x => x.Procedencia).Include(x => x.TipoVidrio).Where(x => x.SAP.Contains(model.Busqueda) && x.Activo).OrderBy(x => x.Modelo.Nombre).ThenBy(x => x.StartYear).ThenBy(x => x.TipoParte.Clasificacion.Nombre).ThenBy(x => x.TipoParte.Nombre).ToArray();
                        model.ProductoImagenes = db.ProductoImagenes.Include(x => x.Imagen).ToArray();
                    }
                    else
                    {
                        model.Productos = db.Productos.Include(x => x.Color).Include(x => x.Modelo.Marca).Include(x => x.Mercado).Include(x => x.Modelo).Include(x => x.Modelo.Marca.Pais).Include(x => x.TipoParte).Include(x => x.TipoParte.Clasificacion).Include(x => x.Procedencia).Include(x => x.TipoVidrio).Where(x => x.NAGS.Contains(model.Busqueda) && x.Activo).OrderBy(x => x.Modelo.Nombre).ThenBy(x => x.StartYear).ThenBy(x => x.TipoParte.Clasificacion.Nombre).ThenBy(x => x.TipoParte.Nombre).ToArray();
                        model.ProductoImagenes = db.ProductoImagenes.Include(x => x.Imagen).ToArray();
                    }
                    break;
                case "ProductoViewModel2":
                    int year = int.Parse(model.Year ?? "0");
                    if (year > 0)
                    {
                        model.Productos = db.Productos.Include(x => x.Color).Include(x => x.Modelo.Marca).Include(x => x.Mercado).Include(x => x.Modelo).Include(x => x.Modelo.Marca.Pais).Include(x => x.TipoParte).Include(x => x.TipoParte.Clasificacion).Include(x => x.Procedencia).Include(x => x.TipoVidrio).Where(x => x.Modelo.Marca.MarcaId.Equals(model.Marca) && x.Modelo.ModeloId.Equals(model.Modelo) && year >= x.StartYear && year <= x.EndYear && x.Activo).OrderBy(x => x.Modelo.Nombre).ThenBy(x => x.StartYear).ThenBy(x => x.TipoParte.Clasificacion.Nombre).ThenBy(x => x.TipoParte.Nombre).ToArray();
                        //model.Productos = db.Productos.Include(x => x.Modelo.Marca.Pais).Include(x => x.Modelo.Marca).Include(x => x.TipoParte).Include(x => x.Modelo).Include(x => x.TipoParte.Clasificacion).Where(x => x.Modelo.Marca.MarcaId.Equals(model.Marca) && x.Modelo.ModeloId.Equals(model.Modelo) && year >= x.StartYear && year <= x.EndYear && x.Activo).OrderBy(x => x.Modelo.Nombre).ThenBy(x => x.StartYear).ThenBy(x => x.TipoParte.Clasificacion.Nombre).ThenBy(x => x.TipoParte.Nombre).ToArray();
                        model.ProductoImagenes = db.ProductoImagenes.Include(x => x.Imagen).ToArray();
                    }
                    else
                    {
                        model.Productos = db.Productos.Include(x => x.Color).Include(x => x.Modelo.Marca).Include(x => x.Mercado).Include(x => x.Modelo).Include(x => x.Modelo.Marca.Pais).Include(x => x.TipoParte).Include(x => x.TipoParte.Clasificacion).Include(x => x.Procedencia).Include(x => x.TipoVidrio).Where(x => x.Modelo.Marca.MarcaId.Equals(model.Marca) && x.Modelo.ModeloId.Equals(model.Modelo) && x.Activo).OrderBy(x => x.Modelo.Nombre).ThenBy(x => x.StartYear).ThenBy(x => x.TipoParte.Clasificacion.Nombre).ThenBy(x => x.TipoParte.Nombre).ToArray();
                        //model.Productos = db.Productos.Include(x => x.Modelo.Marca.Pais).Include(x => x.Modelo.Marca).Include(x => x.TipoParte).Include(x => x.Modelo).Include(x => x.TipoParte.Clasificacion).Where(x => x.Modelo.Marca.MarcaId.Equals(model.Marca) && x.Modelo.ModeloId.Equals(model.Modelo) && x.Activo).OrderBy(x => x.Modelo.Nombre).ThenBy(x => x.StartYear).ThenBy(x => x.TipoParte.Clasificacion.Nombre).ThenBy(x => x.TipoParte.Nombre).ToArray();
                        model.ProductoImagenes = db.ProductoImagenes.Include(x => x.Imagen).ToArray();
                    }
                    break;
            }
            var user = db.Users.Include(x => x.Pais).Where(x => x.UserName.Equals(User.Identity.Name)).FirstOrDefault();
            model.Marcas = db.Marcas.Where(x => x.PaisId.Equals(user.PaisId ?? string.Empty) && x.Activo).OrderBy(x => x.Nombre).ToArray();
            model.TotalProductos = db.Productos.Where(x => x.Modelo.Marca.Pais.PaisId.Equals(user.PaisId) && x.Activo).Count();
            model.TotalproductIMG = (from PD in db.Productos
                                     join PI in db.ProductoImagenes on PD.ProductoId equals PI.ProductoId
                                     join IMG in db.Imagenes on PI.ImagenId equals IMG.ImagenId
                                     where IMG.Nombre.Contains("default")
                                     select new { PD.ProductoId }).Count();
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

            var producto = db.Productos.Include(x => x.Modelo.Marca).Include(x => x.Modelo).Include(x => x.TipoParte).Include(x => x.TipoVidrio).Include(x => x.TipoParte.Clasificacion).Include(x => x.Mercado).Include(x => x.Color).Include(x => x.Procedencia).Where(x => x.ProductoId.Equals(id)).FirstOrDefault();
            var viewmodel = new Models.DetailsProductoViewModel()
            {
                Producto = producto,
                ProductoImagen = db.ProductoImagenes.Include(x => x.Imagen).Where(x => x.ProductoId.Equals(producto.ProductoId)).ToArray()
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

            var producto = db.Productos.Include(x => x.Modelo.Marca).Include(x => x.Modelo).Include(x => x.TipoParte).Include(x => x.TipoVidrio).Include(x => x.TipoParte.Clasificacion).Include(x => x.Mercado).Include(x => x.Color).Include(x => x.Procedencia).Where(x => x.ProductoId.Equals(id)).FirstOrDefault();
            var viewmodel = new Models.DetailsProductoViewModel()
            {
                Producto = producto,
                ProductoImagen = db.ProductoImagenes.Include(x => x.Imagen).Where(x => x.ProductoId.Equals(producto.ProductoId)).ToArray()
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
        public ActionResult Upload(Models.UploadViewModel model)
        {
            db.TemporalProductos.RemoveRange(db.TemporalProductos.ToArray());
            ServerUploadsFolder();
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

            if (table.Rows.Count > 1000)
            {
                ModelState.AddModelError("File", "El archivo indicado supera los 1000 registros máximos para procesar");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            List<VitroSql.TempProducto> reg_errors = new List<VitroSql.TempProducto>();
            List<string> imagenes = new List<string>();
            var listPrductoImagen = new List<MassiveProductImages>();

            if (table.Rows.Count <= 1000)
            {
                var user = db.Users.Include(x => x.Pais).Where(x => x.UserName.Equals(User.Identity.Name)).FirstOrDefault();
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

                        if (column.ColumnName.ToLower().StartsWith("imagen"))
                        {
                            if (Directory.Exists(model.Recursos))
                            {
                                try
                                {
                                    string root = Path.GetFullPath(model.Recursos);
                                    var filename = string.Empty;
                                    string extension = string.Empty;
                                    Image image = Image.FromFile(Path.Combine(root, rows[column].ToString()));

                                    if (image != null)
                                    {
                                        filename = $"{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}__{rows[column].ToString()}";
                                        image.Save(Path.Combine(Server.MapPath("~/Resources/Uploads/"), filename));
                                        temp.RefImagen = filename;
                                        extension = Path.GetExtension(filename);
                                        imagenes.Add(filename);
                                    }

                                    string DefaultImagePath = Server.MapPath("~/Resources/Uploads/") + filename;
                                    byte[] imageArray = System.IO.File.ReadAllBytes(DefaultImagePath);
                                    string base64ImageRepresentation = Convert.ToBase64String(imageArray);


                                    byte[] Imgbytes = imageArray;
                                    string ImagenId = $"{Guid.NewGuid()}";


                                    if (model.Actualizar)
                                    {
                                        var listMassiveSearch = db.MassiveProductImages.Where(m => m.Sap == temp.SAP).ToList();
                                        if (listMassiveSearch != null)
                                        {
                                            foreach (var detail in listMassiveSearch)
                                            {
                                                db.MassiveProductImages.Remove(detail);
                                                db.SaveChanges();
                                            }

                                            listPrductoImagen.Add(new VitroSql.MassiveProductImages()
                                            {
                                                Nombre = filename,
                                                Sap = temp.SAP,
                                                Extension = extension,
                                                Contenido = Imgbytes,
                                                Posicion = IndiceImagen,
                                                ProductoId = "",
                                                ImagenId = ImagenId
                                            });
                                        }
                                        else
                                            System.Diagnostics.Debug.WriteLine($"ERROR: El producto {temp.SAP} no ha sido creado");
                                        
                                       
                                    }
                                    else
                                    {
                                        //List add
                                        listPrductoImagen.Add(new VitroSql.MassiveProductImages()
                                        {
                                            Nombre = filename,
                                            Sap = temp.SAP,
                                            Extension = extension,
                                            Contenido = Imgbytes,
                                            Posicion = IndiceImagen,
                                            ProductoId = "",
                                            ImagenId = ImagenId
                                        });
                                    }

                                    IndiceImagen++;
                                }
                                catch (Exception error)
                                {
                                    string root = Path.GetFullPath(model.Recursos);
                                    var filename = string.IsNullOrEmpty(rows[column].ToString()) ? $"{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}__{rows[0].ToString()}" : $"{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}__{rows[column].ToString()}";
                                    Image image = Image.FromFile(Path.Combine(root, "default.jpg"));
                                    image.Save(Path.Combine(Server.MapPath("~/Resources/Uploads/"), filename));
                                    temp.RefImagen = filename;
                                    imagenes.Add(filename);
                                    System.Diagnostics.Debug.WriteLine($"ERROR: {error.Message}");
                                }
                            }
                        }
                    }
                    if (temp.RefImagen == null)
                    {
                        reg_errors.Add(temp);
                    }
                    db.TemporalProductos.Add(temp);
                }
                db.SaveChanges();
                db.Database.ExecuteSqlCommand("exec sp_cargue @PAIS, @USERNAME, @ACTUALIZAPRODUCTOS", new SqlParameter("@PAIS", "COLOMBIA"), new SqlParameter("@USERNAME", User.Identity.Name), new SqlParameter("@ACTUALIZAPRODUCTOS", model.Actualizar));
                db.Database.CommandTimeout = 300;
            }

            InsertorUpdateMaxImage(listPrductoImagen,model.Actualizar);

            TempData["ProccessRowsCount"] = table.Rows.Count;
            TempData["ErrorImageUploadsCount"] = reg_errors.Count;
            TempData["ErrorImageUploads"] = reg_errors;
            TempData["ProccessSuccessCount"] = db.TemporalProductos.Count(x => x.Valido);
            TempData["ProccessFailsCount"] = db.TemporalProductos.Count(x => !x.Valido);
            return reg_errors.Count > 0 ? RedirectToAction("Upload", new { State = "Fails" }) : RedirectToAction("Upload", new { State = "Upload" });
        }

        public void InsertorUpdateMaxImage(List<MassiveProductImages> list, bool update)
        {
            try
            {
                VitroSql.MassiveProductImages massiveProductImages = new VitroSql.MassiveProductImages();
                List<VitroSql.MassiveProductImages> listMassiveProductImages = new List<VitroSql.MassiveProductImages>();

                if(update)
                  massiveProductImages.FechaActualizacion = DateTime.UtcNow;

                foreach (var item in list)
                {
                    var producto = db.Productos.Where(x => x.SAP == item.Sap).FirstOrDefault();
                    massiveProductImages.Nombre = item.Nombre;
                    massiveProductImages.Sap = item.Sap;
                    massiveProductImages.Extension = item.Extension;
                    massiveProductImages.Contenido = item.Contenido;
                    massiveProductImages.Posicion = item.Posicion;
                    massiveProductImages.ProductoId = producto.ProductoId;
                    massiveProductImages.ImagenId = item.ImagenId;

                    db.MassiveProductImages.Add(massiveProductImages);
                    db.SaveChanges();
                }
               
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR: {error.Message}");
            }
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