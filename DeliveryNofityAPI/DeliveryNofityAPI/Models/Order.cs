namespace DeliveryNofityAPI.Models
{
    public class Order
    {
        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string DeliveryAddress { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();
        public DateTime DeliveryExpected { get; set; }
    }
}
