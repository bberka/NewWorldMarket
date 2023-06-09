using System.ComponentModel.DataAnnotations;
using EasMe.EntityFrameworkCore;

namespace NewWorldMarket.Entities;

public class OrderReport : IEntity
{
    [Key]
    public Guid Guid { get; set; }
    public Guid OrderGuid { get; set; }
    public Order Order { get; set; } = null!;
    public Guid? UserGuid { get; set; }
    public User? User { get; set; } = null!;
    public DateTime RegisterDate { get; set; } = DateTime.Now;
    [MaxLength(1000)]
    public string Message { get; set; }
    public int State { get; set; } = 0; //Pending state
    public int Type { get; set; }

    public DateTime? LastUpdateDate { get; set; }
    [MaxLength(64)]
    public string? RemoteIpAddress { get; set; }
    [MaxLength(64)]
    public string? XRealIpAddress { get; set; }
    [MaxLength(64)]
    public string? XForwardedForIpAddress { get; set; }
    [MaxLength(64)]
    public string? CfConnectingIpAddress { get; set; }
    [MaxLength(128)]
    public string? UserAgent { get; set; }


}