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
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Prosím, vyplňte uživatelské jméno.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Prosím, vyplňte heslo.");
            RuleFor(x => x.Active)
                .Equal(true).WithMessage("Účet není aktivní.");
        }
    }
}
