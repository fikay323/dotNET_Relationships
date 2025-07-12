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
    public class OrderController(AppDbContext dbContext, IPaymentDetailService paymentDetailService, IOrderProductService orderProductService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await dbContext.Orders
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .Include(o => o.PaymentDetail)
                .Select(o => new {
                    Id = o.Id,
                    Name = o.Name,
                    CreatedDate = o.CreatedDate,
                    DeliveryDate = o.DeliveryDate,
                    Products = o.OrderProducts.Select(op => new {
                        Id = op.Product.Id,
                        Name = op.Product.Name,
                        QuantityOrdered = op.Quantity,
                        Price = op.Product.Price
                    }),
                    PaymentDetail = o.PaymentDetail == null ? null : new
                    {
                        OrderId = o.PaymentDetail.OrderId,
                        PaymentMethod = o.PaymentDetail.PaymentMethod,
                        Status = o.PaymentDetail.Status
                    }
                })
                .ToListAsync();
            return Ok(orders);
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> AddOrder(OrderDto orderDto)
        {
            if (orderDto.Products == null || !orderDto.Products.Any())
            {
                return BadRequest("Order must contain at least one product.");
            }

            var order = new Order()
            {
                Name = orderDto.Name,
                CreatedDate = DateTime.UtcNow,
                DeliveryDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(4)),
            };
            await dbContext.AddAsync(order);
            await dbContext.SaveChangesAsync();

            foreach(var itemDto in orderDto.Products)
            {
                var product = await dbContext.Products.FindAsync(itemDto.ProductId);
                if (product is null) return NotFound($"Product with ID {itemDto.ProductId} not found.");

                await orderProductService.CreateOrderOfProduct(order, product, itemDto.Quantity);
            }

            await paymentDetailService.CreatePaymentDetail(order, "Cash");

            var createdOrderWithDetails = await dbContext.Orders
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .Include(o => o.PaymentDetail)
                .Select(o => new {
                    Id = o.Id,
                    Name = o.Name,
                    CreatedDate = o.CreatedDate,
                    DeliveryDate = o.DeliveryDate,
                    Products = o.OrderProducts.Select( op => new {
                        Id = op.Product.Id,
                        Name = op.Product.Name,
                        QuantityOrdered = op.Quantity,
                        Price = op.Product.Price
                    }),
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
