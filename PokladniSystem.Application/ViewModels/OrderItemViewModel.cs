using PokladniSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Application.ViewModels
{
    public class OrderItemViewModel
    {
        public Product? Product { get; set; }
        public int? Quantity { get; set; }
    }
}
