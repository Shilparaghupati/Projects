using System.ComponentModel.DataAnnotations;

namespace CanteenWeb.Models;

public class Payment
{
    [Key]
    public int PaymentId { get; set; }

    public int OrderId { get; set; }

    public decimal Amount { get; set; }

    public string PaymentMethod { get; set; } = string.Empty;

    public string PaymentStatus { get; set; } = "Pending";

    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    public Order? Order { get; set; }
}