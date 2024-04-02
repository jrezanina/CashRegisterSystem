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
    public class AccountAdminEditViewModel
    {
        public string Username { get; set; }
        public string? Password { get; set; }
        public string? RepeatedPassword { get; set; }
        public bool Active {  get; set; }
    }
}
