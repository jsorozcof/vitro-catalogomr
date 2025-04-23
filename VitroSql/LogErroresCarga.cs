using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitroSql
{
    [Table("LogErroresCarga")]
    public class TbLogErroresCarga
    {
        [Key]
        public int ID { get; set; }
        public DateTime FECHA_PROCESO { get; set; }
        public string USUARIO { get; set; }
        public string SAP { get; set; }
        public int FILA { get; set; }
        public string COLUMNA { get; set; }
        public string VALOR_INCORRECTO { get; set; }
        public string DESCRIPCION_ERROR { get; set; }
    }
}
