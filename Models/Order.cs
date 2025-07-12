using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotNET_Relationships.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; } = string.Empty;
        public required DateTime CreatedDate { get; set; }
        public required DateOnly DeliveryDate { get; set; }
        public PaymentDetail? PaymentDetail { get; set; }
        //public required int ProductId { get; set; }
        //public Product Product { get; set; } = null!;
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }
}
