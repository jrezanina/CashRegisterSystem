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
        public string? Username { get; set; }
        public string? Password { get; set; }

        public bool Active { get; set; }
        public bool LoginFailed { get; set; }
    }
}
