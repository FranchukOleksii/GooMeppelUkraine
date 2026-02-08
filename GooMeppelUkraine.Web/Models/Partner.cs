using System.ComponentModel.DataAnnotations;

namespace GooMeppelUkraine.Web.Models
{
    public class Partner
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = default!;

        public string? Url { get; set; }
        public string? LogoUrl { get; set; }

        [Required, MaxLength(5)]
        public string Language { get; set; } = "uk";
    }
}
