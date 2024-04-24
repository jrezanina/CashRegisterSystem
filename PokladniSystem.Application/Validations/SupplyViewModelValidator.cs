using FluentValidation;
using PokladniSystem.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Domain.Validations
{
    public class SupplyViewModelValidator : AbstractValidator<SupplyViewModel>
    {
        public SupplyViewModelValidator()
        {
            RuleFor(x => x.Supply.StoreId)
                .NotEmpty().WithMessage("Prosím, vyplňte prodejnu.")
                .Must((x, storeId) => x.Stores != null && x.Stores.Any(s => s.Id == storeId))
                .WithMessage("Vybraná prodejna není platná.");
        }
    }
}
