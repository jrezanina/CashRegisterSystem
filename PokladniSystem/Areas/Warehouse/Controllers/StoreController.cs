using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokladniSystem.Application.Abstraction;
using PokladniSystem.Application.ViewModels;
using PokladniSystem.Domain.Entities;
using PokladniSystem.Infrastructure.Identity.Enums;

namespace PokladniSystem.Web.Areas.Warehouse.Controllers
{
    [Area("Warehouse")]
    [Authorize(Roles = nameof(Roles.WarehouseAccountant))]
    public class StoreController : Controller
    {
        IStoreService _storeService;
        IContactService _contactService;
        IValidator<StoreViewModel> _storeViewModelValidator;
        public StoreController(IStoreService storeService, IContactService contactService, IValidator<StoreViewModel> storeViewModelValidator)
        {
            _storeService = storeService;
            _contactService = contactService;
            _storeViewModelValidator = storeViewModelValidator;
        }

        public IActionResult Index()
        {
            IList<StoreViewModel> storeViewModels = _storeService.GetStoreViewModels();
            return View(storeViewModels);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(StoreViewModel viewModel)
        {

            ValidationResult result = _storeViewModelValidator.Validate(viewModel);

            ModelState.Clear();
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return View(viewModel);
            }
            int contactId = _contactService.Create(viewModel.Contact);
            viewModel.Store.ContactId = contactId;
            _storeService.Create(viewModel.Store);
            return RedirectToAction(nameof(StoreController.Index));

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            StoreViewModel storeViewModel = _storeService.GetStoreViewModels().Where(s => s.Store.Id == id).FirstOrDefault();
            return View(storeViewModel);
        }

        [HttpPost]
        public IActionResult Edit(StoreViewModel viewModel)
        {
            ValidationResult result = _storeViewModelValidator.Validate(viewModel);


            ModelState.Clear();
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return View(viewModel);
            }

            //viewModel.Contact.Id = viewModel.Store.ContactId;

            if (_contactService.Edit(viewModel.Contact))
                _storeService.Edit(viewModel.Store);
            else
                return View(viewModel);

            return RedirectToAction(nameof(StoreController.Index));
        }
    }
}