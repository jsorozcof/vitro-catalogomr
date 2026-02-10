using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using VitroSql;

namespace VitroCore.Services
{
    public class ProcessProductRepository
    {
        private readonly string _connectionString;

        public ProcessProductRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["VitroContext"].ConnectionString;
        }

        public DataTable ConstruirDataTableImagenes(List<ProductImages> imagenes)
        {
            var table = new DataTable();
            table.Columns.Add("ProductId", typeof(string));
            table.Columns.Add("SAP", typeof(string));
            table.Columns.Add("ImagenId", typeof(Guid));
            table.Columns.Add("Posicion", typeof(int));
            table.Columns.Add("Nombre", typeof(string));
            table.Columns.Add("Extension", typeof(string));
            table.Columns.Add("Contenido", typeof(byte[]));
            table.Columns.Add("FechaCreacion", typeof(DateTime));
            table.Columns.Add("FechaActualizacion", typeof(DateTime));

            foreach (var img in imagenes)
            {
                table.Rows.Add(img.ProductId, img.Sap, img.ImagenId, img.Posicion,img.Nombre, img.Extension, img.Contenido, DateTime.Now,null);
            }

            return table;
        }
        /// <summary>
        /// Ejecuta el procedimiento almacenado para procesar productos.
        /// </summary>
        //public DataTable ProcesarProductos(DataTable productos, List<ProductImages> dataTableImagenes, string pais, bool actualizaProductos, string usuario)
        //{
        //    DataTable dtErrores = new DataTable();

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(_connectionString))
        //        {
        //            conn.Open();
        //            using (SqlCommand cmd = new SqlCommand("dbo.SP_ProcessProducts", conn))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                // Parámetros
        //                cmd.Parameters.Add(new SqlParameter("@PAIS", SqlDbType.NVarChar, 100) { Value = pais });
        //                cmd.Parameters.Add(new SqlParameter("@USERNAME", SqlDbType.NVarChar, 100) { Value = usuario });
        //                cmd.Parameters.Add(new SqlParameter("@ACTUALIZAPRODUCTOS", SqlDbType.Bit) { Value = actualizaProductos });

        //                // Convertir List<TempProducto> a DataTable
        //                DataTable dtProductos = ConvertirADataTable(productos,usuario);
        //                DataTable dtImagenes = ConstruirDataTableImagenes(dataTableImagenes);

        //                // Parámetro de tabla (TVP)
        //                SqlParameter tableParam = new SqlParameter("@DATA", SqlDbType.Structured)
        //                {
        //                    TypeName = "dbo.ProductType",
        //                    Value = dtProductos
        //                };
        //                SqlParameter imgParam = new SqlParameter("@IMAGES", SqlDbType.Structured)
        //                {
        //                    TypeName = "dbo.ProductImagesType",
        //                    Value = dtImagenes
        //                };
        //                cmd.Parameters.Add(tableParam);
        //                cmd.Parameters.Add(imgParam);
        //                //cmd.ExecuteNonQuery();
        //                //// Ejecutar SP y obtener errores
        //                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        //                {
        //                    da.Fill(dtErrores);
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw new Exception("Ocurrió un error al procesar los productos en la base de datos.", ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Ocurrió un error inesperado procesando los productos en la base de datos.", ex);

        //    }

        //    return dtErrores;
        //}


        public ProcessResult ProcesarProductos(
           DataTable productos,
           List<ProductImages> dataTableImagenes,
           string pais,
           bool actualizaProductos,
           string usuario)
        {
            if (productos == null) throw new ArgumentNullException(nameof(productos));
            if (string.IsNullOrWhiteSpace(pais)) throw new ArgumentException("El país no puede ser nulo o vacío.", nameof(pais));
            if (string.IsNullOrWhiteSpace(usuario)) throw new ArgumentException("El usuario no puede ser nulo o vacío.", nameof(usuario));

            var result = new ProcessResult();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand("dbo.SP_ProcessProducts_v2", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 180;

                    // Parámetros de entrada
                    cmd.Parameters.Add("@PAIS", SqlDbType.NVarChar, 100).Value = pais;
                    cmd.Parameters.Add("@USERNAME", SqlDbType.NVarChar, 100).Value = usuario;
                    cmd.Parameters.Add("@ACTUALIZAPRODUCTOS", SqlDbType.Bit).Value = actualizaProductos;

                    // Convertir List<TempProducto> a DataTable
                    DataTable dtProductos = ConvertirADataTable(productos, usuario);

                    // TVP Productos
                    var productParam = new SqlParameter("@DATA", SqlDbType.Structured)
                    {
                        TypeName = "dbo.ProductType",
                        Value = dtProductos
                    };
                    cmd.Parameters.Add(productParam);

                    // TVP Imágenes (si aplica)
                    if (dataTableImagenes != null && dataTableImagenes.Any())
                    {
                        var dtImagenes = ConstruirDataTableImagenes(dataTableImagenes);
                        var imgParam = new SqlParameter("@IMAGES", SqlDbType.Structured)
                        {
                            TypeName = "dbo.ProductImagesType",
                            Value = dtImagenes
                        };
                        cmd.Parameters.Add(imgParam);
                    }

                    // Parámetros de salida
                    var rowsInsertedParam = new SqlParameter("@RowsInserted", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var rowsUpdatedParam = new SqlParameter("@RowsUpdated", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var errorsCountParam = new SqlParameter("@ErrorsCount", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(rowsInsertedParam);
                    cmd.Parameters.Add(rowsUpdatedParam);
                    cmd.Parameters.Add(errorsCountParam);

                    conn.Open();

                    // Usamos DataAdapter porque el SP devuelve 2 resultsets
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        var ds = new DataSet();
                        da.Fill(ds);

                        if (ds.Tables.Count > 0)
                        {
                            // Primer resultset → errores
                            result.Errores = ds.Tables[0];
                        }
                    }

                    // Mapear parámetros de salida
                    result.RowsInserted = rowsInsertedParam.Value != DBNull.Value ? (int)rowsInsertedParam.Value : 0;
                    result.RowsUpdated = rowsUpdatedParam.Value != DBNull.Value ? (int)rowsUpdatedParam.Value : 0;
                    result.ErrorsCount = errorsCountParam.Value != DBNull.Value ? (int)errorsCountParam.Value : 0;
                }
            }
            catch (SqlException ex)
            {
                throw new DataException("Error SQL al procesar los productos.", ex);
            }

            return result;
        }





        /// <summary>
        /// Obtiene los errores almacenados en la tabla LogErroresCarga.
        /// </summary>
        public DataTable ObtenerErroresCarga()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM LogErroresCarga WHERE ESTADO_CORREGIDO = 0", conn))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dtErrores = new DataTable();
                    da.Fill(dtErrores);
                    return dtErrores;
                }
            }
        }

        private DataTable ConvertirADataTable(DataTable productos, string user)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("FILA", typeof(int));
            dt.Columns.Add("PRODUCTID", typeof(string));
            dt.Columns.Add("PAISID", typeof(string));
            dt.Columns.Add("SAP", typeof(string));
            dt.Columns.Add("NAGS", typeof(string));
            dt.Columns.Add("MARCAID", typeof(string));
            dt.Columns.Add("MODELOID", typeof(string));
            dt.Columns.Add("DESCRIPCION", typeof(string));
            dt.Columns.Add("MERCADOID", typeof(string));
            dt.Columns.Add("COLORID", typeof(string));
            dt.Columns.Add("TIPOVIDRIOID", typeof(string));
            dt.Columns.Add("TIPOPARTEID", typeof(string));
            dt.Columns.Add("ANCHO", typeof(decimal));
            dt.Columns.Add("ALTO", typeof(decimal));
            dt.Columns.Add("BOTON", typeof(bool));
            dt.Columns.Add("RED", typeof(bool));
            dt.Columns.Add("SERIGRAFIA", typeof(bool));
            dt.Columns.Add("SENSORLLUVIA", typeof(bool));
            dt.Columns.Add("MOLDURA", typeof(bool));
            dt.Columns.Add("HOLDER", typeof(bool));
            dt.Columns.Add("SENSORCONDENSACION", typeof(bool));
            dt.Columns.Add("HOMOLOGO", typeof(string));
            dt.Columns.Add("CLASIFICACION", typeof(string));
            dt.Columns.Add("PERFORACION", typeof(bool));
            dt.Columns.Add("STARTYEAR", typeof(int));
            dt.Columns.Add("ENDYEAR", typeof(int));
            dt.Columns.Add("PROCEDENCIAID", typeof(string));
            dt.Columns.Add("ACTIVO", typeof(bool));
            dt.Columns.Add("FECHACREACION", typeof(DateTime));
            dt.Columns.Add("FECHAMODIFICACION", typeof(DateTime));
            dt.Columns.Add("ANTENA", typeof(bool));
            dt.Columns.Add("SUBENSAMBLE", typeof(bool));
            dt.Columns.Add("USUARIO", typeof(string));

            int fila = 2;

            List<TbProduct> productList = productos.AsEnumerable()
                .Select(row => new TbProduct
                {
                    ProductId = $"{Guid.NewGuid()}",
                    PaisId = row["PAIS"].ToString(),
                    SAP = row["SAP"].ToString(),
                    NAGS = row["NAGS"].ToString(),
                    MarcaId = row["MARCA"].ToString(),
                    ModeloId = row["MODELO"].ToString(),
                    StartYear = int.Parse(row["AÑO INICIAL"].ToString()),
                    EndYear = int.Parse(row["AÑO FINAL"].ToString()),
                    Descripcion = row["DESCRIPCION"].ToString(),
                    TipoParteId = row["TIPO PARTE"].ToString(),
                    Perforacion = double.Parse(row["PERFORACION"].ToString()),
                    Ancho = double.Parse(row["ANCHO"].ToString()),
                    Alto = double.Parse(row["ALTO"].ToString()),
                    Boton = row["BOTON"].ToString().Equals("SI") ? true : false,
                    Red = row["RED"].ToString().Equals("SI") ? true : false,
                    Serigrafia = row["SERIGRAFIA"].ToString().Equals("SI") ? true : false,
                    SensorLluvia = row["SENSOR LLUVIA"].ToString().Equals("SI") ? true : false,
                    Moldura = row["MOLDURA"].ToString().Equals("SI") ? true : false,
                    Holder = row["HOLDER"].ToString().Equals("SI") ? true : false,
                    Antena = row["ANTENA"].ToString().Equals("SI") ? true : false,
                    SubEnsamble = row["SUB ENSAMBLE"].ToString().Equals("SI") ? true : false,
                    SensorCondensacion = row["SENSOR CONDENSACION"].ToString().Equals("SI") ? true : false,
                    ColorId = row["COLOR"].ToString(),
                    TipoVidrioId = row["TIPO VIDRIO"].ToString(),
                    ProcedenciaId = row["PROCEDENCIA"].ToString(),
                    Homologo = row["HOMOLOGO"].ToString().Equals("SI") ? true : false,
                    Clasificacion = row["CLASIFICACION"].ToString(),
                    MercadoId = row["MERCADO"].ToString(),
                    FechaCreacion = DateTime.Now,
                    CreadoPor = user

                }).ToList();


            foreach (var producto in productList)
            {
                dt.Rows.Add(
                    fila,
                    producto.ProductId, producto.PaisId, producto.SAP, producto.NAGS, producto.MarcaId,
                    producto.ModeloId, producto.Descripcion, producto.MercadoId, producto.ColorId, producto.TipoVidrioId,
                    producto.TipoParteId, producto.Ancho, producto.Alto, producto.Boton, producto.Red,
                    producto.Serigrafia, producto.SensorLluvia, producto.Moldura, producto.Holder,
                    producto.SensorCondensacion, producto.Homologo, producto.Clasificacion, producto.Perforacion,
                    producto.StartYear, producto.EndYear, producto.ProcedenciaId, producto.Activo,
                    producto.FechaCreacion, producto.FechaCreacion, producto.Antena, producto.SubEnsamble,
                    producto.CreadoPor
                );
                fila++;
            }

            return dt;
        }
    }
}
