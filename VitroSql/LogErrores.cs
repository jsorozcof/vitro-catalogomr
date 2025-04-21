using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitroSql
{
    [Table("LogErrores")]
    public class TbLogErrores
    {
        [Key]
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public string Usuario { get; set; }
        public string Modulo { get; set; }
        public string Operacion { get; set; }
        public string Mensaje { get; set; }
        public string DetalleError { get; set; }
        public string StackTrace { get; set; }
        public string Archivo { get; set; }
        public int? Linea { get; set; }
        public string SAP { get; set; }
        public string ProductoId { get; set; } = string.Empty;
    }
}
