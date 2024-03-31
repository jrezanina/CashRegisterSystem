using PokladniSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Application.ViewModels
{
    public class CompanyViewModel
    {
        public Company Company { get; set; }
        public Contact Contact { get; set; }
    }
}
