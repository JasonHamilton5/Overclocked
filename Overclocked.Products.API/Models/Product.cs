namespace Overclocked.Products.API.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public string? Image { get; set; }
        public string? Category { get; set; }
        public bool IsFeatured { get; set; } = false;
    }
}