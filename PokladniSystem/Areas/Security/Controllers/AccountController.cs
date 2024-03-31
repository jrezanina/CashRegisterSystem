using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokladniSystem.Application.Abstraction;
using PokladniSystem.Application.Implementation;
using PokladniSystem.Application.ViewModels;
using PokladniSystem.Domain.Entities;
using PokladniSystem.Domain.Validations;
using PokladniSystem.Infrastructure.Identity.Enums;
using PokladniSystem.Models;
using PokladniSystem.Web.Areas.Warehouse.Controllers;
using System.Diagnostics;

namespace PokladniSystem.Controllers
{
    [Area("Security")]
    public class AccountController : Controller
    {
        IAccountService _accountService;
        IValidator<LoginViewModel> _loginValidator;
        IValidator<RegisterViewModel> _registerValidator;

        public AccountController(IAccountService accountService, IValidator<LoginViewModel> loginValidator, IValidator<RegisterViewModel> registerValidator)
        {
            _accountService = accountService;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
        }

        /*public IActionResult Index()
        {
            IList<StoreViewModel> storeViewModels = _storeService.GetStoreViewModels();
            return View(storeViewModels);
        }*/

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {

            ValidationResult result = _loginValidator.Validate(viewModel);


            ModelState.Clear();
            if (result.IsValid)
            {
                bool isLogged = await _accountService.Login(viewModel);
                if (isLogged)
                    return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace(nameof(Controller), String.Empty), new { area = String.Empty });
                viewModel.LoginFailed = true;
            }

            result.AddToModelState(this.ModelState);
            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            return RedirectToAction(nameof(Login));
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        public IActionResult Register()
        {
            RegisterViewModel viewModel = _accountService.GetRegisterViewModel(null, null, null, null, null);
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            viewModel.StoreId = (viewModel.Role == Roles.Cashier) ? viewModel.StoreId : null;
            ValidationResult result = _registerValidator.Validate(viewModel);


            ModelState.Clear();
            if (result.IsValid)
            {
                string[] errors = await _accountService.Register(viewModel);
                if (errors == null)
                {
                    return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace(nameof(Controller), String.Empty), new { area = String.Empty });
                }
                ModelState.AddModelError("GeneralRegisterError", "Nepodařilo se zaregistrovat uživatele!");
            }

            result.AddToModelState(this.ModelState);
            return View(_accountService.GetRegisterViewModel(viewModel.Username, viewModel.Password, viewModel.RepeatedPassword, viewModel.Role, viewModel.StoreId));
        }
    }
}
