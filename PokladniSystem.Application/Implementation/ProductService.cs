using iText.Commons.Actions.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PokladniSystem.Application.Abstraction;
using PokladniSystem.Application.ViewModels;
using PokladniSystem.Domain.Entities;
using PokladniSystem.Infrastructure.Database;
using PokladniSystem.Infrastructure.Identity;
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
        IAccountService _accountService;
        public ProductService(CRSDbContext dbContext, IAccountService accountService)
        {
            _dbContext = dbContext;
            _accountService = accountService;
        }

        private async Task<(IList<Product> products, int productCount)> GetProductsAsync(string? eanCode, string? sellerCode, int? categoryId, int? vatRateId, int pageNumber, int pageSize)
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

        public void UpdatePriceVAT(Product product)
        {
            Product? productItem = _dbContext.Products.FirstOrDefault(p => p.Id == product.Id);

            if (productItem != null)
            {
                productItem.PriceVAT = Math.Round(productItem.PriceVATFree * (1 + _dbContext.VATRates.FirstOrDefault(r => r.Id == productItem.VATRateId).Rate / 100.0), 2);
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

        public Product GetProduct(int id)
        {
            Product product = _dbContext.Products.FirstOrDefault(p =>p.Id == id);

            return product;
        }

        public int GetProductPagesCount(int pageSize, int pageCount)
        {
            int totalPages = (int)Math.Ceiling((double)pageCount / pageSize);

            return totalPages;
        }

        public async Task<ProductListViewModel> GetProductListViewModelAsync(ProductListViewModel vm)
        {
            Dictionary<int, int> productQuantities = new Dictionary<int, int>();

            var (filteredProducts, productCount) = await GetProductsAsync(vm.EanCodeSearch, vm.SellerCodeSearch, vm.CategoryIdSearch, vm.VATRateIdSearch, vm.CurrentPage, vm.PageSize);
            var filteredProductIds = filteredProducts.Select(p => p.Id).ToList();

            Dictionary<int, int> filteredProductQuantities;

            if (vm.StoreIdSearch != null)
            {
                filteredProductQuantities = _dbContext.Supplies.Where(s => filteredProductIds.Contains(s.ProductId) && s.StoreId == vm.StoreIdSearch).ToDictionary(s => s.ProductId, s => s.Quantity);
            }
            else
            {
                filteredProductQuantities = _dbContext.Supplies.Where(s => filteredProductIds.Contains(s.ProductId)).GroupBy(s => s.ProductId).ToDictionary(g => g.Key, g => g.Sum(s => s.Quantity));
            }

            foreach (var product in filteredProducts)
            {
                productQuantities.Add(product.Id, filteredProductQuantities.ContainsKey(product.Id) ? filteredProductQuantities[product.Id] : 0);
            }

            vm.ProductQuantities = productQuantities;
            vm.Stores = await _dbContext.Stores.ToListAsync();
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

        public async Task<SupplyViewModel> GetSupplyViewModelAsync(SupplyViewModel vm)
        {
            IList<Store> stores = await _dbContext.Stores.ToListAsync();
            Product product = _dbContext.Products.FirstOrDefault(p => p.Id == vm.Supply.ProductId);

            SupplyViewModel viewModel = new SupplyViewModel()
            {
                Supply = vm.Supply,
                Stores = stores,
                Product = product
            };

            return viewModel;
        }

        public async Task<SupplyViewModel> GetSupplyViewModelAsync(int productId)
        {
            IList<Store> stores = await _dbContext.Stores.ToListAsync();
            Product product = _dbContext.Products.FirstOrDefault(p => p.Id == productId);

            SupplyViewModel viewModel = new SupplyViewModel()
            {
                Supply = new Supply()
                {
                    Quantity = 0,
                    ProductId = productId,
                    StoreId = null
                },
                Stores = stores,
                Product = product
            };

            return viewModel;
        }

        public bool StockUp(SupplyViewModel vm)
        {
            Supply supplyItem = _dbContext.Supplies.Where(s => s.ProductId == vm.Supply.ProductId && s.StoreId == vm.Supply.StoreId).FirstOrDefault();

            if (supplyItem != null)
            {
                int totalQuantity = supplyItem.Quantity + vm.Supply.Quantity;

                if (totalQuantity >= 0)
                {
                    supplyItem.Quantity += vm.Supply.Quantity;
                    _dbContext.SaveChanges();

                    return true;
                }
            }
            else
            {
                if (vm.Supply.Quantity >= 0)
                {
                    _dbContext.Supplies.Add(vm.Supply);
                    _dbContext.SaveChanges();

                    return true;
                }
            }

            return false;
        }
    }
}

