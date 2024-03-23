using Microsoft.AspNetCore.Identity;
using PokladniSystem.Application.Abstraction;
using PokladniSystem.Application.ViewModels;
using PokladniSystem.Domain.Entities;
using PokladniSystem.Infrastructure.Database;
using PokladniSystem.Infrastructure.Identity;
using PokladniSystem.Infrastructure.Identity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Application.Implementation
{
    public class AccountIdentityService : IAccountService
    {
        UserManager<User> _userManager;
        
        SignInManager<User> _signInManager;
        
        CRSDbContext _dbContext;


        public AccountIdentityService(UserManager<User> userManager, SignInManager<User> signInManager, CRSDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }

        public RegisterViewModel GetRegisterViewModel(string? username, string? password, string? repeatedPassword, Roles? role, int? storeId)
        {
            RegisterViewModel viewModel = new RegisterViewModel();

            viewModel.Username = username;
            viewModel.Password = password;
            viewModel.RepeatedPassword = repeatedPassword;
            viewModel.Role = role;
            viewModel.StoreId = storeId;
            viewModel.Stores = _dbContext.Stores.ToList();

            return viewModel;
        }

        public async Task<bool> Login(LoginViewModel vm)
        {
            var result = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, true, true);
            return result.Succeeded;
        }

        public Task Logout()
        {
            return _signInManager.SignOutAsync();
        }

        public async Task<string[]> Register(RegisterViewModel vm)
        {
            User user = new User()
            {
                UserName = vm.Username,
                StoreId = vm.StoreId
            };

            string[] errors = null;

            var result = await _userManager.CreateAsync(user, vm.Password);

            if (result.Succeeded)
            {
                var resultRole = await _userManager.AddToRoleAsync(user, vm.Role.ToString());

                if (resultRole.Succeeded == false)
                {
                    for (int i = 0; i < result.Errors.Count(); ++i)
                        result.Errors.Append(result.Errors.ElementAt(i));
                }
            }

            if (result.Errors != null && result.Errors.Count() > 0)
            {
                errors = new string[result.Errors.Count()];
                for (int i = 0; i < result.Errors.Count(); ++i)
                {
                    errors[i] = result.Errors.ElementAt(i).Description;
                }
            }

            return errors;
        }
    }
}
