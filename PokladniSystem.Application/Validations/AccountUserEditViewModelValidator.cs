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
    public class AccountUserEditViewModelValidator : AbstractValidator<AccountUserEditViewModel>
    {
        public AccountUserEditViewModelValidator()
        {
            RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage("Prosím, vyplňte heslo.")
            .Empty().When(x => x.OldPasswordFailed).WithMessage("Neplatné heslo.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Prosím, vyplňte heslo.");
            RuleFor(x => x.RepeatedPassword)
                .NotEmpty().WithMessage("Prosím, vyplňte heslo.")
                .Equal(x => x.Password).WithMessage("Hesla se neshodují.");
        }
    }
}
