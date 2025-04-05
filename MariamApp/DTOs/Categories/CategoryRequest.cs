using System.ComponentModel.DataAnnotations;

namespace MariamApp.DTOs.Categories;

public class CategoryRequest
{
    [Required]
    public string Name { get; set; }
}