using dotNET_Relationships.Contracts;
using dotNET_Relationships.Data;
using dotNET_Relationships.DTOs;
using dotNET_Relationships.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace dotNET_Relationships.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(AppDbContext dbContext, IPaymentDetailService paymentDetailService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await dbContext.Orders
                .Include(o => o.Product)
                .Include(o => o.PaymentDetail)
                .Select(o => new {
                    Id = o.Id,
                    Name = o.Name,
                    Quantity = o.Quantity,
                    CreatedDate = o.CreatedDate,
                    DeliveryDate = o.DeliveryDate,
                    PaymentDetail = o.PaymentDetail == null ? null : new
                    {
                        o.PaymentDetail.OrderId,
                        o.PaymentDetail.PaymentMethod,
                        o.PaymentDetail.Status
                    },
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
                ProductId = orderDto.ProductId
            };
            await dbContext.AddAsync(order);
            await dbContext.SaveChangesAsync();

            await paymentDetailService.CreatePaymentDetail(order, "Cash");

            var createdOrderWithDetails = await dbContext.Orders
                .Include(o => o.Product)
                .Include(o => o.PaymentDetail)
                .Select(o => new {
                    Id = o.Id,
                    Name = o.Name,
                    Quantity = o.Quantity,
                    CreatedDate = o.CreatedDate,
                    DeliveryDate = o.DeliveryDate,
                    Product = o.Product == null ? null : new
                    {
                        Id = o.Product.Id,
                        Name = o.Product.Name
                    },
                    PaymentDetail = o.PaymentDetail == null ? null : new
                    {
                        OrderId = o.PaymentDetail.OrderId,
                        PaymentMethod = o.PaymentDetail.PaymentMethod,
                        Status = o.PaymentDetail.Status
                    }
                })
                .FirstOrDefaultAsync(o => o.Id == order.Id);

            if (createdOrderWithDetails == null) return StatusCode(500, "Failed to retrieve newly created order details.");

            return CreatedAtAction(nameof(GetOrders), new { id = order.Id }, createdOrderWithDetails);
        }
    }
}
