using System.ComponentModel.DataAnnotations;

namespace CanteenWeb.Models;

public class Admin
{
    [Key]
    public int AdminId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}