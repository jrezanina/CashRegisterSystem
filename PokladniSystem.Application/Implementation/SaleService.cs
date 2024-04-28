using Microsoft.EntityFrameworkCore;
using PokladniSystem.Application.Abstraction;
using PokladniSystem.Application.ViewModels;
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
    public class SaleService : ISaleService
    {
        CRSDbContext _dbContext;
        IProductService _productService;

        public SaleService(CRSDbContext dbContext, IProductService productService)
        {
            _dbContext = dbContext;
            _productService = productService;
        }

        public OrderItemViewModel GetOrderItemViewModel(string eanCode, string sellerCode, int quantity)
        {
            Product product = _productService.GetProduct(eanCode, sellerCode);

            if (product != null)
            {
                OrderItemViewModel viewModel = new OrderItemViewModel()
                {
                    Product = product,
                    Quantity = quantity
                };

                return viewModel;
            }

            return null;
        }
    }
}
