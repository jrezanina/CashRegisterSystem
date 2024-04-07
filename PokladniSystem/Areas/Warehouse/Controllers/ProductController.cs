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

        public async Task<IActionResult> Index(int page = 1, string eanCode = null, string sellerCode = null)
        {
            ProductListViewModel viewModel = await _productService.GetProductListViewModelAsync(page, 10, eanCode, sellerCode);
            
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ProductViewModel viewModel = await _productService.GetProductViewModelAsync(null);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel viewModel) 
        {
            viewModel = await _productService.GetProductViewModelAsync(viewModel);
            ValidationResult result = _productViewModelValidator.Validate(viewModel);
            viewModel.Product.PriceVAT = Math.Round(viewModel.Product.PriceVATFree * (1 + viewModel.VATRates.FirstOrDefault(r => r.Id == viewModel.Product.VATRateId).Rate/100.0),2);
            viewModel.Product.PriceVATFree = Math.Round(viewModel.Product.PriceVATFree, 2);
            viewModel.Product.PriceSale = Math.Round(viewModel.Product.PriceSale, 2);

            ModelState.Clear();
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return View(viewModel);
            }

            _productService.Create(viewModel);
            return RedirectToAction(nameof(ProductController.Index));

        }

        /*[HttpGet]
        public IActionResult Edit(int id)
        {
            Category category = _categoryService.GetCategories().Where(c => c.Id == id).FirstOrDefault();
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            ValidationResult result = _categoryValidator.Validate(category);


            ModelState.Clear();
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return View(category);
            }

            _categoryService.Edit(category);
            return RedirectToAction(nameof(CategoryController.Index));
        }
        */
    }
}
