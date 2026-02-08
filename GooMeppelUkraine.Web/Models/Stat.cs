using System.ComponentModel.DataAnnotations;

namespace GooMeppelUkraine.Web.Models
{
    public class Stat
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Label { get; set; } = default!;

        [Required, MaxLength(20)]
        public string Value { get; set; } = default!;

        [Required, MaxLength(5)]
        public string Language { get; set; } = "uk";
    }

}
