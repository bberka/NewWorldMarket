using EasMe;
using NewWorld.BiSMarket.Core.Entity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using EasMe.Extensions;

namespace NewWorld.BiSMarket.Core.Models;

public class CreateSellOrder
{
    public List<Character> CharactersListForView { get; set; } = new();
    public Guid UserGuid { get; set; }
    [Display(Name = "Character")]
    public Guid CharacterGuid { get; set; }
    public Guid ImageGuid { get; set; }
    public float Price { get; set; }
    [Display(Name = "Estimated Delivery Time (Hour)")]
    public int EstimatedDeliveryTimeHour { get; set; }

    /// <summary>
    /// 0: Buy, 1: Sell
    /// </summary>
    public int Type => 1;

    [Display(Name = "Item")]
    public int ItemType { get; set; }
    /// <summary>
    /// Format: (AttributeType:AttributeValue,AttributeType:AttributeValue)
    /// </summary>
    [MaxLength(12)]
    public string Attributes { get; set; } = string.Empty;

    /// <summary>
    /// GemList.txt GemLineNumber
    /// </summary>
    [Display(Name = "Gem")]
    public int GemId { get; set; } = -1;
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
    public int Rarity { get; set; } = -1;
    public int Tier { get; set; } = -1;
    public int GearScore { get; set; } = -1;
    public int LevelRequirement { get; set; } = -1;
    public string ImageBytesBase64String { get; set; }
    public string UniqueHash => $"{ItemType}|{Attributes}|{Perks}".XXHashAsHexString();
    public string GetImageUrl() => $"data:image/png;base64,{ImageBytesBase64String}";

}