using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Domain.Entities
{
    public class Supply : Entity<int>
    {
        public int Quantity { get; set; }
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        [ForeignKey(nameof(Store))]
        public int? StoreId{ get; set; }

        public Product? Product { get; set; }
        public Store? Store { get; set; }
    }
}
