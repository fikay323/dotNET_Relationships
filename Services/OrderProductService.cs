using dotNET_Relationships.Contracts;
using dotNET_Relationships.Data;
using dotNET_Relationships.Models;

namespace dotNET_Relationships.Services
{
    public class OrderProductService(AppDbContext dbContext) : IOrderProductService
    {
        public async Task CreateOrderOfProduct(Order order, Product product, int quantity)
        {
            var orderProduct = new OrderProduct()
            {
                OrderId = order.Id,
                ProductId = product.Id,
                Quantity = quantity
            };

            await dbContext.OrderProducts.AddAsync(orderProduct);
            await dbContext.SaveChangesAsync();
        }
    }
}
