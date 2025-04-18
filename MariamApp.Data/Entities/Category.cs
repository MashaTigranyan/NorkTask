﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MariamApp.Data.Entities;

[Table("categories")]
public class Category
{
    [Key]
    public int Id { get; set; }
    
    [Required, MaxLength(100)]
    public string Name { get; set; }
    
    public ICollection<Product> Products { get; set; } = new List<Product>();
}