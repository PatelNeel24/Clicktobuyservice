namespace Clicktobuyservice.Models
{
    public class ShoppingCart
    {
        public long ShoppingCartID { get; set; }
        public long ProductID { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public string? Price { get; set; } // Assuming price can be stored as string
        public byte[]? TotalPrice { get; set; } // Assuming total price can be stored as byte array
        public long? UserID { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }

}
