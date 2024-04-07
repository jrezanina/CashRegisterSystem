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
        Task<IList<Product>> GetProductsAsync(int pageNumber, int pageSize);
        Task<ProductListViewModel> GetProductListViewModelAsync(int page, int pageSize, string? eanCode, string? sellerCode);
        Task<ProductViewModel> GetProductViewModelAsync(ProductViewModel? vm);
        void Create(ProductViewModel vm);
        void Edit(ProductViewModel vm);
    }
}
