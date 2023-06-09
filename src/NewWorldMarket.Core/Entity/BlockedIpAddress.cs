using System.ComponentModel.DataAnnotations;
using EasMe.EntityFrameworkCore;

namespace NewWorldMarket.Core.Entity;

public class BlockedIpAddress : IEntity
{
    [Key] 
    public Guid Guid { get; set; }

    public DateTime RegisterDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; }
    public int BlockCode { get; set; }

    [MaxLength(128)]
    public string Memo { get; set; }

    //TODO: Must check if remote address is public known VPNs or Proxies Ex: CloudFlare
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