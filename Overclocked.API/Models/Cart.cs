namespace Overclocked.API.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}