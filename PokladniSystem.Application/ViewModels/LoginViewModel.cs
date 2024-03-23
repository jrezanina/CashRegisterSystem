using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Application.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Prosím, vyplňte uživatelské jméno!")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Prosím, vyplňte heslo!")]
        public string? Password { get; set; }
        public bool LoginFailed { get; set; }
    }
}
