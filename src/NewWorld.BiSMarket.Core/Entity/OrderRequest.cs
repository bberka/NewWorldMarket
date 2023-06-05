using System.ComponentModel.DataAnnotations;
using EasMe.EntityFrameworkCore;

namespace NewWorldMarket.Core.Entity;

public class OrderRequest : IEntity
{
    [Key] public Guid Guid { get; set; }

    public DateTime RegisterDate { get; set; } = DateTime.Now;

    public DateTime? CancelDate { get; set; }
    public bool IsCancelled => CancelDate != null;
    public bool IsCompleted => IsCompletionVerifiedByRequester && IsCompletionVerifiedByOrderOwner;
    public bool IsCompletionVerifiedByRequester { get; set; } = false;

    public bool IsCompletionVerifiedByOrderOwner { get; set; } = false;

    //FK
    public Guid OrderGuid { get; set; }
    public Guid CharacterGuid { get; set; }


    //Virtual
    public virtual Character Character { get; set; } = null!;
    public virtual Order Order { get; set; } = null!;
}