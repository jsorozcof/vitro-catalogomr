using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using VitroSql;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DownloadExcelReport()
        {
            var products = GetDataProductReportToExcel();
            var fileBytes = GenerateExcelReport(products);
            string fileName = $"ReporteMaestro_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";

            var cookie = new HttpCookie("excelDownload", "true")
            {
                Expires = DateTime.Now.AddMinutes(5),
                Path = "/"
            };
            Response.Cookies.Add(cookie);

            return File(fileBytes,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public List<ExportFullProductReportDto> GetDataProductReportToExcel()
        {
            // Obtener todos los productos
            var products = db.TbProduct.ToList();

            // Obtener todas las imágenes (una sola consulta)
            var allImages = db.ProductImages.ToList()
                .GroupBy(img => img.ProductId)
                .ToDictionary(g => g.Key, g => g.ToList());

            //var products = db.TbProduct.ToList();
            //var productImages = db.ProductImages.ToList();

            var marcas = db.Marcas.ToDictionary(x => x.MarcaId, x => x.Nombre);
            var modelos = db.Modelos.ToDictionary(x => x.ModeloId, x => x.Nombre);
            var mercados = db.Mercados.ToDictionary(x => x.MercadoId, x => x.Nombre);
            var colores = db.Colores.ToDictionary(x => x.ColorId, x => x.Nombre);
            var tipoVidrio = db.TipoVidrios.ToDictionary(x => x.TipoVidrioId, x => x.Nombre);
            var tipoParte = db.TipoPartes.ToDictionary(x => x.TipoParteId, x => x.Nombre);
            var tipoProcedencia = db.Procedencias.ToDictionary(x => x.ProcedenciaId, x => x.Nombre);
            var paises = db.Paises.ToDictionary(x => x.PaisId, x => x.Nombre);

            // Mapeo
            var productosDto = products.Select(p =>
            {
                var imagenes = allImages.ContainsKey(p.ProductId)
                    ? allImages[p.ProductId]
                    : new List<ProductImages>();

                // Utilidad para encontrar imagen por posición
                Func<int, string> obtenerImagen = pos =>
                    imagenes.FirstOrDefault(x => x.Posicion == pos)?.Nombre ?? "";

                return new ExportFullProductReportDto
                {
                    PAIS = paises.TryGetValue(p.PaisId, out var pais) ? pais : null,
                    SAP = p.SAP,
                    NAGS = p.NAGS,
                    Marca = marcas.TryGetValue(p.MarcaId, out var marca) ? marca : null,
                    Modelo = modelos.TryGetValue(p.ModeloId, out var modelo) ? modelo : null,
                    Descripcion = p.Descripcion,
                    Mercado = mercados.TryGetValue(p.MercadoId, out var mercado) ? mercado : null,
                    Color = colores.TryGetValue(p.ColorId, out var color) ? color : null,
                    TipoVidrio = tipoVidrio.TryGetValue(p.TipoVidrioId, out var vidrio) ? vidrio : null,
                    TipoParte = tipoParte.TryGetValue(p.TipoParteId, out var parte) ? parte : null,
                    Ancho = p.Ancho,
                    Alto = p.Alto,
                    Clasificacion = p.Clasificacion,
                    Procedencia = tipoProcedencia.TryGetValue(p.ProcedenciaId, out var procedencia) ? procedencia : null,
                    FechaCreacion = p.FechaCreacion,
                    CreadoPor = p.CreadoPor,
                    StartYear = p.StartYear,
                    EndYear = p.EndYear,
                    BotonBit = p.Boton,
                    RedBit = p.Red,
                    SerigrafiaBit = p.Serigrafia,
                    SensorLluviaBit = p.SensorLluvia,
                    MolduraBit = p.Moldura,
                    HolderBit = p.Holder,
                    SensorCondensacionBit = p.SensorCondensacion,
                    HomologoBit = p.Homologo,
                    AntenaBit = p.Antena ?? false,
                    SubEnsambleBit = p.SubEnsamble ?? false,
                    ActivoBit = p.Activo,

                    // Imágenes por posición (1 a 10)
                    Imagen = obtenerImagen(1),
                    Imagen1 = obtenerImagen(2),
                    Imagen2 = obtenerImagen(3),
                    Imagen3 = obtenerImagen(4),
                    Imagen4 = obtenerImagen(5),
                    Imagen5 = obtenerImagen(6),
                    Imagen6 = obtenerImagen(7),
                    Imagen7 = obtenerImagen(8),
                    Imagen8 = obtenerImagen(9),
                    Imagen9 = obtenerImagen(10),
                    Imagen10 = obtenerImagen(11),
                };
            }).ToList();
      
            Debug.WriteLine($"Lista productos '{productosDto}");

            //string filename = $"FULLCATALOG__{mercado.Nombre}__{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}__{Guid.NewGuid()}.pdf";

            return productosDto;

        }

        public ActionResult ExportFullProductReportToExcel()
        {
            var productos = GetDataProductReportToExcel(); // Aquí va tu método que retorna List<ExportFullProductReportDto>

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Reporte Productos");
                var currentRow = 1;

                // Encabezados
                var headers = typeof(ExportFullProductReportDto).GetProperties().Select(p => p.Name).ToList();
                for (int i = 0; i < headers.Count; i++)
                {
                    worksheet.Cell(currentRow, i + 1).Value = headers[i];
                    worksheet.Cell(currentRow, i + 1).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                }

                // Datos
                foreach (var item in productos)
                {
                    currentRow++;
                    for (int i = 0; i < headers.Count; i++)
                    {
                        var prop = item.GetType().GetProperty(headers[i]);
                        var value = prop.GetValue(item);

                        if (value is bool boolValue)
                            worksheet.Cell(currentRow, i + 1).Value = boolValue ? "SI" : "NO";
                        else if (value is DateTime dateValue)
                            worksheet.Cell(currentRow, i + 1).Value = dateValue.ToString("yyyy-MM-dd");
                        else
                            worksheet.Cell(currentRow, i + 1).Value = value?.ToString() ?? "";
                    }
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    return File(stream.ToArray(),
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "ReporteProductos.xlsx");
                }
            }

            //var productos = GetDataProductReportToExcel(); // Lógica que obtienes de la base de datos
            //using (var package = ExportFullProductReportToExcel(productos)) // Aquí usas tu helper
            //{
            //    var stream = new MemoryStream();
            //    package.SaveAs(stream);
            //    stream.Position = 0;
            //    string fileName = $"ReporteMaestro_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
            //    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            //}
        }

        public byte[] GenerateExcelReport(List<ExportFullProductReportDto> products)
        {
            List<string> columnHeaders = new List<string>
            {
                "PAIS", "SAP","NAGS", "MARCA","MODELO","AÑO INICIAL","AÑO FINAL", "DESCRIPCION","TIPO PARTE","PERFORACION",
                "ANCHO", "ALTO","BOTON","RED","SERIGRAFIA", "SENSOR LLUVIA","MODULURA","HOLDER","ANTENA","SUBENSAMBLE","SENSOR CONDENSACION",
                "COLOR","TIPO VIDRIO","PROCEDENCIA","HOMOLOGO","CLASIFICACION", "MERCADO", "FECHA CREACION", "CREADO POR",
                "IMAGEN", "IMAGEN1", "IMAGEN2","IMAGEN3","IMAGEN4","IMAGEN5","IMAGEN6","IMAGEN7", "IMAGEN8","IMAGEN9","IMAGEN10"
            };


            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Reporte Productos");

                // Aplicar estilos a la fila de encabezado
                var headerRow = worksheet.Row(1);
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.Yellow;
                headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;


                // Agregar encabezados personalizados
                for (int i = 0; i < columnHeaders.Count; i++)
                {
                    worksheet.Cell(1, i + 1).Value = columnHeaders[i];
                }

                // Agregar datos de productos
                int row = 2;
                foreach (var product in products)
                {
                    worksheet.Cell(row, 1).Value = product.PAIS;
                    worksheet.Cell(row, 2).Value = product.SAP;
                    worksheet.Cell(row, 3).Value = product.NAGS;
                    worksheet.Cell(row, 4).Value = product.Marca;
                    worksheet.Cell(row, 5).Value = product.Modelo;
                    worksheet.Cell(row, 6).Value = product.StartYear;
                    worksheet.Cell(row, 7).Value = product.EndYear;
                    worksheet.Cell(row, 8).Value = product.Descripcion;
                    worksheet.Cell(row, 9).Value = product.TipoParte;
                    worksheet.Cell(row, 10).Value = product.Perforacion;
                    worksheet.Cell(row, 11).Value = product.Ancho;
                    worksheet.Cell(row, 12).Value = product.Alto;
                    worksheet.Cell(row, 13).Value = product.Boton;
                    worksheet.Cell(row, 14).Value = product.Red;
                    worksheet.Cell(row, 15).Value = product.Serigrafia;
                    worksheet.Cell(row, 16).Value = product.SensorLluvia;
                    worksheet.Cell(row, 17).Value = product.Moldura;
                    worksheet.Cell(row, 18).Value = product.Holder;
                    worksheet.Cell(row, 19).Value = product.Antena;
                    worksheet.Cell(row, 20).Value = product.SubEnsamble;
                    worksheet.Cell(row, 21).Value = product.SensorCondensacion;
                    worksheet.Cell(row, 22).Value = product.Color;
                    worksheet.Cell(row, 23).Value = product.TipoVidrio;
                    worksheet.Cell(row, 24).Value = product.Procedencia;
                    worksheet.Cell(row, 25).Value = product.Homologo;
                    worksheet.Cell(row, 26).Value = product.Clasificacion;
                    worksheet.Cell(row, 27).Value = product.Mercado;
                    worksheet.Cell(row, 28).Value = product.FechaCreacion;
                    worksheet.Cell(row, 29).Value = product.CreadoPor;
                    worksheet.Cell(row, 30).Value = product.Imagen;
                    worksheet.Cell(row, 31).Value = product.Imagen1;
                    worksheet.Cell(row, 32).Value = product.Imagen2;
                    worksheet.Cell(row, 33).Value = product.Imagen3;
                    worksheet.Cell(row, 34).Value = product.Imagen4;
                    worksheet.Cell(row, 35).Value = product.Imagen5;
                    worksheet.Cell(row, 36).Value = product.Imagen6;
                    worksheet.Cell(row, 37).Value = product.Imagen7;
                    worksheet.Cell(row, 38).Value = product.Imagen8;
                    worksheet.Cell(row, 39).Value = product.Imagen9;
                    worksheet.Cell(row, 40).Value = product.Imagen10;

                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }

            }

        }

        private static string GetNombre(Dictionary<string, string> dic, string key)
           => key != null && dic.ContainsKey(key) ? dic[key] : null;
        private void ServerFilesFolder()
        {
            if (!Directory.Exists(Server.MapPath("~/Resources/Files")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Resources/Files"));
            }
        }
    }
}