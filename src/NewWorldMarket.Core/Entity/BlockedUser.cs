using System.ComponentModel.DataAnnotations;
using EasMe.EntityFrameworkCore;

namespace NewWorldMarket.Core.Entity;

public class BlockedUser : IEntity
{
    [Key] public Guid Guid { get; set; }

    public DateTime RegisterDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; }
    public Guid UserGuid { get; set; }
    public int BlockCode { get; set; }

    [MaxLength(128)] 
    public string Memo { get; set; }

    public virtual User User { get; set; } = null!;
}