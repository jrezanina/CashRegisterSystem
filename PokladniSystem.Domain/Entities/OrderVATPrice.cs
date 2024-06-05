using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Domain.Entities
{
    public class OrderVATPrice : Entity<int>
    {
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        public int VATRate { get; set; }
        public double VATPrice { get; set; }

        public Order? Order { get; set; }

    }
}
