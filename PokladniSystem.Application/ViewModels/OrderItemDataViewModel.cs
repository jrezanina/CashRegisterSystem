using PokladniSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Application.ViewModels
{
    public class OrderItemDataViewModel
    {
        public string EanCode { get; set; }
        public string SellerCode { get; set; }
        public int Quantity { get; set; }
    }
}
