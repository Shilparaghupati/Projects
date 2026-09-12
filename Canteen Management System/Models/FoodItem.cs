using System.ComponentModel.DataAnnotations;

namespace CanteenWeb.Models;

public class FoodItem
{
    [Key]
    public int FoodId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Category { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public bool IsAvailable { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}