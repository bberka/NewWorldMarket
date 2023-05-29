using System.ComponentModel.DataAnnotations;
using EasMe.EntityFrameworkCore;
using NewWorld.BiSMarket.Core.Models;

namespace NewWorld.BiSMarket.Core.Entity;

public class User : IEntity
{
    [Key]
    public Guid Guid { get; set; }
    [Display(Name = "Register Date")]
    public DateTime RegisterDate { get; set; } = DateTime.Now;

    [Display(Name = "Last Update Date")]
    public DateTime? LastUpdateDate { get; set; }

    [Display(Name = "Last Login Date")]
    public DateTime? LastLoginDate { get; set; }
    
    public bool IsValid { get; set; } = true;
    [MaxLength(32)]
    public string Username { get; set; }

    [MaxLength(512)]
    [Display(Name = "Password")]
    public string PasswordHash { get; set; }

    [MaxLength(512)]
    [EmailAddress]
    [Display(Name = "Email Address")]
    public string Email { get; set; }

    public bool IsEmailVerified { get; set; } = false;
    public bool IsVerifiedAccount { get; set; } = false;

    [MaxLength(512)]
    public string? DiscordId { get; set; }

    [MaxLength(512)]
    public string? SteamId { get; set; }


    public ICollection<Character> Characters { get; set; } = new List<Character>();
    public ICollection<Image> Images { get; set; } = new List<Image>();
}