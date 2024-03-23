using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Domain.Entities
{
    public class Store : Entity<int>
    {
        public string Name { get; set; }

    }
}
