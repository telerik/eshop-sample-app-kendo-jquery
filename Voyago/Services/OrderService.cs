using Microsoft.EntityFrameworkCore;
using Services.ServiceExtensions;
using Services.Interfaces;
using System.Reflection;
using Models.ViewModels;
using Data;
using Entities;

namespace Services
{
    public class OrderService : IOrderService
    {
        private readonly VoyagoDatabaseContext dbContext;

        public OrderService(VoyagoDatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> AddSalesOrder(List<ShoppingCartItemViewModel> items, string userEmail)
        {
            var orederNumberLast = dbContext.SalesOrders.Max(x => x.OrderNumber) != null ? dbContext.SalesOrders.Max(x => x.OrderNumber) : 0;
            var total = items.Sum(items => items.Total);
            var userID = dbContext.Contacts.FirstOrDefault(u => u.EmailAddress == userEmail);
            if (userID != null)
            {
                for (var i = 0; i < items.Count(); i++)
                {
                    var orderForDb = new SalesOrder
                    {                       
                        ProductName = items[i].ProductName,
                        OrderNumber = orederNumberLast + 1,
                        ProductId = items[i].ProductId,
                        Total = total,
                        Status = 1,
                        ShipDate = DateTime.Now,
                        UnitPrice = items[i].ProductPrice,
                        LineTotal = items[i].Total,
                        Quantity = (short)items[i].Quantity,
                        ContactId = userID.ContactId,
                    };
                    await dbContext.SalesOrders.AddAsync(orderForDb);
                }

                return await dbContext.SaveChangesAsync() > 0 ? true : false;
            }
            return false;
        }

        public IQueryable<OrderViewModel?> GetAllOrders(string userEmail)
        {           
            var orders = (from order in dbContext.SalesOrders                         
                          join contact in dbContext.Contacts on order.ContactId equals contact.ContactId                          
                          where contact.EmailAddress == userEmail
                          //group order by order.OrderNumber into x
                          select new OrderViewModel
                          {
                              OrderID = order.OrderId,
                              OrderDate = order.ShipDate,
                              Total = order.Total,
                              Status = order.Status,
                              OrderNumber = order.OrderNumber,
                              UserFirstName = contact.FirstName,
                              UserLasttName = contact.LastName,
                              Phone = contact.Phone,
                              Street = contact.Street,
                              State = contact.State,
                              City = contact.City,
                              Zipcode = contact.ZipCode,
                              Country = contact.Country
                          }).GroupBy( gr => gr.OrderNumber).Select(s => s.FirstOrDefault());          

            return orders;
        }

        public IEnumerable<OrderDetailsViewModel> GetOrderDetailsById(int orderNumber, string userEmail)
        {            
            List<OrderDetailsViewModel> orders = (from order in dbContext.SalesOrders
                          join contact in dbContext.Contacts on order.ContactId equals contact.ContactId
                          where contact.EmailAddress == userEmail && order.OrderNumber == orderNumber
                          select new OrderDetailsViewModel
                          {
                              OrderID = order.OrderId,
                              OrderDate = order.ShipDate,
                              OrderNumber = order.OrderNumber,
                              Total = order.Total,
                              Status = order.Status,
                              UserFirstName = contact.FirstName,
                              UserLasttName = contact.LastName,
                              Phone = contact.Phone,
                              Street = contact.Street,
                              State = contact.State,
                              City = contact.City,
                              Country = contact.Country,
                              ProductID = order.ProductId,
                              ProductName = order.ProductName,
                              ProductPhoto = order.Product.ThumbNailPhoto,
                              Quantity = order.Quantity,
                              UnitPrice = order.UnitPrice,
                              LineTotal = order.LineTotal
                          }).ToList();
            return orders;
        }

        private IQueryable<OrderViewModel> MapOrdersToOrderViewModel(IQueryable<SalesOrder> orders)
        {
            var viewModels = orders.Select(p => new OrderViewModel
            {
                OrderID = p.OrderId,
                OrderDate = p.ShipDate,
                Total = p.Total,
                Status = p.Status
            });

            return viewModels;
        }
    }
}
