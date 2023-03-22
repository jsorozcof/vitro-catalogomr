using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitroSql
{

    public class MassiveProductImages
    {
        [Key]
        public int Id { get; set; }
        public string Sap { get; set; }
        public string ProductoId { get; set; }
        public string ImagenId { get; set; }
        public string Nombre { get; set; }
        public string Extension { get; set; }

        public int Posicion { get; set; }
        public byte[] Contenido { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        

    }
}
