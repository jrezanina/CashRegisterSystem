using PokladniSystem.Application.ViewModels;
using PokladniSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Application.Abstraction
{
    public interface IProductService
    {
        Product GetProduct(string? eanCode, string? sellerCode);
        Task<ProductListViewModel> GetProductListViewModelAsync(ProductListViewModel vm);
        Task<ProductViewModel> GetProductViewModelAsync(int productId);
        Task<ProductViewModel> GetProductViewModelAsync(ProductViewModel? vm);
        Task<SupplyViewModel> GetSupplyViewModelAsync(int productId);
        Task<SupplyViewModel> GetSupplyViewModelAsync(SupplyViewModel? vm);
        void Create(ProductViewModel vm);
        void Edit(ProductViewModel vm);
        void EditPriceSale(ProductViewModel vm);
        void UpdatePriceVAT(Product product);
        bool StockUp(SupplyViewModel vm);
    }
}
