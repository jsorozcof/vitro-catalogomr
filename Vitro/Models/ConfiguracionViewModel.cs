using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vitro.Models
{
    public class ConfiguracionViewModel
    {
        public string ConfiguracionId { get; set; }
        public int DiasVigenciaNuevosProductos { get; set; }

        public VitroSql.MailConfig MailConfig { get; set; }
    }
}