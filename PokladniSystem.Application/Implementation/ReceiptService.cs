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

        string _rootPath;
        const float PointSize = 2.834f;

        public ReceiptService(string rootPath, CRSDbContext dbContext, ISaleService saleService, ICompanyService companyService, IStoreService storeService, IProductService productService)
        {
            _dbContext = dbContext;
            _saleService = saleService;
            _companyService = companyService;
            _storeService = storeService;
            _rootPath = rootPath;
            _productService = productService;

        }
        public byte[] GenerateReceiptPDF(int orderId)
        {
            Order order = _saleService.GetOrder(orderId);
            IList<OrderItem> orderItems = _saleService.GetOrderItems(orderId);
            CompanyViewModel companyVM = _companyService.GetCompanyViewModel();
            StoreViewModel storeVM = _storeService.GetStoreViewModels().FirstOrDefault(s => s.Store.Id == order.StoreId);

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

                        // Company header
                        Paragraph companyHeaderP = new Paragraph(companyVM.Company.Name).SetTextAlignment(TextAlignment.CENTER).SetFont(fontBold).SetFontSize(largeFontSize);
                        document.Add(companyHeaderP);

                        // Company address
                        string companyAddress = $"{(companyVM.Contact.Street != null ? companyVM.Contact.Street : companyVM.Contact.City)} {companyVM.Contact.BuildingNumber},\n" +
                            $"{companyVM.Contact.PostalCode} {companyVM.Contact.City}";
                        Paragraph companyAddressP = new Paragraph(companyAddress).SetTextAlignment(TextAlignment.CENTER).SetFont(fontRegular).SetFontSize(smallFontSize);
                        document.Add(companyAddressP);

                        // Company ICO and DIC
                        string icoDic = $"IČO: {companyVM.Company.ICO}{(companyVM.Company.DIC != null ? $", DIČ: {companyVM.Company.DIC}" : string.Empty)}";
                        Paragraph icoDicP = new Paragraph(icoDic).SetTextAlignment(TextAlignment.CENTER).SetFont(fontRegular).SetFontSize(smallFontSize);
                        document.Add(icoDicP);

                        // Separator
                        LineSeparator lineSeparator = new LineSeparator(new SolidLine()).SetWidth(UnitValue.CreatePercentValue(100));
                        Paragraph lineSeparatorP = new Paragraph().Add(lineSeparator);
                        document.Add(lineSeparatorP);

                        // Store header
                        Paragraph storeHeaderP = new Paragraph("Prodejna:").SetTextAlignment(TextAlignment.CENTER).SetFont(fontBold).SetFontSize(mediumFontSize);
                        document.Add(storeHeaderP);

                        string storeName = $"{storeVM.Store.Name}";
                        Paragraph storeNameP = new Paragraph(storeName).SetTextAlignment(TextAlignment.CENTER).SetFont(fontRegular).SetFontSize(smallFontSize);
                        document.Add(storeNameP);

                        // Store address
                        string storeAddress = $"{(storeVM.Contact.Street != null ? storeVM.Contact.Street : storeVM.Contact.City)} {storeVM.Contact.BuildingNumber},\n" +
                            $"{(storeVM.Contact.PostalCode)} {storeVM.Contact.City}";
                        Paragraph storeAddressP = new Paragraph(storeAddress).SetTextAlignment(TextAlignment.CENTER).SetFont(fontRegular).SetFontSize(smallFontSize);
                        document.Add(storeAddressP);

                        // Separator
                        document.Add(lineSeparatorP);

                        // Order items
                        Table orderItemsT = new Table(3)
                            .AddCell(new Cell().Add(new Paragraph("Název")).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(mediumFontSize)
                            .AddCell(new Cell().Add(new Paragraph("Ks")).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(mediumFontSize)
                            .AddCell(new Cell().Add(new Paragraph("Cena")).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(mediumFontSize);

                        orderItemsT.SetBorder(Border.NO_BORDER);


                        foreach (var item in orderItems)
                        {
                            orderItemsT.AddCell(new Cell().Add(new Paragraph(_productService.GetProduct(item.ProductId).ShortName)).SetFontSize(smallFontSize).SetWidth(UnitValue.CreatePercentValue(70f)).SetBorder(Border.NO_BORDER)).SetFont(fontRegular);
                            orderItemsT.AddCell(new Cell().Add(new Paragraph(((int)item.Quantity).ToString())).SetFontSize(smallFontSize).SetWidth(UnitValue.CreatePercentValue(10f)).SetBorder(Border.NO_BORDER)).SetFont(fontRegular);
                            orderItemsT.AddCell(new Cell().Add(new Paragraph(_productService.GetProduct(item.ProductId).PriceSale.ToString())).SetFontSize(smallFontSize).SetWidth(UnitValue.CreatePercentValue(20f)).SetBorder(Border.NO_BORDER)).SetFont(fontRegular);
                            orderItemsT.AddCell(new Cell().Add(new Paragraph(String.Empty)).SetFontSize(smallFontSize).SetWidth(UnitValue.CreatePercentValue(70f)).SetBorder(Border.NO_BORDER)).SetFont(fontRegular);
                            orderItemsT.AddCell(new Cell().Add(new Paragraph(String.Empty)).SetFontSize(smallFontSize).SetWidth(UnitValue.CreatePercentValue(10f)).SetBorder(Border.NO_BORDER)).SetFont(fontRegular);
                            orderItemsT.AddCell(new Cell().Add(new Paragraph(item.Price.ToString())).SetFontSize(mediumFontSize).SetWidth(UnitValue.CreatePercentValue(20f)).SetBorder(Border.NO_BORDER)).SetFont(fontRegular);
                        }

                        orderItemsT.SetWidth(UnitValue.CreatePointValue(pageWidthInPoints - borderMargin * PointSize));

                        document.Add(orderItemsT);

                        // Separator
                        document.Add(lineSeparatorP);

                        // TotalPrice
                        Table totalPriceT = new Table(2)
                            .AddCell(new Cell().Add(new Paragraph("Součet:")).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER)).SetFont(fontBold).SetFontSize(mediumFontSize)
                            .AddCell(new Cell().Add(new Paragraph($"{Math.Round((double)order.TotalPrice, 2).ToString()} Kč")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER)).SetFont(fontBold).SetFontSize(mediumFontSize);

                        totalPriceT.SetBorder(Border.NO_BORDER);
                        totalPriceT.SetWidth(UnitValue.CreatePointValue(pageWidthInPoints - borderMargin * PointSize));
                     
                        document.Add(totalPriceT);

                        // Separator
                        document.Add(lineSeparatorP);

                        // Order id
                        Table orderIdT = new Table(2)
                            .AddCell(new Cell().Add(new Paragraph("Doklad č.:")).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize)
                            .AddCell(new Cell().Add(new Paragraph(order.Id.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize);

                        orderIdT.SetBorder(Border.NO_BORDER);
                        orderIdT.SetWidth(UnitValue.CreatePointValue(pageWidthInPoints - borderMargin * PointSize));

                        document.Add(orderIdT);

                        // Cashier id
                        Table cashierIdT = new Table(2)
                            .AddCell(new Cell().Add(new Paragraph("Pokladní č.:")).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize)
                            .AddCell(new Cell().Add(new Paragraph(order.UserId.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize);

                        cashierIdT.SetBorder(Border.NO_BORDER);
                        cashierIdT.SetWidth(UnitValue.CreatePointValue(pageWidthInPoints - borderMargin * PointSize));

                        document.Add(cashierIdT);

                        // Date and time
                        Table dateT = new Table(2)
                            .AddCell(new Cell().Add(new Paragraph("Datum:")).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize)
                            .AddCell(new Cell().Add(new Paragraph(order.DateTimeCreated.Date.ToString("dd.MM.yyyy"))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize);

                        dateT.SetBorder(Border.NO_BORDER);
                        dateT.SetWidth(UnitValue.CreatePointValue(pageWidthInPoints - borderMargin * PointSize));

                        document.Add(dateT);

                        Table timeT = new Table(2)
                            .AddCell(new Cell().Add(new Paragraph("Čas:")).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize)
                            .AddCell(new Cell().Add(new Paragraph(order.DateTimeCreated.ToString("HH:mm:ss"))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER)).SetFont(fontRegular).SetFontSize(smallFontSize);

                        timeT.SetBorder(Border.NO_BORDER);
                        timeT.SetWidth(UnitValue.CreatePointValue(pageWidthInPoints - borderMargin * PointSize));
                        
                        document.Add(timeT);
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

            if ( order != null)
            {
                return order.ReceiptSrc;
            }

            return String.Empty;
        }
    }
}
