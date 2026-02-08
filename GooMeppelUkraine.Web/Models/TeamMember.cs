using System.ComponentModel.DataAnnotations;

namespace GooMeppelUkraine.Web.Models
{
    public class TeamMember
    {
        public int Id { get; set; }

        [Required, MaxLength(120)]
        public string Name { get; set; } = default!;

        [MaxLength(120)]
        public string? Role { get; set; }

        public string? PhotoUrl { get; set; }

        [Required, MaxLength(5)]
        public string Language { get; set; } = "uk";
    }
}
