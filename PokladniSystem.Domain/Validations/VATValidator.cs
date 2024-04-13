using FluentValidation;
using PokladniSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Domain.Validations
{
    public class VATRateValidator : AbstractValidator<VATRate>
    {
        public VATRateValidator()
        {
            RuleFor(x => x.Rate)
                .GreaterThanOrEqualTo(0).WithMessage("Sazba DPH musí být celé nezáporné číslo.");
        }
    }
}
