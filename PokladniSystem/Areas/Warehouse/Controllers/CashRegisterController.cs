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
using System.Net;

namespace PokladniSystem.Web.Areas.Warehouse.Controllers
{
    [Area("Warehouse")]
    [Authorize(Roles = nameof(Roles.Cashier))]
    public class CashRegisterController : Controller
    {
        ISaleService _saleService;
        IAccountService _accountService;

        public CashRegisterController(ISaleService saleService, IAccountService accountService)
        {
            _saleService = saleService;
            _accountService = accountService;
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
    }
}
