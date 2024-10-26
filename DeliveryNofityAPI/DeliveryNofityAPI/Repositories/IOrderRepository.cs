using DeliveryNofityAPI.Models;

namespace DeliveryNofityAPI.Repositories
{
    public interface IOrderRepository
    {
        Task<Customer> GetCustomerAsync(string email, int customerId);
        Task<Order> GetMostRecentOrderAsync(int customerId);
    }
}
