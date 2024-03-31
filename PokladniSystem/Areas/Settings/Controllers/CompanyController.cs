using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokladniSystem.Application.Abstraction;
using PokladniSystem.Application.ViewModels;
using PokladniSystem.Controllers;
using PokladniSystem.Domain.Entities;
using PokladniSystem.Infrastructure.Identity.Enums;

namespace PokladniSystem.Web.Areas.Warehouse.Controllers
{
    [Area("Settings")]
    [Authorize(Roles = nameof(Roles.Admin))]
    public class CompanyController : Controller
    {
        ICompanyService _companyService;
        IContactService _contactService;
        IValidator<CompanyViewModel> _companyViewModelValidator;
        public CompanyController(ICompanyService companyService, IContactService contactService, IValidator<CompanyViewModel> companyViewModelValidator)
        {
            _companyService = companyService;
            _contactService = contactService;
            _companyViewModelValidator = companyViewModelValidator;
        }

        [HttpGet]
        public IActionResult Edit()
        {
            CompanyViewModel companyViewModel = _companyService.GetCompanyViewModel();
            return View(companyViewModel);
        }

        [HttpPost]
        public IActionResult Edit(CompanyViewModel viewModel)
        {
            ValidationResult result = _companyViewModelValidator.Validate(viewModel);


            ModelState.Clear();
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return View(viewModel);
            }

            if (_contactService.Edit(viewModel.Contact))
                _companyService.Edit(viewModel.Company);
            else
                return View(viewModel);

            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace(nameof(Controller), String.Empty), new { area = String.Empty });
        }
    }
}
