using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vitro.Models
{
    public class UsuarioViewModel
    {
        public ApplicationUser Usuario { get; set; }
        public string RoleName { get; set; }
    }
}