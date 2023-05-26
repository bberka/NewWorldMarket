﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EasMe.EntityFrameworkCore;

namespace NewWorld.BiSMarket.Core.Entity;


public class Order : IEntity
{
    [Key]
    public Guid Guid { get; set; }

    /// <summary>
    /// 0: Buy, 1: Sell
    /// </summary>
    public byte Type { get; set; }
    public bool IsValid { get; set; } = true;
    public DateTime RegisterDate { get; set; } = DateTime.Now;
    public DateTime? LastUpdateDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public DateTime? CancelledDate { get; set; }
    public bool IsExpired => ExpirationDate != null && ExpirationDate < DateTime.Now;
    public bool IsCancelled => CancelledDate != null;
    public bool IsCompleted => CompletedDate != null;
    public bool IsViewable => !IsExpired && !IsCancelled && !IsCompleted && IsValid;

    public bool IsLimitedToVerifiedUsers { get; set; } = false;

    public int EstimatedDeliveryTimeHour { get; set; } 
    public float Price { get; set; }

    public Guid CharacterGuid { get; set; }
    public Character Character { get; set; } = null!;

    public Guid ImageGuid { get; set; }
    public Image Image { get; set; } = null!;

    public int Region { get; set; }
    public int Server { get; set; }

    public byte ItemType { get; set; }
    /// <summary>
    /// Format: (AttributeType:AttributeValue,AttributeType:AttributeValue)
    /// </summary>
    [MaxLength(12)]
    public string Attributes { get; set; } = string.Empty;
    /// <summary>
    /// GemList.txt GemLineNumber
    /// </summary>
    public int GemId { get; set; } = 0;
    public bool IsEmptySocket { get; set; } = false;
    /// <summary>
    /// This value can not be known by OCR, it is set by the user. Default is true. There are only few cases where this is false.
    /// </summary>
    public bool IsGemChangeable { get; set; } = true;
    /// <summary>
    /// Format: PerkList.txt (PerkLineNumber,PerkLineNumber,PerkLineNumber)
    /// </summary>
    [MaxLength(12)]
    public string Perks { get; set; } = string.Empty;
    public bool IsNamed { get; set; } = false;
    public int Rarity { get; set; } = 0;
    public int Tier { get; set; } = 0;
    public int GearScore { get; set; } = 0;
    public int LevelRequirement { get; set; } = 0;

    [MaxLength(512)]
    public string Hash { get; set; } = string.Empty;
}