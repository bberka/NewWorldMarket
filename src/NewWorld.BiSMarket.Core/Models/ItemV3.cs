using System.ComponentModel.DataAnnotations;
using EasMe;

namespace NewWorld.BiSMarket.Core.Models;

public class ItemV3
{


    public string ItemName { get; set; } = string.Empty;
    public int ItemType { get; set; } = -1;
    /// <summary>
    /// GemList.txt GemLineNumber
    /// </summary>
    public int GemId { get; set; } = -1;
    public int Attribute_1 { get; set; } = -1;
    public int Attribute_2 { get; set; } = -1;
    public int Perk_1 { get; set; } = -1;
    public int Perk_2 { get; set; } = -1;
    public int Perk_3 { get; set; } = -1;
    public bool? IsNamed { get; set; } = null;
    public int Rarity { get; set; } = -1;
    public int Tier { get; set; } = -1;
    public int GearScore { get; set; } = -1;
    public int LevelRequirement { get; set; } = -1;
    /// <summary>
    /// Hash of ItemType, Attributes and Perks. Used to identify the item's type, attributes and perks.
    /// </summary>
    public string UniqueHash => $"{ItemType}|{AttributeString}|{PerkString}".XXHashAsHexString();
    public string AttributeString => $"{Attribute_1},{Attribute_2}";
    public string PerkString => $"{Perk_1},{Perk_2},{Perk_3}";

}