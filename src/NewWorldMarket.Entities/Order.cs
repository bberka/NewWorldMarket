using System.ComponentModel.DataAnnotations;
using EasMe.EntityFrameworkCore;

namespace NewWorldMarket.Entities;

public class Order : IEntity
{
    [Key] public Guid Guid { get; set; }

    [MaxLength(32)] public string ShortId { get; set; }

    /// <summary>
    ///     0: Buy, 1: Sell
    /// </summary>
    public int Type { get; set; }

    public bool IsValid { get; set; } = true;
    public DateTime RegisterDate { get; set; } = DateTime.Now;
    public DateTime? LastUpdateDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public DateTime? CancelledDate { get; set; }


    public bool IsLimitedToVerifiedUsers { get; set; } = false;

    public int EstimatedDeliveryTimeHour { get; set; }
    public float Price { get; set; }

    public Guid CharacterGuid { get; set; }
    public Character Character { get; set; }

    public Guid ImageGuid { get; set; }
    public Image Image { get; set; } = null!;

    public int Region { get; set; }
    public int Server { get; set; }

    public int ItemType { get; set; }

    ///// <summary>
    /////     Format: (AttributeType:AttributeValue,AttributeType:AttributeValue)
    ///// </summary>
    ////[MaxLength(12)]
    public int Attribute1 { get; set; } = -1;
    public int Attribute2 { get; set; } = -1;
    public int Perk1 { get; set; } = -1;
    public int Perk2 { get; set; } = -1;
    public int Perk3 { get; set; } = -1;

    /// <summary>
    ///     GemList.txt GemLineNumber
    /// </summary>
    public int GemId { get; set; } = 0;

    /// <summary>
    ///     This value can not be known by OCR, it is set by the user. Default is true. There are only few cases where this is
    ///     false.
    /// </summary>
    public bool IsGemChangeable { get; set; } = true;

    /// <summary>
    ///     Format: PerkList.txt (PerkLineNumber,PerkLineNumber,PerkLineNumber)
    /// </summary>
    //[MaxLength(12)]
    //public string Perks { get; set; } = string.Empty;
    public bool IsNamed { get; set; } = false;

    public int Rarity { get; set; } = 0;
    public int Tier { get; set; } = 0;
    public int GearScore { get; set; } = 0;
    public int LevelRequirement { get; set; } = 0;

    [MaxLength(512)] public string Hash { get; set; } = string.Empty;


    public bool IsCancelled() => CancelledDate != null;
    public bool IsCompleted() => CompletedDate != null;
    public bool IsExpired() => ExpirationDate != null && ExpirationDate < DateTime.Now;
    public bool IsViewable() => !IsExpired() && !IsCancelled() && !IsCompleted() && IsValid;

}