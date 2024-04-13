using PokladniSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Application.Abstraction
{
    public interface IVATService
    {
        IList<VATRate> GetVATRates();
        void Create(VATRate vatRate);
        bool Delete(int id);
        void Edit(VATRate vatRate);
    }
}
