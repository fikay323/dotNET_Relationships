namespace dotNET_Relationships.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; } = string.Empty;
        public required string Description { get; set; } = string.Empty;
        public required decimal Price { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
