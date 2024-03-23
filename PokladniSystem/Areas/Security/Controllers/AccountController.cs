using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokladniSystem.Application.Abstraction;
using PokladniSystem.Application.Implementation;
using PokladniSystem.Application.ViewModels;
using PokladniSystem.Infrastructure.Identity.Enums;
using PokladniSystem.Models;
using System.Diagnostics;

namespace PokladniSystem.Controllers
{
    [Area("Security")]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private IAccountService _accountService;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                bool isLogged = await _accountService.Login(loginVM);
                if (isLogged)
                    return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace(nameof(Controller), String.Empty), new { area = String.Empty });

                loginVM.LoginFailed = true;
            }

            return View(loginVM);
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

            if (ModelState.IsValid)
            {
                string[] errors = await _accountService.Register(viewModel);

                if (errors == null)
                {
                    return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace(nameof(Controller), String.Empty), new { area = String.Empty });
                }
                ModelState.AddModelError("GeneralRegisterError", "Nepodařilo se zaregistrovat uživatele!");
            }

            return View(_accountService.GetRegisterViewModel(viewModel.Username, viewModel.Password, viewModel.RepeatedPassword, viewModel.Role, viewModel.StoreId));
        }
    }
}
