using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Product
{
    [Required]
    public int ID { get; set; }

    [Required]
    public string product_name { get; set; }

    [Required]
    public int stock { get; set; }

    [Required]
    public int category_id { get; set; }

    [Required]
    public decimal price { get; set; }

    [Required]
    public bool isVisible { get; set; }

    [Required]
    public DateTime createdAt { get; set; }

    [Required]
    public DateTime updatedAt { get; set; }

    [Required]
    [Url]
    public string url_image { get; set; }
}
