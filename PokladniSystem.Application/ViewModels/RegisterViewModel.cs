using PokladniSystem.Domain.Entities;
using PokladniSystem.Domain.Validations;
using PokladniSystem.Infrastructure.Identity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokladniSystem.Domain.Validations;

namespace PokladniSystem.Application.ViewModels
{
    public class RegisterViewModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? RepeatedPassword { get; set; }
        public Roles? Role { get; set; }
        public int? StoreId { get; set; }

        public IList<Store>? Stores { get; set; }

        
    }
}
