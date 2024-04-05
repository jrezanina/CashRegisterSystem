using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Pdf.Colorspace;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Hosting.Server;
using PokladniSystem.Application.Abstraction;
using PokladniSystem.Application.ViewModels;
using PokladniSystem.Infrastructure.Database;
using PokladniSystem.Infrastructure.Migrations;
using System.Reflection.Metadata;
using Document = iText.Layout.Document;
using Path = System.IO.Path;

namespace PokladniSystem.Application.Implementation
{
    public class ReceiptService : IReceiptService
    {
        CRSDbContext _dbContext;
        CompanyViewModel _companyVM;
        StoreViewModel _storeVM;

        string _rootPath;
        const float PointSize = 2.834f; 

        public ReceiptService(string rootPath, CRSDbContext dbContext, ICompanyService companyService, IStoreService storeService)
        {
            _dbContext = dbContext;
            _rootPath = rootPath;
            _companyVM = companyService.GetCompanyViewModel();
            _storeVM = storeService.GetStoreViewModels()[0];

        }
        public byte[] GeneratePDF(int rowCount)
        {

            var pageWidth = 58f;

            int largeFontSize = 14;
            int mediumFontSize = 10;
            int smallFontSize = 10;

            var pageWidthInPoints = pageWidth * PointSize;

            var fontBBX = PdfFontFactory.CreateFont($"{_rootPath}/fonts/cmunbbx.otf");
            var fontBSR = PdfFontFactory.CreateFont($"{_rootPath}/fonts/cmunbsr.otf");

            var pageSize = new PageSize(pageWidthInPoints, PageSize.DEFAULT.GetHeight());

            using (MemoryStream ms = new MemoryStream())
            {
                using (var pdfDoc = new PdfDocument(new PdfWriter(ms)))
                using (var document = new Document(pdfDoc, pageSize))
                {
                    document.SetMargins(0, 0, 0, 0);

                    Paragraph companyP = new Paragraph(_companyVM.Company.Name).SetTextAlignment(TextAlignment.CENTER).SetFont(fontBBX).SetFontSize(largeFontSize);
                    string address1 = $"{(_companyVM.Contact.Street != null ? _companyVM.Contact.Street : _companyVM.Contact.City)} {_companyVM.Contact.BuildingNumber},";
                    string address2 = $"{_companyVM.Contact.PostalCode} {_companyVM.Contact.City}";
                    string icoDic = $"IČO: {_companyVM.Company.ICO}{(_companyVM.Company.DIC != null ? $", DIČ: {_companyVM.Company.DIC}" : string.Empty)}";
                    Paragraph address1P = new Paragraph(address1).SetTextAlignment(TextAlignment.CENTER).SetFont(fontBSR).SetFontSize(mediumFontSize);
                    Paragraph address2P = new Paragraph(address2).SetTextAlignment(TextAlignment.CENTER).SetFont(fontBSR).SetFontSize(mediumFontSize);
                    Paragraph icoDicP = new Paragraph(icoDic).SetTextAlignment(TextAlignment.CENTER).SetFont(fontBSR).SetFontSize(mediumFontSize);
                    document.Add(companyP);
                    document.Add(address1P);
                    document.Add(address2P);
                    document.Add(icoDicP);
                    LineSeparator line = new LineSeparator(new SolidLine());
                    line.SetWidth(UnitValue.CreatePercentValue(100));
                    Paragraph lineP = new Paragraph().Add(line);
                    document.Add(lineP);
                    string address3 = $"{(_storeVM.Contact.Street != null ? _storeVM.Contact.Street : _storeVM.Contact.City)} {_storeVM.Contact.BuildingNumber},";
                    string address4 = $"{(_storeVM.Contact.PostalCode)} {_companyVM.Contact.City}";
                    Paragraph address3P = new Paragraph(address3).SetTextAlignment(TextAlignment.CENTER).SetFont(fontBSR).SetFontSize(mediumFontSize);
                    Paragraph address4P = new Paragraph(address4).SetTextAlignment(TextAlignment.CENTER).SetFont(fontBSR).SetFontSize(mediumFontSize);
                    document.Add(address3P);
                    document.Add(address4P);
                    LineSeparator line2 = new LineSeparator(new SolidLine());
                    line.SetWidth(UnitValue.CreatePercentValue(100));
                    Paragraph line2P = new Paragraph().Add(line);
                    document.Add(line2P);

                    Table table = new Table(3)
                        .AddCell(new Cell().Add(new Paragraph("Název")).SetBorder(Border.NO_BORDER)).SetFont(fontBBX).SetFontSize(mediumFontSize)
                        .AddCell(new Cell().Add(new Paragraph("Ks")).SetBorder(Border.NO_BORDER)).SetFont(fontBBX).SetFontSize(mediumFontSize)
                        .AddCell(new Cell().Add(new Paragraph("Cena")).SetBorder(Border.NO_BORDER)).SetFont(fontBBX).SetFontSize(mediumFontSize);

                    table.SetBorder(Border.NO_BORDER);


                    for (int i = 0; i < rowCount; i++)
                    {
                        table.AddCell(new Cell().Add(new Paragraph("příliš žluťoučký kůň" + (i + 1))).SetFontSize(smallFontSize).SetWidth(UnitValue.CreatePercentValue(70f)).SetBorder(Border.NO_BORDER)).SetFont(fontBSR);
                        table.AddCell(new Cell().Add(new Paragraph("150")).SetFontSize(smallFontSize).SetWidth(UnitValue.CreatePercentValue(10f)).SetBorder(Border.NO_BORDER)).SetFont(fontBSR);
                        table.AddCell(new Cell().Add(new Paragraph("1234,50")).SetFontSize(smallFontSize).SetWidth(UnitValue.CreatePercentValue(20f)).SetBorder(Border.NO_BORDER)).SetFont(fontBSR);
                    }

                    table.SetWidth(UnitValue.CreatePointValue(pageWidthInPoints - 5 * 2.834f));

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
