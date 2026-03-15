namespace Overclocked.Orders.API.DTOs
{
    public class CartDto
    {
        public int CartId { get; set; }
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
    }

    public class CartItemDto
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductSku { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public string? ProductImage { get; set; }
    }

    public class AddToCartDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}