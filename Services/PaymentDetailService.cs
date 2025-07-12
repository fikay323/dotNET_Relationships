using dotNET_Relationships.Contracts;
using dotNET_Relationships.Data;
using dotNET_Relationships.Models;

namespace dotNET_Relationships.Services
{
    public class PaymentDetailService(AppDbContext dbContext) : IPaymentDetailService
    {
        public async Task CreatePaymentDetail(Order order, string paymentMethod)
        {
            var paymentDetail = new PaymentDetail
            {
                OrderId = order.Id,
                PaymentMethod = paymentMethod,
                Status = OrderStatus.Success
            };
            await dbContext.PaymentDetails.AddAsync(paymentDetail);
            await dbContext.SaveChangesAsync();
        }
    }
}
