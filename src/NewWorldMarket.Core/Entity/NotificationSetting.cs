using System.ComponentModel.DataAnnotations;
using EasMe.EntityFrameworkCore;

namespace NewWorldMarket.Core.Entity;

public class NotificationSetting : IEntity
{
    [Key]
    public Guid UserGuid { get; set; }
    public DateTime? LastUpdateDate { get; set; }
    public bool EmailNotifications { get; set; }
    public bool DiscordNotifications { get; set; }

}