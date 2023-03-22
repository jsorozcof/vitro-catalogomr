using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Vitro.Models
{
    public class ProfileViewModel
    {
        public string UserName { get; set; }
        [EmailAddress]
        [Required]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }
        [Phone]
        [Display(Name = "Teléfono")]
        [Required]
        public string PhoneNumber { get; set; }
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
    }
}