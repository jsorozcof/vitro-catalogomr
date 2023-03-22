using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace VitroCore
{
    public class PdfManager : IDisposable
    {
        private bool dispose = false;
        private Document document;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public byte[] Portada { get; set; }
        public byte[] Contraportada { get; set; }
        public byte[] WaterMark { get; set; }
        public byte[] Membrete { get; set; }
        public BaseFont DocumentBaseFont { get; set; }


        public PdfManager()
        {
            document = new Document(PageSize.LETTER, 66, 36, 86, 62);
        }

        public void CreatePDFFile(string path)
        {
            var writer = PdfWriter.GetInstance(document, new FileStream(path, FileMode.Create));
            writer.PageEvent = new PageBreak() { WaterMark = WaterMark, Membrete = Membrete };
            document.Open();
        }

        public void CrearPortada()
        {
            Image background = Image.GetInstance(Portada);
            background.ScaleAbsolute(PageSize.LETTER);
            background.SetAbsolutePosition(0, 0);
            document.Add(background);
        }

        public void CrearMarcaAgua()
        {
            Image watermark = Image.GetInstance(WaterMark);
            watermark.ScaleAbsolute(PageSize.LETTER);
            watermark.SetAbsolutePosition(0, 0);
            document.Add(watermark);
        }

        public void CrearMembrete()
        {
            document.NewPage();
            Image background = Image.GetInstance(Membrete);
            background.ScaleAbsolute(PageSize.LETTER);
            background.SetAbsolutePosition(0, 0);
            document.Add(background);
        }

        public void CrearContraportada()
        {
            Image background = Image.GetInstance(Contraportada);
            background.ScaleAbsolute(PageSize.LETTER);
            background.SetAbsolutePosition(0, 0);
            document.Add(background);
        }

        public void NuevaPagina()
        {
            document.NewPage();
        }

        public void EspacioEnBlanco()
        {
            document.Add(new Paragraph(Environment.NewLine));
        }

        public void CrearTablaAnidada(DataTable table, DataTable table1, byte[] imagebytes)
        {
            PdfPTable pdftablecontainer = new PdfPTable(3);
            pdftablecontainer.DefaultCell.Border = Rectangle.NO_BORDER;
            pdftablecontainer.HorizontalAlignment = Element.ALIGN_LEFT;
            pdftablecontainer.WidthPercentage = 100;

            PdfPTable pdftable = new PdfPTable(table.Columns.Count) { ExtendLastRow = false };
            pdftable.HorizontalAlignment = Element.ALIGN_LEFT;

            Font headerfont = new Font(DocumentFont().BaseFont, 7);
            headerfont.SetStyle(Font.BOLD);
            Font bodyfont = new Font(DocumentFont().BaseFont, 6);
            Font fontEncabezado = new Font(DocumentFont().BaseFont, 12);
            fontEncabezado.SetStyle(Font.BOLD);
            foreach (DataColumn column in table.Columns)
            {
                pdftable.AddCell(new PdfPCell(new Phrase(column.ColumnName.Contains("Column") ? string.Empty : column.ColumnName, fontEncabezado)) { Border = Rectangle.BOTTOM_BORDER, FixedHeight = 45f, VerticalAlignment = Element.ALIGN_BOTTOM, HorizontalAlignment = Element.ALIGN_CENTER });
            }

            foreach (DataRow row in table.Rows)
            {
                pdftable.AddCell(new PdfPCell(new Phrase(row[0].ToString(), new Font(DocumentFont().BaseFont, 12))) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                pdftable.AddCell(new PdfPCell(new Phrase(row[1].ToString(), new Font(DocumentFont().BaseFont, 12))) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                pdftable.AddCell(new PdfPCell(new Phrase(row[2].ToString(), new Font(DocumentFont().BaseFont, 12))) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                pdftable.AddCell(new PdfPCell(new Phrase(row[3].ToString(), new Font(DocumentFont().BaseFont, 12))) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
            }

            PdfPTable pdftable1 = new PdfPTable(table1.Columns.Count) { ExtendLastRow = true };
            pdftable1.HorizontalAlignment = Element.ALIGN_LEFT;
            pdftable1.SetWidths(new float[] { 6.88f, 8.88f, 7.88f, 9.88f, 3.88f, 4.88f, 3.88f, 4.88f, 3.88f, 4.88f, 4.88f, 4.88f, 4.88f, 5.88f, 4.00f, 10.88f, 4.88f, 4.88f });
            foreach (DataColumn column in table1.Columns)
            {
                pdftable1.AddCell(new PdfPCell(new Phrase(column.ColumnName.Contains("Column") ? string.Empty : column.ColumnName, headerfont)) { VerticalAlignment = Element.ALIGN_CENTER, HorizontalAlignment = Element.ALIGN_CENTER });
            }

            TextInfo info = new CultureInfo("en-US", false).TextInfo;
            foreach (DataRow row in table1.Rows)
            {
                pdftable1.AddCell(new PdfPCell(new Phrase(info.ToTitleCase(row[0].ToString().ToLower()), bodyfont)) { });
                pdftable1.AddCell(new PdfPCell(new Phrase(row[1].ToString(), bodyfont)) { });
                pdftable1.AddCell(new PdfPCell(new Phrase(row[2].ToString(), bodyfont)) { });
                pdftable1.AddCell(new PdfPCell(new Phrase(info.ToTitleCase(row[3].ToString().ToLower()), bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                pdftable1.AddCell(new PdfPCell(new Phrase(row[4].ToString(), bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                pdftable1.AddCell(new PdfPCell(new Phrase(row[5].ToString().Length > 17 ? row[5].ToString().Substring(0, 17) : row[5].ToString(), bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                pdftable1.AddCell(new PdfPCell(new Phrase(row[6].ToString(), bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                pdftable1.AddCell(new PdfPCell(new Phrase(row[7].ToString().Equals("True") ? "SI" : "NO", bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                pdftable1.AddCell(new PdfPCell(new Phrase(row[8].ToString().Equals("True") ? "SI" : "NO", bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                pdftable1.AddCell(new PdfPCell(new Phrase(row[9].ToString().Equals("True") ? "SI" : "NO", bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                pdftable1.AddCell(new PdfPCell(new Phrase(row[10].ToString().Equals("True") ? "SI" : "NO", bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                pdftable1.AddCell(new PdfPCell(new Phrase(row[11].ToString().Equals("True") ? "SI" : "NO", bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                pdftable1.AddCell(new PdfPCell(new Phrase(row[12].ToString().Equals("True") ? "SI" : "NO", bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                pdftable1.AddCell(new PdfPCell(new Phrase(row[13].ToString().Equals("True") ? "SI" : "NO", bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                pdftable1.AddCell(new PdfPCell(new Phrase(row[14].ToString().Equals("True") ? "SI" : "NO", bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                pdftable1.AddCell(new PdfPCell(new Phrase(info.ToTitleCase(row[15].ToString().ToLower()), bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                pdftable1.AddCell(new PdfPCell(new Phrase(row[16].ToString().Substring(0, 3), bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                pdftable1.AddCell(new PdfPCell(new Phrase(row[17].ToString().Equals("True") ? "SI" : "NO", bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
            }

            Image image = Image.GetInstance(imagebytes);
            image.ScaleToFit(120, 120);
            pdftablecontainer.AddCell(new PdfPCell(image) { Border = Rectangle.NO_BORDER });
            pdftablecontainer.AddCell(new PdfPCell(pdftable) { Border = Rectangle.NO_BORDER, Colspan = 2 });
            pdftablecontainer.AddCell(new PdfPCell() { Border = Rectangle.NO_BORDER, Colspan = 3, FixedHeight = 10f });
            pdftablecontainer.AddCell(new PdfPCell(pdftable1) { Colspan = 3, Border = Rectangle.NO_BORDER });
            pdftablecontainer.AddCell(new PdfPCell() { Colspan = 3, Border = Rectangle.NO_BORDER, FixedHeight = 15f });
            pdftablecontainer.AddCell(new PdfPCell() { Colspan = 3, Border = Rectangle.TOP_BORDER, FixedHeight = 15f, BorderColor = new BaseColor(103, 152, 193) });

            document.Add(pdftablecontainer);
        }

        public void CrearTablaAnidada(List<PdfDataModel> model)
        {
            PdfPTable pdftablecontainer = new PdfPTable(3);
            pdftablecontainer.DefaultCell.Border = Rectangle.NO_BORDER;
            pdftablecontainer.HorizontalAlignment = Element.ALIGN_LEFT;
            pdftablecontainer.WidthPercentage = 100;

            foreach (var data in model)
            {
                PdfPTable pdftable = new PdfPTable(data.TablaEncabezado.Columns.Count) { ExtendLastRow = false };
                pdftable.HorizontalAlignment = Element.ALIGN_LEFT;

                Font headerfont = new Font(DocumentFont().BaseFont, 7);
                headerfont.SetStyle(Font.BOLD);
                Font bodyfont = new Font(DocumentFont().BaseFont, 6);
                Font fontEncabezado = new Font(DocumentFont().BaseFont, 12);
                fontEncabezado.SetStyle(Font.BOLD);
                foreach (DataColumn column in data.TablaEncabezado.Columns)
                {
                    pdftable.AddCell(new PdfPCell(new Phrase(column.ColumnName.Contains("Column") ? string.Empty : column.ColumnName, fontEncabezado)) { Border = Rectangle.BOTTOM_BORDER, FixedHeight = 35f, VerticalAlignment = Element.ALIGN_BOTTOM, HorizontalAlignment = Element.ALIGN_CENTER });
                }

                foreach (DataRow row in data.TablaEncabezado.Rows)
                {
                    pdftable.AddCell(new PdfPCell(new Phrase(row[0].ToString(), new Font(DocumentFont().BaseFont, 12))) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                    pdftable.AddCell(new PdfPCell(new Phrase(row[1].ToString(), new Font(DocumentFont().BaseFont, 12))) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                    pdftable.AddCell(new PdfPCell(new Phrase(row[2].ToString(), new Font(DocumentFont().BaseFont, 12))) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                    pdftable.AddCell(new PdfPCell(new Phrase(row[3].ToString(), new Font(DocumentFont().BaseFont, 12))) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                }

                PdfPTable pdftable1 = new PdfPTable(data.TablaDetalle.Columns.Count) { ExtendLastRow = true };
                pdftable1.HorizontalAlignment = Element.ALIGN_LEFT;
                pdftable1.SetWidths(new float[] { 6.88f, 8.88f, 7.88f, 9.88f, 3.88f, 4.88f, 3.88f, 4.88f, 3.88f, 4.88f, 4.88f, 4.88f, 4.88f, 5.88f, 4.00f, 10.88f, 4.88f, 4.88f });
                foreach (DataColumn column in data.TablaDetalle.Columns)
                {
                    pdftable1.AddCell(new PdfPCell(new Phrase(column.ColumnName.Contains("Column") ? string.Empty : column.ColumnName, headerfont)) { VerticalAlignment = Element.ALIGN_CENTER, HorizontalAlignment = Element.ALIGN_CENTER });
                }

                TextInfo info = new CultureInfo("en-US", false).TextInfo;
                foreach (DataRow row in data.TablaDetalle.Rows)
                {
                    pdftable1.AddCell(new PdfPCell(new Phrase(info.ToTitleCase(row[0].ToString().ToLower()), bodyfont)) { });
                    pdftable1.AddCell(new PdfPCell(new Phrase(row[1].ToString(), bodyfont)) { });
                    pdftable1.AddCell(new PdfPCell(new Phrase(row[2].ToString(), bodyfont)) { });
                    pdftable1.AddCell(new PdfPCell(new Phrase(info.ToTitleCase(row[3].ToString().ToLower()), bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdftable1.AddCell(new PdfPCell(new Phrase(row[4].ToString(), bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdftable1.AddCell(new PdfPCell(new Phrase(row[5].ToString().Length > 17 ? row[5].ToString().Substring(0, 17) : row[5].ToString(), bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdftable1.AddCell(new PdfPCell(new Phrase(row[6].ToString(), bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdftable1.AddCell(new PdfPCell(new Phrase(row[7].ToString().Equals("True") ? "SI" : "NO", bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdftable1.AddCell(new PdfPCell(new Phrase(row[8].ToString().Equals("True") ? "SI" : "NO", bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdftable1.AddCell(new PdfPCell(new Phrase(row[9].ToString().Equals("True") ? "SI" : "NO", bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdftable1.AddCell(new PdfPCell(new Phrase(row[10].ToString().Equals("True") ? "SI" : "NO", bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdftable1.AddCell(new PdfPCell(new Phrase(row[11].ToString().Equals("True") ? "SI" : "NO", bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdftable1.AddCell(new PdfPCell(new Phrase(row[12].ToString().Equals("True") ? "SI" : "NO", bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdftable1.AddCell(new PdfPCell(new Phrase(row[13].ToString().Equals("True") ? "SI" : "NO", bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdftable1.AddCell(new PdfPCell(new Phrase(row[14].ToString().Equals("True") ? "SI" : "NO", bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdftable1.AddCell(new PdfPCell(new Phrase(info.ToTitleCase(row[15].ToString().ToLower()), bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdftable1.AddCell(new PdfPCell(new Phrase(row[16].ToString().Substring(0, 3), bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    pdftable1.AddCell(new PdfPCell(new Phrase(row[17].ToString().Equals("True") ? "SI" : "NO", bodyfont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                }

                Image image = Image.GetInstance(data.ImageFileName);
                image.ScaleToFit(120, 120);
                pdftablecontainer.AddCell(new PdfPCell(image) { Border = Rectangle.NO_BORDER });
                pdftablecontainer.AddCell(new PdfPCell(pdftable) { Border = Rectangle.NO_BORDER, Colspan = 2 });
                pdftablecontainer.AddCell(new PdfPCell() { Border = Rectangle.NO_BORDER, Colspan = 3, FixedHeight = 2f });
                pdftablecontainer.AddCell(new PdfPCell(pdftable1) { Colspan = 3, Border = Rectangle.NO_BORDER });
                pdftablecontainer.AddCell(new PdfPCell() { Colspan = 3, Border = Rectangle.NO_BORDER, FixedHeight = 8f });
                pdftablecontainer.AddCell(new PdfPCell() { Colspan = 3, Border = Rectangle.TOP_BORDER, FixedHeight = 5f, BorderColor = new BaseColor(103, 152, 193) });
            }
            document.Add(pdftablecontainer);
        }

        public void NumerarPaginas(string filepath)
        {
            try
            {
                byte[] content = File.ReadAllBytes(filepath);
                using (MemoryStream stream = new MemoryStream())
                {
                    using (PdfReader reader = new PdfReader(content))
                    {
                        using (PdfStamper stamper = new PdfStamper(reader, stream))
                        {
                            for (int i = 1; i <= reader.NumberOfPages; i++)
                            {
                                ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_CENTER, new Phrase(i.ToString()), 100f, 25f, 0);
                                System.Diagnostics.Debug.WriteLine($"agregando numerador en pagina {i}");
                            }
                        }
                    }
                    content = stream.ToArray();
                }
                File.WriteAllBytes(filepath, content);
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine(error.Message);
            }
        }

        private Font DocumentFont()
        {
            Font font = new Font(DocumentBaseFont);
            return font;
        }

        public void Dispose()
        {
            document.Dispose();
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (dispose) { return; }
            if (disposing)
            {
                handle.Dispose();
            }
            dispose = true;
        }
    }

    class PageBreak : PdfPageEventHelper
    {
        private PdfContentByte content;
        private PdfTemplate template;
        public byte[] Membrete { get; set; }
        public byte[] WaterMark { get; set; }

        public override void OnStartPage(PdfWriter writer, Document document)
        {
            if (writer.PageNumber > 2)
            {
                Image background = Image.GetInstance(Membrete);
                background.ScaleAbsolute(PageSize.LETTER);
                background.SetAbsolutePosition(0, 0);

                Image watermark = Image.GetInstance(WaterMark);
                watermark.ScaleAbsolute(PageSize.LETTER);
                watermark.SetAbsolutePosition(0, 0);

                document.Add(background);
                document.Add(watermark);


            }
            if (writer.PageNumber > 1)
            {
                PdfPTable table = new PdfPTable(1);
                table.TotalWidth = document.PageSize.Width;

                Chunk footer = new Chunk($"- {document.PageNumber - 1} -");
                PdfPCell cell = new PdfPCell(new Phrase(footer)) { Border = Rectangle.NO_BORDER };
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                table.WriteSelectedRows(0, -1, 0, document.BottomMargin - 40, writer.DirectContent);
            }
        }

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            content = writer.DirectContent;
            template = content.CreateTemplate(50, 50);
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {

        }
    }
}