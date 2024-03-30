using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Domain.Entities
{
    public class Contact : Entity<int>
    {
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Web { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string? Street { get; set; }
        public string BuildingNumber { get; set; }

    }
}
