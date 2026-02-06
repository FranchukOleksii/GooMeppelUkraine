using System.ComponentModel.DataAnnotations;

namespace GooMeppelUkraine.Web.Models;

public class AdminCreateUserVm
{
    [Required, EmailAddress]
    public string Email { get; set; } = default!;

    [Required, MinLength(6)]
    public string Password { get; set; } = default!;

    [Required]
    public string Role { get; set; } = default!;
}

