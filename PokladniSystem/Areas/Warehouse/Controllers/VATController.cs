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
    public class VATController : Controller
    {
        IVATService _vatService;
        IValidator<VATRate> _vatValidator;

        public VATController(IVATService vatService, IValidator<VATRate> vatValidator)
        {
            _vatService = vatService;
            _vatValidator = vatValidator;
        }

        public IActionResult Index(bool deleteError)
        {
            IList<VATRate> vatRates = _vatService.GetVATRates();
            ViewBag.DeleteError = deleteError;
            return View(vatRates);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(VATRate vatRate)
        {
            ValidationResult result = _vatValidator.Validate(vatRate);

            ModelState.Clear();
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return View(vatRate);
            }

            _vatService.Create(vatRate);
            return RedirectToAction(nameof(VATController.Index));

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            VATRate vatRate = _vatService.GetVATRates().Where(v => v.Id == id).FirstOrDefault();
            return View(vatRate);
        }

        [HttpPost]
        public IActionResult Edit(VATRate vatRate)
        {
            ValidationResult result = _vatValidator.Validate(vatRate);


            ModelState.Clear();
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return View(vatRate);
            }

            _vatService.Edit(vatRate);
            return RedirectToAction(nameof(VATController.Index));
        }

        public IActionResult Delete(int id)
        {
            bool deleted = _vatService.Delete(id);

            if (deleted)
            {
                return RedirectToAction(nameof(VATController.Index));
            }
            else
                return RedirectToAction(nameof(VATController.Index), new { deleteError = true });
        }
    }
}
