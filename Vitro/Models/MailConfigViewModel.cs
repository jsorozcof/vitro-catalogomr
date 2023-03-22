using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Vitro.Models
{
    public class MailConfigViewModel
    {
        public string MailConfigId { get; set; }
        [Required]
        public int Puerto { get; set; }
        [Required]
        public bool HabilitarSSL { get; set; }
        [Required]
        public string Host { get; set; }
        [Required]
        [EmailAddress]
        public string MailAccount { get; set; }
        [Required]
        public string MailPassword { get; set; }
    }
}