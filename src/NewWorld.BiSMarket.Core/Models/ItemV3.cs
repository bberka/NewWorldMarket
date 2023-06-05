using EasMe;

namespace NewWorldMarket.Core.Models;

public class ItemV3
{
    public string ItemName { get; set; } = string.Empty;
    public int ItemType { get; set; } = -1;

    /// <summary>
    ///     GemList.txt GemLineNumber
    /// </summary>
    public int GemId { get; set; } = -1;

    public int Attribute1 { get; set; } = -1;
    public int Attribute2 { get; set; } = -1;
    public int Perk1 { get; set; } = -1;
    public int Perk2 { get; set; } = -1;
    public int Perk3 { get; set; } = -1;
    public bool? IsNamed { get; set; } = null;
    public int Rarity { get; set; } = -1;
    public int Tier { get; set; } = -1;
    public int GearScore { get; set; } = -1;
    public int LevelRequirement { get; set; } = -1;

    /// <summary>
    ///     Hash of ItemType, Attributes and Perks. Used to identify the item's type, attributes and perks.
    /// </summary>
    public string UniqueHash => $"{ItemType}|{AttributeString}|{PerkString}".XXHashAsHexString();

    internal string AttributeString => $"{Attribute1},{Attribute2}";
    internal string PerkString => $"{Perk1},{Perk2},{Perk3}";
}