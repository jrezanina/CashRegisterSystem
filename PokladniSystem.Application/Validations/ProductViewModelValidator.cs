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
    public class ProductViewModelValidator : AbstractValidator<ProductViewModel>
    {
        public ProductViewModelValidator()
        {
            RuleFor(x => x.Product.Name)
                .NotEmpty().WithMessage("Prosím, vyplňte název produktu.")
                .Length(2, 40).WithMessage("Délka názvu musí být v rozmezí 2 až 40 znaků.");
            RuleFor(x => x.Product.ShortName)
                .NotEmpty().WithMessage("Prosím, vyplňte zkrácený název produktu.")
                .Length(2, 20).WithMessage("Délka názvu musí být v rozmezí 2 až 20 znaků.");
            RuleFor(x => x.Product.Description)
                .Length(0, 100).WithMessage("Délka popisu musí být kratší než 100 znaků.");
            RuleFor(x => x.Product.EanCode)
                .NotEmpty().When(x => string.IsNullOrEmpty(x.Product.SellerCode))
                .WithMessage("Kód EAN je povinný, pokud není vyplněný kód prodejce.")
                .Matches(@"^\d+$").When(x => !string.IsNullOrEmpty(x.Product.EanCode))
                .WithMessage("Kód EAN musí obsahovat pouze číslice.");
            RuleFor(x => x.Product.SellerCode)
                .NotEmpty().When(x => string.IsNullOrEmpty(x.Product.EanCode))
                .WithMessage("Kód prodejce je povinný, pokud není vyplněný kód EAN.");
            RuleFor(x => x.Product.PriceVATFree)
                .NotEmpty().WithMessage("Prosím, vyplňte cenu produktu.")
                .GreaterThan(0).WithMessage("Cena musí být kladné číslo.");
            RuleFor(x => x.Product.PriceVAT)
                .NotEmpty().WithMessage("Prosím, vyplňte cenu produktu.")
                .GreaterThan(0).WithMessage("Cena musí být kladné číslo.");
            RuleFor(x => x.Product.PriceSale)
                .NotEmpty().WithMessage("Prosím, vyplňte cenu produktu.")
                .GreaterThan(0).WithMessage("Cena musí být kladné číslo.");
            RuleFor(x => x.Product.VATRateId)
                .NotEmpty().WithMessage("Prosím, vyplňte sazbu DPH.")
                .Must((x, vatRateId) => x.VATRates != null && x.VATRates.Any(r => r.Id == vatRateId))
                .WithMessage("Vybraná sazba DPH není platná.");
        }
    }
}
