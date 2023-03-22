using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Vitro.Models
{
    public class SugerenciaViewModel
    {
        [Required]
        public string Marca { get; set; }
        [Required]
        public string Modelo { get; set; }
        [Required]
        [Display(Name ="Tipo de Parte")]
        public string TipoParte { get; set; }
        [Required]
        [Display(Name ="Descripción")]
        public string Descripcion { get; set; }
    }
}