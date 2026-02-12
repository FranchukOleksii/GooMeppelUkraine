using System.ComponentModel.DataAnnotations;

namespace GooMeppelUkraine.Web.Models
{
    public class UserProfile
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? DisplayName { get; set; }
    }
}
