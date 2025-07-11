using System.ComponentModel.DataAnnotations.Schema;

namespace dotNET_Relationships.Models
{
    [Table("Orders")]
    public class Order
    {
        public int Id { get; set; }
        public required string Name { get; set; } = string.Empty;
        public required int Quantity { get; set; }
        public required DateTime CreatedDate { get; set; }
        public required DateOnly DeliveryDate { get; set; }
        public required int ProductId { get; set; }
        public required Product Product { get; set; } = null!;
    }
}
