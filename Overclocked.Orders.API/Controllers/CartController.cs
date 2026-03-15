using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Overclocked.Orders.API.Data;
using Overclocked.Orders.API.DTOs;
using Overclocked.Orders.API.Models;
using System.Security.Claims;
using System.Text.Json;

namespace Overclocked.Orders.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CartController> _logger;

        public CartController(AppDbContext context, ILogger<CartController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("GetCart")]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var cart = await _context.Carts
                    .Include(c => c.Items)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null)
                {
                    _logger.LogInformation("No cart found for user {UserId}, returning empty cart", userId);
                    return Ok(new CartDto());
                }

                using var httpClient = new HttpClient();
                var cartItems = new List<CartItemDto>();

                for (int i = 0; i < cart.Items.Count; i++)
                {
                    var item = cart.Items[i];
                    var productsHost = Environment.GetEnvironmentVariable("PRODUCTS_API_URL") ?? "http://localhost:5001";
                    var productResponse = await httpClient.GetAsync($"{productsHost}/api/products/GetProducts/{item.ProductId}");

                    string productName = "Unknown";
                    string productSku = "N/A";
                    decimal productPrice = 0;
                    string? productImage = null;

                    if (productResponse.IsSuccessStatusCode)
                    {
                        var productJson = await productResponse.Content.ReadAsStringAsync();
                        var jsonOptions = new JsonSerializerOptions();
                        jsonOptions.PropertyNameCaseInsensitive = true;
                        var product = JsonSerializer.Deserialize<ProductResponseDto>(productJson, jsonOptions);

                        if (product != null)
                        {
                            productName = product.Name;
                            productSku = product.SKU;
                            productPrice = product.Price;
                            productImage = product.Image;
                        }
                    }

                    cartItems.Add(new CartItemDto
                    {
                        CartItemId = item.CartItemId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        ProductName = productName,
                        ProductSku = productSku,
                        ProductPrice = productPrice,
                        ProductImage = productImage
                    });
                }

                var cartDto = new CartDto
                {
                    CartId = cart.CartId,
                    Items = cartItems
                };

                _logger.LogInformation("Retrieved cart for user {UserId} with {Count} items", userId, cartDto.Items.Count);
                return Ok(cartDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cart for user");
                return StatusCode(500, new { message = "Something went wrong.", error = ex.Message });
            }
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var cart = await _context.Carts
                    .Include(c => c.Items)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null)
                {
                    cart = new Cart { UserId = userId! };
                    _context.Carts.Add(cart);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Created new cart for user {UserId}", userId);
                }

                var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);

                if (existingItem != null)
                {
                    existingItem.Quantity += request.Quantity;
                    _logger.LogInformation("Updated quantity for product {ProductId} in cart", request.ProductId);
                }
                else
                {
                    var newItem = new CartItem
                    {
                        CartId = cart.CartId,
                        ProductId = request.ProductId,
                        Quantity = request.Quantity
                    };
                    _context.CartItems.Add(newItem);
                    _logger.LogInformation("Added product {ProductId} to cart for user {UserId}", request.ProductId, userId);
                }

                await _context.SaveChangesAsync();
                return Ok(new { message = "Item added to cart." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product {ProductId} to cart", request.ProductId);
                return StatusCode(500, new { message = "Something went wrong.", error = ex.Message });
            }
        }

        [HttpDelete("RemoveFromCart/{cartItemId}")]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var cartItem = await _context.CartItems
                    .Include(i => i.Cart)
                    .FirstOrDefaultAsync(i => i.CartItemId == cartItemId && i.Cart.UserId == userId);

                if (cartItem == null)
                {
                    _logger.LogWarning("Cart item {CartItemId} not found for user {UserId}", cartItemId, userId);
                    return NotFound(new { message = "Cart item not found." });
                }

                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Removed cart item {CartItemId} for user {UserId}", cartItemId, userId);
                return Ok(new { message = "Item removed from cart." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing cart item {CartItemId}", cartItemId);
                return StatusCode(500, new { message = "Something went wrong.", error = ex.Message });
            }
        }
    }
}