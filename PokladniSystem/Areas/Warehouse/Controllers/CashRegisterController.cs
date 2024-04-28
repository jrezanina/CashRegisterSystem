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
using PokladniSystem.Infrastructure.Identity.Enums;
using System.Net;

namespace PokladniSystem.Web.Areas.Warehouse.Controllers
{
    [Area("Warehouse")]
    [Authorize(Roles = nameof(Roles.Cashier))]
    public class CashRegisterController : Controller
    {
        ISaleService _saleService;

        public CashRegisterController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetOrderItem(string eanCode, string sellerCode, int quantity)
        {
            if (quantity <= 0)
            {
                return BadRequest("Množství zboží musí být kladné celé číslo");
            }

            OrderItemViewModel viewModel = _saleService.GetOrderItemViewModel(eanCode, sellerCode, quantity);

            if (viewModel == null)
            {
                return BadRequest("Produkt nenalezen");
            }

            return Ok(viewModel);
        }
    }
}
