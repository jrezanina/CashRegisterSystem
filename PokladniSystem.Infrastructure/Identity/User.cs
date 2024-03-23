using Microsoft.AspNetCore.Identity;
using PokladniSystem.Domain.Entities;
using PokladniSystem.Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Infrastructure.Identity
{
    public class User : IdentityUser<int>, IUser
    {
        [ForeignKey(nameof(Store))]
        public virtual int? StoreId { get; set; }

        public Store? Store { get; set; }
    }
}
