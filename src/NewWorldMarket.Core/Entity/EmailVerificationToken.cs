using System.ComponentModel.DataAnnotations;
using EasMe.EntityFrameworkCore;
using NewWorldMarket.Core.Constants;

namespace NewWorldMarket.Core.Entity;

public class EmailVerificationToken : IEntity
{
    [Key]
    public Guid Guid { get; set; }
    public DateTime RegisterDate { get; set; } = DateTime.Now;
    public DateTime ExpireDate { get; set; }
    public Guid UserGuid { get; set; }
    [MaxLength(ConstMgr.EmailVerificationTokenLength)]
    public string Token { get; set; } = string.Empty;
    public bool IsUsed { get; set; }
    public bool IsExpired() => DateTime.Now > ExpireDate;
}