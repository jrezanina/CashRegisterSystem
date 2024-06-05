using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokladniSystem.Application.Abstraction;
using PokladniSystem.Application.Implementation;
using PokladniSystem.Domain.Entities;
using PokladniSystem.Infrastructure.Identity.Enums;

namespace PokladniSystem.Web.Areas.Warehouse.Controllers
{
    [Area("Warehouse")]
    [Authorize(Roles = nameof(Roles.WarehouseAccountant))]
    public class CategoryController : Controller
    {
        IHtmlSanitizerService _htmlSanitizerService;
        ICategoryService _categoryService;
        IValidator<Category> _categoryValidator;
        public CategoryController(IHtmlSanitizerService htmlSanitizerService, ICategoryService categoryService, IValidator<Category> categoryValidator)
        {
            _htmlSanitizerService = htmlSanitizerService;
            _categoryService = categoryService;
            _categoryValidator = categoryValidator;
        }

        public IActionResult Index()
        {
            IList<Category> categories = _categoryService.GetCategories();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category) 
        {
            category = _htmlSanitizerService.Sanitize(category);
            ValidationResult result = _categoryValidator.Validate(category);


            ModelState.Clear();
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return View(category);
            }

            _categoryService.Create(category);
            return RedirectToAction(nameof(CategoryController.Index));

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Category category = _categoryService.GetCategories().Where(c => c.Id == id).FirstOrDefault();
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            category = _htmlSanitizerService.Sanitize(category);
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

        public IActionResult Delete(int id)
        {
            bool deleted = _categoryService.Delete(id);

            if (deleted)
            {
                return RedirectToAction(nameof(CategoryController.Index));
            }
            else
                return NotFound();
        }
    }
}
