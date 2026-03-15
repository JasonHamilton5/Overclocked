namespace Overclocked.API.DTOs
{
    public class CartDto
    {
        public int CartId { get; set; }
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
    }
}