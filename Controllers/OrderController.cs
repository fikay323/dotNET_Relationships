using dotNET_Relationships.Data;
using dotNET_Relationships.DTOs;
using dotNET_Relationships.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotNET_Relationships.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(AppDbContext dbContext) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await dbContext.Orders
                .Include(o => o.Product)
                .Select(o =>  new {
                    Id = o.Id,
                    Name = o.Name,
                    Quantity = o.Quantity,
                    CreatedDate = o.CreatedDate,
                    DeliveryDate = o.DeliveryDate,
                    Product = new {
                        Id = o.Product.Id,
                        Name = o.Product.Name
                    }
                })
                .ToListAsync();
            return Ok(orders);
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> AddOrder(OrderDto orderDto)
        {
            var product = await dbContext.Products.FindAsync(orderDto.ProductId);
            if (product == null) return NotFound("Product not found");

            var order = new Order()
            {
                Name = orderDto.Name,
                Quantity = orderDto.Quantity,
                CreatedDate = DateTime.UtcNow,
                DeliveryDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(4)),
                ProductId = orderDto.ProductId,
                Product = product
            };
            await dbContext.AddAsync(order);
            await dbContext.SaveChangesAsync();

            return Ok(order);
        }
    }
}
