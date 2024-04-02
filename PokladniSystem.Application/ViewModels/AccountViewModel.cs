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
    public class AccountViewModel
    {
        public string Username { get; set; }
        public string RoleName { get; set; }
        public string? StoreName { get; set; }
        public bool Active { get; set; }
    }
}
