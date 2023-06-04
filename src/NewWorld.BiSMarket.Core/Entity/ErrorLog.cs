using System.ComponentModel.DataAnnotations;

namespace NewWorld.BiSMarket.Core.Entity;

public class ErrorLog
{
    [Key] public Guid Guid { get; set; }

    public DateTime RegisterDate { get; set; } = DateTime.Now;
    public int LogType { get; set; }

    [MaxLength(512)] public string Message { get; set; } = string.Empty;

    [MaxLength(64)] public string? IpAddress { get; set; } = string.Empty;

    [MaxLength(128)] public string? UserAgent { get; set; } = string.Empty;

    [MaxLength(512)] public string? Exception { get; set; } = string.Empty;
}