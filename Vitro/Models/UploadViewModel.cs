using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Vitro.Models
{
    public class UploadViewModel
    {
        [Required(ErrorMessage = "El archivo para la carga de productos es obligatorio.")]
        public HttpPostedFileBase File { get; set; }
        [Display(Name ="Carpeta de Recursos")]
        [Required(ErrorMessage ="Carpeta de Recursos es requerida")]
        public string Recursos { get; set; }
        public bool Actualizar { get; set; }

        public List<VitroSql.TempProducto> TempProductos { get; set; }
    }
}