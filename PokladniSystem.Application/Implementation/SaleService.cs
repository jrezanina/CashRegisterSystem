using iText.Commons.Actions.Contexts;
using iText.Layout.Borders;
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

        public bool IsInStock(int productId, int? storeId, int quantity)
        {
            Supply supply = _dbContext.Supplies.FirstOrDefault(s => s.ProductId == productId && s.StoreId == storeId);

            if (supply != null)
            {
                return supply.Quantity >= quantity;
            }

            return false;
        }

        public Order GetOrder(int orderId)
        {
            Order order = _dbContext.Orders.FirstOrDefault(o => o.Id == orderId);

            return order;
        }

        public IList<OrderItem> GetOrderItems(int orderId)
        {
            IList<OrderItem> orderItems = _dbContext.OrderItems.Where(oi => oi.OrderId == orderId).ToList();

            return orderItems;
        }

        public int CreateOrder(IList<OrderItemViewModel> orderItems, User user)
        {
            IList<OrderItem> items = new List<OrderItem>();
            double totalPrice = 0;

            Order order = new Order()
            {
                DateTimeCreated = DateTime.Now,
                StoreId = (int)user.StoreId,
                UserId = user.Id,
            };

            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();

            foreach (var orderItem in orderItems)
            {
                double price = Math.Round(orderItem.Product.PriceSale * orderItem.Quantity, 2);
                totalPrice += price;

                var item = new OrderItem()
                {
                    OrderId = order.Id,
                    ProductId = orderItem.Product.Id,
                    Quantity = orderItem.Quantity,
                    Price = price
                };

                items.Add(item);
                _dbContext.OrderItems.Add(item);
            }

            order.TotalPrice = Math.Round(totalPrice, MidpointRounding.AwayFromZero);
            _dbContext.SaveChanges();

            return order.Id;
        }

        public void SetOrderReceiptPath(int orderId, string receiptPath)
        {
            Order orderItem = GetOrder(orderId);

            if (orderItem != null)
            {
                orderItem.ReceiptSrc = receiptPath;
                _dbContext.SaveChanges();
            }
        }
    }
}
