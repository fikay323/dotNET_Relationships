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
                .Include(x => x.Orders)
                .Select(o => new
                {
                    Id = o.Id,
                    Name = o.Name,
                    Description = o.Description,
                    Price = o.Price,
                    Orders = o.Orders.Select(p => new
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Quantity = p.Quantity,
                        CreatedDate = p.CreatedDate,
                        DeliveryDate = p.DeliveryDate
                    })
                })
                .ToListAsync();
            return Ok(products);
        }

        [HttpPost("create-product")]
        public async Task<IActionResult> AddProduct(ProductDto productDto)
        {
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
