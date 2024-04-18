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

        public IList<Contact> GetUTBCompanyContact()
        {
            IList<Contact> contacts = new List<Contact>();

            contacts.Add(new Contact
            {
                Id = 1,
                Phone = "+420 576 038 120",
                Email = "podatelna@utb.cz",
                Web = "www.utb.cz",
                City = "Zlín",
                PostalCode = "760 01",
                Street = "nám. T. G. Masaryka",
                BuildingNumber = "5555"
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

        public IList<Company> GetUTBCompanyInformations()
        {
            IList<Company> companies = new List<Company>();

            companies.Add(new Company
            {
                Id = 1,
                Name = "Univerzita Tomáše Bati ve Zlíně",
                ContactId = 1,
                ICO = "70883521",
                DIC = "CZ70883521"
            });

            return companies;
        }

        public IList<Contact> GetFAIStoreContact()
        {
            IList<Contact> contacts = new List<Contact>();

            contacts.Add(new Contact
            {
                Id = 2,
                Phone = "+420 576 035 221",
                Email = "dekanat@fai.utb.cz",
                Web = "www.fai.utb.cz",
                City = "Zlín",
                PostalCode = "760 05",
                Street = "Nad Stráněmi",
                BuildingNumber = "4511"
            });

            return contacts;
        }

        public IList<Store> GetFAIStoreInformations()
        {
            IList<Store> stores = new List<Store>();

            stores.Add(new Store
            {
                Id = 2,
                Name = "Fakulta aplikované informatiky",
                ContactId = 2   
            });

            return stores;
        }

        public IList<Category> GetDefaultCategories()
        {
            IList<Category> categories = new List<Category>();

            categories.Add(new Category { Id = 1, Name = "Potraviny" });
            categories.Add(new Category { Id = 2, Name = "Drogerie" });
            categories.Add(new Category { Id = 3, Name = "Pečivo" });
            categories.Add(new Category { Id = 4, Name = "Mléčné výrobky" });
            categories.Add(new Category { Id = 5, Name = "Nápoje" });

            return categories;
        }

        public IList<Product> GetDefaultProducts()
        {
            IList<Product> products = new List<Product>();

            products.Add(new Product { Id = 1, Name = "Rohlík tukový 43g", ShortName = "Rohlík tukový 43g", SellerCode = "2207", PriceVATFree = 1.50, PriceVAT = 1.68, PriceSale = 2.50, VATRateId = 2 });
            products.Add(new Product { Id = 2, Name = "Chléb konzumní 1000g", ShortName = "Chléb konzumní 1000g", SellerCode = "2701", PriceVATFree = 27.50, PriceVAT = 30.80, PriceSale = 39.90, VATRateId = 2 });
            products.Add(new Product { Id = 3, Name = "Kobliha s náplní meruňka 70g", ShortName = "Kobliha s n.mer. 70g", SellerCode = "2003", PriceVATFree = 6.50, PriceVAT = 7.28, PriceSale = 11.50, VATRateId = 2 });
            products.Add(new Product { Id = 4, Name = "Francouzská bageta 150g", ShortName = "Franc. bageta 150g", SellerCode = "2024", PriceVATFree = 10.00, PriceVAT = 11.20, PriceSale = 15.90, VATRateId = 2 });
            products.Add(new Product { Id = 5, Name = "Trvanlivé mléko plnotučné 3,5% 1l", ShortName = "T.mléko pol. 3,5% 1l", EanCode = "9788071963455", PriceVATFree = 16.00, PriceVAT = 17.92, PriceSale = 24.90, VATRateId = 2 });
            products.Add(new Product { Id = 6, Name = "Smetanový jogurt borůvka 150g", ShortName = "Smet.jog.bor. 150g", EanCode = "4014400901191", PriceVATFree = 9.30, PriceVAT = 10.42, PriceSale = 15.90, VATRateId = 2 });
            products.Add(new Product { Id = 7, Name = "Smetanový jogurt jahoda 150g", ShortName = "Smet.jog.jah. 150g", EanCode = "4014400400007", PriceVATFree = 9.30, PriceVAT = 10.42, PriceSale = 15.90, VATRateId = 2 });
            products.Add(new Product { Id = 8, Name = "Zakysaná smetana 15% 200g", ShortName = "Zak.smetana 15% 200g", EanCode = "7622210606754", PriceVATFree = 11.50, PriceVAT = 12.88, PriceSale = 14.90, VATRateId = 2 });
            products.Add(new Product { Id = 9, Name = "Smetana ke šlehání 31% 200g", ShortName = "Smet.ke šl. 31% 200g", EanCode = "5900259128515", PriceVATFree = 25.50, PriceVAT = 28.56, PriceSale = 37.90, VATRateId = 2 });
            products.Add(new Product { Id = 10, Name = "Tvaroh polotučný 250g", ShortName = "Tvaroh pol. 250g", EanCode = "4000512363835", PriceVATFree = 22.70, PriceVAT = 25.42, PriceSale = 32.90, VATRateId = 2 });
            products.Add(new Product { Id = 11, Name = "Eidam 30% plátky 100g", ShortName = "Eidam 30% plát. 100g", EanCode = "8000500179864", PriceVATFree = 20.00, PriceVAT = 22.40, PriceSale = 32.90, VATRateId = 2 });
            products.Add(new Product { Id = 12, Name = "Zubní pasta 75ml", ShortName = "Zubní pasta 75ml", EanCode = "8594050910072", PriceVATFree = 56.00, PriceVAT = 67.76, PriceSale = 99.90, VATRateId = 3 });
            products.Add(new Product { Id = 13, Name = "Tekuté mýdlo dezinfekční 250ml", ShortName = "Tek.mýdlo dez. 250ml", EanCode = "8594003849626", PriceVATFree = 35.00, PriceVAT = 39.20, PriceSale = 54.90, VATRateId = 2 });
            products.Add(new Product { Id = 14, Name = "Šampon 400ml", ShortName = "Šampon 400ml", EanCode = "5053990161669", PriceVATFree = 70.00, PriceVAT = 84.70, PriceSale = 109.90, VATRateId = 3 });
            products.Add(new Product { Id = 15, Name = "Cola 500ml", ShortName = "Cola 500ml", EanCode = "54491472", PriceVATFree = 15.00, PriceVAT = 18.15, PriceSale = 24.90, VATRateId = 3 });
            products.Add(new Product { Id = 16, Name = "Limonáda 500ml", ShortName = "Limonáda 500ml", EanCode = "20504755", PriceVATFree = 13.00, PriceVAT = 15.73, PriceSale = 21.90, VATRateId = 3 });

            return products;
        }

        public IList<ProductCategory> GetDefaultProductCategories()
        {
            IList<ProductCategory> productCategories = new List<ProductCategory>();

            productCategories.Add(new ProductCategory { Id = 1, ProductId = 1, CategoryId = 1 });
            productCategories.Add(new ProductCategory { Id = 2, ProductId = 1, CategoryId = 3 });
            productCategories.Add(new ProductCategory { Id = 3, ProductId = 2, CategoryId = 1 });
            productCategories.Add(new ProductCategory { Id = 4, ProductId = 2, CategoryId = 3 });
            productCategories.Add(new ProductCategory { Id = 5, ProductId = 3, CategoryId = 1 });
            productCategories.Add(new ProductCategory { Id = 6, ProductId = 3, CategoryId = 3 });
            productCategories.Add(new ProductCategory { Id = 7, ProductId = 4, CategoryId = 1 });
            productCategories.Add(new ProductCategory { Id = 8, ProductId = 4, CategoryId = 3 });
            productCategories.Add(new ProductCategory { Id = 9, ProductId = 5, CategoryId = 1 });
            productCategories.Add(new ProductCategory { Id = 10, ProductId = 5, CategoryId = 4 });
            productCategories.Add(new ProductCategory { Id = 11, ProductId = 6, CategoryId = 1 });
            productCategories.Add(new ProductCategory { Id = 12, ProductId = 6, CategoryId = 4 });
            productCategories.Add(new ProductCategory { Id = 13, ProductId = 7, CategoryId = 1 });
            productCategories.Add(new ProductCategory { Id = 14, ProductId = 7, CategoryId = 4 });
            productCategories.Add(new ProductCategory { Id = 15, ProductId = 8, CategoryId = 1 });
            productCategories.Add(new ProductCategory { Id = 16, ProductId = 8, CategoryId = 4 });
            productCategories.Add(new ProductCategory { Id = 17, ProductId = 9, CategoryId = 1 });
            productCategories.Add(new ProductCategory { Id = 18, ProductId = 9, CategoryId = 4 });
            productCategories.Add(new ProductCategory { Id = 19, ProductId = 10, CategoryId = 1 });
            productCategories.Add(new ProductCategory { Id = 20, ProductId = 10, CategoryId = 4 });
            productCategories.Add(new ProductCategory { Id = 21, ProductId = 11, CategoryId = 1 });
            productCategories.Add(new ProductCategory { Id = 22, ProductId = 11, CategoryId = 4 });
            productCategories.Add(new ProductCategory { Id = 23, ProductId = 12, CategoryId = 2 });
            productCategories.Add(new ProductCategory { Id = 24, ProductId = 13, CategoryId = 2 });
            productCategories.Add(new ProductCategory { Id = 25, ProductId = 14, CategoryId = 2 });
            productCategories.Add(new ProductCategory { Id = 26, ProductId = 15, CategoryId = 1 });
            productCategories.Add(new ProductCategory { Id = 27, ProductId = 15, CategoryId = 5 });
            productCategories.Add(new ProductCategory { Id = 28, ProductId = 16, CategoryId = 1 });
            productCategories.Add(new ProductCategory { Id = 29, ProductId = 16, CategoryId = 5 });

            return productCategories;
        }
    }
}
