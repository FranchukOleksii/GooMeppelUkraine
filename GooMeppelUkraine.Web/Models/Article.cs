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
    public string Language { get; set; } = "uk"; // uk / en / nl

    public bool IsPublished { get; set; } = false;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? PublishedAtUtc { get; set; }
}
