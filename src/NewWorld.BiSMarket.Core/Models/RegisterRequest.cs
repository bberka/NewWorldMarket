using NewWorld.BiSMarket.Core.Entity;
using System.ComponentModel.DataAnnotations;

namespace NewWorld.BiSMarket.Core.Models;

public class RegisterRequest
{
    [MaxLength(32)]
    public string Username { get; set; }

    [MaxLength(64)]
    public string Password { get; set; }
    [MaxLength(64)]
    public string PasswordConfirm { get; set; }

    [MaxLength(512)]
    [EmailAddress]
    public string Email { get; set; }

}