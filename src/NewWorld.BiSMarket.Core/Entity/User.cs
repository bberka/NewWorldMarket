using System.ComponentModel.DataAnnotations;
using EasMe.EntityFrameworkCore;
using NewWorld.BiSMarket.Core.Models;

namespace NewWorld.BiSMarket.Core.Entity;

public class User : IEntity
{
    [Key]
    public Guid Guid { get; set; }
    public DateTime RegisterDate { get; set; } = DateTime.Now;
    public DateTime? LastUpdateDate { get; set; }
    public DateTime? LastLoginDate { get; set; }

    public bool IsValid { get; set; } = true;
    [MaxLength(32)]
    public string Username { get; set; }

    [MaxLength(512)]
    public string PasswordHash { get; set; }

    [MaxLength(512)]
    [EmailAddress]
    public string Email { get; set; }
    public bool IsEmailVerified { get; set; } = false;
    public bool IsVerifiedAccount { get; set; } = false;

    [MaxLength(512)]
    public string? DiscordId { get; set; }

    [MaxLength(512)]
    public string? SteamId { get; set; }


    public ICollection<Character> Characters { get; set; } = new List<Character>();
}