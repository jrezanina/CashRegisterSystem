using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Domain.Entities
{
    public class Company : Entity<int>
    {
        public string Name { get; set; }
        [ForeignKey(nameof(Contact))]
        public int ContactId { get; set; }
        public string ICO { get; set; }
        public string? DIC { get; set; }

        // Phone, Email, Web, Street optional, City, PostalCode, BuildingNumber required
        public Contact? Contact { get; set; }
    }
}
