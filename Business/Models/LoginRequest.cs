using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class LoginRequest
{
    [Required(ErrorMessage = "You must enter your email address.")]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "You must enter your password.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}
