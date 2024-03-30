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

            rates.Add(new VATRate { Id = 1, Rate = 0 });
            rates.Add(new VATRate { Id = 2, Rate = 12 });
            rates.Add(new VATRate { Id = 3, Rate = 21 });

            return rates;
        }

        public IList<Contact> GetDefaultCompanyContact()
        {
            IList<Contact> contacts = new List<Contact>();

            contacts.Add(new Contact
            {
                Id = 1,
                Phone = "+420 123 456 789",
                Email = "email@spolecnost.cz",
                Web = "www.spolecnost.cz",
                City = "Město",
                PostalCode = "123 45",
                Street = "Ulice",
                BuildingNumber = "1234"
            });

            return contacts;
        }

        public IList<Company> GetDefaultCompanyInformations()
        {
            IList<Company> companies = new List<Company>();

            companies.Add(new Company
            {
                Id = 1,
                Name = "Společnost",
                ContactId = 1,
                ICO = "12345678",
                DIC = "CZ12345678"
            });

            return companies;
        }

    }
}
