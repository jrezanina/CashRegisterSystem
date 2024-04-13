using Microsoft.EntityFrameworkCore;
using PokladniSystem.Application.Abstraction;
using PokladniSystem.Domain.Entities;
using PokladniSystem.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace PokladniSystem.Application.Implementation
{
    public class VATService : IVATService
    {
        CRSDbContext _dbContext;
        IProductService _productService;

        public VATService(CRSDbContext dbContext, IProductService productService)
        {
            _dbContext = dbContext;
            _productService = productService;
        }
        public IList<VATRate> GetVATRates()
        {
            return _dbContext.VATRates.ToList();
        }

        public void Create(VATRate vatRate)
        {
            if (_dbContext.VATRates != null)
            {
                _dbContext.VATRates.Add(vatRate);
                _dbContext.SaveChanges();
            }
        }

        public bool Delete(int id)
        {
            bool deleted = false;
            VATRate? vatRateItem = _dbContext.VATRates.FirstOrDefault(v => v.Id == id);

            if (vatRateItem != null)
            {
                var products = _dbContext.Products.Where(p => p.VATRateId == id);
                if (products == null || products.Count() == 0)
                {
                    _dbContext.VATRates.Remove(vatRateItem);
                    _dbContext.SaveChanges();
                    deleted = true;
                }
            }
            return deleted;
        }

        public void Edit(VATRate vatRate)
        {
            VATRate? vatRateItem = _dbContext.VATRates.FirstOrDefault(v => v.Id == vatRate.Id);
            var products = _dbContext.Products.Where(p => p.VATRateId == vatRate.Id).ToList();

            if (vatRateItem != null)
            {
                vatRateItem.Rate = vatRate.Rate;
                _dbContext.SaveChanges();
            }

            foreach (var product in products)
            {
                _productService.UpdatePriceVAT(product);
            }
        }
    }
}
