using PokladniSystem.Application.ViewModels;
using PokladniSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Application.Abstraction
{
    public interface ISaleService
    {
        OrderItemViewModel GetOrderItemViewModel(string eanCode, string sellerCode, int quantity);
        bool IsInStock(int productId, int? storeId, int quantity);
    }
}
