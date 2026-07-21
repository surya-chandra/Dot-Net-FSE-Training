using System.ComponentModel.DataAnnotations;

namespace EFCoreLab02.Models;



public class Supplier
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    [EmailAddress]
    public string? Email { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
