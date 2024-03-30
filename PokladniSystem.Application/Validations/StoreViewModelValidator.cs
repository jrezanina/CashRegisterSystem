using FluentValidation;
using PokladniSystem.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Domain.Validations
{
    public class StoreViewModelValidator : AbstractValidator<StoreViewModel>
    {
        public StoreViewModelValidator()
        {
            RuleFor(x => x.Store.Name)
                .NotEmpty().WithMessage("Prosím, vyplňte název prodejny.")
                .Length(2, 30).WithMessage("Délka názvu musí být v rozmezí 2 až 30 znaků.");
            RuleFor(x => x.Contact.Phone)
                .Matches(@"^[0-9 +]*$").When(x => !string.IsNullOrEmpty(x.Contact.Phone)).WithMessage("Telefonní číslo může obsahovat pouze číslice, mezery a znak '+'");
            RuleFor(x => x.Contact.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Contact.Email)).WithMessage("Nesprávný formát emailové adresy.");
            RuleFor(x => x.Contact.City)
                .NotEmpty().WithMessage("Prosím, vyplňte město prodejny.")
                .Length(2, 40).WithMessage("Délka názvu musí být v rozmezí 2 až 40 znaků.");
            RuleFor(x => x.Contact.PostalCode)
                .NotEmpty().WithMessage("Prosím, vyplňte PSČ prodejny.")
                .Matches(@"^\d{3}\s\d{2}$").WithMessage("PSČ musí mít formát '123 45'");   
            RuleFor(x => x.Contact.BuildingNumber)
                .NotEmpty().WithMessage("Prosím, vyplňte číslo popisné prodejny.")
                .Matches(@"^\d+\/?\d*$").WithMessage("Číslo popisné může obsahovat pouze číslice a lomítko.");         
        }
    }
}
