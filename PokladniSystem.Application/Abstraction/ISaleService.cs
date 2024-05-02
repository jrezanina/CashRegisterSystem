using PokladniSystem.Application.ViewModels;
using PokladniSystem.Domain.Entities;
using PokladniSystem.Infrastructure.Identity;
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
        Order GetOrder(int orderId);
        IList<OrderItem> GetOrderItems(int orderId);
        int CreateOrder(IList<OrderItemViewModel> orderItems, User user);
        bool IsInStock(int productId, int? storeId, int quantity);
        void SetOrderReceiptPath(int orderId, string receiptPath);
    }
}
