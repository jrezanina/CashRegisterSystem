using FluentValidation;
using Org.BouncyCastle.Utilities;
using PokladniSystem.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Domain.Validations
{
    public class CompanyViewModelValidator : AbstractValidator<CompanyViewModel>
    {
        public CompanyViewModelValidator()
        {
            RuleFor(x => x.Company.Name)
                .NotEmpty().WithMessage("Prosím, vyplňte název společnosti.")
                .Length(2, 40).WithMessage("Délka názvu musí být v rozmezí 2 až 40 znaků.");
            RuleFor(x => x.Company.ICO)
                .NotEmpty().WithMessage("Prosím, vyplňte IČO společnosti.")
                .Must(ICOValidator).WithMessage("Neplatné IČO.");
            RuleFor(x => x.Company.DIC)
                .Matches(@"^[A-Z]{2}[0-9]{8}(?:[0-9]{2})?$").When(x => !string.IsNullOrEmpty(x.Company.DIC)).WithMessage("Neplatné DIČ");
            RuleFor(x => x.Contact.Phone)
                .Matches(@"^[0-9 +]*$").When(x => !string.IsNullOrEmpty(x.Contact.Phone)).WithMessage("Telefonní číslo může obsahovat pouze číslice, mezery a znak '+'");
            RuleFor(x => x.Contact.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Contact.Email)).WithMessage("Nesprávný formát emailové adresy.");
            RuleFor(x => x.Contact.City)
                .NotEmpty().WithMessage("Prosím, vyplňte město společnosti.")
                .Length(2, 40).WithMessage("Délka názvu musí být v rozmezí 2 až 40 znaků.");
            RuleFor(x => x.Contact.PostalCode)
                .NotEmpty().WithMessage("Prosím, vyplňte PSČ společnosti.")
                .Matches(@"^\d{3}\s\d{2}$").WithMessage("PSČ musí mít formát '123 45'");
            RuleFor(x => x.Contact.BuildingNumber)
                .NotEmpty().WithMessage("Prosím, vyplňte číslo popisné prodejny.")
                .Matches(@"^\d+\/?\d*$").WithMessage("Číslo popisné může obsahovat pouze číslice a lomítko.");
        }

        private bool ICOValidator(string ico)
        {
            if (ico == null)
                return false;
            
            int result;
            if (ico.Length != 8 || !int.TryParse(ico, out result))
                return false;

            int controlDigit = int.Parse(ico.Substring(7, 1));

            int[] weights = { 8, 7, 6, 5, 4, 3, 2 };
            int sum = 0;

            for (int i = 0; i < weights.Length; i++)
                sum += int.Parse(ico[i].ToString()) * weights[i];

            int remainder = sum % 11;
            int calcControlDigit = remainder == 0 ? 1 : remainder == 1 ? 0 : 11 - remainder;

            return controlDigit == calcControlDigit;

        }
    }
}
