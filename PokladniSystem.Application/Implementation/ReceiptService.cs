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
using PokladniSystem.Domain.Entities;
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
        ISaleService _saleService;
        ICompanyService _companyService;
        IStoreService _storeService;
        IProductService _productService;
        IVATService _vatService;

        string _rootPath;
        const float PointSize = 2.834f;

        public ReceiptService(string rootPath, CRSDbContext dbContext, ISaleService saleService, ICompanyService companyService, IStoreService storeService, IProductService productService, IVATService vatService)
        {
            _dbContext = dbContext;
            _saleService = saleService;
            _companyService = companyService;
            _storeService = storeService;
            _rootPath = rootPath;
            _productService = productService;
            _vatService = vatService;

        }
        public byte[] GenerateReceiptPDF(int orderId)
        {
            Order order = _saleService.GetOrder(orderId);
            IList<OrderItem> orderItems = _saleService.GetOrderItems(orderId);
            CompanyViewModel companyVM = _companyService.GetCompanyViewModel();
            StoreViewModel storeVM = _storeService.GetStoreViewModels().FirstOrDefault(s => s.Store.Id == order.StoreId);
            IList<VATRate> vatRates = _vatService.GetVATRates();

            var pageWidth = 58f;

            int largeFontSize = 14;
            int mediumFontSize = 12;
            int smallFontSize = 10;
            int borderMargin = 3;

            var pageWidthInPoints = pageWidth * PointSize;

            var fontBold = PdfFontFactory.CreateFont($"{_rootPath}/fonts/cmunbbx.otf");
            var fontRegular = PdfFontFactory.CreateFont($"{_rootPath}/fonts/cmunbsr.otf");

            var pageSize = new PageSize(pageWidthInPoints, PageSize.DEFAULT.GetHeight());

            using (MemoryStream ms = new MemoryStream())
            {
                using (var pdfDoc = new PdfDocument(new PdfWriter(ms)))
                {
                    using (var document = new Document(pdfDoc, pageSize))
                    {
                        document.SetMargins(0, 0, 0, 0);

                        string s;
                        Paragraph p;
                        Table t;


                        // Company header
                        p = new Paragraph(companyVM.Company.Name).SetTextAlignment(TextAlignment.CENTER).SetFont(fontBold).SetFontSize(largeFontSize);
                        document.Add(p);

                        // Company address
                        s = $"{(companyVM.Contact.Street != null ? companyVM.Contact.Street : companyVM.Contact.City)} {companyVM.Contact.BuildingNumber},\n" +
                            $"{companyVM.Contact.PostalCode} {companyVM.Contact.City}";
                        p = new Paragraph(s).SetTextAlignment(TextAlignment.CENTER).SetFont(fontRegular).SetFontSize(smallFontSize);
                        document.Add(p);

                        // Company ICO and DIC
                        s = $"IČO: {companyVM.Company.ICO}{(companyVM.Company.DIC != null ? $", DIČ: {companyVM.Company.DIC}" : string.Empty)}";
                        p = new Paragraph(s).SetTextAlignment(TextAlignment.CENTER).SetFont(fontRegular).SetFontSize(smallFontSize);
                        document.Add(p);

                        // Company contact
                        if (companyVM.Contact.Phone != null)
                        {
                            t = new Table(2)
                                .AddCell(new Cell().Add(new Paragraph("Tel.:")).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize)
                                .AddCell(new Cell().Add(new Paragraph(companyVM.Contact.Phone)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize);

                            t.SetBorder(Border.NO_BORDER);
                            t.SetWidth(UnitValue.CreatePointValue(pageWidthInPoints - borderMargin * PointSize));

                            document.Add(t);
                        }

                        if (companyVM.Contact.Email != null)
                        {
                            t = new Table(2)
                                .AddCell(new Cell().Add(new Paragraph("Email:")).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize)
                                .AddCell(new Cell().Add(new Paragraph(companyVM.Contact.Email)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize);

                            t.SetBorder(Border.NO_BORDER);
                            t.SetWidth(UnitValue.CreatePointValue(pageWidthInPoints - borderMargin * PointSize));

                            document.Add(t);
                        }

                        if (companyVM.Contact.Web != null)
                        {
                            t = new Table(2)
                                .AddCell(new Cell().Add(new Paragraph("Web:")).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize)
                                .AddCell(new Cell().Add(new Paragraph(companyVM.Contact.Web)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize);

                            t.SetBorder(Border.NO_BORDER);
                            t.SetWidth(UnitValue.CreatePointValue(pageWidthInPoints - borderMargin * PointSize));

                            document.Add(t);
                        }

                        // Separator
                        LineSeparator ls = new LineSeparator(new SolidLine()).SetWidth(UnitValue.CreatePercentValue(100));
                        p = new Paragraph().Add(ls);
                        document.Add(p);

                        // Store header
                        p = new Paragraph("Prodejna:").SetTextAlignment(TextAlignment.CENTER).SetFont(fontBold).SetFontSize(mediumFontSize);
                        document.Add(p);

                        s = $"{storeVM.Store.Name}";
                        p = new Paragraph(s).SetTextAlignment(TextAlignment.CENTER).SetFont(fontRegular).SetFontSize(smallFontSize);
                        document.Add(p);

                        // Store address
                        s = $"{(storeVM.Contact.Street != null ? storeVM.Contact.Street : storeVM.Contact.City)} {storeVM.Contact.BuildingNumber},\n" +
                            $"{(storeVM.Contact.PostalCode)} {storeVM.Contact.City}";
                        p = new Paragraph(s).SetTextAlignment(TextAlignment.CENTER).SetFont(fontRegular).SetFontSize(smallFontSize);
                        document.Add(p);

                        // Store contact
                        if (storeVM.Contact.Phone != null)
                        {
                            t = new Table(2)
                                .AddCell(new Cell().Add(new Paragraph("Tel.:")).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize)
                                .AddCell(new Cell().Add(new Paragraph(storeVM.Contact.Phone)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize);

                            t.SetBorder(Border.NO_BORDER);
                            t.SetWidth(UnitValue.CreatePointValue(pageWidthInPoints - borderMargin * PointSize));

                            document.Add(t);
                        }

                        if (storeVM.Contact.Email != null)
                        {
                            t = new Table(2)
                                .AddCell(new Cell().Add(new Paragraph("Email:")).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize)
                                .AddCell(new Cell().Add(new Paragraph(storeVM.Contact.Email)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize);

                            t.SetBorder(Border.NO_BORDER);
                            t.SetWidth(UnitValue.CreatePointValue(pageWidthInPoints - borderMargin * PointSize));

                            document.Add(t);
                        }

                        if (storeVM.Contact.Web != null)
                        {
                            t = new Table(2)
                                .AddCell(new Cell().Add(new Paragraph("Web:")).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize)
                                .AddCell(new Cell().Add(new Paragraph(storeVM.Contact.Web)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize);

                            t.SetBorder(Border.NO_BORDER);
                            t.SetWidth(UnitValue.CreatePointValue(pageWidthInPoints - borderMargin * PointSize));

                            document.Add(t);
                        }

                        // Separator
                        document.Add(ls);

                        // Order items
                        t = new Table(3)
                            .AddCell(new Cell().Add(new Paragraph("Název / DPH")).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(mediumFontSize)
                            .AddCell(new Cell().Add(new Paragraph("Ks")).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(mediumFontSize)
                            .AddCell(new Cell().Add(new Paragraph("Cena")).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(mediumFontSize);

                        t.SetBorder(Border.NO_BORDER);


                        foreach (var item in orderItems)
                        {
                            t.AddCell(new Cell().Add(new Paragraph(_productService.GetProduct(item.ProductId).ShortName)).SetFontSize(smallFontSize).SetWidth(UnitValue.CreatePercentValue(70f)).SetBorder(Border.NO_BORDER)).SetFont(fontRegular);
                            t.AddCell(new Cell().Add(new Paragraph(((int)item.Quantity).ToString())).SetFontSize(smallFontSize).SetWidth(UnitValue.CreatePercentValue(10f)).SetBorder(Border.NO_BORDER)).SetFont(fontRegular);
                            t.AddCell(new Cell().Add(new Paragraph(_productService.GetProduct(item.ProductId).PriceSale.ToString())).SetFontSize(smallFontSize).SetWidth(UnitValue.CreatePercentValue(20f)).SetBorder(Border.NO_BORDER)).SetFont(fontRegular);
                            t.AddCell(new Cell().Add(new Paragraph($"{_vatService.GetVATRates().FirstOrDefault(v => v.Id == _productService.GetProduct(item.ProductId).VATRateId).Rate.ToString()}%")).SetFontSize(smallFontSize).SetWidth(UnitValue.CreatePercentValue(70f)).SetBorder(Border.NO_BORDER)).SetFont(fontRegular);
                            t.AddCell(new Cell().Add(new Paragraph(String.Empty)).SetFontSize(smallFontSize).SetWidth(UnitValue.CreatePercentValue(10f)).SetBorder(Border.NO_BORDER)).SetFont(fontRegular);
                            t.AddCell(new Cell().Add(new Paragraph(item.Price.ToString())).SetFontSize(mediumFontSize).SetWidth(UnitValue.CreatePercentValue(20f)).SetBorder(Border.NO_BORDER)).SetFont(fontRegular);
                        }

                        t.SetWidth(UnitValue.CreatePointValue(pageWidthInPoints - borderMargin * PointSize));

                        document.Add(t);

                        // Separator
                        document.Add(ls);

                        // Total price
                        t = new Table(2)
                            .AddCell(new Cell().Add(new Paragraph("Součet:")).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER)).SetFont(fontBold).SetFontSize(mediumFontSize)
                            .AddCell(new Cell().Add(new Paragraph($"{Math.Round((double)order.TotalPrice, 2).ToString()} Kč")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER)).SetFont(fontBold).SetFontSize(mediumFontSize);

                        t.SetBorder(Border.NO_BORDER);
                        t.SetWidth(UnitValue.CreatePointValue(pageWidthInPoints - borderMargin * PointSize));

                        document.Add(t);

                        // DPH Prices
                        foreach (var vatPrice in _saleService.GetOrderVATPrices(orderId))
                        {
                            t = new Table(2)
                                .AddCell(new Cell().Add(new Paragraph($"DPH {vatPrice.VATRate.ToString()}%")).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize)
                                .AddCell(new Cell().Add(new Paragraph($"{Math.Round((double)vatPrice.VATPrice, 2).ToString()} Kč")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize);

                            t.SetBorder(Border.NO_BORDER);
                            t.SetWidth(UnitValue.CreatePointValue(pageWidthInPoints - borderMargin * PointSize));

                            document.Add(t);

                        }

                        // Separator
                        document.Add(ls);

                        // Order id
                        t = new Table(2)
                            .AddCell(new Cell().Add(new Paragraph("Doklad č.:")).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize)
                            .AddCell(new Cell().Add(new Paragraph(order.Id.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize);

                        t.SetBorder(Border.NO_BORDER);
                        t.SetWidth(UnitValue.CreatePointValue(pageWidthInPoints - borderMargin * PointSize));

                        document.Add(t);

                        // Cashier id
                        t = new Table(2)
                            .AddCell(new Cell().Add(new Paragraph("Pokladní č.:")).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize)
                            .AddCell(new Cell().Add(new Paragraph(order.UserId.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize);

                        t.SetBorder(Border.NO_BORDER);
                        t.SetWidth(UnitValue.CreatePointValue(pageWidthInPoints - borderMargin * PointSize));

                        document.Add(t);

                        // Date and time
                        t = new Table(2)
                            .AddCell(new Cell().Add(new Paragraph("Datum:")).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize)
                            .AddCell(new Cell().Add(new Paragraph(order.DateTimeCreated.Date.ToString("dd.MM.yyyy"))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize);

                        t.SetBorder(Border.NO_BORDER);
                        t.SetWidth(UnitValue.CreatePointValue(pageWidthInPoints - borderMargin * PointSize));

                        document.Add(t);

                        t = new Table(2)
                            .AddCell(new Cell().Add(new Paragraph("Čas:")).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize)
                            .AddCell(new Cell().Add(new Paragraph(order.DateTimeCreated.ToString("HH:mm:ss"))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize);

                        t.SetBorder(Border.NO_BORDER);
                        t.SetWidth(UnitValue.CreatePointValue(pageWidthInPoints - borderMargin * PointSize));

                        document.Add(t);
                    }
                }
                return ms.ToArray();
            }
        }

        public async Task<string> SaveReceiptPDFAsync(byte[] pdfBytes, string fileName, string folderName)
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

        public async Task<string> GenerateReceiptAsync(int orderId)
        {
            byte[] pdfBytes = GenerateReceiptPDF(orderId);
            string folder = "OrderReceipts";
            string file = $"order{orderId}.pdf";

            string path = await SaveReceiptPDFAsync(pdfBytes, file, folder);

            return path;
        }

        public string GetReceiptPath(int orderId)
        {
            Order order = _saleService.GetOrder(orderId);

            if (order != null)
            {
                return order.ReceiptSrc;
            }

            return String.Empty;
        }
    }
}
