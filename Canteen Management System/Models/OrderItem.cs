using System.ComponentModel.DataAnnotations;

namespace CanteenWeb.Models;

public class OrderItem
{
    [Key]
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int FoodId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public Order? Order { get; set; }

    public FoodItem? FoodItem { get; set; }
}