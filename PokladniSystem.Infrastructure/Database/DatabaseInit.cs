using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PokladniSystem.Domain.Entities;
using PokladniSystem.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Infrastructure.Database
{
    internal class DatabaseInit
    {
        public IList<VATRate> GetDefaultVATRates()
        {
            IList<VATRate> rates = new List<VATRate>();

            rates.Add(new VATRate { Id = 1, Rate = 0  });
            rates.Add(new VATRate { Id = 2, Rate = 12 });
            rates.Add(new VATRate { Id = 3, Rate = 21 });

            return rates;
        }

        public IList<Store> GetDefaultStores()
        {
            IList<Store> stores = new List<Store>();

            stores.Add(new Store { Id = 1, Name = "Zlín" });
            stores.Add(new Store { Id = 2, Name = "Olomouc" });

            return stores;
        }
    }
}
