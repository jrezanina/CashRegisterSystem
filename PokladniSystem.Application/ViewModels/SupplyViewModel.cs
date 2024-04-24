using PokladniSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Application.ViewModels
{
    public class SupplyViewModel
    {
        public Supply? Supply { get; set; }
        public IList<Store>? Stores { get; set; }
        public Product Product { get; set; }
    }
}
