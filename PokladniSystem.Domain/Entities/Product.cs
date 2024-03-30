using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Domain.Entities
{
    public class Product : Entity<int>
    {
        public string? EanCode {  get; set; }
        public string? SellerCode {  get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string? Description { get; set; }
        public double PriceVATFree { get; set; }
        public double PriceVAT { get; set; }
        public double PriceSale { get; set; }
        [ForeignKey(nameof(VATRate))]
        public int VATRateId { get; set; }

        public VATRate? VATRate { get; set; }

    }
}
