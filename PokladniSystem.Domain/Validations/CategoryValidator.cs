using FluentValidation;
using PokladniSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Domain.Validations
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Prosím, vyplňte název kategorie.")
                .Length(2, 30).WithMessage("Délka názvu musí být v rozmezí 2 až 30 znaků.");
        }
    }
}
