using System.ComponentModel.DataAnnotations;
using EasMe;

namespace NewWorld.BiSMarket.Core.Models;

public class Item
{
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
    public byte Rarity { get; set; } = 0;
    public byte Tier { get; set; } = 0;
    public int GearScore { get; set; } = 0;
    public byte LevelRequirement { get; set; } = 0;

    public string UniqueHash => $"{ItemType}|{Attributes}|{GemId}|{IsEmptySocket}|{Perks}|{IsNamed}|{Rarity}|{Tier}|{GearScore}|{LevelRequirement}".XXHashAsHexString();

}