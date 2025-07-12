using System.ComponentModel.DataAnnotations;

namespace dotNET_Relationships.Models
{
    public enum OrderStatus
    {
        Pending,
        Success,
        Failed
    }
    public class PaymentDetail
    {
        [Key]
        public int OrderId { get; set; }
        public required string PaymentMethod { get; set; } = string.Empty;
        public required OrderStatus Status = OrderStatus.Pending;
        public Order Order = null!;
    }
}
