namespace Overclocked.Orders.API.DTOs
{
    public class ProductResponseDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Image { get; set; }
    }
}