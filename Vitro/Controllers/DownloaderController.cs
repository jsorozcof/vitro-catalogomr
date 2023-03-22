using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Vitro.Controllers
{
    [Authorize]
    public class DownloaderController : Controller
    {
        private readonly Models.ApplicationDbContext db = new Models.ApplicationDbContext();

        // GET: Downloader
        public ActionResult Index()
        {
            var viewmodel = new Models.DownloadViewModel()
            {
                Mercados = db.Mercados.OrderBy(x => x.Nombre).ToList()
            };
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection collection)
        {
            string referencia = collection["_SRC"];
            string filename = $"{Guid.NewGuid()}.pdf";

            ServerFilesFolder();

            byte[] portada = System.IO.File.ReadAllBytes(Server.MapPath("~/Resources/Images/vitro-portada.jpg"));
            byte[] membrete = System.IO.File.ReadAllBytes(Server.MapPath("~/Resources/Images/vitro-membrete.jpg"));
            byte[] watermark = System.IO.File.ReadAllBytes(Server.MapPath("~/Resources/Images/vitro-ma.png"));
            iTextSharp.text.pdf.BaseFont font = iTextSharp.text.pdf.BaseFont.CreateFont(Server.MapPath("~/Fonts/Lato-Regular.ttf"), iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.EMBEDDED);

            using (VitroCore.PdfManager pdf = new VitroCore.PdfManager() { Portada = portada, Membrete = membrete, WaterMark = watermark, DocumentBaseFont = font })
            {
                pdf.CreatePDFFile(Server.MapPath("~/Resources/Files/" + filename));

                pdf.CrearPortada();
                pdf.CrearMembrete();
                pdf.CrearMarcaAgua();

                var producto = db.Productos.Include(x => x.Modelo.Marca).Include(x => x.Modelo).Include(x => x.TipoParte).Include(x => x.TipoVidrio).Include(x => x.TipoParte.Clasificacion).Include(x => x.Mercado).Include(x => x.Color).Include(x => x.Procedencia).Where(x => x.ProductoId.Equals(referencia)).FirstOrDefault();
                var image = db.ProductoImagenes.Include(x => x.Imagen).Where(x => x.ProductoId.Equals(producto.ProductoId)).FirstOrDefault();

                DataTable tablaEncabezado = new DataTable();
                DataTable tablaDetalle = new DataTable();

                tablaEncabezado.Columns.Add("Marca", typeof(string));
                tablaEncabezado.Columns.Add("Modelo", typeof(string));
                tablaEncabezado.Columns.Add("Año Inicial", typeof(int));
                tablaEncabezado.Columns.Add("Año Final", typeof(int));

                tablaDetalle.Columns.Add("Clasif", typeof(string));
                tablaDetalle.Columns.Add("Código SAP", typeof(string));
                tablaDetalle.Columns.Add("NAGS", typeof(string));
                tablaDetalle.Columns.Add("Tipo Parte", typeof(string));
                tablaDetalle.Columns.Add("Perf", typeof(double));
                tablaDetalle.Columns.Add("Ancho", typeof(double));
                tablaDetalle.Columns.Add("Alto", typeof(double));
                tablaDetalle.Columns.Add("Botón", typeof(bool));
                tablaDetalle.Columns.Add("Red", typeof(bool));
                tablaDetalle.Columns.Add("Serig", typeof(bool));
                tablaDetalle.Columns.Add("SenLl", typeof(bool));
                tablaDetalle.Columns.Add("Mold", typeof(bool));
                tablaDetalle.Columns.Add("Hold", typeof(bool));
                tablaDetalle.Columns.Add("SenCon", typeof(bool));
                tablaDetalle.Columns.Add("Ant", typeof(bool));
                tablaDetalle.Columns.Add("Color", typeof(string));
                tablaDetalle.Columns.Add("TipoV", typeof(string));
                tablaDetalle.Columns.Add("Hom", typeof(bool));

                DataRow rowEncabezado = tablaEncabezado.NewRow();
                rowEncabezado[0] = producto.Modelo.Marca.Nombre;
                rowEncabezado[1] = producto.Modelo.Nombre;
                rowEncabezado[2] = producto.StartYear;
                rowEncabezado[3] = producto.EndYear;
                tablaEncabezado.Rows.Add(rowEncabezado);

                DataRow rowDetalle = tablaDetalle.NewRow();
                rowDetalle[0] = producto.TipoParte.Clasificacion.Nombre;
                rowDetalle[1] = producto.SAP;
                rowDetalle[2] = producto.NAGS;
                rowDetalle[3] = producto.TipoParte.Nombre;
                rowDetalle[4] = producto.Perforacion;
                rowDetalle[5] = producto.Ancho;
                rowDetalle[6] = producto.Alto;
                rowDetalle[7] = producto.Boton;
                rowDetalle[8] = producto.Red;
                rowDetalle[9] = producto.Serigrafia;
                rowDetalle[10] = producto.SensorLluvia;
                rowDetalle[11] = producto.Moldura;
                rowDetalle[12] = producto.Holder;
                rowDetalle[13] = producto.SensorCondensacion;
                rowDetalle[14] = producto.Antena;
                rowDetalle[15] = producto.Color.Nombre;
                rowDetalle[16] = producto.TipoVidrio.Nombre;
                rowDetalle[17] = producto.Homologo;
                tablaDetalle.Rows.Add(rowDetalle);

                byte[] imagecontent = System.IO.File.ReadAllBytes(Server.MapPath($"~/Resources/Uploads/{image.Imagen.Nombre}"));
                pdf.CrearTablaAnidada(tablaEncabezado, tablaDetalle, imagecontent);
            }

            return File(Server.MapPath("~/Resources/Files/" + filename), "application/pdf", "vitro_catalogo.pdf");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FullCatalog(FormCollection collection)
        {
            string segmento = collection["Mercado"];
            var mercado = db.Mercados.Where(x => x.MercadoId.Equals(segmento)).FirstOrDefault();
            string filename = $"FULLCATALOG__{mercado.Nombre}__{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}__{Guid.NewGuid()}.pdf";

            ServerFilesFolder();

            byte[] portada = System.IO.File.ReadAllBytes(Server.MapPath("~/Resources/Images/vitro-portada.jpg"));
            byte[] contraportada = System.IO.File.ReadAllBytes(Server.MapPath("~/Resources/Images/vitro-contraportada.jpg"));
            byte[] membrete = System.IO.File.ReadAllBytes(Server.MapPath("~/Resources/Images/vitro-membrete.jpg"));
            byte[] watermark = System.IO.File.ReadAllBytes(Server.MapPath("~/Resources/Images/vitro-ma.png"));
            iTextSharp.text.pdf.BaseFont font = iTextSharp.text.pdf.BaseFont.CreateFont(Server.MapPath("~/Fonts/Lato-Regular.ttf"), iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.EMBEDDED);

            using (VitroCore.PdfManager pdf = new VitroCore.PdfManager() { Portada = portada, Contraportada = contraportada, Membrete = membrete, WaterMark = watermark, DocumentBaseFont = font })
            {
                pdf.CreatePDFFile(Server.MapPath("~/Resources/Files/" + filename));
                pdf.CrearPortada();
                pdf.CrearMembrete();
                pdf.CrearMarcaAgua();

                var usuario = db.Users.Include(x => x.Pais).Where(x => x.UserName.Equals(User.Identity.Name)).FirstOrDefault();
                var productos = db.Productos.Include(x => x.Modelo.Marca).Include(x => x.Modelo).Include(x => x.TipoParte).Include(x => x.TipoVidrio).Include(x => x.TipoParte.Clasificacion).Include(x => x.Mercado).Include(x => x.Color).Include(x => x.Procedencia).Where(x => x.Activo && x.MercadoId.Equals(segmento))
                    .OrderBy(x => x.Mercado.Nombre).ThenBy(x => x.Modelo.Marca.Nombre).ThenBy(x => x.Modelo.Nombre).ThenBy(x => x.StartYear).ThenBy(x => x.EndYear).ToArray();
                var imagenes = db.ProductoImagenes.Include(x => x.Imagen).ToArray();
                var filter = productos.Select(x => new { Marca = x.Modelo.Marca.Nombre, Modelo = x.Modelo.Nombre, StartYear = x.StartYear, EndYear = x.EndYear }).Distinct();

                List<VitroCore.PdfDataModel> datamodel = new List<VitroCore.PdfDataModel>();
                foreach (var prod in filter)
                {
                    DataTable tablaEncabezado = new DataTable();
                    DataTable tablaDetalle = new DataTable();

                    tablaEncabezado.Columns.Add("Marca", typeof(string));
                    tablaEncabezado.Columns.Add("Modelo", typeof(string));
                    tablaEncabezado.Columns.Add("Año Inicial", typeof(int));
                    tablaEncabezado.Columns.Add("Año Final", typeof(int));

                    tablaDetalle.Columns.Add("Clasif", typeof(string));
                    tablaDetalle.Columns.Add("Código SAP", typeof(string));
                    tablaDetalle.Columns.Add("NAGS", typeof(string));
                    tablaDetalle.Columns.Add("Tipo Parte", typeof(string));
                    tablaDetalle.Columns.Add("Perf", typeof(double));
                    tablaDetalle.Columns.Add("Ancho", typeof(double));
                    tablaDetalle.Columns.Add("Alto", typeof(double));
                    tablaDetalle.Columns.Add("Botón", typeof(bool));
                    tablaDetalle.Columns.Add("Red", typeof(bool));
                    tablaDetalle.Columns.Add("Serig", typeof(bool));
                    tablaDetalle.Columns.Add("SenLl", typeof(bool));
                    tablaDetalle.Columns.Add("Mold", typeof(bool));
                    tablaDetalle.Columns.Add("Hold", typeof(bool));
                    tablaDetalle.Columns.Add("SenCon", typeof(bool));
                    tablaDetalle.Columns.Add("Ant", typeof(bool));
                    tablaDetalle.Columns.Add("Color", typeof(string));
                    tablaDetalle.Columns.Add("TipoV", typeof(string));
                    tablaDetalle.Columns.Add("Hom", typeof(bool));

                    DataRow rowEncabezado = tablaEncabezado.NewRow();
                    rowEncabezado[0] = prod.Marca;
                    rowEncabezado[1] = prod.Modelo;
                    rowEncabezado[2] = prod.StartYear;
                    rowEncabezado[3] = prod.EndYear;
                    tablaEncabezado.Rows.Add(rowEncabezado);

                    var detalles = productos.Where(x => x.Modelo.Nombre.Equals(prod.Modelo) && x.Modelo.Marca.Nombre.Equals(prod.Marca) && x.StartYear == prod.StartYear && x.EndYear == prod.EndYear).OrderByDescending(x => x.TipoParte.Clasificacion.Nombre).ToArray();
                    foreach (var detalle in detalles)
                    {
                        DataRow rowDetalle = tablaDetalle.NewRow();
                        rowDetalle[0] = detalle.TipoParte.Clasificacion.Nombre;
                        rowDetalle[1] = detalle.SAP;
                        rowDetalle[2] = detalle.NAGS;
                        rowDetalle[3] = detalle.TipoParte.Nombre;
                        rowDetalle[4] = detalle.Perforacion;
                        rowDetalle[5] = detalle.Ancho;
                        rowDetalle[6] = detalle.Alto;
                        rowDetalle[7] = detalle.Boton;
                        rowDetalle[8] = detalle.Red;
                        rowDetalle[9] = detalle.Serigrafia;
                        rowDetalle[10] = detalle.SensorLluvia;
                        rowDetalle[11] = detalle.Moldura;
                        rowDetalle[12] = detalle.Holder;
                        rowDetalle[13] = detalle.SensorCondensacion;
                        rowDetalle[14] = detalle.Antena;
                        rowDetalle[15] = detalle.Color.Nombre;
                        rowDetalle[16] = detalle.TipoVidrio.Nombre;
                        rowDetalle[17] = detalle.Homologo;
                        tablaDetalle.Rows.Add(rowDetalle);
                    }

                    byte[] imagecontent = System.IO.File.ReadAllBytes(Server.MapPath($"~/Resources/Uploads/{imagenes.Where(x => x.ProductoId.Equals(productos.Where(y => y.Modelo.Marca.Nombre.Equals(prod.Marca) && y.Modelo.Nombre.Equals(prod.Modelo) && y.StartYear == prod.StartYear && y.EndYear == prod.EndYear).FirstOrDefault().ProductoId)).FirstOrDefault().Imagen.Nombre}"));
                    datamodel.Add(new VitroCore.PdfDataModel()
                    {
                        TablaEncabezado = tablaEncabezado,
                        TablaDetalle = tablaDetalle,
                        ImageFileName = imagecontent
                    });
                }
                pdf.CrearTablaAnidada(datamodel);
                pdf.NuevaPagina();
                pdf.CrearContraportada();
            }
            return File(Server.MapPath("~/Resources/Files/" + filename), "application/pdf", $"FULL_CATALOG_{mercado.Nombre}.pdf");
        }

        private void ServerFilesFolder()
        {
            if (!Directory.Exists(Server.MapPath("~/Resources/Files")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Resources/Files"));
            }
        }
    }
}