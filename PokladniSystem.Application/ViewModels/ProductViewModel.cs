using PokladniSystem.Domain.Entities;
using PokladniSystem.Domain.Validations;
using PokladniSystem.Infrastructure.Identity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokladniSystem.Domain.Validations;

namespace PokladniSystem.Application.ViewModels
{
    public class ProductViewModel
    {
        public Product? Product { get; set; }
        public Dictionary<int, bool>? SelectedCategories { get; set; }
        
        public IList<Category> Categories { get; set; }
        public IList<VATRate> VATRates { get; set; }
    }
}
