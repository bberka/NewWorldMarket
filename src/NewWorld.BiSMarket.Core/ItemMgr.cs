using EasMe.Extensions;
using NewWorld.BiSMarket.Core.Constants;
using NewWorld.BiSMarket.Core.Models;

namespace NewWorld.BiSMarket.Core;

public class ItemMgr
{

    private ItemMgr()
    {
        ItemList = Enum.GetValues(typeof(ItemType))
            .Cast<ItemType>()
            .Select(t => new ItemTypeModel
            {
                Key = t.ToString(),
                Name = t.ToString().Replace("_"," "),
                Id = (int)t,
            }).ToList();
    }
	public static ItemMgr This
	{
		get
		{
			Instance ??= new();
			return Instance;
		}
	}
	private static ItemMgr? Instance;

    public IReadOnlyCollection<ItemTypeModel> ItemList;

    public ItemTypeModel? GetItemTypeName(int type)
    {
       return ItemList.FirstOrDefault(t => t.Id == type);
    }
    public ItemTypeModel? GetItemTypeName(string key)
    {
        return ItemList.FirstOrDefault(t => t.Key == key);
    }

}