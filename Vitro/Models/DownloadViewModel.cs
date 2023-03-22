using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vitro.Models
{
    public class DownloadViewModel
    {
        public string IndexMessage { get; set; }
        public string Mercado { get; set; }
        public IEnumerable<VitroSql.Mercado> Mercados { get; set; }
        public int TotalProductos { get; set; }
    }
}