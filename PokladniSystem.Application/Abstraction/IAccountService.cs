using PokladniSystem.Application.ViewModels;
using PokladniSystem.Domain.Entities;
using PokladniSystem.Infrastructure.Identity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Application.Abstraction
{
    public interface IAccountService
    {
        Task<string[]> Register(RegisterViewModel vm);
        Task<bool> Login(LoginViewModel vm);
        Task Logout();
        Task AdminEdit(AccountAdminEditViewModel vm);
        RegisterViewModel GetRegisterViewModel(string? username, string? password, string? repeatedPassword, Roles? role, int? storeId);
        Task<AccountAdminEditViewModel> GetAccountAdminEditViewModel(string username);
        Task<IList<AccountViewModel>> GetAccountViewModels();
    }
}
