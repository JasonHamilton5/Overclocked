using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Overclocked.API.Data;
using Overclocked.API.DTOs;

namespace Overclocked.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(AppDbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _context.Products.ToListAsync();

                if (!products.Any())
                {
                    _logger.LogWarning("No products found in database");
                    return NotFound(new { message = "No products found." });
                }

                _logger.LogInformation("Retrieved {Count} products", products.Count);
                var result = products.Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    SKU = p.SKU,
                    Price = p.Price,
                    Qty = p.Qty,
                    Image = p.Image,
                    Category = p.Category,
                    IsFeatured = p.IsFeatured
                }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products");
                return StatusCode(500, new { message = "Something went wrong.", error = ex.Message });
            }
        }

        [HttpGet("GetProducts/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    _logger.LogWarning("Product with ID {Id} not found", id);
                    return NotFound(new { message = "Product not found." });
                }

                _logger.LogInformation("Retrieved product {Id}", id);
                var result = new ProductDto
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    SKU = product.SKU,
                    Price = product.Price,
                    Qty = product.Qty,
                    Image = product.Image,
                    Category = product.Category,
                    IsFeatured = product.IsFeatured
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product {Id}", id);
                return StatusCode(500, new { message = "Something went wrong.", error = ex.Message });
            }
        }

        [HttpGet("GetFeaturedProducts")]
        public async Task<IActionResult> GetFeaturedProducts()
        {
            try
            {
                var products = await _context.Products
                    .Where(p => p.IsFeatured == true)
                    .ToListAsync();

                if (!products.Any())
                {
                    _logger.LogWarning("No featured products found");
                    return NotFound(new { message = "No featured products found." });
                }

                _logger.LogInformation("Retrieved {Count} featured products", products.Count);
                var result = products.Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    SKU = p.SKU,
                    Price = p.Price,
                    Qty = p.Qty,
                    Image = p.Image,
                    Category = p.Category,
                    IsFeatured = p.IsFeatured
                }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving featured products");
                return StatusCode(500, new { message = "Something went wrong.", error = ex.Message });
            }
        }
    }
}