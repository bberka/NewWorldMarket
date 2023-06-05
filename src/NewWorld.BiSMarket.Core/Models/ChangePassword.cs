using System.ComponentModel.DataAnnotations;

namespace NewWorldMarket.Core.Models;

public class ChangePassword
{
    [MaxLength(64)]
    [Display(Name = "Current Password")]
    public string CurrentPassword { get; set; }

    [MaxLength(64)]
    [Display(Name = "New Password")]
    public string NewPassword { get; set; }

    [MaxLength(64)]
    [Display(Name = "New Password Confirm")]
    public string NewPasswordConfirm { get; set; }
}