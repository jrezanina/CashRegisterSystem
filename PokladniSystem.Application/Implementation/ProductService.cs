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

namespace PokladniSystem.Application.Implementation
{
    public class ProductService : IProductService
    {
        CRSDbContext _dbContext;
        public ProductService(CRSDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IList<Product>> GetProductsAsync(int pageNumber, int pageSize)
        {
            int skipItemsCount = (pageNumber - 1) * pageSize;

            return await _dbContext.Products.OrderBy(p => p.Id).Skip(skipItemsCount).Take(pageSize).ToListAsync();
        }

        public void Create(ProductViewModel vm)
        {
            IList<int>? selectedCategoryIds = vm.SelectedCategories.Where(kv => kv.Value).Select(kv => kv.Key).ToList();


            if (_dbContext.Products != null)
            {
                _dbContext.Products.Add(vm.Product);
                _dbContext.SaveChanges();
            }
            if (_dbContext.ProductCategories != null)
            {
                foreach (var categoryId in selectedCategoryIds)
                {
                    _dbContext.ProductCategories.Add(new ProductCategory()
                    {
                        ProductId = vm.Product.Id,
                        CategoryId = categoryId
                    });
                }
                _dbContext.SaveChanges();
            }
            
        }

        public void Edit(ProductViewModel vm)
        {
            /*Product? productItem = _dbContext.Products.FirstOrDefault(p => p.Id == product.Id);
            if (productItem != null)
            {
                productItem.EanCode = product.EanCode;
                productItem.SellerCode = product.SellerCode;
                productItem.Name = product.Name;
                productItem.ShortName = product.ShortName;
                productItem.Description = product.Description;
                productItem.PriceVATFree = product.PriceVATFree;
                productItem.PriceVAT = product.PriceVAT;
                productItem.PriceSale = product.PriceSale;
                _dbContext.SaveChanges();
            }*/
        }
        public Product GetProduct(int? id, string? eanCode, string? sellerCode)
        {
            if (id != null)
            {
                return _dbContext.Products.FirstOrDefault(p => p.Id == id);
            }
            else if (!string.IsNullOrEmpty(eanCode))
            {
                return _dbContext.Products.FirstOrDefault(p => p.EanCode == eanCode);
            }
            else if (!string.IsNullOrEmpty(sellerCode))
            {
                return _dbContext.Products.FirstOrDefault(p => p.SellerCode == sellerCode);
            }
            else
            {
                return null;
            }
        }

        public int GetProductPagesCount(int pageSize)
        {
            int productCount = _dbContext.Products.Count();
            int totalPages = (int)Math.Ceiling((double)productCount / pageSize);

            return totalPages;
        }

        public async Task<ProductListViewModel> GetProductListViewModelAsync(int page, int pageSize)
        {
            IList<Product> products = await GetProductsAsync(page, pageSize);
            int pageCount = GetProductPagesCount(pageSize);

            ProductListViewModel vm = new ProductListViewModel()
            {
                Products = products,
                CurrentPage = page,
                TotalPages = pageCount
            };

            return vm;
        }

        public async Task<ProductViewModel> GetProductViewModelAsync(ProductViewModel? vm)
        {
            IList<Category> categories = await _dbContext.Categories.ToListAsync();
            IList<VATRate> vatRates = await _dbContext.VATRates.ToListAsync();

            ProductViewModel viewModel = new ProductViewModel()
            {
                Product = vm != null ? vm.Product : null,
                SelectedCategories = vm != null ? vm.SelectedCategories : null, 
                Categories = categories,
                VATRates = vatRates
            };

            return viewModel;
        }
    }
}
/*
 Seznam produktů udělat jako výpis všech produktů včetně stránkování, manažer a účetní tam bude mít navíc edit u každého produktu, účetní ještě přidat, prodavač ne,
navíc by bylo dobré implementovat vyhledávací pole s radio buttonem pro eanCode/sellerCode/název?
 */

