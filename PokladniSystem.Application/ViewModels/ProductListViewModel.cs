using PokladniSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Application.ViewModels
{
    public class ProductListViewModel
    {
        public IList<Product> Products { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string? EanCodeSearch { get; set; }
        public string? SellerCodeSearch { get; set; }
        public int? CategoryIdSearch { get; set; }
        public int? VATRateIdSearch { get; set; }
        public int PageSize = 10;

        public IList<Category> Categories { get; set; }
        public IList<VATRate> VATRates { get; set; }
    }
}
