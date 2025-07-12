namespace dotNET_Relationships.Models
{
    public class OrderProduct
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        public Order Order { get; set; } = null!;
        public Product Product{ get; set; } = null!;

        public int Quantity { get; set; }
    }
}
