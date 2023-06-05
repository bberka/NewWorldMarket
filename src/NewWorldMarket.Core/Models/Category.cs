using EasMe;
using NewWorldMarket.Core.Constants;

namespace NewWorldMarket.Core.Models;

public class Category
{
    public Category(MainCategoryType categoryType, ItemType itemType)
    {
        CategoryType = categoryType;
        ItemType = itemType;
    }

    public MainCategoryType CategoryType { get; }
    public ItemType ItemType { get; set; }
    public string ItemTypeReadableString => ItemType.ToString().Replace("_", " ");
    public string CategoryTypeReadableString => CategoryType.ToString().Replace("__", "-").Replace("_", " ");
    public string Hash => $"{CategoryType}:{ItemType}".ToString().XXHash().AsHexString();
}