namespace dotNET_Relationships.DTOs
{
    public class OrderDto
    {
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public List<ProductOrdered> Products { get; set; } = new List<ProductOrdered>();
    }

    public class ProductOrdered
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
