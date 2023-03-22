using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Drawing;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace Vitro.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class OfflineController : Controller
    {
        private readonly Models.ApplicationDbContext db = new Models.ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create()
        {
            var productos = db.Productos.Include(x => x.TipoParte.Clasificacion).Include(x => x.Color).Include(x => x.Modelo.Marca).Include(x => x.Mercado).Include(x => x.Modelo).Include(x => x.Procedencia).Include(x => x.TipoParte).Include(x => x.TipoVidrio).Where(x => x.Activo).ToArray();
            var users = db.Users.Include(x => x.Pais).Where(x => !x.Disable).ToArray();
            var images = db.ProductoImagenes.Include(x => x.Imagen).ToArray();

            List<VitroCore.AccountExportModel> accounts = new List<VitroCore.AccountExportModel>();
            foreach (var user in users)
            {
                var account = new VitroCore.AccountExportModel()
                {
                    UserName = user.UserName,
                    FingerPrint = user.FingerPrint,
                    Pais = user.Pais != null ? user.Pais.Nombre : string.Empty
                };
                accounts.Add(account);
            }

            List<VitroCore.ProdExportModel> products = new List<VitroCore.ProdExportModel>();
            foreach (var product in productos)
            {
                var producto = new VitroCore.ProdExportModel()
                {
                    SAP = product.SAP,
                    NAGS = product.NAGS,
                    Alto = product.Alto.ToString(),
                    Ancho = product.Ancho.ToString(),
                    Boton = product.Boton,
                    Clasificacion = product.TipoParte.Clasificacion.Nombre,
                    Color = product.Color.Nombre,
                    Descripcion = product.Descripcion,
                    EndYear = product.EndYear.ToString(),
                    Holder = product.Holder,
                    Homologo = product.Homologo,
                    Marca = product.Modelo.Marca.Nombre,
                    Mercado = product.Mercado.Nombre,
                    Modelo = product.Modelo.Nombre,
                    Moldura = product.Moldura,
                    Perforacion = product.Perforacion.ToString(),
                    Procedencia = product.Procedencia.Nombre,
                    Red = product.Red,
                    SensorCondensacion = product.SensorCondensacion,
                    SensorLluvia = product.SensorLluvia,
                    Serigrafia = product.Serigrafia,
                    StartYear = product.StartYear.ToString(),
                    TipoParte = product.TipoParte.Nombre,
                    TipoVidrio = product.TipoVidrio.Nombre,
                };
                using (Image bitmap = Bitmap.FromFile(Path.Combine(Server.MapPath("~/Resources/Uploads/"), images.Where(x => x.ProductoId.Equals(product.ProductoId)).FirstOrDefault().Imagen.Nombre)))
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        Image.GetThumbnailImageAbort abort = new Image.GetThumbnailImageAbort(ThumbCallback);
                        using (Image thumb = bitmap.GetThumbnailImage(150, 150, abort, new IntPtr()))
                        {
                            thumb.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                            producto.Imagen = stream.ToArray();
                        }
                    }
                }
                products.Add(producto);
            }

            List<VitroCore.PaisExportModel> paises = new List<VitroCore.PaisExportModel>();
            var dbpaises = db.Paises.Where(x => x.Activo).ToArray();
            foreach (var pais in dbpaises)
            {
                paises.Add(new VitroCore.PaisExportModel()
                {
                    PaisId = pais.PaisId,
                    Nombre = pais.Nombre
                });
            }

            List<VitroCore.MarcaExportModel> marcas = new List<VitroCore.MarcaExportModel>();
            var dbmarcas = db.Marcas.Include(x => x.Pais).Where(x => x.Activo).ToArray();
            foreach (var marca in dbmarcas)
            {
                marcas.Add(new VitroCore.MarcaExportModel()
                {
                    MarcaId = marca.MarcaId,
                    Nombre = marca.Nombre,
                    PaisId = marca.PaisId
                });
            }

            List<VitroCore.ModeloExportModel> modelos = new List<VitroCore.ModeloExportModel>();
            var dbmodelos = db.Modelos.Where(x => x.Activo).ToArray();
            foreach (var modelo in dbmodelos)
            {
                modelos.Add(new VitroCore.ModeloExportModel()
                {
                    ModeloId = modelo.ModeloId,
                    Nombre = modelo.Nombre,
                    MarcaId = modelo.MarcaId
                });
            }

            using (VitroCore.LocalDatabase database = new VitroCore.LocalDatabase())
            {
                database.CreateDatabase(Server.MapPath("~/Resources/Offline/CATALOGOMR.db"));
                database.ClearTables();
                database.SaveAccount(accounts);
                database.SaveProductos(products);
                database.SavePais(paises);
                database.SaveMarca(marcas);
                database.SaveModelo(modelos);
            }

            try
            {
                System.IO.File.Copy(Server.MapPath("~/Resources/Offline/CATALOGOMR.db"), Server.MapPath("~/Resources/Offline/instalador/CATALOGOMR.db"), true);
                if (System.IO.File.Exists(Server.MapPath("~/Resources/Offline/instalador.zip")))
                {
                    System.IO.File.Delete(Server.MapPath("~/Resources/Offline/instalador.zip"));
                }
                System.IO.Compression.ZipFile.CreateFromDirectory(Server.MapPath("~/Resources/Offline/instalador"), Server.MapPath("~/Resources/Offline/instalador.zip"));
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine(error.Message);
            }
            return File(Server.MapPath("~/Resources/Offline/instalador.zip"), "application/zip", $"{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}-instalador-vitro.zip");
        }

        private bool ThumbCallback()
        {
            return true;
        }
    }
}