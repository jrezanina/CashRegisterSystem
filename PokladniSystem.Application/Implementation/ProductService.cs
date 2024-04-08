using iText.Commons.Actions.Contexts;
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
    public class ProductService : IProductService
    {
        CRSDbContext _dbContext;
        public ProductService(CRSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /*public async Task<IList<Product>> GetProductsAsync(int pageNumber, int pageSize)
        {

            int skipItemsCount = (pageNumber - 1) * pageSize;

            return await _dbContext.Products.OrderBy(p => p.Id).Skip(skipItemsCount).Take(pageSize).ToListAsync();
        }*/

        public async Task<(IList<Product> products, int productCount)> GetProductsAsync(string? eanCode, string? sellerCode, int? categoryId, int? vatRateId, int pageNumber, int pageSize)
        {
            IQueryable<Product> productsQuery = _dbContext.Products;

            if (!string.IsNullOrEmpty(eanCode))
            {
                productsQuery = productsQuery.Where(p => p.EanCode == eanCode);
            }

            if (!string.IsNullOrEmpty(sellerCode))
            {
                productsQuery = productsQuery.Where(p => p.SellerCode == sellerCode);
            }

            if (categoryId != null)
            {
                var productIdsWithCategory = _dbContext.ProductCategories.Where(pc => pc.CategoryId == categoryId).Select(pc => pc.ProductId);
                productsQuery = productsQuery.Where(p => productIdsWithCategory.Contains(p.Id));
            }

            if (vatRateId != null)
            {
                productsQuery = productsQuery.Where(p => p.VATRateId == vatRateId);
            }

            IList<Product> filteredProducts = await productsQuery.ToListAsync();
            filteredProducts = filteredProducts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return (filteredProducts, await productsQuery.CountAsync());

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
            Product? productItem = _dbContext.Products.FirstOrDefault(p => p.Id == vm.Product.Id);
            IList<int>? selectedCategoryIds = vm.SelectedCategories.Where(kv => kv.Value).Select(kv => kv.Key).ToList();

            if (productItem != null)
            {
                productItem.EanCode = vm.Product.EanCode;
                productItem.SellerCode = vm.Product.SellerCode;
                productItem.Name = vm.Product.Name;
                productItem.ShortName = vm.Product.ShortName;
                productItem.Description = vm.Product.Description;
                productItem.PriceVATFree = vm.Product.PriceVATFree;
                productItem.PriceVAT = vm.Product.PriceVAT;
                productItem.PriceSale = vm.Product.PriceSale;
                productItem.VATRateId = vm.Product.VATRateId;
                _dbContext.SaveChanges();
            }

            var productCategories = _dbContext.ProductCategories.Where(pc => pc.ProductId == vm.Product.Id);
            if (productCategories != null)
            {
                _dbContext.ProductCategories.RemoveRange(productCategories);
                _dbContext.SaveChanges();
            }

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

        public void EditPriceSale(ProductViewModel vm)
        {
            Product? productItem = _dbContext.Products.FirstOrDefault(p => p.Id == vm.Product.Id);

            if (productItem != null)
            {
                productItem.PriceSale = vm.Product.PriceSale;
                _dbContext.SaveChanges();
            }
        }

        public Product GetProduct(string? eanCode, string? sellerCode)
        {
            if (!string.IsNullOrEmpty(eanCode))
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

        public int GetProductPagesCount(int pageSize, int pageCount)
        {
            int totalPages = (int)Math.Ceiling((double)pageCount / pageSize);

            return totalPages;
        }

        public async Task<ProductListViewModel> GetProductListViewModelAsync(ProductListViewModel vm)
        {
            var (filteredProducts, productCount) = await GetProductsAsync(vm.EanCodeSearch, vm.SellerCodeSearch, vm.CategoryIdSearch, vm.VATRateIdSearch, vm.CurrentPage, vm.PageSize);
            vm.Categories = await _dbContext.Categories.ToListAsync();
            vm.VATRates = await _dbContext.VATRates.ToListAsync();
            vm.Products = filteredProducts;
            vm.TotalPages = GetProductPagesCount(vm.PageSize, productCount);
            vm.CurrentPage = vm.CurrentPage != 0 ? vm.CurrentPage : 1;

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

        public async Task<ProductViewModel> GetProductViewModelAsync(int productId)
        {
            IList<Category> categories = await _dbContext.Categories.ToListAsync();
            IList<VATRate> vatRates = await _dbContext.VATRates.ToListAsync();

            Dictionary<int, bool> selectedCategories = new Dictionary<int, bool>();

            foreach (var category in categories)
            {
                selectedCategories.Add(category.Id, _dbContext.ProductCategories.Any(pc => pc.ProductId == productId && pc.CategoryId == category.Id));
            }

            ProductViewModel viewModel = new ProductViewModel()
            {
                Product = _dbContext.Products.FirstOrDefault(p => p.Id == productId),
                SelectedCategories = selectedCategories,
                Categories = categories,
                VATRates = vatRates
            };

            return viewModel;
        }
    }
}

