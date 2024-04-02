using FluentValidation;
using PokladniSystem.Application.ViewModels;
using PokladniSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Application.Validations
{
    public class AccountAdminEditViewModelValidator : AbstractValidator<AccountAdminEditViewModel>
    {
        public AccountAdminEditViewModelValidator()
        {
            RuleFor(x => x.RepeatedPassword)
                .Equal(x => x.Password).WithMessage("Hesla se neshodují.");
        }
    }
}
