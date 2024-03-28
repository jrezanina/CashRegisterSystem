using FluentValidation;
using PokladniSystem.Application.ViewModels;
using PokladniSystem.Domain.Entities;
using PokladniSystem.Infrastructure.Identity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Application.Validations
{
    public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterViewModelValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Prosím, vyplňte uživatelské jméno.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Prosím, vyplňte heslo.");
            RuleFor(x => x.RepeatedPassword)
                .NotEmpty().WithMessage("Prosím, vyplňte heslo.")
                .Equal(x => x.Password).WithMessage("Hesla se neshodují.");
            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Prosím, vyberte roli.")
                .IsInEnum().WithMessage("Prosím, vyberte platnou roli.");

            When(x => x.Role == Roles.Cashier, () =>
            {
                RuleFor(x => x.StoreId)
                    .NotEmpty().WithMessage("Prosím, přiřaďte prodejnu.");
            });
        }
    }
}
