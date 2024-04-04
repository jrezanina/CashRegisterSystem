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
        IHttpContextAccessor _contextAccessor;
        IValidator<LoginViewModel> _loginValidator;
        IValidator<RegisterViewModel> _registerValidator;
        IValidator<AccountAdminEditViewModel> _accountAdminEditViewModelValidator;
        IValidator<AccountUserEditViewModel> _accountUserEditViewModelValidator;

        public AccountController(IAccountService accountService, IHttpContextAccessor contextAccessor, IValidator<LoginViewModel> loginValidator, 
            IValidator<RegisterViewModel> registerValidator, IValidator<AccountAdminEditViewModel> accountAdminEditViewModelValidator, 
            IValidator<AccountUserEditViewModel> accountUserEditViewModelValidator)
        {
            _accountService = accountService;
            _contextAccessor = contextAccessor;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
            _accountAdminEditViewModelValidator = accountAdminEditViewModelValidator;
            _accountUserEditViewModelValidator = accountUserEditViewModelValidator;
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> Index()
        {
            IList<AccountViewModel> userViewModels = await _accountService.GetAccountViewModelsAsync();
            return View(userViewModels);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            viewModel.Active = await _accountService.AccountActiveAsync(viewModel.Username);
            ValidationResult result = _loginValidator.Validate(viewModel);


            ModelState.Clear();
            if (result.IsValid)
            {
                bool isLogged = await _accountService.LoginAsync(viewModel);
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
        public async Task<IActionResult> Register()
        {
            RegisterViewModel viewModel = await _accountService.GetRegisterViewModelAsync(null, null, null, null, null);
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
                string[] errors = await _accountService.RegisterAsync(viewModel);
                if (errors == null)
                {
                    return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace(nameof(Controller), String.Empty), new { area = String.Empty });
                }
                ModelState.AddModelError("GeneralRegisterError", "Nepodařilo se zaregistrovat uživatele!");
            }

            result.AddToModelState(this.ModelState);
            return View(await _accountService.GetRegisterViewModelAsync(viewModel.Username, viewModel.Password, viewModel.RepeatedPassword, viewModel.Role, viewModel.StoreId));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UserEdit()
        {
            AccountUserEditViewModel viewModel = await _accountService.GetAccountUserEditViewModelAsync(_contextAccessor.HttpContext.User.Identity.Name);
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UserEdit(AccountUserEditViewModel viewModel)
        {
            viewModel.Username = _contextAccessor.HttpContext.User.Identity.Name;
            viewModel.OldPasswordFailed = !await _accountService.PasswordValidAsync(viewModel.Username, viewModel.OldPassword);
            ValidationResult result = _accountUserEditViewModelValidator.Validate(viewModel);


            ModelState.Clear();
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return View(viewModel);
            }

            await _accountService.UserEditAsync(viewModel);
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace(nameof(Controller), String.Empty), new { area = String.Empty });
        }

        [HttpGet]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> AdminEdit(string id)
        {
            AccountAdminEditViewModel viewModel = await _accountService.GetAccountAdminEditViewModelAsync(id);
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> AdminEdit(AccountAdminEditViewModel viewModel)
        {
            ValidationResult result = _accountAdminEditViewModelValidator.Validate(viewModel);


            ModelState.Clear();
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return View(viewModel);
            }

            await _accountService.AdminEditAsync(viewModel);
            return RedirectToAction(nameof(AccountController.Index));
        }
    }
}
