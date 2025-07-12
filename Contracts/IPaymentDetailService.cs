using dotNET_Relationships.Models;

namespace dotNET_Relationships.Contracts
{
    public interface IPaymentDetailService
    {
        public Task CreatePaymentDetail(Order order, string paymentMethod); 
    }
}
