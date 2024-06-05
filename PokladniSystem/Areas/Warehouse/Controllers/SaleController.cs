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
    [Authorize(Roles = nameof(Roles.WarehouseAccountant) + ", " + nameof(Roles.Manager) + ", " + nameof(Roles.Cashier))]
    public class SaleController : Controller
    {
        ISaleService _saleService;
        IAccountService _accountService;
        IReceiptService _receiptService;

        public SaleController(ISaleService saleService, IAccountService accountService, IReceiptService receiptService)
        {
            _saleService = saleService;
            _accountService = accountService;
            _receiptService = receiptService;
        }

        public async Task<IActionResult> Index(OrderListViewModel viewModel)
        {
            if (User.IsInRole(nameof(Roles.Cashier)))
            {
                User user = await _accountService.GetUserAsync(User.Identity.Name);
                viewModel.StoreIdSearch = user.StoreId;
            }

            viewModel = await _saleService.GetOrderListViewModelAsync(viewModel);

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult GetReceipt(int orderId)
        {
            var receiptPath = _receiptService.GetReceiptPath(orderId);

            return File(receiptPath, "application/pdf");
        }
    }
}
