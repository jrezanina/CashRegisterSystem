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
        Task<string[]> RegisterAsync(RegisterViewModel vm);
        Task<bool> LoginAsync(LoginViewModel vm);
        Task Logout();
        Task<bool> PasswordValidAsync(string username, string password);
        Task<bool> AccountActiveAsync(string username);
        Task<bool> AdminEditAsync(AccountAdminEditViewModel vm);
        Task<bool> UserEditAsync(AccountUserEditViewModel vm);
        Task<RegisterViewModel> GetRegisterViewModelAsync(string? username, string? password, string? repeatedPassword, Roles? role, int? storeId);
        Task<AccountAdminEditViewModel> GetAccountAdminEditViewModelAsync(string username);
        Task<AccountUserEditViewModel> GetAccountUserEditViewModelAsync(string username);
        Task<IList<AccountViewModel>> GetAccountViewModelsAsync();

    }
}
