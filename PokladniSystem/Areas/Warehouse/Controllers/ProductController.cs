using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using PokladniSystem.Application.Abstraction;
using PokladniSystem.Application.Implementation;
using PokladniSystem.Application.ViewModels;
using PokladniSystem.Domain.Entities;
using PokladniSystem.Domain.Validations;
using PokladniSystem.Infrastructure.Identity.Enums;

namespace PokladniSystem.Web.Areas.Warehouse.Controllers
{
    [Area("Warehouse")]
    [Authorize]
    public class ProductController : Controller
    {
        IProductService _productService;
        IValidator<ProductViewModel> _productViewModelValidator;
        public ProductController(IProductService productService, IValidator<ProductViewModel> productViewModelValidator)
        {
            _productService = productService;
            _productViewModelValidator = productViewModelValidator;
        }

        public async Task<IActionResult> Index(ProductListViewModel viewModel)
        {
            viewModel = await _productService.GetProductListViewModelAsync(viewModel);

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = nameof(Roles.WarehouseAccountant))]
        public async Task<IActionResult> Create()
        {
            ProductViewModel viewModel = await _productService.GetProductViewModelAsync(null);
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = nameof(Roles.WarehouseAccountant))]
        public async Task<IActionResult> Create(ProductViewModel viewModel)
        {
            viewModel = await _productService.GetProductViewModelAsync(viewModel);
            ValidationResult result = _productViewModelValidator.Validate(viewModel);

            ModelState.Clear();
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return View(viewModel);
            }

            viewModel.Product.PriceVAT = Math.Round(viewModel.Product.PriceVATFree * (1 + viewModel.VATRates.FirstOrDefault(r => r.Id == viewModel.Product.VATRateId).Rate / 100.0), 2);
            viewModel.Product.PriceVATFree = Math.Round(viewModel.Product.PriceVATFree, 2);
            viewModel.Product.PriceSale = Math.Round(viewModel.Product.PriceSale, 2);

            _productService.Create(viewModel);
            return RedirectToAction(nameof(ProductController.Index));

        }

        [HttpGet]
        [Authorize(Roles = nameof(Roles.WarehouseAccountant) + ", " + nameof(Roles.Manager))]
        public async Task<IActionResult> Edit(int id)
        {
            ProductViewModel viewModel = await _productService.GetProductViewModelAsync(id);
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = nameof(Roles.WarehouseAccountant) + ", " + nameof(Roles.Manager))]
        public async Task<IActionResult> Edit(ProductViewModel viewModel)
        {
            viewModel = await _productService.GetProductViewModelAsync(viewModel);
            ValidationResult result = _productViewModelValidator.Validate(viewModel);

            ModelState.Clear();
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return View(viewModel);
            }

            viewModel.Product.PriceSale = Math.Round(viewModel.Product.PriceSale, 2);

            if (User.IsInRole(nameof(Roles.WarehouseAccountant)))
            {
                viewModel.Product.PriceVAT = Math.Round(viewModel.Product.PriceVATFree * (1 + viewModel.VATRates.FirstOrDefault(r => r.Id == viewModel.Product.VATRateId).Rate / 100.0), 2);
                viewModel.Product.PriceVATFree = Math.Round(viewModel.Product.PriceVATFree, 2);
                _productService.Edit(viewModel);
            }
            else
            {
                _productService.EditPriceSale(viewModel);
            }
            return RedirectToAction(nameof(ProductController.Index));
        }
    }
}
