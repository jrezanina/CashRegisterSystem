using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Domain.Entities
{
    public class ProductCategory : Entity<int>
    {
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        [ForeignKey(nameof(Category))]
        public int CategoryId{ get; set; }

        public Product? Product { get; set; }
        public Category? Category { get; set; }
    }
}
