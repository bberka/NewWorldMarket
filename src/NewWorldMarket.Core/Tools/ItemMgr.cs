﻿using NewWorldMarket.Core.Constants;
using NewWorldMarket.Core.Models;

namespace NewWorldMarket.Core.Tools;

public class ItemMgr
{
    private static ItemMgr? Instance;

    public IReadOnlyCollection<ItemTypeModel> ItemList;

    private ItemMgr()
    {
        ItemList = Enum.GetValues(typeof(ItemType))
            .Cast<ItemType>()
            .Select(t => new ItemTypeModel
            {
                Key = t.ToString(),
                Name = t.ToString().Replace("_", " "),
                Id = (int)t
            }).ToList();
    }

    public static ItemMgr This
    {
        get
        {
            Instance ??= new ItemMgr();
            return Instance;
        }
    }

    public ItemTypeModel? GetItemTypeName(int type)
    {
        return ItemList.FirstOrDefault(t => t.Id == type);
    }

    public ItemTypeModel? GetItemTypeName(string key)
    {
        return ItemList.FirstOrDefault(t => t.Key == key);
    }

    public bool IsValid(int type)
    {
        return ItemList.Any(x => x.Id == type);
    }

    public bool IsValid(string key)
    {
        return ItemList.Any(x => x.Key == key);
    }
}