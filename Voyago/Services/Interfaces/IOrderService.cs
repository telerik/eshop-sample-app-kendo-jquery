using Data;
using Models.ViewModels;

namespace Services.Interfaces
{
    public interface IOrderService
    {
        Task<bool> AddSalesOrder(List<ShoppingCartItemViewModel> items, string userEmail);
        IQueryable<OrderViewModel?> GetAllOrders(string userEmail);
        IEnumerable<OrderDetailsViewModel> GetOrderDetailsById(int orderId, string userEmail);
    }
}
