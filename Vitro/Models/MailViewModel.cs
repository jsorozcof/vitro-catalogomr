using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Vitro.Models
{
    public class MailViewModel
    {
        [Required]
        public string Subject { get; set; }
        public string Bcc { get; set; }
        public HttpPostedFileBase[] Files { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 50)]
        public string Message { get; set; }
    }
}