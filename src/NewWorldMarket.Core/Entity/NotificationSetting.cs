using System.ComponentModel.DataAnnotations;

namespace NewWorldMarket.Core.Entity;

public class NotificationSetting
{
    [Key]
    public Guid UserGuid { get; set; }
    public DateTime? LastUpdateDate { get; set; }
    public bool EmailNotifications { get; set; }
    public bool DiscordNotifications { get; set; }

}