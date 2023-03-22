using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VitroSql
{
    [Table("LogUserAccount")]
    public class LogUserAccount
    {
        [Key]
        public string LogUserAccountId { get; set; }
        public DateTime FechaHora { get; set; }
        public string UserName { get; set; }
        public string IpAddress { get; set; }
    }
}
