using System.ComponentModel.DataAnnotations;
using EasMe;

namespace NewWorld.BiSMarket.Core.Models;

public class Item
{
    public string ItemName { get; set; } = string.Empty;

    public int ItemType { get; set; } = -1;
    /// <summary>
    /// Format: (AttributeType:AttributeValue,AttributeType:AttributeValue)
    /// </summary>
    [MaxLength(12)]
    public string Attributes { get; set; } = string.Empty;
    /// <summary>
    /// GemList.txt GemLineNumber
    /// </summary>
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
    public bool? IsNamed { get; set; } = null;
    public int Rarity { get; set; } = -1;
    public int Tier { get; set; } = -1;
    public int GearScore { get; set; } = -1;
    public int LevelRequirement { get; set; } = -1;

    public string UniqueHash => $"{ItemType}|{Attributes}|{Perks}".XXHashAsHexString();

}