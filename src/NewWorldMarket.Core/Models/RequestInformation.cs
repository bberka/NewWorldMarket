using System.ComponentModel.DataAnnotations;

namespace NewWorldMarket.Core.Models;

public class RequestInformation
{
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