using dotNET_Relationships.Data;
using dotNET_Relationships.DTOs;
using dotNET_Relationships.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotNET_Relationships.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(AppDbContext dbContext) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await dbContext.Products
                .Include(x => x.OrderProducts)
                    .ThenInclude(op => op.Order)
                .Select(o => new
                {
                    Id = o.Id,
                    Name = o.Name,
                    Description = o.Description,
                    Price = o.Price,
                    Orders = o.OrderProducts.Select(p => new
                    {
                        Id = p.Order.Id,
                        Name = p.Order.Name,
                        QuantityOrdered = p.Quantity,
                        CreatedDate = p.Order.CreatedDate,
                        DeliveryDate = p.Order.DeliveryDate
                    })
                })
                .ToListAsync();
            return Ok(products);
        }

        [HttpPost("create-product")]
        public async Task<IActionResult> AddProduct(ProductDto productDto)
        {
            Console.WriteLine(productDto);
            var product = new Product() { 
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
            };
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            return Ok(product);
        }
    }
}
