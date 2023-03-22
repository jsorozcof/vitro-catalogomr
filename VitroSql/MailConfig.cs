using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VitroSql
{
    [Table("MailConfig")]
    public class MailConfig
    {
        [Key]
        public string MailConfigId { get; set; }
        public int Puerto { get; set; }
        public bool? HabilitarSSL { get; set; }
        public string Host { get; set; }
        public string MailAccount { get; set; }
        public string MailPassword { get; set; }
    }
}
