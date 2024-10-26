using DeliveryNofityAPI.Models;
using DeliveryNofityAPI.Repositories;

namespace DeliveryNofityAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<(Customer customer, Order order)> GetCustomerOrderAsync(string email, int customerId)
        {
            var customer = await _orderRepository.GetCustomerAsync(email, customerId);

            if (customer == null) throw new Exception("Invalid customer information.");

            var order = await _orderRepository.GetMostRecentOrderAsync(customerId);
            return (customer, order);
        }
    }
}
