using DeliveryNofityAPI.Models;
using Microsoft.Data.SqlClient;
using DeliveryNofityAPI.Utilities;

namespace DeliveryNofityAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly SqlConnections _conn;

        public OrderRepository(SqlConnections connection)
        {
            _conn = connection;
        }

        public async Task<Customer> GetCustomerAsync(string email, int customerId)
        {
            string query = "SELECT FirstName, LastName FROM Customers WHERE Email = @Email AND CustomerId = @CustomerId";

            using var connection = _conn.CreateConnection();
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@CustomerId", customerId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Customer
                {
                    FirstName = reader["FirstName"].ToString(),
                    LastName = reader["LastName"].ToString()
                };
            }
            return null;
        }

        public async Task<Order> GetMostRecentOrderAsync(int customerId)
        {
            string orderQuery = @"SELECT TOP 1 o.OrderId, o.OrderDate, o.DeliveryExpected, c.HouseNo, c.Street, c.Town, c.PostCode
                          FROM Orders o
                          JOIN Customers c ON o.CustomerId = c.CustomerId
                          WHERE c.CustomerId = @CustomerId
                          ORDER BY o.OrderDate DESC";

            using var connection = _conn.CreateConnection();
            using var orderCommand = new SqlCommand(orderQuery, connection);
            orderCommand.Parameters.AddWithValue("@CustomerId", customerId);

            await connection.OpenAsync();
            using var reader = await orderCommand.ExecuteReaderAsync();

            if (!await reader.ReadAsync()) return null;

            var order = new Order
            {
                OrderNumber = Convert.ToInt32(reader["OrderId"]),
                OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                DeliveryExpected = Convert.ToDateTime(reader["DeliveryExpected"]),
                DeliveryAddress = $"{reader["HouseNo"]} {reader["Street"]}, {reader["Town"]}, {reader["PostCode"]}"
            };

            await reader.CloseAsync();

            // Retrieve order items
            string itemsQuery = @"SELECT oi.Quantity, oi.Price, p.ProductName, o.ContainsGift
                          FROM OrderItems oi
                          JOIN Products p ON oi.ProductId = p.ProductId
                          JOIN Orders o ON oi.OrderId = o.OrderId
                          WHERE oi.OrderId = @OrderId";

            using var itemsCommand = new SqlCommand(itemsQuery, connection);
            itemsCommand.Parameters.AddWithValue("@OrderId", order.OrderNumber);

            using var itemsReader = await itemsCommand.ExecuteReaderAsync();
            while (await itemsReader.ReadAsync())
            {
                order.OrderItems.Add(new OrderItem
                {
                    Product = Convert.ToBoolean(itemsReader["ContainsGift"]) ? "Gift" : itemsReader["ProductName"].ToString(),
                    Quantity = Convert.ToInt32(itemsReader["Quantity"]),
                    PriceEach = Convert.ToDecimal(itemsReader["Price"])
                });
            }

            return order;
        }
    }
}
