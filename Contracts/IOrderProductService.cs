using dotNET_Relationships.Models;

namespace dotNET_Relationships.Contracts
{
    public interface IOrderProductService
    {
        public Task CreateOrderOfProduct(Order order, Product product, int quantity);
    }
}
