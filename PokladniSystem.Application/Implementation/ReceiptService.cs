using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Hosting.Server;
using PokladniSystem.Application.Abstraction;
using System.Reflection.Metadata;
using Document = iText.Layout.Document;
using Path = System.IO.Path;

namespace PokladniSystem.Application.Implementation
{
    public class ReceiptService : IReceiptService
    {
        string _rootPath { get; set; }
        const float PointSize = 2.834f;

        public ReceiptService(string rootPath)
        {

            _rootPath = rootPath;

        }
        public byte[] GeneratePDF(int rowCount)
        {

            var pageWidth = 58f;

            var pageWidthInPoints = pageWidth * PointSize;

            var pageSize = new PageSize(pageWidthInPoints, PageSize.DEFAULT.GetHeight());

            using (MemoryStream ms = new MemoryStream())
            {
                using (var pdfDoc = new PdfDocument(new PdfWriter(ms)))
                using (var document = new Document(pdfDoc, pageSize))
                {
                    document.SetMargins(0, 0, 0, 0);

                    Table table = new Table(3)
                        .AddCell(new Cell().Add(new Paragraph("Název")))//.SetBorder(Border.NO_BORDER))
                        .AddCell(new Cell().Add(new Paragraph("Ks")))//.SetBorder(Border.NO_BORDER));
                        .AddCell(new Cell().Add(new Paragraph("Cena")));//.SetBorder(Border.NO_BORDER));

                    table.SetBorder(Border.NO_BORDER);
                    

                    for (int i = 0; i < rowCount; i++)
                    {
                        table.AddCell(new Cell().Add(new Paragraph("abcdefghijklmnopq" + (i + 1))).SetFontSize(10f).SetWidth(UnitValue.CreatePercentValue(70f)));//.SetBorder(Border.NO_BORDER));
                        table.AddCell(new Cell().Add(new Paragraph("150")).SetFontSize(10f).SetWidth(UnitValue.CreatePercentValue(10f)));//.SetBorder(Border.NO_BORDER));
                        table.AddCell(new Cell().Add(new Paragraph("1234,50")).SetFontSize(10f).SetWidth(UnitValue.CreatePercentValue(20f)));//.SetBorder(Border.NO_BORDER));
                    }

                    table.SetWidth(UnitValue.CreatePointValue(pageWidthInPoints-5*2.834f));

                    document.Add(table);
                }

                return ms.ToArray();
            }
        }

        public async Task<string> SavePDF(byte[] pdfBytes, string fileName, string folderName)
        {
            string filePathOutput = String.Empty;

            if (pdfBytes.Length != 0)
            {

                var fileRelative = Path.Combine(folderName, fileName);
                var filePath = Path.Combine(this._rootPath, fileRelative);

                Directory.CreateDirectory(Path.Combine(_rootPath, folderName));
                await File.WriteAllBytesAsync(filePath, pdfBytes);

                filePathOutput = Path.DirectorySeparatorChar + fileRelative;
            }

            return filePathOutput;
        }
    }
}
