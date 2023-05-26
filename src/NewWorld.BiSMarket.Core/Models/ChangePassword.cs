using System.ComponentModel.DataAnnotations;

namespace NewWorld.BiSMarket.Core.Models;

public class ChangePassword
{
    [MaxLength(64)]
    public string CurrentPassword { get; set; }
    [MaxLength(64)]
    public string NewPassword { get; set; }
    [MaxLength(64)]
    public string NewPasswordConfirm { get; set; }
}