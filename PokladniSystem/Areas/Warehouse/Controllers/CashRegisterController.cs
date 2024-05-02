using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PokladniSystem.Application.Abstraction;
using PokladniSystem.Application.Implementation;
using PokladniSystem.Application.ViewModels;
using PokladniSystem.Domain.Entities;
using PokladniSystem.Infrastructure.Identity;
using PokladniSystem.Infrastructure.Identity.Enums;
using System.Globalization;
using System.Net;
using System.Text;

namespace PokladniSystem.Web.Areas.Warehouse.Controllers
{
    [Area("Warehouse")]
    [Authorize(Roles = nameof(Roles.Cashier))]
    public class CashRegisterController : Controller
    {
        ISaleService _saleService;
        IProductService _productService;
        IAccountService _accountService;
        IReceiptService _receiptService;

        public CashRegisterController(ISaleService saleService, IProductService productService, IAccountService accountService, IReceiptService receiptService)
        {
            _saleService = saleService;
            _productService = productService;
            _accountService = accountService;
            _receiptService = receiptService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetOrderItem(string eanCode, string sellerCode, int quantity)
        {
            User user = await _accountService.GetUserAsync(User.Identity.Name);

            if (quantity <= 0)
            {
                return BadRequest("Množství zboží musí být kladné celé číslo");
            }

            OrderItemViewModel viewModel = _saleService.GetOrderItemViewModel(eanCode, sellerCode, quantity);

            if (viewModel == null)
            {
                return BadRequest("Produkt nenalezen");
            }

            if (!_saleService.IsInStock(viewModel.Product.Id, user.StoreId, quantity))
            {
                return BadRequest("Produkt není v požadovaném množství skladem");
            }

            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CompleteOrder([FromBody] List<OrderItemDataViewModel> orderData)
        {
            User user = await _accountService.GetUserAsync(User.Identity.Name);
            double priceTotal = 0;

            if (orderData.Count == 0)
            {
                return BadRequest("Seznam zboží je prázdný.");
            }

            foreach (var item in orderData)
            {
                OrderItemViewModel itemVM = _saleService.GetOrderItemViewModel(item.EanCode, item.SellerCode, item.Quantity);

                if (itemVM == null)
                {
                    return BadRequest($"Produkt s kódem {((item.EanCode != String.Empty) ? ($"EAN: {item.EanCode} ") : String.Empty)}{((item.SellerCode != String.Empty) ? ($"prodejce: {item.SellerCode}") : String.Empty)}nenalezen");
                }
                if (item.Quantity <= 0)
                {
                    return BadRequest($"Množství zboží u produktu s kódem {((item.EanCode != String.Empty) ? ($"EAN: {item.EanCode} ") : String.Empty)}{((item.SellerCode != String.Empty) ? ($"prodejce: {item.SellerCode}") : String.Empty)}musí být kladné celé číslo");
                }
                if (!_saleService.IsInStock(itemVM.Product.Id, user.StoreId, item.Quantity))
                {
                    return BadRequest($"Produkt s kódem {((item.EanCode != String.Empty) ? ($"EAN: {item.EanCode} ") : String.Empty)}{((item.SellerCode != String.Empty) ? ($"prodejce: {item.SellerCode}") : String.Empty)}není v požadovaném množství skladem");
                }
                priceTotal+= Math.Round(itemVM.Product.PriceSale * itemVM.Quantity, 2); 
            }

            return Ok(priceTotal);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitOrder([FromBody] List<OrderItemDataViewModel> orderData)
        {
            User user = await _accountService.GetUserAsync(User.Identity.Name);
            IList<OrderItemViewModel> itemVMs = new List<OrderItemViewModel>();

            if (orderData.Count == 0)
            {
                return BadRequest("Seznam zboží je prázdný.");
            }

            foreach (var item in orderData)
            {
                OrderItemViewModel itemVM = _saleService.GetOrderItemViewModel(item.EanCode, item.SellerCode, item.Quantity);

                if (itemVM == null)
                {
                    return BadRequest($"Produkt s kódem {((item.EanCode != String.Empty) ? ($"EAN: {item.EanCode} ") : String.Empty)}{((item.SellerCode != String.Empty) ? ($"prodejce: {item.SellerCode}") : String.Empty)}nenalezen");
                }
                if (item.Quantity <= 0)
                {
                    return BadRequest($"Množství zboží u produktu s kódem {((item.EanCode != String.Empty) ? ($"EAN: {item.EanCode} ") : String.Empty)}{((item.SellerCode != String.Empty) ? ($"prodejce: {item.SellerCode}") : String.Empty)}musí být kladné celé číslo");
                }
                if (!_saleService.IsInStock(itemVM.Product.Id, user.StoreId, item.Quantity))
                {
                    return BadRequest($"Produkt s kódem {((item.EanCode != String.Empty) ? ($"EAN: {item.EanCode} ") : String.Empty)}{((item.SellerCode != String.Empty) ? ($"prodejce: {item.SellerCode}") : String.Empty)}není v požadovaném množství skladem");
                }
                itemVMs.Add(itemVM);
            }

            foreach (var item in itemVMs)
            {
                _productService.StockUp(new SupplyViewModel() { Supply = new Supply() { ProductId = item.Product.Id, Quantity = -item.Quantity, StoreId = user.StoreId } });
            }

            int orderId = _saleService.CreateOrder(itemVMs, user);
            string receiptPath = await _receiptService.GenerateReceiptAsync(orderId);

            _saleService.SetOrderReceiptPath(orderId, receiptPath);

            return Ok(orderId);
        }
    }
}
