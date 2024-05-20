using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Order
{
    [Key]
    public long OrderID { get; set; }

    [Required]
    public long UserID { get; set; }

    [Required]
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(50)]
    public string OrderStatus { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [Required]
    [MaxLength(500)]
    public string ShippingAddress { get; set; }

    [Required]
    [MaxLength(500)]
    public string BillingAddress { get; set; }

    [Required]
    [MaxLength(50)]
    public string PaymentMethod { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
