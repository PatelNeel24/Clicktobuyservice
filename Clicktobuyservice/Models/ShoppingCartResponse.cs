namespace Clicktobuyservice.Models
{
    public class ShoppingCartResponse
    {
        public long ShoppingCartId { get; set; }
        public string? ProductImage { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public string? Price { get; set; }
        
    }
}
