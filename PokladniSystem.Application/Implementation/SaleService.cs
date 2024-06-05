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
            Dictionary<int, double> VATRatePrices = new Dictionary<int, double>();
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
                int vatRate = _dbContext.VATRates.FirstOrDefault(v => v.Id == orderItem.Product.VATRateId).Rate;
                double price = Math.Round(orderItem.Product.PriceSale * orderItem.Quantity, 2);
                double vatPrice = Math.Round(price - price / (1 + (double)vatRate / 100), 2);
                totalPrice += price;

                var item = new OrderItem()
                {
                    OrderId = order.Id,
                    ProductId = orderItem.Product.Id,
                    Quantity = orderItem.Quantity,
                    Price = price,
                    VATPrice = vatPrice
                };

                if (VATRatePrices.ContainsKey(vatRate))
                {
                    VATRatePrices[vatRate] += vatPrice;
                }
                else
                {
                    VATRatePrices.Add(vatRate, vatPrice);
                }

                items.Add(item);
                _dbContext.OrderItems.Add(item);
            }

            foreach (var ratePrice in VATRatePrices)
            {
                _dbContext.OrderVATPrices.Add(new OrderVATPrice()
                {
                    OrderId = order.Id,
                    VATRate = ratePrice.Key,
                    VATPrice = ratePrice.Value,
                });
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

        private async Task<IList<Order>> GetFilteredOrdersAsync(int? id, DateTime? dateFrom, DateTime? dateTo, int? storeId, int pageNumber, int pageSize)
        {
            IQueryable<Order> ordersQuery = _dbContext.Orders;

            if (storeId != null)
            {
                ordersQuery = ordersQuery.Where(o => o.StoreId == storeId);
            }
            if (id != null)
            {
                ordersQuery = ordersQuery.Where(o => o.Id == id);
            }

            if (dateFrom != null)
            {
                ordersQuery = ordersQuery.Where(o => o.DateTimeCreated.Date >= dateFrom);
            }

            if (dateTo != null)
            {
                ordersQuery = ordersQuery.Where(o => o.DateTimeCreated.Date <= dateTo);
            }

            IList<Order> filteredOrders = await ordersQuery.ToListAsync();

            return filteredOrders;

        }

        private async Task<(IList<Order> pagedOrders, IList<Order> filteredOrders)> GetOrdersAsync(int? id, DateTime? dateFrom, DateTime? dateTo, int? storeId, int pageNumber, int pageSize)
        {
            IList<Order> filteredOrders = await GetFilteredOrdersAsync(id, dateFrom, dateTo, storeId, pageNumber, pageSize);

            IList<Order> pagedOrders = filteredOrders.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return (pagedOrders, filteredOrders);

        }

        public IList<OrderVATPrice> GetOrderVATPrices(int orderId)
        {
            IList<OrderVATPrice> orderVATPrices = _dbContext.OrderVATPrices.Where(o => o.OrderId == orderId).ToList();

            return orderVATPrices;
        }

        public int GetOrderPagesCount(int pageSize, int pageCount)
        {
            int totalPages = (int)Math.Ceiling((double)pageCount / pageSize);

            return totalPages;
        }

        public async Task<OrderListViewModel> GetOrderListViewModelAsync(OrderListViewModel vm)
        {
            Dictionary<int, IList<OrderVATPrice>> filteredOrdersVATPrices = new Dictionary<int, IList<OrderVATPrice>>();
            Dictionary<int, IList<OrderVATPrice>> pagedOrderVATPrices = new Dictionary<int, IList<OrderVATPrice>>();
            Dictionary<int, double> totalPrices = new Dictionary<int, double>();

            double totalPrice = 0;

            var (pagedOrders, filteredOrders) = await GetOrdersAsync(vm.OrderIdSearch, vm.DateFrom, vm.DateTo, vm.StoreIdSearch, vm.CurrentPage, vm.PageSize);



            foreach (var order in filteredOrders)
            {
                filteredOrdersVATPrices.Add(order.Id, GetOrderVATPrices(order.Id));
                totalPrice += (double)order.TotalPrice;
            }

            foreach (var filteredOrderVatPrices in filteredOrdersVATPrices)
            {
                foreach (var ratePrice in filteredOrderVatPrices.Value)
                {
                    if (totalPrices.ContainsKey(ratePrice.VATRate))
                    {
                        totalPrices[ratePrice.VATRate] += ratePrice.VATPrice;
                    }
                    else
                    {
                        totalPrices.Add(ratePrice.VATRate, ratePrice.VATPrice);
                    }
                }
            }


            vm.Stores = await _dbContext.Stores.ToListAsync();
            vm.Orders = pagedOrders;
            vm.TotalVATPrices = totalPrices;
            vm.TotalPrice = totalPrice;
            vm.TotalPages = GetOrderPagesCount(vm.PageSize, filteredOrders.Count);
            vm.CurrentPage = vm.CurrentPage != 0 ? vm.CurrentPage : 1;

            return vm;
        }
    }
}
