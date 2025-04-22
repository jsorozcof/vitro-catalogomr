

using System;

public class ExportFullProductReportDto
{
    public string PAIS { get; set; }
    public string SAP { get; set; }
    public string NAGS { get; set; }
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public string Descripcion { get; set; }
    public string Mercado { get; set; }
    public string Color { get; set; }
    public string TipoVidrio { get; set; }
    public string TipoParte { get; set; }
    public double Ancho { get; set; }
    public double Alto { get; set; }

    public string Boton => BotonBit ? "SI" : "NO";
    public string Red => RedBit ? "SI" : "NO";
    public string Serigrafia => SerigrafiaBit ? "SI" : "NO";
    public string SensorLluvia => SensorLluviaBit ? "SI" : "NO";
    public string Moldura => MolduraBit ? "SI" : "NO";
    public string Holder => HolderBit ? "SI" : "NO";
    public string SensorCondensacion => SensorCondensacionBit ? "SI" : "NO";
    public string Homologo => HomologoBit ? "SI" : "NO";
    public string Antena => AntenaBit ? "SI" : "NO";
    public string SubEnsamble => SubEnsambleBit ? "SI" : "NO";
    public string Activo => ActivoBit ? "SI" : "NO";

    public string Clasificacion { get; set; }
    public double Perforacion { get; set; }
    public int StartYear { get; set; }
    public int EndYear { get; set; }
    public string Procedencia { get; set; }
    public DateTime FechaCreacion { get; set; }
    public string CreadoPor { get; set; }

    // Internos para manejo de bits
    public bool BotonBit { get; set; }
    public bool RedBit { get; set; }
    public bool SerigrafiaBit { get; set; }
    public bool SensorLluviaBit { get; set; }
    public bool MolduraBit { get; set; }
    public bool HolderBit { get; set; }
    public bool SensorCondensacionBit { get; set; }
    public bool HomologoBit { get; set; }
    public bool AntenaBit { get; set; }
    public bool SubEnsambleBit { get; set; }
    public bool ActivoBit { get; set; }

    // Imágenes por posición
    public string Imagen { get; set; } 
    public string Imagen1 { get; set; }
    public string Imagen2 { get; set; } 
    public string Imagen3 { get; set; }
    public string Imagen4 { get; set; } 
    public string Imagen5 { get; set; }
    public string Imagen6 { get; set; }
    public string Imagen7 { get; set; }
    public string Imagen8 { get; set; }  
    public string Imagen9 { get; set; } 
    public string Imagen10 { get; set; }  
}
//public class ExportFullProductReportDto
//{
//    public string SAP { get; set; }
//    public string NAGS { get; set; }
//    public string Marca { get; set; }
//    public string Modelo { get; set; }
//    public string Descripcion { get; set; }
//    public string Pais { get; set; }
//    public string Color { get; set; }
//    public string Mercado { get; set; }
//    public string TipoVidrio { get; set; }
//    public string TipoParte { get; set; }
//    public double Ancho { get; set; }
//    public double Alto { get; set; }
//    public string Boton { get; set; }
//    public string Red { get; set; }
//    public string Serigrafia { get; set; }
//    public string SensorLluvia { get; set; }
//    public string Moldura { get; set; }
//    public string Holder { get; set; }
//    public string SensorCondensacion { get; set; }
//    public string Homologo { get; set; }
//    public double Perforacion { get; set; }
//    public int StartYear { get; set; }
//    public int EndYear { get; set; }
//    public string ProcedenciaId { get; set; }
//    public string Activo { get; set; }
//    public string Antena { get; set; }
//    public string SubEnsamble { get; set; }
//    public string CreadoPor { get; set; }
//    public DateTime FechaCreacion { get; set; }
//    public DateTime? FechaModificacion { get; set; }

//    // Imágenes por posición
//    public string Imagen { get; set; }
//    public string Imagen1 { get; set; }
//    public string Imagen2 { get; set; }
//    public string Imagen3 { get; set; }
//    public string Imagen4 { get; set; }
//    public string Imagen5 { get; set; }
//    public string Imagen6 { get; set; }
//    public string Imagen7 { get; set; }
//    public string Imagen8 { get; set; }
//    public string Imagen9 { get; set; }
//    public string Imagen10 { get; set; }
//}