using System.ComponentModel.DataAnnotations;

namespace GooMeppelUkraine.Web.Models;

public class Article
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = default!;

    [Required]
    public string Content { get; set; } = default!;

    [Required, MaxLength(5)]
    public string Language { get; set; } = "NL"; 

    public bool IsPublished { get; set; } = false;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? PublishedAtUtc { get; set; }

    [Required, MaxLength(250)]
    public string Slug { get; set; } = default!;

    [MaxLength(70)]
    public string? MetaTitle { get; set; }

    [MaxLength(160)]
    public string? MetaDescription { get; set; }

}
