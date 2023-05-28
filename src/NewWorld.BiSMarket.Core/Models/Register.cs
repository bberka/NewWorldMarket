using NewWorld.BiSMarket.Core.Entity;
using System.ComponentModel.DataAnnotations;

namespace NewWorld.BiSMarket.Core.Models;

public class Register
{
    [MaxLength(32, ErrorMessage = "Username cannot be longer than 32 characters.")]
    [MinLength(4, ErrorMessage = "Username cannot be shorter than 4 characters.")]
    public string Username { get; set; }

    [MaxLength(32, ErrorMessage = "Password cannot be longer than 32 characters.")]
    [MinLength(6,ErrorMessage = "Password cannot be shorter than 6 characters.")]
    public string Password { get; set; }
    [Display(Name = "Password Confirm")]
    public string PasswordConfirm { get; set; }

    [MaxLength(512)]

    [EmailAddress]
    [Display(Name = "Email Address")]

    public string Email { get; set; }

}