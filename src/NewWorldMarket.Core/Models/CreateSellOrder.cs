using System.ComponentModel.DataAnnotations;
using EasMe;
using NewWorldMarket.Entities;

namespace NewWorldMarket.Core.Models;

public class CreateSellOrder
{
    public List<Character> CharactersListForView { get; set; } = new();
    public Guid UserGuid { get; set; }

    [Display(Name = "Character")] public Guid CharacterGuid { get; set; }

    public Guid ImageGuid { get; set; }
    public float Price { get; set; }

    [Display(Name = "Estimated Delivery Time (Hour)")]
    public int EstimatedDeliveryTimeHour { get; set; }

    /// <summary>
    ///     0: Buy, 1: Sell
    /// </summary>
    public int Type => 1;

    [Display(Name = "Item")] public int ItemType { get; set; }

    /// <summary>
    ///     Format: (AttributeType:AttributeValue,AttributeType:AttributeValue)
    /// </summary>
    //[MaxLength(12)]
    //public string Attributes { get; set; } = string.Empty;

    /// <summary>
    ///     GemList.txt GemLineNumber
    /// </summary>
    [Display(Name = "Gem")]
    public int GemId { get; set; } = -1;

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
    [Display(Name = "I confirm the information matches with screenshot")]
    public bool Confirmation { get; set; } = false;

    public int Rarity { get; set; } = -1;
    public int Tier { get; set; } = -1;
    public int GearScore { get; set; } = -1;

    public int LevelRequirement { get; set; } = -1;

    //public string ImageBytesBase64String { get; set; }
    //public string UniqueHash => $"{ItemType}|{AttributeString}|{PerkString}".XXHashAsHexString();

    public string AttributeString
    {
        get
        {
            var text = string.Empty;
            if (Attribute1 != -1)
                text += $"{Attribute1},";
            if (Attribute2 != -1)
                text += $"{Attribute2},";
            return text.TrimEnd(',');
        }
    }

    public string PerkString
    {
        get
        {
            var text = string.Empty;
            if (Perk1 != -1)
                text += $"{Perk1},";
            if (Perk2 != -1)
                text += $"{Perk2},";
            if (Perk3 != -1)
                text += $"{Perk3},";
            return text.TrimEnd(',');
        }
    }

    [Display(Name = "Attribute")] public int Attribute1 { get; set; } = -1;

    [Display(Name = "Attribute")] public int Attribute2 { get; set; } = -1;

    [Display(Name = "Perk 1")] public int Perk1 { get; set; } = -1;

    [Display(Name = "Perk 2")] public int Perk2 { get; set; } = -1;

    [Display(Name = "Perk 3")] public int Perk3 { get; set; } = -1;

    //public string GetImageUrl()
    //{
    //    return $"data:image/png;base64,{ImageBytesBase64String}";
    //}
}