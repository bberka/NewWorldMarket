using NewWorldMarket.Core.Constants;
using System.ComponentModel.DataAnnotations;
using EasMe.EntityFrameworkCore;

namespace NewWorldMarket.Core.Entity;

public class ResetPasswordToken : IEntity
{
    [Key]
    public Guid Guid { get; set; }
    public DateTime RegisterDate { get; set; } = DateTime.Now;
    public DateTime ExpireDate { get; set; }
    public Guid UserGuid { get; set; }
    [MaxLength(ConstMgr.ResetPasswordTokenLength)]
    public string Token { get; set; } = string.Empty;
    public bool IsUsed { get; set; }
    public bool IsExpired() => DateTime.Now > ExpireDate;
}