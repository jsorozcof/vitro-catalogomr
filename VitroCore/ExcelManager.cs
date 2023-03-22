using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.Data;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace VitroCore
{
    public class ExcelManager : IDisposable
    {
        private bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public DataTable ReadFile(Stream stream)
        {
            DataTable table = new DataTable();

            using (ExcelPackage package = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                foreach (var column in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
                {
                    table.Columns.Add(column.Text, typeof(string));
                }

                for (int index = 2; index <= worksheet.Dimension.End.Row; index++)
                {
                    DataRow rowtable = table.NewRow();
                    var rows = worksheet.Cells[index, 1, index, worksheet.Dimension.End.Column];

                    foreach (var cell in rows)
                    {
                        rowtable[cell.Start.Column - 1] = cell.Text;
                    }
                    table.Rows.Add(rowtable);
                }
            }

            return table;
        }

        public void CreateFile(string filepath, DataTable table)
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo(filepath)))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add($"RESUMEN CARGA {DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}");
                sheet.Cells["A1"].Value = "SAP";
                sheet.Cells["B1"].Value = "NAGS";
                sheet.Cells["C1"].Value = "MODELOID";
                sheet.Cells["D1"].Value = "AÑO INICIAL";
                sheet.Cells["E1"].Value = "AÑO FINAL";
                sheet.Cells["F1"].Value = "DESCRIPCION";
                sheet.Cells["G1"].Value = "TIPOPARTEID";
                sheet.Cells["H1"].Value = "PERFORACION";
                sheet.Cells["I1"].Value = "ANCHO";
                sheet.Cells["J1"].Value = "ALTO";
                sheet.Cells["K1"].Value = "BOTON";
                sheet.Cells["L1"].Value = "RED";
                sheet.Cells["M1"].Value = "SERIGRAFIA";
                sheet.Cells["N1"].Value = "SENSOR LLUVIA";
                sheet.Cells["O1"].Value = "MOLDURA";
                sheet.Cells["P1"].Value = "HOLDER";
                sheet.Cells["Q1"].Value = "SENSOR CONDENSACION";
                sheet.Cells["R1"].Value = "COLORID";
                sheet.Cells["S1"].Value = "TIPOVIDRIOID";
                sheet.Cells["T1"].Value = "PROCEDENCIAID";
                sheet.Cells["U1"].Value = "HOMOLOGO";
                sheet.Cells["V1"].Value = "MERCADOID";
                sheet.Cells["W1"].Value = "VALIDO";
                sheet.Cells["X1"].Value = "RESULTADO";

                int reg_index = 0;
                for (int i = 2; i < (table.Rows.Count + 2); i++)
                {
                    sheet.Cells[i, 1].Value = table.Rows[reg_index][0].ToString();
                    sheet.Cells[i, 2].Value = table.Rows[reg_index][1].ToString();
                    sheet.Cells[i, 3].Value = table.Rows[reg_index][2].ToString();
                    sheet.Cells[i, 4].Value = table.Rows[reg_index][3].ToString();
                    sheet.Cells[i, 5].Value = table.Rows[reg_index][4].ToString();
                    sheet.Cells[i, 6].Value = table.Rows[reg_index][5].ToString();
                    sheet.Cells[i, 7].Value = table.Rows[reg_index][6].ToString();
                    sheet.Cells[i, 8].Value = table.Rows[reg_index][7].ToString();
                    sheet.Cells[i, 9].Value = table.Rows[reg_index][8].ToString();
                    sheet.Cells[i, 10].Value = table.Rows[reg_index][9].ToString();
                    sheet.Cells[i, 11].Value = table.Rows[reg_index][10].ToString();
                    sheet.Cells[i, 12].Value = table.Rows[reg_index][11].ToString();
                    sheet.Cells[i, 13].Value = table.Rows[reg_index][12].ToString();
                    sheet.Cells[i, 14].Value = table.Rows[reg_index][13].ToString();
                    sheet.Cells[i, 15].Value = table.Rows[reg_index][14].ToString();
                    sheet.Cells[i, 16].Value = table.Rows[reg_index][15].ToString();
                    sheet.Cells[i, 17].Value = table.Rows[reg_index][16].ToString();
                    sheet.Cells[i, 18].Value = table.Rows[reg_index][17].ToString();
                    sheet.Cells[i, 19].Value = table.Rows[reg_index][18].ToString();
                    sheet.Cells[i, 20].Value = table.Rows[reg_index][19].ToString();
                    sheet.Cells[i, 21].Value = table.Rows[reg_index][20].ToString();
                    sheet.Cells[i, 22].Value = table.Rows[reg_index][21].ToString();
                    sheet.Cells[i, 23].Value = table.Rows[reg_index][22].ToString();
                    sheet.Cells[i, 24].Value = table.Rows[reg_index][23].ToString();
                    reg_index++;
                }

                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
                sheet.Protection.IsProtected = false;
                sheet.Protection.AllowSelectLockedCells = false;

                package.Save();
            }
        }

        public void CreateFilePromociones(string filepath, DataTable table)
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo(filepath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"PLANTILLA DE PROMOCIONES {DateTime.Today}");

                worksheet.Cells["A1"].Value = "ID";
                worksheet.Cells["B1"].Value = "SAP";
                worksheet.Cells["C1"].Value = "NAGS";
                worksheet.Cells["D1"].Value = "DESCRIPCION";
                worksheet.Cells["E1"].Value = "PRECIO";
                worksheet.Cells["F1"].Value = "STOCK";
                worksheet.Cells["G1"].Value = "FECHA INICIO";
                worksheet.Cells["H1"].Value = "FECHA FINAL";
                worksheet.Cells["A1:H1"].Style.Font.Bold = true;
                worksheet.Cells["A1:H1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.LightGray;
                worksheet.Cells["A1:H1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                int rowcount = 2;
                foreach (DataRow row in table.Rows)
                {
                    worksheet.Cells[$"A{rowcount}"].Value = row.Field<string>("ID");
                    worksheet.Cells[$"B{rowcount}"].Value = row.Field<string>("SAP");
                    worksheet.Cells[$"C{rowcount}"].Value = row.Field<string>("NAGS");
                    worksheet.Cells[$"D{rowcount}"].Value = row.Field<string>("DESCRIPCION");
                    rowcount++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                worksheet.Protection.IsProtected = false;
                worksheet.Protection.AllowSelectLockedCells = false;
                package.Save();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                handle.Dispose();
            }
            disposed = true;
        }
    }
}
