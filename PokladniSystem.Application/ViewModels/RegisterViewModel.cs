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
        [Required(ErrorMessage = "Prosím, vyplňte uživatelské jméno.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Prosím, vyplňte heslo.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Prosím, vyplňte heslo.")]
        [Compare(nameof(Password), ErrorMessage = "Hesla se neshodují.")]
        public string? RepeatedPassword { get; set; }

        [Required(ErrorMessage = "Prosím, vyberte roli.")]
        [EnumDataType(typeof(Roles), ErrorMessage = "Prosím, vyberte platnou roli.")]
        public Roles? Role { get; set; }

        [RequiredIf(nameof(Role), Roles.Cashier, "Prosím, přiřaďte prodejnu.")]
        public int? StoreId { get; set; }

        public IList<Store>? Stores { get; set; }

        
    }
}
