using DeliveryNofityAPI.Models;

namespace DeliveryNofityAPI.Services
{
    public interface IOrderService
    {
        Task<(Customer customer, Order order)> GetCustomerOrderAsync(string email, int customerId);
    }
}
