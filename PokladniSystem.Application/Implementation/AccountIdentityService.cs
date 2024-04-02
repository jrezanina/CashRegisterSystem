using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PokladniSystem.Application.Abstraction;
using PokladniSystem.Application.ViewModels;
using PokladniSystem.Domain.Entities;
using PokladniSystem.Infrastructure.Database;
using PokladniSystem.Infrastructure.Identity;
using PokladniSystem.Infrastructure.Identity.Enums;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Application.Implementation
{
    public class AccountIdentityService : IAccountService
    {
        UserManager<User> _userManager;
        RoleManager<Role> _roleManager;
        SignInManager<User> _signInManager;
        CRSDbContext _dbContext;


        public AccountIdentityService(UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager, CRSDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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

        public async Task<IList<AccountViewModel>> GetAccountViewModels()
        {
            IList<AccountViewModel> viewModels = new List<AccountViewModel>();
            IList<User> users = await _userManager.Users.ToListAsync();

            foreach (User user in users)
            {
                IList<string> userRoles = await _userManager.GetRolesAsync(user);
                Store? store = _dbContext.Stores.FirstOrDefault(s => s.Id == user.StoreId);
                viewModels.Add(new AccountViewModel()
                {
                    Username = user.UserName,
                    RoleName = userRoles.FirstOrDefault(),
                    StoreName = (store != null) ? store.Name : null,
                    Active = user.Active
                });
            }

            return viewModels;

        }

        public async Task<AccountAdminEditViewModel> GetAccountAdminEditViewModel(string username)
        {
            AccountAdminEditViewModel viewModel = new AccountAdminEditViewModel();

            User user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);

            viewModel.Username = username;
            viewModel.Password = string.Empty;
            viewModel.Active = user.Active;

            return viewModel;

        }

        public async Task AdminEdit(AccountAdminEditViewModel vm)
        {
            User user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == vm.Username);

            if (user != null)
            {
                user.Active = vm.Active;
                if (vm.Password != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, vm.Password);
                }
                await _userManager.UpdateAsync(user);
                await _dbContext.SaveChangesAsync();
            }
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
