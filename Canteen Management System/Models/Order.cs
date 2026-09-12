using System.ComponentModel.DataAnnotations;

namespace CanteenWeb.Models;

public class Order
{
    [Key]
    public int OrderId { get; set; }

    // ==========================================
    // CUSTOMER
    // ==========================================

    public int CustomerId { get; set; }

    public Customer? Customer { get; set; }


    // ==========================================
    // ORDER INFORMATION
    // ==========================================

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = "Pending";


    // ==========================================
    // ORDER ITEMS
    // ==========================================

    public ICollection<OrderItem> OrderItems { get; set; }
        = new List<OrderItem>();


    // ==========================================
    // PAYMENT
    // ==========================================

    public Payment? Payment { get; set; }
}